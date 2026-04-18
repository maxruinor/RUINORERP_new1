using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models;
using RUINORERP.UI.Network;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Cache;

namespace RUINORERP.UI.Network.Services.Cache
{
    /// <summary>
    /// 缓存请求管理器 - 负责处理与服务器的缓存数据请求逻辑
    /// 注意：此类是对业务层缓存功能的UI层封装，提供更友好的请求接口
    /// </summary>
    public class CacheRequestManager : CacheValidationBase, IDisposable
    {
        private readonly ILogger<CacheRequestManager> _log;
        private readonly ClientCommunicationService _communicationService;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ICacheSyncMetadata _cacheSyncMetadata; // 缓存同步元数据管理器
        private readonly ConcurrentDictionary<string, DateTime> _lastRequestTimes = new ConcurrentDictionary<string, DateTime>();
        private const int RequestIntervalSeconds = 5; // 请求间隔限制
        private const int MaxCacheAgeMinutes = 30; // 缓存最大有效时间（分钟）

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheRequestManager(
            ILogger<CacheRequestManager> log, 
            ClientCommunicationService communicationService, 
            IEntityCacheManager cacheManager,
            ICacheSyncMetadata cacheSyncMetadata)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _cacheSyncMetadata = cacheSyncMetadata ?? throw new ArgumentNullException(nameof(cacheSyncMetadata));
        }

        /// <summary>
        /// 向服务器请求指定表的缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task RequestCacheAsync(string tableName, CancellationToken cancellationToken = default)
        {
            var validationResult = base.ValidateTableName(tableName);
            if (!validationResult.IsValid)
            {
                _log.LogError(validationResult.GetValidationErrors());
                return;
            }

            // 检查取消令牌
            cancellationToken.ThrowIfCancellationRequested();

            // 检查限流和缓存有效性，任一条件不满足则跳过请求
            if (!CheckRequestFrequency(tableName, CacheOperation.Get) || IsLocalCacheValid(tableName))
            {
                return;
            }

            // 创建Get操作的请求并执行，优化为单步处理
            var response = await ProcessCacheOperationAsync(CacheCommands.CacheOperation, new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Get
            }, cancellationToken);

