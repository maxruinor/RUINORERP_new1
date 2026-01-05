using System;
using System.Collections.Generic;
using System.Threading;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.TopServer.Network
{
    /// <summary>
    /// 服务器会话信息 - 用于管理服务器与业务服务器之间的会话管理
    /// 在管理服务器中独立定义，避免直接引用业务服务器项目
    /// </summary>
    public class ServerSession
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 客户端端口
        /// </summary>
        public int ClientPort { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; } = new();

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后活动时间
        /// </summary>
        private long _lastActivityTime = DateTime.Now.ToBinary();
        
        public DateTime LastActivityTime
        {
            get => DateTime.FromBinary(Interlocked.Read(ref _lastActivityTime));
            set => Interlocked.Exchange(ref _lastActivityTime, value.ToBinary());
        }

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        public DateTime LastHeartbeat
        {
            get => LastActivityTime;
            set => LastActivityTime = value;
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 会话状态
        /// </summary>
        public SessionStatus Status { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否已认证
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 服务器实例信息（用于管理服务器）
        /// </summary>
        public ServerInstanceInfo ServerInstance { get; set; }

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// 会话属性字典，用于存储自定义会话属性
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 心跳计数器
        /// </summary>
        public int HeartbeatCount { get; set; }

        /// <summary>
        /// 会话超时时间（分钟）
        /// </summary>
        public int TimeoutMinutes { get; set; } = 30; // 默认30分钟超时

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServerSession()
        {
            SessionID = Guid.NewGuid().ToString();
            Status = SessionStatus.Connected;
            IsConnected = true;
        }

        /// <summary>
        /// 创建会话信息
        /// </summary>
        /// <returns>会话信息对象</returns>
        public static ServerSession Create()
        {
            var now = DateTime.Now;
            var session = new ServerSession
            {
                ConnectedTime = now,
                Status = SessionStatus.Connected
            };
            session.UpdateActivity();
            return session;
        }

        /// <summary>
        /// 更新最后活动时间
        /// </summary>
        public void UpdateActivity()
        {
            Interlocked.Exchange(ref _lastActivityTime, DateTime.Now.ToBinary());
            HeartbeatCount++;
        }

        /// <summary>
        /// 检查会话是否超时
        /// </summary>
        /// <returns>是否超时</returns>
        public bool IsTimeout()
        {
            return (DateTime.Now - LastActivityTime).TotalMinutes > TimeoutMinutes;
        }

        /// <summary>
        /// 验证会话信息有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SessionID) &&
                   !string.IsNullOrEmpty(ClientIp) &&
                   ConnectedTime <= DateTime.Now.AddMinutes(5) &&
                   ConnectedTime >= DateTime.Now.AddMinutes(-5);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"SessionId: {SessionID}, Client: {ClientIp}:{ClientPort}, Status: {Status}, User: {UserName}";
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
}