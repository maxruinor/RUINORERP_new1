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
using Netron.GraphLib;
using Krypton.Toolkit;


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("产品组合单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.产品分割与组合, BizType.产品组合单)]
    public partial class UCProdMerge : BaseBillEditGeneric<tb_ProdMerge, tb_ProdMerge>
    {
        public UCProdMerge()
        {
            InitializeComponent();

        }

        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_ProdMerge, actionStatus);
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdMerge).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public async override void BindData(tb_ProdMerge entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.MergeID > 0)
            {
                entity.PrimaryKeyID = entity.MergeID;
                entity.ActionStatus = ActionStatus.加载;
                if (entity.ProdDetailID > 0)
                {
                    txtProdDetailID.Text = entity.SKU.ToString();
                }
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
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (string.IsNullOrEmpty(entity.MergeNo))
                {
                    entity.MergeNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.产品组合单);
                }
                entity.MergeDate = System.DateTime.Now;
                cmbBOM_ID.DataSource = null;
                if (entity.tb_ProdMergeDetails != null && entity.tb_ProdMergeDetails.Count > 0)
                {
                    entity.tb_ProdMergeDetails.ForEach(c => c.MergeID = 0);
                    entity.tb_ProdMergeDetails.ForEach(c => c.MergeSub_ID = 0);
                }
            }


            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, t => t.MergeTargetQty, txtMergeTargetQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, t => t.MergeSourceTotalQty, txtMergeSourceTotalQty, BindDataType4TextBox.Qty, false);

            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbRelated<tb_BOM_S>(entity, k => k.BOM_ID, v => v.BOM_Name, cmbBOM_ID, false, false);


            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, t => t.MergeNo, txtMergeNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_ProdMerge>(entity, t => t.MergeDate, dtpMergeDate, false);
            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_ProdMerge>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProdMerge>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));



            if (entity.tb_ProdMergeDetails != null && entity.tb_ProdMergeDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProdMergeDetail>(grid1, sgd, entity.tb_ProdMergeDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdMergeDetail>(grid1, sgd, new List<tb_ProdMergeDetail>(), c => c.ProdDetailID);
            }

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ProdMerge>(entity, k => k.SKU, txtProdDetailID, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ProdMerge>(entity, v => v.ProdDetailID, txtProdDetailID, true);
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProdMergeValidator>(), kryptonSplitContainer1.Panel1.Controls);

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
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                    //影响明细的数量
                    if (s2.PropertyName == entity.GetPropertyName<tb_ProdMerge>(c => c.MergeTargetQty))
                    {
                        if (EditEntity.BOM_ID > 0 && EditEntity.tb_ProdMergeDetails.Count > 0)
                        {
                            decimal bomOutQty = EditEntity.tb_bom_s.OutputQty;
                            for (int i = 0; i < EditEntity.tb_ProdMergeDetails.Count; i++)
                            {
                                tb_BOM_SDetail bOM_SDetail = EditEntity.tb_bom_s.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == EditEntity.tb_ProdMergeDetails[i].ProdDetailID);
                                if (bOM_SDetail != null)
                                {
                                    EditEntity.tb_ProdMergeDetails[i].Qty = (bOM_SDetail.UsedQty * EditEntity.MergeTargetQty.ToDecimal() / bomOutQty).ToInt();
                                }
                            }
                            //同步到明细UI表格中？
                            sgh.SynchronizeUpdateCellValue<tb_ProdMergeDetail>(sgd, c => c.Qty, EditEntity.tb_ProdMergeDetails);
                        }
                    }



                    if (entity.ProdDetailID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ProdMerge>(c => c.ProdDetailID))
                    {

                        BaseController<View_ProdDetail> ctrProdDetail = Startup.GetFromFacByName<BaseController<View_ProdDetail>>(typeof(View_ProdDetail).Name + "Controller");

                        var vpprod = await ctrProdDetail.BaseQueryByIdAsync(entity.ProdDetailID);
                        //txtSpecifications.Text = string.Empty;
                        //txtType.Text = string.Empty;
                        //txtproperty.Text = string.Empty;
                        txtSpecifications.Text = vpprod.Specifications;
                        entity.property = vpprod.prop;
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
                            cmbBOM_ID.SelectedValue = vpprod.BOM_ID.Value;
                            LoadItemsFromBOM();
                        }
                        else
                        {
                            MessageBox.Show("没有找到对应的BOM\r\n请确认选择的产品有对应BOM配方。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                    }

                    //
                    if (entity.BOM_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ProdMerge>(c => c.BOM_ID))
                    {

                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_ProdMerge>(c => c.Location_ID))
                    {
                        if (EditEntity.Location_ID > 0)
                        {

                            //明细仓库优先来自于主表，可以手动修改。
                            listCols.SetCol_DefaultValue<tb_ProdMergeDetail>(c => c.Location_ID, EditEntity.Location_ID);
                            if (entity.tb_ProdMergeDetails != null)
                            {
                                entity.tb_ProdMergeDetails.ForEach(c => c.Location_ID = EditEntity.Location_ID);
                                sgh.SetCellValue<tb_ProdMergeDetail>(sgd, colNameExp => colNameExp.Location_ID, EditEntity.Location_ID);
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

        private void Bsa_Click(object sender, EventArgs e)
        {
            SelectLocationTips();
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


            listCols = sgh.GetGridColumns<ProductSharePart, tb_ProdMergeDetail, InventoryInfo>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_ProdMergeDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_ProdMergeDetail>(c => c.MergeSub_ID);
            listCols.SetCol_NeverVisible<tb_ProdMergeDetail>(c => c.MergeID);
            listCols.SetCol_NeverVisible<tb_ProdMergeDetail>(c => c.UnitCost);

            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            //listCols.SetCol_ReadOnly<tb_ProdMergeDetail>(c => c.Qty);




            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_ProdMergeDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_ProdMergeDetail>(c => c.SubtotalCostAmount);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_ProdMergeDetail>(c => c.Qty);
            listCols.SetCol_Summary<ProductSharePart>(c => c.Inv_Cost.Value);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdMergeDetail>(sgd, f => f.Location_ID, t => t.Location_ID);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdMergeDetail>(sgd, f => f.prop, t => t.property);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdMergeDetail>(sgd, f => f.Inv_Cost, t => t.UnitCost);

            //应该只提供一个结构
            List<tb_ProdMergeDetail> lines = new List<tb_ProdMergeDetail>();
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
            sgd.SetDependencyObject<ProductSharePart, tb_ProdMergeDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdMergeDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            grid1.Enter += Grid1_Enter;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
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
                List<tb_ProdMergeDetail> details = new List<tb_ProdMergeDetail>();

                foreach (var item in RowDetails)
                {
                    tb_ProdMergeDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdMergeDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_ProdMergeDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_ProdMergeDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProdMergeDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.MergeSourceTotalQty = details.Sum(c => c.Qty);



            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }

        }


        List<tb_ProdMergeDetail> details = new List<tb_ProdMergeDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtMergeSourceTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_ProdMergeDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProdMergeDetail>;
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
                EditEntity.MergeSourceTotalQty = details.Sum(c => c.Qty);
                EditEntity.tb_ProdMergeDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ProdMergeDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.MergeSourceTotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_ProdMerge> SaveResult = new ReturnMainSubResults<tb_ProdMerge>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.MergeNo}。");
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
        tb_ProdMerge oldobj = CloneHelper.DeepCloneObject<tb_ProdMerge>(EditEntity);
        command.UndoOperation = delegate ()
        {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_ProdMerge>(EditEntity, oldobj);
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
        tb_ProdMergeController<tb_ProdMerge> ctr = Startup.GetFromFac<tb_ProdMergeController<tb_ProdMerge>>();
        List<tb_ProdMerge> _StockIns = new List<tb_ProdMerge>();
        _StockIns.Add(EditEntity);
        ReturnResults<bool> rs = await ctr.AdjustingAsync(_StockIns, ae);
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
                tb_ProdMerge oldobj = CloneHelper.DeepCloneObject<tb_ProdMerge>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<tb_ProdMerge>(EditEntity, oldobj);
                };
                ApprovalEntity ae = await base.Review();

                // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                //因为只需要更新主表
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
                tb_ProdMergeController<tb_ProdMerge> ctr = Startup.GetFromFac<tb_ProdMergeController<tb_ProdMerge>>();
                List<tb_ProdMerge> tb_ProdMerges = new List<tb_ProdMerge>();
                tb_ProdMerges.Add(EditEntity);

                ReturnResults<bool> rs = await ctr.AntiApprovalAsync(tb_ProdMerges);
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
        /// <summary>
        /// 从BOM中加载明细，注意单据新增加时，明细是空的，才加载
        /// </summary>
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
                        MessageBox.Show("请选择要组合的配方");
                    }
                }
                return;
            }
            if (EditEntity.tb_ProdMergeDetails == null)
            {
                EditEntity.tb_ProdMergeDetails = new List<tb_ProdMergeDetail>();
            }
            //新建时才全新加载
            if (EditEntity.tb_ProdMergeDetails.Count > 0)
            {
                return;
            }
            EditEntity.tb_ProdMergeDetails.Clear();
            //通过BOM_id找到明细加载
            List<tb_BOM_SDetail> RowDetails = new List<tb_BOM_SDetail>();
            if (cmbBOM_ID.SelectedValue != null && cmbBOM_ID.SelectedValue.ToString() != "-1")
            {
                if (EditEntity.Location_ID == -1)
                {
                    MessageBox.Show("请选择【所在库位】。");
                    EditEntity.tb_ProdMergeDetails = new List<tb_ProdMergeDetail>();
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
                        List<tb_ProdMergeDetail> details = new List<tb_ProdMergeDetail>();

                        foreach (var item in RowDetails)
                        {
                            tb_ProdMergeDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdMergeDetail>(item);
                            bOM_SDetail.Location_ID = EditEntity.Location_ID;
                            bOM_SDetail.Qty = (RowDetails.FirstOrDefault(c => c.ProdDetailID == bOM_SDetail.ProdDetailID
                            ).UsedQty * EditEntity.MergeTargetQty).ToInt();
                            details.Add(bOM_SDetail);
                        }
                        EditEntity.tb_ProdMergeDetails = details;
                    }
                }
            }
            sgh.LoadItemDataToGrid<tb_ProdMergeDetail>(grid1, sgd, EditEntity.tb_ProdMergeDetails, c => c.ProdDetailID);
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

        private void txtProdDetailID_Enter(object sender, EventArgs e)
        {
            SelectLocationTips();
        }

        private void cmbBOM_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItemsFromBOM();
        }

        private void chkNoBom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoBom.Checked)
            {
                EditEntity.BOM_ID = null;
            }
            else
            {
                EditEntity.BOM_ID = -1;
            }
        }
    }
}
