using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Cache;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 缓存命令处理器 - 处理客户端的缓存请求
    /// 包括缓存数据的获取、发送和管理
    /// </summary>
    [CommandHandler("CacheCommandHandler", priority: 20)]
    public class CacheCommandHandler : BaseCommandHandler
    {
        private readonly ISessionService _sessionService;
        private readonly CacheSubscriptionManager _subscriptionManager; // 使用统一的订阅管理器
        private readonly CacheSyncMetadataManager _cacheSyncMetadataManager;
        protected ILogger<CacheCommandHandler> logger { get; set; }
        // 添加新的缓存管理器字段
        private readonly IEntityCacheManager _cacheManager;
        private readonly ICacheSyncMetadata _syncMetadata;

        // 添加构造函数注入CacheSubscriptionManager
        /// <summary>
        /// 构造函数 - 通过DI注入依赖项
        /// </summary>
        public CacheCommandHandler(
            ILogger<CacheCommandHandler> logger,
            ISessionService sessionService,
            CacheSubscriptionManager subscriptionManager, // 通过DI注入
            IEntityCacheManager cacheManager, // 通过DI注入
                 CacheSyncMetadataManager cacheSyncMetadataManager,
            ICacheSyncMetadata syncMetadata) // 通过DI注入
            : base(logger)
        {
            this.logger = logger;
            _sessionService = sessionService;
            _subscriptionManager = subscriptionManager;
            _cacheSyncMetadataManager = cacheSyncMetadataManager ?? throw new ArgumentNullException(nameof(cacheSyncMetadataManager));
            _cacheManager = cacheManager;
            _syncMetadata = syncMetadata;

            // 使用安全方法设置支持的命令 - 适配简化的缓存命令系统
            SetSupportedCommands(
                CacheCommands.CacheOperation,
                CacheCommands.CacheSync,
                CacheCommands.CacheMetadataSync,
                CacheCommands.CacheSubscription
            );
        }



        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="cmd">队列命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                // 使用字典映射替代冗长的if-else链 - 适配简化的缓存命令系统
                var commandHandlers = new Dictionary<CommandId, Func<QueuedCommand, CancellationToken, Task<IResponse>>>
                {
                    // 统一处理缓存操作命令，内部根据Operation类型区分具体操作
                    { CacheCommands.CacheOperation, HandleCacheOperationAsync },
                    // 处理缓存同步命令
                    { CacheCommands.CacheSync, HandleCacheSyncAsync },

                    { CacheCommands.CacheMetadataSync, HandleCacheMetadataSync },

                    // 处理缓存订阅命令，内部根据SubscribeAction区分订阅和取消订阅
                    { CacheCommands.CacheSubscription, HandleCacheSubscriptionAsync }
                };

                if (commandHandlers.TryGetValue(commandId, out var handler))
                {
                    return await handler(cmd, cancellationToken);
                }

                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的缓存命令类型: {commandId.ToString()}", UnifiedErrorCodes.Command_NotFound);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存命令异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"处理缓存命令异常: {ex.Message}", UnifiedErrorCodes.System_InternalError);
            }
        }

        /// <summary>
        /// 处理缓存操作命令 - 【查询入口】
        /// 职责：处理客户端发起的 CacheQuery (Get) 请求。
        /// </summary>
        private async Task<IResponse> HandleCacheOperationAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (!(cmd.Packet.Request is CacheRequest cacheCommand))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的缓存命令格式", UnifiedErrorCodes.Command_ValidationFailed);
                }

                // 仅处理 Get 操作，其他操作应走 CacheSync 流程
                if (cacheCommand.Operation != CacheOperation.Get)
                {
                    LogWarning($"CacheOperation 命令接收到非 Get 操作: {cacheCommand.Operation}，建议改用 CacheSync 命令");
                }

                return await ProcessCacheRequestAsync(cacheCommand, cmd.Packet, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存查询异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"处理缓存查询异常: {ex.Message}", UnifiedErrorCodes.System_InternalError);
            }
        }

        /// <summary>
        /// 处理缓存同步命令 - 【同步与广播入口】
        /// 职责：处理客户端上报的变更 (Client->Server) 并广播给其他订阅者 (Server->Client)。
        /// </summary>
        private async Task<IResponse> HandleCacheSyncAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (!(cmd.Packet.Request is CacheRequest cacheRequest))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的缓存同步命令格式", UnifiedErrorCodes.Command_ValidationFailed);
                }

                // 1. 在服务器端执行实际的缓存更新/删除
                var result = await ProcessCacheSyncAsync(cacheRequest, cmd.Packet.ExecutionContext, cancellationToken);

                // 2. 如果更新成功，则广播变更到其他在线客户端
                if (result is CacheResponse cacheResponse && cacheResponse.IsSuccess)
                {
                    await BroadcastCacheChangeAsync(cacheRequest, cmd.Packet.ExecutionContext.SessionId, cancellationToken);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogError($"处理缓存同步异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"处理缓存同步异常: {ex.Message}", UnifiedErrorCodes.System_InternalError);
            }
        }


        /// <summary>
        /// 处理缓存元数据同步命令 - 【登录后的首次元数据同步】
        /// 
        /// 【设计思路】：采用"轻量元数据协调 + 重量数据传输"的分层同步策略
        /// 负责在用户登录成功后将服务器的缓存元数据同步到客户端
        /// 
        /// 【工作流程】：
        /// 1. 服务器收集所有表的元数据（表名、数据行数、版本戳、更新时间等）
        /// 2. 发送给客户端（仅几KB）
        /// 3. 客户端比对本地版本，决策需要同步哪些表
        /// 4. 客户端按需请求实际数据（避免盲目全量同步）
        /// 
        /// 【优势】：
        /// - 登录快：元数据仅几KB，传输迅速
        /// - 流量省：只同步变化的表，节省带宽
        /// - 体验好：智能按需加载，避免内存爆炸
        /// 
        /// 【注意】：如果服务器刚启动且缓存尚未初始化完成，可能返回空元数据
        /// </summary>
        /// <param name="cmd">缓存同步命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleCacheMetadataSync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                // 缓存同步命令处理逻辑 - 可以复用现有逻辑或实现新的同步机制
                if (cmd.Packet.Request is CacheMetadataSyncRequest cacheRequest)
                {
                    // 获取当前服务器的所有缓存元数据
                    var serverSyncData = _cacheSyncMetadataManager.GetAllTableSyncInfo();

                    // 【重要】：即使服务器刚启动，也会返回空字典（不会为null）
                    // 如果缓存尚未初始化，serverSyncData.Count == 0
                    if (serverSyncData.Count == 0)
                    {
                        LogWarning("服务器缓存元数据为空，可能是服务器刚启动且缓存尚未初始化完成");
                    }

                    // 创建同步响应
                    var cacheMetadataResponse = new CacheMetadataSyncResponse(
                        serverSyncData.Count,  // UpdatedCount: 成功更新的表数量
                        0,                     // SkippedCount: 跳过的表数量
                        serverSyncData         // CacheMetadataData: 所有表的元数据字典
                    );

                    LogInfo($"响应客户端元数据请求: 表数量={serverSyncData.Count}");

                    // 返回成功响应
                    return cacheMetadataResponse;
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的缓存同步命令格式", UnifiedErrorCodes.Command_ValidationFailed);
                }
            }
            catch (Exception ex)
            {
                LogError($"处理缓存同步异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"处理缓存同步异常: {ex.Message}", UnifiedErrorCodes.System_InternalError);
            }
        }


        /// <summary>
        /// 处理缓存订阅命令 - 处理缓存订阅和取消订阅操作
        /// 支持单个表订阅、单个表取消订阅和批量取消所有表订阅
        /// </summary>
        /// <param name="cmd">缓存订阅命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleCacheSubscriptionAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {

                // 验证请求类型
                if (!(cmd.Packet.Request is CacheRequest cacheRequest))
                {
                    LogWarning($"接收到非缓存请求类型的订阅命令，命令ID: {cmd.Packet.CommandId}");
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的缓存订阅命令格式", UnifiedErrorCodes.Command_ValidationFailed);
                }

                // 获取订阅动作类型和会话信息
                var subscribeAction = cacheRequest.SubscribeAction;
                var tableName = cacheRequest.TableName;
                var sessionId = cmd.Packet.ExecutionContext?.SessionId;

                if (string.IsNullOrEmpty(sessionId))
                {
                    LogWarning("接收到无会话ID的缓存订阅命令");
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "会话ID不能为空", UnifiedErrorCodes.Command_ValidationFailed);
                }

                bool success = false;
                string message = string.Empty;

                // 记录请求日志
                LogDebug($"收到缓存订阅请求: 会话={sessionId}, 表名={tableName}, 动作={subscribeAction}");

                // 根据订阅动作执行相应操作
                switch (subscribeAction)
                {
                    case SubscribeAction.Subscribe:
                        if (string.IsNullOrEmpty(tableName))
                        {
                            LogWarning($"客户端尝试订阅空表名，会话={sessionId}");
                            return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "订阅时表名不能为空", UnifiedErrorCodes.Command_ValidationFailed);
                        }

                        // 验证表是否存在
                        var entityType = _cacheManager?.GetEntityType(tableName);
                        if (entityType == null)
                        {
                            LogWarning($"客户端尝试订阅不存在的表: 会话={sessionId}, 表名={tableName}");
                            return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"订阅的表不存在: {tableName}", UnifiedErrorCodes.Biz_DataNotFound);
                        }

                        // 使用异步方法添加订阅
                        await _subscriptionManager.AddSubscriptionAsync(tableName, sessionId);
                        success = true;
                        message = $"客户端订阅缓存: 会话={sessionId}, 表名={tableName}, 结果={success}";
                        LogInfo(message);
                        break;

                    case SubscribeAction.Unsubscribe:
                        if (string.IsNullOrEmpty(tableName))
                        {
                            // P2-3修复: 批量取消所有订阅（TableName为空时表示取消所有）
                            int unsubscribeCount = _subscriptionManager.RemoveAllSubscriptionsAsync(sessionId);
                            success = true;
                            message = $"客户端取消所有订阅: 会话={sessionId}, 共取消 {unsubscribeCount} 个表订阅";
                            LogInfo(message);
                        }
                        else
                        {
                            // P2-3修复: 单个表取消订阅
                            _subscriptionManager.RemoveSubscriptionAsync(tableName, sessionId);
                            success = true;
                            message = $"客户端取消订阅缓存: 会话={sessionId}, 表名={tableName}";
                            LogInfo(message);
                        }
                        break;

                    default:
                        LogWarning($"收到未知的订阅动作类型: {subscribeAction}, 会话={sessionId}");
                        return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("未知的订阅动作类型");
                }

                // 创建响应
                var response = new CacheResponse
                {
                    RequestId = cacheRequest.RequestId,
                    TableName = tableName,
                    IsSuccess = success,
                    Message = message,
                    Metadata = new Dictionary<string, object> { { "SubscribeAction", (int)subscribeAction } }
                };

                return response;
            }
            catch (Exception ex)
            {
                LogError($"处理缓存订阅异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"处理缓存订阅异常: {ex.Message}", UnifiedErrorCodes.System_InternalError);
            }
        }

        /// <summary>
        /// 广播缓存变更到订阅该表的客户端(增强版 - 包含完整数据)
        /// 
        /// 【设计思路】：使用 CacheSync 命令进行实时数据推送
        /// - 当某个客户端修改数据后，服务器需要通知其他订阅者
        /// - 使用 CacheRequest 作为载体（因为服务器是发起方，符合“谁先发送谁请求”原则）
        /// - 包含完整的实体数据，接收方可以直接更新本地缓存
        /// 
        /// 【与 CacheMetadataSync 的区别】：
        /// - CacheMetadataSync: 传输元数据（统计信息），用于登录时决策
        /// - CacheSync: 传输实际数据（实体内容），用于实时同步
        /// </summary>
        /// <param name="request">缓存请求数据</param>
        /// <param name="excludeSessionId">排除的会话ID（发起变更的客户端）</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task BroadcastCacheChangeAsync(CacheRequest request, string excludeSessionId, CancellationToken cancellationToken)
        {
            try
            {
                // 获取订阅该表的会话列表
                var subscribedSessions = _subscriptionManager.GetSubscribers(request.TableName);
                if (subscribedSessions == null || !subscribedSessions.Any())
                {
                    LogDebug($"表 {request.TableName} 没有订阅者,跳过广播");
                    return;
                }

                // 排除发起变更的客户端
                var targetSessionIds = subscribedSessions.Where(s => s != excludeSessionId).ToList();
                if (!targetSessionIds.Any())
                {
                    LogDebug($"表 {request.TableName} 没有其他订阅者,跳过广播");
                    return;
                }

                // 确保请求包含完整的数据
                if (request.CacheData == null && request.Operation == CacheOperation.Set)
                {
                    LogWarning($"缓存同步请求缺少数据,表名={request.TableName},操作={request.Operation}");
                    return;
                }

                // 【关键修正】：服务器主动推送变更时，应使用 CacheRequest（因为服务器是发起方）
                // 符合“谁先发送谁请求”的原则
                var pushRequest = new CacheRequest
                {
                    RequestId = Guid.NewGuid().ToString(), // 生成新的请求ID
                    TableName = request.TableName,
                    Operation = request.Operation,
                    CacheData = request.CacheData, // 包含完整的数据
                    Timestamp = DateTime.UtcNow,
                    LastRequestTime = DateTime.UtcNow
                };

                // 【优化】：从元数据管理器获取最新的版本戳并同步到请求中
                var syncInfo = _cacheSyncMetadataManager.GetTableSyncInfo(request.TableName);
                if (syncInfo != null && pushRequest.CacheData != null)
                {
                    pushRequest.CacheData.VersionStamp = syncInfo.VersionStamp;
                }

                // 添加同步元数据,用于客户端验证
                pushRequest.Parameters = new Dictionary<string, object>
                {
                    { "SyncType", "ServerPush" },
                    { "ServerTimestamp", DateTime.UtcNow.ToString("O") },
                    { "SourceSessionId", excludeSessionId ?? "Server" }
                };

                // 获取目标会话并推送
                var allSessions = _sessionService.GetAllUserSessions();
                var targetSessions = allSessions.Where(s => targetSessionIds.Contains(s.SessionID)).ToList();

                if (targetSessions.Count == 0)
                {
                    LogWarning($"没有找到目标会话,表名={request.TableName},订阅者={string.Join(",", targetSessionIds)}");
                    return;
                }

                int successCount = 0;
                int failCount = 0;
                var failedSessions = new List<string>();

                foreach (var sessionInfo in targetSessions)
                {
                    try
                    {
                        // 再次检查会话状态，防止在遍历过程中会话断开
                        if (sessionInfo.Status != RUINORERP.PacketSpec.Enums.Core.SessionStatus.Connected) continue;

                        // 服务器主动推送，使用 CacheRequest 作为载体
                        // 优化：创建独立的取消令牌，避免外部取消令牌影响推送
                        using var pushCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        pushCts.CancelAfter(15000); // 保守优化：将超时从30s降低至15s，加快失败反馈
                        
                        await _sessionService.SendPacketCoreAsync<CacheRequest>(
                            sessionInfo, 
                            CacheCommands.CacheSync, 
                            pushRequest, 
                            15000, 
                            pushCts.Token, 
                            PacketDirection.ServerRequest); // 明确标记为服务器发起的请求
                        successCount++;
                    }
                    catch (OperationCanceledException ex)
                    {
                        failCount++;
                        failedSessions.Add(sessionInfo.SessionID);
                        LogWarning($"推送缓存更新到会话超时或被取消：{sessionInfo.SessionID}, 表：{request.TableName}");
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        failedSessions.Add(sessionInfo.SessionID);
                        LogError($"推送缓存更新到会话失败: {sessionInfo.SessionID}, 表: {request.TableName}, 错误: {ex.Message}");
                    }
                }

                // 记录推送结果
                if (failCount > 0)
                {
                    LogWarning($"广播缓存变更部分失败: 表={request.TableName}, " +
                               $"目标客户端数量={targetSessions.Count}, 成功={successCount}, 失败={failCount}, " +
                               $"失败会话ID: {string.Join(", ", failedSessions)}");
                }
                else
                {
                    LogInfo($"广播缓存变更成功: 表={request.TableName}, 目标客户端数量={targetSessions.Count}");
                }
            }
            catch (Exception ex)
            {
                LogError($"广播缓存变更异常: 表={request.TableName}, 错误: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// 统一的缓存请求业务逻辑处理方法
        /// </summary>
        /// <param name="request">缓存命令</param>
        /// <param name="executionContext">执行上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<CacheResponse> ProcessCacheRequestAsync(IRequest request, PacketModel packetModel, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存请求数据
                CacheRequest cacheRequest = request as CacheRequest;
                if (cacheRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("缓存请求数据不能为空");
                }

                // 验证请求数据有效性
                if (string.IsNullOrEmpty(cacheRequest.TableName))
                {
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("表名不能为空");
                }

                // 检查是否需要刷新缓存
                bool cacheRefreshSuccess = true;
                if (cacheRequest.ForceRefresh || !IsCacheValid(cacheRequest.TableName, cacheRequest.LastRequestTime))
                {
                    try
                    {
                        await RefreshCacheDataAsync(cacheRequest.TableName, cancellationToken);
                    }
                    catch (Exception refreshEx)
                    {
                        LogError($"刷新表 {cacheRequest.TableName} 的缓存时发生错误", refreshEx);
                        cacheRefreshSuccess = false;
                    }
                }

                // 即使缓存刷新失败，也尝试获取数据
                var cacheData = await GetTableDataList(cacheRequest.TableName);

                // 创建响应 - 根据数据获取情况确定成功状态
                if (cacheData == null)
                {
                    string errorMessage = cacheRefreshSuccess
                        ? $"缓存数据不存在: {cacheRequest.TableName}"
                        : "缓存刷新失败且无法获取数据";
                    LogError(errorMessage);
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>(errorMessage);
                }

                // 只有当缓存刷新失败但数据获取成功时，记录警告
                if (!cacheRefreshSuccess)
                {
                    LogWarning($"表 {cacheRequest.TableName} 的缓存刷新失败，但成功从当前缓存获取数据");
                }

                // 创建缓存响应
                var cacheResponse = new CacheResponse
                {
                    RequestId = cacheRequest.RequestId,
                    Message = cacheRefreshSuccess ? "缓存数据获取成功" : "从缓存获取数据成功（刷新失败）",
                    CacheData = cacheData,
                    TableName = cacheRequest.TableName,
                    CacheTime = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddMinutes(30),
                    HasMoreData = false,
                    ServerVersion = Program.AppVersion,
                    IsSuccess = true
                };

                // 返回成功响应
                return cacheResponse;
            }
            catch (Exception ex)
            {
                LogError($"处理缓存请求业务逻辑异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>($"处理请求时发生错误: {ex.Message}");
            }
        }




        /// <summary>
        /// 缓存同步
        /// </summary>
        /// <param name="request">缓存命令</param>
        /// <param name="executionContext">执行上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        /// <summary>
        /// 获取跨平台兼容的类型名称，处理System.Private.CoreLib和mscorlib的差异
        /// </summary>
        /// <param name="type">类型对象</param>
        /// <returns>兼容的类型名称字符串</returns>
        private string GetCrossPlatformCompatibleTypeName(Type type)
        {
            if (type == null)
                return null;

            // 获取原始AssemblyQualifiedName
            string typeName = type.AssemblyQualifiedName;
            if (string.IsNullOrEmpty(typeName))
                return null;

            // 替换System.Private.CoreLib为mscorlib，确保.NET 4.8客户端能正确识别
            typeName = typeName.Replace("System.Private.CoreLib", "mscorlib");

            return typeName;
        }

        private async Task<CacheResponse> ProcessCacheSyncAsync(IRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存更新请求数据
                CacheRequest updateRequest = request as CacheRequest;
                if (updateRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("缓存更新请求数据不能为空" + UnifiedErrorCodes.Command_ValidationFailed.Message
                     );
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(updateRequest.TableName))
                {
                    LogInfo("缓存更新表名为空");
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("表名不能为空"
                         );
                }

                #region
                bool updateSuccess = false;
                ResponseBase errorRespnse = null;
                try
                {
                    // P0-4修复: 使用Task.Run将同步操作移到线程池执行，避免阻塞命令调度线程
                    await Task.Run(() =>
                    {
                        // 根据操作类型处理缓存变更
                        switch (updateRequest.Operation)
                        {
                            case CacheOperation.Set:
                                // 验证 CacheData 是否为 null
                                if (updateRequest.CacheData == null)
                                {
                                    logger.LogWarning($"缓存同步请求中 CacheData 为 null, TableName={updateRequest.TableName}");
                                    return;
                                }
                                
                                //CacheDataConverter.SerializeToBytes
                                object entity = updateRequest.CacheData.GetData();
                                
                                // 验证反序列化后的实体是否为 null
                                if (entity == null)
                                {
                                    logger.LogWarning($"缓存数据反序列化失败, TableName={updateRequest.TableName}, EntityTypeName={updateRequest.CacheData.EntityTypeName}");
                                    return;
                                }
                                
                                _cacheManager.UpdateEntityList(updateRequest.TableName, entity);
                                break;
                            case CacheOperation.Remove:
                                _cacheManager.DeleteEntity(updateRequest.TableName, updateRequest.PrimaryKeyValue);
                                break;
                            case CacheOperation.Clear:
                                _cacheManager.DeleteEntityList(updateRequest.TableName);
                                break;
                            default:
                                logger.LogWarning($"不支持的缓存操作类型: {updateRequest.Operation}");
                                break;
                        }
                    }, cancellationToken);
                    updateSuccess = true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"处理缓存同步异常, TableName={updateRequest.TableName}, Operation={updateRequest.Operation}");
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>(
                        $"处理缓存同步异常: {ex.Message}");
                }

                #endregion

                if (!updateSuccess)
                {
                    LogError($"更新缓存数据失败: {updateRequest.TableName}");
                    return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>($"更新缓存数据失败: 未知错误");
                }

                var cacheResponse = new CacheResponse();
                cacheResponse.Message = "缓存同步成功";
                cacheResponse.IsSuccess = true;
                return cacheResponse;
            }
            catch (Exception ex)
            {
                LogError($"处理缓存更新业务逻辑异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse<CacheResponse>($"处理缓存更新业务逻辑异常: {ex.Message}");
            }
        }



        /// <summary>
        /// 检查缓存是否有效
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="lastRequestTime">上次请求时间</param>
        /// <returns>是否有效</returns>
        private bool IsCacheValid(string tableName, DateTime lastRequestTime)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    logger.LogWarning("表名为空，缓存无效");
                    return false;
                }

                // 1. 检查缓存是否存在
                var cacheExists = _cacheManager != null && _cacheManager.GetEntityType(tableName) != null;
                if (!cacheExists)
                {
                    logger.LogDebug("表 {TableName} 的缓存不存在", tableName);
                    return false;
                }

                // 2. 使用注入的ICacheSyncMetadata服务验证缓存完整性
                bool isMetadataValid = true;
                if (_syncMetadata != null)
                {
                    isMetadataValid = _syncMetadata.ValidateTableCacheIntegrity(tableName);
                }

                if (!isMetadataValid)
                {
                    logger.LogWarning("表 {TableName} 的缓存元数据无效", tableName);
                    return false;
                }

                // 3. 检查缓存是否过期
                bool isExpired = false;
                if (_syncMetadata != null)
                {
                    isExpired = _syncMetadata.IsTableExpired(tableName);
                }

                if (isExpired)
                {
                    logger.LogDebug("表 {TableName} 的缓存已过期", tableName);
                    return false;
                }

                logger.LogTrace("表 {TableName} 的缓存验证通过", tableName);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "验证表 {TableName} 的缓存有效性时发生错误", tableName);
                return false;
            }
        }

        /// <summary>
        /// 刷新缓存数据
        /// 支持空表缓存初始化，确保即使是空表也能正确更新缓存元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        private async Task RefreshCacheDataAsync(string tableName, CancellationToken cancellationToken = default)
        {
            try
            {
                // 从数据库加载指定表的数据到缓存
                try
                {
                    Logger.LogDebug("开始刷新表 {TableName} 的缓存数据", tableName);

                    // 获取实体类型
                    var entityType = _cacheManager.GetEntityType(tableName);
                    if (entityType == null)
                    {
                        Logger.LogWarning("未找到表 {TableName} 对应的实体类型，刷新失败", tableName);
                        return;
                    }
                    // 从数据库加载指定表的数据到缓存
                    try
                    {
                        // 获取EntityCacheInitializationService实例
                        var initializationService = Startup.GetFromFac<EntityCacheInitializationService>();
                        // 调用新方法初始化单个表的缓存
                        initializationService.InitializeCacheForTable(tableName);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"加载表 {tableName} 数据时发生错误");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    // 异常处理改进：在异常情况下，尝试清理缓存元数据，避免无效状态
                    try
                    {
                        if (_syncMetadata != null)
                        {
                            Logger.LogDebug("清理表 {TableName} 的缓存元数据，避免无效状态", tableName);
                            _syncMetadata.RemoveTableSyncInfo(tableName);
                        }
                    }
                    catch (Exception cleanupEx)
                    {
                        // P2-4修复: 至少记录日志而不是完全忽略
                        Logger.LogDebug(cleanupEx, "清理表 {TableName} 的缓存元数据异常", tableName);
                    }

                    throw;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "刷新表 {TableName} 的缓存数据时发生错误", tableName);
                throw;
            }
        }

        public async Task<CacheData> GetTableDataList(string tableName)
        {
            CacheData cacheData = null;
            try
            {
                // 使用新的缓存管理器检查表是否存在
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType != null)
                {
                    //发送缓存数据
                    // 使用新的缓存管理器获取实体列表 - 指定参数类型以避免AmbiguousMatchException
                    var method = typeof(IEntityCacheManager).GetMethod("GetEntityList", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null);
                    var genericMethod = method.MakeGenericMethod(entityType);
                    var cacheList = genericMethod.Invoke(_cacheManager, new object[] { tableName });

                    if (cacheList == null)
                    {
                        //启动时服务器都没有加载缓存，则不发送
                        await Task.Delay(100);
                        cacheList = genericMethod.Invoke(_cacheManager, new object[] { tableName });
                    }

                    //上面查询可能还是没有立即加载成功
                    if (cacheList == null)
                    {
                        return cacheData;
                    }

                    // 创建CacheData对象
                    cacheData = new CacheData
                    {
                        TableName = tableName,
                        CacheTime = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddMinutes(30),
                        Version = "1.0"
                    };

                    // 处理不同类型的缓存数据
                    if (cacheList is JArray jArray)
                    {
                        cacheData.EntityByte = JsonCompressionSerializationService.Serialize(cacheList).ToArray();// jArray.ToString(); // 将JArray转换为字符串
                        cacheData.EntityTypeName = GetCrossPlatformCompatibleTypeName(cacheList.GetType()); // 使用跨平台兼容的类型名称
                        cacheData.HasMoreData = false;
                    }
                    else if (TypeHelper.IsGenericList(cacheList.GetType()))
                    {
                        var lastlist = ((IEnumerable<dynamic>)cacheList).ToList();
                        if (lastlist != null)
                        {
                            int pageSize = 100; // 每页100行
                            int totalCount = lastlist.Count;

                            // 只返回第一页数据
                            var firstPageData = lastlist.Take(pageSize).ToList();

                            // 转换为JSON字符串
                            //  string json = JsonConvert.SerializeObject(firstPageData);
                            //  cacheData.Data = json; // 直接存储JSON字符串
                            // 使用带类型信息的序列化方法
                            cacheData.EntityByte = JsonCompressionSerializationService.Serialize(lastlist).ToArray();
                            // 使用跨平台兼容的类型名称，避免System.Private.CoreLib和mscorlib的差异问题
                            cacheData.EntityTypeName = GetCrossPlatformCompatibleTypeName(lastlist.GetType());
                            cacheData.HasMoreData = totalCount > pageSize;

                            Logger?.LogDebug("{TableName}发送第一页数据，总行数:{TotalCount}，当前页:{CurrentPage}", 
                                tableName, totalCount, Math.Min(pageSize, totalCount));
                        }
                    }
                    else
                    {
                        // 尝试将其他类型转换为JSON字符串
                        try
                        {
                            //string json = JsonConvert.SerializeObject(cacheList);
                            // cacheData.Data = json; // 直接存储JSON字符串
                            cacheData.EntityByte = JsonCompressionSerializationService.Serialize(cacheList);
                            cacheData.EntityTypeName = GetCrossPlatformCompatibleTypeName(cacheList.GetType());
                            cacheData.HasMoreData = false;
                        }
                        catch (Exception ex)
                        {
                            LogError($"转换缓存数据失败: {tableName}", ex);
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
            return cacheData;
        }

        /// <summary>
        /// 广播缓存更新通知
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">更新的数据</param>
        /// <returns>任务</returns>
        private async Task BroadcastCacheUpdateAsync(string tableName, object data)
        {
            try
            {


                // 只广播给订阅了该表的客户端
                var subscribers = _subscriptionManager.GetSubscribers(tableName);
                var allSessions = _sessionService.GetAllUserSessions();
                var targetSessions = allSessions.Where(s => subscribers.Contains(s.SessionID)).ToList();

                int successCount = 0;
                int failCount = 0;

                //这里要调试。要看引用方的数据情况
                CacheRequest cacheRequest = CacheRequest.Create(tableName);
                cacheRequest.CacheData = new CacheData
                {
                    TableName = tableName,
                    //  Data = JsonConvert.SerializeObject(data),
                    CacheTime = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddMinutes(30),
                    Version = "1.0"
                };

                foreach (var session in targetSessions)
                {
                    try
                    {
                        if (session != null && !string.IsNullOrEmpty(session.SessionID))
                        {
                            // 使用ISessionService.SendPacketCoreAsync统一处理发送，包含加密和构建过程
                            await _sessionService.SendPacketCoreAsync<CacheRequest>(session, CacheCommands.CacheSync, cacheRequest, 30000, CancellationToken.None, PacketDirection.ServerRequest);
                            successCount++;
                        }
                    }
                    catch (Exception sessionEx)
                    {
                        failCount++;
                        logger.LogWarning(sessionEx, $"广播缓存更新到会话失败: {session?.SessionID ?? "Unknown"}");
                    }
                }

                LogInfo($"广播缓存更新完成: {tableName}, 订阅者数量: {subscribers.Count()}, 成功: {successCount}, 失败: {failCount}");
            }
            catch (Exception ex)
            {
                LogError($"广播缓存更新失败: {tableName}", ex);
            }
        }



        /// <summary>
        /// 将对象转换为字典
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>字典格式的数据</returns>
        private Dictionary<string, object> ConvertToDictionary(object data)
        {
            try
            {
                if (data == null)
                {
                    return new Dictionary<string, object>();
                }

                // 如果已经是字典类型，直接返回
                if (data is Dictionary<string, object> dict)
                {
                    return dict;
                }

                // 如果是列表类型，转换为字典
                if (data is IList<object> list)
                {
                    var result = new Dictionary<string, object>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        result[i.ToString()] = list[i];
                    }
                    return result;
                }

                // 如果是实体类，转换为字典
                string json = JsonConvert.SerializeObject(data);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "转换数据为字典格式失败");
                return new Dictionary<string, object>();
            }
        }


        #region 辅助方法




        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void LogInfo(string message)
        {
            logger.LogInformation($"[CacheCommandHandler] {message}");
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void LogWarning(string message)
        {
            logger.LogWarning($"[CacheCommandHandler] {message}");
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void LogDebug(string message)
        {
            logger.LogDebug($"[CacheCommandHandler] {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常对象</param>
        private void LogError(string message, Exception exception = null)
        {
            if (exception != null)
            {
                logger.LogError(exception, $"[CacheCommandHandler] {message}");
            }
            else
            {
                logger.LogError($"[CacheCommandHandler] {message}");
            }
        }




        #endregion



    }
}



