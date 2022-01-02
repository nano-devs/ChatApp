namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
///		Temporary store chat that can't directly send to group.
/// </summary>
/// <remarks>
///		Removed after all recipient receive the chat or deleted before all recepient get the chat.
/// </remarks>
/// <typeparam name="T">
///		The type used for the primary key for the model.
/// </typeparam>
public class GroupChat<T> where T : struct
{
	/// <summary>
	///		Universally unique identifier for each records.
	/// </summary>
	/// <remarks>
	///		Primary Key for database table.
	/// </remarks>
	public T Id { set; get; }

	/// <summary>
	///		The user id who send the chat or message.
	/// </summary>
	public T From { set; get; }

	/// <summary>
	///		The group id that recieve the chat or message.
	/// </summary>
	public T GroupId { set; get; }

	/// <summary>
	///		The content of the chat.
	/// </summary>
	public string? Message { set; get; }

	/// <summary>
	///		Date and time the message or chat is created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "dd-MM-yyyy HH:mm:ss")]
	public DateTime Timestamp { set; get; } = DateTime.UtcNow;
}

/// <summary>
///		Relationship of user with each other.
/// </summary>
public class GroupChat : GroupChat<Guid>
{ }