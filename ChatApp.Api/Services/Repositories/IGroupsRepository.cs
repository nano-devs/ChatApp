namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public interface IGroupsRepository : IRepository<Group, int>
{
	Task<Group?> GetByUniqueIdAsync(Guid id);
}
