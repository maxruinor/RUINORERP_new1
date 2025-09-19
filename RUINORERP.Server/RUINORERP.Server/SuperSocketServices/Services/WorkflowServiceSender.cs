using RUINORERP.Business;
using RUINORERP.Model;

using RUINORERP.Server.ServerSession;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.WFApproval;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using RUINORERP.Global;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.Server.ServerService
{
    public class WorkflowServiceSender
    {
        public static ILogger<WorkflowServiceSender> _logger { get; set; }
        public WorkflowServiceSender(ILogger<WorkflowServiceSender> logger)
        {
            _logger = logger;
        }

        public static void 通知工作流启动成功(SessionforBiz PlayerSession, string workflowid)
        {
            try
            {
                //PacketProcess pp = new PacketProcess(PlayerSession);
                ByteBuff tx = new ByteBuff(100);
                tx.PushString(PlayerSession.SessionID);
                tx.PushString(workflowid);
                PlayerSession.AddSendData((byte)ServerCommand.通知发起人启动成功, null, tx.toByte());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "通知工作流启动成功");
            }
          
        }
        /*
        public static bool 通知事件(SessionforBiz PlayerSession, tb_UserInfo user)
        {
            bool rs = false;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                //PacketProcess pp = new PacketProcess(PlayerSession);
                ByteBuff tx = new ByteBuff(100);
                if (user != null)
                {
                    rs = true;
                    tx.PushBool(rs);
                    tx.PushString(PlayerSession.SessionID);
                    tx.PushInt64(user.User_ID);
                    tx.PushString(user.UserName);
                    tx.PushString(user.tb_employee.Employee_Name);
                }
                else
                {
                    rs = false;
                    tx.PushBool(rs);
                }
                PlayerSession.AddSendData((byte)ServerCommand.用户登陆回复, null, tx.toByte());
                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            return rs;
        }

        */


    }
}
