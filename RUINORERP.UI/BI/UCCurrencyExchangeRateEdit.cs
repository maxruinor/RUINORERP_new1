using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("币别换算编辑", true, UIType.单表数据)]
    public partial class UCCurrencyExchangeRateEdit : BaseEditGeneric<tb_CurrencyExchangeRate>
    {
        public UCCurrencyExchangeRateEdit()
        {
            InitializeComponent();
        }






        public override void BindData(BaseEntity entity)
        {
            tb_CurrencyExchangeRate currency = entity as tb_CurrencyExchangeRate;
            if (currency.ExchangeRateID == 0)
            {
                currency.EffectiveDate= System.DateTime.Now; 
                //currency.ExpirationDate = System.DateTime.Now;
            }
            DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(currency, t => t.ConversionName, txtConversionName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency, tb_CurrencyExchangeRate>(currency, k => k.Currency_ID, v => v.CurrencyName, t => t.BaseCurrencyID, cmbBaseCurrencyID, false);
            DataBindingHelper.BindData4Cmb<tb_Currency, tb_CurrencyExchangeRate>(currency, k => k.Currency_ID, v => v.CurrencyName, t => t.TargetCurrencyID, cmbTargetCurrencyID, false);
            DataBindingHelper.BindData4DataTime<tb_CurrencyExchangeRate>(currency, t => t.EffectiveDate, dtpEffectiveDate, false);
            DataBindingHelper.BindData4DataTime<tb_CurrencyExchangeRate>(currency, t => t.ExpirationDate, dtpExpirationDate, false);
            DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(currency, t => t.DefaultExchRate.ToString(), txtDefaultExchRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(currency, t => t.ExecuteExchRate.ToString(), txtExecuteExchRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_CurrencyExchangeRate>(currency, t => t.Is_enabled, chkIs_enabled, false);
            DataBindingHelper.BindData4CheckBox<tb_CurrencyExchangeRate>(currency, t => t.Is_available, chkIs_available, false);
            base.BindData(currency);
        }


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

        private void UCCurrencyEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
