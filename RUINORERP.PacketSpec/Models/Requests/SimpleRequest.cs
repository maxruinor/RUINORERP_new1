using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 简单请求类 - 用于快速发送简单类型的请求数据
    /// 支持字符串、布尔值、整数、浮点数等简单类型
    /// 避免为简单请求创建单独的实体类
    /// </summary>
    [Serializable]
    public class SimpleRequest : RequestBase
    {
        /// <summary>
        /// 请求数据对象
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 数据类型标识
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SimpleRequest()
        {
            DataType = "object";
        }

        /// <summary>
        /// 创建字符串请求
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateString(string value, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = "string",
                OperationType = operationType ?? "StringOperation"
            };
        }

        /// <summary>
        /// 创建布尔值请求
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateBool(bool value, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = "bool",
                OperationType = operationType ?? "BoolOperation"
            };
        }

        /// <summary>
        /// 创建整数值请求
        /// </summary>
        /// <param name="value">整数值</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateInt(int value, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = "int",
                OperationType = operationType ?? "IntOperation"
            };
        }

        /// <summary>
        /// 创建长整数值请求
        /// </summary>
        /// <param name="value">长整数值</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateLong(long value, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = "long",
                OperationType = operationType ?? "LongOperation"
            };
        }

        /// <summary>
        /// 创建浮点数值请求
        /// </summary>
        /// <param name="value">浮点数值</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateFloat(float value, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = "float",
                OperationType = operationType ?? "FloatOperation"
            };
        }

        /// <summary>
        /// 创建双精度浮点数值请求
        /// </summary>
        /// <param name="value">双精度浮点数值</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateDouble(double value, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = "double",
                OperationType = operationType ?? "DoubleOperation"
            };
        }

        /// <summary>
        /// 创建通用对象请求
        /// </summary>
        /// <param name="value">对象值</param>
        /// <param name="dataType">数据类型描述</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>简单请求实例</returns>
        public static SimpleRequest CreateObject(object value, string dataType = null, string operationType = null)
        {
            return new SimpleRequest
            {
                Data = value,
                DataType = dataType ?? value?.GetType().Name ?? "object",
                OperationType = operationType ?? "ObjectOperation"
            };
        }

        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <returns>字符串值</returns>
        public string GetStringValue()
        {
            return Data?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns>布尔值</returns>
        public bool GetBoolValue()
        {
            if (Data == null) return false;
            if (Data is bool boolValue) return boolValue;
            if (bool.TryParse(Data.ToString(), out bool result)) return result;
            return false;
        }

        /// <summary>
        /// 获取整数值
        /// </summary>
        /// <returns>整数值</returns>
        public int GetIntValue()
        {
            if (Data == null) return 0;
            if (Data is int intValue) return intValue;
            if (int.TryParse(Data.ToString(), out int result)) return result;
            return 0;
        }

        /// <summary>
        /// 获取长整数值
        /// </summary>
        /// <returns>长整数值</returns>
        public long GetLongValue()
        {
            if (Data == null) return 0;
            if (Data is long longValue) return longValue;
            if (long.TryParse(Data.ToString(), out long result)) return result;
            return 0;
        }

        /// <summary>
        /// 获取浮点数值
        /// </summary>
        /// <returns>浮点数值</returns>
        public float GetFloatValue()
        {
            if (Data == null) return 0;
            if (Data is float floatValue) return floatValue;
            if (float.TryParse(Data.ToString(), out float result)) return result;
            return 0;
        }

        /// <summary>
        /// 获取双精度浮点数值
        /// </summary>
        /// <returns>双精度浮点数值</returns>
        public double GetDoubleValue()
        {
            if (Data == null) return 0;
            if (Data is double doubleValue) return doubleValue;
            if (double.TryParse(Data.ToString(), out double result)) return result;
            return 0;
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return Data != null && !string.IsNullOrEmpty(DataType);
        }

        /// <summary>
        /// 获取强类型值
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>强类型值</returns>
        public T GetValue<T>()
        {
            if (Data == null) return default(T);
            
            if (Data is T directValue)
                return directValue;

            try
            {
                return (T)Convert.ChangeType(Data, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}