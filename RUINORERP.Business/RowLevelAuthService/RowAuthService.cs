using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly ApplicationContext _appContext;
        private readonly ILogger<RowAuthService> _logger;
        private readonly ISqlSugarClient _db;
        private readonly IMemoryCache _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultRuleProvider">默认规则提供者</param>
        /// <param name="context">应用程序上下文</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="db">数据库客户端</param>
        /// <param name="cacheManager">内存缓存管理器</param>
        public RowAuthService(
            IDefaultRowAuthRuleProvider defaultRuleProvider,
            ApplicationContext context,
            ILogger<RowAuthService> logger,
            ISqlSugarClient db,
            IMemoryCache cacheManager)
        {
            _appContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _defaultRuleProvider = defaultRuleProvider ?? throw new ArgumentNullException(nameof(defaultRuleProvider));
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
                var entityInfoService = _appContext.GetRequiredService<IEntityMappingService>();

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
                var entityInfoService = _appContext.GetRequiredService<IEntityMappingService>();

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

                _logger.Debug("行级权限配置保存成功: {RoleId}, {BizType}", config.RoleId, config.BizType);
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
                var entityInfoService = _appContext.GetRequiredService<IEntityMappingService>();

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

                _logger.Debug("权限策略分配角色成功: {PolicyId}, {RoleId}", policyId, roleId);
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

                _logger.Debug("从角色移除权限策略成功: {PolicyId}, {RoleId}", policyId, roleId);
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
        public string GetUserRowAuthFilterClause(Type entityType, long menuId)
        {
            // 检查是否为超级管理员，如果是则跳过行级权限过滤
            if (_appContext.IsSuperUser)
            {
                _logger.LogDebug("当前用户为超级管理员，跳过行级权限过滤");
                return string.Empty;
            }

            if (!_appContext.FunctionConfig.EnableRowLevelAuth)
            {
                return string.Empty;
            }

            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType), "实体类型不能为空");
            }

            // 增强缓存配置，添加绝对过期时间作为后备
            var cacheEntryOptions = CreateCacheEntryOptions();

            try
            {
                string entityName = entityType.Name;
                string cacheKey = GenerateCacheKey(entityName, _appContext.CurrentRole.RoleID, menuId);

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

                // 构建权限上下文
                var context = BuildRowAuthContext(menuId, entityName);

                // 获取当前用户的所有角色ID
                var userRoleIds = _appContext.Roles.Select(r => r.RoleID).ToList();

                if (!userRoleIds.Any())
                {
                    // 用户没有角色，返回null表示无限制
                    _logger.LogDebug("用户 {UserId} 没有任何角色，不应用行级权限限制", context.UserId);
                    _cacheManager.Set(cacheKey, "__NO_RESTRICTION__", cacheEntryOptions);
                    return null;
                }

                // 直接从数据库获取策略
                var policies = GetPoliciesByUserAndMenu(context.UserId, userRoleIds, menuId);

                if (policies != null && policies.Any())
                {
                    _logger.LogDebug("找到 {PolicyCount} 条适用的行级权限规则", policies.Count);

                    // 构建过滤条件
                    var filterClause = BuildFilterClauseFromPolicies(policies, entityName, context);

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
                throw;
            }
        }

        /// <summary>
        /// 获取用户对特定实体类型的数据权限过滤条件(Expression形式)
        /// 适用于SqlSugar的Lambda表达式查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="menuId">菜单ID</param>
        /// <returns>Lambda表达式</returns>
        public System.Linq.Expressions.Expression<Func<T, bool>> GetUserRowAuthFilterExpression<T>(long menuId) where T : class
        {
            try
            {
                var filterClause = GetUserRowAuthFilterClause(typeof(T), menuId);

                if (string.IsNullOrEmpty(filterClause))
                {
                    return t => true;
                }

                return ExpressionFilterHelper.ConvertToExpression<T>(filterClause);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取行级权限过滤Expression失败");
                return t => true;
            }
        }

        /// <summary>
        /// 从权限策略列表构建过滤条件(支持参数化)
        /// </summary>
        /// <param name="policies">权限策略列表</param>
        /// <param name="entityName">实体名称</param>
        /// <param name="context">权限上下文</param>
        /// <returns>构建的过滤条件</returns>
        private string BuildFilterClauseFromPolicies(List<tb_RowAuthPolicy> policies, string entityName, RowAuthContext context)
        {
            try
            {
                var policyClauses = new List<string>();

                foreach (var policy in policies)
                {
                    string clause = BuildPolicyFilterClause(policy, entityName, context);
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
        /// 为单个策略构建过滤条件(支持参数化)
        /// </summary>
        /// <param name="policy">权限策略</param>
        /// <param name="entityName">实体名称</param>
        /// <param name="context">权限上下文</param>
        /// <returns>构建的过滤条件</returns>
        private string BuildPolicyFilterClause(tb_RowAuthPolicy policy, string entityName, RowAuthContext context)
        {
            if (policy == null)
            {
                _logger.LogWarning("策略为空,跳过");
                return string.Empty;
            }

            // 检查是否为参数化规则
            bool isParameterized = !string.IsNullOrEmpty(policy.ParameterizedFilterClause);

            string filterClause = isParameterized
                ? policy.ParameterizedFilterClause
                : policy.FilterClause;

            if (string.IsNullOrEmpty(filterClause))
            {
                _logger.LogWarning("策略过滤条件为空，跳过: {PolicyName}", policy.PolicyName);
                return string.Empty;
            }

            // 如果是恒真条件，直接返回1=1
            if (filterClause.Trim() == "1=1")
            {
                return "1=1";
            }

            // 如果是参数化规则,解析参数
            if (isParameterized)
            {
                filterClause = ParameterizedFilterHelper.ResolveFilterTemplate(filterClause, context);
                _logger.LogDebug("解析参数化过滤条件: {Original} -> {Resolved}",
                    policy.ParameterizedFilterClause, filterClause);
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
                return BuildExistsSubqueryClause(policy, filterClause);
            }
            else
            {
                // 直接使用过滤条件
                return SanitizeFilterClause(filterClause);
            }
        }

        /// <summary>
        /// 构建EXISTS子查询过滤条件
        /// </summary>
        /// <param name="policy">权限策略</param>
        /// <param name="resolvedFilterClause">解析后的过滤条件</param>
        /// <returns>EXISTS子查询SQL</returns>
        private string BuildExistsSubqueryClause(tb_RowAuthPolicy policy, string resolvedFilterClause)
        {
            // 构建标准的EXISTS子查询格式，优化以适应SqlSugar的解析
            // 使用表别名避免可能的命名冲突
            return $"EXISTS (SELECT 1 FROM {policy.JoinTable} jt WHERE {policy.JoinOnClause} AND ({SanitizeFilterClause(resolvedFilterClause)}))";
        }

        /// <summary>
        /// 构建权限上下文
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="entityName">实体名称</param>
        /// <returns>权限上下文</returns>
        private RowAuthContext BuildRowAuthContext(long menuId, string entityName)
        {
            return new RowAuthContext
            {
                UserId = GetCurrentUserId(),
                RoleId = _appContext.CurrentRole?.RoleID ?? 0,
                RoleIds = _appContext.Roles.Select(r => r.RoleID).ToList(),
                EmployeeId = _appContext.CurUserInfo?.UserInfo?.Employee_ID,
                DepartmentId = null, // 可从AppContext获取
                MenuId = menuId,
                EntityName = entityName
            };
        }

        /// <summary>
        /// 创建缓存选项
        /// </summary>
        /// <returns>缓存选项</returns>
        private MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                .SetPriority(CacheItemPriority.Normal);
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

                _logger.Debug("已清除角色 {RoleId} 对实体 {EntityName} 的权限缓存", roleId, entityName);
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
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID，用于区分不同功能的数据规则</param>
        /// <returns>缓存键</returns>
        private string GenerateCacheKey(string entityName, long roleId, long menuId)
        {
            return $"RowAuth:UserId:{GetCurrentUserId()}:Entity:{entityName}:Role:{roleId}:Menu:{menuId}";
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>当前用户ID</returns>
        private long GetCurrentUserId()
        {
            try
            {
                return _appContext.CurUserInfo?.UserID ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取当前用户ID失败，使用默认值0");
                return 0;
            }
        }

        /// <summary>
        /// 获取用户在指定菜单上的所有权限策略(包含角色策略和用户策略)
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID列表</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>权限策略列表</returns>
        private List<tb_RowAuthPolicy> GetPoliciesByUserAndMenu(long userId, List<long> roleIds, long menuId)
        {
            try
            {
                var policies = new List<tb_RowAuthPolicy>();

                // 获取角色级别的策略
                if (roleIds != null && roleIds.Any())
                {
                    var rolePolicies = _db.Queryable<tb_RowAuthPolicy>()
                        .InnerJoin<tb_P4RowAuthPolicyByRole>((p, r) => p.PolicyId == r.PolicyId)
                        .Where((p, r) => p.IsEnabled && roleIds.Contains(r.RoleID) && r.MenuID == menuId)
                        .Select((p, r) => p)
                        .ToList();
                    policies.AddRange(rolePolicies);
                }

                // TODO: 获取用户级别的策略(如果实现了用户级权限)
                // var userPolicies = _db.Queryable<tb_RowAuthPolicy>()
                //     .InnerJoin<tb_P4RowAuthPolicyByUser>((p, u) => p.PolicyId == u.PolicyId)
                //     .Where((p, u) => p.IsEnabled && u.User_ID == userId && u.MenuID == menuId)
                //     .Select((p, u) => p)
                //     .ToList();
                // policies.AddRange(userPolicies);

                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询用户 {UserId} 在菜单 {MenuId} 的权限策略失败", userId, menuId);
                return new List<tb_RowAuthPolicy>();
            }
        }
    }
}
