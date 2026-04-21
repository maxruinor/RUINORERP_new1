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
    /// 数据清理执行引擎
    /// 负责执行数据清理规则
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
        /// 预览清理效果
        /// </summary>
        /// <param name="config">清理配置</param>
        /// <param name="maxPreviewRecords">最大预览记录数</param>
        /// <returns>预览结果</returns>
        public async Task<CleanupPreviewResult> PreviewCleanupAsync(CleanupConfiguration config, int maxPreviewRecords = 100)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var result = new CleanupPreviewResult
            {
                ConfigId = config.ConfigId,
                ConfigName = config.ConfigName
            };

            Log($"开始生成清理预览: {config.ConfigName}");

            try
            {
                // 获取实体类型
                Type entityType = GetEntityType(config.TargetEntityType);
                if (entityType == null)
                {
                    throw new InvalidOperationException($"找不到实体类型: {config.TargetEntityType}");
                }

                // 获取总记录数
                result.TotalRecordCount = await GetTotalRecordCountAsync(entityType);
                Log($"目标表总记录数: {result.TotalRecordCount}");

                // 获取启用的规则
                var enabledRules = config.GetEnabledRules();
                Log($"启用的清理规则数: {enabledRules.Count}");

                // 对每个规则进行预览
                foreach (var rule in enabledRules)
                {
                    var matchCount = await PreviewRuleAsync(entityType, rule, maxPreviewRecords);
                    result.RuleMatchCounts[rule.RuleName] = matchCount;
                    Log($"规则 '{rule.RuleName}' 匹配记录数: {matchCount}");

                    // 根据操作类型分类预览
                    if (matchCount > 0)
                    {
                        var previewData = await GetPreviewDataAsync(entityType, rule, Math.Min(maxPreviewRecords, 10));
                        switch (rule.ActionType)
                        {
                            case CleanupActionType.Delete:
                                result.RecordsToDelete.AddRange(previewData);
                                break;
                            case CleanupActionType.UpdateField:
                                result.RecordsToUpdate.AddRange(previewData);
                                break;
                            case CleanupActionType.MarkAsInvalid:
                                result.RecordsToMark.AddRange(previewData);
                                break;
                            case CleanupActionType.Archive:
                                result.RecordsToArchive.AddRange(previewData);
                                break;
                        }
                    }
                }

                Log("预览生成完成");
                return result;
            }
            catch (Exception ex)
            {
                Log($"预览生成失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 执行数据清理
        /// </summary>
        /// <param name="config">清理配置</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>执行结果</returns>
        public async Task<CleanupExecutionResult> ExecuteCleanupAsync(CleanupConfiguration config, bool isTestMode = false)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            string errorMessage;
            if (!config.Validate(out errorMessage))
            {
                throw new InvalidOperationException($"配置验证失败: {errorMessage}");
            }

            var result = new CleanupExecutionResult
            {
                ConfigId = config.ConfigId,
                ConfigName = config.ConfigName,
                IsTestMode = isTestMode
            };

            Log($"开始执行数据清理: {config.ConfigName}");
            Log($"执行模式: {(isTestMode ? "测试模式" : "正式执行")}");

            try
            {
                // 获取实体类型
                Type entityType = GetEntityType(config.TargetEntityType);
                if (entityType == null)
                {
                    throw new InvalidOperationException($"找不到实体类型: {config.TargetEntityType}");
                }

                // 获取总记录数
                result.TotalRecordCount = await GetTotalRecordCountAsync(entityType);

                // 如果需要备份且不是测试模式
                if (config.EnableBackup && !isTestMode)
                {
                    result.BackupTableName = await BackupDataAsync(entityType, config.BackupTableSuffix);
                    Log($"数据已备份到表: {result.BackupTableName}");
                }

                // 获取启用的规则
                var enabledRules = config.GetEnabledRules();
                int totalRules = enabledRules.Count;
                int currentRule = 0;

                // 执行每个规则
                foreach (var rule in enabledRules)
                {
                    currentRule++;
                    ReportProgress(currentRule, totalRules, $"正在执行规则: {rule.RuleName}");

                    var ruleResult = await ExecuteRuleAsync(entityType, rule, isTestMode, config);
                    result.RuleResults.Add(ruleResult);

                    // 统计结果
                    result.MatchedRecordCount += ruleResult.MatchedCount;
                    result.SuccessCount += ruleResult.SuccessCount;
                    result.FailedCount += ruleResult.FailedCount;
                    result.SkippedCount += ruleResult.SkippedCount;

                    // 统计操作类型
                    switch (rule.ActionType)
                    {
                        case CleanupActionType.Delete:
                            result.DeletedCount += ruleResult.SuccessCount;
                            break;
                        case CleanupActionType.MarkAsInvalid:
                            result.MarkedInvalidCount += ruleResult.SuccessCount;
                            break;
                        case CleanupActionType.Archive:
                            result.ArchivedCount += ruleResult.SuccessCount;
                            break;
                        case CleanupActionType.UpdateField:
                            result.UpdatedCount += ruleResult.SuccessCount;
                            break;
                        case CleanupActionType.LogOnly:
                            result.LogOnlyCount += ruleResult.MatchedCount;
                            break;
                    }

                    Log($"规则 '{rule.RuleName}' 执行完成: 成功 {ruleResult.SuccessCount}, 失败 {ruleResult.FailedCount}");
                }

                result.IsSuccess = result.FailedCount == 0;
                result.Complete();

                Log($"数据清理执行完成: 总耗时 {result.ElapsedMilliseconds}ms");
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.Complete();
                Log($"数据清理执行失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 获取实体类型
        /// </summary>
        /// <param name="entityTypeName">实体类型名称</param>
        /// <returns>实体类型</returns>
        private Type GetEntityType(string entityTypeName)
        {
            // 从当前程序集和RUINORERP.Model程序集中查找
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("RUINORERP.Model")
            };

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetTypes().FirstOrDefault(t =>
                    t.Name == entityTypeName ||
                    t.FullName == entityTypeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>记录数</returns>
        private async Task<int> GetTotalRecordCountAsync(Type entityType)
        {
            var queryMethod = typeof(ISqlSugarClient).GetMethod("Queryable").MakeGenericMethod(entityType);
            var queryable = queryMethod.Invoke(_db, null);
            var countMethod = queryable.GetType().GetMethod("CountAsync");
            var countTask = (Task<int>)countMethod.Invoke(queryable, null);
            return await countTask;
        }

        /// <summary>
        /// 备份数据
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="suffix">备份表后缀</param>
        /// <returns>备份表名</returns>
        private async Task<string> BackupDataAsync(Type entityType, string suffix)
        {
            string tableName = entityType.Name;
            // 清理后缀中的非法字符，只允许字母、数字和下划线
            string safeSuffix = SanitizeSqlIdentifier(suffix);
            string backupTableName = $"{tableName}_{safeSuffix}";

            try
            {
                // 检查备份表是否存在，使用参数化查询
                var checkParams = new { TableName = backupTableName };
                string checkSql = @"
                    SELECT COUNT(*) FROM sys.tables WHERE name = @TableName";
                
                int tableExists = await _db.Ado.GetIntAsync(checkSql, checkParams);
                
                if (tableExists > 0)
                {
                    // 删除已存在的备份表，使用参数化表名
                    string dropSql = $"DROP TABLE IF EXISTS [{backupTableName}]";
                    await _db.Ado.ExecuteCommandAsync(dropSql);
                }

                // 创建备份表，使用参数化查询
                string backupSql = $@"
                    SELECT * INTO [{backupTableName}]
                    FROM [{tableName}]";

                await _db.Ado.ExecuteCommandAsync(backupSql);

                Log($"数据备份成功: {tableName} -> {backupTableName}");
                return backupTableName;
            }
            catch (Exception ex)
            {
                Log($"数据备份失败: {ex.Message}");
                throw new InvalidOperationException($"备份表 {backupTableName} 创建失败", ex);
            }
        }

        /// <summary>
        /// 清理SQL标识符，移除非法字符
        /// </summary>
        /// <param name="identifier">原始标识符</param>
        /// <returns>安全的标识符</returns>
        private string SanitizeSqlIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return "Backup";
            }

            // 只允许字母、数字和下划线
            var sb = new StringBuilder();
            foreach (char c in identifier)
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('_');
                }
            }

            string result = sb.ToString();
            
            // 确保不以数字开头
            if (result.Length > 0 && char.IsDigit(result[0]))
            {
                result = "_" + result;
            }

            // 限制长度
            if (result.Length > 100)
            {
                result = result.Substring(0, 100);
            }

            return result;
        }

        /// <summary>
        /// 预览规则效果
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="rule">清理规则</param>
        /// <param name="maxRecords">最大记录数</param>
        /// <returns>匹配记录数</returns>
        private async Task<int> PreviewRuleAsync(Type entityType, CleanupRule rule, int maxRecords)
        {
            try
            {
                var query = BuildQuery(entityType, rule);
                var countMethod = query.GetType().GetMethod("CountAsync");
                var countTask = (Task<int>)countMethod.Invoke(query, null);
                return await countTask;
            }
            catch (Exception ex)
            {
                Log($"预览规则 '{rule.RuleName}' 失败: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 获取预览数据
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="rule">清理规则</param>
        /// <param name="maxRecords">最大记录数</param>
        /// <returns>预览数据</returns>
        private async Task<List<Dictionary<string, object>>> GetPreviewDataAsync(Type entityType, CleanupRule rule, int maxRecords)
        {
            var result = new List<Dictionary<string, object>>();

            try
            {
                var query = BuildQuery(entityType, rule);
                var takeMethod = query.GetType().GetMethod("Take", new[] { typeof(int) });
                query = takeMethod.Invoke(query, new object[] { maxRecords });

                var toListMethod = query.GetType().GetMethod("ToListAsync");
                var task = (Task)toListMethod.Invoke(query, null);
                await task;

                var records = (IEnumerable<object>)task.GetType().GetProperty("Result").GetValue(task);

                foreach (var record in records)
                {
                    var dict = new Dictionary<string, object>();
                    var properties = record.GetType().GetProperties();
                    foreach (var prop in properties.Take(10)) // 只取前10个字段
                    {
                        try
                        {
                            dict[prop.Name] = prop.GetValue(record);
                        }
                        catch
                        {
                            dict[prop.Name] = null;
                        }
                    }
                    result.Add(dict);
                }
            }
            catch (Exception ex)
            {
                Log($"获取预览数据失败: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 执行清理规则
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="rule">清理规则</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <param name="config">清理配置</param>
        /// <returns>规则执行结果</returns>
        private async Task<RuleExecutionResult> ExecuteRuleAsync(Type entityType, CleanupRule rule, bool isTestMode, CleanupConfiguration config)
        {
            var result = new RuleExecutionResult
            {
                RuleId = rule.RuleId,
                RuleName = rule.RuleName,
                RuleType = rule.RuleType
            };

            try
            {
                // 获取匹配的记录
                var query = BuildQuery(entityType, rule);
                var toListMethod = query.GetType().GetMethod("ToListAsync");
                var task = (Task)toListMethod.Invoke(query, null);
                await task;

                var records = (IEnumerable<object>)task.GetType().GetProperty("Result").GetValue(task);
                var recordList = records.Cast<object>().ToList();

                result.MatchedCount = recordList.Count;

                if (recordList.Count == 0)
                {
                    result.IsSuccess = true;
                    return result;
                }

                // 如果是仅记录模式或测试模式，只记录不执行
                if (rule.ActionType == CleanupActionType.LogOnly || isTestMode)
                {
                    foreach (var record in recordList)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(record),
                            RecordIdentifier = GetRecordIdentifier(record),
                            IsSuccess = true,
                            ActionType = rule.ActionType,
                            AppliedRuleId = rule.RuleId,
                            AppliedRuleName = rule.RuleName
                        };
                        result.RecordResults.Add(recordResult);
                    }
                    result.SuccessCount = recordList.Count;
                    result.IsSuccess = true;
                    return result;
                }

                // 根据操作类型执行相应的清理操作
                switch (rule.ActionType)
                {
                    case CleanupActionType.Delete:
                        await ExecuteDeleteAsync(entityType, recordList, result, config);
                        break;
                    case CleanupActionType.MarkAsInvalid:
                        await ExecuteMarkAsInvalidAsync(entityType, recordList, rule, result, config);
                        break;
                    case CleanupActionType.Archive:
                        await ExecuteArchiveAsync(entityType, recordList, rule, result, config);
                        break;
                    case CleanupActionType.UpdateField:
                        await ExecuteUpdateAsync(entityType, recordList, rule, result, config);
                        break;
                }

                result.IsSuccess = result.FailedCount == 0;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                Log($"执行规则 '{rule.RuleName}' 失败: {ex.Message}");
            }

            result.EndTime = DateTime.Now;
            result.ElapsedMilliseconds = (long)(result.EndTime - result.StartTime).TotalMilliseconds;

            return result;
        }

        /// <summary>
        /// 构建查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="rule">清理规则</param>
        /// <returns>查询对象</returns>
        private object BuildQuery(Type entityType, CleanupRule rule)
        {
            var queryMethod = typeof(ISqlSugarClient).GetMethod("Queryable").MakeGenericMethod(entityType);
            var queryable = queryMethod.Invoke(_db, null);

            // 根据规则类型构建不同的查询条件
            switch (rule.RuleType)
            {
                case CleanupRuleType.DuplicateRemoval:
                    // 重复数据查询逻辑较复杂，需要在ExecuteRule中处理
                    break;

                case CleanupRuleType.EmptyValueRemoval:
                    queryable = BuildEmptyValueQuery(queryable, rule);
                    break;

                case CleanupRuleType.ExpiredDataRemoval:
                    queryable = BuildExpiredDataQuery(queryable, rule);
                    break;

                case CleanupRuleType.InvalidReferenceRemoval:
                    queryable = BuildInvalidReferenceQuery(queryable, rule);
                    break;

                case CleanupRuleType.CustomConditionRemoval:
                    queryable = BuildCustomConditionQuery(queryable, rule);
                    break;

                case CleanupRuleType.DataStandardization:
                    queryable = BuildStandardizationQuery(queryable, rule);
                    break;

                case CleanupRuleType.DataTruncation:
                    queryable = BuildTruncationQuery(queryable, rule);
                    break;
            }

            return queryable;
        }

        /// <summary>
        /// 构建空值查询
        /// </summary>
        private object BuildEmptyValueQuery(object queryable, CleanupRule rule)
        {
            if (rule.EmptyCheckFields == null || rule.EmptyCheckFields.Count == 0)
            {
                return queryable;
            }

            try
            {
                // 获取实体类型的属性信息
                var entityType = queryable.GetType().GetGenericArguments()[0];
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead)
                    .ToDictionary(p => p.Name, p => p.PropertyType);

                // 为每个空值检查字段构建Where条件
                foreach (var fieldName in rule.EmptyCheckFields)
                {
                    if (!properties.ContainsKey(fieldName))
                    {
                        Log($"字段 {fieldName} 不存在于实体 {entityType.Name}");
                        continue;
                    }

                    var propertyType = properties[fieldName];
                    
                    // 根据空值检查模式构建条件
                    switch (rule.EmptyValueMode)
                    {
                        case EmptyValueCheckMode.NullOnly:
                            // 仅检查NULL值
                            queryable = ApplyNullCheck(queryable, fieldName, propertyType, false);
                            break;
                        case EmptyValueCheckMode.NullOrEmpty:
                            // 检查NULL或空字符串
                            queryable = ApplyNullOrEmptyCheck(queryable, fieldName, propertyType);
                            break;
                        case EmptyValueCheckMode.NullOrWhiteSpace:
                            // 检查NULL、空字符串或空白字符
                            queryable = ApplyNullOrWhiteSpaceCheck(queryable, fieldName, propertyType);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"构建空值查询失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 应用NULL值检查
        /// </summary>
        private object ApplyNullCheck(object queryable, string fieldName, Type propertyType, bool includeEmpty)
        {
            try
            {
                // 使用反射调用Where方法
                var whereMethod = queryable.GetType().GetMethods()
                    .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);
                
                if (whereMethod == null)
                {
                    return queryable;
                }

                // 对于可空类型，检查是否为null
                if (Nullable.GetUnderlyingType(propertyType) != null || !propertyType.IsValueType)
                {
                    // 构建表达式: x => x.FieldName == null
                    var parameter = Expression.Parameter(queryable.GetType().GetGenericArguments()[0], "x");
                    var property = Expression.Property(parameter, fieldName);
                    var nullConstant = Expression.Constant(null);
                    var equalExpression = Expression.Equal(property, nullConstant);
                    var lambda = Expression.Lambda(equalExpression, parameter);
                    
                    queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                }
            }
            catch (Exception ex)
            {
                Log($"应用NULL检查失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 应用NULL或空字符串检查
        /// </summary>
        private object ApplyNullOrEmptyCheck(object queryable, string fieldName, Type propertyType)
        {
            try
            {
                if (propertyType == typeof(string) || propertyType == typeof(string))
                {
                    // 使用反射构建字符串.IsNullOrEmpty检查
                    var whereMethod = queryable.GetType().GetMethods()
                        .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);
                    
                    if (whereMethod != null)
                    {
                        var parameter = Expression.Parameter(queryable.GetType().GetGenericArguments()[0], "x");
                        var property = Expression.Property(parameter, fieldName);
                        var nullConstant = Expression.Constant(null);
                        var emptyConstant = Expression.Constant(string.Empty);
                        
                        // x.FieldName == null || x.FieldName == ""
                        var nullCheck = Expression.Equal(property, nullConstant);
                        var emptyCheck = Expression.Equal(property, emptyConstant);
                        var orExpression = Expression.OrElse(nullCheck, emptyCheck);
                        var lambda = Expression.Lambda(orExpression, parameter);
                        
                        queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                    }
                }
                else
                {
                    // 非字符串类型，只检查NULL
                    queryable = ApplyNullCheck(queryable, fieldName, propertyType, false);
                }
            }
            catch (Exception ex)
            {
                Log($"应用NULL或空字符串检查失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 应用NULL或空白字符检查
        /// </summary>
        private object ApplyNullOrWhiteSpaceCheck(object queryable, string fieldName, Type propertyType)
        {
            try
            {
                if (propertyType == typeof(string))
                {
                    // 对于空白字符检查，我们需要先获取数据然后在内存中过滤
                    // 这里简化处理，返回原始queryable，在实际执行时处理
                    Log($"字段 {fieldName} 的空白字符检查将在数据获取后执行");
                }
                else
                {
                    // 非字符串类型，只检查NULL
                    queryable = ApplyNullCheck(queryable, fieldName, propertyType, false);
                }
            }
            catch (Exception ex)
            {
                Log($"应用NULL或空白字符检查失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 构建过期数据查询
        /// </summary>
        private object BuildExpiredDataQuery(object queryable, CleanupRule rule)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rule.DateFieldName))
                {
                    Log("过期数据清理规则未指定日期字段");
                    return queryable;
                }

                var entityType = queryable.GetType().GetGenericArguments()[0];
                var property = entityType.GetProperty(rule.DateFieldName);
                
                if (property == null)
                {
                    Log($"日期字段 {rule.DateFieldName} 不存在于实体 {entityType.Name}");
                    return queryable;
                }

                // 计算过期日期
                DateTime expireDate = DateTime.Now.AddDays(-rule.ExpireDays);
                
                // 构建Where条件: x => x.DateField < expireDate
                var whereMethod = queryable.GetType().GetMethods()
                    .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);
                
                if (whereMethod != null)
                {
                    var parameter = Expression.Parameter(entityType, "x");
                    var dateProperty = Expression.Property(parameter, rule.DateFieldName);
                    var expireConstant = Expression.Constant(expireDate);
                    var lessThanExpression = Expression.LessThan(dateProperty, expireConstant);
                    var lambda = Expression.Lambda(lessThanExpression, parameter);
                    
                    queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                }
            }
            catch (Exception ex)
            {
                Log($"构建过期数据查询失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 构建无效关联查询
        /// </summary>
        private object BuildInvalidReferenceQuery(object queryable, CleanupRule rule)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rule.ForeignKeyField) || 
                    string.IsNullOrWhiteSpace(rule.ReferenceTable))
                {
                    Log("无效关联清理规则缺少必要参数");
                    return queryable;
                }

                // 获取外键字段的所有值
                var entityType = queryable.GetType().GetGenericArguments()[0];
                var fkProperty = entityType.GetProperty(rule.ForeignKeyField);
                
                if (fkProperty == null)
                {
                    Log($"外键字段 {rule.ForeignKeyField} 不存在于实体 {entityType.Name}");
                    return queryable;
                }

                // 查询关联表的所有有效ID
                string refKeyField = string.IsNullOrWhiteSpace(rule.ReferenceKeyField) ? "Id" : rule.ReferenceKeyField;
                
                // 使用SQL查询获取无效关联记录
                // 这里简化处理，实际应该使用SqlSugar的导航属性或子查询
                Log($"无效关联查询: {entityType.Name}.{rule.ForeignKeyField} -> {rule.ReferenceTable}.{refKeyField}");
            }
            catch (Exception ex)
            {
                Log($"构建无效关联查询失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 构建自定义条件查询
        /// </summary>
        private object BuildCustomConditionQuery(object queryable, CleanupRule rule)
        {
            try
            {
                if (rule.CustomConditions == null || rule.CustomConditions.Count == 0)
                {
                    return queryable;
                }

                var entityType = queryable.GetType().GetGenericArguments()[0];
                var parameter = Expression.Parameter(entityType, "x");
                Expression combinedExpression = null;

                for (int i = 0; i < rule.CustomConditions.Count; i++)
                {
                    var condition = rule.CustomConditions[i];
                    var conditionExpression = BuildConditionExpression(parameter, condition, entityType);
                    
                    if (conditionExpression == null)
                    {
                        continue;
                    }

                    if (combinedExpression == null)
                    {
                        combinedExpression = conditionExpression;
                    }
                    else
                    {
                        // 根据逻辑运算符组合条件
                        if (condition.LogicalOperator == LogicalOperator.And)
                        {
                            combinedExpression = Expression.AndAlso(combinedExpression, conditionExpression);
                        }
                        else
                        {
                            combinedExpression = Expression.OrElse(combinedExpression, conditionExpression);
                        }
                    }
                }

                if (combinedExpression != null)
                {
                    var whereMethod = queryable.GetType().GetMethods()
                        .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);
                    
                    if (whereMethod != null)
                    {
                        var lambda = Expression.Lambda(combinedExpression, parameter);
                        queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"构建自定义条件查询失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 构建单个条件表达式
        /// </summary>
        private Expression BuildConditionExpression(ParameterExpression parameter, ConditionItem condition, Type entityType)
        {
            try
            {
                var property = entityType.GetProperty(condition.FieldName);
                if (property == null)
                {
                    Log($"字段 {condition.FieldName} 不存在");
                    return null;
                }

                var propertyExpression = Expression.Property(parameter, condition.FieldName);
                var propertyType = property.PropertyType;
                
                // 处理可空类型
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                
                // 转换值为正确类型
                object convertedValue = ConvertValue(condition.Value, underlyingType);
                var valueExpression = Expression.Constant(convertedValue, propertyType);

                switch (condition.Operator)
                {
                    case ComparisonOperator.Equals:
                        return Expression.Equal(propertyExpression, valueExpression);
                    case ComparisonOperator.NotEquals:
                        return Expression.NotEqual(propertyExpression, valueExpression);
                    case ComparisonOperator.GreaterThan:
                        return Expression.GreaterThan(propertyExpression, valueExpression);
                    case ComparisonOperator.GreaterThanOrEqual:
                        return Expression.GreaterThanOrEqual(propertyExpression, valueExpression);
                    case ComparisonOperator.LessThan:
                        return Expression.LessThan(propertyExpression, valueExpression);
                    case ComparisonOperator.LessThanOrEqual:
                        return Expression.LessThanOrEqual(propertyExpression, valueExpression);
                    case ComparisonOperator.Contains:
                        return BuildContainsExpression(propertyExpression, condition.Value);
                    case ComparisonOperator.StartsWith:
                        return BuildStartsWithExpression(propertyExpression, condition.Value);
                    case ComparisonOperator.EndsWith:
                        return BuildEndsWithExpression(propertyExpression, condition.Value);
                    case ComparisonOperator.IsEmpty:
                        return BuildIsEmptyExpression(propertyExpression, propertyType);
                    case ComparisonOperator.IsNotEmpty:
                        return Expression.Not(BuildIsEmptyExpression(propertyExpression, propertyType));
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                Log($"构建条件表达式失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 构建Contains表达式
        /// </summary>
        private Expression BuildContainsExpression(Expression propertyExpression, string value)
        {
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var valueConstant = Expression.Constant(value);
            return Expression.Call(propertyExpression, containsMethod, valueConstant);
        }

        /// <summary>
        /// 构建StartsWith表达式
        /// </summary>
        private Expression BuildStartsWithExpression(Expression propertyExpression, string value)
        {
            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var valueConstant = Expression.Constant(value);
            return Expression.Call(propertyExpression, startsWithMethod, valueConstant);
        }

        /// <summary>
        /// 构建EndsWith表达式
        /// </summary>
        private Expression BuildEndsWithExpression(Expression propertyExpression, string value)
        {
            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var valueConstant = Expression.Constant(value);
            return Expression.Call(propertyExpression, endsWithMethod, valueConstant);
        }

        /// <summary>
        /// 构建IsEmpty表达式
        /// </summary>
        private Expression BuildIsEmptyExpression(Expression propertyExpression, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                var nullConstant = Expression.Constant(null);
                var emptyConstant = Expression.Constant(string.Empty);
                var nullCheck = Expression.Equal(propertyExpression, nullConstant);
                var emptyCheck = Expression.Equal(propertyExpression, emptyConstant);
                return Expression.OrElse(nullCheck, emptyCheck);
            }
            else
            {
                var nullConstant = Expression.Constant(null);
                return Expression.Equal(propertyExpression, nullConstant);
            }
        }

        /// <summary>
        /// 转换值到目标类型
        /// </summary>
        private object ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            try
            {
                if (targetType == typeof(string))
                {
                    return value;
                }
                else if (targetType == typeof(int) || targetType == typeof(int?))
                {
                    return int.Parse(value);
                }
                else if (targetType == typeof(long) || targetType == typeof(long?))
                {
                    return long.Parse(value);
                }
                else if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                {
                    return decimal.Parse(value);
                }
                else if (targetType == typeof(double) || targetType == typeof(double?))
                {
                    return double.Parse(value);
                }
                else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                {
                    return DateTime.Parse(value);
                }
                else if (targetType == typeof(bool) || targetType == typeof(bool?))
                {
                    return bool.Parse(value);
                }
                else
                {
                    return Convert.ChangeType(value, targetType);
                }
            }
            catch
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }
        }

        /// <summary>
        /// 构建标准化查询
        /// </summary>
        private object BuildStandardizationQuery(object queryable, CleanupRule rule)
        {
            // 标准化操作需要查询所有记录，不添加Where条件
            // 实际的标准化操作在ExecuteRule中处理
            return queryable;
        }

        /// <summary>
        /// 构建截断查询
        /// </summary>
        private object BuildTruncationQuery(object queryable, CleanupRule rule)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rule.TruncationField) || rule.MaxLength <= 0)
                {
                    return queryable;
                }

                var entityType = queryable.GetType().GetGenericArguments()[0];
                var property = entityType.GetProperty(rule.TruncationField);
                
                if (property == null || property.PropertyType != typeof(string))
                {
                    Log($"截断字段 {rule.TruncationField} 不存在或不是字符串类型");
                    return queryable;
                }

                // 构建Where条件: x => x.FieldName != null && x.FieldName.Length > MaxLength
                var whereMethod = queryable.GetType().GetMethods()
                    .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);
                
                if (whereMethod != null)
                {
                    var parameter = Expression.Parameter(entityType, "x");
                    var propertyExpression = Expression.Property(parameter, rule.TruncationField);
                    var nullConstant = Expression.Constant(null);
                    var notNullCheck = Expression.NotEqual(propertyExpression, nullConstant);
                    
                    var lengthProperty = Expression.Property(propertyExpression, "Length");
                    var maxLengthConstant = Expression.Constant(rule.MaxLength);
                    var lengthCheck = Expression.GreaterThan(lengthProperty, maxLengthConstant);
                    
                    var combinedExpression = Expression.AndAlso(notNullCheck, lengthCheck);
                    var lambda = Expression.Lambda(combinedExpression, parameter);
                    
                    queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                }
            }
            catch (Exception ex)
            {
                Log($"构建截断查询失败: {ex.Message}");
            }

            return queryable;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        private async Task ExecuteDeleteAsync(Type entityType, List<object> records, RuleExecutionResult result, CleanupConfiguration config)
        {
            int batchSize = config.TransactionBatchSize;
            int totalRecords = records.Count;
            int processedCount = 0;

            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = records.Skip(i).Take(batchSize).ToList();

                try
                {
                    var deleteMethod = typeof(ISqlSugarClient).GetMethod("Deleteable").MakeGenericMethod(entityType);
                    var deleteable = deleteMethod.Invoke(_db, new object[] { batch });
                    var executeMethod = deleteable.GetType().GetMethod("ExecuteCommandAsync");
                    var deleteTask = (Task<int>)executeMethod.Invoke(deleteable, null);
                    int deletedCount = await deleteTask;

                    for (int j = 0; j < batch.Count; j++)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(batch[j]),
                            RecordIdentifier = GetRecordIdentifier(batch[j]),
                            IsSuccess = true,
                            ActionType = CleanupActionType.Delete
                        };
                        result.RecordResults.Add(recordResult);
                    }

                    result.SuccessCount += batch.Count;
                    processedCount += batch.Count;

                    Log($"已删除 {processedCount}/{totalRecords} 条记录");
                }
                catch (Exception ex)
                {
                    foreach (var record in batch)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(record),
                            RecordIdentifier = GetRecordIdentifier(record),
                            IsSuccess = false,
                            ActionType = CleanupActionType.Delete,
                            ErrorMessage = ex.Message
                        };
                        result.RecordResults.Add(recordResult);
                    }
                    result.FailedCount += batch.Count;
                    Log($"删除记录失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 执行标记无效操作
        /// </summary>
        private async Task ExecuteMarkAsInvalidAsync(Type entityType, List<object> records, CleanupRule rule, RuleExecutionResult result, CleanupConfiguration config)
        {
            int batchSize = config.TransactionBatchSize;
            int totalRecords = records.Count;
            int processedCount = 0;

            // 查找Is_enabled或IsEnabled字段
            var isEnabledProperty = entityType.GetProperty("Is_enabled") ?? entityType.GetProperty("IsEnabled");
            if (isEnabledProperty == null)
            {
                Log($"实体 {entityType.Name} 没有Is_enabled或IsEnabled字段，无法标记为无效");
                result.FailedCount = records.Count;
                return;
            }

            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = records.Skip(i).Take(batchSize).ToList();

                try
                {
                    // 将Is_enabled设置为false
                    foreach (var record in batch)
                    {
                        isEnabledProperty.SetValue(record, false);
                    }

                    // 使用Updateable更新记录
                    var updateMethod = typeof(ISqlSugarClient).GetMethod("Updateable").MakeGenericMethod(entityType);
                    var updateable = updateMethod.Invoke(_db, new object[] { batch });
                    var executeMethod = updateable.GetType().GetMethod("ExecuteCommandAsync");
                    var updateTask = (Task<int>)executeMethod.Invoke(updateable, null);
                    int updatedCount = await updateTask;

                    for (int j = 0; j < batch.Count; j++)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(batch[j]),
                            RecordIdentifier = GetRecordIdentifier(batch[j]),
                            IsSuccess = true,
                            ActionType = CleanupActionType.MarkAsInvalid
                        };
                        result.RecordResults.Add(recordResult);
                    }

                    result.SuccessCount += batch.Count;
                    processedCount += batch.Count;

                    Log($"已标记 {processedCount}/{totalRecords} 条记录为无效");
                }
                catch (Exception ex)
                {
                    foreach (var record in batch)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(record),
                            RecordIdentifier = GetRecordIdentifier(record),
                            IsSuccess = false,
                            ActionType = CleanupActionType.MarkAsInvalid,
                            ErrorMessage = ex.Message
                        };
                        result.RecordResults.Add(recordResult);
                    }
                    result.FailedCount += batch.Count;
                    Log($"标记记录失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 执行归档操作
        /// </summary>
        private async Task ExecuteArchiveAsync(Type entityType, List<object> records, CleanupRule rule, RuleExecutionResult result, CleanupConfiguration config)
        {
            string archiveTableName = $"{entityType.Name}_Archive";
            int batchSize = config.TransactionBatchSize;
            int totalRecords = records.Count;
            int processedCount = 0;

            try
            {
                // 检查归档表是否存在，不存在则创建
                string checkTableSql = $@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{archiveTableName}')
                    BEGIN
                        SELECT * INTO [{archiveTableName}] FROM [{entityType.Name}] WHERE 1=0
                        ALTER TABLE [{archiveTableName}] ADD ArchiveTime DATETIME DEFAULT GETDATE()
                        ALTER TABLE [{archiveTableName}] ADD ArchiveReason NVARCHAR(500)
                    END";

                await _db.Ado.ExecuteCommandAsync(checkTableSql);

                for (int i = 0; i < records.Count; i += batchSize)
                {
                    var batch = records.Skip(i).Take(batchSize).ToList();

                    try
                    {
                        // 插入到归档表
                        var insertMethod = typeof(ISqlSugarClient).GetMethod("Insertable").MakeGenericMethod(entityType);
                        var insertable = insertMethod.Invoke(_db, new object[] { batch });
                        
                        // 设置AS方法指定归档表
                        var asMethod = insertable.GetType().GetMethod("AS", new[] { typeof(string) });
                        if (asMethod != null)
                        {
                            insertable = asMethod.Invoke(insertable, new object[] { archiveTableName });
                        }
                        
                        var executeMethod = insertable.GetType().GetMethod("ExecuteCommandAsync");
                        var insertTask = (Task<int>)executeMethod.Invoke(insertable, null);
                        int insertedCount = await insertTask;

                        // 从原表删除
                        var deleteMethod = typeof(ISqlSugarClient).GetMethod("Deleteable").MakeGenericMethod(entityType);
                        var deleteable = deleteMethod.Invoke(_db, new object[] { batch });
                        var deleteExecuteMethod = deleteable.GetType().GetMethod("ExecuteCommandAsync");
                        var deleteTask = (Task<int>)deleteExecuteMethod.Invoke(deleteable, null);
                        int deletedCount = await deleteTask;

                        for (int j = 0; j < batch.Count; j++)
                        {
                            var recordResult = new RecordCleanupResult
                            {
                                RecordId = GetRecordId(batch[j]),
                                RecordIdentifier = GetRecordIdentifier(batch[j]),
                                IsSuccess = true,
                                ActionType = CleanupActionType.Archive
                            };
                            result.RecordResults.Add(recordResult);
                        }

                        result.SuccessCount += batch.Count;
                        processedCount += batch.Count;

                        Log($"已归档 {processedCount}/{totalRecords} 条记录");
                    }
                    catch (Exception ex)
                    {
                        foreach (var record in batch)
                        {
                            var recordResult = new RecordCleanupResult
                            {
                                RecordId = GetRecordId(record),
                                RecordIdentifier = GetRecordIdentifier(record),
                                IsSuccess = false,
                                ActionType = CleanupActionType.Archive,
                                ErrorMessage = ex.Message
                            };
                            result.RecordResults.Add(recordResult);
                        }
                        result.FailedCount += batch.Count;
                        Log($"归档记录失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"归档操作失败: {ex.Message}");
                result.FailedCount = records.Count;
            }
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        private async Task ExecuteUpdateAsync(Type entityType, List<object> records, CleanupRule rule, RuleExecutionResult result, CleanupConfiguration config)
        {
            int batchSize = config.TransactionBatchSize;
            int totalRecords = records.Count;
            int processedCount = 0;

            // 验证更新字段配置
            if (string.IsNullOrWhiteSpace(rule.UpdateFieldName))
            {
                Log("更新操作未指定字段名");
                result.FailedCount = records.Count;
                return;
            }

            var updateProperty = entityType.GetProperty(rule.UpdateFieldName);
            if (updateProperty == null)
            {
                Log($"字段 {rule.UpdateFieldName} 不存在于实体 {entityType.Name}");
                result.FailedCount = records.Count;
                return;
            }

            // 转换更新值到正确类型
            object updateValue = ConvertValue(rule.UpdateFieldValue, updateProperty.PropertyType);

            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = records.Skip(i).Take(batchSize).ToList();

                try
                {
                    // 设置更新值
                    foreach (var record in batch)
                    {
                        updateProperty.SetValue(record, updateValue);
                    }

                    // 使用Updateable更新记录
                    var updateMethod = typeof(ISqlSugarClient).GetMethod("Updateable").MakeGenericMethod(entityType);
                    var updateable = updateMethod.Invoke(_db, new object[] { batch });
                    
                    // 只更新指定字段
                    var updateColumnsMethod = updateable.GetType().GetMethod("UpdateColumns", new[] { typeof(string[]) });
                    if (updateColumnsMethod != null)
                    {
                        updateable = updateColumnsMethod.Invoke(updateable, new object[] { new[] { rule.UpdateFieldName } });
                    }
                    
                    var executeMethod = updateable.GetType().GetMethod("ExecuteCommandAsync");
                    var updateTask = (Task<int>)executeMethod.Invoke(updateable, null);
                    int updatedCount = await updateTask;

                    for (int j = 0; j < batch.Count; j++)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(batch[j]),
                            RecordIdentifier = GetRecordIdentifier(batch[j]),
                            IsSuccess = true,
                            ActionType = CleanupActionType.UpdateField
                        };
                        result.RecordResults.Add(recordResult);
                    }

                    result.SuccessCount += batch.Count;
                    processedCount += batch.Count;

                    Log($"已更新 {processedCount}/{totalRecords} 条记录的 {rule.UpdateFieldName} 字段");
                }
                catch (Exception ex)
                {
                    foreach (var record in batch)
                    {
                        var recordResult = new RecordCleanupResult
                        {
                            RecordId = GetRecordId(record),
                            RecordIdentifier = GetRecordIdentifier(record),
                            IsSuccess = false,
                            ActionType = CleanupActionType.UpdateField,
                            ErrorMessage = ex.Message
                        };
                        result.RecordResults.Add(recordResult);
                    }
                    result.FailedCount += batch.Count;
                    Log($"更新记录失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 获取记录ID
        /// </summary>
        private string GetRecordId(object record)
        {
            if (record == null) return string.Empty;

            var idProperty = record.GetType().GetProperty("Id") ??
                           record.GetType().GetProperty("ID") ??
                           record.GetType().GetProperty($"{record.GetType().Name}_ID");

            if (idProperty != null)
            {
                var value = idProperty.GetValue(record);
                return value?.ToString() ?? string.Empty;
            }

            return record.GetHashCode().ToString();
        }

        /// <summary>
        /// 获取记录标识（用于显示）
        /// </summary>
        private string GetRecordIdentifier(object record)
        {
            if (record == null) return string.Empty;

            // 尝试获取常见的标识字段
            var identifierProperties = new[] { "Name", "Code", "No", "Number", "Title", "Description" };

            foreach (var propName in identifierProperties)
            {
                var prop = record.GetType().GetProperty(propName);
                if (prop != null)
                {
                    var value = prop.GetValue(record);
                    if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        return value.ToString();
                    }
                }
            }

            return GetRecordId(record);
        }

        /// <summary>
        /// 获取执行日志
        /// </summary>
        /// <returns>日志内容</returns>
        public string GetLog()
        {
            return _logBuilder.ToString();
        }

        /// <summary>
        /// 执行关联清理（基于外键关系自动顺序清理）
        /// </summary>
        /// <param name="config">清理配置</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>执行结果</returns>
        public async Task<CleanupExecutionResult> ExecuteRelatedCleanupAsync(CleanupConfiguration config, bool isTestMode = false)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var result = new CleanupExecutionResult
            {
                ConfigId = config.ConfigId,
                ConfigName = config.ConfigName,
                IsTestMode = isTestMode
            };

            Log($"开始执行关联清理：{config.ConfigName}");
            Log($"清理模式：{(config.CleanupMode == 1 ? "全部清空" : "指定记录清理")}");

            try
            {
                // 获取实体关系分析器
                var analyzer = new EntityRelationshipAnalyzer(_db);

                // 获取目标实体类型
                Type targetEntityType = GetEntityType(config.TargetEntityType);
                if (targetEntityType == null)
                {
                    throw new InvalidOperationException($"找不到实体类型：{config.TargetEntityType}");
                }

                // 生成清理顺序
                var cleanupOrder = analyzer.GenerateCleanupOrder(targetEntityType);
                Log($"生成清理顺序，共 {cleanupOrder.Count} 个表需要清理");

                // 如果是全部清空模式
                if (config.CleanupMode == 1)
                {
                    result = await ExecuteTruncateAllAsync(cleanupOrder, config, isTestMode);
                }
                else
                {
                    // 指定记录清理模式
                    result = await ExecuteDeleteRecordsAsync(cleanupOrder, config, targetEntityType, isTestMode);
                }

                result.IsSuccess = result.FailedCount == 0;
                result.Complete();

                Log($"关联清理执行完成：总耗时 {result.ElapsedMilliseconds}ms");
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.Complete();
                Log($"关联清理执行失败：{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 执行全部清空
        /// </summary>
        /// <param name="cleanupOrder">清理顺序</param>
        /// <param name="config">清理配置</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>执行结果</returns>
        private async Task<CleanupExecutionResult> ExecuteTruncateAllAsync(List<CleanupOrderNode> cleanupOrder, CleanupConfiguration config, bool isTestMode)
        {
            var result = new CleanupExecutionResult
            {
                ConfigId = config.ConfigId,
                ConfigName = config.ConfigName,
                IsTestMode = isTestMode
            };

            int totalTables = cleanupOrder.Count;
            int currentTable = 0;

            // 按顺序清理每个表
            foreach (var node in cleanupOrder)
            {
                currentTable++;
                ReportProgress(currentTable, totalTables, $"正在清空表：{node.TableName}");

                try
                {
                    Log($"开始清空表：{node.TableName} (顺序：{node.Order})");

                    // 获取表记录数
                    int recordCount = await GetTotalRecordCountAsync(node.EntityType);
                    result.TotalRecordCount += recordCount;

                    if (recordCount == 0)
                    {
                        Log($"表 {node.TableName} 无数据，跳过");
                        continue;
                    }

                    if (isTestMode)
                    {
                        Log($"[测试模式] 表 {node.TableName} 将删除 {recordCount} 条记录");
                        result.MatchedRecordCount += recordCount;
                        result.SuccessCount += recordCount;
                    }
                    else
                    {
                        // 执行清空操作
                        string tableName = node.TableName;
                        string truncateSql = $"TRUNCATE TABLE [{tableName}]";
                        await _db.Ado.ExecuteCommandAsync(truncateSql);

                        Log($"表 {node.TableName} 已清空，删除 {recordCount} 条记录");
                        result.DeletedCount += recordCount;
                        result.SuccessCount += recordCount;
                    }

                    node.IsCleaned = true;
                }
                catch (Exception ex)
                {
                    Log($"清空表 {node.TableName} 失败：{ex.Message}");
                    result.FailedCount++;
                    result.ErrorMessage = $"清空表 {node.TableName} 失败：{ex.Message}";

                    if (config.EnableTransaction)
                    {
                        Log($"启用事务回滚");
                        throw;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 执行指定记录删除
        /// </summary>
        /// <param name="cleanupOrder">清理顺序</param>
        /// <param name="config">清理配置</param>
        /// <param name="targetEntityType">目标实体类型</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>执行结果</returns>
        private async Task<CleanupExecutionResult> ExecuteDeleteRecordsAsync(List<CleanupOrderNode> cleanupOrder, CleanupConfiguration config, Type targetEntityType, bool isTestMode)
        {
            var result = new CleanupExecutionResult
            {
                ConfigId = config.ConfigId,
                ConfigName = config.ConfigName,
                IsTestMode = isTestMode
            };

            if (config.RecordIds == null || config.RecordIds.Count == 0)
            {
                throw new InvalidOperationException("指定记录清理模式必须指定要删除的记录 ID 列表");
            }

            Log($"指定清理的记录 ID 数：{config.RecordIds.Count}");

            int totalTables = cleanupOrder.Count;
            int currentTable = 0;

            // 按顺序清理每个表
            foreach (var node in cleanupOrder)
            {
                currentTable++;
                ReportProgress(currentTable, totalTables, $"正在清理表：{node.TableName}");

                try
                {
                    Log($"开始清理表：{node.TableName} (顺序：{node.Order})");

                    int deletedCount = 0;

                    if (node.EntityType == targetEntityType)
                    {
                        // 目标表，根据 ID 删除
                        deletedCount = await DeleteRecordsByIdsAsync(node.EntityType, config.RecordIds, isTestMode);
                    }
                    else
                    {
                        // 关联表，根据外键关系删除
                        deletedCount = await DeleteRelatedRecordsAsync(node.EntityType, targetEntityType, config.RecordIds, isTestMode);
                    }

                    result.DeletedCount += deletedCount;
                    result.SuccessCount += deletedCount;

                    Log($"表 {node.TableName} 清理完成，删除 {deletedCount} 条记录");
                    node.IsCleaned = true;
                }
                catch (Exception ex)
                {
                    Log($"清理表 {node.TableName} 失败：{ex.Message}");
                    result.FailedCount++;
                    result.ErrorMessage = $"清理表 {node.TableName} 失败：{ex.Message}";

                    if (config.EnableTransaction)
                    {
                        Log($"启用事务回滚");
                        throw;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据 ID 删除记录
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="ids">记录 ID 列表</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>删除的记录数</returns>
        private async Task<int> DeleteRecordsByIdsAsync(Type entityType, List<long> ids, bool isTestMode)
        {
            if (isTestMode)
            {
                // 测试模式，只查询不删除
                var queryMethod = typeof(ISqlSugarClient).GetMethod("Queryable").MakeGenericMethod(entityType);
                var queryable = queryMethod.Invoke(_db, null);

                // 构建 Where 条件：x => ids.Contains(x.Id)
                var whereMethod = queryable.GetType().GetMethods()
                    .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);

                if (whereMethod != null)
                {
                    var parameter = Expression.Parameter(entityType, "x");
                    var idProperty = Expression.Property(parameter, entityType.Name + "ID");
                    var idsConstant = Expression.Constant(ids);
                    var containsMethod = typeof(List<long>).GetMethod("Contains", new[] { typeof(long) });
                    var containsCall = Expression.Call(idsConstant, containsMethod, idProperty);
                    var lambda = Expression.Lambda(containsCall, parameter);

                    queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                }

                var countMethod = queryable.GetType().GetMethod("CountAsync");
                var countTask = (Task<int>)countMethod.Invoke(queryable, null);
                int count = await countTask;

                Log($"[测试模式] 表 {entityType.Name} 将删除 {count} 条记录");
                return count;
            }
            else
            {
                // 正式删除
                string tableName = entityType.Name;
                string pkName = entityType.Name + "ID";
                string idsStr = string.Join(",", ids);
                string deleteSql = $"DELETE FROM [{tableName}] WHERE [{pkName}] IN ({idsStr})";

                int deletedCount = await _db.Ado.ExecuteCommandAsync(deleteSql);
                Log($"表 {tableName} 实际删除 {deletedCount} 条记录");
                return deletedCount;
            }
        }

        /// <summary>
        /// 删除关联记录
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="targetEntityType">目标实体类型</param>
        /// <param name="targetIds">目标记录 ID 列表</param>
        /// <param name="isTestMode">是否为测试模式</param>
        /// <returns>删除的记录数</returns>
        private async Task<int> DeleteRelatedRecordsAsync(Type entityType, Type targetEntityType, List<long> targetIds, bool isTestMode)
        {
            // 获取外键关系
            var analyzer = new EntityRelationshipAnalyzer(_db);
            var entityInfo = analyzer.GetEntityRelationship(entityType);

            // 找到指向目标表的外键
            var fkRelation = entityInfo.ForeignKeyRelations.FirstOrDefault(fk =>
                fk.ReferencedEntityType == targetEntityType || fk.ReferencedTableName == targetEntityType.Name);

            if (fkRelation == null)
            {
                Log($"表 {entityType.Name} 没有指向表 {targetEntityType.Name} 的外键关系，跳过");
                return 0;
            }

            Log($"表 {entityType.Name} 通过字段 {fkRelation.ForeignKeyColumn} 关联到表 {targetEntityType.Name}");

            if (isTestMode)
            {
                // 测试模式，只查询不删除
                var queryMethod = typeof(ISqlSugarClient).GetMethod("Queryable").MakeGenericMethod(entityType);
                var queryable = queryMethod.Invoke(_db, null);

                // 构建 Where 条件：x => targetIds.Contains(x.FkField)
                var whereMethod = queryable.GetType().GetMethods()
                    .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);

                if (whereMethod != null)
                {
                    var parameter = Expression.Parameter(entityType, "x");
                    var fkProperty = Expression.Property(parameter, fkRelation.ForeignKeyColumn);
                    var idsConstant = Expression.Constant(targetIds);
                    var containsMethod = typeof(List<long>).GetMethod("Contains", new[] { typeof(long) });
                    var containsCall = Expression.Call(idsConstant, containsMethod, fkProperty);
                    var lambda = Expression.Lambda(containsCall, parameter);

                    queryable = whereMethod.Invoke(queryable, new object[] { lambda });
                }

                var countMethod = queryable.GetType().GetMethod("CountAsync");
                var countTask = (Task<int>)countMethod.Invoke(queryable, null);
                int count = await countTask;

                Log($"[测试模式] 表 {entityType.Name} 将删除 {count} 条关联记录");
                return count;
            }
            else
            {
                // 正式删除
                string tableName = entityType.Name;
                string idsStr = string.Join(",", targetIds);
                string deleteSql = $"DELETE FROM [{tableName}] WHERE [{fkRelation.ForeignKeyColumn}] IN ({idsStr})";

                int deletedCount = await _db.Ado.ExecuteCommandAsync(deleteSql);
                Log($"表 {tableName} 实际删除 {deletedCount} 条关联记录");
                return deletedCount;
            }
        }

        #region 强类型查询引擎

        /// <summary>
        /// 执行强类型查询(泛型方法)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rule">清理规则</param>
        /// <returns>查询结果列表</returns>
        public async Task<List<T>> ExecuteTypedQueryAsync<T>(CleanupRule rule) where T : BaseEntity, new()
        {
            var query = _db.Queryable<T>();
            var predicate = BuildPredicate<T>(rule);
            
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// 构建查询谓词表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="rule">清理规则</param>
        /// <returns>谓词表达式</returns>
        private Expression<Func<T, bool>> BuildPredicate<T>(CleanupRule rule) where T : BaseEntity, new()
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression combinedExpression = null;

                // 根据规则类型构建不同的条件
                switch (rule.RuleType)
                {
                    case CleanupRuleType.EmptyValueRemoval:
                        combinedExpression = BuildEmptyValuePredicate<T>(parameter, rule);
                        break;

                    case CleanupRuleType.ExpiredDataRemoval:
                        combinedExpression = BuildExpiredDataPredicate<T>(parameter, rule);
                        break;

                    case CleanupRuleType.CustomConditionRemoval:
                        combinedExpression = BuildCustomConditionPredicate<T>(parameter, rule);
                        break;

                    case CleanupRuleType.DataTruncation:
                        combinedExpression = BuildTruncationPredicate<T>(parameter, rule);
                        break;

                    default:
                        // 其他规则类型需要在ExecuteRule中特殊处理
                        return null;
                }

                if (combinedExpression != null)
                {
                    return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                }
            }
            catch (Exception ex)
            {
                Log($"构建谓词表达式失败: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 构建空值检查谓词
        /// </summary>
        private Expression BuildEmptyValuePredicate<T>(ParameterExpression parameter, CleanupRule rule) where T : BaseEntity
        {
            if (rule.EmptyCheckFields == null || rule.EmptyCheckFields.Count == 0)
                return null;

            Expression combinedExpression = null;
            var entityType = typeof(T);

            foreach (var fieldName in rule.EmptyCheckFields)
            {
                var property = entityType.GetProperty(fieldName);
                if (property == null)
                    continue;

                Expression fieldExpression = null;
                var propertyExpression = Expression.Property(parameter, fieldName);

                switch (rule.EmptyValueMode)
                {
                    case EmptyValueCheckMode.NullOnly:
                        fieldExpression = Expression.Equal(propertyExpression, Expression.Constant(null));
                        break;

                    case EmptyValueCheckMode.NullOrEmpty:
                        if (property.PropertyType == typeof(string))
                        {
                            var nullCheck = Expression.Equal(propertyExpression, Expression.Constant(null));
                            var emptyCheck = Expression.Equal(propertyExpression, Expression.Constant(string.Empty));
                            fieldExpression = Expression.OrElse(nullCheck, emptyCheck);
                        }
                        else
                        {
                            fieldExpression = Expression.Equal(propertyExpression, Expression.Constant(null));
                        }
                        break;

                    case EmptyValueCheckMode.NullOrWhiteSpace:
                        if (property.PropertyType == typeof(string))
                        {
                            // 简化处理：只检查NULL和空字符串，空白字符在内存中过滤
                            var nullCheck = Expression.Equal(propertyExpression, Expression.Constant(null));
                            var emptyCheck = Expression.Equal(propertyExpression, Expression.Constant(string.Empty));
                            fieldExpression = Expression.OrElse(nullCheck, emptyCheck);
                        }
                        else
                        {
                            fieldExpression = Expression.Equal(propertyExpression, Expression.Constant(null));
                        }
                        break;
                }

                if (fieldExpression != null)
                {
                    if (combinedExpression == null)
                        combinedExpression = fieldExpression;
                    else
                        combinedExpression = Expression.OrElse(combinedExpression, fieldExpression);
                }
            }

            return combinedExpression;
        }

        /// <summary>
        /// 构建过期数据谓词
        /// </summary>
        private Expression BuildExpiredDataPredicate<T>(ParameterExpression parameter, CleanupRule rule) where T : BaseEntity
        {
            if (string.IsNullOrWhiteSpace(rule.DateFieldName))
                return null;

            var entityType = typeof(T);
            var property = entityType.GetProperty(rule.DateFieldName);
            if (property == null)
                return null;

            DateTime expireDate = DateTime.Now.AddDays(-rule.ExpireDays);
            var dateProperty = Expression.Property(parameter, rule.DateFieldName);
            var expireConstant = Expression.Constant(expireDate);

            return Expression.LessThan(dateProperty, expireConstant);
        }

        /// <summary>
        /// 构建自定义条件谓词
        /// </summary>
        private Expression BuildCustomConditionPredicate<T>(ParameterExpression parameter, CleanupRule rule) where T : BaseEntity
        {
            if (rule.CustomConditions == null || rule.CustomConditions.Count == 0)
                return null;

            var entityType = typeof(T);
            Expression combinedExpression = null;

            for (int i = 0; i < rule.CustomConditions.Count; i++)
            {
                var condition = rule.CustomConditions[i];
                var conditionExpression = BuildConditionExpressionForPredicate(parameter, condition, entityType);

                if (conditionExpression == null)
                    continue;

                if (combinedExpression == null)
                {
                    combinedExpression = conditionExpression;
                }
                else
                {
                    if (condition.LogicalOperator == LogicalOperator.And)
                        combinedExpression = Expression.AndAlso(combinedExpression, conditionExpression);
                    else
                        combinedExpression = Expression.OrElse(combinedExpression, conditionExpression);
                }
            }

            return combinedExpression;
        }

        /// <summary>
        /// 构建单个条件表达式(用于谓词)
        /// </summary>
        private Expression BuildConditionExpressionForPredicate(ParameterExpression parameter, ConditionItem condition, Type entityType)
        {
            try
            {
                var property = entityType.GetProperty(condition.FieldName);
                if (property == null)
                    return null;

                var propertyExpression = Expression.Property(parameter, condition.FieldName);
                var propertyType = property.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                object convertedValue = ConvertValue(condition.Value, underlyingType);
                var valueExpression = Expression.Constant(convertedValue, propertyType);

                switch (condition.Operator)
                {
                    case ComparisonOperator.Equals:
                        return Expression.Equal(propertyExpression, valueExpression);
                    case ComparisonOperator.NotEquals:
                        return Expression.NotEqual(propertyExpression, valueExpression);
                    case ComparisonOperator.GreaterThan:
                        return Expression.GreaterThan(propertyExpression, valueExpression);
                    case ComparisonOperator.GreaterThanOrEqual:
                        return Expression.GreaterThanOrEqual(propertyExpression, valueExpression);
                    case ComparisonOperator.LessThan:
                        return Expression.LessThan(propertyExpression, valueExpression);
                    case ComparisonOperator.LessThanOrEqual:
                        return Expression.LessThanOrEqual(propertyExpression, valueExpression);
                    case ComparisonOperator.Contains:
                        return BuildContainsExpression(propertyExpression, condition.Value);
                    case ComparisonOperator.StartsWith:
                        return BuildStartsWithExpression(propertyExpression, condition.Value);
                    case ComparisonOperator.EndsWith:
                        return BuildEndsWithExpression(propertyExpression, condition.Value);
                    case ComparisonOperator.IsEmpty:
                        return BuildIsEmptyExpression(propertyExpression, propertyType);
                    case ComparisonOperator.IsNotEmpty:
                        return Expression.Not(BuildIsEmptyExpression(propertyExpression, propertyType));
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                Log($"构建条件表达式失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 构建截断谓词
        /// </summary>
        private Expression BuildTruncationPredicate<T>(ParameterExpression parameter, CleanupRule rule) where T : BaseEntity
        {
            if (string.IsNullOrWhiteSpace(rule.TruncationField) || rule.MaxLength <= 0)
                return null;

            var entityType = typeof(T);
            var property = entityType.GetProperty(rule.TruncationField);

            if (property == null || property.PropertyType != typeof(string))
                return null;

            var propertyExpression = Expression.Property(parameter, rule.TruncationField);
            var nullConstant = Expression.Constant(null);
            var notNullCheck = Expression.NotEqual(propertyExpression, nullConstant);

            var lengthProperty = Expression.Property(propertyExpression, "Length");
            var maxLengthConstant = Expression.Constant(rule.MaxLength);
            var lengthCheck = Expression.GreaterThan(lengthProperty, maxLengthConstant);

            return Expression.AndAlso(notNullCheck, lengthCheck);
        }

        #endregion

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
        /// 从过滤器表达式中提取ID列表
        /// </summary>
        private List<long> ExtractIdsFromFilter(LambdaExpression filter)
        {
            try
            {
                if (filter.Body is MethodCallExpression methodCall &&
                    methodCall.Method.Name == "Contains" &&
                    methodCall.Object is ConstantExpression constant)
                {
                    return constant.Value as List<long>;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 构建ID IN 过滤表达式
        /// </summary>
        private LambdaExpression BuildIdInFilterExpression(Type entityType, List<long> ids)
        {
            try
            {
                var parameter = Expression.Parameter(entityType, "x");
                var pkName = GetPrimaryKeyNameFromType(entityType);
                var idProperty = Expression.Property(parameter, pkName);
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
        /// 根据表达式删除实体
        /// </summary>
        private async Task<int> DeleteEntitiesByExpressionAsync(Type entityType, LambdaExpression filterExpression, bool isTestMode)
        {
            if (filterExpression == null)
                return 0;

            try
            {
                if (isTestMode)
                {
                    // 测试模式：只统计不删除
                    var countMethod = typeof(ISqlSugarClient).GetMethod("Queryable")?.MakeGenericMethod(entityType);
                    if (countMethod != null)
                    {
                        var queryable = countMethod.Invoke(_db, null);
                        var whereMethod = queryable.GetType().GetMethods()
                            .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);

                        if (whereMethod != null)
                        {
                            queryable = whereMethod.Invoke(queryable, new object[] { filterExpression });
                            var countAsyncMethod = queryable.GetType().GetMethod("CountAsync");
                            var countTask = (Task<int>)countAsyncMethod.Invoke(queryable, null);
                            return await countTask;
                        }
                    }
                }
                else
                {
                    // 正式删除：使用SqlSugar的Deleteable
                    var deleteMethod = typeof(ISqlSugarClient).GetMethod("Deleteable")?.MakeGenericMethod(entityType);
                    if (deleteMethod != null)
                    {
                        var deleteable = deleteMethod.Invoke(_db, null);

                        // 调用Where
                        var whereMethod = deleteable.GetType().GetMethods()
                            .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 1);

                        if (whereMethod != null)
                        {
                            deleteable = whereMethod.Invoke(deleteable, new object[] { filterExpression });
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
            var sugarTableAttr = entityType.GetCustomAttribute<SugarTable>();
            return sugarTableAttr?.TableName ?? entityType.Name;
        }

        /// <summary>
        /// 从实体类型获取主键名称
        /// </summary>
        private string GetPrimaryKeyNameFromType(Type entityType)
        {
            var properties = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<SugarColumn>()?.IsPrimaryKey == true)
                .ToList();

            if (properties.Any())
                return properties.First().Name;

            var defaultPK = entityType.GetProperty(entityType.Name + "ID");
            if (defaultPK != null)
                return defaultPK.Name;

            return "ID";
        }

        #endregion
    }

    /// <summary>
    /// 清理进度事件参数
    /// </summary>
    public class CleanupProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 当前进度
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 总进度
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 进度百分比
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// 进度消息
        /// </summary>
        public string Message { get; set; }
    }
}
