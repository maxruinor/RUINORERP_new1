using Microsoft.Extensions.Logging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.Steps
{    
    public class SayGoodbye : StepBody
    {
        //private ILogger _logger;
        //public SayGoodbye(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<SayGoodbye>(); 
        //}
        public override ExecutionResult Run(IStepExecutionContext context)
        {
          //  _logger.LogInformation("goodbay2023");
            Console.WriteLine("Goodbye");
            System.Windows.Forms.MessageBox.Show("Goodbye");
            return ExecutionResult.Next();
        }
    }
}
