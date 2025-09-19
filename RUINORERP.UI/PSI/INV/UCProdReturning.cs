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
using RUINORERP.PacketSpec.Legacy;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using Krypton.Toolkit;
using System.Diagnostics;

using Netron.GraphLib;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Model.CommonModel;


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("归还单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.借出归还, BizType.归还单)]
    public partial class UCProdReturning : BaseBillEditGeneric<tb_ProdReturning, tb_ProdReturningDetail>, IPublicEntityObject
    {
        public UCProdReturning()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_ProdReturning returning)
            {
                if (returning.BorrowID > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.借出单;
                    rqp.billId = returning.BorrowID;
                    rqp.billNo = returning.BorrowNO;
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
         await   base.LoadRelatedDataToDropDownItemsAsync();
        }
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_ProdReturning, actionStatus);
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdReturning).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }
        public override void BindData(tb_ProdReturning entity, ActionStatus actionStatus)
        {

            if (entity == null)
            {

                return;
            }
            var sw = new Stopwatch();
            sw.Start();
            EditEntity = entity;
            if (entity.ReturnID > 0)
            {
                entity.PrimaryKeyID = entity.ReturnID;
                entity.ActionStatus = ActionStatus.加载;
                // entity.DataStatus = (int)DataStatus.确认;
                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (string.IsNullOrEmpty(entity.ReturnNo))
                {
                    entity.ReturnNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.归还单);
                }
                entity.ReturnDate = System.DateTime.Now;
                if (entity.tb_ProdReturningDetails != null && entity.tb_ProdReturningDetails.Count > 0)
                {
                    entity.tb_ProdReturningDetails.ForEach(c => c.ReturnID = 0);
                    entity.tb_ProdReturningDetails.ForEach(c => c.ReturnSub_ID = 0);
                }
            }

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);

            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.ReturnNo, txtBillNo, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4DataTime<tb_ProdReturning>(entity, t => t.ReturnDate, dtpReturnDate, false);

            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.BorrowNO, txtBorrowNO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdReturning>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_ProdReturning>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));

            DataBindingHelper.BindData4ControlByEnum<tb_ProdReturning>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            txtstatus.ReadOnly = true;
            if (entity.tb_ProdReturningDetails != null && entity.tb_ProdReturningDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_ProdReturningDetail>(grid1, sgd, entity.tb_ProdReturningDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_ProdReturningDetail>(grid1, sgd, new List<tb_ProdReturningDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.BorrowID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ProdReturning>(c => c.BorrowID))
                {
                    LoadRefBillData(entity.BorrowID);
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


            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ProdBorrowing>(entity, v => v.BorrowNo, txtBorrowNO, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaBorrow = Expressionable.Create<tb_ProdBorrowing>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                            //是不是有开关来设置
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                             .And(t => t.isdeleted == false)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdBorrowing).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            queryFilter.FilterLimitExpressions.Add(lambdaBorrow);//意思是只有审核确认的。没有结案的。才能查询出来。

            ControlBindingHelper.ConfigureControlFilter<tb_ProdReturning, tb_ProdBorrowing>(entity, txtBorrowNO, t => t.BorrowNO,
             f => f.BorrowNo, queryFilter, a => a.BorrowID, b => b.BorrowID, null, false);
            base.BindData(entity);

            sw.Stop();
            MainForm.Instance.uclog.AddLog("Binding加载数据耗时：" + sw.ElapsedMilliseconds + "毫秒");
        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SGDefineColumnItem> listCols = sgh.GetGridColumns<ProductSharePart, tb_ProdReturningDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_ProdReturningDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_ProdReturningDetail>(c => c.ReturnSub_ID);
            listCols.SetCol_NeverVisible<tb_ProdReturningDetail>(c => c.ReturnID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            listCols.SetCol_ReadOnly<tb_ProdReturningDetail>(c => c.Price);
            listCols.SetCol_ReadOnly<tb_ProdReturningDetail>(c => c.Cost);
            listCols.SetCol_ReadOnly<tb_ProdReturningDetail>(c => c.SubtotalCostAmount);
            listCols.SetCol_ReadOnly<tb_ProdReturningDetail>(c => c.SubtotalPirceAmount);

            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_NeverVisible<tb_ProdReturningDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_ProdReturningDetail>(c => c.SubtotalCostAmount);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_ProdReturningDetail>(c => c.Qty);

            listCols.SetCol_Formula<tb_ProdReturningDetail>((a, b) => a.Cost * b.Qty, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_ProdReturningDetail>((a, b) => a.Price * b.Qty, c => c.SubtotalPirceAmount);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdReturningDetail>(sgd, f => f.Location_ID, t => t.Location_ID);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdReturningDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdReturningDetail>(sgd, f => f.Standard_Price, t => t.Price);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_ProdReturningDetail>(sgd, f => f.prop, t => t.property);



            //应该只提供一个结构
            List<tb_ProdReturningDetail> lines = new List<tb_ProdReturningDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            // sw.Start();
            //list = await dc.BaseQueryByWhereAsync(exp);
            list = MainForm.Instance.View_ProdDetailList;
            // sw.Stop();
            //MainForm.Instance.uclog.AddLog("Load加载数据耗时：" + sw.ElapsedMilliseconds + "毫秒");

            sgd.SetDependencyObject<ProductSharePart, tb_ProdReturningDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_ProdReturningDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);

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
                List<tb_ProdReturningDetail> details = new List<tb_ProdReturningDetail>();

                foreach (var item in RowDetails)
                {
                    tb_ProdReturningDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_ProdReturningDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_ProdReturningDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_ProdReturningDetail> details = sgd.BindingSourceLines.DataSource as List<tb_ProdReturningDetail>;
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


        List<tb_ProdReturningDetail> details = new List<tb_ProdReturningDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_ProdReturningDetail> detailentity = bindingSourceSub.DataSource as List<tb_ProdReturningDetail>;
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
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Qty);
                EditEntity.TotalAmount = details.Sum(c => c.Price * c.Qty);
                //二选中，验证机制还没有弄好。先这里处理
                if (NeedValidated && EditEntity.CustomerVendor_ID == 0 || EditEntity.CustomerVendor_ID == -1)
                {
                    EditEntity.CustomerVendor_ID = null;
                }

                if (NeedValidated && EditEntity.Employee_ID == 0 || EditEntity.Employee_ID == -1)
                {
                    EditEntity.Employee_ID = null;
                }
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                if (NeedValidated && EditEntity.TotalQty == 0)
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量不能为零!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                EditEntity.tb_ProdReturningDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_ProdReturningDetail>(details))
                {
                    return false;
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_ProdReturning> SaveResult = new ReturnMainSubResults<tb_ProdReturning>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ReturnNo}。");
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
       tb_ProdReturning oldobj = CloneHelper.DeepCloneObject<tb_ProdReturning>(EditEntity);
       command.UndoOperation = delegate ()
       {
           //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
           CloneHelper.SetValues<tb_ProdReturning>(EditEntity, oldobj);
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
       tb_ProdReturningController<tb_ProdReturning> ctr = Startup.GetFromFac<tb_ProdReturningController<tb_ProdReturning>>();
       List<tb_ProdReturning> _StockIns = new List<tb_ProdReturning>();
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
                   tb_ProdReturning oldobj = CloneHelper.DeepCloneObject<tb_ProdReturning>(EditEntity);
                   command.UndoOperation = delegate ()
                   {
                       //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                       CloneHelper.SetValues<tb_ProdReturning>(EditEntity, oldobj);
                   };
                   ApprovalEntity ae = await base.Review();

                   // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                   //因为只需要更新主表
                   //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                   // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
                   tb_ProdReturningController<tb_ProdReturning> ctr = Startup.GetFromFac<tb_ProdReturningController<tb_ProdReturning>>();
                   List<tb_ProdReturning> tb_ProdReturnings = new List<tb_ProdReturning>();
                   tb_ProdReturnings.Add(EditEntity);

                   ReturnResults<bool> rs = await ctr.AntiApprovalAsync(tb_ProdReturnings);
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


        private async Task LoadRefBillData(long? billID)
        {
            ButtonSpecAny bsa = (txtBorrowNO as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            tb_ProdBorrowing refBill = bsa.Tag as tb_ProdBorrowing;
            refBill = await MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>().Where(c => c.BorrowID == billID)
             .Includes(a => a.tb_ProdBorrowingDetails, b => b.tb_proddetail, c => c.tb_prod)
            .SingleAsync();
            //新增时才可以转单
            if (refBill != null)
            {

                tb_ProdReturning master = MainForm.Instance.mapper.Map<tb_ProdReturning>(refBill);
                List<tb_ProdReturningDetail> details = MainForm.Instance.mapper.Map<List<tb_ProdReturningDetail>>(refBill.tb_ProdBorrowingDetails);
                master.ReturnDate = System.DateTime.Now;

                List<tb_ProdReturningDetail> NewDetails = new List<tb_ProdReturningDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    #region 每行产品ID唯一

                    tb_ProdBorrowingDetail item = refBill.tb_ProdBorrowingDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Qty = item.Qty - item.ReQty;// 已经交数量去掉
                    details[i].SubtotalPirceAmount = details[i].Price * details[i].Qty;
                    details[i].SubtotalCostAmount = details[i].Cost * details[i].Qty;
                    if (details[i].Qty > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"借出单{refBill.BorrowNo}，{item.tb_proddetail.tb_prod.CNName}已归还数为{item.ReQty}，可归还数为{details[i].Qty}，当前行数据忽略！");
                    }
                    #endregion


                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{refBill.BorrowNo}已全部归还，请检查是否正在重复归还！");
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

                master.tb_ProdReturningDetails = NewDetails;
                master.BorrowID = refBill.BorrowID;
                master.BorrowNO = refBill.BorrowNo;
                master.TotalAmount = NewDetails.Sum(c => c.SubtotalPirceAmount);
                master.TotalCost = NewDetails.Sum(c => c.SubtotalCostAmount);
                master.TotalQty = NewDetails.Sum(c => c.Qty);

                master.DataStatus = (int)DataStatus.草稿;
                master.ApprovalStatus = (int)ApprovalStatus.未审核;
                master.ApprovalResults = null;
                master.ApprovalOpinions = "";
                master.Modified_at = null;
                master.Modified_by = null;
                master.Approver_at = null;
                master.Approver_by = null;
                master.PrintStatus = 0;
                master.ActionStatus = ActionStatus.新增;

                BusinessHelper.Instance.InitEntity(master);


                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(master, actionStatus);
            }
        }



    }
}
