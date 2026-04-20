using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// 内存缓存适配器实现IRedisCacheService接口
    /// 用于Redis不可用时的降级处理
    /// </summary>
    public class MemoryCacheAdapterForRedis : IRedisCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheAdapterForRedis> _logger;
        private readonly CacheStatistics _statistics = new CacheStatistics();
        private readonly Random _random = new Random();

        private const double JitterRatio = 0.1;
        private readonly TimeSpan NullValueExpiry = TimeSpan.FromMinutes(5);

        public MemoryCacheAdapterForRedis(IMemoryCache cache, ILogger<MemoryCacheAdapterForRedis> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<T> GetAsync<T>(string key)
        {
            try
            {
                if (_cache.TryGetValue(key, out T value))
                {
                    return Task.FromResult(value);
                }
                return Task.FromResult(default(T));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoryCache GetAsync error: {Key}", key);
                return Task.FromResult(default(T));
            }
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expiry)
                    .SetSize(1); // ✅ 添加SetSize，确保缓存项被计入SizeLimit
                _cache.Set(key, value, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoryCache SetAsync error: {Key}", key);
            }
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoryCache RemoveAsync error: {Key}", key);
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            try
            {
                return Task.FromResult(_cache.TryGetValue(key, out _));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoryCache ExistsAsync error: {Key}", key);
                return Task.FromResult(false);
            }
        }

        public async Task<IDictionary<string, T>> GetManyAsync<T>(IEnumerable<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var value = await GetAsync<T>(key);
                if (value != null)
                    result[key] = value;
            }
            return result;
        }

        public Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan expiry)
        {
            foreach (var kvp in values)
            {
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expiry)
                    .SetSize(1); // ✅ 添加SetSize，确保缓存项被计入SizeLimit
                _cache.Set(kvp.Key, kvp.Value, options);
            }
            return Task.CompletedTask;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry)
        {
            if (_cache.TryGetValue(key, out T cachedValue))
            {
                _statistics.RecordHit(key);
                return cachedValue;
            }

            _statistics.RecordMiss();
            var value = await loader();

            var actualExpiry = value == null ? NullValueExpiry : expiry;
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(actualExpiry)
                .SetSize(1); // ✅ 添加SetSize，确保缓存项被计入SizeLimit
            _cache.Set(key, value, options);

            return value;
        }

        public async Task<T> GetOrSetWithLockAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry, TimeSpan lockExpiry = default)
        {
            // 简化的锁实现（内存缓存不支持分布式锁）
            return await GetOrSetAsync(key, loader, expiry);
        }

        public async Task SetWithRandomExpiryAsync<T>(string key, T value, TimeSpan baseExpiry)
        {
            var jitter = TimeSpan.FromTicks((long)(baseExpiry.Ticks * JitterRatio * (2 * _random.NextDouble() - 1)));
            var actualExpiry = baseExpiry + jitter;
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(actualExpiry)
                .SetSize(1); // ✅ 添加SetSize，确保缓存项被计入SizeLimit
            _cache.Set(key, value, options);
            await Task.CompletedTask;
        }

        public CacheStatistics GetStatistics()
        {
            return _statistics;
        }

        public void ResetStatistics()
        {
            _statistics.Reset();
        }

        public Task PublishAsync(string channel, string message)
        {
            // 内存缓存不支持发布订阅
            _logger.LogWarning("PublishAsync not supported in MemoryCacheAdapter");
            return Task.CompletedTask;
        }

        public Task SubscribeAsync(string channel, Action<string> handler)
        {
            // 内存缓存不支持发布订阅
            _logger.LogWarning("SubscribeAsync not supported in MemoryCacheAdapter");
            return Task.CompletedTask;
        }
    }
}
