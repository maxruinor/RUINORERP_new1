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
        /// ȡ�����һ������ʱ��
        /// <para>����</para>
        /// </summary>
        /// <returns></returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo)) return 0;
            return Environment.TickCount - (long)vLastInputInfo.dwTime;


            //    //��ȡϵͳ������ʱ�� 
            //int systemUpTime = Environment.TickCount;
            //int LastInputTicks = 0;
            //int IdleTicks = 0;
            //LASTINPUTINFO LastInputInfo = new LASTINPUTINFO();
            //LastInputInfo.cbSize = (uint)Marshal.SizeOf(LastInputInfo);
            //LastInputInfo.dwTime = 0;
            ////��ȡ�û��ϴβ�����ʱ�� 
            //if (GetLastInputInfo(ref LastInputInfo)) 
            //{ 
            //    LastInputTicks = (int)LastInputInfo.dwTime;
            //    //������ϵͳ���е�ʱ�� 
            //    IdleTicks = systemUpTime �C LastInputTicks; 
            //}
            //lblSystemUpTime.Text = Convert.ToString(systemUpTime / 1000) + " ��";
            //lblIdleTime.Text = Convert.ToString(IdleTicks / 1000) + " ��"; 
        }



        #region Ӳ��id


        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetVolumeInformation(
        string lpRootPathName,                       //   ����ȡ��Ϣ���Ǹ���ĸ�·�� 
        string lpVolumeNameBuffer,                 //   ����װ�ؾ�������꣩��һ���ִ� 
        int nVolumeNameSize,                         //   lpVolumeNameBuffer�ִ��ĳ���   
        ref int lpVolumeSerialNumber,           //   ����װ�ش��̾����кŵı���   
        int lpMaximumComponentLength,   // ָ��һ������������װ���ļ���ÿһ���ֵĳ��ȡ����磬�ڡ�c:\component1\component2.ext��������£����ʹ���component1��component2���Ƶĳ��� .

        int lpFileSystemFlags,
     //   ����װ��һ������������λ��־�ı���������Щ��־λ�Ľ������£�
     //FS_CASE_IS_PRESERVED �ļ����Ĵ�Сд��¼���ļ�ϵͳ
     //FS_CASE_SENSITIVE �ļ���Ҫ���ִ�Сд
     //FS_UNICODE_STORED_ON_DISK �ļ�������ΪUnicode��ʽ
     //FS_PERSISTANT_ACLS �ļ�ϵͳ֧���ļ��ķ��ʿ����б�ACL����ȫ����
     //FS_FILE_COMPRESSION �ļ�ϵͳ֧�����ļ��Ľ����ļ�ѹ��
     //FS_VOL_IS_COMPRESSED �������̾���ѹ���� 

     string lpFileSystemNameBuffer, //ָ��һ��������,����װ���ļ�ϵͳ�����ƣ���FAT��NTFS�Լ�������       
            int nFileSystemNameSize                   //   lpFileSystemNameBuffer�ִ��ĳ���
        );

        /// <summary>
        /// ��ȡӲ��ID
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

