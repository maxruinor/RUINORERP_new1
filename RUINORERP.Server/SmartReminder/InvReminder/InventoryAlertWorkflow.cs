using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.SmartReminder.InvReminder
{
    public class InventoryAlertWorkflow : IWorkflow<InventoryAlertContext>
    {
        public void Build(IWorkflowBuilder<InventoryAlertContext> builder)
        {
            builder
                .StartWith<SendAlertStep>()
                .Then<WaitForResponseStep>()
                .Output(ctx => ctx.IsResolved, step => step.Response)
                .Then<FinalizeStep>();
        }
    }

    public class SendAlertStep : StepBody
    {
        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var reminder = context.Workflow.Services.GetService<INotificationService>();
            reminder.SendAlertAsync(/* 参数 */);
            return ExecutionResult.Next();
        }
    }
}
