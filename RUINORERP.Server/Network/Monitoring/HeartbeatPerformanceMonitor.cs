using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 心跳性能监控器
    /// 用于监控心跳处理性能并动态调整优化策略
    /// </summary>
    public class HeartbeatPerformanceMonitor : IDisposable
    {
        private readonly ILogger<HeartbeatPerformanceMonitor> _logger;
        private readonly Timer _monitoringTimer;
        
        // 性能统计数据
        private readonly ConcurrentQueue<long> _processingTimes;
        private readonly ConcurrentQueue<int> _queueLengths;
        private long _totalProcessed = 0;
        private long _totalErrors = 0;
        
        // 动态配置
        private volatile bool _useBatchProcessing = false;
        private volatile int _mergeThresholdSeconds = 5;
        private volatile int _batchSize = 10;
        
        private bool _disposed = false;
        private readonly object _lockObject = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatPerformanceMonitor(ILogger<HeartbeatPerformanceMonitor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _processingTimes = new ConcurrentQueue<long>();
            _queueLengths = new ConcurrentQueue<int>();
            
            // 每30秒收集一次性能数据
            _monitoringTimer = new Timer(CollectPerformanceData, null, 
                                       TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
            
            _logger.LogInformation("心跳性能监控器初始化完成");
        }

        /// <summary>
        /// 记录心跳处理时间
        /// </summary>
        public void RecordProcessingTime(long milliseconds)
        {
            if (_disposed)
                return;

            _processingTimes.Enqueue(milliseconds);
            Interlocked.Increment(ref _totalProcessed);
            
            // 保持队列大小限制
            while (_processingTimes.Count > 1000)
            {
                _processingTimes.TryDequeue(out _);
            }
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        public void RecordError()
        {
            if (_disposed)
                return;

            Interlocked.Increment(ref _totalErrors);
        }

        /// <summary>
        /// 记录队列长度
        /// </summary>
        public void RecordQueueLength(int length)
        {
            if (_disposed)
                return;

            _queueLengths.Enqueue(length);
            
            // 保持队列大小限制
            while (_queueLengths.Count > 1000)
            {
                _queueLengths.TryDequeue(out _);
            }
        }

        /// <summary>
        /// 获取性能统计信息
        /// </summary>
        public HeartbeatPerformanceStats GetPerformanceStats()
        {
            var stats = new HeartbeatPerformanceStats
            {
                TotalProcessed = _totalProcessed,
                TotalErrors = _totalErrors,
                ErrorRate = _totalProcessed > 0 ? (double)_totalErrors / _totalProcessed * 100 : 0,
                AverageProcessingTime = CalculateAverage(_processingTimes),
                CurrentQueueLength = _queueLengths.IsEmpty ? 0 : GetLatestValue(_queueLengths),
                UseBatchProcessing = _useBatchProcessing,
                MergeThresholdSeconds = _mergeThresholdSeconds,
                BatchSize = _batchSize
            };

            return stats;
        }

        /// <summary>
        /// 收集性能数据并调整策略
        /// </summary>
        private void CollectPerformanceData(object state)
        {
            if (_disposed)
                return;

            try
            {
                var stats = GetPerformanceStats();
                
                // 根据性能数据动态调整策略
                AdjustOptimizationStrategy(stats);
                
                _logger.LogInformation($"心跳性能统计 - 处理总数: {stats.TotalProcessed}, " +
                                     $"错误率: {stats.ErrorRate:F2}%, " +
                                     $"平均处理时间: {stats.AverageProcessingTime:F2}ms, " +
                                     $"当前队列长度: {stats.CurrentQueueLength}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "收集心跳性能数据时发生异常");
            }
        }

        /// <summary>
        /// 根据性能数据调整优化策略
        /// </summary>
        private void AdjustOptimizationStrategy(HeartbeatPerformanceStats stats)
        {
            lock (_lockObject)
            {
                // 根据处理时间和队列长度调整批量处理策略
                if (stats.AverageProcessingTime > 50 && stats.CurrentQueueLength > 10)
                {
                    // 高负载时启用批量处理
                    _useBatchProcessing = true;
                    _batchSize = Math.Min(20, _batchSize + 2);
                    _logger.LogInformation($"启用批量处理，批次大小: {_batchSize}");
                }
                else if (stats.AverageProcessingTime < 20 && stats.CurrentQueueLength < 5)
                {
                    // 低负载时禁用批量处理
                    _useBatchProcessing = false;
                    _batchSize = Math.Max(5, _batchSize - 1);
                    _logger.LogInformation($"禁用批量处理，批次大小: {_batchSize}");
                }

                // 根据错误率调整合并阈值
                if (stats.ErrorRate > 5)
                {
                    // 错误率高时减少合并阈值
                    _mergeThresholdSeconds = Math.Max(2, _mergeThresholdSeconds - 1);
                }
                else if (stats.ErrorRate < 1)
                {
                    // 错误率低时增加合并阈值
                    _mergeThresholdSeconds = Math.Min(10, _mergeThresholdSeconds + 1);
                }
            }
        }

        /// <summary>
        /// 计算平均值
        /// </summary>
        private double CalculateAverage(ConcurrentQueue<long> queue)
        {
            if (queue.IsEmpty)
                return 0;

            long sum = 0;
            int count = 0;
            
            foreach (var value in queue)
            {
                sum += value;
                count++;
            }
            
            return count > 0 ? (double)sum / count : 0;
        }

        /// <summary>
        /// 获取最新值
        /// </summary>
        private int GetLatestValue(ConcurrentQueue<int> queue)
        {
            var values = queue.ToArray();
            return values.Length > 0 ? values[values.Length - 1] : 0;
        }

        /// <summary>
        /// 获取是否应该使用批量处理
        /// </summary>
        public bool ShouldUseBatchProcessing() => _useBatchProcessing;

        /// <summary>
        /// 获取合并阈值（秒）
        /// </summary>
        public int GetMergeThresholdSeconds() => _mergeThresholdSeconds;

        /// <summary>
        /// 获取批量大小
        /// </summary>
        public int GetBatchSize() => _batchSize;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                _monitoringTimer?.Dispose();
                _logger.LogInformation("心跳性能监控器已释放");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放心跳性能监控器时发生异常");
            }
        }
    }

    /// <summary>
    /// 心跳性能统计信息
    /// </summary>
    public class HeartbeatPerformanceStats
    {
        /// <summary>
        /// 总处理数
        /// </summary>
        public long TotalProcessed { get; set; }

        /// <summary>
        /// 总错误数
        /// </summary>
        public long TotalErrors { get; set; }

        /// <summary>
        /// 错误率（百分比）
        /// </summary>
        public double ErrorRate { get; set; }

        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime { get; set; }

        /// <summary>
        /// 当前队列长度
        /// </summary>
        public int CurrentQueueLength { get; set; }

        /// <summary>
        /// 是否使用批量处理
        /// </summary>
        public bool UseBatchProcessing { get; set; }

        /// <summary>
        /// 合并阈值（秒）
        /// </summary>
        public int MergeThresholdSeconds { get; set; }

        /// <summary>
        /// 批量大小
        /// </summary>
        public int BatchSize { get; set; }
    }
}