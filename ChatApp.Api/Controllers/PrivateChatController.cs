namespace ChatApp.API.Controllers;

using System.Data;

using ChatApp.API.Data;
using ChatApp.API.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]/[action]")]
public class PrivateChatController : ControllerBase
{
	private readonly ChatAppDbContext _context;

	public PrivateChatController(ChatAppDbContext context)
	{
		this._context = context;
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
		if (this._context.PrivateChats is null)
		{
			return "Private Chat context is null";
		}

		PrivateChat? chat = null;
		var newId = Guid.Empty;

		while (chat != null)
		{
			newId = Guid.NewGuid();
			chat = await this._context.PrivateChats
				.AsNoTrackingWithIdentityResolution()
				.FirstOrDefaultAsync(o => o.Id == newId);
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

			await this._context.PrivateChats.AddAsync(chat);
			await this._context.SaveChangesAsync();
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
		if (this._context.PrivateChats is null)
		{
			return "Private Chat context is null";
		}

		var records = this._context.PrivateChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o =>
				(o.From == userId && o.To == friendId) ||
				(o.From == friendId && o.To == userId));

		if (records.Any())
		{
			return await records.ToListAsync();
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
		if (this._context.PrivateChats is null)
		{
			return "Private Chat context is null";
		}

		var records = this._context.PrivateChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o =>
				(o.From == userId && o.To == friendId) ||
				(o.From == friendId && o.To == userId));

		if (records.Any())
		{
			this._context.PrivateChats.RemoveRange(await records.ToListAsync());
			await this._context.SaveChangesAsync();
			return this.Ok();
		}
		else
		{
			return $"theres no chat to remove";
		}
	}
}
