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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;
using RUINORERP.Business.CommService;
using FastReport.Table;
using RUINORERP.UI.UControls;
using StackExchange.Redis;
namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("采购订单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购订单)]
    public partial class UCPurOrderQuery : BaseBillQueryMC<tb_PurOrder, tb_PurOrderDetail>,UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCPurOrderQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PurOrderNo);
        }

        public List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为采购入库单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为入库单】", true, false, "NewSumDataGridView_转为采购入库单"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为采购入库单);
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

        private void NewSumDataGridView_转为采购入库单(object sender, EventArgs e)
        {
            List<tb_PurOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_PurEntries != null && item.tb_PurEntries.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.PurOrderNo}：已经生成过入库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }

                    tb_PurOrderController<tb_PurOrder> ctr = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                    tb_PurEntry purEntry = ctr.PurOrderTotb_PurEntry(item);
               
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_PurEntry) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, purEntry);
                    }
                    return;
                }
                else
                {
                    // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                    MessageBox.Show($"当前订单{item.PurOrderNo}：未审核，无法生成入库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
       

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_PurOrder>()
                             .And(t => t.isdeleted == false)
                              .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            base.BuildQueryCondition();
            var lambda = Expressionable.Create<tb_PurOrder>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }

   

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(e => e.TotalQty);
            base.MasterSummaryCols.Add(e => e.TotalAmount);
            base.MasterSummaryCols.Add(e => e.ForeignTotalAmount);
            base.MasterSummaryCols.Add(e => e.TotalTaxAmount);

            base.ChildSummaryCols.Add(e => e.Quantity);
            base.ChildSummaryCols.Add(e => e.SubtotalAmount);
            base.ChildSummaryCols.Add(e => e.TaxAmount);

        }
        /// <summary>
        /// 批量转换为采购入库单
        /// </summary>
        public async  void BatchConversion()
        {
            tb_PurEntryController<tb_PurEntry> ctr = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
            List<tb_PurOrder> selectlist = GetSelectResult();
            int conter = 0;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_PurEntries != null && item.tb_PurEntries.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.PurOrderNo}：已经生成过入库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }

                    }

                     
                    tb_PurOrderController<tb_PurOrder> ctrPurOrder = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                    tb_PurEntry purEntry = ctrPurOrder.PurOrderTotb_PurEntry(item);
                    if (purEntry.tb_PurEntryDetails.Count > 0)
                    {
                        ReturnMainSubResults<tb_PurEntry> rsrs = await ctr.BaseSaveOrUpdateWithChild<tb_PurEntry>(purEntry);
                        if (rsrs.Succeeded)
                        {
                            conter++;
                        }
                        else
                        {
                            MainForm.Instance.uclog.AddLog("转换出错:" + rsrs.ErrorMsg);
                        }
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("转换出错,出库明细转换结果为0行，请检查后重试。");
                    }

                }
                else
                {
                    MainForm.Instance.uclog.AddLog(string.Format("当前订单:{0}的状态为{1},不能转换为采购入库单。", item.PurOrderNo, ((DataStatus)item.DataStatus).ToString()));
                    continue;
                }

            }
            MainForm.Instance.uclog.AddLog("转换完成,成功转换的订单数量:" + conter);
        }



    }



}
