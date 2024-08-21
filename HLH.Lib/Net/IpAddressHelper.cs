using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace HLH.Lib.Net
{
    public static class IpAddressHelper
    {


        public static string GetLocIP()
        {
            string strip = string.Empty;
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in ip)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    strip += address.ToString() + "\r\n";
                }
            }
            return strip;
        }

        /// <summary>
        /// 测试是否为IP，如果是域名则转为IP
        /// </summary>
        /// <param name="DeviceIP"></param>
        /// <returns></returns>
        public static string GetIP(string DeviceIP)
        {
            //判断输入的是否是IP          
            Regex rx = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))");
            if (!rx.IsMatch(DeviceIP))
            {
                //Dns.GetHostAddresses()返回的是一个IPAddress类型的集合,表示这个域名下的所有的IP地址
                IPAddress[] IPs = Dns.GetHostAddresses(DeviceIP);
                DeviceIP = IPs[0].ToString();
            }
            return DeviceIP;
        }

        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <example>IPAddress.Parse("127.0.0.1").ipaddressToint()</example>
        /// <returns>整数</returns>
        public static long ipaddressToint(IPAddress ip)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes())
            {
                o += (long)f << 8 * x--;
            }
            return o;

        }



        //1.将IP地址转化为整形 
        //System.Net.IPAddress   ip=   System.Net.IPAddress.Parse( "192.168.1.2 "); 
        //int   iplong   =(int)   ip.Address; 

        //2.将整型变为IP地址 
        //  int   j=23543; 
        //System.Net.IPAddress   ip1=   System.Net.IPAddress.Parse(j.ToString()); 
        //this.Label1.Text=ip1.ToString();



        /// <summary>
        /// 将整数转为IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress longToIpAddress(long l)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }
            return new IPAddress(b);
        }
    }
}
