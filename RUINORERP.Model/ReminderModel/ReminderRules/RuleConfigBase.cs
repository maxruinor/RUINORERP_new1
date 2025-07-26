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
        // 实现IRuleConfig接口
        [Description("提醒间隔时间（分钟）")]
        public int ReminderIntervalMinutes { get; set; } = 60; // 默认每小时提醒一次
        bool EnableRealtimeReminders { get; set; }

        private bool Is_enabled = true;
        // 修正属性名与接口一致
        public bool IsEnabled
        {
            get => Is_enabled;
            set => Is_enabled = value;
        }

        /// <summary>
        /// 通知渠道
        /// </summary>
        public List<int> NotifyChannels { get; set; }

        public string RuleId { get; set; } = Guid.NewGuid().ToString();
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ReminderPriority Priority { get; set; } = ReminderPriority.Medium;

        //public bool Validate()
        //{
        //    return ProductIds?.Any() == true
        //        && MinStock >= 0
        //        && MaxStock > MinStock
        //        && ReminderIntervalMinutes > 0;
        //}

        public string RuleName { get; set; } = "未命名规则";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        bool IRuleConfig.EnableRealtimeReminders { get; set; } = true;

        //bool IRuleConfig.SaveReminders { get; set; } = true;
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual bool Validate()
        {
            if (string.IsNullOrWhiteSpace(RuleName))
                throw new ArgumentException("规则名称不能为空");

            if (RuleName.Length > 100)
                throw new ArgumentException("规则名称不能超过100个字符");
            return true;
        }

    }
}
