using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public static partial class ObjectExtensionUtils
    {
        private static readonly BindingFlags BindingFlags =
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// 判断对象是否为 Null 或空
        /// </summary>
        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || string.IsNullOrEmpty(obj.ToString());
        }

        /// <summary>
        /// 判断对象是否不为 Null
        /// </summary>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 判断对象是否为 Null
        /// </summary>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        

        /// <summary>
        /// 实体类转 JSON 数据（简易实现，性能较高）
        /// </summary>
        public static string EntityToJson(this object obj)
        {
            if (obj == null)
                return null;

            var properties = obj.GetType().GetProperties();
            if (properties.Length == 0)
                return "{}";

            var sb = new StringBuilder("{");
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var value = property.GetValue(obj);
                sb.Append($"\"{property.Name}\":\"{value?.ToString() ?? ""}\"");
                if (i < properties.Length - 1)
                    sb.Append(",");
            }
            sb.Append("}");

            return sb.ToString();
        }
 
 

        #region 类型转换方法

        /// <summary>
        /// 转换为 Int32 类型
        /// </summary>
        public static int ToInt(this object value, int defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (int.TryParse(value.ToString(), out int result))
                return result;

            if (decimal.TryParse(value.ToString(), out decimal decimalValue))
                return (int)decimalValue;

            return defaultValue;
        }

        /// <summary>
        /// 将 decimal 值四舍五入到指定的小数位数
        /// </summary>
        public static decimal RoundToDecimalPlaces(this decimal value, int decimalPlaces)
        {
            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces), "小数位数不能为负数");

            return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换为 Int64 类型
        /// </summary>
        public static long ToLong(this object value, long defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (value is long longValue)
                return longValue;

            if (long.TryParse(value.ToString(), out long result))
                return result;

            throw new ArgumentException($"无法将对象转换为 long 类型: {value}");
        }

        /// <summary>
        /// 转换为 Double 类型（用于货币）
        /// </summary>
        public static double ToMoney(this object value, double defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            return double.TryParse(value.ToString(), out double result) ? result : defaultValue;
        }

        /// <summary>
        /// 转换为字符串类型
        /// </summary>
        public static string ToSafeString(this object value, string defaultValue = "")
        {
            return value?.ToString()?.Trim() ?? defaultValue;
        }

        /// <summary>
        /// 转换为 Decimal 类型
        /// </summary>
        public static decimal ToDecimal(this object value, decimal defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            return decimal.TryParse(value.ToString(), out decimal result) ? result : defaultValue;
        }

        /// <summary>
        /// 转换为 DateTime 类型
        /// </summary>
        public static DateTime ToDateTime(this object value, DateTime? defaultValue = null)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue ?? DateTime.MinValue;

            if (DateTime.TryParse(value.ToString(), out DateTime result))
                return result;

            return defaultValue ?? DateTime.MinValue;
        }

        /// <summary>
        /// 转换为 Boolean 类型
        /// </summary>
        public static bool ToBool(this object value, bool defaultValue = false)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            return bool.TryParse(value.ToString(), out bool result) ? result : defaultValue;
        }

        #endregion

        #region 属性操作方法

        /// <summary>
        /// 检查对象是否包含指定属性
        /// </summary>
        public static bool ContainsProperty(this object obj, string propertyName)
        {
            return obj != null && obj.GetType().GetProperty(propertyName, BindingFlags) != null;
        }

        /// <summary>
        /// 获取对象的属性信息
        /// </summary>
        public static PropertyInfo GetPropertyInfo(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return obj.GetType().GetProperty(propertyName, BindingFlags);
        }

        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj?.GetType().GetProperty(propertyName, BindingFlags)?.GetValue(obj);
        }

        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var property = obj.GetType().GetProperty(propertyName, BindingFlags);
            property?.SetValue(obj, value);
        }

        /// <summary>
        /// 检查对象是否包含指定字段
        /// </summary>
        public static bool ContainsField(this object obj, string fieldName)
        {
            return obj != null && obj.GetType().GetField(fieldName, BindingFlags) != null;
        }

        /// <summary>
        /// 获取对象的字段值
        /// </summary>
        public static object GetFieldValue(this object obj, string fieldName)
        {
            return obj?.GetType().GetField(fieldName, BindingFlags)?.GetValue(obj);
        }

        /// <summary>
        /// 设置对象的字段值
        /// </summary>
        public static void SetFieldValue(this object obj, string fieldName, object value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var field = obj.GetType().GetField(fieldName, BindingFlags);
            field?.SetValue(obj, value);
        }

        #endregion

        #region 类型转换与操作

        /// <summary>
        /// 安全地将对象转换为指定类型
        /// </summary>
        public static T ChangeType<T>(this object obj)
        {
            return (T)ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// 安全地将对象转换为指定类型
        /// </summary>
        public static object ChangeType(this object obj, Type targetType)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return targetType.IsValueType
                    ? Activator.CreateInstance(targetType)
                    : null;
            }

            var sourceType = obj.GetType();
            if (targetType.IsAssignableFrom(sourceType))
                return obj;

            if (obj is string strValue)
            {
                if (string.IsNullOrEmpty(strValue))
                {
                    if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                        return decimal.Zero;

                    if (targetType == typeof(int) || targetType == typeof(int?))
                        return 0;
                }
            }

            try
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                }

                return Convert.ChangeType(obj, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"无法将对象转换为类型 {targetType.FullName}，值：{obj}", ex);
            }
        }

        /// <summary>
        /// 将对象的指定属性设置为 null 或默认值
        /// </summary>
        public static void SetPropertyToDefault(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var property = obj.GetType().GetProperty(propertyName, BindingFlags);
            if (property == null || !property.CanWrite)
                return;

            SetPropertyToDefault(obj, property);
        }

        /// <summary>
        /// 将对象的指定属性设置为 null 或默认值
        /// </summary>
        public static void SetPropertyToDefault(this object obj, PropertyInfo property)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (property == null || !property.CanWrite)
                return;

            var defaultValue = property.PropertyType.IsValueType
                ? Activator.CreateInstance(property.PropertyType)
                : null;

            property.SetValue(obj, defaultValue);
        }

        /// <summary>
        /// 获取类型的泛型名称
        /// </summary>
        public static string GetGenericTypeName(this Type type)
        {
            if (type == null)
                return string.Empty;

            if (!type.IsGenericType)
                return type.Name;

            var genericArgs = string.Join(",", type.GetGenericArguments().Select(t => t.Name));
            return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericArgs}>";
        }

        /// <summary>
        /// 获取对象类型的泛型名称
        /// </summary>
        public static string GetGenericTypeName(this object obj)
        {
            return obj?.GetType().GetGenericTypeName() ?? string.Empty;
        }

        #endregion
    }
}
