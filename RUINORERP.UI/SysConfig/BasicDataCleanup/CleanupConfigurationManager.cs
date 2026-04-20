using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理配置管理器
    /// 负责配置的持久化存储和加载
    /// </summary>
    public class CleanupConfigurationManager
    {
        private readonly string _configDirectory;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupConfigurationManager()
        {
            _configDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataCleanup", "Configurations");
            _serializer = new XmlSerializer(typeof(CleanupConfiguration));

            // 确保配置目录存在
            if (!Directory.Exists(_configDirectory))
            {
                Directory.CreateDirectory(_configDirectory);
            }
        }

        /// <summary>
        /// 获取配置文件的完整路径
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>配置文件路径</returns>
        private string GetConfigFilePath(string configName)
        {
            string safeFileName = GetSafeFileName(configName);
            return Path.Combine(_configDirectory, $"{safeFileName}.xml");
        }

        /// <summary>
        /// 获取安全的文件名
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>安全的文件名</returns>
        private string GetSafeFileName(string configName)
        {
            // 移除或替换文件名中的非法字符
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string safeName = configName;
            foreach (char c in invalidChars)
            {
                safeName = safeName.Replace(c, '_');
            }
            return safeName;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config">清理配置</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfiguration(CleanupConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // 验证配置
            string errorMessage;
            if (!config.Validate(out errorMessage))
            {
                throw new InvalidOperationException($"配置验证失败: {errorMessage}");
            }

            // 验证配置名称
            if (string.IsNullOrWhiteSpace(config.ConfigName))
            {
                throw new InvalidOperationException("配置名称不能为空");
            }

            // 验证配置名称长度
            if (config.ConfigName.Length > 100)
            {
                throw new InvalidOperationException("配置名称长度不能超过100个字符");
            }

            // 验证配置名称是否包含非法字符
            if (config.ConfigName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new InvalidOperationException("配置名称包含非法字符");
            }

            // 验证目标实体类型
            if (string.IsNullOrWhiteSpace(config.TargetEntityType))
            {
                throw new InvalidOperationException("目标实体类型不能为空");
            }

            // 验证规则配置
            if (config.CleanupRules != null)
            {
                foreach (var rule in config.CleanupRules)
                {
                    ValidateRule(rule, config.TargetEntityType);
                }
            }

            try
            {
                config.UpdateTime = DateTime.Now;
                string filePath = GetConfigFilePath(config.ConfigName);

                using (var writer = new StreamWriter(filePath))
                {
                    _serializer.Serialize(writer, config);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"保存配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证规则配置
        /// </summary>
        /// <param name="rule">清理规则</param>
        /// <param name="entityTypeName">实体类型名称</param>
        private void ValidateRule(CleanupRule rule, string entityTypeName)
        {
            if (rule == null)
            {
                throw new InvalidOperationException("规则对象不能为空");
            }

            if (string.IsNullOrWhiteSpace(rule.RuleName))
            {
                throw new InvalidOperationException("规则名称不能为空");
            }

            if (rule.RuleName.Length > 100)
            {
                throw new InvalidOperationException($"规则 '{rule.RuleName}' 的名称长度不能超过100个字符");
            }

            // 根据规则类型验证特定配置
            switch (rule.RuleType)
            {
                case CleanupRuleType.DuplicateRemoval:
                    if (rule.DuplicateCheckFields == null || rule.DuplicateCheckFields.Count == 0)
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是重复数据清理类型，必须指定重复检查字段");
                    }
                    break;

                case CleanupRuleType.EmptyValueRemoval:
                    if (rule.EmptyCheckFields == null || rule.EmptyCheckFields.Count == 0)
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是空值清理类型，必须指定空值检查字段");
                    }
                    break;

                case CleanupRuleType.ExpiredDataRemoval:
                    if (string.IsNullOrWhiteSpace(rule.DateFieldName))
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是过期数据清理类型，必须指定日期字段");
                    }
                    if (rule.ExpireDays <= 0)
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 的过期天数必须大于0");
                    }
                    break;

                case CleanupRuleType.InvalidReferenceRemoval:
                    if (string.IsNullOrWhiteSpace(rule.ForeignKeyField))
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是无效关联清理类型，必须指定外键字段");
                    }
                    if (string.IsNullOrWhiteSpace(rule.ReferenceTable))
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是无效关联清理类型，必须指定关联表");
                    }
                    break;

                case CleanupRuleType.CustomConditionRemoval:
                    if (rule.CustomConditions == null || rule.CustomConditions.Count == 0)
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是自定义条件清理类型，必须至少指定一个条件");
                    }
                    foreach (var condition in rule.CustomConditions)
                    {
                        if (string.IsNullOrWhiteSpace(condition.FieldName))
                        {
                            throw new InvalidOperationException($"规则 '{rule.RuleName}' 的条件字段名不能为空");
                        }
                    }
                    break;

                case CleanupRuleType.DataStandardization:
                    if (string.IsNullOrWhiteSpace(rule.StandardizationField))
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是数据标准化类型，必须指定标准化字段");
                    }
                    break;

                case CleanupRuleType.DataTruncation:
                    if (string.IsNullOrWhiteSpace(rule.TruncationField))
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 是数据截断类型，必须指定截断字段");
                    }
                    if (rule.MaxLength <= 0)
                    {
                        throw new InvalidOperationException($"规则 '{rule.RuleName}' 的最大长度必须大于0");
                    }
                    break;
            }

            // 验证操作类型配置
            if (rule.ActionType == CleanupActionType.UpdateField)
            {
                if (string.IsNullOrWhiteSpace(rule.UpdateFieldName))
                {
                    throw new InvalidOperationException($"规则 '{rule.RuleName}' 的操作类型是更新字段，必须指定更新字段名");
                }
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>清理配置，未找到返回null</returns>
        public CleanupConfiguration LoadConfiguration(string configName)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                return null;
            }

            try
            {
                string filePath = GetConfigFilePath(configName);

                if (!File.Exists(filePath))
                {
                    return null;
                }

                using (var reader = new StreamReader(filePath))
                {
                    return (CleanupConfiguration)_serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根据配置ID加载配置
        /// </summary>
        /// <param name="configId">配置ID</param>
        /// <returns>清理配置，未找到返回null</returns>
        public CleanupConfiguration LoadConfigurationById(string configId)
        {
            if (string.IsNullOrWhiteSpace(configId))
            {
                return null;
            }

            try
            {
                var allConfigs = GetAllConfigurations();
                foreach (var config in allConfigs)
                {
                    if (config.ConfigId == configId)
                    {
                        return config;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteConfiguration(string configName)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                return false;
            }

            try
            {
                string filePath = GetConfigFilePath(configName);

                if (!File.Exists(filePath))
                {
                    return false;
                }

                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"删除配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取所有配置名称列表
        /// </summary>
        /// <returns>配置名称列表</returns>
        public List<string> GetAllConfigurationNames()
        {
            var names = new List<string>();

            try
            {
                if (!Directory.Exists(_configDirectory))
                {
                    return names;
                }

                var files = Directory.GetFiles(_configDirectory, "*.xml");
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    names.Add(fileName);
                }

                names.Sort();
                return names;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"获取配置列表失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <returns>配置列表</returns>
        public List<CleanupConfiguration> GetAllConfigurations()
        {
            var configs = new List<CleanupConfiguration>();

            try
            {
                var names = GetAllConfigurationNames();
                foreach (var name in names)
                {
                    var config = LoadConfiguration(name);
                    if (config != null)
                    {
                        configs.Add(config);
                    }
                }

                return configs;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"获取配置列表失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取指定实体类型的配置列表
        /// </summary>
        /// <param name="entityTypeName">实体类型名称</param>
        /// <returns>配置列表</returns>
        public List<CleanupConfiguration> GetConfigurationsByEntityType(string entityTypeName)
        {
            var configs = new List<CleanupConfiguration>();

            try
            {
                var allConfigs = GetAllConfigurations();
                foreach (var config in allConfigs)
                {
                    if (string.Equals(config.TargetEntityType, entityTypeName, StringComparison.OrdinalIgnoreCase))
                    {
                        configs.Add(config);
                    }
                }

                return configs;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"获取配置列表失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查配置名称是否已存在
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>是否存在</returns>
        public bool ConfigurationExists(string configName)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                return false;
            }

            string filePath = GetConfigFilePath(configName);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 重命名配置
        /// </summary>
        /// <param name="oldName">旧名称</param>
        /// <param name="newName">新名称</param>
        /// <returns>是否重命名成功</returns>
        public bool RenameConfiguration(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                return false;
            }

            if (string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (ConfigurationExists(newName))
            {
                throw new InvalidOperationException($"配置名称 '{newName}' 已存在");
            }

            try
            {
                var config = LoadConfiguration(oldName);
                if (config == null)
                {
                    return false;
                }

                config.ConfigName = newName;
                config.UpdateTime = DateTime.Now;

                // 保存新配置
                SaveConfiguration(config);

                // 删除旧配置
                DeleteConfiguration(oldName);

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"重命名配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 复制配置
        /// </summary>
        /// <param name="sourceName">源配置名称</param>
        /// <param name="newName">新配置名称</param>
        /// <returns>是否复制成功</returns>
        public bool CopyConfiguration(string sourceName, string newName)
        {
            if (string.IsNullOrWhiteSpace(sourceName) || string.IsNullOrWhiteSpace(newName))
            {
                return false;
            }

            if (ConfigurationExists(newName))
            {
                throw new InvalidOperationException($"配置名称 '{newName}' 已存在");
            }

            try
            {
                var sourceConfig = LoadConfiguration(sourceName);
                if (sourceConfig == null)
                {
                    return false;
                }

                var newConfig = sourceConfig.Clone(newName);
                newConfig.Description = $"从 '{sourceName}' 复制";

                return SaveConfiguration(newConfig);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"复制配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 导出配置到文件
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="exportPath">导出路径</param>
        /// <returns>是否导出成功</returns>
        public bool ExportConfiguration(string configName, string exportPath)
        {
            if (string.IsNullOrWhiteSpace(configName) || string.IsNullOrWhiteSpace(exportPath))
            {
                return false;
            }

            try
            {
                var config = LoadConfiguration(configName);
                if (config == null)
                {
                    return false;
                }

                using (var writer = new StreamWriter(exportPath))
                {
                    _serializer.Serialize(writer, config);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导出配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从文件导入配置
        /// </summary>
        /// <param name="importPath">导入路径</param>
        /// <param name="newName">新配置名称（可选，为空则使用原名称）</param>
        /// <returns>导入的配置</returns>
        public CleanupConfiguration ImportConfiguration(string importPath, string newName = null)
        {
            if (string.IsNullOrWhiteSpace(importPath) || !File.Exists(importPath))
            {
                return null;
            }

            try
            {
                CleanupConfiguration config;
                using (var reader = new StreamReader(importPath))
                {
                    config = (CleanupConfiguration)_serializer.Deserialize(reader);
                }

                if (config == null)
                {
                    return null;
                }

                // 生成新的配置ID
                config.ConfigId = Guid.NewGuid().ToString("N");
                config.CreateTime = DateTime.Now;
                config.UpdateTime = DateTime.Now;

                // 如果指定了新名称，使用新名称
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    config.ConfigName = newName;
                }

                // 检查名称是否已存在
                if (ConfigurationExists(config.ConfigName))
                {
                    config.ConfigName = $"{config.ConfigName}_{DateTime.Now:yyyyMMddHHmmss}";
                }

                SaveConfiguration(config);
                return config;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导入配置失败: {ex.Message}", ex);
            }
        }
    }
}
