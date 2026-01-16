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


    public static class ConnectionStringCryptoHelper
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
        /// <param name="name">连接字符串名称，默认为"LogDatabase"</param>
        /// <returns>解密后的连接字符串</returns>
        /// <exception cref="ApplicationException">当无法获取或解密连接字符串时抛出</exception>
        public static string GetDecryptedConnectionString(string name = "LogDatabase")
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"尝试获取连接字符串 '{name}'...");
                string encryptedConnectionString = null;
                
                // 1. 安全地从ConnectionStrings配置节获取，避免AccessViolationException
                try
                {
                    if (ConfigurationManager.ConnectionStrings != null && ConfigurationManager.ConnectionStrings.Count > 0)
                    {
                        var connSetting = ConfigurationManager.ConnectionStrings[name];
                        if (connSetting != null)
                        {
                            encryptedConnectionString = connSetting.ConnectionString;
                            System.Diagnostics.Debug.WriteLine($"从ConnectionStrings获取到连接字符串 '{name}'");
                        }
                    }
                }
                catch (Exception connEx)
                {
                    System.Diagnostics.Debug.WriteLine($"访问ConnectionStrings时出错: {connEx.Message}");
                    // 继续尝试其他方式，不立即抛出异常
                }

                // 2. 如果ConnectionStrings中没有，尝试从AppSettings获取
                if (string.IsNullOrEmpty(encryptedConnectionString))
                {
                    try
                    {
                        if (ConfigurationManager.AppSettings != null)
                        {
                            encryptedConnectionString = ConfigurationManager.AppSettings[name];
                            if (!string.IsNullOrEmpty(encryptedConnectionString))
                            {
                                System.Diagnostics.Debug.WriteLine($"从AppSettings获取到连接字符串 '{name}'");
                            }
                        }
                    }
                    catch (Exception appEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"访问AppSettings时出错: {appEx.Message}");
                    }
                }

                // 3. 特别处理：如果找不到指定名称的连接字符串，尝试使用通用的"ConnectString"配置
                // 避免递归调用导致的潜在问题
                if (string.IsNullOrEmpty(encryptedConnectionString) && name != "ConnectString")
                {
                    System.Diagnostics.Debug.WriteLine($"尝试使用通用连接字符串 'ConnectString' 替代 '{name}'");
                    try
                    {
                        // 直接获取ConnectString，避免递归调用
                        if (ConfigurationManager.ConnectionStrings != null)
                        {
                            var connectSetting = ConfigurationManager.ConnectionStrings["ConnectString"];
                            if (connectSetting != null)
                            {
                                encryptedConnectionString = connectSetting.ConnectionString;
                            }
                        }
                        
                        // 如果ConnectionStrings中没有，尝试AppSettings
                        if (string.IsNullOrEmpty(encryptedConnectionString) && ConfigurationManager.AppSettings != null)
                        {
                            encryptedConnectionString = ConfigurationManager.AppSettings["ConnectString"];
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"获取通用连接字符串时出错: {ex.Message}");
                    }
                }

                // 4. 如果仍然找不到，抛出异常
                if (string.IsNullOrEmpty(encryptedConnectionString))
                {
                    throw new ApplicationException($"连接字符串 '{name}' 未在配置文件中找到");
                }

                // 5. 解密连接字符串
                string key = "ruinor1234567890";
                System.Diagnostics.Debug.WriteLine("开始解密连接字符串...");
                string decryptedConnectionString = HLH.Lib.Security.EncryptionHelper.AesDecrypt(encryptedConnectionString, key);
                System.Diagnostics.Debug.WriteLine("连接字符串解密成功");
                
                // 6. 验证解密后的连接字符串是否有效
                if (string.IsNullOrEmpty(decryptedConnectionString) || !decryptedConnectionString.ToLower().Contains("server"))
                {
                    throw new ApplicationException("解密后的连接字符串无效或为空");
                }
                
                return decryptedConnectionString;
            }
            catch (AccessViolationException ave)
            {
                System.Diagnostics.Debug.WriteLine($"内存访问冲突错误 - 获取连接字符串 '{name}': {ave.Message}");
                throw new ApplicationException($"内存访问冲突: 无法安全访问配置文件中的连接字符串 '{name}'", ave);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取或解密连接字符串 '{name}' 失败: " + ex.Message);
                // 包装异常，提供更多上下文信息
                throw new ApplicationException($"获取或解密连接字符串 '{name}' 时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取并解密连接字符串，出错时返回默认连接字符串而不是抛出异常
        /// </summary>
        /// <param name="name">连接字符串名称</param>
        /// <param name="defaultConnectionString">默认连接字符串</param>
        /// <returns>解密后的连接字符串或默认连接字符串</returns>
        public static string GetDecryptedConnectionStringOrDefault(string name = "LogDatabase", string defaultConnectionString = null)
        {
            try
            {
                return GetDecryptedConnectionString(name);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取连接字符串失败，使用默认值: {ex.Message}");
                return defaultConnectionString;
            }
        }
    }
}

