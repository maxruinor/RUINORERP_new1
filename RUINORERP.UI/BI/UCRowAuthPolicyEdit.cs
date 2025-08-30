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
using RUINORERP.Business.RowLevelAuthService;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("数据权限规则", true, UIType.单表数据)]
    public partial class UCRowAuthPolicyEdit : BaseEditGeneric<tb_RowAuthPolicy>
    {
        private IDefaultRowAuthRuleProvider _ruleProvider;

        public UCRowAuthPolicyEdit()
        {
            InitializeComponent();
            _ruleProvider = Startup.GetFromFac<IDefaultRowAuthRuleProvider>();
        }

        public override void BindData(BaseEntity entity)
        {
            tb_RowAuthPolicy _EditEntity = entity as tb_RowAuthPolicy;
            
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyName, txtPolicyName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetTable, txtTargetTable, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetEntity, txtTargetEntity, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsJoinRequired, chkIsJoinRequired, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinTable, txtJoinTable, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinType, txtJoinType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinOnClause, txtJoinOnClause, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.FilterClause, txtFilterClause, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.EntityType, txtEntityType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsEnabled, chkIsEnabled, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyDescription, txtPolicyDescription, BindDataType4TextBox.Text, false);
            
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