namespace ChatApp.Api.Services.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;

using ChatApp.Api.Data;
using ChatApp.Api.Models;

using Microsoft.EntityFrameworkCore;

public class GroupChatsRepository : Repository<GroupChat, Guid>, IGroupChatsRepository
{
	protected DbSet<GroupChat> _groupChats;

	public GroupChatsRepository(ChatAppDbContext context) : base(context)
	{
		if (context.GroupChats is null)
		{
			throw new NullReferenceException("Group Chat context is null");
		}

		this._groupChats = context.GroupChats;
	}

	public async Task<IEnumerable<GroupChat>> GetGroupChatThatPendingAsync(Guid groupId, IEnumerable<Guid> pendingGroupChatIds)
	{
		return await this._groupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o =>
				pendingGroupChatIds.Contains(o.Id) &&
				o.GroupId == groupId)
			.ToListAsync();

		//from o in this._groupChats
		//where pendingGroupChatIds.Contains(o.Id) && o.GroupId == groupId
		//select o;
	}

	public async Task<IEnumerable<GroupChat>> GetGroupChatThatHaveNoPendingAsync(IEnumerable<Guid> pendingGroupChatIds)
	{
		return await this._groupChats
			.AsNoTrackingWithIdentityResolution()
			.Where(o => !pendingGroupChatIds.Contains(o.Id))
			.ToListAsync();
	}

}
