namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IPrivateChatsRepository : IRepository<PrivateChat>
{
	Task<IEnumerable<PrivateChat>> GetChatsFromFriendAsync(Guid userId, Guid friendId);

	Task RemoveStoredChatAsync(Guid userId, Guid friendId);
}
