using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.UI.Network.RetryStrategy;

namespace RUINORERP.UI.Network.TimeoutStatistics
{
    /// <summary>
    /// 超时统计管理器
    /// 负责记录、统计和分析网络请求的超时情况
    /// 提供实时监控和历史趋势分析功能
    /// </summary>
    public class TimeoutStatisticsManager
    {
        // 存储特定命令类型的超时统计信息
        private readonly ConcurrentDictionary<string, CommandTimeoutStats> _commandTimeoutStats;

        // 存储超时趋势数据的队列
        private readonly ConcurrentQueue<TimeoutTrendData> _timeoutTrendData;

        // 最大趋势数据点数量
        private const int MaxTrendDataPoints = 1000;

        /// <summary>
        /// 总超时次数
        /// </summary>
        public int TotalTimeoutCount { get; private set; }

        /// <summary>
        /// 总请求次数
        /// </summary>
        public int TotalRequestCount { get; private set; }

        /// <summary>
        /// 构造函数
        /// 初始化超时统计管理器
        /// </summary>
        public TimeoutStatisticsManager()
        {
            _commandTimeoutStats = new ConcurrentDictionary<string, CommandTimeoutStats>();
            _timeoutTrendData = new ConcurrentQueue<TimeoutTrendData>();
            TotalTimeoutCount = 0;
            TotalRequestCount = 0;
        }

        /// <summary>
        /// 记录请求超时信息
        /// </summary>
        /// <param name="commandId">命令类型ID</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        public void RecordTimeout(string commandId, long timeoutMs)
        {
            if (string.IsNullOrEmpty(commandId))
                throw new ArgumentNullException(nameof(commandId));

            // 更新全局统计
            TotalRequestCount++;
            TotalTimeoutCount++;

            // 获取或创建命令特定的统计信息
            var stats = _commandTimeoutStats.GetOrAdd(commandId, id => new CommandTimeoutStats
            {
                CommandId = id,
                TimeoutCount = 0,
                TotalRequestCount = 0,
                SuccessRequestCount = 0,
                AverageProcessingTimeMs = 0,
                MaxProcessingTimeMs = 0,
                MinProcessingTimeMs = long.MaxValue,
                LastRequestTime = DateTime.Now
            });

            // 更新命令特定的统计信息
            lock (stats)
            {
                stats.TimeoutCount++;
                stats.TotalRequestCount++;
                stats.LastRequestTime = DateTime.Now;

                // 计算平均处理时间（使用超时时间作为处理时间）
                if (stats.TotalRequestCount > 0)
                {
                    stats.AverageProcessingTimeMs = ((stats.AverageProcessingTimeMs * (stats.TotalRequestCount - 1)) + timeoutMs) / stats.TotalRequestCount;
                }
                else
                {
                    stats.AverageProcessingTimeMs = timeoutMs;
                }

                // 更新最大处理时间
                if (timeoutMs > stats.MaxProcessingTimeMs)
                {
                    stats.MaxProcessingTimeMs = timeoutMs;
                }

                // 更新最小处理时间
                if (timeoutMs < stats.MinProcessingTimeMs)
                {
                    stats.MinProcessingTimeMs = timeoutMs;
                }
            }

            // 添加趋势数据
            AddTrendData(commandId, true, timeoutMs);
        }

