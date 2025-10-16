using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using RUINORERP.PacketSpec.Commands.Cache;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Business.Security;
using RUINORERP.PacketSpec.Models.Core;
using System.Windows.Forms;
using RUINORERP.Business.CommService;
using FastReport.Table;
using Newtonsoft.Json.Linq;
using RUINORERP.Extensions.Middlewares;
using System.Reflection;
using RUINORERP.Common.Helper;
using RUINORERP.SecurityTool;
using System.Collections;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Cache;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端缓存服务 - 处理缓存请求和响应
    /// 实现IDisposable接口以支持资源清理
    /// 缓存订阅管理在客户端和服务器端的业务作用 ：
    //客户端 ：

    //- 跟踪本地订阅的表，当收到服务器推送的缓存变更时知道如何处理
    //- 允许客户端主动订阅/取消订阅感兴趣的数据表，以接收实时更新
    //- 优化网络流量，只订阅客户端真正需要的表
    //- 提供本地缓存更新机制，收到服务器推送时自动更新本地缓存
    //服务器端 ：

    //- 管理哪些会话订阅了哪些表，数据变更时知道需要通知哪些客户端
    //- 实现缓存变更广播机制，确保所有订阅了特定表的客户端都能及时收到更新
    //- 当会话断开连接时，自动取消其所有订阅，避免资源泄露
    //- 优化服务器性能，只向需要特定数据的客户端发送更新
    /// </summary>
    public class CacheClientService : IDisposable
    {
        private readonly ClientCommunicationService _comm;
        private readonly ILogger<CacheClientService> _log;
        private readonly ICommandCreationService _commandCreationService;
        private readonly CacheSubscriptionManager _subscriptionManager; // 使用统一的订阅管理器
        private readonly Dictionary<string, DateTime> _lastRequestTimes = new Dictionary<string, DateTime>();
        private readonly object _lockObj = new object();
        private IAuthorizeController authorizeController;
        private bool _disposed = false;
        // 添加新的缓存管理器字段
        private readonly IEntityCacheManager _cacheManager;
        private readonly EventDrivenCacheManager _eventDrivenCacheManager;

        /// <summary>
        /// 构造函数 - 通过DI注入依赖项
        /// </summary>
        /// <param name="comm">通信服务</param>
        /// <param name="commandCreationService">命令创建服务</param>
        /// <param name="subscriptionManager">缓存订阅管理器</param>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="log">日志记录器</param>
        public CacheClientService(
            ClientCommunicationService comm,
            ICommandCreationService commandCreationService,
            CacheSubscriptionManager subscriptionManager, // 通过DI注入
            IEntityCacheManager cacheManager, // 通过DI注入
            EventDrivenCacheManager eventDrivenCacheManager,
            ILogger<CacheClientService> log = null)
        {
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            _log = log;
            _commandCreationService = commandCreationService ?? throw new ArgumentNullException(nameof(commandCreationService));
            _subscriptionManager = subscriptionManager; // 通过DI注入
            _subscriptionManager.SetCommunicationService(comm); // 设置通信服务
            // 初始化新的缓存管理器
            _cacheManager = cacheManager; // 通过DI注入
            _eventDrivenCacheManager = eventDrivenCacheManager;
            // 订阅缓存变更事件
            _eventDrivenCacheManager.CacheChanged += OnClientCacheChanged;
            // 注册缓存响应处理
            RegisterCommandHandlers();
            // 注册缓存通知事件处理程序
            RegisterNotificationHandler();
            try
            {
                authorizeController = Startup.GetFromFac<IAuthorizeController>();
            }
            catch { }
        }

        private void RegisterCommandHandlers()
        {
            // 使用简化的缓存命令系统
            // 处理缓存操作和同步命令
            _comm.SubscribeCommand(CacheCommands.CacheOperation, (packet, data) => HandleCacheResponse((CacheResponse)data));
            _comm.SubscribeCommand(CacheCommands.CacheSync, (packet, data) => HandleCacheResponse((CacheResponse)data));

            // 处理订阅命令 - 使用统一的处理方法，内部根据SubscribeAction区分
            _comm.SubscribeCommand(CacheCommands.CacheSubscription, (packet, data) => HandleSubscriptionResponse((CacheResponse)data));
        }

        /// <summary>
        /// 处理缓存订阅响应 - 根据SubscribeAction区分订阅和取消订阅操作
        /// </summary>
        /// <param name="response">缓存响应数据</param>
        private void HandleSubscriptionResponse(CacheResponse response)
        {
            if (response == null || !response.IsSuccess)
            {
                _log?.LogWarning("接收到无效的订阅响应或响应失败: {0}", response?.ErrorMessage);
                return;
            }

            try
            {
                // 从Data中获取订阅信息
                if (response.Metadata != null)
                {
                    // 获取SubscribeAction信息
                    int subscribeAction = 0;
                    if (response.Metadata.ContainsKey("SubscribeAction"))
                    {
                        subscribeAction = Convert.ToInt32(response.Metadata["SubscribeAction"]);
                    }
                    var tableName = response.TableName;

                    if (!string.IsNullOrEmpty(tableName))
                    {
                        switch ((SubscribeAction)subscribeAction)
                        {
                            case SubscribeAction.Subscribe:
                                _log?.LogInformation($"成功订阅表缓存变更: {tableName}");
                                break;
                            case SubscribeAction.Unsubscribe:
                                _log?.LogInformation($"成功取消订阅表缓存变更: {tableName}");
                                break;
                            default:
                                _log?.LogWarning("接收到未知的订阅动作类型: {0}", subscribeAction);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存订阅响应时发生异常");
            }
        }

        /// <summary>
        /// 注册缓存通知事件处理程序
        /// </summary>
        private void RegisterNotificationHandler()
        {
            _comm.CommandReceived += async (command, data) =>
            {
                try
                {
                    // 移除对CacheNotificationHandler的依赖，直接处理通知数据
                }
                catch (Exception ex)
                {
                    _log?.LogError(ex, "处理缓存通知时发生异常");
                }
            };
        }

        /// <summary>
        /// 处理缓存变更通知
        /// </summary>
        /// <param name="data">通知数据（JSON格式的字节数组）</param>
        private async Task HandleCacheNotificationAsync(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                {
                    _log?.LogWarning("接收到空的缓存通知数据");
                    return;
                }

                // 将字节数组转换为字符串
                string json = Encoding.UTF8.GetString(data);

                // 解析通知数据
                var notification = JsonConvert.DeserializeObject<CacheNotification>(json);

                if (notification == null)
                {
                    _log?.LogWarning("无法解析缓存通知数据");
                    return;
                }

                _log?.LogInformation($"接收到缓存{notification.Type}通知，表名: {notification.TableName}");

                // 根据通知类型处理
                switch (notification.Type?.ToLower())
                {
                    case "update":
                        await HandleUpdateNotificationAsync(notification);
                        break;
                    case "delete":
                        await HandleDeleteNotificationAsync(notification);
                        break;
                    default:
                        _log?.LogWarning($"未知的缓存通知类型: {notification.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存通知时发生异常");
            }
        }

        /// <summary>
        /// 处理缓存更新通知
        /// </summary>
        /// <param name="notification">通知数据</param>
        private async Task HandleUpdateNotificationAsync(CacheNotification notification)
        {
            try
            {
                if (notification.Data == null)
                {
                    _log?.LogWarning($"缓存更新通知数据为空，表名: {notification.TableName}");
                    return;
                }

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(notification.TableName);
                if (entityType == null)
                {
                    _log?.LogWarning($"未找到表 {notification.TableName} 的实体类型");
                    return;
                }

                // 更新缓存
                if (notification.Data is Newtonsoft.Json.Linq.JObject jObject)
                {
                    _cacheManager.UpdateEntityList(notification.TableName, jObject);
                }
                else if (notification.Data is Newtonsoft.Json.Linq.JArray jArray)
                {
                    _cacheManager.UpdateEntityList(notification.TableName, jArray);
                }
                else
                {
                    // 尝试将其他类型转换为JObject
                    string json = JsonConvert.SerializeObject(notification.Data);
                    var parsedObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                    _cacheManager.UpdateEntityList(notification.TableName, parsedObject);
                }

                _log?.LogInformation($"成功处理缓存更新通知，表名: {notification.TableName}");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, $"处理缓存更新通知时发生异常，表名: {notification.TableName}");
            }
        }

        /// <summary>
        /// 处理缓存删除通知
        /// </summary>
        /// <param name="notification">通知数据</param>
        private async Task HandleDeleteNotificationAsync(CacheNotification notification)
        {
            try
            {
                // 删除整个表的缓存
                var entityType = _cacheManager.GetEntityType(notification.TableName);
                if (entityType != null)
                {
                    // 注意：这里可能需要更复杂的逻辑来清空整个表的缓存
                    _log?.LogInformation($"处理缓存删除通知，表名: {notification.TableName}");
                }
                else
                {
                    _log?.LogWarning($"未找到表 {notification.TableName} 的实体类型，无法处理删除通知");
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, $"处理缓存删除通知时发生异常，表名: {notification.TableName}");
            }
        }

        /// <summary>
        /// 处理客户端缓存变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void OnClientCacheChanged(object sender, CacheChangedEventArgs e)
        {
            try
            {
                // 只处理需要同步到服务器的变更
                if (!e.SyncToServer)
                    return;

                // 创建缓存更新请求 - 使用简化的操作类型
                var request = new CacheRequest
                {
                    TableName = e.Key,
                    Operation = e.Operation, // 直接使用事件中的操作类型
                    RequestId = IdGenerator.GenerateRequestId(CacheCommands.CacheOperation)
                };

                // 根据操作类型设置请求数据
                if (e.Value != null)
                {
                    request.Data = e.Value;
                }

                // 发送缓存更新命令到服务器 - 使用统一的缓存操作命令
                var command = new BaseCommand<IRequest, IResponse>()
                {
                    Request = request,
                    CommandIdentifier = CacheCommands.CacheOperation
                };

                // 发送命令到服务器
                await _comm.SendOneWayCommandAsync<IRequest>(command, CancellationToken.None);

                _log?.LogInformation($"客户端缓存变更已同步到服务器: {e.Key}, 操作: {e.Operation.ToString()}");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, $"同步客户端缓存变更到服务器时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 请求缓存数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>缓存响应结果</returns>
        public async Task RequestCacheAsync(string tableName, bool forceRefresh = false, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentException("表名不能为空", nameof(tableName));
                var tableNames = TableSchemaManager.Instance.GetAllTableNames();
                if (!tableNames.Contains(tableName))
                {
                    await Task.CompletedTask;
                    return;
                }

                // 手动创建Get操作的缓存请求
                var request = new CacheRequest
                {
                    TableName = tableName,
                    Operation = CacheOperation.Get,
                    ForceRefresh = forceRefresh
                };
                request.LastRequestTime = GetLastRequestTime(tableName);

                // 使用统一命令处理模式，模仿登录业务流程
                var response = await ProcessCacheRequestAsync(request, ct);

                HandleCacheResponse(response);

                // 自动订阅该表的缓存变更通知
                await _subscriptionManager.SubscribeAsync(tableName);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "请求缓存过程中发生异常，表名={0}", tableName);
                // 记录异常信息但不返回
                if (ex.InnerException != null)
                {
                    _log?.LogError(ex.InnerException, "请求缓存过程中发生异常InnerException，表名={0}", tableName);
                }
            }
        }

        /// <summary>
        /// 订阅缓存变更通知
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否订阅成功</returns>
        /// <summary>
        /// 订阅缓存变更通知 - 使用简化的订阅命令
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否订阅成功</returns>
        public async Task<bool> SubscribeCacheAsync(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _log?.LogWarning("订阅缓存时表名为空");
                    return false;
                }

                // 使用简化的订阅请求创建方法
                var request = CacheRequest.CreateSubscriptionRequest(tableName, SubscribeAction.Subscribe);
                // 使用正确的客户端订阅方法（异步版本）
                return await _subscriptionManager.SubscribeAsync(tableName);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, $"订阅缓存 {tableName} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 取消订阅缓存变更通知 - 使用简化的订阅命令
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否取消订阅成功</returns>
        public async Task<bool> UnsubscribeCacheAsync(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _log?.LogWarning("取消订阅缓存时表名为空");
                    return false;
                }

                // 使用简化的订阅请求创建方法
                var request = CacheRequest.CreateSubscriptionRequest(tableName, SubscribeAction.Unsubscribe);
                // 使用正确的客户端取消订阅方法（异步版本）
                return await _subscriptionManager.UnsubscribeAsync(tableName);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, $"取消订阅缓存 {tableName} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 取消所有订阅
        /// </summary>
        public async Task UnsubscribeAllAsync()
        {
            try
            {
                await _subscriptionManager.UnsubscribeAllAsync();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "取消所有订阅时发生异常");
            }
        }

        /// <summary>
        /// 统一处理缓存请求 
        /// </summary>
        /// <param name="request">缓存请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>缓存响应结果</returns>
        private async Task<BaseCommand<CacheResponse>> ProcessCacheRequestAsync(CacheRequest request, CancellationToken ct)
        {
            try
            {
                // 使用CommandDataBuilder构建命令
                //var cacheCommand = CommandDataBuilder.BuildCommand<CacheRequest, CacheResponse>(
                //    CacheCommands.CacheRequest,
                //    request,
                //    cmd => cmd.TimeoutMs = 30000
                //);

                var cacheCommand = CommandDataBuilder.BuildCommand<CacheRequest, CacheResponse>(CacheCommands.CacheOperation, request);

                // 使用新的方法发送命令并获取包含指令信息的响应
                var commandResponse = await _comm.SendCommandWithResponseAsync<CacheRequest, CacheResponse>(cacheCommand, ct, 30000);

                // 检查响应数据是否为空
                if (commandResponse.ResponseData == null)
                {
                    _log?.LogError("缓存请求失败：服务器返回了空的响应数据");
                    return BaseCommand<CacheResponse>.CreateError("服务器返回了空的响应数据");
                }

                // 检查响应是否成功
                if (!commandResponse.ResponseData.IsSuccess)
                {
                    return BaseCommand<CacheResponse>.CreateError(commandResponse.ResponseData.ErrorMessage)
                        .WithMetadata("RequestId", commandResponse.ResponseData.RequestId ?? string.Empty);
                }

                // 缓存请求成功后处理数据
                if (commandResponse.ResponseData.IsSuccess && commandResponse.ResponseData != null)
                {
                    var cacheResponse = commandResponse.ResponseData;

                    // 处理缓存数据
                    //ProcessCacheData(request.TableName, cacheResponse.CacheData);

                    // 更新请求时间
                    UpdateLastRequestTime(request.TableName);

                    if (authorizeController != null && authorizeController.GetShowDebugInfoAuthorization())
                    {
                        //MainForm.Instance.PrintInfoLog($"接收缓存成功: {request.TableName}, 数据量: {cacheResponse.CacheData?.Count ?? 0}");
                    }

                }
                else
                {
                    _log?.LogWarning("缓存请求失败，表名={0}，错误: {1}",
                        request.TableName, commandResponse.ResponseData.ErrorMessage);
                }

                return commandResponse;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存请求失败，表名={0}", request.TableName);
                throw;
            }
        }

        private DateTime GetLastRequestTime(string tableName)
        {
            lock (_lockObj)
            {
                if (_lastRequestTimes.TryGetValue(tableName, out var time))
                    return time;
                return DateTime.MinValue;
            }
        }

        private void UpdateLastRequestTime(string tableName)
        {
            lock (_lockObj)
            {
                _lastRequestTimes[tableName] = DateTime.Now;
            }
        }

        /// <summary>
        /// 处理缓存响应
        /// </summary>
        /// <param name="data">缓存响应数据</param>
        private void HandleCacheResponse(object data)
        {
            try
            {
                if (data == null)
                {
                    return;
                }
                if (data is BaseCommand<CacheResponse> baseCommand)
                {
                    // 确保data是CacheResponse类型
                    if (baseCommand.ResponseData is CacheResponse response)
                    {
                        if (response.IsSuccess && !string.IsNullOrEmpty(response.TableName))
                        {
                            // 处理缓存数据
                            ProcessCacheData(response.TableName, response.CacheData);

                            if (authorizeController != null && authorizeController.GetShowDebugInfoAuthorization())
                            {
                                //    MainForm.Instance.PrintInfoLog($"接收缓存: {response.TableName}, 数据量: {response.CacheData?.Count ?? 0}");
                            }

                            // 如果有更多数据，继续请求
                            if (response.HasMoreData)
                            {
                                Task.Run(async () =>
                                {
                                    await Task.Delay(100);
                                    await RequestCacheAsync(response.TableName, false);
                                });
                            }
                        }
                        else
                        {
                            _log.LogDebug($"接收缓存失败: {response?.Message ?? "未知错误"}");
                        }
                    }
                    else
                    {
                        _log.LogDebug("处理缓存响应失败: 数据格式不正确");
                    }
                }

            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存响应失败: {Message}", ex.Message);
                _log.LogDebug($"处理缓存响应失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheData">缓存数据</param>
        private void ProcessCacheData(string tableName, CacheData cacheData)
        {
            try
            {
                if (cacheData?.Data != null)
                {
                    // 如果Data是字符串，尝试解析为JArray
                    if (cacheData.Data is string jsonString)
                    {
                        var jArray = JArray.Parse(jsonString);
                        // 使用新的缓存管理器更新缓存
                        _cacheManager.UpdateEntityList(tableName, jArray);
                    }
                    // 如果Data已经是JArray类型（兼容旧版本）
                    else if (cacheData.Data is JArray jArray)
                    {
                        // 使用新的缓存管理器更新缓存
                        _cacheManager.UpdateEntityList(tableName, jArray);
                    }
                    else
                    {
                        // 尝试将其他类型转换为JArray
                        string json = JsonConvert.SerializeObject(cacheData.Data);
                        var parsedArray = JArray.Parse(json);
                        // 使用新的缓存管理器更新缓存
                        _cacheManager.UpdateEntityList(tableName, parsedArray);
                    }

                    _log?.LogInformation("缓存数据处理完成，表名={0}", tableName);
                }
                else
                {
                    _log?.LogWarning("缓存数据为空，表名={0}", tableName);
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存数据失败: {Message}", ex.Message);

            }
        }

        /// <summary>
        /// 将字典数据转换为实体列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheData">缓存数据</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>转换后的实体列表</returns>
        private List<object> ConvertDictionaryToEntityList(string tableName, Dictionary<string, object> cacheData, Type entityType)
        {
            try
            {
                // 尝试直接反序列化或转换数据
                var jsonData = JsonConvert.SerializeObject(cacheData);
                var list = JsonConvert.DeserializeObject(jsonData, typeof(List<>).MakeGenericType(entityType));

                if (list is IEnumerable enumerableList)
                {
                    return enumerableList.Cast<object>().ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "转换缓存数据为实体列表失败: {Message}", ex.Message);
                return null;
            }
        }

        #region
        /// <summary>
        /// 处理缓存清空响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheClearResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {

                        _log.LogDebug("缓存清空成功");
                    }
                    else
                    {
                        _log?.LogWarning("缓存清空失败: {Message}", response.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存清空响应失败");

            }
        }

        /// <summary>
        /// 处理缓存统计响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheStatisticsResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log?.LogDebug("缓存统计获取成功");

                    }
                    else
                    {
                        _log?.LogWarning("缓存统计获取失败: {Message}", response.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存统计响应失败");

            }
        }

        /// <summary>
        /// 处理缓存状态响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheStatusResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log.LogDebug("缓存状态获取成功");

                    }
                    else
                    {
                        _log?.LogWarning("缓存状态获取失败: {Message}", response.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存状态响应失败");

            }
        }

        /// <summary>
        /// 处理缓存批量操作响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheBatchOperationResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log.LogDebug("缓存批量操作成功");

                    }
                    else
                    {
                        _log?.LogWarning("缓存批量操作失败: {Message}", response.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存批量操作响应失败");

            }
        }

        /// <summary>
        /// 处理缓存预热响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheWarmupResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log?.LogInformation("缓存预热成功");

                    }
                    else
                    {
                        _log?.LogWarning("缓存预热失败: {Message}", response.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存预热响应失败");
            }
        }

        /// <summary>
        /// 处理缓存失效响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheInvalidateResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log?.LogInformation("缓存失效成功");
                    }
                    else
                    {
                        _log?.LogWarning("缓存失效失败: {Message}", response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存失效响应失败");
            }
        }

        /// <summary>
        /// 处理缓存订阅响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheSubscribeResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log?.LogInformation($"缓存订阅成功: {response.TableName}");
                    }
                    else
                    {
                        _log?.LogWarning("缓存订阅失败: {Message}", response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存订阅响应失败");
            }
        }

        /// <summary>
        /// 处理缓存取消订阅响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public void HandleCacheUnsubscribeResponse(object data)
        {
            try
            {
                if (data is CacheResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _log?.LogInformation($"缓存取消订阅成功: {response.TableName}");
                    }
                    else
                    {
                        _log?.LogWarning("缓存取消订阅失败: {Message}", response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存取消订阅响应失败");
            }
        }

        #endregion

        #region 缓存辅助方法
        /// <summary>
        /// 处理JArray类型的缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="objList">JArray类型的对象列表</param>
        private void GetTableList(string tableName, object objList)
        {
            if (objList != null && objList is JArray jArrayData)
            {
                // 获取表对应的实体类型
                // 使用新的缓存管理器获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType != null)
                {
                    // 转换JArray到实体列表
                    try
                    {
                        var entityList = ConvertJArrayToList(jArrayData, entityType);
                        if (entityList != null && entityList.Count > 0)
                        {
                            // 更新缓存
                            // 使用新的缓存管理器更新缓存
                            _cacheManager.UpdateEntityList(tableName, entityList);
                            _log?.LogInformation("JArray缓存数据处理完成，表名={0}，数据量={1}", tableName, entityList.Count);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log?.LogError(ex, "转换JArray缓存数据失败: {Message}", ex.Message);
                    }
                }
                else
                {
                    _log?.LogWarning("未找到缓存类型配置，表名={0}", tableName);
                }
            }
        }

        /// <summary>
        /// 将JArray转换为指定类型的对象列表
        /// </summary>
        /// <param name="jArray">JArray数据</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>转换后的对象列表</returns>
        private List<object> ConvertJArrayToList(JArray jArray, Type entityType)
        {
            if (jArray == null)
                throw new ArgumentNullException(nameof(jArray));

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            try
            {
                // 使用JToken.ToObject<T>()方法将每个JToken转换为指定类型
                MethodInfo toObjectMethod = typeof(JToken).GetMethod("ToObject", new Type[] { });
                MethodInfo genericToObjectMethod = toObjectMethod.MakeGenericMethod(entityType);

                List<object> list = new List<object>(jArray.Count);
                foreach (JToken token in jArray)
                {
                    object item = genericToObjectMethod.Invoke(token, null);
                    list.Add(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("转换JArray失败", ex);
            }
        }



        /// <summary>
        /// 统一处理从缓存获取名称 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">外键ID</param>
        /// <returns>对应的名称</returns>
        private async Task<string> ProcessGetNameFromCacheAsync(string tableName, long id)
        {
            _log?.LogDebug("从缓存获取名称，表名={0}，ID={1}", tableName, id);

            // 检查表是否在缓存中
            // 使用新的缓存管理器检查表是否存在
            var entityType = _cacheManager.GetEntityType(tableName);
            if (entityType == null)
            {
                _log?.LogWarning("表{0}不在缓存配置中，尝试请求缓存", tableName);
                // 如果不在缓存中，尝试请求
                await Task.Run(async () => await RequestCacheAsync(tableName));
                return string.Empty;
            }

            // 获取缓存数据
            // 使用新的缓存管理器获取实体列表
            var schemaInfo = TableSchemaManager.Instance.GetSchemaInfo(tableName);
            if (schemaInfo == null)
            {
                _log?.LogWarning("表{0}的结构信息为空", tableName);
                return string.Empty;
            }

            // 查找对应ID的记录
            // 使用新的缓存管理器获取实体
            var entity = _cacheManager.GetEntity(tableName, id);
            if (entity != null)
            {
                string name = entity.GetCachePropertyValue(schemaInfo.DisplayField)?.ToString() ?? string.Empty;
                _log?.LogDebug("从缓存获取名称成功，表名={0}，ID={1}，名称={2}", tableName, id, name);
                return name;
            }

            _log?.LogWarning("在表{0}的缓存中未找到ID={1}的记录", tableName, id);
            return string.Empty;
        }

        /// <summary>
        /// 清理指定表的缓存 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ClearCache(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _log?.LogWarning("清理缓存时表名为空");
                    return;
                }

                // 使用统一命令处理模式清理缓存
                ProcessClearCacheAsync(tableName).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理缓存失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 统一处理清理缓存 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>任务</returns>
        private async Task ProcessClearCacheAsync(string tableName)
        {
            _log?.LogInformation("开始清理缓存，表名={0}", tableName);
            // 使用新的缓存管理器删除实体
            _cacheManager.DeleteEntity(tableName, null);
            _log?.LogInformation("清理缓存完成，表名={0}", tableName);
        }

        /// <summary>
        /// 清理所有缓存 - 统一使用泛型命令处理模式
        /// </summary>
        public void ClearAllCache()
        {
            try
            {
                // 使用统一命令处理模式清理所有缓存
                ProcessClearAllCacheAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理所有缓存失败: {Message}", ex.Message);
            }
        }

        /// <summary>
        /// 统一处理清理所有缓存 - 模仿登录业务流程
        /// </summary>
        /// <returns>任务</returns>
        private async Task ProcessClearAllCacheAsync()
        {
            _log?.LogInformation("开始清理所有缓存");
            // 使用新的缓存管理器清空所有缓存
            // 注意：这里可能需要遍历所有表并清空每个表的缓存
            // 由于新的缓存系统可能没有直接的Clear方法，我们可以通过其他方式实现
            lock (_lockObj)
            {
                _lastRequestTimes.Clear();
            }
            _log?.LogInformation("所有缓存清理完成");
        }

        /// <summary>
        /// 批量请求多个表的缓存数据 - 统一使用泛型命令处理模式
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>批量缓存响应结果</returns>
        public async Task<Dictionary<string, BaseCommand<CacheResponse>>> RequestMultipleCachesAsync(
            List<string> tableNames,
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            if (tableNames == null || tableNames.Count == 0)
            {
                throw new ArgumentException("表名列表不能为空", nameof(tableNames));
            }

            var results = new Dictionary<string, BaseCommand<CacheResponse>>();

            try
            {
                _log?.LogInformation("开始批量请求缓存数据，表数量={0}", tableNames.Count);

                // 使用统一命令处理模式批量处理请求
                return await ProcessMultipleCacheRequestsAsync(tableNames, forceRefresh, ct);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "批量请求缓存数据失败");
                // 创建一个包含异常信息的错误响应
                var errorResponse = BaseCommand<CacheResponse>.CreateError($"批量请求缓存数据失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    errorResponse.WithMetadata("InnerException", ex.InnerException.Message);
                }
                return new Dictionary<string, BaseCommand<CacheResponse>>
                {
                    { "Error", errorResponse }
                };
            }
        }

        /// <summary>
        /// 统一处理批量缓存请求 - 模仿登录业务流程
        /// </summary>
        /// <param name="tableNames">表名列表</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>批量缓存响应结果</returns>
        private async Task<Dictionary<string, BaseCommand<CacheResponse>>> ProcessMultipleCacheRequestsAsync(
            List<string> tableNames,
            bool forceRefresh,
            CancellationToken ct)
        {
            var results = new Dictionary<string, BaseCommand<CacheResponse>>();

            try
            {
                // 并行处理多个表的缓存请求
                var tasks = tableNames.Select(tableName => ProcessSingleCacheRequestAsync(tableName, forceRefresh, ct)).ToList();
                var responses = await Task.WhenAll(tasks);

                // 将结果映射到字典中
                for (int i = 0; i < tableNames.Count; i++)
                {
                    results[tableNames[i]] = responses[i];
                }

                _log?.LogInformation("批量请求缓存数据完成，成功处理 {0} 个表", tableNames.Count);
                return results;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "批量处理缓存请求时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 处理单个表的缓存请求
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>缓存响应结果</returns>
        private async Task<BaseCommand<CacheResponse>> ProcessSingleCacheRequestAsync(
            string tableName,
            bool forceRefresh,
            CancellationToken ct)
        {
            try
            {
                // 使用工厂方法创建请求对象
                var request = CacheRequest.Create(tableName, forceRefresh);
                request.LastRequestTime = GetLastRequestTime(tableName);

                // 使用统一命令处理模式，模仿登录业务流程
                var cacheCommand = new CacheCommand();
                cacheCommand.Request = request;

                // 发送命令并获取响应
                var commandResponse = await _comm.SendCommandWithResponseAsync<CacheRequest, CacheResponse>(cacheCommand, ct, 30000);

                // 检查响应数据是否为空
                if (commandResponse.ResponseData == null)
                {
                    _log?.LogError("缓存请求失败：服务器返回了空的响应数据");
                    return BaseCommand<CacheResponse>.CreateError("服务器返回了空的响应数据");
                }

                // 处理成功响应
                if (commandResponse.ResponseData.IsSuccess && commandResponse.ResponseData != null)
                {
                    var cacheResponse = commandResponse.ResponseData;

                    // 处理缓存数据
                    ProcessCacheData(tableName, cacheResponse.CacheData);

                    // 更新请求时间
                    UpdateLastRequestTime(tableName);

                    _log?.LogInformation("成功处理表 {0} 的缓存请求", tableName);
                }
                else
                {
                    _log?.LogWarning("处理表 {0} 的缓存请求失败: {1}", tableName, commandResponse.ResponseData.ErrorMessage);
                }

                return commandResponse;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理表 {0} 的缓存请求时发生异常", tableName);
                return BaseCommand<CacheResponse>.CreateError($"处理表 {tableName} 的缓存请求时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取缓存状态信息
        /// </summary>
        /// <returns>缓存状态信息字典</returns>
        public async Task<Dictionary<string, CacheStatusInfo>> GetCacheStatusAsync()
        {
            try
            {
                _log?.LogDebug("开始获取缓存状态");

                // 使用统一命令处理模式获取缓存状态
                return await ProcessGetCacheStatusAsync();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取缓存状态时发生异常");
                return new Dictionary<string, CacheStatusInfo>();
            }
        }

        /// <summary>
        /// 统一处理获取缓存状态 - 模仿登录业务流程
        /// </summary>
        /// <returns>缓存状态信息</returns>
        private async Task<Dictionary<string, CacheStatusInfo>> ProcessGetCacheStatusAsync()
        {
            var statusInfo = new Dictionary<string, CacheStatusInfo>();

            _log?.LogDebug("开始获取缓存状态");
            // 使用新的缓存管理器获取所有表名
            var tableNames = TableSchemaManager.Instance.GetAllTableNames();
            foreach (var tableName in tableNames)
            {
                // 使用新的缓存管理器获取实体列表
                var entityType = _cacheManager.GetEntityType(tableName);
                int count = 0;
                if (entityType != null)
                {
                    // 获取实体列表并计算数量
                    try
                    {
                        var method = typeof(IEntityCacheManager).GetMethod("GetEntityList");
                        var genericMethod = method.MakeGenericMethod(entityType);
                        var entityList = genericMethod.Invoke(_cacheManager, new object[] { tableName });
                        if (entityList is IEnumerable enumerable)
                        {
                            count = enumerable.Cast<object>().Count();
                        }
                    }
                    catch (Exception ex)
                    {
                        _log?.LogWarning(ex, "获取表 {0} 的实体数量时发生异常", tableName);
                    }
                }

                statusInfo[tableName] = new CacheStatusInfo
                {
                    TableName = tableName,
                    RecordCount = count,
                    LastUpdated = DateTime.Now,
                    IsCached = count > 0
                };
            }

            _log?.LogDebug("获取缓存状态完成，共处理 {0} 个表", statusInfo.Count);
            return statusInfo;
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public async Task<CacheStatistics> GetCacheStatisticsAsync()
        {
            try
            {
                _log?.LogDebug("开始获取缓存统计信息");

                // 使用统一命令处理模式获取缓存统计
                return await ProcessGetCacheStatisticsAsync();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取缓存统计信息时发生异常");
                return new CacheStatistics();
            }
        }

        /// <summary>
        /// 统一处理获取缓存统计 - 模仿登录业务流程
        /// </summary>
        /// <returns>缓存统计信息</returns>
        private async Task<CacheStatistics> ProcessGetCacheStatisticsAsync()
        {
            var statistics = new CacheStatistics();

            try
            {
                _log?.LogDebug("开始获取缓存统计信息");

                // 获取所有表的统计信息
                var tableNames = TableSchemaManager.Instance.GetAllTableNames();
                foreach (var tableName in tableNames)
                {
                    // 使用新的缓存管理器获取实体列表
                    var entityType = _cacheManager.GetEntityType(tableName);
                    if (entityType != null)
                    {
                        try
                        {
                            var method = typeof(IEntityCacheManager).GetMethod("GetEntityList");
                            var genericMethod = method.MakeGenericMethod(entityType);
                            var entityList = genericMethod.Invoke(_cacheManager, new object[] { tableName });
                            if (entityList is IEnumerable enumerable)
                            {
                                statistics.TotalRecords += enumerable.Cast<object>().Count();
                                statistics.CachedTables.Add(tableName);
                            }
                        }
                        catch (Exception ex)
                        {
                            _log?.LogWarning(ex, "获取表 {0} 的统计信息时发生异常", tableName);
                        }
                    }
                }

                statistics.TotalTables = tableNames.Count;
                statistics.CachedTableCount = statistics.CachedTables.Count;

                _log?.LogDebug("获取缓存统计信息完成: 总表数={0}, 缓存表数={1}, 总记录数={2}",
                    statistics.TotalTables, statistics.CachedTableCount, statistics.TotalRecords);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存统计信息时发生异常");
            }

            return statistics;
        }

        /// <summary>
        /// 缓存通知数据结构
        /// </summary>
        public class CacheNotification
        {
            /// <summary>
            /// 通知类型（Update/Delete）
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// 表名
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// 时间戳
            /// </summary>
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// 数据（更新时包含）
            /// </summary>
            public object Data { get; set; }
        }

        #region IDisposable Support
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    if (_eventDrivenCacheManager != null)
                    {
                        _eventDrivenCacheManager.CacheChanged -= OnClientCacheChanged;
                    }
                }

                // 释放非托管资源（如果有的话）

                _disposed = true;
            }
        }
        #endregion
    }

    /// <summary>
    /// 缓存状态信息
    /// </summary>
    public class CacheStatusInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 记录数量
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 是否已缓存
        /// </summary>
        public bool IsCached { get; set; }
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        /// <summary>
        /// 总表数
        /// </summary>
        public int TotalTables { get; set; }

        /// <summary>
        /// 缓存表数
        /// </summary>
        public int CachedTableCount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 缓存的表列表
        /// </summary>
        public List<string> CachedTables { get; set; } = new List<string>();
    }

    #endregion
}