using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;
using RUINORERP.PacketSpec.Commands.CommandDefinitions;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 性能数据存储服务
    /// 在服务器内存中存储和管理客户端上报的性能监控数据
    /// </summary>
    public class PerformanceDataStorageService : IDisposable
    {
        private readonly ILogger<PerformanceDataStorageService> _logger;
        private readonly ConcurrentDictionary<string, ClientPerformanceDataStore> _clientDataStores;
        private readonly Timer _cleanupTimer;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

        // 内存中保留数据的最大时间（默认60分钟）
        private readonly TimeSpan _dataRetentionTime = TimeSpan.FromMinutes(60);
        // 单个客户端最大存储记录数
        private const int MaxRecordsPerClient = 10000;
        // 全局最大存储记录数
        private const int MaxTotalRecords = 100000;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceDataStorageService(ILogger<PerformanceDataStorageService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clientDataStores = new ConcurrentDictionary<string, ClientPerformanceDataStore>();

            // 每5分钟清理一次过期数据
            _cleanupTimer = new Timer(CleanupOldDataCallback, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

            _logger.LogInformation("性能数据存储服务初始化完成");
        }

        /// <summary>
        /// 存储性能数据
        /// </summary>
        public void StorePerformanceData(string clientId, List<PerformanceMetricBase> metrics)
        {
            if (string.IsNullOrEmpty(clientId) || metrics == null || metrics.Count == 0)
                return;

            var store = _clientDataStores.GetOrAdd(clientId, _ => new ClientPerformanceDataStore(clientId));

            foreach (var metric in metrics)
            {
                store.AddMetric(metric);
            }

            // 检查是否超过单客户端限制
            if (store.TotalCount > MaxRecordsPerClient)
            {
                store.TrimToSize(MaxRecordsPerClient);
            }

            // 检查全局限制
            var totalRecords = _clientDataStores.Sum(s => s.Value.TotalCount);
            if (totalRecords > MaxTotalRecords)
            {
                CleanupOldestData((int)(totalRecords - MaxTotalRecords));
            }

            _logger.LogDebug($"存储性能数据: 客户端 {clientId}, {metrics.Count} 条指标");
        }

        /// <summary>
        /// 存储性能数据（从请求）
        /// </summary>
        public PerformanceDataUploadResponse StorePerformanceData(PerformanceDataUploadRequest request)
        {
            var response = new PerformanceDataUploadResponse
            {
                ServerReceiveTime = DateTime.Now
            };

            try
            {
                if (!string.IsNullOrEmpty(request.PerformanceDataJson))
                {
                    var packet = JsonConvert.DeserializeObject<PerformanceDataPacket>(request.PerformanceDataJson);
                    if (packet != null)
                    {
                        var metrics = packet.GetAllMetrics();
                        StorePerformanceData(request.ClientId, metrics);

                        response.ProcessedMetricCount = metrics.Count;

                        if (_clientDataStores.TryGetValue(request.ClientId, out var store))
                        {
                            response.TotalStoredMetrics = store.TotalCount;
                        }

                        _logger.LogInformation($"接收性能数据: 客户端 {request.ClientId}, 机器 {request.MachineName}, {metrics.Count} 条指标");
                    }
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"存储性能数据失败: 客户端 {request.ClientId}");
                response.IsSuccess = false;
                response.ErrorMessage = $"存储失败: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// 查询性能数据
        /// </summary>
        public PerformanceDataQueryResponse QueryPerformanceData(PerformanceDataQueryRequest request)
        {
            var response = new PerformanceDataQueryResponse
            {
                IsSuccess = true
            };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var allMetrics = new List<PerformanceMetricBase>();

                if (string.IsNullOrEmpty(request.TargetClientId))
                {
                    // 查询所有客户端
                    foreach (var store in _clientDataStores.Values)
                    {
                        var metrics = store.QueryMetrics(request.StartTime, request.EndTime, request.MetricTypes);
                        allMetrics.AddRange(metrics);
                    }
                }
                else if (_clientDataStores.TryGetValue(request.TargetClientId, out var clientStore))
                {
                    // 查询特定客户端
                    var metrics = clientStore.QueryMetrics(request.StartTime, request.EndTime, request.MetricTypes);
                    allMetrics.AddRange(metrics);
                }

                // 限制返回记录数
                if (allMetrics.Count > request.MaxRecords)
                {
                    allMetrics = allMetrics.OrderByDescending(m => m.Timestamp)
                                           .Take(request.MaxRecords)
                                           .ToList();
                }

                // 序列化指标数据
                response.MetricsJson = allMetrics.Select(m => JsonConvert.SerializeObject(m)).ToList();
                response.TotalRecords = allMetrics.Count;

                // 生成统计摘要
                if (request.IncludeSummary && allMetrics.Count > 0)
                {
                    var summary = GenerateStatisticsSummary(allMetrics);
                    response.SummaryJson = JsonConvert.SerializeObject(summary);
                }

                _logger.LogDebug($"查询性能数据: 返回 {response.TotalRecords} 条记录");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询性能数据失败");
                response.IsSuccess = false;
                response.ErrorMessage = $"查询失败: {ex.Message}";
            }

            stopwatch.Stop();
            response.QueryExecutionTimeMs = stopwatch.ElapsedMilliseconds;

            return response;
        }

        /// <summary>
        /// 获取统计摘要
        /// </summary>
        public PerformanceStatisticsResponse GetStatisticsSummary(PerformanceStatisticsRequest request)
        {
            var response = new PerformanceStatisticsResponse
            {
                IsSuccess = true,
                TimeSpan = TimeSpan.FromHours(request.TimeRangeHours)
            };

            try
            {
                var startTime = DateTime.Now.AddHours(-request.TimeRangeHours);
                var allMetrics = new List<PerformanceMetricBase>();

                if (string.IsNullOrEmpty(request.TargetClientId))
                {
                    foreach (var store in _clientDataStores.Values)
                    {
                        var metrics = store.QueryMetrics(startTime, null, request.IncludeMetricTypes);
                        allMetrics.AddRange(metrics);
                    }
                }
                else if (_clientDataStores.TryGetValue(request.TargetClientId, out var clientStore))
                {
                    var metrics = clientStore.QueryMetrics(startTime, null, request.IncludeMetricTypes);
                    allMetrics.AddRange(metrics);
                }

                var summary = GenerateStatisticsSummary(allMetrics);
                response.StatisticsJson = JsonConvert.SerializeObject(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成统计摘要失败");
                response.IsSuccess = false;
                response.ErrorMessage = $"生成统计摘要失败: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// 获取所有客户端ID列表
        /// </summary>
        public List<string> GetAllClientIds()
        {
            return _clientDataStores.Keys.ToList();
        }

        /// <summary>
        /// 获取客户端统计信息
        /// </summary>
        public ClientPerformanceInfo GetClientInfo(string clientId)
        {
            if (_clientDataStores.TryGetValue(clientId, out var store))
            {
                return new ClientPerformanceInfo
                {
                    ClientId = clientId,
                    TotalMetrics = store.TotalCount,
                    FirstReportTime = store.FirstReportTime,
                    LastReportTime = store.LastReportTime,
                    MetricTypeCounts = store.GetMetricTypeCounts()
                };
            }
            return null;
        }

        /// <summary>
        /// 获取所有客户端统计信息
        /// </summary>
        public List<ClientPerformanceInfo> GetAllClientInfos()
        {
            return _clientDataStores.Values.Select(s => new ClientPerformanceInfo
            {
                ClientId = s.ClientId,
                TotalMetrics = s.TotalCount,
                FirstReportTime = s.FirstReportTime,
                LastReportTime = s.LastReportTime,
                MetricTypeCounts = s.GetMetricTypeCounts()
            }).ToList();
        }

        /// <summary>
        /// 清除指定客户端的数据
        /// </summary>
        public bool ClearClientData(string clientId)
        {
            if (_clientDataStores.TryRemove(clientId, out _))
            {
                _logger.LogInformation($"清除客户端性能数据: {clientId}");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void ClearAllData()
        {
            _clientDataStores.Clear();
            _logger.LogInformation("清除所有性能监控数据");
        }

        /// <summary>
        /// 生成统计摘要
        /// </summary>
        private PerformanceStatisticsSummary GenerateStatisticsSummary(List<PerformanceMetricBase> metrics)
        {
            var summary = new PerformanceStatisticsSummary
            {
                StartTime = metrics.Min(m => m.Timestamp),
                EndTime = metrics.Max(m => m.Timestamp)
            };

            // 方法执行统计
            var methodMetrics = metrics.OfType<MethodExecutionMetric>().ToList();
            if (methodMetrics.Any())
            {
                summary.MethodSummary = new MethodExecutionSummary
                {
                    TotalCalls = methodMetrics.Count,
                    SuccessfulCalls = methodMetrics.Count(m => m.IsSuccess),
                    FailedCalls = methodMetrics.Count(m => !m.IsSuccess),
                    AverageExecutionTimeMs = methodMetrics.Average(m => m.ExecutionTimeMs),
                    MaxExecutionTimeMs = methodMetrics.Max(m => m.ExecutionTimeMs),
                    MinExecutionTimeMs = methodMetrics.Min(m => m.ExecutionTimeMs),
                    MethodCallCounts = methodMetrics.GroupBy(m => $"{m.ClassName}.{m.MethodName}")
                                                    .ToDictionary(g => g.Key, g => (long)g.Count())
                };
            }

            // 数据库统计
            var dbMetrics = metrics.OfType<DatabasePerformanceMetric>().ToList();
            if (dbMetrics.Any())
            {
                summary.DatabaseSummary = new DatabasePerformanceSummary
                {
                    TotalQueries = dbMetrics.Count,
                    DeadlockCount = dbMetrics.Count(m => m.IsDeadlock),
                    AverageQueryTimeMs = dbMetrics.Average(m => m.ExecutionTimeMs),
                    SlowQueryCount = dbMetrics.Count(m => m.ExecutionTimeMs > 5000),
                    TableAccessCounts = dbMetrics.GroupBy(m => m.TableName)
                                                 .ToDictionary(g => g.Key, g => (long)g.Count())
                };
            }

            // 网络统计
            var networkMetrics = metrics.OfType<NetworkPerformanceMetric>().ToList();
            if (networkMetrics.Any())
            {
                summary.NetworkSummary = new NetworkPerformanceSummary
                {
                    TotalRequests = networkMetrics.Count,
                    SuccessfulRequests = networkMetrics.Count(m => m.IsSuccess),
                    FailedRequests = networkMetrics.Count(m => !m.IsSuccess),
                    AverageResponseTimeMs = networkMetrics.Average(m => m.TotalDurationMs),
                    TotalBytesSent = networkMetrics.Sum(m => m.BytesSent),
                    TotalBytesReceived = networkMetrics.Sum(m => m.BytesReceived),
                    CommandTypeCounts = networkMetrics.GroupBy(m => m.CommandType)
                                                      .ToDictionary(g => g.Key, g => (long)g.Count())
                };
            }

            // 内存统计
            var memoryMetrics = metrics.OfType<MemoryPerformanceMetric>().ToList();
            if (memoryMetrics.Any())
            {
                summary.MemorySummary = new MemoryPerformanceSummary
                {
                    AverageWorkingSetMB = (long)(memoryMetrics.Average(m => m.WorkingSetBytes) / (1024 * 1024)),
                    MaxWorkingSetMB = memoryMetrics.Max(m => m.WorkingSetBytes) / (1024 * 1024),
                    AverageManagedMemoryMB = (long)(memoryMetrics.Average(m => m.ManagedMemoryBytes) / (1024 * 1024)),
                    MaxManagedMemoryMB = memoryMetrics.Max(m => m.ManagedMemoryBytes) / (1024 * 1024),
                    GCTriggerCount = memoryMetrics.Count(m => m.GCTriggered),
                    AverageThreadCount = (int)memoryMetrics.Average(m => m.ThreadCount)
                };
            }

            // 事务统计
            var transactionMetrics = metrics.OfType<TransactionMetric>().ToList();
            if (transactionMetrics.Any())
            {
                summary.TransactionSummary = new TransactionSummary
                {
                    TotalTransactions = transactionMetrics.Count,
                    CommittedCount = transactionMetrics.Count(m => m.IsCommitted),
                    RolledBackCount = transactionMetrics.Count(m => !m.IsCommitted),
                    DeadlockCount = transactionMetrics.Count(m => m.IsDeadlock),
                    AverageDurationMs = transactionMetrics.Where(m => m.DurationMs.HasValue).Average(m => m.DurationMs.Value),
                    LongRunningTransactionCount = transactionMetrics.Count(m => m.DurationMs.HasValue && m.DurationMs.Value > 30000)
                };
            }

            // 死锁统计
            var deadlockMetrics = metrics.OfType<DeadlockMetric>().ToList();
            if (deadlockMetrics.Any())
            {
                summary.DeadlockSummary = new DeadlockSummary
                {
                    TotalDeadlocks = deadlockMetrics.Count,
                    FirstDeadlockTime = deadlockMetrics.Min(m => m.DeadlockTime),
                    LastDeadlockTime = deadlockMetrics.Max(m => m.DeadlockTime),
                    DeadlockResourceCounts = deadlockMetrics.SelectMany(m => m.InvolvedResources)
                                                            .GroupBy(r => r)
                                                            .ToDictionary(g => g.Key, g => g.Count())
                };
            }

            return summary;
        }

        /// <summary>
        /// 清理过期数据回调
        /// </summary>
        private void CleanupOldDataCallback(object state)
        {
            try
            {
                var cutoffTime = DateTime.Now - _dataRetentionTime;

                foreach (var store in _clientDataStores.Values)
                {
                    store.RemoveOldMetrics(cutoffTime);
                }

                // 清理空存储
                var emptyClients = _clientDataStores.Where(s => s.Value.TotalCount == 0)
                                                    .Select(s => s.Key)
                                                    .ToList();

                foreach (var clientId in emptyClients)
                {
                    _clientDataStores.TryRemove(clientId, out _);
                }

                _logger.LogDebug($"清理过期性能数据完成，移除了 {emptyClients.Count} 个空客户端存储");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期性能数据失败");
            }
        }

        /// <summary>
        /// 清理最旧的数据
        /// </summary>
        private void CleanupOldestData(int count)
        {
            var allMetrics = _clientDataStores.Values
                .SelectMany(s => s.GetAllMetrics())
                .OrderBy(m => m.Timestamp)
                .Take(count)
                .ToList();

            foreach (var metric in allMetrics)
            {
                if (_clientDataStores.TryGetValue(metric.ClientId, out var store))
                {
                    store.RemoveMetric(metric.MetricId);
                }
            }

            _logger.LogDebug($"清理最旧性能数据: {count} 条");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _cleanupTimer?.Dispose();
            _clientDataStores.Clear();

            _logger.LogInformation("性能数据存储服务已释放");
        }
    }

    /// <summary>
    /// 客户端性能数据存储
    /// </summary>
    public class ClientPerformanceDataStore
    {
        public string ClientId { get; }
        public long TotalCount => _metrics.Count;
        public DateTime FirstReportTime { get; private set; } = DateTime.MaxValue;
        public DateTime LastReportTime { get; private set; } = DateTime.MinValue;

        private readonly ConcurrentDictionary<string, PerformanceMetricBase> _metrics;
        private readonly ConcurrentDictionary<PerformanceMonitorType, long> _metricTypeCounts;

        public ClientPerformanceDataStore(string clientId)
        {
            ClientId = clientId;
            _metrics = new ConcurrentDictionary<string, PerformanceMetricBase>();
            _metricTypeCounts = new ConcurrentDictionary<PerformanceMonitorType, long>();
        }

        public void AddMetric(PerformanceMetricBase metric)
        {
            metric.ClientId = ClientId;
            _metrics[metric.MetricId] = metric;

            _metricTypeCounts.AddOrUpdate(metric.MetricType, 1, (_, count) => count + 1);

            if (metric.Timestamp < FirstReportTime)
                FirstReportTime = metric.Timestamp;

            if (metric.Timestamp > LastReportTime)
                LastReportTime = metric.Timestamp;
        }

        public bool RemoveMetric(string metricId)
        {
            if (_metrics.TryRemove(metricId, out var metric))
            {
                _metricTypeCounts.AddOrUpdate(metric.MetricType, 0, (_, count) => Math.Max(0, count - 1));
                return true;
            }
            return false;
        }

        public void RemoveOldMetrics(DateTime cutoffTime)
        {
            var oldMetrics = _metrics.Where(m => m.Value.Timestamp < cutoffTime)
                                    .Select(m => m.Key)
                                    .ToList();

            foreach (var metricId in oldMetrics)
            {
                RemoveMetric(metricId);
            }
        }

        public void TrimToSize(int maxSize)
        {
            while (_metrics.Count > maxSize)
            {
                var oldest = _metrics.OrderBy(m => m.Value.Timestamp).FirstOrDefault();
                if (oldest.Key != null)
                {
                    RemoveMetric(oldest.Key);
                }
                else
                {
                    break;
                }
            }
        }

        public List<PerformanceMetricBase> QueryMetrics(DateTime? startTime, DateTime? endTime, List<int> metricTypes)
        {
            var query = _metrics.Values.AsEnumerable();

            if (startTime.HasValue)
                query = query.Where(m => m.Timestamp >= startTime.Value);

            if (endTime.HasValue)
                query = query.Where(m => m.Timestamp <= endTime.Value);

            if (metricTypes != null && metricTypes.Count > 0)
            {
                var types = metricTypes.Cast<PerformanceMonitorType>().ToList();
                query = query.Where(m => types.Contains(m.MetricType));
            }

            return query.OrderByDescending(m => m.Timestamp).ToList();
        }

        public List<PerformanceMetricBase> GetAllMetrics()
        {
            return _metrics.Values.ToList();
        }

        public Dictionary<PerformanceMonitorType, long> GetMetricTypeCounts()
        {
            return _metricTypeCounts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

    /// <summary>
    /// 客户端性能信息
    /// </summary>
    public class ClientPerformanceInfo
    {
        public string ClientId { get; set; }
        public long TotalMetrics { get; set; }
        public DateTime FirstReportTime { get; set; }
        public DateTime LastReportTime { get; set; }
        public Dictionary<PerformanceMonitorType, long> MetricTypeCounts { get; set; }
    }
}
