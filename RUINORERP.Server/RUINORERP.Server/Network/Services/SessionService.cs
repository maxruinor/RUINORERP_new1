using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server;
using SuperSocket.Channel;
using SuperSocket.Connection;


namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// ✅ [统一架构] 统一会话管理器 - 整合SuperSocket和Network会话管理功能
    /// 提供完整的会话生命周期管理、事件机制、统计监控和SuperSocket集成
    /// 会话管理行为严格基于SuperSocket连接和断开事件实现
    /// </summary>
    public class SessionService : ISessionService, IDisposable, RUINORERP.Server.SuperSocketServices.IServerSessionEventHandler
    {
        #region 字段和属性

        private readonly ConcurrentDictionary<string, SessionInfo> _sessions;
        private readonly ConcurrentDictionary<string, IAppSession> _appSessions;
        private readonly Timer _cleanupTimer;
        private readonly Timer _heartbeatTimer;
        private readonly SessionStatistics _statistics;
        private readonly object _lockObject = new object();
        private bool _disposed = false;
        private readonly ILogger<SessionService> _logger;

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

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="maxSessionCount">最大会话数量</param>
        public SessionService(ILogger<SessionService> logger, int maxSessionCount = 1000)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MaxSessionCount = maxSessionCount;
            _sessions = new ConcurrentDictionary<string, SessionInfo>();
            _appSessions = new ConcurrentDictionary<string, IAppSession>();
            _statistics = SessionStatistics.Create(maxSessionCount);

            // 启动清理定时器，每5分钟清理一次超时会话
            _cleanupTimer = new Timer(CleanupCallback, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

            // 启动心跳检查定时器，每分钟检查一次
            _heartbeatTimer = new Timer(HeartbeatCallback, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            _logger.LogInformation("统一会话管理器已启动 - 整合SuperSocket和Network会话管理功能");
        }

        #endregion

        #region ISessionManager 实现 - 基础会话管理

        /// <summary>
        /// 创建新会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientPort">客户端端口</param>
        /// <returns>会话信息</returns>
        public SessionInfo CreateSession(string sessionId, string clientIp, int clientPort = 0)
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

                var sessionInfo = SessionInfo.Create(sessionId, clientIp, clientPort);

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

                    _logger.LogInformation($"创建新会话: {sessionId}, 客户端: {clientIp}:{clientPort}");
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
                    .Where(s => s.IsAuthenticated && s.Username == username)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户会话信息失败: {username}");
                return Enumerable.Empty<SessionInfo>();
            }
        }

        /// <summary>
        /// 创建已认证的会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="userInfo">用户会话信息</param>
        /// <param name="clientIp">客户端IP</param>
        /// <returns>会话信息</returns>
        public async Task<SessionInfo> CreateAuthenticatedSessionAsync(string sessionId, UserSessionInfo userInfo, string clientIp)
        {
            var session = CreateSession(sessionId, clientIp);
            if (session != null)
            {
                session.UserId = userInfo.UserInfo.User_ID;
                session.Username = userInfo.UserInfo.UserName;
                session.IsAuthenticated = true;
                session.LoginTime = DateTime.UtcNow;
                session.UpdateActivity();
                
                // 使用现有同步方法但包装为异步任务返回
                await Task.Run(() => UpdateSession(session));
                return session;
            }
            return null;
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
                    return false;

                if (_sessions.TryGetValue(sessionInfo.SessionID, out var existingSession))
                {
                    // 更新会话信息
                    existingSession.UserId = sessionInfo.UserId;
                    existingSession.Username = sessionInfo.Username;
                    existingSession.IsAuthenticated = sessionInfo.IsAuthenticated;
                    //existingSession.IsAdmin = sessionInfo.IsAdmin;
                    existingSession.UpdateActivity();
                    existingSession.DataContext = sessionInfo.DataContext;

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
                    // 同时移除SuperSocket会话
                    _appSessions.TryRemove(sessionId, out _);

                    lock (_lockObject)
                    {
                        _statistics.TotalDisconnections++;
                        _statistics.CurrentConnections = ActiveSessionCount;
                    }

                    if (sessionInfo != null)
                    {
                        sessionInfo.IsConnected = false;
                        sessionInfo.DisconnectTime = DateTime.Now;

                        // 触发会话断开事件
                        SessionDisconnected?.Invoke(sessionInfo);

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

                return _sessions.ContainsKey(sessionId);
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
                // 检查是否已达到最大会话数
                if (ActiveSessionCount >= MaxSessionCount)
                {
                    _logger.LogWarning($"达到最大会话数量限制: {MaxSessionCount}，拒绝新连接");
                    await session.CloseAsync(CloseReason.ServerShutdown);
                    return;
                }

                SessionInfo sessionInfo = session as SessionInfo;

                // 创建会话信息
                sessionInfo.ConnectedTime = DateTime.Now;
                sessionInfo.UpdateActivity();
                sessionInfo.IsConnected = true;
                sessionInfo.Properties = new Dictionary<string, object>();


                // 存储会话
                var added = _sessions.TryAdd(session.SessionID, sessionInfo);
                if (added)
                {
                    _appSessions.TryAdd(session.SessionID, session);

                    lock (_lockObject)
                    {
                        _statistics.TotalConnections++;
                        _statistics.CurrentConnections = ActiveSessionCount;
                        _statistics.PeakConnections = Math.Max(_statistics.PeakConnections, ActiveSessionCount);
                    }

                    // 触发会话连接事件
                    SessionConnected?.Invoke(sessionInfo);
                    // 调用IServerSessionEventHandler接口的会话连接方法
                    await OnSessionConnectedAsync(sessionInfo);

                    _logger.LogInformation($"SuperSocket会话已连接: SessionID={session.SessionID}, RemoteIP={sessionInfo.RemoteEndPoint}");
                }
                else
                {
                    _logger.LogWarning($"SuperSocket会话连接失败，SessionID已存在: {session.SessionID}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话连接事件时出错: SessionID={session?.SessionID}");
            }
        }

        /// <summary>
        /// 会话连接事件处理 - 项目自定义接口实现
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>异步任务</returns>
        public async Task OnSessionConnectedAsync(SessionInfo sessionInfo)
        {
            try
            {
                // 可以在这里添加额外的连接后处理逻辑
                // 例如记录连接信息、初始化会话状态等
                _logger.LogDebug($"执行会话连接后的自定义处理逻辑: {sessionInfo.SessionID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"执行会话连接自定义处理逻辑时出错: {sessionInfo.SessionID}");
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

                await RemoveSessionAsync(session.SessionID);

                // 如果存在会话信息，调用IServerSessionEventHandler接口的会话断开方法
                if (sessionInfo != null)
                {
                    await OnSessionDisconnectedAsync(sessionInfo, closeReason.Reason.ToString());
                }

                _logger.LogInformation($"SuperSocket会话已断开: SessionID={session.SessionID}, 原因={closeReason.Reason}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话断开事件时出错: SessionID={session?.SessionID}");
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
                _logger.LogInformation($"用户认证成功: SessionID={sessionInfo.SessionID}, Username={sessionInfo.Username}");
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

        #region ISessionService 实现 - SuperSocket集成

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
            
            _appSessions.TryGetValue(sessionId, out var appSession);
            return appSession;
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

        #endregion

        #region ISessionManager 实现 - 统计和监控

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public SessionStatistics GetStatistics()
        {
            lock (_lockObject)
            {
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
        /// 清理超时会话
        /// </summary>
        /// <returns>清理的会话数量</returns>
        public int CleanupTimeoutSessions()
        {
            try
            {
                var timeoutSessions = _sessions.Values
                    .Where(s => s.LastActiveTime.AddMinutes(30) < DateTime.Now)
                    .ToList();

                var removedCount = 0;
                
                // 使用Parallel.ForEach并行处理超时会话的移除
                Parallel.ForEach(timeoutSessions, session => {
                    if (RemoveSession(session.SessionID))
                    {
                        Interlocked.Increment(ref removedCount);
                    }
                });

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
        /// 心跳检查
        /// </summary>
        /// <returns>心跳异常的会话数量</returns>
        public int HeartbeatCheck()
        {
            try
            {
                var abnormalSessions = _sessions.Values
                    .Where(s => s.LastHeartbeat.AddMinutes(5) < DateTime.Now)
                    .ToList();

                var abnormalCount = 0;
                foreach (var session in abnormalSessions)
                {
                    _logger.LogWarning($"会话心跳异常: {session.SessionID}, 用户: {session.Username}");
                    abnormalCount++;
                }

                return abnormalCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "心跳检查失败");
                return 0;
            }
        }

        #endregion

        #region 私有方法
 

        /// <summary>
        /// 清理回调
        /// </summary>
        private void CleanupCallback(object state)
        {
            try
            {
                var removedCount = CleanupTimeoutSessions();
                lock (_lockObject)
                {
                    _statistics.TimeoutSessions += removedCount;
                    _statistics.LastCleanupTime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理回调执行失败");
            }
        }

        /// <summary>
        /// 心跳回调
        /// </summary>
        private void HeartbeatCallback(object state)
        {
            try
            {
                var abnormalCount = HeartbeatCheck();
                lock (_lockObject)
                {
                    _statistics.HeartbeatFailures += abnormalCount;
                    _statistics.LastHeartbeatCheck = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "心跳回调执行失败");
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
                _heartbeatTimer?.Dispose();

                // 关闭并清理所有活动会话
                foreach (var sessionId in _appSessions.Keys.ToList())
                {
                    if (_appSessions.TryGetValue(sessionId, out var session))
                    {
                        try
                        {
                            session.CloseAsync(CloseReason.ServerShutdown).AsTask().Wait(100);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"关闭会话时出错: {sessionId}");
                        }
                    }
                }

                _sessions?.Clear();
                _appSessions?.Clear();
                _disposed = true;
                _logger.LogInformation("统一会话管理器资源已释放");
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}





