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
       
        int ReminderBizType { get; set; }
        int NotifyChannel { get; set; }
        
        int RuleEngineType { get; set; }

        ///// <summary>
        ///// long类型的userid，用逗号隔开
        ///// </summary>
         string NotifyRecipients { get; set; }

        IRuleConfig GetConfig<T>();


    }
}
