using AutoMapper;
using ExCSS;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.PTG;
using OfficeOpenXml;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Report;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UserCenter;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Xml;


namespace RUINORERP.UI.PSI.SAL
{

    /// <summary>
    /// 处理统计分析等各种情况
    /// 多个tabPage添加，自动生成条件
    /// 单表带条件查询 并且可以带分析outlook
    [MenuAttrAssemblyInfo("销售数据汇总", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.销售管理, BizType.销售数据汇总)]
    public partial class UCSalesDataSummary : UserControl
    {
        //是否可以放到主窗体 公用？

        // Colors used when hot tracking over tabs
        private Color _hotMain = Color.FromArgb(255, 240, 200);
        private Color _hotEmbedSelected = Color.FromArgb(255, 241, 224);
        private Color _hotEmbedTracking = Color.FromArgb(255, 231, 162);

        // 8 colors for when the tab is not selected
        private Color[] _normal = new Color[]{ Color.FromArgb(156, 193, 182), Color.FromArgb(247, 184, 134),
                                               Color.FromArgb(217, 173, 194), Color.FromArgb(165, 194, 215),
                                               Color.FromArgb(179, 166, 190), Color.FromArgb(234, 214, 163),
                                               Color.FromArgb(246, 250, 125), Color.FromArgb(188, 168, 225) };
        // 8 colors for when the tab is selected
        private Color[] _select = new Color[]{ Color.FromArgb(200, 221, 215), Color.FromArgb(251, 216, 188),
                                               Color.FromArgb(234, 210, 221), Color.FromArgb(205, 221, 233),
                                               Color.FromArgb(213, 206, 219), Color.FromArgb(244, 232, 204),
                                               Color.FromArgb(250, 252, 183), Color.FromArgb(218, 207, 239) };


        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> MasterColDisplayTypes { get; set; } = new List<Type>();

        /// <summary>
        /// 设置列显示相关的类型
        /// </summary>
        public virtual void BuildColDisplayTypes()
        {
            MasterColDisplayTypes.Add(typeof(tb_SaleOrder));
        }


        /// <summary>
        /// 查询结果右键实现及指向
        /// </summary>
        public virtual void SetContextMenuStrip()
        {

        }




        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        [MustOverride]
        protected virtual async void Query()
        {
            //var eer = errorProviderForAllInput.GetError(txtPDNo);

            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            if (kryptonTreeViewMenu.SelectedNode != null)
            {
                NavigatorMenu navigatorMenu = new NavigatorMenu();
                navigatorMenu = kryptonTreeViewMenu.SelectedNode.Tag as NavigatorMenu;

                switch (navigatorMenu.menuType)
                {
                    case NavigatorMenuType.销售订单数据汇总:

                        #region 销售订单数据汇总 
                        if (kryptonNavigator1.SelectedPage != null)
                        {
                            UCQueryShow queryShow = kryptonNavigator1.SelectedPage.Controls.Find(navigatorMenu.menuType.ToString() + "QueryShow", true).FirstOrDefault() as UCQueryShow;
                            if (queryShow != null)
                            {
                                if (ValidationHelper.hasValidationErrors(queryShow.kryptonPanelQuery.Controls))
                                    return;
                            }

                            UCBillMasterQuery queryMaster = kryptonNavigator1.SelectedPage.Controls.Find(navigatorMenu.menuType.ToString(), true).FirstOrDefault() as UCBillMasterQuery;
                            if (queryMaster != null)
                            {
                                GetSaleOrderDataListFromProc(queryMaster);

                            }
                        }


                        //带有output的存储过程 
                        /*
                        _UCOutlookGridAnalysis1.FieldNameList = UIHelper.GetFieldNameColList(typeof(Proc_SaleOrderStatisticsByEmployee));

                        List<string> SummaryColsForOrder = new List<string>();
                        SummaryColsForOrder.Add("退货数量");//这里要优化，按理可以是引用类型来处理
                        SummaryColsForOrder.Add("退货金额");//这里要优化，按理可以是引用类型来处理.
                        SummaryColsForOrder.Add("税额");//这里要优化，按理可以是引用类型来处理
                        SummaryColsForOrder.Add("实际成交数量");//这里要优化，按理可以是引用类型来处理
                        SummaryColsForOrder.Add("实际成交金额");//这里要优化，按理可以是引用类型来处理
                        _UCOutlookGridAnalysis1.kryptonOutlookGrid1.SubtotalColumns = SummaryColsForOrder;
                        _UCOutlookGridAnalysis1.ColDisplayTypes = new List<Type>();
                        //这个视图是用SQL语句生成的,用生成器。
                        _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(Proc_SaleOrderStatisticsByEmployee));
                        _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(tb_ProjectGroup));
                        _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(tb_SaleOrder));//业务员来自于这个表？
                        _UCOutlookGridAnalysis1.LoadDataToGrid<Proc_SaleOrderStatisticsByEmployee>(dataList);
                        //kryptonWorkspace1.ActivePage = kryptonWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == NavParts.结果分析1.ToString());
                        */

                        #endregion

                        break;

                    case NavigatorMenuType.销售出库业绩汇总:

                        #region 销售出库业绩汇总 
                        List<Proc_SaleOutStatisticsByEmployee> SaleOutList = GetSaleOutDataListFromProc();
                        if (kryptonNavigator1.SelectedPage != null)
                        {
                            UCQueryShow queryShow = kryptonNavigator1.SelectedPage.Controls.Find(navigatorMenu.menuType.ToString() + "QueryShow", true).FirstOrDefault() as UCQueryShow;
                            if (queryShow != null)
                            {
                                if (ValidationHelper.hasValidationErrors(queryShow.kryptonPanelQuery.Controls))
                                    return;
                            }

                            UCBillMasterQuery queryMaster = kryptonNavigator1.SelectedPage.Controls.Find(navigatorMenu.menuType.ToString(), true).FirstOrDefault() as UCBillMasterQuery;
                            if (queryMaster != null)
                            {
                                 
                                queryMaster.bindingSourceMaster.DataSource =SaleOutList.ToBindingSortCollection();
                                queryMaster.ShowSummaryCols();
                            }

                        }

                        //带有output的存储过程 
                        /*
                        _UCOutlookGridAnalysis1.FieldNameList = UIHelper.GetFieldNameColList(typeof(Proc_SaleOutStatisticsByEmployee));

                        List<string> SummaryCols = new List<string>();
                        SummaryCols.Add("退货数量");//这里要优化，按理可以是引用类型来处理
                        SummaryCols.Add("退货金额");//这里要优化，按理可以是引用类型来处理.
                        SummaryCols.Add("税额");//这里要优化，按理可以是引用类型来处理
                        SummaryCols.Add("实际成交数量");//这里要优化，按理可以是引用类型来处理
                        SummaryCols.Add("实际成交金额");//这里要优化，按理可以是引用类型来处理
                        _UCOutlookGridAnalysis1.kryptonOutlookGrid1.SubtotalColumns = SummaryCols;
                        _UCOutlookGridAnalysis1.ColDisplayTypes = new List<Type>();
                        //这个视图是用SQL语句生成的,用生成器。
                        _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(Proc_SaleOutStatisticsByEmployee));
                        _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(tb_ProjectGroup));
                        _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(tb_SaleOrder));//业务员来自于这个表？
                        _UCOutlookGridAnalysis1.LoadDataToGrid<Proc_SaleOutStatisticsByEmployee>(SaleOutList);
                        //kryptonWorkspace1.ActivePage = kryptonWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == NavParts.结果分析1.ToString());
                        */

                        #endregion



                        break;
                    default:
                        break;
                }


            }
        }

