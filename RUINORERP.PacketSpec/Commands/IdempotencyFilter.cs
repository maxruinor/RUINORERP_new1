using System;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands
{
    public sealed class IdempotencyFilter
    {
        private readonly ConcurrentDictionary<string, ResponseBase> _cache = new();
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(5);
        
        public bool TryGetCached(string requestId, out ResponseBase response) =>
            _cache.TryGetValue(requestId, out response);
            
        public void Cache(string requestId, ResponseBase response) =>
            _cache.TryAdd(requestId, response);
            
        // 定时清理略
        //定时清理过期缓存
        //限制缓存大小
        //添加缓存统计信息
    }
}