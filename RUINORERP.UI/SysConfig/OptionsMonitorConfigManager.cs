using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model.ConfigModel;
using RUINORERP.UI.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 集成IOptionsMonitor的配置管理器
    /// 负责三种配置实体类(SystemGlobalConfig, ServerConfig, GlobalValidatorConfig)的管理、同步和热更新
    /// </summary>
    public class OptionsMonitorConfigManager : IDisposable
    {
        private readonly ILogger<OptionsMonitorConfigManager> _logger;
        private readonly IConfigurationManager _networkConfigManager;
        private readonly string _configDirectory;
        
        // 配置变更订阅令牌
        private readonly List<IDisposable> _changeTokens = new List<IDisposable>();
        
        // 配置缓存
        private SystemGlobalConfig _currentGlobalConfig;
        private ServerConfig _currentServerConfig;
        private GlobalValidatorConfig _currentValidatorConfig;
        
        /// <summary>
        /// 获取当前系统全局配置
        /// </summary>
        public SystemGlobalConfig GlobalConfig => _currentGlobalConfig;
        
        /// <summary>
        /// 获取当前服务器配置
        /// </summary>
        public ServerConfig ServerConfig => _currentServerConfig;
        
        /// <summary>
        /// 获取当前验证配置
        /// </summary>
        public GlobalValidatorConfig ValidatorConfig => _currentValidatorConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="networkConfigManager">网络配置管理器</param>
        /// <param name="globalConfigMonitor">全局配置监控器</param>
        /// <param name="serverConfigMonitor">服务器配置监控器</param>
        /// <param name="validatorConfigMonitor">验证配置监控器</param>
        public OptionsMonitorConfigManager(
            ILogger<OptionsMonitorConfigManager> logger,
            IConfigurationManager networkConfigManager
             )
        {
            _logger = logger;
            _networkConfigManager = networkConfigManager;
            
            // 获取应用程序数据目录作为配置存储位置
            _configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RUINORERP", "Configs");
            Directory.CreateDirectory(_configDirectory);
            
            // 初始化配置
            Initialize();
        }
        
        /// <summary>
        /// 初始化配置管理器
        /// </summary>
        private void Initialize()
        {
            // 加载初始配置
            LoadInitialConfigurations().GetAwaiter().GetResult();
            
            // 订阅配置变更
            SubscribeToConfigChanges();
            
            // 从服务器请求最新配置
            RequestLatestConfigurations().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// 加载初始配置
        /// </summary>
        private async Task LoadInitialConfigurations()
        {
            try
            {
                // 加载本地配置文件
                _currentGlobalConfig = LoadConfigFromFile<SystemGlobalConfig>() ?? new SystemGlobalConfig();
                _currentServerConfig = LoadConfigFromFile<ServerConfig>() ?? new ServerConfig();
                _currentValidatorConfig = LoadConfigFromFile<GlobalValidatorConfig>() ?? new GlobalValidatorConfig();
                
                _logger.LogDebug("初始配置加载完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载初始配置失败");
            }
        }
        
        /// <summary>
        /// 订阅配置变更
        /// 在当前实现中，配置变更通过内部方法和通用命令处理器进行处理
        /// </summary>
        private void SubscribeToConfigChanges()
        {
            // 配置变更现在通过内部方法如UpdateConfig进行处理
            // 不再依赖外部的IOptionsMonitor
            _logger.LogDebug("配置管理器已初始化，配置变更将通过内部方法和通用命令处理器处理");
        }
        
        /// <summary>
        /// 更新全局配置
        /// </summary>
        /// <param name="config">新的全局配置</param>
        public void UpdateGlobalConfig(SystemGlobalConfig config)
        {
            _currentGlobalConfig = config;
            SaveConfigToFile(config);
            _logger.LogDebug("全局配置已更新");
        }
        
        /// <summary>
        /// 更新服务器配置
        /// </summary>
        /// <param name="config">新的服务器配置</param>
        public void UpdateServerConfig(ServerConfig config)
        {
            _currentServerConfig = config;
            SaveConfigToFile(config);
            _logger.LogDebug("服务器配置已更新");
        }
        
        /// <summary>
        /// 更新验证配置
        /// </summary>
        /// <param name="config">新的验证配置</param>
        public void UpdateValidatorConfig(GlobalValidatorConfig config)
        {
            _currentValidatorConfig = config;
            SaveConfigToFile(config);
            _logger.LogDebug("验证配置已更新");
        }
        
        /// <summary>
        /// 从服务器请求最新配置
        /// </summary>
        private async Task RequestLatestConfigurations()
        {
            try
            {
                // 注意：这里我们仍然使用配置相关的请求，但在新的架构中，这些应该通过通用数据传输机制处理
                _logger.LogWarning("配置管理器仍在使用旧的配置请求机制，应迁移到通用数据传输");
                
                // 在新的实现中，这些请求应该通过通用命令处理器发送
                // await _generalCommandHandler.SendDataTransferRequestAsync(nameof(SystemGlobalConfig), "");
                
                _logger.LogDebug("已请求最新服务器配置");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "请求服务器配置失败，将使用本地配置");
            }
        }
        
        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        private T LoadConfigFromFile<T>() where T : class, new()
        {            
            try
            {
                string configType = typeof(T).Name;
                string configPath = Path.Combine(_configDirectory, $"{configType}.json");
                
                if (!File.Exists(configPath))
                {
                    _logger.LogDebug($"配置文件不存在: {configType}");
                    return null;
                }
                
                // 使用UTF8编码读取文件，确保中文能正确解析
                string jsonContent = File.ReadAllText(configPath, Encoding.UTF8);
                
                // 尝试直接解析，如果失败则尝试提取Config属性
                try
                {
                    return JsonConvert.DeserializeObject<T>(jsonContent);
                }
                catch
                {
                    // 尝试从包含Config属性的对象中提取
                    try
                    {
                        var wrapper = JsonConvert.DeserializeObject<ConfigWrapper<T>>(jsonContent);
                        return wrapper?.Config;
                    }
                    catch
                    {
                        _logger.LogWarning($"解析配置文件失败，尝试其他格式: {configType}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"从文件加载配置 {typeof(T).Name} 失败");
                return null;
            }
        }
        
        /// <summary>
        /// 保存配置到文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        private void SaveConfigToFile<T>(T config) where T : class
        {            
            try
            {
                string configType = typeof(T).Name;
                string configPath = Path.Combine(_configDirectory, $"{configType}.json");
                
                // 统一配置文件格式，使用Newtonsoft.Json确保中文能正确序列化
                string jsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPath, jsonContent, Encoding.UTF8);
                
                _logger.LogDebug($"配置 {configType} 已保存到文件");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存配置 {typeof(T).Name} 到文件失败");
                throw;
            }
        }
        
        /// <summary>
        /// 处理从服务器接收的配置同步
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configJson">配置JSON数据</param>
        public void HandleConfigSync(string configType, string configJson)
        {
            try
            {
                // 确保配置数据不为空
                if (string.IsNullOrEmpty(configJson))
                {
                    throw new ArgumentNullException(nameof(configJson), "配置数据不能为空");
                }
                
                _logger.LogDebug($"处理配置同步: {configType}");
                
                switch (configType)
                {
                    case nameof(SystemGlobalConfig):
                        var globalConfig = JsonConvert.DeserializeObject<SystemGlobalConfig>(configJson);
                        if (globalConfig != null)
                        {
                            UpdateGlobalConfig(globalConfig);
                        }
                        break;
                    case nameof(ServerConfig):
                        var serverConfig = JsonConvert.DeserializeObject<ServerConfig>(configJson);
                        if (serverConfig != null)
                        {
                            UpdateServerConfig(serverConfig);
                        }
                        break;
                    case nameof(GlobalValidatorConfig):
                        var validatorConfig = JsonConvert.DeserializeObject<GlobalValidatorConfig>(configJson);
                        if (validatorConfig != null)
                        {
                            UpdateValidatorConfig(validatorConfig);
                        }
                        break;
                    default:
                        // 对于其他类型配置，委托给ConfigManager处理
                        if (ConfigManager.Instance != null)
                        {
                            _logger.LogDebug($"将未知配置类型 {configType} 转发给ConfigManager处理");
                            ConfigManager.Instance.HandleConfigSync(configType, configJson);
                        }
                        else
                        {
                            _logger.LogWarning($"未知的配置类型: {configType} 且ConfigManager不可用");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理配置同步 {configType} 失败: {ex.Message}");
                throw;
            }
        }
        
        // UpdateConfigMonitor方法已移除，配置更新现在通过UpdateGlobalConfig、UpdateServerConfig和UpdateValidatorConfig方法处理
        
        /// <summary>
        /// 手动刷新所有配置
        /// </summary>
        public async Task RefreshAllConfigurationsAsync()
        {
            await RequestLatestConfigurations();
        }
        
        /// <summary>
        /// 配置包装器类
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        private class ConfigWrapper<T> where T : class
        {
            public T Config { get; set; }
            public string ConfigName { get; set; }
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 清理变更订阅令牌（如果有）
            foreach (var token in _changeTokens)
            {
                token.Dispose();
            }
            _changeTokens.Clear();
            
            _logger.LogDebug("配置管理器资源已释放");
        }
    }
}