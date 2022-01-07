namespace ChatApp.Api.Controllers;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Repositories;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public class PrivateChatController : ControllerBase
{
	protected IPrivateChatsRepository _privateChatRepository;

	public PrivateChatController(IPrivateChatsRepository privateChatRepository)
	{
		_privateChatRepository = privateChatRepository;
	}

	/// <summary>
	/// Add temporary chat
	/// </summary>
	/// <remarks>
	///	store the message temporary that not received by friend.
	/// </remarks>
	/// <param name="userId"></param>
	/// <param name="friendId"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddChat(Guid userId, Guid friendId, string message)
	{
		PrivateChat? chat = new();
		var newId = Guid.Empty;

		while (chat != null)
		{
			newId = Guid.NewGuid();
			chat = await _privateChatRepository
				.GetByIdAsync(newId);
		}

		try
		{
			chat = new PrivateChat
			{
				Id = newId,
				From = userId,
				To = friendId,
				Message = message,
				Timestamp = DateTime.UtcNow
			};

			await _privateChatRepository.AddAsync(chat);
			await _privateChatRepository.SaveAsync();
			return "message saved temporary";
		}
		catch
		{
			return "Cant store message";
		}
	}

	/// <summary>
	/// Get all temporary chat
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="friendId"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> GetChat(Guid userId, Guid friendId)
	{
		var records = await _privateChatRepository
			.GetChatsFromFriendAsync(userId, friendId);

		if (records.Any())
		{
			return records;
		}
		else
		{
			return $"theres no stored message for { userId } and { friendId }";
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="friendId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemoveChat(Guid userId, Guid friendId)
	{
		try
		{
			await _privateChatRepository.RemoveStoredChatAsync(userId, friendId);
			await _privateChatRepository.SaveAsync();
			return Ok();
		}
		catch
		{
			return $"Failed to remove chats";
		}
	}
}
