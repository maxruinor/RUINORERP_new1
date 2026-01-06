using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{
    /// <summary>
    /// 消息确认状态枚举
    /// 表示消息的确认处理状态
    /// </summary>
    public enum ConfirmStatus
    {
        /// <summary>
        /// 未确认
        /// 默认状态，消息尚未被确认
        /// </summary>
        Unconfirmed = 0,

        /// <summary>
        /// 已确认
        /// 消息已被接收方确认
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// 已拒绝
        /// 消息被接收方拒绝
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// 处理中
        /// 消息正在处理中
        /// </summary>
        Processing = 3,

        /// <summary>
        /// 已完成
        /// 消息相关的任务已完成
        /// </summary>
        Completed = 4
    }

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

    /// <summary>
    /// 消息状态枚举
    /// 表示消息的处理状态
    /// </summary>
    public enum MessageStatus
    {
        /// <summary>
        /// 未读
        /// </summary>
        Unread,

        /// <summary>
        /// 已读
        /// </summary>
        Read,

        /// <summary>
        /// 未处理
        /// </summary>
        Unprocessed,

        /// <summary>
        /// 已处理
        /// </summary>
        Processed,

        /// <summary>
        /// 稍候提醒
        /// </summary>
        WaitRminder,

        Cancel,
    }

    /// <summary>
    /// 消息优先级枚举
    /// 定义消息的优先级
    /// </summary>
    public enum MessagePriority
    {
        /// <summary>
        /// 一般消息
        /// </summary>
        General,

        /// <summary>
        /// 重要消息
        /// </summary>
        Important,

        /// <summary>
        /// 紧急消息
        /// </summary>
        Exception
    }
}