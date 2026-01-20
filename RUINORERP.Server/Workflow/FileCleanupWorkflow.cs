using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using RUINORERP.Server.Network.Services;
using System.Linq;

namespace RUINORERP.Server.Workflow
{
    #region 数据模型
    /// <summary>
    /// 文件清理工作流数据
    /// </summary>
    public class FileCleanupWorkflowData
    {
        /// <summary>
        /// 清理天数阈值
        /// </summary>
        public int ExpiredDaysThreshold { get; set; } = 30;

        /// <summary>
        /// 孤立文件天数阈值
        /// </summary>
        public int OrphanedDaysThreshold { get; set; } = 7;

        /// <summary>
        /// 是否物理删除
        /// </summary>
        public bool PhysicalDelete { get; set; } = false;

        /// <summary>
        /// 清理结果
        /// </summary>
        public FileCleanupResult Result { get; set; } = new FileCleanupResult();
    }

    /// <summary>
    /// 文件清理结果
    /// </summary>
    public class FileCleanupResult
    {
        /// <summary>
        /// 过期文件清理数量
        /// </summary>
        public int ExpiredFilesCount { get; set; }

        /// <summary>
        /// 孤立文件清理数量
        /// </summary>
        public int OrphanedFilesCount { get; set; }

        /// <summary>
        /// 物理孤立文件清理数量
        /// </summary>
        public int PhysicalOrphanedFilesCount { get; set; }

