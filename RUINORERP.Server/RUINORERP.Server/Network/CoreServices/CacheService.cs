using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.CoreServices
{
    /// <summary>
    /// 缓存服务实现 - 提供分布式缓存功能
    /// 迁移自: RUINORERP.PacketSpec.Services.CacheService
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly ICacheManager<object> _cacheManager;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<CacheService> _logger;
        private readonly CacheConfiguration _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="redis">Redis连接</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="config">缓存配置</param>
        public CacheService(
            ICacheManager<object> cacheManager,
            IConnectionMultiplexer redis,
            ILogger<CacheService> logger,
            CacheConfiguration config)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存数据</returns>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("缓存键不能为空", nameof(key));

                var normalizedKey = NormalizeKey(key);
                _logger.LogDebug($"获取缓存项: {normalizedKey}");

                var cachedItem = await _cacheManager.GetAsync<T>(normalizedKey);

                if (cachedItem != null)
                {
                    _logger.LogDebug($"缓存命中: {normalizedKey}");
                    await UpdateAccessTimeAsync(normalizedKey);
                    return cachedItem;
                }

                _logger.LogDebug($"缓存未命中: {normalizedKey}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取缓存项异常: {key}");
                return null;
            }
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>设置结果</returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("缓存键不能为空", nameof(key));

                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                var normalizedKey = NormalizeKey(key);
                var expirationTime = expiration ?? _config.DefaultExpiration;

                _logger.LogDebug($"设置缓存项: {normalizedKey}, 过期时间: {expirationTime}");

                var cacheItem = new CacheItem<T>(normalizedKey, value, ExpirationMode.Absolute, expirationTime);
                var success = await _cacheManager.AddOrUpdateAsync(cacheItem, (existingValue) => value);

                if (success)
                {
                    await SetMetadataAsync(normalizedKey, new CacheMetadata
                    {
                        Key = normalizedKey,
                        CreatedAt = DateTime.UtcNow,
                        LastAccessedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.Add(expirationTime),
                        Size = EstimateSize(value),
                        Type = typeof(T).Name
                    });

                    _logger.LogDebug($"缓存项设置成功: {normalizedKey}");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置缓存项异常: {key}");
                return false;
            }
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>移除结果</returns>
        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return false;

                var normalizedKey = NormalizeKey(key);
                _logger.LogDebug($"移除缓存项: {normalizedKey}");

                var success = await _cacheManager.RemoveAsync(normalizedKey);

                if (success)
                {
                    await RemoveMetadataAsync(normalizedKey);
                    _logger.LogDebug($"缓存项移除成功: {normalizedKey}");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"移除缓存项异常: {key}");
                return false;
            }
        }

        /// <summary>
        /// 检查缓存项是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return false;

                var normalizedKey = NormalizeKey(key);
                return await _cacheManager.ExistsAsync(normalizedKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查缓存项存在异常: {key}");
                return false;
            }
        }

        /// <summary>
        /// 获取缓存项剩余生存时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>剩余生存时间</returns>
        public async Task<TimeSpan?> GetTtlAsync(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return null;

                var normalizedKey = NormalizeKey(key);
                var database = _redis.GetDatabase();
                return await database.KeyTimeToLiveAsync(normalizedKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取缓存项TTL异常: {key}");
                return null;
            }
        }

        /// <summary>
        /// 设置缓存项过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>设置结果</returns>
        public async Task<bool> ExpireAsync(string key, TimeSpan expiration)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return false;

                var normalizedKey = NormalizeKey(key);
                var database = _redis.GetDatabase();
                return await database.KeyExpireAsync(normalizedKey, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置缓存项过期时间异常: {key}");
                return false;
            }
        }

        /// <summary>
        /// 获取匹配模式的缓存键列表
        /// </summary>
        /// <param name="pattern">匹配模式</param>
        /// <returns>缓存键列表</returns>
        public async Task<List<string>> GetKeysAsync(string pattern = "*")
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: pattern).Select(k => k.ToString()).ToList();
                return keys;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取缓存键列表异常: {pattern}");
                return new List<string>();
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public async Task<CacheStatistics> GetStatisticsAsync()
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var info = await server.InfoAsync();

                return new CacheStatistics
                {
                    TotalKeys = (long)info.FirstOrDefault(i => i.Key == "keyspace")?.Value ?? 0,
                    MemoryUsed = (long)info.FirstOrDefault(i => i.Key == "used_memory")?.Value ?? 0,
                    ConnectedClients = (int)info.FirstOrDefault(i => i.Key == "connected_clients")?.Value ?? 0,
                    Uptime = TimeSpan.FromSeconds((long)info.FirstOrDefault(i => i.Key == "uptime_in_seconds")?.Value ?? 0),
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取缓存统计信息异常");
                return new CacheStatistics();
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <returns>清空结果</returns>
        public async Task<bool> ClearAllAsync()
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                await server.FlushAllDatabasesAsync();
                _logger.LogInformation("缓存已清空");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清空缓存异常");
                return false;
            }
        }

        /// <summary>
        /// 批量获取缓存项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="keys">缓存键列表</param>
        /// <returns>缓存数据字典</returns>
        public async Task<Dictionary<string, T>> GetMultipleAsync<T>(IEnumerable<string> keys) where T : class
        {
            try
            {
                if (keys == null || !keys.Any())
                    return new Dictionary<string, T>();

                var normalizedKeys = keys.Select(NormalizeKey).ToList();
                var results = new Dictionary<string, T>();

                foreach (var key in normalizedKeys)
                {
                    var value = await GetAsync<T>(key);
                    if (value != null)
                    {
                        results[key] = value;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"批量获取缓存项异常");
                return new Dictionary<string, T>();
            }
        }

        /// <summary>
        /// 批量设置缓存项
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="items">缓存项字典</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>设置结果</returns>
        public async Task<bool> SetMultipleAsync<T>(Dictionary<string, T> items, TimeSpan? expiration = null) where T : class
        {
            try
            {
                if (items == null || !items.Any())
                    return false;

                var success = true;
                var expirationTime = expiration ?? _config.DefaultExpiration;

                foreach (var item in items)
                {
                    var itemSuccess = await SetAsync(item.Key, item.Value, expirationTime);
                    if (!itemSuccess)
                    {
                        success = false;
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"批量设置缓存项异常");
                return false;
            }
        }

        #region Protected Methods

        /// <summary>
        /// 规范化缓存键
        /// </summary>
        /// <param name="key">原始键</param>
        /// <returns>规范化键</returns>
        protected virtual string NormalizeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;

            // 添加缓存前缀
            var prefix = _config.KeyPrefix ?? "ruinorerp";
            return $"{prefix}:{key.Trim().ToLowerInvariant()}";
        }

        /// <summary>
        /// 更新访问时间
        /// </summary>
        /// <param name="key">缓存键</param>
        protected virtual async Task UpdateAccessTimeAsync(string key)
        {
            try
            {
                var metadata = await GetMetadataAsync(key);
                if (metadata != null)
                {
                    metadata.LastAccessedAt = DateTime.UtcNow;
                    await SetMetadataAsync(key, metadata);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"更新缓存访问时间异常: {key}");
            }
        }

        /// <summary>
        /// 设置缓存元数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="metadata">元数据</param>
        protected virtual async Task SetMetadataAsync(string key, CacheMetadata metadata)
        {
            try
            {
                var metadataKey = $"{key}:metadata";
                await _cacheManager.PutAsync(metadataKey, metadata);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"设置缓存元数据异常: {key}");
            }
        }

        /// <summary>
        /// 获取缓存元数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>元数据</returns>
        protected virtual async Task<CacheMetadata> GetMetadataAsync(string key)
        {
            try
            {
                var metadataKey = $"{key}:metadata";
                return await _cacheManager.GetAsync<CacheMetadata>(metadataKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"获取缓存元数据异常: {key}");
                return null;
            }
        }

        /// <summary>
        /// 移除缓存元数据
        /// </summary>
        /// <param name="key">缓存键</param>
        protected virtual async Task RemoveMetadataAsync(string key)
        {
            try
            {
                var metadataKey = $"{key}:metadata";
                await _cacheManager.RemoveAsync(metadataKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"移除缓存元数据异常: {key}");
            }
        }

        /// <summary>
        /// 估算对象大小
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>估算大小</returns>
        protected virtual long EstimateSize<T>(T obj) where T : class
        {
            if (obj == null)
                return 0;

            try
            {
                // 简单估算对象大小
                return System.Text.Encoding.UTF8.GetByteCount(System.Text.Json.JsonSerializer.Serialize(obj));
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _cacheManager?.Dispose();
                _redis?.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfiguration
    {
        public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
        public string KeyPrefix { get; set; } = "ruinorerp";
        public bool EnableCompression { get; set; } = true;
        public int MaxRetryAttempts { get; set; } = 3;
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(100);
    }

    /// <summary>
    /// 缓存元数据
    /// </summary>
    public class CacheMetadata
    {
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        public long TotalKeys { get; set; }
        public long MemoryUsed { get; set; }
        public int ConnectedClients { get; set; }
        public TimeSpan Uptime { get; set; }
        public DateTime LastUpdated { get; set; }
        public int HitRate { get; set; }
        public int MissRate { get; set; }
    }
}