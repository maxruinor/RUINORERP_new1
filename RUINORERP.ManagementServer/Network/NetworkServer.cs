using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace RUINORERP.ManagementServer.Network
{
    /// <summary>
    /// 网络服务器核心类
    /// 基于SuperSocket框架实现客户端-服务器通信
    /// </summary>
    public class NetworkServer
    {
        private WebSocketServer<ServerSession> _server;
        private int _port;
        private string _ipAddress;

        /// <summary>
        /// 服务器状态变化事件
        /// </summary>
        public event EventHandler<ServerStatusChangedEventArgs> ServerStatusChanged;

        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;

        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        /// <summary>
        /// 收到消息事件
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning => _server?.State == ServerState.Running;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NetworkServer()
        {
            // 从配置文件读取默认端口和IP地址
            _port = int.Parse(ConfigurationManager.AppSettings["ServerPort"] ?? "8080");
            _ipAddress = ConfigurationManager.AppSettings["ServerIP"] ?? "0.0.0.0";
        }

        /// <summary>
        /// 初始化服务器
        /// </summary>
        public void Initialize()
        {
            var hostBuilder = WebSocketHostBuilder.Create()
                .UseWebSocketMessageHandler<ServerSession>((session, message) => HandleMessage(session, message))
                .ConfigureSuperSocket(options =>
                {
                    options.Name = "RUINORERP.ManagementServer";
                    options.Listeners = new List<ListenOptions>
                    {
                        new ListenOptions
                        {
                            Ip = _ipAddress,
                            Port = _port
                        }
                    };
                })
                .ConfigureServices((hostCtx, services) =>
                {
                    // 注册命令处理程序
                    services.AddCommandHandlers();
                });

            _server = hostBuilder.Build();

            // 注册服务器事件
            _server.Started += OnServerStarted;
            _server.Stopped += OnServerStopped;
            _server.NewSessionConnected += OnNewSessionConnected;
            _server.SessionClosed += OnSessionClosed;
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task StartAsync()
        {
            if (_server == null)
            {
                Initialize();
            }

            if (_server.State != ServerState.Running)
            {
                await _server.StartAsync();
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public async Task StopAsync()
        {
            if (_server != null && _server.State == ServerState.Running)
            {
                await _server.StopAsync();
            }
        }

        /// <summary>
        /// 重启服务器
        /// </summary>
        public async Task RestartAsync()
        {
            await StopAsync();
            await StartAsync();
        }

        /// <summary>
        /// 处理收到的消息
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="message">收到的消息</param>
        private async Task HandleMessage(ServerSession session, string message)
        {
            try
            {
                // 解析消息
                var packet = JsonCompressionSerializationService.Deserialize<PacketModel>(message);
                if (packet == null)
                {
                    return;
                }

                // 触发消息接收事件（由AppManager处理）
                MessageReceived?.Invoke(this, new MessageReceivedEventArgs(session, packet));

                // 发送响应（AppManager将处理具体的命令逻辑）
                var response = ResponseFactory.CreateSuccessResponse(packet.ExecutionContext);
                await SendResponse(session, response);
            }
            catch (Exception ex)
            {
                // 记录异常日志
                Console.WriteLine($"处理消息时发生异常: {ex.Message}");
                // 发送错误响应
                var packet = JsonCompressionSerializationService.Deserialize<PacketModel>(message);
                if (packet != null)
                {
                    var response = ResponseFactory.CreateErrorResponse(packet.ExecutionContext, ex.Message);
                    await SendResponse(session, response);
                }
            }
        }

        /// <summary>
        /// 发送响应
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="response">响应数据</param>
        private async Task SendResponse(ServerSession session, IResponse response)
        {
            // 创建响应数据包
            var responsePacket = new PacketModel
            {
                PacketId = IdGenerator.GenerateResponseId(Guid.NewGuid().ToString()),
                Direction = PacketDirection.ServerResponse,
                SessionId = session.SessionID,
                Status = PacketStatus.Completed,
                Response = response
            };

            // 序列化响应数据包
            var message = JsonCompressionSerializationService.Serialize(responsePacket);

            // 发送响应
            await session.SendAsync(message);
        }

        /// <summary>
        /// 服务器启动事件处理
        /// </summary>
        private void OnServerStarted(object sender, EventArgs e)
        {
            // 触发服务器状态变化事件
            ServerStatusChanged?.Invoke(this, new ServerStatusChangedEventArgs(ServerStatus.Running));
        }

        /// <summary>
        /// 服务器停止事件处理
        /// </summary>
        private void OnServerStopped(object sender, EventArgs e)
        {
            // 触发服务器状态变化事件
            ServerStatusChanged?.Invoke(this, new ServerStatusChangedEventArgs(ServerStatus.Stopped));
        }

        /// <summary>
        /// 新会话连接事件处理
        /// </summary>
        private void OnNewSessionConnected(object sender, SessionEventArgs<ServerSession> e)
        {
            // 触发客户端连接事件
            ClientConnected?.Invoke(this, new ClientConnectedEventArgs(e.Session));
        }

        /// <summary>
        /// 会话关闭事件处理
        /// </summary>
        private void OnSessionClosed(object sender, SessionEventArgs<ServerSession> e)
        {
            // 触发客户端断开连接事件
            ClientDisconnected?.Invoke(this, new ClientDisconnectedEventArgs(e.Session));
        }
    }

    /// <summary>
    /// 服务器会话类
    /// </summary>
    public class ServerSession : WebSocketSession
    {
        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; private set; }

        /// <summary>
        /// 服务器实例信息
        /// </summary>
        public ServerInstanceInfo ServerInstance { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public new string SessionID => base.SessionID;

        /// <summary>
        /// 初始化会话
        /// </summary>
        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
            LastActivityTime = DateTime.Now;
        }

        /// <summary>
        /// 更新最后活动时间
        /// </summary>
        public void UpdateLastActivityTime()
        {
            LastActivityTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 服务器状态枚举
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// 停止状态
        /// </summary>
        Stopped = 0,
        /// <summary>
        /// 运行状态
        /// </summary>
        Running = 1,
        /// <summary>
        /// 错误状态
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// 服务器状态变化事件参数
    /// </summary>
    public class ServerStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 新的服务器状态
        /// </summary>
        public ServerStatus NewStatus { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="newStatus">新的服务器状态</param>
        public ServerStatusChangedEventArgs(ServerStatus newStatus)
        {
            NewStatus = newStatus;
        }
    }

    /// <summary>
    /// 客户端连接事件参数
    /// </summary>
    public class ClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 客户端会话
        /// </summary>
        public ServerSession Session { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="session">客户端会话</param>
        public ClientConnectedEventArgs(ServerSession session)
        {
            Session = session;
        }
    }

    /// <summary>
    /// 客户端断开连接事件参数
    /// </summary>
    public class ClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 客户端会话
        /// </summary>
        public ServerSession Session { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="session">客户端会话</param>
        public ClientDisconnectedEventArgs(ServerSession session)
        {
            Session = session;
        }
    }

    /// <summary>
    /// 消息接收事件参数
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 客户端会话
        /// </summary>
        public ServerSession Session { get; }

        /// <summary>
        /// 收到的数据包
        /// </summary>
        public PacketModel Packet { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">收到的数据包</param>
        public MessageReceivedEventArgs(ServerSession session, PacketModel packet)
        {
            Session = session;
            Packet = packet;
        }
    }

    /// <summary>
    /// 服务器实例信息类
    /// </summary>
    public class ServerInstanceInfo
    {
        /// <summary>
        /// 实例ID
        /// </summary>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

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
}