using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Cache;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 缓存元数据同步服务
    /// 负责在用户登录成功后将服务器的缓存元数据同步到客户端
    /// </summary>
    public class CacheMetadataSyncService
    {
        private readonly ISessionService _sessionService;
        private readonly CacheSyncMetadataManager _cacheSyncMetadataManager;
        private readonly ILogger<CacheMetadataSyncService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionService">会话服务</param>
        /// <param name="cacheSyncMetadataManager">缓存同步元数据管理器</param>
        /// <param name="logger">日志记录器</param>
        public CacheMetadataSyncService(
            ISessionService sessionService,
            CacheSyncMetadataManager cacheSyncMetadataManager,
            ILogger<CacheMetadataSyncService> logger)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _cacheSyncMetadataManager = cacheSyncMetadataManager ?? throw new ArgumentNullException(nameof(cacheSyncMetadataManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 向指定客户端同步缓存元数据
        /// 通常在用户登录成功后调用
        /// </summary>
        /// <param name="sessionId">客户端会话ID</param>
        /// <param name="forceOverwrite">是否强制覆盖客户端现有数据</param>
        /// <returns>同步任务</returns>
        public async Task<bool> SyncCacheMetadataToClientAsync(SessionInfo sessionInfo, bool forceOverwrite = false)
        {
            try
            {
                // 获取当前服务器的所有缓存元数据
                var serverSyncData = _cacheSyncMetadataManager.GetAllTableSyncInfo();

                if (serverSyncData == null || serverSyncData.Count == 0)
                {
                    _logger?.LogInformation("服务器缓存元数据为空，跳过同步操作");
                    return true; // 不视为错误
                }

                // 创建同步请求
                var syncRequest = new CacheMetadataSyncRequest(
                    serverSyncData,
                    CacheSyncOperation.FullSync,
                    forceOverwrite)
                {
                    ServerSessionId = sessionInfo.SessionID,
                };

                // 发送同步命令到客户端
                await _sessionService.SendPacketCoreAsync<CacheMetadataSyncRequest>(
                    sessionInfo,
                    CacheCommands.CacheMetadataSync,
                    syncRequest, 10000);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "向会话 {SessionId} 同步缓存元数据时发生错误", sessionInfo.SessionID);
                return false;
            }
        }

        /// <summary>
        /// 向所有在线客户端广播缓存元数据更新
        /// 当服务器缓存发生重大变化时调用
        /// </summary>
        /// <param name="forceOverwrite">是否强制覆盖客户端现有数据</param>
        /// <returns>同步任务</returns>
        public async Task<int> BroadcastCacheMetadataUpdateAsync(bool forceOverwrite = true)
        {
            try
            {
                var sessions = _sessionService.GetAllUserSessions();
                if (sessions == null || sessions.Count() == 0)
                {
                    _logger?.LogInformation("没有在线用户，跳过缓存元数据广播");
                    return 0;
                }

                int successCount = 0;
                int totalCount = sessions.Count();

                _logger?.LogInformation("开始向 {UserCount} 个在线用户广播缓存元数据更新", totalCount);

                // 并行发送同步请求
                var tasks = new Task<bool>[totalCount];
                for (int i = 0; i < totalCount; i++)
                {
                    var session = sessions.ToArray()[i];
                    tasks[i] = SyncCacheMetadataToClientAsync(session, forceOverwrite);
                }

                // 等待所有任务完成
                var results = await Task.WhenAll(tasks);

                // 统计成功数量
                successCount = 0;
                for (int i = 0; i < totalCount; i++)
                {
                    if (results[i])
                    {
                        successCount++;
                    }
                }

                _logger?.LogInformation("缓存元数据广播完成，成功 {SuccessCount}/{TotalCount} 个用户",
                    successCount, totalCount);

                return successCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播缓存元数据更新时发生错误");
                return 0;
            }
        }

        /// <summary>
        /// 处理客户端的缓存元数据同步响应
        /// </summary>
        /// <param name="sessionId">客户端会话ID</param>
        /// <param name="response">客户端响应</param>
        public void HandleClientSyncResponse(string sessionId, CacheMetadataSyncResponse response)
        {
            try
            {
                if (response == null)
                {
                    _logger?.LogWarning("会话 {SessionId} 发送的缓存元数据同步响应为空", sessionId);
                    return;
                }

                if (response.IsSuccess)
                {
                    _logger?.LogInformation("会话 {SessionId} 缓存元数据同步成功：更新 {UpdatedCount} 个表，跳过 {SkippedCount} 个表",
                        sessionId, response.UpdatedCount, response.SkippedCount);
                }
                else
                {
                    _logger?.LogError("会话 {SessionId} 缓存元数据同步失败：{Error}",
                        sessionId, response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理会话 {SessionId} 的缓存元数据同步响应时发生错误", sessionId);
            }
        }
    }
}