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
using RUINORERP.Business.Security;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.Global.EnumExt;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using NPOI.SS.Formula.Functions;

namespace RUINORERP.UI.FM.FMBase
{
    //损益费用单
    public partial class UCProfitLoss : BaseBillEditGeneric<tb_FM_ProfitLoss, tb_FM_ProfitLossDetail>
    {
        public UCProfitLoss()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 亏盈方向。收款=收入， 支出=付款
        /// </summary>
        public ProfitLossDirection profitLossDirect { get; set; }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_FM_ProfitLoss);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
        }
        public override void BindData(tb_FM_ProfitLoss entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {
                return;
            }
            EditEntity = entity;
            if (entity.ProfitLossId > 0)
            {
                entity.PrimaryKeyID = entity.ProfitLossId;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.PostDate = System.DateTime.Now;
                if (CurMenuInfo != null)
                {
                    lbl盘点单.Text = CurMenuInfo.CaptionCN;
                    if (profitLossDirect == ProfitLossDirection.损失)
                    {
                        if (string.IsNullOrEmpty(entity.ProfitLossNo))
                        {
                            entity.ProfitLossNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.损失确认单);
                        }
                    }

                    if (profitLossDirect == ProfitLossDirection.溢余)
                    {
                        if (string.IsNullOrEmpty(entity.ProfitLossNo))
                        {
                            entity.ProfitLossNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.溢余确认单);
                        }
                    }
                    entity.ProfitLossDirection = (int)profitLossDirect;
                }
            }


            DataBindingHelper.BindData4ControlByEnum<tb_FM_ProfitLoss>(entity, t => t.ProfitLossDirection, lblProfitLossDirection, BindDataType4Enum.EnumName, typeof(ProfitLossDirection));

            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.TotalAmount.ToString(), txtTaxTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.ProfitLossNo, txtProfitLossNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4ControlByEnum<tb_FM_ProfitLoss>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ProfitLoss>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(DataStatus));

            DataBindingHelper.BindData4ControlByEnum<tb_FM_ProfitLoss>(entity, t => t.SourceBizType, cmbSourceBizType, BindDataType4Enum.EnumName, typeof(BizType));

            // DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4DataTime<tb_FM_ProfitLoss>(entity, t => t.PostDate, dtpPostDate, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.IsExpenseType, chkIsExpenseType, false);

            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupCode, cmbProjectGroup_ID);
            DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.TaxTotalAmount.ToString(), txtTaxTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.UntaxedTotalAmont.ToString(), txtUntaxedTotalAmont, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.TotalAmount, txtTotalAmount, BindDataType4TextBox.Money, false);


            if (entity.tb_FM_ProfitLossDetails != null && entity.tb_FM_ProfitLossDetails.Count > 0)
            {
                details = entity.tb_FM_ProfitLossDetails;
                sgh.LoadItemDataToGrid<tb_FM_ProfitLossDetail>(grid1, sgd, entity.tb_FM_ProfitLossDetails, c => c.ProfitLossId);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_ProfitLossDetail>(grid1, sgd, new List<tb_FM_ProfitLossDetail>(), c => c.ProfitLossId);
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
            //显示结案凭证图片
            base.BindData(entity);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ProfitLoss).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            //创建表达式
            var lambda = Expressionable.Create<tb_FM_ProfitLoss>()
                         //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                         .And(t => t.isdeleted == false)
                         .And(t => t.ProfitLossDirection == (int)profitLossDirect)

                        //报销人员限制，财务不限制

                        .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }




        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        tb_FM_ProfitLossDetailController<tb_FM_ProfitLossDetail> dc = Startup.GetFromFac<tb_FM_ProfitLossDetailController<tb_FM_ProfitLossDetail>>();
        List<tb_FM_ProfitLossDetail> list = new List<tb_FM_ProfitLossDetail>();

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


            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<tb_FM_ProfitLossDetail>();

            listCols.SetCol_NeverVisible<tb_FM_ProfitLossDetail>(c => c.ProfitLossId);
            listCols.SetCol_NeverVisible<tb_FM_ProfitLossDetail>(c => c.ProfitLossDetail_ID);


            //listCols.SetCol_ReadOnly<tb_FM_ProfitLossDetail>(c => c.CNName);


            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.SubtotalAmont, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.TaxSubtotalAmont, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.UntaxedSubtotalAmont, CustomFormatType.CurrencyFormat);

            //if (profitLossDirect == ProfitLossDirection.损失)
            //{
            //    listCols.SetCol_Format<tb_FM_ProfitLossDetail, ProfitLossType>(o => o.ProfitLossType, EnumFilter<ProfitLossType>.LessThan(ProfitLossType.其他经营损失));
            //}
            //else
            //{
            //    listCols.SetCol_Format<tb_FM_ProfitLossDetail, ProfitLossType>(o => o.ProfitLossType, EnumFilter<ProfitLossType>.GreaterThan(ProfitLossType.库存盘盈));
            //}


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
            listCols.SetCol_Summary<tb_FM_ProfitLossDetail>(c => c.SubtotalAmont);
            listCols.SetCol_Summary<tb_FM_ProfitLossDetail>(c => c.TaxSubtotalAmont);
            listCols.SetCol_Summary<tb_FM_ProfitLossDetail>(c => c.UntaxedSubtotalAmont);

            listCols.SetCol_Formula<tb_FM_ProfitLossDetail>((a, b, c) => a.SubtotalAmont / (1 + b.TaxRate) * c.TaxRate, d => d.TaxSubtotalAmont);
            listCols.SetCol_Formula<tb_FM_ProfitLossDetail>((a, b) => a.UnitPrice * b.Quantity, c => c.SubtotalAmont);
            listCols.SetCol_Formula<tb_FM_ProfitLossDetail>((a, b) => a.SubtotalAmont - b.TaxSubtotalAmont, c => c.UntaxedSubtotalAmont);

            //枚举值 排除或只选择
            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.ProfitLossType, CustomFormatType.EnumOptions, null, typeof(ProfitLossType));
            listCols.SetCol_Format<tb_FM_ProfitLossDetail>(c => c.IncomeExpenseDirection, CustomFormatType.EnumOptions, null, typeof(IncomeExpenseDirection));

            //应该只提供一个结构
            List<tb_FM_ProfitLossDetail> lines = new List<tb_FM_ProfitLossDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_ProfitLossDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
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
                List<tb_FM_ProfitLossDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_ProfitLossDetail>;
                details = details.Where(c => c.SubtotalAmont != 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                EditEntity.TaxTotalAmount = details.Sum(c => c.TaxSubtotalAmont);
                EditEntity.TotalAmount = details.Sum(c => c.SubtotalAmont);
                EditEntity.UntaxedTotalAmont = EditEntity.TotalAmount - EditEntity.TaxTotalAmount;
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }



        List<tb_FM_ProfitLossDetail> details = new List<tb_FM_ProfitLossDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_ProfitLossDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_ProfitLossDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.SubtotalAmont > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                EditEntity.tb_FM_ProfitLossDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_ProfitLossDetail>(details))
                {
                    return false;
                }

                EditEntity.TaxTotalAmount = details.Sum(c => c.TaxSubtotalAmont);
                EditEntity.TotalAmount = details.Sum(c => c.SubtotalAmont);
                EditEntity.UntaxedTotalAmont = EditEntity.TotalAmount - EditEntity.TaxTotalAmount;


                ReturnMainSubResults<tb_FM_ProfitLoss> SaveResult = new ReturnMainSubResults<tb_FM_ProfitLoss>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ProfitLossNo}。");
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






    }
}
