using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 错误分析服务
    /// 提供系统错误分析功能
    /// </summary>
    public class ErrorAnalysisService : IErrorAnalysisService
    {
        private readonly CommandDispatcher _commandDispatcher;

        public ErrorAnalysisService(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// 获取错误分析报告
        /// </summary>
        /// <returns>错误分析报告</returns>
        public string GetErrorAnalysisReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== 错误分析报告 ===");
            report.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
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
        /// 重置统计信息
        /// </summary>
        public void ResetStatistics()
        {
            // 由于错误分析服务主要依赖于命令处理器的统计信息
            // 重置所有命令处理器的统计信息
            var handlers = _commandDispatcher.GetAllHandlers();
            foreach (var handler in handlers)
            {
                handler.ResetStatistics();
            }
        }
    }
}