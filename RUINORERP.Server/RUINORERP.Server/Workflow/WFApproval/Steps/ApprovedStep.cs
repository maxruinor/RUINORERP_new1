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
    /// 审批通过
    /// </summary>
    public class ApprovedStep : StepBodyAsync
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ApprovedStep> _logger;
        public string WorkId { get; set; }
        public string DocumentName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string Approver { get; set; }
        public ApprovedStep(
            ApplicationContext context,
            ILogger<ApprovedStep> logger)
        {
            _context = context;
            _logger = logger;
        }
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            WorkId = context.Workflow.Id;
           
          _logger.LogInformation($" 审批通过 has been approved by {Approver}! DocumentName:{DocumentName},Send mail to:{To}");

          //  var approval = _context.GetRequiredService<T>().ApprovalDatas.FirstOrDefault(x => x.WorkflowId == WorkId);
          //  if (approval != null)
          //  {
          //      approval.Status = "Finished";
          //      approval.Outcome = "Approved";
          //      approval.ApprovedDateTime = DateTime.Now;
          //      approval.Comments = "";
          //      await _context.SaveChangesAsync(default);
          //  }

            return ExecutionResult.Next();
        }
    }
}
