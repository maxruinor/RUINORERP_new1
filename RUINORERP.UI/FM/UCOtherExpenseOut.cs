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
using Krypton.Toolkit;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("其他费用支出", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.收付账款, BizType.其他费用支出)]
    public partial class UCOtherExpenseOut : BaseBillEditGeneric<tb_FM_OtherExpense, tb_FM_OtherExpenseDetail>
    {
        public UCOtherExpenseOut()
        {
            InitializeComponent();
             

        }
    

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_FM_OtherExpense);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            rdb收入.Visible = false;
            rdb支出.Visible = false;
            lblEXPOrINC.Visible = false;
            kryptonGroupBox4.Visible = false;
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
        }

        public override void BindData(tb_FM_OtherExpense entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {
                return;
            }
            EditEntity = entity;
            if (entity.ExpenseMainID > 0)
            {
                entity.PrimaryKeyID = entity.ExpenseMainID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                }
                entity.DocumentDate = System.DateTime.Now;
                if (CurMenuInfo != null)
                {
                    lbl盘点单.Text = CurMenuInfo.CaptionCN;
                    if (CurMenuInfo.CaptionCN.Contains("支出"))
                    {
                        entity.EXPOrINC = false;
                        entity.ExpenseNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.其他费用支出);
                    }

                    if (CurMenuInfo.CaptionCN.Contains("收入"))
                    {
                        entity.EXPOrINC = true;
                        entity.ExpenseNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.其他费用收入);
                    }
                }



            }

            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_FM_OtherExpense>(entity, t => t.EXPOrINC, rdb收入, rdb支出);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ExpenseNo, txtExpenseNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_FM_OtherExpense>(entity, t => t.DocumentDate, dtpDocumentDate, false);
            //DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpense>(entity, t => t.EXPOrINC, chkEXPOrINC, false);
            //有默认值
            DataBindingHelper.BindData4CheckBox<tb_FM_OtherExpense>(entity, t => t.IncludeTax, chkIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_OtherExpense>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_OtherExpense>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            txtstatus.ReadOnly = true;
            if (entity.tb_FM_OtherExpenseDetails != null && entity.tb_FM_OtherExpenseDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_FM_OtherExpenseDetail>(grid1, sgd, entity.tb_FM_OtherExpenseDetails, c => c.ExpenseSubID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_OtherExpenseDetail>(grid1, sgd, new List<tb_FM_OtherExpenseDetail>(), c => c.ExpenseSubID);
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


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        tb_FM_OtherExpenseDetailController<tb_FM_OtherExpenseDetail> dc = Startup.GetFromFac<tb_FM_OtherExpenseDetailController<tb_FM_OtherExpenseDetail>>();
        List<tb_FM_OtherExpenseDetail> list = new List<tb_FM_OtherExpenseDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            if (CurMenuInfo != null)
            {
                lbl盘点单.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_OtherExpenseDetail>();

            listCols.SetCol_NeverVisible<tb_FM_OtherExpenseDetail>(c => c.ExpenseMainID);
            listCols.SetCol_NeverVisible<tb_FM_OtherExpenseDetail>(c => c.ExpenseSubID);



            //listCols.SetCol_ReadOnly<tb_FM_OtherExpenseDetail>(c => c.CNName);


            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.TotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_OtherExpenseDetail>(c => c.UntaxedAmount, CustomFormatType.CurrencyFormat);
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
            listCols.SetCol_Summary<tb_FM_OtherExpenseDetail>(c => c.TotalAmount);
            listCols.SetCol_Summary<tb_FM_OtherExpenseDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_FM_OtherExpenseDetail>(c => c.UntaxedAmount);

            listCols.SetCol_Formula<tb_FM_OtherExpenseDetail>((a, b, c) => a.TotalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);
            listCols.SetCol_Formula<tb_FM_OtherExpenseDetail>((a, b) => a.TotalAmount - b.TaxAmount, c => c.UntaxedAmount);


            //应该只提供一个结构
            List<tb_FM_OtherExpenseDetail> lines = new List<tb_FM_OtherExpenseDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_OtherExpenseDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            base.ControlMasterColumnsInvisible();
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
                List<tb_FM_OtherExpenseDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_OtherExpenseDetail>;
                details = details.Where(c => c.TotalAmount > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.TotalAmount = details.Sum(c => c.TotalAmount);

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_OtherExpense).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_OtherExpense>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                                .And(t => t.EXPOrINC == false)
                            // .And(t => t.Is_enabled == true)
                            //报销人员限制，财务不限制
                            //  .AndIF(MainForm.Instance.AppContext.SysConfig.SaleBizLimited && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }

        List<tb_FM_OtherExpenseDetail> details = new List<tb_FM_OtherExpenseDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_OtherExpenseDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_OtherExpenseDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.TotalAmount > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                EditEntity.tb_FM_OtherExpenseDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_OtherExpenseDetail>(details))
                {
                    return false;
                }
                //必需选一个
                if (NeedValidated && (!rdb支出.Checked && !rdb收入.Checked))
                {
                    MessageBox.Show("收入和支出必需选其一！");
                    return false;
                }
                EditEntity.TaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.TotalAmount = details.Sum(c => c.TotalAmount);


                ReturnMainSubResults<tb_FM_OtherExpense> SaveResult = new ReturnMainSubResults<tb_FM_OtherExpense>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ExpenseNo}。");
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

        /// <summary>
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <returns></returns>

        protected async override Task<ApprovalEntity> Review()
        {
            ApprovalEntity ae = await base.Review();
            if (ae.ApprovalResults)
            {
                EditEntity.DataStatus = (int)DataStatus.完结;
                await AppContext.Db.Updateable(EditEntity).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
            }
            return ae;
        }

        /*
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


            if (EditEntity.tb_FM_OtherExpenseDetails == null || EditEntity.tb_FM_OtherExpenseDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整金额。", UILogType.警告);
                return ae;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_FM_OtherExpense oldobj = CloneHelper.DeepCloneObject<tb_FM_OtherExpense>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
            };

            tb_FM_OtherExpenseController<tb_FM_OtherExpense> ctr = Startup.GetFromFac<tb_FM_OtherExpenseController<tb_FM_OtherExpense>>();
            List<tb_FM_OtherExpense> list = new List<tb_FM_OtherExpense>();
            list.Add(EditEntity);
            ReturnResults<bool> rrs = await ctr.AntiApprovalAsync(list);
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
                MainForm.Instance.PrintInfoLog($"{EditEntity.ExpenseNo}反审失败,请联系管理员！", Color.Red);
            }
            return ae;
        }

        */




    }
}
