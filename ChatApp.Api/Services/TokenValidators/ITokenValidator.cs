namespace ChatApp.Api.Services.TokenValidators;

public interface ITokenValidator
{
    bool Validate(string token);
}
