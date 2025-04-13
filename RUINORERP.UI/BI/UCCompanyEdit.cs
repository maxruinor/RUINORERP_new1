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

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("公司信息编辑", true, UIType.单表数据)]
    public partial class UCCompanyEdit : BaseEditGeneric<tb_Company>
    {
        public UCCompanyEdit()
        {
            InitializeComponent();
        }

        private tb_Company _EditEntity;
        public tb_Company EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_Company;
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.CompanyCode, txtCompanyCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.ENName, txtENName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.ShortName, txtShortName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.LegalPersonName, txtLegalPersonName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.UnifiedSocialCreditIdentifier, txtUnifiedSocialCreditIdentifier, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Contact, txtContact, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.ENAddress, txtENAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Email, txtEmail, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceTitle, txtInvoiceTitle, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceTaxNumber, txtInvoiceTaxNumber, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceAddress, txtInvoiceAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceTEL, txtInvoiceTEL, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceBankAccount, txtInvoiceBankAccount, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceBankName, txtInvoiceBankName, BindDataType4TextBox.Text, false);

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_CompanyValidator>()  , kryptonPanel1.Controls);
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
