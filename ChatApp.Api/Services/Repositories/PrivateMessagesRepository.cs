namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PrivateMessagesRepository : Repository<PrivateMessage, int>, IPrivateMessagesRepository
{
	protected DbSet<PrivateMessage> _privateMessagesRepository;

	public PrivateMessagesRepository(ChatAppDbContext context) : base(context)
	{
		if (context.PrivateMessages is null)
		{
			throw new NullReferenceException("Private Messages context is null");
		}
		
		this._privateMessagesRepository = context.PrivateMessages;
	}

	public async Task<IEnumerable<object>> GetChatsFromFriendAsync(Guid userId, Guid friendId)
	{
		var msg = await _context.PrivateMessages!
			.Where(privateMessage => 
				(privateMessage.SendToUser!.UniqueGuid == friendId && privateMessage.Message!.PostedByUser!.UniqueGuid == userId) ||
				(privateMessage.SendToUser!.UniqueGuid == userId && privateMessage.Message!.PostedByUser!.UniqueGuid == friendId)
				)
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();

		if (msg.Any())
		{
			return msg;
		}

		return new List<string>(){
			$"There's no message for user { userId } from user { friendId }"
		};
	}

	public async Task RemoveStoredChatAsync(Guid userId, Guid friendId)
	{
		try
		{
			var messages = await this._context.Messages
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.PostedByUser.UniqueGuid == userId)
				.ToListAsync();
			
			var privateMessages = await this._privateMessagesRepository
				.AsNoTrackingWithIdentityResolution()
				.Where(o => o.SendToUser.UniqueGuid == friendId)
				.ToListAsync();

			if (messages.Any())
			{
				this._context.Messages.RemoveRange(messages);
			}

			if (privateMessages.Any())
			{
				await this.RemoveRangeAsync(privateMessages);
			}

			await this._context.SaveChangesAsync();
		}
		catch
		{
			throw;
		}
	}
}
