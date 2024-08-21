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
    /// �����̰�����
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
        /// ͨ��COM����ȡCookie���ݡ�(
        /// </summary>
        /// <param name="url">��ǰ��ַ��</param>
        /// <param name="cookieName">CookieName.</param>
        /// <param name="cookieData">���ڱ���Cookie Data��<see cref="StringBuilder"/>ʵ����</param>
        /// <param name="size">Cookie��С��</param>
        /// <returns>����ɹ��򷵻�<c>true</c>,���򷵻�<c>false</c>��</returns>
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(
          string url, string cookieName,
          StringBuilder cookieData, ref int size);
        /// <summary>
        /// ��ȡ��ǰ<see cref="Uri"/>��<see cref="CookieContainer"/>ʵ����
        /// </summary>
        /// <param name="uri">��ǰ<see cref="Uri"/>��ַ��</param>
        /// <returns>��ǰ<see cref="Uri"/>��<see cref="CookieContainer"/>ʵ����</returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;

            // ����Cookie���ݵĴ�С��
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
          ref datasize))
            {
                if (datasize < 0)
                    return null;

                // ȷ�����㹻��Ŀռ�������Cookie���ݡ�
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


        #region ������ͨ�Լ��

        [DllImport("sensapi.dll")]
        private extern static bool IsNetworkAlive(out int connectionDescription);


        /// <summary>
        /// ������صĽ���а���"����",��Ϊ��ͨ״̬
        /// </summary>
        /// <returns></returns>
        public static string Fun_IsNetworkAlive()
        {
            int NETWORK_ALIVE_LAN = 0;
            int NETWORK_ALIVE_WAN = 2;
            int NETWORK_ALIVE_AOL = 4;

            string outPut = null;
            int flags;//������ʽ 
            bool m_bOnline = true;//�Ƿ����� 

            m_bOnline = IsNetworkAlive(out flags);
            if (m_bOnline)//����   
            {
                if ((flags & NETWORK_ALIVE_LAN) == NETWORK_ALIVE_LAN)
                {
                    outPut = "���ߣ�NETWORK_ALIVE_LAN\n";
                }
                if ((flags & NETWORK_ALIVE_WAN) == NETWORK_ALIVE_WAN)
                {
                    outPut = "���ߣ�NETWORK_ALIVE_WAN\n";
                }
                if ((flags & NETWORK_ALIVE_AOL) == NETWORK_ALIVE_AOL)
                {
                    outPut = "���ߣ�NETWORK_ALIVE_AOL\n";
                }
            }
            else
            {
                outPut = "����\n";
            }

            return outPut;
        }

        #endregion




        #region ȡ�ñ���ip

        /// <summary>
        /// ��ȡ��ǰʹ�õ�IP
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
        /// ��ȡ������DNS
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
        /// ����һ������̨���򲢷��������������
        /// </summary>
        /// <param name="filename">������</param>
        /// <param name="arguments">�������</param>
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
        /// ��10����������ʽת����127.0.0.1��ʽ��IP��ַ 
        /// </summary>
        /// <param name="longIP"></param>
        /// <returns></returns>
        public static String long2IP(long longIP)
        {
            StringBuilder sb = new StringBuilder("");
            //ֱ������24λ 
            sb.Append(((longIP & 0xFFFFFFFF) >> 24).ToString());
            sb.Append(".");
            //����8λ��0��Ȼ������16λ 
            sb.Append(((longIP & 0x00FFFFFF) >> 16).ToString());
            sb.Append(".");
            sb.Append(((longIP & 0x0000FFFF) >> 8).ToString());
            sb.Append(".");
            sb.Append((longIP & 0x000000FF).ToString());
            return sb.ToString();
        }


        #endregion



        /// <summary>  
        /// �Ƿ��� Ping ָͨ��������   C#�ж�����״̬����   
        /// </summary>  
        /// <param name="ip">ip ��ַ��������������</param>  
        /// <returns>true ͨ��false ��ͨ</returns>  
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
