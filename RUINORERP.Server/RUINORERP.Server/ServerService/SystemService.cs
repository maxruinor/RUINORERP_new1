using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataPortal;
using TransInstruction;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RUINORERP.Server.ServerService
{
    /// <summary>
    /// 系统服务
    /// </summary>
    public static class SystemService
    {
        public static void 转发异常数据(SessionforBiz SendSession,SessionforBiz SuperAdminSession, OriginalData gd)
        {
            try
            {
                //先解析异常数据
                int index = 0;
                string datatime = ByteDataAnalysis.GetString(gd.Two, ref index);
                string Employee_Name = ByteDataAnalysis.GetString(gd.Two, ref index);
                string MachineName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string IP = ByteDataAnalysis.GetString(gd.Two, ref index);
                string ExMsg = ByteDataAnalysis.GetString(gd.Two, ref index);
                string ExCode = ByteDataAnalysis.GetString(gd.Two, ref index);

                //再转发到超级管理员 ，如果超级管理员不在线。缓存？上线再发送？
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(datatime);
                tx.PushString(SendSession.SessionID);
                tx.PushString(Employee_Name);
                tx.PushString(MachineName);
                tx.PushString(IP);
                tx.PushString(ExMsg);
                tx.PushString(ExCode);
                tx.PushBool(true);//MustDisplay 是否强制显示
                SuperAdminSession.AddSendData((byte)ServerCmdEnum.转发异常, null, tx.toByte());

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }

        }

        /// <summary>
        /// 转发到能处理的人
        /// </summary>
        /// <param name="SendSession"></param>
        /// <param name="SuperAdminSession"></param>
        /// <param name="gd"></param>
        public static void 转发协助处理(SessionforBiz SendSession, SessionforBiz SuperAdminSession, OriginalData gd)
        {
            try
            {
                //先解析数据
                int index = 0;
                string datatime = ByteDataAnalysis.GetString(gd.Two, ref index);
                long RequestUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                string RequestContent = ByteDataAnalysis.GetString(gd.Two, ref index);
                 string BillData = ByteDataAnalysis.GetString(gd.Two, ref index);
                string BillType = ByteDataAnalysis.GetString(gd.Two, ref index);
                //再转发到超级管理员 ，如果超级管理员不在线。缓存？上线再发送？
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(datatime);
                tx.PushInt64(RequestUserID);//请示的人ID
                tx.PushString(RequestContent);
                tx.PushString(BillData);//请示的人姓名。后面单据数据要保存时要名称开头
                tx.PushString(BillType);

                SuperAdminSession.AddSendData((byte)ServerCmdEnum.转发协助处理, null, tx.toByte());

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }

        }

    }
}
