using AutoMapper;
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

namespace RUINORERP.UI.MRP
{

    [MenuAttrAssemblyInfo("生产工作台", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产)]
    public partial class UCProdWorkbench : BaseForm.BaseQuery
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




        public UCProdWorkbench()
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
        private async Task<List<tb_ProductionPlan>> GetProductionPlan()
        {
            bool hasCondition = txtPPNO.Text.Trim().Length > 0 ||
                                txtPDNO.Text.Trim().Length > 0 ||
                                txtFGNO.Text.Trim().Length > 0 ||
                                txtMRNO.Text.Trim().Length > 0 ||
                                txtMRRENO.Text.Trim().Length > 0 ||
                                txtMONO.Text.Trim().Length > 0;

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
            List<tb_ProductionPlan> PURList = null;
            Expression<Func<tb_ProductionPlan, bool>> exp = null;
            Expression<Func<tb_ManufacturingOrder, bool>> expMO = null;
            // 根据索引执行相应的查询逻辑
            switch (index)
            {
                case 1:
                case 2:
                    if (exp == null && (txtPPNO.Text.Trim().Length > 0 || txtPDNO.Text.Trim().Length > 0))
                    {
                        exp = Expressionable.Create<tb_ProductionPlan>() //创建表达式
                      .ToExpression();
                    }
                    if (txtPPNO.Text.Trim().Length > 0)
                    {
                        exp = exp.AndAlso(w => w.PPNo.Contains(txtPPNO.Text.Trim()));
                    }
                    if (txtPDNO.Text.Trim().Length > 0)
                    {
                        exp = exp.AndAlso(w => w.tb_ProductionDemands.Any(d => d.PDNo.Contains(txtPDNO.Text.Trim())));
                    }
                    PURList = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                      .Includes(c => c.tb_employee)
                      .Includes(c => c.tb_department)
                       .Includes(c => c.tb_projectgroup)
                      .Includes(c => c.tb_ProductionPlanDetails)
                      .Includes(c => c.tb_ProductionDemands, d => d.tb_ProductionDemandTargetDetails)
                      .AsNavQueryable()
                      .Includes(c => c.tb_ProductionDemands, d => d.tb_ProduceGoodsRecommendDetails, f => f.tb_ManufacturingOrders, e => e.tb_FinishedGoodsInvs, f => f.tb_FinishedGoodsInvDetails)
                      .AsNavQueryable()
                      .Includes(c => c.tb_ProductionDemands, d => d.tb_ProduceGoodsRecommendDetails, f => f.tb_ManufacturingOrders, c => c.tb_MaterialRequisitions)
                      //.WhereIF(txtPDNO.Text.Trim().Length > 0, w => w.tb_ProductionDemands.Any(d => d.PDNo.Contains(txtPDNO.Text.Trim())))
                      .Where(exp)
                      .OrderBy(c => c.RequirementDate)
                      .ToPageListAsync(1, 1000);

                    break;
                case 3:
                    //缴库单
                    if (txtFGNO.Text.Trim().Length > 0)
                    {
                        List<tb_FinishedGoodsInv> FinishedGoodsInvList = await MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                             .WhereIF(txtFGNO.Text.Trim().Length > 0, w => w.DeliveryBillNo.Contains(txtFGNO.Text.Trim()))
                             .ToListAsync();

                        long[] MOIds = FinishedGoodsInvList.Where(s => s.MOID.HasValue).Select(c => c.MOID.Value).ToArray();
                        if (MOIds.Length > 0)
                        {
                            Expression<Func<tb_ManufacturingOrder, bool>> expMOTemp = Expressionable.Create<tb_ManufacturingOrder>() //创建表达式
                                                      .AndIF(MOIds.Length > 0, c => MOIds.ToArray().Contains(c.MOID))
                                                      .ToExpression();
                            expMO = expMOTemp;
                            goto case 6;
                        }

                    }
                    break;
                case 4:
                    //领料单
                    if (txtMRNO.Text.Trim().Length > 0)
                    {
                        List<tb_MaterialRequisition> MaterialRequisitionList = await MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>()
                             .WhereIF(txtMRNO.Text.Trim().Length > 0, w => w.MaterialRequisitionNO.Contains(txtMRNO.Text.Trim()))
                             .ToListAsync();

                        long[] MOIds = MaterialRequisitionList.Select(c => c.MOID).ToArray();
                        if (MOIds.Length > 0)
                        {
                            Expression<Func<tb_ManufacturingOrder, bool>> expMOTemp = Expressionable.Create<tb_ManufacturingOrder>() //创建表达式
                          .AndIF(MOIds.Length > 0, c => MOIds.ToArray().Contains(c.MOID))
                          .ToExpression();
                            expMO = expMOTemp;
                            goto case 6;
                        }
                    }
                    break;
                case 5:
                    //退料单
                    if (txtMRRENO.Text.Trim().Length > 0)
                    {
                        List<tb_MaterialReturn> MaterialReturnList = await MainForm.Instance.AppContext.Db.Queryable<tb_MaterialReturn>()
                             .WhereIF(txtMRRENO.Text.Trim().Length > 0, w => w.MaterialRequisitionNO.Contains(txtMRRENO.Text.Trim()))
                             .ToListAsync();
                        //通过退料单找到领料单IDS
                        long[] MRIds = MaterialReturnList.Select(c => c.MR_ID).ToArray();
                        if (MRIds.Length > 0)
                        {
                            Expression<Func<tb_MaterialRequisition, bool>> expMR = Expressionable.Create<tb_MaterialRequisition>() //创建表达式
                                                      .AndIF(MRIds.Length > 0, c => MRIds.ToArray().Contains(c.MOID))
                                                      .ToExpression();

                            List<tb_MaterialRequisition> MaterialRequisitionList = await MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>()
                                .Where(expMR)
                                 .ToListAsync();

                            long[] MOIds = MaterialRequisitionList.Select(c => c.MOID).ToArray();
                            if (MOIds.Length > 0)
                            {
                                Expression<Func<tb_ManufacturingOrder, bool>> expMOTemp = Expressionable.Create<tb_ManufacturingOrder>() //创建表达式
                                .AndIF(MOIds.Length > 0, c => MOIds.ToArray().Contains(c.MOID))
                                .ToExpression();
                                expMO = expMOTemp;
                                goto case 6;
                            }
                        }

                    }
                    break;
                case 6:
                    //制令单
                    if (txtMONO.Text.Trim().Length > 0 || expMO != null)
                    {
                        if (txtMONO.Text.Trim().Length > 0 && expMO == null)
                        {
                            expMO = Expressionable.Create<tb_ManufacturingOrder>() //创建表达式
                            .ToExpression();
                            expMO = expMO.AndAlso(w => w.MONO.Contains(txtMONO.Text.Trim()));
                        }

                        var ManufacturingOrders = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                             .WhereIF(txtMONO.Text.Trim().Length > 0, w => w.MONO.Contains(txtMONO.Text.Trim()))
                             .Where(expMO)
                            .ToListAsync();

                        long[] PDIds = ManufacturingOrders.Where(s => s.PDID.HasValue).Select(c => c.PDID.Value).ToArray();
                        if (PDIds.Length > 0)
                        {
                            Expression<Func<tb_ProductionDemand, bool>> expPD = Expressionable.Create<tb_ProductionDemand>() //创建表达式
                        .AndIF(PDIds.Length > 0, c => PDIds.ToArray().Contains(c.PDID))
                        .ToExpression();

                            var ProductionDemands = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionDemand>().Where(expPD).ToListAsync();
                            long[] ppids = ProductionDemands.Select(c => c.PPID).ToArray();
                            if (ppids.Length > 0)
                            {
                                Expression<Func<tb_ProductionPlan, bool>> expPP = Expressionable.Create<tb_ProductionPlan>() //创建表达式
                           .AndIF(ppids.Length > 0, c => ppids.ToArray().Contains(c.PPID))
                           .ToExpression();
                                exp = expPP;
                                goto case 1;
                            }

                        }


                    }
                    break;
                default:
                    MessageBox.Show("请至少输入一个查询条件。");
                    break;
            }

            return PURList;
        }

