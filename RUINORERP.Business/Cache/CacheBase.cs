using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache
{


    /// <summary>
    /// 缓存基础实现
    /// </summary>
    public abstract class CacheBase : ICache
    {
        protected readonly ICacheManager<object> _cacheManager;
        protected readonly ILogger _logger;
        protected readonly CachePolicy _defaultPolicy;

        public CacheBase(ICacheManager<object> cacheManager, ILogger logger, CachePolicy defaultPolicy)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _defaultPolicy = defaultPolicy ?? throw new ArgumentNullException(nameof(defaultPolicy));
        }

        /// <summary>
        /// 缓存名称（用于区分不同业务缓存）
        /// </summary>
        public abstract string CacheName { get; }

        /// <summary>
        /// 生成带前缀的缓存键，避免不同业务缓存键冲突
        /// </summary>
        protected virtual string GetPrefixedKey(string key)
        {
            return $"{CacheName}:{key}";
        }

        public virtual T Get<T>(string key)
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

        public virtual async Task<T> GetAsync<T>(string key)
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

        public virtual void Set<T>(string key, T value, TimeSpan? expiration = null)
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

        public virtual async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
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

        public virtual void AddOrUpdate<T>(string key, T addValue, Func<T, T> updateValueFactory)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);

                _cacheManager.AddOrUpdate(prefixedKey, addValue, existing =>
                {
                    if (existing is T existingValue)
                    {
                        return updateValueFactory(existingValue);
                    }
                    return addValue;
                });

                _logger.LogDebug($"添加或更新缓存: {prefixedKey}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"添加或更新缓存失败: {key}");
            }
        }

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

        public virtual void RemoveByPattern(string pattern)
        {
            try
            {
                var prefixedPattern = GetPrefixedKey(pattern);
                var keysToRemove = _cacheManager.GetKeys().Where(k => k.StartsWith(prefixedPattern)).ToList();

                foreach (var key in keysToRemove)
                {
                    _cacheManager.Remove(key);
                }

                _logger.LogDebug($"按模式移除缓存: {prefixedPattern}, 共移除 {keysToRemove.Count} 项");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"按模式移除缓存失败: {pattern}");
            }
        }

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

        public virtual void Clear()
        {
            try
            {
                var prefix = $"{CacheName}:";
                var keysToRemove = _cacheManager.GetKeys().Where(k => k.StartsWith(prefix)).ToList();

                _cacheManager.Remove(keysToRemove);
                _logger.LogInformation($"清空缓存: {CacheName}, 共移除 {keysToRemove.Count} 项");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"清空缓存失败: {CacheName}");
            }
        }

        public virtual IEnumerable<string> GetKeys()
        {
            try
            {
                var prefix = $"{CacheName}:";
                return _cacheManager.GetKeys()
                    .Where(k => k.StartsWith(prefix))
                    .Select(k => k.Substring(prefix.Length))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取缓存键集合失败: {CacheName}");
                return Enumerable.Empty<string>();
            }
        }
    }

}
