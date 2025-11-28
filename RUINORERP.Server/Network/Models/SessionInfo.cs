using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.Connection;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Security;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.Server.Network.Models
{
    /// <summary>
    /// 会话信息 - 用于会话管理和状态跟踪
    /// 继承自AppSession，既是会话信息也是SuperSocket会话
    /// </summary>
    public class SessionInfo : AppSession
    {
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; } = new();

        /// <summary>
        /// 客户端系统信息
        /// </summary>
        public ClientSystemInfo ClientSystemInfo { get; set; }

        
        /// <summary>
        /// 客户端端口
        /// </summary>
        public int ClientPort { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 连接时间（兼容性属性）
        /// </summary>
        public DateTime? ConnectTime => ConnectedTime;

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
        /// 最后心跳时间（兼容性属性）
        /// </summary>
        public DateTime LastHeartbeat
        {
            get => LastActivityTime;
            set => LastActivityTime = value;
        }



        protected override async ValueTask OnSessionConnectedAsync()
        {
            try
            {
                // 设置会话初始状态
                this.ConnectedTime = DateTime.Now;
                this.Status = SessionStatus.Connected;
                this.UpdateActivity();
                
                await base.OnSessionConnectedAsync();
            }
            catch (Exception ex)
            {
                // 记录错误
                this.LastError = $"会话连接处理错误: {ex.Message}";
            }
        }
        public ConcurrentQueue<byte[]> DataQueue = new ConcurrentQueue<byte[]>();
        /// <summary>
        /// 添加加密后的数据
        /// 通常这个用于广播。一次加密码好多次使用
        /// </summary>
        /// <param name="gde"></param>
        public virtual void AddSendData(EncryptedData gde)
        {
            try
            {
                // 使用新的通讯组件，保持向后兼容
                if (gde.Head != null && gde.Head.Length > 0)
                {
                    DataQueue.Enqueue(gde.Head);
                }
                else
                {
                    Comm.CommService.ShowExceptionMsg("包头长居然为0!");
                }
                if (gde.One != null && gde.One.Length > 0)
                {
                    DataQueue.Enqueue(gde.One);
                }
                if (gde.Two != null && gde.Two.Length > 0)
                {
                    DataQueue.Enqueue(gde.Two);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送数据时出错：DataQueue AddSendData" + ex.Message);
            }
        }
 
        
        /// <summary>
        /// 统一发送数据方法
        /// 支持直接发送字节数组，并处理队列中的数据
        /// </summary>
        /// <param name="dataBytes">要发送的字节数组</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送任务</returns>
        public virtual async ValueTask SendAsync(byte[] dataBytes, CancellationToken cancellationToken = default)
        {
            try
            {
                // 检查会话是否有效和连接状态
                if (Status == SessionStatus.Disconnected || !IsConnected)
                {
                    LastError = "会话无效或已断开连接";    
                    return;
                }
                
                // 如果有数据要发送
                if (dataBytes != null && dataBytes.Length > 0)
                {
                    if(this.IsConnected)
                    {
                        // 直接发送数据
                        await ((IAppSession)this).SendAsync(dataBytes, cancellationToken);

                        // 更新发送统计
                        Interlocked.Increment(ref _sentPacketsCount);
                        Interlocked.Add(ref _totalBytesSent, dataBytes.Length);

                        // 更新会话活动时间
                        UpdateActivity();
                    }
                    else
                    {

                    }
                   
             
                }
            }
            catch (TaskCanceledException ex)
            {
                // 任务被取消，正常处理
                LastError = $"发送取消: {ex.Message}";
            }
            catch (SocketException ex)
            {
                // 网络错误，标记会话为断开
                LastError = $"网络错误: {ex.Message}";
                Status = SessionStatus.Disconnected;
                IsConnected = false;
            }
            catch (Exception ex)
            {
                // 其他错误
                LastError = $"发送数据时发生异常: {ex.Message}";
                try
                {
                    // 尝试记录日志
                    Console.WriteLine($"Session {SessionID} 发送数据失败: {ex.Message}");
                }
                catch
                {
                    // 忽略日志记录错误
                }
            }
        }
        
        /// <summary>
        /// 记录接收数据统计
        /// </summary>
        /// <param name="dataLength">接收的数据长度</param>
        public void RecordReceivedData(int dataLength)
        {
            Interlocked.Increment(ref _receivedPacketsCount);
            Interlocked.Add(ref _totalBytesReceived, dataLength);
            UpdateActivity();
        }
        
        /// <summary>
        /// 处理队列中的数据并发送
        /// 用于批量处理和发送队列中的数据
        /// </summary>
        /// <param name="maxItemsToProcess">单次处理的最大项目数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理任务</returns>
        public virtual async ValueTask ProcessQueueAsync(int maxItemsToProcess = 100, CancellationToken cancellationToken = default)
        {
            if (!IsConnected || Status == SessionStatus.Disconnected)
                return;
            
            int processedCount = 0;
            try
            {
                // 处理队列中的数据，限制单次处理数量
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
                try
                {
                    Console.WriteLine($"Session {SessionID} 处理队列失败: {ex.Message}");
                }
                catch
                {
                    // 忽略日志记录错误
                }
            }
        }
        
        /// <summary>
        /// 获取会话性能统计信息
        /// </summary>
        /// <returns>会话性能统计对象</returns>
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
                ClientVersion = UserInfo.客户端版本,
                DeviceInfo = DeviceInfo
            };
        }
        
        protected override async ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            //Console.WriteLine($@"{DateTime.Now} {SessionName} Session {RemoteEndPoint} closed: {e.Reason}.");
            //if (User != null)
            //{
            //    if (User.在线状态)
            //    {
            //        // Tools.ShowMsg("非正常退出" + player.GameId);
            //        // SephirothServer.CommandServer.RoleService.角色退出(this);
            //    }
            //    else
            //    {
            //        //Tools.ShowMsg("正常退出" + player.GameId);
            //        // SephirothServer.CommandServer.RoleService.角色退出(this);
            //    }
            //}


            await Task.Delay(0);
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
        /// 会话超时时间（分钟）
        /// </summary>
        public int TimeoutMinutes { get; set; } = 30; // 默认30分钟超时

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

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError { get; set; }


        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 会话属性字典，用于存储自定义会话属性
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsConnected { get; internal set; }
        public bool IsAdmin { get; internal set; }

        /// <summary>
        /// 创建会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientPort">客户端端口</param>
        /// <returns>会话信息对象</returns>
        public static SessionInfo Create()
        {
            var now = DateTime.Now;
            var sessionInfo = new SessionInfo
            {
                ConnectedTime = now,
                Status = SessionStatus.Connected
            };
            sessionInfo.UpdateActivity();
            return sessionInfo;
        }

        /// <summary>
        /// 更新最后活动时间
        /// </summary>
        public void UpdateActivity()
        {
            Interlocked.Exchange(ref _lastActivityTime, DateTime.Now.ToBinary());
            HeartbeatCount++;
            //UpdateTimestamp();
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
        /// 检查心跳是否正常
        /// </summary>
        /// <returns>心跳是否正常</returns>
        public bool IsHeartbeatNormal()
        {
            return HeartbeatCount > HeartbeatCheckCount;
        }

        /// <summary>
        /// 验证会话信息有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return
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
        /// 创建会话统计信息实例
        /// </summary>
        /// <param name="maxSessions">最大会话数</param>
        /// <returns>会话统计信息</returns>
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
        public DateTime ServerStartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 服务器运行时间
        /// </summary>
        public TimeSpan Uptime => DateTime.Now - ServerStartTime;

        /// <summary>
        /// 更新统计信息
        /// </summary>
        /// <param name="connections">当前连接数</param>
        /// <param name="bytesSent">发送字节数</param>
        /// <param name="bytesReceived">接收字节数</param>
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
        /// <returns>利用率百分比</returns>
        public double GetConnectionUtilization()
        {
            return MaxConnections > 0 ? (double)CurrentConnections / MaxConnections * 100 : 0;
        }

        /// <summary>
        /// 获取平均吞吐量（字节/秒）
        /// </summary>
        /// <returns>平均吞吐量</returns>
        public double GetAverageThroughput()
        {
            var totalSeconds = Uptime.TotalSeconds;
            return totalSeconds > 0 ? (TotalBytesSent + TotalBytesReceived) / totalSeconds : 0;
        }
        
        /// <summary>
        /// 获取每秒处理请求数
        /// </summary>
        /// <returns>每秒请求数</returns>
        public double GetRequestsPerSecond()
        {
            var totalSeconds = Uptime.TotalSeconds;
            var totalRequests = TotalBytesSent + TotalBytesReceived; // 简化计算，实际应使用消息计数
            return totalSeconds > 0 ? totalRequests / totalSeconds : 0;
        }
        
        /// <summary>
        /// 获取系统健康状态
        /// </summary>
        /// <returns>健康状态描述</returns>
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
    /// 用于服务器监控UI显示
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
        /// 获取格式化的连接时长
        /// </summary>
        public string FormattedDuration => $"{ConnectedDuration.Days}天 {ConnectedDuration.Hours}小时 {ConnectedDuration.Minutes}分钟";
        
        /// <summary>
        /// 获取总流量（格式化）
        /// </summary>
        public string FormattedTotalTraffic => $"{FormatBytes(TotalBytesSent + TotalBytesReceived)}";
        
        /// <summary>
        /// 格式化字节数为可读字符串
        /// </summary>
        private string FormatBytes(long bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double doubleBytes = bytes;

            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                doubleBytes = bytes / 1024.0;
            }

            return $"{doubleBytes:F2} {suffix[i]}";
        }
    }
}