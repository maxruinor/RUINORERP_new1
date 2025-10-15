using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存事件管理器，用于处理本地缓存变更事件
    /// 可以被服务器和客户端共同使用
    /// </summary>
    public class CacheEventManager
    {
        // 定义缓存变更事件
        public event EventHandler<CacheChangedEventArgs> CacheChanged;

        private readonly IEntityCacheManager _cacheManager;
        private readonly TableSchemaManager _tableSchemaManager;

        public CacheEventManager(IEntityCacheManager cacheManager)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _tableSchemaManager = TableSchemaManager.Instance;
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
        /// 更新实体列表缓存并触发事件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">实体列表</param>
        public void UpdateEntityList<T>(List<T> list) where T : class
        {
            var tableName = typeof(T).Name;
            _cacheManager.UpdateEntityList(list);
            OnCacheChanged(tableName, CacheOperation.BatchSet, list);
        }

        /// <summary>
        /// 更新单个实体缓存并触发事件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void UpdateEntity<T>(T entity) where T : class
        {
            var tableName = typeof(T).Name;
            _cacheManager.UpdateEntity(entity);

            // 对于更新操作，我们需要获取主键值
            var primaryKeyValue = GetEntityPrimaryKeyValue(entity, tableName);
            OnCacheChanged(tableName, CacheOperation.Update, entity);
        }

        /// <summary>
        /// 删除实体缓存并触发事件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValue">主键值</param>
        public void DeleteEntity<T>(object idValue) where T : class
        {
            var tableName = typeof(T).Name;
            _cacheManager.DeleteEntity<T>(idValue);
            OnCacheChanged(tableName, CacheOperation.Remove, idValue);
        }

        /// <summary>
        /// 删除多个实体缓存并触发事件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        public void DeleteEntityList<T>(List<T> entities) where T : class
        {
            var tableName = typeof(T).Name;
            _cacheManager.DeleteEntityList(entities);
            OnCacheChanged(tableName, CacheOperation.BatchRemove, entities);
        }
    }
}