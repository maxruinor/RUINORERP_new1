using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Base;
using RUINORERP.UI.Network.Services.Cache;
using RUINORERP.Business.Cache;
using RUINORERP.UI.Network.Services;
using RUINORERP.Model;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 后台缓存加载器 - 简化版本，负责在后台异步加载缓存数据，避免阻塞UI线程
    /// </summary>
    public class BackgroundCacheLoader : IDisposable
    {
        private readonly ILogger<BackgroundCacheLoader> _logger;
        private readonly CacheClientService _cacheClient;
        private readonly SemaphoreSlim _semaphore;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _loadingTasks;
        private readonly IEntityCacheManager _entityCacheManager;
        private Task _backgroundWorker;
        private volatile bool _disposed = false;

        // 任务优先级枚举
        private enum TaskPriority
        {
            Low = 0,
            Normal = 1,
            High = 2
        }

        // 缓存任务结构
        private class CacheTask
        {
            public TaskPriority Priority { get; set; }
            public string TableName { get; set; }
            public Type EntityType { get; set; }
            public bool ForceRefresh { get; set; }
            public int TimeoutMs { get; set; }
            public TaskCompletionSource<bool> CompletionSource { get; set; }
            public DateTime CreatedTime { get; set; }
        }

        // 配置参数
        private const int MaxConcurrentRequests = 3;
        private const int DefaultTimeoutMs = 10000;
        private const int MaxRetries = 2;
        
        // 任务队列 - 按优先级排序
        private readonly List<CacheTask> _taskQueue = new List<CacheTask>();
        // 用于保护任务队列的锁
        private readonly object _queueLock = new object();
        // 用于通知工作线程有新任务的信号量
        private readonly SemaphoreSlim _queueSemaphore = new SemaphoreSlim(0, int.MaxValue);
        // Task对象池，用于复用TaskCompletionSource，减少GC压力
        private readonly TaskPool<bool> _taskPool;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="cacheClient">缓存客户端服务</param>
        public BackgroundCacheLoader(ILogger<BackgroundCacheLoader> logger, CacheClientService cacheClient, IEntityCacheManager entityCacheManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheClient = cacheClient ?? throw new ArgumentNullException(nameof(cacheClient));
            _semaphore = new SemaphoreSlim(MaxConcurrentRequests, MaxConcurrentRequests);
            _cancellationTokenSource = new CancellationTokenSource();
            _entityCacheManager = entityCacheManager ?? throw new ArgumentNullException(nameof(entityCacheManager));
            _loadingTasks = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();
            // 初始化Task对象池，初始大小20，最大100
            _taskPool = new TaskPool<bool>(20, 100, true);

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
        /// <returns>加载任务</returns>
        public Task<bool> AddHighPriorityTaskAsync(string tableName, Type entityType = null, bool forceRefresh = false, int timeoutMs = DefaultTimeoutMs)
        {
            return AddTaskAsync(tableName, entityType, forceRefresh, timeoutMs, TaskPriority.High);
        }

        /// <summary>
        /// 添加缓存加载任务（普通优先级）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        /// <returns>加载任务</returns>
        public Task<bool> AddNormalPriorityTaskAsync(string tableName, Type entityType = null, bool forceRefresh = false, int timeoutMs = DefaultTimeoutMs)
        {
            return AddTaskAsync(tableName, entityType, forceRefresh, timeoutMs, TaskPriority.Normal);
        }

        /// <summary>
        /// 添加缓存加载任务（低优先级）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        /// <returns>加载任务</returns>
        public Task<bool> AddLowPriorityTaskAsync(string tableName, Type entityType = null, bool forceRefresh = false, int timeoutMs = DefaultTimeoutMs)
        {
            return AddTaskAsync(tableName, entityType, forceRefresh, timeoutMs, TaskPriority.Low);
        }

        /// <summary>
        /// 批量添加缓存加载任务
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <returns>批量加载结果</returns>
        public async Task<Dictionary<string, bool>> AddBatchTasksAsync(IEnumerable<string> tableNames, bool forceRefresh = false)
        {
            var tasks = new Dictionary<string, Task<bool>>();
            foreach (var tableName in tableNames)
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    tasks[tableName] = AddTaskAsync(tableName, null, forceRefresh, DefaultTimeoutMs, TaskPriority.Normal);
                }
            }

            var results = new Dictionary<string, bool>();
            foreach (var kvp in tasks)
            {
                try
                {
                    results[kvp.Key] = await kvp.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "批量加载缓存表 {TableName} 失败", kvp.Key);
                    results[kvp.Key] = false;
                }
            }
            return results;
        }

        /// <summary>
        /// 等待缓存加载完成
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="timeoutMs">等待超时时间</param>
        /// <returns>是否加载成功</returns>
        public async Task<bool> WaitForCacheAsync(string tableName, int timeoutMs = 30000)
        {
            if (_loadingTasks.TryGetValue(tableName, out var loadingTask))
            {
                try
                {
                    using var cts = new CancellationTokenSource(timeoutMs);
                    return await loadingTask.Task.WaitAsync(cts.Token);
                }
                catch (TimeoutException)
                {
                    _logger.LogWarning("等待缓存 {TableName} 加载超时", tableName);
                    return false;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("等待缓存 {TableName} 被取消", tableName);
                    return false;
                }
            }

            // 如果没有正在加载的任务，检查缓存是否已存在
            return IsCacheLoaded(tableName);
        }

        /// <summary>
        /// 内部添加任务方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        /// <returns>加载任务</returns>
        private Task<bool> AddTaskAsync(string tableName, Type entityType, bool forceRefresh, int timeoutMs, TaskPriority priority)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("表名不能为空", nameof(tableName));
            }

            // 检查是否已在加载中
            var existingTask = _loadingTasks.GetOrAdd(tableName, _ => _taskPool.Get());

            // 如果不是强制刷新且缓存已存在，直接返回成功
            if (!forceRefresh && IsCacheLoaded(tableName))
            {
                existingTask.TrySetResult(true);
                return existingTask.Task;
            }

            // 创建缓存任务
            var cacheTask = new CacheTask
            {
                Priority = priority,
                TableName = tableName,
                EntityType = entityType,
                ForceRefresh = forceRefresh,
                TimeoutMs = timeoutMs,
                CompletionSource = existingTask,
                CreatedTime = DateTime.Now
            };

            // 添加到任务队列（按优先级排序）
            lock (_queueLock)
            {
                // 查找合适的位置插入，按优先级从高到低排序
                int index = _taskQueue.FindIndex(t => t.Priority < priority);
                if (index == -1)
                {
                    _taskQueue.Add(cacheTask);
                }
                else
                {
                    _taskQueue.Insert(index, cacheTask);
                }
            }

            // 通知工作线程有新任务
            _queueSemaphore.Release();

            return existingTask.Task;
        }

        /// <summary>
        /// 简化的后台工作线程 - 仅用于监控和维护
        /// </summary>
        private async Task BackgroundWorkerAsync(CancellationToken cancellationToken)
        {
            try
            {
                var cleanupInterval = TimeSpan.FromSeconds(10);
                var lastCleanupTime = DateTime.Now;

                while (!cancellationToken.IsCancellationRequested && !_disposed)
                {
                    // 检查是否需要清理已完成的任务
                    if (DateTime.Now - lastCleanupTime > cleanupInterval)
                    {
                        CleanupCompletedTasks();
                        lastCleanupTime = DateTime.Now;
                    }

                    // 尝试从队列获取任务
                    CacheTask cacheTask = null;
                    bool hasTask = false;

                    // 从队列中取出一个任务（按优先级）
                    lock (_queueLock)
                    {
                        if (_taskQueue.Count > 0)
                        {
                            cacheTask = _taskQueue[0];
                            _taskQueue.RemoveAt(0);
                            hasTask = true;
                        }
                    }

                    if (hasTask && cacheTask != null)
                    {
                        // 执行缓存加载任务
                        await LoadCacheAsync(cacheTask.TableName, cacheTask.EntityType, cacheTask.ForceRefresh, cacheTask.TimeoutMs, cacheTask.CompletionSource);
                    }
                    else
                    {
                        // 没有任务时，等待新任务或超时
                        try
                        {
                            await _queueSemaphore.WaitAsync(cleanupInterval, cancellationToken);
                        }
                        catch (OperationCanceledException)
                        {
                            // 取消操作，退出循环
                            break;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("后台缓存加载器工作线程被取消");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "后台缓存加载器工作线程发生异常");
            }

            _logger.LogDebug("后台缓存加载器工作线程已停止");
        }

        /// <summary>
        /// 加载缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="timeoutMs">超时时间</param>
        /// <param name="completionSource">任务完成源</param>
        private async Task LoadCacheAsync(string tableName, Type entityType, bool forceRefresh, int timeoutMs, TaskCompletionSource<bool> completionSource)
        {
            if (_disposed)
            {
                completionSource.TrySetResult(false);
                return;
            }

            int retryCount = 0;

            while (retryCount <= MaxRetries && !_disposed)
            {
                try
                {
                    await _semaphore.WaitAsync(_cancellationTokenSource.Token);
                    try
                    {
                        // 检查是否可缓存
                        if (!IsCacheableTable(tableName))
                        {
                            _logger.LogDebug("表 {TableName} 不支持缓存", tableName);
                            completionSource.TrySetResult(false);
                            return;
                        }

                        // 快速检查，避免重复请求
                        if (!forceRefresh && IsCacheLoaded(tableName))
                        {
                            _logger.LogDebug("缓存 {TableName} 已存在，跳过加载", tableName);
                            completionSource.TrySetResult(true);
                            return;
                        }

                        // 调用缓存服务加载
                        await _cacheClient.RequestCacheAsync(tableName, _cancellationTokenSource.Token);

                        // 异步处理关联表（不阻塞主流程）
                        if (entityType != null && typeof(BaseEntity).IsAssignableFrom(entityType))
                        {
                            _ = Task.Run(() => LoadRelatedTablesAsync(entityType, forceRefresh), _cancellationTokenSource.Token);
                        }

                        _logger.LogDebug("缓存 {TableName} 加载成功", tableName);
                        completionSource.TrySetResult(true);
                        return;
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                catch (OperationCanceledException) when (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _logger.LogWarning("缓存 {TableName} 加载被取消", tableName);
                    completionSource.TrySetResult(false);
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    _logger.LogWarning(ex, "缓存 {TableName} 加载失败，第 {RetryCount} 次重试", tableName, retryCount);

                    if (retryCount > MaxRetries)
                    {
                        _logger.LogError(ex, "缓存 {TableName} 加载失败，已达到最大重试次数", tableName);
                        completionSource.TrySetResult(false);
                        return;
                    }

                    // 简单的重试延迟
                    await Task.Delay(TimeSpan.FromSeconds(2 * retryCount), _cancellationTokenSource.Token);
                }
            }
        }

        /// <summary>
        /// 异步加载关联表（低优先级）
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        private async Task LoadRelatedTablesAsync(Type entityType, bool forceRefresh)
        {
            try
            {
                if (!typeof(BaseEntity).IsAssignableFrom(entityType))
                    return;

                var entityInstance = (BaseEntity)Activator.CreateInstance(entityType);
                var fkRelations = entityInstance.FKRelations;

                // 限制关联表数量，避免过度加载
                foreach (var relation in fkRelations.Take(3))
                {
                    if (IsCacheableTable(relation.FKTableName))
                    {
                        await AddLowPriorityTaskAsync(relation.FKTableName, null, forceRefresh);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "加载实体 {EntityType} 的关联表失败", entityType.Name);
            }
        }

        /// <summary>
        /// 清理已完成的任务引用
        /// </summary>
        private void CleanupCompletedTasks()
        {
            try
            {
                var completedTasks = _loadingTasks
                    .Where(kvp => kvp.Value.Task.IsCompleted)
                    .ToList();

                foreach (var kvp in completedTasks)
                {
                    if (_loadingTasks.TryRemove(kvp.Key, out var tcs))
                    {
                        // 将TaskCompletionSource归还到对象池
                        _taskPool.Return(tcs);
                        _logger.LogDebug("清理已完成的缓存任务: {TableName}", kvp.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理已完成任务时发生错误");
            }
        }

        /// <summary>
        /// 检查缓存是否已加载1
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否已加载</returns>
        private bool IsCacheLoaded(string tableName)
        {
            try
            {
                var entityList = _entityCacheManager.GetEntityListByTableName(tableName);
                return entityList != null && entityList.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断表是否可缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否可缓存</returns>
        private bool IsCacheableTable(string tableName)
        {
            try
            {
                var tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();
                return tableSchemaManager.CacheableTableNames.Contains(tableName);
            }
            catch
            {
                return false;
            }
        }

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                _logger.LogDebug("开始释放后台缓存加载器资源");

                // 取消所有操作
                _cancellationTokenSource.Cancel();

                // 等待后台工作线程完成
                if (_backgroundWorker != null && !_backgroundWorker.IsCompleted)
                {
                    try
                    {
                        if (!_backgroundWorker.Wait(TimeSpan.FromSeconds(5)))
                        {
                            _logger.LogWarning("后台工作线程未在指定时间内完成");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "等待后台工作线程完成时发生错误");
                    }
                }

                // 完成所有待处理任务并归还到对象池
                foreach (var kvp in _loadingTasks)
                {
                    try
                    {
                        kvp.Value.TrySetResult(false);
                        // 将TaskCompletionSource归还到对象池
                        _taskPool.Return(kvp.Value);
                    }
                    catch
                    {
                        // 忽略异常
                    }
                }
                _loadingTasks.Clear();

                // 释放资源
                _semaphore?.Dispose();
                _cancellationTokenSource?.Dispose();
                _taskPool?.Dispose();

                _logger.LogDebug("后台缓存加载器资源释放完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放后台缓存加载器资源时发生错误");
            }
        }

        #endregion
    }
}