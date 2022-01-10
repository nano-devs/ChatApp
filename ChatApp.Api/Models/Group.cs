namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Group
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { set; get; }

	[Required]
	public string? Name { set; get; }
	
	public ICollection<User>? Users { set; get; }

	public ICollection<GroupMessage>? Messages { set; get; }
}