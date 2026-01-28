using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 动态数据验证器
    /// 负责验证动态导入数据的合法性
    /// </summary>
    public class DynamicDataValidator
    {
        /// <summary>
        /// 验证数据表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>验证结果列表</returns>
        public List<ValidationError> Validate(DataTable dataTable, ColumnMappingCollection mappings, Type entityType)
        {
            var errors = new List<ValidationError>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                errors.Add(new ValidationError
                {
                    RowNumber = -1,
                    FieldName = "数据表",
                    ErrorMessage = "数据表为空",
                    ErrorType = ErrorType.EmptyData
                });
                return errors;
            }

            // 检查必填列是否存在
            ValidateRequiredColumns(dataTable, mappings, errors);

            // 检查数据类型是否匹配
            ValidateDataTypes(dataTable, mappings, entityType, errors);

            // 检查唯一键列
            ValidateUniqueKeys(dataTable, mappings, errors);

            // 检查数据范围
            ValidateDataRanges(dataTable, mappings, entityType, errors);

            return errors;
        }

        /// <summary>
        /// 验证必填列是否存在
        /// </summary>
        /// <param name="dataTable">数据表（已应用映射，列名为SystemField）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="errors">错误列表</param>
        private void ValidateRequiredColumns(DataTable dataTable, ColumnMappingCollection mappings, List<ValidationError> errors)
        {
            foreach (var mapping in mappings)
            {
                // 跳过系统生成和默认值的映射
                if (IsSystemGeneratedOrDefaultValueMapping(mapping))
                {
                    continue;
                }

                // 检查映射后的数据表是否包含SystemField列
                if (!dataTable.Columns.Contains(mapping.SystemField?.Key))
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = -1,
                        FieldName = mapping.SystemField?.Key,
                        ErrorMessage = $"映射后的数据表中不存在列 '{mapping.SystemField?.Key}'",
                        ErrorType = ErrorType.ColumnNotFound
                    });
                }
            }
        }

        /// <summary>
        /// 判断是否为系统生成或默认值的映射
        /// </summary>
        /// <param name="mapping">列映射</param>
        /// <returns>是否为系统生成或默认值映射</returns>
        private bool IsSystemGeneratedOrDefaultValueMapping(ColumnMapping mapping)
        {
            // 检查ExcelColumn是否以特殊标记开头
            if (string.IsNullOrEmpty(mapping.ExcelColumn))
            {
                return false;
            }

            return mapping.ExcelColumn.StartsWith("[系统生成]") ||
                   mapping.ExcelColumn.StartsWith("[默认值:");
        }

        /// <summary>
        /// 验证数据类型是否匹配
        /// </summary>
        /// <param name="dataTable">数据表（已应用映射，列名为SystemField）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="errors">错误列表</param>
        private void ValidateDataTypes(DataTable dataTable, ColumnMappingCollection mappings, Type entityType, List<ValidationError> errors)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                foreach (var mapping in mappings)
                {
                    // 跳过系统生成和默认值的映射
                    if (IsSystemGeneratedOrDefaultValueMapping(mapping))
                    {
                        continue;
                    }

                    // 使用SystemField检查列是否存在
                    //应该是验证显示名称
                    if (!dataTable.Columns.Contains(mapping.SystemField?.Value))
                    {
                        continue;
                    }

                    object cellValue = row[mapping.SystemField?.Value];
                    if (cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString()))
                    {
                        continue; // 空值跳过
                    }

                    // 获取目标属性
                    PropertyInfo property = entityType.GetProperty(mapping.SystemField?.Key);
                    if (property == null)
                    {
                        continue;
                    }

                    // 验证数据类型
                    Type targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    if (!TryConvertValue(cellValue, targetType, out _))
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = i + 2, // +2 因为Excel从第2行开始
                            FieldName = mapping.SystemField?.Key,
                            ErrorMessage = $"值 '{cellValue}' 无法转换为类型 {targetType.Name}",
                            ErrorType = ErrorType.TypeMismatch,
                            OriginalValue = cellValue
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 验证唯一键列
        /// </summary>
        /// <param name="dataTable">数据表（已应用映射，列名为SystemField）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="errors">错误列表</param>
        private void ValidateUniqueKeys(DataTable dataTable, ColumnMappingCollection mappings, List<ValidationError> errors)
        {
            var uniqueKeyMapping = mappings.GetUniqueKeyMapping();
            if (uniqueKeyMapping == null)
            {
                return;
            }

            // 使用SystemField检查列是否存在
            if (!dataTable.Columns.Contains(uniqueKeyMapping.SystemField?.Key))
            {
                return;
            }

            var valueMap = new Dictionary<string, List<int>>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                object value = dataTable.Rows[i][uniqueKeyMapping.SystemField?.Key];

                // 如果配置了忽略空值，则跳过空值的重复检查
                bool isEmpty = value == DBNull.Value || string.IsNullOrEmpty(value?.ToString());
                if (isEmpty)
                {
                    // 如果没有设置忽略空值，空值仍然是错误
                    if (!uniqueKeyMapping.IgnoreEmptyValue)
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = i + 2,
                            FieldName = uniqueKeyMapping.SystemField?.Key,
                            ErrorMessage = "唯一键列不能为空",
                            ErrorType = ErrorType.EmptyRequiredField,
                            OriginalValue = null
                        });
                    }
                    // 跳过空值的重复检查
                    continue;
                }

                string stringValue = value.ToString();
                if (!valueMap.ContainsKey(stringValue))
                {
                    valueMap[stringValue] = new List<int>();
                }
                valueMap[stringValue].Add(i + 2);
            }

            // 检查重复值
            foreach (var kvp in valueMap.Where(x => x.Value.Count > 1))
            {
                foreach (int rowNumber in kvp.Value)
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = rowNumber,
                        FieldName = uniqueKeyMapping.SystemField?.Key,
                        ErrorMessage = $"唯一键列值 '{kvp.Key}' 重复，共出现 {kvp.Value.Count} 次",
                        ErrorType = ErrorType.DuplicateValue,
                        OriginalValue = kvp.Key
                    });
                }
            }
        }

        /// <summary>
        /// 验证数据范围
        /// </summary>
        /// <param name="dataTable">数据表（已应用映射，列名为SystemField）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="errors">错误列表</param>
        private void ValidateDataRanges(DataTable dataTable, ColumnMappingCollection mappings, Type entityType, List<ValidationError> errors)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                foreach (var mapping in mappings)
                {
                    // 跳过系统生成和默认值的映射
                    if (IsSystemGeneratedOrDefaultValueMapping(mapping))
                    {
                        continue;
                    }

                    // 使用SystemField检查列是否存在
                    if (!dataTable.Columns.Contains(mapping.SystemField?.Key))
                    {
                        continue;
                    }

                    object cellValue = row[mapping.SystemField?.Key];
                    if (cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString()))
                    {
                        continue;
                    }

                    // 获取目标属性
                    PropertyInfo property = entityType.GetProperty(mapping.SystemField?.Key);
                    if (property == null)
                    {
                        continue;
                    }

                    Type targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    // 验证字符串长度
                    if (targetType == typeof(string))
                    {
                        int maxLength = GetMaxLength(property);
                        if (maxLength > 0 && cellValue.ToString().Length > maxLength)
                        {
                            errors.Add(new ValidationError
                            {
                                RowNumber = i + 2,
                                FieldName = mapping.SystemField?.Key,
                                ErrorMessage = $"字符串长度 {cellValue.ToString().Length} 超过最大限制 {maxLength}",
                                ErrorType = ErrorType.LengthExceeded,
                                OriginalValue = cellValue
                            });
                        }
                    }

                    // 验证数值范围
                    if (targetType == typeof(decimal) || targetType == typeof(double) || targetType == typeof(float))
                    {
                        if (TryConvertValue(cellValue, targetType, out object convertedValue))
                        {
                            decimal numValue = Convert.ToDecimal(convertedValue);
                            var range = GetNumberRange(property);
                            if (range.HasValue)
                            {
                                if (numValue < range.Value.Min || numValue > range.Value.Max)
                                {
                                    errors.Add(new ValidationError
                                    {
                                        RowNumber = i + 2,
                                        FieldName = mapping.SystemField?.Key,
                                        ErrorMessage = $"数值 {numValue} 超出有效范围 [{range.Value.Min}, {range.Value.Max}]",
                                        ErrorType = ErrorType.OutOfRange,
                                        OriginalValue = cellValue
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 尝试转换值
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="convertedValue">转换后的值</param>
        /// <returns>是否转换成功</returns>
        private bool TryConvertValue(object value, Type targetType, out object convertedValue)
        {
            convertedValue = null;

            try
            {
                string stringValue = value.ToString();

                if (targetType == typeof(string))
                {
                    convertedValue = stringValue;
                    return true;
                }
                else if (targetType == typeof(int))
                {
                    if (int.TryParse(stringValue, out int intValue))
                    {
                        convertedValue = intValue;
                        return true;
                    }
                }
                else if (targetType == typeof(long))
                {
                    if (long.TryParse(stringValue, out long longValue))
                    {
                        convertedValue = longValue;
                        return true;
                    }
                }
                else if (targetType == typeof(decimal))
                {
                    if (decimal.TryParse(stringValue, out decimal decimalValue))
                    {
                        convertedValue = decimalValue;
                        return true;
                    }
                }
                else if (targetType == typeof(double))
                {
                    if (double.TryParse(stringValue, out double doubleValue))
                    {
                        convertedValue = doubleValue;
                        return true;
                    }
                }
                else if (targetType == typeof(DateTime))
                {
                    if (DateTime.TryParse(stringValue, out DateTime dateTimeValue))
                    {
                        convertedValue = dateTimeValue;
                        return true;
                    }
                }
                else if (targetType == typeof(bool))
                {
                    if (bool.TryParse(stringValue, out bool boolValue))
                    {
                        convertedValue = boolValue;
                        return true;
                    }
                }
            }
            catch
            {
                // 转换失败
            }

            return false;
        }

        /// <summary>
        /// 获取字符串最大长度
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>最大长度，-1表示无限制</returns>
        private int GetMaxLength(PropertyInfo property)
        {
            // 检查是否有StringLengthAttribute
            var stringLengthAttr = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.StringLengthAttribute;

            if (stringLengthAttr != null)
            {
                return stringLengthAttr.MaximumLength;
            }

            // 默认限制
            return -1;
        }

        /// <summary>
        /// 获取数值范围
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>数值范围，null表示无限制</returns>
        private (decimal Min, decimal Max)? GetNumberRange(PropertyInfo property)
        {
            decimal min = decimal.MinValue;
            decimal max = decimal.MaxValue;

            // 检查是否有RangeAttribute
            var rangeAttr = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RangeAttribute), false)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.RangeAttribute;

            if (rangeAttr != null)
            {
                min = Convert.ToDecimal(rangeAttr.Minimum);
                max = Convert.ToDecimal(rangeAttr.Maximum);
                return (min, max);
            }

            // 检查是否为价格、数量等字段，添加默认限制
            string propName = property.Name.ToLower();
            if (propName.Contains("price") || propName.Contains("cost") || propName.Contains("amount"))
            {
                return (0, decimal.MaxValue); // 价格不能为负
            }
            else if (propName.Contains("quantity") || propName.Contains("count") || propName.Contains("stock"))
            {
                return (0, int.MaxValue); // 数量不能为负
            }

            return null;
        }
    }

    /// <summary>
    /// 验证错误
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public ErrorType ErrorType { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public object OriginalValue { get; set; }
    }

    /// <summary>
    /// 错误类型
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// 数据表为空
        /// </summary>
        EmptyData,

        /// <summary>
        /// 列不存在
        /// </summary>
        ColumnNotFound,

        /// <summary>
        /// 类型不匹配
        /// </summary>
        TypeMismatch,

        /// <summary>
        /// 必填字段为空
        /// </summary>
        EmptyRequiredField,

        /// <summary>
        /// 重复值
        /// </summary>
        DuplicateValue,

        /// <summary>
        /// 长度超出限制
        /// </summary>
        LengthExceeded,

        /// <summary>
        /// 数值超出范围
        /// </summary>
        OutOfRange
    }
}
