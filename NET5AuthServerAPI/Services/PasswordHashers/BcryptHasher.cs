namespace NET5AuthServerAPI.Services.PasswordHashers
{
    public class BCryptHasher : IPasswordHasher
    {
        public string HashPassword(string raw)
        {
            return BCrypt.Net.BCrypt.HashPassword(raw);
        }

        public bool VerifyPassword(string raw, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(raw, hash);
        }
    }
}
