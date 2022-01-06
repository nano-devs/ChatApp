namespace ChatApp.Api.Controllers;

using System.Data;

using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]/[action]")]
public class GroupChatController : ControllerBase
{
	private const char _Delimeter = ';';
	protected IGroupChatsRepository _groupChatRepository;
	protected IGroupMembersRepository _groupMembersRepository;
	protected IPendingGroupChatsRepository _pendingGroupChatsRepository;

	public GroupChatController(IGroupChatsRepository groupChatRepository, IGroupMembersRepository groupMembersRepository, IPendingGroupChatsRepository pendingGroupChatsRepository)
	{
		this._groupChatRepository = groupChatRepository;
		this._groupMembersRepository = groupMembersRepository;
		this._pendingGroupChatsRepository = pendingGroupChatsRepository;
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
		GroupChat? groupChat = new();
		var newGroupChatId = Guid.Empty;

		while (groupChat != null)
		{
			newGroupChatId = Guid.NewGuid();

			groupChat = await this._groupChatRepository
				.GetByIdAsync(newGroupChatId);
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
			await this._groupChatRepository.AddAsync(groupChat);
			await this._groupChatRepository.SaveAsync();
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
		try
		{
			var groupChatId = (Guid)await this.AddChat(userId, groupId, message);

			// add message/chat to PendingGroupChat for all member
			// TODO: only member that offline

			var groupMembers = (IList<Guid>)await this._groupMembersRepository
				.GetGroupMembersAsync(groupId);
			
			groupMembers.Remove(userId);

			return await this.AddPendingChat(groupChatId, groupMembers);
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
	public async Task<object> AddPendingChat(Guid groupChatId, string userId)
	{
		var ids = userId.Contains(_Delimeter) ?
				userId.Split(_Delimeter).Select(Guid.Parse).ToList() :
				new List<Guid> { Guid.Parse(userId) };

		return await this.AddPendingChat(groupChatId, ids);
	}
	
	/// <summary>
	/// Add not received message/chat into PendingGroupChat 
	/// </summary>
	/// <param name="groupChatId"></param>
	/// <param name="userId">User that not received the message/chat</param>
	/// <returns></returns>
	protected async Task<object> AddPendingChat(Guid groupChatId, IList<Guid> userIds)
	{
		try
		{
			var pendingGroupChat = new PendingGroupChat[userIds.Count];

			for (var i = 0; i < pendingGroupChat.Length; i++)
			{
				pendingGroupChat[i] = new PendingGroupChat
				{
					GroupChatId = groupChatId,
					UserId = userIds[i]
				};
			}

			await this._pendingGroupChatsRepository.AddRangeAsync(pendingGroupChat);
			await this._pendingGroupChatsRepository.SaveAsync();
			return "message saved temporary";
		}
		catch
		{
			return "Can't store pending chat message";
		}
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

			var pendingChatIds = await this._pendingGroupChatsRepository
				.GetPendingChatAsync(userId);

			var chats = await this._groupChatRepository
				.GetGroupChatThatPendingAsync(groupId, pendingChatIds);
			
			if (chats.Any())
			{
				return chats;
			}

			return "No pending chat.";
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
	protected async Task<object> RemovePendingChat(Guid userId, IList<Guid> groupChatId)
	{
		try
		{
			//var pendingGroupChat = new PendingGroupChat[groupChatId.Count];
			var pendingGroupChat = new List<PendingGroupChat>();

			foreach (var chatId in groupChatId)
			{
				pendingGroupChat.Add(
					new PendingGroupChat()
					{
						UserId = userId,
						GroupChatId = chatId
					});
			}

			await this._pendingGroupChatsRepository.RemoveRangeAsync(pendingGroupChat);
			await this._pendingGroupChatsRepository.SaveAsync();
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
	public async Task<object> RemovePendingChat(Guid userId, string groupChatId)
	{
		var ids = groupChatId.Contains(_Delimeter) ?
				  groupChatId.Split(_Delimeter).Select(Guid.Parse).ToList() :
				  new List<Guid> { Guid.Parse(groupChatId) };

		return await this.RemovePendingChat(userId, ids);
	}

	/// <summary>
	///	
	/// </summary>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemoveChat(Guid groupId)
	{
		/* 
		DELETE FROM GroupChats 
		WHERE GroupId == x AND 
			  Id not in (SELECT GroupChatId FROM PendingGroupChats);
		 */
		// TODO: simplified version for delete group chat

		// get chat list from group
		
		var chats = await this._groupChatRepository.
			FindAsync(o=>o.Id == groupId);

		//var chats = await this._context.GroupChats
		//	.AsNoTrackingWithIdentityResolution()
		//	.Where(o => o.Id == groupId)
		//	.Select(o => o.Id)
		//	.ToListAsync();

		// get GroupChatId that have pending list
		//var pending = await this._context.PendingGroupChats
		//	.AsNoTrackingWithIdentityResolution()
		//	.Where(o => !chats.Contains(o.GroupChatId))
		//	.Select(o => o.GroupChatId).ToListAsync();

		var pending = await this._pendingGroupChatsRepository
			.GetGroupChatThatHavePendingListAsync(chats.Select(o => o.Id));

		// get group chat list that do not have pending list
		//var removed = this._groupChatRepositoryGroupChats
		//	.AsNoTrackingWithIdentityResolution()
		//	.Where(o => !pending.Contains(o.Id));

		var removed = await this._groupChatRepository
			.GetGroupChatThatHaveNoPendingAsync(pending);

		if (removed.Any())
		{
			await this._groupChatRepository.RemoveRangeAsync(removed);
			await this._groupChatRepository.SaveAsync();
			return this.Ok();
		}
		else
		{
			return $"theres no chat to remove";
		}
	}
}
