using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.WFApproval
{
    /// <summary>
    /// 提交
    /// </summary>
    public class SubmitStep : StepBody
    {
        private readonly ILogger<SubmitStep> logger;

        public string subtext;

        public SubmitStep(ILogger<SubmitStep> _logger)
        {
            logger = _logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Submit" + subtext + System.DateTime.Now);
            logger.LogInformation("Submit" + subtext + System.DateTime.Now);
            return ExecutionResult.Next();
        }
    }

    public class ApproveStep : StepBody
    {
        private ILogger logger;

        public ApproveStep(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ApproveStep>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Workflow, Goodbye");
            logger.LogInformation("Goodbye workflow");

            return ExecutionResult.Next();
        }
    }


    public class CancelStep : StepBody
    {
        private ILogger logger;

        public CancelStep(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<CancelStep>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Workflow, Goodbye");
            logger.LogInformation("Goodbye workflow");

            return ExecutionResult.Next();
        }
    }

    public class EndStep : StepBody
    {
        private ILogger logger;

        public EndStep(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("SleepStep");
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Thread.Sleep(1000);

            logger.LogInformation("Sleeped");

            return ExecutionResult.Next();
        }
    }

    //public class ApprovalData
    //{
    //    public string Request { get; set; }
    //    public string ApprovedBy { get; set; }
    //}
}