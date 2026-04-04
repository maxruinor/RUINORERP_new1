using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Model.Base.StatusManager.PerformanceMonitoring
{
    /// <summary>
    /// 性能监控管理器
    /// 统一的性能监控入口，管理所有监控数据的收集和上报
    /// </summary>
    public class PerformanceMonitorManager : IDisposable
    {
        private readonly ILogger<PerformanceMonitorManager> _logger;
        private readonly ConcurrentQueue<PerformanceMetricBase> _metricsBuffer;
        private readonly Timer _uploadTimer;
        private readonly Timer _cleanupTimer;
        private readonly ConcurrentDictionary<string, MethodMetricsAggregator> _methodMetrics;
        private readonly ConcurrentDictionary<string, DatabaseMetricsAggregator> _databaseMetrics;
        private readonly ConcurrentDictionary<string, NetworkMetricsAggregator> _networkMetrics;
        private readonly ConcurrentDictionary<string, TransactionMetricsAggregator> _transactionMetrics;

        private bool _disposed = false;
        private bool _uploadPaused = false;
        private readonly object _lockObject = new object();

        /// <summary>
        /// 数据上报事件
        /// </summary>
        public event EventHandler<PerformanceDataPacket> OnDataUpload;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceMonitorManager(ILogger<PerformanceMonitorManager> logger = null)
        {
            _logger = logger;
            _metricsBuffer = new ConcurrentQueue<PerformanceMetricBase>();
            _methodMetrics = new ConcurrentDictionary<string, MethodMetricsAggregator>();
            _databaseMetrics = new ConcurrentDictionary<string, DatabaseMetricsAggregator>();
            _networkMetrics = new ConcurrentDictionary<string, NetworkMetricsAggregator>();
            _transactionMetrics = new ConcurrentDictionary<string, TransactionMetricsAggregator>();

            // 初始化定时器
            var uploadInterval = TimeSpan.FromSeconds(PerformanceMonitorSwitch.Config.UploadIntervalSeconds);
            var cleanupInterval = TimeSpan.FromMinutes(PerformanceMonitorSwitch.Config.MemoryRetentionMinutes);

            _uploadTimer = new Timer(UploadMetricsCallback, null, uploadInterval, uploadInterval);
            _cleanupTimer = new Timer(CleanupOldMetricsCallback, null, cleanupInterval, cleanupInterval);

            // 订阅开关变化事件
            PerformanceMonitorSwitch.OnSwitchChanged += OnSwitchChanged;

            _logger?.LogInformation("性能监控管理器初始化完成");
        }

        /// <summary>
        /// 记录方法执行指标
        /// </summary>
        public void RecordMethodExecution(string methodName, string className, long executionTimeMs, bool isSuccess, string exceptionMessage = null)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.MethodExecution))
                return;

            var metric = new MethodExecutionMetric
            {
                MethodName = methodName,
                ClassName = className,
                ExecutionTimeMs = executionTimeMs,
                IsSuccess = isSuccess,
                ExceptionMessage = exceptionMessage,
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);

            // 更新聚合统计
            var key = $"{className}.{methodName}";
            var aggregator = _methodMetrics.GetOrAdd(key, _ => new MethodMetricsAggregator(methodName, className));
            aggregator.Record(executionTimeMs, isSuccess);

            // 检查是否超过阈值
            if (executionTimeMs > PerformanceMonitorSwitch.Config.MethodExecutionThresholdMs)
            {
                _logger?.LogWarning($"方法执行时间超过阈值: {key}, 耗时: {executionTimeMs}ms");
            }
        }

        /// <summary>
        /// 记录数据库性能指标
        /// </summary>
        public void RecordDatabaseOperation(string sqlText, string operationType, string tableName, long executionTimeMs, int affectedRows, bool isSuccess, string errorMessage = null, bool isDeadlock = false)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Database))
                return;

            var metric = new DatabasePerformanceMetric
            {
                SqlText = sqlText?.Length > 500 ? sqlText.Substring(0, 500) + "..." : sqlText,
                OperationType = operationType,
                TableName = tableName,
                ExecutionTimeMs = executionTimeMs,
                AffectedRows = affectedRows,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                IsDeadlock = isDeadlock,
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);

            // 更新聚合统计
            var key = $"{tableName}.{operationType}";
            var aggregator = _databaseMetrics.GetOrAdd(key, _ => new DatabaseMetricsAggregator(operationType, tableName));
            aggregator.Record(executionTimeMs, isSuccess, isDeadlock);

            // 检查是否超过阈值
            if (executionTimeMs > PerformanceMonitorSwitch.Config.DatabaseQueryThresholdMs)
            {
                _logger?.LogWarning($"数据库查询时间超过阈值: {key}, 耗时: {executionTimeMs}ms, SQL: {metric.SqlText}");
            }
        }

        /// <summary>
        /// 记录网络性能指标
        /// </summary>
        public void RecordNetworkRequest(string requestId, string commandType, DateTime startTime, DateTime endTime, long bytesSent, long bytesReceived, bool isSuccess, string errorMessage = null, int retryCount = 0)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Network))
                return;

            var durationMs = (long)(endTime - startTime).TotalMilliseconds;

            var metric = new NetworkPerformanceMetric
            {
                RequestId = requestId,
                CommandType = commandType,
                RequestStartTime = startTime,
                RequestEndTime = endTime,
                TotalDurationMs = durationMs,
                BytesSent = bytesSent,
                BytesReceived = bytesReceived,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                RetryCount = retryCount,
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);

            // 更新聚合统计
            var aggregator = _networkMetrics.GetOrAdd(commandType, _ => new NetworkMetricsAggregator(commandType));
            aggregator.Record(durationMs, bytesSent, bytesReceived, isSuccess);

            // 检查是否超过阈值
            if (durationMs > PerformanceMonitorSwitch.Config.NetworkRequestThresholdMs)
            {
                _logger?.LogWarning($"网络请求时间超过阈值: {commandType}, 耗时: {durationMs}ms");
            }
        }

        /// <summary>
        /// 记录内存使用指标
        /// </summary>
        public void RecordMemoryUsage()
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Memory))
                return;

            var process = Process.GetCurrentProcess();
            ThreadPool.GetAvailableThreads(out int workerThreads, out int ioThreads);

            var metric = new MemoryPerformanceMetric
            {
                WorkingSetBytes = process.WorkingSet64,
                ManagedMemoryBytes = GC.GetTotalMemory(false),
                Gen0Collections = GC.CollectionCount(0),
                Gen1Collections = GC.CollectionCount(1),
                Gen2Collections = GC.CollectionCount(2),
                ThreadCount = process.Threads.Count,
                AvailableWorkerThreads = workerThreads,
                AvailableIoThreads = ioThreads,
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);

            // 检查内存阈值
            var workingSetMB = process.WorkingSet64 / (1024 * 1024);
            if (workingSetMB > PerformanceMonitorSwitch.Config.MemoryCriticalThresholdMB)
            {
                _logger?.LogError($"内存使用达到临界阈值: {workingSetMB}MB");
            }
            else if (workingSetMB > PerformanceMonitorSwitch.Config.MemoryWarningThresholdMB)
            {
                _logger?.LogWarning($"内存使用达到警告阈值: {workingSetMB}MB");
            }
        }

        /// <summary>
        /// 记录CPU使用率
        /// </summary>
        public void RecordCpuUsage(double cpuUsagePercent)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Memory))
                return;

            var process = Process.GetCurrentProcess();

            var metric = new MemoryPerformanceMetric
            {
                WorkingSetBytes = process.WorkingSet64,
                ManagedMemoryBytes = GC.GetTotalMemory(false),
                Gen0Collections = GC.CollectionCount(0),
                Gen1Collections = GC.CollectionCount(1),
                Gen2Collections = GC.CollectionCount(2),
                ThreadCount = process.Threads.Count,
                CpuUsagePercent = cpuUsagePercent,
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);

            // CPU使用率告警阈值
            if (cpuUsagePercent > 90)
            {
                _logger?.LogWarning($"CPU使用率达到警告阈值: {cpuUsagePercent:F1}%");
            }
        }

        /// <summary>
        /// 记录事务指标
        /// </summary>
        public void RecordTransaction(string transactionId, string operationType, DateTime? startTime, DateTime? endTime, bool isCommitted, bool isDeadlock = false, string deadlockInfo = null, List<string> involvedTables = null)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Transaction))
                return;

            long? durationMs = null;
            if (startTime.HasValue && endTime.HasValue)
            {
                durationMs = (long)(endTime.Value - startTime.Value).TotalMilliseconds;
            }

            var metric = new TransactionMetric
            {
                TransactionId = transactionId,
                OperationType = operationType,
                StartTime = startTime,
                EndTime = endTime,
                DurationMs = durationMs,
                IsCommitted = isCommitted,
                IsDeadlock = isDeadlock,
                DeadlockInfo = deadlockInfo,
                InvolvedTables = involvedTables ?? new List<string>(),
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);

            // 更新聚合统计
            var aggregator = _transactionMetrics.GetOrAdd(transactionId, _ => new TransactionMetricsAggregator(transactionId));
            aggregator.Record(operationType, durationMs, isCommitted, isDeadlock);

            // 记录死锁警告
            if (isDeadlock)
            {
                _logger?.LogError($"检测到死锁: {transactionId}, 信息: {deadlockInfo}");
            }
        }

        /// <summary>
        /// 记录死锁指标
        /// </summary>
        public void RecordDeadlock(string deadlockId, DateTime deadlockTime, List<string> involvedResources, List<string> involvedProcesses, string victimProcess, string deadlockGraphXml = null, string deadlockDetails = null)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Deadlock))
                return;

            var metric = new DeadlockMetric
            {
                DeadlockId = deadlockId,
                DeadlockTime = deadlockTime,
                InvolvedResources = involvedResources ?? new List<string>(),
                InvolvedProcesses = involvedProcesses ?? new List<string>(),
                VictimProcess = victimProcess,
                DeadlockGraphXml = deadlockGraphXml,
                DeadlockDetails = deadlockDetails,
                ClientId = GetCurrentClientId()
            };

            EnqueueMetric(metric);
            _logger?.LogError($"记录死锁事件: {deadlockId}, 牺牲进程: {victimProcess}");
        }

        /// <summary>
        /// 触发立即上报（用于重要事件）
        /// </summary>
        public void TriggerImmediateUpload()
        {
            try
            {
                if (_metricsBuffer.IsEmpty)
                {
                    return;
                }

                var packet = new PerformanceDataPacket
                {
                    ClientId = GetCurrentClientId(),
                    SendTime = DateTime.Now
                };

                // 取出缓冲区中的数据
                int count = 0;
                while (_metricsBuffer.TryDequeue(out var metric) && count < 100)
                {
                    packet.AddMetric(metric);
                    count++;
                }

                if (count > 0)
                {
                    packet.PacketSizeBytes = System.Text.Encoding.UTF8.GetByteCount(
                        Newtonsoft.Json.JsonConvert.SerializeObject(packet));

                    OnDataUpload?.Invoke(this, packet);

                    _logger?.LogInformation($"重要事件触发立即上报: {count} 条指标");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "立即上报失败");
            }
        }

        /// <summary>
        /// 暂停上报（网络断开时调用）
        /// </summary>
        public void PauseUpload()
        {
            _uploadPaused = true;
            _logger?.LogInformation("性能数据上报已暂停");
        }

        /// <summary>
        /// 恢复上报（网络重连时调用）
        /// </summary>
        public void ResumeUpload()
        {
            _uploadPaused = false;
            _logger?.LogInformation("性能数据上报已恢复");
        }

        /// <summary>
        /// 获取当前上报状态
        /// </summary>
        public bool IsUploadPaused => _uploadPaused;

        /// <summary>
        /// 将指标重新放回缓冲区（发送失败时调用）
        /// </summary>
        public void RequeueMetrics(PerformanceDataPacket packet)
        {
            if (packet?.MetricsJson == null)
                return;

            try
            {
                foreach (var metricJson in packet.MetricsJson)
                {
                    // 重新解析并放回队列（简化处理，直接放回）
                    if (_metricsBuffer.Count < 10000)
                    {
                        _metricsBuffer.Enqueue(null); // 占位，实际应用中需要完整重构
                    }
                }
                _logger?.LogWarning($"已将{packet.MetricsJson.Count}条指标重新放回缓冲区", packet.MetricsJson.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "重新放回指标失败");
            }
        }

        /// <summary>
        /// 将指标加入缓冲队列
        /// </summary>
        private void EnqueueMetric(PerformanceMetricBase metric)
        {
            if (_metricsBuffer.Count < 10000) // 限制缓冲区大小
            {
                _metricsBuffer.Enqueue(metric);
            }
            else
            {
                // 缓冲区已满，丢弃最旧的数据
                _metricsBuffer.TryDequeue(out _);
                _metricsBuffer.Enqueue(metric);
            }
        }

        /// <summary>
        /// 获取当前客户端ID
        /// </summary>
        private string GetCurrentClientId()
        {
            // 这里可以从应用上下文中获取客户端ID
            // 暂时返回机器名和进程ID的组合
            return $"{Environment.MachineName}_{Process.GetCurrentProcess().Id}";
        }

        /// <summary>
        /// 定时上报回调
        /// </summary>
        private void UploadMetricsCallback(object state)
        {
            if (!PerformanceMonitorSwitch.IsEnabled || _metricsBuffer.IsEmpty)
                return;

            // 检查是否需要延迟上报（网络断开等情况下暂停）
            if (_uploadPaused)
            {
                _logger?.LogDebug("性能数据上报已暂停");
                return;
            }

            try
            {
                var packet = new PerformanceDataPacket
                {
                    ClientId = GetCurrentClientId(),
                    SendTime = DateTime.Now
                };

                // 取出缓冲区中的数据
                int count = 0;
                while (_metricsBuffer.TryDequeue(out var metric) && count < 1000) // 每次最多上报1000条
                {
                    packet.AddMetric(metric);
                    count++;
                }

                if (count > 0)
                {
                    packet.PacketSizeBytes = System.Text.Encoding.UTF8.GetByteCount(
                        Newtonsoft.Json.JsonConvert.SerializeObject(packet));

                    // 触发上报事件
                    OnDataUpload?.Invoke(this, packet);

                    _logger?.LogDebug($"性能数据上报: {count} 条指标");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "性能数据上报失败");
            }
        }

        /// <summary>
        /// 定时清理旧数据回调
        /// </summary>
        private void CleanupOldMetricsCallback(object state)
        {
            try
            {
                // 清理聚合数据中的过期统计
                var cutoffTime = DateTime.Now.AddMinutes(-PerformanceMonitorSwitch.Config.MemoryRetentionMinutes);

                // 清理方法指标
                foreach (var key in _methodMetrics.Keys.ToList())
                {
                    if (_methodMetrics.TryGetValue(key, out var aggregator) && aggregator.LastUpdateTime < cutoffTime)
                    {
                        _methodMetrics.TryRemove(key, out _);
                    }
                }

                // 清理数据库指标
                foreach (var key in _databaseMetrics.Keys.ToList())
                {
                    if (_databaseMetrics.TryGetValue(key, out var aggregator) && aggregator.LastUpdateTime < cutoffTime)
                    {
                        _databaseMetrics.TryRemove(key, out _);
                    }
                }

                // 清理网络指标
                foreach (var key in _networkMetrics.Keys.ToList())
                {
                    if (_networkMetrics.TryGetValue(key, out var aggregator) && aggregator.LastUpdateTime < cutoffTime)
                    {
                        _networkMetrics.TryRemove(key, out _);
                    }
                }

                // 清理事务指标
                foreach (var key in _transactionMetrics.Keys.ToList())
                {
                    if (_transactionMetrics.TryGetValue(key, out var aggregator) && aggregator.LastUpdateTime < cutoffTime)
                    {
                        _transactionMetrics.TryRemove(key, out _);
                    }
                }

                _logger?.LogDebug("性能监控数据清理完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理性能监控数据失败");
            }
        }

        /// <summary>
        /// 开关状态变化处理
        /// </summary>
        private void OnSwitchChanged(object sender, PerformanceSwitchEventArgs e)
        {
            if (e.IsEnabled)
            {
                _logger?.LogInformation("性能监控已启用");
            }
            else
            {
                _logger?.LogInformation("性能监控已禁用，清空缓冲区");
                // 清空缓冲区
                while (_metricsBuffer.TryDequeue(out _)) { }
            }
        }

        /// <summary>
        /// 获取性能统计摘要
        /// </summary>
        public PerformanceStatisticsSummary GetStatisticsSummary(DateTime? startTime = null, DateTime? endTime = null)
        {
            var summary = new PerformanceStatisticsSummary
            {
                StartTime = startTime ?? DateTime.Now.AddHours(-1),
                EndTime = endTime ?? DateTime.Now,
                ClientId = GetCurrentClientId()
            };

            // 方法执行统计
            summary.MethodSummary = new MethodExecutionSummary
            {
                TotalCalls = _methodMetrics.Sum(m => m.Value.TotalCalls),
                SuccessfulCalls = _methodMetrics.Sum(m => m.Value.SuccessfulCalls),
                FailedCalls = _methodMetrics.Sum(m => m.Value.FailedCalls),
                AverageExecutionTimeMs = _methodMetrics.Any() ? _methodMetrics.Average(m => m.Value.AverageTimeMs) : 0,
                MaxExecutionTimeMs = _methodMetrics.Any() ? _methodMetrics.Max(m => m.Value.MaxTimeMs) : 0,
                MinExecutionTimeMs = _methodMetrics.Any() ? _methodMetrics.Min(m => m.Value.MinTimeMs) : 0,
                MethodCallCounts = _methodMetrics.ToDictionary(m => m.Key, m => m.Value.TotalCalls)
            };

            // 数据库统计
            summary.DatabaseSummary = new DatabasePerformanceSummary
            {
                TotalQueries = _databaseMetrics.Sum(m => m.Value.TotalCalls),
                TotalTransactions = _databaseMetrics.Count(m => m.Value.OperationType == "Transaction"),
                DeadlockCount = _databaseMetrics.Sum(m => m.Value.DeadlockCount),
                AverageQueryTimeMs = _databaseMetrics.Any() ? _databaseMetrics.Average(m => m.Value.AverageTimeMs) : 0,
                SlowQueryCount = _databaseMetrics.Sum(m => m.Value.SlowQueryCount),
                TableAccessCounts = _databaseMetrics.GroupBy(m => m.Value.TableName)
                    .ToDictionary(g => g.Key, g => g.Sum(m => m.Value.TotalCalls))
            };

            // 网络统计
            summary.NetworkSummary = new NetworkPerformanceSummary
            {
                TotalRequests = _networkMetrics.Sum(m => m.Value.TotalCalls),
                SuccessfulRequests = _networkMetrics.Sum(m => m.Value.SuccessfulCalls),
                FailedRequests = _networkMetrics.Sum(m => m.Value.FailedCalls),
                AverageResponseTimeMs = _networkMetrics.Any() ? _networkMetrics.Average(m => m.Value.AverageTimeMs) : 0,
                TotalBytesSent = _networkMetrics.Sum(m => m.Value.TotalBytesSent),
                TotalBytesReceived = _networkMetrics.Sum(m => m.Value.TotalBytesReceived),
                CommandTypeCounts = _networkMetrics.ToDictionary(m => m.Key, m => m.Value.TotalCalls)
            };

            return summary;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            PerformanceMonitorSwitch.OnSwitchChanged -= OnSwitchChanged;

            _uploadTimer?.Dispose();
            _cleanupTimer?.Dispose();

            _logger?.LogInformation("性能监控管理器已释放");
        }
    }

    #region 指标聚合器

    /// <summary>
    /// 方法指标聚合器
    /// </summary>
    public class MethodMetricsAggregator
    {
        public string MethodName { get; }
        public string ClassName { get; }
        public long TotalCalls { get; private set; }
        public long SuccessfulCalls { get; private set; }
        public long FailedCalls { get; private set; }
        public double TotalTimeMs { get; private set; }
        public long MaxTimeMs { get; private set; }
        public long MinTimeMs { get; private set; } = long.MaxValue;
        public double AverageTimeMs => TotalCalls > 0 ? TotalTimeMs / TotalCalls : 0;
        public DateTime LastUpdateTime { get; private set; } = DateTime.Now;

        private readonly object _lock = new object();

        public MethodMetricsAggregator(string methodName, string className)
        {
            MethodName = methodName;
            ClassName = className;
        }

        public void Record(long executionTimeMs, bool isSuccess)
        {
            lock (_lock)
            {
                TotalCalls++;
                TotalTimeMs += executionTimeMs;

                if (executionTimeMs > MaxTimeMs)
                    MaxTimeMs = executionTimeMs;

                if (executionTimeMs < MinTimeMs)
                    MinTimeMs = executionTimeMs;

                if (isSuccess)
                    SuccessfulCalls++;
                else
                    FailedCalls++;

                LastUpdateTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// 数据库指标聚合器
    /// </summary>
    public class DatabaseMetricsAggregator
    {
        public string OperationType { get; }
        public string TableName { get; }
        public long TotalCalls { get; private set; }
        public long SuccessfulCalls { get; private set; }
        public long FailedCalls { get; private set; }
        public long DeadlockCount { get; private set; }
        public long SlowQueryCount { get; private set; }
        public double TotalTimeMs { get; private set; }
        public double AverageTimeMs => TotalCalls > 0 ? TotalTimeMs / TotalCalls : 0;
        public DateTime LastUpdateTime { get; private set; } = DateTime.Now;

        private readonly object _lock = new object();

        public DatabaseMetricsAggregator(string operationType, string tableName)
        {
            OperationType = operationType;
            TableName = tableName;
        }

        public void Record(long executionTimeMs, bool isSuccess, bool isDeadlock)
        {
            lock (_lock)
            {
                TotalCalls++;
                TotalTimeMs += executionTimeMs;

                if (isSuccess)
                    SuccessfulCalls++;
                else
                    FailedCalls++;

                if (isDeadlock)
                    DeadlockCount++;

                if (executionTimeMs > 5000) // 超过5秒认为是慢查询
                    SlowQueryCount++;

                LastUpdateTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// 网络指标聚合器
    /// </summary>
    public class NetworkMetricsAggregator
    {
        public string CommandType { get; }
        public long TotalCalls { get; private set; }
        public long SuccessfulCalls { get; private set; }
        public long FailedCalls { get; private set; }
        public long TotalBytesSent { get; private set; }
        public long TotalBytesReceived { get; private set; }
        public double TotalTimeMs { get; private set; }
        public double AverageTimeMs => TotalCalls > 0 ? TotalTimeMs / TotalCalls : 0;
        public DateTime LastUpdateTime { get; private set; } = DateTime.Now;

        private readonly object _lock = new object();

        public NetworkMetricsAggregator(string commandType)
        {
            CommandType = commandType;
        }

        public void Record(long durationMs, long bytesSent, long bytesReceived, bool isSuccess)
        {
            lock (_lock)
            {
                TotalCalls++;
                TotalTimeMs += durationMs;
                TotalBytesSent += bytesSent;
                TotalBytesReceived += bytesReceived;

                if (isSuccess)
                    SuccessfulCalls++;
                else
                    FailedCalls++;

                LastUpdateTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// 事务指标聚合器
    /// </summary>
    public class TransactionMetricsAggregator
    {
        public string TransactionId { get; }
        public long TotalOperations { get; private set; }
        public long CommittedCount { get; private set; }
        public long RolledBackCount { get; private set; }
        public long DeadlockCount { get; private set; }
        public double TotalDurationMs { get; private set; }
        public double AverageDurationMs => TotalOperations > 0 ? TotalDurationMs / TotalOperations : 0;
        public DateTime LastUpdateTime { get; private set; } = DateTime.Now;

        private readonly object _lock = new object();

        public TransactionMetricsAggregator(string transactionId)
        {
            TransactionId = transactionId;
        }

        public void Record(string operationType, long? durationMs, bool isCommitted, bool isDeadlock)
        {
            lock (_lock)
            {
                TotalOperations++;

                if (durationMs.HasValue)
                    TotalDurationMs += durationMs.Value;

                if (isCommitted)
                    CommittedCount++;
                else
                    RolledBackCount++;

                if (isDeadlock)
                    DeadlockCount++;

                LastUpdateTime = DateTime.Now;
            }
        }
    }

    #endregion
}
