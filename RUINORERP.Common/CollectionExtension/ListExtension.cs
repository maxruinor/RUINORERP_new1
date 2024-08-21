using System;
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

namespace RUINORERP.Common.CollectionExtension
{
    /// <summary>
    /// 4.0以上的新语法 在编译层实现方法扩展
    /// 
    /// </summary>
    public static class ListExtension
    {
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
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    //包含在指定点中，才给值
                    if (dtReturn.Columns.Contains(pi.Name))
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                    }
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

    }
}
