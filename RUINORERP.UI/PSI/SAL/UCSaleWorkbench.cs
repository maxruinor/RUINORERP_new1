﻿using AutoMapper;
using FastReport.Barcode;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using FastReport.Utils;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.X509.Qualified;
using RUINOR.Core;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Models;
using RUINORERP.UI.AdvancedQuery;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.FormProperty;
using RUINORERP.UI.Report;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserCenter.DataParts;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XmlDocument = System.Xml.XmlDocument;

namespace RUINORERP.UI.SAL
{

    [MenuAttrAssemblyInfo("销售工作台", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.销售管理)]
    public partial class UCSaleWorkbench : BaseForm.BaseQuery
    {

        public KryptonDockingWorkspace ws;


        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }


        /// <summary>
        /// 传入的是M,即主表类型的实体数据 
        /// </summary>
        /// <param name="obj"></param>
        public delegate void QueryRelatedRowHandler(object obj, System.Windows.Forms.BindingSource bindingSource);

        /// <summary>
        /// 查询引用单据明细
        /// </summary>
        [Browsable(true), Description("查询引用单据明细")]
        public event QueryRelatedRowHandler OnQueryRelatedChild;


        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        /// <summary>
        /// 比方采购入库，对应采购入库明细，
        /// 相关联的是 采购订单的明细，主单先不管
        /// 如果为空，则认为没有关联引用数据
        /// 第三方的子表类型
        /// </summary>
        public Type ChildRelatedEntityType;




        public UCSaleWorkbench()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    foreach (var item in BaseToolStrip.Items)
                    {
                        if (item is ToolStripButton)
                        {
                            ToolStripButton subItem = item as ToolStripButton;
                            subItem.Click += Item_Click;
                            UIHelper.ControlButton(CurMenuInfo, subItem);
                        }
                        if (item is ToolStripDropDownButton subItemDr)
                        {
                            UIHelper.ControlButton(CurMenuInfo, subItemDr);
                            subItemDr.Click += Item_Click;
                            //下一级
                            if (subItemDr.HasDropDownItems)
                            {
                                foreach (var sub in subItemDr.DropDownItems)
                                {
                                    ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                    UIHelper.ControlButton(CurMenuInfo, subStripMenuItem);
                                    subStripMenuItem.Click += Item_Click;
                                }
                            }
                        }
                        if (item is ToolStripSplitButton)
                        {
                            ToolStripSplitButton subItem = item as ToolStripSplitButton;
                            subItem.Click += Item_Click;
                            UIHelper.ControlButton(CurMenuInfo, subItem);
                            //下一级
                            if (subItem.HasDropDownItems)
                            {
                                foreach (var sub in subItem.DropDownItems)
                                {
                                    ToolStripItem subStripMenuItem = sub as ToolStripItem;
                                    subStripMenuItem.Click += Item_Click;
                                }
                            }
                        }
                    }

