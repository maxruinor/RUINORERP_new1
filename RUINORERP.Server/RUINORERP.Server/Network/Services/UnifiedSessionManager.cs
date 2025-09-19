using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using SuperSocket.Server.Abstractions.Session;


namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// ✅ [统一架构] 统一会话管理器 - 整合SuperSocket和Network会话管理功能
    /// 合并了原有的SessionManager和SuperSocket SessionManager功能
    /// 提供完整的会话生命周期管理、事件机制、统计监控和SuperSocket集成
    /// </summary>
    public class SessionManager : ISessionManager, IDisposable
    {
        #region 字段和属性

        private readonly ConcurrentDictionary<string, SessionInfo> _sessions;
        private readonly ConcurrentDictionary<string, IAppSession> _appSessions;
        private readonly Timer _cleanupTimer;
        private readonly Timer _heartbeatTimer;
        private readonly SessionStatistics _statistics;
        private readonly object _lockObject = new object();
        private bool _disposed = false;
        private readonly ILogger<SessionManager> _logger;

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
        public SessionManager(ILogger<SessionManager> logger, int maxSessionCount = 1000)
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
                    sessionInfo.LastActivityTime = DateTime.Now;
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
                    existingSession.LastActivityTime = DateTime.Now;
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

        #region ISessionManager 实现 - SuperSocket集成

        /// <summary>
        /// 注册SuperSocket会话（为兼容旧版接口）
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="session">SuperSocket会话</param>
        /// <returns>注册结果</returns>
        public async Task<bool> RegisterSessionAsync(SessionInfo sessionInfo, IAppSession session)
        {
            try
            {
                // 确保会话信息正确设置
                if (sessionInfo != null && string.IsNullOrEmpty(sessionInfo.SessionID))
                {
                    //sessionInfo.SessionID = session.SessionID;
                }
                
                // 调用现有的AddSessionAsync方法
                var result = await AddSessionAsync(session);
                
                // 如果会话信息不为空，更新会话信息
                if (result && sessionInfo != null)
                {
                    UpdateSession(sessionInfo);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册会话失败");
                return false;
            }
        }

        /// <summary>
        /// 注销SuperSocket会话（为兼容旧版接口）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>注销结果</returns>
        public async Task<bool> UnregisterSessionAsync(string sessionId)
        {
            // 直接调用现有的RemoveSessionAsync方法
            return await RemoveSessionAsync(sessionId);
        }

        /// <summary>
        /// 添加SuperSocket会话
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <returns>添加结果</returns>
        public async Task<bool> AddSessionAsync(IAppSession session)
        {
            try
            {
                var sessionInfo = new SessionInfo
                {
                    //SessionID = session.SessionID,
                    //RemoteEndPoint = session.RemoteEndPoint?.ToString(),
                    //LocalEndPoint = session.LocalEndPoint?.ToString(),
                    //ConnectedTime = DateTime.UtcNow,
                    //LastActiveTime = DateTime.UtcNow,
                    IsConnected = true,
                    Properties = new Dictionary<string, object>()
                };

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

                    _logger.LogInformation($"SuperSocket会话已添加: SessionID={session.SessionID}, RemoteIP={sessionInfo.RemoteEndPoint}");
                }
                else
                {
                    _logger.LogWarning($"SuperSocket会话添加失败，SessionID已存在: {session.SessionID}");
                }

                return added;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"添加SuperSocket会话时出错: SessionID={session?.SessionID}");
                return false;
            }
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
            _appSessions.TryGetValue(sessionId, out var appSession);
            return appSession;
        }

        /// <summary>
        /// 广播消息到所有活动会话
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>成功发送的会话数量</returns>
        public async Task<int> BroadcastMessageAsync(object message)
        {
            try
            {
                var activeSessions = _sessions.Values
                    .Where(s => s.IsConnected)
                    .ToList();

                if (activeSessions.Count == 0)
                {
                    _logger.LogInformation("没有活动会话，跳过广播");
                    return 0;
                }

                var successCount = 0;
                var tasks = new List<Task>();

                foreach (var sessionInfo in activeSessions)
                {
                    var appSession = GetAppSession(sessionInfo.SessionID);
                    if (appSession != null)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                await SendMessageToSession(appSession, message);
                                Interlocked.Increment(ref successCount);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"向会话 {sessionInfo.SessionID} 发送消息时出错");
                            }
                        }));
                    }
                }

                await Task.WhenAll(tasks);
                _logger.LogInformation($"消息广播完成: 目标会话={activeSessions.Count}, 成功发送={successCount}");
                
                return successCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "广播消息时出错");
                return 0;
            }
        }

        /// <summary>
        /// 更新会话活动时间
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>更新结果</returns>
        public bool UpdateSessionActivity(string sessionId)
        {
            try
            {
                if (_sessions.TryGetValue(sessionId, out var sessionInfo))
                {
                    //sessionInfo.LastActiveTime = DateTime.Now;
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
                foreach (var session in timeoutSessions)
                {
                    if (RemoveSession(session.SessionID))
                    {
                        removedCount++;
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
        /// 向指定会话发送消息
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="message">消息</param>
        /// <returns>发送任务</returns>
        private async Task SendMessageToSession(IAppSession session, object message)
        {
            try
            {
                // 这里应该根据实际的消息格式和发送逻辑实现
                // 暂时使用JSON序列化
                var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                var messageBytes = System.Text.Encoding.UTF8.GetBytes(jsonMessage);
                
                await session.SendAsync(messageBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"发送消息到会话时出错: SessionID={session.SessionID}");
                throw;
            }
        }

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