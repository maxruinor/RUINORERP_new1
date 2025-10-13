using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 诊断服务基础接口
    /// 定义诊断服务的基本功能
    /// </summary>
    public interface IDiagnosticsService
    {
        /// <summary>
        /// 获取系统诊断报告
        /// </summary>
        /// <returns>诊断报告</returns>
        string GetDiagnosticsReport();

        /// <summary>
        /// 获取系统健康状态
        /// </summary>
        /// <returns>健康状态</returns>
        SystemHealthStatus GetSystemHealth();

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>处理器统计信息字典</returns>
        Dictionary<string, HandlerStatistics> GetHandlerStatistics();

        /// <summary>
        /// 重置所有处理器统计信息
        /// </summary>
        void ResetAllStatistics();
    }

    /// <summary>
    /// 诊断服务
    /// 用于收集和报告系统状态信息
    /// </summary>
    public class DiagnosticsService : IDiagnosticsService
    {
        private readonly CommandDispatcher _commandDispatcher;

        public DiagnosticsService(CommandDispatcher commandDispatcher)
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
    /// 系统健康状态
    /// </summary>
    public class SystemHealthStatus
    {
        /// <summary>
        /// 系统是否健康
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// 成功率（百分比）
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// 总命令数
        /// </summary>
        public long TotalCommands { get; set; }

        /// <summary>
        /// 失败命令数
        /// </summary>
        public long FailedCommands { get; set; }

        /// <summary>
        /// 超时命令数
        /// </summary>
        public long TimeoutCommands { get; set; }

        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime ReportTime { get; set; }

        /// <summary>
        /// 获取健康状态描述
        /// </summary>
        /// <returns>健康状态描述</returns>
        public string GetStatusDescription()
        {
            if (IsHealthy)
            {
                return $"系统健康 (成功率: {SuccessRate:F2}%, 总命令数: {TotalCommands})";
            }
            else
            {
                return $"系统存在问题 (成功率: {SuccessRate:F2}%, 失败数: {FailedCommands}, 超时数: {TimeoutCommands})";
            }
        }
    }
}