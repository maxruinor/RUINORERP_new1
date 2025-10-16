using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.ServerSession;
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetTaste;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Errors;
using SuperSocket.Server.Abstractions.Session;
using RUINORERP.Business.Cache;
using System.Reflection;


namespace RUINORERP.Server.Network.Commands
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

        // 添加构造函数注入CacheSubscriptionManager
        /// <summary>
        /// 构造函数 - 通过DI注入依赖项
        /// </summary>
        public CacheCommandHandler(
            ILogger<CacheCommandHandler> logger,
            ISessionService sessionService,
            CacheSubscriptionManager subscriptionManager, // 通过DI注入
            IEntityCacheManager cacheManager) // 通过DI注入
            : base(logger)
        {
            this.logger = logger;
            _sessionService = sessionService;
            _subscriptionManager = subscriptionManager;
            _cacheManager = cacheManager;

            // 使用安全方法设置支持的命令 - 适配简化的缓存命令系统
            SetSupportedCommands(
                CacheCommands.CacheOperation,
                CacheCommands.CacheSync,
                CacheCommands.CacheSubscription
            );
        }


        /// <summary>
        /// 判断是否可以处理指定命令
        /// </summary>
        public override bool CanHandle(QueuedCommand cmd)
        {
            if (cmd?.Command == null)
                return false;

            // 使用HashSet提高查找效率 - 适配简化的缓存命令系统
            var supportedCommands = new HashSet<CommandId>
            {
                CacheCommands.CacheOperation,
                CacheCommands.CacheSync,
                CacheCommands.CacheSubscription
            };

            return cmd.Command is CacheCommand || supportedCommands.Contains(cmd.Command.CommandIdentifier);
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="cmd">队列命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Command.CommandIdentifier;

                // 使用字典映射替代冗长的if-else链 - 适配简化的缓存命令系统
                var commandHandlers = new Dictionary<CommandId, Func<QueuedCommand, CancellationToken, Task<BaseCommand<IResponse>>>>
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

                return BaseCommand<IResponse>.CreateError($"不支持的缓存命令类型: {cmd.Command.CommandIdentifier}", UnifiedErrorCodes.Command_NotFound)
                    .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_COMMAND");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存命令异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存命令异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_PROCESSING_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存操作命令 - 统一处理各类缓存操作（Get、Set、Update、Delete、Clear等）
        /// </summary>
        /// <param name="command">缓存操作命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheOperationAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 使用统一的业务逻辑处理方法
                if (!(command.Command is BaseCommand<CacheRequest, CacheResponse> cacheCommand))
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_FORMAT");
                }

                // 直接调用统一处理方法
                return await ProcessCacheRequestAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存操作异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存操作异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_OPERATION_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存同步命令 - 负责服务器与客户端之间的缓存数据同步
        /// </summary>
        /// <param name="command">缓存同步命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheSyncAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 缓存同步命令处理逻辑 - 可以复用现有逻辑或实现新的同步机制
                if (!(command.Command is BaseCommand<CacheRequest, CacheResponse> cacheCommand))
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存同步命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_SYNC_FORMAT");
                }

                // 使用统一的处理方法
                var result = await ProcessCacheRequestAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);

                // 同步完成后广播变更到其他客户端
                if (result?.ResponseData is CacheResponse cacheResponse && cacheResponse.IsSuccess)
                {
                    await BroadcastCacheChangeAsync(cacheResponse, command.Packet.ExecutionContext.SessionId, cancellationToken);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogError($"处理缓存同步异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存同步异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_SYNC_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存订阅命令 - 处理缓存订阅和取消订阅操作
        /// </summary>
        /// <param name="command">缓存订阅命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheSubscriptionAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (!(command.Command is BaseCommand<CacheRequest, CacheResponse> cacheCommand))
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存订阅命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_SUBSCRIPTION_FORMAT");
                }

                var cacheRequest = cacheCommand.Request;
                if (cacheRequest == null || string.IsNullOrEmpty(cacheRequest.TableName))
                {
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "TABLE_NAME_EMPTY");
                }

                // 获取订阅动作类型
                var subscribeAction = cacheRequest.SubscribeAction;
                var tableName = cacheRequest.TableName;
                var sessionId = command.Packet.ExecutionContext.SessionId;
                bool success = false;

                // 根据订阅动作执行相应操作
                switch (subscribeAction)
                {
                    case SubscribeAction.Subscribe:
                        success = _subscriptionManager.Subscribe(sessionId, tableName);
                        LogInfo($"客户端订阅缓存: 会话={sessionId}, 表名={tableName}, 结果={success}");
                        break;
                    case SubscribeAction.Unsubscribe:
                        success = _subscriptionManager.Unsubscribe(sessionId, tableName);
                        LogInfo($"客户端取消订阅缓存: 会话={sessionId}, 表名={tableName}, 结果={success}");
                        break;
                    default:
                        return BaseCommand<IResponse>.CreateError("未知的订阅动作类型", UnifiedErrorCodes.Command_ValidationFailed)
                            .WithMetadata("ErrorCode", "UNKNOWN_SUBSCRIBE_ACTION");
                }

                // 创建响应
                var response = CacheResponse.CreateSuccess(tableName);
                response.Metadata = new Dictionary<string, object> { { "SubscribeAction", (int)subscribeAction } };

                return BaseCommand<IResponse>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存订阅异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存订阅异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_SUBSCRIPTION_ERROR");
            }
        }

        /// <summary>
        /// 广播缓存变更到订阅该表的客户端
        /// </summary>
        /// <param name="response">缓存响应数据</param>
        /// <param name="excludeSessionId">排除的会话ID（发起变更的客户端）</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task BroadcastCacheChangeAsync(CacheResponse response, string excludeSessionId, CancellationToken cancellationToken)
        {
            try
            {
                // 获取订阅该表的会话列表
                var subscribedSessions = _subscriptionManager.GetSubscribers(response.TableName);
                if (subscribedSessions != null && subscribedSessions.Any())
                {
                    // 排除发起变更的客户端
                    var targetSessions = subscribedSessions.Where(s => s != excludeSessionId).ToList();

                    // 创建同步命令
                    var syncCommand = CacheCommand.CreateSyncCommand(response);

                    // 广播给所有订阅的会话
                    foreach (var sessionId in targetSessions)
                    {
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
        /// 处理缓存请求 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="command">缓存请求命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheRequestAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 使用统一的业务逻辑处理方法
                if (!(command.Command is BaseCommand<CacheRequest, CacheResponse> cacheCommand))
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_FORMAT");
                }

                return await ProcessCacheRequestAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存请求异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存请求异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_REQUEST_ERROR");
            }
        }

        /// <summary>
        /// 统一的缓存请求业务逻辑处理方法
        /// </summary>
        /// <param name="cacheCommand">缓存命令</param>
        /// <param name="executionContext">执行上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> ProcessCacheRequestAsync(BaseCommand<CacheRequest, CacheResponse> cacheCommand, CmdContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存请求数据
                var cacheRequest = cacheCommand.Request;
                if (cacheRequest == null)
                {
                    return BaseCommand<IResponse>.CreateError("缓存请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_REQUEST");
                }

                // 验证请求数据有效性
                if (string.IsNullOrEmpty(cacheRequest.TableName))
                {
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }

                // 检查是否需要刷新缓存
                if (cacheRequest.ForceRefresh || !IsCacheValid(cacheRequest.TableName, cacheRequest.LastRequestTime))
                {
                    await RefreshCacheDataAsync(cacheRequest.TableName, cancellationToken);
                }

                // 获取缓存数据
                var cacheData = await GetTableDataList(cacheRequest.TableName);
                if (cacheData == null)
                {
                    return BaseCommand<IResponse>.CreateError($"缓存数据不存在: {cacheRequest.TableName}", UnifiedErrorCodes.Biz_DataNotFound)
                        .WithMetadata("ErrorCode", "CACHE_DATA_NOT_FOUND");
                }

                // 创建缓存响应
                var cacheResponse = new CacheResponse
                {
                    RequestId = cacheRequest.RequestId,
                    Message = "缓存数据获取成功",
                    CacheData = cacheData,
                    TableName = cacheRequest.TableName,
                    CacheTime = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddMinutes(30),
                    HasMoreData = false,
                    ServerVersion = Program.AppVersion,
                    IsSuccess = true
                };

                // 返回成功响应
                return BaseCommand<IResponse>.CreateSuccess(cacheResponse, "缓存数据获取成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存请求业务逻辑异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存请求业务逻辑异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_BUSINESS_ERROR");
            }
        }


        /// <summary>
        /// 处理缓存更新 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="command">缓存更新命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheUpdateAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (!(command.Command is BaseCommand<CacheRequest, IResponse> baseCommand))
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存更新命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_UPDATE_FORMAT");
                }

                return await ProcessCacheUpdateAsync(baseCommand, command.Packet.ExecutionContext, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存更新异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存更新异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_UPDATE_ERROR");
            }
        }

        /// <summary>
        /// 统一的缓存更新业务逻辑处理方法
        /// </summary>
        /// <param name="cacheCommand">缓存命令</param>
        /// <param name="executionContext">执行上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> ProcessCacheUpdateAsync(BaseCommand<CacheRequest, IResponse> cacheCommand, CmdContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存更新请求数据
                var updateRequest = cacheCommand.Request;
                if (updateRequest == null)
                {
                    return BaseCommand<IResponse>.CreateError("缓存更新请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_UPDATE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(updateRequest.TableName))
                {
                    LogError("缓存更新表名为空");
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }


                // 直接处理客户端缓存更新，移除对ServerCacheSyncService的依赖
                bool updateSuccess = await HandleClientCacheUpdateAsync(executionContext.SessionId, updateRequest);
                if (!updateSuccess)
                {
                    LogError($"更新缓存数据失败: {updateRequest.TableName}");
                    return BaseCommand<IResponse>.CreateError($"更新缓存数据失败: 未知错误", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "CACHE_UPDATE_FAILED");
                }

                var cacheResponse = new CacheResponse();
                return BaseCommand<IResponse>.CreateSuccess(cacheResponse, "缓存更新成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存更新业务逻辑异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存更新业务逻辑异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_UPDATE_BUSINESS_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存删除 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="command">缓存删除命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheDeleteAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 使用统一的业务逻辑处理方法
                if (!(command.Command is CacheCommand cacheCommand))
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存删除命令格式", UnifiedErrorCodes.Command_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_DELETE_FORMAT");
                }

                return await ProcessCacheDeleteAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存删除异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存删除异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_DELETE_ERROR");
            }
        }

        /// <summary>
        /// 统一的缓存删除业务逻辑处理方法
        /// </summary>
        /// <param name="cacheCommand">缓存命令</param>
        /// <param name="executionContext">执行上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> ProcessCacheDeleteAsync(CacheCommand cacheCommand, CmdContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存删除请求数据
                var deleteRequest = cacheCommand.Request;
                if (deleteRequest == null)
                {
                    return BaseCommand<IResponse>.CreateError("缓存删除请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_DELETE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(deleteRequest.TableName))
                {
                    LogError("缓存删除表名为空");
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }

                bool deleteSuccess = await HandleClientCacheUpdateAsync(executionContext.SessionId, deleteRequest);
                if (!deleteSuccess)
                {
                    LogError($"删除缓存数据失败: {deleteRequest.TableName}");
                    return BaseCommand<IResponse>.CreateError($"删除缓存数据失败: 未知错误", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "CACHE_DELETE_FAILED");
                }

                LogInfo($"缓存删除成功: {deleteRequest.TableName}");
                var cacheResponse = new CacheResponse();
                return BaseCommand<IResponse>.CreateSuccess(cacheResponse, "缓存删除成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存删除业务逻辑异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存删除业务逻辑异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_DELETE_BUSINESS_ERROR");
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
                // 目前总是返回true，后续可以根据实际需求实现缓存有效性检查逻辑
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 刷新缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>任务</returns>
        private async Task RefreshCacheDataAsync(string tableName, CancellationToken cancellationToken = default)
        {
            try
            {
                // 从数据库加载数据并更新缓存
                // 注意：这是一个假的异步方法，实际的SetDictDataSource不是异步操作
                // 未来如果有真正的异步数据库操作，可以在这里实现

                // 添加一个await Task.CompletedTask来使方法真正成为异步方法
                // 这样可以避免警告，同时保持向后兼容性
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"刷新缓存数据失败: {tableName}");
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
                        cacheData.Data = jArray.ToString(); // 将JArray转换为字符串
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
                            string json = JsonConvert.SerializeObject(firstPageData);
                            cacheData.Data = json; // 直接存储JSON字符串
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
                            string json = JsonConvert.SerializeObject(cacheList);
                            cacheData.Data = json; // 直接存储JSON字符串
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
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
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
                            await ((IAppSession)session).SendAsync(dataBytes);
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
        /// 广播缓存删除通知
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>任务</returns>
        private async Task BroadcastCacheDeleteAsync(string tableName)
        {
            try
            {
                // 创建删除通知
                var deleteNotification = new
                {
                    Type = "Delete",
                    TableName = tableName,
                    Timestamp = DateTime.Now
                };

                // 序列化通知
                string json = JsonConvert.SerializeObject(deleteNotification);
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
                            await ((IAppSession)session).SendAsync(dataBytes);
                            successCount++;
                        }
                    }
                    catch (Exception sessionEx)
                    {
                        failCount++;
                        logger.LogWarning(sessionEx, $"广播缓存删除到会话失败: {session?.SessionID ?? "Unknown"}");
                    }
                }

                LogInfo($"广播缓存删除完成: {tableName}, 订阅者数量: {subscribers.Count()}, 成功: {successCount}, 失败: {failCount}");
            }
            catch (Exception ex)
            {
                LogError($"广播缓存删除失败: {tableName}", ex);
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
        /// 创建统一的错误响应
        /// </summary>
        private BaseCommand<IResponse> CreateErrorResponse(string message, ErrorCode errorCode, string customErrorCode)
        {
            return BaseCommand<IResponse>.CreateError($"{errorCode.Message}: {message}", errorCode.Code)
                .WithMetadata("ErrorCode", customErrorCode);
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private BaseCommand<IResponse> CreateExceptionResponse(Exception ex, string errorCode)
        {
            return BaseCommand<IResponse>.CreateError($"[{ex.GetType().Name}] {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                .WithMetadata("ErrorCode", errorCode)
                .WithMetadata("Exception", ex.Message)
                .WithMetadata("StackTrace", ex.StackTrace);
        }

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
        /// 处理缓存删除（单条记录）
        /// </summary>
        /// <param name="command">缓存删除命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        /// <summary>
        /// 处理缓存删除命令
        /// </summary>
        /// <param name="command">缓存删除命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheRemoveAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var packet = command.Packet;

                // 删除缓存记录
                try
                {
                    // TODO: 使用新的缓存管理器删除缓存项
                    // 暂时记录日志
                    LogInfo($"缓存记录删除成功");
                }
                catch (Exception removeEx)
                {
                    LogError($"删除缓存记录失败", removeEx);
                    return CreateErrorResponse($"删除缓存记录失败: {removeEx.Message}", UnifiedErrorCodes.Biz_OperationFailed, "CACHE_REMOVE_FAILED");
                }

                // 构建响应数据
                var responseData = new CacheResponse
                {
                    TableName = "",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存删除成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存删除异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_REMOVE_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存清空命令
        /// </summary>
        /// <param name="command">缓存清空命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheClearAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: 实现缓存清空逻辑
                // 暂时记录日志
                LogInfo("处理缓存清空命令");

                var responseData = new CacheResponse
                {
                    TableName = "All",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存清空成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存清空异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_CLEAR_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存统计命令
        /// </summary>
        /// <param name="command">缓存统计命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheStatisticsAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: 实现缓存统计逻辑
                // 暂时记录日志
                LogInfo("处理缓存统计命令");

                var responseData = new CacheResponse
                {
                    TableName = "Statistics",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存统计获取成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存统计异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_STATISTICS_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存状态命令
        /// </summary>
        /// <param name="command">缓存状态命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheStatusAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: 实现缓存状态逻辑
                // 暂时记录日志
                LogInfo("处理缓存状态命令");

                var responseData = new CacheResponse
                {
                    TableName = "Status",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存状态获取成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存状态异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_STATUS_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存批量操作命令
        /// </summary>
        /// <param name="command">缓存批量操作命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheBatchOperationAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: 实现缓存批量操作逻辑
                // 暂时记录日志
                LogInfo("处理缓存批量操作命令");

                var responseData = new CacheResponse
                {
                    TableName = "BatchOperation",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存批量操作成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存批量操作异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_BATCH_OPERATION_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存预热命令
        /// </summary>
        /// <param name="command">缓存预热命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheWarmupAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: 实现缓存预热逻辑
                // 暂时记录日志
                LogInfo("处理缓存预热命令");

                var responseData = new CacheResponse
                {
                    TableName = "Warmup",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存预热成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存预热异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_WARMUP_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存失效命令
        /// </summary>
        /// <param name="command">缓存失效命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheInvalidateAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: 实现缓存失效逻辑
                // 暂时记录日志
                LogInfo("处理缓存失效命令");

                var responseData = new CacheResponse
                {
                    TableName = "Invalidate",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存失效成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存失效异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_INVALIDATE_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存订阅命令
        /// </summary>
        /// <param name="command">缓存订阅命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheSubscribeAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取执行上下文中的会话ID
                var sessionId = command.Packet?.ExecutionContext?.SessionId;
                if (string.IsNullOrEmpty(sessionId))
                {
                    return BaseCommand<IResponse>.CreateError("会话ID不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_SESSION_ID");
                }

                // 获取缓存订阅请求数据
                var cacheCommand = command.Command as CacheCommand;
                var subscribeRequest = cacheCommand?.Request;
                if (subscribeRequest == null)
                {
                    return BaseCommand<IResponse>.CreateError("缓存订阅请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_SUBSCRIBE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(subscribeRequest.TableName))
                {
                    LogError("缓存订阅表名为空");
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }

                LogInfo($"处理缓存订阅: 会话={sessionId}, 表名={subscribeRequest.TableName}");

                // 执行订阅操作
                var subscribeResult = _subscriptionManager.Subscribe(sessionId, subscribeRequest.TableName);
                if (!subscribeResult)
                {
                    LogError($"缓存订阅失败: 会话={sessionId}, 表名={subscribeRequest.TableName}");
                    return BaseCommand<IResponse>.CreateError("缓存订阅失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "CACHE_SUBSCRIBE_FAILED");
                }

                LogInfo($"缓存订阅成功: 会话={sessionId}, 表名={subscribeRequest.TableName}");
                var cacheResponse = new CacheResponse
                {
                    TableName = subscribeRequest.TableName,
                    IsSuccess = true,
                    Message = "缓存订阅成功"
                };

                return BaseCommand<IResponse>.CreateSuccess(cacheResponse, "缓存订阅成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存订阅异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_SUBSCRIBE_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存取消订阅命令
        /// </summary>
        /// <param name="command">缓存取消订阅命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheUnsubscribeAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取执行上下文中的会话ID
                var sessionId = command.Packet?.ExecutionContext?.SessionId;
                if (string.IsNullOrEmpty(sessionId))
                {
                    return BaseCommand<IResponse>.CreateError("会话ID不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_SESSION_ID");
                }

                // 获取缓存取消订阅请求数据
                var cacheCommand = command.Command as CacheCommand;
                var unsubscribeRequest = cacheCommand?.Request;
                if (unsubscribeRequest == null)
                {
                    return BaseCommand<IResponse>.CreateError("缓存取消订阅请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_UNSUBSCRIBE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(unsubscribeRequest.TableName))
                {
                    LogError("缓存取消订阅表名为空");
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }

                LogInfo($"处理缓存取消订阅: 会话={sessionId}, 表名={unsubscribeRequest.TableName}");

                // 执行取消订阅操作
                var unsubscribeResult = _subscriptionManager.Unsubscribe(sessionId, unsubscribeRequest.TableName);
                if (!unsubscribeResult)
                {
                    LogError($"缓存取消订阅失败: 会话={sessionId}, 表名={unsubscribeRequest.TableName}");
                    return BaseCommand<IResponse>.CreateError("缓存取消订阅失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "CACHE_UNSUBSCRIBE_FAILED");
                }

                LogInfo($"缓存取消订阅成功: 会话={sessionId}, 表名={unsubscribeRequest.TableName}");
                var cacheResponse = new CacheResponse
                {
                    TableName = unsubscribeRequest.TableName,
                    IsSuccess = true,
                    Message = "缓存取消订阅成功"
                };

                return BaseCommand<IResponse>.CreateSuccess(cacheResponse, "缓存取消订阅成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存取消订阅异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_UNSUBSCRIBE_ERROR");
            }
        }

        /// <summary>
        /// 处理客户端发送的缓存更新请求
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="request">缓存请求</param>
        /// <returns>是否处理成功</returns>
        private async Task<bool> HandleClientCacheUpdateAsync(string sessionId, CacheRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    logger.LogWarning("会话ID为空，无法处理缓存更新请求");
                    return false;
                }

                if (request == null)
                {
                    logger.LogWarning("缓存请求为空，无法处理");
                    return false;
                }


                // 根据操作类型处理缓存变更
                switch (request.Operation)
                {
                    case CacheOperation.Set:
                        HandleUpdateOperationAsync(request);
                        break;
                    case CacheOperation.Remove:
                        await HandleRemoveOperationAsync(request);
                        break;
                    case CacheOperation.Clear:
                        await HandleClearOperationAsync(request);
                        break;
                    default:
                        logger.LogWarning($"不支持的缓存操作类型: {request.Operation}");
                        return false;
                }

                // 广播变更给其他订阅了该表的客户端
                await BroadcastCacheChangeAsync(sessionId, request);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"处理客户端缓存更新时发生异常: 会话={sessionId}, 表名={request?.TableName}");
                return false;
            }
        }

        /// <summary>
        /// 处理更新操作
        /// </summary>
        /// <param name="request">缓存请求</param>
        private void HandleUpdateOperationAsync(CacheRequest request)
        {
            try
            {
                // 使用缓存管理器更新实体
                if (request.Data != null)
                {
                    _cacheManager.UpdateEntityList(request.TableName, request.Data);
                }
                // 使用缓存管理器批量更新实体
                if (request.Data != null)
                {
                    _cacheManager.UpdateEntityList(request.TableName, request.Data);
                    //  logger.Debug($"批量更新操作成功: 表名={request.TableName}, 数据量={request.Data.Count}");
                }
                else
                {
                    logger.Debug($"批量更新操作数据为空: 表名={request.TableName}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"处理更新操作失败: 表名={request.TableName}");
                throw;
            }
        }


        /// <summary>
        /// 处理删除操作
        /// </summary>
        /// <param name="request">缓存请求</param>
        private async Task HandleRemoveOperationAsync(CacheRequest request)
        {
            try
            {
                //如果根据传过来数据情况。是多个KEY还是一个key删除
                // 使用缓存管理器删除实体
                if (request.Parameters["PrimaryKeyName"] != null)
                {
                    _cacheManager.DeleteEntity(request.TableName, request.Parameters["PrimaryKeyName"].ToString());
                }
                else
                {
                    logger.Debug($"删除操作参数不完整: 表名={request.TableName}, 主键={request.Parameters["PrimaryKeyName"]}, 值={request.Parameters["PrimaryKeyName"]}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"处理删除操作失败: 表名={request.TableName}");
                throw;
            }
        }

        /// <summary>
        /// 处理清空操作
        /// </summary>
        /// <param name="request">缓存请求</param>
        private async Task HandleClearOperationAsync(CacheRequest request)
        {
            try
            {
                // 清空操作的处理逻辑
                //_cacheManager.ClearTableCache(request.TableName);
                logger.Debug($"清空操作成功: 表名={request.TableName}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"处理清空操作失败: 表名={request.TableName}");
                throw;
            }
        }

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
                    TableName = request.TableName,
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
                            await ((IAppSession)session).SendAsync(dataBytes);
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



