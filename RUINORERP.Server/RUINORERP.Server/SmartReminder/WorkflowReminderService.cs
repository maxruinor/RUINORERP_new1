using RUINORERP.Server.SmartReminder.InvReminder;
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

        public void RegisterFlows()
        {
            //_workflowHost.RegisterWorkflow<OrderApprovalWorkflow>();
            _workflowHost.RegisterWorkflow<InventoryAlertWorkflow, InventoryAlertContext>();
        }
        public async Task TriggerWorkflowAsync<T>(T data) where T : new()
        {
            await _workflowHost.StartWorkflow(typeof(T).Name, data: data);
        }

    }


    // 库存预警工作流 InventoryAlertWorkflow.cs
    public class InventoryAlertWorkflow : IWorkflow<InventoryAlertContext>
    {
        public string Id => throw new NotImplementedException();

        public int Version => throw new NotImplementedException();

        public void Build(IWorkflowBuilder<InventoryAlertContext> builder)
        {

        }
    }





    public class SendReminderStep : StepBody
    {
        public string Message { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 调用提醒服务
            //var reminder = context.Workflow.GetService<SmartReminderService>();
            //reminder.Trigger(new ReminderRequest
            //{
            //    Type = "OrderApproval",
            //    Context = context.Workflow.Data
            //});
            return ExecutionResult.Next();
        }
    }
}
