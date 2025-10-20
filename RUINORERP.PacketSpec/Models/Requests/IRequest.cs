using System;
using System.Collections.Generic;
using MessagePack;
using RUINORERP.PacketSpec.Models.Requests;

/// <summary>
/// 所有请求 DTO 可选实现此接口，定义请求的公共属性
/// </summary>
//[Union(0, typeof(RequestBase))]
//[Union(1, typeof(FileUploadRequest))]
//[Union(2, typeof(FileDownloadRequest))]
//[Union(3, typeof(FileDeleteRequest))]
//[Union(4, typeof(FileInfoRequest))]
//[Union(5, typeof(FileListRequest))]//这些行不能加。加了无法解析出数据
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
