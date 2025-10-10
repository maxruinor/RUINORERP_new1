using RUINORERP.Model.ReminderModel;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel.ReminderRules;

namespace RUINORERP.Server.SmartReminder.Strategies
{
    // File: Strategies/InventoryStrategy.cs
    public class InventoryStrategy : BaseStrategy<InventoryContext>
    {
        public InventoryStrategy(
            IRuleEngineCenter ruleEngine,
            INotificationService notification,
            ILogger<InventoryStrategy> logger)
            : base(ruleEngine, notification, logger)
        {
        }

        public override bool CanHandle(ReminderBizType reminderType) =>
            reminderType == ReminderBizType.安全库存提醒 ||
            reminderType == ReminderBizType.库存积压提醒;

        protected override string BuildMessage(IReminderRule rule, InventoryContext context)
        {
            var stock = (tb_Inventory)context.GetData();
            return rule switch
            {
               // tb_ReminderRule ssRule => $"安全库存预警：产品 {stock.ProdDetailID} 当前库存 {stock.Quantity}，安全范围 ({ssRule.BusinessConfig.MinStock}-{ssRule.BusinessConfig.MaxStock})",
               // OverstockRule oRule => $"库存积压预警：产品 {stock.ProdDetailID} 当前库存 {stock.Quantity}，超过阈值 {oRule.Threshold}",
                _ => "库存预警通知"
            };
        }
    }
}
