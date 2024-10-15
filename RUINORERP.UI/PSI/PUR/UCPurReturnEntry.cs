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

using Microsoft.Extensions.Logging;
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using Krypton.Toolkit;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.UI.PSI.SAL;
using EnumsNET;
using RUINORERP.UI.ToolForm;


namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("采购退货入库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.采购管理, BizType.采购退货入库单)]
    public partial class UCPurReturnEntry : BaseBillEditGeneric<tb_PurEntryRe, tb_PurEntryReDetail>
    {
        public UCPurReturnEntry()
        {
            InitializeComponent();
            
        }
 
        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_PurEntryRe);
        }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            //DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            //DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);

            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);

        }
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_PurEntryRe).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public override void BindData(tb_PurEntryRe entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.PurEntryRe_ID > 0)
            {
                entity.PrimaryKeyID = entity.PurEntryRe_ID;
                entity.ActionStatus = ActionStatus.加载;
                //entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.PurEntryReNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购退回单);
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_PurEntryReDetails != null && entity.tb_PurEntryReDetails.Count > 0)
                {
                    entity.tb_PurEntryReDetails.ForEach(c => c.PurEntryRe_ID = 0);
                    entity.tb_PurEntryReDetails.ForEach(c => c.PurEntryRe_CID = 0);
                }
            }

            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, v => v.PurEntryNo, txtPurEntryNo, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4CmbByEnum<tb_PurEntryRe>(entity, k => k.ProcessWay, typeof(PurReProcessWay), cmbProcessWay, true);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.PurEntryReNo, txtPurEntryRENo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ActualAmount.ToString(), txtActualAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4DataTime<tb_PurEntryRe>(entity, t => t.BillDate, dtpBillDate, false);

            DataBindingHelper.BindData4DataTime<tb_PurEntryRe>(entity, t => t.ReturnDate, dtpReturnDate, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalDiscountAmount.ToString(), txtDiscountAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
            DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_PurEntryRe>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_PurEntryRe>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_PurEntryReDetails != null && entity.tb_PurEntryReDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_PurEntryReDetail>(grid1, sgd, entity.tb_PurEntryReDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_PurEntryReDetail>(grid1, sgd, new List<tb_PurEntryReDetail>(), c => c.ProdDetailID);
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

                //如果是采购入库引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.PurEntryID > 0 && s2.PropertyName == entity.GetPropertyName<tb_PurEntryRe>(c => c.PurEntryID))
                {
                    LoadRefBillData(entity.PurEntryID);
                }
                else
                {
                    MainForm.Instance.PrintInfoLog(entity.ActionStatus.GetName());
                }

            };

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_PurEntryReValidator(), kryptonSplitContainer1.Panel1.Controls);
                //  base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }








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
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_PurEntry>(entity, v => v.PurEntryID, txtPurEntryNo, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaOrder = Expressionable.Create<tb_PurEntry>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                             .And(t => t.isdeleted == false)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessorPUR = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_PurEntry).Name + "Processor");
            QueryFilter queryFilterPUR = baseProcessorPUR.GetQueryFilter();
            DataBindingHelper.InitFilterForControlByExp<tb_PurEntry>(entity, txtPurEntryNo, c => c.PurEntryNo, queryFilterPUR);

            ToolBarEnabledControl(entity);
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_PurEntryReDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_PurEntryReDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_PurEntryReDetail>(c => c.PurEntryRe_CID);
            listCols.SetCol_NeverVisible<tb_PurEntryReDetail>(c => c.PurEntryRe_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            ControlChildColumnsInvisible(listCols);

            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            listCols.SetCol_Format<tb_PurEntryReDetail>(c => c.Discount, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_PurEntryReDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_PurEntryReDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;

            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurEntryReDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryReDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryReDetail>(c => c.SubtotalPirceAmount);
            }*/

            listCols.SetCol_Summary<tb_PurEntryReDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_PurEntryReDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_PurEntryReDetail>(c => c.SubtotalTrPriceAmount);
            listCols.SetCol_Summary<tb_PurEntryReDetail>(c => c.DiscountAmount);

            listCols.SetCol_Formula<tb_PurEntryReDetail>((a, b, c) => a.UnitPrice * b.Discount - c.SubtotalTrPriceAmount, d => d.DiscountAmount);
            listCols.SetCol_Formula<tb_PurEntryReDetail>((a, b) => a.UnitPrice * b.Discount, c => c.TransactionPrice);
            listCols.SetCol_Formula<tb_PurEntryReDetail>((a, b, c) => a.UnitPrice * b.Discount * c.Quantity, c => c.SubtotalTrPriceAmount);
            listCols.SetCol_Formula<tb_PurEntryReDetail>((a, b, c) => a.SubtotalTrPriceAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryReDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryReDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurEntryReDetail>(sgd, f => f.prop, t => t.property);





            //应该只提供一个结构
            List<tb_PurEntryReDetail> lines = new List<tb_PurEntryReDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                   .AndIF(true, w => w.CNName.Length > 0)
                  // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
                  .ToExpression();//注意 这一句 不能少
                                  // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);
            sgd.SetDependencyObject<ProductSharePart, tb_PurEntryReDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_PurEntryReDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
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
                List<tb_PurEntryReDetail> details = sgd.BindingSourceLines.DataSource as List<tb_PurEntryReDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalDiscountAmount = details.Sum(c => c.DiscountAmount);
                EditEntity.TotalAmount = details.Sum(c => c.SubtotalTrPriceAmount);
                EditEntity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_PurEntryReDetail> details = new List<tb_PurEntryReDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_PurEntryReDetail> detailentity = bindingSourceSub.DataSource as List<tb_PurEntryReDetail>;
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

                EditEntity.tb_PurEntryReDetails = details;


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_PurEntryReDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

               
                ReturnMainSubResults<tb_PurEntryRe> SaveResult = new ReturnMainSubResults<tb_PurEntryRe>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PurEntryReNo}。");
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
    tb_PurEntryRe oldobj = CloneHelper.DeepCloneObject<tb_PurEntryRe>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_PurEntryRe>(EditEntity, oldobj);
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

    // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
    //因为只需要更新主表
    //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
    // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
    tb_PurEntryReController<tb_PurEntryRe> ctr = Startup.GetFromFac<tb_PurEntryReController<tb_PurEntryRe>>();
    ReturnResults<bool> rs = await ctr.ApprovalAsync(EditEntity, ae);
    if (rs.Succeeded)
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
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rs.ErrorMsg},请联系管理员！", Color.Red);
    }

    return ae;
}


