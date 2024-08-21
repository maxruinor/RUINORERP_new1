using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HLH.Lib.Security
{
    public class UrlSecurity
    {
        #region �����ַ���

        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string[] UrlDecrypt(string UrlValues)
        {
            //���ѡ8���ֽڼ�Ϊ��ԿҲΪ��ʼ����
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

                ////+��ͨ��url���ݱ���˿ո�
                string decryptResult = encoding.GetString(ms.ToArray());
                //DecryptString(string)�����ַ���
                string delimStr = ",";
                char[] delimiterArray = delimStr.ToCharArray();
                string[] urlInfoArray = null;
                urlInfoArray = decryptResult.Split(delimiterArray);
                return urlInfoArray;
            }
            catch (Exception ex)
            {
                AutoRecordLog("�����û�����!" + ex.Message);
                //log4netHelper.error("�����û�����",ex);
                //MessageBoxHelper.MessageBox("�޷������û�");
            }
            return null;
        }

        #endregion �ַ�������

        #region �Զ�д����־

        /// <summary>
        /// �Զ�д����־
        /// </summary>
        static void AutoRecordLog(string message)
        {
            //ָ����־�ļ���Ŀ¼

            string fileLogPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\User";
            //����������򴴽�Ŀ¼
            if (!Directory.Exists(fileLogPath))
                Directory.CreateDirectory(fileLogPath);
            string fileLogName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //�����ļ���Ϣ����
            string dd = fileLogPath + "\\" + fileLogName;
            FileInfo finfo = new FileInfo(fileLogPath + "\\" + fileLogName);

            //����ֻд�ļ���

            using (FileStream fs = finfo.OpenWrite())
            {
                //�������洴�����ļ�������д������

                StreamWriter strwriter = new StreamWriter(fs);
                //����д����������ʼλ��Ϊ�ļ�����ĩβ

                strwriter.BaseStream.Seek(0, SeekOrigin.End);
                //д�������ʱ��

                strwriter.WriteLine("��¼ʱ��: " + DateTime.Now.ToString());
                //д����־���ݲ�����
                strwriter.WriteLine(message);
                //д������

                strwriter.WriteLine("---------------------------------------------");
                strwriter.WriteLine();
                //��ջ��������ݣ����ѻ���������д�������

                strwriter.Flush();
                //�ر�д������

                strwriter.Close();
                fs.Close();
            }
        }

        #endregion �Զ�д����־

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="strText">�����ܵ��ַ���</param>
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

        #endregion ����
    }
}
