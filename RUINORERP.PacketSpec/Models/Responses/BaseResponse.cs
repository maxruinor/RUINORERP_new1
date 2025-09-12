using System;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 基础响应模型
    /// </summary>
    [Serializable]
    public class BaseResponse
    {
        /// <summary>
        /// 请求是否成功处理
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码（如果有）
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public DateTime ServerTime { get; set; }

        /// <summary>
        /// 请求ID（用于追踪）
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 响应处理时间（毫秒）
        /// </summary>
        public long ProcessingTimeMs { get; set; }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        public static BaseResponse CreateSuccess(string message = "操作成功")
        {
            return new BaseResponse
            {
                Success = true,
                Message = message,
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        public static BaseResponse CreateFailure(string message, string errorCode = null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 设置请求ID
        /// </summary>
        public BaseResponse WithRequestId(string requestId)
        {
            RequestId = requestId;
            return this;
        }

        /// <summary>
        /// 设置处理时间
        /// </summary>
        public BaseResponse WithProcessingTime(long processingTimeMs)
        {
            ProcessingTimeMs = processingTimeMs;
            return this;
        }

        /// <summary>
        /// 验证响应有效性
        /// </summary>
        public virtual bool IsValid()
        {
            return ServerTime <= DateTime.UtcNow.AddMinutes(5) && 
                   ServerTime >= DateTime.UtcNow.AddMinutes(-5);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"Success: {Success}, Message: {Message}, ErrorCode: {ErrorCode}";
        }
    }

    /// <summary>
    /// 泛型基础响应
    /// </summary>
    [Serializable]
    public class BaseResponse<T> : BaseResponse
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 创建成功响应（带数据）
        /// </summary>
        public static BaseResponse<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建失败响应（带数据）
        /// </summary>
        public static BaseResponse<T> CreateFailure(string message, T data = default, string errorCode = null)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Data = data,
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 验证响应有效性
        /// </summary>
        public override bool IsValid()
        {
            return base.IsValid() && (Data != null || !Success);
        }
    }
}