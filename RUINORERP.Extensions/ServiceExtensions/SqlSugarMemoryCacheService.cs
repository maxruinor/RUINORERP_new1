using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace RUINORERP.Extensions
{
    /// <summary>
    /// 增强版SqlSugar缓存服务，解决键集合获取问题
    /// </summary>
    public class SqlSugarMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

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

        public SqlSugarMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add<V>(string key, V value)
        {
            var fullKey = CachePrefix + key;
            using var entry = _memoryCache.CreateEntry(fullKey);
            entry.Value = value;
            // 注册回调确保键跟踪一致性
            entry.RegisterPostEvictionCallback(RemoveCallback);
            _cacheKeys[fullKey] = 0; // 跟踪键
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
                _locks.TryRemove(cacheKey, out _);
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
        /// 缓存创建
        /// </summary>
        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            Interlocked.Increment(ref _totalRequests);
            var fullKey = CachePrefix + cacheKey;

            if (_memoryCache.TryGetValue(fullKey, out V value))
            {
                Interlocked.Increment(ref _cacheHits);
                return value;
            }

            Interlocked.Increment(ref _cacheMisses);
            
            var semaphore = _locks.GetOrAdd(fullKey, _ => new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1))).Value;
            
            try
            {
                semaphore.Wait();

                if (_memoryCache.TryGetValue(fullKey, out value))
                {
                    Interlocked.Increment(ref _cacheHits);
                    return value;
                }

                var stopwatch = Stopwatch.StartNew();
                value = create();
                stopwatch.Stop();
                Interlocked.Add(ref _totalCreateTimeMs, stopwatch.ElapsedMilliseconds);

                var options = new MemoryCacheEntryOptions();
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
                semaphore.Release();
            }
        }
        
        /// <summary>
        /// 异步版本的GetOrCreate方法
        /// </summary>
        public async Task<V> GetOrCreateAsync<V>(string cacheKey, Func<Task<V>> createAsync, int cacheDurationInSeconds = int.MaxValue)
        {
            Interlocked.Increment(ref _totalRequests);
            var fullKey = CachePrefix + cacheKey;

            if (_memoryCache.TryGetValue(fullKey, out V value))
            {
                Interlocked.Increment(ref _cacheHits);
                return value;
            }

            Interlocked.Increment(ref _cacheMisses);
            
            var semaphore = _locks.GetOrAdd(fullKey, _ => new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1))).Value;
            
            try
            {
                await semaphore.WaitAsync();

                if (_memoryCache.TryGetValue(fullKey, out value))
                {
                    Interlocked.Increment(ref _cacheHits);
                    return value;
                }

                var stopwatch = Stopwatch.StartNew();
                value = await createAsync();
                stopwatch.Stop();
                Interlocked.Add(ref _totalCreateTimeMs, stopwatch.ElapsedMilliseconds);

                var options = new MemoryCacheEntryOptions();
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
                semaphore.Release();
            }
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
        /// 缓存命中率
        /// </summary>
        public double CacheHitRatio => _totalRequests > 0 ? (double)_cacheHits / _totalRequests : 0;
        
        /// <summary>
        /// 平均创建时间（毫秒）
        /// </summary>
        public long AverageCreateTimeMs => _cacheMisses > 0 ? _totalCreateTimeMs / _cacheMisses : 0;
        
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
        /// 清理_cacheKeys中的无效键
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
        }
        
        /// <summary>
        /// 清理指定类型的缓存
        /// </summary>
        public void CleanupCacheByType<V>()
        {
            var keysToRemove = _cacheKeys
                .Where(kv => kv.Key.StartsWith(CachePrefix))
                .Select(kv => kv.Key)
                .ToList();
            
            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
                _locks.TryRemove(key, out _);
            }
        }
    }
}