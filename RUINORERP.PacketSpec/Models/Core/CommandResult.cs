using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class CommandResult : ITraceable, IValidatable
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 额外数据字典
        /// </summary>
        public Dictionary<string, object> ExtraData { get; set; }

        /// <summary>
        /// 命令ID
        /// </summary>
        public string CommandId { get; set; }

        #region ITraceable 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            Timestamp = DateTime.UtcNow;
            LastUpdatedTime = Timestamp;
        }
        #endregion

        #region IValidatable 接口实现
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTime <= DateTime.UtcNow &&
                   CreatedTime >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }
        #endregion

        /// <summary>
        /// 响应数据包（用于发送回客户端）
        /// </summary>
        OriginalData ResponseData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandResult()
        {
            ExtraData = new Dictionary<string, object>();
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static CommandResult Success(object data = null, string message = "操作成功")
        {
            var result = new CommandResult
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
            return result;
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static CommandResult Failure(string message, string errorCode = null, Exception exception = null)
        {
            var result = new CommandResult
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Exception = exception,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
            return result;
        }

        /// <summary>
        /// 创建带响应数据的成功结果
        /// </summary>
        public static CommandResult SuccessWithResponse(OriginalData responseData, object data = null, string message = "操作成功")
        {
            var result = new CommandResult
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                ResponseData = responseData,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
            return result;
        }

        /// <summary>
        /// 创建错误结果
        /// </summary>
        public static CommandResult CreateError(string message, string errorCode = null, Exception ex = null)
        {
            return Failure(message, errorCode, ex);
        }

        /// <summary>
        /// 从异常创建结果
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令结果对象</returns>
        public static CommandResult FromException(Exception exception, string commandId = null)
        {
            return new CommandResult
            {
                IsSuccess = false,
                Message = exception.Message,
                ErrorCode = "500",
                CommandId = commandId,
                Data = exception.StackTrace,
                Exception = exception,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
        }

        /// <summary>
        /// 添加额外数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>当前实例</returns>
        public CommandResult WithExtraData(string key, object value)
        {
            ExtraData[key] = value;
            return this;
        }

        /// <summary>
        /// 获取额外数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>额外数据</returns>
        public T GetExtraData<T>(string key)
        {
            if (ExtraData.TryGetValue(key, out var value) && value is T)
            {
                return (T)value;
            }
            return default(T);
        }
    }

    /// <summary>
    /// 泛型命令执行结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class CommandResult<T> : CommandResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public new T Data { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="data">结果数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>命令结果对象</returns>
        public static CommandResult<T> Success(T data, string message = "操作成功")
        {
            return new CommandResult<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="exception">异常对象</param>
        /// <returns>命令结果对象</returns>
        public static CommandResult<T> Failure(string message, string errorCode = null, Exception exception = null)
        {
            return new CommandResult<T>
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Exception = exception,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
        }
    }
}