            // 利用业务层缓存管理器更新缓存（如果响应成功）
            if (response?.IsSuccess == true && response.CacheData != null)
            {
                _cacheManager.UpdateEntityList(tableName, response.CacheData.GetData());
            }
        }


        /// <summary>
        /// 用户登录成功时向服务器请求所有缓存数据的元数据信息
        /// 
        /// 【设计思路】：采用"轻量元数据协调 + 重量数据传输"的分层同步策略
        /// 1. 先获取元数据（几KB），快速了解服务器有哪些表的缓存及其状态
        /// 2. 客户端比对本地版本，决策需要同步哪些表
        /// 3. 按需请求实际数据（避免盲目全量同步）
        /// 
        /// 【为什么不用 CacheSync 直接获取所有数据？】
        /// - 如果有100个表，直接同步可能几百MB，登录会很慢
        /// - 使用元数据同步，只同步变化的表，节省带宽和时间
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RequestAllCacheSyncMetadataAsync(CancellationToken cancellationToken = default)
        {

            // 检查取消令牌
            cancellationToken.ThrowIfCancellationRequested();

            // 创建Get操作的请求并执行，优化为单步处理
            var cacheRequest = new CacheMetadataSyncRequest
            {
                Operation = CacheSyncOperation.FullSync
            };

            try
            {
                // 【修复】：使用正确的命令 ID - CacheMetadataSync 而非 CacheSync
                var cacheMetadata = await _communicationService.SendCommandWithResponseAsync<CacheMetadataSyncResponse>(
                    CacheCommands.CacheMetadataSync,  // 修正：使用 CacheMetadataSync 命令
                    cacheRequest, 
                    cancellationToken);
                
                if (cacheMetadata != null && cacheMetadata.IsSuccess)
                {
                    _log.LogInformation("成功获取缓存元数据，表数量={Count}", cacheMetadata.CacheMetadataData?.Count ?? 0);
                    
                    // 更新本地缓存元数据
                    if (_cacheSyncMetadata != null && cacheMetadata.CacheMetadataData != null)
                    {
                        _cacheSyncMetadata.BatchUpdateSyncMetadata(cacheMetadata.CacheMetadataData, overwriteExisting: true);
                        _log.LogDebug("已更新本地缓存元数据，共 {Count} 个表", cacheMetadata.CacheMetadataData.Count);
                    }
                }
                else
                {
                    _log.LogWarning("获取缓存元数据失败或返回空数据");
                }
            }
            catch (OperationCanceledException oex)
            {
                _log.LogDebug("缓存请求被取消，ex={0}, 操作类型={1}", oex.Message, cacheRequest.Operation);
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "请求缓存失败，ex={0}, 操作类型={1}", ex.Message, cacheRequest.Operation);
            }


        }


        /// <summary>
        /// 向服务器发送缓存管理请求
        /// </summary>
        public async Task<CacheResponse> SendCacheSubscriptionAsync(string tableName, SubscribeAction operationType, Dictionary<string, object> parameters = null)
        {
            var validationResult = base.ValidateTableName(tableName);
            if (!validationResult.IsValid)
            {
                _log.LogError(validationResult.GetValidationErrors());
                return null;
            }

            // 创建请求并设置参数，优化为一步完成
            return await ProcessCacheOperationAsync(CacheCommands.CacheSubscription, new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Manage,
                SubscribeAction = operationType,
                Parameters = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>())
                {
                    { "OperationType", operationType }
                }
            });
        }

        /// <summary>
        /// 统一处理缓存请求
        /// </summary>
        /// <param name="command">命令ID 区别是同步，还是操作</param>
        /// <param name="request">请求参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        internal async Task<CacheResponse> ProcessCacheOperationAsync(CommandId command, CacheRequest request, CancellationToken cancellationToken = default)
        {
            // 参数验证
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // 检查取消令牌
            cancellationToken.ThrowIfCancellationRequested();

            // 检查请求频率
            if (!CheckRequestFrequency(request.TableName, request.Operation))
            {
                return null;
            }

            // 记录请求时间
            UpdateLastRequestTime(request.TableName, request.Operation);

            try
            {
                // 不再需要检查连接状态，直接发送请求
                // ClientCommunicationService会自动处理连接状态和离线队列
                return await _communicationService.SendCommandWithResponseAsync<CacheResponse>(command, request, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "请求缓存失败，表名={0}, 操作类型={1}", request.TableName, request.Operation);

                // 直接返回错误响应，不再需要特殊处理连接异常
                return new CacheResponse { IsSuccess = false, ErrorMessage = $"处理缓存请求失败: {ex.Message}" };
            }
        }

        /// <summary>
        /// 检查请求频率(带操作类型)
        /// </summary>
        private bool CheckRequestFrequency(string tableName, CacheOperation operation)
        {
            string key = $"{tableName}_{operation}";
            if (_lastRequestTimes.TryGetValue(key, out DateTime lastRequestTime))
            {
                var timeSpan = DateTime.Now - lastRequestTime;
                if (timeSpan.TotalSeconds < RequestIntervalSeconds)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查本地缓存是否有效
        /// </summary>
        private bool IsLocalCacheValid(string tableName)
        {
            try
            {
                // 检查缓存是否存在且有数据
                var rslist = _cacheManager.GetEntityList<object>(tableName);
                if (rslist == null || rslist.Count == 0)
                    return false;

                // 检查缓存是否过期
                string cacheKey = $"{tableName}_lastUpdated";
                if (_lastRequestTimes.TryGetValue(cacheKey, out DateTime lastUpdated))
                {
                    if ((DateTime.Now - lastUpdated).TotalMinutes > MaxCacheAgeMinutes)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "检查本地缓存有效性失败，表名={0}", tableName);
                return false;
            }
        }

        /// <summary>
        /// 更新表的最后请求时间(带操作类型)
        /// </summary>
        internal void UpdateLastRequestTime(string tableName, CacheOperation operation)
        {
            string key = $"{tableName}_{operation}";
            _lastRequestTimes[key] = DateTime.Now;

            // 同时更新表的最后更新时间（用于缓存过期检查）
            string updateKey = $"{tableName}_lastUpdated";
            _lastRequestTimes[updateKey] = DateTime.Now;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _lastRequestTimes.Clear();
        }
    }
}