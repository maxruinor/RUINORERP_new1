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
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using Netron.GraphLib;
using static System.Drawing.Html.CssLength;


namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("套装组合", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料, BizType.套装组合)]
    public partial class UCProdBundle : BaseBillEditGeneric<tb_ProdBundle, tb_ProdBundle>
    {
        public UCProdBundle()
        {
            InitializeComponent();
            base.OnBindDataToUIEvent += UCStockIn_OnBindDataToUIEvent;
        }
        private void UCStockIn_OnBindDataToUIEvent(tb_ProdBundle entity)
        {
            BindData(entity as tb_ProdBundle);
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
        public void BindData(tb_ProdBundle entity)
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


            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
            }


            DataBindingHelper.BindData4Cmb<tb_Unit>(entity, t => t.Unit_ID, c => c.UnitName, cmbUnit);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.BundleName, txtBundleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.BundleName, txtBundleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.BundleName, txtBundleName, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Notes, txtDescription, BindDataType4TextBox.Text, false);
            if (entity.tb_ProdBundleDetails != null && entity.tb_ProdBundleDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProdBundleDetail>(grid1, sgd, entity.tb_ProdBundleDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdBundleDetail>(grid1, sgd, new List<tb_ProdBundleDetail>(), c => c.ProdDetailID);
            }
            if (EditEntity.PrintStatus == 0)
            {
                lblPrintStatus.Text = "未打印";
            }
            else
            {
                lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
            }




            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                entity.ActionStatus = ActionStatus.修改;
                base.ToolBarEnabledControl(MenuItemEnums.修改);

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


            List<SourceGridDefineColumnItem> listCols = sgh.GetGridColumns<ProductSharePart, tb_ProdBundleDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.BundleChildID);
            listCols.SetCol_NeverVisible<tb_ProdBundleDetail>(c => c.BundleID);


            ControlChildColumnsInvisible(listCols);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            //listCols.SetCol_ReadOnly<tb_ProdBundleDetail>(c => c.Qty);




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
            sgd.GridData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_ProdBundleDetail>(c => c.Quantity);



            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdBundleDetail>(sgd, f => f.prop, t => t.property);



            //应该只提供一个结构
            List<tb_ProdBundleDetail> lines = new List<tb_ProdBundleDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                 .AndIF(true, w => w.CNName.Length > 0)
                // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
                .ToExpression();//注意 这一句 不能少
                                // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);
            sgd.SetDependencyObject<ProductSharePart, tb_ProdBundleDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdBundleDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            grid1.Enter += Grid1_Enter;
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
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_ProdBundleDetail bOM_SDetail = mapper.Map<tb_ProdBundleDetail>(item);
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

       Command command = new Command();
       //缓存当前编辑的对象。如果撤销就回原来的值
       tb_ProdBundle oldobj = CloneHelper.DeepCloneObject<tb_ProdBundle>(EditEntity);
       command.UndoOperation = delegate ()
       {
           //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
           CloneHelper.SetValues<tb_ProdBundle>(EditEntity, oldobj);
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
       tb_ProdBundleController<tb_ProdBundle> ctr = Startup.GetFromFac<tb_ProdBundleController<tb_ProdBundle>>();
       List<tb_ProdBundle> _StockIns = new List<tb_ProdBundle>();
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
                   Command command = new Command();
                   //缓存当前编辑的对象。如果撤销就回原来的值
                   tb_ProdBundle oldobj = CloneHelper.DeepCloneObject<tb_ProdBundle>(EditEntity);
                   command.UndoOperation = delegate ()
                   {
                       //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                       CloneHelper.SetValues<tb_ProdBundle>(EditEntity, oldobj);
                   };
                   ApprovalEntity ae = await base.Review();

                   // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                   //因为只需要更新主表
                   //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                   // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
                   tb_ProdBundleController<tb_ProdBundle> ctr = Startup.GetFromFac<tb_ProdBundleController<tb_ProdBundle>>();
                   List<tb_ProdBundle> tb_ProdBundles = new List<tb_ProdBundle>();
                   tb_ProdBundles.Add(EditEntity);

                   ReturnResults<bool> rs = await ctr.AntiApprovalAsync(tb_ProdBundles);
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
                if (details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                //二选中，验证机制还没有弄好。先这里处理
                //if (EditEntity.CustomerVendor_ID == 0 || EditEntity.CustomerVendor_ID == -1)
                //{
                //    EditEntity.CustomerVendor_ID = null;
                //}

                //if (EditEntity.Employee_ID == 0 || EditEntity.Employee_ID == -1)
                //{
                //    EditEntity.Employee_ID = null;
                //}
                //如果明细包含主表中的母件时。不允许保存

                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //EditEntity.MergeSourceTotalQty = details.Sum(c => c.Qty);
                EditEntity.tb_ProdBundleDetails = details;
                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return false;
                }
                if (!base.Validator<tb_ProdBundleDetail>(details))
                {
                    return false;
                }
 
                ReturnMainSubResults<tb_ProdBundle> SaveResult = await base.Save(EditEntity);
                if (SaveResult.Succeeded)
                {
                    lblReview.Text = ((ApprovalStatus)EditEntity.ApprovalStatus).ToString();
                    MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BundleName}。");
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("保存失败，请重试;或联系管理员。" + SaveResult.ErrorMsg, UILogType.错误);
                }
                return SaveResult.Succeeded;
            }
            return false;

        }














    }
}
