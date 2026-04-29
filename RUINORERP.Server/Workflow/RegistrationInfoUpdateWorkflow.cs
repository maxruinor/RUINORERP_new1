using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Services;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow
{
    /// <summary>
    /// 注册信息更新工作流
    /// 每日晚上指定时间从数据库重新加载注册信息到内存
    /// </summary>
    public class RegistrationInfoUpdateWorkflow : IWorkflow
    {
        public string Id => "RegistrationInfoUpdateWorkflow";
        public int Version => 1;

        /// <summary>
        /// 执行时间（每日晚上11点）
        /// </summary>
        private static DateTime ExecutionTime => DateTime.Today.AddHours(23);

        /// <summary>
        /// 下次执行时间间隔
        /// </summary>
        private TimeSpan NextTime => ExecutionTime.AddDays(1) - DateTime.Now;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context => 
                {
                    var logger = Startup.GetFromFac<ILogger<RegistrationInfoUpdateWorkflow>>();
                    logger?.LogInformation($"注册信息更新工作流开始执行 - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                })
                .Recur(data => GetNextExecutionTime(data), data => ShouldContinueLoop(data))
                    .Do(recur => recur
                        .StartWith<RegistrationInfoUpdateStep>(
                            context =>
                            {
                                // 更新下次执行时间
                            }
                        )
                        .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(20))
                    )
                    .Then(context => 
                    {
                        var logger = Startup.GetFromFac<ILogger<RegistrationInfoUpdateWorkflow>>();
                        logger?.LogInformation($"注册信息更新工作流执行完成 - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    });
        }

        private static TimeSpan GetNextExecutionTime(object data)
        {
            return ExecutionTime.AddDays(1) - DateTime.Now;
        }

        private static bool ShouldContinueLoop(object data)
        {
            return data != null && data.ToString() != "";
        }
    }

    /// <summary>
    /// 注册信息更新步骤
    /// 从数据库重新加载注册信息到内存
    /// </summary>
    public class RegistrationInfoUpdateStep : StepBodyAsync
    {
        private readonly ILogger<RegistrationInfoUpdateStep> _logger;
        private readonly IRegistrationService _registrationService;

        public RegistrationInfoUpdateStep(
            ILogger<RegistrationInfoUpdateStep> logger,
            IRegistrationService registrationService)
        {
            _logger = logger;
            _registrationService = registrationService;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                _logger.LogInformation("开始更新注册信息缓存 - {DateTime}", DateTime.Now);

                // 从数据库重新加载注册信息到内存
                var success = await _registrationService.UpdateRegistrationInfoCacheAsync();

                if (success)
                {
                    _logger.LogInformation("注册信息缓存更新成功");
                }
                else
                {
                    _logger.LogWarning("注册信息缓存更新失败");
                }

                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行注册信息更新步骤时发生异常");
                return ExecutionResult.Next();
            }
        }
    }
}
