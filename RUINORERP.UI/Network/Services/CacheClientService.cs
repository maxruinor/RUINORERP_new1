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
            CacheSubscriptionManager subscriptionManager, // 通过DI注入
            IEntityCacheManager cacheManager, // 通过DI注入
            EventDrivenCacheManager eventDrivenCacheManager,
            ILogger<CacheClientService> log = null)
        {
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            _log = log;
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
            // 只订阅CacheSync命令，避免与RequestCacheAsync方法中的显式调用重复
            _comm.SubscribeCommand(CacheCommands.CacheSync, (packet, data) =>
            {
                if (data is CacheResponse response)
                {
                    ProcessCacheResponse(response);
                }
            });

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
                                _log?.LogDebug($"成功订阅表缓存变更: {tableName}");
                                break;
                            case SubscribeAction.Unsubscribe:
                                _log?.LogDebug($"成功取消订阅表缓存变更: {tableName}");
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

                // 根据操作类型设置主键信息
                // CacheChangedEventArgs没有PrimaryKeyName属性，跳过这部分逻辑
                // 可以考虑在后续版本中扩展CacheChangedEventArgs类

                // 发送命令到服务器
                await _comm.SendOneWayCommandAsync<IRequest>(CacheCommands.CacheOperation, request, CancellationToken.None);

                _log?.LogDebug($"客户端缓存变更已同步到服务器: {e.Key}, 操作: {e.Operation.ToString()}");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, $"同步客户端缓存变更到服务器时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 请求缓存数据 - 统一使用泛型命令处理模式
        /// 根据缓存同步元数据智能决定是否需要重新请求，避免重复请求
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
                    _log?.LogDebug("请求的表名不在缓存列表中: {0}", tableName);
                    await Task.CompletedTask;
                    return;
                }

                // 限流检查：防止短时间内重复请求同一个表
                // 定义请求间隔时间，可根据实际需求调整（这里设置为5秒）
                TimeSpan minRequestInterval = TimeSpan.FromSeconds(5);

                // 获取最后请求时间并检查是否在限流时间内
                DateTime lastRequestTime = GetLastRequestTime(tableName);
                if (!forceRefresh && (DateTime.Now - lastRequestTime) < minRequestInterval)
                {
                    _log?.LogDebug("请求过于频繁，已限流: {0}, 距离上次请求时间: {1}秒",
                        tableName, (DateTime.Now - lastRequestTime).TotalSeconds);
                    // 即使被限流，也确保订阅了该表的变更通知
                    await _subscriptionManager.SubscribeAsync(tableName);
                    return;
                }

                // 智能缓存检查：检查是否需要请求缓存
                // 1. 如果强制刷新，则直接请求
                // 2. 检查本地缓存是否存在且有效
                // 3. 检查缓存同步元数据决定是否需要更新
                if (!forceRefresh && IsLocalCacheValid(tableName))
                {
                    _log?.LogDebug("本地缓存有效，无需重新请求: {0}", tableName);
                    // 确保订阅了该表的变更通知
                    await _subscriptionManager.SubscribeAsync(tableName);
                    return;
                }

                // 手动创建Get操作的缓存请求
                var request = new CacheRequest
                {
                    TableName = tableName,
                    Operation = CacheOperation.Get,
                    ForceRefresh = forceRefresh
                };

                // 传递最后请求时间供服务器参考
                request.LastRequestTime = GetLastRequestTime(tableName);

                // 使用统一命令处理模式，模仿登录业务流程
                var Response = await ProcessCacheRequestAsync(request, ct);

                // 直接使用ProcessCacheResponse处理缓存数据，避免重复执行
                if (Response != null)
                {
                    ProcessCacheResponse(Response as CacheResponse);
                }

                // 更新最后请求时间
                UpdateLastRequestTime(tableName);

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
        /// 检查本地缓存是否有效
        /// 根据缓存管理器中的数据判断是否需要重新请求
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>本地缓存是否有效</returns>
        private bool IsLocalCacheValid(string tableName)
        {
            try
            {
                // 检查本地缓存中是否存在该表的数据
                // 使用缓存管理器的方法尝试获取缓存数据
                // 生成标准的缓存键
                string cacheKey = $"{tableName}_List";

                // 尝试获取实体类型
                Type entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _log?.LogDebug("未找到表{0}对应的实体类型", tableName);
                    return false;
                }

                // 尝试使用GetEntityList方法检查缓存是否存在
                // 由于泛型限制，我们需要使用反射来调用
                try
                {
                    // 通过反射调用GetEntityList<T>方法
                    var getEntityListMethod = _cacheManager.GetType().GetMethod("GetEntityList", Type.EmptyTypes);
                    var genericMethod = getEntityListMethod?.MakeGenericMethod(entityType);

                    if (genericMethod != null)
                    {
                        var result = genericMethod.Invoke(_cacheManager, null);
                        if (result is IList cacheList && cacheList.Count > 0)
                        {
                            // 缓存存在且有数据，认为有效
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log?.LogDebug(ex, "通过反射检查缓存时发生异常: {0}", tableName);
                    // 反射调用失败，返回false表示需要重新请求
                    return false;
                }

                // 缓存不存在或为空，需要重新请求
                return false;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "检查本地缓存有效性时发生异常，表名={0}", tableName);
                // 发生异常时返回false，确保能获取到有效数据
                return false;
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
        private async Task<IResponse> ProcessCacheRequestAsync(CacheRequest request, CancellationToken ct)
        {
            try
            {
                // 使用新的方法发送命令并获取包含指令信息的响应
                var response = await _comm.SendCommandWithResponseAsync<IResponse>( CacheCommands.CacheOperation, request, ct, 30000);

                // 检查响应数据是否为空   
                if (response == null)
                {
                    _log?.LogError("缓存请求失败：服务器返回了空的响应数据");
                    return ResponseBase.CreateError("服务器返回了空的响应数据");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    return ResponseBase.CreateError(response.ErrorMessage)
                        .WithMetadata("RequestId", response.RequestId ?? string.Empty);
                }

                // 缓存请求成功后处理数据
                if (response.IsSuccess && response != null)
                {
                    var cacheResponse = response;

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
                        request.TableName, response.ErrorMessage);
                }

                return response;
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
        /// 处理缓存响应数据 - 直接接收CacheResponse类型
        /// </summary>
        /// <param name="response">缓存响应数据</param>
        private void ProcessCacheResponse(CacheResponse response)
        {
            try
            {
                if (response == null)
                {
                    return;
                }

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
                    _log?.LogDebug($"接收缓存失败: {response?.Message ?? "未知错误"}");
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存响应失败: {Message}", ex.Message);
                _log?.LogDebug($"处理缓存响应失败: {ex.Message}");
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
                if (data is CacheResponse cacheResponse)
                {
                    // 确保data是CacheResponse类型

                    if (cacheResponse.IsSuccess && !string.IsNullOrEmpty(cacheResponse.TableName))
                    {
                        // 处理缓存数据
                        ProcessCacheData(cacheResponse.TableName, cacheResponse.CacheData);

                        if (authorizeController != null && authorizeController.GetShowDebugInfoAuthorization())
                        {
                            //    MainForm.Instance.PrintInfoLog($"接收缓存: {response.TableName}, 数据量: {response.CacheData?.Count ?? 0}");
                        }

                        // 如果有更多数据，继续请求
                        if (cacheResponse.HasMoreData)
                        {
                            Task.Run(async () =>
                            {
                                await Task.Delay(100);
                                // 避免递归调用导致死循环
                                // 只在响应明确要求刷新时才重新请求
                                //if (response.ForceRefresh) {
                                //    // 使用不同的方式处理，或者添加延迟和限制条件
                                //    _log?.LogInformation("响应要求刷新缓存: {0}", response.TableName);
                                //    // 这里可以考虑使用一个队列或其他机制来处理，而不是直接递归调用
                                //}
                            });
                        }
                    }
                    else
                    {
                        _log.LogDebug($"接收缓存失败: {cacheResponse?.Message ?? "未知错误"}");
                    }
                }
                else
                {
                    _log.LogDebug("处理缓存响应失败: 数据格式不正确");
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
                        _log?.LogDebug("缓存预热成功");

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
                        _log?.LogDebug("缓存失效成功");
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
                        _log?.LogDebug($"缓存订阅成功: {response.TableName}");
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
                        _log?.LogDebug($"缓存取消订阅成功: {response.TableName}");
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
                            _log?.LogDebug("JArray缓存数据处理完成，表名={0}，数据量={1}", tableName, entityList.Count);
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
                _cacheManager.DeleteEntityList(tableName);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理缓存失败，表名={0}", tableName);
            }
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
            _log?.LogDebug("开始清理所有缓存");
            try
            {
                // 使用新的缓存管理器清空所有缓存
                if (_cacheManager != null)
                {
                    // 获取所有可缓存的表名
                    var cacheableTables = TableSchemaManager.Instance.CacheableTableNames;
                    foreach (var tableName in cacheableTables)
                    {
                        try
                        {
                            // 使用DeleteEntityList方法清理每个表的缓存
                            _cacheManager.DeleteEntityList(tableName);
                            _log?.LogDebug("已清理表 {TableName} 的缓存", tableName);
                        }
                        catch (Exception ex)
                        {
                            _log?.LogError(ex, "清理表 {TableName} 的缓存时发生错误", tableName);
                        }
                    }
                }

                // 清理请求时间记录
                lock (_lockObj)
                {
                    _lastRequestTimes.Clear();
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理所有缓存时发生错误");
            }
            finally
            {
                _log?.LogDebug("所有缓存清理完成");
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