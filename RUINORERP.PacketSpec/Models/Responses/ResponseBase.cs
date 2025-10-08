using MessagePack;
using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 响应基类 - 提供所有响应的公共属性和方法
    /// </summary>
    [MessagePackObject]
    public class ResponseBase : IResponse
    {
        /// <summary>
        /// 业务级错误码；0 表示成功
        /// </summary>
        [Key(0)]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 人类可读错误消息；Success 时可为空
        /// </summary>
        [Key(1)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        [Key(2)]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        [Key(3)]
        public string Message { get; set; }

        /// <summary>
        /// 响应时间戳（UTC时间）
        /// </summary>
        [Key(4)]
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 响应代码
        /// </summary>
        [Key(5)]
        public int Code { get; set; }

        /// <summary>
        /// 请求标识
        /// </summary>
        [Key(6)]
        public string RequestId { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        [Key(7)]
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 扩展元数据（可选）
        /// </summary>
        [Key(8)]
        [MessagePack.IgnoreMember]
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseBase()
        {
            TimestampUtc = DateTime.UtcNow;
            Metadata = new Dictionary<string, object>();
        }


        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public virtual ResponseBase WithMetadata(string key, object value)
        {
            Metadata ??= new Dictionary<string, object>();
            Metadata[key] = value;
            return this;
        }

        /// <summary>
        /// 批量添加元数据
        /// </summary>
        /// <param name="metadata">元数据字典</param>
        /// <returns>当前实例</returns>
        public virtual ResponseBase WithMetadata(Dictionary<string, object> metadata)
        {
            Metadata ??= new Dictionary<string, object>();
            foreach (var item in metadata)
            {
                Metadata[item.Key] = item.Value;
            }
            return this;
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <returns>响应实例</returns>
        public static ResponseBase CreateSuccess(string message = "操作成功")
        {
            return new ConcreteResponse
            {
                IsSuccess = true,
                Message = message,
                Code = 200,
                TimestampUtc = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public static ResponseBase CreateError(string message, int code = 500)
        {
            return new ConcreteResponse
            {
                IsSuccess = false,
                Message = message,
                Code = code,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// 泛型响应基类 - 提供所有响应的公共属性和方法，包含数据部分
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [MessagePackObject]
    public class ResponseBase<T> : ResponseBase
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        [Key(9)]
        public T Data { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ResponseBase() : base()
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public ResponseBase(bool success, string message, T data = default(T), int code = 200)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Code = code;
            this.TimestampUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>响应实例</returns>
        public static ResponseBase<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new ResponseBase<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                Code = 200,
                TimestampUtc = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public new static ResponseBase<T> CreateError(string message, int code = 500)
        {
            return new ResponseBase<T>
            {
                IsSuccess = false,
                Message = message,
                Code = code,
                TimestampUtc = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建失败响应（简化方法）
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public static ResponseBase<T> Failure(string message, int code = 500)
        {
            return new ResponseBase<T>
            {
                IsSuccess = false,
                Message = message,
                Code = code,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// 具体响应实现类
    /// </summary>
    internal class ConcreteResponse : ResponseBase
    {
        // 继承所有基类属性和方法
    }
}
