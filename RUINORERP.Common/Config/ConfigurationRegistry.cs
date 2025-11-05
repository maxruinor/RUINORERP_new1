/*************************************************************
 * 文件名：ConfigurationRegistry.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置类型注册表，用于管理配置类型注册和元数据
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置类型信息
    /// 存储配置类型的元数据
    /// </summary>
    public class ConfigTypeInfo
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// 自定义目录映射
        /// </summary>
        public Dictionary<ConfigPathType, string> CustomDirectories { get; private set; }
        
        public ConfigTypeInfo()
        {
            CustomDirectories = new Dictionary<ConfigPathType, string>();
        }
    }

    /// <summary>
    /// 配置注册表接口
    /// </summary>
    public interface IConfigurationRegistry
    {
        /// <summary>
        /// 注册配置类型
        /// </summary>
        /// <param name="configType">配置类型</param>
        void RegisterConfigType(Type configType);

        /// <summary>
        /// 获取配置类型信息
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置类型信息</returns>
        ConfigTypeInfo GetConfigTypeInfo(string configTypeName);

        /// <summary>
        /// 获取所有配置类型信息
        /// </summary>
        /// <returns>配置类型信息列表</returns>
        IEnumerable<ConfigTypeInfo> GetAllConfigTypes();

        /// <summary>
        /// 检查配置类型是否已注册
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>是否已注册</returns>
        bool IsConfigTypeRegistered(string configTypeName);
    }

    /// <summary>
    /// 配置类型注册表，负责管理所有配置类型的注册和检索
    /// 提供配置类型的动态管理和实例创建
    /// </summary>
    public class ConfigurationRegistry : IConfigurationRegistry
    {
        private readonly Dictionary<string, ConfigTypeInfo> _registeredTypes;
        private readonly object _lockObj = new object();
        
        // 提供单例访问方式（用于不支持依赖注入的场景）
        private static readonly Lazy<ConfigurationRegistry> _instance = new Lazy<ConfigurationRegistry>();
        
        /// <summary>
        /// 获取注册表的单例实例
        /// </summary>
        public static ConfigurationRegistry Instance => _instance.Value;
        
        /// <summary>
        /// 自定义目录映射
        /// </summary>
        private readonly Dictionary<ConfigPathType, string> _customDirectories = new Dictionary<ConfigPathType, string>();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigurationRegistry()
        {
            _registeredTypes = new Dictionary<string, ConfigTypeInfo>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 注册配置类型
        /// </summary>
        /// <param name="configType">配置类型</param>
        public void RegisterConfigType(Type configType)
        {
            if (configType == null)
                throw new ArgumentNullException(nameof(configType));

            string typeName = configType.Name;

            lock (_lockObj)
            {
                // 检查是否已注册
                if (_registeredTypes.ContainsKey(typeName))
                {
                    return;
                }

                // 创建配置类型信息
                var configInfo = new ConfigTypeInfo
                {
                    Type = configType,
                    TypeName = typeName
                };

                _registeredTypes[typeName] = configInfo;
            }
        }
        
        /// <summary>
        /// 使用泛型方式注册配置类型
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        public void Register<T>() where T : class
        {
            RegisterConfigType(typeof(T));
        }
        
        /// <summary>
        /// 注册指定名称的配置类型
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configTypeName">自定义配置类型名称</param>
        public void Register<T>(string configTypeName) where T : class
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));
                
            lock (_lockObj)
            {
                // 检查是否已注册
                if (_registeredTypes.ContainsKey(configTypeName))
                {
                    return;
                }

                // 创建配置类型信息
                var configInfo = new ConfigTypeInfo
                {
                    Type = typeof(T),
                    TypeName = configTypeName
                };

                _registeredTypes[configTypeName] = configInfo;
            }
        }
        
        /// <summary>
        /// 批量注册程序集中的所有配置类型
        /// </summary>
        /// <param name="assembly">要扫描的程序集</param>
        public void RegisterFromAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
                
            var configTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract);
            
            foreach (var configType in configTypes)
            {
                RegisterConfigType(configType);
            }
        }

        /// <summary>
        /// 获取配置类型信息
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置类型信息</returns>
        public ConfigTypeInfo GetConfigTypeInfo(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));

            lock (_lockObj)
            {
                _registeredTypes.TryGetValue(configTypeName, out var configInfo);
                return configInfo;
            }
        }
        
        /// <summary>
        /// 获取所有配置类型信息
        /// </summary>
        /// <returns>配置类型信息列表</returns>
        public IEnumerable<ConfigTypeInfo> GetAllConfigTypes()
        {
            lock (_lockObj)
            {
                return _registeredTypes.Values.ToList();
            }
        }

        /// <summary>
        /// 检查配置类型是否已注册
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>是否已注册</returns>
        public bool IsConfigTypeRegistered(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                return false;

            lock (_lockObj)
            {
                return _registeredTypes.ContainsKey(configTypeName);
            }
        }

        /// <summary>
        /// 根据配置类型名称获取配置类型
        /// </summary>
        /// <param name="typeName">配置类型名称</param>
        /// <returns>配置类型，如果未找到则返回null</returns>
        public Type GetConfigType(string typeName)
        {    
            if (string.IsNullOrEmpty(typeName))
                return null;

            lock (_lockObj)
            {
                _registeredTypes.TryGetValue(typeName, out var configInfo);
                return configInfo?.Type;
            }
        }
        
        /// <summary>
        /// 创建配置实例
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置实例，如果类型未注册或无法创建则返回null</returns>
        public object CreateInstance(string configTypeName)
        {    
            var type = GetConfigType(configTypeName);
            if (type == null)
                return null;
                
            try
            {    
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {    
                throw new InvalidOperationException($"无法创建配置类型 {configTypeName} 的实例", ex);
            }
        }
        
        /// <summary>
        /// 设置配置类型的自定义目录
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="pathType">路径类型</param>
        /// <param name="directory">自定义目录路径</param>
        public void SetCustomDirectory(string configTypeName, ConfigPathType pathType, string directory)
        {    
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));
                
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentNullException(nameof(directory));
                
            lock (_lockObj)
            {    
                if (_registeredTypes.TryGetValue(configTypeName, out var configInfo))
                {    
                    configInfo.CustomDirectories[pathType] = directory;
                }
            }
        }
        
        /// <summary>
        /// 获取配置类型的自定义目录
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="pathType">路径类型</param>
        /// <returns>自定义目录路径，如果未设置则返回null</returns>
        public string GetCustomDirectory(string configTypeName, ConfigPathType pathType)
        {    
            if (string.IsNullOrEmpty(configTypeName))
                return null;
                
            lock (_lockObj)
            {    
                if (_registeredTypes.TryGetValue(configTypeName, out var configInfo) &&
                    configInfo.CustomDirectories.TryGetValue(pathType, out var directory))
                {    
                    return directory;
                }
            }
            
            return null;
        }
    }
}