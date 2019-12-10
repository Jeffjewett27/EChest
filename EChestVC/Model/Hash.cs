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

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hasher.Hash.Length; i++)
            {
                sb.Append(hasher.Hash[i].ToString("x2"));
            }
            return sb.ToString();
            //return Encoding.ASCII.GetString(hasher.Hash);
        }
    }
}
