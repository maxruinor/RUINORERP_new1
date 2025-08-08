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
using RUINORERP.Model.CommonModel;
using SourceGrid;
using RUINORERP.Business.CommService;
using NPOI.POIFS.Properties;
using System.Diagnostics;
using RUINORERP.Common.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using MySqlX.XDevAPI.Common;
using RUINORERP.UI.AdvancedUIModule;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.UI.CommonUI;

using RUINORERP.Global.EnumExt;
using Fireasy.Common.Configuration;
using RUINORERP.UI.Monitoring.Auditing;
using NPOI.SS.Formula.Functions;
using Netron.GraphLib;
using Krypton.Toolkit;


namespace RUINORERP.UI.ASS
{


    /// <summary>
    /// 销售订单时：有运费外币，总金额外币，订单外币。反而出库时不用这么多。外币只是用于记账。出库时只要根据本币和外币及汇率。生成应收时自动算出来。
    /// </summary>
    [MenuAttrAssemblyInfo("售后交付单", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.售后流程, BizType.售后交付单)]
    public partial class UCASAfterSaleDelivery : BaseBillEditGeneric<tb_AS_AfterSaleDelivery, tb_AS_AfterSaleDeliveryDetail>, IPublicEntityObject
    {
        public UCASAfterSaleDelivery()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_AS_AfterSaleDelivery>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority, false);
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_AS_AfterSaleDelivery, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_AfterSaleDelivery).Name + "Processor");
            //QueryConditionFilter = baseProcessor.GetQueryFilter();
            base.QueryConditionBuilder();
            //创建表达式
            var lambda = Expressionable.Create<tb_AS_AfterSaleDelivery>()
                             .And(t => t.isdeleted == false)
                            //自己的只能查自己的
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);
        }




        protected override void LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_AS_AfterSaleDelivery AfterSaleDelivery)
            {
                if (AfterSaleDelivery.ASApplyID.HasValue && AfterSaleDelivery.ASApplyID.Value > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.售后申请单;
                    rqp.billId = AfterSaleDelivery.ASApplyID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{AfterSaleDelivery.ASDeliveryNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(AfterSaleDelivery.ASDeliveryID.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }
            }
            base.LoadRelatedDataToDropDownItemsAsync();
        }


        public override void BindData(tb_AS_AfterSaleDelivery entityPara, ActionStatus actionStatus)
        {
            tb_AS_AfterSaleDelivery entity = entityPara as tb_AS_AfterSaleDelivery;

            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.ASDeliveryID > 0)
                {
                    entity.PrimaryKeyID = entity.ASDeliveryID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;

                    if (string.IsNullOrEmpty(entity.ASDeliveryNo))
                    {
                        entity.ASDeliveryNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.售后交付单);
                    }
                    entity.ApprovalOpinions = string.Empty;
                    entity.DeliveryDate = System.DateTime.Now;
                    if (entity.tb_AS_AfterSaleDeliveryDetails != null && entity.tb_AS_AfterSaleDeliveryDetails.Count > 0)
                    {
                        entity.tb_AS_AfterSaleDeliveryDetails.ForEach(c => c.ASDeliveryID = 0);
                        entity.tb_AS_AfterSaleDeliveryDetails.ForEach(c => c.ASApplyDetailID = 0);
                    }


                    UIHelper.ControlForeignFieldInvisible<tb_AS_AfterSaleDelivery>(this, false);
                }
            }
            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            //==
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ASDeliveryNo, txtASDeliveryNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.TotalDeliveryQty, txtTotalDeliveryQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);


            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty, false);

            //==


            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.Approver_at, dtpApprover_at, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingWay, txtshippingWay, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            //default  DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
            // DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalResults, chkApprovalResults, false);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            if (AppContext.projectGroups != null && AppContext.projectGroups.Count > 0)
            {
                #region 项目组 如果有设置则按设置。没有则全部
                cmbProjectGroup.DataSource = null;
                cmbProjectGroup.DataBindings.Clear();
                BindingSource bs = new BindingSource();
                bs.DataSource = AppContext.projectGroups;
                ComboBoxHelper.InitDropList(bs, cmbProjectGroup, "ProjectGroup_ID", "ProjectGroupName", ComboBoxStyle.DropDownList, false);
                var depa = new Binding("SelectedValue", entity, "ProjectGroup_ID", true, DataSourceUpdateMode.OnValidation);
                //数据源的数据类型转换为控件要求的数据类型。
                depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                //将控件的数据类型转换为数据源要求的数据类型。
                depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                cmbProjectGroup.DataBindings.Add(depa);
                #endregion
            }
            else
            {
                DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            }

            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingWay, txtshippingWay, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_AS_AfterSaleDelivery>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_AS_AfterSaleDeliveryDetails != null && entity.tb_AS_AfterSaleDeliveryDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_AS_AfterSaleDeliveryDetails);
                sgh.LoadItemDataToGrid<tb_AS_AfterSaleDeliveryDetail>(grid1, sgd, entity.tb_AS_AfterSaleDeliveryDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_AS_AfterSaleDeliveryDetail>());
                sgh.LoadItemDataToGrid<tb_AS_AfterSaleDeliveryDetail>(grid1, sgd, new List<tb_AS_AfterSaleDeliveryDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null)
                {
                    return;
                }

                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0
              && s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleDelivery>(c => c.ASApplyID))
                {
                    AfterSaleDelivery(entity.ASApplyID);
                }


                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    //if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleDelivery>(c => c.ExpenseAllocationMode) && entity.ExpenseAllocationMode.HasValue && entity.ExpenseAllocationMode.Value > 0)
                    //{
                    //    if (entity.ExpenseAllocationMode.Value == (int)ExpenseAllocationMode.单一承担)
                    //    {
                    //        //默认为客户
                    //        entity.ExpenseBearerType = (int)ExpenseBearerType.客户;
                    //    }
                    //}

                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleDelivery>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            if (ProjectGroup.tb_ProjectGroupAccountMappers != null && ProjectGroup.tb_ProjectGroupAccountMappers.Count > 0)
                            {
                                EditEntity.tb_projectgroup = ProjectGroup;
                            }
                        }
                    }
                }

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleDelivery>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }

                //如果客户有变化，带出对应有业务员
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleDelivery>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            if (!string.IsNullOrEmpty(cv.SpecialNotes))
                            {
                                entity.Notes = $"【{cv.SpecialNotes}】";
                            }
                            if (cv.Employee_ID.HasValue)
                            {
                                EditEntity.Employee_ID = cv.Employee_ID.Value;
                            }
                            //客户的 地址 电话 联系人都显示到收货地址中。
                            //如果手机为空则显示座机
                            if (string.IsNullOrEmpty(cv.MobilePhone))
                            {
                                cv.MobilePhone = cv.Phone;
                            }
                            EditEntity.ShippingAddress = cv.Address + " " + cv.MobilePhone + " " + cv.Contact;
                        }
                    }
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

            if (EditEntity.PrintStatus == 0)
            {
                lblPrintStatus.Text = "未打印";
            }
            else
            {
                lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
            }
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



            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, v => v.ASApplyNo, txtASApplyNo, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaSO = Expressionable.Create<tb_AS_AfterSaleApply>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                             .And(t => t.isdeleted == false)
                             .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessorSO = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_AfterSaleApply).Name + "Processor");
            QueryFilter queryFilterSO = baseProcessorSO.GetQueryFilter();
            queryFilterSO.FilterLimitExpressions.Add(lambdaSO);

            ControlBindingHelper.ConfigureControlFilter<tb_AS_AfterSaleDelivery, tb_AS_AfterSaleApply>(entity, txtASApplyNo, t => t.ASApplyNo,
              f => f.ASApplyNo, queryFilterSO, a => a.ASApplyID, b => b.ASApplyID, null, false);


            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_AS_AfterSaleDeliveryValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        // 在基类中定义静态属性

        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_AS_AfterSaleDeliveryDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_AS_AfterSaleDeliveryDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_AS_AfterSaleDeliveryDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_AS_AfterSaleDeliveryDetail>(c => c.ASApplyDetailID);
            listCols.SetCol_NeverVisible<tb_AS_AfterSaleDeliveryDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }


            //listCols.SetCol_DefaultValue<tb_AS_AfterSaleDeliveryDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);
            //listCols.SetCol_ReadOnly<tb_AS_AfterSaleDeliveryDetail>(c => c.Quantity);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;



            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_AS_AfterSaleDeliveryDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_AS_AfterSaleDeliveryDetail>(item);
                }
            }
            listCols.SetCol_Summary<tb_AS_AfterSaleDeliveryDetail>(c => c.Quantity);




            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_AfterSaleDeliveryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_AfterSaleDeliveryDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_AfterSaleDeliveryDetail>(sgd, f => f.Model, t => t.CustomerPartNo, false);

            //应该只提供一个结构
            List<tb_AS_AfterSaleDeliveryDetail> lines = new List<tb_AS_AfterSaleDeliveryDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;

            sgd.SetDependencyObject<ProductSharePart, tb_AS_AfterSaleDeliveryDetail>(list);
            sgd.HasRowHeader = true;
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            sgh.InitGrid(grid1, sgd, true, nameof(tb_AS_AfterSaleDeliveryDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;

            sw.Stop();
            MainForm.Instance.uclog.AddLog("加载数据耗时：" + sw.ElapsedMilliseconds + "毫秒");

            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            UIHelper.ControlForeignFieldInvisible<tb_AS_AfterSaleDelivery>(this, false);

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
                List<tb_AS_AfterSaleDeliveryDetail> details = new List<tb_AS_AfterSaleDeliveryDetail>();

                foreach (var item in RowDetails)
                {
                    tb_AS_AfterSaleDeliveryDetail Detail = MainForm.Instance.mapper.Map<tb_AS_AfterSaleDeliveryDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_AS_AfterSaleDeliveryDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
            Summation();
        }

        private void Summation()
        {
            try
            {
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_AS_AfterSaleDeliveryDetail> details = sgd.BindingSourceLines.DataSource as List<tb_AS_AfterSaleDeliveryDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalDeliveryQty = details.Sum(c => c.Quantity);

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_AS_AfterSaleDeliveryDetail> details = new List<tb_AS_AfterSaleDeliveryDetail>();
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



            //如果订单 选择了未付款，但是又选择了非账期的即实收账方式。则审核不通过。
            //如果订单选择了 非未付款，但又选择了账期也不能通过。
            if (NeedValidated)
            {

                if (EditEntity.ASApplyNo.Trim().Length == 0)
                {
                    MessageBox.Show("单据编号由系统自动生成，如果不小心清除，请重新生成单据的单据编号。");
                    return false;
                }

                if (EditEntity.ProjectGroup_ID.HasValue && EditEntity.ProjectGroup_ID.Value <= 0)
                {
                    EditEntity.ProjectGroup_ID = null;
                }


            }

            var eer = errorProviderForAllInput.GetError(txtTotalDeliveryQty);

            bindingSourceSub.EndEdit();

            List<tb_AS_AfterSaleDeliveryDetail> oldOjb = new List<tb_AS_AfterSaleDeliveryDetail>(details.ToArray());

            List<tb_AS_AfterSaleDeliveryDetail> detailentity = bindingSourceSub.DataSource as List<tb_AS_AfterSaleDeliveryDetail>;

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();


                EditEntity.TotalDeliveryQty = details.Sum(c => c.Quantity);



                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (NeedValidated && EditEntity.TotalDeliveryQty != details.Sum(c => c.Quantity))
                {
                    MessageBox.Show($"单据总交付数量{EditEntity.TotalDeliveryQty}和明细交付数量之和{detailentity.Sum(c => c.Quantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                //订单只是警告。可以继续

                EditEntity.tb_AS_AfterSaleDeliveryDetails = details;


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_AS_AfterSaleDeliveryDetail>(details))
                {
                    return false;
                }



                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }


                //if (NeedValidated)
                //{
                //    if (EditEntity.tb_AS_RepairOrders != null && EditEntity.tb_AS_RepairOrders.Count > 0)
                //    {
                //        MessageBox.Show("当前【售后交付单】已有维修单数据，无法修改保存。请检查数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return false;
                //    }
                //}


                ReturnMainSubResults<tb_AS_AfterSaleDelivery> SaveResult = new ReturnMainSubResults<tb_AS_AfterSaleDelivery>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ASApplyNo}。");
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












        protected async override Task<bool> AntiCloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_AS_AfterSaleDelivery>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<tb_AS_AfterSaleDelivery>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_AS_AfterSaleDelivery> EditEntitys = new List<tb_AS_AfterSaleDelivery>();
                EditEntity.Notes = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的,结案了的才能反结案
                List<tb_AS_AfterSaleDelivery> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.完结 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要反结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_AS_AfterSaleDeliveryController<tb_AS_AfterSaleDelivery> ctr = Startup.GetFromFac<tb_AS_AfterSaleDeliveryController<tb_AS_AfterSaleDelivery>>();
                ReturnResults<bool> rs = await ctr.AntiBatchCloseCaseAsync(needCloseCases);
                if (rs.Succeeded)
                {
                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_AfterSaleDelivery>("反结案", EditEntity, $"反结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.ASApplyNo}反结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_AS_AfterSaleDelivery>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<tb_AS_AfterSaleDelivery>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_AS_AfterSaleDelivery> EditEntitys = new List<tb_AS_AfterSaleDelivery>();
                EditEntity.Notes = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的并且通过的情况才能结案
                List<tb_AS_AfterSaleDelivery> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_AS_AfterSaleDeliveryController<tb_AS_AfterSaleDelivery> ctr = Startup.GetFromFac<tb_AS_AfterSaleDeliveryController<tb_AS_AfterSaleDelivery>>();
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
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_AfterSaleDelivery>("结案", EditEntity, $"结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.ASApplyNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        private async void AfterSaleDelivery(long? ASApplyID)
        {
            //要加一个判断 值是否有变化
            //新增时才可以
            //转单
            ButtonSpecAny bsa = (txtASApplyNo as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var AfterSaleApply = bsa.Tag as tb_AS_AfterSaleApply;//这个tag值。赋值会比较当前方法晚，所以失效
            AfterSaleApply = await MainForm.Instance.AppContext.Db.Queryable<tb_AS_AfterSaleApply>().Where(c => c.ASApplyID == ASApplyID)
            .Includes(t => t.tb_AS_AfterSaleApplyDetails, d => d.tb_proddetail)
            .SingleAsync();
            if (AfterSaleApply != null)
            {
                var ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
                tb_AS_AfterSaleDelivery SaleDelivery = ctr.ToAfterSaleDelivery(AfterSaleApply);
                BindData(SaleDelivery, ActionStatus.无操作);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("选取的对象为空！");
            }
        }


        private void lblCustomerVendor_ID_Click(object sender, EventArgs e)
        {

        }

        private void cmbCustomerVendor_ID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
