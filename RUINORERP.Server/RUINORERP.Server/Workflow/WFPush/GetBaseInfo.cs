using Microsoft.Extensions.Logging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;


namespace RUINORERP.Server.Workflow.WFPush
{
    public class GetBaseInfo : StepBody
    {
        private readonly ILogger<IPushWorkflow> _logger;
        public GetBaseInfo(ILogger<IPushWorkflow> logger)
        {
            _logger = logger;
        }

        public object BaseInfo { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //_logger.Info("GetBaseInfo_run", BaseInfo.ToString());
            _logger.Info("GetBaseInfo_run","开始");
            // _logger.LogInformation("测试2", Message);
            //  Console.WriteLine(Message);
            return ExecutionResult.Next();
        }
    }
}
