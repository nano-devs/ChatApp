using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using NET5ChatAppServerAPI.Data;
using NET5ChatAppServerAPI.Models;

namespace NET5ChatAppServerAPI.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class GroupChatController : ControllerBase
	{
		private readonly ChatAppDbContext _context;

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
			GroupChat groupChat = null;
			var newGroupChatId = Guid.Empty;

			do
			{
				newGroupChatId = Guid.NewGuid();

				groupChat = await this._context.GroupChats
					.AsNoTrackingWithIdentityResolution()
					.FirstOrDefaultAsync(o => o.Id == newGroupChatId);
			} while (groupChat != null);

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

				var pendingGroupChat = new PendingGroupChat[groupMembers.Count];

				for (var i = 0; i < pendingGroupChat.Length; i++)
				{
					pendingGroupChat[i].GroupChatId = groupChatId;
					pendingGroupChat[i].UserId = groupMembers[i];
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
		/// <param name="userId">User that not received the message/chat</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<object> AddPendingChat(Guid groupChatId, params Guid[] userId)
		{
			try
			{
				var pendingGroupChat = new PendingGroupChat[userId.Length];

				for (var i = 0; i < pendingGroupChat.Length; i++)
				{
					pendingGroupChat[i].GroupChatId = groupChatId;
					pendingGroupChat[i].UserId = userId[i];
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
			var ids = userId.Contains(';') ?
					userId.Split(';').Select(Guid.Parse).ToList() :
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
			try
			{
				var pendingGroupChat = new PendingGroupChat[groupChatId.Length];

				for (var i = 0; i < pendingGroupChat.Length; i++)
				{
					pendingGroupChat[i].GroupChatId = groupChatId[i];
					pendingGroupChat[i].UserId = userId;
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
}
