using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// Redis缓存服务实现
    /// 实现缓存穿透、击穿、雪崩防护
    /// </summary>
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly CacheStatistics _statistics = new CacheStatistics();
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 缓存穿透防护-空值缓存时间
        /// </summary>
        private readonly TimeSpan NullValueExpiry = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 缓存击穿防护-锁超时时间
        /// </summary>
        private readonly TimeSpan DefaultLockExpiry = TimeSpan.FromSeconds(10);

        /// <summary>
        /// 雪崩防护-时间抖动比例
        /// </summary>
        private const double JitterRatio = 0.1;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = redis.GetDatabase();
        }

        #region 基础缓存操作

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                    return default;

                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GetAsync error: {Key}", key);
                _statistics.RecordError();
                return default;
            }
        }

        /// <inheritdoc/>
        public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                await _db.StringSetAsync(key, json, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SetAsync error: {Key}", key);
                _statistics.RecordError();
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis RemoveAsync error: {Key}", key);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _db.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis ExistsAsync error: {Key}", key);
                return false;
            }
        }

        #endregion

        #region 批量操作

        /// <inheritdoc/>
        public async Task<IDictionary<string, T>> GetManyAsync<T>(IEnumerable<string> keys)
        {
            var result = new Dictionary<string, T>();
            try
            {
                foreach (var key in keys)
                {
                    var value = await GetAsync<T>(key);
                    if (value != null)
                        result[key] = value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GetManyAsync error");
            }
            return result;
        }

        /// <inheritdoc/>
        public async Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan expiry)
        {
            try
            {
                var batch = _db.CreateBatch();
                foreach (var kvp in values)
                {
                    var json = JsonConvert.SerializeObject(kvp.Value);
                    batch.StringSetAsync(kvp.Key, json, expiry);
                }
                batch.Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SetManyAsync error");
            }
        }

        #endregion

        #region 缓存穿透防护-空值缓存

        /// <inheritdoc/>
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry)
        {
            try
            {
                // 1. 尝试从缓存获取
                var cachedValue = await _db.StringGetAsync(key);
                if (!cachedValue.IsNullOrEmpty)
                {
                    // 空值也缓存，但使用较短过期时间
                    if (cachedValue == "null" || cachedValue == "NULL")
                    {
                        _logger.LogDebug("Cache hit (null value): {Key}", key);
                        _statistics.RecordHit(key);
                        return default; // 返回默认值，避免重复查询数据库
                    }
                    else
                    {
                        _logger.LogDebug("Cache hit: {Key}", key);
                        _statistics.RecordHit(key);
                        return JsonConvert.DeserializeObject<T>(cachedValue);
                    }
                }

                _logger.LogDebug("Cache miss: {Key}", key);
                _statistics.RecordMiss();

                // 2. 缓存未命中，加载数据
                var value = await loader();

                // 3. 空值保护：缓存空对象，但使用较短过期时间
                var actualExpiry = value == null ? NullValueExpiry : expiry;
                var jsonToCache = value == null ? "null" : JsonConvert.SerializeObject(value);
                await _db.StringSetAsync(key, jsonToCache, actualExpiry);

                _logger.LogDebug("Cache set: {Key}, Expiry: {Expiry}, IsNull: {IsNull}", key, actualExpiry, value == null);

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GetOrSetAsync error: {Key}", key);
                _statistics.RecordError();
                return await loader(); // 降级到直接加载
            }
        }

        #endregion

        #region 缓存击穿防护-分布式锁

        /// <inheritdoc/>
        public async Task<T> GetOrSetWithLockAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry, TimeSpan lockExpiry = default)
        {
            if (lockExpiry == default)
                lockExpiry = DefaultLockExpiry;

            try
            {
                // 1. 快速获取缓存
                var cachedValue = await _db.StringGetAsync(key);
                if (!cachedValue.IsNullOrEmpty && cachedValue != "null")
                {
                    _statistics.RecordHit(key);
                    return JsonConvert.DeserializeObject<T>(cachedValue);
                }

                _statistics.RecordMiss();

                // 2. 获取分布式锁
                var lockKey = CacheKeyGenerator.Lock(key);
                var lockAcquired = await _db.StringSetAsync(lockKey, "1", lockExpiry, When.NotExists);

                if (!lockAcquired)
                {
                    // 等待后重试获取缓存
                    await Task.Delay(50);
                    cachedValue = await _db.StringGetAsync(key);
                    if (!cachedValue.IsNullOrEmpty && cachedValue != "null")
                    {
                        _statistics.RecordHit(key);
                        return JsonConvert.DeserializeObject<T>(cachedValue);
                    }

                    // 继续等待获取锁
                    await Task.Delay(100);
                    lockAcquired = await _db.StringSetAsync(lockKey, "1", lockExpiry, When.NotExists);
                    
                    if (!lockAcquired)
                    {
                        // 获取锁失败，返回默认值（避免雪崩）
                        _logger.LogWarning("Failed to acquire lock: {LockKey}", lockKey);
                        return default;
                    }
                }

                try
                {
                    // 3. 双重检查
                    cachedValue = await _db.StringGetAsync(key);
                    if (!cachedValue.IsNullOrEmpty && cachedValue != "null")
                    {
                        _statistics.RecordHit(key);
                        return JsonConvert.DeserializeObject<T>(cachedValue);
                    }

                    // 4. 加载数据
                    var value = await loader();

                    // 5. 使用雪崩防护的过期时间
                    var actualExpiry = value == null ? NullValueExpiry : GetRandomExpiry(expiry);
                    var jsonToCache = value == null ? "null" : JsonConvert.SerializeObject(value);
                    await _db.StringSetAsync(key, jsonToCache, actualExpiry);

                    return value;
                }
                finally
                {
                    // 6. 释放锁
                    await _db.KeyDeleteAsync(lockKey);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GetOrSetWithLockAsync error: {Key}", key);
                _statistics.RecordError();
                return await loader(); // 降级到直接加载
            }
        }

        #endregion

        #region 缓存雪崩防护-过期时间随机化

        /// <inheritdoc/>
        public async Task SetWithRandomExpiryAsync<T>(string key, T value, TimeSpan baseExpiry)
        {
            try
            {
                var actualExpiry = GetRandomExpiry(baseExpiry);
                var json = JsonConvert.SerializeObject(value);
                await _db.StringSetAsync(key, json, actualExpiry);
                _logger.LogDebug("Cache set with random expiry: {Key}, BaseExpiry: {BaseExpiry}, ActualExpiry: {ActualExpiry}", 
                    key, baseExpiry, actualExpiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SetWithRandomExpiryAsync error: {Key}", key);
                _statistics.RecordError();
            }
        }

        /// <summary>
        /// 获取随机过期时间（雪崩防护）
        /// 使用Random.Shared确保线程安全
        /// </summary>
        private TimeSpan GetRandomExpiry(TimeSpan baseExpiry)
        {
            var jitter = TimeSpan.FromTicks((long)(baseExpiry.Ticks * JitterRatio * (2 * Random.Shared.NextDouble() - 1)));
            return baseExpiry + jitter;
        }

        #endregion

        #region 统计与监控

        /// <inheritdoc/>
        public CacheStatistics GetStatistics()
        {
            return _statistics;
        }

        /// <inheritdoc/>
        public void ResetStatistics()
        {
            _statistics.Reset();
        }

        #endregion

        #region 发布订阅

        /// <inheritdoc/>
        public async Task PublishAsync(string channel, string message)
        {
            try
            {
                var subscriber = _redis.GetSubscriber();
                await subscriber.PublishAsync(channel, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis PublishAsync error: {Channel}", channel);
            }
        }

        /// <inheritdoc/>
        public async Task SubscribeAsync(string channel, Action<string> handler)
        {
            try
            {
                var subscriber = _redis.GetSubscriber();
                await subscriber.SubscribeAsync(channel, (redisChannel, value) =>
                {
                    handler(value);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SubscribeAsync error: {Channel}", channel);
            }
        }

        #endregion
    }
}