/// <summary>
/// 反审核
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


    if (EditEntity.tb_PurEntryReDetails == null || EditEntity.tb_PurEntryReDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
        return;
    }

    Command command = new Command();

    tb_PurEntryRe oldobj = CloneHelper.DeepCloneObject<tb_PurEntryRe>(EditEntity);
    command.UndoOperation = delegate ()
    {
        CloneHelper.SetValues<tb_PurEntryRe>(EditEntity, oldobj);
    };

    tb_PurEntryReController<tb_PurEntryRe> ctr = Startup.GetFromFac<tb_PurEntryReController<tb_PurEntryRe>>();

    ReturnResults<bool> rs = await ctr.AntiApprovalAsync(EditEntity);
    if (rs.Succeeded)
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
        MainForm.Instance.PrintInfoLog($"{EditEntity.PurEntryNo}反审失败,{rs.ErrorMsg},请联系管理员！", Color.Red);
    }

}
*/


        string purEntryid = string.Empty;

        /// <summary>
        /// 将采购入库单转换为采购入库退货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadRefBillData(long? saleoutid)
        {
            //要加一个判断 值是否有变化
            //新增时才可以

            ButtonSpecAny bsa = (txtPurEntryNo as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var purEntry = bsa.Tag as tb_PurEntry;
            if (purEntry != null)
            {
                if (!string.IsNullOrEmpty(purEntryid) && purEntryid.Equals(purEntry.PurEntryID.ToString()))
                {
                    return;
                }
                purEntryid = purEntry.PurEntryID.ToString();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                tb_PurEntryRe entity = mapper.Map<tb_PurEntryRe>(purEntry);
                List<tb_PurEntryReDetail> details = mapper.Map<List<tb_PurEntryReDetail>>(purEntry.tb_PurEntryDetails);

                List<tb_PurEntryReDetail> NewDetails = new List<tb_PurEntryReDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_PurEntryDetail item = purEntry.tb_PurEntryDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = details[i].Quantity - item.ReturnedQty;// 减掉已经退回的数量
                    details[i].SubtotalTrPriceAmount = details[i].TransactionPrice * details[i].Quantity;

                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"当前行的SKU:{item.tb_proddetail.SKU}已退回数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    }
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"采购入库单:{entity.PurEntryNo}已全部退回，请检查是否正在重复操作！");
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




                entity.tb_PurEntryReDetails = NewDetails;
                entity.TotalAmount = NewDetails.Sum(c => c.SubtotalTrPriceAmount);
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.TotalTaxAmount = NewDetails.Sum(c => c.TaxAmount);
                entity.TotalDiscountAmount = NewDetails.Sum(c => c.DiscountAmount);


                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;
                entity.ReturnDate = System.DateTime.Now;
                if (entity.PurEntryID > 0)
                {
                    entity.CustomerVendor_ID = purEntry.CustomerVendor_ID;
                    entity.PurEntryNo = purEntry.PurEntryNo;
                }
                BusinessHelper.Instance.InitEntity(entity);
                BindData(entity as tb_PurEntryRe);
            }

        }
    }
}
