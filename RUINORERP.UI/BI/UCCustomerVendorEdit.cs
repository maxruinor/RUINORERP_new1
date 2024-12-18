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
using RUINORERP.Global;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("往来单位编辑", true, UIType.单表数据)]
    public partial class UCCustomerVendorEdit : BaseEditGeneric<tb_CustomerVendor>
    {
        public UCCustomerVendorEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
            chkOther.ToolTipValues.Description = "其他类型，如物流公司，第三方仓库，加工厂等合作伙伴等。";
        }
        tb_ModuleDefinition crmMod = null;
        private tb_CustomerVendor _EditEntity;
        public async override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
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
                //新建时默认启用
                _EditEntity.Is_available = true;
                _EditEntity.Is_enabled = true;


            }

            DataBindingHelper.BindData4Cmb<tb_CustomerVendorType>(entity, k => k.Type_ID, v => v.TypeName, txtType_ID);

            DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v => v.CustomerName, cmbCustomer_id);

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
                //如果在模块定义中客户关系是启用时，就必须录入来源的目标客户。
                crmMod = await MainForm.Instance.AppContext.Db.Queryable<tb_ModuleDefinition>().Where(c => c.ModuleName == nameof(ModuleMenuDefine.客户关系)).FirstAsync();
                if (crmMod.Available)
                {
                    lblCustomer_id.Visible = true;
                    cmbCustomer_id.Visible = true;
                }
                else
                {
                    lblCustomer_id.Visible = false;
                    cmbCustomer_id.Visible = false;
                }

                base.InitRequiredToControl(new tb_CustomerVendorValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            if (_EditEntity.CustomerVendor_ID > 0)
            {
                btnAddPayeeInfo.Visible = true;
            }
            else
            {
                btnAddPayeeInfo.Visible = false;
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
            if (crmMod.Available && (!_EditEntity.Customer_id.HasValue || _EditEntity.Customer_id.Value <= 0))
            {
                //客户关系模块启用时，销售客户的来源必须选择。
                MessageBox.Show("销售客户的来源必须选择。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnAddPayeeInfo_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCFMPayeeInfoEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_FM_PayeeInfo> frmaddg = frm as BaseEditGeneric<tb_FM_PayeeInfo>;
                frmaddg.Text = "收款账号编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_FM_PayeeInfo>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_FM_PayeeInfo payeeInfo = obj as tb_FM_PayeeInfo;
                payeeInfo.CustomerVendor_ID = _EditEntity.CustomerVendor_ID;
                BaseEntity bty = payeeInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    UIBizSrvice.SavePayeeInfo(payeeInfo);
                }
            }
        }

        private void UCCustomerVendorEdit_Load(object sender, EventArgs e)
        {
            if (Text.Contains("客户"))
            {
                lblCustomer_id.Visible = true;
                cmbCustomer_id.Visible = true;
                txtIsCustomer.Checked = true;
            }
            else
            {
                lblCustomer_id.Visible = false;
                cmbCustomer_id.Visible = false;
                if (Text.Contains("供应商"))
                {
                    txtIsVendor.Checked = true;
                }
            }
        }

        private void cmbCustomer_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            //将目标客户信息带过来。可以用automapper
        }
    }
}
