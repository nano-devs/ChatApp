using NET5AuthServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.RefreshTokenRepositories
{
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

        public Task<RefreshToken> GetByToken(string token)
        {
            RefreshToken refreshToken = refreshTokens.FirstOrDefault(refreshToken => refreshToken.Token == token);
            return Task.FromResult(refreshToken);
        }
    }
}
