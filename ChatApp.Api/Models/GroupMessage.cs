namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GroupMessage
{
    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    public int MessageId { get; set; }

    [Required]
    [ForeignKey("User")]
    public int SendToUserId { get; set; }

    public Group? Group { get; set; }

    public Message? Message { get; set; }

    public User? SendToUser { get; set; }
}
