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
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("项目组编辑", true, UIType.单表数据)]
    public partial class UCProjectGroupEdit: BaseEditGeneric<tb_ProjectGroup>
    {
        public UCProjectGroupEdit()
        {
            InitializeComponent();
        }


        private tb_ProjectGroup _EditEntity;

        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_ProjectGroup;
            if (_EditEntity.ProjectGroup_ID == 0)
            {
                _EditEntity.ProjectGroupCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.ProjectGroupCode);
            }
            DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.ProjectGroupCode, txtProjectGroupCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.ProjectGroupName, txtProjectGroupName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.ResponsiblePerson, txtResponsiblePerson, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_ProjectGroup>(entity, t => t.StartDate, dtpStartDate, false);
            DataBindingHelper.BindData4CheckBox<tb_ProjectGroup>(entity, t => t.Is_enabled, chkIs_enabled, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartment);

            //有默认值
            DataBindingHelper.BindData4DataTime<tb_ProjectGroup>(entity, t => t.EndDate, dtpEndDate, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_ProjectGroupValidator(), kryptonPanel1.Controls);
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
