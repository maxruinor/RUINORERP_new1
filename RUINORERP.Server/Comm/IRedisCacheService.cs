using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// Redis缓存服务接口
    /// </summary>
    public interface IRedisCacheService
    {
        /// <summary>
        /// 缓存操作-获取或设置
        /// </summary>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry);

        /// <summary>
        /// 缓存操作-获取或设置（带分布式锁，防止缓存击穿）
        /// </summary>
        Task<T> GetOrSetWithLockAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry, TimeSpan lockExpiry = default);

        /// <summary>
        /// 删除缓存
        /// </summary>
        Task RemoveAsync(string key);

        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 批量获取缓存
        /// </summary>
        Task<IDictionary<string, T>> GetManyAsync<T>(IEnumerable<string> keys);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan expiry);

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        CacheStatistics GetStatistics();

        /// <summary>
        /// 重置统计信息
        /// </summary>
        void ResetStatistics();

        /// <summary>
        /// 设置缓存（带雪崩防护的过期时间随机化）
        /// </summary>
        Task SetWithRandomExpiryAsync<T>(string key, T value, TimeSpan baseExpiry);

        /// <summary>
        /// 获取缓存（无统计）
        /// </summary>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan expiry);

        /// <summary>
        /// 发布消息
        /// </summary>
        Task PublishAsync(string channel, string message);

        /// <summary>
        /// 订阅消息
        /// </summary>
        Task SubscribeAsync(string channel, Action<string> handler);
    }
}
