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
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using FastReport.Table;
using Org.BouncyCastle.Asn1.Cmp;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;




namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("产品分割单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.产品分割与组合, BizType.产品分割单)]
    public partial class UCProdSplit : BaseBillEditGeneric<tb_ProdSplit, tb_ProdSplit>
    {
        public UCProdSplit()
        {
            InitializeComponent();
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_ProdSplit);
        }

        protected override void Cancel()
        {
            base.Cancel();
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            cmbBOM_ID.DataSource = null;
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

        }
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdSplit).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public async override void BindData(tb_ProdSplit entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.SplitID > 0)
            {
                entity.PrimaryKeyID = entity.SplitID;
                entity.ActionStatus = ActionStatus.加载;
                if (entity.ProdDetailID > 0)
                {
                    txtProdDetailID.Text = entity.SKU.ToString();
                }
                // entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色

                /*加载临时数据*/
                #region
                BaseController<View_ProdDetail> ctrProdDetail = Startup.GetFromFacByName<BaseController<View_ProdDetail>>(typeof(View_ProdDetail).Name + "Controller");
                var vpprod = await ctrProdDetail.BaseQueryByIdAsync(entity.ProdDetailID);

                txtSpecifications.Text = vpprod.Specifications;
                entity.property = vpprod.prop;
                txtBOM_Name.Text = vpprod.CNName;
                entity.SKU = vpprod.SKU;
                txtType.Text = UIHelper.ShowGridColumnsNameValue<tb_Prod>(nameof(vpprod.Type_ID), vpprod.Type_ID);

                BindingSource bs = new BindingSource();
                List<tb_BOM_S> tlist = new List<tb_BOM_S>();

                tlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>().Where(m => m.ProdDetailID == entity.ProdDetailID)
                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                        .ToListAsync();
                Expression<Func<tb_BOM_S, long>> keyValue = p => p.BOM_ID;
                Expression<Func<tb_BOM_S, string>> NameValue = p => p.BOM_Name;

                string key = keyValue.GetMemberInfo().Name;
                string keyname = NameValue.GetMemberInfo().Name;

                InsertSelectItem<tb_BOM_S>(key, keyname, tlist);
                bs.DataSource = tlist;
                ComboBoxHelper.InitDropList(bs, cmbBOM_ID, key, keyname, ComboBoxStyle.DropDown, true);




                #endregion
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (string.IsNullOrEmpty(entity.SplitNo))
                {
                    entity.SplitNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.产品分割单);
                }
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                entity.SplitDate = System.DateTime.Now;
                if (entity.tb_ProdSplitDetails != null && entity.tb_ProdSplitDetails.Count > 0)
                {
                    entity.tb_ProdSplitDetails.ForEach(c => c.SplitID = 0);
                    entity.tb_ProdSplitDetails.ForEach(c => c.SplitSub_ID = 0);
                }
            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SplitNo, txtSplitNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_ProdSplit>(entity, t => t.SplitDate, dtpSplitDate, false);
            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SplitParentQty, txtSplitParentQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SplitChildTotalQty, txtSplitChildTotalQty, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);



            DataBindingHelper.BindData4CmbRelated<tb_BOM_S>(entity, k => k.BOM_ID, v => v.BOM_Name, cmbBOM_ID, false, false);

            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4ControlByEnum<tb_ProdSplit>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_ProdSplit>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProdSplit>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_ProdSplitDetails != null && entity.tb_ProdSplitDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProdSplitDetail>(grid1, sgd, entity.tb_ProdSplitDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdSplitDetail>(grid1, sgd, new List<tb_ProdSplitDetail>(), c => c.ProdDetailID);
            }

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, k => k.SKU, txtProdDetailID, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ProdSplit>(entity, v => v.ProdDetailID, txtProdDetailID, true);
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProdSplitValidator>(), kryptonSplitContainer1.Panel1.Controls);

                //创建表达式  草稿 结案 和没有提交的都不显示
                var lambdaOrder = Expressionable.Create<View_ProdDetail>()
                                // .And(t => t.DataStatus == (int)DataStatus.确认)
                                .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_ProdDetail).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                ///视图指定成实体表，为了显示关联数据
                DataBindingHelper.InitFilterForControlByExp<View_ProdDetail>(entity, txtProdDetailID, c => c.SKU, queryFilterC, typeof(tb_Prod));
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //影响子件的数量
                if (s2.PropertyName == entity.GetPropertyName<tb_ProdSplit>(c => c.SplitParentQty))
                {
                    if (EditEntity.BOM_ID > 0 && EditEntity.tb_ProdSplitDetails != null && EditEntity.tb_ProdSplitDetails.Count > 0)
                    {
                        decimal bomOutQty = EditEntity.tb_bom_s.OutputQty;
                        for (int i = 0; i < EditEntity.tb_ProdSplitDetails.Count; i++)
                        {
                            tb_BOM_SDetail bOM_SDetail = EditEntity.tb_bom_s.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == EditEntity.tb_ProdSplitDetails[i].ProdDetailID);
                            if (bOM_SDetail != null)
                            {
                                EditEntity.tb_ProdSplitDetails[i].Qty = (bOM_SDetail.UsedQty * EditEntity.SplitParentQty.ToDecimal() / bomOutQty).ToInt();
                            }
                        }

                        //同步到明细UI表格中？
                        sgh.SynchronizeUpdateCellValue<tb_ProdSplitDetail>(sgd, c => c.Qty, EditEntity.tb_ProdSplitDetails);
                    }
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_ProdMerge>(c => c.Location_ID))
                {
                    if (EditEntity.Location_ID > 0)
                    {
                        //明细仓库优先来自于主表，可以手动修改。
                        listCols.SetCol_DefaultValue<tb_ProdSplitDetail>(c => c.Location_ID, EditEntity.Location_ID);
                        if (entity.tb_ProdSplitDetails != null)
                        {
                            entity.tb_ProdSplitDetails.ForEach(c => c.Location_ID = EditEntity.Location_ID);
                            sgh.SetCellValue<tb_ProdSplitDetail>(sgd, colNameExp => colNameExp.Location_ID, EditEntity.Location_ID);
                        }
                    }
                }
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                    if (entity.ProdDetailID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ProdSplit>(c => c.ProdDetailID))
                    {

                        BaseController<View_ProdDetail> ctrProdDetail = Startup.GetFromFacByName<BaseController<View_ProdDetail>>(typeof(View_ProdDetail).Name + "Controller");
                        var vpprod = await ctrProdDetail.BaseQueryByIdAsync(entity.ProdDetailID);

                        txtSpecifications.Text = vpprod.Specifications;
                        entity.property = vpprod.prop;
                        txtBOM_Name.Text = vpprod.CNName;
                        entity.SKU = vpprod.SKU;
                        txtType.Text = UIHelper.ShowGridColumnsNameValue<tb_Prod>(nameof(vpprod.Type_ID), vpprod.Type_ID);

                        BindingSource bs = new BindingSource();
                        List<tb_BOM_S> tlist = new List<tb_BOM_S>();

                        tlist = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>().Where(m => m.ProdDetailID == entity.ProdDetailID)
                                .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                                .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                .ToListAsync();
                        Expression<Func<tb_BOM_S, long>> keyValue = p => p.BOM_ID;
                        Expression<Func<tb_BOM_S, string>> NameValue = p => p.BOM_Name;

                        string key = keyValue.GetMemberInfo().Name;
                        string keyname = NameValue.GetMemberInfo().Name;

                        InsertSelectItem<tb_BOM_S>(key, keyname, tlist);
                        bs.DataSource = tlist;
                        ComboBoxHelper.InitDropList(bs, cmbBOM_ID, key, keyname, ComboBoxStyle.DropDown, true);

                        // 不管选什么都先清空
                        txtBOM_Name.Text = string.Empty;
                        txtBOM_No.Text = string.Empty;

                        if (vpprod.BOM_ID.HasValue && vpprod.BOM_ID > 0)
                        {
                            //给一个默认
                            cmbBOM_ID.SelectedValue = vpprod.BOM_ID;
                            LoadItemsFromBOM();
                        }
                        else
                        {
                            if (tlist.Count == 1)
                            {
                                MessageBox.Show("没有找到对应的BOM\r\n请确认选择的产品有对应BOM配方。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                //给一个默认
                                cmbBOM_ID.SelectedIndex = 0;
                                cmbBOM_ID.SelectedIndexChanged -= CmbBOM_ID_SelectedIndexChanged;
                                cmbBOM_ID.SelectedIndexChanged += CmbBOM_ID_SelectedIndexChanged;
                            }

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

        private void CmbBOM_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItemsFromBOM();
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UCStockIn_Load(object sender, EventArgs e)
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = sgh.GetGridColumns<ProductSharePart, tb_ProdSplitDetail, InventoryInfo>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_ProdSplitDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_ProdSplitDetail>(c => c.SplitSub_ID);
            listCols.SetCol_NeverVisible<tb_ProdSplitDetail>(c => c.SplitID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            //listCols.SetCol_ReadOnly<tb_ProdSplitDetail>(c => c.Qty);




            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_ProdSplitDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_ProdSplitDetail>(c => c.SubtotalCostAmount);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_ProdSplitDetail>(c => c.Qty);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdSplitDetail>(sgd, f => f.Location_ID, t => t.Location_ID);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdSplitDetail>(sgd, f => f.prop, t => t.property);



            //应该只提供一个结构
            List<tb_ProdSplitDetail> lines = new List<tb_ProdSplitDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //     .AndIF(true, w => w.CNName.Length > 0)
            //    // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //    .ToExpression();//注意 这一句 不能少
            //                    // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_ProdSplitDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdSplitDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            grid1.Enter += Grid1_Enter;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo,this);
        }

        private void Grid1_Enter(object sender, EventArgs e)
        {
            SelectLocationTips();
        }

        private void SelectLocationTips()
        {
            if (EditEntity == null)
            {
                return;
            }
            if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            {
                MessageBox.Show("请选择【所在库位】。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLocation_ID.Focus();
            }
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
                List<tb_ProdSplitDetail> details = new List<tb_ProdSplitDetail>();
                
                foreach (var item in RowDetails)
                {
                    tb_ProdSplitDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdSplitDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_ProdSplitDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_ProdSplitDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProdSplitDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.SplitChildTotalQty = details.Sum(c => c.Qty);

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }

        }


        List<tb_ProdSplitDetail> details = new List<tb_ProdSplitDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtSplitChildTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_ProdSplitDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProdSplitDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                EditEntity.SplitChildTotalQty = details.Sum(c => c.Qty);
                EditEntity.tb_ProdSplitDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                //二选中，验证机制还没有弄好。先这里处理
                //if (EditEntity.CustomerVendor_ID == 0 || EditEntity.CustomerVendor_ID == -1)
                //{
                //    EditEntity.CustomerVendor_ID = null;
                //}

                if (EditEntity.Employee_ID == 0 || EditEntity.Employee_ID == -1)
                {
                    EditEntity.Employee_ID = null;
                }
                //如果明细包含主表中的母件时。不允许保存
                if (NeedValidated && details.Any(c => c.ProdDetailID == EditEntity.ProdDetailID))
                {
                    System.Windows.Forms.MessageBox.Show("明细中，不能有和母件相同的数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }


                if (NeedValidated && !base.Validator<tb_ProdSplitDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.SplitChildTotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

                ReturnMainSubResults<tb_ProdSplit> SaveResult = new ReturnMainSubResults<tb_ProdSplit>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SplitNo}。");
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

    RevertCommand command = new RevertCommand();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_ProdSplit oldobj = CloneHelper.DeepCloneObject<tb_ProdSplit>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_ProdSplit>(EditEntity, oldobj);
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
    tb_ProdSplitController<tb_ProdSplit> ctr = Startup.GetFromFac<tb_ProdSplitController<tb_ProdSplit>>();

    ReturnResults<tb_ProdSplit> rs = await ctr.ApprovalAsync(EditEntity);
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
        base.ToolBarEnabledControl(MenuItemEnums.审核);
        //如果审核结果为不通过时，审核不是灰色。
        if (!ae.ApprovalResults)
        {
            toolStripbtnReview.Enabled = true;
        }
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核成功。");
    }
    else
    {
        //审核失败 要恢复之前的值
        command.Undo();
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rs.ErrorMsg},请联系管理员！", Color.Red);
    }

    return ae;
    }


    protected async override void ReReview()
    {
    if (EditEntity == null)
    {
        return;
    }
    //如果已经审核通过，则不能重复审核
    if (EditEntity.ApprovalStatus.HasValue)
    {
        if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
        {
            if (EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
            {
                // MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。");
                RevertCommand command = new RevertCommand();
                //缓存当前编辑的对象。如果撤销就回原来的值
                tb_ProdSplit oldobj = CloneHelper.DeepCloneObject<tb_ProdSplit>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<tb_ProdSplit>(EditEntity, oldobj);
                };
                ApprovalEntity ae = await base.Review();

                // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                //因为只需要更新主表
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
                tb_ProdSplitController<tb_ProdSplit> ctr = Startup.GetFromFac<tb_ProdSplitController<tb_ProdSplit>>();
                List<tb_ProdSplit> tb_ProdSplits = new List<tb_ProdSplit>();
                tb_ProdSplits.Add(EditEntity);

                ReturnResults<bool> rs = await ctr.AntiApprovalAsync(tb_ProdSplits);
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
                    //如果审核结果为不通过时，审核不是灰色。
                    if (!ae.ApprovalResults)
                    {
                        toolStripbtnReview.Enabled = true;
                    }
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}反审成功。");
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}反审失败{rs.ErrorMsg},请联系管理员！", Color.Red);
                }

            }
        }
    }



    }
    */
        private void LoadItemsFromBOM()
        {
            if (EditEntity == null)
            {
                return;
            }
            if (cmbBOM_ID.SelectedValue == null || cmbBOM_ID.SelectedValue.ToString() == "-1")
            {
                if (cmbBOM_ID.Items != null)
                {
                    if (cmbBOM_ID.Items.Count > 2)//有一个是请选择
                    {
                        MessageBox.Show("请选择要拆分的配方");
                    }
                }

                //清空一下明细？
                EditEntity.tb_ProdSplitDetails.Clear();
                sgh.LoadItemDataToGrid<tb_ProdSplitDetail>(grid1, sgd, new List<tb_ProdSplitDetail>(), c => c.ProdDetailID);
                return;
            }
            if (EditEntity.tb_ProdSplitDetails == null)
            {
                EditEntity.tb_ProdSplitDetails = new List<tb_ProdSplitDetail>();
            }
            //新建时才全新加载
            if (EditEntity.tb_ProdSplitDetails.Count > 0)
            {
                if (EditEntity.DataStatus != (int)DataStatus.新建 && EditEntity.tb_ProdSplitDetails.Count > 0)
                {
                    return;
                }

            }
            EditEntity.tb_ProdSplitDetails.Clear();
            //通过BOM_id找到明细加载
            List<tb_BOM_SDetail> RowDetails = new List<tb_BOM_SDetail>();
            if (cmbBOM_ID.SelectedValue.ToString() != "-1")
            {
                if (EditEntity.Location_ID == -1)
                {
                    MessageBox.Show("请选择【所在库位】。");
                    EditEntity.tb_ProdSplitDetails = new List<tb_ProdSplitDetail>();
                    return;
                }

                if (cmbBOM_ID.SelectedItem is tb_BOM_S bOM_S)
                {
                    //将这个赋值给这里。当分割数据变更时，可以同步计算。单向的。父级变了。子级变。反之不用变化。
                    EditEntity.tb_bom_s = bOM_S;

                    txtBOM_No.Text = bOM_S.BOM_No;
                    EditEntity.BOM_No = bOM_S.BOM_No;
                    EditEntity.BOM_ID = bOM_S.BOM_ID;
                    txtBOM_Name.Text = bOM_S.BOM_Name;
                    RowDetails = bOM_S.tb_BOM_SDetails;
                    if (RowDetails != null)
                    {
                        List<tb_ProdSplitDetail> details = new List<tb_ProdSplitDetail>();
                        
                        decimal bomOutQty = EditEntity.tb_bom_s.OutputQty;
                        foreach (var item in RowDetails)
                        {
                            tb_ProdSplitDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdSplitDetail>(item);
                            bOM_SDetail.Location_ID = EditEntity.Location_ID;
                            bOM_SDetail.Qty = (RowDetails.FirstOrDefault(c => c.ProdDetailID == bOM_SDetail.ProdDetailID
                           
                            ).UsedQty * EditEntity.SplitParentQty / bomOutQty).ToInt();
                            View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(item.ProdDetailID);
                            if (obj != null)
                            {
                                bOM_SDetail.property = obj.prop;
                            }
                            details.Add(bOM_SDetail);
                        }
                        EditEntity.tb_ProdSplitDetails = details;
                    }
                }
            }
            sgh.LoadItemDataToGrid<tb_ProdSplitDetail>(grid1, sgd, EditEntity.tb_ProdSplitDetails, c => c.ProdDetailID);
        }

        private void txtProdDetailID_Enter(object sender, EventArgs e)
        {
            SelectLocationTips();
        }

        private void cmbLocation_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EditEntity != null)
            {
                if (EditEntity.ActionStatus == ActionStatus.加载)
                {
                    return;
                }
            }
            //请选择
            if (cmbLocation_ID.SelectedValue is tb_Location location)
            {
                if (location.Location_ID == -1)
                {
                    EditEntity.Location_ID = 0;
                    return;
                }
                else
                {

                }
            }

            //会多次执行。所以加载放到了属性变化对应字段时操作。但是这里做一个验证
            if (cmbLocation_ID.SelectedValue != null && cmbLocation_ID.SelectedValue.ToLong() != -1)
            {
                EditEntity.Location_ID = cmbLocation_ID.SelectedValue.ToLong();
            }
            else
            {
                EditEntity.Location_ID = 0;
            }
        }


    }
}
