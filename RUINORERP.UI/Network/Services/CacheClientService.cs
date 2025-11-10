using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Responses.Cache;
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
        private readonly ConcurrentDictionary<string, bool> _subscriptions;

        private readonly CacheRequestManager _cacheRequestManager; // 新增：使用专门的请求管理器
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
            _cacheResponseProcessor = cacheResponseProcessor;
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
        /// 注册命令处理程序
        /// </summary>
        private void RegisterCommandHandlers()
        {
            // 使用简化的缓存命令系统
            // 只订阅CacheSync命令，避免与RequestCacheAsync方法中的显式调用重复
            _commService.SubscribeCommand(CacheCommands.CacheSync, (packet, data) =>
            {
                if (data is CacheResponse response)
                {
                    _cacheResponseProcessor.ProcessCacheResponse(response);
                }
            });

            // 处理订阅命令 - 使用统一的处理方法，内部根据SubscribeAction区分
            _commService.SubscribeCommand(CacheCommands.CacheSubscription, (packet, data) =>
            {
                if (data is CacheResponse response)
                {
                    // 可以在这里添加额外的订阅响应处理逻辑
                    _log.LogDebug($"收到订阅响应: 表名={response.TableName}, 操作={response.Operation}");
                }
            });
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
                if (cacheResponse!=null)
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
        public async Task RequestCacheAsync(string tableName)
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
                // 请求缓存数据
                await _cacheRequestManager.RequestCacheAsync(tableName);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "请求缓存数据失败，表名={0}", tableName);
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