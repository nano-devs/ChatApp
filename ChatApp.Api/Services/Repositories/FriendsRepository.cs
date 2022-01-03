namespace ChatApp.Api.Services.Repositories;

using System.Collections.Generic;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class FriendsRepository : Repository<Friends>, IFriendsRepository
{
	protected DbSet<Friends> _friends;

	public FriendsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.Friends is null)
		{
			throw new NullReferenceException("Friends context is null");
		}

		this._friends = context.Friends;
	}

	public IEnumerable<Guid> GetFriends(Guid userId)
	{
		return this._friends
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.UserId == userId)
			.Select(o => o.FriendId);
	}

	public bool IsFriendExist(Guid userId, Guid friendId)
	{
		var friends = this._friends
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.UserId == userId && o.FriendId == friendId);

		// alternative
		//var friends = this._context.Friends
		//	.AsNoTrackingWithIdentityResolution()
		//	.Where(o => (o.UserId == userId && o.FriendId == friendId) || 
		//				(o.UserId == friendId && o.FriendId == userId));

		return friends.Any();
	}

	public async Task<bool> AddFriendshipAsync(Guid userId, Guid friendId)
	{
		try
		{
			var friend1 = new Friends
			{
				UserId = userId,
				FriendId = friendId
			};
			var friend2 = new Friends
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

	public async Task<bool> RemoveFriendshipAsync(Guid userId, Guid friendId)
	{
		try
		{
			var friend1 = new Friends
			{
				UserId = userId,
				FriendId = friendId
			};
			var friend2 = new Friends
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
