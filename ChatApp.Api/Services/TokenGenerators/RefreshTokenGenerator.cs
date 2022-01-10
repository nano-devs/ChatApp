namespace ChatApp.Api.Services.TokenGenerators;

using ChatApp.Api.Models;
using ChatApp.Api.Models.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class RefreshTokenGenerator : ITokenGenerator
{
    private readonly AuthenticationConfiguration configuration;

    public RefreshTokenGenerator(AuthenticationConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        // key used to sign jwt is gonna be the same as the key used for verify jwt
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.RefreshTokenSecret!));
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
