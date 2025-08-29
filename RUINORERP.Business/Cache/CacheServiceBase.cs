using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 通用缓存服务基类
    /// 实现ICacheService接口，提供基本的缓存操作功能
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    public class CacheServiceBase<T> : ICacheService<T> where T : class
    {
        protected readonly ICacheManager<object> _cacheManager;
        protected readonly ILogger _logger;
        protected readonly string _cacheName;
        protected readonly CachePolicy _defaultPolicy;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="defaultPolicy">默认缓存策略</param>
        public CacheServiceBase(ICacheManager<object> cacheManager, ILogger logger, string cacheName, CachePolicy defaultPolicy = null)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheName = cacheName ?? throw new ArgumentNullException(nameof(cacheName));
            _defaultPolicy = defaultPolicy ?? CachePolicy.Default;
        }

        /// <summary>
        /// 缓存名称
        /// </summary>
        public string CacheName => _cacheName;

        /// <summary>
        /// 生成带前缀的缓存键
        /// </summary>
        /// <param name="key">原始键</param>
        /// <returns>带前缀的键</returns>
        protected virtual string GetPrefixedKey(string key)
        {
            return $"{_cacheName}:{key}";
        }

        /// <summary>
        /// 获取单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public virtual T Get(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var value = _cacheManager.Get<T>(prefixedKey);

                if (value != null)
                {
                    _logger.LogDebug($"缓存命中: {prefixedKey}");
                }
                else
                {
                    _logger.LogDebug($"缓存未命中: {prefixedKey}");
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取缓存失败: {key}");
                return default;
            }
        }

        /// <summary>
        /// 异步获取单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public virtual async Task<T> GetAsync(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var value = await _cacheManager.GetAsync<T>(prefixedKey);

                if (value != null)
                {
                    _logger.LogDebug($"异步缓存命中: {prefixedKey}");
                }
                else
                {
                    _logger.LogDebug($"异步缓存未命中: {prefixedKey}");
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步获取缓存失败: {key}");
                return default;
            }
        }

        /// <summary>
        /// 设置单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        public virtual void Set(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var effectiveExpiration = expiration ?? _defaultPolicy.Expiration;

                _cacheManager.Add(prefixedKey, value, effectiveExpiration);
                _logger.LogDebug($"设置缓存: {prefixedKey}, 过期时间: {effectiveExpiration}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置缓存失败: {key}");
            }
        }

        /// <summary>
        /// 异步设置单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        public virtual async Task SetAsync(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var effectiveExpiration = expiration ?? _defaultPolicy.Expiration;

                await _cacheManager.AddAsync(prefixedKey, value, effectiveExpiration);
                _logger.LogDebug($"异步设置缓存: {prefixedKey}, 过期时间: {effectiveExpiration}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步设置缓存失败: {key}");
            }
        }

        /// <summary>
        /// 获取或设置缓存项
        /// 如果缓存不存在，则通过factory函数获取数据并缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>缓存值</returns>
        public virtual T GetOrSet(string key, Func<T> factory, TimeSpan? expiration = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var value = Get(key);
            if (value == null)
            {
                // 缓存不存在，调用factory获取数据
                value = factory();
                if (value != null)
                {
                    Set(key, value, expiration);
                }
            }
            return value;
        }

        /// <summary>
        /// 异步获取或设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>缓存值</returns>
        public virtual async Task<T> GetOrSetAsync(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var value = await GetAsync(key);
            if (value == null)
            {
                // 缓存不存在，调用factory获取数据
                value = await factory();
                if (value != null)
                {
                    await SetAsync(key, value, expiration);
                }
            }
            return value;
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        public virtual bool Remove(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var result = _cacheManager.Remove(prefixedKey);

                if (result)
                {
                    _logger.LogDebug($"移除缓存: {prefixedKey}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"移除缓存失败: {key}");
                return false;
            }
        }

        /// <summary>
        /// 异步移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        public virtual async Task<bool> RemoveAsync(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var result = await _cacheManager.RemoveAsync(prefixedKey);

                if (result)
                {
                    _logger.LogDebug($"异步移除缓存: {prefixedKey}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步移除缓存失败: {key}");
                return false;
            }
        }

        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存是否存在</returns>
        public virtual bool Exists(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                return _cacheManager.Exists(prefixedKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查缓存存在性失败: {key}");
                return false;
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public virtual void Clear()
        {
            try
            {
                var prefix = $"{_cacheName}:";
                var keysToRemove = _cacheManager.GetKeys().Where(k => k.StartsWith(prefix)).ToList();

                _cacheManager.Remove(keysToRemove);
                _logger.LogInformation($"清空缓存: {_cacheName}, 共移除 {keysToRemove.Count} 项");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"清空缓存失败: {_cacheName}");
            }
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns>缓存键集合</returns>
        public virtual IEnumerable<string> GetKeys()
        {
            try
            {
                var prefix = $"{_cacheName}:";
                return _cacheManager.GetKeys()
                    .Where(k => k.StartsWith(prefix))
                    .Select(k => k.Substring(prefix.Length))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取缓存键集合失败: {_cacheName}");
                return Enumerable.Empty<string>();
            }
        }
    }

    /// <summary>
    /// 实体缓存服务基类
    /// 实现IEntityCacheService接口，专门处理实体对象的缓存
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class EntityCacheServiceBase<T> : CacheServiceBase<T>, IEntityCacheService<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="defaultPolicy">默认缓存策略</param>
        public EntityCacheServiceBase(ICacheManager<object> cacheManager, ILogger logger, string cacheName, CachePolicy defaultPolicy = null)
            : base(cacheManager, logger, cacheName, defaultPolicy)
        {}

        /// <summary>
        /// 获取实体键
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体键</returns>
        protected virtual string GetEntityKey(long id)
        {
            return $"Entity:{typeof(T).Name}:{id}";
        }

        /// <summary>
        /// 获取实体列表键
        /// </summary>
        /// <returns>实体列表键</returns>
        protected virtual string GetEntityListKey()
        {
            return $"EntityList:{typeof(T).Name}";
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        public virtual T GetEntity(long id)
        {
            return Get(GetEntityKey(id));
        }

        /// <summary>
        /// 异步获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        public virtual async Task<T> GetEntityAsync(long id)
        {
            return await GetAsync(GetEntityKey(id));
        }

        /// <summary>
        /// 设置实体缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">实体ID</param>
        /// <param name="expiration">过期时间</param>
        public virtual void SetEntity(T entity, long id, TimeSpan? expiration = null)
        {
            Set(GetEntityKey(id), entity, expiration);
        }

        /// <summary>
        /// 异步设置实体缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">实体ID</param>
        /// <param name="expiration">过期时间</param>
        public virtual async Task SetEntityAsync(T entity, long id, TimeSpan? expiration = null)
        {
            await SetAsync(GetEntityKey(id), entity, expiration);
        }

        /// <summary>
        /// 移除实体缓存
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否移除成功</returns>
        public virtual bool RemoveEntity(long id)
        {
            return Remove(GetEntityKey(id));
        }

        /// <summary>
        /// 异步移除实体缓存
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否移除成功</returns>
        public virtual async Task<bool> RemoveEntityAsync(long id)
        {
            return await RemoveAsync(GetEntityKey(id));
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual List<T> GetEntityList()
        {
            return Get<List<T>>(GetEntityListKey()) ?? new List<T>();
        }

        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual async Task<List<T>> GetEntityListAsync()
        {
            var result = await GetAsync<List<T>>(GetEntityListKey());
            return result ?? new List<T>();
        }

        /// <summary>
        /// 设置实体列表缓存
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="expiration">过期时间</param>
        public virtual void SetEntityList(List<T> entities, TimeSpan? expiration = null)
        {
            if (entities == null || !entities.Any())
                return;

            Set(GetEntityListKey(), entities, expiration);
        }

        /// <summary>
        /// 异步设置实体列表缓存
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="expiration">过期时间</param>
        public virtual async Task SetEntityListAsync(List<T> entities, TimeSpan? expiration = null)
        {
            if (entities == null || !entities.Any())
                return;

            await SetAsync(GetEntityListKey(), entities, expiration);
        }

        /// <summary>
        /// 获取或设置实体列表缓存
        /// 如果缓存不存在，则通过factory函数获取数据并缓存
        /// </summary>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetOrSetEntityList(Func<List<T>> factory, TimeSpan? expiration = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return GetOrSet(GetEntityListKey(), factory, expiration) ?? new List<T>();
        }

        /// <summary>
        /// 异步获取或设置实体列表缓存
        /// </summary>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>实体列表</returns>
        public virtual async Task<List<T>> GetOrSetEntityListAsync(Func<Task<List<T>>> factory, TimeSpan? expiration = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var result = await GetOrSetAsync(GetEntityListKey(), factory, expiration);
            return result ?? new List<T>();
        }
    }
}