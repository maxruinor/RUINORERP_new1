using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces;
using RUINORERP.Server.Network.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 通用广播服务实现
    /// 负责向客户端广播通用请求数据
    /// </summary>
    public class GeneralBroadcastService : IGeneralBroadcastService
    {
        private readonly ISessionService _sessionManager;
        private readonly ILogger<GeneralBroadcastService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionManager">会话管理器</param>
        /// <param name="logger">日志记录器</param>
        public GeneralBroadcastService(ISessionService sessionManager, ILogger<GeneralBroadcastService> logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// 向所有客户端广播请求数据（无需等待响应）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>异步任务</returns>
        public async Task BroadcastToAllClients(CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取所有活跃会话
                var activeSessions = _sessionManager.GetAllUserSessions();
                int broadcastCount = 0;
                int failedCount = 0;

                // 向每个会话发送请求数据
                foreach (var session in activeSessions)
                {
                    try
                    {
                        // 使用SessionService中的SendCommandAsync方法发送请求（无需等待响应）
                        await _sessionManager.SendCommandAsync(session.SessionID, commandId, request);
                        broadcastCount++;
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogWarning(ex, $"向会话 {session.SessionID} 广播请求数据失败");
                    }
                }

                _logger.Debug($"请求数据广播完成 - 成功: {broadcastCount}, 失败: {failedCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "向所有客户端广播请求数据时发生错误");
                // 不抛出异常，避免影响主流程
            }
        }

        /// <summary>
        /// 向特定会话广播请求数据（无需等待响应）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>异步任务</returns>
        public async Task BroadcastToSession(string sessionId, CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取指定会话
                var session = _sessionManager.GetSession(sessionId);
                if (session != null)
                {
                    // 使用SessionService中的SendCommandAsync方法发送请求（无需等待响应）
                    await _sessionManager.SendCommandAsync(sessionId, CommandId.FromUInt16(commandId), request);
                    _logger.LogInformation($"向会话 {sessionId} 广播请求数据成功");
                }
                else
                {
                    _logger.LogWarning($"会话 {sessionId} 不存在，无法广播请求数据");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"向会话 {sessionId} 广播请求数据时发生错误");
                // 不抛出异常，避免影响主流程
            }
        }

        /// <summary>
        /// 向特定用户组广播请求数据（无需等待响应）
        /// </summary>
        /// <param name="userGroup">用户组</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>异步任务</returns>
        public async Task BroadcastToUserGroup(string userGroup, CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取所有活跃会话
                var activeSessions = _sessionManager.GetAllUserSessions();
                int broadcastCount = 0;
                int failedCount = 0;

                // 向指定用户组的会话发送请求数据
                foreach (var session in activeSessions.Where(s => s.UserInfo?.UserGroup == userGroup))
                {
                    try
                    {
                        // 使用SessionService中的SendCommandAsync方法发送请求（无需等待响应）
                        await _sessionManager.SendCommandAsync(session.SessionID, CommandId.FromUInt16(commandId), request);
                        broadcastCount++;
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogWarning(ex, $"向用户组 {userGroup} 的会话 {session.SessionID} 广播请求数据失败");
                    }
                }

                _logger.LogInformation($"向用户组 {userGroup} 广播请求数据完成 - 成功: {broadcastCount}, 失败: {failedCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"向用户组 {userGroup} 广播请求数据时发生错误");
                // 不抛出异常，避免影响主流程
            }
        }

        /// <summary>
        /// 向所有客户端发送请求并等待响应
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据列表</returns>
        public async Task<GeneralResponse[]> SendRequestToAllClients(CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取所有活跃会话
                var activeSessions = _sessionManager.GetAllUserSessions();
                var responses = new List<GeneralResponse>();
                int successCount = 0;
                int failedCount = 0;

                // 向每个会话发送请求并等待响应
                foreach (var session in activeSessions)
                {
                    try
                    {
                        // 使用SessionService中的SendCommandAndWaitForResponseAsync方法发送请求并等待响应
                        var response = await _sessionManager.SendCommandAndWaitForResponseAsync(session.SessionID, CommandId.FromUInt16(commandId), request);
                        if (response?.Response is GeneralResponse generalResponse)
                        {
                            responses.Add(generalResponse);
                            successCount++;
                        }
                        else
                        {
                            failedCount++;
                            _logger.LogWarning($"从会话 {session.SessionID} 接收到无效响应");
                        }
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogWarning(ex, $"向会话 {session.SessionID} 发送请求并等待响应失败");
                    }
                }

                _logger.LogInformation($"请求发送完成 - 成功: {successCount}, 失败: {failedCount}");
                return responses.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "向所有客户端发送请求并等待响应时发生错误");
                // 不抛出异常，避免影响主流程
                return new GeneralResponse[0];
            }
        }

        /// <summary>
        /// 向特定会话发送请求并等待响应
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据</returns>
        public async Task<GeneralResponse> SendRequestToSession(string sessionId, CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取指定会话
                var session = _sessionManager.GetSession(sessionId);
                if (session != null)
                {
                    // 使用SessionService中的SendCommandAndWaitForResponseAsync方法发送请求并等待响应
                    var response = await _sessionManager.SendCommandAndWaitForResponseAsync(sessionId, CommandId.FromUInt16(commandId), request);
                    if (response?.Response is GeneralResponse generalResponse)
                    {
                        _logger.LogInformation($"向会话 {sessionId} 发送请求并接收响应成功");
                        return generalResponse;
                    }
                    else
                    {
                        _logger.LogWarning($"从会话 {sessionId} 接收到无效响应");
                        return null;
                    }
                }
                else
                {
                    _logger.LogWarning($"会话 {sessionId} 不存在，无法发送请求");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"向会话 {sessionId} 发送请求并等待响应时发生错误");
                // 不抛出异常，避免影响主流程
                return null;
            }
        }


        /// <summary>
        /// 向特定会话发送请求并等待响应
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据</returns>
        public async Task<List<GeneralResponse>> SendRequestToSession(CommandId commandId, GeneralRequest request)
        {
            List<GeneralResponse> GeneralResponseList = new List<GeneralResponse>();
            try
            {
                var sessions = _sessionManager.GetAllUserSessions();
                foreach (var session in sessions)
                {
                    if (session != null)
                    {
                        // 使用SessionService中的SendCommandAndWaitForResponseAsync方法发送请求并等待响应
                        var response = await _sessionManager.SendCommandAndWaitForResponseAsync(session.SessionID, CommandId.FromUInt16(commandId), request);
                        if (response?.Response is GeneralResponse generalResponse)
                        {
                            _logger.LogInformation($"向会话 {session.SessionID} 发送请求并接收响应成功");
                            GeneralResponseList.Add(generalResponse);
                        }
                        else
                        {
                            _logger.LogWarning($"从会话 {session.SessionID} 接收到无效响应");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"会话 {session.SessionID} 不存在，无法发送请求");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"发送请求并等待响应时发生错误");
                // 不抛出异常，避免影响主流程
            }
            return GeneralResponseList;
        }


        /// <summary>
        /// 向特定用户组发送请求并等待响应
        /// </summary>
        /// <param name="userGroup">用户组</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据列表</returns>
        public async Task<GeneralResponse[]> SendRequestToUserGroup(string userGroup, CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取所有活跃会话
                var activeSessions = _sessionManager.GetAllUserSessions();
                var responses = new List<GeneralResponse>();
                int successCount = 0;
                int failedCount = 0;

                // 向指定用户组的会话发送请求并等待响应
                foreach (var session in activeSessions.Where(s => s.UserInfo?.UserGroup == userGroup))
                {
                    try
                    {
                        // 使用SessionService中的SendCommandAndWaitForResponseAsync方法发送请求并等待响应
                        var response = await _sessionManager.SendCommandAndWaitForResponseAsync(session.SessionID, CommandId.FromUInt16(commandId), request);
                        if (response?.Response is GeneralResponse generalResponse)
                        {
                            responses.Add(generalResponse);
                            successCount++;
                        }
                        else
                        {
                            failedCount++;
                            _logger.LogWarning($"从用户组 {userGroup} 的会话 {session.SessionID} 接收到无效响应");
                        }
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogWarning(ex, $"向用户组 {userGroup} 的会话 {session.SessionID} 发送请求并等待响应失败");
                    }
                }

                _logger.LogInformation($"向用户组 {userGroup} 发送请求完成 - 成功: {successCount}, 失败: {failedCount}");
                return responses.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"向用户组 {userGroup} 发送请求并等待响应时发生错误");
                // 不抛出异常，避免影响主流程
                return new GeneralResponse[0];
            }
        }
    }
}