namespace ChatApp.Auth.Services.RefreshTokenRepositories; 

using ChatApp.Auth.Models;

public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();

    public Task Create(RefreshToken refreshToken)
    {
        refreshToken.Id = Guid.NewGuid();

        refreshTokens.Add(refreshToken);

        return Task.CompletedTask;
    }

    public Task Delete(Guid refreshTokenId)
    {
        refreshTokens.RemoveAll(rt => rt.Id == refreshTokenId);

        return Task.CompletedTask;
    }

    public Task DeleteAll(Guid userId)
    {
        refreshTokens.RemoveAll(rt => rt.UserId.ToString() == userId.ToString());

        return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetByToken(string token)
    {
        RefreshToken? refreshToken = refreshTokens.FirstOrDefault(refreshToken => refreshToken.Token == token);
        return Task.FromResult(refreshToken);
    }
}

