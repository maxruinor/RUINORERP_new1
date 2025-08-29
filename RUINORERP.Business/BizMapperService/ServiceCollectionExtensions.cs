using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Global;
using System;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 服务集合扩展类 - 提供实体信息服务的注册扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册实体信息服务及其依赖项
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合，支持链式调用</returns>
        public static IServiceCollection AddEntityInfoServices(this IServiceCollection services)
        {
            // 注册实体信息服务的实现类为单例
            services.AddSingleton<IEntityInfoService, EntityInfoService>();
           // services.AddSingleton<IDefaultRowAuthPolicyInitializationService, DefaultRowAuthPolicyInitializationService>();
            services.AddScoped<IDefaultRowAuthPolicyInitializationService, DefaultRowAuthPolicyInitializationService>();
            // 注册实体信息配置类为单例（用于迁移原有EntityBizMappingService的注册逻辑）
            services.AddSingleton<EntityInfoConfig>();
            
            //// 保持向后兼容性：直接使用现有的EntityBizMappingService实现
             //services.AddSingleton<IEntityBizMappingService, EntityBizMappingService>();
            
            return services;
        }
        
        /// <summary>
        /// 注册实体信息服务及其依赖项，并执行常用实体映射的注册
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合，支持链式调用</returns>
        public static IServiceCollection AddEntityInfoServicesWithMappings(this IServiceCollection services)
        {
            services.AddEntityInfoServices();
            
            // 注册一个初始化类，用于在应用启动时执行实体映射注册
            services.AddSingleton<IStartupInitializer, StartupInitializer>();
            
            return services;
        }
        
        /// <summary>
        /// 启动初始化接口 - 提供应用启动时执行初始化操作的能力
        /// </summary>
        public interface IStartupInitializer 
        {
            /// <summary>
            /// 执行初始化操作
            /// </summary>
            void Initialize();
        }
        
        /// <summary>
        /// 启动初始化实现类
        /// </summary>
        private class StartupInitializer : IStartupInitializer 
        {
            private readonly EntityInfoConfig _config;
            private readonly ILogger<StartupInitializer> _logger;
            
            public StartupInitializer(EntityInfoConfig config, ILogger<StartupInitializer> logger)
            {
                _config = config;
                _logger = logger;
            }
            
            /// <summary>
            /// 执行实体映射注册
            /// </summary>
            public void Initialize()
            {
                try
                {
                    //_logger.LogInformation("开始执行实体映射注册");
                    _config.RegisterCommonMappings();
                    //_logger.LogInformation("实体映射注册完成");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "实体映射注册失败: {ErrorMessage}", ex.Message);
                }
            }
        }
    }
}