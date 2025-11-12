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
using RUINORERP.UI.AdvancedUIModule;
using Krypton.Navigator;

using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

using Microsoft.Extensions.Logging;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using System.Diagnostics;
using Netron.GraphLib;
using LiveChartsCore.Geo;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.Network.Services;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.FM;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("借出单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.借出归还, BizType.借出单)]
    public partial class UCProdBorrowing : BaseBillEditGeneric<tb_ProdBorrowing, tb_ProdBorrowingDetail>, IPublicEntityObject
    {
        public UCProdBorrowing()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }

        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_ProdBorrowing borrowing)
            {
                if (borrowing.tb_ProdReturnings != null && borrowing.tb_ProdReturnings.Count > 0)
                {
                    foreach (var item in borrowing.tb_ProdReturnings)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.归还单;
                        rqp.billId = item.ReturnID;
                        rqp.billNo = item.ReturnNo;
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


                //查是否有损失费用单
                if (borrowing.DataStatus >= (int)DataStatus.确认)
                {
                    var ProfitLossList = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ProfitLoss>()
                    .Includes(a => a.tb_FM_ProfitLossDetails)
                    .Where(a => a.SourceBillId == borrowing.BorrowID)
                    .ToListAsync();
                    if (ProfitLossList != null && ProfitLossList.Count > 0)
                    {
                        foreach (var item in ProfitLossList)
                        {
                            var rqp = new Model.CommonModel.RelatedQueryParameter();
                            if (item.ProfitLossDirection == (int)ProfitLossDirection.损失)
                            {
                                rqp.bizType = BizType.损失确认单;
                            }
                            else
                            {
                                rqp.bizType = BizType.溢余确认单;
                            }

                            rqp.billId = item.ProfitLossId;
                            rqp.billNo = item.ProfitLossNo;
                            ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                            RelatedMenuItem.Name = $"{rqp.billId}";
                            RelatedMenuItem.Tag = rqp;
                            RelatedMenuItem.Text = $"{rqp.bizType}:{rqp.billNo}";
                            RelatedMenuItem.ToolTipText = $"{rqp.billNo}金额费用【{item.TotalAmount}】";
                            RelatedMenuItem.Click += base.MenuItem_Click;
                            if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(rqp.billId.ToString()))
                            {
                                toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                            }
                        }
                    }
                }

            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }

        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_ProdBorrowing, actionStatus);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            //DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);


        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdBorrowing).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        //private tb_ProdBorrowing _EditEntity;
        //public tb_ProdBorrowing EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(tb_ProdBorrowing entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.BorrowID > 0)
            {
                entity.PrimaryKeyID = entity.BorrowID;
                entity.ActionStatus = ActionStatus.加载;
                //  entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (string.IsNullOrEmpty(entity.BorrowNo))
                {
                    entity.BorrowNo = ClientBizCodeService.GetBizBillNo(BizType.借出单);
                }

                entity.DueDate = System.DateTime.Now.AddDays(30);//最长时间为30天
                entity.Out_date = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_ProdBorrowingDetails != null && entity.tb_ProdBorrowingDetails.Count > 0)
                {
                    entity.tb_ProdBorrowingDetails.ForEach(c => c.BorrowID = 0);
                    entity.tb_ProdBorrowingDetails.ForEach(c => c.BorrowDetaill_ID = 0);
                }
                entity.Created_at = System.DateTime.Now;
                BusinessHelper.Instance.InitEntity(entity);
            }

            //销售不能借给供应商东西
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                chkIsVendor.Visible = false;
            }
            else
            {
                chkIsVendor.Visible = true;
            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.BorrowNo, txtBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_ProdBorrowing>(entity, t => t.DueDate, dtpBill_Date, true);
            DataBindingHelper.BindData4DataTime<tb_ProdBorrowing>(entity, t => t.Out_date, dtpOut_date, true);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, t => t.Reason, txtReason, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_ProdBorrowing>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProdBorrowing>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            DataBindingHelper.BindData4CheckBox<tb_ProdBorrowing>(entity, t => t.IsVendor, chkIsVendor, false);

            if (entity.IsVendor)
            {
                InitLoadSupplierData(entity);
            }
            else
            {
                InitLoadCustomerData(entity);
            }

            if (entity.tb_ProdBorrowingDetails != null && entity.tb_ProdBorrowingDetails.Count > 0)
            {
                // details = entity.tb_ProdBorrowingDetails;
                sgh.LoadItemDataToGrid<tb_ProdBorrowingDetail>(grid1, sgd, entity.tb_ProdBorrowingDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdBorrowingDetail>(grid1, sgd, new List<tb_ProdBorrowingDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_ProdBorrowing>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProdBorrowingValidator>(), kryptonPanelMainInfo.Controls);
                    if (s2.PropertyName == entity.GetPropertyName<tb_ProdBorrowing>(c => c.IsVendor))
                    {
                        if (chkIsVendor.Checked)
                        {
                            InitLoadSupplierData(entity);
                        }
                        else
                        {
                            InitLoadCustomerData(entity);
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




            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProdBorrowingValidator>(), kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }


        private void InitLoadSupplierData(tb_ProdBorrowing entity)
        {

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterSupplier = baseProcessor.GetQueryFilter();

            //这里外层来实现对客户供应商的限制
            string IsVendor = "".ToFieldName<tb_CustomerVendor>(c => c.IsVendor);
            //应收付款中的往来单位额外添加一些条件
            var lambdaIsVendor = Expressionable.Create<tb_CustomerVendor>()
                .And(t => t.IsVendor == true)
                .And(t => t.isdeleted == false)
                .And(t => t.Is_enabled == true)
              .ToExpression();
            // QueryField queryFieldVendor = queryFilterSupplier.QueryFields.Where(c => c.FieldName == IsVendor).FirstOrDefault();
            queryFilterSupplier.FilterLimitExpressions.Clear();
            queryFilterSupplier.SetFieldLimitCondition(lambdaIsVendor);

            //这时要特殊处理一下这个 因为是相同的控件上 改变条件。InitFilterForControlByExp这个里面跳过了已经添加过的 叹号
            cmbCustomerVendor_ID.ButtonSpecs.Clear();

            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(EditEntity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterSupplier.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(EditEntity, cmbCustomerVendor_ID, c => c.CVName, queryFilterSupplier);
        }
        private void InitLoadCustomerData(tb_ProdBorrowing entity)
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterCustomer = baseProcessor.GetQueryFilter();

            string IsCustomer = "".ToFieldName<tb_CustomerVendor>(c => c.IsCustomer);
            //应收付款中的往来单位额外添加一些条件
            var lambdaIsCustomer = Expressionable.Create<tb_CustomerVendor>()
                .And(t => t.IsCustomer == true)
                .And(t => t.isdeleted == false)
                .And(t => t.Is_enabled == true)
              .ToExpression();
            // QueryField queryFieldCustomer = queryFilterCustomer.QueryFields.Where(c => c.FieldName == IsCustomer).FirstOrDefault();
            queryFilterCustomer.FilterLimitExpressions.Clear();
            queryFilterCustomer.SetFieldLimitCondition(lambdaIsCustomer);

            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterCustomer.GetFilterExpression<tb_CustomerVendor>(), true);

            //这时要特殊处理一下这个 因为是相同的控件上 改变条件。InitFilterForControlByExp这个里面跳过了已经添加过的 叹号
            cmbCustomerVendor_ID.ButtonSpecs.Clear();

            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterCustomer);


        }



        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UCStockOut_Load(object sender, System.EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_ProdBorrowingDetail>(c => c.ProdDetailID, false);
            listCols.SetCol_NeverVisible<tb_ProdBorrowingDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_ProdBorrowingDetail>(c => c.BorrowDetaill_ID);
            listCols.SetCol_NeverVisible<tb_ProdBorrowingDetail>(c => c.BorrowID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            listCols.SetCol_ReadOnly<tb_ProdBorrowingDetail>(c => c.ReQty);
            if (!AppContext.IsSuperUser)
            {
                listCols.SetCol_ReadOnly<tb_ProdBorrowingDetail>(c => c.ReQty);
            }

            //具体审核权限的人才显示
            /*
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_ProdBorrowingDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_ProdBorrowingDetail>(c => c.SubtotalCostAmount);
                listCols.SetCol_NeverVisible<tb_ProdBorrowingDetail>(c => c.SubtotalPirceAmount);
            }
            */



            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_ProdBorrowingDetail>(c => c.Qty);
            listCols.SetCol_Formula<tb_ProdBorrowingDetail>((a, b) => a.Cost * b.Qty, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_ProdBorrowingDetail>((a, b) => a.Price * b.Qty, c => c.SubtotalPirceAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBorrowingDetail>(sgd, f => f.Location_ID, t => t.Location_ID);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBorrowingDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBorrowingDetail>(sgd, f => f.Standard_Price, t => t.Price);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBorrowingDetail>(sgd, f => f.prop, t => t.property);




            //应该只提供一个结构
            List<tb_ProdBorrowingDetail> lines = new List<tb_ProdBorrowingDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //   .AndIF(true, w => w.CNName.Length > 0)
            //  // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //  .ToExpression();//注意 这一句 不能少
            //                  // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = await dc.BaseQueryByWhereAsync(exp);
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_ProdBorrowingDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdBorrowingDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sw.Stop();
            MainForm.Instance.uclog.AddLog("加载数据耗时：" + sw.ElapsedMilliseconds + "毫秒");
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
                List<tb_ProdBorrowingDetail> details = new List<tb_ProdBorrowingDetail>();

                foreach (var item in RowDetails)
                {
                    tb_ProdBorrowingDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdBorrowingDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_ProdBorrowingDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }
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
                List<tb_ProdBorrowingDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProdBorrowingDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Qty);
                EditEntity.TotalAmount = details.Sum(c => c.Price * c.Qty);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_ProdBorrowingDetail> details = new List<tb_ProdBorrowingDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_ProdBorrowingDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProdBorrowingDetail>;
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
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Qty);
                EditEntity.TotalAmount = details.Sum(c => c.Price * c.Qty);
                if (NeedValidated && EditEntity.TotalQty == 0)
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量不能为零!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                EditEntity.tb_ProdBorrowingDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ProdBorrowingDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_ProdBorrowing> SaveResult = new ReturnMainSubResults<tb_ProdBorrowing>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BorrowNo}。");
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


    }
}
