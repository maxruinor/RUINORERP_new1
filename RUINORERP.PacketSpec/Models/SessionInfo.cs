using System;
using System.Net;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 会话信息 - 用于会话管理和状态跟踪
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 客户端端口
        /// </summary>
        public int ClientPort { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectedTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// 心跳计数器
        /// </summary>
        public int HeartbeatCount { get; set; }

        /// <summary>
        /// 心跳检查计数器
        /// </summary>
        public int HeartbeatCheckCount { get; set; }

        /// <summary>
        /// 会话状态
        /// </summary>
        public SessionStatus Status { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 是否已认证
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 会话超时时间（分钟）
        /// </summary>
        public int TimeoutMinutes { get; set; }

        /// <summary>
        /// 发送数据包计数
        /// </summary>
        public long SentPacketsCount { get; set; }

        /// <summary>
        /// 接收数据包计数
        /// </summary>
        public long ReceivedPacketsCount { get; set; }

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 创建会话信息
        /// </summary>
        public static SessionInfo Create(string sessionId, string clientIp, int clientPort = 0)
        {
            var now = DateTime.UtcNow;
            return new SessionInfo
            {
                SessionId = sessionId,
                ClientIp = clientIp,
                ClientPort = clientPort,
                ConnectedTime = now,
                LastActivityTime = now,
                Status = SessionStatus.Connected,
                TimeoutMinutes = 30, // 默认30分钟超时
                HeartbeatCount = 0,
                HeartbeatCheckCount = 0
            };
        }

        /// <summary>
        /// 更新最后活动时间
        /// </summary>
        public void UpdateActivity()
        {
            LastActivityTime = DateTime.UtcNow;
            HeartbeatCount++;
        }

        /// <summary>
        /// 检查会话是否超时
        /// </summary>
        public bool IsTimeout()
        {
            return (DateTime.UtcNow - LastActivityTime).TotalMinutes > TimeoutMinutes;
        }

        /// <summary>
        /// 检查心跳是否正常
        /// </summary>
        public bool IsHeartbeatNormal()
        {
            return HeartbeatCount > HeartbeatCheckCount;
        }

        /// <summary>
        /// 验证会话信息有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SessionId) &&
                   !string.IsNullOrEmpty(ClientIp) &&
                   ConnectedTime <= DateTime.UtcNow.AddMinutes(5) &&
                   ConnectedTime >= DateTime.UtcNow.AddMinutes(-5);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"SessionId: {SessionId}, Client: {ClientIp}:{ClientPort}, Status: {Status}, User: {Username}";
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 部门信息
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        public static UserInfo Create(int userId, string username, string sessionId = null, string clientIp = null)
        {
            return new UserInfo
            {
                UserId = userId,
                Username = username,
                SessionId = sessionId,
                ClientIp = clientIp,
                IsOnline = true,
                LastLoginTime = DateTime.UtcNow,
                Roles = Array.Empty<string>()
            };
        }

        /// <summary>
        /// 验证用户信息有效性
        /// </summary>
        public bool IsValid()
        {
            return UserId > 0 && 
                   !string.IsNullOrEmpty(Username) &&
                   LastLoginTime <= DateTime.UtcNow.AddMinutes(10) &&
                   LastLoginTime >= DateTime.UtcNow.AddMinutes(-10);
        }
    }

    /// <summary>
    /// 会话状态枚举
    /// </summary>
    public enum SessionStatus
    {
        /// <summary>
        /// 已连接
        /// </summary>
        Connected = 0,

        /// <summary>
        /// 已认证
        /// </summary>
        Authenticated = 1,

        /// <summary>
        /// 活动中
        /// </summary>
        Active = 2,

        /// <summary>
        /// 空闲中
        /// </summary>
        Idle = 3,

        /// <summary>
        /// 已断开
        /// </summary>
        Disconnected = 4,

        /// <summary>
        /// 错误状态
        /// </summary>
        Error = 5,

        /// <summary>
        /// 超时状态
        /// </summary>
        Timeout = 6
    }

    /// <summary>
    /// 会话统计信息
    /// </summary>
    public class SessionStatistics
    {
        /// <summary>
        /// 总连接数
        /// </summary>
        public long TotalConnections { get; set; }

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections { get; set; }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnections { get; set; }

        /// <summary>
        /// 总数据发送量（字节）
        /// </summary>
        public long TotalBytesSent { get; set; }

        /// <summary>
        /// 总数据接收量（字节）
        /// </summary>
        public long TotalBytesReceived { get; set; }

        /// <summary>
        /// 总错误数
        /// </summary>
        public long TotalErrors { get; set; }

        /// <summary>
        /// 总超时数
        /// </summary>
        public long TotalTimeouts { get; set; }

        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public DateTime ServerStartTime { get; set; }

        /// <summary>
        /// 服务器运行时间
        /// </summary>
        public TimeSpan Uptime => DateTime.UtcNow - ServerStartTime;

        /// <summary>
        /// 创建会话统计信息
        /// </summary>
        public static SessionStatistics Create(int maxConnections = 1000)
        {
            return new SessionStatistics
            {
                ServerStartTime = DateTime.UtcNow,
                MaxConnections = maxConnections,
                CurrentConnections = 0,
                TotalConnections = 0
            };
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        public void UpdateConnectionStats(int connections, long bytesSent = 0, long bytesReceived = 0)
        {
            CurrentConnections = connections;
            TotalBytesSent += bytesSent;
            TotalBytesReceived += bytesReceived;
            
            if (connections > MaxConnections)
            {
                MaxConnections = connections;
            }
        }

        /// <summary>
        /// 获取连接利用率
        /// </summary>
        public double GetConnectionUtilization()
        {
            return MaxConnections > 0 ? (double)CurrentConnections / MaxConnections * 100 : 0;
        }

        /// <summary>
        /// 获取平均吞吐量（字节/秒）
        /// </summary>
        public double GetAverageThroughput()
        {
            var totalSeconds = Uptime.TotalSeconds;
            return totalSeconds > 0 ? (TotalBytesSent + TotalBytesReceived) / totalSeconds : 0;
        }
    }
}