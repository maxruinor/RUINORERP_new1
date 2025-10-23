using RUINORERP.IServices;
using RUINORERP.Model.ConfigModel;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Services
{
    /// <summary>
    /// 配置管理服务实现
    /// 处理配置文件的加载、保存和默认值创建
    /// </summary>
    public class ConfigManagerService : IConfigManagerService
    {
        private readonly IConfigEncryptionService _encryptionService;
        private readonly ILogger<ConfigManagerService> _logger;
        private readonly string _configDirectory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptionService">加密服务</param>
        /// <param name="logger">日志记录器</param>
        public ConfigManagerService(IConfigEncryptionService encryptionService, ILogger<ConfigManagerService> logger)
        {
            _encryptionService = encryptionService;
            _logger = logger;
            // 设置配置目录路径
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
        /// </summary>
        public T LoadConfig<T>(string configType) where T : BaseConfig
        {
            try
            {
                string filePath = GetConfigFilePath(configType);
                
                if (File.Exists(filePath))
                {
                    _logger.LogInformation("加载配置文件: {FilePath}", filePath);
                    string jsonContent = File.ReadAllText(filePath);
                    T config = JsonConvert.DeserializeObject<T>(jsonContent);
                    
                    // 解密配置中的敏感字段
                    config = _encryptionService.DecryptConfig(config);
                    
                    return config;
                }
                else
                {
                    _logger.LogWarning("配置文件不存在，创建默认配置: {ConfigType}", configType);
                    // 配置文件不存在，创建默认配置
                    return CreateAndSaveDefaultConfig<T>(configType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置文件失败: {ConfigType}", configType);
                // 加载失败，返回默认配置
                return CreateAndSaveDefaultConfig<T>(configType);
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
                
                _logger.LogInformation("配置保存成功: {ConfigType}", configType);
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
                
                _logger.LogInformation("创建默认配置: {ConfigType}", typeof(T).Name);
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
            return Path.Combine(_configDirectory, $"{configType}.json");
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