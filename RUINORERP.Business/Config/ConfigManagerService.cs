using RUINORERP.Model.ConfigModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置管理服务实现 - 包装泛型服务，支持配置刷新、事件通知和灵活加载
    /// </summary>
    public class ConfigManagerService : IConfigManagerService
    {
        private readonly ILogger<ConfigManagerService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _configDirectory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="configuration">配置对象</param>
        /// <param name="serviceProvider">服务提供程序，用于获取泛型配置服务</param>
        public ConfigManagerService(
            ILogger<ConfigManagerService> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            
            // 设置配置目录路径
            _configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            
            // 确保配置目录存在
            if (!Directory.Exists(_configDirectory))
            {
                Directory.CreateDirectory(_configDirectory);
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        public T GetConfig<T>(string configType) where T : BaseConfig, new()
        {
            return LoadConfig<T>(configType);
        }

        /// <summary>
        /// 获取配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        public T GetConfig<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.CurrentConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        public T LoadConfig<T>(string configType) where T : BaseConfig, new()
        {
            try
            {
                // 使用泛型服务加载配置
                var service = GetGenericConfigService<T>();
                return service.LoadConfig();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置失败: {ConfigType}", configType);
                throw;
            }
        }

        /// <summary>
        /// 加载配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        public T LoadConfig<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.LoadConfig();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 异步加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigAsync<T>(string configType) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.LoadConfigAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步加载配置失败: {ConfigType}", configType);
                throw;
            }
        }

        /// <summary>
        /// 异步加载配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigAsync<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.LoadConfigAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步加载配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 从指定路径加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        public T LoadConfigFromPath<T>(string filePath) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.LoadConfigFromPath(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从指定路径加载配置失败: {ConfigType}, 路径: {FilePath}", typeof(T).Name, filePath);
                throw;
            }
        }

        /// <summary>
        /// 异步从指定路径加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigFromPathAsync<T>(string filePath) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.LoadConfigFromPathAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步从指定路径加载配置失败: {ConfigType}, 路径: {FilePath}", typeof(T).Name, filePath);
                throw;
            }
        }

        /// <summary>
        /// 从JSON字符串加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        public T LoadConfigFromJson<T>(string jsonContent) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.LoadConfigFromJson(jsonContent);
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
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        public async Task<T> LoadConfigFromJsonAsync<T>(string jsonContent) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.LoadConfigFromJsonAsync(jsonContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步从JSON字符串加载配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>刷新后的配置对象</returns>
        public T RefreshConfig<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.RefreshConfig();
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
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>刷新后的配置对象</returns>
        public async Task<T> RefreshConfigAsync<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.RefreshConfigAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步刷新配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfig<T>(T config, string configType) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.SaveConfig(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置失败: {ConfigType}", configType);
                return false;
            }
        }

        /// <summary>
        /// 保存配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfig<T>(T config) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.SaveConfig(config);
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
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveConfigAsync<T>(T config, string configType) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.SaveConfigAsync(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步保存配置失败: {ConfigType}", configType);
                return false;
            }
        }

        /// <summary>
        /// 异步保存配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveConfigAsync<T>(T config) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return await service.SaveConfigAsync(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步保存配置失败: {ConfigType}", typeof(T).Name);
                return false;
            }
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>默认配置对象</returns>
        public T CreateDefaultConfig<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.CreateDefaultConfig();
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
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置文件是否存在</returns>
        public bool ConfigFileExists(string configType)
        {
            return File.Exists(GetConfigFilePath(configType));
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置文件路径</returns>
        public string GetConfigFilePath(string configType)
        {
            // 检查是否已经包含.json后缀，如果没有则添加
            string fileName = configType.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
                ? configType
                : $"{configType}.json";
            
            return Path.Combine(_configDirectory, fileName);
        }

        /// <summary>
        /// 重置为默认配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>重置后的默认配置对象</returns>
        public T ResetToDefault<T>(string configType) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.ResetToDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置配置失败: {ConfigType}", configType);
                throw;
            }
        }

        /// <summary>
        /// 重置为默认配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>重置后的默认配置对象</returns>
        public T ResetToDefault<T>() where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                return service.ResetToDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置配置失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }
        
        /// <summary>
        /// 解析环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径字符串</param>
        /// <returns>解析后的路径</returns>
        public string ResolveEnvironmentVariables(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
                
            try
            {
                return Environment.ExpandEnvironmentVariables(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析环境变量失败: {Path}", path);
                return path;
            }
        }

        /// <summary>
        /// 注册配置变更事件处理器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="handler">配置变更事件处理器</param>
        public void RegisterConfigChangedHandler<T>(EventHandler<ConfigChangedEventArgs<T>> handler) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                service.ConfigChanged += handler;
                _logger.LogDebug("已注册配置变更事件处理器: {ConfigType}", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册配置变更事件处理器失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 注销配置变更事件处理器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="handler">配置变更事件处理器</param>
        public void UnregisterConfigChangedHandler<T>(EventHandler<ConfigChangedEventArgs<T>> handler) where T : BaseConfig, new()
        {
            try
            {
                var service = GetGenericConfigService<T>();
                service.ConfigChanged -= handler;
                _logger.LogDebug("已注销配置变更事件处理器: {ConfigType}", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注销配置变更事件处理器失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }
        
        /// <summary>
        /// 获取泛型配置服务
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>泛型配置服务</returns>
        private IGenericConfigService<T> GetGenericConfigService<T>() where T : BaseConfig, new()
        {
            try
            {
                var genericServiceType = typeof(IGenericConfigService<>).MakeGenericType(typeof(T));
                var service = _serviceProvider.GetService(genericServiceType) as IGenericConfigService<T>;
                
                if (service == null)
                {
                    throw new InvalidOperationException($"未找到类型 {typeof(T).Name} 的配置服务");
                }
                
                return service;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取泛型配置服务失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }
    }
}
