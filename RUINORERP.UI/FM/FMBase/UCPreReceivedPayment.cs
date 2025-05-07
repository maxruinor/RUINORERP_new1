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
using FastReport.Table;
using MathNet.Numerics.Optimization;


namespace RUINORERP.UI.FM
{

    /// <summary>
    /// 预收付款单， 
    /// </summary>
    public partial class UCPreReceivedPayment : BaseBillEditGeneric<tb_FM_PreReceivedPayment, tb_FM_PreReceivedPayment>
    {
        public UCPreReceivedPayment()
        {
            InitializeComponent();
            // usedActionStatus = true;

        }

        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }

        /// <summary>
        /// 收付款类型决定对应的菜单功能
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }

        //草稿 → 已审核 → 部分生效 → 全部生效 或 草稿 → 已冲销
        public override void BindData(tb_FM_PreReceivedPayment entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            EditEntity = entity;
            if (entity == null)
            {
                return;
            }
            txtApprovalOpinions.ReadOnly = true;
            if (entity.PreRPID > 0)
            {
                entity.PrimaryKeyID = entity.PreRPID;
                entity.ActionStatus = ActionStatus.加载;
                lblLocalPrepaidAmountInWords.Text = entity.LocalPrepaidAmount.ToUpper();
                if (entity.CustomerVendor_ID > 0)
                {
                    //如果线索引入相关数据
                    if (PaymentType == ReceivePaymentType.付款)
                    {
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
                    //else
                    //{
                    //    //收款信息不显示，只在付款时显示
                    //    cmbPayeeInfoID.Visible = false;
                    //    btnInfo.Visible = false;
                    //    kryptonGroupBox收款账号信息.Visible = false;
                    //}
                }
                else
                {
                    //清空
                    cmbPayeeInfoID.DataBindings.Clear();
                }
                //根据币别如果是外币才显示外币相关的字段
                if (entity.Currency_ID > 0)
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_Currency>(entity.Currency_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_Currency cv)
                        {
                            if (cv.CurrencyCode != DefaultCurrency.RMB.ToString())
                            {
                                //显示外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_FM_PreReceivedPayment>(this, true);
                                lblExchangeRate.Visible = true;
                                txtExchangeRate.Visible = true;
                            }
                            else
                            {
                                //隐藏外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_FM_PreReceivedPayment>(this, false);
                                lblExchangeRate.Visible = false;
                                txtExchangeRate.Visible = false;
                            }
                        }

                    }
                }
            }
            else
            {
                entity.ReceivePaymentType = (int)PaymentType;
                entity.ActionStatus = ActionStatus.新增;
                entity.PrePayDate = System.DateTime.Now;
                entity.PrePaymentStatus = (long)PrePaymentStatus.草稿;
                if (PaymentType == ReceivePaymentType.付款)
                {
                    entity.PreRPNO = BizCodeGenerator.Instance.GetBizBillNo(BizType.预付款单);
                }
                else
                {
                    entity.PreRPNO = BizCodeGenerator.Instance.GetBizBillNo(BizType.预收款单);
                }
                //entity.InvoiceDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                //清空

                // 清空 DataSource（如果适用）
                cmbPayeeInfoID.DataSource = null;
                cmbPayeeInfoID.DataBindings.Clear();
                cmbPayeeInfoID.Items.Clear();
                cmbAccount_type.DataSource = null;
                cmbAccount_type.Items.Clear();
                cmbAccount_type.DataBindings.Clear();
                txtPayeeAccountNo.Text = "";
                lblExchangeRate.Visible = false;

            }

            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID, c => c.Cash == true);
          
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);

            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.PreRPNO, txtPreRPNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);

             DataBindingHelper.BindData4CheckBox<tb_FM_PreReceivedPayment>(entity, t => t.IsAvailable, chkIsAvailable, false);

            DataBindingHelper.BindData4ControlByEnum<tb_FM_PreReceivedPayment>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_FM_PreReceivedPayment>(entity, t => t.PrePayDate, dtpPrePayDate, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.PrePaymentReason, txtPrePaymentReason, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.SourceBizType, txtBizType, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ForeignPrepaidAmount.ToString(), txtForeignPrepaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalPrepaidAmount.ToString(), txtLocalPrepaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ForeignPaidAmount.ToString(), txtForeignPaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalPaidAmount.ToString(), txtLocalPaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ForeignBalanceAmount.ToString(), txtForeignBalanceAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalBalanceAmount.ToString(), txtLocalBalanceAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Label<tb_FM_PreReceivedPayment>(entity, k => k.LocalPrepaidAmountInWords, lblLocalPrepaidAmountInWords, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_PreReceivedPayment>(entity, t => t.PrePaymentStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(PrePaymentStatus));
            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, entity);


            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)//供应商和第三方
                            .AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)//供应商和第三方
                            .And(t => t.isdeleted == false)
                            .And(t => t.Is_enabled == true)
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
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_PreReceivedPaymentValidator>(), kryptonPanel1.Controls);
            }

            entity.PropertyChanged += async (sender, s2) =>
            {
                if (s2.PropertyName == entity.GetPropertyName<tb_FM_PreReceivedPayment>(c => c.CustomerVendor_ID))
                {
                    if (PaymentType == ReceivePaymentType.付款)
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

                }
                //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    if (PaymentType == ReceivePaymentType.付款 && s2.PropertyName == entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.PayeeInfoID))
                    {
                        cmbPayeeInfoID.Enabled = true;
                        //加载收款信息
                        if (entity.PayeeInfoID > 0)
                        {
                            tb_FM_PayeeInfo payeeInfo = null;
                            var obj = BizCacheHelper.Instance.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                            if (obj != null && obj.ToString() != "System.Object")
                            {
                                if (obj is tb_FM_PayeeInfo cv)
                                {
                                    payeeInfo = cv;
                                }
                            }
                            else
                            {
                                //直接加载 不用缓存
                                payeeInfo = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PayeeInfo>().Where(c => c.PayeeInfoID == entity.PayeeInfoID).FirstAsync();
                            }
                            if (payeeInfo != null)
                            {
                                DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(payeeInfo, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
                                //添加收款信息。展示给财务看
                                entity.PayeeAccountNo = payeeInfo.Account_No;
                                lblBelongingBank.Text = payeeInfo.BelongingBank;
                                lblOpeningbank.Text = payeeInfo.OpeningBank;
                                cmbAccount_type.SelectedItem = payeeInfo.Account_type;
                                if (!string.IsNullOrEmpty(payeeInfo.PaymentCodeImagePath))
                                {
                                    btnInfo.Tag = payeeInfo;
                                    btnInfo.Visible = true;
                                }
                                else
                                {
                                    btnInfo.Tag = string.Empty;
                                    btnInfo.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            txtPayeeAccountNo.Text = "";
                            lblBelongingBank.Text = "";
                            lblOpeningbank.Text = "";
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_PreReceivedPayment>(c => c.Currency_ID))
                    {
                        //根据币别如果是外币才显示外币相关的字段
                        if (entity.Currency_ID > 0)
                        {
                            var obj = BizCacheHelper.Instance.GetEntity<tb_Currency>(entity.Currency_ID);
                            if (obj != null && obj.ToString() != "System.Object")
                            {
                                if (obj is tb_Currency cv)
                                {
                                    if (cv.CurrencyCode.Trim() != DefaultCurrency.RMB.ToString())
                                    {
                                        //显示外币相关
                                        UIHelper.ControlForeignFieldInvisible<tb_FM_PreReceivedPayment>(this, true);
                                        //需要有一个方法。通过外币代码得到换人民币的汇率
                                        entity.ExchangeRate = BizService.GetExchangeRateFromCache(cv.Currency_ID, AppContext.BaseCurrency.Currency_ID);
                                        lblExchangeRate.Visible = true;
                                        txtExchangeRate.Visible = true;
                                    }
                                    else
                                    {
                                        //隐藏外币相关
                                        UIHelper.ControlForeignFieldInvisible<tb_FM_PreReceivedPayment>(this, false);
                                        lblExchangeRate.Visible = false;
                                        txtExchangeRate.Visible = false;
                                    }
                                }
                            }
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_PreReceivedPayment>(c => c.LocalPrepaidAmount))
                    {
                        lblLocalPrepaidAmountInWords.Text = entity.LocalPrepaidAmount.ToUpper();
                    }

                }
            };

            //加载收款信息
            if (entity.PayeeInfoID.HasValue && entity.PayeeInfoID > 0)
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

            //显示图片
            LoadImageData(entity.PaymentImagePath);
            base.BindData(entity);
        }



        private async void LoadImageData(string ImagePath)
        {
            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                try
                {
                    byte[] img = await httpWebService.DownloadImgFileAsync(ImagePath);
                    PaymentImage.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                    PaymentImage.Visible = true;
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                }
            }
            else
            {
                PaymentImage.Visible = true;
            }
        }


        private void SetValueToRowImage()
        {
            PaymentImage.RowImage.image = PaymentImage.Image;
            PaymentImage.RowImage.ImageBytes = PaymentImage.Image.ToBytes();
            EditEntity.RowImage = PaymentImage.RowImage;
            EditEntity.PaymentImagePath = EditEntity.RowImage.ImageFullName;
        }

        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            if (PaymentImage.Image != null)
            {
                //还要处理更新的情况。是不是命名类似于grid
                SetValueToRowImage();
            }
            var eer = errorProviderForAllInput.GetError(txtLocalPrepaidAmount);
         
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

                //if (NeedValidated && EditEntity.TotalAmount == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("金额不能为零，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return false;
                //}

                ReturnMainSubResults<tb_FM_PreReceivedPayment> SaveResult = new ReturnMainSubResults<tb_FM_PreReceivedPayment>();
                if (NeedValidated)
                {

                    //保存图片
                    #region 

                    if (ReflectionHelper.ExistPropertyName<tb_FM_PreReceivedPayment>(nameof(EditEntity.RowImage)) && EditEntity.RowImage != null)
                    {
                        if (EditEntity.RowImage.image != null)
                        {
                            if (!EditEntity.RowImage.oldhash.Equals(EditEntity.RowImage.newhash, StringComparison.OrdinalIgnoreCase)
                             && EditEntity.PaymentImagePath == EditEntity.RowImage.ImageFullName)
                            {
                                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                //如果服务器有旧文件 。可以先删除
                                if (!string.IsNullOrEmpty(EditEntity.RowImage.oldhash))
                                {
                                    string oldfileName = EditEntity.RowImage.Dir + EditEntity.RowImage.realName + "-" + EditEntity.RowImage.oldhash;
                                    string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                                    MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                                }
                                string newfileName = EditEntity.RowImage.GetUploadfileName();
                                ////上传新文件时要加后缀名
                                string uploadRsult = await httpWebService.UploadImageAsync(newfileName + ".jpg", EditEntity.RowImage.ImageBytes, "upload");
                                if (uploadRsult.Contains("UploadSuccessful"))
                                {
                                    //重要
                                    EditEntity.RowImage.ImageFullName = EditEntity.RowImage.UpdateImageName(EditEntity.RowImage.newhash);
                                    EditEntity.PaymentImagePath= EditEntity.RowImage.ImageFullName;
                                
                                    //成功后。旧文件名部分要和上传成功后新文件名部分一致。后面修改只修改新文件名部分。再对比
                                    MainForm.Instance.PrintInfoLog("UploadSuccessful for base List:" + newfileName);
                                }
                                else
                                {
                                    MainForm.Instance.LoginWebServer();
                                }
                            }
                        }
                    }
                    #endregion
                    //保存路径

                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SourceBillNo}。");
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

            if (!string.IsNullOrEmpty(EditEntity.PaymentImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                string deleteRsult = await httpWebService.DeleteImageAsync(EditEntity.PaymentImagePath, "delete123");
                MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
            }
            #endregion

            bool result = true;

            return result;
        }

        protected async override Task<ReturnResults<tb_FM_PreReceivedPayment>> Delete()
        {
            ReturnResults<tb_FM_PreReceivedPayment> rss = new ReturnResults<tb_FM_PreReceivedPayment>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (PrePaymentStatus)(EditEntity.GetPropertyValue(typeof(PrePaymentStatus).Name).ToInt());
                //没有收到钱之前都可以删除？
                if ( dataStatus == PrePaymentStatus.草稿|| dataStatus == PrePaymentStatus.待审核 ||  dataStatus==PrePaymentStatus.已生效)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (!AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                        {
                            MessageBox.Show("只有创建人才能删除待审核的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除待审核的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment> ctr = Startup.GetFromFac<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    bool rs = await ctr.BaseDeleteAsync(EditEntity as tb_FM_PreReceivedPayment);
                    if (rs)
                    {
                        
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
            switch (PaymentType)
            {
                case ReceivePaymentType.收款:
                    lblBillText.Text = "预收款单";
                    lblAccount_id.Text = "收款账号";
                    lblPrePayDate.Text = "收款日期";
                    lblPrePaymentReason.Text = "收款事由";
                    lblCustomerVendor_ID.Text = "付款单位";
                    btnInfo.Visible = false;
                    cmbPayeeInfoID.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    kryptonGroupBox收款账号信息.Visible = false;

                    break;
                case ReceivePaymentType.付款:
                    lblBillText.Text = "预付款单";
                    lblAccount_id.Text = "付款账号";
                    lblPrePayDate.Text = "付款日期";
                    lblPrePaymentReason.Text = "付款事由";
                    lblCustomerVendor_ID.Text = "收款单位";
                    break;
                default:
                    break;
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {

        }

        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnInfo_Click_1(object sender, EventArgs e)
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
                            frmaddg.CurMenuInfo = this.CurMenuInfo;
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

                    }

                }
            }
        }
    }
}
