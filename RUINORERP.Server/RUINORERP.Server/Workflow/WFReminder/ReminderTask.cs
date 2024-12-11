using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataPortal;
using TransInstruction;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.WFReminder
{
    /// <summary>
    /// 将数据推送到客户端
    /// </summary>
    public class ReminderTask : StepBody
    {
        /// <summary>
        /// 接收人ID
        /// </summary>
        public long RecipientID { get; set; }

        public int RemindCount { get; set; } = 1;
        /// <summary>
        /// 提醒的消息
        /// </summary>
        public string ReminderMessage { get; set; }

        public string TagetTableName { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this.RemindCount++;
            //byte[] pushdata = HLH.Lib.Helper.SerializationHelper.SerializeDataEntity(data);
            //服务器收到客户端基础信息变更分布
            //回推
            //WorkflowServiceSender.通知工作流启动成功(UserSession, workflowid);
            foreach (var item in frmMain.Instance.sessionListBiz)
            {
                try
                {
                    OriginalData exMsg = new OriginalData();
                    exMsg.cmd = (byte)ServerCmdEnum.工作流数据推送;
                    exMsg.One = null;
                    //这种可以写一个扩展方法
                    ByteBuff tx = new ByteBuff(100);
                    tx.PushString(System.DateTime.Now.ToString());
                    tx.PushString(TagetTableName);
                    tx.PushString("给客户端发提示消息测试！分发测试" + TagetTableName.ToString());
                    exMsg.Two = tx.toByte();
                    item.Value.AddSendData(exMsg);
                    frmMain.Instance.PrintInfoLog("工作流数据推送");
                }
                catch (Exception ex)
                {
                    frmMain.Instance.PrintInfoLog("服务器收到客户端基础信息变更分布失败:" + item.Value.User.用户名 + ex.Message);
                }

            }
            return ExecutionResult.Next();
        }
    }
}
