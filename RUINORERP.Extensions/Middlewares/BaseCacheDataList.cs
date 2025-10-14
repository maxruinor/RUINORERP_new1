using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using RUINORERP.Common;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Extensions.Middlewares
{
    /// <summary>
    /// 表结构信息类，用于存储表的元数据信息
    /// </summary>
    public class TableSchemaInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键字段名
        /// </summary>
        public string PrimaryKeyField { get; set; }

        /// <summary>
        /// 显示名称字段名
        /// </summary>
        public string DisplayField { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 是否是视图
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// 是否需要缓存
        /// </summary>
        public bool IsCacheable { get; set; } = true;

        /// <summary>
        /// 表描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 外键关系列表
        /// </summary>
        public List<ForeignKeyRelation> ForeignKeys { get; set; } = new List<ForeignKeyRelation>();
        
        /// <summary>
        /// 验证表结构信息是否完整
        /// </summary>
        /// <returns>验证结果</returns>
        public bool Validate()
        {
            return !string.IsNullOrEmpty(TableName) &&
                   !string.IsNullOrEmpty(PrimaryKeyField) &&
                   !string.IsNullOrEmpty(DisplayField) &&
                   EntityType != null;
        }
        
        /// <summary>
        /// 获取表结构信息的字符串表示
        /// </summary>
        /// <returns>表结构信息字符串</returns>
        public string ToInfoString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"表名: {TableName}");
            sb.AppendLine($"主键字段: {PrimaryKeyField}");
            sb.AppendLine($"显示字段: {DisplayField}");
            sb.AppendLine($"实体类型: {EntityType?.Name ?? "未指定"}");
            sb.AppendLine($"是否视图: {IsView}");
            sb.AppendLine($"是否缓存: {IsCacheable}");
            sb.AppendLine($"描述: {Description ?? "无"}");
            sb.AppendLine($"外键数量: {ForeignKeys?.Count ?? 0}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 外键关系类
    /// </summary>
    public class ForeignKeyRelation
    {
        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKeyField { get; set; }

        /// <summary>
        /// 关联的表名
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 关联表的主键字段
        /// </summary>
        public string RelatedPrimaryKey { get; set; }
    }

    /// <summary>
    /// 缓存数据列表管理器，统一管理所有需要缓存的表结构信息
    /// </summary>
    public class BaseCacheDataList
    {
        /// <summary>
        /// 表结构信息字典，键为表名
        /// </summary>
        private readonly ConcurrentDictionary<string, TableSchemaInfo> _tableSchemas = 
            new ConcurrentDictionary<string, TableSchemaInfo>();

        /// <summary>
        /// 实体类型到表名的映射字典
        /// </summary>
        private readonly ConcurrentDictionary<Type, string> _typeToTableName = 
            new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// 获取所有注册的表结构信息
        /// </summary>
        public IReadOnlyDictionary<string, TableSchemaInfo> TableSchemas => _tableSchemas;

        /// <summary>
        /// 获取所有需要缓存的表名
        /// </summary>
        public IEnumerable<string> CacheableTableNames => 
            _tableSchemas.Values.Where(t => t.IsCacheable).Select(t => t.TableName);

        /// <summary>
        /// 注册表结构信息（泛型方法）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        public void RegistInformation<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null) where T : class
        {
            var entityType = typeof(T);
            var tableName = entityType.Name;

            // 获取主键字段名
            var primaryKeyField = GetMemberName(primaryKeyExpression);
            if (string.IsNullOrEmpty(primaryKeyField))
            {
                throw new ArgumentException($"无法从表达式中提取主键字段名: {primaryKeyExpression}");
            }

            // 获取显示字段名
            var displayField = GetMemberName(displayFieldExpression);
            if (string.IsNullOrEmpty(displayField))
            {
                throw new ArgumentException($"无法从表达式中提取显示字段名: {displayFieldExpression}");
            }

            var schemaInfo = new TableSchemaInfo
            {
                TableName = tableName,
                PrimaryKeyField = primaryKeyField,
                DisplayField = displayField,
                EntityType = entityType,
                IsView = isView,
                IsCacheable = isCacheable,
                Description = description ?? tableName
            };

            // 加载外键关系
            LoadForeignKeyRelations(schemaInfo);

            // 验证表结构信息
            if (!schemaInfo.Validate())
            {
                throw new InvalidOperationException($"表结构信息验证失败: {schemaInfo.ToInfoString()}");
            }

            // 添加到字典
            _tableSchemas.TryAdd(tableName, schemaInfo);
            _typeToTableName.TryAdd(entityType, tableName);
        }

        /// <summary>
        /// 注册表结构信息（非泛型方法）
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="primaryKeyField">主键字段名</param>
        /// <param name="displayField">显示字段名</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        public void RegistInformation(
            Type entityType,
            string primaryKeyField,
            string displayField,
            bool isView = false,
            bool isCacheable = true,
            string description = null)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            if (string.IsNullOrEmpty(primaryKeyField))
            {
                throw new ArgumentException("主键字段名不能为空", nameof(primaryKeyField));
            }

            if (string.IsNullOrEmpty(displayField))
            {
                throw new ArgumentException("显示字段名不能为空", nameof(displayField));
            }

            var tableName = entityType.Name;

            var schemaInfo = new TableSchemaInfo
            {
                TableName = tableName,
                PrimaryKeyField = primaryKeyField,
                DisplayField = displayField,
                EntityType = entityType,
                IsView = isView,
                IsCacheable = isCacheable,
                Description = description ?? tableName
            };

            // 加载外键关系
            LoadForeignKeyRelations(schemaInfo);

            // 验证表结构信息
            if (!schemaInfo.Validate())
            {
                throw new InvalidOperationException($"表结构信息验证失败: {schemaInfo.ToInfoString()}");
            }

            // 添加到字典
            _tableSchemas.TryAdd(tableName, schemaInfo);
            _typeToTableName.TryAdd(entityType, tableName);
        }

        /// <summary>
        /// 根据表名获取表结构信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表结构信息，如果不存在返回null</returns>
        public TableSchemaInfo GetSchemaInfo(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            _tableSchemas.TryGetValue(tableName, out var schemaInfo);
            return schemaInfo;
        }

        /// <summary>
        /// 根据实体类型获取表结构信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>表结构信息，如果不存在返回null</returns>
        public TableSchemaInfo GetSchemaInfo(Type entityType)
        {
            if (entityType == null)
            {
                return null;
            }

            _typeToTableName.TryGetValue(entityType, out var tableName);
            return string.IsNullOrEmpty(tableName) ? null : GetSchemaInfo(tableName);
        }

        /// <summary>
        /// 根据表名获取实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体类型，如果不存在返回null</returns>
        public Type GetEntityType(string tableName)
        {
            var schemaInfo = GetSchemaInfo(tableName);
            return schemaInfo?.EntityType;
        }

        /// <summary>
        /// 检查表是否已注册
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否已注册</returns>
        public bool ContainsTable(string tableName)
        {
            return !string.IsNullOrEmpty(tableName) && _tableSchemas.ContainsKey(tableName);
        }

        /// <summary>
        /// 检查实体类型是否已注册
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>是否已注册</returns>
        public bool ContainsType(Type entityType)
        {
            return entityType != null && _typeToTableName.ContainsKey(entityType);
        }

        /// <summary>
        /// 从表达式中提取成员名称
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns>成员名称</returns>
        private string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            // 处理Lambda表达式
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            // 处理转换表达式（如：int -> object）
            if (expression.Body is UnaryExpression unaryExpression && 
                unaryExpression.Operand is MemberExpression operandMemberExpression)
            {
                return operandMemberExpression.Member.Name;
            }

            return null;
        }

        /// <summary>
        /// 加载外键关系
        /// </summary>
        /// <param name="schemaInfo">表结构信息</param>
        private void LoadForeignKeyRelations(TableSchemaInfo schemaInfo)
        {
            if (schemaInfo?.EntityType == null)
            {
                return;
            }

            // 获取所有属性
            var properties = schemaInfo.EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // 查找外键关系特性
                var fkAttributes = property.GetCustomAttributes(typeof(FKRelationAttribute), false);
                if (fkAttributes.Length > 0)
                {
                    foreach (FKRelationAttribute fkAttr in fkAttributes)
                    {
                        var foreignKeyRelation = new ForeignKeyRelation
                        {
                            ForeignKeyField = property.Name,
                            RelatedTableName = fkAttr.FKTableName == "tb_ProdDetail" ? "View_ProdDetail" : fkAttr.FKTableName,
                            RelatedPrimaryKey = fkAttr.FK_IDColName
                        };

                        schemaInfo.ForeignKeys.Add(foreignKeyRelation);
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有已注册的表名
        /// </summary>
        /// <returns>表名列表</returns>
        public List<string> GetAllTableNames()
        {
            return _tableSchemas.Keys.ToList();
        }

        /// <summary>
        /// 获取所有需要缓存的表名
        /// </summary>
        /// <returns>需要缓存的表名列表</returns>
        public List<string> GetCacheableTableNamesList()
        {
            return _tableSchemas.Values
                .Where(t => t.IsCacheable)
                .Select(t => t.TableName)
                .ToList();
        }

        /// <summary>
        /// 清除所有注册的表结构信息
        /// </summary>
        public void Clear()
        {
            _tableSchemas.Clear();
            _typeToTableName.Clear();
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns>统计信息字符串</returns>
        public string GetStatistics()
        {
            var totalTables = _tableSchemas.Count;
            var cacheableTables = _tableSchemas.Values.Count(t => t.IsCacheable);
            var views = _tableSchemas.Values.Count(t => t.IsView);

            return $"总表数: {totalTables}, 可缓存表数: {cacheableTables}, 视图数: {views}";
        }
        
        /// <summary>
        /// 获取所有表结构信息的详细字符串表示
        /// </summary>
        /// <returns>所有表结构信息的字符串表示</returns>
        public string GetAllSchemaInfoString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== 所有表结构信息 ===");
            foreach (var kvp in _tableSchemas)
            {
                sb.AppendLine($"--- {kvp.Key} ---");
                sb.AppendLine(kvp.Value.ToInfoString());
            }
            return sb.ToString();
        }
    }
}