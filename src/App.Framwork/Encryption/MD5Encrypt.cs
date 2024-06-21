using System;
using System.Security.Cryptography;
using System.Text;

namespace App.Framwork.Encryption
{
    /// <summary>
    /// Md5加密
    /// </summary>
    public static class Md5Encrypt
    {
        /// <summary>
        /// 32位md5加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.Default.GetBytes(text));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 16位md5加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns></returns>
        public static string Encrypt16(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string encrypted = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(text)), 4, 8);
            return encrypted.Replace("-", "").ToLower();
        }
    }
}