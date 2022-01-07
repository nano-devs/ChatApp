namespace ChatApp.Api.Services.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class PrivateChatsRepository : Repository<PrivateChat>, IPrivateChatsRepository
{
	protected DbSet<PrivateChat> _privateChats;

	public PrivateChatsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.PrivateChats is null)
		{
			throw new NullReferenceException("Private Chat context is null");
		}

		this._privateChats = context.PrivateChats;
	}

	public async Task<IEnumerable<PrivateChat>> GetChatsFromFriendAsync(Guid userId, Guid friendId)
	{
		try
		{
			return await this._privateChats
				.AsNoTrackingWithIdentityResolution()
				.Where(o => (o.From == friendId && o.To == userId))
				.ToListAsync();
		}
		catch
		{
			throw;
		}
	}

	public async Task RemoveStoredChatAsync(Guid userId, Guid friendId)
	{
		try
		{
			var chats = await this._privateChats
			   .AsNoTrackingWithIdentityResolution()
			   .Where(o => (o.From == friendId && o.To == userId))
			   .ToListAsync();

			if (chats.Any())
			{
				await this.RemoveRangeAsync(chats);
			}
		}
		catch
		{
			throw;
		}
	}
}
