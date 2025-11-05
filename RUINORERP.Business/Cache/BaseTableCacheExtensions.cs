using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 基础表缓存扩展方法
    /// 为现有缓存系统提供基础表缓存管理的扩展功能
    /// </summary>
    public static class BaseTableCacheExtensions
    {
        /// <summary>
        /// 创建基础表缓存管理器实例
        /// </summary>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="cacheSyncMetadata">缓存同步元数据管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>基础表缓存管理器实例</returns>
        public static IBaseTableCacheManager CreateBaseTableCacheManager(
            this IEntityCacheManager cacheManager,
            ICacheSyncMetadata cacheSyncMetadata,
            ILogger<BaseTableCacheManager> logger = null)
        {
            return new BaseTableCacheManager(cacheManager, cacheSyncMetadata, logger);
        }

        /// <summary>
        /// 使用基础表缓存管理器更新实体列表并验证完整性
        /// 扩展IEntityCacheManager的功能
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="entityList">实体列表</param>
        /// <param name="cacheSyncMetadata">缓存同步元数据管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>如果更新成功且缓存完整返回true，否则返回false</returns>
        public static bool UpdateEntityListWithIntegrityCheck<T>(
            this IEntityCacheManager cacheManager,
            string tableName,
            List<T> entityList,
            ICacheSyncMetadata cacheSyncMetadata,
            ILogger logger = null)
            where T : class
        {
            try
            {
                // 先使用现有方法更新缓存
                cacheManager.UpdateEntityList(tableName, entityList);

                // 创建临时的基础表缓存管理器实例进行验证
                var baseTableCacheManager = new BaseTableCacheManager(
                    cacheManager,
                    cacheSyncMetadata,
                    logger as ILogger<BaseTableCacheManager>);

                // 验证缓存完整性
                return baseTableCacheManager.ValidateTableCacheIntegrity(tableName);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新表 {TableName} 的缓存并进行完整性检查时发生错误", tableName);
                return false;
            }
        }

        /// <summary>
        /// 检查并修复缓存不完整的表
        /// </summary>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="cacheSyncMetadata">缓存同步元数据管理器</param>
        /// <param name="tableName">表名</param>
        /// <param name="reloadFunction">重新加载数据的函数</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>如果修复成功返回true，否则返回false</returns>
        public static bool CheckAndFixCacheIntegrity<T>(
            this IEntityCacheManager cacheManager,
            ICacheSyncMetadata cacheSyncMetadata,
            string tableName,
            Func<List<T>> reloadFunction,
            ILogger logger = null)
            where T : class
        {
            try
            {
                // 创建临时的基础表缓存管理器实例
                var baseTableCacheManager = new BaseTableCacheManager(
                    cacheManager,
                    cacheSyncMetadata,
                    logger as ILogger<BaseTableCacheManager>);

                // 检查缓存完整性
                if (!baseTableCacheManager.ValidateTableCacheIntegrity(tableName))
                {
                    logger?.LogWarning("表 {TableName} 的缓存数据不完整，尝试重新加载", tableName);

                    // 重新加载数据
                    var newData = reloadFunction();
                    if (newData != null && newData.Count > 0)
                    {
                        // 重新更新缓存
                        cacheManager.UpdateEntityList(tableName, newData);
                        
                        // 再次验证完整性
                        bool fixedSuccessfully = baseTableCacheManager.ValidateTableCacheIntegrity(tableName);
                        
                        if (fixedSuccessfully)
                        {
                            logger?.LogInformation("表 {TableName} 的缓存数据已成功修复", tableName);
                        }
                        else
                        {
                            logger?.LogError("表 {TableName} 的缓存数据修复失败", tableName);
                        }
                        
                        return fixedSuccessfully;
                    }
                    else
                    {
                        logger?.LogError("表 {TableName} 重新加载数据失败，无法修复缓存", tableName);
                        return false;
                    }
                }

                // 缓存已经是完整的
                return true;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "检查并修复表 {TableName} 的缓存完整性时发生错误", tableName);
                return false;
            }
        }

        /// <summary>
        /// 获取缓存状态报告
        /// 生成一个详细的缓存状态报告字符串
        /// </summary>
        /// <param name="baseTableCacheManager">基础表缓存管理器</param>
        /// <returns>缓存状态报告字符串</returns>
        public static string GetCacheStatusReport(this IBaseTableCacheManager baseTableCacheManager)
        {
            var allTablesInfo = baseTableCacheManager.GetAllBaseTablesCacheInfo();
            var incompleteTables = baseTableCacheManager.GetTablesWithIncompleteCache();
            
            StringBuilder report = new StringBuilder();
            report.AppendLine("缓存状态报告");
            report.AppendLine("========================================");
            report.AppendLine($"总表数: {allTablesInfo.Count}");
            report.AppendLine($"缓存完整的表: {allTablesInfo.Count - incompleteTables.Count}");
            report.AppendLine($"缓存不完整的表: {incompleteTables.Count}");
            report.AppendLine("----------------------------------------");
            
            if (incompleteTables.Count > 0)
            {
                report.AppendLine("缓存不完整的表列表:");
                foreach (var tableName in incompleteTables)
                {
                    var info = baseTableCacheManager.GetBaseTableCacheInfo(tableName);
                    if (info != null)
                    {
                        report.AppendLine($"  - {info}");
                    }
                }
                report.AppendLine("----------------------------------------");
            }
            
            report.AppendLine("表缓存详细信息:");
            foreach (var tableInfo in allTablesInfo.OrderBy(t => t.TableName))
            {
                report.AppendLine($"  - {tableInfo.TableName}");
                report.AppendLine($"    行数: {tableInfo.DataCount}");
                report.AppendLine($"    大小: {tableInfo.ReadableSize}");
                report.AppendLine($"    状态: {tableInfo.StatusDescription}");
                report.AppendLine($"    更新时间: {tableInfo.LastUpdateTime}");
                // 基础表缓存永不过期，不需要显示过期时间
                if (tableInfo.HasExpiration)
                {
                    report.AppendLine($"    过期策略: 已配置");
                }
            }
            report.AppendLine("========================================");
            report.AppendLine($"生成时间: {DateTime.Now}");
            
            return report.ToString();
        }

        /// <summary>
        /// 批量检查并修复多个表的缓存完整性
        /// </summary>
        /// <param name="baseTableCacheManager">基础表缓存管理器</param>
        /// <param name="tableNames">要检查的表名列表</param>
        /// <param name="reloadFunction">重新加载数据的函数，接收表名，返回实体列表</param>
        /// <returns>成功修复的表数量</returns>
        public static int BatchCheckAndFixCacheIntegrity(
            this IBaseTableCacheManager baseTableCacheManager,
            List<string> tableNames,
            Func<string, List<object>> reloadFunction)
        {
            int fixedCount = 0;
            
            foreach (var tableName in tableNames)
            {
                if (!baseTableCacheManager.ValidateTableCacheIntegrity(tableName))
                {
                    try
                    {
                        var newData = reloadFunction(tableName);
                        if (newData != null && newData.Count > 0)
                        {
                            if (baseTableCacheManager.UpdateBaseTableCache<object>(tableName, newData))
                            {
                                fixedCount++;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // 忽略单个表的错误，继续处理其他表
                    }
                }
            }
            
            return fixedCount;
        }

        /// <summary>
        /// 获取表缓存的健康评分（0-100分）
        /// 基于缓存完整性、过期状态等因素计算
        /// </summary>
        /// <param name="baseTableCacheManager">基础表缓存管理器</param>
        /// <returns>缓存健康评分</returns>
        public static int GetCacheHealthScore(this IBaseTableCacheManager baseTableCacheManager)
        {
            var allTablesInfo = baseTableCacheManager.GetAllBaseTablesCacheInfo();
            if (allTablesInfo.Count == 0)
                return 0;
            
            int totalScore = 0;
            foreach (var tableInfo in allTablesInfo)
            {
                int tableScore = 100;
                
                // 检查缓存是否为空
                if (tableInfo.IsEmpty)
                    tableScore -= 50;
                
                // 检查缓存是否过期
                if (tableInfo.IsExpired)
                    tableScore -= 30;
                
                // 确保分数在0-100之间
                tableScore = Math.Max(0, Math.Min(100, tableScore));
                totalScore += tableScore;
            }
            
            return totalScore / allTablesInfo.Count;
        }
    }
}