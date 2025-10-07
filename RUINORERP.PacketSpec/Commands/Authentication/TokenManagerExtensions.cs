using Microsoft.Extensions.DependencyInjection;
using System;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// TokenManager依赖注入扩展类 - 简化版
    /// 移除兼容层，采用纯依赖注入模式
    /// </summary>
    public static class TokenManagerExtensions
    {
        /// <summary>
        /// 添加Token管理服务到依赖注入容器 - 简化版
        /// 包括：Token存储、Token服务、Token管理器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">Token服务配置选项</param>
        /// <returns>服务集合，支持链式调用</returns>
        public static IServiceCollection AddTokenManager(this IServiceCollection services, Action<TokenServiceOptions> configureOptions = null)
        {
            // 配置Token服务选项
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            else
            {
                // 默认配置
                services.Configure<TokenServiceOptions>(options =>
                {
                    options.SecretKey = "your-default-secret-key";
                    options.DefaultExpiryHours = 8;
                    options.RefreshTokenExpiryHours = 24;
                });
            }

            // 注册Token存储服务
            services.AddSingleton<ITokenStorage, MemoryTokenStorage>();

            // 注册Token服务
            services.AddSingleton<ITokenService, JwtTokenService>();

            // 注册Token管理器
            services.AddSingleton<TokenManager>();

            return services;
        }

        /// <summary>
        /// 添加Token管理服务到依赖注入容器（自定义存储实现）- 简化版
        /// </summary>
        /// <typeparam name="TStorage">自定义Token存储实现类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">Token服务配置选项</param>
        /// <returns>服务集合，支持链式调用</returns>
        public static IServiceCollection AddTokenManager<TStorage>(this IServiceCollection services, Action<TokenServiceOptions> configureOptions = null) 
            where TStorage : class, ITokenStorage
        {
            // 配置Token服务选项
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // 注册自定义Token存储服务
            services.AddSingleton<ITokenStorage, TStorage>();

            // 注册Token服务
            services.AddSingleton<ITokenService, JwtTokenService>();

            // 注册Token管理器
            services.AddSingleton<TokenManager>();

            return services;
        }

        /// <summary>
        /// 添加Token管理服务到依赖注入容器（自定义服务和存储实现）- 简化版
        /// </summary>
        /// <typeparam name="TStorage">自定义Token存储实现类型</typeparam>
        /// <typeparam name="TService">自定义Token服务实现类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">Token服务配置选项</param>
        /// <returns>服务集合，支持链式调用</returns>
        public static IServiceCollection AddTokenManager<TStorage, TService>(this IServiceCollection services, Action<TokenServiceOptions> configureOptions = null) 
            where TStorage : class, ITokenStorage
            where TService : class, ITokenService
        {
            // 配置Token服务选项
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // 注册自定义Token存储服务
            services.AddSingleton<ITokenStorage, TStorage>();

            // 注册自定义Token服务
            services.AddSingleton<ITokenService, TService>();

            // 注册Token管理器
            services.AddSingleton<TokenManager>();

            return services;
        }
    }
}
