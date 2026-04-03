using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Extensions
{
    /// <summary>
    /// 增强版SqlSugar缓存服务，解决键集合获取问题
    /// </summary>
    public class SqlSugarMemoryCacheService : ICacheService, IDisposable
    {
        private bool _disposed = false;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SqlSugarMemoryCacheService> _logger;

        // 使用并发字典主动跟踪所有缓存键
        private readonly ConcurrentDictionary<string, byte> _cacheKeys = new();

        // 用于细粒度锁的并发字典（使用Lazy避免内存泄漏）
        private readonly ConcurrentDictionary<string, Lazy<SemaphoreSlim>> _locks = new();

        // 缓存分区前缀（避免与其他缓存冲突）
        private const string CachePrefix = "SqlSugarDataCache.";

        // 缓存统计字段
        private long _totalRequests = 0;
        private long _cacheHits = 0;
        private long _cacheMisses = 0;
        private long _totalCreateTimeMs = 0;
        private long _timeoutRequests = 0;

        // 性能监控开关
        private readonly bool _enablePerformanceMonitoring = true;
        private readonly bool _enableTiming = true;
        private readonly LogLevel _minLogLevel = LogLevel.Warning;

        // 清理任务取消令牌
        private readonly CancellationTokenSource _cleanupCts = new();
        private Task _cleanupTask = null;
        private bool _isCleanupRunning = false;

        public SqlSugarMemoryCacheService(IMemoryCache memoryCache, ILogger<SqlSugarMemoryCacheService> logger = null, bool enablePerformanceMonitoring = true, bool enableTiming = true)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _enablePerformanceMonitoring = enablePerformanceMonitoring;
            _enableTiming = enableTiming;

            // 根据日志级别设置最小日志级别
            if (logger != null)
            {
                _minLogLevel = LogLevel.Information; // 默认只记录 Information 及以上级别
            }
        }

        public void Add<V>(string key, V value)
        {
            var fullKey = CachePrefix + key;

            // 添加默认过期时间（1小时），避免永久缓存
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .RegisterPostEvictionCallback(RemoveCallback);

            _memoryCache.Set(fullKey, value, options);
            _cacheKeys[fullKey] = 0;
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            var fullKey = CachePrefix + key;

            // 修复：只调用一次Set方法，同时设置过期时间和回调
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDurationInSeconds))
                .RegisterPostEvictionCallback(RemoveCallback);

            _memoryCache.Set(fullKey, value, options);
            _cacheKeys[fullKey] = 0;
        }

        private void RemoveCallback(object key, object value, EvictionReason reason, object state)
        {
            if (key is string cacheKey)
            {
                _cacheKeys.TryRemove(cacheKey, out _);

                if (_locks.TryRemove(cacheKey, out var semaphoreLazy))
                {
                    // 只在日志级别允许时记录 Debug 日志
                    if (_logger != null && _minLogLevel <= LogLevel.Debug)
                    {
                        _logger?.LogDebug("缓存键 {CacheKey} 已过期，清理原因：{Reason}", cacheKey, reason);
                    }
                }

            }
        }

        public bool ContainsKey<V>(string key)
        {
            return _memoryCache.TryGetValue(CachePrefix + key, out _);
        }

        public V Get<V>(string key)
        {
            return _memoryCache.Get<V>(CachePrefix + key);
        }

        // 实现ICacheService接口要求的无参数版本
        public IEnumerable<string> GetAllKey<V>()
        {
            return GetAllKeyWithBatchSize<V>(int.MaxValue);
        }

        // 扩展版本，支持批量获取键
        public IEnumerable<string> GetAllKeyWithBatchSize<V>(int batchSize)
        {
            return _cacheKeys.Keys
                .Where(k => k.StartsWith(CachePrefix))
                .Take(batchSize)
                .Select(k => k.Substring(CachePrefix.Length))
                .ToArray();
        }


        /// <summary>
        /// 缓存创建方法
        /// </summary>
        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            string fullKey = CachePrefix + cacheKey;
            bool isHit = false;

            try
            {
                // 快速路径：缓存命中
                if (_memoryCache.TryGetValue(fullKey, out V value))
                {
                    isHit = true;
                    return value;
                }

                // 获取锁，使用线程安全的 Lazy 初始化
                Lazy<SemaphoreSlim> semaphoreLazy = _locks.GetOrAdd(fullKey, _ =>
                    new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1), LazyThreadSafetyMode.ExecutionAndPublication));
                SemaphoreSlim semaphore = semaphoreLazy.Value;

                bool lockTaken = false;
                try
                {
                    // 尝试获取锁，设置超时时间
                    if (!semaphore.Wait(TimeSpan.FromSeconds(5)))
                    {
                        // 超时处理，记录警告日志
                        if (_enablePerformanceMonitoring)
                        {
                            Interlocked.Increment(ref _timeoutRequests);
                        }

                        // 只在日志级别允许时记录
                        if (_logger != null && _minLogLevel <= LogLevel.Warning)
                        {
                            _logger?.LogWarning("获取缓存锁超时，直接执行创建操作。缓存键：{CacheKey}", cacheKey);
                        }

                        return ExecuteCreateWithTimer(create);
                    }

                    lockTaken = true;

                    // 双重检查缓存
                    if (_memoryCache.TryGetValue(fullKey, out value))
                    {
                        isHit = true;
                        return value;
                    }

                    // 执行创建操作，记录时间
                    value = ExecuteCreateWithTimer(create);

                    // 设置缓存
                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                    if (cacheDurationInSeconds < int.MaxValue)
                    {
                        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds);
                    }
                    options.RegisterPostEvictionCallback(RemoveCallback);

                    _memoryCache.Set(fullKey, value, options);
                    _cacheKeys[fullKey] = 0;

                    return value;
                }
                finally
                {
                    if (lockTaken)
                    {
                        try
                        {
                            semaphore.Release();
                        }
                        catch (SemaphoreFullException ex)
                        {
                            _logger?.LogError(ex, "释放缓存锁失败，可能存在锁计数错误");
                        }
                        catch { /* 忽略其他异常 */ }
                    }
                }
            }
            finally
            {
                // 统一统计
                Interlocked.Increment(ref _totalRequests);
                if (isHit)
                    Interlocked.Increment(ref _cacheHits);
                else
                    Interlocked.Increment(ref _cacheMisses);
            }
        }

        /// <summary>
        /// 执行创建操作并记录时间（带开关控制）
        /// </summary>
        /// <typeparam name="V">返回值类型</typeparam>
        /// <param name="create">创建委托</param>
        /// <returns>创建的结果</returns>
        private V ExecuteCreateWithTimer<V>(Func<V> create)
        {
            if (!_enableTiming)
            {
                // 关闭计时时直接执行
                return create();
            }

            var stopwatch = Stopwatch.StartNew();
            var value = create();
            stopwatch.Stop();
            Interlocked.Add(ref _totalCreateTimeMs, stopwatch.ElapsedMilliseconds);
            return value;
        }

        /// <summary>
        /// 异步版本的GetOrCreate方法
        /// </summary>
        public async Task<V> GetOrCreateAsync<V>(string cacheKey, Func<Task<V>> createAsync, int cacheDurationInSeconds = int.MaxValue)
        {
            string fullKey = CachePrefix + cacheKey;
            bool isHit = false;

            try
            {
                // 快速路径：缓存命中
                if (_memoryCache.TryGetValue(fullKey, out V value))
                {
                    isHit = true;
                    return value;
                }

                // 获取锁，使用线程安全的 Lazy 初始化
                Lazy<SemaphoreSlim> semaphoreLazy = _locks.GetOrAdd(fullKey, _ =>
                    new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1), LazyThreadSafetyMode.ExecutionAndPublication));
                SemaphoreSlim semaphore = semaphoreLazy.Value;

                bool lockTaken = false;
                try
                {
                    // 异步等待锁，设置超时时间
                    if (!await semaphore.WaitAsync(TimeSpan.FromSeconds(5)))
                    {
                        // 超时处理，记录警告日志
                        if (_enablePerformanceMonitoring)
                        {
                            Interlocked.Increment(ref _timeoutRequests);
                        }

                        // 只在日志级别允许时记录
                        if (_logger != null && _minLogLevel <= LogLevel.Warning)
                        {
                            _logger?.LogWarning("获取缓存锁超时，直接执行创建操作。缓存键：{CacheKey}", cacheKey);
                        }

                        return await ExecuteCreateAsyncWithTimer(createAsync);
                    }

                    lockTaken = true;

                    if (_memoryCache.TryGetValue(fullKey, out value))
                    {
                        isHit = true;
                        return value;
                    }

                    value = await ExecuteCreateAsyncWithTimer(createAsync);

                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                    if (cacheDurationInSeconds < int.MaxValue)
                    {
                        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds);
                    }
                    options.RegisterPostEvictionCallback(RemoveCallback);

                    _memoryCache.Set(fullKey, value, options);
                    _cacheKeys[fullKey] = 0;

                    return value;
                }
                finally
                {
                    if (lockTaken)
                    {
                        try
                        {
                            semaphore.Release();
                        }
                        catch (SemaphoreFullException ex)
                        {
                            _logger?.LogError(ex, "释放缓存锁失败，可能存在锁计数错误");
                        }
                        catch { /* 忽略其他异常 */ }
                    }
                }
            }
            finally
            {
                // 统一统计
                Interlocked.Increment(ref _totalRequests);
                if (isHit)
                    Interlocked.Increment(ref _cacheHits);
                else
                    Interlocked.Increment(ref _cacheMisses);
            }
        }

        /// <summary>
        /// 异步执行创建操作并记录时间（带开关控制）
        /// </summary>
        /// <typeparam name="V">返回值类型</typeparam>
        /// <param name="createAsync">创建委托</param>
        /// <returns>创建的结果</returns>
        private async Task<V> ExecuteCreateAsyncWithTimer<V>(Func<Task<V>> createAsync)
        {
            if (!_enableTiming)
            {
                // 关闭计时时直接执行
                return await createAsync();
            }

            var stopwatch = Stopwatch.StartNew();
            var value = await createAsync();
            stopwatch.Stop();
            Interlocked.Add(ref _totalCreateTimeMs, stopwatch.ElapsedMilliseconds);
            return value;
        }

        /// <summary>
        /// 带细粒度锁的异步GetOrCreate方法
        /// </summary>
        public async Task<V> GetOrCreateWithFineLockAsync<V>(string cacheKey, Func<Task<V>> createAsync, int cacheDurationInSeconds = int.MaxValue)
        {
            return await GetOrCreateAsync(cacheKey, createAsync, cacheDurationInSeconds);
        }

        public void Remove<V>(string key)
        {
            var fullKey = CachePrefix + key;
            _memoryCache.Remove(fullKey);
            _cacheKeys.TryRemove(fullKey, out _);
            _locks.TryRemove(fullKey, out _);
        }

        //添加缓存统计接口
        public int CachedItemsCount => _cacheKeys.Count;

        /// <summary>
        /// 缓存命中率（百分比，保留 2 位小数）
        /// </summary>
        public double CacheHitRatio => _enablePerformanceMonitoring && _totalRequests > 0 ? Math.Round((double)_cacheHits / _totalRequests * 100, 2) : 0;

        /// <summary>
        /// 平均创建时间（毫秒，保留 2 位小数）
        /// </summary>
        public double AverageCreateTimeMs => _enableTiming && _cacheMisses > 0 ? Math.Round((double)_totalCreateTimeMs / _cacheMisses, 2) : 0;

        /// <summary>
        /// 总请求数
        /// </summary>
        public long TotalRequests => _totalRequests;

        /// <summary>
        /// 缓存命中数
        /// </summary>
        public long CacheHits => _cacheHits;

        /// <summary>
        /// 缓存未命中数
        /// </summary>
        public long CacheMisses => _cacheMisses;

        /// <summary>
        /// 总创建时间（毫秒）
        /// </summary>
        public long TotalCreateTimeMs => _totalCreateTimeMs;

        /// <summary>
        /// 超时请求数
        /// </summary>
        public long TimeoutRequests => _timeoutRequests;

        /// <summary>
        /// 清理_cacheKeys 中的无效键
        /// </summary>
        public void CleanupStaleKeys()
        {
            var keysToRemove = _cacheKeys
                .Where(kv => !_memoryCache.TryGetValue(kv.Key, out _))
                .Select(kv => kv.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _cacheKeys.TryRemove(key, out _);
                _locks.TryRemove(key, out _);
            }

            // 定期清理时记录日志（只在 Information 级别）
            if (_logger != null && _minLogLevel <= LogLevel.Information && keysToRemove.Count > 0)
            {
                _logger?.LogInformation("清理过期缓存键，共清理 {Count} 个", keysToRemove.Count);
            }
        }

        /// <summary>
        /// 清理指定类型的缓存
        /// </summary>
        public void CleanupCacheByType<V>()
        {
            string typeFullName = typeof(V).FullName;
            string typePrefix = $"{CachePrefix}{typeFullName}.";

            var keysToRemove = _cacheKeys
                .Where(kv => kv.Key.StartsWith(typePrefix))
                .Select(kv => kv.Key)
                .ToList();

            foreach (string key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
                _locks.TryRemove(key, out _);
            }

            // 只在日志级别允许时记录
            if (_logger != null && _minLogLevel <= LogLevel.Information)
            {
                _logger?.LogInformation("清理类型 {TypeName} 的缓存，共清理 {Count} 个键", typeFullName, keysToRemove.Count);
            }
        }

        /// <summary>
        /// 缓存操作统计信息
        /// </summary>
        public class CacheStats
        {
            public long TotalRequests { get; set; }
            public long CacheHits { get; set; }
            public long CacheMisses { get; set; }
            public long TimeoutRequests { get; set; }
            public double HitRatio => TotalRequests > 0 ? Math.Round((double)CacheHits / TotalRequests * 100, 2) : 0;
            public long TotalCreateTimeMs { get; set; }
            public double AverageCreateTimeMs => CacheMisses > 0 ? Math.Round((double)TotalCreateTimeMs / CacheMisses, 2) : 0;
            public int CachedItemsCount { get; set; }

        }
        /// <summary>
        /// 获取缓存统计信息（带开关控制）
        /// </summary>
        public CacheStats GetCacheStats()
        {
            return new CacheStats
            {
                TotalRequests = _enablePerformanceMonitoring ? _totalRequests : 0,
                CacheHits = _enablePerformanceMonitoring ? _cacheHits : 0,
                CacheMisses = _enablePerformanceMonitoring ? _cacheMisses : 0,
                TimeoutRequests = _enablePerformanceMonitoring ? _timeoutRequests : 0,
                TotalCreateTimeMs = _enableTiming ? _totalCreateTimeMs : 0,
                CachedItemsCount = _cacheKeys.Count
            };

        }
        /// <summary>
        /// 定期清理过期缓存
        /// </summary>
        /// <param name="interval">清理间隔（秒）</param>
        public void StartPeriodicCleanup(int interval = 300) // 默认5分钟
        {
            if (_isCleanupRunning)
            {
                _logger?.LogWarning("定期清理任务已在运行中，无需重复启动");
                return;
            }

            _isCleanupRunning = true;
            _cleanupTask = Task.Run(async () =>
            {
                while (!_cleanupCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(interval), _cleanupCts.Token);
                        CleanupStaleKeys();
                        _logger?.LogDebug("定期清理缓存任务执行完成");
                    }
                    catch (OperationCanceledException)
                    {
                        _logger?.LogInformation("定期清理缓存任务已取消");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "定期清理缓存失败");
                    }
                }
                _isCleanupRunning = false;
            });
        }

        /// <summary>
        /// 停止定期清理任务
        /// </summary>
        public void StopPeriodicCleanup()
        {
            if (_cleanupCts != null && !_cleanupCts.IsCancellationRequested)
            {
                _cleanupCts.Cancel();
                // 只在日志级别允许时记录
                if (_logger != null && _minLogLevel <= LogLevel.Information)
                {
                    _logger?.LogInformation("正在停止定期清理缓存任务");
                }
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~SqlSugarMemoryCacheService()
        {
            Dispose(false);
        }

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
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    StopPeriodicCleanup();

                    // 等待清理任务完成，增加日志记录
                    if (_cleanupTask != null && !_cleanupTask.IsCompleted)
                    {
                        // 只在日志级别允许时记录
                        if (_logger != null && _minLogLevel <= LogLevel.Information)
                        {
                            _logger?.LogInformation("等待清理任务完成...");
                        }

                        bool completed = _cleanupTask.Wait(TimeSpan.FromSeconds(5));
                        if (!completed)
                        {
                            // 只在日志级别允许时记录警告
                            if (_logger != null && _minLogLevel <= LogLevel.Warning)
                            {
                                _logger?.LogWarning("清理任务未能在 5 秒内完成，强制退出");
                            }
                        }
                    }

                    _cleanupCts?.Dispose();

                    // 先通知所有锁不再接受新的请求
                    foreach (var lockPair in _locks)
                    {
                        try
                        {
                            // 尝试释放（如果当前线程持有锁）
                            lockPair.Value.Value?.Release();
                        }
                        catch { /* 忽略 */ }
                    }

                    // 等待一小段时间让其他线程完成操作
                    if (_logger != null && _minLogLevel <= LogLevel.Debug)
                    {
                        _logger?.LogDebug("等待 100ms 让其他线程完成操作...");
                    }
                    Thread.Sleep(100);

                    // 然后清理
                    foreach (var lockPair in _locks)
                    {
                        try
                        {
                            lockPair.Value.Value?.Dispose();
                        }
                        catch { /* 忽略释放锁时的异常 */ }
                    }
                    _locks.Clear();
                    _cacheKeys.Clear();
                }

                _disposed = true;
            }
        }
    }
}
