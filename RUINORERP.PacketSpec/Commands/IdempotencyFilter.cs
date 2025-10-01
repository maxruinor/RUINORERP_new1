using System;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 幂等过滤器，用于确保相同命令的多次执行不会产生重复结果
    /// 在分布式系统和网络通信中，用于防止因网络重试等原因导致的重复处理
    /// </summary>
    public sealed class IdempotencyFilter
    {
        /// <summary>
        /// 用于存储命令响应的线程安全缓存字典
        /// 键为命令标识符，值为对应的响应对象
        /// </summary>
        private readonly ConcurrentDictionary<string, ResponseBase> _cache = new();
        
        /// <summary>
        /// 缓存项的生存时间，默认为5分钟
        /// 超过此时间的缓存项应被清理
        /// </summary>
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(5);
        
        /// <summary>
        /// 尝试从缓存中获取指定命令标识符的响应
        /// </summary>
        /// <param name="CommandIdentifier">命令唯一标识符</param>
        /// <param name="response">如果找到缓存项，则返回对应的响应对象；否则返回null</param>
        /// <returns>如果找到缓存项则返回true，否则返回false</returns>
        public bool TryGetCached(string CommandIdentifier, out ResponseBase response) =>
            _cache.TryGetValue(CommandIdentifier, out response);
            
        /// <summary>
        /// 将命令响应添加到缓存中
        /// </summary>
        /// <param name="CommandIdentifier">命令唯一标识符</param>
        /// <param name="response">要缓存的响应对象</param>
        public void Cache(string CommandIdentifier, ResponseBase response) =>
            _cache.TryAdd(CommandIdentifier, response);
            
        // 待实现功能：
        // 1. 定时清理过期缓存项
        // 2. 限制缓存大小，防止内存占用过大
        // 3. 添加缓存统计信息，如缓存命中率、缓存项数量等
        // 4. 实现线程安全的缓存管理
    }
}
