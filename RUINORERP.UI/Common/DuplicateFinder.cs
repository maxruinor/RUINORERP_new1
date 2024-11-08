using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 重复对象的查找器，对象中的所有属性相同，则认为是重复对象
    /// </summary>
    public static class DuplicateFinder
    {
        public static IEnumerable<T> FindDuplicates<T>(IEnumerable<T> source)
        {
            var grouped = source.GroupBy(item => item, new EqualityComparer<T>());
            foreach (var group in grouped)
            {
                if (group.Count() > 1)
                {
                    foreach (var item in group)
                    {
                        yield return item;
                    }
                }
            }
        }

        private class EqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        public class CustomEqualityComparer<T> : IEqualityComparer<T> where T : class // 确保T是引用类型
        {
            private readonly string[] _ignoredProperties;

            public CustomEqualityComparer(params string[] ignoredProperties)
            {
                _ignoredProperties = ignoredProperties;
            }

            public bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null || y == null)
                    return false;

                if (x.GetType() != y.GetType()) return false;

                var properties = x.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (Array.IndexOf(_ignoredProperties, property.Name) >= 0)
                        continue;

                    var value1 = property.GetValue(x);
                    var value2 = property.GetValue(y);

                    // 如果属性值是可比较的值类型，直接比较
                    if (property.PropertyType.IsValueType && !property.PropertyType.IsEnum)
                    {
                        if (!Equals(value1, value2))
                            return false;
                    }
                    // 如果属性值是引用类型，需要检查是否为null，并递归比较
                    else
                    {
                        if ((value1 == null && value2 != null) || (value1 != null && value2 == null))
                            return false;
                        if (value1 != null && !Equals(value1, value2))
                            return false;
                    }
                }

                return true;
            }

            public int GetHashCode(T obj)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));

                var type = obj.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                int hash = 17;

                foreach (var property in properties)
                {
                    if (Array.IndexOf(_ignoredProperties, property.Name) >= 0)
                        continue;

                    var value = property.GetValue(obj);
                    if (value == null)
                        continue; // 跳过null值

                    if (property.PropertyType.IsValueType && !property.PropertyType.IsEnum)
                    {
                        hash = hash * 23 + value.GetHashCode();
                    }
                    else
                    {
                        // 对于引用类型，递归调用GetHashCode
                        hash = hash * 23 + (value?.GetHashCode() ?? 0);
                    }
                }
                //17 和 23 是常用的质数，用作哈希算法中的初始值和乘数。选择质数的原因是基于哈希算法的设计原则，以提高哈希码的分布均匀性，从而减少哈希冲突的概率。
                return hash;
            }
        }

        /// <summary>
        /// 对象是SugarColumn列组成的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class CustomSugarColumnEqualityComparer<T> : IEqualityComparer<T> where T : class
        {
            private readonly string[] _ignoredProperties;

            public CustomSugarColumnEqualityComparer(params string[] ignoredProperties)
            {
                _ignoredProperties = ignoredProperties;
            }

            /// <summary>
            /// 判断两个对象是否相等
            /// </summary>
            /// <param name="x">是一个只有一行数据的元组类型</param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(T x, T y)
            {
                if (x == null || y == null)
                    return false;


                if (ReferenceEquals(x, y))
                    return true;



                if (x.GetType() != y.GetType())
                    return false;

                var properties = x.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    // 检查是否应忽略此属性
                    if (Array.IndexOf(_ignoredProperties, property.Name) >= 0 || !IsSugarColumnProperty(property))
                        continue;

                    var value1 = property.GetValue(x);
                    var value2 = property.GetValue(y);

                    // 如果属性值是可比较的值类型，直接比较
                    if (property.PropertyType.IsValueType && !property.PropertyType.IsEnum)
                    {
                        if (!Equals(value1, value2))
                            return false;
                    }
                    // 如果属性值是引用类型，需要检查是否为null，并递归比较
                    else
                    {
                        if ((value1 == null && value2 != null) || (value1 != null && value2 == null))
                            return false;
                        if (value1 != null && !Equals(value1, value2))
                            return false;
                    }
                }

                return true;
            }

            public int GetHashCode(T obj)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));

                var type = obj.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                int hash = 17;

                foreach (var property in properties)
                {
                    // 检查是否应忽略此属性
                    if (Array.IndexOf(_ignoredProperties, property.Name) >= 0 || !IsSugarColumnProperty(property))
                        continue;

                    var value = property.GetValue(obj);
                    if (value == null)
                        continue; // 跳过null值

                    if (property.PropertyType.IsValueType && !property.PropertyType.IsEnum)
                    {
                        hash = hash * 23 + value.GetHashCode();
                    }
                    else
                    {
                        // 对于引用类型，递归调用GetHashCode
                        hash = hash * 23 + (value?.GetHashCode() ?? 0);
                    }
                }

                return hash;
            }

            private bool IsSugarColumnProperty(PropertyInfo property)
            {
                // 查找SugarColumn特性，并检查IsIgnore属性
                var sugarColumnAttribute = property.GetCustomAttributes(true)
                                                     .OfType<SugarColumn>()
                                                     .FirstOrDefault();
                // 如果找到了SugarColumn特性，并且IsIgnore为true，则忽略该属性
                if (sugarColumnAttribute != null && sugarColumnAttribute.IsIgnore)
                {
                    return false;
                }
                // 如果没有找到SugarColumn特性，或者IsIgnore为false，则考虑该属性
                return sugarColumnAttribute != null;
            }
        }


        public static class GroupingKeySelector
        {
            public static object CreateKeyForGrouping<T>(T item, string[] ignoredProperties)
            {
                // 使用反射获取对象的所有公共实例属性
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var keyProperties = properties
                    .Where(p => !ignoredProperties.Contains(p.Name) && IsSugarColumnProperty(p))
                    .ToArray();

                // 使用 Tuple 创建键
                var values = keyProperties.Select(p => p.GetValue(item)).ToArray();
                if (values.Length == 0)
                {
                    throw new InvalidOperationException("No properties available for grouping.");
                }
                else if (values.Length == 1)
                {
                    return values[0]; // 单个属性，直接返回属性值
                }
                else
                {
                    return Tuple.Create(values); // 多个属性，创建元组作为键
                }
            }

            private static bool IsSugarColumnProperty(PropertyInfo property)
            {
                var sugarColumnAttribute = property.GetCustomAttributes(true)
                    .OfType<SugarColumn>()
                    .FirstOrDefault();
                return sugarColumnAttribute != null && !sugarColumnAttribute.IsIgnore;
            }
        }
    }
}


