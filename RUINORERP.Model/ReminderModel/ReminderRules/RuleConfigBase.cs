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

        /// <summary>
        /// 验证配置，返回包含消息的结果
        /// </summary>
        /// <returns></returns>
        public virtual RuleValidationResult Validate()
        {
            var result = new RuleValidationResult();

            // 基础验证逻辑：检测频率必须大于0
            if (CheckIntervalByMinutes <= 0)
            {
                result.AddError("检测频率必须大于0分钟");
            }
            // 可以根据业务需要添加更多基础验证规则

            return result;
        }

    }
}
