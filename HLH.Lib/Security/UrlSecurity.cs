using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HLH.Lib.Security
{
    public class UrlSecurity
    {
        #region 解密字符串

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string[] UrlDecrypt(string UrlValues)
        {
            //随机选8个字节既为密钥也为初始向量
            Byte[] byKey64 = { 10, 20, 30, 40, 50, 60, 70, 80 };
            Byte[] Iv64 = { 11, 22, 33, 44, 55, 66, 77, 85 };
            try
            {
                UrlValues = UrlValues.Replace(" ", "+");
                Byte[] inputByteArray = new byte[UrlValues.Length];

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(UrlValues);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;

                ////+号通过url传递变成了空格。
                string decryptResult = encoding.GetString(ms.ToArray());
                //DecryptString(string)解密字符串
                string delimStr = ",";
                char[] delimiterArray = delimStr.ToCharArray();
                string[] urlInfoArray = null;
                urlInfoArray = decryptResult.Split(delimiterArray);
                return urlInfoArray;
            }
            catch (Exception ex)
            {
                AutoRecordLog("激活用户出错!" + ex.Message);
                //log4netHelper.error("激活用户出错",ex);
                //MessageBoxHelper.MessageBox("无法激活用户");
            }
            return null;
        }

        #endregion 字符串加密

        #region 自动写入日志

        /// <summary>
        /// 自动写入日志
        /// </summary>
        static void AutoRecordLog(string message)
        {
            //指定日志文件的目录

            string fileLogPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\User";
            //如果不存在则创建目录
            if (!Directory.Exists(fileLogPath))
                Directory.CreateDirectory(fileLogPath);
            string fileLogName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //定义文件信息对象
            string dd = fileLogPath + "\\" + fileLogName;
            FileInfo finfo = new FileInfo(fileLogPath + "\\" + fileLogName);

            //创建只写文件流

            using (FileStream fs = finfo.OpenWrite())
            {
                //根据上面创建的文件流创建写数据流

                StreamWriter strwriter = new StreamWriter(fs);
                //设置写数据流的起始位置为文件流的末尾

                strwriter.BaseStream.Seek(0, SeekOrigin.End);
                //写入错误发生时间

                strwriter.WriteLine("记录时间: " + DateTime.Now.ToString());
                //写入日志内容并换行
                strwriter.WriteLine(message);
                //写入间隔符

                strwriter.WriteLine("---------------------------------------------");
                strwriter.WriteLine();
                //清空缓冲区内容，并把缓冲区内容写入基础流

                strwriter.Flush();
                //关闭写数据流

                strwriter.Close();
                fs.Close();
            }
        }

        #endregion 自动写入日志

        #region 解密

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strText">待加密的字符串</param>
        /// <returns>string</returns>
        public static string UrlEncrypt(string strText)
        {
            Byte[] Iv64 = { 11, 22, 33, 44, 55, 66, 77, 85 };
            Byte[] byKey64 = { 10, 20, 30, 40, 50, 60, 70, 80 };

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey64, Iv64), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        #endregion 解密
    }
}
