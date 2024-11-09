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
using RUINORERP.Common.Extensions;
using System.Collections;
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.MRP.MP
{

    [MenuAttrAssemblyInfo("生产退料单查询", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.生产退料单)]
    public partial class UCMaterialReturnQuery : BaseBillQueryMC<tb_MaterialReturn, tb_MaterialReturnDetail>
    {
        public UCMaterialReturnQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.BillNo);
            //显示转出库单
            tsbtnBatchConversion.Visible = false;
            //base._UCBillMasterQuery.ColDisplayType = typeof(tb_MaterialReturn);
        }

        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_MaterialReturn, tb_MaterialRequisition>(c => c.MaterialRequisitionNO, r => r.MaterialRequisitionNO);
            base.SetGridViewDisplayConfig();
        }
        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.MR_ID);
           // base.ChildInvisibleCols.Add(c => c.ProdDetailID);
        }
 

        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_MaterialReturn, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));

            System.Linq.Expressions.Expression<Func<tb_MaterialReturn, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));

            //System.Linq.Expressions.Expression<Func<tb_MaterialReturn, int?>> exprPriority;
            // exprPriority = (p) => p.Priority;
            // base.MasterColNameDataDictionary.TryAdd(exprPriority.GetMemberInfo().Name, GetKeyValuePairs(typeof(Priority)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_MaterialReturnDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
           
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_MaterialReturnDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_MaterialReturn>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                               // .And(t => t.Is_enabled == true)
                               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MaterialReturn).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.Cost);
        }









        /*
        /// <summary>
        /// 销售订单审核，审核成功后，库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// </summary>
        /// <returns></returns>
        public async override Task<ApprovalEntity> Review(List<tb_MaterialReturn> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_MaterialReturn> needApprovals = EditEntitys.Where(
                c => ((c.ApprovalStatus.HasValue
                && c.ApprovalStatus.Value == (int)ApprovalStatus.已审核
                && c.ApprovalResults.HasValue && !c.ApprovalResults.Value))
                || (c.ApprovalStatus.HasValue && c.ApprovalStatus == (int)ApprovalStatus.未审核)
                ).ToList();

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

            tb_MaterialReturnController<tb_MaterialReturn> ctr = Startup.GetFromFac<tb_MaterialReturnController<tb_MaterialReturn>>();
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
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！\r\n{rrs.ErrorMsg}", Color.Red);
            }

            return ae;
        }
     

        /// <summary>
        /// 销售订单反审
        /// </summary>
        /// <param name="EditEntitys"></param>
        /// <returns></returns>
        public async override Task<bool> ReReview(List<tb_MaterialReturn> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            foreach (tb_MaterialReturn EditEntity in EditEntitys)
            {
                #region 反审
                //反审，要审核过，并且通过了，才能反审。
                if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
                {
                    MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                    continue;
                }


                if (EditEntity.tb_MaterialReturnDetails == null || EditEntity.tb_MaterialReturnDetails.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                    continue;
                }

                Command command = new Command();
                //缓存当前编辑的对象。如果撤销就回原来的值
                tb_MaterialReturn oldobj = CloneHelper.DeepCloneObject<tb_MaterialReturn>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<tb_MaterialReturn>(EditEntity, oldobj);
                };

                tb_MaterialReturnController<tb_MaterialReturn> ctr = Startup.GetFromFac<tb_MaterialReturnController<tb_MaterialReturn>>();
                List<tb_MaterialReturn> list = new List<tb_MaterialReturn>();
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
                    MainForm.Instance.PrintInfoLog($"{EditEntity.BillNo}反审失败,请联系管理员！\r\n{rrs.ErrorMsg}", Color.Red);
                }

                #endregion
            }
            return true;
        }
           */

        public async override Task<bool> CloseCase(List<tb_MaterialReturn> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_MaterialReturn> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_MaterialReturnController<tb_MaterialReturn> ctr = Startup.GetFromFac<tb_MaterialReturnController<tb_MaterialReturn>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
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
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }





    }
}
