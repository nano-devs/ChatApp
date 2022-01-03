namespace ChatApp.Auth.Services.RefreshTokenRepositories;

using ChatApp.Auth.Models;

public interface IRefreshTokenRepository
{
    Task Create(RefreshToken refreshToken);
    Task<RefreshToken?> GetByToken(string token);
    Task Delete(Guid refreshTokenId);
    Task DeleteAll(Guid userId);
}
