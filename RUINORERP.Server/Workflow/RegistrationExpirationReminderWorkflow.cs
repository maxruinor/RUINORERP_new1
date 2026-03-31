using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.Server.Services;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow
{
    /// <summary>
    /// 注册到期提醒工作流
    /// 每日上午10点检查注册状态，向即将到期的用户发送提醒
    /// </summary>
    public class RegistrationExpirationReminderWorkflow : IWorkflow
    {
        public string Id => "RegistrationExpirationReminderWorkflow";
        public int Version => 1;

        /// <summary>
        /// 执行时间（每日上午10点）
        /// </summary>
        private static DateTime ExecutionTime => DateTime.Today.AddHours(10);

        /// <summary>
        /// 下次执行时间间隔
        /// </summary>
        private TimeSpan NextTime => ExecutionTime.AddDays(1) - DateTime.Now;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context => 
                {
                    var logger = Startup.GetFromFac<ILogger<RegistrationExpirationReminderWorkflow>>();
                    logger?.LogInformation($"注册到期提醒工作流开始执行 - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                })
                .Recur(data => NextTime, data => data.ToString() != "")
                    .Do(recur => recur
                        .StartWith<RegistrationExpirationReminderStep>(
                            context =>
                            {
                                // 更新下次执行时间
                            }
                        )
                        .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(20))
                    )
                    .Then(context => 
                    {
                        var logger = Startup.GetFromFac<ILogger<RegistrationExpirationReminderWorkflow>>();
                        logger?.LogInformation($"注册到期提醒工作流执行完成 - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    });
        }
    }

    /// <summary>
    /// 注册到期提醒步骤
    /// 检查注册状态，向即将到期的用户发送提醒
    /// </summary>
    public class RegistrationExpirationReminderStep : StepBodyAsync
    {
        private readonly ILogger<RegistrationExpirationReminderStep> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly Server.Network.Services.ServerMessageService _messageService;

        public RegistrationExpirationReminderStep(
            ILogger<RegistrationExpirationReminderStep> logger,
            IRegistrationService registrationService,
            Server.Network.Services.ServerMessageService messageService)
        {
            _logger = logger;
            _registrationService = registrationService;
            _messageService = messageService;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                _logger.LogInformation("开始执行注册到期提醒检查 - {DateTime}", DateTime.Now);

                // 检查是否需要到期提醒
                var needsReminder = await _registrationService.CheckExpirationReminderAsync(reminderDays: 30);

                if (!needsReminder)
                {
                    _logger.LogInformation("无需发送注册到期提醒");
                    return ExecutionResult.Next();
                }

                // 获取到期提醒信息
                var expirationReminder = await _registrationService.GetExpirationReminderInfoAsync(reminderDays: 30);

                if (expirationReminder == null || !expirationReminder.NeedsReminder)
                {
                    _logger.LogInformation("到期提醒信息为空或不需要提醒");
                    return ExecutionResult.Next();
                }

                // 批量发送到期提醒
                var result = await _messageService.SendExpirationReminderBatchAsync(expirationReminder);

                _logger.LogInformation(
                    "注册到期提醒发送完成 - 总数：{Total}，成功：{Success}，失败：{Failed}",
                    result["Total"], result["Success"], result["Failed"]);

                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行注册到期提醒步骤时发生异常");
                return ExecutionResult.Next();
            }
        }
    }
}
