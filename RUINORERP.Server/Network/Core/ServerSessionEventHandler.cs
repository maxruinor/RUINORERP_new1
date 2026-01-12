using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// 服务器会话事件处理器接口
    /// </summary>
    public interface IServerSessionEventHandler
    {
        /// <summary>
        /// 会话连接事件
        /// </summary>
        Task OnSessionConnectedAsync(SessionInfo sessionInfo);

        /// <summary>
        /// 会话断开事件
        /// </summary>
        Task OnSessionDisconnectedAsync(SessionInfo sessionInfo, string reason);

        /// <summary>
        /// 用户认证成功事件
        /// </summary>
        Task OnUserAuthenticatedAsync(SessionInfo sessionInfo);

        /// <summary>
        /// 用户认证失败事件
        /// </summary>
        Task OnAuthenticationFailedAsync(SessionInfo sessionInfo, string reason);

        /// <summary>
        /// 会话超时事件
        /// </summary>
        Task OnSessionTimeoutAsync(SessionInfo sessionInfo);

        /// <summary>
        /// 会话错误事件
        /// </summary>
        Task OnSessionErrorAsync(SessionInfo sessionInfo, Exception error);
    }

    /// <summary>
    /// 服务器会话事件处理器实现
    /// </summary>
    public class ServerSessionEventHandler : IServerSessionEventHandler
    {
        private readonly ILogger<ServerSessionEventHandler> _logger;
        private readonly ISessionService _sessionManager;

        public ServerSessionEventHandler(
            ILogger<ServerSessionEventHandler> logger,
            ISessionService sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// 会话连接事件
        /// </summary>
        public async Task OnSessionConnectedAsync(SessionInfo sessionInfo)
        {
            try
            {

                // 记录连接日志
                await LogSessionEventAsync("Connected", sessionInfo);

                // 可以在这里添加其他业务逻辑
                // 比如发送欢迎消息、检查IP黑名单等
                await SendWelcomeMessageAsync(sessionInfo);

                // 检查是否需要限制连接
                await CheckConnectionLimitsAsync(sessionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话连接事件失败: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 会话断开事件
        /// </summary>
        public async Task OnSessionDisconnectedAsync(SessionInfo sessionInfo, string reason)
        {
            try
            {

                // 记录断开日志
                await LogSessionEventAsync("Disconnected", sessionInfo, reason);

                // 清理会话相关资源
                await CleanupSessionResourcesAsync(sessionInfo);

                // 如果是认证用户，记录用户下线
                if (sessionInfo.IsAuthenticated && sessionInfo.UserId.HasValue)
                {
                    await LogUserOfflineAsync(sessionInfo.UserId.ToString(), reason);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话断开事件失败: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 用户认证成功事件
        /// </summary>
        public async Task OnUserAuthenticatedAsync(SessionInfo sessionInfo)
        {
            try
            {
                // 记录认证成功日志
                await LogSessionEventAsync("Authenticated", sessionInfo);

                // 检查是否有重复登录
                await CheckDuplicateLoginAsync(sessionInfo);

                // 发送认证成功消息
                await SendAuthenticationSuccessMessageAsync(sessionInfo);

                // 更新用户在线状态
                await UpdateUserOnlineStatusAsync(sessionInfo.UserId?.ToString(), true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理用户认证成功事件失败: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 用户认证失败事件
        /// </summary>
        public async Task OnAuthenticationFailedAsync(SessionInfo sessionInfo, string reason)
        {
            try
            {
                _logger.LogWarning($"用户认证失败: SessionId={sessionInfo.SessionID}, 原因={reason}");

                // 记录认证失败日志
                await LogSessionEventAsync("AuthenticationFailed", sessionInfo, reason);

                // 检查是否需要限制IP
                await CheckAuthenticationFailuresAsync(sessionInfo);

                // 发送认证失败消息
                await SendAuthenticationFailureMessageAsync(sessionInfo, reason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理用户认证失败事件失败: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 会话超时事件
        /// </summary>
        public async Task OnSessionTimeoutAsync(SessionInfo sessionInfo)
        {
            try
            {
                _logger.LogWarning($"会话超时: SessionId={sessionInfo.SessionID}");

                // 记录超时日志
                await LogSessionEventAsync("Timeout", sessionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话超时事件失败: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 会话错误事件
        /// </summary>
        public async Task OnSessionErrorAsync(SessionInfo sessionInfo, Exception error)
        {
            try
            {
                _logger.LogError(error, $"会话错误: SessionId={sessionInfo.SessionID}");

                // 记录错误日志
                await LogSessionEventAsync("Error", sessionInfo, error.Message);

                // 根据错误类型决定是否断开连接
                if (IsFatalError(error))
                {
                    //await _sessionManager.DisconnectSessionAsync(sessionInfo.SessionID, "严重错误");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理会话错误事件失败: {sessionInfo.SessionID}");
            }
        }

        /// <summary>
        /// 记录会话事件日志
        /// </summary>
        private async Task LogSessionEventAsync(string eventType, SessionInfo sessionInfo, string additionalInfo = null)
        {
            try
            {
                // 这里可以将事件记录到数据库或其他持久化存储
                var logMessage = $"SessionEvent: {eventType}, SessionId: {sessionInfo.SessionID}, " +
                               $"IP: {sessionInfo.ClientIp}, UserId: {sessionInfo.UserId}";

                if (!string.IsNullOrEmpty(additionalInfo))
                {
                    logMessage += $", Info: {additionalInfo}";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录会话事件日志失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 发送欢迎消息
        /// </summary>
        private async Task SendWelcomeMessageAsync(SessionInfo sessionInfo)
        {
            try
            {
                // 这里可以发送欢迎消息给客户端
                _logger.LogDebug($"发送欢迎消息: {sessionInfo.SessionID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送欢迎消息失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 检查连接限制
        /// </summary>
        private async Task CheckConnectionLimitsAsync(SessionInfo sessionInfo)
        {
            try
            {
                // 检查IP连接数限制
                var stats = _sessionManager.GetStatistics();
                if (stats.TotalConnections > stats.MaxConnections) // 假设最大1000个连接
                {
                    _logger.LogWarning($"连接数超过限制: {stats.MaxConnections}");
                    // 可以考虑拒绝新连接或断开最老的连接
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查连接限制失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 清理会话资源
        /// </summary>
        private async Task CleanupSessionResourcesAsync(SessionInfo sessionInfo)
        {
            try
            {
                // 清理会话相关的资源
                // 比如临时文件、缓存数据等
                _logger.LogDebug($"清理会话资源: {sessionInfo.SessionID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理会话资源失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 记录用户下线
        /// </summary>
        private async Task LogUserOfflineAsync(string userId, string reason)
        {
            try
            {
                // 这里可以更新用户状态到数据库
                await UpdateUserOnlineStatusAsync(userId, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录用户下线失败");
            }
        }

        /// <summary>
        /// 检查重复登录
        /// </summary>
        private async Task CheckDuplicateLoginAsync(SessionInfo sessionInfo)
        {
            try
            {
                //var userSessions = _sessionManager.GetSession(sessionInfo.SessionID);
                //var activeSessions = userSessions.Count();

                //if (activeSessions > 1)
                //{
                    
                //    // 根据业务规则处理重复登录
                //    // 可以选择断开之前的会话或者拒绝新登录
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查重复登录失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 发送认证成功消息
        /// </summary>
        private async Task SendAuthenticationSuccessMessageAsync(SessionInfo sessionInfo)
        {
            try
            {
                _logger.LogDebug($"发送认证成功消息: {sessionInfo.SessionID}");
                // 这里可以发送认证成功的响应给客户端
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送认证成功消息失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 更新用户在线状态
        /// </summary>
        private async Task UpdateUserOnlineStatusAsync(string userId, bool isOnline)
        {
            try
            {
                _logger.LogDebug($"更新用户在线状态: UserId={userId}, IsOnline={isOnline}");
                // 这里可以调用用户服务更新在线状态
                // await _userService.UpdateOnlineStatusAsync(userId, isOnline);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用户在线状态失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 检查认证失败次数
        /// </summary>
        private async Task CheckAuthenticationFailuresAsync(SessionInfo sessionInfo)
        {
            try
            {
                // 这里可以实现IP黑名单逻辑
                // 比如连续认证失败超过限制次数，就加入黑名单
                _logger.LogDebug($"检查认证失败次数: {sessionInfo.ClientIp}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查认证失败次数失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 发送认证失败消息
        /// </summary>
        private async Task SendAuthenticationFailureMessageAsync(SessionInfo sessionInfo, string reason)
        {
            try
            {
                _logger.LogDebug($"发送认证失败消息: {sessionInfo.SessionID}, 原因: {reason}");
                // 这里可以发送认证失败的响应给客户端
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送认证失败消息失败");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 判断是否为致命错误
        /// </summary>
        private bool IsFatalError(Exception error)
        {
            // 这里可以根据异常类型判断是否为致命错误
            return error is OutOfMemoryException || 
                   error is StackOverflowException;
        }
    }
}