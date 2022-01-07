namespace ChatApp.Auth.Services.TokenGenerators;

using ChatApp.Auth.Models;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
