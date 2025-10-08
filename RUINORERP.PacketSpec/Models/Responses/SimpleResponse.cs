using System;
using MessagePack;
namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 简单响应类 - 用于快速返回简单类型的响应数据
    /// 支持字符串、布尔值、整数、浮点数等简单类型
    /// 与SimpleRequest配套使用
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class SimpleResponse : ResponseBase
    {
        /// <summary>
        /// 响应数据对象
        /// </summary>
        [Key(10)]
        public object Data { get; set; }

        /// <summary>
        /// 数据类型标识
        /// </summary>
        [Key(11)]
        public string DataType { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SimpleResponse()
        {
            DataType = "object";
        }

        /// <summary>
        /// 创建成功的字符串响应
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessString(string value, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = "string"
            };
        }

        /// <summary>
        /// 创建成功的布尔值响应
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessBool(bool value, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = "bool"
            };
        }

        /// <summary>
        /// 创建成功的整数值响应
        /// </summary>
        /// <param name="value">整数值</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessInt(int value, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = "int"
            };
        }

        /// <summary>
        /// 创建成功的长整数值响应
        /// </summary>
        /// <param name="value">长整数值</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessLong(long value, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = "long"
            };
        }

        /// <summary>
        /// 创建成功的浮点数值响应
        /// </summary>
        /// <param name="value">浮点数值</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessFloat(float value, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = "float"
            };
        }

        /// <summary>
        /// 创建成功的双精度浮点数值响应
        /// </summary>
        /// <param name="value">双精度浮点数值</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessDouble(double value, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = "double"
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateFailure(string message, int errorCode = 500)
        {
            return new SimpleResponse
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Data = null,
                DataType = "null"
            };
        }

        /// <summary>
        /// 创建成功的通用对象响应
        /// </summary>
        /// <param name="value">对象值</param>
        /// <param name="dataType">数据类型描述</param>
        /// <param name="message">响应消息</param>
        /// <returns>简单响应实例</returns>
        public static SimpleResponse CreateSuccessObject(object value, string dataType = null, string message = "操作成功")
        {
            return new SimpleResponse
            {
                IsSuccess = true,
                Message = message,
                Data = value,
                DataType = dataType ?? value?.GetType().Name ?? "object"
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
