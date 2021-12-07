using NET5AuthServerAPI.Models;
using NET5AuthServerAPI.Models.Responses;
using NET5AuthServerAPI.Services.RefreshTokenRepositories;
using NET5AuthServerAPI.Services.TokenGenerators;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Services.Authenticators
{
    public class Authenticator
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly ITokenGenerator accessTokenGenerator;
        private readonly ITokenGenerator refreshTokenGenerator;

        public Authenticator(IRefreshTokenRepository refreshTokenRepository, 
            ITokenGenerator accessTokenGenerator, 
            ITokenGenerator refreshTokenGenerator)
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
}
