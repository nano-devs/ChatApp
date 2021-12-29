using System;

namespace NET6ChatAppServerApi.Models
{
	/// <summary>
	/// Store the Group chat message id and user id that 
	/// not received the message.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PendingGroupChat<T> where T : struct
	{
		/// <summary>
		///		Message that not received by group member.
		/// </summary>
		public T GroupChatId { set; get; }

		/// <summary>
		///		User that not received the message.
		/// </summary>
		public T UserId { set; get; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class PendingGroupChat : PendingGroupChat<Guid>
	{
	}
}
