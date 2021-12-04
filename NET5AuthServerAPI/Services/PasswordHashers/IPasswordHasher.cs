namespace NET5AuthServerAPI.Services.PasswordHashers
{
    public interface IPasswordHasher
    {
        string HashPassword(string raw);

        bool VerifyPassword(string raw, string hash);
    }
}
