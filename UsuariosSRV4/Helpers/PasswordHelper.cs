// UsuariosSRV4/Helpers/PasswordHelper.cs
using System.Security.Cryptography;
using System.Text;

namespace UsuariosSRV4.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}