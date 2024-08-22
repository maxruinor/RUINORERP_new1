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
            if (_EditEntity.Audit_ID == 0)
            {
                dtpDate.Checked = true;
                _EditEntity.ActionTime = System.DateTime.Now;
            }
             
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();


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
