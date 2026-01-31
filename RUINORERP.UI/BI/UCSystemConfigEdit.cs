using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using RUINORERP.Model.ConfigModel;
using LiveChartsCore.Geo;
using Newtonsoft.Json;
using RUINORERP.UI.BusinessService.SmartMenuService;
using System.Collections.Generic;
using RUINORERP.Business.Validator;
using FluentValidation.Results;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("系统参数编辑", true, UIType.单表数据)]
    public partial class UCSystemConfigEdit : BaseEditGeneric<tb_SystemConfig>
    {
        public UCSystemConfigEdit()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SystemConfig>(typeof(Global.库存成本计算方式), e => e.CostCalculationMethod, cmbCostCalculationMethod, false);
            }
            
        }

        private tb_SystemConfig SystemConfig = new tb_SystemConfig();


        public override void BindData(BaseEntity entity)
        {
            tb_SystemConfig SystemConfig = entity as tb_SystemConfig;

            if (SystemConfig != null && SystemConfig.ID == 0)
            {
                SystemConfig.IMConfig = string.Empty;
                SystemConfig.FMConfig = string.Empty;
                SystemConfig.FunctionConfiguration = string.Empty;
            }

            DataBindingHelper.BindData4CmbByEnum<tb_SystemConfig>(SystemConfig, k => k.CostCalculationMethod, typeof(库存成本计算方式), cmbCostCalculationMethod, false);
            DataBindingHelper.BindData4CmbByEnum<tb_SystemConfig>(SystemConfig, k => k.FreightAllocationRules, typeof(FreightAllocationRules), cmbFreightAllocationRules, false);

            DataBindingHelper.BindData4TextBox<tb_SystemConfig>(SystemConfig, t => t.QtyDataPrecision, txtQtyDataPrecision, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_SystemConfig>(SystemConfig, t => t.TaxRateDataPrecision, txtTaxRateDataPrecision, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_SystemConfig>(SystemConfig, t => t.MoneyDataPrecision, txtMoneyDataPrecision, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.CurrencyDataPrecisionAutoAddZero, chkAutoAddZero, false);

            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.CheckNegativeInventory, chkCheckNegativeInventory, false);

            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.OwnershipControl, chkOwnershipControl, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.ShowDebugInfo, chkShowDebugInfo, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.IsDebug, chkIsDebug, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.SaleBizLimited, chkSaleBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.UseBarCode, chkUseBarCode, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.DepartBizLimited, chkDepartBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.PurchsaeBizLimited, chkPurchsaeBizLimited, false);

            DataBindingHelper.BindData4TextBox<tb_SystemConfig>(SystemConfig, t => t.AutoApprovedPurOrderAmount, txtAutoApprovedPurOrderAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SystemConfig>(SystemConfig, t => t.AutoApprovedSaleOrderAmount, txtAutoApprovedSaleOrderAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.QueryPageLayoutCustomize, chkQueryPageLayoutCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.QueryGridColCustomize, chkQueryGridColCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.BillGridColCustomize, chkBillGridColCustomize, false);

            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.EnableVoucherModule, chkEnableVoucherModule, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.EnableContractModule, chkEnableContractModule, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.EnableInvoiceModule, chkEnableInvoiceModule, false);
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.EnableMultiCurrency, chkEnableMultiCurrency, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(SystemConfig, t => t.EnableFinancialModule, chkEnableFinancialModule, false);

            #region 财务模块配置
            try
            {
                fMConfiguration = JsonConvert.DeserializeObject<FMConfiguration>(SystemConfig.FMConfig);
            }
            catch (Exception)
            {

            }

            if (fMConfiguration == null)
            {
                fMConfiguration = new FMConfiguration();
            }

            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.EnableARAutoOffsetPreReceive, chkEnableARAutoOffsetPreReceive, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.EnableAPAutoOffsetPrepay, chkEnableAPAutoOffsetPrepay, false);

            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.EnablePaymentAutoOffsetAP, chkEnablePaymentAutoOffsetAP, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.EnablePaymentAutoOffsetAR, chkEnablePaymentAutoOffsetAR, false);

            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.AutoAuditPreReceive, chkAutoAuditPreReceive, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.AutoAuditPrePayment, chkAutoAuditPrePayment, false);


            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.AutoAuditReceivePaymentRecordByPlatform, chkAutoAuditReceivePaymentRecordByPlatform, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.AutoAuditReceiveable, chkAutoAuditReceiveable, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.AutoAuditPaymentable, chkAutoAuditPaymentable, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.EnableAutoRefundOnOrderCancel, chkEnableAutoRefundOnOrderCancel, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.AutoAuditExpensePaymentRecord, chkAutoAuditExpensePaymentRecord, false);
            DataBindingHelper.BindData4CheckBox<FMConfiguration>(fMConfiguration, t => t.EnableAutoAuditSalesOutboundForFullPrepaymentOrders, chkEnableAutoAuditSalesOutboundForFullPrepaymentOrders, false);

            // 金额计算容差阈值数据绑定
            DataBindingHelper.BindData4TextBox<FMConfiguration>(fMConfiguration, t => t.AmountCalculationTolerance, txtAmountCalculationTolerance, BindDataType4TextBox.Money, false);
            #endregion

            #region 系统功能配置

            try
            {
                functionConfiguration = JsonConvert.DeserializeObject<FunctionConfiguration>(SystemConfig.FunctionConfiguration);
            }
            catch (Exception)
            {

            }

            if (functionConfiguration == null)
            {
                functionConfiguration = new FunctionConfiguration();
            }

            DataBindingHelper.BindData4CheckBox<FunctionConfiguration>(functionConfiguration, t => t.EnableRowLevelAuth, chkEnableRowLevelAuth, false);

            #endregion

            base.BindData(entity);
        }


        FMConfiguration fMConfiguration = new FMConfiguration();

        FunctionConfiguration functionConfiguration = new FunctionConfiguration();

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            // 使用 FluentValidation 验证 FMConfiguration
            var fmValidator = new FMConfigurationValidator();
            ValidationResult fmValidationResult = fmValidator.Validate(fMConfiguration);
            if (!fmValidationResult.IsValid)
            {
                string errorMessage = string.Join("\n", fmValidationResult.Errors.Select(e => e.ErrorMessage));
                MessageBox.Show(
                    "财务配置验证失败：\n" + errorMessage,
                    "配置验证",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 使用 FluentValidation 验证 FunctionConfiguration
            var functionValidator = new FunctionConfigurationValidator();
            ValidationResult functionValidationResult = functionValidator.Validate(functionConfiguration);
            if (!functionValidationResult.IsValid)
            {
                string errorMessage = string.Join("\n", functionValidationResult.Errors.Select(e => e.ErrorMessage));
                MessageBox.Show(
                    "功能配置验证失败：\n" + errorMessage,
                    "配置验证",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 额外验证：金额计算容差阈值建议值与金额精度保持一致
            // 默认值 = 10^(-金额精度)，但允许设置更大的值（不能超过1）
            int moneyPrecision = 2; // 默认2位
            if (bindingSourceEdit.Current is tb_SystemConfig config && config.MoneyDataPrecision >= 0)
            {
                moneyPrecision = config.MoneyDataPrecision;
            }

            // 计算推荐的容差阈值（基于金额精度）
            // 3位精度 -> 0.001
            // 2位精度 -> 0.01
            // 4位精度 -> 0.0001
            decimal recommendedTolerance = (decimal)Math.Pow(10, -moneyPrecision);

            // 验证容差阈值不能超过1
            if (fMConfiguration.AmountCalculationTolerance >= 1m)
            {
                MessageBox.Show(
                    $"金额计算容差阈值必须小于1。\n" +
                    $"当前值为 {fMConfiguration.AmountCalculationTolerance}。",
                    "输入验证",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 如果当前值不是推荐值，给出提示但允许保存
            if (fMConfiguration.AmountCalculationTolerance != recommendedTolerance)
            {
                var result = MessageBox.Show(
                    $"当前金额精度为 {moneyPrecision} 位小数。\n" +
                    $"推荐容差阈值为 {recommendedTolerance}。\n" +
                    $"当前值为 {fMConfiguration.AmountCalculationTolerance}。\n\n" +
                    $"是否自动调整为推荐值 {recommendedTolerance}？\n\n" +
                    $"点击[是]使用推荐值\n" +
                    $"点击[否]保留当前值继续保存",
                    "容差阈值建议",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    fMConfiguration.AmountCalculationTolerance = recommendedTolerance;
                    // 更新文本框显示
                    txtAmountCalculationTolerance.Text = recommendedTolerance.ToString();
                }
            }

            // 更新JSON数据
            var fmjsonData = JsonConvert.SerializeObject(fMConfiguration);
            var funjsonData = JsonConvert.SerializeObject(functionConfiguration);
            if (bindingSourceEdit.Current is tb_SystemConfig sysconfig)
            {
                sysconfig.FMConfig = fmjsonData;
                sysconfig.FunctionConfiguration = funjsonData;
            }
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void chkIsDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkIsDebug.Checked)
            {
                chkShowDebugInfo.Checked = false;
            }
            lblShowDebugInfo.Visible = chkShowDebugInfo.Visible = chkIsDebug.Checked;
        }
    }
}
