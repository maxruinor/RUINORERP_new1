using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.Server.Network.Models;
using SuperSocket.Connection;
using SuperSocket.Server.Abstractions.Session;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 会话管理服务接口 - 重构版
    /// 整合SuperSocket和Network会话管理器的功能
    /// 
    /// 【重构要点】
    /// 1. 简化接口方法签名
    /// 2. 统一会话信息访问方式
    /// 3. 明确返回值和异常处理
    /// </summary>
    public interface ISessionService : IDisposable
    {
        #region 会话管理 - 基础属性

        /// <summary>
        /// 活动会话数量
        /// </summary>
        int ActiveSessionCount { get; }

        /// <summary>
        /// 最大会话数量
        /// </summary>
        int MaxSessionCount { get; set; }

        #endregion

        #region 会话管理 - 创建和销毁

        /// <summary>
        /// 创建新会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话信息</returns>
        SessionInfo CreateSession(string sessionId);

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

        #region 会话管理 - 查询

        /// <summary>
        /// 获取会话信息（通过会话ID）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话信息</returns>
        SessionInfo GetSession(string sessionId);

        /// <summary>
        /// 获取会话信息（通过用户ID）
        /// </summary>
        /// <param name="userId">用户ID（用户名表主键）</param>
        /// <returns>会话信息</returns>
        SessionInfo GetSession(long userId);

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
        /// 获取SuperSocket会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>SuperSocket会话</returns>
        IAppSession GetAppSession(string sessionId);

        #endregion

        #region 会话管理 - 更新

        /// <summary>
        /// 更新会话信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新是否成功</returns>
        bool UpdateSession(SessionInfo sessionInfo);
        
        /// <summary>
        /// 异步更新会话信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新任务</returns>
        Task<bool> UpdateSessionAsync(SessionInfo sessionInfo);
        
        /// <summary>
        /// 轻量级会话更新（仅更新活动时间）
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>更新是否成功</returns>
        bool UpdateSessionLight(SessionInfo sessionInfo);

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
        /// 获取统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        SessionStatistics GetStatistics();

        /// <summary>
        /// 重置会话统计信息
        /// </summary>
        void ResetStatistics();

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

        #region SuperSocket集成

        /// <summary>
        /// 添加SuperSocket会话
        /// </summary>
        Task<bool> AddSessionAsync(IAppSession session);

        /// <summary>
        /// 移除SuperSocket会话
        /// </summary>
        Task<bool> RemoveSessionAsync(string sessionId);

        #endregion

        #region 连接管理

        /// <summary>
        /// 主动断开指定会话连接（T人功能）
        /// </summary>
        /// <param name="sessionId">要断开的会话ID</param>
        /// <param name="reason">断开原因</param>
        /// <returns>断开是否成功</returns>
        Task<bool> DisconnectSessionAsync(string sessionId, string reason = "服务器强制断开");

        /// <summary>
        /// 主动断开指定用户的所有会话连接
        /// </summary>
        /// <param name="username">要断开的用户名</param>
        /// <param name="reason">断开原因</param>
        /// <returns>成功断开的会话数量</returns>
        Task<int> DisconnectUserSessionsAsync(string username, string reason = "服务器强制断开");

        #endregion

        #region 服务器主动发送命令

        /// <summary>
        /// 向指定会话发送命令并等待响应
        /// </summary>
        Task<PacketModel> SendCommandAndWaitForResponseAsync<TRequest>(
            string sessionID,
            CommandId commandId,
            TRequest request,
            int timeoutMs = 30000,
            CancellationToken ct = default)
            where TRequest : class, IRequest;

        /// <summary>
        /// 向指定会话发送命令（单向，不等待响应）
        /// </summary>
        Task<bool> SendCommandAsync<TRequest>(
            string sessionID,
            CommandId commandId,
            TRequest request,
            CancellationToken ct = default)
            where TRequest : class, IRequest;

        /// <summary>
        /// 核心数据包发送方法
        /// </summary>
        Task SendPacketCoreAsync<TRequest>(
            SessionInfo sessionInfo,
            CommandId commandId,
            TRequest request,
            int timeoutMs,
            CancellationToken ct = default,
            PacketDirection packetDirection = PacketDirection.ServerRequest,
            string responseTypeName = null)
            where TRequest : class, IRequest;

        #endregion

        #region 会话生命周期事件处理

        /// <summary>
        /// 处理会话连接事件
        /// </summary>
        ValueTask OnSessionConnectedAsync(IAppSession session);

        /// <summary>
        /// 处理会话断开事件
        /// </summary>
        ValueTask OnSessionClosedAsync(IAppSession session, CloseEventArgs closeReason);

        /// <summary>
        /// 用户认证成功事件处理
        /// </summary>
        Task OnUserAuthenticatedAsync(SessionInfo sessionInfo);

        /// <summary>
        /// 用户认证失败事件处理
        /// </summary>
        Task OnAuthenticationFailedAsync(SessionInfo sessionInfo, string reason);

        /// <summary>
        /// 会话超时事件处理
        /// </summary>
        Task OnSessionTimeoutAsync(SessionInfo sessionInfo);

        /// <summary>
        /// 会话错误事件处理
        /// </summary>
        Task OnSessionErrorAsync(SessionInfo sessionInfo, Exception error);

        #endregion

        #region 请求管理

        /// <summary>
        /// 尝试移除待处理请求
        /// </summary>
        bool TryRemovePendingRequest(string requestId, out TaskCompletionSource<PacketModel> taskCompletionSource);

        #endregion
    }
}
