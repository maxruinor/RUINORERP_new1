using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderResults
{
    public class SafetyStockResult : BaseReminderResult
    {
        public override ReminderBizType BusinessType => ReminderBizType.安全库存提醒;
        public override string Title => $"安全库存预警 - {ProductName}";
        public override string Summary => $"当前库存: {CurrentStock}，安全范围: [{MinStock}-{MaxStock}]";

        // 具体业务属性
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int CurrentStock { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public int RecommendedQuantity { get; set; }
        public string Unit { get; set; }

        public override IEnumerable<DataColumn> GetDataColumns()
        {
            return new List<DataColumn>
        {
            new DataColumn("产品名称", typeof(string)),
            new DataColumn("SKU编码", typeof(string)),
            new DataColumn("当前库存", typeof(int)),
            new DataColumn("最低安全库存", typeof(int)),
            new DataColumn("最高安全库存", typeof(int)),
            new DataColumn("建议补货量", typeof(int)),
            new DataColumn("单位", typeof(string)),
            new DataColumn("提醒时间", typeof(DateTime))
        };
        }

        public override IEnumerable<object[]> GetDataRows()
        {
            return new List<object[]>
        {
            new object[] { ProductName, SKU, CurrentStock, MinStock, MaxStock, RecommendedQuantity, Unit, TriggerTime }
        };
        }
    }
}
