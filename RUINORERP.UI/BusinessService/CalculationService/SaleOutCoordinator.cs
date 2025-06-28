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
    public class SaleOutCoordinator : SourceGridCoordinator<tb_SaleOut, tb_SaleOutDetail>
    {
        private readonly AuthorizeController _authController;

        public SaleOutCoordinator(
            tb_SaleOut master,
            List<tb_SaleOutDetail> details,
            SourceGridHelper gridHelper)
            : base(master, details, gridHelper)
        {
            _authController = MainForm.Instance.authorizeController;
            SetMapFields();
        }

        protected override void SetMapFields()
        {
            SetMapFields(c => c.FreightCost, d => d.AllocatedFreightCost);
            SetMapFields(c => c.FreightIncome, d => d.AllocatedFreightIncome);
        }

        protected override void HandleMasterPropertyChange(string propertyName)
        {
            if (Details==null)
            {
                return;
            }
            decimal totalQty = Details.Sum(d => d.Quantity);
            if (totalQty <= 0) return;

            switch (propertyName)
            {
                case nameof(tb_SaleOut.FreightCost):
                    AllocateFreight(propertyName, Master.FreightCost, d => d.AllocatedFreightCost);
                    break;
                case nameof(tb_SaleOut.FreightIncome):
                    AllocateFreight(propertyName, Master.FreightIncome, d => d.AllocatedFreightIncome);
                    break;
            }

            CalculateSummary();
        }

        private void AllocateFreight(string masterPropertyName, decimal masterValue, Expression<Func<tb_SaleOutDetail, decimal>> detailSelector)
        {
            if (Details == null)
            {
                return;
            }
            string detailPropertyName = detailSelector.GetMemberInfo().Name;
            //如果汇总的字段都为0才不计算。
            decimal allocatedFreightCostTotal = 0;
            allocatedFreightCostTotal = Details.Sum(d => (decimal?)typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (masterValue == 0 && allocatedFreightCostTotal == 0)
            {
                return;
            }


            string mapKey = MapFields.FirstOrDefault(m => m.Value == detailPropertyName).Key;

            if (mapKey == null) return;

            var allocatedTotal = Details.Sum(d => (decimal?)typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (Math.Abs((decimal)typeof(tb_SaleOut).GetProperty(mapKey)?.GetValue(Master) - allocatedTotal) < 0.001m)
                return;

            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules != (int)FreightAllocationRules.产品数量占比)
                return;

            Master.TotalQty = Details.Sum(d => d.Quantity);

            foreach (var detail in Details)
            {
                var quantity = detail.Quantity.ObjToDecimal();
                var allocatedValue = masterValue * (quantity / Master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.SetValue(detail, allocatedValue);
            }

            //计算后新值的和
            allocatedFreightCostTotal = Details.Sum(d => (decimal?)typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (masterValue != allocatedFreightCostTotal)
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
                        remainingFreight -= allocatedValue;
                    }
                    else
                    {
                        var allocatedValue = masterValue * (quantity / Master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                        detail.SetPropertyValue(detailPropertyName, allocatedValue);
                        remainingFreight -= allocatedValue;
                    }

                }
                #endregion
            }

            _gridHelper.UpdateGridColumn<tb_SaleOutDetail>(detailPropertyName);
        }

        protected override void HandleDetailChange(tb_SaleOut master, tb_SaleOutDetail detail)
        {
            SafeExecute(() =>
            {
                detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;

                decimal freightSum = Details.Sum(d => d.AllocatedFreightCost);
                if (Math.Abs(Master.FreightCost - freightSum) > 0.001m)
                {
                    Master.FreightCost = freightSum;
                }

                decimal freightIncomeSum = Details.Sum(d => d.AllocatedFreightIncome);
                if (Math.Abs(Master.FreightIncome - freightIncomeSum) > 0.001m)
                {
                    Master.FreightIncome = freightIncomeSum;
                }

                CalculateSummary();
            });
        }

        private void CalculateSummary()
        {
            Master.TotalQty = Details.Sum(d => d.Quantity);
            Master.TotalCost = Details.Sum(d => (d.Cost + d.CustomizedCost) * d.Quantity) + Master.FreightCost;
            Master.TotalAmount = Details.Sum(d => d.TransactionPrice * d.Quantity) + Master.FreightIncome;
            Master.TotalCommissionAmount = Details.Sum(c => c.CommissionAmount);
            Master.TotalTaxAmount = Details.Sum(c => c.SubtotalTaxAmount);
            Master.TotalTaxAmount = Master.TotalTaxAmount.ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());

        }
    }
}
