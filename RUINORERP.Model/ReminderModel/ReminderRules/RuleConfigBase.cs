using Newtonsoft.Json;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{
    // 基础配置实现
    public abstract class RuleConfigBase : IRuleConfig
    {

        [Description("检测频率(分钟)")]
        public int CheckIntervalByMinutes { get;  set; }


        //bool IRuleConfig.SaveReminders { get; set; } = true;
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual bool Validate()
        {
            return true;
        }

    }
}
