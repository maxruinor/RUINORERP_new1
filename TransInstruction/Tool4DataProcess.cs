using System;
using System.Text;

namespace TransInstruction
{
    /// <summary>
    /// 数据处理的工具  很多方法来自Tools
    /// </summary>
    public static class Tool4DataProcess
    {
         



        public static object[] Sclie(object[] buff, int start, int size)
        {
            object[] ret = new object[size];
            for (int i = 0; i < size; i++)
            {
                ret[i] = buff[start + i];
            }
            return ret;
        }



        public static byte[] StrToBytes(string val)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (val == null)
            {
                return new byte[0];

            }

            var arr = Encoding.UTF8.GetBytes(val);
            return arr;
            //return System.Text.Encoding.GetEncoding("GB2312").GetBytes(val);
        }

        /// <summary> 
        /// 字节数组转16进制字符串 空格分割开
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes, bool speaceSeparator)
        {

            StringBuilder ret = new StringBuilder();
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret.Append(bytes[i].ToString("X2"));
                    if (speaceSeparator == true) ret.Append(" ");
                }
            }
            return ret.ToString();
        }


 


        /// <summary>
        /// 将16进制的字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }


        //public static String str2HexStr(String str)
        //{
        //    char[] chars = "0123456789ABCDEF".ToCharArray();
        //    StringBuilder sb = new StringBuilder("");
        //    byte[] bs = str.GetBytes();
        //    int bit;
        //    for (int i = 0; i < bs.Length; i++)
        //    {
        //        bit = (bs[i] & 0x0f0) >> 4;
        //        sb.Append(chars[bit]);
        //        bit = bs[i] & 0x0f;
        //        sb.Append(chars[bit]);
        //    }
        //    return sb.ToString().trim();
        //}


        /// <summary>
        /// 十六进制的字串转为byte[]数组空格可有可无 01 AB D3 28 33 55 to byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }






 
        /// <summary>
        /// 将字符串转为HEX值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">从去掉空格后的第几个字符开始</param>
        /// <param name="size"></param>
        /// <param name="ret"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool StrToHex(string str, int start, int size, out byte[] ret, out string msg)
        {
            msg = "";
            str = str.Replace(" ", "");
            var index = 0;
            str = str.Substring(start);
            ret = new byte[str.Length / 2];
            var bys = System.Text.Encoding.Default.GetBytes(str);
            var by2 = new byte[2];
            for (int i = 0; i < bys.Length / 2; i++)
            {
                try
                {
                    by2[0] = bys[i * 2];
                    by2[0 + 1] = bys[i * 2 + 1];
                    ret[index++] = Convert.ToByte(System.Text.Encoding.Default.GetString(by2), 16);
                }
                catch (Exception ex)
                {
                    msg += ex.Message;
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            // DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            // long lTime = long.Parse(timeStamp + "0000000");
            // TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);

            //DateTime startTime = System.TimeZoneInfo.Local.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime startTime = System.TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), System.TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp);
            //  TimeSpan toNow = new TimeSpan(lTime);
            DateTime nowTime = startTime.AddMilliseconds(lTime);
            return nowTime;
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime startTime = System.TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), System.TimeZoneInfo.Local);
            DateTime nowTime = time;
            //DateTime timeStamp = DateTime.Parse("2006-01-02 15:04:05");
            //startTime = timeStamp;
            //DateTime nowTime = DateTime.Now;
            //DateTime timeStamp = DateTime.Parse("2013-11-25 20:23:30");
            //以上面这个为标准的话，这个是结果：1385382210000
            // DateTime nowTime = timeStamp;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);

            return unixTime;
            //  System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            //  return (int)(time - startTime).TotalSeconds;
        }


 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="by"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="speace"></param>
        /// <returns></returns>
        public static string Hex2Str(byte[] by, int start, int size, bool speace)
        {
            if (by.Length == 0)
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            if (size == -1)
            {
                size = by.Length;
            }
            for (int i = 0; i < size; i++)
            {
                ret.Append(by[i + start].ToString("X2"));
                if (speace == true) ret.Append(" ");
            }
            return ret.ToString();
        }

        /// <summary>
        /// byte[] to string
        /// </summary>
        /// <param name="bys"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string toStr(byte[] bys, int start = 0, int count = -1)
        {
            if (count <= 0)
            {
                count = bys.Length;
            }
            //return System.Text.Encoding.GetEncoding("GB2312").GetString(bys, start, count);
            return System.Text.Encoding.UTF8.GetString(bys, start, count);
        }
 






    }



}
