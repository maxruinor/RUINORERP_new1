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
                
                // 特殊处理空表情况，确保空表缓存也能被正确记录和管理
                if (dataCount == 0)
                {
                    _logger?.LogDebug("已更新空表 {TableName} 的缓存同步元数据: 数据数量=0, 估计大小={EstimatedSize} 字节",
                        tableName, estimatedSize);
                }
                else
                {
                    _logger?.LogDebug("已更新表 {TableName} 的缓存同步元数据: 数据数量={DataCount}, 估计大小={EstimatedSize} 字节",
                        tableName, dataCount, estimatedSize);
                }
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

        /// <summary>
        /// 验证表缓存数据的完整性
        /// 只验证元数据是否存在且有效，不直接访问实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>如果缓存信息有效返回true，否则返回false</returns>
        public bool ValidateTableCacheIntegrity(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName), "表名不能为空");

                var syncInfo = GetTableSyncInfo(tableName);
                if (syncInfo == null)
                {
                    _logger?.LogWarning("表 {TableName} 的缓存信息不存在，无法验证完整性", tableName);
                    return false;
                }

                // 有效的标准是：更新时间合理，且如果有数据行数（包括0）记录
                // 将空表（DataCount=0）也视为有效的缓存，避免频繁查询数据库
                bool isIntegrity = syncInfo.LastUpdateTime > DateTime.MinValue;
                
                // 记录空表缓存验证情况
                if (syncInfo.DataCount == 0) {
                    _logger?.LogDebug("表 {TableName} 为空表，缓存已验证为完整（DataCount=0）", tableName);
                }
                
                if (!isIntegrity)
                {
                    _logger?.LogWarning("表 {TableName} 的缓存元数据无效: 数据行数={DataCount}, 更新时间={LastUpdateTime}",
                        tableName, syncInfo.DataCount, syncInfo.LastUpdateTime);
                }
                else
                {
                    _logger?.LogDebug("表 {TableName} 的缓存元数据有效", tableName);
                }

                return isIntegrity;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证表 {TableName} 的缓存完整性时发生错误", tableName);
                return false;
            }
        }

        /// <summary>
        /// 获取所有缓存不完整的表
        /// </summary>
        /// <returns>缓存不完整的表名列表</returns>
        public List<string> GetTablesWithIncompleteCache()
        {
            try
            {
                var incompleteTables = new List<string>();
                var allTablesInfo = GetAllTableSyncInfo();

                foreach (var kvp in allTablesInfo)
                {
                    if (!ValidateTableCacheIntegrity(kvp.Key))
                    {
                        incompleteTables.Add(kvp.Key);
                    }
                }

                _logger?.LogDebug("发现 {Count} 个表的缓存数据不完整", incompleteTables.Count);
                return incompleteTables;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取缓存不完整的表时发生错误");
                return new List<string>();
            }
        }

        /// <summary>
        /// 刷新缓存信息不完整的表
        /// </summary>
        /// <param name="refreshAction">刷新操作，接收表名作为参数</param>
        /// <returns>成功执行刷新操作的表数量</returns>
        public int RefreshIncompleteTables(Action<string> refreshAction)
        {
            try
            {
                if (refreshAction == null)
                    throw new ArgumentNullException(nameof(refreshAction), "刷新操作不能为空");

                var incompleteTables = GetTablesWithIncompleteCache();
                int refreshedCount = 0;

                foreach (var tableName in incompleteTables)
                {
                    _logger?.LogInformation("开始执行表 {TableName} 的刷新操作", tableName);
                    
                    try
                    {
                        // 执行外部提供的刷新操作
                        refreshAction(tableName);
                        
                        // 验证元数据是否有效
                        if (ValidateTableCacheIntegrity(tableName))
                        {
                            refreshedCount++;
                            _logger?.LogInformation("表 {TableName} 的刷新操作成功且元数据有效", tableName);
                        }
                        else
                        {
                            _logger?.LogWarning("表 {TableName} 的刷新操作后元数据验证失败", tableName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "执行表 {TableName} 的刷新操作时发生错误", tableName);
                    }
                }

                _logger?.LogInformation("刷新操作完成，成功执行 {RefreshedCount}/{TotalCount} 个表", 
                    refreshedCount, incompleteTables.Count);
                return refreshedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行缓存信息不完整的表刷新时发生错误");
                return 0;
            }
        }
    }
}