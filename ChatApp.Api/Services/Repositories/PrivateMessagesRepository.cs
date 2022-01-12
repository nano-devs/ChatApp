namespace ChatApp.Api.Services.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

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
		try
		{
			var msg = await 
				(from message in this._context.Messages
				 join privateMessage in this._context.PrivateMessages on message.Id equals privateMessage.MessageId
				 where message.PostedByUser.UniqueGuid == userId &&
					   privateMessage.SendToUser.UniqueGuid == friendId
				 select new
				 {
					 message.Id,
					 message.PostedByUserId,
					 message.SentDateTime,
					 message.Content
				 })
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
		catch
		{
			throw;
		}
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
