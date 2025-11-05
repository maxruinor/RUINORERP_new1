/*************************************************************
 * 文件名：SmartConfigPathResolver.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：智能配置路径解析器，支持配置迁移和兼容性管理
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 智能配置路径解析器
    /// 继承自默认实现，并添加配置迁移和兼容性功能
    /// 支持从旧配置系统平滑迁移到新系统
    /// </summary>
    public class SmartConfigPathResolver : DefaultConfigPathResolver, IConfigPathResolver
    {
        private readonly ILogger<SmartConfigPathResolver> _logger;
        private readonly bool _enableMigration;
        private readonly string _legacyConfigDir;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="enableMigration">是否启用配置迁移</param>
        public SmartConfigPathResolver(ILogger<SmartConfigPathResolver> logger = null, bool enableMigration = true)
        {
            _logger = logger;
            _enableMigration = enableMigration;
            
            // 定义旧配置目录（从旧代码中提取的硬编码路径）
            _legacyConfigDir = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
        }

        /// <summary>
        /// 获取旧路径用于迁移
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>旧配置文件的路径</returns>
        /// <exception cref="ArgumentException">当配置类型名称为空时抛出</exception>
        public string GetLegacyConfigPath(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
            {
                throw new ArgumentException("配置类型名称不能为空", nameof(configTypeName));
            }

            return Path.Combine(_legacyConfigDir, $"{configTypeName}.json");
        }

        /// <summary>
        /// 判断是否应该使用旧路径
        /// 默认规则：如果启用了迁移，且新配置不存在而旧配置存在，则返回true
        /// </summary>
        /// <returns>是否应该使用旧路径</returns>
        public bool ShouldUseLegacyPath()
        {
            // 仅在启用迁移时考虑使用旧路径
            if (!_enableMigration)
            {
                return false;
            }

            try
            {
                // 获取新配置目录
                string newConfigDir = GetConfigDirectory(ConfigPathType.Server);
                
                // 检查是否存在旧配置目录且有文件，而新配置目录为空或不存在
                bool hasLegacyConfigs = Directory.Exists(_legacyConfigDir) && Directory.GetFiles(_legacyConfigDir, "*.json").Length > 0;
                bool hasNewConfigs = Directory.Exists(newConfigDir) && Directory.GetFiles(newConfigDir, "*.json").Length > 0;
                
                return hasLegacyConfigs && !hasNewConfigs;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "判断是否使用旧路径时出错");
                return false;
            }
        }

        /// <summary>
        /// 按需迁移配置
        /// 当旧配置存在而新配置不存在时，自动迁移配置文件
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>是否成功迁移配置</returns>
        public async Task<bool> MigrateConfigIfNeededAsync(string configTypeName)
        {
            if (!_enableMigration)
            {
                return false;
            }

            try
            {
                var legacyPath = GetLegacyConfigPath(configTypeName);
                var newPath = GetConfigFilePath(configTypeName, ConfigPathType.Server);
                
                // 检查旧配置是否存在且新配置是否不存在
                if (File.Exists(legacyPath) && !File.Exists(newPath))
                {
                    _logger?.LogInformation("开始迁移配置文件: {ConfigType} 从 {LegacyPath} 到 {NewPath}", 
                        configTypeName, legacyPath, newPath);

                    // 确保新配置目录存在
                    EnsureConfigDirectoryExists(ConfigPathType.Server);
                    
                    // 自动迁移配置文件（复制）
                    File.Copy(legacyPath, newPath, true);
                    
                    // 记录迁移日志
                    await LogMigrationAsync(configTypeName, legacyPath, newPath);
                    
                    _logger?.LogInformation("配置文件迁移成功: {ConfigType}", configTypeName);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置文件迁移失败: {ConfigType}", configTypeName);
                return false;
            }
        }

        /// <summary>
        /// 记录配置迁移日志
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="legacyPath">旧路径</param>
        /// <param name="newPath">新路径</param>
        /// <returns>异步任务</returns>
        protected virtual async Task LogMigrationAsync(string configTypeName, string legacyPath, string newPath)
        {
            // 这里可以实现更详细的日志记录，例如写入到单独的迁移日志文件
            // 当前实现仅使用依赖注入的日志记录器
            _logger?.LogInformation("配置迁移完成: {ConfigType}", configTypeName);
            
            // 模拟异步操作
            await Task.CompletedTask;
        }

        /// <summary>
        /// 批量迁移所有配置
        /// </summary>
        /// <returns>成功迁移的配置数量</returns>
        public async Task<int> MigrateAllConfigsAsync()
        {
            if (!_enableMigration || !Directory.Exists(_legacyConfigDir))
            {
                return 0;
            }

            int migratedCount = 0;
            
            try
            {
                // 获取所有旧配置文件
                string[] legacyFiles = Directory.GetFiles(_legacyConfigDir, "*.json");
                
                foreach (string legacyFile in legacyFiles)
                {
                    string configTypeName = Path.GetFileNameWithoutExtension(legacyFile);
                    bool migrated = await MigrateConfigIfNeededAsync(configTypeName);
                    if (migrated)
                    {
                        migratedCount++;
                    }
                }
                
                _logger?.LogInformation("批量配置迁移完成，成功迁移 {Count} 个配置文件", migratedCount);
                return migratedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量配置迁移失败");
                return migratedCount;
            }
        }
    }
}