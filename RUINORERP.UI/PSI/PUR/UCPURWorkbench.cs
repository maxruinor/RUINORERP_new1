using AutoMapper;
using FastReport.Barcode;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using FastReport.Utils;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using Mysqlx.Expr;
using Netron.GraphLib;
using NPOI.OpenXmlFormats.Spreadsheet;
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
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Models;
using RUINORERP.UI.AdvancedQuery;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BI;
using RUINORERP.UI.Common;
using RUINORERP.UI.CRM;
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
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TransInstruction;
using XmlDocument = System.Xml.XmlDocument;

namespace RUINORERP.UI.PUR
{

    [MenuAttrAssemblyInfo("采购工作台", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理)]
    public partial class UCPURWorkbench : BaseForm.BaseQuery
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




        public UCPURWorkbench()
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
        private async Task<List<tb_PurOrder>> GetProductionPlan()
        {
            bool hasCondition = txtPurOrderNO.Text.Trim().Length > 0 ||
                                txtPurEntryNO.Text.Trim().Length > 0 ||
                                txtSaleOrderNO.Text.Trim().Length > 0 ||
                                txtBuyRequestNO.Text.Trim().Length > 0 ||
                                txtPURReNo.Text.Trim().Length > 0 ||
                                txtPurReEntryNo.Text.Trim().Length > 0;


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
            List<tb_PurOrder> SaleList = null;
            //Expression<Func<tb_PurEntry, bool>> expPE = null;// Expressionable.Create<tb_PurEntry>().ToExpression();
            Expression<Func<tb_PurOrder, bool>> expPO = null;// Expressionable.Create<tb_PurOrder>().ToExpression();
            Expression<Func<tb_PurEntryRe, bool>> expPERe = null;// Expressionable.Create<tb_PurEntryRe>().ToExpression();

            // 根据索引执行相应的查询逻辑
            switch (index)
            {
                //采购订单+采购入库单
                case 1:
                case 2:
                    if (expPO == null && (txtPurOrderNO.Text.Trim().Length > 0 || txtPurEntryNO.Text.Trim().Length > 0))
                    {
                        expPO = Expressionable.Create<tb_PurOrder>() //创建表达式
                      .ToExpression();
                    }
                    if (txtPurOrderNO.Text.Trim().Length > 0)
                    {
                        expPO = expPO.AndAlso(w => w.PurOrderNo.Contains(txtPurOrderNO.Text.Trim()));
                    }
                    if (txtPurEntryNO.Text.Trim().Length > 0)
                    {
                        expPO = expPO.AndAlso(w => w.tb_PurEntries.Any(d => d.PurEntryNo.Contains(txtPurEntryNO.Text.Trim())));
                    }
                    SaleList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                      .Includes(c => c.tb_employee)
                      .Includes(c => c.tb_customervendor)
                       .Includes(c => c.tb_department)
                      .Includes(c => c.tb_PurOrderDetails, d => d.tb_proddetail, f => f.tb_prod)
                      .Includes(c => c.tb_PurEntries, d => d.tb_PurEntryDetails)
                       .AsNavQueryable()
                      .Includes(c => c.tb_PurEntries, d => d.tb_PurEntryRes, f => f.tb_PurEntryReDetails)
                        .AsNavQueryable()
                        .Includes(c => c.tb_PurEntries, d => d.tb_PurEntryRes, f => f.tb_PurReturnEntries, g => g.tb_PurReturnEntryDetails)
                       .WhereIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了采购只看到自己的
                      .Where(expPO)
                      .Where(t => t.DataStatus == (int)DataStatus.确认)
                      .Where(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
                      .Where(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
                      .OrderBy(c => c.PurDate)
                      // .WithCache(60) // 缓存60秒
                      .ToPageListAsync(1, 1000);

                    break;
                case 3:
                    if (txtSaleOrderNO.Text.Trim().Length > 0)
                    {
                        List<tb_SaleOrder> SOList = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                             .WhereIF(txtSaleOrderNO.Text.Trim().Length > 0, w => w.SOrderNo.Contains(txtSaleOrderNO.Text.Trim()))
                             .ToListAsync();

                        long[] SOIds = SOList.Select(c => c.SOrder_ID).ToArray();
                        if (SOIds.Length > 0)
                        {
                            expPO = Expressionable.Create<tb_PurOrder>() //创建表达式
                         .AndIF(SOIds.Length > 0, c => c.SOrder_ID.HasValue && c.RefBizType == (int)BizType.销售订单 && SOIds.ToArray().Contains(c.RefBillID.Value))
                         .ToExpression();

                            goto case 1;
                        }
                    }
                    break;
                case 4:
                    //请购单
                    if (txtBuyRequestNO.Text.Trim().Length > 0)
                    {
                        List<tb_BuyingRequisition> BRList = await MainForm.Instance.AppContext.Db.Queryable<tb_BuyingRequisition>()
                             .WhereIF(txtBuyRequestNO.Text.Trim().Length > 0, w => w.PuRequisitionNo.Contains(txtBuyRequestNO.Text.Trim()))
                             .ToListAsync();
                        long[] BRIds = BRList.Select(c => c.PuRequisition_ID).ToArray();
                        if (BRIds.Length > 0)
                        {
                            expPO = Expressionable.Create<tb_PurOrder>() //创建表达式
                            .AndIF(BRIds.Length > 0, c => c.RefBillID.HasValue && c.RefBizType == (int)BizType.请购单 && BRIds.ToArray().Contains(c.RefBillID.Value))
                            .ToExpression();
                            goto case 1;
                        }
                    }
                    break;
                case 5:
                    //采购退货
                    if (txtPURReNo.Text.Trim().Length > 0 || expPERe != null)
                    {
                        if (txtPURReNo.Text.Trim().Length > 0 && expPERe == null)
                        {
                            expPERe = Expressionable.Create<tb_PurEntryRe>().ToExpression();
                        }

                        List<tb_PurEntryRe> ReturnList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryRe>()
                            .WhereIF(txtPURReNo.Text.Trim().Length > 0, w => w.PurEntryReNo.Contains(txtPURReNo.Text.Trim()))
                            .Where(expPERe)
                             .ToListAsync();
                        //找到下一级的IDS
                        long[] EntryIds = ReturnList.Where(c => c.PurEntryID.HasValue).Select(c => c.PurEntryID.Value).ToArray();
                        if (EntryIds.Length > 0)
                        {
                            Expression<Func<tb_PurEntry, bool>> expEntry = Expressionable.Create<tb_PurEntry>() //创建表达式
                         .AndIF(EntryIds.Length > 0, c => EntryIds.ToArray().Contains(c.PurEntryID))
                         .ToExpression();

                            List<tb_PurEntry> EntryList = await MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                                .Where(expEntry)
                                 .ToListAsync();

                            long[] POIDs = EntryList.Where(c => c.PurOrder_ID.HasValue).Select(c => c.PurOrder_ID.Value).ToArray();
                            if (POIDs.Length > 0)
                            {
                                expPO = Expressionable.Create<tb_PurOrder>() //创建表达式
                               .AndIF(POIDs.Length > 0, c => POIDs.ToArray().Contains(c.PurOrder_ID))
                               .ToExpression();
                                goto case 1;
                            }
                        }

                    }
                    break;
                case 6:
                    //采购退货入库
                    if (txtPurReEntryNo.Text.Trim().Length > 0)
                    {
                        var PurReEntrys = await MainForm.Instance.AppContext.Db.Queryable<tb_PurReturnEntry>()
                             .WhereIF(txtPurReEntryNo.Text.Trim().Length > 0, w => w.PurReEntryNo.Contains(txtPurReEntryNo.Text.Trim()))
                            .ToListAsync();

                        long[] PurEntryRe_IDs = PurReEntrys.Where(s => s.PurEntryRe_ID.HasValue).Select(c => c.PurEntryRe_ID.Value).ToArray();
                        if (PurEntryRe_IDs.Length > 0)
                        {
                            expPERe = Expressionable.Create<tb_PurEntryRe>() //创建表达式
                      .AndIF(PurEntryRe_IDs.Length > 0, c => PurEntryRe_IDs.ToArray().Contains(c.PurEntryRe_ID))
                      .ToExpression();

                            goto case 5;
                        }

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
            bool hasPPNO = txtPurOrderNO.Text.Trim().Length > 0;
            bool hasPDNO = txtPurEntryNO.Text.Trim().Length > 0;
            bool hasFGNO = txtSaleOrderNO.Text.Trim().Length > 0;
            bool hasMRNO = txtBuyRequestNO.Text.Trim().Length > 0;
            bool hasMRRENO = txtPURReNo.Text.Trim().Length > 0;
            bool hasMONO = txtPurReEntryNo.Text.Trim().Length > 0;
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


        public async void Query(List<tb_PurOrder> _PURList = null)
        {
            List<tb_PurOrder> purList = await GetProductionPlan();
            if (purList != null)
            {
                SubQuery(purList);
            }
        }
        public async void SubQuery(List<tb_PurOrder> List = null)
        {
            await uCPur.QueryData(List);
        }

        internal override void LoadQueryParametersToUI(object LoadItems)
        {
            if (LoadItems != null && LoadItems is List<tb_PurOrder> pplist)
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

        private ToolStripMenuItem tsmenuItemAddAnnotations;
        ContextMenuStrip newContextMenuStrip;
        UCPUR uCPur = new UCPUR();
        private void UCProdWorkbench_Load(object sender, EventArgs e)
        {

            if (this.DesignMode)
            {
                return;
            }
            else
            {
                newContextMenuStrip = new();
                tsmenuItemAddAnnotations = new ToolStripMenuItem("添加批注");
                tsmenuItemAddAnnotations.Click += new System.EventHandler(this.tsmenuItemAddAnnotations_Click);
                //插到前面
                newContextMenuStrip.Items.Insert(0, tsmenuItemAddAnnotations);

                kryptonDockableWorkspaceQuery.ShowMaximizeButton = false;
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
                    string xmlFilePath = "UCPURWorkbenchPersistence.xml";
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
                            case "采购":
                                // kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Left, Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                                kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "采购").ToArray());
                                break;
                        }
                    }
                }

                kryptonDockingManagerQuery.Cells.ForEach(c => c.Button.CloseButtonDisplay = ButtonDisplay.Hide);

            }
        }

        private async void tsmenuItemAddAnnotations_Click(object sender, EventArgs e)
        {
            if (uCPur.kryptonTreeGridView1.CurrentRow != null)
            {
                if (uCPur.kryptonTreeGridView1.CurrentRow.Tag.GetType().BaseType.Name == "BaseEntity")
                {
                    Type rowOjbType = uCPur.kryptonTreeGridView1.CurrentRow.Tag.GetType();
                    BizTypeMapper mapper = new BizTypeMapper();

                    object PKValue = uCPur.kryptonTreeGridView1.CurrentRow.Tag.GetPropertyValue(UIHelper.GetPrimaryKeyColName(rowOjbType));


                    object frm = Activator.CreateInstance(typeof(UCglCommentEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_gl_Comment> frmaddg = frm as BaseEditGeneric<tb_gl_Comment>;
                        frmaddg.CurMenuInfo = this.CurMenuInfo;
                        frmaddg.Text = "添加批注";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_gl_Comment>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_gl_Comment EntityInfo = obj as tb_gl_Comment;
                        EntityInfo.BusinessID = PKValue.ToLong();
                        EntityInfo.BizTypeID = (int)mapper.GetBizType(rowOjbType);
                        EntityInfo.DbTableName = rowOjbType.Name;
                        BusinessHelper.Instance.InitEntity(EntityInfo);
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;
                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            var entiry = await MainForm.Instance.AppContext.Db.Storageable(EntityInfo).DefaultAddElseUpdate().ExecuteReturnEntityAsync();
                            if (entiry.CommentID > 0)
                            {

                            }
                            else
                            {

                            }

                            //BaseController<tb_gl_Comment> ctrRecords = Startup.GetFromFacByName<BaseController<tb_gl_Comment>>(typeof(tb_gl_Comment).Name + "Controller");
                            //ReturnResults<tb_gl_Comment> result = await ctrRecords.BaseSaveOrUpdate(EntityInfo);
                            //if (result.Succeeded)
                            //{

                            //    MainForm.Instance.ShowStatusText("添加成功!");
                            //}
                            //else
                            //{
                            //    MainForm.Instance.ShowStatusText("添加失败!");
                            //}
                        }
                    }
                }
            }
        }

        private KryptonPage CreateCell()
        {
            uCPur.Name = "uCSale";
            uCPur.Dock = DockStyle.Fill;
            uCPur.kryptonTreeGridView1.RowHeadersVisible = true;
            uCPur.ContextMenuStrip = newContextMenuStrip;
            KryptonPage page = UIForKryptonHelper.NewPage("采购", 1, uCPur);
            //page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            page.ClearFlags(KryptonPageFlags.All);
            return page;
        }

        private void txtSearchKey_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in uCPur.kryptonTreeGridView1.GridNodes)
            {
                string keywords = txtSearchKey.Text.ToLower().Trim();
                if (keywords.Length > 0)
                {
                    if (HasSearchKey(item, keywords))
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        item.Visible = false;
                    }
                    // foreach  写一个方法来实现一些查找功能
                    //item.Nodes.ForEach(node =>
                    //{
                    //    if (node.Cells[4].Value.ToString().Contains(keywords)) // 假设我们正在查找"keywords"
                    //    {
                    //        item.Visible = true; // 找到节点后赋值
                    //    }
                    //    else
                    //    {
                    //        item.Visible = false;
                    //    }
                    //});


                }
                else
                {
                    item.Visible = true;
                }


            }
        }

        private bool HasSearchKey(KryptonTreeGridNodeRow item, string keywords)
        {
            bool rs = false;

            foreach (var node in item.Nodes)
            {
                if (node.Cells[4].Value.ToString().ToLower().Contains(keywords)) // 假设我们正在查找"keywords"
                {
                    rs = true;
                    break;
                }
                else
                {
                    rs = false;
                    break;
                }
            }
            //item.Nodes.ForEach(node =>
            //{

            //});
            return rs;
        }

    }
}