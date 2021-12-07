using System;
using System.ComponentModel.DataAnnotations;

namespace NET5ChatAppServerAPI.Models
{
	/// <summary>
	///		Temporary store chat that can't directly send.
	/// </summary>
	/// <remarks>
	///		Removed after the recipient receive the chat or delete before the recepient get the chat.
	/// </remarks>
	/// <typeparam name="T">
	///		The type used for the primary key for the model.
	/// </typeparam>
	public class PrivateChat<T> : BaseModel<T>
		where T : struct
	{
		/// <summary>
		///		The user id who send the chat or message.
		/// </summary>
		public T From { set; get; }
		
		/// <summary>
		///		The user id that recieve the chat or message.
		/// </summary>
		public T To { set; get; }

		/// <summary>
		///		The content of the chat.
		/// </summary>
		public string Message { set; get; }

		/// <summary>
		///		Date and time the message or chat is created.
		/// </summary>
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "dd-MM-yyyy HH:mm:ss")]
		public DateTime Timestamp { set; get; } = DateTime.UtcNow;
	}

	/// <summary>
	///		Temporary store chat that can't directly send.
	/// </summary>
	/// <remarks>
	///		Removed after the recipient receive the chat or delete before the recepient get the chat.
	/// </remarks>
	public class PrivateChat : PrivateChat<Guid>
	{

	}
}
