using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
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

        /// <summary>
        /// 获取明细行的分摊基础值
        /// </summary>
        /// <param name="detail">明细行</param>
        /// <param name="rule">分摊规则</param>
        /// <returns>分摊基础值(数量/金额)</returns>
        private decimal GetAllocationBaseValue(tb_PurEntryDetail detail, FreightAllocationRules rule)
        {
            switch (rule)
            {
                case FreightAllocationRules.产品数量占比:
                    return detail.Quantity.ObjToDecimal();
                case FreightAllocationRules.产品金额占比:
                    return detail.SubtotalAmount;
                case FreightAllocationRules.产品重量占比:
                    // 如果明细表没有重量字段,使用数量作为替代
                    return detail.Quantity.ObjToDecimal();
                default:
                    return 0m;
            }
        }

        /// <summary>
        /// 获取所有明细行的分摊基础值总和
        /// </summary>
        /// <param name="rule">分摊规则</param>
        /// <returns>总和</returns>
        private decimal GetTotalAllocationBaseValue(FreightAllocationRules rule)
        {
            return Details.Sum(d => GetAllocationBaseValue(d, rule));
        }

        protected override void HandleMasterPropertyChange(string propertyName)
        {
            if (Details == null)
            {
                return;
            }
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
        /// 运费分摊计算：采用"比例分配+余数调整"算法
        /// 确保分摊总和严格等于主表值，最后一行承担所有四舍五入误差
        /// </summary>
        /// <param name="masterValue">主表运费总额</param>
        /// <param name="detailSelector">明细运费分摊字段选择器</param>
        private void AllocateFreight(decimal masterValue, Expression<Func<tb_PurEntryDetail, decimal>> detailSelector)
        {
            if (Details == null || Details.Count == 0)
            {
                return;
            }

            // 获取容差阈值（从授权控制器获取，默认0.0001）
            decimal tolerance = _authController.GetAmountCalculationTolerance();

            string detailPropertyName = detailSelector.GetMemberInfo().Name;
            var currentAllocatedTotal = Details.Sum(d => (decimal?)typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            
            // 主表运费和当前分摊总和都在容差范围内时，无需计算
            if (Math.Abs(masterValue) <= tolerance && Math.Abs(currentAllocatedTotal) <= tolerance)
            {
                return;
            }

            // 验证字段映射关系
            string mapKey = MapFields.FirstOrDefault(m => m.Value == detailPropertyName).Key;
            if (mapKey == null) return;

            // 如果当前分摊总和已经等于主表值（在容差范围内），无需重新分摊
            var masterFieldValue = (decimal?)typeof(tb_PurEntry).GetProperty(mapKey)?.GetValue(Master) ?? 0m;
            if (Math.Abs(masterFieldValue - currentAllocatedTotal) <= tolerance)
                return;

            // 获取当前的分摊规则
            FreightAllocationRules allocationRule = (FreightAllocationRules)MainForm.Instance.AppContext.SysConfig.FreightAllocationRules;

            // 根据分摊规则计算总基础值
            decimal totalBaseValue = GetTotalAllocationBaseValue(allocationRule);
            if (totalBaseValue <= 0) return;

            // 获取系统配置的金额精度（确保为4位小数）
            int precision = _authController.GetMoneyDataPrecision();

            // 采用"比例分配+余数调整"算法，确保总和严格等于主表值
            decimal remainingAmount = masterValue;
            int lastDetailIndex = Details.Count - 1;

            for (int i = 0; i < Details.Count; i++)
            {
                var detail = Details[i];
                var baseValue = GetAllocationBaseValue(detail, allocationRule);

                if (i == lastDetailIndex)
                {
                    // 最后一行：直接使用剩余金额，承担所有四舍五入误差
                    var allocatedValue = remainingAmount.ToRoundDecimalPlaces(precision);
                    // 确保最后一行也在容差范围内
                    if (Math.Abs(allocatedValue) <= tolerance)
                    {
                        allocatedValue = 0m;
                    }
                    detail.SetPropertyValue(detailPropertyName, allocatedValue);
                    // 保存分摊规则到明细行
                    detail.FreightAllocationRules = (int)allocationRule;
                }
                else
                {
                    // 非最后一行：按基础值比例分摊
                    var allocatedValue = masterValue * (baseValue / totalBaseValue)
                        .ToRoundDecimalPlaces(precision);
                    // 确保分摊值在容差范围内
                    if (Math.Abs(allocatedValue) <= tolerance)
                    {
                        allocatedValue = 0m;
                    }
                    detail.SetPropertyValue(detailPropertyName, allocatedValue);
                    remainingAmount -= allocatedValue;
                    // 保存分摊规则到明细行
                    detail.FreightAllocationRules = (int)allocationRule;
                }
            }

            // 验证分摊总和是否在容差范围内
            var finalTotal = Details.Sum(d => (decimal?)typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.GetValue(d) ?? 0m);
            if (Math.Abs(masterValue - finalTotal) > tolerance)
            {
                // 如果超出容差，强制调整最后一行
                if (Details.Count > 0)
                {
                    var lastDetail = Details[Details.Count - 1];
                    var currentLastValue = (decimal?)typeof(tb_PurEntryDetail).GetProperty(detailPropertyName)?.GetValue(lastDetail) ?? 0m;
                    var adjustment = masterValue - (finalTotal - currentLastValue);
                    var adjustedValue = adjustment.ToRoundDecimalPlaces(precision);
                    lastDetail.SetPropertyValue(detailPropertyName, adjustedValue);
                }
            }

            // 更新网格显示
            _gridHelper.UpdateGridColumn<tb_PurEntryDetail>(detailPropertyName);
            _gridHelper.UpdateGridColumn<tb_PurEntryDetail>(c => c.FreightAllocationRules);
        }



        protected override void HandleDetailChange(tb_PurEntry master, tb_PurEntryDetail detail)
        {
            SafeExecute(() =>
            {
                detail.SubtotalAmount = (detail.UnitPrice + detail.CustomizedCost) * detail.Quantity;

                decimal tolerance = _authController.GetAmountCalculationTolerance();
                decimal freightSum = Details.Sum(d => d.AllocatedFreightCost);
                if (Math.Abs(Master.ShipCost - freightSum) > tolerance)
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
