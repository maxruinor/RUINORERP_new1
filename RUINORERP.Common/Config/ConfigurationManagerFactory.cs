using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置管理器工厂，负责创建和管理各种配置管理器实例
    /// </summary>
    public class ConfigurationManagerFactory
    {
        private readonly IConfigPathResolver _pathResolver;
        private readonly ConfigurationRegistry _registry;
        private readonly ILoggerFactory _loggerFactory;
        private readonly Dictionary<string, object> _configManagers = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        /// <param name="registry">配置类型注册表</param>
        /// <param name="loggerFactory">日志工厂</param>
        public ConfigurationManagerFactory(IConfigPathResolver pathResolver, ConfigurationRegistry registry, ILoggerFactory loggerFactory)
        {
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// 获取指定类型的配置管理器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>配置管理器实例</returns>
        public IConfigurationManager<TConfig> GetConfigurationManager<TConfig>() where TConfig : class, new()
        {
            string configTypeName = typeof(TConfig).Name;
            return (IConfigurationManager<TConfig>)GetConfigurationManagerInternal(configTypeName, typeof(TConfig));
        }

        /// <summary>
        /// 根据配置类型名称获取配置管理器
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置管理器实例，如果未找到对应类型则返回null</returns>
        public object GetConfigurationManager(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));

            Type configType = _registry.GetConfigType(configTypeName);
            if (configType == null)
                return null;

            return GetConfigurationManagerInternal(configTypeName, configType);
        }

        /// <summary>
        /// 内部方法：获取或创建配置管理器实例
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="configType">配置类型</param>
        /// <returns>配置管理器实例</returns>
        private object GetConfigurationManagerInternal(string configTypeName, Type configType)
        {
            lock (_lock)
            {
                if (!_configManagers.TryGetValue(configTypeName, out object manager))
                {
                    // 创建泛型配置管理器实例
                    Type managerType = typeof(ConfigurationManager<>).MakeGenericType(configType);
                    manager = Activator.CreateInstance(managerType, _pathResolver, _loggerFactory.CreateLogger(managerType));
                    _configManagers[configTypeName] = manager;
                }
                return manager;
            }
        }

        /// <summary>
        /// 注册并获取指定类型的配置管理器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>配置管理器实例</returns>
        public IConfigurationManager<TConfig> RegisterAndGet<TConfig>() where TConfig : class, new()
        {
            // 注册配置类型
            _registry.RegisterConfigType(typeof(TConfig));
            // 返回配置管理器
            return GetConfigurationManager<TConfig>();
        }

        /// <summary>
        /// 获取所有已创建的配置管理器
        /// </summary>
        /// <returns>配置管理器字典</returns>
        public IReadOnlyDictionary<string, object> GetAllConfigurationManagers()
        {
            lock (_lock)
            {
                return new Dictionary<string, object>(_configManagers);
            }
        }
    }

    /// <summary>
    /// 泛型配置管理器实现
    /// </summary>
    /// <typeparam name="TConfig">配置类型</typeparam>
    public class ConfigurationManager<TConfig> : IConfigurationManager<TConfig> where TConfig : class, new()
    {
        private readonly IConfigPathResolver _pathResolver;
        private readonly ILogger _logger;
        private readonly object _lock = new object();
        private TConfig _currentConfig;
        private bool _isLoaded = false;

        /// <summary>
        /// 当前配置实例
        /// </summary>
        public TConfig CurrentConfig
        {
            get
            {
                if (!_isLoaded)
                {
                    Load();
                }
                return _currentConfig;
            }
        }

        /// <summary>
        /// 配置变更事件
        /// </summary>
        public event EventHandler<ConfigurationChangedEventArgs<TConfig>> ConfigurationChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        /// <param name="logger">日志记录器</param>
        public ConfigurationManager(IConfigPathResolver pathResolver, ILogger logger)
        {
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentConfig = CreateDefaultConfig();
        }
        
        /// <summary>
        /// 创建默认配置
        /// 支持IConfigInitializable接口的配置初始化
        /// </summary>
        /// <returns>初始化后的配置实例</returns>
        private TConfig CreateDefaultConfig()
        {
            TConfig config = new TConfig();
            
            // 如果配置实现了IConfigInitializable接口，调用初始化方法
            if (config is IConfigInitializable initializableConfig)
            {
                try
                {
                    initializableConfig.Initialize();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "初始化配置失败: {ConfigType}", typeof(TConfig).Name);
                }
            }
            
            return config;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns>配置是否加载成功</returns>
        public bool Load()
        {
            return Load(ConfigPathType.Server);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置是否加载成功</returns>
        public bool Load(ConfigPathType pathType)
        {
            try
            {
                string configTypeName = typeof(TConfig).Name;
                string filePath = _pathResolver.GetConfigFilePath(configTypeName, pathType);

                if (!System.IO.File.Exists(filePath))
                {
                    _logger.LogWarning("配置文件不存在: {FilePath}", filePath);
                    _currentConfig = CreateDefaultConfig();
                    _isLoaded = true;
                    return false;
                }

                string jsonContent = System.IO.File.ReadAllText(filePath);
                var loadedConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<TConfig>(jsonContent);

                lock (_lock)
                {
                    TConfig oldConfig = _currentConfig;
                    _currentConfig = loadedConfig ?? new TConfig();
                    _isLoaded = true;

                    // 触发配置变更事件
                    OnConfigurationChanged(oldConfig, _currentConfig);
                }

                _logger.LogInformation("成功加载配置: {ConfigType}", configTypeName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置失败: {ConfigType}", typeof(TConfig).Name);
                return false;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns>配置是否保存成功</returns>
        public bool Save()
        {
            return Save(_currentConfig);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config">要保存的配置</param>
        /// <returns>配置是否保存成功</returns>
        public bool Save(TConfig config)
        {
            return Save(config, ConfigPathType.Server);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config">要保存的配置</param>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置是否保存成功</returns>
        public bool Save(TConfig config, ConfigPathType pathType)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            try
            {
                string configTypeName = typeof(TConfig).Name;
                
                // 确保配置目录存在
                _pathResolver.EnsureConfigDirectoryExists(pathType);
                
                string filePath = _pathResolver.GetConfigFilePath(configTypeName, pathType);
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText(filePath, jsonContent);

                lock (_lock)
                {
                    TConfig oldConfig = _currentConfig;
                    _currentConfig = config;
                    _isLoaded = true;

                    // 触发配置变更事件
                    OnConfigurationChanged(oldConfig, _currentConfig);
                }

                _logger.LogInformation("成功保存配置: {ConfigType}", configTypeName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置失败: {ConfigType}", typeof(TConfig).Name);
                return false;
            }
        }

        /// <summary>
        /// 刷新配置
        /// </summary>
        /// <returns>配置是否刷新成功</returns>
        public bool Refresh()
        {
            return Load();
        }

        /// <summary>
        /// 重置配置为默认值
        /// </summary>
        /// <returns>配置是否重置成功</returns>
        public bool Reset()
        {
            try
            {
                _currentConfig = CreateDefaultConfig();
                _isLoaded = true;
                
                // 触发配置变更事件
                OnConfigurationChanged(null, _currentConfig);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置配置失败: {ConfigType}", typeof(TConfig).Name);
                return false;
            }
        }

        /// <summary>
        /// 触发配置变更事件
        /// </summary>
        /// <param name="oldConfig">旧配置</param>
        /// <param name="newConfig">新配置</param>
        protected virtual void OnConfigurationChanged(TConfig oldConfig, TConfig newConfig)
        {
            ConfigurationChanged?.Invoke(this, new ConfigurationChangedEventArgs<TConfig>(oldConfig, newConfig));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 清理资源
            ConfigurationChanged = null;
        }
    }

    /// <summary>
    /// 配置变更事件参数
    /// </summary>
    /// <typeparam name="TConfig">配置类型</typeparam>
    public class ConfigurationChangedEventArgs<TConfig> : EventArgs
    {
        /// <summary>
        /// 旧配置
        /// </summary>
        public TConfig OldConfig { get; }

        /// <summary>
        /// 新配置
        /// </summary>
        public TConfig NewConfig { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldConfig">旧配置</param>
        /// <param name="newConfig">新配置</param>
        public ConfigurationChangedEventArgs(TConfig oldConfig, TConfig newConfig)
        {
            OldConfig = oldConfig;
            NewConfig = newConfig;
        }
    }
}