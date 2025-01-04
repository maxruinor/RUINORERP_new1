using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HLH.Lib.Security
{

    public static class EncryptionHelper
    {
        /// <summary>
        ///加密 传入的KEY可以随意一些。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncryptByHashKey(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            try
            {
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
                // 确保key长度为16、24或32字节
                string normalizedKey = NormalizeKeyLength(key, 256); // 假设使用256位密钥
                byte[] keyBytes = GetHash(key, 32); // 对于AES-256位密钥
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = keyBytes,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }
        public static byte[] GetHash(string input, int outputBytes)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Enumerable.Repeat(hashBytes, outputBytes / hashBytes.Length + 1)
                                 .SelectMany(b => b)
                                 .Take(outputBytes)
                                 .ToArray();
            }
        }
        /*
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">符合AES密钥的要求（128位、192位或256位，即16字节、24字节或32字节 密文 注意：密文长度有 16，24，32，不能用其它长度的密文</param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            try
            {
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }
        */

        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            try
            {
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }

        private static string NormalizeKeyLength(string key, int keySizeInBits)
        {
            int keyLength = keySizeInBits / 8; // 将位转换为字节
            if (key.Length > keyLength)
            {
                // 如果用户名太长，截断它
                return key.Substring(0, keyLength);
            }
            else
            {
                // 如果用户名太短，用'0'填充它
                return key.PadRight(keyLength, '0');
            }
        }


        //public static string NormalizeKeyLength(string key, int keySize)
        //{
        //    int keyLength = keySize;/// 8; // 将位转换为字节
        //    if (key.Length > keyLength)
        //    {
        //        // 如果用户名太长，截断它
        //        return key.Substring(0, keyLength);
        //    }
        //    else
        //    {
        //        // 如果用户名太短，用'0'填充它
        //        return key.PadRight(keyLength, '0');
        //    }
        //}

            /// <summary>
            ///  AES 解密 已经在使用。注意引用的地方
            /// </summary>
            /// <param name="str">明文（待解密）</param>
            /// <param name="key">密文 注意：密文长度有 16，24，32，不能用其它长度的密文</param>
            /// <returns></returns>
            public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            try
            {
                Byte[] toEncryptArray = Convert.FromBase64String(str);

                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }

        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="key">密文 注意：密文长度有 16，24，32，不能用其它长度的密文</param>
        /// <returns></returns>
        public static string AesDecryptByHashKey(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            try
            {
                Byte[] toEncryptArray = Convert.FromBase64String(str);
                // 确保key长度为16、24或32字节
                string normalizedKey = NormalizeKeyLength(key, 256); // 假设使用256位密钥
                byte[] keyBytes = GetHash(key, 32); // 对于AES-256位密钥
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = keyBytes,// Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }

        }

        /*  下面是AI生成的。没有验证通过


        /// <summary>
        /// 加密数据使用了Rfc2898DeriveBytes来生成密钥和IV，并且确保了加密和解密过程是可逆的
        /// 请注意，Encoding.Default可能不是最好的选择，因为它取决于系统的默认编码，这可能不是UTF-8。如果你的文本是Unicode，你可能想要使用Encoding.Unicode或Encoding.UTF8。 此外，DESCryptoServiceProvider不是最安全的加密算法，如果你需要更高的安全性，你可能会考虑使用Aes算法。但是，Aes算法的密钥和IV的长度与DES不同，所以你需要相应地调整代码
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <param name="sKey">加密密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EnText(string text, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                // 使用密码派生函数来生成密钥和IV
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(sKey, 8);
                des.Key = deriveBytes.GetBytes(8);
                des.IV = deriveBytes.GetBytes(8);

                ICryptoTransform encryptor = des.CreateEncryptor();

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    StringBuilder ret = new StringBuilder();
                    foreach (byte b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }
                    return ret.ToString();
                }
            }
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="encryptedText">要解密的文本</param>
        /// <param name="sKey">解密密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DeText(string encryptedText, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                // 使用密码派生函数来生成密钥和IV
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(sKey, 8);
                des.Key = deriveBytes.GetBytes(8);
                des.IV = deriveBytes.GetBytes(8);

                ICryptoTransform decryptor = des.CreateDecryptor();

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    byte[] inputBytes = new byte[encryptedText.Length / 2];
                    for (int i = 0; i < encryptedText.Length; i += 2)
                    {
                        inputBytes[i / 2] = Convert.ToByte(encryptedText.Substring(i, 2), 16);
                    }

                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        */


    }
}
