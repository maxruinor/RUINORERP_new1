using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace HLH.Lib.Security
{
    /// <summary>
    /// CryptoUtil 的摘要说明。
    /// </summary>
    public class CryptoUtil
    {

        //随机选8个字节既为密钥也为初始向量
        Byte[] byKey64 = { 42, 16, 93, 156, 78, 4, 218, 32 };
        Byte[] Iv64 = { 55, 103, 246, 79, 36, 99, 167, 3 };
        Byte[] byKey192 = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 250, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 };
        Byte[] Iv192 = { 55, 103, 246, 79, 36, 99, 167, 3, 42, 5, 62, 83, 184, 7, 209, 13, 145, 23, 200, 58, 173, 10, 121, 222 };

        public CryptoUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 标准的DES加密
        /// </summary>
        /// <param name="str_Sql">标准的DES加密</param>
        public String Encrypt(String strText)
        {
            // Byte[] byKey = {};
            //Byte[] IV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
            try
            {
                //byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 标准的DES解密
        /// </summary>
        /// <param name="str_Sql">标准的DES解密</param>
        public String Decrypt(String strText)
        {
            // Byte[] byKey = {};
            // Byte[] IV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
            Byte[] inputByteArray = new byte[strText.Length];
            try
            {
                //byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
