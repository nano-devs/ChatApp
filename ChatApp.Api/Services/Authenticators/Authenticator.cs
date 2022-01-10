namespace ChatApp.Api.Services.Authenticators;

using ChatApp.Api.Models;
using ChatApp.Api.Models.Responses;
using ChatApp.Api.Services.Repositories.RefreshTokenRepositories;
using ChatApp.Api.Services.TokenGenerators;

public class Authenticator
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly ITokenGenerator accessTokenGenerator;
    private readonly ITokenGenerator refreshTokenGenerator;

    public Authenticator(IRefreshTokenRepository refreshTokenRepository, 
        AccessTokenGenerator accessTokenGenerator, 
        RefreshTokenGenerator refreshTokenGenerator)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.accessTokenGenerator = accessTokenGenerator;
        this.refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<AuthenticatedUserResponse> Authenticate(User user)
    {
        string accessToken = accessTokenGenerator.GenerateToken(user);
        string refreshToken = refreshTokenGenerator.GenerateToken(user);

        RefreshToken refreshTokenDTO = new RefreshToken()
        {
            UserId = user.Id,
            Token = refreshToken,
        };

        await refreshTokenRepository.Create(refreshTokenDTO);

        return new AuthenticatedUserResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}
