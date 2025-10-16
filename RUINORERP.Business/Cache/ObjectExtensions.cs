using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

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
                if (obj.GetType().FullName == "Newtonsoft.Json.Linq.JObject")
                {
                    dynamic jObj = obj;
                    JToken token = jObj[propertyName];
                    if (token != null)
                    {
                        // 返回强类型值而不是JToken
                        return token.ToObject<object>();
                    }
                    // 尝试大小写不敏感匹配
                    dynamic jObjLower = obj;
                    foreach (var prop in (IDictionary<string, JToken>)jObj)
                    {
                        if (prop.Key.ToLower() == propertyName.ToLower())
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
                // 记录异常信息到日志（如果有日志服务）
                // System.Diagnostics.Debug.WriteLine($"获取属性值失败: {ex.Message}");
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
            
            // 对于字符串比较，转换为字符串后比较
            string str1 = value1.ToString();
            string str2 = value2.ToString();
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 将对象转换为ExpandoObject
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
                    resultDict[kvp.Key] = kvp.Value;
                
                return result;
            }

            // 对于普通对象，使用反射获取属性并创建ExpandoObject
            var resultExpando = new ExpandoObject();
            var resultExpandoDict = (IDictionary<string, object>)resultExpando;
            
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.CanRead)
                    resultExpandoDict[prop.Name] = prop.GetValue(obj);
            }

            return resultExpando;
        }
    }
}