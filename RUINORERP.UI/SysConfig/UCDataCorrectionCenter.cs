using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using WorkflowCore.Interface;
using RUINORERP.UI.WorkFlowTester;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System.Linq.Expressions;
using RUINORERP.Extensions.Middlewares;
using SqlSugar;
using RUINORERP.UI.Report;
using System.Reflection;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Collections.Concurrent;
using Org.BouncyCastle.Math.Field;
using RUINORERP.UI.PSI.PUR;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.UI.SS;
using System.Management.Instrumentation;
using RUINORERP.Business.CommService;
using RUINORERP.Model.CommonModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Netron.GraphLib;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("数据校正中心", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataCorrectionCenter : UserControl
    {
        public UCDataCorrectionCenter()
        {
            InitializeComponent();
        }


        Type[] ModelTypes;
        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        List<string> typeNames = new List<string>();

        List<SugarTable> stlist = new List<SugarTable>();

        private void UCDataFix_Load(object sender, EventArgs e)
        {
            LoadTree();
        }

        private async void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeViewTableList.SelectedNode != null && treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Text == "菜单枚举类型修复")
                {
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_MenuInfo).Name)
                    {
                        #region 菜单枚举类型修复
                        tb_MenuInfoController<tb_MenuInfo> ctrMenuInfo = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
                        List<tb_MenuInfo> menuInfos = ctrMenuInfo.Query();

                        tb_PrintTemplateController<tb_PrintTemplate> ctrPrintTemplate = Startup.GetFromFac<tb_PrintTemplateController<tb_PrintTemplate>>();
                        List<tb_PrintTemplate> PrintTemplates = ctrPrintTemplate.Query();

                        tb_PrintConfigController<tb_PrintConfig> ctrPrintConfig = Startup.GetFromFac<tb_PrintConfigController<tb_PrintConfig>>();
                        List<tb_PrintConfig> PrintConfigs = ctrPrintConfig.Query();


                        for (int i = 0; i < menuInfos.Count; i++)
                        {
                            if (menuInfos[i].MenuName == "借出单")
                            {

                            }

                            //检查菜单设置中的枚举类型是不是和代码中的一致，因为代码可能会维护修改
                            if (menuInfos[i].BizType != null && menuInfos[i].BizType.HasValue)
                            {
                                int oldid = menuInfos[i].BizType.Value;

                                //新值来自枚举值硬编码
                                #region 取新值后对比旧值，不一样的就更新到菜单表中

                                // 获取当前正在执行的程序集
                                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                                // 已知类的全名（包括命名空间）
                                string className = menuInfos[i].ClassPath;
                                // 获取类型对象
                                Type type = currentAssembly.GetType(className);
                                if (type != null)
                                {
                                    #region 从最基础的窗体类中获取枚举值 如果与旧值不一样则更新
                                    // 使用 Activator 创建类的实例
                                    object instance = Activator.CreateInstance(type);
                                    var descType = typeof(MenuAttrAssemblyInfo);
                                    // 类型是否为窗体，否则跳过，进入下一个循环
                                    // 是否为自定义特性，否则跳过，进入下一个循环
                                    if (!type.IsDefined(descType, false))
                                        continue;

                                    // 强制为自定义特性
                                    MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;
                                    // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                                    if (attribute == null || !attribute.Enabled)
                                        continue;
                                    // 域注入
                                    //Services.AddScoped(type);
                                    MenuAttrAssemblyInfo info = new MenuAttrAssemblyInfo();
                                    info = attribute;
                                    info.ClassName = type.Name;
                                    info.ClassType = type;
                                    info.ClassPath = type.FullName;
                                    info.Caption = attribute.Describe;
                                    info.MenuPath = attribute.MenuPath;
                                    info.UiType = attribute.UiType;
                                    if (attribute.MenuBizType.HasValue)
                                    {
                                        info.MenuBizType = attribute.MenuBizType.Value;
                                    }

                                    int newid = (int)((BizType)info.MenuBizType.Value);
                                    if (oldid == newid)
                                    {
                                        //richTextBoxLog.AppendText($"{menuInfos[i].MenuName}=>{menuInfos[i].BizType} ==>" + (BizType)menuInfos[i].BizType.Value + "\r\n");
                                    }
                                    else
                                    {
                                        richTextBoxLog.AppendText($"菜单信息：{menuInfos[i].MenuName}=>{menuInfos[i].BizType} ==========不应该是===={(BizType)menuInfos[i].BizType.Value}=======应该是{newid}" + "\r\n");
                                        if (!chkTestMode.Checked)
                                        {
                                            menuInfos[i].BizType = newid;
                                            await ctrMenuInfo.UpdateMenuInfo(menuInfos[i]);
                                        }
                                    }
                                    #endregion

                                    #region 打印配置和打印模板中也用了这个BizType 
                                    /*TODO: 打印模板与业务类型和名称无关。只是通过配置ID来关联的*/
                                    var printconfig = PrintConfigs.FirstOrDefault(p => p.BizName == menuInfos[i].MenuName);
                                    if (printconfig != null && printconfig.BizType != newid)
                                    {
                                        richTextBoxLog.AppendText($"打印配置：{printconfig.BizName}=>{printconfig.BizType} ==========不应该是===={(BizType)printconfig.BizType}=======应该是{newid}" + "\r\n");
                                        printconfig.BizType = newid;
                                        if (!chkTestMode.Checked)
                                        {
                                            await ctrPrintConfig.UpdateAsync(printconfig);
                                        }

                                        if (printconfig.tb_PrintTemplates != null && printconfig.tb_PrintTemplates.Count > 0)
                                        {
                                            var reportTemplate = printconfig.tb_PrintTemplates.FirstOrDefault(c => c.BizName == menuInfos[i].MenuName);
                                            if (reportTemplate != null && reportTemplate.BizType != newid)
                                            {
                                                richTextBoxLog.AppendText($"打印模板：{reportTemplate.BizName}=>{reportTemplate.BizType} ==========不应该是===={(BizType)reportTemplate.BizType.Value}=======应该是{newid}" + "\r\n");
                                                reportTemplate.BizType = newid;
                                                if (!chkTestMode.Checked)
                                                {
                                                    await ctrPrintTemplate.UpdateAsync(reportTemplate);
                                                }

                                            }
                                        }


                                    }


                                    #endregion


                                }
                                else
                                {
                                    MessageBox.Show($"{className}类型未找到,请确认菜单{menuInfos[i].MenuName}配置是否正确。");
                                }

                                #endregion



                            }


                        }

                        #endregion
                    }
                }

                if (treeView1.SelectedNode.Text == "采购订单价格修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurOrderDetail).Name)
                    {
                        #region 采购订单明细价格修复
                        //折扣修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1
                        //单价修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1

                        tb_PurOrderController<tb_PurOrder> ctrPurOrder = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();

                        tb_PurOrderDetailController<tb_PurOrderDetail> ctrPurOrderDetail = Startup.GetFromFac<tb_PurOrderDetailController<tb_PurOrderDetail>>();

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrderDetail> PurOrderDetails = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrderDetail>()
                            .Includes(c => c.tb_purorder, b => b.tb_PurOrderDetails)
                            .Where(c => c.Discount == 0 && c.UnitPrice > 0 && c.TransactionPrice == 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails.Count; i++)
                        {
                            PurOrderDetails[i].Discount = 1;
                            PurOrderDetails[i].TransactionPrice = PurOrderDetails[i].UnitPrice;
                            PurOrderDetails[i].SubtotalAmount = PurOrderDetails[i].TransactionPrice * PurOrderDetails[i].Quantity;

                            if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails[i].tb_purorder.TotalAmount = PurOrderDetails[i].SubtotalAmount;
                                PurOrderDetails[i].tb_purorder.ActualAmount = PurOrderDetails[i].SubtotalAmount + PurOrderDetails[i].tb_purorder.ShippingCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(PurOrderDetails[i]);
                                if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(PurOrderDetails[i].tb_purorder);
                                }
                            }
                        }


                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrderDetail> PurOrderDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrderDetail>()
                            .Includes(c => c.tb_purorder, b => b.tb_PurOrderDetails)
                            .Where(c => c.Discount == 1 && c.UnitPrice == 0 && c.TransactionPrice > 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails1.Count; i++)
                        {
                            PurOrderDetails1[i].Discount = 1;
                            PurOrderDetails1[i].UnitPrice = PurOrderDetails1[i].TransactionPrice;
                            PurOrderDetails1[i].SubtotalAmount = PurOrderDetails1[i].TransactionPrice * PurOrderDetails1[i].Quantity;

                            if (PurOrderDetails1[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount;
                                PurOrderDetails1[i].tb_purorder.ActualAmount = PurOrderDetails1[i].SubtotalAmount + PurOrderDetails1[i].tb_purorder.ShippingCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(PurOrderDetails1[i]);
                                if (PurOrderDetails1[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(PurOrderDetails1[i].tb_purorder);
                                }
                            }
                        }



                        //单价不等于成交价时。看折扣情况
                        PurOrderDetails = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrderDetail>()
                           .Includes(c => c.tb_purorder, b => b.tb_PurOrderDetails)
                           .Where(c => c.UnitPrice != c.TransactionPrice && c.Discount != 0)
                           .ToList();

                        for (int i = 0; i < PurOrderDetails.Count; i++)
                        {
                            //如果是成交价等于单价*折扣，跳过
                            if (PurOrderDetails[i].TransactionPrice == PurOrderDetails[i].UnitPrice * PurOrderDetails[i].Discount)
                            {
                                continue;
                            }
                            PurOrderDetails[i].TransactionPrice = PurOrderDetails[i].UnitPrice * PurOrderDetails[i].Discount;
                            PurOrderDetails[i].SubtotalAmount = PurOrderDetails[i].TransactionPrice * PurOrderDetails[i].Quantity;

                            if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails[i].tb_purorder.TotalAmount = PurOrderDetails[i].SubtotalAmount;
                                PurOrderDetails[i].tb_purorder.ActualAmount = PurOrderDetails[i].SubtotalAmount + PurOrderDetails[i].tb_purorder.ShippingCost;
                            }
                            else
                            {

                            }

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurOrderDetail.UpdateAsync(PurOrderDetails[i]);
                                if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                                {
                                    await ctrPurOrder.UpdateAsync(PurOrderDetails[i].tb_purorder);
                                }
                            }
                        }



                        #endregion
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurOrder).Name)
                    {
                        #region 采购订单金额修复

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurOrder> PurOrders = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                            .Includes(c => c.tb_PurOrderDetails)
                            .ToList();

                        for (int i = 0; i < PurOrders.Count; i++)
                        {
                            //检测明细小计：
                            for (int j = 0; j < PurOrders[i].tb_PurOrderDetails.Count; j++)
                            {
                                //如果明细的小计不等于成交价*数量
                                if (PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount !=
                                    PurOrders[i].tb_PurOrderDetails[j].TransactionPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}中的{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount = PurOrders[i].tb_PurOrderDetails[j].TransactionPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity;
                                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i].tb_PurOrderDetails[j]).UpdateColumns(t => new { t.SubtotalAmount }).ExecuteCommandAsync();
                                        richTextBoxLog.AppendText($"采购订单明细{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}的小计金额{PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                    }
                                }
                            }

                            //检测订单总计：
                            if (PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount) != PurOrders[i].TotalAmount)
                            {
                                if (chkTestMode.Checked)
                                {
                                    richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}总金额{PurOrders[i].TotalAmount}不等于他的明细的小计求各项总和：{PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount)} " + "\r\n");
                                }
                                else
                                {
                                    PurOrders[i].TotalAmount = PurOrders[i].tb_PurOrderDetails.Sum(c => c.SubtotalAmount);
                                    PurOrders[i].ActualAmount = PurOrders[i].TotalAmount + PurOrders[i].ShippingCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i]).UpdateColumns(t => new { t.TotalAmount, t.ActualAmount }).ExecuteCommandAsync();
                                    richTextBoxLog.AppendText($"采购订单{PurOrders[i].PurOrderNo}的总金额修复成功：{totalamountCounter} " + "\r\n");
                                }
                            }
                            //检测总计：
                        }

                        #endregion
                    }
                }
                if (treeView1.SelectedNode.Text == "采购入库单价格修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntryDetail).Name)
                    {
                        #region 采购入库明细价格修复
                        //折扣修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1
                        //单价修复  如果单价=成交价并且大于0.折扣=0.则将折扣修改为1

                        tb_PurEntryController<tb_PurEntry> ctrPurEntry = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();

                        tb_PurEntryDetailController<tb_PurEntryDetail> ctrPurEntryDetail = Startup.GetFromFac<tb_PurEntryDetailController<tb_PurEntryDetail>>();

                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurEntryDetail> PurEntryDetails = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                            .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                            .Where(c => c.UnitPrice > 0 && c.TransactionPrice == 0 && c.Discount != 0)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails.Count; i++)
                        {
                            PurEntryDetails[i].Discount = 1;
                            PurEntryDetails[i].TransactionPrice = PurEntryDetails[i].UnitPrice;
                            PurEntryDetails[i].SubtotalAmount = PurEntryDetails[i].TransactionPrice * PurEntryDetails[i].Quantity;
                            if (!chkTestMode.Checked)
                            {

                                await ctrPurEntryDetail.UpdateAsync(PurEntryDetails[i]);

                            }
                            else
                            {
                                richTextBoxLog.AppendText($"1采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                            }
                        }


                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        List<tb_PurEntryDetail> PurEntryDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                            .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                            .Where(c => c.UnitPrice == 0 && c.TransactionPrice > 0)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails1.Count; i++)
                        {
                            PurEntryDetails1[i].Discount = 1;
                            PurEntryDetails1[i].UnitPrice = PurEntryDetails1[i].TransactionPrice;
                            PurEntryDetails1[i].SubtotalAmount = PurEntryDetails1[i].TransactionPrice * PurEntryDetails1[i].Quantity;

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurEntryDetail.UpdateAsync(PurEntryDetails1[i]);

                            }
                            else
                            {
                                richTextBoxLog.AppendText($"2采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                            }
                        }



                        //折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        PurEntryDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                            .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                            .Where(c => c.UnitPrice != c.TransactionPrice)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails1.Count; i++)
                        {
                            //如果是成交价等于单价*折扣，跳过
                            if (PurEntryDetails1[i].TransactionPrice == PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Discount)
                            {
                                continue;
                            }
                            PurEntryDetails1[i].TransactionPrice = PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Discount;
                            PurEntryDetails1[i].SubtotalAmount = PurEntryDetails1[i].TransactionPrice * PurEntryDetails1[i].Quantity;

                            if (!chkTestMode.Checked)
                            {
                                await ctrPurEntryDetail.UpdateAsync(PurEntryDetails1[i]);
                            }
                            else
                            {
                                richTextBoxLog.AppendText($"3采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                            }
                        }


                        #endregion
                    }
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntry).Name)
                    {
                        #region 采购入库单金额修复

                        List<tb_PurEntry> tb_PurEntrys = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_PurEntryDetails)
                            .ToList();

                        for (int i = 0; i < tb_PurEntrys.Count; i++)
                        {
                            //检测明细小计：
                            for (int j = 0; j < tb_PurEntrys[i].tb_PurEntryDetails.Count; j++)
                            {
                                //如果明细的小计不等于成交价*数量
                                if (tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount !=
                                    tb_PurEntrys[i].tb_PurEntryDetails[j].TransactionPrice * tb_PurEntrys[i].tb_PurEntryDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购入库单{tb_PurEntrys[i].PurEntryNo}中金额的{tb_PurEntrys[i].tb_PurEntryDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount = tb_PurEntrys[i].tb_PurEntryDetails[j].TransactionPrice * tb_PurEntrys[i].tb_PurEntryDetails[j].Quantity;
                                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(tb_PurEntrys[i].tb_PurEntryDetails[j]).UpdateColumns(t => new { t.SubtotalAmount }).ExecuteCommandAsync();
                                        richTextBoxLog.AppendText($"采购入库单明细{tb_PurEntrys[i].tb_PurEntryDetails[j].ProdDetailID}的小计金额{tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                    }
                                }
                            }

                            //检测订单总计：
                            if (tb_PurEntrys[i].tb_PurEntryDetails.Sum(c => c.SubtotalAmount) != tb_PurEntrys[i].TotalAmount)
                            {
                                if (chkTestMode.Checked)
                                {
                                    richTextBoxLog.AppendText($"采购入库单金额：{tb_PurEntrys[i].PurEntryNo}总金额{tb_PurEntrys[i].TotalAmount}不等于他的明细的小计求各项总和：{tb_PurEntrys[i].tb_PurEntryDetails.Sum(c => c.SubtotalAmount)} " + "\r\n");
                                }
                                else
                                {
                                    tb_PurEntrys[i].TotalAmount = tb_PurEntrys[i].tb_PurEntryDetails.Sum(c => c.SubtotalAmount);
                                    tb_PurEntrys[i].ActualAmount = tb_PurEntrys[i].TotalAmount + tb_PurEntrys[i].ShippingCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(tb_PurEntrys[i]).UpdateColumns(t => new { t.TotalAmount, t.ActualAmount }).ExecuteCommandAsync();
                                    richTextBoxLog.AppendText($"采购入库单{tb_PurEntrys[i].PurEntryNo}的总金额修复成功：{totalamountCounter} " + "\r\n");
                                }
                            }
                            //检测总计：
                        }

                        #endregion

                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_PurEntry).Name)
                    {
                        #region 采购订单明细有单价成交价的。入库明细中没有时要修复
                        List<tb_PurEntry> MyPurEntrys = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_PurEntryDetails)
                             .Includes(c => c.tb_purorder)
                            .Includes(c => c.tb_purorder, d => d.tb_PurOrderDetails)
                            .ToList();


                        for (int a = 0; a < MyPurEntrys.Count; a++)
                        {
                            //如果入库明细单价为0时则检测订单明细中单价多少。
                            for (int b = 0; b < MyPurEntrys[a].tb_PurEntryDetails.Count; b++)
                            {
                                if (MyPurEntrys[a].tb_PurEntryDetails[b].TransactionPrice == 0 && MyPurEntrys[a].tb_PurEntryDetails[b].UnitPrice == 0)
                                {
                                    //没有引用订单的跳过
                                    if (MyPurEntrys[a].tb_purorder == null)
                                    {
                                        continue;
                                    }
                                    var orderdetail = MyPurEntrys[a].tb_purorder.tb_PurOrderDetails.FirstOrDefault(c => c.ProdDetailID == MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID);
                                    if (orderdetail != null)
                                    {
                                        if (orderdetail.UnitPrice > 0 || orderdetail.TransactionPrice > 0)
                                        {
                                            //更新入库单明细
                                            if (chkTestMode.Checked)
                                            {
                                                richTextBoxLog.AppendText($"采购入库单明细{MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID}的价格需要修复。" + "\r\n");
                                            }
                                            else
                                            {
                                                MyPurEntrys[a].tb_PurEntryDetails[b].Discount = 1;
                                                MyPurEntrys[a].tb_PurEntryDetails[b].UnitPrice = orderdetail.UnitPrice;
                                                MyPurEntrys[a].tb_PurEntryDetails[b].TransactionPrice = orderdetail.TransactionPrice;
                                                int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(MyPurEntrys[a].tb_PurEntryDetails[b]).UpdateColumns(t => new { t.SubtotalAmount, t.Discount, t.TransactionPrice, t.UnitPrice }).ExecuteCommandAsync();
                                                richTextBoxLog.AppendText($"采购入库单明细{MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID}的小计金额{MyPurEntrys[a].tb_PurEntryDetails[b].SubtotalAmount}修复成功：{totalamountCounter} " + "\r\n");
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }

                if (treeView1.SelectedNode.Text == "销售订单价格修复")
                {


                }
                if (treeView1.SelectedNode.Text == "销售出库单价格修复")
                {

                }

                if (treeView1.SelectedNode.Text == "生产计划数量修复")
                {

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_FinishedGoodsInv).Name)
                    {
                        //修复缴库库明细和等于主表的总数量
                        List<tb_FinishedGoodsInv> FinishedGoodsInvList = await MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                            .Includes(c => c.tb_FinishedGoodsInvDetails)
                            .ToListAsync();
                        List<tb_FinishedGoodsInv> updatelist = new();
                        for (int i = 0; i < FinishedGoodsInvList.Count; i++)
                        {
                            if (FinishedGoodsInvList[i].TotalQty != FinishedGoodsInvList[i].tb_FinishedGoodsInvDetails.Sum(c => c.Qty))
                            {
                                if (!chkTestMode.Checked)
                                {
                                    FinishedGoodsInvList[i].TotalQty = FinishedGoodsInvList[i].tb_FinishedGoodsInvDetails.Sum(c => c.Qty);
                                    updatelist.Add(FinishedGoodsInvList[i]);
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"{FinishedGoodsInvList[i]} 等待修复 \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {

                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalQty }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复缴库库明细和等于主表的总数量 修复成功：{totalamountCounter} " + "\r\n");
                        }
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ManufacturingOrder).Name)
                    {

                        //制令单已完成数量要等于=名下所有缴库单数量之和
                        var ManufacturingOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                            .Includes(c => c.tb_FinishedGoodsInvs)
                                     .ToListAsync();

                        List<tb_ManufacturingOrder> updatelist = new();
                        for (int i = 0; i < ManufacturingOrders.Count; i++)
                        {
                            if (ManufacturingOrders[i].QuantityDelivered != ManufacturingOrders[i].tb_FinishedGoodsInvs.Where(c => c.DataStatus == 4).Sum(c => c.TotalQty))
                            {
                                if (!chkTestMode.Checked)
                                {
                                    ManufacturingOrders[i].QuantityDelivered = ManufacturingOrders[i].tb_FinishedGoodsInvs.Where(c => c.DataStatus == 4).Sum(c => c.TotalQty);
                                    updatelist.Add(ManufacturingOrders[i]);
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"{ManufacturingOrders[i].MONO} 等待修复 \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.QuantityDelivered }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复缴库单数量 修复成功：{totalamountCounter} " + "\r\n");
                        }
                    }

                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ProductionPlan).Name)
                    {
                        //计划完成数量等于他名下的需求单下的所有制令单完成数量之和
                        var ProductionPlans = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                             .Includes(a => a.tb_ProductionPlanDetails)
                            .Includes(c => c.tb_ProductionDemands, b => b.tb_ManufacturingOrders)
                            .ToListAsync();

                        List<tb_ProductionPlan> updatelist = new();
                        for (int ii = 0; ii < ProductionPlans.Count; ii++)
                        {
                            List<tb_ProductionPlanDetail> updatePlanDetails = new List<tb_ProductionPlanDetail>();
                            for (int jj = 0; jj < ProductionPlans[ii].tb_ProductionPlanDetails.Count; jj++)
                            {
                                int totalqty = 0;
                                for (int kk = 0; kk < ProductionPlans[ii].tb_ProductionDemands.Count; kk++)
                                {
                                    totalqty += ProductionPlans[ii].tb_ProductionDemands[kk].tb_ManufacturingOrders.Where(c => (c.DataStatus == 4 || c.DataStatus == 8) && c.ProdDetailID == ProductionPlans[ii].tb_ProductionPlanDetails[jj].ProdDetailID).Sum(c => c.QuantityDelivered);
                                }
                                if (totalqty == 0)
                                {
                                    if (ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity > 0)
                                    {
                                        richTextBoxLog.AppendText($"{ProductionPlans[ii].PPNo}计划明细中==>{totalqty}==========0时。明细保存的是{ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity} \r\n");
                                    }
                                    //如果制令单数量为0，则跳过
                                    continue;
                                }
                                if (totalqty != ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity)
                                {
                                    if (!chkTestMode.Checked)
                                    {

                                        ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity = totalqty;
                                        updatePlanDetails.Add(ProductionPlans[ii].tb_ProductionPlanDetails[jj]);
                                    }
                                    else
                                    {
                                        richTextBoxLog.AppendText($"{ProductionPlans[ii].PPNo}计划明细{ProductionPlans[ii].tb_ProductionPlanDetails[jj].CompletedQuantity}==>{totalqty} 等待修复！！！！！ \r\n");
                                    }
                                }

                            }

                            if (!chkTestMode.Checked)
                            {
                                int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatePlanDetails).UpdateColumns(t => new { t.CompletedQuantity }).ExecuteCommandAsync();
                                richTextBoxLog.AppendText($"{ProductionPlans[ii].PPNo}修复计划明细数量 修复成功：{totalamountCounter} " + "\r\n");
                            }



                            if (ProductionPlans[ii].TotalCompletedQuantity != ProductionPlans[ii].tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity))
                            {
                                if (!chkTestMode.Checked)
                                {
                                    ProductionPlans[ii].TotalCompletedQuantity = ProductionPlans[ii].tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity);
                                    updatelist.Add(ProductionPlans[ii]);
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"PPNo:{ProductionPlans[ii].PPNo},{ProductionPlans[ii].TotalCompletedQuantity} 等待修复 为{ProductionPlans[ii].tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity)} \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalCompletedQuantity }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"修复生产计划总数量 修复成功：{totalamountCounter} " + "\r\n");
                        }

                    }
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {


        }

        private void LoadTree()
        {
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            ModelTypes = dalAssemble.GetExportedTypes();
            typeNames = ModelTypes.Select(m => m.Name).ToList();
            treeViewTableList.Nodes.Clear();
            stlist.Clear();
            foreach (var type in ModelTypes)
            {
                var attrs = type.GetCustomAttributes<SugarTable>();
                foreach (var attr in attrs)
                {
                    if (attr is SugarTable st)
                    {
                        //var t = Startup.ServiceProvider.GetService(type);//SugarColumn 或进一步取字段特性也可以
                        //var t = Startup.ServiceProvider.CreateInstance(type);//SugarColumn 或进一步取字段特性也可以
                        if (st.TableName.Contains("tb_") && !type.Name.Contains("QueryDto"))
                        {
                            //if (txtTableName.Text.Trim().Length > 0 && st.TableName.Contains(txtTableName.Text.Trim()))
                            //{
                            if (st.TableName.Contains("tb_MenuInfo") || st.TableName == "tb_MenuInfo")
                            {

                            }
                            TreeNode node = new TreeNode(st.TableName);
                            node.Name = st.TableName;
                            node.Tag = type;
                            treeViewTableList.Nodes.Add(node);
                            stlist.Add(st);
                            //}
                        }
                        continue;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse && sender is TreeView tv)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
            //TreeViewSingleSelectedAndChecked(TreeView1, e);
            e.Node.Checked = true;
            node_AfterCheck(sender, e);

        }



        void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // only do it if the node became checked:
            if (e.Node.Checked)
            {
                // for all the nodes in the tree...
                foreach (TreeNode cur_node in e.Node.TreeView.Nodes)
                {
                    // ... which are not the freshly checked one...
                    if (cur_node != e.Node)
                    {
                        // ... uncheck them
                        cur_node.Checked = false;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="e"></param>
        public static void TreeViewSingleSelectedAndChecked(Krypton.Toolkit.KryptonTreeView tv, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
        }

        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);

            }
        }

    }
}
