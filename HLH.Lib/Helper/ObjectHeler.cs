﻿using System;
using System.Reflection;

namespace HLH.Lib.Helper
{
    public class ObjectHeler
    {
        /// <summary>
        /// 判断两个相同引用类型的对象的属性值是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">对象1</param>
        /// <param name="obj2">对象2</param>
        /// <param name="type">按type类型中的属性进行比较</param>
        /// <returns></returns>
        public static bool CompareProperties<T>(T obj1, T obj2, Type type)
        {
            //为空判断
            if (obj1 == null && obj2 == null)
                return true;
            else if (obj1 == null || obj2 == null)
                return false;

            Type t = type;
            PropertyInfo[] props = t.GetProperties();
            foreach (var po in props)
            {
                if (IsCanCompare(po.PropertyType))
                {
                    if (po.GetValue(obj1) == null && po.GetValue(obj2) == null)
                    {
                        continue;
                    }
                    if (po.GetValue(obj1) != null && po.GetValue(obj2) == null)
                    {
                        return false;
                    }
                    if (po.GetValue(obj1) == null && po.GetValue(obj2) != null)
                    {
                        return false;
                    }

                    if (!po.GetValue(obj1).Equals(po.GetValue(obj2)))
                    {
                        return false;
                    }
                }
                else
                {
                    return CompareProperties(po.GetValue(obj1), po.GetValue(obj2), po.PropertyType);
                }
            }

            return true;
        }

        /// <summary>
        /// 该类型是否可直接进行值的比较
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsCanCompare(Type t)
        {
            if (t.IsValueType)
            {
                return true;
            }
            else
            {
                //String是特殊的引用类型，它可以直接进行值的比较
                if (t.FullName == typeof(String).FullName)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
