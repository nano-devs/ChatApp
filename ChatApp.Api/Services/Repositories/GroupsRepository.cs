namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class GroupsRepository : Repository<Group, int>, IGroupsRepository
{
	protected DbSet<Group> _groups;

	public GroupsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.Groups is null)
		{
			throw new NullReferenceException("Groups context is null");
		}

		this._groups = context.Groups;
	}

	public override Group? GetById(int id)
	{
		return this._groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefault(o => o.Id == id);
	}

	public override async Task<Group?> GetByIdAsync(int id)
	{
		return await this._groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(o => o.Id == id);
	}

	public async Task<Group?> GetByUniqueIdAsync(Guid uniqueId)
	{
		return await this._groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(o => o.UniqueGuid == uniqueId);
	}
}
