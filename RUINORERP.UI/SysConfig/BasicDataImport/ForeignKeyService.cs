using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 外键处理服务
    /// 负责外键值的获取和验证，支持预加载和缓存功能
    /// 实现IForeignKeyService接口，支持依赖注入
    /// </summary>
    public class ForeignKeyService : IForeignKeyService
    {
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 外键数据缓存
        /// Key: 表名，Value: 字段值到主键ID的映射
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _foreignKeyCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端</param>
        public ForeignKeyService(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _foreignKeyCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
        }

        /// <summary>
        /// 预加载外键数据
        /// </summary>
        /// <param name="mappings">列映射配置集合</param>
        public void PreloadForeignKeyData(IEnumerable<ColumnMapping> mappings)
        {
            if (mappings == null || !mappings.Any())
            {
                return;
            }

            // 收集所有外键映射
            var foreignKeyMappings = mappings.Where(m => m.DataSourceType == DataSourceType.ForeignKey).ToList();
            if (!foreignKeyMappings.Any())
            {
                return;
            }

            // 按表分组预加载数据
            var tableGroups = foreignKeyMappings.GroupBy(m => m.ForeignConfig?.ForeignKeyTable?.Key).Where(g => !string.IsNullOrEmpty(g.Key));
            foreach (var group in tableGroups)
            {
                string tableName = group.Key;
                foreach (var mapping in group)
                {
                    if (!string.IsNullOrEmpty(mapping.ForeignConfig?.ForeignKeyField?.Key))
                    {
                        PreloadForeignKeyData(tableName, mapping.ForeignConfig.ForeignKeyField.Key);
                    }
                }
            }
        }

        /// <summary>
        /// 预加载指定表的外键数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        public void PreloadForeignKeyData(string tableName, string fieldName)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(fieldName))
            {
                return;
            }

            try
            {
                string cacheKey = $"{tableName}_{fieldName}";
                if (_foreignKeyCache.ContainsKey(cacheKey))
                {
                    return; // 已经缓存过
                }

                // 构建查询SQL
                string sql = $"SELECT ID, {fieldName} FROM {tableName}";
                var data = _db.Ado.GetDataTable(sql);

                // 构建缓存
                var fieldValueToIdMap = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow row in data.Rows)
                {
                    object id = row["ID"];
                    object fieldValue = row[fieldName];
                    if (fieldValue != DBNull.Value && fieldValue != null)
                    {
                        string key = fieldValue.ToString().Trim();
                        fieldValueToIdMap.TryAdd(key, id);
                    }
                }

                _foreignKeyCache.TryAdd(cacheKey, fieldValueToIdMap);
                Debug.WriteLine($"预加载外键数据完成: {tableName}.{fieldName}, 共 {fieldValueToIdMap.Count} 条记录");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"预加载外键数据失败: {tableName}.{fieldName}, 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            _foreignKeyCache.Clear();
        }

        /// <summary>
        /// 获取外键值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>外键ID值</returns>
        public object GetForeignKeyValue(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // 从Excel原始列或映射后的数据表中获取代码值
                string foreignKeyValue = null;
                string sourceColumnName = null;
                string sourceColumnDisplayName = null;

                // 优先从指定的外键来源列获取（Excel列）
                if (mapping.ForeignConfig?.ForeignKeySourceColumn != null && !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn.ExcelColumnName))
                {
                    // 使用配置的外键来源列（如"供应商"列）
                    sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.ExcelColumnName;
                    // DisplayName = 显示名称
                    sourceColumnDisplayName = mapping.ForeignConfig.ForeignKeySourceColumn.DisplayName ?? sourceColumnName;
                    if (dataTableContainsColumn(row.Table, sourceColumnName))
                    {
                        foreignKeyValue = row[sourceColumnName]?.ToString();
                    }
                }
                else if (!string.IsNullOrEmpty(mapping.ExcelColumn) &&
                         !mapping.ExcelColumn.StartsWith("[") &&
                         !mapping.ExcelColumn.StartsWith("("))
                {
                    // 如果没有指定外键来源列，但映射有Excel列，尝试使用映射的Excel列
                    sourceColumnName = mapping.ExcelColumn;
                    sourceColumnDisplayName = mapping.ExcelColumn;
                    if (dataTableContainsColumn(row.Table, sourceColumnName))
                    {
                        foreignKeyValue = row[sourceColumnName]?.ToString();
                    }
                }
                else
                {
                    // 尝试从系统字段列获取（映射后的列名）
                    sourceColumnName = mapping.SystemField?.Value;
                    sourceColumnDisplayName = mapping.SystemField?.Value;
                    if (dataTableContainsColumn(row.Table, sourceColumnName))
                    {
                        foreignKeyValue = row[sourceColumnName]?.ToString();
                    }
                }

                // 如果获取到了值，查询关联表获取主键ID
                if (!string.IsNullOrEmpty(foreignKeyValue) &&
                    mapping.ForeignConfig != null &&
                    !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeyTable?.Key) &&
                    !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeyField?.Key))
                {
                    // 查找关联表中的对应值（通过代码字段）
                    object foreignKeyId = GetForeignKeyId(foreignKeyValue, mapping.ForeignConfig.ForeignKeyTable.Key, mapping.ForeignConfig.ForeignKeyField.Key);
                    if (foreignKeyId != null)
                    {
                        return foreignKeyId;
                    }
                    else
                    {
                        errorMessage = $"行 {rowNumber} 外键值 '{foreignKeyValue}' (来源列: {sourceColumnDisplayName ?? sourceColumnName}) " +
                            $"在关联表 {mapping.ForeignConfig.ForeignKeyTable.Key} 的字段 {mapping.ForeignConfig.ForeignKeyField.Key} 中未找到对应记录。";

                        // 如果是供应商表，提供额外提示
                        if (mapping.ForeignConfig.ForeignKeyTable.Key == "tb_CustomerVendor")
                        {
                            errorMessage += "\n\n提示：请确保供应商名称在供应商表中已存在，或者先导入供应商数据。";
                        }

                        return null;
                    }
                }
                else if (mapping.IsRequired)
                {
                    // 如果是必填字段但没有获取到外键值
                    errorMessage = $"行 {rowNumber} 字段 {mapping.SystemField?.Value} 是必填字段，但无法从列 '{sourceColumnDisplayName ?? sourceColumnName}' 获取有效的外键值";
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                errorMessage = $"行 {rowNumber} 处理外键值时发生错误: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// 获取外键ID
        /// 优先从缓存中获取，缓存未命中时查询数据库
        /// </summary>
        /// <param name="foreignKeyValue">外键代码值</param>
        /// <param name="relatedTableName">关联表名</param>
        /// <param name="relatedTableField">关联表字段名</param>
        /// <returns>外键主键ID</returns>
        public object GetForeignKeyId(string foreignKeyValue, string relatedTableName, string relatedTableField)
        {
            try
            {
                if (string.IsNullOrEmpty(foreignKeyValue) ||
                    string.IsNullOrEmpty(relatedTableName) ||
                    string.IsNullOrEmpty(relatedTableField))
                {
                    return null;
                }

                string trimmedValue = foreignKeyValue.Trim();

                // 尝试从缓存中获取
                string cacheKey = $"{relatedTableName}_{relatedTableField}";
                if (_foreignKeyCache.TryGetValue(cacheKey, out var fieldValueToIdMap))
                {
                    if (fieldValueToIdMap.TryGetValue(trimmedValue, out var cachedId))
                    {
                        Debug.WriteLine($"从缓存中获取外键ID: {relatedTableName}.{relatedTableField} = {trimmedValue} -> {cachedId}");
                        return cachedId;
                    }
                }

                // 缓存未命中，查询数据库
                // 构建查询SQL，通过代码字段查询主键ID
                // 例如：SELECT ID FROM tb_ProdCategories WHERE CategoryCode = 'CATEGORY001'
                // 例如：SELECT ID FROM tb_CustomerVendor WHERE VendorName = '供应商A'
                string sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} = @value";

                // 使用参数化查询防止SQL注入
                var parameters = new { value = trimmedValue };

                // 执行查询
                var result = _db.Ado.GetDataTable(sql, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    // 检查是否有多个匹配记录（唯一性验证）
                    if (result.Rows.Count > 1)
                    {
                        // 对于供应商表等需要唯一性验证的表，记录警告
                        Debug.WriteLine(
                            $"警告：外键查询返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}，匹配数：{result.Rows.Count}");

                        // 如果启用了严格模式，可以抛出异常
                        // throw new Exception($"外键值 '{foreignKeyValue}' 在表 {relatedTableName} 中不唯一，找到 {result.Rows.Count} 条记录");
                    }

                    object id = result.Rows[0]["ID"];
                    // 将结果添加到缓存
                    AddToCache(cacheKey, trimmedValue, id);
                    return id;
                }

                // 如果没有找到，尝试模糊匹配（对于供应商名称等可能包含空格的情况）
                if (relatedTableField.ToLower().Contains("name") ||
                    relatedTableField.ToLower().Contains("vendor") ||
                    relatedTableField.ToLower().Contains("supplier"))
                {
                    sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} LIKE @value";
                    parameters = new { value = $"%{trimmedValue}%" };
                    result = _db.Ado.GetDataTable(sql, parameters);

                    if (result != null && result.Rows.Count > 0)
                    {
                        if (result.Rows.Count > 1)
                        {
                            Debug.WriteLine(
                                $"警告：模糊匹配返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}");
                        }
                        object id = result.Rows[0]["ID"];
                        // 模糊匹配结果不缓存，因为可能不准确
                        return id;
                    }
                }

                // 如果没有找到，返回null
                return null;
            }
            catch (Exception ex)
            {
                // 记录错误信息
                Debug.WriteLine($"查询外键ID失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 将外键数据添加到缓存
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="id">主键ID</param>
        private void AddToCache(string cacheKey, string fieldValue, object id)
        {
            if (!_foreignKeyCache.ContainsKey(cacheKey))
            {
                _foreignKeyCache.TryAdd(cacheKey, new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase));
            }

            if (_foreignKeyCache.TryGetValue(cacheKey, out var fieldValueToIdMap))
            {
                fieldValueToIdMap.TryAdd(fieldValue, id);
            }
        }

        /// <summary>
        /// 验证外键
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateForeignKey(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage)
        {
            object foreignKeyId = GetForeignKeyValue(row, mapping, rowNumber, out errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        /// <summary>
        /// 检查DataTable是否包含指定列
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否包含</returns>
        private bool dataTableContainsColumn(DataTable table, string columnName)
        {
            return table?.Columns.Contains(columnName) ?? false;
        }
    }
}
