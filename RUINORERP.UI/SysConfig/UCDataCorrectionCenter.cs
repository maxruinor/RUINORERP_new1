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
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using NPOI.SS.Formula.Functions;
using RUINORERP.Global.EnumExt.CRM;
using HLH.Lib.Security;

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

                if (treeView1.SelectedNode.Text == "借出已还修复为完结")
                {
                    List<tb_ProdBorrowing> items = MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>()
                       .Includes(c => c.tb_ProdBorrowingDetails)
                       .Where(c => c.DataStatus == (int)DataStatus.确认)
                       .ToList();

                    foreach (tb_ProdBorrowing item in items)
                    {
                        if (!item.TotalQty.Equals(item.tb_ProdBorrowingDetails.Sum(c => c.Qty)))
                        {
                            richTextBoxLog.AppendText($"借出总数量和明细和不对：{item.BorrowID}：{item.BorrowNo}" + "\r\n");
                        }

                        if (item.TotalQty.Equals(item.tb_ProdBorrowingDetails.Sum(c => c.ReQty)))
                        {
                            richTextBoxLog.AppendText($"借出数量等于归还数量：{item.BorrowID}：{item.BorrowNo}" + "\r\n");
                        }
                    }

                    if (chkTestMode.Checked)
                    {

                    }


                }

                if (treeView1.SelectedNode.Text == "修复CRM跟进计划状态")
                {
                    #region 修复CRM跟进计划状态
                    List<tb_CRM_FollowUpPlans> followUpPlans = await MainForm.Instance.AppContext.Db.Queryable<tb_CRM_FollowUpPlans>()
              .Includes(c => c.tb_CRM_FollowUpRecordses)
              .Where(c => c.PlanEndDate < System.DateTime.Today &&
              (c.PlanStatus != (int)FollowUpPlanStatus.已完成 || c.PlanStatus == (int)FollowUpPlanStatus.未执行)
              && c.PlanStatus != (int)FollowUpPlanStatus.已取消
              )
              .ToListAsync();

                    // 假设配置的延期天数存储在 DelayDays 变量中
                    int DelayDays = 3;

                    for (int i = 0; i < followUpPlans.Count; i++)
                    {
                        if (followUpPlans[i].tb_CRM_FollowUpRecordses.Count > 0)
                        {
                            followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.已完成;
                        }
                        else
                        {
                            if (followUpPlans[i].PlanEndDate < System.DateTime.Today)
                            {
                                TimeSpan timeSinceEnd = System.DateTime.Today - followUpPlans[i].PlanEndDate;
                                if (timeSinceEnd.TotalDays <= DelayDays)
                                {
                                    followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.延期中;
                                }
                                else
                                {
                                    followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.未执行;
                                }

                                //发送消息给执行人。
                            }
                        }


                    }

                    #endregion
                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"要修复的行数为:{followUpPlans.Count}" + "\r\n");
                    }
                    else
                    {
                        int plancounter = await MainForm.Instance.AppContext.Db.Updateable<tb_CRM_FollowUpPlans>(followUpPlans).UpdateColumns(it => new { it.PlanStatus }).ExecuteCommandAsync();

                        richTextBoxLog.AppendText($"修复CRM跟进计划状态成功：{plancounter} " + "\r\n");
                    }

                }

                if (treeView1.SelectedNode.Text == "拟销在制在途修复")
                {

                }

                if (treeView1.SelectedNode.Text == "用户密码加密")
                {
                    #region 用户密码加密
                    List<tb_UserInfo> AllUsers = MainForm.Instance.AppContext.Db.Queryable<tb_UserInfo>()
                        .Where(c => c.Password.Length < 10)
                     .ToList();
                    for (int i = 0; i < AllUsers.Count; i++)
                    {
                        AllUsers[i].Notes = AllUsers[i].Password;
                        string enPwd = EncryptionHelper.AesEncryptByHashKey(AllUsers[i].Password, AllUsers[i].UserName);
                        AllUsers[i].Password = enPwd;
                        // string pwd = EncryptionHelper.AesDecryptByHashKey(enPwd, "张家歌");
                        richTextBoxLog.AppendText($"要修复的用户:{AllUsers[i].UserName}" + "\r\n");
                    }

                    #endregion
                    //一次性统计加密码一下密码：

                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"要修复的用户密码加密行数为:{AllUsers.Count}" + "\r\n");
                    }
                    else
                    {
                        int plancounter = await MainForm.Instance.AppContext.Db.Updateable<tb_UserInfo>(AllUsers).UpdateColumns(it => new { it.Password, it.Notes }).ExecuteCommandAsync();

                        richTextBoxLog.AppendText($"要修复的用户密码加密状态成功：{plancounter} " + "\r\n");
                    }
                }

                if (treeView1.SelectedNode.Text == "成本修复")
                {
                    //成本修复思路
                    //1）成本本身修复，将所有入库明细按加权平均算一下。更新到库存里面。
                    //2）修复所有出库明细
                    #region 成本本身修复
                    List<tb_Inventory> Allitems = MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                    .Includes(c => c.tb_proddetail, d => d.tb_PurEntryDetails)
                   .ToList();

                    List<tb_Inventory> updateList = new List<tb_Inventory>();
                    foreach (tb_Inventory item in Allitems)
                    {
                        if (item.tb_proddetail.tb_PurEntryDetails.Count > 0
                            && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.TransactionPrice) > 0
                            && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity) > 0
                            )
                        {
                            //第笔的入库的数量*成交价/总数量
                            var transPrice = item.tb_proddetail.tb_PurEntryDetails
                                .Where(c => c.TransactionPrice > 0 && c.Quantity > 0)
                                .Sum(c => c.TransactionPrice * c.Quantity) / item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity);
                            if (transPrice > 0)
                            {
                                transPrice = Math.Round(transPrice, 3);
                                item.CostMovingWA = transPrice;
                                item.Inv_AdvCost = item.CostMovingWA;
                                item.Inv_Cost = item.CostMovingWA;
                                richTextBoxLog.AppendText($"产品SKU:{item.tb_proddetail.SKU}的价格以最后成本价格修复为：{transPrice}：" + "\r\n");
                                updateList.Add(item);
                            }
                        }
                    }
                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"要修复的行数为:{Allitems.Count}" + "\r\n");
                    }
                    if (!chkTestMode.Checked)
                    {
                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updateList).UpdateColumns(t => new { t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                        richTextBoxLog.AppendText($"修复成本价格成功：{totalamountCounter} " + "\r\n");
                    }

                    #endregion

                    //#region 最后入库价不为0时，成本为0时，将这个入库成本给到库存成本。
                    //List<tb_Inventory> items = MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                    //    .Includes(c => c.tb_proddetail, d => d.tb_PurEntryDetails)
                    //   .ToList();

                    //foreach (tb_Inventory item in items)
                    //{
                    //    if (item.Inv_Cost == 0 && item.Inv_AdvCost == 0 && item.CostFIFO == 0 && item.CostMonthlyWA == 0 && item.CostMovingWA == 0
                    //        && item.tb_proddetail.tb_PurEntryDetails.Count > 0)
                    //    {
                    //        var transPrice = item.tb_proddetail.tb_PurEntryDetails[item.tb_proddetail.tb_PurEntryDetails.Count - 1].TransactionPrice;
                    //        if (transPrice > 0)
                    //        {
                    //            richTextBoxLog.AppendText($"产品SKU{item.tb_proddetail.SKU}的价格以最后入库价格修复：{transPrice}：" + "\r\n");

                    //            item.CostFIFO = item.CostMovingWA;
                    //            item.CostMonthlyWA = item.CostMovingWA;
                    //            item.Inv_AdvCost = item.CostMovingWA;
                    //            item.Inv_Cost = item.CostMovingWA;
                    //            item.CostMovingWA = transPrice;
                    //        }
                    //    }
                    //}

                    //if (!chkTestMode.Checked)
                    //{

                    //    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(items).UpdateColumns(t => new { t.CostMovingWA, t.Inv_AdvCost, t.CostFIFO, t.CostMonthlyWA, t.Inv_Cost }).ExecuteCommandAsync();
                    //    richTextBoxLog.AppendText($"修复价格成功：{totalamountCounter} " + "\r\n");
                    //}

                    //#endregion
                }

                if (treeView1.SelectedNode.Text == "销售订单价格修复")
                {


                }
                if (treeView1.SelectedNode.Text == "销售出库单价格修复")
                {

                }
                if (treeView1.SelectedNode.Text == "销售数量与明细数量和的检测")
                {
                    #region 销售订单数量与明细数量和的检测
                    List<tb_SaleOrder> SaleOrders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                        .Includes(c => c.tb_SaleOrderDetails)
                       .ToList();
                    foreach (tb_SaleOrder Order in SaleOrders)
                    {
                        if (!Order.TotalQty.Equals(Order.tb_SaleOrderDetails.Sum(c => c.Quantity)))
                        {
                            richTextBoxLog.AppendText($"销售订单数量不对：{Order.SOrder_ID}：{Order.SOrderNo}" + "\r\n");
                        }
                    }

                    #endregion

                    #region 销售订单数量与明细数量和的检测
                    List<tb_SaleOut> SaleOuts = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                        .Includes(c => c.tb_SaleOutDetails)
                       .ToList();
                    foreach (tb_SaleOut Saleout in SaleOuts)
                    {
                        if (!Saleout.TotalQty.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.Quantity)))
                        {
                            richTextBoxLog.AppendText($"销售出库数量不对：{Saleout.SaleOut_MainID}：{Saleout.SaleOutNo}" + "\r\n");
                        }
                    }

                    #endregion

                }

                if (treeView1.SelectedNode.Text == "将销售客户转换为目标客户")
                {
                    MessageBox.Show("只能执行一次。已经执行过了。");
                    //crm数据修复 只能执行一次。这里要注释掉。
                    /*
                    #region 
                    List<tb_CustomerVendor> CustomerVendors = MainForm.Instance.AppContext.Db.Queryable<tb_CustomerVendor>()
                        .IncludesAllFirstLayer()
                        .Where(c => c.IsCustomer == true && !c.CVName.Contains("信保"))
                        .ToList();
                    List<tb_CRM_Customer> customers = new List<tb_CRM_Customer>();
                    foreach (var Customer in CustomerVendors)
                    {
                        IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                        tb_CRM_Customer entity = mapper.Map<tb_CRM_Customer>(Customer);
                        BusinessHelper.Instance.InitEntity(entity);
                        customers.Add(entity);
                    }
                    tb_CRM_CustomerController<tb_CRM_Customer> ctr = Startup.GetFromFac<tb_CRM_CustomerController<tb_CRM_Customer>>();
                    List<long> ids = await ctr.AddAsync(customers);

                    if (ids.Count > 0)
                    {
                        richTextBoxLog.AppendText($"保存成功：{ids.Count}条记录" + "\r\n");
                    }

                    #endregion
                    */
                }
                if (treeView1.SelectedNode.Text == "属性重复的SKU检测")
                {
                    //思路是将属性全查出来。将属性按规则排序后比较

                    #region 判断是否有重复的属性值。将属性值添加到列表，按一定规则排序，然后判断是否有重复

                    List<tb_Prod_Attr_Relation> attr_Relations = MainForm.Instance.AppContext.Db.Queryable<tb_Prod_Attr_Relation>()
                        .IncludesAllFirstLayer()
                        .Where(c => c.PropertyValueID.HasValue && c.Property_ID.HasValue)
                        .ToList();
                    //首先将这些数据按品分组

                    //先找到主产品
                    var prodIDs = attr_Relations.GroupBy(c => c.ProdBaseID).Select(c => c.Key).ToList();
                    foreach (var prodID in prodIDs)
                    {
                        #region
                        List<string> DuplicateAttributes = new List<string>();
                        //根据主产品找到SKU详情
                        var prodDetai = attr_Relations.Where(c => c.ProdBaseID.Value == prodID).GroupBy(c => c.ProdDetailID).Select(c => c.Key).ToList();

                        //根据详情找到对应的所有属性值
                        foreach (var detail in prodDetai)
                        {
                            #region 找组合值 按一个顺序串起来加到一个集合再去比较重复
                            string sortedDaString = string.Empty;
                            foreach (var item in attr_Relations.Where(c => c.ProdDetailID.Value == detail))
                            {
                                // da 是一个 string 数组
                                string[] da = attr_Relations
                                .Where(c => c.ProdDetailID == item.ProdDetailID)
                                .ToList()
                                .Select(c => c.tb_prodpropertyvalue.PropertyValueName)
                                .ToArray();
                                // 将 da 转换为排序后的列表
                                List<string> sortedDa = da.OrderBy(x => x).ToList();

                                // 将排序后的列表转换为字符串
                                sortedDaString = string.Join(", ", sortedDa);
                            }
                            // 添加到 DuplicateAttributes 集合中
                            DuplicateAttributes.Add(sortedDaString);
                            #endregion
                        }

                        //这里是这个产品下面的所有SKU对应的属性值的,串起来的集合数量等于SKU的个数
                        // 找出 DuplicateAttributes 中的重复值
                        var duplicates = DuplicateAttributes
                            .GroupBy(s => s)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();

                        if (duplicates.Count > 0)
                        {
                            // 输出重复的值
                            foreach (var dup in duplicates)
                            {
                                richTextBoxLog.AppendText($"产品ID：{prodID}中的属性值重复:" + dup + "\r\n");
                            }
                        }
                        #endregion

                    }

                    #endregion

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


                if (treeView1.SelectedNode.Text == "制令单自制品修复")
                {
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_ManufacturingOrder).Name)
                    {
                        List<tb_ManufacturingOrder> ManufacturingOrderList = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                           .Includes(c => c.tb_productiondemand, b => b.tb_ProduceGoodsRecommendDetails)
                           .ToListAsync();

                        List<tb_ManufacturingOrder> MoUpdatelist = new();
                        foreach (tb_ManufacturingOrder ManufacturingOrder in ManufacturingOrderList)
                        {
                            if (!ManufacturingOrder.PDCID.HasValue)
                            {
                                if (!chkTestMode.Checked)
                                {
                                    var prddetail = ManufacturingOrder.tb_productiondemand.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ProdDetailID == ManufacturingOrder.ProdDetailID);
                                    if (prddetail == null)
                                    {
                                        continue;
                                    }
                                    ManufacturingOrder.PDCID = prddetail.PDCID;
                                    MoUpdatelist.Add(ManufacturingOrder);
                                    richTextBoxLog.AppendText($"PDCID 为空， 成功修复为 {prddetail.PDCID} \r\n");
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"PDCID 为空， 等待修复为  \r\n");
                                }
                            }
                        }
                        if (!chkTestMode.Checked)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(MoUpdatelist).UpdateColumns(t => new { t.PDCID }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"制令单 PDCID总数量 修复成功：{totalamountCounter} " + "\r\n");
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
