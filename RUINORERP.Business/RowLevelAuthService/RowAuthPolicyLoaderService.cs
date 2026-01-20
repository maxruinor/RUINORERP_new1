// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：系统启动时加载行级权限规则的服务
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
    /// 行级权限策略加载服务
    /// 负责系统启动时加载和缓存行级权限规则
    /// </summary>
    public interface IRowAuthPolicyLoaderService
    {
        /// <summary>
        /// 加载所有启用的行级权限策略到缓存
        /// </summary>
        Task LoadAllPoliciesAsync();

        /// <summary>
        /// 预加载指定用户的行级权限策略
        /// </summary>
        /// <param name="userId">用户ID</param>
        Task LoadPoliciesForUserAsync(long userId, List<long> userRoles);

        /// <summary>
        /// 预加载指定角色的行级权限策略
        /// </summary>
        /// <param name="roleIds">角色ID列表</param>
        Task LoadPoliciesForRolesAsync(List<long> roleIds);

        /// <summary>
        /// 刷新策略缓存（当策略配置变更时调用）
        /// </summary>
        Task RefreshPolicyCacheAsync();

        /// <summary>
        /// 检查策略加载状态
        /// </summary>
        bool IsLoaded { get; }
    }

    /// <summary>
    /// 行级权限策略加载服务实现
    /// </summary>
    public class RowAuthPolicyLoaderService : IRowAuthPolicyLoaderService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<RowAuthPolicyLoaderService> _logger;
        private readonly IRowAuthPolicyQueryService _policyQueryService;
        private readonly Itb_RowAuthPolicyServices _policyService;
        private readonly EventDrivenCacheManager _cacheManager;
        private bool _isLoaded = false;

        public bool IsLoaded => _isLoaded;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RowAuthPolicyLoaderService(
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<RowAuthPolicyLoaderService> logger,
            IRowAuthPolicyQueryService policyQueryService,
            Itb_RowAuthPolicyServices policyService,
            EventDrivenCacheManager cacheManager)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            _policyQueryService = policyQueryService;
            _policyService = policyService;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 加载所有启用的行级权限策略到缓存
        /// 注意：此方法作为后台缓存加载操作，失败不应影响系统主流程
        /// </summary>
        public async Task LoadAllPoliciesAsync()
        {
            try
            {
                // 获取所有启用的策略
                var allPolicies = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_RowAuthPolicy>()
                    .Where(p => p.IsEnabled)
                    .ToListAsync() ?? new List<tb_RowAuthPolicy>();

                // 获取所有策略关联的用户
                var userPolicyRelations = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_P4RowAuthPolicyByUser>()
                    .Where(u => u.IsEnabled)
                    .ToListAsync() ?? new List<tb_P4RowAuthPolicyByUser>();

                // 获取所有策略关联的角色
                var rolePolicyRelations = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_P4RowAuthPolicyByRole>()
                    .Where(r => r.IsEnabled)
                    .ToListAsync() ?? new List<tb_P4RowAuthPolicyByRole>();

                // 获取所有唯一用户ID
                var userIds = userPolicyRelations?
                    .Select(u => u.User_ID)
                    .Distinct()
                    .ToList() ?? new List<long>();

                // 获取所有唯一角色ID
                var roleIds = rolePolicyRelations?
                    .Select(r => r.RoleID)
                    .Distinct()
                    .ToList() ?? new List<long>();

                // 预加载所有用户的策略缓存
                if (userIds.Any() && roleIds.Any())
                {
                    var loadTasks = userIds.Select(userId => LoadPoliciesForUserAsync(userId, roleIds));
                    await Task.WhenAll(loadTasks);
                }

                _isLoaded = true;
                _logger.LogInformation($"成功加载行级权限策略: {userPolicyRelations?.Count ?? 0} 个用户关联, {rolePolicyRelations?.Count ?? 0} 个角色关联");
            }
            catch (Exception ex)
            {
                // 权限缓存加载失败不应影响系统主流程，只记录日志
                _logger.LogWarning(ex, "加载行级权限策略失败，但不影响系统运行。权限过滤将降级为默认行为");
                _isLoaded = false;
            }
        }

        /// <summary>
        /// 预加载指定用户的行级权限策略
        /// </summary>
        /// <param name="userId">用户ID</param>
        public async Task LoadPoliciesForUserAsync(long userId, List<long> userRoles)
        {
            try
            {
                // 使用策略查询服务获取策略（会自动缓存）
                await _policyQueryService.GetPoliciesByUserAndRolesAsync(userId, userRoles);

                _logger.LogDebug($"已为用户 {userId} 加载行级权限策略");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"为用户 {userId} 加载行级权限策略失败");
            }
        }

        /// <summary>
        /// 预加载指定角色的行级权限策略
        /// </summary>
        /// <param name="roleIds">角色ID列表</param>
        public async Task LoadPoliciesForRolesAsync(List<long> roleIds)
        {
            try
            {
                // 使用策略查询服务获取策略（会自动缓存）
                await _policyQueryService.GetPoliciesByRolesAsync(roleIds);

                _logger.LogDebug($"已为角色 {string.Join(",", roleIds)} 加载行级权限策略");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"为角色 {string.Join(",", roleIds)} 加载行级权限策略失败");
            }
        }

        /// <summary>
        /// 刷新策略缓存（当策略配置变更时调用）
        /// </summary>
        public async Task RefreshPolicyCacheAsync()
        {
            try
            {

                // 清除缓存
                _policyQueryService.ClearPolicyCache();

                // 重新加载所有策略
                await LoadAllPoliciesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新行级权限策略缓存失败");
                throw;
            }
        }
    }
}
