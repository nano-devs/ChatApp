using System;
using System.ComponentModel.DataAnnotations;

namespace NET5ChatAppServerAPI.Models
{
	/// <summary>
	///		Model with custom Primary Key.
	/// </summary>
	/// <typeparam name="T">
	///		The type used for the primary key for the model.
	/// </typeparam>
	public abstract class BaseModel<T>
		where T : struct
	{
		/// <summary>
		///		Universally unique identifier for each records.
		/// </summary>
		/// <remarks>
		///		Primary Key for database table.
		/// </remarks>
		public virtual T Id { set; get; }
	}

	/// <summary>
	///		Model with <see cref="Guid"/> as Primary Key.
	/// </summary>
	public abstract class BaseModel : BaseModel<Guid>
	{

	}
}
