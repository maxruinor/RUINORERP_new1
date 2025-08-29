using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限服务实现
    /// 提供行级数据权限控制的核心功能
    /// </summary>
    public class RowAuthService : IRowAuthService
    {
        private readonly IDefaultRowAuthRuleProvider _defaultRuleProvider;
        private readonly IEntityInfoService _entityBizMappingService;
        private readonly ApplicationContext _appContext;
        private readonly ILogger<RowAuthService> _logger;
        private readonly ISqlSugarClient _db;
        private readonly IMemoryCache _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultRuleProvider">默认规则提供者</param>
        /// <param name="entityBizMappingService">实体业务映射服务</param>
        /// <param name="context">应用程序上下文</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="db">数据库客户端</param>
        /// <param name="cacheManager">内存缓存管理器</param>
        public RowAuthService(
            IDefaultRowAuthRuleProvider defaultRuleProvider,
            IEntityInfoService entityBizMappingService,
            ApplicationContext context,
            ILogger<RowAuthService> logger,
            ISqlSugarClient db,
            IMemoryCache cacheManager)
        {
            _appContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _defaultRuleProvider = defaultRuleProvider ?? throw new ArgumentNullException(nameof(defaultRuleProvider));
            _entityBizMappingService = entityBizMappingService ?? throw new ArgumentNullException(nameof(entityBizMappingService));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        /// <summary>
        /// 获取指定角色和业务类型的行级权限配置
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="bizType">业务类型</param>
        /// <returns>行级权限配置DTO</returns>
        public RowAuthConfigDto GetRowAuthConfig(long roleId, BizType bizType)
        {
            try
            {
                var dto = new RowAuthConfigDto
                {
                    RoleId = roleId,
                    BizType = bizType,
                    AvailableDefaultOptions = _defaultRuleProvider.GetDefaultRuleOptions(bizType),
                    AssignedPolicies = new List<tb_RowAuthPolicy>(),
                    NewCustomPolicy = new tb_RowAuthPolicy()
                };

                // 获取实体信息服务
                var entityInfoService = _appContext.GetRequiredService<IEntityInfoService>();

                // 获取实体信息
                var entityInfo = entityInfoService.GetEntityInfo(bizType);
                if (entityInfo == null)
                {
                    _logger.LogWarning("未找到业务类型 {BizType} 对应的实体信息", bizType);
                    return dto;
                }

                // 获取已分配给此角色的所有规则
                dto.AssignedPolicies = _db.Queryable<tb_RowAuthPolicy>()
                    .InnerJoin<tb_P4RowAuthPolicyByRole>((p, r) => p.PolicyId == r.PolicyId)
                    .Where((p, r) => p.TargetEntity == entityInfo.EntityName && r.RoleID == roleId)
                    .Select((p, r) => p)
                    .ToList();

                // 获取可用的默认策略（尚未分配给该角色的）
                var allDefaultPolicies = _db.Queryable<tb_RowAuthPolicy>()
                    .Where(p => p.TargetEntity == entityInfo.EntityName && p.DefaultRuleEnum.HasValue)
                    .ToList();

                dto.AvailableDefaultPolicies = allDefaultPolicies
                    .Where(p => !dto.AssignedPolicies.Any(ap => ap.PolicyId == p.PolicyId))
                    .ToList();



                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取行级权限配置失败: {RoleId}, {BizType}", roleId, bizType);
                throw;
            }
        }

        /// <summary>
        /// 保存行级权限配置
        /// </summary>
        /// <param name="config">行级权限配置DTO</param>
        /// <returns>保存是否成功</returns>
        public bool SaveRowAuthConfig(RowAuthConfigDto config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "配置对象不能为空");
            }

            try
            {  // 获取实体信息服务
                var entityInfoService = _appContext.GetRequiredService<IEntityInfoService>();

                // 获取实体信息
                var entityInfo = entityInfoService.GetEntityInfo(config.BizType);
                if (entityInfo == null)
                {
                    _logger.LogWarning("未找到业务类型 {BizType} 对应的实体信息", config.BizType);
                    return false;
                }

                // 处理新创建的自定义规则
                if (config.NewCustomPolicy != null &&
                    !string.IsNullOrEmpty(config.NewCustomPolicy.FilterClause))
                {
                    config.NewCustomPolicy.TargetEntity = entityInfo.EntityName;
                    config.NewCustomPolicy.EntityType = entityInfo.EntityType.FullName;
                    config.NewCustomPolicy.Created_at = DateTime.Now;
                    config.NewCustomPolicy.Created_by = GetCurrentUserId();

                    // 插入新规则
                    var policyId = _db.Insertable(config.NewCustomPolicy).ExecuteReturnBigIdentity();

                    // 将新规则分配给当前角色
                    _db.Insertable(new tb_P4RowAuthPolicyByRole
                    {
                        PolicyId = policyId,
                        RoleID = config.RoleId
                    }).ExecuteCommand();
                }

                // 清除相关缓存
                ClearAuthCache(config.RoleId, entityInfo.EntityName);

                _logger.LogInformation("行级权限配置保存成功: {RoleId}, {BizType}", config.RoleId, config.BizType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存行级权限配置失败: {RoleId}, {BizType}", config.RoleId, config.BizType);
                return false;
            }
        }

        /// <summary>
        /// 获取所有权限策略
        /// </summary>
        /// <returns>权限策略列表</returns>
        public List<tb_RowAuthPolicy> GetAllPolicies()
        {
            try
            {
                return _db.Queryable<tb_RowAuthPolicy>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有权限策略失败");
                throw;
            }
        }

        /// <summary>
        /// 为规则分配角色
        /// </summary>
        /// <param name="policyId">策略ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>分配是否成功</returns>
        public bool AssignPolicyToRole(long policyId, long roleId)
        {
            try
            {
                // 检查是否已存在关联
                var exists = _db.Queryable<tb_P4RowAuthPolicyByRole>()
                    .Where(r => r.PolicyId == policyId && r.RoleID == roleId)
                    .Any();

                if (!exists)
                {
                    _db.Insertable(new tb_P4RowAuthPolicyByRole
                    {
                        PolicyId = policyId,
                        RoleID = roleId
                    }).ExecuteCommand();
                }

                // 清除相关缓存
                ClearAuthCacheForPolicy(policyId);

                _logger.LogInformation("权限策略分配角色成功: {PolicyId}, {RoleId}", policyId, roleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "权限策略分配角色失败: {PolicyId}, {RoleId}", policyId, roleId);
                return false;
            }
        }

        /// <summary>
        /// 从角色移除规则
        /// </summary>
        /// <param name="policyId">策略ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>移除是否成功</returns>
        public bool RemovePolicyFromRole(long policyId, long roleId)
        {
            try
            {
                _db.Deleteable<tb_P4RowAuthPolicyByRole>()
                    .Where(r => r.PolicyId == policyId && r.RoleID == roleId)
                    .ExecuteCommand();

                // 清除相关缓存
                ClearAuthCacheForPolicy(policyId);

                _logger.LogInformation("从角色移除权限策略成功: {PolicyId}, {RoleId}", policyId, roleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从角色移除权限策略失败: {PolicyId}, {RoleID}", policyId, roleId);
                return false;
            }
        }

        /// <summary>
        /// 获取用户对特定实体类型的数据权限过滤条件
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>SQL过滤条件子句</returns>
        public string GetUserRowAuthFilterClause(System.Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType), "实体类型不能为空");
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                  .SetPriority(CacheItemPriority.Normal);

            try
            {
                string entityName = entityType.Name;
                string cacheKey = GenerateCacheKey(entityName);

                // 尝试从缓存获取
                if (_cacheManager.TryGetValue(cacheKey, out object cachedValue))
                {
                    if (cachedValue is string cachedString)
                    {
                        // 处理特殊标记
                        if (cachedString == "__NO_RESTRICTION__" || cachedString == "__NO_POLICY__")
                        {
                            return null;
                        }
                        return cachedString;
                    }
                    return null;
                }

                // 获取用户所有角色 
                //为了跑通而已  (r => r.RoleID).  应该是用户的ID
                var userRoleIds = _appContext.Roles.Select(r => r.RoleID).ToList();
                if (!userRoleIds.Any())
                {
                    // 用户没有角色，返回null表示无限制
                    // 不缓存null值，使用特殊标记表示"无限制"
              
                    _cacheManager.Set(cacheKey, "__NO_RESTRICTION__", cacheEntryOptions);
                    return null;
                }

                // 获取用户所有角色对应的所有规则
                var policies = _db.Queryable<tb_RowAuthPolicy>()
                    .InnerJoin<tb_P4RowAuthPolicyByRole>((p, r) => p.PolicyId == r.PolicyId)
                    .Where((p, r) => p.IsEnabled && p.TargetEntity == entityName)
                    .Where((p, r) => userRoleIds.Contains(r.RoleID))
                    .Select((p, r) => p)
                    .ToList();

                if (policies != null && policies.Any())
                {
                    // 处理多条规则：用 OR 连接
                    var policyClauses = new List<string>();
                    foreach (var policy in policies)
                    {
                        string clause;
                        if (policy.IsJoinRequired.GetValueOrDefault())
                        {
                            clause = $"EXISTS (SELECT 1 FROM {policy.JoinTable} jt WHERE {policy.JoinOnClause} AND ({policy.FilterClause}))";
                        }
                        else
                        {
                            clause = policy.FilterClause;
                        }
                        policyClauses.Add($"({clause})");
                    }
                    var filterClause = string.Join(" OR ", policyClauses);
                    _cacheManager.Set(cacheKey, filterClause, TimeSpan.FromMinutes(30));
                    return filterClause;
                }

                // 无规则
                // 不缓存null值，使用特殊标记表示"无规则"
                _cacheManager.Set(cacheKey, "__NO_POLICY__", cacheEntryOptions);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取行级权限过滤条件失败: {EntityType}", entityType.FullName);
                throw;
            }
        }

        /// <summary>
        /// 清除与特定角色和实体相关的缓存
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="entityName">实体名称</param>
        private void ClearAuthCache(long roleId, string entityName)
        {
            try
            {
                // 获取所有拥有此角色的用户
                var usersWithRole = _db.Queryable<tb_P4RowAuthPolicyByRole>()
                    .Where(ur => ur.RoleID == roleId)
                    .ToList();

                // 清除这些用户对此实体的缓存
                foreach (var userId in usersWithRole)
                {
                    var cacheKey = $"RowAuth:UserId:{userId}:Entity:{entityName}";
                    _cacheManager.Remove(cacheKey);
                }

                _logger.LogInformation("已清除角色 {RoleId} 对实体 {EntityName} 的权限缓存", roleId, entityName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清除权限缓存失败: {RoleId}, {EntityName}", roleId, entityName);
            }
        }

        /// <summary>
        /// 清除与特定策略相关的缓存
        /// </summary>
        /// <param name="policyId">策略ID</param>
        private void ClearAuthCacheForPolicy(long policyId)
        {
            try
            {
                // 获取策略对应的实体
                var policy = _db.Queryable<tb_RowAuthPolicy>()
                    .Where(p => p.PolicyId == policyId)
                    .First();

                if (policy != null)
                {
                    // 获取所有使用此策略的角色
                    var rolesWithPolicy = _db.Queryable<tb_P4RowAuthPolicyByRole>()
                        .Where(r => r.PolicyId == policyId)
                        .Select(r => r.RoleID)
                        .ToList();

                    // 清除这些角色对相应实体的缓存
                    foreach (var roleId in rolesWithPolicy)
                    {
                        ClearAuthCache(roleId, policy.TargetEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清除策略相关权限缓存失败: {PolicyId}", policyId);
            }
        }

        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <returns>缓存键</returns>
        private string GenerateCacheKey(string entityName)
        {
            return $"RowAuth:UserId:{GetCurrentUserId()}:Entity:{entityName}";
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>当前用户ID</returns>
        private long GetCurrentUserId()
        {
            try
            {
                return _appContext.CurrentUser?.UserID ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取当前用户ID失败，使用默认值0");
                return 0;
            }
        }
    }
}
