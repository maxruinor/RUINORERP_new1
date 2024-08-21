using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CommonProcess
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            StringProcess.frmTextProcesserTest test = new StringProcess.frmTextProcesserTest();
            Application.Run(test);
        }
        public static void ThrowException2MainThread(Exception ex)
        {
            Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {

            //frmMain.Instance.PrintInfoLog("Application_ThreadException:" + e.Exception.Message);
            //frmMain.Instance.PrintInfoLog(e.Exception.StackTrace);
            //HLH.Lib.Helper.log4netHelper.fatal("系统级Application_ThreadException", e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                              "with the following information:\n\n";
            if (e.IsTerminating)
            {
                //frmMain.Instance.PrintInfoLog("这个异常导致程序终止");
            }
            else
            {
                //frmMain.Instance.PrintInfoLog("CurrentDomain_UnhandledException:" + errorMsg);
            }
        }
    }
}
