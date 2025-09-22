using System;
using System.Collections.Generic;

namespace RUINORERP.UI.Network.Models
{
    /// <summary>
    /// API响应模型
    /// 统一服务器响应的格式
    /// </summary>
    /// <typeparam name="TData">响应数据类型</typeparam>
    public class ApiResponse<TData>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// 响应时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public Dictionary<string, object> Meta { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiResponse()
        {
            Timestamp = DateTime.Now;
            Meta = new Dictionary<string, object>();
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">响应消息</param>
        /// <returns>成功响应实例</returns>
        public static ApiResponse<TData> SuccessResponse(TData data = default, string message = "操作成功")
        {
            return new ApiResponse<TData>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns>失败响应实例</returns>
        public static ApiResponse<TData> Failure(string message, string errorCode = null)
        {
            return new ApiResponse<TData>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }

    /// <summary>
    /// API响应模型（无数据）
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="message">响应消息</param>
        /// <returns>成功响应实例</returns>
        public static ApiResponse SuccessResponse(string message = "操作成功")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns>失败响应实例</returns>
        public static ApiResponse Failure(string message, string errorCode = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}