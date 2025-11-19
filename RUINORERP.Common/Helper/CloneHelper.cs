using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace RUINORERP.Common.Helper
{
    public static class CloneHelper
    {
        #region 反射赋值 意思是规范代码可以使用。暂时没有去验证了。by 2024-9-8

        /*
         * cndeng大佬你好，首先感蟹你的回复。
你的方法我试用了，出现的问题是：如果这个类的引用类型是字段而不是属性，它就无法复制。但如果把字段改成属性搞上get set外套，则可以复制成功。
如：把TestClass类中的属性  public List<int> Numbers { get; set; }  改成字段 public List<int> Numbers = [];   则无法复制此字段中的值。
现在的问题是我的代码已经写了很多了，如果要把一个类的所有字段都改成属性，许多地方都要有相应的修改，工程量有点大。
不知道大佬有没有办法优化一下这个方法？让他可以兼顾字段引用类型的复制。
         */

        public static T DeepCloneByReflection<T>(this T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            return (T)DeepCloneByReflection((object)source);
        }

        private static object DeepCloneByReflection(object source)
        {
            if (source == null)
                return null;

            Type type = source.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                return source;
            }
            else if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                var array = source as Array;
                var copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCloneByReflection(array.GetValue(i)), i);
                }
                return copied;
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                Type itemType = type.GetGenericArguments()[0];
                var collection = Activator.CreateInstance(type) as IList;
                var list = source as IList;
                foreach (var item in list)
                {
                    collection.Add(DeepCloneByReflection(item));
                }
                return collection;
            }
            else
            {
                var clone = Activator.CreateInstance(type);
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        var value = property.GetValue(source);
                        property.SetValue(clone, DeepCloneByReflection(value));
                    }
                }
                return clone;
            }
        }

        #endregion

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
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCloneObject_old<T>(this T source) where T : class
        {
            if (source == null)
                return default(T);
            // 尝试使用序列化实现深拷贝
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, source);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return (T)binaryFormatter.Deserialize(memoryStream);
                }
            }
            catch (SerializationException ex)
            {

                // 如果对象不可序列化，则回退到反射方式
            }
            // 反射方式深拷贝
            Type type = typeof(T);
            T target = (T)Activator.CreateInstance(type);

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
                    if (propertyInfo.GetValue(source) == null)
                    {
                        continue;
                    }
                    var nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                    try
                    {
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(source), nullableConverter.UnderlyingType), null);
                    }
                    catch (Exception ex)
                    {
                        var typeArray = propertyInfo.PropertyType.GetGenericArguments();
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(source), typeArray[0]), null);
                    }
                }
                else
                {

                    try
                    {
                        propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(source), propertyInfo.PropertyType), null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(propertyInfo.Name + "|" + propertyInfo.PropertyType.Name + "|" + source.ToString());
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }

                }
            }

            return instance;
        }

        /// <summary>
        /// 2025-07-18验证，无法复制二级的子集合属性 用：DeepCloneObject_maxnew 可以
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCloneObject<T>(this T source)
        {
            if (source == null)
                return default(T);

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, source);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return (T)binaryFormatter.Deserialize(memoryStream);
                }
            }
            catch (SerializationException)
            {
                // 序列化失败，回退到反射方式
            }

            Type type = typeof(T);
            T target = (T)Activator.CreateInstance(type);
            try
            {
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (!property.CanRead || !property.CanWrite)
                        continue;

                    object value = property.GetValue(source, null);
                    Type valueType = property.PropertyType;

                    if (valueType.IsPrimitive || valueType == typeof(string) || valueType == typeof(DateTime) || valueType == typeof(decimal))
                    {
                        property.SetValue(target, value, null);
                    }
                    else if (valueType.IsArray)
                    {
                        Array sourceArray = (Array)value;
                        if (sourceArray != null)
                        {
                            Type elementType = valueType.GetElementType();
                            Array targetArray = Array.CreateInstance(elementType, sourceArray.Length);
                            for (int i = 0; i < sourceArray.Length; i++)
                            {
                                object element = sourceArray.GetValue(i);
                                targetArray.SetValue(element?.DeepCloneObject(), i);
                            }
                            property.SetValue(target, targetArray, null);
                        }
                    }
                    else if (value is IEnumerable sourceEnumerable)
                    {
                        // 关键修复：处理集合类型
                        Type actualType = sourceEnumerable.GetType();

                        // 确定元素类型
                        Type elementType = GetEnumerableElementType(actualType);

                        // 创建目标集合实例
                        object targetCollection = CreateTargetCollection(actualType, elementType);
                        if (targetCollection == null) continue;

                        // 获取Add方法
                        MethodInfo addMethod = GetAddMethod(actualType, elementType);
                        if (addMethod == null) continue;

                        // 克隆并添加元素
                        foreach (var item in sourceEnumerable)
                        {
                            object clonedItem = item?.DeepCloneObject();
                            addMethod.Invoke(targetCollection, new[] { clonedItem });
                        }

                        property.SetValue(target, targetCollection, null);
                    }
                    else
                    {
                        property.SetValue(target, value?.DeepCloneObject(), null);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return target;
        }

        #region 深度复制克隆

        public static T DeepCloneObject_maxnew<T>(this T source, int maxDepth = 3)
        {
            if (source == null)
                return default;

            // 处理循环引用的字典
            var clonedObjects = new Dictionary<object, object>(new ReferenceEqualityComparer());
            return (T)DeepCloneInternal(source, clonedObjects, 0, maxDepth);
        }

        private static object DeepCloneInternal(object source, Dictionary<object, object> clonedObjects, int currentDepth, int maxDepth)
        {
            if (source == null)
                return null;

            // 检查最大递归深度
            if (currentDepth >= maxDepth)
                return source;

            // 检查是否已克隆过该对象
            if (clonedObjects.TryGetValue(source, out var cloned))
                return cloned;

            Type type = source.GetType();

            // 处理基本类型
            if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal))
                return source;

            // 处理数组
            if (type.IsArray)
            {
                var sourceArray = (Array)source;
                var elementType = type.GetElementType();
                var targetArray = Array.CreateInstance(elementType, sourceArray.Length);
                clonedObjects.Add(source, targetArray);

                for (int i = 0; i < sourceArray.Length; i++)
                {
                    try
                    {
                        var element = sourceArray.GetValue(i);
                        targetArray.SetValue(DeepCloneInternal(element, clonedObjects, currentDepth + 1, maxDepth), i);
                    }
                    catch (Exception ex)
                    {
                        // 记录异常并继续克隆其他元素
                       // .Instance.logger.LogError($"克隆数组元素时发生异常: {ex.Message}");
                        continue;
                    }
                }

                return targetArray;
            }

            // 处理集合类型
            if (source is IEnumerable enumerable)
            {
                // 获取实际集合类型和元素类型
                Type actualType = source.GetType();
                Type elementType = GetElementType(actualType) ?? typeof(object);

                // 创建目标集合
                object targetCollection = CreateCollectionInstance(actualType, elementType);
                if (targetCollection == null)
                    return source; // 无法创建则返回原对象

                clonedObjects.Add(source, targetCollection);

                // 获取Add方法
                MethodInfo addMethod = GetAddMethod(targetCollection.GetType(), elementType);
                if (addMethod == null)
                    return targetCollection;

                // 克隆并添加元素
                foreach (var item in enumerable)
                {
                    try
                    {
                        object clonedItem = DeepCloneInternal(item, clonedObjects, currentDepth + 1, maxDepth);
                        addMethod.Invoke(targetCollection, new[] { clonedItem });
                    }
                    catch (Exception ex)
                    {
                        // 记录异常并继续克隆其他元素
                        //MainForm.Instance.logger.LogError($"克隆集合元素时发生异常: {ex.Message}");
                        continue;
                    }
                }

                return targetCollection;
            }

            // 处理实体对象
            object target = Activator.CreateInstance(type);
            clonedObjects.Add(source, target);

            // 克隆属性
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                //// 跳过导航属性（避免循环）
                //if (property.Name.StartsWith("tb_") || property.PropertyType.Namespace == type.Namespace)
                //    continue;

                try
                {
                    object value = property.GetValue(source);
                    object clonedValue = DeepCloneInternal(value, clonedObjects, currentDepth + 1, maxDepth);
                    property.SetValue(target, clonedValue);
                }
                catch (Exception ex)
                {
                    // 记录异常并继续克隆其他属性
                    //logger.LogError($"克隆属性 {property.Name} 时发生异常: {ex.Message}");
                    continue;
                }
            }

            return target;
        }


        // 辅助类：解决字典比较引用相等性问题
        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y) => ReferenceEquals(x, y);
            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        // 获取集合元素类型
        private static Type GetElementType(Type collectionType)
        {
            // 处理泛型集合
            if (collectionType.IsGenericType)
            {
                var genericArgs = collectionType.GetGenericArguments();
                if (genericArgs.Length == 1)
                    return genericArgs[0];
            }

            // 处理数组
            if (collectionType.IsArray)
                return collectionType.GetElementType();

            // 处理IEnumerable<T>
            var iEnumerable = collectionType.GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType &&
                                   t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return iEnumerable?.GetGenericArguments()[0];
        }

        // 创建集合实例
        private static object CreateCollectionInstance(Type collectionType, Type elementType)
        {
            // 处理具体类型（如List<T>）
            if (collectionType.IsClass && !collectionType.IsAbstract)
            {
                try { return Activator.CreateInstance(collectionType); }
                catch { /* 继续尝试其他方式 */ }
            }

            // 处理接口（如IList<T>）
            if (collectionType.IsInterface && collectionType.IsGenericType)
            {
                var genericType = collectionType.GetGenericTypeDefinition();
                if (genericType == typeof(IList<>) || genericType == typeof(ICollection<>))
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            }

            // 默认创建List<T>
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
        }

      
        // 获取Add方法
        private static MethodInfo GetAddMethod(Type collectionType, Type elementType)
        {
            // 检查ICollection<T>.Add方法
            var collectionInterface = collectionType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));

            if (collectionInterface != null)
            {
                return collectionInterface.GetMethod("Add");
            }

            // 查找参数类型匹配的Add方法
            return collectionType.GetMethod("Add", new[] { elementType })
                ?? collectionType.GetMethod("Add", new[] { typeof(object) });
        }
        #endregion


        // 获取集合元素类型
        private static Type GetEnumerableElementType(Type collectionType)
        {
            // 检查是否实现了IEnumerable<T>
            var enumerableInterface = collectionType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumerableInterface?.GetGenericArguments()[0] ?? typeof(object);
        }

        // 创建目标集合实例
        private static object CreateTargetCollection(Type collectionType, Type elementType)
        {
            try
            {
                // 尝试使用默认构造函数
                return Activator.CreateInstance(collectionType);
            }
            catch
            {
                // 回退到创建List<T>实例（如果可能）
                if (typeof(IList).IsAssignableFrom(collectionType))
                {
                    Type listType = typeof(List<>).MakeGenericType(elementType);
                    return Activator.CreateInstance(listType);
                }
            }
            return null;
        }



        /// <summary>
        /// 在 .NET 中，有几个现成的库可以帮助简化深度复制对象的实现：

        //        AutoMapper：这是一个流行的对象到对象映射器，它不仅可以用于映射，还可以用于深度复制对象。使用 AutoMapper，你可以配置映射关系，然后使用它来复制对象。它的性能也相当不错，对于大多数应用场景来说是一个不错的选择。

        //DeepCloner：这是一个专门用于深度复制的库，它提供了一个简单的 API 来克隆对象。DeepCloner 可以处理复杂的对象图，包括集合和循环引用。

        //FastDeepCloner：这是一个高性能的深度克隆库，它提供了比传统二进制序列化更快的克隆方法。FastDeepCloner 支持多种.NET 框架，并且可以通过添加[FastDeepClonerIgnore] 属性来忽略特定属性的克隆。

        //System.Text.Json：如果你不需要安装额外的库，可以考虑使用.NET 自带的 System.Text.Json 库来进行深度复制。这种方法不需要序列化和反序列化整个对象图，但是它可能不适用于所有类型的复杂对象。

        //XmlSerializer 或 DataContractSerializer：这些是.NET 框架提供的序列化器，它们可以用来深度复制对象。这些方法需要你的类和成员标记为[Serializable] 或 [DataContract]，并且可能需要一些额外的配置。

        //BinaryFormatter：这是一个旧的.NET 序列化器，它可以用于深度复制对象。但是，从.NET Core 3.0 开始，BinaryFormatter 已被弃用，因为它存在安全问题。

        //选择哪个库取决于你的具体需求，包括性能要求、是否需要处理特殊类型（如 byte[] 或 Image）、以及是否愿意引入外部依赖。在处理 byte[] 数组时，通常需要手动复制数组；而对于 Image 类型的对象，你可能需要调用特定的复制方法或重新从源创建实例。如果需要忽略某个属性，可以在克隆过程中检查属性名并跳过它，或者使用库提供的忽略属性的功能（如果支持）。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ignoreProperties"></param>
        /// <returns></returns>
        public static T DeepCloneObjectAdv<T>(this T t, List<string> ignoreProperties = null) where T : class
        {
            var instance = Activator.CreateInstance<T>();
            var propertyInfos = instance.GetType().GetProperties();

            // 可以在这里定义一个忽略属性的列表
            // var ignoreProperties = new List<string> { "PropertyNameToIgnore" }; // 替换为实际要忽略的属性名

            foreach (var propertyInfo in propertyInfos)
            {
                if (ignoreProperties != null)
                {
                    if (ignoreProperties.Contains(propertyInfo.Name)) // 检查是否需要忽略该属性
                    {
                        continue;
                    }
                }

                if (!propertyInfo.CanWrite)
                {
                    continue;
                }

                var propertyValue = propertyInfo.GetValue(t);

                if (propertyValue == null)
                {
                    continue;
                }

                if (propertyInfo.PropertyType == typeof(byte[]))
                {
                    // 处理 byte[] 数组
                    byte[] byteArray = (byte[])propertyValue;
                    byte[] clonedByteArray = new byte[byteArray.Length];
                    Array.Copy(byteArray, clonedByteArray, byteArray.Length);
                    propertyInfo.SetValue(instance, clonedByteArray);
                }
                else if (propertyInfo.PropertyType == typeof(System.Drawing.Image))
                {
                    // 处理 Image 类型
                    System.Drawing.Image image = (System.Drawing.Image)propertyValue;
                    System.Drawing.Image clonedImage = (System.Drawing.Image)image.Clone();
                    propertyInfo.SetValue(instance, clonedImage);
                }
                else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    var nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                    try
                    {
                        // 检查底层类型是否为值类型或字符串，避免复杂类型转换
                        var underlyingType = nullableConverter.UnderlyingType;
                        if (underlyingType.IsValueType || underlyingType == typeof(string))
                        {
                            propertyInfo.SetValue(instance, Convert.ChangeType(propertyValue, underlyingType), null);
                        }
                        else
                        {
                            // 对于复杂类型，直接赋值而不转换
                            propertyInfo.SetValue(instance, propertyValue, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        var typeArray = propertyInfo.PropertyType.GetGenericArguments();
                        var targetType = typeArray[0];
                        // 检查目标类型是否为值类型或字符串，避免复杂类型转换
                        if (targetType.IsValueType || targetType == typeof(string))
                        {
                            propertyInfo.SetValue(instance, Convert.ChangeType(propertyValue, targetType), null);
                        }
                        else
                        {
                            // 对于复杂类型，直接赋值而不转换
                            propertyInfo.SetValue(instance, propertyValue, null);
                        }
                    }
                }
                else if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    // 对于值类型和字符串，使用类型转换
                    propertyInfo.SetValue(instance, Convert.ChangeType(propertyValue, propertyInfo.PropertyType), null);
                }
                else
                {
                    // 对于复杂类型（如导航属性），直接赋值而不转换
                    propertyInfo.SetValue(instance, propertyValue, null);
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
                        // 检查底层类型是否为值类型或字符串，避免复杂类型转换
                        var underlyingType = nullableConverter.UnderlyingType;
                        if (underlyingType.IsValueType || underlyingType == typeof(string))
                        {
                            propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), underlyingType), null);
                        }
                        else
                        {
                            // 对于复杂类型，直接赋值而不转换
                            propertyInfo.SetValue(instance, propertyInfo.GetValue(t), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        var typeArray = propertyInfo.PropertyType.GetGenericArguments();
                        var targetType = typeArray[0];
                        // 检查目标类型是否为值类型或字符串，避免复杂类型转换
                        if (targetType.IsValueType || targetType == typeof(string))
                        {
                            propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), targetType), null);
                        }
                        else
                        {
                            // 对于复杂类型，直接赋值而不转换
                            propertyInfo.SetValue(instance, propertyInfo.GetValue(t), null);
                        }
                    }
                }
                else if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    // 对于值类型和字符串，使用类型转换
                    propertyInfo.SetValue(instance, Convert.ChangeType(propertyInfo.GetValue(t), propertyInfo.PropertyType), null);
                }
                else
                {
                    // 对于复杂类型（如导航属性），直接赋值而不转换
                    propertyInfo.SetValue(instance, propertyInfo.GetValue(t), null);
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
                    // 检查属性是否可写
                    if (!propertyInfo.CanWrite)
                    {
                        continue; // 如果不可写，跳过该属性
                    }
                    var sourceValue = propertyInfo.GetValue(item);

                    if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        var nullableConverter = new NullableConverter(propertyInfo.PropertyType);

                        // 如果源值为 null，则直接设置为 null
                        if (sourceValue == null)
                        {
                            propertyInfo.SetValue(model, null, null);
                        }
                        else
                        {
                            // 转换为可空类型的底层类型
                            var convertedValue = Convert.ChangeType(sourceValue, nullableConverter.UnderlyingType);
                            propertyInfo.SetValue(model, convertedValue, null);
                        }
                    }
                    else
                    {
                        // 如果源值为 null，则直接设置为 null
                        if (sourceValue == null)
                        {
                            propertyInfo.SetValue(model, null, null);
                        }
                        else
                        {
                            // 转换为目标类型的值
                            var convertedValue = Convert.ChangeType(sourceValue, propertyInfo.PropertyType);
                            propertyInfo.SetValue(model, convertedValue, null);
                        }
                    }
                }

                result.Add(model);
            }

            return result;
        }
    }
}
