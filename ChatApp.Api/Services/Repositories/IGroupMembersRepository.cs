namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IGroupMembersRepository : IRepository<GroupMember, Guid>
{
	Task<IEnumerable<Guid>> GetGroupMembersAsync(Guid groupId);

	Task<bool> IsMemberExistAsync(Guid groupId, Guid userId);

	Task AddMemberAsync(Guid groupId, Guid userId);

	Task AddMemberRangeAsync(Guid groupId, IEnumerable<Guid> userIds);
}
