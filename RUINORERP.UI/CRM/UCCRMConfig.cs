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
using FluentValidation.Results;
using FluentValidation;
using NPOI.SS.Formula.Functions;

namespace RUINORERP.UI.CRM
{


    [MenuAttrAssemblyInfo("参数配置", true, UIType.单表数据)]
    public partial class UCCRMConfig : BaseUControl
    {
        public UCCRMConfig()
        {
            InitializeComponent();
        }
        private tb_CRMConfig _EditEntity;
        public tb_CRMConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRMConfig entity)
        {
            _EditEntity = entity;
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
            if (entity.CRMConfigID == 0)
            {
                BusinessHelper.Instance.InitEntity(entity);
            }
            else
            {
                BusinessHelper.Instance.EditEntity(entity);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // bindingSourceEdit.CancelEdit();
            Exit(this);
        }


        private async void btnOk_Click(object sender, EventArgs e)
        {
            tb_CRMConfigValidator validator = new tb_CRMConfigValidator();
            ValidationResult results = validator.Validate(EditEntity);
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                tb_CRMConfigController<tb_CRMConfig> ctr = Startup.GetFromFac<tb_CRMConfigController<tb_CRMConfig>>();
                ReturnResults<tb_CRMConfig> rrs = await ctr.BaseSaveOrUpdate(EditEntity);
                if (rrs.Succeeded)
                {
                    Exit(this);
                }
                else
                {
                    MessageBox.Show("保存出错。");
                }
                //保存
                //a MainForm.Instance.AppContext.Db.Insertable<tb_CRMConfig>(EditEntity).ExecuteReturnSnowflakeId();
            }

        }

        private async void UCLeadsEdit_Load(object sender, EventArgs e)
        {
            tb_CRMConfig CRMConfig = await MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().FirstAsync();
            if (CRMConfig == null)
            {
                CRMConfig = new tb_CRMConfig();
            }
            EditEntity = CRMConfig;
            BindData(EditEntity);
        }


    }
}
