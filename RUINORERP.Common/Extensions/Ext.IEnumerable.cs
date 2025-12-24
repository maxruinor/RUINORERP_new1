//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Dto;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.Common.Extensions
{

    public static partial class ExtObject
    {

        /// <summary>
        /// 排除指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Cdictionarys"></param>
        /// <param name="exCols"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, string> exclude<T>(this ConcurrentDictionary<string, string> Cdictionarys, List<Expression<Func<T, object>>> exCols)
        {
            foreach (var exCol in exCols)
            {
                MemberInfo minfo = exCol.GetMemberInfo();
                string key = minfo.Name;
                string tempValue = string.Empty;
                Cdictionarys.TryRemove(key, out tempValue);
            }
            return Cdictionarys;
        }



        ///// <summary>
        /////     A NameValueCollection extension method that converts the @this to a dictionary.
        ///// </summary>
        ///// <param name="this">The @this to act on.</param>
        ///// <returns>@this as an IDictionary&lt;string,object&gt;</returns>
        //public static IDictionary<string, string?> ToDictionary(this System.Collections.Specialized.NameValueCollection? @this)
        //{
        //    var dict = new Dictionary<string, string?>();
        //    if (@this != null)
        //    {
        //        foreach (var key in @this.AllKeys)
        //        {
        //            if (key!=null)
        //            {
        //                dict.Add(key, @this[key]);
        //            }

        //        }
        //    }

        //    return dict;
        //}

        /// <summary>
        /// 复制序列中的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">原数据</param>
        /// <param name="startIndex">原数据开始复制的起始位置</param>
        /// <param name="length">需要复制的数据长度</param>
        /// <returns></returns>
        public static IEnumerable<T> Copy<T>(this IEnumerable<T> iEnumberable, int startIndex, int length)
        {
            var sourceArray = iEnumberable.ToArray();
            T[] newArray = new T[length];
            Array.Copy(sourceArray, startIndex, newArray, 0, length);

            return newArray;
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach (var item in iEnumberable)
            {
                func(item);
            }
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T, int> func)
        {
            var array = iEnumberable.ToArray();
            for (int i = 0; i < array.Count(); i++)
            {
                func(array[i], i);
            }
        }

        /// <summary>
        /// IEnumerable转换为List'T'
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> CastToList<T>(this IEnumerable source)
        {
            return new List<T>(source.Cast<T>());
        }

        /// <summary>
        /// 将IEnumerable'T'转为对应的DataTable
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable.ToJson().ToDataTable();
        }



        /// <summary>
        /// 将 List<T> 转换为 DataTable 的通用方法,其中T是一个自定义类型，FieldNameList是一个ConcurrentDictionary<string, string>，其中Key是列的名称，Value是列的标题：
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="fieldNameList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list, ConcurrentDictionary<string, string> fieldNameList)
        {
            // 创建一个 DataTable 对象
            DataTable dataTable = new DataTable();

            // 获取 T 类型的所有公共属性
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 遍历属性
            foreach (PropertyInfo property in properties)
            {
                // 获取属性的名称和值
                string propertyName = property.Name;
                // 如果属性不在 FieldNameList 中，则忽略该属性
                if (!fieldNameList.ContainsKey(propertyName))
                {
                    continue;
                }
                Type newcolType;
                if (property == null)
                {
                    newcolType = typeof(string);
                }
                else
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        newcolType = Nullable.GetUnderlyingType(property.PropertyType);
                    }
                    else
                    {
                        newcolType = property.PropertyType;
                    }
                }
                // 创建一个 DataColumn 对象
                DataColumn dataColumn = new DataColumn(propertyName, newcolType);
                dataColumn.Caption = fieldNameList[propertyName];

                // 将 DataColumn 添加到 DataTable 中
                dataTable.Columns.Add(dataColumn);
            }

            // 遍历 List<T> 中的对象
            foreach (T obj in list)
            {
                // 创建一个 DataRow 对象
                DataRow dataRow = dataTable.NewRow();

                // 遍历属性
                foreach (PropertyInfo property in properties)
                {
                    // 获取属性的名称和值
                    string propertyName = property.Name;
                    // 如果属性不在 FieldNameList 中，则忽略该属性
                    if (!fieldNameList.ContainsKey(propertyName))
                    {
                        continue;
                    }

                    object propertyValue = property.GetValue(obj, null);
                    if (propertyValue != null)
                    {
                        // 将属性值设置为 DataRow 的值
                        dataRow[propertyName] = propertyValue;
                    }
                    else
                    {
                        // 处理 propertyValue 为 null 的情况
                        dataRow[propertyName] = DBNull.Value;
                    }

                }

                // 将 DataRow 添加到 DataTable 中
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        /// <summary>
        /// 将两个部分的List<T>转换为DataTable，其中SubPartsfieldNameList和fieldNameList是两个ConcurrentDictionary<string, string>，其中Key是列的名称，Value是列的标题
        /// SubParts主要用于显示部分,他们通过一个主键关联 产品明细ID
        /// </summary>
        /// <typeparam name="Sub"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="list"></param>
        /// <param name="subList"></param>
        /// <param name="SubPartsfieldNameList"></param>
        /// <param name="MainfieldNameList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable_old<Sub, M>(this List<M> list, List<Sub> subList,
            ConcurrentDictionary<string, string> SubPartsfieldNameList,
            ConcurrentDictionary<string, string> MainfieldNameList, Expression<Func<M, object>> RelatedKey)
        {
            // 创建一个 DataTable 对象
            DataTable dataTable = new DataTable();
            MemberInfo minfo = RelatedKey.GetMemberInfo();
            string keyBizName = minfo.Name;

            #region 构建表结构
            #region   获取Sub类型的所有公共属性 展示用的

            PropertyInfo[] propertiesSub = typeof(Sub).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //检测两个数据部分的列是否都 包含业务主键列
            if (propertiesSub.ContainsField(keyBizName))
            {
                throw new Exception("请确保两部分的列都 包括指定的业务主键列:" + keyBizName);
            }
            else
            {
                //添加主键列但是标题为空。因为不需要显示
            }

            // 遍历属性
            foreach (PropertyInfo property in propertiesSub)
            {
                // 获取属性的名称和值
                string propertyName = property.Name;

                // 如果属性不在 FieldNameList 中，则忽略该属性
                if (!SubPartsfieldNameList.ContainsKey(propertyName))
                {
                    continue;
                }
                Type newcolType;
                if (property == null)
                {
                    newcolType = typeof(string);
                }
                else
                {

                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        newcolType = Nullable.GetUnderlyingType(property.PropertyType);
                    }
                    else
                    {
                        newcolType = property.PropertyType;
                    }
                }
                // 创建一个 DataColumn 对象
                System.Diagnostics.Debug.WriteLine(newcolType.Name);
                DataColumn dataColumn = new DataColumn(propertyName, newcolType);
                dataColumn.Caption = SubPartsfieldNameList[propertyName];

                // 将 DataColumn 添加到 DataTable 中
                if (!dataTable.Columns.Contains(propertyName))
                {
                    dataTable.Columns.Add(dataColumn);
                }

            }

            #endregion

            #region 主要数据部分

            // 获取 T 类型的所有公共属性
            PropertyInfo[] properties = typeof(M).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //检测两个数据部分的列是否都 包含业务主键列
            if (properties.ContainsField(keyBizName))
            {
                throw new Exception("请确保两部分的列都 包括指定的业务主键列:" + keyBizName);
            }
            // 遍历属性
            foreach (PropertyInfo property in properties)
            {
                // 获取属性的名称和值
                string propertyName = property.Name;

                // 如果属性不在 FieldNameList 中，则忽略该属性
                if (!MainfieldNameList.ContainsKey(propertyName))
                {
                    continue;
                }
                Type newcolType;
                if (property == null)
                {
                    newcolType = typeof(string);
                }
                else
                {

                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        newcolType = Nullable.GetUnderlyingType(property.PropertyType);
                    }
                    else
                    {
                        newcolType = property.PropertyType;
                    }
                }
                // 创建一个 DataColumn 对象
                DataColumn dataColumn = new DataColumn(propertyName, newcolType);
                dataColumn.Caption = MainfieldNameList[propertyName];
                if (!dataTable.Columns.Contains(propertyName))
                {
                    dataTable.Columns.Add(dataColumn);
                }
            }

            #endregion

            #endregion

            List<DataRow> drs = new List<DataRow>();
            #region  获取Sub类型的所有公共属性的值 展示用的
            // 遍历 List<T> 中的对象
            foreach (Sub obj in subList)
            {
                // 创建一个 DataRow 对象
                DataRow dataRow = dataTable.NewRow();
                // 遍历属性
                foreach (PropertyInfo property in propertiesSub)
                {
                    // 获取属性的名称和值
                    string propertyName = property.Name;
                    // 如果属性不在 FieldNameList 中，则忽略该属性，业务主键列不能排除
                    if (SubPartsfieldNameList.ContainsKey(propertyName) || propertyName == keyBizName)
                    {
                        object propertyValue = property.GetValue(obj, null);
                        if (propertyValue != null)
                        {
                            // 将属性值设置为 DataRow 的值
                            dataRow[propertyName] = propertyValue;
                        }
                        else
                        {
                            // 处理 propertyValue 为 null 的情况
                            dataRow[propertyName] = DBNull.Value;
                        }
                    }
                }
                // 将 DataRow 添加到 DataTable 中
                drs.Add(dataRow);

            }
            #endregion



            // 遍历 List<T> 中的对象
            foreach (M obj in list)
            {

                object KeyPropertyValue = ReflectionHelper.GetPropertyValue(obj, keyBizName);
                // 创建一个 DataRow 对象
                DataRow dataRow = dataTable.NewRow();
                DataRow dataRowTemp = drs.FirstOrDefault(c => c[keyBizName].ToString() == KeyPropertyValue.ToString());
                if (dataRowTemp != null)
                {
                    dataRow.ItemArray = dataRowTemp.ItemArray;
                }
                else
                {
                    continue;
                }
                if (dataRow == null)
                {
                    continue;
                }
                // 遍历属性
                foreach (PropertyInfo property in properties)
                {
                    // 获取属性的名称和值
                    string propertyName = property.Name;
                    // 如果属性不在 FieldNameList 中，则忽略该属性
                    if (!MainfieldNameList.ContainsKey(propertyName))
                    {
                        continue;
                    }

                    object propertyValue = property.GetValue(obj, null);
                    if (propertyValue != null)
                    {
                        //如果为日期时。值要特殊处理
                        //if (property.PropertyType == typeof(DateTime))
                        //{
                        //    MessageBox.Show(propertyValue.ToString());
                        //}

                        // 将属性值设置为 DataRow 的值
                        dataRow[propertyName] = propertyValue;
                    }
                    else
                    {
                        // 处理 propertyValue 为 null 的情况
                        dataRow[propertyName] = DBNull.Value;
                    }

                }

                // 将 DataRow 添加到 DataTable 中
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }


        public static DataTable ToDataTable<Sub, M>(this List<M> list, List<Sub> subList,
    ConcurrentDictionary<string, string> SubPartsfieldNameList,
    ConcurrentDictionary<string, string> MainfieldNameList, Expression<Func<M, object>> RelatedKey,
    Func<DataTable, Type, bool> foreignKeyConversionCallback = null) where Sub : class where M : class
        {
            // 创建一个 DataTable 对象
            DataTable dataTable = new DataTable();

            // 获取主键名称
            MemberInfo minfo = RelatedKey.GetMemberInfo();
            string keyBizName = minfo.Name;

            // 检查主键列是否存在于两个列表中
            if (!SubPartsfieldNameList.ContainsKey(keyBizName) || !MainfieldNameList.ContainsKey(keyBizName))
            {
                throw new Exception("请确保两部分的列都包括指定的业务主键列:" + keyBizName);
            }

            // 构建 DataTable 的列
            BuildDataTableColumns(dataTable, SubPartsfieldNameList, MainfieldNameList, keyBizName, typeof(Sub), typeof(M));

            // 填充 DataTable 的数据
            FillDataTableData<Sub, M>(dataTable, subList, list, SubPartsfieldNameList, MainfieldNameList, keyBizName);

            // 如果提供了回调函数，调用它进行外键转换
            if (foreignKeyConversionCallback != null)
            {
                foreignKeyConversionCallback(dataTable, typeof(M));
                foreignKeyConversionCallback(dataTable, typeof(Sub));
            }

            return dataTable;
        }

        private static void BuildDataTableColumns(DataTable dataTable,
            ConcurrentDictionary<string, string> SubPartsfieldNameList,
            ConcurrentDictionary<string, string> MainfieldNameList,
            string keyBizName, Type subType, Type mainType)
        {
            // 获取所有属性，用于后续查找
            Dictionary<string, PropertyInfo> allProperties = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo property in subType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!allProperties.ContainsKey(property.Name))
                {
                    allProperties.Add(property.Name, property);
                }
            }
            foreach (PropertyInfo property in mainType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!allProperties.ContainsKey(property.Name))
                {
                    allProperties.Add(property.Name, property);
                }
            }

            // 按照字段列表顺序构建列，先处理SubPartsfieldNameList，再处理MainfieldNameList
            // 这样可以确保列顺序与字段列表中定义的顺序一致
            
            // 1. 先处理SubPartsfieldNameList中的列
            foreach (var kvp in SubPartsfieldNameList.OrderBy(k => k.Key))
            {
                string propertyName = kvp.Key;
                if (allProperties.TryGetValue(propertyName, out PropertyInfo property))
                {
                    // 确保获取正确的列类型，特别是处理可空类型
                    Type colType;
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        colType = Nullable.GetUnderlyingType(property.PropertyType);
                    }
                    else
                    {
                        colType = property.PropertyType;
                    }

                    // 创建列时确保使用原始属性的类型
                    DataColumn column = new DataColumn(propertyName, colType);
                    column.Caption = kvp.Value;

                    // 如果相同列名的列已经存在，则跳过
                    if (!dataTable.Columns.Contains(column.ColumnName))
                    {
                        dataTable.Columns.Add(column);
                    }
                }
            }

            // 2. 再处理MainfieldNameList中的列
            foreach (var kvp in MainfieldNameList.OrderBy(k => k.Key))
            {
                string propertyName = kvp.Key;
                // 如果该列已经在SubPartsfieldNameList中处理过，则跳过
                if (SubPartsfieldNameList.ContainsKey(propertyName))
                {
                    continue;
                }

                if (allProperties.TryGetValue(propertyName, out PropertyInfo property))
                {
                    // 确保获取正确的列类型，特别是处理可空类型
                    Type colType;
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        colType = Nullable.GetUnderlyingType(property.PropertyType);
                    }
                    else
                    {
                        colType = property.PropertyType;
                    }

                    // 创建列时确保使用原始属性的类型
                    DataColumn column = new DataColumn(propertyName, colType);
                    column.Caption = kvp.Value;

                    // 如果相同列名的列已经存在，则跳过
                    if (!dataTable.Columns.Contains(column.ColumnName))
                    {
                        dataTable.Columns.Add(column);
                    }
                }
            }
        }

        private static void FillDataTableData<BaseInfo, Detail>(DataTable dataTable, List<BaseInfo> BaseInfoList, List<Detail> DetailList,
            ConcurrentDictionary<string, string> SubPartsfieldNameList,
            ConcurrentDictionary<string, string> MainfieldNameList, string keyBizName) where BaseInfo : class where Detail : class
        {
            foreach (Detail mainObj in DetailList)
            {
                object keyValue = ReflectionHelper.GetPropertyValue(mainObj, keyBizName);
                DataRow mainRow = dataTable.NewRow();
                FillDataRow(mainRow, mainObj, MainfieldNameList, keyBizName);

                //找到关联的子对象，并更新行根据主键
                var subObj = BaseInfoList.Where(c => c.GetPropertyValue(keyBizName).Equals(keyValue)).FirstOrDefault();
                if (subObj != null)
                {
                    FillDataRow(mainRow, subObj, SubPartsfieldNameList, keyBizName);
                }
                dataTable.Rows.Add(mainRow);
            }
        }

        /// <summary>
        /// 将DataTable中的外键值转换为显示值
        /// </summary>
        /// <param name="dataTable">要转换的DataTable</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="getForeignKeyDisplayValuesCallback">获取外键显示值的回调函数</param>
        public static void ConvertForeignKeysToDisplayValues(DataTable dataTable, Type entityType,
            Func<string, string, Dictionary<object, string>> getForeignKeyDisplayValuesCallback)
        {
            // 获取实体类的所有属性
            PropertyInfo[] properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // 获取 FKRelationAttribute 特性
                var fkAttr = property.GetCustomAttribute<FKRelationAttribute>(false);
                if (fkAttr == null)
                    continue;

                // 检查 DataTable 中是否包含该外键列
                if (!dataTable.Columns.Contains(property.Name))
                    continue;

                // 获取外键表名和ID列名
                string fkTableName = fkAttr.FKTableName;
                string fkIdColumnName = fkAttr.FK_IDColName;

                // 为该外键列添加一个显示列
                string displayColumnName = $"{property.Name}_Display";
                if (!dataTable.Columns.Contains(displayColumnName))
                {
                    // 获取原外键列的Caption，用于显示列
                    string originalCaption = dataTable.Columns[property.Name].Caption;
                    
                    // 创建显示列，并设置友好的Caption
                    DataColumn displayColumn = new DataColumn(displayColumnName, typeof(string));
                    displayColumn.Caption = originalCaption; // 使用原外键列的标题作为显示列的标题
                    
                    // 获取原外键列的位置，将显示列插入到原外键列的旁边
                    int originalColumnIndex = dataTable.Columns.IndexOf(property.Name);
                    dataTable.Columns.Add(displayColumn);
                    
                    // 将显示列移动到原外键列的后面，保持相关列在一起
                    displayColumn.SetOrdinal(originalColumnIndex + 1);
                }

                // 使用回调函数获取外键对应的显示值映射
                Dictionary<object, string> fkDisplayValues = getForeignKeyDisplayValuesCallback(fkTableName, fkIdColumnName);

                // 遍历 DataTable 行，将外键值转换为显示值
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[property.Name] != DBNull.Value)
                    {
                        var fkValue = row[property.Name];
                        if (fkDisplayValues.TryGetValue(fkValue, out string displayValue))
                        {
                            row[displayColumnName] = displayValue;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 将DataTable中的外键值转换为显示值（重载版本，传递更多上下文信息）
        /// </summary>
        /// <param name="dataTable">要转换的DataTable</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="getForeignKeyDisplayValuesCallback">获取外键显示值的回调函数，参数包括外键表名、外键列名、当前处理的列名和DataTable</param>
        public static void ConvertForeignKeysToDisplayValues(DataTable dataTable, Type entityType,
            Func<string, string, string, DataTable, Dictionary<object, string>> getForeignKeyDisplayValuesCallback)
        {
            // 获取实体类的所有属性
            PropertyInfo[] properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // 获取 FKRelationAttribute 特性
                var fkAttr = property.GetCustomAttribute<FKRelationAttribute>(false);
                if (fkAttr == null)
                    continue;

                // 检查 DataTable 中是否包含该外键列
                if (!dataTable.Columns.Contains(property.Name))
                    continue;

                // 获取外键表名和ID列名
                string fkTableName = fkAttr.FKTableName;
                string fkIdColumnName = fkAttr.FK_IDColName;

                // 为该外键列添加一个显示列
                string displayColumnName = $"{property.Name}_Display";
                if (!dataTable.Columns.Contains(displayColumnName))
                {
                    // 获取原外键列的Caption，用于显示列
                    string originalCaption = dataTable.Columns[property.Name].Caption;
                    
                    // 创建显示列，并设置友好的Caption
                    DataColumn displayColumn = new DataColumn(displayColumnName, typeof(string));
                    displayColumn.Caption = originalCaption; // 使用原外键列的标题作为显示列的标题
                    
                    // 获取原外键列的位置，将显示列插入到原外键列的旁边
                    int originalColumnIndex = dataTable.Columns.IndexOf(property.Name);
                    dataTable.Columns.Add(displayColumn);
                    
                    // 将显示列移动到原外键列的后面，保持相关列在一起
                    displayColumn.SetOrdinal(originalColumnIndex + 1);
                }

                // 从DataTable中提取出当前列的所有唯一外键值，只处理实际存在的值
                HashSet<object> uniqueFkValues = new HashSet<object>();
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[property.Name] != DBNull.Value)
                    {
                        uniqueFkValues.Add(row[property.Name]);
                    }
                }

                // 使用回调函数获取外键对应的显示值映射，传递当前列名和DataTable
                Dictionary<object, string> fkDisplayValues = getForeignKeyDisplayValuesCallback(fkTableName, fkIdColumnName, property.Name, dataTable);

                // 遍历 DataTable 行，将外键值转换为显示值
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[property.Name] != DBNull.Value)
                    {
                        var fkValue = row[property.Name];
                        if (fkDisplayValues.TryGetValue(fkValue, out string displayValue))
                        {
                            row[displayColumnName] = displayValue;
                        }
                    }
                }
            }
        }

        private static void FillDataRow(DataRow row, object obj, ConcurrentDictionary<string, string> fieldNameList, string keyBizName)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string propertyName = property.Name;
                if (fieldNameList.ContainsKey(propertyName))
                {
                    object value = property.GetValue(obj);
                    // 确保数据填充时也能正确保持原始类型
                    if (value != null && row.Table.Columns.Contains(propertyName))
                    {
                        // 获取目标列的类型
                        Type columnType = row.Table.Columns[propertyName].DataType;

                        // 尝试将值转换为列的类型，确保类型兼容性
                        try
                        {
                            // 如果值的类型与列类型兼容，直接赋值
                            if (columnType.IsAssignableFrom(value.GetType()) || columnType.IsInstanceOfType(value))
                            {
                                row[propertyName] = value;
                            }
                            else
                            {
                                // 对于值类型，尝试显式转换
                                if (columnType.IsValueType)
                                {
                                    row[propertyName] = Convert.ChangeType(value, columnType);
                                }
                                else
                                {
                                    // 对于引用类型，直接赋值
                                    row[propertyName] = value;
                                }
                            }
                        }
                        catch
                        {
                            // 如果转换失败，回退到原始赋值方式
                            row[propertyName] = value;
                        }
                    }
                    else
                    {
                        row[propertyName] = DBNull.Value;
                    }
                }
            }
        }

    }
}