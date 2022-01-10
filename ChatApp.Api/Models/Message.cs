namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime SentDateTime { set; get; } = DateTime.Now;

    [Required]
    [MaxLength(255)]
    public string? Content { get; set; }

    [Required]
    public int PostedByUserId { get; set; }

    [ForeignKey("PostedByUserId")]
    public User? PostedByUser { get; set; }

    public PrivateMessage? SendToPrivateMessage { get; set; }

    public Group? SendToGroup { get; set; }
}
