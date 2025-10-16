using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 缓存信息实体类
    /// 包含服务器或客户端缓存信息，作为进一步请求的基准
    /// 服务器可以对比这些信息进行更新、刷新缓存等操作
    /// </summary>
    [Serializable]
    [Obsolete]
    public class CacheInfo
    {
        /// <summary>
        /// 缓存数量
        /// </summary>
        public int CacheCount { get; set; }

        /// <summary>
        /// 缓存对象的名称
        /// </summary>
        public string CacheName { get; set; } = string.Empty;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 数据数量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 内存大小
        /// </summary>
        public long MemorySize { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否有缓存过期的设置
        /// </summary>
        public bool HasExpire { get; set; } = false;

        /// <summary>
        /// 服务器上的过期时间，超过了就再次查询
        /// 客户端会根据这个时间做对比，行数量不一样或超过这个时间一分钟以上可以再次请求
        /// </summary>
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheInfo()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="cacheCount">缓存数量</param>
        public CacheInfo(string cacheName, int cacheCount)
        {
            if (string.IsNullOrEmpty(cacheName))
                throw new ArgumentNullException(nameof(cacheName), "缓存名称不能为空");
            
            if (cacheCount < 0)
                throw new ArgumentOutOfRangeException(nameof(cacheCount), "缓存数量不能为负数");

            CacheName = cacheName;
            CacheCount = cacheCount;
        }

        /// <summary>
        /// 创建缓存信息实例
        /// </summary>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="cacheCount">缓存数量</param>
        /// <returns>缓存信息实例</returns>
        public static CacheInfo Create(string cacheName, int cacheCount)
        {
            return new CacheInfo(cacheName, cacheCount);
        }

        /// <summary>
        /// 创建带过期时间的缓存信息实例
        /// </summary>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="cacheCount">缓存数量</param>
        /// <param name="expirationTime">过期时间</param>
        /// <returns>带过期时间的缓存信息实例</returns>
        public static CacheInfo CreateWithExpiration(string cacheName, int cacheCount, DateTime expirationTime)
        {
            var cacheInfo = new CacheInfo(cacheName, cacheCount)
            {
                HasExpire = true,
                ExpirationTime = expirationTime
            };
            return cacheInfo;
        }
    }

    /// <summary>
    /// 最后缓存获取信息
    /// </summary>
    [Serializable]
    [Obsolete]
    public class LastCacheFetchInfo
    {
        /// <summary>
        /// 最后处理的表名
        /// </summary>
        public string LastTableName { get; set; } = string.Empty;

        /// <summary>
        /// 最后获取时间
        /// </summary>
        public DateTime LastFetchTime { get; set; } = DateTime.MinValue;
    }

    /// <summary>
    /// 缓存获取管理器
    /// 负责管理缓存获取的顺序和时间
    /// </summary>
    [Obsolete]
    public class CacheFetchManager
    {
        private readonly LastCacheFetchInfo _lastCacheFetchInfo = new LastCacheFetchInfo();
        private static readonly object _lockObject = new object();
        private const int DEFAULT_FETCH_INTERVAL_SECONDS = 60;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheFetchManager()
        {
            // 初始化上次处理信息
            _lastCacheFetchInfo.LastFetchTime = DateTime.MinValue;
        }

        /// <summary>
        /// 更新最后缓存获取信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <exception cref="ArgumentNullException">当表名为空时抛出</exception>
        public void UpdateLastCacheFetchInfo(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");

            lock (_lockObject)
            {
                _lastCacheFetchInfo.LastTableName = tableName;
                _lastCacheFetchInfo.LastFetchTime = DateTime.Now;
                // 保存到文件、数据库或缓存中
            }
        }

        /// <summary>
        /// 获取下一个可以处理的表名
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <returns>下一个可以处理的表名，没有则返回null</returns>
        /// <exception cref="ArgumentNullException">当表名列表为空时抛出</exception>
        public string GetNextTableName(IEnumerable<string> tableNames)
        {
            if (tableNames == null)
                throw new ArgumentNullException(nameof(tableNames), "表名列表不能为空");

            lock (_lockObject)
            {
                var currentTime = DateTime.Now;
                var eligibleTables = tableNames
                    .Where(tn => !string.IsNullOrEmpty(tn))
                    .Where(tn => tn != _lastCacheFetchInfo.LastTableName || 
                                (currentTime - _lastCacheFetchInfo.LastFetchTime).TotalSeconds > DEFAULT_FETCH_INTERVAL_SECONDS);
                
                return eligibleTables.FirstOrDefault();
            }
        }

        /// <summary>
        /// 检查指定表是否可以获取缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否可以获取缓存</returns>
        public bool CanFetchCache(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;

            lock (_lockObject)
            {
                var currentTime = DateTime.Now;
                return tableName != _lastCacheFetchInfo.LastTableName || 
                       (currentTime - _lastCacheFetchInfo.LastFetchTime).TotalSeconds > DEFAULT_FETCH_INTERVAL_SECONDS;
            }
        }
    }
}
