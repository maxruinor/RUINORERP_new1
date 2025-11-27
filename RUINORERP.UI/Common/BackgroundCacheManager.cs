using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Base;
using RUINORERP.UI.Network.Services.Cache;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;
using RUINORERP.Business.Cache;
using RUINORERP.Model;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 后台缓存管理器 - 负责在后台异步加载缓存数据，避免阻塞UI线程
    /// 采用渐进式加载策略，优先加载关键数据，延迟加载次要数据
    /// </summary>
    public class BackgroundCacheManager : IDisposable
    {
        private readonly ILogger<BackgroundCacheManager> _logger;
        private readonly SemaphoreSlim _semaphore;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConcurrentQueue<CacheLoadTask> _pendingTasks;
        private readonly ConcurrentDictionary<string, CacheLoadStatus> _cacheStatus;
        private readonly Dictionary<string, TaskCompletionSource<bool>> _tableLocks;
        private readonly object _tableLocksLock;
        private readonly SortedSet<CacheLoadTask> _prioritizedTasks;
        private readonly object _prioritizedTasksLock;
        private readonly ConcurrentDictionary<string, DateTime> _lastUsedTime;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);
        private readonly int _maxTaskQueueSize = 1000;
        private readonly IDisposable _cleanupTimer;
        private readonly CancellationToken _cancellationToken;
        private readonly CacheClientService _cacheClient;
        private Task _backgroundWorker;
        private volatile bool _isRunning;
        private List<Task> _delayedTasks = new List<Task>(); // 存储延迟任务引用
        private DateTime _lastCleanupTime; // 不在声明时初始化

        // 配置参数
        private const int MaxConcurrentRequests = 3; // 最大并发请求数
        private const int DefaultTimeoutMs = 10000; // 默认超时时间10秒
        private const int ExtendedTimeoutMs = 30000; // 扩展超时时间30秒
        private const int RetryDelayMs = 2000; // 重试延迟2秒
        private const int MaxRetries = 2; // 最大重试次数
        private const int CacheStatusCleanupInterval = 300000; // 缓存状态清理间隔5分钟

        /// <summary>
        /// 缓存加载状态枚举
        /// </summary>
        public enum CacheLoadState
        {
            Pending,    // 待加载
            Loading,    // 加载中
            Loaded,     // 已加载
            Failed,     // 加载失败
            Fallback    // 使用降级数据
        }

        /// <summary>
        /// 缓存加载状态信息
        /// </summary>
        private class CacheLoadStatus
        {
            public CacheLoadState State { get; set; } = CacheLoadState.Pending;
            public DateTime LastAttemptTime { get; set; } = DateTime.MinValue;
            public int RetryCount { get; set; } = 0;
            public string ErrorMessage { get; set; }
            public DateTime LastUsedTime { get; set; } = DateTime.Now;
        }

        /// <summary>
        /// 缓存加载任务
        /// </summary>
        private class CacheLoadTask
        {
            public string TableName { get; set; }
            public Type EntityType { get; set; }
            public int Priority { get; set; } = 0; // 优先级，数值越大优先级越高
            public bool ForceRefresh { get; set; }
            public int TimeoutMs { get; set; } = DefaultTimeoutMs;
            public TaskCompletionSource<bool> CompletionSource { get; set; } = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BackgroundCacheManager(ILogger<BackgroundCacheManager> logger, CacheClientService cacheClient)
        {
            _logger = logger;
            _semaphore = new SemaphoreSlim(3, 3); // 最多3个并发任务
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _pendingTasks = new ConcurrentQueue<CacheLoadTask>();
            _cacheStatus = new ConcurrentDictionary<string, CacheLoadStatus>();
            _tableLocks = new Dictionary<string, TaskCompletionSource<bool>>();
            _tableLocksLock = new object();
            _prioritizedTasks = new SortedSet<CacheLoadTask>();
            _prioritizedTasksLock = new object();
            _lastUsedTime = new ConcurrentDictionary<string, DateTime>();
            _cacheClient = cacheClient;
            // 创建定期清理计时器
            _cleanupTimer = new Timer(_ => CleanupCacheStatus(), null, _cleanupInterval, _cleanupInterval);


            _lastCleanupTime = DateTime.Now; // 在构造函数中初始化
            
            // 启动后台工作线程
            _backgroundWorker = Task.Run(() => BackgroundWorkerAsync(_cancellationTokenSource.Token));

        }

        /// <summary>
        /// 添加缓存加载任务（高优先级）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        public Task<bool> AddHighPriorityTaskAsync(string tableName, Type entityType = null, bool forceRefresh = false, int timeoutMs = DefaultTimeoutMs)
        {
            return AddTaskInternal(tableName, entityType, 100, forceRefresh, timeoutMs);
        }

        /// <summary>
        /// 添加缓存加载任务（普通优先级）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        public Task<bool> AddNormalPriorityTaskAsync(string tableName, Type entityType = null, bool forceRefresh = false, int timeoutMs = ExtendedTimeoutMs)
        {
            return AddTaskInternal(tableName, entityType, 50, forceRefresh, timeoutMs);
        }

        /// <summary>
        /// 添加缓存加载任务（低优先级）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        public Task<bool> AddLowPriorityTaskAsync(string tableName, Type entityType = null, bool forceRefresh = false, int timeoutMs = ExtendedTimeoutMs)
        {
            return AddTaskInternal(tableName, entityType, 10, forceRefresh, timeoutMs);
        }

        /// <summary>
        /// 批量添加缓存加载任务
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <param name="priority">优先级</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        public async Task<Dictionary<string, bool>> AddBatchTasksAsync(IEnumerable<string> tableNames, int priority = 50, bool forceRefresh = false)
        {
            var tasks = new Dictionary<string, Task<bool>>();
            foreach (var tableName in tableNames)
            {
                tasks[tableName] = AddTaskInternal(tableName, null, priority, forceRefresh, ExtendedTimeoutMs);
            }

            var results = new Dictionary<string, bool>();
            foreach (var kvp in tasks)
            {
                results[kvp.Key] = await kvp.Value;
            }
            return results;
        }

        /// <summary>
        /// 获取缓存加载状态
        /// </summary>
        /// <param name="tableName">表名</param>
        public CacheLoadState GetCacheStatus(string tableName)
        {
            if (_cacheStatus.TryGetValue(tableName, out var status))
            {
                status.LastUsedTime = DateTime.Now; // 更新最后使用时间
                return status.State;
            }
            return CacheLoadState.Pending;
        }

        /// <summary>
        /// 等待缓存加载完成
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="timeoutMs">等待超时时间</param>
        public async Task<bool> WaitForCacheAsync(string tableName, int timeoutMs = 30000)
        {
            var cts = new CancellationTokenSource(timeoutMs);
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    var status = GetCacheStatus(tableName);
                    if (status == CacheLoadState.Loaded || status == CacheLoadState.Fallback)
                    {
                        return true;
                    }
                    if (status == CacheLoadState.Failed)
                    {
                        return false;
                    }
                    await Task.Delay(100, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // 超时异常，正常处理
            }
            return false;
        }

        /// <summary>
        /// 清理过期缓存状态
        /// </summary>
        private void CleanupCacheStatus()
        {
            try
            {
                var now = DateTime.Now;
                var cutoffTime = now.AddMinutes(-30); // 清理30分钟未使用的状态

                foreach (var key in _cacheStatus.Keys.ToList())
                {
                    if (_cacheStatus.TryGetValue(key, out var status) &&
                        status.LastUsedTime < cutoffTime &&
                        status.State != CacheLoadState.Loading)
                    {
                        _cacheStatus.TryRemove(key, out _);
                        _logger.LogDebug("缓存状态 {TableName} 已清理", key);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理缓存状态时发生错误");
            }
        }

        /// <summary>
        /// 内部添加任务方法
        /// </summary>
        private Task<bool> AddTaskInternal(string tableName, Type entityType, int priority, bool forceRefresh, int timeoutMs)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("表名不能为空", nameof(tableName));
            }

            // 检查任务队列大小
            if (_pendingTasks.Count > _maxTaskQueueSize)
            {
                _logger.LogWarning("任务队列已满，拒绝添加新任务: {TableName}", tableName);
                return Task.FromResult(false);
            }

            // 检查是否已在加载或已加载
            if (!forceRefresh && _cacheStatus.TryGetValue(tableName, out var existingStatus))
            {
                if (existingStatus.State == CacheLoadState.Loaded || existingStatus.State == CacheLoadState.Loading)
                {
                    _logger.LogDebug("缓存 {TableName} 已经处于 {State} 状态，跳过请求", tableName, existingStatus.State);
                    existingStatus.LastUsedTime = DateTime.Now; // 更新使用时间
                    return Task.FromResult(existingStatus.State == CacheLoadState.Loaded);
                }
            }

            var task = new CacheLoadTask
            {
                TableName = tableName,
                EntityType = entityType,
                Priority = priority,
                ForceRefresh = forceRefresh,
                TimeoutMs = timeoutMs
            };

            _pendingTasks.Enqueue(task);
            return task.CompletionSource.Task;
        }

        /// <summary>
        /// 后台工作线程
        /// </summary>
        private async Task BackgroundWorkerAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("后台缓存管理器启动");

            var prioritizedTasks = new SortedSet<CacheLoadTask>(new CacheTaskComparer());
            // 添加任务处理间隔，避免CPU占用过高
            const int ProcessIntervalMs = 100; // 任务处理间隔100ms

            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 定期清理缓存状态
                    if (DateTime.Now - _lastCleanupTime > TimeSpan.FromMilliseconds(CacheStatusCleanupInterval))
                    {
                        CleanupCacheStatus();
                    }

                    // 收集待处理任务
                    while (_pendingTasks.TryDequeue(out var task))
                    {
                        // 检查是否已在处理中，避免重复添加到优先队列
                        if (_cacheStatus.TryGetValue(task.TableName, out var status) && status.State == CacheLoadState.Loading)
                        {
                            _logger.LogDebug("缓存 {TableName} 正在加载中，跳过重复任务", task.TableName);
                            continue;
                        }

                        lock (_prioritizedTasksLock) // 线程安全地添加任务
                        {
                            prioritizedTasks.Add(task);
                        }
                    }

                    CacheLoadTask currentTask = null;
                    lock (_prioritizedTasksLock) // 线程安全地获取和移除任务
                    {
                        if (prioritizedTasks.Count > 0)
                        {
                            currentTask = prioritizedTasks.Max;
                            prioritizedTasks.Remove(currentTask);
                        }
                    }

                    if (currentTask == null)
                    {
                        await Task.Delay(100, cancellationToken);
                        continue;
                    }

                    await ProcessCacheTaskAsync(currentTask, cancellationToken);

                    // 添加任务处理间隔，避免连续处理导致的CPU占用过高
                    await Task.Delay(ProcessIntervalMs, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("后台缓存管理器被取消");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "后台缓存管理器处理任务时发生错误");
                    // 发生异常时短暂暂停，避免无限循环抛出异常
                    await Task.Delay(1000, cancellationToken);
                }
            }

            _logger.LogInformation("后台缓存管理器停止");
        }

        /// <summary>
        /// 处理缓存任务
        /// </summary>
        private async Task ProcessCacheTaskAsync(CacheLoadTask task, CancellationToken cancellationToken)
        {
            var tableName = task.TableName;

            // 更新状态为加载中
            var status = _cacheStatus.GetOrAdd(tableName, _ => new CacheLoadStatus());
            if (status.State == CacheLoadState.Loading)
            {
                task.CompletionSource.TrySetResult(false);
                return;
            }

            // 检查上次失败时间，如果距离太短则延迟处理
            if (status.State == CacheLoadState.Failed && DateTime.Now - status.LastAttemptTime < TimeSpan.FromSeconds(5))
            {
                _logger.LogDebug("缓存 {TableName} 距离上次失败时间过短，延迟处理", tableName);
                // 降低优先级后重新入队
                task.Priority = Math.Max(1, task.Priority / 2);
                _pendingTasks.Enqueue(task);
                task.CompletionSource.TrySetResult(false);
                return;
            }

            // 原子性地更新状态
            bool canStartLoading = false;
            if (status.State != CacheLoadState.Loading)
            {
                // 在实际更新前再次检查，确保没有其他线程已经开始加载
                canStartLoading = true;
                status.State = CacheLoadState.Loading;
                status.LastAttemptTime = DateTime.Now;
                status.LastUsedTime = DateTime.Now;
            }

            if (!canStartLoading)
            {
                task.CompletionSource.TrySetResult(false);
                return;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    // 直接调用底层缓存服务，而不是通过UIBizService，避免循环调用
                    // 获取表结构管理器实例
                    var tableSchemaManager = TableSchemaManager.Instance;

                    // 处理主表
                    if (tableSchemaManager.ContainsTable(tableName) && IsCacheableTable(tableName))
                    {
                        // 快速检查，避免重复请求
                        var entityList = EntityCacheHelper.GetEntityListByTableName(tableName);
                        if (task.ForceRefresh || entityList == null || entityList.Count == 0)
                        {
                            // 使用BackgroundCacheManager自己的缓存客户端服务
                            await _cacheClient.RequestCacheAsync(tableName, cancellationToken);
                        }
                    }

                    // 关联表处理 - 限制处理数量，避免过度加载
                    if (task.EntityType != null && typeof(BaseEntity).IsAssignableFrom(task.EntityType))
                    {
                        try
                        {
                            // 创建实体实例来获取FKRelations
                            BaseEntity entityInstance = (BaseEntity)Activator.CreateInstance(task.EntityType);
                            var fkRelations = entityInstance.FKRelations;

                            // 限制关联表数量
                            foreach (var relation in fkRelations.Take(3))
                            {
                                if (IsCacheableTable(relation.FKTableName))
                                {
                                    // 为关联表添加低优先级任务
                                    await AddLowPriorityTaskAsync(relation.FKTableName, null, task.ForceRefresh, task.TimeoutMs);
                                }
                            }
                        }
                        catch (Exception ex) when (!(ex is OperationCanceledException))
                        {
                            _logger.LogWarning(ex, $"获取实体{task.EntityType.Name}的外键关系失败");
                        }
                    }

                    status.State = CacheLoadState.Loaded;
                    status.RetryCount = 0;
                    status.ErrorMessage = null;

                    _logger.LogInformation("缓存 {TableName} 加载成功", tableName);
                    task.CompletionSource.TrySetResult(true);
                }
                catch (TaskCanceledException ex)
                {
                    // 区分超时异常和取消异常
                    if (ex.CancellationToken == cancellationToken)
                    {
                        status.State = CacheLoadState.Failed;
                        status.ErrorMessage = "任务被全局取消";
                        _logger.LogWarning("缓存 {TableName} 加载被全局取消", tableName);
                    }
                    else
                    {
                        status.State = CacheLoadState.Failed;
                        status.ErrorMessage = "任务执行超时";
                        _logger.LogWarning("缓存 {TableName} 加载超时", tableName);
                    }
                    task.CompletionSource.TrySetResult(false);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                status.State = CacheLoadState.Failed;
                status.ErrorMessage = "任务被取消";
                _logger.LogWarning("缓存 {TableName} 加载被取消", tableName);
                task.CompletionSource.TrySetResult(false);
            }
            catch (Exception ex)
            {
                status.RetryCount++;
                status.LastAttemptTime = DateTime.Now;

                if (status.RetryCount <= MaxRetries)
                {
                    // 改进重试机制：使用指数退避策略
                    int backoffDelay = (int)Math.Min(RetryDelayMs * Math.Pow(2, status.RetryCount - 1), 30000); // 最大延迟30秒
                    _logger.LogWarning(ex, "缓存 {TableName} 加载失败，{RetryCount}/{MaxRetries} 次重试，下次重试将在 {DelayMs}ms 后",
                        tableName, status.RetryCount, MaxRetries, backoffDelay);

                    // 创建一个新的任务对象，避免重用导致的潜在问题
                    var retryTask = new CacheLoadTask
                    {
                        TableName = task.TableName,
                        EntityType = task.EntityType,
                        Priority = Math.Max(1, task.Priority / 2), // 降低优先级
                        ForceRefresh = task.ForceRefresh,
                        TimeoutMs = Math.Min(task.TimeoutMs * 2, ExtendedTimeoutMs) // 增加超时时间
                    };

                    // 使用延迟入队，而不是立即入队
                    var delayedEnqueueTask = Task.Run(async () =>
                    {
                        try
                        {
                            await Task.Delay(backoffDelay, cancellationToken);
                            if (_isRunning && !cancellationToken.IsCancellationRequested)
                            {
                                _pendingTasks.Enqueue(retryTask);
                            }
                            else
                            {
                                retryTask.CompletionSource.TrySetResult(false);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            retryTask.CompletionSource.TrySetResult(false);
                        }
                        catch (Exception delayEx)
                        {
                            _logger.LogError(delayEx, "延迟入队任务时发生错误");
                            retryTask.CompletionSource.TrySetResult(false);
                        }
                    }, cancellationToken);

                    // 存储延迟任务引用，以便在Dispose时等待完成
                    lock (_delayedTasks)
                    {
                        _delayedTasks.Add(delayedEnqueueTask);
                    }

                    // 当延迟任务完成时，清理引用
                    _ = delayedEnqueueTask.ContinueWith(t =>
                    {
                        lock (_delayedTasks)
                        {
                            _delayedTasks.Remove(t);
                        }
                    }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
                }
                else
                {
                    // 达到最大重试次数，标记为失败
                    status.State = CacheLoadState.Failed;
                    status.ErrorMessage = ex.Message;

                    _logger.LogError(ex, "缓存 {TableName} 加载失败，已达到最大重试次数 {MaxRetries}", tableName, MaxRetries);
                }

                // 无论是否重试，都设置当前任务的完成状态
                task.CompletionSource.TrySetResult(false);
            }
        }

        /// <summary>
        /// 任务比较器（按优先级排序）
        /// </summary>
        private class CacheTaskComparer : IComparer<CacheLoadTask>
        {
            public int Compare(CacheLoadTask x, CacheLoadTask y)
            {
                if (x == null || y == null)
                    return 0;

                // 优先级高的排在后面（Max会取出优先级最高的）
                return x.Priority.CompareTo(y.Priority);
            }
        }

        #region IDisposable 实现
        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                // 停止运行标志
                _isRunning = false;

                // 取消所有操作
                try
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                }
                catch (ObjectDisposedException)
                { }

                // 等待延迟任务完成
                try
                {
                    List<Task> tasksToWait;
                    lock (_delayedTasks)
                    {
                        tasksToWait = _delayedTasks.ToList();
                    }

                    if (tasksToWait.Any())
                    {
                        _logger.LogInformation("等待 {Count} 个延迟任务完成", tasksToWait.Count);
                        Task.WaitAll(tasksToWait.ToArray(), TimeSpan.FromSeconds(5));
                    }
                }
                catch (TaskCanceledException)
                { }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "等待延迟任务完成时发生错误");
                }

                // 等待后台工作线程完成
                if (_backgroundWorker != null && !_backgroundWorker.IsCompleted)
                {
                    _logger.LogInformation("等待后台工作线程完成");
                    try
                    {
                        if (!_backgroundWorker.Wait(TimeSpan.FromSeconds(10)))
                        {
                            _logger.LogWarning("后台工作线程未在指定时间内完成");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "等待后台工作线程完成时发生错误");
                    }
                }

                // 释放信号量
                try
                {
                    _semaphore.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "释放信号量时发生错误");
                }

                // 完成所有待处理任务
                while (_pendingTasks.TryDequeue(out var task))
                {
                    try
                    {
                        task.CompletionSource.TrySetResult(false);
                    }
                    catch { }
                }

                // 清理缓存状态
                _cacheStatus.Clear();

                _logger.LogInformation("后台缓存管理器资源已释放");
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~BackgroundCacheManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 判断表是否可缓存
        /// </summary>
        private bool IsCacheableTable(string tableName)
        {
            // 与UIBizService.cs中的实现保持一致，使用TableSchemaManager.Instance.CacheableTableNames
            return TableSchemaManager.Instance.CacheableTableNames.Contains(tableName);
        }

        #endregion
    }
}