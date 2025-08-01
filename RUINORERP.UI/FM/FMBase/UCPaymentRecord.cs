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
using Netron.GraphLib;
using RUINORERP.UI.UCSourceGrid;
using RUINORERP.Model.Dto;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.StatusManagerService;


namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 收付款记录合并
    /// </summary>
    public partial class UCPaymentRecord : BaseBillEditGeneric<tb_FM_PaymentRecord, tb_FM_PaymentRecordDetail>
    {
        public UCPaymentRecord()
        {
            InitializeComponent();
            // usedActionStatus = true;

        }
        public override void QueryConditionBuilder()
        {

            base.QueryConditionBuilder();
            //创建表达式
            var lambda = Expressionable.Create<tb_FM_PaymentRecord>()
                              //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                              // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                              .And(t => t.ReceivePaymentType == (int)PaymentType)
                             .And(t => t.isdeleted == false)
                            //报销人员限制，财务不限制 自己的只能查自己的
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Created_by == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }
        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.反审);
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.复制性新增);
            base.AddExcludeMenuList(MenuItemEnums.新增);
        }


        private void LoadPayeeInfo(tb_FM_PaymentRecord entity)
        {
            if (entity.CustomerVendor_ID.HasValue && entity.CustomerVendor_ID.Value > 0)
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

                    DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
                    DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterPayeeInfo, true);

                    #endregion
                    lblCustomerVendor_ID.Visible = true;
                    cmbCustomerVendor_ID.Visible = true;
                    lblReimburser.Visible = false;
                    cmbReimburser.Visible = false;

                }
                else
                {
                    lblPayeeInfoID.Visible = false;
                    cmbPayeeInfoID.Visible = false;
                    btnInfo.Visible = false;
                }
            }
            else if (entity.Reimburser.HasValue && entity.Reimburser.Value > 0)
            {
                #region 收款信息可以根据往来单位带出 ，并且可以添加

                //创建表达式
                var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
                            .And(t => t.Is_enabled == true)
                            .And(t => t.Employee_ID == entity.Reimburser)
                            .ToExpression();//注意 这一句 不能少
                BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
                QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
                queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

                DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
                DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterPayeeInfo, true);

                lblCustomerVendor_ID.Visible = false;
                cmbCustomerVendor_ID.Visible = false;
                lblReimburser.Visible = true;
                cmbReimburser.Visible = true;
                #endregion
            }
            else
            {
                //清空
                cmbPayeeInfoID.DataBindings.Clear();
            }

        }


        /// <summary>
        /// 收付款方式决定对应的菜单功能
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }

        //草稿 → 已审核 → 部分生效 → 全部生效 或 草稿 → 已冲销
        public override void BindData(tb_FM_PaymentRecord entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            EditEntity = entity;
            if (entity == null)
            {
                return;
            }
            txtApprovalOpinions.ReadOnly = true;
            if (entity.PaymentId > 0)
            {
                entity.PrimaryKeyID = entity.PaymentId;
                entity.ActionStatus = ActionStatus.加载;
                LoadPayeeInfo(entity);
                ControlCurrency(entity);

            }
            else
            {
                entity.ReceivePaymentType = (int)PaymentType;
                entity.ActionStatus = ActionStatus.新增;
                entity.PaymentDate = System.DateTime.Now;
                entity.PaymentStatus = (int)PaymentStatus.草稿;
                if (string.IsNullOrEmpty(entity.PaymentNo))
                {
                    if (PaymentType == ReceivePaymentType.付款)
                    {
                        entity.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
                    }
                    else
                    {
                        entity.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
                    }
                }

                //entity.InvoiceDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                //清空
                ControlCurrency(entity);


                // 清空 DataSource（如果适用）
                cmbPayeeInfoID.DataSource = null;
                cmbPayeeInfoID.DataBindings.Clear();
                cmbPayeeInfoID.Items.Clear();
                //如果是转单过来的
                LoadPayeeInfo(entity);

            }
            DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.BindData4Cmb<tb_Employee, tb_FM_PaymentRecord>(entity, k => k.Employee_ID, v => v.Employee_Name,
                t => t.Reimburser.Value, cmbEmployee_ID, false);

            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PaymentNo, txtPaymentNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_PaymentRecord>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //  DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.TotalForeignAmount.ToString(), txtTotalForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.TotalLocalAmount.ToString(), txtTotalLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_PaymentRecord>(entity, t => t.PaymentStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(PaymentStatus));
            //  DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.SourceBizType, txtBizType, BindDataType4TextBox.Qty, false);
            //DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.SourceBilllID, txtSourceBilllID, BindDataType4TextBox.Qty, false);
            //  DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text, false);
            //  DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.PaymentDate, dtpPaymentDate, false);
            //  DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReferenceNo, txtReferenceNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalResults, chkApprovalResults, false);


            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, entity);
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            if (entity.tb_FM_PaymentRecordDetails != null && entity.tb_FM_PaymentRecordDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_FM_PaymentRecordDetail>(grid1, sgd, entity.tb_FM_PaymentRecordDetails, c => c.PaymentDetailId);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_PaymentRecordDetail>(grid1, sgd, new List<tb_FM_PaymentRecordDetail>(), c => c.PaymentDetailId);
            }
            if (entity.ReceivePaymentType == (long)ReceivePaymentType.收款)
            {
                //收客户的款
                var lambda = Expressionable.Create<tb_CustomerVendor>()
                                .And(t => t.IsCustomer == true)//供应商和第三方
                                .And(t => t.isdeleted == false)
                                .And(t => t.Is_enabled == true)
                                .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                queryFilterC.FilterLimitExpressions.Add(lambda);

                //带过滤的下拉绑定要这样
                DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);
            }
            else
            {
                //创建表达式
                var lambda = Expressionable.Create<tb_CustomerVendor>()
                                .And(t => t.IsVendor == true || t.IsOther == true)//供应商和第三方
                                .And(t => t.isdeleted == false)
                                .And(t => t.Is_enabled == true)
                                .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                queryFilterC.FilterLimitExpressions.Add(lambda);

                //带过滤的下拉绑定要这样
                DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);
            }
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordValidator>(), kryptonPanel1.Controls);
            }

            //如果是草稿或新建  字段修改 自动状态为修改后
            // 获取当前状态
            var statusProperty = typeof(PaymentStatus).Name;
            var currentStatus = (PaymentStatus)Enum.ToObject(typeof(PaymentStatus),
                EditEntity.GetPropertyValue(statusProperty)
            );

            entity.PropertyChanged += async (sender, s2) =>
            {
                if (FMPaymentStatusHelper.CanModify<PaymentStatus>(currentStatus))
                {
                    entity.ActionStatus = ActionStatus.修改;
                }
                if (s2.PropertyName == entity.GetPropertyName<tb_FM_PaymentRecord>(c => c.Reimburser))
                {
                    LoadPayeeInfo(entity);
                }
                if (s2.PropertyName == entity.GetPropertyName<tb_FM_PaymentRecord>(c => c.CustomerVendor_ID))
                {
                    LoadPayeeInfo(entity);
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
                            if (obj != null)
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
                                //添加收款信息。展示给财务看
                                entity.PayeeAccountNo = payeeInfo.Account_No;

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

                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_PaymentRecord>(c => c.Currency_ID))
                    {
                        ControlCurrency(entity);
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
                        //添加收款信息。展示给财务看
                        entity.PayeeAccountNo = cv.Account_No;
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
                }
            }

            //显示图片
            LoadImageData(entity.PaymentImagePath);
            base.BindData(entity);
        }

        protected async override Task<ReviewResult> Review()
        {
            ReviewResult reviewResult = new ReviewResult();
            if (!EditEntity.Paytype_ID.HasValue || EditEntity.Paytype_ID.Value == MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID)
            {
                MessageBox.Show("请选择正确的付款方式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                reviewResult = await base.Review();
            }
            return reviewResult;
        }
        private void ControlCurrency(tb_FM_PaymentRecord entity)
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
                            UIHelper.ControlForeignFieldInvisible<tb_FM_PaymentRecord>(this, true);
                            //需要有一个方法。通过外币代码得到换人民币的汇率
                            // entity.ExchangeRate = BizService.GetExchangeRateFromCache(cv.Currency_ID, AppContext.BaseCurrency.Currency_ID);
                            lblExchangeRate.Visible = true;
                            txtExchangeRate.Visible = true;
                        }
                        else
                        {
                            //隐藏外币相关
                            UIHelper.ControlForeignFieldInvisible<tb_FM_PaymentRecord>(this, false);
                            lblExchangeRate.Visible = false;
                            txtExchangeRate.Visible = false;
                            if (listCols != null)
                            {
                                listCols.SetCol_DefaultHide<tb_FM_PaymentRecordDetail>(c => c.ExchangeRate);
                            }

                        }
                    }
                }
            }
        }

        protected override void LoadRelatedDataToDropDownItems()
        {
            if (base.EditEntity is tb_FM_PaymentRecord PaymentRecord)
            {
                if (PaymentRecord.tb_FM_PaymentRecordDetails != null)
                {
                    foreach (var item in PaymentRecord.tb_FM_PaymentRecordDetails)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = (BizType)item.SourceBizType;
                        rqp.billId = item.SourceBilllId;

                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{item.SourceBilllId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.SourceBillNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        RelatedMenuItem.DropDownItemClicked += MenuItem_DropDownItemClicked;

                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.SourceBilllId.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }

                }
            }
            base.LoadRelatedDataToDropDownItems();
        }


        private void MenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        protected override void RelatedQuery()
        {

        }

        private async void LoadImageData(string CloseCaseImagePath)
        {
            if (!string.IsNullOrWhiteSpace(CloseCaseImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                try
                {
                    byte[] img = await httpWebService.DownloadImgFileAsync(CloseCaseImagePath);
                    //  magicPictureBox1.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                    //  magicPictureBox1.Visible = true;
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                }
            }
            else
            {
                // magicPictureBox1.Visible = false;
            }
        }



        List<tb_FM_PaymentRecordDetail> details = new List<tb_FM_PaymentRecordDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtTotalForeignAmount);
            bindingSourceSub.EndEdit();
            if (NeedValidated && (EditEntity.TotalForeignAmount == 0 && EditEntity.TotalLocalAmount == 0))
            {
                System.Windows.Forms.MessageBox.Show("收款金额不能为零，请检查数据录入是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            List<tb_FM_PaymentRecordDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_PaymentRecordDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ForeignAmount != 0 || t.LocalAmount != 0).ToList();


                //



                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                EditEntity.tb_FM_PaymentRecordDetails = details;


                //收付款单中的  收款或付款账号中的币别是否与选的币别一致。
                if (NeedValidated && EditEntity.Currency_ID > 0 && EditEntity.Account_id > 0)
                {
                    tb_FM_Account bizcatch = BizCacheHelper.Instance.GetEntity<tb_FM_Account>(EditEntity.Account_id);
                    if (bizcatch != null && bizcatch.Currency_ID != EditEntity.Currency_ID)
                    {
                        MessageBox.Show("收付款账号中的币别与当前单据的币别不一致。");
                        return false;
                    }
                }


                //如果主表的总金额和明细金额加总后不相等，则提示
                if (NeedValidated && EditEntity.TotalForeignAmount != details.Sum(c => c.ForeignAmount))
                {
                    if (MessageBox.Show("总金额外币和明细金额外币总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }


                if (NeedValidated && EditEntity.TotalLocalAmount != details.Sum(c => c.LocalAmount))
                {
                    if (MessageBox.Show("总金额本币和明细金额本币总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_PaymentRecordDetail>(details))
                {
                    return false;
                }
                if (NeedValidated)
                {//处理图片
                    bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_PaymentRecordDetails);
                    if (uploadImg)
                    {
                        ////更新图片名后保存到数据库
                        //int ImgCounter = await MainForm.Instance.AppContext.Db.Updateable<tb_FM_PaymentRecordDetail>(EditEntity.tb_FM_PaymentRecordDetails)
                        //    .UpdateColumns(t => new { t.EvidenceImagePath })
                        //    .ExecuteCommandAsync();
                        //if (ImgCounter > 0)
                        //{
                        MainForm.Instance.PrintInfoLog($"图片保存成功,。");
                        //}
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("图片上传出错。");
                        return false;
                    }
                }

                ReturnMainSubResults<tb_FM_PaymentRecord> SaveResult = new ReturnMainSubResults<tb_FM_PaymentRecord>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PaymentNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }
            else
            {
                MainForm.Instance.uclog.AddLog("加载状态下无法保存");
                return false;
            }
        }

        protected override async Task<bool> Submit()
        {
            bool result = await Submit(PaymentStatus.待审核);
            if (result)
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

        protected async override Task<ReturnResults<tb_FM_PaymentRecord>> Delete()
        {
            ReturnResults<tb_FM_PaymentRecord> rss = new ReturnResults<tb_FM_PaymentRecord>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html

                var paymentStatus = (PaymentStatus)(EditEntity.GetPropertyValue(typeof(PaymentStatus).Name).ToInt());
                if (paymentStatus == PaymentStatus.待审核 || paymentStatus == PaymentStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除

                    //if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                    //{
                    //    MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                    //    rss.Succeeded = false;
                    //    return rss;
                    //}


                    tb_FM_PaymentRecordController<tb_FM_PaymentRecord> ctr = Startup.GetFromFac<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    bool rs = await ctr.BaseDeleteByNavAsync(EditEntity as tb_FM_PaymentRecord);
                    if (rs)
                    {
                        //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除", EditEntity);
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



        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();



        private void UCPaymentRecord_Load(object sender, EventArgs e)
        {
            #region
            switch (PaymentType)
            {
                case ReceivePaymentType.收款:
                    lblBillText.Text = "应收款单";
                    lblAccount_id.Text = "收款账号";
                    lblCustomerVendor_ID.Text = "应付单位";

                    btnInfo.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    cmbPayeeInfoID.Visible = false;

                    break;
                case ReceivePaymentType.付款:
                    lblBillText.Text = "应付款单";
                    lblAccount_id.Text = "付款账号";
                    lblCustomerVendor_ID.Text = "应收单位";
                    break;
                default:
                    break;
            }

            #endregion
            if (!this.DesignMode)
            {
                MainForm.Instance.LoginWebServer();
                if (CurMenuInfo != null)
                {
                    lblBillText.Text = CurMenuInfo.CaptionCN;
                }
            }

            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_PaymentRecordDetail>();

            listCols.SetCol_NeverVisible<tb_FM_PaymentRecordDetail>(c => c.PaymentDetailId);
            listCols.SetCol_NeverVisible<tb_FM_PaymentRecordDetail>(c => c.SourceBilllId);

            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_Format<tb_FM_PaymentRecordDetail>(c => c.SourceBizType, CustomFormatType.EnumOptions, null, typeof(BizType));
            listCols.SetCol_Format<tb_FM_PaymentRecordDetail>(c => c.LocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_PaymentRecordDetail>(c => c.ForeignAmount, CustomFormatType.CurrencyFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);

            //  listCols.SetCol_Formula<tb_FM_PaymentRecordDetail>((a, b) => a.UnitPrice * b.Quantity, c => c.LocalPayableAmount);//-->成交价是结果列
            listCols.SetCol_Summary<tb_FM_PaymentRecordDetail>(c => c.ForeignAmount);
            listCols.SetCol_Summary<tb_FM_PaymentRecordDetail>(c => c.LocalAmount);


            sgd.GridMasterData = EditEntity;

            //应该只提供一个结构
            List<tb_FM_PaymentRecordDetail> lines = new List<tb_FM_PaymentRecordDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;



            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_PaymentRecordDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            switch (PaymentType)
            {
                case ReceivePaymentType.收款:
                    lblBillText.Text = "收款单";
                    lblAccount_id.Text = "收款账号";
                    //lblPaymentDate.Text = "收款日期";
                    lblCustomerVendor_ID.Text = "付款单位";
                    btnInfo.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    cmbPayeeInfoID.Visible = false;
                    //kryptonGroupBox收款账号信息.Visible = false;

                    break;
                case ReceivePaymentType.付款:
                    lblBillText.Text = "付款单";
                    lblAccount_id.Text = "付款账号";
                    //lblPaymentDate.Text = "付款日期";
                    lblCustomerVendor_ID.Text = "收款单位";
                    btnInfo.Visible = true;
                    lblPayeeInfoID.Visible = true;
                    cmbPayeeInfoID.Visible = true;
                    //kryptonGroupBox收款账号信息.Visible = true;
                    break;
                default:
                    break;
            }

        }
        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {

            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {

                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_FM_PaymentRecordDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_PaymentRecordDetail>;
                //红冲时就是负数
                details = details.Where(c => c.LocalAmount != 0 || c.ForeignAmount != 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                EditEntity.TotalForeignAmount = details.Sum(c => c.ForeignAmount);
                EditEntity.TotalLocalAmount = details.Sum(c => c.LocalAmount);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }
        private void Sgh_OnAddDataRow(object rowObj)
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
    }
}
