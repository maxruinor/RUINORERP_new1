/*************************************************************
 * 文件名：ConfigConsistencyValidator.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置一致性验证工具，支持配置文件一致性验证和回滚机制
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Validation
{
    /// <summary>
    /// 配置一致性验证结果
    /// </summary>
    public class ConfigConsistencyResult
    {
        /// <summary>
        /// 是否一致
        /// </summary>
        public bool IsConsistent { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath { get; set; }

        /// <summary>
        /// 验证错误信息列表
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// 不一致的属性列表
        /// </summary>
        public List<string> InconsistentProperties { get; set; } = new List<string>();

        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime ValidationTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取验证结果的字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"配置类型: {ConfigType}, 是否一致: {IsConsistent}, 错误: {string.Join(", ", Errors)}";
        }
    }

    /// <summary>
    /// 配置回滚结果
    /// </summary>
    public class ConfigRollbackResult
    {
        /// <summary>
        /// 是否成功回滚
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 回滚前的配置路径
        /// </summary>
        public string PreviousConfigPath { get; set; }

        /// <summary>
        /// 回滚使用的备份路径
        /// </summary>
        public string RollbackSourcePath { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 回滚时间
        /// </summary>
        public DateTime RollbackTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 配置一致性验证器
    /// 负责验证配置文件的一致性并支持配置回滚
    /// </summary>
    public class ConfigConsistencyValidator
    {
        private readonly IConfigPathResolver _pathResolver;
        private readonly ILogger<ConfigConsistencyValidator> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private const int MaxBackupCount = 10; // 最大备份文件数量

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathResolver">配置路径解析器</param>
        /// <param name="logger">日志记录器</param>
        public ConfigConsistencyValidator(IConfigPathResolver pathResolver, ILogger<ConfigConsistencyValidator> logger = null)
        {
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        /// <summary>
        /// 验证单个配置文件的一致性
        /// </summary>
        /// <param name="configType">配置类型名称</param>
        /// <param name="expectedConfig">预期的配置对象</param>
        /// <returns>验证结果</returns>
        public async Task<ConfigConsistencyResult> ValidateConsistencyAsync(string configType, object expectedConfig = null)
        {
            var result = new ConfigConsistencyResult
            {
                ConfigType = configType,
                ConfigFilePath = _pathResolver.GetConfigFilePath(configType)
            };

            try
            {
                // 检查文件是否存在
                if (!File.Exists(result.ConfigFilePath))
                {
                    result.Errors.Add($"配置文件不存在: {result.ConfigFilePath}");
                    result.IsConsistent = false;
                    return result;
                }

                // 检查文件完整性
                if (!await ValidateFileIntegrityAsync(result.ConfigFilePath, result.Errors))
                {
                    result.IsConsistent = false;
                    return result;
                }

                // 如果提供了预期配置，则验证内容一致性
                if (expectedConfig != null)
                {
                    await ValidateContentConsistencyAsync(result, expectedConfig);
                }
                else
                {
                    result.IsConsistent = true; // 仅检查文件存在性和完整性
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证配置一致性时发生异常: {ConfigType}", configType);
                result.Errors.Add($"验证异常: {ex.Message}");
                result.IsConsistent = false;
            }

            if (result.IsConsistent)
            {
                _logger?.LogInformation("配置验证通过: {ConfigType}", configType);
            }
            else
            {
                _logger?.LogWarning("配置验证失败: {ConfigType}, 错误: {Errors}", 
                    configType, string.Join(", ", result.Errors));
            }

            return result;
        }

        /// <summary>
        /// 验证所有已注册配置文件的一致性
        /// </summary>
        /// <param name="registry">配置注册表</param>
        /// <returns>验证结果字典</returns>
        public async Task<Dictionary<string, ConfigConsistencyResult>> ValidateAllConsistencyAsync(IConfigurationRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var results = new Dictionary<string, ConfigConsistencyResult>();
            var configTypes = registry.GetRegisteredConfigTypes();

            foreach (var configType in configTypes)
            {
                results[configType] = await ValidateConsistencyAsync(configType);
            }

            return results;
        }

        /// <summary>
        /// 验证文件完整性
        /// </summary>
        private async Task<bool> ValidateFileIntegrityAsync(string filePath, List<string> errors)
        {
            try
            {
                // 检查文件大小
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length == 0)
                {
                    errors.Add("配置文件为空");
                    return false;
                }

                // 检查JSON格式是否有效
                string jsonContent = await File.ReadAllTextAsync(filePath);
                try
                {
                    JsonDocument.Parse(jsonContent);
                }
                catch (JsonException ex)
                {
                    errors.Add($"JSON格式错误: {ex.Message}");
                    return false;
                }

                // 检查文件权限
                if (!HasWritePermission(filePath))
                {
                    errors.Add("配置文件无写权限");
                }

                return true;
            }
            catch (Exception ex)
            {
                errors.Add($"文件完整性验证异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 验证内容一致性
        /// </summary>
        private async Task ValidateContentConsistencyAsync(ConfigConsistencyResult result, object expectedConfig)
        {
            try
            {
                string jsonContent = await File.ReadAllTextAsync(result.ConfigFilePath);
                var actualConfig = JsonDocument.Parse(jsonContent);
                var expectedJson = JsonSerializer.Serialize(expectedConfig, _jsonOptions);
                var expectedDoc = JsonDocument.Parse(expectedJson);

                // 比较JSON属性
                CompareJsonDocuments(actualConfig.RootElement, expectedDoc.RootElement, "", result.InconsistentProperties);

                if (result.InconsistentProperties.Any())
                {
                    result.IsConsistent = false;
                    result.Errors.Add($"发现 {result.InconsistentProperties.Count} 个不一致的属性");
                }
                else
                {
                    result.IsConsistent = true;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"内容一致性验证异常: {ex.Message}");
                result.IsConsistent = false;
            }
        }

        /// <summary>
        /// 比较JSON文档
        /// </summary>
        private void CompareJsonDocuments(JsonElement actual, JsonElement expected, string path, List<string> inconsistentProps)
        {
            if (actual.ValueKind != expected.ValueKind)
            {
                inconsistentProps.Add($"{path}: 类型不匹配 (实际: {actual.ValueKind}, 预期: {expected.ValueKind})");
                return;
            }

            switch (expected.ValueKind)
            {
                case JsonValueKind.Object:
                    // 检查预期对象的所有属性
                    foreach (var prop in expected.EnumerateObject())
                    {
                        string propPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";
                        
                        if (!actual.TryGetProperty(prop.Name, out JsonElement actualProp))
                        {
                            inconsistentProps.Add($"{propPath}: 属性缺失");
                        }
                        else
                        {
                            CompareJsonDocuments(actualProp, prop.Value, propPath, inconsistentProps);
                        }
                    }
                    break;

                case JsonValueKind.Array:
                    // 简单数组长度比较
                    if (actual.GetArrayLength() != expected.GetArrayLength())
                    {
                        inconsistentProps.Add($"{path}: 数组长度不匹配 (实际: {actual.GetArrayLength()}, 预期: {expected.GetArrayLength()})");
                    }
                    break;

                case JsonValueKind.String:
                    if (actual.GetString() != expected.GetString())
                    {
                        inconsistentProps.Add($"{path}: 值不匹配");
                    }
                    break;

                case JsonValueKind.Number:
                    // 简化的数值比较
                    if (actual.GetDouble() != expected.GetDouble())
                    {
                        inconsistentProps.Add($"{path}: 数值不匹配");
                    }
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    if (actual.GetBoolean() != expected.GetBoolean())
                    {
                        inconsistentProps.Add($"{path}: 布尔值不匹配");
                    }
                    break;
            }
        }

        /// <summary>
        /// 检查是否有写权限
        /// </summary>
        private bool HasWritePermission(string filePath)
        {
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 备份配置文件
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>备份文件路径</returns>
        public async Task<string> BackupConfigAsync(string configType)
        {
            string configPath = _pathResolver.GetConfigFilePath(configType);
            
            if (!File.Exists(configPath))
            {
                _logger?.LogWarning("无法备份不存在的配置文件: {ConfigType}", configType);
                return null;
            }

            try
            {
                string backupDir = Path.Combine(Path.GetDirectoryName(configPath), "Backups");
                Directory.CreateDirectory(backupDir);

                // 创建带时间戳的备份文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string backupFileName = $"{configType}_{timestamp}.json";
                string backupPath = Path.Combine(backupDir, backupFileName);

                // 复制文件
                await Task.Run(() => File.Copy(configPath, backupPath, true));
                
                // 清理旧备份
                await CleanupOldBackupsAsync(backupDir, configType);
                
                _logger?.LogInformation("配置已备份: {ConfigType} -> {BackupPath}", configType, backupPath);
                return backupPath;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "备份配置文件失败: {ConfigType}", configType);
                throw;
            }
        }

        /// <summary>
        /// 清理旧备份文件
        /// </summary>
        private async Task CleanupOldBackupsAsync(string backupDir, string configType)
        {
            await Task.Run(() =>
            {
                try
                {
                    // 获取该配置类型的所有备份文件
                    var backupFiles = Directory.GetFiles(backupDir, $"{configType}_*.json")
                        .OrderByDescending(f => new FileInfo(f).CreationTime)
                        .ToList();

                    // 删除超过最大数量的旧备份
                    for (int i = MaxBackupCount; i < backupFiles.Count; i++)
                    {
                        try
                        {
                            File.Delete(backupFiles[i]);
                            _logger?.LogDebug("已删除旧备份: {BackupFile}", backupFiles[i]);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "删除旧备份文件失败: {BackupFile}", backupFiles[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "清理备份文件时发生异常");
                }
            });
        }

        /// <summary>
        /// 回滚配置到最近的备份
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>回滚结果</returns>
        public async Task<ConfigRollbackResult> RollbackToLatestBackupAsync(string configType)
        {
            var result = new ConfigRollbackResult
            {
                ConfigType = configType,
                PreviousConfigPath = _pathResolver.GetConfigFilePath(configType)
            };

            try
            {
                // 查找最新的备份文件
                string backupDir = Path.Combine(Path.GetDirectoryName(result.PreviousConfigPath), "Backups");
                var latestBackup = Directory.GetFiles(backupDir, $"{configType}_*.json")
                    .OrderByDescending(f => new FileInfo(f).CreationTime)
                    .FirstOrDefault();

                if (latestBackup == null)
                {
                    result.ErrorMessage = "未找到备份文件";
                    result.IsSuccessful = false;
                    _logger?.LogWarning("回滚失败: 未找到{ConfigType}的备份文件", configType);
                    return result;
                }

                // 先备份当前配置（以便再次回滚）
                await BackupConfigAsync(configType);

                // 执行回滚
                result.RollbackSourcePath = latestBackup;
                await Task.Run(() => File.Copy(latestBackup, result.PreviousConfigPath, true));
                
                result.IsSuccessful = true;
                _logger?.LogInformation("配置回滚成功: {ConfigType} 从 {BackupPath} 回滚", configType, latestBackup);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置回滚失败: {ConfigType}", configType);
                result.ErrorMessage = ex.Message;
                result.IsSuccessful = false;
            }

            return result;
        }

        /// <summary>
        /// 回滚配置到指定的备份文件
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="backupFilePath">备份文件路径</param>
        /// <returns>回滚结果</returns>
        public async Task<ConfigRollbackResult> RollbackToSpecificBackupAsync(string configType, string backupFilePath)
        {
            if (!File.Exists(backupFilePath))
            {
                throw new FileNotFoundException("指定的备份文件不存在", backupFilePath);
            }

            var result = new ConfigRollbackResult
            {
                ConfigType = configType,
                PreviousConfigPath = _pathResolver.GetConfigFilePath(configType),
                RollbackSourcePath = backupFilePath
            };

            try
            {   
                // 先备份当前配置
                await BackupConfigAsync(configType);

                // 执行回滚
                await Task.Run(() => File.Copy(backupFilePath, result.PreviousConfigPath, true));
                
                result.IsSuccessful = true;
                _logger?.LogInformation("配置回滚成功: {ConfigType} 从指定备份回滚", configType);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置回滚失败: {ConfigType} 从 {BackupPath}", configType, backupFilePath);
                result.ErrorMessage = ex.Message;
                result.IsSuccessful = false;
            }

            return result;
        }

        /// <summary>
        /// 获取所有备份文件
        /// </summary>
        /// <param name="configType">配置类型（可选）</param>
        /// <returns>备份文件列表</returns>
        public List<string> GetBackupFiles(string configType = null)
        {
            try
            {
                string baseDir = _pathResolver.GetConfigDirectory(ConfigPathType.Server);
                string backupDir = Path.Combine(baseDir, "Backups");

                if (!Directory.Exists(backupDir))
                    return new List<string>();

                string searchPattern = string.IsNullOrEmpty(configType) ? "*.json" : $"{configType}_*.json";
                
                return Directory.GetFiles(backupDir, searchPattern)
                    .OrderByDescending(f => new FileInfo(f).CreationTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取备份文件列表失败");
                return new List<string>();
            }
        }

        /// <summary>
        /// 验证配置并在不一致时自动回滚
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="expectedConfig">预期配置</param>
        /// <returns>验证和回滚结果</returns>
        public async Task<Tuple<ConfigConsistencyResult, ConfigRollbackResult>> ValidateAndAutoRollbackAsync(string configType, object expectedConfig)
        {
            var validationResult = await ValidateConsistencyAsync(configType, expectedConfig);
            ConfigRollbackResult rollbackResult = null;

            if (!validationResult.IsConsistent)
            {
                _logger?.LogWarning("配置不一致，执行自动回滚: {ConfigType}", configType);
                rollbackResult = await RollbackToLatestBackupAsync(configType);
            }

            return Tuple.Create(validationResult, rollbackResult);
        }
    }

    /// <summary>
    /// 配置回滚服务
    /// 提供配置回滚的高级服务
    /// </summary>
    public class ConfigRollbackService
    {
        private readonly ConfigConsistencyValidator _validator;
        private readonly ILogger<ConfigRollbackService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="validator">配置一致性验证器</param>
        /// <param name="logger">日志记录器</param>
        public ConfigRollbackService(ConfigConsistencyValidator validator, ILogger<ConfigRollbackService> logger = null)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger;
        }

        /// <summary>
        /// 批量回滚多个配置
        /// </summary>
        /// <param name="configTypes">配置类型列表</param>
        /// <returns>回滚结果字典</returns>
        public async Task<Dictionary<string, ConfigRollbackResult>> RollbackMultipleConfigsAsync(IEnumerable<string> configTypes)
        {
            var results = new Dictionary<string, ConfigRollbackResult>();

            foreach (var configType in configTypes)
            {
                try
                {
                    results[configType] = await _validator.RollbackToLatestBackupAsync(configType);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "批量回滚配置失败: {ConfigType}", configType);
                    results[configType] = new ConfigRollbackResult
                    {
                        ConfigType = configType,
                        IsSuccessful = false,
                        ErrorMessage = ex.Message
                    };
                }
            }

            return results;
        }

        /// <summary>
        /// 创建配置变更事务
        /// 确保配置变更的原子性，支持回滚
        /// </summary>
        /// <returns>配置事务对象</returns>
        public ConfigTransaction CreateTransaction()
        {
            return new ConfigTransaction(_validator, _logger);
        }
    }

    /// <summary>
    /// 配置事务
    /// 支持配置变更的原子性操作
    /// </summary>
    public class ConfigTransaction : IDisposable
    {
        private readonly ConfigConsistencyValidator _validator;
        private readonly ILogger _logger;
        private readonly List<string> _changedConfigs = new List<string>();
        private readonly Dictionary<string, string> _backupPaths = new Dictionary<string, string>();
        private bool _committed = false;
        private bool _rolledBack = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        internal ConfigTransaction(ConfigConsistencyValidator validator, ILogger logger)
        {
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// 准备配置变更
        /// 在变更前备份配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        public async Task PrepareChangeAsync(string configType)
        {
            if (_committed || _rolledBack)
                throw new InvalidOperationException("事务已结束，无法准备新的变更");

            if (!_changedConfigs.Contains(configType))
            {
                string backupPath = await _validator.BackupConfigAsync(configType);
                _backupPaths[configType] = backupPath;
                _changedConfigs.Add(configType);
                _logger?.LogInformation("事务准备变更配置: {ConfigType}", configType);
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (_rolledBack)
                throw new InvalidOperationException("事务已回滚，无法提交");

            _committed = true;
            _logger?.LogInformation("事务已提交，变更配置: {ConfigTypes}", string.Join(", ", _changedConfigs));
        }

        /// <summary>
        /// 回滚事务
        /// 恢复所有变更的配置到事务前的状态
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_committed)
                throw new InvalidOperationException("事务已提交，无法回滚");

            _rolledBack = true;
            _logger?.LogWarning("事务正在回滚，恢复配置: {ConfigTypes}", string.Join(", ", _changedConfigs));

            foreach (var configType in _changedConfigs)
            {
                if (_backupPaths.TryGetValue(configType, out string backupPath) && backupPath != null)
                {
                    try
                    {
                        await _validator.RollbackToSpecificBackupAsync(configType, backupPath);
                        _logger?.LogInformation("事务回滚成功: {ConfigType}", configType);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "事务回滚失败: {ConfigType}", configType);
                    }
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// 如果事务未提交，自动回滚
        /// </summary>
        public void Dispose()
        {
            if (!_committed && !_rolledBack)
            {
                _logger?.LogWarning("事务未显式提交或回滚，自动执行回滚");
                Task.Run(async () => await RollbackAsync()).Wait();
            }
        }
    }
}