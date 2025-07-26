using Newtonsoft.Json;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderResults
{
    // 基础结果实现
    public abstract class BaseReminderResult : IReminderResult
    {
        public string ResultId { get; } = Guid.NewGuid().ToString();
        public long RuleId { get; set; }
        public abstract ReminderBizType BusinessType { get; }
        public DateTime TriggerTime { get; set; } = DateTime.Now;
        public abstract string Title { get; }
        public abstract string Summary { get; }
        public bool IsRead { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsArchived { get; set; }

        public virtual string ToDisplayJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public abstract IEnumerable<DataColumn> GetDataColumns();
        public abstract IEnumerable<object[]> GetDataRows();
    }
}
