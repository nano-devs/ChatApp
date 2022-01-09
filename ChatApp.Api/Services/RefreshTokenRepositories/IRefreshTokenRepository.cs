﻿namespace ChatApp.Api.Services.RefreshTokenRepositories;

using ChatApp.Api.Models;

public interface IRefreshTokenRepository
{
    Task Create(RefreshToken refreshToken);
    Task<RefreshToken?> GetByToken(string token);
    Task Delete(Guid refreshTokenId);
    Task DeleteAll(int userId);
}
