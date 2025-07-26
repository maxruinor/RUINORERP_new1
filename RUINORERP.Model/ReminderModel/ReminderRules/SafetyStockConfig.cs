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
        // 辅助属性，不参与JSON序列化
        [JsonIgnore]
        public string _ProductIds
        {
            get => string.Join(",", ProductIds);
            set => ProductIds = value?.Split(',')
                                 .Select(long.Parse)
                                 .ToList() ?? new List<long>();
        }

        public List<long> ProductIds { get; set; } = new List<long>();

        [Description("最小安全库存")]
        public int MinStock { get; set; }

        [Description("最大安全库存")]
        public int MaxStock { get; set; }
 
        [Description("补货数量")]
        public long ReorderQuantity { get; set; }
     


        //提醒库位
        [Description("提醒库位")]
        public List<long> LocationIds { get; set; } = new List<long>();
 

        public override bool Validate()
        {
            return ProductIds?.Any() == true
                && MinStock >= 0
                && MaxStock > MinStock
                && ReminderIntervalMinutes > 0;
        }

        //public override void Validate()
        //{
        //    base.Validate();

        //    if (MinStock < 0)
        //        throw new ArgumentException("安全库存下限不能小于0");

        //    if (MaxStock <= MinStock)
        //        throw new ArgumentException("安全库存上限必须大于下限");

        //    if (ReorderQuantity <= 0)
        //        throw new ArgumentException("补货数量必须大于0");

        //    if (ProductIds == null || !ProductIds.Any())
        //        throw new ArgumentException("必须指定至少一个产品");
        //}
    }
}
