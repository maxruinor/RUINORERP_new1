﻿using System;
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

namespace RUINORERP.UI.MRP.PQC
{

    [MenuAttrAssemblyInfo("返工退库单查询", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.生产品控, BizType.返工退库单)]
    public partial class UCMRPReworkReturnQuery : BaseBillQueryMC<tb_MRP_ReworkReturn, tb_MRP_ReworkReturnDetail>
    {
        public UCMRPReworkReturnQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ReworkReturnNo);
            //显示转出库单
            tsbtnBatchConversion.Visible = false;
            //base._UCBillMasterQuery.ColDisplayType = typeof(tb_MRP_ReworkReturn);
        }

        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_MRP_ReworkReturn, tb_ManufacturingOrder>(c => c.ReworkReturnNo, r => r.MONO);
            base.SetGridViewDisplayConfig();
        }
        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.MOID);
            // base.ChildInvisibleCols.Add(c => c.ProdDetailID);
        }


        public override void BuildColNameDataDictionary()
        {
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_MRP_ReworkReturnDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
            base.BuildColNameDataDictionary();
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_MRP_ReworkReturn>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                               // .And(t => t.Is_enabled == true)
                             

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MRP_ReworkReturn).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalReworkFee);
        }









        /*
        /// <summary>
        /// 销售订单审核，审核成功后，库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// </summary>
        /// <returns></returns>
        public async override Task<ApprovalEntity> Review(List<tb_MRP_ReworkReturn> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_MRP_ReworkReturn> needApprovals = EditEntitys.Where(
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

            tb_MRP_ReworkReturnController<tb_MRP_ReworkReturn> ctr = Startup.GetFromFac<tb_MRP_ReworkReturnController<tb_MRP_ReworkReturn>>();
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
        public async override Task<bool> ReReview(List<tb_MRP_ReworkReturn> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            foreach (tb_MRP_ReworkReturn EditEntity in EditEntitys)
            {
                #region 反审
                //反审，要审核过，并且通过了，才能反审。
                if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
                {
                    MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                    continue;
                }


                if (EditEntity.tb_MRP_ReworkReturnDetails == null || EditEntity.tb_MRP_ReworkReturnDetails.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                    continue;
                }

                RevertCommand command = new RevertCommand();
                //缓存当前编辑的对象。如果撤销就回原来的值
                tb_MRP_ReworkReturn oldobj = CloneHelper.DeepCloneObject<tb_MRP_ReworkReturn>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<tb_MRP_ReworkReturn>(EditEntity, oldobj);
                };

                tb_MRP_ReworkReturnController<tb_MRP_ReworkReturn> ctr = Startup.GetFromFac<tb_MRP_ReworkReturnController<tb_MRP_ReworkReturn>>();
                List<tb_MRP_ReworkReturn> list = new List<tb_MRP_ReworkReturn>();
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

        public async override Task<bool> CloseCase(List<tb_MRP_ReworkReturn> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_MRP_ReworkReturn> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_MRP_ReworkReturnController<tb_MRP_ReworkReturn> ctr = Startup.GetFromFac<tb_MRP_ReworkReturnController<tb_MRP_ReworkReturn>>();
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