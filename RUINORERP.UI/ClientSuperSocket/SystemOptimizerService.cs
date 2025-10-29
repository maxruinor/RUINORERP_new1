using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RUINORERP.UI.SuperSocketClient
{
    /// <summary>
    /// 系统优化服务
    /// </summary>
    [Obsolete("此类已过时，不再使用")]
    public static class SystemOptimizerService
    {
        public static OriginalData 异常信息发送(string message, Exception exception)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuffer(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                if (MainForm.Instance.AppContext.CurUserInfo==null)
                {
                    tx.PushString(MainForm.Instance.AppContext.SessionId);
                }
                else
                {
                    if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee == null)
                    {
                        tx.PushString(MainForm.Instance.AppContext.SessionId);
                    }
                    else
                    {
                        tx.PushString(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name);
                    }
                }
                

                tx.PushString(MainForm.Instance.AppContext.log.MachineName);
                tx.PushString(HLH.Lib.Net.IpAddressHelper.GetLocIP());
                tx.PushString(message);
                tx.PushString(exception.StackTrace);
                //gd.Cmd = (byte)system  ExceptionReport.OperationCode;
                gd.One = null;
                gd.Two = tx.ToByteArray();
                //如果自己是超级管理员就不发送
                if (!MainForm.Instance.AppContext.IsSuperUser)
                {
                    //MainForm.Instance.ecs.AddSendData(gd);
                }

            }
            catch (Exception)
            {

            }
            return gd;
        }

        public static OriginalData 异常信息发送(string message)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuffer(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name);
                tx.PushString(MainForm.Instance.AppContext.log.MachineName);
                tx.PushString(HLH.Lib.Net.IpAddressHelper.GetLocIP());
                tx.PushString(message);
                tx.PushString(string.Empty);
                gd.Cmd = (byte)SystemCommands.ExceptionReport.OperationCode;
                gd.One = null;
                gd.Two = tx.ToByteArray();
                //如果自己是超级管理员就不发送
                //if (!MainForm.Instance.AppContext.IsSuperUser)
                //{
                //MainForm.Instance.ecs.AddSendData(gd);
                //}

            }
            catch (Exception)
            {

            }
            return gd;
        }
    }
}
