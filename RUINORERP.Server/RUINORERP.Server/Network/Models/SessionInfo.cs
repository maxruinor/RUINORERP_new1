using RUINORERP.PacketSpec.Protocol;
using SuperSocket.Connection;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Models
{
    /// <summary>
    /// 会话信息 - 用于会话管理和状态跟踪
    /// </summary>
    public class SessionInfo : AppSession
    {
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
        public DateTime ConnectedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 连接时间（兼容性属性）
        /// </summary>
        public DateTime? ConnectTime => ConnectedTime;

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; } = DateTime.Now;

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
                // 创建新会话
                var sessionInfo = new SessionInfo
                {
                   // SessionID = this.SessionID,
                   // RemoteEndPoint = this.RemoteEndPoint?.ToString(),
                    ConnectedTime = DateTime.Now,
                    Status = SessionStatus.Connected
                };
                sessionInfo.UpdateActivity();

                // 保存到会话管理器
                // await SessionService.AddSessionAsync(sessionInfo);

                await base.OnSessionConnectedAsync();

                ////通知客户端一条消息 - 使用增强的通讯组件
                //OriginalData WelcomeMsg = new OriginalData
                //{
                //    Cmd = (byte)ServerCommand.首次连接欢迎消息,
                //    One = null
                //};

                //var tx = new ByteBufferer(100);
                //tx.PushString("欢迎连接到贝思特服务器，请登录。");
                //WelcomeMsg.Two = tx.toByte();

                //// 使用增强的通讯适配器
                //await LegacyCommAdapter.SendDataToSession(this, WelcomeMsg);


                //string msg;
                //byte[] head;
                //// 发送 256个固定值
                //Tool4DataProcess.StrToHex("310631B5316D315B314231D33170319031D43189313931A231AA314A315731A5316031FB31BD31AF3188318A3126313B31253177317C318531DA316031C631AD31D031F531AE31F0310531173120311331B531D731DD3109313331583030316B31BB317F31F331143120314631B4312D31E331D2318831F1315231BE31F131AD315F31D231F7310C3183311931E4314931BC311831EA31053120318B3129311D31663143313B3114312931E8317631F1315231D4315331F431AD31DF318731E0319131F2310431903116318931FF3196312A314931BB319831C731103126319B310731C8310B31A83165314C31D931DD31CC31903185314F31A6313931A1312E", 0, -1, out head, out msg);
                //await (this as IAppSession).SendAsync(head);

                ////发送欢迎消息：在第一次连接时，服务器可能会发送一条欢迎消息或登录提示给客户端，例如：“欢迎连接到服务器，请登录。”
                ////Console.WriteLine($@"{DateTime.Now} {SessionName} New Session connected: {RemoteEndPoint}.");

                ////发送消息给客户端
                //// var msg = $@"Welcome to {SessionName}: {RemoteEndPoint}";
                //// await (this as IAppSession).SendAsync(Encoding.UTF8.GetBytes(msg + "\r\n"));
            }
            catch (Exception exx)
            {

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

        public virtual void AddSendData(OriginalData d)
        {
            try
            {
                // 使用增强的通讯适配器
                //EncryptedData gde = PacketSpec.Security.UnifiedCryptographyService.EncryptData(d);
                //AddSendData(gde);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送数据时出错：AddSendData" + ex.Message);
            }
        }
        public ValueTask SendAsync(byte[] data)
        {
            return (this as IAppSession).SendAsync(data);
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
        public string Username { get; set; }

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
        public long SentPacketsCount { get; set; }

        /// <summary>
        /// 发送数据包计数（兼容性属性）
        /// </summary>
        public long TotalSent => SentPacketsCount;

        /// <summary>
        /// 接收数据包计数
        /// </summary>
        public long ReceivedPacketsCount { get; set; }

        /// <summary>
        /// 接收数据包计数（兼容性属性）
        /// </summary>
        public long TotalReceived => ReceivedPacketsCount;

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
        /// 会话属性字典，用于存储自定义会话属性
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsConnected { get; internal set; }

        /// <summary>
        /// 创建会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientPort">客户端端口</param>
        /// <returns>会话信息对象</returns>
        public static SessionInfo Create(string sessionId, string clientIp, int clientPort = 0)
        {
            var now = DateTime.Now;
            var sessionInfo = new SessionInfo
            {
                ClientIp = clientIp,
                ClientPort = clientPort,
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
            LastActivityTime = DateTime.Now;
            HeartbeatCount++;
            //UpdateTimestamp();
        }

        /// <summary>
        /// 检查会话是否超时
        /// </summary>
        /// <returns>是否超时</returns>
        public bool IsTimeout()
        {
            return (DateTime.UtcNow - LastActivityTime).TotalMinutes > TimeoutMinutes;
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
                   ConnectedTime <= DateTime.UtcNow.AddMinutes(5) &&
                   ConnectedTime >= DateTime.UtcNow.AddMinutes(-5);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"SessionId: {SessionID}, Client: {ClientIp}:{ClientPort}, Status: {Status}, User: {Username}";
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
        public TimeSpan Uptime => DateTime.UtcNow - ServerStartTime;


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
    }
}