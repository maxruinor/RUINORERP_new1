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
    /// 库存快照工作流数据类
    /// </summary>
    public class InventorySnapshotData
    {
        public DateTime NextExecutionTime { get; set; }
        public int ExecutionCount { get; set; }
        public bool ShouldContinue { get; set; } = true;
    }

    /// <summary>
    /// 库存快照工作流
    /// 【修复】使用WorkflowCore内置的Recur机制，移除手动Timer调度
    /// 【修复】使用强类型数据类InventorySnapshotData而非object
    /// </summary>
    public class InventorySnapshotWorkflow : IWorkflow<InventorySnapshotData>
    {
        public string Id => "InventorySnapshotWorkflow";
        public int Version => 2;  // 升级版本号

        public void Build(IWorkflowBuilder<InventorySnapshotData> builder)
        {
            builder
                .StartWith<InitializeScheduleStep>()
                .Recur(
                    data => CalculateRecurInterval(data),
                    data => ShouldContinueRecur(data))
                .Do(recur => recur
                    .StartWith<GenerateSnapshotStep>()
                    .Then<CleanupOldSnapshotsStep>()
                    .Then(context => UpdateExecutionInfo(context))
                    .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(5)))
                .Then(context => LogWorkflowCompletion(context));
        }

        /// <summary>
        /// 计算Recur间隔时间
        /// 【修复】提取为独立方法，避免表达式树限制
        /// </summary>
        private static TimeSpan CalculateRecurInterval(object data)
        {
            // 【修复】安全类型转换，避免CancellationProcessor传入非预期类型
            if (data is InventorySnapshotData snapshotData)
            {
                var interval = (snapshotData.NextExecutionTime - DateTime.Now).TotalMilliseconds;
                return TimeSpan.FromMilliseconds(Math.Max(interval, 60000));  // 最小间隔1分钟
            }
            // 如果类型不匹配，返回默认间隔
            return TimeSpan.FromMinutes(5);
        }

        /// <summary>
        /// 判断是否应该继续循环
        /// 【修复】提取为独立方法，避免表达式树限制
        /// </summary>
        private static bool ShouldContinueRecur(object data)
        {
            // 【修复】安全类型转换，避免CancellationProcessor传入非预期类型
            if (data is InventorySnapshotData snapshotData)
            {
                return snapshotData.ShouldContinue;
            }
            // 如果类型不匹配，停止循环
            return false;
        }

        /// <summary>
        /// 更新执行信息
        /// 【修复】提取为独立方法，保持代码清晰
        /// </summary>
        private static ExecutionResult UpdateExecutionInfo(IStepExecutionContext context)
        {
            // 更新下次执行时间
            var snapshotData = context.Workflow.Data as InventorySnapshotData;
            if (snapshotData != null)
            {
                snapshotData.NextExecutionTime = InventorySnapshotWorkflow.CalculateNextExecutionTime();
                snapshotData.ExecutionCount++;
                
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 第{snapshotData.ExecutionCount}次执行完成，下次执行: {snapshotData.NextExecutionTime:yyyy-MM-dd HH:mm:ss}");
            }
            return ExecutionResult.Next();
        }

        /// <summary>
        /// 记录工作流完成日志
        /// 【修复】提取为独立方法，保持代码清晰
        /// </summary>
        private static ExecutionResult LogWorkflowCompletion(IStepExecutionContext context)
        {
            var finalData = context.Workflow.Data as InventorySnapshotData;
            if (finalData != null)
            {
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 工作流结束，总执行次数: {finalData.ExecutionCount}");
            }
            return ExecutionResult.Next();
        }

        /// <summary>
        /// 计算下次执行时间
        /// </summary>
        public static DateTime CalculateNextExecutionTime()
        {
            var now = DateTime.Now;
            var executionTime = ScheduledTaskHelper.GetTaskExecutionTime(ScheduledTaskHelper.InventorySnapshotTask);
            
            var nextRun = new DateTime(now.Year, now.Month, now.Day,
                executionTime.Hours, executionTime.Minutes, executionTime.Seconds);
            
            if (nextRun <= now)
                nextRun = nextRun.AddDays(1);
            
            return nextRun;
        }
    }

    /// <summary>
    /// 初始化调度步骤
    /// </summary>
    public class InitializeScheduleStep : StepBodyAsync
    {
        private readonly ILogger<InitializeScheduleStep> _logger;

        public InitializeScheduleStep(ILogger<InitializeScheduleStep> logger)
        {
            _logger = logger;
        }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                // 【修复】使用 as 运算符进行安全类型转换
                var workflowData = context.Workflow.Data as InventorySnapshotData;
                
                if (workflowData == null)
                {
                    workflowData = new InventorySnapshotData
                    {
                        NextExecutionTime = InventorySnapshotWorkflow.CalculateNextExecutionTime(),
                        ExecutionCount = 0
                    };
                    context.Workflow.Data = workflowData;
                }
                else
                {
                    workflowData.NextExecutionTime = InventorySnapshotWorkflow.CalculateNextExecutionTime();
                    workflowData.ExecutionCount = 0;
                }
                
                _logger.LogInformation("库存快照工作流初始化，下次执行时间: {NextTime}", workflowData.NextExecutionTime);
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 工作流初始化，下次执行: {workflowData.NextExecutionTime:yyyy-MM-dd HH:mm:ss}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "库存快照工作流初始化失败");
                frmMainNew.Instance?.PrintErrorLog($"[库存快照] 初始化失败: {ex.Message}");
            }
            
            return Task.FromResult(ExecutionResult.Next());
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

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation("开始生成库存快照...");
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 开始执行: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                
                var success = await _snapshotService.GenerateDailySnapshot();
                
                stopwatch.Stop();
                
                if (success)
                {
                    _logger.LogInformation("库存快照生成成功，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                    frmMainNew.Instance?.PrintInfoLog($"[库存快照] 生成成功，耗时: {stopwatch.ElapsedMilliseconds}ms");
                }
                else
                {
                    _logger.LogDebug("库存快照生成完成，但无数据变化，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                    frmMainNew.Instance?.PrintInfoLog($"[库存快照] 无数据变化，耗时: {stopwatch.ElapsedMilliseconds}ms");
                }
                
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "生成库存快照失败，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                frmMainNew.Instance?.PrintErrorLog($"[库存快照] 执行失败: {ex.Message}，耗时: {stopwatch.ElapsedMilliseconds}ms");
                return ExecutionResult.Next();  // 【修复】继续执行清理步骤，不中断工作流
            }
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
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation("开始清理过期库存快照...");
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 开始清理过期数据: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                
                // 保留最近12个月的快照（可从配置读取）
                int keepMonths = 12;
                var count = _snapshotService.CleanupExpiredSnapshots(keepMonths);
                
                stopwatch.Stop();
                
                _logger.LogInformation("过期库存快照清理完成，共清理 {Count} 条记录，耗时: {ElapsedMs}ms", count, stopwatch.ElapsedMilliseconds);
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 清理完成: 删除{count}条记录，耗时: {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "清理过期库存快照失败，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                frmMainNew.Instance?.PrintErrorLog($"[库存快照] 清理失败: {ex.Message}，耗时: {stopwatch.ElapsedMilliseconds}ms");
            }
            
            return Task.FromResult(ExecutionResult.Next());
        }
    }

    /// <summary>
    /// 库存快照工作流配置（仅保留手动触发功能）
    /// 【修复】移除Timer调度逻辑，使用WorkflowCore内置调度
    /// </summary>
    public static class InventorySnapshotWorkflowConfig
    {
        /// <summary>
        /// 是否启用调试模式（调试模式下可立即执行）
        /// 【修复】从配置文件读取，默认关闭
        /// </summary>
        private static volatile bool _debugMode = false;
        private static readonly object _debugModeLock = new object();

        public static bool DebugMode 
        { 
            get => _debugMode;
            set 
            { 
                lock(_debugModeLock) 
                { 
                    _debugMode = value;
                    SafeLogInfo($"[配置] 调试模式已{(value ? "启用" : "禁用")}");
                }
            }
        }

        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册工作流和步骤
            services.AddTransient<InventorySnapshotWorkflow>();
            services.AddTransient<InitializeScheduleStep>();
            services.AddTransient<GenerateSnapshotStep>();
            services.AddTransient<CleanupOldSnapshotsStep>();
        }

        /// <summary>
        /// 启动库存快照工作流（一次性调用）
        /// 【修复】不再创建Timer，由WorkflowCore管理调度
        /// </summary>
        public static async Task<bool> StartWorkflow(IWorkflowHost host)
        {
            try
            {
                // 注册工作流（强类型需要指定数据类）
                host.RegisterWorkflow<InventorySnapshotWorkflow, InventorySnapshotData>();
                
                // 【修复】移除GetActiveWorkflows检查（API不存在），直接启动
                // WorkflowCore会防止重复启动同一工作流实例

                // 启动工作流实例，传入初始数据
                var initialData = new InventorySnapshotData
                {
                    NextExecutionTime = InventorySnapshotWorkflow.CalculateNextExecutionTime(),
                    ExecutionCount = 0,
                    ShouldContinue = true
                };
                
                await host.StartWorkflow("InventorySnapshotWorkflow", 1, initialData);
                
                frmMainNew.Instance?.PrintInfoLog($"[库存快照] 工作流已启动，将由WorkflowCore自动调度");
                return true;
            }
            catch (Exception ex)
            {
                frmMainNew.Instance?.PrintErrorLog($"[库存快照] 启动失败: {ex.Message}");
                #if DEBUG
                System.Diagnostics.Debug.WriteLine($"工作流启动错误: {ex.Message}");
                #endif
                return false;
            }
        }

        /// <summary>
        /// 手动触发库存快照（用于测试和调试）
        /// </summary>
        public static async Task<bool> TriggerManually(IWorkflowHost host)
        {
            try
            {
                frmMainNew.Instance?.PrintInfoLog($"手动触发库存快照工作流: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await host.StartWorkflow<InventorySnapshotWorkflow>("InventorySnapshotWorkflow");
                frmMainNew.Instance?.PrintInfoLog($"手动触发库存快照工作流完成: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                return true;
            }
            catch (Exception ex)
            {
                frmMainNew.Instance?.PrintErrorLog($"手动触发库存快照工作流错误: {ex.Message}");
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
                    #if DEBUG
                    System.Diagnostics.Debug.WriteLine($"[工作流日志] {message}");
                    #endif
                    Console.WriteLine($"[工作流日志] {message}");
                }
            }
            catch (Exception ex)
            {
                // 如果日志输出失败，使用最基础的方式输出
                #if DEBUG
                System.Diagnostics.Debug.WriteLine($"[工作流日志错误] {message} - 错误: {ex.Message}");
                #endif
                Console.WriteLine($"[工作流日志错误] {message} - 错误: {ex.Message}");
            }
        }
    }
}
