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


    [MenuAttrAssemblyInfo("币别资料编辑", true, UIType.单表数据)]
    public partial class UCCurrencyEdit : BaseEditGeneric<tb_Currency>
    {
        public UCCurrencyEdit()
        {
            InitializeComponent();
        }






        public override void BindData(BaseEntity entity)
        {
            tb_Currency currency = entity as tb_Currency;
            if (currency.Currency_ID == 0)
            {
                currency.Country = string.Empty;
            }

            DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.Country, txtCountry, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.CurrencyCode, txtCurrencyCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.CurrencyName, txtCurrencyName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.CurrencySymbol, txtCurrencySymbol, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_Currency>(entity, t => t.Is_enabled, chkIs_enabled, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_Currency>(entity, t => t.Is_available, chkIs_available, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_Currency>(entity, t => t.Is_BaseCurrency, chkIs_BaseCurrency, false);
            base.BindData(entity);
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
