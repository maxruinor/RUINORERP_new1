using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Models;
using RUINORERP.Model;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Monitoring;
using RUINORERP.Common.LogHelper;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 心跳命令处理器1
    /// 处理客户端发送的心跳命令，维持连接活跃状态
    /// 优化版：动态计算心跳间隔，增强错误处理
    /// </summary>
    [CommandHandler("HeartbeatCommandHandler", priority: 0)]
    public class HeartbeatCommandHandler : BaseCommandHandler
    {
        private readonly ISessionService _sessionService;
        private readonly HeartbeatBatchProcessor _batchProcessor;
        private readonly HeartbeatPerformanceMonitor _performanceMonitor;

        /// <summary>
        /// 构造函数 - 通过依赖注入获取服务
        /// </summary>
        public HeartbeatCommandHandler(
             ISessionService sessionService,
             HeartbeatBatchProcessor batchProcessor,
             HeartbeatPerformanceMonitor performanceMonitor,
            ILogger<HeartbeatCommandHandler> logger) : base(logger ?? throw new ArgumentNullException(nameof(logger)))
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _batchProcessor = batchProcessor ?? throw new ArgumentNullException(nameof(batchProcessor));
            _performanceMonitor = performanceMonitor ?? throw new ArgumentNullException(nameof(performanceMonitor));

            // 使用安全方法设置支持的命令
            SetSupportedCommands(SystemCommands.Heartbeat);

            // 提高清理频率到1分钟
            _cleanupTimer = new Timer(
                CleanupExpiredHeartbeatResponses, 
                null, 
                TimeSpan.FromMinutes(1),  // 首次延迟1分钟
                TimeSpan.FromMinutes(1)   // 每1分钟清理一次
            );

            // ✅ 方案C：订阅会话断开事件，会话断开时立即清理心跳记录
            _sessionService.SessionDisconnected += OnSessionDisconnected;
        }

        /// <summary>
        /// ✅ 方案C：会话断开事件处理 - 立即清理对应的心跳记录
        /// </summary>
        private void OnSessionDisconnected(SessionInfo sessionInfo)
        {
            try
            {
                if (sessionInfo == null)
                    return;

                // 根据SessionID清理
                if (!string.IsNullOrEmpty(sessionInfo.SessionID))
                {
                    _lastHeartbeatResponses.TryRemove(sessionInfo.SessionID, out _);
                }

                // 根据UserId清理（如果存在）
                if (sessionInfo.UserId.HasValue && sessionInfo.UserId.Value > 0)
                {
                    var userIdStr = sessionInfo.UserId.Value.ToString();
                    _lastHeartbeatResponses.TryRemove(userIdStr, out _);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清理会话断开时的心跳记录异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;
            try
            {
                var commandId = cmd.Packet.CommandId;

                if (commandId == SystemCommands.Heartbeat)
                {
                    var response = await HandleHeartbeatAsync(cmd, cancellationToken);

                    // 记录处理时间
                    var processingTime = (DateTime.Now - startTime).TotalMilliseconds;
                    _performanceMonitor.RecordProcessingTime((long)processingTime);

                    return response;
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的命令类型: {cmd.Packet.CommandId.ToString()}");
                }
            }
            catch (Exception ex)
            {
                var processingDuration = (DateTime.Now - startTime).TotalMilliseconds;
                LogError($"处理心跳命令异常，耗时: {processingDuration}ms", ex);
                LogWarning($"[主动断开连接] 心跳命令处理异常，可能导致连接中断: {ex.Message}");

                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理心跳命令异常");
            }
        }

        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => _sessionService;

        /// <summary>
        /// 缓存的心跳响应模板，减少序列化开销
        /// </summary>
        private static readonly HeartbeatResponse _cachedSuccessResponse = new HeartbeatResponse
        {
            IsSuccess = true,
            Status = "OK",
            ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            ServerInfo = new Dictionary<string, object>
            {
                ["ServerVersion"] = "1.0.0",
                ["RecommendedInterval"] = 15000
            }
        };

        /// <summary>
        /// 上次心跳响应时间缓存，用于响应合并
        /// </summary>
        private readonly ConcurrentDictionary<string, DateTime> _lastHeartbeatResponses =
            new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 清理定时器，用于定期清理过期的心跳记录
        /// </summary>
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// 处理心跳命令（优化版 - 快速响应）
        /// 修复：简化处理逻辑，快速响应客户端，避免超时
        /// </summary>
        private async Task<IResponse> HandleHeartbeatAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;

            try
            {
                if (queuedCommand.Packet.Request is HeartbeatRequest heartbeatRequest)
                {
                    // 快速验证请求
                    if (!heartbeatRequest.IsValid() || heartbeatRequest.UserId == 0)
                    {
                        // 快速返回失败响应，不记录详细日志
                        return new HeartbeatResponse
                        {
                            IsSuccess = false,
                            Status = "Invalid Request",
                            NextIntervalMs = 30000
                        };
                    }

                    // ✅ 优化：优先使用UserId查找会话（更稳定，重连后不变）
                    var sessionInfo = SessionService.GetSession(heartbeatRequest.UserId);

                    if (sessionInfo == null)
                    {
                        // ⚠️ 会话不存在，可能是重连后登录尚未完成
                        Logger?.LogWarning($"[心跳] 会话未找到: UserId={heartbeatRequest?.UserId ?? 0}, ClientId={heartbeatRequest?.ClientId ?? string.Empty}, ComputerName={heartbeatRequest?.ComputerName ?? string.Empty}");

                        // ✅ 修复：延长重试间隔至5秒，避免心跳风暴
                        return new HeartbeatResponse
                        {
                            IsSuccess = false,
                            Status = "SessionNotReady", // ✅ 明确状态：会话未就绪
                            NextIntervalMs = 5000, // ✅ 从3秒延长至5秒，避免请求风暴
                            ErrorMessage = "会话未就绪，请稍后重试"
                        };
                    }
                    
                    // ✅ 新增：检查会话是否已断开，如果断开则快速失败
                    if (!sessionInfo.IsConnected)
                    {
                        Logger?.LogWarning($"[心跳] 会话已断开: UserId={heartbeatRequest.UserId}, SessionId={sessionInfo.SessionID}");
                        
                        return new HeartbeatResponse
                        {
                            IsSuccess = false,
                            Status = "Session Disconnected",
                            NextIntervalMs = 2000, // ✅ 更短的间隔，促使客户端快速重连
                            ErrorMessage = "会话已断开，请重新连接"
                        };
                    }

                    // 立即更新最后活动时间和心跳时间（关键：快速更新）
                    var currentTime = DateTime.Now;
                    sessionInfo.LastActivityTime = currentTime;
                    sessionInfo.LastHeartbeat = currentTime;
                    sessionInfo.HeartbeatFailedCount = 0; // 重置失败计数

                    // 快速创建响应（不等待异步操作）
                    var nextIntervalMs = CalculateNextHeartbeatInterval(sessionInfo);
                    var response = new HeartbeatResponse
                    {
                        IsSuccess = true,
                        Status = "OK",
                        ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        NextIntervalMs = nextIntervalMs,
                        ServerInfo = new Dictionary<string, object>
                        {
                            ["ServerTime"] = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            ["SessionId"] = sessionInfo.SessionID,
                            ["RecommendedInterval"] = nextIntervalMs
                        }
                    };

                    // 使用批量处理器处理更新，减少Task.Run创建
                    _batchProcessor.EnqueueHeartbeat(heartbeatRequest, sessionInfo.SessionID);

                    return response;
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(queuedCommand.Packet, "心跳请求数据格式错误");
                }
            }
            catch (Exception ex)
            {
                var processingDuration = (DateTime.Now - startTime).TotalMilliseconds;
                LogError($"处理心跳命令异常，耗时: {processingDuration}ms", ex);
                _performanceMonitor.RecordError();
                return ResponseFactory.CreateSpecificErrorResponse(queuedCommand.Packet.ExecutionContext, ex, "处理心跳命令异常");
            }
        }

        /// <summary>
        /// 动态计算下次心跳间隔
        /// ✅ 外网优化：根据网络状态和会话活跃度智能调整心跳间隔，最大不超过30秒
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>下次心跳间隔（毫秒）</returns>
        private int CalculateNextHeartbeatInterval(SessionInfo sessionInfo)
        {
            // 默认15秒间隔，与客户端保持一致
            const int defaultIntervalMs = 15000;
            const int minIntervalMs = 10000;  // 最小10秒
            const int maxIntervalMs = 30000;  // ✅ 外网优化：最大30秒（原60秒太长，易被防火墙断开）

            // 如果会话信息无效，使用默认间隔
            if (sessionInfo == null)
            {
                //logger?.LogWarning("会话信息为空，使用默认心跳间隔");
                return defaultIntervalMs;
            }

            // 根据会话活跃度调整间隔
            var timeSinceLastActivity = DateTime.Now - sessionInfo.LastActivityTime;
            var intervalMs = defaultIntervalMs;

            if (timeSinceLastActivity.TotalMinutes < 10)
            {
                // 10分钟内有活动，使用较短间隔（快速检测断线）
                intervalMs = minIntervalMs;
            }
            else if (timeSinceLastActivity.TotalMinutes < 60)
            {
                // 10-60分钟内有活动，使用默认间隔
                intervalMs = defaultIntervalMs;
            }
            else
            {
                // ✅ 外网优化：超过60分钟无活动，使用30秒间隔（原60秒太长）
                // 说明：大多数防火墙/NAT设备在30-60秒内清理空闲连接
                intervalMs = maxIntervalMs;
            }

            //logger?.LogDebug("动态计算心跳间隔: {IntervalMs}ms (最后活动: {Minutes}分钟前)", intervalMs, timeSinceLastActivity.TotalMinutes);

            return intervalMs;
        }

        /// <summary>
        /// 批量更新用户操作信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="userInfo">用户操作信息</param>
        private void UpdateUserInfoBatch(SessionInfo sessionInfo, UserOperationInfo userInfo)
        {
            if (sessionInfo?.UserInfo == null || userInfo == null)
                return;

            // 批量赋值，减少多次属性访问
                

            sessionInfo.UserInfo.UserName = userInfo.UserName;
            sessionInfo.UserInfo.DisplayName = userInfo.DisplayName;
            sessionInfo.UserInfo.CurrentModule = userInfo.CurrentModule;
            sessionInfo.UserInfo.CurrentForm = userInfo.CurrentForm;
            sessionInfo.UserInfo.LoginTime = userInfo.LoginTime;
            sessionInfo.UserInfo.HeartbeatCount = userInfo.HeartbeatCount;
            sessionInfo.UserInfo.ClientVersion = userInfo.ClientVersion;
            sessionInfo.UserInfo.ClientIp = userInfo.ClientIp;
            
            // ✅ 关键修复：确保IdleTime正确更新（单位：秒）
            if (userInfo.IdleTime >= 0) // IdleTime可以为0，表示刚有操作
            {
                sessionInfo.UserInfo.IdleTime = userInfo.IdleTime;
            }
            
            sessionInfo.UserInfo.IsSuperUser = userInfo.IsSuperUser;
            sessionInfo.UserInfo.IsAuthorized = userInfo.IsAuthorized;
            sessionInfo.UserInfo.OperatingSystem = userInfo.OperatingSystem;
            sessionInfo.UserInfo.MachineName = userInfo.MachineName;
            sessionInfo.UserInfo.CpuInfo = userInfo.CpuInfo;
            sessionInfo.UserInfo.MemorySize = userInfo.MemorySize;
        }

        /// <summary>
        /// 创建优化的心跳响应
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="nextIntervalMs">下次间隔</param>
        /// <param name="currentTime">当前时间</param>
        /// <returns>心跳响应</returns>
        private HeartbeatResponse CreateOptimizedHeartbeatResponse(SessionInfo sessionInfo, int nextIntervalMs, DateTime currentTime)
        {
            // 检查是否可以合并响应
            var sessionId = sessionInfo?.SessionID ?? string.Empty;
            if (ShouldMergeResponse(sessionId))
            {
                // 返回缓存的响应副本，减少序列化开销
                var cachedResponse = new HeartbeatResponse
                {
                    IsSuccess = _cachedSuccessResponse.IsSuccess,
                    Status = _cachedSuccessResponse.Status,
                    ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    NextIntervalMs = nextIntervalMs,
                    ServerInfo = new Dictionary<string, object>(_cachedSuccessResponse.ServerInfo)
                };

                cachedResponse.ServerInfo["SessionId"] = sessionId;
                cachedResponse.ServerInfo["ServerTime"] = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                return cachedResponse;
            }

            // 正常创建响应
            var response = new HeartbeatResponse
            {
                IsSuccess = true,
                Status = "OK",
                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                NextIntervalMs = nextIntervalMs,
                ServerInfo = new Dictionary<string, object>
                {
                    ["ServerTime"] = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["ServerVersion"] = "1.0.0",
                    ["SessionId"] = sessionInfo.SessionID,
                    ["RecommendedInterval"] = nextIntervalMs,
                    ["ProcessingDurationMs"] = 0
                }
            };

            // 更新最后一次响应时间
            _lastHeartbeatResponses[sessionId] = currentTime;
            return response;
        }

        /// <summary>
        /// 判断是否应该合并响应
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>是否合并</returns>
        private bool ShouldMergeResponse(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                return false;

            // 使用性能监控器的动态阈值
            var mergeThreshold = _performanceMonitor?.GetMergeThresholdSeconds() ?? 5;

            if (_lastHeartbeatResponses.TryGetValue(sessionId, out var lastTime))
            {
                // 使用动态合并阈值
                return (DateTime.Now - lastTime).TotalSeconds < mergeThreshold;
            }
            return false;
        }

        /// <summary>
        /// 直接处理心跳（非批量模式）
        /// </summary>
        private async Task<IResponse> ProcessHeartbeatDirectly(
            HeartbeatRequest heartbeatRequest,
            SessionInfo sessionInfo,
            int nextIntervalMs,
            DateTime currentTime,
            CancellationToken cancellationToken)
        {
            // 创建优化的心跳响应
            var response = CreateOptimizedHeartbeatResponse(sessionInfo, nextIntervalMs, currentTime);

            // 异步更新完整会话信息（不阻塞响应）
            if (heartbeatRequest.UserOperationInfo != null)
            {
                // 使用Task.Run优化异步操作，减少堆分配
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 批量更新用户操作信息
                        UpdateUserInfoBatch(sessionInfo, heartbeatRequest.UserOperationInfo);

                        // 异步更新到SessionService
                        await SessionService.UpdateSessionAsync(sessionInfo);
                    }
                    catch (Exception ex)
                    {
                        LogError("异步更新会话信息失败", ex);
                    }
                });
            }
            else
            {
                // 如果没有用户操作信息，使用轻量级更新
                _ = Task.Run(() => SessionService.UpdateSessionLight(sessionInfo));
            }

            return response;
        }

        /// <summary>
        /// 创建立即响应（批量处理模式）
        /// </summary>
        private IResponse CreateImmediateResponse(SessionInfo sessionInfo, int nextIntervalMs, DateTime currentTime)
        {
            var response = new HeartbeatResponse
            {
                IsSuccess = true,
                Status = "BATCH_PROCESSING",
                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                NextIntervalMs = nextIntervalMs,
                ServerInfo = new Dictionary<string, object>
                {
                    ["ServerTime"] = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["ServerVersion"] = "1.0.0",
                    ["SessionId"] = sessionInfo.SessionID,
                    ["Mode"] = "BATCH",
                    ["RecommendedInterval"] = nextIntervalMs
                }
            };

            return response;
        }

        /// <summary>
        /// 判断是否应该使用批量处理
        /// </summary>
        /// <returns>是否使用批量处理</returns>
        private bool ShouldUseBatchProcessing()
        {
            // 结合性能监控数据和队列状态做出决策
            var queueStats = _batchProcessor?.GetQueueStats();
            var performanceStats = _performanceMonitor?.GetPerformanceStats();

            // 多重条件判断
            bool queueCondition = queueStats?.QueueLength > 5;
            bool performanceCondition = _performanceMonitor?.ShouldUseBatchProcessing() ?? false;
            bool loadCondition = (performanceStats?.AverageProcessingTime ?? 0) > 30;

            return queueCondition || performanceCondition || loadCondition;
        }

        /// <summary>
        /// 清理过期的心跳记录
        /// 防止_lastHeartbeatResponses字典无限增长
        /// </summary>
        private void CleanupExpiredHeartbeatResponses(object state)
        {
            try
            {
                var expiredSessions = new List<string>();
                var now = DateTime.Now;
                var expirationThreshold = TimeSpan.FromHours(1); // 1小时过期

                // 查找过期的心跳记录
                foreach (var kvp in _lastHeartbeatResponses)
                {
                    if (now - kvp.Value > expirationThreshold)
                    {
                        expiredSessions.Add(kvp.Key);
                    }
                }

                // 移除过期的记录
                foreach (var sessionId in expiredSessions)
                {
                    _lastHeartbeatResponses.TryRemove(sessionId, out _);
                }

                if (expiredSessions.Count > 0)
                {
                    Logger.LogDebug($"清理了 {expiredSessions.Count} 个过期的心跳记录");
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "清理过期心跳记录时发生异常");
            }
        }

    }
}
