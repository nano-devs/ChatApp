﻿namespace ChatApp.Auth.Services.TokenGenerators;

using ChatApp.Auth.Models;
using ChatApp.Auth.Models.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AccessTokenGenerator : ITokenGenerator
{
    private readonly AuthenticationConfiguration configuration;

    public AccessTokenGenerator(AuthenticationConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.AccessTokenSecret!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        JwtSecurityToken token = new JwtSecurityToken(
            configuration.Issuer,
            configuration.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(configuration.AccessTokenExpirationMinutes),
            credentials
        );

        // get the string of jwt token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
