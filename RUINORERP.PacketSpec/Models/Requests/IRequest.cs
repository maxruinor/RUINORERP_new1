using System;
using System.Collections.Generic;

/// <summary>
/// 所有请求 DTO 可选实现此接口，定义请求的公共属性
/// </summary>
public interface IRequest
{
    /// <summary>
    /// 请求唯一标识
    /// </summary>
    string RequestId { get; set; }

    /// <summary>
    /// 请求操作类型
    /// </summary>
    string OperationType { get; set; }

    /// <summary>
    /// 请求时间戳（UTC时间）
    /// </summary>
    DateTime TimestampUtc { get; set; }

    /// <summary>
    /// 客户端信息
    /// </summary>
    string ClientInfo { get; set; }

    /// <summary>
    /// 扩展元数据（可选）
    /// </summary>
    Dictionary<string, object> Metadata { get; set; }
}
