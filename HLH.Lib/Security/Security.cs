using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurityCore
{
    /// 
    /// Security 的摘要说明。
    /// 类中URL及账号加密使用了不同的KEY。调用URL加密过程如下： 
    ///EIP.Framework.Security objSecurity = new EIP.Framework.Security(); 
    ///objSecurity.EncryptQueryString(""待加密的字符串""); 
    ///解密:objSecurity.DecryptQueryString(""传递过来的参数);
    /// 
    class UrlQueryStringSecurity
    {
        string _QueryStringKey = "12345"; //URL传输参数加密Key 
        string _PassWordKey = "12345"; //PassWord加密Key 

        public UrlQueryStringSecurity()
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
        }

        /// 
        /// 加密URL传输的字符串 
        /// 
        public string EncryptQueryString(string QueryString)
        {
            return Encrypt(QueryString, _QueryStringKey);
        }

        /// 
        /// 解密URL传输的字符串 
        /// 
        public string DecryptQueryString(string QueryString)
        {
            return Decrypt(QueryString, _QueryStringKey);
        }

        /// 
        /// 加密账号口令 
        /// 
        public string EncryptPassWord(string PassWord)
        {
            return Encrypt(PassWord, _PassWordKey);
        }
        /// 
        /// 解密账号口令 
        /// 
        public string DecryptPassWord(string PassWord)
        {
            return Decrypt(PassWord, _PassWordKey);
        }

        /// 
        /// DEC 加密过程 
        /// 
        public string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte数组中 

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[] inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt); 

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey); //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
            MemoryStream ms = new MemoryStream(); //使得输入密码必须输入英文文本 
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// 
        /// DEC 解密过程 
        ///
        public string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量，此值重要，不能修改 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder(); //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象 

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        /// 
        /// 检查己加密的字符串是否与原文相同 
        /// 
        public bool ValidateString(string EnString, string FoString, int Mode)
        {
            switch (Mode)
            {
                default:
                case 1:
                    if (Decrypt(EnString, _QueryStringKey) == FoString.ToString())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case 2:
                    if (Decrypt(EnString, _PassWordKey) == FoString.ToString())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        //以下为另外一种加解密方式，比较简单 

        public string DecryptStr(string rs) //顺序减1解码
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] - 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }

        public string EncryptStr(string rs) //倒序加1加密
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] + 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }
    }
}

