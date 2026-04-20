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
