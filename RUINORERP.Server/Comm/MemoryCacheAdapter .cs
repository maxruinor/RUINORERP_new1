using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
 
    public class MemoryCacheAdapter : ICacheAdapter
    {
        private readonly IMemoryCache cache;

        public MemoryCacheAdapter(IMemoryCache cache)
        {
            this.cache = cache;
        }

        // 取个固定名字
        public string Name => "memory";

        public void Set<T>(string key, T result, TimeSpan ttl)
        {
            cache.Set(key, result, ttl);
        }

        public bool TryGetValue<T>(string key, out T result)
        {
            return cache.TryGetValue(key, out result);
        }
    }

}
