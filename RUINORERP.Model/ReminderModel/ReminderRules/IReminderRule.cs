using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{
    public interface IReminderRule
    {
        long RuleId { get; set; }

        int ReminderBizType { get; set; }

        int RuleEngineType { get; set; }

        // 新增离线消息处理属性
        bool PersistUntilDelivered { get; set; }

        /// <summary>
        /// 通知渠道，枚举值
        /// </summary>
        List<int> NotifyChannels { get; set; }

        List<long> NotifyRecipients { get; set; }


        IRuleConfig GetConfig<T>();


    }
}
