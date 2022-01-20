namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IGroupMembersRepository : IRepository<GroupMember, int>
{
	Task<IEnumerable<int>> GetGroupMembersAsync(int groupId);

	Task<IEnumerable<int>> GetGroupMembersAsync(Guid groupUniqueId);

	Task<bool> IsMemberExistAsync(int groupId, int userId);

	Task AddMemberAsync(int groupId, int userId);

	Task AddMemberRangeAsync(int groupId, IEnumerable<int> userIds);
}
