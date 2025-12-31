using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.ManagementServer.ConfigurationManagement
{
    /// <summary>
    /// 配置管理核心类
    /// 负责配置信息的存储、检索和更新
    /// </summary>
    public class ConfigurationManager
    {
        // 配置信息字典，键为配置名称，值为配置值
        private ConcurrentDictionary<string, ConfigurationInfo> _configurations;
        
        /// <summary>
        /// 配置更新事件
        /// </summary>
        public event EventHandler<ConfigurationUpdatedEventArgs> ConfigurationUpdated;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigurationManager()
        {
            _configurations = new ConcurrentDictionary<string, ConfigurationInfo>();
            
            // 初始化默认配置
            InitializeDefaultConfigurations();
        }
        
        /// <summary>
        /// 初始化默认配置
        /// </summary>
        private void InitializeDefaultConfigurations()
        {
            // 从App.config加载配置
            var appSettings = ConfigurationManager.AppSettings;
            foreach (var key in appSettings.AllKeys)
            {
                AddOrUpdateConfiguration(key, appSettings[key], "从App.config加载的默认配置");
            }
        }
        
        /// <summary>
        /// 添加或更新配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="value">配置值</param>
        /// <param name="description">配置描述</param>
        public void AddOrUpdateConfiguration(string name, string value, string description = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "配置名称不能为空");
            }
            
            var configInfo = new ConfigurationInfo
            {
                Name = name,
                Value = value,
                Description = description,
                LastUpdateTime = DateTime.Now
            };
            
            // 添加或更新配置
            _configurations.AddOrUpdate(name, configInfo, (key, oldValue) =>
            {
                oldValue.Value = value;
                oldValue.Description = description;
                oldValue.LastUpdateTime = DateTime.Now;
                return oldValue;
            });
            
            // 触发配置更新事件
            ConfigurationUpdated?.Invoke(this, new ConfigurationUpdatedEventArgs(configInfo));
        }
        
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns>配置信息</returns>
        public ConfigurationInfo GetConfiguration(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "配置名称不能为空");
            }
            
            _configurations.TryGetValue(name, out ConfigurationInfo configInfo);
            return configInfo;
        }
        
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值，如果不存在则返回默认值</returns>
        public string GetConfigurationValue(string name, string defaultValue = null)
        {
            var configInfo = GetConfiguration(name);
            return configInfo?.Value ?? defaultValue;
        }
        
        /// <summary>
        /// 获取所有配置信息
        /// </summary>
        /// <returns>所有配置信息</returns>
        public IEnumerable<ConfigurationInfo> GetAllConfigurations()
        {
            return _configurations.Values;
        }
        
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns>是否删除成功</returns>
        public bool RemoveConfiguration(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "配置名称不能为空");
            }
            
            return _configurations.TryRemove(name, out _);
        }
        
        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="value">配置值</param>
        /// <returns>验证结果</returns>
        public ConfigurationValidationResult ValidateConfiguration(string name, string value)
        {
            var result = new ConfigurationValidationResult
            {
                IsValid = true,
                Message = "配置验证通过"
            };
            
            // 可以根据配置名称添加特定的验证逻辑
            switch (name)
            {
                case "ServerPort":
                    // 验证端口号是否为有效数字且在有效范围内
                    if (!int.TryParse(value, out int port))
                    {
                        result.IsValid = false;
                        result.Message = "端口号必须是有效数字";
                    }
                    else if (port < 1 || port > 65535)
                    {
                        result.IsValid = false;
                        result.Message = "端口号必须在1-65535范围内";
                    }
                    break;
                
                case "HeartbeatTimeout":
                    // 验证心跳超时时间是否为有效数字且大于0
                    if (!int.TryParse(value, out int timeout))
                    {
                        result.IsValid = false;
                        result.Message = "心跳超时时间必须是有效数字";
                    }
                    else if (timeout <= 0)
                    {
                        result.IsValid = false;
                        result.Message = "心跳超时时间必须大于0";
                    }
                    break;
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// 配置信息类
    /// </summary>
    public class ConfigurationInfo
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 配置值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 配置描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
    
    /// <summary>
    /// 配置更新事件参数
    /// </summary>
    public class ConfigurationUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// 更新的配置信息
        /// </summary>
        public ConfigurationInfo ConfigurationInfo { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configInfo">更新的配置信息</param>
        public ConfigurationUpdatedEventArgs(ConfigurationInfo configInfo)
        {
            ConfigurationInfo = configInfo;
        }
    }
    
    /// <summary>
    /// 配置验证结果类
    /// </summary>
    public class ConfigurationValidationResult
    {
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// 验证消息
        /// </summary>
        public string Message { get; set; }
    }
}
