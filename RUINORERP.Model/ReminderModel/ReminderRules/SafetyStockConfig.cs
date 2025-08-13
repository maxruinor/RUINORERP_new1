using Newtonsoft.Json;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel.ReminderResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{
    public class SafetyStockConfig : RuleConfigBase
    {
 

        // 添加工作流所需的所有参数
        [Description("计算周期（天）")]
        public int CalculationPeriodDays { get; set; } = 90;

        /// <summary>
        /// 安全库存是通过计算来的。当然也可以人工手动指定
        /// </summary>
        [Description("安全库存计算频率(天)")]
        public int CalculateSafetyStockIntervalByDays { get; set; } = 3;


        [Description("采购提前期（天）")]
        public int PurchaseLeadTimeDays { get; set; } = 7;

        [Description("服务水平系数")]
        public double ServiceLevelFactor { get; set; } = 1.64;

        [Description("是否手动指定安全库存")]
        public bool IsCalculateSafetyStockByHand { get; set; } = false;


        [Description("手动指定安全库存")]
        public bool ManualSafetyStockLevel { get; set; }

        [Description("要检测的产品对象")]
        public List<long> ProductIds { get; set; } = new List<long>();

        [Description("最小安全库存")]
        public int MinStock { get; set; }

        [Description("最大安全库存")]
        public int MaxStock { get; set; }

        [Description("预警激活")]
        public bool Alert_Activation { get; set; } = true;


        [Description("提醒库位")]
        public List<long> LocationIds { get; set; } = new List<long>();


        /// <summary>
        /// 重写验证方法，先执行基类验证，再添加子类特有验证
        /// </summary>
        /// <returns></returns>
        public override RuleValidationResult Validate()
        {
            // 先执行基类的验证逻辑
            var result = base.Validate();

            // 产品ID验证
            if (ProductIds == null || !ProductIds.Any())
            {
                result.AddError("必须选择至少一个产品");
            }
            else if (ProductIds.Any(id => id <= 0))
            {
                result.AddError("产品ID不能为负数或零");
            }

            // 库位验证
            if (LocationIds == null || !LocationIds.Any())
            {
                result.AddError("必须选择至少一个库位");
            }
            else if (LocationIds.Any(id => id <= 0))
            {
                result.AddError("库位ID不能为负数或零");
            }

            // 库存数量验证
            if (MinStock < 0)
            {
                result.AddError("最小安全库存不能小于0");
            }

            if (MaxStock <= MinStock)
            {
                result.AddError("最大安全库存必须大于最小安全库存");
            }

            // 补货数量验证
            //if (ReorderQuantity <= 0)
            //{
            //    result.AddError("补货数量必须大于0");
            //}

            // 检测频率特殊验证（安全库存检查建议不低于30分钟）
            if (CheckIntervalByMinutes > 0 && CheckIntervalByMinutes < 30)
            {
                result.AddError("安全库存检测频率建议不低于30分钟");
            }

            return result;
        }
         
    }
}
