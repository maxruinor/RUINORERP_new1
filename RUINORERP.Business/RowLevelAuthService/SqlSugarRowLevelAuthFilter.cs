using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SqlSugar;
using RUINORERP.Model;
using RUINORERP.Common.Log4Net;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 基于SqlSugar的行级权限过滤器
    /// </summary>
    public class SqlSugarRowLevelAuthFilter
    {
        private readonly ILogger<SqlSugarRowLevelAuthFilter> _logger;
        private readonly SqlSugarScope _db;

        public SqlSugarRowLevelAuthFilter(ILogger<SqlSugarRowLevelAuthFilter> logger, SqlSugarScope db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// 应用行级权限过滤条件到查询
        /// </summary>
        /// <typeparam name="T">主实体类型</typeparam>
        /// <param name="query">原始查询</param>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>应用过滤后的查询</returns>
        public ISugarQueryable<T> ApplyFilter<T>(ISugarQueryable<T> query, string filterClause) where T : class
        {
            if (string.IsNullOrWhiteSpace(filterClause))
            {
                _logger?.LogDebug("行级权限过滤条件为空，跳过处理");
                return query;
            }

            try
            {
                // 预处理过滤条件
                string processedClause = PreprocessFilterClause(filterClause);
                _logger?.LogDebug("预处理后的过滤条件: {ProcessedClause}", processedClause);

                // 解析过滤条件类型
                if (IsExistsSubQuery(processedClause))
                {
                    return ApplyExistsFilter(query, processedClause);
                }
                else if (IsInSubQuery(processedClause))
                {
                    return ApplyInFilter(query, processedClause);
                }
                else
                {
                    // 尝试作为简单条件处理
                    return ApplySimpleFilter(query, processedClause);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用行级权限过滤时发生错误: {FilterClause}", filterClause);
                return query; // 出错时返回原始查询
            }
        }

        /// <summary>
        /// 应用EXISTS子查询过滤
        /// </summary>
        private ISugarQueryable<T> ApplyExistsFilter<T>(ISugarQueryable<T> query, string existsClause) where T : class
        {
            try
            {
                // 提取EXISTS子查询内容
                string subQuery = ExtractSubQueryContent(existsClause, "EXISTS");

                // 解析子查询中的关联信息
                var joinInfo = ParseExistsJoinInfo<T>(subQuery);

                if (joinInfo == null)
                {
                    _logger?.LogWarning("无法解析EXISTS子查询的关联信息: {SubQuery}", subQuery);
                    return query;
                }

                // 使用解析的信息构建正确的EXISTS查询
                string mainTableName = GetTableName(joinInfo.MainEntityType);
                string relatedTableName = GetTableName(joinInfo.RelatedEntityType);
                
                // 构建正确的EXISTS子查询SQL
                string subQuerySql = $"SELECT 1 FROM {relatedTableName} jt " +
                                   $"WHERE jt.{joinInfo.MainTableField} = {mainTableName}.{joinInfo.MainTableField} " +
                                   $"AND jt.{joinInfo.FilterField} {joinInfo.FilterOperator} '{joinInfo.FilterValue}'";

                // 使用SqlSugar的Where方法应用EXISTS查询
                return query.Where($"EXISTS ({subQuerySql})");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用EXISTS过滤时发生错误: {ExistsClause}", existsClause);
                return query;
            }
        }

        /// <summary>
        /// 解析EXISTS子查询的关联信息
        /// </summary>
        private ExistsJoinInfo ParseExistsJoinInfo<T>(string subQuery) where T : class
        {
            try
            {
                // 使用正则表达式解析子查询
                // 修复正则表达式，正确处理AND条件后的括号和值
                var pattern = @"FROM\s+(\w+)\s+(\w+)\s+WHERE\s+([\w\.]+)\s*=\s*([\w\.]+)\s+AND\s+\(*([\w\.]+)\s*([=<>]+)\s*([^\s\)]+)";
                var match = Regex.Match(subQuery, pattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    _logger?.LogWarning("无法解析子查询条件: {SubQuery}", subQuery);
                    return null;
                }

                // 提取解析结果
                string relatedTable = match.Groups[1].Value;
                string relatedAlias = match.Groups[2].Value;
                string mainTableField = match.Groups[4].Value.Contains('.') ?
                    match.Groups[4].Value.Split('.')[1] : match.Groups[4].Value;
                string relatedTableField = match.Groups[5].Value.Contains('.') ?
                    match.Groups[5].Value.Split('.')[1] : match.Groups[5].Value;
                string filterOperator = match.Groups[6].Value;
                string filterValue = match.Groups[7].Value.Trim('\'', '"'); // 去除引号

                // 获取主实体类型
                Type mainEntityType = typeof(T);

                // 获取关联实体类型
                Type relatedEntityType = FindEntityTypeByTableName(relatedTable);
                if (relatedEntityType == null)
                {
                    _logger?.LogWarning("找不到表 {TableName} 对应的实体类型", relatedTable);
                    return null;
                }

                return new ExistsJoinInfo
                {
                    MainEntityType = mainEntityType,
                    RelatedEntityType = relatedEntityType,
                    MainTableField = mainTableField,
                    RelatedTableField = relatedTableField,
                    FilterField = relatedTableField,
                    FilterOperator = filterOperator,
                    FilterValue = filterValue
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析EXISTS关联信息时发生错误: {SubQuery}", subQuery);
                return null;
            }
        }

        /// <summary>
        /// 构建SqlSugar的EXISTS查询
        /// </summary>
        private string BuildSqlSugarExistsQuery(ExistsJoinInfo joinInfo)
        {
            try
            {
                // 构建子查询SQL
                // 修复字段引用，使用正确的表别名和字段
                string subQuerySql = $"SELECT 1 FROM {GetTableName(joinInfo.RelatedEntityType)} jt " +
                                    $"WHERE {joinInfo.MainTableField} = jt.{joinInfo.RelatedTableField} " +
                                    $"AND jt.{joinInfo.FilterField} {joinInfo.FilterOperator} '{joinInfo.FilterValue}'";

                _logger?.LogDebug("构建的EXISTS子查询: {SubQuerySql}", subQuerySql);
                return subQuerySql;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建EXISTS查询时发生错误");
                return string.Empty;
            }
        }

        /// <summary>
        /// 应用IN子查询过滤
        /// </summary>
        private ISugarQueryable<T> ApplyInFilter<T>(ISugarQueryable<T> query, string inClause) where T : class
        {
            try
            {
                // 提取IN子查询内容
                string subQuery = ExtractSubQueryContent(inClause, "IN");

                // 解析IN子查询信息
                var inInfo = ParseInJoinInfo<T>(subQuery);
                if (inInfo == null)
                {
                    _logger?.LogWarning("无法解析IN子查询信息: {SubQuery}", subQuery);
                    return query;
                }

                // 构建完整的IN查询条件
                string mainTableName = GetTableName(inInfo.MainEntityType);
                string relatedTableName = GetTableName(inInfo.RelatedEntityType);
                
                // 构建子查询SQL
                string subQuerySql = $"SELECT {inInfo.SelectField} FROM {relatedTableName} jt " +
                                   $"WHERE jt.{inInfo.FilterField} {inInfo.FilterOperator} '{inInfo.FilterValue}'";
                
                // 构建IN查询条件
                string condition = $"{mainTableName}.{inInfo.MainTableField} IN ({subQuerySql})";
                return query.Where(condition);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用IN过滤时发生错误: {InClause}", inClause);
                return query;
            }
        }

        /// <summary>
        /// 解析IN子查询信息
        /// </summary>
        private InJoinInfo ParseInJoinInfo<T>(string subQuery) where T : class
        {
            try
            {
                // 使用正则表达式解析IN子查询
                // 修复正则表达式，正确处理值部分
                var pattern = @"SELECT\s+([\w\.]+)\s+FROM\s+(\w+)\s+WHERE\s+([\w\.]+)\s*([=<>]+)\s*([^\s\)]+)";
                var match = Regex.Match(subQuery, pattern, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    _logger?.LogWarning("无法解析IN子查询条件: {SubQuery}", subQuery);
                    return null;
                }

                // 提取解析结果
                string selectField = match.Groups[1].Value.Contains('.') ?
                    match.Groups[1].Value.Split('.')[1] : match.Groups[1].Value;
                string relatedTable = match.Groups[2].Value;
                string filterField = match.Groups[3].Value.Contains('.') ?
                    match.Groups[3].Value.Split('.')[1] : match.Groups[3].Value;
                string filterOperator = match.Groups[4].Value;
                string filterValue = match.Groups[5].Value.Trim('\'', '"'); // 去除引号

                // 获取主实体类型
                Type mainEntityType = typeof(T);

                // 获取关联实体类型
                Type relatedEntityType = FindEntityTypeByTableName(relatedTable);
                if (relatedEntityType == null)
                {
                    _logger?.LogWarning("找不到表 {TableName} 对应的实体类型", relatedTable);
                    return null;
                }

                return new InJoinInfo
                {
                    MainEntityType = mainEntityType,
                    RelatedEntityType = relatedEntityType,
                    SelectField = selectField,
                    FilterField = filterField,
                    FilterOperator = filterOperator,
                    FilterValue = filterValue
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析IN关联信息时发生错误: {SubQuery}", subQuery);
                return null;
            }
        }

        /// <summary>
        /// 构建SqlSugar的IN查询
        /// </summary>
        private string BuildSqlSugarInQuery(InJoinInfo inInfo)
        {
            try
            {
                // 构建子查询SQL
                string subQuerySql = $"SELECT {inInfo.SelectField} FROM {GetTableName(inInfo.RelatedEntityType)} jt " +
                                    $"WHERE jt.{inInfo.FilterField} {inInfo.FilterOperator} '{inInfo.FilterValue}'";

                _logger?.LogDebug("构建的IN子查询: {SubQuerySql}", subQuerySql);
                return subQuerySql;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建IN查询时发生错误");
                return string.Empty;
            }
        }

        /// <summary>
        /// 应用简单过滤条件
        /// </summary>
        private ISugarQueryable<T> ApplySimpleFilter<T>(ISugarQueryable<T> query, string simpleClause) where T : class
        {
            try
            {
                _logger?.LogDebug("应用简单过滤条件: {SimpleClause}", simpleClause);
                return query.Where(simpleClause);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用简单过滤条件时发生错误: {SimpleClause}", simpleClause);
                return query;
            }
        }

        /// <summary>
        /// 预处理过滤条件
        /// </summary>
        private string PreprocessFilterClause(string clause)
        {
            // 移除可能的前导和尾随空格
            return clause?.Trim();
        }

        /// <summary>
        /// 判断是否为EXISTS子查询
        /// </summary>
        private bool IsExistsSubQuery(string clause)
        {
            return clause?.IndexOf("EXISTS", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 判断是否为IN子查询
        /// </summary>
        private bool IsInSubQuery(string clause)
        {
            return clause?.IndexOf("IN", StringComparison.OrdinalIgnoreCase) >= 0 &&
                   clause.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 提取子查询内容
        /// </summary>
        private string ExtractSubQueryContent(string clause, string keyword)
        {
            try
            {
                // 查找关键字位置
                int keywordIndex = clause.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (keywordIndex < 0)
                    return clause;

                // 查找子查询开始位置
                int startIndex = clause.IndexOf("(", keywordIndex);
                if (startIndex < 0)
                    return clause;

                // 查找子查询结束位置
                int endIndex = FindMatchingParenthesis(clause, startIndex);
                if (endIndex < 0)
                    return clause;

                // 提取子查询内容
                return clause.Substring(startIndex + 1, endIndex - startIndex - 1);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "提取子查询内容时发生错误: {Clause}", clause);
                return clause;
            }
        }

        /// <summary>
        /// 查找匹配的括号位置
        /// </summary>
        private int FindMatchingParenthesis(string text, int startIndex)
        {
            int balance = 1;
            for (int i = startIndex + 1; i < text.Length; i++)
            {
                if (text[i] == '(')
                    balance++;
                else if (text[i] == ')')
                    balance--;

                if (balance == 0)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 根据实体类型获取表名
        /// </summary>
        private string GetTableName(Type entityType)
        {
            try
            {
                // 使用SqlSugar的SugarTable特性获取表名
                var sugarTableAttr = entityType.GetCustomAttribute<SugarTable>();
                if (sugarTableAttr != null)
                {
                    return sugarTableAttr.TableName;
                }

                // 如果没有特性，使用类名作为表名
                return entityType.Name;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取实体 {EntityType} 的表名时发生错误", entityType?.Name);
                return entityType?.Name ?? string.Empty;
            }
        }

        /// <summary>
        /// 根据表名查找实体类型
        /// </summary>
        private Type FindEntityTypeByTableName(string tableName)
        {
            try
            {
                // 获取当前程序集中的所有类型
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        // 查找具有SugarTable特性的类型
                        var types = assembly.GetTypes()
                            .Where(t => t.GetCustomAttribute<SugarTable>()?.TableName.Equals(tableName, StringComparison.OrdinalIgnoreCase) == true);

                        var type = types.FirstOrDefault();
                        if (type != null)
                            return type;
                    }
                    catch
                    {
                        // 忽略无法加载的程序集
                        continue;
                    }
                }

                _logger?.LogWarning("找不到表 {TableName} 对应的实体类型", tableName);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "根据表名 {TableName} 查找实体类型时发生错误", tableName);
                return null;
            }
        }
    }

    /// <summary>
    /// EXISTS连接信息
    /// </summary>
/// <summary>
/// 存在连接信息类，用于描述实体间存在关系的连接信息
/// </summary>
public class ExistsJoinInfo
{
    /// <summary>
    /// 获取或设置主实体类型
    /// </summary>
    public Type MainEntityType { get; set; }
    
    /// <summary>
    /// 获取或设置关联实体类型
    /// </summary>
    public Type RelatedEntityType { get; set; }
    
    /// <summary>
    /// 获取或设置主表字段名
    /// </summary>
    public string MainTableField { get; set; }
    
    /// <summary>
    /// 获取或设置关联表字段名
    /// </summary>
    public string RelatedTableField { get; set; }
    
    /// <summary>
    /// 获取或设置过滤字段名
    /// </summary>
    public string FilterField { get; set; }
    
    /// <summary>
    /// 获取或设置过滤操作符
    /// </summary>
    public string FilterOperator { get; set; }
    
    /// <summary>
    /// 获取或设置过滤值
    /// </summary>
    public string FilterValue { get; set; }
}


    /// <summary>
    /// IN连接信息
    /// </summary>
    public class InJoinInfo
    {
        public Type MainEntityType { get; set; }
        public Type RelatedEntityType { get; set; }
        public string MainTableField { get; set; }
        public string SelectField { get; set; }
        public string FilterField { get; set; }
        public string FilterOperator { get; set; }
        public string FilterValue { get; set; }
    }
}