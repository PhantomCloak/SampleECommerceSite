using System;
using System.Security.Cryptography;
using System.Text;

namespace EFurni.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static int GetHashMd5(this string instance)
        {
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(instance));
            return BitConverter.ToInt32(hashed, 0);
        }
    }
}