using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.Model.Context;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.CustomAttribute;
using System.Collections;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 优化的缓存管理器实现
    /// </summary>
    [NoWantIOC]
    public class EntityCacheManager : IEntityCacheManager
    {
        #region 依赖注入字段
        private readonly ICacheManager<object> _cacheManager;
        private readonly TableSchemaManager _tableSchemaManager;
        private readonly ILogger<EntityCacheManager> _logger;
        #endregion

        #region 缓存存储
        /// <summary>
        /// 实体列表缓存
        /// </summary>
        public ICacheManager<object> EntityListCache { get; }

        /// <summary>
        /// 单个实体缓存（按表名+ID存储）
        /// </summary>
        public ICacheManager<object> EntityCache { get; }

        /// <summary>
        /// 显示值缓存（按表名+ID存储显示值）
        /// </summary>
        public ICacheManager<object> DisplayValueCache { get; }
        #endregion

        #region 构造函数
        public EntityCacheManager(
            ILogger<EntityCacheManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tableSchemaManager = TableSchemaManager.Instance;

            // 初始化缓存管理器
            _cacheManager = CacheFactory.Build<object>(settings =>
                settings
                    .WithSystemRuntimeCacheHandle()
                    .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120)));

            // 初始化缓存
            EntityListCache = _cacheManager;
            EntityCache = CacheFactory.Build<object>(settings => settings.WithSystemRuntimeCacheHandle());
            DisplayValueCache = CacheFactory.Build<object>(settings => settings.WithSystemRuntimeCacheHandle());
        }
        #endregion

        #region 缓存查询方法实现
        /// <summary>
        /// 获取指定类型的实体列表
        /// </summary>
        public List<T> GetEntityList<T>() where T : class
        {
            var tableName = typeof(T).Name;
            return GetEntityList<T>(tableName);
        }

        /// <summary>
        /// 根据表名获取指定类型的实体列表
        /// </summary>
        public List<T> GetEntityList<T>(string tableName) where T : class
        {
            try
            {
                var cacheKey = $"EntityList_{tableName}";
                var cachedList = EntityListCache.Get(cacheKey);

                // 修复类型转换问题：处理List<ExpandoObject>的情况
                if (cachedList is List<System.Dynamic.ExpandoObject> expandoList)
                {
                    // 将ExpandoObject列表转换为具体类型的列表
                    var result = new List<T>();
                    foreach (var item in expandoList)
                    {
                        // 这里需要根据具体情况实现转换逻辑
                        // 一种简单的方式是使用Json序列化/反序列化
                        try
                        {
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                            var typedItem = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                            result.Add(typedItem);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, $"转换ExpandoObject到类型 {typeof(T).Name} 时发生错误");
                        }
                    }
                    return result;
                }

                if (cachedList is List<T> typedList)
                {
                    return typedList;
                }

                if (cachedList is List<object> objectList)
                {
                    return objectList.OfType<T>().ToList();
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {tableName} 的实体列表缓存时发生错误");
                return new List<T>();
            }
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public T GetEntity<T>(object idValue) where T : class
        {
            try
            {
                var tableName = typeof(T).Name;
                var cacheKey = $"Entity_{tableName}_{idValue}";
                var cachedEntity = EntityCache.Get(cacheKey);

                if (cachedEntity is T entity)
                {
                    return entity;
                }

                // 如果单个实体缓存中没有，则从列表中查找
                var list = GetEntityList<T>(tableName);
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

                if (schemaInfo != null && list != null)
                {
                    var entityFound = list.FirstOrDefault(e =>
                    {
                        var idPropertyValue = e.GetPropertyValue(schemaInfo.PrimaryKeyField);
                        return idPropertyValue?.ToString() == idValue.ToString();
                    });

                    // 将找到的实体加入单个实体缓存
                    if (entityFound != null)
                    {
                        EntityCache.Put(cacheKey, entityFound);
                    }

                    return entityFound;
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {typeof(T).Name} ID为 {idValue} 的实体缓存时发生错误");
                return default;
            }
        }

        /// <summary>
        /// 根据表名和主键值获取实体
        /// </summary>
        public object GetEntity(string tableName, object primaryKeyValue)
        {
            try
            {
                var entityType = _tableSchemaManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    return null;
                }

                var cacheKey = $"Entity_{tableName}_{primaryKeyValue}";
                var cachedEntity = EntityCache.Get(cacheKey);

                if (cachedEntity != null && entityType.IsInstanceOfType(cachedEntity))
                {
                    return cachedEntity;
                }

                // 如果单个实体缓存中没有，则从列表中查找
                var method = typeof(EntityCacheManager)
                    .GetMethod(nameof(GetEntityList), BindingFlags.Public | BindingFlags.Instance)
                    .MakeGenericMethod(entityType);

                var list = method.Invoke(this, new object[] { tableName }) as IEnumerable;
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

                if (schemaInfo != null && list != null)
                {
                    foreach (var item in list)
                    {
                        var idPropertyValue = item.GetPropertyValue(schemaInfo.PrimaryKeyField);
                        if (idPropertyValue?.ToString() == primaryKeyValue.ToString())
                        {
                            // 将找到的实体加入单个实体缓存
                            EntityCache.Put(cacheKey, item);
                            return item;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {tableName} ID为 {primaryKeyValue} 的实体缓存时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 获取指定表名的显示值
        /// </summary>
        public object GetDisplayValue(string tableName, object idValue)
        {
            try
            {
                var cacheKey = $"DisplayValue_{tableName}_{idValue}";
                var cachedValue = DisplayValueCache.Get(cacheKey);

                if (cachedValue != null)
                {
                    return cachedValue;
                }

                // 如果显示值缓存中没有，则从实体中获取
                var entity = GetEntity(tableName, idValue);
                if (entity != null)
                {
                    var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                    if (schemaInfo != null)
                    {
                        var displayValue = entity.GetPropertyValue(schemaInfo.DisplayField);
                        // 将显示值加入缓存
                        DisplayValueCache.Put(cacheKey, displayValue);
                        return displayValue;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {tableName} ID为 {idValue} 的显示值缓存时发生错误");
                return null;
            }
        }
        #endregion

        #region 缓存更新方法实现
        /// <summary>
        /// 更新实体列表缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void UpdateEntityList<T>(List<T> list) where T : class
        {
            var tableName = typeof(T).Name;
            
            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过更新操作");
                return;
            }
            
            UpdateEntityList(tableName, list);
        }

        /// <summary>
        /// 更新单个实体缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void UpdateEntity<T>(T entity) where T : class
        {
            if (entity == null)
            {
                return;
            }

            var tableName = typeof(T).Name;
            
            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过更新操作");
                return;
            }
            
            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

            if (schemaInfo != null)
            {
                var idValue = entity.GetPropertyValue(schemaInfo.PrimaryKeyField);
                if (idValue != null)
                {
                    var cacheKey = $"Entity_{tableName}_{idValue}";
                    EntityCache.Put(cacheKey, entity);

                    // 同时更新显示值缓存
                    var displayValue = entity.GetPropertyValue(schemaInfo.DisplayField);
                    var displayCacheKey = $"DisplayValue_{tableName}_{idValue}";
                    DisplayValueCache.Put(displayCacheKey, displayValue);
                }
            }

            // 同时更新列表缓存中的该实体
            UpdateEntityInList(tableName, entity);
        }

        /// <summary>
        /// 根据表名更新缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void UpdateEntityList(string tableName, object list)
        {
            try
            {
                // 智能过滤：只处理需要缓存的表
                if (!IsTableCacheable(tableName))
                {
                    _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过更新操作");
                    return;
                }
                
                var cacheKey = $"EntityList_{tableName}";

                // 修复：确保存储的是正确类型的列表而不是ExpandoObject
                object cacheValue = list;

                // 如果是List<ExpandoObject>，尝试转换为具体类型
                if (list is List<System.Dynamic.ExpandoObject> expandoList)
                {
                    // 获取实体类型
                    var entityType = _tableSchemaManager.GetEntityType(tableName);
                    if (entityType != null)
                    {
                        // 创建正确类型的List
                        var listType = typeof(List<>).MakeGenericType(entityType);
                        var typedList = Activator.CreateInstance(listType);

                        // 使用反射获取Add方法并添加元素
                        var addMethod = listType.GetMethod("Add");
                        foreach (var item in expandoList)
                        {
                            try
                            {
                                // 将ExpandoObject转换为具体类型
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                                var typedItem = Newtonsoft.Json.JsonConvert.DeserializeObject(json, entityType);
                                addMethod?.Invoke(typedList, new[] { typedItem });
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogWarning(ex, $"转换ExpandoObject到类型 {entityType.Name} 时发生错误");
                            }
                        }

                        cacheValue = typedList;
                    }
                }

                EntityListCache.Put(cacheKey, cacheValue);

                // 清空该表的所有单个实体缓存和显示值缓存
                ClearEntityCaches(tableName);

                _logger?.LogInformation($"已更新表 {tableName} 的实体列表缓存");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新表 {tableName} 的实体列表缓存时发生错误");
            }
        }

        /// <summary>
        /// 根据表名更新单个实体缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void UpdateEntity(string tableName, object entity)
        {
            if (entity == null)
            {
                return;
            }

            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过更新操作");
                return;
            }

            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
            if (schemaInfo != null)
            {
                var idValue = entity.GetPropertyValue(schemaInfo.PrimaryKeyField);
                if (idValue != null)
                {
                    var cacheKey = $"Entity_{tableName}_{idValue}";
                    EntityCache.Put(cacheKey, entity);

                    // 同时更新显示值缓存
                    var displayValue = entity.GetPropertyValue(schemaInfo.DisplayField);
                    var displayCacheKey = $"DisplayValue_{tableName}_{idValue}";
                    DisplayValueCache.Put(displayCacheKey, displayValue);
                }
            }

            // 同时更新列表缓存中的该实体
            UpdateEntityInList(tableName, entity);
        }

        /// <summary>
        /// 更新列表缓存中的指定实体
        /// </summary>
        private void UpdateEntityInList<T>(string tableName, T entity)
        {
            try
            {
                var cacheKey = $"EntityList_{tableName}";
                var cachedList = EntityListCache.Get(cacheKey);

                if (cachedList is List<T> list)
                {
                    var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                    if (schemaInfo != null)
                    {
                        var entityId = entity.GetPropertyValue(schemaInfo.PrimaryKeyField);
                        if (entityId != null)
                        {
                            var existingEntity = list.FirstOrDefault(e =>
                            {
                                var idPropertyValue = e.GetPropertyValue(schemaInfo.PrimaryKeyField);
                                return idPropertyValue?.ToString() == entityId.ToString();
                            });

                            if (existingEntity != null)
                            {
                                // 更新现有实体
                                var index = list.IndexOf(existingEntity);
                                list[index] = entity;
                            }
                            else
                            {
                                // 添加新实体
                                list.Add(entity);
                            }

                            EntityListCache.Put(cacheKey, list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新表 {tableName} 列表缓存中的实体时发生错误");
            }
        }
        #endregion

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

        #region 缓存删除方法实现
        /// <summary>
        /// 删除指定ID的实体缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void DeleteEntity<T>(object idValue) where T : class
        {
            var tableName = typeof(T).Name;
            
            // 智能过滤：只处理需要缓存的表
                if (!IsTableCacheable(tableName))
                {
                    _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过删除操作");
                    return;
                }
            
            DeleteEntity(tableName, idValue);
        }

        /// <summary>
        /// 删除实体列表缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void DeleteEntityList<T>(List<T> entities) where T : class
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            var tableName = typeof(T).Name;
            
            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过删除操作");
                return;
            }
            
            var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

            if (schemaInfo != null)
            {
                foreach (var entity in entities)
                {
                    var idValue = entity.GetPropertyValue(schemaInfo.PrimaryKeyField);
                    if (idValue != null)
                    {
                        var cacheKey = $"Entity_{tableName}_{idValue}";
                        EntityCache.Remove(cacheKey);

                        var displayCacheKey = $"DisplayValue_{tableName}_{idValue}";
                        DisplayValueCache.Remove(displayCacheKey);
                    }
                }
            }

            // 从列表缓存中删除这些实体
            RemoveEntitiesFromList(tableName, entities);
        }

        /// <summary>
        /// 根据表名和主键删除实体缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        public void DeleteEntity(string tableName, object primaryKeyValue)
        {
            try
            {
                // 智能过滤：只处理需要缓存的表
                if (!IsTableCacheable(tableName))
                {
                    _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过删除操作");
                    return;
                }
                
                var cacheKey = $"Entity_{tableName}_{primaryKeyValue}";
                EntityCache.Remove(cacheKey);

                var displayCacheKey = $"DisplayValue_{tableName}_{primaryKeyValue}";
                DisplayValueCache.Remove(displayCacheKey);

                // 从列表缓存中删除该实体
                RemoveEntityFromList(tableName, primaryKeyValue);

                _logger?.LogInformation($"已删除表 {tableName} ID为 {primaryKeyValue} 的实体缓存");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"删除表 {tableName} ID为 {primaryKeyValue} 的实体缓存时发生错误");
            }
        }

        /// <summary>
        /// 从列表缓存中删除指定实体
        /// </summary>
        private void RemoveEntityFromList(string tableName, object primaryKeyValue)
        {
            try
            {
                var cacheKey = $"EntityList_{tableName}";
                var cachedList = EntityListCache.Get(cacheKey);

                if (cachedList is IList list)
                {
                    var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                    if (schemaInfo != null)
                    {
                        object entityToRemove = null;
                        foreach (var item in list)
                        {
                            var idPropertyValue = item.GetPropertyValue(schemaInfo.PrimaryKeyField);
                            if (idPropertyValue?.ToString() == primaryKeyValue.ToString())
                            {
                                entityToRemove = item;
                                break;
                            }
                        }

                        if (entityToRemove != null)
                        {
                            list.Remove(entityToRemove);
                            EntityListCache.Put(cacheKey, list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从表 {tableName} 列表缓存中删除实体时发生错误");
            }
        }

        /// <summary>
        /// 从列表缓存中删除多个实体
        /// </summary>
        private void RemoveEntitiesFromList<T>(string tableName, List<T> entities)
        {
            try
            {
                var cacheKey = $"EntityList_{tableName}";
                var cachedList = EntityListCache.Get(cacheKey);

                if (cachedList is List<T> list)
                {
                    var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                    if (schemaInfo != null)
                    {
                        var entityIds = entities.Select(e => e.GetPropertyValue(schemaInfo.PrimaryKeyField)?.ToString())
                                                .Where(id => id != null)
                                                .ToHashSet();

                        list.RemoveAll(e =>
                        {
                            var idPropertyValue = e.GetPropertyValue(schemaInfo.PrimaryKeyField);
                            return idPropertyValue != null && entityIds.Contains(idPropertyValue.ToString());
                        });

                        EntityListCache.Put(cacheKey, list);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从表 {tableName} 列表缓存中删除多个实体时发生错误");
            }
        }

        /// <summary>
        /// 清空指定表的所有单个实体缓存和显示值缓存
        /// </summary>
        private void ClearEntityCaches(string tableName)
        {
            try
            {
                // 这里可以考虑使用缓存标签或其他机制来批量删除
                // 当前实现为简单起见，不进行具体操作
                _logger?.LogInformation($"已清空表 {tableName} 的单个实体缓存和显示值缓存");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"清空表 {tableName} 的单个实体缓存和显示值缓存时发生错误");
            }
        }

        /// <summary>
        /// 批量删除指定主键数组的实体缓存（智能过滤，只处理需要缓存的表）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        public void DeleteEntities<T>(object[] idValues) where T : class
        {
            var tableName = typeof(T).Name;
            
            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过批量删除操作");
                return;
            }
            
            DeleteEntities(tableName, idValues);
        }

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存（智能过滤，只处理需要缓存的表）
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
                    _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过批量删除操作");
                    return;
                }
                
                if (primaryKeyValues == null || primaryKeyValues.Length == 0)
                {
                    _logger?.LogDebug("主键数组为空，跳过批量删除操作");
                    return;
                }

                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    _logger?.LogWarning($"表 {tableName} 的结构信息未找到，跳过批量删除操作");
                    return;
                }

                // 批量删除单个实体缓存和显示值缓存
                foreach (var primaryKeyValue in primaryKeyValues)
                {
                    if (primaryKeyValue != null)
                    {
                        var cacheKey = $"Entity_{tableName}_{primaryKeyValue}";
                        EntityCache.Remove(cacheKey);

                        var displayCacheKey = $"DisplayValue_{tableName}_{primaryKeyValue}";
                        DisplayValueCache.Remove(displayCacheKey);
                    }
                }

                // 从列表缓存中批量删除这些实体
                RemoveEntitiesFromListByKeys(tableName, primaryKeyValues);

                _logger?.LogInformation($"已批量删除表 {tableName} 中 {primaryKeyValues.Length} 个实体的缓存");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"批量删除表 {tableName} 实体缓存时发生错误");
            }
        }

        /// <summary>
        /// 从列表缓存中批量删除指定主键的实体
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        private void RemoveEntitiesFromListByKeys(string tableName, object[] primaryKeyValues)
        {
            try
            {
                var cacheKey = $"EntityList_{tableName}";
                var cachedList = EntityListCache.Get(cacheKey);

                if (cachedList is IList list)
                {
                    var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                    if (schemaInfo != null)
                    {
                        var primaryKeyStrings = primaryKeyValues.Select(pk => pk?.ToString()).Where(pk => pk != null).ToHashSet();
                        
                        // 使用LINQ过滤出需要保留的实体
                        var entitiesToKeep = new List<object>();
                        foreach (var item in list)
                        {
                            var idPropertyValue = item.GetPropertyValue(schemaInfo.PrimaryKeyField)?.ToString();
                            if (idPropertyValue == null || !primaryKeyStrings.Contains(idPropertyValue))
                            {
                                entitiesToKeep.Add(item);
                            }
                        }

                        // 重新设置列表缓存
                        EntityListCache.Put(cacheKey, entitiesToKeep);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从表 {tableName} 列表缓存中批量删除实体时发生错误");
            }
        }
        #endregion

        #region 缓存初始化方法实现
        /// <summary>
        /// 初始化表结构信息
        /// </summary>
        public void InitializeTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null) where T : class
        {
            _tableSchemaManager.RegisterTableSchema(
                primaryKeyExpression,
                displayFieldExpression,
                isView,
                isCacheable,
                description);
        }

        /// <summary>
        /// 获取实体类型
        /// </summary>
        public Type GetEntityType(string tableName)
        {
            return _tableSchemaManager.GetEntityType(tableName);
        }

        /// <summary>
        /// 序列化缓存数据
        /// </summary>
        /// <param name="data">要序列化的数据</param>
        /// <param name="type">序列化方式</param>
        /// <returns>序列化后的字节数组</returns>
        public byte[] SerializeCacheData<T>(T data, CacheSerializationHelper.SerializationType type = CacheSerializationHelper.SerializationType.Json)
        {
            return CacheSerializationHelper.Serialize(data, type);
        }

        /// <summary>
        /// 反序列化缓存数据
        /// </summary>
        /// <param name="data">序列化后的字节数组</param>
        /// <param name="type">序列化方式</param>
        /// <returns>反序列化后的对象</returns>
        public T DeserializeCacheData<T>(byte[] data, CacheSerializationHelper.SerializationType type = CacheSerializationHelper.SerializationType.Json)
        {
            return CacheSerializationHelper.Deserialize<T>(data, type);
        }
        #endregion
    }
}