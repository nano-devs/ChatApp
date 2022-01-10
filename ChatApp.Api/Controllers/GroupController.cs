namespace ChatApp.Api.Controllers;

using ChatApp.Api.Models;
using ChatApp.Api.Services.Repositories;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public class GroupController : ControllerBase
{
	protected IRepository<Group, Guid> _groupsRepository;
	protected IGroupMembersRepository _groupMembersRepository;

	public GroupController(GroupsRepository groupsRepository, IGroupMembersRepository groupMembersRepository)
	{
		this._groupsRepository = groupsRepository;
		this._groupMembersRepository = groupMembersRepository;
	}

	/// <summary>
	///		Get all group.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> Index()
	{
		var groups = await this._groupsRepository.GetAllAsync();

		if (groups.Any())
		{
			return groups;
		}

		return "Theres no groups available in database";
	}

	/// <summary>
	///		Get details of specific group.
	/// </summary>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> Details(Guid groupId)
	{
		if (this._groupsRepository is null)
		{
			return "Group context is null";
		}

		var group = await this._groupsRepository.GetByIdAsync(groupId);

		if (group == null)
		{
			return this.NotFound();
		}

		return group;
	}

	/// <summary>
	///		Create a new group
	/// </summary>
	/// <param name="name">Group name.</param>
	/// <param name="userId">The user id that create the group.</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<Guid> Create(string name, Guid userId)
	{
		Group? group = new Group();
		var newGroupId = Guid.Empty;

		while (group != null)
		{
			newGroupId = Guid.NewGuid();
			group = await this._groupsRepository.GetByIdAsync(newGroupId);
		}

		try
		{
			await this._groupsRepository.AddAsync(
				new Group() { Id = newGroupId, Name = name });

			await this._groupMembersRepository.AddAsync(
				new GroupMember() { GroupId = newGroupId, UserId = userId });

			await this._groupsRepository.SaveAsync();

			return newGroupId;
		}
		catch
		{
			return Guid.Empty;
		}
	}

	/// <summary>
	///		Get all member in specific group.
	/// </summary>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> GetGroupMember(Guid groupId)
	{
		var groupMembers = await this._groupMembersRepository.GetGroupMembersAsync(groupId);

		if (groupMembers.Any())
		{
			return groupMembers;
		}

		return $"No member in { groupId }";
	}

	[HttpPost]
	public async Task<object> Edit(Guid groupId, string name)
	{
		return await this.ChangeName(groupId, name);
	}

	/// <summary>
	///		Change group name
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> ChangeName(Guid groupId, string name)
	{
		var group = await this._groupsRepository.GetByIdAsync(groupId);

		if (group == null)
		{
			return this.NotFound();
		}

		try
		{
			group.Name = name;
			await this._groupsRepository.UpdateAsync(group);
			await this._groupsRepository.SaveAsync();
			return this.Ok();
		}
		catch
		{
			return "Failed to change group name";
		}
	}

	/// <summary>
	///		Delete the group
	/// </summary>
	/// <param name="groupId"></param>
	/// <remarks>All member or user that still in the group automatically removed.</remarks>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> Delete(Guid groupId)
	{
		var group = await this._groupsRepository.GetByIdAsync(groupId);

		if (group == null)
		{
			return this.NotFound();
		}

		try
		{
			await this._groupsRepository.RemoveAsync(group);
			await this._groupsRepository.SaveAsync();

			return groupId;
		}
		catch
		{
			return "Failed to delete group";
		}
	}

	/// <summary>
	///		Add new member into group
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="userpId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> AddMember(Guid groupId, Guid userId)
	{
		if (this._groupsRepository is null)
		{
			return "Group Members context is null";
		}

		var exist = await this._groupMembersRepository.IsMemberExistAsync(groupId, userId);

		if (exist)
		{
			return $"{ userId } already a member of group { groupId }";
		}

		try
		{
			await this._groupMembersRepository.AddMemberAsync(groupId, userId);
			await this._groupMembersRepository.SaveAsync();

			return this.Ok();
		}
		catch
		{
			return $"Failed to add { userId } as a member of group { groupId }";
		}
	}

	/// <summary>
	///		Remove a user from the group
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="userpId"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<object> RemoveMember(Guid groupId, Guid userId)
	{
		var exist = await this._groupMembersRepository.IsMemberExistAsync(groupId, userId);

		if (exist)
		{
			return $"{ userId } is not a member of group { groupId }";
		}

		try
		{
			await this._groupMembersRepository.AddMemberAsync(groupId, userId);
			await this._groupMembersRepository.SaveAsync();

			return this.Ok();
		}
		catch
		{
			return $"Failed to remove { userId } from group { groupId }";
		}
	}
}
