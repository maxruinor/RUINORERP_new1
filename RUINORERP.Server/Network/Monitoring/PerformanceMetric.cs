using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 性能指标类型枚举
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// 计数器（单调递增）
        /// </summary>
        Counter,
        
        /// <summary>
        /// 仪表盘（可增可减的瞬时值）
        /// </summary>
        Gauge,
        
        /// <summary>
        /// 直方图（统计分布）
        /// </summary>
        Histogram
    }

    /// <summary>
    /// 性能指标基类
    /// </summary>
    public abstract class PerformanceMetric
    {
        /// <summary>
        /// 指标名称
        /// </summary>
        public string MetricName { get; set; }
        
        /// <summary>
        /// 指标分类
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// 指标类型
        /// </summary>
        public MetricType MetricType { get; set; }
        
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 标签（用于维度扩展）
        /// </summary>
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 计数器指标（单调递增）
    /// </summary>
    public class CounterMetric : PerformanceMetric
    {
        /// <summary>
        /// 计数值
        /// </summary>
        public long Value { get; set; }
        
        public CounterMetric()
        {
            MetricType = MetricType.Counter;
        }
    }

    /// <summary>
    /// 仪表盘指标（可增可减的瞬时值）
    /// </summary>
    public class GaugeMetric : PerformanceMetric
    {
        /// <summary>
        /// 指标值
        /// </summary>
        public double Value { get; set; }
        
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        
        public GaugeMetric()
        {
            MetricType = MetricType.Gauge;
        }
    }

    /// <summary>
    /// 直方图指标（统计分布）
    /// </summary>
    public class HistogramMetric : PerformanceMetric
    {
        /// <summary>
        /// 样本数量
        /// </summary>
        public long Count { get; set; }
        
        /// <summary>
        /// 总和
        /// </summary>
        public double Sum { get; set; }
        
        /// <summary>
        /// 最小值
        /// </summary>
        public double Min { get; set; }
        
        /// <summary>
        /// 最大值
        /// </summary>
        public double Max { get; set; }
        
        /// <summary>
        /// 平均值
        /// </summary>
        public double Avg { get; set; }
        
        /// <summary>
        /// 50 分位值（中位数）
        /// </summary>
        public double P50 { get; set; }
        
        /// <summary>
        /// 95 分位值
        /// </summary>
        public double P95 { get; set; }
        
        /// <summary>
        /// 99 分位值
        /// </summary>
        public double P99 { get; set; }
        
        public HistogramMetric()
        {
            MetricType = MetricType.Histogram;
        }
    }

    /// <summary>
    /// 性能指标快照（用于 UI 展示）
    /// </summary>
    public class PerformanceMetricSnapshot
    {
        /// <summary>
        /// 指标名称
        /// </summary>
        public string MetricName { get; set; }
        
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// 指标类型
        /// </summary>
        public MetricType MetricType { get; set; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public double CurrentValue { get; set; }
        
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 状态（Normal/Warning/Critical）
        /// </summary>
        public MetricStatus Status { get; set; }
        
        /// <summary>
        /// 告警阈值
        /// </summary>
        public double? WarningThreshold { get; set; }
        
        /// <summary>
        /// 严重告警阈值
        /// </summary>
        public double? CriticalThreshold { get; set; }
    }

    /// <summary>
    /// 指标状态枚举
    /// </summary>
    public enum MetricStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        
        /// <summary>
        /// 严重
        /// </summary>
        Critical
    }

    /// <summary>
    /// 性能指标采集器接口
    /// </summary>
    public interface IPerformanceMetricsCollector
    {
        /// <summary>
        /// 采集器名称
        /// </summary>
        string CollectorName { get; }
        
        /// <summary>
        /// 采集频率（秒）
        /// </summary>
        int CollectionIntervalSeconds { get; }
        
        /// <summary>
        /// 采集指标
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>性能指标集合</returns>
        Task<IEnumerable<PerformanceMetric>> CollectAsync(CancellationToken ct = default);
        
        /// <summary>
        /// 获取当前指标快照（用于 UI 实时展示）
        /// </summary>
        /// <returns>指标快照列表</returns>
        IEnumerable<PerformanceMetricSnapshot> GetCurrentSnapshots();
    }

    /// <summary>
    /// 性能监控配置
    /// </summary>
    public class PerformanceMonitoringConfig
    {
        /// <summary>
        /// 是否启用性能监控
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// 采集频率（秒）
        /// </summary>
        public int CollectionIntervalSeconds { get; set; } = 10;
        
        /// <summary>
        /// 历史数据保留天数
        /// </summary>
        public int DataRetentionDays { get; set; } = 30;
        
        /// <summary>
        /// 内存警告阈值（MB）
        /// </summary>
        public int MemoryWarningThresholdMB { get; set; } = 500;
        
        /// <summary>
        /// CPU 警告阈值（%）
        /// </summary>
        public int CpuWarningThresholdPercent { get; set; } = 70;
        
        /// <summary>
        /// 编号生成耗时警告阈值（ms）
        /// </summary>
        public int BizCodeGenerationWarningThresholdMs { get; set; } = 1000;
        
        /// <summary>
        /// 广播延迟警告阈值（ms）
        /// </summary>
        public int BroadcastLatencyWarningThresholdMs { get; set; } = 5000;
        
        /// <summary>
        /// 缓存命中率警告阈值（%）
        /// </summary>
        public int CacheHitRateWarningThresholdPercent { get; set; } = 80;
        
        /// <summary>
        /// 数据库查询耗时警告阈值（ms）
        /// </summary>
        public int DatabaseQueryWarningThresholdMs { get; set; } = 500;
    }
}
