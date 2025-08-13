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
using Krypton.Toolkit;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.UI.PSI.SAL;
using EnumsNET;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.PSI.PUR;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RUINORERP.UI.MRP.PQC
{
    [MenuAttrAssemblyInfo("返工入库单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.生产品控, BizType.返工入库单)]
    public partial class UCMRPReworkEntry : BaseBillEditGeneric<tb_MRP_ReworkEntry, tb_MRP_ReworkEntryDetail>
    {
        public UCMRPReworkEntry()
        {
            InitializeComponent();
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_MRP_ReworkEntry);
        }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

        }
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MRP_ReworkEntry).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public override void BindData(tb_MRP_ReworkEntry entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.ReworkEntryID > 0)
            {
                entity.PrimaryKeyID = entity.ReworkEntryID;
                entity.ActionStatus = ActionStatus.加载;
                //entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.EntryDate = System.DateTime.Now;
                if (string.IsNullOrEmpty(entity.ReworkEntryNo))
                {
                    entity.ReworkEntryNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.返工入库单);
                }
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_MRP_ReworkEntryDetails != null && entity.tb_MRP_ReworkEntryDetails.Count > 0)
                {
                    entity.tb_MRP_ReworkEntryDetails.ForEach(c => c.ReworkEntryCID = 0);
                    entity.tb_MRP_ReworkEntryDetails.ForEach(c => c.ReworkEntryCID = 0);
                }
            }


            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.ReworkEntryNo, txtReworkEntryNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.TotalReworkFee.ToString(), txtTotalReworkFee, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_MRP_ReworkEntry>(entity, t => t.EntryDate, dtpEntryDate, false);
            DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkEntry>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
            DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkEntry>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkEntry>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);

            DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkEntry>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text, false);

            //DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);

            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == false);

            //返厂 只包含 加工厂和供应商非客户
            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer ==false)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);

            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);


            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntry>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);


            DataBindingHelper.BindData4ControlByEnum<tb_MRP_ReworkEntry>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_MRP_ReworkEntry>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_MRP_ReworkEntryDetails != null && entity.tb_MRP_ReworkEntryDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_MRP_ReworkEntryDetail>(grid1, sgd, entity.tb_MRP_ReworkEntryDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_MRP_ReworkEntryDetail>(grid1, sgd, new List<tb_MRP_ReworkEntryDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
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

                //如果是制令单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_MRP_ReworkEntry>(c => c.ReworkReturnID))
                    {
                        if (entity.ReworkReturnID > 0)
                        {
                          await  LoadRefBillData(entity.ReworkReturnID);
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_MRP_ReworkEntry>(c => c.DepartmentID))
                    {
                        if (cmbDepartmentID.SelectedIndex == 0)
                        {
                            entity.DepartmentID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_MRP_ReworkEntry>(c => c.CustomerVendor_ID))
                    {
                        if (cmbCustomerVendor_ID.SelectedIndex == 0)
                        {
                            entity.CustomerVendor_ID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_MRP_ReworkEntry>(c => c.IsOutSourced))
                    {
                        cmbCustomerVendor_ID.Visible = entity.IsOutSourced;
                        if (entity.IsOutSourced)
                        {
                            entity.DepartmentID = null;
                        }
                        else
                        {
                            cmbCustomerVendor_ID.Visible = false;
                        }
                    }
                    ToolBarEnabledControl(entity);

                }
                else
                {
                   // MainForm.Instance.PrintInfoLog(entity.ActionStatus.GetName());
                }

            };

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_MRP_ReworkEntryValidator>(), kryptonSplitContainer1.Panel1.Controls);
                //  base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, v => v.ReworkReturnNo, txtReworkReturnID, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaMO = Expressionable.Create<tb_MRP_ReworkReturn>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                             .And(t => t.isdeleted == false)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessorMO = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MRP_ReworkReturn).Name + "Processor");
            QueryFilter queryFilterMO = baseProcessorMO.GetQueryFilter();
            queryFilterMO.FilterLimitExpressions.Add(lambdaMO);
            ControlBindingHelper.ConfigureControlFilter<tb_MRP_ReworkEntry, tb_MRP_ReworkReturn>(entity, txtReworkReturnID, t => t.ReworkReturnNo,
             f => f.ReworkReturnNo, queryFilterMO, a => a.ReworkReturnID, b => b.ReworkReturnID, null, false);
            base.BindData(entity);
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


            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_MRP_ReworkEntryDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_MRP_ReworkEntryDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_MRP_ReworkEntryDetail>(c => c.ReworkEntryCID);
            listCols.SetCol_NeverVisible<tb_MRP_ReworkEntryDetail>(c => c.ReworkEntryID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
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


            listCols.SetCol_Format<tb_MRP_ReworkEntryDetail>(c => c.ReworkFee, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_MRP_ReworkEntryDetail>(c => c.UnitCost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_MRP_ReworkEntryDetail>(c => c.SubtotalCostAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_MRP_ReworkEntryDetail>(c => c.SubtotalReworkFee, CustomFormatType.CurrencyFormat);

            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_MRP_ReworkEntryDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_MRP_ReworkEntryDetail>(item);
                }
            }

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;

            listCols.SetCol_Summary<tb_MRP_ReworkEntryDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_MRP_ReworkEntryDetail>(c => c.SubtotalCostAmount);
            listCols.SetCol_Summary<tb_MRP_ReworkEntryDetail>(c => c.SubtotalReworkFee);
            listCols.SetCol_Formula<tb_MRP_ReworkEntryDetail>((a, b) => a.UnitCost * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_MRP_ReworkEntryDetail>((a, b) => a.ReworkFee * b.Quantity, c => c.SubtotalReworkFee);

            listCols.SetCol_FormulaReverse<tb_MRP_ReworkEntryDetail>(d => d.UnitCost == 0 && d.Quantity != 0 && d.SubtotalCostAmount != 0, (a, b) => a.SubtotalCostAmount / b.Quantity, c => c.UnitCost);
            listCols.SetCol_FormulaReverse<tb_MRP_ReworkEntryDetail>(d => d.ReworkFee == 0 && d.Quantity != 0 && d.SubtotalReworkFee != 0, (a, b) => a.SubtotalReworkFee / b.Quantity, c => c.ReworkFee);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_MRP_ReworkEntryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_MRP_ReworkEntryDetail>(sgd, f => f.prop, t => t.property);

            //应该只提供一个结构
            List<tb_MRP_ReworkEntryDetail> lines = new List<tb_MRP_ReworkEntryDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_MRP_ReworkEntryDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_MRP_ReworkEntryDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo,this);
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
                List<tb_MRP_ReworkEntryDetail> details = sgd.BindingSourceLines.DataSource as List<tb_MRP_ReworkEntryDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => c.SubtotalCostAmount);
                EditEntity.TotalReworkFee = details.Sum(c => c.SubtotalReworkFee);

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_MRP_ReworkEntryDetail> details = new List<tb_MRP_ReworkEntryDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_MRP_ReworkEntryDetail> detailentity = bindingSourceSub.DataSource as List<tb_MRP_ReworkEntryDetail>;
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

                EditEntity.tb_MRP_ReworkEntryDetails = details;


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_MRP_ReworkEntryDetail>(details))
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


                ReturnMainSubResults<tb_MRP_ReworkEntry> SaveResult = new ReturnMainSubResults<tb_MRP_ReworkEntry>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ReworkEntryNo}。");
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

        /// <summary>
        /// 将缴库单转换为返工退货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task LoadRefBillData(long? ReturnID)
        {

            //要加一个判断 值是否有变化
            //新增时才可以

            ButtonSpecAny bsa = (txtReworkReturnID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var ReworkReturn = bsa.Tag as tb_MRP_ReworkReturn;
            if (ReworkReturn == null)
            {
                ReworkReturn = await MainForm.Instance.AppContext.Db.Queryable<tb_MRP_ReworkReturn>()
                    .Where(c => c.ReworkReturnID == ReturnID.Value)
                .Includes(a => a.tb_MRP_ReworkReturnDetails, b => b.tb_proddetail, c => c.tb_prod)
                .SingleAsync();
            }
            if (ReworkReturn != null)
            {
                
                tb_MRP_ReworkEntry entity = MainForm.Instance.mapper.Map<tb_MRP_ReworkEntry>(ReworkReturn);
                List<tb_MRP_ReworkEntryDetail> details = MainForm.Instance.mapper.Map<List<tb_MRP_ReworkEntryDetail>>(ReworkReturn.tb_MRP_ReworkReturnDetails);
                List<tb_MRP_ReworkEntryDetail> NewDetails = new List<tb_MRP_ReworkEntryDetail>();

                List<string> tipsMsg = new List<string>();
                for (int i = 0; i < details.Count; i++)
                {
                    var ReturnDetail = ReworkReturn.tb_MRP_ReworkReturnDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.Location_ID == details[i].Location_ID);
                    if (ReturnDetail == null)
                    {
                        continue;
                    }
                    if (ReturnDetail.Quantity > ReturnDetail.DeliveredQuantity)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"当前行的SKU:{details[i].tb_proddetail.SKU}可通过返工入库数量为0，当前行数据将不会加载到明细！");
                    }
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"返工退库单:{ReworkReturn.ReworkReturnNo}已全部入库，请检查是否正在重复操作！");
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
                entity.tb_MRP_ReworkEntryDetails = NewDetails;
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;
                entity.EntryDate = System.DateTime.Now;
                if (ReworkReturn.CustomerVendor_ID.HasValue && ReworkReturn.CustomerVendor_ID.Value > 0)
                {
                    entity.CustomerVendor_ID = ReworkReturn.CustomerVendor_ID.Value;
                }
                if (ReworkReturn.IsOutSourced && ReworkReturn.CustomerVendor_ID.HasValue)
                {
                    entity.CustomerVendor_ID = ReworkReturn.CustomerVendor_ID.Value;
                }
                if (ReworkReturn.DepartmentID.HasValue)
                {
                    entity.DepartmentID = ReworkReturn.DepartmentID.Value;
                }

                entity.ReworkReturnID = ReworkReturn.ReworkReturnID;
                entity.ReworkReturnNo = ReworkReturn.ReworkReturnNo;
                BusinessHelper.Instance.InitEntity(entity);
                BindData(entity as tb_MRP_ReworkEntry);
            }

        }
    }
}
