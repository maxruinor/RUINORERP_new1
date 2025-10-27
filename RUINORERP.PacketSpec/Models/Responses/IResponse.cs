using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 响应接口 - 定义所有响应对象的公共契约
    /// Message: 用于传达操作结果的通用信息，无论成功还是失败都应该有值
    /// ErrorMessage: 专门用于存储详细的错误信息，只有在失败时才有意义，成功时应为null
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 业务级错误码；0 表示成功
        /// </summary>
        int ErrorCode { get; set; }

        /// <summary>
        /// 详细的错误信息；仅在操作失败时设置，成功时为null
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// 操作结果的通用描述信息；成功或失败时都应提供有意义的描述
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 响应时间戳
        /// </summary>
        DateTime Timestamp { get; set; }


        /// <summary>
        /// 请求标识 - 用于将响应与原始请求关联，支持异步通信和审计追踪
        /// </summary>
        string RequestId { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 扩展元数据（可选）
        /// </summary>
        Dictionary<string, object> Metadata { get; set; }

    }
}