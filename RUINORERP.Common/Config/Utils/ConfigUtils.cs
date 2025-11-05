/*************************************************************
 * 文件名：ConfigUtils.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置工具类
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using RUINORERP.Common.Config.Attributes;
using RUINORERP.Common.Config.Encryption;

namespace RUINORERP.Common.Config.Utils
{
    /// <summary>
    /// 配置工具类
    /// 提供配置相关的实用方法
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// 默认配置文件扩展名
        /// </summary>
        public const string DefaultConfigExtension = ".json";
        
        /// <summary>
        /// 默认配置目录名
        /// </summary>
        public const string DefaultConfigDirectoryName = "Configs";

        /// <summary>
        /// 环境变量前缀
        /// </summary>
        public const string EnvironmentVariablePrefix = "RUINORERP_";

        /// <summary>
        /// 获取默认配置目录路径
        /// </summary>
        /// <param name="basePath">基础路径，如果为null则使用应用程序目录</param>
        /// <returns>配置目录路径</returns>
        public static string GetDefaultConfigDirectory(string basePath = null)
        {
            string rootPath = string.IsNullOrEmpty(basePath) ? 
                AppContext.BaseDirectory : basePath;
            
            // 尝试在应用程序目录下找到配置目录
            string configDir = Path.Combine(rootPath, DefaultConfigDirectoryName);
            
            // 如果配置目录不存在，尝试上一级目录
            if (!Directory.Exists(configDir) && Directory.GetParent(rootPath) != null)
            {
                configDir = Path.Combine(Directory.GetParent(rootPath).FullName, DefaultConfigDirectoryName);
            }
            
            return configDir;
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="fileName">文件名，如果为null则使用类型名</param>
        /// <param name="configDir">配置目录，如果为null则使用默认配置目录</param>
        /// <param name="includeExtension">是否包含文件扩展名</param>
        /// <returns>配置文件路径</returns>
        public static string GetConfigFilePath<TConfig>(
            string fileName = null, 
            string configDir = null, 
            bool includeExtension = true)
        {
            return GetConfigFilePath(typeof(TConfig), fileName, configDir, includeExtension);
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="fileName">文件名，如果为null则使用类型名</param>
        /// <param name="configDir">配置目录，如果为null则使用默认配置目录</param>
        /// <param name="includeExtension">是否包含文件扩展名</param>
        /// <returns>配置文件路径</returns>
        public static string GetConfigFilePath(
            Type configType, 
            string fileName = null, 
            string configDir = null, 
            bool includeExtension = true)
        {
            if (configType == null)
                throw new ArgumentNullException(nameof(configType));
            
            string directory = string.IsNullOrEmpty(configDir) ? 
                GetDefaultConfigDirectory() : configDir;
            
            // 确保目录存在
            Directory.CreateDirectory(directory);
            
            // 如果没有指定文件名，使用类型名
            string name = string.IsNullOrEmpty(fileName) ? 
                configType.Name : fileName;
            
            // 如果需要，添加扩展名
            if (includeExtension && !Path.HasExtension(name))
            {
                name += DefaultConfigExtension;
            }
            
            return Path.Combine(directory, name);
        }

        /// <summary>
        /// 从环境变量加载配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="prefix">环境变量前缀</param>
        /// <returns>配置对象</returns>
        public static TConfig LoadFromEnvironmentVariables<TConfig>(string prefix = null)
            where TConfig : class, new()
        {
            var config = new TConfig();
            LoadFromEnvironmentVariables(config, prefix);
            return config;
        }

        /// <summary>
        /// 从环境变量加载配置到现有对象
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="prefix">环境变量前缀</param>
        public static void LoadFromEnvironmentVariables<TConfig>(TConfig config, string prefix = null)
            where TConfig : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            string envPrefix = string.IsNullOrEmpty(prefix) ? 
                EnvironmentVariablePrefix : prefix;
            
            // 确保前缀以下划线结尾
            if (!envPrefix.EndsWith("_"))
                envPrefix += "_";
            
            var type = config.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
            
            foreach (var property in properties)
            {
                // 获取环境变量属性
                var envAttribute = property.GetCustomAttribute<ConfigEnvironmentVariableAttribute>();
                string variableName;
                
                if (envAttribute != null)
                {
                    variableName = envAttribute.VariableName;
                }
                else
                {
                    // 否则使用属性名（转换为环境变量命名约定）
                    variableName = $"{envPrefix}{property.Name.ToUpperInvariant().Replace('.', '_')}";
                }
                
                // 尝试获取环境变量
                string value = Environment.GetEnvironmentVariable(variableName);
                if (value != null)
                {
                    SetPropertyValue(config, property, value);
                    
                    // 如果是必需的，记录已找到
                    if (envAttribute?.Required ?? false)
                    {
                        // 可以在这里记录日志
                    }
                }
                else if (envAttribute?.Required ?? false)
                {
                    throw new InvalidOperationException($"缺少必需的环境变量: {variableName}");
                }
            }
        }

        /// <summary>
        /// 设置属性值
        /// 尝试将字符串值转换为属性类型
        /// </summary>
        /// <param name="obj">目标对象</param>
        /// <param name="property">属性信息</param>
        /// <param name="value">字符串值</param>
        public static void SetPropertyValue(object obj, PropertyInfo property, string value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (value == null) return;
            
            Type propertyType = property.PropertyType;
            
            // 处理可空类型
            Type underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType != null)
            {
                propertyType = underlyingType;
            }
            
            try
            {
                // 处理枚举
                if (propertyType.IsEnum)
                {
                    property.SetValue(obj, Enum.Parse(propertyType, value, true));
                    return;
                }
                
                // 处理基本类型和常见类型
                object convertedValue;
                switch (Type.GetTypeCode(propertyType))
                {
                    case TypeCode.Boolean:
                        // 支持多种布尔值表示
                        if (string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = true;
                        }
                        else if (string.Equals(value, "0", StringComparison.OrdinalIgnoreCase) ||
                                 string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) ||
                                 string.Equals(value, "no", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = false;
                        }
                        else
                        {
                            convertedValue = Convert.ToBoolean(value);
                        }
                        break;
                    
                    case TypeCode.DateTime:
                        // 尝试多种日期格式
                        if (DateTime.TryParse(value, out var dateTime))
                        {
                            convertedValue = dateTime;
                        }
                        else
                        {
                            convertedValue = Convert.ToDateTime(value);
                        }
                        break;
                    
                    case TypeCode.Decimal:
                        convertedValue = Convert.ToDecimal(value);
                        break;
                    
                    case TypeCode.Double:
                        convertedValue = Convert.ToDouble(value);
                        break;
                    
                    case TypeCode.Int16:
                        convertedValue = Convert.ToInt16(value);
                        break;
                    
                    case TypeCode.Int32:
                        convertedValue = Convert.ToInt32(value);
                        break;
                    
                    case TypeCode.Int64:
                        convertedValue = Convert.ToInt64(value);
                        break;
                    
                    case TypeCode.Single:
                        convertedValue = Convert.ToSingle(value);
                        break;
                    
                    case TypeCode.String:
                        convertedValue = value;
                        break;
                    
                    case TypeCode.Guid:
                        convertedValue = Guid.Parse(value);
                        break;
                    
                    // 处理集合类型
                    case TypeCode.Object:
                        if (propertyType.IsArray)
                        {
                            // 简单处理逗号分隔的数组
                            Type elementType = propertyType.GetElementType();
                            string[] items = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            Array array = Array.CreateInstance(elementType, items.Length);
                            
                            for (int i = 0; i < items.Length; i++)
                            {
                                array.SetValue(Convert.ChangeType(items[i].Trim(), elementType), i);
                            }
                            
                            convertedValue = array;
                        }
                        else if (propertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)))
                        {
                            // 尝试处理List<T>等集合
                            convertedValue = JsonSerializer.Deserialize(value, propertyType);
                        }
                        else
                        {
                            // 对于复杂类型，尝试JSON反序列化
                            convertedValue = JsonSerializer.Deserialize(value, propertyType);
                        }
                        break;
                    
                    default:
                        convertedValue = Convert.ChangeType(value, propertyType);
                        break;
                }
                
                property.SetValue(obj, convertedValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"无法将值 '{value}' 转换为属性 '{property.Name}' 的类型 '{property.PropertyType.Name}'", ex);
            }
        }

        /// <summary>
        /// 应用配置默认值
        /// 根据ConfigDefaultValueAttribute设置属性默认值
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        public static void ApplyDefaultValues<TConfig>(TConfig config)
            where TConfig : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            var type = config.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
            
            foreach (var property in properties)
            {
                // 获取默认值属性
                var defaultAttribute = property.GetCustomAttribute<ConfigDefaultValueAttribute>();
                if (defaultAttribute != null)
                {
                    // 获取当前值
                    object currentValue = property.GetValue(config);
                    
                    // 如果当前值为null或默认值，应用默认值
                    if (currentValue == null || IsDefaultValue(currentValue, property.PropertyType))
                    {
                        object defaultValue = defaultAttribute.DefaultValue;
                        
                        if (defaultValue != null)
                        {
                            // 如果默认值类型与属性类型不匹配，尝试转换
                            if (defaultValue.GetType() != property.PropertyType)
                            {
                                try
                                {
                                    defaultValue = Convert.ChangeType(defaultValue, property.PropertyType);
                                }
                                catch (InvalidCastException)
                                {
                                    // 转换失败时跳过
                                    continue;
                                }
                            }
                            
                            property.SetValue(config, defaultValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查值是否为类型的默认值
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <param name="type">类型</param>
        /// <returns>是否为默认值</returns>
        private static bool IsDefaultValue(object value, Type type)
        {
            // 如果值为null，对于引用类型就是默认值
            if (value == null)
                return !type.IsValueType;
            
            // 对于值类型，检查是否等于默认构造的值
            if (type.IsValueType)
            {
                object defaultValue = Activator.CreateInstance(type);
                return value.Equals(defaultValue);
            }
            
            // 对于特殊的引用类型，进行特殊处理
            switch (value)
            {
                case string str:
                    return string.IsNullOrEmpty(str);
                case Array array:
                    return array.Length == 0;
                case ICollection<object> collection:
                    return collection.Count == 0;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取配置路径
        /// 构建配置项的点分隔路径
        /// </summary>
        /// <param name="parentPath">父配置路径</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>完整的配置路径</returns>
        public static string BuildConfigPath(string parentPath, string propertyName)
        {
            if (string.IsNullOrEmpty(parentPath))
                return propertyName;
            
            return $"{parentPath}.{propertyName}";
        }

        /// <summary>
        /// 脱敏敏感配置值
        /// 根据ConfigSensitiveAttribute对敏感配置值进行脱敏
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="sensitiveAttribute">敏感信息属性</param>
        /// <returns>脱敏后的值</returns>
        public static string MaskSensitiveValue(string value, ConfigSensitiveAttribute sensitiveAttribute = null)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            
            if (sensitiveAttribute == null)
                return value;
            
            switch (sensitiveAttribute.MaskingRule?.ToLower())
            {
                case "full":
                    // 完全脱敏
                    return new string(sensitiveAttribute.MaskingCharacter, Math.Min(8, value.Length));
                
                case "partial":
                default:
                    // 部分脱敏
                    // 对于长度小于等于3的字符串，只显示第一个字符
                    if (value.Length <= 3)
                    {
                        return value[0] + new string(sensitiveAttribute.MaskingCharacter, value.Length - 1);
                    }
                    // 对于中等长度的字符串，显示首尾各一个字符
                    else if (value.Length <= 10)
                    {
                        int maskLength = value.Length - 2;
                        return value[0] + new string(sensitiveAttribute.MaskingCharacter, maskLength) + value[value.Length - 1];
                    }
                    // 对于长字符串，显示前3个和后4个字符
                    else
                    {
                        int maskLength = value.Length - 7;
                        return value.Substring(0, 3) + new string(sensitiveAttribute.MaskingCharacter, maskLength) + 
                               value.Substring(value.Length - 4);
                    }
                
                case "pattern":
                    // 使用正则表达式进行脱敏
                    if (!string.IsNullOrEmpty(sensitiveAttribute.MaskingPattern))
                    {
                        try
                        {
                            // 创建掩码字符串
                            string mask = new string(sensitiveAttribute.MaskingCharacter, 6);
                            return Regex.Replace(value, sensitiveAttribute.MaskingPattern, mask);
                        }
                        catch (ArgumentException)
                        {
                            // 如果正则表达式无效，回退到部分脱敏
                            goto case "partial";
                        }
                    }
                    else
                    {
                        // 如果没有提供模式，回退到部分脱敏
                        goto case "partial";
                    }
            }
        }

        /// <summary>
        /// 验证配置对象
        /// 使用数据注解和配置特定的验证属性
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>验证结果列表</returns>
        public static List<ValidationResult> ValidateConfig<TConfig>(TConfig config)
            where TConfig : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(config, null, null);
            
            // 添加环境信息
            validationContext.Items["Environment"] = GetCurrentEnvironment();
            
            // 验证配置对象
            Validator.TryValidateObject(config, validationContext, results, true);
            
            return results;
        }

        /// <summary>
        /// 获取当前环境
        /// 从环境变量中读取环境设置
        /// </summary>
        /// <returns>环境名称（如Development, Staging, Production）</returns>
        public static string GetCurrentEnvironment()
        {   
            // 尝试从环境变量读取
            string environment = Environment.GetEnvironmentVariable($"{EnvironmentVariablePrefix}ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment))
                return environment;
            
            // 尝试读取ASPNETCORE_ENVIRONMENT（如果使用ASP.NET Core）
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment))
                return environment;
            
            // 默认开发环境
            return "Development";
        }

        /// <summary>
        /// 检查配置文件是否需要加密
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否需要加密</returns>
        public static bool NeedsEncryption(string filePath)
        {   
            // 这里可以根据文件路径、名称模式或内容进行判断
            // 简单实现：检查文件名是否包含敏感关键词
            string fileName = Path.GetFileName(filePath);
            string[] sensitiveKeywords = { "secret", "password", "key", "credential", "token", "certificate" };
            
            return sensitiveKeywords.Any(keyword => 
                fileName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// 合并配置
        /// 将源配置对象的值合并到目标配置对象
        /// 只合并非默认值
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="target">目标配置对象</param>
        /// <param name="source">源配置对象</param>
        /// <param name="overrideExisting">是否覆盖已存在的值</param>
        public static void MergeConfig<TConfig>(TConfig target, TConfig source, bool overrideExisting = true)
            where TConfig : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            
            var type = typeof(TConfig);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
            
            foreach (var property in properties)
            {
                // 获取源属性值
                object sourceValue = property.GetValue(source);
                
                // 如果源值为默认值，跳过
                if (sourceValue == null || IsDefaultValue(sourceValue, property.PropertyType))
                    continue;
                
                // 获取目标属性值
                object targetValue = property.GetValue(target);
                
                // 如果目标值不为默认值且不允许覆盖，跳过
                if (!overrideExisting && targetValue != null && !IsDefaultValue(targetValue, property.PropertyType))
                    continue;
                
                // 设置目标属性值
                property.SetValue(target, sourceValue);
            }
        }

        /// <summary>
        /// 从配置对象中提取敏感属性
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>敏感属性及其值的字典</returns>
        public static Dictionary<string, object> GetSensitiveProperties<TConfig>(TConfig config)
            where TConfig : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            var sensitiveProperties = new Dictionary<string, object>();
            var type = config.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var property in properties)
            {
                // 检查是否有敏感属性标记
                if (property.GetCustomAttribute<ConfigSensitiveAttribute>() != null)
                {
                    try
                    {
                        object value = property.GetValue(config);
                        sensitiveProperties.Add(property.Name, value);
                    }
                    catch (Exception)
                    {
                        // 忽略无法访问的属性
                    }
                }
            }
            
            return sensitiveProperties;
        }

        /// <summary>
        /// 克隆配置对象
        /// 通过JSON序列化和反序列化实现深拷贝
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>配置对象的深拷贝</returns>
        public static TConfig CloneConfig<TConfig>(TConfig config)
            where TConfig : class
        {
            if (config == null)
                return null;
            
            var json = JsonSerializer.Serialize(config);
            return JsonSerializer.Deserialize<TConfig>(json);
        }
    }
}