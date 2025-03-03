using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{
    public static class DatatableHelper
    {
        //public static DataTable ToDataTable(this List<dynamic> list)
        //{
        //    DataTable dataTable = new DataTable();

        //    if (list != null && list.Count > 0)
        //    {
        //        // 获取动态对象的属性
        //        var firstItem = list[0];
        //        if (firstItem == null)
        //        {
        //            return dataTable;
        //        }

        //        // 获取列名
        //        var properties = (
        //            from property in ((IDictionary<string, object>)firstItem).Keys
        //            select property).ToList();

        //        foreach (var property in properties)
        //        {
        //            dataTable.Columns.Add(property);
        //        }

        //        // 填充数据
        //        foreach (var item in list)
        //        {
        //            DataRow row = dataTable.NewRow();

        //            foreach (var property in properties)
        //            {
        //                row[property] = item[property];
        //            }

        //            dataTable.Rows.Add(row);
        //        }
        //    }

        //    return dataTable;
        //}



        public static DataTable ToDataTable(this List<dynamic> list, string tableName = "数据表", bool formatDate = true)
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = tableName;
            
            if (list != null && list.Count > 0)
            {
                // 获取第一个动态对象作为列的参考
                var firstItem = list[0];
                if (firstItem == null)
                {
                    return dataTable;
                }

                // 获取列名，假设动态对象实现了 IDictionary<string, object>
                if (firstItem is IDictionary<string, object> expandoObj)
                {
                    foreach (var key in expandoObj.Keys)
                    {
                        if (key.Contains("日期"))
                        {
                            if (key.Contains("日期") && formatDate)
                            {
                                dataTable.Columns.Add(key, typeof(DateTime));
                            }
                        }
                        else
                        {
                            dataTable.Columns.Add(key, typeof(string));
                        }
                        //ColumnMapping
                        //dataTable.Columns.Add(key);

                    }
                }

                // 填充数据
                foreach (var item in list)
                {
                    DataRow row = dataTable.NewRow();

                    // 将动态对象转换为字典
                    if (item is IDictionary<string, object> itemDict)
                    {
                        foreach (var key in itemDict.Keys)
                        {
                            if (key.Contains("日期"))
                            {
                                if (key.Contains("日期") && formatDate)
                                {
                                    row[key] = DateTime.Parse(itemDict[key].ToString()).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    row[key] = DateTime.Parse(itemDict[key].ToString());
                                }
                            }
                            else
                            {
                                row[key] = itemDict[key];
                            }
                            
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }

            return dataTable;
        }
    }
}
