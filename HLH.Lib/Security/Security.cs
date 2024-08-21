using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurityCore
{
    /// 
    /// Security ��ժҪ˵����
    /// ����URL���ʺż���ʹ���˲�ͬ��KEY������URL���ܹ������£� 
    ///EIP.Framework.Security objSecurity = new EIP.Framework.Security(); 
    ///objSecurity.EncryptQueryString(""�����ܵ��ַ���""); 
    ///����:objSecurity.DecryptQueryString(""���ݹ����Ĳ���);
    /// 
    class UrlQueryStringSecurity
    {
        string _QueryStringKey = "12345"; //URL�����������Key 
        string _PassWordKey = "12345"; //PassWord����Key 

        public UrlQueryStringSecurity()
        {
            // 
            // TODO: �ڴ˴���ӹ��캯���߼� 
            // 
        }

        /// 
        /// ����URL������ַ��� 
        /// 
        public string EncryptQueryString(string QueryString)
        {
            return Encrypt(QueryString, _QueryStringKey);
        }

        /// 
        /// ����URL������ַ��� 
        /// 
        public string DecryptQueryString(string QueryString)
        {
            return Decrypt(QueryString, _QueryStringKey);
        }

        /// 
        /// �����ʺſ��� 
        /// 
        public string EncryptPassWord(string PassWord)
        {
            return Encrypt(PassWord, _PassWordKey);
        }
        /// 
        /// �����ʺſ��� 
        /// 
        public string DecryptPassWord(string PassWord)
        {
            return Decrypt(PassWord, _PassWordKey);
        }

        /// 
        /// DEC ���ܹ��� 
        /// 
        public string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //���ַ����ŵ�byte������ 

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[] inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt); 

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //�������ܶ������Կ��ƫ���� 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey); //ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes���� 
            MemoryStream ms = new MemoryStream(); //ʹ�����������������Ӣ���ı� 
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
        /// DEC ���ܹ��� 
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

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸� 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder(); //����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı���������� 

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        /// 
        /// ��鼺���ܵ��ַ����Ƿ���ԭ����ͬ 
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

        //����Ϊ����һ�ּӽ��ܷ�ʽ���Ƚϼ� 

        public string DecryptStr(string rs) //˳���1����
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

        public string EncryptStr(string rs) //�����1����
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

