using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 性能监控服务
    /// 提供系统性能相关的监控功能
    /// </summary>
    public class PerformanceMonitoringService : IPerformanceMonitoringService
    {
        private readonly CommandDispatcher _commandDispatcher;

        public PerformanceMonitoringService(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// 获取详细的性能报告
        /// </summary>
        /// <returns>性能报告</returns>
        public string GetPerformanceReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== 系统性能报告 ===");
            report.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
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
        /// 获取实时监控数据
        /// </summary>
        /// <returns>实时监控数据</returns>
        public RealTimeMonitoringData GetRealTimeData()
        {
            var handlers = _commandDispatcher.GetAllHandlers();
            
            return new RealTimeMonitoringData
            {
                Timestamp = DateTime.Now,
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
        /// 重置统计信息
        /// </summary>
        public void ResetStatistics()
        {
            // 由于性能监控服务主要依赖于命令处理器的统计信息
            // 重置所有命令处理器的统计信息
            var handlers = _commandDispatcher.GetAllHandlers();
            foreach (var handler in handlers)
            {
                handler.ResetStatistics();
            }
        }
    }
}