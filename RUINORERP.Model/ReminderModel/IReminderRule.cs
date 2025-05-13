using RUINORERP.Global.EnumExt;
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

        //脚本的代码，可能要用一个方法从数据库中的来转？
        string Condition { get; set; }

        //也要从数据为中转换为过
        ReminderBizType  ReminderBiz  { get; set; }
        string NotifyChannels { get; set; }
        int RuleEngineType { get; set; }
    }
}
