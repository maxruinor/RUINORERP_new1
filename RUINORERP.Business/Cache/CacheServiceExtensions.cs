using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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

            // 注册新的缓存服务
            RegisterNewCacheServices(services);

            // 可以根据需要添加更多业务缓存...

            return services;
        }

        /// <summary>
        /// 注册新的缓存服务
        /// </summary>
        private static void RegisterNewCacheServices(IServiceCollection services)
        {
            // 注册通用缓存服务工厂
            services.AddSingleton(typeof(ICacheService<>), typeof(CacheServiceBase<>));
            services.AddSingleton(typeof(IEntityCacheService<>), typeof(EntityCacheServiceBase<>));
        }

        /// <summary>
        /// 添加通用缓存服务
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="cacheName">缓存名称</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCacheService<T>(this IServiceCollection services, string cacheName)
            where T : class
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (string.IsNullOrEmpty(cacheName))
                throw new ArgumentNullException(nameof(cacheName));

            services.AddSingleton<ICacheService<T>>(sp =>
            {
                var factory = sp.GetRequiredService<ICacheManagerFactory>();
                var cacheManager = factory.GetCacheManager(cacheName);
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<CacheServiceBase<T>>();
                var config = sp.GetRequiredService<CacheConfiguration>();
                var policy = config.GetPolicy(cacheName);

                return new CacheServiceBase<T>(cacheManager, logger, cacheName, policy);
            });

            return services;
        }

        /// <summary>
        /// 添加实体缓存服务
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="cacheName">缓存名称</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddEntityCacheService<T>(this IServiceCollection services, string cacheName = null)
            where T : class
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var actualCacheName = cacheName ?? $"Entity:{typeof(T).Name}";

            services.AddSingleton<IEntityCacheService<T>>(sp =>
            {
                var factory = sp.GetRequiredService<ICacheManagerFactory>();
                var cacheManager = factory.GetCacheManager(actualCacheName);
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<EntityCacheServiceBase<T>>();
                var config = sp.GetRequiredService<CacheConfiguration>();
                var policy = config.GetPolicy(actualCacheName);

                return new EntityCacheServiceBase<T>(cacheManager, logger, actualCacheName, policy);
            });

            return services;
        }

        /// <summary>
        /// 初始化缓存帮助类
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public static void InitializeCacheHelper(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var factory = provider.GetRequiredService<ICacheManagerFactory>();
            var cacheManager = factory.GetCacheManager("Default");
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<CacheHelper>();
            var config = provider.GetRequiredService<CacheConfiguration>();
            var defaultPolicy = config.DefaultPolicy;

            CacheHelper.Initialize(cacheManager, logger, defaultPolicy);
        }
    }
}
