using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Message
{

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
