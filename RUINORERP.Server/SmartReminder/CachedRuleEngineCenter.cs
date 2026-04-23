using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.SmartReminder.InvReminder;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    /// <summary>
    /// 带缓存的规则引擎中心
    /// </summary>
    public class CachedRuleEngineCenter : RuleEngineCenter
    {
        private readonly IMemoryCache _cache;
        private readonly IDatabase _redis;
        private readonly ILogger<CachedRuleEngineCenter> _logger;

        public CachedRuleEngineCenter(
            IMemoryCache cache,
            IConnectionMultiplexer redis, // 注入 Redis 连接管理器
            ILogger<CachedRuleEngineCenter> logger) 
            : base(logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _redis = redis?.GetDatabase() ?? throw new ArgumentNullException(nameof(redis));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 评估规则（带内存缓存）
        /// </summary>
        public override async Task<bool> EvaluateAsync(IReminderRule rule, object context)
        {
            var cacheKey = $"rule_eval_{rule.RuleId}";
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                entry.SetSize(1);
                _logger.LogDebug("从内存缓存获取或计算规则结果: {RuleId}", rule.RuleId);
                return await base.EvaluateAsync(rule, context);
            });
        }
    }
}
