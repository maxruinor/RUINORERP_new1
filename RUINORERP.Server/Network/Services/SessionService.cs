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
using RUINORERP.Business.CommService;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Models.Responses.Message;

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

        // 只保留一个会话字典，SessionInfo继承自AppSession，本身就是IAppSession
        private readonly ConcurrentDictionary<string, SessionInfo> _sessions;
        private readonly Timer _cleanupTimer;
        private readonly SessionStatistics _statistics;
        private readonly object _lockObject = new object();
        private bool _disposed = false;
        private readonly ILogger<SessionService> _logger;
        private readonly CacheSubscriptionManager _subscriptionManager; // 使用统一的订阅管理器
        
        // 存储待处理的请求任务，用于匹配响应
        private static readonly ConcurrentDictionary<string, TaskCompletionSource<PacketModel>> _pendingRequests = 
            new ConcurrentDictionary<string, TaskCompletionSource<PacketModel>>();

        #endregion

        #region 事件定义

        /// <summary>
        /// 消息响应事件 - 当从客户端接收到响应时触发
        /// </summary>
        public event EventHandler<MessageResponseEventArgs> MessageResponseReceived;

        /// <summary>
        /// 弹窗消息响应事件
        /// </summary>
        public event EventHandler<MessageResponseEventArgs> PopupMessageResponseReceived;

        /// <summary>
        /// 用户消息响应事件
        /// </summary>
        public event EventHandler<MessageResponseEventArgs> UserMessageResponseReceived;

        /// <summary>
        /// 部门消息响应事件
        /// </summary>
        public event EventHandler<MessageResponseEventArgs> DepartmentMessageResponseReceived;

        /// <summary>
        /// 广播消息响应事件
        /// </summary>
        public event EventHandler<MessageResponseEventArgs> BroadcastMessageResponseReceived;

        /// <summary>
        /// 系统通知响应事件
        /// </summary>
        public event EventHandler<MessageResponseEventArgs> SystemNotificationResponseReceived;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="maxSessionCount">最大会话数量</param>
        public SessionService(ILogger<SessionService> logger, CacheSubscriptionManager subscriptionManager, int maxSessionCount = 1000)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MaxSessionCount = maxSessionCount;
            _sessions = new ConcurrentDictionary<string, SessionInfo>();
            _statistics = SessionStatistics.Create(maxSessionCount);
            _subscriptionManager = subscriptionManager; // 服务器模式
            _subscriptionManager.IsServerMode = true;
            // 启动清理定时器，每5分钟清理一次超时会话并检查心跳
            _cleanupTimer = new Timer(CleanupAndHeartbeatCallback, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

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

                return query.ToList();
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
                    return false;

                if (_sessions.TryGetValue(sessionInfo.SessionID, out var existingSession))
                {
                    // 更新会话信息
                    existingSession.UserId = sessionInfo.UserId;
                    existingSession.UserName = sessionInfo.UserName;
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
                    lock (_lockObject)
                    {
                        _statistics.TotalDisconnections++;
                        _statistics.CurrentConnections = ActiveSessionCount;
                    }

                    if (sessionInfo != null)
                    {
                        sessionInfo.IsConnected = false;
                        sessionInfo.DisconnectTime = DateTime.Now;

                        // 取消该会话的所有缓存订阅
                        _subscriptionManager.UnsubscribeAll(sessionId);

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

                // SessionInfo继承自AppSession，可以直接转换
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
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    _logger.LogWarning("断开会话失败：会话ID为空");
                    return false;
                }

                // 获取会话信息
                if (!_sessions.TryGetValue(sessionId, out var sessionInfo))
                {
                    _logger.LogWarning($"断开会话失败：会话不存在，SessionID={sessionId}");
                    return false;
                }

                try
                {
                    // 主动关闭SuperSocket连接
                    await sessionInfo.CloseAsync(CloseReason.ServerShutdown);
                    _logger.LogInformation($"已主动断开会话连接: SessionID={sessionId}, 用户={sessionInfo.UserName}, 原因={reason}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"关闭SuperSocket会话连接失败: SessionID={sessionId}");
                }

                // 移除会话记录
                var removeResult = RemoveSession(sessionId);

                if (removeResult)
                {
                    _logger.LogInformation($"会话已成功断开并移除: SessionID={sessionId}, 用户={sessionInfo.UserName}, 原因={reason}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"会话断开但移除记录失败: SessionID={sessionId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"断开会话时发生异常: SessionID={sessionId}");
                return false;
            }
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

                var results = await Task.WhenAll(disconnectTasks);
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
            CancellationToken ct,
            string responseTypeName = null)
            where TRequest : class, IRequest
        {
            ct.ThrowIfCancellationRequested();
            
            try
            {
                // 构建数据包
                var packet = PacketBuilder.Create()
                    .WithDirection(PacketDirection.Request)
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
                var payload = JsonCompressionSerializationService.Serialize<PacketModel>(packet);
                var original = new OriginalData((byte)packet.CommandId.Category, new[] { packet.CommandId.OperationCode }, payload);
                var encrypted = UnifiedEncryptionProtocol.EncryptServerDataToClient(original);
                
                // 发送数据
                await sessionInfo.SendAsync(encrypted.ToArray(), ct);
                
                _logger?.LogDebug("数据包发送成功: SessionID={SessionID}, CommandId={CommandId}, RequestId={RequestId}",
                    sessionInfo.SessionID, commandId, request.RequestId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"发送数据包时发生错误: SessionID={sessionInfo?.SessionID}, CommandId={commandId}");
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
            int timeoutMs = 30000, 
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

                // 创建任务完成源
                var tcs = new TaskCompletionSource<PacketModel>();
                var requestId = request.RequestId;
                
                // 注册待处理请求
                _pendingRequests.TryAdd(requestId, tcs);
                
                try
                {
                    // 发送命令
                    await SendPacketCoreAsync(sessionInfo, commandId, request, timeoutMs, ct);
                    
                    // 等待响应或超时
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    cts.CancelAfter(timeoutMs);
                    
                    var timeoutTask = Task.Delay(timeoutMs, cts.Token);
                    var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
                    
                    // 从待处理请求中移除
                    _pendingRequests.TryRemove(requestId, out _);
                    
                    if (completedTask == timeoutTask)
                    {
                        throw new TimeoutException($"请求超时（{timeoutMs}ms），指令类型：{commandId.ToString()}，请求ID: {requestId}");
                    }
                    
                    ct.ThrowIfCancellationRequested();
                    
                    return await tcs.Task;
                }
                catch (Exception)
                {
                    // 从待处理请求中移除
                    _pendingRequests.TryRemove(requestId, out _);
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

                if (commandId == null)
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
        /// </summary>
        /// <param name="packet">响应数据包</param>
        public void HandleClientResponse(PacketModel packet)
        {
            try
            {
                var requestId = packet?.ExecutionContext?.RequestId;
                if (string.IsNullOrEmpty(requestId))
                    return;

                // 查找匹配的待处理请求
                if (_pendingRequests.TryRemove(requestId, out var pendingRequest))
                {
                    // 完成任务
                    pendingRequest.TrySetResult(packet);
                    _logger?.LogDebug("处理客户端响应完成，请求ID: {RequestId}", requestId);
                }
                else
                {
                    // 如果没有等待任务，则触发事件
                    TriggerResponseEvents(packet);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理客户端响应时发生错误");
            }
        }
        
        /// <summary>
        /// 触发响应事件
        /// </summary>
        /// <param name="packet">响应数据包</param>
        private void TriggerResponseEvents(PacketModel packet)
        {
            try
            {
                var eventArgs = new MessageResponseEventArgs
                {
                    SessionId = packet.ExecutionContext?.SessionId,
                    CommandId = packet.CommandId,
                    ResponseData = packet.Response,
                    IsSuccess = packet.Response?.IsSuccess ?? false,
                    ErrorMessage = packet.Response?.ErrorMessage,
                    Timestamp = DateTime.Now
                };

                // 触发通用响应事件
                MessageResponseReceived?.Invoke(this, eventArgs);

                // 根据命令类型触发特定事件
                switch (packet.CommandId.FullCode)
                {
                    case var code when code == PacketSpec.Commands.Message.MessageCommands.SendPopupMessage.FullCode:
                        PopupMessageResponseReceived?.Invoke(this, eventArgs);
                        break;
                    case var code when code == PacketSpec.Commands.Message.MessageCommands.SendMessageToUser.FullCode:
                        UserMessageResponseReceived?.Invoke(this, eventArgs);
                        break;
                    case var code when code == PacketSpec.Commands.Message.MessageCommands.SendMessageToDepartment.FullCode:
                        DepartmentMessageResponseReceived?.Invoke(this, eventArgs);
                        break;
                    case var code when code == PacketSpec.Commands.Message.MessageCommands.BroadcastMessage.FullCode:
                        BroadcastMessageResponseReceived?.Invoke(this, eventArgs);
                        break;
                    case var code when code == PacketSpec.Commands.Message.MessageCommands.SendSystemNotification.FullCode:
                        SystemNotificationResponseReceived?.Invoke(this, eventArgs);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发响应事件时发生错误");
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
                    .Where(s => s.LastActivityTime.AddMinutes(30) < DateTime.Now)
                    .ToList();

                var removedCount = 0;

                // 使用Parallel.ForEach并行处理超时会话的移除
                Parallel.ForEach(timeoutSessions, session =>
                {
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
                    _logger.LogWarning($"会话心跳异常: {session.SessionID}, 用户: {session.UserName}");
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
        /// 清理和心跳检查回调
        /// 合并清理超时会话和心跳检查到一个定时器中，减少系统开销
        /// </summary>
        private void CleanupAndHeartbeatCallback(object state)
        {
            try
            {
                // 清理超时会话
                var removedCount = CleanupTimeoutSessions();

                // 检查心跳异常
                var abnormalCount = HeartbeatCheck();

                lock (_lockObject)
                {
                    _statistics.TimeoutSessions += removedCount;
                    _statistics.HeartbeatFailures += abnormalCount;
                    _statistics.LastCleanupTime = DateTime.Now;
                    _statistics.LastHeartbeatCheck = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理和心跳检查回调执行失败");
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

                // 关闭并清理所有活动会话
                foreach (var sessionId in _sessions.Keys.ToList())
                {
                    if (_sessions.TryGetValue(sessionId, out var session))
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
                _disposed = true;
                _logger.LogDebug("统一会话管理器资源已释放");
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}














