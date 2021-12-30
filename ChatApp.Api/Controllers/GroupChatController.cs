using System.Data;

using ChatApp.API.Data;
using ChatApp.API.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GroupChatController : ControllerBase
{
	private readonly ChatAppDbContext _context;
	private const char _Delimeter = ';';

	public GroupChatController(ChatAppDbContext context)
	{
		this._context = context;
	}

	/// <summary>
	/// Add temporary chat
	/// </summary>
	/// <remarks>
	///	store the message temporary that not received by other member.
	/// </remarks>
	/// <param name="userId"></param>
	/// <param name="groupId"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddChat(Guid userId, Guid groupId, string message)
	{
		if (this._context.GroupChats is null)
		{
			return "Group Chat context is null";
		}

		GroupChat? groupChat = null;
		var newGroupChatId = Guid.Empty;

		while (groupChat != null)
		{
			newGroupChatId = Guid.NewGuid();

			groupChat = await this._context.GroupChats
				.AsNoTrackingWithIdentityResolution()
				.FirstOrDefaultAsync(o => o.Id == newGroupChatId);
		}

		try
		{
			groupChat = new GroupChat
			{
				Id = newGroupChatId,
				From = userId,
				GroupId = groupId,
				Message = message,
				Timestamp = DateTime.UtcNow
			};
			await this._context.GroupChats.AddAsync(groupChat);
			await this._context.SaveChangesAsync();
			return newGroupChatId;
		}
		catch
		{
			return "Cant store message";
		}
	}

	/// <summary>
	/// Add temporary chat
	/// </summary>
	/// <remarks>
	///	store the message temporary that not received by other member
	///	into PendingChatGroup.
	/// </remarks>
	/// <param name="userId"></param>
	/// <param name="groupId"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddChatV2(Guid userId, Guid groupId, string message)
	{
		if (this._context.GroupMembers is null)
		{
			return "Group Member context is null";
		}

		try
		{
			var groupChatId = (Guid)await this.AddChat(userId, groupId, message);

			// add message/chat to PendingGroupChat for all member
			// TODO: only member that offline

			var groupMembers = await this._context.GroupMembers
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.GroupId == groupId && o.UserId != userId)
				.Select(o => o.UserId)
				.ToListAsync();

			return await this.AddPendingChat(groupChatId, groupMembers.ToArray());
		}
		catch
		{
			return "Cant store message";
		}
	}

	/// <summary>
	/// Add not received message/chat into PendingGroupChat 
	/// </summary>
	/// <param name="groupChatId"></param>
	/// <param name="userId">User that not received the message/chat</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddPendingChat(Guid groupChatId, params Guid[] userId)
	{
		if (this._context.PendingGroupChats is null)
		{
			return "Pending Group Chat Chat context is null";
		}

		try
		{
			var pendingGroupChat = new PendingGroupChat[userId.Length];

			for (var i = 0; i < pendingGroupChat.Length; i++)
			{
				pendingGroupChat[i] = new PendingGroupChat
				{
					GroupChatId = groupChatId,
					UserId = userId[i]
				};
			}

			await this._context.PendingGroupChats.AddRangeAsync(pendingGroupChat);
			await this._context.SaveChangesAsync();
			return "message saved temporary";
		}
		catch
		{
			return "Cant store message";
		}
	}

	/// <summary>
	/// Add not received message/chat into PendingGroupChat 
	/// </summary>
	/// <param name="groupChatId"></param>
	/// <param name="userId">User that not received the message/chat, delimiter ';'</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddPendingChatV2(Guid groupChatId, string userId)
	{
		var ids = userId.Contains(_Delimeter) ?
				userId.Split(_Delimeter).Select(Guid.Parse).ToList() :
				new List<Guid> { Guid.Parse(userId) };

		return await this.AddPendingChat(groupChatId, ids.ToArray());
	}

	/// <summary>
	/// Sync chat from PendingGroupChat
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> SyncChat(Guid userId, Guid groupId)
	{
		if (this._context.PendingGroupChats is null)
		{
			return "Pending Group Chat Chat context is null";
		}

		try
		{
			/* 
			SELECT *
			FROM GroupChats
			WHERE GroupChats.GroupId == x AND
			GroupChats.Id in
				(SELECT GroupChatId
				FROM PendingGroupChats
				WHERE UserId == x)
			*/
			// TODO: simplified query for sync chat

			var pendingChat = await this._context.PendingGroupChats
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.UserId == userId)
				.Select(o => o.GroupChatId)
				.ToListAsync();

			var chats = from o in this._context.GroupChats
						where pendingChat.Contains(o.Id) && o.GroupId == groupId
						select o;

			return await chats.ToListAsync();
		}
		catch
		{
			return "No pending message";
		}
	}

	/// <summary>
	/// Remove pending list from PendingChatGroup
	/// </summary>
	/// <param name="groupChatId"></param>
	/// <param name="userId">User that not received the message/chat</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemovePendingChat(Guid userId, params Guid[] groupChatId)
	{
		if (this._context.PendingGroupChats is null)
		{
			return "Pending Group Chat Chat context is null";
		}

		try
		{
			var pendingGroupChat = new PendingGroupChat[groupChatId.Length];

			for (var i = 0; i < pendingGroupChat.Length; i++)
			{
				pendingGroupChat[i] = new PendingGroupChat
				{
					GroupChatId = groupChatId[i],
					UserId = userId
				};
			}

			this._context.PendingGroupChats.RemoveRange(pendingGroupChat);
			await this._context.SaveChangesAsync();
			return $"some pending chat list for user { userId } is removed";
		}
		catch
		{
			return "No pending list to remove";
		}
	}

	/// <summary>
	/// Remove pending list from PendingChatGroup
	/// </summary>
	/// <param name="groupChatId"></param>
	/// <param name="userId">User that not received the message/chat</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemovePendingChatV2(Guid userId, string groupChatId)
	{
		var ids = groupChatId.Contains(_Delimeter) ?
				   groupChatId.Split(_Delimeter).Select(Guid.Parse).ToList() :
				   new List<Guid> { Guid.Parse(groupChatId) };

		return await this.RemovePendingChat(userId, ids.ToArray());
	}

	/// <summary>
	///	
	/// </summary>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemoveChat(Guid groupId)
	{
		if (this._context.PendingGroupChats is null)
		{
			return "Pending Group Chat Chat context is null";
		}

		if (this._context.GroupChats is null)
		{
			return "Group Chat Chat context is null";
		}

		/* 
		DELETE FROM GroupChats 
		WHERE GroupId == x AND 
			  Id not in (SELECT GroupChatId FROM PendingGroupChats);
		 */
		// TODO: simplified version for delete group chat

		// get chat list from group
		var chats = await this._context.GroupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.Id == groupId)
			.Select(o => o.Id)
			.ToListAsync();

		// get GroupChatId that have pending list
		var pending = await this._context.PendingGroupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o => !chats.Contains(o.GroupChatId))
			.Select(o => o.GroupChatId).ToListAsync();

		// get group chat list that do not have pending list
		var removed = this._context.GroupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o => !pending.Contains(o.Id));

		if (removed.Any())
		{
			this._context.GroupChats.RemoveRange(await removed.ToListAsync());
			await this._context.SaveChangesAsync();
			return this.Ok();
		}
		else
		{
			return $"theres no chat to remove";
		}
	}
}
