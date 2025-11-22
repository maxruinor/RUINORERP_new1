using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Core
{
 
    /// <summary>
    /// 数据包方向枚举
    /// </summary>
    public enum PacketDirection
    {
        /// <summary>
        /// 未知方向 - 初始状态或未定义
        /// </summary>
        [Description("未知")]
        Unknown = 0,

        /// <summary>
        /// 客户端发起的请求 - 期待服务器响应
        /// </summary>
        [Description("客户端请求")]
        ClientRequest = 1,

        /// <summary>
        /// 服务器对客户端请求的响应
        /// </summary>
        [Description("服务器响应")]
        ServerResponse = 2,

        /// <summary>
        /// 服务器主动推送 - 客户端是否响应可选
        /// </summary>
        [Description("服务器推送")]
        ServerRequest = 3,

        /// <summary>
        /// 客户端响应 - 双向通信场景
        /// </summary>
        [Description("客户端响应")]
        ClientResponse = 4,
    }

    /// <summary>
    /// 数据包状态枚举
    /// </summary>
    public enum PacketStatus
    {
        /// <summary>
        /// 已创建
        /// </summary>
        [Description("已创建")]
        Created = 0,

        /// <summary>
        /// 已发送
        /// </summary>
        [Description("已发送")]
        Sent = 1,

        /// <summary>
        /// 已接收
        /// </summary>
        [Description("已接收")]
        Received = 2,

        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Processing = 3,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed = 4,

        /// <summary>
        /// 已失败
        /// </summary>
        [Description("已失败")]
        Failed = 5,

        /// <summary>
        /// 已超时
        /// </summary>
        [Description("已超时")]
        Timeout = 6,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancelled = 7,
        Error = 8
    }
}
