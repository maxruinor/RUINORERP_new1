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


    [MenuAttrAssemblyInfo("异常日志查看", true, UIType.单表数据)]
    public partial class UCLogsEdit : BaseEditGeneric<Logs>
    {
        public UCLogsEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            Logs _EditEntity = entity as Logs;
            if (_EditEntity.ID == 0)
            {
                dtpDate.Checked = true;
                _EditEntity.Date = System.DateTime.Now;
            }
            DataBindingHelper.BindData4DataTime<Logs>(entity, t => t.Date, dtpDate, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Level, txtLevel, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Logger, txtLogger, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Message, txtMessage, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Exception, txtException, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Operator, txtOperator, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.ModName, txtModName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Path, txtPath, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.ActionName, txtActionName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.IP, txtIP, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.MAC, txtMAC, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.MachineName, txtMachineName, BindDataType4TextBox.Text, false);
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
