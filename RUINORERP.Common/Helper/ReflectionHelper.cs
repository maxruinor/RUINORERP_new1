using RUINORERP.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.Common.Helper
{
    /// <summary>
    /// c# 反射相关帮助类 2020-8-8
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 取指定类型的属性
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <param name="fieldName">对应的属性名</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(object obj, string fieldName)
        {
            Type type = typeof(T);
            return GetPropertyInfo<T>(obj, fieldName);
        }

        /// <summary>
        /// 取指定类型的属性
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <param name="fieldName">对应的属性名</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Type type, object obj, string fieldName)
        {
            PropertyInfo pi = null;
            foreach (PropertyInfo field in type.GetProperties())
            {
                //这里全小写了
                if (field.Name.ToLower() == fieldName.ToLower())
                {
                    pi = field;
                    break;
                }
            }
            return pi;
        }



        #region 对象相关 
        /// <summary>
        /// 取对象属性值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetModelValue(string FieldName, object obj)
        {
            if (obj==null)
            {
                return null;
            }
            try
            {
                Type Ts = obj.GetType();
                var pro = Ts.GetProperty(FieldName);
                if (pro == null)
                {
                    return null;
                }
                object o = pro.GetValue(obj, null);
                if (null == o)
                    return null;
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value))
                    return null;
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// 设置对象属性值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="Value"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SetModelValue(string FieldName, string Value, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object v = Convert.ChangeType(Value, Ts.GetProperty(FieldName).PropertyType);
                Ts.GetProperty(FieldName).SetValue(obj, v, null);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion




        public static bool ExistPropertyName(Type t, string PropertyName)
        {
            bool rs = false;
            //  Type t = typeof(T);
            PropertyInfo[] array_property = t.GetProperties();
            foreach (PropertyInfo property in array_property)
            {
                if (property.Name == PropertyName)
                {
                    rs = true;
                    break;
                }
            }
            return rs;
        }


        //public static bool ExistPropertyName<T>(Func<T, object> Exp)
        //{
        //    return ExistPropertyName<T>(Exp.GetMethodInfo().Name);
        //}

        //public static bool ExistPropertyName<T>(Expression<Func<T, object>> PNameExp)
        //{
        //    MemberInfo minfo = PNameExp.GetMemberInfo();
        //    string propertyName = minfo.Name;
        //    return ExistPropertyName<T>(propertyName);
        //}
        public static bool ExistPropertyName<T>(string PropertyName)
        {
            bool rs = false;
            Type t = typeof(T);
            PropertyInfo[] array_property = t.GetProperties();
            foreach (PropertyInfo property in array_property)
            {
                if (property.Name == PropertyName)
                {
                    rs = true;
                    break;
                }
            }
            return rs;
        }


        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetAllPropertyName(Type t)
        {
            IList<string> list_propertyname = null;

            //  Type t = typeof(T);
            PropertyInfo[] array_property = t.GetProperties();

            //检测是否有属性
            if (array_property.Length <= 0)
                return list_propertyname;

            list_propertyname = new List<string>();
            foreach (PropertyInfo property in array_property)
            {
                string s_propertyname = property.Name;
                list_propertyname.Add(s_propertyname);
            }

            return list_propertyname;
        }



        /// <summary>
        /// 获取对象属性名对应的值
        /// </summary>
        /// <param name="obj">有值的对象</param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, string propertyname)
        {
            Type type = obj.GetType();

            PropertyInfo property = type.GetProperty(propertyname);

            //可以注释了。只是测试用
            //PropertyInfo[] pinfos = type.GetProperties();

            if (property == null) return string.Empty;

            object o = property.GetValue(obj, null);

            if (o == null) return string.Empty;

            return o;
        }


        /// <summary>
        /// 获取对象属性名对应的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">实例，要有值，否则出错</param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyname);

            //可以注释了。只是测试用
            //PropertyInfo[] pinfos = type.GetProperties();

            if (property == null) return string.Empty;

            object o = property.GetValue(t, null);

            if (o == null) return string.Empty;

            return o.ToString();
        }




        /// <summary>
        /// 最好还是不用这个，直接调用 指定参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tp"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static object ExecutionClassMethod(Type tp, string MethodName, object[] para)
        {
            //Type tp = typeof(T);

            // GetMethod : 搜索具有指定名称的公共方法。
            MethodInfo method = tp.GetMethod(MethodName);

            // 创造一个实例
            object obj = Activator.CreateInstance(tp);

            // 调用函数
            return method.Invoke(obj, para);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tp"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static object ExecutionClassMethod(Type tp, string MethodName)
        {
            //Type tp = typeof(T);

            // GetMethod : 搜索具有指定名称的公共方法。
            MethodInfo method = tp.GetMethod(MethodName);

            // 创造一个实例
            object obj = Activator.CreateInstance(tp);

            // 调用函数
            return method.Invoke(obj, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tp"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static object ExecutionClassMethod<T>(Type tp, string MethodName)
        {
            //Type tp = typeof(T);

            // GetMethod : 搜索具有指定名称的公共方法。
            MethodInfo method = tp.GetMethod(MethodName);

            // 创造一个实例
            object obj = Activator.CreateInstance(tp);

            // 调用函数
            return method.Invoke(obj, null);
        }

        /// <summary>
        /// 类型匹配
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsType(Type type, string typeName)
        {
            if (type.ToString() == typeName)
                return true;
            if (type.ToString() == "System.Object")
                return false;

            return IsType(type.BaseType, typeName);
        }

        /// <summary>
        /// 设置实体属性值,根据属性名称
        /// add by Vincent.Q 10.12.30 by 2020-8-8 优化提取
        /// </summary>
        /// <param name="as_propertyname"></param>
        /// <param name="aobj_propertyvalue"></param>
        public static void SetPropertyValue(object obj, string as_propertyname, object aobj_propertyvalue)
        {
            if (obj == null)
            {
                return;
            }

            Type t = obj.GetType();
            PropertyInfo property = t.GetProperty(as_propertyname);
            if (property == null)
            {
                return;
            }

            object PropertyValue = aobj_propertyvalue;

            // 设置属性值
            try
            {
                // 枚举类型
                if (property.PropertyType.IsEnum)
                {
                    if (PropertyValue == null)
                    {
                        property.SetValue(obj, null);
                    }
                    else
                    {
                        property.SetValue(obj, Enum.Parse(property.PropertyType, PropertyValue.ToString()));
                    }
                }
                // 字节数组
                else if (property.PropertyType.IsArray && property.PropertyType.Name.ToLower() == "byte[]")
                {
                    if (PropertyValue == null)
                    {
                        property.SetValue(obj, null);
                    }
                    else if (PropertyValue.GetType().Name == "Bitmap")
                    {
                        property.SetValue(obj, ImageHelper.ConvertImageToByteEx(PropertyValue as Bitmap));
                    }
                    else if (PropertyValue.GetType().Name == "Image")
                    {
                        property.SetValue(obj, ImageHelper.ConvertImageToByteEx(PropertyValue as Image));
                    }
                    else
                    {
                        property.SetValue(obj, PropertyValue);
                    }
                }
                // 日期时间类型
                else if (property.PropertyType == typeof(DateTime))
                {
                    if (PropertyValue == null)
                    {
                        property.SetValue(obj, default(DateTime));
                    }
                    else if (PropertyValue is DateTime)
                    {
                        property.SetValue(obj, PropertyValue);
                    }
                    else
                    {
                        if (DateTime.TryParse(PropertyValue.ToString(), out DateTime parsedDate))
                        {
                            property.SetValue(obj, parsedDate);
                        }
                        else
                        {
                            property.SetValue(obj, default(DateTime));
                        }
                    }
                }
                // 字符串类型
                else if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, PropertyValue?.ToString() ?? string.Empty);
                }
                // 泛型 Nullable<>
                else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (PropertyValue == null || PropertyValue.ToString() == "")
                    {
                        property.SetValue(obj, null);
                    }
                    else
                    {
                        var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                        property.SetValue(obj, Convert.ChangeType(PropertyValue, underlyingType));
                    }
                }
                // 值类型（如 int, long, double 等）
                else if (property.PropertyType.IsValueType)
                {
                    if (PropertyValue == null || PropertyValue.ToString() == "")
                    {
                        property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        try
                        {
                            property.SetValue(obj, Convert.ChangeType(PropertyValue, property.PropertyType));
                        }
                        catch (Exception exC)
                        {
                            property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                        }
                    }
                }
                // 其他类型
                else
                {
                    if (PropertyValue == null)
                    {
                        property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        property.SetValue(obj, Convert.ChangeType(PropertyValue, property.PropertyType));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"设置属性值时出错：{obj.GetType().Name}: 属性名: {as_propertyname}, 值: {aobj_propertyvalue}, 错误信息: {ex.Message}");
            }
        }

        /*
        public static void SetPropertyValue(object obj, string as_propertyname, object aobj_propertyvalue)
        {
            try
            {
                if (obj == null)
                {
                    return;

                }
                #region 设置属性值
                //获取实体类型
                Type t = obj.GetType();
                //获取属性信息,并判断是否存在
                PropertyInfo property = t.GetProperty(as_propertyname);
                if (property == null)
                    return;

                string PropertyName = property.Name;

                //object PropertyValue = aobj_propertyvalue == null ? string.Empty : aobj_propertyvalue;
                object PropertyValue = aobj_propertyvalue;

                if (property.PropertyType.IsEnum) //属性类型是否表示枚举
                {
                    object enumName = Enum.ToObject(property.PropertyType, int.Parse(PropertyValue.ToString()));
                    t.GetProperty(as_propertyname).SetValue(obj, enumName, null); //获取枚举值，设置属性值
                }
                else if (property.PropertyType.IsArray && property.PropertyType.Name.ToLower() == "byte[]")
                {
                    if (PropertyValue == null)
                    {
                        return;
                    }
                    //MessageBox.Show(PropertyValue.GetType().Name);
                    if (PropertyValue.GetType().Name == "Bitmap")
                    {
                        property.SetValue(obj, ImageHelper.ConvertImageToByteEx(PropertyValue as Bitmap));
                    }
                    else if (PropertyValue.GetType().Name == "Image")
                    {
                        property.SetValue(obj, ImageHelper.ConvertImageToByteEx(PropertyValue as Image));
                    }
                    else
                    {
                        property.SetValue(obj, PropertyValue);
                    }

                }
                else if (property.PropertyType.Name.ToLower() == "datetime")
                {
                    property.SetValue(obj, PropertyValue);
                }
                else if (property.PropertyType.Name.ToLower() == "string")
                {
                    PropertyValue = PropertyValue == null ? string.Empty : PropertyValue;
                    property.SetValue(obj, PropertyValue);
                }
                else if (!property.PropertyType.IsGenericType)
                {
                    if (PropertyValue == null)
                    {
                        property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        try
                        {
                            // 尝试转换类型
                            property.SetValue(obj, Convert.ChangeType(PropertyValue, property.PropertyType));
                        }
                        catch (Exception exC)
                        {
                            property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                            //Console.WriteLine($"类型转换失败: {ex.Message}");
                        }
                    }
                }
                else
                {
                    //泛型Nullable<>
                    Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        //Nullable<>类型时，判断是否有值，有值则进行转换，没有则赋值为null
                        if (PropertyValue == null || PropertyValue.ToString() == "")
                        {
                            property.SetValue(obj, null);
                        }
                        else
                        {

                            property.SetValue(obj, Convert.ChangeType(PropertyValue, Nullable.GetUnderlyingType(property.PropertyType)));
                        }

                    }
                }

                return;

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception($"设置属性值时出错：{obj.GetType().Name}:属性名:{as_propertyname},值:{aobj_propertyvalue},{ex.Message}");
            }

        }

        */


        /// <summary>
        /// 没有完全成。后面优化2020
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="MethodName"></param>
        public static void GetMethodParameterInfo(Type ht, string MethodName)
        {

            MethodInfo methodInfo = ht.GetMethod(MethodName);
            ParameterInfo[] paramsInfo = methodInfo.GetParameters();//得到指定方法的参数列表   

            object[] obj = new object[paramsInfo.Length];
            for (int i = 0; i < paramsInfo.Length; i++)
            {

                Type tType = paramsInfo[i].ParameterType;

                //如果它是值类型,或者String   

                if (tType.Equals(typeof(string)) || (!tType.IsInterface && !tType.IsClass))
                {
                    //改变参数类型   
                    //  obj[i] = Convert.ChangeType(Request.Form[i], tType);

                }

                else if (tType.IsClass)//如果是类,将它的json字符串转换成对象   
                {

                    // obj[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(Request.Form[i], tType);
                }

            }
        }


        public T GetTypeClass<T>() where T : new()
        {
            T Tclass = new T();  //实例化一个 T 类对象
                                 //获取该类类型  
            System.Type t = Tclass.GetType();

            //得到所有属性
            System.Reflection.PropertyInfo[] propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // 遍历所有属性
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo property = propertyInfos[i];
                // 获取属性名
                //Debug.LogError("property name : " + property.Name);
                // 给属性赋值
                property.SetValue(Tclass, System.Convert.ChangeType(i, property.PropertyType), null);
                // 获取属性值
                //Debug.LogError(property.GetValue(Tclass, null));
            }

            return Tclass;
        }







    }
}
