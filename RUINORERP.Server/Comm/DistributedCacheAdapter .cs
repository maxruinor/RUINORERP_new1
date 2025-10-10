using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{

    //https://blog.csdn.net/victorxqu/article/details/112133169

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CacheAttribute : Attribute
    {
        // 由于多个缓存实现，我们需要有使用顺序指定
        public int Order { get; set; }
        public string CacheKey { get; set; }
        public string Ttl { get; set; }
        public string CacheName { get; set; }
    }

    public class DistributedCacheAdapter : ICacheAdapter
    {
        private readonly IDistributedCache cache;
        private readonly string name;

        public DistributedCacheAdapter(IDistributedCache cache, string name)
        {
            this.cache = cache;
            this.name = name;
        }

        /// 这里我们就不固定名字了，大家想用 redis 就可以自己名字取redis
        public string Name => name;

        public void Set<T>(string key, T result, TimeSpan ttl)
        {
            cache.Set(key,
                JsonSerializer.SerializeToUtf8Bytes(result),  // 为了简单，我们就不在扩展更多不同序列化器了，这里就用System.Text.Json
                new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = ttl });  // 同样，为了简单，也只支持ttl缓存策略
        }

        public bool TryGetValue<T>(string key, out T result)
        {
            var data = cache.Get(key);
            if (data == null)
            {
                result = default;
                return false;
            }
            else
            {
                result = JsonSerializer.Deserialize<T>(data);
                return true;
            }
        }
    }

}
