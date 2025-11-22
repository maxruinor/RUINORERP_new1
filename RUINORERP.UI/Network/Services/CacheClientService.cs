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
    /// </summary>
    public class CacheClientService : CacheValidationBase, IDisposable
    {
        private readonly ILogger<CacheClientService> _log;
        private readonly IEntityCacheManager _cacheManager;
        // 客户端使用：管理订阅的表，每个表是否有订阅
        private ConcurrentDictionary<string, bool> _subscriptions = new ConcurrentDictionary<string, bool>();

        private readonly CacheRequestManager _cacheRequestManager; // 缓存请求管理器
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; // 事件驱动缓存管理器
        private readonly ClientCommunicationService _commService; // 通信服务
        private readonly CacheResponseProcessor _cacheResponseProcessor;
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
            CacheRequestManager cacheRequestManager,
            EventDrivenCacheManager eventDrivenCacheManager,
            ClientCommunicationService commService,
            CacheResponseProcessor cacheResponseProcessor
            )
        {
            _cacheResponseProcessor = cacheResponseProcessor ?? throw new ArgumentNullException(nameof(cacheResponseProcessor));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _cacheRequestManager = cacheRequestManager ?? throw new ArgumentNullException(nameof(cacheRequestManager));
            _eventDrivenCacheManager = eventDrivenCacheManager ?? throw new ArgumentNullException(nameof(eventDrivenCacheManager));
            _commService = commService ?? throw new ArgumentNullException(nameof(commService));

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
        /// 注册命令处理程序 - 增强版，添加详细日志和错误处理
        /// 服务器推送的缓存用的这种方式。也可以写一个 LockCommandHandler 类似的处理类。只是这里事件优先
        /// </summary>
        private void RegisterCommandHandlers()
        {
            // 使用简化的缓存命令系统
            // 只订阅CacheSync命令，避免与RequestCacheAsync方法中的显式调用重复
            // 注册缓存同步命令处理器 - 处理服务器推送的缓存更新
            RegisterCacheSyncHandler();

            // 注册缓存订阅命令处理器 - 处理订阅响应
            RegisterCacheSubscriptionHandler();

            // 注册缓存批量同步命令处理器 - 处理批量缓存更新
            RegisterCacheBatchSyncHandler();
        }

        /// <summary>
        /// 注册缓存同步命令处理器
        /// 处理服务器推送的缓存数据
        /// </summary>
        private void RegisterCacheSyncHandler()
        {
            _commService.SubscribeCommand(CacheCommands.CacheSync, (packet, data) =>
            {
                try
                {

                    if (packet.Request is CacheRequest cacheRequest)
                    {
                        // 处理缓存响应
                        _cacheResponseProcessor.ProcessCacheRequest(cacheRequest);
                    }
                    else
                    {
                        _log.LogWarning("缓存同步数据格式无效，期望CacheResponse，实际类型={0}", data?.GetType().Name ?? "null");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "处理缓存同步命令失败，命令ID={0}", packet.CommandId);
                    // 不抛出异常，避免影响其他命令处理
                }
            });
        }

        /// <summary>
        /// 注册缓存订阅命令处理器
        /// </summary>
        private void RegisterCacheSubscriptionHandler()
        {
            _commService.SubscribeCommand(CacheCommands.CacheSubscription, (packet, data) =>
            {
                try
                {
                    _log.LogDebug("收到缓存订阅响应命令，命令ID={0}", packet.CommandId);

                    if (data is CacheResponse response)
                    {
                        _log.LogInformation("缓存订阅响应处理成功，表名={0}, 操作={1}, 成功状态={2}",
                            response.TableName, response.Operation, response.IsSuccess);

                        // 更新订阅状态
                        UpdateSubscriptionStatus(response);

                        // 如果是订阅成功，可以触发缓存初始化
                        if (response.IsSuccess && response.Operation == CacheOperation.Set)
                        {
                            _log.LogDebug("订阅成功，可以触发缓存初始化，表名={0}", response.TableName);
                            // 可以在这里添加缓存初始化逻辑
                        }
                    }
                    else
                    {
                        _log.LogWarning("缓存订阅响应数据格式无效，期望CacheResponse，实际类型={0}", data?.GetType().Name ?? "null");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "处理缓存订阅响应命令失败，命令ID={0}", packet.CommandId);
                }
            });
        }

        /// <summary>
        /// 注册缓存批量同步命令处理器
        /// </summary>
        private void RegisterCacheBatchSyncHandler()
        {
            _commService.SubscribeCommand(CacheCommands.CacheSync, async (packet, data) =>
            {
                try
                {

                    if (data is List<CacheResponse> responses)
                    {
                        _log.LogDebug("批量缓存同步数据包解析成功，响应数量={0}", responses.Count);

                        // 直接处理每个缓存响应
                        foreach (var response in responses)
                        {
                            _cacheResponseProcessor.ProcessCacheResponse(response);
                        }

                        _log.LogInformation("批量缓存同步处理成功，响应数量={0}", responses.Count);
                    }
                    else
                    {
                        _log.LogWarning("批量缓存同步数据格式无效，期望List<CacheResponse>，实际类型={0}", data?.GetType().Name ?? "null");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "处理批量缓存同步命令失败，命令ID={0}", packet.CommandId);
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
                var tables = TableSchemaManager.Instance.GetCacheableTableNamesByType(tableType);
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
                var cacheableTables = TableSchemaManager.Instance.GetAllTableNames();

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
                _log.LogWarning("同步缓存更新时表名或实体为空");
                return;
            }

            _log.LogDebug("准备同步缓存更新，表名={0}", tableName);

            try
            {
                CacheData cacheData = CacheData.Create(tableName, entity);
                //cacheData.EntityType = TableSchemaManager.Instance.GetSchemaInfo(tableName).EntityType;
                cacheData.EntityTypeName = entity.GetType().AssemblyQualifiedName;
                cacheData.EntityByte = JsonCompressionSerializationService.Serialize(entity);

                // 创建缓存更新请求并处理
                await _cacheRequestManager.ProcessCacheOperationAsync(CacheCommands.CacheSync, new CacheRequest
                {
                    Operation = CacheOperation.Set,
                    TableName = tableName,
                    CacheData = cacheData,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "同步缓存更新失败，表名={0}", tableName);
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
                    PrimaryKeyName = TableSchemaManager.Instance.GetSchemaInfo(tableName).PrimaryKeyField,
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
        private async void OnClientCacheChanged(object sender, CacheChangedEventArgs e)
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
                if (!TableSchemaManager.Instance.CacheableTableNames.Contains(e.Key))
                    return;


                // 创建缓存更新请求 - 使用简化的操作类型
                var request = new CacheRequest
                {
                    TableName = e.Key,
                    Operation = e.Operation, // 直接使用事件中的操作类型
                    Timestamp = DateTime.UtcNow
                };



                // 根据操作类型设置请求数据
                if (e.Value != null)
                {
                    CacheData cacheData = CacheData.Create(e.Key, e.Value);
                    request.CacheData = cacheData;
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