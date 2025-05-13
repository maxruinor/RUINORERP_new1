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
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.CommService;
using ZXing.Common;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("采购订单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购订单)]
    public partial class UCPurOrder : BaseBillEditGeneric<tb_PurOrder, tb_PurOrderDetail>
    {
        public UCPurOrder()
        {
            InitializeComponent();


        }

        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_PurOrder, actionStatus);
        }
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {

            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_PurOrder>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
        }

        public override void BindData(tb_PurOrder entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.PurOrder_ID > 0)
            {
                entity.PrimaryKeyID = entity.PurOrder_ID;
                entity.ActionStatus = ActionStatus.加载;
                if (entity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    lblExchangeRate.Visible = true;
                    txtExchangeRate.Visible = true;
                    UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, true);

                }
                else
                {
                    lblExchangeRate.Visible = false;
                    txtExchangeRate.Visible = false;
                    UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
                }
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.PurOrderNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购订单);
                entity.PurDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_PurOrderDetails != null && entity.tb_PurOrderDetails.Count > 0)
                {
                    entity.tb_PurOrderDetails.ForEach(c => c.PurOrder_ID = 0);
                    entity.tb_PurOrderDetails.ForEach(c => c.PurOrder_ChildID = 0);
                }
                if (AppContext.BaseCurrency != null)
                {
                    entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                }
             
                lblExchangeRate.Visible = false;
                txtExchangeRate.Visible = false;
                UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
            }
            DataBindingHelper.BindData4CmbByEnum<tb_PurOrder>(entity, k => k.PayStatus, typeof(PayStatus), cmbPayStatus, false);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            //不是业务，不用指定组
            //if (AppContext.projectGroups != null && AppContext.projectGroups.Count > 0)
            //{
            //    #region 项目组
            //    cmbProjectGroup.DataSource = null;
            //    cmbProjectGroup.DataBindings.Clear();
            //    BindingSource bs = new BindingSource();
            //    bs.DataSource = AppContext.projectGroups;
            //    ComboBoxHelper.InitDropList(bs, cmbProjectGroup, "ProjectGroup_ID", "ProjectGroupName", ComboBoxStyle.DropDownList, false);
            //    var depa = new Binding("SelectedValue", entity, "ProjectGroup_ID", true, DataSourceUpdateMode.OnValidation);
            //    //数据源的数据类型转换为控件要求的数据类型。
            //    depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //    //将控件的数据类型转换为数据源要求的数据类型。
            //    depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //    cmbProjectGroup.DataBindings.Add(depa);
            //    #endregion
            //}
            //else
            //{
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            //}

            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.PurOrderNo, txtPurOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.PurDate, dtpPurDate, false);
            DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ShippingCost.ToString(), txtShippingCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);

            //到货日期 是入库单的时间写回 逻辑后面再定
            //            DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.Arrival_date, dtpArrival_date, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignDeposit.ToString(), txtForeignDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.RefNO, txtRefNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_PurOrder>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_PurOrder>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, EditEntity);

            if (entity.tb_PurOrderDetails != null && entity.tb_PurOrderDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_PurOrderDetail>(grid1, sgd, entity.tb_PurOrderDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_PurOrderDetail>(grid1, sgd, new List<tb_PurOrderDetail>(), c => c.ProdDetailID);
            }

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
                                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID )
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

                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.Paytype_ID) && entity.Paytype_ID.HasValue && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            EditEntity.tb_projectgroup = ProjectGroup;
                        }
                    }
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.PreDeliveryDate))
                {
                    if (EditEntity.PreDeliveryDate.HasValue)
                    {
                        PreDeliveryDate = EditEntity.PreDeliveryDate.Value;
                        //预交日期来自于主表
                        listCols.SetCol_DefaultValue<tb_PurOrderDetail>(c => c.PreDeliveryDate, PreDeliveryDate);
                        if (entity.tb_PurOrderDetails != null)
                        {
                            entity.tb_PurOrderDetails.ForEach(c => c.PreDeliveryDate = PreDeliveryDate);
                            sgh.SetCellValue<tb_PurOrderDetail>(sgd, colNameExp => colNameExp.PreDeliveryDate, PreDeliveryDate);
                        }
                    }
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.ShippingCost))
                {
                    EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShippingCost;

                    if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID )
                    {
                        EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                        //
                        EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                    }
                }

                //如果客户有变化，带出对应有业务员
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            if (cv.Employee_ID.HasValue)
                            {
                                EditEntity.Employee_ID = cv.Employee_ID.Value;
                            }
                        }
                    }
                }
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


            };

            ShowPrintStatus(lblPrintStatus, EditEntity);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_PurOrderValidator>(), kryptonSplitContainer1.Panel1.Controls);
                //  base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();

        DateTime PreDeliveryDate = System.DateTime.Now;

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_PurOrderDetail, InventoryInfo>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.PurOrder_ChildID);
            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.PurOrder_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
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
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.IncludingTax);
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.DeliveredQuantity);
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.TotalReturnedQty);

            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);


            //https://blog.csdn.net/m0_46426259/article/details/120265783  格式化
            
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.SubtotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.Cost);
                //listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.SubtotalCostAmount);
                //listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.SubtotalPirceAmount);
            }
            */

            //listCols.SetCol_Summary<tb_PurOrderDetail>(c => c.Quantity);
            //listCols.SetCol_Summary<tb_PurOrderDetail>(c => c.TransactionPrice);
            //listCols.SetCol_Summary<tb_PurOrderDetail>(c => c.SubtotalAmount);
            //listCols.SetCol_Summary<tb_PurOrderDetail>(c => c.TaxAmount);

            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_PurOrderDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_PurOrderDetail>(item);
                }
            }

      
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => a.UnitPrice  * c.Quantity, c => c.SubtotalAmount);
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => a.SubtotalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);

            listCols.SetCol_FormulaReverse<tb_PurOrderDetail>(d => d.UnitPrice == 0, (a, b) => a.SubtotalAmount / b.Quantity, c => c.UnitPrice);//-->成交价是结果列


            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurOrderDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurOrderDetail>(sgd, f => f.VendorModelCode, t => t.VendorModelCode);

            //应该只提供一个结构
            List<tb_PurOrderDetail> lines = new List<tb_PurOrderDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.SetDependencyObject<ProductSharePart, tb_PurOrderDetail>(MainForm.Instance.list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_PurOrderDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
            lblExchangeRate.Visible = false;
            txtExchangeRate.Visible = false;
        }

        /// <summary>
        /// 这里实现的是价格历史自动给值
        /// </summary>
        /// <param name="_View_ProdDetail"></param>
        /// <param name="rowObj"></param>
        /// <param name="griddefine"></param>
        /// <param name="Position"></param>
        private void Sgh_OnLoadRelevantFields(object _View_ProdDetail, object rowObj, SourceGridDefine griddefine, Position Position)
        {
            if (EditEntity == null)
            {
                return;
            }
            View_ProdDetail vp = (View_ProdDetail)_View_ProdDetail;
            tb_PurOrderDetail _SDetail = (tb_PurOrderDetail)rowObj;
            //通过产品查询页查出来后引过来才有值，如果直接在输入框输入SKU这种唯一的。就没有则要查一次。这时是缓存了？
            if (vp.ProdDetailID > 0 && EditEntity.Employee_ID > 0)
            {
                tb_PriceRecord pr = MainForm.Instance.AppContext.Db.Queryable<tb_PriceRecord>().Where(a => a.Employee_ID == EditEntity.Employee_ID && a.ProdDetailID == vp.ProdDetailID).Single();
                if (pr != null && _SDetail != null)
                {
                    _SDetail.UnitPrice = pr.PurPrice;
                    var Col = griddefine.grid.Columns.GetColumnInfo(griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_PurOrderDetail.UnitPrice)).UniqueId);
                    if (Col != null)
                    {
                        griddefine.grid[Position.Row, Col.Index].Value = _SDetail.UnitPrice;
                    }

                }
            }
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
                List<tb_PurOrderDetail> details = new List<tb_PurOrderDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_PurOrderDetail bOM_SDetail = mapper.Map<tb_PurOrderDetail>(item);
                    if (EditEntity.PreDeliveryDate.HasValue)
                    {
                        bOM_SDetail.PreDeliveryDate = EditEntity.PreDeliveryDate.Value;
                    }
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_PurOrderDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_PurOrderDetail> details = sgd.BindingSourceLines.DataSource as List<tb_PurOrderDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                //SetCol_Summary 只计算了简单的小计乘法，复杂的暂时在这里处理
                /*
                tb_PurOrderDetail rowObj = _rowObj as tb_PurOrderDetail;
                SourceGridDefineColumnItem colTaxAmount = myGridDefine.GetColumnDefineInfo<tb_PurOrderDetail>(c => c.TaxAmount);
                if (colTaxAmount != null)
                {
                    if (rowObj.TaxRate.Value == 0)
                    {
                        rowObj.TaxAmount = 0;
                    }
                    else
                    {
                        rowObj.TaxAmount = rowObj.TransactionPrice / (1 + rowObj.TaxRate.Value);
                        //保存两位小数
                        rowObj.TaxAmount = Math.Round(rowObj.TaxAmount.Value, 2);
                    }

                    myGridDefine.grid[position.Row, colTaxAmount.ColIndex].Value = ReflectionHelper.GetPropertyValue(rowObj, colTaxAmount.ColName);
                    //如果税额大于0则主明标记和明细标记都是含税
                    var colIncludingTax = myGridDefine.GetColumnDefineInfo<tb_PurOrderDetail>(c => c.IncludingTax);
                    if (rowObj.TaxAmount > 0)
                    {
                        EditEntity.IsIncludeTax = true;
                        myGridDefine.grid[position.Row, colIncludingTax.ColIndex].Value = true;
                    }
                    else
                    {
                        myGridDefine.grid[position.Row, colIncludingTax.ColIndex].Value = false;
                    }
                }
                */

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                //EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => c.SubtotalAmount);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShippingCost;
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

        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            List<tb_PurOrderDetail> details = new List<tb_PurOrderDetail>();
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);
            bindingSourceSub.EndEdit();

            //如果订单 选择了未付款，但是又选择了非账期的即实收账方式。则审核不通过。
            //如果订单选择了 非未付款，但又选择了账期也不能通过。
            if (NeedValidated && EditEntity.Paytype_ID.HasValue)
            {
                if (EditEntity.PayStatus == (int)PayStatus.未付款)
                {
                    var paytype = EditEntity.Paytype_ID.Value;
                    var paymethod = BizCacheHelper.Instance.GetEntity<tb_PaymentMethod>(EditEntity.Paytype_ID.Value);
                    if (paymethod != null && paymethod.ToString() != "System.Object")
                    {
                        if (paymethod is tb_PaymentMethod pm)
                        {

                            if (pm.Cash || pm.Paytype_Name != DefaultPaymentMethod.账期.ToString())
                            {
                                MessageBox.Show("未付款时，【付款方式】错误,请选择【账期】。");
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    //付过时不能选账期  要选部分付款时使用的方式
                    var paytype = EditEntity.Paytype_ID.Value;
                    var paymethod = BizCacheHelper.Instance.GetEntity<tb_PaymentMethod>(EditEntity.Paytype_ID.Value);
                    if (paymethod != null && paymethod.ToString() != "System.Object")
                    {
                        if (paymethod is tb_PaymentMethod pm)
                        {
                            //如果是账期，但是又选择的是非 未付款
                            if (pm.Paytype_Name == DefaultPaymentMethod.账期.ToString())
                            {
                                MessageBox.Show("【付款方式】错误,全部付款或部分付款时，请选择【付款方式】时使用的方式。");
                                return false;
                            }
                        }
                    }
                }
            }

            List<tb_PurOrderDetail> detailentity = bindingSourceSub.DataSource as List<tb_PurOrderDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

                EditEntity.tb_PurOrderDetails = details;
                foreach (var item in details)
                {
                    item.tb_purorder = EditEntity;
                }
                if (EditEntity.TotalTaxAmount > 0)
                {
                    EditEntity.IsIncludeTax = true;
                }
                else
                {
                    EditEntity.IsIncludeTax = false;
                }
                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_PurOrderDetail>(details))
                {
                    return false;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => c.SubtotalAmount);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShippingCost;

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (NeedValidated)
                {
                    if (EditEntity.tb_PurEntries != null && EditEntity.tb_PurEntries.Count > 0)
                    {
                        MessageBox.Show("当前订单已有采购入库数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                ReturnMainSubResults<tb_PurOrder> SaveResult = new ReturnMainSubResults<tb_PurOrder>();
                if (NeedValidated)
                {
                    if (EditEntity.TotalTaxAmount > 0)
                    {
                        EditEntity.IsIncludeTax = true;
                    }
                    else
                    {
                        EditEntity.IsIncludeTax = false;
                    }

                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PurOrderNo}。");
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



        protected async override Task<bool> Submit()
        {
            if (!base.Validator(EditEntity))
            {
                return false;
            }
            bool rs = await base.Submit();
            return rs;
        }


        tb_PurOrderController<tb_PurOrder> ctr = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();




        /// <summary>
        /// 结案
        /// </summary>
        protected override async Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }

            //要审核过，并且通过了，才能结案。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.未审核 && EditEntity.DataStatus == (int)DataStatus.确认)
            {
                MainForm.Instance.uclog.AddLog("已经审核的单据才能结案。");
                return false;
            }
            if (EditEntity.tb_PurOrderDetails == null || EditEntity.tb_PurOrderDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return false;
            }

            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_PurOrder>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommBillData cbd = bcf.GetBillData<tb_PurOrder>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                RevertCommand command = new RevertCommand();
                tb_PurOrder oldobj = CloneHelper.DeepCloneObject<tb_PurOrder>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    CloneHelper.SetValues<tb_PurOrder>(EditEntity, oldobj);
                };
                List<tb_PurOrder> _PurOrders = [EditEntity];
                ReturnResults<bool> returnResults = await ctr.BatchCloseCaseAsync(_PurOrders);
                if (returnResults.Succeeded)
                {

                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);

                    //审核成功
                    base.ToolBarEnabledControl(MenuItemEnums.结案);
                    toolStripbtnReview.Enabled = true;

                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    MainForm.Instance.PrintInfoLog($"{EditEntity.PurOrderNo}结案失败,请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}

