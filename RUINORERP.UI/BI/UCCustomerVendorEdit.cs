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

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("往来单位编辑", true, UIType.单表数据)]
    public partial class UCCustomerVendorEdit : BaseEditGeneric<tb_CustomerVendor>
    {
        public UCCustomerVendorEdit()
        {
            InitializeComponent();
            chkOther.ToolTipValues.Description = "其他类型，如物流公司，第三方仓库，加工厂等合作伙伴等。";
        }

        private tb_CustomerVendor _EditEntity;
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_CustomerVendor;

            if (_EditEntity.CustomerVendor_ID == 0)
            {
               
                if (Text.Contains("其他"))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.CVOther);
                    //txtIsCustomer.Enabled = false;
                    //txtIsVendor.Enabled = false;
                    chkOther.Enabled = true;
                }
                if (Text.Contains("客户"))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Customer);
                    _EditEntity.IsCustomer = true;
                }
                if (Text.Contains("供应商"))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Supplier);
                    _EditEntity.IsVendor = true;
                }
            }
            
            DataBindingHelper.BindData4Cmb<tb_CustomerVendorType>(entity, k => k.Type_ID, v => v.TypeName, txtType_ID);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CVName, txtCVName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Contact, txtContact, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CVCode, txtCVCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.ShortName, txtShortName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsCustomer, txtIsCustomer, false);
            DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsExclusive, chk责任人专属, false);
            DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsVendor, txtIsVendor, false);
            DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsOther, chkOther, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_CustomerVendor>(entity, t => t.Is_enabled, rdbis_enabledYes, rdbis_enabledNo);
            //有默认值
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_CustomerVendor>(entity, t => t.Is_available, rdbis_availableYes, rdbis_availableNo);
            //有默认值

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_CustomerVendorValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
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
