using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Server.Network.Models;

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
        Task<bool> AddSessionAsync(SuperSocket.Server.Abstractions.Session.IAppSession session);

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
        SuperSocket.Server.Abstractions.Session.IAppSession GetAppSession(string sessionId);

        /// <summary>
        /// 广播消息到所有活动会话
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>成功发送的会话数量</returns>
        Task<int> BroadcastMessageAsync(object message);

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

        #endregion
    }

 
}