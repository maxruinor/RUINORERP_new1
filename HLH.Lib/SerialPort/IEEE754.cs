using HLH.Lib.Helper;
using System;
using System.Text;

namespace IPS.Lib
{
    public class IEEE754
    {
        //http://topic.csdn.net/t/20050705/12/4123838.html

        // ���ڴ�СΪ32-bit�ĸ�������32-bitΪ�����ȣ�64-bit������Ϊ˫���ȣ�80-bitΪ��չ���ȸ���������     
        // 1�����31   bitΪ����λ��Ϊ0���ʾ��������֮Ϊ�����������ֵ��s��ʾ��     
        // 2����30��23   bitΪ�����������ֵ��e��ʾ��     
        // 3����22��0   bit��23   bit��Ϊϵ������Ϊ�����ƴ�С�����ٶ���С����ʮ����ֵΪx��     
        //   
        // ���չ涨���ø�������ֵ��ʮ���Ʊ�ʾΪ��     
        // ��   (-1)^s     *   (1   +   x)   *   2^(e   -   127)     
        //   
        // ����49E48E68��˵��     
        // 1�����31   bitΪ0����s   =   0     
        // 2����30��23   bit����Ϊ100   1001   1������ʮ���ƾ���147����e   =   147��     
        // 3����22��0   bit����Ϊ110   0100   1000   1110   0110   1000��Ҳ���Ƕ����ƵĴ�С��0.110   0100   1000   1110   0110   1000����ʮ������ʽΪ0.78559589385986328125����x   =   0.78559589385986328125��     
        //   
        // �������ø�������ʮ���Ʊ�ʾ     
        //       =��(-1)^s     *   (1   +   x)   *   2^(e   -   127)     
        // =��(-1)^0     *   (1+   0.78559589385986328125)   *   2^(147-127)     
        // =   1872333     
        //   
        // �������windows�Դ��ļ�������һ�¡�     
        public static string HexToFloat(string strHex)
        {
            string temprs = "0";
            try
            {

                if (strHex.Length > 8)
                {
                    strHex = strHex.Substring(0, 8);
                }

                if (strHex.Length < 8)
                {
                    strHex = strHex.PadLeft(8, '0');
                }

                string strBase16 = strHex;
                string strTemp = "";
                double temp = 0;
                int m_s = 0; //   ����   
                int m_e = 0; //   ��   
                double m_x = 0; //   С������   
                double m_re = 0; //   ������   
                //Lib.log4netHelper.error("��һ��strBase16:" + strBase16.ToString());
                strTemp = strBase16.Substring(0, 2);
                temp = Convert.ToInt32(strTemp, 16) & 0x80;
                if (temp == 128) m_s = 1;
                //Lib.log4netHelper.error("�ڶ���strBase16:" + strBase16.ToString());
                strTemp = strBase16.Substring(0, 3);
                temp = Convert.ToInt32(strTemp, 16) & 0x7f8;
                m_e = Convert.ToInt32(temp / Math.Pow(2, 3));
                //Lib.log4netHelper.error("������strBase16:" + strBase16.ToString());
                strTemp = strBase16.Substring(2, 6);
                temp = Convert.ToInt32(strTemp, 16) & 0x7fffff;
                m_x = temp / Math.Pow(2, 23);

                m_re = Math.Pow(-1, m_s) * (1 + m_x) * Math.Pow(2, m_e - 127);
                decimal mrs = 0;
                decimal.TryParse(m_re.ToString(), out mrs);
                temprs = decimal.Round(mrs, 5).ToString();

            }
            catch (Exception ex)
            {
                log4netHelper.error("strHex��ֵ:" + strHex + "|" + ex.StackTrace.ToString(), ex);
                temprs = "0";
            }
            return temprs;
            // return decimal.Round(Convert.ToDecimal(m_re), 5).ToString();
        }




        public static double IEEE754Float(byte[] data, int i)
        {
            int s;
            int e;
            double m;
            double m_dblReturn = 0;

            s = data[i + 2] & 128;
            e = (data[i + 2] & 127) * 2 + (data[i + 3] & 128) / 128;
            m = (Convert.ToDouble((data[i + 3] & 127)) * 65536 + Convert.ToDouble(data[i]) * 256 + Convert.ToDouble(data[i + 1]))

            / 8388608;
            m_dblReturn = Math.Pow((-1), s) * Math.Pow(2, (e - 127)) * (m + 1);

            return m_dblReturn;

        }

