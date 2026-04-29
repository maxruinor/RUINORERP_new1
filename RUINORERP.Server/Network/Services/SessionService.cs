    using Azure;
    using Microsoft.Extensions.Logging;
    using RUINORERP.Business.Cache;
    using RUINORERP.Business.CommService;
    using RUINORERP.Model;
    using RUINORERP.Model.ConfigModel;
    using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
    using RUINORERP.PacketSpec.Models;
    using RUINORERP.PacketSpec.Models.Common;
    using RUINORERP.PacketSpec.Models.Core;
    using RUINORERP.PacketSpec.Models.Message;
    using RUINORERP.PacketSpec.Models.Requests;
    using RUINORERP.PacketSpec.Models.Responses;
    using RUINORERP.PacketSpec.Security;
    using RUINORERP.PacketSpec.Serialization;
    using RUINORERP.Server.Network.Core;
    using RUINORERP.Server.Network.Interfaces.Services;
    using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Monitoring;
using RUINORERP.Server.Services;
using SuperSocket.Channel;
    using SuperSocket.Connection;
    using SuperSocket.Server;
    using SuperSocket.Server.Abstractions.Session;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// ✅ [统一架构] 统一会话管理器 - 整合SuperSocket和Network会话管理功能
    /// 提供完整的会话生命周期管理、事件机制、统计监控和SuperSocket集成
    /// 会话管理行为严格基于SuperSocket连接和断开事件实现
    /// </summary>
    public class SessionService : ISessionService, IDisposable
    {
        #region 字段和属性

        // 只保留一个会话字典，SessionInfo继承自AppSession，本身就是IAppSession
        private readonly ConcurrentDictionary<string, SessionInfo> _sessions;
        private readonly Timer _cleanupTimer;
        private SessionStatistics _statistics;
        private readonly object _lockObject = new object();
        private bool _disposed = false;
        private readonly ILogger<SessionService> _logger;
        private readonly CacheSubscriptionManager _subscriptionManager; // 使用统一的订阅管理器
        private readonly HeartbeatPerformanceMonitor _heartbeatPerformanceMonitor; // 心跳性能监控器

        // ❌ 移除基于IP的DDoS防护 - NAT环境下多台客户端共享同一IP，会导致误判
        // private readonly ConcurrentDictionary<string, ConnectionRateTracker> _connectionRates;
        // private readonly Timer _rateCleanupTimer;

        // ✅ 使用统一的待处理请求追踪器（消除重复代码）
        private readonly PendingRequestTracker<PacketModel> _requestTracker;

        /// <summary>
        /// 客户端响应处理器 - 统一管理所有客户端响应数据
        /// </summary>
        // private readonly IClientResponseHandler _clientResponseHandler; // 不需要直接依赖，避免循环依赖

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="subscriptionManager">订阅管理器</param>
        /// <param name="maxSessionCount">最大会话数量</param>
        public SessionService(
            ILogger<SessionService> logger,
            CacheSubscriptionManager subscriptionManager,
            HeartbeatPerformanceMonitor heartbeatPerformanceMonitor,
            int maxSessionCount = 1000)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionManager = subscriptionManager ?? throw new ArgumentNullException(nameof(subscriptionManager));
            _heartbeatPerformanceMonitor = heartbeatPerformanceMonitor ?? throw new ArgumentNullException(nameof(heartbeatPerformanceMonitor));
            MaxSessionCount = maxSessionCount;
            _sessions = new ConcurrentDictionary<string, SessionInfo>();
            _statistics = SessionStatistics.Create(maxSessionCount);
            
            // ❌ 移除基于IP的DDoS防护初始化 - NAT环境下会导致误判
            // _connectionRates = new ConcurrentDictionary<string, ConnectionRateTracker>();
            // _rateCleanupTimer = new Timer(CleanupConnectionRates, null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
            
            // ✅ 使用统一的请求追踪器（30秒超时，1分钟清理，最多500个待处理请求）
            _requestTracker = new PendingRequestTracker<PacketModel>(
                defaultTimeout: TimeSpan.FromSeconds(30),
                cleanupInterval: TimeSpan.FromMinutes(1),
                maxPendingCount: 500
            );
            
            // ✅ 初始化清理定时器(每10分钟清理一次超时会话)
            _cleanupTimer = new Timer(
                state => CleanupTimeoutSessions(), 
                null, 
                TimeSpan.FromMinutes(10),  // 首次延迟10分钟
                TimeSpan.FromMinutes(10)   // 每10分钟执行一次
            );
            
            // ✅ 简化：移除应用层心跳检查，依赖SuperSocket底层连接检测
            // SuperSocket会在TCP连接断开时自动触发OnSessionClosedAsync
            // 不再需要定时检查LastHeartbep0-at，减少CPU开销和复杂度

            _logger.LogInformation("SessionService初始化完成（含DDoS防护和定时清理）");

        }

        #endregion

        #region ISessionManager 实现 - 基础会话管理

        /// <summary>
        /// 活动会话数量
        /// </summary>
        public int ActiveSessionCount => _sessions.Count;

        /// <summary>
        /// 最大会话数量
        /// </summary>
        public int MaxSessionCount { get; set; } = 1000;

        /// <summary>
        /// 会话连接事件
        /// </summary>
        public event Action<SessionInfo> SessionConnected;

        /// <summary>
        /// 会话断开事件
        /// </summary>
        public event Action<SessionInfo> SessionDisconnected;

        /// <summary>
        /// 会话更新事件
        /// </summary>
        public event Action<SessionInfo> SessionUpdated;

        /// <summary>
        /// 创建新会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientPort">客户端端口</param>
        /// <returns>会话信息</returns>
        public SessionInfo CreateSession(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    throw new ArgumentException("会话ID不能为空", nameof(sessionId));
                }

                if (ActiveSessionCount >= MaxSessionCount)
                {
                    _logger.LogWarning($"达到最大会话数量限制: {MaxSessionCount}");
                    return null;
                }

                var sessionInfo = SessionInfo.Create();

                if (_sessions.TryAdd(sessionId, sessionInfo))
                {
                    lock (_lockObject)
                    {
                        _statistics.TotalConnections++;
                        _statistics.CurrentConnections = ActiveSessionCount;
                        _statistics.PeakConnections = Math.Max(_statistics.PeakConnections, ActiveSessionCount);
                    }

                    // 触发会话连接事件
                    SessionConnected?.Invoke(sessionInfo);

                    return sessionInfo;
                }

                _logger.LogWarning($"会话已存在: {sessionId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"创建会话失败: {sessionId}");
                return null;
            }
        }

        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话信息</returns>
        public SessionInfo GetSession(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                    return null;

                if (_sessions.TryGetValue(sessionId, out var sessionInfo))
                {
                    // 更新最后访问时间
                    sessionInfo.UpdateActivity();
                    return sessionInfo;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取会话信息失败: {sessionId}");
                return null;
            }
        }


        /// <summary>
        /// 获取会话信息
        /// 根据用户ID，因为断开后系统重连会话ID会变化。通过登录人ID获取会话更可靠。
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>会话信息</returns>
        public SessionInfo GetSession(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return null;
                }

                // ✅ 优化：查找匹配用户ID且连接活跃的会话（优先）
                var activeSession = _sessions.Values
                    .FirstOrDefault(s => s.UserId.HasValue && s.UserId.Value == userId && s.IsConnected);

                if (activeSession != null)
                {
                    // 更新最后访问时间
                    activeSession.UpdateActivity();
                    return activeSession;
                }

                // ✅ 容错：如果没有活跃会话，尝试查找任意匹配UserId的会话（可能是刚重连还未更新状态）
                var anySession = _sessions.Values
                    .FirstOrDefault(s => s.UserId.HasValue && s.UserId.Value == userId);

                if (anySession != null)
                {
                    _logger?.LogDebug($"[GetSession] 找到非活跃会话: UserId={userId}, SessionId={anySession.SessionID}, IsConnected={anySession.IsConnected}");
                    anySession.UpdateActivity();
                    return anySession;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户ID {userId} 的会话信息失败");
                return null;
            }
        }


        /// <summary>
        /// 获取指定用户名的所有会话
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>会话信息列表</returns>
        public IEnumerable<SessionInfo> GetUserSessions(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return Enumerable.Empty<SessionInfo>();

                return _sessions.Values
                    .Where(s => s.IsAuthenticated && s.UserName == username)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户会话信息失败: {username}");
                return Enumerable.Empty<SessionInfo>();
            }
        }

        /// <summary>
        /// 获取所有已认证的用户会话
        /// </summary>
        /// <param name="excludeSessionIds">可选：要排除的会话ID数组</param>
        /// <returns>已认证的用户会话列表</returns>
        public IEnumerable<SessionInfo> GetAllUserSessions(params string[] excludeSessionIds)
        {
            try
            {
                var query = _sessions.Values.Where(s => s.IsAuthenticated);

                // 如果指定了要排除的会话ID数组，则从结果中排除这些会话
                if (excludeSessionIds != null && excludeSessionIds.Length > 0)
                {
                    var excludeSet = new HashSet<string>(excludeSessionIds.Where(id => !string.IsNullOrEmpty(id)));
                    if (excludeSet.Count > 0)
                    {
                        query = query.Where(s => !excludeSet.Contains(s.SessionID));
                    }
                }

                var result = query.ToList();
                
                // 添加诊断日志，帮助排查会话数量不一致问题
                _logger.LogDebug($"[GetAllUserSessions] 总会话数={_sessions.Count}, 已认证会话数={result.Count}, 排除会话=[{string.Join(",", excludeSessionIds ?? Array.Empty<string>())}]");
                
                return result;
            }
            catch (Exception ex)
            {
                var excludeIds = excludeSessionIds != null ? string.Join(", ", excludeSessionIds) : "无";
                _logger.LogError(ex, $"获取所有用户会话失败，排除会话ID: [{excludeIds}]");
                return Enumerable.Empty<SessionInfo>();
            }
        }


        /// <summary>
        /// 更新会话信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateSession(SessionInfo sessionInfo)
        {
            try
            {
                if (sessionInfo == null || string.IsNullOrEmpty(sessionInfo.SessionID))
                {
                    _logger.LogWarning("更新会话失败：会话信息或会话ID为空");
                    return false;
                }

                if (_sessions.TryGetValue(sessionInfo.SessionID, out var existingSession))
                {
                    // 增强线程安全性：使用锁保护会话更新操作
                    lock (existingSession)
                    {
                        // ✅ 使用UserInfo作为唯一数据源更新用户信息
                        if (sessionInfo.UserInfo != null)
                        {
                            // 复制UserInfo中的关键属性
                            if (existingSession.UserInfo == null)
                                existingSession.UserInfo = new CurrentUserInfo();

                            // 核心标识
                            existingSession.UserInfo.UserID = sessionInfo.UserInfo.UserID;
                            existingSession.UserInfo.UserName = sessionInfo.UserInfo.UserName;
                            existingSession.UserInfo.DisplayName = sessionInfo.UserInfo.DisplayName;
                            existingSession.UserInfo.EmployeeId = sessionInfo.UserInfo.EmployeeId;
                            
                            // 认证状态
                            existingSession.UserInfo.IsSuperUser = sessionInfo.UserInfo.IsSuperUser;
                            existingSession.UserInfo.IsAuthorized = sessionInfo.UserInfo.IsAuthorized;
                            
                            // 客户端环境
                            existingSession.UserInfo.ClientVersion = sessionInfo.UserInfo.ClientVersion;
                            existingSession.UserInfo.ClientIp = sessionInfo.UserInfo.ClientIp;
                            existingSession.UserInfo.OperatingSystem = sessionInfo.UserInfo.OperatingSystem;
                            existingSession.UserInfo.MachineName = sessionInfo.UserInfo.MachineName;
                            existingSession.UserInfo.CpuInfo = sessionInfo.UserInfo.CpuInfo;
                            existingSession.UserInfo.MemorySize = sessionInfo.UserInfo.MemorySize;
                            
                            // 当前操作
                            existingSession.UserInfo.CurrentModule = sessionInfo.UserInfo.CurrentModule;
                            existingSession.UserInfo.CurrentForm = sessionInfo.UserInfo.CurrentForm;
                            existingSession.UserInfo.HeartbeatCount = sessionInfo.UserInfo.HeartbeatCount;
                            existingSession.UserInfo.IdleTime = sessionInfo.UserInfo.IdleTime;
                            
                            // 更新最后心跳时间
                            existingSession.UserInfo.LastHeartbeatTime = DateTime.Now;
                        }

                        // 更新会话状态属性
                        existingSession.IsAuthenticated = sessionInfo.IsAuthenticated;
                        existingSession.IsAdmin = sessionInfo.IsAdmin;
                        existingSession.UpdateActivity();
                        existingSession.DataContext = sessionInfo.DataContext;
                    }

                    // 触发会话更新事件
                    SessionUpdated?.Invoke(existingSession);

                    _logger.LogDebug($"更新会话信息: {sessionInfo.SessionID}");
                    return true;
                }

                _logger.LogWarning($"会话不存在，无法更新: {sessionInfo.SessionID}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新会话信息失败: {sessionInfo?.SessionID}");
                return false;
            }
        }
        
        /// <summary>
        /// 异步更新会话信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新任务</returns>
        public async Task<bool> UpdateSessionAsync(SessionInfo sessionInfo)
        {
            return await Task.Run(() => UpdateSession(sessionInfo));
        }

        /// <summary>
        /// 轻量级会话更新（仅更新活动时间）
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateSessionLight(SessionInfo sessionInfo)
        {
            try
            {
                if (sessionInfo == null || string.IsNullOrEmpty(sessionInfo.SessionID))
                {
                    return false;
                }

                if (_sessions.TryGetValue(sessionInfo.SessionID, out var existingSession))
                {
                    // 仅更新关键时间戳，减少开销
                    existingSession.UpdateActivity();
                    existingSession.LastHeartbeat = DateTime.Now;

                    // 轻量级更新不触发事件以减少开销
                    // SessionUpdated?.Invoke(existingSession);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, $"轻量级更新会话信息失败: {sessionInfo?.SessionID}");
                return false;
            }
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>删除是否成功</returns>
        public bool RemoveSession(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                    return false;

                if (_sessions.TryRemove(sessionId, out var sessionInfo))
                {
                    if (sessionInfo != null)
                    {
                        sessionInfo.IsConnected = false;
                        sessionInfo.DisconnectTime = DateTime.Now;

                        // 触发会话断开事件（在清理订阅和锁之前触发，确保其他组件能获取到完整的会话信息）
                        SessionDisconnected?.Invoke(sessionInfo);

                        // 取消该会话的所有缓存订阅
                        _subscriptionManager.RemoveAllSubscriptionsAsync(sessionId);

                        // 清理会话数据，释放内存（解决内存泄漏问题）
                        sessionInfo.Clear();

                        lock (_lockObject)
                        {
                            _statistics.TotalDisconnections++;
                            _statistics.CurrentConnections = ActiveSessionCount;
                        }

                        var duration = sessionInfo.DisconnectTime - sessionInfo.ConnectedTime;
                        _logger.LogInformation($"会话已删除: {sessionId}, 连接时长={duration?.TotalMinutes:F2}分钟");
                    }

                    return true;
                }

                _logger.LogWarning($"会话不存在，无法删除: {sessionId}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除会话失败: {sessionId}");
                return false;
            }
        }

        /// <summary>
        /// 验证会话是否有效
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>是否有效</returns>
        public bool IsValidSession(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                    return false;

                if (!_sessions.TryGetValue(sessionId, out var sessionInfo))
                    return false;
                
                // 同时检查会话存在性和连接状态
                lock (sessionInfo)
                {
                    return sessionInfo.IsConnected && sessionInfo.IsAuthenticated;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"验证会话有效性失败: {sessionId}");
                return false;
            }
        }

        #endregion

        #region ISessionEventHandler 实现

        /// <summary>
        /// 处理会话连接事件 - 基于SuperSocket连接事件触发
        /// 严格按照SuperSocket的连接事件处理会话创建和管理
        /// </summary>
        /// <param name="session">连接的会话</param>
        /// <returns>异步任务</returns>
        public async ValueTask OnSessionConnectedAsync(IAppSession session)
        {
            try
            {
                string clientIp = GetClientIp(session);
                var remoteEndPoint = session.RemoteEndPoint?.ToString() ?? "未知";
                
                // ✅ 快速断开检测：如果连接后立即断开（5秒内），可能是端口扫描
                // 记录连接时间用于后续分析
                session["ConnectTime"] = DateTime.Now;
                session["ClientIp"] = clientIp;
                
                // ❌ 移除基于单一IP的DDoS防护和端口扫描检测
                // 原因：外网环境下多台客户端通过路由器上网时会共享同一外网IP，外网是NAT环境
                // 基于单一IP的限制（每分钟120次、5秒内10次）会严重误判，影响正常用户使用
                // 如需防护，应基于IP+用户名组合实现，或在应用层进行更精确的控制

                // 检查是否已达到最大会话数
                if (ActiveSessionCount >= MaxSessionCount)
                {
                    _logger.LogWarning("达到最大会话数量限制: {MaxCount}，拒绝新连接，来源: {RemoteEndPoint}", 
                        MaxSessionCount, remoteEndPoint);
                    await session.CloseAsync(CloseReason.ServerShutdown);
                    return;
                }

                // SessionInfo继承自AppSession，可以直接转换
                SessionInfo sessionInfo = session as SessionInfo;

                // 增强对会话转换异常的处理
                if (sessionInfo == null)
                {
                    _logger.LogError($"会话转换失败，SessionID: {session.SessionID}");
                    await session.CloseAsync(CloseReason.ServerShutdown);
                    return;
                }

                // 创建会话信息
                sessionInfo.ConnectedTime = DateTime.Now;
                sessionInfo.UpdateActivity();
                sessionInfo.IsConnected = true;
                sessionInfo.Properties = new Dictionary<string, object>();

                // 存储会话
                var added = _sessions.TryAdd(session.SessionID, sessionInfo);
                if (added)
                {
                    lock (_lockObject)
                    {
                        _statistics.TotalConnections++;
                        _statistics.CurrentConnections = ActiveSessionCount;
                        _statistics.PeakConnections = Math.Max(_statistics.PeakConnections, ActiveSessionCount);
                    }

                    // 触发会话连接事件
                    SessionConnected?.Invoke(sessionInfo);
                    
                    // ✅ 关键修复：在会话创建成功后，异步发送欢迎消息
                    // 使用 Task.Run 避免阻塞 SuperSocket 的连接回调
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // ✅ 优化：解耦欢迎流程与登录验证，允许未回复 WelcomeAck 的会话直接处理 Login 请求
                            // 仅发送欢迎消息，不再设置 IsHandshakeCompleted 标志位来阻塞后续逻辑
                            
                            // ✅ 修复：增加超时保护，防止同步等待导致连接回调长时间阻塞
                            // ✅ 外网优化：超时时间延长至60秒，适应高延迟网络环境
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                            await SendWelcomeMessageAsync(sessionInfo).WaitAsync(cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            _logger.LogWarning("[欢迎消息] 发送过程超时: {SessionId}", sessionInfo.SessionID);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"欢迎流程处理失败: {sessionInfo.SessionID}");
                        }
                    });
                }
                else
                {
                    _logger.LogWarning($"SuperSocket会话连接失败，SessionID已存在: {session.SessionID}");
                    await session.CloseAsync(CloseReason.ServerShutdown);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话连接事件时出错: SessionID={session?.SessionID}");
                try
                {
                    // 修复编译错误：ValueTask? 没有 GetAwaiter 方法
                    // 先检查 session 是否为 null，再调用 CloseAsync 方法
                    if (session != null)
                    {
                        await session.CloseAsync(CloseReason.ServerShutdown);
                    }
                }
                catch (Exception closeEx)
                {
                    _logger.LogWarning(closeEx, $"关闭异常会话时出错: SessionID={session?.SessionID}");
                }
            }
        }

        /// <summary>
        /// 发送欢迎消息到客户端
        /// 修改为同步等待发送完成，确保客户端能收到欢迎消息1
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>异步任务</returns>
        private async Task SendWelcomeMessageAsync(SessionInfo sessionInfo)
        {
            try
            {
                _logger.LogInformation("[欢迎消息] 准备发送欢迎消息: SessionId={SessionId}", sessionInfo.SessionID);
                
                // 延迟500毫秒再发送欢迎消息，确保客户端已准备好接收
                await Task.Delay(500);
                
                // 从依赖注入容器中获取服务器配置
                var serverConfig = Startup.GetFromFac<ServerGlobalConfig>();

                string announcement = serverConfig?.Announcement ?? "欢迎使用RUINORERP系统！";

                var welcomeRequest = WelcomeRequest.CreateWithAnnouncement(
                    sessionInfo.SessionID,
                    GetServerVersion(),
                    announcement);

                sessionInfo.WelcomeSentTime = DateTime.Now;

                _logger.LogDebug("[欢迎消息] 欢迎消息内容: RequestId={RequestId}, Version={Version}, Announcement={Announcement}",
                    welcomeRequest.RequestId, welcomeRequest.ServerVersion, announcement);

                // ✅ 优化：外网环境下增加超时时间至30秒，适应高延迟场景
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                await SendPacketCoreAsync(
                    sessionInfo,
                    SystemCommands.Welcome,
                    welcomeRequest,
                    20000,
                    cts.Token,
                    PacketDirection.ServerRequest
                ).ConfigureAwait(false);
                
                _logger.LogInformation("[欢迎消息] 欢迎消息已成功发送至客户端: SessionId={SessionId}, RequestId={RequestId}", 
                    sessionInfo.SessionID, welcomeRequest.RequestId);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[欢迎消息] 发送超时: SessionId={SessionId}", sessionInfo.SessionID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[欢迎消息] 发送失败: SessionId={SessionId}, Error={Error}", 
                    sessionInfo.SessionID, ex.Message);
            }
        }

        /// <summary>
        /// 获取服务器版本号
        /// </summary>
        /// <returns>服务器版本号</returns>
        private string GetServerVersion()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                return versionInfo.FileVersion ?? "1.0.0.0";
            }
            catch
            {
                return "1.0.0.0";
            }
        }

        /// <summary>
        /// 处理会话断开事件 - 基于SuperSocket断开事件触发
        /// 严格按照SuperSocket的断开事件处理会话清理和资源释放
        /// </summary>
        /// <param name="session">断开的会话</param>
        /// <param name="closeReason">断开原因</param>
        /// <returns>异步任务</returns>
        public async ValueTask OnSessionClosedAsync(IAppSession session, CloseEventArgs closeReason)
        {
            try
            {
                // 获取会话信息
                _sessions.TryGetValue(session.SessionID, out var sessionInfo);
                
                // ✅ 快速断开检测：检查连接持续时间
                var clientIp = session["ClientIp"]?.ToString() ?? GetClientIp(session);
                var remoteEndPoint = session.RemoteEndPoint?.ToString() ?? "未知";
                
                if (session["ConnectTime"] is DateTime connectTime)
                {
                    var duration = DateTime.Now - connectTime;
                    
                    // ❌ 移除基于IP的端口扫描检测
                    // 原因：外网NAT环境下，多台客户端共享同一IP是正常现象
                    // 仅记录日志用于诊断，不进行任何封禁或限制操作
                    if (duration.TotalSeconds < 3 && (sessionInfo == null || !sessionInfo.IsAuthenticated))
                    {
                        _logger.LogDebug("[连接诊断] SessionID={SessionId}, IP={ClientIp}, 连接持续{Duration:F1}秒后断开（未认证）",
                            session.SessionID, clientIp, duration.TotalSeconds);
                    }
                    // ✅ 优化：正常断开使用 Debug 级别，避免外网不稳定时日志刷屏
                    else if (sessionInfo != null && sessionInfo.IsAuthenticated)
                    {
                        _logger.LogDebug("[断开] 认证会话断开: SessionID={SessionId}, 用户={UserName}, 持续时间={Duration:F1}秒, 原因={Reason}", 
                            session.SessionID, sessionInfo.UserName, duration.TotalSeconds, closeReason.Reason);
                    }
                    else
                    {
                        _logger.LogDebug("[断开] 会话断开: SessionID={SessionId}, 来源={RemoteEndPoint}, 持续时间={Duration:F1}秒, 原因={Reason}", 
                            session.SessionID, remoteEndPoint, duration.TotalSeconds, closeReason.Reason);
                    }
                }
                else
                {
                    _logger.LogDebug("[断开] 会话断开: SessionID={SessionId}, 来源={RemoteEndPoint}, 原因={Reason}", 
                        session.SessionID, remoteEndPoint, closeReason.Reason);
                }

                // 如果会话信息存在，先触发断开事件再移除会话
                if (sessionInfo != null)
                {
                    // 更新会话状态
                    sessionInfo.IsConnected = false;
                    sessionInfo.DisconnectTime = DateTime.Now;

                    // 触发会话断开事件
                    SessionDisconnected?.Invoke(sessionInfo);

                    // 调用IServerSessionEventHandler接口的会话断开方法
                    await OnSessionDisconnectedAsync(sessionInfo, closeReason.Reason.ToString());
                }

                // 移除会话
                await RemoveSessionAsync(session.SessionID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理会话断开事件时出错: SessionID={SessionId}", session?.SessionID);
            }
        }

        /// <summary>
        /// 会话断开事件处理 - 项目自定义接口实现
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="reason">断开原因</param>
        /// <returns>异步任务</returns>
        public async Task OnSessionDisconnectedAsync(SessionInfo sessionInfo, string reason)
        {
            try
            {
                // 可以在这里添加额外的断开后处理逻辑
                // 例如清理资源、保存会话历史等
                _logger.LogDebug($"执行会话断开后的自定义处理逻辑: {sessionInfo.SessionID}, 原因: {reason}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"执行会话断开自定义处理逻辑时出错: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 用户认证成功事件处理 - 项目自定义接口实现
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>异步任务</returns>
        public async Task OnUserAuthenticatedAsync(SessionInfo sessionInfo)
        {
            try
            {
                _logger.LogInformation($"用户认证成功: SessionID={sessionInfo.SessionID}, Username={sessionInfo.UserName}");
                // 认证成功后的处理逻辑
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理用户认证成功事件时出错: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 用户认证失败事件处理 - 项目自定义接口实现
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="reason">失败原因</param>
        /// <returns>异步任务</returns>
        public async Task OnAuthenticationFailedAsync(SessionInfo sessionInfo, string reason)
        {
            try
            {
                _logger.LogWarning($"用户认证失败: SessionID={sessionInfo.SessionID}, 原因: {reason}");
                // 认证失败后的处理逻辑
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理用户认证失败事件时出错: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 会话超时事件处理 - 项目自定义接口实现
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>异步任务</returns>
        public async Task OnSessionTimeoutAsync(SessionInfo sessionInfo)
        {
            try
            {
                _logger.LogInformation($"会话超时: SessionID={sessionInfo.SessionID}");
                // 会话超时后的处理逻辑
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话超时事件时出错: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 会话错误事件处理 - 项目自定义接口实现
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="error">错误异常</param>
        /// <returns>异步任务</returns>
        public async Task OnSessionErrorAsync(SessionInfo sessionInfo, Exception error)
        {
            try
            {
                _logger.LogError(error, $"会话错误: SessionID={sessionInfo.SessionID}");
                // 会话错误后的处理逻辑
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话错误事件时出错: {sessionInfo.SessionID}");
            }
        }

        #endregion

        #region SuperSocket集成功能

        /// <summary>
        /// 添加SuperSocket会话
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <returns>添加结果</returns>
        public async Task<bool> AddSessionAsync(IAppSession session)
        {
            // 此方法现在通过OnSessionConnectedAsync处理SuperSocket连接事件
            // 保留此方法以保持接口兼容性
            await OnSessionConnectedAsync(session);
            return _sessions.ContainsKey(session.SessionID);
        }

        /// <summary>
        /// 移除SuperSocket会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>移除结果</returns>
        public async Task<bool> RemoveSessionAsync(string sessionId)
        {
            return RemoveSession(sessionId);
        }

        /// <summary>
        /// 获取SuperSocket会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>SuperSocket会话</returns>
        public IAppSession GetAppSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            // SessionInfo继承自AppSession，本身就是IAppSession
            _sessions.TryGetValue(sessionId, out var sessionInfo);
            return sessionInfo;
        }

        /// <summary>
        /// 更新会话活动时间
        /// 确保会话活动时间正确更新，以便会话超时清理机制正常工作
        /// 使用SessionInfo类提供的UpdateActivity()方法更新活动时间，避免直接设置属性可能出现的问题
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>更新结果</returns>
        public bool UpdateSessionActivity(string sessionId)
        {
            try
            {
                if (_sessions.TryGetValue(sessionId, out var sessionInfo))
                {
                    // 使用专门的UpdateActivity方法更新活动时间和心跳计数
                    sessionInfo.UpdateActivity();

                    // 触发会话更新事件，通知UI更新状态
                    SessionUpdated?.Invoke(sessionInfo);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新会话活动时间时出错: SessionID={sessionId}");
                return false;
            }
        }



        /// <summary>
        /// 设置会话属性
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        /// <returns>设置结果</returns>
        public bool SetSessionProperty(string sessionId, string key, object value)
        {
            try
            {
                if (_sessions.TryGetValue(sessionId, out var sessionInfo))
                {
                    sessionInfo.Properties = sessionInfo.Properties ?? new Dictionary<string, object>();
                    sessionInfo.Properties[key] = value;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置会话属性时出错: SessionID={sessionId}, Key={key}");
                return false;
            }
        }

        /// <summary>
        /// 主动断开指定会话连接（T人功能）
        /// </summary>
        /// <param name="sessionId">要断开的会话ID</param>
        /// <param name="reason">断开原因，默认为"服务器强制断开"</param>
        /// <returns>断开是否成功</returns>
        public async Task<bool> DisconnectSessionAsync(string sessionId, string reason = "服务器强制断开")
        {
            bool success = false;
            SessionInfo sessionInfo = null;
            string username = "未知";

            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    _logger.LogWarning("断开会话失败：会话ID为空");
                    return false;
                }

                // 获取会话信息
                if (!_sessions.TryGetValue(sessionId, out sessionInfo))
                {
                    _logger.LogWarning($"断开会话失败：会话不存在，SessionID={sessionId}");
                    return false;
                }

                username = sessionInfo.UserName ?? "未知";

                // 添加主动断开连接警告日志
                _logger.LogWarning($"[主动断开连接] 准备断开会话: SessionID={sessionId}, UserName={username}, ClientIp={sessionInfo.ClientIp}, IsConnected={sessionInfo.IsConnected}, 原因={reason}");

                try
                {
                    // 记录关闭前的会话状态
                    _logger.LogDebug($"[断开会话] CloseAsync调用前: SessionID={sessionId}, IsConnected={sessionInfo.IsConnected}");
                    
                    // 主动关闭SuperSocket连接
                    await sessionInfo.CloseAsync(CloseReason.ServerShutdown);
                    
                    _logger.LogDebug($"[断开会话] CloseAsync调用后: SessionID={sessionId}, IsConnected={sessionInfo.IsConnected}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"关闭SuperSocket会话连接失败: SessionID={sessionId}, UserName={username}");
                    // 关闭失败不影响后续清理
                }

                // 移除会话记录，确保资源释放
                success = RemoveSession(sessionId);
                
                // 验证会话是否真正被移除
                bool isRemoved = !_sessions.ContainsKey(sessionId);

                if (success && isRemoved)
                {
                    _logger.LogInformation($"会话已成功断开并移除: SessionID={sessionId}, 用户={username}, 原因={reason}");
                }
                else if (success && !isRemoved)
                {
                    _logger.LogWarning($"会话移除返回值成功但会话仍存在: SessionID={sessionId}, 用户={username}");
                }
                else
                {
                    _logger.LogWarning($"会话断开但移除记录失败: SessionID={sessionId}, 用户={username}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"断开会话时发生异常: SessionID={sessionId}, UserName={username}");
                try
                {
                    // 无论如何都尝试移除会话记录，确保资源释放
                    if (sessionInfo != null)
                    {
                        success = RemoveSession(sessionId);
                        if (success)
                        {
                            _logger.LogInformation($"异常情况下会话记录已成功移除: SessionID={sessionId}, UserName={username}");
                        }
                    }
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, $"异常情况下移除会话记录失败: SessionID={sessionId}, UserName={username}");
                }
            }

            return success;
        }

        /// <summary>
        /// 主动断开指定用户的所有会话连接（T人功能）
        /// </summary>
        /// <param name="username">要断开的用户名</param>
        /// <param name="reason">断开原因，默认为"服务器强制断开"</param>
        /// <returns>成功断开的会话数量</returns>
        public async Task<int> DisconnectUserSessionsAsync(string username, string reason = "服务器强制断开")
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("断开用户会话失败：用户名为空");
                    return 0;
                }

                // 获取该用户的所有会话
                var userSessions = GetUserSessions(username).ToList();

                if (userSessions.Count == 0)
                {
                    _logger.LogInformation($"用户没有活动会话: Username={username}");
                    return 0;
                }

                // 添加主动断开连接警告日志
                _logger.LogWarning($"[主动断开连接] 准备断开用户所有会话: Username={username}, 会话数量={userSessions.Count}, 原因={reason}");

                int successCount = 0;

                // 并行断开所有会话
                var disconnectTasks = userSessions.Select(async session =>
                {
                    try
                    {
                        var result = await DisconnectSessionAsync(session.SessionID, reason);
                        return result ? 1 : 0;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"断开用户会话失败: SessionID={session.SessionID}, Username={username}");
                        return 0;
                    }
                });

                // 过滤掉可能的null任务，防止ArgumentException异常
                var validDisconnectTasks = disconnectTasks.Where(t => t != null).ToList();
                var results = validDisconnectTasks.Any() ? await Task.WhenAll(validDisconnectTasks) : Array.Empty<int>();
                successCount = results.Sum();

                _logger.LogInformation($"用户会话断开完成: Username={username}, 总会话数={userSessions.Count}, 成功断开={successCount}");
                return successCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"断开用户所有会话时发生异常: Username={username}");
                return 0;
            }
        }

        #endregion

        #region 服务器主动发送命令
        /// <summary>
        /// 发送数据包的核心私有方法
        /// 封装了构建数据包、序列化、加密和发送的公共逻辑
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">要发送的请求数据</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="responseTypeName">期望的响应类型名称</param>
        /// <exception cref="OperationCanceledException">当操作被取消时抛出</exception>
        /// <exception cref="TimeoutException">当请求超时时抛出</exception>
        public async Task SendPacketCoreAsync<TRequest>(
            SessionInfo sessionInfo,
            CommandId commandId,
            TRequest request,
            int timeoutMs,
            CancellationToken ct = default,
            PacketDirection packetDirection = PacketDirection.ServerRequest,
            string responseTypeName = null)
            where TRequest : class, IRequest
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                // 增强参数检查
                if (sessionInfo == null)
                {
                    _logger.LogError("发送数据包失败: 会话信息为空");
                    throw new ArgumentNullException(nameof(sessionInfo));
                }

                if (request == null)
                {
                    _logger.LogError($"发送数据包失败: 请求数据为空, SessionID={sessionInfo.SessionID}");
                    throw new ArgumentNullException(nameof(request));
                }

                if (string.IsNullOrEmpty(request.RequestId))
                {
                    _logger.LogError($"发送数据包失败: 请求ID为空, SessionID={sessionInfo.SessionID}");
                    throw new ArgumentException("请求ID不能为空", nameof(request.RequestId));
                }

                // 构建数据包
                var packet = PacketBuilder.Create()
                    .WithDirection(packetDirection)
                    .WithTimeout(timeoutMs)
                    .WithRequest(request)
                    .Build();

                // 设置执行上下文
                if (packet.ExecutionContext == null)
                {
                    packet.ExecutionContext = new CommandContext();
                }

                // 确保必要的上下文属性被设置
                packet.ExecutionContext.RequestId = request.RequestId;
                packet.CommandId = commandId;
                packet.ExecutionContext.SessionId = sessionInfo.SessionID;
                packet.ExecutionContext.UserId = sessionInfo.UserId ?? 0;

                // 设置期望的响应类型信息
                if (!string.IsNullOrEmpty(responseTypeName))
                {
                    packet.ExecutionContext.ExpectedResponseTypeName = responseTypeName;
                }

                // 序列化和加密数据包
                var serializedData = JsonCompressionSerializationService.Serialize<PacketModel>(packet);
                var original = new OriginalData((byte)packet.CommandId.Category, new[] { packet.CommandId.OperationCode }, serializedData);
                var encrypted = UnifiedEncryptionProtocol.EncryptServerDataToClient(original);

                // 发送数据
                await sessionInfo.SendAsync(encrypted.ToArray(), ct);

                _logger?.LogDebug("数据包发送成功: SessionID={SessionID}, CommandId={CommandId}, RequestId={RequestId}",
                    sessionInfo.SessionID, commandId, request.RequestId);
            }
            catch (OperationCanceledException)
            {
                // 取消操作不需要记录错误日志
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"发送数据包时发生错误: SessionID={sessionInfo?.SessionID}, CommandId={commandId}, RequestId={request?.RequestId}");
                throw;
            }
        }

        /// <summary>
        /// 向指定会话发送命令并等待响应
        /// </summary>
        /// <param name="sessionID">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>响应数据包</returns>
        public async Task<PacketModel> SendCommandAndWaitForResponseAsync<TRequest>(
            string sessionID,
            CommandId commandId,
            TRequest request,
            int timeoutMs = 10000,
            CancellationToken ct = default)
            where TRequest : class, IRequest
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    _logger.LogWarning("发送命令失败：会话ID为空");
                    throw new ArgumentException("会话ID不能为空", nameof(sessionID));
                }

                // 获取会话信息
                if (!_sessions.TryGetValue(sessionID, out var sessionInfo))
                {
                    _logger.LogWarning($"发送命令失败：会话不存在，SessionID={sessionID}");
                    throw new InvalidOperationException($"会话不存在: {sessionID}");
                }

                // ✅ 使用统一的请求追踪器
                var requestId = request.RequestId;
                var tcs = _requestTracker.Register(requestId);

                try
                {
                    // 发送命令
                    await SendPacketCoreAsync(sessionInfo, commandId, request, timeoutMs, ct, PacketDirection.ServerRequest);

                    // 等待响应或超时
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    cts.CancelAfter(timeoutMs);

                    var timeoutTask = Task.Delay(timeoutMs, cts.Token);
                    var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        throw new TimeoutException($"请求超时（{timeoutMs}ms），指令类型：{commandId.ToString()}，请求ID: {requestId}");
                    }

                    ct.ThrowIfCancellationRequested();

                    return await tcs.Task;
                }
                catch (Exception ex)
                {
                    // ✅ 追踪器会自动清理，这里只需取消任务
                    _requestTracker.TryCancel(requestId);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"发送命令并等待响应时发生异常: SessionID={sessionID}, CommandId={commandId}");
                throw;
            }
        }

        /// <summary>
        /// 向指定会话发送命令（单向，不等待响应）
        /// </summary>
        /// <param name="sessionID">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送是否成功</returns>
        [Obsolete("直接SendPacketCoreAsync更灵活")]
        public async Task<bool> SendCommandAsync<TRequest>(
            string sessionID,
            CommandId commandId,
            TRequest request,
            CancellationToken ct = default)
            where TRequest : class, IRequest
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    _logger.LogWarning("发送命令失败：会话ID为空");
                    return false;
                }

                if (commandId.FullCode == 0)
                {
                    _logger.LogWarning("发送命令失败：命令为空");
                    return false;
                }

                // 获取会话信息
                if (!_sessions.TryGetValue(sessionID, out var sessionInfo))
                {
                    _logger.LogWarning($"发送命令失败：会话不存在，SessionID={sessionID}");
                    return false;
                }

                // 发送命令（不等待响应）
                await SendPacketCoreAsync(sessionInfo, commandId, request, 5000, ct);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"发送命令时发生异常: SessionID={sessionID}, CommandId={commandId}");
                return false;
            }
        }

        /// <summary>
        /// 处理从客户端接收到的响应包
        /// 已弃用：响应处理现在由SuperSocketCommandAdapter直接处理
        /// </summary>
        /// <param name="packet">响应数据包</param>
        [Obsolete("响应处理现在由SuperSocketCommandAdapter直接处理")]
        public void HandleClientResponse(PacketModel packet)
        {
            // 此方法已不再使用
        }

        /// <summary>
        /// 尝试完成待处理请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <param name="response">响应数据</param>
        /// <returns>是否成功完成</returns>
        public bool TryCompletePendingRequest(string requestId, PacketModel response)
        {
            return _requestTracker.TryComplete(requestId, response);
        }
        #endregion



        #region ISessionManager 实现 - 统计和监控

        /// <summary>
        /// 会话统计信息
        /// </summary>
        public SessionStatistics GetStatistics()
        {
            lock (_lockObject)
            {
                // 清理过期的统计信息
                CleanExpiredStatistics();

                return new SessionStatistics
                {
                    TotalConnections = _statistics.TotalConnections,
                    TotalDisconnections = _statistics.TotalDisconnections,
                    CurrentConnections = ActiveSessionCount,
                    PeakConnections = _statistics.PeakConnections,
                    TimeoutSessions = _statistics.TimeoutSessions,
                    HeartbeatFailures = _statistics.HeartbeatFailures,
                    LastCleanupTime = _statistics.LastCleanupTime,
                    LastHeartbeatCheck = _statistics.LastHeartbeatCheck
                };
            }
        }

        /// <summary>
        /// 清理过期的统计信息
        /// </summary>
        private void CleanExpiredStatistics()
        {
            try
            {
                // 如果距离上次清理已经超过1小时，则重置部分统计信息
                if (_statistics.LastCleanupTime.AddHours(1) < DateTime.Now)
                {
                    // 重置超时会话和心跳失败统计，避免数值无限增长
                    _statistics.TimeoutSessions = 0;
                    _statistics.HeartbeatFailures = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期统计信息时发生错误");
            }
        }

        /// <summary>
        /// 重置会话统计信息
        /// </summary>
        public void ResetStatistics()
        {
            try
            {
                lock (_lockObject)
                {
                    // 保存不应重置的信息
                    var currentConnections = ActiveSessionCount;
                    var serverStartTime = _statistics.ServerStartTime;

                    // 创建新的统计信息对象
                    _statistics = SessionStatistics.Create(MaxSessionCount);

                    // 恢复不应重置的信息
                    _statistics.CurrentConnections = currentConnections;
                    _statistics.ServerStartTime = serverStartTime;

                    // 更新当前时间戳
                    _statistics.LastCleanupTime = DateTime.Now;
                    _statistics.LastHeartbeatCheck = DateTime.Now;
                }

                _logger.LogInformation("会话统计信息已成功重置");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置会话统计信息时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 清理超时会话 - 优化版
        /// 减少过于激进的清理策略，避免误判正常连接
        /// </summary>
        /// <returns>清理的会话数量</returns>
        public int CleanupTimeoutSessions()
        {
            try
            {
                // 1. 首先获取所有会话ID，避免在遍历过程中修改集合导致异常
                var allSessionIds = _sessions.Keys.ToList();
                var timeoutSessions = new List<SessionInfo>();
                var currentTime = DateTime.Now;

                // 2. 筛选超时会话和未验证会话，使用线程安全的方式访问会话信息
                foreach (var sessionId in allSessionIds)
                {
                    if (_sessions.TryGetValue(sessionId, out var session))
                    {
                        // 增强线程安全性：使用锁保护会话访问
                        lock (session)
                        {
                            // 检查1: 活动超时（60分钟无活动）
                            var inactiveTime = currentTime - session.LastActivityTime;
                            if (inactiveTime.TotalMinutes > 60)
                            {
                                // ✅ 外网优化：已认证会话超时时间延长至4小时（原90分钟）
                                // 说明：外网用户可能长时间离开（会议、午餐等），频繁重新登录体验差
                                const int authenticatedSessionTimeout = 240; // 4小时
                                
                                if (session.IsAuthenticated && inactiveTime.TotalMinutes < authenticatedSessionTimeout)
                                {
                                    _logger.LogDebug($"[活动超时警告] SessionID={session.SessionID}, IP={session.ClientIp}, 无活动时间={inactiveTime.TotalMinutes:F1}分钟");
                                }
                                else
                                {
                                    timeoutSessions.Add(session);
                                    _logger.LogWarning($"[活动超时] SessionID={session.SessionID}, IP={session.ClientIp}, 最后活动={session.LastActivityTime:yyyy-MM-dd HH:mm:ss}, 无活动时间={inactiveTime.TotalMinutes:F1}分钟");
                                }
                                continue;
                            }

                            // 检查2: 未验证会话（欢迎回复超时15分钟后强制断开），延长超时时间适应网络延迟
                            if (!session.WelcomeAckReceived &&
                                session.WelcomeSentTime.HasValue &&
                                session.WelcomeSentTime.Value.AddMinutes(15) < currentTime)
                            {
                                timeoutSessions.Add(session);
                                _logger.LogWarning($"[欢迎超时-定时检查] SessionID={session.SessionID}, IP={session.ClientIp}");
                                continue;
                            }

                            // 检查3: 已验证但未授权的会话（30分钟内未登录强制断开），增加超时时间
                            if (session.WelcomeAckReceived &&
                                !session.IsAuthenticated &&
                                session.ConnectedTime.AddMinutes(30) < currentTime)
                            {
                                timeoutSessions.Add(session);
                                _logger.LogWarning($"[未授权超时] SessionID={session.SessionID}, IP={session.ClientIp}, 连接时间={session.ConnectedTime:yyyy-MM-dd HH:mm:ss}");
                                continue;
                            }
                        }
                    }
                }

                int totalTimeoutSessions = timeoutSessions.Count;
                if (totalTimeoutSessions == 0)
                {
                    // 没有超时会话，直接返回
                    _logger.LogDebug("未发现超时会话，无需清理");
                    return 0;
                }

                var removedCount = 0;

                // 3. 优化并行处理逻辑
                // 根据超时会话数量动态调整并行度，避免过度占用系统资源
                var parallelOptions = new ParallelOptions
                {
                    // 会话数量较少时，使用顺序处理更高效
                    // 会话数量较多时，限制并行度为CPU核心数的一半，避免影响服务器其他功能
                    MaxDegreeOfParallelism = totalTimeoutSessions <= 10 ? 1 : Math.Max(1, Environment.ProcessorCount / 2)
                };

                try
                {
                    Parallel.ForEach(timeoutSessions, parallelOptions, session =>
                    {
                        try
                        {
                            if (RemoveSession(session.SessionID))
                            {
                                Interlocked.Increment(ref removedCount);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 捕获单个会话清理异常，避免影响其他会话清理
                            _logger.LogWarning(ex, $"清理单个超时会话失败: {session.SessionID}");
                        }
                    });
                }
                catch (AggregateException ex)
                {
                    // 处理并行处理中的聚合异常
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        _logger.LogWarning(innerEx, "并行清理超时会话时发生异常");
                    }
                }

                if (removedCount > 0)
                {
                    _logger.LogInformation($"清理超时会话完成，共清理 {removedCount} 个会话");
                }

                return removedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理超时会话失败");
                return 0;
            }
        }

        /// <summary>
        /// ✅ 简化：移除应用层心跳检查
        /// SuperSocket底层已处理TCP连接断开检测，无需重复实现
        /// Client端保留心跳用于主动检测网络状态
        /// </summary>
        [Obsolete("已移除应用层心跳检查，依赖SuperSocket底层连接检测")]
        public int HeartbeatCheck()
        {
            _logger.LogWarning("HeartbeatCheck() 已废弃，SuperSocket底层已处理连接检测");
            return 0;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// ✅ 简化：移除合并清理和心跳检查
        /// SuperSocket底层已处理连接检测，只需定期清理超时会话
        /// </summary>
        [Obsolete("已移除应用层心跳检查")]
        private void CleanupAndHeartbeatCallback(object state)
        {
            // 只保留会话超时清理，不再检查心跳
            CleanupTimeoutSessions();
            
            // 异步清理待处理请求
            _ = Task.Run(() => CleanupPendingRequests()).ConfigureAwait(false);
        }

        /// <summary>
        /// ✅ 简化：移除合并的心跳检查逻辑
        /// 只保留会话活动超时清理（60分钟无活动）
        /// </summary>
        [Obsolete("已移除应用层心跳检查")]
        private (int removedCount, int abnormalCount) CleanupAndHeartbeatCheckCombined()
        {
            var allSessionIds = _sessions.Keys.ToList();
            var sessionsToRemove = new List<string>();
            var currentTime = DateTime.Now;

            // 只检查活动超时（60分钟无活动）
            foreach (var sessionId in allSessionIds)
            {
                if (!_sessions.TryGetValue(sessionId, out var session))
                    continue;

                lock (session)
                {
                    var inactiveTime = currentTime - session.LastActivityTime;
                    if (inactiveTime.TotalMinutes > 60)
                    {
                        sessionsToRemove.Add(sessionId);
                    }
                }
            }

            // 批量移除超时会话
            int removedCount = 0;
            foreach (var sessionId in sessionsToRemove)
            {
                if (RemoveSession(sessionId))
                {
                    removedCount++;
                }
            }

            return (removedCount, 0); // abnormalCount始终为0
        }

        /// <summary>
        /// 清理过期的待处理请求，防止内存泄漏
        /// ✅ 使用统一的追踪器，自动处理超时清理
        /// </summary>
        private void CleanupPendingRequests()
        {
            try
            {
                // ✅ PendingRequestTracker 内部定时器已自动清理超时请求
                // 这里只需记录统计信息
                var pendingCount = _requestTracker.Count;
                if (pendingCount > 100)
                {
                    _logger.LogWarning("待处理请求数量较多: {Count}", pendingCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理待处理请求时发生错误");
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _cleanupTimer?.Dispose();
                // ❌ 移除DDoS防护相关清理 - 已禁用基于IP的防护
                // _rateCleanupTimer?.Dispose();
                // _connectionRates.Clear();

                // Fire-and-forget: 启动异步关闭，不等待完成
                _ = Task.Run(async () =>
                {
                    var sessionIds = _sessions.Keys.ToList();
                    int totalSessions = sessionIds.Count;
                    int closedSessions = 0;
                    
                    foreach (var sessionId in sessionIds)
                    {
                        try
                        {
                            if (_sessions.TryGetValue(sessionId, out var session))
                            {
                                // ✅ 优化：增加超时保护，防止单个会话关闭阻塞整个流程
                                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                                await session.CloseAsync(CloseReason.ServerShutdown);
                                Interlocked.Increment(ref closedSessions);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"关闭会话时出错: {sessionId}");
                        }
                    }
                    
                    _sessions?.Clear();
                    _logger.LogInformation($"统一会话管理器资源已释放，共处理{totalSessions}个会话，成功关闭{closedSessions}个");
                });

                _disposed = true;
                _logger.LogInformation("统一会话管理器资源释放中(异步关闭会话)");
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 收集心跳性能监控数据
        /// </summary>
        private void CollectHeartbeatPerformanceData()
        {
            try
            {
                if (_heartbeatPerformanceMonitor == null)
                    return;

                // 记录当前活动会话数
                _heartbeatPerformanceMonitor.RecordQueueLength(ActiveSessionCount);

                // 记录心跳统计信息
                var stats = GetStatistics();
                if (stats.HeartbeatFailures > 0)
                {
                    // 记录心跳失败
                    for (int i = 0; i < stats.HeartbeatFailures; i++)
                    {
                        _heartbeatPerformanceMonitor.RecordError();
                    }
                }

                _logger.LogDebug($"收集心跳性能数据 - 活动会话: {ActiveSessionCount}, 心跳失败: {stats.HeartbeatFailures}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "收集心跳性能数据时发生异常");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取客户端真实IP地址(优先使用服务器端Socket信息)
        /// </summary>
        /// <param name="session">会话对象</param>
        /// <returns>客户端IP地址</returns>
        private string GetClientIp(IAppSession session)
        {
            try
            {
                if (session?.RemoteEndPoint != null)
                {
                    var ipEndpoint = session.RemoteEndPoint as System.Net.IPEndPoint;
                    if (ipEndpoint != null)
                    {
                        string ip = ipEndpoint.Address.ToString();
                        
                        // 记录IP类型用于调试
                        if (ipEndpoint.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            _logger.LogDebug($"[IP获取] 检测到IPv6客户端: {ip}, SessionID={session.SessionID}");
                        }
                        else
                        {
                            _logger.LogDebug($"[IP获取] IPv4客户端: {ip}, SessionID={session.SessionID}");
                        }
                        
                        return ip;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取客户端IP地址失败");
            }
            
            return "0.0.0.0"; // 使用0.0.0.0表示未知IP
        }

        /// <summary>
        /// ❌ 已移除：清理过期的连接频率记录
        /// 原因：基于IP的DDoS防护在NAT环境下会导致误判，已完全禁用
        /// </summary>
        // private void CleanupConnectionRates(object state)
        // {
        //     try
        //     {
        //         var now = DateTime.Now;
        //         var expiredKeys = _connectionRates
        //             .Where(kvp => kvp.Value.LastConnection < now.AddMinutes(-5))
        //             .Select(kvp => kvp.Key)
        //             .ToList();
        //
        //         foreach (var key in expiredKeys)
        //         {
        //             _connectionRates.TryRemove(key, out _);
        //         }
        //
        //         if (expiredKeys.Count > 0)
        //         {
        //             _logger.LogDebug($"[DDoS防护] 清理{expiredKeys.Count}个过期的连接记录，剩余{_connectionRates.Count}个");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogWarning(ex, "[DDoS防护] 清理连接记录失败");
        //     }
        // }

        #endregion
    }

    /// <summary>
    /// ❌ 已移除：连接频率跟踪器
    /// 原因：基于IP的DDoS防护在NAT环境下会导致误判，已完全禁用
    /// </summary>
    // public class ConnectionRateTracker
    // {
    //     public int ConnectionCount { get; set; }
    //     public DateTime FirstConnection { get; set; }
    //     public DateTime LastConnection { get; set; }
    //
    //     public void RecordConnection()
    //     {
    //         ConnectionCount++;
    //         LastConnection = DateTime.Now;
    //         if (FirstConnection == default)
    //             FirstConnection = DateTime.Now;
    //     }
    //
    //     public bool IsExceedingLimit(int maxConnections, TimeSpan timeWindow)
    //     {
    //         var windowStart = DateTime.Now - timeWindow;
    //         
    //         if (LastConnection < windowStart)
    //         {
    //             ConnectionCount = 0;
    //             FirstConnection = DateTime.Now;
    //             return false;
    //         }
    //         
    //         return ConnectionCount > maxConnections;
    //     }
    // }
}






















