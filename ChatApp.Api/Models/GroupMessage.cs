namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;

public class GroupMessage
{
    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    public int MessageId { get; set; }

    public Group? Group { get; set; }

    public Message? Message { get; set; }
}
