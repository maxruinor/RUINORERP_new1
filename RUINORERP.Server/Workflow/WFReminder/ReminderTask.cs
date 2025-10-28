using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;
using System.Numerics;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.ServerSession;
using Newtonsoft.Json;
using Azure.Core;
using RUINORERP.Model.TransModel;
using RUINORERP.Model;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.BizService;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;

namespace RUINORERP.Server.Workflow.WFReminder
{
    /// <summary>
    /// 将数据推送到客户端
    /// </summary>
    public class ReminderTask : StepBody
    {
        private readonly ISessionService _sessionService;
        
        DataServiceChannel serviceChannel;

        public ReminderTask(DataServiceChannel _serviceChannel)
        {
            serviceChannel = _serviceChannel;
            _sessionService = Startup.GetFromFac<ISessionService>();
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
            frmMainNew.Instance.ReminderBizDataList.TryGetValue(BizData.BizPrimaryKey, out exData);
            if (context.CancellationToken.IsCancellationRequested)
            {
                Status = MessageStatus.Cancel;
                //直接清除停止
                frmMainNew.Instance.ReminderBizDataList.TryRemove(exData.BizPrimaryKey, out exData);
                return ExecutionResult.Next();
            }
            var data = context.Workflow.Data as ReminderData;
            if (data.IsCancelled)
            {
                Status = MessageStatus.Cancel;
                //直接清除停止
                frmMainNew.Instance.ReminderBizDataList.TryRemove(exData.BizPrimaryKey, out exData);
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
                    var sessions = _sessionService.GetAllUserSessions();
                    foreach (var session in sessions)
                    {
                        if (exData.ReceiverUserIDs.Contains(session.UserInfo.UserID))
                        {
                            try
                            {
                                exData.RemindTimes++;
                                
                                // 构建提醒消息
                                var reminderData = new
                                {
                                    SendTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    ReminderData = exData,
                                    ForcePopup = true
                                };
                                
                                var messageJson = System.Text.Json.JsonSerializer.Serialize(reminderData,
                                    new System.Text.Json.JsonSerializerOptions
                                    {
                                        // 忽略循环引用
                                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                                    });
                                
                                // 发送工作流提醒命令 - 使用新的发送方法
                                var messageData = new
                                {
                                    Command = "WORKFLOW_REMINDER",
                                    Data = messageJson
                                };

                                var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                                var success = _sessionService.SendCommandAsync(
                                    session.SessionID, 
                                    MessageCommands.SendMessageToUser, 
                                    request).Result; // 注意：这里使用.Result是为了保持原有的同步行为
                                
                                if (success)
                                {
                                    frmMainNew.Instance.ReminderBizDataList.TryUpdate(BizData.BizPrimaryKey, exData, exData);
                                    if (frmMainNew.Instance.IsDebug)
                                    {
                                        frmMainNew.Instance.PrintInfoLog($"工作流提醒推送到{session.UserName}");
                                    }
                                }
                                else
                                {
                                    frmMainNew.Instance.PrintInfoLog($"发送工作流提醒到用户 {session.UserName} 失败");
                                }
                            }
                            catch (Exception ex)
                            {
                                frmMainNew.Instance.PrintInfoLog("服务器工作流提醒推送分布失败:" + session.UserName + ex.Message);
                            }
                        }
                    }
                }
            }
            return ExecutionResult.Next();
        }
    }
}
