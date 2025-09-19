using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Core
{
    /// <summary>
    /// 数据包命令枚举
    /// </summary>
    public enum PacketCommand : uint
    {
        /// <summary>
        /// 未知命令
        /// </summary>
        [Description("未知命令")]
        Unknown = 0,

        /// <summary>
        /// 心跳包
        /// </summary>
        [Description("心跳包")]
        Heartbeat = 1,

        /// <summary>
        /// 登录请求
        /// </summary>
        [Description("登录请求")]
        Login = 2,

        /// <summary>
        /// 登录响应
        /// </summary>
        [Description("登录响应")]
        LoginResponse = 3,

        /// <summary>
        /// 查询请求
        /// </summary>
        [Description("查询请求")]
        Query = 4,

        /// <summary>
        /// 查询响应
        /// </summary>
        [Description("查询响应")]
        QueryResponse = 5,

        /// <summary>
        /// 文件上传
        /// </summary>
        [Description("文件上传")]
        FileUpload = 6,

        /// <summary>
        /// 文件下载
        /// </summary>
        [Description("文件下载")]
        FileDownload = 7,

        /// <summary>
        /// 文件删除
        /// </summary>
        [Description("文件删除")]
        FileDelete = 8,

        /// <summary>
        /// 错误响应
        /// </summary>
        [Description("错误响应")]
        Error = 9,

        /// <summary>
        /// 成功响应
        /// </summary>
        [Description("成功响应")]
        Success = 10,

        /// <summary>
        /// 数据同步
        /// </summary>
        [Description("数据同步")]
        DataSync = 11,

        /// <summary>
        /// 缓存更新
        /// </summary>
        [Description("缓存更新")]
        CacheUpdate = 12,

        /// <summary>
        /// 通知消息
        /// </summary>
        [Description("通知消息")]
        Notification = 13,

        /// <summary>
        /// 广播消息
        /// </summary>
        [Description("广播消息")]
        Broadcast = 14,

        /// <summary>
        /// 系统命令
        /// </summary>
        [Description("系统命令")]
        System = 15
    }

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
        Bidirectional = 3
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
        Cancelled = 7
    }
}