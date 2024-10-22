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
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("账号编辑", true, UIType.单表数据)]
    public partial class UCFMAccountEdit : BaseEditGeneric<tb_FM_Account>
    {
        public UCFMAccountEdit()
        {
            InitializeComponent();
        }


        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4Cmb<tb_FM_Subject>(entity, k => k.subject_id, v => v.subject_name, cmbsubject_id);

            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyCode, cmbCurrency_ID);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_name, txtaccount_name, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_No, txtaccount_No, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CmbByEnum<tb_FM_Account>(entity, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Bank, txtBank, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.OpeningBalance.ToString(), txtOpeningBalance, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.CurrentBalance.ToString(), txtCurrentBalance, BindDataType4TextBox.Money, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_FM_AccountValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
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



    }
}
