﻿using System;
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
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("其他出库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.其他出入库管理, BizType.其他出库单)]
    public partial class UCStockOut : BaseBillEditGeneric<tb_StockOut, tb_StockOutDetail>, IPublicEntityObject
    {
        public UCStockOut()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }
 


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_StockOut);
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_StockOut).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        //private tb_StockOut _EditEntity;
        //public tb_StockOut EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(tb_StockOut entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.MainID > 0)
            {
                entity.PrimaryKeyID = entity.MainID;
                entity.ActionStatus = ActionStatus.加载;
                //  entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (string.IsNullOrEmpty(entity.BillNo))
                {
                    entity.BillNo = ClientBizCodeService.GetBizBillNo(BizType.其他出库单);
                }
                entity.Bill_Date = System.DateTime.Now;
                entity.Out_date = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            }
            DataBindingHelper.BindData4Cmb<tb_OutInStockType>(entity, k => k.Type_ID, v => v.TypeName, cmbType_ID, c => c.OutIn == false);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_StockOut>(entity, t => t.Bill_Date, dtpBill_Date, true);
            DataBindingHelper.BindData4DataTime<tb_StockOut>(entity, t => t.Out_date, dtpOut_date, true);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockOut>(entity, t => t.RefNO, txtRefNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_StockOut>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_StockOut>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_StockOutDetails != null && entity.tb_StockOutDetails.Count > 0)
            {
                // details = entity.tb_StockOutDetails;
                sgh.LoadItemDataToGrid<tb_StockOutDetail>(grid1, sgd, entity.tb_StockOutDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_StockOutDetail>(grid1, sgd, new List<tb_StockOutDetail>(), c => c.ProdDetailID);
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
                    base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService <tb_StockOutValidator> (), kryptonPanelMainInfo.Controls);
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
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService <tb_StockOutValidator> (), kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UCStockOut_Load(object sender, System.EventArgs e)
        {

            InitDataTocmbbox();
            


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_StockOutDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_StockOutDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_StockOutDetail>(c => c.Detaill_ID);
            listCols.SetCol_NeverVisible<tb_StockOutDetail>(c => c.MainID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            //具体审核权限的人才显示
            /*
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_StockOutDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_StockOutDetail>(c => c.SubtotalCostAmount);
                listCols.SetCol_NeverVisible<tb_StockOutDetail>(c => c.SubtotalPirceAmount);
            }
            */



            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_StockOutDetail>(c => c.Qty);


            listCols.SetCol_Formula<tb_StockOutDetail>((a, b) => a.Cost * b.Qty, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_StockOutDetail>((a, b) => a.Price * b.Qty, c => c.SubtotalPirceAmount);



            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockOutDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockOutDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockOutDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockOutDetail>(sgd, f => f.Standard_Price, t => t.Price);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockOutDetail>(sgd, f => f.prop, t => t.property);




            //应该只提供一个结构
            List<tb_StockOutDetail> lines = new List<tb_StockOutDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //   .AndIF(true, w => w.CNName.Length > 0)
            //  // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //  .ToExpression();//注意 这一句 不能少
            //                  // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_StockOutDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_StockOutDetail));
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
                List<tb_StockOutDetail> details = new List<tb_StockOutDetail>();
                
                foreach (var item in RowDetails)
                {
                    tb_StockOutDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_StockOutDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_StockOutDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }
        }

        /// <summary>
        /// 明细列值变更时计算汇总金额
        /// </summary>
        /// <param name="_rowObj">行对象</param>
        /// <param name="myGridDefine">网格定义</param>
        /// <param name="position">位置</param>
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
                List<tb_StockOutDetail> details = sgd.BindingSourceLines.DataSource as List<tb_StockOutDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                // 获取系统配置的金额精度
                int precision = MainForm.Instance.authorizeController.GetMoneyDataPrecision();
                
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Qty).ToRoundDecimalPlaces(precision);
                EditEntity.TotalAmount = details.Sum(c => c.Price * c.Qty).ToRoundDecimalPlaces(precision);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_StockOutDetail> details = new List<tb_StockOutDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_StockOutDetail> detailentity = bindingSourceSub.DataSource as List<tb_StockOutDetail>;
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
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.tb_StockOutDetails = details;
                // 获取系统配置的金额精度
                int precision = MainForm.Instance.authorizeController.GetMoneyDataPrecision();
                
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Qty).ToRoundDecimalPlaces(precision);
                EditEntity.TotalAmount = details.Sum(c => c.Price * c.Qty).ToRoundDecimalPlaces(precision);
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_StockOutDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_StockOut> SaveResult = new ReturnMainSubResults<tb_StockOut>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BillNo}。");
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

    RevertCommand command = new RevertCommand();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_StockOut oldobj = CloneHelper.DeepCloneObject<tb_StockOut>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_StockOut>(EditEntity, oldobj);
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
    tb_StockOutController<tb_StockOut> ctr = Startup.GetFromFac<tb_StockOutController<tb_StockOut>>();
    ReturnResults<tb_StockOut> rmrs = await ctr.ApprovalAsync(EditEntity);
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


    if (EditEntity.tb_StockOutDetails == null || EditEntity.tb_StockOutDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
        return;
    }

    RevertCommand command = new RevertCommand();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_StockOut oldobj = CloneHelper.DeepCloneObject<tb_StockOut>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_StockOut>(EditEntity, oldobj);
    };

    tb_StockOutController<tb_StockOut> ctr = Startup.GetFromFac<tb_StockOutController<tb_StockOut>>();
    List<tb_StockOut> list = new List<tb_StockOut>();
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
