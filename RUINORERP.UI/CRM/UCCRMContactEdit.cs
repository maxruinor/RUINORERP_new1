﻿using System;
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


    [MenuAttrAssemblyInfo("联系人编辑", true, UIType.单表数据)]
    public partial class UCCRMContactEdit : BaseEditGeneric<tb_CRM_Contact>
    {
        public UCCRMContactEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRM_Contact _EditEntity;
        public tb_CRM_Contact EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity,ActionStatus actionStatus = ActionStatus.无操作)
        {
             
            _EditEntity = entity as tb_CRM_Contact;
            DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v => v.CustomerName, cmbCustomer_id);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.SocialTools, txtSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Contact_Name, txtContact_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Contact_Email, txtContact_Email, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Contact_Phone, txtContact_Phone, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Position, txtPosition, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Preferences, txtPreferences, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

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
