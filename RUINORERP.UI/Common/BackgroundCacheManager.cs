using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network.Services.Cache;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 后台缓存管理器 - 负责在后台异步加载缓存数据，避免阻塞UI线程
    /// 采用渐进式加载策略，优先加载关键数据，延迟加载次要数据
    /// </summary>
    public class BackgroundCacheManager : IDisposable
    {
        private readonly ILogger<BackgroundCacheManager> _logger;
        private readonly CacheClientService _cacheClient;
        private readonly ConcurrentDictionary<string, CacheLoadStatus> _cacheStatus;
        private readonly SemaphoreSlim _semaphore;
        private readonly CancellationTokenSource _globalCts;
        private readonly ConcurrentQueue<CacheLoadTask> _pendingTasks;
        private Task _backgroundWorker;
        private volatile bool _isRunning;

        // 配置参数
        private const int MaxConcurrentRequests = 3; // 最大并发请求数
        private const int DefaultTimeoutMs = 10000; // 默认超时时间10秒
        private const int ExtendedTimeoutMs = 30000; // 扩展超时时间30秒
        private const int RetryDelayMs = 2000; // 重试延迟2秒
        private const int MaxRetries = 2; // 最大重试次数

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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheClient = cacheClient ?? throw new ArgumentNullException(nameof(cacheClient));
            _cacheStatus = new ConcurrentDictionary<string, CacheLoadStatus>();
            _semaphore = new SemaphoreSlim(MaxConcurrentRequests, MaxConcurrentRequests);
            _globalCts = new CancellationTokenSource();
            _pendingTasks = new ConcurrentQueue<CacheLoadTask>();
            _isRunning = true;

            // 启动后台工作线程
            _backgroundWorker = Task.Run(() => BackgroundWorkerAsync(_globalCts.Token));
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
            return false;
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

            // 检查是否已经在加载或已加载
            if (!forceRefresh && _cacheStatus.TryGetValue(tableName, out var existingStatus))
            {
                if (existingStatus.State == CacheLoadState.Loaded || existingStatus.State == CacheLoadState.Loading)
                {
                    _logger.LogDebug("缓存 {TableName} 已经处于 {State} 状态，跳过请求", tableName, existingStatus.State);
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

            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 收集待处理任务
                    while (_pendingTasks.TryDequeue(out var task))
                    {
                        prioritizedTasks.Add(task);
                    }

                    if (prioritizedTasks.Count == 0)
                    {
                        await Task.Delay(100, cancellationToken);
                        continue;
                    }

                    // 处理高优先级任务
                    var currentTask = prioritizedTasks.Max;
                    prioritizedTasks.Remove(currentTask);

                    await ProcessCacheTaskAsync(currentTask, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("后台缓存管理器被取消");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "后台缓存管理器处理任务时发生错误");
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

            status.State = CacheLoadState.Loading;
            status.LastAttemptTime = DateTime.Now;

            try
            {
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    // 使用UIBizService的RequestCache方法，但设置更长的超时时间
                    await UIBizService.RequestCache(tableName, task.EntityType, task.ForceRefresh, task.TimeoutMs, cancellationToken, true);
                    
                    status.State = CacheLoadState.Loaded;
                    status.RetryCount = 0;
                    status.ErrorMessage = null;
                    
                    _logger.LogInformation("缓存 {TableName} 加载成功", tableName);
                    task.CompletionSource.TrySetResult(true);
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
                
                if (status.RetryCount <= MaxRetries)
                {
                    // 重试机制：延迟后重新加入队列
                    _logger.LogWarning(ex, "缓存 {TableName} 加载失败，{RetryCount}/{MaxRetries} 次重试", 
                        tableName, status.RetryCount, MaxRetries);
                    
                    await Task.Delay(RetryDelayMs * status.RetryCount, cancellationToken);
                    
                    // 重新加入队列，降低优先级
                    task.Priority = Math.Max(1, task.Priority / 2);
                    _pendingTasks.Enqueue(task);
                }
                else
                {
                    // 达到最大重试次数，标记为失败
                    status.State = CacheLoadState.Failed;
                    status.ErrorMessage = ex.Message;
                    
                    _logger.LogError(ex, "缓存 {TableName} 加载失败，已达到最大重试次数 {MaxRetries}", tableName, MaxRetries);
                    task.CompletionSource.TrySetResult(false);
                }
            }
        }

        /// <summary>
        /// 任务比较器（按优先级排序）
        /// </summary>
        private class CacheTaskComparer : IComparer<CacheLoadTask>
        {
            public int Compare(CacheLoadTask x, CacheLoadTask y)
            {
                if (x == null || y == null) return 0;
                
                // 优先级高的排在后面（Max会取出优先级最高的）
                return x.Priority.CompareTo(y.Priority);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _isRunning = false;
            _globalCts?.Cancel();
            _globalCts?.Dispose();
            _semaphore?.Dispose();
            _backgroundWorker?.Wait(TimeSpan.FromSeconds(5));
        }
    }
}