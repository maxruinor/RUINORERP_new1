using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 表结构信息管理器，统一管理所有表的元数据信息
    /// </summary>
    public class TableSchemaManager
    {
        #region 单例模式
        private static TableSchemaManager _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// 获取TableSchemaManager的单例实例
        /// </summary>
        public static TableSchemaManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TableSchemaManager();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 数据存储
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
        /// 表名到实体类型的映射字典
        /// </summary>
        private readonly ConcurrentDictionary<string, Type> _tableNameToType =
            new ConcurrentDictionary<string, Type>();
        #endregion

        #region 公共属性
        /// <summary>
        /// 获取所有注册的表结构信息
        /// </summary>
        public IReadOnlyDictionary<string, TableSchemaInfo> TableSchemas => _tableSchemas;

        /// <summary>
        /// 获取所有需要缓存的表名
        /// </summary>
        public IEnumerable<string> CacheableTableNames =>
            _tableSchemas.Values.Where(t => t.IsCacheable).Select(t => t.TableName);
        #endregion

        #region 注册方法
        /// <summary>
        /// 注册表结构信息（泛型方法）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">主显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
        /// <param name="otherDisplayFieldExpressions">其他需要缓存的显示字段表达式</param>
        public void RegisterTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params Expression<Func<T, object>>[] otherDisplayFieldExpressions) where T : class
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

            // 初始化显示字段列表
            var displayFields = new List<string> { displayField };
            
            // 添加其他显示字段
            if (otherDisplayFieldExpressions != null && otherDisplayFieldExpressions.Length > 0)
            {
                foreach (var expr in otherDisplayFieldExpressions)
                {
                    var fieldName = GetMemberName(expr);
                    if (!string.IsNullOrEmpty(fieldName) && !displayFields.Contains(fieldName))
                    {
                        displayFields.Add(fieldName);
                    }
                }
            }

            var schemaInfo = new TableSchemaInfo
            {
                TableName = tableName,
                PrimaryKeyField = primaryKeyField,
                DisplayField = displayField,
                DisplayFields = displayFields,
                EntityType = entityType,
                IsView = isView,
                IsCacheable = isCacheable,
                CacheWholeRow = cacheWholeRow,
                Description = description ?? tableName,
                Properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
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
            _tableNameToType.TryAdd(tableName, entityType);
        }

        /// <summary>
        /// 注册表结构信息（非泛型方法）
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="primaryKeyField">主键字段名</param>
        /// <param name="displayField">主显示字段名</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        /// <param name="otherDisplayFields">其他需要缓存的显示字段名</param>
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
        public void RegisterTableSchema(
            Type entityType,
            string primaryKeyField,
            string displayField,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params string[] otherDisplayFields)
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

            // 初始化显示字段列表
            var displayFields = new List<string> { displayField };
            
            // 添加其他显示字段
            if (otherDisplayFields != null && otherDisplayFields.Length > 0)
            {
                foreach (var fieldName in otherDisplayFields)
                {
                    if (!string.IsNullOrEmpty(fieldName) && !displayFields.Contains(fieldName))
                    {
                        displayFields.Add(fieldName);
                    }
                }
            }

            var tableName = entityType.Name;

            var schemaInfo = new TableSchemaInfo
            {
                TableName = tableName,
                PrimaryKeyField = primaryKeyField,
                DisplayField = displayField,
                DisplayFields = displayFields,
                EntityType = entityType,
                IsView = isView,
                IsCacheable = isCacheable,
                CacheWholeRow = cacheWholeRow,
                Description = description ?? tableName,
                Properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
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
            _tableNameToType.TryAdd(tableName, entityType);
        }
        #endregion

        #region 查询方法
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
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            _tableNameToType.TryGetValue(tableName, out var entityType);
            return entityType;
        }

        /// <summary>
        /// 获取主键字段名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主键字段名</returns>
        public string GetPrimaryKeyField(string tableName)
        {
            var schemaInfo = GetSchemaInfo(tableName);
            return schemaInfo?.PrimaryKeyField;
        }

        /// <summary>
        /// 获取主显示字段名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主显示字段名</returns>
        public string GetDisplayField(string tableName)
        {
            var schemaInfo = GetSchemaInfo(tableName);
            return schemaInfo?.DisplayField;
        }

        /// <summary>
        /// 获取所有显示字段名列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>显示字段名列表</returns>
        public List<string> GetDisplayFields(string tableName)
        {
            var schemaInfo = GetSchemaInfo(tableName);
            return schemaInfo?.DisplayFields ?? new List<string>();
        }

        /// <summary>
        /// 获取外键关系列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>外键关系列表</returns>
        public List<ForeignKeyRelation> GetForeignKeys(string tableName)
        {
            var schemaInfo = GetSchemaInfo(tableName);
            return schemaInfo?.ForeignKeys ?? new List<ForeignKeyRelation>();
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
        #endregion

        #region 辅助方法
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
                            RelatedPrimaryKey = fkAttr.FK_IDColName,
                            ForeignKeyProperty = property
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
            _tableNameToType.Clear();
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

        public IEnumerable<TableSchemaInfo> GetAllSchemaInfo()
        {
            return _tableSchemas.Values.ToList();
        }
        #endregion
    }
}