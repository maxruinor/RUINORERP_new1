using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.PacketSpec.Commands.Cache;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 事件驱动缓存管理器，用于处理本地缓存变更事件
    /// 可以被服务器和客户端共同使用
    /// </summary>
    public class EventDrivenCacheManager
    {
        // 定义缓存变更事件
        public event EventHandler<CacheChangedEventArgs> CacheChanged;

        private readonly IEntityCacheManager _cacheManager;
        private readonly TableSchemaManager _tableSchemaManager;
        private readonly ILogger<EventDrivenCacheManager> _logger;

        /// <summary>
        /// 构造函数 - 通过依赖注入获取依赖项
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        public EventDrivenCacheManager(IEntityCacheManager cacheManager, ILogger<EventDrivenCacheManager> logger = null)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _tableSchemaManager = TableSchemaManager.Instance;
            _logger = logger;
        }

        /// <summary>
        /// 触发缓存变更事件
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="operation">操作类型</param>
        /// <param name="value">变更的值</param>
        /// <param name="syncToServer">是否同步到服务器</param>
        protected virtual void OnCacheChanged(string tableName, CacheOperation operation, object value, bool syncToServer = true)
        {
            CacheChanged?.Invoke(this, new CacheChangedEventArgs
            {
                Key = tableName,
                Operation = operation,
                ValueType = value?.GetType(),
                Value = value,
                SyncToServer = syncToServer
            });
        }

        /// <summary>
        /// 更新实体列表缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">实体列表</param>
        public void UpdateEntityList<T>(List<T> list) where T : class
        {
            var tableName = typeof(T).Name;

            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                return;
            }

            // 更新缓存并触发事件
            _cacheManager.UpdateEntityList(list);
            OnCacheChanged(tableName, CacheOperation.Set, list);
        }

        /// <summary>
        /// 更新单个实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void UpdateEntity<T>(T entity) where T : class
        {
            var tableName = typeof(T).Name;

            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                return;
            }

            // 更新缓存并触发事件
            _cacheManager.UpdateEntity(entity);
            OnCacheChanged(tableName, CacheOperation.Set, entity);
        }

        /// <summary>
        /// 删除实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValue">主键值</param>
        public void DeleteEntity<T>(object idValue) where T : class
        {
            var tableName = typeof(T).Name;

            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                return;
            }

            // 删除缓存并触发事件
            _cacheManager.DeleteEntity<T>(idValue);
            OnCacheChanged(tableName, CacheOperation.Remove, idValue);
        }

        /// <summary>
        /// 删除多个实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        public void DeleteEntityList<T>(List<T> entities) where T : class
        {
            var tableName = typeof(T).Name;

            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                return;
            }

            // 删除缓存并触发事件
            _cacheManager.DeleteEntityList(entities);
            OnCacheChanged(tableName, CacheOperation.Remove, entities);
        }

        /// <summary>
        /// 删除指定表的整个实体列表缓存并触发事件
        /// </summary>
        /// <param name="tableName">表名</param>
        public void DeleteEntityList(string tableName)
        {
            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                return;
            }

            // 删除缓存并触发事件
            _cacheManager.DeleteEntityList(tableName);
            OnCacheChanged(tableName, CacheOperation.Remove, null);
        }

        /// <summary>
        /// 批量删除指定主键数组的实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        public void DeleteEntities<T>(object[] idValues) where T : class
        {
            ProcessDeleteEntities(() =>
            {
                var tableName = typeof(T).Name;
                _cacheManager.DeleteEntities<T>(idValues);
                OnCacheChanged(tableName, CacheOperation.Remove, idValues);
                return typeof(T).Name;
            });
        }

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        public void DeleteEntities(string tableName, params object[] primaryKeyValues)
        {
            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                return;
            }

            ProcessDeleteEntities(() =>
            {
                _cacheManager.DeleteEntities(tableName, primaryKeyValues);
                OnCacheChanged(tableName, CacheOperation.Remove, primaryKeyValues);
                return tableName;
            });
        }

        /// <summary>
        /// 处理批量删除实体缓存的通用方法，包含异常处理
        /// </summary>
        /// <param name="deleteAction">删除操作委托，返回表名用于日志记录</param>
        private void ProcessDeleteEntities(Func<string> deleteAction)
        {
            try
            {
                string tableName = deleteAction();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"批量删除表缓存并触发事件时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 检查表是否需要缓存（智能过滤核心方法）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否需要缓存</returns>
        private bool IsTableCacheable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;

            // 获取表结构信息并检查是否可缓存
            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
            return schemaInfo != null && schemaInfo.IsCacheable;
        }
    }
}