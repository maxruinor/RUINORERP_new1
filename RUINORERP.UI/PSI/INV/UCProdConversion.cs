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
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using Netron.GraphLib;
using Krypton.Toolkit;
 


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("产品转换单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.产品转换单)]
    public partial class UCProdConversion : BaseBillEditGeneric<tb_ProdConversion, tb_ProdConversionDetail>
    {
        public UCProdConversion()
        {
            InitializeComponent();

        }

        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_ProdConversion, actionStatus);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

        }
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdConversion).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public override void BindData(tb_ProdConversion entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.ConversionID > 0)
            {
                entity.PrimaryKeyID = entity.ConversionID;
                entity.ActionStatus = ActionStatus.加载;

            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                entity.ConversionNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.产品转换单);
                entity.ConversionDate = System.DateTime.Now;
                if (entity.tb_ProdConversionDetails != null && entity.tb_ProdConversionDetails.Count > 0)
                {
                    entity.tb_ProdConversionDetails.ForEach(c => c.ConversionID = 0);
                    entity.tb_ProdConversionDetails.ForEach(c => c.ConversionSub_ID = 0);
                }
            }

            DataBindingHelper.BindData4TextBox<tb_ProdConversion>(entity, t => t.TotalConversionQty, txtTotalConversionQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProdConversion>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdConversion>(entity, t => t.Reason, txtReason, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_ProdConversion>(entity, t => t.ConversionNo, txtConversionNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_ProdConversion>(entity, t => t.ConversionDate, dtpConversionDate, false);
            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_ProdConversion>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProdConversion>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_ProdConversionDetails != null && entity.tb_ProdConversionDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProdConversionDetail>(grid1, sgd, entity.tb_ProdConversionDetails, c => c.ProdDetailID_from);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdConversionDetail>(grid1, sgd, new List<tb_ProdConversionDetail>(), c => c.ProdDetailID_from);
            }

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService <tb_ProdConversionValidator> (), kryptonSplitContainer1.Panel1.Controls);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

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
            base.BindData(entity);
        }

        private void Bsa_Click(object sender, EventArgs e)
        {
            SelectLocationTips();
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
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
            listCols = sgh.GetGridColumns<tb_ProdConversionDetail>();

            listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.ProdDetailID_from);
            listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.ProdDetailID_to);
            listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.ConversionSub_ID);
            listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.ConversionID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
                listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.BarCode_from);
                listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.BarCode_to);
            }
            listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.ConversionID);
            listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.ConversionID);

            List<SourceToTargetMatchCol> sourceToTargetMatchesA = new List<SourceToTargetMatchCol>();
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.SKU, t => t.SKU_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.CNName, t => t.CNName_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.prop, t => t.property_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.Specifications, t => t.Specifications_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.Type_ID, t => t.Type_ID_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.Model, t => t.Model_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.ProdDetailID, t => t.ProdDetailID_from);
            sourceToTargetMatchesA.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.BarCode, t => t.BarCode_from);



            List<SourceToTargetMatchCol> sourceToTargetMatchesB = new List<SourceToTargetMatchCol>();


            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.SKU, t => t.SKU_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.CNName, t => t.CNName_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.prop, t => t.property_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.Specifications, t => t.Specifications_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.Type_ID, t => t.Type_ID_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.Model, t => t.Model_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.ProdDetailID, t => t.ProdDetailID_to);
            sourceToTargetMatchesB.SetSourceToTargetMatchCol<View_ProdDetail, tb_ProdConversionDetail>(s => s.BarCode, t => t.BarCode_to);


            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.SKU_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.CNName_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.property_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.Model_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.Specifications_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.Type_ID_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.ProdDetailID_from, sourceToTargetMatchesA);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.BarCode_from, sourceToTargetMatchesA);


            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.SKU_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.CNName_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.property_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.Model_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.Specifications_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.Type_ID_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.ProdDetailID_to, sourceToTargetMatchesB);
            listCols.SetCol_EditorDataSource<View_ProdDetail, tb_ProdConversionDetail>(c => c.BarCode_to, sourceToTargetMatchesB);
            
            //产品选择时不能多选。会出问题。
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.SKU_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.CNName_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.property_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.Model_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.Specifications_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.Type_ID_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.ProdDetailID_from, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.BarCode_from, false);

            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.SKU_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.CNName_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.property_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.Model_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.Specifications_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.Type_ID_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.ProdDetailID_to, false);
            listCols.SetCol_CanMuliSelect<tb_ProdConversionDetail>(c => c.BarCode_to, false);



            ControlChildColumnsInvisible(listCols);
            ////实际在中间实体定义时加了只读属性，功能相同
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            //listCols.SetCol_ReadOnly<tb_ProdConversionDetail>(c => c.ConversionQty);




            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_ProdConversionDetail>(c => c.SubtotalCostAmount);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_ProdConversionDetail>(c => c.ConversionQty);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdConversionDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdConversionDetail>(sgd, f => f.prop, t => t.property_from);

            //应该只提供一个结构
            List<tb_ProdConversionDetail> lines = new List<tb_ProdConversionDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //     .AndIF(true, w => w.CNName.Length > 0)
            //    // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //    .ToExpression();//注意 这一句 不能少
            //                    // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_ProdConversionDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdConversionDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;
            grid1.Enter += Grid1_Enter;
            base.ControlMasterColumnsInvisible();
        }

        private void Sgh_OnLoadRelevantFields(object _View_ProdDetail, object rowObj, SourceGridDefine griddefine, Position Position)
        {
            if (EditEntity == null)
            {
                return;
            }

            View_ProdDetail vp = (View_ProdDetail)_View_ProdDetail;
            tb_ProdConversionDetail _SDetail = (tb_ProdConversionDetail)rowObj;
            //通过产品查询页查出来后引过来才有值，如果直接在输入框输入SKU这种唯一的。就没有则要查一次。这时是缓存了？
            if (vp.ProdDetailID > 0 && EditEntity.Employee_ID > 0)
            {
                _SDetail.property_from = vp.prop;
                var Col = griddefine.grid.Columns.GetColumnInfo(griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_ProdConversionDetail.property_from)).UniqueId);
                if (Col!=null)
                {
                    griddefine.grid[Position.Row, Col.Index].Value = _SDetail.property_from;
                }
                
            }
        }


        private void Grid1_Enter(object sender, EventArgs e)
        {
            SelectLocationTips();
        }
        private void SelectLocationTips()
        {
            if (EditEntity == null)
            {
                return;
            }
            if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            {
                MessageBox.Show("请选择【所在库位】。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLocation_ID.Focus();
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
                List<tb_ProdConversionDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProdConversionDetail>;
                details = details.Where(c => c.ProdDetailID_from > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                EditEntity.TotalConversionQty = details.Sum(c => Math.Abs(c.ConversionQty));
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }

        }


        List<tb_ProdConversionDetail> details = new List<tb_ProdConversionDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalConversionQty);
            bindingSourceSub.EndEdit();
            List<tb_ProdConversionDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProdConversionDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID_from > 0 && t.ProdDetailID_to > 0).ToList();

                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                //如果需验证时，则明细中转换的数量的绝对值的和不能为零。
                if (NeedValidated && details.Sum(c => Math.Abs(c.ConversionQty)) == 0)
                {
                    System.Windows.Forms.MessageBox.Show("转换数量不能为零。请检查数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (EditEntity.Employee_ID == 0 || EditEntity.Employee_ID == -1)
                {
                    EditEntity.Employee_ID = null;
                }


                //如果明细来源和目标相同，则提示
                if (NeedValidated && details.Any(c => c.ProdDetailID_from == c.ProdDetailID_to))
                {
                    System.Windows.Forms.MessageBox.Show("明细中，来源和目标产品不能相同。请检查数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                var aa = details.Select(c => c.ProdDetailID_from).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.TotalConversionQty = details.Sum(c => c.ConversionQty);
                EditEntity.tb_ProdConversionDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ProdConversionDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalConversionQty != details.Sum(c => c.ConversionQty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_ProdConversion> SaveResult = new ReturnMainSubResults<tb_ProdConversion>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ConversionNo}。");
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





    }
}
