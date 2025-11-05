/*************************************************************
 * 文件名：ConfigEnvironmentProvider.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置环境提供者
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RUINORERP.Common.Config.Utils;

namespace RUINORERP.Common.Config.Environment
{
    /// <summary>
    /// 配置环境类型
    /// </summary>
    public enum ConfigEnvironmentType
    {
        /// <summary>
        /// 开发环境
        /// </summary>
        Development,
        
        /// <summary>
        /// 测试环境
        /// </summary>
        Testing,
        
        /// <summary>
        /// 预发布环境
        /// </summary>
        Staging,
        
        /// <summary>
        /// 生产环境
        /// </summary>
        Production,
        
        /// <summary>
        /// 自定义环境
        /// </summary>
        Custom
    }

    /// <summary>
    /// 环境配置项
    /// </summary>
    public class EnvironmentSetting
    {
        /// <summary>
        /// 环境名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 环境类型
        /// </summary>
        public ConfigEnvironmentType Type { get; set; }
        
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath { get; set; }
        
        /// <summary>
        /// 环境特定设置
        /// </summary>
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
        
        /// <summary>
        /// 是否为当前活动环境
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 配置环境提供者接口
    /// </summary>
    public interface IConfigEnvironmentProvider
    {
        /// <summary>
        /// 获取当前环境
        /// </summary>
        /// <returns>当前环境名称</returns>
        string GetCurrentEnvironment();
        
        /// <summary>
        /// 获取当前环境类型
        /// </summary>
        /// <returns>当前环境类型</returns>
        ConfigEnvironmentType GetCurrentEnvironmentType();
        
        /// <summary>
        /// 设置当前环境
        /// </summary>
        /// <param name="environmentName">环境名称</param>
        void SetCurrentEnvironment(string environmentName);
        
        /// <summary>
        /// 检查是否为指定环境
        /// </summary>
        /// <param name="environmentName">环境名称</param>
        /// <returns>是否为指定环境</returns>
        bool IsEnvironment(string environmentName);
        
        /// <summary>
        /// 是否为开发环境
        /// </summary>
        /// <returns>是否为开发环境</returns>
        bool IsDevelopment();
        
        /// <summary>
        /// 是否为测试环境
        /// </summary>
        /// <returns>是否为测试环境</returns>
        bool IsTesting();
        
        /// <summary>
        /// 是否为预发布环境
        /// </summary>
        /// <returns>是否为预发布环境</returns>
        bool IsStaging();
        
        /// <summary>
        /// 是否为生产环境
        /// </summary>
        /// <returns>是否为生产环境</returns>
        bool IsProduction();
        
        /// <summary>
        /// 获取环境特定的配置文件路径
        /// </summary>
        /// <param name="baseFileName">基础文件名</param>
        /// <param name="configDir">配置目录</param>
        /// <returns>环境特定的配置文件路径</returns>
        string GetEnvironmentConfigPath(string baseFileName, string configDir = null);
        
        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <param name="key">变量名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>环境变量值</returns>
        string GetEnvironmentVariable(string key, string defaultValue = null);
        
        /// <summary>
        /// 设置环境变量
        /// </summary>
        /// <param name="key">变量名</param>
        /// <param name="value">变量值</param>
        void SetEnvironmentVariable(string key, string value);
        
        /// <summary>
        /// 获取所有注册的环境
        /// </summary>
        /// <returns>环境设置列表</returns>
        List<EnvironmentSetting> GetAllEnvironments();
        
        /// <summary>
        /// 注册环境
        /// </summary>
        /// <param name="setting">环境设置</param>
        void RegisterEnvironment(EnvironmentSetting setting);
    }

    /// <summary>
    /// 默认配置环境提供者
    /// </summary>
    public class DefaultConfigEnvironmentProvider : IConfigEnvironmentProvider
    {
        private readonly List<EnvironmentSetting> _environments = new List<EnvironmentSetting>();
        private string _currentEnvironment;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultConfigEnvironmentProvider()
        {
            // 注册预定义环境
            RegisterPredefinedEnvironments();
            
            // 初始化当前环境
            _currentEnvironment = GetEnvironmentFromSources();
        }
        
        /// <summary>
        /// 注册预定义环境
        /// </summary>
        private void RegisterPredefinedEnvironments()
        {
            RegisterEnvironment(new EnvironmentSetting
            {
                Name = "Development",
                Type = ConfigEnvironmentType.Development,
                Priority = 10,
                Settings = new Dictionary<string, string>
                {
                    { "DebugEnabled", "true" },
                    { "LogLevel", "Debug" }
                }
            });
            
            RegisterEnvironment(new EnvironmentSetting
            {
                Name = "Testing",
                Type = ConfigEnvironmentType.Testing,
                Priority = 20,
                Settings = new Dictionary<string, string>
                {
                    { "DebugEnabled", "true" },
                    { "LogLevel", "Information" }
                }
            });
            
            RegisterEnvironment(new EnvironmentSetting
            {
                Name = "Staging",
                Type = ConfigEnvironmentType.Staging,
                Priority = 30,
                Settings = new Dictionary<string, string>
                {
                    { "DebugEnabled", "false" },
                    { "LogLevel", "Warning" }
                }
            });
            
            RegisterEnvironment(new EnvironmentSetting
            {
                Name = "Production",
                Type = ConfigEnvironmentType.Production,
                Priority = 40,
                Settings = new Dictionary<string, string>
                {
                    { "DebugEnabled", "false" },
                    { "LogLevel", "Error" }
                }
            });
        }
        
        /// <summary>
        /// 从各种源获取环境信息
        /// </summary>
        /// <returns>环境名称</returns>
        private string GetEnvironmentFromSources()
        {   
            // 1. 尝试从环境变量获取
            string environment = GetEnvironmentVariable("RUINORERP_ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment))
                return environment;
            
            // 2. 尝试从应用设置获取
            environment = GetEnvironmentFromAppSettings();
            if (!string.IsNullOrEmpty(environment))
                return environment;
            
            // 3. 尝试从命令行参数获取
            environment = GetEnvironmentFromCommandLineArgs();
            if (!string.IsNullOrEmpty(environment))
                return environment;
            
            // 4. 默认开发环境
            return "Development";
        }
        
        /// <summary>
        /// 从应用设置获取环境
        /// </summary>
        /// <returns>环境名称</returns>
        private string GetEnvironmentFromAppSettings()
        {   
            try
            {   
                // 尝试读取应用配置文件
                string appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                if (File.Exists(appSettingsPath))
                {   
                    // 简单读取JSON配置
                    // 注意：这里简化处理，实际应该使用配置解析器
                    string content = File.ReadAllText(appSettingsPath);
                    if (content.Contains("\"Environment\":"))
                    {   
                        int startIndex = content.IndexOf("\"Environment\":\"") + 15;
                        int endIndex = content.IndexOf('"', startIndex);
                        if (endIndex > startIndex)
                        {   
                            return content.Substring(startIndex, endIndex - startIndex);
                        }
                    }
                }
            }
            catch (Exception)
            {   
                // 忽略异常，继续尝试其他方式
            }
            
            return null;
        }
        
        /// <summary>
        /// 从命令行参数获取环境
        /// </summary>
        /// <returns>环境名称</returns>
        private string GetEnvironmentFromCommandLineArgs()
        {   
            try
            {   
                string[] args = Environment.GetCommandLineArgs();
                foreach (string arg in args)
                {   
                    if (arg.StartsWith("--environment=", StringComparison.OrdinalIgnoreCase) ||
                        arg.StartsWith("-e=", StringComparison.OrdinalIgnoreCase))
                    {   
                        return arg.Split('=')[1];
                    }
                }
            }
            catch (Exception)
            {   
                // 忽略异常
            }
            
            return null;
        }
        
        /// <summary>
        /// 获取当前环境
        /// </summary>
        /// <returns>当前环境名称</returns>
        public string GetCurrentEnvironment()
        {   
            return _currentEnvironment;
        }
        
        /// <summary>
        /// 获取当前环境类型
        /// </summary>
        /// <returns>当前环境类型</returns>
        public ConfigEnvironmentType GetCurrentEnvironmentType()
        {   
            var environment = _environments.FirstOrDefault(e => e.Name.Equals(_currentEnvironment, StringComparison.OrdinalIgnoreCase));
            return environment?.Type ?? ConfigEnvironmentType.Custom;
        }
        
        /// <summary>
        /// 设置当前环境
        /// </summary>
        /// <param name="environmentName">环境名称</param>
        public void SetCurrentEnvironment(string environmentName)
        {   
            if (string.IsNullOrEmpty(environmentName))
                throw new ArgumentNullException(nameof(environmentName));
            
            // 验证环境名称格式
            if (!IsValidEnvironmentName(environmentName))
                throw new ArgumentException("环境名称格式无效", nameof(environmentName));
            
            _currentEnvironment = environmentName;
            
            // 更新活动环境标志
            UpdateActiveEnvironmentFlags();
        }
        
        /// <summary>
        /// 验证环境名称格式
        /// </summary>
        /// <param name="environmentName">环境名称</param>
        /// <returns>是否有效</returns>
        private bool IsValidEnvironmentName(string environmentName)
        {   
            // 简单验证：只允许字母、数字、下划线和连字符
            foreach (char c in environmentName)
            {   
                if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// 更新活动环境标志
        /// </summary>
        private void UpdateActiveEnvironmentFlags()
        {   
            foreach (var env in _environments)
            {   
                env.IsActive = env.Name.Equals(_currentEnvironment, StringComparison.OrdinalIgnoreCase);
            }
        }
        
        /// <summary>
        /// 检查是否为指定环境
        /// </summary>
        /// <param name="environmentName">环境名称</param>
        /// <returns>是否为指定环境</returns>
        public bool IsEnvironment(string environmentName)
        {   
            return _currentEnvironment.Equals(environmentName, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// 是否为开发环境
        /// </summary>
        /// <returns>是否为开发环境</returns>
        public bool IsDevelopment()
        {   
            return IsEnvironment("Development");
        }
        
        /// <summary>
        /// 是否为测试环境
        /// </summary>
        /// <returns>是否为测试环境</returns>
        public bool IsTesting()
        {   
            return IsEnvironment("Testing");
        }
        
        /// <summary>
        /// 是否为预发布环境
        /// </summary>
        /// <returns>是否为预发布环境</returns>
        public bool IsStaging()
        {   
            return IsEnvironment("Staging");
        }
        
        /// <summary>
        /// 是否为生产环境
        /// </summary>
        /// <returns>是否为生产环境</returns>
        public bool IsProduction()
        {   
            return IsEnvironment("Production");
        }
        
        /// <summary>
        /// 获取环境特定的配置文件路径
        /// </summary>
        /// <param name="baseFileName">基础文件名</param>
        /// <param name="configDir">配置目录</param>
        /// <returns>环境特定的配置文件路径</returns>
        public string GetEnvironmentConfigPath(string baseFileName, string configDir = null)
        {   
            if (string.IsNullOrEmpty(baseFileName))
                throw new ArgumentNullException(nameof(baseFileName));
            
            string directory = string.IsNullOrEmpty(configDir) ? ConfigUtils.GetDefaultConfigDirectory() : configDir;
            
            // 获取文件扩展名
            string extension = Path.GetExtension(baseFileName);
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(baseFileName);
            
            // 构建环境特定的文件名
            string environmentFileName = $"{nameWithoutExtension}.{_currentEnvironment}{extension}";
            string environmentPath = Path.Combine(directory, environmentFileName);
            
            // 如果环境特定的文件存在，返回它
            if (File.Exists(environmentPath))
                return environmentPath;
            
            // 否则返回基础文件路径
            return Path.Combine(directory, baseFileName);
        }
        
        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <param name="key">变量名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>环境变量值</returns>
        public string GetEnvironmentVariable(string key, string defaultValue = null)
        {   
            if (string.IsNullOrEmpty(key))
                return defaultValue;
            
            // 1. 尝试获取系统环境变量
            string value = System.Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(value))
                return value;
            
            // 2. 尝试从当前环境设置获取
            var currentEnv = _environments.FirstOrDefault(e => e.IsActive);
            if (currentEnv != null && currentEnv.Settings.TryGetValue(key, out string envValue))
                return envValue;
            
            // 3. 返回默认值
            return defaultValue;
        }
        
        /// <summary>
        /// 设置环境变量
        /// </summary>
        /// <param name="key">变量名</param>
        /// <param name="value">变量值</param>
        public void SetEnvironmentVariable(string key, string value)
        {   
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            
            // 设置系统环境变量
            System.Environment.SetEnvironmentVariable(key, value);
            
            // 更新当前环境设置
            var currentEnv = _environments.FirstOrDefault(e => e.IsActive);
            if (currentEnv != null)
            {
                currentEnv.Settings[key] = value;
            }
        }
        
        /// <summary>
        /// 获取所有注册的环境
        /// </summary>
        /// <returns>环境设置列表</returns>
        public List<EnvironmentSetting> GetAllEnvironments()
        {   
            return _environments.OrderByDescending(e => e.Priority).ToList();
        }
        
        /// <summary>
        /// 注册环境
        /// </summary>
        /// <param name="setting">环境设置</param>
        public void RegisterEnvironment(EnvironmentSetting setting)
        {   
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));
            
            if (string.IsNullOrEmpty(setting.Name))
                throw new ArgumentException("环境名称不能为空", nameof(setting));
            
            // 检查是否已存在同名环境
            var existing = _environments.FirstOrDefault(e => e.Name.Equals(setting.Name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {   
                // 更新现有环境
                existing.Type = setting.Type;
                existing.Priority = setting.Priority;
                existing.ConfigFilePath = setting.ConfigFilePath;
                
                // 合并设置
                foreach (var kvp in setting.Settings)
                {   
                    existing.Settings[kvp.Key] = kvp.Value;
                }
            }
            else
            {   
                // 添加新环境
                _environments.Add(setting);
            }
            
            // 更新活动标志
            UpdateActiveEnvironmentFlags();
        }
    }
    
    /// <summary>
    /// 配置环境扩展方法
    /// </summary>
    public static class ConfigEnvironmentExtensions
    {
        /// <summary>
        /// 获取环境特定的配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="environmentProvider">环境提供者</param>
        /// <param name="baseFileName">基础文件名</param>
        /// <param name="configDir">配置目录</param>
        /// <returns>环境特定的配置</returns>
        public static T GetEnvironmentConfig<T>(this IConfigEnvironmentProvider environmentProvider, 
            string baseFileName = null, string configDir = null) where T : class, new()
        {
            if (environmentProvider == null)
                throw new ArgumentNullException(nameof(environmentProvider));
            
            // 获取文件名
            string fileName = string.IsNullOrEmpty(baseFileName) ? typeof(T).Name + ".json" : baseFileName;
            
            // 获取环境特定的配置文件路径
            string configPath = environmentProvider.GetEnvironmentConfigPath(fileName, configDir);
            
            // 如果文件存在，读取配置
            if (File.Exists(configPath))
            {
                try
                {
                    var parser = new DefaultConfigParser();
                    return parser.Parse<T>(configPath);
                }
                catch (Exception)
                {
                    // 如果读取失败，返回默认配置
                }
            }
            
            // 返回默认配置
            return new T();
        }
        
        /// <summary>
        /// 获取环境配置文件路径列表
        /// 按照优先级排序
        /// </summary>
        /// <param name="environmentProvider">环境提供者</param>
        /// <param name="baseFileName">基础文件名</param>
        /// <param name="configDir">配置目录</param>
        /// <returns>配置文件路径列表</returns>
        public static List<string> GetConfigFilePaths(this IConfigEnvironmentProvider environmentProvider, 
            string baseFileName, string configDir = null)
        {
            if (environmentProvider == null)
                throw new ArgumentNullException(nameof(environmentProvider));
            
            if (string.IsNullOrEmpty(baseFileName))
                throw new ArgumentNullException(nameof(baseFileName));
            
            var paths = new List<string>();
            string directory = string.IsNullOrEmpty(configDir) ? ConfigUtils.GetDefaultConfigDirectory() : configDir;
            string extension = Path.GetExtension(baseFileName);
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(baseFileName);
            
            // 获取所有环境
            var environments = environmentProvider.GetAllEnvironments();
            
            // 为每个环境构建配置文件路径
            foreach (var env in environments)
            {
                string envFileName = $"{nameWithoutExtension}.{env.Name}{extension}";
                string envPath = Path.Combine(directory, envFileName);
                
                if (File.Exists(envPath))
                {
                    paths.Add(envPath);
                }
            }
            
            // 添加基础配置文件路径
            string basePath = Path.Combine(directory, baseFileName);
            if (File.Exists(basePath) && !paths.Contains(basePath))
            {
                paths.Add(basePath);
            }
            
            return paths;
        }
        
        /// <summary>
        /// 根据环境获取配置值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="environmentProvider">环境提供者</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static T GetConfigValue<T>(this IConfigEnvironmentProvider environmentProvider, 
            string key, T defaultValue = default)
        {
            if (environmentProvider == null)
                throw new ArgumentNullException(nameof(environmentProvider));
            
            if (string.IsNullOrEmpty(key))
                return defaultValue;
            
            string value = environmentProvider.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            
            try
            {
                // 尝试转换值
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// 获取环境特定的连接字符串
        /// </summary>
        /// <param name="environmentProvider">环境提供者</param>
        /// <param name="name">连接字符串名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>连接字符串</returns>
        public static string GetConnectionString(this IConfigEnvironmentProvider environmentProvider, 
            string name, string defaultValue = null)
        {
            if (environmentProvider == null)
                throw new ArgumentNullException(nameof(environmentProvider));
            
            // 尝试多种格式的环境变量
            string[] keyFormats = {
                $"CONNECTION_STRING_{name.ToUpper()}",
                $"CONNECTIONSTRINGS_{name.ToUpper()}",
                $"{ConfigUtils.EnvironmentVariablePrefix}CONNECTION_{name.ToUpper()}",
                name
            };
            
            foreach (string key in keyFormats)
            {
                string value = environmentProvider.GetEnvironmentVariable(key);
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            
            return defaultValue;
        }
    }
}