using System;
using System.Security.Cryptography;
using System.Text;

namespace HLH.Lib.Security
{

    /// <summary>
    /// 注册码生成器
    /// </summary>
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    namespace HLH.Lib.Security
    {
        public class CreateRegisterCodeWin
        {
            /// <summary>
            /// 加密数据
            /// </summary>
            /// <param name="Text"></param>
            /// <param name="sKey"></param>
            /// <returns></returns>
            public static string EnText(string Text, string sKey)
            {
                StringBuilder ret = new StringBuilder();
                try
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(Text);
                    // 通过两次哈希密码设置对称算法的初始化向量
                    //des.Key = GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray();
                    des.Key = Encoding.UTF8.GetBytes(GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray());

                    // 通过两次哈希密码设置算法的机密密钥
                    //des.IV = GenerateHash(sKey, "SHA1").Substring(0, 8).Take(8).ToArray();
                    des.IV = Encoding.UTF8.GetBytes(GenerateHash(sKey, "SHA1").Substring(0, 8).Take(8).ToArray());


                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    foreach (byte b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }
                    return ret.ToString();
                }
                catch
                {
                    return "";
                }
            }

            /// <summary>
            /// 解密数据
            /// </summary>
            /// <param name="Text"></param>
            /// <param name="sKey"></param>
            /// <returns></returns>
            public static string DeText(string Text, string sKey)
            {
                try
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    int len = Text.Length / 2;
                    byte[] inputByteArray = new byte[len];
                    int x, i;
                    for (x = 0; x < len; x++)
                    {
                        i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                        inputByteArray[x] = (byte)i;
                    }
                    // 通过两次哈希密码设置对称算法的初始化向量
                    //des.Key = GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray();
                    des.Key = Encoding.UTF8.GetBytes(GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray());

                    // 通过两次哈希密码设置算法的机密密钥
                    //des.IV = GenerateHash(sKey, "SHA1").Substring(0, 8).Take(8).ToArray();
                    des.IV = Encoding.UTF8.GetBytes(GenerateHash(sKey, "SHA1").Substring(0, 8).Take(8).ToArray());

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return "";
                }
            }

            /// <summary>
            /// 将加密的字符串转换为注册码形式
            /// </summary>
            /// <param name="input">要加密字符串</param>
            /// <returns>装换后的字符串</returns>
            public static string Transform(string input, string skey)
            {
                string transactSn = string.Empty;
                if (input == "")
                {
                    return transactSn;
                }
                string initSn = string.Empty;
                try
                {
                    initSn = EnText(EnText(input, skey), skey);
                    transactSn = initSn.Substring(0, 5) + "-" + initSn.Substring(5, 5) +
                    "-" + initSn.Substring(10, 5) + "-" + initSn.Substring(15, 5) +
                    "-" + initSn.Substring(20, 5);
                    return transactSn;
                }
                catch
                {
                    return transactSn;
                }
            }

            /// <summary>
            /// 生成机器码
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static string CreateMachineCode(string input)
            {
                return Transform(input, "MACHINECODE");
            }

            /// <summary>
            /// 生成哈希值
            /// </summary>
            /// <param name="input">接受输入字符串和哈希算法名称，返回一个哈希字符串</param>
            /// <param name="hashAlgorithm"></param>
            /// <returns></returns>
            //private static string GenerateHash(string input, string hashAlgorithm)
            //{
            //    using (HashAlgorithm algorithm = hashAlgorithm switch
            //    {
            //        "MD5" => MD5.Create(),
            //        "SHA1" => SHA1.Create(),
            //        _ => throw new NotSupportedException("Unsupported hash algorithm"),
            //    })
            //    {
            //        byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            //        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            //    }
            //}

            /// <summary>
            /// 生成哈希值
            /// </summary>
            /// <param name="input">接受输入字符串和哈希算法名称，返回一个哈希字符串</param>
            /// <param name="hashAlgorithm"></param>
            /// <returns></returns>
            private static string GenerateHash(string input, string hashAlgorithm)
            {
                HashAlgorithm algorithm = null;
                if (hashAlgorithm == "MD5")
                {
                    algorithm = MD5CryptoServiceProvider.Create();
                }
                else if (hashAlgorithm == "SHA1")
                {
                    //algorithm = SHA1.Create();
                    algorithm = SHA1CryptoServiceProvider.Create();
                }
                else
                {

                    /**
                     MD5CryptoServiceProvider和SHA1CryptoServiceProvider是.NET Framework中提供的实现，它们是不可变的并且是线程安全的。在.NET Core或.NET 5/6中，你应该使用MD5.Create()和SHA1.Create()，因为MD5CryptoServiceProvider和SHA1CryptoServiceProvider在这些新版本中已被弃用。
                     */
                    throw new NotSupportedException("Unsupported hash algorithm");
                }

                using (algorithm) // Ensure the algorithm is disposed after use
                {
                    byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();

                    //return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
