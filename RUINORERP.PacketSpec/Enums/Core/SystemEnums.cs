using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Core
{
    /// <summary>
    /// 系统基础命令枚举
    /// </summary>
    public enum SystemCommand : uint
    {
        /// <summary>
        /// 空命令/心跳包
        /// </summary>
        [Description("空命令/心跳包")]
        None = 0x0000,

        /// <summary>
        /// 心跳包
        /// </summary>
        [Description("心跳包")]
        Heartbeat = 0x0001,

        /// <summary>
        /// 心跳回复
        /// </summary>
        [Description("心跳回复")]
        HeartbeatResponse = 0x0002,

        /// <summary>
        /// 系统状态查询
        /// </summary>
        [Description("系统状态查询")]
        SystemStatus = 0x0003,

        /// <summary>
        /// 异常报告
        /// </summary>
        [Description("异常报告")]
        ExceptionReport = 0x0004
    }

    /// <summary>
    /// 数据包类型枚举
    /// </summary>
    public enum PacketType
    {
        // 系统相关
        System = 1000,
        SystemResponse = 1001,
        
        // 数据同步相关
        DataSync = 2000,
        DataSyncResponse = 2001,
        
        // 工作流相关
        Workflow = 3000,
        WorkflowResponse = 3001,
        
        // 用户认证相关
        Authentication = 4000,
        AuthenticationResponse = 4001,
        
        // 业务数据相关
        BusinessData = 5000,
        BusinessDataResponse = 5001,
        
        // 文件传输相关
        FileTransfer = 6000,
        FileTransferResponse = 6001,
        
        // 通知相关
        Notification = 7000,
        NotificationResponse = 7001,
        
        // 心跳和连接管理
        Heartbeat = 8000,
        HeartbeatResponse = 8001,
        
        // 错误和异常
        Error = 9000,
        
        // 通用响应
        GenericResponse = 9999
    }
    
    /// <summary>
    /// 数据包优先级
    /// </summary>
    public enum PacketPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3
    }

 


    /// <summary>
    /// 过滤器状态枚举
    /// </summary>
    public enum FilterState
    {
        /// <summary>
        /// 准备就绪
        /// </summary>
        Ready,
        
        /// <summary>
        /// 处理中
        /// </summary>
        Processing,
        
        /// <summary>
        /// 错误状态
        /// </summary>
        Error,
        
        /// <summary>
        /// 完成状态
        /// </summary>
        Completed
    }
}
