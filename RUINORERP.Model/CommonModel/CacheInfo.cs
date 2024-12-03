using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    /// 有关于缓存的实体类
    /// 包含所有服务器或客户端缓存信息的情况,客户端以为这些信息为基准做进一步的请求
    /// 服务器也可以对比这些。做对比更新。刷新缓存等操作。
    /// 缓存了哪些表，表中行数。等再根据这个基础数据做进一步的操作
    /// </summary>
    public class CacheInfo
    {
        //缓存类型
        // public CacheType CacheType { get; set; }

        //缓存数量
        public int CacheCount { get; set; }

        //缓存对象的名称
        public string CacheName { get; set; }


        /// <summary>
        /// 是否有缓存过期的设置
        /// </summary>
        public bool HasExpire { get; set;}

        /// <summary>
        /// 服务器上的过期时间，超过了就再次查询一下。客户端会根据这个时间做对比，
        /// 行数量不一样。或超过这个时间一分钟以上可以再次请求
        /// </summary>
        public DateTime ExpirationTime { get; set; }


        //缓存对象Key   
        //public string CacheKey { get; set; }
        public CacheInfo(string cacheName, int cacheCount)
        {
            CacheCount = cacheCount;
            CacheName = cacheName;
        }

        public CacheInfo()
        {

        }
    }


    public class LastCacheFetchInfo
    {
        public string LastTableName { get; set; }
        public DateTime LastFetchTime { get; set; }
    }

    public class CacheFetchManager
    {
        private readonly LastCacheFetchInfo _lastCacheFetchInfo = new LastCacheFetchInfo();
        private static readonly object _lockObject = new object();

        public CacheFetchManager()
        {
            // 初始化上次处理信息，可以从文件、数据库或缓存中加载
            _lastCacheFetchInfo.LastFetchTime = DateTime.MinValue;
        }

        public void UpdateLastCacheFetchInfo(string tableName)
        {
            lock (_lockObject)
            {
                _lastCacheFetchInfo.LastTableName = tableName;
                _lastCacheFetchInfo.LastFetchTime = DateTime.Now;
                // 保存到文件、数据库或缓存中
            }
        }

        public string GetNextTableName(IEnumerable<string> tableNames)
        {
            lock (_lockObject)
            {
                var currentTime = DateTime.Now;
                foreach (var tableName in tableNames)
                {
                    if (tableName != _lastCacheFetchInfo.LastTableName && (currentTime - _lastCacheFetchInfo.LastFetchTime).TotalSeconds > 60)
                    {
                        return tableName;
                    }
                }
                return null; // 没有找到可以处理的表
            }
        }
         
    }
}
