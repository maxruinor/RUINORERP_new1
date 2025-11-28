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
using RUINORERP.UI.AdvancedUIModule;
using Krypton.Navigator;

using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

using Microsoft.Extensions.Logging;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Model.Base;
using RUINORERP.Business.Processor;
using StackExchange.Redis;
using RUINORERP.Global.EnumExt;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.PSI.INV
{
    /// <summary>
    /// 实现各种情况下的盘点工作 2023-10-12
    /// 思路更改，期初表 存在的意义少了。
    /// 盘点时如果选择期初，成本强制显示出来，并且必须输入。其他的方式。不显示成本也不用必须输入。
    /// </summary>
    [MenuAttrAssemblyInfo("盘点作业", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.盘点管理, BizType.盘点单)]
    public partial class UCStocktake : BaseBillEditGeneric<tb_Stocktake, tb_StocktakeDetail>, IPublicEntityObject
    {
        public UCStocktake()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        private void UCStocktake_OnBindDataToUIEvent(tb_Stocktake entity)
        {
            if (entity != null)
            {
                tb_Stocktake stocktake = entity as tb_Stocktake;
                if (stocktake.DataStatus == (int)DataStatus.完结 || stocktake.DataStatus == (int)DataStatus.确认)
                {
                    base.toolStripbtnReview.Enabled = false;
                    base.toolStripbtnModify.Enabled = false;
                }
                else
                {
                    base.toolStripbtnReview.Enabled = true;
                    base.toolStripbtnModify.Enabled = true;
                }
            }

            BindData(entity as tb_Stocktake);
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_Stocktake);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Stocktake).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v => v.Name, cmbLocation_ID);
            InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Stocktake>(typeof(CheckMode), e => e.CheckMode, cmbCheckMode, true);
            DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Stocktake>(typeof(Adjust_Type), e => e.Adjust_Type, cmb调整类型, false);


            //枚举过滤了一下
            //EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //List<string> listStr = new List<string>();
            //List<EnumEntityMember> list = new List<EnumEntityMember>();
            //list = typeof(CheckMode).GetListByEnum<CheckMode>(selectedItem: 2);
            //bindingHelper.InitDataToCmbByEnumOnWhere(list, "CheckMode", cmbCheckMode);


        }

        protected override void Cancel()
        {
            cmbCheckMode.Enabled = true;
            base.Cancel();
            lblPrintStatus.Text = "";
        }
        public override void BindData(tb_Stocktake _BaseEntity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            tb_Stocktake entity = _BaseEntity as tb_Stocktake;
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.MainID > 0)
                {
                    entity.PrimaryKeyID = entity.MainID;
                    entity.ActionStatus = ActionStatus.加载;

                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (string.IsNullOrEmpty(entity.CheckNo))
                    {
                        entity.CheckNo = ClientBizCodeService.GetBizBillNo(BizType.盘点单);
                    }
                    entity.Check_date = System.DateTime.Now;
                    entity.CarryingDate = System.DateTime.Now;
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    //设置一下默认的枚举
                    //不要默认，让用户选。因为输入数据后再选择。会清空输入的内容

                    //if (entity.CheckMode == 0)
                    //{
                    //    entity.CheckMode = (int)CheckMode.一般盘点;
                    //}


                    if (entity.Adjust_Type == 0)
                    {
                        entity.Adjust_Type = (int)Adjust_Type.全部;
                    }
                    if (entity.tb_StocktakeDetails != null && entity.tb_StocktakeDetails.Count > 0)
                    {
                        entity.tb_StocktakeDetails.ForEach(c => c.MainID = 0);
                        entity.tb_StocktakeDetails.ForEach(c => c.SubID = 0);
                    }
                }


            }


            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }

            EditEntity = entity;
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
            {
                if (EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                {
                    base.ToolBarEnabledControl(MenuItemEnums.审核);
                }
            }
            else
            {
                //没有审核 都可以修改
                base.toolStripbtnModify.Enabled = true;
            }

            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                txtCheckTotalAmount.Visible = false;
                txtDiffAmount.Visible = false;
                txtCarryingTotalAmount.Visible = false;
            }*/

            DataBindingHelper.BindData4CmbByEnum<tb_Stocktake, CheckMode>(entity, k => k.CheckMode, cmbCheckMode, false);
            DataBindingHelper.BindData4CmbByEnum<tb_Stocktake, Adjust_Type>(entity, k => k.Adjust_Type, cmb调整类型, false);

            DataBindingHelper.BindData4CmbByEntity<tb_Employee>(entity, k => k.Employee_ID, cmbEmployee_ID);
            DataBindingHelper.BindData4CmbByEntity<tb_Location>(entity, k => k.Location_ID, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckNo, txtCheckNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CarryingTotalQty.ToString(), txtCarryingTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CarryingTotalAmount.ToString(), txtCarryingTotalAmount, BindDataType4TextBox.Money, true);



            DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.Check_date, dtpcheck_date, false);
            DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.CarryingDate, dtpCarryingDate, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.DiffTotalAmount.ToString(), txtDiffTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.DiffTotalQty.ToString(), txtDiffTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckTotalQty.ToString(), txtCheckTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckTotalAmount.ToString(), txtCheckTotalAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4ControlByEnum<tb_Stocktake>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_Stocktake>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, EditEntity);
            txtstatus.ReadOnly = true;
            if (entity.tb_StocktakeDetails != null && entity.tb_StocktakeDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_StocktakeDetails);
                details = entity.tb_StocktakeDetails;
                sgh.LoadItemDataToGrid<tb_StocktakeDetail>(grid1, sgd, entity.tb_StocktakeDetails, c => c.ProdDetailID);
            }
            else
            {

                sgh.LoadItemDataToGrid<tb_StocktakeDetail>(grid1, sgd, new List<tb_StocktakeDetail>(), c => c.ProdDetailID);
                //LoadDataToGrid(new List<tb_StocktakeDetail>());
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {

                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿)
                || (true && entity.DataStatus == (int)DataStatus.新建)
                )
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_Stocktake>(c => c.CheckMode))
                    {
                        if (EditEntity.CheckMode >= 0)
                        {
                            ControlCostByCheckModel((CheckMode)EditEntity.CheckMode);
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
                ShowPrintStatus(lblPrintStatus, EditEntity);
            };

            base.BindData(entity);
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UCStocktake_Load(object sender, EventArgs e)
        {
            grid1.Enter += Grid1_Enter;
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_StocktakeDetail>(c => c.ProdDetailID, true);

            //List<SourceGridDefineColumnItem> cols1 = SourceGridDefine.GetSourceGridDefineColumnItems<ProductSharePart>();
            ////指定了关键字段ProdDetailID
            //List<SourceGridDefineColumnItem> cols2 = SourceGridDefine.GetSourceGridDefineColumnItems<tb_StocktakeDetail>();
            //listCols.AddRange(cols1);
            //listCols.AddRange(cols2);

            //List<string> RepeatColNames = listCols.Select(c => c.ColName).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            // 产品视图中带出字段，除了大部分用于展示外，还有部分也存于单据明细中。
            //即要把这个值保存到单据明细中。其中产品ID是必须的。设置为主键。 特点是两部分中都会存在，重复特性
            //https://www.cnblogs.com/fps2tao/p/16355863.html  各中集合运算  优先代码 可学习代码 By watson TODO

            //两个集合中都存的字段，即字段名相同
            //List<string> RepeatColNames = cols1.Select(c => c.ColName).ToList().Intersect(cols2.Select(c => c.ColName).ToList()).ToList();

            listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.SubID);
            listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.MainID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            //如果选择了库位,则只会显示这个库位下面的货架 这个逻辑将来来处理

            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            listCols.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.DiffQty);
            listCols.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.CarryinglQty);
            listCols.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.DiffSubtotalAmount);
            listCols.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.CheckSubtotalAmount);
            listCols.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.CarryingSubtotalAmount);
            InitLoadGrid();

            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            //sgd.BindingSourceLines.ListChanged += BindingSourceLines_ListChanged;
            // bindingSourceSub.AddingNew += BindingSourceSub_AddingNew;
            //控件主表的字段显示权限
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        private void InitLoadGrid()
        {
            listCols.SetCol_Format<tb_StocktakeDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_StocktakeDetail>(c => c.Cost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_StocktakeDetail>(c => c.UntaxedCost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_StocktakeDetail>(c => c.DiffSubtotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_StocktakeDetail>(c => c.CheckSubtotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_StocktakeDetail>(c => c.CarryingSubtotalAmount, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;

            sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.Inv_Cost, t => t.UntaxedCost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.prop, t => t.property);
            //由查询的结果中含有的字段，指向到明细中的字段中,这里是将查询到的库存放到载账数量

            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_StocktakeDetail>(sgd, f => f.Quantity, t => t.CarryinglQty);
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_StocktakeDetail>(sgd, f => f.Inv_Cost, t => t.UntaxedCost);

            //sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_StocktakeDetail>(sgd, (a, b) => a.Inv_Cost / (1 + b.),
            //我要实现 通过上面类似的传入条件 及公式 ，能计算 目标值 具体是计算含税单价
            //暂时直接加载，将有再看要不要实现
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_StocktakeDetail>(sgd, f => f.Inv_Cost, t => t.Cost);

            listCols.SetCol_Formula<tb_StocktakeDetail>(
                 (a, b) => a.UntaxedCost * (1 + b.TaxRate),  // 计算公式
                 r => r.Cost,                                // 结果列
                 d => d.TaxRate > 0                           // 条件：税率大于0时才计算
             );


            //  listCols.SetCol_Formula<tb_StocktakeDetail>((a, b, c) => (a.CarryinglQty * b.CarryinglQty * c.CarryinglQty), F => F.CarryinglQty);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CheckQty);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CarryinglQty);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.DiffQty);

            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.DiffSubtotalAmount);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CheckSubtotalAmount);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CarryingSubtotalAmount);


            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.CheckQty - b.CarryinglQty), r => r.DiffQty);//-->成交价是结果列
            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b, c) => a.Cost / (1 + b.TaxRate), d => d.UntaxedCost);
            //listCols.SetCol_FormulaReverse<tb_StocktakeDetail>(d => d.CheckQty != d.CarryinglQty, (a, b) => (a.CheckQty - b.CarryinglQty), r => r.DiffQty);//-->成交价是结果列

            listCols.SetCol_FormulaReverse<tb_StocktakeDetail>(d => d.Cost == 0, (a, b, c) => a.UntaxedCost * (1 + b.TaxRate), d => d.Cost);

            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.DiffQty * b.UntaxedCost), r => r.DiffSubtotalAmount);
            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.CarryinglQty * b.UntaxedCost), r => r.CarryingSubtotalAmount);
            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.CheckQty * b.UntaxedCost), r => r.CheckSubtotalAmount);

            //应该只提供一个结构
            List<tb_StocktakeDetail> lines = new List<tb_StocktakeDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_StocktakeDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_StocktakeDetail));
        }

        private void Grid1_Enter(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            Control firstInvalidControl = null;

            if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            {
                sb.AppendLine("请选择【盘点仓库】。");
                firstInvalidControl = cmbLocation_ID;
            }

            if (EditEntity.CheckMode == 0 || EditEntity.CheckMode == -1)
            {
                sb.AppendLine("请选择【盘点方式】。");
                if (firstInvalidControl == null)
                {
                    firstInvalidControl = cmbCheckMode;
                }
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (firstInvalidControl != null)
                {
                    firstInvalidControl.Focus();
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
                List<tb_StocktakeDetail> details = new List<tb_StocktakeDetail>();

                foreach (var item in RowDetails)
                {
                    tb_StocktakeDetail Detail = MainForm.Instance.mapper.Map<tb_StocktakeDetail>(item);
                    if (item.Quantity.HasValue)
                    {
                        Detail.CarryinglQty = item.Quantity.Value;
                    }

                    if (item.Inv_Cost.HasValue)
                    {
                        Detail.CarryingSubtotalAmount = Detail.CarryinglQty * item.Inv_Cost.Value;
                        Detail.Cost = item.Inv_Cost.Value;
                    }

                    details.Add(Detail);
                }

                sgh.InsertItemDataToGrid<tb_StocktakeDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }
        }

        public void DoSomething(Func<int, int, int> op)
        {
            Console.WriteLine(op(5, 2));
        }

        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {

            if (EditEntity == null)
            {
                return;
            }
            try
            {
                //给默认值的话，可以批量处理，按int类型这种？
                tb_StocktakeDetail rowObj = _rowObj as tb_StocktakeDetail;


                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                //details = sgd.BindingSourceLines.List.CastToList<tb_StocktakeDetail>();
                details = sgd.BindingSourceLines.DataSource as List<tb_StocktakeDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                EditEntity.CheckTotalQty = details.Sum(c => c.CheckQty);
                EditEntity.CarryingTotalQty = details.Sum(c => c.CarryinglQty);
                EditEntity.DiffTotalQty = details.Sum(c => c.DiffQty);
                EditEntity.CheckTotalAmount = details.Sum(c => c.UntaxedCost * c.CheckQty);
                EditEntity.CarryingTotalAmount = details.Sum(c => c.UntaxedCost * c.CarryinglQty);
                EditEntity.DiffTotalAmount = details.Sum(c => c.UntaxedCost * c.DiffQty);

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MainForm.Instance.uclog.AddLog(ex.Message);

            }
        }


        protected override async Task<bool> Submit()
        {
            if (EditEntity == null)
            {
                return false;
            }
            CheckMode initMode = (CheckMode)EditEntity.CheckMode;
            if (initMode == CheckMode.期初盘点 && details.Any(c => c.UntaxedCost == 0))
            {
                MessageBox.Show("【期初盘点】模式下，盘点产品的未税成本不能为0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            switch (EditEntity.Adjust_Type)
            {
                case (int)Adjust_Type.全部:
                    break;
                case (int)Adjust_Type.增加:
                    if (EditEntity.tb_StocktakeDetails.Any(c => c.DiffQty < 0))
                    {
                        if (MessageBox.Show("系统检测到您实际盘点差异数据减少，？\r\n你确定选择的【调整类型】正确吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    break;
                case (int)Adjust_Type.减少:
                    if (EditEntity.tb_StocktakeDetails.Any(c => c.DiffQty > 0))
                    {
                        if (MessageBox.Show("系统检测到您盘点差异数据有增加，？\r\n你确定选择的【调整类型】正确吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    break;
            }

            return await base.Submit();
        }





        List<tb_StocktakeDetail> details = new List<tb_StocktakeDetail>();



        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtDiffTotalAmount);

            bindingSourceSub.EndEdit();
            //List<tb_StocktakeDetail> oldOjb = new List<tb_StocktakeDetail>(details.ToArray());

            //tb_Stocktake EditEntity = EditEntity;
            List<tb_StocktakeDetail> detailentity = bindingSourceSub.DataSource as List<tb_StocktakeDetail>;
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

                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();


                if (NeedValidated && aa.Count > 0)
                {
                    var prod = MainForm.Instance.View_ProdDetailList.FirstOrDefault(c => c.ProdDetailID.ToString() == aa[0].ToString());
                    System.Windows.Forms.MessageBox.Show($"明细中，SKU{prod.SKU},{prod.CNName}\r\n相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.uclog.AddLog($"明细中，SKU{prod.SKU},{prod.CNName}相同的产品不能多行录入:盘点单号:{EditEntity.CheckNo},产品ID:{aa[0].ToString()}");
                    }
                    return false;
                }

                if (NeedValidated && (EditEntity.CheckTotalQty == 0 || detailentity.Sum(c => c.CheckQty) == 0))
                {
                    if (MessageBox.Show("您确定整单盘点数量为零吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return false;
                    }
                }


                EditEntity.tb_StocktakeDetails = details;
                EditEntity.CheckTotalQty = details.Sum(c => c.CheckQty);
                EditEntity.CarryingTotalQty = details.Sum(c => c.CarryinglQty);
                EditEntity.DiffTotalQty = details.Sum(c => c.DiffQty);
                EditEntity.CheckTotalAmount = details.Sum(c => c.Cost * c.CheckQty);
                EditEntity.CarryingTotalAmount = details.Sum(c => c.Cost * c.CarryinglQty);
                EditEntity.DiffTotalAmount = details.Sum(c => c.Cost * c.DiffQty);
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }


                if (NeedValidated && !base.Validator<tb_StocktakeDetail>(details))
                {
                    return false;
                }




                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

                //  EditEntity.DataStatus = ApprovalStatus.未审核.ToString();


                ReturnMainSubResults<tb_Stocktake> SaveResult = new ReturnMainSubResults<tb_Stocktake>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.CheckNo}。");
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




        private void btnImportCheckProd_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                MessageBox.Show("请先查询或新建盘点单。");
                return;
            }


            if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            {
                MessageBox.Show("请先选择盘点产品所在的库位。");
                return;
            }

            using (QueryFormGeneric dg = new QueryFormGeneric())
            {
                dg.Text = "盘点数据导入";
                dg.StartPosition = FormStartPosition.CenterScreen;
                dg.prodQuery.MultipleChoices = true;
                dg.prodQuery.QueryField = EditEntity.GetPropertyName<tb_Stocktake>(c => c.Location_ID);
                dg.prodQuery.QueryValue = EditEntity.Location_ID;
                dg.prodQuery.UseType = ProdQueryUseType.盘点导入;

                if (dg.ShowDialog() == DialogResult.OK)
                {
                    /// Control.Tag = dg.QueryObjects;
                    // Control.Value = dg.QueryValue;
                    //将查询到的结果转换为单据明细 ,货加号来自于库存，后面添加查询对象除产品，等还可以依库存查
                    List<tb_StocktakeDetail> details = new List<tb_StocktakeDetail>();
                    foreach (View_ProdDetail item in dg.prodQuery.QueryObjects)
                    {

                        tb_StocktakeDetail detail = MainForm.Instance.mapper.Map<tb_StocktakeDetail>(item);
                        detail.property = item.prop;
                        if (item.Inv_Cost.HasValue)
                        {
                            detail.UntaxedCost = item.Inv_Cost.Value;
                        }
                        detail.Cost = detail.UntaxedCost * (1 + detail.TaxRate);
                        if (item.Quantity.HasValue)
                        {
                            detail.CarryinglQty = item.Quantity.Value;
                        }
                        detail.CarryingSubtotalAmount = detail.UntaxedCost * detail.CarryinglQty;
                        detail.CheckQty = 0;
                        detail.DiffQty = 0;
                        detail.CheckSubtotalAmount = 0;
                        details.Add(detail);
                    }
                    //sgh.LoadItemDataToGrid(details);
                    sgh.LoadItemDataToGrid<tb_StocktakeDetail>(grid1, sgd, details, c => c.ProdDetailID);
                }
            }

        }


        private void ControlCostByCheckModel(CheckMode checkMode)
        {

            //如果选期初 ，则载账日期需要是当前时间，调整方式 则不可能是减少，是增加。
            if (checkMode == CheckMode.期初盘点)
            {
                lblCostTips.Visible = true;
                //成本字段可以修改
                sgd.DefineColumns.SetCol_ReadWrite<tb_StocktakeDetail>(c => c.Cost);
                sgd.DefineColumns.SetCol_ReadWrite<tb_StocktakeDetail>(c => c.UntaxedCost);
                sgd.DefineColumns.SetCol_ReadWrite<tb_StocktakeDetail>(c => c.TaxRate);
                //添加
                listCols.FirstOrDefault(c => c.ColName == nameof(tb_StocktakeDetail.Cost)).NeverVisible = false;
                listCols.FirstOrDefault(c => c.ColName == nameof(tb_StocktakeDetail.Cost)).Visible = true;

                listCols.FirstOrDefault(c => c.ColName == nameof(tb_StocktakeDetail.UntaxedCost)).NeverVisible = false;
                listCols.FirstOrDefault(c => c.ColName == nameof(tb_StocktakeDetail.UntaxedCost)).Visible = true;

                listCols.FirstOrDefault(c => c.ColName == nameof(tb_StocktakeDetail.TaxRate)).NeverVisible = false;
                listCols.FirstOrDefault(c => c.ColName == nameof(tb_StocktakeDetail.TaxRate)).Visible = true;

                InitLoadGrid();
            }
            else
            {
                lblCostTips.Visible = false;
                UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
                InitLoadGrid();
                //成本字段不可以修改
                sgd.DefineColumns.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.UntaxedCost);
                //成本字段不可以修改
                sgd.DefineColumns.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.Cost);
                //成本字段不可以修改
                sgd.DefineColumns.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.TaxRate);

                //如果权限配方不能看成本时。录入明细后不能切换成期初盘点。因为期初是可以看成本的。大于权限配置。
                if (listCols != null)
                {
                    var detail = new tb_StocktakeDetail();
                    if (listCols.Where(c => c.ColName == nameof(detail.UntaxedCost)).FirstOrDefault().Visible == false)
                    {
                        cmbCheckMode.Enabled = false;
                    }
                    else
                    {
                        cmbCheckMode.Enabled = true;
                    }
                }

            }
        }

    }



}
