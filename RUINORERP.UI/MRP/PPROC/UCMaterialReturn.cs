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

namespace RUINORERP.UI.MRP.MP
{
    [MenuAttrAssemblyInfo("生产退料单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.生产退料单)]
    public partial class UCMaterialReturn : BaseBillEditGeneric<tb_MaterialReturn, tb_MaterialReturn>
    {
        public UCMaterialReturn()
        {
            InitializeComponent();
            // InitDataToCmbByEnumDynamicGeneratedDataSource<tb_MaterialReturn>(typeof(Priority), e => e.Priority, cmbOrderPriority, false);
        }



        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_MaterialReturn, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MaterialReturn).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(tb_MaterialReturn entityPara, ActionStatus actionStatus)
        {
            tb_MaterialReturn entity = entityPara as tb_MaterialReturn;
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                cmbCustomerVendor_ID.Visible = entity.Outgoing;
                if (entity.MRE_ID > 0)
                {
                    entity.PrimaryKeyID = entity.MRE_ID;
                    entity.ActionStatus = ActionStatus.加载;
                   
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (entity.BillNo.IsNullOrEmpty())
                    {
                        entity.BillNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.生产退料单);
                    }
                    entity.ReturnDate = System.DateTime.Now;
                    entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    if (entity.tb_MaterialReturnDetails != null && entity.tb_MaterialReturnDetails.Count > 0)
                    {
                        entity.tb_MaterialReturnDetails.ForEach(c => c.MRE_ID = 0);
                        entity.tb_MaterialReturnDetails.ForEach(c => c.Sub_ID = 0);
                    }
                }
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;

            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.TotalCostAmount.ToString(), txtTotalCostAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_MaterialReturn>(entity, t => t.ReturnDate, dtpReturnDate, false);

            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
            DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.Outgoing, chkOutgoing, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);

            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_MaterialReturn>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_MaterialReturn>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_MaterialReturnDetails != null && entity.tb_MaterialReturnDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_MaterialReturnDetail>(grid1, sgd, entity.tb_MaterialReturnDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_MaterialReturnDetail>(grid1, sgd, new List<tb_MaterialReturnDetail>(), c => c.ProdDetailID);
            }
            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }

                //如果是制令单引入变化则加载明细及相关数据
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    if (entity.MR_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_MaterialReturn>(c => c.MR_ID))
                    {
                        LoadChildItems(entity.MR_ID);
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_MaterialReturn>(c => c.DepartmentID))
                    {
                        if (cmbDepartmentID.SelectedIndex == 0)
                        {
                            entity.DepartmentID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_MaterialReturn>(c => c.CustomerVendor_ID))
                    {
                        if (cmbCustomerVendor_ID.SelectedIndex == 0)
                        {
                            entity.CustomerVendor_ID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_MaterialReturn>(c => c.Outgoing))
                    {
                        cmbCustomerVendor_ID.Visible = entity.Outgoing;
                        if (entity.Outgoing)
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

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_MaterialReturn>(c => c.DataStatus))
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

            //显示 打印状态 如果是草稿状态 不显示打印
            if (EditEntity.PrintStatus == 0)
            {
                lblPrintStatus.Text = "未打印";
            }
            else
            {
                lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
            }

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, v => v.MaterialRequisitionNO, txtRefNO, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_MaterialReturn>(entity, v => v.MR_ID, txtRefNO, true);

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MaterialRequisition).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_MaterialRequisition>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
                             //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);

            DataBindingHelper.InitFilterForControlByExp<tb_MaterialRequisition>(entity, txtRefNO, c => c.MaterialRequisitionNO, queryFilter);


            //创建表达式 外发工厂
            var lambdaOut = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsOther == true)
                            .ToExpression();

