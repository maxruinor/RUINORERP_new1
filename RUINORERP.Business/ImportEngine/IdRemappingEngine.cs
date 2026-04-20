using RUINORERP.Common.Helper;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    /// <summary>
    /// ID 重映射引擎 - 解决不同实例间数据导入时的 ID 冲突与外键引用问题
    /// </summary>
    public class IdRemappingEngine
    {
        private readonly ISqlSugarClient _db;
        
        // 全局缓存：表名 -> (逻辑键值 -> 物理ID)
        // 例如：{ "tb_CustomerVendor": { "华为": 123456... } }
        private Dictionary<string, Dictionary<string, long>> _globalLogicalKeyCache = new Dictionary<string, Dictionary<string, long>>();
        
        // 记录新生成的 ID 映射（旧 ID -> 新 ID）
        public Dictionary<long, long> IdMapping { get; } = new Dictionary<long, long>();
        
        // 表名 + 逻辑键值 -> 物理ID 的映射
        private Dictionary<string, Dictionary<string, long>> _tableKeyToIdMap = new Dictionary<string, Dictionary<string, long>>();

        public IdRemappingEngine(ISqlSugarClient db)
        {
            _db = db;
        }

        /// <summary>
        /// 注册映射关系(供DatabaseWriterService调用)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="logicalKey">逻辑键值</param>
        /// <param name="physicalId">物理ID</param>
        public void RegisterMapping(string tableName, string logicalKey, long physicalId)
        {
            if (!_tableKeyToIdMap.ContainsKey(tableName))
            {
                _tableKeyToIdMap[tableName] = new Dictionary<string, long>();
            }
            _tableKeyToIdMap[tableName][logicalKey] = physicalId;
        }
        
        /// <summary>
        /// 注册新生成的ID(用于子表引用)
        /// </summary>
        public void RegisterNewId(string tableName, string businessKeyValue, long newId)
        {
            if (!_tableKeyToIdMap.ContainsKey(tableName))
                _tableKeyToIdMap[tableName] = new Dictionary<string, long>();
                    
            _tableKeyToIdMap[tableName][businessKeyValue] = newId;
                    
            // 同时更新全局缓存
            CacheLogicalKey(tableName, businessKeyValue, newId);
        }

        /// <summary>
        /// 获取新生成的ID（供DataSplitterService调用）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="logicalKeyValue">逻辑键值</param>
        /// <returns>物理ID，如果未找到返回null</returns>
        public object GetNewId(string tableName, object logicalKeyValue)
        {
            if (logicalKeyValue == null) return null;
            
            string key = logicalKeyValue.ToString();
            if (_tableKeyToIdMap.ContainsKey(tableName) && 
                _tableKeyToIdMap[tableName].ContainsKey(key))
            {
                return _tableKeyToIdMap[tableName][key];
            }
            
            return null;
        }

        /// <summary>
        /// 预处理一组实体，完成 ID 分配和外键修正
        /// </summary>
        public async Task ProcessEntitiesAsync<T>(List<T> entities) where T : BaseEntity, new()
        {
            if (entities == null || !entities.Any()) return;

            var entityType = typeof(T);
            var tableName = entityType.GetCustomAttribute<SugarTable>()?.TableName ?? entityType.Name;
            var logicalKeyName = entities[0].LogicalKeyPropertyName;
            
            // 1. 预扫描：从目标库获取已存在的逻辑主键映射并存入全局缓存
            await LoadExistingLogicalKeysAsync<T>(entities, logicalKeyName, tableName);

            // 2. 遍历实体进行 ID 分配和引用修正
            foreach (var entity in entities)
            {
                AssignOrMapId(entity, logicalKeyName, tableName);
                
                // 【核心改进】修正外键引用（通过名称查找 ID）
                await ResolveForeignKeysByNameAsync(entity);
            }

            // 3. 处理主子表关联
            await ProcessChildEntitiesAsync(entities, tableName);
        }

        /// <summary>
        /// 通过外键名称解析并替换为物理 ID
        /// </summary>
        private async Task ResolveForeignKeysByNameAsync<T>(T entity) where T : BaseEntity
        {
            var fkRelations = entity.ImportFKRelations;
            foreach (var fk in fkRelations)
            {
                var currentFkValue = ReflectionHelper.GetPropertyValue(entity, fk.PropertyName);
                
                // 1. 如果当前值是字符串（名称），则尝试转换为 ID
                if (currentFkValue is string fkName && !string.IsNullOrEmpty(fkName))
                {
                    long resolvedId = await GetIdByLogicalKeyAsync(fk.FKTableName, fkName);
                    if (resolvedId > 0)
                    {
                        ReflectionHelper.SetPropertyValue(entity, fk.PropertyName, resolvedId);
                    }
                    else
                    {
                        // 【容错反馈】记录未匹配到的外键名称，方便用户排查
                        //Instance?.PrintInfoLog($"警告：无法在外键表 [{fk.FKTableName}] 中找到名为 '{fkName}' 的记录。");
                    }
                }
                // 2. 如果当前值是 ID，但属于本次导入中新生成的 ID，则进行映射替换
                else if (currentFkValue != null)
                {
                    var fkId = Convert.ToInt64(currentFkValue);
                    if (IdMapping.ContainsKey(fkId))
                    {
                        ReflectionHelper.SetPropertyValue(entity, fk.PropertyName, IdMapping[fkId]);
                    }
                }
            }
        }

        /// <summary>
        /// 根据表名和逻辑键值获取物理 ID
        /// </summary>
        private async Task<long> GetIdByLogicalKeyAsync(string tableName, string logicalValue)
        {
            // 1. 检查内存缓存
            if (_globalLogicalKeyCache.ContainsKey(tableName))
            {
                if (_globalLogicalKeyCache[tableName].TryGetValue(logicalValue, out long id))
                {
                    return id;
                }
            }

            // 2. 【新增】缓存未命中时，尝试从数据库动态加载该名称对应的 ID
            try
            {
                // 动态获取目标表的逻辑主键列名
                string logicalKeyCol = "ID";
                var assembly = System.Reflection.Assembly.Load("RUINORERP.Model");
                var targetType = assembly.GetTypes().FirstOrDefault(t => 
                    t.GetCustomAttribute<SqlSugar.SugarTable>()?.TableName == tableName);
                
                if (targetType != null)
                {
                    var instance = Activator.CreateInstance(targetType) as BaseEntity;
                    if (instance != null)
                    {
                        logicalKeyCol = instance.LogicalKeyPropertyName ?? "ID";
                    }
                }

                var sql = $"SELECT TOP 1 ID FROM [{tableName}] WHERE [{logicalKeyCol}] = @val";
                var result = await _db.Ado.GetScalarAsync(sql, new { val = logicalValue });
                
                if (result != null && long.TryParse(result.ToString(), out long dbId))
                {
                    if (!_globalLogicalKeyCache.ContainsKey(tableName))
                        _globalLogicalKeyCache[tableName] = new Dictionary<string, long>();
                    
                    _globalLogicalKeyCache[tableName][logicalValue] = dbId;
                    return dbId;
                }
            }
            catch { /* 忽略动态查询错误 */ }

            return 0;
        }

        /// <summary>
        /// 预加载依赖表数据到缓存
        /// </summary>
        public async Task PreloadDependencyTablesAsync(List<RUINORERP.Model.ImportEngine.Models.ImportProfile> dependencyProfiles)
        {
            foreach (var profile in dependencyProfiles)
            {
                await LoadExistingLogicalKeysByProfileAsync(profile);
            }
        }

        /// <summary>
        /// 根据Profile配置加载已存在的逻辑键
        /// </summary>
        private async Task LoadExistingLogicalKeysByProfileAsync(RUINORERP.Model.ImportEngine.Models.ImportProfile profile)
        {
            if (profile == null || profile.BusinessKeys == null || !profile.BusinessKeys.Any())
                return;

            string tableName = profile.TargetTable;
            if (string.IsNullOrEmpty(tableName))
                return;

            // 构建查询条件
            var whereConditions = profile.BusinessKeys.Select(k => $"[{k}] IS NOT NULL").ToList();
            var whereClause = string.Join(" AND ", whereConditions);

            // 查询已存在的记录
            var sql = $"SELECT {string.Join(", ", profile.BusinessKeys.Select(k => $"[{k}]"))}, [ID] FROM [{tableName}] WHERE {whereClause}";
            var dataTable = await _db.Ado.GetDataTableAsync(sql);

            if (!_globalLogicalKeyCache.ContainsKey(tableName))
            {
                _globalLogicalKeyCache[tableName] = new Dictionary<string, long>();
            }

            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                // 生成组合键
                var keyParts = profile.BusinessKeys.Select(k => row[k]?.ToString()).ToList();
                string logicalKey = string.Join("|", keyParts.Where(v => v != null));

                if (!string.IsNullOrEmpty(logicalKey) && row["ID"] != null)
                {
                    long physicalId = Convert.ToInt64(row["ID"]);
                    _globalLogicalKeyCache[tableName][logicalKey] = physicalId;
                }
            }
        }

        /// <summary>
        /// 缓存逻辑键映射
        /// </summary>
        private void CacheLogicalKey(string tableName, string logicalKey, long physicalId)
        {
            if (!_globalLogicalKeyCache.ContainsKey(tableName))
            {
                _globalLogicalKeyCache[tableName] = new Dictionary<string, long>();
            }
            _globalLogicalKeyCache[tableName][logicalKey] = physicalId;
        }

        /// <summary>
        /// 获取或创建外键ID(混合来源解析核心逻辑)
        /// </summary>
        /// <param name="tableName">外键表名</param>
        /// <param name="businessKeyName">业务键字段名</param>
        /// <param name="businessKeyValue">业务键值(如供应商名称)</param>
        /// <param name="sourceRow">源数据行(用于自动创建时提取其他字段)</param>
        /// <param name="tableProfile">表配置(用于自动创建)</param>
        /// <returns>物理ID</returns>
        public async Task<long> GetOrCreateForeignKeyIdAsync(
            string tableName,
            string businessKeyName,
            string businessKeyValue,
            System.Data.DataRow sourceRow,
            RUINORERP.Model.ImportEngine.Models.ImportProfile tableProfile)
        {
            if (string.IsNullOrEmpty(businessKeyValue))
            {
                throw new Exception($"外键业务键值不能为空");
            }

            // 1. 尝试从缓存获取
            if (_globalLogicalKeyCache.ContainsKey(tableName) &&
                _globalLogicalKeyCache[tableName].TryGetValue(businessKeyValue, out long cachedId))
            {
                return cachedId;
            }

            // 2. 查询数据库
            var sql = $"SELECT TOP 1 [ID] FROM [{tableName}] WHERE [{businessKeyName}] = @val";
            var result = await _db.Ado.GetScalarAsync(sql, new { val = businessKeyValue });

            if (result != null && long.TryParse(result.ToString(), out long dbId))
            {
                // 缓存并返回
                CacheLogicalKey(tableName, businessKeyValue, dbId);
                return dbId;
            }

            // 3. 如果允许自动创建,则插入新记录
            if (tableProfile != null && ShouldAutoCreateForProfile(tableProfile))
            {
                return await CreateNewRecordAndReturnIdAsync(tableName, businessKeyName, businessKeyValue, sourceRow, tableProfile);
            }

            throw new Exception($"外键值 '{businessKeyValue}' 在表 {tableName} 中不存在且不允许自动创建");
        }

        /// <summary>
        /// 判断是否允许自动创建
        /// </summary>
        private bool ShouldAutoCreateForProfile(RUINORERP.Model.ImportEngine.Models.ImportProfile profile)
        {
            // 检查ColumnMappings中是否有ForeignKeyConfig配置了AutoCreateIfNotExists
            if (profile.ColumnMappings != null)
            {
                foreach (var mapping in profile.ColumnMappings)
                {
                    if (mapping.ForeignConfig != null && mapping.ForeignConfig.AutoCreateIfNotExists)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 创建新记录并返回ID
        /// </summary>
        private async Task<long> CreateNewRecordAndReturnIdAsync(
            string tableName,
            string businessKeyName,
            string businessKeyValue,
            System.Data.DataRow sourceRow,
            RUINORERP.Model.ImportEngine.Models.ImportProfile tableProfile)
        {
            // 构建INSERT语句
            var columnsToInsert = new List<string>();
            var parameters = new List<SqlSugar.SugarParameter>();

            // 添加业务键字段
            columnsToInsert.Add($"[{businessKeyName}]");
            parameters.Add(new SqlSugar.SugarParameter($"@{businessKeyName}", businessKeyValue));

            // 添加其他必填字段(从sourceRow中提取)
            if (tableProfile.ColumnMappings != null)
            {
                foreach (var mapping in tableProfile.ColumnMappings)
                {
                    // 跳过已经添加的业务键
                    if (mapping.DbColumn == businessKeyName)
                        continue;

                    // 只处理必填且有默认值的字段
                    if (mapping.IsRequired && sourceRow.Table.Columns.Contains(mapping.ExcelHeader))
                    {
                        var value = sourceRow[mapping.ExcelHeader];
                        if (value != null && value != DBNull.Value)
                        {
                            columnsToInsert.Add($"[{mapping.DbColumn}]");
                            parameters.Add(new SqlSugar.SugarParameter($"@{mapping.DbColumn}", value));
                        }
                    }
                }
            }

            var columnNames = string.Join(", ", columnsToInsert);
            var paramNames = string.Join(", ", columnsToInsert.Select(c => c.Replace("[", "@").Replace("]", "")));

            var insertSql = $"INSERT INTO [{tableName}] ({columnNames}) VALUES ({paramNames}); SELECT SCOPE_IDENTITY() AS NewId;";

            var newId = await _db.Ado.GetScalarAsync(insertSql, parameters.ToArray());

            if (newId != null && long.TryParse(newId.ToString(), out long physicalId))
            {
                // 注册新生成的ID
                RegisterNewId(tableName, businessKeyValue, physicalId);
                return physicalId;
            }

            throw new Exception($"创建表 {tableName} 的新记录失败");
        }

        private async Task ProcessChildEntitiesAsync<T>(List<T> parentEntities, string parentTableName) where T : BaseEntity
        {
            foreach (var parent in parentEntities)
            {
                var navProps = parent.GetType().GetProperties()
                    .Where(p => p.GetCustomAttribute<SqlSugar.Navigate>() != null && 
                                p.PropertyType.IsGenericType && 
                                p.PropertyType.GetGenericTypeDefinition() == typeof(List<>));

                foreach (var navProp in navProps)
                {
                    var childList = navProp.GetValue(parent) as System.Collections.IList;
                    if (childList != null)
                    {
                        foreach (var child in childList)
                        {
                            if (child is BaseEntity childEntity)
                            {
                                // 递归处理子实体的 ID 和外键
                                var childLogicalKey = childEntity.LogicalKeyPropertyName;
                                if (!string.IsNullOrEmpty(childLogicalKey))
                                {
                                    await ProcessSingleChildAsync(childEntity, parent, parentTableName);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task ProcessSingleChildAsync(BaseEntity child, BaseEntity parent, string parentTableName)
        {
            var pkName = child.GetPrimaryKeyColName();
            var currentId = Convert.ToInt64(ReflectionHelper.GetPropertyValue(child, pkName) ?? 0);
            
            if (currentId == 0)
            {
                var newId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                ReflectionHelper.SetPropertyValue(child, pkName, newId);
            }

            // 尝试寻找指向父级的 FK 并修正
            foreach (var fk in child.ImportFKRelations)
            {
                // 简单启发式：如果 FK 的表名与父类表名一致，则赋值父类 ID
                var parentTable = parent.GetType().GetCustomAttribute<SqlSugar.SugarTable>()?.TableName;
                if (fk.FKTableName == parentTable)
                {
                    var parentId = ReflectionHelper.GetPropertyValue(parent, parent.GetPrimaryKeyColName());
                    ReflectionHelper.SetPropertyValue(child, fk.PropertyName, parentId);
                }
            }
        }

        /// <summary>
        /// 加载目标库中已存在的逻辑主键映射
        /// </summary>
        private async Task LoadExistingLogicalKeysAsync<T>(List<T> entities, string logicalKeyName, string tableName) where T : BaseEntity, new()
        {
            if (!_globalLogicalKeyCache.ContainsKey(tableName))
            {
                var logicalValues = entities.Select(e => ReflectionHelper.GetPropertyValue(e, logicalKeyName)?.ToString()).Where(v => !string.IsNullOrEmpty(v)).Distinct().ToList();
                
                if (logicalValues.Any())
                {
                    // 动态查询目标库
                    // 获取主键列名
                    var sampleEntity = new T();
                    string pkColumnName = sampleEntity.GetPrimaryKeyColName();
                    
                    var existingRecords = await _db.Queryable<T>()
                        .Where($"[{logicalKeyName}] IN (@values)", new { values = logicalValues })
                        .Select($"{logicalKeyName}, {pkColumnName}")
                        .ToListAsync();

                    var map = new Dictionary<string, long>();
                    foreach (var record in existingRecords)
                    {
                        var keyVal = ReflectionHelper.GetPropertyValue(record, logicalKeyName)?.ToString();
                        var idVal = ReflectionHelper.GetPropertyValue(record, pkColumnName);
                        if (!string.IsNullOrEmpty(keyVal) && idVal != null)
                        {
                            map[keyVal] = Convert.ToInt64(idVal);
                        }
                    }
                    _globalLogicalKeyCache[tableName] = map;
                }
                else
                {
                    _globalLogicalKeyCache[tableName] = new Dictionary<string, long>();
                }
            }
        }

        /// <summary>
        /// 为实体分配 ID 或建立映射
        /// </summary>
        private void AssignOrMapId<T>(T entity, string logicalKeyName, string tableName) where T : BaseEntity
        {
            var logicalKeyValue = ReflectionHelper.GetPropertyValue(entity, logicalKeyName)?.ToString();
            if (string.IsNullOrEmpty(logicalKeyValue))
            {
                // 如果没有逻辑主键值，直接生成新 ID
                var pkName = entity.GetPrimaryKeyColName();
                var newId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                ReflectionHelper.SetPropertyValue(entity, pkName, newId);
                return;
            }

            var pkName2 = entity.GetPrimaryKeyColName();
            var currentId = Convert.ToInt64(ReflectionHelper.GetPropertyValue(entity, pkName2) ?? 0);

            if (_globalLogicalKeyCache[tableName].ContainsKey(logicalKeyValue))
            {
                // 目标库已存在，使用现有 ID
                var existingId = _globalLogicalKeyCache[tableName][logicalKeyValue];
                ReflectionHelper.SetPropertyValue(entity, pkName2, existingId);
                if (currentId != 0 && currentId != existingId)
                {
                    IdMapping[currentId] = existingId;
                }
            }
            else
            {
                // 目标库不存在，生成新 ID
                var newId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                ReflectionHelper.SetPropertyValue(entity, pkName2, newId);
                _globalLogicalKeyCache[tableName][logicalKeyValue] = newId;
                
                // 记录旧 ID 到新 ID 的映射（如果旧 ID 不为 0）
                if (currentId != 0)
                {
                    IdMapping[currentId] = newId;
                }
            }
        }

        /// <summary>
        /// 修正实体的外键引用
        /// </summary>
        private async Task CorrectForeignKeysAsync<T>(T entity) where T : BaseEntity
        {
            var fkRelations = entity.ImportFKRelations;
            foreach (var fk in fkRelations)
            {
                var fkValue = ReflectionHelper.GetPropertyValue(entity, fk.PropertyName);
                if (fkValue != null)
                {
                    var fkId = Convert.ToInt64(fkValue);
                    if (IdMapping.ContainsKey(fkId))
                    {
                        // 如果该外键指向的实体在本次导入中被重新分配了 ID，则更新引用
                        ReflectionHelper.SetPropertyValue(entity, fk.PropertyName, IdMapping[fkId]);
                    }
                    else
                    {
                        // 尝试通过逻辑主键查找目标表 ID（跨表引用修正）
                        // 这一步比较复杂，通常需要在 ProcessEntitiesAsync 中按依赖顺序调用
                    }
                }
            }
        }
    }
}
