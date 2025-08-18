using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;

using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using SqlSugar;
using Krypton.Navigator;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.SuperSocketClient;
using TransInstruction;
using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;
using RUINORERP.UI.ToolForm;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;

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
                .AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)
                .AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)
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
            ToolStripButton toolStripButton生成对账单 = new System.Windows.Forms.ToolStripButton();
            toolStripButton生成对账单.Text = "生成对账单";
            toolStripButton生成对账单.Image = global::RUINORERP.UI.Properties.Resources.MakeSureCost;
            toolStripButton生成对账单.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton生成对账单.Name = "生成对账单MakesureCost";
            toolStripButton生成对账单.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton生成对账单);
            toolStripButton生成对账单.ToolTipText = "生成对账单。";
            toolStripButton生成对账单.Click += new System.EventHandler(this.toolStripButton生成对账单_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton生成对账单 };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }

        private void toolStripButton生成对账单_Click(object sender, EventArgs e)
        {
            /*后面再设计一个对账功能菜单？生成的回写一下？ 搞一个对账的表？*/
            /*
            if (base.bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
            {
                //  弹出提示说：您确定将这个公司回收投入到公海吗？
                if (bindingSourceList.Current is View_Inventory ViewInventory)
                {
                    var amountRule = new AmountValidationRule();
                    using (var inputForm = new frmInputObject(amountRule))
                    {
                        inputForm.DefaultTitle = "请输入成本价格";
                        tb_Inventory Inventory = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>().Where(c => c.Inventory_ID == ViewInventory.Inventory_ID).SingleAsync();
                        if (inputForm.ShowDialog() == DialogResult.OK)
                        {
                            if (MessageBox.Show($"确定将产品:【{ViewInventory.SKU + "-" + ViewInventory.CNName}】库存成本设为：{inputForm.InputContent}吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                Inventory.CostFIFO = inputForm.InputContent.ToDecimal();
                                Inventory.CostMonthlyWA = inputForm.InputContent.ToDecimal();
                                Inventory.CostMovingWA = inputForm.InputContent.ToDecimal();
                                Inventory.Inv_AdvCost = inputForm.InputContent.ToDecimal();
                                Inventory.Inv_Cost = inputForm.InputContent.ToDecimal();

                                int result = await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(Inventory).UpdateColumns(t => new { t.CostFIFO, t.CostMonthlyWA, t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                                if (result > 0)
                                {
                                    MainForm.Instance.ShowStatusText("生成对账单成功!");
                                    
                                }
                            }
                        }
                    }

                }
            }
            */


        }


        #endregion



        #region 转为收付款单
        public override List<ContextMenuController> AddContextMenu()
        {
            //List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            if (PaymentType == ReceivePaymentType.收款)
            {
                list.Add(new ContextMenuController("【转为收款单】", true, false, "NewSumDataGridView_转为收付款单"));
            }
            else
            {
                list.Add(new ContextMenuController("【转为付款单】", true, false, "NewSumDataGridView_转为收付款单"));
            }
            list.Add(new ContextMenuController("【生成对账单】", true, false, "NewSumDataGridView_生成对账单"));
            list.Add(new ContextMenuController($"【预{PaymentType}抵扣】", true, false, "NewSumDataGridView_预收预付抵扣"));
            list.Add(new ContextMenuController($"【批量智能预{PaymentType}抵扣】", true, false, "NewSumDataGridView_批量智能预收预付抵扣"));
            list.Add(new ContextMenuController($"【撤销预{PaymentType}抵扣】", true, false, "NewSumDataGridView_撤销预收预付抵扣"));
            list.Add(new ContextMenuController($"【快捷全额{PaymentType}】", true, false, "NewSumDataGridView_快捷全额收付款"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为收付款单);
            ContextClickList.Add(NewSumDataGridView_生成对账单);
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
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    RealList.Add(item);
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

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            tb_FM_PaymentRecord PaymentRecord = paymentController.BuildPaymentRecord(RealList);
            PaymentRecord.Remark = "快捷全额" + PaymentType.ToString();
            PaymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(PaymentRecord, false);
            if (rrs.Succeeded)
            {
                //自动审核收款单
                PaymentRecord.ApprovalOpinions = $"快捷{PaymentType.ToString()}，自动审核";
                PaymentRecord.ApprovalStatus = (int)ApprovalStatus.已审核;
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


        private void NewSumDataGridView_转为收付款单(object sender, EventArgs e)
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

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            tb_FM_PaymentRecord ReturnObject = paymentController.BuildPaymentRecord(RealList);
            tb_FM_PaymentRecord paymentRecord = ReturnObject;
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

            string Flag = string.Empty;
            if (PaymentType == ReceivePaymentType.收款)
            {
                Flag = typeof(RUINORERP.UI.FM.UCFMReceivedRecord).FullName;
            }
            else
            {
                Flag = typeof(RUINORERP.UI.FM.UCFMPaymentRecord).FullName;
            }

            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
        && m.EntityName == nameof(tb_FM_PaymentRecord)
        && m.BIBaseForm == "BaseBillEditGeneric`2" && m.ClassPath == Flag)
            .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, paymentRecord);
            }
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
                    bool canConvert = item.ARAPStatus == (int)ARAPStatus.全部支付 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
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
                    bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
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
                var receivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                var receivable = RealList[0];

                // 2. 查找可抵扣的预收付款单
                var availableAdvances = await receivablePayableController.FindAvailableAdvances(receivable);
                if (!availableAdvances.Any())
                {
                    MessageBox.Show("没有找到可抵扣的预收付款单！");
                    return;
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
                            if (MessageBox.Show($"当前【应{PaymentType}单】:{receivable.ARAPNo}，未核销余额:{receivable.LocalBalanceAmount.ToString("##.00")}元\r\n通过【预{PaymentType}单】:{preRPNOsPreview} 抵扣{selectedAdvances.Sum(x => x.LocalBalanceAmount).ToString("##.00")}元。\r\n您确定吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
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

            }
        }



        //如果销售订单审核，预收款审核后 生成的收款单 在没有审核前。就执行销售出库，这时应收没有及时抵扣时，在这里执行抵扣
        private async void NewSumDataGridView_批量智能预收预付抵扣(object sender, EventArgs e)
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
                    bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
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
                var receivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                // 2. 查找可抵扣的预收付款单
                var availableAdvances = await receivablePayableController.FindAvailableAdvances(RealList);
                if (availableAdvances.Any())
                {

                    StringBuilder sbOffset = new StringBuilder();
                    foreach (var item in availableAdvances)
                    {
                        sbOffset.Append($"{item.Key.ARAPNo}:金额{item.Key.LocalBalanceAmount.ToString("###.00")}===》{item.Value.PreRPNO}:金额：{item.Value.LocalBalanceAmount.ToString("###.00")}").Append("\r\n");
                    }

                    if (MessageBox.Show($"{sbOffset.ToString()}你确定进行对应抵扣吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {

                        int OffsetCounter = availableAdvances.Count;
                        decimal OffsetAmount = availableAdvances.Sum(c => c.Key.LocalBalanceAmount);
                        for (int i = 0; i < availableAdvances.Count; i++)
                        {
                            var kv = availableAdvances.ElementAt(i);
                            if (kv.Key.LocalBalanceAmount != kv.Value.LocalBalanceAmount)
                            {
                                OffsetCounter--;
                                OffsetAmount = OffsetAmount - kv.Key.LocalBalanceAmount;
                                MainForm.Instance.PrintInfoLog($"预收付款单抵扣金额不一致，跳过。{kv.Key.ARAPNo}:金额{kv.Key.LocalBalanceAmount.ToString("###.00")}===》{kv.Value.PreRPNO}:金额：{kv.Value.LocalBalanceAmount.ToString("###.00")}");
                                continue;
                            }
                            await receivablePayableController.ApplyManualPaymentAllocation(kv.Key, new List<tb_FM_PreReceivedPayment> { kv.Value });
                        }
                        MessageBox.Show($"批量智能预收预付抵扣，成功匹配【{OffsetCounter}】组！，金额{OffsetAmount}");
                    }
                }
                else
                {
                    MessageBox.Show("没有找到可抵扣的预收付款单！");
                    return;
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);
            }
        }


        //按客户生成对账单
        private void NewSumDataGridView_生成对账单(object sender, EventArgs e)
        {

        }

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
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ARAPId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
            if (PaymentType == ReceivePaymentType.收款)
            {
                //应收款，不需要对方的收款信息。收款才要显示
                base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
                base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            }

        }


        protected override void Delete(List<tb_FM_ReceivablePayable> Datas)
        {
            MessageBox.Show("应收应付记录不能删除？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void UCReceivablePayableQuery_Load(object sender, EventArgs e)
        {

            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_ReceivablePayable>(c => c.SourceBizType, c => c.SourceBillNo);
            BizTypeMapper mapper = new BizTypeMapper();
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                var tableName = mapper.GetTableType((BizType)biztype);
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
