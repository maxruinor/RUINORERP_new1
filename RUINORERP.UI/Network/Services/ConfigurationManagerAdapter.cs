using Microsoft.Extensions.Logging;
using RUINORERP.Business.Config;
using RUINORERP.UI.Network.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 配置管理器适配器
    /// 作为IConfigurationManager和新的配置管理服务之间的桥接器
    /// 负责将对IConfigurationManager的调用转发到新的ConfigManagerService
    /// </summary>
    public class ConfigurationManagerAdapter : IConfigurationManager
    {
        private readonly IConfigManagerService _configManagerService;
        private readonly ILogger<ConfigurationManagerAdapter> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configManagerService">新的配置管理服务</param>
        /// <param name="logger">日志记录器</param>
        public ConfigurationManagerAdapter(IConfigManagerService configManagerService, ILogger<ConfigurationManagerAdapter> logger)
        {
            _configManagerService = configManagerService ?? throw new ArgumentNullException(nameof(configManagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 保存配置到本地文件
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configData">配置数据（JSON格式）</param>
        /// <param name="version">配置版本</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveConfigurationAsync(string configType, string configData, string version)
        {
            try
            {
                _logger.LogDebug("使用配置管理器适配器保存配置: {ConfigType}", configType);
                
                // 尝试通过配置类型获取对应的配置类型
                Type configClassType = GetConfigTypeByName(configType);
                if (configClassType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configType);
                    return false;
                }
                
                // 使用反射调用对应的LoadConfigFromJsonAsync方法
                await _configManagerService.LoadConfigFromJsonAsync(configClassType, configData);
                
                // 保存版本信息（当前实现中版本信息由BaseConfig处理）
                _logger.LogDebug("配置 {ConfigType} 保存成功", configType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置 {ConfigType} 时发生错误", configType);
                return false;
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>配置数据（JSON格式）</returns>
        public async Task<string> LoadConfigurationAsync(string configType)
        {
            try
            {
                _logger.LogDebug("使用配置管理器适配器加载配置: {ConfigType}", configType);
                
                Type configClassType = GetConfigTypeByName(configType);
                if (configClassType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configType);
                    return null;
                }
                
                // 使用反射调用对应的GetConfigAsync方法
                var configObj = await _configManagerService.GetConfigAsync(configClassType);
                if (configObj == null)
                {
                    _logger.LogWarning("配置 {ConfigType} 不存在", configType);
                    return null;
                }
                
                // 序列化配置对象为JSON
                return Newtonsoft.Json.JsonConvert.SerializeObject(configObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置 {ConfigType} 时发生错误", configType);
                return null;
            }
        }

        /// <summary>
        /// 获取配置版本
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>配置版本</returns>
        public async Task<string> GetConfigurationVersionAsync(string configType)
        {
            try
            {
                _logger.LogDebug("使用配置管理器适配器获取配置版本: {ConfigType}", configType);
                
                Type configClassType = GetConfigTypeByName(configType);
                if (configClassType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configType);
                    return "0.0.0.0";
                }
                
                // 获取配置对象
                var configObj = await _configManagerService.GetConfigAsync(configClassType);
                if (configObj is BaseConfig baseConfig)
                {
                    return baseConfig.Version ?? "0.0.0.0";
                }
                
                return "0.0.0.0";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置 {ConfigType} 版本时发生错误", configType);
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// 重载配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否重载成功</returns>
        public async Task<bool> ReloadConfigurationAsync(string configType)
        {
            try
            {
                _logger.LogDebug("使用配置管理器适配器重载配置: {ConfigType}", configType);
                
                Type configClassType = GetConfigTypeByName(configType);
                if (configClassType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configType);
                    return false;
                }
                
                // 使用反射调用对应的RefreshConfigAsync方法
                await _configManagerService.RefreshConfigAsync(configClassType);
                
                _logger.LogDebug("配置 {ConfigType} 重载成功", configType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重载配置 {ConfigType} 时发生错误", configType);
                return false;
            }
        }

        /// <summary>
        /// 检查配置是否存在
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否存在</returns>
        public async Task<bool> ConfigurationExistsAsync(string configType)
        {
            try
            {
                _logger.LogDebug("使用配置管理器适配器检查配置是否存在: {ConfigType}", configType);
                
                Type configClassType = GetConfigTypeByName(configType);
                if (configClassType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configType);
                    return false;
                }
                
                // 尝试获取配置，如果成功则说明配置存在
                var configObj = await _configManagerService.GetConfigAsync(configClassType);
                return configObj != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查配置 {ConfigType} 是否存在时发生错误", configType);
                return false;
            }
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteConfigurationAsync(string configType)
        {
            try
            {
                _logger.LogDebug("使用配置管理器适配器删除配置: {ConfigType}", configType);
                
                Type configClassType = GetConfigTypeByName(configType);
                if (configClassType == null)
                {
                    _logger.LogWarning("未知的配置类型: {ConfigType}", configType);
                    return false;
                }
                
                // 构造配置文件路径
                string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RUINORERP", "Configs", $"{configType}.json");
                
                // 删除配置文件
                if (File.Exists(configFilePath))
                {
                    File.Delete(configFilePath);
                    _logger.LogDebug("配置文件 {FilePath} 删除成功", configFilePath);
                }
                
                // 也删除版本文件
                string versionFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RUINORERP", "Configs", $"{configType}.version");
                if (File.Exists(versionFilePath))
                {
                    File.Delete(versionFilePath);
                    _logger.LogDebug("版本文件 {FilePath} 删除成功", versionFilePath);
                }
                
                _logger.LogDebug("配置 {ConfigType} 删除成功", configType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除配置 {ConfigType} 时发生错误", configType);
                return false;
            }
        }

        /// <summary>
        /// 根据配置类型名称获取对应的配置类类型
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置类类型</returns>
        private Type GetConfigTypeByName(string configTypeName)
        {
            switch (configTypeName)
            {
                case "SystemGlobalConfig":
                    return typeof(SystemGlobalConfig);
                case "UIConfig":
                    return typeof(UIConfig);
                case "DBConfig":
                    return typeof(DBConfig);
                default:
                    // 尝试通过反射查找类型
                    try
                    {
                        return Type.GetType($"RUINORERP.Business.Config.{configTypeName}") ?? 
                               Type.GetType($"{configTypeName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "无法获取配置类型: {ConfigTypeName}", configTypeName);
                        return null;
                    }
            }
        }
    }
}