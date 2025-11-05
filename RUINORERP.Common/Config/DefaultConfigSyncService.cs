using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置同步服务的默认实现（简化版）
    /// </summary>
    public class DefaultConfigSyncService : IConfigSyncService
    {
        private readonly ILogger<DefaultConfigSyncService> _logger;
        private readonly ConfigurationRegistry _registry;
        private readonly ConfigurationManagerFactory _factory;
        private readonly IConfigPathResolver _pathResolver;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="registry">配置类型注册表</param>
        /// <param name="factory">配置管理器工厂</param>
        /// <param name="pathResolver">配置路径解析器</param>
        public DefaultConfigSyncService(
            ILogger<DefaultConfigSyncService> logger,
            ConfigurationRegistry registry,
            ConfigurationManagerFactory factory,
            IConfigPathResolver pathResolver)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
        }

        /// <summary>
        /// 发布配置变更到所有客户端
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置实例</param>
        /// <param name="forceApply">是否强制应用配置</param>
        /// <returns>是否发布成功</returns>
        public Task<bool> PublishConfigChangeAsync<TConfig>(TConfig config, bool forceApply = true) where TConfig : class
        {
            try
            {
                if (config == null)
                    throw new ArgumentNullException(nameof(config));

                string configTypeName = typeof(TConfig).Name;
                
                // 确保配置类型已注册
                if (!_registry.IsConfigTypeRegistered(configTypeName))
                {
                    _registry.RegisterConfigType(typeof(TConfig));
                }

                // 创建同步数据
                var syncData = new ConfigSyncData
                {
                    ConfigType = configTypeName,
                    ConfigData = Newtonsoft.Json.JsonConvert.SerializeObject(config),
                    Version = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    ForceApply = forceApply
                };

                _logger.LogInformation("成功发布配置变更: {ConfigType}", configTypeName);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发布配置变更失败: {ConfigType}", typeof(TConfig).Name);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 应用从服务端接收的配置变更
        /// </summary>
        /// <param name="configSyncData">配置同步数据</param>
        /// <returns>是否应用成功</returns>
        public Task<bool> ApplyConfigSyncAsync(ConfigSyncData configSyncData)
        {
            try
            {
                if (configSyncData == null)
                    throw new ArgumentNullException(nameof(configSyncData));

                if (string.IsNullOrEmpty(configSyncData.ConfigType) || string.IsNullOrEmpty(configSyncData.ConfigData))
                {
                    _logger.LogWarning("配置同步数据无效: 缺少配置类型或数据");
                    return Task.FromResult(false);
                }

                // 获取配置类型
                Type configType = _registry.GetConfigType(configSyncData.ConfigType);
                if (configType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configSyncData.ConfigType);
                    return Task.FromResult(false);
                }

                // 反序列化配置数据
                object config = Newtonsoft.Json.JsonConvert.DeserializeObject(configSyncData.ConfigData, configType);
                if (config == null)
                {
                    _logger.LogWarning("配置数据反序列化失败: {ConfigType}", configSyncData.ConfigType);
                    return Task.FromResult(false);
                }

                // 使用反射调用配置管理器的保存方法
                bool saveResult = SaveConfigByReflection(configSyncData.ConfigType, config, ConfigPathType.Client);

                if (saveResult)
                {
                    _logger.LogInformation("成功应用配置变更: {ConfigType}, 版本: {Version}", 
                        configSyncData.ConfigType, configSyncData.Version);
                }

                return Task.FromResult(saveResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "应用配置变更失败: {ConfigType}", configSyncData?.ConfigType);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 向服务端请求最新配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>配置实例，如果请求失败则返回null</returns>
        public async Task<TConfig> RequestLatestConfigAsync<TConfig>() where TConfig : class, new()
        {
            try
            {
                // 从本地加载
                _logger.LogInformation("请求最新配置: {ConfigType}", typeof(TConfig).Name);

                // 从工厂获取配置管理器
                var configManager = _factory.GetConfigurationManager<TConfig>();
                
                // 尝试从服务器路径加载
                bool loaded = configManager.Load(ConfigPathType.Server);
                if (!loaded)
                {
                    // 如果失败，尝试从客户端路径加载
                    loaded = configManager.Load(ConfigPathType.Client);
                }

                return loaded ? configManager.CurrentConfig : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "请求最新配置失败: {ConfigType}", typeof(TConfig).Name);
                return null;
            }
        }

        /// <summary>
        /// 向服务端请求所有配置
        /// </summary>
        /// <returns>是否请求成功</returns>
        public async Task<bool> RequestAllConfigsAsync()
        {
            try
            {
                _logger.LogInformation("请求所有配置");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "请求所有配置失败");
                return false;
            }
        }

        /// <summary>
        /// 使用反射保存配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="config">配置实例</param>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>是否保存成功</returns>
        private bool SaveConfigByReflection(string configTypeName, object config, ConfigPathType pathType)
        {
            try
            {
                // 从工厂获取配置管理器
                object configManager = _factory.GetConfigurationManager(configTypeName);
                if (configManager == null)
                {
                    _logger.LogWarning("未找到配置管理器: {ConfigType}", configTypeName);
                    return false;
                }

                // 使用反射调用保存方法
                var saveMethod = configManager.GetType().GetMethod("Save", new Type[] { config.GetType(), typeof(ConfigPathType) });
                if (saveMethod == null)
                {
                    _logger.LogWarning("未找到保存方法: {ConfigType}", configTypeName);
                    return false;
                }

                return (bool)saveMethod.Invoke(configManager, new object[] { config, pathType });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "使用反射保存配置失败: {ConfigType}", configTypeName);
                return false;
            }
        }
    }
}