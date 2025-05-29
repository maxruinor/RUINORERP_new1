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
using RUINORERP.UI.Monitoring.Auditing;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("审计日志查看", true, UIType.单表数据)]
    public partial class UCAuditLogsEdit : BaseEditGeneric<tb_AuditLogs>
    {
        public UCAuditLogsEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            tb_AuditLogs _EditEntity = entity as tb_AuditLogs;
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.UserName, txtUserName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ActionTime, txtActionTime, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ActionType, txtActionType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ObjectType, txtObjectType, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ObjectId, txtObjectId, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ObjectNo, txtObjectNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.OldState, txtOldState, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.NewState, txtNewState, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.DataContent, txtDataContent, BindDataType4TextBox.Text, false);
            if (entity != null && !string.IsNullOrEmpty(_EditEntity.DataContent))
            {
              auditLogViewer1.LoadAuditData(_EditEntity.DataContent);
            }
        }

    
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                //bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }



    }
}
