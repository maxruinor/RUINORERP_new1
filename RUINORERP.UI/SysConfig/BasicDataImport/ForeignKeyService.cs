using RUINORERP.Business.Cache;
using RUINORERP.Global;
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
        private readonly ITableSchemaManager _tableSchemaManager;

        /// <summary>
        /// 外键数据缓存
        /// Key: 表名，Value: 字段值到主键ID的映射
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _foreignKeyCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">数据库客户端</param>
        /// <param name="tableSchemaManager">表结构管理器</param>
        public ForeignKeyService(ISqlSugarClient db, ITableSchemaManager tableSchemaManager = null)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _tableSchemaManager = tableSchemaManager;
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
            var foreignKeyMappings = mappings.Where(m => m.ColumnDataSourceType == (int)DataSourceType.ForeignKey).ToList();
            if (!foreignKeyMappings.Any())
            {
                return;
            }

            // 按表分组预加载数据
            var tableGroups = foreignKeyMappings.GroupBy(m => 
            {
                var fkConfig = m.DataSourceConfig as DatabaseReferenceConfig;
                return fkConfig?.ForeignTableName;
            }).Where(g => !string.IsNullOrEmpty(g.Key));
            foreach (var group in tableGroups)
            {
                string tableName = group.Key;
                foreach (var mapping in group)
                {
                    var fkConfig = mapping.DataSourceConfig as DatabaseReferenceConfig;
                    if (!string.IsNullOrEmpty(fkConfig?.ForeignFieldName))
                    {
                        PreloadForeignKeyData(mapping);
                    }
                }
            }
        }

        /// <summary>
        /// 预加载指定表的外键数据
        /// </summary>
        /// <param name="mapping">列映射配置</param>
        public void PreloadForeignKeyData(ColumnMapping mapping)
        {
            var fkConfig = mapping.DataSourceConfig as DatabaseReferenceConfig;
            if (fkConfig == null)
            {
                Debug.WriteLine("预加载外键数据失败: DataSourceConfig不是ForeignKeyConfig类型");
                return;
            }

            try
            {
                string cacheKey = $"{fkConfig.ForeignTableName}_{fkConfig.ForeignFieldName}";
                if (_foreignKeyCache.ContainsKey(cacheKey))
                {
                    return; // 已经缓存过
                }

                // ✅ 获取关联表的主键字段名（用于查询ID）
                string primaryKeyField = GetPrimaryKeyFieldName(fkConfig.ForeignTableName);
                if (string.IsNullOrEmpty(primaryKeyField))
                {
                    Debug.WriteLine($"预加载外键数据失败: 无法获取表 {fkConfig.ForeignTableName} 的主键字段名");
                    return;
                }

                // ✅ 正确SQL：只查询关联表的（业务字段，主键ID）
                // 例如：SELECT CategoryCode, Category_ID FROM tb_ProdCategories
                string sql = $"SELECT {fkConfig.ForeignFieldName}, {primaryKeyField} FROM {fkConfig.ForeignTableName}";
                var data = _db.Ado.GetDataTable(sql);

                // 构建缓存：业务字段值 -> 主键ID 的映射
                var fieldValueToIdMap = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow row in data.Rows)
                {
                    object id = row[primaryKeyField];
                    object fieldValue = row[fkConfig.ForeignFieldName];
                    if (fieldValue != DBNull.Value && fieldValue != null)
                    {
                        string key = fieldValue.ToString().Trim();
                        fieldValueToIdMap.TryAdd(key, id);
                    }
                }

                _foreignKeyCache.TryAdd(cacheKey, fieldValueToIdMap);
                Debug.WriteLine($"预加载外键数据成功: {fkConfig.ForeignTableName}.{fkConfig.ForeignFieldName} -> {primaryKeyField}, 共 {fieldValueToIdMap.Count} 条记录");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"预加载外键数据失败: {fkConfig.ForeignTableName}.{fkConfig.ForeignFieldName}, 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取表的主键字段名
        /// </summary>
        private string GetPrimaryKeyFieldName(string tableName)
        {
            try
            {
                // 使用 TableSchemaManager 获取表的主键信息
                var tableSchema = _tableSchemaManager.GetSchemaInfo(tableName);
                if (tableSchema.PrimaryKeyField != null)
                {
                    return tableSchema.PrimaryKeyField;
                }

                // 回退：常见主键字段名
                var commonPkNames = new[] { "ID", "Category_ID", "Vendor_ID", "Prod_ID" };
                foreach (var name in commonPkNames)
                {
                    try
                    {
                        var testSql = $"SELECT TOP 1 {name} FROM {tableName}";
                        _db.Ado.GetDataTable(testSql);
                        return name;
                    }
                    catch
                    {
                        // 字段不存在，继续尝试
                    }
                }

                return null;
            }
            catch
            {
                return null;
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
        /// 逻辑：从Excel列获取编码值 -> 在缓存中查找匹配数据行 -> 提取关联表主键ID -> 返回主键ID
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>外键主键ID值</returns>
        public object GetForeignKeyValue(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // 步骤1: 从Excel中获取外键来源列的值（编码/代码值）
                string sourceCodeValue = GetSourceCodeValueFromExcel(row, mapping,  out string sourceColumnDisplayName);
                
                // 如果没有获取到编码值，返回错误
                if (string.IsNullOrEmpty(sourceCodeValue))
                {
                    // 检查是否必填：通过 ExcelConfig 的 IgnoreEmptyValue 和 EmptyValueDefault 判断
                    var excelConfig = mapping.DataSourceConfig as ExcelConfig;
                    bool isRequired = excelConfig == null || 
                                     (!excelConfig.IgnoreEmptyValue && string.IsNullOrEmpty(excelConfig.EmptyValueDefault));
                    
                    if (isRequired)
                    {
                        errorMessage = $"行 {rowNumber} 字段 {mapping.SystemField?.Value} 是必填字段，但无法从列 '{sourceColumnDisplayName}' 获取有效的外键值";
                    }
                    return null;
                }

                // 步骤2: 从缓存中查找该编码值对应的主键ID
                var fkConfig = mapping.DataSourceConfig as DatabaseReferenceConfig;
                if (fkConfig != null &&
                    !string.IsNullOrEmpty(fkConfig.ForeignTableName) &&
                    !string.IsNullOrEmpty(fkConfig.ForeignFieldName) &&
                    fkConfig.ForeignKeySourceColumn != null &&
                    !string.IsNullOrEmpty(fkConfig.ForeignKeySourceColumn.Key))
                {
                    string tableName = fkConfig.ForeignTableName;
                    string relatedField = fkConfig.ForeignFieldName;
                    string sourceField = fkConfig.ForeignKeySourceColumn.Key;
                    
                    // 构建缓存键
                    string cacheKey = $"{tableName}_{relatedField}";
                    
                    // 从缓存获取 字段值 -> 主键ID 的映射
                    object foreignKeyId;
                    if (_foreignKeyCache.TryGetValue(cacheKey, out var fieldValueToIdMap))
                    {
                        string trimmedValue = sourceCodeValue.Trim();
                        if (fieldValueToIdMap.TryGetValue(trimmedValue, out foreignKeyId))
                        {
                            // 成功从缓存中获取到主键ID
                            return foreignKeyId;
                        }
                    }
                    
                    // 缓存未命中，返回错误
                    errorMessage = $"行 {rowNumber} 外键值 '{sourceCodeValue}' (来源列: {sourceColumnDisplayName}) " +
                        $"在关联表 {tableName} 中未找到对应记录。";

                    // 如果是供应商表，提供额外提示
                    if (tableName == "tb_CustomerVendor")
                    {
                        errorMessage += "\n\n提示：请确保{sourceColumnDisplayName}在供应商表中已存在，或者先导入供应商数据。";
                    }

                    return null;
                }

                // 如果配置不完整，返回null
                return null;
            }
            catch (Exception ex)
            {
                errorMessage = $"行 {rowNumber} 处理外键值时发生错误: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// 从Excel行中获取源代码值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="sourceColumnDisplayName">来源列显示名称</param>
        /// <returns>源代码值</returns>
        private string GetSourceCodeValueFromExcel(DataRow row, ColumnMapping mapping, out string sourceColumnDisplayName)
        {
            sourceColumnDisplayName = string.Empty;
            
            // 确定来源列名（优先级：外键来源列 > Excel列 > 系统字段列）
            string sourceColumnName = DetermineSourceColumnName(mapping, out sourceColumnDisplayName);

            // 从Excel数据行中获取值
            if (!string.IsNullOrEmpty(sourceColumnName) && row.Table.ContainsColumn(sourceColumnName))
            {
                return row[sourceColumnName]?.ToString()?.Trim();
            }

            return null;
        }

        /// <summary>
        /// 确定来源列名
        /// </summary>
        private string DetermineSourceColumnName(ColumnMapping mapping, out string displayName)
        {
            displayName = string.Empty;

            // 1. 优先从指定的外键来源列获取（Excel列）
            var fkConfig = mapping.DataSourceConfig as DatabaseReferenceConfig;
            if (fkConfig != null && fkConfig.ForeignKeySourceColumn != null && !string.IsNullOrEmpty(fkConfig.ForeignKeySourceColumn.Key))
            {
                displayName = fkConfig.ForeignKeySourceColumn.Value ?? fkConfig.ForeignKeySourceColumn.Key;
                return fkConfig.ForeignKeySourceColumn.Key;
            }
            
            // 2. 如果没有指定外键来源列，但映射有Excel列，使用映射的Excel列
            if (!string.IsNullOrEmpty(mapping.OriginalExcelColumn) &&
                !mapping.OriginalExcelColumn.StartsWith("[") &&
                !mapping.OriginalExcelColumn.StartsWith("("))
            {
                displayName = mapping.OriginalExcelColumn;
                return mapping.OriginalExcelColumn;
            }
            
            // 3. 尝试从系统字段列获取（映射后的列名）
            displayName = mapping.SystemField?.Value;
            return mapping.SystemField?.Value;
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

                // ✅ 缓存未命中，查询数据库 - 使用正确的主键字段名
                string primaryKeyField = GetPrimaryKeyFieldName(relatedTableName);
                if (string.IsNullOrEmpty(primaryKeyField))
                {
                    Debug.WriteLine($"查询外键ID失败: 无法获取表 {relatedTableName} 的主键字段名");
                    return null;
                }

                // 例如：SELECT Category_ID FROM tb_ProdCategories WHERE CategoryCode = 'BG'
                string sql = $"SELECT {primaryKeyField} FROM {relatedTableName} WHERE {relatedTableField} = @value";

                // 使用参数化查询防止SQL注入
                var parameters = new { value = trimmedValue };

                // 执行查询
                var result = _db.Ado.GetDataTable(sql, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    if (result.Rows.Count > 1)
                    {
                        Debug.WriteLine(
                            $"警告：外键查询返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}，匹配数：{result.Rows.Count}");
                    }

                    object id = result.Rows[0][primaryKeyField];
                    AddToCache(cacheKey, trimmedValue, id);
                    return id;
                }

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
        public void AddToCache(string cacheKey, string fieldValue, object id)
        {
            if (!_foreignKeyCache.ContainsKey(cacheKey))
            {
                _foreignKeyCache.TryAdd(cacheKey, new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase));
            }

            if (_foreignKeyCache.TryGetValue(cacheKey, out var fieldValueToIdMap))
            {
                fieldValueToIdMap.TryAdd(fieldValue.Trim(), id);
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


    }
}
