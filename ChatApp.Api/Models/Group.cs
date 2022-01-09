namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Group
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { set; get; }

	[Required]
	public Guid UniqueGuid { set; get; } = Guid.NewGuid();

	[Required]
	public string? Name { set; get; }
	
	public ICollection<User>? Users { set; get; }

	public ICollection<Message>? Messages { set; get; }
}