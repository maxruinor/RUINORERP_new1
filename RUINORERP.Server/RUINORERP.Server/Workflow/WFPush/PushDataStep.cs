using System;
using System.Collections.Generic;
using System.Text;
using TransInstruction;
using TransInstruction.DataPortal;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.WFPush
{
    /// <summary>
    /// 功能将传过来的数据，发出去
    /// </summary>
    public class PushDataStep : StepBody
    {
        public string TagetTableName { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {

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
                    //tx.PushString(System.DateTime.Now.ToString());
                    tx.PushString(System.DateTime.Now.ToString());
                    tx.PushString(TagetTableName);
                    //  tx.PushInt(pushdata.Length);
                    //  tx.PushBytes(pushdata);
                    tx.PushString("给客户端发提示消息测试！分发测试" + TagetTableName.ToString());
                    exMsg.Two = tx.toByte();
                    item.Value.AddSendData(exMsg);
                    frmMain.Instance.PrintInfoLog("工作流数据推送");
                }
                catch (Exception ex)
                {
                    frmMain.Instance.PrintInfoLog("服务器收到客户端基础信息变更分布失败:" + item.Value.User.UserName + ex.Message);
                }

            }
            return ExecutionResult.Next();
        }
    }
}
