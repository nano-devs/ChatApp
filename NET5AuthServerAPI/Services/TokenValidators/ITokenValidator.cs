namespace NET5AuthServerAPI.Services.TokenValidators
{
    public interface ITokenValidator
    {
        bool Validate(string token);
    }
}
