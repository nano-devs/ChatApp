namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
///		Relationship of user and group
/// </summary>
public class GroupMember<T> where T : struct
{
	/// <summary>
	///		User who join the group
	/// </summary>
	[Required]
	public T UserId { set; get; }

	public User? User { set; get; }

	/// <summary>
	///		Group that the user join
	/// </summary>
	[Required]
	public T GroupId { set; get; }

	public Group? Group { set; get; }
}

/// <summary>
///		Relationship of user and group
/// </summary>
public class GroupMember : GroupMember<int>
{ }
