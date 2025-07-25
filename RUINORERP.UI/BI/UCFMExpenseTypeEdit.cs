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
using FastReport;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("费用类型编辑", true, UIType.单表数据)]
    public partial class UCFMExpenseTypeEdit : BaseEditGeneric<tb_FM_ExpenseType>
    {
        public UCFMExpenseTypeEdit()
        {
            InitializeComponent();
        }


        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4Cmb<tb_FM_Subject>(entity, k => k.Subject_id, v => v.Subject_name, cmbsubject_id);

            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseType>(entity, t => t.Expense_name, txtExpense_name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_ExpenseType, ReceivePaymentType>(entity, k => k.ReceivePaymentType, cmbReceivePaymentType, false);
            //有默认值

            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseType>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_ExpenseTypeValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
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
