using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Message
{
    /// <summary>
    /// 消息通信命令枚举
    /// </summary>
    public enum MessageCommand : uint
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        [Description("发送消息")]
        SendMessage = 0x0300,

        /// <summary>
        /// 接收消息
        /// </summary>
        [Description("接收消息")]
        ReceiveMessage = 0x0301,

        /// <summary>
        /// 消息已读确认
        /// </summary>
        [Description("消息已读确认")]
        MessageRead = 0x0302,

        /// <summary>
        /// 广播消息
        /// </summary>
        [Description("广播消息")]
        BroadcastMessage = 0x0303,

        /// <summary>
        /// 发送弹窗消息
        /// </summary>
        [Description("发送弹窗消息")]
        SendPopupMessage = 0x0304,

        /// <summary>
        /// 转发弹窗消息
        /// </summary>
        [Description("转发弹窗消息")]
        ForwardPopupMessage = 0x0305,

        /// <summary>
        /// 消息响应
        /// </summary>
        [Description("消息响应")]
        MessageResponse = 0x0306,

        /// <summary>
        /// 转发消息结果
        /// </summary>
        [Description("转发消息结果")]
        ForwardMessageResult = 0x0307,

        /// <summary>
        /// 系统消息
        /// </summary>
        [Description("系统消息")]
        SystemMessage = 0x0308,

        /// <summary>
        /// 提示消息
        /// </summary>
        [Description("提示消息")]
        NotificationMessage = 0x0309
    }


    /// <summary>
    /// 消息类型枚举
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        Text = 0,

        /// <summary>
        /// 提示消息
        /// </summary>
        Prompt = 1,

        /// <summary>
        /// 即时消息
        /// </summary>
        IM = 2,

        /// <summary>
        /// 业务数据
        /// </summary>
        BusinessData = 3,

        /// <summary>
        /// 事件消息
        /// </summary>
        Event = 4,

        /// <summary>
        /// 系统消息
        /// </summary>
        System = 5,

        /// <summary>
        /// 未知消息
        /// </summary>
        Unknown = 6,

        /// <summary>
        /// 通知消息
        /// </summary>
        Notification = 7,

        /// <summary>
        /// 用户消息
        /// </summary>
        UserMessage = 9
    }

    /// <summary>
    /// 消息提示类型
    /// </summary>
    public enum PromptType
    {
        /// <summary>
        /// 信息提示
        /// </summary>
        Information = 0,

        /// <summary>
        /// 警告提示
        /// </summary>
        Warning = 1,

        /// <summary>
        /// 错误提示
        /// </summary>
        Error = 2,

        /// <summary>
        /// 确认窗口
        /// </summary>
        Confirmation = 3,

        /// <summary>
        /// 输入窗口
        /// </summary>
        Input = 4,

        /// <summary>
        /// 日志消息
        /// </summary>
        LogMessage = 5,

        /// <summary>
        /// 通知窗口
        /// </summary>
        Notification = 6,


        Success = 7,
    }


}
