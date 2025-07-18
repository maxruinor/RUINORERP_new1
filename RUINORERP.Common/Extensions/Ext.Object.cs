﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using RUINORERP.Common.ClassLibrary;

namespace RUINORERP.Common.Extensions
{
    public static partial class ExtObject
    {
        //        扩展方法的注意点：
        //* 需要写在一个静态类中
        //* 必须是一个静态方法
        //* 通过第一个参数和this关键字指定扩展的目标类型
        //* 不同类型的扩展方法不一定要写在同一个类中


        private static BindingFlags BindingFlags =>
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;


        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            string objStr = obj.ToString();
            return string.IsNullOrEmpty(objStr);
        }

        /// <summary>
        /// 不等于NULL？
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 等于NULL？
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ContractResolver = new CustomContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            //serializerSettings.Converters.Add(new UnixDateTimeConvertor());

            return JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToRedisJson(this object obj)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ContractResolver = new CustomContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings);
        }

        /// <summary>
        /// 将对象序列化成Json字符串,同时忽略null字段
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJsonByIgnore(this object obj)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ContractResolver = new CustomContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings);
        }

        /// <summary>
        /// 实体类转json数据，速度快
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public static string EntityToJson(this object t)
        {
            if (t == null)
                return null;
            string jsonStr = "";
            jsonStr += "{";
            PropertyInfo[] infos = t.GetType().GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                jsonStr = jsonStr + "\"" + infos[i].Name + "\":\"" + infos[i].GetValue(t) + "\"";
                if (i != infos.Length - 1)
                    jsonStr += ",";
            }

            jsonStr += "}";
            return jsonStr;
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepCloneByjson<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;

            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr);
            if (xmlDoc != null)
            {
                string xmlDocStr = xmlDoc.InnerXml;

                return xmlDocStr;
            }

            return null;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="rootNodeName">根节点名(建议设为xml)</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj, string rootNodeName)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, rootNodeName);
            if (xmlDoc != null)
            {
                string xmlDocStr = xmlDoc.InnerXml;

                return xmlDocStr;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static int ToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue == null) return 0;
            if (thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            // 如果转换失败，尝试将其转换为 decimal 类型，然后再转换为 int 类型
            if (decimal.TryParse(thisValue.ToString(), out decimal decimalValue))
            {
                return (int)decimalValue;
            }
            return reval;
        }
        /// <summary>
        /// 将decimal值四舍五入到指定的小数位数。
        /// </summary>
        /// <param name="value">要四舍五入的decimal值</param>
        /// <param name="decimalPlaces">小数位数</param>
        /// <returns>四舍五入后的decimal值</returns>
        public static decimal ToRoundDecimalPlaces(this decimal value, int decimalPlaces)
        {
            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException("decimalPlaces", "小数位数不能为负数");

            var factor = Math.Pow(10, decimalPlaces);
            var result = Math.Round(Convert.ToDouble(value) * factor) / factor;
            return (decimal)result;
        }

        public static long ToLong(this object thisValue)
        {
            if (thisValue == null) return 0;
            if (thisValue is long) return (long)thisValue; // 如果已经是long类型，直接返回
            if (thisValue is string strValue)
            {
                // 尝试将字符串转换为long
                if (long.TryParse(strValue, out long reval))
                {
                    return reval;
                }
                else
                {
                    throw new ArgumentException("字符串不能转换为long类型。");
                }
            }
            if (thisValue is IConvertible convertible)
            {
                // 尝试使用IConvertible接口转换为long
                try
                {
                    return convertible.ToInt64(null);
                }
                catch
                {
                    throw new ArgumentException("对象不能转换为long类型。");
                }
            }
            throw new ArgumentException("不支持的对象类型转换为long。");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static int ToInt(this object thisValue, int errorValue)
        {
            int reval;
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return errorValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static double ToMoney(this object thisValue)
        {
            double reval;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static double ToMoney(this object thisValue, double errorValue)
        {
            double reval;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return errorValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static string ToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString()?.Trim();
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static string ToString(this object thisValue, string errorValue)
        {
            if (thisValue != null) return thisValue.ToString()?.Trim();
            return errorValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object thisValue)
        {
            decimal reval;
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object thisValue, decimal errorValue)
        {
            decimal reval;
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return errorValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object thisValue)
        {
            DateTime reval = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                reval = Convert.ToDateTime(thisValue);
            }
            return reval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object thisValue, DateTime errorValue)
        {
            DateTime reval;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return errorValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ToBool(this object thisValue)
        {
            bool reval = false;
            if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return reval;
        }

        /// <summary>
        /// 是否拥有某属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static bool ContainsProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, BindingFlags) != null;
        }


        public static PropertyInfo GetPropertyInfo<T>(this T obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Type type = obj.GetType();
            return type.GetProperty(propertyName);
        }
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetProperty(propertyName);
        }



        /// <summary>
        /// 获取某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
        //    return obj.GetType().GetProperty(propertyName, BindingFlags)?.GetValue(obj);
        //}
        //// 扩展方法: 获取对象属性值
        //public static object GetPropertyValue(this object obj, string propertyName)
        //{
            if (obj == null) return null;
            var property = obj.GetType().GetProperty(propertyName);
            return property?.GetValue(obj);
        }

        /// <summary>
        /// 只是为了方便取属性名。因为实体修改后，编译时就能发现错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyNameExp"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(this object obj, Expression<Func<T, object>> propertyNameExp)
        {
            string propertyName = propertyNameExp.GetMemberInfo().Name;
            return obj.GetType().GetProperty(propertyName, BindingFlags)?.GetValue(obj);
        }


        /// <summary>
        /// 只是为了方便取属性名。因为实体修改后，编译时就能发现错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyNameExp"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, BindingFlags)?.GetValue(obj);
        }

        /// <summary>
        /// 设置某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
        //    obj.GetType().GetProperty(propertyName, BindingFlags)?.SetValue(obj, value);
        //}
        //// 扩展方法: 设置对象属性值
        //public static void SetPropertyValue(this object obj, string propertyName, object value)
        //{
            if (obj == null) return;
            var property = obj.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);
            }
        }

        /// <summary>
        /// 设置某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue<T>(this object obj, Expression<Func<T, object>> propertyNameExp, object value)
        {
            string propertyName = propertyNameExp.GetMemberInfo().Name;
            obj.GetType().GetProperty(propertyName, BindingFlags)?.SetValue(obj, value);
        }

        /// <summary>
        /// 是否拥有某字段
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static bool ContainsField(this object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName, BindingFlags) != null;
        }

        /// <summary>
        /// 获取某字段值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static object GetGetFieldValue(this object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName, BindingFlags)?.GetValue(obj);
        }

        /// <summary>
        /// 设置某字段值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetFieldValue(this object obj, string fieldName, object value)
        {
            obj.GetType().GetField(fieldName, BindingFlags)?.SetValue(obj, value);
        }

        /// <summary>
        /// 改变实体类型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType(this object obj, Type targetType)
        {
            return obj.ToJson().ToObject(targetType);
        }

        /// <summary>
        /// 改变实体类型
        /// </summary>
        /// <typeparam name="T">目标泛型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T ChangeType<T>(this object obj)
        {
            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 改变类型
        /// </summary>
        /// <param name="obj">原对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType_ByConvert(this object obj, Type targetType)
        {
            object resObj = obj;
            try
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    NullableConverter newNullableConverter = new NullableConverter(targetType);
                    resObj = newNullableConverter.ConvertFrom(obj);
                }
                else
                {
                    resObj = Convert.ChangeType(obj, targetType);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("转换错误:" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return resObj;
        }

        /// <summary>
        /// 代替ChangeType_ByConvert
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static object ChangeTypeSafely(this object obj, Type targetType)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                {
                    // 如果obj是null或DBNull.Value，尝试创建targetType的默认值
                    if (Nullable.GetUnderlyingType(targetType) != null || targetType.IsValueType)
                    {
                        return Activator.CreateInstance(targetType);
                    }
                    return null;
                }

                if (obj is string strObj && string.IsNullOrEmpty(strObj))
                {
                    // 如果obj是空字符串，特殊处理
                    if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                    {
                        return decimal.Zero;
                    }
                    // 如果obj是空字符串，特殊处理
                    if (targetType == typeof(int) || targetType == typeof(int?))
                    {
                        return 0;
                    }
                }

                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    NullableConverter nullableConverter = new NullableConverter(targetType);
                    return nullableConverter.ConvertFrom(obj);
                }
                else
                {
                    return Convert.ChangeType(obj, targetType);
                }
            }
            catch (Exception ex)
            {
                // 抛出异常，让调用者处理
                throw new InvalidOperationException("转换错误:" + $"无法将对象转换为类型 {targetType.FullName}\r\n对象值：{obj.ToString()}", ex);
            }
        }

        /// <summary>
        /// 将一个对象中的指定属性名的属性设置为null（值类型重置为默认值)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetPropertyInfoToNull(this object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                // 检查是否可以写入（可设置值）并且不是索引器或只读属性
                if (property.CanWrite && property.Name == propertyName)
                {
                    // 尝试将引用类型的属性设置为 null
                    if (property.PropertyType.IsClass || Nullable.GetUnderlyingType(property.PropertyType) != null)
                    {
                        property.SetValue(obj, null, null);
                    }
                    // 值类型的属性不能设置为 null，这里可以进行特殊处理，例如重置为默认值
                    else if (property.PropertyType.IsValueType)
                    {
                        // 重置为默认值
                        property.SetValue(obj, Activator.CreateInstance(property.PropertyType), null);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 将一个对象中的指定属性名的属性设置为null（值类型重置为默认值)
        /// 这个方法有点小问题。但是在这里确实可以按照这逻辑使用，就是如果要null值设置，可以当做直接返回的意思。
        /// 修改的方法就是先判断值类型。else就是null值设置
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetPropertyInfoToNull(this object obj, PropertyInfo property)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            // 检查是否可以写入（可设置值）并且不是索引器或只读属性
            if (property.CanWrite)
            {
                // 尝试将引用类型的属性设置为 null
                if (property.PropertyType.IsClass || Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    property.SetValue(obj, null, null);
                }
                // 值类型的属性不能设置为 null，这里可以进行特殊处理，例如重置为默认值
                else if (property.PropertyType.IsValueType)
                {
                    // 重置为默认值
                    property.SetValue(obj, Activator.CreateInstance(property.PropertyType), null);
                }
            }
        }


        /// <summary>
        /// GenericTypeExtensions这个类中已经使用
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //public static string GetGenericTypeName(this Type type)
        //{
        //    string typeName;

        //    if (type.IsGenericType)
        //    {
        //        var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
        //        typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        //    }
        //    else
        //    {
        //        typeName = type.Name;
        //    }

        //    return typeName;
        //}

  
    }
}