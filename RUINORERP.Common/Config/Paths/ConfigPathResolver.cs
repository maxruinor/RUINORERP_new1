/*************************************************************
 * 文件名：ConfigPathResolver.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置路径解析器
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RUINORERP.Common.Config.Environment;

namespace RUINORERP.Common.Config.Paths
{
    /// <summary>
    /// 路径类型
    /// </summary>
    public enum ConfigPathType
    {
        /// <summary>
        /// 绝对路径
        /// </summary>
        Absolute,
        
        /// <summary>
        /// 相对路径
        /// </summary>
        Relative,
        
        /// <summary>
        /// 环境路径（使用环境变量）
        /// </summary>
        Environment,
        
        /// <summary>
        /// 特殊路径（如 ~/ 等）
        /// </summary>
        Special,
        
        /// <summary>
        /// 网络路径
        /// </summary>
        Network
    }

    /// <summary>
    /// 配置路径解析器接口
    /// </summary>
    public interface IConfigPathResolver
    {
        /// <summary>
        /// 解析配置路径
        /// </summary>
        /// <param name="path">输入路径</param>
        /// <param name="basePath">基础路径</param>
        /// <returns>解析后的完整路径</returns>
        string ResolvePath(string path, string basePath = null);
        
        /// <summary>
        /// 获取配置目录
        /// </summary>
        /// <param name="environment">环境名称</param>
        /// <returns>配置目录路径</returns>
        string GetConfigDirectory(string environment = null);
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="environment">环境名称</param>
        /// <param name="baseDir">基础目录</param>
        /// <returns>配置文件完整路径</returns>
        string GetConfigFilePath(string fileName, string environment = null, string baseDir = null);
        
        /// <summary>
        /// 检查路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否存在</returns>
        bool Exists(string path);
        
        /// <summary>
        /// 创建目录（如果不存在）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>创建后的路径</returns>
        string EnsureDirectory(string path);
        
        /// <summary>
        /// 获取路径类型
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>路径类型</returns>
        ConfigPathType GetPathType(string path);
        
        /// <summary>
        /// 规范化路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>规范化后的路径</returns>
        string NormalizePath(string path);
    }

    /// <summary>
    /// 默认配置路径解析器
    /// </summary>
    public class DefaultConfigPathResolver : IConfigPathResolver
    {
        private readonly IConfigEnvironmentProvider _environmentProvider;
        private readonly List<string> _configDirectories = new List<string>();
        private readonly Regex _environmentVariableRegex = new Regex(@"%([^%]+)%", RegexOptions.Compiled);
        private readonly Regex _tildeRegex = new Regex(@"^~", RegexOptions.Compiled);
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="environmentProvider">环境提供者</param>
        public DefaultConfigPathResolver(IConfigEnvironmentProvider environmentProvider = null)
        {
            _environmentProvider = environmentProvider ?? new DefaultConfigEnvironmentProvider();
            RegisterDefaultConfigDirectories();
        }
        
        /// <summary>
        /// 注册默认配置目录
        /// </summary>
        private void RegisterDefaultConfigDirectories()
        {
            // 注册标准配置目录（按优先级排序）
            _configDirectories.Add(Path.Combine(AppContext.BaseDirectory, "Config"));
            _configDirectories.Add(Path.Combine(AppContext.BaseDirectory, "Configuration"));
            _configDirectories.Add(AppContext.BaseDirectory);
            _configDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RUINORERP", "Config"));
        }
        
        /// <summary>
        /// 解析配置路径
        /// </summary>
        /// <param name="path">输入路径</param>
        /// <param name="basePath">基础路径</param>
        /// <returns>解析后的完整路径</returns>
        public string ResolvePath(string path, string basePath = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            
            // 规范化路径
            string normalizedPath = NormalizePath(path);
            
            // 解析环境变量
            normalizedPath = ResolveEnvironmentVariables(normalizedPath);
            
            // 处理特殊路径
            normalizedPath = ResolveSpecialPaths(normalizedPath);
            
            // 检查是否为绝对路径
            if (Path.IsPathRooted(normalizedPath))
                return normalizedPath;
            
            // 使用基础路径或当前目录
            string resolvedBasePath = string.IsNullOrEmpty(basePath) ? GetDefaultBasePath() : basePath;
            return Path.Combine(resolvedBasePath, normalizedPath);
        }
        
        /// <summary>
        /// 解析环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的路径</returns>
        private string ResolveEnvironmentVariables(string path)
        {
            return _environmentVariableRegex.Replace(path, match =>
            {
                string variableName = match.Groups[1].Value;
                string variableValue = _environmentProvider.GetEnvironmentVariable(variableName);
                return string.IsNullOrEmpty(variableValue) ? match.Value : variableValue;
            });
        }
        
        /// <summary>
        /// 解析特殊路径
        /// </summary>
        /// <param name="path">包含特殊路径的字符串</param>
        /// <returns>解析后的路径</returns>
        private string ResolveSpecialPaths(string path)
        {
            // 处理 ~/ 路径（用户目录）
            if (_tildeRegex.IsMatch(path))
            {
                string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                path = _tildeRegex.Replace(path, homeDirectory, 1);
            }
            
            // 处理其他特殊路径
            path = path.Replace("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path = path.Replace("%LOCALAPPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            path = path.Replace("%TEMP%", Path.GetTempPath());
            
            return path;
        }
        
        /// <summary>
        /// 获取默认基础路径
        /// </summary>
        /// <returns>默认基础路径</returns>
        private string GetDefaultBasePath()
        {
            // 尝试查找存在的配置目录
            foreach (string dir in _configDirectories)
            {
                if (Directory.Exists(dir))
                    return dir;
            }
            
            // 如果没有找到，返回应用程序基础目录
            return AppContext.BaseDirectory;
        }
        
        /// <summary>
        /// 获取配置目录
        /// </summary>
        /// <param name="environment">环境名称</param>
        /// <returns>配置目录路径</returns>
        public string GetConfigDirectory(string environment = null)
        {
            string envName = string.IsNullOrEmpty(environment) ? _environmentProvider.GetCurrentEnvironment() : environment;
            
            // 尝试获取环境特定的配置目录
            string envConfigDir = Path.Combine(GetDefaultBasePath(), envName);
            if (Directory.Exists(envConfigDir))
                return envConfigDir;
            
            // 使用默认配置目录
            string defaultConfigDir = GetDefaultBasePath();
            
            // 如果配置目录不存在，尝试创建
            return EnsureDirectory(defaultConfigDir);
        }
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="environment">环境名称</param>
        /// <param name="baseDir">基础目录</param>
        /// <returns>配置文件完整路径</returns>
        public string GetConfigFilePath(string fileName, string environment = null, string baseDir = null)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            
            // 获取基础目录
            string configDir = string.IsNullOrEmpty(baseDir) ? GetConfigDirectory(environment) : baseDir;
            
            // 构建完整路径
            string filePath = Path.Combine(configDir, fileName);
            
            // 检查文件是否存在，如果不存在尝试查找环境特定的文件
            if (!File.Exists(filePath))
            {
                string envName = string.IsNullOrEmpty(environment) ? _environmentProvider.GetCurrentEnvironment() : environment;
                string envFileName = GetEnvironmentFileName(fileName, envName);
                string envFilePath = Path.Combine(configDir, envFileName);
                
                if (File.Exists(envFilePath))
                    return envFilePath;
            }
            
            return filePath;
        }
        
        /// <summary>
        /// 获取环境特定的文件名
        /// </summary>
        /// <param name="fileName">基础文件名</param>
        /// <param name="environment">环境名称</param>
        /// <returns>环境特定的文件名</returns>
        private string GetEnvironmentFileName(string fileName, string environment)
        {
            string extension = Path.GetExtension(fileName);
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            return $"{nameWithoutExtension}.{environment}{extension}";
        }
        
        /// <summary>
        /// 检查路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否存在</returns>
        public bool Exists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            
            string resolvedPath = ResolvePath(path);
            return File.Exists(resolvedPath) || Directory.Exists(resolvedPath);
        }
        
        /// <summary>
        /// 创建目录（如果不存在）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>创建后的路径</returns>
        public string EnsureDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            
            string resolvedPath = ResolvePath(path);
            
            // 确保目录存在
            Directory.CreateDirectory(resolvedPath);
            
            return resolvedPath;
        }
        
        /// <summary>
        /// 获取路径类型
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>路径类型</returns>
        public ConfigPathType GetPathType(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            
            // 检查网络路径
            if (path.StartsWith(@"\\") || path.StartsWith("//"))
                return ConfigPathType.Network;
            
            // 检查环境变量路径
            if (_environmentVariableRegex.IsMatch(path))
                return ConfigPathType.Environment;
            
            // 检查特殊路径
            if (_tildeRegex.IsMatch(path) || path.Contains("%APPDATA%") || path.Contains("%LOCALAPPDATA%") || path.Contains("%TEMP%"))
                return ConfigPathType.Special;
            
            // 检查绝对路径
            if (Path.IsPathRooted(path))
                return ConfigPathType.Absolute;
            
            // 默认相对路径
            return ConfigPathType.Relative;
        }
        
        /// <summary>
        /// 规范化路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>规范化后的路径</returns>
        public string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            
            // 替换路径分隔符为当前平台的标准分隔符
            string normalized = path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            
            // 规范化路径格式
            normalized = Path.GetFullPath(normalized);
            
            return normalized;
        }
        
        /// <summary>
        /// 注册配置目录
        /// </summary>
        /// <param name="directory">目录路径</param>
        /// <param name="priority">优先级（值越小优先级越高）</param>
        public void RegisterConfigDirectory(string directory, int priority = 0)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentNullException(nameof(directory));
            
            // 解析并规范化目录路径
            string normalizedDir = NormalizePath(directory);
            
            // 如果目录已存在，先移除
            _configDirectories.RemoveAll(d => d.Equals(normalizedDir, StringComparison.OrdinalIgnoreCase));
            
            // 根据优先级添加目录
            if (priority <= 0 || priority >= _configDirectories.Count)
            {
                // 添加到开头（最高优先级）
                _configDirectories.Insert(0, normalizedDir);
            }
            else
            {
                // 添加到指定位置
                _configDirectories.Insert(priority, normalizedDir);
            }
        }
        
        /// <summary>
        /// 获取所有可能的配置文件路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="environment">环境名称</param>
        /// <returns>配置文件路径列表</returns>
        public List<string> GetAllPossibleConfigPaths(string fileName, string environment = null)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            
            var paths = new List<string>();
            string envName = string.IsNullOrEmpty(environment) ? _environmentProvider.GetCurrentEnvironment() : environment;
            string envFileName = GetEnvironmentFileName(fileName, envName);
            
            // 为每个配置目录构建可能的路径
            foreach (string dir in _configDirectories)
            {
                // 添加基础文件名路径
                paths.Add(Path.Combine(dir, fileName));
                
                // 添加环境特定文件名路径
                paths.Add(Path.Combine(dir, envFileName));
            }
            
            // 添加当前目录路径
            paths.Add(Path.Combine(Environment.CurrentDirectory, fileName));
            paths.Add(Path.Combine(Environment.CurrentDirectory, envFileName));
            
            return paths.Distinct().ToList();
        }
    }
    
    /// <summary>
    /// 配置路径扩展方法
    /// </summary>
    public static class ConfigPathExtensions
    {
        /// <summary>
        /// 尝试解析配置文件路径
        /// </summary>
        /// <param name="pathResolver">路径解析器</param>
        /// <param name="fileName">文件名</param>
        /// <param name="resolvedPath">解析后的路径</param>
        /// <param name="environment">环境名称</param>
        /// <returns>是否成功解析</returns>
        public static bool TryResolveConfigPath(this IConfigPathResolver pathResolver, 
            string fileName, out string resolvedPath, string environment = null)
        {
            if (pathResolver == null)
                throw new ArgumentNullException(nameof(pathResolver));
            
            try
            {
                var allPaths = pathResolver.GetAllPossibleConfigPaths(fileName, environment);
                
                foreach (string path in allPaths)
                {
                    if (File.Exists(path))
                    {
                        resolvedPath = path;
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // 忽略异常
            }
            
            resolvedPath = null;
            return false;
        }
        
        /// <summary>
        /// 获取配置文件的备份路径
        /// </summary>
        /// <param name="pathResolver">路径解析器</param>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns>备份文件路径</returns>
        public static string GetBackupPath(this IConfigPathResolver pathResolver, 
            string configPath, string timestamp = null)
        {
            if (pathResolver == null)
                throw new ArgumentNullException(nameof(pathResolver));
            
            if (string.IsNullOrEmpty(configPath))
                throw new ArgumentNullException(nameof(configPath));
            
            string timestampStr = timestamp ?? DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string extension = Path.GetExtension(configPath);
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(configPath);
            string directory = Path.GetDirectoryName(configPath);
            
            // 构建备份文件名
            string backupFileName = $"{nameWithoutExtension}.{timestampStr}.bak{extension}";
            
            return Path.Combine(directory ?? string.Empty, backupFileName);
        }
        
        /// <summary>
        /// 确保配置文件的目录存在
        /// </summary>
        /// <param name="pathResolver">路径解析器</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件路径</returns>
        public static string EnsureFilePathDirectory(this IConfigPathResolver pathResolver, string filePath)
        {
            if (pathResolver == null)
                throw new ArgumentNullException(nameof(pathResolver));
            
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                pathResolver.EnsureDirectory(directory);
            }
            
            return filePath;
        }
        
        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="pathResolver">路径解析器</param>
        /// <param name="fullPath">完整路径</param>
        /// <param name="basePath">基础路径</param>
        /// <returns>相对路径</returns>
        public static string GetRelativePath(this IConfigPathResolver pathResolver, 
            string fullPath, string basePath = null)
        {
            if (pathResolver == null)
                throw new ArgumentNullException(nameof(pathResolver));
            
            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentNullException(nameof(fullPath));
            
            string resolvedFullPath = pathResolver.ResolvePath(fullPath);
            string resolvedBasePath = string.IsNullOrEmpty(basePath) ? 
                pathResolver.GetDefaultBasePath() : pathResolver.ResolvePath(basePath);
            
            Uri fullUri = new Uri(resolvedFullPath);
            Uri baseUri = new Uri(resolvedBasePath + Path.DirectorySeparatorChar);
            
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
        
        /// <summary>
        /// 获取默认基础路径
        /// </summary>
        /// <param name="pathResolver">路径解析器</param>
        /// <returns>默认基础路径</returns>
        private static string GetDefaultBasePath(this IConfigPathResolver pathResolver)
        {
            // 这个方法是为了扩展方法内部使用
            if (pathResolver is DefaultConfigPathResolver defaultResolver)
            {
                return defaultResolver.GetConfigDirectory();
            }
            return AppContext.BaseDirectory;
        }
    }
}