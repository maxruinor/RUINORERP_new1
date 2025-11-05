/*************************************************************
 * 文件名：ConfigDependencyInjectionAdapter.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置依赖注入适配器
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RUINORERP.Common.Config.Caching;
using RUINORERP.Common.Config.Parsing;
using RUINORERP.Common.Config.Strategies;
using RUINORERP.Common.Config.Validation;
using RUINORERP.Common.Config.Auditing;
using RUINORERP.Common.Config.Encryption;
using RUINORERP.Common.Config.Notifications;
using RUINORERP.Common.Config.Watching;

namespace RUINORERP.Common.Config.DI
{
    /// <summary>
    /// 服务生命周期枚举
    /// </summary>
    public enum ServiceLifetime
    {
        /// <summary>
        /// 单例模式 - 全局唯一实例
        /// </summary>
        Singleton,
        
        /// <summary>
        /// 作用域模式 - 每个作用域一个实例
        /// </summary>
        Scoped,
        
        /// <summary>
        /// 瞬态模式 - 每次请求一个新实例
        /// </summary>
        Transient
    }

    /// <summary>
    /// 配置服务注册选项
    /// </summary>
    public class ConfigServiceOptions
    {
        /// <summary>
        /// 启用配置缓存
        /// </summary>
        public bool EnableCaching { get; set; } = true;
        
        /// <summary>
        /// 启用配置审计
        /// </summary>
        public bool EnableAuditing { get; set; } = false;
        
        /// <summary>
        /// 启用配置加密
        /// </summary>
        public bool EnableEncryption { get; set; } = true;
        
        /// <summary>
        /// 启用配置变更通知
        /// </summary>
        public bool EnableChangeNotifications { get; set; } = true;
        
        /// <summary>
        /// 启用配置文件监视
        /// </summary>
        public bool EnableFileWatching { get; set; } = false;
        
        /// <summary>
        /// 配置文件基础目录
        /// </summary>
        public string ConfigBaseDirectory { get; set; }
        
        /// <summary>
        /// 自动验证配置
        /// </summary>
        public bool AutoValidate { get; set; } = true;
        
        /// <summary>
        /// 自动应用默认值
        /// </summary>
        public bool AutoApplyDefaults { get; set; } = true;
        
        /// <summary>
        /// 自动从环境变量加载配置
        /// </summary>
        public bool AutoLoadFromEnvironment { get; set; } = true;
        
        /// <summary>
        /// 注册的配置类型列表
        /// </summary>
        internal List<Type> ConfigTypes { get; } = new List<Type>();
        
        /// <summary>
        /// 注册配置类型
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>配置服务选项</returns>
        public ConfigServiceOptions RegisterConfig<TConfig>() where TConfig : class, new()
        {
            ConfigTypes.Add(typeof(TConfig));
            return this;
        }
    }

    /// <summary>
    /// 服务注册器接口
    /// 定义服务注册的基本操作
    /// </summary>
    public interface IServiceRegistrar
    {
        /// <summary>
        /// 注册单例服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="TImplementation">实现类型</typeparam>
        void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService;
        
        /// <summary>
        /// 注册单例服务（带实例）
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="instance">服务实例</param>
        void RegisterSingleton<TService>(TService instance) where TService : class;
        
        /// <summary>
        /// 注册单例服务（带工厂）
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="factory">服务工厂</param>
        void RegisterSingleton<TService>(Func<IServiceProvider, TService> factory) where TService : class;
        
        /// <summary>
        /// 注册作用域服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="TImplementation">实现类型</typeparam>
        void RegisterScoped<TService, TImplementation>() where TImplementation : class, TService;
        
        /// <summary>
        /// 注册作用域服务（带工厂）
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="factory">服务工厂</param>
        void RegisterScoped<TService>(Func<IServiceProvider, TService> factory) where TService : class;
        
        /// <summary>
        /// 注册瞬态服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="TImplementation">实现类型</typeparam>
        void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService;
        
        /// <summary>
        /// 注册瞬态服务（带工厂）
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="factory">服务工厂</param>
        void RegisterTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class;
        
        /// <summary>
        /// 获取服务提供者
        /// </summary>
        /// <returns>服务提供者</returns>
        IServiceProvider BuildServiceProvider();
    }

    /// <summary>
    /// 配置依赖注入适配器
    /// 提供配置系统与不同DI容器的适配能力
    /// </summary>
    public class ConfigDependencyInjectionAdapter
    {
        private readonly IServiceRegistrar _serviceRegistrar;
        private readonly ConfigServiceOptions _options;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceRegistrar">服务注册器</param>
        /// <param name="options">配置服务选项</param>
        public ConfigDependencyInjectionAdapter(IServiceRegistrar serviceRegistrar, ConfigServiceOptions options)
        {
            _serviceRegistrar = serviceRegistrar ?? throw new ArgumentNullException(nameof(serviceRegistrar));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        
        /// <summary>
        /// 注册配置系统服务
        /// </summary>
        public void RegisterConfigServices()
        {
            // 注册配置选项
            _serviceRegistrar.RegisterSingleton(_options);
            
            // 注册核心服务
            RegisterCoreServices();
            
            // 注册功能模块服务
            RegisterFeatureServices();
            
            // 注册配置管理器
            RegisterConfigManagers();
        }
        
        /// <summary>
        /// 注册核心服务
        /// </summary>
        private void RegisterCoreServices()
        {
            // 注册配置解析器
            _serviceRegistrar.RegisterSingleton<IConfigParser, DefaultConfigParser>();
            
            // 注册配置验证器
            _serviceRegistrar.RegisterTransient<IConfigValidator, DefaultConfigValidator>();
            
            // 注册配置策略
            _serviceRegistrar.RegisterSingleton(typeof(IConfigStrategy<>), typeof(DefaultConfigStrategy<>));
            _serviceRegistrar.RegisterSingleton(typeof(AppConfigStrategy<>));
            _serviceRegistrar.RegisterSingleton(typeof(UserConfigStrategy<>));
            _serviceRegistrar.RegisterSingleton(typeof(ModuleConfigStrategy<>));
        }
        
        /// <summary>
        /// 注册功能模块服务
        /// </summary>
        private void RegisterFeatureServices()
        {
            // 配置缓存
            if (_options.EnableCaching)
            {
                _serviceRegistrar.RegisterSingleton<IConfigCacheManager, DefaultConfigCacheManager>();
            }
            else
            {
                _serviceRegistrar.RegisterSingleton<IConfigCacheManager, NoOpConfigCacheManager>();
            }
            
            // 配置审计
            if (_options.EnableAuditing)
            {
                _serviceRegistrar.RegisterSingleton<IConfigAuditLogger, DefaultConfigAuditLogger>();
            }
            else
            {
                _serviceRegistrar.RegisterSingleton<IConfigAuditLogger, NoOpConfigAuditLogger>();
            }
            
            // 配置加密
            if (_options.EnableEncryption)
            {
                _serviceRegistrar.RegisterSingleton<IConfigEncryptor, DefaultConfigEncryptor>();
            }
            else
            {
                _serviceRegistrar.RegisterSingleton<IConfigEncryptor, NoOpConfigEncryptor>();
            }
            
            // 配置变更通知
            if (_options.EnableChangeNotifications)
            {
                _serviceRegistrar.RegisterSingleton<IConfigChangeNotifier, DefaultConfigChangeNotifier>();
            }
            else
            {
                _serviceRegistrar.RegisterSingleton<IConfigChangeNotifier, NoOpConfigChangeNotifier>();
            }
            
            // 配置文件监视
            if (_options.EnableFileWatching)
            {
                _serviceRegistrar.RegisterSingleton<IConfigWatcher, DefaultConfigWatcher>();
            }
            else
            {
                _serviceRegistrar.RegisterSingleton<IConfigWatcher, NoOpConfigWatcher>();
            }
        }
        
        /// <summary>
        /// 注册配置管理器
        /// </summary>
        private void RegisterConfigManagers()
        {
            // 注册通用配置管理器工厂
            _serviceRegistrar.RegisterSingleton<ConfigManagerFactory>();
            
            // 注册具体的配置类型
            foreach (var configType in _options.ConfigTypes)
            {
                RegisterConfigManagerForType(configType);
            }
        }
        
        /// <summary>
        /// 为特定类型注册配置管理器
        /// </summary>
        /// <param name="configType">配置类型</param>
        private void RegisterConfigManagerForType(Type configType)
        {
            Type managerInterfaceType = typeof(IConfigManager<>).MakeGenericType(configType);
            Type managerType = typeof(ConfigManager<>).MakeGenericType(configType);
            
            // 创建配置管理器的工厂方法
            Func<IServiceProvider, object> factory = (sp) =>
            {
                // 获取配置策略
                Type strategyInterfaceType = typeof(IConfigStrategy<>).MakeGenericType(configType);
                var strategy = sp.GetService(strategyInterfaceType);
                
                if (strategy == null)
                {
                    // 如果没有找到特定策略，使用默认策略
                    strategy = sp.GetService(typeof(DefaultConfigStrategy<>).MakeGenericType(configType));
                }
                
                // 创建配置管理器实例
                var manager = Activator.CreateInstance(managerType, new object[] {
                    sp.GetService<IConfigParser>(),
                    sp.GetService<IConfigValidator>(),
                    sp.GetService<IConfigCacheManager>(),
                    sp.GetService<IConfigAuditLogger>(),
                    sp.GetService<IConfigChangeNotifier>(),
                    sp.GetService<IConfigWatcher>(),
                    sp.GetService<IConfigEncryptor>(),
                    strategy
                });
                
                return manager;
            };
            
            // 注册为单例
            _serviceRegistrar.RegisterSingleton(managerInterfaceType, factory);
            
            // 同时注册为具体类型，便于直接解析
            _serviceRegistrar.RegisterSingleton(managerType, factory);
        }
    }
    
    /// <summary>
    /// 配置依赖注入扩展方法
    /// </summary>
    public static class ConfigDependencyInjectionExtensions
    {
        /// <summary>
        /// 添加配置系统
        /// </summary>
        /// <param name="registrar">服务注册器</param>
        /// <param name="configure">配置操作</param>
        /// <returns>服务注册器</returns>
        public static IServiceRegistrar AddConfigSystem(this IServiceRegistrar registrar, Action<ConfigServiceOptions> configure = null)
        {
            var options = new ConfigServiceOptions();
            configure?.Invoke(options);
            
            var adapter = new ConfigDependencyInjectionAdapter(registrar, options);
            adapter.RegisterConfigServices();
            
            return registrar;
        }
        
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="options">配置服务选项</param>
        /// <returns>配置服务选项</returns>
        public static ConfigServiceOptions AddConfig<TConfig>(this ConfigServiceOptions options)
            where TConfig : class, new()
        {
            options.RegisterConfig<TConfig>();
            return options;
        }
        
        /// <summary>
        /// 注册指定程序集中的所有配置类型
        /// </summary>
        /// <param name="options">配置服务选项</param>
        /// <param name="assembly">程序集</param>
        /// <param name="predicate">类型筛选条件</param>
        /// <returns>配置服务选项</returns>
        public static ConfigServiceOptions RegisterConfigsFromAssembly(
            this ConfigServiceOptions options, 
            Assembly assembly, 
            Func<Type, bool> predicate = null)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            
            // 获取程序集中所有符合条件的类型
            var types = assembly.GetTypes()
                .Where(t => 
                    t.IsClass && 
                    !t.IsAbstract && 
                    t.IsPublic &&
                    t.GetConstructor(Type.EmptyTypes) != null &&
                    (predicate == null || predicate(t)));
            
            foreach (var type in types)
            {
                options.ConfigTypes.Add(type);
            }
            
            return options;
        }
        
        /// <summary>
        /// 从当前程序集注册配置类型
        /// </summary>
        /// <param name="options">配置服务选项</param>
        /// <returns>配置服务选项</returns>
        public static ConfigServiceOptions RegisterConfigsFromCurrentAssembly(this ConfigServiceOptions options)
        {
            return options.RegisterConfigsFromAssembly(Assembly.GetCallingAssembly());
        }
        
        /// <summary>
        /// 启用所有功能
        /// </summary>
        /// <param name="options">配置服务选项</param>
        /// <returns>配置服务选项</returns>
        public static ConfigServiceOptions EnableAllFeatures(this ConfigServiceOptions options)
        {
            options.EnableCaching = true;
            options.EnableAuditing = true;
            options.EnableEncryption = true;
            options.EnableChangeNotifications = true;
            options.EnableFileWatching = true;
            options.AutoValidate = true;
            options.AutoApplyDefaults = true;
            options.AutoLoadFromEnvironment = true;
            
            return options;
        }
        
        /// <summary>
        /// 禁用所有功能
        /// </summary>
        /// <param name="options">配置服务选项</param>
        /// <returns>配置服务选项</returns>
        public static ConfigServiceOptions DisableAllFeatures(this ConfigServiceOptions options)
        {
            options.EnableCaching = false;
            options.EnableAuditing = false;
            options.EnableEncryption = false;
            options.EnableChangeNotifications = false;
            options.EnableFileWatching = false;
            options.AutoValidate = false;
            options.AutoApplyDefaults = false;
            options.AutoLoadFromEnvironment = false;
            
            return options;
        }
    }
}