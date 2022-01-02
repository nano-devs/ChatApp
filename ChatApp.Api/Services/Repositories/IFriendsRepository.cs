namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IFriendsRepository : IRepository<Friends>
{
	IEnumerable<Guid> GetFriends(Guid userId);

	bool IsFriendExist(Guid userId, Guid friendId);

	Task<bool> AddFriendshipAsync(Guid userId, Guid friendId);

	Task<bool> RemoveFriendshipAsync(Guid userId, Guid friendId);
	
}
