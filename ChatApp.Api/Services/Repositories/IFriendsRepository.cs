namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IFriendsRepository : IRepository<Friend, Guid>
{
	Task<IEnumerable<object>> GetFriendsAsync(int userId);

	Task<bool> IsFriendExistAsync(int userId, int friendId);

	Task<bool> AddFriendshipAsync(int userId, int friendId);

	Task<bool> RemoveFriendshipAsync(int userId, int friendId);

}
