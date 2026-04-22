using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces;
using RUINORERP.Server.Network.Interfaces.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

                // P0-1修复: 改为并行发送，限制并发度避免瞬间占满连接池
                var maxConcurrency = Math.Min(activeSessions.Count(), 20);
                using var semaphore = new SemaphoreSlim(maxConcurrency);
                var tasks = activeSessions.Select(async session =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await _sessionManager.SendCommandAsync(session.SessionID, commandId, request);
                        Interlocked.Increment(ref broadcastCount);
                    }
                    catch (Exception ex)
                    {
                        Interlocked.Increment(ref failedCount);
                        _logger.LogWarning(ex, $"向会话 {session.SessionID} 广播请求数据失败");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }).ToArray();

                // 等待所有发送任务完成，最多等待5秒
                try
                {
                    await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(5));
                }
                catch (TimeoutException)
                {
                    _logger.LogWarning("广播发送超时，部分会话可能未收到消息");
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
                var groupSessions = activeSessions.Where(s => s.UserInfo?.UserGroup == userGroup).ToList();
                int broadcastCount = 0;
                int failedCount = 0;

                if (groupSessions.Count == 0)
                {
                    _logger.LogInformation($"用户组 {userGroup} 没有活跃会话，跳过广播");
                    return;
                }

                // P0-1修复: 改为并行发送，限制并发度避免瞬间占满连接池
                var maxConcurrency = Math.Min(groupSessions.Count, 20);
                using var semaphore = new SemaphoreSlim(maxConcurrency);
                var tasks = groupSessions.Select(async session =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await _sessionManager.SendCommandAsync(session.SessionID, CommandId.FromUInt16(commandId), request);
                        Interlocked.Increment(ref broadcastCount);
                    }
                    catch (Exception ex)
                    {
                        Interlocked.Increment(ref failedCount);
                        _logger.LogWarning(ex, $"向用户组 {userGroup} 的会话 {session.SessionID} 广播请求数据失败");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }).ToArray();

                // 等待所有发送任务完成，最多等待5秒
                try
                {
                    await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(5));
                }
                catch (TimeoutException)
                {
                    _logger.LogWarning($"向用户组 {userGroup} 广播发送超时，部分会话可能未收到消息");
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
        /// 向所有客户端发送请求并等待响应（优化版：并行+超时隔离）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据列表</returns>
        public async Task<GeneralResponse[]> SendRequestToAllClients(CommandId commandId, GeneralRequest request)
        {
            try
            {
                // 获取所有活跃会话
                var activeSessions = _sessionManager.GetAllUserSessions().ToList();
                const int REQUEST_TIMEOUT_MS = 10000;
                var responses = new List<GeneralResponse>();

                if (activeSessions.Count == 0)
                {
                    return responses.ToArray();
                }

                // P0-1修复: 改为并行发送，每个请求独立超时，互不影响
                var maxConcurrency = Math.Min(activeSessions.Count, 20);
                using var semaphore = new SemaphoreSlim(maxConcurrency);
                var tasks = activeSessions.Select(async session =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        using var cts = new CancellationTokenSource(REQUEST_TIMEOUT_MS);
                        var response = await _sessionManager.SendCommandAndWaitForResponseAsync(
                            session.SessionID,
                            CommandId.FromUInt16(commandId),
                            request,
                            REQUEST_TIMEOUT_MS,
                            cts.Token);

                        if (response?.Response is GeneralResponse generalResponse)
                        {
                            responses.Add(generalResponse);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogWarning($"会话 {session.SessionID} 请求超时");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"向会话 {session.SessionID} 发送请求失败");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }).ToArray();

                // 等待所有响应，最多等待15秒
                try
                {
                    await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(15));
                }
                catch (TimeoutException)
                {
                    _logger.LogWarning("双向广播等待响应超时");
                }

                _logger.LogInformation($"请求发送完成 - 成功: {responses.Count}, 会话总数: {activeSessions.Count}");
                return responses.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "向所有客户端发送请求并等待响应时发生错误");
                return Array.Empty<GeneralResponse>();
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
                        var response = await _sessionManager.SendCommandAndWaitForResponseAsync(session.SessionID, commandId, request);
                        if (response?.Response is GeneralResponse generalResponse)
                        {
                            _logger.Debug($"向会话 {session.SessionID} 发送请求并接收响应成功");
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
        /// 向特定用户组发送请求并等待响应（优化版：并行+超时隔离）
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
                var groupSessions = activeSessions.Where(s => s.UserInfo?.UserGroup == userGroup).ToList();
                const int REQUEST_TIMEOUT_MS = 10000;
                var responses = new ConcurrentBag<GeneralResponse>();

                if (groupSessions.Count == 0)
                {
                    _logger.LogInformation($"用户组 {userGroup} 没有活跃会话，跳过请求");
                    return responses.ToArray();
                }

                // P0-1修复: 改为并行发送，每个请求独立超时
                var maxConcurrency = Math.Min(groupSessions.Count, 20);
                using var semaphore = new SemaphoreSlim(maxConcurrency);
                var tasks = groupSessions.Select(async session =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        using var cts = new CancellationTokenSource(REQUEST_TIMEOUT_MS);
                        var response = await _sessionManager.SendCommandAndWaitForResponseAsync(
                            session.SessionID,
                            CommandId.FromUInt16(commandId),
                            request,
                            REQUEST_TIMEOUT_MS,
                            cts.Token);

                        if (response?.Response is GeneralResponse generalResponse)
                        {
                            responses.Add(generalResponse);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogWarning($"用户组 {userGroup} 会话 {session.SessionID} 请求超时");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"向用户组 {userGroup} 的会话 {session.SessionID} 发送请求失败");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }).ToArray();

                // 等待所有响应，最多等待15秒
                try
                {
                    await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(15));
                }
                catch (TimeoutException)
                {
                    _logger.LogWarning($"向用户组 {userGroup} 广播等待响应超时");
                }

                _logger.LogInformation($"向用户组 {userGroup} 发送请求完成 - 成功: {responses.Count}, 会话总数: {groupSessions.Count}");
                return responses.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"向用户组 {userGroup} 发送请求并等待响应时发生错误");
                return Array.Empty<GeneralResponse>();
            }
        }
    }
}