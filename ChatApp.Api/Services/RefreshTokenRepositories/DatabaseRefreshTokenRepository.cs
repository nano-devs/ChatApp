namespace ChatApp.Api.Services.RefreshTokenRepositories;

using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;

public class DatabaseRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ChatAppDbContext context;

    public DatabaseRefreshTokenRepository(ChatAppDbContext context)
    {
        this.context = context;
    }

    public async Task Create(RefreshToken refreshToken)
    {
        context.RefreshTokens?.Add(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid refreshTokenId)
    {
        RefreshToken? refreshToken = await context.RefreshTokens!.FindAsync(refreshTokenId);
        if (refreshToken != null)
        {
            context.RefreshTokens.Remove(refreshToken);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAll(int userId)
    {
        IEnumerable<RefreshToken> refreshTokens = await context.RefreshTokens!.Where(t => t.UserId == userId).ToListAsync();

        context.RefreshTokens!.RemoveRange(refreshTokens);
        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await context.RefreshTokens!.FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
    }
}

