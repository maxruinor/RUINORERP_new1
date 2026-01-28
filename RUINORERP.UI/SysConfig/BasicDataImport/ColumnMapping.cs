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
        FieldCopy = 5
    }

    /// <summary>
    /// 列映射配置模型
    /// 用于存储Excel列与系统字段的映射关系
    /// </summary>
    public class ColumnMapping
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Excel列名（数据来源标识）
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 系统字段引用（键值对：Key=英文字段名, Value=中文显示名）
        /// </summary>
        public SerializableKeyValuePair<string> SystemField { get; set; }

        /// <summary>
        /// 字段元信息（包含完整的字段定义）
        /// 注：此属性不序列化到XML配置文件中，仅用于运行时使用
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public FieldMetadata FieldMetadata { get; set; }


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
        /// 是否为外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 外键表引用（键值对：Key=英文表名, Value=中文表名）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

        /// <summary>
        /// 外键字段引用（键值对：Key=英文字段名, Value=中文显示名）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

        /// <summary>
        /// 数据来源类型
        /// 用于标识字段数据的来源方式
        /// </summary>
        public DataSourceType DataSourceType { get; set; } = DataSourceType.Excel;

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
        /// 根据字段元信息初始化字段属性
        /// </summary>
        /// <param name="metadata">字段元信息</param>
        public void InitializeFromMetadata(FieldMetadata metadata)
        {
            if (metadata == null)
            {
                return;
            }

            // 使用统一的SerializableKeyValuePair结构存储字段信息
            SystemField = new SerializableKeyValuePair<string>(metadata.FieldName, metadata.DisplayName);
            DataType = metadata.DataTypeName;
            IsForeignKey = metadata.IsForeignKey;
            IsSystemGenerated = metadata.IsIdentity || metadata.IsPrimaryKey;

            // 如果有外键表，初始化外键表引用
            if (!string.IsNullOrEmpty(metadata.ForeignTable))
            {
                ForeignKeyTable = new SerializableKeyValuePair<string>(metadata.ForeignTable, GetTableDisplayNameInternal(metadata.ForeignTable));
            }

            // 根据属性设置数据来源类型
            if (IsSystemGenerated)
            {
                DataSourceType = DataSourceType.SystemGenerated;
            }
            else if (IsForeignKey)
            {
                DataSourceType = DataSourceType.ForeignKey;
            }
            else if (!string.IsNullOrEmpty(metadata.DefaultValue))
            {
                DataSourceType = DataSourceType.DefaultValue;
            }

            if (string.IsNullOrEmpty(DefaultValue))
            {
                DefaultValue = metadata.DefaultValue;
            }
        }

        /// <summary>
        /// 验证字段值
        /// </summary>
        /// <param name="value">要验证的值</param>
        /// <param name="errorMessage">错误信息输出</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateValue(object value, out string errorMessage)
        {
            if (FieldMetadata != null)
            {
                return FieldMetadata.ValidateValue(value, out errorMessage);
            }

            // 如果没有元信息，进行基础验证
            errorMessage = string.Empty;
            
            if (IsRequired && (value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString())))
            {
                errorMessage = $"字段【{SystemField?.Value ?? SystemField?.Key ?? string.Empty}】不能为空";
                return false;
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
            if (FieldMetadata != null)
            {
                return FieldMetadata.ConvertValue(value);
            }

            // 如果有默认值且值为空，返回默认值
            if ((value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString())) 
                && !string.IsNullOrEmpty(DefaultValue))
            {
                return DefaultValue;
            }

            return value;
        }

        /// <summary>
        /// 是否必填字段
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsRequired
        {
            get
            {
                if (FieldMetadata != null)
                {
                    return FieldMetadata.IsRequired;
                }
                return false;
            }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int MaxLength
        {
            get
            {
                if (FieldMetadata != null)
                {
                    return FieldMetadata.MaxLength;
                }
                return 0;
            }
        }

        /// <summary>
        /// 获取表的中文显示名称（内部辅助方法）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>中文显示名称</returns>
        private string GetTableDisplayNameInternal(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;

            var tableNames = new Dictionary<string, string>
            {
                { "tb_CustomerVendor", "客户供应商表" },
                { "tb_ProdCategories", "产品类目表" },
                { "tb_Prod", "产品基本信息表" },
                { "tb_ProdDetail", "产品详情信息表" },
                { "tb_ProdProperty", "产品属性表" },
                { "tb_ProdPropertyValue", "产品属性值表" }
            };

            return tableNames.ContainsKey(tableName) ? tableNames[tableName] : tableName;
        }
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

    }
}