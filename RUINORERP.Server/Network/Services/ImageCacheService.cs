using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Model.BusinessImage;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 图片缓存服务 - 服务器端实现
    /// 提供基于内存的图片信息缓存,减少数据库查询压力
    /// 继承自ImageCacheServiceBase,使用30分钟滑动过期和2小时绝对过期策略(优化内存占用)
    /// </summary>
    public class ImageCacheService : ImageCacheServiceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memoryCache">内存缓存</param>
        public ImageCacheService(IMemoryCache memoryCache) 
            : base(memoryCache, slidingExpirationMinutes: 30, absoluteExpirationHours: 2)
        {
        }
    }
}