        /// <summary>
        /// 释放的存储空间(字节)
        /// </summary>
        public long FreedBytes { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
    #endregion

    #region 工作流步骤
    /// <summary>
    /// 初始化清理参数步骤
    /// </summary>
    public class InitializeCleanupParameters : StepBody
    {
        /// <summary>
        /// 清理天数阈值
        /// </summary>
        public int ExpiredDaysThreshold { get; set; }

        /// <summary>
        /// 孤立文件天数阈值
        /// </summary>
        public int OrphanedDaysThreshold { get; set; }

        /// <summary>
        /// 是否物理删除
        /// </summary>
        public bool PhysicalDelete { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as FileCleanupWorkflowData;

            // 设置参数值
            data.ExpiredDaysThreshold = ExpiredDaysThreshold > 0 ? ExpiredDaysThreshold : 30;
            data.OrphanedDaysThreshold = OrphanedDaysThreshold > 0 ? OrphanedDaysThreshold : 7;
            data.PhysicalDelete = PhysicalDelete;

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 清理过期文件步骤
    /// </summary>
    public class CleanupExpiredFilesStep : StepBodyAsync
    {
        private readonly FileCleanupService _fileCleanupService;
        private readonly ILogger<CleanupExpiredFilesStep> _logger;

        public CleanupExpiredFilesStep(FileCleanupService fileCleanupService, ILogger<CleanupExpiredFilesStep> logger)
        {
            _fileCleanupService = fileCleanupService;
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as FileCleanupWorkflowData;

            try
            {
                _logger.LogInformation($"开始清理过期文件,阈值: {data.ExpiredDaysThreshold}天");

                var expiredCount = await _fileCleanupService.CleanupExpiredFilesAsync(
                    daysThreshold: data.ExpiredDaysThreshold,
                    physicalDelete: data.PhysicalDelete
                );

                data.Result.ExpiredFilesCount = expiredCount;
                _logger.LogInformation($"过期文件清理完成,清理数量: {expiredCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期文件失败");
                data.Result.Errors.Add($"清理过期文件失败: {ex.Message}");
            }

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 清理孤立文件步骤
    /// </summary>
    public class CleanupOrphanedFilesStep : StepBodyAsync
    {
        private readonly FileCleanupService _fileCleanupService;
        private readonly ILogger<CleanupOrphanedFilesStep> _logger;

        public CleanupOrphanedFilesStep(FileCleanupService fileCleanupService, ILogger<CleanupOrphanedFilesStep> logger)
        {
            _fileCleanupService = fileCleanupService;
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as FileCleanupWorkflowData;

            try
            {
                _logger.LogInformation($"开始清理孤立文件,阈值: {data.OrphanedDaysThreshold}天");

                var orphanedCount = await _fileCleanupService.CleanupOrphanedFilesAsync(
                    daysThreshold: data.OrphanedDaysThreshold,
                    physicalDelete: data.PhysicalDelete
                );

                data.Result.OrphanedFilesCount = orphanedCount;
                _logger.LogInformation($"孤立文件清理完成,清理数量: {orphanedCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理孤立文件失败");
                data.Result.Errors.Add($"清理孤立文件失败: {ex.Message}");
            }

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 清理物理孤立文件步骤
    /// </summary>
    public class CleanupPhysicalOrphanedFilesStep : StepBodyAsync
    {
        private readonly FileCleanupService _fileCleanupService;
        private readonly ILogger<CleanupPhysicalOrphanedFilesStep> _logger;

        public CleanupPhysicalOrphanedFilesStep(FileCleanupService fileCleanupService, ILogger<CleanupPhysicalOrphanedFilesStep> logger)
        {
            _fileCleanupService = fileCleanupService;
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as FileCleanupWorkflowData;

            try
            {
                _logger.LogInformation("开始清理物理孤立文件");

                var physicalCount = await _fileCleanupService.CleanupPhysicalOrphanedFilesAsync();

                data.Result.PhysicalOrphanedFilesCount = physicalCount;
                _logger.LogInformation($"物理孤立文件清理完成,清理数量: {physicalCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理物理孤立文件失败");
                data.Result.Errors.Add($"清理物理孤立文件失败: {ex.Message}");
            }

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 获取统计信息步骤
    /// </summary>
    public class GetCleanupStatisticsStep : StepBodyAsync
    {
        private readonly FileCleanupService _fileCleanupService;
        private readonly ILogger<GetCleanupStatisticsStep> _logger;

        public GetCleanupStatisticsStep(FileCleanupService fileCleanupService, ILogger<GetCleanupStatisticsStep> logger)
        {
            _fileCleanupService = fileCleanupService;
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as FileCleanupWorkflowData;

            try
            {
                var stats = await _fileCleanupService.GetCleanupStatisticsAsync();
                
                // 计算释放的存储空间
                long totalStorageBefore = (stats.ExpiredFiles + stats.OrphanedFiles) * 1024 * 1024; // 估算
                data.Result.FreedBytes = (stats.ExpiredFiles + stats.OrphanedFiles + data.Result.PhysicalOrphanedFilesCount) * 1024 * 1024;

                _logger.LogInformation($"文件存储统计 - 总文件: {stats.TotalFiles}, 正常: {stats.ActiveFiles}, 过期: {stats.ExpiredFiles}, 孤立: {stats.OrphanedFiles}, 总存储: {stats.TotalStorageSizeFormatted}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取统计信息失败");
                data.Result.Errors.Add($"获取统计信息失败: {ex.Message}");
            }

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 完成步骤
    /// </summary>
    public class CompleteFileCleanup : StepBody
    {
        private readonly ILogger<CompleteFileCleanup> _logger;

        public CompleteFileCleanup(ILogger<CompleteFileCleanup> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as FileCleanupWorkflowData;

            var freedMB = data.Result.FreedBytes / 1024.0 / 1024.0;

            _logger.LogInformation($"文件清理任务完成");
            _logger.LogInformation($"  过期文件: {data.Result.ExpiredFilesCount}");
            _logger.LogInformation($"  孤立文件: {data.Result.OrphanedFilesCount}");
            _logger.LogInformation($"  物理孤立文件: {data.Result.PhysicalOrphanedFilesCount}");
            _logger.LogInformation($"  释放空间: {freedMB:F2} MB");

            if (data.Result.Errors.Any())
            {
                _logger.LogWarning($"文件清理任务完成,但有 {data.Result.Errors.Count} 个错误: {string.Join("; ", data.Result.Errors)}");
            }

            return ExecutionResult.Next();
        }
    }
    #endregion

    #region 工作流定义
    /// <summary>
    /// 文件清理工作流
    /// 每天凌晨2点自动清理过期和孤立文件
    /// </summary>
    public class FileCleanupWorkflow : IWorkflow<FileCleanupWorkflowData>
    {
        public string Id => "FileCleanupWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<FileCleanupWorkflowData> builder)
        {
            builder
                .StartWith<InitializeCleanupParameters>()
                    .Input(step => step.ExpiredDaysThreshold, data => data.ExpiredDaysThreshold)
                    .Input(step => step.OrphanedDaysThreshold, data => data.OrphanedDaysThreshold)
                    .Input(step => step.PhysicalDelete, data => data.PhysicalDelete)
                .Then<CleanupExpiredFilesStep>()
                .Then<CleanupOrphanedFilesStep>()
                .Then<CleanupPhysicalOrphanedFilesStep>()
                .Then<GetCleanupStatisticsStep>()
                .Then<CompleteFileCleanup>();
        }
    }
    #endregion

    #region 工作流注册与启动
    /// <summary>
    /// 文件清理工作流配置
    /// </summary>
    public static class FileCleanupWorkflowConfig
    {
        private static System.Timers.Timer _timer;

        /// <summary>
        /// 注册工作流
        /// </summary>
        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册工作流
            services.AddTransient<FileCleanupWorkflow>();
            services.AddTransient<InitializeCleanupParameters>();
            services.AddTransient<CleanupExpiredFilesStep>();
            services.AddTransient<CleanupOrphanedFilesStep>();
            services.AddTransient<CleanupPhysicalOrphanedFilesStep>();
            services.AddTransient<GetCleanupStatisticsStep>();
            services.AddTransient<CompleteFileCleanup>();
        }

        /// <summary>
        /// 启动文件清理工作流(每天凌晨2点执行)
        /// </summary>
        public static async Task<bool> ScheduleFileCleanup(IWorkflowHost host)
        {
            try
            {
                // 注册工作流
                host.RegisterWorkflow<FileCleanupWorkflow, FileCleanupWorkflowData>();

                // 计算下次执行时间
                var now = DateTime.Now;
                var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);
                if (nextRunTime <= now)
                    nextRunTime = nextRunTime.AddDays(1);

                // 计算时间间隔
                var interval = nextRunTime - now;

                // 首次延迟执行
                _timer = new System.Timers.Timer(interval.TotalMilliseconds);
                _timer.Elapsed += async (sender, e) =>
                {
                    frmMainNew.Instance?.PrintInfoLog($"开始执行文件清理任务: {DateTime.Now}");
                    try
                    {
                        // 执行工作流
                        await host.StartWorkflow("FileCleanupWorkflow", new FileCleanupWorkflowData());
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"文件清理工作流执行错误: {ex.Message}");
                    }

                    // 改为每天执行一次
                    _timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
                };
                _timer.Start();

                frmMainNew.Instance?.PrintInfoLog($"文件清理工作流已启动,首次执行时间: {nextRunTime}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"文件清理工作流注册错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    }
    #endregion
}
