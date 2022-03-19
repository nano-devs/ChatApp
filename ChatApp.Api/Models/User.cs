namespace ChatApp.Api.Models;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser<int>
{
    public Guid UniqueGuid { get; set; } = Guid.NewGuid();

    [Required]
    public string? PublicName { get; set; }
    
    public ICollection<User>? Friends { get; set; }
    
    public ICollection<GroupMember>? GroupMembers { get; set; }
    
    public ICollection<Message>? Messages { get; set; }
    
    public ICollection<PrivateMessage>? PrivateMessages { get; set; }
}
