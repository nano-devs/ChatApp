namespace ChatApp.Api.Services.Repositories;

using System.Collections.Generic;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class GroupMembersRepository : Repository<GroupMember, int>, IGroupMembersRepository
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

	public async Task<IEnumerable<int>> GetGroupMembersAsync(int groupId)
	{
		return await this._groupMembers
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.GroupId == groupId)
			.Select(o => o.UserId)
			.ToListAsync();
	}

	public async Task<IEnumerable<int>> GetGroupMembersAsync(Guid groupUniqueId)
	{
		return await this._groupMembers
			.AsNoTrackingWithIdentityResolution()
			.Where(o => o.Group.UniqueGuid == groupUniqueId)
			.Select(o => o.UserId)
			.ToListAsync();
	}

	public async Task<bool> IsMemberExistAsync(int groupId, int userId)
	{
		return await this._groupMembers
			.AsNoTrackingWithIdentityResolution()
			.AnyAsync(o => o.GroupId == groupId && o.UserId == userId);
	}

	public async Task AddMemberAsync(int groupId, int userId)
	{
		await this._groupMembers.AddAsync(
			new GroupMember()
			{
				GroupId = groupId,
				UserId = userId
			});
	}

	public async Task AddMemberRangeAsync(int groupId, IEnumerable<int> userIds)
	{
		foreach (var userId in userIds)
		{
			await this.AddMemberAsync(groupId, userId);
		}
	}
}
