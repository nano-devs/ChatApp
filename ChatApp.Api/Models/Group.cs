namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;

public class Group<T> where T : struct
{
	public T Id { set; get; }

	[Required]
	public Guid UniqueGuid { set; get; }

	[Required]
	public string? Name { set; get; }

	public ICollection<GroupMember>? GroupMembers { set; get; }

	public ICollection<GroupMessage>? Messages { set; get; }
}

public class Group : Group<int>
{

}