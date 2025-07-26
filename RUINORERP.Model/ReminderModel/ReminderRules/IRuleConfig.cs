using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{
    /// <summary>
    /// 提醒规则配置接口
    /// </summary>
    public interface IRuleConfig
    {
        /// <summary>
        /// 规则唯一标识
        /// </summary>
        string RuleId { get; set; }

        List<int> NotifyChannels { get; set; }
        /// <summary>
        /// 提醒频率（分钟）
        /// </summary>
        int ReminderIntervalMinutes { get; set; }

        /// <summary>
        /// 启用实时提醒
        /// </summary>
        bool EnableRealtimeReminders { get; set; }

        /// <summary>
        /// 规则是否启用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 提醒开始时间
        /// </summary>
        DateTime? StartTime { get; set; }

        /// <summary>
        /// 提醒结束时间
        /// </summary>
        DateTime? EndTime { get; set; }

        /// <summary>
        /// 提醒优先级
        /// </summary>
        ReminderPriority Priority { get; set; }


        //用户不在线，是否上线后再提醒

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        bool Validate();
    }
}
