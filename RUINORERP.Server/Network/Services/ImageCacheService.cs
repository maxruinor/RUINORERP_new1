using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Common.Caching;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 图片缓存服务 - 服务器端实现
    /// 提供基于内存的图片信息缓存,减少数据库查询压力
    /// 继承自ImageCacheServiceBase,使用默认的4小时滑动过期和8小时绝对过期策略
    /// </summary>
    public class ImageCacheService : ImageCacheServiceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memoryCache">内存缓存</param>
        public ImageCacheService(IMemoryCache memoryCache) 
            : base(memoryCache, slidingExpirationMinutes: 240, absoluteExpirationHours: 8)
        {
            // 服务器端可以使用默认配置,无需额外初始化
        }
    }
}
