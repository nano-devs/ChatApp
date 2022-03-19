namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IPrivateMessagesRepository : IRepository<PrivateMessage, int>
{
	Task<IEnumerable<object>> GetChatsFromFriendAsync(Guid userId, Guid friendId);

	Task RemoveStoredChatAsync(Guid userId, Guid friendId);
}
