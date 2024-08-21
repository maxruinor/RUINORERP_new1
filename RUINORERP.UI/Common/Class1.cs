using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace RUINORERP.UI.Common
{
    public class CacheManager
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private static readonly ConcurrentDictionary<string, ReaderWriterLockSlim>
             Locks = new ConcurrentDictionary<string, ReaderWriterLockSlim>();
        private const int CacheLengthInHours = 1;

        public object AddOrGetExisting(string key, Func<object> factoryMethod)
        {
            Locks.GetOrAdd(key, new ReaderWriterLockSlim());

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(CacheLengthInHours)
            };
            return Cache.AddOrGetExisting
                (key, new Lazy<object>(factoryMethod), policy);
        }

        public object Get(string key)
        {
            var targetLock = AcquireLockObject(key);
            if (targetLock != null)
            {
                targetLock.EnterReadLock();

                try
                {
                    var cacheItem = Cache.GetCacheItem(key);
                    if (cacheItem != null)
                        return cacheItem.Value;
                }
                finally
                {
                    targetLock.ExitReadLock();
                }
            }

            return null;
        }

        public void Update<T>(string key, Func<T, object> updateMethod)
        {
            var targetLock = AcquireLockObject(key);
            var targetItem = (Lazy<object>)Get(key);

            if (targetLock == null || key == null) return;
            targetLock.EnterWriteLock();

            try
            {
                updateMethod((T)targetItem.Value);
            }
            finally
            {
                targetLock.ExitWriteLock();
            }
        }

        private ReaderWriterLockSlim AcquireLockObject(string key)
        {
            return Locks.ContainsKey(key) ? Locks[key] : null;
        }
    }
}
