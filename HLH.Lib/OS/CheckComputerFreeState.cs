using System;
using System.Runtime.InteropServices;

namespace HLH.Lib.OS
{
    //https://docs.microsoft.com/zh-cn/dotnet/api/system.environment.tickcount?redirectedfrom=MSDN&view=netcore-3.1#System_Environment_TickCount
    public class CheckComputerFreeState
    {
        /// <summary>
        /// 创建结构体用于返回捕获时间
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            /// <summary>
            /// 设置结构体块容量
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            /// <summary>
            /// 抓获的时间
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// 获取键盘和鼠标没有操作的时间 这样不准确， 如果电脑一直开着。会出现负数
        /// </summary>
        /// <returns>用户上次使用系统到现在的时间间隔，单位为秒</returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo))
            {
                return 0;
            }
            else
            {
                var count = Environment.TickCount & Int32.MaxValue - (long)(vLastInputInfo.dwTime & Int32.MaxValue);
                var icount = count / 1000;
                return icount;
            }
        }


        public static long GetLastInputTimeMinute()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo))
            {
                return 0;
            }
            else
            {
                var count = Environment.TickCount - (long)vLastInputInfo.dwTime;
                var icount = count / (1000 * 60);
                return icount;
            }
        }

    }
}

