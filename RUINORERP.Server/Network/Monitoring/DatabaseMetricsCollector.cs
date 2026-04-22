using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 数据库性能监控采集器
    /// </summary>
    public class DatabaseMetricsCollector : IPerformanceMetricsCollector
    {
        private readonly string _collectorName = "DatabaseMetrics";
        private readonly int _collectionIntervalSeconds = 10;
        
        private long _queryCount = 0;
        private long _slowQueryCount = 0;
        private long _retryCount = 0;
        private long _errorCount = 0;
        
        private readonly ConcurrentQueue<double> _queryTimes = new ConcurrentQueue<double>();
        
        private readonly PerformanceMonitoringConfig _config;
        
        public DatabaseMetricsCollector(PerformanceMonitoringConfig config = null)
        {
            _config = config ?? new PerformanceMonitoringConfig();
        }
        
        public void RecordQuery(double queryTimeMs, bool isSlow = false)
        {
            Interlocked.Increment(ref _queryCount);
            _queryTimes.Enqueue(queryTimeMs);
            
            while (_queryTimes.Count > 1000)
            {
                _queryTimes.TryDequeue(out _);
            }
            
            if (isSlow)
            {
                Interlocked.Increment(ref _slowQueryCount);
            }
        }
        
        public void RecordRetry()
        {
            Interlocked.Increment(ref _retryCount);
        }
        
        public void RecordError()
        {
            Interlocked.Increment(ref _errorCount);
        }
        
        public Task<IEnumerable<PerformanceMetric>> CollectAsync(CancellationToken ct = default)
        {
            var metrics = new List<PerformanceMetric>();
            
            var queryTimes = _queryTimes.ToArray();
            if (queryTimes.Length > 0)
            {
                metrics.Add(new HistogramMetric
                {
                    MetricName = "database_query_time",
                    Category = "Database",
                    MetricType = MetricType.Histogram,
                    Timestamp = DateTime.Now,
                    Count = queryTimes.Length,
                    Sum = queryTimes.Sum(),
                    Min = queryTimes.Min(),
                    Max = queryTimes.Max(),
                    Avg = queryTimes.Average(),
                    P50 = CalculatePercentile(queryTimes, 50),
                    P95 = CalculatePercentile(queryTimes, 95),
                    P99 = CalculatePercentile(queryTimes, 99),
                    Tags = new Dictionary<string, string> { { "operation", "query" } }
                });
            }
            
            metrics.Add(new CounterMetric
            {
                MetricName = "database_query_total",
                Category = "Database",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _queryCount),
                Tags = new Dictionary<string, string> { { "operation", "query" } }
            });
            
            metrics.Add(new CounterMetric
            {
                MetricName = "database_slow_query",
                Category = "Database",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _slowQueryCount),
                Tags = new Dictionary<string, string> { { "type", "slow" } }
            });
            
            return Task.FromResult<IEnumerable<PerformanceMetric>>(metrics);
        }
        
        public IEnumerable<PerformanceMetricSnapshot> GetCurrentSnapshots()
        {
            var snapshots = new List<PerformanceMetricSnapshot>();
            
            var queryTimes = _queryTimes.ToArray();
            var avgQueryTime = queryTimes.Length > 0 ? queryTimes.Average() : 0;
            
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "平均查询耗时",
                Category = "数据库服务",
                MetricType = MetricType.Gauge,
                CurrentValue = avgQueryTime,
                Unit = "ms",
                Timestamp = DateTime.Now,
                Status = avgQueryTime > _config.DatabaseQueryWarningThresholdMs ? 
                    MetricStatus.Warning : MetricStatus.Normal,
                WarningThreshold = _config.DatabaseQueryWarningThresholdMs
            });
            
            return snapshots;
        }
        
        private double CalculatePercentile(double[] values, double percentile)
        {
            if (values.Length == 0) return 0;
            var sorted = values.OrderBy(v => v).ToArray();
            var index = (int)Math.Ceiling(percentile / 100.0 * sorted.Length) - 1;
            return sorted[Math.Max(0, index)];
        }
        
        public string CollectorName => _collectorName;
        public int CollectionIntervalSeconds => _collectionIntervalSeconds;
    }
}
