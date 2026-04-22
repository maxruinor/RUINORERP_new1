using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model.ConfigModel;
using System;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 服务集合扩展
    /// 提供配置管理相关的扩展方法
    /// 注意：此方法仅用于 Microsoft DI 兼容层，实际推荐使用 Autofac 模块化注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加配置管理服务（仅注册非泛型服务）
        /// 泛型服务应在 Autofac 中注册，避免双重注册导致实例不一致
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        [Obsolete("建议使用 Autofac 模块化注册方式，参考 BusinessDIConfig.ConfigureContainer")]
        public static IServiceCollection AddConfigManagementServices(this IServiceCollection services)
        {
            // 只注册非泛型服务，泛型服务由 Autofac 管理
            services.AddSingleton<IConfigSyncService, ConfigSyncService>();
            services.AddSingleton<IConfigManagerService, ConfigManagerService>();
            services.AddSingleton<IConfigValidationService, ConfigValidationService>();
            services.AddSingleton<IConfigEncryptionService, ConfigEncryptionService>();
            services.AddSingleton<IConfigVersionService, ConfigVersionService>();
            
            // 注意：不再注册泛型服务 IGenericConfigService<>
            // 泛型服务应在 Autofac 中通过 RegisterGeneric 注册
            
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