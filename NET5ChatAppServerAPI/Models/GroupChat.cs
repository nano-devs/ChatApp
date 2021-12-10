﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NET5ChatAppServerAPI.Models
{
	/// <summary>
	///		Temporary store chat that can't directly send to group.
	/// </summary>
	/// <remarks>
	///		Removed after all recipient receive the chat or deleted before all recepient get the chat.
	/// </remarks>
	/// <typeparam name="T">
	///		The type used for the primary key for the model.
	/// </typeparam>
	public class GroupChat<T> : BaseModel<T>
		where T : struct
	{
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
		public string Message { set; get; }

		/// <summary>
		///		Date and time the message or chat is created.
		/// </summary>
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "dd-MM-yyyy HH:mm:ss")]
		public DateTime Timestamp { set; get; } = DateTime.UtcNow;

		/// <summary>
		///		Member id that already receive the message.
		/// </summary>
		public string AlreadySendTo { set; get; }
	}

	/// <summary>
	///		Relationship of user with each other.
	/// </summary>
	public class GroupChat : GroupChat<Guid>
	{

	}
}