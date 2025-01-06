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
using RUINORERP.UI.BI;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.SysConfig;
using Fireasy.Common.Extensions;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("付款申请单", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.收付账款, BizType.付款申请单)]
    public partial class UCPaymentApplicationEdit : BaseBillEditGeneric<tb_FM_PaymentApplication, tb_FM_PaymentApplication>
    {
        public UCPaymentApplicationEdit()
        {
            InitializeComponent();
            // usedActionStatus = true;

        }

        public override void BindData(tb_FM_PaymentApplication entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            EditEntity = entity;
            if (entity == null)
            {
                return;
            }

            if (entity.ApplicationID > 0)
            {
                entity.PrimaryKeyID = entity.ApplicationID;
                entity.ActionStatus = ActionStatus.加载;
                //lblMoneyUpper.Text = entity.TotalAmount.Value.ToUpper();
                if (entity.CustomerVendor_ID > 0)
                {
                    //如果线索引入相关数据
                    #region 收款信息可以根据往来单位带出 ，并且可以添加

                    //创建表达式
                    var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
                                .And(t => t.Is_enabled == true)
                                .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
                                .ToExpression();//注意 这一句 不能少
                    BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
                    QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
                    queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

                    DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.Account_name, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
                    DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.Account_name, queryFilterPayeeInfo, true);


                    #endregion
                }
                // entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApplicationNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款申请单);
                entity.InvoiceDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;

                //if (entity.tb != null && entity.tb_PurOrderDetails.Count > 0)
                //{
                //    entity.tb_PurOrderDetails.ForEach(c => c.PurOrder_ID = 0);
                //    entity.tb_PurOrderDetails.ForEach(c => c.PurOrder_ChildID = 0);
                //}
            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.ApplicationNo, txtApplicationNo, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            // DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(entity, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);

            DataBindingHelper.BindData4Label<tb_FM_PaymentApplication>(entity, k => k.PamountInWords, lblMoneyUpper, BindDataType4TextBox.Text, true);

            DataBindingHelper.BindData4CheckBox<tb_FM_PaymentApplication>(entity, t => t.IsAdvancePayment, chkIsAdvancePayment, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.PrePaymentBill_id, txtPrePaymentBill_id, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.PayReasonItems, txtPayReasonItems, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_FM_PaymentApplication>(entity, t => t.InvoiceDate, dtpInvoiceDate, false);

            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.OverpaymentAmount.ToString(), txtOverpaymentAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4TextBox<tb_FM_PaymentApplication>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);

            // DataBindingHelper.BindData4CheckBox<tb_FM_PaymentApplication>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4ControlByEnum<tb_PurEntry>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));

            DataBindingHelper.BindData4ControlByEnum<tb_PurEntry>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, entity);

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

                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentApplicationValidator>(), kryptonPanel1.Controls);
            }


            entity.PropertyChanged += (sender, s2) =>
            {
                if (s2.PropertyName == entity.GetPropertyName<tb_FM_PaymentApplication>(c => c.CustomerVendor_ID))
                {
                    //如果线索引入相关数据
                    #region 收款信息可以根据往来单位带出 ，并且可以添加

                    //创建表达式
                    var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
                                .And(t => t.Is_enabled == true)
                                .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
                                .ToExpression();//注意 这一句 不能少
                    BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
                    QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
                    queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

                    DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.Account_name, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);


                    DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.Account_name, queryFilterPayeeInfo, true);


                    #endregion
                }

                //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    cmbPayeeInfoID.Enabled = true;
                    //加载收款信息
                    if (entity.PayeeInfoID > 0)
                    {
                        //cmbPayeeInfoID.SelectedIndex = cmbPayeeInfoID.FindStringExact(emp.Account_name);
                        var obj = BizCacheHelper.Instance.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                        if (obj != null && obj.ToString() != "System.Object")
                        {
                            if (obj is tb_FM_PayeeInfo cv)
                            {
                                DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(cv, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
                                //添加收款信息。展示给财务看
                                entity.PayeeAccountNo = cv.Account_No;
                                lblBelongingBank.Text = cv.BelongingBank;
                                lblOpeningbank.Text = cv.OpeningBank;
                                cmbAccount_type.SelectedItem = cv.Account_type;
                                if (!string.IsNullOrEmpty(cv.PaymentCodeImagePath))
                                {
                                    btnInfo.Tag = cv;
                                    btnInfo.Visible = true;
                                }
                                else
                                {
                                    btnInfo.Tag = string.Empty;
                                    btnInfo.Visible = false;
                                }
                            }
                            else
                            {
                                txtPayeeAccountNo.Text = "";
                                lblBelongingBank.Text = "";
                                lblOpeningbank.Text = "";
                            }
                        }
                    }
                }
                else
                {
                    cmbPayeeInfoID.Enabled = false;
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_FM_PaymentApplication>(c => c.TotalAmount))
                {
                    entity.PamountInWords= entity.TotalAmount.Value.ToUpper();
                    //lblMoneyUpper.Text = entity.TotalAmount.Value.ToUpper();
                }

            };

            //加载收款信息
            if (entity.PayeeInfoID > 0)
            {
                var obj = BizCacheHelper.Instance.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                if (obj != null && obj.ToString() != "System.Object")
                {
                    if (obj is tb_FM_PayeeInfo cv)
                    {
                        DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(cv, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
                        //添加收款信息。展示给财务看
                        entity.PayeeAccountNo = cv.Account_No;
                        lblBelongingBank.Text = cv.BelongingBank;
                        lblOpeningbank.Text = cv.OpeningBank;
                        // lblPayeeAccountName.Text = cv.Account_name;
                        cmbAccount_type.SelectedItem = cv.Account_type;
                        if (!string.IsNullOrEmpty(cv.PaymentCodeImagePath))
                        {
                            btnInfo.Tag = cv;
                            btnInfo.Visible = true;
                        }
                        else
                        {
                            btnInfo.Tag = string.Empty;
                            btnInfo.Visible = false;
                        }
                    }
                    else
                    {
                        txtPayeeAccountNo.Text = "";
                        lblBelongingBank.Text = "";
                        lblOpeningbank.Text = "";
                    }
                }
            }
            //显示结案凭证图片
            LoadImageData(entity.CloseCaseImagePath);
            base.BindData(entity);
        }

        private async void LoadImageData(string CloseCaseImagePath)
        {
            if (!string.IsNullOrWhiteSpace(CloseCaseImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                try
                {
                    byte[] img = await httpWebService.DownloadImgFileAsync(CloseCaseImagePath);
                    magicPictureBox1.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                    magicPictureBox1.Visible = true;
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                }
            }
            else
            {
                magicPictureBox1.Visible = false;
            }
        }

        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtTotalAmount);

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {

                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalAmount == 0)
                {
                    System.Windows.Forms.MessageBox.Show("金额不能为零，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.PamountInWords = lblMoneyUpper.Text;
                ReturnMainSubResults<tb_FM_PaymentApplication> SaveResult = new ReturnMainSubResults<tb_FM_PaymentApplication>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ApplicationNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }
            return false;
        }



        protected override async Task<bool> Submit()
        {
            bool rs = await base.Submit();
            if (rs)
            {
                ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
                var temppath = configManager.GetValue("WebServerUrl");
                if (string.IsNullOrEmpty(temppath))
                {
                    MainForm.Instance.uclog.AddLog("请先配置图片服务器路径", UILogType.错误);
                }
            }
            return true;
        }

        public override async Task<bool> DeleteRemoteImages()
        {

            if (EditEntity == null)
            {
                return false;
            }

            #region 删除主图的结案图。一般没有结案是没有的。结案就不会有结案图了。也有特殊情况。

            if (!string.IsNullOrEmpty(EditEntity.CloseCaseImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                string deleteRsult = await httpWebService.DeleteImageAsync(EditEntity.CloseCaseImagePath, "delete123");
                MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
            }
            #endregion

            bool result = true;

            return result;
        }

        protected async override Task<ReturnResults<tb_FM_PaymentApplication>> Delete()
        {
            ReturnResults<tb_FM_PaymentApplication> rss = new ReturnResults<tb_FM_PaymentApplication>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (DataStatus)(EditEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                        {
                            MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_PaymentApplicationController<tb_FM_PaymentApplication> ctr = Startup.GetFromFac<tb_FM_PaymentApplicationController<tb_FM_PaymentApplication>>();
                    bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_FM_PaymentApplication);
                    if (rs)
                    {
                        //AuditLogHelper.Instance.CreateAuditLog<T>("删除", EditEntity);
                        //if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        //{
                        //    //MainForm.Instance.logger.Debug($"单据显示中删除:{typeof(T).Name}，主键值：{PKValue.ToString()} "); //如果要生效 要将配置文件中 <add key="log4net.Internal.Debug" value="true " /> 也许是：logn4net.config <log4net debug="false"> 改为true
                        //}
                        // bindingSourceSub.Clear();

                        ////删除远程图片及本地图片
                        await DeleteRemoteImages();

                        //提示一下删除成功
                        MainForm.Instance.uclog.AddLog("提示", "删除成功");

                        //加载一个空的显示的UI
                        // bindingSourceSub.Clear();
                        //base.OnBindDataToUIEvent(EditEntity as tb_FM_ExpenseClaim, ActionStatus.删除);
                        Exit(this);
                    }
                }
                else
                {
                    //
                    MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                }
            }
            return rss;
        }


        private void UCCustomerVendorEdit_Load(object sender, EventArgs e)
        {

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (sender is KryptonButton btninfo)
            {
                if (btninfo.Tag != null)
                {
                    if (!string.IsNullOrWhiteSpace(btninfo.Tag.ToString()))
                    {
                        //tb_FM_PayeeInfo payeeInfo = btninfo.Tag as tb_FM_PayeeInfo;

                        #region 显示收款详情信息

                        object frm = Activator.CreateInstance(typeof(UCFMPayeeInfoEdit));
                        if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                        {
                            BaseEditGeneric<tb_FM_PayeeInfo> frmaddg = frm as BaseEditGeneric<tb_FM_PayeeInfo>;
                            frmaddg.Text = "收款账号详情";
                            frmaddg.bindingSourceEdit.DataSource = new List<tb_FM_PayeeInfo>();
                            object obj = frmaddg.bindingSourceEdit.AddNew();
                            obj = btninfo.Tag;
                            tb_FM_PayeeInfo payeeInfo = obj as tb_FM_PayeeInfo;
                            BaseEntity bty = payeeInfo as BaseEntity;
                            bty.ActionStatus = ActionStatus.加载;
                            frmaddg.BindData(bty);
                            if (frmaddg.ShowDialog() == DialogResult.OK)
                            {

                            }
                        }
                        #endregion
                        //HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        //try
                        //{
                        //    byte[] img = await httpWebService.DownloadImgFileAsync(btninfo.Tag.ToString());
                        //    frmPictureViewer pictureViewer = new frmPictureViewer();
                        //    pictureViewer.PictureBoxViewer.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                        //    pictureViewer.ShowDialog();
                        //}
                        //catch (Exception ex)
                        //{
                        //    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                        //}
                    }

                }
            }
        }

        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
