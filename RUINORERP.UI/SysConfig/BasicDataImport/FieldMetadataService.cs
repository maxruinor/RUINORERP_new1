using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SqlSugar;
using RUINORERP.Model;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 字段元信息服务
    /// 负责从实体类型动态提取和缓存字段元信息
    /// </summary>
    public class FieldMetadataService
    {
        /// <summary>
        /// 字段元信息缓存（键为实体类型FullName，值为字段名到元信息的字典）
        /// </summary>
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, FieldMetadata>> _metadataCache = 
            new ConcurrentDictionary<string, ConcurrentDictionary<string, FieldMetadata>>();

        /// <summary>
        /// 获取指定实体类型的字段元信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>字段名到元信息的字典</returns>
        public static ConcurrentDictionary<string, FieldMetadata> GetFieldMetadata<TEntity>()
        {
            return GetFieldMetadata(typeof(TEntity));
        }

        /// <summary>
        /// 获取指定实体类型的字段元信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>字段名到元信息的字典</returns>
        public static ConcurrentDictionary<string, FieldMetadata> GetFieldMetadata(Type entityType)
        {
            string cacheKey = entityType.FullName;

            if (_metadataCache.TryGetValue(cacheKey, out var cachedMetadata))
            {
                return cachedMetadata;
            }

            // 提取字段元信息
            var metadata = ExtractFieldMetadata(entityType);

            // 缓存结果
            _metadataCache.TryAdd(cacheKey, metadata);

            return metadata;
        }

        /// <summary>
        /// 从实体类型提取字段元信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>字段名到元信息的字典</returns>
        private static ConcurrentDictionary<string, FieldMetadata> ExtractFieldMetadata(Type entityType)
        {
            var metadata = new ConcurrentDictionary<string, FieldMetadata>();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var fieldMeta = ExtractFieldMetadataFromProperty(property);
                if (fieldMeta != null)
                {
                    metadata.TryAdd(fieldMeta.FieldName, fieldMeta);
                }
            }

            return metadata;
        }

        /// <summary>
        /// 从属性提取字段元信息
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>字段元信息</returns>
        private static FieldMetadata ExtractFieldMetadataFromProperty(PropertyInfo property)
        {
            // 获取SugarColumn特性
            var sugarColumn = property.GetCustomAttribute<SugarColumn>();
            
            // 如果没有SugarColumn特性，则跳过（不映射到数据库）
            if (sugarColumn == null)
            {
                return null;
            }

            // 判断是否为导航属性（复杂类型）
            if (IsComplexType(property.PropertyType))
            {
                return null;
            }

            var fieldMeta = new FieldMetadata
            {
                FieldName = sugarColumn.ColumnName ?? property.Name,
                DisplayName = !string.IsNullOrEmpty(sugarColumn.ColumnDescription) 
                    ? sugarColumn.ColumnDescription 
                    : property.Name,
                DataType = GetUnderlyingType(property.PropertyType),
                DataTypeName = GetUnderlyingType(property.PropertyType).FullName,
                IsNullable = sugarColumn.IsNullable,
                IsPrimaryKey = sugarColumn.IsPrimaryKey,
                IsIdentity = sugarColumn.IsIdentity,
                DefaultValue = sugarColumn.DefaultValue,
                Description = sugarColumn.ColumnDescription,
                SugarColumn = sugarColumn  // 直接保存SugarColumn对象
            };

            // 设置最大长度
            if (fieldMeta.DataType == typeof(string) && sugarColumn.Length > 0)
            {
                fieldMeta.MaxLength = sugarColumn.Length;
            }

            // 判断是否为外键（根据字段名约定：以ID结尾且非主键）
            if (fieldMeta.FieldName.EndsWith("ID") && !fieldMeta.IsPrimaryKey)
            {
                fieldMeta.IsForeignKey = true;
                fieldMeta.ForeignTable = ExtractForeignTableName(fieldMeta.FieldName);
            }

            // 判断是否必填
            fieldMeta.IsRequired = !fieldMeta.IsNullable && !fieldMeta.IsIdentity;

            return fieldMeta;
        }

        /// <summary>
        /// 获取基础类型（去除可空类型包装）
        /// </summary>
        /// <param name="type">原始类型</param>
        /// <returns>基础类型</returns>
        private static Type GetUnderlyingType(Type type)
        {
            if (type == null) return typeof(string);

            // 处理可空类型
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(type);
            }

            // 处理枚举类型
            if (type.IsEnum)
            {
                return typeof(int);
            }

            return type;
        }

        /// <summary>
        /// 判断是否为复杂类型（需要跳过的类型）
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否为复杂类型</returns>
        private static bool IsComplexType(Type type)
        {
            // 基础类型
            if (type.IsValueType || type == typeof(string))
            {
                return false;
            }

            // 可空的基础类型
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return underlyingType != null && underlyingType.IsValueType;
            }

            // 其他类型视为复杂类型
            return true;
        }

        /// <summary>
        /// 从外键字段名提取关联表名
        /// </summary>
        /// <param name="fieldName">外键字段名</param>
        /// <returns>关联表名</returns>
        private static string ExtractForeignTableName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName) || !fieldName.EndsWith("ID"))
            {
                return string.Empty;
            }

            // 去除ID后缀，得到表名
            var tableName = fieldName.Substring(0, fieldName.Length - 2);
            
            // 可选：转换为复数形式或其他规则
            return tableName;
        }

        /// <summary>
        /// 根据字段名获取字段元信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="fieldName">字段名</param>
        /// <returns>字段元信息，如果不存在则返回null</returns>
        public static FieldMetadata GetFieldMetadata<TEntity>(string fieldName)
        {
            var metadata = GetFieldMetadata<TEntity>();
            if (metadata.TryGetValue(fieldName, out var fieldMeta))
            {
                return fieldMeta;
            }
            return null;
        }

        /// <summary>
        /// 根据字段中文名获取字段元信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="displayName">字段中文名</param>
        /// <returns>字段元信息，如果不存在则返回null</returns>
        public static FieldMetadata GetFieldMetadataByDisplayName<TEntity>(string displayName)
        {
            var metadata = GetFieldMetadata<TEntity>();
            return metadata.Values.FirstOrDefault(m => m.DisplayName == displayName);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCache()
        {
            _metadataCache.Clear();
        }

        /// <summary>
        /// 清除指定类型的缓存
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        public static void ClearCache<TEntity>()
        {
            string cacheKey = typeof(TEntity).FullName;
            if (_metadataCache.TryGetValue(cacheKey, out var metadata))
            {
                metadata.Clear();
            }
        }

        /// <summary>
        /// 获取所有导入支持的实体类型
        /// </summary>
        /// <returns>实体类型列表</returns>
        public static List<Type> GetSupportedImportTypes()
        {
            return new List<Type>
            {
                typeof(tb_Prod),
                typeof(tb_ProdCategories),
                typeof(tb_CustomerVendor),
                typeof(tb_ProdDetail),
                // 可以继续添加其他支持的类型
            };
        }

        /// <summary>
        /// 将字段元信息转换为键值对字典（用于显示）
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>字段名到中文名的字典</returns>
        public static ConcurrentDictionary<string, string> GetFieldNameDisplayDictionary<TEntity>()
        {
            var metadata = GetFieldMetadata<TEntity>();
            var dict = new ConcurrentDictionary<string, string>();
            
            foreach (var kvp in metadata)
            {
                dict.TryAdd(kvp.Key, kvp.Value.DisplayName);
            }
            
            return dict;
        }

        /// <summary>
        /// 将字段元信息转换为键值对列表（用于配置界面）
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>键值对列表</returns>
        public static List<KeyValuePair<string, FieldMetadata>> GetMetadataList<TEntity>()
        {
            var metadata = GetFieldMetadata<TEntity>();
            return metadata.OrderBy(m => m.Value.DisplayName).ToList();
        }
    }
}
