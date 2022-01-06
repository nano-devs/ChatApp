namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Models;

public interface IFriendsRepository : IRepository<Friends>
{
	Task<IEnumerable<Guid>> GetFriendsAsync(Guid userId);

	Task<bool> IsFriendExistAsync(Guid userId, Guid friendId);

	Task<bool> AddFriendshipAsync(Guid userId, Guid friendId);

	Task<bool> RemoveFriendshipAsync(Guid userId, Guid friendId);
	
}
