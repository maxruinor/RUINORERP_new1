using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Model.ReminderModel;
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
    public class CachedRuleEngineCenter(IMemoryCache cache,
        ILogger<SmartReminderMonitor> logger,
        ApplicationContext _AppContextData,
        IUnitOfWorkManage unitOfWorkManage) : RuleEngineCenter
    {
        private readonly IMemoryCache _cache = cache;
        private readonly ApplicationContext _appContext = _AppContextData;
        private readonly ILogger<SmartReminderMonitor> _logger = logger;

        public override async Task<bool> EvaluateAsync(IReminderRule rule, object context)
        {
            var cacheKey = $"{rule.RuleEngineType}_{rule.RuleId}";
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return await base.EvaluateAsync(rule, context);
            });
        }

        public override async Task<bool> EvaluateAsync(IReminderRule rule, object context)
        {
            //var redis = _redis.GetDatabase();
            var cacheKey = $"rule:{rule.RuleId}:eval";

            var cachedResult = await redis.StringGetAsync(cacheKey);
            if (cachedResult.HasValue)
                return (bool)cachedResult;

            var result = await base.EvaluateAsync(rule, context);
            await redis.StringSetAsync(cacheKey, result, expiry: TimeSpan.FromMinutes(5));
            return result;
        }


    }
}
