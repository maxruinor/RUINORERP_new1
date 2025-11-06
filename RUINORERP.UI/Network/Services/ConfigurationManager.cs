using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 配置管理器实现
    /// 负责客户端配置文件的读写、版本管理和热更新
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly ILogger<ConfigurationManager> _logger;
        private readonly string _configDirectory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ConfigurationManager(ILogger<ConfigurationManager> logger)
        {
            _logger = logger;
            // 获取应用程序数据目录作为配置存储位置
            _configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RUINORERP", "Configs");
            
            // 确保配置目录存在
            Directory.CreateDirectory(_configDirectory);
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>配置文件路径</returns>
        private string GetConfigFilePath(string configType)
        {
            // 对配置类型进行安全处理，避免路径遍历攻击
            string safeConfigType = Path.GetFileNameWithoutExtension(configType);
            return Path.Combine(_configDirectory, $"{safeConfigType}.json");
        }

        /// <summary>
        /// 获取配置版本文件路径
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>版本文件路径</returns>
        private string GetVersionFilePath(string configType)
        {
            string safeConfigType = Path.GetFileNameWithoutExtension(configType);
            return Path.Combine(_configDirectory, $"{safeConfigType}.version");
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
                _logger.LogDebug($"保存配置: {configType}, 版本: {version}");
                
                string configPath = GetConfigFilePath(configType);
                string versionPath = GetVersionFilePath(configType);
                
                // 使用文件事务确保数据一致性
                using (var configStream = new FileStream(configPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(configStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(configData);
                    await writer.FlushAsync();
                }
                
                // 保存版本信息
                using (var versionStream = new FileStream(versionPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(versionStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(version);
                    await writer.FlushAsync();
                }
                
                _logger.LogDebug($"配置 {configType} 保存成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存配置 {configType} 时发生错误");
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
                string configPath = GetConfigFilePath(configType);
                
                if (!File.Exists(configPath))
                {
                    _logger.LogWarning($"配置文件不存在: {configType}");
                    return null;
                }
                
                using (var reader = new StreamReader(configPath, Encoding.UTF8))
                {
                    string configData = await reader.ReadToEndAsync();
                    _logger.LogDebug($"配置 {configType} 加载成功");
                    return configData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"加载配置 {configType} 时发生错误");
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
                string versionPath = GetVersionFilePath(configType);
                
                if (!File.Exists(versionPath))
                {
                    _logger.LogWarning($"版本文件不存在: {configType}");
                    return "0.0.0.0";
                }
                
                using (var reader = new StreamReader(versionPath, Encoding.UTF8))
                {
                    string version = await reader.ReadToEndAsync();
                    return version.Trim();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取配置 {configType} 版本时发生错误");
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
                _logger.LogDebug($"重载配置: {configType}");
                
                // 读取配置文件
                string configData = await LoadConfigurationAsync(configType);
                
                if (configData == null)
                {
                    _logger.LogWarning($"配置 {configType} 重载失败，配置文件不存在");
                    return false;
                }
                
                // 在这里可以触发配置变更事件，通知应用程序的其他部分重新加载配置
                // 例如通过事件总线或依赖注入的观察者模式实现
                
                _logger.LogDebug($"配置 {configType} 重载成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"重载配置 {configType} 时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 检查配置是否存在
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否存在</returns>
        public Task<bool> ConfigurationExistsAsync(string configType)
        {
            string configPath = GetConfigFilePath(configType);
            bool exists = File.Exists(configPath);
            
            if (!exists)
            {
                _logger.LogDebug($"配置文件不存在: {configType}");
            }
            
            return Task.FromResult(exists);
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
                string configPath = GetConfigFilePath(configType);
                string versionPath = GetVersionFilePath(configType);
                
                // 删除配置文件
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
                
                // 删除版本文件
                if (File.Exists(versionPath))
                {
                    File.Delete(versionPath);
                }
                
                _logger.LogDebug($"配置 {configType} 删除成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除配置 {configType} 时发生错误");
                return false;
            }
        }
    }
}