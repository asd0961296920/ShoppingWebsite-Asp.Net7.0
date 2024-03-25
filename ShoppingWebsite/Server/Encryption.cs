using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Security.Cryptography;

namespace Server
{
    public class Encryption
    {
        public static string CheckMacValue(string data)
        {
            string replacedString = data.Replace("%", "%25");

            replacedString = replacedString.Replace("~", "%7e");
            replacedString = replacedString.Replace("+", "%2b");
            replacedString = replacedString.Replace(" ", "+");
            replacedString = replacedString.Replace("@", "%40");
            replacedString = replacedString.Replace("#", "%23");
            replacedString = replacedString.Replace("$", "%24");
            replacedString = replacedString.Replace("&", "%26");
            replacedString = replacedString.Replace("=", "%3d");
            replacedString = replacedString.Replace(";", "%3b");
            replacedString = replacedString.Replace("?", "%3f");

            replacedString = replacedString.Replace("/", "%2f");
            replacedString = replacedString.Replace("\\", "%5c");
            replacedString = replacedString.Replace(">", "%3e");
            replacedString = replacedString.Replace("<", "%3c");
            replacedString = replacedString.Replace("`", "%60");
            replacedString = replacedString.Replace("[", "%5b");
            replacedString = replacedString.Replace("]", "%5d");
            replacedString = replacedString.Replace("{", "%7b");
            replacedString = replacedString.Replace("}", "%7d");
            replacedString = replacedString.Replace(":", "%3a");

            replacedString = replacedString.Replace("'", "%27");
            replacedString = replacedString.Replace("\"", "%22");
            replacedString = replacedString.Replace(",", "%2c");
            replacedString = replacedString.Replace("|", "%7c");


            //轉小寫
            replacedString = replacedString.ToLower();

            replacedString = CalculateSHA256(replacedString);

            return replacedString;
        }

        public static string CalculateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("X2")); // 将每个字节转换为两位的十六进制字符串
                }

                return builder.ToString();
            }
        }
    }
}

