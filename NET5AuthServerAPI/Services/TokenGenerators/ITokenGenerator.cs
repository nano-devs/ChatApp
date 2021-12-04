using NET5AuthServerAPI.Models;

namespace NET5AuthServerAPI.Services.TokenGenerators
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
