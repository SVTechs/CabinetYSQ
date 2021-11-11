using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Encryption
{
    public class ShaEncode
    {
        public static string Sha1Encode(string plainText, bool toLower)
        {
            var buffer = Encoding.UTF8.GetBytes(plainText);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }
            if (toLower) return sb.ToString().ToLower();
            return sb.ToString();
        }
    }
}
