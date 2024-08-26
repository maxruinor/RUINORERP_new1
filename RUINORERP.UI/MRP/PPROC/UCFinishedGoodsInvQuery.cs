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
using RUINORERP.Common.Helper;
using RUINOR.Core;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("缴库单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.缴库单)]
    public partial class UCFinishedGoodsInvQuery : BaseBillQueryMC<tb_FinishedGoodsInv, tb_FinishedGoodsInvDetail>
    {
        public UCFinishedGoodsInvQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.DeliveryBillNo);
         
            base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
            //  base.OnQueryRelatedChild += UCPurEntryQuery_OnQueryRelatedChild;
        }

        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.MOID);
            base.MasterInvisibleCols.Add(c => c.FG_ID);
        }


        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_FinishedGoodsInv, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));


            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_FinishedGoodsInvDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_FinishedGoodsInv>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                               // .And(t => t.Is_enabled == true)
                               .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FinishedGoodsInv).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();


        }




        public override void BuildSummaryCols()
        {
            // base.MasterSummaryCols.Add(c => c.t);
            //base.MasterSummaryCols.Add(c => c.TotalAmount);
            // base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            //
            // base.ChildSummaryCols.Add(c => c.Quantity);

            // base.ChildRelatedSummaryCols.Add(c => c.Quantity);
        }

 

        /*

        /// <summary>
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <returns></returns>
        public async override Task<ApprovalEntity> Review(List<tb_FinishedGoodsInv> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_FinishedGoodsInv> needApprovals = EditEntitys.Where(
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

            tb_FinishedGoodsInvController<tb_FinishedGoodsInv> ctr = Startup.GetFromFac<tb_FinishedGoodsInvController<tb_FinishedGoodsInv>>();
            ReturnResults<bool> rs = await ctr.BatchApproval(needApprovals, ae);
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
        private void UCFinishedGoodsInvQuery_Load(object sender, EventArgs e)
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FinishedGoodsInv, tb_ManufacturingOrder>(a => a.MONo, b => b.MONO);
        }
    }



}
