using AutoMapper;
using Microsoft.Extensions.Logging;
using Netron.NetronLight;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RUINOR.Core;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.SS;
using RUINORERP.UI.UControls;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using RulesEngine;
using RulesEngine.Models;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkflowCore.Interface;
using Rule = RulesEngine.Models.Rule;

namespace RUINORERP.UI.ASS
{

    [MenuAttrAssemblyInfo("售后交付单查询", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.售后流程, BizType.售后交付单)]
    public partial class UCASAfterSaleDeliveryQuery : BaseBillQueryMC<tb_AS_AfterSaleDelivery, tb_AS_AfterSaleDeliveryDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCASAfterSaleDeliveryQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ASDeliveryNo);
        }

        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_AS_AfterSaleDelivery, tb_AS_AfterSaleApply>(c => c.ASApplyNo, r => r.ASApplyNo);
            base.SetGridViewDisplayConfig();
        }




        public async Task<List<RuleResultWithFilter>> ExecuteRulesWithFilter(RulesEngine.RulesEngine re, tb_UserInfo user, tb_MenuInfo menu)
        {
            var results =await re.ExecuteAllRulesAsync("QueryFilterRules", user, menu);
            return results.Select(r => new RuleResultWithFilter
            {
                IsSuccess = r.IsSuccess,
                FilterExpression = r.IsSuccess ?
                    r.Rule.Expression.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Trim('"')
                    : null
            }).ToList();
        }







        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_AS_AfterSaleDelivery>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalDeliveryQty);

            base.ChildSummaryCols.Add(c => c.Quantity);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ASApplyID);
            base.ChildInvisibleCols.Add(c => c.ASApplyDetailID);
            base.ChildInvisibleCols.Add(c => c.ASDeliveryID);
        }





        public async override Task<bool> CloseCase(List<tb_AS_AfterSaleDelivery> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_AS_AfterSaleDelivery> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.审核通过 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_AS_AfterSaleDeliveryController<tb_AS_AfterSaleDelivery> ctr = Startup.GetFromFac<tb_AS_AfterSaleDeliveryController<tb_AS_AfterSaleDelivery>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                MainForm.Instance.PrintInfoLog($"结案操作成功！", Color.Red);
                for (int i = 0; i < needCloseCases.Count; i++)
                {
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_AfterSaleDelivery>("结案成功", needCloseCases[i]);
                }
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
