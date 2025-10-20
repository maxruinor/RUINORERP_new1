using System;
using System.Collections.Generic;
using MessagePack;

/// <summary>
/// 所有请求 DTO 可选实现此接口，定义请求的公共属性
/// </summary>
//[Union(0, typeof(LoginRequest))]
//[Union(1, typeof(RequestBase))]
public interface IRequest
{
    /// <summary>
    /// 请求唯一标识
    /// </summary>
    string RequestId { get; set; }

    /// <summary>
    /// 请求时间戳
    /// </summary>
    DateTime Timestamp { get; set; }
 
}
