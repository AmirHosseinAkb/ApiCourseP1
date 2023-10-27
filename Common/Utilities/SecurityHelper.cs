using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public class SecurityHelper
    {
        public static string HashPasswordSHA256(string password)
        {
            using(var sha = SHA256.Create())
            {
                var passBytes=Encoding.UTF8.GetBytes(password);
                var encodedPass=sha.ComputeHash(passBytes);
                return Convert.ToBase64String(encodedPass);
            }
        }
    }
}
