using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SqlSugar;

namespace RUINORERP.Model.Utilities
{

    /// <summary>
    /// 通用列表转换器：实现 string 与 List<T> 的双向转换
    /// 数据库存储：逗号分隔字符串（如 "1,3,5" 或 "a,b,c"）
    /// 内存使用：List<T>
    /// </summary>
    public class ListToStringConverter<T> : ISugarDataConverter
    {
        private readonly bool _filterZeros;

        /// <summary>
        /// 创建通用列表转换器
        /// </summary>
        /// <param name="filterZeros">是否过滤零值（仅对数值类型有效）</param>
        public ListToStringConverter(bool filterZeros = true)
        {
            _filterZeros = typeof(T).IsNumericType() && filterZeros;
        }

        // 数据库字段类型（固定为字符串）
        public Type DbType => typeof(string);

        // 将数据库字符串转换为 List<T>
        public object Deserialize(object value)
        {
            if (value == null || value == DBNull.Value)
                return CreateEmptyList();

            var str = value.ToString();
            if (string.IsNullOrWhiteSpace(str))
                return CreateEmptyList();

            return ParseStringToList(str);
        }

        // 将 List<T> 转换为数据库字符串（逗号分隔）
        public object Serialize(object value)
        {
            if (value == null) return DBNull.Value;

            if (value is List<T> list)
            {
                // 应用过滤规则（数值类型可过滤零值）
                var filteredList = _filterZeros
                    ? list.Where(item => !IsZero(item)).ToList()
                    : list;

                return string.Join(",", filteredList);
            }

            throw new ArgumentException($"转换失败：值不是 List<{typeof(T).Name}>类型");
        }

        // 将内存中的 List<T> 转换为数据库存储的字符串
        public SugarParameter ParameterConverter<TParam>(object value, int parameterIndex)
        {
            string parameterName = $"@param{parameterIndex}";

            if (value == null)
                return new SugarParameter(parameterName, DBNull.Value, System.Data.DbType.String);

            if (value is List<T> list)
            {
                var serialized = Serialize(list);
                return new SugarParameter(parameterName, serialized, System.Data.DbType.String);
            }

            throw new ArgumentException($"无法转换类型 {value.GetType()}，预期为 List<{typeof(T).Name}>");
        }

        // 将数据库读取的字符串转换为内存中的 List<T>
        public TParam QueryConverter<TParam>(IDataRecord dataRecord, int index)
        {
            object dbValue = dataRecord.GetValue(index);
            var result = Deserialize(dbValue);
            return (TParam)result;
        }

        #region 辅助方法

        private List<T> CreateEmptyList() => new List<T>();

        private List<T> ParseStringToList(string input)
        {
            return input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseSingleValue)
                .Where(value => !_filterZeros || !IsZero(value))
                .ToList();
        }

        private T ParseSingleValue(string str)
        {
            try
            {
                // 特殊处理字符串类型
                if (typeof(T) == typeof(string))
                    return (T)(object)str.Trim();

                // 处理数值类型
                if (typeof(T) == typeof(int) && int.TryParse(str, out int intVal))
                    return (T)(object)intVal;

                if (typeof(T) == typeof(long) && long.TryParse(str, out long longVal))
                    return (T)(object)longVal;

                // 添加其他类型的支持...
            }
            catch
            {
                // 解析失败时返回默认值
            }

            return default;
        }

        private bool IsZero(T value)
        {
            if (value == null) return true;

            switch (value)
            {
                case int i: return i == 0;
                case long l: return l == 0;
                default: return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// 类型扩展方法
    /// </summary>
    public static class TypeExtensions
    {
        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
