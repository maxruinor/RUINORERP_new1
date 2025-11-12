﻿using System;
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
using RUINORERP.UI.Network.Services;
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
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using Krypton.Toolkit;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.UI.PSI.SAL;
using EnumsNET;
using RUINORERP.UI.ToolForm;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Model.CommonModel;


namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("采购退货入库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购退货入库)]
    public partial class UCPurReturnEntry : BaseBillEditGeneric<tb_PurReturnEntry, tb_PurReturnEntryDetail>, IPublicEntityObject
    {
        public UCPurReturnEntry()
        {
            InitializeComponent();

            AddPublicEntityObject(typeof(ProductSharePart));
        }
        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_PurReturnEntry returnEntry)
            {
                if (returnEntry.PurEntryRe_ID.HasValue)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.采购退货单;
                    rqp.billId = returnEntry.PurEntryRe_ID.Value;
                    rqp.billNo = returnEntry.PurEntryReNo;
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
                //如果有出库，则查应收
                if (returnEntry.DataStatus >= (int)DataStatus.确认)
                {
                    var receivablePayables = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                    .Where(c => c.ARAPStatus >= (int)ARAPStatus.待审核
                                                                    && c.CustomerVendor_ID == returnEntry.CustomerVendor_ID
                                                                    && c.SourceBillId == returnEntry.PurReEntry_ID)
                                                                    .ToListAsync();
                    foreach (var item in receivablePayables)
                    {
                        var rqpara = new Model.CommonModel.RelatedQueryParameter();
                        rqpara.bizType = BizType.应付款单;
                        rqpara.billId = item.ARAPId;
                        ToolStripMenuItem RelatedMenuItemPara = new ToolStripMenuItem();
                        RelatedMenuItemPara.Name = $"{rqpara.billId}";
                        RelatedMenuItemPara.Tag = rqpara;
                        RelatedMenuItemPara.Text = $"{rqpara.bizType}:{item.ARAPNo}";
                        RelatedMenuItemPara.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.ARAPId.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItemPara);
                        }
                    }
                }

            }
            base.LoadRelatedDataToDropDownItemsAsync();
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_PurReturnEntry);
        }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            //DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            //DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);

            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);

        }
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_PurReturnEntry>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }
        public override void BindData(tb_PurReturnEntry entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }

            EditEntity = entity;
            if (entity.PurReEntry_ID > 0)
            {
                entity.PrimaryKeyID = entity.PurReEntry_ID;
                entity.ActionStatus = ActionStatus.加载;
                //entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                purEntryReid = string.Empty;
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;

                if (string.IsNullOrEmpty(entity.PurReEntryNo))
                {
                    entity.PurReEntryNo = ClientBizCodeService.GetBizBillNo(BizType.采购退货入库);
                }
                if (entity.tb_purentryre != null && entity.tb_purentryre.Currency_ID.HasValue)
                {
                    entity.Currency_ID = entity.tb_purentryre.Currency_ID.Value;
                }
                else
                {
                    if (AppContext.BaseCurrency != null)
                    {
                        entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                    }
                }
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_PurReturnEntryDetails != null && entity.tb_PurReturnEntryDetails.Count > 0)
                {
                    entity.tb_PurReturnEntryDetails.ForEach(c => c.PurReEntry_CID = 0);
                    entity.tb_PurReturnEntryDetails.ForEach(c => c.PurReEntry_ID = 0);
                }


            }


           

            //DataBindingHelper.BindData4CmbByEnum<tb_PurReturnEntry>(entity, k => k.ShippingWay, typeof(PurReProcessWay), cmbProcessWay, true);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.PurReEntryNo, txtPurReEntryNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4DataTime<tb_PurReturnEntry>(entity, t => t.BillDate, dtpBillDate, false);
            DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.is_force_offset, chkis_force_offset, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
            DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_PurReturnEntry>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_PurReturnEntry>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_PurReturnEntryDetails != null && entity.tb_PurReturnEntryDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_PurReturnEntryDetail>(grid1, sgd, entity.tb_PurReturnEntryDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_PurReturnEntryDetail>(grid1, sgd, new List<tb_PurReturnEntryDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
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

                //如果是采购入库引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.PurEntryRe_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_PurReturnEntry>(c => c.PurEntryRe_ID))
                {
                    await Task.Delay(200);//要延迟一下。不然tag中的值还没有赋值就会执行了
                    LoadRefBillData(entity.PurEntryRe_ID);
                }
                else
                {
                    // MainForm.Instance.PrintInfoLog(entity.ActionStatus.GetName());
                }

            };


            //绑定这个。数据联动
            DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, v => v.PurEntryReNo, txtPurEntryRe_ID, BindDataType4TextBox.Text, true);

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)
                            .ToExpression();//注意 这一句 不能少
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

                //创建表达式  草稿 结案 和没有提交的都不显示
                var lambdaRe = Expressionable.Create<tb_PurEntryRe>()
                                .And(t => t.DataStatus == (int)DataStatus.确认)
                                 .And(t => t.isdeleted == false)
                                .ToExpression();//注意 这一句 不能少
                BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_PurEntryRe).Name + "Processor");
                QueryFilter queryFilter = basePro.GetQueryFilter();
                queryFilter.FilterLimitExpressions.Add(lambdaRe);//意思是只有审核确认的。没有结案的。才能查询出来。

                //绑定这个。数据感叹号快捷查询
                ControlBindingHelper.ConfigureControlFilter<tb_PurReturnEntry, tb_PurEntryRe>(entity, txtPurEntryRe_ID, t => t.PurEntryReNo,
            f => f.PurEntryReNo, queryFilter, a => a.PurEntryRe_ID, b => b.PurEntryRe_ID, null, false);

                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_PurReturnEntryValidator>(), kryptonSplitContainer1.Panel1.Controls);
                //  base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            base.BindData(entity);
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_PurReturnEntryDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_PurReturnEntryDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_PurReturnEntryDetail>(c => c.PurReEntry_CID);
            listCols.SetCol_NeverVisible<tb_PurReturnEntryDetail>(c => c.PurReEntry_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);

            listCols.SetCol_Format<tb_PurReturnEntryDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_PurReturnEntryDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurReturnEntryDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurReturnEntryDetail>(c => c.SubtotalTrPriceAmount, CustomFormatType.CurrencyFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;



            listCols.SetCol_Summary<tb_PurReturnEntryDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_PurReturnEntryDetail>(c => c.TaxAmount);
            listCols.SetCol_Summary<tb_PurReturnEntryDetail>(c => c.SubtotalTrPriceAmount);


            listCols.SetCol_Formula<tb_PurReturnEntryDetail>((a, b, c) => a.UnitPrice * c.Quantity, c => c.SubtotalTrPriceAmount);
            listCols.SetCol_Formula<tb_PurReturnEntryDetail>((a, b, c) => a.SubtotalTrPriceAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurReturnEntryDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurReturnEntryDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurReturnEntryDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurReturnEntryDetail>(sgd, f => f.VendorModelCode, t => t.VendorModelCode);


            //应该只提供一个结构
            List<tb_PurReturnEntryDetail> lines = new List<tb_PurReturnEntryDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //       .AndIF(true, w => w.CNName.Length > 0)
            //      // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //      .ToExpression();//注意 这一句 不能少
            //                      // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_PurReturnEntryDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_PurReturnEntryDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
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
                List<tb_PurReturnEntryDetail> details = sgd.BindingSourceLines.DataSource as List<tb_PurReturnEntryDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => c.SubtotalTrPriceAmount);
                EditEntity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_PurReturnEntryDetail> details = new List<tb_PurReturnEntryDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_PurReturnEntryDetail> detailentity = bindingSourceSub.DataSource as List<tb_PurReturnEntryDetail>;
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

                EditEntity.tb_PurReturnEntryDetails = details;
                if (EditEntity.TotalTaxAmount > 0)
                {
                    EditEntity.IsIncludeTax = true;
                }
                else
                {
                    EditEntity.IsIncludeTax = false;
                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_PurReturnEntryDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证


                ReturnMainSubResults<tb_PurReturnEntry> SaveResult = new ReturnMainSubResults<tb_PurReturnEntry>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PurReEntryNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}，请重试;或联系管理员。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }

            return false;

        }


        string purEntryReid = string.Empty;

        /// <summary>
        /// 将采购退货单转换为采购退货入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task LoadRefBillData(long? saleoutid)
        {
            //要加一个判断 值是否有变化
            //新增时才可以

            ButtonSpecAny bsa = (txtPurEntryRe_ID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var purEntryRe = bsa.Tag as tb_PurEntryRe;
            if (purEntryRe != null)
            {
                //意思是不用重复加载
                if (!string.IsNullOrEmpty(purEntryReid) && purEntryReid.Equals(purEntryRe.PurEntryRe_ID.ToString()))
                {
                    return;
                }
                purEntryReid = purEntryRe.PurEntryRe_ID.ToString();

                tb_PurReturnEntry entity = MainForm.Instance.mapper.Map<tb_PurReturnEntry>(purEntryRe);
                List<tb_PurReturnEntryDetail> details = MainForm.Instance.mapper.Map<List<tb_PurReturnEntryDetail>>(purEntryRe.tb_PurEntryReDetails);

                List<tb_PurReturnEntryDetail> NewDetails = new List<tb_PurReturnEntryDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_PurEntryReDetail item = new tb_PurEntryReDetail();
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].PurEntryRe_CID > 0)
                    {
                        item = purEntryRe.tb_PurEntryReDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.PurEntryRe_CID == details[i].PurEntryRe_CID);
                    }
                    else
                    {
                        item = purEntryRe.tb_PurEntryReDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    }
                    #region 

                    details[i].Quantity = details[i].Quantity - item.DeliveredQuantity;// 减掉已经入的数量
                    details[i].SubtotalTrPriceAmount = details[i].UnitPrice * details[i].Quantity;

                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        item.tb_proddetail = await AppContext.Db.Queryable<tb_ProdDetail>().Where(c => c.ProdDetailID == item.ProdDetailID).SingleAsync();
                        tipsMsg.Add($"当前行的SKU:{item.tb_proddetail.SKU}已退回数量为{details[i].Quantity}，已全部交付。当前行数据将不会加载到明细！");
                    }
                    #endregion

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"采购退货单:{entity.PurEntryReNo}已全部入回仓库，请检查是否正在重复操作！");
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

                entity.tb_PurReturnEntryDetails = NewDetails;
                entity.TotalAmount = NewDetails.Sum(c => c.SubtotalTrPriceAmount);
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.TotalTaxAmount = NewDetails.Sum(c => c.TaxAmount);

                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;
                entity.BillDate = System.DateTime.Now;
                if (entity.PurEntryRe_ID > 0)
                {
                    entity.CustomerVendor_ID = purEntryRe.CustomerVendor_ID;
                    entity.DepartmentID = purEntryRe.DepartmentID;
                    entity.Paytype_ID = purEntryRe.Paytype_ID;
                }
                BusinessHelper.Instance.InitEntity(entity);
                BindData(entity as tb_PurReturnEntry);
            }

        }


    }
}
