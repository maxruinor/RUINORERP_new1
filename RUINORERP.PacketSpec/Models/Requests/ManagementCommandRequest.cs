using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.ServerManagement;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 服务器管理命令请求基类
    /// 用于TopServer与子服务器之间的管理通信
    /// </summary>
    public class ManagementCommandRequest : RequestBase
    {
        /// <summary>
        /// 服务器实例ID
        /// </summary>
        public Guid? ServerInstanceId { get; set; }

        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime RequestTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 请求元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 服务器注册请求
    /// </summary>
    public class ServerRegisterRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 服务器ID（唯一标识）
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 服务器类型（业务服务器、文件服务器等）
        /// </summary>
        public string ServerType { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务器版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 服务器能力描述
        /// </summary>
        public ServerCapabilities Capabilities { get; set; } = new ServerCapabilities();

        /// <summary>
        /// 认证令牌
        /// </summary>
        public string AuthToken { get; set; }
    }

    /// <summary>
    /// 服务器心跳请求（用于TopServer管理通讯）
    /// </summary>
    public class ServerHeartbeatRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 服务器指标
        /// </summary>
        public ServerMetrics Metrics { get; set; }
    }

    /// <summary>
    /// 状态上报请求
    /// </summary>
    public class StatusReportRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 服务器状态
        /// </summary>
        public ManagementCommands.ServerStatus Status { get; set; }

        /// <summary>
        /// 服务器指标
        /// </summary>
        public ServerMetrics Metrics { get; set; }

        /// <summary>
        /// 在线用户数
        /// </summary>
        public int OnlineUserCount { get; set; }

        /// <summary>
        /// 活跃连接数
        /// </summary>
        public int ActiveConnectionCount { get; set; }
    }

    /// <summary>
    /// 用户信息上报请求
    /// </summary>
    public class UsersReportRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 在线用户列表
        /// </summary>
        public List<UserInfo> OnlineUsers { get; set; } = new List<UserInfo>();

        /// <summary>
        /// 总用户数
        /// </summary>
        public int TotalUserCount { get; set; }

        /// <summary>
        /// 上报时间范围
        /// </summary>
        public TimeSpan ReportTimeRange { get; set; } = TimeSpan.FromMinutes(5);
    }

    /// <summary>
    /// 配置上报请求
    /// </summary>
    public class ConfigurationReportRequest : ManagementCommandRequest
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
    }

    /// <summary>
    /// 配置更新请求（TopServer下发）
    /// </summary>
    public class ConfigurationUpdateRequest : ManagementCommandRequest
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
        /// 是否强制更新
        /// </summary>
        public bool ForceUpdate { get; set; }

        /// <summary>
        /// 更新后是否重启服务
        /// </summary>
        public bool RestartAfterUpdate { get; set; }
    }

    /// <summary>
    /// 获取配置请求
    /// </summary>
    public class GetConfigurationRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 当前配置版本
        /// </summary>
        public string CurrentConfigVersion { get; set; }
    }

    /// <summary>
    /// 用户活动上报请求
    /// </summary>
    public class UserActivityReportRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 活动记录列表
        /// </summary>
        public List<UserActivity> Activities { get; set; } = new List<UserActivity>();

        /// <summary>
        /// 上报时间范围
        /// </summary>
        public TimeSpan ReportTimeRange { get; set; } = TimeSpan.FromMinutes(5);
    }

    /// <summary>
    /// 服务器授权查询请求
    /// </summary>
    public class AuthorizationQueryRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 服务器ID
        /// </summary>
        public string ServerId { get; set; }
    }

    /// <summary>
    /// 服务器授权更新请求
    /// </summary>
    public class AuthorizationUpdateRequest : ManagementCommandRequest
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
        /// 授权信息
        /// </summary>
        public Dictionary<string, object> AuthInfo { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 服务器实例列表查询请求
    /// </summary>
    public class QueryServerInstancesRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 过滤条件 - 服务器类型
        /// </summary>
        public string ServerTypeFilter { get; set; }

        /// <summary>
        /// 过滤条件 - 服务器状态
        /// </summary>
        public ManagementCommands.ServerStatus? StatusFilter { get; set; }

        /// <summary>
        /// 分页参数 - 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页参数 - 每页大小
        /// </summary>
        public int PageSize { get; set; } = 50;
    }

    /// <summary>
    /// 服务器重启指令请求
    /// </summary>
    public class RestartServerRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 目标服务器ID
        /// </summary>
        public string TargetServerId { get; set; }

        /// <summary>
        /// 重启原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 延迟时间（秒）
        /// </summary>
        public int DelaySeconds { get; set; } = 0;

        /// <summary>
        /// 强制重启
        /// </summary>
        public bool ForceRestart { get; set; }
    }

    /// <summary>
    /// 服务器停止指令请求
    /// </summary>
    public class StopServerRequest : ManagementCommandRequest
    {
        /// <summary>
        /// 目标服务器ID
        /// </summary>
        public string TargetServerId { get; set; }

        /// <summary>
        /// 停止原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 延迟时间（秒）
        /// </summary>
        public int DelaySeconds { get; set; } = 0;

        /// <summary>
        /// 强制停止
        /// </summary>
        public bool ForceStop { get; set; }
    }

    #region 辅助数据模型

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }
    }

    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 在线
        /// </summary>
        Online = 0,

        /// <summary>
        /// 离线
        /// </summary>
        Offline = 1,

        /// <summary>
        /// 忙碌
        /// </summary>
        Busy = 2,

        /// <summary>
        /// 离开
        /// </summary>
        Away = 3
    }

    /// <summary>
    /// 用户活动
    /// </summary>
    public class UserActivity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// 活动时间
        /// </summary>
        public DateTime ActivityTime { get; set; }

        /// <summary>
        /// 活动详情
        /// </summary>
        public string ActivityDetail { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }
    }

    #endregion
}
