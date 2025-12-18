using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    public class CommService
    {
        public static string GetTime()
        {
            return DateTime.Now.ToString("u").Substring(11, 8);
        }

        //保存在Log4net_cath
        public static void ShowExceptionMsg(string msg)
        {
            System.Diagnostics.Debug.WriteLine(GetTime() + " > " + msg);
            frmMainNew.Instance._logger.Error("ShowExceptionMsg:"+msg);
        }

        public static void ShowExceptionMsg(string msg, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(GetTime() + " > " + msg);
            //frmMainNew.Instance.PrintInfoLog(GetTime() + " > " + msg + ex.Message + ex.StackTrace);
        }
    }
}
