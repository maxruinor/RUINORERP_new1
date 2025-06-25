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
using RUINORERP.Business;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("单据编号规则编辑", true, UIType.单表数据)]
    public partial class UCsysBillNoRuleEdit : BaseEditGeneric<tb_sys_BillNoRule>
    {
        public UCsysBillNoRuleEdit()
        {
            InitializeComponent();
            //DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_sys_BillNoRule>(typeof(Global.库存成本计算方式), e => e.CostCalculationMethod, cmbCostCalculationMethod, false);
        }

        private tb_sys_BillNoRule _EditEntity;
        public tb_sys_BillNoRule EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_sys_BillNoRule entity)
        {
            _EditEntity = entity;
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.BizType, typeof(BizType), cmbBizType, false);

            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.DateFormat, typeof(DateFormat), cmbDateFormat, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.ResetMode, typeof(ResetMode), cmbResetMode, false);

            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Prefix, txtPrefix, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.SequenceLength, txtSequenceLength, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.UseCheckDigit, chkUseCheckDigit, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RedisKeyPattern, txtRedisKeyPattern, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.IsActive, chkIsActive, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            base.BindData(entity);
        }


        /*
        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.CostCalculationMethod, typeof(库存成本计算方式), cmbCostCalculationMethod, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.FreightAllocationRules, typeof(FreightAllocationRules), cmbFreightAllocationRules, false);

            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.QtyDataPrecision, txtQtyDataPrecision, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.TaxRateDataPrecision, txtTaxRateDataPrecision, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.MoneyDataPrecision, txtMoneyDataPrecision, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.CurrencyDataPrecisionAutoAddZero, chkAutoAddZero, false);

            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.CheckNegativeInventory, chkCheckNegativeInventory, false);

            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.OwnershipControl, chkOwnershipControl, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.ShowDebugInfo, chkShowDebugInfo, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.IsDebug, chkIsDebug, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.SaleBizLimited, chkSaleBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.UseBarCode, chkUseBarCode, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.DepartBizLimited, chkDepartBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.PurchsaeBizLimited, chkPurchsaeBizLimited, false);

            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.AutoApprovedPurOrderAmount, txtAutoApprovedPurOrderAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.AutoApprovedSaleOrderAmount, txtAutoApprovedSaleOrderAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.QueryPageLayoutCustomize, chkQueryPageLayoutCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.QueryGridColCustomize, chkQueryGridColCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.BillGridColCustomize, chkBillGridColCustomize, false);

            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.EnableVoucherModule, chkEnableVoucherModule, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.EnableContractModule, chkEnableContractModule, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.EnableInvoiceModule, chkEnableInvoiceModule, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.EnableMultiCurrency, chkEnableMultiCurrency, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.EnableFinancialModule, chkEnableFinancialModule, false);

            base.BindData(entity);
        }
        */


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
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UCSystemConfigEdit_Load(object sender, EventArgs e)
        {

        }

       
    }
}
