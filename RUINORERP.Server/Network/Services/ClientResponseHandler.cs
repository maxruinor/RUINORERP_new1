using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 客户端响应处理器 - 统一管理所有客户端响应数据
    /// 不依赖事件机制，集中处理所有客户端响应，特别处理欢迎机制
    /// </summary>
    public class ClientResponseHandler : IClientResponseHandler
    {
        #region 字段和属性

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<ClientResponseHandler> _logger;

        /// <summary>
        /// 待处理的请求任务字典 - 用于匹配请求和响应
        /// Key: RequestId, Value: TaskCompletionSource<PacketModel>
        /// </summary>
        private readonly ConcurrentDictionary<string, TaskCompletionSource<PacketModel>> _pendingRequests;

        /// <summary>
        /// 响应统计信息
        /// </summary>
        private readonly ResponseStatistics _statistics;

        /// <summary>
        /// 响应处理委托字典 - 按命令类型注册自定义处理逻辑
        /// Key: CommandCode, Value: 响应处理委托
        /// </summary>
        private readonly Dictionary<int, Func<PacketModel, SessionInfo, Task<ResponseProcessingResult>>> _responseHandlers;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ClientResponseHandler(
            ILogger<ClientResponseHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pendingRequests = new ConcurrentDictionary<string, TaskCompletionSource<PacketModel>>();
            _statistics = new ResponseStatistics();
            _responseHandlers = new Dictionary<int, Func<PacketModel, SessionInfo, Task<ResponseProcessingResult>>>();

            // 注册默认响应处理器
            RegisterDefaultHandlers();
        }

        #endregion

        #region 核心处理方法

        /// <summary>
        /// 处理客户端响应 - 统一入口方法
        /// </summary>
        /// <param name="packet">响应数据包</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>处理结果</returns>
        public async Task<ResponseProcessingResult> HandleResponseAsync(PacketModel packet, SessionInfo sessionInfo)
        {
            try
            {
                if (packet == null)
                {
                    _logger.LogWarning("收到空响应数据包");
                    return ResponseProcessingResult.Failure("响应数据包为空");
                }

                var sessionId = sessionInfo?.SessionID;
                var requestId = packet?.ExecutionContext?.RequestId;

                if (string.IsNullOrEmpty(sessionId))
                {
                    _logger.LogWarning("会话信息无效");
                    return ResponseProcessingResult.Failure("会话信息无效");
                }

                // 更新会话活动时间
                sessionInfo.UpdateActivity();

                // 优先检查是否有等待该响应的请求（SendCommandAndWaitForResponseAsync模式）
                if (!string.IsNullOrEmpty(requestId))
                {
                    if (_pendingRequests.TryRemove(requestId, out var pendingRequest))
                    {
                        _logger.LogDebug("匹配到待处理请求，请求ID: {RequestId}, 会话: {SessionId}", requestId, sessionId);

                        // 完成等待任务
                        pendingRequest.TrySetResult(packet);

                        // 统计信息更新
                        _statistics.IncrementMatchedPendingRequests();
                        _statistics.IncrementTotalProcessedResponses();

                        return ResponseProcessingResult.Success($"已匹配请求 {requestId}");
                    }
                }

                // 没有匹配的待处理请求，使用特定处理器处理
                return await ProcessWithHandlerAsync(packet, sessionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理客户端响应时发生异常");
                _statistics.IncrementProcessingErrors();
                return ResponseProcessingResult.Failure($"处理异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 使用注册的处理器处理响应
        /// </summary>
        /// <param name="packet">响应数据包</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseProcessingResult> ProcessWithHandlerAsync(PacketModel packet, SessionInfo sessionInfo)
        {
            var commandCode = packet.CommandId.FullCode;

            // 查找是否有注册的特定处理器
            if (_responseHandlers.TryGetValue(commandCode, out var handler))
            {
                _logger.LogDebug("使用特定处理器处理响应: {CommandCode}, 会话: {SessionId}", commandCode, sessionInfo.SessionID);
                try
                {
                    var result = await handler(packet, sessionInfo);
                    _statistics.IncrementHandledByCustomHandlers();
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "特定处理器执行失败: {CommandCode}", commandCode);
                    return ResponseProcessingResult.Failure($"处理器执行失败: {ex.Message}");
                }
            }

                // 没有特定处理器，使用默认处理
                return ProcessDefault(packet, sessionInfo);
        }

        /// <summary>
        /// 默认响应处理逻辑
        /// </summary>
        /// <param name="packet">响应数据包</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>处理结果</returns>
        private ResponseProcessingResult ProcessDefault(PacketModel packet, SessionInfo sessionInfo)
        {
            _logger.LogDebug("使用默认处理器处理响应: {CommandCode}, 会话: {SessionId}", packet.CommandId.FullCode, sessionInfo.SessionID);

            // 默认处理：只记录日志，不执行任何操作
            // 这允许服务器处理不需要特定逻辑的响应
            _statistics.IncrementHandledByDefaultHandler();

            return ResponseProcessingResult.Success("使用默认处理逻辑");
        }

        #endregion

        #region 欢迎机制专用处理

        /// <summary>
        /// 处理欢迎确认响应（WelcomeAck）
        /// 特别处理欢迎机制中的响应情况，自动将会话状态标记为已验证
        /// </summary>
        /// <param name="packet">响应数据包</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>处理结果</returns>
        private ResponseProcessingResult HandleWelcomeAckAsync(PacketModel packet, SessionInfo sessionInfo)
        {
            try
            {
                _logger.LogInformation("收到欢迎确认响应: {SessionId}, IP: {ClientIp}", sessionInfo.SessionID, sessionInfo.ClientIp);

                // 验证响应类型
                if (!(packet.Response is WelcomeResponse welcomeResponse))
                {
                    _logger.LogWarning("欢迎确认响应类型不匹配，期望: WelcomeResponse, 实际: {ResponseType}", 
                        packet.Response?.GetType().Name);
                    return ResponseProcessingResult.Failure("响应类型不匹配");
                }

                // 验证会话状态
                if (sessionInfo.IsVerified)
                {
                    _logger.LogWarning("会话已验证，收到重复的欢迎确认: {SessionId}", sessionInfo.SessionID);
                    return ResponseProcessingResult.Success("会话已验证（重复确认）");
                }

                // 检查欢迎消息是否已发送
                if (!sessionInfo.WelcomeSentTime.HasValue)
                {
                    _logger.LogWarning("欢迎消息未发送，但收到欢迎确认: {SessionId}", sessionInfo.SessionID);
                    return ResponseProcessingResult.Failure("欢迎消息未发送");
                }

                // 检查欢迎确认超时（2分钟超时）
                var welcomeTimeout = TimeSpan.FromMinutes(2);
                if (sessionInfo.WelcomeSentTime.Value.Add(welcomeTimeout) < DateTime.Now)
                {
                    _logger.LogWarning("欢迎确认超时: {SessionId}, 发送时间: {SentTime}", 
                        sessionInfo.SessionID, sessionInfo.WelcomeSentTime.Value);
                    return ResponseProcessingResult.Failure("欢迎确认超时");
                }

                // 记录客户端信息
                if (sessionInfo.UserInfo == null)
                {
                    sessionInfo.UserInfo = new CurrentUserInfo();
                }

                sessionInfo.UserInfo.客户端版本 = welcomeResponse.ClientVersion;
                sessionInfo.UserInfo.操作系统 = welcomeResponse.ClientOS;
                sessionInfo.UserInfo.机器名 = welcomeResponse.ClientMachineName;
                sessionInfo.UserInfo.CPU信息 = welcomeResponse.ClientCPU;
                sessionInfo.UserInfo.内存大小 = welcomeResponse.ClientMemoryMB > 0
                    ? $"{welcomeResponse.ClientMemoryMB / 1024:F1} GB"
                    : "未知";

                // 标记会话为已验证
                sessionInfo.IsVerified = true;
                sessionInfo.WelcomeAckReceived = true;

                // 更新会话状态
                sessionInfo.Status = SessionStatus.Active;

                // 计算欢迎握手耗时
                var handshakeDuration = DateTime.Now - sessionInfo.WelcomeSentTime.Value;

                // 统计信息更新
                _statistics.IncrementWelcomeAckReceived();
                _statistics.AddTotalWelcomeHandshakeTimeMs((long)handshakeDuration.TotalMilliseconds);

                return ResponseProcessingResult.Success("欢迎握手成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理欢迎确认响应时发生异常: {SessionId}", sessionInfo.SessionID);
                _statistics.IncrementWelcomeAckErrors();
                return ResponseProcessingResult.Failure($"欢迎确认处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 异步处理欢迎确认响应（用于注册到委托字典）
        /// </summary>
        /// <param name="packet">响应数据包</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseProcessingResult> HandleWelcomeAckAsyncWrapper(PacketModel packet, SessionInfo sessionInfo)
        {
            // 实际处理是同步的，这里包装成异步以匹配委托签名
            return await Task.FromResult(HandleWelcomeAckAsync(packet, sessionInfo));
        }

        #endregion

        #region 待处理请求管理

        /// <summary>
        /// 注册待处理请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <param name="taskCompletionSource">任务完成源</param>
        /// <returns>是否注册成功</returns>
        public bool RegisterPendingRequest(string requestId, TaskCompletionSource<PacketModel> taskCompletionSource)
        {
            if (string.IsNullOrEmpty(requestId) || taskCompletionSource == null)
            {
                _logger.LogWarning("注册待处理请求失败：参数无效");
                return false;
            }

            var result = _pendingRequests.TryAdd(requestId, taskCompletionSource);
            if (result)
            {
                _statistics.IncrementPendingRequestsRegistered();
                _logger.LogDebug("注册待处理请求: {RequestId}", requestId);
            }
            else
            {
                _logger.LogWarning("注册待处理请求失败：请求ID已存在: {RequestId}", requestId);
            }

            return result;
        }

        /// <summary>
        /// 移除待处理请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>是否移除成功</returns>
        public bool RemovePendingRequest(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                return false;
            }

            var result = _pendingRequests.TryRemove(requestId, out _);
            if (result)
            {
                _statistics.IncrementPendingRequestsRemoved();
                _logger.LogDebug("移除待处理请求: {RequestId}", requestId);
            }

            return result;
        }

        /// <summary>
        /// 清理过期的待处理请求
        /// </summary>
        /// <param name="timeoutMinutes">超时分钟数，默认5分钟</param>
        /// <returns>清理的请求数量</returns>
        public int CleanupExpiredPendingRequests(int timeoutMinutes = 5)
        {
            var cleanedCount = 0;
            var now = DateTime.UtcNow;

            // 查找已完成的任务（Task.IsCompleted为true）
            foreach (var kvp in _pendingRequests.Where(kvp => kvp.Value.Task.IsCompleted))
            {
                if (_pendingRequests.TryRemove(kvp.Key, out var tcs))
                {
                    cleanedCount++;
                    _logger.LogDebug("清理已完成的待处理请求: {RequestId}", kvp.Key);
                }
            }

            if (cleanedCount > 0)
            {
                _logger.LogInformation("清理了 {Count} 个已完成的待处理请求", cleanedCount);
                _statistics.AddExpiredPendingRequestsCleaned(cleanedCount);
            }

            return cleanedCount;
        }

        #endregion

        #region 响应处理器注册

        /// <summary>
        /// 注册自定义响应处理器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理委托</param>
        public void RegisterResponseHandler(CommandId commandId, Func<PacketModel, SessionInfo, Task<ResponseProcessingResult>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var commandCode = commandId.FullCode;
            _responseHandlers[commandCode] = handler;
            _logger.LogInformation("注册响应处理器: {CommandCode}", commandCode);
        }

        /// <summary>
        /// 注册默认响应处理器
        /// </summary>
        private void RegisterDefaultHandlers()
        {
            // 注册欢迎确认处理器
            RegisterResponseHandler(PacketSpec.Commands.SystemCommands.WelcomeAck, HandleWelcomeAckAsyncWrapper);
        }

        /// <summary>
        /// 取消注册响应处理器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>是否取消成功</returns>
        public bool UnregisterResponseHandler(CommandId commandId)
        {
            var commandCode = commandId.FullCode;
            var removed = _responseHandlers.Remove(commandCode);
            if (removed)
            {
                _logger.LogInformation("取消注册响应处理器: {CommandCode}", commandCode);
            }
            return removed;
        }

        #endregion

        #region 统计信息

        /// <summary>
        /// 获取响应统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public ResponseStatistics GetStatistics()
        {
            return new ResponseStatistics
            {
                TotalProcessedResponses = _statistics.TotalProcessedResponses,
                MatchedPendingRequests = _statistics.MatchedPendingRequests,
                HandledByCustomHandlers = _statistics.HandledByCustomHandlers,
                HandledByDefaultHandler = _statistics.HandledByDefaultHandler,
                ProcessingErrors = _statistics.ProcessingErrors,
                WelcomeAckReceived = _statistics.WelcomeAckReceived,
                WelcomeAckErrors = _statistics.WelcomeAckErrors,
                TotalWelcomeHandshakeTimeMs = _statistics.TotalWelcomeHandshakeTimeMs,
                PendingRequestsRegistered = _statistics.PendingRequestsRegistered,
                PendingRequestsRemoved = _statistics.PendingRequestsRemoved,
                ExpiredPendingRequestsCleaned = _statistics.ExpiredPendingRequestsCleaned,
                CurrentPendingRequestsCount = _pendingRequests.Count
            };
        }

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public void ResetStatistics()
        {
            _statistics.TotalProcessedResponses = 0;
            _statistics.MatchedPendingRequests = 0;
            _statistics.HandledByCustomHandlers = 0;
            _statistics.HandledByDefaultHandler = 0;
            _statistics.ProcessingErrors = 0;
            _statistics.WelcomeAckReceived = 0;
            _statistics.WelcomeAckErrors = 0;
            _statistics.TotalWelcomeHandshakeTimeMs = 0;
            _statistics.PendingRequestsRegistered = 0;
            _statistics.PendingRequestsRemoved = 0;
            _statistics.ExpiredPendingRequestsCleaned = 0;

            _logger.LogInformation("响应统计信息已重置");
        }

        #endregion
    }
}
