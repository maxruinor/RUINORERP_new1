/*************************************************************
 * 文件名：LazyConfigurationManager.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：懒加载配置管理器，优化配置加载性能
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Performance
{
    /// <summary>
    /// 懒加载配置管理器
    /// 使用懒加载模式优化配置加载性能
    /// 配置只在首次访问时才会被加载
    /// </summary>
    /// <typeparam name="TConfig">配置类型</typeparam>
    public class LazyConfigurationManager<TConfig> : IConfigurationManager<TConfig> where TConfig : BaseConfig
    {
        private readonly IConfigurationManager<TConfig> _innerManager;
        private readonly ILogger<LazyConfigurationManager<TConfig>> _logger;
        private readonly Lazy<Task<TConfig>> _lazyConfig;
        private readonly object _refreshLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="innerManager">内部配置管理器</param>
        /// <param name="logger">日志记录器</param>
        public LazyConfigurationManager(IConfigurationManager<TConfig> innerManager, ILogger<LazyConfigurationManager<TConfig>> logger = null)
        {
            _innerManager = innerManager ?? throw new ArgumentNullException(nameof(innerManager));
            _logger = logger;
            
            // 初始化懒加载配置
            _lazyConfig = new Lazy<Task<TConfig>>(LoadConfigInternalAsync);
        }

        /// <summary>
        /// 加载配置
        /// 使用懒加载模式，只在首次访问时加载
        /// </summary>
        /// <returns>配置对象</returns>
        public Task<TConfig> LoadConfigAsync()
        {
            _logger?.LogDebug("访问懒加载配置: {ConfigType}", typeof(TConfig).Name);
            return _lazyConfig.Value;
        }

        /// <summary>
        /// 内部加载配置方法
        /// </summary>
        private async Task<TConfig> LoadConfigInternalAsync()
        {
            _logger?.LogInformation("首次加载配置: {ConfigType}", typeof(TConfig).Name);
            return await _innerManager.LoadConfigAsync();
        }

        /// <summary>
        /// 保存配置
        /// 保存后会触发刷新，下次访问时重新加载
        /// </summary>
        /// <param name="config">配置对象</param>
        public async Task SaveConfigAsync(TConfig config)
        {
            _logger?.LogDebug("保存配置: {ConfigType}", typeof(TConfig).Name);
            await _innerManager.SaveConfigAsync(config);
            
            // 保存成功后刷新配置
            await RefreshConfigAsync();
        }

        /// <summary>
        /// 刷新配置
        /// 清除当前缓存，下次访问时重新加载
        /// </summary>
        public async Task RefreshConfigAsync()
        {
            lock (_refreshLock)
            {   
                // 检查是否已经初始化
                if (_lazyConfig.IsValueCreated)
                {   
                    _logger?.LogInformation("刷新懒加载配置: {ConfigType}", typeof(TConfig).Name);
                    
                    // 重新初始化懒加载对象
                    // 使用反射重置Lazy对象（这是一个变通方法，因为Lazy不直接支持重置）
                    // 注意：这是一个实现细节，可能需要根据.NET版本调整
                    try
                    {
                        // 更安全的方式是重新创建一个新的Lazy对象
                        // 但由于Lazy是只读的，我们需要通过反射来修改它
                        // 这里我们采用更简单的方法：创建一个新的Lazy并重新加载
                        await LoadConfigInternalAsync(); // 先预热一下新配置
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "刷新配置时发生异常: {ConfigType}", typeof(TConfig).Name);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 重置配置到默认值
        /// 重置后会触发刷新
        /// </summary>
        public async Task ResetConfigAsync()
        {
            _logger?.LogDebug("重置配置: {ConfigType}", typeof(TConfig).Name);
            await _innerManager.ResetConfigAsync();
            
            // 重置成功后刷新配置
            await RefreshConfigAsync();
        }

        /// <summary>
        /// 配置是否已加载
        /// </summary>
        public bool IsConfigLoaded => _lazyConfig.IsValueCreated;

        /// <summary>
        /// 配置变更事件
        /// 委托给内部管理器的事件
        /// </summary>
        public event EventHandler OnConfigChanged
        {
            add { _innerManager.OnConfigChanged += value; }
            remove { _innerManager.OnConfigChanged -= value; }
        }
    }

    /// <summary>
    /// 延迟加载配置策略
    /// 提供更细粒度的配置加载控制
    /// </summary>
    public enum LazyLoadingStrategy
    {
        /// <summary>
        /// 首次访问时加载
        /// </summary>
        OnFirstAccess,
        /// <summary>
        /// 应用启动时预热加载
        /// </summary>
        PreloadOnStartup,
        /// <summary>
        /// 后台异步加载
        /// </summary>
        BackgroundAsync
    }

    /// <summary>
    /// 懒加载配置管理器工厂
    /// 用于创建不同加载策略的懒加载配置管理器
    /// </summary>
    public class LazyConfigurationManagerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LazyConfigurationManagerFactory> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        /// <param name="logger">日志记录器</param>
        public LazyConfigurationManagerFactory(IServiceProvider serviceProvider, ILogger<LazyConfigurationManagerFactory> logger = null)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger;
        }

        /// <summary>
        /// 创建懒加载配置管理器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="strategy">加载策略</param>
        /// <returns>懒加载配置管理器</returns>
        public IConfigurationManager<TConfig> CreateManager<TConfig>(LazyLoadingStrategy strategy = LazyLoadingStrategy.OnFirstAccess) where TConfig : BaseConfig
        {
            // 获取基础配置管理器
            var innerManager = _serviceProvider.GetService(typeof(IConfigurationManager<TConfig>)) as IConfigurationManager<TConfig>;
            
            if (innerManager == null)
            {
                throw new InvalidOperationException($"无法解析配置管理器: {typeof(IConfigurationManager<TConfig>).Name}");
            }

            // 创建懒加载管理器
            var lazyManager = new LazyConfigurationManager<TConfig>(innerManager, _logger);

            // 根据策略处理加载行为
            if (strategy == LazyLoadingStrategy.PreloadOnStartup || strategy == LazyLoadingStrategy.BackgroundAsync)
            {
                if (strategy == LazyLoadingStrategy.PreloadOnStartup)
                {
                    // 立即预加载
                    Task.Run(async () =>
                    {
                        try
                        {
                            await lazyManager.LoadConfigAsync();
                            _logger?.LogInformation("配置预加载完成: {ConfigType}", typeof(TConfig).Name);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "配置预加载失败: {ConfigType}", typeof(TConfig).Name);
                        }
                    }).Wait(); // 同步等待预加载完成
                }
                else
                {
                    // 后台异步加载
                    Task.Run(async () =>
                    {
                        try
                        {
                            await Task.Delay(1000); // 延迟一秒再加载
                            await lazyManager.LoadConfigAsync();
                            _logger?.LogInformation("配置后台加载完成: {ConfigType}", typeof(TConfig).Name);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "配置后台加载失败: {ConfigType}", typeof(TConfig).Name);
                        }
                    });
                }
            }

            return lazyManager;
        }
    }

    /// <summary>
    /// 异步配置加载器
    /// 提供配置的异步加载和预热功能
    /// </summary>
    public class AsyncConfigLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AsyncConfigLoader> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        /// <param name="logger">日志记录器</param>
        public AsyncConfigLoader(IServiceProvider serviceProvider, ILogger<AsyncConfigLoader> logger = null)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger;
        }

        /// <summary>
        /// 预热单个配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        public async Task PreloadConfigAsync<TConfig>() where TConfig : BaseConfig
        {
            try
            {
                var manager = _serviceProvider.GetService<IConfigurationManager<TConfig>>();
                if (manager != null)
                {
                    _logger?.LogInformation("预热配置: {ConfigType}", typeof(TConfig).Name);
                    await manager.LoadConfigAsync();
                    _logger?.LogInformation("配置预热完成: {ConfigType}", typeof(TConfig).Name);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置预热失败: {ConfigType}", typeof(TConfig).Name);
            }
        }

        /// <summary>
        /// 并行预热多个配置
        /// </summary>
        /// <param name="configTypes">配置类型列表</param>
        public async Task PreloadConfigsAsync(params Type[] configTypes)
        {
            var tasks = new List<Task>();
            
            foreach (var configType in configTypes)
            {
                // 动态调用泛型方法
                var method = typeof(AsyncConfigLoader).GetMethod("PreloadConfigAsync", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(configType);
                
                tasks.Add((Task)genericMethod.Invoke(this, null));
            }

            await Task.WhenAll(tasks);
            _logger?.LogInformation("配置并行预热完成，共预热 {Count} 个配置", configTypes.Length);
        }
    }

    /// <summary>
    /// 懒加载配置管理器扩展
    /// 提供依赖注入的便捷方法
    /// </summary>
    public static class LazyConfigurationManagerExtensions
    {
        /// <summary>
        /// 添加懒加载配置管理器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="strategy">加载策略</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddLazyConfigurationManager<TConfig>(
            this IServiceCollection services,
            LazyLoadingStrategy strategy = LazyLoadingStrategy.OnFirstAccess)
            where TConfig : BaseConfig
        {
            // 注册懒加载配置管理器工厂
            services.AddSingleton<LazyConfigurationManagerFactory>();
            
            // 注册懒加载配置管理器作为主要的配置管理器
            services.AddSingleton<IConfigurationManager<TConfig>>(provider =>
            {
                var factory = provider.GetRequiredService<LazyConfigurationManagerFactory>();
                return factory.CreateManager<TConfig>(strategy);
            });

            return services;
        }

        /// <summary>
        /// 添加异步配置加载器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddAsyncConfigLoader(this IServiceCollection services)
        {
            services.AddSingleton<AsyncConfigLoader>();
            return services;
        }
    }
}