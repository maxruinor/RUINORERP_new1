using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RUINORERP.Common.Helper.Security
{
    /// <summary>
    /// 配置加密助手类
    /// 提供配置项的加密和解密功能
    /// </summary>
    public class ConfigEncryptionHelper
    {
        // 加密密钥 - 在实际项目中，应该从安全存储或环境变量中获取
        // 注意：此处为示例，实际生产环境中需要更安全的密钥管理
        private static readonly string _encryptionKey = "RUINOR_ERP_CONFIG_ENCRYPTION_KEY_2024";
        
        // 初始化向量
        private static readonly byte[] _iv = new byte[16] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using (Aes aesAlg = Aes.Create())
            {
                // 设置加密参数
                aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
                aesAlg.IV = _iv;

                // 创建加密器
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // 写入加密数据
                        swEncrypt.Write(plainText);
                    }

                    // 获取加密后的字节数组
                    byte[] encrypted = msEncrypt.ToArray();
                    // 返回Base64编码的字符串
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText">加密后的Base64字符串</param>
        /// <returns>解密后的明文</returns>
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    // 设置解密参数
                    aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
                    aesAlg.IV = _iv;

                    // 创建解密器
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // 将Base64字符串转换为字节数组
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // 读取解密后的数据
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                // 解密失败时返回原始字符串，避免配置加载错误
                // 在实际项目中，可以记录错误日志
                return cipherText;
            }
        }

        /// <summary>
        /// 判断字符串是否已加密
        /// </summary>
        /// <param name="text">待检查的字符串</param>
        /// <returns>是否已加密</returns>
        public static bool IsEncrypted(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            // 简单检查：加密后的字符串是Base64格式，以加密标记开头
            // 在实际项目中，可以使用更复杂的判断逻辑
            return text.StartsWith("ENCRYPTED_");
        }

        /// <summary>
        /// 加密配置值，添加加密标记
        /// </summary>
        /// <param name="value">配置值</param>
        /// <returns>带标记的加密配置值</returns>
        public static string EncryptConfigValue(string value)
        {
            if (IsEncrypted(value))
                return value;

            string encrypted = Encrypt(value);
            return "ENCRYPTED_" + encrypted;
        }

        /// <summary>
        /// 解密配置值
        /// </summary>
        /// <param name="value">带标记的加密配置值</param>
        /// <returns>解密后的配置值</returns>
        public static string DecryptConfigValue(string value)
        {
            if (!IsEncrypted(value))
                return value;

            // 移除加密标记
            string encrypted = value.Substring(9);
            return Decrypt(encrypted);
        }

        /// <summary>
        /// 加密JSON对象中的敏感字段
        /// </summary>
        /// <param name="jsonObject">JSON对象</param>
        /// <param name="sensitiveFields">敏感字段列表</param>
        public static void EncryptSensitiveFields(JObject jsonObject, string[] sensitiveFields)
        {
            if (jsonObject == null || sensitiveFields == null || sensitiveFields.Length == 0)
                return;

            foreach (string field in sensitiveFields)
            {
                if (jsonObject.ContainsKey(field) && jsonObject[field].Type == JTokenType.String)
                {
                    string value = jsonObject[field].ToString();
                    if (!string.IsNullOrEmpty(value) && !IsEncrypted(value))
                    {
                        jsonObject[field] = EncryptConfigValue(value);
                    }
                }
            }
        }

        /// <summary>
        /// 解密JSON对象中的敏感字段
        /// </summary>
        /// <param name="jsonObject">JSON对象</param>
        /// <param name="sensitiveFields">敏感字段列表</param>
        public static void DecryptSensitiveFields(JObject jsonObject, string[] sensitiveFields)
        {
            if (jsonObject == null || sensitiveFields == null || sensitiveFields.Length == 0)
                return;

            foreach (string field in sensitiveFields)
            {
                if (jsonObject.ContainsKey(field) && jsonObject[field].Type == JTokenType.String)
                {
                    string value = jsonObject[field].ToString();
                    if (IsEncrypted(value))
                    {
                        jsonObject[field] = DecryptConfigValue(value);
                    }
                }
            }
        }
    }
}