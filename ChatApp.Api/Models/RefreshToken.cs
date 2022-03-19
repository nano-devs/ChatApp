namespace ChatApp.Api.Models;

public class RefreshToken
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public string? Token { get; set; }
}
