using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.UCSourceGrid;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.Common.Extensions;
using SourceGrid;
using static OpenTK.Graphics.OpenGL.GL;

namespace RUINORERP.UI.BusinessService.CalculationService
{
    /// <summary>
    /// 销售出库单双向计算协调器,主表字段影响 子表。子表也影响主表
    /// </summary>
    public class PurEntryCoordinator : SourceGridCoordinator<tb_PurEntry, tb_PurEntryDetail>
    {
        private readonly AuthorizeController _authController;

        public PurEntryCoordinator(
            tb_PurEntry master,
            List<tb_PurEntryDetail> details,
            SourceGridHelper gridHelper)
            : base(master, details, gridHelper)
        {
            _authController = MainForm.Instance.authorizeController;
            SetMapFields();
        }

        protected override void SetMapFields()
        {
            SetMapFields(c => c.ShipCost, d => d.AllocatedFreightCost);
        }

        protected override void HandleMasterPropertyChange(string propertyName)
        {
            decimal totalQty = Details.Sum(d => d.Quantity);
            if (totalQty <= 0) return;

            switch (propertyName)
            {
                case nameof(tb_PurEntry.ShipCost):
                    AllocateFreight(Master.ShipCost, d => d.AllocatedFreightCost);
                    break;
            }

            CalculateSummary();
        }

        /// <summary>
        /// 主表字段变化时调用
        /// </summary>
        /// <param name="masterValue"></param>
        /// <param name="detailSelector"></param>
        private void AllocateFreight(decimal masterValue, Expression<Func<tb_PurEntryDetail, decimal>> detailSelector)
        {
            if (masterValue == 0 && Details.Sum(d => d.AllocatedFreightCost) == 0)
            {
                return;
            }

            string detailPropertyName = detailSelector.GetMemberInfo().Name;
            string mapKey = MapFields.FirstOrDefault(m => m.Value == detailPropertyName).Key;

            if (mapKey == null) return;

            var allocatedTotal = Details.Sum(d => (decimal?)typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (Math.Abs((decimal)typeof(tb_PurEntry).GetProperty(mapKey)?.GetValue(Master) - allocatedTotal) < 0.001m)
                return;

            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules != (int)FreightAllocationRules.产品数量占比)
                return;


            if (Master.TotalQty <= 0) return;

            foreach (var detail in Details)
            {
                var quantity = detail.Quantity.ObjToDecimal();
                var allocatedValue = masterValue * (quantity / Master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.SetValue(detail, allocatedValue);
            }

            if (masterValue != Details.Sum(d => d.AllocatedFreightCost))
            {
                #region 如果因为分摊时 四舍五入导致总和不等于主表值，再采用“比例分配+余数调整”的方法重新分摊
                decimal remainingFreight = masterValue;
                int lastDetailIndex = Details.Count - 1;

                for (int i = 0; i < Details.Count; i++)
                {
                    var detail = Details[i];
                    var quantity = detail.Quantity.ObjToDecimal();

                    if (i == lastDetailIndex)
                    {
                        // 最后一行调整余数，确保总和等于主表值
                        var allocatedValue = remainingFreight.ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                        detail.SetPropertyValue(detailPropertyName, allocatedValue);
                        //typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.SetValue(detail, allocatedValue);
                        remainingFreight -= allocatedValue;
                    }
                    else
                    {
                        var allocatedValue = masterValue * (quantity / Master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                        //typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.SetValue(detail, allocatedValue);
                        detail.SetPropertyValue(detailPropertyName, allocatedValue);
                        remainingFreight -= allocatedValue;
                    }

                }
                #endregion
            }
            _gridHelper.UpdateGridColumn<tb_PurEntryDetail>(detailPropertyName);

        }

        protected override void HandleDetailChange(tb_PurEntry master, tb_PurEntryDetail detail)
        {
            SafeExecute(() =>
            {

                decimal freightSum = Details.Sum(d => d.AllocatedFreightCost);
                if (Math.Abs(Master.ShipCost - freightSum) > 0.001m)
                {
                    Master.ShipCost = freightSum;
                }

                CalculateSummary();
            });
        }

        private void CalculateSummary()
        {
            Master.TotalQty = Details.Sum(d => d.Quantity);
            Master.TotalAmount = Details.Sum(d => (d.UnitPrice + d.CustomizedCost) * d.Quantity) + Master.ShipCost;
            Master.TotalTaxAmount = Details.Sum(c => c.TaxAmount);
            Master.TotalTaxAmount = Master.TotalTaxAmount.ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
        }
    }
}
