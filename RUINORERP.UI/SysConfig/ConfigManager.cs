
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 配置管理器
    /// 负责管理系统全局配置和验证配置，包括初始化、监控文件变化等功能
    /// 同时支持从数据库加载动态配置
    /// </summary>
    public class ConfigManager : IDisposable
    {
        #region 单例模式实现
        private static readonly Lazy<ConfigManager> _instance = new Lazy<ConfigManager>(() => new ConfigManager());
        
        /// <summary>
        /// 获取配置管理器实例
        /// </summary>
        public static ConfigManager Instance => _instance.Value;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ConfigManager() { }
        #endregion
        
        #region 配置属性
        private IOptionsMonitor<SystemGlobalconfig> _globalConfigMonitor;
        private IOptionsMonitor<GlobalValidatorConfig> _validatorConfigMonitor;
        
        // 本地存储的配置对象，用于不依赖IOptionsMonitor的场景
        private SystemGlobalconfig _localGlobalConfig;
        private GlobalValidatorConfig _localValidatorConfig;
        
        // 存储配置变更订阅
        private readonly List<Action<SystemGlobalconfig>> _globalConfigSubscribers = new List<Action<SystemGlobalconfig>>();
        private readonly List<Action<GlobalValidatorConfig>> _validatorConfigSubscribers = new List<Action<GlobalValidatorConfig>>();
        
        // 数据库配置缓存
        private readonly Dictionary<string, string> _dbConfigCache = new Dictionary<string, string>();
        
        /// <summary>
        /// 系统全局配置监控器
        /// </summary>
        public IOptionsMonitor<SystemGlobalconfig> GlobalConfigMonitor
        {
            get => _globalConfigMonitor;
            set
            {
                if (_globalConfigMonitor != value)
                {
                    _globalConfigMonitor = value;
                    
                    // 订阅配置变更事件
                    if (_globalConfigMonitor != null)
                    {
                        _globalConfigMonitor.OnChange(OnGlobalConfigChanged);
                    }
                }
            }
        }
        
        /// <summary>
        /// 验证配置监控器
        /// </summary>
        public IOptionsMonitor<GlobalValidatorConfig> ValidatorConfigMonitor
        {
            get => _validatorConfigMonitor;
            set
            {
                if (_validatorConfigMonitor != value)
                {
                    _validatorConfigMonitor = value;
                    
                    // 订阅配置变更事件
                    if (_validatorConfigMonitor != null)
                    {
                        _validatorConfigMonitor.OnChange(OnValidatorConfigChanged);
                    }
                }
            }
        }
        
        /// <summary>
        /// 当前系统全局配置
        /// </summary>
        public SystemGlobalconfig GlobalConfig 
        {
            get 
            {
                // 优先使用IOptionsMonitor的值，否则使用本地存储的值
                return _globalConfigMonitor?.CurrentValue ?? _localGlobalConfig;
            }
            private set
            {
                _localGlobalConfig = value;
            }
        }
        
        /// <summary>
        /// 当前验证配置
        /// </summary>
        public GlobalValidatorConfig ValidatorConfig 
        {
            get 
            {
                // 优先使用IOptionsMonitor的值，否则使用本地存储的值
                return _validatorConfigMonitor?.CurrentValue ?? _localValidatorConfig;
            }
            private set
            {
                _localValidatorConfig = value;
            }
        }
        #endregion
        
        #region 配置文件监控
        private FileSystemWatcher _globalConfigWatcher;
        private FileSystemWatcher _validatorConfigWatcher;
        private string _configDirectory;
        
        /// <summary>
        /// 配置变更事件
        /// </summary>
        public event EventHandler<ConfigChangedEventArgs> ConfigChanged;
        
        /// <summary>
        /// 初始化配置管理器
        /// </summary>
        /// <param name="configMonitor">系统全局配置监控器</param>
        /// <param name="validatorMonitor">验证配置监控器</param>
        public void Initialize(IOptionsMonitor<SystemGlobalconfig> configMonitor, IOptionsMonitor<GlobalValidatorConfig> validatorMonitor)
        {
            // 初始化配置监控器
            GlobalConfigMonitor = configMonitor;
            ValidatorConfigMonitor = validatorMonitor;
            
            // 获取配置目录（与Startup.cs中的配置路径保持一致）
            _configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            
            // 初始化配置文件监控
            InitializeFileWatchers();
            
            // 确保配置文件存在
            EnsureConfigFilesExist();
            
            // 异步加载数据库配置
            Task.Run(() => LoadConfigValues());
        }
        
        /// <summary>
        /// 从数据库加载配置值
        /// </summary>
        public async Task LoadConfigValues()
        {
            List<tb_SysGlobalDynamicConfig> configEntries = new List<tb_SysGlobalDynamicConfig>();
            try
            {
                // 从数据库加载配置项
                tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig> ctr = Startup.GetFromFac<tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig>>();
                configEntries = await ctr.QueryAsync();
                if (configEntries == null)
                {
                    return;
                }
                
                // 清空并重新加载缓存
                lock (_dbConfigCache)
                {
                    _dbConfigCache.Clear();
                    foreach (var entry in configEntries)
                    {
                        if (!_dbConfigCache.ContainsKey(entry.ConfigKey))
                        {
                            _dbConfigCache.Add(entry.ConfigKey, entry.ConfigValue);
                        }
                    }
                }
                
                // 触发配置变更事件
                ConfigChanged?.Invoke(this, new ConfigChangedEventArgs { ConfigType = ConfigType.Database });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从数据库加载配置失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 初始化文件监控器
        /// </summary>
        private void InitializeFileWatchers()
        {
            if (!Directory.Exists(_configDirectory))
            {
                Directory.CreateDirectory(_configDirectory);
            }
            
            // 监控系统全局配置文件
            _globalConfigWatcher = CreateFileWatcher(nameof(SystemGlobalconfig));
            _globalConfigWatcher.Changed += OnGlobalConfigFileChanged;
            
            // 监控验证配置文件
            _validatorConfigWatcher = CreateFileWatcher(nameof(GlobalValidatorConfig));
            _validatorConfigWatcher.Changed += OnValidatorConfigFileChanged;
        }
        
        /// <summary>
        /// 创建文件监控器
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>文件监控器实例</returns>
        private FileSystemWatcher CreateFileWatcher(string configName)
        {
            var watcher = new FileSystemWatcher
            {
                Path = _configDirectory,
                Filter = $"{configName}.json",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
                EnableRaisingEvents = true
            };
            
            return watcher;
        }
        
        /// <summary>
        /// 确保配置文件存在，如果不存在则创建默认配置
        /// </summary>
        public void EnsureConfigFilesExist()
        {
            // 确保系统全局配置文件存在
            EnsureConfigFileExists<SystemGlobalconfig>();
            
            // 确保验证配置文件存在
            EnsureConfigFileExists<GlobalValidatorConfig>();
        }
        
        /// <summary>
        /// 确保指定类型的配置文件存在
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        private void EnsureConfigFileExists<T>() where T : BaseConfig, new()
        {
            string configFileName = typeof(T).Name;
            string configPath = Path.Combine(_configDirectory, $"{configFileName}.json");
            
            if (!File.Exists(configPath))
            {
                // 创建默认配置
                var config = new T();
                // 创建包含配置对象的字典，而不是使用动态属性名的匿名对象
                var configObject = new { Config = config, ConfigName = configFileName };
                string configJson = JsonConvert.SerializeObject(configObject, Formatting.Indented);
                
                // 确保目录存在
                if (!Directory.Exists(_configDirectory))
                {
                    Directory.CreateDirectory(_configDirectory);
                }
                
                // 写入配置文件
                File.WriteAllText(configPath, configJson);
            }
        }
        
        /// <summary>
        /// 系统全局配置文件变更处理
        /// </summary>
        private void OnGlobalConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            HandleConfigFileChanged(e.FullPath, ConfigType.Global);
        }
        
        /// <summary>
        /// 验证配置文件变更处理
        /// </summary>
        private void OnValidatorConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            HandleConfigFileChanged(e.FullPath, ConfigType.Validator);
        }
        
        /// <summary>
        /// 处理配置文件变更
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="configType">配置类型</param>
        private void HandleConfigFileChanged(string filePath, ConfigType configType)
        {
            try
            {
                // 防止文件被锁定，等待一小段时间
                Task.Delay(100).Wait();
                
                // 引发配置变更事件
                ConfigChanged?.Invoke(this, new ConfigChangedEventArgs { ConfigType = configType });
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序
                System.Diagnostics.Debug.WriteLine($"处理配置文件变更时出错: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 系统全局配置变更处理
        /// </summary>
        private void OnGlobalConfigChanged(SystemGlobalconfig updatedConfig)
        {
            // 更新本地配置
            _localGlobalConfig = updatedConfig;
            
            // 触发事件
            ConfigChanged?.Invoke(this, new ConfigChangedEventArgs { ConfigType = ConfigType.Global, Config = updatedConfig });
            
            // 通知所有订阅者
            foreach (var subscriber in _globalConfigSubscribers.ToList())
            {
                try
                {
                    subscriber(updatedConfig);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"配置变更订阅者执行异常: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 验证配置变更处理
        /// </summary>
        private void OnValidatorConfigChanged(GlobalValidatorConfig updatedConfig)
        {
            // 更新本地配置
            _localValidatorConfig = updatedConfig;
            
            // 触发事件
            ConfigChanged?.Invoke(this, new ConfigChangedEventArgs { ConfigType = ConfigType.Validator, Config = updatedConfig });
            
            // 通知所有订阅者
            foreach (var subscriber in _validatorConfigSubscribers.ToList())
            {
                try
                {
                    subscriber(updatedConfig);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"配置变更订阅者执行异常: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 订阅系统全局配置变更，类似于IOptionsMonitor.OnChange
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        /// <returns>用于取消订阅的IDisposable对象</returns>
        public IDisposable OnChange(Action<SystemGlobalconfig> listener)
        {
            // 直接调用现有方法，保持接口兼容
            return OnGlobalConfigChange(listener);
        }
        
        /// <summary>
        /// 订阅系统全局配置变更的具体实现
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        /// <returns>用于取消订阅的IDisposable对象</returns>
        public IDisposable OnGlobalConfigChange(Action<SystemGlobalconfig> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));
                
            _globalConfigSubscribers.Add(listener);
            
            // 返回用于取消订阅的对象
            return new UnsubscribeToken(() => _globalConfigSubscribers.Remove(listener));
        }
        
        /// <summary>
        /// 订阅验证配置变更，类似于IOptionsMonitor.OnChange
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        /// <returns>用于取消订阅的IDisposable对象</returns>
        public IDisposable OnValidatorConfigChange(Action<GlobalValidatorConfig> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));
                
            _validatorConfigSubscribers.Add(listener);
            
            // 返回用于取消订阅的对象
            return new UnsubscribeToken(() => _validatorConfigSubscribers.Remove(listener));
        }
        
        /// <summary>
        /// 用于取消订阅的令牌类
        /// </summary>
        private class UnsubscribeToken : IDisposable
        {
            private readonly Action _unsubscribeAction;
            private bool _isDisposed;
            
            public UnsubscribeToken(Action unsubscribeAction)
            {
                _unsubscribeAction = unsubscribeAction ?? throw new ArgumentNullException(nameof(unsubscribeAction));
            }
            
            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _unsubscribeAction();
                    _isDisposed = true;
                }
            }
        }
        
        /// <summary>
        /// 手动刷新配置
        /// </summary>
        public void RefreshConfig()
        {
            // 通过FileSystemWatcher触发文件变更事件，以重新加载配置
            TriggerConfigFileChange<SystemGlobalconfig>();
            TriggerConfigFileChange<GlobalValidatorConfig>();
            
            // 刷新数据库配置
            Task.Run(() => LoadConfigValues());
        }
        
        /// <summary>
        /// 触发配置文件变更，用于手动刷新配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        private void TriggerConfigFileChange<T>() where T : BaseConfig
        {
            string configFileName = typeof(T).Name;
            string configPath = Path.Combine(_configDirectory, $"{configFileName}.json");
            
            if (File.Exists(configPath))
            {
                // 通过修改文件的最后写入时间来触发变更事件
                File.SetLastWriteTime(configPath, DateTime.Now);
            }
        }
        
        /// <summary>
        /// 手动更新全局配置
        /// </summary>
        /// <param name="newConfig">新的配置对象</param>
        public void UpdateGlobalConfig(SystemGlobalconfig newConfig)
        {
            if (newConfig == null)
                throw new ArgumentNullException(nameof(newConfig));
                
            // 更新本地配置
            _localGlobalConfig = newConfig;
            
            // 触发配置变更处理
            OnGlobalConfigChanged(newConfig);
            
            // 保存到文件
            SaveConfigToFile(newConfig, Path.Combine(_configDirectory, $"{nameof(SystemGlobalconfig)}.json"));
        }
        
        /// <summary>
        /// 手动更新验证配置
        /// </summary>
        /// <param name="newConfig">新的配置对象</param>
        public void UpdateValidatorConfig(GlobalValidatorConfig newConfig)
        {
            if (newConfig == null)
                throw new ArgumentNullException(nameof(newConfig));
                
            // 更新本地配置
            _localValidatorConfig = newConfig;
            
            // 触发配置变更处理
            OnValidatorConfigChanged(newConfig);
            
            // 保存到文件
            SaveConfigToFile(newConfig, Path.Combine(_configDirectory, $"{nameof(GlobalValidatorConfig)}.json"));
        }
        

        
        /// <summary>
        /// 将配置保存到文件
        /// </summary>
        private void SaveConfigToFile<T>(T config, string filePath)
        {
            try
            {
                // 创建包含配置对象的字典，而不是使用动态属性名的匿名对象
                var configObject = new { Config = config, ConfigName = typeof(T).Name };
                string configJson = JsonConvert.SerializeObject(configObject, Formatting.Indented);
                File.WriteAllText(filePath, configJson);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存配置文件失败: {ex.Message}");
            }
        }
        #endregion
        
        #region 配置值获取
        /// <summary>
        /// 获取配置值（优先从数据库配置中查找，然后从系统配置中查找）
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>配置值字符串</returns>
        public string GetValue(string key)
        {
            // 首先尝试从数据库配置中查找
            if (_dbConfigCache.TryGetValue(key, out var dbValue))
            {
                return dbValue;
            }
            
            // 然后尝试从系统配置中查找
            try
            {
                // 获取配置属性
                var property = typeof(SystemGlobalconfig).GetProperty(key);
                if (property != null && GlobalConfig != null)
                {
                    var value = property.GetValue(GlobalConfig);
                    return value?.ToString() ?? string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取配置值失败: {ex.Message}");
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 获取泛型配置值（仅从系统配置中查找）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns>配置值</returns>
        public T GetValue<T>(string propertyName)
        {
            try
            {
                // 获取配置属性
                var property = typeof(SystemGlobalconfig).GetProperty(propertyName);
                if (property != null && GlobalConfig != null)
                {
                    var value = property.GetValue(GlobalConfig);
                    if (value != null && value is T)
                    {
                        return (T)value;
                    }
                    // 如果类型不匹配，尝试转换
                    if (value != null)
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                return default;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取配置值失败: {ex.Message}");
                return default;
            }
        }
        
        /// <summary>
        /// 获取数据库配置值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>配置值字符串</returns>
        public string GetDatabaseValue(string key)
        {
            if (_dbConfigCache.TryGetValue(key, out var value))
            {
                return value;
            }
            
            System.Diagnostics.Debug.WriteLine($"数据库配置键 '{key}' 未找到");
            return string.Empty;
        }
        
        /// <summary>
        /// 检查配置键是否存在
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>是否存在</returns>
        public bool ContainsKey(string key)
        {
            // 检查数据库配置
            if (_dbConfigCache.ContainsKey(key))
            {
                return true;
            }
            
            // 检查系统配置属性
            var property = typeof(SystemGlobalconfig).GetProperty(key);
            return property != null;
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
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    if (_globalConfigWatcher != null)
                    {
                        _globalConfigWatcher.Changed -= OnGlobalConfigFileChanged;
                        _globalConfigWatcher.Dispose();
                        _globalConfigWatcher = null;
                    }
                    
                    if (_validatorConfigWatcher != null)
                    {
                        _validatorConfigWatcher.Changed -= OnValidatorConfigFileChanged;
                        _validatorConfigWatcher.Dispose();
                        _validatorConfigWatcher = null;
                    }
                    
                    // 清空订阅列表
                    _globalConfigSubscribers.Clear();
                    _validatorConfigSubscribers.Clear();
                    _dbConfigCache.Clear();
                }
                
                _disposed = true;
            }
        }
        
        /// <summary>
        /// 析构函数
        /// </summary>
        ~ConfigManager()
        {
            Dispose(false);
        }
        #endregion
    }
    
    /// <summary>
    /// 配置类型枚举
    /// </summary>
    public enum ConfigType
    {
        /// <summary>
        /// 系统全局配置
        /// </summary>
        Global,
        
        /// <summary>
        /// 验证配置
        /// </summary>
        Validator,
        
        /// <summary>
        /// 数据库配置
        /// </summary>
        Database
    }
    
    /// <summary>
    /// 配置变更事件参数
    /// </summary>
    public class ConfigChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public ConfigType ConfigType { get; set; }
        
        /// <summary>
        /// 变更后的配置对象
        /// </summary>
        public object Config { get; set; }
    }
    
    /// <summary>
    /// 用于动态参数表中的值的数据类型
    /// </summary>
    public enum ConfigValueType
    {
        [DescriptionAttribute("文本")]
        String,

        [DescriptionAttribute("整数")]
        Integer,

        [DescriptionAttribute("布尔值")]
        Boolean,

        [DescriptionAttribute("小数")]
        Decimal,

        [DescriptionAttribute("日期")]
        DateTime,

        [DescriptionAttribute("双精度浮点数")]
        Double,

        [DescriptionAttribute("单精度浮点数")]
        Float,

        [DescriptionAttribute("长整数")]
        Long,

        [DescriptionAttribute("短整数")]
        Short,

        [DescriptionAttribute("字节数组")]
        Byte,

        [DescriptionAttribute("时间跨度")]
        TimeSpan,

        [DescriptionAttribute("GUID")]
        Guid,

        [DescriptionAttribute("URI")]
        Uri,

        [DescriptionAttribute("版本号")]
        Version,

        [DescriptionAttribute("数组")]
        Array,

        [DescriptionAttribute("列表")]
        List,

        [DescriptionAttribute("字典")]
        Dictionary,

        [DescriptionAttribute("集合")]
        Collection,

        [DescriptionAttribute("对象")]
        Object
    }
}
