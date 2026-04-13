using Microsoft.Extensions.Logging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.Steps
{
    public class PrintMessage : StepBody
    {
  

        private readonly ILogger<PrintMessage> _logger;
        public PrintMessage(ILogger<PrintMessage> logger)
        {
            _logger = logger;
        }

        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 服务器端不应使用UI组件,改用日志记录
            _logger.LogInformation("工作流消息: {Message}", Message);
            System.Diagnostics.Debug.WriteLine(Message);
            return ExecutionResult.Next();
        }
    }
}
