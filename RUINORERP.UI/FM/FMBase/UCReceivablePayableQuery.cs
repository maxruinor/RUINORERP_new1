using AutoMapper;
using AutoUpdateTools;
using Krypton.Navigator;
using LiveChartsCore.Geo;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using RUINOR.Core;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global.Model;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.UControls;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.FM
{
    //应收应付查询
    public partial class UCReceivablePayableQuery : BaseBillQueryMC<tb_FM_ReceivablePayable, tb_FM_ReceivablePayableDetail>
    {
        public UCReceivablePayableQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ARAPNo);
        }
        public ReceivePaymentType PaymentType { get; set; }
        public override void BuildLimitQueryConditions()
        {

            //这里外层来实现对客户供应商的限制
            string customerVendorId = "".ToFieldName<tb_CustomerVendor>(c => c.CustomerVendor_ID);

            //应收付款中的往来单位额外添加一些条件
            var lambdaCv = Expressionable.Create<tb_CustomerVendor>()
              //.AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)
              //.AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)
              .ToExpression();
            QueryField queryField = QueryConditionFilter.QueryFields.Where(c => c.FieldName == customerVendorId).FirstOrDefault();
            queryField.SubFilter.FilterLimitExpressions.Add(lambdaCv);


            var lambda = Expressionable.Create<tb_FM_ReceivablePayable>()
                              .And(t => t.isdeleted == false)
                             .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }
        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList("批量处理");
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.复制性新增);
            base.AddExcludeMenuList(MenuItemEnums.数据特殊修正);
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.删除);
            base.AddExcludeMenuList(MenuItemEnums.提交);
            base.AddExcludeMenuList(MenuItemEnums.新增);
            base.AddExcludeMenuList(MenuItemEnums.导入);
            base.AddExcludeMenuList(MenuItemEnums.修改);
            base.AddExcludeMenuList(MenuItemEnums.保存);
        }

        #region 添加生成对账单

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }


        #endregion



        #region 转为收付款单1
        public override List<ContextMenuController> AddContextMenu()
        {
            //List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            if (PaymentType == ReceivePaymentType.收款)
            {
                list.Add(new ContextMenuController("【转为收款单】", true, false, "NewSumDataGridView_转为收付款单"));
                list.Add(new ContextMenuController($"【预{PaymentType}抵扣】", true, false, "NewSumDataGridView_预收预付抵扣"));
                list.Add(new ContextMenuController($"【批量智能预{PaymentType}抵扣】", true, false, "NewSumDataGridView_批量智能预收预付抵扣"));
                list.Add(new ContextMenuController($"【撤销预{PaymentType}抵扣】", true, false, "NewSumDataGridView_撤销预收预付抵扣"));
                list.Add(new ContextMenuController($"【快捷全额{PaymentType}】", true, false, "NewSumDataGridView_快捷全额收付款"));
            }
            else
            {
                list.Add(new ContextMenuController("【转为付款单】", true, false, "NewSumDataGridView_转为收付款单"));
                list.Add(new ContextMenuController($"【预{PaymentType}抵扣】", true, false, "NewSumDataGridView_预收预付抵扣"));
                list.Add(new ContextMenuController($"【批量智能预{PaymentType}抵扣】", true, false, "NewSumDataGridView_批量智能预收预付抵扣"));
                list.Add(new ContextMenuController($"【撤销预{PaymentType}抵扣】", true, false, "NewSumDataGridView_撤销预收预付抵扣"));
                list.Add(new ContextMenuController($"【快捷全额{PaymentType}】", true, false, "NewSumDataGridView_快捷全额收付款"));
            }
            //对账中心处理，因为要控制是否能生成对账单
            //list.Add(new ContextMenuController("【生成对账单】", true, false, "NewSumDataGridView_生成对账单"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为收付款单);
            //ContextClickList.Add(NewSumDataGridView_生成对账单);
            ContextClickList.Add(NewSumDataGridView_预收预付抵扣);
            ContextClickList.Add(NewSumDataGridView_批量智能预收预付抵扣);
            ContextClickList.Add(NewSumDataGridView_撤销预收预付抵扣);
            ContextClickList.Add(NewSumDataGridView_快捷全额收付款);
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

        private async void NewSumDataGridView_快捷全额收付款(object sender, EventArgs e)
        {

            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为收款单
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    if (item.AllowAddToStatement)
                    {
                        RealList.Add(item);

                    }
                    else
                    {
                        msg.Append(counter.ToString() + ") ");
                        msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】,已经加入了对账单，不能重复支付， 无法生成{PaymentType.ToString()}单。").Append("\r\n");
                        counter++;
                    }
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法生成{PaymentType.ToString()}单。").Append("\r\n");
                    counter++;
                }
            }
            //多选时。要相同客户才能合并到一个收款单
            if (RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append($"多选时，要相同客户才能合并到一个{PaymentType.ToString()}单");
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
                msg.Append($"请至少选择一行数据转为收{PaymentType.ToString()}单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                //查验选中的应收 应付款单 是不是有预收付款的单据。
                //优化提醒。比方：有预收付款，但是没有确认预收款单，也无法预收付款抵扣，而直接去操作快捷收付款。这个顺序不对。
                var receivablePayableController = Startup.GetFromFac<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                foreach (var item in RealList)
                {
                    var checkResult = await receivablePayableController.CheckUnconfirmedPrePaymentExists(item);
                    if (!checkResult.Succeeded)
                    {
                        MessageBox.Show($"【{item.ARAPNo}】有未确认的预收付款单{string.Join(",", checkResult.DataList)}，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }


            }

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            tb_FM_PaymentRecord PaymentRecord = await paymentController.BuildPaymentRecord(RealList);

            if (MessageBox.Show($"{PaymentType.ToString()}金额为:{PaymentRecord.TotalLocalAmount.ToString("#.0000")}元，确定吗？", "金额确认", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return;
            }
            PaymentRecord.Remark = "快捷全额" + PaymentType.ToString();
            PaymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(PaymentRecord, false);
            if (rrs.Succeeded)
            {
                //自动审核收款单
                PaymentRecord.ApprovalOpinions = $"快捷{PaymentType.ToString()}，自动审核";
                PaymentRecord.ApprovalStatus = (int)ApprovalStatus.审核通过;
                PaymentRecord.ApprovalResults = true;
                ReturnResults<tb_FM_PaymentRecord> rrRecord = await paymentController.ApprovalAsync(PaymentRecord);
                if (!rrRecord.Succeeded)
                {
                    MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_PaymentRecord>($"快捷全额 {PaymentType.ToString()}，自动审核失败：" + rrRecord.ErrorMsg, rrRecord.ReturnObject as tb_FM_PaymentRecord);
                }
                else
                {
                    MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_PaymentRecord>($"快捷全额 {PaymentType.ToString()}，自动审核成功", rrRecord.ReturnObject as tb_FM_PaymentRecord);
                }
            }
        }


        private async void NewSumDataGridView_转为收付款单(object sender, EventArgs e)
        {

            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为收款单
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    //允许对账的，能才直接付款。不允许可能是已经加入了对账
                    if (item.AllowAddToStatement)
                    {
                        RealList.Add(item);

                    }
                    else
                    {
                        msg.Append(counter.ToString() + ") ");
                        msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】,已经加入了对账单，不能重复支付， 无法生成{PaymentType.ToString()}单。").Append("\r\n");
                        counter++;
                    }

                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法生成{PaymentType.ToString()}单。").Append("\r\n");
                    counter++;
                }
            }
            //多选时。要相同客户才能合并到一个收款单
            if (RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append($"多选时，要相同客户才能合并到一个{PaymentType.ToString()}单");
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
                msg.Append($"请至少选择一行数据转为收{PaymentType.ToString()}单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                var receivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                // 2. 查找可抵扣的预收付款单
                var availableAdvances = await receivablePayableController.FindAvailableAdvances(RealList);
                if (availableAdvances.Any())
                {
                    MessageBox.Show($"有可抵扣的预{PaymentType.ToString()}单！,请先进行抵扣操作！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            tb_FM_PaymentRecord ReturnObject = await paymentController.BuildPaymentRecord(RealList);
            tb_FM_PaymentRecord paymentRecord = ReturnObject;
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

            BizType bizType = 0;
            tb_MenuInfo RelatedMenuInfo = null;
            if (PaymentType == ReceivePaymentType.收款)
            {
                bizType = BizType.收款单;
            }
            else
            {
                bizType = BizType.付款单;
            }

            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                 && m.BizType == (int)bizType
                 && m.BIBaseForm == "BaseBillEditGeneric`2")
                     .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, paymentRecord);
                paymentRecord.HasChanged = true;
            }
        }
        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证应收应付单是否可以进行抵扣操作
        /// </summary>
        /// <param name="receivablePayable">应收应付单实体</param>
        /// <param name="message">错误信息</param>
        /// <returns>是否可以抵扣</returns>
        private bool ValidateReceivableForOffset(tb_FM_ReceivablePayable receivablePayable, ref string message)
        {
            if (receivablePayable == null)
            {
                message = "应收应付单信息无效。";
                return false;
            }

            if (receivablePayable.CustomerVendor_ID <= 0)
            {
                message = "应收应付单的往来单位信息不完整，无法进行抵扣操作。";
                return false;
            }

            if (receivablePayable.Currency_ID <= 0)
            {
                message = "应收应付单的币种信息不完整，无法进行抵扣操作。";
                return false;
            }
            //这里逻辑要修改，如果余额等于0才提示无需，
            if (receivablePayable.LocalBalanceAmount == 0)
            {
                message = $"应收应付单【{receivablePayable.ARAPNo}】的余额为零，无需进行抵扣操作。";
                return false;
            }
            //这里逻辑要修改，如果余额等于0才提示无需，
            if (receivablePayable.LocalBalanceAmount < 0)
            {
                message = $"应收应付单【{receivablePayable.ARAPNo}】的余额小于零，不能进行抵扣操作。";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证选中的应收应付单列表
        /// </summary>
        /// <param name="selectedList">选中的应收应付单列表</param>
        /// <param name="validList">有效的应收应付单列表</param>
        /// <param name="message">错误信息</param>
        /// <returns>是否有有效的应收应付单</returns>
        private bool ValidateSelectedReceivables(List<tb_FM_ReceivablePayable> selectedList, out List<tb_FM_ReceivablePayable> validList, out string message)
        {
            validList = new List<tb_FM_ReceivablePayable>();
            message = string.Empty;
            StringBuilder msg = new StringBuilder();
            int counter = 1;

            foreach (var item in selectedList)
            {
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 &&
                                   item.ApprovalStatus == (int)ApprovalStatus.审核通过 &&
                                   item.ApprovalResults.HasValue &&
                                   item.ApprovalResults.Value;

                if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    validList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法进行抵扣。").Append("\r\n");
                    counter++;
                }
            }

            message = msg.ToString();
            return validList.Count > 0;
        }

        /// <summary>
        /// 显示批量操作结果
        /// </summary>
        /// <param name="successCount">成功数量</param>
        /// <param name="successAmount">成功金额</param>
        /// <param name="failedList">失败列表</param>
        /// <param name="operationName">操作名称</param>
        private void ShowBatchOperationResult(int successCount, decimal successAmount, List<string> failedList, string operationName)
        {
            StringBuilder resultMsg = new StringBuilder();
            resultMsg.AppendLine($"{operationName}完成！");
            resultMsg.AppendLine($"成功处理【{successCount}】张单据，金额【{successAmount.ToString("###.00")}】元");

            if (failedList.Any())
            {
                resultMsg.AppendLine();
                resultMsg.AppendLine($"失败【{failedList.Count}】张单据：");
                resultMsg.AppendLine(string.Join("\r\n", failedList));
            }

            MessageBox.Show(resultMsg.ToString(), $"{operationName}结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        //如果销售订单审核，预收款审核后 生成的收款单 在没有审核前。就执行销售出库，这时应收没有及时抵扣时，在这里执行抵扣
        private async void NewSumDataGridView_撤销预收预付抵扣(object sender, EventArgs e)
        {
            //1,查找能抵扣的待核销或部分核销的预收付款单数据集合
            //2,抵扣，更新预收付款单和应收应付记录表
            //3,核销记录

            //抵扣错误时，需要撤销
            //应收应付 状态为 全部支付或部分支付，再到核销表中去找核销记录

            try
            {
                List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
                List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
                StringBuilder msg = new StringBuilder();
                int counter = 1;
                foreach (var item in selectlist)
                {
                    //只有审核状态才可以转换为收款单
                    bool canConvert = item.ARAPStatus == (int)ARAPStatus.全部支付 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                    if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                    {
                        RealList.Add(item);
                    }
                    else
                    {
                        msg.Append(counter.ToString() + ") ");
                        msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法进行撤销抵扣。").Append("\r\n");
                        counter++;
                    }
                }
                //多选时。要相同客户才能合并到一个收款单
                if (RealList.Count() > 1)
                {
                    msg.Append("一次只能选择一行数据进行撤销抵扣操作");
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
                    msg.Append("请至少选择一行数据进行撤销抵扣操作");
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var receivable = RealList[0];

                var receivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                var Settlements = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PaymentSettlement>()
                        .Where(c => c.CustomerVendor_ID == receivable.CustomerVendor_ID)
                        .Where(c => c.Currency_ID == receivable.Currency_ID)
                        .Where(c => c.ReceivePaymentType == receivable.ReceivePaymentType)
                        .Where(c => c.TargetBillId == receivable.ARAPId && c.isdeleted == false)
                        .OrderBy(c => c.SettleDate)
                        .ToListAsync();

                // 2. 查找可撤销抵扣的预收付款单
                var availableAdvances = await receivablePayableController.FindAvailableAntiOffset(receivable, Settlements);
                if (!availableAdvances.Any())
                {
                    MessageBox.Show("没有找到可撤销抵扣操作的预收付款单！");
                    return;
                }

                // 初始化选择器
                using (var selector = new frmAdvanceSelector<tb_FM_PreReceivedPayment>())
                {

                    selector.ConfirmButtonText = "撤销抵扣";
                    selector.AllowMultiSelect = true;
                    // 使用表达式树配置列映射
                    selector.ConfigureColumn(x => x.PreRPNO, "单据编号");
                    selector.ConfigureColumn(x => x.SettledLocalAmount, "核销金额");
                    selector.ConfigureColumn(x => x.LocalPrepaidAmount, "预付金额");
                    selector.ConfigureColumn(x => x.LocalBalanceAmount, "可用金额");
                    selector.ConfigureColumn(x => x.CustomerVendor_ID, "客户");
                    selector.ConfigureColumn(x => x.PrePayDate, "付款日期");
                    selector.ConfigureColumn(x => x.SourceBizType, "来源业务");
                    selector.ConfigureColumn(x => x.SourceBillNo, "来源单号");
                    selector.ConfigureSummaryColumn(x => x.LocalPrepaidAmount);
                    selector.ConfigureSummaryColumn(x => x.LocalBalanceAmount);
                    selector.ConfigureSummaryColumn(x => x.SettledLocalAmount);

                    //预收付款中的核销金额是被核销金额。不一定是当前的应收付款中的核销金额，只有核销记录中才是最准确的，所以这里要处理一下。
                    foreach (var Settlement in Settlements)
                    {
                        if (Settlement.SourceBillId.HasValue)
                        {
                            var antiOffsetPrepaid = availableAdvances.FirstOrDefault(c => c.PreRPID == Settlement.SourceBillId.Value);
                            if (antiOffsetPrepaid != null)
                            {
                                antiOffsetPrepaid.SettledLocalAmount = Settlement.SettledLocalAmount;
                            }

                        }
                    }

                    selector.InitializeSelector(availableAdvances, $"选择要撤销的预{PaymentType}单");



                    if (selector.ShowDialog() == DialogResult.OK)
                    {
                        var selectedAdvances = selector.SelectedItems;

                        //// 将选中单据的PreRPNO字段值用逗号连接成字符串
                        //string preRPNOs = string.Join(", ", selectedAdvances.Select(item => item.PreRPNO));

                        // 显示前10个单据编号，其余用省略号表示
                        string preRPNOsPreview = string.Join(", ",
                            selectedAdvances.Take(5).Select(item => item.PreRPNO));
                        preRPNOsPreview = preRPNOsPreview.TrimEnd(',');
                        if (selectedAdvances.Count > 5)
                        {
                            preRPNOsPreview += $" 等 {selectedAdvances.Count} 张单据\r\n";
                        }
                        if (selectedAdvances.Count > 0)
                        {
                            // 使用选中的预付款单
                            //您确定要将当前应收款单，通过通过预收付款抵扣元吗？
                            if (MessageBox.Show($"您确定要撤销当前【应{PaymentType}单】:{receivable.ARAPNo}\r\n通过【预{PaymentType}单】:{preRPNOsPreview} 抵扣{selectedAdvances.Sum(x => x.SettledLocalAmount).ToString("##.00")}元吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                List<KeyValuePair<tb_FM_PreReceivedPayment, tb_FM_PaymentSettlement>> PrePaySettlements = new List<KeyValuePair<tb_FM_PreReceivedPayment, tb_FM_PaymentSettlement>>();
                                foreach (var item in selectedAdvances)
                                {
                                    var PrePay = availableAdvances.FirstOrDefault(c => c.PreRPID == item.PreRPID);
                                    var Settlement = Settlements.FirstOrDefault(c => c.SourceBillId == PrePay.PreRPID);
                                    PrePaySettlements.Add(new KeyValuePair<tb_FM_PreReceivedPayment, tb_FM_PaymentSettlement>(PrePay, Settlement));
                                }

                                await receivablePayableController.RevokeApplyManualPaymentAllocation(receivable, PrePaySettlements, true);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);
                MessageBox.Show($"预收预付抵扣操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //如果销售订单审核，预收款审核后 生成的收款单 在没有审核前。就执行销售出库，这时应收没有及时抵扣时，在这里执行抵扣
        private async void NewSumDataGridView_预收预付抵扣(object sender, EventArgs e)
        {
            //1,查找能抵扣的待核销或部分核销的预收付款单数据集合
            //2,抵扣，更新预收付款单和应收应付记录表
            //3,核销记录

            try
            {
                List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
                List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
                StringBuilder msg = new StringBuilder();
                int counter = 1;
                foreach (var item in selectlist)
                {
                    //只有审核状态才可以转换为收款单
                    bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                    if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                    {
                        RealList.Add(item);
                    }
                    else
                    {
                        msg.Append(counter.ToString() + ") ");
                        msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法进行抵扣。").Append("\r\n");
                        counter++;
                    }
                }
                //多选时。要相同客户才能合并到一个收款单
                if (RealList.Count() > 1)
                {
                    msg.Append("一次只能选择一行数据进行抵扣");
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
                    msg.Append("请至少选择一行数据进行抵扣");
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var receivable = RealList[0];

                // 数据校验：检查应收应付单的数据完整性
                string validationMessage = string.Empty;
                if (!ValidateReceivableForOffset(receivable, ref validationMessage))
                {
                    MessageBox.Show(validationMessage, "数据校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var receivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                // 2. 查找可抵扣的预收付款单
                var availableAdvances = await receivablePayableController.FindAvailableAdvances(receivable);
                if (!availableAdvances.Any())
                {
                    MessageBox.Show("没有找到可抵扣的预收付款单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    List<string> relatedOrderNos = new List<string>();

                    if (receivable.SourceBizType == (int)BizType.销售出库单)
                    {
                        var saleOutController = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
                        var saleOut = await saleOutController.BaseQueryByIdAsync(receivable.SourceBillId.Value);
                        if (saleOut != null && !string.IsNullOrEmpty(saleOut.SaleOrderNo))
                        {
                            relatedOrderNos.Add(saleOut.SaleOrderNo);
                        }
                    }
                    else if (receivable.SourceBizType == (int)BizType.采购入库单)
                    {
                        var purInboundController = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
                        var purInbound = await purInboundController.BaseQueryByIdAsync(receivable.SourceBillId.Value);
                        if (purInbound != null && !string.IsNullOrEmpty(purInbound.PurOrder_NO))
                        {
                            relatedOrderNos.Add(purInbound.PurOrder_NO);
                        }
                    }

                    if (relatedOrderNos.Any())
                    {
                        availableAdvances = availableAdvances.OrderByDescending(c => relatedOrderNos.Contains(c.SourceBillNo)).ThenBy(c => c.PrePayDate).ToList();
                    }
                    else
                    {
                        availableAdvances = availableAdvances.OrderByDescending(c => c.PrePayDate).ToList();
                    }
                }

                // 检查预收付款单的可用余额是否足够
                decimal totalAvailableAmount = availableAdvances.Sum(x => x.LocalBalanceAmount);
                if (totalAvailableAmount < receivable.LocalBalanceAmount)
                {
                    var confirmResult = MessageBox.Show(
                        $"可抵扣的预收付款单总余额【{totalAvailableAmount.ToString("###.00")}】元小于应收应付单余额【{receivable.LocalBalanceAmount.ToString("###.00")}】元。\r\n" +
                        $"是否继续进行部分抵扣？",
                        "余额不足确认",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.No)
                    {
                        return;
                    }
                }

                // 创建列映射
                var columns = new Dictionary<string, string>
                {
                     { "PreRPNO", "单据编号" },
                     { "LocalPrepaidAmount", "金额" },
                     { "LocalBalanceAmount", "可用金额" },
                     { "CustomerVendor_ID", "客户" },
                     { "PrePayDate", "付款日期" },
                    { "SourceBizType", "来源业务" },
                    { "SourceBillNo", "来源单号" }

                };

                // 初始化选择器
                using (var selector = new frmAdvanceSelector<tb_FM_PreReceivedPayment>())
                {
                    
                    selector.ConfirmButtonText = "抵扣";
                    selector.AllowMultiSelect = true;

                    //selector.InitializeSelector(availableAdvances, columns, $"选择预{PaymentType}单");
                    // 使用表达式树配置列映射
                    selector.ConfigureColumn(x => x.PreRPNO, "单据编号");
                    selector.ConfigureColumn(x => x.LocalPrepaidAmount, "金额");
                    selector.ConfigureColumn(x => x.LocalBalanceAmount, "可用金额");
                    selector.ConfigureColumn(x => x.CustomerVendor_ID, "客户");
                    selector.ConfigureColumn(x => x.PrePayDate, "付款日期");
                    selector.ConfigureColumn(x => x.SourceBizType, "来源业务");
                    selector.ConfigureColumn(x => x.SourceBillNo, "来源单号");
                    selector.ConfigureSummaryColumn(x => x.LocalPrepaidAmount);
                    selector.ConfigureSummaryColumn(x => x.LocalBalanceAmount);
                    selector.InitializeSelector(availableAdvances, $"选择预{PaymentType}单");

                    // 设置金额格式化
                    //selector.SetColumnFormatter("Amount", value => $"{value:N2}");
                    //selector.SetColumnFormatter("RemainAmount", value => $"{value:N2}");

                    if (selector.ShowDialog() == DialogResult.OK)
                    {
                        var selectedAdvances = selector.SelectedItems;

                        //// 将选中单据的PreRPNO字段值用逗号连接成字符串
                        //string preRPNOs = string.Join(", ", selectedAdvances.Select(item => item.PreRPNO));

                        // 显示前10个单据编号，其余用省略号表示
                        string preRPNOsPreview = string.Join(", ",
                            selectedAdvances.Take(5).Select(item => item.PreRPNO));
                        preRPNOsPreview = preRPNOsPreview.TrimEnd(',');
                        if (selectedAdvances.Count > 5)
                        {
                            preRPNOsPreview += $" 等 {selectedAdvances.Count} 张单据\r\n";
                        }
                        if (selectedAdvances.Count > 0)
                        {
                            // 使用选中的预付款单
                            //您确定要将当前应收款单，通过通过预收付款抵扣元吗？
                            if (MessageBox.Show($"当前【应{PaymentType}单】:{receivable.ARAPNo}，未核销余额:{receivable.LocalBalanceAmount.ToString("##.00")}元\r\n通过【预{PaymentType}单】:{preRPNOsPreview} 从{selectedAdvances.Sum(x => x.LocalBalanceAmount).ToString("##.00")}元中进行抵扣{receivable.LocalBalanceAmount.ToString("##.00")}元。\r\n您确定吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                //一一对应检测订单号是否相同
                                //应收款的出库单的订单号
                                List<tb_SaleOut> SaleOutList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                                   .Includes(c => c.tb_saleorder)
                                  .Where(c => c.SaleOut_MainID == receivable.SourceBillId)
                                  .ToListAsync();
                                long[] SaleOrderIds = SaleOutList.Select(c => c.tb_saleorder.SOrder_ID).ToArray();

                                //选择的预收款对应的订单号
                                long[] PreOrderIds = selectedAdvances.Where(c => c.SourceBizType == (int)BizType.销售订单).Select(c => c.SourceBillId.Value).ToArray();
                                var saleOrderSet = new HashSet<long>(SaleOrderIds);
                                var preOrderSet = new HashSet<long>(PreOrderIds);
                                if (!saleOrderSet.SetEquals(preOrderSet))
                                {
                                    if (MessageBox.Show($"当前【应{PaymentType}单】:{receivable.ARAPNo}与【预{PaymentType}单】：{preRPNOsPreview}的订单号不一致，你确实要抵扣吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                    {
                                        await receivablePayableController.ApplyManualPaymentAllocation(receivable, selectedAdvances);
                                    }
                                }
                                else
                                {
                                    await receivablePayableController.ApplyManualPaymentAllocation(receivable, selectedAdvances);
                                }



                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);
                MessageBox.Show($"撤销预收预付抵扣操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //如果销售订单审核，预收款审核后 生成的收款单 在没有审核前。就执行销售出库，这时应收没有及时抵扣时，在这里执行抵扣
        //批量智能预收预付抵扣：支持多个应收应付单批量抵扣，每个应收应付单可使用多个预收付款单进行抵扣
        private async void NewSumDataGridView_批量智能预收预付抵扣(object sender, EventArgs e)
        {
            //1,查找能抵扣的待核销或部分核销的预收付款单数据集合
            //2,抵扣，更新预收付款单和应收应付记录表
            //3,核销记录

            try
            {
                List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
                List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
                string message;

                if (!ValidateSelectedReceivables(selectlist, out RealList, out message))
                {
                    MessageBox.Show(message, "数据校验", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (RealList.Count == 0)
                {
                    MessageBox.Show("请至少选择一行数据进行抵扣", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var receivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                // 2. 为每个应收应付单查找可抵扣的预收付款单
                Dictionary<tb_FM_ReceivablePayable, List<tb_FM_PreReceivedPayment>> receivableToAdvancesMap = new Dictionary<tb_FM_ReceivablePayable, List<tb_FM_PreReceivedPayment>>();
                StringBuilder sbOffset = new StringBuilder();
                List<tb_FM_ReceivablePayable> invalidReceivables = new List<tb_FM_ReceivablePayable>();

                foreach (var receivable in RealList)
                {
                    // 数据校验：检查应收应付单的数据完整性
                    string validationMessage = string.Empty;
                    if (!ValidateReceivableForOffset(receivable, ref validationMessage))
                    {
                        invalidReceivables.Add(receivable);
                        sbOffset.Append($"应{PaymentType.ToString()}单{receivable.ARAPNo}:{validationMessage}\r\n");
                        continue;
                    }

                    // 查找可抵扣的预收付款单（使用单个抵扣的逻辑）
                    var availableAdvances = await receivablePayableController.FindAvailableAdvances(receivable);
                    if (availableAdvances.Any())
                    {
                        receivableToAdvancesMap.Add(receivable, availableAdvances);

                        // 构建抵扣信息
                        sbOffset.Append($"应{PaymentType.ToString()}单{receivable.ARAPNo}:余额{receivable.LocalBalanceAmount.ToString("###.00")}元=>");
                        sbOffset.Append(string.Join("、", availableAdvances.Select(a => $"{a.PreRPNO}:{a.LocalBalanceAmount.ToString("###.00")}")));
                        sbOffset.Append("\r\n");
                    }
                    else
                    {
                        invalidReceivables.Add(receivable);
                        sbOffset.Append($"应{PaymentType.ToString()}单{receivable.ARAPNo}:没有找到可抵扣的预{PaymentType.ToString()}单\r\n");
                    }
                }

                if (!receivableToAdvancesMap.Any())
                {
                    string msgTips = string.Empty;
                    if (!_UCBillMasterQuery.newSumDataGridViewMaster.UseSelectedColumn)
                    {
                        msgTips = "请使用【多选模式】，选择要抵扣的单据。";
                    }

                    MessageBox.Show($"没有找到可抵扣的预收付款单！{msgTips}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //
                if (invalidReceivables.Any())
                {
                    int skipCount = invalidReceivables.Count();
                    if (MessageBox.Show($"共有 {skipCount} 张应{PaymentType.ToString()}单没有找到可抵扣的预{PaymentType.ToString()}单，将跳过。其他单据可以进行抵扣，是否继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                if (MessageBox.Show($"{sbOffset.ToString()}你确定进行对应抵扣吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    int successCount = 0;
                    decimal successAmount = 0;
                    List<string> failedList = new List<string>();

                    foreach (var kvp in receivableToAdvancesMap)
                    {
                        var receivable = kvp.Key;
                        var advances = kvp.Value;

                        try
                        {
                            // 执行抵扣操作（与单个抵扣相同的逻辑）
                            bool result = await receivablePayableController.ApplyManualPaymentAllocation(receivable, advances);
                            if (result)
                            {
                                successCount++;
                                successAmount += receivable.LocalBalanceAmount;
                                MainForm.Instance.PrintInfoLog($"成功抵扣应{PaymentType.ToString()}单：{receivable.ARAPNo}");
                            }
                            else
                            {
                                failedList.Add($"应{PaymentType.ToString()}单{receivable.ARAPNo}");
                                MainForm.Instance.PrintInfoLog($"抵扣失败应{PaymentType.ToString()}单：{receivable.ARAPNo}");
                            }
                        }
                        catch (Exception ex)
                        {
                            failedList.Add($"应{PaymentType.ToString()}单{receivable.ARAPNo}：{ex.Message}");
                            MainForm.Instance.logger.Error(ex, $"批量抵扣异常应{PaymentType.ToString()}单：{receivable.ARAPNo}");
                        }
                    }

                    // 显示批量操作结果
                    ShowBatchOperationResult(successCount, successAmount, failedList, "批量智能预收预付抵扣");

                    // 刷新数据
                    if (successCount > 0)
                    {
                        // 刷新查询数据
                        if (_UCBillMasterQuery != null)
                        {
                            base.Query(QueryDtoProxy);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);
                MessageBox.Show($"批量抵扣操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
        //按客户生成对账单
        private async void NewSumDataGridView_生成对账单(object sender, EventArgs e)
        {

            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为收款单
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法生成{PaymentType.ToString()}对账单。").Append("\r\n");
                    counter++;
                }
            }

            if (RealList.GroupBy(g => g.Currency_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append($"多选时，币别相同才能合并到一个{PaymentType.ToString()}对账单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //多选时。要相同客户才能合并到一个收款单
            if (RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append($"多选时，要相同客户才能合并到一个{PaymentType.ToString()}对账单");
                #region 显示多个客户抬头

                // 显示前10个单据编号，其余用省略号表示
                string CustomerVendors = string.Join(", ",
                    RealList.Select(item => item.tb_customervendor).Distinct().Select(c => c.CVName));
                CustomerVendors = CustomerVendors.TrimEnd(',');
                int Count = RealList.Select(item => item.tb_customervendor).Distinct().Count();
                //if (RealList.Select(item => item.CustomerVendor_ID).Count() > 5)
                //{
                //    CustomerVendors += $" 等 {CustomerVendors.Count} 张单据\r\n";
                //}

                #endregion
                if (MessageBox.Show(msg.ToString(), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    //您已选择 N 个客户，生成对账单时将合并这些客户的往来数据，是否继续？
                    MessageBox.Show($"您已选择 {Count} 个客户:{CustomerVendors}，生成对账单时将合并这些客户的往来数据，请确认是否继续？", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
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
                msg.Append($"请至少选择一行数据转为收{PaymentType.ToString()}对账单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementController<tb_FM_Statement>>();
            tb_FM_Statement statement = await paymentController.BuildStatement(RealList, PaymentType);

            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

            BizType bizType = 0;
            tb_MenuInfo RelatedMenuInfo = null;
            string classpath = string.Empty;
            if (PaymentType == ReceivePaymentType.收款)
            {
                bizType = BizType.收款对账单;
                classpath= "RUINORERP.UI.FM.UCReceiptStatement";
            }
            else
            {
                bizType = BizType.付款对账单;
                classpath = "RUINORERP.UI.FM.UCPaymentStatement";
            }
            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                 && m.BizType == (int)bizType
                 && m.ClassPath == classpath)
                     .FirstOrDefault();

            if (RelatedMenuInfo != null)
            {
                await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, statement);
            }
        }
        */
        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TaxTotalAmount);

            base.MasterSummaryCols.Add(c => c.TotalLocalPayableAmount);
            base.MasterSummaryCols.Add(c => c.TotalForeignPayableAmount);

            base.MasterSummaryCols.Add(c => c.ForeignBalanceAmount);
            base.MasterSummaryCols.Add(c => c.LocalBalanceAmount);

            base.MasterSummaryCols.Add(c => c.ForeignPaidAmount);
            base.MasterSummaryCols.Add(c => c.LocalPaidAmount);
            base.ChildSummaryCols.Add(c => c.LocalPayableAmount);
            base.ChildSummaryCols.Add(c => c.TaxLocalAmount);
            base.ChildSummaryCols.Add(c => c.Quantity);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ARAPId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
            if (PaymentType == ReceivePaymentType.收款)
            {
                //应收款，不需要对方的收款信息。付款才要显示
                base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
                base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            }
        }



        private void UCReceivablePayableQuery_Load(object sender, EventArgs e)
        {

            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_ReceivablePayable>(c => c.SourceBizType, c => c.SourceBillNo);
            // 使用EntityMappingHelper代替BizTypeMapper
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                var tableName = EntityMappingHelper.GetEntityType((BizType)biztype);
                if (tableName == null)
                {
                    continue;
                }
                ////这个参数中指定要双击的列单号。是来自另一组  一对一的指向关系
                //因为后面代码去查找时，直接用的 从一个对象中找这个列的值。但是枚举显示的是名称。所以这里直接传入枚举的值。
                KeyNamePair keyNamePair = new KeyNamePair(((int)((BizType)biztype)).ToString(), tableName.Name);
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_ReceivablePayable>(c => c.SourceBillNo, keyNamePair);
            }

            #endregion

        }
    }
}
