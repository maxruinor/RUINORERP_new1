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
using AutoMapper;
using Castle.Core.Resource;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;

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
            //默认不是专属客户，默认不是专属责任人。
            _EditEntity.IsExclusive = false;
            if (_EditEntity.CustomerVendor_ID == 0)
            {
                if (Text.Contains("其他"))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.CVOther);
                    //txtIsCustomer.Enabled = false;
                    //txtIsVendor.Enabled = false;
                    chkOther.Enabled = true;
                    chkNoNeedSource.Visible = false;
                }
                if (Text.Contains("客户"))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Customer);
                    _EditEntity.IsCustomer = true;
                    chkNoNeedSource.Visible = true;
                    lblCustomerCreditDays.Visible = true;
                    txtCustomerCreditDays.Visible = true;
                    lblCustomerCreditLimit.Visible = true;
                    txtCustomerCreditLimit.Visible = true;

                    lblSupplierCreditDays.Visible = false;
                    txtSupplierCreditDays.Visible = false;
                    lblSupplierCreditLimit.Visible = false;
                    txtSupplierCreditLimit.Visible = false;

                }
                if (Text.Contains("供应商"))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Supplier);
                    _EditEntity.IsVendor = true;
                    chkNoNeedSource.Visible = true;
                    lblCustomerCreditDays.Visible = false;
                    txtCustomerCreditDays.Visible = false;
                    lblCustomerCreditLimit.Visible = false;
                    txtCustomerCreditLimit.Visible = false;

                    lblSupplierCreditDays.Visible = true;
                    txtSupplierCreditDays.Visible = true;
                    lblSupplierCreditLimit.Visible = true;
                    txtSupplierCreditLimit.Visible = true;

                }
                if (string.IsNullOrEmpty(_EditEntity.CVCode))
                {
                    _EditEntity.CVCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.BusinessPartner);
                }
                //新建时默认启用
                _EditEntity.Is_enabled = true;
            }

            DataBindingHelper.BindData4Cmb<tb_CustomerVendorType>(entity, k => k.Type_ID, v => v.TypeName, txtType_ID);

            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CustomerCreditLimit.ToString(), txtCustomerCreditLimit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CustomerCreditDays, txtCustomerCreditDays, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.SupplierCreditLimit.ToString(), txtSupplierCreditLimit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.SupplierCreditDays, txtSupplierCreditDays, BindDataType4TextBox.Qty, false);

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
            //如果在模块定义中客户关系是启用时，就必须录入来源的目标客户。
            crmMod = await MainForm.Instance.AppContext.Db.Queryable<tb_ModuleDefinition>().Where(c => c.ModuleName == nameof(ModuleMenuDefine.客户关系)).FirstAsync();

            if (crmMod != null &&
                MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM)
                && crmMod.Available && _EditEntity.IsCustomer)
            {
                lblCustomer_id.Visible = true;
                cmbCustomer_id.Visible = true;
            }
            else
            {
                lblCustomer_id.Visible = false;
                cmbCustomer_id.Visible = false;
            }

            //创建表达式
            var lambda = Expressionable.Create<tb_CRM_Customer>()
                            .And(t => t.Employee_ID != null)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Customer).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);

            //带过滤的下拉绑定要这样
            DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v => v.CustomerName, cmbCustomer_id, queryFilterC.GetFilterExpression<tb_CRM_Customer>(), true);

            DataBindingHelper.InitFilterForControlByExp<tb_CRM_Customer>(entity, cmbCustomer_id, c => c.CustomerName, queryFilterC);


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {

                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_CustomerVendorValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            if (_EditEntity.CustomerVendor_ID > 0)
            {
                btnAddPayeeInfo.Visible = true;
                btnAddBillingInformation.Visible = true;
            }
            else
            {
                btnAddPayeeInfo.Visible = false;
                btnAddBillingInformation.Visible = false;
            }

            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果线索引入相关数据
                if ((_EditEntity.ActionStatus == ActionStatus.新增 || _EditEntity.ActionStatus == ActionStatus.修改) && _EditEntity.Customer_id.HasValue && _EditEntity.Customer_id.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_CustomerVendor>(c => c.Customer_id))
                {
                    await ToCustomer(_EditEntity);
                }
            };

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
            if (crmMod != null &&
         MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM)
         && crmMod.Available && (!_EditEntity.Customer_id.HasValue || _EditEntity.Customer_id.Value <= 0) && _EditEntity.IsCustomer)
            {
                if (!chkNoNeedSource.Checked)
                {
                    //客户关系模块启用时，销售客户的来源必须选择。
                    MessageBox.Show("销售客户的来源必须选择。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
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
            if (_EditEntity.CustomerVendor_ID == 0)
            {
                MessageBox.Show("请先正确选择往来单位数据。");
                return;
            }
            object frm = Activator.CreateInstance(typeof(UCFMPayeeInfoEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_FM_PayeeInfo> frmaddg = frm as BaseEditGeneric<tb_FM_PayeeInfo>;
                frmaddg.CurMenuInfo = this.CurMenuInfo;
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


        //将目标客户信息带过来。可以用automapper
        private async Task<tb_CustomerVendor> ToCustomer(tb_CustomerVendor entity)
        {
            tb_CRM_Customer crmCustomer;
            //ButtonSpecAny bsa = cmbCustomer_id.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            //if (bsa == null)
            //{
            //    return null;
            //}
            //saleorder = bsa.Tag as tb_SaleOrder;

            crmCustomer = await MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Customer>()
            .Where(c => c.Customer_id == entity.Customer_id)
            .SingleAsync();

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            mapper.Map(crmCustomer, entity);  // 直接将 crmLeads 的值映射到传入的 entity 对象上，保持了引用
                                              // entity = mapper.Map<tb_CRM_Customer>(crmLeads);//这个是直接重新生成了对象。
            entity.ActionStatus = ActionStatus.新增;

            List<string> tipsMsg = new List<string>();

            StringBuilder msg = new StringBuilder();
            foreach (var item in tipsMsg)
            {
                msg.Append(item).Append("\r\n");
            }
            if (tipsMsg.Count > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            BusinessHelper.Instance.InitEntity(entity);
            return entity;
        }

        private void txtIsCustomer_CheckedChanged(object sender, EventArgs e)
        {
            //如果CRM启用，则客户要选择来源客户。
            //如果CRM启用了
            if (crmMod != null &&
         MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM)
         && crmMod.Available)
            {
                lblCustomer_id.Visible = txtIsCustomer.Checked;
                cmbCustomer_id.Visible = txtIsCustomer.Checked;
            }
        }

        private void chkNoNeedSource_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoNeedSource.Checked)
            {
                _EditEntity.Customer_id = null;
                cmbCustomer_id.Visible = false;
            }
            else
            {
                cmbCustomer_id.Visible = true;
            }

        }

        private void btnAddBillingInformation_Click(object sender, EventArgs e)
        {
            if (_EditEntity.CustomerVendor_ID == 0)
            {
                MessageBox.Show("请先正确选择往来单位数据。");
                return;
            }
            object frm = Activator.CreateInstance(typeof(UCBillingInformationEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_BillingInformation> frmaddg = frm as BaseEditGeneric<tb_BillingInformation>;
                frmaddg.CurMenuInfo = this.CurMenuInfo;
                frmaddg.Text = "开票资料编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_BillingInformation>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_BillingInformation Info = obj as tb_BillingInformation;
                Info.CustomerVendor_ID = _EditEntity.CustomerVendor_ID;
                BaseEntity bty = Info as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    UIBizSrvice.SaveBillingInformation(Info);
                }
            }
        }
    }
}
