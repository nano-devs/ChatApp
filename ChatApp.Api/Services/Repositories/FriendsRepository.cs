namespace ChatApp.Api.Services.Repositories;

using System.Collections.Generic;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class FriendsRepository : Repository<Friend, Guid>, IFriendsRepository
{
	protected DbSet<Friend> _friends;

	public FriendsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.Friends is null)
		{
			throw new NullReferenceException("Friends context is null");
		}

		this._friends = context.Friends;
	}

	public async Task<IEnumerable<object>> GetFriendsAsync(int userId)
	{
		return await this._friends
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.UserId == userId)
			.Select(o => new { o.FriendId, o.Friends})
			.ToListAsync();
	}

	public async Task<bool> IsFriendExistAsync(int userId, int friendId)
	{
		return await this._friends
			.AsNoTrackingWithIdentityResolution()
			.AnyAsync(o => o.UserId == userId && o.FriendId == friendId);

		// alternative
		// return await this._context.Friends
		//	.AsNoTrackingWithIdentityResolution()
		//	.AnyAsync(o => (o.UserId == userId && o.FriendId == friendId) || 
		//				(o.UserId == friendId && o.FriendId == userId));
	}

	public async Task<bool> AddFriendshipAsync(int userId, int friendId)
	{
		try
		{
			var friend1 = new Friend
			{
				UserId = userId,
				FriendId = friendId
			};
			var friend2 = new Friend
			{
				UserId = friendId,
				FriendId = userId
			};

			await this.AddAsync(friend1);
			await this.AddAsync(friend2);
			return true;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task<bool> RemoveFriendshipAsync(int userId, int friendId)
	{
		try
		{
			var friend1 = new Friend
			{
				UserId = userId,
				FriendId = friendId
			};
			var friend2 = new Friend
			{
				UserId = friendId,
				FriendId = userId
			};

			await this.RemoveAsync(friend1);
			await this.RemoveAsync(friend2);
			return true;
		}
		catch (Exception)
		{
			throw;
		}
	}
}
