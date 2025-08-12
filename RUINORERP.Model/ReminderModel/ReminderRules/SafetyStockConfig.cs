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
    public class SafetyStockConfig : RuleConfigBase
    {
        //// 辅助属性，不参与JSON序列化
        //[JsonIgnore]
        //public string _ProductIds
        //{
        //    get => string.Join(",", ProductIds);
        //    set => ProductIds = value?.Split(',')
        //                         .Select(long.Parse)
        //                         .ToList() ?? new List<long>();
        //}

        [Description("要检测的产品对象")]
        public List<long> ProductIds { get; set; } = new List<long>();

        [Description("最小安全库存")]
        public int MinStock { get; set; }

        [Description("最大安全库存")]
        public int MaxStock { get; set; }

        [Description("补货数量")]
        public long ReorderQuantity { get; set; }


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
