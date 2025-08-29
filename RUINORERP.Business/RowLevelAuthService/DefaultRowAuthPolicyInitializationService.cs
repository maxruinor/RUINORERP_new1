using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.RowLevelAuthService
{
    public class DefaultRowAuthPolicyInitializationService : IDefaultRowAuthPolicyInitializationService
    {
        private readonly ISqlSugarClient _db;
        private readonly IDefaultRowAuthRuleProvider _defaultRuleProvider;
        private readonly IEntityInfoService _entityInfoService;
        private readonly ILogger<DefaultRowAuthPolicyInitializationService> _logger;
        private readonly ApplicationContext _appContext;

        public DefaultRowAuthPolicyInitializationService(
            ISqlSugarClient db,
            IDefaultRowAuthRuleProvider defaultRuleProvider,
            IEntityInfoService entityInfoService,
            ILogger<DefaultRowAuthPolicyInitializationService> logger,
            ApplicationContext appContext)
        {
            _db = db;
            _defaultRuleProvider = defaultRuleProvider;
            _entityInfoService = entityInfoService;
            _logger = logger;
            _appContext = appContext;
        }

        public async Task InitializeDefaultPoliciesAsync()
        {
            try
            {
                _logger.LogInformation("开始初始化默认行级权限策略...");

                // 获取所有业务类型
                var allBizTypes = Enum.GetValues(typeof(BizType)).Cast<BizType>().ToList();

                foreach (var bizType in allBizTypes)
                {
                    if (bizType == BizType.无对应数据) continue;

                    await EnsureDefaultPoliciesForBizTypeAsync(bizType);
                }

                _logger.LogInformation("默认行级权限策略初始化完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化默认行级权限策略时发生错误");
                throw;
            }
        }

        public async Task<bool> EnsureDefaultPoliciesForBizTypeAsync(BizType bizType)
        {
            try
            {
                var policies = await GetOrCreateDefaultPoliciesAsync(bizType);
                return policies != null && policies.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "确保业务类型 {BizType} 的默认策略时发生错误", bizType);
                return false;
            }
        }

        public async Task<List<tb_RowAuthPolicy>> GetOrCreateDefaultPoliciesAsync(BizType bizType)
        {
            try
            {
                // 获取实体信息
                var entityInfo = _entityInfoService.GetEntityInfo(bizType);
                if (entityInfo == null)
                {
                    _logger.LogWarning("未找到业务类型 {BizType} 对应的实体信息", bizType);
                    return new List<tb_RowAuthPolicy>();
                }

                // 检查是否已存在默认策略
                var existingPolicies = _db.Queryable<tb_RowAuthPolicy>()
                    .Where(p => p.TargetEntity == entityInfo.EntityName && p.DefaultRuleEnum.HasValue)
                    .ToList();

                if (existingPolicies.Any())
                {
                    return existingPolicies;
                }

                // 获取该业务类型支持的默认规则选项
                var defaultOptions = _defaultRuleProvider.GetDefaultRuleOptions(bizType);
                var newPolicies = new List<tb_RowAuthPolicy>();

                // 为每个默认选项创建策略
                foreach (var option in defaultOptions)
                {
                    var policy = _defaultRuleProvider.CreatePolicyFromDefaultOption(bizType, option, 0);
                    policy.DefaultRuleEnum =policy.DefaultRuleEnum;
                    policy.PolicyName = $"[默认] {policy.PolicyName}";

                    // 插入数据库
                    policy.PolicyId = _db.Insertable(policy).ExecuteReturnBigIdentity();
                    newPolicies.Add(policy);

                    _logger.LogInformation("已创建默认策略: {PolicyName} (ID: {PolicyId})", policy.PolicyName, policy.PolicyId);
                }

                return newPolicies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取或创建业务类型 {BizType} 的默认策略时发生错误", bizType);
                throw;
            }
        }
    }
}
