using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理执行引擎 - 简洁版
    /// 基于导航属性的级联删除功能
    /// </summary>
    public class DataCleanupEngine
    {
        private readonly ISqlSugarClient _db;
        private readonly StringBuilder _logBuilder;

        /// <summary>
        /// 日志事件
        /// </summary>
        public event EventHandler<string> OnLog;

        /// <summary>
        /// 进度事件
        /// </summary>
        public event EventHandler<CleanupProgressEventArgs> OnProgress;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">数据库连接</param>
        public DataCleanupEngine(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logBuilder = new StringBuilder();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            _logBuilder.AppendLine(logEntry);
            OnLog?.Invoke(this, logEntry);
        }

        /// <summary>
        /// 报告进度
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <param name="message">进度消息</param>
        private void ReportProgress(int current, int total, string message)
        {
            OnProgress?.Invoke(this, new CleanupProgressEventArgs
            {
                Current = current,
                Total = total,
                Message = message,
                Percentage = total > 0 ? (int)((double)current / total * 100) : 0
            });
        }

        /// <summary>
        /// 获取执行日志
        /// </summary>
        /// <returns>日志内容</returns>
        public string GetLog()
        {
            return _logBuilder.ToString();
        }

        #region 级联删除引擎

        /// <summary>
        /// 基于导航属性的级联删除
        /// </summary>
        /// <typeparam name="T">根实体类型</typeparam>
        /// <param name="targetIds">目标记录ID列表</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>级联删除结果</returns>
        public async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync<T>(List<long> targetIds, bool isTestMode = false) where T : BaseEntity, new()
        {
            var result = new CascadeDeleteResult
            {
                IsTestMode = isTestMode,
                StartTime = DateTime.Now
            };

            try
            {
                Log($"开始执行级联删除: {typeof(T).Name}, ID数量: {targetIds.Count}, 测试模式: {isTestMode}");

                var analyzer = new EntityRelationshipAnalyzer(_db);

                // 1. 构建级联删除计划
                var cascadePlan = BuildCascadePlan(typeof(T), targetIds, analyzer);
                Log($"生成级联计划: 共 {cascadePlan.Steps.Count} 个步骤, 最大深度: {cascadePlan.MaxDepth}");

                // 2. 按依赖顺序执行删除(从叶子节点到根节点)
                int stepIndex = 0;
                var reversedSteps = cascadePlan.Steps.AsEnumerable().Reverse().ToList();
                foreach (var step in reversedSteps)
                {
                    stepIndex++;
                    step.ExecutionStartTime = DateTime.Now;

                    ReportProgress(stepIndex, cascadePlan.Steps.Count, $"正在清理: {step.TableName} (第{step.Depth + 1}层)");
                    Log($"执行步骤 {stepIndex}/{cascadePlan.Steps.Count}: 清理表 {step.TableName} (深度: {step.Depth})");

                    int deletedCount = await DeleteEntitiesByExpressionAsync(step.EntityType, step.FilterExpression, isTestMode);

                    step.ExecutedCount = deletedCount;
                    result.AddStepResult(step, deletedCount);

                    Log($"表 {step.TableName} 清理完成: 删除 {deletedCount} 条记录");
                }

                result.IsSuccess = true;
                result.Complete();

                Log($"级联删除完成: 总删除 {result.TotalDeletedCount} 条记录, 涉及 {result.AffectedTableCount} 个表, 耗时 {result.TotalElapsedMs}ms");
            }
            catch (Exception ex)
            {
                result.MarkAsFailed(ex.Message);
                Log($"级联删除失败: {ex.Message}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// 构建级联删除计划
        /// </summary>
        /// <param name="rootEntityType">根实体类型</param>
        /// <param name="rootIds">根实体ID列表</param>
        /// <param name="analyzer">关系分析器</param>
        /// <returns>级联删除计划</returns>
        private CascadeDeletePlan BuildCascadePlan(Type rootEntityType, List<long> rootIds, EntityRelationshipAnalyzer analyzer)
        {
            var plan = new CascadeDeletePlan
            {
                RootEntityType = rootEntityType,
                RootEntityIds = rootIds
            };

            var visited = new HashSet<string>();

            void Visit(Type entityType, LambdaExpression filterExpr, int depth, string fkField, Type parentType)
            {
                if (visited.Contains(entityType.FullName))
                    return;

                visited.Add(entityType.FullName);

                var entityInfo = analyzer.GetEntityRelationship(entityType);
                var tableName = GetTableNameFromType(entityType);

                // 添加当前层的删除步骤
                var step = new CascadeDeleteStep
                {
                    EntityType = entityType,
                    TableName = tableName,
                    FilterExpression = filterExpr,
                    Depth = depth,
                    ForeignKeyField = fkField,
                    ParentEntityType = parentType
                };

                plan.Steps.Add(step);
                plan.MaxDepth = Math.Max(plan.MaxDepth, depth);

                // 递归处理OneToMany导航(被其他表引用的情况)
                foreach (var navRelation in entityInfo.NavigationRelations)
                {
                    if (navRelation.Type == ForeignKeyType.ReferencedByOthers && navRelation.RelatedEntityType != null)
                    {
                        // 构建子表的过滤条件: child.ForeignKeyField IN (parentIds)
                        var childFilter = BuildChildFilterExpression(navRelation.RelatedEntityType, navRelation.ForeignKeyColumn, filterExpr);

                        Visit(navRelation.RelatedEntityType, childFilter, depth + 1, navRelation.ForeignKeyColumn, entityType);
                    }
                }
            }

            // 从根节点开始: WHERE Id IN (targetIds)
            var rootFilter = BuildIdInFilterExpression(rootEntityType, rootIds);
            Visit(rootEntityType, rootFilter, 0, null, null);

            return plan;
        }

        /// <summary>
        /// 构建子表过滤表达式
        /// </summary>
        private LambdaExpression BuildChildFilterExpression(Type childEntityType, string fkField, LambdaExpression parentFilter)
        {
            try
            {
                var parameter = Expression.Parameter(childEntityType, "x");
                var fkProperty = Expression.Property(parameter, fkField);

                // 从父过滤器中提取ID列表
                var ids = ExtractIdsFromFilter(parentFilter);

                if (ids != null && ids.Count > 0)
                {
                    var idsConstant = Expression.Constant(ids);
                    var containsMethod = typeof(List<long>).GetMethod("Contains", new[] { typeof(long) });
                    var containsCall = Expression.Call(idsConstant, containsMethod, fkProperty);
                    return Expression.Lambda(containsCall, parameter);
                }
            }
            catch (Exception ex)
            {
                Log($"构建子表过滤表达式失败: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 构建ID IN过滤表达式
        /// </summary>
        private LambdaExpression BuildIdInFilterExpression(Type entityType, List<long> ids)
        {
            try
            {
                var parameter = Expression.Parameter(entityType, "x");
                
                // 尝试获取主键字段
                var pkProperty = entityType.GetProperty($"{entityType.Name}ID") ?? 
                                entityType.GetProperty("ID") ?? 
                                entityType.GetProperty("Id");
                                
                if (pkProperty == null)
                {
                    Log($"无法找到实体 {entityType.Name} 的主键字段");
                    return null;
                }

                var idProperty = Expression.Property(parameter, pkProperty.Name);
                var idsConstant = Expression.Constant(ids);
                var containsMethod = typeof(List<long>).GetMethod("Contains", new[] { typeof(long) });
                var containsCall = Expression.Call(idsConstant, containsMethod, idProperty);
                
                return Expression.Lambda(containsCall, parameter);
            }
            catch (Exception ex)
            {
                Log($"构建ID过滤表达式失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 从过滤器中提取ID列表
        /// </summary>
        private List<long> ExtractIdsFromFilter(LambdaExpression filter)
        {
            try
            {
                // 简化实现：假设过滤器是 x => ids.Contains(x.Id) 形式
                if (filter.Body is MethodCallExpression methodCall &&
                    methodCall.Method.Name == "Contains" &&
                    methodCall.Object is ConstantExpression constant)
                {
                    return constant.Value as List<long>;
                }
            }
            catch (Exception ex)
            {
                Log($"提取ID列表失败: {ex.Message}");
            }

            return new List<long>();
        }

        /// <summary>
        /// 根据表达式删除实体
        /// </summary>
        private async Task<int> DeleteEntitiesByExpressionAsync(Type entityType, LambdaExpression filterExpression, bool isTestMode)
        {
            if (filterExpression == null)
                return 0;

            try
            {
                // 将 LambdaExpression 转换为泛型表达式 Expression<Func<T,bool>>
                var lambdaType = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));
                var genericLambda = Expression.Lambda(lambdaType, filterExpression.Body, filterExpression.Parameters);
                        
                if (isTestMode)
                {
                    // 测试模式:只统计不删除
                    var countMethod = typeof(ISqlSugarClient).GetMethod("Queryable")?.MakeGenericMethod(entityType);
                    if (countMethod != null)
                    {
                        var queryable = countMethod.Invoke(_db, null);
                        var whereMethod = queryable.GetType().GetMethods()
                            .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1 && 
                                               m.GetParameters()[0].ParameterType.IsGenericType &&
                                               m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

                        if (whereMethod != null)
                        {
                            queryable = whereMethod.Invoke(queryable, new object[] { genericLambda });
                            var countAsyncMethod = queryable.GetType().GetMethod("CountAsync");
                            var countTask = (Task<int>)countAsyncMethod.Invoke(queryable, null);
                            return await countTask;
                        }
                    }
                }
                else
                {
                    // 正式删除:使用SqlSugar的Deleteable
                    var deleteMethod = typeof(ISqlSugarClient).GetMethod("Deleteable")?.MakeGenericMethod(entityType);
                    if (deleteMethod != null)
                    {
                        var deleteable = deleteMethod.Invoke(_db, null);

                        // 调用Where
                        var whereMethod = deleteable.GetType().GetMethods()
                            .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1 &&
                                               m.GetParameters()[0].ParameterType.IsGenericType &&
                                               m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

                        if (whereMethod != null)
                        {
                            deleteable = whereMethod.Invoke(deleteable, new object[] { genericLambda });
                            var executeMethod = deleteable.GetType().GetMethod("ExecuteCommandAsync");
                            var deleteTask = (Task<int>)executeMethod.Invoke(deleteable, null);
                            return await deleteTask;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"删除实体失败 [{entityType.Name}]: {ex.Message}");
                throw;
            }

            return 0;
        }

        /// <summary>
        /// 从实体类型获取表名
        /// </summary>
        private string GetTableNameFromType(Type entityType)
        {
            // 尝试从 SugarTable 特性获取表名
            var sugarTableAttr = entityType.GetCustomAttributes(typeof(SugarTable), false)
                .FirstOrDefault() as SugarTable;
            
            if (sugarTableAttr != null && !string.IsNullOrEmpty(sugarTableAttr.TableName))
            {
                return sugarTableAttr.TableName;
            }

            // 默认使用类名
            return entityType.Name;
        }

        #endregion
    }
}

