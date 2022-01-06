namespace ChatApp.Api.Services.Repositories;

using System;
using System.Linq;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class PendingGroupChatsRepository : Repository<PendingGroupChat>, IPendingGroupChatsRepository
{
	protected DbSet<PendingGroupChat> _pendingGroupChats;

	public PendingGroupChatsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.PendingGroupChats is null)
		{
			throw new NullReferenceException("Pending Group Chat context is null");
		}

		this._pendingGroupChats = context.PendingGroupChats;
	}

	/// <summary>
	///		Get group chat that not received by user.
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public async Task<IEnumerable<Guid>> GetPendingChatAsync(Guid userId)
	{
		return await this._pendingGroupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.UserId == userId)
			.Select(o => o.GroupChatId)
			.ToListAsync();
	}

	public async Task<IEnumerable<Guid>> GetGroupChatThatHavePendingListAsync(IEnumerable<Guid> groupChatIds)
	{
		return await this._pendingGroupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o => !groupChatIds.Contains(o.GroupChatId))
			.Select(o => o.GroupChatId)
			.ToListAsync();
	}

}
