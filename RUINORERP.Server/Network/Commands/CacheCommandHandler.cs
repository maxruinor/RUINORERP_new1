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
        protected ILogger<CacheCommandHandler> logger { get; set; }

        // 添加无参构造函数，以支持Activator.CreateInstance创建实例
        /// <summary>
        /// 构造函数 - 初始化支持的命令类型
        /// </summary>
        public CacheCommandHandler() : base(new LoggerFactory().CreateLogger<BaseCommandHandler>())
        {
            logger = new LoggerFactory().CreateLogger<CacheCommandHandler>();
            _sessionService = Startup.GetFromFac<ISessionService>();

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                CacheCommands.CacheDataList,
                CacheCommands.CacheRequest,
                CacheCommands.CacheUpdate,
                CacheCommands.CacheDelete,
                CacheCommands.CacheClear,
                CacheCommands.CacheStatistics,
                CacheCommands.CacheStatus,
                CacheCommands.CacheBatchOperation,
                CacheCommands.CacheWarmup,
                CacheCommands.CacheInvalidate,
                CacheCommands.CacheSubscribe,
                CacheCommands.CacheUnsubscribe
            );
        }


        public async Task<CacheData> GetTableDataList(string tableName)
        {
            CacheData cacheData = null;
            try
            {
                if (MyCacheManager.Instance.NewTableList.ContainsKey(tableName))
                {
                    //发送缓存数据
                    var CacheList = MyCacheManager.Instance.CacheEntityList.Get(tableName);
                    if (CacheList == null)
                    {
                        //启动时服务器都没有加载缓存，则不发送
                        await Task.Delay(100);
                        CacheList = MyCacheManager.Instance.CacheEntityList.Get(tableName);
                    }

                    //上面查询可能还是没有立即加载成功
                    if (CacheList == null)
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
                    if (CacheList is JArray jArray)
                    {
                        cacheData.Data = jArray.ToString(); // 将JArray转换为字符串
                        cacheData.HasMoreData = false;
                    }
                    else if (TypeHelper.IsGenericList(CacheList.GetType()))
                    {
                        var lastlist = ((IEnumerable<dynamic>)CacheList).ToList();
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
                            
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"{tableName}发送第一页数据，总行数:{totalCount}，当前页:{Math.Min(pageSize, totalCount)}");
                            }
                        }
                    }
                    else
                    {
                        // 尝试将其他类型转换为JSON字符串
                        try
                        {
                            string json = JsonConvert.SerializeObject(CacheList);
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



        public CacheCommandHandler(ILogger<CacheCommandHandler> _Logger)
            : base(_Logger)
        {
            logger = _Logger;
            _sessionService = Startup.GetFromFac<ISessionService>();

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                CacheCommands.CacheDataList,
                CacheCommands.CacheRequest,
                CacheCommands.CacheUpdate,
                CacheCommands.CacheDelete,
                CacheCommands.CacheClear,
                CacheCommands.CacheStatistics,
                CacheCommands.CacheStatus,
                CacheCommands.CacheBatchOperation,
                CacheCommands.CacheWarmup,
                CacheCommands.CacheInvalidate,
                CacheCommands.CacheSubscribe,
                CacheCommands.CacheUnsubscribe
            );
        }

        public CacheCommandHandler(
            ILogger<CacheCommandHandler> _Logger,
            ISessionService sessionService
           )
            : base(_Logger)
        {
            logger = _Logger;
            _sessionService = sessionService;

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                CacheCommands.CacheDataList,
                CacheCommands.CacheRequest,
                CacheCommands.CacheUpdate,
                CacheCommands.CacheDelete,
                CacheCommands.CacheClear,
                CacheCommands.CacheStatistics,
                CacheCommands.CacheStatus,
                CacheCommands.CacheBatchOperation,
                CacheCommands.CacheWarmup,
                CacheCommands.CacheInvalidate,
                CacheCommands.CacheSubscribe,
                CacheCommands.CacheUnsubscribe
            );
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<CommandId> SupportedCommands { get; protected set; }



        /// <summary>
        /// 判断是否可以处理指定命令
        /// </summary>
        public override bool CanHandle(QueuedCommand cmd)
        {
            if (cmd?.Command == null)
                return false;

            return cmd.Command is CacheCommand ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheDataList ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheRequest ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheUpdate ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheDelete ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheClear ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheStatistics ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheStatus ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheBatchOperation ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheWarmup ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheInvalidate ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheSubscribe ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheUnsubscribe;
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
                if (commandId == CacheCommands.CacheDataList || commandId == CacheCommands.CacheRequest)
                {
                    return await HandleCacheRequestAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheUpdate)
                {
                    return await HandleCacheUpdateAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheDelete)
                {
                    return await HandleCacheDeleteAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheClear)
                {
                    return await HandleCacheClearAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheStatistics)
                {
                    return await HandleCacheStatisticsAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheStatus)
                {
                    return await HandleCacheStatusAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheBatchOperation)
                {
                    return await HandleCacheBatchOperationAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheWarmup)
                {
                    return await HandleCacheWarmupAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheInvalidate)
                {
                    return await HandleCacheInvalidateAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheSubscribe)
                {
                    return await HandleCacheSubscribeAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheUnsubscribe)
                {
                    return await HandleCacheUnsubscribeAsync(cmd, cancellationToken);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError($"不支持的缓存命令类型: {cmd.Command.CommandIdentifier}", UnifiedErrorCodes.Command_NotFound)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理缓存命令异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存命令异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_PROCESSING_ERROR");
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
                if (command.Command is BaseCommand<CacheRequest, CacheResponse> cacheCommand)
                {
                    return await ProcessCacheRequestAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_FORMAT");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理缓存请求异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存请求异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_REQUEST_ERROR");
            }
        }

        /// <summary>
        /// 统一的缓存请求业务逻辑处理方法（模仿登录业务的ProcessLoginAsync）
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
                var cacheData =await GetTableDataList(cacheRequest.TableName);
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
                // 使用统一的业务逻辑处理方法
                if (command.Command is CacheCommand cacheCommand)
                {
                    return await ProcessCacheUpdateAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存更新命令格式", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_UPDATE_FORMAT");
                }
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
        private async Task<BaseCommand<IResponse>> ProcessCacheUpdateAsync(CacheCommand cacheCommand, CmdContext executionContext, CancellationToken cancellationToken)
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

                if (updateRequest == null)
                {
                    LogError("缓存更新数据为空");
                    return BaseCommand<IResponse>.CreateError("更新数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_UPDATE_DATA");
                }

                LogInfo($"处理缓存更新: 会话={executionContext.SessionId}, 表名={updateRequest.TableName}");

                // 更新缓存
                try
                {
                    // 将数据转换为JObject
                    // JObject obj = JObject.FromObject(updateRequest.Data);
                    // MyCacheManager.Instance.UpdateEntityList(updateRequest.TableName, obj);

                    // 如果是产品表有变化，还需要更新产品视图的缓存
                    if (updateRequest.TableName == nameof(tb_Prod))
                    {
                        //  var prod = obj.ToObject<tb_Prod>();
                        //   LogInfo($"产品缓存已更新: {prod?.Prod_ID}");
                        // TODO: 广播产品缓存数据更新
                        // BroadcastProdCatchData(UserSession, prod);
                    }
                }
                catch (Exception updateEx)
                {
                    LogError($"更新缓存数据失败: {updateRequest.TableName}", updateEx);
                    return BaseCommand<IResponse>.CreateError($"更新缓存数据失败: {updateEx.Message}", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "CACHE_UPDATE_FAILED");
                }

                // 广播给其他客户端
                // await BroadcastCacheUpdateAsync(updateRequest.TableName, updateRequest.Data);

                LogInfo($"缓存更新成功: {updateRequest.TableName}");
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
                if (command.Command is CacheCommand cacheCommand)
                {
                    return await ProcessCacheDeleteAsync(cacheCommand, command.Packet.ExecutionContext, cancellationToken);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("不支持的缓存删除命令格式", UnifiedErrorCodes.Command_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_DELETE_FORMAT");
                }
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

                // 删除缓存 - 还要看传过来要删除的具体值，还要通知其它用户
                try
                {
                    //if (!string.IsNullOrEmpty(deleteRequest.PrimaryKeyName) && deleteRequest.PrimaryKeyValue != null)
                    //{
                    //    // 根据主键删除特定记录
                    //    MyCacheManager.Instance.DeleteEntityList(deleteRequest.TableName, deleteRequest.PrimaryKeyName, deleteRequest.PrimaryKeyValue);
                    //    LogInfo($"缓存记录已删除: {deleteRequest.TableName}, 主键={deleteRequest.PrimaryKeyName}, 值={deleteRequest.PrimaryKeyValue}");
                    //}
                    //else
                    //{
                    //    // 删除整个表的缓存
                    //    MyCacheManager.Instance.RemoveCache(deleteRequest.TableName);
                    //    LogInfo($"缓存表已删除: {deleteRequest.TableName}");
                    //}
                }
                catch (Exception deleteEx)
                {
                    LogError($"删除缓存数据失败: {deleteRequest.TableName}", deleteEx);
                    return BaseCommand<IResponse>.CreateError($"删除缓存数据失败: {deleteEx.Message}", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "CACHE_DELETE_FAILED");
                }

                // 广播给其他客户端
                await BroadcastCacheDeleteAsync(deleteRequest.TableName);

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
                // 检查缓存是否存在
                //var cacheInfo = _cacheService.GetCacheInfo(tableName);
                //if (cacheInfo == null || cacheInfo.LastUpdated < lastRequestTime)
                //{
                //    return false;
                //}

                //// 检查缓存是否过期
                //if (cacheInfo.ExpirationTime < DateTime.Now)
                //{
                //    return false;
                //}

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

                // 广播给所有客户端
                var allSessions = _sessionService.GetAllUserSessions();
                int successCount = 0;
                int failCount = 0;

                foreach (var session in allSessions)
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

                LogInfo($"广播缓存更新完成: {tableName}, 成功: {successCount}, 失败: {failCount}");
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

                // 广播给所有客户端
                var allSessions = _sessionService.GetAllUserSessions();
                int successCount = 0;
                int failCount = 0;

                foreach (var session in allSessions)
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

                LogInfo($"广播缓存删除完成: {tableName}, 成功: {successCount}, 失败: {failCount}");
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
        private async Task<BaseCommand<IResponse>> HandleCacheRemoveAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var packet = command.Packet;

                // 验证请求数据
                //if (string.IsNullOrEmpty(removeRequest.TableName))
                //{
                //    LogError("缓存删除表名为空");
                //    return CreateErrorResponse("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_TABLE_NAME");
                //}

                //if (string.IsNullOrEmpty(removeRequest.Key))
                //{
                //    LogError("缓存删除键为空");
                //    return CreateErrorResponse("删除键不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_REMOVE_KEY");
                //}


                // 删除缓存记录
                //try
                //{
                //    MyCacheManager.Instance.RemoveCacheItem(removeRequest.TableName, removeRequest.Key);
                //    LogInfo($"缓存记录删除成功: {removeRequest.TableName}, 键={removeRequest.Key}");
                //}
                //catch (Exception removeEx)
                //{
                //    LogError($"删除缓存记录失败: {removeRequest.TableName}, 键={removeRequest.Key}", removeEx);
                //    return CreateErrorResponse($"删除缓存记录失败: {removeEx.Message}", UnifiedErrorCodes.Biz_OperationFailed, "CACHE_REMOVE_FAILED");
                //}

                // 广播更新通知
                //   await BroadcastCacheUpdateAsync(removeRequest.TableName, null);

                // 构建响应数据
                var responseData = new CacheResponse
                {
                    TableName = "",
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
                // TODO: 实现缓存订阅逻辑
                LogInfo("处理缓存订阅命令");

                var responseData = new CacheResponse
                {
                    TableName = "Subscribe",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存订阅成功");
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
                // TODO: 实现缓存取消订阅逻辑
                LogInfo("处理缓存取消订阅命令");

                var responseData = new CacheResponse
                {
                    TableName = "Unsubscribe",
                    IsSuccess = true
                };

                return BaseCommand<IResponse>.CreateSuccess(responseData, "缓存取消订阅成功");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存取消订阅异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_UNSUBSCRIBE_ERROR");
            }
        }
    }
}