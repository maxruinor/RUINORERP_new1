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


using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global.Model;
using RUINORERP.UI.ToolForm;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using System.Threading;
using System.Linq.Expressions;
using RUINORERP.Business.RowLevelAuthService;
using StatementType = RUINORERP.Global.EnumExt.StatementType;
using System.Runtime.InteropServices;

namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 对账单创建中心
    /// </summary>
    //对账单创建中心
    [MenuAttrAssemblyInfo("对账中心", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.对账管理, BizType.对账单)]
    [SharedIdRequired]
    public partial class UCStatementCreator : BaseBillQueryMC<tb_FM_ReceivablePayable, tb_FM_ReceivablePayableDetail>
    {
        public ReceivePaymentType PaymentType { get; set; }

        // 保存对账类型选择控件的引用
        private Krypton.Toolkit.KryptonComboBox cmbPaymentType;

        public UCStatementCreator()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ARAPNo);


            // 订阅查询条件加载完成事件
            this.Load += UCStatementCreator_Load;
        }

        private void UCStatementCreator_Load(object sender, EventArgs e)
        {
            // 查询条件面板加载完成后，添加对账类型选择框
            AddPaymentTypeToQueryPanel();

        }






        // 保存当前选择的对账类型
        private StatementType CurrentStatementType { get; set; }
        
        // 保存是否合并对账的选项
        private bool IsMergeStatement { get; set; }

        /// <summary>
        /// 动态创建对账类型的下拉选项
        /// </summary>
        private void AddPaymentTypeToQueryPanel()
        {
            try
            {
                // 查找查询条件面板
                var queryPanel = this.Controls.Find("kryptonPanelQuery", true).FirstOrDefault() as Krypton.Toolkit.KryptonPanel;
                if (queryPanel == null || queryPanel.Parent == null)
                    return;

                // 获取查询面板的父容器
                var parentContainer = queryPanel.Parent;

                // 计算新面板和查询面板的位置
                int paymentPanelHeight = 40;
                int originalQueryPanelLocation = queryPanel.Location.Y;

                // 创建新的面板用于放置对账类型选择控件
                Krypton.Toolkit.KryptonPanel paymentTypePanel = new Krypton.Toolkit.KryptonPanel();
                paymentTypePanel.Name = "paymentTypePanel";
                paymentTypePanel.Size = new Size(queryPanel.Width, paymentPanelHeight);
                paymentTypePanel.Location = new Point(queryPanel.Location.X, queryPanel.Location.Y);
                paymentTypePanel.Dock = DockStyle.Top;

                // 创建对账类型选择标签
                Krypton.Toolkit.KryptonLabel label = new Krypton.Toolkit.KryptonLabel();
                label.Text = "对账类型";
                label.Location = new Point(10, 10);
                label.Size = new Size(80, 24);

                // 创建下拉选择框
                cmbPaymentType = new Krypton.Toolkit.KryptonComboBox();
                cmbPaymentType.Location = new Point(100, 8);
                cmbPaymentType.Size = new Size(150, 24);
                cmbPaymentType.Name = "cmbPaymentType";

                // 添加选项，余额对账作为默认选项排在第一位
                cmbPaymentType.Items.Add("余额对账");
                cmbPaymentType.Items.Add("收款对账");
                cmbPaymentType.Items.Add("付款对账");
                cmbPaymentType.SelectedIndex = 0; // 默认选择余额对账
                CurrentStatementType = StatementType.余额对账;

                // 创建合并对账复选框
                Krypton.Toolkit.KryptonCheckBox chkMergeStatement = new Krypton.Toolkit.KryptonCheckBox();
                chkMergeStatement.Text = "合并多个单位对账";
                chkMergeStatement.Location = new Point(260, 10);
                chkMergeStatement.Size = new Size(150, 20);
                chkMergeStatement.Name = "chkMergeStatement";
                chkMergeStatement.Checked = false;
                IsMergeStatement = false;

                // 添加复选框变化事件
                chkMergeStatement.CheckedChanged += (s, e) =>
                {
                    IsMergeStatement = chkMergeStatement.Checked;
                    // 刷新查询条件和数据
                    BuildLimitQueryConditions();
                    QueryConditionBuilder();
                    // 重新执行查询
                    base.QueryDtoProxy = LoadQueryConditionToUI();
                    base.Query(base.QueryDtoProxy);
                };

                // 添加选择变化事件
                cmbPaymentType.SelectedIndexChanged += (s, e) =>
                {
                    // 更新当前对账类型，根据下拉框选择正确映射到枚举值
                    switch (cmbPaymentType.SelectedIndex)
                    {
                        case 0: // 余额对账
                            CurrentStatementType = StatementType.余额对账;
                            PaymentType = ReceivePaymentType.收款; // 余额对账时暂时设为收款
                            break;
                        case 1: // 收款对账
                            CurrentStatementType = StatementType.收款对账;
                            PaymentType = ReceivePaymentType.收款;
                            break;
                        case 2: // 付款对账
                            CurrentStatementType = StatementType.付款对账;
                            PaymentType = ReceivePaymentType.付款;
                            break;
                        default:
                            CurrentStatementType = StatementType.余额对账;
                            PaymentType = ReceivePaymentType.收款;
                            break;
                    }

                    // 刷新查询条件和数据
                    BuildLimitQueryConditions();
                    QueryConditionBuilder();
                    // 重新执行查询
                    base.QueryDtoProxy = LoadQueryConditionToUI();
                    base.Query(base.QueryDtoProxy);
                };

                // 将控件添加到新面板
                paymentTypePanel.Controls.Add(label);
                paymentTypePanel.Controls.Add(cmbPaymentType);
                paymentTypePanel.Controls.Add(chkMergeStatement);

                // 调整查询面板的位置
                queryPanel.Location = new Point(queryPanel.Location.X, queryPanel.Location.Y + paymentPanelHeight);
                queryPanel.Size = new Size(queryPanel.Width, queryPanel.Height - paymentPanelHeight);

                // 将新面板添加到父容器
                parentContainer.Controls.Add(paymentTypePanel);

                // 确保新面板在查询面板之上
                paymentTypePanel.BringToFront();

                // 调整父容器的其他控件位置
                foreach (Control ctrl in parentContainer.Controls)
                {
                    if (ctrl != queryPanel && ctrl != paymentTypePanel && ctrl.Location.Y >= originalQueryPanelLocation)
                    {
                        ctrl.Location = new Point(ctrl.Location.X, ctrl.Location.Y + paymentPanelHeight);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常但不中断程序
                MainForm.Instance.logger.LogError(ex, "添加对账类型选择控件失败");
            }
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// 这里用一套表两套查询条件
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ReceivablePayableProcessorByStatement).Name);
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            // 清除现有条件以避免重复
            QueryConditionFilter.FilterLimitExpressions.Clear();

            // 创建基础查询条件
            var lambdaBuilder = Expressionable.Create<tb_FM_ReceivablePayable>()
                              .And(t => t.isdeleted == false)
                              .And(t => t.AllowAddToStatement == true)
                              .And(t => t.LocalBalanceAmount != 0)
                              .And(t => t.ARAPStatus == (int)ARAPStatus.待审核 || t.ARAPStatus == (int)ARAPStatus.待支付 || t.ARAPStatus == (int)ARAPStatus.部分支付);

            // 根据不同的对账类型添加过滤条件
            if (CurrentStatementType == StatementType.收款对账)
            {
                // 收款对账：只显示收款类型的数据
                lambdaBuilder.And(t => t.ReceivePaymentType == (int)ReceivePaymentType.收款);
            }
            else if (CurrentStatementType == StatementType.付款对账)
            {
                // 付款对账：只显示付款类型的数据
                lambdaBuilder.And(t => t.ReceivePaymentType == (int)ReceivePaymentType.付款);
            }
            // 余额对账：不过滤收付款类型，显示所有类型的数据

            var lambda = lambdaBuilder.ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }



        public override void BuildLimitQueryConditions()
        {
            //这里外层来实现对客户供应商的限制
            string customerVendorId = "".ToFieldName<tb_CustomerVendor>(c => c.CustomerVendor_ID);

            //应收付款中的往来单位额外添加一些条件
            //还是将来利用行级权限来实现？
            var lambdaCv = Expressionable.Create<tb_CustomerVendor>();

            // 根据不同的对账类型设置客户供应商过滤条件
            if (CurrentStatementType == StatementType.收款对账)
            {
                // 收款对账：只显示客户
                lambdaCv.And(t => t.IsCustomer == true);
            }
            else if (CurrentStatementType == StatementType.付款对账)
            {
                // 付款对账：只显示供应商
                lambdaCv.And(t => t.IsVendor == true);
            }
            // 余额对账：不过滤客户/供应商类型，显示所有

            QueryField queryField = QueryConditionFilter.QueryFields.Where(c => c.FieldName == customerVendorId).FirstOrDefault();
            queryField.SubFilter.FilterLimitExpressions.Add(lambdaCv.ToExpression());

            // 创建基础查询条件
            var lambdaBuilder = Expressionable.Create<tb_FM_ReceivablePayable>()
                              .And(t => t.isdeleted == false)
                              .And(t => t.AllowAddToStatement == true)
                              .And(t => t.LocalBalanceAmount != 0)
                              .And(t => t.ARAPStatus == (int)ARAPStatus.待审核 || t.ARAPStatus == (int)ARAPStatus.待支付 || t.ARAPStatus == (int)ARAPStatus.部分支付);

            // 根据不同的对账类型添加过滤条件
            if (CurrentStatementType == StatementType.收款对账)
            {
                // 收款对账：只显示收款类型的数据
                lambdaBuilder.And(t => t.ReceivePaymentType == (int)ReceivePaymentType.收款);
            }
            else if (CurrentStatementType == StatementType.付款对账)
            {
                // 付款对账：只显示付款类型的数据
                lambdaBuilder.And(t => t.ReceivePaymentType == (int)ReceivePaymentType.付款);
            }
            // 余额对账：不过滤收付款类型，显示所有类型的数据

            var lambda = lambdaBuilder.ToExpression();
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





        #region 转为对账款单
        public override List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【生成对账单】", true, false, "NewSumDataGridView_生成对账单"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_生成对账单);
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


        #endregion

        private async void NewSumDataGridView_生成对账单(object sender, EventArgs e)
        {

            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                ReceivePaymentType PaymentType = (ReceivePaymentType)item.ReceivePaymentType;
                //只有审核状态才可以转换为收款单
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
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
                msg.Append($"多选时，币别相同才能合并到一个对账单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //多选时检查是否允许合并多个单位对账
            var distinctCustomerVendors = RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).ToList();
            if (distinctCustomerVendors.Count > 1)
            {
                if (!IsMergeStatement)
                {
                    msg.Append($"多选时，要相同客户才能合并到一个对账单");
                    #region 显示多个客户抬头

                    // 显示前10个单据编号，其余用省略号表示
                    string CustomerVendors = string.Join(", ",
                        RealList.Select(item => item.tb_customervendor).Distinct().Select(c => c.CVName));
                    CustomerVendors = CustomerVendors.TrimEnd(',');
                    int Count = distinctCustomerVendors.Count;

                    #endregion
                    
                    // 提供用户选择是否合并对账
                    var dialogResult = MessageBox.Show($"您已选择 {Count} 个客户：{CustomerVendors}\n\n是否要合并这些客户的往来数据进行对账？\n\n选择【是】将生成合并对账单，选择【否】将只对同一客户进行对账。", 
                        "合并多个单位对账", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (dialogResult == DialogResult.Yes)
                    {
                        IsMergeStatement = true;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        msg.Append($"\n\n请选择同一个客户的往来数据进行对账。");
                        MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
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
                msg.Append($"请至少选择一行数据转为对账单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            tb_FM_Statement statement = new();
            //对账模式，直接使用原始数据，因为已经查询时处理过了。针对余额对账，需要重新计算
            var receivePaymentType = GetStatementType(RealList, CurrentStatementType);

            // 获取调整后的数据副本，不修改原始数据
            List<tb_FM_ReceivablePayable> adjustedList = SetStatementItems(RealList, receivePaymentType, CurrentStatementType);

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementController<tb_FM_Statement>>();
            ReturnResults<tb_FM_Statement> rrs = await paymentController.BuildStatement(adjustedList, receivePaymentType, CurrentStatementType, IsMergeStatement);
            if (rrs.Succeeded)
            {
                statement = rrs.ReturnObject;
                MenuPowerHelper menuPowerHelper;
                menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

                BizType bizType = BizType.对账单;
                tb_MenuInfo RelatedMenuInfo = null;
                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                     && m.BizType == (int)bizType
                     && m.BIBaseForm == "BaseBillEditGeneric`2")
                         .FirstOrDefault();
                if (RelatedMenuInfo != null)
                {
                    await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, statement);
                    statement.HasChanged = true;
                }
            }
            else
            {
                MessageBox.Show(rrs.ErrorMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public override void BuildSummaryCols()
        {
            // 重写汇总逻辑，以支持余额对账模式下的正确汇总
            // 注意：这里只是定义需要汇总的列，实际汇总计算需要在基类中处理
            base.MasterSummaryCols.Add(c => c.TaxTotalAmount);
            base.MasterSummaryCols.Add(c => c.ForeignBalanceAmount);
            base.MasterSummaryCols.Add(c => c.ForeignPaidAmount);
            base.MasterSummaryCols.Add(c => c.ForeignReconciledAmount);
            base.MasterSummaryCols.Add(c => c.TotalForeignPayableAmount);

            base.MasterSummaryCols.Add(c => c.LocalBalanceAmount);
            base.MasterSummaryCols.Add(c => c.LocalPaidAmount);
            base.MasterSummaryCols.Add(c => c.LocalReconciledAmount);
            base.MasterSummaryCols.Add(c => c.TotalLocalPayableAmount);

            base.MasterSummaryCols.Add(c => c.UntaxedTotalAmont);
            base.MasterSummaryCols.Add(c => c.ShippingFee);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.LocalPayableAmount);
            base.ChildSummaryCols.Add(c => c.TaxLocalAmount);

            // 金额调整逻辑现在在重写的Query方法中实现，会在查询结果返回后直接处理数据
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ARAPId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
        }


 

        private void UCReceivablePayableQuery_Load(object sender, EventArgs e)
        {

            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_ReceivablePayable>(c => c.SourceBizType, c => c.SourceBillNo);
            // 使用EntityMappingHelper代替BizTypeMapper
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RealList"></param>
        /// <returns></returns>
        private ReceivePaymentType GetStatementType(List<tb_FM_ReceivablePayable> RealList, StatementType CurrentStatementType)
        {

            /*
             - **总体原则**：如果对账类型为余额对账，则根据选中数据的总余额正负值，系统自动决定生成收款对账单或付款对账单
            - **具体计算方法**：
              - 系统通过对应收款单和应付款单金额进行冲销抵扣，计算出总金额
              - 收款类型数据作为进项，使用加法计算
              - 付款类型数据作为出项，使用减法计算
              - 根据计算得出的总金额正负值确定对账单类型：
                - 总金额为正数时，生成收款对账单
                - 总金额为负数时，生成付款对账单
             */
            //先默认收款，再根据逻辑去算
            ReceivePaymentType statementType = ReceivePaymentType.收款;
            // 处理查询结果
            try
            {
                List<tb_FM_ReceivablePayable> list = RealList ?? new List<tb_FM_ReceivablePayable>();

                // 根据不同的对账类型处理
                if (CurrentStatementType == StatementType.收款对账)
                {
                    // 收款对账直接返回收款类型
                    return ReceivePaymentType.收款;
                }
                else if (CurrentStatementType == StatementType.付款对账)
                {
                    // 付款对账直接返回付款类型
                    return ReceivePaymentType.付款;
                }
                else if (CurrentStatementType == StatementType.余额对账)
                {
                    // 余额对账需要根据RealList中的类型来判断处理方式
                    var receivePaymentTypes = list.Select(x => x.ReceivePaymentType).Distinct().ToList();
                    
                    // 如果只有一种类型，则直接使用该类型
                    if (receivePaymentTypes.Count == 1)
                    {
                        return (ReceivePaymentType)receivePaymentTypes.First();
                    }
                    
                    // 如果有两种类型，则按现有逻辑通过余额对冲后的结果来决定方向
                    List<tb_FM_ReceivablePayable> adjustedList = new List<tb_FM_ReceivablePayable>();
                    foreach (var item in list)
                    {
                        // 创建数据副本以避免修改原始数据
                        var adjustedItem = new tb_FM_ReceivablePayable();

                        // 使用反射复制所有公共属性
                        foreach (var property in typeof(tb_FM_ReceivablePayable).GetProperties())
                        {
                            if (property.CanRead && property.CanWrite)
                            {
                                try
                                {
                                    property.SetValue(adjustedItem, property.GetValue(item));
                                }
                                catch { }
                            }
                        }

                        // 根据收付款类型调整金额正负值
                        if (item.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {
                            // 付款类型直接取负号
                            adjustedItem.TotalLocalPayableAmount = -adjustedItem.TotalLocalPayableAmount;
                            adjustedItem.TotalForeignPayableAmount = -adjustedItem.TotalForeignPayableAmount;
                            adjustedItem.LocalBalanceAmount = -adjustedItem.LocalBalanceAmount;
                            adjustedItem.ForeignBalanceAmount = -adjustedItem.ForeignBalanceAmount;
                            adjustedItem.LocalPaidAmount = -adjustedItem.LocalPaidAmount;
                            adjustedItem.ForeignPaidAmount = -adjustedItem.ForeignPaidAmount;
                            adjustedItem.TaxTotalAmount = -adjustedItem.TaxTotalAmount;
                        }
                        // 收款类型保持原始金额不变

                        adjustedList.Add(adjustedItem);
                    }

                    // 使用调整后的数据列表
                    list = adjustedList;
                    
                    var LastAmount = list.Sum(c => c.LocalBalanceAmount);
                    if (LastAmount > 0)
                    {
                        statementType = ReceivePaymentType.收款;
                    }
                    else if (LastAmount < 0)
                    {
                        statementType = ReceivePaymentType.付款;
                    }
                    else
                    {
                        // 余额为0时，默认使用收款类型
                        statementType = ReceivePaymentType.收款;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "获取对账单类型时发生错误");
            }

            return statementType;
        }

        /// <summary>
        /// 在确定对账类型的基础上来决定数据正负值
        /// 注意：此方法创建数据副本而不是直接修改原始数据，以避免用户重新对账时出现错误
        /// </summary>
        /// <param name="RealList">原始应收付款单列表</param>
        /// <param name=nameof(ReceivePaymentType)>对账单类型</param>
        /// <param name="CurrentStatementType">当前对账模式</param>
        /// <returns>调整后的应收付款单副本列表</returns>
        private List<tb_FM_ReceivablePayable> SetStatementItems(List<tb_FM_ReceivablePayable> RealList, ReceivePaymentType receivePaymentType, StatementType CurrentStatementType)
        {
            // 创建新的列表来存储调整后的数据副本
            List<tb_FM_ReceivablePayable> adjustedList = new List<tb_FM_ReceivablePayable>();

            /*
             - **总体原则**：根据选中数据的总余额正负值，系统自动决定生成收款对账单或付款对账单
            - **具体计算方法**：
              - 系统通过对应收款单和应付款单金额进行冲销抵扣，计算出总金额
              - 收款类型数据作为进项，使用加法计算
              - 付款类型数据作为出项，使用减法计算
              - 根据计算得出的总金额正负值确定对账单类型：
                - 总金额为正数时，生成收款对账单
                - 总金额为负数时，生成付款对账单
             */
            try
            {
                if (RealList == null) return adjustedList;

                // 为每个原始项目创建副本并进行调整
                foreach (var item in RealList)
                {
                    // 创建数据副本以避免修改原始数据
                    var adjustedItem = new tb_FM_ReceivablePayable();

                    // 使用反射复制所有公共属性
                    foreach (var property in typeof(tb_FM_ReceivablePayable).GetProperties())
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            try
                            {
                                property.SetValue(adjustedItem, property.GetValue(item));
                            }
                            catch { }
                        }
                    }

                    // 余额对账模式下，根据收付款类型调整金额正负值
                    if (CurrentStatementType == StatementType.余额对账)
                    {
                        // 意思是：付款时，付款出去是正数，收进来算加负号用来抵扣冲销
                        if (adjustedItem.ReceivePaymentType == (int)ReceivePaymentType.收款 && receivePaymentType == ReceivePaymentType.付款)
                        {
                            // 收款类型直接取负号
                            adjustedItem.TotalLocalPayableAmount = -adjustedItem.TotalLocalPayableAmount;
                            adjustedItem.TotalForeignPayableAmount = -adjustedItem.TotalForeignPayableAmount;
                            adjustedItem.LocalBalanceAmount = -adjustedItem.LocalBalanceAmount;
                            adjustedItem.ForeignBalanceAmount = -adjustedItem.ForeignBalanceAmount;
                            adjustedItem.LocalPaidAmount = -adjustedItem.LocalPaidAmount;
                            adjustedItem.ForeignPaidAmount = -adjustedItem.ForeignPaidAmount;
                            adjustedItem.TaxTotalAmount = -adjustedItem.TaxTotalAmount;
                        }
                        else if (adjustedItem.ReceivePaymentType == (int)ReceivePaymentType.付款 && receivePaymentType == ReceivePaymentType.收款)
                        {
                            // 付款类型直接取负号
                            adjustedItem.TotalLocalPayableAmount = -adjustedItem.TotalLocalPayableAmount;
                            adjustedItem.TotalForeignPayableAmount = -adjustedItem.TotalForeignPayableAmount;
                            adjustedItem.LocalBalanceAmount = -adjustedItem.LocalBalanceAmount;
                            adjustedItem.ForeignBalanceAmount = -adjustedItem.ForeignBalanceAmount;
                            adjustedItem.LocalPaidAmount = -adjustedItem.LocalPaidAmount;
                            adjustedItem.ForeignPaidAmount = -adjustedItem.ForeignPaidAmount;
                            adjustedItem.TaxTotalAmount = -adjustedItem.TaxTotalAmount;
                        }
                    }

                    // 添加到调整后的列表
                    adjustedList.Add(adjustedItem);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "创建调整后的对账数据副本时发生错误");
            }

            return adjustedList;
        }


    }
}
