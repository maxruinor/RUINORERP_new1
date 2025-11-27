using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using RUINORERP.PacketSpec.Models.Cache;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存同步扩展类
    /// 为IEntityCacheManager提供缓存同步相关的扩展方法
    /// 整合了旧版CacheInfo的功能，使其与新的缓存架构兼容
    /// 提供便捷的缓存同步操作和基础表缓存管理功能
    /// </summary>
    public static class CacheSyncExtensions
    {
        private static ICacheSyncMetadata _defaultSyncMetadata;

        private static IServiceProvider _serviceProvider;

        public static void AddCacheSyncMetaSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            _serviceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        /// 默认缓存同步元数据管理器
        /// 在没有显式提供的情况下使用
        /// </summary>
        public static ICacheSyncMetadata DefaultSyncMetadata
        {
            get
            {
                if (_defaultSyncMetadata == null)
                {
                    // 尝试从依赖注入容器获取实例
                    try
                    {
                        _defaultSyncMetadata = _serviceProvider.GetRequiredService<ICacheSyncMetadata>();
                    }
                    catch (Exception)
                    {
                        // 记录异常并使用后备策略
                      //  _logger?.LogWarning(ex, "无法从依赖注入容器获取ICacheSyncMetadata实例");
                        // 发生异常时创建新实例
                        _defaultSyncMetadata = new CacheSyncMetadataManager();
                    }
                }
                return _defaultSyncMetadata;
            }
            set => _defaultSyncMetadata = value;
        }

        /// <summary>
        /// 更新实体列表缓存并记录同步元数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="list">实体列表</param>
        /// <param name="syncMetadata">缓存同步元数据管理器（可选）</param>
        public static void UpdateEntityListWithSync<T>(this IEntityCacheManager cacheManager, List<T> list, ICacheSyncMetadata syncMetadata = null)
            where T : class
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            // 使用提供的同步元数据管理器或默认的
            syncMetadata = syncMetadata ?? DefaultSyncMetadata;

            var tableName = typeof(T).Name;

            // 更新缓存
            cacheManager.UpdateEntityList(list);

            try
            {
                // 计算数据数量和估计大小
                int dataCount = list?.Count ?? 0;
                long estimatedSize = EstimateObjectSize(list);

                // 创建并更新同步信息
                var syncInfo = new CacheSyncInfo
                {
                    TableName = tableName,
                    DataCount = dataCount,
                    EstimatedSize = estimatedSize,
                    LastUpdateTime = DateTime.Now,
                    // 使用默认的过期时间（可根据配置调整）
                    ExpirationTime = DateTime.Now.AddHours(2)
                };

                syncMetadata.UpdateTableSyncInfo(tableName, dataCount, estimatedSize);
            }
            catch (Exception ex)
            {
                // 记录异常但不中断主流程
                System.Diagnostics.Debug.WriteLine($"更新表 {tableName} 的缓存同步元数据时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据表名更新缓存并记录同步元数据
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="list">实体列表</param>
        /// <param name="syncMetadata">缓存同步元数据管理器（可选）</param>
        public static void UpdateEntityListWithSync(this IEntityCacheManager cacheManager, string tableName, object list, ICacheSyncMetadata syncMetadata = null)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            // 使用提供的同步元数据管理器或默认的
            syncMetadata = syncMetadata ?? DefaultSyncMetadata;

            // 更新缓存
            cacheManager.UpdateEntityList(tableName, list);

            try
            {
                // 计算数据数量和估计大小
                int dataCount = GetCollectionCount(list);
                long estimatedSize = EstimateObjectSize(list);

                // 创建并更新同步信息
                var syncInfo = new CacheSyncInfo
                {
                    TableName = tableName,
                    DataCount = dataCount,
                    EstimatedSize = estimatedSize,
                    LastUpdateTime = DateTime.Now,
                    // 使用默认的过期时间（可根据配置调整）
                    ExpirationTime = DateTime.Now.AddHours(2)
                };

                syncMetadata.UpdateTableSyncInfo(tableName, dataCount, estimatedSize);
            }
            catch (Exception ex)
            {
                // 记录异常但不中断主流程
                System.Diagnostics.Debug.WriteLine($"更新表 {tableName} 的缓存同步元数据时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否需要同步表缓存
        /// 基于缓存同步元数据决定是否需要从服务器获取最新数据
        /// 整合了旧版CacheInfo的NeedRequesCache功能
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="syncMetadata">缓存同步元数据管理器（可选）</param>
        /// <returns>如果需要同步返回true</returns>
        public static bool NeedsCacheSync(this IEntityCacheManager cacheManager, string tableName, ICacheSyncMetadata syncMetadata = null)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            // 使用提供的同步元数据管理器或默认的
            syncMetadata = syncMetadata ?? DefaultSyncMetadata;

            try
            {
                // 检查缓存同步元数据是否存在
                var syncInfo = syncMetadata.GetTableSyncInfo(tableName);
                if (syncInfo == null)
                {
                    // 没有同步元数据，需要同步
                    return true;
                }

                // 检查是否已过期
                if (syncInfo.NeedsSync())
                {
                    // 已过期，需要同步
                    return true;
                }

                // 检查本地缓存中的数据数量是否与同步元数据一致
                // 注意：这可能会触发缓存加载，在高频调用场景中需要优化
                var entityList = cacheManager.GetEntityList<object>(tableName);
                int localCount = GetCollectionCount(entityList);

                // 如果本地缓存数量与同步元数据记录的数量不同，则需要同步
                return localCount != syncInfo.DataCount;
            }
            catch (Exception ex)
            {
                // 记录异常并默认需要同步
                System.Diagnostics.Debug.WriteLine($"检查表 {tableName} 是否需要同步时发生错误: {ex.Message}");
                return true;
            }
        }

        /// <summary>
        /// 从旧版CacheInfo转换为新版CacheSyncInfo
        /// 用于兼容旧的缓存信息系统
        /// </summary>
        /// <param name="cacheInfo">旧版CacheInfo对象</param>
        /// <returns>新版CacheSyncInfo对象</returns>
        public static CacheSyncInfo ConvertToSyncInfo(this RUINORERP.Model.CommonModel.CacheInfo cacheInfo)
        {
            if (cacheInfo == null)
                return null;

            return new CacheSyncInfo
            {
                TableName = cacheInfo.TableName,
                DataCount = cacheInfo.DataCount,
                EstimatedSize = cacheInfo.MemorySize,
                LastUpdateTime = cacheInfo.LastUpdateTime,
                ExpirationTime = cacheInfo.HasExpire ? cacheInfo.ExpirationTime : DateTime.MaxValue,
                // 从CacheName中提取其他有用信息（如果需要）
                SourceInfo = cacheInfo.CacheName
            };
        }

        /// <summary>
        /// 从新版CacheSyncInfo转换为旧版CacheInfo
        /// 用于向后兼容
        /// </summary>
        /// <param name="syncInfo">新版CacheSyncInfo对象</param>
        /// <returns>旧版CacheInfo对象</returns>
        public static RUINORERP.Model.CommonModel.CacheInfo ConvertToCacheInfo(this CacheSyncInfo syncInfo)
        {
            if (syncInfo == null)
                return null;

            return new RUINORERP.Model.CommonModel.CacheInfo
            {
                TableName = syncInfo.TableName,
                DataCount = syncInfo.DataCount,
                MemorySize = syncInfo.EstimatedSize,
                LastUpdateTime = syncInfo.LastUpdateTime,
                HasExpire = syncInfo.ExpirationTime < DateTime.MaxValue,
                ExpirationTime = syncInfo.ExpirationTime,
                // 使用表名作为缓存名
                CacheName = syncInfo.SourceInfo ?? syncInfo.TableName,
                // 设置CacheCount为DataCount（保持兼容性）
                CacheCount = syncInfo.DataCount
            };
        }

        /// <summary>
        /// 获取表的缓存同步信息
        /// 整合了旧版CacheInfo的功能
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="syncMetadata">缓存同步元数据管理器（可选）</param>
        /// <returns>缓存同步信息</returns>
        public static CacheSyncInfo GetTableSyncInfo(this IEntityCacheManager cacheManager, string tableName, ICacheSyncMetadata syncMetadata = null)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            // 使用提供的同步元数据管理器或默认的
            syncMetadata = syncMetadata ?? DefaultSyncMetadata;

            try
            {
                // 先尝试从同步元数据管理器获取
                var syncInfo = syncMetadata.GetTableSyncInfo(tableName);
                if (syncInfo != null)
                {
                    return syncInfo;
                }

                // 如果没有找到，尝试从缓存中获取实体列表并计算信息
                var entityList = cacheManager.GetEntityList<object>(tableName);
                if (entityList != null)
                {
                    // 创建新的同步信息
                    syncInfo = new CacheSyncInfo
                    {
                        TableName = tableName,
                        DataCount = GetCollectionCount(entityList),
                        EstimatedSize = EstimateObjectSize(entityList),
                        LastUpdateTime = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddHours(2) // 默认过期时间
                    };

                    // 更新到同步元数据管理器
                    syncMetadata.UpdateTableSyncInfo(tableName, syncInfo.DataCount, syncInfo.EstimatedSize);
                    // 设置过期时间
                    syncMetadata.SetTableExpiration(tableName, syncInfo.ExpirationTime);

                    return syncInfo;
                }
            }
            catch (Exception ex)
            {
                // 记录异常但不中断流程
                System.Diagnostics.Debug.WriteLine($"获取表 {tableName} 的缓存同步信息时发生错误: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 设置表的缓存过期时间
        /// 整合了旧版CacheInfo的过期时间设置功能
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="expirationTime">过期时间</param>
        /// <param name="syncMetadata">缓存同步元数据管理器（可选）</param>
        public static void SetTableCacheExpiration(this IEntityCacheManager cacheManager, string tableName, DateTime expirationTime, ICacheSyncMetadata syncMetadata = null)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            // 使用提供的同步元数据管理器或默认的
            syncMetadata = syncMetadata ?? DefaultSyncMetadata;

            try
            {
                // 设置过期时间
                syncMetadata.SetTableExpiration(tableName, expirationTime);
            }
            catch (Exception ex)
            {
                // 记录异常但不中断流程
                System.Diagnostics.Debug.WriteLine($"设置表 {tableName} 的缓存过期时间时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建带过期时间的缓存同步信息
        /// 类似旧版CacheInfo.CreateWithExpiration方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dataCount">数据数量</param>
        /// <param name="estimatedSize">估计大小</param>
        /// <param name="expirationTime">过期时间</param>
        /// <returns>缓存同步信息</returns>
        public static CacheSyncInfo CreateSyncInfoWithExpiration(string tableName, int dataCount, long estimatedSize, DateTime expirationTime)
        {
            return new CacheSyncInfo
            {
                TableName = tableName,
                DataCount = dataCount,
                EstimatedSize = estimatedSize,
                LastUpdateTime = DateTime.Now,
                ExpirationTime = expirationTime
            };
        }

        #region 私有辅助方法

        /// <summary>
        /// 获取集合中元素的数量
        /// </summary>
        /// <param name="collection">集合对象</param>
        /// <returns>元素数量</returns>
        private static int GetCollectionCount(object collection)
        {
            if (collection == null)
                return 0;

            if (collection is ICollection coll) // 实现了ICollection接口的集合
                return coll.Count;

            if (collection is string str) // 字符串特殊处理
                return str.Length;

            if (collection is JArray jArray) // JSON数组特殊处理
                return jArray.Count;

            // 如果是可枚举类型但没有实现ICollection，则遍历计数
            if (collection is IEnumerable enumerable)
            {
                int count = 0;
                foreach (var item in enumerable)
                    count++;
                return count;
            }

            // 单个对象
            return 1;
        }

        /// <summary>
        /// 估计对象的内存大小（字节）
        /// 注意：这是一个粗略的估计，实际内存占用可能有所不同
        /// </summary>
        /// <param name="obj">要估计大小的对象</param>
        /// <returns>估计的内存大小（字节）</returns>
        private static long EstimateObjectSize(object obj)
        {
            if (obj == null)
                return 0;

            // 基本类型的大小估计
            if (obj is int || obj is float || obj is uint || obj is bool)
                return 4;
            if (obj is long || obj is double || obj is ulong)
                return 8;
            if (obj is short || obj is ushort || obj is char)
                return 2;
            if (obj is byte || obj is sbyte)
                return 1;
            if (obj is decimal)
                return 16;
            if (obj is string str)
                return str.Length * 2 + 24; // 每个字符2字节，加上对象头和长度信息

            // 集合类型的大小估计
            if (obj is ICollection coll)
            {
                long size = 40; // 集合对象头大小估计
                foreach (var item in coll)
                    size += EstimateObjectSize(item);
                return size;
            }

            if (obj is JArray jArray)
            {
                long size = 40; // JArray对象头大小估计
                foreach (var item in jArray)
                    size += EstimateObjectSize(item);
                return size;
            }

            if (obj is JObject jObject)
            {
                long size = 40; // JObject对象头大小估计
                foreach (var property in jObject.Properties())
                {
                    size += EstimateObjectSize(property.Name);
                    size += EstimateObjectSize(property.Value);
                }
                return size;
            }

            // 复杂对象的大小估计
            long objectSize = 40; // 对象头大小估计
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanRead)
                {
                    try
                    {
                        object value = property.GetValue(obj);
                        objectSize += EstimateObjectSize(value);
                    }
                    catch { }
                }
            }

            return objectSize;
        }

        #endregion

        /// <summary>
        /// 检查并刷新基础表缓存（如果不完整）
        /// </summary>
        /// <param name="syncMetadata">缓存同步元数据管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="refreshAction">刷新操作</param>
        /// <returns>如果执行了刷新操作并成功返回true，否则返回false</returns>
        public static bool CheckAndRefreshBaseTableCache(this ICacheSyncMetadata syncMetadata, string tableName, Action<string> refreshAction)
        {
            if (!syncMetadata.ValidateTableCacheIntegrity(tableName))
            {
                try
                {
                    refreshAction?.Invoke(tableName);
                    return syncMetadata.ValidateTableCacheIntegrity(tableName);
                }
                catch
                {
                    return false;
                }
            }
            return true; // 缓存已经是完整的
        }

        /// <summary>
        /// 获取缓存状态的可读描述
        /// </summary>
        /// <param name="syncInfo">缓存同步信息</param>
        /// <returns>缓存状态的可读描述</returns>
        public static string GetStatusDescription(this CacheSyncInfo syncInfo)
        {
            if (syncInfo == null)
                return "不存在";

            if (syncInfo.DataCount <= 0)
                return "空缓存";

            if (syncInfo.HasExpiration && syncInfo.ExpirationTime < DateTime.Now)
                return "已过期";

            return "正常";
        }

        /// <summary>
        /// 获取缓存大小的可读字符串表示
        /// </summary>
        /// <param name="syncInfo">缓存同步信息</param>
        /// <returns>缓存大小的可读字符串</returns>
        public static string GetReadableSize(this CacheSyncInfo syncInfo)
        {
            if (syncInfo == null || syncInfo.EstimatedSize < 0)
                return "未知";

            if (syncInfo.EstimatedSize < 1024)
                return $"{syncInfo.EstimatedSize} B";
            else if (syncInfo.EstimatedSize < 1024 * 1024)
                return $"{(syncInfo.EstimatedSize / 1024.0):F2} KB";
            else
                return $"{(syncInfo.EstimatedSize / (1024.0 * 1024.0)):F2} MB";
        }
    }
}