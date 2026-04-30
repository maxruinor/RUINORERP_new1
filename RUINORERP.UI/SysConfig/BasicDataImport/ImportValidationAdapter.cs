using RUINORERP.Business;
using RUINORERP.Model.Context;
using RUINORERP.Model.ImportEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 基础数据导入验证适配器
    /// 将现有的FluentValidation验证器体系适配到导入流程中
    /// </summary>
    public class ImportValidationAdapter
    {
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appContext">应用程序上下文</param>
        public ImportValidationAdapter(ApplicationContext appContext = null)
        {
            _appContext = appContext;
        }

        /// <summary>
        /// 验证列映射配置
        /// 在保存配置前进行基本验证
        /// </summary>
        /// <param name="mapping">列映射配置</param>
        /// <param name="errors">错误列表（输出）</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateColumnMapping(ColumnMapping mapping, out List<string> errors)
        {
            errors = new List<string>();

            if (mapping == null)
            {
                errors.Add("列映射配置不能为空");
                return false;
            }

            // 1. 验证系统字段不能为空
            if (string.IsNullOrEmpty(mapping.SystemField?.Key))
            {
                errors.Add("系统字段不能为空");
                return false;
            }

            // 2. 验证外键关联配置完整性
            if (mapping.DataSourceType == DataSourceType.ForeignKey)
            {
                var foreignConfig = mapping.DataSourceConfig as ForeignKeyConfig;
                if (foreignConfig == null)
                {
                    errors.Add($"字段【{mapping.SystemField.Value}】配置为外键关联，但未配置外关联信息");
                    return false;
                }

                if (string.IsNullOrEmpty(foreignConfig.ForeignKeyTable?.Key))
                {
                    errors.Add($"字段【{mapping.SystemField.Value}】的外键表未配置");
                    return false;
                }

                if (string.IsNullOrEmpty(foreignConfig.ForeignKeyField?.Key))
                {
                    errors.Add($"字段【{mapping.SystemField.Value}】的外键字段未配置");
                    return false;
                }

                // 验证外键来源列（如果配置了）
                if (foreignConfig.ForeignKeySourceColumn != null &&
                    !string.IsNullOrEmpty(foreignConfig.ForeignKeySourceColumn.Key))
                {
                    // 可选：检查Excel列是否存在
                }
            }

            // 3. 验证列拼接配置
            if (mapping.DataSourceType == DataSourceType.ColumnConcat)
            {
                var concatConfig = mapping.DataSourceConfig as ColumnConcatConfig;
                if (concatConfig == null || concatConfig.SourceColumns == null || 
                    concatConfig.SourceColumns.Count < 2)
                {
                    errors.Add($"字段【{mapping.SystemField.Value}】配置为列拼接，但至少需要指定2个源列");
                    return false;
                }
            }

            // 4. 验证枚举默认值配置（只有当明确配置了枚举值但未配置类型名称时才报错）
            var defaultConfig = mapping.DataSourceConfig as DefaultValueConfig;
            if (defaultConfig != null && !string.IsNullOrEmpty(defaultConfig.EnumName))
            {
                if (string.IsNullOrEmpty(defaultConfig.EnumTypeName))
                {
                    errors.Add($"字段【{mapping.SystemField.Value}】的枚举类型名称未配置");
                    return false;
                }
            }

            // 5. 验证图片配置
            if (mapping.DataSourceType == DataSourceType.ExcelImage)
            {
                var imageConfig = mapping.DataSourceConfig as ExcelImageConfig;
                if (imageConfig != null)
                {
                    // 验证图片命名规则
                    if (imageConfig.NamingRule == ImageNamingRule.ColumnValue &&
                        string.IsNullOrEmpty(imageConfig.NamingReferenceColumn))
                    {
                        errors.Add($"字段【{mapping.SystemField.Value}】使用列值命名，但未指定参考列");
                        return false;
                    }
                }
            }

            // 6. 验证业务键配置
            if (mapping.IsBusinessKey)
            {
                // 业务键字段通常应该是必填的
                // 检查是否为必填：Excel数据源且未配置忽略空值
                bool isRequiredField = mapping.DataSourceType == DataSourceType.Excel &&
                                       !(mapping.DataSourceConfig is ExcelConfig excelCfg && excelCfg.IgnoreEmptyValue);
                
                if (!isRequiredField)
                {
                    // 警告但不阻止：业务键建议设置为必填
                    System.Diagnostics.Debug.WriteLine(
                        $"警告：字段【{mapping.SystemField.Value}】标记为业务键，但未设置为必填");
                }
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// 验证整个导入配置
        /// </summary>
        /// <param name="config">导入配置</param>
        /// <param name="errors">错误列表（输出）</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateImportConfiguration(ImportConfiguration config, out List<string> errors)
        {
            errors = new List<string>();

            if (config == null)
            {
                errors.Add("导入配置不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(config.MappingName))
            {
                errors.Add("配置名称不能为空");
                return false;
            }

            if (config.ColumnMappings == null || config.ColumnMappings.Count == 0)
            {
                errors.Add("至少需要配置一个列映射");
                return false;
            }

            // 验证每个列映射
            foreach (var mapping in config.ColumnMappings)
            {
                if (!ValidateColumnMapping(mapping, out List<string> mappingErrors))
                {
                    errors.AddRange(mappingErrors);
                }
            }

            // 验证去重配置
            if (config.EnableDeduplication)
            {
                var dedupFields = config.GetEffectiveDeduplicateFields();
                if (dedupFields == null || dedupFields.Count == 0)
                {
                    errors.Add("启用了去重功能，但未配置去重字段");
                }
            }

            // 验证业务键配置
            var businessKeyMappings = config.ColumnMappings.Where(m => m.IsBusinessKey).ToList();
            if (businessKeyMappings.Count > 0)
            {
                // 检查所有业务键是否都配置了存在性策略
                foreach (var bkMapping in businessKeyMappings)
                {
                    if (bkMapping.ExistenceStrategy == ExistenceStrategy.Error)
                    {
                        // Error策略需要特别注意，确保用户理解其含义
                        System.Diagnostics.Debug.WriteLine(
                            $"提示：字段【{bkMapping.SystemField.Value}】使用报错策略，遇到重复记录将中止导入");
                    }
                }
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// 使用实体验证器验证单个实体对象
        /// 复用现有的 FluentValidation 验证器体系
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>验证错误消息，空字符串表示验证通过</returns>
        public string ValidateEntity<T>(T entity) where T : class
        {
            if (entity == null)
            {
                return "实体对象不能为空";
            }

            try
            {
                Type entityType = typeof(T);
                
                // 构造验证器类型名: 实体名 + Validator
                string validatorName = $"{entityType.Name}Validator";
                
                // 在 RUINORERP.Business.Validator 命名空间下查找验证器
                Type validatorType = Type.GetType($"RUINORERP.Business.Validator.{validatorName}");
                
                if (validatorType == null)
                {
                    // 没有对应的验证器，跳过验证
                    return string.Empty;
                }

                // 创建验证器实例
                var validatorInstance = Activator.CreateInstance(validatorType, new object[] { _appContext });
                
                // 获取验证器的 Validate 方法
                var validateMethod = validatorType.GetMethod("Validate",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                if (validateMethod == null)
                {
                    return string.Empty;
                }

                // 调用验证方法
                var validationResult = validateMethod.Invoke(validatorInstance, new[] { entity });
                
                if (validationResult == null)
                {
                    return string.Empty;
                }

                // 处理验证结果
                Type validationResultType = validationResult.GetType();
                var errorsProperty = validationResultType.GetProperty("Errors");
                
                if (errorsProperty == null)
                {
                    return string.Empty;
                }

                var errors = errorsProperty.GetValue(validationResult);
                if (errors == null)
                {
                    return string.Empty;
                }

                Type errorsType = errors.GetType();
                var countProperty = errorsType.GetProperty("Count");
                
                if (countProperty == null)
                {
                    return string.Empty;
                }

                int errorCount = (int)countProperty.GetValue(errors);
                if (errorCount == 0)
                {
                    return string.Empty;
                }

                // 收集所有错误消息
                var errorMessages = new List<string>();
                var indexerMethod = errorsType.GetMethod("get_Item");
                
                if (indexerMethod != null)
                {
                    for (int i = 0; i < errorCount; i++)
                    {
                        var error = indexerMethod.Invoke(errors, new object[] { i });
                        var errorMessageProperty = error.GetType().GetProperty("ErrorMessage");
                        
                        if (errorMessageProperty != null)
                        {
                            string errorMessage = errorMessageProperty.GetValue(error)?.ToString();
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessages.Add(errorMessage);
                            }
                        }
                    }
                }

                return string.Join("; ", errorMessages);
            }
            catch (Exception ex)
            {
                // 验证过程出错，记录日志但不阻止导入
                System.Diagnostics.Debug.WriteLine($"实体验证失败: {ex.Message}");
                return $"验证过程出错: {ex.Message}";
            }
        }
    }
}
