using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services.Statistics
{
    /// <summary>
    /// 内存缓存实现 (生产环境建议替换为 Redis)
    /// </summary>
    public class MemoryCacheService : IReportCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, DateTime> _keyTimes;

        public MemoryCacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1000, // 最多 1000 个缓存项
                ExpirationScanFrequency = TimeSpan.FromMinutes(5) // 每 5 分钟扫描一次过期项
            });
            _keyTimes = new ConcurrentDictionary<string, DateTime>();
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (_cache.TryGetValue(key, out object cachedValue))
            {
                value = (T)cachedValue;
                return true;
            }
            value = default;
            return false;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30),
                Size = 1
            };

            await Task.Run(() =>
            {
                _cache.Set(key, value, options);
                _keyTimes[key] = DateTime.Now;
            });
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _keyTimes.TryRemove(key, out _);
        }

        public void Clear()
        {
            _cache.Dispose();
            _keyTimes.Clear();
        }
    }
}
