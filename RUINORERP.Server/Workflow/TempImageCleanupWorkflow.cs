using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Server.Helpers;
using SqlSugar;
using Autofac;
using System.Timers;

namespace RUINORERP.Server.Workflow
{
    #region 数据模型
    /// <summary>
    /// 临时图片清理工作流数据
    /// </summary>
    public class TempImageCleanupData
    {
        /// <summary>
        /// 临时图片目录
        /// </summary>
        public string TempImageDirectory { get; set; }
        
        /// <summary>
        /// 保留天数
        /// </summary>
        public int RetentionDays { get; set; }
        
        /// <summary>
        /// 清理结果
        /// </summary>
        public TempImageCleanupResult Result { get; set; } = new TempImageCleanupResult();
        
        /// <summary>
        /// 已引用的文件名集合，用于在步骤间共享数据
        /// </summary>
        public HashSet<string> ReferencedFileNames { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
    
    /// <summary>
    /// 临时图片清理结果
    /// </summary>
    public class TempImageCleanupResult
    {
        /// <summary>
        /// 扫描的文件总数
        /// </summary>
        public int TotalFiles { get; set; }
        
        /// <summary>
        /// 删除的文件数
        /// </summary>
        public int DeletedFiles { get; set; }
        
        /// <summary>
        /// 释放的空间（字节）
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
    /// 初始化参数步骤
    /// </summary>
    public class InitializeTempImageCleanupParameters : StepBody
    {
        /// <summary>
        /// 临时图片目录
        /// </summary>
        public string TempImageDirectory { get; set; }
        
        /// <summary>
        /// 保留天数
        /// </summary>
        public int RetentionDays { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as TempImageCleanupData;
            
            // 设置默认值 - 修复：优先使用步骤参数，其次使用数据中的默认值
            data.TempImageDirectory = !string.IsNullOrEmpty(TempImageDirectory) 
                ? TempImageDirectory
                : !string.IsNullOrEmpty(data.TempImageDirectory)
                    ? data.TempImageDirectory
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempImages");
                
            data.RetentionDays = RetentionDays > 0 
                ? RetentionDays
                : data.RetentionDays > 0
                    ? data.RetentionDays
                    : 7;
            
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 获取已引用文件名步骤
    /// </summary>
    public class GetReferencedFileNames : StepBodyAsync
    {
        private readonly IComponentContext _componentContext;
        private readonly ILogger<GetReferencedFileNames> _logger;

        public GetReferencedFileNames(IComponentContext componentContext, ILogger<GetReferencedFileNames> logger)
        {
            _componentContext = componentContext;
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as TempImageCleanupData;
            var referencedFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // 获取文件存储信息控制器
                var fileStorageInfoController = Startup.GetFromFac<tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo>>();

                // 查询所有文件存储信息
                var allFiles = await fileStorageInfoController.BaseQueryAsync(string.Empty);

                // 提取所有已引用的文件名
                foreach (var file in allFiles)
                {
                    if (!string.IsNullOrEmpty(file.StorageFileName))
                    {
                        referencedFileNames.Add(file.StorageFileName);
                    }
                }

                _logger.LogInformation($"已获取{referencedFileNames.Count}个已引用的文件记录");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取已引用文件名失败");
                data.Result.Errors.Add($"获取已引用文件名失败: {ex.Message}");
                // 在出现异常时返回终止执行结果
                return ExecutionResult.Next();
            }

            // 将结果存储在工作流数据对象中
            if (data.Result.Errors.Count == 0)
            {
                // 使用工作流数据对象存储引用文件名集合
                data.Result.TotalFiles = referencedFileNames.Count;
                // 直接存储在工作流数据对象中
                data.ReferencedFileNames = referencedFileNames;
            }
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 清理过期图片步骤
    /// </summary>
    public class CleanupExpiredImages : StepBodyAsync
    {
        private readonly ILogger<CleanupExpiredImages> _logger;

        public CleanupExpiredImages(ILogger<CleanupExpiredImages> logger)
        {
            _logger = logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as TempImageCleanupData;
            
            try
            {
                // 确保临时目录存在
                if (!Directory.Exists(data.TempImageDirectory))
                {
                    _logger.LogInformation($"临时图片目录不存在: {data.TempImageDirectory}");
                    return ExecutionResult.Next();
                }

                // 获取需要保留的截止日期
                var cutoffDate = DateTime.Now.AddDays(-data.RetentionDays);

                // 从工作流数据中获取已引用的文件名
                var referencedFileNames = data.ReferencedFileNames ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // 统计信息
                int totalFiles = 0;
                int deletedFiles = 0;
                long freedBytes = 0;

                // 递归扫描临时目录
                foreach (var file in Directory.EnumerateFiles(data.TempImageDirectory, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        totalFiles++;
                        var fileInfo = new FileInfo(file);

                        // 检查文件最后修改时间是否超过保留期限
                        if (fileInfo.LastWriteTime < cutoffDate)
                        {
                        // 检查文件是否未被引用
                        var fileName = Path.GetFileName(file);
                        if (!referencedFileNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                        {
                            try
                            {
                                // 删除未引用且过期的文件
                                File.Delete(file);
                                deletedFiles++;
                                freedBytes += fileInfo.Length;
                                _logger.LogDebug($"已删除临时图片: {file}");
                            }
                            catch (Exception deleteEx)
                            {
                                _logger.LogWarning(deleteEx, $"删除文件失败: {file}");
                                data.Result.Errors.Add($"删除文件失败: {file} - {deleteEx.Message}");
                            }
                        }
                        else
                        {
                            _logger.LogDebug($"文件被引用，跳过删除: {file}");
                        }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"处理文件时出错: {file}");
                        data.Result.Errors.Add($"处理文件时出错: {file} - {ex.Message}");
                        // 继续处理下一个文件，不中断整个任务
                    }
                }

                // 清理空目录
                CleanEmptyDirectories(data.TempImageDirectory, _logger);

                // 更新结果
                data.Result.TotalFiles = totalFiles;
                data.Result.DeletedFiles = deletedFiles;
                data.Result.FreedBytes = freedBytes;

                _logger.LogInformation($"临时图片清理完成。扫描文件: {totalFiles}, 删除文件: {deletedFiles}, 释放空间: {freedBytes / 1024 / 1024:F2} MB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "临时图片清理失败");
                data.Result.Errors.Add($"临时图片清理失败: {ex.Message}");
            }

            return ExecutionResult.Next();
        }

        /// <summary>
        /// 递归清理空目录
        /// </summary>
        /// <param name="directoryPath">要清理的目录路径</param>
        /// <param name="logger">日志记录器</param>
        private void CleanEmptyDirectories(string directoryPath, ILogger logger)
        {
            try
            {
                // 检查目录是否存在
                if (!Directory.Exists(directoryPath))
                    return;

                // 获取所有子目录（避免在循环中修改目录结构）
                var subDirectories = Directory.GetDirectories(directoryPath);
                
                foreach (var subDir in subDirectories)
                {
                    // 递归清理子目录
                    CleanEmptyDirectories(subDir, logger);

                    // 检查并删除空目录（重新检查目录是否存在）
                    if (Directory.Exists(subDir) && Directory.GetFileSystemEntries(subDir).Length == 0)
                    {
                        try
                        {
                            Directory.Delete(subDir);
                            logger.LogDebug($"已删除空目录: {subDir}");
                        }
                        catch (Exception deleteEx)
                        {
                            logger.LogWarning(deleteEx, $"删除空目录失败: {subDir}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"清理空目录时出错: {directoryPath}");
            }
        }
    }

    /// <summary>
    /// 完成步骤
    /// </summary>
    public class CompleteTempImageCleanup : StepBody
    {
        private readonly ILogger<CompleteTempImageCleanup> _logger;

        public CompleteTempImageCleanup(ILogger<CompleteTempImageCleanup> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var data = context.Workflow.Data as TempImageCleanupData;
            
            // 记录清理结果
            _logger.LogInformation($"临时图片清理任务完成。扫描文件: {data.Result.TotalFiles}, 删除文件: {data.Result.DeletedFiles}, 释放空间: {data.Result.FreedBytes / 1024 / 1024:F2} MB");
            
            if (data.Result.Errors.Any())
            {
                _logger.LogWarning($"临时图片清理任务完成，但有 {data.Result.Errors.Count} 个错误: {string.Join("; ", data.Result.Errors)}");
            }

            return ExecutionResult.Next();
        }
    }
    #endregion

    #region 工作流定义
    /// <summary>
    /// 临时图片清理工作流
    /// </summary>
    public class TempImageCleanupWorkflow : IWorkflow<TempImageCleanupData>
    {
        public string Id => "TempImageCleanupWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<TempImageCleanupData> builder)
        {
            builder
                .StartWith<InitializeTempImageCleanupParameters>()
                    .Input(step => step.TempImageDirectory, data => data.TempImageDirectory)
                    .Input(step => step.RetentionDays, data => data.RetentionDays)
                .Then<GetReferencedFileNames>()
                .Then<CleanupExpiredImages>()
                .Then<CompleteTempImageCleanup>();
        }
    }
    #endregion

    #region 工作流注册与启动
    /// <summary>
    /// 临时图片清理工作流配置
    /// </summary>
    public static class TempImageCleanupWorkflowConfig
    {
        private static System.Timers.Timer _timer;
        
        /// <summary>
        /// 注册工作流
        /// </summary>
        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册步骤和工作流
            services.AddTransient<TempImageCleanupWorkflow>();
            services.AddTransient<InitializeTempImageCleanupParameters>();
            services.AddTransient<GetReferencedFileNames>();
            services.AddTransient<CleanupExpiredImages>();
            services.AddTransient<CompleteTempImageCleanup>();
        }

        /// <summary>
        /// 启动临时图片清理工作流（每天凌晨2点执行）
        /// </summary>
        public static async Task<bool> ScheduleTempImageCleanup(IWorkflowHost host)
        {
            try
            {
                // 注册工作流
                host.RegisterWorkflow<TempImageCleanupWorkflow, TempImageCleanupData>();

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
                    frmMainNew.Instance.PrintInfoLog($"开始执行临时图片清理任务: {System.DateTime.Now.ToString()}");
                    try
                    {
                        // 执行工作流
                        await host.StartWorkflow("TempImageCleanupWorkflow", new TempImageCleanupData());
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"临时图片清理工作流执行错误: {ex.Message}");
                    }

                    // 改为每天执行一次
                    _timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
                };
                _timer.Start();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"临时图片清理工作流注册错误: {ex.Message}");
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