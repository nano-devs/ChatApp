namespace ChatApp.Api.Services.TokenGenerators;

using ChatApp.Api.Models;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
