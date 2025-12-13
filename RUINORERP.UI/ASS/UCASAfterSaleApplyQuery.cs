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
using Microsoft.Extensions.Logging;
using RulesEngine.Models;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using System.IO;
using Rule = RulesEngine.Models.Rule;
using RUINORERP.Model.CommonModel;
using System.Linq.Expressions;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using WorkflowCore.Interface;
using RUINORERP.UI.SS;
using RulesEngine;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using Netron.NetronLight;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.ASS
{

    [MenuAttrAssemblyInfo("售后申请单查询", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.售后流程, BizType.售后申请单)]
    public partial class UCASAfterSaleApplyQuery : BaseBillQueryMC<tb_AS_AfterSaleApply, tb_AS_AfterSaleApplyDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCASAfterSaleApplyQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ASApplyNo);
        }

        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为售后交付单);
            ContextClickList.Add(NewSumDataGridView_标记已打印);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【标记已打印】", true, false, "NewSumDataGridView_标记已打印"));
            list.Add(new ContextMenuController("【转为交付单】", true, false, "NewSumDataGridView_转为售后交付单"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_标记已打印);
            ContextClickList.Add(NewSumDataGridView_转为售后交付单);


            List<ContextMenuController> list = new List<ContextMenuController>();
            list = AddContextMenu();

            UIHelper.ControlContextMenuInvisible(CurMenuInfo, list);

            if (_UCBillMasterQuery != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = _UCBillMasterQuery.newSumDataGridViewMaster.GetContextMenu(_UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip
                    , ContextClickList, list, true
                    );
                _UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = newContextMenuStrip;
            }
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


        private async void NewSumDataGridView_标记已打印(object sender, EventArgs e)
        {
            try
            {
                List<tb_AS_AfterSaleApply> selectlist = GetSelectResult();
                foreach (var item in selectlist)
                {
                    item.PrintStatus++;
                    tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply> ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
                    await ctr.SaveOrUpdate(item);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog($"标记已打印操作失败: {ex.Message}", Color.Red);
            }
        }

 
        private void NewSumDataGridView_转为售后交付单(object sender, EventArgs e)
        {
            List<tb_AS_AfterSaleApply> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_AS_AfterSaleDeliveries != null && item.tb_AS_AfterSaleDeliveries.Count > 0)
                    {
                        if (MessageBox.Show($"当前售后申请单{item.ASApplyNo}：已经生成过【售后交付单】，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }

                    tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply> ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
                    var DeliveryEntity = ctr.ToAfterSaleDelivery(item);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_AS_AfterSaleDelivery) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, DeliveryEntity);
                    }
                    return;
                }
                else
                {
                    if (item.DataStatus == (int)DataStatus.完结)
                    {
                        MessageBox.Show($"当前【售后申请单】{item.ASApplyNo}：已结案，无法生成【售后交付单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (item.DataStatus == (int)DataStatus.草稿 || item.DataStatus == (int)DataStatus.新建)
                    {
                        
                        MessageBox.Show($"当前【售后申请单】{item.ASApplyNo}：未审核，无法生成【售后交付单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
            }
        }



        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_AS_AfterSaleApply>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalDeliveredQty);
            base.MasterSummaryCols.Add(c => c.TotalInitialQuantity);
            base.MasterSummaryCols.Add(c => c.TotalConfirmedQuantity);

            base.ChildSummaryCols.Add(c => c.DeliveredQty);
            base.ChildSummaryCols.Add(c => c.ConfirmedQuantity);
            base.ChildSummaryCols.Add(c => c.InitialQuantity);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ASApplyID);
            base.ChildInvisibleCols.Add(c => c.ASApplyDetailID);
            base.ChildInvisibleCols.Add(c => c.ASApplyID);
        }


      
  

        public async override Task<bool> CloseCase(List<tb_AS_AfterSaleApply> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_AS_AfterSaleApply> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.审核通过 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply> ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                MainForm.Instance.PrintInfoLog($"结案操作成功！", Color.Red);
                MainForm.Instance.logger.LogInformation($"结案操作成功！");
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
