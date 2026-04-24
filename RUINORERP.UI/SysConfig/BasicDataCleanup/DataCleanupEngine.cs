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
    /// 辅助类：从 Model 实体中提取元数据（主键、外键）
    /// </summary>
    public static class ModelMetadataHelper
    {
        private static readonly Dictionary<string, (string PkName, List<(string TableName, string FkCol)> Refs)> _cache = new();

        public static (string PkName, List<(string TableName, string FkCol)> Refs) GetMetadata<T>() where T : BaseEntity, new()
        {
            var type = typeof(T);
            if (_cache.TryGetValue(type.FullName, out var cached)) return cached;

            // 1. 找主键：必须严格匹配 IsPrimaryKey = true
            var pkProp = type.GetProperties().FirstOrDefault(p => 
                p.GetCustomAttribute<SugarColumn>()?.IsPrimaryKey == true);
            
            // 如果模型里没标主键， fallback 到 PrimaryKeyID
            if (pkProp == null) pkProp = type.GetProperty("PrimaryKeyID");
            
            var pkName = pkProp?.GetCustomAttribute<SugarColumn>()?.ColumnName ?? pkProp?.Name;
            if (string.IsNullOrEmpty(pkName)) throw new Exception($"实体 {type.Name} 未找到有效主键！");

            // 2. 找所有引用该表的外键（通过 Navigate OneToMany）
            var refs = new List<(string TableName, string FkCol)>();
            foreach (var prop in type.GetProperties())
            {
                var navAttr = prop.GetCustomAttribute<Navigate>();
                if (navAttr != null && navAttr.GetNavigateType() == NavigateType.OneToMany)
                {
                    var childType = prop.PropertyType.IsGenericType 
                        ? prop.PropertyType.GetGenericArguments()[0] 
                        : prop.PropertyType;
                    
                    // 获取子表中外键字段的名称
                    var fkColName = navAttr.GetName();
                    refs.Add((childType.Name, fkColName));
                }
            }

            var result = (PkName: pkName, Refs: refs);
            _cache[type.FullName] = result;
            return result;
        }
    }

    /// <summary>
    /// 数据清理引擎 - 融合 Model 元数据与纯SQL错误驱动策略
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
        /// 执行级联删除 - 采用“纯SQL错误驱动自动排序”策略
        /// </summary>
        public async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync<T>(List<long> targetIds, bool isTestMode = false) where T : BaseEntity, new()
        {
            var result = new CascadeDeleteResult { IsTestMode = isTestMode, StartTime = DateTime.Now };
            if (!targetIds.Any()) return result;
        
            try
            {
                Log($"[启动] 纯SQL级联删除：{typeof(T).Name}, ID数：{targetIds.Count}, 模式：{(isTestMode ? "测试" : "正式")}");
                        
                var meta = ModelMetadataHelper.GetMetadata<T>();
                var tableName = typeof(T).Name;
                
                Log($"[元数据] 实体: {tableName}, 主键字段: {meta.PkName}, 发现 {meta.Refs.Count} 个下游依赖");

                // 初始化删除任务列表（必须使用正确的主键字段名）
                var deleteTasks = new List<DeleteTask>
                {
                    new DeleteTask 
                    { 
                        TableName = tableName, 
                        ColumnName = meta.PkName, // ✅ 确保这里用的是 PurOrder_ID 而不是 SOrder_ID
                        Ids = targetIds, 
                        IsCompleted = false 
                    }
                };

                // 预加载已知的外键依赖到任务列表中（作为备选方案）
                foreach (var refInfo in meta.Refs)
                {
                    // 这里我们先不添加，而是等错误发生时再确认，或者用于验证
                }
        
                await _unitOfWork.BeginTranAsync();
                try
                {
                    int maxRetries = 10; // 防止死循环
                    int retryCount = 0;
                    int totalDeleted = 0;
        
                    while (deleteTasks.Any(t => !t.IsCompleted) && retryCount < maxRetries)
                    {
                        retryCount++;
                        bool hasProgress = false;
        
                        foreach (var task in deleteTasks.Where(t => !t.IsCompleted).ToList())
                        {
                            try
                            {
                                // ✅ 尝试执行 SQL 删除
                                var count = await _unitOfWork.GetDbClient().Deleteable<object>()
                                    .AS(task.TableName)
                                    .Where($"[{task.ColumnName}] IN (@ids)", new { ids = task.Ids })
                                    .ExecuteCommandAsync();
                                        
                                if (count > 0)
                                {
                                    task.IsCompleted = true;
                                    totalDeleted += count;
                                    hasProgress = true;
                                    Log($"[第{retryCount}轮] ✓ 成功删除 {task.TableName} ({count}条)");
                                }
                                else
                                {
                                    // ✅ 关键修复：只有当确认没有相关记录时才标记完成
                                    // 如果是因为字段名错误导致的 SQL 异常，会在 catch 块处理
                                    // 这里我们暂时不标记完成，留给下一轮重试，除非我们确认它真的没数据了
                                    // 为了防止死循环，我们增加一个计数器，但首先我们要确保 ColumnName 是对的
                                    var checkSql = $"SELECT COUNT(1) FROM [{task.TableName}] WHERE [{task.ColumnName}] IN (@ids)";
                                    var remaining = await _unitOfWork.GetDbClient().Ado.SqlQuerySingleAsync<int>(checkSql, new { ids = task.Ids });
                                    
                                    if (remaining == 0)
                                    {
                                        task.IsCompleted = true;
                                        Log($"[第{retryCount}轮] ℹ️ {task.TableName} 无相关记录，标记完成");
                                    }
                                    else
                                    {
                                        Log($"[第{retryCount}轮] ⚠️ {task.TableName} 仍有 {remaining} 条记录未删除，等待依赖清理...");
                                    }
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                // ❌ 捕获外键冲突，解析并动态添加新的删除任务
                                var fkInfo = ParseForeignKeyError(sqlEx.Message);
                                if (fkInfo != null)
                                {
                                    Log($"[第{retryCount}轮] ⚠️ {task.TableName} 被 {fkInfo.Value.TableName}.{fkInfo.Value.ColumnName} 引用");
                                            
                                    // 查找子表中引用了当前 ID 的记录
                                    var subIds = await FindReferencingIdsAsync(fkInfo.Value.TableName, fkInfo.Value.ColumnName, task.Ids);
                                            
                                    if (subIds.Any())
                                    {
                                        // 检查是否已经存在该任务
                                        var existingTask = deleteTasks.FirstOrDefault(t => t.TableName == fkInfo.Value.TableName && t.ColumnName == fkInfo.Value.ColumnName);
                                        if (existingTask == null)
                                        {
                                            deleteTasks.Add(new DeleteTask
                                            {
                                                TableName = fkInfo.Value.TableName,
                                                ColumnName = fkInfo.Value.ColumnName, // ✅ 必须使用解析出的外键字段名
                                                Ids = subIds,
                                                IsCompleted = false
                                            });
                                            Log($"[第{retryCount}轮] ➕ 新增删除任务: {fkInfo.Value.TableName}.{fkInfo.Value.ColumnName} ({subIds.Count}条)");
                                            hasProgress = true; 
                                        }
                                        else if (!existingTask.IsCompleted)
                                        {
                                            // 如果任务已存在但未完成，合并 ID 列表
                                            var newIds = subIds.Except(existingTask.Ids).ToList();
                                            if (newIds.Any())
                                            {
                                                existingTask.Ids.AddRange(newIds);
                                                Log($"[第{retryCount}轮] 🔄 更新任务 {fkInfo.Value.TableName}，新增 {newIds.Count} 条记录");
                                                hasProgress = true;
                                            }
                                            else
                                            {
                                                Log($"[第{retryCount}轮] ℹ️ 任务 {fkInfo.Value.TableName} 已存在且 ID 无变化，尝试再次删除...");
                                                // 即使 ID 没变，我们也标记有进展，让循环再跑一次，防止因为偶发锁或缓存导致的失败
                                                hasProgress = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Log($"[第{retryCount}轮] ⚠️ 警告: 在 {fkInfo.Value.TableName} 中未找到引用 ID。可能是误报，或者数据已被其他方式清理。");
                                        // 即使没找到 ID，我们也标记一下，防止死循环
                                        hasProgress = true;
                                    }
                                }
                                else
                                {
                                    Log($"[第{retryCount}轮] ❌ 发生非外键 SQL 错误: {sqlEx.Message}");
                                    throw; // 非外键错误，直接抛出
                                }
                            }
                        }
        
                        if (!hasProgress && deleteTasks.Any(t => !t.IsCompleted))
                        {
                            throw new InvalidOperationException("无法解决外键依赖，可能存在循环引用或未知约束。");
                        }
                    }
        
                    result.IsSuccess = true;
                    result.TotalDeletedCount = totalDeleted;
                    result.Complete();
        
                    if (isTestMode)
                    {
                        await _unitOfWork.RollbackTranAsync();
                        Log($"[测试完成] 事务已回滚。预计影响 {totalDeleted} 条记录。");
                    }
                    else
                    {
                        await _unitOfWork.CommitTranAsync();
                        Log($"[正式完成] 事务已提交。总计删除 {totalDeleted} 条记录。");
                    }
                }
                catch (Exception) { await _unitOfWork.RollbackTranAsync(); throw; }
            }
            catch (Exception ex)
            {
                result.MarkAsFailed(ex.Message);
                _logger?.LogError(ex, "级联删除异常");
            }
            return result;
        }
        
        /// <summary>
        /// 辅助类：删除任务
        /// </summary>
        private class DeleteTask
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public List<long> Ids { get; set; }
            public bool IsCompleted { get; set; }
        }
        
        /// <summary>
        /// 查找子表中引用了指定父ID的记录ID
        /// </summary>
        private async Task<List<long>> FindReferencingIdsAsync(string tableName, string columnName, List<long> parentIds)
        {
            if (!parentIds.Any()) return new List<long>();
            var db = _unitOfWork.GetDbClient();
                    
            try
            {
                var sql = $"SELECT DISTINCT [{columnName}] FROM [{tableName}] WHERE [{columnName}] IN (@ids)";
                var ids = await db.Ado.SqlQueryAsync<long>(sql, new { ids = parentIds });
                return ids.ToList();
            }
            catch (Exception ex)
            {
                Log($"[查找引用] ❌ 查询 {tableName}.{columnName} 失败: {ex.Message}");
                return new List<long>();
            }
        }

        /// <summary>
        /// 解析依赖并执行批量删除（递归核心）- 真正的自底向上删除
        /// </summary>
        private async Task<int> ResolveAndExecuteBatchDeleteAsync<T>(BaseController<T> controller, List<long> ids, bool isTestMode, int depth) where T : BaseEntity, new()
        {
            if (depth > 5) throw new InvalidOperationException("级联深度超过限制(5层)，请检查是否存在循环引用。");
            if (!ids.Any()) return 0;
        
            var entityType = typeof(T);
            var pkProp = GetPrimaryKeyProperty<T>();
            int deletedCount = 0;
        
            foreach (var id in ids)
            {
                var entity = Activator.CreateInstance<T>();
                pkProp.SetValue(entity, id);
        
                try
                {
                    // 1. 尝试删除当前层级
                    if (!isTestMode) await controller.BaseDeleteByNavAsync(entity);
                    deletedCount++;
                }
                catch (SqlException sqlEx)
                {
                    // 2. ✅ 错误驱动：解析外键冲突
                    var fkInfo = ParseForeignKeyError(sqlEx.Message);
                    if (fkInfo != null)
                    {
                        Log($"[深度 {depth}] 发现依赖: {entityType.Name} <- {fkInfo.Value.TableName}.{fkInfo.Value.ColumnName}");
                                
                        // 3. ✅ 关键修复：直接删除冲突的子表，而不是查找中间表
                        // 例如：删除 tb_SaleOut 时，tb_SaleOutDetail 冲突
                        // 我们应该直接 DELETE FROM tb_SaleOutDetail WHERE SaleOut_MainID = @saleOutId
                        // 但问题是：我们不知道 SaleOut_MainID 的值是多少！
                        
                        // 解决方案：查询出所有引用了当前实体的子表记录的外键值
                        // 即：找出哪些 tb_SaleOut 记录被 tb_SaleOutDetail 引用，且这些 tb_SaleOut 又引用了当前的 tb_SaleOrder
                        
                        var subTableFkValues = await GetSubTableReferencingValuesAsync(
                            fkInfo.Value.TableName,      // tb_SaleOutDetail
                            fkInfo.Value.ColumnName,     // SaleOut_MainID
                            entityType.Name,             // tb_SaleOut
                            pkProp.Name,                 // SaleOut_MainID (主键)
                            ids                          // 当前要删除的 ID 列表
                        );
                        
                        if (subTableFkValues.Any())
                        {
                            Log($"[深度 {depth}] 找到子表 {fkInfo.Value.TableName} 需要删除 {subTableFkValues.Count} 条记录");
                            
                            // 4. 递归删除子表（传入的是子表外键字段的值，即中间表的主键）
                            var subDeleted = await DeleteTableByForeignKeyAsync(
                                fkInfo.Value.TableName, 
                                fkInfo.Value.ColumnName, 
                                subTableFkValues,  // 这些是 tb_SaleOutDetail.SaleOut_MainID 的值
                                depth + 1
                            );
                            deletedCount += subDeleted;
                        }
                                
                        // 5. 重试删除当前记录
                        if (!isTestMode) await controller.BaseDeleteByNavAsync(entity);
                        deletedCount++;
                    }
                    else { throw; } // 无法解析则抛出
                }
            }
            return deletedCount;
        }
        
        /// <summary>
        /// 获取子表中引用了当前实体关联记录的外键值
        /// </summary>
        /// <param name="subTableName">子表名 (如 tb_SaleOutDetail)</param>
        /// <param name="subTableFkColumn">子表外键字段 (如 SaleOut_MainID)</param>
        /// <param name="parentTableName">父表名 (如 tb_SaleOut)</param>
        /// <param name="parentTablePkColumn">父表主键字段 (如 SaleOut_MainID)</param>
        /// <param name="rootIds">根节点ID列表 (如 tb_SaleOrder 的 ID)</param>
        private async Task<List<long>> GetSubTableReferencingValuesAsync(
            string subTableName, 
            string subTableFkColumn,
            string parentTableName,
            string parentTablePkColumn,
            List<long> rootIds)
        {
            var db = _unitOfWork.GetDbClient();
            
            // 需要构建一个 JOIN 查询：
            // SELECT DISTINCT detail.SaleOut_MainID 
            // FROM tb_SaleOutDetail detail
            // INNER JOIN tb_SaleOut saleout ON detail.SaleOut_MainID = saleout.SaleOut_MainID
            // WHERE saleout.Order_ID IN (@rootIds)
            
            // 但问题是我们不知道 tb_SaleOut 中哪个字段引用了 tb_SaleOrder
            // 所以我们需要先找出 parentTableName 中引用 rootIds 的记录
            
            // 步骤1: 找出父表中与根节点相关的记录ID
            var parentIds = await FindParentRecordsLinkedToRootAsync(parentTableName, parentTablePkColumn, rootIds);
            
            if (!parentIds.Any())
            {
                Log($"[查找关联] ⚠️ 未找到 {parentTableName} 中与根节点相关的记录");
                return new List<long>();
            }
            
            Log($"[查找关联] 找到 {parentTableName} 中 {parentIds.Count} 条相关记录");
            
            // 步骤2: 查询子表中引用了这些父表记录的外键值
            var sql = $"SELECT DISTINCT [{subTableFkColumn}] FROM [{subTableName}] WHERE [{subTableFkColumn}] IN (@parentIds)";
            var values = await db.Ado.SqlQueryAsync<long>(sql, new { parentIds });
            
            return values.ToList();
        }
        
        /// <summary>
        /// 查找父表中与根节点相关的记录ID
        /// </summary>
        private async Task<List<long>> FindParentRecordsLinkedToRootAsync(string parentTableName, string parentPkColumn, List<long> rootIds)
        {
            var db = _unitOfWork.GetDbClient();
            
            // 尝试常见的外键字段名
            var possibleFkNames = new[] 
            { 
                "Order_ID",
                "SaleOrder_ID", 
                "MainID",
                "Header_ID",
                parentTableName.Replace("tb_", "").Replace("Sale", "").Replace("Pur", "") + "_ID"
            };
            
            foreach (var fkName in possibleFkNames)
            {
                try
                {
                    var sql = $"SELECT DISTINCT [{parentPkColumn}] FROM [{parentTableName}] WHERE [{fkName}] IN (@rootIds)";
                    var ids = await db.Ado.SqlQueryAsync<long>(sql, new { rootIds });
                    if (ids.Any())
                    {
                        Log($"[查找父表] 通过字段 {fkName} 找到 {ids.Count} 条记录");
                        return ids.ToList();
                    }
                }
                catch
                {
                    continue;
                }
            }
            
            Log($"[查找父表] ⚠️ 未找到 {parentTableName} 中引用根节点的外键字段");
            return new List<long>();
        }

        /// <summary>
        /// 根据外键关系删除子表数据（支持递归）
        /// </summary>
        private async Task<int> DeleteTableByForeignKeyAsync(string tableName, string columnName, List<long> parentIds, int depth)
        {
            if (!parentIds.Any()) return 0;
            var db = _unitOfWork.GetDbClient();
            int totalDeleted = 0;
        
            try
            {
                // ✅ 直接使用原生 SQL 批量删除，无需查询子表 ID
                var count = await db.Deleteable<object>()
                    .AS(tableName)
                    .Where($"[{columnName}] IN (@ids)", new { ids = parentIds })
                    .ExecuteCommandAsync();
                        
                Log($"[深度 {depth}] 已删除 {tableName} {count} 条记录");
                return count;
            }
            catch (SqlException ex)
            {
                // 子表也存在外键依赖，继续递归
                var fkInfo = ParseForeignKeyError(ex.Message);
                if (fkInfo != null)
                {
                    Log($"[深度 {depth}] 深层依赖: {tableName} <- {fkInfo.Value.TableName}.{fkInfo.Value.ColumnName}");
                            
                    // ✅ 关键修复：递归删除更深层时，仍然使用相同的 parentIds
                    // 因为子表的外键字段指向的是父表的主键，而不是子表自己的主键
                    var deepDeleted = await DeleteTableByForeignKeyAsync(fkInfo.Value.TableName, fkInfo.Value.ColumnName, parentIds, depth + 1);
                            
                    // 重试删除当前层
                    var retryCount = await db.Deleteable<object>()
                        .AS(tableName)
                        .Where($"[{columnName}] IN (@ids)", new { ids = parentIds })
                        .ExecuteCommandAsync();
                            
                    Log($"[深度 {depth}] 解决深层依赖后，删除 {tableName} {retryCount} 条记录");
                    return deepDeleted + retryCount;
                }
                throw;
            }
        }


        /// <summary>
        /// 智能处理外键冲突:查询相关ID并递归删除
        /// </summary>
        private async Task<bool> SmartHandleForeignKeyConflictAsync<T>(long mainId, string referencingTableName, string referencingColumnName) where T : BaseEntity, new()
        {
            var db = _unitOfWork.GetDbClient();

            try
            {
                Log($"[智能处理] 开始处理外键冲突: 表={referencingTableName}, 字段={referencingColumnName}");

                // 1. 查询出所有引用该主表记录的子记录ID
                var sql = $"SELECT DISTINCT [{referencingColumnName}] FROM [{referencingTableName}] WHERE [{referencingColumnName}] IN (@ids)";

                // 首先需要找到哪些中间表的记录引用了主表
                // 例如: 供应商 → 采购订单 → 入库单 → 入库明细
                // 我们需要先找到采购订单ID,然后找到入库单ID,最后删除入库明细

                // 简化策略:直接删除引用表中所有关联的记录
                // 注意:这里可能需要多层查询,但为了简化,我们先尝试直接删除

                var deletedCount = await db.Deleteable<object>()
                    .AS(referencingTableName)
                    .Where($"EXISTS (SELECT 1 FROM [{typeof(T).Name}] WHERE [{typeof(T).Name}].PrimaryKeyID = [{referencingTableName}].[{referencingColumnName}] AND [{typeof(T).Name}].PrimaryKeyID = @id)",
                        new { id = mainId })
                    .ExecuteCommandAsync();

                if (deletedCount > 0)
                {
                    Log($"[智能处理] ✓ 成功删除 {referencingTableName} {deletedCount} 条记录");
                    return true;
                }
                else
                {
                    // 如果直接删除失败,尝试通过中间表查找
                    Log($"[智能处理] 直接删除未影响任何记录,尝试通过导航属性查找...");
                    return await DeleteViaNavigationPropertyAsync<T>(mainId, referencingTableName, referencingColumnName);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"[智能处理] 处理外键冲突失败: {referencingTableName}");
                Log($"[智能处理] ✗ 处理失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 通过导航属性查找并删除关联数据
        /// </summary>
        private async Task<bool> DeleteViaNavigationPropertyAsync<T>(long mainId, string referencingTableName, string referencingColumnName) where T : BaseEntity, new()
        {
            var db = _unitOfWork.GetDbClient();
            var entityType = typeof(T);

            try
            {
                // 查找指向目标表的导航属性
                var navProperties = entityType.GetProperties()
                    .Where(p =>
                    {
                        var navigateAttr = p.GetCustomAttribute<SqlSugar.Navigate>();
                        return navigateAttr != null && navigateAttr.GetNavigateType() == SqlSugar.NavigateType.OneToMany;
                    })
                    .ToList();

                foreach (var navProp in navProperties)
                {
                    var navigateAttr = navProp.GetCustomAttribute<SqlSugar.Navigate>();
                    var childType = navProp.PropertyType.IsGenericType
                        ? navProp.PropertyType.GetGenericArguments()[0]
                        : navProp.PropertyType;
                    var childTableName = childType.Name;

                    // 检查这个子表是否就是我们要删除的表,或者它的子表
                    if (string.Equals(childTableName, referencingTableName, StringComparison.OrdinalIgnoreCase))
                    {
                        // 找到了!直接删除
                        var fkFieldName = navigateAttr.GetName();
                        var deletedCount = await db.Deleteable<object>()
                            .AS(childTableName)
                            .Where($"{fkFieldName} = @id", new { id = mainId })
                            .ExecuteCommandAsync();

                        Log($"[导航属性] ✓ 删除 {childTableName} {deletedCount} 条记录");
                        return true;
                    }
                }

                // 如果没找到匹配的导航属性,尝试直接根据表名和字段删除
                Log($"[直接删除] 未找到匹配的导航属性,尝试直接删除 {referencingTableName}");
                var directDeleted = await db.Deleteable<object>()
                    .AS(referencingTableName)
                    .Where($"{referencingColumnName} IN (SELECT PrimaryKeyID FROM [{entityType.Name}] WHERE PrimaryKeyID = @id)",
                        new { id = mainId })
                    .ExecuteCommandAsync();

                if (directDeleted > 0)
                {
                    Log($"[直接删除] ✓ 删除 {referencingTableName} {directDeleted} 条记录");
                    return true;
                }

                Log($"[直接删除] ✗ 未找到相关记录");
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"[导航属性] 处理失败");
                return false;
            }
        }

        /// <summary>
        /// 根据外键信息删除关联表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="mainId">主表ID</param>
        /// <param name="referencingTableName">引用表名(外键所在的表)</param>
        /// <param name="referencingColumnName">引用字段名(外键字段)</param>
        /// <returns>是否删除成功</returns>
        private async Task<bool> DeleteReferencingTableAsync<T>(long mainId, string referencingTableName, string referencingColumnName) where T : BaseEntity, new()
        {
            var db = _unitOfWork.GetDbClient();
            var entityType = typeof(T);

            try
            {
                _logger?.LogInformation($"开始删除关联表: {referencingTableName}, 字段={referencingColumnName}, 主ID={mainId}");

                // 1. 尝试通过导航属性查找关联
                var navProperties = entityType.GetProperties()
                    .Where(p =>
                    {
                        var navigateAttr = p.GetCustomAttribute<SqlSugar.Navigate>();
                        return navigateAttr != null && navigateAttr.GetNavigateType() == SqlSugar.NavigateType.OneToMany;
                    })
                    .ToList();

                // 查找匹配的导航属性
                foreach (var navProp in navProperties)
                {
                    var navigateAttr = navProp.GetCustomAttribute<SqlSugar.Navigate>();
                    var fkFieldName = navigateAttr.GetName();

                    var childType = navProp.PropertyType.IsGenericType
                        ? navProp.PropertyType.GetGenericArguments()[0]
                        : navProp.PropertyType;
                    var childTableName = childType.Name;

                    // 如果匹配的表名
                    if (string.Equals(childTableName, referencingTableName, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(fkFieldName, referencingColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        var deletedCount = await db.Deleteable<object>()
                            .AS(childTableName)
                            .Where($"{fkFieldName} = @id", new { id = mainId })
                            .ExecuteCommandAsync();

                        _logger?.LogInformation($"通过导航属性删除 {childTableName} {deletedCount} 条记录");
                        return true;
                    }
                }

                // 2. 如果没找到导航属性,直接根据表名和字段名删除
                var deleted = await db.Deleteable<object>()
                    .AS(referencingTableName)
                    .Where($"{referencingColumnName} = @id", new { id = mainId })
                    .ExecuteCommandAsync();

                _logger?.LogInformation($"直接删除 {referencingTableName} {deleted} 条记录");
                return deleted >= 0; // 成功执行即为true(可能0条记录)
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"删除关联表失败: {referencingTableName}, 字段={referencingColumnName}");
                return false;
            }
        }

        /// <summary>
        /// 模拟深层级删除(测试模式使用,仅统计不实际删除)
        /// </summary>
        private async Task<int> SimulateDeepLevelDeleteAsync<T>(T entity) where T : BaseEntity, new()
        {
            var db = _unitOfWork.GetDbClient();
            var entityType = typeof(T);
            int totalCount = 0;

            try
            {
                // 获取所有 OneToMany 导航属性
                var navProperties = entityType.GetProperties()
                    .Where(p =>
                    {
                        var navigateAttr = p.GetCustomAttribute<SqlSugar.Navigate>();
                        return navigateAttr != null &&
                               navigateAttr.GetNavigateType() == SqlSugar.NavigateType.OneToMany;
                    })
                    .ToList();

                foreach (var navProp in navProperties)
                {
                    var navigateAttr = navProp.GetCustomAttribute<SqlSugar.Navigate>();
                    var fkFieldName = navigateAttr.GetName();

                    var childType = navProp.PropertyType.IsGenericType
                        ? navProp.PropertyType.GetGenericArguments()[0]
                        : navProp.PropertyType;
                    var childTableName = childType.Name;

                    // 查询该导航属性下的记录数
                    var count = await db.Queryable<object>()
                        .AS(childTableName)
                        .Where($"{fkFieldName} = @id", new { id = entity.PrimaryKeyID })
                        .CountAsync();

                    totalCount += count;
                    Log($"[测试] {childTableName} 表预计删除 {count} 条记录 (外键: {fkFieldName})");

                    // 递归检查子表的导航属性(最多2层,避免过深)
                    if (count > 0)
                    {
                        var subNavCount = await SimulateSubTableDeletesAsync(childType, fkFieldName, entity.PrimaryKeyID, 1);
                        totalCount += subNavCount;
                    }
                }

                return totalCount;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"模拟深层级删除统计失败: {entityType.Name}");
                return 0;
            }
        }

        /// <summary>
        /// 递归模拟子表的深层级删除
        /// </summary>
        private async Task<int> SimulateSubTableDeletesAsync(Type parentType, string parentFkField, long parentId, int currentDepth)
        {
            if (currentDepth > 2) return 0; // 限制递归深度

            var db = _unitOfWork.GetDbClient();
            int totalCount = 0;

            try
            {
                var navProperties = parentType.GetProperties()
                    .Where(p =>
                    {
                        var navigateAttr = p.GetCustomAttribute<SqlSugar.Navigate>();
                        return navigateAttr != null &&
                               navigateAttr.GetNavigateType() == SqlSugar.NavigateType.OneToMany;
                    })
                    .ToList();

                foreach (var navProp in navProperties)
                {
                    var navigateAttr = navProp.GetCustomAttribute<SqlSugar.Navigate>();
                    var fkFieldName = navigateAttr.GetName();

                    var childType = navProp.PropertyType.IsGenericType
                        ? navProp.PropertyType.GetGenericArguments()[0]
                        : navProp.PropertyType;
                    var childTableName = childType.Name;

                    var count = await db.Queryable<object>()
                        .AS(childTableName)
                        .Where($"{fkFieldName} = @id", new { id = parentId })
                        .CountAsync();

                    totalCount += count;

                    if (count > 0 && currentDepth < 2)
                    {
                        var subCount = await SimulateSubTableDeletesAsync(childType, fkFieldName, parentId, currentDepth + 1);
                        totalCount += subCount;
                    }
                }

                return totalCount;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"递归模拟子表删除失败: {parentType.Name}, 深度={currentDepth}");
                return 0;
            }
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
        /// 获取实体的主键属性
        /// </summary>
        private PropertyInfo GetPrimaryKeyProperty<T>() where T : BaseEntity, new()
        {
            var type = typeof(T);
            // 优先查找标记了 [SugarColumn(IsPrimaryKey = true)] 的属性
            var pkProp = type.GetProperties().FirstOrDefault(p =>
                p.GetCustomAttribute<SqlSugar.SugarColumn>()?.IsPrimaryKey == true);

            // 如果没找到，尝试查找常见的命名约定
            if (pkProp == null)
            {
                pkProp = type.GetProperty("Id") ??
                         type.GetProperty("PrimaryKeyID") ??
                         type.GetProperty($"{type.Name}ID");
            }

            return pkProp ?? throw new InvalidOperationException($"无法确定实体 {type.Name} 的主键属性");
        }

        /// <summary>
        /// 解析 SQL Server 外键约束错误信息
        /// </summary>
        private (string TableName, string ColumnName)? ParseForeignKeyError(string errorMessage)
        {
            // 示例: DELETE 语句与 REFERENCE 约束"FK_PURENTRY_PURENTRYDETAIL"冲突。
            // 该冲突发生于数据库"erpnew"，表"dbo.tb_PurEntryDetail", column 'PurEntryID'。

            // 修复后的正则：匹配 表"xxx" 或 表 'xxx' 或 表 xxx
            var tableMatch = System.Text.RegularExpressions.Regex.Match(errorMessage, @"表\s*[""'`]?(?<table>[\w\.]+)[""'`]?");
            var colMatch = System.Text.RegularExpressions.Regex.Match(errorMessage, @"column\s*['""](?<col>[^'""]+)['""]|字段\s*['""](?<col2>[^'""]+)['""]");

            if (tableMatch.Success && colMatch.Success)
            {
                string tableName = tableMatch.Groups["table"].Value;
                // 去除可能的 dbo. 前缀
                if (tableName.Contains(".")) tableName = tableName.Split('.')[1];

                // 获取列名，兼容 column 'xxx' 和 字段 'xxx'
                string columnName = !string.IsNullOrEmpty(colMatch.Groups["col"].Value)
                    ? colMatch.Groups["col"].Value
                    : colMatch.Groups["col2"].Value;
                return (tableName, columnName);
            }
            return null;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        private void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            OnLog?.Invoke(this, logEntry);
            _logger?.LogInformation(message);
        }        /// <summary>


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



