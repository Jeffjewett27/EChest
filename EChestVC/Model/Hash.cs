using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace EChestVC.Model
{
    static class Hash
    {
        public static string ComputeHash(string input)
        {
            return ComputeHash(Encoding.ASCII.GetBytes(input));
        }

        public static string ComputeHash(byte[] input)
        {
            var hasher = SHA256.Create();
            hasher.ComputeHash(input);
            string hash = HashToHex(hasher);
            hasher.Dispose();
            return hash;
        }

        public static string ComputeHash(Stream input)
        {
            var hasher = SHA256.Create();
            hasher.ComputeHash(input);
            string hash = HashToHex(hasher);
            hasher.Dispose();
            return hash;
        }

        private static string HashToHex(HashAlgorithm hasher)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hasher.Hash.Length; i++)
            {
                sb.Append(hasher.Hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
