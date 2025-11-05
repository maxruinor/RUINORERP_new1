/*************************************************************
 * 文件名：ConfigPathResolver.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置路径解析器接口和基础实现
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.IO;

namespace RUINORERP.Common.Config.Path
{
    /// <summary>
    /// 配置路径解析器接口
    /// 定义配置文件路径的解析和管理功能
    /// </summary>
    public interface IConfigPathResolver
    {
        /// <summary>
        /// 获取配置文件路径
        /// 根据配置类型名称生成配置文件路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置文件路径</returns>
        string GetConfigFilePath(string configTypeName);

        /// <summary>
        /// 获取配置目录路径
        /// 返回配置文件存储的根目录
        /// </summary>
        /// <returns>配置目录路径</returns>
        string GetConfigDirectory();

        /// <summary>
        /// 获取临时配置文件路径
        /// 用于临时存储或原子操作
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>临时配置文件路径</returns>
        string GetTempConfigFilePath(string configTypeName);

        /// <summary>
        /// 获取配置备份路径
        /// 用于配置备份存储
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="backupId">备份标识符</param>
        /// <returns>配置备份路径</returns>
        string GetBackupConfigFilePath(string configTypeName, string backupId = null);

        /// <summary>
        /// 获取旧版配置路径
        /// 用于配置迁移
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>旧版配置文件路径</returns>
        string GetLegacyConfigPath(string configTypeName);

        /// <summary>
        /// 判断是否应该使用旧路径
        /// 根据配置和环境判断是否需要使用旧版路径
        /// </summary>
        /// <returns>是否应该使用旧路径</returns>
        bool ShouldUseLegacyPath();

        /// <summary>
        /// 按需迁移配置
        /// 如果需要，从旧路径迁移配置到新路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>是否执行了迁移</returns>
        Task<bool> MigrateConfigIfNeededAsync(string configTypeName);

        /// <summary>
        /// 批量迁移所有配置
        /// 迁移所有旧版配置文件到新路径
        /// </summary>
        /// <returns>迁移的配置数量</returns>
        Task<int> MigrateAllConfigsAsync();

        /// <summary>
        /// 验证配置路径
        /// 检查配置文件路径是否有效
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <returns>验证结果</returns>
        bool ValidateConfigPath(string configPath);

        /// <summary>
        /// 确保配置目录存在
        /// 如果不存在则创建目录
        /// </summary>
        /// <returns>是否成功确保目录存在</returns>
        bool EnsureConfigDirectoryExists();

        /// <summary>
        /// 获取配置路径诊断信息
        /// 提供配置路径相关的诊断信息
        /// </summary>
        /// <returns>配置路径诊断信息</returns>
        ConfigPathDiagnostics GetPathDiagnostics();

        /// <summary>
        /// 检查目录访问权限
        /// 检查是否有对配置目录的读写权限
        /// </summary>
        /// <returns>是否有权限</returns>
        bool CheckDirectoryAccess();
    }

    /// <summary>
    /// 配置路径诊断信息
    /// 包含配置路径相关的诊断数据
    /// </summary>
    public class ConfigPathDiagnostics
    {
        /// <summary>
        /// 当前配置目录
        /// </summary>
        public string CurrentConfigDirectory { get; set; }

        /// <summary>
        /// 旧版配置目录
        /// </summary>
        public string LegacyConfigDirectory { get; set; }

        /// <summary>
        /// 配置目录是否存在
        /// </summary>
        public bool ConfigDirectoryExists { get; set; }

        /// <summary>
        /// 旧版配置目录是否存在
        /// </summary>
        public bool LegacyDirectoryExists { get; set; }

        /// <summary>
        /// 是否有写入权限
        /// </summary>
        public bool HasWritePermission { get; set; }

        /// <summary>
        /// 是否有读取权限
        /// </summary>
        public bool HasReadPermission { get; set; }

        /// <summary>
        /// 配置文件数量
        /// 当前目录下的配置文件数量
        /// </summary>
        public int ConfigFileCount { get; set; }

        /// <summary>
        /// 旧版配置文件数量
        /// 旧版目录下的配置文件数量
        /// </summary>
        public int LegacyConfigFileCount { get; set; }

        /// <summary>
        /// 错误信息
        /// 如果有任何问题，包含错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 获取诊断摘要
        /// </summary>
        /// <returns>诊断摘要文本</returns>
        public override string ToString()
        {
            var issues = new System.Text.StringBuilder();
            
            if (!ConfigDirectoryExists)
                issues.AppendLine("配置目录不存在");
            if (!HasWritePermission)
                issues.AppendLine("无配置目录写入权限");
            if (!HasReadPermission)
                issues.AppendLine("无配置目录读取权限");
            if (!string.IsNullOrEmpty(ErrorMessage))
                issues.AppendLine(ErrorMessage);
            
            return issues.Length > 0 
                ? $"配置路径诊断发现问题:\n{issues}" 
                : $"配置路径正常。当前目录: {CurrentConfigDirectory}, 文件数: {ConfigFileCount}";
        }
    }

    /// <summary>
    /// 基础配置路径解析器
    /// 提供配置路径解析器的基础实现
    /// </summary>
    public class BaseConfigPathResolver : IConfigPathResolver
    {
        /// <summary>
        /// 默认配置文件扩展名
        /// </summary>
        protected const string DefaultConfigExtension = ".json";

        /// <summary>
        /// 配置目录
        /// </summary>
        protected readonly string _configDirectory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configDirectory">配置目录</param>
        public BaseConfigPathResolver(string configDirectory = null)
        {
            _configDirectory = configDirectory ?? GetDefaultConfigDirectory();
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置文件路径</returns>
        public virtual string GetConfigFilePath(string configTypeName, ConfigPathType pathType = ConfigPathType.Server)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));

            EnsureConfigDirectoryExists(pathType);
            return Path.Combine(GetConfigDirectory(pathType), $"{configTypeName}{DefaultConfigExtension}");
        }

        /// <summary>
        /// 获取配置目录路径
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置目录路径</returns>
        public virtual string GetConfigDirectory(ConfigPathType pathType = ConfigPathType.Server)
        {
            switch (pathType)
            {
                case ConfigPathType.Server:
                    return Path.Combine(AppContext.BaseDirectory, "SysConfigFiles");
                case ConfigPathType.Client:
                    string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    return Path.Combine(appDataDir, "RUINORERP", "Configs");
                case ConfigPathType.Version:
                    return Path.Combine(GetConfigDirectory(ConfigPathType.Server), "Versions");
                default:
                    throw new ArgumentOutOfRangeException(nameof(pathType), pathType, "无效的配置路径类型");
            }
        }

        /// <summary>
        /// 确保配置目录存在
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        public virtual void EnsureConfigDirectoryExists(ConfigPathType pathType = ConfigPathType.Server)
        {
            string directory = GetConfigDirectory(pathType);
            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    throw new IOException($"无法创建配置目录: {directory}", ex);
                }
            }
        }

        /// <summary>
        /// 获取默认配置目录
        /// </summary>
        /// <returns>默认配置目录路径</returns>
        protected virtual string GetDefaultConfigDirectory()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataDir, "RUINORERP", "Config");
        }
    }

    /// <summary>
    /// 配置路径解析器扩展方法
    /// 提供便捷的路径操作方法
    /// </summary>
    public static class ConfigPathResolverExtensions
    {
        /// <summary>
        /// 确保配置目录可访问
        /// 检查并尝试创建可访问的配置目录
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        /// <returns>是否成功确保目录可访问</returns>
        public static bool EnsureAccessibleConfigDirectory(this IConfigPathResolver pathResolver)
        {
            if (!pathResolver.EnsureConfigDirectoryExists())
            {
                // 如果默认目录无法访问，尝试使用替代目录
                return false;
            }

            return pathResolver.CheckDirectoryAccess();
        }

        /// <summary>
        /// 生成配置文件名称
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置文件名称</returns>
        public static string GenerateConfigFileName(this IConfigPathResolver pathResolver, string configTypeName)
        {
            return $"{configTypeName}{BaseConfigPathResolver.DefaultConfigExtension}";
        }
    }
}