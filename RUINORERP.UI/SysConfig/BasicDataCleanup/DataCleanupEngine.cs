using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using RUINORERP.Business;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using System.Reflection;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 实体元数据帮助类 - 从 Model 实体中提取元数据（主键、外键、依赖关系）
    /// </summary>
    public static class ModelMetadataHelper
    {
        private static readonly Dictionary<string, EntityMetadata> _cache = new();

        /// <summary>
        /// 获取实体的元数据信息
        /// </summary>
        public static EntityMetadata GetMetadata<T>() where T : BaseEntity, new()
        {
            var type = typeof(T);
            return GetMetadata(type);
        }

        /// <summary>
        /// 根据类型获取实体元数据
        /// </summary>
        public static EntityMetadata GetMetadata(Type entityType)
        {
            if (_cache.TryGetValue(entityType.FullName, out var cached)) return cached;

            var metadata = ExtractMetadata(entityType);
            _cache[entityType.FullName] = metadata;
            return metadata;
        }

        /// <summary>
        /// 根据实体名称获取元数据（优先从缓存，其次从数据库查询）
        /// </summary>
        public static async Task<EntityMetadata> GetMetadataAsync(ISqlSugarClient db, string tableName)
        {
            // 1. 先尝试从实体程序集加载
            var cacheKey = $"RUINORERP.Model.{tableName}";
            if (_cache.TryGetValue(cacheKey, out var cached)) return cached;

            try
            {
                var assembly = Assembly.Load("RUINORERP.Model");
                var entityType = assembly.GetTypes().FirstOrDefault(t => 
                    t.Name == tableName && typeof(BaseEntity).IsAssignableFrom(t));

                if (entityType != null)
                {
                    return GetMetadata(entityType);
                }
            }
            catch
            {
                // 实体不存在，忽略
            }

            // 2. 实体不存在，从数据库查询表结构
            return await GetMetadataFromDatabaseAsync(db, tableName);
        }

        /// <summary>
        /// 从数据库查询表的元数据（主键列名）
        /// </summary>
        private static async Task<EntityMetadata> GetMetadataFromDatabaseAsync(ISqlSugarClient db, string tableName)
        {
            var cacheKey = $"DB.{tableName}";
            if (_cache.TryGetValue(cacheKey, out var cached)) return cached;

            try
            {
                // 查询主键列名
                var sql = @"
                    SELECT c.COLUMN_NAME
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE c 
                        ON tc.CONSTRAINT_NAME = c.CONSTRAINT_NAME
                    WHERE tc.TABLE_NAME = @tableName 
                      AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY'";

                var primaryKeyColumn = await db.Ado.GetStringAsync(sql, new { tableName = tableName });

                var metadata = new EntityMetadata
                {
                    TableName = tableName,
                    PrimaryKeyName = primaryKeyColumn ?? "PrimaryKeyID"
                };

                _cache[cacheKey] = metadata;
                return metadata;
            }
            catch (Exception ex)
            {
                // 查询失败，默认使用 PrimaryKeyID
                var metadata = new EntityMetadata
                {
                    TableName = tableName,
                    PrimaryKeyName = "PrimaryKeyID"
                };

                _cache[cacheKey] = metadata;
                return metadata;
            }
        }

        /// <summary>
        /// 提取实体元数据
        /// </summary>
        private static EntityMetadata ExtractMetadata(Type type)
        {
            var metadata = new EntityMetadata
            {
                EntityType = type,
                EntityName = type.Name
            };

            // 1. 获取表名
            var tableAttr = type.GetCustomAttribute<SugarTable>();
            metadata.TableName = tableAttr?.TableName ?? type.Name;

            // 2. 找主键：优先使用 IsPrimaryKey = true
            var pkProp = type.GetProperties().FirstOrDefault(p =>
                p.GetCustomAttribute<SugarColumn>()?.IsPrimaryKey == true);

            if (pkProp == null)
            {
                pkProp = type.GetProperty("PrimaryKeyID");
            }

            if (pkProp != null)
            {
                var sugarColumn = pkProp.GetCustomAttribute<SugarColumn>();
                metadata.PrimaryKeyName = sugarColumn?.ColumnName ?? pkProp.Name;
                metadata.PrimaryKeyProperty = pkProp.Name;
            }

            if (string.IsNullOrEmpty(metadata.PrimaryKeyName))
                throw new Exception($"实体 {type.Name} 未找到有效主键！");

            // 3. 找所有引用该表的外键（通过 Navigate OneToMany）
            // OneToMany 表示当前表是"父表"，导航属性指向的表是"子表"
            foreach (var prop in type.GetProperties())
            {
                var navAttr = prop.GetCustomAttribute<Navigate>();
                if (navAttr != null && navAttr.GetNavigateType() == NavigateType.OneToMany)
                {
                    var childType = prop.PropertyType.IsGenericType
                        ? prop.PropertyType.GetGenericArguments()[0]
                        : prop.PropertyType;

                    var fkColName = navAttr.GetName();

                    metadata.ChildRelations.Add(new ChildRelationInfo
                    {
                        ChildTableName = childType.Name,
                        ForeignKeyColumn = fkColName,
                        NavigationProperty = prop.Name
                    });
                }
            }

            return metadata;
        }

        /// <summary>
        /// 递归获取所有下游依赖（包括多层级的子表）
        /// </summary>
        public static List<DependencyNode> GetAllDependencies<T>(int maxDepth = 10) where T : BaseEntity, new()
        {
            var rootMetadata = GetMetadata<T>();
            return GetAllDependenciesRecursive(rootMetadata.EntityName, maxDepth, new Dictionary<string, bool>());
        }

        /// <summary>
        /// 递归获取依赖节点
        /// </summary>
        private static List<DependencyNode> GetAllDependenciesRecursive(
            string entityName,
            int maxDepth,
            Dictionary<string, bool> visited)
        {
            var result = new List<DependencyNode>();

            if (maxDepth <= 0 || visited.ContainsKey(entityName))
                return result;

            visited[entityName] = true;

            try
            {
                var assembly = Assembly.Load("RUINORERP.Model");
                var entityType = assembly.GetTypes().FirstOrDefault(t => t.Name == entityName && typeof(BaseEntity).IsAssignableFrom(t));

                if (entityType == null)
                    return result;

                var metadata = GetMetadata(entityType);

                foreach (var childRelation in metadata.ChildRelations)
                {
                    var node = new DependencyNode
                    {
                        TableName = childRelation.ChildTableName,
                        ForeignKeyColumn = childRelation.ForeignKeyColumn,
                        ParentTableName = entityName,
                        Depth = maxDepth
                    };

                    result.Add(node);

                    var subDeps = GetAllDependenciesRecursive(childRelation.ChildTableName, maxDepth - 1, visited);
                    result.AddRange(subDeps);
                }
            }
            catch
            {
                // 忽略无法加载的类型
            }

            return result;
        }
    }

    /// <summary>
    /// 依赖节点（用于拓扑排序）
    /// </summary>
    public class DependencyNode
    {
        public string TableName { get; set; }
        public string ForeignKeyColumn { get; set; }
        public string ParentTableName { get; set; }
        public int Depth { get; set; }
    }

    /// <summary>
    /// 数据库外键信息（从 sys.foreign_keys 查询）
    /// </summary>
    public class DbForeignKeyInfo
    {
        /// <summary>
        /// 外键名称
        /// </summary>
        public string ForeignKeyName { get; set; }
        
        /// <summary>
        /// 引用方表名（子表，有外键的表）
        /// </summary>
        public string ReferencingTableName { get; set; }
        
        /// <summary>
        /// 引用方外键列名
        /// </summary>
        public string ReferencingColumnName { get; set; }
        
        /// <summary>
        /// 被引用方表名（父表，主键所在的表）
        /// </summary>
        public string ReferencedTableName { get; set; }
        
        /// <summary>
        /// 被引用方主键列名
        /// </summary>
        public string ReferencedColumnName { get; set; }
    }

    /// <summary>
    /// 数据库表删除信息
    /// </summary>
    public class DbTableDeleteInfo
    {
        public string TableName { get; set; }
        public string ForeignKeyColumn { get; set; }
    }

    /// <summary>
    /// 数据清理引擎 - 基于实体元数据的级联删除
    /// </summary>
    public class DataCleanupEngine
    {
        private readonly ILogger<DataCleanupEngine> _logger;
        private readonly IUnitOfWorkManage _unitOfWork;
        
        public DataCleanupEngine()
        {
            _logger = Startup.GetFromFac<ILogger<DataCleanupEngine>>();
            _unitOfWork = Startup.GetFromFac<IUnitOfWorkManage>();
        }

        /// <summary>
        /// 获取数据库客户端（供外部使用）
        /// </summary>
        public ISqlSugarClient GetDbClient()
        {
            return _unitOfWork?.GetDbClient();
        }
        
        /// <summary>
        /// 执行级联删除 - 数据库元数据模式（直接从数据库获取外键关系）
        /// </summary>
        public async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync<T>(List<long> targetIds, bool isTestMode = false) where T : BaseEntity, new()
        {
            return await ExecuteCascadeDeleteByDbAsync<T>(targetIds, isTestMode);
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        private async Task<int> ExecuteDeleteAsync(
            string tableName,
            string pkColumn,
            List<long> ids,
            bool isTestMode)
        {
            var db = _unitOfWork.GetDbClient();
            
            Log($"[删除] 表: {tableName}, 主键: {pkColumn}, ID数: {ids.Count}, 测试模式: {isTestMode}");

            if (isTestMode)
            {
                var count = await db.Queryable<object>()
                    .AS(tableName)
                    .Where($"[{pkColumn}] IN (@ids)", new { ids = ids })
                    .CountAsync();
                return count;
            }
            else
            {
                return await db.Deleteable<object>()
                    .AS(tableName)
                    .Where($"[{pkColumn}] IN (@ids)", new { ids = ids })
                    .ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 解析 SQL Server 外键约束错误信息
        /// </summary>
        private (string TableName, string ColumnName)? ParseForeignKeyError(string errorMessage)
        {
            var tableMatch = System.Text.RegularExpressions.Regex.Match(errorMessage, @"表\s*[""'`]?(?<table>[\w\.]+)[""'`]?");
            var colMatch = System.Text.RegularExpressions.Regex.Match(errorMessage, @"column\s*['""](?<col>[^'""]+)['""]|字段\s*['""](?<col2>[^'""]+)['""]");

            if (tableMatch.Success && colMatch.Success)
            {
                string tableName = tableMatch.Groups["table"].Value;
                if (tableName.Contains(".")) tableName = tableName.Split('.')[1];

                string columnName = !string.IsNullOrEmpty(colMatch.Groups["col"].Value)
                    ? colMatch.Groups["col"].Value
                    : colMatch.Groups["col2"].Value;
                return (tableName, columnName);
            }
            return null;
        }

        /// <summary>
        /// 执行级联删除 - 数据库元数据模式（直接从数据库获取外键关系）
        /// </summary>
        private async Task<CascadeDeleteResult> ExecuteCascadeDeleteByDbAsync<T>(List<long> targetIds, bool isTestMode) where T : BaseEntity, new()
        {
            var result = new CascadeDeleteResult { IsTestMode = isTestMode, StartTime = DateTime.Now };
            if (!targetIds.Any()) return result;

            var db = _unitOfWork.GetDbClient();
            var rootMetadata = ModelMetadataHelper.GetMetadata<T>();
            var rootTableName = rootMetadata.TableName;
            var rootPkColumn = rootMetadata.PrimaryKeyName;

            Log($"[启动] 数据库元数据模式级联删除：{rootTableName}, ID数：{targetIds.Count}, 模式：{(isTestMode ? "测试" : "正式")}");

            try
            {
                await _unitOfWork.BeginTranAsync();

                try
                {
                    // 1. 递归查找所有依赖表（从数据库查询外键关系）
                    var allDependencies = new Dictionary<string, List<DbForeignKeyInfo>>();
                    await CollectAllForeignKeysRecursiveAsync(rootTableName, allDependencies, new HashSet<string>());

                    Log($"[依赖分析] 发现 {allDependencies.Count} 个关联表");
                    foreach (var kvp in allDependencies)
                    {
                        Log($"  → {kvp.Key}: {kvp.Value.Count} 个外键引用");
                    }

                    // 2. 生成删除顺序（自底向上：先删子表，后删父表）
                    var deleteOrder = GenerateDeleteOrder(allDependencies, rootTableName);
                    Log($"[删除顺序] 共 {deleteOrder.Count} 个阶段");
                    for (int i = 0; i < deleteOrder.Count; i++)
                    {
                        Log($"  阶段 {i + 1}: {deleteOrder[i].TableName}.{deleteOrder[i].ForeignKeyColumn}");
                    }

                    // 3. ⚠️ 关键修复：预查询所有表的主键ID
                    Log($"\n========== [步骤3] 预查询所有表的主键ID ==========");
                    var idMap = new Dictionary<string, List<long>>();
                    idMap[rootTableName] = new List<long>(targetIds);
                    
                    // 递归查询所有子表的主键ID
                    async Task QueryChildIds(string parentTable, List<long> parentIds, string parentFkColumn)
                    {
                        if (!allDependencies.ContainsKey(parentTable))
                            return;
                        
                        foreach (var fk in allDependencies[parentTable])
                        {
                            string childTable = fk.ReferencingTableName;
                            string childFkColumn = fk.ReferencingColumnName;
                            
                            // ✅ 修复：从数据库查询主键列（支持没有实体类的表）
                            var childMetadata = await ModelMetadataHelper.GetMetadataAsync(db, childTable);
                            string childPkColumn = childMetadata?.PrimaryKeyName ?? "PrimaryKeyID";
                            
                            Log($"[预查询] {parentTable}.{parentFkColumn} → {childTable}.{childFkColumn} (主键: {childPkColumn})");
                            
                            // 查询子表的主键ID
                            var childIds = await db.Ado.SqlQueryAsync<long>(
                                $"SELECT [{childPkColumn}] FROM [{childTable}] WHERE [{childFkColumn}] IN (@ids)",
                                new { ids = parentIds }
                            );
                            
                            var childIdList = childIds.ToList();
                            idMap[childTable] = childIdList;
                            
                            Log($"[预查询]   结果: {childIdList.Count} 条记录");
                            
                            // 递归查询下一层
                            await QueryChildIds(childTable, childIdList, childFkColumn);
                        }
                    }
                    
                    // 开始递归查询
                    await QueryChildIds(rootTableName, targetIds, rootPkColumn);
                    
                    Log($"[预查询完成] 共查询 {idMap.Count} 个表的ID");
                    foreach (var kvp in idMap)
                    {
                        Log($"  → {kvp.Key}: {kvp.Value.Count} 条记录");
                    }

                    // 4. 按删除顺序执行删除
                    Log($"\n========== [步骤4] 执行删除 ==========");
                    var deletedCount = 0;

                    for (int stage = 0; stage < deleteOrder.Count; stage++)
                    {
                        var tableInfo = deleteOrder[stage];
                        
                        // 根表跳过，最后单独处理
                        if (tableInfo.TableName == rootTableName)
                        {
                            Log($"[阶段 {stage + 1}] ⚠️ 根表，跳过");
                            continue;
                        }
                        
                        // 从 idMap 中获取该表的主键ID列表
                        var idsToDelete = idMap.ContainsKey(tableInfo.TableName) 
                            ? idMap[tableInfo.TableName] 
                            : new List<long>();
                        
                        // ⚠️ 关键修复：获取该表的主键列名（不是外键列！）
                        // ✅ 修复：从数据库查询主键列（支持没有实体类的表）
                        var tableMetadata = await ModelMetadataHelper.GetMetadataAsync(db, tableInfo.TableName);
                        string primaryKeyColumn = tableMetadata?.PrimaryKeyName ?? "PrimaryKeyID";
                        
                        Log($"\n---------- [阶段 {stage + 1}/{deleteOrder.Count}] ----------");
                        Log($"[阶段 {stage + 1}] 表名: {tableInfo.TableName}");
                        Log($"[阶段 {stage + 1}] 主键列: {primaryKeyColumn}");
                        Log($"[阶段 {stage + 1}] 待删除ID数量: {idsToDelete.Count}");
                        
                        if (idsToDelete.Count == 0)
                        {
                            Log($"[阶段 {stage + 1}] ℹ️ 无记录，跳过");
                            continue;
                        }
                        
                        if (isTestMode)
                        {
                            Log($"[阶段 {stage + 1}] 🧪 测试模式：只查询");
                            var count = await db.Queryable<object>()
                                .AS(tableInfo.TableName)
                                .Where($"[{primaryKeyColumn}] IN (@ids)", new { ids = idsToDelete })
                                .CountAsync();
                            
                            Log($"[阶段 {stage + 1}] ✓ 预计删除 {count} 条");
                            deletedCount += count;
                        }
                        else
                        {
                            Log($"[阶段 {stage + 1}] 🔴 正式删除");
                            Log($"[阶段 {stage + 1}] SQL: DELETE FROM [{tableInfo.TableName}] WHERE [{primaryKeyColumn}] IN ({idsToDelete.Count}个ID)");
                            
                            var deleted = await db.Deleteable<object>()
                                .AS(tableInfo.TableName)
                                .Where($"[{primaryKeyColumn}] IN (@ids)", new { ids = idsToDelete })
                                .ExecuteCommandAsync();

                            Log($"[阶段 {stage + 1}] ✓ 删除 {deleted} 条记录");
                            deletedCount += deleted;
                        }

                        ReportProgress(stage + 1, deleteOrder.Count, $"正在删除 {tableInfo.TableName}");
                    }

                    // 5. 最后删除根表
                    Log($"\n========== [步骤5] 删除根表 ==========");
                    Log($"[根表] 表名: {rootTableName}");
                    Log($"[根表] 主键: {rootPkColumn}");
                    Log($"[根表] ID数量: {targetIds.Count}");
                    
                    if (isTestMode)
                    {
                        var rootCount = await db.Queryable<object>()
                            .AS(rootTableName)
                            .Where($"[{rootPkColumn}] IN (@ids)", new { ids = targetIds })
                            .CountAsync();
                        
                        Log($"[根表] ✓ [测试] 预计删除 {rootCount} 条");
                        deletedCount += rootCount;
                    }
                    else
                    {
                        Log($"[根表] 🔴 正式删除");
                        Log($"[根表] SQL: DELETE FROM [{rootTableName}] WHERE [{rootPkColumn}] IN ({targetIds.Count}个ID)");
                        
                        var rootDeleted = await db.Deleteable<object>()
                            .AS(rootTableName)
                            .Where($"[{rootPkColumn}] IN (@ids)", new { ids = targetIds })
                            .ExecuteCommandAsync();
                        
                        Log($"[根表] ✓ 删除 {rootDeleted} 条记录");
                        deletedCount += rootDeleted;
                    }

                    Log($"\n========== [删除汇总] ==========");
                    Log($"[汇总] 总删除数: {deletedCount}");
                    Log($"[汇总] 涉及表数: {deleteOrder.Count}");

                    result.TotalDeletedCount = deletedCount;
                    result.IsSuccess = true;

                    if (isTestMode)
                    {
                        Log($"[事务] 回滚事务...");
                        await _unitOfWork.RollbackTranAsync();
                        Log($"[事务] ✓ 已回滚");
                    }
                    else
                    {
                        Log($"[事务] 提交事务...");
                        await _unitOfWork.CommitTranAsync();
                        Log($"[事务] ✓ 已提交");
                    }
                    
                    Log($"\n========== [完成] ==========");
                }
                catch (Exception ex)
                {
                    Log($"\n========== [异常捕获] ==========");
                    Log($"[异常捕获] 捕获到未处理的异常!");
                    Log($"[异常捕获] 异常类型: {ex.GetType().FullName}");
                    Log($"[异常捕获] 异常消息: {ex.Message}");
                    Log($"[异常捕获] 堆栈跟踪: {ex.StackTrace}");
                    
                    if (ex.InnerException != null)
                    {
                        Log($"[异常捕获] 内部异常类型: {ex.InnerException.GetType().FullName}");
                        Log($"[异常捕获] 内部异常消息: {ex.InnerException.Message}");
                        Log($"[异常捕获] 内部异常堆栈: {ex.InnerException.StackTrace}");
                    }
                    
                    Log($"[事务操作] 准备回滚事务（因异常）...");
                    await _unitOfWork.RollbackTranAsync();
                    Log($"[事务操作] ✓ 事务已回滚");
                    
                    throw; // 重新抛出异常，让外层catch处理
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                Log($"[错误] {ex.Message}");
                Log($"[堆栈] {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Log($"[内部错误] {ex.InnerException.Message}");
                }
            }

            result.Complete();
            return result;
        }

        /// <summary>
        /// 递归收集所有外键关系（从数据库）
        /// 注意：这里收集的是引用当前表的其他表（子表），而不是当前表引用的表（父表）
        /// </summary>
        private async Task CollectAllForeignKeysRecursiveAsync(
            string tableName,
            Dictionary<string, List<DbForeignKeyInfo>> dependencies,
            HashSet<string> visited)
        {
            if (visited.Contains(tableName))
            {
                Log($"[依赖收集] ⚠️ {tableName} 已访问过，跳过（防止循环引用）");
                return;
            }
        
            Log($"[依赖收集] → 正在分析表: {tableName}");
            visited.Add(tableName);
        
            var db = _unitOfWork.GetDbClient();
        
            // 查询引用该表的所有外键（其他表引用当前表）
            // 使用“谁引用了我”查询：fk.referenced_object_id = 当前表
            Log($"[依赖收集]   执行SQL: 查询 sys.foreign_keys WHERE referenced_table = {tableName}");
                    
            var foreignKeys = await db.Ado.SqlQueryAsync<DbForeignKeyInfo>(@"
                SELECT 
                    fk.name AS ForeignKeyName,
                    tp.name AS ReferencingTableName,
                    cp.name AS ReferencingColumnName,
                    tr.name AS ReferencedTableName,
                    cr.name AS ReferencedColumnName
                FROM sys.foreign_keys AS fk
                INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
                INNER JOIN sys.tables AS tp ON fkc.parent_object_id = tp.object_id
                INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
                INNER JOIN sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
                INNER JOIN sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
                WHERE tr.name = @tableName", 
                new { tableName });
        
            Log($"[依赖收集]   查询结果: 找到 {foreignKeys.Count} 个外键引用");
                    
            if (!dependencies.ContainsKey(tableName))
                dependencies[tableName] = new List<DbForeignKeyInfo>();
        
            dependencies[tableName].AddRange(foreignKeys);
                    
            foreach (var fk in foreignKeys)
            {
                Log($"[依赖收集]     - {fk.ReferencingTableName}.{fk.ReferencingColumnName} → {tableName}");
            }
        
            // 递归处理引用当前表的子表
            if (foreignKeys.Count > 0)
            {
                Log($"[依赖收集]   递归处理 {foreignKeys.Count} 个子表...");
            }
                    
            foreach (var fk in foreignKeys)
            {
                // fk.ReferencingTableName 是引用当前表（被引用方）的表，即子表
                await CollectAllForeignKeysRecursiveAsync(fk.ReferencingTableName, dependencies, visited);
            }
                    
            if (foreignKeys.Count > 0)
            {
                Log($"[依赖收集] ← {tableName} 分析完成，共 {foreignKeys.Count} 个子表");
            }
        }

        /// <summary>
        /// 生成删除顺序（自底向上）
        /// 使用拓扑排序算法：先删除叶子节点（没有子表的表），最后删除根表
        /// </summary>
        private List<DbTableDeleteInfo> GenerateDeleteOrder(Dictionary<string, List<DbForeignKeyInfo>> dependencies, string rootTableName)
        {
            var result = new List<DbTableDeleteInfo>();
            
            // 构建图结构：表名 -> 引用的子表列表
            var graph = new Dictionary<string, HashSet<string>>();
            var allTables = new HashSet<string> { rootTableName };
            
            // 收集所有表和依赖关系
            foreach (var kvp in dependencies)
            {
                string parentTable = kvp.Key;
                allTables.Add(parentTable);
                
                if (!graph.ContainsKey(parentTable))
                    graph[parentTable] = new HashSet<string>();
                
                foreach (var fk in kvp.Value)
                {
                    string childTable = fk.ReferencingTableName;
                    graph[parentTable].Add(childTable);
                    allTables.Add(childTable);
                }
            }
            
            // 计算每个表的入度（有多少父表引用它）
            var inDegree = new Dictionary<string, int>();
            foreach (var table in allTables)
            {
                inDegree[table] = 0;
            }
            
            foreach (var kvp in graph)
            {
                foreach (var child in kvp.Value)
                {
                    if (inDegree.ContainsKey(child))
                        inDegree[child]++;
                }
            }
            
            // 拓扑排序：从入度为0的表开始（叶子节点）
            var queue = new Queue<string>();
            foreach (var kvp in inDegree)
            {
                if (kvp.Value == 0)
                {
                    queue.Enqueue(kvp.Key);
                }
            }
            
            while (queue.Count > 0)
            {
                string currentTable = queue.Dequeue();
                
                // 找到该表的外键列名
                string fkColumn = "";
                if (dependencies.ContainsKey(currentTable))
                {
                    // 这个表被其他表引用，找到引用它的外键
                    foreach (var kvp in dependencies)
                    {
                        var fk = kvp.Value.FirstOrDefault(f => f.ReferencingTableName == currentTable);
                        if (fk != null)
                        {
                            fkColumn = fk.ReferencingColumnName;
                            break;
                        }
                    }
                }
                
                result.Add(new DbTableDeleteInfo
                {
                    TableName = currentTable,
                    ForeignKeyColumn = fkColumn
                });
                
                // 更新子节点的入度
                if (graph.ContainsKey(currentTable))
                {
                    foreach (var child in graph[currentTable])
                    {
                        inDegree[child]--;
                        if (inDegree[child] == 0)
                        {
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            
            // 反转结果，使根表在最后
            result.Reverse();
            
            Log($"[拓扑排序] 生成删除顺序: {string.Join(" → ", result.Select(r => r.TableName))}");
            
            return result;
        }

        /// <summary>
        /// 处理外键冲突（批量处理版本）
        /// ⚠️ 关键修复：先查询中间表的主键ID，再删除子表
        /// </summary>
        /// <param name="db">数据库客户端</param>
        /// <param name="parentIds">父表的ID列表（如 SaleOut_MainID）</param>
        /// <param name="parentTable">父表名（如 tb_SaleOutRe）</param>
        /// <param name="parentFkColumn">父表的外键列（如 SaleOut_MainID）</param>
        /// <param name="childTable">冲突的子表名（如 tb_SaleOutReDetail）</param>
        /// <param name="childFkColumn">子表的外键列（如 SaleOutRe_ID）</param>
        /// <param name="isTestMode">是否测试模式</param>
        /// <returns>删除的记录数</returns>
        private async Task<int> HandleForeignKeyConflictAsync(
            ISqlSugarClient db, 
            List<long> parentIds, 
            string parentTable,
            string parentFkColumn,
            string childTable, 
            string childFkColumn,
            bool isTestMode)
        {
            Log($"\n========== [外键冲突处理] 开始 ==========");
            Log($"[外键冲突] 父表: {parentTable}");
            Log($"[外键冲突] 父表外键列: {parentFkColumn}");
            Log($"[外键冲突] 子表: {childTable}");
            Log($"[外键冲突] 子表外键列: {childFkColumn}");
            Log($"[外键冲突] 父ID数量: {parentIds.Count}");
            Log($"[外键冲突] 父ID示例: {string.Join(", ", parentIds.Take(5))}{(parentIds.Count > 5 ? $"... (共{parentIds.Count}个)" : "")}");
            Log($"[外键冲突] 模式: {(isTestMode ? "测试" : "正式")}");

            try
            {
                // ⚠️ 关键步骤：先查询父表中符合条件的主键ID
                // 例如：SELECT PrimaryKeyID FROM tb_SaleOutRe WHERE SaleOut_MainID IN (@parentIds)
                Log($"[外键冲突] 步骤1: 查询父表主键ID...");
                Log($"[外键冲突]   SQL: SELECT * FROM [{parentTable}] WHERE [{parentFkColumn}] IN ({parentIds.Count}个ID)");
                
                // ✅ 修复：从数据库查询主键列（支持没有实体类的表）
                var parentMetadata = await ModelMetadataHelper.GetMetadataAsync(db, parentTable);
                string parentPkColumn = parentMetadata?.PrimaryKeyName ?? "PrimaryKeyID";
                Log($"[外键冲突]   父表主键列: {parentPkColumn}");
                
                // 查询父表的主键ID
                var parentPkIds = await db.Ado.SqlQueryAsync<long>(
                    $"SELECT [{parentPkColumn}] FROM [{parentTable}] WHERE [{parentFkColumn}] IN (@parentIds)",
                    new { parentIds }
                );
                
                var parentPkIdList = parentPkIds.ToList();
                Log($"[外键冲突]   查询结果: 找到 {parentPkIdList.Count} 条父表记录");
                
                if (parentPkIdList.Count == 0)
                {
                    Log($"[外键冲突]   ⚠️ 父表中没有匹配的记录，可能已被删除");
                    Log($"[外键冲突处理] ========== 完成（无记录） ==========");
                    return 0;
                }
                
                Log($"[外键冲突]   主键ID示例: {string.Join(", ", parentPkIdList.Take(5))}{(parentPkIdList.Count > 5 ? $"... (共{parentPkIdList.Count}个)" : "")}");
                
                // 步骤2：用父表的主键ID去删除子表
                Log($"[外键冲突] 步骤2: 删除子表记录...");
                Log($"[外键冲突]   SQL: DELETE FROM [{childTable}] WHERE [{childFkColumn}] IN ({parentPkIdList.Count}个ID)");
                
                if (isTestMode)
                {
                    Log($"[外键冲突] 🧪 测试模式：查询冲突记录数");
                    var count = await db.Queryable<object>()
                        .AS(childTable)
                        .Where($"[{childFkColumn}] IN (@ids)", new { ids = parentPkIdList })
                        .CountAsync();
                    
                    Log($"[外键冲突] ✓ [测试] 预计删除 {count} 条冲突记录");
                    Log($"[外键冲突处理] ========== 完成 ==========");
                    return count;
                }
                else
                {
                    Log($"[外键冲突] 🔴 正式模式：执行冲突记录删除");
                    
                    var deleted = await db.Deleteable<object>()
                        .AS(childTable)
                        .Where($"[{childFkColumn}] IN (@ids)", new { ids = parentPkIdList })
                        .ExecuteCommandAsync();

                    Log($"[外键冲突] ✓ 已删除冲突记录 {deleted} 条");
                    Log($"[外键冲突处理] ========== 完成 ==========");
                    return deleted;
                }
            }
            catch (Exception ex)
            {
                Log($"[外键冲突] ✗ 处理失败!");
                Log($"[外键冲突] 错误类型: {ex.GetType().FullName}");
                Log($"[外键冲突] 错误消息: {ex.Message}");
                Log($"[外键冲突] 堆栈跟踪: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Log($"[外键冲突] 内部异常: {ex.InnerException.Message}");
                }
                
                Log($"[外键冲突处理] ========== 完成（失败） ==========");
                return 0;
            }
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        private void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            OnLog?.Invoke(this, logEntry);
            _logger?.LogInformation(message);
        }

        /// <summary>
        /// 报告进度
        /// </summary>
        private void ReportProgress(int current, int total, string message)
        {
            OnProgressChanged?.Invoke(this, new CleanupProgressEventArgs
            {
                Current = current,
                Total = total,
                Message = message,
                Percentage = total > 0 ? (int)((double)current / total * 100) : 0
            });
        }

        /// <summary>
        /// 日志事件
        /// </summary>
        public event EventHandler<string> OnLog;

        /// <summary>
        /// 进度变化事件
        /// </summary>
        public event EventHandler<CleanupProgressEventArgs> OnProgressChanged;
    }
}
