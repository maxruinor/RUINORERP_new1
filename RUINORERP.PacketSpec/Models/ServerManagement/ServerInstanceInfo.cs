using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.ServerManagement
{
    /// <summary>
    /// 服务器实例信息类（公共模型）
    /// 用于TopServer和Server之间的通讯和管理
    /// </summary>
    public class ServerInstanceInfo
    {
        /// <summary>
        /// 实例ID
        /// </summary>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// 服务器ID
        /// </summary>
        public string ServerId { get; set; } = string.Empty;

        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName { get; set; } = string.Empty;

        /// <summary>
        /// 服务器类型
        /// </summary>
        public string ServerType { get; set; } = string.Empty;

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        public DateTime LastHeartbeatTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ServerInstanceStatus Status { get; set; }

        /// <summary>
        /// 在线用户数
        /// </summary>
        public int OnlineUserCount { get; set; }

        /// <summary>
        /// 当前指标
        /// </summary>
        public ServerMetrics Metrics { get; set; }

        /// <summary>
        /// 服务器能力
        /// </summary>
        public ServerCapabilities Capabilities { get; set; }
    }

    /// <summary>
    /// 服务器实例状态枚举
    /// </summary>
    public enum ServerInstanceStatus
    {
        /// <summary>
        /// 离线
        /// </summary>
        Offline = 0,
        /// <summary>
        /// 在线
        /// </summary>
        Online = 1,
        /// <summary>
        /// 异常
        /// </summary>
        Exception = 2
    }

    /// <summary>
    /// 服务器指标
    /// </summary>
    public class ServerMetrics
    {
        /// <summary>
        /// CPU使用率（百分比）
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// 内存使用率（百分比）
        /// </summary>
        public double MemoryUsage { get; set; }

        /// <summary>
        /// 磁盘使用率（百分比）
        /// </summary>
        public double DiskUsage { get; set; }

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections { get; set; }

        /// <summary>
        /// 平均响应时间（毫秒）
        /// </summary>
        public double AverageResponseTime { get; set; }

        /// <summary>
        /// 总请求数
        /// </summary>
        public long TotalRequests { get; set; }

        /// <summary>
        /// 错误数
        /// </summary>
        public long ErrorCount { get; set; }

        /// <summary>
        /// 运行时间（秒）
        /// </summary>
        public long UptimeSeconds { get; set; }

        /// <summary>
        /// 网络接收字节数
        /// </summary>
        public long NetworkBytesReceived { get; set; }

        /// <summary>
        /// 网络发送字节数
        /// </summary>
        public long NetworkBytesSent { get; set; }
    }

    /// <summary>
    /// 服务器能力
    /// </summary>
    public class ServerCapabilities
    {
        /// <summary>
        /// 支持的业务类型
        /// </summary>
        public List<string> SupportedBusinessTypes { get; set; } = new List<string>();

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnections { get; set; }

        /// <summary>
        /// 最大并发数
        /// </summary>
        public int MaxConcurrency { get; set; }

        /// <summary>
        /// 支持的命令类别
        /// </summary>
        public List<string> SupportedCommandCategories { get; set; } = new List<string>();

        /// <summary>
        /// 特性列表
        /// </summary>
        public Dictionary<string, object> Features { get; set; } = new Dictionary<string, object>();
    }
}
