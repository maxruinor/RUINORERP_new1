using AutoMapper;
using DevAge.Windows.Forms;
using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BI;
using RUINORERP.UI.Common;
using RUINORERP.UI.MRP.MP;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Report;
using RUINORERP.UI.SysConfig;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using Image = System.Drawing.Image;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("费用报销单", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用报销, BizType.费用报销单)]
    public partial class UCExpenseClaim : BaseBillEditGeneric<tb_FM_ExpenseClaim, tb_FM_ExpenseClaimDetail>, IToolStripMenuInfoAuth
    {
        public UCExpenseClaim()
        {
            InitializeComponent();
        }
        public override void AddExcludeMenuList()
        {
            //通过付款单来联动结案
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.作废);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
        }


        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            //加载关联的单据
            if (base.EditEntity is tb_FM_ExpenseClaim expenseClaim)
            {
                //如果有出库，则查应收
                if (expenseClaim.DataStatus >= (int)DataStatus.确认)
                {
                    var receivablePayables = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                    .Where(c => c.ARAPStatus >= (int)ARAPStatus.待审核
                                                                        && c.SourceBizType == (int)BizType.费用报销单
                                                                    && c.SourceBillId == expenseClaim.ClaimMainID)
                                                                    .ToListAsync();
                    foreach (var item in receivablePayables)
                    {
                        var rqp = new Model.CommonModel.RelatedQueryParameter();
                        if (item.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {
                            rqp.bizType = BizType.应付款单;
                        }
                        else
                        {
                            rqp.bizType = BizType.应收款单;
                        }
                        rqp.billId = item.ARAPId;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        if (item.IsForCommission)
                        {
                            RelatedMenuItem.Text = $"{rqp.bizType}[佣金]:{item.ARAPNo}";
                        }
                        else
                        {
                            RelatedMenuItem.Text = $"{rqp.bizType}:{item.ARAPNo}";
                        }

                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.ARAPId.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }
            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }

        public override void BindData(tb_FM_ExpenseClaim entity, ActionStatus actionStatus)
        {

            if (entity == null)
            {
                return;
            }

            EditEntity = entity;
            if (entity.ClaimMainID > 0)
            {
                entity.PrimaryKeyID = entity.ClaimMainID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
                if (entity.DataStatus == (int)DataStatus.完结)
                {
                    cmbPayStatus.Visible = true;
                    lblPayStatus.Visible = true;
                    lblPaytype_ID.Visible = true;
                    cmbPaytype_ID.Visible = true;
                }
                else
                {
                    cmbPayStatus.Visible = false;
                    lblPayStatus.Visible = false;
                    lblPaytype_ID.Visible = false;
                    cmbPaytype_ID.Visible = false;
                }
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                cmbPayStatus.Visible = false;
                lblPayStatus.Visible = false;
                lblPaytype_ID.Visible = false;
                cmbPaytype_ID.Visible = false;

                //默认优化报销自己
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    EditEntity.Employee_ID = entity.Employee_ID;
                }

                entity.DocumentDate = System.DateTime.Now;
                if (string.IsNullOrEmpty(entity.ClaimNo))
                {
                    entity.ClaimNo = ClientBizCodeService.GetBizBillNo(BizType.费用报销单);
                }

                //新增时，默认币别为人民币
                if (cmbCurrency_ID.Items.Count > 1)
                {
                    cmbCurrency_ID.SelectedIndex = 1;//默认第一个人民币
                }
                entity.Currency_ID = MainForm.Instance.AppContext.BaseCurrency.Currency_ID;
            }

            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_ExpenseClaim, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false, PayStatus.全额预付, PayStatus.部分预付);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimNo, txtClaimNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, c => c.Employee_ID.HasValue && c.Employee_ID.Value == entity.Employee_ID);
            DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.DocumentDate, dtpDocumentDate, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimAmount.ToString(), txtClaimlAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ExpenseClaim>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ExpenseClaim>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_ExpenseClaimValidator>(), kryptonPanel1.Controls);
                //UIBaseTool uIBaseTool = new();
                //uIBaseTool.CurMenuInfo = CurMenuInfo;
                //uIBaseTool.AddEditableQueryControl<tb_Employee>(cmbEmployee_ID, false);
                LoadPayeeInfo(entity);
                #region 报销人 可以选择 可以添加
                var lambdaEmp = Expressionable.Create<tb_Employee>()
                                .And(t => t.Is_enabled == true)
                                .ToExpression();
                BaseProcessor baseProcessorEmp = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Employee).Name + "Processor");
                QueryFilter queryFilterEmp = baseProcessorEmp.GetQueryFilter();
                queryFilterEmp.FilterLimitExpressions.Add(lambdaEmp);
                DataBindingHelper.InitFilterForControlByExpCanEdit<tb_Employee>(entity, cmbEmployee_ID, c => c.Employee_Name, queryFilterEmp, true);

                #endregion
            }
            else
            {
                //加载收款信息
                if (entity.PayeeInfoID > 0)
                {
                    //cmbPayeeInfoID.SelectedIndex = cmbPayeeInfoID.FindStringExact(emp.Account_name);
                    var obj = _cacheManager.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_FM_PayeeInfo cv)
                        {
                            //添加收款信息。展示给财务看
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
            }
            if (entity.tb_FM_ExpenseClaimDetails != null && entity.tb_FM_ExpenseClaimDetails.Count > 0)
            {
                //新建和草稿时子表编辑也可以保存。
                foreach (var item in entity.tb_FM_ExpenseClaimDetails)
                {
                    item.PropertyChanged += (sender, s1) =>
                    {
                        //权限允许
                        if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                        {
                            EditEntity.ActionStatus = ActionStatus.修改;
                        }
                    };
                }
                sgh.LoadItemDataToGrid<tb_FM_ExpenseClaimDetail>(grid1, sgd, entity.tb_FM_ExpenseClaimDetails, c => c.ClaimSubID);
                // 模拟按下 Tab 键
                SendKeys.Send("{TAB}");//为了显示远程图片列
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_ExpenseClaimDetail>(grid1, sgd, new List<tb_FM_ExpenseClaimDetail>(), c => c.ClaimSubID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    EditEntity.ActionStatus = ActionStatus.修改;
                }
                if (entity.Employee_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ExpenseClaim>(c => c.Employee_ID))
                {
                    LoadPayeeInfo(entity);
                }

                //如果报销人有变化，带出对应的收款方式
                if (entity.PayeeInfoID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ExpenseClaim>(c => c.PayeeInfoID))
                {
                    var obj = _cacheManager.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_FM_PayeeInfo cv)
                        {

                            //添加收款信息。展示给财务看
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



            };

            //显示 打印状态 如果是草稿状态 不显示打印
            if ((DataStatus)EditEntity.DataStatus != DataStatus.草稿)
            {
                toolStripbtnPrint.Enabled = true;
                if (EditEntity.PrintStatus == 0)
                {
                    lblPrintStatus.Text = "未打印";
                }
                else
                {
                    lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
                }
            }
            else
            {
                toolStripbtnPrint.Enabled = false;
            }
            //显示结案凭证图片
            LoadImageData(entity.CloseCaseImagePath);
            base.BindData(entity);
        }


        private void LoadPayeeInfo(tb_FM_ExpenseClaim entity)
        {
            cmbPayeeInfoID.DataBindings.Clear();
            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, c => c.Employee_ID.HasValue && c.Employee_ID.Value == entity.Employee_ID);
            #region  收款信息可以根据报销人带出 ，并且可以添加

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_PayeeInfo>()
                            .And(t => t.Is_enabled == true)
                            .And(t => t.Employee_ID == entity.Employee_ID)//限制了只能处理自己 的收款信息
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterC, true);

            #endregion
        }

        private async Task LoadImageData(string CloseCaseImagePath)
        {
            if (!string.IsNullOrWhiteSpace(CloseCaseImagePath))
            {
                // 检查是否为多图片路径（包含分号分隔的多个路径）
                if (CloseCaseImagePath.Contains(";"))
                {
                    // 启用多图片支持模式
                    magicPictureBox1.MultiImageSupport = true;
                    magicPictureBox1.ImagePaths = CloseCaseImagePath;
                }
                else
                {
                    // 单图片模式
                    magicPictureBox1.MultiImageSupport = false;

                    //try
                    //{
                    //    var image = await DownloadImageAsync(
                    //        CloseCaseImagePath, 
                    //        MainForm.Instance.AppContext);

                    //    if (image != null)
                    //    {
                    //        magicPictureBox1.Image = image;
                    //        magicPictureBox1.Visible = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                    //}
                }
            }
            else
            {
                magicPictureBox1.Visible = false;
            }
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ExpenseClaim).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_ExpenseClaim>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                            //报销人员限制，财务不限制 自己的只能查自己的
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }

        private void Grid1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        tb_FM_OtherExpenseDetailController<tb_FM_ExpenseClaimDetail> dc = Startup.GetFromFac<tb_FM_OtherExpenseDetailController<tb_FM_ExpenseClaimDetail>>();
        List<tb_FM_ExpenseClaimDetail> list = new List<tb_FM_ExpenseClaimDetail>();
        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UCStockIn_Load(object sender, EventArgs e)
        {
            MainForm.Instance.LoginWebServer();
            if (CurMenuInfo != null)
            {
                lbl盘点单.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();


            // 为结案凭证图片控件添加双击事件，支持上传图片
            magicPictureBox1.DoubleClick += MagicPictureBox1_DoubleClick;

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_ExpenseClaimDetail>();
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.ClaimMainID);
            listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.ClaimSubID);
            listCols.SetCol_DefaultValue<tb_FM_ExpenseClaimDetail>(c => c.TaxRate, 0.00);
            listCols.SetCol_DefaultValue<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount, 0.00M);

            listCols.SetCol_ReadOnly<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount);
            listCols.SetCol_ReadOnly<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.SingleAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount, CustomFormatType.CurrencyFormat);
            //            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImage, CustomFormatType.Image);
            listCols.SetCol_Format<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImagePath, CustomFormatType.WebPathImage);

            listCols.SetCol_DataFilter<tb_FM_ExpenseClaimDetail, tb_FM_ExpenseType>(c => c.ExpenseType_id,
                 DataFilter<tb_FM_ExpenseType>.Where(p => p.EXPOrINC == true)
                 );

            sgd = new SourceGridDefine(grid1, listCols, true);

            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.SubtotalPirceAmount);
            }*/


            //listCols.SetCol_NeverVisible<tb_FM_ExpenseClaimDetail>(c => c.EvidenceImage);//后面会删除这一列
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.SingleAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_FM_ExpenseClaimDetail>(c => c.UntaxedAmount);

            listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b, c) => a.SingleAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);
            listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.SingleAmount - b.TaxAmount, c => c.UntaxedAmount);

            ////反算成交单价，目标列能重复添加。已经优化好了。
            //listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.SubtotalAmount / b.Quantity, c => c.TransactionPrice);//-->成交价是结果列
            ////反算折扣
            //listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.TransactionPrice / b.UnitPrice, c => c.Discount);
            //listCols.SetCol_Formula<tb_FM_ExpenseClaimDetail>((a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);

            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);



            //应该只提供一个结构
            List<tb_FM_ExpenseClaimDetail> lines = new List<tb_FM_ExpenseClaimDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_ExpenseClaimDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

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
            try
            {

                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_FM_ExpenseClaimDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_ExpenseClaimDetail>;
                details = details.Where(c => c.SingleAmount != 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("单项金额必须大于0");
                    return;
                }
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.ClaimAmount = details.Sum(c => c.SingleAmount);
                EditEntity.UntaxedAmount = details.Sum(C => C.UntaxedAmount);

                //添加总计金额小于0的提示
                if (EditEntity.ClaimAmount < 0)
                {
                    MainForm.Instance.uclog.AddLog("警告：报销费用总计小于0，请调整明细金额！", Global.UILogType.警告);
                }

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
            foreach (tb_FM_ExpenseClaimDetail detail in EditEntity.tb_FM_ExpenseClaimDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_ExpenseClaimDetail).GetProperties();
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

        List<tb_FM_ExpenseClaimDetail> details = new List<tb_FM_ExpenseClaimDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtClaimlAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_ExpenseClaimDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_ExpenseClaimDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.SingleAmount != 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("明细记录中，【单项金额】不能为零，请录入有效记录！");
                    return false;
                }

                if (details.Sum(c => c.TaxAmount) > 0)
                {
                    EditEntity.IncludeTax = true;
                }

                EditEntity.tb_FM_ExpenseClaimDetails = details;
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.ClaimAmount = details.Sum(c => c.SingleAmount);
                EditEntity.UntaxedAmount = details.Sum(C => C.UntaxedAmount);

                //如果主表的总金额和明细金额加总后不相等，则提示
                if (NeedValidated && EditEntity.ClaimAmount != details.Sum(c => c.SingleAmount))
                {
                    if (MessageBox.Show("总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (NeedValidated && EditEntity.UntaxedAmount != details.Sum(c => c.UntaxedAmount))
                {
                    if (MessageBox.Show("未税总金额和明细未税金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (NeedValidated && EditEntity.ClaimAmount != details.Sum(c => c.SingleAmount))
                {
                    if (MessageBox.Show("核准总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (NeedValidated && EditEntity.ClaimAmount < 0)
                {
                    //总计金额不能为负数，强制不允许保存
                    MessageBox.Show("报销费用总计不能小于0，请调整明细金额！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_ExpenseClaimDetail>(details))
                {
                    return false;
                }

                // 处理结案凭证图片上传
                if (NeedValidated && magicPictureBox1.Image != null)
                {
                    string fileName = $"CloseCase_{EditEntity.ClaimNo}_{DateTime.Now:yyyyMMddHHmmss}.png";
                    string fileId = "";// await UploadCloseCaseImage(magicPictureBox1.Image, fileName);
                    if (!string.IsNullOrEmpty(fileId))
                    {
                        EditEntity.CloseCaseImagePath = fileId;
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("结案凭证图片上传失败。", Global.UILogType.错误);
                        // 根据业务需求决定是否阻止保存
                        // return false;
                    }
                }

                if (NeedValidated)
                {//处理图片
                    bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_ExpenseClaimDetails);
                    if (uploadImg)
                    {
                        ////更新图片名后保存到数据库
                        //int ImgCounter = await MainForm.Instance.AppContext.Db.Updateable<tb_FM_ExpenseClaimDetail>(EditEntity.tb_FM_ExpenseClaimDetails)
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

                ReturnMainSubResults<tb_FM_ExpenseClaim> SaveResult = new ReturnMainSubResults<tb_FM_ExpenseClaim>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        EditEntity.AcceptChanges();
                        EditEntity.tb_FM_ExpenseClaimDetails.ForEach(c => c.AcceptChanges());

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ClaimNo}。");
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
            bool rs = await base.Submit();
            if (rs)
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

        public override async Task<bool> DeleteRemoteImages()
        {

            if (EditEntity == null || EditEntity.tb_FM_ExpenseClaimDetails == null)
            {
                return false;
            }

            #region 删除主图的结案图。一般没有结案是没有的。结案就不会有结案图了。也有特殊情况。


            #endregion

            bool result = true;
            foreach (tb_FM_ExpenseClaimDetail detail in EditEntity.tb_FM_ExpenseClaimDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_ExpenseClaimDetail).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            if (detail.GetPropertyValue(prop.Name) != null
                                && detail.GetPropertyValue(prop.Name).ToString().Contains("-"))
                            {
                                string imageNameValue = detail.GetPropertyValue(prop.Name).ToString();
                                //比较是否更新了图片数据
                                //old_new 无后缀文件名
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = new SourceGrid.Cells.Models.ValueImageWeb();
                                valueImageWeb.CellImageHashName = imageNameValue;
                                string oldfileName = valueImageWeb.GetOldRealfileName();
                                string newfileName = valueImageWeb.GetNewRealfileName();
                                string TempFileName = string.Empty;
                                //fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                //保存在本地临时目录 删除
                                if (System.IO.File.Exists(TempFileName))
                                {
                                    System.IO.File.Delete(TempFileName);
                                }
                                //上传到服务器，删除本地
                                // 使用ImageManagementHelper替代HttpWebService
                                //bool deleteResult = await UI.Common.ImageManagementHelper.DeleteImageAsync(
                                //    newfileName, 
                                //    MainForm.Instance.AppContext);
                                //MainForm.Instance.PrintInfoLog(deleteResult ? "Success" : "Failed");
                            }
                        }
                    }

                }
            }
            return result;
        }

        protected async override Task<ReturnResults<tb_FM_ExpenseClaim>> Delete()
        {
            ReturnResults<tb_FM_ExpenseClaim> rss = new ReturnResults<tb_FM_ExpenseClaim>();
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

                    tb_FM_ExpenseClaimController<tb_FM_ExpenseClaim> ctr = Startup.GetFromFac<tb_FM_ExpenseClaimController<tb_FM_ExpenseClaim>>();
                    bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_FM_ExpenseClaim);
                    if (rs)
                    {
                        //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除", EditEntity);
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

        /// <summary>
        /// 结案凭证图片控件双击事件处理
        /// </summary>
        private void MagicPictureBox1_DoubleClick(object sender, EventArgs e)
        {
            // 如果是多图片模式，使用MagicPictureBox内置的上传功能
            if (magicPictureBox1.MultiImageSupport)
            {
                // MagicPictureBox已经内置了上传功能，无需额外处理
                return;
            }

            // 单图片模式，打开文件选择对话框
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择结案凭证图片";
                openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                        magicPictureBox1.Image = image;
                        magicPictureBox1.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog($"加载图片失败: {ex.Message}", Global.UILogType.错误);
                        MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


    }
}


