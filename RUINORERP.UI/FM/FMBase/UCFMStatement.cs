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
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.Network.Services;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;
using Microsoft.Extensions.Logging;
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using Krypton.Toolkit;
using Netron.GraphLib;
using RUINORERP.UI.SysConfig;
using SourceGrid.Cells.Editors;
using RUINORERP.UI.MRP.MP;
using SourceGrid.Cells.Models;
using RUINORERP.UI.BI;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using System.Configuration;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Model.CommonModel;
using FastReport;
using FastReport.Data;

using StatementType = RUINORERP.Global.EnumExt.StatementType;
using RUINORERP.Business.Cache;

namespace RUINORERP.UI.FM
{

    /// <summary>
    /// 应收应付对账单
    /// </summary>
    [MenuAttrAssemblyInfo("对账单", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.对账管理, BizType.对账单)]
    [SharedIdRequired]
    public partial class UCFMStatement : BaseBillEditGeneric<tb_FM_Statement, tb_FM_StatementDetail>, IPublicEntityObject, IToolStripMenuInfoAuth
    {
        public UCFMStatement()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                AddPublicEntityObject(typeof(ProductSharePart));
            }
        }

        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.新增);
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }
        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_FM_Statement statement)
            {
                #region 付款单

                var PaymentRecords = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PaymentRecord>()
                                                                .Where(c => c.PaymentStatus >= (int)PaymentStatus.待审核
                                                                && c.CustomerVendor_ID == statement.CustomerVendor_ID)
                                                                 .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == statement.StatementId
                                                                 && d.SourceBizType == (int)BizType.对账单))
                                                                .ToListAsync();
                foreach (var item in PaymentRecords)
                {
                    var rqpara = new Model.CommonModel.RelatedQueryParameter();
                    if (item.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        rqpara.bizType = BizType.收款单;
                    }
                    else
                    {
                        rqpara.bizType = BizType.付款单;
                    }
                    rqpara.billId = item.PaymentId;
                    ToolStripMenuItem RelatedMenuItemPara = new ToolStripMenuItem();
                    RelatedMenuItemPara.Name = $"{rqpara.billId}";
                    RelatedMenuItemPara.Tag = rqpara;
                    RelatedMenuItemPara.Text = $"{rqpara.bizType}:{item.PaymentNo}";
                    RelatedMenuItemPara.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.PaymentId.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItemPara);
                    }
                }

                #endregion

                #region 应收应付
                //if (statement.StatementStatus == (int)StatementStatus.部分结算 || statement.StatementStatus == (int)StatementStatus.已结清)
                //{
                var receivablePayables = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                .Where(c => c.ARAPStatus >= (int)ARAPStatus.待审核
                                                                && c.CustomerVendor_ID == statement.CustomerVendor_ID
                                                                && statement.tb_FM_StatementDetails.Any(d => d.ARAPId == c.ARAPId))
                                                                .ToListAsync();
                foreach (var item in receivablePayables)
                {
                    var rqpara = new Model.CommonModel.RelatedQueryParameter();
                    if (item.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        rqpara.bizType = BizType.应收款单;
                    }
                    else
                    {
                        rqpara.bizType = BizType.应付款单;
                    }

                    rqpara.billId = item.ARAPId;
                    ToolStripMenuItem RelatedMenuItemPara = new ToolStripMenuItem();
                    RelatedMenuItemPara.Name = $"{rqpara.billId}";
                    RelatedMenuItemPara.Tag = rqpara;
                    RelatedMenuItemPara.Text = $"{rqpara.bizType}:{item.ARAPNo}";
                    RelatedMenuItemPara.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.ARAPId.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItemPara);
                    }
                    //}
                }
                #endregion


            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }
        /// <summary>
        /// 收付款方式决定对应的菜单功能
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
        }

        public override void BindData(tb_FM_Statement entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {
                return;
            }

            EditEntity = entity;
            if (entity.StatementId > 0)
            {

                //隐藏外币相关
                UIHelper.ControlForeignFieldInvisible<tb_FM_Statement>(this, false);
                if (listCols != null)
                {
                    listCols.SetCol_DefaultHide<tb_FM_StatementDetail>(c => c.ExchangeRate);
                }

                // 注意：不要覆盖基类已设置的状态值，BaseBillEdit.LoadDataToUI中已处理
                // entity.PrimaryKeyID = entity.StatementId; // 移到基类处理
                // entity.ActionStatus = ActionStatus.加载;   // 移到基类处理
                
                //如果审核了，审核要灰色
                LoadPayeeInfo(entity, false);
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.StatementStatus = (int)StatementStatus.草稿;
                //不能覆盖生成时给定的值
                if (entity.ReceivePaymentType == 0)
                {
                    entity.ReceivePaymentType = (int)PaymentType;
                }

                entity.ActionStatus = ActionStatus.新增;


                //到期日期应该是根据对应客户的账期的天数来算
                //entity.DueDate = System.DateTime.Now;
                if (string.IsNullOrEmpty(entity.StatementNo))
                {
                    entity.StatementNo = ClientBizCodeService.GetBizBillNo(BizType.对账单);
                }

                entity.StatementStatus = (int)StatementStatus.草稿;

                // 清空 DataSource（如果适用）
                cmbPayeeInfoID.DataSource = null;
                cmbPayeeInfoID.DataBindings.Clear();
                cmbPayeeInfoID.Items.Clear();
                LoadPayeeInfo(entity, true);
            }

            if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
            {
                lblBillText.Text = "付款对账单";
            }
            else
            {
                lblBillText.Text = "收款对账单";
            }

            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, c => c.CustomerVendor_ID.HasValue && c.CustomerVendor_ID.Value == entity.CustomerVendor_ID);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_Statement>(entity, t => t.StatementStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(StatementStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_Statement>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.StatementNo, txtStatementNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4Label<tb_FM_Statement>(entity, k => k.PamountInWords, lblMoneyUpper, BindDataType4TextBox.Text, true);
            //冗余的
            //DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.StartDate, dtpStartDate, false);
            DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.EndDate, dtpEndDate, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.OpeningBalanceForeignAmount.ToString(), txtOpeningBalanceForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.OpeningBalanceLocalAmount.ToString(), txtOpeningBalanceLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivableForeignAmount.ToString(), txtTotalReceivableForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivableLocalAmount.ToString(), txtTotalReceivableLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPayableForeignAmount.ToString(), txtTotalPayableForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPayableLocalAmount.ToString(), txtTotalPayableLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivedForeignAmount.ToString(), txtTotalReceivedForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivedLocalAmount.ToString(), txtTotalReceivedLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPaidForeignAmount.ToString(), txtTotalPaidForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPaidLocalAmount.ToString(), txtTotalPaidLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ClosingBalanceForeignAmount.ToString(), txtClosingBalanceForeignAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ClosingBalanceLocalAmount.ToString(), txtClosingBalanceLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementValidator>(), kryptonPanel1.Controls);

                #region 收款信息 ，并且可以添加
                LoadPayeeInfo(entity, true);
                #endregion
            }

            InitLoadGrid();

            // 始终设置表格数据源，即使明细数据为空
            details = entity.tb_FM_StatementDetails ?? new List<tb_FM_StatementDetail>();
          

            //新建和草稿时子表编辑也可以保存。
            foreach (var item in details)
            {
                item.PropertyChanged += (sender, s1) =>
                {
                    //权限允许
                    if ((true && entity.StatementStatus == (int)StatementStatus.草稿) ||
                    (true && entity.StatementStatus == (int)StatementStatus.新建))
                    {
                        EditEntity.ActionStatus = ActionStatus.修改;
                    }
                };
            }

            sgh.LoadItemDataToGrid<tb_FM_StatementDetail>(grid1, sgd, details, c => c.ARAPId);



            // 模拟按下 Tab 键
            //SendKeys.Send("{TAB}");//为了显示远程图片列

            UIBizService.SynchronizeColumnOrder(sgd, listCols.Select(c => c.DisplayController).ToList());
            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许
                if ((true && entity.StatementStatus == (int)StatementStatus.草稿)
                || (true && entity.StatementStatus == (int)StatementStatus.新建))
                {
                    EditEntity.ActionStatus = ActionStatus.修改;
                }


                if (entity.PayeeInfoID == null && (entity.StatementStatus == (int)StatementStatus.草稿) || (true && entity.StatementStatus == (int)StatementStatus.新建))
                {
                    LoadPayeeInfo(entity, true);

                    //设置一个默认值 如果有默认收款账号时
                    //如果原来的值，在下拉集合中，则不变。说明是对应厂商的收款信息。如果不是，则看有不有设置默认值
                    var payeeInfoList = cmbPayeeInfoID.Items.CastToList<tb_FM_PayeeInfo>().Where(c => c.PayeeInfoID != -1).ToList();
                    if (!payeeInfoList.Any(c => c.PayeeInfoID == entity.PayeeInfoID))
                    {
                        if (payeeInfoList.FirstOrDefault(c => c.IsDefault) != null)
                        {
                            entity.PayeeInfoID = payeeInfoList.FirstOrDefault(c => c.IsDefault).PayeeInfoID;
                            btnInfo.Tag = payeeInfoList.FirstOrDefault(c => c.IsDefault);
                        }
                    }
                }



                //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    if (PaymentType == ReceivePaymentType.付款)
                    {
                        LoadCustomerVendor(entity);
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_Statement>(c => c.OpeningBalanceLocalAmount))
                    {
                        TotalSum();
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_Statement>(c => c.OpeningBalanceForeignAmount))
                    {
                        TotalSum();
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_Statement>(c => c.PayeeInfoID))
                    {
                        cmbPayeeInfoID.Enabled = true;
                        //加载收款信息
                        if (entity.PayeeInfoID > 0)
                        {
                            tb_FM_PayeeInfo payeeInfo = null;
                            var obj = RUINORERP.Business.Cache.EntityCacheHelper.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
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

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_Statement>(c => c.TotalPayableLocalAmount) || s2.PropertyName == entity.GetPropertyName<tb_FM_Statement>(c => c.ActionStatus))
                    {
                        entity.PamountInWords = entity.tb_FM_StatementDetails.Sum(c => c.IncludedLocalAmount).ToUpperAmount();
                    }
                }

                //到期日期应该是根据对应客户的账期的天数来算
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_Statement>(c => c.CustomerVendor_ID))
                {
                    var obj = RUINORERP.Business.Cache.EntityCacheHelper.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            // entity.DueDate = System.DateTime.Now.AddDays(cv.CreditDays);
                        }
                    }
                }

            // 注意：ToolBarEnabledControl 已在基类 LoadDataToUI 中统一调用，移除重复调用
            // await base.ToolBarEnabledControl(entity);

            };

            #region 应收单 不用显示 收款信息 ，付款时才要显示对方的信息。

            if (PaymentType == ReceivePaymentType.收款)
            {

                btnInfo.Visible = false;
                lblPayeeInfoID.Visible = false;
                cmbPayeeInfoID.Visible = false;

            }
            else
            {
                if (entity.tb_fm_payeeinfo != null)
                {

                    if (!string.IsNullOrEmpty(entity.tb_fm_payeeinfo.PaymentCodeImagePath))
                    {
                        btnInfo.Tag = entity.tb_fm_payeeinfo;
                        btnInfo.Visible = true;
                    }
                    else
                    {
                        btnInfo.Tag = string.Empty;
                        btnInfo.Visible = false;
                    }
                }
            }



            #endregion


            // 注意：打印状态显示已在基类 ShowPrintStatus 方法中统一处理
            // 只保留按钮启用状态的逻辑
            if ((StatementStatus)EditEntity.StatementStatus != StatementStatus.草稿)
            {
                toolStripbtnPrint.Enabled = true;
            }
            else
            {
                toolStripbtnPrint.Enabled = false;
            }

            base.BindData(entity);
        }

        /// <summary>
        /// 加载客户供应商信息
        /// </summary>
        /// <param name="entity"></param>
        private void LoadCustomerVendor(tb_FM_Statement entity)
        {
            // 检查是否需要在UI线程上执行
            if (cmbCustomerVendor_ID.InvokeRequired)
            {
                cmbCustomerVendor_ID.Invoke(new Action(() => LoadCustomerVendor(entity)));
                return;
            }

            // 客户供应商
            Expression<Func<tb_CustomerVendor, bool>> lambda = t => t.IsVendor == true;
            if (PaymentType == ReceivePaymentType.收款)
            {
                lambda = t => t.IsCustomer == true;
            }

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);

            //带过滤的下拉绑定要这样
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);
        }


        public void LoadPayeeInfo(tb_FM_Statement entity, bool NeedEdit)
        {
            // 检查是否需要在UI线程上执行
            if (cmbCustomerVendor_ID.InvokeRequired)
            {
                cmbCustomerVendor_ID.Invoke(new Action(() => LoadPayeeInfo(entity, NeedEdit)));
                return;
            }

            #region 收款信息可以根据往来单位带出 ，并且可以添加

            // 创建表达式 - 使用SqlSugar的Expressionable
            var expressionable = Expressionable.Create<tb_FM_PayeeInfo>()
                            .And(t => t.Is_enabled == true);

            if (entity.CustomerVendor_ID > 0)
            {
                // 使用SqlSugar的Expressionable继续添加条件
                expressionable = expressionable.And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID);
            }

            // 最后转换为表达式
            Expression<Func<tb_FM_PayeeInfo, bool>> lambdaPayeeInfo = expressionable.ToExpression();


            BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
            queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

            // 初始化筛选后的列表
            List<tb_FM_PayeeInfo> filteredList = new List<tb_FM_PayeeInfo>();

            // 优先从缓存获取数据
            var EntityList = EntityCacheHelper.GetEntityList<tb_FM_PayeeInfo>(nameof(tb_FM_PayeeInfo));
            if (EntityList != null && EntityList.Any())
            {
                // 使用完全避免编译的筛选方法
                filteredList = SafeFilterList(EntityList, lambdaPayeeInfo);

                //过滤失败时用原始的缓存数据
                if (filteredList.Count == 0 && filteredList.Count < EntityList.Count)
                {
                    filteredList = EntityList;
                }
            }

            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
            if (NeedEdit)
            {
                DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterPayeeInfo, true);
            }



            #endregion
        }


        private void InitLoadGrid()
        {

            sgd = new SourceGridDefine(grid1, listCols, true);

            sgd.GridMasterData = EditEntity;

            listCols.SetCol_Summary<tb_FM_StatementDetail>(c => c.IncludedForeignAmount);
            listCols.SetCol_Summary<tb_FM_StatementDetail>(c => c.IncludedLocalAmount);
            listCols.SetCol_Summary<tb_FM_StatementDetail>(c => c.RemainingForeignAmount);
            listCols.SetCol_Summary<tb_FM_StatementDetail>(c => c.RemainingLocalAmount);
            listCols.SetCol_Summary<tb_FM_StatementDetail>(c => c.WrittenOffForeignAmount);
            listCols.SetCol_Summary<tb_FM_StatementDetail>(c => c.WrittenOffLocalAmount);


            //应该只提供一个结构
            List<tb_FM_StatementDetail> lines = new List<tb_FM_StatementDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_FM_StatementDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_StatementDetail));
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {

            base.QueryConditionBuilder();
            //创建表达式
            var lambda = Expressionable.Create<tb_FM_Statement>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             //.And(t => t.ReceivePaymentType == (int)PaymentType)
                             .And(t => t.isdeleted == false)
                            //报销人员限制，财务不限制 自己的只能查自己的
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Created_by == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }



        private void Grid1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();

        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCFMStatement_Load(object sender, EventArgs e)
        {
            AddExtendButton(CurMenuInfo);
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

        
            if (CurMenuInfo != null)
            {
                lblBillText.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_StatementDetail>();


            listCols.SetCol_NeverVisible<tb_FM_StatementDetail>(c => c.StatementDetailId);
            listCols.SetCol_NeverVisible<tb_FM_StatementDetail>(c => c.StatementId);
            listCols.SetCol_NeverVisible<tb_FM_StatementDetail>(c => c.IncludedForeignAmount);
            listCols.SetCol_NeverVisible<tb_FM_StatementDetail>(c => c.RemainingForeignAmount);
            listCols.SetCol_NeverVisible<tb_FM_StatementDetail>(c => c.WrittenOffForeignAmount);

            listCols.SetCol_ReadOnly<tb_FM_StatementDetail>(c => c.WrittenOffLocalAmount);
            listCols.SetCol_ReadOnly<tb_FM_StatementDetail>(c => c.RemainingLocalAmount);

            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            //listCols.SetCol_DefaultValue<tb_FM_StatementDetail>(c => c.ForeignPayableAmount, 0.00M);

            //listCols.SetCol_DisplayFormatText<tb_FM_StatementDetail>(c => c.SourceBizType, 1);

            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.ExchangeRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.WrittenOffLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.WrittenOffForeignAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.RemainingForeignAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.RemainingLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.IncludedForeignAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.IncludedLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.ReceivePaymentType, CustomFormatType.EnumOptions, null, typeof(ReceivePaymentType));
            listCols.SetCol_Format<tb_FM_StatementDetail>(c => c.ARAPWriteOffStatus, CustomFormatType.EnumOptions, null, typeof(ARAPWriteOffStatus));

            InitLoadGrid();

            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);

            //隐藏外币相关
            UIHelper.ControlForeignFieldInvisible<tb_FM_Statement>(this, false);
            if (listCols != null)
            {
                listCols.SetCol_DefaultHide<tb_FM_StatementDetail>(c => c.ExchangeRate);
            }
            UIBizService.SynchronizeColumnOrder(sgd, listCols.Select(c => c.DisplayController).ToList());
        }
        #region 坏账处理

        ToolStripButton toolStripButton坏账处理 = new System.Windows.Forms.ToolStripButton();






        #endregion
        private void Sgh_OnAddDataRow(object rowObj)
        {



        }

        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {
            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                TotalSum();
            }

        }


        void TotalSum()
        {

            try
            {
                // 严格按照使用说明书的业务规则计算对账单汇总数据
                List<tb_FM_StatementDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_StatementDetail>;
                details = details.Where(c => c.IncludedLocalAmount != 0 || c.IncludedForeignAmount != 0).ToList();
                //if (details.Count == 0)
                //{
                //    MainForm.Instance.uclog.AddLog("对账金额不能为0");
                //    return;
                //}
                var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementController<tb_FM_Statement>>();
                paymentController.CalculateTotalAmount(EditEntity, details, (ReceivePaymentType)EditEntity.ReceivePaymentType, (StatementType)EditEntity.StatementType);

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }


        /// <summary>
        /// 保存图片到服务器。所有图片都保存到服务器。即使草稿换电脑还可以看到
        /// </summary>
        /// <param name="RemoteSave"></param>
        /// <returns></returns>
        private async Task<bool> SaveImage(bool RemoteSave)
        {
            bool result = true;
            foreach (tb_FM_StatementDetail detail in EditEntity.tb_FM_StatementDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_StatementDetail).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        int realIndex = grid1.Columns.GetColumnInfo(col.UniqueId).Index;
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            //保存图片到本地临时目录，图片数据保存在grid1控件中，所以要循环控件的行，控件真实数据行以1为起始
                            int totalRowsFlag = grid1.RowsCount;
                            if (grid1.HasSummary)
                            {
                                totalRowsFlag--;
                            }
                            for (int i = 1; i < totalRowsFlag; i++)
                            {
                                var model = grid1[i, realIndex].Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

                                if (grid1[i, realIndex].Value == null)
                                {
                                    continue;
                                }
                                string fileName = string.Empty;
                                if (grid1[i, realIndex].Value.ToString().Contains(".jpg") && grid1[i, realIndex].Value.ToString() == detail.GetPropertyValue(prop.Name).ToString())
                                {
                                    fileName = grid1[i, realIndex].Value.ToString();
                                    //  fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                    //if (grid1[i, realIndex].Tag == null && valueImageWeb.CellImageBytes != null)
                                    if (valueImageWeb.CellImageBytes != null)
                                    {
                                        //保存到本地
                                        //if (EditEntity.DataStatus == (int)DataStatus.草稿)
                                        //{
                                        //    //保存在本地临时目录
                                        //    ImageProcessor.SaveBytesAsImage(valueImageWeb.CellImageBytes, fileName);
                                        //    grid1[i, realIndex].Tag = ImageHashHelper.GenerateHash(valueImageWeb.CellImageBytes);
                                        //}
                                        //else
                                        //{
                                        //上传到服务器，删除本地
                                        //实际应该可以直接传二进制数据，但是暂时没有实现，所以先保存到本地，再上传
                                        //ImageProcessor.SaveBytesAsImage(valueImageWeb.CellImageBytes, fileName);
                                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                        UIConfigManager configManager = Startup.GetFromFac<UIConfigManager>();
                                        var upladurl = configManager.GetValue("WebServerUploadUrl");
                                        string uploadRsult = await httpWebService.UploadImageAsyncOK(upladurl, fileName, valueImageWeb.CellImageBytes, "upload");
                                        //string uploadRsult = await HttpHelper.UploadImageAsyncOK("http://192.168.0.99:8080/upload/", fileName, "upload");
                                        if (uploadRsult.Contains("上传成功"))
                                        {
                                            MainForm.Instance.PrintInfoLog(uploadRsult);
                                        }
                                        else
                                        {
                                            MainForm.Instance.PrintInfoLog("请重试！ " + uploadRsult);
                                            MainForm.Instance.LoginWebServer();
                                        }


                                        //}
                                    }
                                }

                                //UploadImage("http://127.0.0.1/upload", "D:/test.jpg", "upload");
                                // string uploadRsult = await HttpHelper.UploadImageAsync(AppContext.WebServerUrl + @"/upload", fileName, "amw");
                                //                            string uploadRsult = await HttpHelper.UploadImage(AppContext.WebServerUrl + @"/upload", fileName, "upload");

                            }


                        }
                    }
                }
            }
            return result;
        }

        List<tb_FM_StatementDetail> details = new List<tb_FM_StatementDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtTotalPaidLocalAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_StatementDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_StatementDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值,这里样品 可能金额为0.也显示。方便对账
                details = detailentity.Where(t => t.IncludedLocalAmount != 0 || t.IncludedForeignAmount != 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                EditEntity.tb_FM_StatementDetails = details;


                //收付款单中的  收款或付款账号中的币别是否与选的币别一致。
                //if (NeedValidated && EditEntity.Currency_ID > 0 && EditEntity.Account_id > 0)
                //{
                //    tb_FM_Account bizcatch = RUINORERP.Business.Cache.EntityCacheHelper.GetEntity<tb_FM_Account>(EditEntity.Account_id);
                //    if (bizcatch != null && bizcatch.Currency_ID != EditEntity.Currency_ID)
                //    {
                //        MessageBox.Show("收付款账号中的币别与当前单据的币别不一致。");
                //        return false;
                //    }
                //}


                //如果主表的总金额和明细金额加总后不相等，则提示
                //if (NeedValidated && EditEntity.TotalPayableLocalAmount != details.Sum(c => c.LocalPayableAmount) + EditEntity.ShippingFee)
                //{
                //    if (MessageBox.Show("总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                //    {
                //        return false;
                //    }
                //}



                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_StatementDetail>(details))
                {
                    return false;
                }
                if (NeedValidated)
                {//处理图片
                    bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_StatementDetails);
                    if (uploadImg)
                    {
                        ////更新图片名后保存到数据库
                        //int ImgCounter = await MainForm.Instance.AppContext.Db.Updateable<tb_FM_StatementDetail>(EditEntity.tb_FM_StatementDetails)
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

                ReturnMainSubResults<tb_FM_Statement> SaveResult = new ReturnMainSubResults<tb_FM_Statement>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.StatementNo}。");
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
            bool result = await Submit(StatementStatus.新建);
            if (result)
            {
                UIConfigManager configManager = Startup.GetFromFac<UIConfigManager>();
                var temppath = configManager.GetValue("WebServerUrl");
                if (string.IsNullOrEmpty(temppath))
                {
                    MainForm.Instance.uclog.AddLog("请先配置图片服务器路径", UILogType.错误);
                }
            }
            return true;
        }



        protected async override Task<ReturnResults<tb_FM_Statement>> Delete()
        {
            ReturnResults<tb_FM_Statement> rss = new ReturnResults<tb_FM_Statement>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (StatementStatus)(EditEntity.GetPropertyValue(typeof(StatementStatus).Name).ToLong());
                if (dataStatus <= StatementStatus.确认)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if ((dataStatus == StatementStatus.新建 || dataStatus == StatementStatus.确认) && !AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                        {
                            rss.ErrorMsg = $"只有创建人才能删除{((StatementStatus)dataStatus).ToString()}的对账单。";
                            MessageBox.Show(rss.ErrorMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_StatementController<tb_FM_Statement> ctr = Startup.GetFromFac<tb_FM_StatementController<tb_FM_Statement>>();
                    bool rs = await ctr.BaseDeleteByNavAsync(EditEntity as tb_FM_Statement);
                    if (rs)
                    {
                        MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_Statement>("删除", EditEntity);
                        //if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        //{
                        //    //MainForm.Instance.logger.Debug($"单据显示中删除:{typeof(T).Name}，主键值：{PKValue.ToString()} "); //如果要生效 要将配置文件中 <add key="log4net.Internal.Debug" value="true " /> 也许是：logn4net.config <log4net debug="false"> 改为true
                        //}
                        // bindingSourceSub.Clear();

                        ////删除远程图片及本地图片
                        ///暂时使用了逻辑删除所以不执行删除远程图片操作。
                        //await DeleteRemoteImages();

                        //提示一下删除成功
                        MainForm.Instance.uclog.AddLog("提示", "删除成功");

                        //加载一个空的显示的UI
                        // bindingSourceSub.Clear();
                        //base.OnBindDataToUIEvent(EditEntity as tb_FM_Statement, ActionStatus.删除);
                        Exit(this);
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("提示", $"已【{((StatementStatus)dataStatus).ToString()}】的对账单无法删除");
                }
            }
            return rss;
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