        public  List<Proc_SaleOrderStatisticsByEmployee> GetSaleOrderDataListFromProc(UCBillMasterQuery queryMaster)
        {

            //先固定两个特殊的查询条件集合
            List<Expression<Func<Proc_SaleOrderStatisticsByEmployee, object>>> expressions = new List<Expression<Func<Proc_SaleOrderStatisticsByEmployee, object>>>();
            expressions.Add(c => c.Employee_ID);
            expressions.Add(c => c.ProjectGroup_ID);

            List<string> conditions = ExpressionHelper.ExpressionListToStringList(expressions);



            //带有output的存储过程 
            Proc_SaleOrderStatisticsByEmployeePara paraOrder = UIparaOrder as Proc_SaleOrderStatisticsByEmployeePara;
            string strProjectGroups = string.Empty;

            List<string> GroupByFields = new List<string>();
            for (int i = 0; i < conditions.Count; i++)
            {
                bool pgGroupCanIgnore = paraOrder.GetPropertyValue(conditions[i] + "_CmbMultiChoiceCanIgnore").ToBool();
                if (pgGroupCanIgnore)
                {
                    GroupByFields.Add(conditions[i]);
                }
            }
            OrderInvisibleCols.Clear();
            //将条件都认为不可见
            OrderInvisibleCols.AddRange(conditions);
            //分组就要显示出来
            foreach (var item in GroupByFields)
            {
                OrderInvisibleCols.Remove(item);
            }


            List<object> ProjectGroupList = new List<object>();
            ProjectGroupList = paraOrder.GetPropertyValue("ProjectGroup_ID" + "_MultiChoiceResults") as List<object>;
            if (ProjectGroupList.Count > 0)
            {
                strProjectGroups = string.Join(",", ProjectGroupList.ToArray());
            }

            string strEmployees = string.Empty;
            List<object> EmployeeGroupList = new List<object>();
            EmployeeGroupList = paraOrder.GetPropertyValue("Employee_ID" + "_MultiChoiceResults") as List<object>;
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                if (!EmployeeGroupList.Contains(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString()))
                {
                    EmployeeGroupList.Add(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString());
                }
            }

