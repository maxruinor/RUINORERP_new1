using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.WFReminder
{
    public class ReminderStart : StepBody
    {
        public string Description { get; set; } = "remindstart";
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //这个步骤内得到工流传入的数据
            Console.WriteLine("Hello world");
            return ExecutionResult.Next();
        }
    }
}
