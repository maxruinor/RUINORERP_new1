using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Model;
using RUINORERP.Global.CustomAttribute;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.ComponentModel;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 智能规则配置助手类
    /// 提供辅助方法来支持行级权限规则的智能配置
    /// </summary>
    public class SmartRuleConfigHelper
    {
        private readonly IBusinessEntityMappingService _entityInfoService;
        private readonly ILogger<SmartRuleConfigHelper> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityInfoService">实体信息服务</param>
        /// <param name="loggerFactory">日志工厂</param>
        public SmartRuleConfigHelper(IBusinessEntityMappingService entityInfoService, ILoggerFactory loggerFactory)
        {
            _entityInfoService = entityInfoService;
            _logger = loggerFactory.CreateLogger<SmartRuleConfigHelper>();
        }

        /// <summary>
        /// 获取实体的所有字段信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>字段信息列表</returns>
        public List<EntityFieldInfo> GetEntityFields(Type entityType)
        {
            try
            {
                if (entityType == null)
                {
                    return new List<EntityFieldInfo>();
                }

                var entityInfo = _entityInfoService.GetEntityInfo(entityType);
                if (entityInfo != null && entityInfo.Fields != null)
                {
                    return entityInfo.Fields.Values.ToList();
                }

                // 如果EntityInfoService中没有找到，通过反射获取
                var fields = new List<EntityFieldInfo>();
                var properties = entityType.GetProperties();

                foreach (var property in properties)
                {
                    // 跳过导航属性（通常是ICollection<T>）
                    if (property.PropertyType.IsGenericType && 
                        property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        continue;
                    }

                    var fieldInfo = new EntityFieldInfo
                    {
                        FieldName = property.Name,
                        FieldType = property.PropertyType,
                        IsNullable = !property.PropertyType.IsValueType || 
                                    (property.PropertyType.IsGenericType && 
                                    property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    };

                    // 尝试获取字段描述
                    var descAttr = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (descAttr != null)
                    {
                        fieldInfo.Description = descAttr.Description;
                    }

                    // 检查是否为主键
                    var sugarColumnAttr = Attribute.GetCustomAttribute(property, typeof(SugarColumn)) as SugarColumn;
                    if (sugarColumnAttr != null && sugarColumnAttr.IsPrimaryKey)
                    {
                        fieldInfo.IsPrimaryKey = true;
                    }

                    fields.Add(fieldInfo);
                }

                return fields;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取实体字段信息失败: {EntityType}", entityType?.FullName);
                return new List<EntityFieldInfo>();
            }
        }

        /// <summary>
        /// 获取实体的所有外键字段信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>外键字段信息列表</returns>
        public List<ForeignKeyInfo> GetForeignKeyFields(Type entityType)
        {
            try
            {
                if (entityType == null)
                {
                    return new List<ForeignKeyInfo>();
                }

                var foreignKeys = new List<ForeignKeyInfo>();
                var properties = entityType.GetProperties();

                foreach (var property in properties)
                {
                    // 查找FKRelationAttribute特性
                        var fkAttr = Attribute.GetCustomAttribute(property, typeof(FKRelationAttribute)) as FKRelationAttribute;
                        if (fkAttr != null)
                        {
                            // 查找关联实体类型
                            Type relatedEntityType = null;
                            try
                            {
                                // 尝试通过表名查找实体类型
                                relatedEntityType = _entityInfoService.GetEntityTypeByTableName(fkAttr.FKTableName);

                                // 如果没找到，尝试通过命名约定查找
                                if (relatedEntityType == null && !string.IsNullOrEmpty(fkAttr.FKTableName))
                                {
                                    // 简单的命名约定：表名去掉tb_前缀，首字母大写
                                    string entityName = fkAttr.FKTableName;
                                    if (entityName.StartsWith("tb_"))
                                    {
                                        entityName = entityName.Substring(3);
                                    }

                                    // 在RUINORERP.Model命名空间下查找
                                    relatedEntityType = Type.GetType($"RUINORERP.Model.{entityName}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "获取关联实体类型失败: {FKTableName}", fkAttr.FKTableName);
                            }

                            foreignKeys.Add(new ForeignKeyInfo
                            {
                                FieldName = property.Name,
                                FieldType = property.PropertyType,
                                ForeignTableName = fkAttr.FKTableName,
                                RelatedEntityType = relatedEntityType
                            });
                        }
                }

                return foreignKeys;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取外键字段信息失败: {EntityType}", entityType?.FullName);
                return new List<ForeignKeyInfo>();
            }
        }

        /// <summary>
        /// 生成默认的过滤条件
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="userId">用户ID</param>
        /// <returns>默认过滤条件</returns>
        public string GenerateDefaultFilterClause(Type entityType, long userId)
        {
            try
            {
                if (entityType == null)
                {
                    return string.Empty;
                }

                // 获取实体信息
                var entityInfo = _entityInfoService.GetEntityInfo(entityType);
                if (entityInfo == null)
                {
                    return string.Empty;
                }

                // 查找常见的用户相关字段
                var fields = GetEntityFields(entityType);
                var userField = fields.FirstOrDefault(f => 
                    f.FieldName.Equals("CreateBy", StringComparison.OrdinalIgnoreCase) ||
                    f.FieldName.Equals("CreatedBy", StringComparison.OrdinalIgnoreCase) ||
                    f.FieldName.Equals("CreateUserID", StringComparison.OrdinalIgnoreCase) ||
                    f.FieldName.Equals("CreatedUserID", StringComparison.OrdinalIgnoreCase) ||
                    f.FieldName.Equals("OperatorID", StringComparison.OrdinalIgnoreCase) ||
                    f.FieldName.Equals("Employee_ID", StringComparison.OrdinalIgnoreCase) ||
                    f.FieldName.Contains("User") && f.FieldType == typeof(long) ||
                    f.FieldName.Contains("Emp") && f.FieldType == typeof(long)
                );

                if (userField != null)
                {
                    return $"[{userField.FieldName}] = {userId}";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成默认过滤条件失败: {EntityType}, UserId: {UserId}", entityType?.FullName, userId);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取常用的操作符列表
        /// </summary>
        /// <returns>操作符列表</returns>
        public List<string> GetCommonOperators()
        {
            return new List<string>
            {
                "=", "<>", ">", "<", ">=", "<=", "LIKE", "IN"
            };
        }

        /// <summary>
        /// 验证SQL条件表达式
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <param name="errorMessage">错误信息（如果验证失败）</param>
        /// <returns>是否有效</returns>
        public bool ValidateFilterClause(string filterClause, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(filterClause))
                {
                    return true; // 空条件视为有效
                }

                // 简单的SQL注入检查
                var invalidKeywords = new[] { "DROP", "DELETE", "TRUNCATE", "INSERT", "UPDATE", "EXEC", "ALTER", "CREATE" };
                foreach (var keyword in invalidKeywords)
                {
                    if (filterClause.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        errorMessage = $"条件中包含不允许的SQL关键字: {keyword}";
                        return false;
                    }
                }

                // 检查括号是否匹配
                int openBracketCount = filterClause.Count(c => c == '(');
                int closeBracketCount = filterClause.Count(c => c == ')');
                if (openBracketCount != closeBracketCount)
                {
                    errorMessage = "条件中的括号不匹配";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "验证条件时发生错误: " + ex.Message;
                _logger.LogError(ex, "验证过滤条件失败: {FilterClause}", filterClause);
                return false;
            }
        }

        /// <summary>
        /// 获取建议的规则名称
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>建议的规则名称</returns>
        public string GetSuggestedPolicyName(Type entityType, string filterClause)
        {
            try
            {
                if (entityType == null)
                {
                    return "自定义数据权限规则";
                }

                string entityName = entityType.Name;
                // 去掉tb_前缀
                if (entityName.StartsWith("tb_"))
                {
                    entityName = entityName.Substring(3);
                }

                string baseName = $"{entityName}数据权限";

                // 基于过滤条件添加更具体的描述
                if (!string.IsNullOrEmpty(filterClause))
                {
                    if (filterClause.IndexOf("=", StringComparison.Ordinal) >= 0 && 
                        filterClause.IndexOf("User", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return $"{baseName}(个人数据)";
                    }
                    else if (filterClause.IndexOf("IN", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return $"{baseName}(多选限制)";
                    }
                    else if (filterClause.IndexOf("LIKE", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return $"{baseName}(模糊匹配)";
                    }
                }

                return baseName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成建议的规则名称失败: {EntityType}", entityType?.FullName);
                return "自定义数据权限规则";
            }
        }
    }

    /// <summary>
    /// 外键信息类
    /// </summary>
    public class ForeignKeyInfo
    {
        /// <summary>
        /// 外键字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 外键字段类型
        /// </summary>
        public Type FieldType { get; set; }

        /// <summary>
        /// 关联表名
        /// </summary>
        public string ForeignTableName { get; set; }

        /// <summary>
        /// 关联实体类型
        /// </summary>
        public Type RelatedEntityType { get; set; }

        /// <summary>
        /// 显示名称（用于UI）
        /// </summary>
        public string DisplayName => !string.IsNullOrEmpty(ForeignTableName) ? 
            $"{FieldName} (关联 {ForeignTableName})" : FieldName;
    }
}