using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 缓存性能监控采集器
    /// </summary>
    public class CacheMetricsCollector : IPerformanceMetricsCollector
    {
        private readonly string _collectorName = "CacheMetrics";
        private readonly int _collectionIntervalSeconds = 10;
        
        private long _hitCount = 0;
        private long _missCount = 0;
        private long _evictionCount = 0;
        private long _insertCount = 0;
        
        private readonly ConcurrentQueue<double> _accessTimes = new ConcurrentQueue<double>();
        
        private readonly PerformanceMonitoringConfig _config;
        
        public CacheMetricsCollector(PerformanceMonitoringConfig config = null)
        {
            _config = config ?? new PerformanceMonitoringConfig();
        }
        
        public void RecordCacheHit(double accessTimeMs = 0)
        {
            Interlocked.Increment(ref _hitCount);
            if (accessTimeMs > 0)
            {
                _accessTimes.Enqueue(accessTimeMs);
                while (_accessTimes.Count > 1000)
                {
                    _accessTimes.TryDequeue(out _);
                }
            }
        }
        
        public void RecordCacheMiss()
        {
            Interlocked.Increment(ref _missCount);
        }
        
        public void RecordEviction()
        {
            Interlocked.Increment(ref _evictionCount);
        }
        
        public void RecordInsert()
        {
            Interlocked.Increment(ref _insertCount);
        }
        
        public Task<IEnumerable<PerformanceMetric>> CollectAsync(CancellationToken ct = default)
        {
            var metrics = new List<PerformanceMetric>();
            
            var total = Interlocked.Read(ref _hitCount) + Interlocked.Read(ref _missCount);
            var hitRate = total > 0 ? (double)Interlocked.Read(ref _hitCount) / total * 100 : 100;
            
            metrics.Add(new GaugeMetric
            {
                MetricName = "cache_hit_rate",
                Category = "Cache",
                MetricType = MetricType.Gauge,
                Timestamp = DateTime.Now,
                Value = hitRate,
                Unit = "%",
                Tags = new Dictionary<string, string> { { "metric", "hit_rate" } }
            });
            
            metrics.Add(new CounterMetric
            {
                MetricName = "cache_hit",
                Category = "Cache",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _hitCount),
                Tags = new Dictionary<string, string> { { "result", "hit" } }
            });
            
            metrics.Add(new CounterMetric
            {
                MetricName = "cache_miss",
                Category = "Cache",
                MetricType = MetricType.Counter,
                Timestamp = DateTime.Now,
                Value = Interlocked.Read(ref _missCount),
                Tags = new Dictionary<string, string> { { "result", "miss" } }
            });
            
            return Task.FromResult<IEnumerable<PerformanceMetric>>(metrics);
        }
        
        public IEnumerable<PerformanceMetricSnapshot> GetCurrentSnapshots()
        {
            var snapshots = new List<PerformanceMetricSnapshot>();
            
            var total = Interlocked.Read(ref _hitCount) + Interlocked.Read(ref _missCount);
            var hitRate = total > 0 ? (double)Interlocked.Read(ref _hitCount) / total * 100 : 100;
            
            snapshots.Add(new PerformanceMetricSnapshot
            {
                MetricName = "缓存命中率",
                Category = "缓存服务",
                MetricType = MetricType.Gauge,
                CurrentValue = hitRate,
                Unit = "%",
                Timestamp = DateTime.Now,
                Status = hitRate < _config.CacheHitRateWarningThresholdPercent ? 
                    MetricStatus.Warning : MetricStatus.Normal,
                WarningThreshold = _config.CacheHitRateWarningThresholdPercent
            });
            
            return snapshots;
        }
        
        public string CollectorName => _collectorName;
        public int CollectionIntervalSeconds => _collectionIntervalSeconds;
    }
}
