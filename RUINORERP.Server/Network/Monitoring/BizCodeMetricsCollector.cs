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
    /// 编号生成性能监控采集器
    /// 负责收集和聚合编号生成相关的性能指标
    /// </summary>
    public class BizCodeMetricsCollector : IPerformanceMetricsCollector
    {
        private readonly string _collectorName = "BizCodeMetrics";
        private readonly int _collectionIntervalSeconds = 10;
        
        // 响应时间记录（用于计算直方图）
        private readonly ConcurrentQueue<double> _responseTimes = new ConcurrentQueue<double>();
        
        // 计数器
        private long _successCount = 0;
        private long _failCount = 0;
        private long _timeoutCount = 0;
        private long _retryCount = 0;
        
        // 当前瞬时值
        private double _currentAvgResponseTime = 0;
        private int _currentRequestsPerSecond = 0;
        
        // 锁对象
        private readonly object _lockObject = new object();
        
        // 配置
        private readonly PerformanceMonitoringConfig _config;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public BizCodeMetricsCollector(PerformanceMonitoringConfig config = null)
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
        /// 记录编号生成请求
        /// </summary>
        /// <param name="responseTimeMs">响应时间（毫秒）</param>
        /// <param name="success">是否成功</param>
        /// <param name="isTimeout">是否超时</param>
        /// <param name="retryCount">重试次数</param>
        public void RecordGeneration(double responseTimeMs, bool success, bool isTimeout = false, int retryCount = 0)
        {
            _responseTimes.Enqueue(responseTimeMs);
            
            // 限制队列大小
            while (_responseTimes.Count > 10000)
            {
                _responseTimes.TryDequeue(out _);
            }
            
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
            
            if (retryCount > 0)
            {
                Interlocked.Add(ref _retryCount, retryCount);
            }
        }
        
        /// <summary>
        /// 计算当前指标
        /// </summary>
        private void CalculateCurrentMetrics()
        {
            lock (_lockObject)
            {
                // 计算最近 1 秒的请求数（简化实现）
                _currentRequestsPerSecond = (int)(_successCount + _failCount) / 
                    (int)(DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
                
                // 计算平均响应时间
                var times = _responseTimes.ToArray();
                if (times.Length > 0)
                {
                    _currentAvgResponseTime = times.Average();
                }
            }
        }
        
        /// <summary>
        /// 采集指标
        /// </summary>
        public Task<IEnumerable<PerformanceMetric>> CollectAsync(CancellationToken ct = default)
        {
            var metrics = new List<PerformanceMetric>();
            
            // 计算响应时间统计
            var responseTimes = _responseTimes.ToArray();
            
            if (responseTimes.Length > 0)
            {
                // 响应时间直方图
                metrics.Add(new HistogramMetric
                {
                    MetricName = "biz_code_generation_time",
                    Category = "BizCode",
                    MetricType = MetricType.Histogram,
                    Timestamp = DateTime.Now,
                    Count = responseTimes.Length,
                    Sum = responseTimes.Sum(),
                    Min = responseTimes.Min(),
                    Max = responseTimes.Max(),
                    Avg = responseTimes.Average(),
                    P50 = CalculatePercentile(responseTimes, 50),
                    P95 = CalculatePercentile(responseTimes, 95),
                    P99 = CalculatePercentile(responseTimes, 99),
                    Tags = new Dictionary<string, string> { { "operation", "generation" } }
                });
            }
            
            // 成功次数计数器
            metrics.Add(new CounterMetric
            {
                MetricName = "biz_code_generation_success",
                Category = "BizCode",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _successCount),
                Tags = new Dictionary<string, string> { { "result", "success" } }
            });
            
            // 失败次数计数器
            metrics.Add(new CounterMetric
            {
                MetricName = "biz_code_generation_failure",
                Category = "BizCode",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _failCount),
                Tags = new Dictionary<string, string> { { "result", "failure" } }
            });
            
            // 超时次数计数器
            metrics.Add(new CounterMetric
            {
                MetricName = "biz_code_generation_timeout",
                Category = "BizCode",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _timeoutCount),
                Tags = new Dictionary<string, string> { { "error_type", "timeout" } }
            });
            
            // 重试次数计数器
            metrics.Add(new CounterMetric
            {
                MetricName = "biz_code_generation_retry",
                Category = "BizCode",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _retryCount),
                Tags = new Dictionary<string, string> { { "operation", "retry" } }
            });
            
            // 当前平均响应时间仪表盘
            metrics.Add(new GaugeMetric
            {
                MetricName = "biz_code_current_avg_response_time",
                Category = "BizCode",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = _currentAvgResponseTime,
                Unit = "ms",
                Tags = new Dictionary<string, string> { { "metric", "avg_response_time" } }
            });
            
            // 成功率仪表盘
            var total = Interlocked.Read(ref _successCount) + Interlocked.Read(ref _failCount);
            var successRate = total > 0 ? (double)Interlocked.Read(ref _successCount) / total * 100 : 100;
            metrics.Add(new GaugeMetric
            {
                MetricName = "biz_code_success_rate",
                Category = "BizCode",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = successRate,
                Unit = "%",
                Tags = new Dictionary<string, string> { { "metric", "success_rate" } }
            });
            
            return Task.FromResult<IEnumerable<PerformanceMetric>>(metrics);
        }
        
        /// <summary>
        /// 获取当前指标快照
        /// </summary>
        public IEnumerable<PerformanceMetricSnapshot> GetCurrentSnapshots()
        {
            var snapshots = new List<PerformanceMetricSnapshot>();
            
            // 平均响应时间
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "平均响应时间",
                Category = "编号生成",
                MetricType = MetricType.Gauge,
                CurrentValue = _currentAvgResponseTime,
                Unit = "ms",
                Timestamp = DateTime.Now,
                Status = _currentAvgResponseTime > _config.BizCodeGenerationWarningThresholdMs ? 
                    MetricStatus.Warning : MetricStatus.Normal,
                WarningThreshold = _config.BizCodeGenerationWarningThresholdMs
            });
            
            // 成功率
            var total = Interlocked.Read(ref _successCount) + Interlocked.Read(ref _failCount);
            var successRate = total > 0 ? (double)Interlocked.Read(ref _successCount) / total * 100 : 100;
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "成功率",
                Category = "编号生成",
                MetricType = MetricType.Gauge,
                CurrentValue = successRate,
                Unit = "%",
                Timestamp = DateTime.Now,
                Status = successRate < 95 ? MetricStatus.Warning : 
                         successRate < 90 ? MetricStatus.Critical : MetricStatus.Normal
            });
            
            // 累计成功
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "累计成功",
                Category = "编号生成",
                MetricType = MetricType.Counter,
                CurrentValue = Interlocked.Read(ref _successCount),
                Unit = "次",
                Timestamp = DateTime.Now,
                Status = MetricStatus.Normal
            });
            
            // 累计失败
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "累计失败",
                Category = "编号生成",
                MetricType = MetricType.Counter,
                CurrentValue = Interlocked.Read(ref _failCount),
                Unit = "次",
                Timestamp = DateTime.Now,
                Status = Interlocked.Read(ref _failCount) > 0 ? MetricStatus.Warning : MetricStatus.Normal
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
            while (_responseTimes.TryDequeue(out _)) { }
            _successCount = 0;
            _failCount = 0;
            _timeoutCount = 0;
            _retryCount = 0;
            _currentAvgResponseTime = 0;
            _currentRequestsPerSecond = 0;
        }
        
        public string CollectorName => _collectorName;
        public int CollectionIntervalSeconds => _collectionIntervalSeconds;
    }
}
