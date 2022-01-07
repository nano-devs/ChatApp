namespace ChatApp.Auth.Services.TokenValidators;

public interface ITokenValidator
{
    bool Validate(string token);
}
