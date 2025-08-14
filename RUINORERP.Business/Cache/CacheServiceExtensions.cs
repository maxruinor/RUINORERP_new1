using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存服务注入扩展
    /// </summary>
    public static class CacheServiceExtensions
    {
        /// <summary>
        /// 添加缓存服务
        /// </summary>
        public static IServiceCollection AddCacheServices(this IServiceCollection services, Action<CacheConfiguration> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            // 配置缓存全局设置
            var cacheConfig = new CacheConfiguration();
            configure(cacheConfig);
            services.AddSingleton(cacheConfig);

            // 注册缓存管理器工厂
            services.AddSingleton<ICacheManagerFactory, CacheManagerFactory>();

            // 注册业务缓存
            services.AddSingleton<IBaseDataCache, BaseDataCache>();
            services.AddSingleton<IInventoryCache, InventoryCache>();

            // 可以根据需要添加更多业务缓存...

            return services;
        }
    }
}
