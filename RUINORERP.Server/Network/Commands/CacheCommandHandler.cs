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
                CacheCommands.CacheDataList.FullCode,
                CacheCommands.CacheSync.FullCode,
                CacheCommands.CacheUpdate.FullCode,
                CacheCommands.CacheDelete.FullCode
            );
        }


        public static async void 发送缓存数据列表(string tableName)
        {
            try
            {
                if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
                {
                    //发送缓存数据
                    var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (CacheList == null)
                    {
                        //启动时服务器都没有加载缓存，则不发送
                        BizCacheHelper.Instance.SetDictDataSource(tableName, true);
                        await Task.Delay(500);
                        CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    }

                    //上面查询可能还是没有立即加载成功
                    if (CacheList == null)
                    {
                        return;
                    }

                    if (CacheList is JArray)
                    {
                        //暂时认为服务器的都是泛型形式保存的
                    }

                    if (TypeHelper.IsGenericList(CacheList.GetType()))
                    {
                        var lastlist = ((IEnumerable<dynamic>)CacheList).ToList();
                        if (lastlist != null)
                        {
                            int pageSize = 100; // 每页100行
                            for (int i = 0; i < lastlist.Count; i += pageSize)
                            {
                                // 计算当前页的结束索引，确保不会超出数组界限
                                int endIndex = Math.Min(i + pageSize, lastlist.Count);

                                // 获取当前页的JArray片段
                                object page = lastlist.Skip(i).Take(endIndex - i).ToArray();

                                // 处理当前页
                                发送缓存数据列表(tableName, page);

                                // 如果当前页是最后一页，可能不足200行，需要特殊处理
                                if (endIndex == lastlist.Count)
                                {
                                    //处理最后一页的逻辑，如果需要的话
                                    if (frmMain.Instance.IsDebug)
                                    {
                                        frmMain.Instance.PrintInfoLog($"{tableName}最后一页发送完成,总行数:{endIndex}");
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }

        }


        private static void 发送缓存数据列表(string tableName, object list)
        {
            try
            {
                string json = JsonConvert.SerializeObject(list,
                      new JsonSerializerSettings
                      {
                          Converters = new List<JsonConverter> { new CustomCollectionJsonConverter() },
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // 或 ReferenceLoopHandling.Serialize
                          Formatting = Formatting.None // 禁用格式化
                      });

                //tx.PushString(tableName);
                //tx.PushString(json);

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据:" + ex.Message);
            }

        }

        public CacheCommandHandler(ILogger<CacheCommandHandler> _Logger)
            : base(_Logger)
        {
            logger = _Logger;
            _sessionService = Startup.GetFromFac<ISessionService>();

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                CacheCommands.CacheDataList.FullCode,
                CacheCommands.CacheSync.FullCode,
                CacheCommands.CacheUpdate.FullCode,
                CacheCommands.CacheDelete.FullCode
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
                CacheCommands.CacheDataList.FullCode,
                CacheCommands.CacheSync.FullCode,
                CacheCommands.CacheUpdate.FullCode,
                CacheCommands.CacheDelete.FullCode
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
                   cmd.Command.CommandIdentifier == CacheCommands.CacheSync ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheUpdate ||
                   cmd.Command.CommandIdentifier == CacheCommands.CacheDelete;
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
                if (commandId == CacheCommands.CacheDataList)
                {
                    return await HandleCacheRequestAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheSync)
                {
                    return await HandleCacheSyncAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheUpdate)
                {
                    return await HandleCacheUpdateAsync(cmd, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheDelete)
                {
                    return await HandleCacheDeleteAsync(cmd, cancellationToken);
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
        /// 处理缓存请求
        /// </summary>
        /// <param name="command">缓存请求命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheRequestAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取请求数据

                if (command.Command == null || command.Command.RequestDataByMessagePack == null)
                {
                    LogError("缓存请求数据为空");
                    return BaseCommand<IResponse>.CreateError("缓存请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_REQUEST");
                }

                if (command.Command is CacheCommand cacheCommand)
                {

                    // 解析请求数据
              
                    if (cacheCommand.RequestDataByMessagePack  == null)
                    {
                        LogError("解析缓存请求数据失败");
                        return BaseCommand<IResponse>.CreateError("解析缓存请求数据失败", UnifiedErrorCodes.Command_ValidationFailed)
                            .WithMetadata("ErrorCode", "INVALID_CACHE_REQUEST");
                    }

                    // 验证请求数据有效性
                    if (string.IsNullOrEmpty(cacheRequest.TableName))
                    {
                        LogError("缓存请求表名为空");
                        return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                            .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                    }

                    // 记录请求信息
                    LogInfo($"处理缓存请求: 会话={packet.ExecutionContext.SessionId}, 表名={cacheRequest.TableName}, 强制刷新={cacheRequest.ForceRefresh}");

                    // 检查是否需要刷新缓存
                    if (cacheRequest.ForceRefresh || !IsCacheValid(cacheRequest.TableName, cacheRequest.LastRequestTime))
                    {
                        LogInfo($"需要刷新缓存数据: {cacheRequest.TableName}");
                        await RefreshCache
                    }



                    // 解析请求数据
                    string json = Encoding.UTF8.GetString(packet.Body);
                    var cacheRequest = JsonConvert.DeserializeObject<CacheRequest>(json);
                    if (cacheRequest == null)
                    {
                        LogError("解析缓存请求数据失败");
                        return BaseCommand<IResponse>.CreateError("解析缓存请求数据失败", UnifiedErrorCodes.Command_ValidationFailed)
                            .WithMetadata("ErrorCode", "INVALID_CACHE_REQUEST");
                    }

                    // 验证请求数据有效性
                    if (string.IsNullOrEmpty(cacheRequest.TableName))
                    {
                        LogError("缓存请求表名为空");
                        return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                            .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                    }

                    // 记录请求信息
                    LogInfo($"处理缓存请求: 会话={packet.ExecutionContext.SessionId}, 表名={cacheRequest.TableName}, 强制刷新={cacheRequest.ForceRefresh}");

                    // 检查是否需要刷新缓存
                    if (cacheRequest.ForceRefresh || !IsCacheValid(cacheRequest.TableName, cacheRequest.LastRequestTime))
                    {
                        LogInfo($"需要刷新缓存数据: {cacheRequest.TableName}");
                        await RefreshCacheDataAsync(cacheRequest.TableName, cancellationToken);
                    }

                    // 获取缓存数据
                    var cacheData = MyCacheManager.Instance.GetCache(cacheRequest.TableName);
                    if (cacheData == null)
                    {
                        LogWarning($"缓存数据不存在: {cacheRequest.TableName}");
                        return BaseCommand<IResponse>.CreateError($"缓存数据不存在: {cacheRequest.TableName}", UnifiedErrorCodes.Biz_DataNotFound)
                            .WithMetadata("ErrorCode", "CACHE_DATA_NOT_FOUND");
                    }

                    // 准备缓存响应
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

                    // 发送缓存数据到客户端
                    await SendCacheResponseAsync(packet.ExecutionContext.SessionId, cacheResponse);

                    LogInfo($"缓存请求处理成功: {cacheRequest.TableName}, 数据量: {cacheData.Count}");

                    // 返回成功响应
                    return BaseCommand<IResponse>.CreateSuccess("缓存数据获取成功")
                        .WithMetadata("TableName", cacheRequest.TableName)
                        .WithMetadata("DataCount", cacheData.Count.ToString())
                        .WithMetadata("CacheTime", cacheResponse.CacheTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
        /// 处理缓存同步
        /// </summary>
        /// <param name="command">缓存同步命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheSyncAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    LogError("缓存同步请求数据为空");
                    return BaseCommand<IResponse>.CreateError("缓存同步请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_SYNC_REQUEST");
                }



                // 解析同步请求数据
                string json = Encoding.UTF8.GetString(packet.Body);
                var syncRequest = JsonConvert.DeserializeObject<CacheSyncRequest>(json);
                if (syncRequest == null)
                {
                    LogError("解析缓存同步请求数据失败");
                    return BaseCommand<IResponse>.CreateError("解析缓存同步请求数据失败", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_SYNC_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(syncRequest.SyncMode))
                {
                    LogError("缓存同步模式为空");
                    return BaseCommand<IResponse>.CreateError("同步模式不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_SYNC_MODE");
                }

                LogInfo($"处理缓存同步: 会话={packet.ExecutionContext.SessionId}, 模式={syncRequest.SyncMode}, 表数量={syncRequest.CacheKeys?.Count ?? 0}");

                // 根据同步模式处理
                int syncedCount = 0;
                if (syncRequest.SyncMode == "FULL" || syncRequest.CacheKeys == null || syncRequest.CacheKeys.Count == 0)
                {
                    // 全量同步
                    syncedCount = await SyncAllCacheAsync(syncRequest, packet.ExecutionContext.SessionId, cancellationToken);
                }
                else
                {
                    // 增量同步指定的缓存键
                    syncedCount = await SyncSpecificCacheAsync(syncRequest.CacheKeys, packet.ExecutionContext.SessionId, cancellationToken);
                }

                LogInfo($"缓存同步成功: 同步了 {syncedCount} 个表的数据");

                return BaseCommand<IResponse>.CreateSuccess("缓存同步成功")
                    .WithMetadata("SyncMode", syncRequest.SyncMode)
                    .WithMetadata("SyncedCount", syncedCount.ToString())
                    .WithMetadata("SyncTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                LogError($"处理缓存同步异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存同步异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_SYNC_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存更新
        /// </summary>
        /// <param name="command">缓存更新命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheUpdateAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 解析更新数据
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    LogError("缓存更新请求数据为空");
                    return BaseCommand<IResponse>.CreateError("缓存更新请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_UPDATE_REQUEST");
                }



                // 处理更新逻辑
                string json = Encoding.UTF8.GetString(packet.Body);
                var updateRequest = JsonConvert.DeserializeObject<CacheUpdateRequest>(json);
                if (updateRequest == null)
                {
                    LogError("解析缓存更新请求数据失败");
                    return BaseCommand<IResponse>.CreateError("解析缓存更新请求数据失败", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_UPDATE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(updateRequest.TableName))
                {
                    LogError("缓存更新表名为空");
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }

                if (updateRequest.Data == null)
                {
                    LogError("缓存更新数据为空");
                    return BaseCommand<IResponse>.CreateError("更新数据不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_UPDATE_DATA");
                }

                LogInfo($"处理缓存更新: 会话={packet.ExecutionContext.SessionId}, 表名={updateRequest.TableName}");

                // 更新缓存
                try
                {
                    // 将数据转换为JObject
                    JObject obj = JObject.FromObject(updateRequest.Data);
                    MyCacheManager.Instance.UpdateEntityList(updateRequest.TableName, obj);

                    // 如果是产品表有变化，还需要更新产品视图的缓存
                    if (updateRequest.TableName == nameof(tb_Prod))
                    {
                        var prod = obj.ToObject<tb_Prod>();
                        LogInfo($"产品缓存已更新: {prod?.Prod_ID}");
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
                await BroadcastCacheUpdateAsync(updateRequest.TableName, updateRequest.Data);

                LogInfo($"缓存更新成功: {updateRequest.TableName}");

                return BaseCommand<IResponse>.CreateSuccess("缓存更新成功")
                    .WithMetadata("TableName", updateRequest.TableName)
                    .WithMetadata("UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .WithMetadata("Broadcasted", "true");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存更新异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存更新异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_UPDATE_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存删除
        /// </summary>
        /// <param name="command">缓存删除命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> HandleCacheDeleteAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 解析删除数据
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    LogError("缓存删除请求数据为空");
                    return BaseCommand<IResponse>.CreateError("缓存删除请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "EMPTY_CACHE_DELETE_REQUEST");
                }



                // 处理删除逻辑
                string json = Encoding.UTF8.GetString(packet.Body);
                var deleteRequest = JsonConvert.DeserializeObject<CacheDeleteRequest>(json);
                if (deleteRequest == null)
                {
                    LogError("解析缓存删除请求数据失败");
                    return BaseCommand<IResponse>.CreateError("解析缓存删除请求数据失败", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_DELETE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(deleteRequest.TableName))
                {
                    LogError("缓存删除表名为空");
                    return BaseCommand<IResponse>.CreateError("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed)
                        .WithMetadata("ErrorCode", "EMPTY_TABLE_NAME");
                }

                LogInfo($"处理缓存删除: 会话={packet.ExecutionContext.SessionId}, 表名={deleteRequest.TableName}");

                // 删除缓存 - 还要看传过来要删除的具体值，还要通知其它用户
                try
                {
                    if (!string.IsNullOrEmpty(deleteRequest.PrimaryKeyName) && deleteRequest.PrimaryKeyValue != null)
                    {
                        // 根据主键删除特定记录
                        MyCacheManager.Instance.DeleteEntityList(deleteRequest.TableName, deleteRequest.PrimaryKeyName, deleteRequest.PrimaryKeyValue);
                        LogInfo($"缓存记录已删除: {deleteRequest.TableName}, 主键={deleteRequest.PrimaryKeyName}, 值={deleteRequest.PrimaryKeyValue}");
                    }
                    else
                    {
                        // 删除整个表的缓存
                        MyCacheManager.Instance.RemoveCache(deleteRequest.TableName);
                        LogInfo($"缓存表已删除: {deleteRequest.TableName}");
                    }
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

                return BaseCommand<IResponse>.CreateSuccess("缓存删除成功")
                    .WithMetadata("TableName", deleteRequest.TableName)
                    .WithMetadata("DeleteTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .WithMetadata("Broadcasted", "true");
            }
            catch (Exception ex)
            {
                LogError($"处理缓存删除异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理缓存删除异常: {ex.Message}", UnifiedErrorCodes.System_InternalError)
                    .WithMetadata("ErrorCode", "CACHE_DELETE_ERROR");
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
        private void RefreshCacheDataAsync(string tableName)
        {
            try
            {
                // 从数据库加载数据并更新缓存
                BizCacheHelper.Instance.SetDictDataSource(tableName, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"刷新缓存数据失败: {tableName}");
                throw;
            }
        }



        /// <summary>
        /// 发送缓存响应到客户端
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="response">缓存响应</param>
        /// <returns>任务</returns>
        private async Task SendCacheResponseAsync(string sessionId, CacheResponse response)
        {
            try
            {
                // 获取会话
                var session = _sessionService.GetSession(sessionId);
                if (session == null)
                {
                    logger.LogWarning($"找不到会话: {sessionId}");
                    return;
                }

                // 序列化响应
                string json = JsonConvert.SerializeObject(response);
                byte[] dataBytes = Encoding.UTF8.GetBytes(json);

                //todo 
                // await session.SendAsync(buffer);

                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintInfoLog($"发送缓存数据到客户端: {sessionId}, 表名: {response.TableName}, 数据量: {response.CacheData.Count}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"发送缓存响应到客户端失败: {sessionId}");
            }
        }

        /// <summary>
        /// 同步所有缓存到客户端
        /// </summary>
        /// <param name="syncCommand">同步命令</param>
        /// <returns>任务</returns>
        private async Task SyncAllCacheAsync(CacheCommand syncCommand)
        {
            try
            {
                // 获取所有缓存表名
                var allTableNames = BizCacheHelper.Manager.NewTableTypeList.Keys;
                if (allTableNames == null || allTableNames.Count == 0)
                {
                    return;
                }

                // 分批发送缓存数据
                foreach (var tableName in allTableNames)
                {
                    // 避免发送过多数据导致网络阻塞，间隔一段时间
                    await Task.Delay(100);

                    // 获取并发送缓存数据
                    var cacheData = BizCacheHelper.Instance.GetEntity(tableName, true);
                    Dictionary<string, object> cacheDataDic = new Dictionary<string, object>();
                    cacheDataDic.Add(tableName, cacheData);
                    if (cacheData != null)
                    {
                        var syncResponse = new CacheResponse
                        {
                            Code = 200,
                            Message = "同步成功",
                            CacheData = cacheDataDic,
                            TableName = tableName,
                            CacheTime = DateTime.Now,
                            ExpirationTime = DateTime.Now.AddMinutes(30),
                            HasMoreData = false,
                            ServerVersion = Program.AppVersion
                        };

                        await SendCacheResponseAsync(syncCommand.Packet.SessionId, syncResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "全量同步缓存失败");
                throw;
            }
        }

        /// <summary>
        /// 同步指定的缓存表到客户端
        /// </summary>
        /// <param name="cacheKeys">缓存键列表</param>
        /// <returns>任务</returns>
        private async Task SyncSpecificCacheAsync(List<string> cacheKeys)
        {
            try
            {
                foreach (var tableName in cacheKeys)
                {
                    // 避免发送过多数据导致网络阻塞，间隔一段时间
                    await Task.Delay(50);

                    // 获取并发送缓存数据
                    List<CacheInfo> CacheInfos = new List<CacheInfo>();
                    foreach (var item in BizCacheHelper.Manager.NewTableList)
                    {
                        CacheInfo cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(item.Key) as CacheInfo;
                        if (cacheInfo != null)
                        {
                            CacheInfos.Add(cacheInfo);
                        }
                    }

                    //var cacheData = await GetCacheDataAsync(tableName);
                    //if (cacheData != null && cacheData.Count > 0)
                    //{
                    //    var syncResponse = new CacheResponse
                    //    {
                    //        StatusCode = 200,
                    //        Message = "同步成功",
                    //        CacheData = cacheData,
                    //        TableName = tableName,
                    //        CacheTime = DateTime.Now,
                    //        ExpireTime = DateTime.Now.AddMinutes(30),
                    //        HasMoreData = false,
                    //        ServerVersion = Program.AppVersion
                    //    };

                    //    // 发送到所有客户端
                    //    var allSessions = _sessionService.GetAllSessions();
                    //    foreach (var session in allSessions)
                    //    {
                    //        await SendCacheResponseAsync(session.SessionId, syncResponse);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "增量同步缓存失败");
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

                // 广播给所有客户端
                var allSessions = _sessionService.GetAllUserSessions();
                int successCount = 0;
                int failCount = 0;



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

                            //await session.SendAsync(buffer);
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

        /// <summary>
        /// 处理缓存获取
        /// </summary>
        /// <param name="command">缓存获取命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> HandleCacheGetAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    LogError("缓存获取请求数据为空");
                    return CreateErrorResponse("缓存获取请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_CACHE_GET_REQUEST");
                }



                // 解析获取请求
                string json = Encoding.UTF8.GetString(packet.Body);
                var getRequest = JsonConvert.DeserializeObject<CacheGetRequest>(json);
                if (getRequest == null)
                {
                    LogError("解析缓存获取请求数据失败");
                    return CreateErrorResponse("解析缓存获取请求数据失败", UnifiedErrorCodes.Command_ValidationFailed, "INVALID_CACHE_GET_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(getRequest.TableName))
                {
                    LogError("缓存获取表名为空");
                    return CreateErrorResponse("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_TABLE_NAME");
                }

                LogInfo($"处理缓存获取: 会话={packet.ExecutionContext.SessionId}, 表名={getRequest.TableName}");

                // 获取缓存数据
                object cacheData = null;
                try
                {
                    if (!string.IsNullOrEmpty(getRequest.PrimaryKeyName) && getRequest.PrimaryKeyValue != null)
                    {
                        // 根据主键获取特定记录
                        cacheData = MyCacheManager.Instance.GetEntityList(getRequest.TableName, getRequest.PrimaryKeyName, getRequest.PrimaryKeyValue);
                        LogInfo($"获取缓存记录: {getRequest.TableName}, 主键={getRequest.PrimaryKeyName}, 值={getRequest.PrimaryKeyValue}");
                    }
                    else
                    {
                        // 获取整个表的缓存
                        cacheData = MyCacheManager.Instance.GetCache(getRequest.TableName);
                        LogInfo($"获取缓存表: {getRequest.TableName}");
                    }
                }
                catch (Exception getEx)
                {
                    LogError($"获取缓存数据失败: {getRequest.TableName}", getEx);
                    return CreateErrorResponse($"获取缓存数据失败: {getEx.Message}", UnifiedErrorCodes.Biz_OperationFailed, "CACHE_GET_FAILED");
                }

                if (cacheData == null)
                {
                    LogWarning($"缓存不存在: {getRequest.TableName}");
                    return CreateErrorResponse("缓存不存在", UnifiedErrorCodes.Biz_DataNotFound, "CACHE_NOT_FOUND");
                }

                // 构建响应数据
                var responseData = new
                {
                    TableName = getRequest.TableName,
                    Data = cacheData,
                    Timestamp = DateTime.Now,
                    SessionId = packet.ExecutionContext.SessionId,
                    TotalCount = cacheData is IEnumerable enumerable ? enumerable.Cast<object>().Count() : 1
                };

                LogInfo($"缓存获取成功: {getRequest.TableName}");

                return ResponseBase.CreateSuccess("缓存获取成功", responseData)
                    .WithMetadata("TableName", getRequest.TableName)
                    .WithMetadata("HasData", "true")
                    .WithMetadata("GetTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .WithMetadata("SessionId", packet.ExecutionContext.SessionId);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存获取异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_GET_ERROR");
            }
        }

        /// <summary>
        /// 处理缓存设置
        /// </summary>
        /// <param name="command">缓存设置命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> HandleCacheSetAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    LogError("缓存设置请求数据为空");
                    return CreateErrorResponse("缓存设置请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_CACHE_SET_REQUEST");
                }



                // 解析设置请求
                string json = Encoding.UTF8.GetString(packet.Body);
                var setRequest = JsonConvert.DeserializeObject<CacheSetRequest>(json);
                if (setRequest == null)
                {
                    LogError("解析缓存设置请求数据失败");
                    return CreateErrorResponse("解析缓存设置请求数据失败", UnifiedErrorCodes.Command_ValidationFailed, "INVALID_CACHE_SET_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(setRequest.TableName))
                {
                    LogError("缓存设置表名为空");
                    return CreateErrorResponse("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_TABLE_NAME");
                }

                if (setRequest.Data == null)
                {
                    LogError("缓存设置数据为空");
                    return CreateErrorResponse("缓存数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_CACHE_DATA");
                }

                LogInfo($"处理缓存设置: 会话={packet.ExecutionContext.SessionId}, 表名={setRequest.TableName}");

                // 设置缓存
                try
                {
                    MyCacheManager.Instance.SetCache(setRequest.TableName, setRequest.Data);
                    LogInfo($"缓存设置成功: {setRequest.TableName}");
                }
                catch (Exception setEx)
                {
                    LogError($"设置缓存数据失败: {setRequest.TableName}", setEx);
                    return CreateErrorResponse($"设置缓存数据失败: {setEx.Message}", UnifiedErrorCodes.Biz_OperationFailed, "CACHE_SET_FAILED");
                }

                // 广播更新通知
                await BroadcastCacheUpdateAsync(setRequest.TableName, setRequest.Data);

                // 构建响应数据
                var responseData = new
                {
                    TableName = setRequest.TableName,
                    SetTime = DateTime.Now,
                    SessionId = packet.ExecutionContext.SessionId,
                    DataType = setRequest.Data.GetType().Name
                };

                LogInfo($"缓存设置完成: {setRequest.TableName}");

                return ResponseBase.CreateSuccess("缓存设置成功", responseData)
                    .WithMetadata("TableName", setRequest.TableName)
                    .WithMetadata("SetTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .WithMetadata("Broadcasted", "true")
                    .WithMetadata("SessionId", packet.ExecutionContext.SessionId);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存设置异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_SET_ERROR");
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
        private async Task<ResponseBase> HandleCacheRemoveAsync(QueuedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    LogError("缓存删除请求数据为空");
                    return CreateErrorResponse("缓存删除请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_CACHE_REMOVE_REQUEST");
                }



                // 解析删除请求
                string json = Encoding.UTF8.GetString(packet.Body);
                var removeRequest = JsonConvert.DeserializeObject<CacheRemoveRequest>(json);
                if (removeRequest == null)
                {
                    LogError("解析缓存删除请求数据失败");
                    return CreateErrorResponse("解析缓存删除请求数据失败", UnifiedErrorCodes.Command_ValidationFailed, "INVALID_CACHE_REMOVE_REQUEST");
                }

                // 验证请求数据
                if (string.IsNullOrEmpty(removeRequest.TableName))
                {
                    LogError("缓存删除表名为空");
                    return CreateErrorResponse("表名不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_TABLE_NAME");
                }

                if (string.IsNullOrEmpty(removeRequest.Key))
                {
                    LogError("缓存删除键为空");
                    return CreateErrorResponse("删除键不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_REMOVE_KEY");
                }

                LogInfo($"处理缓存删除: 会话={packet.ExecutionContext.SessionId}, 表名={removeRequest.TableName}, 键={removeRequest.Key}");

                // 删除缓存记录
                try
                {
                    MyCacheManager.Instance.RemoveCacheItem(removeRequest.TableName, removeRequest.Key);
                    LogInfo($"缓存记录删除成功: {removeRequest.TableName}, 键={removeRequest.Key}");
                }
                catch (Exception removeEx)
                {
                    LogError($"删除缓存记录失败: {removeRequest.TableName}, 键={removeRequest.Key}", removeEx);
                    return CreateErrorResponse($"删除缓存记录失败: {removeEx.Message}", UnifiedErrorCodes.Biz_OperationFailed, "CACHE_REMOVE_FAILED");
                }

                // 广播更新通知
                await BroadcastCacheUpdateAsync(removeRequest.TableName, null);

                // 构建响应数据
                var responseData = new
                {
                    TableName = removeRequest.TableName,
                    RemoveKey = removeRequest.Key,
                    RemoveTime = DateTime.Now,
                    SessionId = packet.ExecutionContext.SessionId
                };

                LogInfo($"缓存删除完成: {removeRequest.TableName}, 键={removeRequest.Key}");

                return ResponseBase.CreateSuccess("缓存删除成功", responseData)
                    .WithMetadata("TableName", removeRequest.TableName)
                    .WithMetadata("RemoveKey", removeRequest.Key)
                    .WithMetadata("RemoveTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .WithMetadata("Broadcasted", "true")
                    .WithMetadata("SessionId", packet.ExecutionContext.SessionId);
            }
            catch (Exception ex)
            {
                LogError($"处理缓存删除异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "CACHE_REMOVE_ERROR");
            }
        }
    }
}