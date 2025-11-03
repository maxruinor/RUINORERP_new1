using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Message
{
 

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
