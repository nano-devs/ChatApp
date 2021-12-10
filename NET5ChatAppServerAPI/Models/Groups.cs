using System;

namespace NET5ChatAppServerAPI.Models
{
	/// <summary>
	///		Available groups.
	/// </summary>
	/// <typeparam name="T">
	///		The type used for the primary key for the model.
	/// </typeparam>
	public class Groups<T> : BaseModel<T>
		where T : struct
	{
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
