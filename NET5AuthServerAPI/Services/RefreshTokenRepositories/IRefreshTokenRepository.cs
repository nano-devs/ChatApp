using NET5AuthServerAPI.Models;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {
        Task Create(RefreshToken refreshToken);
        Task<RefreshToken> GetByToken(string token);
        Task Delete(int refreshTokenId);
    }
}
