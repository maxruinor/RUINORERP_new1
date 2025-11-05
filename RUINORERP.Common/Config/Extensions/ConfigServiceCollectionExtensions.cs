/*************************************************************
 * 文件名：ConfigServiceCollectionExtensions.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置系统依赖注入扩展，提供完整的配置服务注册
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Config.Operation;
using RUINORERP.Common.Config.Path;
using RUINORERP.Common.Config.Performance;
using RUINORERP.Common.Config.Validation;
using System;
using System.Reflection;

namespace RUINORERP.Common.Config.Extensions
{
    /// <summary>
    /// 配置系统依赖注入扩展
    /// 提供便捷的配置服务注册方法
    /// </summary>
    public static class ConfigServiceCollectionExtensions
    {
        /// <summary>
        /// 添加基础配置系统
        /// 注册核心配置组件
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configDirectory">配置目录路径</param>
        /// <param name="legacyConfigDirectory">旧版配置目录路径</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddConfigSystem(
            this IServiceCollection services,
            string configDirectory = null,
            string legacyConfigDirectory = null)
        {
            // 注册配置路径解析器
            services.AddSmartConfigPathResolver(configDirectory, legacyConfigDirectory);

            // 注册配置注册表
            services.TryAddSingleton<IConfigurationRegistry, ConfigurationRegistry>();

            // 注册配置策略工厂
            services.TryAddSingleton<ConfigStrategyFactory>();
            
            // 注册服务器和客户端配置策略
            services.TryAddSingleton<ServerConfigStrategy>();
            services.TryAddSingleton<ClientConfigStrategy>();

            return services;
        }

        /// <summary>
        /// 添加增强版配置系统
        /// 包含完整的配置功能，如缓存、审计、健康检查等
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="options">配置选项</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddEnhancedConfigSystem(
            this IServiceCollection services,
            Action<EnhancedConfigOptions> options = null)
        {
            // 创建配置选项
            var configOptions = new EnhancedConfigOptions();
            options?.Invoke(configOptions);

            // 注册基础配置系统
            services.AddConfigSystem(
                configOptions.ConfigDirectory,
                configOptions.LegacyConfigDirectory);

            // 注册配置管理器（原子操作）
            if (configOptions.UseAtomicOperations)
            {
                services.AddSingleton(typeof(IConfigurationManager<>), typeof(AtomicConfigManager<>));
            }

            // 注册配置缓存
            if (configOptions.EnableCaching)
            {
                services.AddMemoryCache();
                services.AddSingleton(typeof(ICachedConfigurationManager<>), typeof(CachedConfigurationManager<>));
            }

            // 注册懒加载配置管理器
            if (configOptions.EnableLazyLoading)
            {
                services.AddSingleton(typeof(ILazyConfigurationManager<>), typeof(LazyConfigurationManager<>));
                services.AddAsyncConfigLoader();
            }

            // 注册配置审计
            if (configOptions.EnableAudit)
            {
                services.AddSingleton<IConfigAuditLogStore>(provider =>
                {
                    var auditDir = configOptions.AuditLogDirectory ?? 
                        System.IO.Path.Combine(configOptions.ConfigDirectory ?? GetDefaultConfigDirectory(), "AuditLogs");
                    var logger = provider.GetService<ILogger<FileSystemAuditLogStore>>();
                    return new FileSystemAuditLogStore(auditDir, logger);
                });
                services.AddSingleton<ConfigAuditLogger>();
            }

            // 注册配置监控
            if (configOptions.EnableMonitoring)
            {
                services.TryAddSingleton<IConfigMetrics, DefaultConfigMetrics>();
                services.TryAddSingleton<ConfigMonitor>();
            }

            // 注册配置验证器
            if (configOptions.EnableValidation)
            {
                services.TryAddSingleton<ConfigConsistencyValidator>();
                services.TryAddSingleton<ConfigMigrationValidator>();
            }

            // 注册配置健康检查
            if (configOptions.EnableHealthChecks)
            {
                services.AddHealthChecks()
                    .AddCheck<ConfigHealthCheck>("config_health");
            }

            // 注册增强配置服务
            services.AddSingleton<EnhancedConfigService>();

            // 注册配置事务支持
            if (configOptions.EnableTransactions)
            {
                services.AddSingleton<ConfigRollbackService>();
                services.AddScoped<IConfigTransaction>(provider =>
                {
                    var rollbackService = provider.GetRequiredService<ConfigRollbackService>();
                    var logger = provider.GetService<ILogger<ConfigTransaction>>();
                    return new ConfigTransaction(rollbackService, logger);
                });
            }

            return services;
        }

        /// <summary>
        /// 注册配置类型
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="isCritical">是否为关键配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection RegisterConfigType<TConfig>(
            this IServiceCollection services,
            bool isCritical = false)
            where TConfig : BaseConfig
        {
            // 注册配置类型
            services.AddSingleton(provider =>
            {
                var registry = provider.GetRequiredService<IConfigurationRegistry>();
                registry.Register<TConfig>(isCritical);
                return registry;
            });

            // 注册对应的配置管理器
            services.AddSingleton<IConfigurationManager<TConfig>, AtomicConfigManager<TConfig>>();

            return services;
        }

        /// <summary>
        /// 从程序集中自动注册配置类型
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="assembly">程序集</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection RegisterConfigTypesFromAssembly(
            this IServiceCollection services,
            Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            services.AddSingleton(provider =>
            {
                var registry = provider.GetRequiredService<IConfigurationRegistry>();
                registry.RegisterFromAssembly(assembly);
                return registry;
            });

            return services;
        }

        /// <summary>
        /// 获取默认配置目录
        /// </summary>
        /// <returns>默认配置目录路径</returns>
        private static string GetDefaultConfigDirectory()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return System.IO.Path.Combine(appDataDir, "RUINORERP", "Config");
        }

        /// <summary>
        /// 添加带审计的配置管理器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="scope">配置作用域</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddAuditedConfigManager<TConfig>(
            this IServiceCollection services,
            ConfigScope scope = ConfigScope.Server)
            where TConfig : BaseConfig
        {
            services.AddSingleton<IConfigurationManager<TConfig>>(provider =>
            {
                var pathResolver = provider.GetRequiredService<IConfigPathResolver>();
                var logger = provider.GetService<ILogger<AtomicConfigManager<TConfig>>>();
                var auditLogger = provider.GetRequiredService<ConfigAuditLogger>();
                
                // 创建原子配置管理器
                var atomicManager = new AtomicConfigManager<TConfig>(pathResolver, logger, auditLogger);
                
                // 添加审计中间件
                return new ConfigAuditMiddleware<TConfig>(atomicManager, auditLogger, scope);
            });
            
            return services;
        }

        /// <summary>
        /// 添加带缓存的配置管理器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="cacheDuration">缓存持续时间</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCachedConfigManager<TConfig>(
            this IServiceCollection services,
            TimeSpan? cacheDuration = null)
            where TConfig : BaseConfig
        {
            services.AddMemoryCache();
            
            services.AddSingleton<IConfigurationManager<TConfig>>(provider =>
            {
                var pathResolver = provider.GetRequiredService<IConfigPathResolver>();
                var logger = provider.GetService<ILogger<AtomicConfigManager<TConfig>>>();
                var memoryCache = provider.GetRequiredService<IMemoryCache>();
                
                // 创建原子配置管理器
                var atomicManager = new AtomicConfigManager<TConfig>(pathResolver, logger);
                
                // 添加缓存装饰器
                return new CachedConfigurationManager<TConfig>(
                    atomicManager,
                    memoryCache,
                    logger,
                    cacheDuration ?? TimeSpan.FromMinutes(5));
            });
            
            return services;
        }

        /// <summary>
        /// 添加完整的配置管理管道
        /// 包含原子操作、缓存、审计等功能
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="scope">配置作用域</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCompleteConfigManager<TConfig>(
            this IServiceCollection services,
            ConfigScope scope = ConfigScope.Server)
            where TConfig : BaseConfig
        {
            services.AddMemoryCache();
            
            services.AddSingleton<IConfigurationManager<TConfig>>(provider =>
            {
                var pathResolver = provider.GetRequiredService<IConfigPathResolver>();
                var logger = provider.GetService<ILogger<AtomicConfigManager<TConfig>>>();
                var memoryCache = provider.GetRequiredService<IMemoryCache>();
                var auditLogger = provider.GetRequiredService<ConfigAuditLogger>();
                
                // 创建原子配置管理器作为基础
                var atomicManager = new AtomicConfigManager<TConfig>(pathResolver, logger, auditLogger);
                
                // 添加缓存装饰器
                var cachedManager = new CachedConfigurationManager<TConfig>(
                    atomicManager,
                    memoryCache,
                    logger,
                    TimeSpan.FromMinutes(5));
                
                // 添加审计中间件
                return new ConfigAuditMiddleware<TConfig>(cachedManager, auditLogger, scope);
            });
            
            return services;
        }
    }

    /// <summary>
    /// 增强配置系统选项
    /// 用于配置增强版配置系统的行为
    /// </summary>
    public class EnhancedConfigOptions
    {
        /// <summary>
        /// 配置目录路径
        /// </summary>
        public string ConfigDirectory { get; set; }

        /// <summary>
        /// 旧版配置目录路径
        /// 用于配置迁移
        /// </summary>
        public string LegacyConfigDirectory { get; set; }

        /// <summary>
        /// 审计日志目录路径
        /// </summary>
        public string AuditLogDirectory { get; set; }

        /// <summary>
        /// 是否启用原子操作
        /// 确保配置读写的原子性
        /// </summary>
        public bool UseAtomicOperations { get; set; } = true;

        /// <summary>
        /// 是否启用缓存
        /// 缓存配置以提高性能
        /// </summary>
        public bool EnableCaching { get; set; } = true;

        /// <summary>
        /// 是否启用懒加载
        /// 延迟加载配置以优化启动性能
        /// </summary>
        public bool EnableLazyLoading { get; set; } = true;

        /// <summary>
        /// 是否启用审计
        /// 记录配置变更历史
        /// </summary>
        public bool EnableAudit { get; set; } = true;

        /// <summary>
        /// 是否启用监控
        /// 监控配置操作性能和状态
        /// </summary>
        public bool EnableMonitoring { get; set; } = true;

        /// <summary>
        /// 是否启用验证
        /// 验证配置的一致性和完整性
        /// </summary>
        public bool EnableValidation { get; set; } = true;

        /// <summary>
        /// 是否启用健康检查
        /// 提供配置系统的健康状态检查
        /// </summary>
        public bool EnableHealthChecks { get; set; } = true;

        /// <summary>
        /// 是否启用事务支持
        /// 支持配置的事务性操作
        /// </summary>
        public bool EnableTransactions { get; set; } = true;
    }
}