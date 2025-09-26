using System;

namespace RUINORERP.PacketSpec.Enums.Core
{
    /// <summary>
    /// 消息类型枚举
    /// 用于明确区分数据包的类型
    /// </summary>
    [Serializable]
    public enum MessageType
    {
        /// <summary>
        /// 请求类型消息
        /// 从客户端发送到服务器，需要服务器响应
        /// </summary>
        Request = 0,
        
        /// <summary>
        /// 响应类型消息
        /// 从服务器发送到客户端，回应客户端的请求
        /// </summary>
        Response = 1,
        
        /// <summary>
        /// 通知类型消息
        /// 单向消息，不需要接收方响应
        /// </summary>
        Notification = 2
    }
}