using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace RUINORERP.Common.Helper
{
    public static class CloneHelper
    {


        /// <summary>
        /// 通过遍历属性赋值
        /// </summary>
        /// <returns></returns>
        public static void SetValues<T>(object newItem, object oldItem)
        {

            //System.Reflection.PropertyInfo[] properties = oldItem.GetType().GetProperties();
            System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (!properties[i].CanWrite)
                {
                    continue;
                }
                //properties[i].SetValue(newItem, (i + 1));
                var propertyValue = properties[i].GetValue(oldItem, null);
                //if (propertyValue == null)
                //    continue;
                properties[i].SetValue(newItem, propertyValue, null);

                //CopyField(oldItem, newItem, properties[i].Name);
            }
        }


        private static void CopyField(object source, object target, string fieldName)
        {
            if (source == null) return;
            if (target == null) return;



            var sourceField = source.GetType().GetField(fieldName);
            if (sourceField != null)
            {
                var targetField = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (targetField != null)
                {
                    targetField.SetValue(target, sourceField.GetValue(source));
                }
            }
        }


        /// <summary>
        /// 传入类型B的对象b，将b与a相同名称的值进行赋值给创建的a中        
        /// </summary>
        /// <typeparam name="A">类型A</typeparam>
        /// <typeparam name="B">类型B</typeparam>
        /// <param name="b">类型为B的参数b</param>
        /// <returns>拷贝b中相同属性的值的a</returns>
        public static A MapperTwo<A, B>(B b)
        {
            A a = Activator.CreateInstance<A>();
            try
            {
                Type Typeb = typeof(B);//获得类型  
                Type Typea = typeof(A);
                foreach (PropertyInfo ap in Typea.GetProperties())
                {
                    System.Reflection.PropertyInfo bp = Typeb.GetProperty(ap.Name); //获取指定名称的属性
                    if (bp != null) //如果B对象也有该属性
                    {
                        if (ap.GetSetMethod() != null) //判断A对象是否有能用Set方法
                        {
                            if (bp.GetGetMethod() != null) //判断B对象是否有能用Get方法
                            {
                                ap.SetValue(a, bp.GetValue(b, null), null);//获得b对象属性的值复制给a对象的属性   
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return a;
        }

        public static object Clone(object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();

            var result = type.InvokeMember("", BindingFlags.CreateInstance, null, obj, null);

            foreach (var pi in properties)
            {
                if (pi.CanWrite)
                {
                    var value = pi.GetValue(obj, null);
                    pi.SetValue(result, value, null);
                }
            }

            return result;
        }

        /// <summary>
        /// 对象Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T DeepCloneObject<T>(this T t) where T : class
        {
            var instance = Activator.CreateInstance<T>();
            var propertyInfos = instance.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                //如果属性没有设置的方法则直接退出
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (propertyInfo.GetValue(t) == null)
                    {
                        continue;
                    }
                    var nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                    try
                    {
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), nullableConverter.UnderlyingType), null);
                    }
                    catch (Exception ex)
                    {
                        var typeArray = propertyInfo.PropertyType.GetGenericArguments();
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), typeArray[0]), null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), propertyInfo.PropertyType), null);
                }
            }

            return instance;
        }


        /// <summary>
        /// 对象Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T DeepCloneObject<T>(object t) 
        {
            var instance = Activator.CreateInstance<T>();
            var propertyInfos = t.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                //如果属性没有设置的方法则直接退出
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (propertyInfo.GetValue(t) == null)
                    {
                        continue;
                    }
                    var nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                    try
                    {
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), nullableConverter.UnderlyingType), null);
                    }
                    catch (Exception ex)
                    {
                        var typeArray = propertyInfo.PropertyType.GetGenericArguments();
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), typeArray[0]), null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), propertyInfo.PropertyType), null);
                }
            }

            return instance;
        }
        /// <summary>
        /// List的Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        public static IList<T> DeepCloneList<T>(this IList<T> tList) where T : class
        {
            var result = new List<T>();

            foreach (var item in tList)
            {
                var model = Activator.CreateInstance<T>();
                var propertyInfos = model.GetType().GetProperties();

                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        var nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                        propertyInfo.SetValue(model, Convert.ChangeType(propertyInfo.GetValue(item), nullableConverter.UnderlyingType), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(model, Convert.ChangeType(propertyInfo.GetValue(item), propertyInfo.PropertyType), null);
                    }
                }

                result.Add(model);
            }

            return result;
        }

    }
}
