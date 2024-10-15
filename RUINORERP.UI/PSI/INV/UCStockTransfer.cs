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
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("调拨单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.调拨单)]
    public partial class UCStockTransfer : BaseBillEditGeneric<tb_StockTransfer, tb_StockTransferDetail>
    {
        public UCStockTransfer()
        {
            InitializeComponent();
        }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_StockTransfer);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            //DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            //DataBindingHelper.InitDataToCmb<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName, cmbType_ID, c => c.OutIn == false);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_StockTransfer).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        //private tb_StockTransfer _EditEntity;
        //public tb_StockTransfer EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(tb_StockTransfer entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.StockTransferID > 0)
            {
                entity.PrimaryKeyID = entity.StockTransferID;
                entity.ActionStatus = ActionStatus.加载;
                //  entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.StockTransferNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.调拨单);
                entity.Transfer_date = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            }


            DataBindingHelper.BindData4CmbRefWithLimitedByAlias<tb_Location>(entity, nameof(tb_Location.Location_ID), nameof(tb_StockTransfer.Location_ID_from), nameof(tb_Location.Name), nameof(tb_Location), cmbLocation_ID_from, c => c.Is_enabled == true);
            DataBindingHelper.BindData4CmbRefWithLimitedByAlias<tb_Location>(entity, nameof(tb_Location.Location_ID), nameof(tb_StockTransfer.Location_ID_to), nameof(tb_Location.Name), nameof(tb_Location), cmbLocation_ID_to, c => c.Is_enabled == true);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.StockTransferNo, txtStockTransferNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.TotalTransferAmount.ToString(), txtTotalTransferAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4DataTime<tb_StockTransfer>(entity, t => t.Transfer_date, dtpTransfer_date, true);
            DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4ControlByEnum<tb_StockTransfer>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_StockTransfer>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            
            if (entity.tb_StockTransferDetails != null && entity.tb_StockTransferDetails.Count > 0)
            {
                // details = entity.tb_StockTransferDetails;
                sgh.LoadItemDataToGrid<tb_StockTransferDetail>(grid1, sgd, entity.tb_StockTransferDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_StockTransferDetail>(grid1, sgd, new List<tb_StockTransferDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    base.InitRequiredToControl(new tb_StockTransferValidator(), kryptonPanelMainInfo.Controls);
                    // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
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

     
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_StockTransferValidator(), kryptonPanelMainInfo.Controls);
            }
            ToolBarEnabledControl(entity);

            ControlMasterColumnsInvisible();
        }


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UCStockOut_Load(object sender, System.EventArgs e)
        {

            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_StockTransferDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_StockTransferDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_StockTransferDetail>(c => c.StockTransferDetaill_ID);
            listCols.SetCol_NeverVisible<tb_StockTransferDetail>(c => c.StockTransferID);
            ControlChildColumnsInvisible(listCols);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            //具体审核权限的人才显示
            /*
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_StockTransferDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_StockTransferDetail>(c => c.SubtotalCostAmount);
                listCols.SetCol_NeverVisible<tb_StockTransferDetail>(c => c.SubtotalPirceAmount);
            }
            */



            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_StockTransferDetail>(c => c.Qty);


            listCols.SetCol_Formula<tb_StockTransferDetail>((a, b) => a.Cost * b.Qty, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_StockTransferDetail>((a, b) => a.TransPrice * b.Qty, c => c.SubtotalTransferPirceAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockTransferDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockTransferDetail>(sgd, f => f.TransPrice, t => t.TransPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockTransferDetail>(sgd, f => f.prop, t => t.property);


            //应该只提供一个结构
            List<tb_StockTransferDetail> lines = new List<tb_StockTransferDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
               .AndIF(true, w => w.CNName.Length > 0)
              // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
              .ToExpression();//注意 这一句 不能少
                              // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);

            sgd.SetDependencyObject<ProductSharePart, tb_StockTransferDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_StockTransferDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
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
                List<tb_StockTransferDetail> details = new List<tb_StockTransferDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_StockTransferDetail bOM_SDetail = mapper.Map<tb_StockTransferDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_StockTransferDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_StockTransferDetail> details = sgd.BindingSourceLines.DataSource as List<tb_StockTransferDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Qty);
                EditEntity.TotalTransferAmount = details.Sum(c => c.TransPrice * c.Qty);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_StockTransferDetail> details = new List<tb_StockTransferDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_StockTransferDetail> detailentity = bindingSourceSub.DataSource as List<tb_StockTransferDetail>;
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
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.tb_StockTransferDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_StockTransferDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_StockTransfer> SaveResult = new ReturnMainSubResults<tb_StockTransfer>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.StockTransferNo}。");
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

    Command command = new Command();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_StockTransfer oldobj = CloneHelper.DeepCloneObject<tb_StockTransfer>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_StockTransfer>(EditEntity, oldobj);
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
    tb_StockTransferController<tb_StockTransfer> ctr = Startup.GetFromFac<tb_StockTransferController<tb_StockTransfer>>();
    ReturnResults<tb_StockTransfer> rmrs = await ctr.ApprovalAsync(EditEntity);
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
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
        MainForm.Instance.PrintInfoLog(rmrs.ErrorMsg);
    }

    return ae;
}




/// <summary>
/// 列表中不再实现反审，批量，出库反审情况极少。并且是仔细处理
/// </summary>
protected async override void ReReview()
{
    if (EditEntity == null)
    {
        return;
    }

    //反审，要审核过，并且通过了，才能反审。
    if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
    {
        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
        return;
    }


    if (EditEntity.tb_StockTransferDetails == null || EditEntity.tb_StockTransferDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
        return;
    }

    Command command = new Command();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_StockTransfer oldobj = CloneHelper.DeepCloneObject<tb_StockTransfer>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_StockTransfer>(EditEntity, oldobj);
    };

    tb_StockTransferController<tb_StockTransfer> ctr = Startup.GetFromFac<tb_StockTransferController<tb_StockTransfer>>();
    List<tb_StockTransfer> list = new List<tb_StockTransfer>();
    list.Add(EditEntity);
    bool Succeeded = await ctr.AntiApprovalAsync(list);
    if (Succeeded)
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
        MainForm.Instance.PrintInfoLog($"销售出库单{EditEntity.BillNo}反审失败,请联系管理员！", Color.Red);
    }

}


*/



    }
}
