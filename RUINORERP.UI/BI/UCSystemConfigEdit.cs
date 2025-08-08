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

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("系统参数编辑", true, UIType.单表数据)]
    public partial class UCSystemConfigEdit : BaseEditGeneric<tb_SystemConfig>
    {
        public UCSystemConfigEdit()
        {
            InitializeComponent();
            DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SystemConfig>(typeof(Global.库存成本计算方式), e => e.CostCalculationMethod, cmbCostCalculationMethod, false);
        }

        private tb_SystemConfig SystemConfig = new tb_SystemConfig();


        public override void BindData(BaseEntity entity)
        {
            tb_SystemConfig SystemConfig = entity as tb_SystemConfig;
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

            #region FMconfig
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

            #endregion

            base.BindData(entity);
        }


        FMConfiguration fMConfiguration = new FMConfiguration();


        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {

                //这里不能用上面的SystemConfig，丢失？ 引用没传到值？
                // 更新JSON数据
                var jsonData = JsonConvert.SerializeObject(fMConfiguration);
                if (bindingSourceEdit.Current is tb_SystemConfig sysconfig)
                {
                    sysconfig.FMConfig = jsonData;
                }
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UCSystemConfigEdit_Load(object sender, EventArgs e)
        {

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
