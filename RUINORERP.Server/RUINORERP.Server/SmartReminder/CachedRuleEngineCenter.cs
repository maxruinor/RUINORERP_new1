using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.SmartReminder.InvReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public class CachedRuleEngineCenter(IMemoryCache cache,
        ILogger<InventoryMonitor> logger,
        ApplicationContext _AppContextData,
        IUnitOfWorkManage unitOfWorkManage) : RuleEngineCenter
    {
        private readonly IMemoryCache _cache = cache;
        private readonly ApplicationContext _appContext = _AppContextData;
        private readonly ILogger<InventoryMonitor> _logger = logger;

        public override async Task<bool> EvaluateAsync(IReminderRule rule, object context)
        {
            var cacheKey = $"{rule.RuleEngineType}_{rule.RuleId}";
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return await base.EvaluateAsync(rule, context);
            });
        }
    }
}
