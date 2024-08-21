using System.Diagnostics;

namespace HLH.Lib.Helper
{
    public static class SystemOperation
    {
        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="processName">进程名</param>
        public static void KillProcess(string processName)
        {
            Process[] myproc = Process.GetProcesses();
            foreach (Process item in myproc)
            {
                if (item.ProcessName == processName)
                {
                    item.Kill();
                }
            }
        }


        /// <summary>
        /// 打开指定程序
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="appName"></param>
        public static void ProcessStart(string appPath, string appName)
        {

            System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();

            //设置外部程序名  
            Info.FileName = appName;

            //设置外部程序工作目录为   C:\  
            Info.WorkingDirectory = appPath;

            //最小化方式启动
            Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

            //声明一个程序类  
            System.Diagnostics.Process Proc;

            try
            {
                Proc = System.Diagnostics.Process.Start(Info);
                System.Threading.Thread.Sleep(200);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return;
            }
        }


    }
}
