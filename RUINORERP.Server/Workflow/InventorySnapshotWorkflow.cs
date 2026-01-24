using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using RUINORERP.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow
{

    /// <summary>
    /// 库存快照工作流
    /// </summary>
    public class InventorySnapshotWorkflow : IWorkflow
    {
        public string Id => "InventorySnapshotWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<GenerateSnapshotStep>()
                .Then<CleanupOldSnapshotsStep>();
        }
    }

    /// <summary>
    /// 生成库存快照步骤
    /// </summary>
    public class GenerateSnapshotStep : StepBodyAsync
    {
        private readonly tb_InventorySnapshotController<tb_InventorySnapshot> _snapshotService;
        private readonly ILogger<GenerateSnapshotStep> _logger;

        public GenerateSnapshotStep(tb_InventorySnapshotController<tb_InventorySnapshot> snapshotService, ILogger<GenerateSnapshotStep> logger)
        {
            _snapshotService = snapshotService;
            _logger = logger;
        }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                var sueccess = _snapshotService.GenerateDailySnapshot();
                //库存快照生成完成
                //ExecutionResult executionResult = new ExecutionResult();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成库存快照失败");
                //return Task.FromResult(ExecutionResult.F(ex.Message));

            }
            return Task.FromResult(ExecutionResult.Next());
        }
    }

    /// <summary>
    /// 清理旧快照步骤
    /// </summary>
    public class CleanupOldSnapshotsStep : StepBodyAsync
    {
        private readonly tb_InventorySnapshotController<tb_InventorySnapshot> _snapshotService;
        private readonly ILogger<CleanupOldSnapshotsStep> _logger;

        public CleanupOldSnapshotsStep(tb_InventorySnapshotController<tb_InventorySnapshot> snapshotService, ILogger<CleanupOldSnapshotsStep> logger)
        {
            _snapshotService = snapshotService;
            _logger = logger;
        }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                _logger.LogInformation("开始清理过期库存快照...");
                // 保留最近12个月的快照
                var count = _snapshotService.CleanupExpiredSnapshots(12);
                _logger.LogInformation($"过期库存快照清理完成，共清理 {count} 条记录");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期库存快照失败");

            }
            return Task.FromResult(ExecutionResult.Next());
        }
    }

    public static class InventorySnapshotWorkflowConfig
    {
        /// <summary>
        /// 库存快照执行时间配置（从配置文件读取，默认凌晨3点）
        /// </summary>
        public static TimeSpan DailyExecutionTime =>
            ScheduledTaskHelper.GetTaskExecutionTime(ScheduledTaskHelper.InventorySnapshotTask);

        /// <summary>
        /// 是否启用调试模式（调试模式下可立即执行）
        /// </summary>
        public static bool DebugMode = false;

        /// <summary>
        /// 调试模式下的执行间隔（分钟）
        /// </summary>
        public static int DebugExecutionIntervalMinutes = 5;

        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册步骤和工作流
            // 注册服务
            services.AddTransient<InventorySnapshotWorkflow>();
            services.AddTransient<GenerateSnapshotStep>();
            services.AddTransient<CleanupOldSnapshotsStep>();
        }

        /// <summary>
        /// 库存快照调度（支持可配置的执行时间点）
        /// </summary>
        public static async Task<bool> ScheduleInventorySnapshot(IWorkflowHost host)
        {
            try
            {
                // 注册工作流
                host.RegisterWorkflow<InventorySnapshotWorkflow>();

                if (DebugMode)
                {
                    // 调试模式：按固定间隔执行
                    await ScheduleDebugMode(host);
                }
                else
                {
                    // 生产模式：按指定时间点每天执行一次
                    await ScheduleProductionMode(host);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"工作流注册错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 生产模式调度：按指定时间点每天执行一次
        /// </summary>
        private static async Task ScheduleProductionMode(IWorkflowHost host)
        {
            // 计算下次执行时间
            var now = DateTime.Now;
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day,
                DailyExecutionTime.Hours, DailyExecutionTime.Minutes, DailyExecutionTime.Seconds);

            if (nextRunTime <= now)
                nextRunTime = nextRunTime.AddDays(1);

            // 计算时间间隔
            var interval = nextRunTime - now;

            // 首次延迟执行
            var timer = new System.Timers.Timer(interval.TotalMilliseconds);
            timer.Elapsed += async (sender, e) =>
            {
                SafeLogInfo($"开始库存快照工作流执行: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                try
                {
                    // 执行工作流
                    await host.StartWorkflow<InventorySnapshotWorkflow>("InventorySnapshotWorkflow");
                    SafeLogInfo($"库存快照工作流执行完成: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                }
                catch (Exception ex)
                {
                    SafeLogInfo($"库存快照工作流执行错误: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"工作流执行错误: {ex.Message}");
                }

                // 改为每天执行一次
                timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            };
            timer.Start();

            SafeLogInfo($"库存快照工作流已调度，下次执行时间: {nextRunTime:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// 调试模式调度：按固定间隔执行
        /// </summary>
        private static async Task ScheduleDebugMode(IWorkflowHost host)
        {
            var timer = new System.Timers.Timer(TimeSpan.FromMinutes(DebugExecutionIntervalMinutes).TotalMilliseconds);
            timer.Elapsed += async (sender, e) =>
            {
                SafeLogInfo($"[调试模式] 开始库存快照工作流执行: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                try
                {
                    // 执行工作流
                    await host.StartWorkflow<InventorySnapshotWorkflow>("InventorySnapshotWorkflow");
                    SafeLogInfo($"[调试模式] 库存快照工作流执行完成: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                }
                catch (Exception ex)
                {
                    SafeLogInfo($"[调试模式] 库存快照工作流执行错误: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[调试模式] 工作流执行错误: {ex.Message}");
                }
            };
            timer.Start();

            // 立即执行一次
            await host.StartWorkflow<InventorySnapshotWorkflow>("InventorySnapshotWorkflow");
            SafeLogInfo($"[调试模式] 库存快照工作流已启动，执行间隔: {DebugExecutionIntervalMinutes}分钟");
        }

        /// <summary>
        /// 手动触发库存快照（用于测试和调试）
        /// </summary>
        public static async Task<bool> TriggerManually(IWorkflowHost host)
        {
            try
            {
                SafeLogInfo($"手动触发库存快照工作流: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await host.StartWorkflow<InventorySnapshotWorkflow>("InventorySnapshotWorkflow");
                SafeLogInfo($"手动触发库存快照工作流完成: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                return true;
            }
            catch (Exception ex)
            {
                SafeLogInfo($"手动触发库存快照工作流错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 安全日志输出方法（避免主窗体未初始化时的空引用异常）
        /// </summary>
        private static void SafeLogInfo(string message)
        {
            try
            {
                if (frmMainNew.Instance != null)
                {
                    frmMainNew.Instance.PrintInfoLog(message);
                }
                else
                {
                    // 主窗体未初始化时，使用控制台输出
                    System.Diagnostics.Debug.WriteLine($"[工作流日志] {message}");
                    Console.WriteLine($"[工作流日志] {message}");
                }
            }
            catch (Exception ex)
            {
                // 如果日志输出失败，使用最基础的方式输出
                System.Diagnostics.Debug.WriteLine($"[工作流日志错误] {message} - 错误: {ex.Message}");
                Console.WriteLine($"[工作流日志错误] {message} - 错误: {ex.Message}");
            }
        }
    }
}
