using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 审计日志性能监控类
    /// 提供队列状态、刷新性能、错误统计等监控指标
    /// </summary>
    public class AuditLogMetrics
    {
        private readonly ILogger _logger;
        private readonly object _lock = new object();
        
        /// <summary>
        /// 队列大小
        /// </summary>
        public int QueueSize { get; set; }

        /// <summary>
        /// 总日志数
        /// </summary>
        public long TotalLogs { get; private set; }

        private long _totalLogsField = 0;

        /// <summary>
        /// 成功刷新次数
        /// </summary>
        public long SuccessFlushes { get; private set; }

        /// <summary>
        /// 失败刷新次数
        /// </summary>
        public long FailedFlushes { get; private set; }

        /// <summary>
        /// 丢弃日志数
        /// </summary>
        public long DroppedLogs { get; private set; }

        private long _droppedLogsField = 0;

        /// <summary>
        /// 上次刷新耗时
        /// </summary>
        public TimeSpan LastFlushDuration { get; set; }

        /// <summary>
        /// 平均刷新耗时
        /// </summary>
        public TimeSpan AverageFlushDuration { get; set; }

        /// <summary>
        /// 最大刷新耗时
        /// </summary>
        public TimeSpan MaxFlushDuration { get; set; }

        /// <summary>
        /// 刷新耗时历史（最近100次）
        /// </summary>
        private readonly Queue<TimeSpan> _flushDurationHistory = new Queue<TimeSpan>(100);

        /// <summary>
        /// 上次刷新时间
        /// </summary>
        public DateTime? LastFlushTime { get; set; }

        /// <summary>
        /// 服务启动时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 当前重试次数
        /// </summary>
        public int CurrentRetryCount { get; set; }

        public AuditLogMetrics(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 记录入队日志
        /// </summary>
        public void LogEnqueue(int count)
        {
            var newValue = Interlocked.Add(ref _totalLogsField, count);
            TotalLogs = newValue;
        }

        /// <summary>
        /// 记录刷新开始
        /// </summary>
        public void LogFlushStart()
        {
            // 重置当前重试计数
            CurrentRetryCount = 0;
        }

        /// <summary>
        /// 记录刷新成功
        /// </summary>
        /// <param name="duration">刷新耗时</param>
        /// <param name="logCount">刷新的日志数量</param>
        public void LogFlushSuccess(TimeSpan duration, int logCount)
        {
            lock (_lock)
            {
                SuccessFlushes++;
                LastFlushDuration = duration;
                LastFlushTime = DateTime.Now;

                // 更新耗时历史
                _flushDurationHistory.Enqueue(duration);
                if (_flushDurationHistory.Count > 100)
                {
                    _flushDurationHistory.Dequeue();
                }

                // 计算平均耗时
                AverageFlushDuration = TimeSpan.FromTicks(
                    _flushDurationHistory.Sum(t => t.Ticks) / _flushDurationHistory.Count
                );

                // 更新最大耗时
                if (duration > MaxFlushDuration)
                {
                    MaxFlushDuration = duration;
                }

                _logger.LogDebug(
                    $"审计日志刷新成功 | 数量:{logCount} | 耗时:{duration.TotalMilliseconds:F2}ms | " +
                    $"平均耗时:{AverageFlushDuration.TotalMilliseconds:F2}ms | " +
                    $"队列大小:{QueueSize}"
                );
            }
        }

        /// <summary>
        /// 记录刷新失败
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void LogFlushFailure(Exception ex)
        {
            lock (_lock)
            {
                FailedFlushes++;
                CurrentRetryCount++;
                
                _logger.LogError(
                    ex,
                    $"审计日志刷新失败 | 重试次数:{CurrentRetryCount} | " +
                    $"队列大小:{QueueSize} | 失败总数:{FailedFlushes}"
                );
            }
        }

        /// <summary>
        /// 记录日志丢弃
        /// </summary>
        /// <param name="reason">丢弃原因</param>
        public void LogDropped(string reason)
        {
            var newValue = Interlocked.Increment(ref _droppedLogsField);
            DroppedLogs = newValue;
            
            _logger.LogWarning(
                $"审计日志被丢弃 | 原因:{reason} | " +
                $"总丢弃数:{DroppedLogs} | 当前队列:{QueueSize}"
            );
        }

        /// <summary>
        /// 获取性能统计报告
        /// </summary>
        public string GetPerformanceReport()
        {
            var uptime = DateTime.Now - StartTime;
            var successRate = SuccessFlushes + FailedFlushes > 0
                ? (double)SuccessFlushes / (SuccessFlushes + FailedFlushes) * 100
                : 100;

            var throughput = uptime.TotalSeconds > 0
                ? TotalLogs / uptime.TotalSeconds
                : 0;

            return $"""
                ====== 审计日志性能报告 ======
                运行时长: {uptime:hh\:mm\:ss}
                总日志数: {TotalLogs:N0}
                成功刷新: {SuccessFlushes:N0}
                失败刷新: {FailedFlushes:N0}
                丢弃日志: {DroppedLogs:N0}
                成功率: {successRate:F2}%
                吞吐量: {throughput:F2} logs/s
                
                当前队列: {QueueSize}
                上次刷新: {LastFlushTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "无"}
                上次耗时: {LastFlushDuration.TotalMilliseconds:F2}ms
                平均耗时: {AverageFlushDuration.TotalMilliseconds:F2}ms
                最大耗时: {MaxFlushDuration.TotalMilliseconds:F2}ms
                ================================
                """;
        }

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                TotalLogs = 0;
                _totalLogsField = 0;
                SuccessFlushes = 0;
                FailedFlushes = 0;
                DroppedLogs = 0;
                _droppedLogsField = 0;
                LastFlushDuration = TimeSpan.Zero;
                AverageFlushDuration = TimeSpan.Zero;
                MaxFlushDuration = TimeSpan.Zero;
                _flushDurationHistory.Clear();
                LastFlushTime = null;
                StartTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 检查是否需要告警
        /// </summary>
        /// <param name="maxQueueSize">最大队列大小</param>
        /// <returns>告警信息，无告警返回null</returns>
        public string CheckAlerts(int maxQueueSize)
        {
            var alerts = new List<string>();

            // 队列告警
            if (QueueSize > maxQueueSize * 0.9)
            {
                alerts.Add($"队列使用率超过90% ({QueueSize}/{maxQueueSize})");
            }

            // 失败率告警
            var totalFlushes = SuccessFlushes + FailedFlushes;
            if (totalFlushes > 10)
            {
                var failureRate = (double)FailedFlushes / totalFlushes;
                if (failureRate > 0.1)
                {
                    alerts.Add($"刷新失败率过高 ({failureRate * 100:F2}%)");
                }
            }

            // 耗时告警
            if (AverageFlushDuration.TotalMilliseconds > 1000)
            {
                alerts.Add($"平均刷新耗时过长 ({AverageFlushDuration.TotalMilliseconds:F2}ms)");
            }

            // 丢弃告警
            if (DroppedLogs > TotalLogs * 0.01)
            {
                alerts.Add($"日志丢弃率过高 ({(double)DroppedLogs / TotalLogs * 100:F2}%)");
            }

            return alerts.Count > 0 ? string.Join("; ", alerts) : null;
        }
    }
}
