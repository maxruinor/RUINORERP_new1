using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
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
                _logger.LogInformation("开始生成库存快照...");
                var count = _snapshotService.GenerateDailySnapshot();
                _logger.LogInformation($"库存快照生成完成，共生成 {count} 条记录");
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
        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册步骤和工作流
            // 注册服务
            services.AddTransient<InventorySnapshotWorkflow>();
            services.AddTransient<GenerateSnapshotStep>();
            services.AddTransient<CleanupOldSnapshotsStep>();
        }

        /// <summary>
        /// 库存快照（每天凌晨1点执行）
        /// </summary>
        public static async Task<bool> ScheduleInventorySnapshot(IWorkflowHost host)
        {
            try
            {
                // 注册工作流
                host.RegisterWorkflow<InventorySnapshotWorkflow>();

                // 计算下次执行时间
                var now = DateTime.Now;
                var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 1, 0, 0);
                if (nextRunTime <= now)
                    nextRunTime = nextRunTime.AddDays(1);

                // 计算时间间隔
                var interval = nextRunTime - now;

                // 首次延迟执行
                var timer = new System.Timers.Timer(interval.TotalMilliseconds);
                timer.Elapsed += async (sender, e) =>
                {
                    frmMainNew.Instance.PrintInfoLog($"开始工作流执行: {System.DateTime.Now.ToString()}");
                    try
                    {
                        // 执行工作流
                        await host.StartWorkflow<InventorySnapshotWorkflow>("InventorySnapshotWorkflow");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"工作流执行错误: {ex.Message}");
                    }

                    // 改为每天执行一次
                    timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
                };
                timer.Start();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"工作流注册错误: {ex.Message}");
                return false;
            }
        }
    }
}
