using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 批量同步完成事件参数
    /// </summary>
    public class BatchSyncCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// 成功更新的表数量
        /// </summary>
        public int UpdatedCount { get; }

        /// <summary>
        /// 跳过的表数量
        /// </summary>
        public int SkippedCount { get; }

        /// <summary>
        /// 总表数量
        /// </summary>
        public int TotalCount => UpdatedCount + SkippedCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updatedCount">成功更新的表数量</param>
        /// <param name="skippedCount">跳过的表数量</param>
        public BatchSyncCompletedEventArgs(int updatedCount, int skippedCount)
        {
            UpdatedCount = updatedCount;
            SkippedCount = skippedCount;
        }
    }

    /// <summary>
    /// 缓存同步元数据管理器
    /// 实现ICacheSyncMetadata接口，管理缓存同步所需的元数据
    /// 支持批量同步操作和事件通知
    /// </summary>
    public class CacheSyncMetadataManager : ICacheSyncMetadata
    {
        private readonly ConcurrentDictionary<string, CacheSyncInfo> _syncMetadata;
        private readonly ILogger<CacheSyncMetadataManager> _logger;
        private readonly object _lock = new object();

        /// <summary>
        /// 批量同步完成事件
        /// </summary>
        public event EventHandler<BatchSyncCompletedEventArgs> OnBatchSyncCompleted;

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
        /// 支持空表缓存验证，确保即使是空表也能被正确识别为有效缓存
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

                // 定义缓存有效的标准：
                // 1. 更新时间必须合理（大于最小时间）
                // 2. 数据行数必须有明确记录（包括0，用于空表）
                bool isUpdateTimeValid = syncInfo.LastUpdateTime > DateTime.MinValue;
                bool isDataCountValid = syncInfo.DataCount >= 0;
                
                // 综合验证结果
                bool isIntegrity = isUpdateTimeValid && isDataCountValid;
                
                // 详细记录验证过程和结果
                if (syncInfo.DataCount == 0)
                {
                    _logger?.LogDebug("表 {TableName} 为空表，缓存验证: 更新时间有效={UpdateTimeValid}, 数据行数有效={DataCountValid}, 整体结果={Result}",
                        tableName, isUpdateTimeValid, isDataCountValid, isIntegrity);
                }
                else
                {
                    _logger?.LogDebug("表 {TableName} 缓存验证: 更新时间有效={UpdateTimeValid}, 数据行数有效={DataCountValid}, 整体结果={Result}",
                        tableName, isUpdateTimeValid, isDataCountValid, isIntegrity);
                }
                
                if (!isIntegrity)
                {
                    _logger?.LogWarning("表 {TableName} 的缓存元数据无效: 数据行数={DataCount}, 更新时间={LastUpdateTime}",
                        tableName, syncInfo.DataCount, syncInfo.LastUpdateTime);
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
        /// 批量更新所有表的缓存同步元数据
        /// 用于客户端和服务器之间的批量同步
        /// </summary>
        /// <param name="syncData">要同步的缓存元数据字典</param>
        /// <param name="overwriteExisting">是否覆盖已存在的元数据，默认false（只更新不存在的）</param>
        public void BatchUpdateSyncMetadata(Dictionary<string, CacheSyncInfo> syncData, bool overwriteExisting = false)
        {
            if (syncData == null)
                throw new ArgumentNullException(nameof(syncData), "同步数据不能为空");

            try
            {
                int updatedCount = 0;
                int skippedCount = 0;

                foreach (var kvp in syncData)
                {
                    string tableName = kvp.Key;
                    CacheSyncInfo newSyncInfo = kvp.Value;

                    if (string.IsNullOrEmpty(tableName) || newSyncInfo == null)
                    {
                        _logger?.LogWarning("跳过无效的同步数据项: 表名={TableName}, 同步信息={SyncInfo}", 
                            tableName, newSyncInfo != null ? "有效" : "无效");
                        skippedCount++;
                        continue;
                    }

                    try
                    {
                        // 确保表名一致
                        if (string.IsNullOrEmpty(newSyncInfo.TableName))
                        {
                            newSyncInfo.TableName = tableName;
                        }

                        // 根据参数决定是否覆盖现有数据
                        bool shouldUpdate = overwriteExisting || !_syncMetadata.ContainsKey(tableName);
                        
                        if (shouldUpdate)
                        {
                            // 使用克隆对象确保数据安全
                            _syncMetadata[tableName] = newSyncInfo.Clone();
                            updatedCount++;
                            
                            _logger?.LogDebug("已更新表 {TableName} 的同步元数据: 数据数量={DataCount}, 大小={Size} 字节, 更新时间={UpdateTime}",
                                tableName, newSyncInfo.DataCount, newSyncInfo.EstimatedSize, newSyncInfo.LastUpdateTime);
                        }
                        else
                        {
                            skippedCount++;
                            _logger?.LogDebug("跳过表 {TableName} 的同步更新（已存在且不允许覆盖）", tableName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "更新表 {TableName} 的同步元数据时发生错误", tableName);
                        skippedCount++;
                    }
                }

                _logger?.LogInformation("批量更新缓存同步元数据完成: 成功更新 {UpdatedCount} 个表，跳过 {SkippedCount} 个表", 
                    updatedCount, skippedCount);

                // 触发同步完成事件（如果需要的话）
                OnBatchSyncCompleted?.Invoke(this, new BatchSyncCompletedEventArgs(updatedCount, skippedCount));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量更新缓存同步元数据时发生错误");
                throw;
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