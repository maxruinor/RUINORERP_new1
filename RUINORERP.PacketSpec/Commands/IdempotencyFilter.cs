using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 幂等过滤器，用于确保相同命令的多次执行不会产生重复结果
    /// 在分布式系统和网络通信中，用于防止因网络重试等原因导致的重复处理
    /// 
    /// 改进说明：
    /// 1. 支持基于请求参数的幂等性判断，而不仅仅是命令类型
    /// 2. 使用请求参数的哈希值作为缓存键的一部分
    /// 3. 支持可配置的幂等键生成策略
    /// 4. 添加缓存项过期清理机制
    /// </summary>
    public sealed class IdempotencyFilter
    {
        /// <summary>
        /// 缓存项包装类，包含响应数据和过期时间
        /// </summary>
        private class CacheItem
        {
            public IResponse Response { get; set; }
            public DateTime ExpireTime { get; set; }
            public DateTime CreateTime { get; set; }
        }

        /// <summary>
        /// 用于存储命令响应的线程安全缓存字典
        /// 键为基于命令标识符和请求参数生成的唯一键，值为缓存项包装对象
        /// </summary>
        private readonly ConcurrentDictionary<string, CacheItem> _cache = new();

        /// <summary>
        /// 缓存项的生存时间，默认为5分钟
        /// 超过此时间的缓存项应被清理
        /// </summary>
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 最大缓存项数量，防止内存占用过大
        /// </summary>
        private const int MaxCacheSize = 10000;

        /// <summary>
        /// 上次清理时间
        /// </summary>
        private DateTime _lastCleanupTime = DateTime.Now;

        /// <summary>
        /// 清理间隔时间
        /// </summary>
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(1);

        /// <summary>
        /// 尝试从缓存中获取指定命令的响应
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="response">如果找到缓存项，则返回对应的响应对象；否则返回null</param>
        /// <returns>如果找到缓存项则返回true，否则返回false</returns>
        public bool TryGetCached(IRequest request, CommandId commandId, out IResponse response)
        {
            response = null;

            if (request == null)
                return false;

            var cacheKey = GenerateCacheKey(request, commandId);
            if (string.IsNullOrEmpty(cacheKey))
                return false;

            if (_cache.TryGetValue(cacheKey, out var cacheItem))
            {
                // 检查是否过期
                if (cacheItem.ExpireTime > DateTime.Now)
                {
                    response = cacheItem.Response;
                    return true;
                }
                else
                {
                    // 过期项，异步清理
                    _cache.TryRemove(cacheKey, out _);
                }
            }

            return false;
        }

        /// <summary>
        /// 将命令响应添加到缓存中
        /// </summary>
        /// <param name="packet">数据包对象</param>
        /// <param name="response">要缓存的响应对象</param>
        public void Cache(PacketModel packet, IResponse response)
        {
            if (packet == null || response == null) 
                return;

            var cacheKey = GenerateCacheKey(packet.Request,packet.CommandId);
            if (string.IsNullOrEmpty(cacheKey))
                return;

            // 检查是否需要清理过期缓存
            TryCleanupExpiredItems();

            // 检查缓存大小限制
            if (_cache.Count >= MaxCacheSize)
            {
                // 移除最旧的10%缓存项
                var itemsToRemove = _cache.OrderBy(kvp => kvp.Value.CreateTime)
                    .Take(MaxCacheSize / 10)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in itemsToRemove)
                {
                    _cache.TryRemove(key, out _);
                }
            }

            var now = DateTime.Now;
            var cacheItem = new CacheItem
            {
                Response = response,
                CreateTime = now,
                ExpireTime = now.Add(_ttl)
            };

            _cache.AddOrUpdate(cacheKey, cacheItem, (key, existing) => cacheItem);
        }

        /// <summary>
        /// 生成缓存键
        /// 基于命令标识符和请求参数生成唯一键
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>缓存键，如果无法生成则返回null</returns>
        private string GenerateCacheKey(IRequest request, CommandId commandId)
        {

            try
            {
                // 基础键：命令标识符
                var baseKey = commandId.ToString();

                // 获取请求数据用于生成参数哈希
                var parameterHash = ComputeRequestHash(request);

                // 组合成最终缓存键
                return $"{baseKey}:{parameterHash}";
            }
            catch (Exception ex)
            {
                // 如果生成失败，记录日志并返回基础键
                return commandId.ToString();
            }
        }



        /// <summary>
        /// 计算请求数据的哈希值
        /// </summary>
        /// <param name="data">请求数据</param>
        /// <returns>哈希值字符串</returns>
        private string ComputeRequestHash(object data)
        {
            if (data == null)
                return "null";

            try
            {
                // 使用MessagePack序列化数据
                var serializedData = JsonCompressionSerializationService.Serialize(data);

                // 计算SHA256哈希
                using (var sha256 = SHA256.Create())
                {
                    var hashBytes = sha256.ComputeHash(serializedData);
                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch
            {
                // 如果序列化失败，使用对象的ToString
                var dataString = data.ToString();
                using (var sha256 = SHA256.Create())
                {
                    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataString));
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        /// <summary>
        /// 尝试清理过期缓存项
        /// </summary>
        private void TryCleanupExpiredItems()
        {
            var now = DateTime.Now;

            // 检查是否需要清理
            if ((now - _lastCleanupTime) < _cleanupInterval)
                return;

            _lastCleanupTime = now;

            try
            {
                var expiredKeys = _cache.Where(kvp => kvp.Value.ExpireTime <= now)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    _cache.TryRemove(key, out _);
                }
            }
            catch
            {
                // 清理失败时不影响主流程
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public IdempotencyCacheStatistics GetStatistics()
        {
            var now = DateTime.Now;
            var items = _cache.ToArray();

            return new IdempotencyCacheStatistics
            {
                TotalItems = items.Length,
                ExpiredItems = items.Count(kvp => kvp.Value.ExpireTime <= now),
                ActiveItems = items.Count(kvp => kvp.Value.ExpireTime > now),
                OldestItemTime = items.Any() ? items.Min(kvp => kvp.Value.CreateTime) : DateTime.MinValue,
                NewestItemTime = items.Any() ? items.Max(kvp => kvp.Value.CreateTime) : DateTime.MinValue
            };
        }

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }
    }

    /// <summary>
    /// 幂等缓存统计信息
    /// </summary>
    public class IdempotencyCacheStatistics
    {
        public int TotalItems { get; set; }
        public int ExpiredItems { get; set; }
        public int ActiveItems { get; set; }
        public DateTime OldestItemTime { get; set; }
        public DateTime NewestItemTime { get; set; }
    }
}
