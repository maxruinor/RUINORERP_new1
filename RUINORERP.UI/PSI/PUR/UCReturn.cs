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
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("返厂单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.返厂出库)]
    public partial class UCReturn : BaseBillEditGeneric<tb_PurEntry, tb_PurEntryQueryDto>
    {
        public UCReturn()
        {
            InitializeComponent();
            base.OnBindDataToUIEvent += UCStockIn_OnBindDataToUIEvent;
        }
        private void UCStockIn_OnBindDataToUIEvent(tb_PurEntry entity)
        {
            BindData(entity as tb_PurEntry);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);

        }

        public void BindData(tb_PurEntry entity)
        {
            if (entity == null)
            {
                MainForm.Instance.uclog.AddLog("实体不能为空", UILogType.警告);
                return;
            }
            EditEntity = entity;
            if (entity.PEID > 0)
            {
                entity.PrimaryKeyID = entity.PEID;
                entity.actionStatus = ActionStatus.加载;
                entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.actionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.新建;
                entity.PurEntryNo = BizCodeGenerationHelper.GetBizBillNo(BizType.采购入库单);
                // entity.DeliveryDate = System.DateTime.Now;
            }
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.PurEntryNo, txtPurEntryNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ActualAmount.ToString(), txtActualAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_PurEntry>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CehckBox<tb_PurEntry>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.DiscountAmount.ToString(), txtDiscountAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CehckBox<tb_PurEntry>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
            DataBindingHelper.BindData4CehckBox<tb_PurEntry>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4TextBox<tb_PurEntry>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text, false);

            txtstatus.Text = ((DataStatus)entity.DataStatus).ToString();
            txtstatus.Enabled = false;
            if (entity.tb_PurEntryDetails != null && entity.tb_PurEntryDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_PurEntryDetail>(grid1, sgd, entity.tb_PurEntryDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_PurEntryDetail>(grid1, sgd, new List<tb_PurEntryDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if (true)
                {
                    entity.actionStatus = ActionStatus.修改;
                    base.ToolBarEnabledControl(MenuItemEnums.修改);
                }
            };
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController dc = Startup.GetFromFac<View_ProdDetailController>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); ;
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            ///显示列表对应的中文
            //ConcurrentQueue<KeyValuePair<string, PropertyInfo>> Ddc = EmitHelper.GetfieldNameList(combinedType);

            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            List<SourceGridDefineColumnItem> cols1 = SourceGridDefine.GetSourceGridDefineColumnItems<ProductSharePart>();
            //指定了关键字段ProdDetailID
            List<SourceGridDefineColumnItem> cols2 = SourceGridDefine.GetSourceGridDefineColumnItems<tb_PurEntryDetail>(sub => sub.ProdDetailID);
            listCols.AddRange(cols1);
            listCols.AddRange(cols2);

            listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.PEID);
            listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.PECID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            //具体审核权限的人才显示
            if (!AppContext.currentUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.SubtotalPirceAmount);
            }
            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_PurEntryDetail>(c => c.TotalAmount);

            sgd = new SourceGridDefine(grid1, listCols, true);

            //应该只提供一个结构
            bindingSourceSub.DataSource = new List<tb_PurEntryDetail>();
            sgd.BindingSourceLines = bindingSourceSub;

            StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.Query(sb.ToString());
            sgd.SetDependencyObject<View_ProdDetail, ProductSharePart, tb_PurEntryDetail>(list, sub => sub.ProdDetailID);
            foreach (var item in sgd.DependQuery.RelatedCols)
            {
                SourceGridDefineColumnItem col = listCols.Where(c => c.ColName == item.ColName).FirstOrDefault();
                if (col != null)
                {
                    col.IsDependQueryCol = true;
                }
            }
            //应该只提供一个结构
            List<tb_PurEntryDetail> lines = new List<tb_PurEntryDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_PurEntryDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
        }


        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {
            try
            {
                //给默认值的话，可以批量处理，按int类型这种？
                tb_PurEntryDetail rowObj = _rowObj as tb_PurEntryDetail;
                //计算值 
                if (!rowObj.UnitPrice.HasValue)
                {
                    rowObj.UnitPrice = 0;
                }
                //计算值 
                if (!rowObj.TransactionPrice.HasValue)
                {
                    rowObj.TransactionPrice = 0;
                }
                rowObj.TotalAmount = rowObj.Quantity.Value * rowObj.TransactionPrice.Value;

                //反算  比方通过已经总价 和数量 算出单价  盘点这时不需要这样处理
                //if (myGridDefine[position.Column].ColName == "DiffAmount")
                //{
                //    // rowObj.DiffQty = (rowObj.DiffAmount / rowObj.Cost);
                //}
                //else
                //{
                //    rowObj.DiffAmount = rowObj.DiffQty * rowObj.Cost;
                //}

                //sgd.BindingSourceLines

                //计算总金额  这些逻辑是不是放到业务层？后面要优化

                List<tb_PurEntryDetail> details = sgd.BindingSourceLines.DataSource as List<tb_PurEntryDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                EditEntity.TotalQty = details.Sum(c => c.Quantity.Value);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice.Value * c.Quantity.Value);
                //把值显示到UI
                #region 找到对应的UI上的列

                foreach (PropertyInfo field in typeof(tb_PurEntryDetail).GetProperties())
                {
                    SourceGridDefineColumnItem dcol = null;
                    foreach (Attribute attr in field.GetCustomAttributes(true))
                    {
                        if (attr is FKRelationAttribute)
                        {

                        }
                        if (attr is SugarColumn)
                        {
                            SugarColumn entityAttr = attr as SugarColumn;
                            if (null != entityAttr)
                            {
                                if (entityAttr.ColumnDescription == null)
                                {
                                    continue;
                                }
                                if (entityAttr.IsIdentity)
                                {
                                    continue;
                                }
                                if (entityAttr.IsPrimaryKey)
                                {
                                    continue;
                                }
                                if (entityAttr.ColumnDescription.Trim().Length > 0)
                                {
                                    dcol = new SourceGridDefineColumnItem();
                                    //dcol.ColName = field.Name;
                                    //dcol.ColCaption = entityAttr.ColumnDescription;
                                    dcol = myGridDefine.DefineColumns.FirstOrDefault(f => f.ColName == field.Name);
                                }

                            }
                        }

                    }

                    if (dcol != null)
                    {
                        myGridDefine.grid[position.Row, dcol.ColIndex].Value = ReflectionHelper.GetPropertyValue(rowObj, dcol.ColName);
                    }

                }
                #endregion


            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MainForm.Instance.uclog.AddLog(ex.Message);
            }
        }

        List<tb_PurEntryDetail> details = new List<tb_PurEntryDetail>();
        protected async override void Save()
        {
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_PurEntryDetail> detailentity = bindingSourceSub.DataSource as List<tb_PurEntryDetail>;
            if (EditEntity.actionStatus == ActionStatus.新增 || EditEntity.actionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID.HasValue).ToList();
                details = details.Where(t => t.ProdDetailID.Value > 0).ToList();
                //如果没有有效的明细。直接提示
                if (details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return;
                }
                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return;
                }

                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其它输入条码验证

                EditEntity.tb_PurEntryDetails = details;
                ReturnMainSubResults<tb_PurEntry> SaveResult = await base.Save(EditEntity);
                if (SaveResult.Succeeded)
                {
                    lblReview.Text = ((ApprovalStatus)EditEntity.ApprovalStatus).ToString();
                }
            }



        }



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

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_PurEntry oldobj = CloneHelper.DeepCloneObject<tb_PurEntry>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_PurEntry>(EditEntity, oldobj);
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

            // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            //因为只需要更新主表
            //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
            // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
            tb_PurEntryController<tb_PurEntry> ctr = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
            bool Succeeded = await ctr.AdjustingAsync(EditEntity, ae);
            if (Succeeded)
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
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！");
            }

            return ae;
        }


        protected override void Print()
        {

            FastReport.Report FReport;
            FReport = new FastReport.Report();
            FReport.RegisterData(details, "Main");
            String reportFile = "SOB.frx";
            RptPreviewForm frm = new RptPreviewForm();
            frm.ReprotfileName = reportFile;
            frm.MyReport = FReport;
            frm.ShowDialog();

        }

    

 
    }
}
