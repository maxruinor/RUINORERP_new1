using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using RUINORERP.PacketSpec.Protocol;
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

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 缓存命令处理器 - 处理客户端的缓存请求
    /// 包括缓存数据的获取、发送和管理
    /// </summary>
    [CommandHandler("CacheCommandHandler", priority: 80)]
    public class CacheCommandHandler : CommandHandlerBase
    {
        private readonly ISessionService _sessionService;
        protected ILogger<CacheCommandHandler> logger { get; set; }

        // 添加无参构造函数，以支持Activator.CreateInstance创建实例
        public CacheCommandHandler() : base(new LoggerFactory().CreateLogger<CommandHandlerBase>())
        {
            logger = new LoggerFactory().CreateLogger<CacheCommandHandler>();
            _sessionService = Startup.GetFromFac<ISessionService>();


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
        }

        public CacheCommandHandler(
            ILogger<CacheCommandHandler> _Logger,
            ISessionService sessionService
           )
            : base(_Logger)
        {
            logger = _Logger;
            _sessionService = sessionService;
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            CacheCommands.CacheDataList.FullCode,
            CacheCommands.CacheSync.FullCode,
            CacheCommands.CacheUpdate.FullCode,
            CacheCommands.CacheDelete.FullCode
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 80;

        /// <summary>
        /// 判断是否可以处理指定命令
        /// </summary>
        public override bool CanHandle(ICommand command)
        {
            return command is CacheCommand ||
                   command.CommandIdentifier == CacheCommands.CacheDataList ||
                   command.CommandIdentifier == CacheCommands.CacheSync ||
                   command.CommandIdentifier == CacheCommands.CacheUpdate ||
                   command.CommandIdentifier == CacheCommands.CacheDelete;
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<ResponseBase> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;
                if (commandId == CacheCommands.CacheDataList)
                {
                    return await HandleCacheRequestAsync(command, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheSync)
                {
                    return await HandleCacheSyncAsync(command, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheUpdate)
                {
                    return await HandleCacheUpdateAsync(command, cancellationToken);
                }
                else if (commandId == CacheCommands.CacheDelete)
                {
                    return await HandleCacheDeleteAsync(command, cancellationToken);
                }
                else
                {
                    var errorResponse = ResponseBase.CreateError($"不支持的缓存命令类型: {command.CommandIdentifier}", 400)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_COMMAND");
                    return ConvertToApiResponse(errorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理缓存命令时发生异常");
                var errorResponse = ResponseBase.CreateError($"缓存命令处理失败: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_PROCESSING_ERROR");
                return ConvertToApiResponse(errorResponse);
            }
        }

        /// <summary>
        /// 处理缓存请求
        /// </summary>
        /// <param name="command">缓存请求命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> HandleCacheRequestAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取请求数据
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    return ResponseBase.CreateError("缓存请求数据为空", 400);
                }

                // 解析请求数据
                string json = Encoding.UTF8.GetString(packet.Body);
                var cacheRequest = JsonConvert.DeserializeObject<CacheRequest>(json);
                if (cacheRequest == null)
                {
                    return ResponseBase.CreateError("解析缓存请求数据失败", 400);
                }

                // 验证表名
                if (string.IsNullOrEmpty(cacheRequest.TableName))
                {
                    return ResponseBase.CreateError("表名不能为空", 400);
                }

                // 记录调试信息
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintInfoLog($"客户端请求缓存: {cacheRequest.TableName}, 强制刷新: {cacheRequest.ForceRefresh}");
                }

                // 检查是否需要刷新缓存
                if (cacheRequest.ForceRefresh || !IsCacheValid(cacheRequest.TableName, cacheRequest.LastRequestTime))
                {
                    // 刷新缓存数据
                    RefreshCacheDataAsync(cacheRequest.TableName);
                }
                //BizCacheHelper.Instance.GetEntity(tableName);
                //// 准备缓存响应
                //var cacheData = await GetCacheDataAsync(cacheRequest.TableName);
                //var cacheResponse = new CacheResponse
                //{
                //    RequestId = cacheRequest.RequestId,
                //    StatusCode = 200,
                //    Message = "成功",
                //    CacheData = cacheData,
                //    TableName = cacheRequest.TableName,
                //    CacheTime = DateTime.Now,
                //    ExpireTime = DateTime.Now.AddMinutes(30), // 默认30分钟过期
                //    HasMoreData = false, // 暂时不支持分批发送
                //    ServerVersion = Program.AppVersion
                //};

                //// 发送缓存数据到客户端
                //await SendCacheResponseAsync(packet.SessionId, cacheResponse);

                return ResponseBase.CreateSuccess("缓存请求处理成功");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理缓存请求时发生异常");
                return ResponseBase.CreateError($"处理缓存请求失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 处理缓存同步
        /// </summary>
        /// <param name="command">缓存同步命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> HandleCacheSyncAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var syncCommand = command as CacheCommand;
                if (syncCommand == null)
                {
                    return ResponseBase.CreateError("无效的缓存同步命令", 400);
                }

                // 根据同步模式处理
                if (syncCommand.SyncMode == "FULL" || syncCommand.CacheKeys == null || syncCommand.CacheKeys.Count == 0)
                {
                    // 全量同步
                    await SyncAllCacheAsync(syncCommand);
                }
                else
                {
                    // 增量同步指定的缓存键
                    await SyncSpecificCacheAsync(syncCommand.CacheKeys);
                }

                return ResponseBase.CreateSuccess("缓存同步成功");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理缓存同步时发生异常");
                return ResponseBase.CreateError($"处理缓存同步失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 处理缓存更新
        /// </summary>
        /// <param name="command">缓存更新命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> HandleCacheUpdateAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 解析更新数据
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    return ResponseBase.CreateError("缓存更新数据为空", 400);
                }

                // 处理更新逻辑
                string json = Encoding.UTF8.GetString(packet.Body);
                dynamic updateData = JsonConvert.DeserializeObject(json);
                string tableName = updateData?.TableName;
                object data = updateData?.Data;

                if (string.IsNullOrEmpty(tableName))
                {
                    return ResponseBase.CreateError("表名不能为空", 400);
                }

                // 更新缓存
                #region 
                // 将item转换为JObject
                JObject obj = JObject.Parse(json);
                MyCacheManager.Instance.UpdateEntityList(tableName, obj);
                //如果是产品表有变化 还要需要更新产品视图的缓存
                if (tableName == nameof(tb_Prod))
                {
                    var prod = obj.ToObject<tb_Prod>();
                    //toDO
                    //BroadcastProdCatchData(UserSession, prod);
                }
                #endregion

                // 广播给其他客户端
                await BroadcastCacheUpdateAsync(tableName, data);

                return ResponseBase.CreateSuccess("缓存更新成功");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理缓存更新时发生异常");
                return ResponseBase.CreateError($"处理缓存更新失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 处理缓存删除
        /// </summary>
        /// <param name="command">缓存删除命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> HandleCacheDeleteAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 解析删除数据
                var packet = command.Packet;
                if (packet == null || packet.Body == null || packet.Body.Length == 0)
                {
                    return ResponseBase.CreateError("缓存删除数据为空", 400);
                }

                // 处理删除逻辑
                string json = Encoding.UTF8.GetString(packet.Body);
                dynamic deleteData = JsonConvert.DeserializeObject(json);
                string tableName = deleteData?.TableName;

                if (string.IsNullOrEmpty(tableName))
                {
                    return ResponseBase.CreateError("表名不能为空", 400);
                }

                // 删除缓存 还要看传过来要删除的具体值，还要通知其它用户
                //todo 
                //  MyCacheManager.Instance.DeleteEntityList(tableName, PKColName, PKValue);
                // 广播给其他客户端
                await BroadcastCacheDeleteAsync(tableName);

                return ResponseBase.CreateSuccess("缓存删除成功");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理缓存删除时发生异常");
                return ResponseBase.CreateError($"处理缓存删除失败: {ex.Message}", 500);
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
                byte[] dataBytes = Encoding.UTF8.GetBytes(json);

                // 广播给所有客户端
                //var allSessions = _sessionService.GetAllSessions();
                //foreach (var session in allSessions)
                //{
                //    OriginalData od = new OriginalData
                //    {
                //        Command = (byte)ServerCommand.缓存更新通知,
                //        Two = ByteDataAnalysis.PackString(json)
                //    };
                //    byte[] buffer = CryptoProtocol.EncryptServerPackToClient(od);
                //    await session.SendAsync(buffer);
                //}
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"广播缓存更新失败: {tableName}");
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
                //var allSessions = _sessionService.GetUserSessions();
                //foreach (var session in allSessions)
                //{
                //    OriginalData od = new OriginalData
                //    {
                //        Command = (byte)ServerCommand.缓存删除通知,
                //        Two = ByteDataAnalysis.PackString(json)
                //    };
                //    byte[] buffer = CryptoProtocol.EncryptServerPackToClient(od);
                //    await session.SendAsync(buffer);
                //}
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"广播缓存删除失败: {tableName}");
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
    }
}