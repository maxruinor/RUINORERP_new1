using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using RUINORERP.Global;
using RUINORERP.Model.ImportEngine.Enums;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据库存在性处理策略枚举
    /// </summary>
    public enum ExistenceStrategy
    {
        [Description("跳过")]
        Skip = 0,

        [Description("更新")]
        Update = 1,

        [Description("报错")]
        Error = 2
    }

    /// <summary>
    /// 列映射配置模型
    /// 用于存储Excel列与系统字段的映射关系
    /// 采用统一的数据源配置架构，每种数据来源类型都有对应的配置类
    /// </summary>
    [Serializable]
    [XmlRoot("ColumnMapping")]
    [XmlInclude(typeof(ExcelConfig))]
    [XmlInclude(typeof(DefaultValueConfig))]
    [XmlInclude(typeof(SystemGeneratedConfig))]
    [XmlInclude(typeof(ForeignKeyConfig))]
    [XmlInclude(typeof(SelfReferenceConfig))]
    [XmlInclude(typeof(FieldCopyConfig))]
    [XmlInclude(typeof(ColumnConcatConfig))]
    [XmlInclude(typeof(ExcelImageConfig))]
    public class ColumnMapping
    {
        #region 基础属性

        /// <summary>
        /// 映射ID（唯一标识）
        /// </summary>
        [XmlElement("MappingId")]
        public Guid MappingId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 系统字段引用（键值对：Key=英文字段名, Value=中文显示名）
        /// </summary>
        [XmlElement("SystemField")]
        public SerializableKeyValuePair<string> SystemField { get; set; }

        /// <summary>
        /// 是否值唯一（用于去重判断）
        /// </summary>
        [XmlElement("IsUniqueValue")]
        public bool IsUniqueValue { get; set; }

        /// <summary>
        /// 是否为业务键字段（用于唯一性校验）
        /// </summary>
        [XmlElement("IsBusinessKey")]
        public bool IsBusinessKey { get; set; }

        /// <summary>
        /// 数据类型（目标字段的数据类型）
        /// </summary>
        [XmlElement("DataType")]
        public string DataType { get; set; }

        /// <summary>
        /// 存在性处理策略（当记录已存在时的处理方式）
        /// </summary>
        [XmlElement("ExistenceStrategy")]
        public ExistenceStrategy ExistenceStrategy { get; set; } = ExistenceStrategy.Skip;

        #endregion

        #region 统一数据源配置

        /// <summary>
        /// 数据来源类型
        /// 用于标识字段数据的来源方式
        /// </summary>
        [XmlElement("DataSourceType")]
        public DataSourceType DataSourceType { get; set; } = DataSourceType.Excel;

        /// <summary>
        /// 数据源配置（统一配置接口）
        /// 根据DataSourceType的不同，存储对应的配置对象
        /// </summary>
        [XmlElement("DataSourceConfig")]
        public IDataSourceConfig DataSourceConfig { get; set; }

        #endregion

        

        #region 便捷属性（用于兼容旧代码）

        /// <summary>
        /// 是否为图片列
        /// </summary>
        [XmlIgnore]
        public bool IsImageColumn { get; set; }

        /// <summary>
        /// 图片列类型
        /// </summary>
        [XmlIgnore]
        public ImageColumnType ImageColumnType { get; set; } = ImageColumnType.Path;

        /// <summary>
        /// 是否忽略空值（为空时不导入）
        /// </summary>
        [XmlIgnore]
        public bool IgnoreEmptyValue { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        [XmlIgnore]
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 自身引用字段
        /// </summary>
        [XmlIgnore]
        public SerializableKeyValuePair<string> SelfReferenceField { get; set; }

        /// <summary>
        /// 复制字段
        /// </summary>
        [XmlIgnore]
        public SerializableKeyValuePair<string> CopyFromField { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [XmlIgnore]
        public bool IsRequired { get; set; }

        /// <summary>
        /// 系统生成配置（便捷属性）
        /// </summary>
        [XmlIgnore]
        public SystemGeneratedConfig SystemGeneratedConfig
        {
            get => DataSourceConfig as SystemGeneratedConfig;
            set
            {
                DataSourceConfig = value;
                if (value != null)
                    DataSourceType = DataSourceType.SystemGenerated;
            }
        }

        /// <summary>
        /// Excel列名（便捷属性，从ExcelConfig获取）
        /// </summary>
        [XmlIgnore]
        public string ExcelColumn
        {
            get => (DataSourceConfig as ExcelConfig)?.ExcelColumn;
            set
            {
                if (DataSourceConfig is ExcelConfig config)
                {
                    config.ExcelColumn = value;
                }
            }
        }

        /// <summary>
        /// 默认值（便捷属性，从DefaultValueConfig获取）
        /// </summary>
        [XmlIgnore]
        public string DefaultValue
        {
            get => (DataSourceConfig as DefaultValueConfig)?.Value;
            set
            {
                if (DataSourceConfig is DefaultValueConfig config)
                {
                    config.Value = value;
                }
            }
        }

        /// <summary>
        /// 外键配置（便捷属性）
        /// </summary>
        [XmlIgnore]
        public ForeignRelatedConfig ForeignConfig
        {
            get
            {
                var config = DataSourceConfig as ForeignKeyConfig;
                if (config != null)
                {
                    return new ForeignRelatedConfig
                    {
                        ForeignKeyTable = config.ForeignKeyTable,
                        ForeignKeyField = config.ForeignKeyField,
                        ForeignKeySourceColumn = config.ForeignKeySourceColumn
                    };
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DataSourceConfig = new ForeignKeyConfig
                    {
                        ForeignKeyTable = value.ForeignKeyTable,
                        ForeignKeyField = value.ForeignKeyField,
                        ForeignKeySourceColumn = value.ForeignKeySourceColumn
                    };
                    DataSourceType = DataSourceType.ForeignKey;
                }
                else
                {
                    DataSourceConfig = null;
                }
            }
        }

        /// <summary>
        /// 列拼接配置（便捷属性）
        /// </summary>
        [XmlIgnore]
        public ColumnConcatConfig ConcatConfig
        {
            get => DataSourceConfig as ColumnConcatConfig;
            set
            {
                DataSourceConfig = value;
                if (value != null)
                    DataSourceType = DataSourceType.ColumnConcat;
            }
        }

        /// <summary>
        /// 图片配置（便捷属性）
        /// </summary>
        [XmlIgnore]
        public ExcelImageConfig ImageConfig
        {
            get => DataSourceConfig as ExcelImageConfig;
            set
            {
                DataSourceConfig = value;
                if (value != null)
                    DataSourceType = DataSourceType.ExcelImage;
            }
        }

        /// <summary>
        /// 枚举默认值配置（便捷属性，从DefaultValueConfig获取）
        /// </summary>
        [XmlIgnore]
        public DefaultValueConfig EnumDefaultConfig
        {
            get => DataSourceConfig as DefaultValueConfig;
            set
            {
                DataSourceConfig = value;
                if (value != null)
                    DataSourceType = DataSourceType.DefaultValue;
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取系统字段名
        /// </summary>
        public string GetSystemFieldName()
        {
            return SystemField?.Key ?? string.Empty;
        }

        /// <summary>
        /// 获取系统字段显示名
        /// </summary>
        public string GetSystemFieldDisplayName()
        {
            return SystemField?.Value ?? string.Empty;
        }

        /// <summary>
        /// 获取配置的显示描述
        /// </summary>
        public string GetConfigDescription()
        {
            switch (DataSourceType)
            {
                case DataSourceType.Excel:
                    var excelConfig = DataSourceConfig as ExcelConfig;
                    return $"Excel列: {excelConfig?.ExcelColumn ?? string.Empty}";

                case DataSourceType.DefaultValue:
                    var defaultConfig = DataSourceConfig as DefaultValueConfig;
                    return $"默认值: {defaultConfig?.Value ?? string.Empty}";

                case DataSourceType.SystemGenerated:
                    var sysConfig = DataSourceConfig as SystemGeneratedConfig;
                    return $"系统生成: {sysConfig?.GetGeneratedTypeDisplayName() ?? "系统生成"}";

                case DataSourceType.ForeignKey:
                    var foreignConfig = DataSourceConfig as ForeignKeyConfig;
                    return $"外键关联: {foreignConfig?.ForeignTableDisplayName ?? "外键"}";

                case DataSourceType.SelfReference:
                    var selfConfig = DataSourceConfig as SelfReferenceConfig;
                    return $"自身引用: {selfConfig?.ReferenceFieldDisplayName ?? "自身引用"}";

                case DataSourceType.FieldCopy:
                    var copyConfig = DataSourceConfig as FieldCopyConfig;
                    return $"字段复制: {copyConfig?.SourceFieldDisplayName ?? "字段复制"}";

                case DataSourceType.ColumnConcat:
                    return "列拼接";

                case DataSourceType.ExcelImage:
                    return "Excel图片";

                default:
                    return "未知来源";
            }
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            // 验证系统字段必须设置
            if (SystemField == null || string.IsNullOrWhiteSpace(SystemField.Key))
            {
                errorMessage = "请选择系统字段";
                return false;
            }

            // 如果有数据源配置，验证配置
            if (DataSourceConfig != null)
            {
                return DataSourceConfig.Validate(out errorMessage);
            }

            return true;
        }

        #endregion
    }
}