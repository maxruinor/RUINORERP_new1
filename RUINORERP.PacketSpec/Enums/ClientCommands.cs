namespace RUINORERP.PacketSpec.Enums
{
    /// <summary>
    /// 客户端指令枚举
    /// </summary>
    public enum ClientCommand : uint
    {
        /// <summary>
        /// 空指令
        /// </summary>
        None = 0x0,
        
        /// <summary>
        /// 认证相关指令 (0x01-0x10)
        /// </summary>
        PrepareLogin = 0x01,
        UserLogin = 0x02,
        
        /// <summary>
        /// 缓存相关指令 (0x03-0x07)
        /// </summary>
        RequestCache = 0x03,
        UpdateCache = 0x04,
        DeleteCache = 0x07,
        UpdateDynamicConfig = 0x10,
        
        /// <summary>
        /// 工作流相关指令 (0x08-0x09, 0x12)
        /// </summary>
        WorkflowStart = 0x08,
        WorkflowCommand = 0x09,
        WorkflowApprove = 0x12,
        
        /// <summary>
        /// 系统管理指令 (0x05-0x06, 0x11, 0x15, 0x19)
        /// </summary>
        ReportException = 0x05,
        RequestAssistance = 0x06,
        SendPopupMessage = 0x11,
        RequestForceLogout = 0x15,
        RequestForceLogin = 0x19,
        
        /// <summary>
        /// 提醒相关指令 (0x16-0x18)
        /// </summary>
        WorkflowReminderRequest = 0x16,
        WorkflowReminderChange = 0x17,
        WorkflowReminderResponse = 0x18,
        
        /// <summary>
        /// 复合指令 (0x20-0x24)
        /// </summary>
        CompositeWorkflowRequest = 0x20,
        CompositeLoginRequest = 0x21,
        CompositeEntityRequest = 0x22,
        CompositeMessageProcessing = 0x23,
        CompositeLockProcessing = 0x24,
        
        /// <summary>
        /// 心跳包
        /// </summary>
        Heartbeat = 0x99,
        
        /// <summary>
        /// 其他特殊指令
        /// </summary>
        OpenHelp = 0x90156,
        DeleteHelpItem = 0x90158,
        SwitchLineLogin = 0x90091,
        CharacterWaiting = 0x90092
    }

    /// <summary>
    /// 客户端子指令枚举（工作流操作类型）
    /// </summary>
    public enum ClientSubCommand : byte
    {
        Approve = 0x90,
        Reject = 0x91
    }

    /// <summary>
    /// 工作流业务类型枚举
    /// </summary>
    public enum WorkflowBusinessType
    {
        BasicDataPush = 1,
        ReminderBusinessPush = 2
    }

    /// <summary>
    /// 数据包来源类型
    /// </summary>
    public enum PackageSourceType
    {
        Client,
        Server
    }
}