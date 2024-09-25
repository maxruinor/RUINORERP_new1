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
using TransInstruction;
using Microsoft.Extensions.Logging;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Model.Base;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.INV
{
    /// <summary>
    /// 实现各种情况下的盘点工作 2023-10-12
    /// 思路更改，期初表 存在的意义少了。
    /// 盘点时如果选择期初，成本强制显示出来，并且必须输入。其它的方式。不显示成本也不用必须输入。
    /// </summary>
    [MenuAttrAssemblyInfo("盘点作业", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.盘点单)]
    public partial class UCStocktake : BaseBillEditGeneric<tb_Stocktake, tb_StocktakeDetail>
    {
        public UCStocktake()
        {
            InitializeComponent();
            base.OnBindDataToUIEvent += UCStocktake_OnBindDataToUIEvent;
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
            //  InitDataToCmbByEnumDynamicGeneratedDataSource(typeof(CheckMode), cmbCheckMode);
            DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Stocktake>(typeof(Adjust_Type), e => e.Adjust_Type, cmb调整类型, false);
            EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //枚举过滤了一下
            CheckMode checkMode = CheckMode.日常盘点;
            List<string> listStr = new List<string>();
            List<EnumEntityMember> list = new List<EnumEntityMember>();
            list = checkMode.GetListByEnum(2);
            bindingHelper.InitDataToCmbByEnumOnWhere(list, "CheckMode", cmbCheckMode);

            //var c = new Controller();
            //cmb调整类型.DataBindings.Add(new Binding("SelectedValue", c, "SelectedValue", true, DataSourceUpdateMode.OnPropertyChanged));
            //this.BindingContextChanged += (sender, e) =>
            //{
            //    // If you change combo binding formatting enabled parameter to false,
            //    // the next will throw the exception you are getting
            //    c.SelectedValue = Adjust_Type.全部;
            //};
        }

        public override void BindData(BaseEntity _BaseEntity)
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
                    entity.CheckNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.盘点单);
                    entity.Check_date = System.DateTime.Now;
                    entity.CarryingDate = System.DateTime.Now;
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    //设置一下默认的枚举

                    entity.CheckMode = (int)CheckMode.一般盘点;
                    entity.Adjust_Type = (int)Adjust_Type.全部; 
          

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

            DataBindingHelper.BindData4CmbByEnum<tb_Stocktake>(entity, k => k.CheckMode, typeof(CheckMode), cmbCheckMode, false);
            DataBindingHelper.BindData4CmbByEnum<tb_Stocktake>(entity, k => k.Adjust_Type, typeof(Adjust_Type), cmb调整类型, false);

            DataBindingHelper.BindData4CmbByEntity<tb_Stocktake>(entity, k => k.Employee_ID, cmbEmployee_ID);
            DataBindingHelper.BindData4CmbByEntity<tb_Location>(entity, k => k.Location_ID, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckNo, txtCheckNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CarryingTotalQty.ToString(), txtCarryingTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CarryingTotalAmount.ToString(), txtCarryingTotalAmount, BindDataType4TextBox.Money, true);
           
             
           
            DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.Check_date, dtpcheck_date, false);
            DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.CarryingDate, dtpCarryingDate, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.DiffTotalAmount.ToString(), txtDiffAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Label<tb_Stocktake>(entity, t => t.DiffTotalQty.ToString(), lblDiffQty, BindDataType4TextBox.Qty, false);
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
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    

                    //if (s2.PropertyName == entity.GetPropertyName<tb_Stocktake>(c => c.Location_ID))
                    //{
                    //    if (EditEntity.Location_ID > 0)
                    //    {

                    //        //明细仓库优先来自于主表，可以手动修改。
                    //        listCols.SetCol_DefaultValue<tb_Stocktake>(c => c.Location_ID, EditEntity.Location_ID);
                    //        if (entity.tb_StocktakeDetails != null)
                    //        {
                    //            entity.tb_StocktakeDetails.ForEach(c => c.lo = EditEntity.Location_ID);
                    //            sgh.SetCellValue<tb_ProdMergeDetail>(sgd, colNameExp => colNameExp.Location_ID, EditEntity.Location_ID);
                    //        }
                    //    }
                    //}
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
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
        private void UCStocktake_Load(object sender, EventArgs e)
        {
            grid1.Enter += Grid1_Enter;
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            /////显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_StocktakeDetail>();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            //Type combinedType = Common.EmitHelper.MergeTypes(true, typeof(ProductSharePart), typeof(tb_StocktakeDetail));
            //object ReturnSumInst = Activator.CreateInstance(combinedType);

            ///显示列表对应的中文
           // ConcurrentQueue<KeyValuePair<string, PropertyInfo>> Ddc = EmitHelper.GetfieldNameList(combinedType);



             listCols = new List<SourceGridDefineColumnItem>();
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
            ControlChildColumnsInvisible(listCols);


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




            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridData = EditEntity;
            /*
            //具体审核权限的人才显示成本？
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.DiffSubtotalAmount, true, c => c.DiffQty, c => c.Cost);
                listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CheckSubtotalAmount, true, c => c.CheckQty, c => c.Cost);
                listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CarryingSubtotalAmount, true, c => c.CarryinglQty, c => c.Cost);
            }
            else
            {
                listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.CarryingSubtotalAmount);
                listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.DiffSubtotalAmount);
                listCols.SetCol_NeverVisible<tb_StocktakeDetail>(c => c.CheckSubtotalAmount);

            }*/

            //  listCols.SetCol_Formula<tb_StocktakeDetail>((a, b, c) => (a.CarryinglQty * b.CarryinglQty * c.CarryinglQty), F => F.CarryinglQty);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CheckQty);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CarryinglQty);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.DiffQty);

            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.DiffSubtotalAmount);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CheckSubtotalAmount);
            listCols.SetCol_Summary<tb_StocktakeDetail>(c => c.CarryingSubtotalAmount);


            listCols.SetCol_FormulaReverse<tb_StocktakeDetail>(d => d.CheckQty != d.CarryinglQty, (a, b) => (a.CheckQty - b.CarryinglQty), r => r.DiffQty);//-->成交价是结果列

            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.DiffQty * b.Cost), r => r.DiffSubtotalAmount);
            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.CarryinglQty * b.Cost), r => r.CarryingSubtotalAmount);
            listCols.SetCol_Formula<tb_StocktakeDetail>((a, b) => (a.CheckQty * b.Cost), r => r.CheckSubtotalAmount);



            sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.prop, t => t.property);
            //由查询的结果中含有的字段，指向到明细中的字段中,这里是将查询到的库存放到载账数量

            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_StocktakeDetail>(sgd, f => f.Quantity, t => t.CarryinglQty);
            // sgh.SetPointToColumnPairs<ProductSharePart, tb_StocktakeDetail>(sgd, f => f.STOCK, t => t.CarryinglQty);




            //应该只提供一个结构
            List<tb_StocktakeDetail> lines = new List<tb_StocktakeDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
               .AndIF(true, w => w.CNName.Length > 0)
              // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
              .ToExpression();//注意 这一句 不能少
                              // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);
            sgd.SetDependencyObject<ProductSharePart, tb_StocktakeDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_StocktakeDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            //sgd.BindingSourceLines.ListChanged += BindingSourceLines_ListChanged;
            // bindingSourceSub.AddingNew += BindingSourceSub_AddingNew;
        }

        private void Grid1_Enter(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }
            if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            {
                MessageBox.Show("请选择【盘点仓库】。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLocation_ID.Focus();
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
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_StocktakeDetail Detail = mapper.Map<tb_StocktakeDetail>(item);
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
                //计算值 

                //rowObj.DiffQty = rowObj.CheckQty - rowObj.CarryinglQty;
                // rowObj.DiffSubtotalAmount = rowObj.DiffQty * rowObj.Cost;
                // rowObj.CarryingSubtotalAmount = rowObj.CarryinglQty * rowObj.Cost;
                // rowObj.CheckSubtotalAmount = rowObj.CheckQty * rowObj.Cost;

                //DoSomething((x, y) => rowObj.CheckQty - rowObj.CarryinglQty);


                // Operator<int>.Add(rowObj.CheckQty, rowObj.CarryinglQty);

                //反算  比方通过已经总价 和数量 算出单价  盘点这时不需要这样处理
                //if (myGridDefine[position.Column].ColName == "DiffAmount")
                //{
                //    // rowObj.DiffQty = (rowObj.DiffAmount / rowObj.Cost);
                //}
                //else
                //{
                //    rowObj.DiffAmount = rowObj.DiffQty * rowObj.Cost;
                //}

                //sgd.BindingSourceLines

                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                //details = sgd.BindingSourceLines.List.CastToList<tb_StocktakeDetail>();
                details = sgd.BindingSourceLines.DataSource as List<tb_StocktakeDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                decimal? totalMoney = details.Sum(r => r.Cost * r.CheckQty);
                EditEntity.CheckTotalQty = details.Sum(c => c.CheckQty);
                EditEntity.CarryingTotalQty = details.Sum(c => c.CarryinglQty);
                EditEntity.DiffTotalQty = details.Sum(c => c.DiffQty);
                EditEntity.CheckTotalAmount = details.Sum(c => c.Cost * c.CheckQty);
                EditEntity.CarryingTotalAmount = details.Sum(c => c.Cost * c.CarryinglQty);
                EditEntity.DiffTotalAmount = details.Sum(c => c.Cost * c.DiffQty);
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


        //protected async override void Delete()
        //{
        //    if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
        //    {
        //        tb_Stocktake main = (tb_Stocktake)bindingSourceMain.Current;
        //        tb_StocktakeDetail sub = (tb_StocktakeDetail)bindingSourceSub.Current;
        //        bindingSourceMain.Remove(main);
        //        bindingSourceSub.Remove(sub);
        //        //      db.DeleteNav(list)
        //        //.IncludesAllFirstLayer()//自动2级
        //        //.IncludeByNameString(nameof(类.导航)).ThenIncludeByNameString(nameof(类.导航2))//3级
        //        //.ExecuteCommand();
        //        base.Delete();
        //    }

        //}










        //private void UcAdv_AdvQueryEvent(bool useLike, Model.Base.BaseEntityDto dto)
        //{
        //    //AdvQueryShowResult(useLike, dto);

        //}

        List<tb_StocktakeDetail> details = new List<tb_StocktakeDetail>();


        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtDiffAmount);

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
                    var prod = MainForm.Instance.list.FirstOrDefault(c => c.ProdDetailID.ToString() == aa[0].ToString());
                    System.Windows.Forms.MessageBox.Show($"明细中，SKU{prod.SKU},{prod.CNName}\r\n相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.uclog.AddLog($"明细中，SKU{prod.SKU},{prod.CNName}相同的产品不能多行录入:盘点单号:{EditEntity.CheckNo},产品ID:{aa[0].ToString()}");
                    }
                    return false;
                }

                EditEntity.tb_StocktakeDetails = details;

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


        /*
        if (_EditEntity.actionStatus == ActionStatus.修改)
        {
            //产品ID有值才算有效值
            details = detailentity.Where(t => t.ProdDetailID.HasValue).ToList();
            details = details.Where(t => t.ProdDetailID.Value > 0).ToList();
            //如果没有有效的明细。直接提示
            if (details.Count == 0)
            {
                MessageBox.Show("请录入有效明细记录！");
                return;
            }

            //前后比较是否变化
            ComPareResult result = UITools.ComPare(oldOjb, details);
            if (!result.IsEqual)
            {
                //把差集删除
                //https://www.cnblogs.com/nuomibaibai/p/17043541.html
                //string [] dogs = animals.Select(x=>x.dog).ToArray();
                //差集 简化为明细ID的差，得到被删除的IDS
                List<long> oldids = oldOjb.Select(id => id.SubID).ToList();
                List<long> newids = details.Select(id => id.SubID).ToList();
                List<long> chaji = oldids.Except<long>(newids).ToList();
                List<object> olist = new List<object>();
                chaji.ForEach(i => olist.Add(i));
                //await soc.DeleteAsyncForDetail(olist.ToArray());
            }
            else
            {
                if (EditEntity.actionStatus == ActionStatus.无操作)
                {
                    EditEntity.actionStatus = ActionStatus.修改;
                }
            }
            //然后再写保存逻辑



            //计算总金额
            //decimal? totalMoney = details.Sum(r => r.quantity * r.price);
            //_EditEntity.TotalAmount = totalMoney.Value;
            //_EditEntity.tb_sales_order_details = details;
            //设置目标ID成功后就行头写上编号？
            //   表格中的验证提示
            //   其他输入条码验证
            bool vd = base.ShowInvalidMessage(ctrMain.Validator(_EditEntity));
            if (!vd)
            {
                return;
            }
            ReturnMainSubResults<tb_Stocktake> rmr = new ReturnMainSubResults<tb_Stocktake>();
            rmr = await soc.BaseSaveOrUpdateWithChild<tb_Stocktake>(_EditEntity);

            //_EditEntity = await soc.AddAsync(_EditEntity, details);
            //_EditEntity = await soc.AddAsync(_EditEntity);
            lblReview.Text = _EditEntity.status;
        }
        */

        //  base.Save();



        /*
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
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。");
                return null;
            }
        }
    }

    if (EditEntity.CarryingTotalQty != details.Sum(c => c.CarryinglQty))
    {
        System.Windows.Forms.MessageBox.Show($"单据载账总数量{EditEntity.CarryingTotalQty}和明细载账数量的和{details.Sum(c => c.CarryinglQty)}不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }
    if (EditEntity.CheckTotalQty != details.Sum(c => c.CheckQty))
    {
        System.Windows.Forms.MessageBox.Show($"单据盘点总数量{EditEntity.CheckTotalQty}和明细盘点数量的和{details.Sum(c => c.CheckQty)}不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    if (EditEntity.DiffTotalQty != details.Sum(c => c.DiffQty))
    {
        System.Windows.Forms.MessageBox.Show($"单据差异总数量{EditEntity.DiffTotalQty}和明细差异数量的和{details.Sum(c => c.DiffQty)}不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }


    Command command = new Command();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_Stocktake oldobj = CloneHelper.DeepCloneObject<tb_Stocktake>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_Stocktake>(EditEntity, oldobj);
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
    //因为只需要更新主表
    //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
    // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
    tb_StocktakeController<tb_Stocktake> ctr = Startup.GetFromFac<tb_StocktakeController<tb_Stocktake>>();
    ReturnResults<tb_Stocktake> rrs = await ctr.ApprovalAsync(EditEntity);
    if (rrs.Succeeded)
    {

        //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
        //{

        //}
        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
        //MainForm.Instance.ecs.AddSendData(od);
        EditEntity.DataStatus = (int)DataStatus.确认;
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
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rrs.ErrorMsg},如果无法解决，请联系管理员！");
    }

    return ae;
    }


    /// <summary>
    /// 列表中不再实现反审，批量，出库反审情况极少。并且是仔细处理
    /// </summary>
    protected async override Task<ApprovalEntity> ReReview()
    {
    ApprovalEntity ae = new ApprovalEntity();
    if (EditEntity == null)
    {
        return ae;
    }

    //反审，要审核过，并且通过了，才能反审。
    if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
    {
        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
        return ae;
    }


    if (EditEntity.tb_StocktakeDetails == null || EditEntity.tb_StocktakeDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量。", UILogType.警告);
        return ae;
    }

    Command command = new Command();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_Stocktake oldobj = CloneHelper.DeepCloneObject<tb_Stocktake>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_Stocktake>(EditEntity, oldobj);
    };

    tb_StocktakeController<tb_Stocktake> ctr = Startup.GetFromFac<tb_StocktakeController<tb_Stocktake>>();
    ReturnResults<bool> rrs = await ctr.AntiApprovalAsync(EditEntity);
    if (rrs.Succeeded)
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
        MainForm.Instance.PrintInfoLog($"盘点单{EditEntity.CheckNo}反审失败,{rrs.ErrorMsg},请联系管理员！", Color.Red);
    }
    return ae;
    }
    */
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
                        IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                        tb_StocktakeDetail detail = mapper.Map<tb_StocktakeDetail>(item);
                        detail.property = item.prop;
                        if (item.Inv_Cost.HasValue)
                        {
                            detail.Cost = item.Inv_Cost.Value;
                        }
                        if (item.Quantity.HasValue)
                        {
                            detail.CarryinglQty = item.Quantity.Value;
                        }
                        detail.CarryingSubtotalAmount = detail.Cost * detail.CarryinglQty;
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

        private void cmbCheckMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sgd == null || cmbCheckMode.SelectedItem == null || cmbCheckMode.SelectedValue.ToString() == "-1")
            {
                return;
            }
            //如果选期初 ，则载帐日期需要是当前时间，调整方式 则不可能是减少，是增加。
            if (cmbCheckMode.SelectedValue.ToString() == ((int)CheckMode.期初盘点).ToString())
            {
                //成本字段可以修改
                sgd.DefineColumns.SetCol_ReadWrite<tb_StocktakeDetail>(c => c.Cost);
            }
            else
            {
                //成本字段不可以修改
                sgd.DefineColumns.SetCol_ReadOnly<tb_StocktakeDetail>(c => c.Cost);
            }
        }
    }



}
