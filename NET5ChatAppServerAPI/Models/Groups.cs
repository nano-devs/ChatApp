using System;

namespace NET5ChatAppServerAPI.Models
{
	/// <summary>
	///		Available groups.
	/// </summary>
	/// <typeparam name="T">
	///		The type used for the primary key for the model.
	/// </typeparam>
	public class Groups<T> where T : struct
	{
		/// <summary>
		///		Universally unique identifier for each records.
		/// </summary>
		/// <remarks>
		///		Primary Key for database table.
		/// </remarks>
		public T Id { set; get; }

		/// <summary>
		///		Group name.
		/// </summary>
		public string Name { set; get; }
	}

	/// <summary>
	///		Available groups.
	/// </summary>
	public class Groups : Groups<Guid>
	{

	}
}
