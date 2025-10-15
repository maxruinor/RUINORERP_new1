using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models;
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
        /// 获取实体的主键值
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>主键值</returns>
        private object GetEntityPrimaryKeyValue(object entity, string tableName)
        {
            if (entity == null)
                return null;

            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
            if (schemaInfo == null)
                return null;

            var property = entity.GetType().GetProperty(schemaInfo.PrimaryKeyField);
            return property?.GetValue(entity);
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
            
            // 先更新缓存
            _cacheManager.UpdateEntityList(list);
            
            // 触发缓存变更事件，同步到服务器和其他用户
            OnCacheChanged(tableName, CacheOperation.BatchUpdate, list);
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
            
            // 先更新缓存
            _cacheManager.UpdateEntity(entity);
            
            // 触发缓存变更事件，同步到服务器和其他用户
            OnCacheChanged(tableName, CacheOperation.Update, entity);
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
            
            // 先删除缓存
            _cacheManager.DeleteEntity<T>(idValue);
            
            // 触发缓存变更事件，同步到服务器和其他用户
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
            
            // 先删除缓存
            _cacheManager.DeleteEntityList(entities);
            
            // 触发缓存变更事件，同步到服务器和其他用户
            OnCacheChanged(tableName, CacheOperation.BatchRemove, entities);
        }

        /// <summary>
        /// 批量删除指定主键数组的实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        public void DeleteEntities<T>(object[] idValues) where T : class
        {
            try
            {
                var tableName = typeof(T).Name;
                
                // 智能过滤：只处理需要缓存的表
                if (!IsTableCacheable(tableName))
                {
                    return;
                }
                
                // 调用基础缓存管理器批量删除缓存
                _cacheManager.DeleteEntities<T>(idValues);

                // 触发缓存删除事件
                OnCacheChanged(tableName, CacheOperation.Remove, idValues);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"批量删除 {typeof(T).Name} 实体缓存并触发事件时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存并触发事件（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        public void DeleteEntities(string tableName, object[] primaryKeyValues)
        {
            try
            {
                // 智能过滤：只处理需要缓存的表
                if (!IsTableCacheable(tableName))
                {
                    return;
                }
                
                // 调用基础缓存管理器批量删除缓存
                _cacheManager.DeleteEntities(tableName, primaryKeyValues);
                
                // 触发缓存删除事件
                OnCacheChanged(tableName, CacheOperation.Remove, primaryKeyValues);
                
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"批量删除表 {tableName} 实体缓存并触发事件时发生错误");
                throw;
            }
        }

        #region 智能过滤方法
        /// <summary>
        /// 检查表是否需要缓存（智能过滤核心方法）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否需要缓存</returns>
        private bool IsTableCacheable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;

            // 获取表结构信息
            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
            
            // 如果表未注册或明确标记为不需要缓存，则跳过
            if (schemaInfo == null || !schemaInfo.IsCacheable)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}