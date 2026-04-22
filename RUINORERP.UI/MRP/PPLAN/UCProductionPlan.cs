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
using static StackExchange.Redis.Role;
using RUINORERP.Global.Model;
using RUINORERP.Business.BizMapperService;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.MRP.MP
{
    [MenuAttrAssemblyInfo("生产计划单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制造规划, BizType.生产计划单)]
    public partial class UCProductionPlan : BaseBillEditGeneric<tb_ProductionPlan, tb_ProductionPlanDetail>
    {
        public UCProductionPlan()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_ProductionPlan>(typeof(Priority), e => e.Priority, cmbOrderPriority, false);

        }
        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_ProductionPlan ProductionPlan)
            {
                if (ProductionPlan.SOrder_ID.HasValue && ProductionPlan.SOrder_ID.Value > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.销售订单;
                    rqp.billId = ProductionPlan.SOrder_ID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{ProductionPlan.SaleOrderNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(ProductionPlan.SOrder_ID.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }

                if (ProductionPlan.tb_ProductionDemands != null && ProductionPlan.tb_ProductionDemands.Count > 0)
                {
                    foreach (var item in ProductionPlan.tb_ProductionDemands)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.需求分析;
                        rqp.billId = item.PDID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.PDNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.PDID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }

            }
            await base.LoadRelatedDataToDropDownItemsAsync();
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
        public override async void BindData(tb_ProductionPlan entityPara, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (actionStatus == ActionStatus.删除)
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
                    entity.Priority = (int)Priority.正常;
                    if (entity.PPNo.IsNullOrEmpty())
                    {
                        var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                        entity.PPNo = await bizCodeService.GenerateBizBillNoAsync(BizType.生产计划单);
                    }
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    entity.PlanDate = System.DateTime.Now;
                    entity.RequirementDate = System.DateTime.Now.AddDays(10);//这是不是一个平均时间。将来可以根据数据优化？
                    listCols.SetCol_DefaultValue<tb_ProductionPlanDetail>(c => c.RequirementDate, EditEntity.RequirementDate.ToShortDateString());
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
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_ProductionPlan>(c => c.SOrder_ID) && entity.SOrder_ID.HasValue)
                    {
                        LoadChildItems(entity.SOrder_ID.Value);
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_ProductionPlan>(c => c.RequirementDate))
                    {
                        //明细优先来自于主表，可以手动修改。
                        listCols.SetCol_DefaultValue<tb_ProductionPlanDetail>(c => c.RequirementDate, EditEntity.RequirementDate.ToShortDateString());
                        for (int i = 0; i < EditEntity.tb_ProductionPlanDetails.Count; i++)
                        {
                            EditEntity.tb_ProductionPlanDetails[i].RequirementDate = EditEntity.RequirementDate;
                        }

                        //同步到明细UI表格中？
                        sgh.SynchronizeUpdateCellValue<tb_ProductionPlanDetail>(sgd, c => c.RequirementDate, EditEntity.tb_ProductionPlanDetails);
                    }
                }


                //如果客户有变化，带出对应有业务员

            };



            //先绑定这个。ControlBindingHelper.ConfigureControlFilter 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, v => v.SaleOrderNo, txtSaleOrder, BindDataType4TextBox.Text, true);
            
            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaSaleOrder = Expressionable.Create<tb_SaleOrder>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                            .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.审核通过)
                            .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
                            .And(t => t.isdeleted == false)
                            .ToExpression();
            
            BaseProcessor baseProSaleOrder = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryFilter queryFilterSaleOrder = baseProSaleOrder.GetQueryFilter();
            
            queryFilterSaleOrder.FilterLimitExpressions.Add(lambdaSaleOrder);
            
            // 使用ControlBindingHelper.ConfigureControlFilter配置控件过滤，避免光标锁定问题
            ControlBindingHelper.ConfigureControlFilter<tb_ProductionPlan, tb_SaleOrder>(entity, txtSaleOrder, t => t.SaleOrderNo,
                f => f.SOrderNo, queryFilterSaleOrder, a => a.SOrder_ID, b => b.SOrder_ID, null, false);

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
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProductionPlanValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail>();
        SourceGridHelper sgh = new SourceGridHelper();

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            //InitDataTocmbbox();


            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_ProductionPlanDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;



            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_ProductionPlanDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice); listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols.SetCol_NeverVisible<tb_ProductionPlanDetail>(c => c.ProdDetailID);
            //listCols.SetCol_NeverVisible<tb_ProductionPlanDetail>(c => c.BOM_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
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

            //if (CurMenuInfo.tb_P4Fields != null)
            //{
            //    List<tb_P4Field> P4Fields =
            //        CurMenuInfo.tb_P4Fields
            //        .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
            //        && p.tb_fieldinfo.IsChild && !p.IsVisble).ToList();
            //    foreach (var item in P4Fields)
            //    {
            //        //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
            //        listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_ProductionPlanDetail));
            //    }

            //}
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

            sgd.SetDependencyObject<ProductSharePart, tb_ProductionPlanDetail>(MainForm.Instance.View_ProdDetailList);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProductionPlanDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
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

                foreach (var item in RowDetails)
                {
                    tb_ProductionPlanDetail Detail = MainForm.Instance.mapper.Map<tb_ProductionPlanDetail>(item);
                    Detail.Quantity = 0;
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
                if (NeedValidated && aa.Count > 0)
                {
                    string message = GetDuplicateProductMessage(aa[0]);
                    System.Windows.Forms.MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        if (SaveResult.ErrorMsg.Contains("插入重复键"))
                        {
                            MessageBox.Show("保存失败，计划单号不能重复，如果是销售订单拆分而成，建议在单号后手动添加序号。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                        }

                    }
                }
                return SaveResult.Succeeded;
            }
            return false;
        }

 

        /// <summary>
        /// 生产计划结案
        /// 添加确认机制：如果存在已发料但未缴库的情况，提示用户确认
        /// </summary>
        /// <returns></returns>
        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }

            // 预检查：检查是否存在已发料但未缴库的情况
            StringBuilder warningMsg = new StringBuilder();
            int warningCount = 0;
            
            // 加载计划单的完整数据
            var planFullData = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                .Includes(p => p.tb_ProductionDemands, d => d.tb_ManufacturingOrders)
                .Where(p => p.PPID == EditEntity.PPID)
                .FirstAsync();

            if (planFullData?.tb_ProductionDemands != null)
            {
                foreach (var demand in planFullData.tb_ProductionDemands)
                {
                    if (demand.tb_ManufacturingOrders != null)
                    {
                        foreach (var mo in demand.tb_ManufacturingOrders)
                        {
                            if (mo.DataStatus == (int)DataStatus.确认 && mo.ApprovalResults.HasValue && mo.ApprovalResults.Value)
                            {
                                // 加载制令单的完整数据
                                var moFullData = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                                    .Includes(m => m.tb_MaterialRequisitions, mr => mr.tb_MaterialRequisitionDetails)
                                    .Includes(m => m.tb_FinishedGoodsInvs, fg => fg.tb_FinishedGoodsInvDetails)
                                    .Where(m => m.MOID == mo.MOID)
                                    .FirstAsync();

                                if (moFullData?.tb_MaterialRequisitions != null)
                                {
                                    var approvedMaterialRequisitions = moFullData.tb_MaterialRequisitions
                                        .Where(mr => mr.DataStatus == (int)DataStatus.确认
                                            && mr.ApprovalStatus.HasValue
                                            && mr.ApprovalStatus.Value == (int)ApprovalStatus.审核通过)
                                        .ToList();

                                    if (approvedMaterialRequisitions.Any())
                                    {
                                        decimal totalMaterialSent = approvedMaterialRequisitions
                                            .SelectMany(mr => mr.tb_MaterialRequisitionDetails)
                                            .Sum(mrd => mrd.ActualSentQty);

                                        decimal totalFinishedGoods = 0;
                                        if (moFullData.tb_FinishedGoodsInvs != null)
                                        {
                                            totalFinishedGoods = moFullData.tb_FinishedGoodsInvs
                                                .Where(fg => fg.DataStatus == (int)DataStatus.确认
                                                    && fg.ApprovalStatus.HasValue
                                                    && fg.ApprovalStatus.Value == (int)ApprovalStatus.审核通过)
                                                .SelectMany(fg => fg.tb_FinishedGoodsInvDetails)
                                                .Sum(fgd => fgd.Qty);
                                        }

                                        if (totalMaterialSent > 0 && totalFinishedGoods == 0)
                                        {
                                            warningCount++;
                                            warningMsg.AppendLine($"  - 制令单[{mo.MONO}]:已发料{totalMaterialSent}但未缴库");
                                        }
                                        else if (totalMaterialSent > totalFinishedGoods)
                                        {
                                            warningCount++;
                                            warningMsg.AppendLine($"  - 制令单[{mo.MONO}]:已发料{totalMaterialSent}大于已缴库{totalFinishedGoods}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 如果存在警告，显示确认对话框
            if (warningCount > 0)
            {
                DialogResult confirmResult = MessageBox.Show(
                    $"检测到以下制令单存在已发料但未缴库的情况：\n\n{warningMsg}\n是否继续强制结案？\n\n提示：强制结案后，这些制令单将被标记为完结，未缴库的物料将无法自动关联。",
                    "结案确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmResult == DialogResult.No)
                {
                    MainForm.Instance.PrintInfoLog("用户取消了结案操作", Color.Orange);
                    return false;
                }
            }
        
            // 使用增强后的通用意见窗体
            CommonUI.frmGenericOpinion<tb_ProductionPlan> frm = new CommonUI.frmGenericOpinion<tb_ProductionPlan>();
            frm.FormTitle = "生产计划结案确认";
            frm.OpinionLabelText = "结案意见:";
            frm.BindData(EditEntity, 
                e => e.PPNo, 
                e => e.CloseCaseOpinions);
        
            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<tb_ProductionPlan> EditEntitys = new List<tb_ProductionPlan>();
                EditEntity.CloseCaseOpinions = frm.OpinionText;
                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return false;
                }
                EditEntitys.Add(EditEntity);
                //已经审核的并且通过的情况才能结案
                List<tb_ProductionPlan> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.审核通过 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
                if (needCloseCases.Count == 0)
                {
                    MessageBox.Show($"要结案的数据行数为:{needCloseCases.Count}:请检查数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // ✅ 关键修复:业务校验失败时返回false,让基类恢复按钮状态
                    return false;
                }
                tb_ProductionPlanController<tb_ProductionPlan> ctr = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
                ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
                if (rs.Succeeded)
                {
                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{
        
                    //}
                    //这里审核完了的话,如果这个单存在于工作流的集合队列中,则向服务器说明审核完成。
                    //这里推送到审核,启动工作流  队列应该有一个策略 比方优先级,桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    base.Refreshs();
                    // ✅ 结案成功返回true
                    return true;
                }
                else
                {
                    // ✅ 关键修复:结案失败时弹出明确的错误提示,并返回false让基类恢复按钮状态
                    string errorMsg = string.IsNullOrEmpty(rs.ErrorMsg) ? "未知错误" : rs.ErrorMsg;
                    KryptonMessageBox.Show($"结案操作失败!\n\n失败原因:{errorMsg}\n\n如无法解决,请联系管理员!", 
                        "结案失败", 
                        Krypton.Toolkit.KryptonMessageBoxButtons.OK, 
                        Krypton.Toolkit.KryptonMessageBoxIcon.Error);
                            
                    MainForm.Instance.PrintInfoLog($"{EditEntity.PPNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决,请联系管理员!", Color.Red);
                    // ✅ 返回false,让基类的错误处理逻辑恢复按钮为可用状态
                    return false;
                }
            }
            else
            {
                // ✅ 用户取消操作,返回false,基类会恢复按钮状态
                return false;
            }
        }




        //注意这里从销售订单中引入明细。明细中会存在相同产品分开录入的情况。
        //比方备品。这里在计划明细中合并处理。目前认为生产采购一次数量大才好。当然应该也可以分开。可能逻辑还有一些小问题
        private async Task LoadChildItems(long? saleorderid)
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

                tb_ProductionPlan entity = MainForm.Instance.mapper.Map<tb_ProductionPlan>(saleorder);
                List<tb_ProductionPlanDetail> details = MainForm.Instance.mapper.Map<List<tb_ProductionPlanDetail>>(saleorder.tb_SaleOrderDetails);
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
                    tb_SaleOrderDetail _SaleOrderDetail = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.Location_ID == details[i].Location_ID);
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
                            var newdetail = NewDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.Location_ID == details[i].Location_ID);
                            if (newdetail != null)
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
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;
                entity.PrintStatus = 0;


                entity.RequirementDate = System.DateTime.Now.AddDays(10);//这是不是一个平均时间。将来可以根据数据优化？   
                entity.PlanDate = System.DateTime.Now;

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
                entity.HasChanged = true;
            }
        }

    }
}
