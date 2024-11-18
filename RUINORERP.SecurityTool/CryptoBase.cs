using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityCore
{
    /// <summary>
    /// ���������
    /// </summary>
     class CryptoBase
    {
        /// <summary>
        /// ����ת�����������ֽ�����ַ���֮���ת����Ĭ��Ϊ��������
        /// </summary>
        static public Encoding Encode = Encoding.Default;
        public enum EncoderMode { Base64Encoder, HexEncoder };

        /// <summary>
        /// ������ģʽ���ַ�������
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">����</param>
        /// <param name="em">����ģʽ</param>
        /// <returns>���ܺ󾭹�������ַ���</returns>
        public String Encrypt(String data, String pass, CryptoBase.EncoderMode em)
        {
            if (data == null || pass == null) return null;
            if (em == EncoderMode.Base64Encoder)
                return Convert.ToBase64String(EncryptEx(Encode.GetBytes(data), pass));
            else
                return ByteToHex(EncryptEx(Encode.GetBytes(data), pass));
        }

        /// <summary>
        /// ������ģʽ���ַ�������
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">����</param>
        /// <param name="em">����ģʽ</param>
        /// <returns>����</returns>
        public String Decrypt(String data, String pass, CryptoBase.EncoderMode em)
        {
            if (data == null || pass == null) return null;
            if (em == EncoderMode.Base64Encoder)
                return Encode.GetString(DecryptEx(Convert.FromBase64String(data), pass));
            else
                return Encode.GetString(DecryptEx(HexToByte(data), pass));
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">����</param>
        /// <returns>���ܺ󾭹�Ĭ�ϱ�����ַ���</returns>
        public String Encrypt(String data, String pass)
        {
            return Encrypt(data, pass, EncoderMode.Base64Encoder);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">Ҫ���ܵľ������������</param>
        /// <param name="pass">����</param>
        /// <returns>����</returns>
        public String Decrypt(String data, String pass)
        {
            return Decrypt(data, pass, EncoderMode.Base64Encoder);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">��Կ</param>
        /// <returns>����</returns>
        virtual public Byte[] EncryptEx(Byte[] data, String pass) { return null; }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">����</param>
        /// <returns>����</returns>
        virtual public Byte[] DecryptEx(Byte[] data, String pass) { return null; }

        static public Byte[] HexToByte(String szHex)
        {
            // ����ʮ�����ƴ���һ���ֽ�
            Int32 iLen = szHex.Length;
            if (iLen <= 0 || 0 != iLen % 2)
            {
                return null;
            }
            Int32 dwCount = iLen / 2;
            UInt32 tmp1, tmp2;
            Byte[] pbBuffer = new Byte[dwCount];
            for (Int32 i = 0; i < dwCount; i++)
            {
                tmp1 = (UInt32)szHex[i * 2] - (((UInt32)szHex[i * 2] >= (UInt32)'A') ? (UInt32)'A' - 10 : (UInt32)'0');
                if (tmp1 >= 16) return null;
                tmp2 = (UInt32)szHex[i * 2 + 1] - (((UInt32)szHex[i * 2 + 1] >= (UInt32)'A') ? (UInt32)'A' - 10 : (UInt32)'0');
                if (tmp2 >= 16) return null;
                pbBuffer[i] = (Byte)(tmp1 * 16 + tmp2);
            }
            return pbBuffer;
        }

        static public String ByteToHex(Byte[] vByte)
        {
            if (vByte == null || vByte.Length < 1) return null;
            StringBuilder sb = new StringBuilder(vByte.Length * 2);
            for (int i = 0; i < vByte.Length; i++)
            {
                if ((UInt32)vByte[i] < 0) return null;
                UInt32 k = (UInt32)vByte[i] / 16;
                sb.Append((Char)(k + ((k > 9) ? 'A' - 10 : '0')));
                k = (UInt32)vByte[i] % 16;
                sb.Append((Char)(k + ((k > 9) ? 'A' - 10 : '0')));
            }
            return sb.ToString();
        }
    }

    interface ICrypto
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">��Կ</param>
        /// <returns>����</returns>
        Byte[] EncryptEx(Byte[] data, String pass);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="pass">����</param>
        /// <returns>����</returns>
        Byte[] DecryptEx(Byte[] data, String pass);
    }
}

