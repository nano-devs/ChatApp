namespace ChatApp.Api.Models;

using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public string? Token { get; set; }
}
