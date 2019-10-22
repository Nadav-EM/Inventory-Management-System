using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Telephone
{
    class Sha
    {
        private string hashedData;
        public Sha(string str)
        {
            hashedData = ComputeSha256Hash(str);
        }
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        /// <summary>
        /// מקבל את הסיסמא אחרי הצפנה
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return hashedData;
        }
    }
}
