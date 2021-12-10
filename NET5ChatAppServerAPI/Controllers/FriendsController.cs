using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NET5ChatAppServerAPI.Data;
using NET5ChatAppServerAPI.Models;

namespace NET5ChatAppServerAPI.Controllers
{
	public class FriendsController : BaseApiController
	{
		private readonly ChatAppDbContext _context;

		public FriendsController(ChatAppDbContext context)
		{
			this._context = context;
		}

		/// <summary>
		///		Get all friends.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<object> Index()
		{
			var records = from o in this._context.Friends
						  select o;

			if (records.Any() == false)
			{
				return "no friends exist in database";
			}

			return await records.AsNoTrackingWithIdentityResolution().ToListAsync();
		}

		/// <summary>
		///		Get the user all friends
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<object> MyFriends(Guid userId)
		{
			var friends = this._context.Friends
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.UserId == userId)
				.Select(o => o.FriendId);
		
			if (friends.Any())
			{
				return await friends.ToListAsync();
			}
			else
			{
				return $"You ({ userId }) do not have a friend";
			}
		}

		/// <summary>
		///		Add new friends
		/// </summary>
		/// <param name="groupId"></param>
		/// <param name="userpId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<object> AddFriend(Guid userId, Guid friendId)
		{
			var exist = this._context.Friends
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.UserId == userId && o.FriendId == friendId);

			if (exist.Any())
			{
				return $"{ userId } already a friend { friendId }";
			}
			else
			{
				try
				{
					var friend = new Friends
					{
						UserId = userId,
						FriendId = friendId
					};

					await this._context.Friends.AddAsync(friend);
					await this._context.SaveChangesAsync();
					return Ok();
				}
				catch
				{
					return "Failed to add { friendId } to { userId }";
				}
			}
		}

		/// <summary>
		///		Remove friend
		/// </summary>
		/// <param name="groupId"></param>
		/// <param name="userpId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<object> RemoveFriend(Guid userId, Guid friendId)
		{
			var friend = this._context.Friends
							.AsNoTrackingWithIdentityResolution()
							.Where(o => o.UserId == userId && o.FriendId == friendId);

			if (friend.Any())
			{
				try
				{
					this._context.Friends.Remove(await friend.FirstOrDefaultAsync());
					await this._context.SaveChangesAsync();
					return Ok();
				}
				catch
				{
					return $"Failed to remove friend ({ friend }) from user ({ friendId })";
				}
			}
			else
			{
				return $"{ userId } do not have a friend { friendId }";
			}
		}
	}
}
