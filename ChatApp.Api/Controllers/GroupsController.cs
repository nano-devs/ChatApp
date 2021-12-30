using ChatApp.API.Data;
using ChatApp.API.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GroupsController : ControllerBase
{
	private readonly ChatAppDbContext _context;

	public GroupsController(ChatAppDbContext context)
	{
		this._context = context;
	}

	/// <summary>
	///		Get all group.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> Index()
	{
		var records = from o in this._context.Groups
					  select new { o.Id, o.Name };

		if (records.Any() == false)
		{
			return "not found";
		}

		return await records.AsNoTrackingWithIdentityResolution().ToListAsync();
	}

	/// <summary>
	///		Get details of specific group.
	/// </summary>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> Details(Guid groupId)
	{
		if (this._context.Groups is null)
		{
			return "Group context is null";
		}

		var group = await this._context.Groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(m => m.Id == groupId);

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
		if (this._context.Groups is null)
		{
			return Guid.Empty;
		}

		if (this._context.GroupMembers is null)
		{
			return Guid.Empty;
		}

		Groups? group = null;
		var newGroupId = Guid.Empty;

		while (group != null)
		{
			newGroupId = Guid.NewGuid();
			group = await this._context.Groups
				.AsNoTrackingWithIdentityResolution()
				.FirstOrDefaultAsync(o => o.Id == newGroupId);
		}

		group = new Groups
		{
			Id = newGroupId,
			Name = name
		};

		try
		{
			await this._context.Groups.AddAsync(group);

			// create GroupMember after adding into database
			// to minimize concurency conflict for creating new Group
			var member = new GroupMember
			{
				GroupId = group.Id,
				UserId = userId
			};

			await this._context.GroupMembers.AddAsync(member);
			await this._context.SaveChangesAsync();
			return group.Id;
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
		if (this._context.GroupMembers is null)
		{
			return "Group Members context is null";
		}

		var records = this._context.GroupMembers
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.GroupId == groupId)
			.Select(o => o.UserId);

		return await records.ToListAsync();
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
		if (this._context.Groups is null)
		{
			return "Group context is null";
		}

		var group = await this._context.Groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(o => o.Id == groupId);

		if (group != null)
		{
			try
			{
				group.Name = name;
				this._context.Groups.Update(group);
				await this._context.SaveChangesAsync();
				return this.Ok();
			}
			catch
			{
				return "Failed to change group name";
			}
		}
		else
		{
			return this.NotFound();
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
		if (this._context.Groups is null)
		{
			return "Group context is null";
		}

		var group = await this._context.Groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(o => o.Id == groupId);

		if (group != null)
		{
			try
			{
				this._context.Remove(group);
				await this._context.SaveChangesAsync();
				return groupId;
			}
			catch
			{
				return "Failed to delete group";
			}
		}
		else
		{
			return this.NotFound();
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
		if (this._context.GroupMembers is null)
		{
			return "Group Members context is null";
		}

		var exist = this._context.GroupMembers
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.GroupId == groupId && o.UserId == userId);

		if (exist.Any())
		{
			return $"{ userId } already a member of group { groupId }";
		}
		else
		{
			var member = new GroupMember
			{
				GroupId = groupId,
				UserId = userId
			};

			await this._context.GroupMembers.AddAsync(member);
			await this._context.SaveChangesAsync();
			return this.Ok();
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
		if (this._context.GroupMembers is null)
		{
			return "Group Members context is null";
		}

		var member = this._context.GroupMembers
						.AsNoTrackingWithIdentityResolution()
						.Where(o => o.GroupId == groupId && o.UserId == userId);

		if (member.Any())
		{
			this._context.GroupMembers.Remove(await member.FirstAsync());
			await this._context.SaveChangesAsync();
			return this.Ok();
		}
		else
		{
			return $"{ userId } not a member of group { groupId }";
		}
	}
}
