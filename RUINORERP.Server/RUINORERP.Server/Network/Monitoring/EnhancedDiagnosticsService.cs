using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 增强的诊断服务
    /// 提供更全面的系统监控和诊断功能
    /// </summary>
    public class EnhancedDiagnosticsService
    {
        private readonly CommandDispatcher _commandDispatcher;

        public EnhancedDiagnosticsService(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// 获取系统诊断报告
        /// </summary>
        /// <returns>诊断报告</returns>
        public string GetDiagnosticsReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== 系统诊断报告 ===");
            report.AppendLine($"生成时间: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            // 获取命令调度器信息
            report.AppendLine("== 命令调度器信息 ==");
            report.AppendLine($"是否已初始化: {_commandDispatcher.IsInitialized}");
            report.AppendLine($"处理器数量: {_commandDispatcher.HandlerCount}");
            report.AppendLine();

            // 获取处理器统计信息
            report.AppendLine("== 处理器统计信息 ==");
            var handlers = _commandDispatcher.GetAllHandlers();
            foreach (var handler in handlers)
            {
                report.AppendLine($"处理器: {handler.Name}");
                report.AppendLine($"  状态: {handler.Status}");
                report.AppendLine($"  优先级: {handler.Priority}");
                report.AppendLine($"  是否已初始化: {handler.IsInitialized}");
                
                var statistics = handler.GetStatistics();
                report.AppendLine($"  {statistics.GetStatisticsReport()}");
                report.AppendLine();
            }

            return report.ToString();
        }

        /// <summary>
        /// 获取详细的性能报告
        /// </summary>
        /// <returns>性能报告</returns>
        public string GetPerformanceReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== 系统性能报告 ===");
            report.AppendLine($"生成时间: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            var handlers = _commandDispatcher.GetAllHandlers();
            var totalCommands = handlers.Sum(h => h.GetStatistics().TotalCommandsProcessed);
            var totalSuccess = handlers.Sum(h => h.GetStatistics().SuccessfulCommands);
            var totalFailed = handlers.Sum(h => h.GetStatistics().FailedCommands);
            var totalTimeouts = handlers.Sum(h => h.GetStatistics().TimeoutCount);
            
            report.AppendLine("== 总体性能 ==");
            report.AppendLine($"总处理命令数: {totalCommands}");
            report.AppendLine($"成功处理数: {totalSuccess}");
            report.AppendLine($"失败处理数: {totalFailed}");
            report.AppendLine($"超时次数: {totalTimeouts}");
            report.AppendLine($"成功率: {(totalCommands > 0 ? (double)totalSuccess / totalCommands * 100 : 0):F2}%");
            report.AppendLine();

            // 按处理时间排序的处理器
            var handlersByTime = handlers.OrderByDescending(h => h.GetStatistics().AverageProcessingTimeMs).ToList();
            report.AppendLine("== 处理器性能排行（按平均处理时间） ==");
            foreach (var handler in handlersByTime.Take(10)) // 只显示前10个
            {
                var stats = handler.GetStatistics();
                report.AppendLine($"  {handler.Name}: {stats.AverageProcessingTimeMs:F2}ms");
            }

            return report.ToString();
        }

        /// <summary>
        /// 获取错误分析报告
        /// </summary>
        /// <returns>错误分析报告</returns>
        public string GetErrorAnalysisReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== 错误分析报告 ===");
            report.AppendLine($"生成时间: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            var handlers = _commandDispatcher.GetAllHandlers();
            var errorHandlers = handlers.Where(h => h.GetStatistics().FailedCommands > 0 || 
                                                   h.GetStatistics().LastErrorTime.HasValue)
                                       .ToList();

            if (!errorHandlers.Any())
            {
                report.AppendLine("未发现错误记录");
                return report.ToString();
            }

            report.AppendLine("== 有错误记录的处理器 ==");
            foreach (var handler in errorHandlers)
            {
                var stats = handler.GetStatistics();
                report.AppendLine($"处理器: {handler.Name}");
                report.AppendLine($"  失败命令数: {stats.FailedCommands}");
                report.AppendLine($"  超时次数: {stats.TimeoutCount}");
                
                if (stats.LastErrorTime.HasValue)
                {
                    report.AppendLine($"  最后错误时间: {stats.LastErrorTime:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine($"  最后错误信息: {stats.LastError}");
                    if (!string.IsNullOrEmpty(stats.LastErrorStackTrace))
                    {
                        report.AppendLine($"  错误堆栈: {stats.LastErrorStackTrace}");
                    }
                }
                report.AppendLine();
            }

            return report.ToString();
        }

        /// <summary>
        /// 获取实时监控数据
        /// </summary>
        /// <returns>实时监控数据</returns>
        public RealTimeMonitoringData GetRealTimeData()
        {
            var handlers = _commandDispatcher.GetAllHandlers();
            
            return new RealTimeMonitoringData
            {
                Timestamp = DateTime.UtcNow,
                TotalHandlers = handlers.Count(),
                ActiveHandlers = handlers.Count(h => h.Status == HandlerStatus.Running),
                TotalCommandsProcessed = handlers.Sum(h => h.GetStatistics().TotalCommandsProcessed),
                CurrentProcessing = handlers.Sum(h => h.GetStatistics().CurrentProcessingCount),
                SuccessRate = CalculateSuccessRate(handlers),
                AverageProcessingTime = handlers.Any() ? 
                    handlers.Average(h => h.GetStatistics().AverageProcessingTimeMs) : 0
            };
        }

        /// <summary>
        /// 计算成功率
        /// </summary>
        /// <param name="handlers">处理器列表</param>
        /// <returns>成功率</returns>
        private double CalculateSuccessRate(IEnumerable<ICommandHandler> handlers)
        {
            var total = handlers.Sum(h => h.GetStatistics().TotalCommandsProcessed);
            var success = handlers.Sum(h => h.GetStatistics().SuccessfulCommands);
            return total > 0 ? (double)success / total * 100 : 100;
        }

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>处理器统计信息字典</returns>
        public Dictionary<string, HandlerStatistics> GetHandlerStatistics()
        {
            return _commandDispatcher.GetHandlerStatistics();
        }

        /// <summary>
        /// 重置所有处理器统计信息
        /// </summary>
        public void ResetAllStatistics()
        {
            var handlers = _commandDispatcher.GetAllHandlers();
            foreach (var handler in handlers)
            {
                handler.ResetStatistics();
            }
        }

        /// <summary>
        /// 获取系统健康状态
        /// </summary>
        /// <returns>健康状态</returns>
        public SystemHealthStatus GetSystemHealth()
        {
            var handlers = _commandDispatcher.GetAllHandlers();
            var totalCommands = handlers.Sum(h => h.GetStatistics().TotalCommandsProcessed);
            var failedCommands = handlers.Sum(h => h.GetStatistics().FailedCommands);
            var timeoutCommands = handlers.Sum(h => h.GetStatistics().TimeoutCount);
            
            var successRate = totalCommands > 0 ? (double)(totalCommands - failedCommands) / totalCommands * 100 : 100;
            
            return new SystemHealthStatus
            {
                IsHealthy = successRate > 95 && timeoutCommands < 10,
                SuccessRate = successRate,
                TotalCommands = totalCommands,
                FailedCommands = failedCommands,
                TimeoutCommands = timeoutCommands,
                ReportTime = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// 实时监控数据
    /// </summary>
    public class RealTimeMonitoringData
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 总处理器数
        /// </summary>
        public int TotalHandlers { get; set; }

        /// <summary>
        /// 活跃处理器数
        /// </summary>
        public int ActiveHandlers { get; set; }

        /// <summary>
        /// 总处理命令数
        /// </summary>
        public long TotalCommandsProcessed { get; set; }

        /// <summary>
        /// 当前正在处理的命令数
        /// </summary>
        public int CurrentProcessing { get; set; }

        /// <summary>
        /// 成功率（百分比）
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime { get; set; }
    }
}