using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;
using System;
using System.Collections.Concurrent;
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
        // 使用并发集合跟踪已初始化的业务类型，避免重复检查
        private readonly ConcurrentDictionary<BizType, bool> _initializedBizTypes = new ConcurrentDictionary<BizType, bool>();

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
                int initializedCount = 0;
                int skippedCount = 0;

                foreach (var bizType in allBizTypes)
                {
                    if (bizType == BizType.无对应数据) continue;

                    // 如果业务类型已经初始化过，则跳过
                    if (_initializedBizTypes.TryGetValue(bizType, out bool isInitialized) && isInitialized)
                    {
                        skippedCount++;
                        _logger.LogDebug("业务类型 {BizType} 已初始化过，跳过检查", bizType);
                        continue;
                    }

                    bool result = await EnsureDefaultPoliciesForBizTypeAsync(bizType);
                    if (result)
                    {
                        initializedCount++;
                        // 标记业务类型为已初始化
                        _initializedBizTypes.TryAdd(bizType, true);
                    }
                }

                _logger.LogInformation("默认行级权限策略初始化完成：已初始化 {InitializedCount} 个业务类型，跳过 {SkippedCount} 个已初始化业务类型",
                    initializedCount, skippedCount);
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
//                    _logger.LogWarning("未找到业务类型 {BizType} 对应的实体信息", bizType);
                    return new List<tb_RowAuthPolicy>();
                }

                // 验证实体信息中的必要字段
                if (string.IsNullOrEmpty(entityInfo.TableName) || string.IsNullOrEmpty(entityInfo.EntityName))
                {
                    _logger.LogWarning("业务类型 {BizType} 的实体信息缺少必要字段：TableName={TableName}, EntityName={EntityName}",
                        bizType, entityInfo.TableName, entityInfo.EntityName);
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

                // 使用事务确保数据一致性
                using (var transaction = _db.Ado.UseTran())
                {
                    try
                    {
                        // 为每个默认选项创建策略
                        foreach (var option in defaultOptions)
                        {
                            var policy = _defaultRuleProvider.CreatePolicyFromDefaultOption(bizType, option, 0);
                            // 设置DefaultRuleEnum为选项的Key值（确保在int范围内）
                            if (option.Key >= int.MinValue && option.Key <= int.MaxValue)
                            {
                                policy.DefaultRuleEnum = Convert.ToInt32(option.Key);
                            }
                            policy.PolicyName = $"[默认] {policy.PolicyName}";

                            // 再次验证必填字段
                            if (string.IsNullOrEmpty(policy.PolicyName) || string.IsNullOrEmpty(policy.TargetTable) ||
                                string.IsNullOrEmpty(policy.TargetEntity))
                            {
                                _logger.LogWarning("创建的策略缺少必填字段: {PolicyName}, {TargetTable}, {TargetEntity}",
                                    policy.PolicyName, policy.TargetTable, policy.TargetEntity);
                                continue;
                            }

                            // 插入数据库
                            policy.PolicyId = _db.Insertable(policy).ExecuteReturnSnowflakeId();
                            newPolicies.Add(policy);

                            _logger.LogInformation("已创建默认策略: {PolicyName} (ID: {PolicyId})", policy.PolicyName, policy.PolicyId);
                        }

                        // 提交事务
                        transaction.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        // 回滚事务
                        transaction.RollbackTran();
                        _logger.LogError(ex, "创建默认策略时发生错误，事务已回滚: {BizType}", bizType);
                        throw;
                    }
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
