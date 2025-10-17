using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存同步元数据管理器
    /// 实现ICacheSyncMetadata接口，管理缓存同步所需的元数据
    /// </summary>
    public class CacheSyncMetadataManager : ICacheSyncMetadata
    {
        private readonly ConcurrentDictionary<string, CacheSyncInfo> _syncMetadata;
        private readonly ILogger<CacheSyncMetadataManager> _logger;
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public CacheSyncMetadataManager(ILogger<CacheSyncMetadataManager> logger = null)
        {
            _logger = logger;
            _syncMetadata = new ConcurrentDictionary<string, CacheSyncInfo>();
        }

        /// <summary>
        /// 获取指定表的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>缓存同步元数据，如果不存在则返回null</returns>
        public CacheSyncInfo GetTableSyncInfo(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");

            _syncMetadata.TryGetValue(tableName, out var syncInfo);
            return syncInfo;
        }

        /// <summary>
        /// 更新指定表的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dataCount">数据数量</param>
        /// <param name="estimatedSize">估计大小（字节）</param>
        public void UpdateTableSyncInfo(string tableName, int dataCount, long estimatedSize = 0)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");

            if (dataCount < 0)
                throw new ArgumentOutOfRangeException(nameof(dataCount), "数据数量不能为负数");

            try
            {
                // 获取或创建缓存同步信息
                var syncInfo = _syncMetadata.AddOrUpdate(
                    tableName,
                    _ => new CacheSyncInfo(tableName),
                    (_, existingInfo) => existingInfo);

                // 更新属性
                syncInfo.DataCount = dataCount;
                syncInfo.EstimatedSize = estimatedSize;
                syncInfo.LastUpdateTime = DateTime.Now;

                _logger?.LogDebug("已更新表 {TableName} 的缓存同步元数据: 数据数量={DataCount}, 估计大小={EstimatedSize} 字节",
                    tableName, dataCount, estimatedSize);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新表 {TableName} 的缓存同步元数据时发生错误", tableName);
                throw;
            }
        }

        /// <summary>
        /// 更新指定表的缓存同步元数据
        /// 直接使用CacheSyncInfo对象更新
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="syncInfo">缓存同步信息</param>
        public void UpdateTableSyncInfo(string tableName, CacheSyncInfo syncInfo)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");
            if (syncInfo == null)
                throw new ArgumentNullException(nameof(syncInfo), "缓存同步信息不能为空");

            try
            {
                // 确保表名一致
                if (string.IsNullOrEmpty(syncInfo.TableName))
                {
                    syncInfo.TableName = tableName;
                }
                else if (syncInfo.TableName != tableName)
                {
                    _logger?.LogWarning("CacheSyncInfo中的表名与参数表名不一致，将使用参数表名: {ParamTableName} vs {SyncInfoTableName}",
                        tableName, syncInfo.TableName);
                }

                // 更新同步元数据
                _syncMetadata[tableName] = syncInfo.Clone();

                _logger?.LogDebug("已更新表 {TableName} 的缓存同步元数据: 数据数量={DataCount}, 估计大小={EstimatedSize} 字节, 过期时间={ExpirationTime}",
                    tableName, syncInfo.DataCount, syncInfo.EstimatedSize, syncInfo.ExpirationTime);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新表 {TableName} 的缓存同步元数据时发生错误", tableName);
                throw;
            }
        }

        /// <summary>
        /// 设置表缓存的过期时间
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="expirationTime">过期时间</param>
        public void SetTableExpiration(string tableName, DateTime expirationTime)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");

            try
            {
                // 获取或创建缓存同步信息
                var syncInfo = _syncMetadata.AddOrUpdate(
                    tableName,
                    _ => new CacheSyncInfo(tableName),
                    (_, existingInfo) => existingInfo);

                // 更新过期时间
                syncInfo.ExpirationTime = expirationTime;

                _logger?.LogDebug("已设置表 {TableName} 的缓存过期时间: {ExpirationTime}", tableName, expirationTime);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置表 {TableName} 的缓存过期时间时发生错误", tableName);
                throw;
            }
        }

        /// <summary>
        /// 检查表缓存是否过期
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>如果缓存已过期或不存在返回true</returns>
        public bool IsTableExpired(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return true;

            if (_syncMetadata.TryGetValue(tableName, out var syncInfo))
            {
                // 检查是否有过期设置且已过期
                return syncInfo.HasExpiration && syncInfo.ExpirationTime < DateTime.Now;
            }

            // 如果缓存同步信息不存在，视为过期
            return true;
        }

        /// <summary>
        /// 获取所有表的缓存同步元数据
        /// </summary>
        /// <returns>所有表的缓存同步元数据字典</returns>
        public Dictionary<string, CacheSyncInfo> GetAllTableSyncInfo()
        {
            return _syncMetadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone());
        }

        /// <summary>
        /// 从同步元数据中移除指定表
        /// </summary>
        /// <param name="tableName">表名</param>
        public void RemoveTableSyncInfo(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");

            try
            {
                if (_syncMetadata.TryRemove(tableName, out _))
                {
                    _logger?.LogDebug("已从缓存同步元数据中移除表 {TableName}", tableName);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从缓存同步元数据中移除表 {TableName} 时发生错误", tableName);
                throw;
            }
        }

        /// <summary>
        /// 清理过期的缓存同步元数据
        /// </summary>
        public void CleanupExpiredSyncInfo()
        {
            try
            {
                var expiredTables = _syncMetadata.Where(kvp => kvp.Value.HasExpiration && kvp.Value.ExpirationTime < DateTime.Now)
                                              .Select(kvp => kvp.Key)
                                              .ToList();

                foreach (var tableName in expiredTables)
                {
                    _syncMetadata.TryRemove(tableName, out _);
                }

                if (expiredTables.Count > 0)
                {
                    _logger?.LogDebug("已清理 {Count} 个过期的缓存同步元数据", expiredTables.Count);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理过期的缓存同步元数据时发生错误");
            }
        }
    }
}