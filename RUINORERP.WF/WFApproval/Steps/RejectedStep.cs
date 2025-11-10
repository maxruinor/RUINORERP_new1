using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
namespace RUINORERP.WF.WFApproval.Steps
{
    /// <summary>
    /// 被拒绝 发邮件通知申请人已被拒绝
    /// </summary>
    public class RejectedStep : StepBodyAsync
    {
        //private readonly IApplicationDbContext _context;

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
        public override  async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            Body = $"Your request document has been rejected! DocumentName:{DocumentName}";
            Subject = $"Your request has been rejected.";
            await Task.Delay(1);
            //var request = new MailRequest();
            // request.To = To;
            // request.Subject = Subject;
            // request.Body = Body;
            // await _mailService.SendAsync(request);
            Console.WriteLine($"Your request document has been rejected by {Approver}! DocumentName:{DocumentName},Send mail to:{To}");
            _logger.LogDebug($"Your request document has been rejected by {Approver}! DocumentName:{DocumentName},Send mail to:{To}");

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
