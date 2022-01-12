namespace ChatApp.Api.Models;

/// <summary>
///		Relationship of user with each other.
/// </summary>
/// <typeparam name="T">
///		The type used for the primary key for the model.
/// </typeparam>
public class Friend<T> where T : struct
{
	/// <summary>
	///		The id of user.
	/// </summary>
	public T UserId { set; get; }

	public User? User { set; get; }

	/// <summary>
	///		The id of user friend.
	/// </summary>
	public T FriendId { set; get; }

	public User? Friends { set; get; }
}

/// <summary>
///		Relationship of user with each other.
/// </summary>
public class Friends : Friend<Guid>
{ }