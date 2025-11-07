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
using RUINORERP.Business.CommService;
using static RUINORERP.Business.Cache.IEntityCacheManager;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 优化的缓存管理器实现
    /// </summary>
    public class EntityCacheManager : IEntityCacheManager
    {
        #region 依赖注入字段
        private readonly TableSchemaManager _tableSchemaManager;
        private readonly ILogger<EntityCacheManager> _logger;
        private readonly ICacheDataProvider _cacheDataProvider;
        private readonly ICacheSyncMetadata _cacheSyncMetadata;
        #endregion

        #region 缓存同步元数据接口实现
        /// <summary>
        /// 获取指定表的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>缓存同步元数据</returns>
        public CacheSyncInfo GetTableSyncInfo(string tableName)
        {
            return _cacheSyncMetadata?.GetTableSyncInfo(tableName);
        }

        /// <summary>
        /// 设置表缓存的过期时间
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="expirationTime">过期时间</param>
        public void SetTableCacheExpiration(string tableName, DateTime expirationTime)
        {
            _cacheSyncMetadata?.SetTableExpiration(tableName, expirationTime);
        }
        #endregion
        #region 缓存存储
        /// <summary>
        /// 统一缓存管理器 - 优化：使用单一缓存管理器管理所有缓存数据
        /// </summary>
        private ICacheManager<object> _cacheManager;

        // 使用接口中定义的CacheKeyType枚举

        #region 缓存统计字段
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        private long _cacheHits;

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        private long _cacheMisses;

        /// <summary>
        /// 缓存写入次数
        /// </summary>
        private long _cachePuts;

        /// <summary>
        /// 缓存删除次数
        /// </summary>
        private long _cacheRemoves;

        /// <summary>
        /// 缓存项统计信息字典
        /// </summary>
        private readonly Dictionary<string, CacheItemStatistics> _cacheItemStatistics = new Dictionary<string, CacheItemStatistics>();

        /// <summary>
        /// 缓存统计锁，保证线程安全
        /// </summary>
        private readonly object _statisticsLock = new object();

        /// <summary>
        /// 最大缓存大小（已调整为800MB以避免频繁清理）
        /// </summary>
        private readonly long _maxCacheSize = 800 * 1024 * 1024;

        /// <summary>
        /// 缓存大小检查阈值（达到最大大小的80%时触发清理）
        /// </summary>
        private readonly long _cacheSizeThreshold;
        #endregion
        #endregion

        #region 缓存更新辅助方法
        /// <summary>
        /// 更新列表缓存中的单个实体
        /// 当从数据源获取到单个实体时，同步更新到列表缓存中
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="entity">要更新的实体</param>
        private void UpdateEntityInList<T>(string tableName, T entity) where T : class
        {
            try
            {
                var listCacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
                var cachedListObj = _cacheManager.Get(listCacheKey);

                if (cachedListObj == null)
                    return;

                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                    return;

                var entityId = entity.GetCachePropertyValue(schemaInfo.PrimaryKeyField);
                bool updated = false;

                // 根据缓存对象类型选择适当的更新方法
                if (cachedListObj is JArray jArray)
                {
                    updated = UpdateJArrayEntity(jArray, schemaInfo.PrimaryKeyField, entityId, entity);
                }
                else if (cachedListObj is List<ExpandoObject> expandoList)
                {
                    updated = UpdateExpandoListEntity(expandoList, schemaInfo.PrimaryKeyField, entityId, entity);
                }
                else if (cachedListObj is IList && cachedListObj.GetType().IsGenericType)
                {
                    updated = UpdateGenericListEntity(cachedListObj, schemaInfo.PrimaryKeyField, entityId, entity, tableName);
                }

                // 如果已更新，则将列表放回缓存
                if (updated)
                {
                    _cacheManager.Put(listCacheKey, cachedListObj);
                    _logger?.LogDebug($"已成功更新表 {tableName} 中ID为 {entityId} 的实体缓存");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新表 {tableName} 的列表缓存中的实体时发生错误");
            }
        }

        /// <summary>
        /// 比较两个实体ID是否匹配
        /// </summary>
        private bool AreEntityIdsMatching(object id1, object id2)
        {
            return id1?.ToString() == id2?.ToString();
        }

        /// <summary>
        /// 更新JArray中的实体
        /// </summary>
        private bool UpdateJArrayEntity(JArray jArray, string primaryKeyField, object entityId, object entity)
        {
            for (int i = 0; i < jArray.Count; i++)
            {
                if (jArray[i] is JObject jobj && AreEntityIdsMatching(jobj[primaryKeyField], entityId))
                {
                    // 替换为新的实体数据
                    jArray[i] = JObject.FromObject(entity);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新ExpandoObject列表中的实体
        /// </summary>
        private bool UpdateExpandoListEntity(List<ExpandoObject> expandoList, string primaryKeyField, object entityId, object entity)
        {
            for (int i = 0; i < expandoList.Count; i++)
            {
                var expando = expandoList[i] as IDictionary<string, object>;
                if (expando != null && expando.ContainsKey(primaryKeyField) &&
                    AreEntityIdsMatching(expando[primaryKeyField], entityId))
                {
                    // 创建新的ExpandoObject并复制属性
                    var newExpando = new ExpandoObject();
                    var expandoDict = (IDictionary<string, object>)newExpando;

                    foreach (var prop in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        expandoDict[prop.Name] = prop.GetValue(entity);
                    }

                    expandoList[i] = newExpando;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新强类型List<T>中的实体
        /// </summary>
        private bool UpdateGenericListEntity(object cachedListObj, string primaryKeyField, object entityId, object entity, string tableName)
        {
            var listType = cachedListObj.GetType();
            var itemType = listType.GenericTypeArguments[0];

            // 创建一个强类型的列表引用
            var typedList = Convert.ChangeType(cachedListObj, typeof(List<>).MakeGenericType(itemType));

            if (typedList is IList list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item != null)
                    {
                        var cachedItemId = item.GetCachePropertyValue(primaryKeyField);
                        if (AreEntityIdsMatching(cachedItemId, entityId))
                        {
                            // 找到匹配的实体，替换为新实体
                            // 确保实体类型与列表项类型兼容
                            if (itemType.IsAssignableFrom(entity.GetType()))
                            {
                                list[i] = entity;
                            }
                            else
                            {
                                // 如果类型不完全匹配，尝试转换
                                try
                                {
                                    var convertedEntity = Convert.ChangeType(entity, itemType);
                                    list[i] = convertedEntity;
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogWarning(ex, $"无法将实体转换为目标类型 {itemType.Name}");
                                    continue;
                                }
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region 缓存配置方法
        /// <summary>
        /// 获取缓存过期时间（可配置）
        /// </summary>
        /// <returns>缓存过期时间间隔</returns>
        private TimeSpan GetCacheExpirationTime()
        {
            try
            {
                // 这里可以从配置文件、数据库或其他配置源获取过期时间
                // 目前返回默认值，后续可以扩展为从配置中心获取
                return TimeSpan.FromHours(2); // 默认2小时过期
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取缓存过期时间配置失败，使用默认值");
                return TimeSpan.FromHours(2); // 默认2小时过期
            }
        }
        #endregion

        #region 缓存键生成方法
        /// <summary>
        /// 生成统一格式的缓存键
        /// </summary>
        /// <param name="keyType">缓存键类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValue">主键值（用于实体和显示值缓存）</param>
        /// <returns>格式化的缓存键</returns>
        public string GenerateCacheKey(CacheKeyType keyType, string tableName, object primaryKeyValue = null)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentNullException(nameof(tableName), "表名不能为空");
                }

                // 根据不同的缓存类型生成不同格式的缓存键
                switch (keyType)
                {
                    case CacheKeyType.List:
                        // 实体列表缓存键格式：Table_{表名}_List
                        return $"Table_{tableName}_List";

                    case CacheKeyType.Entity:
                        // 单个实体缓存键格式：Table_{表名}_Entity_{主键值}
                        // 对于实体缓存，主键值是必需的
                        if (primaryKeyValue == null)
                        {
                            throw new ArgumentNullException(nameof(primaryKeyValue), "对于Entity类型的缓存键，主键值不能为空");
                        }
                        return $"Table_{tableName}_Entity_{primaryKeyValue}";

                    case CacheKeyType.DisplayValue:
                        // 显示值缓存键格式：Table_{表名}_Display_{主键值}
                        return $"Table_{tableName}_Display_{primaryKeyValue ?? string.Empty}";

                    case CacheKeyType.QueryResult:
                        // 查询结果缓存键格式：Table_{表名}_Query_{查询标识}
                        // 使用主键值作为查询标识（可以是查询条件的哈希值或其他标识）
                        return $"Table_{tableName}_Query_{primaryKeyValue ?? string.Empty}";

                    default:
                        // 对于未明确支持的类型，记录警告并使用默认格式
                        _logger?.LogWarning($"不支持的缓存键类型: {keyType}，使用默认格式");
                        return $"Table_{tableName}_{keyType}_{primaryKeyValue ?? string.Empty}";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"生成缓存键时发生错误。类型: {keyType}, 表名: {tableName}");
                throw;
            }
        }
        #endregion

        #region 构造函数
        public EntityCacheManager(
            ILogger<EntityCacheManager> logger,
        ICacheDataProvider cacheDataProvider = null,
            ICacheSyncMetadata cacheSyncMetadata = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tableSchemaManager = TableSchemaManager.Instance;
            _cacheDataProvider = cacheDataProvider;
            _cacheSyncMetadata = cacheSyncMetadata; // 可选依赖
            // 设置缓存大小阈值（最大大小的80%）
            _cacheSizeThreshold = (long)(_maxCacheSize * 0.8);

            // 初始化统一缓存管理器 - 优化：使用单一缓存管理器，统一存储结构
            _cacheManager = CacheFactory.Build<object>(settings =>
                settings
                    .WithSystemRuntimeCacheHandle()
                    .WithExpiration(ExpirationMode.Sliding, GetCacheExpirationTime())); // 使用可配置的缓存过期时间

            // 初始化缓存监控日志
            _logger.Debug("实体缓存管理器初始化完成，最大缓存大小：{MaxSize}MB，清理阈值：{Threshold}MB，过期时间：{ExpirationTime}小时",
                _maxCacheSize / (1024 * 1024), _cacheSizeThreshold / (1024 * 1024), GetCacheExpirationTime().TotalHours);
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
        /// 添加缓存丢失检测：当获取到的缓存为空或0行，但基础表缓存信息显示应有数据时，从数据源重新获取
        /// </summary>
        public List<T> GetEntityList<T>(string tableName) where T : class
        {
            try
            {
                var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
                var cachedList = _cacheManager.Get(cacheKey);

                // 检查缓存是否过期，如果过期则强制从数据源获取
                bool isCacheExpired = _cacheSyncMetadata?.IsTableExpired(tableName) ?? false;
                if (isCacheExpired)
                {
                    _logger?.LogDebug($"表 {tableName} 的缓存已过期，将从数据源获取最新数据");
                    cachedList = null; // 强制从数据源获取
                }

                // 更新缓存访问统计
                UpdateCacheAccessStatistics(cacheKey, cachedList != null, "List", tableName, cachedList);

                // 检查是否存在缓存丢失情况：缓存返回了空列表，但缓存同步元数据显示应该有数据或缓存不完整
                bool isCacheMissing = false;
                if (cachedList is List<T> listObj && listObj.Count == 0)
                {
                    try
                    {
                        // 使用缓存同步元数据验证缓存完整性
                        if (_cacheSyncMetadata != null)
                        {
                            // 验证表缓存数据的完整性
                            bool isCacheValid = _cacheSyncMetadata.ValidateTableCacheIntegrity(tableName);
                            if (!isCacheValid)
                            {
                                _logger?.LogWarning($"检测到表 {tableName} 缓存不完整，将从数据源重新获取数据。");
                                isCacheMissing = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogDebug(ex, "验证缓存完整性时发生错误");
                    }
                }

                // 如果缓存为空、已过期或检测到缓存丢失，尝试从数据源获取
                if (cachedList == null || isCacheMissing)
                {
                    if (_cacheDataProvider != null)
                    {
                        try
                        {
                            List<T> list = null;

                            // 处理当T是Object类型的情况
                            if (typeof(T) == typeof(object))
                            {
                                // 从TableSchemaManager获取正确的实体类型
                                var entityType = _tableSchemaManager.GetEntityType(tableName);
                                if (entityType != null)
                                {
                                    _logger?.LogDebug($"检测到T是Object类型，从表名 {tableName} 获取实体类型: {entityType.Name}");

                                    // 使用反射创建并调用正确类型的GetEntityListFromSource方法
                                    var getListMethod = typeof(ICacheDataProvider).GetMethod("GetEntityListFromSource");
                                    if (getListMethod != null)
                                    {
                                        var genericMethod = getListMethod.MakeGenericMethod(entityType);
                                        var stronglyTypedList = genericMethod.Invoke(_cacheDataProvider, new object[] { tableName }) as IEnumerable;

                                        if (stronglyTypedList != null)
                                        {
                                            // 转换为List<object>返回
                                            list = stronglyTypedList.Cast<T>().ToList();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 正常情况：直接使用T类型查询
                                list = _cacheDataProvider.GetEntityListFromSource<T>(tableName);
                            }

                            // 即使列表为空也要缓存，避免空表频繁查询数据库
                            if (list != null)
                            {
                                // 将获取到的数据（包括空列表）更新到缓存
                                PutToCache(cacheKey, list, "List", tableName);

                                // 直接更新缓存同步元数据
                                UpdateCacheSyncMetadataAfterEntityChange(tableName);

                                return list;
                            }
                        }
                        catch (Exception dataEx)
                        {
                            _logger?.LogError(dataEx, $"从数据源获取表 {tableName} 的实体列表时发生错误");
                        }
                    }
                    return new List<T>();
                }

                // 支持处理从socket传输过来的JArray类型数据
                if (cachedList != null && cachedList.GetType().FullName == "Newtonsoft.Json.Linq.JArray")
                {
                    try
                    {
                        var jArray = cachedList as Newtonsoft.Json.Linq.JArray;
                        return jArray?.ToObject<List<T>>() ?? new List<T>();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, $"转换JArray到类型 {typeof(T).Name} 时发生错误");
                        return new List<T>();
                    }
                }

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

                // 在catch块中不再重复数据源获取逻辑，因为已在try块前部处理
                return new List<T>();
            }
        }

        /// <summary>
        /// 根据ID从列表缓存中获取实体
        /// 注意：现在只从列表缓存中查找，不再使用单个实体缓存
        /// </summary>
        public T GetEntity<T>(object idValue) where T : class
        {
            try
            {
                var tableName = typeof(T).Name;

                // 直接从列表缓存中查找实体
                // GetEntityList方法内部已经处理了缓存过期的情况
                var list = GetEntityList<T>(tableName);
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

                if (schemaInfo != null && list != null)
                {
                    var entityFound = list.FirstOrDefault(e =>
                    {
                        var idPropertyValue = e.GetCachePropertyValue(schemaInfo.PrimaryKeyField);
                        return idPropertyValue?.ToString() == idValue.ToString();
                    });

                    if (entityFound != null)
                    {
                        _logger?.LogDebug($"从表 {tableName} 的列表缓存中找到ID为 {idValue} 的实体");
                        return entityFound;
                    }
                    // 如果列表中也没有，则尝试从数据源获取（如果提供了数据提供者）
                    else if (_cacheDataProvider != null)
                    {
                        try
                        {
                            // 处理当T是Object类型的情况
                            if (typeof(T) == typeof(object))
                            {
                                // 从TableSchemaManager获取正确的实体类型
                                var entityType = _tableSchemaManager.GetEntityType(tableName);
                                if (entityType != null)
                                {
                                    _logger?.LogDebug($"检测到T是Object类型，从表名 {tableName} 获取实体类型: {entityType.Name}");

                                    // 使用反射创建并调用正确类型的GetEntityFromSource方法
                                    var getEntityMethod = typeof(ICacheDataProvider).GetMethod("GetEntityFromSource");
                                    if (getEntityMethod != null)
                                    {
                                        var genericMethod = getEntityMethod.MakeGenericMethod(entityType);
                                        entityFound = genericMethod.Invoke(_cacheDataProvider, new object[] { tableName, idValue }) as T;

                                        if (entityFound != null)
                                        {
                                            // 只更新列表缓存
                                            UpdateEntityInList(tableName, entityFound);
                                            _logger?.LogDebug($"从数据源获取表 {tableName} ID为 {idValue} 的实体并更新到列表缓存");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 正常情况：直接使用T类型查询
                                entityFound = _cacheDataProvider.GetEntityFromSource<T>(tableName, idValue);
                                if (entityFound != null)
                                {
                                    // 只更新列表缓存
                                    UpdateEntityInList(tableName, entityFound);
                                    _logger?.LogDebug($"从数据源获取表 {tableName} ID为 {idValue} 的实体并更新到列表缓存");
                                }
                            }
                            return entityFound;
                        }
                        catch (Exception dataEx)
                        {
                            _logger?.LogError(dataEx, $"从数据源获取表 {tableName} ID为 {idValue} 的实体时发生错误");
                        }
                    }
                    return null;
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
                // 直接从列表缓存中查找实体
                var entityType = _tableSchemaManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    return null;
                }

                // 如果需要缓存访问统计，可以使用特定的键格式
                var accessStatisticsKey = $"Access_Entity_{tableName}_{primaryKeyValue}";

                // 直接从列表中查找 - 使用接受tableName参数的重载版本
                var method = typeof(EntityCacheManager)
                    .GetMethod(nameof(GetEntityList), new[] { typeof(string) })
                    .MakeGenericMethod(entityType);

                var list = method.Invoke(this, new object[] { tableName }) as IEnumerable;
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

                if (schemaInfo != null && list != null)
                {
                    foreach (var item in list)
                    {
                        var idPropertyValue = item.GetCachePropertyValue(schemaInfo.PrimaryKeyField);
                        if (idPropertyValue != null && idPropertyValue.SafeEquals(primaryKeyValue))
                        {
                            // 更新缓存访问统计
                            UpdateCacheAccessStatistics(accessStatisticsKey, true, "Entity", tableName, item);
                            return item;
                        }
                    }
                }

                // 更新缓存访问统计 - 未找到
                UpdateCacheAccessStatistics(accessStatisticsKey, false, "Entity", tableName, null);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {tableName} ID为 {primaryKeyValue} 的实体时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 获取指定表名和主键的显示值
        /// </summary>
        public object GetDisplayValue(string tableName, object idValue)
        {
            try
            {
                // 如果需要缓存访问统计，可以使用特定的键格式
                var accessStatisticsKey = $"Access_Display_{tableName}_{idValue}";
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo != null)
                {   // 直接从实体中获取显示值
                    var entity = GetEntity(tableName, idValue);
                    if (entity != null)
                    {
                        var displayValue = entity.GetCachePropertyValue(schemaInfo.DisplayField);
                        // 更新缓存访问统计
                        UpdateCacheAccessStatistics(accessStatisticsKey, true, "Display", tableName, displayValue);
                        return displayValue;
                    }
                }
                else
                {
                    //不在缓存中的表。直接返回原值
                    return idValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {tableName} ID为 {idValue} 的显示值时发生错误");
                return null;
            }
        }
        
        /// <summary>
        /// 根据表名获取实体列表，返回强类型集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表，类型为表对应的强类型集合</returns>
        public dynamic GetEntityListByTableName(string tableName)
        {
            try
            {
                // 获取表对应的实体类型
                var entityType = _tableSchemaManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _logger?.LogWarning($"无法获取表 {tableName} 对应的实体类型");
                    return null;
                }
                
                _logger?.LogDebug($"检测到表 {tableName} 对应的实体类型: {entityType.Name}");
                
                // 使用反射调用泛型的GetEntityList方法
                var getListMethod = typeof(EntityCacheManager)
                    .GetMethod(nameof(GetEntityList), new[] { typeof(string) })
                    .MakeGenericMethod(entityType);
                
                // 调用方法并返回结果
                return getListMethod.Invoke(this, new object[] { tableName });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取表 {tableName} 的实体列表时发生错误");
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
        /// 更新列表缓存中的单个实体（智能过滤，只处理需要缓存的表）
        /// 会自动清理相关的Entity和DisplayValue缓存，保持缓存一致性
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

            // 直接更新列表缓存中的该实体
            UpdateEntityInList(tableName, entity);

            _logger?.LogDebug($"已更新表 {tableName} 列表缓存中的单个实体，并自动清理相关的Entity和DisplayValue缓存");

            // 更新缓存同步元数据
            UpdateCacheSyncMetadataAfterEntityChange(tableName);
        }

        /// <summary>
        /// 根据表名更新缓存（智能过滤，只处理需要缓存的表）
        /// 智能更新逻辑：
        /// 1. 如果传入的是单个实体对象，使用主键查找并更新列表中的相应项
        /// 2. 如果传入的是集合对象（List、Array、IEnumerable等），则更新整个缓存
        /// </summary>
        public void UpdateEntityList(string tableName, object list)
        {
            try
            {
                // 使用封装的智能过滤方法
                if (!CheckAndLogIfTableCacheable(tableName))
                {
                    return;
                }

                var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);

                // 智能检测：判断是单个实体还是集合
                // 1. 检查是否是单个实体对象
                // 获取实体类型信息
                var entityType = _tableSchemaManager.GetEntityType(tableName);
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);

                // 判断是否为单个实体对象（非字符串、非集合类型，且类型匹配或可转换）
                if (list != null &&
                    !(list is string) &&
                    !(list is Newtonsoft.Json.Linq.JArray) &&
                    !(list is IEnumerable) &&
                    entityType != null &&
                    schemaInfo != null)
                {
                    // 获取列表缓存，检查是否已存在
                    var cachedListObj = _cacheManager.Get(cacheKey);
                    if (cachedListObj != null)
                    {
                        // 如果缓存中存在列表，执行单个实体更新
                        try
                        {
                            // 使用UpdateEntityInList方法更新单个实体
                            // 需要根据实体类型进行适当转换
                            var entity = Convert.ChangeType(list, entityType);
                            UpdateEntityInList(tableName, entity);

                            // 更新缓存同步元数据
                            UpdateCacheSyncMetadataAfterEntityChange(tableName);

                            return;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, $"尝试执行单个实体更新失败，回退到更新整个缓存: {ex.Message}");
                        }
                    }
                }

                // 修复：确保存储的是正确类型的列表
                object cacheValue = list;

                // 处理字符串格式的JSON数据（可能来自socket传输）
                if (list is string jsonString)
                {
                    try
                    {
                        var jArray = Newtonsoft.Json.Linq.JArray.Parse(jsonString);
                        cacheValue = jArray;
                        _logger?.LogDebug($"已解析JSON字符串为JArray类型，长度: {jArray.Count}");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, $"解析JSON字符串失败: {ex.Message}");
                        return;
                    }
                }

                // 处理JArray类型数据（可能来自socket传输）
                if (cacheValue is Newtonsoft.Json.Linq.JArray jArrayData)
                {
                    // 获取实体类型
                    if (entityType != null)
                    {
                        try
                        {
                            // 将JArray转换为具体类型的列表
                            var typedList = jArrayData.ToObject(typeof(List<>).MakeGenericType(entityType));
                            if (typedList != null)
                            {
                                cacheValue = typedList;
                                _logger?.LogDebug($"已将JArray转换为类型 {entityType.Name} 的列表");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, $"转换JArray到类型 {entityType.Name} 时发生错误，将保持JArray格式");
                            // 转换失败时保持JArray格式，以便GetEntityList方法可以处理
                        }
                    }
                }

                // 如果是List<ExpandoObject>，尝试转换为具体类型
                if (cacheValue is List<System.Dynamic.ExpandoObject> expandoList)
                {
                    // 获取实体类型

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

                PutToCache(cacheKey, cacheValue, "List", tableName);

                // 更新缓存同步元数据
                UpdateCacheSyncMetadataAfterEntityChange(tableName);

                _logger?.Debug($"已更新表 {tableName} 的实体列表缓存，数据类型: {cacheValue.GetType().Name}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新表 {tableName} 的实体列表缓存时发生错误");
            }
        }

        /// <summary>
        /// 根据表名更新列表缓存中的单个实体（智能过滤，只处理需要缓存的表）
        /// 注意：现在只更新列表缓存，不再更新单个实体和显示值缓存
        /// </summary>
        public void UpdateEntity(string tableName, object entity)
        {
            if (entity == null)
            {
                return;
            }

            // 使用封装的智能过滤方法
            if (!CheckAndLogIfTableCacheable(tableName))
            {
                return;
            }

            // 直接更新列表缓存中的该实体
            UpdateEntityInList(tableName, entity);

            _logger?.LogDebug($"已更新表 {tableName} 列表缓存中的单个实体");

            // 更新缓存同步元数据
            UpdateCacheSyncMetadataAfterEntityChange(tableName);
        }

        /// <summary>
        /// 检查并记录表是否可缓存，封装智能过滤逻辑
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表是否可缓存</returns>
        private bool CheckAndLogIfTableCacheable(string tableName)
        {
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过更新操作");
                return false;
            }
            return true;
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
        /// 从列表缓存中删除多个实体（智能过滤，只处理需要缓存的表）
        /// 注意：现在只从列表缓存中删除，不再删除单个实体和显示值缓存
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

            // 从列表缓存中删除这些实体
            RemoveEntitiesFromList(tableName, entities);
            _logger?.Debug($"已从表 {tableName} 的列表缓存中删除 {entities.Count} 个实体");
        }

        /// <summary>
        /// 删除指定表的整个实体列表缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public void DeleteEntityList(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");
            }

            // 智能过滤：只处理需要缓存的表
            if (!IsTableCacheable(tableName))
            {
                _logger?.LogDebug($"表 {tableName} 不需要缓存，跳过删除操作");
                return;
            }

            // 生成缓存键并直接删除缓存
            var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
            RemoveFromCache(cacheKey);

            // 更新缓存同步元数据
            UpdateCacheSyncMetadataAfterEntityChange(tableName);
            _logger?.Debug($"已删除表 {tableName} 的整个列表缓存");
        }

        /// <summary>
        /// 从列表缓存中删除指定主键的实体（智能过滤，只处理需要缓存的表）
        /// 注意：现在只从列表缓存中删除，不再删除单个实体和显示值缓存
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

                // 从列表缓存中删除该实体
                RemoveEntityFromList(tableName, primaryKeyValue);

                _logger?.Debug($"已从表 {tableName} 的列表缓存中删除ID为 {primaryKeyValue} 的实体");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从表 {tableName} 的列表缓存中删除ID为 {primaryKeyValue} 的实体时发生错误");
            }
        }

        /// <summary>
        /// 更新实体变更后的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        private void UpdateCacheSyncMetadataAfterEntityChange(string tableName)
        {
            if (_cacheSyncMetadata == null) return;

            try
            {
                var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
                var cachedList = _cacheManager.Get(cacheKey);

                if (cachedList != null)
                {
                    // 使用专门的方法计算数据数量和大小
                    int dataCount = CalculateDataCount(cachedList);
                    long estimatedSize = EstimateObjectSize(cachedList);

                    _cacheSyncMetadata.UpdateTableSyncInfo(tableName, dataCount, estimatedSize);
                    _logger?.LogDebug($"已更新表 {tableName} 的缓存同步元数据，数据数量: {dataCount}");
                }
                else
                {
                    // 如果列表不存在，则移除元数据
                    _cacheSyncMetadata.RemoveTableSyncInfo(tableName);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新表 {tableName} 的缓存同步元数据时发生错误");
            }
        }

        /// <summary>
        /// 计算缓存对象中的数据数量
        /// </summary>
        /// <param name="cacheObject">缓存对象</param>
        /// <returns>数据数量</returns>
        private int CalculateDataCount(object cacheObject)
        {
            if (cacheObject == null) return 0;

            if (cacheObject is ICollection collection)
            {
                return collection.Count;
            }
            else if (cacheObject is Newtonsoft.Json.Linq.JArray jArray)
            {
                return jArray.Count;
            }

            return 0;
        }

        private void RemoveEntityFromList(string tableName, object primaryKeyValue)
        {
            try
            {
                var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
                var cachedList = _cacheManager.Get(cacheKey);

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
                            PutToCache(cacheKey, list, "List", tableName);

                            // 更新缓存同步元数据
                            UpdateCacheSyncMetadataAfterEntityChange(tableName);
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
                var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
                var cachedList = _cacheManager.Get(cacheKey);

                if (cachedList is List<T> list)
                {
                    var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                    if (schemaInfo != null)
                    {
                        var entityIds = entities.Select(e => e.GetCachePropertyValue(schemaInfo.PrimaryKeyField)?.ToString())
                                                .Where(id => id != null)
                                                .ToHashSet();

                        // 获取原始列表大小用于比较
                        int originalCount = list.Count;

                        list.RemoveAll(e =>
                        {
                            var idPropertyValue = e.GetCachePropertyValue(schemaInfo.PrimaryKeyField);
                            return idPropertyValue != null && entityIds.Contains(idPropertyValue.ToString());
                        });

                        PutToCache(cacheKey, list, "List", tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从表 {tableName} 列表缓存中删除多个实体时发生错误");
            }
        }



        /// <summary>
        /// 从列表缓存中批量删除指定主键数组的实体（智能过滤，只处理需要缓存的表）
        /// 注意：现在只从列表缓存中删除，不再删除单个实体和显示值缓存
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
        /// 从列表缓存中批量删除指定主键数组的实体（智能过滤，只处理需要缓存的表）
        /// 注意：现在只从列表缓存中删除，不再删除单个实体和显示值缓存
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

                // 从列表缓存中批量删除这些实体
                RemoveEntitiesFromListByKeys(tableName, primaryKeyValues);

                _logger?.Debug($"已从表 {tableName} 的列表缓存中批量删除 {primaryKeyValues.Length} 个实体");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从表 {tableName} 的列表缓存中批量删除实体时发生错误");
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
                var cacheKey = GenerateCacheKey(CacheKeyType.List, tableName);
                var cachedList = _cacheManager.Get(cacheKey);

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
                            var idPropertyValue = item.GetCachePropertyValue(schemaInfo.PrimaryKeyField)?.ToString();
                            if (idPropertyValue == null || !primaryKeyStrings.Contains(idPropertyValue))
                            {
                                entitiesToKeep.Add(item);
                            }
                        }

                        // 重新设置列表缓存
                        PutToCache(cacheKey, entitiesToKeep, "List", tableName);
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
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">主显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
        /// <param name="otherDisplayFieldExpressions">其他需要缓存的显示字段表达式</param>
        public void InitializeTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params Expression<Func<T, object>>[] otherDisplayFieldExpressions) where T : class
        {
            _tableSchemaManager.RegisterTableSchema(
                primaryKeyExpression,
                displayFieldExpression,
                isView,
                isCacheable,
                description,
                cacheWholeRow,
                otherDisplayFieldExpressions);
        }

        /// <summary>
        /// 获取实体类型
        /// </summary>
        public Type GetEntityType(string tableName)
        {
            return _tableSchemaManager.GetEntityType(tableName);
        }



        #endregion

        #region 缓存统计和监控实现
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public long CacheHits => _cacheHits;

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public long CacheMisses => _cacheMisses;

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public double HitRatio
        {
            get
            {
                long total = _cacheHits + _cacheMisses;
                return total == 0 ? 0 : (double)_cacheHits / total;
            }
        }

        /// <summary>
        /// 缓存写入次数
        /// </summary>
        public long CachePuts => _cachePuts;

        /// <summary>
        /// 缓存删除次数
        /// </summary>
        public long CacheRemoves => _cacheRemoves;

        /// <summary>
        /// 缓存项总数
        /// </summary>
        public int CacheItemCount
        {
            get
            {
                lock (_statisticsLock)
                {
                    return _cacheItemStatistics.Count;
                }
            }
        }

        /// <summary>
        /// 缓存大小（估计值，单位：字节）
        /// </summary>
        public long EstimatedCacheSize
        {
            get
            {
                lock (_statisticsLock)
                {
                    return _cacheItemStatistics.Values.Sum(s => s.EstimatedSize);
                }
            }
        }

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public void ResetStatistics()
        {
            lock (_statisticsLock)
            {
                _cacheHits = 0;
                _cacheMisses = 0;
                _cachePuts = 0;
                _cacheRemoves = 0;
                _cacheItemStatistics.Clear();
                _logger.Debug("缓存统计信息已重置");
            }
        }

        /// <summary>
        /// 获取缓存项统计详情
        /// </summary>
        public Dictionary<string, CacheItemStatistics> GetCacheItemStatistics()
        {
            lock (_statisticsLock)
            {
                return new Dictionary<string, CacheItemStatistics>(_cacheItemStatistics);
            }
        }

        /// <summary>
        /// 获取按表名分组的缓存统计
        /// </summary>
        public Dictionary<string, TableCacheStatistics> GetTableCacheStatistics()
        {
            lock (_statisticsLock)
            {
                var tableStats = new Dictionary<string, TableCacheStatistics>();

                // 按表名和缓存类型分组统计
                foreach (var stats in _cacheItemStatistics.Values)
                {
                    if (!tableStats.ContainsKey(stats.TableName))
                    {
                        tableStats[stats.TableName] = new TableCacheStatistics
                        {
                            TableName = stats.TableName
                        };
                    }

                    var tableStat = tableStats[stats.TableName];
                    tableStat.EstimatedTotalSize += stats.EstimatedSize;

                    switch (stats.CacheType)
                    {
                        case "EntityList":
                            tableStat.EntityListCount++;
                            break;
                        case "Entity":
                            tableStat.EntityCount++;
                            break;
                        case "DisplayValue":
                            tableStat.DisplayValueCount++;
                            break;
                    }
                }

                return tableStats;
            }
        }

        /// <summary>
        /// 更新缓存访问统计
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="isHit">是否命中</param>
        /// <param name="cacheType">缓存类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="value">缓存值</param>
        private void UpdateCacheAccessStatistics(string cacheKey, bool isHit, string cacheType, string tableName, object value)
        {
            lock (_statisticsLock)
            {
                if (isHit)
                {
                    _cacheHits++;
                    if (_cacheItemStatistics.ContainsKey(cacheKey))
                    {
                        var stats = _cacheItemStatistics[cacheKey];
                        stats.LastAccessedTime = DateTime.Now;
                        stats.AccessCount++;
                    }
                }
                else
                {
                    _cacheMisses++;
                }
            }
        }

        /// <summary>
        /// 向缓存中添加项（带统计和大小检查）
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="cacheType">缓存类型</param>
        /// <param name="tableName">表名</param>
        private void PutToCache(string cacheKey, object value, string cacheType, string tableName)
        {
            if (value == null)
                return;

            // 检查缓存大小并在必要时进行清理
            CheckAndCleanCacheSize();

            // 估计缓存项大小
            long estimatedSize = EstimateObjectSize(value);

            // 检查是否存在覆盖情况：原列表有数据，新列表数据更少或为空
            bool isReplacingDataWithLessOrEmpty = false;
            try
            {
                // 尝试获取现有缓存值
                if (_cacheManager.Exists(cacheKey))
                {
                    var existingValue = _cacheManager.Get(cacheKey);
                    if (existingValue != null)
                    {
                        // 检查现有值是否为列表且有数据
                        var existingList = existingValue as System.Collections.IList;
                        var newList = value as System.Collections.IList;

                        if (existingList != null && newList != null)
                        {
                            // 当原列表有数据，而新列表数据更少或为空时记录
                            if (existingList.Count > 0 && newList.Count <= existingList.Count)
                            {
                                isReplacingDataWithLessOrEmpty = true;
                                // 这里可以添加断点或日志记录，帮助调试
                                System.Diagnostics.Debug.WriteLine($"[缓存覆盖警告] 表 {tableName} - 缓存键: {cacheKey} 原数据行数: {existingList.Count}, 新数据行数: {newList.Count}");
                                // 可以在这里设置条件断点
                                // System.Diagnostics.Debugger.Break(); // 如果需要强制中断调试，可以取消注释这行
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录调试检查过程中的异常，但不影响主要缓存操作
                System.Diagnostics.Debug.WriteLine($"[缓存调试检查错误] {ex.Message}");
            }

            lock (_statisticsLock)
            {
                // 更新缓存统计
                _cachePuts++;

                // 更新或创建缓存项统计信息
                if (!_cacheItemStatistics.ContainsKey(cacheKey))
                {
                    _cacheItemStatistics[cacheKey] = new CacheItemStatistics
                    {
                        Key = cacheKey,
                        CacheType = cacheType,
                        TableName = tableName,
                        CreatedTime = DateTime.Now,
                        LastAccessedTime = DateTime.Now,
                        AccessCount = 0,
                        EstimatedSize = estimatedSize,
                        ValueType = value.GetType().FullName
                    };
                }
                else
                {
                    var stats = _cacheItemStatistics[cacheKey];
                    stats.LastAccessedTime = DateTime.Now;
                    stats.EstimatedSize = estimatedSize;
                    stats.ValueType = value.GetType().FullName;
                }
            }

            // 如果检测到数据覆盖问题，记录详细日志
            if (isReplacingDataWithLessOrEmpty)
            {
                System.Diagnostics.Debug.WriteLine($"[缓存覆盖操作] 正在执行表 {tableName} 的缓存更新，可能导致数据减少");
            }

            // 执行实际的缓存操作
            _cacheManager.Put(cacheKey, value);
        }

        /// <summary>
        /// 从缓存中移除项（带统计）
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        private void RemoveFromCache(string cacheKey)
        {
            lock (_statisticsLock)
            {
                _cacheRemoves++;
                if (_cacheItemStatistics.ContainsKey(cacheKey))
                {
                    _cacheItemStatistics.Remove(cacheKey);
                }
            }

            _cacheManager.Remove(cacheKey);
        }



        /// <summary>
        /// 检查缓存大小并在必要时进行清理
        /// </summary>
        private void CheckAndCleanCacheSize()
        {
            long currentSize = EstimatedCacheSize;

            // 如果缓存大小超过阈值，则进行清理
            if (currentSize >= _cacheSizeThreshold)
            {
                _logger.LogWarning("缓存大小超过阈值，触发自动清理机制。当前大小：{CurrentSize}MB，阈值：{Threshold}MB",
                    currentSize / (1024 * 1024), _cacheSizeThreshold / (1024 * 1024));

                // 执行缓存清理（移除最少使用的项目）
                CleanCacheByLeastRecentlyUsed();

                _logger.Debug("缓存清理完成。清理后大小：{NewSize}MB", EstimatedCacheSize / (1024 * 1024));
            }
        }

        /// <summary>
        /// 按最少使用原则清理缓存
        /// </summary>
        private void CleanCacheByLeastRecentlyUsed()
        {
            lock (_statisticsLock)
            {
                // 获取所有缓存项并按最后访问时间排序（最久未使用的在前）
                var itemsToRemove = _cacheItemStatistics.Values
                    .OrderBy(s => s.LastAccessedTime)
                    .Take(_cacheItemStatistics.Count / 10) // 减少为移除10%的项，降低清理频率
                    .Select(s => s.Key)
                    .ToList();

                // 记录详细的缓存清理日志
                if (itemsToRemove.Any())
                {
                    _logger.LogWarning("缓存清理执行：移除了{Count}个最少使用的缓存项，包括表：{Tables}",
                        itemsToRemove.Count,
                        string.Join(", ", itemsToRemove.Select(k => _cacheItemStatistics[k].TableName)));
                }

                // 移除选定的缓存项
                foreach (var key in itemsToRemove)
                {
                    _cacheItemStatistics.Remove(key);
                    _cacheManager.Remove(key);
                    _cacheRemoves++;
                }
            }
        }

        /// <summary>
        /// 估计对象大小（字节）
        /// </summary>
        /// <param name="obj">要估计大小的对象</param>
        /// <returns>估计的字节大小</returns>
        private long EstimateObjectSize(object obj)
        {
            if (obj == null)
                return 0;

            try
            {
                // 使用JSON序列化来粗略估计对象大小
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                return Encoding.UTF8.GetByteCount(json);
            }
            catch
            {
                // 如果序列化失败，返回一个保守估计值
                return 1024; // 1KB
            }
        }
        #endregion
    }
}