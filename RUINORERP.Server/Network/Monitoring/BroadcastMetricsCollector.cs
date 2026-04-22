using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 广播服务性能监控采集器
    /// 负责收集和聚合广播服务相关的性能指标
    /// </summary>
    public class BroadcastMetricsCollector : IPerformanceMetricsCollector
    {
        private readonly string _collectorName = "BroadcastMetrics";
        private readonly int _collectionIntervalSeconds = 10;
        
        // 响应时间记录
        private readonly ConcurrentQueue<double> _latencies = new ConcurrentQueue<double>();
        
        // 计数器
        private long _totalBroadcastCount = 0;
        private long _successCount = 0;
        private long _failCount = 0;
        private long _timeoutCount = 0;
        
        // 并发统计
        private long _currentConcurrentCount = 0;
        private long _maxConcurrentCount = 0;
        private long _totalClientsBroadcasted = 0;
        
        // 瞬时值
        private double _currentAvgLatency = 0;
        private int _currentBroadcastsPerSecond = 0;
        
        // 配置
        private readonly PerformanceMonitoringConfig _config;
        
        // 锁对象
        private readonly object _lockObject = new object();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public BroadcastMetricsCollector(PerformanceMonitoringConfig config = null)
        {
            _config = config ?? new PerformanceMonitoringConfig();
            
            // 启动定时计算任务
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    CalculateCurrentMetrics();
                }
            });
        }
        
        /// <summary>
        /// 记录广播请求开始
        /// </summary>
        public void RecordBroadcastStart()
        {
            Interlocked.Increment(ref _currentConcurrentCount);
            
            // 更新最大并发数
            long current = Interlocked.Read(ref _currentConcurrentCount);
            while (current > _maxConcurrentCount)
            {
                long original = Interlocked.CompareExchange(ref _maxConcurrentCount, current, _maxConcurrentCount);
                if (original >= current) break;
                current = Interlocked.Read(ref _currentConcurrentCount);
            }
        }
        
        /// <summary>
        /// 记录广播请求完成
        /// </summary>
        /// <param name="latencyMs">延迟（毫秒）</param>
        /// <param name="success">是否成功</param>
        /// <param name="clientCount">客户端数量</param>
        /// <param name="isTimeout">是否超时</param>
        public void RecordBroadcastComplete(double latencyMs, bool success, int clientCount, bool isTimeout = false)
        {
            _latencies.Enqueue(latencyMs);
            
            // 限制队列大小
            while (_latencies.Count > 10000)
            {
                _latencies.TryDequeue(out _);
            }
            
            Interlocked.Decrement(ref _currentConcurrentCount);
            Interlocked.Increment(ref _totalBroadcastCount);
            Interlocked.Add(ref _totalClientsBroadcasted, clientCount);
            
            if (success)
            {
                Interlocked.Increment(ref _successCount);
            }
            else
            {
                Interlocked.Increment(ref _failCount);
            }
            
            if (isTimeout)
            {
                Interlocked.Increment(ref _timeoutCount);
            }
        }
        
        /// <summary>
        /// 计算当前指标
        /// </summary>
        private void CalculateCurrentMetrics()
        {
            lock (_lockObject)
            {
                // 计算每秒广播数
                var elapsedSeconds = (int)(DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
                _currentBroadcastsPerSecond = elapsedSeconds > 0 ? (int)(_totalBroadcastCount / elapsedSeconds) : 0;
                
                // 计算平均延迟
                var latencies = _latencies.ToArray();
                if (latencies.Length > 0)
                {
                    _currentAvgLatency = latencies.Average();
                }
            }
        }
        
        /// <summary>
        /// 采集指标
        /// </summary>
        public Task<IEnumerable<PerformanceMetric>> CollectAsync(CancellationToken ct = default)
        {
            var metrics = new List<PerformanceMetric>();
            
            // 延迟直方图
            var latencies = _latencies.ToArray();
            if (latencies.Length > 0)
            {
                metrics.Add(new HistogramMetric
                {
                    MetricName = "broadcast_latency",
                    Category = "Broadcast",
                    MetricType = MetricType.Histogram,
                    Timestamp = DateTime.Now,
                    Count = latencies.Length,
                    Sum = latencies.Sum(),
                    Min = latencies.Min(),
                    Max = latencies.Max(),
                    Avg = latencies.Average(),
                    P50 = CalculatePercentile(latencies, 50),
                    P95 = CalculatePercentile(latencies, 95),
                    P99 = CalculatePercentile(latencies, 99),
                    Tags = new Dictionary<string, string> { { "operation", "broadcast" } }
                });
            }
            
            // 总广播次数
            metrics.Add(new CounterMetric
            {
                MetricName = "broadcast_total",
                Category = "Broadcast",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _totalBroadcastCount),
                Tags = new Dictionary<string, string> { { "operation", "broadcast" } }
            });
            
            // 成功次数
            metrics.Add(new CounterMetric
            {
                MetricName = "broadcast_success",
                Category = "Broadcast",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _successCount),
                Tags = new Dictionary<string, string> { { "result", "success" } }
            });
            
            // 失败次数
            metrics.Add(new CounterMetric
            {
                MetricName = "broadcast_failure",
                Category = "Broadcast",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _failCount),
                Tags = new Dictionary<string, string> { { "result", "failure" } }
            });
            
            // 超时次数
            metrics.Add(new CounterMetric
            {
                MetricName = "broadcast_timeout",
                Category = "Broadcast",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _timeoutCount),
                Tags = new Dictionary<string, string> { { "error_type", "timeout" } }
            });
            
            // 当前并发数
            metrics.Add(new GaugeMetric
            {
                MetricName = "broadcast_current_concurrent",
                Category = "Broadcast",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _currentConcurrentCount),
                Unit = "个",
                Tags = new Dictionary<string, string> { { "metric", "concurrency" } }
            });
            
            // 最大并发数
            metrics.Add(new GaugeMetric
            {
                MetricName = "broadcast_max_concurrent",
                Category = "Broadcast",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _maxConcurrentCount),
                Unit = "个",
                Tags = new Dictionary<string, string> { { "metric", "max_concurrency" } }
            });
            
            // 平均延迟
            metrics.Add(new GaugeMetric
            {
                MetricName = "broadcast_current_avg_latency",
                Category = "Broadcast",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = _currentAvgLatency,
                Unit = "ms",
                Tags = new Dictionary<string, string> { { "metric", "avg_latency" } }
            });
            
            // 成功率
            var total = Interlocked.Read(ref _successCount) + Interlocked.Read(ref _failCount);
            var successRate = total > 0 ? (double)Interlocked.Read(ref _successCount) / total * 100 : 100;
            metrics.Add(new GaugeMetric
            {
                MetricName = "broadcast_success_rate",
                Category = "Broadcast",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = successRate,
                Unit = "%",
                Tags = new Dictionary<string, string> { { "metric", "success_rate" } }
            });
            
            // 平均每客户端延迟
            var totalClients = Interlocked.Read(ref _totalClientsBroadcasted);
            var totalBroadcasts = Interlocked.Read(ref _totalBroadcastCount);
            if (totalBroadcasts > 0)
            {
                metrics.Add(new GaugeMetric
                {
                    MetricName = "broadcast_avg_clients_per_broadcast",
                    Category = "Broadcast",
                    MetricType = MetricType.Gauge,
                    Timestamp = DateTime.Now,
                    Value = (double)totalClients / totalBroadcasts,
                    Unit = "个",
                    Tags = new Dictionary<string, string> { { "metric", "avg_clients" } }
                });
            }
            
            return Task.FromResult<IEnumerable<PerformanceMetric>>(metrics);
        }
        
        /// <summary>
        /// 获取当前指标快照
        /// </summary>
        public IEnumerable<PerformanceMetricSnapshot> GetCurrentSnapshots()
        {
            var snapshots = new List<PerformanceMetricSnapshot>();
            
            // 平均延迟
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "平均延迟",
                Category = "广播服务",
                MetricType = MetricType.Gauge,
                CurrentValue = _currentAvgLatency,
                Unit = "ms",
                Timestamp = DateTime.Now,
                Status = _currentAvgLatency > _config.BroadcastLatencyWarningThresholdMs ? 
                    MetricStatus.Warning : MetricStatus.Normal,
                WarningThreshold = _config.BroadcastLatencyWarningThresholdMs
            });
            
            // 成功率
            var total = Interlocked.Read(ref _successCount) + Interlocked.Read(ref _failCount);
            var successRate = total > 0 ? (double)Interlocked.Read(ref _successCount) / total * 100 : 100;
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "成功率",
                Category = "广播服务",
                MetricType = MetricType.Gauge,
                CurrentValue = successRate,
                Unit = "%",
                Timestamp = DateTime.Now,
                Status = successRate < 95 ? MetricStatus.Warning : 
                         successRate < 90 ? MetricStatus.Critical : MetricStatus.Normal
            });
            
            // 当前并发
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "当前并发数",
                Category = "广播服务",
                MetricType = MetricType.Gauge,
                CurrentValue = Interlocked.Read(ref _currentConcurrentCount),
                Unit = "个",
                Timestamp = DateTime.Now,
                Status = MetricStatus.Normal
            });
            
            // 累计广播
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "累计广播次数",
                Category = "广播服务",
                MetricType = MetricType.Counter,
                CurrentValue = Interlocked.Read(ref _totalBroadcastCount),
                Unit = "次",
                Timestamp = DateTime.Now,
                Status = MetricStatus.Normal
            });
            
            return snapshots;
        }
        
        /// <summary>
        /// 计算百分位数
        /// </summary>
        private double CalculatePercentile(double[] values, double percentile)
        {
            if (values.Length == 0) return 0;
            
            var sorted = values.OrderBy(v => v).ToArray();
            var index = (int)Math.Ceiling(percentile / 100.0 * sorted.Length) - 1;
            return sorted[Math.Max(0, index)];
        }
        
        /// <summary>
        /// 重置统计数据
        /// </summary>
        public void ResetStatistics()
        {
            while (_latencies.TryDequeue(out _)) { }
            _totalBroadcastCount = 0;
            _successCount = 0;
            _failCount = 0;
            _timeoutCount = 0;
            _currentConcurrentCount = 0;
            _maxConcurrentCount = 0;
            _totalClientsBroadcasted = 0;
            _currentAvgLatency = 0;
            _currentBroadcastsPerSecond = 0;
        }
        
        public string CollectorName => _collectorName;
        public int CollectionIntervalSeconds => _collectionIntervalSeconds;
    }
}
