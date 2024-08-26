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
using RUINORERP.Business.Security;
using RUINORERP.Model.Base;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("销售出库单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.销售管理, BizType.销售出库单)]
    public partial class UCSaleOutQuery : BaseBillQueryMC<tb_SaleOut, tb_SaleOutDetail>
    {
        public UCSaleOutQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.SaleOutNo);

            //出库单本身不用显示
            tsbtnBatchConversion.Enabled = false;
            if (MainForm.Instance.AppContext.IsSuperUser)
            {
                base.tsbtnAntiApproval.Visible = true;
            }
            else
            {
                base.tsbtnAntiApproval.Visible = false;
            }

        }

        public override void SetGridViewDisplayConfig()
        {
            //base.SetRelatedBillCols<tb_SaleOrder>(c => c.SOrderNo, r => r.SaleOrderNo);
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_SaleOut, tb_SaleOrder>(c => c.SaleOrderNo, r => r.SOrderNo);
            base.SetGridViewDisplayConfig();
        }



        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_SaleOut, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));


            System.Linq.Expressions.Expression<Func<tb_SaleOut, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));


            System.Linq.Expressions.Expression<Func<tb_SaleOut, int?>> exprPayStatus;
            exprPayStatus = (p) => p.PayStatus;
            base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_SaleOutDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_SaleOutDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildInvisibleCols()
        {
            //在销售出库单中，引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.SOrder_ID);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOut).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            base.MasterSummaryCols.Add(c => c.TotalUntaxedAmount);
            base.MasterSummaryCols.Add(c => c.ShipCost);
            base.MasterSummaryCols.Add(c => c.CollectedMoney);
            base.MasterSummaryCols.Add(c => c.PrePayMoney);
            base.MasterSummaryCols.Add(c => c.Deposit);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.SubtotalUntaxedAmount);
            base.ChildSummaryCols.Add(c => c.CommissionAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTaxAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.TotalReturnedQty);
            base.BuildSummaryCols();
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_SaleOut>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                            // .And(t => t.Is_enabled == true)

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }


        /// <summary>
        /// 初始化列表数据
        /// </summary>
        //internal void InitListData()
        //{
        //    base.newSumDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //    base.newSumDataGridView1.XmlFileName = this.Name + nameof(tb_SaleOut);
        //    base.newSumDataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_SaleOut));
        //    bindingSourceListMain.DataSource = new List<tb_SaleOut>();
        //    base.newSumDataGridView1.DataSource = null;
        //    //绑定导航
        //    base.newSumDataGridView1.DataSource = bindingSourceListMain.DataSource;
        //}

        //public override List<tb_SaleOut> GetPrintDatas(tb_SaleOut EditEntity)
        //{
        //    List<tb_SaleOut> datas = new List<tb_SaleOut>();
        //    tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
        //    List<tb_SaleOut> PrintData = ctr.GetPrintData(EditEntity.SaleOut_MainID);
        //    return PrintData;
        //}

        /*
        /// <summary>
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <returns></returns>
        public async override Task<ApprovalEntity> Review(List<tb_SaleOut> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_SaleOut> needApprovals = EditEntitys.Where(
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

            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            ReturnResults<bool> rs = await ctr.BatchApprovalAsync(needApprovals, ae);
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
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return ae;
        }
             */

        public async override Task<bool> CloseCase(List<tb_SaleOut> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOut> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
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

        /*
        /// <summary>
        /// 列表中不再实现反审，批量，出库反审情况极少。并且是仔细处理
        /// </summary>
        public async override Task<bool> ReReview(List<tb_SaleOut> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            for (int i = 0; i < EditEntitys.Count; i++)
            {
                ReturnResults<bool> rs = await ctr.AntiApprovalAsync(EditEntitys[i]);
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
                    tsbtnAntiApproval.Enabled = false;

                }
                else
                {
                    //审核失败 要恢复之前的值
                    MainForm.Instance.PrintInfoLog($"销售出库单{EditEntitys[i].SaleOutNo}反审失败,{rs.ErrorMsg},请联系管理员！", Color.Red);
                }
            }

            return true;
        }
*/

    }



}
