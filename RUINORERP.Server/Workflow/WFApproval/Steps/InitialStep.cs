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
    /// InitialStep 初始化,通知审批人有新的请求
    /// </summary>
    public class InitialStep : StepBody
    {
        private readonly ApplicationContext _appContext;
        private readonly ILogger<InitialStep> _logger;
        public string WorkId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DocumentName { get; set; }
        public int DocumentId { get; set; }

        public InitialStep(
            ApplicationContext appContext,
            ILogger<InitialStep> logger)
        {
            _appContext = appContext;
            _logger = logger;
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            System.Diagnostics.Debug.WriteLine($"开始 {WorkId}");
            return ExecutionResult.Next();
        }
    }
}
