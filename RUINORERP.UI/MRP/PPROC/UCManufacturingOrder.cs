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
using RUINORERP.UI.CommonUI;
using RUINORERP.Business.BizMapperService;
using HLH.WinControl.MyTypeConverter;
using RUINORERP.Model.CommonModel;

namespace RUINORERP.UI.MRP.MP
{
    [MenuAttrAssemblyInfo("制令单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.制令单)]
    public partial class UCManufacturingOrder : BaseBillEditGeneric<tb_ManufacturingOrder, tb_ManufacturingOrderDetail>
    {
        public UCManufacturingOrder()
        {
            InitializeComponent();
        }

        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_ManufacturingOrder ManufacturingOrder)
            {
                if (ManufacturingOrder.PDID.HasValue && ManufacturingOrder.PDID.Value > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.需求分析;
                    rqp.billId = ManufacturingOrder.PDID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{ManufacturingOrder.PDNO}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(ManufacturingOrder.PDID.Value.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }

                if (ManufacturingOrder.tb_MaterialRequisitions != null && ManufacturingOrder.tb_MaterialRequisitions.Count > 0)
                {
                    foreach (var item in ManufacturingOrder.tb_MaterialRequisitions)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.生产领料单;
                        rqp.billId = item.MR_ID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.MaterialRequisitionNO}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.MR_ID.ToString()))
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
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_ManufacturingOrder, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ManufacturingOrder).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(tb_ManufacturingOrder entityPara, ActionStatus actionStatus)
        {
            tb_ManufacturingOrder entity = entityPara as tb_ManufacturingOrder;
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                cmbCustomerVendor_ID_Out.Visible = entity.IsOutSourced;
                if (entity.MOID > 0)
                {
                    entity.PrimaryKeyID = entity.MOID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色

                    if (entity.tb_bom_s == null && entity.BOM_ID > 0)
                    {
                        //    entity.tb_bom_s = await _appContext.Db.CopyNew().Queryable<tb_BOM_S>()
                        //.Includes(a => a.tb_proddetail, b => b.tb_Inventories)
                        // .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                        //    .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                        //    .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail)
                        //    .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_Inventories)
                        //.Where(c => _bomIDs.Contains(c.BOM_ID)).ToListAsync();
                    }
                }
                else
                {
                    entity.Priority = (int)Priority.正常;
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (entity.MONO.IsNullOrEmpty())
                    {
                        entity.MONO = BizCodeGenerator.Instance.GetBizBillNo(BizType.制令单);
                    }
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    entity.Created_at = System.DateTime.Now;
                }

                chkIsCustomizedOrder.ToolTipValues.Description = "定制单，只借用标品的配方。缴库时，只缴数量不影响成本！";
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v => v.TypeName, cmbProdType);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartment);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_ManufacturingOrder>(entity, k => k.Priority, typeof(Priority), cmbPriority, true);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ManufacturingQty, txtManufacturingQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.PeopleQty, txtPeopleQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.MONO, txtMONO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ManufacturingQty, txtManufacturingQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.PreStartDate, dtpPreStartDate, false);
            DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.PreEndDate, dtpPreEndDate, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.PDNO, txtRefBillNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ManufacturingOrder>(entity, v => v.PDID, txtRefBillNO, true);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.WorkingHour.ToString(), txtWorkingHour, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.MachineHour.ToString(), txtMachineHour, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ApportionedCost.ToString(), txtApportionedCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.TotalManuFee.ToString(), txtTotalManuFee, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.TotalProductionCost.ToString(), txtTotalProductionCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Specifications, txtSpec, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.QuantityDelivered, txtQuantityDelivered, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
            txtQuantityDelivered.ReadOnly = true;
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ManufacturingQty, txtManufacturingQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, cmbLocation_ID);

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, v => v.CNName, txtProdDetail, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ManufacturingOrder>(entity, v => v.ProdDetailID, txtProdDetail, true);

            DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
            DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v => v.UnitName, cmbUnit);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_ManufacturingOrder>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ManufacturingOrder>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_ManufacturingOrderDetails != null && entity.tb_ManufacturingOrderDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ManufacturingOrderDetail>(grid1, sgd, entity.tb_ManufacturingOrderDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ManufacturingOrderDetail>(grid1, sgd, new List<tb_ManufacturingOrderDetail>(), c => c.ProdDetailID);
            }

            /* 因为太复杂。这里先注释掉 只能由需求分析生成

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, v => v.PDNO, txtRefBillNO, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ManufacturingOrder>(entity, v => v.PDID, txtRefBillNO, true);

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProductionDemand).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_ProductionDemand>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);
            DataBindingHelper.InitFilterForControlByExp<tb_ProductionDemand>(entity, txtRefBillNO, c => c.PDNo, queryFilter, null, c => c.PDID);
            */



            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }

                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.PDID.HasValue && entity.PDID > 0)
                {
                    //修改是否外发时，加工费可能不一样。
                    if (s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.IsOutSourced) && entity.tb_bom_s != null)
                    {
                        if (entity.IsOutSourced)
                        {
                            entity.ApportionedCost = entity.tb_bom_s.OutApportionedCost / entity.tb_bom_s.OutputQty * entity.ManufacturingQty;
                            entity.TotalManuFee = entity.tb_bom_s.TotalOutManuCost / entity.tb_bom_s.OutputQty * entity.ManufacturingQty;
                        }
                        else
                        {
                            entity.ApportionedCost = entity.tb_bom_s.SelfApportionedCost / entity.tb_bom_s.OutputQty * entity.ManufacturingQty;
                            entity.TotalManuFee = entity.tb_bom_s.TotalSelfManuCost / entity.tb_bom_s.OutputQty * entity.ManufacturingQty;
                            entity.CustomerVendor_ID_Out = null;
                        }
                        entity.TotalProductionCost = entity.ApportionedCost + entity.TotalManuFee + entity.TotalMaterialCost;
                    }
                }
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.PDID.HasValue)
                {
                    if (entity.PDID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.PDID))
                    {
                        //因为太复杂。目前暂时只能由需求分析那边生成
                        //LoadChildItems(entity.PDID.Value);
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.DepartmentID))
                    {
                        if (cmbDepartment.SelectedIndex == 0)
                        {
                            entity.DepartmentID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.CustomerVendor_ID))
                    {
                        if (cmbCustomerVendor_ID.SelectedIndex == 0)
                        {
                            entity.CustomerVendor_ID = null;
                            cmbCustomerVendor_ID.SelectedIndex = -1;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.IsOutSourced))
                    {
                        cmbCustomerVendor_ID_Out.Visible = entity.IsOutSourced;
                        if (entity.IsOutSourced)
                        {
                            entity.DepartmentID = null;
                        }
                    }
                    ToolBarEnabledControl(entity);
                }

                //影响子件的数量
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.ManufacturingQty))
                {
                    if (EditEntity.BOM_ID > 0 && EditEntity.tb_ManufacturingOrderDetails.Count > 0)
                    {
                        if (EditEntity.tb_bom_s == null)
                        {
                            EditEntity.tb_bom_s = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                            .Includes(c => c.tb_BOM_SDetails)
                            .Where(c => c.BOM_ID == EditEntity.BOM_ID).Single();
                        }
                        decimal bomOutQty = EditEntity.tb_bom_s.OutputQty;
                        for (int i = 0; i < EditEntity.tb_ManufacturingOrderDetails.Count; i++)
                        {
                            tb_BOM_SDetail bOM_SDetail = EditEntity.tb_bom_s.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == EditEntity.tb_ManufacturingOrderDetails[i].ProdDetailID);
                            if (bOM_SDetail != null)
                            {
                                EditEntity.tb_ManufacturingOrderDetails[i].ShouldSendQty = (bOM_SDetail.UsedQty.ToInt() * (EditEntity.ManufacturingQty / bomOutQty)).ToInt();
                            }
                        }

                        //同步到明细UI表格中？
                        sgh.SynchronizeUpdateCellValue<tb_ManufacturingOrderDetail>(sgd, c => c.ShouldSendQty, EditEntity.tb_ManufacturingOrderDetails);
                    }
                }

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_ManufacturingOrder>(c => c.DataStatus))
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


            //===


            //创建表达式 客户
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

            // 不一致后面处理 要改写两个方法

            //创建表达式 外发工厂
            var lambdaOut = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsOther == true)
                            .ToExpression();

            BaseProcessor baseProcessorOut = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterCOut = baseProcessorOut.GetQueryFilter();
            queryFilterCOut.FilterLimitExpressions.Add(lambdaOut);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor, tb_ManufacturingOrder>(entity, k => k.CustomerVendor_ID, v => v.CVName, k => k.CustomerVendor_ID_Out.Value, cmbCustomerVendor_ID_Out, true, queryFilterCOut.GetFilterExpression<tb_CustomerVendor>());
            //如果要查询功能。可能要优化一个方法 像上面绑定一下能指定不同的字段 如果不注释就会绑定时出错。赋值不到 CustomerVendor_ID_Out  TODO要优化一下。暂时只用下拉
            //DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID_Out, c => c.CVName, queryFilterCOut);


            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ManufacturingOrderValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
            toolStripbtnAdd.Enabled = false;
            toolStripbtnAdd.ToolTipText = "制令单由需求分析中生成。";
            toolStripbtnAdd.AutoToolTip = false;
        }


        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_ManufacturingOrderDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_ManufacturingOrderDetail>();
        SourceGridHelper sgh = new SourceGridHelper();

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {


            //InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_ManufacturingOrderDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_ManufacturingOrderDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<tb_ManufacturingOrderDetail>(c => c.ProdDetailID);
            //listCols.SetCol_NeverVisible<tb_ManufacturingOrderDetail>(c => c.BOM_ID);
            listCols.SetCol_NeverVisible<tb_ManufacturingOrderDetail>(c => c.BOM_NO);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //listCols.SetCol_DefaultValue<tb_ManufacturingOrderDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            listCols.SetCol_ReadOnly<tb_ManufacturingOrderDetail>(c => c.BOM_ID);
            listCols.SetCol_ReadOnly<tb_ManufacturingOrderDetail>(c => c.property);
            listCols.SetCol_ReadOnly<tb_ManufacturingOrderDetail>(c => c.ActualSentQty);
            listCols.SetCol_ReadOnly<tb_ManufacturingOrderDetail>(c => c.OverSentQty);
            listCols.SetCol_ReadOnly<tb_ManufacturingOrderDetail>(c => c.WastageQty);
            listCols.SetCol_ReadOnly<tb_ManufacturingOrderDetail>(c => c.CurrentIinventory);


            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.ActualSentQty);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.ShouldSendQty);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.ActualSentQty);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.OverSentQty);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.WastageQty);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.CurrentIinventory);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.SubtotalUnitCost);
            listCols.SetCol_Summary<tb_ManufacturingOrderDetail>(c => c.UnitCost);

            //制令单前期是计划性的。以计划的生产数量为标准去算成本这些
            listCols.SetCol_Formula<tb_ManufacturingOrderDetail>((a, b) => a.ShouldSendQty * b.UnitCost, c => c.SubtotalUnitCost);
            //bomid的下拉值。受当前行选择时会改变下拉范围,由产品ID决定BOM显示
            sgh.SetCol_LimitedConditionsForSelectionRange<tb_ManufacturingOrderDetail>(sgd, t => t.ProdDetailID, f => f.BOM_ID);


            //if (CurMenuInfo.tb_P4Fields != null)
            //{
            //    List<tb_P4Field> P4Fields =
            //        CurMenuInfo.tb_P4Fields
            //        .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
            //        && p.tb_fieldinfo.IsChild && !p.IsVisble).ToList();
            //    foreach (var item in P4Fields)
            //    {
            //        //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
            //        listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_ManufacturingOrderDetail));
            //    }

            //}
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_ManufacturingOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_ManufacturingOrderDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_ManufacturingOrderDetail>(sgd, f => f.BOM_ID, t => t.BOM_ID);
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_ManufacturingOrderDetail>(sgd, f => f.Quantity, t => t.CurrentIinventory);
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_ManufacturingOrderDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_ManufacturingOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);

            // TODO by watson 如果有BOM则显示BOM里的成本。如果没有则是库存成本，如果这里要算成本人工费这些，还要进一步处理 制令单与BOM清单中的各中费用关系
            //如果
            //sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_ManufacturingOrderDetail>(sgd, f => f.Inv_Cost t => t.MaterialCost);

            List<tb_ManufacturingOrderDetail> lines = new List<tb_ManufacturingOrderDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;


            sgd.SetDependencyObject<ProductSharePart, tb_ManufacturingOrderDetail>(MainForm.Instance.View_ProdDetailList);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ManufacturingOrderDetail));
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
                List<tb_ManufacturingOrderDetail> details = new List<tb_ManufacturingOrderDetail>();

                foreach (var item in RowDetails)
                {
                    tb_ManufacturingOrderDetail Detail = MainForm.Instance.mapper.Map<tb_ManufacturingOrderDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_ManufacturingOrderDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_ManufacturingOrderDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ManufacturingOrderDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                //if (EditEntity.IsOutSourced)
                //{
                //    //外发
                //    EditEntity.TotalManuFee =bom EditEntity.ManufacturingQty
                //}
                //else
                //{
                //    //自制
                //}
                EditEntity.TotalMaterialCost = details.Sum(c => c.SubtotalUnitCost);
                EditEntity.TotalProductionCost = EditEntity.TotalMaterialCost + EditEntity.ApportionedCost + EditEntity.TotalManuFee;
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_ManufacturingOrderDetail> details = new List<tb_ManufacturingOrderDetail>();




        /// <summary>
        /// 制作令明细中是可以存在相同产品的并且数量不同，会在领料单中合并
        /// </summary>
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtManufacturingQty);

            bindingSourceSub.EndEdit();

            List<tb_ManufacturingOrderDetail> oldOjb = new List<tb_ManufacturingOrderDetail>(details.ToArray());

            List<tb_ManufacturingOrderDetail> detailentity = bindingSourceSub.DataSource as List<tb_ManufacturingOrderDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                if (NeedValidated && (EditEntity.ManufacturingQty == 0 || detailentity.Sum(c => c.ShouldSendQty) == 0))
                {
                    MessageBox.Show("生产数量或明细应发数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                EditEntity.tb_ManufacturingOrderDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ManufacturingOrderDetail>(details))
                {
                    return false;
                }

                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                if (NeedValidated)
                {
                    if (EditEntity.tb_MaterialRequisitions != null && EditEntity.tb_MaterialRequisitions.Count > 0)
                    {
                        MessageBox.Show("当前制令单已经存在领料单数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                if (NeedValidated)
                {
                    if (EditEntity.tb_FinishedGoodsInvs != null && EditEntity.tb_FinishedGoodsInvs.Count > 0)
                    {
                        MessageBox.Show("当前制令单已经存在缴库数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                //成本的检测，放到审核阶段来
                if (NeedValidated)
                {

                    if (EditEntity.tb_bom_s == null)
                    {
                        var bomCtroller = MainForm.Instance.AppContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                        EditEntity.tb_bom_s = await bomCtroller.BaseQueryByIdAsync(EditEntity.BOM_ID);
                    }

                    // 1. 配方单位成本
                    decimal bomUnitCost = EditEntity.IsOutSourced
                        ? EditEntity.tb_bom_s.OutProductionAllCosts
                        : EditEntity.tb_bom_s.SelfProductionAllCosts;

                    // 2. 制令单实际单位成本
                    decimal moUnitCost = EditEntity.ManufacturingQty == 0
                        ? 0
                        : EditEntity.TotalProductionCost / EditEntity.ManufacturingQty;

                    // 3. 差异阈值（10%）
                    const decimal tolerance = 0.1m;
                    const decimal Bigtolerance = 0.5m;
                    decimal diffRatio = bomUnitCost == 0
                        ? (moUnitCost == 0 ? 0 : 1)   // 避免除以 0
                        : Math.Abs(moUnitCost - bomUnitCost) / bomUnitCost;

                    // 4. 差异过大，提示并阻断
                    if (diffRatio > tolerance)
                    {
                        string msg = $"产出母件的生产单位成本({moUnitCost:N2})与配方成本({bomUnitCost:N2})差异过大（>{tolerance:P0}），\n" +
                                     "是否为定制单？确定继续吗？";
                        if (MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return false;
                        }

                    }
                    // 4. 差异过大，一半了，提示并阻断
                    if (diffRatio > Bigtolerance)
                    {
                        string msg = $"产出母件的生产单位成本({moUnitCost:N2})与配方成本({bomUnitCost:N2})差异过大（>{Bigtolerance:P0}），\n" +
                                     "是否为定制单？请检查数据后重试！";
                        System.Windows.Forms.MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }


                ReturnMainSubResults<tb_ManufacturingOrder> SaveResult = new ReturnMainSubResults<tb_ManufacturingOrder>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.MONO}。");
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

/// <summary>
/// 制作令明细中是可以存在相同产品的并且数量不同，会在领料单中合并
/// </summary>
/// <returns></returns>
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

    if (EditEntity.tb_ManufacturingOrderDetails == null || EditEntity.tb_ManufacturingOrderDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整产品数量和金额数据。", UILogType.警告);
        return null;
    }

    RevertCommand command = new RevertCommand();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_ManufacturingOrder oldobj = CloneHelper.DeepCloneObject<tb_ManufacturingOrder>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_ManufacturingOrder>(EditEntity, oldobj);
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
    tb_ManufacturingOrderController<tb_ManufacturingOrder> ctr = Startup.GetFromFac<tb_ManufacturingOrderController<tb_ManufacturingOrder>>();
    List<tb_ManufacturingOrder> entitys = new List<tb_ManufacturingOrder>();
    entitys.Add(EditEntity);
    ReturnResults<bool> rmrs = await ctr.BatchApprovalAsync(entitys, ae);
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
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,原因是：{rmrs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
    }
    return ae;
}


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
    String reportFile = typeof(tb_ManufacturingOrder).Name + ".frx";
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
            string ReprotfileName = typeof(tb_ManufacturingOrder).Name + ".frx";
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = ReprotfileName;
            // frm.ShowDialog();
            //调用内置方法 给数据源 新编辑，后面的话，直接load 可以不用给数据源的格式
            //string ReprotfileName = "SOB.frx";
            //List<tb_ManufacturingOrder> main = new List<tb_ManufacturingOrder>();
            //_EditEntity.tb_sales_order_details = details;
            //main.Add(_EditEntity);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            List<tb_ManufacturingOrder> rptSources = new List<tb_ManufacturingOrder>();
            tb_ManufacturingOrder saleOrder = EditEntity as tb_ManufacturingOrder;
            saleOrder.tb_ManufacturingOrderDetails = details;
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
            //List<tb_ManufacturingOrder> main = new List<tb_ManufacturingOrder>();
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


            if (EditEntity.tb_ManufacturingOrderDetails == null || EditEntity.tb_ManufacturingOrderDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return ae;
            }

            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_ManufacturingOrder oldobj = CloneHelper.DeepCloneObject<tb_ManufacturingOrder>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_ManufacturingOrder>(EditEntity, oldobj);
            };

            tb_ManufacturingOrderController<tb_ManufacturingOrder> ctr = Startup.GetFromFac<tb_ManufacturingOrderController<tb_ManufacturingOrder>>();
            List<tb_ManufacturingOrder> list = new List<tb_ManufacturingOrder>();
            list.Add(EditEntity);
            ReturnResults<bool> rs = await ctr.BatchAntiApprovalAsync(list);
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
                MainForm.Instance.PrintInfoLog($"{EditEntity.MONO}反审失败,{rs.ErrorMsg}请联系管理员！", Color.Red);
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
            List<tb_ManufacturingOrder> EditEntitys = new List<tb_ManufacturingOrder>();
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_ManufacturingOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }
            bool baseRs = await base.CloseCaseAsync();
            return baseRs;
        }

        // 移除未使用的BizTypeMapper，使用EntityMappingHelper代替
 

        private void chkIsOutSourced_CheckedChanged(object sender, EventArgs e)
        {
            cmbCustomerVendor_ID_Out.Visible = chkIsOutSourced.Checked;
        }


    }
}
