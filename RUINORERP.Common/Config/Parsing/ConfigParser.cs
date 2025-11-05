/*************************************************************
 * 文件名：ConfigParser.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置解析器，处理不同格式配置文件的序列化和反序列化
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Parsing
{
    /// <summary>
    /// 配置格式类型
    /// 定义支持的配置文件格式
    /// </summary>
    public enum ConfigFormat
    {
        /// <summary>
        /// JSON格式
        /// </summary>
        Json,
        /// <summary>
        /// XML格式
        /// </summary>
        Xml,
        /// <summary>
        /// YAML格式
        /// </summary>
        Yaml,
        /// <summary>
        /// INI格式
        /// </summary>
        Ini,
        /// <summary>
        /// 自定义格式
        /// </summary>
        Custom
    }

    /// <summary>
    /// 配置解析选项
    /// 定义配置解析器的行为选项
    /// </summary>
    public class ConfigParserOptions
    {
        /// <summary>
        /// 配置格式
        /// </summary>
        public ConfigFormat Format { get; set; } = ConfigFormat.Json;

        /// <summary>
        /// 是否忽略未知属性
        /// </summary>
        public bool IgnoreUnknownProperties { get; set; } = true;

        /// <summary>
        /// 是否格式化输出
        /// </summary>
        public bool FormatOutput { get; set; } = true;

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// JSON序列化设置
        /// </summary>
        public JsonSerializerSettings JsonSettings { get; set; }

        /// <summary>
        /// 创建默认的解析选项
        /// </summary>
        /// <returns>默认配置解析选项</returns>
        public static ConfigParserOptions Default()
        {
            return new ConfigParserOptions
            {
                Format = ConfigFormat.Json,
                IgnoreUnknownProperties = true,
                FormatOutput = true,
                Encoding = Encoding.UTF8,
                JsonSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate
                }
            };
        }
    }

    /// <summary>
    /// 配置解析器接口
    /// 定义配置序列化和反序列化的核心功能
    /// </summary>
    public interface IConfigParser
    {
        /// <summary>
        /// 将配置对象序列化为字符串
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="options">解析选项</param>
        /// <returns>序列化后的字符串</returns>
        Task<string> SerializeAsync<T>(T config, ConfigParserOptions options = null) where T : class;

        /// <summary>
        /// 将字符串反序列化为配置对象
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="content">序列化的字符串内容</param>
        /// <param name="options">解析选项</param>
        /// <returns>配置对象</returns>
        Task<T> DeserializeAsync<T>(string content, ConfigParserOptions options = null) where T : class;

        /// <summary>
        /// 将配置对象保存到文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="options">解析选项</param>
        Task SaveToFileAsync<T>(T config, string filePath, ConfigParserOptions options = null) where T : class;

        /// <summary>
        /// 从文件加载配置对象
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="options">解析选项</param>
        /// <returns>配置对象</returns>
        Task<T> LoadFromFileAsync<T>(string filePath, ConfigParserOptions options = null) where T : class;

        /// <summary>
        /// 检测配置文件格式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>配置格式</returns>
        Task<ConfigFormat> DetectFormatAsync(string filePath);
    }

    /// <summary>
    /// 默认配置解析器实现
    /// 提供基于Newtonsoft.Json的JSON配置解析功能
    /// </summary>
    public class DefaultConfigParser : IConfigParser
    {
        private readonly ConfigParserOptions _defaultOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultConfigParser()
        {
            _defaultOptions = ConfigParserOptions.Default();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultOptions">默认解析选项</param>
        public DefaultConfigParser(ConfigParserOptions defaultOptions)
        {
            _defaultOptions = defaultOptions ?? ConfigParserOptions.Default();
        }

        /// <summary>
        /// 将配置对象序列化为字符串
        /// </summary>
        public async Task<string> SerializeAsync<T>(T config, ConfigParserOptions options = null) where T : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            options = options ?? _defaultOptions;

            return await Task.Run(() =>
            {
                switch (options.Format)
                {
                    case ConfigFormat.Json:
                        var jsonSettings = options.JsonSettings ?? _defaultOptions.JsonSettings;
                        return JsonConvert.SerializeObject(config, options.FormatOutput ? Formatting.Indented : Formatting.None, jsonSettings);
                    
                    // 后续可以扩展支持其他格式
                    case ConfigFormat.Xml:
                        throw new NotSupportedException("XML格式尚未实现");
                    case ConfigFormat.Yaml:
                        throw new NotSupportedException("YAML格式尚未实现");
                    case ConfigFormat.Ini:
                        throw new NotSupportedException("INI格式尚未实现");
                    case ConfigFormat.Custom:
                        throw new NotSupportedException("自定义格式需要自定义解析器");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options.Format), options.Format, "不支持的配置格式");
                }
            });
        }

        /// <summary>
        /// 将字符串反序列化为配置对象
        /// </summary>
        public async Task<T> DeserializeAsync<T>(string content, ConfigParserOptions options = null) where T : class
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            options = options ?? _defaultOptions;

            return await Task.Run(() =>
            {
                switch (options.Format)
                {
                    case ConfigFormat.Json:
                        var jsonSettings = options.JsonSettings ?? _defaultOptions.JsonSettings;
                        jsonSettings.MissingMemberHandling = options.IgnoreUnknownProperties 
                            ? MissingMemberHandling.Ignore 
                            : MissingMemberHandling.Error;
                        
                        try
                        {
                            return JsonConvert.DeserializeObject<T>(content, jsonSettings);
                        }
                        catch (JsonException ex)
                        {
                            throw new ConfigParsingException("JSON解析错误", ex);
                        }
                    
                    // 后续可以扩展支持其他格式
                    case ConfigFormat.Xml:
                        throw new NotSupportedException("XML格式尚未实现");
                    case ConfigFormat.Yaml:
                        throw new NotSupportedException("YAML格式尚未实现");
                    case ConfigFormat.Ini:
                        throw new NotSupportedException("INI格式尚未实现");
                    case ConfigFormat.Custom:
                        throw new NotSupportedException("自定义格式需要自定义解析器");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options.Format), options.Format, "不支持的配置格式");
                }
            });
        }

        /// <summary>
        /// 将配置对象保存到文件
        /// </summary>
        public async Task SaveToFileAsync<T>(T config, string filePath, ConfigParserOptions options = null) where T : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            options = options ?? _defaultOptions;

            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 序列化并保存到文件
            string content = await SerializeAsync(config, options);
            await File.WriteAllTextAsync(filePath, content, options.Encoding);
        }

        /// <summary>
        /// 从文件加载配置对象
        /// </summary>
        public async Task<T> LoadFromFileAsync<T>(string filePath, ConfigParserOptions options = null) where T : class
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            
            if (!File.Exists(filePath))
                throw new FileNotFoundException("配置文件不存在", filePath);

            options = options ?? _defaultOptions;

            // 如果未指定格式，尝试自动检测
            if (options.Format == ConfigFormat.Json)
            {
                ConfigFormat detectedFormat = await DetectFormatAsync(filePath);
                if (detectedFormat != ConfigFormat.Json)
                {
                    options = new ConfigParserOptions(options)
                    {
                        Format = detectedFormat
                    };
                }
            }

            // 读取文件内容并反序列化
            string content = await File.ReadAllTextAsync(filePath, options.Encoding);
            return await DeserializeAsync<T>(content, options);
        }

        /// <summary>
        /// 检测配置文件格式
        /// </summary>
        public async Task<ConfigFormat> DetectFormatAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            
            if (!File.Exists(filePath))
                throw new FileNotFoundException("配置文件不存在", filePath);

            // 首先通过文件扩展名判断
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".json":
                    return ConfigFormat.Json;
                case ".xml":
                    return ConfigFormat.Xml;
                case ".yaml":
                case ".yml":
                    return ConfigFormat.Yaml;
                case ".ini":
                    return ConfigFormat.Ini;
            }

            // 如果扩展名无法确定，尝试通过文件内容判断
            try
            {
                // 读取文件前几个字符进行判断
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new StreamReader(stream))
                {
                    string firstLine = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(firstLine))
                    {
                        firstLine = firstLine.Trim();
                        // JSON判断
                        if (firstLine.StartsWith("{") || firstLine.StartsWith("["))
                        {
                            return ConfigFormat.Json;
                        }
                        // XML判断
                        if (firstLine.StartsWith("<?xml") || firstLine.StartsWith("<"))
                        {
                            return ConfigFormat.Xml;
                        }
                        // YAML判断
                        if (firstLine.StartsWith("#") || firstLine.Contains(": ") || firstLine == "---")
                        {
                            return ConfigFormat.Yaml;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // 如果无法通过内容判断，返回默认格式
            }

            // 默认返回JSON格式
            return ConfigFormat.Json;
        }
    }

    /// <summary>
    /// 配置解析异常
    /// 当配置解析过程中发生错误时抛出
    /// </summary>
    public class ConfigParsingException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        public ConfigParsingException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        public ConfigParsingException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 配置解析扩展类
    /// 提供配置解析相关的扩展方法
    /// </summary>
    public static class ConfigParserExtensions
    {
        /// <summary>
        /// 合并两个配置对象
        /// 第二个配置对象的值将覆盖第一个配置对象的值
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="parser">配置解析器</param>
        /// <param name="source">源配置对象</param>
        /// <param name="target">目标配置对象</param>
        /// <returns>合并后的配置对象</returns>
        public static async Task<T> MergeAsync<T>(this IConfigParser parser, T source, T target) where T : class, new()
        {
            if (source == null)
                return target ?? new T();
            
            if (target == null)
                return source;

            // 使用JSON序列化和反序列化来合并对象
            string sourceJson = await parser.SerializeAsync(source);
            string targetJson = await parser.SerializeAsync(target);

            JObject sourceObj = JObject.Parse(sourceJson);
            JObject targetObj = JObject.Parse(targetJson);

            // 将source的值合并到target中，但只有当target中不存在该属性或值为null时才替换
            foreach (var property in sourceObj.Properties())
            {
                if (targetObj.Property(property.Name) == null || targetObj[property.Name].Type == JTokenType.Null)
                {
                    targetObj[property.Name] = property.Value;
                }
            }

            return targetObj.ToObject<T>();
        }

        /// <summary>
        /// 验证配置文件是否有效
        /// </summary>
        /// <param name="parser">配置解析器</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>配置文件是否有效</returns>
        public static async Task<bool> IsValidConfigFileAsync(this IConfigParser parser, string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                ConfigFormat format = await parser.DetectFormatAsync(filePath);
                string content = await File.ReadAllTextAsync(filePath);

                switch (format)
                {
                    case ConfigFormat.Json:
                        JToken.Parse(content); // 尝试解析JSON，无效时会抛出异常
                        return true;
                    case ConfigFormat.Xml:
                        // 简单的XML验证，可以扩展为更严格的验证
                        try
                        {
                            using (var reader = new System.Xml.XmlTextReader(new StringReader(content)))
                            {
                                while (reader.Read()) { }
                            }
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    default:
                        // 对于其他格式，可以添加相应的验证逻辑
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}