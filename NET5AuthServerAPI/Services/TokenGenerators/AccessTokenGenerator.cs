using Microsoft.IdentityModel.Tokens;
using NET5AuthServerAPI.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NET5AuthServerAPI.Services.TokenGenerators
{
    public class AccessTokenGenerator : ITokenGenerator
    {
        private readonly AuthenticationConfiguration configuration;

        public AccessTokenGenerator(AuthenticationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // key used to sign jwt is gonna be the same as the key used for verify jwt
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.AccessTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                configuration.Issuer, // issuer domain
                configuration.Audience, // audience
                claims,
                System.DateTime.UtcNow, // token valid datetime
                System.DateTime.UtcNow.AddMinutes(configuration.AccessTokenExpirationMinutes), // token expired datetime
                credentials);

            // get the string of jwt token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
