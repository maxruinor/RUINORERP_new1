using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 通信管理器 - 应用层统一通信入口
    /// 
    /// 设计目的：
    /// 1. 整合所有通信组件，提供统一的客户端与服务器通信入口
    /// 2. 负责连接管理、心跳检测、自动重连等基础设施功能
    /// 3. 作为应用层（UI层、控制器层）的通信管理中枢
    /// 
    /// 使用场景：
    /// - 在应用层（如 MainForm、FrmLogin 等）使用 CommunicationManager 来管理整体通信
    /// - 负责连接/断开连接、监控连接状态、处理重连等
    /// 
    /// 注意事项：
    /// - 业务层应使用 IClientCommunicationService 进行具体的通信操作
    /// - 不要在业务层直接使用 CommunicationManager 进行通信操作
    /// - CommunicationManager.IsConnected 委托给内部的 IClientCommunicationService.IsConnected
    /// 
    /// 使用示例：
    /// // 在应用层检查连接状态和执行连接操作
    /// if (!communicationManager.IsConnected)
    /// {
    ///     var connected = await communicationManager.ConnectAsync("127.0.0.1", 7538);
    /// }
    /// 
    /// // 在业务层使用通信服务发送命令
    /// var response = await communicationService.SendCommandAsync&lt;LoginRequest, LoginResult&gt;(
    ///     CommandId.Login, loginRequest);
    /// </summary>
    public class CommunicationManager : IDisposable
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly HeartbeatManager _heartbeatManager;
        private bool _isInitialized;
        private bool _autoReconnect = true;
        private int _reconnectInterval = 5000; // 重连间隔时间（毫秒）
        private int _maxReconnectAttempts = 5; // 最大重连尝试次数
        private int _currentReconnectAttempt = 0;
        private string _lastServerUrl;
        private int _lastPort;

        /// <summary>
        /// 连接状态变更事件
        /// </summary>
        public event Action<bool> ConnectionStatusChanged;

        /// <summary>
        /// 错误发生事件
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        /// <summary>
        /// 接收到服务器推送命令事件
        /// </summary>
        public event Action<CommandId, object> CommandReceived;

        /// <summary>
        /// 是否已连接到服务器
        /// </summary>
        public bool IsConnected => _communicationService?.IsConnected ?? false;

        /// <summary>
        /// 是否自动重连
        /// </summary>
        public bool AutoReconnect
        {
            get => _autoReconnect;
            set => _autoReconnect = value;
        }

        /// <summary>
        /// 重连延迟时间
        /// </summary>
        public TimeSpan ReconnectDelay
        {
            get => TimeSpan.FromMilliseconds(_reconnectInterval);
            set => _reconnectInterval = (int)value.TotalMilliseconds;
        }

        /// <summary>
        /// 最大重连尝试次数
        /// </summary>
        public int MaxReconnectAttempts
        {
            get => _maxReconnectAttempts;
            set => _maxReconnectAttempts = value;
        }

        /// <summary>
        /// 上次连接的服务器地址
        /// </summary>
        public string LastServerUrl => _lastServerUrl;

        /// <summary>
        /// 上次连接的端口号
        /// </summary>
        public int LastPort => _lastPort;

        /// <summary>
        /// 当前重连尝试次数
        /// </summary>
        public int CurrentReconnectAttempt => _currentReconnectAttempt;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">客户端通信服务</param>
        /// <param name="heartbeatManager">心跳管理器</param>
        public CommunicationManager(
            IClientCommunicationService communicationService, 
            HeartbeatManager heartbeatManager)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _heartbeatManager = heartbeatManager ?? throw new ArgumentNullException(nameof(heartbeatManager));
        }

        /// <summary>
        /// 初始化通信管理器
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            // 注册心跳管理器事件
            _heartbeatManager.ConnectionLost += OnConnectionLost;
            _heartbeatManager.HeartbeatFailed += OnHeartbeatFailed;
            
            // 注册通信服务的命令接收事件
            _communicationService.CommandReceived += OnCommandReceived;
            
            _isInitialized = true;
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken cancellationToken = default)
        {
            try
            {
                Initialize();
                
                // 保存服务器地址和端口，用于重连
                _lastServerUrl = serverUrl;
                _lastPort = port;
                
                var connected = await _communicationService.ConnectAsync(serverUrl, port, cancellationToken);
                
                if (connected)
                {
                    OnConnectionStatusChanged(true);
                    _currentReconnectAttempt = 0;
                    // 启动心跳
                    _heartbeatManager.Start();
                }
                
                return connected;
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                // 停止心跳
                _heartbeatManager.Stop();
                
                // 断开通信服务连接
                _communicationService.Disconnect();
                
                OnConnectionStatusChanged(false);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
            }
        }

        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>带包装的API响应</returns>
        public async Task<ApiResponse<TResponse>> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId, 
            TRequest requestData, 
            CancellationToken cancellationToken = default, 
            int timeoutMs = 30000)
        {
            try
            {
                // 检查连接状态
                if (!IsConnected)
                {
                    // 如果配置了自动重连且未达到最大尝试次数，尝试重连
                    if (_autoReconnect && _currentReconnectAttempt < _maxReconnectAttempts)
                    {
                        await AttemptReconnect(cancellationToken);
                    }
                    
                    if (!IsConnected)
                    {
                        return ApiResponse<TResponse>.Failure("未连接到服务器", 500);
                    }
                }
                
                // 发送命令
                return await _communicationService.SendCommandAsync<TRequest, TResponse>(
                    commandId, 
                    requestData, 
                    cancellationToken, 
                    timeoutMs);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return ApiResponse<TResponse>.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// 发送命令并等待响应（使用命令对象）
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>带包装的API响应</returns>
        public async Task<ApiResponse<TResponse>> SendCommandAsync<TResponse>(
            ICommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _communicationService.SendCommandAsync<TResponse>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return ApiResponse<TResponse>.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// 发送单向命令（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送是否成功</returns>
        public async Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId, 
            TRequest requestData, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _communicationService.SendOneWayCommandAsync(commandId, requestData, cancellationToken);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return false;
            }
        }

        /// <summary>
        /// 尝试重新连接
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>重连是否成功</returns>
        private async Task<bool> AttemptReconnect(CancellationToken cancellationToken)
        {
            try
            {
                _currentReconnectAttempt++;
                
                // 等待重连间隔时间
                await Task.Delay(_reconnectInterval, cancellationToken);
                
                // 检查是否已保存服务器地址和端口
                if (string.IsNullOrEmpty(_lastServerUrl))
                {
                    throw new Exception("未保存服务器地址和端口，无法重连");
                }
                
                Console.WriteLine($"正在尝试重连服务器 ({_currentReconnectAttempt}/{_maxReconnectAttempts}): {_lastServerUrl}:{_lastPort}");
                
                // 执行重连
                var connected = await _communicationService.ConnectAsync(
                    _lastServerUrl, 
                    _lastPort, 
                    cancellationToken);
                
                if (connected)
                {
                    OnConnectionStatusChanged(true);
                    _heartbeatManager.Start();
                    Console.WriteLine("重连成功");
                }
                
                return connected;
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new Exception($"重连尝试 {_currentReconnectAttempt} 失败: {ex.Message}", ex));
                return false;
            }
        }

        /// <summary>
        /// 处理连接丢失事件
        /// </summary>
        private void OnConnectionLost()
        {
            OnConnectionStatusChanged(false);
            
            // 如果配置了自动重连，尝试重连
            if (_autoReconnect)
            {
                _ = Task.Run(() => AttemptReconnect(CancellationToken.None));
            }
        }

        /// <summary>
        /// 处理心跳失败事件
        /// </summary>
        /// <param name="ex">异常信息</param>
        private void OnHeartbeatFailed(Exception ex)
        {
            OnErrorOccurred(new Exception("心跳失败", ex));
        }

        /// <summary>
        /// 触发连接状态变更事件
        /// </summary>
        /// <param name="isConnected">是否已连接</param>
        private void OnConnectionStatusChanged(bool isConnected)
        {
            ConnectionStatusChanged?.Invoke(isConnected);
        }

        /// <summary>
        /// 触发错误发生事件
        /// </summary>
        /// <param name="ex">异常信息</param>
        private void OnErrorOccurred(Exception ex)
        {
            ErrorOccurred?.Invoke(ex);
            Console.WriteLine($"通信错误: {ex.Message}");
        }

        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private void OnCommandReceived(CommandId commandId, object data)
        {
            CommandReceived?.Invoke(commandId, data);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            
            // 取消注册事件
            _heartbeatManager.ConnectionLost -= OnConnectionLost;
            _heartbeatManager.HeartbeatFailed -= OnHeartbeatFailed;
            _communicationService.CommandReceived -= OnCommandReceived;
            
            // 释放通信服务
            _communicationService?.Dispose();
        }
    }
}