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
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("采购入库查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购入库单)]
    public partial class UCPurEntryQuery : BaseBillQueryMC<tb_PurEntry, tb_PurEntryDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCPurEntryQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PurEntryNo);
            base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
            base.OnQueryRelatedChild += UCPurEntryQuery_OnQueryRelatedChild;
        }
        public override List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为应付款单】", true, false, "NewSumDataGridView_转为应付款单"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为退货单);
            ContextClickList.Add(NewSumDataGridView_转为应付款单);
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

        /*
        private void NewSumDataGridView_转为退货单(object sender, EventArgs e)
        {
            List<tb_PurEntry> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为采购退货单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_PurEntryRes != null && item.tb_PurEntryRes.Count > 0)
                    {
                        if (MessageBox.Show($"当前【采购入库单】{item.PurEntryNo}：已经生成过【采购退货单】，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }
                    var ctr = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
                    tb_PurEntryRe saleOutre = ctr.(item);

                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_PurEntryRe) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, saleOutre);
                    }
                    return;
                }
                else
                {
                    // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                    MessageBox.Show($"当前【采购入库单】{item.PurEntryNo}：未审核，无法生成【采购退货单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        */
        private async void NewSumDataGridView_转为应付款单(object sender, EventArgs e)
        {
            List<tb_PurEntry> selectlist = GetSelectResult();
            if (selectlist.Count > 1)
            {
                MessageBox.Show("每次只能选择一个【采购入库单】转成【应付款单】");
                return;
            }
            List<tb_PurEntry> RealList = new List<tb_PurEntry>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为应收
                bool canConvert = item.DataStatus == (long)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前采购入库单 {item.PurEntryNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法生【应付款单】。").Append("\r\n");
                    counter++;
                }
            }

            if (msg.ToString().Length > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (RealList.Count == 0)
                {
                    return;
                }
            }

            if (RealList.Count == 0)
            {
                msg.Append("请至少选择一行数据转为【应付款单】");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ReceivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
            tb_FM_ReceivablePayable ReceivablePayable = await ReceivablePayableController.BuildReceivablePayable(RealList[0], false);
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            string Flag = string.Empty;
            //付款
            Flag = typeof(RUINORERP.UI.FM.UCPayable).FullName;

            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == nameof(tb_FM_ReceivablePayable)
                        && m.BIBaseForm == "BaseBillEditGeneric`2" && m.ClassPath == Flag)
            .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, ReceivablePayable);
            }

        }
        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_PurEntry, tb_PurOrder>(c => c.PurOrder_NO, r => r.PurOrderNo);
            base.SetGridViewDisplayConfig();
        }

        private async void UCPurEntryQuery_OnQueryRelatedChild(object obj, BindingSource bindingSource)
        {
            if (obj != null)
            {
                if (obj is tb_PurEntry purEntry)
                {
                    if (purEntry.PurOrder_ID == null)
                    {
                        bindingSource.DataSource = null;
                    }
                    else
                    {
                        bindingSource.DataSource = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrderDetail>().Where(c => c.PurOrder_ID == purEntry.tb_purorder.PurOrder_ID).ToListAsync();
                    }

                }
            }
        }



        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.PurOrder_ID);
        }
        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_PurEntry>()
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
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_PurEntry>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            base.MasterSummaryCols.Add(c => c.ForeignTotalAmount);
            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.SubtotalAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalUntaxedAmount);
            base.ChildRelatedSummaryCols.Add(c => c.Quantity);

        }





    }



}
