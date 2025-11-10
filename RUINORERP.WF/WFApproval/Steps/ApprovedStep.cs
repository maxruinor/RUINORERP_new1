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
    /// 审批通过
    /// </summary>
    public class ApprovedStep : StepBodyAsync
    {
        //private readonly IApplicationDbContext _context;
        //private readonly IMailService _mailService;
        //private readonly ILogger<ApprovedStep> _logger;
        public string WorkId { get; set; }
        public string DocumentName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string Approver { get; set; }
        //public ApprovedStep(
        //    //IApplicationDbContext context,
        //    IMailService mailService,
        //    ILogger<ApprovedStep> logger)
        //{
        //    //_context = context;
        //    _mailService = mailService;
        //    _logger = logger;
        //}

        /// <summary>
        /// 审核人
        /// </summary>
        public long UserId { get; set; }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            await Task.Delay(1);
            // _logger.LogDebug($"Your request document has been approved by {Approver}! DocumentName:{DocumentName},Send mail to:{To}");

            //var approval = _context.ApprovalDatas.FirstOrDefault(x => x.WorkflowId == WorkId);
            //if (approval != null)
            //{
            //    approval.Status = "Finished";
            //    approval.Outcome = "Approved";
            //    approval.ApprovedDateTime = DateTime.Now;
            //    approval.Comments = "";
            //    await _context.SaveChangesAsync(default);
            //}

            return ExecutionResult.Next();
        }
    }
}
