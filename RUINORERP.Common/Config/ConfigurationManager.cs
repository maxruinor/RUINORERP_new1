using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 泛型配置管理器实现
    /// 提供统一的配置管理功能，支持任意配置类型
    /// 实现黑盒处理模式，对配置内容不做假设
    /// </summary>
    /// <typeparam name="TConfig">配置类型</typeparam>
    public class ConfigurationManager<TConfig> : IConfigurationManager<TConfig> where TConfig : class, new()
    {
        private readonly IConfigPathResolver _pathResolver;
        private readonly ILogger<ConfigurationManager<TConfig>> _logger;
        private readonly FileSystemWatcher _fileWatcher;
        private readonly object _lockObject = new object();
        private readonly SemaphoreSlim _loadLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _saveLock = new SemaphoreSlim(1, 1);
        
        // 当前配置实例 - 配置作为黑盒存储，不关心具体内容
        private TConfig _currentConfig;
        
        // 配置类型名称，默认为类型名称
        private string _configTypeName;
        
        // 配置变更事件
        public event EventHandler<ConfigurationChangedEventArgs<TConfig>> ConfigurationChanged;
        
        // 配置变更订阅列表 - 使用观察者模式实现配置热更新
        private readonly List<Action<TConfig>> _subscribers = new List<Action<TConfig>>();
        
        // 订阅令牌列表 - 用于管理订阅生命周期
        private readonly List<IDisposable> _subscriptionTokens = new List<IDisposable>();
        
        // 是否已加载
        private bool _isLoaded = false;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        /// <param name="logger">日志记录器</param>
        public ConfigurationManager(IConfigPathResolver pathResolver, ILogger<ConfigurationManager<TConfig>> logger)
        {
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
            _logger = logger;
            
            // 初始化默认配置
            _currentConfig = new TConfig();
            
            // 初始化文件监控
            _fileWatcher = InitializeFileWatcher();
            
            // 自动注册到配置注册表
            RegisterToRegistry();
        }
        
        /// <summary>
        /// 当前配置实例
        /// </summary>
        public TConfig CurrentConfig => _currentConfig;
        
   
        
        #region 异步方法实现
        
        /// <summary>
        /// 获取当前配置
        /// </summary>
        public TConfig GetCurrentConfig()
        {
            return _currentConfig;
        }
        
        /// <summary>
        /// 异步加载配置
        /// </summary>
        public async Task<TConfig> LoadConfigAsync(ConfigPathType pathType = ConfigPathType.Server)
        {
            // 使用异步锁确保线程安全
            await _loadLock.WaitAsync();
            try
            {
                // 使用配置类型名称作为配置标识，支持自定义配置名称
                string configFileName = _configTypeName ?? typeof(TConfig).Name;
                string configFilePath = _pathResolver.GetConfigFilePath(configFileName, pathType);
                
                _logger?.LogDebug("正在加载配置: {ConfigType} 从路径: {FilePath}", configFileName, configFilePath);
                
                // 如果配置文件不存在，创建默认配置
                if (!File.Exists(configFilePath))
                {
                    _logger?.LogWarning("配置文件不存在，将创建默认配置: {FilePath}", configFilePath);
                    // 创建默认配置
                    _currentConfig = CreateDefaultConfig();
                    // 保存默认配置到文件
                    await SaveConfigAsync(_currentConfig, pathType);
                    NotifyConfigChanged();
                    _isLoaded = true;
                    return _currentConfig;
                }
                
                // 异步读取配置文件内容
                string jsonContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);
                
                // 智能反序列化 - 支持多种配置格式
                _currentConfig = await DeserializeConfigAsync(jsonContent);
                
                // 配置加载成功后，启用文件监控
                _fileWatcher.EnableRaisingEvents = true;
                _isLoaded = true;
                
                return _currentConfig;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "加载配置失败: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
                // 加载失败时，使用默认配置
                _currentConfig = CreateDefaultConfig();
                return _currentConfig;
            }
            finally
            {
                _loadLock.Release();
            }
        }
        
        /// <summary>
        /// 智能反序列化配置 - 支持多种配置格式
        /// </summary>
        private async Task<TConfig> DeserializeConfigAsync(string jsonContent)
        {
            try
            {
                // 尝试直接反序列化
                var config = JsonConvert.DeserializeObject<TConfig>(jsonContent);
                if (config != null)
                {
                    return config;
                }
            }
            catch (JsonException)
            {
                // 第一种格式失败，尝试解析为包装对象
                try
                {
                    // 尝试解析为包含Config属性的包装对象
                    var wrapper = JsonConvert.DeserializeObject<ConfigWrapper<TConfig>>(jsonContent);
                    if (wrapper?.Config != null)
                    {
                        return wrapper.Config;
                    }
                }
                catch (JsonException)
                {
                    // 包装对象解析失败，尝试动态对象解析
                    try
                    {
                        // 尝试动态解析，支持不同格式的配置文件
                        var dynamicConfig = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                        if (dynamicConfig != null)
                        {
                            // 检查是否有Config属性
                            if (dynamicConfig.Config != null)
                            {
                                return JsonConvert.DeserializeObject<TConfig>(JsonConvert.SerializeObject(dynamicConfig.Config));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "配置智能解析失败");
                    }
                }
            }
            
            // 所有解析都失败，返回默认配置
            _logger?.LogWarning("配置解析失败，使用默认配置");
            return CreateDefaultConfig();
        }
        
        /// <summary>
        /// 异步保存配置
        /// </summary>
        public async Task<bool> SaveConfigAsync(TConfig config, ConfigPathType pathType = ConfigPathType.Server)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            await _saveLock.WaitAsync();
            try
            {
                // 使用配置类型名称作为配置标识，支持自定义配置名称
                string configFileName = _configTypeName ?? typeof(TConfig).Name;
                string configFilePath = _pathResolver.GetConfigFilePath(configFileName, pathType);
                
                _logger?.LogDebug("正在保存配置: {ConfigType} 到路径: {FilePath}", configFileName, configFilePath);
                
                // 序列化配置，统一使用UTF8编码
                string jsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                
                // 异步写入文件，使用UTF8编码确保中文正确保存
                await File.WriteAllTextAsync(configFilePath, jsonContent, Encoding.UTF8);
                
                // 更新当前配置实例
                _currentConfig = config;
                _isLoaded = true;
                
                // 通知所有订阅者配置已变更 - 实现配置热更新
                NotifyConfigChanged();
                
                _logger?.LogDebug("配置保存成功: {ConfigType}", configFileName);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "保存配置失败: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
                return false;
            }
            finally
            {
                _saveLock.Release();
            }
        }
        
        /// <summary>
        /// 异步刷新配置
        /// </summary>
        public async Task<bool> RefreshAsync(ConfigPathType pathType = ConfigPathType.Server)
        {
            try
            {
                await LoadConfigAsync(pathType);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "刷新配置失败: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
                return false;
            }
        }
        
        /// <summary>
        /// 创建默认配置
        /// </summary>
        public TConfig CreateDefaultConfig()
        {
            try
            {
                TConfig config = new TConfig();
                
                // 如果配置实现了IConfigInitializable接口，调用初始化方法
                if (config is IConfigInitializable initializableConfig)
                {
                    initializableConfig.Initialize();
                }
                
                return config;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建默认配置失败: {ConfigType}", typeof(TConfig).Name);
                return new TConfig();
            }
        }
        
        /// <summary>
        /// 异步创建默认配置并保存
        /// </summary>
        public async Task<TConfig> CreateDefaultConfigAsync()
        {
            try
            {
                // 使用内部统一的CreateDefaultConfig方法，确保配置正确初始化
                _currentConfig = CreateDefaultConfig();
                
                // 保存到文件
                bool saved = await SaveConfigAsync(_currentConfig);
                if (!saved)
                {
                    _logger?.LogWarning("创建默认配置成功，但保存失败");
                }
                
                _logger?.LogDebug("默认配置创建成功: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
                return _currentConfig;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建默认配置失败: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
                throw;
            }
        }
        
        /// <summary>
        /// 订阅配置变更
        /// </summary>
        public IDisposable Subscribe(Action<TConfig> onChange)
        {
            if (onChange == null)
            {
                throw new ArgumentNullException(nameof(onChange));
            }
            
            lock (_lockObject)
            {
                // 创建订阅令牌
                var subscriptionToken = new ConfigSubscriptionToken<TConfig>(this, onChange);
                
                // 保存订阅者和订阅令牌
                _subscribers.Add(onChange);
                _subscriptionTokens.Add(subscriptionToken);
                
                _logger?.LogDebug("配置订阅已添加: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
                return subscriptionToken;
            }
        }
        
        /// <summary>
        /// 检查配置文件是否存在
        /// </summary>
        public bool ConfigExists(ConfigPathType pathType = ConfigPathType.Server)
        {
            // 使用配置类型名称作为配置标识
            string configFileName = _configTypeName ?? typeof(TConfig).Name;
            string configFilePath = _pathResolver.GetConfigFilePath(configFileName, pathType);
            return File.Exists(configFilePath);
        }
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        public string GetConfigFilePath(ConfigPathType pathType = ConfigPathType.Server)
        {
            // 使用配置类型名称作为配置标识
            string configFileName = _configTypeName ?? typeof(TConfig).Name;
            return _pathResolver.GetConfigFilePath(configFileName, pathType);
        }
        
        /// <summary>
        /// 通知配置已变更
        /// </summary>
        public void NotifyConfigChanged()
        {
            try
            {
                // 获取当前配置的副本，避免并发修改问题
                TConfig currentConfigCopy;
                List<Action<TConfig>> subscribersCopy;
                
                lock (_lockObject)
                {
                    currentConfigCopy = _currentConfig;
                    subscribersCopy = new List<Action<TConfig>>(_subscribers);
                }
                
                // 触发事件
                ConfigurationChanged?.Invoke(this, new ConfigurationChangedEventArgs<TConfig>(currentConfigCopy));
                
                // 异步通知所有订阅者，避免阻塞
                _ = Task.Run(() =>
                {
                    foreach (var subscriber in subscribersCopy)
                    {
                        try
                        {
                            subscriber(currentConfigCopy);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "通知配置订阅者时发生错误");
                        }
                    }
                });
                
                _logger?.LogDebug("配置变更通知已发送: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "通知配置变更失败: {ConfigType}", _configTypeName ?? typeof(TConfig).Name);
            }
        }
        
        /// <summary>
        /// 设置配置类型名称
        /// </summary>
        public void SetConfigTypeName(string configTypeName)
        {
            _configTypeName = configTypeName;
            
            // 更新文件监控器的过滤条件
            if (_fileWatcher != null)
            {
                _fileWatcher.Filter = $"{configTypeName}.json";
            }
        }
        
        #endregion
        
        #region 私有辅助方法
        
        /// <summary>
        /// 初始化文件监控器
        /// </summary>
        private FileSystemWatcher InitializeFileWatcher()
        {
            // 配置文件监控器，实现配置热更新
            var watcher = new FileSystemWatcher
            {
                Path = _pathResolver.GetConfigDirectory(),
                Filter = $"{typeof(TConfig).Name}.json",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
                EnableRaisingEvents = false // 初始时禁用，加载完成后启用
            };
            watcher.Changed += OnConfigFileChanged;
            
            return watcher;
        }
        
        /// <summary>
        /// 配置文件变更处理 - 实现配置热更新的文件监控机制
        /// </summary>
        private void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            // 避免文件锁定问题，延迟加载
            Task.Delay(100).ContinueWith(_ =>
            {
                try
                {
                    _logger?.LogDebug("检测到配置文件变更: {FileName}", e.Name);
                    // 异步加载配置
                    _ = LoadConfigAsync();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "处理配置文件变更失败: {FileName}", e.Name);
                }
            });
        }
        
        /// <summary>
        /// 移除订阅者
        /// </summary>
        internal void RemoveSubscriber(Action<TConfig> subscriber)
        {
            lock (_lockObject)
            {
                _subscribers.Remove(subscriber);
                // 清理订阅令牌
                _subscriptionTokens.RemoveAll(token => token is ConfigSubscriptionToken<TConfig> subscriptionToken && subscriptionToken.Subscriber == subscriber);
            }
        }
        
        /// <summary>
        /// 注册到配置注册表
        /// </summary>
        private void RegisterToRegistry()
        {
            try
            {
                ConfigurationRegistry.Instance.Register<TConfig>();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "注册配置类型到注册表失败");
            }
        }
        
        #endregion
        
        #region IDisposable实现
        
        private bool _disposed = false;
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    if (_fileWatcher != null)
                    {
                        _fileWatcher.EnableRaisingEvents = false;
                        _fileWatcher.Changed -= OnConfigFileChanged;
                        _fileWatcher.Dispose();
                    }
                    
                    _loadLock.Dispose();
                    _saveLock.Dispose();
                    
                    // 清理订阅
                    _subscribers.Clear();
                    _subscriptionTokens.ForEach(token => token.Dispose());
                    _subscriptionTokens.Clear();
                }
                
                _disposed = true;
            }
        }
        
        ~ConfigurationManager()
        {
            Dispose(false);
        }
        
        #endregion
        
        /// <summary>
        /// 配置包装器类 - 用于兼容不同格式的配置文件
        /// </summary>
        private class ConfigWrapper<T>
        {
            public T Config { get; set; }
            public string ConfigName { get; set; }
        }
        
        /// <summary>
        /// 配置订阅令牌 - 用于管理配置订阅的生命周期
        /// </summary>
        private class ConfigSubscriptionToken<TSub> : IDisposable
        {
            private readonly ConfigurationManager<TSub> _configManager;
            public Action<TSub> Subscriber { get; }
            private bool _isDisposed;
            
            public ConfigSubscriptionToken(ConfigurationManager<TSub> configManager, Action<TSub> subscriber)
            {
                _configManager = configManager;
                Subscriber = subscriber;
            }
            
            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _configManager.RemoveSubscriber(Subscriber);
                    _isDisposed = true;
                }
            }
        }
    }
    
    /// <summary>
    /// 配置变更事件参数类
    /// </summary>
    public class ConfigurationChangedEventArgs<TConfig> : EventArgs
    {
        /// <summary>
        /// 新的配置对象
        /// </summary>
        public TConfig NewConfig { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="newConfig">新的配置对象</param>
        public ConfigurationChangedEventArgs(TConfig newConfig)
        {
            NewConfig = newConfig;
        }
    }
}