using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;

namespace RUINORERP.Common.Helper
{
    public static class CryptoHelper
    {
        // 修改这些密钥 - 在实际应用中应从安全的地方获取
        //private static readonly byte[] Key = Encoding.UTF8.GetBytes("Yhua32CharacterLongKeyHere!!"); // 32字节密钥
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ruinor1234567890"); // 32字节密钥
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("Ywaw16CharacterIV!!"); // 16字节初始化向量

        /// <summary>
        /// 加密字符串
        /// </summary>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从配置文件获取并解密连接字符串
        /// </summary>
        public static string GetDecryptedConnectionString(string name = "LogDatabase")
        {
            try
            {
                var encryptedConnectionString = ConfigurationManager.ConnectionStrings[name]?.ConnectionString;

                if (string.IsNullOrEmpty(encryptedConnectionString))
                {
                    // 尝试从AppSettings获取
                    encryptedConnectionString = ConfigurationManager.AppSettings[name];
                }

                if (string.IsNullOrEmpty(encryptedConnectionString))
                {
                    throw new ApplicationException($"连接字符串 '{name}' 未在配置文件中找到");
                }

                // string conn = AppSettings.GetValue("ConnectString");
                string key = "ruinor1234567890";
                string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(encryptedConnectionString, key);
                return newconn;
                //  return Decrypt(encryptedConnectionString);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("解密连接字符串时发生错误", ex);
            }
        }
    }
}