                    Krypton.Toolkit.KryptonButton button设置查询条件 = new Krypton.Toolkit.KryptonButton();
                    button设置查询条件.Text = "设置查询条件";
                    button设置查询条件.ToolTipValues.Description = "对查询条件进行个性化设置。";
                    button设置查询条件.ToolTipValues.EnableToolTips = true;
                    button设置查询条件.ToolTipValues.Heading = "提示";
                    button设置查询条件.Click += button设置查询条件_Click;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button设置查询条件);
                }
            }

        }
        private void button设置查询条件_Click(object sender, EventArgs e)
        {
            MenuPersonalizedSettings();
        }
        protected virtual void MenuPersonalizedSettings()
        {
            UserCenter.frmMenuPersonalization frmMenu = new UserCenter.frmMenuPersonalization();
            frmMenu.MenuPathKey = CurMenuInfo.ClassPath;
            if (frmMenu.ShowDialog() == DialogResult.OK)
            {
                if (!this.DesignMode)
                {
                    MenuPersonalization personalization = new MenuPersonalization();
                    UserGlobalConfig.Instance.MenuPersonalizationlist.TryGetValue(CurMenuInfo.ClassPath, out personalization);
                    if (personalization != null)
                    {
                        decimal QueryShowColQty = personalization.QueryConditionShowColsQty;
                        QueryDtoProxy = LoadQueryConditionToUI(QueryShowColQty);
                    }
                    else
                    {
                        QueryDtoProxy = LoadQueryConditionToUI(frmMenu.QueryShowColQty.Value);
                    }
                }
            }
        }


        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> MasterColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ChildColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ChildRelatedColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        public virtual void BuildColNameDataDictionary()
        {

        }

        public delegate void QueryHandler();

        [Browsable(true), Description("查询主表")]
        public event QueryHandler OnQuery;


        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            DoButtonClick(RUINORERP.Common.Helper.EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
        }


        #region 对查询条件进行个性化设置
        private async Task<List<tb_SaleOrder>> GetProductionPlan()
        {
            bool hasCondition = txtSaleOrderNO.Text.Trim().Length > 0 ||
                                txtBuyRequestNO.Text.Trim().Length > 0 ||
                                txtPurEntryNO.Text.Trim().Length > 0 ||
                                txtSaleOutNO.Text.Trim().Length > 0 ||
                                txtSaleOutReNO.Text.Trim().Length > 0 ||
                                txtPurOrderNO.Text.Trim().Length > 0;

            if (!hasCondition)
            {
                MessageBox.Show("请至少输入一个查询条件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            int index = CheckQueryPara();
            if (index == 0)
            {
                return null;
            }
            List<tb_SaleOrder> SaleList = null;
            Expression<Func<tb_SaleOrder, bool>> exp = null;
            Expression<Func<tb_PurOrder, bool>> expPO = Expressionable.Create<tb_PurOrder>().ToExpression();
            // 根据索引执行相应的查询逻辑
            switch (index)
            {
                case 1:
                case 2:
                    if (exp == null)
                    {
                        exp = Expressionable.Create<tb_SaleOrder>() //创建表达式
                      .ToExpression();
                    }
                    if (txtSaleOrderNO.Text.Trim().Length > 0)
                    {
                        exp = exp.AndAlso(w => w.SOrderNo.Contains(txtSaleOrderNO.Text.Trim()));
                    }
                    if (txtBuyRequestNO.Text.Trim().Length > 0)
                    {
                        exp = exp.AndAlso(w => w.tb_SaleOuts.Any(d => d.SaleOutNo.Contains(txtSaleOutNO.Text.Trim())));
                    }
                    SaleList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                      .Includes(c => c.tb_employee)
                      .Includes(c => c.tb_customervendor)
                       .Includes(c => c.tb_projectgroup)
                      .Includes(c => c.tb_SaleOrderDetails, d => d.tb_proddetail, f => f.tb_prod)
                      .Includes(c => c.tb_SaleOuts, d => d.tb_SaleOutDetails)

                       .AsNavQueryable()
                      .Includes(c => c.tb_PurOrders, d => d.tb_PurOrderDetails)
                      // .AsNavQueryable()
                      // .Includes(c => c.tb_ProductionDemands, d => d.tb_ProduceGoodsRecommendDetails, f => f.tb_ManufacturingOrders, c => c.tb_MaterialRequisitions)
                      //.WhereIF(txtPDNO.Text.Trim().Length > 0, w => w.tb_ProductionDemands.Any(d => d.PDNo.Contains(txtPDNO.Text.Trim())))
                      .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                      .Where(exp)
                      .OrderBy(c => c.SaleDate)
                       .WithCache(60) // 缓存60秒
                      .ToListAsync();

                    break;
                case 3:
                    //采购入单
                    if (txtPurEntryNO.Text.Trim().Length > 0)
                    {
                        List<tb_PurEntry> PurEntryList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                             .WhereIF(txtPurEntryNO.Text.Trim().Length > 0, w => w.PurEntryNo.Contains(txtPurEntryNO.Text.Trim()))
                             .ToListAsync();

                        long[] POIds = PurEntryList.Where(s => s.PurOrder_ID.HasValue).Select(c => c.PurOrder_ID.Value).ToArray();

                        Expression<Func<tb_PurOrder, bool>> expPOTemp = Expressionable.Create<tb_PurOrder>() //创建表达式
                          .AndIF(POIds.Length > 0, c => POIds.ToArray().Contains(c.PurOrder_ID))
                          .ToExpression();
                        expPO = expPOTemp;
                        goto case 6;
                    }
                    break;
                case 4:
                    //销售出库
                    if (txtSaleOutNO.Text.Trim().Length > 0)
                    {
                        List<tb_SaleOut> SaleOutList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                             .WhereIF(txtSaleOutNO.Text.Trim().Length > 0, w => w.SaleOutNo.Contains(txtSaleOutNO.Text.Trim()))
                             .ToListAsync();

                        long[] SOIds = SaleOutList.Where(c => c.SOrder_ID.HasValue).Select(c => c.SOrder_ID.Value).ToArray();

                        Expression<Func<tb_SaleOrder, bool>> expSO = Expressionable.Create<tb_SaleOrder>() //创建表达式
                          .AndIF(SOIds.Length > 0, c => SOIds.ToArray().Contains(c.SOrder_ID))
                          .ToExpression();
                        exp = expSO;
                        goto case 1;
                    }
                    break;
                case 5:
                    //销售退回单
                    if (txtSaleOutReNO.Text.Trim().Length > 0)
                    {
                        List<tb_SaleOutRe> ReturnList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutRe>()
                             .WhereIF(txtSaleOutReNO.Text.Trim().Length > 0, w => w.ReturnNo.Contains(txtSaleOutReNO.Text.Trim()))
                             .ToListAsync();
                        //找到下一级的IDS
                        long[] OutIds = ReturnList.Where(c => c.SaleOut_MainID.HasValue).Select(c => c.SaleOut_MainID.Value).ToArray();
                        Expression<Func<tb_SaleOut, bool>> expOut = Expressionable.Create<tb_SaleOut>() //创建表达式
                          .AndIF(OutIds.Length > 0, c => OutIds.ToArray().Contains(c.SaleOut_MainID))
                          .ToExpression();

                        List<tb_SaleOut> OutList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                            .Where(expOut)
                             .ToListAsync();

                        long[] SOIDlist = OutList.Where(c => c.SOrder_ID.HasValue).Select(c => c.SOrder_ID.Value).ToArray();

                        Expression<Func<tb_SaleOrder, bool>> expSO = Expressionable.Create<tb_SaleOrder>() //创建表达式
                          .AndIF(SOIDlist.Length > 0, c => SOIDlist.ToArray().Contains(c.SOrder_ID))
                          .ToExpression();
                        exp = expSO;
                        goto case 1;
                    }
                    break;
                case 6:
                    //采购订单
                    if (txtPurOrderNO.Text.Trim().Length > 0 || expPO != null)
                    {
                        var PurOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                             .WhereIF(txtPurOrderNO.Text.Trim().Length > 0, w => w.PurOrderNo.Contains(txtPurOrderNO.Text.Trim()))
                             .Where(expPO)
                            .ToListAsync();

                        long[] RefBillIds = PurOrders.Where(s => s.RefBillID.HasValue && s.RefBizType.HasValue && s.RefBizType.Value == (int)BizType.销售订单).Select(c => c.RefBillID.Value).ToArray();

                        Expression<Func<tb_SaleOrder, bool>> expSO = Expressionable.Create<tb_SaleOrder>() //创建表达式
                        .AndIF(RefBillIds.Length > 0, c => RefBillIds.ToArray().Contains(c.RefBillID.Value))
                        .ToExpression();
                        exp = expSO;
                        goto case 1;
                    }
                    break;
                default:
                    MessageBox.Show("请至少输入一个查询条件。");
                    break;
            }

            return SaleList;
        }

        private int CheckQueryPara()
        {
            bool hasPPNO = txtSaleOrderNO.Text.Trim().Length > 0;
            bool hasPDNO = txtBuyRequestNO.Text.Trim().Length > 0;
            bool hasFGNO = txtPurEntryNO.Text.Trim().Length > 0;
            bool hasMRNO = txtSaleOutNO.Text.Trim().Length > 0;
            bool hasMRRENO = txtSaleOutReNO.Text.Trim().Length > 0;
            bool hasMONO = txtPurOrderNO.Text.Trim().Length > 0;
            int index = 0;
            // 检查是否恰好有一个条件被输入
            bool exactlyOneCondition = hasPPNO ^ hasPDNO ^ hasFGNO ^ hasMRNO ^ hasMRRENO ^ hasMONO;

            if (exactlyOneCondition)
            {
                // 找到唯一一个非空条件的索引
                index =
                   (hasPPNO ? 1 : 0) +
                   (hasPDNO ? 2 : 0) +
                   (hasFGNO ? 3 : 0) +
                   (hasMRNO ? 4 : 0) +
                   (hasMRRENO ? 5 : 0) +
                   (hasMONO ? 6 : 0);
                return index;
            }
            else
            {
                MessageBox.Show("请确保只输入一个查询条件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return index;

        }

        #endregion


        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            MainForm.Instance.AppContext.log.ActionName = menuItem.ToString();
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {

            }
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

            switch (menuItem)
            {
                case MenuItemEnums.查询:
                    Query();
                    toolStripSplitButtonPrint.Enabled = true;
                    break;
                case MenuItemEnums.复制性新增:

                    break;

                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.提交:

                    break;
                case MenuItemEnums.属性:
                    Property();
                    break;
                case MenuItemEnums.审核:


                    break;
                case MenuItemEnums.反审:

                    break;
                case MenuItemEnums.结案:

                    break;

                case MenuItemEnums.转入库单:

                    break;
                case MenuItemEnums.打印:

                    break;
                case MenuItemEnums.预览:

                    break;
                case MenuItemEnums.设计:

                    break;
                case MenuItemEnums.删除:

                    break;
                case MenuItemEnums.导出:

                    break;
                default:
                    break;
            }
        }











        protected frmFormProperty frm = new frmFormProperty();
        protected virtual void Property()
        {
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
                // ToolBarEnabledControl(MenuItemEnums.属性);
                //AuditLogHelper.Instance.CreateAuditLog<T>("属性", EditEntity);
            }
        }



        protected virtual void Exit(object thisform)
        {
            try
            {

            }
            catch (Exception ex)
            {


            }
            //退出
            CloseTheForm(thisform);

        }
        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            KryptonPage page = (thisform as Control).Parent as KryptonPage;
            MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
            page.Dispose();

        }


        public async void Query(List<tb_SaleOrder> _PURList = null)
        {
            List<tb_SaleOrder> purList = await GetProductionPlan();
            if (purList != null)
            {
                SubQuery(purList);
            }
        }
        public async void SubQuery(List<tb_SaleOrder> List = null)
        {
            await uCSale.QueryData(List);
        }

        internal override void LoadQueryParametersToUI(object LoadItems)
        {
            if (LoadItems != null && LoadItems is List<tb_SaleOrder> pplist)
            {
                SubQuery(pplist);
            }
        }

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);
                        break;
                    case Keys.F1:

                        break;
                    case Keys.Enter:
                        Query();
                        break;
                }

            }
            return false;
        }
        UCSale uCSale = new UCSale();
        private void UCProdWorkbench_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            else
            {
                // Setup docking functionality
                ws = kryptonDockingManagerQuery.ManageWorkspace(kryptonDockableWorkspaceQuery);
                kryptonDockingManagerQuery.ManageControl(kryptonPanelMainBig, ws);
                kryptonDockingManagerQuery.ManageFloating(MainForm.Instance);

                //创建面板并加入
                KryptonPageCollection Kpages = new KryptonPageCollection();
                if (Kpages.Count == 0)
                {
                    Kpages.Add(CreateCell());
                }

                //加载布局
                try
                {
                    //Location of XML file
                    string xmlFilePath = "UCSaleWorkbenchPersistence.xml";
                    if (System.IO.File.Exists(xmlFilePath) && AuthorizeController.GetQueryPageLayoutCustomize(MainForm.Instance.AppContext))
                    {
                        #region load
                        // Create the XmlNodeReader object.
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlFilePath);
                        XmlNodeReader nodeReader = new XmlNodeReader(doc);
                        // Set the validation settings.
                        XmlReaderSettings settings = new XmlReaderSettings();
                        //settings.ValidationType = ValidationType.Schema;
                        //settings.Schemas.Add("urn:bookstore-schema", "books.xsd");
                        //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                        //settings.NewLineChars = Environment.NewLine;
                        //settings.Indent = true;

                        using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                                {
                                    //加载停靠信息
                                    ws.LoadElementFromXml(reader, Kpages);
                                }
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        //没有个性化文件时用默认的
                        if (!string.IsNullOrEmpty(CurMenuInfo.DefaultLayout))
                        {
                            #region load
                            //加载XML文件
                            XmlDocument xmldoc = new XmlDocument();
                            //获取XML字符串
                            string xmlStr = xmldoc.InnerXml;
                            //字符串转XML
                            xmldoc.LoadXml(CurMenuInfo.DefaultLayout);

                            XmlNodeReader nodeReader = new XmlNodeReader(xmldoc);
                            XmlReaderSettings settings = new XmlReaderSettings();
                            using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                            {
                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                                    {
                                        //加载停靠信息
                                        ws.LoadElementFromXml(reader, Kpages);
                                    }
                                }

                            }
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog("加载查询页布局配置文件出错。" + ex.Message, Global.UILogType.错误);
                    MainForm.Instance.logger.LogError(ex, "加载查询页布局配置文件出错。");
                }

                //如果加载过的停靠信息中不正常。就手动初始化
                foreach (KryptonPage page in Kpages)
                {
                    if (!(page is KryptonStorePage) && !kryptonDockingManagerQuery.ContainsPage(page.UniqueName))
                    {
                        switch (page.UniqueName)
                        {
                            case "查询条件":
                                //kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                                //kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                                break;
                            case "销售":
                                // kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Left, Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                                kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "销售").ToArray());
                                break;
                        }
                    }
                }
            }
        }

        private KryptonPage CreateCell()
        {
            uCSale.Name = "uCSale";
            uCSale.Dock = DockStyle.Fill;
            uCSale.kryptonTreeGridView1.RowHeadersVisible = true;
            KryptonPage page = UIForKryptonHelper.NewPage("销售", 1, uCSale);
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            return page;
        }


    }
}