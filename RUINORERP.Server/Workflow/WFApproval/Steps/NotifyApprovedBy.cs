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
namespace RUINORERP.Server.Workflow.WFApproval.Steps
{

    /// <summary>
    /// 通知审批人
    /// </summary>
    public class NotifyApprovedBy : StepBody
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<NotifyApprovedBy> _logger;
        private readonly ISessionService _sessionService;
        
        public string WorkId { get; set; }
        public string BillID { get; set; }
        public string BizType { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Approver { get; set; }


        public NotifyApprovedBy(
            ApplicationContext context,
            ILogger<NotifyApprovedBy> logger)
        {
            _context = context;
            _logger = logger;
            _sessionService = Startup.GetFromFac<ISessionService>();
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            _logger.LogInformation($"通知审核人"+WorkId);
            //通知谁 有来自谁的提交消息
            string msg = $"{DateTime.Now} 来自 {From} 的审核请求,单号{BillID}，请及时处理！: {BizType}";
            
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
                        Approver = Approver,
                        Timestamp = DateTime.Now
                    };
                    
                    var messageJson = System.Text.Json.JsonSerializer.Serialize(notificationData);
                    
                    // 发送审批通知命令
                    var success = _sessionService.SendCommandToSession(
                        session.SessionID, 
                        "APPROVAL_NOTIFICATION", 
                        messageJson
                    );
                    
                    if (!success)
                    {
                        _logger.LogWarning($"发送审批通知到 {session.UserName} 失败");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintInfoLog("NotifyApprovedBy:" + ex.Message);
                }
            }

            return ExecutionResult.Next();
        }
    }
}
