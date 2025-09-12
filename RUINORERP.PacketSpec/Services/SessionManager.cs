using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Services
{
    /// <summary>
    /// 会话管理服务接口
    /// </summary>
    public interface ISessionManager
    {
        /// <summary>
        /// 活动会话数量
        /// </summary>
        int ActiveSessionCount { get; }

        /// <summary>
        /// 最大会话数量
        /// </summary>
        int MaxSessionCount { get; set; }

        /// <summary>
        /// 创建新会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientPort">客户端端口</param>
        /// <returns>会话信息</returns>
        SessionInfo CreateSession(string sessionId, string clientIp, int clientPort = 0);

        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话信息</returns>
        SessionInfo GetSession(string sessionId);

        /// <summary>
        /// 更新会话信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新是否成功</returns>
        bool UpdateSession(SessionInfo sessionInfo);

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>删除是否成功</returns>
        bool RemoveSession(string sessionId);

        /// <summary>
        /// 获取所有活动会话
        /// </summary>
        /// <returns>活动会话列表</returns>
        IEnumerable<SessionInfo> GetActiveSessions();

        /// <summary>
        /// 根据用户ID获取会话
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>会话信息列表</returns>
        IEnumerable<SessionInfo> GetSessionsByUserId(int userId);

        /// <summary>
        /// 根据用户名获取会话
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>会话信息列表</returns>
        IEnumerable<SessionInfo> GetSessionsByUsername(string username);

        /// <summary>
        /// 清理超时会话
        /// </summary>
        /// <returns>清理的会话数量</returns>
        int CleanupTimeoutSessions();

        /// <summary>
        /// 心跳检查
        /// </summary>
        /// <returns>心跳异常的会话数量</returns>
        int HeartbeatCheck();

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        SessionStatistics GetStatistics();

        /// <summary>
        /// 验证会话是否有效
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>是否有效</returns>
        bool IsValidSession(string sessionId);

        /// <summary>
        /// 会话连接事件
        /// </summary>
        event Action<SessionInfo> SessionConnected;

        /// <summary>
        /// 会话断开事件
        /// </summary>
        event Action<SessionInfo> SessionDisconnected;

        /// <summary>
        /// 会话更新事件
        /// </summary>
        event Action<SessionInfo> SessionUpdated;
    }

    /// <summary>
    /// 会话管理服务实现
    /// </summary>
    public class SessionManager : ISessionManager, IDisposable
    {
        private readonly ConcurrentDictionary<string, SessionInfo> _sessions;
        private readonly Timer _cleanupTimer;
        private readonly Timer _heartbeatTimer;
        private readonly SessionStatistics _statistics;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

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
        /// 构造函数
        /// </summary>
        /// <param name="maxSessionCount">最大会话数量</param>
        public SessionManager(int maxSessionCount = 1000)
        {
            MaxSessionCount = maxSessionCount;
            _sessions = new ConcurrentDictionary<string, SessionInfo>();
            _statistics = SessionStatistics.Create(maxSessionCount);

            // 启动清理定时器，每5分钟清理一次超时会话
            _cleanupTimer = new Timer(CleanupCallback, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

            // 启动心跳检查定时器，每分钟检查一次
            _heartbeatTimer = new Timer(HeartbeatCallback, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            LogInfo("会话管理器已启动");
        }

        /// <summary>
        /// 创建新会话
        /// </summary>
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
                    LogWarning($"达到最大会话数量限制: {MaxSessionCount}");
                    return null;
                }

                var sessionInfo = SessionInfo.Create(sessionId, clientIp, clientPort);
                
                if (_sessions.TryAdd(sessionId, sessionInfo))
                {
                    lock (_lockObject)
                    {
                        _statistics.TotalConnections++;
                        _statistics.UpdateConnectionStats(ActiveSessionCount);
                    }

                    LogInfo($"创建新会话: {sessionId} ({clientIp}:{clientPort})");
                    SessionConnected?.Invoke(sessionInfo);
                    return sessionInfo;
                }
                else
                {
                    LogWarning($"会话ID已存在: {sessionId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogError($"创建会话失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取会话信息
        /// </summary>
        public SessionInfo GetSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            _sessions.TryGetValue(sessionId, out var sessionInfo);
            return sessionInfo;
        }

        /// <summary>
        /// 更新会话信息
        /// </summary>
        public bool UpdateSession(SessionInfo sessionInfo)
        {
            try
            {
                if (sessionInfo == null || string.IsNullOrEmpty(sessionInfo.SessionId))
                {
                    return false;
                }

                if (_sessions.TryGetValue(sessionInfo.SessionId, out var existingSession))
                {
                    // 更新现有会话信息
                    existingSession.LastActivityTime = sessionInfo.LastActivityTime;
                    existingSession.HeartbeatCount = sessionInfo.HeartbeatCount;
                    existingSession.Status = sessionInfo.Status;
                    existingSession.UserId = sessionInfo.UserId;
                    existingSession.Username = sessionInfo.Username;
                    existingSession.IsAuthenticated = sessionInfo.IsAuthenticated;
                    existingSession.SentPacketsCount = sessionInfo.SentPacketsCount;
                    existingSession.ReceivedPacketsCount = sessionInfo.ReceivedPacketsCount;

                    SessionUpdated?.Invoke(existingSession);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"更新会话失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        public bool RemoveSession(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    return false;
                }

                if (_sessions.TryRemove(sessionId, out var sessionInfo))
                {
                    lock (_lockObject)
                    {
                        _statistics.UpdateConnectionStats(ActiveSessionCount);
                    }

                    LogInfo($"删除会话: {sessionId}");
                    SessionDisconnected?.Invoke(sessionInfo);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"删除会话失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取所有活动会话
        /// </summary>
        public IEnumerable<SessionInfo> GetActiveSessions()
        {
            return _sessions.Values.Where(s => s.Status != SessionStatus.Disconnected);
        }

        /// <summary>
        /// 根据用户ID获取会话
        /// </summary>
        public IEnumerable<SessionInfo> GetSessionsByUserId(int userId)
        {
            return _sessions.Values.Where(s => s.UserId == userId && s.Status != SessionStatus.Disconnected);
        }

        /// <summary>
        /// 根据用户名获取会话
        /// </summary>
        public IEnumerable<SessionInfo> GetSessionsByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return Enumerable.Empty<SessionInfo>();
            }

            return _sessions.Values.Where(s => 
                string.Equals(s.Username, username, StringComparison.OrdinalIgnoreCase) && 
                s.Status != SessionStatus.Disconnected);
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public int CleanupTimeoutSessions()
        {
            var cleanupCount = 0;
            var timeoutSessions = new List<string>();

            foreach (var kvp in _sessions)
            {
                if (kvp.Value.IsTimeout())
                {
                    timeoutSessions.Add(kvp.Key);
                }
            }

            foreach (var sessionId in timeoutSessions)
            {
                if (RemoveSession(sessionId))
                {
                    cleanupCount++;
                    lock (_lockObject)
                    {
                        _statistics.TotalTimeouts++;
                    }
                }
            }

            if (cleanupCount > 0)
            {
                LogInfo($"清理了 {cleanupCount} 个超时会话");
            }

            return cleanupCount;
        }

        /// <summary>
        /// 心跳检查
        /// </summary>
        public int HeartbeatCheck()
        {
            var abnormalCount = 0;

            foreach (var sessionInfo in _sessions.Values)
            {
                if (!sessionInfo.IsHeartbeatNormal())
                {
                    abnormalCount++;
                    sessionInfo.Status = SessionStatus.Timeout;
                    LogWarning($"会话心跳异常: {sessionInfo.SessionId}");
                }
                else
                {
                    // 更新心跳检查计数
                    sessionInfo.HeartbeatCheckCount = sessionInfo.HeartbeatCount;
                }
            }

            return abnormalCount;
        }

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        public SessionStatistics GetStatistics()
        {
            lock (_lockObject)
            {
                _statistics.CurrentConnections = ActiveSessionCount;
                return _statistics;
            }
        }

        /// <summary>
        /// 验证会话是否有效
        /// </summary>
        public bool IsValidSession(string sessionId)
        {
            var session = GetSession(sessionId);
            return session != null && session.IsValid() && session.Status != SessionStatus.Disconnected;
        }

        /// <summary>
        /// 清理定时器回调
        /// </summary>
        private void CleanupCallback(object state)
        {
            try
            {
                CleanupTimeoutSessions();
            }
            catch (Exception ex)
            {
                LogError($"定时清理会话时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 心跳检查定时器回调
        /// </summary>
        private void HeartbeatCallback(object state)
        {
            try
            {
                HeartbeatCheck();
            }
            catch (Exception ex)
            {
                LogError($"心跳检查时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        private void LogInfo(string message)
        {
            Console.WriteLine($"[SessionManager] INFO: {message}");
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        private void LogWarning(string message)
        {
            Console.WriteLine($"[SessionManager] WARNING: {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            Console.WriteLine($"[SessionManager] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[SessionManager] Exception: {ex}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _cleanupTimer?.Dispose();
                _heartbeatTimer?.Dispose();
                _sessions.Clear();
                _disposed = true;

                LogInfo("会话管理器已停止");
            }

            GC.SuppressFinalize(this);
        }
    }
}