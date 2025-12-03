using System.Threading.Tasks;
using RUINORERP.UI.Network.TimeoutStatistics;
using Microsoft.Extensions.Logging;
using System;

namespace RUINORERP.UI.Network.ErrorHandling
{
    /// <summary>
    /// 连接错误处理策略
    /// 处理网络连接相关的错误，提供智能重连和错误恢复机制
    /// </summary>
    public class ConnectionErrorHandlingStrategy : IErrorHandlingStrategy
    {
        private readonly ILogger<ConnectionErrorHandlingStrategy> _logger;
        private readonly ConnectionManager _connectionManager;
        private int _consecutiveConnectionErrors = 0;
        private DateTime _lastConnectionErrorTime = DateTime.MinValue;
        private readonly object _errorCountLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="connectionManager">连接管理器</param>
        public ConnectionErrorHandlingStrategy(
            ILogger<ConnectionErrorHandlingStrategy> logger = null,
            ConnectionManager connectionManager = null)
        {
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<ConnectionErrorHandlingStrategy>.Instance;
            _connectionManager = connectionManager;
        }

        /// <summary>
        /// 处理连接错误
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="commandId">相关命令ID</param>
        /// <returns>异步任务</returns>
        public async Task HandleErrorAsync(NetworkErrorType errorType, string errorMessage, string commandId)
        {
            try
            {
                _logger?.LogDebug("处理连接错误：{ErrorType} - {ErrorMessage}, 命令ID: {CommandId}", 
                    errorType, errorMessage, commandId);

                // 记录连接错误次数
                TrackConnectionError();

                // 根据错误类型采取不同的处理策略
                switch (errorType)
                {
                    case NetworkErrorType.ConnectionError:
                        await HandleConnectionErrorAsync(errorMessage, commandId);
                        break;

                    case NetworkErrorType.TimeoutError:
                        await HandleTimeoutErrorAsync(errorMessage, commandId);
                        break;

                    case NetworkErrorType.ServerError:
                        await HandleNetworkErrorAsync(errorMessage, commandId);
                        break;

                    default:
                        _logger?.LogDebug("未处理的连接相关错误类型：{ErrorType}", errorType);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理连接错误时发生异常：{ErrorMessage}", errorMessage);
            }
        }

        /// <summary>
        /// 处理连接错误
        /// </summary>
        private async Task HandleConnectionErrorAsync(string errorMessage, string commandId)
        {
            _logger?.LogInformation("检测到连接错误：{ErrorMessage}", errorMessage);

            // 检查是否需要触发重连
            if (_connectionManager != null)
            {
                // 检查当前连接状态
                if (!_connectionManager.IsConnected)
                {
                    var (isReconnecting, lastAttempt, _) = _connectionManager.GetReconnectStatus();
                    
                    if (!isReconnecting)
                    {
                        _logger?.LogDebug("连接已断开且未在重连中，尝试手动重连");
                        
                        // 尝试手动重连
                        bool reconnectResult = await _connectionManager.ManualReconnectAsync();
                        if (reconnectResult)
                        {
                            ResetErrorCount();
                            _logger?.LogDebug("手动重连成功");
                        }
                        else
                        {
                            _logger?.LogWarning("手动重连失败，将由自动重连机制处理");
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("连接已在重连中，不重复触发");
                    }
                }
            }

            // 根据连续错误次数采取不同策略
            if (_consecutiveConnectionErrors >= 5)
            {
                _logger?.LogWarning("连续连接错误次数过多（{Count}），建议检查网络连接", _consecutiveConnectionErrors);
                // 可以在这里添加更严重的处理逻辑，比如通知用户检查网络
            }
        }

        /// <summary>
        /// 处理超时错误
        /// </summary>
        private async Task HandleTimeoutErrorAsync(string errorMessage, string commandId)
        {
            _logger?.LogWarning("检测到超时错误：{ErrorMessage}", errorMessage);

            // 超时错误可能需要特殊处理
            // 比如调整超时时间、重试等
            if (_consecutiveConnectionErrors >= 3)
            {
                _logger?.LogInformation("连续超时次数较多，检查连接状态");
                if (_connectionManager != null && !_connectionManager.IsConnected)
                {
                    await _connectionManager.ManualReconnectAsync();
                }
            }
        }

        /// <summary>
        /// 处理网络错误
        /// </summary>
        private async Task HandleNetworkErrorAsync(string errorMessage, string commandId)
        {
            _logger?.LogWarning("检测到网络错误：{ErrorMessage}", errorMessage);

            // 网络错误可能需要等待网络恢复
            // 检查网络可用性
            if (await CheckNetworkAvailabilityAsync())
            {
                _logger?.LogDebug("网络可用，尝试重新连接");
                if (_connectionManager != null)
                {
                    await _connectionManager.ManualReconnectAsync();
                }
            }
            else
            {
                _logger?.LogInformation("网络不可用，等待网络恢复");
            }
        }

        /// <summary>
        /// 检查网络可用性
        /// </summary>
        private async Task<bool> CheckNetworkAvailabilityAsync()
        {
            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await ping.SendPingAsync("114.114.114.114", 3000); // 国内DNS，3秒超时
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 记录连接错误
        /// </summary>
        private void TrackConnectionError()
        {
            lock (_errorCountLock)
            {
                var timeSinceLastError = DateTime.Now - _lastConnectionErrorTime;
                
                // 如果距离上次错误超过1分钟，重置计数器
                if (timeSinceLastError.TotalMinutes > 1)
                {
                    _consecutiveConnectionErrors = 1;
                }
                else
                {
                    _consecutiveConnectionErrors++;
                }

                _lastConnectionErrorTime = DateTime.Now;
                
                _logger?.LogDebug("连接错误计数：{Count}, 距离上次错误：{Time}秒", 
                    _consecutiveConnectionErrors, timeSinceLastError.TotalSeconds);
            }
        }

        /// <summary>
        /// 重置错误计数
        /// </summary>
        private void ResetErrorCount()
        {
            lock (_errorCountLock)
            {
                _consecutiveConnectionErrors = 0;
                _logger?.LogDebug("连接错误计数已重置");
            }
        }

        /// <summary>
        /// 获取是否支持该错误类型
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <returns>是否支持该错误类型的处理</returns>
        public bool SupportsErrorType(NetworkErrorType errorType)
        {
            return errorType == NetworkErrorType.ConnectionError ||
                   errorType == NetworkErrorType.TimeoutError ||
                   errorType == NetworkErrorType.ServerError;
        }

        /// <summary>
        /// 获取错误统计信息
        /// </summary>
        /// <returns>错误统计信息</returns>
        public (int ConsecutiveErrors, DateTime LastErrorTime) GetErrorStatistics()
        {
            lock (_errorCountLock)
            {
                return (_consecutiveConnectionErrors, _lastConnectionErrorTime);
            }
        }
    }
}