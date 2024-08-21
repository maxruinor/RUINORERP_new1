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


    [MenuAttrAssemblyInfo("按钮信息编辑", true, UIType.单表数据)]
    public partial class UCButtonInfoEdit : BaseEditGeneric<tb_ButtonInfo>
    {
        public UCButtonInfoEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.BtnName, txtBtnName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.BtnText, txtBtnText, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.FormName, txtFormName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_ButtonInfo>(entity, t => t.IsForm, chkIsForm, false);
            DataBindingHelper.BindData4CheckBox<tb_ButtonInfo>(entity, t => t.IsEnabled, chkIsEnabled, false);
            DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            base.errorProviderForAllInput.DataSource = entity;
        }

        //public override void BindData(tb_ButtonInfo entity)
        //{
        //    DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.BtnName, txtBtnName, BindDataType4TextBox.Text, false);
        //    DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.BtnText, txtBtnText, BindDataType4TextBox.Text, false);
        //    DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.FormName, txtFormName, BindDataType4TextBox.Text, false);
        //    DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text, false);
        //    DataBindingHelper.BindData4CehckBox<tb_ButtonInfo>(entity, t => t.IsForm, chkIsForm, false);
        //    DataBindingHelper.BindData4CehckBox<tb_ButtonInfo>(entity, t => t.IsEnabled, chkIsEnabled, false);
        //    DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
        //    base.errorProviderForAllInput.DataSource = entity;
        //}


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
