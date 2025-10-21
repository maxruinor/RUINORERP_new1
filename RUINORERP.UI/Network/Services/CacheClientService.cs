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

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 缓存客户端服务类，负责管理缓存订阅、同步和请求操作
    /// </summary>
    public class CacheClientService : CacheValidationBase, IDisposable
    {
        private readonly ILogger<CacheClientService> _log;
        private readonly IEntityCacheManager _cacheManager;
        private readonly UICacheSubscriptionManager _subscriptionManager;
        private readonly CacheRequestManager _cacheRequestManager; // 新增：使用专门的请求管理器

        // 私有成员变量
        private bool _disposed = false;
        private readonly string _componentName = nameof(CacheClientService);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="cacheRequestManager">缓存请求管理器</param>
        public CacheClientService(ILogger<CacheClientService> logger, UICacheSubscriptionManager subscriptionManager, IEntityCacheManager cacheManager, CacheRequestManager cacheRequestManager)
        {
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _cacheRequestManager = cacheRequestManager ?? throw new ArgumentNullException(nameof(cacheRequestManager));
            // 使用业务层的订阅管理器，避免重复实现
            _subscriptionManager = subscriptionManager;
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
                // 添加本地订阅
                _subscriptionManager.AddSubscription(tableName);
                
                // 发送订阅请求到服务器
                await _cacheRequestManager.SendCacheManageRequestAsync(tableName, "Subscribe", null);
            }
            catch (Exception ex)
            {                
                _log.LogError(ex, "订阅表{0}失败", tableName);
                // 订阅失败时移除本地订阅
                _subscriptionManager.RemoveSubscription(tableName);
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

            // 检查是否已订阅
            if (!_subscriptionManager.IsSubscribed(tableName))
            {
                _log.LogDebug("表{0}未订阅，跳过取消订阅", tableName);
                return;
            }

            try
            {                
                // 发送取消订阅请求到服务器
                await _cacheRequestManager.SendCacheManageRequestAsync(tableName, "Unsubscribe", null);
                
                // 成功后移除本地订阅
                _subscriptionManager.RemoveSubscription(tableName);
            }
            catch (Exception ex)
            {                
                _log.LogError(ex, "取消订阅表{0}失败", tableName);
            }
        }



        /// <summary>
        /// 取消所有缓存订阅
        /// </summary>
        public async Task UnsubscribeAllAsync()
        {
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法取消所有订阅", _componentName);
                return;
            }

            try
            {
                // 获取当前已订阅表的快照
                var subscribedTables = GetSubscribedTables().ToList();
                var failedTables = new List<string>();

                // 并行取消订阅以提高效率
                var tasks = subscribedTables.Select(async tableName =>
                {
                    try
                    {
                        await UnsubscribeCacheAsync(tableName);
                    }
                    catch (Exception ex)
                    {
                        _log.LogWarning(ex, "取消订阅表{0}失败", tableName);
                        failedTables.Add(tableName);
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "取消所有缓存订阅过程中发生错误");
            }
        }

        /// <summary>
        /// 检查表是否已订阅
        /// </summary>
        public bool IsSubscribed(string tableName)
        {            
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法检查订阅状态", _componentName);
                return false;
            }

            return _subscriptionManager.IsSubscribed(tableName);
        }

        /// <summary>
        /// 获取所有已订阅的表
        /// </summary>
        public IEnumerable<string> GetSubscribedTables()
        {            
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法获取订阅表列表", _componentName);
                return Enumerable.Empty<string>();
            }

            // 使用业务层订阅管理器获取订阅列表
            return _subscriptionManager.GetSubscriptions();
        }

        /// <summary>
        /// 从缓存获取名称
        /// </summary>
        public async Task<string> GetNameFromCacheAsync(string tableName, long id)
        {            
            // 检查是否已释放
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法从缓存获取名称", _componentName);
                return string.Empty;
            }

            try
            {                
                // 直接使用IEntityCacheManager的GetDisplayValue方法
                var displayValue = _cacheManager.GetDisplayValue(tableName, id);
                string name = displayValue?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(name))
                {                    
                    _log.LogDebug("从缓存获取名称成功，表名={0}，ID={1}，名称={2}",
                        tableName, id, name);
                }
                else
                {                    
                    _log.LogWarning("在表{0}的缓存中未找到ID={1}的记录或显示字段配置", tableName, id);
                }
                return name;
            }
            catch (Exception ex)
            {                
                _log.LogError(ex, "从缓存获取名称失败，表名={0}，ID={1}", tableName, id);
                return string.Empty;
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
                // 创建缓存更新请求并处理
                await _cacheRequestManager.ProcessCacheRequestAsync(new CacheRequest
                {
                    Operation = CacheOperation.Set,
                    TableName = tableName,
                    Data = entity,
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
                await _cacheRequestManager.ProcessCacheRequestAsync(new CacheRequest
                {
                    Operation = CacheOperation.Remove,
                    TableName = tableName,
                    Data = entityId,
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                // 可以在这里添加额外的资源释放逻辑
            }
        }
    }
}