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


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("其他入库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.其他入库单)]
    public partial class UCStockIn : BaseBillEditGeneric<tb_StockIn, tb_StockInDetail>
    {
        public UCStockIn()
        {
            InitializeComponent();
        }
  
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_StockIn, actionStatus);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.InitDataToCmb<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName, cmbType_ID, c => c.OutIn == true);
        }
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_StockIn).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public override void BindData(tb_StockIn entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.MainID > 0)
            {
                entity.PrimaryKeyID = entity.MainID;
                entity.ActionStatus = ActionStatus.加载;
                // entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.BillNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.其他入库单);

                entity.Bill_Date = System.DateTime.Now;
                entity.Enter_Date = System.DateTime.Now;
                if (entity.tb_StockInDetails != null && entity.tb_StockInDetails.Count > 0)
                {
                    entity.tb_StockInDetails.ForEach(c => c.MainID = 0);
                    entity.tb_StockInDetails.ForEach(c => c.Sub_ID = 0);
                }
            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.BindData4Cmb<tb_OutInStockType>(entity, k => k.Type_ID, v => v.TypeName, cmbType_ID, c => c.OutIn == true);


            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4DataTime<tb_StockIn>(entity, t => t.Bill_Date, dtpBill_Date, false);
            DataBindingHelper.BindData4DataTime<tb_StockIn>(entity, t => t.Enter_Date, dtpIn_date, false);
            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.RefNO, txtRefNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_StockIn>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_StockIn>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));

            DataBindingHelper.BindData4ControlByEnum<tb_StockIn>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            txtstatus.ReadOnly = true;
            if (entity.tb_StockInDetails != null && entity.tb_StockInDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_StockInDetail>(grid1, sgd, entity.tb_StockInDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_StockInDetail>(grid1, sgd, new List<tb_StockInDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
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
            };

            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            // .And(t => t.IsCustomer == true)
                            //ndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

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


            List<SourceGridDefineColumnItem> listCols = sgh.GetGridColumns<ProductSharePart, tb_StockInDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_StockInDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_StockInDetail>(c => c.Sub_ID);
            listCols.SetCol_NeverVisible<tb_StockInDetail>(c => c.MainID);
            ControlChildColumnsInvisible(listCols);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            listCols.SetCol_ReadOnly<tb_StockInDetail>(c => c.Price);
            listCols.SetCol_ReadOnly<tb_StockInDetail>(c => c.Cost);
            listCols.SetCol_ReadOnly<tb_StockInDetail>(c => c.SubtotalCostAmount);
            listCols.SetCol_ReadOnly<tb_StockInDetail>(c => c.SubtotalPirceAmount);

            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_StockInDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_StockInDetail>(c => c.SubtotalCostAmount);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_StockInDetail>(c => c.Qty);

            listCols.SetCol_Formula<tb_StockInDetail>((a, b) => a.Cost * b.Qty, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_StockInDetail>((a, b) => a.Price * b.Qty, c => c.SubtotalPirceAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockInDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockInDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockInDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockInDetail>(sgd, f => f.Standard_Price, t => t.Price);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_StockInDetail>(sgd, f => f.prop, t => t.property);



            //应该只提供一个结构
            List<tb_StockInDetail> lines = new List<tb_StockInDetail>();
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
            sgd.SetDependencyObject<ProductSharePart, tb_StockInDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_StockInDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            base.ControlMasterColumnsInvisible();
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
                List<tb_StockInDetail> details = new List<tb_StockInDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_StockInDetail bOM_SDetail = mapper.Map<tb_StockInDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_StockInDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_StockInDetail> details = sgd.BindingSourceLines.DataSource as List<tb_StockInDetail>;
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


        List<tb_StockInDetail> details = new List<tb_StockInDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_StockInDetail> detailentity = bindingSourceSub.DataSource as List<tb_StockInDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.tb_StockInDetails = details;
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
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
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_StockInDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_StockIn> SaveResult = new ReturnMainSubResults<tb_StockIn>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BillNo}。");
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
       tb_StockIn oldobj = CloneHelper.DeepCloneObject<tb_StockIn>(EditEntity);
       command.UndoOperation = delegate ()
       {
           //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
           CloneHelper.SetValues<tb_StockIn>(EditEntity, oldobj);
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
       tb_StockInController<tb_StockIn> ctr = Startup.GetFromFac<tb_StockInController<tb_StockIn>>();

       ReturnResults<tb_StockIn> rs = await ctr.ApprovalAsync(EditEntity);
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
                   tb_StockIn oldobj = CloneHelper.DeepCloneObject<tb_StockIn>(EditEntity);
                   command.UndoOperation = delegate ()
                   {
                       //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                       CloneHelper.SetValues<tb_StockIn>(EditEntity, oldobj);
                   };
                   ApprovalEntity ae = await base.Review();

                   // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                   //因为只需要更新主表
                   //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                   // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
                   tb_StockInController<tb_StockIn> ctr = Startup.GetFromFac<tb_StockInController<tb_StockIn>>();
                   List<tb_StockIn> tb_StockIns = new List<tb_StockIn>();
                   tb_StockIns.Add(EditEntity);

                   ReturnResults<bool> rs = await ctr.AntiApprovalAsync(tb_StockIns);
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
        private void cmbType_ID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
