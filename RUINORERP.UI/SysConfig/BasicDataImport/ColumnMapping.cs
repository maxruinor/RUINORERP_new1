using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        SelfReference = 4
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
        /// Excel列名
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 系统字段名（实际数据库列名）
        /// </summary>
        public string SystemField { get; set; }

        /// <summary>
        /// 系统字段中文名称
        /// </summary>
        public string SystemFieldDisplayName { get; set; }

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
        /// 映射配置名称
        /// </summary>
        public string MappingName { get; set; }

        /// <summary>
        /// 是否对该字段进行去重复处理
        /// 启用后，在解析和导入时会根据该字段的值去除重复的数据行
        /// 只保留第一次出现的记录，后续重复的记录将被过滤掉
        /// </summary>
        public bool RemoveDuplicates { get; set; }

        /// <summary>
        /// 目标实体类型名称
        /// </summary>
        public string EntityType { get; set; }

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
        /// 关联表名（外键关联的表）
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 关联表字段（中文名称，用于显示）
        /// </summary>
        public string RelatedTableField { get; set; }

        /// <summary>
        /// 关联表字段（实际字段名）
        /// </summary>
        public string RelatedTableFieldName { get; set; }

        /// <summary>
        /// 数据来源类型
        /// 用于标识字段数据的来源方式
        /// </summary>
        public DataSourceType DataSourceType { get; set; } = DataSourceType.Excel;

        /// <summary>
        /// 自身引用字段（当DataSourceType为SelfReference时使用）
        /// 存储当前表自身的字段名（如树结构中的父类ID对应ID字段）
        /// </summary>
        public string SelfReferenceFieldName { get; set; }

        /// <summary>
        /// 自身引用字段中文名称（用于显示）
        /// </summary>
        public string SelfReferenceFieldDisplayName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

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

            SystemField = metadata.FieldName;
            SystemFieldDisplayName = metadata.DisplayName;
            DataType = metadata.DataTypeName;
            IsForeignKey = metadata.IsForeignKey;
            IsSystemGenerated = metadata.IsIdentity || metadata.IsPrimaryKey;
            RelatedTableName = metadata.ForeignTable;

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
                errorMessage = $"字段【{SystemFieldDisplayName ?? SystemField}】不能为空";
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
    }

    /// <summary>
    /// 列映射配置集合
    /// </summary>
    public class ColumnMappingCollection : List<ColumnMapping>
    {
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
            return this.FirstOrDefault(m => m.SystemField.Equals(systemField, StringComparison.OrdinalIgnoreCase));
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
        /// 获取启用去重复值的映射配置
        /// </summary>
        /// <returns>启用了去重复的映射配置，如果没有则返回null</returns>
        public ColumnMapping GetRemoveDuplicatesMapping()
        {
            return this.FirstOrDefault(m => m.RemoveDuplicates);
        }

        /// <summary>
        /// 检查是否需要去重复
        /// </summary>
        /// <returns>是否启用去重复</returns>
        public bool IsRemoveDuplicatesEnabled()
        {
            return this.Any(m => m.RemoveDuplicates);
        }
    }
}