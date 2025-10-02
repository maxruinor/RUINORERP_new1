using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 所有响应 DTO 可选实现此接口，由框架自动填充公共错误信息
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 业务级错误码；0 表示成功
        /// </summary>
        int ErrorCode { get; set; }

        /// <summary>
        /// 人类可读错误消息；Success 时可为空
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 响应时间戳（UTC时间）
        /// </summary>
        DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 响应代码
        /// </summary>
        int Code { get; set; }

        /// <summary>
    /// 请求标识
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
