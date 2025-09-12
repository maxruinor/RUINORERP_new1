using Newtonsoft.Json;
using RUINORERP.Model.CommonModel;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体数据内容提取器
    /// 要优化。提高性能！！！
    /// </summary>
    public static class EntityDataExtractor
    {
        // 缓存类型信息以提高性能
        private static readonly ConcurrentDictionary<Type, TypeData> TypeCache = new ConcurrentDictionary<Type, TypeData>();

        // 子表命名模式（主表名+Detail）
        private static readonly Regex DetailPattern = new Regex(@"^(.*)Detail$", RegexOptions.Compiled);

        /// <summary>
        /// 提取实体的重点数据内容
        /// </summary>
        public static string ExtractDataContent(object entity, bool ignoreNulls = true, bool includeChildTables = true)
        {
            if (entity == null) return string.Empty;

            try
            {
                var entityData = ExtractEntityData(entity, ignoreNulls, includeChildTables);
                return JsonConvert.SerializeObject(entityData, Formatting.Indented);
            }
            catch (Exception ex)
            {
                // 记录错误但不影响主流程
                Console.WriteLine($"生成数据内容失败: {ex.Message}");
                return string.Empty;
            }
        }

        private static Dictionary<string, object> ExtractEntityData(object entity, bool ignoreNulls, bool includeChildTables)
        {
            var entityType = entity.GetType();
            var typeData = GetTypeData(entityType);
            var result = new Dictionary<string, object>();

            // 处理主表属性
            foreach (var property in typeData.AuditedProperties)
            {
                var value = property.Info.GetValue(entity);

                // 忽略空值
                if (ignoreNulls && IsNullOrEmpty(value))
                    continue;

                // 处理子表集合
                if (includeChildTables && property.IsChildTable)
                {
                    var collection = value as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        var childItems = new List<Dictionary<string, object>>();
                        foreach (var item in collection)
                        {
                            if (item != null)
                            {
                                childItems.Add(ExtractEntityData(item, ignoreNulls, false));
                            }
                        }
                        result[property.DisplayName] = childItems;
                    }
                }
                else
                {
                    // 处理普通属性
                    result[property.DisplayName] = value;
                }
            }

            return result;
        }

        private static TypeData GetTypeData(Type type)
        {
            // 从缓存中获取类型数据
            if (TypeCache.TryGetValue(type, out var typeData))
                return typeData;

            // 初始化类型数据
            typeData = new TypeData
            {
                Type = type,
                AuditedProperties = new List<PropertyData>()
            };

            // 获取所有公共实例属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string mainTableName = string.Empty;
            // 分析当前类型是否为Detail子表
            var isDetailTable = DetailPattern.IsMatch(type.Name);
            if (isDetailTable)
            {

                mainTableName = isDetailTable ? DetailPattern.Match(type.Name).Groups[1].Value : null;
            }
            else
            {
                //mainTableName = type.Name.Substring(0, type.Name.Length - "Detail".Length);
                mainTableName = type.Name;
            }

            int counterTest = 0;

            foreach (var property in properties)
            {
                counterTest++;
                if (counterTest == 44)
                {

                }
                // 跳过忽略的属性
                if (ShouldSkipProperty(property, mainTableName))
                    continue;

                var propertyData = new PropertyData
                {
                    Info = property,
                    DisplayName = GetPropertyDisplayName(property),
                    IsChildTable = IsChildTableProperty(property, mainTableName)
                };

                typeData.AuditedProperties.Add(propertyData);
            }

            // 缓存类型数据
            TypeCache[type] = typeData;
            return typeData;
        }

        private static bool ShouldSkipProperty(PropertyInfo property, string mainTableName)
        {
            //如果是子表 则要记录
            if (property.Name.Contains("Detail") && property.Name.Contains(mainTableName))
            {
                return false;
            }


            // 跳过忽略的属性
            var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>();
            if (sugarColumnAttr != null && sugarColumnAttr.IsIgnore)
                return true;

            // 跳过导航属性（除非是子表集合）
            var navigateAttr = property.GetCustomAttribute<Navigate>();
            if (navigateAttr != null && navigateAttr.GetNavigateType() != NavigateType.OneToMany)
                return true;

            // 跳过不需要记录的属性
            var skipProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "StatusEvaluator", "FieldNameList", "HelpInfos", "RowImage"
        };

            return skipProperties.Contains(property.Name);
        }

        private static string GetPropertyDisplayName(PropertyInfo property)
        {
            // 优先使用 AuditField 特性的 DisplayName
            var auditAttr = property.GetCustomAttribute<AuditFieldAttribute>();
            if (auditAttr != null && !string.IsNullOrEmpty(auditAttr.DisplayName))
                return auditAttr.DisplayName;

            // 其次使用 SugarColumn 特性的 ColumnDescription
            var sugarAttr = property.GetCustomAttribute<SugarColumn>();
            if (sugarAttr != null && !string.IsNullOrEmpty(sugarAttr.ColumnDescription))
                return sugarAttr.ColumnDescription;

            // 最后使用属性名
            return property.Name;
        }

        private static bool IsChildTableProperty(PropertyInfo property, string mainTableName)
        {
            // 检查是否为集合类型
            if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType))
                return false;

            // 获取集合元素类型
            Type elementType;
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                elementType = property.PropertyType.GetGenericArguments()[0];
            }
            else if (property.PropertyType.IsArray)
            {
                elementType = property.PropertyType.GetElementType();
            }
            else
            {
                return false;
            }

            // 检查元素类型是否符合子表命名模式
            if (mainTableName != null && elementType.Name == $"{mainTableName}Detail")
                return true;

            // 检查是否有导航特性且为一对多关系
            var navigateAttr = property.GetCustomAttribute<Navigate>();
            return navigateAttr != null && navigateAttr.GetNavigateType() == NavigateType.OneToMany;
        }

        private static bool IsNullOrEmpty(object value)
        {
            if (value == null)
                return true;

            if (value is string str)
                return string.IsNullOrEmpty(str);

            if (value is System.Collections.IEnumerable collection)
                return !collection.Cast<object>().Any();

            return false;
        }

        // 类型数据缓存结构
        private class TypeData
        {
            public Type Type { get; set; }
            public List<PropertyData> AuditedProperties { get; set; }
        }

        // 属性数据结构
        private class PropertyData
        {
            public PropertyInfo Info { get; set; }
            public string DisplayName { get; set; }
            public bool IsChildTable { get; set; }
        }
    }
}
