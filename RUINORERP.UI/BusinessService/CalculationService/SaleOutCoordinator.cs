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

        /// <summary>
        /// 运费分摊计算：采用"比例分配+余数调整"算法
        /// 确保分摊总和严格等于主表值，最后一行承担所有四舍五入误差
        /// </summary>
        /// <param name="masterPropertyName">主表字段名称（用于日志，当前未使用）</param>
        /// <param name="masterValue">主表运费总额</param>
        /// <param name="detailSelector">明细运费分摊字段选择器</param>
        private void AllocateFreight(string masterPropertyName, decimal masterValue, Expression<Func<tb_SaleOutDetail, decimal>> detailSelector)
        {
            if (Details == null || Details.Count == 0)
            {
                return;
            }

            string detailPropertyName = detailSelector.GetMemberInfo().Name;

            // 主表运费和当前分摊总和都为0时，无需计算
            var currentAllocatedTotal = Details.Sum(d => (decimal?)typeof(tb_SaleOutDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (masterValue == 0 && currentAllocatedTotal == 0)
            {
                return;
            }

            // 验证字段映射关系
            string mapKey = MapFields.FirstOrDefault(m => m.Value == detailPropertyName).Key;
            if (mapKey == null) return;

            // 如果当前分摊总和已经等于主表值，无需重新分摊
            var masterFieldValue = (decimal?)typeof(tb_SaleOut).GetProperty(mapKey)?.GetValue(Master) ?? 0m;
            if (Math.Abs(masterFieldValue - currentAllocatedTotal) < 0.001m)
                return;

            // 检查分摊规则配置
            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules != (int)FreightAllocationRules.产品数量占比)
                return;

            // 验证总数量
            Master.TotalQty = Details.Sum(d => d.Quantity);
            if (Master.TotalQty <= 0) return;

            // 获取系统配置的金额精度
            int precision = _authController.GetMoneyDataPrecision();

            // 采用"比例分配+余数调整"算法，确保总和严格等于主表值
            decimal remainingAmount = masterValue;
            int lastDetailIndex = Details.Count - 1;

            for (int i = 0; i < Details.Count; i++)
            {
                var detail = Details[i];
                var quantity = detail.Quantity.ObjToDecimal();

                if (i == lastDetailIndex)
                {
                    // 最后一行：直接使用剩余金额，承担所有四舍五入误差
                    var allocatedValue = remainingAmount.ToRoundDecimalPlaces(precision);
                    detail.SetPropertyValue(detailPropertyName, allocatedValue);
                }
                else
                {
                    // 非最后一行：按数量比例分摊
                    var allocatedValue = masterValue * (quantity / Master.TotalQty)
                        .ToRoundDecimalPlaces(precision);
                    detail.SetPropertyValue(detailPropertyName, allocatedValue);
                    remainingAmount -= allocatedValue;
                }
            }

            // 更新网格显示
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
