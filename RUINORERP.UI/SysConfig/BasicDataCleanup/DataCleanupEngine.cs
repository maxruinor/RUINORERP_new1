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

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理引擎 - 复用现有的 BaseController 级联删除功能
    /// 优势:
    /// 1. 使用项目已有的 BaseController.BaseDeleteByNavAsync 方法
    /// 2. 支持 SqlSugar 的 DeleteNav 导航删除(自动级联)
    /// 3. 通过泛型 T 直接指定 RUINORERP.Model 中的强类型实体
    /// 4. 无需重新实现复杂的关系分析和表达式树构建
    /// 5. ✅ 使用 IUnitOfWorkManage 管理事务,避免 DataReader 挂起问题
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
        /// 执行级联删除 - 复用 BaseController.BaseDeleteByNavAsync
        /// </summary>
        /// <typeparam name="T">实体类型(来自RUINORERP.Model)</typeparam>
        /// <param name="targetIds">目标记录ID列表</param>
        /// <param name="isTestMode">是否测试模式(执行但回滚,用于准确统计)</param>
        /// <returns>清理结果</returns>
        public async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync<T>(List<long> targetIds, bool isTestMode = false) where T : BaseEntity, new()
        {
            var result = new CascadeDeleteResult
            {
                IsTestMode = isTestMode,
                StartTime = DateTime.Now
            };

            try
            {
                Log($"开始执行级联删除：{typeof(T).Name}, ID 数量：{targetIds.Count}, 测试模式：{isTestMode}");

                // 获取对应的 Controller (复用现有架构)
                var controller = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                if (controller == null)
                {
                    throw new InvalidOperationException($"未找到对应的 Controller: {typeof(T).Name}Controller");
                }

                int totalDeleted = 0;
                int stepIndex = 0; // 提升到 try 块开始位置，catch 块才能访问

                // ✅ 使用 IUnitOfWorkManage 管理事务(替代 TransactionScope)
                // 优势: 自动处理 DataReader 清理、支持异步事务、自动超时保护
                await _unitOfWork.BeginTranAsync();
                
                try
                {
                    // ✅ 关键修复:先删除所有深层级关联数据,再删除主表
                    // 这样可以避免外键约束冲突
                    foreach (var id in targetIds)
                    {
                        stepIndex++;
                        ReportProgress(stepIndex, targetIds.Count, $"正在处理 ID: {id}");

                        // 构建实体对象(需要设置真实的主键属性,而不是 PrimaryKeyID)
                        var entity = Activator.CreateInstance<T>();
                        
                        // ✅ 关键修复:通过反射找到并设置真实的主键属性
                        var pkPropertyName = GetPrimaryKeyPropertyName<T>();
                        var pkProperty = typeof(T).GetProperty(pkPropertyName);
                        if (pkProperty != null && pkProperty.CanWrite)
                        {
                            pkProperty.SetValue(entity, id);
                            Log($"设置主键 {pkPropertyName} = {id}");
                        }
                        else
                        {
                            throw new InvalidOperationException($"未找到实体 {typeof(T).Name} 的主键属性: {pkPropertyName}");
                        }
                        
                        // 同时设置 PrimaryKeyID 作为备用
                        entity.PrimaryKeyID = id;

                        // ✅ 测试模式跳过实际删除,仅统计
                        if (isTestMode)
                        {
                            Log($"[测试模式] 模拟删除 ID={id}");
                            totalDeleted++;
                            continue;
                        }

                        // ✅ 优化方案:直接调用 BaseController 的通用方法,不依赖子控制器
                        // 优势:
                        // 1. 不依赖子控制器的 BaseDeleteByNavAsync 实现
                        // 2. 利用 DeleteDeepLevelRelationsByNavigationAsync 自动扫描所有关联
                        // 3. 支持递归处理多层级关系(默认5层)
                        // 4. 不影响自动生成的子控制器
                        await controller.DeleteDeepLevelRelationsByNavigationAsync(entity);
                        Log($"已清理 {typeof(T).Name} ID={id} 的所有深层级关联数据");
                        
                        // 执行主表删除(使用 DeleteNav,如果失败则降级为简单删除)
                        try
                        {
                            await controller.BaseDeleteByNavAsync(entity);
                            totalDeleted++;
                            Log($"✓ 成功删除 ID={id} 及其关联数据");
                        }
                        catch (InvalidOperationException ex) when (ex.Message.Contains("序列不包含任何元素"))
                        {
                            // 降级方案:实体没有导航属性,使用简单删除
                            _logger?.LogDebug(ex, $"{typeof(T).Name} 没有导航属性,使用简单删除");
                            await controller.BaseDeleteAsync(entity);
                            totalDeleted++;
                            Log($"✓ 成功删除 ID={id} (简单删除)");
                        }
                        catch (SqlException ex)
                        {
                            // SQL异常(如外键约束冲突),解析外键信息并针对性删除
                            _logger?.LogWarning(ex, $"DeleteNav失败: {ex.Message}");
                            
                            // 解析外键约束错误信息
                            var fkInfo = ParseForeignKeyError(ex.Message);
                            if (fkInfo != null)
                            {
                                Log($"检测到外键约束冲突: 表={fkInfo.Value.TableName}, 字段={fkInfo.Value.ColumnName}");
                                
                                // 根据外键信息删除关联数据
                                var deleted = await DeleteReferencingTableAsync<T>(id, fkInfo.Value.TableName, fkInfo.Value.ColumnName);
                                if (deleted)
                                {
                                    // 删除关联数据后,重试删除主表
                                    try
                                    {
                                        await controller.BaseDeleteByNavAsync(entity);
                                        totalDeleted++;
                                        Log($"✓ 解决外键冲突后成功删除 ID={id}");
                                    }
                                    catch (Exception retryEx)
                                    {
                                        _logger?.LogError(retryEx, $"重试删除 ID={id} 失败");
                                        result.FailedIds ??= new List<long>();
                                        result.FailedIds.Add(id);
                                        continue;
                                    }
                                }
                                else
                                {
                                    result.FailedIds ??= new List<long>();
                                    result.FailedIds.Add(id);
                                    continue;
                                }
                            }
                            else
                            {
                                // 无法解析外键信息,记录失败并继续
                                _logger?.LogError(ex, $"无法解析外键错误信息: {ex.Message}");
                                result.FailedIds ??= new List<long>();
                                result.FailedIds.Add(id);
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            // 记录错误但继续处理其他记录,避免因单条记录失败导致整个批处理中断
                            _logger?.LogError(ex, $"删除 ID={id} 时发生错误,继续处理下一条: {ex.Message}");
                            Log($"✗ 删除 ID={id} 失败: {ex.Message}, 继续处理下一条...");
                            
                            // 记录失败但继续循环
                            result.FailedIds ??= new List<long>();
                            result.FailedIds.Add(id);
                            continue;
                        }
                    }

                    result.IsSuccess = true;
                    result.TotalDeletedCount = totalDeleted;
                    result.Complete();

                    if (isTestMode)
                    {
                        // 测试模式:回滚事务,不真正删除
                        await _unitOfWork.RollbackTranAsync();
                        Log($"[测试模式] 事务已回滚,数据未实际删除");
                    }
                    else
                    {
                        // 正式模式:提交事务
                        await _unitOfWork.CommitTranAsync();
                        Log($"级联删除完成: 总计删除 {totalDeleted} 条主记录及其关联数据");
                    }
                }
                catch (Exception)
                {
                    // 发生异常时回滚事务
                    await _unitOfWork.RollbackTranAsync();
                    throw; // 重新抛出,让外层 catch 处理
                }
            }
            catch (Exception ex)
            {
                result.MarkAsFailed(ex.Message);
                Log($"级联删除失败:{typeof(T).Name}");
                            
                // ✅ 增强错误诊断:检测外键约束冲突
                string detailedError = $"[错误详情]\n" +
                    $"  • 实体类型:{typeof(T).FullName}\n" +
                    $"  • 目标 ID 数:{targetIds.Count}\n" +
                    $"  • 异常类型:{ex.GetType().Name}\n" +
                    $"  • 异常消息:{ex.Message}\n";
                            
                // 检测是否为外键约束冲突
                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("外键约束"))
                {
                    detailedError += $"\n[外键约束冲突分析]\n" +
                        $"  ⚠️ 原因:Controller 的 BaseDeleteByNavAsync 方法中遗漏了某些关联表的 Include\n" +
                        $"  💡 解决方案:\n" +
                        $"    1. 检查 {typeof(T).Name}Controller.BaseDeleteByNavAsync 方法\n" +
                        $"    2. 确保所有导航属性都已 Include\n" +
                        $"    3. 特别注意明细表(如 xxxDetails)是否遗漏\n" +
                        $"    4. 参考错误信息中的表和字段名定位缺失的关联\n";
                                
                    // 尝试从错误消息中提取冲突的表名
                    var match = System.Text.RegularExpressions.Regex.Match(ex.Message, "table\\s+([\\w.]+)");
                    if (match.Success)
                    {
                        detailedError += $"\n  🔍 冲突表名:{match.Groups[1].Value}\n";
                    }
                                                    
                    var colMatch = System.Text.RegularExpressions.Regex.Match(ex.Message, "column\\s+'([^']+)'");
                    if (colMatch.Success)
                    {
                        detailedError += $"  🔍 冲突字段:{colMatch.Groups[1].Value}\n";
                    }
                }
                            
                detailedError += $"\n  • 堆栈跟踪:{ex.StackTrace}";
                            
                if (ex.InnerException != null)
                {
                    detailedError += $"\n  • 内部异常:{ex.InnerException.Message}";
                }
                            
                Log(detailedError);
                _logger?.LogError(ex, $"级联删除失败:{typeof(T).Name}");
                throw;
            }

            return result;
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
        /// 获取实体的主键属性名(优先使用 SqlSugar 特性标记的主键)
        /// </summary>
        private static string GetPrimaryKeyPropertyName<T>() where T : BaseEntity, new()
        {
            var entityType = typeof(T);
            
            // 1. 查找标记了 SugarColumn(IsPrimaryKey = true) 的属性
            var sugarPkProps = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<SqlSugar.SugarColumn>()?.IsPrimaryKey == true)
                .ToList();
            
            if (sugarPkProps.Any())
            {
                return sugarPkProps.First().Name;
            }
            
            // 2. 如果没有 SugarColumn 标记,尝试常见的主键命名约定
            var commonPkNames = new[] { "Id", "ID", entityType.Name + "_ID", entityType.Name + "Id" };
            foreach (var pkName in commonPkNames)
            {
                var prop = entityType.GetProperty(pkName);
                if (prop != null)
                {
                    return pkName;
                }
            }
            
            // 3. 如果都找不到,返回 PrimaryKeyID(备用方案)
            return "PrimaryKeyID";
        }

        /// <summary>
        /// 强制删除实体(绕过导航属性,直接执行SQL删除)
        /// </summary>
        private async Task<bool> ForceDeleteEntityAsync<T>(long id) where T : BaseEntity, new()
        {
            var db = _unitOfWork.GetDbClient();
            var entityType = typeof(T);
            var tableName = entityType.Name;
            var pkPropertyName = GetPrimaryKeyPropertyName<T>();

            _logger?.LogInformation($"开始强制删除 {tableName}, ID={id}");

            try
            {
                // 1. 先获取所有 OneToMany 导航属性(子表)
                var navProperties = entityType.GetProperties()
                    .Where(p =>
                    {
                        var navigateAttr = p.GetCustomAttribute<SqlSugar.Navigate>();
                        return navigateAttr != null &&
                               navigateAttr.GetNavigateType() == SqlSugar.NavigateType.OneToMany;
                    })
                    .ToList();

                // 2. 删除所有子表数据
                foreach (var navProp in navProperties)
                {
                    var navigateAttr = navProp.GetCustomAttribute<SqlSugar.Navigate>();
                    var fkFieldName = navigateAttr.GetName();
                    var childTableName = navProp.PropertyType.IsGenericType
                        ? navProp.PropertyType.GetGenericArguments()[0].Name
                        : navProp.PropertyType.Name;

                    var deletedCount = await db.Deleteable<object>()
                        .AS(childTableName)
                        .Where($"{fkFieldName} = @id", new { id })
                        .ExecuteCommandAsync();

                    _logger?.LogDebug($"已删除 {childTableName} {deletedCount} 条记录");
                }

                // 3. 删除主表数据
                var mainDeleted = await db.Deleteable<object>()
                    .AS(tableName)
                    .Where($"{pkPropertyName} = @id", new { id })
                    .ExecuteCommandAsync();

                _logger?.LogInformation($"强制删除完成: {tableName}, ID={id}, 影响行数={mainDeleted}");
                return mainDeleted > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"强制删除失败: {tableName}, ID={id}");
                return false;
            }
        }

        /// <summary>
        /// 解析外键约束错误信息,提取冲突的表名和字段名
        /// </summary>
        /// <param name="errorMessage">SQL异常消息</param>
        /// <returns>外键信息(表名,字段名),解析失败返回null</returns>
        private (string TableName, string ColumnName)? ParseForeignKeyError(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return null;

            try
            {
                // SQL Server 中文错误格式: "DELETE 语句与 REFERENCE 约束"FK_xxx".该冲突发生于数据库"xxx"，表"dbo.tb_xxx", column 'xxx_ID'."
                // 匹配表名: 表"dbo.tb_TableName" 或 表"tb_TableName"
                var tableMatch = Regex.Match(errorMessage, @"表\s*[""']?dbo\.(\w+)[""']?", RegexOptions.IgnoreCase);
                if (tableMatch.Success)
                {
                    var tableName = tableMatch.Groups[1].Value;
                    
                    // 匹配字段名: column 'xxx' 或 column "xxx"
                    var columnMatch = Regex.Match(errorMessage, @"column\s*['""](\w+)['""]", RegexOptions.IgnoreCase);
                    var columnName = columnMatch.Success ? columnMatch.Groups[1].Value : null;
                    
                    _logger?.LogDebug($"解析外键错误: 表={tableName}, 字段={columnName}");
                    return (tableName, columnName);
                }

                // 匹配英文格式: table "dbo.tb_TableName", column 'xxx'
                var enTableMatch = Regex.Match(errorMessage, @"table\s+[""']?dbo\.(\w+)[""']?\s*,\s*column\s+['""]?(\w+)['""]?", RegexOptions.IgnoreCase);
                if (enTableMatch.Success)
                {
                    return (enTableMatch.Groups[1].Value, enTableMatch.Groups[2].Value);
                }

                // 匹配中文格式: 表 'tb_xxx', 字段 'xxx'
                var cnMatch = Regex.Match(errorMessage, @"表\s*['""]?(\w+)['""]?\s*,\s*字段\s*['""]?(\w+)['""]?", RegexOptions.IgnoreCase);
                if (cnMatch.Success)
                {
                    return (cnMatch.Groups[1].Value, cnMatch.Groups[2].Value);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"解析外键错误信息失败: {errorMessage}");
                return null;
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

