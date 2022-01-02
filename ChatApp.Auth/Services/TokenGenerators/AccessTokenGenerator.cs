﻿namespace ChatApp.Auth.Services.TokenGenerators;

using Microsoft.IdentityModel.Tokens;
using ChatApp.Auth.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatApp.Auth.Models.Configurations;

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
            new Claim(ClaimTypes.Name, user.UserName),
        };

        JwtSecurityToken token = new JwtSecurityToken(
            configuration.Issuer, // issuer domain
            configuration.Audience, // audience
            claims,
            DateTime.UtcNow, // token valid datetime
            DateTime.UtcNow.AddMinutes(configuration.AccessTokenExpirationMinutes), // token expired datetime
            credentials);

        // get the string of jwt token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

