using System;
using System.Runtime.InteropServices;

namespace HLH.Lib.HardWare
{
    public class SystemApiService
    {
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// 取得最后一次输入时间
        /// <para>毫秒</para>
        /// </summary>
        /// <returns></returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo)) return 0;
            return Environment.TickCount - (long)vLastInputInfo.dwTime;


            //    //获取系统的运行时间 
            //int systemUpTime = Environment.TickCount;
            //int LastInputTicks = 0;
            //int IdleTicks = 0;
            //LASTINPUTINFO LastInputInfo = new LASTINPUTINFO();
            //LastInputInfo.cbSize = (uint)Marshal.SizeOf(LastInputInfo);
            //LastInputInfo.dwTime = 0;
            ////获取用户上次操作的时间 
            //if (GetLastInputInfo(ref LastInputInfo)) 
            //{ 
            //    LastInputTicks = (int)LastInputInfo.dwTime;
            //    //求差，就是系统空闲的时间 
            //    IdleTicks = systemUpTime – LastInputTicks; 
            //}
            //lblSystemUpTime.Text = Convert.ToString(systemUpTime / 1000) + " 秒";
            //lblIdleTime.Text = Convert.ToString(IdleTicks / 1000) + " 秒"; 
        }



        #region 硬盘id


        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetVolumeInformation(
        string lpRootPathName,                       //   欲获取信息的那个卷的根路径 
        string lpVolumeNameBuffer,                 //   用于装载卷名（卷标）的一个字串 
        int nVolumeNameSize,                         //   lpVolumeNameBuffer字串的长度   
        ref int lpVolumeSerialNumber,           //   用于装载磁盘卷序列号的变量   
        int lpMaximumComponentLength,   // 指定一个变量，用于装载文件名每一部分的长度。例如，在“c:\component1\component2.ext”的情况下，它就代表component1或component2名称的长度 .

        int lpFileSystemFlags,
     //   用于装载一个或多个二进制位标志的变量。对这些标志位的解释如下：
     //FS_CASE_IS_PRESERVED 文件名的大小写记录于文件系统
     //FS_CASE_SENSITIVE 文件名要区分大小写
     //FS_UNICODE_STORED_ON_DISK 文件名保存为Unicode格式
     //FS_PERSISTANT_ACLS 文件系统支持文件的访问控制列表（ACL）安全机制
     //FS_FILE_COMPRESSION 文件系统支持逐文件的进行文件压缩
     //FS_VOL_IS_COMPRESSED 整个磁盘卷都是压缩的 

     string lpFileSystemNameBuffer, //指定一个缓冲区,用于装载文件系统的名称（如FAT，NTFS以及其他）       
            int nFileSystemNameSize                   //   lpFileSystemNameBuffer字串的长度
        );

        /// <summary>
        /// 获取硬盘ID
        /// </summary>
        /// <returns></returns>
        public string GetdiskID()
        {
            const int MAX_FILENAME_LEN = 256;
            int retVal = 0;
            int a = 0;
            int b = 0;
            string str1 = null;
            string str2 = null;

            GetVolumeInformation(
                @"C:\",
                str1,
                MAX_FILENAME_LEN,
                ref retVal,
                a,
                b,
                str2,
                MAX_FILENAME_LEN);
            return retVal.ToString();
        }

        #endregion



    }


}

