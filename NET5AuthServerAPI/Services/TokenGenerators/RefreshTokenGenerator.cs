using Microsoft.IdentityModel.Tokens;
using NET5AuthServerAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NET5AuthServerAPI.Services.TokenGenerators
{
    public class RefreshTokenGenerator : ITokenGenerator
    {
        private readonly AuthenticationConfiguration configuration;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // redundant code

            // key used to sign jwt is gonna be the same as the key used for verify jwt
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.RefreshTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                configuration.Issuer, // issuer domain
                configuration.Audience, // audience
                null, // claims
                System.DateTime.UtcNow, // token valid datetime
                System.DateTime.UtcNow.AddMinutes(configuration.RefreshTokenExpirationMinutes), // token expired datetime
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
