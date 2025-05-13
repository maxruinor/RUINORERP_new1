using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Server.SmartReminder.ReminderContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.Strategies
{
    // File: Strategies/BaseStrategy.cs
    public abstract class BaseStrategy<T> : IReminderStrategy where T : IReminderContext
    {
        protected readonly IRuleEngineCenter _ruleEngine;
        protected readonly INotificationService _notification;
        protected readonly ILogger _logger;

        protected BaseStrategy(
            IRuleEngineCenter ruleEngine,
            INotificationService notification,
            ILogger logger)
        {
            _ruleEngine = ruleEngine;
            _notification = notification;
            _logger = logger;
        }

        public int Priority => throw new NotImplementedException();

        public abstract bool CanHandle(ReminderBizType reminderType);

        public async Task CheckAsync(IReminderRule rule, IReminderContext context)
        {
            if (context is T typedContext)
            {
                var result = await _ruleEngine.EvaluateAsync(rule, typedContext.GetData());

                if (result)
                {
                    var message = BuildMessage(rule, typedContext);
                    await _notification.SendNotificationAsync(rule, message, typedContext.GetData());
                }
            }
        }

        protected abstract string BuildMessage(IReminderRule rule, T context);
    }
}
