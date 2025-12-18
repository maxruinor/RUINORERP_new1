using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
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
    /// 被拒绝 发邮件通知申请人已被拒绝
    /// </summary>
    public class RejectedStep : StepBodyAsync
    {
#pragma warning disable CS0169 // 从不使用字段“RejectedStep._context”
        private readonly ApplicationContext _context;
#pragma warning restore CS0169 // 从不使用字段“RejectedStep._context”
        private readonly ILogger<RejectedStep> _logger;
        public string WorkId { get; set; }
        public string DocumentName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Approver { get; set; }
        public string Outcome { get; set; }
        public string Comments { get; set; }
        public RejectedStep(
            //IApplicationDbContext context,
            //IMailService mailService,
            ILogger<RejectedStep> logger)
        {
            //_context = context;
           // _mailService = mailService;
            _logger = logger;
        }
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            WorkId = context.Workflow.Id;
            Body = $"Your request document has been rejected! DocumentName:{DocumentName}";
            Subject = $"Your request has been rejected.";
            //var request = new MailRequest();
           // request.To = To;
           // request.Subject = Subject;
           // request.Body = Body;
           // await _mailService.SendAsync(request);
            System.Diagnostics.Debug.WriteLine($"Your request document has been rejected by {Approver}! DocumentName:{DocumentName},Send mail to:{To}");
            _logger.LogInformation($"Your request document has been rejected by {Approver}! DocumentName:{DocumentName},Send mail to:{To}");

            //var approval = _context.ApprovalDatas.FirstOrDefault(x => x.WorkflowId == WorkId);
            //if (approval != null)
            //{
            //    approval.Status = "Finished";
            //    approval.Outcome = "Rejected";
            //    approval.ApprovedDateTime=DateTime.Now;
            //    approval.Comments = "";
            //    await _context.SaveChangesAsync(default);
            //}
            return ExecutionResult.Next();
        }
    }
}
