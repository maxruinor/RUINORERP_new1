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
        /// 支持普通对象、动态对象和字典对象
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
                    if (dictionary.TryGetValue(propertyName, out var value))
                        return value;
                    return null;
                }

                // 处理动态对象
                if (obj is dynamic)
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
                var property = obj.GetType().GetProperty(propertyName, 
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (property != null && property.CanRead)
                    return property.GetValue(obj);

                // 尝试获取字段值（如果是私有字段等）
                var field = obj.GetType().GetField(propertyName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (field != null)
                    return field.GetValue(obj);

                return null;
            }
            catch (Exception ex)
            {
                // 在实际使用中应该使用日志记录异常
                // 为了简洁，这里直接返回null
                return null;
            }
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