using System.ComponentModel;

namespace RUINORERP.Model.TransModel
{
    /// <summary>
    /// 统一的消息命令类型枚举
    /// 精简为3个核心类别：弹出式提醒、业务性提醒和系统消息
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 弹出式提醒
        /// 需要立即用户注意的消息
        /// </summary>
        [Description("弹出式提醒")]
        Popup = 1,
        
        /// <summary>
        /// 业务性提醒
        /// 业务流程相关的消息
        /// </summary>
        [Description("业务性提醒")]
        Business = 2,
        
        /// <summary>
        /// 系统消息
        /// 系统级通知和日志
        /// </summary>
        [Description("系统消息")]
        System = 3
    }
}