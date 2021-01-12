using System;
using System.Linq;
using System.Text;

namespace ServiceTests.CryptoFunctions
{
    public class CryptoHelper
    {
        public static string CreateMd5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}