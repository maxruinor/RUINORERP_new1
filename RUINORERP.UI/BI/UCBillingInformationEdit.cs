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
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("开票资料编辑", true, UIType.单表数据)]
    public partial class UCBillingInformationEdit : BaseEditGeneric<tb_BillingInformation>
    {
        public UCBillingInformationEdit()
        {
            InitializeComponent();
        }

        private tb_BillingInformation _EditEntity;
        public tb_BillingInformation EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_BillingInformation;
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Title, txtTitle, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.TaxNumber, txtTaxNumber, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.PITEL, txtPITEL, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.BankAccount, txtBankAccount, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.BankName, txtBankName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Email, txtEmail, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_BillingInformation>(entity, t => t.IsActive, chkIsActive, false);
            //创建表达式
            var lambdaSupplier = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                            .AndIF(AuthorizeController.GetExclusiveLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterSupplier = baseProcessor.GetQueryFilter();

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                queryFilterSupplier.FilterLimitExpressions.Add(lambdaSupplier);
                DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterSupplier.GetFilterExpression<tb_CustomerVendor>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterSupplier);
            }
            else
            {
                DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterSupplier.GetFilterExpression<tb_CustomerVendor>(), true);
            }
            entity.PropertyChanged += (sender, s2) =>
            {
                if (_EditEntity == null)
                {
                    return;
                }
                if (_EditEntity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_BillingInformation>(c => c.CustomerVendor_ID))
                {
                    if (cmbCustomerVendor_ID.SelectedItem is tb_CustomerVendor cv)
                    {
                        if (cv != null)
                        {
                            _EditEntity.Title = cv.CVName;
                            _EditEntity.Address = cv.Address;
                            _EditEntity.PITEL = cv.Phone;
                        }
                    }
                }
            };

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_BillingInformationValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);

                if (_EditEntity.CustomerVendor_ID > 0)
                {
                    if (cmbCustomerVendor_ID.SelectedItem is tb_CustomerVendor cv)
                    {
                        if (cv != null)
                        {
                            _EditEntity.Title = cv.CVName;
                            _EditEntity.Address = cv.Address;
                            _EditEntity.PITEL = cv.Phone;
                        }
                    }
                }

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
