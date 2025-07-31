using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Model.Utilities
{
    /// <summary>
    /// SqlSugar 转换器：实现 string 与 List<int>的双向转换
    /// 数据库存储：逗号分隔字符串（如 "1,3,5"）
    /// 内存使用：List<int>
    /// </summary>
    public class ListIntToStringConverter : ISugarDataConverter
    {
        // 将数据库字符串转换为 List<int>
        public object Deserialize(object value)
        {
            if (value == null || value == DBNull.Value)
                return new List<int>();

            var str = value.ToString();
            if (string.IsNullOrEmpty(str))
                return new List<int>();

            return str.Split(',')
            .Select(s => int.TryParse(s, out int num) ? num : 0)
            .Where(num => num != 0) // 过滤无效值
            .ToList();
        }

        // 将 List<int>转换为数据库字符串（逗号分隔）
        public object Serialize(object value)
        {
            if (value == null)
                return null;

            if (value is List<int> list)
            {
                return string.Join(",", list.Where(num => num != 0)); // 排除 0 值（如果需要）
            }

            throw new ArgumentException(" 转换失败：值不是 List<int>类型 ");
        }

        // 数据库字段类型（这里用 varchar）
        public Type DbType => typeof(string);



        /// <summary>
        /// 将内存中的 List<int>转换为数据库存储的字符串
        /// </summary>
        public SugarParameter ParameterConverter<T>(object value, int parameterIndex)
        {
            // 参数名称格式：@param + 索引，符合 SqlSugar 规范
            string parameterName = $"@param {parameterIndex}";

            // 处理空值情况
            if (value == null)
            {
                return new SugarParameter(parameterName, DBNull.Value, System.Data.DbType.String);
            }

            // 转换 List<int>为逗号分隔字符串
            if (value is List<int> intList)
            {
                string dbValue = string.Join(",", intList.Where(i => i != 0)); // 过滤无效值
                return new SugarParameter(parameterName, dbValue, System.Data.DbType.String);
            }

            // 处理类型不匹配的异常
            throw new ArgumentException($" 无法转换类型 {value.GetType()} 到字符串，预期类型为 List<int>");
        }

        /// <summary>
        /// 将数据库读取的字符串转换为内存中的 List<int>
        /// </summary>
        public T QueryConverter<T>(IDataRecord dataRecord, int index)
        {
            // 读取数据库值
            object dbValue = dataRecord.GetValue(index);

            // 处理 DBNull 情况
            if (dbValue == DBNull.Value || dbValue == null)
            {
                return (T)(object)new List<int>();
            }

            // 转换字符串为 List<int>
            string stringValue = dbValue.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(stringValue))
            {
                return (T)(object)new List<int>();
            }

            List<int> result = stringValue.Split(',')
            .Select(s => int.TryParse(s, out int num) ? num : 0)
            .Where(num => num != 0) // 过滤解析失败的无效值
            .ToList();

            return (T)(object)result;
        }
    }

    /// <summary>
    /// SqlSugar 转换器：实现 string 与 List<int>的双向转换
    /// 数据库存储：逗号分隔字符串（如 "1,3,5"）
    /// 内存使用：List<int>
    /// </summary>
    public class ListLongToStringConverter : ISugarDataConverter
    {
        // 将数据库字符串转换为 List<int>
        public object Deserialize(object value)
        {
            if (value == null || value == DBNull.Value)
                return new List<long>();

            var str = value.ToString();
            if (string.IsNullOrEmpty(str))
                return new List<long>();

            return str.Split(',')
            .Select(s => long.TryParse(s, out long num) ? num : 0)
            .Where(num => num != 0) // 过滤无效值
            .ToList();
        }

        // 将 List<int>转换为数据库字符串（逗号分隔）
        public object Serialize(object value)
        {
            if (value == null)
                return null;

            if (value is List<long> list)
            {
                return string.Join(",", list.Where(num => num != 0)); // 排除 0 值（如果需要）
            }

            throw new ArgumentException(" 转换失败：值不是 List<int>类型 ");
        }

        // 数据库字段类型（这里用 varchar）
        public Type DbType => typeof(string);



        /// <summary>
        /// 将内存中的 List<int>转换为数据库存储的字符串
        /// </summary>
        public SugarParameter ParameterConverter<T>(object value, int parameterIndex)
        {
            // 参数名称格式：@param + 索引，符合 SqlSugar 规范
            string parameterName = $"@param {parameterIndex}";

            // 处理空值情况
            if (value == null)
            {
                return new SugarParameter(parameterName, DBNull.Value, System.Data.DbType.String);
            }

            // 转换 List<int>为逗号分隔字符串
            if (value is List<long> intList)
            {
                string dbValue = string.Join(",", intList.Where(i => i != 0)); // 过滤无效值
                return new SugarParameter(parameterName, dbValue, System.Data.DbType.String);
            }

            // 处理类型不匹配的异常
            throw new ArgumentException($" 无法转换类型 {value.GetType()} 到字符串，预期类型为 List<int>");
        }

        /// <summary>
        /// 将数据库读取的字符串转换为内存中的 List<int>
        /// </summary>
        public T QueryConverter<T>(IDataRecord dataRecord, int index)
        {
            // 读取数据库值
            object dbValue = dataRecord.GetValue(index);

            // 处理 DBNull 情况
            if (dbValue == DBNull.Value || dbValue == null)
            {
                return (T)(object)new List<int>();
            }

            // 转换字符串为 List<int>
            string stringValue = dbValue.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(stringValue))
            {
                return (T)(object)new List<long>();
            }

            List<long> result = stringValue.Split(',')
            .Select(s => long.TryParse(s, out long num) ? num : 0)
            .Where(num => num != 0) // 过滤解析失败的无效值
            .ToList();

            return (T)(object)result;
        }
    }

}
