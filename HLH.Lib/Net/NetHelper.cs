using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace HLH.Lib.Net
{
    /// <summary>
    /// 网络编程帮助类
    /// </summary>
    public class NetHelper
    {





        public static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
            System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }

        /// <summary>
        /// 通过COM来获取Cookie数据。(
        /// </summary>
        /// <param name="url">当前网址。</param>
        /// <param name="cookieName">CookieName.</param>
        /// <param name="cookieData">用于保存Cookie Data的<see cref="StringBuilder"/>实例。</param>
        /// <param name="size">Cookie大小。</param>
        /// <returns>如果成功则返回<c>true</c>,否则返回<c>false</c>。</returns>
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(
          string url, string cookieName,
          StringBuilder cookieData, ref int size);
        /// <summary>
        /// 获取当前<see cref="Uri"/>的<see cref="CookieContainer"/>实例。
        /// </summary>
        /// <param name="uri">当前<see cref="Uri"/>地址。</param>
        /// <returns>当前<see cref="Uri"/>的<see cref="CookieContainer"/>实例。</returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;

            // 定义Cookie数据的大小。
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
          ref datasize))
            {
                if (datasize < 0)
                    return null;

                // 确信有足够大的空间来容纳Cookie数据。
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData,
                  ref datasize))
                    return null;
            }


            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }


        #region 网络连通性检测

        [DllImport("sensapi.dll")]
        private extern static bool IsNetworkAlive(out int connectionDescription);


        /// <summary>
        /// 如果返回的结果中包括"在线",则为连通状态
        /// </summary>
        /// <returns></returns>
        public static string Fun_IsNetworkAlive()
        {
            int NETWORK_ALIVE_LAN = 0;
            int NETWORK_ALIVE_WAN = 2;
            int NETWORK_ALIVE_AOL = 4;

            string outPut = null;
            int flags;//上网方式 
            bool m_bOnline = true;//是否在线 

            m_bOnline = IsNetworkAlive(out flags);
            if (m_bOnline)//在线   
            {
                if ((flags & NETWORK_ALIVE_LAN) == NETWORK_ALIVE_LAN)
                {
                    outPut = "在线：NETWORK_ALIVE_LAN\n";
                }
                if ((flags & NETWORK_ALIVE_WAN) == NETWORK_ALIVE_WAN)
                {
                    outPut = "在线：NETWORK_ALIVE_WAN\n";
                }
                if ((flags & NETWORK_ALIVE_AOL) == NETWORK_ALIVE_AOL)
                {
                    outPut = "在线：NETWORK_ALIVE_AOL\n";
                }
            }
            else
            {
                outPut = "断线\n";
            }

            return outPut;
        }

        #endregion




        #region 取得本机ip

        /// <summary>
        /// 获取当前使用的IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            string result = RunApp("route", "print", true);
            Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            if (m.Success)
            {
                return m.Groups[2].Value;
            }
            else
            {
                try
                {
                    System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient();
                    c.Connect("www.baidu.com", 80);
                    string ip = ((System.Net.IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
                    c.Close();
                    return ip;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 获取本机主DNS
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryDNS()
        {
            string result = RunApp("nslookup", "", true);
            Match m = Regex.Match(result, @"\d+\.\d+\.\d+\.\d+");
            if (m.Success)
            {
                return m.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 运行一个控制台程序并返回其输出参数。
        /// </summary>
        /// <param name="filename">程序名</param>
        /// <param name="arguments">输入参数</param>
        /// <returns></returns>
        public static string RunApp(string filename, string arguments, bool recordLog)
        {
            try
            {
                if (recordLog)
                {
                    Trace.WriteLine(filename + " " + arguments);
                }
                Process proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    if (recordLog)
                    {
                        Trace.WriteLine(txt);
                    }
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                    return txt;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 将10进制整数形式转换成127.0.0.1形式的IP地址 
        /// </summary>
        /// <param name="longIP"></param>
        /// <returns></returns>
        public static String long2IP(long longIP)
        {
            StringBuilder sb = new StringBuilder("");
            //直接右移24位 
            sb.Append(((longIP & 0xFFFFFFFF) >> 24).ToString());
            sb.Append(".");
            //将高8位置0，然后右移16位 
            sb.Append(((longIP & 0x00FFFFFF) >> 16).ToString());
            sb.Append(".");
            sb.Append(((longIP & 0x0000FFFF) >> 8).ToString());
            sb.Append(".");
            sb.Append((longIP & 0x000000FF).ToString());
            return sb.ToString();
        }


        #endregion



        /// <summary>  
        /// 是否能 Ping 通指定的主机   C#判断网络状态代码   
        /// </summary>  
        /// <param name="ip">ip 地址或主机名或域名</param>  
        /// <returns>true 通，false 不通</returns>  
        public static bool Ping(string ip)
        {
            int timeout = 1000;
            string data = "Test Data!";
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
            options.DontFragment = true; byte[] buffer = Encoding.ASCII.GetBytes(data);
            System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success) return true; else return false;
        }

    }

}
