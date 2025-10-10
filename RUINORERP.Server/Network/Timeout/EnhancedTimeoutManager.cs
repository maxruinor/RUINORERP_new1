using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Timeout
{
    /// <summary>
    /// 增强的超时管理器
    /// 提供更全面的超时监控和处理功能
    /// </summary>
    public class EnhancedTimeoutManager
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ConcurrentDictionary<string, TimeoutRecord> _timeoutRecords;
        private readonly Timer _timeoutAnalysisTimer;
        private readonly TimeSpan _analysisInterval = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _defaultTimeoutThreshold = TimeSpan.FromSeconds(5);
        private readonly object _lockObject = new object();

        public EnhancedTimeoutManager(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _timeoutRecords = new ConcurrentDictionary<string, TimeoutRecord>();
            
            // 启动超时分析定时器
            _timeoutAnalysisTimer = new Timer(AnalyzeTimeouts, null, _analysisInterval, _analysisInterval);
        }

        /// <summary>
        /// 记录超时事件
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <param name="handlerName">处理器名称</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="processingTime">处理时间</param>
        /// <param name="timeoutThreshold">超时阈值</param>
        public void RecordTimeout(string handlerId, string handlerName, uint commandId, TimeSpan processingTime, TimeSpan timeoutThreshold)
        {
            var record = new TimeoutRecord
            {
                Id = Guid.NewGuid().ToString(),
                HandlerId = handlerId,
                HandlerName = handlerName,
                CommandId = commandId,
                ProcessingTime = processingTime,
                TimeoutThreshold = timeoutThreshold,
                Timestamp = DateTime.UtcNow
            };

            _timeoutRecords[record.Id] = record;
        }

        /// <summary>
        /// 获取超时记录
        /// </summary>
        /// <param name="recordId">记录ID</param>
        /// <returns>超时记录</returns>
        public TimeoutRecord GetTimeoutRecord(string recordId)
        {
            return _timeoutRecords.TryGetValue(recordId, out var record) ? record : null;
        }

        /// <summary>
        /// 获取所有超时记录
        /// </summary>
        /// <returns>超时记录列表</returns>
        public List<TimeoutRecord> GetAllTimeoutRecords()
        {
            return _timeoutRecords.Values.ToList();
        }

        /// <summary>
        /// 获取超时统计信息
        /// </summary>
        /// <returns>超时统计信息</returns>
        public TimeoutStatistics GetTimeoutStatistics()
        {
            var records = _timeoutRecords.Values.ToList();
            var now = DateTime.UtcNow;
            var last24Hours = now.AddHours(-24);
            var recentRecords = records.Where(r => r.Timestamp >= last24Hours).ToList();

            return new TimeoutStatistics
            {
                TotalTimeouts = records.Count,
                RecentTimeouts = recentRecords.Count,
                AverageProcessingTime = records.Any() ? records.Average(r => r.ProcessingTime.TotalMilliseconds) : 0,
                AverageTimeoutThreshold = records.Any() ? records.Average(r => r.TimeoutThreshold.TotalMilliseconds) : 0,
                TimeoutByHandler = records.GroupBy(r => r.HandlerName)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TimeoutByCommand = records.GroupBy(r => r.CommandId)
                    .ToDictionary(g => g.Key, g => g.Count()),
                WorstPerformingHandlers = GetWorstPerformingHandlers(records),
                TimeoutTrend = GetTimeoutTrend(recentRecords)
            };
        }

        /// <summary>
        /// 获取性能最差的处理器
        /// </summary>
        /// <param name="records">超时记录</param>
        /// <returns>性能最差的处理器列表</returns>
        private List<WorstPerformingHandler> GetWorstPerformingHandlers(List<TimeoutRecord> records)
        {
            return records.GroupBy(r => r.HandlerName)
                .Select(g => new WorstPerformingHandler
                {
                    HandlerName = g.Key,
                    TimeoutCount = g.Count(),
                    AverageProcessingTime = g.Average(r => r.ProcessingTime.TotalMilliseconds),
                    MaxProcessingTime = g.Max(r => r.ProcessingTime.TotalMilliseconds)
                })
                .OrderByDescending(h => h.TimeoutCount)
                .Take(10)
                .ToList();
        }

        /// <summary>
        /// 获取超时趋势
        /// </summary>
        /// <param name="recentRecords">最近的超时记录</param>
        /// <returns>超时趋势</returns>
        private TimeoutTrend GetTimeoutTrend(List<TimeoutRecord> recentRecords)
        {
            if (!recentRecords.Any()) return new TimeoutTrend();

            var hourlyCounts = new int[24];
            var now = DateTime.UtcNow;

            foreach (var record in recentRecords)
            {
                var hoursAgo = (int)(now - record.Timestamp).TotalHours;
                if (hoursAgo >= 0 && hoursAgo < 24)
                {
                    hourlyCounts[23 - hoursAgo]++;
                }
            }

            return new TimeoutTrend
            {
                HourlyCounts = hourlyCounts,
                TrendDirection = CalculateTrendDirection(hourlyCounts)
            };
        }

        /// <summary>
        /// 计算趋势方向
        /// </summary>
        /// <param name="hourlyCounts">小时计数</param>
        /// <returns>趋势方向</returns>
        private TrendDirection CalculateTrendDirection(int[] hourlyCounts)
        {
            if (hourlyCounts.Length < 2) return TrendDirection.Stable;

            var firstHalf = hourlyCounts.Take(hourlyCounts.Length / 2).Average();
            var secondHalf = hourlyCounts.Skip(hourlyCounts.Length / 2).Average();

            if (secondHalf > firstHalf * 1.1) return TrendDirection.Increasing;
            if (secondHalf < firstHalf * 0.9) return TrendDirection.Decreasing;
            return TrendDirection.Stable;
        }

        /// <summary>
        /// 分析超时情况
        /// </summary>
        /// <param name="state">状态对象</param>
        private void AnalyzeTimeouts(object state)
        {
            lock (_lockObject)
            {
                try
                {
                    var handlers = _commandDispatcher.GetAllHandlers();
                    var timeoutStats = GetTimeoutStatistics();

                    // 检查是否有异常的超时增长
                    if (timeoutStats.TimeoutTrend.TrendDirection == TrendDirection.Increasing)
                    {
                        // 可以在这里添加告警逻辑
                    }

                    // 清理过期的超时记录（保留最近7天）
                    var cutoffTime = DateTime.UtcNow.AddDays(-7);
                    var expiredRecords = _timeoutRecords.Where(kvp => kvp.Value.Timestamp < cutoffTime).ToList();

                    foreach (var expired in expiredRecords)
                    {
                        _timeoutRecords.TryRemove(expired.Key, out _);
                    }
                }
                catch (Exception ex)
                {
                    // 记录分析错误，但不中断定时器
                }
            }
        }

        /// <summary>
        /// 获取超时分析报告
        /// </summary>
        /// <returns>超时分析报告</returns>
        public string GetTimeoutAnalysisReport()
        {
            var stats = GetTimeoutStatistics();
            var report = $"=== 超时分析报告 ===\n";
            report += $"生成时间: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\n\n";

            report += "== 超时统计 ==\n";
            report += $"总超时次数: {stats.TotalTimeouts}\n";
            report += $"最近24小时超时次数: {stats.RecentTimeouts}\n";
            report += $"平均处理时间: {stats.AverageProcessingTime:F2}ms\n";
            report += $"平均超时阈值: {stats.AverageTimeoutThreshold:F2}ms\n";
            report += $"超时趋势: {stats.TimeoutTrend.TrendDirection}\n\n";

            report += "== 按处理器分类 ==\n";
            foreach (var handler in stats.TimeoutByHandler.OrderByDescending(kvp => kvp.Value))
            {
                report += $"  {handler.Key}: {handler.Value}次超时\n";
            }

            report += "\n== 按命令分类 ==\n";
            foreach (var command in stats.TimeoutByCommand.OrderByDescending(kvp => kvp.Value))
            {
                report += $"  命令{command.Key}: {command.Value}次超时\n";
            }

            report += "\n== 性能最差的处理器 ==\n";
            foreach (var handler in stats.WorstPerformingHandlers)
            {
                report += $"  {handler.HandlerName}:\n";
                report += $"    超时次数: {handler.TimeoutCount}\n";
                report += $"    平均处理时间: {handler.AverageProcessingTime:F2}ms\n";
                report += $"    最大处理时间: {handler.MaxProcessingTime:F2}ms\n";
            }

            return report;
        }

        /// <summary>
        /// 重置超时统计
        /// </summary>
        public void ResetTimeoutStatistics()
        {
            _timeoutRecords.Clear();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _timeoutAnalysisTimer?.Dispose();
        }
    }

    /// <summary>
    /// 超时记录
    /// </summary>
    public class TimeoutRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 处理器ID
        /// </summary>
        public string HandlerId { get; set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 命令ID
        /// </summary>
        public uint CommandId { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public TimeSpan ProcessingTime { get; set; }

        /// <summary>
        /// 超时阈值
        /// </summary>
        public TimeSpan TimeoutThreshold { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 超时统计信息
    /// </summary>
    public class TimeoutStatistics
    {
        /// <summary>
        /// 总超时次数
        /// </summary>
        public int TotalTimeouts { get; set; }

        /// <summary>
        /// 最近超时次数
        /// </summary>
        public int RecentTimeouts { get; set; }

        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime { get; set; }

        /// <summary>
        /// 平均超时阈值（毫秒）
        /// </summary>
        public double AverageTimeoutThreshold { get; set; }

        /// <summary>
        /// 按处理器分类的超时次数
        /// </summary>
        public Dictionary<string, int> TimeoutByHandler { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 按命令分类的超时次数
        /// </summary>
        public Dictionary<uint, int> TimeoutByCommand { get; set; } = new Dictionary<uint, int>();

        /// <summary>
        /// 性能最差的处理器
        /// </summary>
        public List<WorstPerformingHandler> WorstPerformingHandlers { get; set; } = new List<WorstPerformingHandler>();

        /// <summary>
        /// 超时趋势
        /// </summary>
        public TimeoutTrend TimeoutTrend { get; set; }
    }

    /// <summary>
    /// 性能最差的处理器
    /// </summary>
    public class WorstPerformingHandler
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 超时次数
        /// </summary>
        public int TimeoutCount { get; set; }

        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime { get; set; }

        /// <summary>
        /// 最大处理时间（毫秒）
        /// </summary>
        public double MaxProcessingTime { get; set; }
    }

    /// <summary>
    /// 超时趋势
    /// </summary>
    public class TimeoutTrend
    {
        /// <summary>
        /// 每小时超时次数
        /// </summary>
        public int[] HourlyCounts { get; set; } = new int[24];

        /// <summary>
        /// 趋势方向
        /// </summary>
        public TrendDirection TrendDirection { get; set; } = TrendDirection.Stable;
    }

    /// <summary>
    /// 趋势方向
    /// </summary>
    public enum TrendDirection
    {
        /// <summary>
        /// 稳定
        /// </summary>
        Stable,

        /// <summary>
        /// 增加
        /// </summary>
        Increasing,

        /// <summary>
        /// 减少
        /// </summary>
        Decreasing
    }
}