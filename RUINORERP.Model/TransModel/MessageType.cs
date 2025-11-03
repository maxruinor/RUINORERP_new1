using System.ComponentModel;

namespace RUINORERP.Model.TransModel
{
    /// <summary>
    /// 统一的消息命令类型枚举
    /// 整合了原有的MessageType和MessageCmdType
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        
        /// <summary>
        /// 普通消息
        /// </summary>
        [Description("普通消息")]
        Message = 1,
        
        /// <summary>
        /// 提醒消息
        /// </summary>
        [Description("提醒")]
        Reminder = 2,
        
        /// <summary>
        /// 事件消息
        /// </summary>
        [Description("事件")]
        Event = 3,
        
        /// <summary>
        /// 任务消息
        /// </summary>
        [Description("任务")]
        Task = 4,
        
        /// <summary>
        /// 通知消息
        /// </summary>
        [Description("通知")]
        Notice = 5,
        
        /// <summary>
        /// 业务消息
        /// </summary>
        [Description("业务")]
        Business = 6,
        
        /// <summary>
        /// 提示消息
        /// </summary>
        [Description("提示")]
        Prompt = 7,
        
        /// <summary>
        /// 解锁请求
        /// </summary>
        [Description("解锁请求")]
        UnLockRequest = 8,
        
        /// <summary>
        /// 异常日志
        /// </summary>
        [Description("异常日志")]
        ExceptionLog = 9,
        
        /// <summary>
        /// 广播消息
        /// </summary>
        [Description("广播")]
        Broadcast = 10,
        
        /// <summary>
        /// 审批消息
        /// </summary>
        [Description("审批")]
        Approve = 11,
        
        /// <summary>
        /// 系统消息
        /// </summary>
        [Description("系统")]
        System = 12,
        
        /// <summary>
        /// 文本消息
        /// </summary>
        [Description("文本")]
        Text = 13,
        
        /// <summary>
        /// 即时消息
        /// </summary>
        [Description("即时消息")]
        IM = 14,
        
        /// <summary>
        /// 业务数据
        /// </summary>
        [Description("业务数据")]
        BusinessData = 15,
        
        /// <summary>
        /// 用户消息
        /// </summary>
        [Description("用户消息")]
        UserMessage = 16
    }
}