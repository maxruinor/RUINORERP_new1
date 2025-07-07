using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel
{
    /// <summary>
    /// 要提醒的内容
    /// </summary>
    public interface IReminderContext
    {
        Type DataType { get; }
        object GetData();
    }

    /// <summary>
    /// 提醒规则配置接口
    /// </summary>
    public interface IRuleConfig
    {
        /// <summary>
        /// 提醒频率（分钟）
        /// </summary>
        int ReminderIntervalMinutes { get; set; }

        /// <summary>
        /// 规则是否启用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 规则唯一标识
        /// </summary>
        string RuleId { get; set; }


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

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        bool Validate();
    }

    /// <summary>
    /// 提醒优先级
    /// </summary>
    public enum ReminderPriority
    {
        [Description("低")]
        Low = 1,
        [Description("中")]
        Medium = 2,
        [Description("高")]
        High = 3,
        [Description("紧急")]
        Critical = 4

    }
}
