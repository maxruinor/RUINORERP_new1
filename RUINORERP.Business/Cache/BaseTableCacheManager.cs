using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 基础表缓存管理器
    /// 专门用于管理基础表的缓存信息，包括表名、行数、刷新时间等
    /// 用于在网络问题时验证缓存数据的完整性
    /// </summary>
    public class BaseTableCacheManager : IBaseTableCacheManager
    {
        #region 依赖注入字段
        private readonly IEntityCacheManager _cacheManager;
        private readonly ICacheSyncMetadata _cacheSyncMetadata;
        private readonly ILogger<BaseTableCacheManager> _logger;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="cacheSyncMetadata">缓存同步元数据管理器</param>
        /// <param name="logger">日志记录器</param>
        public BaseTableCacheManager(
            IEntityCacheManager cacheManager,
            ICacheSyncMetadata cacheSyncMetadata,
            ILogger<BaseTableCacheManager> logger = null)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _cacheSyncMetadata = cacheSyncMetadata ?? throw new ArgumentNullException(nameof(cacheSyncMetadata));
            _logger = logger;
        }
        #endregion

        #region 基础表缓存信息方法
        /// <summary>
        /// 获取所有基础表的缓存信息
        /// </summary>
        /// <returns>所有基础表的缓存信息列表</returns>
        public List<BaseTableCacheInfo> GetAllBaseTablesCacheInfo()
        {
            try
            {
                var allSyncInfos = _cacheSyncMetadata.GetAllTableSyncInfo();
                var baseTablesInfo = new List<BaseTableCacheInfo>();

                foreach (var kvp in allSyncInfos)
                {
                    var syncInfo = kvp.Value;
                    if (syncInfo != null)
                    {
                        // 创建基础表缓存信息对象
                        var tableInfo = new BaseTableCacheInfo
                        {
                            TableName = syncInfo.TableName,
                            DataCount = syncInfo.DataCount,
                            LastUpdateTime = syncInfo.LastUpdateTime,
                            // 基础表缓存永不过期
                            HasExpiration = false,
                            EstimatedSize = syncInfo.EstimatedSize,
                            SourceInfo = syncInfo.SourceInfo
                        };

                        baseTablesInfo.Add(tableInfo);
                    }
                }

                _logger?.LogDebug("已获取 {Count} 个表的缓存信息", baseTablesInfo.Count);
                return baseTablesInfo.OrderBy(t => t.TableName).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取基础表缓存信息时发生错误");
                return new List<BaseTableCacheInfo>();
            }
        }

        /// <summary>
        /// 获取指定表的缓存信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表的缓存信息，如果不存在则返回null</returns>
        public BaseTableCacheInfo GetBaseTableCacheInfo(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName), "表名不能为空");

                var syncInfo = _cacheSyncMetadata.GetTableSyncInfo(tableName);
                if (syncInfo == null)
                {
                    _logger?.LogWarning("表 {TableName} 的缓存信息不存在", tableName);
                    return null;
                }

                // 创建并返回基础表缓存信息对象
                return new BaseTableCacheInfo
                {
                    TableName = syncInfo.TableName,
                    DataCount = syncInfo.DataCount,
                    LastUpdateTime = syncInfo.LastUpdateTime,
                    // 基础表缓存永不过期
                    HasExpiration = false,
                    EstimatedSize = syncInfo.EstimatedSize,
                    SourceInfo = syncInfo.SourceInfo
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取表 {TableName} 的缓存信息时发生错误", tableName);
                return null;
            }
        }

        /// <summary>
        /// 验证表缓存数据的完整性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>如果缓存数据完整返回true，否则返回false</returns>
        public bool ValidateTableCacheIntegrity(string tableName)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName), "表名不能为空");

                var cacheInfo = GetBaseTableCacheInfo(tableName);
                if (cacheInfo == null)
                {
                    _logger?.LogWarning("表 {TableName} 的缓存信息不存在，无法验证完整性", tableName);
                    return false;
                }

                // 获取实际缓存的实体列表
                var entityList = _cacheManager.GetEntityList<object>(tableName);
                int actualCount = entityList?.Count ?? 0;

                // 验证缓存数据数量是否匹配
                bool isIntegrity = cacheInfo.DataCount > 0 && actualCount > 0 && cacheInfo.DataCount == actualCount;
                
                if (!isIntegrity)
                {
                    _logger?.LogWarning("表 {TableName} 的缓存数据不完整: 记录的数量={ExpectedCount}, 实际缓存的数量={ActualCount}",
                        tableName, cacheInfo.DataCount, actualCount);
                }
                else
                {
                    _logger?.LogDebug("表 {TableName} 的缓存数据完整", tableName);
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
                var allTablesInfo = GetAllBaseTablesCacheInfo();

                foreach (var tableInfo in allTablesInfo)
                {
                    if (!ValidateTableCacheIntegrity(tableInfo.TableName))
                    {
                        incompleteTables.Add(tableInfo.TableName);
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
        /// 更新基础表缓存信息并验证完整性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityList">实体列表</param>
        /// <returns>如果更新成功且缓存完整返回true，否则返回false</returns>
        public bool UpdateBaseTableCache<T>(string tableName, List<T> entityList) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName), "表名不能为空");

                // 更新缓存
                _cacheManager.UpdateEntityList(tableName, entityList);
                
                // 等待短暂时间确保缓存更新完成
                Task.Delay(50).Wait();
                
                // 验证缓存完整性
                return ValidateTableCacheIntegrity(tableName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新表 {TableName} 的缓存并验证完整性时发生错误", tableName);
                return false;
            }
        }

        /// <summary>
        /// 设置基础表缓存信息
        /// 用于更新和保存基础表的缓存元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheInfo">缓存信息对象</param>
        public void SetBaseTableCacheInfo(string tableName, BaseTableCacheInfo cacheInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new ArgumentNullException(nameof(tableName), "表名不能为空");

                if (cacheInfo == null)
                    throw new ArgumentNullException(nameof(cacheInfo), "缓存信息对象不能为空");

                // 使用UpdateTableSyncInfo方法更新表同步信息
                _cacheSyncMetadata.UpdateTableSyncInfo(
                    tableName, 
                    cacheInfo.DataCount, 
                    cacheInfo.EstimatedSize);

                // 记录最后更新时间和源信息（如果有）
                var existingSyncInfo = _cacheSyncMetadata.GetTableSyncInfo(tableName);
                if (existingSyncInfo != null) {
                    // 通过更新后获取的对象记录额外信息
                    _logger?.LogDebug("表 {TableName} 的缓存信息已更新: 数据行数={DataCount}, 更新时间={LastUpdateTime}",
                        tableName, existingSyncInfo.DataCount, existingSyncInfo.LastUpdateTime);
                }

                _logger?.LogDebug("已成功更新表 {TableName} 的基础缓存信息", tableName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置表 {TableName} 的缓存信息时发生错误", tableName);
                throw;
            }
        }

        /// <summary>
        /// 刷新缓存不完整的表
        /// </summary>
        /// <param name="refreshAction">刷新操作，接收表名作为参数</param>
        /// <returns>成功刷新的表数量</returns>
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
                    _logger?.LogInformation("开始刷新表 {TableName} 的缓存", tableName);
                    
                    try
                    {
                        refreshAction(tableName);
                        
                        // 验证刷新后的缓存完整性
                        if (ValidateTableCacheIntegrity(tableName))
                        {
                            refreshedCount++;
                            _logger?.LogInformation("表 {TableName} 的缓存刷新成功且验证通过", tableName);
                        }
                        else
                        {
                            _logger?.LogWarning("表 {TableName} 的缓存刷新后验证失败", tableName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "刷新表 {TableName} 的缓存时发生错误", tableName);
                    }
                }

                _logger?.LogInformation("刷新操作完成，成功刷新 {RefreshedCount}/{TotalCount} 个表", 
                    refreshedCount, incompleteTables.Count);
                return refreshedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "刷新缓存不完整的表时发生错误");
                return 0;
            }
        }
        #endregion
    }

    /// <summary>
    /// 基础表缓存信息类
    /// 存储基础表的缓存信息，用于验证缓存数据的完整性
    /// 注意：基础表缓存永不过期
    /// </summary>
    public class BaseTableCacheInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 数据行数
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 估计内存大小（字节）
        /// </summary>
        public long EstimatedSize { get; set; }

        /// <summary>
        /// 源信息
        /// 用于存储额外的源数据信息
        /// </summary>
        public string SourceInfo { get; set; }

        /// <summary>
        /// 是否有过期设置（基础表缓存永不过期）
        /// </summary>
        public bool HasExpiration { get; set; } = false;

        /// <summary>
        /// 判断缓存是否过期（基础表缓存永不过期）
        /// </summary>
        public bool IsExpired => false;

        /// <summary>
        /// 判断缓存是否为空（无数据）
        /// </summary>
        public bool IsEmpty => DataCount <= 0;

        /// <summary>
        /// 获取缓存大小的可读字符串表示
        /// </summary>
        public string ReadableSize
        {
            get
            {
                if (EstimatedSize < 1024)
                    return $"{EstimatedSize} B";
                else if (EstimatedSize < 1024 * 1024)
                    return $"{(EstimatedSize / 1024.0):F2} KB";
                else
                    return $"{(EstimatedSize / (1024.0 * 1024.0)):F2} MB";
            }
        }

        /// <summary>
        /// 获取缓存状态的可读描述
        /// </summary>
        public string StatusDescription
        {
            get
            {
                if (IsEmpty)
                    return "空缓存";
                return "正常";
            }
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>表缓存信息的字符串表示</returns>
        public override string ToString()
        {
            return $"{TableName} - {DataCount} 行 - {StatusDescription} - 更新时间: {LastUpdateTime}";
        }
    }
}