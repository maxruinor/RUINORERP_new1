using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TestCache
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing cache modifications...");
            
            // 测试字符串解析为JArray
            string jsonString = "[{\"Id\":1,\"Name\":\"Test\"},{\"Id\":2,\"Name\":\"Test2\"}]";
            
            try
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(jsonString);
                Console.WriteLine($"Successfully parsed JSON string to JArray with {jArray.Count} items");
                
                // 测试ConvertJArrayToDataTable方法
                var dataTable = ConvertJArrayToDataTable(jArray);
                Console.WriteLine($"Successfully converted JArray to DataTable with {dataTable.Rows.Count} rows and {dataTable.Columns.Count} columns");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("Test completed.");
        }
        
        static System.Data.DataTable ConvertJArrayToDataTable(JArray jArray)
        {
            var dataTable = new System.Data.DataTable();
            
            if (jArray == null || jArray.Count == 0)
            {
                return dataTable;
            }
            
            // 获取第一个对象来确定列结构
            var firstItem = jArray[0];
            
            if (firstItem is JObject jObject)
            {
                // 为每个属性创建列
                foreach (var property in jObject.Properties())
                {
                    var columnType = GetColumnType(property.Value.Type);
                    dataTable.Columns.Add(property.Name, columnType);
                }
                
                // 添加数据行
                foreach (JObject item in jArray)
                {
                    var row = dataTable.NewRow();
                    foreach (var property in item.Properties())
                    {
                        var value = property.Value.ToObject(dataTable.Columns[property.Name].DataType);
                        row[property.Name] = value;
                    }
                    dataTable.Rows.Add(row);
                }
            }
            else
            {
                // 如果不是JObject数组，创建一个单列表
                dataTable.Columns.Add("Value", typeof(string));
                foreach (var item in jArray)
                {
                    dataTable.Rows.Add(item.ToString());
                }
            }
            
            return dataTable;
        }
        
        static Type GetColumnType(JTokenType jTokenType)
        {
            switch (jTokenType)
            {
                case JTokenType.Integer:
                    return typeof(long);
                case JTokenType.Float:
                    return typeof(double);
                case JTokenType.String:
                    return typeof(string);
                case JTokenType.Boolean:
                    return typeof(bool);
                case JTokenType.Date:
                    return typeof(DateTime);
                case JTokenType.Guid:
                    return typeof(Guid);
                case JTokenType.Null:
                    return typeof(string);
                default:
                    return typeof(string);
            }
        }
    }
}