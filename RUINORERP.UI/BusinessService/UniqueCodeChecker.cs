using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.BusinessService
{
    public class UniqueCodeChecker
    {
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly MemoryCacheEntryOptions _SqlErrorCacheOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30) // 30分钟内无访问自动过期
        };

        private readonly string _connectionString;

        public UniqueCodeChecker(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 校验编号唯一性（含缓存）
        public bool IsCodeUnique(string tableName, string code)
        {
            string cacheKey = GetCacheKey(tableName, code);
            bool? exists = _cache.Get(cacheKey) as bool?;
            if (exists.HasValue) return exists.Value;

            _cache.Set(cacheKey, exists, DateTimeOffset.Now.AddMinutes(5));
            return !exists.Value; // true表示唯一，false表示已存在
        }

        // 保存数据时更新缓存
        public void SaveWithCache(string tableName, string code)
        {
            
        }

        private string GetCacheKey(string tableName, string code)
        {
            string CacheKeyPrefix = "UniqueCodeChecker:";
            return $"{CacheKeyPrefix}{tableName}:{code}";
        }
    }
}
