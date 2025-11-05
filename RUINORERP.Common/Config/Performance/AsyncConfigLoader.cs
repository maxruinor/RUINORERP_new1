/*************************************************************
 * 文件名：AsyncConfigLoader.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：异步配置加载器，支持配置的懒加载和预加载优化
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Performance
{
    /// <summary>
    /// 异步配置加载器
    /// 用于优化配置加载性能，支持预加载和异步加载
    /// </summary>
    public class AsyncConfigLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AsyncConfigLoader> _logger;
        private readonly ConcurrentDictionary<string, Task<object>> _loadingTasks;
        private readonly object _lockObj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        /// <param name="logger">日志记录器</param>
        public AsyncConfigLoader(IServiceProvider serviceProvider, ILogger<AsyncConfigLoader> logger = null)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger;
            _loadingTasks = new ConcurrentDictionary<string, Task<object>>();
        }

        /// <summary>
        /// 预加载配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>加载任务</returns>
        public Task<TConfig> PreloadConfigAsync<TConfig>() where TConfig : BaseConfig
        {
            return (Task<TConfig>)PreloadConfigAsync(typeof(TConfig));
        }

        /// <summary>
        /// 预加载配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>加载任务</returns>
        public Task<object> PreloadConfigAsync(Type configType)
        {
            if (configType == null)
                throw new ArgumentNullException(nameof(configType));

            if (!typeof(BaseConfig).IsAssignableFrom(configType))
                throw new ArgumentException($"类型 {configType.Name} 必须继承自 BaseConfig");

            string configTypeName = configType.Name;

            // 使用并发字典避免重复加载
            return _loadingTasks.GetOrAdd(configTypeName, async _ =>
            {
                try
                {
                    _logger?.LogDebug("开始预加载配置: {ConfigType}", configTypeName);
                    
                    // 获取配置管理器类型
                    var managerType = typeof(IConfigurationManager<>).MakeGenericType(configType);
                    
                    // 从服务容器获取配置管理器
                    var manager = _serviceProvider.GetService(managerType);
                    if (manager == null)
                    {
                        _logger?.LogWarning("配置管理器未注册: {ConfigType}", configTypeName);
                        return null;
                    }

                    // 调用LoadConfigAsync方法
                    var loadMethod = managerType.GetMethod("LoadConfigAsync");
                    if (loadMethod == null)
                    {
                        _logger?.LogError("配置管理器缺少LoadConfigAsync方法: {ConfigType}", configTypeName);
                        return null;
                    }

                    // 执行加载并返回结果
                    var result = await (Task)loadMethod.Invoke(manager, null);
                    
                    _logger?.LogDebug("配置预加载完成: {ConfigType}", configTypeName);
                    
                    // 获取任务的结果
                    return ((dynamic)result).Result;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "预加载配置失败: {ConfigType}", configTypeName);
                    
                    // 从加载任务字典中移除失败的任务
                    _loadingTasks.TryRemove(configTypeName, out _);
                    
                    throw;
                }
            });
        }

        /// <summary>
        /// 批量预加载配置
        /// </summary>
        /// <param name="configTypes">配置类型数组</param>
        /// <returns>加载任务</returns>
        public async Task PreloadConfigsAsync(params Type[] configTypes)
        {
            if (configTypes == null || configTypes.Length == 0)
                return;

            _logger?.LogInformation("开始批量预加载配置，共 {Count} 个配置", configTypes.Length);

            try
            {
                // 并行预加载所有配置
                var tasks = configTypes.Select(type => PreloadConfigAsync(type)).ToList();
                await Task.WhenAll(tasks);

                _logger?.LogInformation("批量预加载配置完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量预加载配置失败");
                throw;
            }
        }

        /// <summary>
        /// 异步加载配置
        /// 如果配置已经在预加载中，则返回现有的加载任务
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>配置实例</returns>
        public async Task<TConfig> LoadAsync<TConfig>() where TConfig : BaseConfig
        {
            return await PreloadConfigAsync<TConfig>();
        }

        /// <summary>
        /// 清除配置加载缓存
        /// </summary>
        public void ClearCache()
        {
            _loadingTasks.Clear();
            _logger?.LogDebug("配置加载缓存已清除");
        }

        /// <summary>
        /// 清除特定配置的加载缓存
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        public void ClearCache<TConfig>() where TConfig : BaseConfig
        {
            string configTypeName = typeof(TConfig).Name;
            _loadingTasks.TryRemove(configTypeName, out _);
            _logger?.LogDebug("配置加载缓存已清除: {ConfigType}", configTypeName);
        }

        /// <summary>
        /// 获取当前正在加载的配置数量
        /// </summary>
        public int LoadingTaskCount => _loadingTasks.Count;

        /// <summary>
        /// 检查配置是否正在加载或已加载
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>是否正在加载或已加载</returns>
        public bool IsLoadingOrLoaded<TConfig>() where TConfig : BaseConfig
        {
            return _loadingTasks.ContainsKey(typeof(TConfig).Name);
        }
    }

    /// <summary>
    /// 异步配置加载器扩展
    /// 提供依赖注入和便捷方法
    /// </summary>
    public static class AsyncConfigLoaderExtensions
    {
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

        /// <summary>
        /// 获取或创建配置实例
        /// 如果配置已经预加载，则直接返回，否则创建新的实例
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="loader">异步配置加载器</param>
        /// <returns>配置实例</returns>
        public static async Task<TConfig> GetOrCreateConfigAsync<TConfig>(this AsyncConfigLoader loader)
            where TConfig : BaseConfig, new()
        {
            try
            {
                var config = await loader.LoadAsync<TConfig>();
                return config ?? new TConfig();
            }
            catch (Exception)
            {
                // 加载失败时返回新实例
                return new TConfig();
            }
        }
    }

    /// <summary>
    /// 配置预热服务
    /// 在应用启动时预加载关键配置
    /// </summary>
    public class ConfigWarmupService
    {
        private readonly AsyncConfigLoader _configLoader;
        private readonly IConfigurationRegistry _configRegistry;
        private readonly ILogger<ConfigWarmupService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configLoader">异步配置加载器</param>
        /// <param name="configRegistry">配置注册表</param>
        /// <param name="logger">日志记录器</param>
        public ConfigWarmupService(
            AsyncConfigLoader configLoader,
            IConfigurationRegistry configRegistry,
            ILogger<ConfigWarmupService> logger = null)
        {
            _configLoader = configLoader ?? throw new ArgumentNullException(nameof(configLoader));
            _configRegistry = configRegistry ?? throw new ArgumentNullException(nameof(configRegistry));
            _logger = logger;
        }

        /// <summary>
        /// 预热关键配置
        /// 预加载所有标记为关键的配置
        /// </summary>
        /// <returns>预热任务</returns>
        public async Task WarmupCriticalConfigsAsync()
        {
            try
            {
                // 获取所有关键配置类型
                var criticalTypeNames = _configRegistry.GetCriticalConfigTypes().ToList();
                
                if (!criticalTypeNames.Any())
                {
                    _logger?.LogInformation("没有关键配置需要预热");
                    return;
                }

                _logger?.LogInformation("开始预热关键配置，共 {Count} 个", criticalTypeNames.Count);

                // 获取所有配置类型信息
                var allConfigTypes = _configRegistry.GetAllConfigTypes().ToList();
                
                // 筛选出关键配置类型
                var criticalTypes = criticalTypeNames
                    .Select(name => allConfigTypes.FirstOrDefault(info => info.TypeName == name)?.Type)
                    .Where(type => type != null)
                    .ToList();

                // 预加载所有关键配置
                await _configLoader.PreloadConfigsAsync(criticalTypes.ToArray());

                _logger?.LogInformation("关键配置预热完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "预热关键配置失败");
                throw;
            }
        }
    }
}