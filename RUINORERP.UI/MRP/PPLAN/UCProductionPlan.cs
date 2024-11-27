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
using RUINOR.Core;
using SqlSugar;
using System.Linq.Expressions;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using Krypton.Toolkit;
using RUINORERP.UI.PSI.PUR;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.CommService;

namespace RUINORERP.UI.MRP.MP
{
    [MenuAttrAssemblyInfo("生产计划单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制造规划, BizType.生产计划单)]
    public partial class UCProductionPlan : BaseBillEditGeneric<tb_ProductionPlan, tb_ProductionPlanDetail>
    {
        public UCProductionPlan()
        {
            InitializeComponent();
            InitDataToCmbByEnumDynamicGeneratedDataSource<tb_ProductionPlan>(typeof(Priority), e => e.Priority, cmbOrderPriority, false);

        }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_ProductionPlan);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProductionPlan).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(tb_ProductionPlan entityPara, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (actionStatus==ActionStatus.删除)
            {
                return;
            }
            tb_ProductionPlan entity = entityPara;
            if (entity == null)
            {
                return;
            }

            if (entity != null)
            {
                if (entity.PPID > 0)
                {
                    entity.PrimaryKeyID = entity.PPID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (entity.PPNo.IsNullOrEmpty())
                    {
                        entity.PPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.生产计划单);
                    }
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    entity.PlanDate = System.DateTime.Now;
                    entity.RequirementDate = System.DateTime.Now.AddDays(10);//这是不是一个平均时间。将来可以根据数据优化？
                    if (entity.tb_ProductionPlanDetails != null && entity.tb_ProductionPlanDetails.Count > 0)
                    {
                        entity.tb_ProductionPlanDetails.ForEach(c => c.PPID = 0);
                        entity.tb_ProductionPlanDetails.ForEach(c => c.PPCID = 0);
                    }
                }
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            DataBindingHelper.BindData4CheckBox<tb_ProductionPlan>(entity, t => t.Analyzed, chkAnalyzed, false);
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.PPNo, txtPPNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbprojectGroup);
            DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.PlanDate, dtpPlanDate, false);
            DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.RequirementDate, dtpRequirementDate, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartment);
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_ProductionPlan>(entity, k => k.Priority, typeof(Priority), cmbOrderPriority, true);
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.TotalQuantity, txtTotalQuantity, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.TotalCompletedQuantity, txt总完成数, BindDataType4TextBox.Qty, false);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_ProductionPlan>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProductionPlan>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_ProductionPlanDetails != null && entity.tb_ProductionPlanDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProductionPlanDetail>(grid1, sgd, entity.tb_ProductionPlanDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProductionPlanDetail>(grid1, sgd, new List<tb_ProductionPlanDetail>(), c => c.ProdDetailID);
            }
            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }

                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.SOrder_ID.HasValue && entity.SOrder_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ProductionPlan>(c => c.SOrder_ID))
                {
                    LoadChildItems(entity.SOrder_ID.Value);
                }


                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_ProductionPlan>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }
                //如果客户有变化，带出对应有业务员

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



            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, v => v.SaleOrderNo, txtSaleOrder, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ProductionPlan>(entity, v => v.SOrder_ID, txtSaleOrder, true);

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_SaleOrder>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);

            DataBindingHelper.InitFilterForControlByExp<tb_SaleOrder>(entity, txtSaleOrder, c => c.SOrderNo, queryFilter);



            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_ProductionPlanValidator(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail>();
        SourceGridHelper sgh = new SourceGridHelper();

        List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            //InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_ProductionPlanDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;



            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_ProductionPlanDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<tb_ProductionPlanDetail>(c => c.ProdDetailID);
            //listCols.SetCol_NeverVisible<tb_ProductionPlanDetail>(c => c.BOM_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            ControlChildColumnsInvisible(listCols);
            //listCols.SetCol_DefaultValue<tb_ProductionPlanDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            listCols.SetCol_ReadOnly<tb_ProductionPlanDetail>(c => c.CompletedQuantity);
            listCols.SetCol_ReadOnly<tb_ProductionPlanDetail>(c => c.AnalyzedQuantity);
            listCols.SetCol_ReadOnly<tb_ProductionPlanDetail>(c => c.IsAnalyzed);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;


            listCols.SetCol_Summary<tb_ProductionPlanDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_ProductionPlanDetail>(c => c.CompletedQuantity);

            //bomid的下拉值。受当前行选择时会改变下拉范围,由产品ID决定BOM显示
            sgh.SetCol_LimitedConditionsForSelectionRange<tb_ProductionPlanDetail>(sgd, t => t.ProdDetailID, f => f.BOM_ID);


            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_ProductionPlanDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProductionPlanDetail>(sgd, f => f.Location_ID, t => t.Location_ID);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProductionPlanDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProductionPlanDetail>(sgd, f => f.Specifications, t => t.Specifications);
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionPlanDetail>(sgd, f => f.BOM_ID, t => t.BOM_ID);


            //应该只提供一个结构
            List<tb_ProductionPlanDetail> lines = new List<tb_ProductionPlanDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            sgd.SetDependencyObject<ProductSharePart, tb_ProductionPlanDetail>(MainForm.Instance.list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProductionPlanDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            base.ControlMasterColumnsInvisible();
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
                List<tb_ProductionPlanDetail> details = new List<tb_ProductionPlanDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_ProductionPlanDetail Detail = mapper.Map<tb_ProductionPlanDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_ProductionPlanDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_ProductionPlanDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProductionPlanDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQuantity = details.Sum(c => c.Quantity);
                EditEntity.TotalCompletedQuantity = details.Sum(c => c.CompletedQuantity);

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_ProductionPlanDetail> details = new List<tb_ProductionPlanDetail>();
        /// <summary>
        /// 查询结果 选中行的变化事件
        /// </summary>
        /// <param name="entity"></param>

        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQuantity);

            bindingSourceSub.EndEdit();

            List<tb_ProductionPlanDetail> oldOjb = new List<tb_ProductionPlanDetail>(details.ToArray());

            List<tb_ProductionPlanDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProductionPlanDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                if (NeedValidated && (EditEntity.TotalQuantity == 0 || detailentity.Sum(c => c.Quantity) == 0))
                {
                    MessageBox.Show("单据及明细总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (NeedValidated && EditEntity.TotalQuantity != detailentity.Sum(c => c.Quantity))
                {
                    MessageBox.Show($"单据总数量{EditEntity.TotalQuantity}和明细总数量{detailentity.Sum(c => c.Quantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                EditEntity.tb_ProductionPlanDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ProductionPlanDetail>(details))
                {
                    return false;
                }


                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }

                if (NeedValidated)
                {
                    if (EditEntity.tb_ProductionDemands != null && EditEntity.tb_ProductionDemands.Count > 0)
                    {
                        MessageBox.Show("当前计划单已经存在需求分析数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                ReturnMainSubResults<tb_ProductionPlan> SaveResult = new ReturnMainSubResults<tb_ProductionPlan>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PPNo}。");
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

            if (EditEntity.tb_ProductionPlanDetails == null || EditEntity.tb_ProductionPlanDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整产品数量和金额数据。", UILogType.警告);
                return null;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_ProductionPlan oldobj = CloneHelper.DeepCloneObject<tb_ProductionPlan>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_ProductionPlan>(EditEntity, oldobj);
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
            tb_ProductionPlanController<tb_ProductionPlan> ctr = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
            ReturnResults<tb_ProductionPlan> rmrs = await ctr.ApprovalAsync(EditEntity);
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
                ToolBarEnabledControl(EditEntity);
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
            }
            return ae;
        }
        */
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
            String reportFile = typeof(tb_ProductionPlan).Name + ".frx";
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





        /*
        protected override void PrintDesigned()
        {
            string ReprotfileName = typeof(tb_ProductionPlan).Name + ".frx";
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = ReprotfileName;
            // frm.ShowDialog();
            //调用内置方法 给数据源 新编辑，后面的话，直接load 可以不用给数据源的格式
            //string ReprotfileName = "SOB.frx";
            //List<tb_ProductionPlan> main = new List<tb_ProductionPlan>();
            //_EditEntity.tb_sales_order_details = details;
            //main.Add(_EditEntity);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            List<tb_ProductionPlan> rptSources = new List<tb_ProductionPlan>();
            tb_ProductionPlan saleOrder = EditEntity as tb_ProductionPlan;
            saleOrder.tb_ProductionPlanDetails = details;
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
            //List<tb_ProductionPlan> main = new List<tb_ProductionPlan>();
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


            if (EditEntity.tb_ProductionPlanDetails == null || EditEntity.tb_ProductionPlanDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return ae;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_ProductionPlan oldobj = CloneHelper.DeepCloneObject<tb_ProductionPlan>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_ProductionPlan>(EditEntity, oldobj);
            };

            tb_ProductionPlanController<tb_ProductionPlan> ctr = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
            ReturnResults<bool> Succeeded = await ctr.AntiApprovalAsync(EditEntity);
            if (Succeeded.Succeeded)
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
                ToolBarEnabledControl(EditEntity);
                toolStripbtnReview.Enabled = true;

            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.PPNo}反审失败{Succeeded.ErrorMsg},请联系管理员！", Color.Red);
            }
            return ae;
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
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_ProductionPlan>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<tb_ProductionPlan>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_ProductionPlan> EditEntitys = new List<tb_ProductionPlan>();
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return false;
                }
                EditEntitys.Add(EditEntity);
                //已经审核的并且通过的情况才能结案
                List<tb_ProductionPlan> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }
                tb_ProductionPlanController<tb_ProductionPlan> ctr = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
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
                    base.Refreshs();
                    return true;
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.PPNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                    return false;
                }

            }
            return true;
        }




        //注意这里从销售订单中引入明细。明细中会存在相同产品分开录入的情况。
        //比方备品。这里在计划明细中合并处理。目前认为生产采购一次数量大才好。当然应该也可以分开。可能逻辑还有一些小问题
        private async void LoadChildItems(long? saleorderid)
        {
            //ButtonSpecAny bsa = (txtSaleOrder as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            //if (bsa == null)
            //{
            //    return;
            //}
            //var saleorder = bsa.Tag as tb_SaleOrder;
            //因为要查BOM情况。不会传过来。
            var saleorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>().Where(c => c.SOrder_ID == saleorderid)
          .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
          .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_bom_s)
          .SingleAsync();
            //新增时才可以转单
            if (saleorder != null)
            {
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                tb_ProductionPlan entity = mapper.Map<tb_ProductionPlan>(saleorder);
                List<tb_ProductionPlanDetail> details = mapper.Map<List<tb_ProductionPlanDetail>>(saleorder.tb_SaleOrderDetails);
                entity.PlanDate = System.DateTime.Now;
                List<tb_ProductionPlanDetail> NewDetails = new List<tb_ProductionPlanDetail>();
                List<string> tipsMsg = new List<string>();

                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("销售订单明细中，有相同的产品,系统已自动合并!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_SaleOrderDetail _SaleOrderDetail = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    if (_SaleOrderDetail != null && _SaleOrderDetail.tb_proddetail.tb_bom_s != null)
                    {
                        details[i].BOM_ID = _SaleOrderDetail.tb_proddetail.tb_bom_s.BOM_ID;
                    }
                    else
                    {
                        tipsMsg.Add($"{_SaleOrderDetail.tb_proddetail.SKU}-{_SaleOrderDetail.tb_proddetail.tb_prod.CNName}：");
                        //后面优化？检测一下库存
                        tipsMsg.Add($"没有BOM配方。无法正常进行需求分析,请删除！");
                        tipsMsg.Add($"如是半成品配件，需要外采。请另下采购单。");
                    }

                    if (details[i].Quantity > 0)
                    {
                        //合并存在的。
                        if (aa.Count > 1)
                        {
                            var newdetail = NewDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                            if (newdetail!= null)
                            {
                                newdetail.Quantity += details[i].Quantity;
                                continue;
                            }
                        }

                        NewDetails.Add(details[i]);
                    }
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
                entity.tb_ProductionPlanDetails = NewDetails;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;
                entity.PrintStatus = 0;

                if (saleorder.DeliveryDate.HasValue)
                {
                    entity.RequirementDate = saleorder.DeliveryDate.Value;
                }
                else
                {
                    entity.RequirementDate = System.DateTime.Now.AddDays(10);//这是不是一个平均时间。将来可以根据数据优化？   
                    entity.PlanDate = System.DateTime.Now;
                }
                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.SOrder_ID = saleorder.SOrder_ID;
                    entity.SaleOrderNo = saleorder.SOrderNo;
                    //为了在单号上区别是来自于销售的，还是自己计划的，将PP+so
                    EditEntity.PPNo = "PP" + entity.SaleOrderNo;
                }
                BusinessHelper.Instance.InitEntity(entity);
                //编号已经生成，在新点开一个页面时，自动生成。
                if (EditEntity.PPNo.IsNotEmptyOrNull())
                {
                    entity.PPNo = EditEntity.PPNo;

                }

                BindData(entity);
            }
        }

    }
}
