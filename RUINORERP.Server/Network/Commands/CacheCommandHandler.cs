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
                CacheCommands.CacheDataList,
                CacheCommands.CacheSync,
                CacheCommands.CacheUpdate,
                CacheCommands.CacheDelete
            );
        }


        public async Task<object> GetTableDataList(string tableName)
        {
            object CacheData = null;
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
                        await Task.Delay(100);
                        CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    }

                    //上面查询可能还是没有立即加载成功
                    if (CacheList == null)
                    {
                        return CacheData;
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
                                CacheData = lastlist.Skip(i).Take(endIndex - i).ToArray();

                                // 如果当前页是最后一页，可能不足200行，需要特殊处理
                                if (endIndex == lastlist.Count)
                                {
                                    //处理最后一页的逻辑，如果需要的话
                                    if (frmMain.Instance.IsDebug)
                                    {
                                        frmMain.Instance.PrintInfoLog($"{tableName}最后一页发送完成,总行数:{endIndex}");
                                    }
                                }
                                return CacheData;

                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
            return CacheData;
        }



        public CacheCommandHandler(ILogger<CacheCommandHandler> _Logger)
            : base(_Logger)
        {
            logger = _Logger;
            _sessionService = Startup.GetFromFac<ISessionService>();

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                CacheCommands.CacheDataList,
                CacheCommands.CacheSync,
                CacheCommands.CacheUpdate,
                CacheCommands.CacheDelete
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
                CacheCommands.CacheSync,
                CacheCommands.CacheUpdate,
                CacheCommands.CacheDelete
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
                if (command.Command is CacheCommand cacheCommand)
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
        private async Task<BaseCommand<IResponse>> ProcessCacheRequestAsync(CacheCommand cacheCommand, CmdContext executionContext, CancellationToken cancellationToken)
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
                var cacheData = GetTableDataList(cacheRequest.TableName);
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
                    //CacheData = cacheData,
                    TableName = cacheRequest.TableName,
                    CacheTime = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddMinutes(30),
                    HasMoreData = false,
                    ServerVersion = Program.AppVersion,
                    IsSuccess = true
                };

                // 发送缓存数据到客户端
                await SendCacheResponseAsync(executionContext.SessionId, cacheResponse);

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
                BizCacheHelper.Instance.SetDictDataSource(tableName, true);

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
                            Message = "同步成功",
                            //CacheData = cacheDataDic,
                            TableName = tableName,
                            CacheTime = DateTime.Now,
                            ExpirationTime = DateTime.Now.AddMinutes(30),
                            HasMoreData = false,
                            ServerVersion = Program.AppVersion
                        };

                        //await SendCacheResponseAsync(syncCommand.Packet.SessionId, syncResponse);
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
    }
}