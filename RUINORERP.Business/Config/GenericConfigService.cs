using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 通用配置服务实现
    /// 提供对特定类型配置的通用操作实现，支持配置刷新、事件通知和灵活加载
    /// </summary>
    /// <typeparam name="T">配置类型，必须继承自BaseConfig且有公共无参构造函数</typeparam>
    public class GenericConfigService<T> : IGenericConfigService<T> where T : BaseConfig, new()
    {
        private readonly IConfigEncryptionService _encryptionService;
        private readonly IConfigValidationService _validationService;
        private readonly ILogger<GenericConfigService<T>> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _configDirectory;
        private readonly string _configFileName;
        private T _currentConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptionService">加密服务</param>
        /// <param name="validationService">验证服务</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="configuration">配置对象</param>
        public GenericConfigService(
            IConfigEncryptionService encryptionService,
            IConfigValidationService validationService,
            ILogger<GenericConfigService<T>> logger,
            IConfiguration configuration)
        {
            _encryptionService = encryptionService;
            _validationService = validationService;
            _logger = logger;
            _configuration = configuration;
            
            // 统一配置目录
            _configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            _configFileName = $"{typeof(T).Name}.json";
            
            EnsureConfigDirectoryExists();
            
            // 初始化当前配置
            _currentConfig = LoadConfig();
        }
        
        /// <summary>
        /// 配置变更事件
        /// </summary>
        public event EventHandler<ConfigChangedEventArgs<T>> ConfigChanged;
        
        /// <summary>
        /// 获取当前内存中的配置对象
        /// </summary>
        public T CurrentConfig
        {
            get { return _currentConfig; }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns>配置对象</returns>
        public T LoadConfig()
        {
            try
            {
                _logger.LogDebug("加载配置: {ConfigType}", typeof(T).Name);
                
                // 优先从IConfiguration加载
                var configSection = _configuration.GetSection(typeof(T).Name);
                if (configSection != null && configSection.Exists())
                {
                    T config = new T();
                    configSection.Bind(config);
                    config = _encryptionService.DecryptConfig(config);
                    
                    // 触发配置变更事件
                    OnConfigChanged(config, _currentConfig, "从IConfiguration加载");
                    _currentConfig = config;
                    return config;
                }
                
                // 后备：从文件加载
                return LoadConfigFromFile();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置失败: {ConfigType}", typeof(T).Name);
                return CreateAndSaveDefaultConfig();
            }
        }

        /// <summary>
        /// 异步加载配置
        /// </summary>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigAsync()
        {
            return await Task.Run(() => LoadConfig());
        }
        
        /// <summary>
        /// 从指定路径加载配置
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        public T LoadConfigFromPath(string filePath)
        {
            try
            {
                _logger.LogDebug("从指定路径加载配置: {FilePath}", filePath);
                
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath), "配置文件路径不能为空");
                }
                
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("配置文件不存在", filePath);
                }
                
                string jsonContent = File.ReadAllText(filePath);
                T config = JsonConvert.DeserializeObject<T>(jsonContent);
                config = _encryptionService.DecryptConfig(config);
                
                // 验证配置
                var validationResult = ValidateConfig(config);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("从路径加载的配置验证失败: {Errors}", validationResult.GetErrorMessage());
                    throw new InvalidOperationException($"配置验证失败: {validationResult.GetErrorMessage()}");
                }
                
                // 触发配置变更事件
                OnConfigChanged(config, _currentConfig, $"从路径 {filePath} 加载");
                _currentConfig = config;
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从指定路径加载配置失败: {FilePath}", filePath);
                throw;
            }
        }
        
        /// <summary>
        /// 异步从指定路径加载配置
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigFromPathAsync(string filePath)
        {
            return await Task.Run(() => LoadConfigFromPath(filePath));
        }
        
        /// <summary>
        /// 从JSON字符串加载配置
        /// </summary>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        public T LoadConfigFromJson(string jsonContent)
        {
            try
            {
                _logger.LogDebug("从JSON字符串加载配置: {ConfigType}", typeof(T).Name);
                
                if (string.IsNullOrEmpty(jsonContent))
                {
                    throw new ArgumentNullException(nameof(jsonContent), "JSON内容不能为空");
                }
                
                T config = JsonConvert.DeserializeObject<T>(jsonContent);
                config = _encryptionService.DecryptConfig(config);
                
                // 验证配置
                var validationResult = ValidateConfig(config);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("从JSON加载的配置验证失败: {Errors}", validationResult.GetErrorMessage());
                    throw new InvalidOperationException($"配置验证失败: {validationResult.GetErrorMessage()}");
                }
                
                // 触发配置变更事件
                OnConfigChanged(config, _currentConfig, "从JSON字符串加载");
                _currentConfig = config;
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从JSON字符串加载配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }
        
        /// <summary>
        /// 异步从JSON字符串加载配置
        /// </summary>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigFromJsonAsync(string jsonContent)
        {
            return await Task.Run(() => LoadConfigFromJson(jsonContent));
        }
        
        /// <summary>
        /// 刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <returns>刷新后的配置对象</returns>
        public T RefreshConfig()
        {
            try
            {
                _logger.LogDebug("刷新配置: {ConfigType}", typeof(T).Name);
                return LoadConfig();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }
        
        /// <summary>
        /// 异步刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <returns>刷新后的配置对象</returns>
        public async Task<T> RefreshConfigAsync()
        {
            return await Task.Run(() => RefreshConfig());
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config">要保存的配置对象</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfig(T config)
        {
            try
            {
                if (config == null)
                {
                    _logger.LogWarning("保存配置失败：配置对象为空");
                    return false;
                }

                // 验证配置
                var validationResult = ValidateConfig(config);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("配置验证失败: {Errors}", validationResult.GetErrorMessage());
                    return false;
                }

                // 加密敏感字段
                T encryptedConfig = _encryptionService.EncryptConfig(config);
                
                // 保存到文件 - 注意：保存格式需要与IOptionsMonitor期望的格式一致
                // IOptionsMonitor期望配置文件包含类型名称作为根节点
                string filePath = GetConfigFilePath();
                // 创建包含类型名称的包装对象，确保与IOptionsMonitor的配置结构一致
                var configWrapper = new Dictionary<string, T> { { typeof(T).Name, encryptedConfig } };
                string jsonContent = JsonConvert.SerializeObject(configWrapper, Formatting.Indented);
                File.WriteAllText(filePath, jsonContent);
                
                // 触发配置变更事件
                OnConfigChanged(config, _currentConfig, "配置保存");
                _currentConfig = config;
                
                _logger.LogDebug("配置保存成功: {ConfigType}", typeof(T).Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置失败: {ConfigType}", typeof(T).Name);
                return false;
            }
        }

        /// <summary>
        /// 异步保存配置
        /// </summary>
        /// <param name="config">要保存的配置对象</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveConfigAsync(T config)
        {
            return await Task.Run(() => SaveConfig(config));
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <returns>默认配置对象</returns>
        public T CreateDefaultConfig()
        {
            try
            {
                T config = new T();
                config.InitDefault();
                _logger.LogDebug("创建默认配置: {ConfigType}", typeof(T).Name);
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建默认配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 重置为默认配置
        /// </summary>
        /// <returns>重置后的默认配置对象</returns>
        public T ResetToDefault()
        {
            T defaultConfig = CreateAndSaveDefaultConfig();
            
            // 触发配置变更事件
            OnConfigChanged(defaultConfig, _currentConfig, "重置为默认配置");
            _currentConfig = defaultConfig;
            
            return defaultConfig;
        }

        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="config">要验证的配置对象</param>
        /// <returns>验证结果</returns>
        public ConfigValidationResult ValidateConfig(T config)
        {
            return _validationService.ValidateConfig(config);
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns>配置文件路径</returns>
        public string GetConfigFilePath()
        {
            return Path.Combine(_configDirectory, _configFileName);
        }

        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <returns>配置对象</returns>
        private T LoadConfigFromFile()
        {
            string filePath = GetConfigFilePath();
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("配置文件不存在，创建默认配置: {FilePath}", filePath);
                return CreateAndSaveDefaultConfig();
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                
                // 尝试以包含根节点的格式读取配置
                // 格式: { "TypeName": { ...配置内容... } }
                try
                {
                    var configWrapper = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonContent);
                    if (configWrapper != null && configWrapper.TryGetValue(typeof(T).Name, out T config))
                    {
                        config = _encryptionService.DecryptConfig(config);
                        
                        // 触发配置变更事件
                        OnConfigChanged(config, _currentConfig, "从文件加载(带根节点格式)");
                        _currentConfig = config;
                        
                        return config;
                    }
                }
                catch (Exception innerEx)
                {
                    _logger.LogDebug(innerEx, "以带根节点格式读取配置失败，尝试直接读取: {ConfigType}", typeof(T).Name);
                }
                
                // 如果带根节点格式读取失败，尝试直接读取（向后兼容旧格式）
                T Lastconfig = JsonConvert.DeserializeObject<T>(jsonContent);
                Lastconfig = _encryptionService.DecryptConfig(Lastconfig);
                
                // 触发配置变更事件
                OnConfigChanged(Lastconfig, _currentConfig, "从文件加载(直接格式)");
                _currentConfig = Lastconfig;
                
                return Lastconfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从文件加载配置失败: {FilePath}", filePath);
                return CreateAndSaveDefaultConfig();
            }
        }

        /// <summary>
        /// 创建并保存默认配置
        /// </summary>
        /// <returns>默认配置对象</returns>
        private T CreateAndSaveDefaultConfig()
        {
            T config = CreateDefaultConfig();
            SaveConfig(config);
            return config;
        }

        /// <summary>
        /// 确保配置目录存在
        /// </summary>
        private void EnsureConfigDirectoryExists()
        {
            if (!Directory.Exists(_configDirectory))
            {
                Directory.CreateDirectory(_configDirectory);
            }
        }
        
        /// <summary>
        /// 触发配置变更事件
        /// </summary>
        /// <param name="newConfig">新的配置对象</param>
        /// <param name="oldConfig">旧的配置对象</param>
        /// <param name="reason">变更原因</param>
        protected virtual void OnConfigChanged(T newConfig, T oldConfig, string reason)
        {
            try
            {
                ConfigChanged?.Invoke(this, new ConfigChangedEventArgs<T>
                {
                    NewConfig = newConfig,
                    OldConfig = oldConfig,
                    Reason = reason
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "触发配置变更事件失败: {ConfigType}", typeof(T).Name);
                // 不抛出异常，避免影响主流程
            }
        }
    }
}