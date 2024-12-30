using System;
using System.Security.Cryptography;
using System.Text;

namespace HLH.Lib.Security
{

    /// <summary>
    /// ע����������
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
            /// ��������
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
                    // ͨ�����ι�ϣ�������öԳ��㷨�ĳ�ʼ������
                    //des.Key = GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray();
                    des.Key = Encoding.UTF8.GetBytes(GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray());

                    // ͨ�����ι�ϣ���������㷨�Ļ�����Կ
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
            /// ��������
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
                    // ͨ�����ι�ϣ�������öԳ��㷨�ĳ�ʼ������
                    //des.Key = GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray();
                    des.Key = Encoding.UTF8.GetBytes(GenerateHash(sKey, "MD5").Substring(0, 8).Take(8).ToArray());

                    // ͨ�����ι�ϣ���������㷨�Ļ�����Կ
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
            /// �����ܵ��ַ���ת��Ϊע������ʽ
            /// </summary>
            /// <param name="input">Ҫ�����ַ���</param>
            /// <returns>װ������ַ���</returns>
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
            /// ���ɻ�����
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static string CreateMachineCode(string input)
            {
                return Transform(input, "MACHINECODE");
            }

            /// <summary>
            /// ���ɹ�ϣֵ
            /// </summary>
            /// <param name="input">���������ַ����͹�ϣ�㷨���ƣ�����һ����ϣ�ַ���</param>
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
            /// ���ɹ�ϣֵ
            /// </summary>
            /// <param name="input">���������ַ����͹�ϣ�㷨���ƣ�����һ����ϣ�ַ���</param>
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
                     MD5CryptoServiceProvider��SHA1CryptoServiceProvider��.NET Framework���ṩ��ʵ�֣������ǲ��ɱ�Ĳ������̰߳�ȫ�ġ���.NET Core��.NET 5/6�У���Ӧ��ʹ��MD5.Create()��SHA1.Create()����ΪMD5CryptoServiceProvider��SHA1CryptoServiceProvider����Щ�°汾���ѱ����á�
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
