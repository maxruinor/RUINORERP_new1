using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.ServerManagement;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 服务器管理命令响应基类
    /// </summary>
    public class ManagementCommandResponse : ResponseBase
    {
        /// <summary>
        /// 响应时间戳
        /// </summary>
        public DateTime ResponseTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 扩展方法：为所有管理命令响应类添加CreateErrorResponse
    /// </summary>
    public static class ManagementCommandResponseExtensions
    {
        /// <summary>
        /// 创建错误响应
        /// </summary>
        public static T CreateErrorResponse<T>(this T response, string message) where T : ManagementCommandResponse, new()
        {
            var errorResponse = new T
            {
                IsSuccess = false,
                Message = message,
                ResponseTime = DateTime.Now
            };
            return errorResponse;
        }

        /// <summary>
        /// 创建错误响应（带异常）
        /// </summary>
        public static T CreateErrorResponse<T>(this T response, string message, Exception exception) where T : ManagementCommandResponse, new()
        {
            var errorResponse = new T
            {
                IsSuccess = false,
                Message = message,
                ResponseTime = DateTime.Now
            };
            return errorResponse;
        }
    }

    /// <summary>
    /// 服务器注册响应
    /// </summary>
    public class ServerRegisterResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 是否注册成功
        /// </summary>
        public bool RegistrationSuccessful { get; set; }

        /// <summary>
        /// 分配的实例ID
        /// </summary>
        public Guid? AssignedInstanceId { get; set; }

        /// <summary>
        /// 同级服务器列表（用于集群发现）
        /// </summary>
        public List<ServerInstanceInfo> PeerServers { get; set; } = new List<ServerInstanceInfo>();

        /// <summary>
        /// 服务器授权信息
        /// </summary>
        public AuthorizationInfo AuthorizationInfo { get; set; }

        /// <summary>
        /// 初始配置信息
        /// </summary>
        public Dictionary<string, object> InitialConfiguration { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 服务器心跳响应（用于TopServer管理通讯）
    /// </summary>
    public class ServerHeartbeatResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 心跳确认
        /// </summary>
        public bool HeartbeatConfirmed { get; set; }

        /// <summary>
        /// 下次心跳间隔（秒）
        /// </summary>
        public int NextHeartbeatInterval { get; set; } = 30;

        /// <summary>
        /// 服务器端需要执行的操作
        /// </summary>
        public List<PendingOperation> PendingOperations { get; set; } = new List<PendingOperation>();
    }

    /// <summary>
    /// 状态上报响应
    /// </summary>
    public class StatusReportResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 状态确认
        /// </summary>
        public bool StatusConfirmed { get; set; }

        /// <summary>
        /// TopServer对状态的反馈
        /// </summary>
        public string StatusFeedback { get; set; }

        /// <summary>
        /// 建议的调整操作
        /// </summary>
        public List<string> SuggestedActions { get; set; } = new List<string>();
    }

    /// <summary>
    /// 用户信息上报响应
    /// </summary>
    public class UsersReportResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 接收确认
        /// </summary>
        public bool ReportAccepted { get; set; }

        /// <summary>
        /// 接收的用户数
        /// </summary>
        public int ReceivedUserCount { get; set; }
    }

    /// <summary>
    /// 配置上报响应
    /// </summary>
    public class ConfigurationReportResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 上报确认
        /// </summary>
        public bool ReportAccepted { get; set; }

        /// <summary>
        /// 配置是否需要更新
        /// </summary>
        public bool ConfigUpdateRequired { get; set; }

        /// <summary>
        /// 最新配置版本
        /// </summary>
        public string LatestConfigVersion { get; set; }
    }

    /// <summary>
    /// 配置更新响应
    /// </summary>
    public class ConfigurationUpdateResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 更新成功
        /// </summary>
        public bool UpdateSuccessful { get; set; }

        /// <summary>
        /// 应用配置的版本
        /// </summary>
        public string AppliedConfigVersion { get; set; }

        /// <summary>
        /// 是否需要重启
        /// </summary>
        public bool RestartRequired { get; set; }

        /// <summary>
        /// 重启时间（如果需要）
        /// </summary>
        public DateTime? RestartTime { get; set; }
    }

    /// <summary>
    /// 获取配置响应
    /// </summary>
    public class GetConfigurationResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 配置版本
        /// </summary>
        public string ConfigVersion { get; set; }

        /// <summary>
        /// 配置数据
        /// </summary>
        public Dictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 配置类型
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 是否有更新
        /// </summary>
        public bool HasUpdate { get; set; }
    }

    /// <summary>
    /// 用户活动上报响应
    /// </summary>
    public class UserActivityReportResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 接收确认
        /// </summary>
        public bool ReportAccepted { get; set; }

        /// <summary>
        /// 接收的活动数
        /// </summary>
        public int ReceivedActivityCount { get; set; }
    }

    /// <summary>
    /// 服务器授权查询响应
    /// </summary>
    public class AuthorizationQueryResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthorizationInfo AuthorizationInfo { get; set; }

        /// <summary>
        /// 授权是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 授权剩余天数
        /// </summary>
        public int RemainingDays { get; set; }
    }

    /// <summary>
    /// 服务器授权更新响应
    /// </summary>
    public class AuthorizationUpdateResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 更新成功
        /// </summary>
        public bool UpdateSuccessful { get; set; }

        /// <summary>
        /// 更新后的授权信息
        /// </summary>
        public AuthorizationInfo UpdatedAuthorizationInfo { get; set; }
    }

    /// <summary>
    /// 服务器实例列表查询响应
    /// </summary>
    public class QueryServerInstancesResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 服务器实例列表
        /// </summary>
        public List<RUINORERP.PacketSpec.Models.ServerManagement.ServerInstanceInfo> ServerInstances { get; set; } = new List<RUINORERP.PacketSpec.Models.ServerManagement.ServerInstanceInfo>();

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 服务器统计信息响应
    /// </summary>
    public class ServerStatisticsResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 服务器总数
        /// </summary>
        public int TotalServers { get; set; }

        /// <summary>
        /// 在线服务器数
        /// </summary>
        public int OnlineServers { get; set; }

        /// <summary>
        /// 离线服务器数
        /// </summary>
        public int OfflineServers { get; set; }

        /// <summary>
        /// 异常服务器数
        /// </summary>
        public int ExceptionServers { get; set; }

        /// <summary>
        /// 总用户数
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 在线用户数
        /// </summary>
        public int OnlineUsers { get; set; }

        /// <summary>
        /// 总连接数
        /// </summary>
        public int TotalConnections { get; set; }

        /// <summary>
        /// 平均CPU使用率
        /// </summary>
        public double AverageCpuUsage { get; set; }

        /// <summary>
        /// 平均内存使用率
        /// </summary>
        public double AverageMemoryUsage { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime StatisticsTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 服务器重启指令响应
    /// </summary>
    public class RestartServerResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 是否接受重启指令
        /// </summary>
        public bool RestartAccepted { get; set; }

        /// <summary>
        /// 计划重启时间
        /// </summary>
        public DateTime? PlannedRestartTime { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RejectReason { get; set; }
    }

    /// <summary>
    /// 服务器停止指令响应
    /// </summary>
    public class StopServerResponse : ManagementCommandResponse
    {
        /// <summary>
        /// 是否接受停止指令
        /// </summary>
        public bool StopAccepted { get; set; }

        /// <summary>
        /// 计划停止时间
        /// </summary>
        public DateTime? PlannedStopTime { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RejectReason { get; set; }
    }

    #region 辅助数据模型

    /// <summary>
    /// 授权信息
    /// </summary>
    public class AuthorizationInfo
    {
        /// <summary>
        /// 服务器ID
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public ManagementCommands.AuthorizationType AuthType { get; set; }

        /// <summary>
        /// 授权开始时间
        /// </summary>
        public DateTime? AuthStartTime { get; set; }

        /// <summary>
        /// 授权结束时间
        /// </summary>
        public DateTime? AuthEndTime { get; set; }

        /// <summary>
        /// 最大用户数
        /// </summary>
        public int MaxUsers { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public string AuthStatus { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public Dictionary<string, object> AuthInfo { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 待执行操作
    /// </summary>
    public class PendingOperation
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 操作优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 操作参数
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 操作说明
        /// </summary>
        public string Description { get; set; }
    }

    #endregion
}
