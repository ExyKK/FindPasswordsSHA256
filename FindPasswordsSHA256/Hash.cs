using System;
using System.Security.Cryptography;
using System.Text;

namespace FindPasswordsSHA256
{
    class Hash
    {
        public static string GetStringSha256Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha = new SHA256Managed())
            {
                byte[] message = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(message);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
