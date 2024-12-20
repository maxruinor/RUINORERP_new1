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
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("部门编辑", true, UIType.单表数据)]
    public partial class UCDepartmentEdit : BaseEditGeneric<tb_Department>
    {
        public UCDepartmentEdit()
        {
            InitializeComponent();
        }


        public override void BindData(BaseEntity entity)
        {
            tb_Department _EditEntity = entity as tb_Department;
            if (_EditEntity.DepartmentID == 0)
            {
                _EditEntity.DepartmentCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Department);
            }
            DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.DepartmentName, txtDepartmentName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
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