            if (EmployeeGroupList.Count > 0)
            {
                strEmployees = string.Join(",", EmployeeGroupList.ToArray());
            }

            string GroupByFieldValue = string.Empty;
            if (GroupByFields.Count == 0)
            {
                GroupByFieldValue = "";
            }
            else if (GroupByFields.Count == 1)
            {
                GroupByFieldValue = GroupByFields[0];
            }
            else
            {
                GroupByFieldValue = "Both";
            }

            //-- 默认为''，可以是 'Employee_ID', 'ProjectGroup_ID' 或 '其中一个',如果为''则不分组
            var GroupByField = new SugarParameter("@GroupByField ", GroupByFieldValue);
            var ProjectGroups = new SugarParameter("@ProjectGroups ", strProjectGroups == string.Empty ? null : strProjectGroups);
            var Employees = new SugarParameter("@Employees ", strEmployees == string.Empty ? null : strEmployees);
            var StartQuery = new SugarParameter("@Start ", paraOrder.Start.ToString("yyyy-MM-dd"));//string 格式  '2024-04-01'
            var EndQuery = new SugarParameter("@End ", paraOrder.End.ToString("yyyy-MM-dd"));
            var sqloutput = new SugarParameter("@sqlOutput", null, true);//设置为output
            var SaleOrderDataList =  MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_SaleOrderStatisticsByEmployee>("Proc_SaleOrderStatisticsByEmployee",
                GroupByField, ProjectGroups, Employees, StartQuery, EndQuery, sqloutput);//返回List

            //动态 控制显示列
            foreach (var item in conditions)
            {
                ColumnDisplayController cdcInv = queryMaster.newSumDataGridViewMaster.ColumnDisplays.Where(c => c.ColName == item).FirstOrDefault();
                if (cdcInv != null)
                {
                    //分组存在就显示，不则不显示
                    cdcInv.Disable = GroupByFields.Any(c => c == item) ? false : true;
                }
            }


            queryMaster.bindingSourceMaster.DataSource = SaleOrderDataList.ToBindingSortCollection();
            queryMaster.ShowSummaryCols();

            return SaleOrderDataList;
        }

        public  List<Proc_SaleOutStatisticsByEmployee> GetSaleOutDataListFromProc()
        {
            //带有output的存储过程 
            Proc_SaleOutStatisticsByEmployeePara paraOut = UIparaOut as Proc_SaleOutStatisticsByEmployeePara;
            string strProjectGroups = string.Empty;
            List<object> ProjectGroupList = new List<object>();
            ProjectGroupList = paraOut.GetPropertyValue("ProjectGroup_ID" + "_MultiChoiceResults") as List<object>;
            if (ProjectGroupList.Count > 0)
            {
                strProjectGroups = string.Join(",", ProjectGroupList.ToArray());
            }

            string strEmployees = string.Empty;
            List<object> EmployeeGroupList = new List<object>();
            EmployeeGroupList = paraOut.GetPropertyValue("Employee_ID" + "_MultiChoiceResults") as List<object>;
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                if (!EmployeeGroupList.Contains(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString()))
                {
                    EmployeeGroupList.Add(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString());
                }
            }

            if (EmployeeGroupList.Count > 0)
            {
                strEmployees = string.Join(",", EmployeeGroupList.ToArray());
            }


