using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Configuration;


namespace SecurityCore
{
    class CreateRegisterCode
    {

        ///   <summary>   
        ///   加密数据   
        ///   </summary>   a
        ///   <param   name="Text"></param>   
        ///   <param   name="sKey"></param>   
        ///   <returns></returns>   
        private string EnText(string Text, string sKey)
        {
            StringBuilder ret = new StringBuilder();
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(Text);
                //通过两次哈希密码设置对称算法的初始化向量   
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8), "sha1").Substring(0, 8));
                //des.Key = ASCIIEncoding.ASCII.GetBytes(System.Security.Cryptography.HashAlgorithm.Create();Security.FormsAuthentication.HashPasswordForStoringInConfigFile(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8), "sha1").Substring(0, 8));
                //通过两次哈希密码设置算法的机密密钥   
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile  (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8), "md5").Substring(0, 8));
                
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

        ///   <summary>   
        ///   解密数据   
        ///   </summary>   
        ///   <param   name="Text"></param>   
        ///   <param   name="sKey"></param>   
        ///   <returns></returns>   
        private string DeText(string Text, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();   //定义DES加密对象   
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                //通过两次哈希密码设置对称算法的初始化向量   
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile
                (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8), "sha1").Substring(0, 8));
                //通过两次哈希密码设置算法的机密密钥   
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile
                (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8), "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();//定义内存流   
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);//定义加密流   
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        ///   <summary>   
        ///   将加密的字符串转换为注册码形式   
        ///   </summary>   
        ///   <param   name="input">要加密字符串</param>   
        ///   <returns>装换后的字符串</returns>   
        public string transform(string input, string skey)
        {
            string transactSn = string.Empty;
            if (input == "")
            {
                return transactSn;
            }
            string initSn = string.Empty;
            try
            {
                initSn = this.EnText(this.EnText(input, skey), skey).ToString();
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
    }
}
