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
using MathNet.Numerics.LinearAlgebra.Factorization;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("包装信息", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料, BizType.包装信息)]
    public partial class UCPacking : BaseBillEditGeneric<tb_Packing, tb_PackingDetail>
    {
        public UCPacking()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Packing>(typeof(Priority), e => e.Priority, cmbOrderPriority, false);

            toolStripButton结案.Visible = false;
            toolStripBtnReverseReview.Visible = false;
            toolStripbtnReview.Visible = false;
            toolStripbtnSubmit.Visible = false;
            txtproperty.Enabled = false;
            txtSKU.Enabled = false;
        }



        internal override void LoadDataToUI(object Entity)
        {

            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_Packing, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Packing).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(tb_Packing entityPara, ActionStatus actionStatus)
        {

            tb_Packing entity = entityPara as tb_Packing;
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.Pack_ID > 0)
                {
                    entity.PrimaryKeyID = entity.Pack_ID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    entity.Modified_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    entity.Modified_at = System.DateTime.Now;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    entity.Created_at = System.DateTime.Now;
                    if (entity.BundleID.HasValue || entity.ProdBaseID.HasValue || entity.ProdDetailID.HasValue)
                    {
                        toolStripbtnAdd.Enabled = false;
                    }
                    entity.Is_enabled = true;
                    if (entity.ProdBaseID.HasValue || entity.ProdDetailID.HasValue)
                    {
                        toolStripbtnAdd.Enabled = false;
                        if (entity.Unit_ID == 0)
                        {
                            entity.Unit_ID = entity.tb_prod.Unit_ID;
                        }
                        
                    }
                    if (entity.tb_PackingDetails != null && entity.tb_PackingDetails.Count > 0)
                    {
                        entity.tb_PackingDetails.ForEach(c => c.Pack_ID = 0);
                        entity.tb_PackingDetails.ForEach(c => c.PackDetail_ID = 0);
                    }
                    if (entity.tb_BoxRuleses != null && entity.tb_BoxRuleses.Count > 0)
                    {
                        entity.tb_BoxRuleses.ForEach(c => c.Pack_ID = 0);
                        entity.tb_BoxRuleses.ForEach(c => c.BoxRules_ID = 0);
                    }
                }
            }


            EditEntity = entity;

            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.PackagingName, txtPackagingName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Unit>(entity, t => t.Unit_ID, v => v.UnitName, cmbUnit_ID);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.BoxMaterial, txtBoxMaterial, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.GrossWeight.ToString(), txtGrossWeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.NetWeight.ToString(), txtNetWeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_Packing>(entity, t => t.Is_enabled, chkIs_enabled, false);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;


            if (entity.tb_PackingDetails != null && entity.tb_PackingDetails.Count > 0)
            {
                sgh1.LoadItemDataToGrid<tb_PackingDetail>(grid1, sgd1, entity.tb_PackingDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh1.LoadItemDataToGrid<tb_PackingDetail>(grid1, sgd1, new List<tb_PackingDetail>(), c => c.ProdDetailID);
            }

            if (entity.tb_BoxRuleses != null && entity.tb_BoxRuleses.Count > 0)
            {
                sgh2.LoadItemDataToGrid<tb_BoxRules>(grid2, sgd2, entity.tb_BoxRuleses, c => c.Pack_ID);
            }
            else
            {
                sgh2.LoadItemDataToGrid<tb_BoxRules>(grid2, sgd2, new List<tb_BoxRules>(), c => c.Pack_ID);
            }


            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改))
                {


                    if (s2.PropertyName == entity.GetPropertyName<tb_Packing>(c => c.Length) ||
                      s2.PropertyName == entity.GetPropertyName<tb_Packing>(c => c.Width) ||
                      s2.PropertyName == entity.GetPropertyName<tb_Packing>(c => c.Height))
                    {
                        entity.Volume = entity.Length * entity.Width * entity.Height;
                    }


                    if (s2.PropertyName == entity.GetPropertyName<tb_Packing>(c => c.Pack_ID))
                    {
                        if (EditEntity.Pack_ID > 0)
                        {

                            //包装ID来自于主表
                            listCols1.SetCol_DefaultValue<tb_BoxRules>(c => c.Pack_ID, EditEntity.Pack_ID);
                        }
                    }


                }
                if (s2.PropertyName == entity.GetPropertyName<tb_Packing>(c => c.Pack_ID))
                {
                    //  entity.PackagingName = $"{prodDetail.CNName}中{prodDetail.SKU}的包装情况";
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_Packing>(c => c.ProdDetailID))
                {

                    if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改))
                    {
                        //加载关联数据
                        if (cmbProdDetailID.ButtonSpecs.Count > 0 && cmbProdDetailID.ButtonSpecs[0].Tag != null)
                        {
                            if (cmbProdDetailID.ButtonSpecs[0].Tag is View_ProdDetail vp)
                            {
                                entity.property = vp.prop;
                                entity.SKU = vp.SKU;
                            }
                        }
                        else
                        {
                            BaseController<View_ProdDetail> ctrProdDetail = Startup.GetFromFacByName<BaseController<View_ProdDetail>>(typeof(View_ProdDetail).Name + "Controller");
                            if (entity.ProdDetailID.HasValue)
                            {
                                var prodDetail = await ctrProdDetail.BaseQueryByIdAsync(entity.ProdDetailID);
                                entity.property = prodDetail.prop;
                                entity.SKU = prodDetail.SKU;
                            }
                        }
                    }
                }



            };

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_PackingValidator(), kryptonPanelMainInfo.Controls);
                //UIBaseTool uIBaseTool = new();
                //uIBaseTool.CurMenuInfo = CurMenuInfo;
                //uIBaseTool.InitEditItemToControl<tb_Packing>(entity, kryptonPanelMainInfo.Controls);

                var lambdaProdBundle = Expressionable.Create<tb_ProdBundle>()
                   .And(t => t.Is_enabled == true)
                   .ToExpression();
                BaseProcessor baseProcessorProdBundle = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdBundle).Name + "Processor");
                QueryFilter queryFilterProdBundle = baseProcessorProdBundle.GetQueryFilter();
                queryFilterProdBundle.FilterLimitExpressions.Add(lambdaProdBundle);
                DataBindingHelper.BindData4Cmb<tb_ProdBundle>(entity, k => k.BundleID, v => v.BundleName, cmbBundleID, queryFilterProdBundle.GetFilterExpression<tb_ProdBundle>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_ProdBundle>(entity, cmbBundleID, c => c.BundleName, queryFilterProdBundle);


                var lambdaProdProdDetail = Expressionable.Create<View_ProdDetail>()
                        .And(t => t.产品可用 == true)
                        .ToExpression();
                BaseProcessor baseProcessorProdDetail = Startup.GetFromFacByName<BaseProcessor>(typeof(View_ProdDetail).Name + "Processor");
                QueryFilter queryFilterProdDetail = baseProcessorProdDetail.GetQueryFilter();
                queryFilterProdDetail.FilterLimitExpressions.Add(lambdaProdProdDetail);
                DataBindingHelper.BindData4Cmb<View_ProdDetail>(entity, k => k.ProdDetailID, v => v.CNName, cmbProdDetailID, queryFilterProdDetail.GetFilterExpression<View_ProdDetail>(), true);
                DataBindingHelper.InitFilterForControlByExp<View_ProdDetail>(entity, cmbProdDetailID, c => c.SKU, queryFilterProdDetail);




                var lambdaProd = Expressionable.Create<tb_Prod>()
                                .And(t => t.isdeleted == false)
                                .ToExpression();
                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Prod).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                queryFilterC.FilterLimitExpressions.Add(lambdaProd);
                DataBindingHelper.BindData4Cmb<tb_Prod>(entity, k => k.ProdBaseID, v => v.CNName, cmbProdBaseID, queryFilterC.GetFilterExpression<tb_Prod>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_Prod>(entity, cmbProdBaseID, c => c.CNName, queryFilterC);

            }
            else
            {
                DataBindingHelper.BindData4Cmb<tb_Prod>(entity, k => k.ProdBaseID, v => v.CNName, cmbProdBaseID, true);
                DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v => v.SKU, cmbProdDetailID, true);
                DataBindingHelper.BindData4Cmb<tb_ProdBundle>(entity, k => k.BundleID, v => v.BundleName, cmbBundleID, true);
            }

            base.BindData(entity);
        }


        protected override async Task<ReturnResults<tb_Packing>> Delete()
        {
            ReturnResults<tb_Packing> rss = new ReturnResults<tb_Packing>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }
            //弱引用时，不能直接删除

            if (MessageBox.Show("系统不建议删除基础资料\r\n你确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                tb_PackingController<tb_Packing> ctr = Startup.GetFromFac<tb_PackingController<tb_Packing>>();
                bool rs = await ctr.BaseDeleteByNavAsync(EditEntity as tb_Packing);
                rss.Succeeded = rs;
                rss.ReturnObject = EditEntity;
                if (rs)
                {
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.logger.LogInformation($"UCpacking删除:{typeof(tb_Packing).Name}，主键值：{EditEntity.Pack_ID.ToString()} "); //如果要生效 要将配置文件
                    }
                    bindingSourceSub.Clear();
                    //提示一下删除成功
                    MainForm.Instance.uclog.AddLog("提示", "删除成功");
                    //加载一个空的显示的UI
                    bindingSourceOtherSub.Clear();
                    sgd1.BindingSourceLines.Clear();
                    sgd2.BindingSourceLines.Clear();
                    Add();
                }
            }
            return rss;
        }



        SourceGridDefine sgd1 = null;
        SourceGridDefine sgd2 = null;

        SourceGridHelper sgh1 = new SourceGridHelper();
        SourceGridHelper sgh2 = new SourceGridHelper();

        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        List<SourceGridDefineColumnItem> listCols1 = new List<SourceGridDefineColumnItem>();
        List<SourceGridDefineColumnItem> listCols2 = new List<SourceGridDefineColumnItem>();

        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            //InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);
            LoadGrid1();
            LoadGrid2();
            base.ControlMasterColumnsInvisible();
        }

        private void LoadGrid1()
        { ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_PackingDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;



            //指定了关键字段ProdDetailID
            listCols1 = sgh1.GetGridColumns<ProductSharePart, tb_PackingDetail>(c => c.ProdDetailID, false);
            listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols1.SetCol_NeverVisible<tb_PackingDetail>(c => c.ProdDetailID);
            listCols1.SetCol_NeverVisible<tb_PackingDetail>(c => c.Created_at);
            listCols1.SetCol_NeverVisible<tb_PackingDetail>(c => c.Created_by);
            listCols1.SetCol_NeverVisible<tb_PackingDetail>(c => c.Modified_at);
            listCols1.SetCol_NeverVisible<tb_PackingDetail>(c => c.Modified_by);
            listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            ControlChildColumnsInvisible(listCols1);
            //listCols.SetCol_DefaultValue<tb_PackingDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            listCols1.SetCol_ReadOnly<tb_PackingDetail>(c => c.PackDetail_ID);
            listCols1.SetCol_ReadOnly<tb_PackingDetail>(c => c.property);
            sgd1 = new SourceGridDefine(grid1, listCols1, true);
            sgd1.GridMasterData = EditEntity;


            listCols1.SetCol_Summary<tb_PackingDetail>(c => c.Quantity);




            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listCols1.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_PackingDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/




            sgh1.SetPointToColumnPairs<ProductSharePart, tb_PackingDetail>(sgd1, f => f.prop, t => t.property);

            //sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_PackingDetail>(sgd, f => f.BOM_ID, t => t.BOM_ID);


            //应该只提供一个结构
            List<tb_PackingDetail> lines = new List<tb_PackingDetail>();
            bindingSourceSub.DataSource = lines;
            sgd1.BindingSourceLines = bindingSourceSub;

            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //   .AndIF(true, w => w.CNName.Length > 0)
            //  // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //  .ToExpression();//注意 这一句 不能少
          
            //list = dc.BaseQueryByWhere(exp);

            list = MainForm.Instance.list;

            sgd1.SetDependencyObject<ProductSharePart, tb_PackingDetail>(list);

            sgd1.HasRowHeader = true;
            sgh1.InitGrid(grid1, sgd1, true, nameof(tb_PackingDetail));
            sgh1.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh1.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;

        }

        private void LoadGrid2()
        {

            ///显示列表对应的中文

            grid2.BorderStyle = BorderStyle.FixedSingle;
            grid2.Selection.EnableMultiSelection = false;
            listCols2 = new List<SourceGridDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols2 = sgh2.GetGridColumns<tb_BoxRules>();

            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Created_at);
            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Created_by);
            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Created_at);
            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Created_by);
            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Modified_at);
            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Modified_by);

            //当前编辑的。当然就是当前包装的装箱规格
            listCols2.SetCol_NeverVisible<tb_BoxRules>(c => c.Pack_ID);

            listCols2.SetCol_AutoSizeMode<tb_BoxRules>(c => c.Volume, SourceGrid.AutoSizeMode.EnableAutoSize);
            listCols2.SetCol_AutoSizeMode<tb_BoxRules>(c => c.GrossWeight, SourceGrid.AutoSizeMode.EnableAutoSizeView);
            listCols2.SetCol_AutoSizeMode<tb_BoxRules>(c => c.NetWeight, SourceGrid.AutoSizeMode.EnableAutoSizeView);
            listCols2.SetCol_AutoSizeMode<tb_BoxRules>(c => c.BoxRuleName, SourceGrid.AutoSizeMode.EnableAutoSizeView);

            ControlChildColumnsInvisible(listCols2);
            listCols2.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols2.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols2.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            sgd2 = new SourceGridDefine(grid2, listCols2, true);

            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.TotalCostAmount, true);
                listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.TotalCostAmount);
            }*/

            //listCols.SetCol_Summary<tb_BoxRules>(c => c.QuantityPerBox);

            //listCols.SetCol_Formula<tb_BoxRules>((a, b) => a.Cost * b.Quantity, c => c.SubtotalCostAmount);

            //bomid的下拉值。受当前行选择时会改变下拉范围,由产品ID决定BOM显示
            //前一列的选择影响后一列的显示
            //sgh2.SetCol_LimitedConditionsForSelectionRange<tb_BoxRules>(sgd2, t => t.ProdDetailID, f => f.BOM_ID);




            //List<KeyNamePair> keyNamePairs = new List<KeyNamePair>();
            //KeyNamePair keyName = new KeyNamePair();
            //keyName.SetKeyName<tb_BoxRules, tb_CartoonBox>(a => a.BoxRuleName, b => b.BoxRuleName);


            //选了纸箱规格后会影响其他列的值。自动生成
            listCols2.SetCol_RelatedValue<tb_BoxRules, tb_CartoonBox>(t => t.CartonID, t => t.BoxRuleName, "用{0}装箱{1}", new KeyNamePair<tb_BoxRules, tb_CartoonBox>(a => a.CartonID, b => b.CartonName), new KeyNamePair<tb_BoxRules>(a => a.PackingMethod));
            listCols2.SetCol_RelatedValue<tb_BoxRules, tb_CartoonBox>(t => t.PackingMethod, t => t.BoxRuleName, "用{0}装箱{1}", new KeyNamePair<tb_BoxRules, tb_CartoonBox>(a => a.CartonID, b => b.CartonName), new KeyNamePair<tb_BoxRules>(a => a.PackingMethod));

            //选择通用纸箱规格后。将他的数据带到其他列。
            listCols2.SetCol_RelatedValue<tb_BoxRules, tb_CartoonBox>(t => t.CartonID, t => t.Length, "{0}", new KeyNamePair<tb_BoxRules, tb_CartoonBox>(a => a.CartonID, b => b.Length));
            listCols2.SetCol_RelatedValue<tb_BoxRules, tb_CartoonBox>(t => t.CartonID, t => t.Width, "{0}", new KeyNamePair<tb_BoxRules, tb_CartoonBox>(a => a.CartonID, b => b.Width));
            listCols2.SetCol_RelatedValue<tb_BoxRules, tb_CartoonBox>(t => t.CartonID, t => t.Height, "{0}", new KeyNamePair<tb_BoxRules, tb_CartoonBox>(a => a.CartonID, b => b.Height));

            listCols2.SetCol_RelatedValue<tb_BoxRules>(a => a.QuantityPerBox, b => b.PackingMethod, "每{0}一箱", c => c.QuantityPerBox);

            listCols2.SetCol_Formula<tb_BoxRules>((a, b, c) => a.Length * b.Width * c.Height, d => d.Volume);

            //sgh2.SetPointToColumnPairs<tb_CartoonBox, tb_BoxRules>(sgd2, f => f.Volume, t => t.TransactionPrice);

            //应该是审核时要处理的逻辑暂时隐藏
            //将数量默认为已出库数量
            //listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => a.Quantity, c => c.TotalReturnedQty);
            //listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.TotalReturnedQty);

            //  sgh2.SetPointToColumnPairs<ProductSharePart, tb_BoxRules>(sgd2, f => f.Inv_Cost, t => t.Cost);
            //sgh2.SetPointToColumnPairs<ProductSharePart, tb_BoxRules>(sgd2, f => f.Standard_Price, t => t.TransactionPrice);

            listCols2.SetCol_DefaultValue<tb_BoxRules>(c => c.Is_enabled, true);

            //应该只提供一个结构
            List<tb_BoxRules> lines = new List<tb_BoxRules>();
            bindingSourceOtherSub.DataSource = lines;
            sgd2.BindingSourceLines = bindingSourceOtherSub;
            //    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            // .AndIF(true, w => w.CNName.Length > 0)
            //// .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //.ToExpression();//注意 这一句 不能少
            //                // StringBuilder sb = new StringBuilder();
            //    /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //    list = dc.BaseQueryByWhere(exp);

            list = MainForm.Instance.list;

            sgd2.SetDependencyObject<ProductSharePart, tb_BoxRules>(list);
            sgd2.HasRowHeader = true;
            sgd2.HasSummaryRow = false;
            sgh2.InitGrid(grid2, sgd2, true, nameof(tb_BoxRules));
            sgh2.OnCalculateColumnValue += Sgh2_OnCalculateColumnValue;
            sgh2.OnValidateDataCell += Sgh2_OnValidateDataCell;
        }

        private void Sgh2_OnValidateDataCell(object rowObj)
        {

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
                List<tb_PackingDetail> details = new List<tb_PackingDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_PackingDetail Detail = mapper.Map<tb_PackingDetail>(item);
                    details.Add(Detail);
                }
                sgh1.InsertItemDataToGrid<tb_PackingDetail>(grid1, sgd1, details, c => c.ProdDetailID, position);
            }

        }

        private void Sgh2_OnCalculateColumnValue(object rowObj, SourceGridDefine griddefine, SourceGrid.Position Position)
        {
            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {
                sgh2.SetCellValue(sgd2.GetColumnDefineInfo<tb_BoxRules>(c => c.BoxRuleName), Position, rowObj, false, true);
            }
            catch (Exception ex)
            {


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
                List<tb_PackingDetail> details = sgd1.BindingSourceLines.DataSource as List<tb_PackingDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }



            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_PackingDetail> details = new List<tb_PackingDetail>();

        List<tb_BoxRules> boxrules = new List<tb_BoxRules>();


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
            //var eer = errorProviderForAllInput.GetError(txtTotalQuantity);

            bindingSourceSub.EndEdit();



            List<tb_PackingDetail> detailentity = bindingSourceSub.DataSource as List<tb_PackingDetail>;
            List<tb_BoxRules> boxrulesEntity = bindingSourceOtherSub.DataSource as List<tb_BoxRules>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    if (MessageBox.Show("你确定不录入包装清单数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return false;
                    }

                }
                else
                {
                    if (NeedValidated && detailentity.Sum(c => c.Quantity) == 0)
                    {
                        MessageBox.Show("明细总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("包装清单中，相同的产品不能多行录入，请增加数量!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //====
                //选了箱子ID有值才算有效值
                boxrules = boxrulesEntity.Where(t => t.CartonID > 0).ToList();

                //一种箱子时只有一种箱规
                var bb = boxrules.Select(c => c.CartonID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && bb.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("箱规信息中，相同的箱子不能有多个箱规!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.tb_PackingDetails = details;
                EditEntity.tb_BoxRuleses = boxrules;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_PackingDetail>(details))
                {
                    return false;
                }

                //     RuleFor(tb_BoxRules =>tb_BoxRules.Pack_ID).Must(CheckForeignKeyValue).WithMessage("包装信息:下拉选择值不正确。"); 这个不能要
                if (NeedValidated && !base.Validator<tb_BoxRules>(boxrules))
                {
                    return false;
                }

                ReturnMainSubResults<tb_Packing> SaveResult = new ReturnMainSubResults<tb_Packing>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PackagingName}。");
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









        private async void LoadChildItems(long? saleorderid)
        {
            //ButtonSpecAny bsa = (txtSaleOrder as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            //if (bsa == null)
            //{
            //    return;
            //}
            //var saleorder = bsa.Tag as tb_SaleOrder;
            //因为要查BOM情况。不会传过来。
            var saleorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>().Where(c => c.SOrder_ID == saleorderid)
          .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
          .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_bom_s)
          .SingleAsync();
            //新增时才可以转单
            if (saleorder != null)
            {
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                tb_Packing entity = mapper.Map<tb_Packing>(saleorder);
                List<tb_PackingDetail> details = mapper.Map<List<tb_PackingDetail>>(saleorder.tb_SaleOrderDetails);
                entity.Created_at = System.DateTime.Now;
                List<tb_PackingDetail> NewDetails = new List<tb_PackingDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_SaleOrderDetail _SaleOrderDetail = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    if (_SaleOrderDetail != null && _SaleOrderDetail.tb_proddetail.tb_bom_s != null)
                    {
                        // details[i].BOM_ID = _SaleOrderDetail.tb_proddetail.tb_bom_s.BOM_ID;
                    }
                    else
                    {
                        tipsMsg.Add($"{_SaleOrderDetail.tb_proddetail.tb_prod.CNName}：");
                        //后面优化？检测一下库存
                        tipsMsg.Add($"没有BOM配方。无法正常进行需求分析,请删除！");
                        tipsMsg.Add($"如是半成品配件，需要外采。请另下采购单。");
                    }


                    if (details[i].Quantity > 0)
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
                entity.tb_PackingDetails = NewDetails;

                entity.ActionStatus = ActionStatus.新增;





                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(entity, actionStatus);
            }
        }

    }
}
