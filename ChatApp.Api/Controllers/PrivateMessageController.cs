namespace ChatApp.Api.Controllers;

using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class PrivateMessageController : ControllerBase
{
	protected IPrivateMessagesRepository _privateMessagesRepository;
	protected ChatAppDbContext _context;

	public PrivateMessageController(ChatAppDbContext context, IPrivateMessagesRepository privateMessagesRepository)
	{
		this._context = context;
		this._privateMessagesRepository = privateMessagesRepository;
	}

	[HttpPost]
	public async Task<object> AddChat(Guid userId, Guid friendId, string message)
	{
		if (message == null)
		{
			message = "";
		}

		var messageId = _context.Messages.Count() + 1;

		var msg = new Message
		{
			Id = messageId,
			PostedByUser = new User { UniqueGuid = userId },
			SentDateTime = DateTime.UtcNow,
			Content = message
		};

		var privateMessage = new PrivateMessage
		{
			Id = _context.PrivateMessages.Count() + 1,
			MessageId = messageId,
			SendToUser = new User { UniqueGuid = friendId }
		};

		try
		{
			await _context.Messages.AddAsync(msg);
			await this._privateMessagesRepository.AddAsync(privateMessage);
			await _privateMessagesRepository.SaveAsync();
			return "message saved temporary";
		}
		catch
		{
			return "Cant store message";
		}
	}

	[HttpGet]
	public async Task<object> GetChat(Guid userId, Guid friendId)
	{
		var messages = await _privateMessagesRepository
			.GetChatsFromFriendAsync(userId, friendId);

		if (messages.Any())
		{
			return messages;
		}
		else
		{
			return $"theres no stored message for { userId } and { friendId }";
		}
	}

	[HttpPost]
	public async Task<object> RemoveChat(Guid userId, Guid friendId)
	{
		try
		{
			await this._privateMessagesRepository.RemoveStoredChatAsync(userId, friendId);
			await this._privateMessagesRepository.SaveAsync();
			return Ok();
		}
		catch
		{
			return $"Failed to remove chats";
		}
	}
}
