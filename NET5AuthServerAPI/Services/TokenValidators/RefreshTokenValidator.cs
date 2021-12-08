using Microsoft.IdentityModel.Tokens;
using NET5AuthServerAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NET5AuthServerAPI.Services.TokenValidators
{
    public class RefreshTokenValidator : ITokenValidator
    {
        private readonly AuthenticationConfiguration configuration;

        public RefreshTokenValidator(AuthenticationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool Validate(string token)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.RefreshTokenSecret)),
                ValidIssuer = configuration.Issuer,
                ValidAudience = configuration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = System.TimeSpan.Zero,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
