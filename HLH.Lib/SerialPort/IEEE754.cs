using HLH.Lib.Helper;
using System;
using System.Text;

namespace IPS.Lib
{
    public class IEEE754
    {
        //http://topic.csdn.net/t/20050705/12/4123838.html

        // 对于大小为32-bit的浮点数（32-bit为单精度，64-bit浮点数为双精度，80-bit为扩展精度浮点数），     
        // 1、其第31   bit为符号位，为0则表示正数，反之为复数，其读数值用s表示；     
        // 2、第30～23   bit为幂数，其读数值用e表示；     
        // 3、第22～0   bit共23   bit作为系数，视为二进制纯小数，假定该小数的十进制值为x；     
        //   
        // 则按照规定，该浮点数的值用十进制表示为：     
        // ＝   (-1)^s     *   (1   +   x)   *   2^(e   -   127)     
        //   
        // 对于49E48E68来说，     
        // 1、其第31   bit为0，即s   =   0     
        // 2、第30～23   bit依次为100   1001   1，读成十进制就是147，即e   =   147。     
        // 3、第22～0   bit依次为110   0100   1000   1110   0110   1000，也就是二进制的纯小数0.110   0100   1000   1110   0110   1000，其十进制形式为0.78559589385986328125，即x   =   0.78559589385986328125。     
        //   
        // 这样，该浮点数的十进制表示     
        //       =　(-1)^s     *   (1   +   x)   *   2^(e   -   127)     
        // =　(-1)^0     *   (1+   0.78559589385986328125)   *   2^(147-127)     
        // =   1872333     
        //   
        // 你可以用windows自带的计算器算一下。     
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
                int m_s = 0; //   数符   
                int m_e = 0; //   阶   
                double m_x = 0; //   小数部分   
                double m_re = 0; //   计算结果   
                //Lib.log4netHelper.error("第一次strBase16:" + strBase16.ToString());
                strTemp = strBase16.Substring(0, 2);
                temp = Convert.ToInt32(strTemp, 16) & 0x80;
                if (temp == 128) m_s = 1;
                //Lib.log4netHelper.error("第二次strBase16:" + strBase16.ToString());
                strTemp = strBase16.Substring(0, 3);
                temp = Convert.ToInt32(strTemp, 16) & 0x7f8;
                m_e = Convert.ToInt32(temp / Math.Pow(2, 3));
                //Lib.log4netHelper.error("第三次strBase16:" + strBase16.ToString());
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
                log4netHelper.error("strHex的值:" + strHex + "|" + ex.StackTrace.ToString(), ex);
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
            int m_s = 0;	 //   数符 
            int m_e = 0;	 //   阶 
            double m_x = 0;	//   小数部分 
            //	 double   m_re   =   0   ;	//   计算结果 
            double absFloatValue = Math.Abs(floatValue);

            if (floatValue < 0)
            {
                m_s = 1;	//   m_s   =   1 
            }
            //   共   31~0   位 
            Base16 += Convert.ToInt64(m_s * Math.Pow(2, 31));	 //   首位，左移31位，到31位处 

            m_e = (int)Math.Log(absFloatValue, 2) + 127;
            Base16 += Convert.ToInt64(m_e * Math.Pow(2, 23));	 //   首位，左移23个，到31~23位处 

            m_x = absFloatValue / Math.Pow(2, m_e - 127) - 1;
            m_x = m_x * Math.Pow(2, 23);	 //首位，左移22个，从-1位开始的段，到22~0位处 
            Base16 += Convert.ToInt64(m_x);


            return Base16.ToString("X8 ");
        }


    }

    public class IEEETest
    {
        // <summary>
        /// 将二进制值转ASCII格式十六进制字符串
        /// </summary>
        /// <param name="data">二进制值</param>
        /// <param name="length">定长度的二进制</param>
        /// <returns>ASCII格式十六进制字符串</returns>
        public static string toHexString(int data, int length)
        {
            string result = "";
            if (data > 0)
                result = Convert.ToString(data, 16).ToUpper();
            if (result.Length < length)
            {
                // 位数不够补0
                StringBuilder msg = new StringBuilder(0);
                msg.Length = 0;
                msg.Append(result);
                for (; msg.Length < length; msg.Insert(0, "0")) ;
                result = msg.ToString();
            }
            return result;
        }

        /// <summary>
        /// 将浮点数转ASCII格式十六进制字符串（符合IEEE-754标准（32））
        /// </summary>
        /// <param name="data">浮点数值</param>
        /// <returns>十六进制字符串</returns>
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
        /// 将ASCII格式十六进制字符串转浮点数（符合IEEE-754标准（32））
        /// </summary>
        /// <param name="data">十六进制字符串</param>
        /// <returns>浮点数值</returns>
        public static float intStringToFloat(String data)
        {
            if (data.Length < 8 || data.Length > 8)
            {
                //throw new NotEnoughDataInBufferException(data.length(), 8);
                throw (new ApplicationException("缓存中的数据不完整。"));
            }
            else
            {
                byte[] intBuffer = new byte[4];
                // 将16进制串按字节逆序化（一个字节2个ASCII码）
                for (int i = 0; i < 4; i++)
                {
                    intBuffer[i] = Convert.ToByte(data.Substring((3 - i) * 2, 2), 16);
                }
                return BitConverter.ToSingle(intBuffer, 0);
            }
        }
    }


}
