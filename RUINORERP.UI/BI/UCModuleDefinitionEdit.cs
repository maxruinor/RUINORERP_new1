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


    [MenuAttrAssemblyInfo("模块定义编辑", true, UIType.单表数据)]
    public partial class UCModuleDefinitionEdit : BaseEditGeneric<tb_ModuleDefinition>
    {
        public UCModuleDefinitionEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_ModuleDefinition>(entity, t => t.ModuleNo, txtModuleNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ModuleDefinition>(entity, t => t.ModuleName, txtModuleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_ModuleDefinition>(entity, t => t.Visible, chkVisible, false);
            DataBindingHelper.BindData4CheckBox<tb_ModuleDefinition>(entity, t => t.Available, chkAvailable, false);
            DataBindingHelper.BindData4TextBox<tb_ModuleDefinition>(entity, t => t.IconFile_Path, txtIconFile_Path, BindDataType4TextBox.Text, false);
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
