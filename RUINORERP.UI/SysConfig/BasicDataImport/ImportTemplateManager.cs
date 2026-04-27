using RUINORERP.Common;
using RUINORERP.Global;
using RUINORERP.Model.ImportEngine.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 导入模板管理器
    /// 管理导入配置的保存、加载和复用
    /// 借鉴 DevTools 的模板应用思路
    /// </summary>
    public class ImportTemplateManager
    {
        private readonly string _templateDirectory;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportTemplateManager()
        {
            // 设置配置文件保存路径
            _templateDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ImportTemplates");

            if (!Directory.Exists(_templateDirectory))
            {
                Directory.CreateDirectory(_templateDirectory);
            }

            _serializer = new XmlSerializer(typeof(ImportTemplate));
        }

        /// <summary>
        /// 保存导入模板
        /// </summary>
        /// <param name="template">导入模板</param>
        /// <exception cref="ArgumentNullException">当模板为空时抛出</exception>
        /// <exception cref="ArgumentException">当模板名称为空时抛出</exception>
        public void SaveTemplate(ImportTemplate template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            if (string.IsNullOrWhiteSpace(template.TemplateName))
                throw new ArgumentException("模板名称不能为空", nameof(template.TemplateName));

            // 清理模板名称（去除非法字符）
            string safeName = GetSafeFileName(template.TemplateName);
            string filePath = Path.Combine(_templateDirectory, $"{safeName}.xml");

            // 更新模板元数据
            template.UpdateTime = DateTime.Now;
            if (template.CreateTime == default)
                template.CreateTime = DateTime.Now;

            using (var writer = new StreamWriter(filePath))
            {
                _serializer.Serialize(writer, template);
            }
        }

        /// <summary>
        /// 加载导入模板
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns>导入模板，不存在则返回 null</returns>
        public ImportTemplate LoadTemplate(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return null;

            string safeName = GetSafeFileName(templateName);
            string filePath = Path.Combine(_templateDirectory, $"{safeName}.xml");

            if (!File.Exists(filePath))
                return null;

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return (ImportTemplate)_serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载模板失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteTemplate(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return false;

            string safeName = GetSafeFileName(templateName);
            string filePath = Path.Combine(_templateDirectory, $"{safeName}.xml");

            if (!File.Exists(filePath))
                return false;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"删除模板失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取所有模板名称
        /// </summary>
        /// <returns>模板名称列表</returns>
        public List<string> GetAllTemplateNames()
        {
            try
            {
                return Directory.GetFiles(_templateDirectory, "*.xml")
                    .Select(Path.GetFileNameWithoutExtension)
                    .OrderBy(name => name)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取模板列表失败: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// 根据实体类型获取适用模板
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>模板列表</returns>
        public List<ImportTemplate> GetTemplatesForEntity(Type entityType)
        {
            if (entityType == null)
                return new List<ImportTemplate>();

            var templates = new List<ImportTemplate>();
            var allNames = GetAllTemplateNames();

            foreach (var name in allNames)
            {
                var template = LoadTemplate(name);
                if (template?.EntityType == entityType.FullName)
                {
                    templates.Add(template);
                }
            }

            return templates.OrderByDescending(t => t.UpdateTime).ToList();
        }

        /// <summary>
        /// 检查模板是否存在
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns>是否存在</returns>
        public bool TemplateExists(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return false;

            string safeName = GetSafeFileName(templateName);
            string filePath = Path.Combine(_templateDirectory, $"{safeName}.xml");
            return File.Exists(filePath);
        }

        /// <summary>
        /// 从导入配置创建模板
        /// </summary>
        /// <param name="config">导入配置</param>
        /// <param name="templateName">模板名称</param>
        /// <param name="description">模板描述</param>
        /// <returns>导入模板</returns>
        public ImportTemplate CreateTemplateFromConfig(
            ImportConfiguration config,
            string templateName,
            string description = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var template = new ImportTemplate
            {
                TemplateName = templateName,
                EntityType = config.EntityType,
                Description = description ?? $"{config.MappingName} 导入模板",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                ColumnMappings = config.ColumnMappings?.Select(m => new TemplateColumnMapping
                {
                    ExcelColumn = m.ExcelColumn,
                    DbColumn = m.SystemField?.Key,
                    DbColumnDisplay = m.SystemField?.Value,
                    IsKey = m.IsUniqueValue,
                    DataSourceType = m.DataSourceType,
                    DefaultValue = m.DefaultValue,
                    IsRequired = m.IsRequired
                }).ToList() ?? new List<TemplateColumnMapping>(),
                LogicalKeyField = config.ColumnMappings?.FirstOrDefault(m => m.IsUniqueValue)?.SystemField?.Key,
                EnableDeduplication = config.EnableDeduplication,
                DeduplicateFields = config.DeduplicateFields?.ToList() ?? new List<string>(),
                DeduplicateStrategy = config.DeduplicateStrategy
            };

            return template;
        }

        /// <summary>
        /// 应用模板到导入配置
        /// </summary>
        /// <param name="config">导入配置</param>
        /// <param name="template">导入模板</param>
        public void ApplyTemplate(ImportConfiguration config, ImportTemplate template)
        {
            if (config == null || template?.ColumnMappings == null)
                return;

            // 应用列映射
            foreach (var templateMapping in template.ColumnMappings)
            {
                var existingMapping = config.ColumnMappings?.FirstOrDefault(
                    m => m.SystemField?.Key == templateMapping.DbColumn);

                if (existingMapping != null)
                {
                    // 更新现有映射
                    existingMapping.ExcelColumn = templateMapping.ExcelColumn;
                    existingMapping.IsUniqueValue = templateMapping.IsKey;
                    existingMapping.DataSourceType = templateMapping.DataSourceType;
                    existingMapping.DefaultValue = templateMapping.DefaultValue;
                    existingMapping.IsRequired = templateMapping.IsRequired;
                }
                else if (!string.IsNullOrEmpty(templateMapping.DbColumn))
                {
                    // 创建新映射
                    var newMapping = new ColumnMapping
                    {
                        ExcelColumn = templateMapping.ExcelColumn,
                        SystemField = new SerializableKeyValuePair<string>(
                            templateMapping.DbColumn,
                            templateMapping.DbColumnDisplay ?? templateMapping.DbColumn),
                        IsUniqueValue = templateMapping.IsKey,
                        DataSourceType = templateMapping.DataSourceType,
                        DefaultValue = templateMapping.DefaultValue,
                        IsRequired = templateMapping.IsRequired
                    };

                    if (config.ColumnMappings == null)
                        config.ColumnMappings = new List<ColumnMapping>();

                    config.ColumnMappings.Add(newMapping);
                }
            }

            // 应用去重配置
            config.EnableDeduplication = template.EnableDeduplication;
            config.DeduplicateFields = template.DeduplicateFields?.ToList() ?? new List<string>();
            config.DeduplicateStrategy = template.DeduplicateStrategy;
        }

        /// <summary>
        /// 获取安全的文件名
        /// </summary>
        private string GetSafeFileName(string name)
        {
            // 移除或替换文件名中的非法字符
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                name = name.Replace(c, '_');
            }
            return name.Trim();
        }
    }

    /// <summary>
    /// 导入模板
    /// </summary>
    [Serializable]
    public class ImportTemplate
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 实体类型完整名称
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 模板描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 列映射列表
        /// </summary>
        public List<TemplateColumnMapping> ColumnMappings { get; set; }

        /// <summary>
        /// 逻辑主键字段
        /// </summary>
        public string LogicalKeyField { get; set; }

        /// <summary>
        /// 是否启用去重
        /// </summary>
        public bool EnableDeduplication { get; set; }

        /// <summary>
        /// 去重字段列表
        /// </summary>
        public List<string> DeduplicateFields { get; set; }

        /// <summary>
        /// 去重策略
        /// </summary>
        public DeduplicateStrategy DeduplicateStrategy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public ImportTemplate()
        {
            ColumnMappings = new List<TemplateColumnMapping>();
            DeduplicateFields = new List<string>();
            DeduplicateStrategy = DeduplicateStrategy.FirstOccurrence;
        }
    }

    /// <summary>
    /// 模板列映射
    /// </summary>
    [Serializable]
    public class TemplateColumnMapping
    {
        /// <summary>
        /// Excel 列名
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string DbColumn { get; set; }

        /// <summary>
        /// 数据库字段显示名
        /// </summary>
        public string DbColumnDisplay { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public DataSourceType DataSourceType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }
    }
}
