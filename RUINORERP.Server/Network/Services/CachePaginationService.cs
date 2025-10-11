using CacheManager.Core;
using MessagePack;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Network.Services
{
    public class CachePaginationService
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger<CachePaginationService> _logger;
        private readonly ICacheManager<object> _cacheManager;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _cacheLocks;

        public CachePaginationService(
            ISqlSugarClient db,
            ILogger<CachePaginationService> logger,
            ICacheManager<object> cacheManager)
        {
            _db = db;
            _logger = logger;
            _cacheManager = cacheManager;
            _cacheLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        /// <summary>
        /// 获取分页缓存数据（支持强类型）
        /// </summary>
        public async Task<PagedCacheData<T>> GetPagedCacheDataAsync<T>(
            string tableName,
            int pageIndex = 0,
            int pageSize = 1000,
            Dictionary<string, object> filterConditions = null,
            string orderBy = null,
            bool descending = false,
            bool forceRefresh = false,
            CancellationToken cancellationToken = default)
            where T : class, new()
        {
            var cacheKey = $"{tableName}_paged_{typeof(T).Name}";
            var cacheLock = _cacheLocks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));

            await cacheLock.WaitAsync(cancellationToken);
            try
            {
                // 1. 检查缓存有效性
                if (!forceRefresh && await IsCacheValidAsync(tableName))
                {
                    var cachedData = await GetCachedPagedDataAsync<T>(tableName, pageIndex, pageSize,
                        filterConditions, orderBy, descending);
                    if (cachedData != null)
                    {
                        _logger.LogDebug("从缓存获取分页数据: {TableName}, 页码: {PageIndex}", tableName, pageIndex);
                        return cachedData;
                    }
                }

                // 2. 缓存无效或强制刷新，从数据库获取
                _logger.LogInformation("缓存无效，从数据库加载分页数据: {TableName}", tableName);
                return await LoadPagedDataFromDatabaseAsync<T>(tableName, pageIndex, pageSize,
                    filterConditions, orderBy, descending, cancellationToken);
            }
            finally
            {
                cacheLock.Release();
            }
        }

        /// <summary>
        /// 检查缓存有效性
        /// </summary>
        private async Task<bool> IsCacheValidAsync(string tableName)
        {
            try
            {
                var cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(tableName) as CacheInfo;
                if (cacheInfo == null)
                    return false;

                // 检查缓存是否过期（默认1小时）
                var cacheAge = DateTime.Now - cacheInfo.LastUpdateTime;
                return cacheAge.TotalHours < 1;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "检查缓存有效性失败: {TableName}", tableName);
                return false;
            }
        }

        /// <summary>
        /// 从缓存获取分页数据
        /// </summary>
        private async Task<PagedCacheData<T>> GetCachedPagedDataAsync<T>(
            string tableName,
            int pageIndex,
            int pageSize,
            Dictionary<string, object> filterConditions,
            string orderBy,
            bool descending)
            where T : class, new()
        {
            try
            {
                // 1. 获取完整的缓存数据
                var cacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                if (cacheList == null)
                    return null;

                // 2. 转换为强类型列表
                var typedList = ConvertToTypedList<T>(cacheList);
                if (typedList == null || !typedList.Any())
                    return null;

                // 3. 应用过滤条件
                var filteredList = ApplyFilters(typedList, filterConditions);

                // 4. 应用排序
                var sortedList = ApplySorting(filteredList, orderBy, descending);

                // 5. 应用分页
                var totalCount = sortedList.Count;
                var pagedData = sortedList
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 6. 构建响应
                return new PagedCacheData<T>
                {
                    CacheData = new CacheData<T>
                    {
                        TableName = tableName,
                        Data = pagedData,
                        CacheTime = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddHours(1),
                        Version = await GetCacheVersionAsync(tableName),
                        HasMoreData = totalCount > (pageIndex + 1) * pageSize
                    },
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从缓存获取分页数据失败: {TableName}", tableName);
                return null;
            }
        }

        /// <summary>
        /// 从数据库加载分页数据
        /// </summary>
        private async Task<PagedCacheData<T>> LoadPagedDataFromDatabaseAsync<T>(
            string tableName,
            int pageIndex,
            int pageSize,
            Dictionary<string, object> filterConditions,
            string orderBy,
            bool descending,
            CancellationToken cancellationToken)
            where T : class, new()
        {
            try
            {
                // 1. 构建查询条件
                var query = _db.Queryable<T>();

                // 2. 应用过滤条件
                query = ApplyDatabaseFilters(query, filterConditions);

                // 3. 应用排序
                query = ApplyDatabaseSorting(query, orderBy, descending);

                // 4. 执行分页查询
                var refTotal = 0;
                var pagedResult = await query.ToPageListAsync(pageIndex + 1, pageSize, refTotal);

                // 5. 更新缓存
                await UpdateCacheAsync<T>(tableName, pagedResult);

                // 6. 构建响应
                return new PagedCacheData<T>
                {
                    CacheData = new CacheData<T>
                    {
                        TableName = tableName,
                        Data = pagedResult,
                        CacheTime = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddHours(1),
                        Version = await GetCacheVersionAsync(tableName),
                        HasMoreData = refTotal > (pageIndex + 1) * pageSize
                    },
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = refTotal,
                    TotalPages = (int)Math.Ceiling((double)refTotal / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从数据库加载分页数据失败: {TableName}", tableName);
                throw new DataAccessException($"加载{tableName}数据失败", ex);
            }
        }

        /// <summary>
        /// 转换为强类型列表
        /// </summary>
        private List<T> ConvertToTypedList<T>(object cacheData) where T : class, new()
        {
            if (cacheData == null) return new List<T>();

            try
            {
                // 方法1: 直接类型转换
                if (cacheData is List<T> typedList)
                    return typedList;

                // 方法2: JArray转换
                if (cacheData is JArray jArray)
                    return jArray.ToObject<List<T>>();

                // 方法3: 动态转换
                if (cacheData is IEnumerable<dynamic> dynamicEnumerable)
                    return dynamicEnumerable.Select(x => ConvertDynamicToType<T>(x)).Cast<T>().ToList();

                // 方法4: 反射转换
                if (cacheData is IList list)
                {
                    var result = new List<T>();
                    foreach (var item in list)
                    {
                        if (item is T typedItem)
                        {
                            result.Add(typedItem);
                        }
                        else
                        {
                            var convertedItem = ConvertToType<T>(item);
                            if (convertedItem != null)
                                result.Add(convertedItem);
                        }
                    }
                    return result;
                }

                // 方法5: JSON序列化转换
                var json = JsonConvert.SerializeObject(cacheData);
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "转换缓存数据到类型 {TypeName} 失败", typeof(T).Name);
                return new List<T>();
            }
        }

        /// <summary>
        /// 应用内存过滤条件
        /// </summary>
        private List<T> ApplyFilters<T>(List<T> data, Dictionary<string, object> filterConditions)
        {
            if (filterConditions == null || !filterConditions.Any())
                return data;

            try
            {
                return data.Where(item =>
                {
                    foreach (var condition in filterConditions)
                    {
                        var property = typeof(T).GetProperty(condition.Key);
                        if (property == null) continue;

                        var value = property.GetValue(item);
                        if (!Equals(value, condition.Value))
                            return false;
                    }
                    return true;
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "应用内存过滤条件失败，返回原始数据");
                return data;
            }
        }

        /// <summary>
        /// 应用内存排序
        /// </summary>
        private List<T> ApplySorting<T>(List<T> data, string orderBy, bool descending)
        {
            if (string.IsNullOrEmpty(orderBy))
                return data;

            try
            {
                var property = typeof(T).GetProperty(orderBy);
                if (property == null)
                    return data;

                if (descending)
                    return data.OrderByDescending(x => property.GetValue(x)).ToList();
                else
                    return data.OrderBy(x => property.GetValue(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "应用内存排序失败，返回原始数据");
                return data;
            }
        }

        /// <summary>
        /// 应用数据库过滤条件
        /// </summary>
        private ISugarQueryable<T> ApplyDatabaseFilters<T>(
            ISugarQueryable<T> query,
            Dictionary<string, object> filterConditions)
            where T : class, new()
        {
            if (filterConditions == null || !filterConditions.Any())
                return query;

            try
            {
                foreach (var condition in filterConditions)
                {
                    query = query.Where($"{condition.Key} = @0", condition.Value);
                }
                return query;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "应用数据库过滤条件失败");
                return query;
            }
        }

        /// <summary>
        /// 应用数据库排序
        /// </summary>
        private ISugarQueryable<T> ApplyDatabaseSorting<T>(
            ISugarQueryable<T> query,
            string orderBy,
            bool descending)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(orderBy))
                return query;

            try
            {
                return descending ?
                    query.OrderBy($"{orderBy} DESC") :
                    query.OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "应用数据库排序失败");
                return query;
            }
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        private async Task UpdateCacheAsync<T>(string tableName, List<T> data)
        {
            try
            {
                // 更新实体缓存
                MyCacheManager.Instance.UpdateEntityList(tableName, data);

                // 更新缓存信息
                var cacheInfo = new CacheInfo
                {
                    TableName = tableName,
                    LastUpdateTime = DateTime.Now,
                    DataCount = data.Count,
                    MemorySize = EstimateMemorySize(data)
                };
                MyCacheManager.Instance.CacheInfoList.Put(tableName, cacheInfo);

                // 记录缓存更新日志
                _logger.LogInformation("缓存更新成功: {TableName}, 数据量: {Count}", tableName, data.Count);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新缓存失败: {TableName}", tableName);
            }
        }

        /// <summary>
        /// 获取缓存版本
        /// </summary>
        private async Task<string> GetCacheVersionAsync(string tableName)
        {
            try
            {
                var cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(tableName) as CacheInfo;
                return cacheInfo?.LastUpdateTime.Ticks.ToString() ?? DateTime.Now.Ticks.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取缓存版本失败: {TableName}", tableName);
                return DateTime.Now.Ticks.ToString();
            }
        }

        /// <summary>
        /// 类型转换辅助方法
        /// </summary>
        private T ConvertToType<T>(object source) where T : class, new()
        {
            if (source == null) return default(T);
            if (source is T typed) return typed;

            try
            {
                // 方法1: MessagePack转换
                var bytes = MessagePackSerializer.Serialize(source);
                return MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch
            {
                try
                {
                    // 方法2: JSON转换
                    var json = JsonConvert.SerializeObject(source);
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "类型转换失败: {SourceType} -> {TargetType}",
                        source.GetType().Name, typeof(T).Name);
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 动态对象转换
        /// </summary>
        private T ConvertDynamicToType<T>(dynamic source) where T : class, new()
        {
            if (source == null) return default(T);

            try
            {
                var result = new T();
                var properties = typeof(T).GetProperties();

                foreach (var prop in properties)
                {
                    try
                    {
                        var value = GetDynamicValue(source, prop.Name);
                        if (value != null)
                        {
                            var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                            prop.SetValue(result, convertedValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "设置属性 {PropertyName} 失败", prop.Name);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "动态对象转换失败");
                return default(T);
            }
        }

        /// <summary>
        /// 获取动态对象属性值
        /// </summary>
        private object GetDynamicValue(dynamic obj, string propertyName)
        {
            try
            {
                if (obj is IDictionary<string, object> dict)
                {
                    return dict.ContainsKey(propertyName) ? dict[propertyName] : null;
                }

                if (obj is ExpandoObject expando)
                {
                    return ((IDictionary<string, object>)expando).ContainsKey(propertyName) ?
                        ((IDictionary<string, object>)expando)[propertyName] : null;
                }

                return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 估算内存大小
        /// </summary>
        private long EstimateMemorySize<T>(List<T> data)
        {
            try
            {
                if (data == null || !data.Any()) return 0;

                // 简单估算：对象数量 × 平均对象大小（假设1KB）
                return data.Count * 1024;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 批量获取多个表的分页缓存数据
        /// </summary>
        public async Task<Dictionary<string, PagedCacheData<T>>> GetMultiplePagedCachesAsync<T>(
            List<string> tableNames,
            int pageIndex = 0,
            int pageSize = 1000,
            CancellationToken cancellationToken = default)
            where T : class, new()
        {
            var tasks = tableNames.ToDictionary(
                tableName => tableName,
                tableName => GetPagedCacheDataAsync<T>(tableName, pageIndex, pageSize,
                    cancellationToken: cancellationToken));

            await Task.WhenAll(tasks.Values);

            return tasks.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Result);
        }

        /// <summary>
        /// 预加载分页缓存（性能优化）
        /// </summary>
        public async Task PreloadPagedCacheAsync<T>(
            string tableName,
            int totalPagesToPreload = 5,
            int pageSize = 1000,
            CancellationToken cancellationToken = default)
            where T : class, new()
        {
            try
            {
                var tasks = new List<Task>();

                for (int i = 0; i < totalPagesToPreload; i++)
                {
                    tasks.Add(GetPagedCacheDataAsync<T>(tableName, i, pageSize,
                        cancellationToken: cancellationToken));
                }

                await Task.WhenAll(tasks);
                _logger.LogInformation("预加载分页缓存完成: {TableName}, 页数: {Pages}",
                    tableName, totalPagesToPreload);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "预加载分页缓存失败: {TableName}", tableName);
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public async Task<CachePaginationStats> GetCacheStatsAsync()
        {
            var stats = new CachePaginationStats
            {
                TotalCachedTables = BizCacheHelper.Manager.NewTableList.Count,
                CacheLocksCount = _cacheLocks.Count,
                LastUpdateTime = DateTime.Now
            };

            foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
            {
                var cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(tableName) as CacheInfo;
                if (cacheInfo != null)
                {
                    stats.TotalCachedRecords += cacheInfo.DataCount;
                    stats.TotalMemoryUsage += cacheInfo.MemorySize;
                }
            }

            return await Task.FromResult(stats);
        }
    }

    /// <summary>
    /// 缓存分页统计信息
    /// </summary>
    public class CachePaginationStats
    {
        public int TotalCachedTables { get; set; }
        public long TotalCachedRecords { get; set; }
        public long TotalMemoryUsage { get; set; }
        public int CacheLocksCount { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

    /// <summary>
    /// 数据访问异常
    /// </summary>
    public class DataAccessException : Exception
    {
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
