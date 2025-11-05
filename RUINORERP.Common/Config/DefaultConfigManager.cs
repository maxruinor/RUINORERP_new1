using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 默认配置管理器实现（简化版）
    /// 提供配置的加载、保存功能
    /// </summary>
    public class DefaultConfigManager : IConfigManager
    {
        /// <summary>
        /// 配置路径解析器
        /// </summary>
        private readonly IConfigPathResolver _pathResolver;
        
        /// <summary>
        /// JSON序列化设置
        /// </summary>
        private readonly JsonSerializerSettings _jsonSettings;
        
        /// <summary>
        /// 初始化默认配置管理器
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        public DefaultConfigManager(IConfigPathResolver pathResolver)
        {
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
            
            // 配置JSON序列化设置
            _jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
        
        /// <summary>
        /// 初始化配置管理器
        /// 确保必要的目录结构存在
        /// </summary>
        public void Initialize()
        {
            // 确保所有配置目录存在
            _pathResolver.EnsureConfigDirectoryExists(ConfigPathType.Server);
            _pathResolver.EnsureConfigDirectoryExists(ConfigPathType.Client);
        }
        
        /// <summary>
        /// 加载指定类型的配置
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置对象实例</returns>
        public TConfig LoadConfig<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new()
        {
            string typeName = GetConfigTypeName<TConfig>(configTypeName);
            string filePath = _pathResolver.GetConfigFilePath(typeName, configType);
            TConfig config = new TConfig();
            
            // 如果文件存在，则加载
            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    config = JsonConvert.DeserializeObject<TConfig>(jsonContent, _jsonSettings);
                }
                catch (Exception ex)
                {
                    // 记录错误但不抛出异常，返回默认配置
                    Console.WriteLine($"加载配置失败: {filePath}, 错误: {ex.Message}");
                }
            }
            
            return config;
        }
        
        /// <summary>
        /// 异步加载指定类型的配置
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置对象实例的任务</returns>
        public async Task<TConfig> LoadConfigAsync<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new()
        {
            return await Task.Run(() => LoadConfig<TConfig>(configType, configTypeName));
        }
        
        /// <summary>
        /// 保存配置到文件
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="config">配置对象实例</param>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        public void SaveConfig<TConfig>(TConfig config, ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            string typeName = GetConfigTypeName<TConfig>(configTypeName);
            string filePath = _pathResolver.GetConfigFilePath(typeName, configType);
            
            try
            {
                // 确保目录存在
                _pathResolver.EnsureConfigDirectoryExists(configType);
                
                // 序列化并保存
                string jsonContent = JsonConvert.SerializeObject(config, _jsonSettings);
                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new IOException($"保存配置失败: {filePath}", ex);
            }
        }
        
        /// <summary>
        /// 异步保存配置到文件
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="config">配置对象实例</param>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>保存任务</returns>
        public async Task SaveConfigAsync<TConfig>(TConfig config, ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class
        {
            await Task.Run(() => SaveConfig(config, configType, configTypeName));
        }
        
        /// <summary>
        /// 刷新配置
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>刷新后的配置对象</returns>
        public TConfig RefreshConfig<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new()
        {
            // 重新加载
            return LoadConfig<TConfig>(configType, configTypeName);
        }
        
        /// <summary>
        /// 异步刷新配置
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>刷新后的配置对象的任务</returns>
        public async Task<TConfig> RefreshConfigAsync<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new()
        {
            return await Task.Run(() => RefreshConfig<TConfig>(configType, configTypeName));
        }
        
        /// <summary>
        /// 验证配置
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="config">配置对象实例</param>
        /// <returns>配置是否有效</returns>
        public bool ValidateConfig<TConfig>(TConfig config) where TConfig : class
        {
            if (config == null)
            {
            }
            
            // 基本验证：尝试序列化以确保配置对象可序列化
            try
            {
                string json = JsonConvert.SerializeObject(config, _jsonSettings);
                return !string.IsNullOrEmpty(json);
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 获取配置类型名称
        /// 如果未指定，则使用类型的名称
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置类型名称</returns>
        private string GetConfigTypeName<TConfig>(string configTypeName)
        {
            return string.IsNullOrEmpty(configTypeName) ? typeof(TConfig).Name : configTypeName;
        }
    }
}