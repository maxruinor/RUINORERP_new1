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

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 最新的基本资料 单表的处理 2023-10-12
    /// </summary>

    [MenuAttrAssemblyInfo("付款方式数据编辑", true, UIType.单表数据)]
    public partial class UCPaymentMethodEdit : BaseEditGeneric<tb_PaymentMethod>
    {
        public UCPaymentMethodEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_PaymentMethod>(entity, t => t.Paytype_Name, txtPaytype_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PaymentMethod>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_PaymentMethod>(entity, t => t.Cash, chkCash, false);
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
    }
}
