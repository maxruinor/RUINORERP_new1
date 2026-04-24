using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Security;
using SuperSocket.Connection;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Network.Models
{
    /// <summary>
    /// ✅ 会话信息实体 - 重构版
    /// 继承自AppSession，既是会话信息也是SuperSocket会话
    /// 
    /// 【重构要点】
    /// 1. 消除冗余字段 - UserId/UserName/ClientIp等统一从UserInfo获取
    /// 2. 简化嵌套层级 - 直接引用UserInfo，避免重复定义
    /// 3. 统一命名规范 - 核心属性使用标准命名
    /// 4. 心跳数据集中管理
    /// </summary>
    public class SessionInfo : AppSession
    {
        // ⚠️ SuperSocket AppSession通过反射创建，无法使用依赖注入
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SessionInfo>();
        
        #region 连接信息 (Client Info)

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 客户端端口
        /// </summary>
        public int ClientPort { get; set; }

        /// <summary>
        /// 设备信息（计算机名等）
        /// </summary>
        public string DeviceInfo { get; set; }

        #endregion

        #region 用户信息 (User Info) - 唯一数据源

        /// <summary>
        /// 用户信息 - 唯一数据源
        /// 包含所有用户相关信息，避免在SessionInfo中重复定义
        /// </summary>
        public CurrentUserInfo UserInfo { get; set; } = new();

        /// <summary>
        /// 用户ID（用户名表主键）- 便捷访问属性
        /// 数据来源: UserInfo.UserID
        /// </summary>
        public long? UserId => UserInfo?.UserID > 0 ? UserInfo.UserID : null;

        /// <summary>
        /// 用户名 - 便捷访问属性
        /// 数据来源: UserInfo.UserName
        /// </summary>
        public string UserName => UserInfo?.UserName;

        /// <summary>
        /// 员工ID - 便捷访问属性
        /// 数据来源: UserInfo.EmployeeId
        /// </summary>
        public long EmpId => UserInfo?.EmployeeId ?? 0;

        #endregion

        #region 客户端系统信息 (Client System Info)

        /// <summary>
        /// 客户端系统信息 - 包含客户端完整的系统、硬件、环境信息
        /// </summary>
        public ClientSystemInfo ClientSystemInfo { get; set; }

        #endregion

        #region 连接状态 (Connection State)

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 连接时间（兼容性属性）
        /// </summary>
        public DateTime? ConnectTime => ConnectedTime;

        /// <summary>
        /// 最后活动时间（核心时间戳）
        /// </summary>
        private long _lastActivityTime = DateTime.Now.ToBinary();
        
        public DateTime LastActivityTime
        {
            get => DateTime.FromBinary(Interlocked.Read(ref _lastActivityTime));
            set => Interlocked.Exchange(ref _lastActivityTime, value.ToBinary());
        }

        /// <summary>
        /// 最后心跳时间 - 便捷访问属性
        /// </summary>
        public DateTime LastHeartbeat
        {
            get => LastActivityTime;
            set => LastActivityTime = value;
        }

        /// <summary>
        /// 断开连接时间
        /// </summary>
        public DateTime? DisconnectTime { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 会话是否已连接
        /// </summary>
        public bool IsConnected { get; internal set; }

        /// <summary>
        /// 会话状态
        /// </summary>
        public SessionStatus Status { get; set; }

        #endregion

        #region 认证状态 (Authentication State)

        /// <summary>
        /// 是否已验证（连接握手验证）
        /// 客户端收到欢迎消息并回复确认后才为true
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// 是否已认证（登录验证）
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public bool IsAdmin { get; internal set; }

        /// <summary>
        /// 欢迎消息发送时间
        /// </summary>
        public DateTime? WelcomeSentTime { get; set; }

        /// <summary>
        /// 是否收到欢迎确认消息
        /// </summary>
        public bool WelcomeAckReceived { get; set; } = false;

        #endregion

        #region 心跳管理 (Heartbeat Management) - 集中管理

        /// <summary>
        /// 心跳计数器
        /// </summary>
        public int HeartbeatCount { get; set; }

        /// <summary>
        /// 心跳失败计数器
        /// </summary>
        public int HeartbeatFailedCount { get; set; }

        /// <summary>
        /// 最后心跳序号（用于检测心跳丢失）
        /// </summary>
        public long LastHeartbeatSequence { get; set; }

        /// <summary>
        /// 会话超时时间（分钟）
        /// </summary>
        public int TimeoutMinutes { get; set; } = 30;

        /// <summary>
        /// 记录一次心跳成功
        /// </summary>
        public void RecordHeartbeat()
        {
            HeartbeatCount++;
            HeartbeatFailedCount = 0;
            LastHeartbeatSequence++;
            UpdateActivity();
        }

        /// <summary>
        /// 记录一次心跳失败
        /// </summary>
        public void RecordHeartbeatFailure()
        {
            HeartbeatFailedCount++;
        }

        #endregion

        #region 性能统计 (Performance Stats)

        /// <summary>
        /// 发送数据包计数
        /// </summary>
        private long _sentPacketsCount = 0;
        public long SentPacketsCount 
        {
            get => _sentPacketsCount;
            set => Interlocked.Exchange(ref _sentPacketsCount, value);
        }

        /// <summary>
        /// 发送数据包计数（兼容性属性）
        /// </summary>
        public long TotalSent => SentPacketsCount;

        /// <summary>
        /// 接收数据包计数
        /// </summary>
        private long _receivedPacketsCount = 0;
        public long ReceivedPacketsCount 
        {
            get => _receivedPacketsCount;
            set => Interlocked.Exchange(ref _receivedPacketsCount, value);
        }

        /// <summary>
        /// 接收数据包计数（兼容性属性）
        /// </summary>
        public long TotalReceived => ReceivedPacketsCount;
        
        /// <summary>
        /// 发送字节总数
        /// </summary>
        private long _totalBytesSent = 0;
        public long TotalBytesSent
        {
            get => _totalBytesSent;
            set => Interlocked.Exchange(ref _totalBytesSent, value);
        }
        
        /// <summary>
        /// 接收字节总数
        /// </summary>
        private long _totalBytesReceived = 0;
        public long TotalBytesReceived
        {
            get => _totalBytesReceived;
            set => Interlocked.Exchange(ref _totalBytesReceived, value);
        }

        #endregion

        #region 错误和属性

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// 会话属性字典，用于存储自定义会话属性
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        #endregion

        #region 数据队列

        /// <summary>
        /// DataQueue最大容量限制
        /// </summary>
        private const int MaxDataQueueSize = 100;
        
        /// <summary>
        /// 队列清理告警阈值 - 超过此清理次数时触发告警
        /// </summary>
        private const int DataQueueCleanupAlertThreshold = 50;
        
        /// <summary>
        /// 数据队列（用于广播等场景）
        /// </summary>
        public ConcurrentQueue<byte[]> DataQueue = new ConcurrentQueue<byte[]>();
        
        /// <summary>
        /// DataQueue被清理的次数
        /// </summary>
        public int DataQueueCleanupCount { get; private set; }

        /// <summary>
        /// 是否触发过队列清理告警
        /// </summary>
        public bool HasTriggeredQueueCleanupAlert { get; private set; }

        /// <summary>
        /// 队列清理告警事件 - 超过阈值时触发
        /// </summary>
        public event EventHandler<DataQueueCleanupEventArgs> DataQueueCleanupAlert;

        #endregion

        #region SuperSocket 生命周期

        protected override async ValueTask OnSessionConnectedAsync()
        {
            try
            {
                this.ConnectedTime = DateTime.Now;
                this.Status = SessionStatus.Connected;
                this.UpdateActivity();
                await base.OnSessionConnectedAsync();
            }
            catch (Exception ex)
            {
                this.LastError = $"会话连接处理错误: {ex.Message}";
            }
        }
        
        protected override async ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            await Task.Delay(0);
        }

        #endregion

        #region 数据发送

        /// <summary>
        /// 添加加密后的数据到发送队列
        /// </summary>
        public virtual void AddSendData(EncryptedData gde)
        {
            try
            {
                // 限制队列大小
                int cleanupCount = 0;
                while (DataQueue.Count >= MaxDataQueueSize)
                {
                    DataQueue.TryDequeue(out _);
                    cleanupCount++;
                }
                
                if (cleanupCount > 0)
                {
                    DataQueueCleanupCount += cleanupCount;
                    
                    // 超过阈值时触发告警事件
                    if (!HasTriggeredQueueCleanupAlert && DataQueueCleanupCount >= DataQueueCleanupAlertThreshold)
                    {
                        HasTriggeredQueueCleanupAlert = true;
                        DataQueueCleanupAlert?.Invoke(this, new DataQueueCleanupEventArgs(SessionID, DataQueueCleanupCount, cleanupCount));
                    }
                    
                    _logger?.LogWarning("[SessionInfo] Session {SessionId}: 清理了 {CleanupCount} 个积压数据包, 累计清理: {TotalCleanup}", 
                        SessionID, cleanupCount, DataQueueCleanupCount);
                }
                
                if (gde.Head != null && gde.Head.Length > 0)
                    DataQueue.Enqueue(gde.Head);
                if (gde.One != null && gde.One.Length > 0)
                    DataQueue.Enqueue(gde.One);
                if (gde.Two != null && gde.Two.Length > 0)
                    DataQueue.Enqueue(gde.Two);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"发送数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public virtual async ValueTask SendAsync(byte[] dataBytes, CancellationToken cancellationToken = default)
        {
            try
            {
                if (Status == SessionStatus.Disconnected || !IsConnected)
                {
                    LastError = "会话无效或已断开连接";    
                    return;
                }
                
                if (dataBytes != null && dataBytes.Length > 0 && IsConnected)
                {
                    await ((IAppSession)this).SendAsync(dataBytes, cancellationToken);
                    Interlocked.Increment(ref _sentPacketsCount);
                    Interlocked.Add(ref _totalBytesSent, dataBytes.Length);
                    UpdateActivity();
                }
            }
            catch (TaskCanceledException ex)
            {
                LastError = $"发送取消: {ex.Message}";
            }
            catch (SocketException ex)
            {
                LastError = $"网络错误: {ex.Message}";
                Status = SessionStatus.Disconnected;
                IsConnected = false;
            }
            catch (Exception ex)
            {
                LastError = $"发送数据时发生异常: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Session {SessionID} 发送数据失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 记录接收数据统计
        /// </summary>
        public void RecordReceivedData(int dataLength)
        {
            Interlocked.Increment(ref _receivedPacketsCount);
            Interlocked.Add(ref _totalBytesReceived, dataLength);
            UpdateActivity();
        }
        
        /// <summary>
        /// 处理队列中的数据并发送
        /// </summary>
        public virtual async ValueTask ProcessQueueAsync(int maxItemsToProcess = 100, CancellationToken cancellationToken = default)
        {
            if (!IsConnected || Status == SessionStatus.Disconnected)
                return;
            
            int processedCount = 0;
            try
            {
                while (processedCount < maxItemsToProcess && DataQueue.TryDequeue(out byte[] dataToSend))
                {
                    if (dataToSend != null && dataToSend.Length > 0)
                    {
                        await SendAsync(dataToSend, cancellationToken);
                        processedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = $"处理数据队列时发生异常: {ex.Message}";
            }
        }

        #endregion

        #region 活动管理

        /// <summary>
        /// 更新最后活动时间
        /// </summary>
        public void UpdateActivity()
        {
            Interlocked.Exchange(ref _lastActivityTime, DateTime.Now.ToBinary());
        }

        /// <summary>
        /// 检查会话是否超时
        /// </summary>
        public bool IsTimeout()
        {
            return (DateTime.Now - LastActivityTime).TotalMinutes > TimeoutMinutes;
        }

        /// <summary>
        /// 验证会话信息有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ClientIp) &&
                   ConnectedTime <= DateTime.Now.AddMinutes(5) &&
                   ConnectedTime >= DateTime.Now.AddMinutes(-5);
        }

        #endregion

        #region 性能统计

        /// <summary>
        /// 获取会话性能统计信息
        /// </summary>
        public SessionPerformanceStats GetPerformanceStats()
        {
            return new SessionPerformanceStats
            {
                SessionId = SessionID,
                UserName = UserName,
                Status = Status,
                SentPackets = SentPacketsCount,
                ReceivedPackets = ReceivedPacketsCount,
                TotalBytesSent = TotalBytesSent,
                TotalBytesReceived = TotalBytesReceived,
                ConnectedDuration = DateTime.Now - ConnectedTime,
                LastActivity = LastActivityTime,
                IsConnected = IsConnected,
                ClientInfo = $"{ClientIp}:{ClientPort}",
                ClientVersion = UserInfo?.ClientVersion,
                DeviceInfo = DeviceInfo
            };
        }

        #endregion

        #region 数据清理

        /// <summary>
        /// 清理会话数据，释放内存（解决内存泄漏问题）
        /// </summary>
        public void Clear()
        {
            // 清除用户信息
            UserInfo = null;
            
            // 清除客户端系统信息
            ClientSystemInfo = null;
            
            // 清除数据队列
            ClearDataQueue();
            
            // 清除属性字典
            if (Properties != null)
            {
                Properties.Clear();
                Properties = null;
            }
            
            // 清除错误信息
            LastError = null;
            
            // 重置统计计数器
            _sentPacketsCount = 0;
            _receivedPacketsCount = 0;
            _totalBytesSent = 0;
            _totalBytesReceived = 0;
            
            // 重置心跳相关
            HeartbeatCount = 0;
            HeartbeatFailedCount = 0;
            LastHeartbeatSequence = 0;
            
            // 标记为已断开
            IsConnected = false;
            Status = SessionStatus.Disconnected;
        }

        /// <summary>
        /// 清理数据队列
        /// </summary>
        public void ClearDataQueue()
        {
            if (DataQueue != null)
            {
                // 将队列中的所有元素出队以释放引用
                while (DataQueue.TryDequeue(out _)) { }
            }
        }

        #endregion

        #region 工厂方法

        /// <summary>
        /// 创建会话信息
        /// </summary>
        public static SessionInfo Create()
        {
            var now = DateTime.Now;
            var sessionInfo = new SessionInfo
            {
                ConnectedTime = now,
                Status = SessionStatus.Connected,
                UserInfo = new CurrentUserInfo()
            };
            sessionInfo.UpdateActivity();
            return sessionInfo;
        }

        #endregion

        #region ToString

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"SessionId: {SessionID}, Client: {ClientIp}:{ClientPort}, Status: {Status}, User: {UserName}";
        }

        #endregion
    }

    #region 会话状态枚举

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

    #endregion

    #region 会话统计信息

    /// <summary>
    /// 会话统计信息
    /// </summary>
    public class SessionStatistics
    {
        /// <summary>
        /// 总连接数
        /// </summary>
        public int TotalConnections { get; set; }
        /// <summary>
        /// 总断开连接数
        /// </summary>
        public int TotalDisconnections { get; set; }
        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections { get; set; }
        /// <summary>
        /// 峰值连接数
        /// </summary>
        public int PeakConnections { get; set; }
        /// <summary>
        /// 超时会话数
        /// </summary>
        public int TimeoutSessions { get; set; }
        /// <summary>
        /// 心跳失败次数
        /// </summary>
        public int HeartbeatFailures { get; set; }
        /// <summary>
        /// 最后清理时间
        /// </summary>
        public DateTime LastCleanupTime { get; set; }
        /// <summary>
        /// 最后心跳检查时间
        /// </summary>
        public DateTime LastHeartbeatCheck { get; set; }
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
        /// 总请求数
        /// </summary>
        public long TotalRequests { get; set; }
        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public DateTime ServerStartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 服务器运行时间
        /// </summary>
        public TimeSpan Uptime => DateTime.Now - ServerStartTime;

        /// <summary>
        /// 创建会话统计信息实例
        /// </summary>
        public static SessionStatistics Create(int maxSessions = 1000)
        {
            return new SessionStatistics
            {
                MaxConnections = maxSessions,
                TotalConnections = 0,
                TotalDisconnections = 0,
                CurrentConnections = 0,
                PeakConnections = 0,
                TimeoutSessions = 0,
                HeartbeatFailures = 0,
                LastCleanupTime = DateTime.Now,
                LastHeartbeatCheck = DateTime.Now
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
                MaxConnections = connections;
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

        /// <summary>
        /// 获取每秒请求数
        /// </summary>
        public double GetRequestsPerSecond()
        {
            var totalSeconds = Uptime.TotalSeconds;
            return totalSeconds > 0 ? (double)TotalRequests / totalSeconds : 0;
        }

        /// <summary>
        /// 获取系统健康状态
        /// </summary>
        public string GetHealthStatus()
        {
            if (CurrentConnections >= MaxConnections * 0.9)
                return "警告：连接数接近上限";
            if (TimeoutSessions > CurrentConnections * 0.1)
                return "警告：超时会话比例较高";
            return "正常";
        }
    }
    
    /// <summary>
    /// 会话性能统计信息
    /// </summary>
    public class SessionPerformanceStats
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 会话状态
        /// </summary>
        public SessionStatus Status { get; set; }
        /// <summary>
        /// 发送数据包数
        /// </summary>
        public long SentPackets { get; set; }
        /// <summary>
        /// 接收数据包数
        /// </summary>
        public long ReceivedPackets { get; set; }
        /// <summary>
        /// 发送总字节数
        /// </summary>
        public long TotalBytesSent { get; set; }
        /// <summary>
        /// 接收总字节数
        /// </summary>
        public long TotalBytesReceived { get; set; }
        /// <summary>
        /// 连接时长
        /// </summary>
        public TimeSpan ConnectedDuration { get; set; }
        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivity { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get; set; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public string ClientInfo { get; set; }
        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }
        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }
        
        /// <summary>
        /// 格式化的连接时长
        /// </summary>
        public string FormattedDuration => $"{ConnectedDuration.Days}天 {ConnectedDuration.Hours}小时 {ConnectedDuration.Minutes}分钟";
        
        /// <summary>
        /// 总流量（格式化）
        /// </summary>
        public string FormattedTotalTraffic => $"{FormatBytes(TotalBytesSent + TotalBytesReceived)}";
        
        private string FormatBytes(long bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double doubleBytes = bytes;
            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
                doubleBytes = bytes / 1024.0;
            return $"{doubleBytes:F2} {suffix[i]}";
        }
    }

    #endregion

    #region DDoS防护

    /// <summary>
    /// 连接频率跟踪器
    /// </summary>
    public class ConnectionRateTracker
    {
        public int ConnectionCount { get; set; }
        public DateTime FirstConnection { get; set; }
        public DateTime LastConnection { get; set; }

        public void RecordConnection()
        {
            ConnectionCount++;
            LastConnection = DateTime.Now;
            if (FirstConnection == default)
                FirstConnection = DateTime.Now;
        }

        public bool IsExceedingLimit(int maxConnections, TimeSpan timeWindow)
        {
            var windowStart = DateTime.Now - timeWindow;
            if (LastConnection < windowStart)
            {
                ConnectionCount = 0;
                FirstConnection = DateTime.Now;
                return false;
            }
            return ConnectionCount > maxConnections;
        }
    }

    #endregion

    /// <summary>
    /// 数据队列清理告警事件参数
    /// </summary>
    public class DataQueueCleanupEventArgs : EventArgs
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// 累计清理次数
        /// </summary>
        public int TotalCleanupCount { get; }

        /// <summary>
        /// 本次清理次数
        /// </summary>
        public int ThisCleanupCount { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="totalCleanupCount">累计清理次数</param>
        /// <param name="thisCleanupCount">本次清理次数</param>
        public DataQueueCleanupEventArgs(string sessionId, int totalCleanupCount, int thisCleanupCount)
        {
            SessionId = sessionId;
            TotalCleanupCount = totalCleanupCount;
            ThisCleanupCount = thisCleanupCount;
        }
    }

   
}
