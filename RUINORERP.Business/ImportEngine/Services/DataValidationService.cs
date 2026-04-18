using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Business.ImportEngine.Services;
using RUINORERP.Model.ImportEngine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 数据验证服务实现
    /// 负责导入前的数据验证，包括必填项、数据类型、外键关联等
    /// </summary>
    public class DataValidationService : IDataValidationService
    {
        private readonly IForeignKeyCacheService _foreignKeyService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="foreignKeyService">外键缓存服务</param>
        public DataValidationService(IForeignKeyCacheService foreignKeyService)
        {
            _foreignKeyService = foreignKeyService ?? throw new ArgumentNullException(nameof(foreignKeyService));
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="data">待验证的数据表</param>
        /// <param name="config">验证配置</param>
        /// <returns>验证错误列表，如果为空表示验证通过</returns>
        public List<ValidationError> Validate(DataTable data, Models.ValidationConfig config)
        {
            var errors = new List<ValidationError>();

            if (data == null || data.Rows.Count == 0)
            {
                errors.Add(new ValidationError
                {
                    RowNumber = -1,
                    FieldName = "数据表",
                    ErrorMessage = "数据表为空",
                    ErrorType = "EmptyData"
                });
                return errors;
            }

            // TODO: 后续完善验证逻辑
            // 目前暂时跳过详细验证，等待UCBasicDataImport改造后再实现
            
            return errors;
        }

        /// <summary>
        /// 检查唯一性约束
        /// 验证导入数据中的唯一性字段是否已存在于数据库中
        /// </summary>
        /// <param name="data">待验证的数据表</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="uniqueFields">唯一性字段列表</param>
        /// <returns>重复记录错误列表</returns>
        public List<ValidationError> CheckUniqueValues(
            DataTable data, 
            string tableName, 
            List<string> uniqueFields)
        {
            var errors = new List<ValidationError>();

            if (data == null || uniqueFields == null || uniqueFields.Count == 0)
            {
                return errors;
            }

            // 验证字段是否存在
            foreach (var field in uniqueFields)
            {
                if (!data.Columns.Contains(field))
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = -1,
                        FieldName = field,
                        ErrorMessage = $"唯一性字段 '{field}' 在数据表中不存在",
                        ErrorType = "ColumnNotFound"
                    });
                }
            }

            if (errors.Any())
            {
                return errors;
            }

            // 检查数据内部的重复
            var valueMap = new Dictionary<string, List<int>>();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                // 生成组合键
                var keyParts = uniqueFields.Select(f => data.Rows[i][f]?.ToString() ?? "").ToList();
                string key = string.Join("|", keyParts);

                if (string.IsNullOrEmpty(key))
                {
                    continue; // 跳过空值
                }

                if (!valueMap.ContainsKey(key))
                {
                    valueMap[key] = new List<int>();
                }
                valueMap[key].Add(i + 2); // Excel行号从2开始
            }

            // 收集重复值
            foreach (var kvp in valueMap.Where(x => x.Value.Count > 1))
            {
                foreach (int rowNumber in kvp.Value)
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = rowNumber,
                        FieldName = string.Join(", ", uniqueFields),
                        ErrorMessage = $"唯一键值 '{kvp.Key}' 重复，共出现 {kvp.Value.Count} 次",
                        ErrorType = "DuplicateValue"
                    });
                }
            }

            return errors;
        }

        #region 私有验证方法

        /// <summary>
        /// 验证必填列是否存在
        /// </summary>
        private void ValidateRequiredColumns(DataTable dataTable, ImportProfile profile, List<ValidationError> errors)
        {
            if (profile.ColumnMappings == null)
            {
                return;
            }

            foreach (var mapping in profile.ColumnMappings)
            {
                // 跳过系统生成和默认值的映射
                if (IsSystemGeneratedOrDefaultValueMapping(mapping))
                {
                    continue;
                }

                // 检查是否为必填字段
                if (!mapping.IsRequired)
                {
                    continue;
                }

                // 检查映射后的数据表是否包含SystemField列
                if (!dataTable.Columns.Contains(mapping.SystemField?.Item2))
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = -1,
                        FieldName = mapping.SystemField?.Item1,
                        ErrorMessage = $"映射后的数据表中不存在列 '{mapping.SystemField?.Item2}'",
                        ErrorType = "ColumnNotFound"
                    });
                }
            }
        }

        /// <summary>
        /// 判断是否为系统生成或默认值的映射
        /// </summary>
        private bool IsSystemGeneratedOrDefaultValueMapping(ColumnMapping mapping)
        {
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
        private void ValidateDataTypes(DataTable dataTable, ImportProfile profile, List<ValidationError> errors)
        {
            if (profile.ColumnMappings == null)
            {
                return;
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                foreach (var mapping in profile.ColumnMappings)
                {
                    // 跳过系统生成和默认值的映射
                    if (IsSystemGeneratedOrDefaultValueMapping(mapping))
                    {
                        continue;
                    }

                    // 检查列是否存在
                    if (!dataTable.Columns.Contains(mapping.SystemField?.Item2))
                    {
                        continue;
                    }

                    object cellValue = row[mapping.SystemField?.Item2];
                    if (cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString()))
                    {
                        continue; // 空值跳过
                    }

                    // 外键验证由ValidateForeignKeys统一处理
                    if (mapping.DataSourceType == RUINORERP.Model.ImportEngine.Enums.DataSourceType.ForeignKey)
                    {
                        continue;
                    }

                    // 验证数据类型（这里简化处理，实际应该根据目标类型验证）
                    // 由于Business层不依赖具体的实体类型，这里只做基础验证
                    // 详细的类型验证应该在SmartImportEngine中进行
                }
            }
        }

        /// <summary>
        /// 验证唯一键列
        /// </summary>
        private void ValidateUniqueKeys(DataTable dataTable, ImportProfile profile, List<ValidationError> errors)
        {
            if (profile.ColumnMappings == null)
            {
                return;
            }

            // 查找标记为唯一键的映射
            var uniqueKeyMapping = profile.ColumnMappings.FirstOrDefault(m => m.IsUniqueKey);
            if (uniqueKeyMapping == null)
            {
                return;
            }

            // 检查列是否存在
            if (!dataTable.Columns.Contains(uniqueKeyMapping.SystemField?.Item2))
            {
                return;
            }

            var valueMap = new Dictionary<string, List<int>>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                object value = dataTable.Rows[i][uniqueKeyMapping.SystemField?.Item2];

                // 如果配置了忽略空值，则跳过空值的重复检查
                bool isEmpty = value == DBNull.Value || string.IsNullOrEmpty(value?.ToString());
                if (isEmpty)
                {
                    if (!uniqueKeyMapping.IgnoreEmptyValue)
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = i + 2,
                            FieldName = uniqueKeyMapping.SystemField?.Item1,
                            ErrorMessage = "唯一键列不能为空",
                            ErrorType = "EmptyRequiredField"
                        });
                    }
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
                        FieldName = uniqueKeyMapping.SystemField?.Item1,
                        ErrorMessage = $"唯一键列值 '{kvp.Key}' 重复，共出现 {kvp.Value.Count} 次",
                        ErrorType = "DuplicateValue"
                    });
                }
            }
        }

        /// <summary>
        /// 验证数据范围
        /// </summary>
        private void ValidateDataRanges(DataTable dataTable, ImportProfile profile, List<ValidationError> errors)
        {
            // 简化版本：不做详细的数据范围验证
            // 详细的范围验证应该在SmartImportEngine中根据实体属性进行
        }

        /// <summary>
        /// 验证外键
        /// </summary>
        private void ValidateForeignKeys(DataTable dataTable, ImportProfile profile, List<ValidationError> errors)
        {
            if (profile.ColumnMappings == null)
            {
                return;
            }

            // 筛选出所有外键映射
            var foreignKeyMappings = profile.ColumnMappings
                .Where(m => m.DataSourceType == RUINORERP.Model.ImportEngine.Enums.DataSourceType.ForeignKey)
                .ToList();

            if (!foreignKeyMappings.Any())
            {
                return;
            }

            // 遍历所有数据行
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                int rowNumber = i + 2; // Excel行号从2开始

                // 遍历所有外键映射
                foreach (var mapping in foreignKeyMappings)
                {
                    // 获取外键值
                    string sourceValue = GetForeignKeySourceValue(row, mapping);
                    
                    if (string.IsNullOrEmpty(sourceValue))
                    {
                        // 如果是必填字段，报错
                        if (mapping.IsRequired)
                        {
                            errors.Add(new ValidationError
                            {
                                RowNumber = rowNumber,
                                FieldName = mapping.SystemField?.Item1,
                                ErrorMessage = $"外键字段 '{mapping.SystemField?.Item2}' 是必填字段，但值为空",
                                ErrorType = "EmptyRequiredField"
                            });
                        }
                        continue;
                    }

                    // 使用外键缓存服务验证
                    string tableName = mapping.ForeignConfig?.ForeignKeyTable?.Key;
                    string errorMessage;
                    
                    var foreignKeyId = _foreignKeyService.GetForeignKeyValue(tableName, sourceValue, out errorMessage);
                    
                    if (foreignKeyId == null && !string.IsNullOrEmpty(errorMessage))
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = rowNumber,
                            FieldName = mapping.SystemField?.Item1,
                            ErrorMessage = errorMessage,
                            ErrorType = "ForeignKeyValidationFailed"
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 获取外键来源值
        /// </summary>
        private string GetForeignKeySourceValue(DataRow row, ColumnMapping mapping)
        {
            // 优先从指定的外键来源列获取
            if (mapping.ForeignConfig?.ForeignKeySourceColumn != null &&
                !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn.ExcelColumnName))
            {
                string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.ExcelColumnName;
                if (row.Table.Columns.Contains(sourceColumnName))
                {
                    return row[sourceColumnName]?.ToString()?.Trim();
                }
            }

            // 否则使用SystemField列
            if (mapping.SystemField != null && 
                !string.IsNullOrEmpty(mapping.SystemField.Item2) &&
                row.Table.Columns.Contains(mapping.SystemField.Item2))
            {
                return row[mapping.SystemField.Item2]?.ToString()?.Trim();
            }

            return null;
        }

        #endregion
    }
}
