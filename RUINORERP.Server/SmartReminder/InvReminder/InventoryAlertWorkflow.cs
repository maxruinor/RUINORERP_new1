using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using RUINORERP.Server.SmartReminder;
using RUINORERP.Model.ReminderModel;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.SmartReminder.InvReminder
{
    public class InventoryAlertWorkflow : IWorkflow<InventoryAlertContext>
    {
        public string Id => "InventoryAlertWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<InventoryAlertContext> builder)
        {
            builder
                .StartWith<SendAlertStep>()
                .Input(step => step.Message, ctx => ctx.AlertMessage)
                .Input(step => step.Rule, ctx => ctx.Rule)
                .Input(step => step.ContextData, ctx => ctx.ContextData)
                .Then<WaitForResponseStep>()
                .Output(ctx => ctx.IsResolved, step => step.Response)
                .Then<FinalizeStep>()
                .Input(step => step.IsResolved, ctx => ctx.IsResolved)
                .Input(step => step.RuleId, ctx => ctx.Rule?.RuleId);
        }
    }

    public class InventoryAlertContext
    {
        public IReminderRule Rule { get; set; }
        public string AlertMessage { get; set; }
        public object ContextData { get; set; }
        public bool IsResolved { get; set; }
    }

    public class SendAlertStep : StepBody
    {
        public string Message { get; set; }
        public IReminderRule Rule { get; set; }
        public object ContextData { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            try
            {
                var reminder = context.Workflow.Services.GetService<INotificationService>();
                if (reminder != null && !string.IsNullOrEmpty(Message) && Rule != null)
                {
                    reminder.SendNotificationAsync(Rule, Message, ContextData);
                    return ExecutionResult.Next();
                }
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                var logger = context.Workflow.Services.GetService<ILogger<SendAlertStep>>();
                logger?.LogError(ex, "发送预警通知失败");
                return ExecutionResult.Next(); // 即使失败也继续流程
            }
        }
    }

    public class WaitForResponseStep : StepBody
    {
        public bool Response { get; private set; }
        public int TimeoutMinutes { get; set; } = 60; // 默认等待60分钟

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var logger = context.Workflow.Services.GetService<ILogger<WaitForResponseStep>>();
            
            // 检查是否已经等待超时
            if (DateTime.Now - context.ExecutionPointer.CreationTime > TimeSpan.FromMinutes(TimeoutMinutes))
            {
                logger?.LogInformation("工作流等待响应超时，将继续执行");
                Response = false;
                return ExecutionResult.Next();
            }

            // 检查是否有用户响应
            // 这里应该从数据库或Redis中查询是否有用户对该提醒的处理记录
            // 简化实现，实际应用中需要与业务系统集成
            var workflowId = context.Workflow.Id;
            bool hasResponse = CheckUserResponse(workflowId);
            
            if (hasResponse)
            {
                Response = true;
                logger?.LogInformation("收到用户响应，工作流继续执行");
                return ExecutionResult.Next();
            }
            else
            {
                // 继续等待，设置回调时间
                logger?.LogDebug("等待用户响应中...");
                return ExecutionResult.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        private bool CheckUserResponse(string workflowId)
        {
            // 这里应该实现实际的查询逻辑
            // 例如：从数据库中查询该工作流ID对应的处理记录
            return false; // 简化实现
        }
    }

    public class FinalizeStep : StepBody
    {
        public bool IsResolved { get; set; }
        public long RuleId { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var logger = context.Workflow.Services.GetService<ILogger<FinalizeStep>>();
            
            try
            {
                // 记录工作流完成状态
                logger?.LogInformation("库存预警工作流完成，规则ID: {RuleId}, 是否已解决: {IsResolved}", 
                    RuleId, IsResolved);
                
                // 这里可以添加后续处理逻辑，例如：
                // 1. 更新提醒状态到数据库
                // 2. 触发其他业务流程
                // 3. 记录历史记录
                
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "工作流结束步骤执行失败，规则ID: {RuleId}", RuleId);
                return ExecutionResult.Next(); // 即使失败也标记完成
            }
        }
    }
}
