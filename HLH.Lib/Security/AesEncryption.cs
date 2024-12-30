using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
 

namespace HLH.Lib.Security
{
    /*
     使用方法：
加密：使用EncryptString方法，传入明文字符串和密钥。
解密：使用DecryptString方法，传入加密后的字符串和密钥。
注意事项：
密钥管理：密钥不应硬编码在代码中，应通过安全的方式存储和传输。
初始化向量（IV）：在加密过程中，IV应随机生成，并与加密数据一起存储，因为解密时需要使用相同的IV。
异常处理：在实际应用中，应添加适当的异常处理逻辑来处理加密和解密过程中可能出现的错误。
这种方法利用AES算法的安全性，结合随机IV，提供了一个较为安全的加密解密解决方案。然而，请注意，加密算法的安全性也依赖于密钥的保密性和系统的安全性。在实际应用中，还应考虑其他安全措施，如使用安全的密钥存储机制、定期更换密钥等。


     */
    public class AesEncryption
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptString(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // 生成随机的初始化向量
                aesAlg.GenerateIV();
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aesAlg.Key = keyBytes;

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
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

        public static string DecryptString(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aesAlg.Key = keyBytes;

                // 从加密的字符串中提取初始化向量
                byte[] fullCipher = Convert.FromBase64String(cipherText);
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                using (MemoryStream msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    /*
     
    byte[] aesKey = AesKeyGenerator.GenerateAesKey256();
        // 这里可以将aesKey用于AES加密
     */
    public class AesKeyGenerator
    {
        public static byte[] GenerateAesKey256(string password)
        {
            // 使用Rfc2898DeriveBytes生成密钥，这里使用一个密码和盐值
            // 密码和盐值应该是随机生成的，并且保密
           // string password = "your_password_here"; // 请替换为一个安全的密码
            byte[] salt = new byte[32]; // 盐值长度应至少与密钥长度相同
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt); // 生成随机盐值
            }

            // 创建Rfc2898DeriveBytes实例，使用密码、盐值和迭代次数
            // 迭代次数越高，密钥派生越慢，也越安全
            int iterations = 1000; // 可以根据需要调整迭代次数
            var kdf = new Rfc2898DeriveBytes(password, salt, iterations);

            // 生成256位密钥
            byte[] key = kdf.GetBytes(32); // 256位 / 8 = 32字节

            // 打印密钥和盐值（实际应用中不应该打印密钥）
            Console.WriteLine("密钥: " + BitConverter.ToString(key));
            Console.WriteLine("盐值: " + BitConverter.ToString(salt));

            return key;
        }
    }

    // 使用示例
    //public class Program
    //{
    //    public static void Main()
    //    {
    //        byte[] aesKey = AesKeyGenerator.GenerateAesKey256();
    //        // 这里可以将aesKey用于AES加密
    //    }
    //}
    public class AesEncryptionNew
    {
        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] buffer = new byte[length];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
            }
            return buffer;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key"> key = "your-256-bit-key"; // This should be a 256-bit key for AES</param>
        /// <returns></returns>
        public static string EncryptString(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Generate a random IV
                aesAlg.IV = GenerateRandomBytes(aesAlg.BlockSize / 8);

                // Convert the key and IV to byte arrays
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                string encrypted = string.Empty;
                // Create a MemoryStream and a CryptoStream using the MemoryStream
                using (MemoryStream msEncrypt = new MemoryStream())
                {


                    using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(keyBytes, aesAlg.IV), CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainBytes);
                    }
                    // Convert the encrypted data to a base64 string
                    encrypted = Convert.ToBase64String(msEncrypt.ToArray());
                }
                // Return the IV and encrypted data concatenated with a colon (:)
                return Convert.ToBase64String(aesAlg.IV) + ":" + encrypted;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key"> key = "your-256-bit-key"; // This should be a 256-bit key for AES</param>
        /// <returns></returns>
        public static string DecryptString(string cipherText, string key)
        {
            // Split the IV and encrypted data
            string[] parts = cipherText.Split(':');
            string ivString = parts[0];
            string encryptedData = parts[1];

            // Convert the IV and encrypted data from base64 strings to byte arrays
            byte[] iv = Convert.FromBase64String(ivString);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            using (Aes aesAlg = Aes.Create())
            {
                // Convert the key to a byte array
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                // Create a MemoryStream and a CryptoStream using the MemoryStream
                using (var msDecrypt = new MemoryStream(encryptedBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }


        /*
         * private string key = "your-256-bit-key"; // This should be a 256-bit key for AES

    public MainForm()
    {
        InitializeComponent();
    }

    private void buttonEncrypt_Click(object sender, EventArgs e)
    {
        string plainText = textBoxPlainText.Text;
        string encryptedText = AesEncryption.EncryptString(plainText, key);
        textBoxEncryptedText.Text = encryptedText;
    }

    private void buttonDecrypt_Click(object sender, EventArgs e)
    {
        string cipherText = textBoxEncryptedText.Text;
        string decryptedText = AesEncryption.DecryptString(cipherText, key);
        textBoxDecryptedText.Text = decryptedText;
    }
         * **/

//        要生成一个安全的256位AES加密密钥，可以遵循以下步骤：

//使用安全的随机数生成器：生成密钥需要高质量的随机数。使用安全的随机数生成器（CSPRNG，Cryptographically Secure Pseudo-Random Number Generator）是确保密钥不可预测性的关键。在Java中，SecureRandom类是一个CSPRNG，可以用于生成安全的随机数
//。

//密钥生成：使用KeyGenerator类来生成密钥。通过KeyGenerator.getInstance("AES")获得一个KeyGenerator对象，并指定AES算法。然后使用keyGen.init(256, secureRandom)初始化KeyGenerator，指定密钥长度为256位，并传入SecureRandom实例以确保随机性
//。

//生成密钥：调用keyGen.generateKey() 生成密钥。这个密钥将是一个SecretKey对象，可以通过secretKey.getEncoded() 方法获取密钥的字节数组表示
//。

//密钥存储：生成的密钥必须安全存储，以防止未经授权的访问和泄露。可以使用硬件安全模块（HSM）或软件密钥库（如AWS KMS、Azure Key Vault）来保护密钥
//。


    }
}
