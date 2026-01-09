// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：行级权限策略查询服务实现，支持根据用户和角色获取对应的权限策略
// ********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.IServices;
using RUINORERP.Business.Cache;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限策略查询服务实现
    /// 负责根据用户和角色获取对应的行级权限策略
    /// </summary>
    public class RowAuthPolicyQueryService : IRowAuthPolicyQueryService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<RowAuthPolicyQueryService> _logger;
        private readonly Itb_P4RowAuthPolicyByRoleServices _policyByRoleService;
        private readonly Itb_P4RowAuthPolicyByUserServices _policyByUserService;
        private readonly Itb_RowAuthPolicyServices _policyService;
        private readonly IEntityCacheManager _cacheManager;

        /// <summary>
        /// 内置字典缓存，用于存储行级权限策略
        /// Key格式: "RowAuthPolicy_User_{userId}_Menu_{menuId}" 或 "RowAuthPolicy_Roles_{roleIds}_Menu_{menuId}"
        /// </summary>
        private readonly Dictionary<string, List<tb_RowAuthPolicy>> _policyCache;
        private readonly object _cacheLock = new object();

        /// <summary>
        /// 策略缓存Key前缀
        /// </summary>
        private const string CACHE_KEY_PREFIX = "RowAuthPolicy_";

        public RowAuthPolicyQueryService(
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<RowAuthPolicyQueryService> logger,
            Itb_P4RowAuthPolicyByRoleServices policyByRoleService,
            Itb_P4RowAuthPolicyByUserServices policyByUserService,
            Itb_RowAuthPolicyServices policyService,
            IEntityCacheManager cacheManager)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            _policyByRoleService = policyByRoleService;
            _policyByUserService = policyByUserService;
            _policyService = policyService;
            _cacheManager = cacheManager;
            _policyCache = new Dictionary<string, List<tb_RowAuthPolicy>>();
        }

        /// <summary>
        /// 根据用户ID获取该用户的所有行级权限策略
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="menuId">菜单ID（可选）</param>
        /// <returns>行级权限策略列表</returns>
        public async Task<List<tb_RowAuthPolicy>> GetPoliciesByUserAsync(long userId, long? menuId = null)
        {
            string cacheKey = $"{CACHE_KEY_PREFIX}User_{userId}_Menu_{menuId}";

            // 尝试从内置字典缓存获取
            lock (_cacheLock)
            {
                if (_policyCache.TryGetValue(cacheKey, out var cachedPolicies))
                {
                    return cachedPolicies;
                }
            }

            try
            {
                // 查询用户与策略的关联
                var userPolicyRelations = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_P4RowAuthPolicyByUser>()
                    .Where(u => u.User_ID == userId && u.IsEnabled)
                    .WhereIF(menuId.HasValue, u => u.MenuID == menuId)
                    .Includes(u => u.tb_rowauthpolicy)
                    .ToListAsync();

                // 提取策略
                var policies = userPolicyRelations
                    .Where(u => u.tb_rowauthpolicy != null && u.tb_rowauthpolicy.IsEnabled)
                    .Select(u => u.tb_rowauthpolicy)
                    .ToList();

                // 缓存结果到内置字典
                lock (_cacheLock)
                {
                    _policyCache[cacheKey] = policies;
                }

                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户{userId}的行级权限策略失败");
                return new List<tb_RowAuthPolicy>();
            }
        }

        /// <summary>
        /// 根据角色ID列表获取所有对应的行级权限策略
        /// </summary>
        /// <param name="roleIds">角色ID列表</param>
        /// <param name="menuId">菜单ID（可选）</param>
        /// <returns>行级权限策略列表</returns>
        public async Task<List<tb_RowAuthPolicy>> GetPoliciesByRolesAsync(List<long> roleIds, long? menuId = null)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return new List<tb_RowAuthPolicy>();
            }

            string cacheKey = $"{CACHE_KEY_PREFIX}Roles_{string.Join(",", roleIds)}_Menu_{menuId}";

            // 尝试从内置字典缓存获取
            lock (_cacheLock)
            {
                if (_policyCache.TryGetValue(cacheKey, out var cachedPolicies))
                {
                    return cachedPolicies;
                }
            }

            try
            {
                // 查询角色与策略的关联
                var rolePolicyRelations = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_P4RowAuthPolicyByRole>()
                    .Where(r => roleIds.Contains(r.RoleID) && r.IsEnabled)
                    .WhereIF(menuId.HasValue, r => r.MenuID == menuId)
                    .Includes(r => r.tb_rowauthpolicy)
                    .ToListAsync();

                // 提取策略并去重（按PolicyId）
                var policies = rolePolicyRelations
                    .Where(r => r.tb_rowauthpolicy != null && r.tb_rowauthpolicy.IsEnabled)
                    .Select(r => r.tb_rowauthpolicy)
                    .GroupBy(p => p.PolicyId)
                    .Select(g => g.First())
                    .ToList();

                // 缓存结果到内置字典
                lock (_cacheLock)
                {
                    _policyCache[cacheKey] = policies;
                }

                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取角色{string.Join(",", roleIds)}的行级权限策略失败");
                return new List<tb_RowAuthPolicy>();
            }
        }

        /// <summary>
        /// 根据用户ID和该用户的角色ID列表获取所有行级权限策略（用户策略和角色策略的并集）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID列表</param>
        /// <param name="menuId">菜单ID（可选）</param>
        /// <returns>行级权限策略列表（去重）</returns>
        public async Task<List<tb_RowAuthPolicy>> GetPoliciesByUserAndRolesAsync(long userId, List<long> roleIds, long? menuId = null)
        {
            // 并行获取用户策略和角色策略
            var userPoliciesTask = GetPoliciesByUserAsync(userId, menuId);
            var rolePoliciesTask = GetPoliciesByRolesAsync(roleIds, menuId);

            await Task.WhenAll(userPoliciesTask, rolePoliciesTask);

            var userPolicies = await userPoliciesTask;
            var rolePolicies = await rolePoliciesTask;

            // 合并策略并去重（用户策略优先级高于角色策略）
            var allPolicies = userPolicies
                .Union(rolePolicies)
                .GroupBy(p => p.PolicyId)
                .Select(g => g.First())
                .ToList();

            return allPolicies;
        }

        /// <summary>
        /// 根据实体类型获取所有启用的行级权限策略
        /// </summary>
        /// <param name="entityType">实体类型全限定名</param>
        /// <returns>行级权限策略列表</returns>
        public async Task<List<tb_RowAuthPolicy>> GetPoliciesByEntityTypeAsync(string entityType)
        {
            string cacheKey = $"{CACHE_KEY_PREFIX}EntityType_{entityType}";

            // 尝试从内置字典缓存获取
            lock (_cacheLock)
            {
                if (_policyCache.TryGetValue(cacheKey, out var cachedPolicies))
                {
                    return cachedPolicies;
                }
            }

            try
            {
                var policies = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_RowAuthPolicy>()
                    .Where(p => p.EntityType == entityType && p.IsEnabled)
                    .ToListAsync();

                // 缓存结果到内置字典
                lock (_cacheLock)
                {
                    _policyCache[cacheKey] = policies;
                }

                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取实体类型{entityType}的行级权限策略失败");
                return new List<tb_RowAuthPolicy>();
            }
        }

        /// <summary>
        /// 检查指定菜单是否配置了行级权限策略
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否有配置策略</returns>
        public async Task<bool> HasPolicyForMenuAsync(long menuId)
        {
            try
            {
                // 检查角色策略
                var hasRolePolicy = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_P4RowAuthPolicyByRole>()
                    .Where(r => r.MenuID == menuId && r.IsEnabled)
                    .AnyAsync();

                if (hasRolePolicy)
                {
                    return true;
                }

                // 检查用户策略
                var hasUserPolicy = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_P4RowAuthPolicyByUser>()
                    .Where(u => u.MenuID == menuId && u.IsEnabled)
                    .AnyAsync();

                return hasUserPolicy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查菜单{menuId}的行级权限策略配置失败");
                return false;
            }
        }

        /// <summary>
        /// 清除策略缓存（在策略变更时调用）
        /// </summary>
        public void ClearPolicyCache()
        {
            try
            {
                lock (_cacheLock)
                {
                    _policyCache.Clear();
                }
                _logger.LogInformation("已清除行级权限策略缓存");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清除行级权限策略缓存失败");
            }
        }
    }
}
