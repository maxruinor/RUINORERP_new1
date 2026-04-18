using RUINORERP.Business.ImportEngine.Models;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 外键缓存服务实现
    /// 负责外键值的获取和验证，支持预加载和缓存功能
    /// </summary>
    public class ForeignKeyCacheService : IForeignKeyCacheService
    {
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 外键数据缓存
        /// Key: 表名_字段名，Value: 字段值到主键ID的映射
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _foreignKeyCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">数据库客户端</param>
        public ForeignKeyCacheService(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _foreignKeyCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
        }

        /// <summary>
        /// 预加载外键数据
        /// 在导入前批量查询所有关联表数据，缓存起来用于外键解析
        /// </summary>
        /// <param name="foreignKeyConfigs">外键配置列表</param>
        public void PreloadForeignKeyData(IEnumerable<Models.ForeignKeyConfig> foreignKeyConfigs)
        {
            if (foreignKeyConfigs == null || !foreignKeyConfigs.Any())
            {
                return;
            }

            // 按表分组预加载数据
            var tableGroups = foreignKeyConfigs
                .GroupBy(c => c.TableName)
                .Where(g => !string.IsNullOrEmpty(g.Key));

            foreach (var group in tableGroups)
            {
                foreach (var config in group)
                {
                    PreloadSingleForeignKeyData(config);
                }
            }
        }

        /// <summary>
        /// 预加载单个外键的数据
        /// </summary>
        /// <param name="config">外键配置</param>
        private void PreloadSingleForeignKeyData(Models.ForeignKeyConfig config)
        {
            if (config == null) return;

            try
            {
                string tableName = config.TableName;
                string fieldName = config.KeyField;
                string sourceField = config.SourceField;

                if (string.IsNullOrEmpty(tableName) || 
                    string.IsNullOrEmpty(fieldName) || 
                    string.IsNullOrEmpty(sourceField))
                {
                    return;
                }

                string cacheKey = $"{tableName}_{fieldName}";
                
                // 检查是否已缓存
                if (_foreignKeyCache.ContainsKey(cacheKey))
                {
                    return;
                }

                // 构建查询SQL：查询主键ID、显示字段、来源字段
                string sql = $"SELECT [{fieldName}], [{sourceField}] FROM [{tableName}]";
                var data = _db.Ado.GetDataTable(sql);

                // 构建缓存：字段值 -> 主键ID
                var fieldValueToIdMap = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow row in data.Rows)
                {
                    object id = row[fieldName];
                    object fieldValue = row[sourceField];
                    
                    if (fieldValue != DBNull.Value && fieldValue != null)
                    {
                        string key = fieldValue.ToString().Trim();
                        fieldValueToIdMap.TryAdd(key, id);
                    }
                }

                _foreignKeyCache.TryAdd(cacheKey, fieldValueToIdMap);
                
                Debug.WriteLine($"成功预加载外键数据: {tableName}.{fieldName}, 缓存记录数: {fieldValueToIdMap.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"预加载外键数据失败: {config.TableName}.{config.KeyField}, 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取外键值
        /// 从缓存中查找外键对应的ID
        /// </summary>
        /// <param name="tableName">外键表名</param>
        /// <param name="fieldValue">字段值（如供应商名称）</param>
        /// <param name="errorMessage">错误信息输出</param>
        /// <returns>外键ID，如果未找到返回null</returns>
        public object GetForeignKeyValue(string tableName, string fieldValue, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(fieldValue))
                {
                    errorMessage = "表名或字段值不能为空";
                    return null;
                }

                string trimmedValue = fieldValue.Trim();

                // 注意：这里需要知道具体的字段名才能构建cacheKey
                // 在实际使用中，应该通过mapping来获取完整的cacheKey
                // 这个方法主要用于简单场景
                
                // 尝试遍历所有缓存键，查找匹配的表
                foreach (var kvp in _foreignKeyCache)
                {
                    if (kvp.Key.StartsWith($"{tableName}_"))
                    {
                        if (kvp.Value.TryGetValue(trimmedValue, out var foreignKeyId))
                        {
                            return foreignKeyId;
                        }
                    }
                }

                // 缓存未命中
                errorMessage = $"外键值 '{fieldValue}' 在表 {tableName} 中未找到对应记录";
                return null;
            }
            catch (Exception ex)
            {
                errorMessage = $"获取外键值时发生错误: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearCache()
        {
            _foreignKeyCache.Clear();
        }

        /// <summary>
        /// 清除指定表的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ClearCache(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return;
            }

            var keysToRemove = _foreignKeyCache.Keys
                .Where(k => k.StartsWith($"{tableName}_"))
                .ToList();

            foreach (var key in keysToRemove)
            {
                _foreignKeyCache.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// 获取外键ID（完整版本，支持指定字段名）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>外键ID</returns>
        public object GetForeignKeyId(string tableName, string fieldName, string fieldValue, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(tableName) || 
                    string.IsNullOrEmpty(fieldName) || 
                    string.IsNullOrEmpty(fieldValue))
                {
                    errorMessage = "参数不能为空";
                    return null;
                }

                string trimmedValue = fieldValue.Trim();
                string cacheKey = $"{tableName}_{fieldName}";

                // 尝试从缓存中获取
                if (_foreignKeyCache.TryGetValue(cacheKey, out var fieldValueToIdMap))
                {
                    if (fieldValueToIdMap.TryGetValue(trimmedValue, out var cachedId))
                    {
                        Debug.WriteLine($"从缓存中获取外键ID: {tableName}.{fieldName} = {trimmedValue} -> {cachedId}");
                        return cachedId;
                    }
                }

                // 缓存未命中，查询数据库
                string sql = $"SELECT ID FROM [{tableName}] WHERE [{fieldName}] = @value";
                var parameters = new { value = trimmedValue };
                var result = _db.Ado.GetDataTable(sql, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    // 检查唯一性
                    if (result.Rows.Count > 1)
                    {
                        Debug.WriteLine($"警告：外键查询返回多个结果。表：{tableName}，字段：{fieldName}，值：{fieldValue}，匹配数：{result.Rows.Count}");
                    }

                    object id = result.Rows[0]["ID"];
                    
                    // 添加到缓存
                    AddToCache(cacheKey, trimmedValue, id);
                    
                    return id;
                }

                // 尝试模糊匹配（针对名称字段）
                if (fieldName.ToLower().Contains("name") ||
                    fieldName.ToLower().Contains("vendor") ||
                    fieldName.ToLower().Contains("supplier"))
                {
                    sql = $"SELECT ID FROM [{tableName}] WHERE [{fieldName}] LIKE @value";
                    parameters = new { value = $"%{trimmedValue}%" };
                    result = _db.Ado.GetDataTable(sql, parameters);

                    if (result != null && result.Rows.Count > 0)
                    {
                        if (result.Rows.Count > 1)
                        {
                            Debug.WriteLine($"警告：模糊匹配返回多个结果。表：{tableName}，字段：{fieldName}，值：{fieldValue}");
                        }
                        
                        object id = result.Rows[0]["ID"];
                        // 模糊匹配结果不缓存，因为可能不准确
                        return id;
                    }
                }

                errorMessage = $"外键值 '{fieldValue}' 在表 {tableName}.{fieldName} 中未找到";
                return null;
            }
            catch (Exception ex)
            {
                errorMessage = $"查询外键ID失败: {ex.Message}";
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
    }
}
