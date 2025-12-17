using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model.ConfigModel;
using System;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 服务集合扩展
    /// 提供配置管理相关的扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加配置管理服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddConfigManagementServices(this IServiceCollection services)
        {
            // 注册配置同步服务
            services.AddSingleton<IConfigSyncService, ConfigSyncService>();
            
            // 注册配置管理相关服务
            services.AddSingleton<IConfigManagerService, ConfigManagerService>();
            services.AddSingleton(typeof(IGenericConfigService<>), typeof(GenericConfigService<>));
            services.AddSingleton<IConfigValidationService, ConfigValidationService>();
            services.AddSingleton<IConfigEncryptionService, ConfigEncryptionService>();
            services.AddSingleton<IConfigVersionService, ConfigVersionService>();
            services.AddSingleton(typeof(IGenericConfigVersionService<>), typeof(GenericConfigVersionService<>));
            
            return services;
        }

        /// <summary>
        /// 注册配置变更事件处理器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="serviceProvider">服务提供程序</param>
        public static void RegisterConfigChangeHandlers<T>(this IServiceProvider serviceProvider) where T : BaseConfig, new()
        {
            var configManager = serviceProvider.GetRequiredService<IConfigManagerService>();
            var configSync = serviceProvider.GetRequiredService<IConfigSyncService>();
            
            // 注册配置变更事件处理器
            configManager.RegisterConfigChangedHandler<T>((sender, e) =>
            {
                configSync.UpdateConfigInstance(e.NewConfig);
            });
        }

        /// <summary>
        /// 注册所有配置变更事件处理器
        /// </summary>
        /// <param name="serviceProvider">服务提供程序</param>
        public static void RegisterAllConfigChangeHandlers(this IServiceProvider serviceProvider)
        {
            // 注册系统全局配置变更处理器
            serviceProvider.RegisterConfigChangeHandlers<SystemGlobalConfig>();
            
            // 注册全局验证配置变更处理器
            serviceProvider.RegisterConfigChangeHandlers<GlobalValidatorConfig>();
            
            // 注册服务器全局配置变更处理器
            serviceProvider.RegisterConfigChangeHandlers<ServerGlobalConfig>();
            
            // 可根据需要添加更多配置类型
        }
    }
}