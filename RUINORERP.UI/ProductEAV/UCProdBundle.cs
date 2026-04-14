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

using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using Netron.GraphLib;
using static System.Drawing.Html.CssLength;
using Krypton.Toolkit;


namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("套装组合", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料, BizType.套装组合)]
    public partial class UCProdBundle : BaseBillEditGeneric<tb_ProdBundle, tb_ProdBundleDetail>
    {
        public UCProdBundle()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加排除菜单列表 - 套装组合不需要结案功能
        /// </summary>
        public override void AddExcludeMenuList()
        {
            // 套装组合是基础资料，不需要结案/反结案功能
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.反结案);
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_ProdBundle);
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdBundle).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public override void BindData(tb_ProdBundle entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.BundleID > 0)
            {
                entity.PrimaryKeyID = entity.BundleID;
                entity.ActionStatus = ActionStatus.加载;
                
                // 显示审核状态
                if (entity.ApprovalStatus.HasValue)
                {
                    lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
                }
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (entity.tb_ProdBundleDetails != null && entity.tb_ProdBundleDetails.Count > 0)
                {
                    entity.tb_ProdBundleDetails.ForEach(c => c.BundleID = 0);
                    entity.tb_ProdBundleDetails.ForEach(c => c.BundleChildID = 0);
                }
            }


            DataBindingHelper.BindData4Cmb<tb_Unit>(entity, t => t.Unit_ID, c => c.UnitName, cmbUnit);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.BundleName, txtBundleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.TargetQty, txtTargetQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Notes, txtDescription, BindDataType4TextBox.Text, false);
            
            // 绑定数据状态和审核状态到标签
            DataBindingHelper.BindData4ControlByEnum<tb_ProdBundle>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProdBundle>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            
            if (entity.tb_ProdBundleDetails != null && entity.tb_ProdBundleDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProdBundleDetail>(grid1, sgd, entity.tb_ProdBundleDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdBundleDetail>(grid1, sgd, new List<tb_ProdBundleDetail>(), c => c.ProdDetailID);
            }
            
            // 初始化打印状态显示
            UpdatePrintStatusDisplay();

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null) return;
                
                // 更新打印状态显示
                UpdatePrintStatusDisplay();
                
                // 如果状态发生变化，刷新按钮状态
                if (s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.DataStatus) ||
                    s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.ApprovalStatus) ||
                    s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.ApprovalResults) ||
                    s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.PrintStatus))
                {
                    UpdateButtonStates();
                }
            };

            // 初始化按钮状态
            UpdateButtonStates();

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
            

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SGDefineColumnItem> listCols = sgh.GetGridColumns<ProductSharePart, tb_ProdBundleDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.BundleChildID);
            listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.BundleID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);

            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            //listCols.SetCol_ReadOnly<tb_ProdBundleDetail>(c => c.);




            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.SubtotalCostAmount);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_ProdBundleDetail>(c => c.Quantity);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBundleDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBundleDetail>(sgd, f => f.Standard_Price, t => t.SaleUnitPrice);

            //应该只提供一个结构
            List<tb_ProdBundleDetail> lines = new List<tb_ProdBundleDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //     .AndIF(true, w => w.CNName.Length > 0)
            //    // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //    .ToExpression();//注意 这一句 不能少
            //                    // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_ProdBundleDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdBundleDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            grid1.Enter += Grid1_Enter;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo,this);
        }

        /// <summary>
        /// 更新打印状态显示
        /// </summary>
        private void UpdatePrintStatusDisplay()
        {
            if (EditEntity == null) return;
            
            if (EditEntity.PrintStatus == 0)
            {
                lblPrintStatus.Text = "未打印";
            }
            else
            {
                lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
            }
        }

        /// <summary>
        /// 根据单据状态更新按钮可用性
        /// 参考销售订单的状态驱动按钮控制逻辑
        /// </summary>
        private void UpdateButtonStates()
        {
            if (EditEntity == null) return;
            
            var dataStatus = (DataStatus)EditEntity.DataStatus;
            var approvalStatus = (ApprovalStatus)(EditEntity.ApprovalStatus ?? 0);
            
            // 打印按钮：非草稿/新建状态且已审核通过才能打印
            bool canPrint = dataStatus != DataStatus.草稿 && 
                           dataStatus != DataStatus.新建 &&
                           approvalStatus == ApprovalStatus.审核通过;
            toolStripbtnPrint.Enabled = canPrint;
            
            // 删除按钮：草稿或新建状态可以删除
            bool canDelete = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
            if (toolStripbtnDelete != null)
            {
                toolStripbtnDelete.Enabled = canDelete;
            }
            
            // 注意：审核、反审核、修改等按钮由基类的权限系统自动控制
            // 这里只需要控制打印和删除按钮即可
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
            //if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            //{
            //    MessageBox.Show("请选择【所在库位】。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    cmbLocation_ID.Focus();
            //}
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
                List<tb_ProdBundleDetail> details = new List<tb_ProdBundleDetail>();
                
                foreach (var item in RowDetails)
                {
                    tb_ProdBundleDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdBundleDetail>(item);
                    bOM_SDetail.Quantity = 1;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_ProdBundleDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_ProdBundleDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProdBundleDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                //  EditEntity.MergeSourceTotalQty = details.Sum(c => c.Qty);



            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }

        }

        

        List<tb_ProdBundleDetail> details = new List<tb_ProdBundleDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtMergeSourceTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_ProdBundleDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProdBundleDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
               

                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 0)
                {
                    string message = GetDuplicateProductMessage(aa[0]);
                    System.Windows.Forms.MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                //EditEntity.MergeSourceTotalQty = details.Sum(c => c.Qty);
                EditEntity.tb_ProdBundleDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ProdBundleDetail>(details))
                {
                    return false;
                }


                ReturnMainSubResults<tb_ProdBundle> SaveResult = new ReturnMainSubResults<tb_ProdBundle>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BundleName}。");
                        
                        // 保存成功后刷新按钮状态
                        UpdateButtonStates();
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














        /// <summary>
        /// 重写删除方法，调用基类删除方法后清空图片控件
        /// </summary>
        protected override async Task<ReturnResults<tb_ProdBundle>> Delete()
        {
            if (EditEntity == null)
            {
                return new ReturnResults<tb_ProdBundle> { Succeeded = false, ErrorMsg = "没有要删除的数据" };
            }

            // 调用基类的删除方法
            ReturnResults<tb_ProdBundle> result = await base.Delete();
            
            return result;
        }

    }
}
