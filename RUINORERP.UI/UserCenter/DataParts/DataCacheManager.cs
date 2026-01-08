using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Global;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 数据缓存管理器，用于减少重复的数据库查询
    /// </summary>
    public class DataCacheManager : IExcludeFromRegistration
    {
        private IMemoryCache _memoryCache;
        private static DataCacheManager _instance;
        private static readonly object _lock = new object();

        public static DataCacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataCacheManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private DataCacheManager()
        {
            var cacheOptions = new MemoryCacheOptions
            {
                SizeLimit = 1000 // 设置缓存大小限制
            };
            _memoryCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 异步获取缓存数据，如果不存在则执行查询函数并将结果缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="queryFunc">查询函数</param>
        /// <param name="cacheDuration">缓存持续时间（分钟）</param>
        /// <returns>缓存或查询结果</returns>
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> queryFunc, int cacheDuration = 10)
        {
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var result = await queryFunc();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDuration),
                Size = 1
            };

            _memoryCache.Set(key, result, cacheOptions);
            return result;
        }

        /// <summary>
        /// 同步获取缓存数据，如果不存在则执行查询函数并将结果缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="queryFunc">查询函数</param>
        /// <param name="cacheDuration">缓存持续时间（分钟）</param>
        /// <returns>缓存或查询结果</returns>
        public T GetOrSet<T>(string key, Func<T> queryFunc, int cacheDuration = 10)
        {
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var result = queryFunc();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDuration),
                Size = 1
            };

            _memoryCache.Set(key, result, cacheOptions);
            return result;
        }

        /// <summary>
        /// 清除指定键的缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAll()
        {
            // 重新创建缓存实例以清除所有缓存项
            var cacheOptions = new MemoryCacheOptions
            {
                SizeLimit = 1000 // 保持相同的缓存大小限制
            };
            _memoryCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 检查缓存中是否存在指定键
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        public bool ContainsKey(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }
    }
}