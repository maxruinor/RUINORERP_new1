using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.ReminderRuleStrategy
{
    /// <summary>
    /// 根据销售趋势查询数据的策略
    /// </summary>
    public class SalesTrendStrategy : IReminderStrategy
    {
        public int Priority => 0;

        public bool CanHandle(ReminderBizType reminderType)
        {
            return true;
        }

        //public  Task CheckAsync(IReminderRule policy, tb_Inventory stock)
        //{
        //    // 实现销售趋势分析逻辑
        //}

        public async Task<bool> CheckAsync(IReminderRule rule, IReminderContext context)
        {
            // 实现销售趋势分析逻辑
           
            return true;
        }
    }
 
}
