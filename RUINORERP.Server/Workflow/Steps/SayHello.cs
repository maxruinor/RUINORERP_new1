using Microsoft.Extensions.Logging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.Steps
{
    public class SayHello : StepBody
    {
        //private ILogger<SayHello> _logger;
        //public SayHello(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<SayHello>();
        //}
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //_logger.LogWarning("只是测试哦，hello123");
            System.Windows.Forms.MessageBox.Show("sayhello123");
            Console.WriteLine("Hello");
            return ExecutionResult.Next();
        }
    }
}
