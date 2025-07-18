using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

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

        // 缓存分区前缀（避免与其他缓存冲突）
        private const string CachePrefix = "SqlSugarDataCache.";

        public SqlSugarMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add<V>(string key, V value)
        {
            var fullKey = CachePrefix + key;
            using var entry = _memoryCache.CreateEntry(fullKey);
            entry.Value = value;
            _cacheKeys[fullKey] = 0; // 跟踪键
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            var fullKey = CachePrefix + key;
            _memoryCache.Set(fullKey, value, DateTimeOffset.Now.AddSeconds(cacheDurationInSeconds));

            // 注册过期回调自动清理
            var options = new MemoryCacheEntryOptions()
                .RegisterPostEvictionCallback(RemoveCallback);

            _memoryCache.Set(fullKey, value, options);
            _cacheKeys[fullKey] = 0;
        }

        private void RemoveCallback(object key, object value, EvictionReason reason, object state)
        {
            if (key is string cacheKey)
            {
                _cacheKeys.TryRemove(cacheKey, out _);
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

        
        public IEnumerable<string> GetAllKey<V>()
        {
            // 直接返回主动跟踪的键集合（移除分区前缀）
            return _cacheKeys.Keys
                .Where(k => k.StartsWith(CachePrefix))
                .Select(k => k.Substring(CachePrefix.Length))
                .ToList();
        }


        /// <summary>
        ///如果键数量巨大（>10,000） 在GetAllKey中分页返回
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllKey<V>(int batchSize = 1000)
        {
            // 直接返回主动跟踪的键集合（移除分区前缀）
            return _cacheKeys.Keys
                .Where(k => k.StartsWith(CachePrefix))
                .Select(k => k.Substring(CachePrefix.Length))
                .Take(batchSize)
                .ToList();
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            var fullKey = CachePrefix + cacheKey;

            return _memoryCache.GetOrCreate(fullKey, entry =>
            {
                _cacheKeys[fullKey] = 0;

                if (cacheDurationInSeconds < int.MaxValue)
                {
                    entry.AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(cacheDurationInSeconds);
                    entry.RegisterPostEvictionCallback(RemoveCallback);
                }

                return create();
            });
        }

        public void Remove<V>(string key)
        {
            var fullKey = CachePrefix + key;
            _memoryCache.Remove(fullKey);
            _cacheKeys.TryRemove(fullKey, out _);
        }



        //添加缓存统计接口
        public int CachedItemsCount => _cacheKeys.Count;
    }
}