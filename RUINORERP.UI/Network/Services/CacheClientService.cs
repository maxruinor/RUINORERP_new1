using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using RUINORERP.UI.Network.Services.Cache;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using System.Linq;
using RUINORERP.UI.Network;
using RUINORERP.Business.CommService;
using System.Threading;
using Netron.GraphLib;
using RUINORERP.PacketSpec.Serialization;
using NPOI.SS.Formula.Functions;
using Netron.NetronLight;
using FastReport.Table;
using RUINORERP.PacketSpec.Commands;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Models.Cache;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 缓存客户端服务类，负责管理缓存订阅、同步和请求操作
    /// 处理本地缓存变更事件并同步到服务器
    /// 
    /// </summary>
    public class CacheClientService : CacheValidationBase, IDisposable
    {
        private readonly ILogger<CacheClientService> _log;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ICacheSyncMetadata _cacheSyncMetadata; // 缓存同步元数据管理器
        // 客户端使用：管理订阅的表，每个表是否有订阅
        private ConcurrentDictionary<string, bool> _subscriptions = new ConcurrentDictionary<string, bool>();

        private readonly CacheRequestManager _cacheRequestManager; // 缓存请求管理器
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; // 事件驱动缓存管理器
        private readonly ClientCommunicationService _commService; // 通信服务
        private readonly CacheResponseProcessor _cacheResponseProcessor;
        private readonly ITableSchemaManager _tableSchemaManager;
        // 私有成员变量
        private bool _disposed = false;
        private readonly string _componentName = nameof(CacheClientService);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="subscriptionManager">订阅管理器</param>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="cacheRequestManager">缓存请求管理器</param>
        /// <param name="eventDrivenCacheManager">事件驱动缓存管理器</param>
        /// <param name="commService">通信服务</param>
        public CacheClientService(ILogger<CacheClientService> logger,
            IEntityCacheManager cacheManager,
            ICacheSyncMetadata cacheSyncMetadata,
            CacheRequestManager cacheRequestManager,
            EventDrivenCacheManager eventDrivenCacheManager,
            ClientCommunicationService commService,
            CacheResponseProcessor cacheResponseProcessor
            )
        {
            _cacheResponseProcessor = cacheResponseProcessor ?? throw new ArgumentNullException(nameof(cacheResponseProcessor));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _cacheSyncMetadata = cacheSyncMetadata ?? throw new ArgumentNullException(nameof(cacheSyncMetadata));
            _cacheRequestManager = cacheRequestManager ?? throw new ArgumentNullException(nameof(cacheRequestManager));
            _eventDrivenCacheManager = eventDrivenCacheManager ?? throw new ArgumentNullException(nameof(eventDrivenCacheManager));
            _commService = commService ?? throw new ArgumentNullException(nameof(commService));
            _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();
            // 订阅的缓存变更事件
            _eventDrivenCacheManager.CacheChanged += OnClientCacheChanged;
            // 订阅的连接状态变化事件
            if (_commService is ClientCommunicationService clientCommService)
            {
                clientCommService.ConnectionStateChanged += OnConnectionStateChanged;
            }

            // 注册缓存响应处理
            RegisterCommandHandlers();
        }

        /// <summary>
        /// 处理连接状态变化事件
        /// </summary>
        /// <param name="isConnected">是否已连接</param>
        private void OnConnectionStateChanged(bool isConnected)
        {
            // 不再需要手动管理缓存队列，现在由ClientCommunicationService自动处理
            _log.Debug("连接状态变化: {0}", isConnected ? "已连接" : "已断开");
        }


        /// <summary>
        /// 注册命令处理程序 - 【指令分发中心】
        /// </summary>
        private void RegisterCommandHandlers()
        {
            RegisterCacheSyncHandler();      // 处理服务器推送 (CacheSync)
            RegisterCacheSubscriptionHandler(); // 处理订阅响应 (CacheSubscription)
        }

        /// <summary>
        /// 注册缓存同步命令处理器 - 【接收服务器推送】
        /// 职责：处理 Server -> Client 的 CacheSync 消息。
        /// 注意：根据“谁先发送谁请求”原则，服务器推送时使用 CacheRequest 作为载体。
        /// </summary>
        private void RegisterCacheSyncHandler()
        {
            _commService.SubscribeCommand(CacheCommands.CacheSync, (packet, data) =>
            {
                try
                {
                    // 【优先级 1】处理 CacheRequest 格式（服务器标准推送）
                    if (data is CacheRequest cacheRequest)
                    {
                        ProcessServerPushedRequest(cacheRequest, packet.CommandId);
                        return;
                    }

                    // 【优先级 2】兼容旧版或特殊场景下的 CacheResponse 推送
                    if (packet.Response is CacheResponse cacheResponse)
                    {
                        _log.LogDebug("接收到 CacheResponse 格式的推送（兼容模式），表名={0}", cacheResponse.TableName);
                        ProcessServerPushedResponse(cacheResponse, packet.CommandId);
                        return;
                    }

                    _log.LogWarning("缓存同步数据格式无效，期望 CacheRequest，实际类型={0}",
                        data?.GetType().Name ?? "null");
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "处理缓存同步命令失败，命令ID={0}", packet.CommandId);
                }
            });
        }

        /// <summary>
        /// 处理服务器推送的缓存请求（标准推送）
        /// </summary>
        private void ProcessServerPushedRequest(CacheRequest request, CommandId commandId)
        {
            if (request == null || string.IsNullOrEmpty(request.TableName)) return;

            // 将 CacheRequest 转换为 CacheResponse 后统一处理
            var response = new CacheResponse
            {
                TableName = request.TableName,
                Operation = request.Operation,
                CacheData = request.CacheData,
                IsSuccess = true,
                Message = "服务器推送",
                Timestamp = DateTime.UtcNow
            };

            // 使用统一的方法处理
            ProcessServerPushInternal(response);
        }

        /// <summary>
        /// 处理服务器推送的缓存响应 (兼容模式)
        /// </summary>
        private void ProcessServerPushedResponse(CacheResponse response, CommandId commandId)
        {
            if (response == null || string.IsNullOrEmpty(response.TableName)) return;

            // 使用统一的方法处理
            ProcessServerPushInternal(response);
        }

        /// <summary>
        /// 统一处理服务器推送的缓存数据
        /// </summary>
        /// <param name="response">缓存响应数据</param>
        private void ProcessServerPushInternal(CacheResponse response)
        {
            // 【优化】：版本戳冲突检测 - 避免用旧数据覆盖新数据
            var localSyncInfo = _cacheSyncMetadata.GetTableSyncInfo(response.TableName);
            if (localSyncInfo != null && response.CacheData != null)
            {
                // 如果本地版本比推送的版本还新，则忽略此次推送
                if (localSyncInfo.VersionStamp > response.CacheData.VersionStamp)
                {
                    _log.LogDebug("跳过过期的服务器推送: 表={0}, 本地版本={1}, 推送版本={2}",
                        response.TableName, localSyncInfo.VersionStamp, response.CacheData.VersionStamp);
                    return;
                }
            }

            // 核心处理逻辑委托给 Processor
            _cacheResponseProcessor.ProcessCacheResponse(response);
            
            // 同步更新本地元数据中的版本戳
            if (response.CacheData != null && response.CacheData.VersionStamp > 0)
            {
                _cacheSyncMetadata.UpdateTableSyncInfo(response.TableName, 0, 0); // 触发时间更新
            }
            
            _log.LogDebug("成功处理服务器推送的缓存数据，表名={0}, 操作={1}", response.TableName, response.Operation);
        }

        /// <summary>
        /// 注册缓存订阅命令处理器 - 【处理订阅响应】
        /// </summary>
        private void RegisterCacheSubscriptionHandler()
        {
            _commService.SubscribeCommand(CacheCommands.CacheSubscription, (packet, data) =>
            {
                try
                {
                    if (data is CacheResponse response)
                    {
                        UpdateSubscriptionStatus(response);
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "处理缓存订阅响应失败");
                }
            });
        }



        /// <summary>
        /// 更新订阅状态
        /// </summary>
        private void UpdateSubscriptionStatus(CacheResponse response)
        {
            try
            {
                if (response.IsSuccess)
                {
                    if (response.Operation == CacheOperation.Set)
                    {
                        _subscriptions.TryAdd(response.TableName, true);
                        _log.LogDebug("更新订阅状态成功，表名={0}，状态=已订阅", response.TableName);
                    }
                    else if (response.Operation == CacheOperation.Remove)
                    {
                        _subscriptions.TryRemove(response.TableName, out _);
                        _log.LogDebug("更新订阅状态成功，表名={0}，状态=未订阅", response.TableName);
                    }
                }
                else
                {
                    _log.LogWarning("订阅操作失败，表名={0}, 操作={1}, 消息={2}",
                        response.TableName, response.Operation, response.Message ?? "无错误信息");
                }
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "更新订阅状态失败，表名={0}", response.TableName);
            }
        }


        /// <summary>
        /// 清除同步监控统计 - 简化版本，使用现有统计功能
        /// </summary>
        public void ClearSyncMonitorStatistics()
        {
            try
            {
                // 使用EntityCacheManager的统计重置功能
                _cacheManager.ResetStatistics();
                _log.LogInformation("缓存同步监控统计已清除");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "清除同步监控统计失败");
            }
        }



        /// <summary>
        /// 订阅缓存变更
        /// </summary>
        public async Task SubscribeCacheAsync(string tableName)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法订阅缓存变更", _componentName);
                return;
            }

            try
            {
                // 发送订阅请求到服务器
                var cacheResponse = await _cacheRequestManager.SendCacheSubscriptionAsync(tableName, SubscribeAction.Subscribe, null);
                if (cacheResponse != null)
                {
                    if (cacheResponse.IsSuccess)
                    {
                        bool add = _subscriptions.TryAdd(tableName, true);
                        //如果已经存在呢？更新？
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "订阅表{0}失败", tableName);
            }
        }

        /// <summary>
        /// 取消缓存订阅
        /// </summary>
        public async Task UnsubscribeCacheAsync(string tableName)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法取消订阅缓存变更", _componentName);
                return;
            }

            try
            {
                // 发送取消订阅请求到服务器
                var cacheResponse = await _cacheRequestManager.SendCacheSubscriptionAsync(tableName, SubscribeAction.Unsubscribe, null);
                if (cacheResponse.IsSuccess)
                {
                    bool add = _subscriptions.TryRemove(tableName, out _);
                    //如果已经存在呢？更新？
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "取消订阅表{0}失败", tableName);
            }
        }




        /// <summary>
        /// 按表类型订阅数据表
        /// </summary>
        /// <param name="tableType">表类型</param>
        public async Task SubscribeTablesByTypeAsync(TableType tableType)
        {
            try
            {
                // 获取指定类型且可缓存的表
                var tables = _tableSchemaManager.GetCacheableTableNamesByType(tableType);
                var failedTables = new List<string>();

                if (tables.Count == 0)
                {
                    _log.LogDebug("没有找到类型为{0}且可缓存的表", tableType);
                    return;
                }

                // 并行订阅以提高效率
                var tasks = tables.Select(async tableName =>
                {
                    try
                    {
                        await SubscribeCacheAsync(tableName);
                        _log.LogDebug("已订阅表: {0}, 类型: {1}", tableName, tableType);
                    }
                    catch (Exception ex)
                    {
                        _log.LogWarning(ex, "订阅表{0}失败，类型: {1}", tableName, tableType);
                        failedTables.Add(tableName);
                    }
                });

                // 过滤掉可能的null任务，防止ArgumentException异常
                var validTasks = tasks.Where(t => t != null).ToList();
                if (validTasks.Any())
                {
                    await Task.WhenAll(validTasks);
                }

                _log.LogDebug("类型{0}表订阅完成: 成功={1}, 失败={2}",
                    tableType, tables.Count - failedTables.Count, failedTables.Count);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "按类型{0}订阅表时发生错误", tableType);
            }
        }

        /// <summary>
        /// 订阅所有基础业务表
        /// </summary>
        public async Task SubscribeAllBaseTablesAsync()
        {
            try
            {
                // 使用优化后的订阅方法
                await SubscribeTablesByTypeAsync(TableType.Base);
                await SubscribeTablesByTypeAsync(TableType.Business);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "全量订阅基础业务表时发生错误");
                // 添加重试机制
                await RetrySubscribeAllBaseTablesAsync();
            }
        }

        /// <summary>
        /// 重试订阅所有基础业务表
        /// </summary>
        private async Task RetrySubscribeAllBaseTablesAsync(int maxRetries = 3)
        {
            for (int i = 1; i <= maxRetries; i++)
            {
                try
                {
                    _log.LogDebug($"重试订阅基础业务表，第{i}次尝试");
                    await SubscribeTablesByTypeAsync(TableType.Base);
                    await SubscribeTablesByTypeAsync(TableType.Business);
                    _log.LogDebug("重试订阅基础业务表成功");
                    break;
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, $"第{i}次重试订阅基础业务表失败");
                    if (i == maxRetries)
                    {
                        _log.LogError("达到最大重试次数，订阅基础业务表失败");
                        throw;
                    }
                    // 等待一段时间后重试
                    await Task.Delay(1000 * i);
                }
            }
        }


        /// <summary>
        /// 清理指定表的缓存
        /// </summary>
        public void ClearCache(string tableName)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法清理缓存", _componentName);
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    _log.LogWarning("清理缓存时表名为空");
                    return;
                }

                _cacheManager.DeleteEntityList(tableName);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "清理缓存失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法清理所有缓存", _componentName);
                return;
            }

            try
            {
                // 获取所有可缓存的表名
               
                var cacheableTables = _tableSchemaManager.GetAllTableNames();

                foreach (var tableName in cacheableTables)
                {
                    try
                    {
                        _cacheManager.DeleteEntityList(tableName);
                        _log.LogDebug("已清理表 {0} 的缓存", tableName);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, "清理表 {0} 的缓存时发生错误", tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "清理所有缓存时发生错误");
            }
        }

        /// <summary>
        /// 向服务器请求指定表的缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <exception cref="OperationCanceledException">当操作被取消时抛出</exception>
        public async Task RequestCacheAsync(string tableName, CancellationToken cancellationToken = default)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法请求缓存数据", _componentName);
                return;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                _log.LogWarning("请求缓存时表名为空");
                return;
            }

            try
            {
                // 检查取消令牌
                cancellationToken.ThrowIfCancellationRequested();

                // 请求缓存数据
                await _cacheRequestManager.RequestCacheAsync(tableName, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _log.LogDebug("缓存请求被用户取消，表名={0}", tableName);
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "请求缓存数据失败，表名={0}", tableName);
                throw;
            }
        }



        /// <summary>
        /// 用户登录成功时向服务器请求所有缓存数据的元数据信息，就是一个总表，保存了各个表的最后更新时间戳，数据行数等信息
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RequestAllCacheSyncMetadataAsync(CancellationToken cancellationToken = default)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法请求缓存数据", _componentName);
                return;
            }

            try
            {
                // 检查取消令牌
                cancellationToken.ThrowIfCancellationRequested();

                // 请求缓存数据
                await _cacheRequestManager.RequestAllCacheSyncMetadataAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                _log.LogDebug("缓存请求被用户取消，ex={0}", oex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "请求缓存数据失败，ex={0}", ex.Message);
            }
        }



        /// <summary>
        /// 同步缓存更新到服务器
        /// </summary>
        public async Task SyncCacheUpdateAsync(string tableName, object entity)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法同步缓存更新", _componentName);
                return;
            }

            if (string.IsNullOrEmpty(tableName) || entity == null)
            {
                _log.LogWarning("同步缓存更新时表名或实体为空, TableName={0}, EntityIsNull={1}", 
                    tableName, entity == null);
                return;
            }

            _log.LogDebug("准备同步缓存更新，表名={0}, 实体类型={1}", tableName, entity.GetType().Name);

            try
            {
                // 获取当前表的版本戳
                var syncInfo = _cacheSyncMetadata.GetTableSyncInfo(tableName);
                long versionStamp = syncInfo?.VersionStamp ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                
                CacheData cacheData = CacheData.Create(tableName, entity, versionStamp: versionStamp);
                //cacheData.EntityType = TableSchemaManager.Instance.GetSchemaInfo(tableName).EntityType;
                cacheData.EntityTypeName = entity.GetType().AssemblyQualifiedName;
                cacheData.EntityByte = JsonCompressionSerializationService.Serialize(entity);
                
                // ✅ 验证 CacheData 是否正确创建
                if (cacheData == null || cacheData.EntityByte == null || cacheData.EntityByte.Length == 0)
                {
                    _log.LogError("CacheData 创建失败或数据为空, TableName={0}, EntityTypeName={1}", 
                        tableName, entity.GetType().FullName);
                    return;
                }
                
                _log.LogDebug("CacheData 创建成功, TableName={0}, DataSize={1} bytes", 
                    tableName, cacheData.EntityByte.Length);

                // 创建缓存更新请求并处理
                var cacheRequest = new CacheRequest
                {
                    Operation = CacheOperation.Set,
                    TableName = tableName,
                    CacheData = cacheData,
                    Timestamp = DateTime.UtcNow
                };
                
                _log.LogDebug("发送缓存同步请求, RequestId={0}, TableName={1}, Operation={2}",
                    cacheRequest.RequestId, tableName, cacheRequest.Operation);
                
                await _cacheRequestManager.ProcessCacheOperationAsync(CacheCommands.CacheSync, cacheRequest);
                
                _log.LogDebug("缓存同步请求发送成功, TableName={0}", tableName);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "同步缓存更新失败，表名={0}, 实体类型={1}", 
                    tableName, entity?.GetType().FullName ?? "null");
            }
        }

        /// <summary>
        /// 同步缓存删除到服务器
        /// </summary>
        public async Task SyncCacheDeleteAsync(string tableName, long entityId)
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法同步缓存删除", _componentName);
                return;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                _log.LogWarning("同步缓存删除时表名为空");
                return;
            }

            _log.LogDebug("准备同步缓存删除，表名={0}，ID={1}", tableName, entityId);

            try
            {
                // 创建缓存删除请求并处理
                await _cacheRequestManager.ProcessCacheOperationAsync(CacheCommands.CacheSync, new CacheRequest
                {
                    Operation = CacheOperation.Remove,
                    TableName = tableName,
                    PrimaryKeyName = _tableSchemaManager.GetSchemaInfo(tableName).PrimaryKeyField,
                    PrimaryKeyValue = entityId,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "同步缓存删除失败，表名={0}，ID={1}", tableName, entityId);
            }
        }

        /// <summary>
        /// 验证请求参数（使用基类方法）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="operation">操作类型</param>
        /// <returns>是否有效</returns>
        private bool ValidateRequest(string tableName, CacheOperation operation)
        {
            // 验证表名
            var tableValidation = base.ValidateTableName(tableName);
            if (!tableValidation.IsValid)
            {
                _log.LogError($"ValidateRequest: {tableValidation.GetValidationErrors()}");
                return false;
            }

            // 验证操作类型
            if (!Enum.IsDefined(typeof(CacheOperation), operation))
            {
                _log.LogError("ValidateRequest: 无效的操作类型");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 处理客户端缓存变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnClientCacheChanged(object sender, CacheChangedEventArgs e)
        {
            // 使用 _ = 显式丢弃任务，避免 async void
            _ = HandleCacheChangedAsync(e);
        }

        /// <summary>
        /// 异步处理缓存变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        private async Task HandleCacheChangedAsync(CacheChangedEventArgs e)
        {
            try
            {
                // 检查是否已释放
                if (_disposed)
                {
                    _log.LogWarning("{0}已释放，无法处理缓存变更事件", _componentName);
                    return;
                }

                // 只处理需要同步到服务器的变更
                if (!e.SyncToServer)
                    return;

                //在基础列表中的才要同步到服务器
                if (!_tableSchemaManager.CacheableTableNames.Contains(e.Key))
                    return;

                // ✅ 关键修复: Set 操作必须有 Value
                if (e.Operation == CacheOperation.Set && e.Value == null)
                {
                    _log.LogWarning("缓存同步事件数据异常: Operation=Set, 但 Value 为 null, TableName={0}", e.Key);
                    return; // ❌ 数据无效,不同步
                }

                // 创建缓存更新请求 - 使用简化的操作类型
                var request = new CacheRequest
                {
                    TableName = e.Key,
                    Operation = e.Operation, // 直接使用事件中的操作类型
                    Timestamp = DateTime.UtcNow
                };

                if (request.Operation == CacheOperation.Remove)
                {
                    request.PrimaryKeyValue = e.Value;
                }

                // 根据操作类型设置请求数据
                if (e.Value != null)
                {
                    // 获取当前表的版本戳
                    var syncInfo = _cacheSyncMetadata.GetTableSyncInfo(e.Key);
                    long versionStamp = syncInfo?.VersionStamp ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    
                    CacheData cacheData = CacheData.Create(e.Key, e.Value, versionStamp: versionStamp);
                    
                    // ✅ 验证 CacheData 是否正确创建
                    if (cacheData == null || cacheData.EntityByte == null || cacheData.EntityByte.Length == 0)
                    {
                        _log.LogError("CacheData 创建失败, TableName={0}, Operation={1}, ValueType={2}", 
                            e.Key, e.Operation, e.ValueType?.FullName ?? "null");
                        return; // ❌ 数据无效,不同步
                    }
                    
                    request.CacheData = cacheData;
                    _log.LogDebug("缓存变更事件处理成功, TableName={0}, Operation={1}, DataSize={2} bytes",
                        e.Key, e.Operation, cacheData.EntityByte.Length);
                }
                else if (e.Operation == CacheOperation.Set)
                {
                    // ✅ 额外保护: Set 操作不应该走到这里
                    _log.LogError("逻辑错误: Set 操作的 Value 为 null, TableName={0}", e.Key);
                    return; // ❌ 数据无效,不同步
                }

                // 发送命令到服务器
                await _commService.SendOneWayCommandAsync<CacheRequest>(CacheCommands.CacheSync, request, CancellationToken.None);

                _log.LogDebug("客户端缓存变更已同步到服务器: {0}, 操作: {1}", e.Key, e.Operation.ToString());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "同步客户端缓存变更到服务器时发生异常: {0}", ex.Message);

                // 发生异常时也记录到日志，后续可以考虑添加重试或队列机制
            }
        }

        /// <summary>
        /// 批量同步缓存更新到服务器 - 简化版本，使用现有缓存管理器
        /// </summary>
        public async Task BatchSyncCacheUpdatesAsync(List<(string tableName, object entity)> updates)
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法批量同步缓存更新", _componentName);
                return;
            }

            if (updates == null || updates.Count == 0)
            {
                _log.LogWarning("批量同步缓存更新时更新列表为空");
                return;
            }

            _log.LogInformation("开始批量同步缓存更新，数量={0}", updates.Count);

            try
            {
                var successCount = 0;

                // 逐个处理更新操作
                foreach (var (tableName, entity) in updates)
                {
                    try
                    {
                        // 使用现有的缓存管理器更新实体
                        _cacheManager.UpdateEntity(tableName, entity);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        _log.LogWarning(ex, "批量更新中单个实体更新失败，表名={0}", tableName);
                    }
                }

                _log.LogInformation("批量同步缓存更新完成，总数={0}, 成功={1}", updates.Count, successCount);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "批量同步缓存更新失败，数量={0}", updates.Count);
            }
        }

        /// <summary>
        /// 批量同步缓存删除到服务器 - 简化版本，使用现有缓存管理器
        /// </summary>
        public async Task BatchSyncCacheDeletesAsync<T>(string tableName, List<object> entityIds)
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法批量同步缓存删除", _componentName);
                return;
            }

            if (string.IsNullOrEmpty(tableName) || entityIds == null || entityIds.Count == 0)
            {
                _log.LogWarning("批量同步缓存删除时参数无效");
                return;
            }

            try
            {

                // 使用现有的缓存管理器删除实体
                _cacheManager.DeleteEntities(tableName, entityIds.ToArray());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "批量同步缓存删除失败，表名={0}, 数量={1}", tableName, entityIds.Count);
            }
        }




        /// <summary>
        /// 批量清空缓存 - 简化版本，使用现有缓存管理器
        /// </summary>
        public async Task BatchClearCacheAsync(List<string> tableNames)
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法批量清空缓存", _componentName);
                return;
            }

            if (tableNames == null || tableNames.Count == 0)
            {
                _log.LogWarning("批量清空缓存时表名列表为空");
                return;
            }

            _log.LogInformation("开始批量清空缓存，表数量={0}", tableNames.Count);

            try
            {
                var successCount = 0;

                // 逐个处理清空操作
                foreach (var tableName in tableNames)
                {
                    try
                    {
                        // 使用现有的缓存管理器清空表缓存
                        _cacheManager.DeleteEntities(tableName, null);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        _log.LogWarning(ex, "批量清空中单个表清空失败，表名={0}", tableName);
                    }
                }

                _log.LogInformation("批量清空缓存完成，表数量={0}, 成功={1}", tableNames.Count, successCount);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "批量清空缓存失败，表数量={0}", tableNames.Count);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                // 取消订阅缓存变更事件
                if (_eventDrivenCacheManager != null)
                {
                    _eventDrivenCacheManager.CacheChanged -= OnClientCacheChanged;
                }
                // 取消订阅连接状态变化事件
                if (_commService is ClientCommunicationService clientCommService)
                {
                    clientCommService.ConnectionStateChanged -= OnConnectionStateChanged;
                }
            }
        }
    }
}