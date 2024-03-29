﻿namespace ChatApp.Api.Controllers;
using ChatApp.Api.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(AuthenticationSchemes = "jwt")]
[ApiController]
[Route("[controller]/[action]")]
public class FriendController : ControllerBase
{
	protected IFriendsRepository _friendsRepository;

	public FriendController(IFriendsRepository friendsRepository)
	{
		this._friendsRepository = friendsRepository;
	}

	/// <summary>
	///		Get all friends.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> Index()
	{
		var friends = await this._friendsRepository.GetAllAsync();

		if (friends.Any())
		{
			return friends;
		}

		return "no friends exist in database";
	}

	/// <summary>
	///		Get the user all friends
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> MyFriends(int userId)
	{
		if (this._friendsRepository is null)
		{
			return "Friends context is null";
		}

		var friends = await this._friendsRepository.GetFriendsAsync(userId);

		if (friends.Any())
		{
			return friends;
		}
		else
		{
			return $"You ({ userId }) do not have a friend";
		}
	}

	/// <summary>
	///		Add new friends
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="userpId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddFriend(int userId, int friendId)
	{
		if (this._friendsRepository is null)
		{
			return "Friends context is null";
		}

		if (await this._friendsRepository.IsFriendExistAsync(userId, friendId))
		{
			return $"{ userId } already a friend { friendId }";
		}

		try
		{
			var success = await this._friendsRepository.AddFriendshipAsync(userId, friendId);
			if (success)
			{
				await this._friendsRepository.SaveAsync();
				return this.Ok();
			}

			return "Failed to add { friendId } for { userId }";
		}
		catch
		{
			return "Failed to add { friendId } for { userId }";
		}
	}

	/// <summary>
	///		Remove friend
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="userpId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemoveFriend(int userId, int friendId)
	{
		if (this._friendsRepository is null)
		{
			return "Friends context is null";
		}

		if (!await this._friendsRepository.IsFriendExistAsync(userId, friendId))
		{
			return $"{ userId } do not have a friend { friendId }";

		}

		try
		{
			var success = await this._friendsRepository.RemoveFriendshipAsync(userId, friendId);
			if (success)
			{
				await this._friendsRepository.SaveAsync();
				return this.Ok();
			}

			return $"Failed to remove friend ({ friendId }) from user ({ userId })";
		}
		catch
		{
			return $"Failed to remove friend ({ friendId }) from user ({ userId })";
		}
	}
}
