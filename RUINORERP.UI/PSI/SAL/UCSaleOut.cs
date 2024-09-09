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
using RUINORERP.Model.QueryDto;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using SqlSugar;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using System.Linq.Expressions;
using Krypton.Toolkit;
using RUINORERP.Business.Processor;
using FastReport.Fonts;
using RUINORERP.UI.PSI.PUR;

namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售出库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.销售管理, BizType.销售出库单)]
    public partial class UCSaleOut : BaseBillEditGeneric<tb_SaleOut, tb_SaleOutQueryDto>
    {
        public UCSaleOut()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOut>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority);
            base.OnBindDataToUIEvent += UcSaleOrderEdit_OnBindDataToUIEvent;
            base.toolStripButton结案.Visible = true;
        }

        private void UcSaleOrderEdit_OnBindDataToUIEvent(tb_SaleOut entity)
        {
            BindData(entity);

        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_SaleOut);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOut).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }


        public void BindData(tb_SaleOut entity)
        {
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.SaleOut_MainID > 0)
                {
                    entity.PrimaryKeyID = entity.SaleOut_MainID;
                    entity.ActionStatus = ActionStatus.加载;
                    //entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    entity.OutDate = System.DateTime.Now;
                    entity.SaleOutNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售出库单);
                    if (entity.tb_SaleOutDetails != null && entity.tb_SaleOutDetails.Count > 0)
                    {
                        entity.tb_SaleOutDetails.ForEach(c => c.SaleOut_MainID = 0);
                        entity.tb_SaleOutDetails.ForEach(c => c.SaleOutDetail_ID = 0);
                    }

                }


            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);

            //DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v => v.SOrderNo, cmbOrder_ID);


            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.IsFromPlatform, chk平台单, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.SaleOutNo, txtSaleOutNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.OutDate, dtpOutDate, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.CollectedMoney.ToString(), txtCollectedMoney, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            // DataBindingHelper.BindData4CehckBox<tb_SaleOut>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalTaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalUntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.DiscountAmount.ToString(), txtDiscountAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PrePayMoney.ToString(), txtPrePayMoney, BindDataType4TextBox.Money, false);
            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOut>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOut>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_SaleOutDetails != null && entity.tb_SaleOutDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_SaleOutDetails);
                sgh.LoadItemDataToGrid<tb_SaleOutDetail>(grid1, sgd, entity.tb_SaleOutDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_SaleOutDetail>());
                sgh.LoadItemDataToGrid<tb_SaleOutDetail>(grid1, sgd, new List<tb_SaleOutDetail>(), c => c.ProdDetailID);
            }

            //缓存一下结果下次如果一样，就忽略？
            //object tempcopy = entity.Clone();
            //如果属性变化 则状态为修改

            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.SOrder_ID.HasValue && entity.SOrder_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.SOrder_ID))
                {
                    await OrderToOutBill(entity.SOrder_ID.Value);
                    MainForm.Instance.PrintInfoLog("一次");
                }


                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    entity.ActionStatus = ActionStatus.修改;
                    base.ToolBarEnabledControl(MenuItemEnums.修改);
                }



                if (entity.CustomerVendor_ID.HasValue && entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.CustomerVendor_ID))
                {
                    var obj = CacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            EditEntity.Employee_ID = cv.Employee_ID;
                        }
                    }
                }





                //显示 打印状态 如果是草稿状态 不显示打印
                if ((DataStatus)entity.DataStatus != DataStatus.草稿)
                {
                    toolStripbtnPrint.Enabled = true;
                    if (entity.PrintStatus == 0)
                    {
                        lblPrintStatus.Text = "未打印";
                    }
                    else
                    {
                        lblPrintStatus.Text = $"打印{entity.PrintStatus}次";
                    }

                }
                else
                {
                    toolStripbtnPrint.Enabled = false;
                }

            };


            tb_CustomerVendorController<tb_CustomerVendor> cvctr = Startup.GetFromFac<tb_CustomerVendorController<tb_CustomerVendor>>();
            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                            .ToExpression();//注意 这一句 不能少

            // base.InitFilterForControl<tb_CustomerVendor, tb_CustomerVendorQueryDto>(entity, cmbCustomerVendor_ID, c => c.CVName, lambda, cvctr.GetQueryParameters());
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, v => v.SaleOrderNo, txtSaleOrder, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_SaleOut>(entity, v => v.SOrder_ID, txtSaleOrder, true);

            //tb_SaleOrderController<tb_SaleOrder> ctrsaleorder = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

            //创建表达式  草稿 结案 和没有提交的都不显示
            //var lambdaOrder = Expressionable.Create<tb_SaleOrder>()
            //                .And(t => t.DataStatus == (int)DataStatus.确认)
            //                 .And(t => t.isdeleted == false)
            //                .ToExpression();//注意 这一句 不能少
            //base.InitFilterForControl<tb_SaleOrder, tb_SaleOrderQueryDto>(entity, txtSaleOrder, c => c.SOrderNo, lambdaOrder, ctrsaleorder.GetQueryParameters());

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_SaleOrder>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
             .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
             .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);

            DataBindingHelper.InitFilterForControlByExp<tb_SaleOrder>(entity, txtSaleOrder, c => c.SOrderNo, queryFilter);
            ToolBarEnabledControl(entity);
        }

        public void InitDataTocmbbox()
        {
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
        }

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_SaleOutDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
         并且，表格中可以编辑的需要查询的列是唯一能查到详情的

        SetDependencyObject<P, S, T>   P包含S字段包含T字段--》是有且包含
         */



        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_SaleOutDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_SaleOutDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_SaleOutDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_SaleOutDetail>(c => c.SaleOutDetail_ID);
            listCols.SetCol_NeverVisible<tb_SaleOutDetail>(c => c.ProdDetailID);
            ControlChildColumnsInvisible(listCols);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.TotalCostAmount, true);
            }
            */

            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.UnitPrice * b.Discount, c => c.TransactionPrice);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.Cost * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.SubtotalTaxAmount, d => d.SubtotalUntaxedAmount);
            //将数量默认为已出库数量  这个逻辑不对这个是订单累计 的出库数量只能是在出库审核时才累计数据，这里最多只读
            //listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.Quantity, c => c.TotalDeliveredQty);

            listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.CommissionAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.prop, t => t.property);




            //应该只提供一个结构
            List<tb_SaleOutDetail> lines = new List<tb_SaleOutDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                 .AndIF(true, w => w.CNName.Length > 0)
                // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
                .ToExpression();//注意 这一句 不能少
                                // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);
            sgd.SetDependencyObject<ProductSharePart, tb_SaleOutDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_SaleOutDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
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
                List<tb_SaleOutDetail> details = sgd.BindingSourceLines.DataSource as List<tb_SaleOutDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);

                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalUntaxedAmount = details.Sum(c => c.SubtotalUntaxedAmount);
                EditEntity.CollectedMoney = EditEntity.TotalAmount;
                EditEntity.TotalUntaxedAmount = EditEntity.TotalUntaxedAmount + EditEntity.ShipCost;
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_SaleOutDetail> details = new List<tb_SaleOutDetail>();
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
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);

            bindingSourceSub.EndEdit();

            List<tb_SaleOutDetail> oldOjb = new List<tb_SaleOutDetail>(details.ToArray());

            List<tb_SaleOutDetail> detailentity = bindingSourceSub.DataSource as List<tb_SaleOutDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();


                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 1)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                EditEntity.tb_SaleOutDetails = details;
                if (EditEntity.tb_SaleOutDetails == null || EditEntity.tb_SaleOutDetails.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                    return false;
                }


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_SaleOutDetail>(details))
                {
                    return false;
                }

                //二选中，验证机制还没有弄好。先这里处理
                if (EditEntity.CustomerVendor_ID == 0 || EditEntity.CustomerVendor_ID == -1)
                {
                    EditEntity.CustomerVendor_ID = null;
                }

                if (EditEntity.Employee_ID == 0 || EditEntity.Employee_ID == -1)
                {
                    EditEntity.Employee_ID = null;
                }
                ////计算总金额
                //decimal? totalMoney = details.Sum(r => r.Quantity * r.TransactionPrice);
                //EditEntity.TotalAmount = totalMoney.Value;
                //EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;


                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (NeedValidated)
                {
                    if (EditEntity.tb_SaleOutRes != null && EditEntity.tb_SaleOutRes.Count > 0)
                    {
                        MessageBox.Show("当前销售出库单已有销售出库退回数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                ReturnMainSubResults<tb_SaleOut> SaveResult = new ReturnMainSubResults<tb_SaleOut>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SaleOutNo}。");
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


            //CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            //if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            //{
            List<tb_SaleOut> EditEntitys = new List<tb_SaleOut>();
            // EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOut> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
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
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }
            return true;
            //}
            //else
            //{
            //    return false;
            //}

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
                        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。");
                        return null;
                    }
                }
            }
            if (EditEntity.tb_SaleOutDetails == null || EditEntity.tb_SaleOutDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return null;
            }
            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOut oldobj = CloneHelper.DeepCloneObject<tb_SaleOut>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOut>(EditEntity, oldobj);
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
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
       
            ReturnResults<tb_SaleOut> rrs = await ctr.ApprovalAsync(EditEntity);
            if (rrs.Succeeded)
            {
                ////销售出库 如果数据来自销售订单。数量相等也一样 在销售订单中 结案。
                //if (EditEntity.tb_saleorder != null && EditEntity.TotalQty == EditEntity.tb_saleorder.TotalQty)
                //{
                //    EditEntity.tb_saleorder.DataStatus = (int)DataStatus.完结;
                //    await AppContext.Db.Updateable(EditEntity.tb_saleorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                //}

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
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
            }

            return ae;
        }
        */
        /*
        /// <summary>
        /// 列表中不再实现反审，批量，出库反审情况极少。并且是仔细处理
        /// </summary>
        protected async override void ReReview()
        {
            if (EditEntity == null)
            {
                return;
            }

            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                return;
            }


            if (EditEntity.tb_SaleOutDetails == null || EditEntity.tb_SaleOutDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return;
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOut oldobj = CloneHelper.DeepCloneObject<tb_SaleOut>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOut>(EditEntity, oldobj);
            };

            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            ReturnResults<bool> rs = await ctr.AntiApprovalAsync(EditEntity);
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
                MainForm.Instance.PrintInfoLog($"销售出库单{EditEntity.SaleOutNo}反审失败,{rs.ErrorMsg},请联系管理员！", Color.Red);
            }

        }
        */


        string saleorderid = string.Empty;

        private async Task<tb_SaleOut> OrderToOutBill(long _sorderid)
        {
            tb_SaleOrder saleorder;
            ButtonSpecAny bsa = txtSaleOrder.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return null;
            }
            //saleorder = bsa.Tag as tb_SaleOrder;

            saleorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
            .Where(c => c.SOrder_ID == _sorderid)
            .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
            .SingleAsync();


            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            tb_SaleOut entity = mapper.Map<tb_SaleOut>(saleorder);


            entity.DataStatus = (int)DataStatus.草稿;
            entity.ApprovalStatus = (int)ApprovalStatus.未审核;
            entity.ApprovalResults = null;
            entity.ApprovalOpinions = "";
            entity.Approver_at = null;
            entity.Approver_by = null;
            entity.PrintStatus = 0;
            entity.ActionStatus = ActionStatus.新增;
            if (saleorder.DeliveryDate.HasValue)
            {
                entity.OutDate = saleorder.DeliveryDate.Value;
                entity.DeliveryDate = saleorder.DeliveryDate;
            }
            else
            {
                entity.OutDate = System.DateTime.Now;
                entity.DeliveryDate = System.DateTime.Now;
            }

            if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
            {
                entity.CustomerVendor_ID = saleorder.CustomerVendor_ID;
                entity.SaleOrderNo = saleorder.SOrderNo;
                entity.PlatformOrderNo = saleorder.PlatformOrderNo;
                entity.IsFromPlatform = saleorder.IsFromPlatform;
            }

            List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
            List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();
            List<string> tipsMsg = new List<string>();
            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (aa.Count > 0 && details[i].SaleOrderDetail_ID > 0)
                {
                    #region 产品ID可能大于1行，共用料号情况
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                    details[i].Quantity = item.Quantity - item.TotalDeliveredQty;// 已经出数量去掉
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"销售订单{saleorder.SOrderNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已出库数为{item.TotalDeliveredQty}，可出库数为{details[i].Quantity}，当前行数据忽略！");
                    }

                    #endregion
                }
                else
                {
                    #region 每行产品ID唯一
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = details[i].Quantity - item.TotalDeliveredQty;// 减掉已经出库的数量
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    details[i].SubtotalCostAmount = details[i].Cost * details[i].Quantity;

                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    }
                    #endregion
                }

            }

            if (NewDetails.Count == 0)
            {
                tipsMsg.Add($"订单:{entity.SaleOrderNo}已全部出库，请检查是否正在重复出库！");
            }

            entity.tb_SaleOutDetails = NewDetails;
            entity.TotalAmount = details.Sum(c => c.SubtotalTransAmount);

            if (saleorder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) != entity.TotalAmount)
            {
                tipsMsg.Add($"当前引用订单:{entity.SaleOrderNo}与当前出库明细累计金额不同，请注意检查！");
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
            BusinessHelper.Instance.InitEntity(entity);
            BindData(entity as tb_SaleOut);
            return entity;
        }

    }
}
