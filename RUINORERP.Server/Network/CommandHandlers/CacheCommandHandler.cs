using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.Comm;
using RUINORERP.Server.BizService;
using RUINORERP.Common;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;
using System;
using Newtonsoft.Json.Linq;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetTaste;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Errors;
using SuperSocket.Server.Abstractions.Session;
using RUINORERP.Business.Cache;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Windows.Forms;
using System.IO.Packaging;
using RUINORERP.PacketSpec.Serialization;
using SqlSugar;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Cache;


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
            ICacheSyncMetadata syncMetadata) // 通过DI注入
            : base(logger)
        {
            this.logger = logger;
            _sessionService = sessionService;
            _subscriptionManager = subscriptionManager;
            _cacheManager = cacheManager;
            _syncMetadata = syncMetadata;

            // 使用安全方法设置支持的命令 - 适配简化的缓存命令系统
            SetSupportedCommands(
                CacheCommands.CacheOperation,
                CacheCommands.CacheSync,
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
        /// 处理缓存操作命令 - 统一处理各类缓存操作（Get、Set、Update、Delete、Clear等）
        /// </summary>
        /// <param name="cmd">缓存操作命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleCacheOperationAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                // 使用统一的业务逻辑处理方法
                if (!(cmd.Packet.Request is CacheRequest cacheCommand))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的缓存命令格式", UnifiedErrorCodes.Command_ValidationFailed);
                }

                // 直接调用统一处理方法
                return await ProcessCacheRequestAsync(cacheCommand, cmd.Packet, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存操作异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"处理缓存操作异常: {ex.Message}", UnifiedErrorCodes.System_InternalError);
            }
        }

        /// <summary>
        /// 处理缓存同步命令 - 负责服务器与客户端之间的缓存数据同步
        /// </summary>
        /// <param name="cmd">缓存同步命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleCacheSyncAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                // 缓存同步命令处理逻辑 - 可以复用现有逻辑或实现新的同步机制
                if (!(cmd.Packet.Request is CacheRequest cacheRequest))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的缓存同步命令格式", UnifiedErrorCodes.Command_ValidationFailed);
                }

                // 使用统一的处理方法
                var result = await ProcessCacheSyncAsync(cacheRequest, cmd.Packet.ExecutionContext, cancellationToken);

                // 同步完成后广播变更到其他客户端
                if (result is CacheResponse cacheResponse && cacheResponse.IsSuccess)
                {
                    // 广播变更,请求中包含了要更新的缓存数据信息
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
                            // 批量取消所有订阅（TableName为空时表示取消所有）
                            int unsubscribeCount = _subscriptionManager.RemoveAllSubscriptionsAsync(sessionId);
                            success = true;
                            message = $"客户端取消所有订阅: 会话={sessionId}, 共取消 {unsubscribeCount} 个表订阅";
                            LogInfo(message);
                        }
                        else
                        {
                            // 单个表取消订阅
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
        /// 广播缓存变更到订阅该表的客户端
        /// </summary>
        /// <param name="request">缓存响应数据</param>
        /// <param name="excludeSessionId">排除的会话ID（发起变更的客户端）</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task BroadcastCacheChangeAsync(CacheRequest request, string excludeSessionId, CancellationToken cancellationToken)
        {
            try
            {
                // 获取订阅该表的会话列表
                var subscribedSessions = _subscriptionManager.GetSubscribers(request.TableName);
                if (subscribedSessions != null && subscribedSessions.Any())
                {
                    // 排除发起变更的客户端
                    var targetSessions = subscribedSessions.Where(s => s != excludeSessionId).ToList();

                    var package = PacketModel.CreateFromRequest(CacheCommands.CacheSync, request)
                        .WithDirection(PacketSpec.Enums.Core.PacketDirection.ServerRequest);
                    var serializedData = JsonCompressionSerializationService.Serialize(package).ToArray();

                    var sessions = _sessionService.GetAllUserSessions(excludeSessionId);
                    // 加密数据
                    var originalData = new OriginalData((byte)package.CommandId.Category, new byte[] { package.CommandId.OperationCode },
                        serializedData
                    );
                    var encryptedData = PacketSpec.Security.UnifiedEncryptionProtocol.EncryptServerDataToClient(originalData);
                    // 广播给所有订阅的会话
                    foreach (var sessionInfo in sessions)
                    {
                        await sessionInfo.SendAsync(encryptedData.ToArray(), cancellationToken);
                        //后面来具体实现 TODO 
                        // await _sessionService.SendCommandToSession(sessionId, syncCommand, cancellationToken);
                    }

                }
            }
            catch (Exception ex)
            {
                LogError($"广播缓存变更异常: {ex.Message}", ex);
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

        private Task<CacheResponse> ProcessCacheSyncAsync(IRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存更新请求数据
                CacheRequest updateRequest = request as CacheRequest;
                if (updateRequest == null)
                {
                    return Task.FromResult<CacheResponse>(ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("缓存更新请求数据不能为空" + UnifiedErrorCodes.Command_ValidationFailed.Message
                     ));
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(updateRequest.TableName))
                {
                    LogInfo("缓存更新表名为空");
                    return Task.FromResult<CacheResponse>(ResponseFactory.CreateSpecificErrorResponse<CacheResponse>("表名不能为空"
                         ));
                }

                #region
                bool updateSuccess = false;
                ResponseBase errorRespnse = null;
                try
                {
                    // 根据操作类型处理缓存变更
                    switch (updateRequest.Operation)
                    {
                        case CacheOperation.Set:
                            //CacheDataConverter.SerializeToBytes
                            object entity = updateRequest.CacheData.GetData();
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
                    updateSuccess = true;
                }
                catch (Exception ex)
                {
                    //errorRespnse = new ResponseBase
                    //{
                    //    IsSuccess = false,
                    //    Message = ex.Message,
                    //    Timestamp = DateTime.Now
                    //};
                }

                #endregion

                if (!updateSuccess)
                {
                    LogError($"更新缓存数据失败: {updateRequest.TableName}");
                    return Task.FromResult<CacheResponse>(ResponseFactory.CreateSpecificErrorResponse<CacheResponse>($"更新缓存数据失败: 未知错误"));
                }

                var cacheResponse = new CacheResponse();
                cacheResponse.Message = "缓存同步成功";
                cacheResponse.IsSuccess = true;
                return Task.FromResult<CacheResponse>(cacheResponse);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存更新业务逻辑异常: {ex.Message}", ex);
                return Task.FromResult<CacheResponse>(ResponseFactory.CreateSpecificErrorResponse<CacheResponse>($"处理缓存更新业务逻辑异常: {ex.Message}"));
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
                        await initializationService.InitializeSingleTableCacheAsync(tableName);
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
                    catch { /* 忽略清理异常 */ }

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

                            if (frmMainNew.Instance.IsDebug)
                            {
                                frmMainNew.Instance.PrintInfoLog($"{tableName}发送第一页数据，总行数:{totalCount}，当前页:{Math.Min(pageSize, totalCount)}");
                            }
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
                // 创建更新通知
                var updateNotification = new
                {
                    Type = "Update",
                    TableName = tableName,
                    Timestamp = DateTime.Now,
                    Data = data
                };

                // 序列化通知
                string json = JsonConvert.SerializeObject(updateNotification);
                byte[] dataBytes = Encoding.UTF8.GetBytes(json);

                // 只广播给订阅了该表的客户端
                var subscribers = _subscriptionManager.GetSubscribers(tableName);
                var allSessions = _sessionService.GetAllUserSessions();
                var targetSessions = allSessions.Where(s => subscribers.Contains(s.SessionID)).ToList();

                int successCount = 0;
                int failCount = 0;

                foreach (var session in targetSessions)
                {
                    try
                    {
                        if (session != null && !string.IsNullOrEmpty(session.SessionID))
                        {
                            await (session).SendAsync(dataBytes);
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



        /// <summary>
        /// 广播缓存变更给其他客户端
        /// </summary>
        /// <param name="senderSessionId">发送者会话ID</param>
        /// <param name="request">缓存请求</param>
        private async Task BroadcastCacheChangeAsync(string senderSessionId, CacheRequest request)
        {
            try
            {
                // 创建变更通知
                var notification = new
                {
                    Type = request.Operation,
                    request.TableName,
                    Timestamp = DateTime.Now
                };

                // 序列化通知
                string json = JsonConvert.SerializeObject(notification);
                byte[] dataBytes = Encoding.UTF8.GetBytes(json);

                // 只广播给订阅了该表的客户端（排除发送者）
                var subscribers = _subscriptionManager.GetSubscribers(request.TableName);
                var allSessions = _sessionService.GetAllUserSessions();
                var targetSessions = allSessions
                    .Where(s => subscribers.Contains(s.SessionID) && s.SessionID != senderSessionId)
                    .ToList();

                logger.Debug($"准备广播缓存变更: 表名={request.TableName}, 订阅者数量={subscribers.Count()}, 目标客户端数量={targetSessions.Count}");

                int successCount = 0;
                int failCount = 0;

                foreach (var session in targetSessions)
                {
                    try
                    {
                        if (session != null && !string.IsNullOrEmpty(session.SessionID))
                        {
                            await (session).SendAsync(dataBytes);
                            successCount++;
                        }
                    }
                    catch (Exception sessionEx)
                    {
                        failCount++;
                        logger.LogWarning(sessionEx, $"广播缓存变更到会话失败: {session?.SessionID ?? "Unknown"}");
                    }
                }

                logger.Debug($"广播缓存变更完成: 表名={request.TableName}, 成功={successCount}, 失败={failCount}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"广播缓存变更失败: 表名={request?.TableName}");
            }
        }
    }
}



