namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class GroupsRepository : Repository<Groups>
{
	protected DbSet<Groups> _groups;

	public GroupsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.Groups is null)
		{
			throw new NullReferenceException("Groups context is null");
		}

		this._groups = context.Groups;
	}

	public override Groups? GetById(Guid id)
	{
		return this._groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefault(o => o.Id == id);
	}

	public override async Task<Groups?> GetByIdAsync(Guid id)
	{
		return await this._groups
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(o => o.Id == id);
	}
}
