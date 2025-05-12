using Fireasy.Common.Extensions;
using RUINORERP.Model.ReminderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Primitives;

namespace RUINORERP.Server.SmartReminder
{
    /*
    public class TimeBasedScheduler
    {
        public bool ShouldTrigger(IReminderRule rule)
        {
            var now = DateTime.Now;
            if (rule.Schedule?.DaysOfWeek != null &&
                !rule.Schedule.DaysOfWeek.Contains(now.DayOfWeek))
                return false;

            return now.Between(rule.Schedule?.StartTime, rule.Schedule?.EndTime);
        }
    }

    // 在检查入口处添加过滤
    public async Task CheckInventoryAsync()
    {
        if (!_scheduler.ShouldTrigger(currentRule)) return;
        // ...原有逻辑...
    }
    */
}
