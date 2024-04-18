using System.Text;
using System.Security.Cryptography;

namespace COMMSFinalProject.Services 
{
    public class HashHelper
    {
        public static string HashPassword(string password)
        {
            SHA1CryptoServiceProvider cryptoProv = new SHA1CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] encryptedBytes = cryptoProv.ComputeHash(passwordBytes);
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
