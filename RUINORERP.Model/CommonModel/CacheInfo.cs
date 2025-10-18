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

    

   
}
