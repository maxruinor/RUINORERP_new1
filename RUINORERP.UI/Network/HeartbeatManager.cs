using RUINORERP.PacketSpec.Models.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.UI.Services;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 心跳管理器 - 专注于核心的心跳检测功能
    /// 负责定期检测与服务器的连接状态
    [obs]过时
    /// </summary>
    public class HeartbeatManager : IDisposable
    {
        #region 私有字段

        private readonly ISocketClient _socketClient;
        private ClientCommunicationService? _communicationService;
        private readonly int _heartbeatIntervalMs;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _heartbeatTask;
        private int _failedAttempts;
        private bool _isDisposed;
        private bool _isRunning;
        private readonly object _lock = new object();
        private readonly ILogger<HeartbeatManager> _logger;
        private DateTime _lastHeartbeatTime;
        private readonly IClientSystemInfoService _systemInfoService;

        #endregion

        #region 公共属性

        /// <summary>
        /// 心跳失败事件
        /// 当心跳失败时触发，参数为连续失败次数
        /// </summary>
        public event Action<int> HeartbeatFailed;

        /// <summary>
        /// 心跳恢复事件
        /// 当心跳从失败状态恢复时触发
        /// </summary>
        public event Action HeartbeatRecovered;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_lock)
                {
                    return _isRunning && _heartbeatTask != null && !_heartbeatTask.IsCompleted;
                }
            }
        }
        /// <summary>
        /// 设置客户端通信服务引用
        /// 在依赖注入配置中被调用，避免循环依赖
        /// </summary>
        /// <param name="communicationService">客户端通信服务实例</param>
        public void SetCommunicationService(ClientCommunicationService communicationService)
        {
            _communicationService = communicationService;
        }
           

        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime LastHeartbeatTime => _lastHeartbeatTime;

        /// <summary>
        /// 当前失败次数
        /// </summary>
        public int CurrentFailedAttempts => _failedAttempts;

        #endregion

        #region 构造函数

        /// <summary>
        /// 心跳管理器构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="communicationService">客户端通信服务</param>
        /// <param name="systemInfoService">系统信息服务（可选）</param>
        /// <param name="heartbeatIntervalMs">心跳间隔，默认30秒</param>
        /// <param name="logger">日志记录器（可选）</param>
        public HeartbeatManager(
            ISocketClient socketClient,
            ClientCommunicationService communicationService,
            IClientSystemInfoService systemInfoService = null,
            int heartbeatIntervalMs = 30000,
            ILogger<HeartbeatManager> logger = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _systemInfoService = systemInfoService;
            _heartbeatIntervalMs = heartbeatIntervalMs;
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<HeartbeatManager>.Instance;
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 启动心跳检测
        /// </summary>
        public void Start()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(HeartbeatManager));
            }

            lock (_lock)
            {
                if (_isRunning || (_heartbeatTask != null && !_heartbeatTask.IsCompleted))
                    return;

                _isRunning = true;
                _failedAttempts = 0;
                _cancellationTokenSource = new CancellationTokenSource();
                
                _heartbeatTask = Task.Run(async () => await HeartbeatLoopAsync(_cancellationTokenSource.Token));
                _logger?.LogInformation("心跳管理器已启动，间隔：{IntervalMs}ms", _heartbeatIntervalMs);
            }
        }

        /// <summary>
        /// 停止心跳检测
        /// </summary>
        public void Stop()
        {
            lock (_lock)
            {
                if (!_isRunning)
                    return;

                _isRunning = false;
                _cancellationTokenSource?.Cancel();
                
                try
                {
                    if (_heartbeatTask != null && !_heartbeatTask.IsCompleted)
                    {
                        _heartbeatTask.Wait(TimeSpan.FromSeconds(3));
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "停止心跳任务时发生异常");
                }
                finally
                {
                    _heartbeatTask = null;
                    _cancellationTokenSource?.Dispose();
                    _cancellationTokenSource = null;
                }

                _logger?.LogInformation("心跳管理器已停止");
            }
        }

        /// <summary>
        /// 心跳检测循环
        /// 定期执行心跳检测
        /// </summary>
        private async Task HeartbeatLoopAsync(CancellationToken cancellationToken)
        {
            _logger?.LogDebug("进入心跳循环");

            while (!cancellationToken.IsCancellationRequested && _isRunning)
            {
                try
                {
                    // 检查连接状态
                    if (!_socketClient.IsConnected)
                    {
                        await Task.Delay(_heartbeatIntervalMs, cancellationToken);
                        continue;
                    }

                    // 发送心跳
                    bool success = await SendHeartbeatAsync(cancellationToken);

                    lock (_lock)
                    {
                        _lastHeartbeatTime = DateTime.Now;
                        
                        if (success)
                        {
                            // 如果之前有失败，触发恢复事件
                            if (_failedAttempts > 0)
                            {
                                _failedAttempts = 0;
                                HeartbeatRecovered?.Invoke();
                                _logger?.LogInformation("心跳恢复");
                            }
                        }
                        else
                        {
                            _failedAttempts++;
                            
                            // 触发失败事件
                            HeartbeatFailed?.Invoke(_failedAttempts);
                            _logger?.LogWarning("心跳失败，连续失败次数：{FailedAttempts}", _failedAttempts);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "心跳循环中发生异常");
                    lock (_lock)
                    {
                        _failedAttempts++;
                        HeartbeatFailed?.Invoke(_failedAttempts);
                    }
                }

                await Task.Delay(_heartbeatIntervalMs, cancellationToken);
            }

            _logger?.LogDebug("退出心跳循环");
        }

        /// <summary>
        /// 发送单个心跳请求
        /// </summary>
        private async Task<bool> SendHeartbeatAsync(CancellationToken cancellationToken)
        {
            try
            {
                var heartbeatRequest = new HeartbeatRequest();
                
                // 如果有系统信息服务，添加系统信息
                if (_systemInfoService != null)
                {
                    //太耗时，先注释掉
                    //heartbeatRequest.ResourceUsage = _systemInfoService.GetResourceUsage();
                }

                var response = await _communicationService.SendCommandWithResponseAsync<HeartbeatResponse>(
                    SystemCommands.Heartbeat, heartbeatRequest,  cancellationToken);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "发送心跳时发生异常");
                return false;
            }
        }

        #endregion

        #region 资源释放

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的具体实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                Stop();
                _isDisposed = true;
            }
        }

        #endregion
    }
}