using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Messaging;

namespace RUINORERP.Server.Workflow.WFApproval.Steps
{

    /// <summary>
    /// 通知相关人员完成了审批
    /// </summary>
    public class NotifyApprovedCompleted : StepBody
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<NotifyApprovedCompleted> _logger;
        private readonly ISessionService _sessionService;
        
        public string WorkId { get; set; }
        public string BillID { get; set; }
        public string BizType { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Approver { get; set; }


        public NotifyApprovedCompleted(
            ApplicationContext context,
            ILogger<NotifyApprovedCompleted> logger)
        {
            _context = context;
            _logger = logger;
            _sessionService = Startup.GetFromFac<ISessionService>();
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            _logger.LogInformation($"通知完成" + WorkId);
            //通知谁 有来自谁的提交消息
            string msg = $"{DateTime.Now} 来自 {From} 的审核完成,单号{BillID}，请知悉！: {BizType}";
            
            var sessions = _sessionService.GetAllUserSessions();
            foreach (var session in sessions)
            {
                try
                {
                    // 构建通知消息
                    var notificationData = new
                    {
                        WorkflowId = context.Workflow.Id,
                        Message = msg,
                        BillID = BillID,
                        BizType = BizType,
                        Timestamp = DateTime.Now
                    };
                    
                    var messageJson = System.Text.Json.JsonSerializer.Serialize(notificationData);
                    
                    // 发送审批完成通知命令 - 使用新的发送方法
                    var messageData = new
                    {
                        Command = "APPROVAL_COMPLETED_NOTIFICATION",
                        Data = messageJson
                    };

                    var request = new MessageRequest(MessageType.Business, messageData);
                    var success = _sessionService.SendCommandAsync(
                        session.SessionID, 
                        MessageCommands.SendMessageToUser, 
                        request).Result; // 注意：这里使用.Result是为了保持原有的同步行为
                    
                    if (!success)
                    {
                        _logger.LogWarning($"发送审批完成通知到 {session.UserName} 失败");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintInfoLog("NotifyApprovedCompleted:" + ex.Message);
                }
            }

            return ExecutionResult.Next();
        }
    }
}
