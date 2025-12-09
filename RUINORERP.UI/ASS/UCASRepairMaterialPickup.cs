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
using RUINORERP.UI.Network.Services;
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
using RUINORERP.Model.CommonModel;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.ASS
{
    [MenuAttrAssemblyInfo("维修领料单", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.维修中心, BizType.维修领料单)]
    public partial class UCASRepairMaterialPickup : BaseBillEditGeneric<tb_AS_RepairMaterialPickup, tb_AS_RepairMaterialPickupDetail
        >
    {
        public UCASRepairMaterialPickup()
        {
            InitializeComponent();
        }

        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_AS_RepairMaterialPickup MaterialPickup)
            {
                if (MaterialPickup.RepairOrderID.HasValue && MaterialPickup.RepairOrderID.Value > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.维修工单;
                    rqp.billId = MaterialPickup.RepairOrderID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{MaterialPickup.RepairOrderNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(MaterialPickup.RepairOrderID.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }
            }
         await   base.LoadRelatedDataToDropDownItemsAsync();
        }
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_AS_RepairMaterialPickup, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_RepairMaterialPickup).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(tb_AS_RepairMaterialPickup entityPara, ActionStatus actionStatus)
        {
            tb_AS_RepairMaterialPickup entity = entityPara as tb_AS_RepairMaterialPickup;
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {

                if (entity.RMRID > 0)
                {
                    entity.PrimaryKeyID = entity.RMRID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (entity.MaterialPickupNO.IsNullOrEmpty())
                    {
                        entity.MaterialPickupNO = ClientBizCodeService.GetBizBillNo(BizType.维修领料单);
                    }
                    entity.DeliveryDate = System.DateTime.Now;
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    if (entity.tb_AS_RepairMaterialPickupDetails != null && entity.tb_AS_RepairMaterialPickupDetails.Count > 0)
                    {
                        entity.tb_AS_RepairMaterialPickupDetails.ForEach(c => c.RMRID = 0);
                        entity.tb_AS_RepairMaterialPickupDetails.ForEach(c => c.RMPDetailID = 0);
                    }
                }
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;

            DataBindingHelper.BindData4DataTime<tb_AS_RepairMaterialPickup>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.RepairOrderNo, txtRepairOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalPrice.ToString(), txtTotalPrice, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalReQty, txtTotalReQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalSendQty.ToString(), txtTotalSendQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_AS_RepairMaterialPickup>(entity, t => t.ReApply, chkReApply, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_AS_RepairMaterialPickup>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.MaterialPickupNO, txtMaterialPickupNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_AS_RepairMaterialPickup>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_AS_RepairMaterialPickup>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_AS_RepairMaterialPickupDetails != null && entity.tb_AS_RepairMaterialPickupDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_AS_RepairMaterialPickupDetail>(grid1, sgd, entity.tb_AS_RepairMaterialPickupDetails, c => c.ProdDetailID);

                //明细中修改数据，主表数据更新状态变化导致保存按钮变化
                foreach (var item in entity.tb_AS_RepairMaterialPickupDetails)
                {
                    //如果属性变化 则状态为修改
                    item.PropertyChanged += (sender, s2) =>
                    {
                        if ((entity.ActionStatus == ActionStatus.加载 && entity.ApprovalStatus == (int)ApprovalStatus.未审核) && s2.PropertyName == entity.GetPropertyName<tb_AS_RepairMaterialPickupDetail>(c => c.ActualSentQty))
                        {

                        }
                    };
                }
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_AS_RepairMaterialPickupDetail>(grid1, sgd, new List<tb_AS_RepairMaterialPickupDetail>(), c => c.ProdDetailID);
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
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改))
                {
                    if (entity.RepairOrderID > 0 && s2.PropertyName == entity.GetPropertyName<tb_AS_RepairMaterialPickup>(c => c.RepairOrderID))
                    {
                        LoadChildItems(entity.RepairOrderID);
                    }

                     
                }
 
                //预计产量是来自于制令单，如果修改则要同步修改明细的发料数量
                //影响明细的数量
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改))
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



            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, v => v.RepairOrderNo, txtRepairOrderNo, BindDataType4TextBox.Text, true);


            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_RepairOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            //这里控制查询的最终数据。是不是应该放开，在UI上 条件限制。这样不会因为没有审核。找不到数据。只是选择了。再判断 状态。这要可以提醒上一级 审核。
            //TODO 
            var lambdaOrder = Expressionable.Create<tb_AS_RepairOrder>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
             .And(t => t.RepairStatus == (int)RepairStatus.维修中)//才可以领料
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);

            ControlBindingHelper.ConfigureControlFilter<tb_AS_RepairMaterialPickup, tb_AS_RepairOrder>(entity, txtRepairOrderNo, t => t.RepairOrderNo,
           f => f.RepairOrderNo, queryFilter, a => a.RepairOrderID, b => b.RepairOrderID, null, false);


            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_AS_RepairMaterialPickupValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_AS_RepairMaterialPickupsDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_AS_RepairMaterialPickupsDetail>();
        SourceGridHelper sgh = new SourceGridHelper();


        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
        
            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_AS_RepairMaterialPickupsDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_AS_RepairMaterialPickupDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<tb_AS_RepairMaterialPickupDetail>(c => c.ProdDetailID);
            //listCols.SetCol_NeverVisible<tb_AS_RepairMaterialPickupsDetail>(c => c.BOM_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //listCols.SetCol_DefaultValue<tb_AS_RepairMaterialPickupsDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_ReadOnly<tb_AS_RepairMaterialPickupDetail>(c => c.ShouldSendQty);
            listCols.SetCol_ReadOnly<tb_AS_RepairMaterialPickupDetail>(c => c.ReturnQty);
            listCols.SetCol_ReadOnly<tb_AS_RepairMaterialPickupDetail>(c => c.Cost);

            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_AS_RepairMaterialPickupDetail>(c => c.SubtotalCost);
            listCols.SetCol_Summary<tb_AS_RepairMaterialPickupDetail>(c => c.SubtotalPrice);
            listCols.SetCol_Summary<tb_AS_RepairMaterialPickupDetail>(c => c.ActualSentQty);
            listCols.SetCol_Summary<tb_AS_RepairMaterialPickupDetail>(c => c.ShouldSendQty);
            listCols.SetCol_Summary<tb_AS_RepairMaterialPickupDetail>(c => c.ReturnQty);
            listCols.SetCol_Summary<tb_AS_RepairMaterialPickupDetail>(c => c.CanQuantity);

            //bomid的下拉值。受当前行选择时会改变下拉范围,由产品ID决定BOM显示
            // sgh.SetCol_LimitedConditionsForSelectionRange<tb_AS_RepairMaterialPickupsDetail>(sgd, t => t.ProdDetailID, f => f.BOM_ID);


            //if (CurMenuInfo.tb_P4Fields != null)
            //{
            //    List<tb_P4Field> P4Fields =
            //         CurMenuInfo.tb_P4Fields
            //         .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
            //         && p.tb_fieldinfo.IsChild && !p.IsVisble).ToList();
            //    foreach (var item in P4Fields)
            //    {
            //        //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
            //        listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_AS_RepairMaterialPickupDetail));
            //    }

            //}
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairMaterialPickupDetail>(sgd, f => f.Location_ID, t => t.Location_ID);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairMaterialPickupDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairMaterialPickupDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            //listCols.SetCol_Formula<tb_AS_RepairMaterialPickupDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.SubtotalTaxAmount, d => d.ActualSentQty);
            listCols.SetCol_Formula<tb_AS_RepairMaterialPickupDetail>((a, b) => a.Cost * b.ActualSentQty, c => c.SubtotalCost);


            //应该只提供一个结构
            List<tb_AS_RepairMaterialPickupDetail> lines = new List<tb_AS_RepairMaterialPickupDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;


            sgd.SetDependencyObject<ProductSharePart, tb_AS_RepairMaterialPickupDetail>(MainForm.Instance.View_ProdDetailList);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_AS_RepairMaterialPickupDetail));
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
                List<tb_AS_RepairMaterialPickupDetail> details = new List<tb_AS_RepairMaterialPickupDetail>();

                foreach (var item in RowDetails)
                {
                    tb_AS_RepairMaterialPickupDetail Detail = MainForm.Instance.mapper.Map<tb_AS_RepairMaterialPickupDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_AS_RepairMaterialPickupDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_AS_RepairMaterialPickupDetail> details = sgd.BindingSourceLines.DataSource as List<tb_AS_RepairMaterialPickupDetail>;
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

        List<tb_AS_RepairMaterialPickupDetail> details = new List<tb_AS_RepairMaterialPickupDetail>();
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
            var eer = errorProviderForAllInput.GetError(txtTotalSendQty);

            bindingSourceSub.EndEdit();

            List<tb_AS_RepairMaterialPickupDetail> oldOjb = new List<tb_AS_RepairMaterialPickupDetail>(details.ToArray());

            List<tb_AS_RepairMaterialPickupDetail> detailentity = bindingSourceSub.DataSource as List<tb_AS_RepairMaterialPickupDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                if (NeedValidated && detailentity.Sum(c => c.ActualSentQty) == 0)
                {
                    MessageBox.Show("明细中，实发总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (NeedValidated && aa.Count > 0)
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

                EditEntity.tb_AS_RepairMaterialPickupDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_AS_RepairMaterialPickupDetail>(details))
                {
                    return false;
                }


                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }


                ReturnMainSubResults<tb_AS_RepairMaterialPickup> SaveResult = new ReturnMainSubResults<tb_AS_RepairMaterialPickup>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.MaterialPickupNO}。");
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
            List<tb_AS_RepairMaterialPickup> EditEntitys = new List<tb_AS_RepairMaterialPickup>();
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_AS_RepairMaterialPickup> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_AS_RepairMaterialPickupController<tb_AS_RepairMaterialPickup> ctr = Startup.GetFromFac<tb_AS_RepairMaterialPickupController<tb_AS_RepairMaterialPickup>>();
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
                MainForm.Instance.PrintInfoLog($"{EditEntity.MaterialPickupNO}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }



        /// <summary>
        /// 由制令单加载明细
        /// 发料单明细相同也不累计了。直接发
        /// </summary>
        /// <param name="id"></param>
        private async  Task LoadChildItems(long? id)
        {

            var ctr = Startup.GetFromFac<tb_AS_RepairOrderController<tb_AS_RepairOrder>>();

            //因为要查BOM情况。不会传过来。
            var SourceBill = await MainForm.Instance.AppContext.Db.Queryable<tb_AS_RepairOrder>().Where(c => c.RepairOrderID == id)
          .Includes(a => a.tb_AS_RepairOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
          .Includes(a => a.tb_AS_RepairOrderMaterialDetails, b => b.tb_proddetail, c => c.tb_Inventories)
          .SingleAsync();
            //新增时才可以转单
            if (SourceBill != null)
            {
                var ctrPickup = Startup.GetFromFac<tb_AS_RepairMaterialPickupController<tb_AS_RepairMaterialPickup>>();
                var entity =await ctrPickup.ToRepairMaterialPickupAsync(SourceBill);
                BusinessHelper.Instance.InitEntity(entity);
                ////编号已经生成，在新点开一个页面时，自动生成。
                //if (EditEntity.MaterialPickupNO.IsNotEmptyOrNull())
                //{
                //    entity.MaterialPickupNO = EditEntity.MaterialPickupNO;
                //}
                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(entity, actionStatus);
            }
        }

        private void chkOutgoing_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
