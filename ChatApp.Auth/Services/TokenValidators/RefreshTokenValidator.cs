﻿namespace ChatApp.Auth.Services.TokenValidators;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public class RefreshTokenValidator : ITokenValidator
{
    private readonly TokenValidationParameters validationParameters;
    private readonly JwtSecurityTokenHandler tokenHandler;

    public RefreshTokenValidator(TokenValidationParameters validationParameters, JwtSecurityTokenHandler tokenHandler)
    {
        this.validationParameters = validationParameters;
        this.tokenHandler = tokenHandler;
    }

    public bool Validate(string token)
    {
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
