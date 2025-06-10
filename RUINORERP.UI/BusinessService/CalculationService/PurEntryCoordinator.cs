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
            decimal totalQty = _details.Sum(d => d.Quantity);
            if (totalQty <= 0) return;

            switch (propertyName)
            {
                case nameof(tb_PurEntry.ShipCost):
                    AllocateFreight(_master.ShipCost, d => d.AllocatedFreightCost);
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
            if (masterValue == 0 && _details.Sum(d => d.AllocatedFreightCost) == 0)
            {
                return;
            }

            string detailPropertyName = detailSelector.GetMemberInfo().Name;
            string mapKey = MapFields.FirstOrDefault(m => m.Value == detailPropertyName).Key;

            if (mapKey == null) return;

            var allocatedTotal = _details.Sum(d => (decimal?)typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (Math.Abs((decimal)typeof(tb_PurEntry).GetProperty(mapKey)?.GetValue(_master) - allocatedTotal) < 0.001m)
                return;

            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules != (int)FreightAllocationRules.产品数量占比)
                return;


            if (_master.TotalQty <= 0) return;

            foreach (var detail in _details)
            {
                var quantity = detail.Quantity.ObjToDecimal();
                var allocatedValue = masterValue * (quantity / _master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
                typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.SetValue(detail, allocatedValue);
            }

            if (masterValue != _details.Sum(d => d.AllocatedFreightCost))
            {
                #region 如果因为分摊时 四舍五入导致总和不等于主表值，再采用“比例分配+余数调整”的方法重新分摊
                decimal remainingFreight = masterValue;
                int lastDetailIndex = _details.Count - 1;

                for (int i = 0; i < _details.Count; i++)
                {
                    var detail = _details[i];
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
                        var allocatedValue = masterValue * (quantity / _master.TotalQty).ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
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

                decimal freightSum = _details.Sum(d => d.AllocatedFreightCost);
                if (Math.Abs(_master.ShipCost - freightSum) > 0.001m)
                {
                    _master.ShipCost = freightSum;
                }

                CalculateSummary();
            });
        }

        private void CalculateSummary()
        {
            _master.TotalQty = _details.Sum(d => d.Quantity);
            _master.TotalAmount = _details.Sum(d => (d.UnitPrice + d.CustomizedCost) * d.Quantity) + _master.ShipCost;
            _master.TotalTaxAmount = _details.Sum(c => c.TaxAmount);
            _master.TotalTaxAmount = _master.TotalTaxAmount.ToRoundDecimalPlaces(_authController.GetMoneyDataPrecision());
        }
    }
}
