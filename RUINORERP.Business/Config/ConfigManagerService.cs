using RUINORERP.Model.ConfigModel;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置管理服务实现
    /// 处理配置文件的加载、保存和默认值创建
    /// </summary>
    public class ConfigManagerService : IConfigManagerService
    {
        private readonly IConfigEncryptionService _encryptionService;
        private readonly ILogger<ConfigManagerService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _configDirectory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptionService">加密服务</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="configuration">配置对象</param>
        public ConfigManagerService(IConfigEncryptionService encryptionService, ILogger<ConfigManagerService> logger, IConfiguration configuration)
        {
            _encryptionService = encryptionService;
            _logger = logger;
            _configuration = configuration;
            // 设置配置目录路径（仅用于保存配置文件）
            _configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            
            // 确保配置目录存在
            if (!Directory.Exists(_configDirectory))
            {
                Directory.CreateDirectory(_configDirectory);
            }
        }

        public T GetConfig<T>(string configType) where T : BaseConfig
        {
            return LoadConfig<T>(configType);
        }

        /// <summary>
        /// 加载配置文件
        /// 优先使用IConfiguration进行配置加载，提高性能并支持热重载
        /// </summary>
        public T LoadConfig<T>(string configType) where T : BaseConfig
        {
            try
            {
                _logger.Debug("从IConfiguration加载配置: {ConfigType}", configType);
                
                // 从IConfiguration中获取配置节
                var configSection = _configuration.GetSection(configType);
                if (configSection != null && configSection.Exists())
                {
                    T config = Activator.CreateInstance<T>();
                    configSection.Bind(config);
                    
                    // 解密配置中的敏感字段
                    config = _encryptionService.DecryptConfig(config);
                    
                    // 解析环境变量
                    if (config is ServerConfig serverConfig)
                    {
                        serverConfig.FileStoragePath = ResolveEnvironmentVariables(serverConfig.FileStoragePath);
                    }
                    
                    _logger.Debug("配置加载成功: {ConfigType}", configType);
                    return config;
                }
                
                // 如果IConfiguration中没有找到配置，尝试从文件加载（作为后备）
                _logger.LogWarning("IConfiguration中未找到配置，尝试从文件加载: {ConfigType}", configType);
                return LoadConfigFromFile<T>(configType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置失败: {ConfigType}", configType);
                // 加载失败，返回默认配置
                return CreateAndSaveDefaultConfig<T>(configType);
            }
        }
        
        /// <summary>
        /// 从文件加载配置（作为后备方案）
        /// </summary>
        private T LoadConfigFromFile<T>(string configType) where T : BaseConfig
        {
            string filePath = GetConfigFilePath(configType);
            
            if (File.Exists(filePath))
            {
                _logger.Debug("从文件加载配置: {FilePath}", filePath);
                string jsonContent = File.ReadAllText(filePath);
                
                try
                {
                    // 尝试直接反序列化
                    T config = JsonConvert.DeserializeObject<T>(jsonContent);
                    
                    // 解密配置中的敏感字段
                    config = _encryptionService.DecryptConfig(config);
                    
                    // 解析环境变量
                    if (config is ServerConfig serverConfig)
                    {
                        serverConfig.FileStoragePath = ResolveEnvironmentVariables(serverConfig.FileStoragePath);
                    }
                    
                    return config;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "文件反序列化失败: {FilePath}", filePath);
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// 解析路径中的环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的实际路径</returns>
        public string ResolveEnvironmentVariables(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
                
            try
            {
                // 使用环境变量展开路径中的%ENV_VAR%格式变量
                return Environment.ExpandEnvironmentVariables(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析环境变量失败: {Path}", path);
                return path; // 解析失败时返回原始路径
            }
        }

        /// <summary>
        /// 异步加载配置文件
        /// </summary>
        public async Task<T> LoadConfigAsync<T>(string configType) where T : BaseConfig
        {
            return await Task.Run(() => LoadConfig<T>(configType));
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public bool SaveConfig<T>(T config, string configType) where T : BaseConfig
        {
            try
            {
                if (config == null)
                {
                    _logger.LogWarning("保存配置失败：配置对象为空");
                    return false;
                }



                // 加密配置中的敏感字段
                T encryptedConfig = _encryptionService.EncryptConfig(config);

                string filePath = GetConfigFilePath(configType);
                
                // 保存配置到文件
                string jsonContent = JsonConvert.SerializeObject(encryptedConfig, Formatting.Indented);
                File.WriteAllText(filePath, jsonContent);
                
                _logger.Debug("配置保存成功: {ConfigType}", configType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置文件失败: {ConfigType}", configType);
                return false;
            }
        }

        /// <summary>
        /// 异步保存配置文件
        /// </summary>
        public async Task<bool> SaveConfigAsync<T>(T config, string configType) where T : BaseConfig
        {
            return await Task.Run(() => SaveConfig(config, configType));
        }

        /// <summary>
        /// 创建配置默认值
        /// </summary>
        public T CreateDefaultConfig<T>() where T : BaseConfig
        {
            try
            {
                // 使用反射创建实例
                T config = Activator.CreateInstance<T>();
                

                
                // 尝试调用InitDefault方法（如果存在）
                MethodInfo initMethod = typeof(T).GetMethod("InitDefault", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (initMethod != null && initMethod.GetParameters().Length == 0)
                {
                    initMethod.Invoke(config, null);
                }
                
                _logger.Debug("创建默认配置: {ConfigType}", typeof(T).Name);
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建默认配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 检查配置文件是否存在
        /// </summary>
        public bool ConfigFileExists(string configType)
        {
            return File.Exists(GetConfigFilePath(configType));
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        public string GetConfigFilePath(string configType)
        {
            // 检查是否已经包含.json后缀，如果没有则添加
            string fileName = configType.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
                ? configType
                : $"{configType}.json";
            
            return Path.Combine(_configDirectory, fileName);
        }

        /// <summary>
        /// 重置配置为默认值
        /// </summary>
        public T ResetToDefault<T>(string configType) where T : BaseConfig
        {
            // 创建默认配置并保存
            return CreateAndSaveDefaultConfig<T>(configType);
        }

        /// <summary>
        /// 创建并保存默认配置
        /// </summary>
        private T CreateAndSaveDefaultConfig<T>(string configType) where T : BaseConfig
        {
            T config = CreateDefaultConfig<T>();
            SaveConfig(config, configType);
            return config;
        }
    }
}
