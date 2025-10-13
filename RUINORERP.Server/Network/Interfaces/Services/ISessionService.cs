using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Server.Network.Models;
using SuperSocket.Server.Abstractions.Session;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 会话管理服务接口 - 提供统一的会话生命周期管理
    /// 整合SuperSocket和Network会话管理器的功能
    /// </summary>
    public interface ISessionService : IDisposable
    {
        #region 会话管理
        
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
        SessionInfo CreateSession(string sessionId);

        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话信息</returns>
        SessionInfo GetSession(string sessionId);

        /// <summary>
        /// 获取指定用户名的所有会话
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>会话信息列表</returns>
        IEnumerable<SessionInfo> GetUserSessions(string username);

        /// <summary>
        /// 获取所有已认证的用户会话
        /// </summary>
        /// <param name="excludeSessionIds">可选：要排除的会话ID数组</param>
        /// <returns>已认证的用户会话列表</returns>
        IEnumerable<SessionInfo> GetAllUserSessions(params string[] excludeSessionIds);

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
        /// 验证会话是否有效
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>是否有效</returns>
        bool IsValidSession(string sessionId);

        #endregion

        #region 事件机制

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

        #endregion

        #region 统计和监控

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        SessionStatistics GetStatistics();

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

        #endregion

        #region SuperSocket集成功能

        /// <summary>
        /// 添加SuperSocket会话
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <returns>添加结果</returns>
        Task<bool> AddSessionAsync(IAppSession session);

        /// <summary>
        /// 移除SuperSocket会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>移除结果</returns>
        Task<bool> RemoveSessionAsync(string sessionId);

        /// <summary>
        /// 获取SuperSocket会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>SuperSocket会话</returns>
        IAppSession GetAppSession(string sessionId);

 

        /// <summary>
        /// 更新会话活动时间
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>更新结果</returns>
        bool UpdateSessionActivity(string sessionId);

        /// <summary>
        /// 设置会话属性
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        /// <returns>设置结果</returns>
        bool SetSessionProperty(string sessionId, string key, object value);

        /// <summary>
        /// 主动断开指定会话连接（T人功能）
        /// </summary>
        /// <param name="sessionId">要断开的会话ID</param>
        /// <param name="reason">断开原因，默认为"服务器强制断开"</param>
        /// <returns>断开是否成功</returns>
        Task<bool> DisconnectSessionAsync(string sessionId, string reason = "服务器强制断开");

        /// <summary>
        /// 主动断开指定用户的所有会话连接（T人功能）
        /// </summary>
        /// <param name="username">要断开的用户名</param>
        /// <param name="reason">断开原因，默认为"服务器强制断开"</param>
        /// <returns>成功断开的会话数量</returns>
        Task<int> DisconnectUserSessionsAsync(string username, string reason = "服务器强制断开");

        bool SendCommandToSession(string sessionID, string v, object value);

        #endregion
    }

 
}