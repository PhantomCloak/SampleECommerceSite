using System;
using System.Security.Cryptography;
using System.Text;

namespace EFurni.Services
{
    internal class CryptographyService : ICryptographyService
    {
        private static readonly HashAlgorithm HashAlgorithm = new SHA256Managed();
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;
        
        public string HashString(string plainText,string saltStr)
        {
            var salt = DefaultEncoding.GetBytes(saltStr);
            var plainTextBytes = DefaultEncoding.GetBytes(plainText);

            var algorithm = HashAlgorithm;

            var saltedPlaintextBytes = new byte[plainText.Length + salt.Length];

            Array.Copy(plainTextBytes,0,saltedPlaintextBytes,0,plainText.Length);
            Array.Copy(salt,0,saltedPlaintextBytes,plainText.Length,salt.Length);

            var hashedBytes = algorithm.ComputeHash(saltedPlaintextBytes);
            
            var sb = new StringBuilder();
            foreach (var b in hashedBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            
            return sb.ToString();
        }
    }
}