using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel
{
    public interface IReminderRule
    {
        long RuleId { get; set; }
         int RuleEngineType { get; set; }
    }
}
