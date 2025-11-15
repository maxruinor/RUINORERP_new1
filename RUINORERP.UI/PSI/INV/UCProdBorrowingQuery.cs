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
using RUINORERP.Business.Processor;
using RUINORERP.Model.Base;
using RUINORERP.UI.UControls;


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("借出单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.借出归还, BizType.借出单)]
    public partial class UCProdBorrowingQuery : BaseBillQueryMC<tb_ProdBorrowing, tb_ProdBorrowingDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {

        public UCProdBorrowingQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.BorrowNo);
        }

        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为归还单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为归还单】", true, false, "NewSumDataGridView_转为归还单"));
            list.Add(new ContextMenuController("【转为费用单】", true, false, "NewSumDataGridView_转为费用单"));
            return list;
        }


        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为归还单);
            ContextClickList.Add(NewSumDataGridView_转为费用单);

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

        private async void NewSumDataGridView_转为归还单(object sender, EventArgs e)
        {
            List<tb_ProdBorrowing> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_ProdReturnings != null && item.tb_ProdReturnings.Count > 0)
                    {
                        if (MessageBox.Show($"当前【借出单】{item.BorrowNo}：已经生成过【归还单】，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }
                    tb_ProdReturningController<tb_ProdReturning> ctr = Startup.GetFromFac<tb_ProdReturningController<tb_ProdReturning>>();
                    tb_ProdReturning ProdReturning =await ctr.BorrowToProdReturning(item);

                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_ProdReturning) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, ProdReturning);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"当前【借出单】{item.BorrowNo}：未审核，无法生成【归还单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void NewSumDataGridView_转为费用单(object sender, EventArgs e)
        {
            List<tb_ProdBorrowing> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    var ctr = Startup.GetFromFac<tb_FM_ProfitLossController<tb_FM_ProfitLoss>>();
                    tb_FM_ProfitLoss profitLoss =await ctr.BuildProfitLoss(item);

                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_FM_ProfitLoss) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, profitLoss);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"当前【借出单】{item.BorrowNo}：未审核，无法生成【损失费用单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_ProdBorrowing>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                             // .And(t => t.Is_enabled == true)
                             .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)

                            // .And(t => t.Is_enabled == true)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdBorrowing).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_ProdBorrowing>()
                .And(t => t.isdeleted == false)
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.ChildSummaryCols.Add(c => c.Qty);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
        }

        public override void BuildInvisibleCols()
        {
            //base.MasterInvisibleCols.Add(c => c.TotalCost);
            //base.ChildInvisibleCols.Add(c => c.Cost);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }






    }
}
