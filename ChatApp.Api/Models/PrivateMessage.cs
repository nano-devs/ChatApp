using System.ComponentModel.DataAnnotations;

namespace ChatApp.Api.Models;

public class PrivateMessage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SendToUserId { get; set; }

    [Required]
    public int MessageId { get; set; }

    public Guid? SendToUser { get; set; }

    public Message? Message { get; set; }
}
