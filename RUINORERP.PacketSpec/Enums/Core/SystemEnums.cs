using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Core
{

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
        Critical = 3,
        Realtime = 4
    }


    
}
