using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RUINORERP.Common.CollectionExtension
{
    /// <summary>
    /// 4.0以上的新语法 在编译层实现方法扩展
    /// 
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// 属性信息缓存,提升反射性能
        /// Key: Type.FullName + 属性名
        /// </summary>
        private static readonly ConcurrentDictionary<string, PropertyInfo> _propertyInfoCache = new ConcurrentDictionary<string, PropertyInfo>();

        /// <summary>
        /// 类型属性缓存
        /// Key: Type.FullName
        /// </summary>
        private static readonly ConcurrentDictionary<string, PropertyInfo[]> _typePropertiesCache = new ConcurrentDictionary<string, PropertyInfo[]>();

        //public static IQueryable<T> DataList<T>(this IQueryable<T> source, int page = 1, int rows = int.MaxValue, string sort = "Id", string order = "desc")
        //{
        //    return source.DataSort(sort, order).DataPage(page, rows);
        //}
        /// <summary>
        /// 获取一个对象的属性，对象如果为空，则返回属性的默认值
        /// </summary>
        /// <typeparam name="TSource">对象类型</typeparam>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="entity">需要判空的对象</param>
        /// <param name="func">执行函数</param>
        /// <param name="t">默认值</param>
        /// <returns></returns>
        public static T ProDefault<TSource, T>(this TSource entity, Func<TSource, T> func, T t = default(T)) where TSource : class
        {
            if (entity != null)
            {
                return func(entity);
            }
            return t;
        }

        //public static IQueryable<T> DataSort<T>(this IQueryable<T> source, string sortExpression, string order)
        //{
        //    try
        //    {
        //        var sortStrs = sortExpression.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //        var defaultOrder = "desc";
        //        if (sortStrs.Length == 2)
        //        {
        //            defaultOrder = sortStrs[1];
        //        }
        //        if (typeof(T).GetProperty("Id") != null && sortStrs[0] != "Id")
        //        {
        //            source = source.OrderBy(sortStrs[0] + " " + defaultOrder + ",Id desc");
        //        }
        //        else
        //        {
        //            source = source.OrderBy(sortStrs[0] + " " + defaultOrder);
        //        }
        //        return source;
        //    }
        //    catch (Exception)
        //    {
        //        return source;
        //    }
        //}

        public static IQueryable<T> DataPage<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber <= 1)
            {
                if (pageSize == int.MaxValue)
                {
                    return source;
                }
                return source.Take(pageSize);
            }
            else
            {
                return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            if (condition)
            {
                source = source.Where(predicate);
            }
            return source;
        }
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            if (condition)
            {
                source = source.Where(predicate);
            }
            return source;
        }

        ///// <summary>
        ///// 实现了List<T>的扩展
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static IList<T> Where<T>(this IList<T> list, Expression<Func<T, bool>> whereExp)
        //{
        //    return list.Where<T>(whereExp);
        //}


        /// <summary>
        /// 实现了List<T>的扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static BindingSortCollection<T> AsSortList<T>(this IList<T> list)
        {
            BindingSortCollection<T> BSL = new BindingSortCollection<T>();
            foreach (var item in list)
            {
                BSL.Add(item);
            }
            return BSL;
        }



        /// <summary>
        /// IList根据字段排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="orderName">排序属性名称</param>
        /// <param name="order">排序方式(asc|desc)</param>
        public static void OrderByField<T>(this IList<T> list, string orderName, string order = "asc")
        {
            List<T> newList = (List<T>)list;

            var types = typeof(T).GetProperties();
            var isExitsType = types.Where(k => k.Name == orderName);//是否存在属性
            if (isExitsType != null || isExitsType.Count() > 0)
            {
                var listType = isExitsType.FirstOrDefault().PropertyType.Name;
                newList.Sort(delegate (T a, T b)
                {
                    var value1 = a.GetType().GetProperty(orderName).GetValue(a, null);
                    var value2 = b.GetType().GetProperty(orderName).GetValue(b, null);

                    IComparable comparableObj = value1 as IComparable;
                    comparableObj = comparableObj ?? value2 as IComparable;

                    return comparableObj != null ? comparableObj.CompareTo(value2) : 0;
                });
                if (order == "desc")
                {
                    newList.Reverse();
                }
            }
        }


        /// <summary>
        /// 测试，参数中必需要有 this List<T> listpara
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listpara"></param>
        /// <returns></returns>
        public static List<T> AsMySortList<T>(this List<T> listpara)
        {
            IList<T> BSL = new BindingSortCollection<T>();
            return BSL as List<T>;
        }
        //}


        //public static class ListExtension
        //{
        public static BindingSortCollection<T> ToBindingSortCollection<T>(this List<T> @this)
        {
            BindingSortCollection<T> sortlist = new BindingSortCollection<T>();

            foreach (var item in @this)
            {
                sortlist.Add(item);
            }
            return sortlist;
        }
        /*

        public static DataTable ToDataTable<T>(this IList<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;
            if (varlist == null)
                return dtReturn;
            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue

                    (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        */



        public static DataTable ToDataTable<T>(this IList<T> varlist, List<Expression<Func<T, object>>> expColumns)
        {
            string[] cols = new string[expColumns.Count];
            for (int i = 0; i < expColumns.Count; i++)
            {
                MemberInfo minfo = expColumns[i].GetMemberInfo();
                string key = minfo.Name;
                cols[i] = key;
            }
            return varlist.ToDataTable<T>(cols);
        }

        /// <summary>
        /// 生成的列要在指定的列中，如果为null，则不指定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> varlist, params string[] columns)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null)
                return dtReturn;

            // 使用缓存的属性信息
            string typeKey = typeof(T).FullName;
            if (!_typePropertiesCache.TryGetValue(typeKey, out oProps))
            {
                oProps = typeof(T).GetProperties();
                _typePropertiesCache.TryAdd(typeKey, oProps);
            }

            foreach (PropertyInfo pi in oProps)
            {
                Type colType = pi.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                if (columns != null && columns.Length > 0)
                {
                    if (columns.Contains(pi.Name))
                    {
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                else
                {
                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            foreach (T rec in varlist)
            {
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    //包含在指定点中，才给值
                    if (dtReturn.Columns.Contains(pi.Name))
                    {
                        object value = pi.GetValue(rec, null);
                        dr[pi.Name] = value == null ? DBNull.Value : value;
                    }
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// 生成的列要在指定的列中，如果key不存在于实体，给空值。建字符列。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <param name="columnsCaption">key:en列名,value 中文 ，如果key不存在于实体，给空值</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> varlist, KeyValuePair<string, string>[] columnsCaption,
            bool IsShortDate = false)
        {
            DataTable dtReturn = new DataTable();

            //key是英文属性列名，value是列的类型。先把这一波全部列出来，再按参数添加
            List<KeyValuePair<string, Type>> colsTypeList = new List<KeyValuePair<string, Type>>();

            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null)
                return dtReturn;

            // 使用缓存的属性信息
            string typeKey = typeof(T).FullName;
            if (!_typePropertiesCache.TryGetValue(typeKey, out oProps))
            {
                oProps = typeof(T).GetProperties();
                _typePropertiesCache.TryAdd(typeKey, oProps);
            }

            foreach (PropertyInfo pi in oProps)
            {
                Type colType = pi.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                if (columnsCaption != null && columnsCaption.Length > 0)
                {
                    var col = columnsCaption.FirstOrDefault(c => c.Key == pi.Name);
                    if (col.Key != null)
                    {
                        colsTypeList.Add(new KeyValuePair<string, Type>(col.Key, colType));
                    }
                }

            }

            //添加列
            foreach (var item in columnsCaption)
            {
                DataColumn dc = null;
                KeyValuePair<string, Type> colType = colsTypeList.FirstOrDefault(c => c.Key == item.Key);
                if (colType.Key != null)
                {
                    #region 参数集合中的列存在于实体属性中时
                    if (colType.Value == typeof(DateTime) && IsShortDate)
                    {
                        dc = new DataColumn(item.Key, typeof(string));
                    }
                    else
                    {
                        dc = new DataColumn(item.Key, colType.Value);
                    }
                    #endregion
                }
                else
                {
                    dc = new DataColumn(item.Key, typeof(string));
                }
                dc.ColumnName = item.Key;
                dc.Caption = item.Value;
                dtReturn.Columns.Add(dc);
            }

            foreach (T rec in varlist)
            {
                DataRow dr = dtReturn.NewRow();
                foreach (DataColumn col in dtReturn.Columns)
                {
                    var colsType = colsTypeList.FirstOrDefault(c => c.Key == col.ColumnName);
                    if (colsType.Key != null)
                    {
                        if (colsType.Value == typeof(DateTime) && IsShortDate)
                        {
                            object value = rec.GetPropertyValue<T>(col.ColumnName);
                            dr[col.ColumnName] = value == null ? DBNull.Value : value;
                            if (dr[col.ColumnName] != DBNull.Value)
                            {
                                dr[col.ColumnName] = Convert.ToDateTime(dr[col.ColumnName]).ToString("yyyy-MM-dd");
                            }
                        }
                        else
                        {
                            object value = rec.GetPropertyValue<T>(col.ColumnName);
                            dr[col.ColumnName] = value == null ? DBNull.Value : value;
                        }
                    }
                    else
                    {
                        dr[col.ColumnName] = DBNull.Value;
                    }
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }


        /// <summary>
        /// 生成的列要在指定的列中，如果为null，则不指定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <param name="columns">key:en列名,value 中文 </param>
        /// <param name="AdditionalColumns">额外的列。通常如果有子树节点时。内容是动态的不是指定T的类型，所以额外添加，key:en列名,value 中文 </param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> varlist, KeyValuePair<string, string>[] columns,
            bool IsShortDate = false, params KeyValuePair<string, string>[] AdditionalColumns)
        {
            DataTable dtReturn = new DataTable();

            //key是英文属性列名，value是列的类型。先把这一波全部列出来，再按参数添加
            List<KeyValuePair<string, Type>> cols = new List<KeyValuePair<string, Type>>();

            // column names 
            PropertyInfo[] oProps = null;
            if (varlist == null)
                return dtReturn;

            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        if (columns != null || columns.Length > 0)
                        {
                            var col = columns.FirstOrDefault(c => c.Key == pi.Name);
                            if (col.Key != null)
                            {
                                if (colType == typeof(DateTime) && IsShortDate)
                                {
                                    DataColumn dc = new DataColumn(col.Value, typeof(string));
                                    dc.ColumnName = col.Key;
                                    dc.Caption = col.Value;
                                    dtReturn.Columns.Add(dc);
                                }
                                else
                                {
                                    DataColumn dc = new DataColumn(col.Value, colType);
                                    dc.ColumnName = col.Key;
                                    dc.Caption = col.Value;
                                    dtReturn.Columns.Add(dc);
                                }

                            }
                            else
                            {
                                //不存在这个实体中的字段时。只添加列名。值也会为空。
                                //DataColumn dc = new DataColumn(col.Value, typeof(string));
                                //dc.ColumnName = col.Key;
                                //dc.Caption = col.Value;
                                //dtReturn.Columns.Add(dc);
                            }

                        }
                        else
                        {
                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }

                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    //包含在指定点中，才给值
                    if (dtReturn.Columns.Contains(pi.Name))
                    {
                        if (colType == typeof(DateTime) && IsShortDate)
                        {
                            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                            dr[pi.Name] = Convert.ToDateTime(dr[pi.Name]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                                             (rec, null);
                        }
                    }
                }


                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }


    }
}
