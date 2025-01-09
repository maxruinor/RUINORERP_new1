using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataPortal;
using TransInstruction;

namespace RUINORERP.UI.SuperSocketClient
{
    /// <summary>
    /// 系统优化服务
    /// </summary>
    public static class SystemOptimizerService
    {
        public static OriginalData 异常信息发送(string message, Exception exception)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                tx.PushString(System.DateTime.Now.ToString());
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
                gd.cmd = (byte)ClientCmdEnum.实时汇报异常;
                gd.One = null;
                gd.Two = tx.toByte();
                //如果自己是超级管理员就不发送
                if (!MainForm.Instance.AppContext.IsSuperUser)
                {
                    MainForm.Instance.ecs.AddSendData(gd);
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
                var tx = new ByteBuff(100);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name);
                tx.PushString(MainForm.Instance.AppContext.log.MachineName);
                tx.PushString(HLH.Lib.Net.IpAddressHelper.GetLocIP());
                tx.PushString(message);
                tx.PushString(string.Empty);
                gd.cmd = (byte)ClientCmdEnum.实时汇报异常;
                gd.One = null;
                gd.Two = tx.toByte();
                //如果自己是超级管理员就不发送
                //if (!MainForm.Instance.AppContext.IsSuperUser)
                //{
                MainForm.Instance.ecs.AddSendData(gd);
                //}

            }
            catch (Exception)
            {

            }
            return gd;
        }
    }
}
