using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderResults
{
    // 提醒结果基础接口
    public interface IReminderResult
    {
        string ResultId { get; }
        long RuleId { get; }
        ReminderBizType BusinessType { get; }
        DateTime TriggerTime { get; }
        string Title { get; }
        string Summary { get; }
        bool IsRead { get; set; }
        bool IsProcessed { get; set; }
        bool IsArchived { get; set; }
        string ToDisplayJson();
        IEnumerable<DataColumn> GetDataColumns();
        IEnumerable<object[]> GetDataRows();
    }
}