            BaseProcessor baseProcessorOut = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterCOut = baseProcessorOut.GetQueryFilter();
            queryFilterCOut.FilterLimitExpressions.Add(lambdaOut);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor, tb_MaterialReturn>(entity, k => k.CustomerVendor_ID, v => v.CVName, k => k.CustomerVendor_ID.Value, cmbCustomerVendor_ID, true, queryFilterCOut.GetFilterExpression<tb_CustomerVendor>());
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterCOut);


            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_MaterialReturnValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_MaterialReturnDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_MaterialReturnDetail>();
        SourceGridHelper sgh = new SourceGridHelper();

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            //InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_MaterialReturnDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;



            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_MaterialReturnDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<tb_MaterialReturnDetail>(c => c.ProdDetailID);
            //listCols.SetCol_NeverVisible<tb_MaterialReturnDetail>(c => c.BOM_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //listCols.SetCol_DefaultValue<tb_MaterialReturnDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            listCols.SetCol_ReadOnly<tb_MaterialReturnDetail>(c => c.Cost);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;


            listCols.SetCol_Summary<tb_MaterialReturnDetail>(c => c.Quantity);


            //bomid的下拉值。受当前行选择时会改变下拉范围,由产品ID决定BOM显示
            // sgh.SetCol_LimitedConditionsForSelectionRange<tb_MaterialReturnDetail>(sgd, t => t.ProdDetailID, f => f.BOM_ID);


            //if (CurMenuInfo.tb_P4Fields != null)
            //{
            //    List<tb_P4Field> P4Fields =
            //        CurMenuInfo.tb_P4Fields
            //        .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
            //        && p.tb_fieldinfo.IsChild && !p.IsVisble).ToList();
            //    foreach (var item in P4Fields)
            //    {
            //        //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
            //        listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_MaterialReturnDetail));
            //    }

            //}
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_MaterialReturnDetail>(sgd, f => f.Location_ID, t => t.Location_ID);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_MaterialReturnDetail>(sgd, f => f.prop, t => t.property);




            //应该只提供一个结构
            List<tb_MaterialReturnDetail> lines = new List<tb_MaterialReturnDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;


            sgd.SetDependencyObject<ProductSharePart, tb_MaterialReturnDetail>(MainForm.Instance.list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_MaterialReturnDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo,this);
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
                List<tb_MaterialReturnDetail> details = new List<tb_MaterialReturnDetail>();
                
                foreach (var item in RowDetails)
                {
                    tb_MaterialReturnDetail Detail = MainForm.Instance.mapper.Map<tb_MaterialReturnDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_MaterialReturnDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_MaterialReturnDetail> details = sgd.BindingSourceLines.DataSource as List<tb_MaterialReturnDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalAmount = details.Sum(c => c.Price * c.Quantity);
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCostAmount = details.Sum(c => c.Cost * c.Quantity);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_MaterialReturnDetail> details = new List<tb_MaterialReturnDetail>();
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
            var eer = errorProviderForAllInput.GetError(txtTotalQty);

            bindingSourceSub.EndEdit();

            List<tb_MaterialReturnDetail> oldOjb = new List<tb_MaterialReturnDetail>(details.ToArray());

            List<tb_MaterialReturnDetail> detailentity = bindingSourceSub.DataSource as List<tb_MaterialReturnDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                //if (EditEntity.TotatQty == 0 || detailentity.Sum(c => c.Quantity) == 0)
                //{
                //    MessageBox.Show("单据及明细总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                //if (EditEntity.TotatQty != detailentity.Sum(c => c.Quantity))
                //{
                //    MessageBox.Show($"单据总数量{EditEntity.TotatQty}和明细总数量{detailentity.Sum(c => c.Quantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
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
                EditEntity.tb_MaterialReturnDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_MaterialReturnDetail>(details))
                {
                    return false;
                }


                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }


                ReturnMainSubResults<tb_MaterialReturn> SaveResult = new ReturnMainSubResults<tb_MaterialReturn>();
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

        if (EditEntity.tb_MaterialReturnDetails == null || EditEntity.tb_MaterialReturnDetails.Count == 0)
        {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整产品数量和金额数据。", UILogType.警告);
        return null;
        }

        RevertCommand command = new RevertCommand();
        //缓存当前编辑的对象。如果撤销就回原来的值
        tb_MaterialReturn oldobj = CloneHelper.DeepCloneObject<tb_MaterialReturn>(EditEntity);
        command.UndoOperation = delegate ()
        {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_MaterialReturn>(EditEntity, oldobj);
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
        tb_MaterialReturnController<tb_MaterialReturn> ctr = Startup.GetFromFac<tb_MaterialReturnController<tb_MaterialReturn>>();
        List<tb_MaterialReturn> entitys = new List<tb_MaterialReturn>();
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
        String reportFile = typeof(tb_MaterialReturn).Name + ".frx";
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
            string ReprotfileName = typeof(tb_MaterialReturn).Name + ".frx";
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = ReprotfileName;
            // frm.ShowDialog();
            //调用内置方法 给数据源 新编辑，后面的话，直接load 可以不用给数据源的格式
            //string ReprotfileName = "SOB.frx";
            //List<tb_MaterialReturn> main = new List<tb_MaterialReturn>();
            //_EditEntity.tb_sales_order_details = details;
            //main.Add(_EditEntity);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            List<tb_MaterialReturn> rptSources = new List<tb_MaterialReturn>();
            tb_MaterialReturn saleOrder = EditEntity as tb_MaterialReturn;
            saleOrder.tb_MaterialReturnDetails = details;
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
            //List<tb_MaterialReturn> main = new List<tb_MaterialReturn>();
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


            if (EditEntity.tb_MaterialReturnDetails == null || EditEntity.tb_MaterialReturnDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return ae;
            }

            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_MaterialReturn oldobj = CloneHelper.DeepCloneObject<tb_MaterialReturn>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_MaterialReturn>(EditEntity, oldobj);
            };

            tb_MaterialReturnController<tb_MaterialReturn> ctr = Startup.GetFromFac<tb_MaterialReturnController<tb_MaterialReturn>>();
            List<tb_MaterialReturn> list = new List<tb_MaterialReturn>();
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
                MainForm.Instance.PrintInfoLog($"{EditEntity.BillNo}反审失败,请联系管理员！\r\n{rrs.ErrorMsg}", Color.Red);
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
            List<tb_MaterialReturn> EditEntitys = new List<tb_MaterialReturn>();
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_MaterialReturn> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_MaterialReturnController<tb_MaterialReturn> ctr = Startup.GetFromFac<tb_MaterialReturnController<tb_MaterialReturn>>();
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
                MainForm.Instance.PrintInfoLog($"{EditEntity.BillNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }



        /// <summary>
        /// 由领料单加载明细
        /// </summary>
        /// <param name="id"></param>
        private async void LoadChildItems(long? id)
        {


            tb_MaterialRequisitionController<tb_MaterialRequisition> ctr = Startup.GetFromFac<tb_MaterialRequisitionController<tb_MaterialRequisition>>();
            //var saleorder = bsa.Tag as tb_SaleOrder;

            var SourceBill = await MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>().Where(c => c.MR_ID == id)
          .Includes(a => a.tb_MaterialRequisitionDetails, b => b.tb_proddetail, c => c.tb_prod)
          .Includes(a => a.tb_manufacturingorder, b => b.tb_proddetail, c => c.tb_prod)
          .Includes(a => a.tb_manufacturingorder, b => b.tb_ManufacturingOrderDetails)
          .SingleAsync();
            //新增时才可以转单
            if (SourceBill != null)
            {
                
                tb_MaterialReturn entity = MainForm.Instance.mapper.Map<tb_MaterialReturn>(SourceBill);
                List<tb_MaterialReturnDetail> details = MainForm.Instance.mapper.Map<List<tb_MaterialReturnDetail>>(SourceBill.tb_MaterialRequisitionDetails);
                entity.ReturnDate = System.DateTime.Now;
                List<tb_MaterialReturnDetail> NewDetails = new List<tb_MaterialReturnDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_MaterialRequisitionDetail _SourceBillDetail = SourceBill.tb_MaterialRequisitionDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    ////TODO:
                    if (_SourceBillDetail.ReturnQty != _SourceBillDetail.ActualSentQty)
                    {
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
                entity.tb_MaterialReturnDetails = NewDetails;
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
                entity.Outgoing = SourceBill.Outgoing;
                entity.tb_materialrequisition = SourceBill;//退料得先知道是哪个领料单退的。审核时需要这些数据
                if (SourceBill.CustomerVendor_ID.HasValue)
                {
                    entity.CustomerVendor_ID = SourceBill.CustomerVendor_ID.Value;
                }

                //if (SourceBill.PreStartDate.HasValue)
                //{
                //    entity.DeliveryDate = SourceBill.DeliveryDate.Value;
                //}
                //else
                //{
                //    entity.RequirementDate = System.DateTime.Now.AddDays(10);//这是不是一个平均时间。将来可以根据数据优化？   
                //    entity.PlanDate = System.DateTime.Now;
                //}
                if (SourceBill.MR_ID > 0)
                {
                    entity.MR_ID = SourceBill.MR_ID;
                    entity.MaterialRequisitionNO = SourceBill.MaterialRequisitionNO;
                }
                BusinessHelper.Instance.InitEntity(entity);
                //编号已经生成，在新点开一个页面时，自动生成。
                if (EditEntity.BillNo.IsNotEmptyOrNull())
                {
                    entity.BillNo = EditEntity.BillNo;
                }


                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(entity, actionStatus);
            }
        }
       
    }
}
