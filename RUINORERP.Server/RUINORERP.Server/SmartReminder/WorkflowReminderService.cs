using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.SmartReminder
{
    public class WorkflowReminderService
    {
        private readonly IWorkflowHost _workflowHost;

        public WorkflowReminderService(IWorkflowHost workflowHost)
        {
            _workflowHost = workflowHost;
            RegisterFlows();
        }

        private void RegisterFlows()
        {
            _workflowHost.RegisterWorkflow<OrderApprovalWorkflow>();
        }
    }

    public class OrderApprovalWorkflow : IWorkflow<OrderContext>
    {
        public void Build(IWorkflowBuilder<OrderContext> builder)
        {
            builder
                .StartWith<SendReminderStep>()
                .Input(step => step.Message, ctx => $"请审核订单 {ctx.OrderId}")
                .Then<WaitForResponseStep>()
                .Output(ctx => ctx.Approved, step => step.Response)
                .Then<FinalizeStep>();
        }
    }

    public class SendReminderStep : StepBody
    {
        public string Message { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 调用提醒服务
            var reminder = context.Workflow.Services.GetService<SmartReminderService>();
            reminder.Trigger(new ReminderRequest
            {
                Type = "OrderApproval",
                Context = context.Workflow.Data
            });
            return ExecutionResult.Next();
        }
    }
}