        public static double IEEE754Float(string p_strValue)
        {
            int s;
            int e;
            double m;
            double m_dblReturn = 0;

            byte[] data = new byte[4];

            data[0] = Convert.ToByte(p_strValue.Substring(0, 2), 16);
            data[1] = Convert.ToByte(p_strValue.Substring(2, 2), 16);
            data[2] = Convert.ToByte(p_strValue.Substring(4, 2), 16);
            data[3] = Convert.ToByte(p_strValue.Substring(6, 2), 16);

            s = data[2] & 128;
            e = (data[2] & 127) * 2 + (data[3] & 128) / 128;
            m = (Convert.ToDouble((data[3] & 127)) * 65536 + Convert.ToDouble(data[0]) * 256 + Convert.ToDouble(data[1])) /

            8388608;
            m_dblReturn = Math.Pow((-1), s) * Math.Pow(2, (e - 127)) * (m + 1);

            return m_dblReturn;

        }


        public static string IEEE754Float(double floatValue)
        {

            Int64 Base16 = 0x00000000;
            int m_s = 0;	 //   ���� 
            int m_e = 0;	 //   �� 
            double m_x = 0;	//   С������ 
            //	 double   m_re   =   0   ;	//   ������ 
            double absFloatValue = Math.Abs(floatValue);

            if (floatValue < 0)
            {
                m_s = 1;	//   m_s   =   1 
            }
            //   ��   31~0   λ 
            Base16 += Convert.ToInt64(m_s * Math.Pow(2, 31));	 //   ��λ������31λ����31λ�� 

            m_e = (int)Math.Log(absFloatValue, 2) + 127;
            Base16 += Convert.ToInt64(m_e * Math.Pow(2, 23));	 //   ��λ������23������31~23λ�� 

            m_x = absFloatValue / Math.Pow(2, m_e - 127) - 1;
            m_x = m_x * Math.Pow(2, 23);	 //��λ������22������-1λ��ʼ�ĶΣ���22~0λ�� 
            Base16 += Convert.ToInt64(m_x);


            return Base16.ToString("X8 ");
        }


    }

    public class IEEETest
    {
        // <summary>
        /// ��������ֵתASCII��ʽʮ�������ַ���
        /// </summary>
        /// <param name="data">������ֵ</param>
        /// <param name="length">�����ȵĶ�����</param>
        /// <returns>ASCII��ʽʮ�������ַ���</returns>
        public static string toHexString(int data, int length)
        {
            string result = "";
            if (data > 0)
                result = Convert.ToString(data, 16).ToUpper();
            if (result.Length < length)
            {
                // λ��������0
                StringBuilder msg = new StringBuilder(0);
                msg.Length = 0;
                msg.Append(result);
                for (; msg.Length < length; msg.Insert(0, "0")) ;
                result = msg.ToString();
            }
            return result;
        }

        /// <summary>
        /// ��������תASCII��ʽʮ�������ַ���������IEEE-754��׼��32����
        /// </summary>
        /// <param name="data">������ֵ</param>
        /// <returns>ʮ�������ַ���</returns>
        public static string floatToIntString(float data)
        {
            byte[] intBuffer = BitConverter.GetBytes(data);
            StringBuilder stringBuffer = new StringBuilder(0);
            for (int i = 0; i < intBuffer.Length; i++)
            {
                stringBuffer.Insert(0, toHexString(intBuffer[i] & 0xff, 2));
            }
            return stringBuffer.ToString();
        }

        /// <summary>
        /// ��ASCII��ʽʮ�������ַ���ת������������IEEE-754��׼��32����
        /// </summary>
        /// <param name="data">ʮ�������ַ���</param>
        /// <returns>������ֵ</returns>
        public static float intStringToFloat(String data)
        {
            if (data.Length < 8 || data.Length > 8)
            {
                //throw new NotEnoughDataInBufferException(data.length(), 8);
                throw (new ApplicationException("�����е����ݲ�������"));
            }
            else
            {
                byte[] intBuffer = new byte[4];
                // ��16���ƴ����ֽ����򻯣�һ���ֽ�2��ASCII�룩
                for (int i = 0; i < 4; i++)
                {
                    intBuffer[i] = Convert.ToByte(data.Substring((3 - i) * 2, 2), 16);
                }
                return BitConverter.ToSingle(intBuffer, 0);
            }
        }
    }


}
