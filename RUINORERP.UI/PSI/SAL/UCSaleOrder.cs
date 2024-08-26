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
using RUINORERP.Model.QueryDto;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using SqlSugar;
using System.Linq.Expressions;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using NPOI.SS.Formula.Functions;
using RUINORERP.Model.CommonModel;
using SourceGrid;
using RUINORERP.Business.CommService;


namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售订单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.销售管理, BizType.销售订单)]
    public partial class UCSaleOrder : BaseBillEditGeneric<tb_SaleOrder, tb_SaleOrderQueryDto>
    {
        public UCSaleOrder()
        {
            InitializeComponent();
            toolStripButton付款调整.Visible = true;
            InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOrder>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority, false);
            base.OnBindDataToUIEvent += UcSaleOrderEdit_OnBindDataToUIEvent;
        }

        private void UcSaleOrderEdit_OnBindDataToUIEvent(tb_SaleOrder entity)
        {
            BindData(entity as BaseEntity);
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as BaseEntity);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }


        protected async override void UpdatePaymentStatus()
        {
            if (EditEntity == null)
            {
                return;
            }

            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能更新付款状态。");
                return;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
            };

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

            if (MessageBox.Show("你确定要调整当前订单的付款状态吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            ReturnResults<bool> rr = await ctr.UpdatePaymentStatus(EditEntity);
            if (rr.Succeeded)
            {
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);


                base.ToolBarEnabledControl(MenuItemEnums.付款调整);

            }
            else
            {
                //付款调整失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}付款调整失败{rr.ErrorMsg},请联系管理员！", Color.Red);
            }
            base.UpdatePaymentStatus();
        }



        public override void BindData(BaseEntity entityPara)
        {
            tb_SaleOrder entity = entityPara as tb_SaleOrder;
            if (entity == null)
            {
 
                return;
            }

            if (entity != null)
            {
                if (entity.SOrder_ID > 0)
                {
                    entity.PrimaryKeyID = entity.SOrder_ID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    entity.SOrderNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售订单);
                    entity.SaleDate = System.DateTime.Now;
                    entity.IsFromPlatform = true;//默认 平台单为真
                    //entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                }
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.SOrderNo, txtOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, true);


            DataBindingHelper.BindData4CmbByEnum<tb_SaleOrder>(entity, k => k.PayStatus, typeof(PayStatus), cmbPayStatus, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.SaleDate, dtpSaleDate, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShippingWay, txtshippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.CollectedMoney.ToString(), txtCollectedMoney, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PrePayMoney.ToString(), txtPrePayMoney, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_SaleOrder>(entity, k => k.OrderPriority, typeof(Priority), cmbOrderPriority, true);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalUntaxedAmount, txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalTaxAmount, txtTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOrder>(entity, t => t.IsFromPlatform, chk平台单, false);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOrder>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOrder>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_SaleOrderDetails != null && entity.tb_SaleOrderDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_SaleOrderDetails);
                sgh.LoadItemDataToGrid<tb_SaleOrderDetail>(grid1, sgd, entity.tb_SaleOrderDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_SaleOrderDetail>());
                sgh.LoadItemDataToGrid<tb_SaleOrderDetail>(grid1, sgd, new List<tb_SaleOrderDetail>(), c => c.ProdDetailID);
            }


            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null)
                {
                    return;
                }

                if (toolStripButton付款调整.Visible = true && (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.PayStatus) || s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.Paytype_ID)))
                {
                    toolStripButton付款调整.Enabled = true;
                }

                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    entity.ActionStatus = ActionStatus.修改;
                    base.ToolBarEnabledControl(MenuItemEnums.修改);
                }

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }

                //如果客户有变化，带出对应有业务员
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.CustomerVendor_ID))
                {
                    var obj = CacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            EditEntity.Employee_ID = cv.Employee_ID;
                            EditEntity.ShippingAddress = cv.Address;
                        }
                    }
                }


                if (entity.ShipCost > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.ShipCost))
                {
                    EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                    EditEntity.TotalUntaxedAmount = EditEntity.TotalUntaxedAmount + EditEntity.ShipCost;
                    EditEntity.CollectedMoney = EditEntity.TotalAmount;

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

            if (EditEntity.PrintStatus == 0)
            {
                lblPrintStatus.Text = "未打印";
            }
            else
            {
                lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
            }
            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);




            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_SaleOrderValidator(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            ToolBarEnabledControl(entity);
        }

        public void InitDataTocmbbox()
        {
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOrder>(typeof(PayStatus), e => e.PayStatus, cmbPayStatus, false);
        }

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_SaleOrderDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
         并且，表格中可以编辑的需要查询的列是唯一能查到详情的

        SetDependencyObject<P, S, T>   P包含S字段包含T字段--》是有且包含
         */



        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_SaleOrderDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_SaleOrderDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            //InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_SaleOrderDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_SaleOrderDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_SaleOrderDetail>(c => c.SaleOrderDetail_ID);
            listCols.SetCol_NeverVisible<tb_SaleOrderDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            listCols.SetCol_DefaultValue<tb_SaleOrderDetail>(a => a.Discount, 1m);
            ControlChildColumnsInvisible(listCols);
            //listCols.SetCol_DefaultValue<tb_SaleOrderDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            listCols.SetCol_ReadOnly<tb_SaleOrderDetail>(c => c.TotalDeliveredQty);

            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.Discount, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridData = EditEntity;


            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.UnitPrice * b.Discount, c => c.TransactionPrice);//-->成交价是结果列


            //listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);


            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.UnitPrice == 0 && d.Discount != 0 && d.TransactionPrice != 0, (a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);//-->成交价是结果列
            //单价和成交价不一样时，并且单价不能为零时，可以计算出折扣的值 TODO:!!!!
            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.UnitPrice != d.TransactionPrice && d.UnitPrice != 0, (a, b) => a.TransactionPrice / b.UnitPrice, c => c.Discount);//-->单价 
            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.Quantity != 0 && d.SubtotalTransAmount != 0, (a, b) => a.SubtotalTransAmount / b.Quantity, c => c.TransactionPrice);//-->成交价是结果列
            //listCols.SetCol_Formula<tb_SaleOrderDetail>(d => d.Quantity != 0,(a, b) => a.SubtotalTransAmount / b.Quantity, c => c.UnitPrice);//-->成交价是结果列

            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);

            //反算时还要加更复杂的逻辑：如果单价为0时，则可以反算到单价。折扣不变。（默认为1）， 如果单价有值。则反算折扣？成交价？

            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);
            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.SubtotalTaxAmount, d => d.SubtotalUntaxedAmount);
            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.Cost * b.Quantity, c => c.SubtotalCostAmount);

            //listCols.SetCol_Summary<tb_SaleOrderDetail>(c => c.Quantity);
            //listCols.SetCol_Summary<tb_SaleOrderDetail>(c => c.CommissionAmount);

            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_SaleOrderDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_SaleOrderDetail>(item);
                }
            }




            /*
             
        Mathos.Parser.MathParser parser = new Mathos.Parser.MathParser();

        string expr = "(x+(2*x)/(1-x))"; // the expression

        decimal result = 0; // the storage of the result

        parser.LocalVariables.Add("x", 41); // 41 is the value of x

        result = parser.Parse(expr); // parsing

        Console.WriteLine(result); // 38.95
             */

            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_SaleOrderDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.prop, t => t.property);






            //应该只提供一个结构
            List<tb_SaleOrderDetail> lines = new List<tb_SaleOrderDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;
            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                .AndIF(true, w => w.CNName.Length > 0)
               // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
               .ToExpression();//注意 这一句 不能少
                               // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);
            sgd.SetDependencyObject<ProductSharePart, tb_SaleOrderDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_SaleOrderDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;

        }

        private void Sgh_OnLoadRelevantFields(object _View_ProdDetail, object rowObj, SourceGridDefine griddefine, Position Position)
        {
            if (EditEntity == null)
            {
                return;
            }

            View_ProdDetail vp = (View_ProdDetail)_View_ProdDetail;
            tb_SaleOrderDetail _SDetail = (tb_SaleOrderDetail)rowObj;
            //通过产品查询页查出来后引过来才有值，如果直接在输入框输入SKU这种唯一的。就没有则要查一次。这时是缓存了？
            if (vp.ProdDetailID > 0 && EditEntity.Employee_ID.HasValue && EditEntity.Employee_ID.Value > 0)
            {
                tb_PriceRecord pr = MainForm.Instance.AppContext.Db.Queryable<tb_PriceRecord>().Where(a => a.Employee_ID == EditEntity.Employee_ID && a.ProdDetailID == vp.ProdDetailID).Single();
                if (pr != null)
                {
                    _SDetail.UnitPrice = pr.SalePrice;
                    int ColIndex = griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_SaleOrderDetail.UnitPrice)).ColIndex;
                    griddefine.grid[Position.Row, ColIndex].Value = _SDetail.UnitPrice;
                }
            }
        }


        private void Sgh_OnLoadMultiRowData(object rows, SourceGrid.Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_SaleOrderDetail> details = new List<tb_SaleOrderDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_SaleOrderDetail Detail = mapper.Map<tb_SaleOrderDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_SaleOrderDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }

        }

        private void Sgh_OnCalculateColumnValue(object rowObj, SourceGridDefine griddefine, SourceGrid.Position Position)
        {
            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {
                //if (EditEntity.actionStatus == ActionStatus.加载)
                //{
                //    return;
                //}
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_SaleOrderDetail> details = sgd.BindingSourceLines.DataSource as List<tb_SaleOrderDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Quantity);

                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalUntaxedAmount = details.Sum(c => c.SubtotalUntaxedAmount);

                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                EditEntity.CollectedMoney = EditEntity.TotalAmount;
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_SaleOrderDetail> details = new List<tb_SaleOrderDetail>();
        /// <summary>
        /// 查询结果 选中行的变化事件
        /// </summary>
        /// <param name="entity"></param>

        protected async override void Save()
        {

            if (EditEntity == null)
            {
                return;
            }

            if (EditEntity.PlatformOrderNo != null)
            {
                EditEntity.PlatformOrderNo = EditEntity.PlatformOrderNo.Trim();//去空格
            }

            if (!chk平台单.Checked && !string.IsNullOrEmpty(txtPlatformOrderNo.Text))
            {
                //检测平台单号。重复性提示
                tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                tb_SaleOrder saleOrder = ctr.ExistFieldValueWithReturn(c => c.PlatformOrderNo == txtPlatformOrderNo.Text.Trim());
                if (saleOrder != null)
                {
                    string empName = UIHelper.ShowGridColumnsNameValue(typeof(tb_SaleOrder), "Employee_ID", saleOrder.Employee_ID);
                    if (MessageBox.Show($"系统检测到相同平台单号的订单，订单号是：{saleOrder.SOrderNo},业务员是{empName}\r\n确定继续保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return;
                    }
                }

            }


            var eer = errorProviderForAllInput.GetError(txtTotalAmount);

            bindingSourceSub.EndEdit();

            List<tb_SaleOrderDetail> oldOjb = new List<tb_SaleOrderDetail>(details.ToArray());

            List<tb_SaleOrderDetail> detailentity = bindingSourceSub.DataSource as List<tb_SaleOrderDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalUntaxedAmount = details.Sum(c => c.SubtotalUntaxedAmount);
                EditEntity.TotalUntaxedAmount = EditEntity.TotalUntaxedAmount + EditEntity.ShipCost;
                EditEntity.CollectedMoney = EditEntity.TotalUntaxedAmount;
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;

                if (EditEntity.TotalQty == 0 || detailentity.Sum(c => c.Quantity) == 0)
                {
                    MessageBox.Show("单据及明细总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    MessageBox.Show($"单据总数量{EditEntity.TotalQty}和明细总数量{detailentity.Sum(c => c.Quantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //如果没有有效的明细。直接提示
                if (details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                EditEntity.tb_SaleOrderDetails = details;
                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 1)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //为什么这么做呢？
                //foreach (var item in details)
                //{
                //    item.tb_saleorder = EditEntity;
                //}

                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return;
                }
                if (!base.Validator<tb_SaleOrderDetail>(details))
                {
                    return;
                }


                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                if (EditEntity.TotalAmount == 0)
                {
                    if (MessageBox.Show(this, "订单总金额数据为零\r\n你确定吗? ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                if (EditEntity.SOrder_ID > 0)
                {
                    //如果是超级管理员，提供一个保存方式 就是在基本数据行不变时。只更新部分字段
                    if (MainForm.Instance.AppContext.IsSuperUser)
                    {
                        if (MessageBox.Show("确定是部分数据更新吗？\r\n如有删除增加明细！请点【否】", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            ReturnMainSubResults<tb_SaleOrder> UpdateResult = await base.UpdateSave(EditEntity);
                            if (UpdateResult.Succeeded)
                            {
                                MainForm.Instance.PrintInfoLog($"更新成功，{EditEntity.SOrderNo}。");
                            }
                            else
                            {
                                MainForm.Instance.PrintInfoLog($"更新失败，{UpdateResult.ErrorMsg}。", Color.Red);
                            }
                        }
                        else
                        {
                            //更新式  要先删除前面的数据相关的数据
                            await base.Save(EditEntity);
                        }
                    }
                    else
                    {
                        //更新式  要先删除前面的数据相关的数据
                        await base.Save(EditEntity);
                    }

                }
                else
                {
                    ReturnMainSubResults<tb_SaleOrder> SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SOrderNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }

            }
        }


        protected async override Task<ApprovalEntity> Review()
        {
            if (EditEntity == null)
            {
                return null;
            }

            //如果已经审核通过，则不能重复审核
            if (EditEntity.ApprovalStatus.HasValue)
            {
                if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
                {
                    if (EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                    {
                        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。", UILogType.警告);
                        return null;
                    }
                }
            }

            if (EditEntity.tb_SaleOrderDetails == null || EditEntity.tb_SaleOrderDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整产品数量和金额数据。", UILogType.警告);
                return null;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
            };
            ApprovalEntity ae = await base.Review();
            if (EditEntity == null)
            {
                return null;
            }
            if (ae.ApprovalStatus == (int)ApprovalStatus.未审核)
            {
                return null;
            }
            //ReturnResults<tb_Stocktake> rmr = new ReturnResults<tb_Stocktake>();
            // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            //BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            //因为只需要更新主表
            //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
            // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFacByName<tb_SaleOrderController<tb_SaleOrder>>(typeof(T).Name + "Controller");
            ReturnResults<bool> rmrs = await ctr.ApprovalAsync(EditEntity, ae);
            if (rmrs.Succeeded)
            {
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                //审核成功
                base.ToolBarEnabledControl(MenuItemEnums.审核);
                //如果审核结果为不通过时，审核不是灰色。
                if (!ae.ApprovalResults)
                {
                    toolStripbtnReview.Enabled = true;
                }
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,原因是：{rmrs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                MessageBox.Show($"{ae.bizName}:{ae.BillNo}审核失败,\r\n 原因是：{rmrs.ErrorMsg},如果无法解决，请联系管理员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                toolStripbtnReview.Enabled = true;
            }
            return ae;
        }




        /*
        protected override void Print()
        {
            //base.Print();
            //return;
            //if (_EditEntity == null)
            //{
            //    return;
            //    _EditEntity = new tb_Stocktake();
            //}
            //List<tb_Stocktake> main = new List<tb_Stocktake>();
            ////公共产品数据部分
            //List<tb_Product> products = new List<tb_Product>();
            //foreach (tb_StocktakeDetail item in details)
            //{
            //    //载入数据就是相对完整的
            //    tb_Product prod = list.Find(p => p.Product_ID == item.Product_ID);
            //    if (prod != null)
            //    {
            //        item.tb_Product = prod;
            //    }
            //}

            //_EditEntity.tb_StocktakeDetail = details;
            // main.Add(_EditEntity);
            //FastReport.Report FReport;
            //FReport = new FastReport.Report();
            //FReport.RegisterData(details, "Main");
            //String reportFile = "SOB.frx";
            //RptPreviewForm frm = new RptPreviewForm();
            //frm.ReprotfileName = reportFile;
            //frm.MyReport = FReport;
            //frm.ShowDialog();


            //List<tb_Stocktake> main = new List<tb_Stocktake>();
            ////公共产品数据部分
            //List<View_ProdDetail> products = new List<tb_Product>();
            //foreach (tb_StocktakeDetail item in details)
            //{
            //    //载入数据就是相对完整的
            //    tb_Product prod = list.Find(p => p.Product_ID == item.Product_ID);
            //    if (prod != null)
            //    {
            //        item.tb_Product = prod;
            //    }
            //}

            //_EditEntity.tb_StocktakeDetail = details;
            //main.Add(_EditEntity);


            FastReport.Report FReport;
            FReport = new FastReport.Report();
            FReport.RegisterData(details, "Main");
            String reportFile = typeof(tb_SaleOrder).Name + ".frx";
            RptPreviewForm frm = new RptPreviewForm();
            frm.ReprotfileName = reportFile;
            frm.MyReport = FReport;
            frm.ShowDialog();


        }
        */

        //protected override void Print()
        //{

        //    PrintData = ctr.GetPrintData(EditEntity.PrimaryKeyID);
        //    base.Print();

        //}



        private void cmbCustomerVendor_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerVendor_ID.SelectedIndex > 0)
            {
                if (cmbCustomerVendor_ID.SelectedItem is tb_CustomerVendor cv)
                {
                    if (cv.Employee_ID.HasValue)
                    {
                        cmbEmployee_ID.SelectedValue = cv.Employee_ID.Value;
                        EditEntity.Employee_ID = cv.Employee_ID.Value;
                        cmbCustomerVendor_ID.EndUpdate();
                        //cmbEmployee_ID.EndUpdate();
                    }
                }
            }
        }

        /*
        protected override void PrintDesigned()
        {
            string ReprotfileName = typeof(tb_SaleOrder).Name + ".frx";
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = ReprotfileName;
            // frm.ShowDialog();
            //调用内置方法 给数据源 新编辑，后面的话，直接load 可以不用给数据源的格式
            //string ReprotfileName = "SOB.frx";
            //List<tb_SaleOrder> main = new List<tb_SaleOrder>();
            //_EditEntity.tb_sales_order_details = details;
            //main.Add(_EditEntity);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            List<tb_SaleOrder> rptSources = new List<tb_SaleOrder>();
            tb_SaleOrder saleOrder = EditEntity as tb_SaleOrder;
            saleOrder.tb_SaleOrderDetails = details;
            rptSources.Add(saleOrder);

            FReport.RegisterData(rptSources, "报表数据源");
            String reportFile = string.Format("ReportTemplate/{0}", ReprotfileName);
            FReport.Load(reportFile);  //载入报表文件
            //FReport.FileName = "销售订单单据";
            FReport.Design();

        }
        */

        /*
          protected override void Preview()
        {
            //RptMainForm PRT = new RptMainForm();
            //PRT.ShowDialog();
            //return;
            //RptPreviewForm pForm = new RptPreviewForm();
            //pForm.ReprotfileName = "SOB.frx";
            //List<tb_SaleOrder> main = new List<tb_SaleOrder>();
            //main.Add(_EditEntity);
            //pForm.Show();
            tb_Stocktake testmain = new tb_Stocktake();
            //要给值
            //if (EditEntity == null)
            //{
            //    return;
            //    EditEntity = new tb_Stocktake();
            //}

            List<T> main = new List<T>();
            testmain.tb_StocktakeDetails = details;
            main.Add(testmain);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            FReport.RegisterData(main, "Main");
            String reportFile = "SOB.frx";
            RptPreviewForm frm = new RptPreviewForm();
            frm.ReprotfileName = reportFile;
            frm.MyReport = FReport;
            frm.ShowDialog();
        }
         */

      /*
        protected async override void ReReview()
        {
            {
                return;
            }

            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                return;
            }


            if (EditEntity.tb_SaleOrderDetails == null || EditEntity.tb_SaleOrderDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
            };

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
            List<tb_SaleOrder> list = new List<tb_SaleOrder>();
            list.Add(EditEntity);
            ReturnResults<bool> rr = await ctr.AntiApprovalAsync(list);
            if (rr.Succeeded)
            {

                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                //审核成功
                base.ToolBarEnabledControl(MenuItemEnums.反审);
                toolStripbtnReview.Enabled = true;

            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}反审失败{rr.ErrorMsg},请联系管理员！", Color.Red);
            }

        }

        */
        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_SaleOrder>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<tb_SaleOrder>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_SaleOrder> EditEntitys = new List<tb_SaleOrder>();
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的并且通过的情况才能结案
                List<tb_SaleOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
                if (rs.Succeeded)
                {
                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        private void chk非平台单_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
