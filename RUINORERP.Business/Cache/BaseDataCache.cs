using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache.Attributes;
using RUINORERP.Model.Base;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 基础数据缓存实现
    /// 为各种基础数据实体提供统一的缓存管理功能
    /// </summary>
    [CacheName("BaseData")]
    public class BaseDataCache : EntityCacheServiceBase<BaseEntity>, IBaseDataCache
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="defaultPolicy">默认缓存策略</param>
        public BaseDataCache(ICacheManager<object> cacheManager, ILogger<BaseDataCache> logger, CachePolicy defaultPolicy = null)
            : base(cacheManager, logger, "BaseData", defaultPolicy)
        {}

        // --- 泛型实体缓存操作 --- //
        
        /// <summary>
        /// 获取指定实体类型的所有缓存实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体列表</returns>
        public List<T> GetEntityList<T>() where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                return cache.GetEntityList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取实体列表失败: {typeof(T).Name}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 异步获取指定实体类型的所有缓存实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体列表</returns>
        public async Task<List<T>> GetEntityListAsync<T>() where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                return await cache.GetEntityListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步获取实体列表失败: {typeof(T).Name}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 获取指定ID的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        public T GetEntity<T>(long id) where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                return cache.GetEntity(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取实体失败: {typeof(T).Name}, ID: {id}");
                return null;
            }
        }

        /// <summary>
        /// 异步获取指定ID的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        public async Task<T> GetEntityAsync<T>(long id) where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                return await cache.GetEntityAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步获取实体失败: {typeof(T).Name}, ID: {id}");
                return null;
            }
        }

        /// <summary>
        /// 设置实体列表缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        /// <param name="expiration">过期时间</param>
        public void SetEntityList<T>(List<T> entities, TimeSpan? expiration = null) where T : BaseEntity
        {
            try
            {
                if (entities == null || !entities.Any())
                    return;

                var cache = GetTypedCache<T>();
                cache.SetEntityList(entities, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置实体列表缓存失败: {typeof(T).Name}");
            }
        }

        /// <summary>
        /// 异步设置实体列表缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        /// <param name="expiration">过期时间</param>
        public async Task SetEntityListAsync<T>(List<T> entities, TimeSpan? expiration = null) where T : BaseEntity
        {
            try
            {
                if (entities == null || !entities.Any())
                    return;

                var cache = GetTypedCache<T>();
                await cache.SetEntityListAsync(entities, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步设置实体列表缓存失败: {typeof(T).Name}");
            }
        }

        /// <summary>
        /// 更新实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void UpdateEntity<T>(T entity) where T : BaseEntity
        {
            try
            {
                if (entity == null)
                    return;

                var cache = GetTypedCache<T>();
                cache.SetEntity(entity, entity.PrimaryKeyID);

                // 同时更新列表缓存（实际项目中可能需要更复杂的策略）
                var list = GetEntityList<T>();
                var index = list.FindIndex(e => e.PrimaryKeyID == entity.PrimaryKeyID);

                if (index >= 0)
                {
                    list[index] = entity;
                }
                else
                {
                    list.Add(entity);
                }

                cache.SetEntityList(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新实体缓存失败: {typeof(T).Name}, ID: {entity?.PrimaryKeyID}");
            }
        }

        /// <summary>
        /// 异步更新实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public async Task UpdateEntityAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                if (entity == null)
                    return;

                var cache = GetTypedCache<T>();
                await cache.SetEntityAsync(entity, entity.PrimaryKeyID);

                // 同时更新列表缓存（实际项目中可能需要更复杂的策略）
                var list = await GetEntityListAsync<T>();
                var index = list.FindIndex(e => e.PrimaryKeyID == entity.PrimaryKeyID);

                if (index >= 0)
                {
                    list[index] = entity;
                }
                else
                {
                    list.Add(entity);
                }

                await cache.SetEntityListAsync(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步更新实体缓存失败: {typeof(T).Name}, ID: {entity?.PrimaryKeyID}");
            }
        }

        /// <summary>
        /// 移除实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体ID</param>
        public void RemoveEntity<T>(long id) where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                cache.RemoveEntity(id);

                // 从列表中移除
                var list = GetEntityList<T>();
                list.RemoveAll(e => e.PrimaryKeyID == id);
                cache.SetEntityList(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"移除实体缓存失败: {typeof(T).Name}, ID: {id}");
            }
        }

        /// <summary>
        /// 异步移除实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体ID</param>
        public async Task RemoveEntityAsync<T>(long id) where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                await cache.RemoveEntityAsync(id);

                // 从列表中移除
                var list = await GetEntityListAsync<T>();
                list.RemoveAll(e => e.PrimaryKeyID == id);
                await cache.SetEntityListAsync(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步移除实体缓存失败: {typeof(T).Name}, ID: {id}");
            }
        }

        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体列表</returns>
        public List<T> QueryEntities<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            try
            {
                if (predicate == null)
                    return GetEntityList<T>();

                var list = GetEntityList<T>();
                return list.AsQueryable().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"查询实体列表失败: {typeof(T).Name}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 获取或设置实体列表缓存
        /// 如果缓存不存在，则通过factory函数获取数据并缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>实体列表</returns>
        public List<T> GetOrSetEntityList<T>(Func<List<T>> factory, TimeSpan? expiration = null) where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                return cache.GetOrSetEntityList(factory, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取或设置实体列表缓存失败: {typeof(T).Name}");
                return factory?.Invoke() ?? new List<T>();
            }
        }

        /// <summary>
        /// 异步获取或设置实体列表缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>实体列表</returns>
        public async Task<List<T>> GetOrSetEntityListAsync<T>(Func<Task<List<T>>> factory, TimeSpan? expiration = null) where T : BaseEntity
        {
            try
            {
                var cache = GetTypedCache<T>();
                return await cache.GetOrSetEntityListAsync(factory, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步获取或设置实体列表缓存失败: {typeof(T).Name}");
                return factory != null ? await factory() : new List<T>();
            }
        }

        /// <summary>
        /// 清空特定实体类型的所有缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public void ClearEntityCache<T>() where T : BaseEntity
        {
            try
            {
                var entityTypeKey = typeof(T).Name;
                var keysToRemove = GetKeys().Where(k => k.StartsWith(entityTypeKey)).ToList();

                foreach (var key in keysToRemove)
                {
                    Remove(key);
                }

                _logger.LogInformation($"清空实体缓存: {typeof(T).Name}, 共移除 {keysToRemove.Count} 项");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"清空实体缓存失败: {typeof(T).Name}");
            }
        }

        /// <summary>
        /// 异步清空特定实体类型的所有缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public async Task ClearEntityCacheAsync<T>() where T : BaseEntity
        {
            try
            {
                var entityTypeKey = typeof(T).Name;
                var keysToRemove = GetKeys().Where(k => k.StartsWith(entityTypeKey)).ToList();

                foreach (var key in keysToRemove)
                {
                    await RemoveAsync(key);
                }

                _logger.LogInformation($"异步清空实体缓存: {typeof(T).Name}, 共移除 {keysToRemove.Count} 项");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"异步清空实体缓存失败: {typeof(T).Name}");
            }
        }

        // --- 内部辅助方法 --- //

        /// <summary>
        /// 获取指定类型的实体缓存服务实例
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体缓存服务实例</returns>
        private EntityCacheServiceBase<T> GetTypedCache<T>() where T : BaseEntity
        {
            // 创建特定类型的实体缓存服务实例
            return new EntityCacheServiceBase<T>(_cacheManager, _logger, $"BaseData:{typeof(T).Name}", _defaultPolicy);
        }
    }
}


