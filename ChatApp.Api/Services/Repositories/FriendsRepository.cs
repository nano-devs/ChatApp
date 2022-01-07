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

	public async Task<IEnumerable<Guid>> GetFriendsAsync(Guid userId)
	{
		return await this._friends
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.UserId == userId)
			.Select(o => o.FriendId)
			.ToListAsync();
	}

	public async Task<bool> IsFriendExistAsync(Guid userId, Guid friendId)
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
