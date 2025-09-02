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
using System.Threading.Tasks;

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
            {
                // 获取实体信息服务
                var entityInfoService = _appContext.GetRequiredService<IEntityInfoService>();

                // 获取实体信息
                var entityInfo = entityInfoService.GetEntityInfo(config.BizType);
                if (entityInfo == null)
                {
                    _logger.LogWarning("未找到业务类型 {BizType} 对应的实体信息", config.BizType);
                    return false;
                }

                // 开始事务
                _db.Ado.BeginTran();

                try
                {
                    // 获取该角色在该实体上已有的所有策略ID
                    var existingPolicyIds = _db.Queryable<tb_P4RowAuthPolicyByRole>()
                        .Where(r => r.RoleID == config.RoleId)
                        .InnerJoin<tb_RowAuthPolicy>((r, p) => r.PolicyId == p.PolicyId)
                        .Where((r, p) => p.TargetEntity == entityInfo.EntityName)
                        .Select((r, p) => r.PolicyId)
                        .ToList();

                    // 处理已分配的策略（新增或保留的）
                    var assignedPolicyIds = new List<long>();
                    if (config.AssignedPolicies != null)
                    {
                        foreach (var policy in config.AssignedPolicies)
                        {
                            assignedPolicyIds.Add(policy.PolicyId);

                            // 如果策略尚未分配给该角色，则进行分配
                            if (!existingPolicyIds.Contains(policy.PolicyId))
                            {
                                _db.Insertable(new tb_P4RowAuthPolicyByRole
                                {
                                    PolicyId = policy.PolicyId,
                                    RoleID = config.RoleId
                                }).ExecuteReturnSnowflakeId();
                            }
                        }
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
                        var policyId = _db.Insertable(config.NewCustomPolicy).ExecuteReturnSnowflakeId();

                        // 将新规则分配给当前角色
                        _db.Insertable(new tb_P4RowAuthPolicyByRole
                        {
                            PolicyId = policyId,
                            RoleID = config.RoleId
                        }).ExecuteReturnSnowflakeId();
                    }

                    // 移除不再分配的策略
                    var policiesToRemove = existingPolicyIds.Except(assignedPolicyIds).ToList();
                    if (policiesToRemove.Any())
                    {
                        _db.Deleteable<tb_P4RowAuthPolicyByRole>()
                            .Where(r => r.RoleID == config.RoleId && policiesToRemove.Contains(r.PolicyId))
                            .ExecuteCommand();
                    }

                    // 提交事务
                    _db.Ado.CommitTran();
                }
                catch (Exception innerEx)
                {
                    // 回滚事务
                    _db.Ado.RollbackTran();
                    _logger.LogError(innerEx, "保存行级权限配置事务失败: {RoleId}, {BizType}", config.RoleId, config.BizType);
                    return false;
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
        /// 异步保存行级权限配置
        /// </summary>
        /// <param name="config">行级权限配置DTO</param>
        /// <returns>保存是否成功</returns>
        public async Task<bool> SaveRowAuthConfigAsync(RowAuthConfigDto config)
        {
            // 使用Task.FromResult包装同步方法的结果，提供异步接口
            // 在实际应用中，这里应该使用真正的异步数据库操作
            return await Task.Run(() => SaveRowAuthConfig(config));
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
        /// 根据指定的业务类型获取能参与配置的规则
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>权限策略列表</returns>
        public List<tb_RowAuthPolicy> GetAllPolicies(BizType bizType)
        {
            try
            {
                // 获取实体信息服务
                var entityInfoService = _appContext.GetRequiredService<IEntityInfoService>();

                // 获取实体信息
                var entityInfo = entityInfoService.GetEntityInfo(bizType);
                if (entityInfo == null)
                {
                    _logger.LogWarning("未找到业务类型 {BizType} 对应的实体信息", bizType);
                    return new List<tb_RowAuthPolicy>();
                }

                // 查询与该实体相关的所有规则
                return _db.Queryable<tb_RowAuthPolicy>()
                    .Where(p => p.TargetEntity == entityInfo.EntityName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据业务类型获取权限策略失败: {BizType}", bizType);
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
                    }).ExecuteReturnSnowflakeId();
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
        /// 基于SqlSugar框架优化的实现，提供更高效的规则组合和SQL生成
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="menuId">菜单ID，用于区分不同功能的数据规则</param>
        /// <returns>SQL过滤条件子句</returns>
        public string GetUserRowAuthFilterClause(System.Type entityType, long menuId)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType), "实体类型不能为空");
            }

            // 增强缓存配置，添加绝对过期时间作为后备
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                  .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                  .SetPriority(CacheItemPriority.Normal);

            try
            {
                string entityName = entityType.Name;
                string cacheKey = GenerateCacheKey(entityName, menuId);

                // 尝试从缓存获取
                if (_cacheManager.TryGetValue(cacheKey, out object cachedValue))
                {
                    if (cachedValue is string cachedString)
                    {
                        // 处理特殊标记
                        if (cachedString == "__NO_RESTRICTION__" || cachedString == "__NO_POLICY__")
                        {
                            _logger.LogDebug("从缓存获取到行级权限状态: {Status}", cachedString);
                            return null;
                        }
                        _logger.LogDebug("从缓存获取到行级权限过滤条件");
                        return cachedString;
                    }
                    return null;
                }

                // 获取当前用户的所有角色ID
                var currentUserId = GetCurrentUserId();
                var userRoleIds = _appContext.Roles.Select(r => r.RoleID).ToList();

                if (!userRoleIds.Any())
                {
                    // 用户没有角色，返回null表示无限制
                    _logger.LogInformation("用户 {UserId} 没有任何角色，不应用行级权限限制", currentUserId);
                    _cacheManager.Set(cacheKey, "__NO_RESTRICTION__", cacheEntryOptions);
                    return null;
                }

                // 获取用户角色对应的权限规则（角色级别）
                var rolePolicies = _db.Queryable<tb_RowAuthPolicy>()
                    .InnerJoin<tb_P4RowAuthPolicyByRole>((p, r) => p.PolicyId == r.PolicyId)
                    .Where((p, r) => p.IsEnabled && p.TargetEntity == entityName)
                    .Where((p, r) => userRoleIds.Contains(r.RoleID))
                    // 增加菜单ID过滤条件，只获取与当前菜单相关的规则
                    .Where((p, r) => r.MenuID == menuId)
                    .Select((p, r) => new tb_RowAuthPolicy
                    {
                        PolicyId = p.PolicyId,
                        PolicyName = p.PolicyName,
                        TargetEntity = p.TargetEntity,
                        TargetTable = p.TargetTable,
                        IsJoinRequired = p.IsJoinRequired,
                        JoinTable = p.JoinTable,
                        JoinType = p.JoinType,
                        JoinOnClause = p.JoinOnClause,
                        FilterClause = p.FilterClause
                    })
                    .ToList();

                // 获取用户直接绑定的权限规则（用户级别）
                var userPolicies = _db.Queryable<tb_RowAuthPolicy>()
                    .InnerJoin<tb_P4RowAuthPolicyByUser>((p, u) => p.PolicyId == u.PolicyId)
                    .Where((p, u) => p.IsEnabled && p.TargetEntity == entityName)
                    .Where((p, u) => u.User_ID == currentUserId)
                    // 增加菜单ID过滤条件，只获取与当前菜单相关的规则
                    .Where((p, u) => u.MenuID == menuId)
                    .Select((p, u) => new tb_RowAuthPolicy
                    {
                        PolicyId = p.PolicyId,
                        PolicyName = p.PolicyName,
                        TargetEntity = p.TargetEntity,
                        TargetTable = p.TargetTable,
                        IsJoinRequired = p.IsJoinRequired,
                        JoinTable = p.JoinTable,
                        JoinType = p.JoinType,
                        JoinOnClause = p.JoinOnClause,
                        FilterClause = p.FilterClause
                    })
                    .ToList();

                // 合并角色级别和用户级别的规则（去重）
                var policies = rolePolicies.Union(userPolicies).ToList();

                if (policies != null && policies.Any())
                {
                    _logger.LogDebug("找到 {PolicyCount} 条适用的行级权限规则", policies.Count);
                    // 处理多条规则：用 OR 连接
                    var filterClause = BuildFilterClauseFromPolicies(policies, entityName);

                    // 缓存生成的过滤条件
                    _cacheManager.Set(cacheKey, filterClause, cacheEntryOptions);
                    _logger.LogDebug("已缓存行级权限过滤条件");
                    return filterClause;
                }

                // 无规则
                _logger.LogDebug("未找到适用的行级权限规则");
                _cacheManager.Set(cacheKey, "__NO_POLICY__", cacheEntryOptions);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取行级权限过滤条件失败: {EntityType}", entityType.FullName);
                // 在生产环境中可以考虑返回null，确保系统不会因权限问题而崩溃
                // 但在开发/测试环境中应该抛出异常以便及时发现问题
                throw;
            }
        }

        /// <summary>
        /// 从权限策略列表构建过滤条件
        /// </summary>
        /// <param name="policies">权限策略列表</param>
        /// <param name="entityName">实体名称</param>
        /// <returns>构建的过滤条件</returns>
        private string BuildFilterClauseFromPolicies(List<tb_RowAuthPolicy> policies, string entityName)
        {
            try
            {
                var policyClauses = new List<string>();

                foreach (var policy in policies)
                {
                    string clause = BuildPolicyFilterClause(policy, entityName);
                    if (!string.IsNullOrEmpty(clause))
                    {
                        policyClauses.Add(clause);
                    }
                }

                if (!policyClauses.Any())
                {
                    return "1=1";
                }

                // 多条规则用OR连接
                return string.Join(" OR ", policyClauses.Select(c => $"({c})"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "构建行级权限过滤条件失败");
                return "1=1";
            }
        }

        /// <summary>
        /// 为单个策略构建过滤条件
        /// </summary>
        /// <param name="policy">权限策略</param>
        /// <param name="entityName">实体名称</param>
        /// <returns>构建的过滤条件</returns>
        private string BuildPolicyFilterClause(tb_RowAuthPolicy policy, string entityName)
        {
            if (policy == null || string.IsNullOrEmpty(policy.FilterClause))
            {
                _logger.LogWarning("策略或过滤条件为空，跳过");
                return string.Empty;
            }

            // 如果是恒真条件，直接返回1=1
            if (policy.FilterClause.Trim() == "1=1")
            {
                return "1=1";
            }

            // 处理需要关联表的情况
            if (policy.IsJoinRequired.GetValueOrDefault())
            {
                if (string.IsNullOrEmpty(policy.JoinTable) || string.IsNullOrEmpty(policy.JoinOnClause))
                {
                    _logger.LogWarning("策略需要关联表，但关联信息不完整，跳过: {PolicyName}", policy.PolicyName);
                    return string.Empty;
                }

                // 构建EXISTS子查询
                // 使用SqlSugar支持的SQL语法格式
                return BuildExistsSubqueryClause(policy);
            }
            else
            {
                // 直接使用过滤条件
                return SanitizeFilterClause(policy.FilterClause);
            }
        }

        /// <summary>
        /// 构建EXISTS子查询过滤条件
        /// </summary>
        /// <param name="policy">权限策略</param>
        /// <returns>EXISTS子查询SQL</returns>
        private string BuildExistsSubqueryClause(tb_RowAuthPolicy policy)
        {
            // 构建标准的EXISTS子查询格式，优化以适应SqlSugar的解析
            // 使用表别名避免可能的命名冲突
            return $"EXISTS (SELECT 1 FROM {policy.JoinTable} jt WHERE {policy.JoinOnClause} AND ({SanitizeFilterClause(policy.FilterClause)}))";
        }

        /// <summary>
        /// 清理过滤条件，确保SQL语法正确和安全
        /// </summary>
        /// <param name="filterClause">原始过滤条件</param>
        /// <returns>清理后的过滤条件</returns>
        private string SanitizeFilterClause(string filterClause)
        {
            if (string.IsNullOrEmpty(filterClause))
                return string.Empty;

            // 去除首尾空格
            string sanitized = filterClause.Trim();

            // 移除多余的括号（如果有）
            if (sanitized.StartsWith("(") && sanitized.EndsWith(")"))
            {
                sanitized = sanitized.Substring(1, sanitized.Length - 2).Trim();
            }

            // 基础的SQL注入防护检查
            if (ContainsPotentialSqlInjection(sanitized))
            {
                _logger.LogWarning("检测到可能的SQL注入风险，过滤条件已被拒绝: {FilterClause}", sanitized);
                return "1=1";
            }

            return sanitized;
        }

        /// <summary>
        /// 检查过滤条件是否包含潜在的SQL注入风险
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>是否包含风险</returns>
        private bool ContainsPotentialSqlInjection(string filterClause)
        {
            // 简单的SQL注入检查，实际项目中应使用更复杂的检测机制
            string lowerClause = filterClause.ToLower();
            string[] sqlKeywords = new string[] { "drop", "truncate", "alter", "exec", "execute", "union", "insert", "update", "delete" };

            foreach (string keyword in sqlKeywords)
            {
                if (lowerClause.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
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
                // 获取所有拥有此角色的用户ID
                var userIds = _db.Queryable<tb_User_Role>()
                    .Where(ur => ur.RoleID == roleId)
                    .Select(ur => ur.User_ID)
                    .ToList();


                // 清除这些用户对此实体的缓存
                foreach (var userId in userIds)
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
        /// <param name="menuId">菜单ID，用于区分不同功能的数据规则</param>
        /// <returns>缓存键</returns>
        private string GenerateCacheKey(string entityName, long menuId)
        {
            return $"RowAuth:UserId:{GetCurrentUserId()}:Entity:{entityName}:Menu:{menuId}";
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
