namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IGroupChatsRepository : IRepository<GroupChat>
{
	Task<IEnumerable<GroupChat>> GetGroupChatThatPendingAsync(Guid groupId, IEnumerable<Guid> pendingGroupChatIds);

	Task<IEnumerable<GroupChat>> GetGroupChatThatHaveNoPendingAsync(IEnumerable<Guid> pendingGroupChatIds);
}
