using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace HLH.Lib.Helper
{
    //使用实例
    /*
     * //  实现深复制-方式1：依次赋值和实例化
            //DeepCopy newObj = new DeepCopy();
            //newObj.a = new A();
            //newObj.a.message = this.a.message;
            //newObj.i = this.i;

            //return newObj;
            // 实现深复制-方式2：序列化/反序列化
            BinaryFormatter bf = new BinaryFormatter(); 
            MemoryStream ms = new MemoryStream(); 
            bf.Serialize(ms, this); 
            ms.Position = 0; 
            return bf.Deserialize(ms);
     */
    public class DeepCopyHelper
    {
        // 用一个字典来存放每个对象的反射次数来避免反射代码的循环递归
        static readonly Dictionary<Type, int> TypereflectionCountDic = new Dictionary<Type, int>();
        //static object _deepCopyDemoClasstypeRef = null;
        // 利用反射实现深拷贝
        /// <summary>
        /// 可能会出错
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyWithReflection<T>(T obj)
        {
            Type type = obj.GetType();
            // 如果是字符串或值类型则直接返回
            if (obj is string || type.IsValueType) return obj;
            if (type.IsArray)
            {
                if (type.FullName != null)
                {
                    Type elementType = Type.GetType(type.FullName.Replace("[]", string.Empty));
                    var array = obj as Array;
                    if (array != null)
                    {
                        Array copied = Array.CreateInstance(elementType ?? throw new InvalidOperationException(), array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                            copied.SetValue(DeepCopyWithReflection(array.GetValue(i)), i);
                        }
                        return (T)Convert.ChangeType(copied, obj.GetType());
                    }
                }
            }

            object retval = Activator.CreateInstance(obj.GetType());

            PropertyInfo[] properties = obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(obj, null);
                if (propertyValue == null)
                    continue;
                property.SetValue(retval, DeepCopyWithReflection(propertyValue), null);
            }
            return (T)retval;
        }



        public static T DeepCopyWithReflection_Second<T>(T obj)
        {
            Type type = obj.GetType();

            // 如果是字符串或值类型则直接返回
            if (obj is string || type.IsValueType) return obj;
            if (type.IsArray)
            {
                if (type.FullName != null)
                {
                    Type elementType = Type.GetType(type.FullName.Replace("[]", string.Empty));
                    var array = obj as Array;
                    if (array != null)
                    {
                        Array copied = Array.CreateInstance(elementType ?? throw new InvalidOperationException(), array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                            copied.SetValue(DeepCopyWithReflection_Second(array.GetValue(i)), i);
                        }
                        return (T)Convert.ChangeType(copied, obj.GetType());
                    }
                }
            }

            // 对于类类型开始记录对象反射的次数
            int reflectionCount = Add(TypereflectionCountDic, obj.GetType());
            if (reflectionCount > 1)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());

            PropertyInfo[] properties = obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(obj, null);
                if (propertyValue == null)
                    continue;
                property.SetValue(retval, DeepCopyWithReflection_Second(propertyValue), null);
            }
            return (T)retval;
        }



        //public static T DeepCopyWithReflection_Third<T>(T obj)
        //{
        //    Type type = obj.GetType();
        //    // 如果是字符串或值类型则直接返回
        //    if (obj is string || type.IsValueType) return obj;
        //    if (type.IsArray)
        //    {
        //        Type elementType = Type.GetType(type.FullName.Replace("[]", string.Empty));
        //        var array = obj as Array;
        //        Array copied = Array.CreateInstance(elementType, array.Length);
        //        for (int i = 0; i < array.Length; i++)
        //        {
        //            copied.SetValue(DeepCopyWithReflection_Second(array.GetValue(i)), i);
        //        }
        //        return (T) Convert.ChangeType(copied, obj.GetType());
        //    }
        //    int reflectionCount = Add(typereflectionCountDic, obj.GetType());
        //    if (reflectionCount > 1 && obj.GetType() == typeof(DeepCopyDemoClass))
        //        return (T) DeepCopyDemoClasstypeRef; // 返回deepCopyClassB对象
        //    object retval = Activator.CreateInstance(obj.GetType());
        //    if (retval.GetType() == typeof(DeepCopyDemoClass))
        //        DeepCopyDemoClasstypeRef = retval; // 保存一开始创建的DeepCopyDemoClass对象
        //    PropertyInfo[] properties = obj.GetType().GetProperties(
        //        BindingFlags.Public | BindingFlags.NonPublic
        //        | BindingFlags.Instance | BindingFlags.Static);
        //    foreach (var property in properties)
        //    {
        //        var propertyValue = property.GetValue(obj, null);
        //        if (propertyValue == null)
        //            continue;
        //        property.SetValue(retval, DeepCopyWithReflection_Third(propertyValue), null);
        //    }
        //    return (T) retval;
        //}

        //private static T SetArrayObject<T>(T arrayObj)
        //{
        //    Type elementType = Type.GetType(arrayObj.GetType().FullName.Replace("[]", string.Empty));
        //    var array = arrayObj as Array;
        //    Array copied = Array.CreateInstance(elementType, array.Length);
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        copied.SetValue(DeepCopyWithReflection_Third(array.GetValue(i)), i);
        //    }
        //    return (T) Convert.ChangeType(copied, arrayObj.GetType());
        //}

        private static int Add(Dictionary<Type, int> dict, Type key)
        {
            if (key == typeof(string) || key.IsValueType) return 0;
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, 1);
                return dict[key];
            }
            dict[key] += 1;
            return dict[key];
        }

        // 利用XML序列化和反序列化实现
        public static T DeepCopyWithXmlSerializer<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        // 利用二进制序列化和反序列实现（亲测有用）
        public static T DeepCopyWithBinarySerialize<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                // 序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        // 利用DataContractSerializer序列化和反序列化实现
        //public static T DeepCopy<T>(T obj)
        //{
        //    object retval;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        DataContractSerializer ser = new DataContractSerializer(typeof(T));
        //        ser.WriteObject(ms, obj);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        retval = ser.ReadObject(ms);
        //        ms.Close();
        //    }
        //    return (T) retval;
        //}
        // 表达式树实现
        // ....
        public static class TransExpV2<TIn, TOut>
        {
            private static readonly Func<TIn, TOut> cache = GetFunc();
            private static Func<TIn, TOut> GetFunc()
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();

                foreach (var item in typeof(TOut).GetProperties())
                {
                    if (!item.CanWrite)
                        continue;

                    MemberExpression property =
                        Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));

                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                MemberInitExpression memberInitExpression =
                    Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());

                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression,
                    new ParameterExpression[] { parameterExpression });
                return lambda.Compile();
            }

            public static TOut Trans(TIn tIn)
            {
                return cache(tIn);
            }
        }
    }
}
