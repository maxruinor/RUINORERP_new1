using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 心跳管理器
    /// 负责定期发送心跳命令以保持连接活跃
    /// </summary>
    public class HeartbeatManager : IDisposable
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly int _heartbeatIntervalMs;
        private readonly int _reconnectAttempts;
        private readonly int _reconnectIntervalMs;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _heartbeatTask;
        private int _failedAttempts;
        private bool _isDisposed;

        /// <summary>
        /// 心跳管理器构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="heartbeatIntervalMs">心跳间隔（毫秒），默认30秒</param>
        /// <param name="reconnectAttempts">重连尝试次数，默认3次</param>
        /// <param name="reconnectIntervalMs">重连间隔（毫秒），默认5秒</param>
        public HeartbeatManager(
            IClientCommunicationService communicationService,
            int heartbeatIntervalMs = 30000,
            int reconnectAttempts = 3,
            int reconnectIntervalMs = 5000)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _heartbeatIntervalMs = heartbeatIntervalMs;
            _reconnectAttempts = reconnectAttempts;
            _reconnectIntervalMs = reconnectIntervalMs;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 开始发送心跳
        /// </summary>
        public void Start()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(HeartbeatManager));
            }

            if (_heartbeatTask != null && !_heartbeatTask.IsCompleted)
            {
                return; // 已经在运行中
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _heartbeatTask = Task.Run(SendHeartbeatsAsync, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// 停止发送心跳
        /// </summary>
        public void Stop()
        {
            if (!_isDisposed)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _heartbeatTask = null;
            }
        }

        /// <summary>
        /// 定期发送心跳
        /// </summary>
        private async Task SendHeartbeatsAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if (_communicationService.IsConnected)
                    {
                        // 创建并发送心跳命令
                        var heartbeatCommand = new Commands.HeartbeatCommand();
                        var response = await _communicationService.SendCommandAsync<object>(heartbeatCommand, _cancellationTokenSource.Token);

                        if (response.Success)
                        {
                            // 心跳发送成功，重置失败计数器
                            _failedAttempts = 0;
                            OnHeartbeatSuccess();
                        }
                        else
                        {
                            // 心跳发送失败，增加失败计数器
                            _failedAttempts++;
                            OnHeartbeatFailed(response.Message);
                        }
                    }
                    else if (_failedAttempts < _reconnectAttempts)
                    {
                        // 如果连接断开，尝试重连
                        _failedAttempts++;
                        OnReconnectionAttempt(_failedAttempts);
                    }
                    else
                    {
                        // 超过重连次数，触发重连失败事件
                        OnReconnectionFailed();
                        _failedAttempts = 0;
                    }
                }
                catch (Exception ex)
                {
                    _failedAttempts++;
                    OnHeartbeatException(ex);
                }

                // 等待下一次心跳间隔
                try
                {
                    await Task.Delay(_heartbeatIntervalMs, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // 正常取消，不需要处理
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Stop();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #region 事件处理

        /// <summary>
        /// 心跳发送成功事件
        /// </summary>
        public event Action OnHeartbeatSuccess = delegate { };

        /// <summary>
        /// 心跳发送失败事件
        /// </summary>
        public event Action<string> OnHeartbeatFailed = delegate { };

        /// <summary>
        /// 心跳异常事件
        /// </summary>
        public event Action<Exception> OnHeartbeatException = delegate { };

        /// <summary>
        /// 重连尝试事件
        /// </summary>
        public event Action<int> OnReconnectionAttempt = delegate { };

        /// <summary>
        /// 重连失败事件
        /// </summary>
        public event Action OnReconnectionFailed = delegate { };

        /// <summary>
        /// 连接丢失事件
        /// </summary>
        public event Action ConnectionLost = delegate { };

        /// <summary>
        /// 心跳失败事件
        /// </summary>
        public event Action<Exception> HeartbeatFailed = delegate { };

        #endregion
    }
}