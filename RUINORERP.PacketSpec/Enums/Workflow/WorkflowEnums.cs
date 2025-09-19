using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Workflow
{
    ///// <summary>
    ///// 工作流命令枚举
    ///// </summary>
    //public enum WorkflowCommand : uint
    //{
       
    //    /// <summary>
    //    /// 工作流提醒推送
    //    /// </summary>
    //    [Description("工作流提醒推送")]
    //    WorkflowReminderPush = 0x040B,

    //    /// <summary>
    //    /// 工作流数据推送
    //    /// </summary>
    //    [Description("工作流数据推送")]
    //    WorkflowDataPush = 0x040C
    //}

    /// <summary>
    /// 下一步处理步骤
    /// </summary>
    public enum NextProcessStep
    {
        /// <summary>
        /// 无操作
        /// </summary>
        NoOperation = 0,

        /// <summary>
        /// 转发
        /// </summary>
        Forward = 1,

        /// <summary>
        /// 回复
        /// </summary>
        Reply = 2,

        /// <summary>
        /// 广播
        /// </summary>
        Broadcast = 3,

        /// <summary>
        /// 保存/存储
        /// </summary>
        Store = 4,

        /// <summary>
        /// 确认
        /// </summary>
        Confirm = 5,
    }

    /// <summary>
    /// 安全库存提醒类型
    /// </summary>
    public enum SafetyStockAlertType
    {
        /// <summary>
        /// 低于安全库存
        /// </summary>
        BelowSafetyStock = 1,

        /// <summary>
        /// 低于最低库存
        /// </summary>
        BelowMinimumStock = 2,

        /// <summary>
        /// 高于最高库存
        /// </summary>
        AboveMaximumStock = 3,

        /// <summary>
        /// 库存预警
        /// </summary>
        StockWarning = 4,

        /// <summary>
        /// 库存紧急
        /// </summary>
        StockEmergency = 5,

        /// <summary>
        /// 需要补货
        /// </summary>
        NeedReplenishment = 6,

        /// <summary>
        /// 库存过期预警
        /// </summary>
        StockExpiryWarning = 7,

        /// <summary>
        /// 库存周转率低
        /// </summary>
        LowStockTurnover = 8
    }

    /// <summary>
    /// 安全库存提醒优先级
    /// </summary>
    public enum SafetyStockAlertPriority
    {
        /// <summary>
        /// 低优先级
        /// </summary>
        Low = 0,

        /// <summary>
        /// 普通优先级
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 高优先级
        /// </summary>
        High = 2,

        /// <summary>
        /// 紧急优先级
        /// </summary>
        Urgent = 3
    }

    /// <summary>
    /// 安全库存处理状态
    /// </summary>
    public enum SafetyStockProcessStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 1,

        /// <summary>
        /// 已处理
        /// </summary>
        Processed = 2,

        /// <summary>
        /// 已忽略
        /// </summary>
        Ignored = 3,

        /// <summary>
        /// 已转发
        /// </summary>
        Forwarded = 4,

        /// <summary>
        /// 需要确认
        /// </summary>
        NeedsConfirmation = 5
    }
}
