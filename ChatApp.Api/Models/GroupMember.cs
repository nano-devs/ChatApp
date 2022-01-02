namespace ChatApp.Api.Models;

/// <summary>
///		Relationship of user and group
/// </summary>
public class GroupMember<T> where T : struct
{
	/// <summary>
	///		User who join the group
	/// </summary>
	public T UserId { set; get; }

	/// <summary>
	///		Group that the user join
	/// </summary>
	public T GroupId { set; get; }
}

/// <summary>
///		Relationship of user and group
/// </summary>
public class GroupMember : GroupMember<Guid>
{ }
