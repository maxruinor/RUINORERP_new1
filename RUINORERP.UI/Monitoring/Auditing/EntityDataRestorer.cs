using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using RUINORERP.Model.CommonModel;

namespace RUINORERP.UI.Monitoring.Auditing
{
    /// <summary>
    /// 实体数据恢复器（支持主子表结构）
    /// </summary>
    public static class EntityDataRestorer
    {
        // 缓存类型属性映射（中文名->PropertyInfo）
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _propertyMapCache =
            new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// 将JSON数据恢复为实体对象（支持主子表结构）
        /// </summary>
        /// <typeparam name="T">主实体类型</typeparam>
        /// <param name="jsonData">EntityDataExtractor生成的JSON数据</param>
        /// <returns>恢复后的实体对象</returns>
        public static T RestoreEntity<T>(string jsonData) where T : new()
        {
            // 解析JSON数据
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            return (T)RestoreEntity(typeof(T), data);
        }

        public static object RestoreEntity(Type entityType, Dictionary<string, object> data)
        {
            // 创建实体实例
            var entity = Activator.CreateInstance(entityType);

            // 获取属性映射（中文名->PropertyInfo）
            var propertyMap = GetPropertyMap(entityType);

            foreach (var kvp in data)
            {
                var displayName = kvp.Key;
                var value = kvp.Value;

                // 查找匹配的属性
                if (propertyMap.TryGetValue(displayName, out var property))
                {
                    // 处理子表集合
                    if (IsChildTableCollection(property))
                    {
                        var childItems = RestoreChildTable(property, value);
                        property.SetValue(entity, childItems);
                    }
                    // 处理普通属性
                    else
                    {
                        var convertedValue = ConvertValue(value, property.PropertyType);
                        property.SetValue(entity, convertedValue);
                    }
                }
            }

            return entity;
        }

        // 处理子表集合
        private static object RestoreChildTable(PropertyInfo collectionProperty, object value)
        {
            // 获取子项类型（List<T>中的T）
            var itemType = collectionProperty.PropertyType.GetGenericArguments()[0];

            // 创建集合实例
            var listType = typeof(List<>).MakeGenericType(itemType);
            var collection = (IList)Activator.CreateInstance(listType);

            // 处理每个子表项
            if (value is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    if (item is JObject jObject)
                    {
                        var childData = jObject.ToObject<Dictionary<string, object>>();
                        var childEntity = RestoreEntity(itemType, childData);
                        collection.Add(childEntity);
                    }
                }
            }

            return collection;
        }

        // 获取类型的中文属性映射
        private static Dictionary<string, PropertyInfo> GetPropertyMap(Type type)
        {
            if (_propertyMapCache.TryGetValue(type, out var cachedMap))
                return cachedMap;

            var propertyMap = new Dictionary<string, PropertyInfo>();

            // 获取所有公共实例属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // 获取属性显示名称
                var displayName = GetPropertyDisplayName(property);

                if (!string.IsNullOrEmpty(displayName))
                {
                    propertyMap[displayName] = property;
                }
            }

            // 添加到缓存
            _propertyMapCache[type] = propertyMap;
            return propertyMap;
        }

        // 获取属性显示名称（与EntityDataExtractor相同的逻辑）
        private static string GetPropertyDisplayName(PropertyInfo property)
        {
            // 1. 检查AuditFieldAttribute
            var auditAttr = property.GetCustomAttribute<AuditFieldAttribute>();
            if (auditAttr != null && !string.IsNullOrEmpty(auditAttr.DisplayName))
                return auditAttr.DisplayName;

            // 2. 检查SugarColumn的ColumnDescription
            var sugarAttr = property.GetCustomAttribute<SugarColumn>();
            if (sugarAttr != null && !string.IsNullOrEmpty(sugarAttr.ColumnDescription))
                return sugarAttr.ColumnDescription;

            // 3. 检查DescriptionAttribute
            var descAttr = property.GetCustomAttribute<DescriptionAttribute>();
            if (descAttr != null && !string.IsNullOrEmpty(descAttr.Description))
                return descAttr.Description;

            // 4. 检查DisplayAttribute
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null && !string.IsNullOrEmpty(displayAttr.Name))
                return displayAttr.Name;

            // 5. 默认使用属性名
            return property.Name;
        }

        // 类型转换处理
        private static object ConvertValue(object value, Type targetType)
        {
            if (value == null)
                return null;

            // 处理可为空类型
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // 枚举类型处理
            if (underlyingType.IsEnum)
            {
                if (value is string strValue)
                    return Enum.Parse(underlyingType, strValue);

                return Enum.ToObject(underlyingType, value);
            }

            // 日期类型处理
            if (underlyingType == typeof(DateTime))
            {
                if (value is string strDate)
                    return DateTime.Parse(strDate);
            }

            // 数值类型处理
            if (underlyingType == typeof(decimal) && value is string decimalStr)
                return decimal.Parse(decimalStr);

            if (underlyingType == typeof(int) && value is string intStr)
                return int.Parse(intStr);

            if (underlyingType == typeof(long) && value is string longStr)
                return long.Parse(longStr);

            // 默认转换
            return Convert.ChangeType(value, underlyingType);
        }

        // 判断是否是子表集合属性
        private static bool IsChildTableCollection(PropertyInfo property)
        {
            // 必须是List<T>类型
            if (!property.PropertyType.IsGenericType ||
                property.PropertyType.GetGenericTypeDefinition() != typeof(List<>))
                return false;

            // 检查是否符合子表命名规则
            var itemType = property.PropertyType.GetGenericArguments()[0];
            return itemType.Name.EndsWith("Detail");
        }
    }
}
