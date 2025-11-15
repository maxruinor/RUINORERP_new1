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
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.CommonUI;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.ToolForm;
using NPOI.SS.Formula.Functions;
using RUINORERP.UI.HelpSystem;

namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("销售订单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售订单)]
    [HelpMapping("forms/UCSaleOrderQuery.html", Title = "销售订单查询")]
    public partial class UCSaleOrderQuery : BaseBillQueryMC<tb_SaleOrder, tb_SaleOrderDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCSaleOrderQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.SOrderNo);
            
            // 在Load事件中启用帮助功能，确保控件已完全初始化
            this.Load += (sender, e) => {
                // 为工具栏按钮设置帮助键
                if (HelpManager.Config.IsHelpSystemEnabled)
                {
                    SetToolbarButtonHelpKeys();
                }
            };
        }
        
        /// <summary>
        /// 处理键盘事件以支持F1帮助
        /// </summary>
        private void UCSaleOrderQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                // 显示当前焦点控件的帮助
                ShowFocusedControlHelp();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                // 显示帮助系统主窗体
                if (HelpManager.Config.IsHelpSystemEnabled)
                {
                    var mainForm = FindMainForm();
                    if (mainForm != null)
                    {
                        mainForm.ShowHelpSystemForm();
                    }
                }
                e.Handled = true;
            }
        }
        
        /// <summary>
        /// 显示当前焦点控件的帮助
        /// </summary>
        private void ShowFocusedControlHelp()
        {
            if (!HelpManager.Config.IsHelpSystemEnabled) return;
            
            try
            {
                // 获取当前焦点控件
                Control focusedControl = this.ActiveControl;
                
                // 如果焦点控件有帮助键，则显示对应的帮助
                if (focusedControl?.Tag is string helpKey && !string.IsNullOrEmpty(helpKey))
                {
                    HelpManager.ShowHelpByKey(helpKey);
                    return;
                }
                
                // 否则显示窗体级别的帮助
                HelpManager.ShowHelpByType(this.GetType());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示帮助时出错: {ex.Message}", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 为工具栏按钮设置帮助键
        /// </summary>
        private void SetToolbarButtonHelpKeys()
        {
            try
            {
                // 为查询工具栏按钮设置帮助键
                if (BaseToolStrip != null)
                {
                    foreach (ToolStripItem item in BaseToolStrip.Items)
                    {
                        if (item is ToolStripButton button)
                        {
                            switch (button.Text)
                            {
                                case "查询":
                                    button.SetControlHelpKey("button_query");
                                    break;
                                case "新增":
                                    button.SetControlHelpKey("button_add");
                                    break;
                                case "修改":
                                    button.SetControlHelpKey("button_edit");
                                    break;
                                case "删除":
                                    button.SetControlHelpKey("button_delete");
                                    break;
                                case "打印":
                                    button.SetControlHelpKey("button_print");
                                    break;
                                case "导出":
                                    button.SetControlHelpKey("button_export");
                                    break;
                                case "结案":
                                    button.SetControlHelpKey("button_close_case");
                                    break;
                            }
                        }
                    }
                }
                
                // 为右键菜单项设置帮助键
                SetContextMenuHelpKeys();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序执行
                System.Diagnostics.Debug.WriteLine($"设置工具栏按钮帮助键时出错: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 为右键菜单项设置帮助键
        /// </summary>
        private void SetContextMenuHelpKeys()
        {
            try
            {
                // 为右键菜单项设置帮助键
                if (_UCBillMasterQuery?.newSumDataGridViewMaster?.ContextMenuStrip != null)
                {
                    foreach (ToolStripItem item in _UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip.Items)
                    {
                        if (item is ToolStripMenuItem menuItem)
                        {
                            switch (menuItem.Text)
                            {
                                case "【标记已打印】":
                                    menuItem.SetControlHelpKey("contextmenu_mark_printed");
                                    break;
                                case "【转为出库单】":
                                    menuItem.SetControlHelpKey("contextmenu_convert_to_outbound");
                                    break;
                                case "【订单取消作废】":
                                    menuItem.SetControlHelpKey("contextmenu_cancel_order");
                                    break;
                                case "【转为采购单】":
                                    menuItem.SetControlHelpKey("contextmenu_convert_to_purchase");
                                    break;
                                case "【预收货款】":
                                    menuItem.SetControlHelpKey("contextmenu_advance_payment");
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序执行
                System.Diagnostics.Debug.WriteLine($"设置右键菜单帮助键时出错: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 查找主窗体
        /// </summary>
        private Form FindMainForm()
        {
            // 首先尝试通过Parent查找
            Control parent = this.Parent;
            while (parent != null)
            {
                if (parent is Form form)
                    return form;
                parent = parent.Parent;
            }
            
            // 如果通过Parent找不到，尝试通过Application.OpenForms查找
            foreach (Form form in Application.OpenForms)
            {
                if (form is RUINORERP.UI.MainForm)
                    return form;
            }
            
            return null;
        }
        
        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为销售出库单);
            ContextClickList.Add(NewSumDataGridView_取消订单);
            ContextClickList.Add(NewSumDataGridView_标记已打印);
            ContextClickList.Add(NewSumDataGridView_转为采购订单);
            ContextClickList.Add(NewSumDataGridView_预收货款);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【标记已打印】", true, false, "NewSumDataGridView_标记已打印"));
            list.Add(new ContextMenuController("【转为出库单】", true, false, "NewSumDataGridView_转为销售出库单"));
            list.Add(new ContextMenuController("【订单取消作废】", true, false, "NewSumDataGridView_取消订单"));
            list.Add(new ContextMenuController("【转为采购单】", true, false, "NewSumDataGridView_转为采购订单"));
            list.Add(new ContextMenuController("【预收货款】", true, false, "NewSumDataGridView_预收货款"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_标记已打印);
            ContextClickList.Add(NewSumDataGridView_转为销售出库单);
            ContextClickList.Add(NewSumDataGridView_取消订单);
            ContextClickList.Add(NewSumDataGridView_转为采购订单);
            ContextClickList.Add(NewSumDataGridView_预收货款);

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
        private async void NewSumDataGridView_预收货款(object sender, EventArgs e)
        {
            //金额，付款方式，收款账号，备注，经办人
            //思路是先输入金额。生成一个单后。再显示出来。让用户可以完善信息。再审核
            try
            {
                List<tb_SaleOrder> selectlist = GetSelectResult();
                List<tb_SaleOrder> RealList = new List<tb_SaleOrder>();
                StringBuilder msg = new StringBuilder();
                int counter = 1;
                foreach (var item in selectlist)
                {
                    //只有审核状态才可以转换为收款单
                    if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                    {
                        RealList.Add(item);
                    }
                    else
                    {
                        msg.Append(counter.ToString() + ") ");
                        msg.Append($"当前销售订单 {item.SOrderNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法进行再次预收款。").Append("\r\n");
                        counter++;
                    }
                }
                //多选时。要相同客户才能合并到一个收款单
                if (RealList.Count() > 1)
                {
                    msg.Append("一次只能选择一行数据进行预收款。");
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
                    msg.Append("请至少选择一行数据进行预收款。");
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var SOrder = RealList[0];
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
                        if (SOrder.Deposit + PreAmount > SOrder.TotalAmount)
                        {
                            if (MessageBox.Show($"【销售订单】原有预付款金额{SOrder.Deposit}+当前预付款：{PreAmount}，超过了订单总金额{SOrder.TotalAmount}，你确定客户有超额付款吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton: MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return;
                            }
                        }

                        if (MessageBox.Show($"针对订单：{SOrder.SOrderNo}，确定收到客户{SOrder.tb_customervendor.CVName}:收到预付款：{inputForm.InputContent}元吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                            var rs = await ctr.ManualPrePayment(inputForm.InputContent.ObjToDecimal(), SOrder);
                            MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                            string Flag = string.Empty;
                            Flag = SharedFlag.Flag1.ToString();
                            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                            && m.EntityName == nameof(tb_FM_PreReceivedPayment) && m.BIBaseForm == "BaseBillEditGeneric`2" && m.UIPropertyIdentifier == Flag).FirstOrDefault();
                            if (RelatedMenuInfo != null)
                            {
                                await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, rs.ReturnObject);
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
            List<tb_SaleOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                item.PrintStatus++;
                tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                await ctr.SaveOrUpdate(item);
            }
        }


        //里面代码是转出库单的，还没有实现。后面再实现TODO!!!
        private void NewSumDataGridView_转为采购订单(object sender, EventArgs e)
        {
            List<tb_SaleOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_PurOrders != null && item.tb_PurOrders.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过采购单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }

                    tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                    tb_PurOrder purOrder = ctr.SaleOrderToPurOrder(item);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_PurOrder) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, purOrder);
                    }
                    return;
                }
                else
                {
                    if (item.DataStatus == (int)DataStatus.完结)
                    {
                        // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                        MessageBox.Show($"当前订单{item.SOrderNo}：已结案，无法生成采购单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (item.DataStatus == (int)DataStatus.草稿 || item.DataStatus == (int)DataStatus.新建)
                    {
                        // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                        MessageBox.Show($"当前订单{item.SOrderNo}：未审核，无法生成采购单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
            }
        }

        private async void NewSumDataGridView_转为销售出库单(object sender, EventArgs e)
        {
            List<tb_SaleOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOuts != null && item.tb_SaleOuts.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过出库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }

                    tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                    //tb_SaleOut saleOut = SaleOrderToSaleOut(item);
                    tb_SaleOut saleOut =await ctr.SaleOrderToSaleOut(item);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_SaleOut) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                     await   menuPowerHelper.ExecuteEvents(RelatedMenuInfo, saleOut);
                    }
                    return;
                }
                else
                {
                    if (item.DataStatus == (int)DataStatus.完结)
                    {
                        // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                        MessageBox.Show($"当前订单{item.SOrderNo}：已结案，无法生成出库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (item.DataStatus == (int)DataStatus.草稿 || item.DataStatus == (int)DataStatus.新建)
                    {
                        // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                        MessageBox.Show($"当前订单{item.SOrderNo}：未审核，无法生成出库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
            }
        }





        /// <summary>
        /// 订单取消作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NewSumDataGridView_取消订单(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要执行【订单取消作废】操作吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            List<tb_SaleOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //订单已结案，无法取消，订单已经出库无法直接取消
                //只有审核状态才可以转换为出库单
                if (item.DataStatus != (int)DataStatus.完结 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOuts != null && item.tb_SaleOuts.Count > 0)
                    {
                        MessageBox.Show($"当前订单：{item.SOrderNo},已经生成过出库单，\r\n无法取消作废,只能销售退回处理！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //取消订单的原因
                    var CancelReasonRule = new LengthAndChineseValidationRule
                    {
                        MinLength = 5,
                        MaxLength = 500,
                        MaxChineseChars = 50   // 例如只让最多 50 个汉字
                    };

                    
                    using (var inputForm = new frmInputObject(CancelReasonRule))
                    {
                        inputForm.DefaultTitle = "请输入订单【取消作废】的原因";
                        if (inputForm.ShowDialog() == DialogResult.OK)
                        {
                            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                            //如果订单状态是审核状态，则取消订单
                            //审核状态时取消订单 有退款则退款
                            //如果有预付款，则取消订单时，需要退款

                            ReturnResults<tb_SaleOrder> rmrs = await ctr.CancelOrder(item, inputForm.InputContent);
                            if (rmrs.Succeeded)
                            {
                                await MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOrder>("取消订单【作废】", rmrs.ReturnObject, $"结果:{(rmrs.Succeeded ? "成功" : "失败")},{rmrs.ErrorMsg}");
                            }
                            else
                            {
                                MessageBox.Show($"当前订单：{item.SOrderNo}{rmrs.ErrorMsg}！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                }
                else
                {
                    if (item.DataStatus == (int)DataStatus.完结)
                    {
                        // 弹出提示窗口：没有审核的销售订单， 
                        MessageBox.Show($"当前订单{item.SOrderNo}：已结案，无法直接取消订单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (item.DataStatus == (int)DataStatus.草稿 || item.DataStatus == (int)DataStatus.新建)
                    {
                        // 弹出提示窗口：没有审核的销售订单， 
                        MessageBox.Show($"当前订单{item.SOrderNo}：未审核，可以直接删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }

                }
            }
        }


    
        /// <summary>
        /// 可以通过  注册 数字要"引号" 转换不然会失败
        /// </summary>
        private void lambdaTest()
        {
            // 构造 lambda 表达式的字符串形式
            string exprString = "t => t.Employee_ID == \"121\" && t.DepartmentId == \"11\"";
            // 解析 lambda 表达式字符串，生成表达式树
            var parameter = Expression.Parameter(typeof(tb_SaleOrder), "t");
            var lambdaExpr = DynamicExpressionParser.ParseLambda(new[] { parameter }, typeof(bool), exprString);

            // 编译表达式树为委托
            var func = (Func<tb_SaleOrder, bool>)lambdaExpr.Compile();

            var lambda = Expressionable.Create<tb_SaleOrder>();
            var user = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            var menu = CurMenuInfo;


            // 示例值（根据实际类型调整）
            var resolvedExpr = "t => t.Employee_ID == \"112\" && t.DepartmentId == \"3121\"";

            // 解析为 Lambda 表达式
            var config = new ParsingConfig
            {
                ResolveTypesBySimpleName = true,
                CustomTypeProvider = new DefaultDynamicLinqCustomTypeProvider() // 确保识别自定义类型
            };

            var paramT = Expression.Parameter(typeof(tb_SaleOrder), "t");
            var filterExprLambda = DynamicExpressionParser.ParseLambda(
                config,
                new[] { paramT },
                typeof(bool),
                resolvedExpr
            ) as Expression<Func<tb_SaleOrder, bool>>;

            lambda.And(filterExprLambda);
            lambda.And(t => t.isdeleted == false);

            // 验证生成的表达式
            Console.WriteLine(lambda.ToExpression().ToString());
            // 输出示例: t => ((t.Employee_ID == 121) && (t.DepartmentId == 11)) && (t.isdeleted == False)
        }

        // 占位符替换方法
        private string ReplacePlaceholders(string expr, tb_MenuInfo menu, tb_UserInfo user)
        {
            return expr
                //.Replace("@UserId", user.User_ID.ToString())
                .Replace("@EmployeeId", user.Employee_ID.ToString())
                .Replace("@DepartmentId", user.tb_employee.tb_department.ID.ToString());
        }
        private string ReplacePlaceholders(string expr, tb_SaleOrder t)
        {
            return expr
                .Replace("@EmployeeId", t.Employee_ID.ToString())
                .Replace("@DepartmentId", t.tb_employee.tb_department.ID.ToString());
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_SaleOrder>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            base.MasterSummaryCols.Add(c => c.ForeignTotalAmount);
            base.MasterSummaryCols.Add(c => c.FreightIncome);
            base.MasterSummaryCols.Add(c => c.TotalCommissionAmount);
            base.MasterSummaryCols.Add(c => c.Deposit);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.CommissionAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTaxAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTransAmount);

        }

        public override void BuildInvisibleCols()
        {
            //base.MasterInvisibleCols.Add(c => c.TotalCost);
            //base.ChildInvisibleCols.Add(c => c.Cost);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


        /// <summary>
        /// 批量转换为销售出库单
        /// </summary>
        public async Task BatchConversion()
        {
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            List<tb_SaleOrder> selectlist = GetSelectResult();
            int conter = 0;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOuts != null && item.tb_SaleOuts.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过出库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }

                    }

                    tb_SaleOrderController<tb_SaleOrder> ctrOrder = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                    tb_SaleOut saleOut =await ctrOrder.SaleOrderToSaleOut(item);

                    ReturnMainSubResults<tb_SaleOut> rsrs = await ctr.BaseSaveOrUpdateWithChild<tb_SaleOut>(saleOut);
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
                    MainForm.Instance.uclog.AddLog(string.Format("当前订单:{0}的状态为{1},不能转换为销售出库单。", item.SOrderNo, ((DataStatus)item.DataStatus).ToString()));
                    continue;
                }

            }
            MainForm.Instance.uclog.AddLog("转换完成,成功订单数量:" + conter);
        }

        /* 放到了 订单的业务层 
        /// <summary>
        /// 转换为销售出库单
        /// </summary>
        /// <param name="saleorder"></param>
        public tb_SaleOut SaleOrderToSaleOut(tb_SaleOrder saleorder)
        {
            tb_SaleOut entity = new tb_SaleOut();
            //转单
            if (saleorder != null)
            {
                 
                entity = mapper.Map<tb_SaleOut>(saleorder);
                List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
                //转单要TODO
                //转换时，默认认为订单出库数量就等于这次出库数量，是否多个订单累计？，如果是UI录单。则只是默认这个数量。也可以手工修改
                List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = details[i].Quantity - item.TotalDeliveredQty;// 减掉已经出库的数量
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    details[i].SubtotalCostAmount = details[i].Cost * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                }

                entity.tb_SaleOutDetails = NewDetails;
                entity.ApprovalOpinions = "批量转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                if (NewDetails.Count != details.Count)
                {
                    //已经出库过，第二次不包括 运费
                    entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                    entity.TotalCost = NewDetails.Sum(c => c.Cost * c.Quantity);
                    entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                    entity.TotalUntaxedAmount = NewDetails.Sum(c => c.SubtotalUntaxedAmount);
                }

                if (saleorder.DeliveryDate.HasValue)
                {
                    entity.OutDate = saleorder.DeliveryDate.Value;
                    entity.DeliveryDate = saleorder.DeliveryDate;
                }
                else
                {
                    entity.OutDate = System.DateTime.Now;
                    entity.DeliveryDate = System.DateTime.Now;
                }

                BusinessHelper.Instance.InitEntity(entity);

                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = saleorder.CustomerVendor_ID;
                    entity.SaleOrderNo = saleorder.SOrderNo;
                }
                entity.SaleOutNo = BizCodeService.GetBizBillNo(BizType.销售出库单);
                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }
        */

        //public override List<tb_SaleOrder> GetPrintDatas(List<tb_SaleOrder> EditEntitys)
        //{
        //    List<tb_SaleOrder> datas = new List<tb_SaleOrder>();
        //    foreach (var item in EditEntitys)
        //    {
        //        tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
        //        var PrintData = ctr.GetPrintData(item.SOrder_ID);
        //        datas.Add(PrintData[0] as tb_SaleOrder);
        //    }
        //    return datas;
        //}


        //public override List<tb_SaleOrder> GetPrintDatas(tb_SaleOrder EditEntity)
        //{
        //    List<tb_SaleOrder> datas = new List<tb_SaleOrder>();
        //    tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
        //    List<tb_SaleOrder> PrintData = ctr.GetPrintData(EditEntity.SOrder_ID);
        //    return PrintData;
        //}


        /*
 /// <summary>
 /// 销售订单审核，审核成功后，库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
 /// </summary>
 /// <returns></returns>
 protected async override Task<ApprovalEntity> Review(tb_SaleOrder EditEntity)
 {
     if (EditEntity == null)
     {
         return null;
     }

     //同时检查数量和金额，总数量和总金额不能小于明细小计的和
     if (EditEntity.TotalQty < EditEntity.tb_SaleOrderDetails.Sum(c => c.Quantity))
     {
         MainForm.Instance.PrintInfoLog($"订单：{EditEntity.SOrderNo}:总数量不能小于明细小计之和！");
         return null;
     }

     if (EditEntity.TotalAmount < EditEntity.tb_SaleOrderDetails.Sum(c => c.TransactionPrice * c.Quantity))
     {
         MainForm.Instance.PrintInfoLog($"订单：{EditEntity.SOrderNo}:总金额不能小于明细小计之和！");
         return null;
     }
     ApprovalEntity ae =await base.Review(EditEntity);

     return ae;
 }


 /// <summary>
 /// 销售订单反审
 /// </summary>
 /// <param name="EditEntitys"></param>
 /// <returns></returns>
 public async override Task<bool> ReReview(List<tb_SaleOrder> EditEntitys)
 {
     if (EditEntitys == null)
     {
         return false;
     }
     foreach (tb_SaleOrder EditEntity in EditEntitys)
     {
         #region 反审
         //反审，要审核过，并且通过了，才能反审。
         if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
         {
             MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
             continue;
         }


         if (EditEntity.tb_SaleOrderDetails == null || EditEntity.tb_SaleOrderDetails.Count == 0)
         {
             MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
             continue;
         }

         RevertCommand command = new RevertCommand();
         //缓存当前编辑的对象。如果撤销就回原来的值
         tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
         command.UndoOperation = delegate ()
         {
             //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
             CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
         };

         tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

         ReturnResults<bool> rr = await ctr.AntiApprovalAsync(EditEntity);
         if (rr.Succeeded)
         {
             //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
             //{

             //}
             //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
             //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
             //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
             //MainForm.Instance.ecs.AddSendData(od);

             //审核成功
         }
         else
         {
             //审核失败 要恢复之前的值
             command.Undo();
             MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}反审失败{rr.ErrorMsg},请联系管理员！", Color.Red);
         }

         #endregion
     }
     return true;
 }
 */

        public async override Task<bool> CloseCase(List<tb_SaleOrder> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                MainForm.Instance.PrintInfoLog($"结案操作成功！", Color.Red);
                MainForm.Instance.logger.LogInformation($"结案操作成功！");
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
