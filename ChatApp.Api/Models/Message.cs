namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;

public class Message
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]
    public DateTime SentDateTime { set; get; } = DateTime.Now;

    [Required]
    [MaxLength(255)]
    public string? Content { get; set; }

    [Required]
    public int PostedByUserId { get; set; }

    public User? PostedByUser { get; set; }
}
