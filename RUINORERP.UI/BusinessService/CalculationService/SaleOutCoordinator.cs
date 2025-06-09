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
            decimal totalQty = _details.Sum(d => d.Quantity);
            if (totalQty <= 0) return;

            switch (propertyName)
            {
                case nameof(tb_SaleOut.FreightCost):
                    AllocateFreight(propertyName, _master.FreightCost, d => d.AllocatedFreightCost);
                    break;
                case nameof(tb_SaleOut.FreightIncome):
                    AllocateFreight(propertyName, _master.FreightIncome, d => d.AllocatedFreightIncome);
                    break;
            }

            CalculateSummary();
        }

        private void AllocateFreight(string masterPropertyName, decimal masterValue, Expression<Func<tb_SaleOutDetail, decimal>> detailSelector)
        {
            if (masterValue == 0) return;

            string detailPropertyName = detailSelector.GetMemberInfo().Name;
            string mapKey = MapFields.FirstOrDefault(m => m.Value == detailPropertyName).Key;

            if (mapKey == null) return;

            var allocatedTotal = _details.Sum(d => (decimal?)typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (Math.Abs((decimal)typeof(tb_SaleOut).GetProperty(mapKey)?.GetValue(_master) - allocatedTotal) < 0.001m)
                return;

            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules != (int)FreightAllocationRules.产品数量占比)
                return;

            _master.TotalQty = _details.Sum(d => d.Quantity);

            foreach (var detail in _details)
            {
                var quantity = detail.Quantity.ObjToDecimal();
                var allocatedValue = masterValue * (quantity / _master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.SetValue(detail, allocatedValue);
            }

            _gridHelper.UpdateGridColumn<tb_SaleOutDetail>(detailPropertyName);
        }

        protected override void HandleDetailChange(tb_SaleOut master, tb_SaleOutDetail detail)
        {
            SafeExecute(() =>
            {
                detail.SubtotalCostAmount = detail.Cost * detail.Quantity;

                decimal freightSum = _details.Sum(d => d.AllocatedFreightCost);
                if (Math.Abs(_master.FreightCost - freightSum) > 0.001m)
                {
                    _master.FreightCost = freightSum;
                }

                decimal freightIncomeSum = _details.Sum(d => d.AllocatedFreightIncome);
                if (Math.Abs(_master.FreightIncome - freightIncomeSum) > 0.001m)
                {
                    _master.FreightIncome = freightIncomeSum;
                }

                CalculateSummary();
            });
        }

        private void CalculateSummary()
        {
            _master.TotalQty = _details.Sum(d => d.Quantity);
            _master.TotalCost = _details.Sum(d => (d.Cost + d.CustomizedCost) * d.Quantity) + _master.FreightCost;
            _master.TotalAmount = _details.Sum(d => d.TransactionPrice * d.Quantity) + _master.FreightIncome;
            _master.TotalCommissionAmount = _details.Sum(c => c.CommissionAmount);
            _master.TotalTaxAmount = _details.Sum(c => c.SubtotalTaxAmount);
            _master.TotalTaxAmount = _master.TotalTaxAmount.ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());

        }
    }
}
