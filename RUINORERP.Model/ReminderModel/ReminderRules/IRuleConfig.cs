using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{
    /// <summary>
    /// 提醒规则配置接口
    /// 只关注业务。通用配置在规则表中定义好了
    /// </summary>
    public interface IRuleConfig
    {
        /// <summary>
        /// 提醒频率（分钟）
        /// </summary>
        //int ReminderIntervalMinutes { get; set; }

        [Description("检测频率(分钟)")]
         int CheckIntervalByMinutes { get; set; }


        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        bool Validate();
    }
}
