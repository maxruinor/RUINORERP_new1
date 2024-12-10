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
using RUINORERP.UI.SysConfig;
using System.Diagnostics;
using Org.BouncyCastle.Crypto.Macs;

namespace RUINORERP.UI.CRM
{


    [MenuAttrAssemblyInfo("客户关系参数配置", true, UIType.单表数据)]
    public partial class UCCRMConfig : BaseEditGeneric<tb_CRMConfig>
    {
        public UCCRMConfig()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRMConfig _EditEntity;
        public tb_CRMConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

            _EditEntity = entity as tb_CRMConfig;

            DataBindingHelper.BindData4CheckBox<tb_CRMConfig>(entity, t => t.CS_UseLeadsFunction, chkCS_UseLeadsFunction, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_NewCustToLeadsCustDays, txtCS_NewCustToLeadsCustDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_SleepingCustomerDays, txtCS_SleepingCustomerDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_LostCustomersDays, txtCS_LostCustomersDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_ActiveCustomers, txtCS_ActiveCustomers, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.LS_ConvCustHasFollowUpDays, txtLS_ConvCustHasFollowUpDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.LS_ConvCustNoTransDays, txtLS_ConvCustNoTransDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.LS_ConvCustLostDays, txtLS_ConvCustLostDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.NoFollToPublicPoolDays, txtNoFollToPublicPoolDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CustomerNoOrderDays, txtCustomerNoOrderDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CustomerNoFollowUpDays, txtCustomerNoFollowUpDays, BindDataType4TextBox.Qty, false);

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

        private void UCLeadsEdit_Load(object sender, EventArgs e)
        {

        }


    }
}
