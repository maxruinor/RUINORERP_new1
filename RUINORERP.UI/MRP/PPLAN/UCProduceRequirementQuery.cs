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

    [MenuAttrAssemblyInfo("需求分析查询", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制造规划, BizType.需求分析)]
    public partial class UCProduceRequirementQuery : BaseBillQueryMC<tb_ProductionDemand, tb_ProductionDemandDetail>
    {
        public UCProduceRequirementQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PDNo);
            base.ChildRelatedEntityType = typeof(tb_ProductionDemandTargetDetail);
            base.OnQueryRelatedChild += UCPurEntryQuery_OnQueryRelatedChild;
            //base._UCBillMasterQuery.ColDisplayType = typeof(tb_ProductionDemand);
        }
        private async void UCPurEntryQuery_OnQueryRelatedChild(object obj, BindingSource bindingSource)
        {
            if (obj != null)
            {
                if (obj is tb_ProductionDemand Production)
                {
                    if (Production == null || Production.PDID == 0)
                    {
                        bindingSource.DataSource = null;
                    }
                    else if (Production.PDID > 0 && (bindingSource.DataSource == null))
                    {
                        bindingSource.DataSource = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionDemandTargetDetail>().Where(c => c.PDID == Production.PDID).ToListAsync();
                    }
                }
            }
        }

        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_ProductionDemand, tb_ProductionPlan>(a => a.PPNo, b => b.PPNo);
            base.SetGridViewDisplayConfig();
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_ProductionDemand>()
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
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProductionDemand).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            //base.MasterSummaryCols.Add(c => c.q);

            //base.ChildSummaryCols.Add(c => c.Quantity);
            //base.ChildSummaryCols.Add(c => c.CompletedQuantity);

        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PDID);
            base.MasterInvisibleCols.Add(c => c.PPID);
            // base.ChildInvisibleCols.Add(c => c.BOM_ID);
            // base.ChildInvisibleCols.Add(c => c.ProdDetailID);

        }





 

        public async override Task<bool> CloseCase(List<tb_ProductionDemand> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_ProductionDemand> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.审核通过 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();
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
