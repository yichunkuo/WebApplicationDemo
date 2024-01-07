using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplicationDemo.Helper
{
    public static class HashHelper
    {
        /// <summary>
        /// 隨機產生鹽
        /// </summary>
        /// <param name="length">鹽需要的長度</param>
        /// <returns></returns>
        public static string GenerateRandomSalt(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                var result = new StringBuilder(length);
                foreach (var byteValue in randomBytes)
                {
                    result.Append(chars[byteValue % chars.Length]);
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// SHA-256加密
        /// </summary>
        /// <param name="userword">要加密的字</param>
        /// <param name="usersalt">加密用的鹽</param>
        /// <returns></returns>
        public static string HashWord(string userword, string usersalt)
        {
            // 編碼
            byte[] passwordBytes = Encoding.UTF8.GetBytes(userword);
            byte[] saltBytes = Encoding.UTF8.GetBytes(usersalt);

            // 合併密碼和鹽
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            // 使用 SHA-256 進行計算
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// 複雜度檢查，夠複雜才是true，不夠複雜回false
        /// </summary>
        /// <param name="userword">要檢查的字</param>
        /// <param name="limit">最少要幾種(最大4種都檢查)</param>
        /// <returns></returns>
        public static bool CheckWorkComplex(string userword, int limit)
        {
            int strength = 0;

            // 使用正則表達式檢查特殊字符、大寫字母、小寫字母、數字
            Regex teShu = new Regex("[~!@#$%_^&*()=+[\\]{}''\";:/?.,><`|！·￥…—（）\\-、；：。，》《]");
            Regex daXie = new Regex("[A-Z]");
            Regex xiaoXie = new Regex("[a-z]");
            Regex shuZi = new Regex("[0-9]");

            if (teShu.IsMatch(userword)) strength++;
            if (daXie.IsMatch(userword)) strength++;
            if (xiaoXie.IsMatch(userword)) strength++;
            if (shuZi.IsMatch(userword)) strength++;

            return strength > limit;
        }

        public static string GetClientIpAddress()
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ipAddress;
        }
    }
}