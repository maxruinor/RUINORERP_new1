using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server
{
    /// <summary>
    /// 系统保护的一些数据和方法
    /// </summary>
    public class SystemProtectionData
    {

        public SystemProtectionData()
        {


            UserOnlineCount = 1;
            ExpirationDate = DateTime.MinValue;

        }

        //取电脑的CPUID
        public static string GetCpuID()
        {
            return "";
        }

        /// <summary>
        /// 取电脑的第一个物理网卡的ID
        /// </summary>
        public static string GetMAC()
        {
            return "";
        }

        /// <summary>
        /// 取电脑的第一个物理硬盘的ID
        /// </summary>
        /// <returns></returns>
        public static string GetDiskID()
        {
            return "";
        }





        public static bool IsSystemProtectionEnabled
        {
            get
            {
                return false;
            }
        }

        public static bool IsSystemProtectionEnabledForUser(string userName)
        {
            return false;
        }


        //限制在线的人数
        public int UserOnlineCount { get; set; } = 0;

        //软件使用到期的日期
        public DateTime ExpirationDate { get; set; } = DateTime.MinValue;

        //获取当前电脑的时间
        public static DateTime GetLocalTime()
        {
            return DateTime.Now;
        }

    }
}
