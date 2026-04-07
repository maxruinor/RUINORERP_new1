using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Model.Base.StatusManager.PerformanceMonitoring
{
    /// <summary>
    /// 性能指标基类
    /// </summary>
    public abstract class PerformanceMetricBase
    {
        /// <summary>
        /// 指标ID
        /// </summary>
        public string MetricId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 客户端ID/会话ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 指标类型
        /// </summary>
        public PerformanceMonitorType MetricType { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 机器名称
        /// </summary>
        public string MachineName { get; set; } = Environment.MachineName;

        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; set; } = Process.GetCurrentProcess().Id;

        /// <summary>
        /// 转换为字典（用于JSON序列化）
        /// </summary>
        /// <returns>包含所有属性的字典</returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                [nameof(MetricId)] = MetricId,
                [nameof(ClientId)] = ClientId,
                [nameof(MetricType)] = MetricType,
                [nameof(Timestamp)] = Timestamp,
                [nameof(MachineName)] = MachineName,
                [nameof(ProcessId)] = ProcessId
            };
        }
    }

    /// <summary>
    /// 方法执行性能指标
    /// </summary>
    public class MethodExecutionMetric : PerformanceMetricBase
    {
        public MethodExecutionMetric()
        {
            MetricType = PerformanceMonitorType.MethodExecution;
        }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 执行耗时（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 是否成功执行
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常信息（如果执行失败）
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 调用参数摘要（可选）
        /// </summary>
        public string ParametersSummary { get; set; }
    }

    /// <summary>
    /// 数据库性能指标
    /// </summary>
    public class DatabasePerformanceMetric : PerformanceMetricBase
    {
        public DatabasePerformanceMetric()
        {
            MetricType = PerformanceMonitorType.Database;
        }

        /// <summary>
        /// SQL语句/操作类型
        /// </summary>
        public string SqlText { get; set; }

        /// <summary>
        /// 操作类型（Query/Insert/Update/Delete/Transaction）
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 执行耗时（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 影响行数
        /// </summary>
        public int AffectedRows { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 是否死锁
        /// </summary>
        public bool IsDeadlock { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 事务ID
        /// </summary>
        public string TransactionId { get; set; }
    }

    /// <summary>
    /// 网络性能指标
    /// </summary>
    public class NetworkPerformanceMetric : PerformanceMetricBase
    {
        public NetworkPerformanceMetric()
        {
            MetricType = PerformanceMonitorType.Network;
        }

        /// <summary>
        /// 请求ID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime RequestStartTime { get; set; }

        /// <summary>
        /// 请求结束时间
        /// </summary>
        public DateTime RequestEndTime { get; set; }

        /// <summary>
        /// 总耗时（毫秒）
        /// </summary>
        public long TotalDurationMs { get; set; }

        /// <summary>
        /// 发送字节数
        /// </summary>
        public long BytesSent { get; set; }

        /// <summary>
        /// 接收字节数
        /// </summary>
        public long BytesReceived { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 服务器处理时间（毫秒）
        /// </summary>
        public long ServerProcessingTimeMs { get; set; }
    }

    /// <summary>
    /// 内存使用指标
    /// </summary>
    public class MemoryPerformanceMetric : PerformanceMetricBase
    {
        public MemoryPerformanceMetric()
        {
            MetricType = PerformanceMonitorType.Memory;
        }

        /// <summary>
        /// 工作集内存（字节）
        /// </summary>
        public long WorkingSetBytes { get; set; }

        /// <summary>
        /// 托管堆内存（字节）
        /// </summary>
        public long ManagedMemoryBytes { get; set; }

        /// <summary>
        /// GC第0代回收次数
        /// </summary>
        public int Gen0Collections { get; set; }

        /// <summary>
        /// GC第1代回收次数
        /// </summary>
        public int Gen1Collections { get; set; }

        /// <summary>
        /// GC第2代回收次数
        /// </summary>
        public int Gen2Collections { get; set; }

        /// <summary>
        /// 当前线程数
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        /// 线程池可用工作线程数
        /// </summary>
        public int AvailableWorkerThreads { get; set; }

        /// <summary>
        /// 线程池可用IO线程数
        /// </summary>
        public int AvailableIoThreads { get; set; }

        /// <summary>
        /// CPU使用率（百分比）
        /// </summary>
        public double CpuUsagePercent { get; set; }

        /// <summary>
        /// 是否触发GC
        /// </summary>
        public bool GCTriggered { get; set; }
    }

    /// <summary>
    /// 缓存性能指标
    /// </summary>
    public class CachePerformanceMetric : PerformanceMetricBase
    {
        public CachePerformanceMetric()
        {
            MetricType = PerformanceMonitorType.Cache;
        }

        /// <summary>
        /// 缓存键
        /// </summary>
        public string CacheKey { get; set; }

        /// <summary>
        /// 操作类型（Get/Set/Remove）
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 是否命中
        /// </summary>
        public bool IsHit { get; set; }

        /// <summary>
        /// 缓存项大小（字节，可选）
        /// </summary>
        public long? ItemSizeBytes { get; set; }

        /// <summary>
        /// 操作耗时（毫秒）
        /// </summary>
        public long OperationTimeMs { get; set; }

        /// <summary>
        /// 缓存总项数
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 缓存命中率（百分比）
        /// </summary>
        public double HitRate { get; set; }
    }

    /// <summary>
    /// UI响应性能指标
    /// </summary>
    public class UIResponseMetric : PerformanceMetricBase
    {
        public UIResponseMetric()
        {
            MetricType = PerformanceMonitorType.UIResponse;
        }

        /// <summary>
        /// 窗体/控件名称
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// 操作类型（Load/Render/Click/DataBinding）
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 响应耗时（毫秒）
        /// </summary>
        public long ResponseTimeMs { get; set; }

        /// <summary>
        /// 数据行数（如果是数据操作）
        /// </summary>
        public int DataRowCount { get; set; }

        /// <summary>
        /// 是否卡顿（超过阈值）
        /// </summary>
        public bool IsStuttering { get; set; }
    }

    /// <summary>
    /// 事务性能指标
    /// </summary>
    public class TransactionMetric : PerformanceMetricBase
    {
        public TransactionMetric()
        {
            MetricType = PerformanceMonitorType.Transaction;
        }

        /// <summary>
        /// 事务ID
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 操作类型（Begin/Commit/Rollback）
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 事务开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 事务结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 事务持续时间（毫秒）
        /// </summary>
        public long? DurationMs { get; set; }

        /// <summary>
        /// 是否成功提交
        /// </summary>
        public bool IsCommitted { get; set; }

        /// <summary>
        /// 是否发生死锁
        /// </summary>
        public bool IsDeadlock { get; set; }

        /// <summary>
        /// 死锁信息
        /// </summary>
        public string DeadlockInfo { get; set; }

        /// <summary>
        /// 涉及的数据表
        /// </summary>
        public List<string> InvolvedTables { get; set; } = new List<string>();
    }

    /// <summary>
    /// 死锁检测指标
    /// </summary>
    public class DeadlockMetric : PerformanceMetricBase
    {
        public DeadlockMetric()
        {
            MetricType = PerformanceMonitorType.Deadlock;
        }

        /// <summary>
        /// 死锁ID
        /// </summary>
        public string DeadlockId { get; set; }

        /// <summary>
        /// 死锁发生时间
        /// </summary>
        public DateTime DeadlockTime { get; set; }

        /// <summary>
        /// 涉及的资源
        /// </summary>
        public List<string> InvolvedResources { get; set; } = new List<string>();

        /// <summary>
        /// 涉及的进程/会话
        /// </summary>
        public List<string> InvolvedProcesses { get; set; } = new List<string>();

        /// <summary>
        /// 牺牲进程
        /// </summary>
        public string VictimProcess { get; set; }

        /// <summary>
        /// 死锁图XML（SQL Server提供）
        /// </summary>
        public string DeadlockGraphXml { get; set; }

        /// <summary>
        /// 死锁详情
        /// </summary>
        public string DeadlockDetails { get; set; }
    }

    /// <summary>
    /// 性能监控数据包
    /// 用于SuperSocket传输
    /// </summary>
    public class PerformanceDataPacket
    {
        /// <summary>
        /// 数据包ID
        /// </summary>
        public string PacketId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 指标数据列表（JSON序列化）
        /// </summary>
        public List<string> MetricsJson { get; set; } = new List<string>();

        /// <summary>
        /// 指标类型列表
        /// </summary>
        public List<PerformanceMonitorType> MetricTypes { get; set; } = new List<PerformanceMonitorType>();

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        public int PacketSizeBytes { get; set; }

        /// <summary>
        /// 添加指标
        /// </summary>
        public void AddMetric<T>(T metric) where T : PerformanceMetricBase
        {
            var json = JsonConvert.SerializeObject(metric);
            MetricsJson.Add(json);
            MetricTypes.Add(metric.MetricType);
        }

        /// <summary>
        /// 获取所有指标
        /// </summary>
        public List<PerformanceMetricBase> GetAllMetrics()
        {
            var metrics = new List<PerformanceMetricBase>();
            for (int i = 0; i < MetricsJson.Count; i++)
            {
                var metric = DeserializeMetric(MetricsJson[i], MetricTypes[i]);
                if (metric != null)
                {
                    metrics.Add(metric);
                }
            }
            return metrics;
        }

        /// <summary>
        /// 反序列化指标
        /// </summary>
        private PerformanceMetricBase DeserializeMetric(string json, PerformanceMonitorType type)
        {
            return type switch
            {
                PerformanceMonitorType.MethodExecution => JsonConvert.DeserializeObject<MethodExecutionMetric>(json),
                PerformanceMonitorType.Database => JsonConvert.DeserializeObject<DatabasePerformanceMetric>(json),
                PerformanceMonitorType.Network => JsonConvert.DeserializeObject<NetworkPerformanceMetric>(json),
                PerformanceMonitorType.Memory => JsonConvert.DeserializeObject<MemoryPerformanceMetric>(json),
                PerformanceMonitorType.Cache => JsonConvert.DeserializeObject<CachePerformanceMetric>(json),
                PerformanceMonitorType.UIResponse => JsonConvert.DeserializeObject<UIResponseMetric>(json),
                PerformanceMonitorType.Transaction => JsonConvert.DeserializeObject<TransactionMetric>(json),
                PerformanceMonitorType.Deadlock => JsonConvert.DeserializeObject<DeadlockMetric>(json),
                PerformanceMonitorType.DocumentLock => JsonConvert.DeserializeObject<DocumentLockMetric>(json),
                _ => null
            };
        }
    }

    /// <summary>
    /// 性能监控统计摘要
    /// </summary>
    public class PerformanceStatisticsSummary
    {
        /// <summary>
        /// 统计时间范围开始
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 统计时间范围结束
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 方法执行统计
        /// </summary>
        public MethodExecutionSummary MethodSummary { get; set; } = new MethodExecutionSummary();

        /// <summary>
        /// 数据库性能统计
        /// </summary>
        public DatabasePerformanceSummary DatabaseSummary { get; set; } = new DatabasePerformanceSummary();

        /// <summary>
        /// 网络性能统计
        /// </summary>
        public NetworkPerformanceSummary NetworkSummary { get; set; } = new NetworkPerformanceSummary();

        /// <summary>
        /// 内存使用统计
        /// </summary>
        public MemoryPerformanceSummary MemorySummary { get; set; } = new MemoryPerformanceSummary();

        /// <summary>
        /// 缓存性能统计
        /// </summary>
        public CachePerformanceSummary CacheSummary { get; set; } = new CachePerformanceSummary();

        /// <summary>
        /// 事务统计
        /// </summary>
        public TransactionSummary TransactionSummary { get; set; } = new TransactionSummary();

        /// <summary>
        /// 死锁统计
        /// </summary>
        public DeadlockSummary DeadlockSummary { get; set; } = new DeadlockSummary();
    }

    /// <summary>
    /// 方法执行统计摘要
    /// </summary>
    public class MethodExecutionSummary
    {
        public long TotalCalls { get; set; }
        public long SuccessfulCalls { get; set; }
        public long FailedCalls { get; set; }
        public double AverageExecutionTimeMs { get; set; }
        public long MaxExecutionTimeMs { get; set; }
        public long MinExecutionTimeMs { get; set; }
        public Dictionary<string, long> MethodCallCounts { get; set; } = new Dictionary<string, long>();
    }

    /// <summary>
    /// 数据库性能统计摘要
    /// </summary>
    public class DatabasePerformanceSummary
    {
        public long TotalQueries { get; set; }
        public long TotalTransactions { get; set; }
        public long DeadlockCount { get; set; }
        public double AverageQueryTimeMs { get; set; }
        public long SlowQueryCount { get; set; }
        public Dictionary<string, long> TableAccessCounts { get; set; } = new Dictionary<string, long>();
    }

    /// <summary>
    /// 网络性能统计摘要
    /// </summary>
    public class NetworkPerformanceSummary
    {
        public long TotalRequests { get; set; }
        public long SuccessfulRequests { get; set; }
        public long FailedRequests { get; set; }
        public double AverageResponseTimeMs { get; set; }
        public long TotalBytesSent { get; set; }
        public long TotalBytesReceived { get; set; }
        public Dictionary<string, long> CommandTypeCounts { get; set; } = new Dictionary<string, long>();
    }

    /// <summary>
    /// 内存使用统计摘要
    /// </summary>
    public class MemoryPerformanceSummary
    {
        public long AverageWorkingSetMB { get; set; }
        public long MaxWorkingSetMB { get; set; }
        public long AverageManagedMemoryMB { get; set; }
        public long MaxManagedMemoryMB { get; set; }
        public int GCTriggerCount { get; set; }
        public int AverageThreadCount { get; set; }
    }

    /// <summary>
    /// 缓存性能统计摘要
    /// </summary>
    public class CachePerformanceSummary
    {
        public long TotalOperations { get; set; }
        public long HitCount { get; set; }
        public long MissCount { get; set; }
        public double AverageHitRate { get; set; }
        public double AverageOperationTimeMs { get; set; }
    }

    /// <summary>
    /// 事务统计摘要
    /// </summary>
    public class TransactionSummary
    {
        public long TotalTransactions { get; set; }
        public long CommittedCount { get; set; }
        public long RolledBackCount { get; set; }
        public long DeadlockCount { get; set; }
        public double AverageDurationMs { get; set; }
        public long LongRunningTransactionCount { get; set; }
    }

    /// <summary>
    /// 死锁统计摘要
    /// </summary>
    public class DeadlockSummary
    {
        public long TotalDeadlocks { get; set; }
        public DateTime? FirstDeadlockTime { get; set; }
        public DateTime? LastDeadlockTime { get; set; }
        public Dictionary<string, int> DeadlockResourceCounts { get; set; } = new Dictionary<string, int>();
    }

    /// <summary>
    /// 单据锁定性能指标
    /// 用于监控服务器端单据锁定系统的运行状态
    /// </summary>
    public class DocumentLockMetric : PerformanceMetricBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DocumentLockMetric()
        {
            MetricType = PerformanceMonitorType.DocumentLock;
        }

        /// <summary>
        /// 当前活跃锁数量
        /// </summary>
        public int ActiveLockCount { get; set; }

        /// <summary>
        /// 过期锁数量（待清理）
        /// </summary>
        public int ExpiredLockCount { get; set; }

        /// <summary>
        /// 孤儿锁数量（会话断开但锁仍存在）
        /// </summary>
        public int OrphanedLockCount { get; set; }

        /// <summary>
        /// 待处理的解锁请求数量
        /// </summary>
        public int PendingUnlockRequestCount { get; set; }

        /// <summary>
        /// 锁超时次数（统计周期内）
        /// </summary>
        public long LockTimeoutCount { get; set; }

        /// <summary>
        /// 锁成功获取次数（统计周期内）
        /// </summary>
        public long LockAcquireSuccessCount { get; set; }

        /// <summary>
        /// 锁冲突次数（统计周期内）
        /// </summary>
        public long LockConflictCount { get; set; }

        /// <summary>
        /// 广播总次数（统计周期内）
        /// </summary>
        public long BroadcastTotalCount { get; set; }

        /// <summary>
        /// 广播成功次数（统计周期内）
        /// </summary>
        public long BroadcastSuccessCount { get; set; }

        /// <summary>
        /// 广播失败次数（统计周期内）
        /// </summary>
        public long BroadcastFailedCount { get; set; }

        /// <summary>
        /// 平均锁持有时间（秒）
        /// </summary>
        public double AverageLockHoldTimeSeconds { get; set; }

        /// <summary>
        /// 最大锁持有时间（秒）
        /// </summary>
        public long MaxLockHoldTimeSeconds { get; set; }

        /// <summary>
        /// 锁超时率（百分比）= LockTimeoutCount / LockAcquireSuccessCount * 100
        /// </summary>
        public double LockTimeoutRate => LockAcquireSuccessCount > 0 
            ? (double)LockTimeoutCount / LockAcquireSuccessCount * 100 
            : 0;

        /// <summary>
        /// 广播成功率（百分比）= BroadcastSuccessCount / BroadcastTotalCount * 100
        /// </summary>
        public double BroadcastSuccessRate => BroadcastTotalCount > 0 
            ? (double)BroadcastSuccessCount / BroadcastTotalCount * 100 
            : 100;

        /// <summary>
        /// 转换为字典（用于JSON序列化）
        /// </summary>
        public override Dictionary<string, object> ToDictionary()
        {
            var dict = base.ToDictionary();
            dict[nameof(ActiveLockCount)] = ActiveLockCount;
            dict[nameof(ExpiredLockCount)] = ExpiredLockCount;
            dict[nameof(OrphanedLockCount)] = OrphanedLockCount;
            dict[nameof(PendingUnlockRequestCount)] = PendingUnlockRequestCount;
            dict[nameof(LockTimeoutCount)] = LockTimeoutCount;
            dict[nameof(LockAcquireSuccessCount)] = LockAcquireSuccessCount;
            dict[nameof(LockConflictCount)] = LockConflictCount;
            dict[nameof(BroadcastTotalCount)] = BroadcastTotalCount;
            dict[nameof(BroadcastSuccessCount)] = BroadcastSuccessCount;
            dict[nameof(BroadcastFailedCount)] = BroadcastFailedCount;
            dict[nameof(AverageLockHoldTimeSeconds)] = AverageLockHoldTimeSeconds;
            dict[nameof(MaxLockHoldTimeSeconds)] = MaxLockHoldTimeSeconds;
            dict[nameof(LockTimeoutRate)] = Math.Round(LockTimeoutRate, 2);
            dict[nameof(BroadcastSuccessRate)] = Math.Round(BroadcastSuccessRate, 2);
            return dict;
        }
    }
}
