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
        /// 根据实体名称获取元数据
        /// </summary>
        public static EntityMetadata GetMetadata(string entityName)
        {
            var cacheKey = $"RUINORERP.Model.{entityName}";
            if (_cache.TryGetValue(cacheKey, out var cached)) return cached;

            try
            {
                var assembly = Assembly.Load("RUINORERP.Model");
                var entityType = assembly.GetTypes().FirstOrDefault(t => 
                    t.Name == entityName && typeof(BaseEntity).IsAssignableFrom(t));

                if (entityType == null)
                    throw new Exception($"无法找到实体类型: {entityName}");

                return GetMetadata(entityType);
            }
            catch (Exception ex)
            {
                throw new Exception($"加载实体 {entityName} 失败: {ex.Message}");
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
    /// 依赖图构建器 - 从实体元数据构建依赖关系图
    /// </summary>
    public class DependencyGraphBuilder
    {
        private readonly Dictionary<string, List<string>> _graph = new();
        private readonly Dictionary<string, HashSet<string>> _reverseGraph = new();
        private readonly Dictionary<string, EntityMetadata> _metadataCache = new();

        /// <summary>
        /// 从根实体构建依赖图
        /// </summary>
        public DependencyGraphBuilder BuildFromRoot<T>() where T : BaseEntity, new()
        {
            var rootType = typeof(T);
            BuildGraphRecursive(rootType.Name, new Dictionary<string, bool>());
            return this;
        }

        /// <summary>
        /// 递归构建依赖图
        /// </summary>
        private void BuildGraphRecursive(string entityName, Dictionary<string, bool> visited)
        {
            if (visited.ContainsKey(entityName))
                return;

            visited[entityName] = true;

            var metadata = ModelMetadataHelper.GetMetadata(entityName);
            _metadataCache[entityName] = metadata;

            if (!_graph.ContainsKey(entityName))
                _graph[entityName] = new List<string>();

            foreach (var childRelation in metadata.ChildRelations)
            {
                var childTableName = childRelation.ChildTableName;

                if (!_graph.ContainsKey(entityName))
                    _graph[entityName] = new List<string>();

                if (!_graph[entityName].Contains(childTableName))
                    _graph[entityName].Add(childTableName);

                if (!_reverseGraph.ContainsKey(childTableName))
                    _reverseGraph[childTableName] = new HashSet<string>();

                _reverseGraph[childTableName].Add(entityName);

                BuildGraphRecursive(childTableName, visited);
            }
        }

        /// <summary>
        /// 获取依赖图
        /// </summary>
        public Dictionary<string, List<string>> GetGraph()
        {
            return _graph;
        }

        /// <summary>
        /// 获取反向依赖图（子表 -> 父表）
        /// </summary>
        public Dictionary<string, HashSet<string>> GetReverseGraph()
        {
            return _reverseGraph;
        }

        /// <summary>
        /// 获取所有节点
        /// </summary>
        public HashSet<string> GetAllNodes()
        {
            var nodes = new HashSet<string>(_graph.Keys);
            foreach (var keys in _reverseGraph.Keys)
            {
                nodes.Add(keys);
            }
            return nodes;
        }

        /// <summary>
        /// 获取实体的元数据
        /// </summary>
        public EntityMetadata GetMetadata(string entityName)
        {
            if (_metadataCache.TryGetValue(entityName, out var metadata))
                return metadata;

            return ModelMetadataHelper.GetMetadata(entityName);
        }
    }

    /// <summary>
    /// 拓扑排序器 - 计算正确的删除顺序（自底向上）
    /// </summary>
    public class TopologicalSorter
    {
        /// <summary>
        /// 执行拓扑排序，返回自底向上的删除顺序
        /// 删除顺序：先删子表（叶子节点），后删父表（根节点）
        /// </summary>
        /// <returns>排序后的表列表（叶子节点在前，根节点在后）</returns>
        public List<DeleteStage> Sort(DependencyGraphBuilder graphBuilder)
        {
            var graph = graphBuilder.GetGraph();
            var reverseGraph = graphBuilder.GetReverseGraph();
            var allNodes = graphBuilder.GetAllNodes();

            // outDegree: 该表有多少个子表（被它引用的表）
            var outDegree = new Dictionary<string, int>();
            foreach (var node in allNodes)
            {
                outDegree[node] = graph.ContainsKey(node) ? graph[node].Count : 0;
            }

            var result = new List<DeleteStage>();
            var processed = new HashSet<string>();

            while (processed.Count < allNodes.Count)
            {
                // 找没有子表的节点（叶子节点），先删除
                var currentLevel = outDegree
                    .Where(kv => !processed.Contains(kv.Key) && kv.Value == 0)
                    .Select(kv => kv.Key)
                    .ToList();

                if (currentLevel.Count == 0 && processed.Count < allNodes.Count)
                {
                    throw new InvalidOperationException("依赖图存在循环引用，无法完成拓扑排序");
                }

                var stage = new DeleteStage
                {
                    StageNumber = result.Count + 1
                };

                foreach (var node in currentLevel)
                {
                    var metadata = graphBuilder.GetMetadata(node);
                    stage.Tables.Add(new DeleteTableInfo
                    {
                        TableName = node,
                        PrimaryKeyColumn = metadata?.PrimaryKeyName ?? "PrimaryKeyID"
                    });
                    processed.Add(node);
                }

                // 更新父节点的outDegree（删除了子节点后，父节点就少了一个子依赖）
                foreach (var node in currentLevel)
                {
                    if (reverseGraph.ContainsKey(node))
                    {
                        foreach (var parent in reverseGraph[node])
                        {
                            if (outDegree.ContainsKey(parent))
                                outDegree[parent]--;
                        }
                    }
                }

                if (stage.Tables.Count > 0)
                    result.Add(stage);
            }

            return result;
        }
    }

    /// <summary>
    /// 删除阶段
    /// </summary>
    public class DeleteStage
    {
        public int StageNumber { get; set; }
        public List<DeleteTableInfo> Tables { get; set; } = new();
    }

    /// <summary>
    /// 待删除表信息
    /// </summary>
    public class DeleteTableInfo
    {
        public string TableName { get; set; }
        public string PrimaryKeyColumn { get; set; }
    }

    /// <summary>
    /// 数据清理引擎 - 基于实体元数据的级联删除
    /// </summary>
    public class DataCleanupEngine
    {
        private readonly ILogger<DataCleanupEngine> _logger;
        private readonly IUnitOfWorkManage _unitOfWork;
        private readonly Dictionary<string, List<long>> _deletedIdsMap = new();

        public DataCleanupEngine()
        {
            _logger = Startup.GetFromFac<ILogger<DataCleanupEngine>>();
            _unitOfWork = Startup.GetFromFac<IUnitOfWorkManage>();
        }

        /// <summary>
        /// 执行级联删除 - 采用"元数据驱动"策略
        /// </summary>
        public async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync<T>(List<long> targetIds, bool isTestMode = false) where T : BaseEntity, new()
        {
            var result = new CascadeDeleteResult { IsTestMode = isTestMode, StartTime = DateTime.Now };
            if (!targetIds.Any()) return result;

            _deletedIdsMap.Clear();

            try
            {
                Log($"[启动] 元数据驱动级联删除：{typeof(T).Name}, ID数：{targetIds.Count}, 模式：{(isTestMode ? "测试" : "正式")}");

                var rootMetadata = ModelMetadataHelper.GetMetadata<T>();
                Log($"[元数据] 根实体: {rootMetadata.TableName}, 主键: {rootMetadata.PrimaryKeyName}");

                _deletedIdsMap[rootMetadata.EntityName] = targetIds;

                var graphBuilder = new DependencyGraphBuilder();
                graphBuilder.BuildFromRoot<T>();
                var allNodes = graphBuilder.GetAllNodes();
                Log($"[依赖图] 发现 {allNodes.Count} 个关联表");

                var sorter = new TopologicalSorter();
                var deleteStages = sorter.Sort(graphBuilder);
                Log($"[拓扑排序] 生成 {deleteStages.Count} 个删除阶段");

                for (int i = 0; i < deleteStages.Count; i++)
                {
                    var stage = deleteStages[i];
                    var tableNames = string.Join(", ", stage.Tables.Select(t => t.TableName));
                    Log($"[阶段 {i + 1}] {tableNames}");
                }

                await _unitOfWork.BeginTranAsync();
                try
                {
                    int totalDeleted = 0;
                    int currentStage = 1;
                    int totalStages = deleteStages.Count;

                    foreach (var stage in deleteStages)
                    {
                        foreach (var tableInfo in stage.Tables)
                        {
                            List<long> idsToDelete;

                            if (tableInfo.TableName == rootMetadata.EntityName)
                            {
                                idsToDelete = targetIds;
                            }
                            else
                            {
                                var parentInfo = FindParentInfo(tableInfo.TableName, deleteStages, currentStage, graphBuilder);
                                if (parentInfo == null)
                                {
                                    Log($"[阶段 {currentStage}] ⚠️ 无法确定 {tableInfo.TableName} 的父表关系，跳过");
                                    continue;
                                }

                                idsToDelete = await FindChildIdsAsync(
                                    tableInfo.TableName,
                                    tableInfo.PrimaryKeyColumn,
                                    parentInfo.Value.parentTable,
                                    parentInfo.Value.fkColumn,
                                    parentInfo.Value.parentIds
                                );
                            }

                            if (!idsToDelete.Any())
                            {
                                Log($"[阶段 {currentStage}] ℹ️ {tableInfo.TableName} 无相关记录");
                                continue;
                            }

                            var deletedCount = await ExecuteDeleteAsync(
                                tableInfo.TableName,
                                tableInfo.PrimaryKeyColumn,
                                idsToDelete,
                                isTestMode
                            );

                            _deletedIdsMap[tableInfo.TableName] = idsToDelete;

                            totalDeleted += deletedCount;
                            Log($"[阶段 {currentStage}] ✓ 删除 {tableInfo.TableName} ({deletedCount}条)");

                            ReportProgress(currentStage, totalStages, $"正在删除 {tableInfo.TableName}");
                        }

                        currentStage++;
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
                catch (Exception)
                {
                    await _unitOfWork.RollbackTranAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"[错误] {ex.Message}";
                Log(errorMsg);
                Log($"[堆栈] {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Log($"[内部错误] {ex.InnerException.Message}");
                    Log($"[内部堆栈] {ex.InnerException.StackTrace}");
                }
                
                result.MarkAsFailed(ex.Message);
                _logger?.LogError(ex, "级联删除异常");
            }

            return result;
        }

        /// <summary>
        /// 查找父表信息
        /// </summary>
        private (string parentTable, string fkColumn, List<long> parentIds)? FindParentInfo(
            string childTableName,
            List<DeleteStage> stages,
            int currentStage,
            DependencyGraphBuilder graphBuilder)
        {
            for (int i = currentStage - 1; i >= 0; i--)
            {
                foreach (var tableInfo in stages[i].Tables)
                {
                    var metadata = graphBuilder.GetMetadata(tableInfo.TableName);
                    var childRelation = metadata.ChildRelations.FirstOrDefault(c => c.ChildTableName == childTableName);

                    if (childRelation != null)
                    {
                        if (_deletedIdsMap.TryGetValue(tableInfo.TableName, out var parentIds) && parentIds.Any())
                        {
                            return (tableInfo.TableName, childRelation.ForeignKeyColumn, parentIds);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查找子表中引用父表记录的ID
        /// </summary>
        private async Task<List<long>> FindChildIdsAsync(
            string childTableName,
            string childPkColumn,
            string parentTableName,
            string parentPkColumn,
            List<long> parentIds)
        {
            var db = _unitOfWork.GetDbClient();
            string fkColumn = string.Empty;

            try
            {
                // 尝试通过 Navigate 属性找到外键字段名
                var parentMetadata = ModelMetadataHelper.GetMetadata(parentTableName);
                var childRelation = parentMetadata.ChildRelations
                    .FirstOrDefault(c => c.ChildTableName == childTableName);

                if (childRelation != null)
                {
                    fkColumn = childRelation.ForeignKeyColumn;
                }
                else
                {
                    // 尝试常见的命名模式
                    fkColumn = $"{parentTableName.Replace("tb_", "")}_ID";
                }

                var sql = $"SELECT DISTINCT [{childPkColumn}] FROM [{childTableName}] WHERE [{fkColumn}] IN (@parentIds)";
                var ids = await db.Ado.SqlQueryAsync<long>(sql, new { parentIds = parentIds });
                return ids.ToList();
            }
            catch (Exception ex)
            {
                Log($"[查找子表] ⚠️ 查询 {childTableName} 失败: {ex.Message}");
                Log($"[查找子表] SQL: SELECT DISTINCT [{childPkColumn}] FROM [{childTableName}] WHERE [{fkColumn}] IN (...)");
                Log($"[查找子表] 父表: {parentTableName}, 父ID数量: {parentIds.Count}");
                Log($"[查找子表] 堆栈: {ex.StackTrace}");
                return new List<long>();
            }
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
