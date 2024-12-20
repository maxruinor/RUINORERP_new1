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

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("货架编辑", true, UIType.单表数据)]
    public partial class UCStorageRackEdit : BaseEditGeneric<tb_StorageRack>
    {
        public UCStorageRackEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.RackNO, txtRackNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.RackName, txtRackName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.RackLocation, txtRackLocation, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, false);
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_StorageRackValidator>() , kryptonPanel1.Controls);
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
