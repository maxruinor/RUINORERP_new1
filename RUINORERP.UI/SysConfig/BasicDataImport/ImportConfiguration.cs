using RUINORERP.Common;
using RUINORERP.Global;
using RUINORERP.Model.ImportEngine.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据库存在性处理策略枚举
    /// 用于判断整行数据已存在于数据库中时的处理方式
    /// </summary>
    public enum ExistenceStrategyType
    {
        /// <summary>
        /// 跳过（默认）
        /// </summary>
        [Description("跳过")]
        Skip = 0,

        /// <summary>
        /// 更新
        /// </summary>
        [Description("更新")]
        Update = 1,

        /// <summary>
        /// 报错
        /// </summary>
        [Description("报错")]
        Error = 2
    }

    /// <summary>
    /// 数据导入全局配置类
    /// </summary>
    /// <remarks>
    /// 该类包含数据导入过程中的全局配置项，管理配置、解析、保存各环节的完整配置。
    /// 在全局配置下层级化管理映射关系集合，确保配置结构清晰有序。
    /// </remarks>
    [Serializable]
    [XmlRoot("ImportConfiguration")]
    public class ImportConfiguration
    {
        /// <summary>
        /// 映射配置名称
        /// </summary>
        [XmlElement("MappingName")]
        public string MappingName { get; set; }

        /// <summary>
        /// 目标实体类型名称（用于保存和加载配置）
        /// </summary>
        [XmlElement("EntityType")]
        public string EntityType { get; set; }

        /// <summary>
        /// 导入类型标识（用于区分客户和供应商等使用相同表的情况）
        /// </summary>
        [XmlElement("ImportType")]
        public string ImportType { get; set; }

        /// <summary>
        /// 目标表引用
        /// </summary>
        [XmlElement("TargetTable")]
        public SerializableKeyValuePair<string> TargetTable { get; set; }

        /// <summary>
        /// 存在性处理策略（当整行数据已存在于数据库时的处理方式）
        /// </summary>
        /// <remarks>
        /// 根据配置的业务键字段（IsBusinessKey）组合来判断整行数据是否存在
        /// Skip: 跳过已存在的行（默认）
        /// Update: 更新已存在的行
        /// Error: 报错提示
        /// </remarks>
        [XmlElement("ExistenceStrategy")]
        public int ExistenceStrategy { get; set; } = (int)ExistenceStrategyType.Skip;

        /// <summary>
        /// 是否启用全局去重
        /// </summary>
        /// <remarks>
        /// 为true时，将对解析后的数据行执行去重处理，去重依据由 DeduplicateFields 指定
        /// </remarks>
        [XmlElement("EnableDeduplication")]
        public bool EnableDeduplication { get; set; }

        /// <summary>
        /// 去重字段列表（系统字段名）
        /// </summary>
        /// <remarks>
        /// 支持多字段组合去重。例如：["ProdCode", "Color"] 表示按"产品编号+颜色"组合去重
        /// 如果为空列表，则使用配置了IsUniqueValue的字段作为去重依据
        /// </remarks>
        [XmlArray("DeduplicateFields")]
        [XmlArrayItem("Field")]
        public List<string> DeduplicateFields { get; set; }

        /// <summary>
        /// 去重策略
        /// </summary>
        /// <remarks>
        /// FirstOccurrence: 保留第一条记录（默认）
        /// LastOccurrence: 保留最后一条记录
        /// </remarks>
        [XmlElement("DeduplicateStrategy")]
        public DeduplicateStrategy DeduplicateStrategy { get; set; } = DeduplicateStrategy.FirstOccurrence;

        /// <summary>
        /// 是否在去重时忽略空值
        /// </summary>
        /// <remarks>
        /// 为true时，如果去重字段的值为空，则跳过该行的去重判断
        /// </remarks>
        [XmlElement("IgnoreEmptyValuesInDeduplication")]
        public bool IgnoreEmptyValuesInDeduplication { get; set; } = true;

        /// <summary>
        /// 列映射集合
        /// </summary>
        [XmlArray("ColumnMappings")]
        [XmlArrayItem("ColumnMapping")]
        public List<ColumnMapping> ColumnMappings { get; set; }

        /// <summary>
        /// 配置创建时间
        /// </summary>
        [XmlElement("CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 配置更新时间
        /// </summary>
        [XmlElement("UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 配置描述
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 配置版本（用于兼容性检查）
        /// </summary>
        [XmlElement("Version")]
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 依赖的其他表名列表（用于关联表导入）
        /// </summary>
        /// <remarks>
        /// 表示当前表的数据依赖哪些其他表的数据。导入时会先导入依赖表，再导入当前表。
        /// 例如：产品明细表依赖产品表，则此处填入产品表名。
        /// </remarks>
        [XmlArray("DependentTables")]
        [XmlArrayItem("TableName")]
        public List<string> DependentTables { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportConfiguration()
        {
            MappingName = string.Empty;
            EntityType = string.Empty;
            EnableDeduplication = false;
            DeduplicateFields = new List<string>();
            ColumnMappings = new List<ColumnMapping>();
            DependentTables = new List<string>();
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 获取有效的去重字段列表
        /// </summary>
        /// <returns>去重字段列表</returns>
        /// <remarks>
        /// 如果DeduplicateFields为空，则返回配置了IsUniqueValue的字段列表。
        /// 如果有外键关联映射，会自动添加外键来源列到去重字段中。
        /// </remarks>
        public List<string> GetEffectiveDeduplicateFields()
        {
            var resultFields = new List<string>();

            // 1. 获取配置的去重字段
            if (DeduplicateFields != null && DeduplicateFields.Count > 0)
            {
                resultFields.AddRange(DeduplicateFields);
            }
            else
            {
                // 如果未指定去重字段，则使用配置了唯一性的字段
                if (ColumnMappings != null)
                {
                    foreach (var mapping in ColumnMappings)
                    {
                        if (mapping.IsUniqueValue && !string.IsNullOrEmpty(mapping.SystemField?.Key))
                        {
                            resultFields.Add(mapping.SystemField.Key);
                        }
                    }
                }
            }

            // 2. 自动添加外键来源列到去重字段中
            // 这样可以确保去重时考虑外键参考值，避免因外键值不同而被误判为重复
            if (ColumnMappings != null)
            {
                var foreignSourceColumns = new HashSet<string>();
                foreach (var mapping in ColumnMappings)
                {
                    // 检查是否为外键关联类型
                    if (mapping.ColumnDataSourceType == (int)DataSourceType.ForeignKey)
                    {
                        var foreignConfig = mapping.DataSourceConfig as ForeignKeyConfig;
                        if (foreignConfig != null &&
                            foreignConfig.ForeignKeySourceColumn != null &&
                            !string.IsNullOrEmpty(foreignConfig.ForeignKeySourceColumn.Key))
                        {
                            // 添加外键来源列的Excel列名
                            string sourceColumnName = foreignConfig.ForeignKeySourceColumn.Key;
                            if (!foreignSourceColumns.Contains(sourceColumnName))
                            {
                                resultFields.Add(sourceColumnName);
                                foreignSourceColumns.Add(sourceColumnName);
                            }
                        }
                    }
                }
            }

            return resultFields;
        }

        /// <summary>
        /// 获取业务键字段列表（用于存在性判断）
        /// </summary>
        /// <returns>业务键字段列表</returns>
        /// <remarks>
        /// 返回所有标记为 IsBusinessKey 的字段，用于判断整行数据是否存在于数据库中
        /// </remarks>
        public List<string> GetBusinessKeyFields()
        {
            var resultFields = new List<string>();

            if (ColumnMappings != null)
            {
                foreach (var mapping in ColumnMappings)
                {
                    if (mapping.IsBusinessKey && !string.IsNullOrEmpty(mapping.SystemField?.Key))
                    {
                        resultFields.Add(mapping.SystemField.Key);
                    }
                }
            }

            return resultFields;
        }

        /// <summary>
        /// 更新配置更新时间
        /// </summary>
        public void UpdateTimestamp()
        {
            UpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 根据系统字段名获取映射配置
        /// </summary>
        /// <param name="systemField">系统字段名</param>
        /// <returns>映射配置，如果找不到则返回null</returns>
        public ColumnMapping GetMappingBySystemField(string systemField)
        {
            return ColumnMappings?.FirstOrDefault(m => m.SystemField?.Key == systemField);
        }

        /// <summary>
        /// 根据Excel列名获取映射配置
        /// </summary>
        /// <param name="excelColumn">Excel列名</param>
        /// <returns>映射配置，如果找不到则返回null</returns>
        public ColumnMapping GetMappingByExcelColumn(string excelColumn)
        {
            return ColumnMappings?.FirstOrDefault(m => m.OriginalExcelColumn == excelColumn);
        }
    }

    /// <summary>
    /// 去重策略枚举
    /// </summary>
    public enum DeduplicateStrategy
    {
        /// <summary>
        /// 保留第一条记录（默认）
        /// </summary>
        [Description("保留第一条记录")]
        FirstOccurrence = 0,

        /// <summary>
        /// 保留最后一条记录
        /// </summary>
        [Description("保留最后一条记录")]
        LastOccurrence = 1
    }
}
