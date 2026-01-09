// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：实体类型选择器辅助类，用于获取和管理系统中所有可用的实体类型
// ********************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Model;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 实体类型选择器辅助类
    /// 负责获取和管理系统中所有可用的实体类型
    /// </summary>
    public class EntityTypeSelectorHelper
    {
        private readonly ITableSchemaManager _tableSchemaManager;
        private readonly ILogger<EntityTypeSelectorHelper> _logger;
        private readonly Dictionary<string, TableInfo> _tableInfoCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableSchemaManager">表结构管理器</param>
        /// <param name="logger">日志记录器</param>
        public EntityTypeSelectorHelper(ITableSchemaManager tableSchemaManager, ILogger<EntityTypeSelectorHelper> logger)
        {
            _tableSchemaManager = tableSchemaManager;
            _logger = logger;
            _tableInfoCache = new Dictionary<string, TableInfo>();
        }

        /// <summary>
        /// 获取所有可用的表信息列表（按表类型分类）
        /// </summary>
        /// <returns>表信息字典，Key为表类型</returns>
        public async Task<Dictionary<TableType, List<TableInfo>>> GetAllTablesAsync()
        {
            try
            {
                var result = new Dictionary<TableType, List<TableInfo>>();

                // 初始化分类
                foreach (TableType type in Enum.GetValues(typeof(TableType)))
                {
                    result[type] = new List<TableInfo>();
                }

                // 从表结构管理器获取可缓存的表
                var cacheableTables = await _tableSchemaManager.GetCacheableTableNamesListAsync();

                if (cacheableTables == null || cacheableTables.Count == 0)
                {
                    _logger.LogWarning("未获取到可缓存的表信息");
                    return result;
                }

                // 遍历表列表并分类
                foreach (var tableName in cacheableTables)
                {
                    var tableInfo = await GetTableInfoAsync(tableName);
                    if (tableInfo != null && result.ContainsKey(tableInfo.TableType))
                    {
                        result[tableInfo.TableType].Add(tableInfo);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取表信息列表失败");
                return new Dictionary<TableType, List<TableInfo>>();
            }
        }

        /// <summary>
        /// 获取指定表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表信息</returns>
        public async Task<TableInfo> GetTableInfoAsync(string tableName)
        {
            try
            {
                // 检查缓存
                if (_tableInfoCache.ContainsKey(tableName))
                {
                    return _tableInfoCache[tableName];
                }

                // 从表结构管理器获取实体类型
                var entityType = await _tableSchemaManager.GetEntityTypeByTableNameAsync(tableName);
                
                if (string.IsNullOrEmpty(entityType))
                {
                    _logger.LogWarning($"表 {tableName} 未找到对应的实体类型");
                    return null;
                }

                // 获取实体类的描述
                var description = GetEntityDescription(entityType);

                // 确定表类型
                var tableType = DetermineTableType(tableName, entityType);

                var tableInfo = new TableInfo
                {
                    TableName = tableName,
                    EntityType = entityType,
                    Description = description,
                    TableType = tableType,
                    IsCacheable = true
                };

                // 加入缓存
                _tableInfoCache[tableName] = tableInfo;

                return tableInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取表 {tableName} 的信息失败");
                return null;
            }
        }
 

 

        /// <summary>
        /// 获取实体类的描述信息
        /// </summary>
        /// <param name="entityType">实体类型全限定名</param>
        /// <returns>描述文本</returns>
        private string GetEntityDescription(string entityType)
        {
            try
            {
                var type = Type.GetType(entityType);
                if (type == null)
                {
                    return entityType;
                }

                var descriptionAttr = type.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttr?.Description ?? type.Name;
            }
            catch
            {
                return entityType;
            }
        }

        /// <summary>
        /// 确定表的类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>表类型</returns>
        private TableType DetermineTableType(string tableName, string entityType)
        {
            // 根据表名或实体类型进行分类判断
            // 这里可以根据实际项目的命名规范进行调整

            // 判断是否为系统表
            if (tableName.StartsWith("sys_") || tableName.StartsWith("tb_") && tableName.Contains("Info"))
            {
                return TableType.System;
            }

            // 判断是否为基础数据表
            var baseTableKeywords = new[] { "Prod", "Location", "Customer", "Supplier", "Employee", "Material", "Warehouse" };
            if (baseTableKeywords.Any(keyword => tableName.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                return TableType.Base;
            }

            // 判断是否为业务数据表
            var businessTableKeywords = new[] { "Order", "Bill", "Inbound", "Outbound", "Stock", "Invoice", "Receipt" };
            if (businessTableKeywords.Any(keyword => tableName.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                return TableType.Business;
            }

            // 判断是否为视图
            if (entityType.Contains("View", StringComparison.OrdinalIgnoreCase))
            {
                return TableType.View;
            }

            // 默认未分类
            return TableType.Unknown;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            _tableInfoCache.Clear();
        }
    }
}