            var ProjectGroups = new SugarParameter("@ProjectGroups ", strProjectGroups == string.Empty ? null : strProjectGroups);
            var Employees = new SugarParameter("@Employees ", strEmployees == string.Empty ? null : strEmployees);
            var StartQuery = new SugarParameter("@Start ", paraOut.Start.ToString("yyyy-MM-dd"));//string 格式  '2024-04-01'
            var EndQuery = new SugarParameter("@End ", paraOut.End.ToString("yyyy-MM-dd"));
            var sqloutput = new SugarParameter("@sqlOutput", null, true);//设置为output
                                                                         //var list = db.Ado.UseStoredProcedure().SqlQuery<Class1>("sp_school", nameP, ageP);//返回List
            var SaleOutList =  MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_SaleOutStatisticsByEmployee>("Proc_SaleOutStatisticsByEmployee"
                , ProjectGroups, Employees, StartQuery, EndQuery, sqloutput);//返回List
            return SaleOutList;
        }


        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        public bool WithOutlook { get; set; }
        /// <summary>
        /// 相关外键表或实体查询实体。如果是视图传入T时起作用
        /// </summary>
        public Type ReladtedEntityType
        {
            get; set;
        }

        public Navigator navigator = null;

        public UCSalesDataSummary()
        {
            navigator = BuildNavigatorMenu();

            if (this.DesignMode)
            {
                return;
            }
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    //权限菜单
                    if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                    {
                        CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.ClassPath == this.ToString()).FirstOrDefault();
                        if (CurMenuInfo == null)
                        {
                            MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                            return;
                        }
                    }
                    foreach (var item in BaseToolStrip.Items)
                    {
                        if (item is ToolStripButton)
                        {
                            ToolStripButton subItem = item as ToolStripButton;
                            subItem.Click += Item_Click;
                        }
                        if (item is ToolStripDropDownButton)
                        {
                            ToolStripDropDownButton subItem = item as ToolStripDropDownButton;
                            subItem.Click += Item_Click;
                            //下一级
                            if (subItem.HasDropDownItems)
                            {
                                foreach (var sub in subItem.DropDownItems)
                                {
                                    ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                    subStripMenuItem.Click += Item_Click;
                                }
                            }
                        }
                        //打印特殊处理
                        if (item is ToolStripSplitButton)
                        {
                            ToolStripSplitButton subItem = item as ToolStripSplitButton;
                            subItem.Click += Item_Click;
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
                }
            }

        }



        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> MasterColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();


        public List<KeyValuePair<object, string>> GetKeyValuePairs(Type enumType)
        {
            List<KeyValuePair<object, string>> kvlistPayStatus = new List<KeyValuePair<object, string>>();
            Array enumValues = Enum.GetValues(enumType);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            while (e.MoveNext())
            {
                currentValue = (int)e.Current;
                currentName = e.Current.ToString();
                kvlistPayStatus.Add(new KeyValuePair<object, string>(currentValue, currentName));
            }
            return kvlistPayStatus;
        }




        public delegate void QueryHandler();

        [Browsable(true), Description("查询主表")]
        public event QueryHandler OnQuery;


        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            DoButtonClick(RUINORERP.Common.Helper.EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
        }


        #region 为了性能 打印认为打印时 检测过的打印机相关配置在一个窗体下成功后。即可不每次检测
        private tb_PrintConfig printConfig = null;
        public tb_PrintConfig _PrintConfig
        {
            get
            {

                return printConfig;
            }
            set
            {
                printConfig = value;

            }
        }
        #endregion

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected async virtual void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                /*
                tb_MenuInfo menuInfo = MainForm.Instance.AppContext.CurUserInfo.UserMenuList.Where(c => c.MenuType == "行为菜单").Where(c => c.FormName == this.Name).FirstOrDefault();
                if (menuInfo == null)
                {
                    MessageBox.Show($"没有使用【{menuInfo.MenuName}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<tb_ButtonInfo> btnList = MainForm.Instance.AppContext.CurUserInfo.UserButtonList.Where(c => c.MenuID == menuInfo.MenuID).ToList();
                if (!btnList.Where(b => b.BtnText == menuItem.ToString()).Any())
                {
                    MessageBox.Show($"没有使用【{menuItem.ToString()}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }*/
            }

            switch (menuItem)
            {

                case MenuItemEnums.查询:
                    Query();
                    break;
                //case MenuItemEnums.高级查询:
                //    AdvQuery();
                //    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.提交:
                    Submit();
                    break;
                case MenuItemEnums.属性:

                    break;
                case MenuItemEnums.审核:
                    await Review();
                    break;


                case MenuItemEnums.打印:
                    Print(RptMode.PRINT);
                    break;
                case MenuItemEnums.预览:
                    Print(RptMode.PREVIEW);
                    break;
                case MenuItemEnums.设计:
                    Print(RptMode.DESIGN);
                    break;
                case MenuItemEnums.导出:
                    if (kryptonNavigator1.SelectedPage != null)
                    {
                        UCBillMasterQuery queryMaster = kryptonNavigator1.SelectedPage.Controls.Find(kryptonNavigator1.SelectedPage.UniqueName, true).FirstOrDefault() as UCBillMasterQuery;
                        UIExcelHelper.ExportExcel(queryMaster.newSumDataGridViewMaster);
                    }

                    break;
                default:
                    break;
            }
        }



        public virtual void Print(RptMode rptMode)
        {
            //List<M> selectlist = GetSelectResult();
            //if (_PrintConfig == null || _PrintConfig.tb_PrintTemplates == null)
            //{
            //    _PrintConfig = PrintHelper<M>.GetPrintConfig(selectlist);
            //}
            //bool rs = await PrintHelper<M>.Print(selectlist, rptMode, _PrintConfig);
            //if (rs && rptMode == RptMode.PRINT)
            //{
            //    toolStripSplitButtonPrint.Enabled = false;
            //}
        }


        //public virtual List<M> GetSelectResult()
        //{
        //    List<M> selectlist = new List<M>();
        //    if (_UCMasterQuery != null)
        //    {
        //        _UCMasterQuery.newSumDataGridViewMaster.EndEdit();
        //        if (true)
        //        {
        //            #region 批量处理
        //            if (_UCMasterQuery.newSumDataGridViewMaster.SelectedRows != null)
        //            {
        //                foreach (DataGridViewRow dr in _UCMasterQuery.newSumDataGridViewMaster.Rows)
        //                {
        //                    if (!(dr.DataBoundItem is M))
        //                    {
        //                        MessageBox.Show("TODO:请调试这里");
        //                    }
        //                    selectlist.Add((M)dr.DataBoundItem);
        //                    //if (_UCMasterQuery.newSumDataGridViewMaster.UseSelectedColumn && (bool)dr.Cells["Selected"].Value)
        //                    //{
        //                    //    selectlist.Add((M)dr.DataBoundItem);
        //                    //}
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region 单行处理
        //            if (_UCMasterQuery.newSumDataGridViewMaster.CurrentRow != null)
        //            {
        //                var dr = _UCMasterQuery.newSumDataGridViewMaster.CurrentRow;
        //                if (!(dr.DataBoundItem is M))
        //                {
        //                    MessageBox.Show("TODO:请调试这里");
        //                }
        //                selectlist.Add((M)dr.DataBoundItem);
        //            }
        //            #endregion
        //        }
        //    }

        //    return selectlist;
        //}

        protected virtual void AdvQuery()
        {

        }

        protected virtual void Submit()
        {

        }
        protected virtual void Print()
        {
            //https://www.cnblogs.com/westsoft/p/8594379.html  三联单
            //  RptPrintConfig config = new RptPrintConfig();
            // config.ShowDialog();
        }

        /// <summary>
        /// 暂时只支持一级审核，将来可以设计配置 可选多级审核。并且能看到每级的审核情况
        /// </summary>
        protected async virtual Task<ApprovalEntity> Review()
        {
            await Task.Delay(0);
            MessageBox.Show("应该有选项 同意和同意，原因？");
            return null;
        }





        private void workspaceCellAdding(object sender, WorkspaceCellEventArgs e)
        {
            e.Cell.Button.ButtonDisplayLogic = ButtonDisplayLogic.None;
            // Hide the default close and context buttons, they are not relevant for this demo
            e.Cell.Button.CloseButtonAction = CloseButtonAction.None;
            e.Cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            e.Cell.Button.ContextButtonDisplay = ButtonDisplay.Hide;
        }



        protected virtual void Exit(object thisform)
        {
            //保存配置
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.NewLineChars = Environment.NewLine;
            settings.Indent = true;
            using XmlWriter xmlWriter = XmlWriter.Create("Navigator" + typeof(Proc_SaleOutStatisticsByEmployee).Name + "Persistence.xml", settings);
            {
                xmlWriter.WriteStartDocument(true);
                //文档类型
                xmlWriter.WriteDocType("Html", null, null, "<!ENTITY h \"hardcover\">");

                xmlWriter.WriteStartElement("Html");
                //命名空间
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, @"http://.xsd");
                //指令
                String PItext = "type=\"text/xsl\" href=\"book.xsl\"";
                xmlWriter.WriteProcessingInstruction("xml-stylesheet", PItext);
                //注释
                xmlWriter.WriteComment("标题头");
                //cdata
                xmlWriter.WriteCData(@"<javasritpt><javasritpt>");
                //kryptonWorkspace1.SaveLayoutToXml(xmlWriter);
            }

            //退出
            CloseTheForm(thisform);

        }
        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = (Krypton.Workspace.KryptonWorkspaceCell)MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            KryptonPage page = (thisform as Control).Parent as KryptonPage;
            MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
            /*
            if (page == null)
            {
                //浮动
               
            }
            else
            {
                //活动内
                if (cell.Pages.Contains(page))
                {
                    cell.Pages.Remove(page);
                    page.Dispose();
                }
            }
            */
        }


        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<string> OrderInvisibleCols { get; set; } = new List<string>();


        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<string> OutInvisibleCols { get; set; } = new List<string>();


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


        // public KryptonPageCollection Kpages { get; set; } = new KryptonPageCollection();

        private void BaseNavigatorGeneric_Load(object sender, EventArgs e)
        {
            this.CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.ClassPath == this.ToString()).FirstOrDefault();
            if (this.CurMenuInfo == null)
            {
                MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                return;
            }
            //月销售，依客戶，依业务
            kryptonTreeViewMenu.Nodes.Clear();
            foreach (var item in navigator.NavigatorMenus)
            {
                TreeNode node = new TreeNode(item.MenuName);
                node.Name = item.MenuName;
                node.Tag = item;
                kryptonTreeViewMenu.Nodes.Add(node);
            }

            if (this.DesignMode)
            {
                return;
            }

            //设置列显示，这里实现了 员工编号转换为名字。里面用的是销售订单。是因为销售订单中引用了两个表，因为这里是存储过程。没有引用关联表
            BuildColDisplayTypes();


            #region 添加容器
            //默认加一个一。不然不显示，加载时清除，以后完善

            //创建面板并加入
            //Kpages = new KryptonPageCollection();


            #endregion
            //默认加载一次
            NavigatorMenu navigatorMenu = new NavigatorMenu();
            navigatorMenu = kryptonTreeViewMenu.Nodes[0].Tag as NavigatorMenu;
            //if (!navigatorMenu.UILoaded)
            //{
            KryptonPage kryptonPage = AddPage(navigatorMenu.menuType);
            kryptonNavigator1.SelectedPage = kryptonPage;
            //navigatorMenu.UILoaded = true;
            kryptonTreeViewMenu.SelectedNode = kryptonTreeViewMenu.Nodes[0];
            // }

            kryptonNavigator1.SelectedPageChanged += KryptonNavigator1_SelectedPageChanged;
        }

        private void KryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (kryptonNavigator1.SelectedPage == null)
            {
                return;
            }
            else
            {
                TreeNode treeNode = kryptonTreeViewMenu.Nodes.Find(kryptonNavigator1.SelectedPage.UniqueName, true).FirstOrDefault();
                if (treeNode != null)
                {
                    kryptonTreeViewMenu.SelectedNode = treeNode;
                    kryptonTreeViewMenu.Select();
                }
            }

        }

        /// <summary>
        /// 构建导航菜单
        /// </summary>
        public Navigator BuildNavigatorMenu()
        {
            Navigator navigator = new Navigator();
            NavigatorMenu navigator1 = new NavigatorMenu();
            navigator1.MenuLevel = 0;
            navigator1.MenuSort = 0;
            navigator1.menuType = NavigatorMenuType.销售订单数据汇总;
            navigator1.MenuName = navigator1.menuType.ToString();
            navigator.NavigatorMenus.Add(navigator1);

            NavigatorMenu navigator2 = new NavigatorMenu();
            navigator2.MenuLevel = 0;
            navigator2.MenuSort = 1;
            navigator2.menuType = NavigatorMenuType.销售出库业绩汇总;
            navigator2.MenuName = navigator2.menuType.ToString();

            navigator.NavigatorMenus.Add(navigator2);
            return navigator;
        }


        private KryptonPage AddPage(NavigatorMenuType menuType)
        {

            KryptonPage page = kryptonNavigator1.Pages.Where(c => c.UniqueName == menuType.ToString()).FirstOrDefault();
            if (page == null)
            {
                page = new KryptonPage();
            }
            else
            {
                return page;
            }
            page.UniqueName = menuType.ToString();
            // Set the page title
            page.Text = menuType.ToString();
            // Remove the default image for the page
            page.ImageSmall = null;
            // Set the padding so contained controls are indented
            //page.Padding = new Padding(0);
            // Get the colors to use for this new page
            Color normal = _normal[kryptonNavigator1.Pages.Count % _normal.Length];
            Color select = _select[kryptonNavigator1.Pages.Count % _select.Length];
            // Set the page colors
            page.StateNormal.Page.Color1 = select;
            page.StateNormal.Page.Color2 = normal;
            page.StateNormal.Tab.Back.Color2 = normal;
            page.StateSelected.Tab.Back.Color2 = select;
            page.StateTracking.Tab.Back.Color2 = _hotMain;
            page.StatePressed.Tab.Back.Color2 = _hotMain;

            // We want the page drawn as a gradient with colors relative to its own area
            page.StateCommon.Page.ColorAlign = PaletteRectangleAlign.Local;
            page.StateCommon.Page.ColorStyle = PaletteColorStyle.Sigma;
            Type entityType = null;
            UCQueryShow queryShow = new UCQueryShow();
            queryShow.Dock = DockStyle.Fill;
            BaseProcessor baseProcessor = null;

            UCBillMasterQuery _UCMasterQuery = new UCBillMasterQuery();

            switch (menuType)
            {
                case NavigatorMenuType.销售明细分析:
                    break;
                case NavigatorMenuType.销售订单数据汇总:
                    entityType = typeof(Proc_SaleOrderStatisticsByEmployee);
                    baseProcessor = Startup.GetFromFacByName<BaseProcessor>(nameof(Proc_SaleOrderStatisticsByEmployee) + "Processor");
                    queryShow.Name = nameof(NavigatorMenuType.销售订单数据汇总) + "QueryShow";
                    LoadQueryConditionToUISaleOrder(queryShow.kryptonPanelQuery, baseProcessor.GetQueryFilter());
                    //分组条件
                    _UCMasterQuery.InvisibleCols = OrderInvisibleCols;
                    break;
                case NavigatorMenuType.销售出库业绩汇总:
                    entityType = typeof(Proc_SaleOutStatisticsByEmployee);
                    queryShow.Name = nameof(NavigatorMenuType.销售出库业绩汇总) + "QueryShow";
                    baseProcessor = Startup.GetFromFacByName<BaseProcessor>(nameof(Proc_SaleOutStatisticsByEmployee) + "Processor");
                    LoadQueryConditionToUISaleOut(queryShow.kryptonPanelQuery, baseProcessor.GetQueryFilter());
                    //分组条件
                    _UCMasterQuery.InvisibleCols = OutInvisibleCols;
                    break;
                default:
                    break;
            }

            _UCMasterQuery.entityType = entityType;
            _UCMasterQuery.Name = menuType.ToString();

            _UCMasterQuery.SummaryCols = baseProcessor.GetSummaryCols();
            _UCMasterQuery.ColNameDataDictionary = MasterColNameDataDictionary;
            _UCMasterQuery.newSumDataGridViewMaster.UseCustomColumnDisplay = true;
            _UCMasterQuery.ShowSummaryCols();
            _UCMasterQuery.ColDisplayTypes = MasterColDisplayTypes;
            _UCMasterQuery.newSumDataGridViewMaster.Dock = DockStyle.Fill;
            _UCMasterQuery.Dock = DockStyle.Fill;
            queryShow.kryptonPanelDetailGrid.Controls.Add(_UCMasterQuery);

            //MasterQuery(typeof(Proc_SaleOrderStatisticsByEmployee));
            //Kpages.Add(UCOutlookGridAnalysis1Load(typeof(Proc_SaleOrderStatisticsByEmployee)));
            page.Controls.Add(queryShow);
            //            Kpages.Add(UCOutlookGridAnalysis1Load(typeof(Proc_SaleOrderStatisticsByEmployee)));

            // Add page to end of the navigator collection
            kryptonNavigator1.Pages.Add(page);
            return page;
        }

        private KryptonWorkspaceCell CreateCell(KryptonPage[] kryptonPages)
        {
            KryptonWorkspaceCell cell = new KryptonWorkspaceCell();
            UITools uITools = new UITools();
            cell.Pages.AddRange(kryptonPages);
            return cell;
        }

        private KryptonWorkspaceCell CreateCell(KryptonPage kryptonPage)
        {
            KryptonWorkspaceCell cell = new KryptonWorkspaceCell();
            UITools uITools = new UITools();
            cell.Pages.Add(kryptonPage);
            return cell;
        }

        private KryptonWorkspaceCell CreateCell(string title, Control control)
        {
            KryptonWorkspaceCell cell = new KryptonWorkspaceCell();
            UITools uITools = new UITools();
            cell.Pages.Add(uITools.NewPage(title, control));
            return cell;
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
            p.Width = 150;
            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            // _count++;
            return p;
        }


        private BaseEntity _queryDto = new BaseEntity();

        public BaseEntity UIparaOut { get => _queryDto; set => _queryDto = value; }


        private BaseEntity _queryDtoOrder = new BaseEntity();

        public BaseEntity UIparaOrder { get => _queryDtoOrder; set => _queryDtoOrder = value; }


        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public BaseEntity LoadQueryConditionToUISaleOut(Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器, QueryFilter queryFilter)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            UIparaOut = new Proc_SaleOutStatisticsByEmployeePara();
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();

            UIparaOut = UIGenerateHelper.CreateQueryUI(typeof(Proc_SaleOutStatisticsByEmployeePara), true, kryptonPanel条件生成容器, queryFilter, 4);
            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            return UIparaOut;
        }


        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public BaseEntity LoadQueryConditionToUISaleOrder(Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器, QueryFilter queryFilter)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            UIparaOrder = new Proc_SaleOrderStatisticsByEmployeePara();
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            UIparaOrder = UIGenerateHelper.CreateQueryUI(typeof(Proc_SaleOrderStatisticsByEmployeePara), true, kryptonPanel条件生成容器, queryFilter, 4);

            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            return UIparaOrder;
        }



        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ChildColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();



        /*

        #region 查询条件

        private UCQueryCondition _UCQueryCondition;
        private KryptonPage QueryCondition()
        {
            _UCQueryCondition = new UCQueryCondition();
            _UCQueryCondition.entityType = typeof(M);
            LoadQueryConditionToUI(_UCQueryCondition.kryptonPanelQuery);

            KryptonPage page = NewPage(NavParts.查询条件.ToString(), 1, _UCQueryCondition);
            // Document pages cannot be docked or auto hidden
            //page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            page.AllowDrop = true;
            page.Width = 150;
            page.ClearFlags(KryptonPageFlags.All);

            return page;
        }


        #endregion
        */

        #region 查询结果-主表






        #endregion

        /*
        #region 子表


        /// <summary>
        /// 明细表要统计的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildSummaryCols { get; set; } = new List<Expression<Func<C, object>>>();


        /// <summary>
        /// 明细表要统计的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildRelatedSummaryCols { get; set; } = new List<Expression<Func<C, object>>>();




        /// <summary>
        /// 明细表不可见的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildInvisibleCols { get; set; } = new List<Expression<Func<C, object>>>();


        private UCBillChildQuery _UCBillChildQuery;
        private KryptonPage ChildQuery()
        {
            _UCBillChildQuery = new UCBillChildQuery();
            _UCBillChildQuery.Name = "_UCBillChildQuery";
            _UCBillChildQuery.entityType = typeof(C);
            List<string> childlist = ExpressionHelper.ExpressionListToStringList(ChildSummaryCols);
            _UCBillChildQuery.InvisibleCols = ExpressionHelper.ExpressionListToStringList(ChildInvisibleCols);
            _UCBillChildQuery.SummaryCols = childlist;
            //_UCBillChildQuery.ColDisplayTypes=
            _UCBillChildQuery.ColNameDataDictionary = ChildColNameDataDictionary;
            KryptonPage page = NewPage(NavParts.明细.ToString(), 1, _UCBillChildQuery);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            return page;
        }


        #endregion

        #region 关联

        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ChildRelatedColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();
        /// <summary>
        /// 明细表不可见的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildRelatedInvisibleCols { get; set; } = new List<Expression<Func<C, object>>>();

        private UCBillChildQuery _UCBillChildQuery_Related;

        /// <summary>
        /// 比方采购入库，对应采购入库明细，
        /// 相关联的是 采购订单的明细，主单先不管
        /// 如果为空，则认为没有关联引用数据
        /// 第三方的子表类型
        /// </summary>
        public Type ChildRelatedEntityType;

        private KryptonPage Child_RelatedQuery()
        {
            _UCBillChildQuery_Related = new UCBillChildQuery();
            _UCBillChildQuery_Related.Name = "_UCBillChildQuery_Related";
            _UCBillChildQuery_Related.entityType = ChildRelatedEntityType;
            List<string> childlist = ExpressionHelper.ExpressionListToStringList(ChildRelatedSummaryCols);
            _UCBillChildQuery_Related.InvisibleCols = ExpressionHelper.ExpressionListToStringList(ChildRelatedInvisibleCols);
            _UCBillChildQuery_Related.SummaryCols = childlist;
            _UCBillChildQuery_Related.ColNameDataDictionary = ChildColNameDataDictionary;
            KryptonPage page = NewPage(NavParts.关联单据.ToString(), 1, _UCBillChildQuery_Related);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            return page;
        }
        #endregion

        #region 分组结果

        /// <summary>
        /// 分析结果中要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> OutlookGridGroupAnalysisSubtotalColumns { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillOutlookGridAnalysis _UCOutlookGridGroupAnalysis;


        /// <summary>
        /// 加载分析数据 部分 数据 不能太多。不然性能影响体验
        /// </summary>
        /// <returns></returns>

        private KryptonPage UCOutlookGridGroupAnalysisLoad()
        {
            _UCOutlookGridGroupAnalysis = new UCBillOutlookGridAnalysis();
            _UCOutlookGridGroupAnalysis.entityType = typeof(M);
            _UCOutlookGridGroupAnalysis.ColDisplayTypes = _UCMasterQuery.ColDisplayTypes;
            List<string> masterlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            _UCOutlookGridGroupAnalysis.SummaryCols = masterlist;
            _UCOutlookGridGroupAnalysis.InvisibleCols = ExpressionHelper.ExpressionListToStringList(MasterInvisibleCols);
            _UCOutlookGridGroupAnalysis.ColNameDataDictionary = MasterColNameDataDictionary;

            KryptonPage page = NewPage(NavParts.分组显示.ToString(), 1, _UCOutlookGridGroupAnalysis);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }

        #endregion
        */

        /*
        #region  分析2

        /// <summary>
        /// 分析结果中要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> OutlookGridAnalysis2SubtotalColumns { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillOutlookGridAnalysis _UCOutlookGridAnalysis2;


        /// <summary>
        /// 加载分析数据 部分 数据 不能太多。不然性能影响体验
        /// </summary>
        /// <returns></returns>

        private KryptonPage UCOutlookGridAnalysis2Load()
        {
            _UCOutlookGridAnalysis2 = new UCBillOutlookGridAnalysis();
            // _UCOutlookGridAnalysis2.entityType = typeof(M);
            //   _UCOutlookGridAnalysis2.ColDisplayTypes = _UCMasterQuery.ColDisplayTypes;
            //List<string> masterlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            // _UCOutlookGridAnalysis2.SummaryCols = masterlist;
            //_UCOutlookGridAnalysis2.InvisibleCols = ExpressionHelper.ExpressionListToStringList(MasterInvisibleCols);
            // _UCOutlookGridAnalysis2.ColNameDataDictionary = MasterColNameDataDictionary;
            KryptonPage page = NewPage(NavParts.结果分析2.ToString(), 1, _UCOutlookGridAnalysis2);
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }







        #endregion
        */
        private void kryptonTreeViewMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (kryptonTreeViewMenu.SelectedNode != null)
            {
                NavigatorMenu navigatorMenu = new NavigatorMenu();
                navigatorMenu = kryptonTreeViewMenu.SelectedNode.Tag as NavigatorMenu;
                //if (!navigatorMenu.UILoaded)
                //{
                KryptonPage kryptonPage = AddPage(navigatorMenu.menuType);
                kryptonNavigator1.SelectedPage = kryptonPage;
                //LoadUIByMenu(navigatorMenu.menuType);
                //    navigatorMenu.UILoaded = true;
                //}
                //else
                //{
                //    //激活
                //}
            }
        }
    }
}
