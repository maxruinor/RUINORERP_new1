using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存帮助类 - 提供简单易用的缓存访问方式
    /// </summary>
    public static class CacheHelper
    {
        private static ICacheManager<object> _cacheManager;
        private static ILogger _logger;
        private static CachePolicy _defaultPolicy = CachePolicy.Default;

        /// <summary>
        /// 初始化缓存帮助类
        /// </summary>
        public static void Initialize(ICacheManager<object> cacheManager, ILogger logger, CachePolicy defaultPolicy = null)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _defaultPolicy = defaultPolicy ?? CachePolicy.Default;
        }

        #region 基础缓存操作
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public static T Get<T>(string cacheName, string key)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                var value = _cacheManager.Get<T>(prefixedKey);

                if (value != null && _logger != null)
                {
                    _logger.LogDebug($"缓存命中: {prefixedKey}");
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取缓存失败: {cacheName}:{key}");
                return default;
            }
        }

        /// <summary>
        /// 异步获取缓存
        /// </summary>
        public static async Task<T> GetAsync<T>(string cacheName, string key)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                var value = await _cacheManager.GetAsync<T>(prefixedKey);

                if (value != null && _logger != null)
                {
                    _logger.LogDebug($"异步缓存命中: {prefixedKey}");
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"异步获取缓存失败: {cacheName}:{key}");
                return default;
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        public static void Set<T>(string cacheName, string key, T value, TimeSpan? expiration = null)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                var effectiveExpiration = expiration ?? _defaultPolicy.Expiration;

                _cacheManager.Add(prefixedKey, value, effectiveExpiration);
                _logger?.LogDebug($"设置缓存: {prefixedKey}, 过期时间: {effectiveExpiration}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"设置缓存失败: {cacheName}:{key}");
            }
        }

        /// <summary>
        /// 异步设置缓存
        /// </summary>
        public static async Task SetAsync<T>(string cacheName, string key, T value, TimeSpan? expiration = null)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                var effectiveExpiration = expiration ?? _defaultPolicy.Expiration;

                await _cacheManager.AddAsync(prefixedKey, value, effectiveExpiration);
                _logger?.LogDebug($"异步设置缓存: {prefixedKey}, 过期时间: {effectiveExpiration}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"异步设置缓存失败: {cacheName}:{key}");
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public static bool Remove(string cacheName, string key)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                var result = _cacheManager.Remove(prefixedKey);

                if (result && _logger != null)
                {
                    _logger.LogDebug($"移除缓存: {prefixedKey}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"移除缓存失败: {cacheName}:{key}");
                return false;
            }
        }

        /// <summary>
        /// 异步移除缓存
        /// </summary>
        public static async Task<bool> RemoveAsync(string cacheName, string key)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                var result = await _cacheManager.RemoveAsync(prefixedKey);

                if (result && _logger != null)
                {
                    _logger.LogDebug($"异步移除缓存: {prefixedKey}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"异步移除缓存失败: {cacheName}:{key}");
                return false;
            }
        }

        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        public static bool Exists(string cacheName, string key)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefixedKey = $"{cacheName}:{key}";
                return _cacheManager.Exists(prefixedKey);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"检查缓存存在性失败: {cacheName}:{key}");
                return false;
            }
        }

        /// <summary>
        /// 清空指定缓存名称下的所有缓存
        /// </summary>
        public static void ClearCache(string cacheName)
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                var prefix = $"{cacheName}:";
                var keysToRemove = _cacheManager.GetKeys().Where(k => k.StartsWith(prefix)).ToList();

                _cacheManager.Remove(keysToRemove);
                _logger?.LogInformation($"清空缓存: {cacheName}, 共移除 {keysToRemove.Count} 项");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"清空缓存失败: {cacheName}");
            }
        }

        #endregion

        #region 实体缓存操作
        /// <summary>
        /// 获取实体缓存
        /// </summary>
        public static T GetEntity<T>(string cacheName, long id)
            where T : class
        {
            return Get<T>(cacheName, $"Entity:{typeof(T).Name}:{id}");
        }

        /// <summary>
        /// 异步获取实体缓存
        /// </summary>
        public static Task<T> GetEntityAsync<T>(string cacheName, long id)
            where T : class
        {
            return GetAsync<T>(cacheName, $"Entity:{typeof(T).Name}:{id}");
        }

        /// <summary>
        /// 设置实体缓存
        /// </summary>
        public static void SetEntity<T>(string cacheName, T entity, long id, TimeSpan? expiration = null)
            where T : class
        {
            Set(cacheName, $"Entity:{typeof(T).Name}:{id}", entity, expiration);
        }

        /// <summary>
        /// 异步设置实体缓存
        /// </summary>
        public static Task SetEntityAsync<T>(string cacheName, T entity, long id, TimeSpan? expiration = null)
            where T : class
        {
            return SetAsync(cacheName, $"Entity:{typeof(T).Name}:{id}", entity, expiration);
        }

        /// <summary>
        /// 移除实体缓存
        /// </summary>
        public static bool RemoveEntity<T>(string cacheName, long id)
            where T : class
        {
            return Remove(cacheName, $"Entity:{typeof(T).Name}:{id}");
        }

        /// <summary>
        /// 异步移除实体缓存
        /// </summary>
        public static Task<bool> RemoveEntityAsync<T>(string cacheName, long id)
            where T : class
        {
            return RemoveAsync(cacheName, $"Entity:{typeof(T).Name}:{id}");
        }

        /// <summary>
        /// 获取实体列表缓存
        /// </summary>
        public static List<T> GetEntityList<T>(string cacheName)
            where T : class
        {
            return Get<List<T>>(cacheName, $"EntityList:{typeof(T).Name}") ?? new List<T>();
        }

        /// <summary>
        /// 异步获取实体列表缓存
        /// </summary>
        public static async Task<List<T>> GetEntityListAsync<T>(string cacheName)
            where T : class
        {
            var result = await GetAsync<List<T>>(cacheName, $"EntityList:{typeof(T).Name}");
            return result ?? new List<T>();
        }

        /// <summary>
        /// 设置实体列表缓存
        /// </summary>
        public static void SetEntityList<T>(string cacheName, List<T> entities, TimeSpan? expiration = null)
            where T : class
        {
            if (entities == null || !entities.Any())
                return;

            Set(cacheName, $"EntityList:{typeof(T).Name}", entities, expiration);
        }

        /// <summary>
        /// 异步设置实体列表缓存
        /// </summary>
        public static Task SetEntityListAsync<T>(string cacheName, List<T> entities, TimeSpan? expiration = null)
            where T : class
        {
            if (entities == null || !entities.Any())
                return Task.CompletedTask;

            return SetAsync(cacheName, $"EntityList:{typeof(T).Name}", entities, expiration);
        }

        #endregion

        #region 通用数据缓存操作
        /// <summary>
        /// 获取或设置缓存
        /// 如果缓存不存在，则通过factory函数获取数据并缓存
        /// </summary>
        public static T GetOrSet<T>(string cacheName, string key, Func<T> factory, TimeSpan? expiration = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var value = Get<T>(cacheName, key);
            if (value == null || (typeof(T).IsClass && object.Equals(value, Activator.CreateInstance(typeof(T)))))
            {
                // 缓存不存在，调用factory获取数据
                value = factory();
                if (value != null)
                {
                    Set(cacheName, key, value, expiration);
                }
            }
            return value;
        }

        /// <summary>
        /// 异步获取或设置缓存
        /// </summary>
        public static async Task<T> GetOrSetAsync<T>(string cacheName, string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var value = await GetAsync<T>(cacheName, key);
            if (value == null || (typeof(T).IsClass && object.Equals(value, Activator.CreateInstance(typeof(T)))))
            {
                // 缓存不存在，调用factory获取数据
                value = await factory();
                if (value != null)
                {
                    await SetAsync(cacheName, key, value, expiration);
                }
            }
            return value;
        }
        #endregion

        #region 缓存性能监控
        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public static CacheStatistics GetStatistics()
        {
            if (_cacheManager == null)
                throw new InvalidOperationException("CacheHelper 尚未初始化");

            try
            {
                return new CacheStatistics
                {
                    TotalItems = _cacheManager.Count,
                    TotalKeys = _cacheManager.GetKeys().Count()
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取缓存统计信息失败");
                return new CacheStatistics { TotalItems = -1, TotalKeys = -1 };
            }
        }

        /// <summary>
        /// 缓存统计信息类
        /// </summary>
        public class CacheStatistics
        {
            public long TotalItems { get; set; }
            public long TotalKeys { get; set; }
        }
        #endregion
    }
}