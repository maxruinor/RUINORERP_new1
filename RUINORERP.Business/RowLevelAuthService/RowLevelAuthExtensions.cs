// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：行级权限扩展方法，用于在查询中应用行级权限规则
// ********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SqlSugar;
using RUINORERP.Model;
using RUINORERP.Business.RowLevelAuthService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 行级权限扩展方法
    /// 提供在ISugarQueryable上应用行级权限规则的能力
    /// </summary>
    public static class RowLevelAuthExtensions
    {
        /// <summary>
        /// 应用行级权限规则到查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="query">可查询对象</param>
        /// <param name="policies">行级权限策略列表</param>
        /// <param name="db">数据库实例</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>应用了行级权限规则的查询</returns>
        public static ISugarQueryable<T> ApplyRowLevelAuth<T>(
            this ISugarQueryable<T> query,
            List<tb_RowAuthPolicy> policies,
            ISqlSugarClient db,
            ILogger logger = null) where T : class
        {
            if (policies == null || policies.Count == 0)
            {
                return query;
            }

            var entityType = typeof(T).FullName;
            var applicablePolicies = policies.Where(p => p.EntityType == entityType && p.IsEnabled).ToList();

            if (applicablePolicies.Count == 0)
            {
                return query;
            }

            try
            {
                foreach (var policy in applicablePolicies)
                {
                    query = ApplyPolicy(query, policy, db, logger);
                }

                logger?.LogInformation($"已为实体 {entityType} 应用 {applicablePolicies.Count} 条行级权限规则");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"应用行级权限规则到实体 {entityType} 失败");
            }

            return query;
        }

        /// <summary>
        /// 应用单个权限策略到查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="query">可查询对象</param>
        /// <param name="policy">权限策略</param>
        /// <param name="db">数据库实例</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>应用了权限策略的查询</returns>
        private static ISugarQueryable<T> ApplyPolicy<T>(
            ISugarQueryable<T> query,
            tb_RowAuthPolicy policy,
            ISqlSugarClient db,
            ILogger logger) where T : class
        {
            try
            {
                // 判断是否需要联表
                if (policy.IsJoinRequired.HasValue && policy.IsJoinRequired.Value && !string.IsNullOrEmpty(policy.JoinTable))
                {
                    // 使用子查询实现联表过滤
                    if (!string.IsNullOrEmpty(policy.FilterClause))
                    {
                        var subQuerySql = string.IsNullOrEmpty(policy.JoinOnClause)
                            ? $"SELECT 1 FROM {policy.JoinTable} jt WHERE jt.{policy.JoinTableJoinField} = {policy.TargetEntity}.{policy.JoinTableJoinField}"
                            : $"SELECT 1 FROM {policy.JoinTable} jt WHERE {policy.JoinOnClause}";

                        var fullWhereClause = $"EXISTS ({subQuerySql}";

                        // 添加过滤条件
                        if (!string.IsNullOrEmpty(policy.FilterClause))
                        {
                            fullWhereClause += $" AND {policy.FilterClause}";
                        }

                        fullWhereClause += ")";

                        // 使用SQL片段方式添加条件
                        query = query.Where(fullWhereClause);
                    }
                }
                else
                {
                    // 不需要联表，直接应用过滤条件
                    if (!string.IsNullOrEmpty(policy.FilterClause))
                    {
                        query = query.Where(policy.FilterClause);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"应用权限策略 {policy.PolicyName} 失败");
            }

            return query;
        }

        /// <summary>
        /// 检查指定实体类型是否配置了行级权限
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="policies">权限策略列表</param>
        /// <returns>是否有配置行级权限</returns>
        public static bool HasRowLevelAuthFor<T>(this List<tb_RowAuthPolicy> policies) where T : class
        {
            if (policies == null || policies.Count == 0)
            {
                return false;
            }

            var entityType = typeof(T).FullName;
            return policies.Any(p => p.EntityType == entityType && p.IsEnabled);
        }

        /// <summary>
        /// 获取指定实体类型的所有权限策略
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="policies">权限策略列表</param>
        /// <returns>该实体类型的权限策略列表</returns>
        public static List<tb_RowAuthPolicy> GetPoliciesFor<T>(this List<tb_RowAuthPolicy> policies) where T : class
        {
            if (policies == null || policies.Count == 0)
            {
                return new List<tb_RowAuthPolicy>();
            }

            var entityType = typeof(T).FullName;
            return policies.Where(p => p.EntityType == entityType && p.IsEnabled).ToList();
        }
    }
}
