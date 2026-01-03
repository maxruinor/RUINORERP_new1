using AutoMapper;
using FastReport.Table;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.FM.FMBase;
using RUINORERP.UI.UControls;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("采购订单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购订单)]
    public partial class UCPurOrderQuery : BaseBillQueryMC<tb_PurOrder, tb_PurOrderDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCPurOrderQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PurOrderNo);
        }

        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为采购入库单);
            ContextClickList.Add(NewSumDataGridView_预付货款);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为入库单】", true, false, "NewSumDataGridView_转为采购入库单"));
            list.Add(new ContextMenuController("【预付货款】", true, false, "NewSumDataGridView_预付货款"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为采购入库单);
            ContextClickList.Add(NewSumDataGridView_预付货款);
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


        private async void NewSumDataGridView_预付货款(object sender, EventArgs e)
        {
            try
            {
                List<tb_PurOrder> selectlist = GetSelectResult();
                List<tb_PurOrder> RealList = new List<tb_PurOrder>();
                StringBuilder msg = new StringBuilder();
                int counter = 1;
                foreach (var item in selectlist)
                {
                    //只有审核状态才可以转换为收款单
                    if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                    {
                        RealList.Add(item);
                    }
                    else
                    {
                        msg.Append(counter.ToString() + ") ");
                        msg.Append($"当前采购订单 {item.SOrderNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法进行再次预付款。").Append("\r\n");
                        counter++;
                    }
                }
                //多选时。要相同客户才能合并到一个收款单
                if (RealList.Count() > 1)
                {
                    msg.Append("一次只能选择一行数据进行预付款。");
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
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
                    msg.Append("请至少选择一行数据进行预付款。");
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var PurOrder = RealList[0];
                var amountRule = new AmountValidationRule();
                using (var inputForm = new frmInputObject(amountRule))
                {
                    inputForm.DefaultTitle = "请输入预付款金额";
                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        if (inputForm.InputContent.ToDecimal() <= 0)
                        {
                            MessageBox.Show("预付款金额必须大于0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        decimal PreAmount = inputForm.InputContent.ObjToDecimal();
                        //检测新增的订金是不是大于总金额了。
                        if (PurOrder.Deposit + PreAmount > PurOrder.TotalAmount)
                        {
                            if (MessageBox.Show($"【采购订单】原有预付款金额{PurOrder.Deposit}+当前预付款：{PreAmount}，超过了订单总金额{PurOrder.TotalAmount}，你确定要超额付款吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton: MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return;
                            }
                        }

                        if (MessageBox.Show($"针对采购订单：{PurOrder.SOrderNo}，确定向供应商{PurOrder.tb_customervendor.CVName}:预付款：{inputForm.InputContent}元吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var ctr = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                            var rs = await ctr.ManualPrePayment(inputForm.InputContent.ObjToDecimal(), PurOrder);
                            if (rs.Succeeded)
                            {
                                MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

                                string Flag = string.Empty;
                                Flag = SharedFlag.Flag2.ToString();

                                tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_FM_PreReceivedPayment)
                                && m.BIBaseForm == "BaseBillEditGeneric`2" && m.UIPropertyIdentifier == Flag).FirstOrDefault();
                                if (RelatedMenuInfo != null)
                                {
                                    await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, rs.ReturnObject);
                                    rs.ReturnObject.HasChanged = true;
                                }
                                return;
                            }
                            else
                            {
                                MessageBox.Show($"{rs.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void NewSumDataGridView_转为采购入库单(object sender, EventArgs e)
        {
            List<tb_PurOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
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
                    tb_PurEntry purEntry = await ctr.PurOrderTotb_PurEntry(item);

                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_PurEntry) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, purEntry);
                        purEntry.HasChanged = true;
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
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_PurOrder>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.SOrder_ID);
            base.ChildInvisibleCols.Add(c => c.PurOrder_ChildID);
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
        public async Task BatchConversion()
        {
            tb_PurEntryController<tb_PurEntry> ctr = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
            List<tb_PurOrder> selectlist = GetSelectResult();
            int conter = 0;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
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
                    tb_PurEntry purEntry = await ctrPurOrder.PurOrderTotb_PurEntry(item);
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
