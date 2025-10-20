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
    public class loopWork : StepBody
    {
        private ILogger logger;

        public loopWork(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("workerss");
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("循环工作" + System.DateTime.Now.ToString());
            return ExecutionResult.Next();
        }
    }
}
