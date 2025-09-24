using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 缓存数据同步服务
    /// 处理客户端与服务器之间的缓存数据同步功能
    /// </summary>
    public class CacheSyncService
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ClientCommandDispatcher _commandDispatcher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="commandDispatcher">命令调度器</param>
        public CacheSyncService(
            IClientCommunicationService communicationService,
            ClientCommandDispatcher commandDispatcher)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        }

        /// <summary>
        /// 请求缓存数据
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>缓存数据响应</returns>
        public async Task<ApiResponse<object>> RequestCacheDataAsync(
            string cacheKey,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备请求数据
                var requestData = new
                {
                    CacheKey = cacheKey,
                    Timestamp = DateTime.UtcNow
                };

                // 发送缓存请求命令并等待响应
                var response = await _communicationService.SendCommandAsync<object, object>(
                    CacheCommands.CacheRequest,
                    requestData,
                    cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure($"请求缓存数据过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 同步缓存数据
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="data">缓存数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>同步结果</returns>
        public async Task<ApiResponse<bool>> SyncCacheDataAsync(
            string cacheKey,
            object data,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备同步数据
                var syncData = new
                {
                    CacheKey = cacheKey,
                    Data = data,
                    Timestamp = DateTime.UtcNow
                };

                // 发送缓存同步命令
                var success = await _communicationService.SendOneWayCommandAsync(
                    CacheCommands.CacheSync,
                    syncData,
                    cancellationToken);

                if (success)
                {
                    return ApiResponse<bool>.CreateSuccess(true, "缓存数据同步成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure("缓存数据同步失败", 500);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"缓存数据同步过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 执行全量数据同步
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>同步结果</returns>
        public async Task<ApiResponse<bool>> FullSyncAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备全量同步数据
                var syncData = new
                {
                    SyncType = "Full",
                    Timestamp = DateTime.UtcNow
                };

                // 发送全量同步命令并等待响应
                var response = await _communicationService.SendCommandAsync<object, bool>(
                    DataSyncCommands.FullSync,
                    syncData,
                    cancellationToken);

                if (response.Success)
                {
                    return ApiResponse<bool>.CreateSuccess(response.Data, "全量数据同步成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure(response.Message, response.Code);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"全量数据同步过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 执行增量数据同步
        /// </summary>
        /// <param name="lastSyncTime">上次同步时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>同步结果</returns>
        public async Task<ApiResponse<bool>> IncrementalSyncAsync(
            DateTime lastSyncTime,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备增量同步数据
                var syncData = new
                {
                    LastSyncTime = lastSyncTime,
                    SyncType = "Incremental",
                    Timestamp = DateTime.UtcNow
                };

                // 发送增量同步命令并等待响应
                var response = await _communicationService.SendCommandAsync<object, bool>(
                    DataSyncCommands.IncrementalSync,
                    syncData,
                    cancellationToken);

                if (response.Success)
                {
                    return ApiResponse<bool>.CreateSuccess(response.Data, "增量数据同步成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure(response.Message, response.Code);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"增量数据同步过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 查询同步状态
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>同步状态信息</returns>
        public async Task<ApiResponse<object>> GetSyncStatusAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备状态查询数据
                var statusData = new
                {
                    QueryType = "SyncStatus",
                    Timestamp = DateTime.UtcNow
                };

                // 发送同步状态查询命令并等待响应
                var response = await _communicationService.SendCommandAsync<object, object>(
                    DataSyncCommands.SyncStatus,
                    statusData,
                    cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Failure($"查询同步状态过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 使缓存数据失效
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>失效结果</returns>
        public async Task<ApiResponse<bool>> InvalidateCacheAsync(
            string cacheKey,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备失效数据
                var invalidateData = new
                {
                    CacheKey = cacheKey,
                    Timestamp = DateTime.UtcNow
                };

                // 发送缓存失效命令
                var success = await _communicationService.SendOneWayCommandAsync(
                    CacheCommands.CacheInvalidate,
                    invalidateData,
                    cancellationToken);

                if (success)
                {
                    return ApiResponse<bool>.CreateSuccess(true, "缓存数据失效成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure("缓存数据失效失败", 500);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"缓存数据失效过程中发生异常: {ex.Message}", 500);
            }
        }
    }
}