using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace RUINORERP.Common.CollectionExtension
{

    /// <summary> 
    /// 泛型集合与DataSet/DataTable互相转换 
    /// </summary> 
    public static class IListDataSet
    {


        /// <summary>
        /// List<>对象转为表格 by 2020-8-7
        /// </summary>
        /// <param name="obj">必需为List相关集合或表</param>
        /// <returns></returns>
        public static DataTable ObjectToTable(object obj)
        {
            try
            {
                Type t;
                if (obj.GetType().IsGenericType)
                {
                    t = obj.GetType().GetGenericTypeDefinition();
                }
                else
                {
                    t = obj.GetType();
                }
                if (t == typeof(List<>) || t == typeof(BindingSortCollection<>) ||
                    t == typeof(IEnumerable<>))
                {
                    DataTable dt = new DataTable();
                    IEnumerable<object> lstenum = obj as IEnumerable<object>;
                    if (lstenum.Count() > 0)
                    {
                        var ob1 = lstenum.GetEnumerator();
                        ob1.MoveNext();
                        foreach (var item in ob1.Current.GetType().GetProperties())
                        {
                            //如果是在我的框架中 生成的 实体 有 SEQNO ，这个列 不处理。不需要生成到表格中。排除它。
                            if (item.Name == "SEQNO")
                            {
                                continue;
                            }

                            // 当字段类型是Nullable<>时
                            Type colType = item.PropertyType;
                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {

                                colType = colType.GetGenericArguments()[0];

                            }

                            DataColumn dc = new DataColumn();
                            dc.ColumnName = item.Name;
                            dc.DataType = colType;
                            dt.Columns.Add(dc);
                            //dt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                        }
                        //数据
                        foreach (var item in lstenum)
                        {
                            DataRow row = dt.NewRow();
                            foreach (var sub in item.GetType().GetProperties())
                            {
                                //如果是在我的框架中 生成的 实体 有 SEQNO ，这个列 不处理。不需要生成到表格中。排除它。
                                if (sub.Name == "SEQNO")
                                {
                                    continue;
                                }

                                //处理DB.null
                                row[sub.Name] = sub.GetValue(item, null) == null ? DBNull.Value : sub.GetValue(item, null);
                                //row[sub.Name] = sub.GetValue(item, null);
                            }
                            dt.Rows.Add(row);
                        }
                        return dt;
                    }
                }
                else if (t == typeof(DataTable))
                {
                    return (DataTable)obj;
                }
                else   //(t==typeof(Object))
                {
                    DataTable dt = new DataTable();
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        dt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                    }
                    DataRow row = dt.NewRow();
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        //dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                        row[item.Name] = item.GetValue(obj, null);
                    }
                    dt.Rows.Add(row);
                    return dt;
                }

            }
            catch (Exception ex)
            {
            }
            return null;
        }





        /// <summary>
        /// DataTable转换List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(DataTable dt)
        {
            List<T> ret = new List<T>();
            Type type = typeof(T);
            List<string> lstColumns = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                PropertyInfo[] proInfo = type.GetProperties();
                T entity = Activator.CreateInstance<T>();
                foreach (PropertyInfo p in proInfo)
                {
                    if (!dt.Columns.Contains(p.Name) || dr[p.Name] == null || dr[p.Name] == DBNull.Value)
                        continue;
                    if (p.PropertyType == typeof(DateTime) && Convert.ToDateTime(dr[p.Name]) < Convert.ToDateTime("1753-01-01"))
                        continue;
                    try
                    {
                        object obj = Convert.ChangeType(dr[p.Name], p.PropertyType);
                        p.SetValue(entity, obj, null);
                    }
                    catch
                    {

                    }
                }
                ret.Add(entity);
            }
            return ret;
        }

        /// <summary>
        /// List<T>转换DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToTable<T>(List<T> list)
        {
            Type type = typeof(T);
            PropertyInfo[] proInfo = type.GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo p in proInfo)
            {
                //类型存在Nullable<Type>时，需要进行以下处理，否则异常
                Type t = p.PropertyType;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                    t = t.GetGenericArguments()[0];
                dt.Columns.Add(p.Name, t);
            }
            foreach (T t in list)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo p in proInfo)
                {
                    object obj = p.GetValue(t);
                    if (obj == null) continue;
                    if (p.PropertyType == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                        continue;
                    dr[p.Name] = obj;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static T TableRowToEntity<T>(DataTable dt, int rowIndex)
        {
            Type type = typeof(T);
            T entity = Activator.CreateInstance<T>();
            if (dt == null) return entity;
            DataRow dr = dt.Rows[rowIndex];
            PropertyInfo[] proInfo = type.GetProperties();
            foreach (PropertyInfo p in proInfo)
            {
                if (!dt.Columns.Contains(p.Name) || dr[p.Name] == null || dr[p.Name] == DBNull.Value)
                    continue;
                if (p.PropertyType == typeof(DateTime) && Convert.ToDateTime(dr[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    continue;
                try
                {
                    object obj = Convert.ChangeType(dr[p.Name], p.PropertyType);
                    p.SetValue(entity, obj, null);
                }
                catch
                {

                }
            }
            return entity;
        }

        /// <summary>
        /// 示例:
        /// var query = from....;
        /// DataTable dt = query.ToDataTable(rec => new object[] { query });
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        static public DataTable ToDataTable<T>(this IEnumerable<T> varlist, CreateRowDelegate<T> fn)
        {

            DataTable dtReturn = new DataTable();

            // column names

            PropertyInfo[] oProps = null;

            // Could add a check to verify that there is an element 0

            foreach (T rec in varlist)
            {

                // Use reflection to get property names, to create table, Only first time, others will follow

                if (oProps == null)
                {

                    oProps = ((Type)rec.GetType()).GetProperties();

                    foreach (PropertyInfo pi in oProps)
                    {
                        // 当字段类型是Nullable<>时
                        Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {

                            colType = colType.GetGenericArguments()[0];

                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));

                    }

                }

                DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
                {

                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);

                }

                dtReturn.Rows.Add(dr);

            }

            return (dtReturn);

        }

        public delegate object[] CreateRowDelegate<T>(T t);



        public static DataTable ListToDataTable<T>(List<T> entitys)
        {

            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }

            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static DataSet ListToDataSet<T>(IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (PropertyInfo propInfo in elementType.GetProperties())
            {
                DataColumn dc = new DataColumn();
                dc.AllowDBNull = true;
                dc.ColumnName = propInfo.Name;
                dc.DataType = propInfo.PropertyType;

                t.Columns.Add(dc);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();
                foreach (PropertyInfo propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }
                t.Rows.Add(row);
            }

            return ds;
        }

        /// <summary> 
        /// 集合装换DataSet 
        /// </summary> 
        /// <param name="list">集合</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:08 HPDV2806 
        public static DataSet ToDataSet(IList p_List)
        {
            DataSet result = new DataSet();
            DataTable _DataTable = new DataTable();
            if (p_List.Count > 0)
            {
                PropertyInfo[] propertys = p_List[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    _DataTable.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < p_List.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(p_List[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(_DataTable);
            return result;
        }

        /// <summary> 
        /// 泛型集合转换DataSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="list">泛型集合</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:43 HPDV2806 
        public static DataSet ToDataSet<T>(IList<T> list)
        {
            return ToDataSet<T>(list, null);
        }


        /// <summary> 
        /// 泛型集合转换DataSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_List">泛型集合</param> 
        /// <param name="p_PropertyName">待转换属性名数组</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:44 HPDV2806 
        public static DataSet ToDataSet<T>(IList<T> p_List, params string[] p_PropertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (p_PropertyName != null)
                propertyNameList.AddRange(p_PropertyName);

            DataSet result = new DataSet();
            DataTable _DataTable = new DataTable();
            if (p_List.Count > 0)
            {
                PropertyInfo[] propertys = p_List[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        // 没有指定属性的情况下全部属性都要转换 
                        _DataTable.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            _DataTable.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < p_List.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(p_List[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(p_List[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(_DataTable);
            return result;
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_DataSet">DataSet</param> 
        /// <param name="p_TableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:46 HPDV2806 
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, int p_TableIndex)
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化 
            IList<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_DataSet">DataSet</param> 
        /// <param name="p_TableName">待转换数据表名称</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:47 HPDV2806 
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, string p_TableName)
        {
            int _TableIndex = 0;
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(p_TableName))
                return null;
            for (int i = 0; i < p_DataSet.Tables.Count; i++)
            {
                // 获取Table名称在Tables集合中的索引值 
                if (p_DataSet.Tables[i].TableName.Equals(p_TableName))
                {
                    _TableIndex = i;
                    break;
                }
            }
            return DataSetToIList<T>(p_DataSet, _TableIndex);
        }

        public static List<T> ConvertDataSetToEntities<T>(DataSet ds)
        {
            //设置DataSet根节点节点名
            //ds.DataSetName = GetType(List<T>).Name;
            ////设置DataTable节点名(实体类名)
            //ds.Tables[0].TableName = GetType(List<T>).Name;
            //打开DataSet的XML内容字符串读取流
            System.IO.StringReader sr = new System.IO.StringReader(ds.GetXml());
            //创建XML序列化类
            XmlSerializer xmlser = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            //将XML反序列化为对象列表
            List<T> list = xmlser.Deserialize(sr) as List<T>;
            sr.Close();
            return list;

        }

        public static DataSet ConvertEntitesToDataSet<T>(List<T> list)
        {
            //创建XML序列化类

            XmlSerializer xmlser = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            //创建XML字符串构造器

            System.Text.StringBuilder sb = new StringBuilder();
            //创建XML写入流
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            //序列化实体列表到XML写入流
            xmlser.Serialize(sw, list);
            sw.Close();
            //创建XML对象
            System.IO.StringReader sr = new System.IO.StringReader(sb.ToString());
            DataSet ds = new DataSet();
            //通过XML读取内容到DataSet
            ds.ReadXml(sr);
            sr.Close();
            return ds;
        }
    }
}