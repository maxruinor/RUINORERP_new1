using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation.Condition
{
    /// <summary>
    /// 并不完善
    /// </summary>
    public class DaysCondition : ICondition
    {
        public bool IsSatisfied(Dictionary<string, object> data)
        {
            if (data.TryGetValue("Days", out var daysObj) && daysObj is decimal days)
            {
                return days <= 1;
            }
            return false;
        }
    }

    public class MoreThanOneDayCondition : ICondition
    {
        public bool IsSatisfied(Dictionary<string, object> data)
        {
            if (data.TryGetValue("Days", out var daysObj) && daysObj is decimal days)
            {
                return days > 1;
            }
            return false;
        }
    }
}
