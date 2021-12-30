using ChatApp.API.Data;
using ChatApp.API.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FriendsController : ControllerBase
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
		if (this._context.Friends is null)
		{
			return "Friends context is null";
		}

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
		if (this._context.Friends is null)
		{
			return "Friends context is null";
		}

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
				return this.Ok();
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
		if (this._context.Friends is null)
		{
			return "Friends context is null";
		}

		var friend = this._context.Friends
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.UserId == userId && o.FriendId == friendId);

		if (friend.Any())
		{
			try
			{
				this._context.Friends.Remove(await friend.FirstAsync());
				await this._context.SaveChangesAsync();
				return this.Ok();
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
