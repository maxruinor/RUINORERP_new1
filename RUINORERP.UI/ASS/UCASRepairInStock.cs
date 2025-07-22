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
using FluentValidation.Results;
using System.Linq.Expressions;
using Krypton.Toolkit;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SourceGrid;
using EnumsNET;
using RUINORERP.UI.PSI.PUR;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.SysConfig;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.PSI.SAL;


namespace RUINORERP.UI.ASS
{
    [MenuAttrAssemblyInfo("维修入库单", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.维修中心, BizType.维修入库单)]
    public partial class UCASRepairInStock : BaseBillEditGeneric<tb_AS_RepairInStock, tb_AS_RepairInStockDetail>, IPublicEntityObject
    {
        public UCASRepairInStock()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_AS_RepairInStock>()
            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_AS_RepairInStock);
        }


        protected override void LoadRelatedDataToDropDownItems()
        {
            if (base.EditEntity is tb_AS_RepairInStock RepairInStock)
            {
                if ( RepairInStock.RepairOrderID > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.维修工单;
                    rqp.billId = RepairInStock.RepairOrderID;
                    rqp.billNo = RepairInStock.RepairOrderNo;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{rqp.billNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(rqp.billId.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }
            }
            base.LoadRelatedDataToDropDownItems();
        }
        public override void BindData(tb_AS_RepairInStock entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            base.EditEntity = entity;
            if (entity != null)
            {
                if (entity.RepairInStockID > 0)
                {
                    entity.PrimaryKeyID = entity.RepairInStockID;
                    entity.ActionStatus = ActionStatus.加载;
                    //entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (string.IsNullOrEmpty(entity.RepairInStockNo))
                    {
                        entity.RepairInStockNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.维修工单);
                    }
                    entity.Employee_ID = AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    if (entity.tb_AS_RepairInStockDetails != null && entity.tb_AS_RepairInStockDetails.Count > 0)
                    {
                        entity.tb_AS_RepairInStockDetails.ForEach(c => c.RepairInStockID = 0);
                        entity.tb_AS_RepairInStockDetails.ForEach(c => c.RepairInStockDetailID = 0);
                    }
                }
            }

           

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            //==
            DataBindingHelper.BindData4TextBox<tb_AS_RepairInStock>(entity, t => t.RepairInStockNo, txtRepairInStockNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);


            DataBindingHelper.BindData4TextBox<tb_AS_RepairInStock>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty, false);


            DataBindingHelper.BindData4DataTime<tb_AS_RepairInStock>(entity, t => t.EntryDate, dtpEntryDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairInStock>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairInStock>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
         
            //==

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_AS_RepairInStock>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_AS_RepairInStock>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_AS_RepairInStockDetails != null && entity.tb_AS_RepairInStockDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_AS_RepairInStockDetail>(grid1, sgd, entity.tb_AS_RepairInStockDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_AS_RepairInStockDetail>(grid1, sgd, new List<tb_AS_RepairInStockDetail>(), c => c.ProdDetailID);
            }

            




            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                              .And(t => t.IsCustomer == true)
                              .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);


            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_AS_RepairInStock>(entity, v => v.RepairOrderNo, txtRepairOrderID, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaSO = Expressionable.Create<tb_AS_RepairOrder>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                             .And(t => t.isdeleted == false)
                             .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessorSO = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_RepairOrder).Name + "Processor");
            QueryFilter queryFilterSO = baseProcessorSO.GetQueryFilter();
            queryFilterSO.FilterLimitExpressions.Add(lambdaSO);

            ControlBindingHelper.ConfigureControlFilter<tb_AS_RepairInStock, tb_AS_RepairOrder>(entity, txtRepairOrderID, t => t.RepairOrderNo,
              f => f.RepairOrderNo, queryFilterSO, a => a.RepairOrderID, b => b.RepairOrderID, null, false);

            ToolBarEnabledControl(entity);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairInStock>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairInStock>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup_ID.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            if (ProjectGroup.tb_ProjectGroupAccountMappers != null && ProjectGroup.tb_ProjectGroupAccountMappers.Count > 0)
                            {
                                EditEntity.tb_projectgroup = ProjectGroup;
                            }
                        }
                    }
                }
               
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) &&  entity.RepairOrderID > 0 
                && s2.PropertyName == entity.GetPropertyName<tb_AS_RepairInStock>(c => c.RepairOrderID))
                {
                    RepairInStock(entity.RepairOrderID);
                }

                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            EditEntity.Employee_ID = cv.Employee_ID.Value;
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
            base.BindData(entity);
        }

        public void InitDataTocmbbox()
        {
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
        }

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_AS_RepairInStockDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
         并且，表格中可以编辑的需要查询的列是唯一能查到详情的

        SetDependencyObject<P, S, T>   P包含S字段包含T字段--》是有且包含
         */



        SourceGridDefine sgd = null;
    

        SourceGridHelper sgh = new SourceGridHelper();
    
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);
            LoadGrid1();
      
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        private void LoadGrid1()
        {
            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_AS_RepairInStockDetail>();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_AS_RepairInStockDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.RepairInStockID);
            listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.RepairInStockDetailID);
            listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.RepairOrderDetailID);
            listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.ProdDetailID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_AS_RepairInStockDetail>(c => c.TotalCostAmount, true);
                listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.TotalCostAmount);
            }*/
            listCols.SetCol_Summary<tb_AS_RepairInStockDetail>(c => c.Quantity);



            // listCols.SetCol_Formula<tb_AS_RepairInStockDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.TaxSubtotalAmount, d => d.UntaxedAmount);

            //应该是审核时要处理的逻辑暂时隐藏
            //将数量默认为已出库数量
            //listCols.SetCol_Formula<tb_AS_RepairInStockDetail>((a, b) => a.Quantity, c => c.TotalReturnedQty);
            //listCols.SetCol_NeverVisible<tb_AS_RepairInStockDetail>(c => c.TotalReturnedQty);

            //sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairInStockDetail>(sgd, f => f.Inv_Cost, t => t.MaterialCost);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairInStockDetail>(sgd, f => f.Standard_Price, t => t.MaterialCost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairInStockDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairInStockDetail>(sgd, f => f.Location_ID, t => t.Location_ID);



            //应该只提供一个结构
            List<tb_AS_RepairInStockDetail> lines = new List<tb_AS_RepairInStockDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;
            //    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            // .AndIF(true, w => w.CNName.Length > 0)
            //// .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //.ToExpression();//注意 这一句 不能少
            //                // StringBuilder sb = new StringBuilder();
            //    /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //    list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_AS_RepairInStockDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_AS_RepairInStockDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
        }
       

        private void Sgh_OnLoadMultiRowData(object rows, Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_AS_RepairInStockDetail> details = new List<tb_AS_RepairInStockDetail>();

                foreach (var item in RowDetails)
                {
                    tb_AS_RepairInStockDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_AS_RepairInStockDetail>(item);
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_AS_RepairInStockDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_AS_RepairInStockDetail> details = sgd.BindingSourceLines.DataSource as List<tb_AS_RepairInStockDetail>;
                if (details == null)
                {
                    return;
                }
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);


                //EditEntity.tb_AS_RepairInStockMaterialDetails = LastRefurbishedMaterials;
                //EditEntity.TotalMaterialAmount = EditEntity.tb_AS_RepairInStockMaterialDetails.Sum(c => c.UnitPrice * c.Quantity);
                //EditEntity.TotalAmount = EditEntity.tb_AS_RepairInStockMaterialDetails.Sum(c => c.UnitPrice * c.Quantity) + EditEntity.LaborCost;
                //EditEntity.TotalMaterialCost = EditEntity.tb_AS_RepairInStockMaterialDetails.Sum(c => c.Cost * c.Quantity);

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_AS_RepairInStockDetail> details = new List<tb_AS_RepairInStockDetail>();
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

            List<tb_AS_RepairInStockDetail> oldOjb = new List<tb_AS_RepairInStockDetail>(details.ToArray());

            List<tb_AS_RepairInStockDetail> detailentity = bindingSourceSub.DataSource as List<tb_AS_RepairInStockDetail>;
          

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();


                if (NeedValidated && detailentity.Sum(c => c.Quantity) == 0)
                {
                    MessageBox.Show("明细中，入库总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }


                if (NeedValidated)
                {

                    if (EditEntity.RepairOrderNo.Trim().Length == 0)
                    {
                        MessageBox.Show("编号由系统自动生成，如果不小心清除，请重新生成单据的编号。");
                        return false;
                    }

                    if (EditEntity.ProjectGroup_ID.HasValue && EditEntity.ProjectGroup_ID.Value <= 0)
                    {
                        EditEntity.ProjectGroup_ID = null;
                    }

               

                }


                //因为有一个特殊验证： RuleFor(tb_AS_RepairInStockDetail => tb_AS_RepairInStockDetail.Quantity).NotEqual(0).When(c => c.tb_AS_RepairInStock.RefundOnly == false).WithMessage("非仅退款时，退回数量不能为0为零。");
                foreach (var item in details)
                {
                    item.tb_as_repairinstock = EditEntity;
                }


                EditEntity.tb_AS_RepairInStockDetails = details;

                EditEntity.TotalQty = details.Sum(c => c.Quantity);


               
                
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_AS_RepairInStockDetail>(details))
                {
                    return false;
                }
            
                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }



                ReturnMainSubResults<tb_AS_RepairInStock> SaveResult = new ReturnMainSubResults<tb_AS_RepairInStock>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.RepairOrderNo}。");
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

        //protected override async Task<bool> Submit()
        //{
        //    if (EditEntity != null)
        //    {
        //        if (EditEntity.RepairStartDate == null)
        //        {
        //            //退货日期不能为空。
        //            System.Windows.Forms.MessageBox.Show("退货日期不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return false;
        //        }
        //        bool rs = await base.Submit();
        //        return rs;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        private void cmbCustomerVendor_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerVendor_ID.SelectedIndex > 0)
            {
                if (cmbCustomerVendor_ID.SelectedItem is tb_CustomerVendor cv)
                {
                    if (cv.Employee_ID.HasValue)
                    {
                        cmbEmployee_ID.SelectedValue = cv.Employee_ID.Value;
                    }
                }
            }
        }

        private async void RepairInStock(long? RepairOrderID)
        {
            //要加一个判断 值是否有变化
            //新增时才可以
            //转单
            ButtonSpecAny bsa = (txtRepairOrderID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var RepairOrder = bsa.Tag as tb_AS_RepairOrder;//这个tag值。赋值会比较当前方法晚，所以失效
            RepairOrder = await MainForm.Instance.AppContext.Db.Queryable<tb_AS_RepairOrder>().Where(c => c.RepairOrderID == RepairOrderID)
            .Includes(t => t.tb_AS_RepairOrderDetails, d => d.tb_proddetail)
          //  .Includes(t => t.tb_AS_RepairInStocks, a => a.tb_AS_RepairInStockDetails)
          //  .Includes(t => t.tb_as_aftersaleapply, a => a.tb_AS_AfterSaleApplyDetails)
            .SingleAsync();
            if (RepairOrder != null)
            {
                var ctr = Startup.GetFromFac<tb_AS_RepairInStockController<tb_AS_RepairInStock>>();
                tb_AS_RepairInStock RepairInStock = ctr.ToRepairInStock(RepairOrder);
                BindData(RepairInStock as tb_AS_RepairInStock);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("选取的对象为空！");
            }
        }

        /*
        /// <summary>
        /// 将销售订单转换为销售退货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadSaleOutBillData(long? ASApplyID)
        {
            //要加一个判断 值是否有变化
            //新增时才可以
            //转单
            ButtonSpecAny bsa = (txtASApplyID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var AfterSaleApply = bsa.Tag as tb_AS_AfterSaleApply;//这个tag值。赋值会比较当前方法晚，所以失效
            AfterSaleApply = await MainForm.Instance.AppContext.Db.Queryable<tb_AS_AfterSaleApply>().Where(c => c.ASApplyID == ASApplyID)
            .Includes(t => t.tb_AS_AfterSaleApplyDetails, d => d.tb_proddetail)
            .Includes(t => t.tb_AS_RepairInStocks, a => a.tb_AS_RepairInStockDetails)
            .SingleAsync();
            if (AfterSaleApply != null)
            {
                //如果这个销售出库单，已经有提交或审核过的。并且数量等于出库总数量则无法再次录入退回单。应该是不会显示出来了。
                if (AfterSaleApply.tb_AS_RepairInStocks.Sum(c => c.TotalQty) == AfterSaleApply.TotalConfirmedQuantity)
                {
                    MainForm.Instance.uclog.AddLog("当前售后申请单已经全部维修回，无法再次维修。");

                    return;
                }

                ASApplyID = AfterSaleApply.ASApplyID;

                tb_AS_RepairInStock entity = MainForm.Instance.mapper.Map<tb_AS_RepairInStock>(AfterSaleApply);
                List<tb_AS_RepairInStockDetail> details = MainForm.Instance.mapper.Map<List<tb_AS_RepairInStockDetail>>(AfterSaleApply.tb_AS_AfterSaleApplyDetails);
                List<tb_AS_RepairInStockDetail> NewDetails = new List<tb_AS_RepairInStockDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_AS_AfterSaleApplyDetail item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"当前行的SKU:{item.tb_proddetail.SKU}已维修数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    }
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"售后申请单:{entity.ASApplyNo}已全部维修处理，请检查是否正在重复操作！");
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


                entity.tb_AS_RepairInStockDetails = NewDetails;
                entity.RepairStartDate = System.DateTime.Now;

                dtpPreDeliveryDate.Checked = true;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                if (entity.ASApplyID.HasValue && entity.ASApplyID > 0)
                {
                    entity.CustomerVendor_ID = AfterSaleApply.CustomerVendor_ID;
                    entity.ASApplyNo = AfterSaleApply.ASApplyNo;
                    entity.ASApplyID = AfterSaleApply.ASApplyID;
                }
                BusinessHelper.Instance.InitEntity(entity);
                BindData(entity as tb_AS_RepairInStock);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("选取的对象为空！");
            }
        }

        */
    }
}
