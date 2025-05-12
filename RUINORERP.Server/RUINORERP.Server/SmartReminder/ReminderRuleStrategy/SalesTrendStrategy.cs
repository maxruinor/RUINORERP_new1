using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.ReminderRuleStrategy
{
    public class SalesTrendStrategy : IAlertStrategy
    {
        public int Priority => 2;

        public async Task CheckAsync(tb_ReminderRule policy, tb_Inventory stock)
        {
            // 实现销售趋势分析逻辑
        }
    }

    // 注册到DI容器
    //services.AddSingleton<IAlertStrategy, SalesTrendStrategy>();
}
