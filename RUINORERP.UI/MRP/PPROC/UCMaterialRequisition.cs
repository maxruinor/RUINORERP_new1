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
using System.Web.WebSockets;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.CommService;

namespace RUINORERP.UI.MRP.MP
{
    [MenuAttrAssemblyInfo("生产领料单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.生产领料单)]
    public partial class UCMaterialRequisition : BaseBillEditGeneric<tb_MaterialRequisition, tb_MaterialRequisitionDetail
        >
    {
        public UCMaterialRequisition()
        {
            InitializeComponent();
            // InitDataToCmbByEnumDynamicGeneratedDataSource<tb_MaterialRequisition>(typeof(Priority), e => e.Priority, cmbOrderPriority, false);
        }

     

        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_MaterialRequisition, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MaterialRequisition).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(tb_MaterialRequisition entityPara, ActionStatus actionStatus)
        {
            tb_MaterialRequisition entity = entityPara as tb_MaterialRequisition;
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.MR_ID > 0)
                {
                    entity.PrimaryKeyID = entity.MR_ID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (entity.MaterialRequisitionNO.IsNullOrEmpty())
                    {
                        entity.MaterialRequisitionNO = BizCodeGenerator.Instance.GetBizBillNo(BizType.生产领料单);
                    }
                    entity.DeliveryDate = System.DateTime.Now;
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    if (entity.tb_MaterialRequisitionDetails != null && entity.tb_MaterialRequisitionDetails.Count > 0)
                    {
                        entity.tb_MaterialRequisitionDetails.ForEach(c => c.MR_ID = 0);
                        entity.tb_MaterialRequisitionDetails.ForEach(c => c.Detail_ID = 0);
                    }
                }
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;


            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.MaterialRequisitionNO, txtMaterialRequisitionNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_MaterialRequisition>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.shippingWay, txtshippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TotalPrice.ToString(), txtTotalPrice, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ExpectedQuantity, txtExpectedQuantity, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.ReApply, chkReApply, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
            DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.Outgoing, chkOutgoing, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbprojectGroup);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_MaterialRequisition>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_MaterialRequisition>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_MaterialRequisitionDetails != null && entity.tb_MaterialRequisitionDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_MaterialRequisitionDetail>(grid1, sgd, entity.tb_MaterialRequisitionDetails, c => c.ProdDetailID);

                //明细中修改数据，主表数据更新状态变化导致保存按钮变化
                foreach (var item in entity.tb_MaterialRequisitionDetails)
                {
                    //如果属性变化 则状态为修改
                    item.PropertyChanged += (sender, s2) =>
                    {
                        if ((entity.ActionStatus == ActionStatus.加载 && entity.ApprovalStatus == (int)ApprovalStatus.未审核) && s2.PropertyName == entity.GetPropertyName<tb_MaterialRequisitionDetail>(c => c.ActualSentQty))
                        {
                            
                        }
                    };
                }
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_MaterialRequisitionDetail>(grid1, sgd, new List<tb_MaterialRequisitionDetail>(), c => c.ProdDetailID);
            }
            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null)
                {
                    return;
                }
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    
                }

                //如果是制令单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.MOID > 0 && s2.PropertyName == entity.GetPropertyName<tb_MaterialRequisition>(c => c.MOID))
                {
                    LoadChildItems(entity.MOID);
                }

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_MaterialRequisition>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }
                //预计产量是来自于制令单，如果修改则要同步修改明细的发料数量
                //影响明细的数量
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && s2.PropertyName == entity.GetPropertyName<tb_MaterialRequisition>(c => c.ExpectedQuantity))
                {
                    if (EditEntity.tb_manufacturingorder.tb_bom_s == null && EditEntity.tb_manufacturingorder.BOM_ID > 0)
                    {
                        EditEntity.tb_manufacturingorder.tb_bom_s = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                        .Includes(a => a.tb_BOM_SDetails)
                        .Where(c => c.BOM_ID == EditEntity.tb_manufacturingorder.BOM_ID).Single();
                    }
                    if (EditEntity.tb_manufacturingorder.tb_bom_s != null
                    && EditEntity.tb_manufacturingorder.tb_bom_s.BOM_ID > 0
                    && EditEntity.tb_manufacturingorder.tb_bom_s.tb_BOM_SDetails.Count > 0)
                    {
                        decimal bomOutQty = EditEntity.tb_manufacturingorder.tb_bom_s.OutputQty;
                        for (int i = 0; i < EditEntity.tb_MaterialRequisitionDetails.Count; i++)
                        {
                            tb_BOM_SDetail bOM_SDetail = EditEntity.tb_manufacturingorder.tb_bom_s.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == EditEntity.tb_MaterialRequisitionDetails[i].ProdDetailID);
                            if (bOM_SDetail != null)
                            {
                                EditEntity.tb_MaterialRequisitionDetails[i].ActualSentQty = (bOM_SDetail.UsedQty * (EditEntity.ExpectedQuantity / bomOutQty)).ToInt();
                            }
                        }
                    }
                    //同步到明细UI表格中？
                     sgh.SynchronizeUpdateCellValue<tb_MaterialRequisitionDetail>(sgd, c => c.ActualSentQty, EditEntity.tb_MaterialRequisitionDetails);
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



            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, v => v.MONO, txtManufacturingOrder, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_MaterialRequisition>(entity, v => v.MOID, txtManufacturingOrder, true);

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ManufacturingOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_ManufacturingOrder>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);
            DataBindingHelper.InitFilterForControlByExp<tb_ManufacturingOrder>(entity, txtManufacturingOrder, c => c.MONO, queryFilter);

            //创建表达式 外发工厂
            var lambdaOut = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsOther== true)
                            .ToExpression();

            BaseProcessor baseProcessorOut = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterCOut = baseProcessorOut.GetQueryFilter();
            queryFilterCOut.FilterLimitExpressions.Add(lambdaOut);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor, tb_MaterialRequisition>(entity, k => k.CustomerVendor_ID, v => v.CVName, k => k.CustomerVendor_ID.Value, cmbCustomerVendor_ID, true, queryFilterCOut.GetFilterExpression<tb_CustomerVendor>());
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterCOut);



            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_MaterialRequisitionValidator(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_MaterialRequisitionsDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_MaterialRequisitionsDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
       

                List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            //InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_MaterialRequisitionsDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;



            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_MaterialRequisitionDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<tb_MaterialRequisitionDetail>(c => c.ProdDetailID);
            //listCols.SetCol_NeverVisible<tb_MaterialRequisitionsDetail>(c => c.BOM_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            ControlChildColumnsInvisible(listCols);
            //listCols.SetCol_DefaultValue<tb_MaterialRequisitionsDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_ReadOnly<tb_MaterialRequisitionDetail>(c => c.ShouldSendQty);
            listCols.SetCol_ReadOnly<tb_MaterialRequisitionDetail>(c => c.ReturnQty);
            listCols.SetCol_ReadOnly<tb_MaterialRequisitionDetail>(c => c.Cost);


            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_MaterialRequisitionDetail>(c => c.SubtotalCost);
            listCols.SetCol_Summary<tb_MaterialRequisitionDetail>(c => c.SubtotalPrice);
            listCols.SetCol_Summary<tb_MaterialRequisitionDetail>(c => c.ActualSentQty);
            listCols.SetCol_Summary<tb_MaterialRequisitionDetail>(c => c.ShouldSendQty);
            listCols.SetCol_Summary<tb_MaterialRequisitionDetail>(c => c.ReturnQty);
            listCols.SetCol_Summary<tb_MaterialRequisitionDetail>(c => c.CanQuantity);

            //bomid的下拉值。受当前行选择时会改变下拉范围,由产品ID决定BOM显示
            // sgh.SetCol_LimitedConditionsForSelectionRange<tb_MaterialRequisitionsDetail>(sgd, t => t.ProdDetailID, f => f.BOM_ID);


            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_MaterialRequisitionDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_MaterialRequisitionDetail>(sgd, f => f.Location_ID, t => t.Location_ID);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_MaterialRequisitionDetail>(sgd, f => f.prop, t => t.property);

            //listCols.SetCol_Formula<tb_MaterialRequisitionDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.SubtotalTaxAmount, d => d.ActualSentQty);
            //  listCols.SetCol_Formula<tb_MaterialRequisitionDetail>((a, b) => a.CanQuantity - b.ActualSentQty, c => c.);

            //应该只提供一个结构
            List<tb_MaterialRequisitionDetail> lines = new List<tb_MaterialRequisitionDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

      
            sgd.SetDependencyObject<ProductSharePart, tb_MaterialRequisitionDetail>(MainForm.Instance.list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_MaterialRequisitionDetail));
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
                List<tb_MaterialRequisitionDetail> details = new List<tb_MaterialRequisitionDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_MaterialRequisitionDetail Detail = mapper.Map<tb_MaterialRequisitionDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_MaterialRequisitionDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_MaterialRequisitionDetail> details = sgd.BindingSourceLines.DataSource as List<tb_MaterialRequisitionDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalCost = details.Sum(c => c.SubtotalCost);
                EditEntity.TotalPrice = details.Sum(c => c.SubtotalPrice);
                EditEntity.TotalSendQty = details.Sum(c => c.ActualSentQty);
                EditEntity.TotalReQty = details.Sum(c => c.ReturnQty);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_MaterialRequisitionDetail> details = new List<tb_MaterialRequisitionDetail>();
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
            var eer = errorProviderForAllInput.GetError(txtExpectedQuantity);

            bindingSourceSub.EndEdit();

            List<tb_MaterialRequisitionDetail> oldOjb = new List<tb_MaterialRequisitionDetail>(details.ToArray());

            List<tb_MaterialRequisitionDetail> detailentity = bindingSourceSub.DataSource as List<tb_MaterialRequisitionDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                if (NeedValidated && detailentity.Sum(c => c.ActualSentQty) == 0)
                {
                    MessageBox.Show("明细总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                EditEntity.TotalSendQty = details.Sum(c => c.ActualSentQty);
                EditEntity.TotalReQty = details.Sum(c => c.ReturnQty);

                if (NeedValidated && (EditEntity.TotalSendQty != detailentity.Sum(c => c.ActualSentQty) || EditEntity.TotalReQty != detailentity.Sum(c => c.ReturnQty)))
                {
                    MessageBox.Show($"单据总数量{EditEntity.TotalSendQty}和明细实发总数量{detailentity.Sum(c => c.ActualSentQty)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                for (int i = 0; i < details.Count; i++)
                {
                    if (NeedValidated && details[i].ActualSentQty <= 0)
                    {
                        System.Windows.Forms.MessageBox.Show("明细中实发数量，不能小于或等于零!，请修改或删除该行数据再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

                EditEntity.tb_MaterialRequisitionDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_MaterialRequisitionDetail>(details))
                {
                    return false;
                }


                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                if (NeedValidated)
                {
                    if (EditEntity.tb_MaterialReturns != null && EditEntity.tb_MaterialReturns.Count > 0)
                    {
                        MessageBox.Show("当前领料单已有退回数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                ReturnMainSubResults<tb_MaterialRequisition> SaveResult = new ReturnMainSubResults<tb_MaterialRequisition>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.MaterialRequisitionNO}。");
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

     

        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            List<tb_MaterialRequisition> EditEntitys = new List<tb_MaterialRequisition>();
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_MaterialRequisition> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_MaterialRequisitionController<tb_MaterialRequisition> ctr = Startup.GetFromFac<tb_MaterialRequisitionController<tb_MaterialRequisition>>();
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
                base.Query();
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"{EditEntity.MaterialRequisitionNO}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }



        /// <summary>
        /// 由制令单加载明细
        /// 发料单明细相同也不累计了。直接发
        /// </summary>
        /// <param name="id"></param>
        private async void LoadChildItems(long? id)
        {
            //生成制令单  以目标为基准。
            tb_ManufacturingOrderController<tb_ManufacturingOrder> ctr = Startup.GetFromFac<tb_ManufacturingOrderController<tb_ManufacturingOrder>>();

            //List<tb_ManufacturingOrder> MOList = await ctr.GenerateManufacturingOrderNew(EditEntity);

            //要实现一个 弹出再选择明细的这种功能窗体。如果就一行。就直接返回？

            //ButtonSpecAny bsa = (txtSaleOrder as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            //if (bsa == null)
            //{
            //    return;
            //}
            //var saleorder = bsa.Tag as tb_SaleOrder;
            //因为要查BOM情况。不会传过来。
            var SourceBill = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>().Where(c => c.MOID == id)
          .Includes(a => a.tb_ManufacturingOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
          .Includes(a => a.tb_ManufacturingOrderDetails, b => b.tb_proddetail, c => c.tb_Inventories)
          .SingleAsync();
            //新增时才可以转单
            if (SourceBill != null)
            {
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                tb_MaterialRequisition entity = mapper.Map<tb_MaterialRequisition>(SourceBill);
                List<tb_MaterialRequisitionDetail> details = mapper.Map<List<tb_MaterialRequisitionDetail>>(SourceBill.tb_ManufacturingOrderDetails);
                entity.DeliveryDate = System.DateTime.Now;
                List<tb_MaterialRequisitionDetail> NewDetails = new List<tb_MaterialRequisitionDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_ManufacturingOrderDetail _SourceBillDetail = SourceBill.tb_ManufacturingOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.Location_ID == details[i].Location_ID);
                    details[i].ShouldSendQty = (_SourceBillDetail.ShouldSendQty + _SourceBillDetail.WastageQty - _SourceBillDetail.ActualSentQty).ToInt();
                    var inv = _SourceBillDetail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == details[i].Location_ID);
                    if (inv != null)
                    {
                        if (inv.Quantity >= details[i].ShouldSendQty)
                        {
                            details[i].ActualSentQty = details[i].ShouldSendQty;
                        }
                        else
                        {
                            details[i].ActualSentQty = 0;//仓库不够时 ，暂时默认为0，让手动输入
                        }

                        details[i].CanQuantity = inv.Quantity;
                    }
                    details[i].ManufacturingOrderDetailRowID = _SourceBillDetail.MOCID;
                    if (details[i].ShouldSendQty > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        //如果指定了是补领则显示
                        if (EditEntity.ReApply)
                        {
                            entity.ReApply = true;
                            NewDetails.Add(details[i]);
                        }
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
                entity.tb_manufacturingorder = SourceBill;
                entity.ExpectedQuantity = SourceBill.ManufacturingQty - SourceBill.QuantityDelivered;
                entity.tb_MaterialRequisitionDetails = NewDetails;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;
                entity.PrintStatus = 0;
                entity.Outgoing = SourceBill.IsOutSourced;
                entity.CustomerVendor_ID = SourceBill.CustomerVendor_ID_Out;

                //传入计划单中订单等相关数据，但由于是弱引用。要用程序控制
                //2024-6-26修改为强引用了。是不是可以优化？
                if (SourceBill.PDID.HasValue)
                {
                    BizTypeMapper BizMapper = new BizTypeMapper();

                    tb_ProductionDemand productionDemand = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionDemand>().Where(c => c.PDID == SourceBill.PDID)
                                    .Includes(a => a.tb_productionplan)
                                    .SingleAsync();


                    if (productionDemand.tb_productionplan != null)
                    {
                        entity.ProjectGroup_ID = productionDemand.tb_productionplan.ProjectGroup_ID;
                    }


                }





                if (SourceBill.MOID > 0)
                {
                    entity.MOID = SourceBill.MOID;
                    entity.MONO = SourceBill.MONO;
                }
                BusinessHelper.Instance.InitEntity(entity);
                //编号已经生成，在新点开一个页面时，自动生成。
                if (EditEntity.MaterialRequisitionNO.IsNotEmptyOrNull())
                {
                    entity.MaterialRequisitionNO = EditEntity.MaterialRequisitionNO;
                }


                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(entity, actionStatus);
            }
        }

        private void chkOutgoing_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
