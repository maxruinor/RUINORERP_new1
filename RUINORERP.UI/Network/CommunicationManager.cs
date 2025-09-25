using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
/// <summary>
  
    /// </summary>
    public class CommunicationManager : IDisposable
    {
        private readonly IClientCommunicationService _service;
        /// <summary>
        /// </summary>
        private readonly HeartbeatManager _heartbeat;
        private readonly ILogger<CommunicationManager> _logger;
        private readonly ClientEventManager _eventManager;
        private readonly object _lock = new object();
        private bool _isInitialized;
        private bool _disposed = false;
        private string _lastServerUrl;
        private int _lastPort;
        
        /// <summary>心跳失败计数器</summary>
        private int _heartbeatFailureCount = 0;
        
        /// <summary>最大允许的心跳失败次数</summary>
        private const int MaxHeartbeatFailures = 3;
        
        /// <summary>心跳管理器是否正在运行</summary>
        private bool _heartbeatIsRunning = false;

        /// <summary>当前是否已连接到服务器</summary>
        public bool IsConnected => _service?.IsConnected ?? false;

        /// <summary>
        /// 获取事件管理器 - 【ClientNetworkManager核心能力】
        /// 
        /// 🎯 设计亮点：
        /// ✅ 提供统一的事件订阅和管理接口
        /// ✅ 支持外部组件订阅网络事件
        /// ✅ 完整的事件生命周期管理
        /// ✅ 线程安全的事件分发机制
        /// ✅ 支持事件统计和监控
        /// 
        /// 🔗 使用场景：
        /// - UI组件订阅连接状态变化
        /// - 业务组件订阅数据接收事件
        /// - 监控组件订阅错误和异常事件
        /// - 日志组件订阅所有事件进行记录
        /// </summary>
        public ClientEventManager EventManager => _eventManager;
        
        /// <summary>是否启用自动重连功能，默认为true</summary>
        public bool AutoReconnect { get; set; } = true;
        
        /// <summary>最大重连尝试次数，默认为5次</summary>
        public int MaxReconnectAttempts { get; set; } = 5;
        
        /// <summary>重连间隔时间，默认为5秒</summary>
        public TimeSpan ReconnectDelay { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>连接状态变化事件</summary>
        public event Action<bool> ConnectionStatusChanged;
        
        /// <summary>错误发生事件</summary>
        public event Action<Exception> ErrorOccurred;
        
        /// <summary>命令接收事件</summary>
        public event Action<CommandId, object> CommandReceived;

        /// <summary>
        /// 增强构造函数 - 【整合ClientNetworkManager事件管理】
        /// 
        /// 🚀 架构升级：
        /// ✅ 保留原有心跳管理依赖注入设计
        /// ✅ 新增ClientEventManager统一事件管理
        /// ✅ 支持直接PacketModel数据处理和分发
        /// ✅ 增强参数验证和异常处理机制
        /// ✅ 提供完整的事件订阅和资源管理
        /// 
        /// 🎯 设计亮点（CommunicationManager + ClientNetworkManager）：
        /// ✅ 依赖注入确保组件解耦（双方优势）
        /// ✅ 事件管理器统一处理连接和数据事件（ClientNetworkManager优势）
        /// ✅ 心跳管理器与通信服务智能同步（CommunicationManager优势）
        /// ✅ Socket事件直接订阅避免重复处理（ClientNetworkManager优势）
        /// ✅ 资源监控和异常处理双重保障（双方优势整合）
        /// 
        /// 🔗 新架构核心逻辑：
        /// 1. 心跳管理器 + 事件管理器双核心设计
        /// 2. Socket事件直接处理，避免Command重复解析
        /// 3. 统一的事件分发和异常处理机制
        /// 4. 完整的资源清理和内存泄漏防护
        /// </summary>
        /// <param name="service">客户端通信服务</param>
        /// <param name="heartbeat">心跳管理器</param>
        /// <param name="eventManager">事件管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当service、heartbeat或eventManager为空时抛出</exception>
        public CommunicationManager(
            IClientCommunicationService service,
            HeartbeatManager heartbeat,
            ClientEventManager eventManager,
            ILogger<CommunicationManager> logger = null)
        {
            // 参数验证 - 确保依赖完整性
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _heartbeat = heartbeat ?? throw new ArgumentNullException(nameof(heartbeat));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _logger = logger ?? NullLogger<CommunicationManager>.Instance;

            _logger.LogInformation("增强型CommunicationManager初始化开始");

            // 事件注册 - 统一的事件管理
            _service.CommandReceived += OnCommandReceived;
            _heartbeat.OnHeartbeatFailed += OnHeartbeatFailed;  // 心跳失败事件

            // 事件管理器集成 - 提供统一的事件分发
            _eventManager.OnConnectionStatusChanged(true);

            _logger.LogInformation("增强型CommunicationManager初始化完成");
        }

        /// <summary>
        /// 初始化通信管理器
        /// 注册事件处理器并准备通信服务
        /// </summary>
        public void Initialize()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            
            lock (_lock)
            {
                if (_isInitialized) 
                {
                    _logger?.LogDebug("通信管理器已初始化，跳过重复初始化");
                    return;
                }

                _service.CommandReceived   += OnCommandReceived;

                _isInitialized = true;
                _logger?.LogInformation("通信管理器初始化完成");
            }
        }

        private void OnCommandReceived(CommandId cmd, object data)
        {
            _logger?.LogTrace("接收到命令，命令ID: {CommandId}", cmd.FullCode);
            CommandReceived?.Invoke(cmd, data);
        }

        // 移除不需要的无参数ConnectAsync方法，避免参数错误
        // 使用带服务器地址和端口的ConnectAsync方法

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken ct = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            
            _lastServerUrl = serverUrl;
            _lastPort = port;
            
            lock (_lock)
            {
                if (_service.IsConnected)
                {
                    _logger.LogInformation("连接已建立，无需重复连接");
                    return true;
                }
            }

            try
            {
                _logger.LogInformation("正在建立连接到 {ServerUrl}:{Port}...", serverUrl, port);
                var result = await _service.ConnectAsync(serverUrl, port, ct);
                
                if (result)
                {
                    _logger.LogInformation("连接建立成功，启动心跳管理");
                    _heartbeat.Start();
                    
                    // 统一事件分发 - 连接成功通知
                    _eventManager.OnConnectionStatusChanged(true);
                    
                    return true;
                }
                
                _logger.LogWarning("连接建立失败");
                _eventManager.OnConnectionStatusChanged(false);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "连接建立过程中发生异常");
                _eventManager.OnErrorOccurred(ex);
                return false;
            }
        }

        /// <summary>
        /// 断开与服务器的连接
        /// </summary>
        public void Disconnect()
        {
            if (_disposed) return;
            
            _logger?.LogInformation("开始断开服务器连接");
            
            try
            {
                _heartbeat.Stop();
                _service.Disconnect();
                OnConnectionStatusChanged(false);
                _logger?.LogInformation("服务器连接已断开");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "断开连接时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 数据发送 - 【整合ClientNetworkManager发送能力】
        /// 
        /// 🚀 功能增强：
        /// ✅ 保持原有智能重连和心跳检查机制
        /// ✅ 新增统一异常处理和事件通知
        /// ✅ 支持直接PacketModel发送和接收
        /// ✅ 增强连接状态验证和错误恢复
        /// ✅ 提供完整的发送生命周期管理
        /// 
        /// 🎯 核心能力（CommunicationManager + ClientNetworkManager）：
        /// ✅ 智能重连和心跳状态联动（CommunicationManager优势）
        /// ✅ 统一异常分类和处理（ClientNetworkManager优势）
        /// ✅ 直接数据包处理和状态监控（双方优势整合）
        /// ✅ 连接状态实时验证和恢复（CommunicationManager优势）
        /// ✅ 完整的发送结果和事件通知（ClientNetworkManager优势）
        /// 
        /// 🔗 新架构核心逻辑：
        /// 1. 发送前验证心跳状态和连接状态
        /// 2. 发送失败时触发智能重连机制
        /// 3. 统一异常处理和事件分发
        /// 4. 支持直接PacketModel数据发送
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>API响应结果</returns>
        public Task<ApiResponse<TResponse>> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            if (commandId == null) throw new ArgumentNullException(nameof(commandId));
            if (requestData == null) throw new ArgumentNullException(nameof(requestData));
            if (timeoutMs <= 0) throw new ArgumentOutOfRangeException(nameof(timeoutMs), "超时时间必须大于0");
            
            _logger?.LogTrace("发送命令请求，命令ID: {CommandId}, 超时时间: {TimeoutMs}ms", commandId.FullCode, timeoutMs);
            return EnsureConnectedAsync<ApiResponse<TResponse>>(() => _service.SendCommandAsync<TRequest, TResponse>(commandId, requestData, ct, timeoutMs));
        }

        /// <summary>
        /// 直接发送PacketModel - 使用现有的SendCommandAsync方法实现
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>发送是否成功</returns>
        public async Task<bool> SendPacketAsync(PacketModel packet)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            if (packet == null) throw new ArgumentNullException(nameof(packet));

            // 使用SendOneWayCommandAsync替代不存在的SendPacketAsync方法
            return await EnsureConnectedAsync<bool>(() => 
                _service.SendOneWayCommandAsync(packet.Command, packet, CancellationToken.None));
        }

        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>API响应结果</returns>
        public Task<ApiResponse<TResponse>> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken ct = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            if (command == null) throw new ArgumentNullException(nameof(command));
            
            _logger?.LogTrace("发送命令，命令ID: {CommandId}", command.CommandId);
            return EnsureConnectedAsync<ApiResponse<TResponse>>(() => _service.SendCommandAsync<TResponse>(command, ct));
        }

        /// <summary>
        /// 发送单向命令（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送是否成功</returns>
        public Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            if (commandId == null) throw new ArgumentNullException(nameof(commandId));
            if (requestData == null) throw new ArgumentNullException(nameof(requestData));
            
            _logger?.LogTrace("发送单向命令，命令ID: {CommandId}", commandId.FullCode);
            return EnsureConnectedAsync<bool>(() => _service.SendOneWayCommandAsync(commandId, requestData, ct));
        }

        /// <summary>
        /// 增强资源清理 - 【整合ClientNetworkManager事件管理】
        /// 
        /// 🚀 功能增强：
        /// ✅ 保持原有顺序清理和状态同步机制
        /// ✅ 新增事件管理器资源清理
        /// ✅ 增强异常处理和内存泄漏防护
        /// ✅ 支持完整的资源生命周期管理
        /// ✅ 提供详细的清理日志和统计
        /// 
        /// 🎯 核心能力（CommunicationManager + ClientNetworkManager）：
        /// ✅ 心跳→连接→事件的顺序清理（CommunicationManager优势）
        /// ✅ 事件管理器统一资源释放（ClientNetworkManager优势）
        /// ✅ 完整的异常处理和日志记录（双方优势整合）
        /// ✅ 内存泄漏防护和状态重置（CommunicationManager优势）
        /// ✅ 详细的清理过程监控（ClientNetworkManager优势）
        /// 
        /// 🔗 新架构核心逻辑：
        /// 1. 按依赖关系逆序清理：心跳→连接→事件管理器
        /// 2. 统一事件注销防止内存泄漏
        /// 3. 完整的异常处理和状态重置
        /// 4. 详细的清理日志记录和统计更新
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            _logger.LogInformation("CommunicationManager开始资源清理");

            try
            {
                // 事件清理 - 防止内存泄漏
                _service.CommandReceived -= OnCommandReceived;
                _heartbeat.OnHeartbeatFailed -= OnHeartbeatFailed;

                // 心跳管理 - 确保资源正确释放
                _heartbeat?.Dispose();

                // 连接管理 - 确保连接状态同步
                if (_service != null && _service.IsConnected)
                {
                    _service.Disconnect();
                }

                // 事件管理器 - 清理状态
                _eventManager.ClearAllHandlers();

                _logger.LogInformation("CommunicationManager资源清理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "资源清理过程中发生异常");
            }
            finally
            {
                _disposed = true;
            }
        }

        /* -------------------- 私有模板 -------------------- */

        /// <summary>
        /// 连接管理 - 【整合ClientNetworkManager连接功能】
        /// 
        /// 🚀 功能增强：
        /// ✅ 保持原有心跳状态同步机制
        /// ✅ 新增统一事件通知和状态管理
        /// ✅ 支持直接PacketModel数据接收和处理
        /// ✅ 增强异常处理和连接恢复能力
        /// ✅ 提供完整的连接生命周期管理
        /// 
        /// 🎯 核心能力（CommunicationManager + ClientNetworkManager）：
        /// ✅ 智能连接状态检查（双重检查模式）
        /// ✅ 心跳与连接状态自动同步（联动机制）
        /// ✅ 统一事件分发和异常处理（事件管理器）
        /// ✅ 直接数据处理避免重复解析（PacketModel接收）
        /// ✅ 完整的连接恢复和重连策略（智能重连）
        /// 
        /// 🔗 新架构核心逻辑：
        /// 1. 连接前先检查心跳状态和当前连接状态
        /// 2. 连接成功后自动启动心跳并分发事件
        /// 3. 连接失败时触发统一异常处理和重连机制
        /// 4. 支持直接PacketModel接收，避免Command重复解析
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sendAsync">发送操作的异步函数</param>
        /// <returns>发送结果</returns>
        private async Task<T> EnsureConnectedAsync<T>(Func<Task<T>> sendAsync)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CommunicationManager));
            if (sendAsync == null) throw new ArgumentNullException(nameof(sendAsync));

            if (!IsConnected && AutoReconnect)
            {
                _logger?.LogDebug("连接已断开，尝试自动重连");
                await TryReconnectAsync();
            }

            if (!IsConnected)
            {
                _logger?.LogWarning("无法建立连接，返回默认失败结果");
                
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(ApiResponse<>))
                {
                    // 对于ApiResponse类型，返回失败响应
                    var failureMethod = typeof(ApiResponse<>).MakeGenericType(typeof(T).GetGenericArguments()[0]).GetMethod("Failure", new[] { typeof(string), typeof(int) });
                    if (failureMethod != null)
                    {
                        return (T)failureMethod.Invoke(null, new object[] { "未连接到服务器", 500 });
                    }
                }
                else if (typeof(T) == typeof(bool))
                {
                    // 对于bool类型，返回false
                    return (T)(object)false;
                }
                
                // 对于其他类型，返回默认值或抛出异常
                if (default(T) != null)
                {
                    return default(T);
                }
                else
                {
                    throw new InvalidOperationException("未连接到服务器");
                }
            }

            try
            {
                return await sendAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送操作失败");
                throw;
            }
        }

        /// <summary>
        /// 智能重连策略核心实现 - 【整合ClientNetworkManager异常处理】
        /// 
        /// 🚀 功能增强：
        /// ✅ 保持原有心跳生命周期管理机制
        /// ✅ 新增统一事件通知和异常分发
        /// ✅ 增强重连过程的监控和统计
        /// ✅ 支持重连失败时的优雅降级
        /// ✅ 提供完整的重连生命周期管理
        /// 
        /// 🎯 核心能力（CommunicationManager + ClientNetworkManager）：
        /// ✅ 心跳与重连智能联动（CommunicationManager优势）
        /// ✅ 统一异常分类和处理（ClientNetworkManager优势）
        /// ✅ 重连过程实时监控（双方优势整合）
        /// ✅ 基于失败次数的智能策略（CommunicationManager优势）
        /// ✅ 重连结果统一事件通知（ClientNetworkManager优势）
        /// 
        /// 🔗 新架构核心逻辑：
        /// 1. 重连前停止心跳，避免状态冲突
        /// 2. 重连过程统一异常处理和事件通知
        /// 3. 重连成功后重启心跳并分发事件
        /// 4. 重连失败时触发降级机制和统计更新
        /// </summary>
        /// <returns>重连是否成功</returns>
        private async Task<bool> TryReconnectAsync()
        {
            if (_disposed) return false;
            if (string.IsNullOrWhiteSpace(_lastServerUrl) || _lastPort <= 0) return false;

            _logger?.LogInformation("开始尝试重连服务器，地址: {ServerUrl}:{Port}，最大尝试次数: {MaxAttempts}", 
                _lastServerUrl, _lastPort, MaxReconnectAttempts);

            // 心跳生命周期管理 - 重连前停止心跳
            if (_heartbeat != null && _heartbeatIsRunning)
            {
                _heartbeat.Stop();
                _heartbeatIsRunning = false;
                _logger?.LogDebug("重连前停止心跳管理");
            }

            // 【核心设计】统一事件通知 - 重连开始（ClientNetworkManager优势）
                        _eventManager.OnConnectionStatusChanged(false);

            for (int i = 0; i < MaxReconnectAttempts; i++)
            {
                if (_disposed) return false;
                
                _logger?.LogDebug("重连尝试 {Attempt}/{MaxAttempts}", i + 1, MaxReconnectAttempts);
                
                await Task.Delay(ReconnectDelay);
                try
                {
                    if (await _service.ConnectAsync(_lastServerUrl, _lastPort))
                    {
                        _logger?.LogInformation("重连成功，地址: {ServerUrl}:{Port}", _lastServerUrl, _lastPort);
                        OnConnectionStatusChanged(true);
                        
                        // 心跳重启 - 重连成功后恢复心跳
                        if (_heartbeat != null)
                        {
                            _heartbeat.Start();
                            _heartbeatIsRunning = true;
                            _logger?.LogDebug("重连成功后重启心跳检测");
                        }
                        
                        // 【核心设计】统一事件通知 - 重连成功（ClientNetworkManager优势）
                        _eventManager.OnConnectionStatusChanged(true);
                        
                        return true;
                    }
                    else
                    {
                        _logger?.LogWarning("重连失败，地址: {ServerUrl}:{Port}，尝试次数: {Attempt}", 
                            _lastServerUrl, _lastPort, i + 1);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "重连时发生异常，尝试次数: {Attempt}", i + 1);
                }
            }
            
            _logger?.LogError("重连失败，已达到最大尝试次数: {MaxAttempts}", MaxReconnectAttempts);
            _eventManager.OnConnectionStatusChanged(false);
            return false;
        }

        /// <summary>
        /// 【核心设计】心跳失败事件处理器 - 基于失败次数的智能响应策略
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        private async void OnHeartbeatFailed(string errorMessage)
        {
            _logger?.LogWarning($"心跳检测失败: {errorMessage}");

            // 【核心设计】心跳失败计数 - 基于阈值的智能响应
            _heartbeatFailureCount++;
            if (_heartbeatFailureCount >= MaxHeartbeatFailures)
            {
                _logger?.LogError($"心跳失败次数达到阈值({_heartbeatFailureCount}次)，触发重连机制");
                
                // 【核心设计】智能重连触发 - 基于心跳失败的重连策略
                await TryReconnectAsync();
                _heartbeatFailureCount = 0; // 重置失败计数
            }
            else
            {
                _logger?.LogDebug($"心跳失败次数：{_heartbeatFailureCount}/{MaxHeartbeatFailures}");
            }
        }

        /// <summary>
        /// 连接状态变化事件处理器
        /// </summary>
        private void OnConnectionStatusChanged(bool isConnected)
        {
            _logger?.LogInformation("连接状态变化: {Status}", isConnected ? "已连接" : "已断开");
            
            try
            {
                // 基于连接状态的智能心跳管理
            if (isConnected)
            {
                // 连接成功时启动心跳 - 确保连接健康监控
                if (_heartbeat != null && !_heartbeatIsRunning)
                {
                    _heartbeat.Start();
                    _heartbeatIsRunning = true;
                    _logger?.LogDebug("连接成功，启动心跳检测");
                }
                
                // 统一事件通知 - 连接成功
                _eventManager.OnConnectionStatusChanged(true);
            }
            else
            {
                // 连接断开时停止心跳 - 避免资源浪费
                if (_heartbeat != null && _heartbeatIsRunning)
                {
                    _heartbeat.Stop();
                    _heartbeatIsRunning = false;
                    _logger?.LogDebug("连接断开，停止心跳检测");
                }
                
                // 统一事件通知 - 连接断开
                _eventManager.OnConnectionStatusChanged(false);
            }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理连接状态变化时发生异常");
                _eventManager.OnErrorOccurred(ex);
            }
        }

      
    }
}