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
            // 优化：预先计算结果，减少枚举器的创建和LINQ操作
            var result = new List<string>();
            int count = 0;
            
            // 直接枚举键，避免Where操作
            foreach (var fullKey in _cacheKeys.Keys)
            {
                // 检查是否达到批量大小
                if (count >= batchSize)
                    break;
                
                // 检查是否是SqlSugar缓存键
                if (fullKey.StartsWith(CachePrefix))
                {
                    // 移除前缀并添加到结果
                    string keyWithoutPrefix = fullKey.Substring(CachePrefix.Length);
                    result.Add(keyWithoutPrefix);
                    count++;
                }
            }
            
            return result;
        }


        /// <summary>
        /// 缓存创建1
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="create"></param>
        /// <param name="cacheDurationInSeconds"></param>
        /// <returns></returns>
        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            var fullKey = CachePrefix + cacheKey;

            return _memoryCache.GetOrCreate(fullKey, entry =>
            {
                // 设置过期时间和回调
                if (cacheDurationInSeconds < int.MaxValue)
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds);
                }
                entry.RegisterPostEvictionCallback(RemoveCallback);

                // 先执行create函数，再添加到键跟踪，确保只有成功创建缓存项才会跟踪键
                V value = create();
                
                // 只有当create函数成功执行后，才将键添加到跟踪列表
                _cacheKeys[fullKey] = 0;
                
                return value;
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
        
        /// <summary>
        /// 清理_cacheKeys中的无效键
        /// </summary>
        public void CleanupStaleKeys()
        {
            var keysToRemove = new List<string>();
            
            // 查找所有不在实际缓存中的键
            foreach (var fullKey in _cacheKeys.Keys)
            {
                if (!_memoryCache.TryGetValue(fullKey, out _))
                {
                    keysToRemove.Add(fullKey);
                }
            }
            
            // 批量移除无效键
            foreach (var key in keysToRemove)
            {
                _cacheKeys.TryRemove(key, out _);
            }
        }
        
        /// <summary>
        /// 清理指定类型的缓存
        /// </summary>
        /// <typeparam name="V">缓存值类型</typeparam>
        public void CleanupCacheByType<V>()
        {
            var keysToRemove = new List<string>();
            
            // 查找所有SqlSugar缓存键
            foreach (var fullKey in _cacheKeys.Keys)
            {
                if (fullKey.StartsWith(CachePrefix))
                {
                    keysToRemove.Add(fullKey);
                }
            }
            
            // 批量移除指定类型的缓存
            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }
        }
    }
}