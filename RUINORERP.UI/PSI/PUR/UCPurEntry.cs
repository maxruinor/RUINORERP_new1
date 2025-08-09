using AutoMapper;
using Krypton.Toolkit;
using LiveChartsCore.Geo;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cmp;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BusinessService.CalculationService;
using RUINORERP.UI.Common;
using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenTK.Graphics.OpenGL.GL;

namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("采购入库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购入库单)]
    public partial class UCPurEntry : BaseBillEditGeneric<tb_PurEntry, tb_PurEntryDetail>, IPublicEntityObject
    {
        public UCPurEntry()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }



        /// <summary>
        /// 用于其他UI传入的数据载入。并不是刷新数据
        /// </summary>
        /// <param name="Entity"></param>
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_PurEntry, actionStatus);
        }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            // DataBindingHelper.InitDataToCmb<tb_PurOrder>(k => k.PurOrder_ID, v => v.PurOrderNo, cmbPOID);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_PurEntry>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        private PurEntryCoordinator _coordinator;
        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
                //加载关联的单据
                if (base.EditEntity is tb_PurEntry purEntry)
                {
                    if (purEntry.PurOrder_ID.HasValue)
                    {
                        var rqp = new Model.CommonModel.RelatedQueryParameter();
                        rqp.bizType = BizType.采购订单;
                        rqp.billId = purEntry.PurOrder_ID.Value;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{purEntry.PurOrder_NO}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(purEntry.PurOrder_ID.Value.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }

                        if (purEntry.tb_PurEntryRes != null && purEntry.tb_PurEntryRes.Count > 0)
                        {
                            foreach (var item in purEntry.tb_PurEntryRes)
                            {
                                var rqpSub = new Model.CommonModel.RelatedQueryParameter();
                                rqpSub.bizType = BizType.采购退货单;
                                rqpSub.billId = item.PurEntryRe_ID;
                                ToolStripMenuItem RelatedMenuItemSub = new ToolStripMenuItem();
                                RelatedMenuItemSub.Name = $"{rqpSub.billId}";
                                RelatedMenuItemSub.Tag = rqpSub;
                                RelatedMenuItemSub.Text = $"{rqpSub.bizType}:{item.PurEntryReNo}";
                                RelatedMenuItemSub.Click += base.MenuItem_Click;
                                if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(rqpSub.billId.ToString()))
                                {
                                    toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItemSub);
                                }
                            }

                        }

                    //如果有出库，则查应收
                    if (purEntry.DataStatus >= (int)DataStatus.确认)
                    {
                        var receivablePayables = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                        .Where(c => c.ARAPStatus >= (int)ARAPStatus.待审核
                                                                        && c.CustomerVendor_ID == purEntry.CustomerVendor_ID
                                                                        && c.SourceBillId == purEntry.PurEntryID)
                                                                        .ToListAsync();
                        foreach (var item in receivablePayables)
                        {
                            var rqpara = new Model.CommonModel.RelatedQueryParameter();
                            rqpara.bizType = BizType.应付款单;
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
                        }
                    }

                }
                }
              await  base.LoadRelatedDataToDropDownItemsAsync();
            }
        public override void BindData(tb_PurEntry entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.PurEntryID > 0)
            {
                entity.PrimaryKeyID = entity.PurEntryID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.Employee_ID = AppContext.CurUserInfo.UserInfo.Employee_ID;

                if (string.IsNullOrEmpty(entity.PurEntryNo))
                {
                    entity.PurEntryNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购入库单);
                }


                entity.EntryDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_PurEntryDetails != null && entity.tb_PurEntryDetails.Count > 0)
                {
                    entity.tb_PurEntryDetails.ForEach(c => c.PurEntryID = 0);
                    entity.tb_PurEntryDetails.ForEach(c => c.PurEntryDetail_ID = 0);
                }
            }

            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CmbByEnum<tb_PurEntry>(entity, k => k.PayStatus, typeof(PayStatus), cmbPayStatus, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.PurEntryNo, txtPurEntryNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4DataTime<tb_PurEntry>(entity, t => t.EntryDate, dtpEntryDate, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CheckBox<tb_PurEntry>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
            DataBindingHelper.BindData4CheckBox<tb_PurEntry>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
            DataBindingHelper.BindData4CheckBox<tb_PurEntry>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4ControlByEnum<tb_PurEntry>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));

            DataBindingHelper.BindData4ControlByEnum<tb_PurEntry>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //DataBindingHelper.BindData4Cmb<tb_PurOrder>(entity, k => k.PurOrder_ID, v => v.PurOrderNo, cmbPOID);
           
            if (entity.tb_PurEntryDetails != null && entity.tb_PurEntryDetails.Count > 0)
            {
                details = entity.tb_PurEntryDetails;
            }
            sgh.LoadItemDataToGrid<tb_PurEntryDetail>(grid1, sgd, details, c => c.ProdDetailID);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    //根据币别如果是外币才显示外币相关的字段
                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.Currency_ID) && entity.Currency_ID > 0)
                    {
                        if (cmbCurrency_ID.SelectedItem is tb_Currency cv)
                        {
                            if (cv.CurrencyCode.Trim() != DefaultCurrency.RMB.ToString())
                            {
                                //显示外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, true);
                                entity.ExchangeRate = BizService.GetExchangeRateFromCache(cv.Currency_ID, AppContext.BaseCurrency.Currency_ID);
                                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                                {
                                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                                    //
                                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                                }
                                lblExchangeRate.Visible = true;
                                txtExchangeRate.Visible = true;
                                lblForeignTotalAmount.Text = $"金额({cv.CurrencyCode})";
                            }
                            else
                            {
                                //隐藏外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
                                lblExchangeRate.Visible = false;
                                txtExchangeRate.Visible = false;
                                entity.ExchangeRate = 1;
                                entity.ForeignTotalAmount = 0;
                            }
                        }

                    }

               
                    /*

                    if (s2.PropertyName == entity.GetPropertyName<tb_PurEntry>(c => c.ShipCost) && entity.ShipCost > 0)
                    {
                        EditEntity.TotalAmount = EditEntity.tb_PurEntryDetails.Sum(c => (c.CustomizedCost + c.UnitPrice) * c.Quantity);
                        EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                        //分摊成本
                        if (authorizeController == null)
                        {
                            authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                        }


                        #region 分摊成本计算

                        entity.TotalQty = entity.tb_PurEntryDetails.Sum(c => c.Quantity);

                        //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                        if (entity.ShipCost > 0)
                        {
                            //根据系统设置中的分摊规则来分配运费收入到明细。
                            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                            {
                                Expression<Func<tb_PurEntryDetail, object>> colNameExp = c => c.AllocatedFreightCost;
                                string colName = colNameExp.GetMemberInfo().Name;
                                var coltarget = sgh.SGDefine[colName];
                                int colIndex = sgh.SGDefine.grid.Columns.GetColumnInfo(coltarget.UniqueId).Index;


                                // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                                foreach (var item in entity.tb_PurEntryDetails)
                                {
                                    item.AllocatedFreightCost = entity.ShipCost * (item.Quantity / entity.TotalQty);
                                    item.AllocatedFreightCost = item.AllocatedFreightCost.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                                    item.FreightAllocationRules = MainForm.Instance.AppContext.SysConfig.FreightAllocationRules;
                                    for (int i = 0; i < sgh.SGDefine.grid.Rows.Count; i++)
                                    {
                                        if (sgh.SGDefine.grid.Rows[i].Tag != null && sgh.SGDefine.grid.Rows[i].Tag is tb_PurEntryDetail line)
                                        {
                                            if (line.ProdDetailID == item.ProdDetailID)
                                            {
                                                Position position = new Position(i, colIndex);
                                                sgh.SetCellValueForCurrentUICell(coltarget, colName, position, item);
                                                continue;
                                            }
                                        }
                                    }

                                }

                            }
                        }


                        #endregion
                    }
                    */
                }
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.PurOrder_ID.HasValue && entity.PurOrder_ID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_PurEntry>(c => c.PurOrder_ID))
                {
                    try
                    {
                        LoadPurOrder(entity.PurOrder_ID);
                        cmbPayStatus.Enabled = false;
                        cmbPaytype_ID.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog("加载采购订单失败！" + ex.Message, UILogType.错误);
                    }

                }


            };


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_PurEntryValidator>(), kryptonSplitContainer1.Panel1.Controls);
            }
            ShowPrintStatus(lblPrintStatus, EditEntity);



            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, v => v.PurOrder_NO, txtPurOrderNO, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_PurOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaSaleOut = Expressionable.Create<tb_PurOrder>()
         .And(t => t.DataStatus == (int)DataStatus.确认)
         .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
         .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
          .And(t => t.isdeleted == false)
         .ToExpression();
            queryFilter.SetFieldLimitCondition(lambdaSaleOut);
            ControlBindingHelper.ConfigureControlFilter<tb_PurEntry, tb_PurOrder>(entity, txtPurOrderNO, t => t.PurOrder_NO,
                f => f.PurOrderNo, queryFilter, a => a.PurOrder_ID, b => b.PurOrder_ID, null, false);
            _coordinator = new(EditEntity, details, sgh);
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();

        private void UCStockIn_Load(object sender, EventArgs e)
        {

            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_PurEntryDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.PurEntryID);
            listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.PurEntryDetail_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            //订单指定了仓库时。更新了 在途数量 所以这里不能修改了。要改前面订单也要改
            listCols.SetCol_ReadOnly<tb_PurEntryDetail>(c => c.Location_ID);


            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.FreightAllocationRules, CustomFormatType.EnumOptions, null, typeof(FreightAllocationRules));
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.SubtotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.UntaxedUnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.UntaxedCustomizedCost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.SubtotalUntaxedAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurEntryDetail>(c => c.CustomizedCost, CustomFormatType.CurrencyFormat);
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

            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.SubtotalAmount);
            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.SubtotalUntaxedAmount);


            listCols.SetCol_Formula<tb_PurEntryDetail>((a, b, c) => (a.UnitPrice + a.CustomizedCost) * c.Quantity, c => c.SubtotalAmount);
            listCols.SetCol_Formula<tb_PurEntryDetail>((a, b, c) => a.SubtotalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);

            listCols.SetCol_Formula<tb_PurEntryDetail>((a, b, c) => a.UnitPrice / (1 + b.TaxRate), d => d.UntaxedUnitPrice);
            listCols.SetCol_Formula<tb_PurEntryDetail>((a, b, c) => a.CustomizedCost / (1 + b.TaxRate), d => d.UntaxedCustomizedCost);
            listCols.SetCol_Formula<tb_PurEntryDetail>((a, b, c) => (a.UntaxedCustomizedCost + a.UntaxedUnitPrice) * c.Quantity, c => c.SubtotalUntaxedAmount);

            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.AllocatedFreightCost);

            //反算成交价
            listCols.SetCol_FormulaReverse<tb_PurEntryDetail>((a) => a.Quantity != 0, (a, b) => a.SubtotalAmount / b.Quantity, c => c.UnitPrice);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryDetail>(sgd, f => f.VendorModelCode, t => t.VendorModelCode);




            //应该只提供一个结构
            List<tb_PurEntryDetail> lines = new List<tb_PurEntryDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            sgd.SetDependencyObject<ProductSharePart, tb_PurEntryDetail>(MainForm.Instance.View_ProdDetailList);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_PurEntryDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }
        private void Sgh_OnLoadMultiRowData(object rows, Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_PurEntryDetail> details = new List<tb_PurEntryDetail>();

                foreach (var item in RowDetails)
                {
                    tb_PurEntryDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_PurEntryDetail>(item);
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_PurEntryDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_PurEntryDetail> details = sgd.BindingSourceLines.DataSource as List<tb_PurEntryDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                if (_coordinator != null)
                {
                    _coordinator.Master = EditEntity;
                    _coordinator.Details = details;
                }

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                EditEntity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.TotalUntaxedAmount = details.Sum(c => (c.UntaxedCustomizedCost + c.UntaxedUnitPrice) * c.Quantity);
                //EditEntity.ShipCost = details.Sum(c => c.AllocatedFreightCost);
                //不含税的总金额+不含税运费
                decimal UntaxedShippingCost = 0;
                if (EditEntity.ShipCost > 0 && EditEntity.TotalTaxAmount > 0)
                {
                    decimal FreightTaxRate = details.FirstOrDefault(c => c.TaxRate > 0).TaxRate;
                    UntaxedShippingCost = (EditEntity.ShipCost / (1 + FreightTaxRate)); //计算列：不含税运费
                    EditEntity.TotalUntaxedAmount += Math.Round(UntaxedShippingCost, 2);
                }

                //是不是选了外币就通过本币和汇率算一下？
                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                    //
                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                }
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }

        List<tb_PurEntryDetail> details = new List<tb_PurEntryDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            if (NeedValidated && !EditEntity.PurOrder_ID.HasValue || EditEntity.PurOrder_ID.Value == 0)
            {
                MessageBox.Show("请选择正确的采购订单，或从采购订单查询中转为入库单！");
                return false;
            }

            //正常是从订单中转过来。如果直接创建的，则没有订单的。则默认订单的币种
            if (EditEntity.Currency_ID.HasValue == false)
            {
                EditEntity.Currency_ID = MainForm.Instance.AppContext.BaseCurrency.Currency_ID;
            }

            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_PurEntryDetail> detailentity = bindingSourceSub.DataSource as List<tb_PurEntryDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                EditEntity.tb_PurEntryDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (_purorder != null)
                {
                    ///如果入库数量大于订单数据是错误的。
                    if (EditEntity.TotalQty > _purorder.TotalQty)
                    {
                        MainForm.Instance.uclog.AddLog("入库总数量不可能大于订单数量，请检查数据是否正确！PurOrderNo:" + _purorder.PurOrderNo, UILogType.错误);
                        MessageBox.Show("入库总数量不可能大于订单数量，请检查数据是否正确！");
                        return false;
                    }
                }

                //如果所有数据都出库。运费成本也要一致。否则提醒
                if (NeedValidated)
                {
                    if (EditEntity.ShipCost != details.Sum(c => c.AllocatedFreightCost))
                    {
                        _coordinator.HandleMasterPropertyChange(c => c.ShipCost);
                        if (MessageBox.Show($"运费成本{EditEntity.ShipCost}与明细运费成本分摊的总金额{details.Sum(c => c.AllocatedFreightCost)}不一致\r\n" +
                           $"系统已经自动分摊，确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                            return false;
                    }
                }

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                EditEntity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
                //是不是选了外币就通过本币和汇率算一下？
                //默认认为运费含税，税率随明细

                EditEntity.TotalUntaxedAmount = details.Sum(c => (c.UntaxedCustomizedCost + c.UntaxedUnitPrice) * c.Quantity);
                //不含税的总金额+不含税运费
                decimal UntaxedShippingCost = 0;
                if (EditEntity.ShipCost > 0 && EditEntity.TotalTaxAmount > 0)
                {
                    decimal FreightTaxRate = details.FirstOrDefault(c => c.TaxRate > 0).TaxRate;
                    UntaxedShippingCost = (EditEntity.ShipCost / (1 + FreightTaxRate)); //计算列：不含税运费
                    EditEntity.TotalUntaxedAmount += Math.Round(UntaxedShippingCost, 2);
                }

                if (EditEntity.TotalTaxAmount > 0)
                {
                    EditEntity.IsIncludeTax = true;
                }
                else
                {
                    EditEntity.IsIncludeTax = false;
                }

                if (NeedValidated && !base.Validator<tb_PurEntryDetail>(details))
                {
                    return false;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证
                if (NeedValidated)
                {
                    if (EditEntity.tb_PurEntryRes != null && EditEntity.tb_PurEntryRes.Count > 0)
                    {
                        MessageBox.Show("当前采购入库单已有采购入库退回数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                ReturnMainSubResults<tb_PurEntry> SaveResult = new ReturnMainSubResults<tb_PurEntry>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PurEntryNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}，请重试;或联系管理员。", Color.Red);
                    }
                }

                return SaveResult.Succeeded;
            }
            return false;
        }



        AuthorizeController authorizeController = null;
        string orderid = string.Empty;

        tb_PurOrder _purorder;


        /// <summary>
        /// 要统一起来。只做一个方法
        /// </summary>
        /// <param name="purOrderID"></param>
        private async Task LoadPurOrder(long? purOrderID)
        {
            ButtonSpecAny bsa = (txtPurOrderNO as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var purorder = bsa.Tag as tb_PurOrder;
            purorder = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>().Where(c => c.PurOrder_ID == purOrderID)
             .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
             .Includes(a => a.tb_PurEntries)
            .SingleAsync();
            _purorder = purorder;
            //新增时才可以转单
            if (purorder != null)
            {
                orderid = purorder.PurOrder_ID.ToString();

                tb_PurEntry entity = MainForm.Instance.mapper.Map<tb_PurEntry>(purorder);
                List<tb_PurEntryDetail> details = MainForm.Instance.mapper.Map<List<tb_PurEntryDetail>>(purorder.tb_PurOrderDetails);

                entity.EntryDate = System.DateTime.Now;
                entity.ApprovalOpinions = "";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.PayStatus = purorder.PayStatus;
                entity.Paytype_ID = purorder.Paytype_ID;
                List<tb_PurEntryDetail> NewDetails = new List<tb_PurEntryDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].PurOrder_ChildID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_PurOrderDetail item = purorder.tb_PurOrderDetails
                            .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                            && c.Location_ID == details[i].Location_ID
                            && c.PurOrder_ChildID == details[i].PurOrder_ChildID);
                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;
                        details[i].SubtotalUntaxedAmount = (details[i].UntaxedUnitPrice + details[i].UntaxedCustomizedCost) * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{purorder.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一

                        tb_PurOrderDetail item = purorder.tb_PurOrderDetails
                            .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                            && c.Location_ID == details[i].Location_ID
                            );
                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{purorder.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }
                        #endregion
                    }

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{entity.PurOrder_NO}已全部入库，请检查是否正在重复入库！");
                }

                StringBuilder msg = new StringBuilder();
                foreach (var item in tipsMsg)
                {
                    msg.Append(item).Append("\r\n");

                }
                if (tipsMsg.Count > 0)
                {
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (authorizeController == null)
                {
                    authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                }
                #region 分摊成本计算

                entity.TotalQty = NewDetails.Sum(c => c.Quantity);

                //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                if (entity.ShipCost > 0)
                {

                    //根据系统设置中的分摊规则来分配运费收入到明细。

                    if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                    {
                        // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                        foreach (var item in NewDetails)
                        {
                            item.AllocatedFreightCost = entity.ShipCost * (item.Quantity / entity.TotalQty);
                            item.AllocatedFreightCost = item.AllocatedFreightCost.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                            item.FreightAllocationRules = MainForm.Instance.AppContext.SysConfig.FreightAllocationRules;
                        }
                    }
                }


                #endregion

                entity.tb_PurEntryDetails = NewDetails;
                if (entity.Currency_ID==null)
                {
                    entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                }

                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(entity, actionStatus);
            }
        }

        private void lblIsIncludeTax_Click(object sender, EventArgs e)
        {

        }

        private void kryptonSplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
