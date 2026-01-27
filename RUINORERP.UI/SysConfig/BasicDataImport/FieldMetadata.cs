using System;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 字段元信息模型
    /// 用于存储字段的完整元信息，包括字段名、中文名、数据类型、是否必填等
    /// </summary>
    [Serializable]
    public class FieldMetadata
    {
        /// <summary>
        /// 字段名（数据库列名）
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段中文名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 字段数据类型
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// 数据类型名称（用于序列化）
        /// </summary>
        public string DataTypeName { get; set; }

        /// <summary>
        /// 是否必填字段
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 最大长度（适用于字符串类型）
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否为自增字段
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 是否为外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 外键关联表（如果是外键）
        /// </summary>
        public string ForeignTable { get; set; }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// SugarColumn 特性对象（保存完整的列配置信息）
        /// 注：此属性不序列化到XML配置文件中，仅用于运行时使用
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public SugarColumn SugarColumn { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FieldMetadata()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="displayName">字段中文名</param>
        /// <param name="dataType">数据类型</param>
        public FieldMetadata(string fieldName, string displayName, Type dataType)
        {
            FieldName = fieldName;
            DisplayName = displayName;
            DataType = dataType;
            DataTypeName = dataType?.FullName ?? "System.String";
            IsNullable = true;
        }

        /// <summary>
        /// 检查值是否符合字段约束
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否通过验证</returns>
        public bool ValidateValue(object value, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 优先使用 SugarColumn 的信息进行验证
            bool isRequired = SugarColumn != null ? !SugarColumn.IsNullable : IsRequired;
            bool isNullable = SugarColumn != null ? SugarColumn.IsNullable : IsNullable;
            int maxLength = SugarColumn != null && SugarColumn.Length > 0
                ? SugarColumn.Length
                : MaxLength;

            // 检查必填字段
            if (isRequired && (value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString())))
            {
                errorMessage = $"字段【{DisplayName}】不能为空";
                return false;
            }

            // 如果值为空且允许为空，则通过验证
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString()))
            {
                if (isNullable)
                {
                    return true;
                }
                else
                {
                    errorMessage = $"字段【{DisplayName}】不能为空";
                    return false;
                }
            }

            // 检查最大长度
            if (maxLength > 0 && value is string strValue && strValue.Length > maxLength)
            {
                errorMessage = $"字段【{DisplayName}】长度不能超过{maxLength}个字符，当前长度：{strValue.Length}";
                return false;
            }

            // 检查数据类型
            try
            {
                if (DataType != null && value != null && value.GetType() != DataType)
                {
                    // 尝试类型转换
                    Convert.ChangeType(value, DataType);
                }
            }
            catch
            {
                errorMessage = $"字段【{DisplayName}】的数据类型不正确，期望类型：{DataType?.Name}，实际值：{value}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 转换值到目标类型
        /// </summary>
        /// <param name="value">原始值</param>
        /// <returns>转换后的值</returns>
        public object ConvertValue(object value)
        {
            // 优先使用 SugarColumn 的默认值
            string defaultValue = SugarColumn?.DefaultValue ?? DefaultValue;
            bool isNullable = SugarColumn != null ? SugarColumn.IsNullable : IsNullable;
            bool isRequired = SugarColumn != null ? !SugarColumn.IsNullable : IsRequired;

            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString()))
            {
                if (!isNullable && !isRequired)
                {
                    // 返回默认值
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        return Convert.ChangeType(defaultValue, DataType);
                    }
                    else if (DataType == typeof(string))
                    {
                        return string.Empty;
                    }
                    else if (DataType == typeof(int) || DataType == typeof(long))
                    {
                        return 0;
                    }
                    else if (DataType == typeof(decimal))
                    {
                        return 0m;
                    }
                    else if (DataType == typeof(DateTime))
                    {
                        return DateTime.Now;
                    }
                    else if (DataType == typeof(bool))
                    {
                        return false;
                    }
                }
                return null;
            }

            try
            {
                if (DataType != null && value?.GetType() != DataType)
                {
                    return Convert.ChangeType(value, DataType);
                }
                return value;
            }
            catch
            {
                // 类型转换失败，返回原值或默认值
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    return Convert.ChangeType(defaultValue, DataType);
                }
                return value;
            }
        }

        /// <summary>
        /// 克隆当前对象
        /// </summary>
        /// <returns>克隆的副本</returns>
        public FieldMetadata Clone()
        {
            return new FieldMetadata
            {
                FieldName = this.FieldName,
                DisplayName = this.DisplayName,
                DataType = this.DataType,
                DataTypeName = this.DataTypeName,
                IsRequired = this.IsRequired,
                MaxLength = this.MaxLength,
                IsPrimaryKey = this.IsPrimaryKey,
                IsIdentity = this.IsIdentity,
                IsForeignKey = this.IsForeignKey,
                ForeignTable = this.ForeignTable,
                IsNullable = this.IsNullable,
                DefaultValue = this.DefaultValue,
                Description = this.Description,
                SugarColumn = this.SugarColumn
            };
        }

        public override string ToString()
        {
            return $"{DisplayName} ({FieldName})";
        }
    }
}
