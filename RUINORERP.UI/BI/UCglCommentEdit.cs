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
using RUINORERP.Global.EnumExt.CRM;
using System.Numerics;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("批注编辑", true, UIType.单表数据)]
    public partial class UCglCommentEdit : BaseEditGeneric<tb_gl_Comment>
    {
        public UCglCommentEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }


        private tb_gl_Comment _EditEntity;
        public tb_gl_Comment EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            EditEntity = entity as tb_gl_Comment;
            if (EditEntity.CommentID == 0)
            {
                EditEntity.Employee_ID= MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                EditEntity.Created_at = DateTime.Now;
                EditEntity.Created_by= MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            }
            DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.CommentContent, txtCommentContent, BindDataType4TextBox.Text, false);
            base.errorProviderForAllInput.DataSource = entity;
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
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
