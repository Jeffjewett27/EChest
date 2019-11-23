using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace EChestVC.Model
{
    static class Hash
    {
        public static string ComputeHash(string input)
        {
            var hasher = SHA256.Create();
            hasher.ComputeHash(Encoding.ASCII.GetBytes(input));
            return Encoding.ASCII.GetString(hasher.Hash);
        }
    }
}
