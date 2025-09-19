using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Core
{
 
    /// <summary>
    /// 数据包方向枚举
    /// </summary>
    public enum PacketDirection
    {
        /// <summary>
        /// 未知方向
        /// </summary>
        [Description("未知方向")]
        Unknown = 0,

        /// <summary>
        /// 客户端到服务器
        /// </summary>
        [Description("客户端到服务器")]
        ClientToServer = 1,

        /// <summary>
        /// 服务器到客户端
        /// </summary>
        [Description("服务器到客户端")]
        ServerToClient = 2,

        /// <summary>
        /// 双向通信
        /// </summary>
        [Description("双向通信")]
        Bidirectional = 3,
        Response = 4,
        Request = 5
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
