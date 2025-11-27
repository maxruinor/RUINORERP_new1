using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 对象扩展方法类
    /// 提供用于缓存系统的对象操作辅助方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 获取对象的指定属性值（缓存专用）
        /// 支持普通对象、动态对象、字典对象和特殊类型
        /// 增强了类型处理和值比较能力，解决不明确匹配问题
        /// </summary>
        /// <param name="obj">源对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值，如果属性不存在则返回null</returns>
        public static object GetCachePropertyValue(this object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;

            try
            {
                // 处理IDictionary<string, object>类型（包括ExpandoObject）
                if (obj is IDictionary<string, object> dictionary)
                {
                    // 首先尝试精确匹配
                    if (dictionary.TryGetValue(propertyName, out var value))
                        return value;
                    
                    // 尝试大小写不敏感匹配
                    var lowerPropertyName = propertyName.ToLower();
                    foreach (var kvp in dictionary)
                    {
                        if (kvp.Key.ToLower() == lowerPropertyName)
                            return kvp.Value;
                    }
                    return null;
                }

                // 处理Newtonsoft.Json.Linq.JObject类型
                if (obj is JObject jObject)
                {
                    // 首先尝试精确匹配
                    JToken token = jObject[propertyName];
                    if (token != null)
                    {
                        // 返回强类型值而不是JToken
                        return token.ToObject<object>();
                    }
                    
                    // 尝试大小写不敏感匹配
                    var lowerPropertyName = propertyName.ToLower();
                    foreach (var prop in (IDictionary<string, JToken>)jObject)
                    {
                        if (prop.Key.ToLower() == lowerPropertyName)
                            return prop.Value.ToObject<object>();
                    }
                    return null;
                }

                // 处理动态对象
                if (obj is ExpandoObject || obj.GetType().IsCOMObject)
                {
                    try
                    {
                        return (object)((dynamic)obj)[propertyName];
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        // 属性不存在时返回null
                        return null;
                    }
                }

                // 处理普通对象，使用反射获取属性值
                // 尝试精确匹配
                var property = obj.GetType().GetProperty(propertyName, 
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (property != null && property.CanRead)
                {
                    var value = property.GetValue(obj);
                    // 确保返回强类型值
                    return value;
                }

                // 尝试获取字段值（如果是私有字段等）
                var field = obj.GetType().GetField(propertyName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (field != null)
                    return field.GetValue(obj);

                // 尝试其他可能的访问方式
                // 例如，对于某些特殊对象，可能需要调用特定的方法来获取属性
                return null;
            }
            catch (Exception ex)
            {
                // 记录异常信息到调试输出
                Debug.WriteLine($"获取属性值失败 [属性名: {propertyName}]: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 安全比较两个对象的值是否相等（用于缓存键比较）
        /// 处理不同类型之间的比较，如字符串和数值类型
        /// </summary>
        /// <param name="value1">第一个值</param>
        /// <param name="value2">第二个值</param>
        /// <returns>如果值相等则返回true，否则返回false</returns>
        public static bool SafeEquals(this object value1, object value2)
        {
            // 处理null情况
            if (value1 == null && value2 == null)
                return true;
            if (value1 == null || value2 == null)
                return false;
            
            // 如果类型相同，直接比较
            if (value1.GetType() == value2.GetType())
                return value1.Equals(value2);
            
            // 特殊处理字符串：如果任一对象已经是字符串，进行类型转换后比较
            if (value1 is string || value2 is string)
            {
                // 避免重复转换
                string str1 = value1 is string ? (string)value1 : value1.ToString();
                string str2 = value2 is string ? (string)value2 : value2.ToString();
                return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
            }
            
            // 对于其他不同类型，尝试值转换比较（例如int和long）
            try
            {
                // 对于数值类型，尝试转换为双精度浮点数进行比较
                if (IsNumericType(value1.GetType()) && IsNumericType(value2.GetType()))
                {
                    double num1 = Convert.ToDouble(value1);
                    double num2 = Convert.ToDouble(value2);
                    return Math.Abs(num1 - num2) < double.Epsilon;
                }
            }
            catch (InvalidCastException)
            {
                // 类型转换失败时，回到基本的ToString比较
            }
            
            // 最后的回退方案：转换为字符串后比较
            return string.Equals(value1.ToString(), value2.ToString(), StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// 检查指定类型是否为数值类型
        /// </summary>
        private static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        /// <summary>
        /// 将对象转换为ExpandoObject
        /// 安全地处理各种对象类型，包括null值、ExpandoObject、字典和普通对象
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的ExpandoObject</returns>
        public static ExpandoObject ToExpandoObject(this object obj)
        {
            if (obj == null)
                return new ExpandoObject();

            // 如果已经是ExpandoObject，直接返回
            if (obj is ExpandoObject expando)
                return expando;

            // 如果是字典，转换为ExpandoObject
            if (obj is IDictionary<string, object> dictionary)
            {
                var result = new ExpandoObject();
                var resultDict = (IDictionary<string, object>)result;
                
                foreach (var kvp in dictionary)
                {
                    // 递归转换嵌套对象
                    if (kvp.Value != null && !(kvp.Value is string) && !(kvp.Value is ValueType) && kvp.Value.GetType().IsClass)
                        resultDict[kvp.Key] = ToExpandoObject(kvp.Value);
                    else
                        resultDict[kvp.Key] = kvp.Value;
                }
                
                return result;
            }

            // 对于普通对象，使用反射获取属性并创建ExpandoObject
            try
            {
                var resultExpando = new ExpandoObject();
                var resultExpandoDict = (IDictionary<string, object>)resultExpando;
                
                foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (prop.CanRead)
                    {
                        try
                        {
                            var value = prop.GetValue(obj);
                            // 递归转换嵌套对象
                            if (value != null && !(value is string) && !(value is ValueType) && value.GetType().IsClass)
                                resultExpandoDict[prop.Name] = ToExpandoObject(value);
                            else
                                resultExpandoDict[prop.Name] = value;
                        }
                        catch (Exception ex)
                        {
                            // 忽略单个属性获取失败的情况，记录错误但继续处理其他属性
                            Debug.WriteLine($"获取属性值失败 [属性名: {prop.Name}]: {ex.Message}");
                        }
                    }
                }

                return resultExpando;
            }
            catch (Exception ex)
            {
                // 如果整个对象转换失败，记录错误并返回空的ExpandoObject
                Debug.WriteLine($"对象转换为ExpandoObject失败: {ex.Message}");
                return new ExpandoObject();
            }
        }
    }
}