        private int CheckQueryPara()
        {
            bool hasPPNO = txtPPNO.Text.Trim().Length > 0;
            bool hasPDNO = txtPDNO.Text.Trim().Length > 0;
            bool hasFGNO = txtFGNO.Text.Trim().Length > 0;
            bool hasMRNO = txtMRNO.Text.Trim().Length > 0;
            bool hasMRRENO = txtMRRENO.Text.Trim().Length > 0;
            bool hasMONO = txtMONO.Text.Trim().Length > 0;
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


        public async void Query(List<tb_ProductionPlan> _PURList = null)
        {
            List<tb_ProductionPlan> purList = await GetProductionPlan();
            if (purList != null)
            {
                SubQuery(purList);
            }
        }
        public async void SubQuery(List<tb_ProductionPlan> _PURList = null)
        {
            await uCMRP.QueryMRPDataStatus(_PURList);
        }

        internal override void LoadQueryParametersToUI(object LoadItems)
        {
            if (LoadItems != null && LoadItems is List<tb_ProductionPlan> pplist)
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
        UCMRP uCMRP = new UCMRP();
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
                    Kpages.Add(CreateMRP());
                }

                //加载布局
                try
                {
                    //Location of XML file
                    string xmlFilePath = "UCProdWorkbenchPersistence.xml";
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
                            case "生产":
                                // kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Left, Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                                kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "生产").ToArray());
                                break;
                        }
                    }
                }
            }
        }

        private KryptonPage CreateMRP()
        {
            uCMRP.Name = "uCMRP";
            uCMRP.Dock = DockStyle.Fill;
            uCMRP.kryptonTreeGridView1.RowHeadersVisible = true;
            KryptonPage page = NewPage("生产", 1, uCMRP);
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            return page;
        }

        private KryptonPage NewPage(string name, int image, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage();
            p.Text = name;
            p.TextTitle = name;
            p.TextDescription = name;
            p.UniqueName = p.Text;
            // p.ImageSmall = imageListSmall.Images[image];

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            // _count++;
            return p;
        }

    }
}