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
    /// 审批超时 通知申请超时被取消
    /// </summary>
    public class CancelStep : StepBodyAsync
    {
        //private readonly IApplicationDbContext _context;
        //private readonly IMailService _mailService;
        //private readonly ILogger<ApprovedStep> _logger;
        public string WorkId { get; set; }
        public string DocumentName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }


        //public CancelStep(
        //    IApplicationDbContext context,
        //    IMailService mailService,
        //    ILogger<ApprovedStep> logger)
        //{
        //    _context = context;
        //    _mailService = mailService;
        //    _logger = logger;
        //}
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            //发邮件通知申请人
            await Task.Delay(1);
            // await _mailService.SendAsync(request);

            //  _logger.LogInformation($"Send notfication:{Body},Send to {To}");

            //var approval = _context.ApprovalDatas.FirstOrDefault(x => x.WorkflowId == WorkId);
            //if (approval != null)
            //{
            //    approval.Status = "Finished";
            //    approval.Outcome = "cancelled";
            //    approval.ApprovedDateTime = DateTime.Now;
            //    approval.Comments = "It's timed out,approval has been automatically cancelled ";
            //    await _context.SaveChangesAsync(default);
            //}
            return ExecutionResult.Next();
        }
    }
}
