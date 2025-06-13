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
using RUINORERP.Global.EnumExt.CRM;
using HLH.Lib.Security;
using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.UserCenter.DataParts;
using NPOI.POIFS.Properties;
using Org.BouncyCastle.Crypto;


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
                            .Where(c => c.UnitPrice > 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails.Count; i++)
                        {
                            //PurOrderDetails[i].Discount = 1;

                            PurOrderDetails[i].SubtotalAmount = PurOrderDetails[i].UnitPrice * PurOrderDetails[i].Quantity;

                            if (PurOrderDetails[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails[i].tb_purorder.TotalAmount = PurOrderDetails[i].SubtotalAmount;
                                PurOrderDetails[i].tb_purorder.TotalAmount = PurOrderDetails[i].SubtotalAmount + PurOrderDetails[i].tb_purorder.ShipCost;
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
                            .Where(c => c.UnitPrice == 0)
                            .ToList();

                        for (int i = 0; i < PurOrderDetails1.Count; i++)
                        {


                            PurOrderDetails1[i].SubtotalAmount = PurOrderDetails1[i].UnitPrice * PurOrderDetails1[i].Quantity;

                            if (PurOrderDetails1[i].tb_purorder.tb_PurOrderDetails.Count == 1)
                            {
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount;
                                PurOrderDetails1[i].tb_purorder.TotalAmount = PurOrderDetails1[i].SubtotalAmount + PurOrderDetails1[i].tb_purorder.ShipCost;
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
                                    PurOrders[i].tb_PurOrderDetails[j].UnitPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购订单金额：{PurOrders[i].PurOrderNo}中的{PurOrders[i].tb_PurOrderDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        PurOrders[i].tb_PurOrderDetails[j].SubtotalAmount = PurOrders[i].tb_PurOrderDetails[j].UnitPrice * PurOrders[i].tb_PurOrderDetails[j].Quantity;
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
                                    PurOrders[i].TotalAmount = PurOrders[i].TotalAmount + PurOrders[i].ShipCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(PurOrders[i]).UpdateColumns(t => new { t.TotalAmount }).ExecuteCommandAsync();
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
                            .Where(c => c.UnitPrice > 0)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails.Count; i++)
                        {

                            PurEntryDetails[i].SubtotalAmount = PurEntryDetails[i].UnitPrice * PurEntryDetails[i].Quantity;
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
                            .Where(c => c.UnitPrice == 0)
                            .ToList();

                        for (int i = 0; i < PurEntryDetails1.Count; i++)
                        {


                            PurEntryDetails1[i].SubtotalAmount = PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Quantity;

                            if (!chkTestMode.Checked)
                            {

                                await ctrPurEntryDetail.UpdateAsync(PurEntryDetails1[i]);

                            }
                            else
                            {
                                richTextBoxLog.AppendText($"2采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                            }
                        }



                        ////折扣为0的单价大于0的。成交价为0的。折扣修改为1，成交价修改为单价
                        //PurEntryDetails1 = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryDetail>()
                        //    .Includes(c => c.tb_purentry, b => b.tb_PurEntryDetails)
                        //    .Where(c => c.UnitPrice != c.TransactionPrice)
                        //    .ToList();

                        //for (int i = 0; i < PurEntryDetails1.Count; i++)
                        //{
                        //    //如果是成交价等于单价*折扣，跳过
                        //    if (PurEntryDetails1[i].TransactionPrice == PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Discount)
                        //    {
                        //        continue;
                        //    }
                        //    PurEntryDetails1[i].TransactionPrice = PurEntryDetails1[i].UnitPrice * PurEntryDetails1[i].Discount;
                        //    PurEntryDetails1[i].SubtotalAmount = PurEntryDetails1[i].TransactionPrice * PurEntryDetails1[i].Quantity;

                        //    if (!chkTestMode.Checked)
                        //    {
                        //        await ctrPurEntryDetail.UpdateAsync(PurEntryDetails1[i]);
                        //    }
                        //    else
                        //    {
                        //        richTextBoxLog.AppendText($"3采购入库单明细{PurEntryDetails[i].ProdDetailID}的价格需要修复" + "\r\n");
                        //    }
                        //}


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
                                    tb_PurEntrys[i].tb_PurEntryDetails[j].UnitPrice * tb_PurEntrys[i].tb_PurEntryDetails[j].Quantity)
                                {
                                    if (chkTestMode.Checked)
                                    {
                                        richTextBoxLog.AppendText($"采购入库单{tb_PurEntrys[i].PurEntryNo}中金额的{tb_PurEntrys[i].tb_PurEntryDetails[j].ProdDetailID}=> =========小计不等于成交价*数量========== " + "\r\n");
                                    }
                                    else
                                    {
                                        tb_PurEntrys[i].tb_PurEntryDetails[j].SubtotalAmount = tb_PurEntrys[i].tb_PurEntryDetails[j].UnitPrice * tb_PurEntrys[i].tb_PurEntryDetails[j].Quantity;
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
                                    tb_PurEntrys[i].TotalAmount = tb_PurEntrys[i].TotalAmount + tb_PurEntrys[i].ShipCost;
                                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(tb_PurEntrys[i]).UpdateColumns(t => new { t.TotalAmount }).ExecuteCommandAsync();
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
                                if (MyPurEntrys[a].tb_PurEntryDetails[b].UnitPrice == 0)
                                {
                                    //没有引用订单的跳过
                                    if (MyPurEntrys[a].tb_purorder == null)
                                    {
                                        continue;
                                    }
                                    var orderdetail = MyPurEntrys[a].tb_purorder.tb_PurOrderDetails.FirstOrDefault(c => c.ProdDetailID == MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID);
                                    if (orderdetail != null)
                                    {
                                        if (orderdetail.UnitPrice > 0)
                                        {
                                            //更新入库单明细
                                            if (chkTestMode.Checked)
                                            {
                                                richTextBoxLog.AppendText($"采购入库单明细{MyPurEntrys[a].tb_PurEntryDetails[b].ProdDetailID}的价格需要修复。" + "\r\n");
                                            }
                                            else
                                            {
                                                MyPurEntrys[a].tb_PurEntryDetails[b].UnitPrice = orderdetail.UnitPrice;

                                                int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(MyPurEntrys[a].tb_PurEntryDetails[b]).UpdateColumns(t => new { t.SubtotalAmount, t.TaxAmount, t.UnitPrice }).ExecuteCommandAsync();
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
                    List<tb_Inventory> UpdateInventories = new List<tb_Inventory>(); //要更新的Inventories
                    StringBuilder sb = new StringBuilder();
                    #region 拟销在制在途修复
                    List<tb_Inventory> inventories = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                      .Includes(c => c.tb_proddetail)
                      .Includes(c => c.tb_location)
                      .ToListAsync();
                    //按仓库按产品去各种业务单据中去找。找到更新
                    List<tb_SaleOrder> SaleOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                     .Includes(c => c.tb_SaleOrderDetails)
                     .Where(c => c.DataStatus == (int)DataStatus.确认)
                     .ToListAsync();
                    int totalSaleQty = 0;


                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalSaleQty = 0;

                        tb_Inventory inventory = inventories[i];
                        foreach (var item in SaleOrders)
                        {
                            totalSaleQty += item.tb_SaleOrderDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.TotalDeliveredQty));
                        }

                        if (inventory.Sale_Qty != totalSaleQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID} 拟销数量由{inventory.Sale_Qty}修复为:{totalSaleQty}" + "\r\n");
                            inventory.Sale_Qty = totalSaleQty;
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion


                    #region 在途修复

                    //按仓库按产品去各种业务单据中去找。找到更新
                    List<tb_PurOrder> PurOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                     .Includes(c => c.tb_PurOrderDetails)
                     .Where(c => c.DataStatus == (int)DataStatus.确认)
                     .ToListAsync();

                    List<tb_PurEntryRe> PurEntryRes = await MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryRe>()
                 .Includes(c => c.tb_PurEntryReDetails)
                 .Where(c => c.DataStatus == (int)DataStatus.确认 && c.ProcessWay == (int)PurReProcessWay.需要返回)
                 .ToListAsync();

                    List<tb_MRP_ReworkReturn> ReworkReturns = await MainForm.Instance.AppContext.Db.Queryable<tb_MRP_ReworkReturn>()
                    .Includes(c => c.tb_MRP_ReworkReturnDetails)
                    .Where(c => c.DataStatus == (int)DataStatus.确认)
                    .ToListAsync();

                    int totalOntheWayQty = 0;
                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalOntheWayQty = 0;
                        tb_Inventory inventory = inventories[i];
                        foreach (var item in PurOrders)
                        {
                            totalOntheWayQty += item.tb_PurOrderDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.DeliveredQuantity));
                        }

                        foreach (var item in ReworkReturns)
                        {
                            totalOntheWayQty += item.tb_MRP_ReworkReturnDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.DeliveredQuantity));
                        }

                        foreach (var item in PurEntryRes)
                        {
                            totalOntheWayQty += item.tb_PurEntryReDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.Quantity - c.DeliveredQuantity));
                        }

                        if (inventory.On_the_way_Qty != totalOntheWayQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID} " +
                                $"在途数量由{inventory.On_the_way_Qty}修复为:{totalOntheWayQty}" + "\r\n");
                            inventory.On_the_way_Qty = totalOntheWayQty;
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion

                    #region 在制修复
                    //在制作中，是算主表的制作目标的产品。
                    List<tb_ManufacturingOrder> manufacturingOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                     .Includes(c => c.tb_ManufacturingOrderDetails)
                     .Where(c => c.DataStatus == (int)DataStatus.确认)
                     .ToListAsync();
                    int totalMakingQty = 0;

                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalMakingQty = 0;
                        tb_Inventory inventory = inventories[i];

                        totalMakingQty += manufacturingOrders.Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                            .Sum(c => (c.ManufacturingQty - c.QuantityDelivered));

                        if (inventory.MakingQty != totalMakingQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID} " +
                                $"在制数量由{inventory.MakingQty}修复为:{totalMakingQty}" + "\r\n");
                            inventory.MakingQty = totalMakingQty;
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion

                    #region 未发数量修复


                    decimal totalNotOutQty = 0;

                    for (int i = 0; i < inventories.Count; i++)
                    {
                        totalNotOutQty = 0;
                        tb_Inventory inventory = inventories[i];
                        foreach (var item in manufacturingOrders)
                        {
                            totalNotOutQty += item.tb_ManufacturingOrderDetails
                                .Where(c => c.ProdDetailID == inventory.ProdDetailID && c.Location_ID == inventory.Location_ID)
                                .Sum(c => (c.ShouldSendQty - c.ActualSentQty));
                        }


                        if (inventory.NotOutQty != totalNotOutQty)
                        {
                            sb.Append($"SKU：{inventory.tb_proddetail.SKU}仓库：{inventory.Location_ID}" +
                                $"未发数量由{inventory.NotOutQty}修复为:{totalNotOutQty}" + "\r\n");
                            inventory.NotOutQty = totalNotOutQty.ToInt();
                            UpdateInventories.Add(inventory);
                        }

                    }

                    #endregion

                    richTextBoxLog.AppendText(sb.ToString());
                    if (chkTestMode.Checked)
                    {
                        richTextBoxLog.AppendText($"拟销在制在途修复 数据行：{UpdateInventories.Count} " + "\r\n");
                    }
                    else
                    {
                        int plancounter = await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(UpdateInventories)
                            .UpdateColumns(it => new { it.Sale_Qty, it.On_the_way_Qty, it.NotOutQty, it.MakingQty }).ExecuteCommandAsync();
                        richTextBoxLog.AppendText($"拟销在制在途修复 数据行：{plancounter} " + "\r\n");
                    }
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
                    dataGridView1.DataSource = await CostFix(false);
                }

                if (treeView1.SelectedNode.Text == "销售订单成本数量修复")
                {
                    List<tb_SaleOrderDetail> saleOrderDetails = new List<tb_SaleOrderDetail>();
                    List<tb_SaleOrder> updatelist = new List<tb_SaleOrder>();
                    #region 销售订单数量与明细数量和的检测
                    List<tb_SaleOrder> SaleOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                        .Includes(c => c.tb_SaleOrderDetails)
                        .Where(c => c.SOrderNo == "SO250611751")
                       .ToListAsync();
                    foreach (tb_SaleOrder SaleOrder in SaleOrders)
                    {
                        for (int i = 0; i < SaleOrder.tb_SaleOrderDetails.Count; i++)
                        {
                            var detail = SaleOrder.tb_SaleOrderDetails[i];
                            bool needadd = false;
                            if (detail.TransactionPrice != detail.UnitPrice * detail.Discount)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细 成交价 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.TransactionPrice = detail.UnitPrice * detail.Discount;
                                needadd = true;
                            }


                            if (detail.SubtotalTransAmount != detail.TransactionPrice * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细  成交价小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
                                needadd = true;
                            }

                            if (detail.SubtotalTaxAmount != detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细税额小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.SubtotalTaxAmount = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                needadd = true;
                            }

                            if (detail.SubtotalCostAmount != (detail.Cost + detail.CustomizedCost) * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细成本小计 不对：{SaleOrder.SOrderNo}" + "\r\n");
                                detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;
                                needadd = true;
                            }
                            if (needadd)
                            {
                                saleOrderDetails.Add(detail);
                                needadd = false;
                            }
                        }


                        bool needaddmain = false;
                        if (!SaleOrder.TotalQty.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity)))
                        {
                            SaleOrder.TotalQty = SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity);

                            richTextBoxLog.AppendText($"销售订单 总数量不对：{SaleOrder.SOrderNo}" + "\r\n");
                            needaddmain = true;
                        }

                        if (!SaleOrder.TotalCommissionAmount.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.CommissionAmount)))
                        {
                            SaleOrder.TotalCommissionAmount = SaleOrder.tb_SaleOrderDetails.Sum(c => c.CommissionAmount);
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总佣金不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }
                        if (!SaleOrder.TotalAmount.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) + SaleOrder.FreightIncome))
                        {
                            SaleOrder.TotalAmount = SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) + SaleOrder.FreightIncome;
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总成交价 不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }

                        if (!SaleOrder.TotalTaxAmount.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount)))
                        {
                            SaleOrder.TotalTaxAmount = SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount);
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总税额 不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }

                        if (!SaleOrder.TotalCost.Equals(SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount)))
                        {
                            SaleOrder.TotalCost = SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount);

                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售订单 总成本 不对：{SaleOrder.SOrderNo}" + "\r\n");
                        }
                        if (needaddmain)
                        {
                            updatelist.Add(SaleOrder);
                            needaddmain = false;
                        }
                    }


                    if (!chkTestMode.Checked)
                    {
                        if (saleOrderDetails.Count > 0)
                        {
                            int detailcounter = await MainForm.Instance.AppContext.Db.Updateable(saleOrderDetails).UpdateColumns(t => new { t.SubtotalCostAmount }).ExecuteCommandAsync();
                            if (detailcounter > 0)
                            {
                                richTextBoxLog.AppendText($"销售订单 明细 修复成功：{detailcounter} " + "\r\n");
                            }
                        }

                        if (updatelist.Count > 0)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist)
                           .UpdateColumns(t => new
                           {
                               t.TotalQty,
                               t.TotalCost,
                           }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"销售订单 主表 修复成功：{totalamountCounter} " + "\r\n");
                        }

                    }
                    else
                    {
                        richTextBoxLog.AppendText($"销售订单 主表{updatelist.Count} 明细 {saleOrderDetails.Count}  需要修复" + "\r\n");
                    }
                    #endregion
                }


                if (treeView1.SelectedNode.Text == "销售出库单成本数量修复")
                {
                    List<tb_SaleOutDetail> saleOutDetails = new List<tb_SaleOutDetail>();
                    List<tb_SaleOut> updatelist = new List<tb_SaleOut>();
                    #region 销售出库数量与明细数量和的检测
                    List<tb_SaleOut> SaleOuts = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                        .Includes(c => c.tb_SaleOutDetails)
                       .ToListAsync();
                    foreach (tb_SaleOut Saleout in SaleOuts)
                    {
                        for (int i = 0; i < Saleout.tb_SaleOutDetails.Count; i++)
                        {
                            var detail = Saleout.tb_SaleOutDetails[i];
                            bool needadd = false;
                            if (detail.TransactionPrice != detail.UnitPrice * detail.Discount)
                            {
                                richTextBoxLog.AppendText($"销售出库 明细 成交价 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.TransactionPrice = detail.UnitPrice * detail.Discount;
                                needadd = true;
                            }


                            if (detail.SubtotalTransAmount != detail.TransactionPrice * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售出库 明细  成交价小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
                                needadd = true;
                            }

                            if (detail.SubtotalTaxAmount != detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate)
                            {
                                decimal tempTax = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                decimal diffpirce = Math.Abs(detail.SubtotalTaxAmount - tempTax);
                                if (diffpirce > 0.01m)
                                {
                                    richTextBoxLog.AppendText($"销售出库 明细税额小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                    detail.SubtotalTaxAmount = detail.SubtotalTransAmount / (1 + detail.TaxRate) * detail.TaxRate;
                                    needadd = true;
                                }

                            }

                            if (detail.SubtotalCostAmount != (detail.Cost + detail.CustomizedCost) * detail.Quantity)
                            {
                                richTextBoxLog.AppendText($"销售出库 明细成本小计 不对：{Saleout.SaleOutNo}" + "\r\n");
                                detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;
                                needadd = true;
                            }
                            if (needadd)
                            {
                                saleOutDetails.Add(detail);
                                needadd = false;
                            }
                        }


                        bool needaddmain = false;
                        if (!Saleout.TotalQty.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.Quantity)))
                        {
                            Saleout.TotalQty = Saleout.tb_SaleOutDetails.Sum(c => c.Quantity);

                            richTextBoxLog.AppendText($"销售出库 总数量不对：{Saleout.SaleOutNo}" + "\r\n");
                            needaddmain = true;
                        }

                        if (!Saleout.TotalCommissionAmount.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.CommissionAmount)))
                        {
                            Saleout.TotalCommissionAmount = Saleout.tb_SaleOutDetails.Sum(c => c.CommissionAmount);
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售出库 总佣金不对：{Saleout.SaleOutNo}" + "\r\n");
                        }
                        if (!Saleout.TotalAmount.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount) + Saleout.FreightIncome))
                        {
                            Saleout.TotalAmount = Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount) + Saleout.FreightIncome;
                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售出库 总成交价 不对：{Saleout.SaleOutNo}" + "\r\n");
                        }

                        if (!Saleout.TotalTaxAmount.Equals(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount)))
                        {

                            decimal diffpirce = Math.Abs(Saleout.TotalTaxAmount - Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount));
                            if (diffpirce > 0.01m && ComparePrice(Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount).ToDouble(), Saleout.TotalTaxAmount.ToDouble()) > 10)
                            {
                                Saleout.TotalTaxAmount = Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);
                                needaddmain = true;
                                richTextBoxLog.AppendText($"销售出库 总税额 不对：{Saleout.SaleOutNo}" + "\r\n");
                            }

                        }

                        if (!Saleout.TotalCost.Equals((Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + Saleout.FreightCost)))
                        {
                            Saleout.TotalCost = Saleout.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount) + Saleout.FreightCost;

                            needaddmain = true;
                            richTextBoxLog.AppendText($"销售出库 总成本 不对：{Saleout.SaleOutNo}" + "\r\n");
                        }


                        if (needaddmain)
                        {
                            updatelist.Add(Saleout);
                            needaddmain = false;
                        }
                    }


                    if (!chkTestMode.Checked)
                    {
                        if (saleOutDetails.Count > 0)
                        {
                            int detailcounter = await MainForm.Instance.AppContext.Db.Updateable(saleOutDetails).UpdateColumns(t => new { t.SubtotalCostAmount }).ExecuteCommandAsync();
                            if (detailcounter > 0)
                            {
                                richTextBoxLog.AppendText($"销售出库 数量成本的检测 修复成功：{detailcounter} " + "\r\n");
                            }
                        }

                        if (updatelist.Count > 0)
                        {
                            int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist)
                           .UpdateColumns(t => new
                           {
                               t.TotalQty,
                               t.TotalCost,
                           }).ExecuteCommandAsync();
                            richTextBoxLog.AppendText($"销售出库 数量成本的检测 修复成功：{totalamountCounter} " + "\r\n");
                        }

                    }
                    else
                    {
                        richTextBoxLog.AppendText($"销售出库 主表{updatelist.Count} 明细 {saleOutDetails.Count}  需要修复" + "\r\n");
                    }
                    #endregion
                }


                if (treeView1.SelectedNode.Text == "配方数量成本的检测")
                {
                    List<tb_BOM_S> bomupdatelist = new();
                    #region 明细要等于主表中的数量的检测
                    List<tb_BOM_S> BOM_Ss = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                        .Includes(c => c.tb_BOM_SDetails)
                       .ToList();
                    foreach (tb_BOM_S bom in BOM_Ss.ToArray())
                    {
                        if (!bom.TotalMaterialQty.Equals(bom.tb_BOM_SDetails.Sum(c => c.UsedQty)))
                        {
                            richTextBoxLog.AppendText($"配方主次表数量不一致：{bom.BOM_ID}：{bom.BOM_No} new:{bom.tb_BOM_SDetails.Sum(c => c.UsedQty)} old{bom.TotalMaterialQty}" + "\r\n");
                            bom.TotalMaterialQty = bom.tb_BOM_SDetails.Sum(c => c.UsedQty);
                            bomupdatelist.Add(bom);
                        }

                        if (!bom.TotalMaterialCost.Equals(bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost)))
                        {

                            decimal diffpirce = Math.Abs(bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost) - bom.TotalMaterialCost);
                            if (diffpirce > 0.2m && ComparePrice(bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost).ToDouble(), bom.TotalMaterialCost.ToDouble()) > 10)
                            {
                                richTextBoxLog.AppendText($"=====成本相差较大：{bom.BOM_ID}：{bom.BOM_No}   new {bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost)}  old {bom.TotalMaterialCost}" + "\r\n");
                                richTextBoxLog.AppendText($"配方主次表材料成本不一致：{bom.BOM_ID}：{bom.BOM_No}   new {bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost)}  old {bom.TotalMaterialCost}" + "\r\n");
                            }


                            bom.TotalMaterialCost = bom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                            bom.OutProductionAllCosts = bom.TotalMaterialCost + bom.TotalOutManuCost + bom.OutApportionedCost;
                            bom.SelfProductionAllCosts = bom.TotalMaterialCost + bom.TotalSelfManuCost + bom.SelfApportionedCost;
                            bomupdatelist.Add(bom);
                        }
                    }

                    #endregion
                    if (!chkTestMode.Checked)
                    {
                        int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(bomupdatelist)
                            .UpdateColumns(t => new
                            {
                                t.TotalMaterialQty,
                                t.TotalMaterialCost,
                                t.OutProductionAllCosts,
                                t.SelfProductionAllCosts
                            }).ExecuteCommandAsync();
                        richTextBoxLog.AppendText($"修复配方数量成本的检测 修复成功：{totalamountCounter} " + "\r\n");
                    }

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

                    #region 销售出库数量与明细数量和的检测
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

                    #region 
                    List<tb_CustomerVendor> CustomerVendors = MainForm.Instance.AppContext.Db.Queryable<tb_CustomerVendor>()
                        .IncludesAllFirstLayer()
                        .Where(c => c.IsCustomer == true && !c.CVName.Contains("信保"))
                        .ToList();
                    List<tb_CRM_Customer> customers = new List<tb_CRM_Customer>();
                    foreach (var Customer in CustomerVendors)
                    {

                        tb_CRM_Customer entity = MainForm.Instance.mapper.Map<tb_CRM_Customer>(Customer);
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

                if (treeView1.SelectedNode.Text == "佣金数据修复[tb_SaleOrder]")
                {
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_SaleOrder).Name)
                    {
                        List<long> ids = new List<long>();
                        List<tb_SaleOrderDetail> updateDetaillist = new();
                        List<tb_SaleOrderDetail> allDetailList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrderDetail>()
                       .Where(c => c.CommissionAmount > 0 && c.UnitCommissionAmount == 0)
                      .ToListAsync();
                        for (int o = 0; o < allDetailList.Count; o++)
                        {
                            var detail = allDetailList[o];
                            detail.UnitCommissionAmount = detail.CommissionAmount / detail.Quantity;
                            updateDetaillist.Add(detail);
                            if (!ids.Contains(detail.SOrder_ID))
                            {
                                ids.Add(detail.SOrder_ID);
                            }
                        }

                        //修复缴库库明细和等于主表的总数量
                        List<tb_SaleOrder> yjList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                            .Where(c => ids.Contains(c.SOrder_ID))
                            .Includes(c => c.tb_SaleOrderDetails)
                            .ToListAsync();
                        List<tb_SaleOrder> updatelist = new();
                        for (int i = 0; i < yjList.Count; i++)
                        {
                            yjList[i].TotalCommissionAmount = yjList[i].tb_SaleOrderDetails.Sum(c => c.CommissionAmount);
                            updatelist.Add(yjList[i]);
                        }
                        int totalamountCounter = 0;
                        int totaldetailcounter = 0;
                        if (!chkTestMode.Checked)
                        {
                            totaldetailcounter = await MainForm.Instance.AppContext.Db.Updateable(updateDetaillist).UpdateColumns(t => new { t.UnitCommissionAmount }).ExecuteCommandAsync();
                            totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalCommissionAmount }).ExecuteCommandAsync();
                        }
                        richTextBoxLog.AppendText($"销售订单佣金数据修复 明细 修复成功：{totaldetailcounter} " + "\r\n");
                        richTextBoxLog.AppendText($"销售订单佣金数据修复 主表 修复成功：{totalamountCounter} " + "\r\n");
                    }
                }
                if (treeView1.SelectedNode.Text == "佣金数据修复[tb_SaleOut]")
                {
                    if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_SaleOut).Name)
                    {
                        List<long> ids = new List<long>();
                        List<tb_SaleOutDetail> updateDetaillist = new();
                        List<tb_SaleOutDetail> allDetailList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutDetail>()
                       .Where(c => c.CommissionAmount > 0 && c.UnitCommissionAmount == 0)
                      .ToListAsync();
                        for (int o = 0; o < allDetailList.Count; o++)
                        {
                            var detail = allDetailList[o];
                            detail.UnitCommissionAmount = detail.CommissionAmount / detail.Quantity;
                            updateDetaillist.Add(detail);
                            if (!ids.Contains(detail.SaleOut_MainID))
                            {
                                ids.Add(detail.SaleOut_MainID);
                            }
                        }

                        //修复缴库库明细和等于主表的总数量
                        List<tb_SaleOut> yjList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                            .Where(c => ids.Contains(c.SaleOut_MainID))
                            .Includes(c => c.tb_SaleOutDetails)
                            .ToListAsync();
                        List<tb_SaleOut> updatelist = new();
                        for (int i = 0; i < yjList.Count; i++)
                        {
                            yjList[i].TotalCommissionAmount = yjList[i].tb_SaleOutDetails.Sum(c => c.CommissionAmount);
                            updatelist.Add(yjList[i]);
                        }
                        int totalamountCounter = 0;
                        int totaldetailcounter = 0;
                        if (!chkTestMode.Checked)
                        {
                            totaldetailcounter = await MainForm.Instance.AppContext.Db.Updateable(updateDetaillist).UpdateColumns(t => new { t.UnitCommissionAmount }).ExecuteCommandAsync();
                            totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalCommissionAmount }).ExecuteCommandAsync();
                        }
                        richTextBoxLog.AppendText($"出库佣金数据修复 明细 修复成功：{totaldetailcounter} " + "\r\n");
                        richTextBoxLog.AppendText($"出库佣金数据修复 主表 修复成功：{totalamountCounter} " + "\r\n");
                    }
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
                if (treeView1.SelectedNode.Text == "采购订单未交数量修复")
                {
                    List<tb_PurOrderDetail> updateDetaillist = new();
                    List<tb_PurOrder> yjList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                        .Includes(c => c.tb_PurOrderDetails)
                        .ToListAsync();

                    List<tb_PurOrder> updatelist = new();
                    for (int i = 0; i < yjList.Count; i++)
                    {
                        for (int j = 0; j < yjList[i].tb_PurOrderDetails.Count; j++)
                        {
                            yjList[i].tb_PurOrderDetails[j].UndeliveredQty = yjList[i].tb_PurOrderDetails[j].Quantity - yjList[i].tb_PurOrderDetails[j].DeliveredQuantity;
                            if (yjList[i].tb_PurOrderDetails[j].UndeliveredQty > 0)
                            {
                                updateDetaillist.Add(yjList[i].tb_PurOrderDetails[j]);
                            }
                        }

                        if (yjList[i].TotalUndeliveredQty != yjList[i].tb_PurOrderDetails.Sum(c => c.UndeliveredQty))
                        {
                            yjList[i].TotalUndeliveredQty = yjList[i].tb_PurOrderDetails.Sum(c => c.UndeliveredQty);
                            updatelist.Add(yjList[i]);
                        }

                    }

                    int totalmasterCounter = 0;
                    int totaldetailcounter = 0;
                    if (!chkTestMode.Checked)
                    {
                        totaldetailcounter = await MainForm.Instance.AppContext.Db.Updateable(updateDetaillist).UpdateColumns(t => new { t.UndeliveredQty }).ExecuteCommandAsync();
                        totalmasterCounter = await MainForm.Instance.AppContext.Db.Updateable(updatelist).UpdateColumns(t => new { t.TotalUndeliveredQty }).ExecuteCommandAsync();
                    }
                    richTextBoxLog.AppendText($"采购订单未交数量修复 明细 修复成功：{totaldetailcounter} " + "\r\n");
                    richTextBoxLog.AppendText($"采购订单未交数量修复 主表 修复成功：{totalmasterCounter} " + "\r\n");
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

                if (treeView1.SelectedNode.Text == "清空业务数据")
                {
                    //核销表 付款表  应收付表 预收付表
                    var StatementController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementController<tb_FM_Statement>>();
                    var StatementList = StatementController.QueryByNav(c => c.StatementId > 0);
                    richTextBoxLog.AppendText($"即将清空业务数据 核销表：{StatementList.Count} " + "\r\n");

                    var PaymentRecordController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    var PaymentRecordList = PaymentRecordController.QueryByNav(c => c.PaymentId > 0);
                    richTextBoxLog.AppendText($"即将清空业务数据 付款表：{PaymentRecordList.Count} " + "\r\n");

                    var ReceivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                    var ReceivablePayableList = ReceivablePayableController.QueryByNav(c => c.ARAPId > 0);
                    richTextBoxLog.AppendText($"即将清空业务数据 应收付表：{ReceivablePayableList.Count} " + "\r\n");

                    var PreReceivedPaymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    var PreReceivedPaymentList = PreReceivedPaymentController.QueryByNav(c => c.PreRPID > 0);
                    richTextBoxLog.AppendText($"即将清空业务数据 预收付表：{PreReceivedPaymentList.Count} " + "\r\n");

                    int totalamountCounter = 0;
                    //销售出库 销售订单
                    if (!chkTestMode.Checked)
                    {
                        totalamountCounter += StatementList.Count;
                        totalamountCounter += PaymentRecordList.Count;
                        totalamountCounter += ReceivablePayableList.Count;
                        totalamountCounter += PreReceivedPaymentList.Count;
                        for (int i = 0; i < StatementList.Count; i++)
                        {
                            await StatementController.BaseDeleteByNavAsync(StatementList[i]);
                        }

                        for (int i = 0; i < PaymentRecordList.Count; i++)
                        {
                            await PaymentRecordController.BaseDeleteByNavAsync(PaymentRecordList[i]);
                        }

                        for (int i = 0; i < ReceivablePayableList.Count; i++)
                        {
                            await ReceivablePayableController.BaseDeleteByNavAsync(ReceivablePayableList[i]);
                        }

                        for (int i = 0; i < PreReceivedPaymentList.Count; i++)
                        {
                            await PreReceivedPaymentController.BaseDeleteByNavAsync(PreReceivedPaymentList[i]);
                        }


                        richTextBoxLog.AppendText($"修复生产计划总数量 修复成功：{totalamountCounter} " + "\r\n");
                    }
                }


            }
        }

        private async Task<List<tb_Inventory>> CostFix(bool updateAllRows = false, string SKU = "", long ProdDetailID = 0)
        {
            List<tb_Inventory> Allitems = new List<tb_Inventory>();
            try
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.BeginTran();
                }
                //成本修复思路
                //1）成本本身修复，将所有入库明细按加权平均算一下。更新到库存里面。
                //2）修复所有出库明细，主要是销售出库，当然还有其它，比方借出，成本金额是重要的指标数据
                //3）成本修复 分  成品 外采和生产  因为这两种成本产生的方式不一样
                #region 成本本身修复
                Allitems = MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                           .AsNavQueryable()
                           .Includes(c => c.tb_proddetail, d => d.tb_PurEntryDetails, e => e.tb_proddetail, f => f.tb_prod)
                           .Includes(c => c.tb_proddetail, d => d.tb_prod)
                           .WhereIF(!string.IsNullOrEmpty(SKU), c => c.tb_proddetail.SKU == SKU)
                            .WhereIF(ProdDetailID > 0, c => c.ProdDetailID == ProdDetailID)
                           // .IFWhere(c => c.tb_proddetail.SKU == "SKU7E881B4629")
                           .ToList();

                List<tb_Inventory> updateInvList = new List<tb_Inventory>();
                foreach (tb_Inventory item in Allitems)
                {
                    if (item.tb_proddetail.tb_PurEntryDetails.Count > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.UnitPrice) > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity) > 0
                        )
                    {
                        //参与成本计算的入库明细记录。要排除单价为0的项
                        var realDetails = item.tb_proddetail.tb_PurEntryDetails.Where(c => c.UnitPrice > 0).ToList();

                        //每笔的入库的数量*成交价/总数量
                        var transPrice = realDetails
                            .Where(c => c.Quantity > 0 && c.UnitPrice > 0)
                            .Sum(c => c.UnitPrice * c.Quantity) / realDetails.Sum(c => c.Quantity);
                        if (transPrice > 0)
                        {
                            //百分比
                            decimal diffpirce = Math.Abs(transPrice - item.Inv_Cost);
                            diffpirce = Math.Round(diffpirce, 2);
                            double percentDiff = ComparePrice(item.Inv_Cost.ToDouble(), transPrice.ToDouble());
                            if (percentDiff > 10)
                            {
                                richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                item.CostMovingWA = transPrice;
                                item.Inv_AdvCost = item.CostMovingWA;
                                item.Inv_Cost = item.CostMovingWA;
                                item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;

                                updateInvList.Add(item);
                            }
                        }
                    }
                }
                if (chkTestMode.Checked)
                {
                    richTextBoxLog.AppendText($"要修复的行数为:{Allitems.Count}" + "\r\n");
                }
                if (!chkTestMode.Checked)
                {
                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updateInvList).UpdateColumns(t => new { t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                    richTextBoxLog.AppendText($"修复成本价格成功：{totalamountCounter} " + "\r\n");
                }
                #endregion

                if (updateAllRows)
                {
                    #region 更新相关数据

                    #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                    foreach (var child in updateInvList)
                    {

                        List<tb_BOM_S> orders = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                    .InnerJoin<tb_BOM_SDetail>((a, b) => a.BOM_ID == b.BOM_ID)
                    .Includes(a => a.tb_BOM_SDetails)
                    .Where(a => a.tb_BOM_SDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                        var distinctbills = orders
                        .GroupBy(o => o.BOM_ID)
                        .Select(g => g.First())
                        .ToList();

                        List<tb_BOM_SDetail> updateListbomdetail = new List<tb_BOM_SDetail>();
                        foreach (var bill in distinctbills)
                        {

                            foreach (var bomDetail in bill.tb_BOM_SDetails)
                            {
                                if (bomDetail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(bomDetail.UnitCost - child.Inv_Cost);
                                    if (diffpirce > 0.2m)
                                    {
                                        bomDetail.UnitCost = child.Inv_Cost;
                                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                        updateListbomdetail.Add(bomDetail);
                                    }
                                }
                            }

                            if (updateListbomdetail.Count > 0)
                            {
                                bill.TotalMaterialCost = bill.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                                bill.OutProductionAllCosts = bill.TotalMaterialCost + bill.TotalOutManuCost + bill.OutApportionedCost;
                                bill.SelfProductionAllCosts = bill.TotalMaterialCost + bill.TotalSelfManuCost + bill.SelfApportionedCost;
                                if (!chkTestMode.Checked)
                                {
                                    await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_S>(bill).ExecuteCommandAsync();
                                }
                            }
                        }

                        if (!chkTestMode.Checked && updateListbomdetail.Count > 0)
                        {
                            await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_SDetail>(updateListbomdetail).ExecuteCommandAsync();
                        }
                    }

                    #endregion

                    #region 更新制令单价格,和BOM类似

                    foreach (var child in updateInvList)
                    {

                        List<tb_ManufacturingOrder> orders = MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                    .InnerJoin<tb_ManufacturingOrderDetail>((a, b) => a.MOID == b.MOID)
                    .Includes(a => a.tb_ManufacturingOrderDetails)
                    .Where(a => a.tb_ManufacturingOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                        var distinctbills = orders
                        .GroupBy(o => o.MOID)
                        .Select(g => g.First())
                        .ToList();

                        List<tb_ManufacturingOrderDetail> updateListdetail = new List<tb_ManufacturingOrderDetail>();
                        foreach (var bill in distinctbills)
                        {
                            foreach (tb_ManufacturingOrderDetail Detail in bill.tb_ManufacturingOrderDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                    {
                                        Detail.UnitCost = child.Inv_Cost;
                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                        updateListdetail.Add(Detail);
                                    }
                                }
                            }

                            bill.TotalMaterialCost = bill.tb_ManufacturingOrderDetails.Sum(c => c.SubtotalUnitCost);
                            bill.TotalProductionCost = bill.TotalMaterialCost + bill.TotalManuFee;

                            if (!chkTestMode.Checked && updateListdetail.Count > 0)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_ManufacturingOrder>(bill).ExecuteCommandAsync();
                            }
                        }
                        if (updateListdetail.Count > 0)
                        {
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_ManufacturingOrderDetail>(updateListdetail).ExecuteCommandAsync();
                            }
                        }
                    }

                    #endregion

                    #region 更新缴库单价格,和BOM类似,  要再计算缴款的成品的成本 再反向更新库存的成本 这种一般是有BOM的

                    foreach (var child in updateInvList)
                    {
                        List<tb_FinishedGoodsInv> orders = MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                        .InnerJoin<tb_FinishedGoodsInvDetail>((a, b) => a.FG_ID == b.FG_ID)
                        .Includes(a => a.tb_FinishedGoodsInvDetails, b => b.tb_proddetail, c => c.tb_Inventories)
                        .Where(a => a.tb_FinishedGoodsInvDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                        var distinctbills = orders
                        .GroupBy(o => o.FG_ID)
                        .Select(g => g.First())
                        .ToList();

                        List<tb_FinishedGoodsInvDetail> updateListdetail = new List<tb_FinishedGoodsInvDetail>();

                        foreach (var bill in distinctbills)
                        {
                            foreach (tb_FinishedGoodsInvDetail Detail in bill.tb_FinishedGoodsInvDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                    {
                                        Detail.MaterialCost = child.Inv_Cost;
                                        Detail.UnitCost = Detail.MaterialCost * Detail.ManuFee + Detail.ApportionedCost;
                                        Detail.ProductionAllCost = Detail.UnitCost * Detail.Qty;
                                        //这时可以算出缴库的产品的单位成本
                                        var nextInv = Detail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == Detail.Location_ID);
                                        if (nextInv != null)
                                        {
                                            nextInv.Inv_Cost = Detail.UnitCost;
                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(nextInv).ExecuteCommandAsync();
                                            }
                                        }

                                        updateListdetail.Add(Detail);
                                    }
                                }
                            }

                            bill.TotalMaterialCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.MaterialCost * c.Qty);
                            bill.TotalManuFee = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ManuFee * c.Qty);
                            bill.TotalApportionedCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ApportionedCost * c.Qty);
                            bill.TotalProductionCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ProductionAllCost);
                            //又进入下一轮更新了
                            if (!chkTestMode.Checked && updateListdetail.Count > 0)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_FinishedGoodsInv>(bill).ExecuteCommandAsync();
                            }
                        }
                        if (updateListdetail.Count > 0)
                        {
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_FinishedGoodsInvDetail>(updateListdetail).ExecuteCommandAsync();
                            }
                        }
                    }

                    #endregion


                    #region 销售订单 出库  退货 记录成本修复
                    foreach (var child in updateInvList)
                    {

                        List<tb_SaleOrder> orders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                         .InnerJoin<tb_SaleOrderDetail>((a, b) => a.SOrder_ID == b.SOrder_ID)
                        .Includes(a => a.tb_SaleOrderDetails)
                        .Includes(a => a.tb_SaleOuts, c => c.tb_SaleOutDetails)
                        .Includes(a => a.tb_SaleOuts, c => c.tb_SaleOutRes, d => d.tb_SaleOutReDetails)
                        .Where(a => a.tb_SaleOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                        var distinctOrders = orders
                        .GroupBy(o => o.SOrder_ID)
                        .Select(g => g.First())
                        .ToList();

                        richTextBoxLog.AppendText($"找到销售订单 {distinctOrders.Count} 条" + "\r\n");
                        foreach (var order in distinctOrders)
                        {
                            #region new

                            foreach (var Detail in order.tb_SaleOrderDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                        if (Detail.TaxRate > 0)
                                        {
                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                        }
                                    }
                                }
                            }
                            order.TotalCost = order.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount);
                            order.TotalAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount);
                            order.TotalQty = order.tb_SaleOrderDetails.Sum(c => c.Quantity);
                            order.TotalTaxAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount);
                            richTextBoxLog.AppendText($"销售订单{order.SOrderNo}总金额：{order.TotalCost} " + "\r\n");

                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrderDetail>(order.tb_SaleOrderDetails).ExecuteCommandAsync();
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(order).ExecuteCommandAsync();
                            }
                            #region 销售出库
                            if (order.tb_SaleOuts != null)
                            {
                                foreach (var SaleOut in order.tb_SaleOuts)
                                {
                                    foreach (var saleoutdetails in SaleOut.tb_SaleOutDetails)
                                    {
                                        if (saleoutdetails.ProdDetailID == child.ProdDetailID)
                                        {
                                            saleoutdetails.Cost = child.Inv_Cost;
                                            saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;

                                            saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                            if (saleoutdetails.TaxRate > 0)
                                            {
                                                saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                            }
                                        }
                                    }
                                    SaleOut.TotalCost = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount);
                                    SaleOut.TotalAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount);
                                    SaleOut.TotalQty = SaleOut.tb_SaleOutDetails.Sum(c => c.Quantity);
                                    SaleOut.TotalTaxAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);


                                    richTextBoxLog.AppendText($"销售出库{SaleOut.SaleOutNo}总金额：{SaleOut.TotalCost} " + "\r\n");
                                    #region 销售退回
                                    if (SaleOut.tb_SaleOutRes != null)
                                    {
                                        foreach (var SaleOutRe in SaleOut.tb_SaleOutRes)
                                        {
                                            foreach (var SaleOutReDetail in SaleOutRe.tb_SaleOutReDetails)
                                            {
                                                if (SaleOutReDetail.ProdDetailID == child.ProdDetailID)
                                                {
                                                    SaleOutReDetail.Cost = child.Inv_Cost;
                                                    SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                    SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                    if (SaleOutReDetail.TaxRate > 0)
                                                    {
                                                        SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                    }
                                                    SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                }
                                            }
                                            SaleOutRe.TotalAmount = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalTransAmount);
                                            SaleOutRe.TotalQty = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.Quantity);

                                            if (SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails != null)
                                            {
                                                foreach (var Refurbished in SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails)
                                                {
                                                    if (Refurbished.ProdDetailID == child.ProdDetailID)
                                                    {
                                                        Refurbished.Cost = child.Inv_Cost;
                                                        Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                    }
                                                }
                                            }


                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutRe>(SaleOutRe).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReDetail>(SaleOutRe.tb_SaleOutReDetails).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReRefurbishedMaterialsDetail>(SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails).ExecuteCommandAsync();
                                            }
                                            richTextBoxLog.AppendText($"销售退回{SaleOutRe.ReturnNo}总金额：{SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalCostAmount)} " + "\r\n");
                                        }

                                    }

                                    #endregion

                                    if (!chkTestMode.Checked)
                                    {
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(SaleOut).ExecuteCommandAsync();
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutDetail>(SaleOut.tb_SaleOutDetails).ExecuteCommandAsync();
                                    }
                                }

                            }
                            #endregion
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(order).ExecuteCommandAsync();
                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrderDetail>(order.tb_SaleOrderDetails).ExecuteCommandAsync();
                            }
                            #endregion
                        }

                    }
                    #endregion


                    #region 借出单 归还
                    foreach (var child in updateInvList)
                    {
                        List<tb_ProdBorrowing> orders = MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>()
                        .InnerJoin<tb_ProdBorrowingDetail>((a, b) => a.BorrowID == b.BorrowID)
                       .Includes(a => a.tb_ProdBorrowingDetails)
                       .Includes(a => a.tb_ProdReturnings, c => c.tb_ProdReturningDetails)
                       .Where(a => a.tb_ProdBorrowingDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                        var distinctbills = orders
                        .GroupBy(o => o.BorrowID)
                        .Select(g => g.First())
                        .ToList();
                        List<tb_ProdBorrowingDetail> updateListdetail = new List<tb_ProdBorrowingDetail>();
                        List<tb_ProdBorrowing> updateListMain = new List<tb_ProdBorrowing>();
                        foreach (var bill in distinctbills)
                        {
                            bool needupdate = false;
                            foreach (var Detail in bill.tb_ProdBorrowingDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                        updateListdetail.Add(Detail);
                                        needupdate = true;
                                    }
                                }
                            }

                            if (needupdate)
                            {
                                bill.TotalCost = bill.tb_ProdBorrowingDetails.Sum(c => c.SubtotalCostAmount);
                                updateListMain.Add(bill);
                            }

                            #region 归还单
                            if (bill.tb_ProdReturnings != null)
                            {
                                foreach (var borrow in bill.tb_ProdReturnings)
                                {
                                    foreach (var returning in borrow.tb_ProdReturningDetails)
                                    {
                                        if (returning.ProdDetailID == child.ProdDetailID)
                                        {
                                            returning.Cost = child.Inv_Cost;
                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                        }
                                    }
                                    borrow.TotalCost = borrow.tb_ProdReturningDetails.Sum(c => c.SubtotalCostAmount);

                                    if (!chkTestMode.Checked)
                                    {
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowing>(borrow).ExecuteCommandAsync();
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowingDetail>(borrow.tb_ProdReturningDetails).ExecuteCommandAsync();
                                    }
                                }
                            }
                            #endregion
                        }
                        if (!chkTestMode.Checked)
                        {
                            await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowing>(updateListMain).ExecuteCommandAsync();
                            await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowingDetail>(updateListdetail).ExecuteCommandAsync();
                        }
                    }

                    #endregion

                    #region 其它出库
                    foreach (var child in updateInvList)
                    {

                        List<tb_StockOut> orders = MainForm.Instance.AppContext.Db.Queryable<tb_StockOut>()
                        .InnerJoin<tb_StockOutDetail>((a, b) => a.MainID == b.MainID)
                        .Includes(a => a.tb_StockOutDetails)
                        .Where(a => a.tb_StockOutDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                        var distinctbills = orders
                        .GroupBy(o => o.MainID)
                        .Select(g => g.First())
                        .ToList();

                        foreach (var bill in distinctbills)
                        {
                            List<tb_StockOutDetail> updateListdetail = new List<tb_StockOutDetail>();
                            foreach (tb_StockOutDetail Detail in bill.tb_StockOutDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                        updateListdetail.Add(Detail);
                                    }
                                }

                            }
                            bill.TotalCost = bill.tb_StockOutDetails.Sum(c => c.SubtotalCostAmount);
                            if (updateListdetail.Count > 0)
                            {
                                if (!chkTestMode.Checked)
                                {
                                    await MainForm.Instance.AppContext.Db.Updateable<tb_StockOutDetail>(updateListdetail).ExecuteCommandAsync();
                                }
                            }

                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_StockOut>(bill).ExecuteCommandAsync();
                            }

                        }
                    }
                    #endregion


                    #region 领料单

                    foreach (var child in updateInvList)
                    {
                        List<tb_MaterialRequisition> orders = MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>()
                      .InnerJoin<tb_MaterialRequisitionDetail>((a, b) => a.MR_ID == b.MR_ID)
                      .Includes(a => a.tb_MaterialRequisitionDetails)
                      .Where(a => a.tb_MaterialRequisitionDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();
                        var distinctbills = orders
                        .GroupBy(o => o.MR_ID)
                        .Select(g => g.First())
                        .ToList();
                        List<tb_MaterialRequisitionDetail> updateListdetail = new List<tb_MaterialRequisitionDetail>();
                        foreach (var bill in distinctbills)
                        {
                            foreach (tb_MaterialRequisitionDetail Detail in bill.tb_MaterialRequisitionDetails)
                            {
                                if (Detail.ProdDetailID == child.ProdDetailID)
                                {
                                    //如果存在则更新 
                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                    {
                                        Detail.Cost = child.Inv_Cost;
                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                        updateListdetail.Add(Detail);
                                    }
                                }
                            }
                            if (updateListdetail.Count > 0)
                            {
                                bill.TotalCost = bill.tb_MaterialRequisitionDetails.Sum(c => c.SubtotalCost);
                            }
                            if (!chkTestMode.Checked && updateListdetail.Count > 0)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_MaterialRequisition>(bill).ExecuteCommandAsync();
                            }
                        }

                        if (updateListdetail.Count > 0)
                        {
                            if (!chkTestMode.Checked)
                            {
                                await MainForm.Instance.AppContext.Db.Updateable<tb_MaterialRequisitionDetail>(updateListdetail).ExecuteCommandAsync();
                            }
                        }
                    }
                    #endregion


                    #endregion

                }



                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.CommitTran();
                }

            }


            catch (Exception ex)
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.RollbackTran();
                }
                throw ex;
            }
            return Allitems;
        }


        //写一个方法来实现两个价格的比较 前一个为原价，后一个为最新价格。
        //求最新价格大于前的价格的百分比。价格是decimal类型
        private double ComparePrice(double oldPrice, double newPrice)
        {
            if (oldPrice < 0 || newPrice < 0)
            {
                //如果有负 直接要求更新
                return 100;
                //throw new ArgumentException("Prices cannot be negative.");
            }

            if (oldPrice == 0)
            {
                // 如果原价为 0，无法计算百分比增长
                // 根据需求返回 -1 或 throw 异常
                return -1; // 或者抛出定制异常
            }
            double diffpirce = Math.Abs(newPrice - oldPrice);
            double percentage = (diffpirce / oldPrice * 100);
            return Math.Round(percentage, 2); // 四舍五入到 2 位小数
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

        private async void 执行选中数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Text == "成本修复" && dataGridView1.CurrentRow != null
                && dataGridView1.CurrentRow.DataBoundItem is tb_Inventory inventory)
            {
                await CostFix(true, string.Empty, inventory.ProdDetailID);
            }
        }

        private void txtSearchKey_TextChanged(object sender, EventArgs e)
        {

            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.DataBoundItem is tb_Inventory Inventory)
                {
                    string keywords = txtSearchKey.Text.ToLower().Trim();
                    if (keywords.Length > 0)
                    {
                        if (Inventory.ProdDetailID.ToString().Contains(keywords))
                        {
                            dr.Selected = true;
                            // 滚动到选中行
                            dataGridView1.CurrentCell = dr.Cells[0]; // 设置当前单元格为该行的第一个单元格
                        }
                        else
                        {
                            dr.Selected = false;
                        }
                    }

                }
            }
        }
    }
}
