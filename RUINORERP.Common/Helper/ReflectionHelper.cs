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

                string s_datatype = property.PropertyType.Name.Trim().ToLower();

                if (property.PropertyType.IsGenericParameter)
                {

                }
                //属性赋值
                object obj_propertyvalue = null;
                switch (s_datatype)
                {
                    case "datetime":

                        //case "System.Nullable`1[System.DateTime]":
                        if (IsType(property.PropertyType, "System.Nullable`1[System.DateTime]"))
                        {
                            if (aobj_propertyvalue.ToString() != "")
                            {
                                try
                                {
                                    //if (aobj_propertyvalue.ToString().Contains("PDT"))
                                    //{
                                    //    aobj_propertyvalue = aobj_propertyvalue.ToString().Replace("PDT", "-0700");
                                    //}
                                    DateTime dtime = DateTime.Parse(aobj_propertyvalue.ToString());
                                    obj_propertyvalue = dtime;

                                    //property.SetValue(obj, dtime, null);
                                    //property.SetValue(obj, (DateTime?)DateTime.ParseExact(aobj_propertyvalue.ToString(), "yyyy-MM-dd HH:mm:ss", null), null);
                                }
                                catch
                                {
                                    //property.SetValue(obj, (DateTime?)DateTime.ParseExact(aobj_propertyvalue.ToString(), "yyyy-MM-dd", null), null);
                                }
                            }
                            else
                            {
                                obj_propertyvalue = null;
                            }
                            //property.SetValue(obj, null, null);
                        }
                        else
                        {
                            DateTime dtime = DateTime.Parse(aobj_propertyvalue.ToString());
                            obj_propertyvalue = dtime;
                        }
                        break;



                    case "nullable`1":
                        #region  不可空类型
                        string tempdataType = property.PropertyType.FullName.Trim().ToLower();
                        if (tempdataType.Contains("system.int32"))
                        {
                            int i32_parm = 0;
                            if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                                aobj_propertyvalue = 0;

                            i32_parm = Convert.ToInt32(aobj_propertyvalue);
                            obj_propertyvalue = i32_parm;
                        }

                        if (tempdataType.Contains("system.decimal"))
                        {
                            decimal i32_parm = 0;
                            if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                                aobj_propertyvalue = 0;

                            i32_parm = Convert.ToDecimal(aobj_propertyvalue);
                            obj_propertyvalue = i32_parm;
                        }
                        if (tempdataType.Contains("system.datetime"))
                        {

                            var isGeneric = property.PropertyType.IsGenericType;
                            //var isPrimitive = property.PropertyType.IsPrimitive;
                            var isValueType = property.PropertyType.IsValueType;
                            if (isGeneric || (isGeneric && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                //property.SetValue(t, value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(pi.PropertyType)),
                            }


                            //case "System.Nullable`1[System.DateTime]":
                            if (IsType(property.PropertyType, "System.Nullable`1[System.DateTime]"))
                            {
                                if (aobj_propertyvalue.ToString() != "")
                                {
                                    try
                                    {
                                        //if (aobj_propertyvalue.ToString().Contains("PDT"))
                                        //{
                                        //    aobj_propertyvalue = aobj_propertyvalue.ToString().Replace("PDT", "-0700");
                                        //}
                                        DateTime dtime = DateTime.Parse(aobj_propertyvalue.ToString());
                                        obj_propertyvalue = dtime;

                                        //property.SetValue(obj, dtime, null);
                                        //property.SetValue(obj, (DateTime?)DateTime.ParseExact(aobj_propertyvalue.ToString(), "yyyy-MM-dd HH:mm:ss", null), null);
                                    }
                                    catch
                                    {
                                        //property.SetValue(obj, (DateTime?)DateTime.ParseExact(aobj_propertyvalue.ToString(), "yyyy-MM-dd", null), null);
                                    }
                                }
                                else
                                {
                                    obj_propertyvalue = null;
                                }
                                //property.SetValue(obj, null, null);
                            }
                            break;
                        }


                        #endregion

                        break;


                    case "object":
                        object objparm = new object();
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = "";
                        objparm = aobj_propertyvalue;
                        obj_propertyvalue = objparm;
                        break;

                    case "boolean":

                        Boolean Boolean_parm = false;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;
                        Boolean_parm = Convert.ToBoolean(aobj_propertyvalue);
                        obj_propertyvalue = Boolean_parm;
                        break;

                    case "double":
                        Double double_parm = 0;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;

                        double_parm = Convert.ToDouble(aobj_propertyvalue);
                        obj_propertyvalue = double_parm;
                        break;


                    case "int64":
                        Int64 int64_parm = 0;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;

                        int64_parm = Convert.ToInt64(aobj_propertyvalue);
                        obj_propertyvalue = int64_parm;
                        break;

                    case "decimal":
                        decimal decimal_parm = 0;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;

                        decimal_parm = Convert.ToDecimal(aobj_propertyvalue);
                        obj_propertyvalue = decimal_parm;
                        break;

                    case "int32":
                        int i_parm = 0;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0;

                        i_parm = Convert.ToInt32(aobj_propertyvalue);
                        obj_propertyvalue = i_parm;
                        break;
                    case "dec":
                        decimal dc_parm = 0.0m;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = 0.0m;

                        dc_parm = Convert.ToDecimal(aobj_propertyvalue);
                        obj_propertyvalue = dc_parm;
                        break;
                    case "string":
                        string s_parm = string.Empty;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = "";

                        s_parm = aobj_propertyvalue.ToString();
                        obj_propertyvalue = s_parm;
                        break;
                    case "dat":
                        DateTime dtm_parm = DateTime.MinValue;
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            obj_propertyvalue = DateTime.MinValue;

                        dtm_parm = Convert.ToDateTime(aobj_propertyvalue);
                        obj_propertyvalue = dtm_parm;
                        break;

                    case "list`1":
                        object list_parm = new object();
                        if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                            aobj_propertyvalue = "";
                        list_parm = aobj_propertyvalue;
                        obj_propertyvalue = list_parm;
                        break;
                    default:
                        //如果这个字段是基础实体类型，则
                        if (aobj_propertyvalue.GetType().BaseType.ToString() == "SMTAPI.Entity.BaseEntity")
                        {
                            object obj_parm = new object();
                            if (aobj_propertyvalue == null || string.IsNullOrEmpty(aobj_propertyvalue.ToString()))
                                aobj_propertyvalue = "";

                            obj_parm = aobj_propertyvalue;
                            obj_propertyvalue = obj_parm;
                        }
                        else
                        {
                            throw new Exception("请修改代码，处理这种数据类型。" + s_datatype);
                        }


                        break;
                }


                //非泛型 时可以用   property.SetValue(obj，Convert.ChangeType(value,property.PropertyType),null);

                if (!property.PropertyType.IsGenericType)
                {
                    //非泛型
                    t.GetProperty(as_propertyname).SetValue(obj, Convert.ChangeType(obj_propertyvalue, property.PropertyType), null);
                }
                else
                {
                    //泛型Nullable<>
                    Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        t.GetProperty(as_propertyname).SetValue(obj, Convert.ChangeType(obj_propertyvalue, Nullable.GetUnderlyingType(property.PropertyType)), null);
                    }
                }

                if (property.PropertyType.IsEnum) //属性类型是否表示枚举
                {
                    object enumName = Enum.ToObject(property.PropertyType, obj_propertyvalue);
                    t.GetProperty(as_propertyname).SetValue(obj, enumName, null); //获取枚举值，设置属性值
                }
                else if (property.PropertyType.IsGenericType) //属性类型是否表示泛型
                {
                    t.GetProperty(as_propertyname).SetValue(obj, obj_propertyvalue, null); //获取枚举值，设置属性值
                }
                else
                {
                    //普通属性
                    t.GetProperty(as_propertyname).SetValue(obj, obj_propertyvalue, null);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception($"设置属性值时出错：{obj.GetType().Name}:属性名:{as_propertyname},值:{aobj_propertyvalue},{ex.Message}");
            }

        }




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