        /// <summary>
        /// 记录请求成功信息
        /// </summary>
        /// <param name="commandId">命令类型ID</param>
        /// <param name="processingTimeMs">处理时间（毫秒）</param>
        public void RecordSuccess(string commandId, long processingTimeMs)
        {
            if (string.IsNullOrEmpty(commandId))
                throw new ArgumentNullException(nameof(commandId));

            // 更新全局统计
            TotalRequestCount++;

            // 获取或创建命令特定的统计信息
            var stats = _commandTimeoutStats.GetOrAdd(commandId, id => new CommandTimeoutStats
            {
                CommandId = id,
                TimeoutCount = 0,
                TotalRequestCount = 0,
                SuccessRequestCount = 0,
                AverageProcessingTimeMs = 0,
                MaxProcessingTimeMs = 0,
                MinProcessingTimeMs = long.MaxValue,
                LastRequestTime = DateTime.Now
            });

            // 更新命令特定的统计信息
            lock (stats)
            {
                stats.SuccessRequestCount++;
                stats.TotalRequestCount++;
                stats.LastRequestTime = DateTime.Now;

                // 计算平均处理时间
                if (stats.TotalRequestCount > 0)
                {
                    stats.AverageProcessingTimeMs = ((stats.AverageProcessingTimeMs * (stats.TotalRequestCount - 1)) + processingTimeMs) / stats.TotalRequestCount;
                }
                else
                {
                    stats.AverageProcessingTimeMs = processingTimeMs;
                }

                // 更新最大处理时间
                if (processingTimeMs > stats.MaxProcessingTimeMs)
                {
                    stats.MaxProcessingTimeMs = processingTimeMs;
                }

                // 更新最小处理时间
                if (processingTimeMs < stats.MinProcessingTimeMs)
                {
                    stats.MinProcessingTimeMs = processingTimeMs;
                }
            }

            // 添加趋势数据
            AddTrendData(commandId, false, processingTimeMs);
        }

        /// <summary>
        /// 添加趋势数据
        /// </summary>
        /// <param name="commandId">命令类型ID</param>
        /// <param name="isTimeout">是否为超时记录</param>
        /// <param name="processingTimeMs">处理时间（毫秒）</param>
        private void AddTrendData(string commandId, bool isTimeout, long processingTimeMs)
        {
            var trendData = new TimeoutTrendData(DateTime.Now, commandId, isTimeout, processingTimeMs);
            _timeoutTrendData.Enqueue(trendData);

            // 限制趋势数据点数量，移除旧数据
            while (_timeoutTrendData.Count > MaxTrendDataPoints)
            {
                _timeoutTrendData.TryDequeue(out _);
            }
        }

        /// <summary>
        /// 获取特定命令类型的超时统计信息
        /// </summary>
        /// <param name="commandId">命令类型ID</param>
        /// <returns>命令超时统计信息，如果不存在则返回null</returns>
        public CommandTimeoutStats GetCommandTimeoutStats(string commandId)
        {
            if (string.IsNullOrEmpty(commandId))
                throw new ArgumentNullException(nameof(commandId));

            _commandTimeoutStats.TryGetValue(commandId, out var stats);
            return stats;
        }

        /// <summary>
        /// 获取所有命令类型的超时统计信息
        /// </summary>
        /// <returns>命令超时统计信息列表</returns>
        public IEnumerable<CommandTimeoutStats> GetAllCommandTimeoutStats()
        {
            return _commandTimeoutStats.Values.ToList();
        }

        /// <summary>
        /// 获取指定时间段内的超时趋势数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>超时趋势数据列表</returns>
        public IEnumerable<TimeoutTrendData> GetTrendData(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
                throw new ArgumentException("开始时间不能晚于结束时间");

            return _timeoutTrendData
                .Where(data => data.Timestamp >= startTime && data.Timestamp <= endTime)
                .ToList();
        }

        /// <summary>
        /// 获取总体超时率
        /// </summary>
        /// <returns>超时率百分比</returns>
        public double GetOverallTimeoutRate()
        {
            if (TotalRequestCount == 0)
                return 0;
            return (double)TotalTimeoutCount / TotalRequestCount * 100;
        }

        /// <summary>
        /// 重置所有统计数据
        /// </summary>
        public void Reset()
        {
            TotalTimeoutCount = 0;
            TotalRequestCount = 0;
            _commandTimeoutStats.Clear();
            //_timeoutTrendData.Clear();
            #if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                _timeoutTrendData.Clear();
            #else
                        while (_timeoutTrendData.TryDequeue(out _)) { }
            #endif
        }
    }
}