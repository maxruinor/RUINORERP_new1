using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RUINORERP.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据来源类型枚举
    /// 用于标识字段数据的来源方式
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// Excel数据源（默认）
        /// 数据来源于Excel文件的对应列
        /// </summary>
        Excel = 0,

        /// <summary>
        /// 默认值
        /// 数据来源于配置的默认值
        /// </summary>
        DefaultValue = 1,

        /// <summary>
        /// 系统生成
        /// 数据由系统自动生成（如自增ID、时间戳等）
        /// </summary>
        SystemGenerated = 2,

        /// <summary>
        /// 外键关联
        /// 数据来源于其他表的外键关联字段
        /// </summary>
        ForeignKey = 3,

        /// <summary>
        /// 自身字段引用
        /// 数据来源于当前表自身的其他字段（如树结构中的父类ID）
        /// </summary>
        SelfReference = 4,

        /// <summary>
        /// 字段复制
        /// 复制同一记录中另一个字段的值
        /// 例如：ProductName字段复制ProductCode字段的值
        /// </summary>
        FieldCopy = 5,

        /// <summary>
        /// 列拼接
        /// 将Excel中的多个列值拼接后赋值给目标字段
        /// 例如：将"姓氏"和"名字"列拼接为"姓名"字段
        /// </summary>
        ColumnConcat = 6
    }

    /// <summary>
    /// 列拼接配置
    /// 用于配置多个Excel列的拼接规则
    /// </summary>
    [Serializable]
    public class ColumnConcatConfig
    {
        /// <summary>
        /// 要拼接的Excel列名列表（按顺序拼接）
        /// </summary>
        public List<string> SourceColumns { get; set; }

        /// <summary>
        /// 列之间的分隔符
        /// 例如："-"、"_"、空格、空字符串等
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// 是否去除每个列的前后空格
        /// </summary>
        public bool TrimWhitespace { get; set; }

        /// <summary>
        /// 是否忽略空值列
        /// 为true时，空值列不会参与拼接
        /// </summary>
        public bool IgnoreEmptyColumns { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ColumnConcatConfig()
        {
            SourceColumns = new List<string>();
            Separator = "";
            TrimWhitespace = true;
            IgnoreEmptyColumns = false;
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否有效</returns>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (SourceColumns == null || SourceColumns.Count == 0)
            {
                errorMessage = "必须至少指定一个源列";
                return false;
            }

            if (SourceColumns.Count < 2)
            {
                errorMessage = "列拼接至少需要两个源列";
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 列映射配置模型
    /// 用于存储Excel列与系统字段的映射关系
    /// </summary>
    public class ColumnMapping
    {
        /// <summary>
        /// Excel列名（数据来源标识）
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 系统字段引用（键值对：Key=英文字段名, Value=中文显示名）
        /// </summary>
        public SerializableKeyValuePair<string> SystemField { get; set; }

        /// <summary>
        /// 是否值唯一
        /// </summary>
        public bool IsUniqueValue { get; set; }

        /// <summary>
        /// 是否忽略空值（为空时不导入）
        /// </summary>
        public bool IgnoreEmptyValue { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 数据来源类型
        /// 用于标识字段数据的来源方式
        /// </summary>
        public DataSourceType DataSourceType { get; set; } = DataSourceType.Excel;

        /// <summary>
        /// 外键关联配置（当DataSourceType为ForeignKey时使用）
        /// 存储外部关联类型的列配置信息
        /// </summary>
        public ForeignRelatedConfig ForeignConfig { get; set; }

        /// <summary>
        /// 自身引用字段（当DataSourceType为SelfReference时使用）
        /// 键值对：Key=英文字段名, Value=中文显示名
        /// 存储当前表自身的字段引用（如树结构中的父类ID对应ID字段）
        /// </summary>
        public SerializableKeyValuePair<string> SelfReferenceField { get; set; }

        /// <summary>
        /// 复制字段（当DataSourceType为FieldCopy时使用）
        /// 键值对：Key=英文字段名, Value=中文显示名
        /// 存储当前表中要复制值的另一个字段名
        /// 例如：ProductName字段复制ProductCode字段的值
        /// </summary>
        public SerializableKeyValuePair<string> CopyFromField { get; set; }

        /// <summary>
        /// 列拼接配置（当DataSourceType为ColumnConcat时使用）
        /// 存储多个Excel列的拼接规则
        /// </summary>
        public ColumnConcatConfig ConcatConfig { get; set; }

        /// <summary>
        /// 枚举类型完整名称
        /// 当数据库字段类型为int但实际使用枚举时，可手动指定枚举类型
        /// 格式："命名空间.枚举名" 如："RUINORERP.Model.EnumProductType"
        /// </summary>
        public string EnumTypeName { get; set; }

        /// <summary>
        /// 是否必填字段
        /// </summary>
        [XmlIgnore]
        public bool IsRequired { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        [XmlIgnore]
        public int MaxLength { get; set; }

        /// <summary>
        /// 验证字段值
        /// </summary>
        /// <param name="value">要验证的值</param>
        /// <param name="errorMessage">错误信息输出</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateValue(object value, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 基础验证：检查必填字段
            if (IsRequired && (value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString())))
            {
                errorMessage = $"字段【{SystemField?.Value ?? SystemField?.Key ?? string.Empty}】不能为空";
                return false;
            }

            // 外键关联验证
            if (DataSourceType == DataSourceType.ForeignKey && ForeignConfig != null)
            {
                if (!ForeignConfig.Validate(out string foreignError))
                {
                    errorMessage = foreignError;
                    return false;
                }
            }

            // 列拼接配置验证
            if (DataSourceType == DataSourceType.ColumnConcat && ConcatConfig != null)
            {
                if (!ConcatConfig.Validate(out string concatError))
                {
                    errorMessage = concatError;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 转换字段值到目标类型
        /// </summary>
        /// <param name="value">原始值</param>
        /// <returns>转换后的值</returns>
        public object ConvertValue(object value)
        {
            // 如果有默认值且值为空，返回默认值
            if ((value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString()))
                && !string.IsNullOrEmpty(DefaultValue))
            {
                return DefaultValue;
            }

            return value;
        }

        /// <summary>
        /// 获取外键表名（便捷属性）
        /// </summary>
        [XmlIgnore]
        public string ForeignKeyTableName => ForeignConfig?.ForeignKeyTable?.Key;

        /// <summary>
        /// 获取外键字段名（便捷属性）
        /// </summary>
        [XmlIgnore]
        public string ForeignKeyFieldName => ForeignConfig?.ForeignKeyField?.Key;

        /// <summary>
        /// 获取外键来源列名（便捷属性）
        /// </summary>
        [XmlIgnore]
        public string ForeignKeySourceColumnName => ForeignConfig?.ForeignKeySourceColumn?.ExcelColumnName;
    }

    /// <summary>
    /// 列映射配置集合
    /// </summary>
    public class ColumnMappingCollection : List<ColumnMapping>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ColumnMappingCollection() : base()
        {
        }

        /// <summary>
        /// 从现有集合初始化
        /// </summary>
        /// <param name="collection">列映射集合</param>
        public ColumnMappingCollection(IEnumerable<ColumnMapping> collection) : base(collection)
        {
        }

        /// <summary>
        /// 根据Excel列名获取映射配置
        /// </summary>
        /// <param name="excelColumn">Excel列名</param>
        /// <returns>列映射配置</returns>
        public ColumnMapping GetMappingByExcelColumn(string excelColumn)
        {
            return this.FirstOrDefault(m => m.ExcelColumn.Equals(excelColumn, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 根据系统字段名获取映射配置
        /// </summary>
        /// <param name="systemField">系统字段名</param>
        /// <returns>列映射配置</returns>
        public ColumnMapping GetMappingBySystemField(string systemField)
        {
            return this.FirstOrDefault(m => m.SystemField?.Key.Equals(systemField, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        /// <summary>
        /// 获取唯一标识列映射
        /// </summary>
        /// <returns>唯一标识列映射配置</returns>
        public ColumnMapping GetUniqueKeyMapping()
        {
            return this.FirstOrDefault(m => m.IsUniqueValue);
        }

        /// <summary>
        /// 获取所有启用忽略空值的映射
        /// </summary>
        /// <returns>忽略空值的映射配置列表</returns>
        public List<ColumnMapping> GetIgnoreEmptyValueMappings()
        {
            return this.Where(m => m.IgnoreEmptyValue).ToList();
        }

        /// <summary>
        /// 获取所有外键关联映射
        /// </summary>
        /// <returns>外键关联映射配置列表</returns>
        public List<ColumnMapping> GetForeignKeyMappings()
        {
            return this.Where(m => m.DataSourceType == DataSourceType.ForeignKey && m.ForeignConfig != null).ToList();
        }
    }
}
