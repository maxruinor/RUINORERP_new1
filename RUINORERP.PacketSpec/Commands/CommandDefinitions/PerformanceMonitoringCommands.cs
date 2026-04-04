using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands.CommandDefinitions
{
    /// <summary>
    /// 性能监控数据上报请求
    /// 客户端向服务器上报性能监控数据
    /// </summary>
    public class PerformanceDataUploadRequest : RequestBase
    {
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 性能数据包（JSON序列化）
        /// </summary>
        public string PerformanceDataJson { get; set; }

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        public int PacketSizeBytes { get; set; }

        /// <summary>
        /// 指标数量
        /// </summary>
        public int MetricCount { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime UploadTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 客户端机器名
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIpAddress { get; set; }
    }

    /// <summary>
    /// 性能监控数据上报响应
    /// </summary>
    public class PerformanceDataUploadResponse : ResponseBase
    {
        /// <summary>
        /// 服务器接收时间
        /// </summary>
        public DateTime ServerReceiveTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 已处理的指标数量
        /// </summary>
        public int ProcessedMetricCount { get; set; }

        /// <summary>
        /// 服务器当前存储的该客户端指标总数
        /// </summary>
        public long TotalStoredMetrics { get; set; }
    }

    /// <summary>
    /// 性能监控数据查询请求
    /// 管理员查询性能监控数据
    /// </summary>
    public class PerformanceDataQueryRequest : RequestBase
    {
        /// <summary>
        /// 查询的客户端ID（为空则查询所有客户端）
        /// </summary>
        public string TargetClientId { get; set; }

        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 指标类型筛选
        /// </summary>
        public List<int> MetricTypes { get; set; } = new List<int>();

        /// <summary>
        /// 最大返回记录数
        /// </summary>
        public int MaxRecords { get; set; } = 1000;

        /// <summary>
        /// 是否包含统计摘要
        /// </summary>
        public bool IncludeSummary { get; set; } = true;
    }

    /// <summary>
    /// 性能监控数据查询响应
    /// </summary>
    public class PerformanceDataQueryResponse : ResponseBase
    {
        /// <summary>
        /// 查询到的性能数据列表（JSON序列化）
        /// </summary>
        public List<string> MetricsJson { get; set; } = new List<string>();

        /// <summary>
        /// 统计摘要（JSON序列化）
        /// </summary>
        public string SummaryJson { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 查询执行时间（毫秒）
        /// </summary>
        public long QueryExecutionTimeMs { get; set; }
    }

    /// <summary>
    /// 性能监控开关控制请求
    /// 管理员控制客户端性能监控开关
    /// </summary>
    public class PerformanceMonitorControlRequest : RequestBase
    {
        /// <summary>
        /// 目标客户端ID（为空则广播给所有客户端）
        /// </summary>
        public string TargetClientId { get; set; }

        /// <summary>
        /// 是否启用性能监控
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 启用的监控项类型列表
        /// </summary>
        public List<int> EnabledMonitorTypes { get; set; } = new List<int>();

        /// <summary>
        /// 配置参数（JSON格式）
        /// </summary>
        public string ConfigJson { get; set; }
    }

    /// <summary>
    /// 性能监控开关控制响应
    /// </summary>
    public class PerformanceMonitorControlResponse : ResponseBase
    {
        /// <summary>
        /// 受影响的客户端数量
        /// </summary>
        public int AffectedClientCount { get; set; }

        /// <summary>
        /// 当前服务器端性能监控状态
        /// </summary>
        public bool ServerMonitorEnabled { get; set; }
    }

    /// <summary>
    /// 性能监控实时数据推送（服务器主动推送）
    /// </summary>
    public class PerformanceRealTimeDataPush : RequestBase
    {
        /// <summary>
        /// 数据来源客户端ID
        /// </summary>
        public string SourceClientId { get; set; }

        /// <summary>
        /// 实时性能数据（JSON序列化）
        /// </summary>
        public string RealTimeDataJson { get; set; }

        /// <summary>
        /// 数据推送时间
        /// </summary>
        public DateTime PushTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否紧急数据（如死锁、内存临界等）
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// 紧急级别（1-5，5为最高）
        /// </summary>
        public int UrgencyLevel { get; set; }
    }

    /// <summary>
    /// 性能监控统计摘要请求
    /// </summary>
    public class PerformanceStatisticsRequest : RequestBase
    {
        /// <summary>
        /// 查询的客户端ID
        /// </summary>
        public string TargetClientId { get; set; }

        /// <summary>
        /// 统计时间范围（小时）
        /// </summary>
        public int TimeRangeHours { get; set; } = 1;

        /// <summary>
        /// 包含的统计类型
        /// </summary>
        public List<int> IncludeMetricTypes { get; set; } = new List<int>();
    }

    /// <summary>
    /// 性能监控统计摘要响应
    /// </summary>
    public class PerformanceStatisticsResponse : ResponseBase
    {
        /// <summary>
        /// 统计摘要数据（JSON序列化）
        /// </summary>
        public string StatisticsJson { get; set; }

        /// <summary>
        /// 统计生成时间
        /// </summary>
        public DateTime StatisticsGeneratedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 涉及的时间段
        /// </summary>
        public TimeSpan TimeSpan { get; set; }
    }

    /// <summary>
    /// 性能监控告警通知（服务器主动推送）
    /// </summary>
    public class PerformanceAlertNotification : RequestBase
    {
        /// <summary>
        /// 告警ID
        /// </summary>
        public string AlertId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 告警来源客户端ID
        /// </summary>
        public string SourceClientId { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        public PerformanceAlertType AlertType { get; set; }

        /// <summary>
        /// 告警级别
        /// </summary>
        public PerformanceAlertLevel AlertLevel { get; set; }

        /// <summary>
        /// 告警标题
        /// </summary>
        public string AlertTitle { get; set; }

        /// <summary>
        /// 告警详情
        /// </summary>
        public string AlertDetails { get; set; }

        /// <summary>
        /// 相关指标数据（JSON）
        /// </summary>
        public string RelatedMetricsJson { get; set; }

        /// <summary>
        /// 告警发生时间
        /// </summary>
        public DateTime AlertTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 建议操作
        /// </summary>
        public string SuggestedAction { get; set; }
    }

    /// <summary>
    /// 性能监控告警类型
    /// </summary>
    public enum PerformanceAlertType
    {
        /// <summary>
        /// 方法执行超时
        /// </summary>
        MethodExecutionTimeout = 1,

        /// <summary>
        /// 数据库查询缓慢
        /// </summary>
        SlowDatabaseQuery = 2,

        /// <summary>
        /// 死锁检测
        /// </summary>
        DeadlockDetected = 3,

        /// <summary>
        /// 内存使用过高
        /// </summary>
        HighMemoryUsage = 4,

        /// <summary>
        /// 网络请求超时
        /// </summary>
        NetworkRequestTimeout = 5,

        /// <summary>
        /// 事务执行时间过长
        /// </summary>
        LongRunningTransaction = 6,

        /// <summary>
        /// 缓存命中率过低
        /// </summary>
        LowCacheHitRate = 7,

        /// <summary>
        /// UI响应卡顿
        /// </summary>
        UIStuttering = 8,

        /// <summary>
        /// 错误率过高
        /// </summary>
        HighErrorRate = 9
    }

    /// <summary>
    /// 性能监控告警级别
    /// </summary>
    public enum PerformanceAlertLevel
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Warning = 2,

        /// <summary>
        /// 严重
        /// </summary>
        Critical = 3,

        /// <summary>
        /// 紧急
        /// </summary>
        Emergency = 4
    }

    /// <summary>
    /// 客户端性能监控状态上报请求
    /// </summary>
    public class PerformanceMonitorStatusRequest : RequestBase
    {
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 性能监控是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 当前启用的监控项
        /// </summary>
        public List<int> EnabledMonitorTypes { get; set; } = new List<int>();

        /// <summary>
        /// 缓冲区指标数量
        /// </summary>
        public int BufferMetricCount { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime ReportTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 客户端性能监控状态上报响应
    /// </summary>
    public class PerformanceMonitorStatusResponse : ResponseBase
    {
        /// <summary>
        /// 服务器确认时间
        /// </summary>
        public DateTime ServerAckTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 服务器建议的配置
        /// </summary>
        public string SuggestedConfigJson { get; set; }
    }
}
