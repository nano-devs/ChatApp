namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IPendingGroupChatsRepository : IRepository<PendingGroupChat, Guid>
{
	Task<IEnumerable<Guid>> GetPendingChatAsync(Guid userId);

	Task<IEnumerable<Guid>> GetGroupChatThatHavePendingListAsync(IEnumerable<Guid> groupChatIds);

}
