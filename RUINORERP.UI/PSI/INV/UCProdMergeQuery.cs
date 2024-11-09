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
using RUINORERP.Common.Extensions;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("产品组合单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.产品组合单)]
    public partial class UCProdMergeQuery : BaseBillQueryMC<tb_ProdMerge, tb_ProdMergeDetail>
    {

        public UCProdMergeQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.MergeNo);
            base.tsbtnAntiApproval.Visible = false;
            base.tsbtnBatchConversion.Visible = false;

        }

        /*
        public override Task<bool> ReReview(List<tb_ProdMerge> EditEntitys)
        {
            MessageBox.Show("请在单据明细中使用反审功能。");
            return null;
        }

        public async override Task<ApprovalEntity> Review(List<tb_ProdMerge> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_ProdMerge> needApprovals = EditEntitys.Where(
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

            tb_ProdMergeController<tb_ProdMerge> ctr = Startup.GetFromFac<tb_ProdMergeController<tb_ProdMerge>>();
            ReturnResults<bool> rs = await ctr.AdjustingAsync(needApprovals, ae);
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
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
            }

            return ae;
        }

        */
        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_ProdMerge, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));

            System.Linq.Expressions.Expression<Func<tb_ProdMerge, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));

            //System.Linq.Expressions.Expression<Func<tb_ProdMerge, int?>> exprPayStatus;
            //exprPayStatus = (p) => p.;
            //base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_ProdMergeDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

           
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_ProdMergeDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_ProdMerge>()
                             .And(t => t.isdeleted == false)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdMerge).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.MergeTargetQty);
            base.MasterSummaryCols.Add(c => c.MergeSourceTotalQty);
            base.ChildSummaryCols.Add(c => c.Qty);
            
        }

        public override void BuildInvisibleCols()
        {
          
        }


    }
}
