namespace ChatApp.Api.Services.Repositories;

using System.Collections.Generic;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class GroupMembersRepository : Repository<GroupMember, Guid>, IGroupMembersRepository
{
	protected DbSet<GroupMember> _groupMembers;

	public GroupMembersRepository(ChatAppDbContext context) : base(context)
	{
		if (context.GroupMembers is null)
		{
			throw new NullReferenceException("Group Members context is null");
		}

		this._groupMembers = context.GroupMembers;
	}

	public async Task<IEnumerable<Guid>> GetGroupMembersAsync(Guid groupId)
	{
		return await this._groupMembers
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.GroupId == groupId)
			.Select(o => o.UserId)
			.ToListAsync();
	}

	public async Task<bool> IsMemberExistAsync(Guid groupId, Guid userId)
	{
		return await this._groupMembers
			.AsNoTrackingWithIdentityResolution()
			.AnyAsync(o => o.GroupId == groupId && o.UserId == userId);
	}

	public async Task AddMemberAsync(Guid groupId, Guid userId)
	{
		await this._groupMembers.AddAsync(
			new GroupMember()
			{
				GroupId = groupId,
				UserId = userId
			});
	}

	public async Task AddMemberRangeAsync(Guid groupId, IEnumerable<Guid> userIds)
	{
		foreach (var userId in userIds)
		{
			await this.AddMemberAsync(groupId, userId);
		}
	}
}
