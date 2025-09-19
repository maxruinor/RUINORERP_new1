using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;
using System.Numerics;
using RUINORERP.Server.ServerSession;
using Newtonsoft.Json;
using Azure.Core;
using RUINORERP.Model.TransModel;
using RUINORERP.Model;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.BizService;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.Server.Workflow.WFReminder
{
    /// <summary>
    /// 将数据推送到客户端
    /// </summary>
    public class ReminderTask : StepBody
    {


        DataServiceChannel serviceChannel;

        public ReminderTask(DataServiceChannel _serviceChannel)
        {
            serviceChannel = _serviceChannel;
        }

        /// <summary>
        /// 提醒时间，只要当前时间大于这个时间就推送提醒
        /// </summary>
        public DateTime RemindTime { get; set; } = System.DateTime.Now;

        public ReminderData BizData { get; set; }
        /// <summary>
        /// 接收人ID
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// 提醒的消息
        /// </summary>
        public string ReminderMessage { get; set; }

        public string TagetTableName { get; set; }

        public MessageStatus Status { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //byte[] pushdata = HLH.Lib.Helper.SerializationHelper.SerializeDataEntity(data);
            //服务器收到客户端基础信息变更分布
            //回推
            //WorkflowServiceSender.通知工作流启动成功(UserSession, workflowid);
            ReminderData exData = null;
            //检测收到的信息
            frmMain.Instance.ReminderBizDataList.TryGetValue(BizData.BizPrimaryKey, out exData);
            if (context.CancellationToken.IsCancellationRequested)
            {
                Status = MessageStatus.Cancel;
                //直接清除停止
                frmMain.Instance.ReminderBizDataList.TryRemove(exData.BizPrimaryKey, out exData);
                return ExecutionResult.Next();
            }
            var data = context.Workflow.Data as ReminderData;
            if (data.IsCancelled)
            {
                Status = MessageStatus.Cancel;
                //直接清除停止
                frmMain.Instance.ReminderBizDataList.TryRemove(exData.BizPrimaryKey, out exData);
                return ExecutionResult.Next();
            }

            //时间到了就不再提醒了。
            if (exData.EndTime < System.DateTime.Now)
            {
                Status = MessageStatus.Cancel;
                //提醒到期了
                serviceChannel.ProcessCRMFollowUpPlansData(exData, Status);
            }
            if (exData.Status != Model.MessageStatus.Cancel)
            {
                //将客户端要求的间隔时间传到步骤的参数，再传到工作流中
                if (exData.Status == Model.MessageStatus.WaitRminder)
                {
                    //将回应的参数传给步骤再传到工作流中
                    RemindTime = System.DateTime.Now.AddSeconds(exData.RemindInterval);
                }
                //相同的事情提醒最多10次
                if (System.DateTime.Now > RemindTime && exData.RemindTimes < 10)
                {
                    foreach (var item in frmMain.Instance.sessionListBiz)
                    {
                        if (exData.ReceiverEmployeeIDs.Contains(item.Value.User.Employee_ID))
                        {
                            try
                            {
                                exData.RemindTimes++;
                                //  WorkflowServiceReceiver.发送工作流提醒();
                                OriginalData exMsg = new OriginalData();
                                exMsg.Cmd = (byte)PacketSpec.Commands.WorkflowCommands.WorkflowReminder;
                                exMsg.One = null;

                                //这种可以写一个扩展方法
                                ByteBuffer tx = new ByteBuffer(100);
                                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                tx.PushString(sendtime);
                                string json = JsonConvert.SerializeObject(exData,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                        });

                                tx.PushString(json);
                                //tx.PushString("【系统提醒】" + System.DateTime.Now.ToString());//发送者
                                //tx.PushString(item.Value.SessionID);
                                //tx.PushString(exData.RemindSubject);
                                //tx.PushString(exData.ReminderContent);
                                tx.PushBool(true);//是否强制弹窗
                                exMsg.Two = tx.ToByteArray();
                                item.Value.AddSendData(exMsg);

                                frmMain.Instance.ReminderBizDataList.TryUpdate(BizData.BizPrimaryKey, exData, exData);
                                if (frmMain.Instance.IsDebug)
                                {
                                    frmMain.Instance.PrintInfoLog($"工作流提醒推送到{item.Value.User.用户名}");
                                }

                            }
                            catch (Exception ex)
                            {
                                frmMain.Instance.PrintInfoLog("服务器工作流提醒推送分布失败:" + item.Value.User.用户名 + ex.Message);
                            }
                        }
                        //如果不注释，相同的员工有多个帐号时。员工只会提醒一个。
                        //else
                        //{
                        //    continue;
                        //}


                    }
                }
            }
            return ExecutionResult.Next();
        }
    }
}
