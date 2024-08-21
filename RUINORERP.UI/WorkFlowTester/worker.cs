using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.UI.WorkFlowTester
{
    public class worker : StepBody
    {
        private ILogger logger;

       
        public worker(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("workerss");
        }
  
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("开始工作");
            logger.LogInformation("开始工作");
            return ExecutionResult.Next();
        }
    }
}
