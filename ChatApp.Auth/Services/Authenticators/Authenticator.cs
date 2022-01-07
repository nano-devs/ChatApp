namespace ChatApp.Auth.Services.Authenticators;

using ChatApp.Auth.Models;
using ChatApp.Auth.Models.Responses;
using ChatApp.Auth.Services.RefreshTokenRepositories;
using ChatApp.Auth.Services.TokenGenerators;

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
