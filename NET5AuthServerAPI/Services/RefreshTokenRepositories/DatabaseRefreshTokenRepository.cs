using Microsoft.EntityFrameworkCore;
using NET5AuthServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.RefreshTokenRepositories
{
    public class DatabaseRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthenticationDbContext context;

        public DatabaseRefreshTokenRepository(AuthenticationDbContext context)
        {
            this.context = context;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid refreshTokenId)
        {
            RefreshToken refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);
            if (refreshToken != null)
            {
                context.RefreshTokens.Remove(refreshToken);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(Guid userId)
        {
            IEnumerable<RefreshToken> refreshTokens = await context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();

            context.RefreshTokens.RemoveRange(refreshTokens);
            await context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
        }
    }
}
