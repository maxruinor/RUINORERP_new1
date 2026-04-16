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

    [MenuAttrAssemblyInfo("生产计划单查询", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制造规划, BizType.生产计划单)]
    public partial class UCProductionPlanQuery : BaseBillQueryMC<tb_ProductionPlan, tb_ProductionPlanDetail>
    {
        public UCProductionPlanQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PPNo);
            
      
            //base._UCBillMasterQuery.ColDisplayType = typeof(tb_ProductionPlan);
        }

        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_ProductionPlan, tb_SaleOrder>(a => a.SaleOrderNo, b => b.SOrderNo);
            base.SetGridViewDisplayConfig();
        }

        public override void BuildColNameDataDictionary()
        {

            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_ProductionPlan, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));

            System.Linq.Expressions.Expression<Func<tb_ProductionPlan, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));

            System.Linq.Expressions.Expression<Func<tb_ProductionPlan, int?>> exprPriority;
            exprPriority = (p) => p.Priority;
            base.MasterColNameDataDictionary.TryAdd(exprPriority.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority)));
 
            
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.View_ProdDetailList)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_ProductionPlanDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_ProductionPlan>()
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
        //public override void BuildQueryCondition()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProductionPlan).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQuantity);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.CompletedQuantity);

        }

        public override void BuildInvisibleCols()
        {
             base.MasterInvisibleCols.Add(c => c.SOrder_ID);
            // base.ChildInvisibleCols.Add(c => c.BOM_ID);
            // base.ChildInvisibleCols.Add(c => c.ProdDetailID);

        }





         

        public async override Task<bool> CloseCase(List<tb_ProductionPlan> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_ProductionPlan> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.审核通过 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_ProductionPlanController<tb_ProductionPlan> ctr = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
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
                base.Query(QueryDtoProxy);
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }





    }
}
