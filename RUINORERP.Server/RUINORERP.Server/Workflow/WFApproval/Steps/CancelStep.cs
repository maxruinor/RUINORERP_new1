using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;

namespace RUINORERP.Server.Workflow.WFApproval.Steps
{
    /// <summary>
    /// 审批超时 通知申请超时被取消
    /// </summary>
    public class CancelStep : StepBodyAsync
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CancelStep> _logger;
        public string WorkId { get; set; }
        public string DocumentName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }


        public CancelStep(
            ApplicationContext context,
            ILogger<CancelStep> logger)
        {
            _context = context;
            _logger = logger;
        }
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            WorkId = context.Workflow.Id;
            //发邮件通知申请人

            // await _mailService.SendAsync(request);

            _logger.LogInformation($"审核取消:{Body},Send to {To}");

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
