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
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using AutoMapper;
using Castle.Core.Resource;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("付款申请单", true, UIType.单表数据)]
    public partial class UCPaymentApplicationEdit : BaseBillEdit
    {
        public UCPaymentApplicationEdit()
        {
            InitializeComponent();
          // usedActionStatus = true;
           
        }
        tb_ModuleDefinition crmMod = null;
        private tb_FM_PaymentApplication _EditEntity;
        public  void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            _EditEntity = entity as tb_FM_PaymentApplication;

            if (_EditEntity.CustomerVendor_ID == 0)
            {

                //新建时默认启用

            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.ApplicationNo, txtApplicationNo, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.Account_name, cmbPayeeInfoID);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4CheckBox<tb_FM_PaymentApplication>(entity, t => t.IsAdvancePayment, chkIsAdvancePayment, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.PrePaymentBill_id, txtPrePaymentBill_id, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.PayReasonItems, txtPayReasonItems, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_FM_PaymentApplication>(entity, t => t.InvoiceDate, dtpInvoiceDate, false);
            DataBindingHelper.BindData4DataTime<tb_FM_PaymentApplication>(entity, t => t.PaymentDate, dtpPaymentDate, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.OverpaymentAmount.ToString(), txtOverpaymentAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_FM_PaymentApplication>(entity, t => t.Approver_at, dtpApprover_at, false);
            //default  DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
            // DataBindingHelper.BindData4CheckBox<tb_FM_PaymentApplication>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4ControlByEnum<tb_PurEntry>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));

            DataBindingHelper.BindData4ControlByEnum<tb_PurEntry>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, entity);

            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.CloseCaseImagePath, txtCloseCaseImagePath, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text, false);


            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == false)//非客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);

            //带过滤的下拉绑定要这样
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);

            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {

               // base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentApplicationValidator>(), kryptonPanel1.Controls);
                //base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            

            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果线索引入相关数据
                //if ((_EditEntity.ActionStatus == ActionStatus.新增 || _EditEntity.ActionStatus == ActionStatus.修改) && _EditEntity.Customer_id.HasValue && _EditEntity.Customer_id.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_PaymentApplication>(c => c.Customer_id))
                //{
                //    await ToCustomer(_EditEntity);
                //}
            };

            BindData(entity);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //bindingSourceEdit.CancelEdit();
            //this.DialogResult = DialogResult.Cancel;
            //this.Close();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            //if (crmMod.Available && (!_EditEntity.Customer_id.HasValue || _EditEntity.Customer_id.Value <= 0) && _EditEntity.IsCustomer)
            //{
            //    //客户关系模块启用时，销售客户的来源必须选择。
            //    MessageBox.Show("销售客户的来源必须选择。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}


            //if (base.Validator())
            //{
            //    bindingSourceEdit.EndEdit();
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}
        }

        private void btnAddPayeeInfo_Click(object sender, EventArgs e)
        {

        }

        private void UCCustomerVendorEdit_Load(object sender, EventArgs e)
        {
           
        }


        //将目标客户信息带过来。可以用automapper
        private async Task<tb_FM_PaymentApplication> ToCustomer(tb_FM_PaymentApplication entity)
        {
            tb_CRM_Customer crmCustomer;
            //ButtonSpecAny bsa = cmbCustomer_id.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            //if (bsa == null)
            //{
            //    return null;
            //}
            //saleorder = bsa.Tag as tb_SaleOrder;

            crmCustomer = await MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Customer>()
            .Where(c => c.Customer_id == 1)
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

        
    }
}
