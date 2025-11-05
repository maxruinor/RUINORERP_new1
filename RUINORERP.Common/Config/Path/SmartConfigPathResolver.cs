/*************************************************************
 * 文件名：SmartConfigPathResolver.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：智能配置路径解析器，支持配置迁移和路径管理
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Path
{
    /// <summary>
    /// 智能配置路径解析器
    /// 实现配置路径的智能解析和配置迁移功能
    /// </summary>
    public class SmartConfigPathResolver : IConfigPathResolver
    {
        private readonly string _baseConfigDirectory;
        private readonly string _legacyConfigDirectory;
        private readonly ILogger<SmartConfigPathResolver> _logger;
        private readonly object _lockObj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseConfigDirectory">基础配置目录</param>
        /// <param name="legacyConfigDirectory">旧版配置目录（可选）</param>
        /// <param name="logger">日志记录器</param>
        public SmartConfigPathResolver(
            string baseConfigDirectory,
            string legacyConfigDirectory = null,
            ILogger<SmartConfigPathResolver> logger = null)
        {
            if (string.IsNullOrEmpty(baseConfigDirectory))
                throw new ArgumentNullException(nameof(baseConfigDirectory));

            _baseConfigDirectory = baseConfigDirectory;
            _legacyConfigDirectory = legacyConfigDirectory ?? Path.Combine(AppContext.BaseDirectory, "Config");
            _logger = logger;

            // 确保配置目录存在
            EnsureDirectoriesExist();
        }

        /// <summary>
        /// 确保配置目录存在
        /// </summary>
        private void EnsureDirectoriesExist()
        {
            lock (_lockObj)
            {
                if (!Directory.Exists(_baseConfigDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(_baseConfigDirectory);
                        _logger?.LogInformation("配置目录已创建: {Directory}", _baseConfigDirectory);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "创建配置目录失败: {Directory}", _baseConfigDirectory);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>配置文件的完整路径</returns>
        public string GetConfigFilePath(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));

            // 确保目录存在
            EnsureDirectoriesExist();

            // 构建配置文件路径
            string fileName = $"{configTypeName}.json";
            string fullPath = System.IO.Path.Combine(_baseConfigDirectory, fileName);

            _logger?.LogTrace("获取配置文件路径: {Path}", fullPath);
            return fullPath;
        }

        /// <summary>
        /// 获取客户端配置文件路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>客户端配置文件的完整路径</returns>
        public string GetClientConfigFilePath(string configTypeName, string clientId)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException(nameof(clientId));

            // 构建客户端配置目录
            string clientDir = System.IO.Path.Combine(_baseConfigDirectory, "Clients", clientId);

            // 确保客户端目录存在
            if (!Directory.Exists(clientDir))
            {
                try
                {
                    Directory.CreateDirectory(clientDir);
                    _logger?.LogInformation("客户端配置目录已创建: {Directory}", clientDir);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "创建客户端配置目录失败: {Directory}", clientDir);
                    throw;
                }
            }

            // 构建配置文件路径
            string fileName = $"{configTypeName}.json";
            string fullPath = System.IO.Path.Combine(clientDir, fileName);

            _logger?.LogTrace("获取客户端配置文件路径: {Path}", fullPath);
            return fullPath;
        }

        /// <summary>
        /// 获取配置目录
        /// </summary>
        /// <returns>配置目录路径</returns>
        public string GetConfigDirectory()
        {
            EnsureDirectoriesExist();
            return _baseConfigDirectory;
        }

        /// <summary>
        /// 获取旧版配置路径
        /// 用于配置迁移
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>旧版配置文件的完整路径</returns>
        public string GetLegacyConfigPath(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));

            string fileName = $"{configTypeName}.json";
            string fullPath = System.IO.Path.Combine(_legacyConfigDirectory, fileName);

            _logger?.LogTrace("获取旧版配置文件路径: {Path}", fullPath);
            return fullPath;
        }

        /// <summary>
        /// 判断是否应该使用旧版路径
        /// 当新版配置不存在而旧版配置存在时返回true
        /// </summary>
        /// <returns>是否应该使用旧版路径</returns>
        public bool ShouldUseLegacyPath()
        {
            try
            {
                // 检查新版配置目录是否存在且非空
                if (Directory.Exists(_baseConfigDirectory) && Directory.GetFiles(_baseConfigDirectory, "*.json").Length > 0)
                    return false;

                // 检查旧版配置目录是否存在且非空
                if (Directory.Exists(_legacyConfigDirectory) && Directory.GetFiles(_legacyConfigDirectory, "*.json").Length > 0)
                {
                    _logger?.LogInformation("检测到旧版配置目录存在，新版配置目录为空，应使用旧版路径");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查是否应该使用旧版路径时出错");
                return false;
            }
        }

        /// <summary>
        /// 按需迁移配置
        /// 当新版配置不存在而旧版配置存在时，自动迁移配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>是否成功迁移配置</returns>
        public async Task<bool> MigrateConfigIfNeededAsync(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new ArgumentNullException(nameof(configTypeName));

            try
            {
                var legacyPath = GetLegacyConfigPath(configTypeName);
                var newPath = GetConfigFilePath(configTypeName);

                // 检查是否需要迁移
                if (!File.Exists(legacyPath))
                {
                    _logger?.LogTrace("不需要迁移配置: 旧版配置文件不存在 - {ConfigType}", configTypeName);
                    return false;
                }

                if (File.Exists(newPath))
                {
                    _logger?.LogTrace("不需要迁移配置: 新版配置文件已存在 - {ConfigType}", configTypeName);
                    return false;
                }

                // 确保目标目录存在
                string newDir = System.IO.Path.GetDirectoryName(newPath);
                if (!Directory.Exists(newDir))
                    Directory.CreateDirectory(newDir);

                // 复制文件（自动迁移配置）
                File.Copy(legacyPath, newPath, true);
                _logger?.LogInformation("配置已自动迁移: {ConfigType} 从 {LegacyPath} 到 {NewPath}", 
                    configTypeName, legacyPath, newPath);

                // 记录迁移日志
                await LogMigrationAsync(configTypeName, legacyPath, newPath);

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "迁移配置失败: {ConfigType}", configTypeName);
                return false;
            }
        }

        /// <summary>
        /// 批量迁移所有配置
        /// </summary>
        /// <returns>迁移的配置数量</returns>
        public async Task<int> MigrateAllConfigsAsync()
        {
            int migratedCount = 0;

            try
            {
                // 检查旧版目录是否存在
                if (!Directory.Exists(_legacyConfigDirectory))
                {
                    _logger?.LogInformation("不需要批量迁移配置: 旧版配置目录不存在");
                    return 0;
                }

                _logger?.LogInformation("开始批量迁移配置...");

                // 获取旧版目录中的所有配置文件
                string[] legacyFiles = Directory.GetFiles(_legacyConfigDirectory, "*.json");
                
                foreach (string legacyFile in legacyFiles)
                {
                    try
                    {
                        string fileName = System.IO.Path.GetFileName(legacyFile);
                        string configTypeName = System.IO.Path.GetFileNameWithoutExtension(fileName);

                        if (await MigrateConfigIfNeededAsync(configTypeName))
                            migratedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "迁移配置文件失败: {File}", legacyFile);
                        // 继续处理下一个文件
                    }
                }

                _logger?.LogInformation("批量迁移配置完成，共迁移 {Count} 个配置", migratedCount);
                return migratedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量迁移配置过程中出错");
                return migratedCount; // 返回已成功迁移的数量
            }
        }

        /// <summary>
        /// 验证配置路径的可访问性
        /// </summary>
        /// <returns>路径是否可访问</returns>
        public bool ValidatePathAccessibility()
        {
            try
            {
                // 检查目录是否存在
                if (!Directory.Exists(_baseConfigDirectory))
                {
                    _logger?.LogWarning("配置目录不存在: {Directory}", _baseConfigDirectory);
                    return false;
                }

                // 检查读权限
                if (!HasReadPermission(_baseConfigDirectory))
                {
                    _logger?.LogWarning("没有配置目录的读取权限: {Directory}", _baseConfigDirectory);
                    return false;
                }

                // 检查写权限
                if (!HasWritePermission(_baseConfigDirectory))
                {
                    _logger?.LogWarning("没有配置目录的写入权限: {Directory}", _baseConfigDirectory);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证配置路径访问权限时出错");
                return false;
            }
        }

        /// <summary>
        /// 检查目录是否有读取权限
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>是否有读取权限</returns>
        private bool HasReadPermission(string directoryPath)
        {
            try
            {
                // 尝试获取目录中的文件列表
                Directory.GetFiles(directoryPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查目录是否有写入权限
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>是否有写入权限</returns>
        private bool HasWritePermission(string directoryPath)
        {
            try
            {
                // 尝试创建临时文件
                string tempFile = System.IO.Path.Combine(directoryPath, $"temp_{Guid.NewGuid().ToString()}.txt");
                File.WriteAllText(tempFile, "");
                File.Delete(tempFile);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// 记录配置迁移日志
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="legacyPath">旧版配置路径</param>
        /// <param name="newPath">新版配置路径</param>
        /// <returns>任务</returns>
        protected virtual async Task LogMigrationAsync(string configTypeName, string legacyPath, string newPath)
        {
            // 这里可以实现更复杂的日志记录逻辑
            // 例如写入迁移日志文件或发送通知
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取配置路径解析器的诊断信息
        /// </summary>
        /// <returns>诊断信息字典</returns>
        public ConfigPathDiagnostics GetDiagnostics()
        {
            return new ConfigPathDiagnostics
            {
                BaseConfigDirectory = _baseConfigDirectory,
                LegacyConfigDirectory = _legacyConfigDirectory,
                BaseDirectoryExists = Directory.Exists(_baseConfigDirectory),
                LegacyDirectoryExists = Directory.Exists(_legacyConfigDirectory),
                ShouldUseLegacyPath = ShouldUseLegacyPath(),
                HasWritePermission = HasWritePermission(_baseConfigDirectory),
                HasReadPermission = HasReadPermission(_baseConfigDirectory),
                BaseConfigFileCount = Directory.Exists(_baseConfigDirectory) ? Directory.GetFiles(_baseConfigDirectory, "*.json").Length : 0,
                LegacyConfigFileCount = Directory.Exists(_legacyConfigDirectory) ? Directory.GetFiles(_legacyConfigDirectory, "*.json").Length : 0
            };
        }
    }

    /// <summary>
    /// 配置路径诊断信息
    /// 用于存储配置路径的诊断信息
    /// </summary>
    public class ConfigPathDiagnostics
    {
        /// <summary>
        /// 基础配置目录
        /// </summary>
        public string BaseConfigDirectory { get; set; }

        /// <summary>
        /// 旧版配置目录
        /// </summary>
        public string LegacyConfigDirectory { get; set; }

        /// <summary>
        /// 基础目录是否存在
        /// </summary>
        public bool BaseDirectoryExists { get; set; }

        /// <summary>
        /// 旧版目录是否存在
        /// </summary>
        public bool LegacyDirectoryExists { get; set; }

        /// <summary>
        /// 是否应该使用旧版路径
        /// </summary>
        public bool ShouldUseLegacyPath { get; set; }

        /// <summary>
        /// 是否有写入权限
        /// </summary>
        public bool HasWritePermission { get; set; }

        /// <summary>
        /// 是否有读取权限
        /// </summary>
        public bool HasReadPermission { get; set; }

        /// <summary>
        /// 基础配置文件数量
        /// </summary>
        public int BaseConfigFileCount { get; set; }

        /// <summary>
        /// 旧版配置文件数量
        /// </summary>
        public int LegacyConfigFileCount { get; set; }

        /// <summary>
        /// 验证诊断信息
        /// </summary>
        /// <returns>是否通过验证</returns>
        public bool IsValid()
        {
            return BaseDirectoryExists && HasReadPermission && HasWritePermission;
        }
    }

    /// <summary>
    /// 智能配置路径解析器扩展
    /// 提供便捷的依赖注入方法
    /// </summary>
    public static class SmartConfigPathResolverExtensions
    {
        /// <summary>
        /// 添加智能配置路径解析器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configDirectory">配置目录（可选）</param>
        /// <param name="legacyConfigDirectory">旧版配置目录（可选）</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSmartConfigPathResolver(
            this IServiceCollection services,
            string configDirectory = null,
            string legacyConfigDirectory = null)
        {
            // 如果未指定配置目录，使用应用程序数据目录下的配置文件夹
            if (string.IsNullOrEmpty(configDirectory))
            {
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                configDirectory = System.IO.Path.Combine(appDataDir, "RUINORERP", "Config");
            }

            services.AddSingleton<IConfigPathResolver>(provider =>
            {
                var logger = provider.GetService<ILogger<SmartConfigPathResolver>>();
                return new SmartConfigPathResolver(configDirectory, legacyConfigDirectory, logger);
            });

            return services;
        }
    }
}