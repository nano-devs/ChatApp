namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class GroupsRepository : Repository<Group, int>
{
	protected DbSet<Group> _groups;

	public GroupsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.Groups is null)
		{
			throw new NullReferenceException("Groups context is null");
		}

		_groups = context.Groups;
	}

	public override Group? GetById(int id)
	{
		return _groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefault(o => o.Id == id);
	}

	public override async Task<Group?> GetByIdAsync(int id)
	{
		return await _groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(o => o.Id == id);
	}
}
