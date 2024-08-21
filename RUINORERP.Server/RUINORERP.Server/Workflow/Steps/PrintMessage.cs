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
            System.Windows.Forms.MessageBox.Show(Message);
            _logger.Info("测试1", Message);
            _logger.LogInformation("测试2", Message);
            Console.WriteLine(Message);
            return ExecutionResult.Next();
        }
    }
}
