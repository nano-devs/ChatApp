using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			try
			{
				var chat = new GroupChat
				{
					From = userId,
					GroupId = groupId,
					Message = message,
					Timestamp = DateTime.UtcNow,
					AlreadySendTo = userId.ToString()
				};
				await this._context.GroupChats.AddAsync(chat);
				await this._context.SaveChangesAsync();
				return "message saved temporary";
			}
			catch
			{
				return "Cant store message";
			}
		}

		/// <summary>
		/// Get all temporary chat
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="groupId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<object> GetChat(Guid userId, Guid groupId)
		{
			var records = this._context.GroupChats
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.GroupId == groupId);

			if (records.Any())
			{
				var chats = await records.ToListAsync();
				//chats.ForEach(o => o.AlreadySendTo += ";" + userId);
				
				foreach (var chat in chats)
				{
					if (!chat.AlreadySendTo.Split(';').Contains(userId.ToString()))
					{
						chat.AlreadySendTo += $";{ userId }";
					}
				}

				this._context.GroupChats.UpdateRange(chats);
				await this._context.SaveChangesAsync();
				return await records.Select(o => new { o.Id, o.From, o.Message, o.Timestamp }).ToListAsync();
			}
			else
			{
				return $"theres no stored message for { groupId }";
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
			var chats = this._context.GroupChats
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.GroupId == groupId);

			if (chats.Any())
			{
				var members = this._context.GroupMembers
					.AsNoTrackingWithIdentityResolution()
					.Where(o => o.GroupId == groupId)
					.Select(o => o.UserId);

				var memberIds = members.ToListAsync();

				var removed = new List<GroupChat>();

				foreach (var data in await chats.Select(o => new { o.Id, o.AlreadySendTo }).ToListAsync())
				{
					var sended = data.AlreadySendTo.Contains(';');
					if (sended)
					{
						var temp = data.AlreadySendTo.Split(';').Select(Guid.Parse).ToList();
						var isEqual = new HashSet<Guid>(await memberIds).SetEquals(temp);
						if (isEqual)
						{
							removed.Add(new GroupChat { Id = data.Id });
						}
					}
					else
					{
						break;
					}
				}

				this._context.GroupChats.RemoveRange(removed);
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
