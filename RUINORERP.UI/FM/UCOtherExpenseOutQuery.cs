using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.Common.Helper;
using RUINOR.Core;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.FM
{

    [MenuAttrAssemblyInfo("其他费用支出查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.其他费用支出)]
    public partial class UCOtherExpenseOutQuery : BaseBillQueryMC<tb_FM_OtherExpense, tb_FM_OtherExpenseDetail>
    {
        public UCOtherExpenseOutQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ExpenseNo);
            //base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
        }



        


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_FM_OtherExpense>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                                .And(t => t.EXPOrINC == false)
                            // .And(t => t.Is_enabled == true)
                            //报销人员限制，财务不限制
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TaxAmount);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.UntaxedAmount);

            base.ChildSummaryCols.Add(c => c.TaxAmount);
            base.ChildSummaryCols.Add(c => c.TotalAmount);
            base.ChildSummaryCols.Add(c => c.UntaxedAmount);
        }


        public override void BuildInvisibleCols()
        {

            //base.ChildInvisibleCols.Add(c => c.Cost);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }

        /*
   /// <summary>
   /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
   /// </summary>
   /// <returns></returns>
   public async override Task<ApprovalEntity> Review(List<tb_FM_OtherExpense> EditEntitys)
   {
       if (EditEntitys == null)
       {
           return null;
       }
       //如果已经审核并且通过，则不能重复审核
       List<tb_FM_OtherExpense> needApprovals = EditEntitys.Where(
           c => ((c.ApprovalStatus.HasValue
           && c.ApprovalStatus.Value == (int)ApprovalStatus.已审核
           && c.ApprovalResults.HasValue && !c.ApprovalResults.Value))
           || (c.ApprovalStatus.HasValue && c.ApprovalStatus == (int)ApprovalStatus.未审核)
           ).ToList();

       //这一行临时的修复用
       needApprovals = EditEntitys;

       if (needApprovals.Count == 0)
       {
           MainForm.Instance.PrintInfoLog($"要审核的数据为：{needApprovals.Count}:请检查数据！");
           return null;
       }


       ApprovalEntity ae = base.BatchApproval(needApprovals);
       if (ae.ApprovalStatus == (int)ApprovalStatus.未审核)
       {
           return null;
       }


       tb_FM_OtherExpenseController<tb_FM_OtherExpense> ctr = Startup.GetFromFac<tb_FM_OtherExpenseController<tb_FM_OtherExpense>>();

       ReturnResults<bool> rrs = await ctr.BatchApprovalAsync(needApprovals, ae);
       if (rrs.Succeeded)
       {
           //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
           //{

           //}
           //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
           //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
           //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
           //MainForm.Instance.ecs.AddSendData(od);
           base.Query(QueryDto);
       }
       else
       {
           MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
       }

       return ae;
   }


   /// <summary>
   /// 反审
   /// </summary>
   /// <param name="EditEntitys"></param>
   /// <returns></returns>
   public async override Task<bool> ReReview(List<tb_FM_OtherExpense> EditEntitys)
   {
       if (EditEntitys == null)
       {
           return false;
       }
       foreach (tb_FM_OtherExpense EditEntity in EditEntitys)
       {
           #region 反审
           //反审，要审核过，并且通过了，才能反审。
           if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
           {
               MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
               continue;
           }


           if (EditEntity.tb_FM_OtherExpenseDetails == null || EditEntity.tb_FM_OtherExpenseDetails.Count == 0)
           {
               MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整金额。", UILogType.警告);
               continue;
           }

           RevertCommand command = new RevertCommand();
           //缓存当前编辑的对象。如果撤销就回原来的值
           tb_FM_OtherExpense oldobj = CloneHelper.DeepCloneObject<tb_FM_OtherExpense>(EditEntity);
           command.UndoOperation = delegate ()
           {
               //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
               CloneHelper.SetValues<tb_FM_OtherExpense>(EditEntity, oldobj);
           };

           tb_FM_OtherExpenseController<tb_FM_OtherExpense> ctr = Startup.GetFromFac<tb_FM_OtherExpenseController<tb_FM_OtherExpense>>();
           List<tb_FM_OtherExpense> list = new List<tb_FM_OtherExpense>();
           list.Add(EditEntity);
           ReturnResults<bool> rrs = await ctr.AntiApprovalAsync(list);
           if (rrs.Succeeded)
           {

               //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
               //{

               //}
               //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
               //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
               //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
               //MainForm.Instance.ecs.AddSendData(od);

               //审核成功
           }
           else
           {
               //审核失败 要恢复之前的值
               command.Undo();
               MainForm.Instance.PrintInfoLog($"{EditEntity.ExpenseNo}反审失败,请联系管理员！", Color.Red);
           }

           #endregion
       }
       return true;
   }*/

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_OtherExpense).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            var lambda = Expressionable.Create<tb_FM_OtherExpense>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            .And(t => t.isdeleted == false)
                               .And(t => t.EXPOrINC == false)
                           // .And(t => t.Is_enabled == true)
                           //报销人员限制，财务不限制
                           //  .AndIF(MainForm.Instance.AppContext.SysConfig.SaleBizLimited && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                           .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);


        }

        //public override List<tb_FM_OtherExpense> GetPrintDatas(List<tb_FM_OtherExpense> EditEntitys)
        //{
        //    List<tb_FM_OtherExpense> datas = new List<tb_FM_OtherExpense>();
        //    foreach (var item in EditEntitys)
        //    {
        //        tb_FM_OtherExpenseController<tb_FM_OtherExpense> ctr = Startup.GetFromFac<tb_FM_OtherExpenseController<tb_FM_OtherExpense>>();
        //        var PrintData = ctr.GetPrintData(item.ExpenseMainID);
        //        datas.Add(PrintData[0] as tb_FM_OtherExpense);
        //    }
        //    return datas;
        //}


        //public override List<tb_FM_OtherExpense> GetPrintDatas(tb_FM_OtherExpense EditEntity)
        //{
        //    List<tb_FM_OtherExpense> datas = new List<tb_FM_OtherExpense>();
        //    tb_FM_OtherExpenseController<tb_FM_OtherExpense> ctr = Startup.GetFromFac<tb_FM_OtherExpenseController<tb_FM_OtherExpense>>();
        //    List<tb_FM_OtherExpense> PrintData = ctr.GetPrintData(EditEntity.ExpenseMainID);
        //    return PrintData;
        //}


    }



}
