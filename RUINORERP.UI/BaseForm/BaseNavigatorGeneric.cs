using AutoMapper;
using ExCSS;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Models;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.FormProperty;
using RUINORERP.UI.Report;
using RUINORERP.UI.UControls;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CommonHelper = RUINORERP.UI.Common.CommonHelper;


namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 处理查询分析等各种情况
    /// 单表带条件查询 并且可以带分析outlook
    /// OK
    /// </summary>
    /// <typeparam name="M">Master表类型</typeparam>
    /// <typeparam name="C">Child表类型-单表时可以传主表</typeparam>
    public partial class BaseNavigatorGeneric<M, C> : BaseNavigator where M : class
    {
        ///// <summary>
        ///// 从工作台点击过来的时候，这个保存初始化时的查询参数
        /////  这个可用可不用。
        ///// </summary>
        //public object QueryDtoProxy { get; set; }
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
        /// 查询分析内部导航部件
        /// </summary>
        public enum NavParts
        {
            查询结果,
            明细,
            关联单据,
            分组显示,
            结果分析1,
            结果分析2,
        }

        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> MasterColDisplayTypes { get; set; } = new List<Type>();

        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> ChildColDisplayTypes { get; set; } = new List<Type>();



        /// <summary>
        /// 设置列显示相关的类型
        /// </summary>
        public virtual void BuildColDisplayTypes()
        {

        }

        public List<NavParts[]> strings = new List<NavParts[]>();
        public virtual List<NavParts[]> AddNavParts()
        {
            List<NavParts[]> strings = new List<NavParts[]>();
            strings.Add(new NavParts[] { NavParts.查询结果, NavParts.分组显示 });
            return strings;
        }

        /// <summary>
        /// 查询结果右键实现及指向
        /// </summary>
        public virtual void SetContextMenuStrip()
        {

        }


        public QueryFilter QueryFilter { get; set; } = new QueryFilter();

        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        [MustOverride]
        protected async virtual void Query()
        {
            using (StatusBusy busy = new StatusBusy("系统正在查询 请稍候"))
            {
                #region
                //if (QueryHandler != null)
                //{
                //    OnQuery()
                //}
                if (ValidationHelper.hasValidationErrors(this.Controls))
                    return;
                BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");

                //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要

                // List<string> queryConditions = new List<string>();

                List<M> list = new List<M>();

                //提取指定的列名，即条件集合
                // queryConditions = new List<string>(QueryFilter.QueryFields.Select(t => t.FieldName).ToList());
                int PageNum = 1;
                int PageSize = int.Parse(txtMaxRow.Text);
                if (QueryFilter.FilterLimitExpressions == null)
                {
                    QueryFilter.FilterLimitExpressions = new List<LambdaExpression>();
                }
                QueryFilter.FilterLimitExpressions.Add(LimitQueryConditions);
                //list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, LimitQueryConditions, QueryDto, PageNum, PageSize) as List<M>;
                list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryFilter, QueryDto, PageNum, PageSize) as List<M>;


                _UCMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();
                _UCMasterQuery.ShowSummaryCols();
                if (_UCOutlookGridGroupAnalysis != null)
                {
                    _UCOutlookGridGroupAnalysis.FieldNameList = _UCMasterQuery.newSumDataGridViewMaster.FieldNameList;
                    _UCOutlookGridGroupAnalysis.bindingSourceOutlook.DataSource = list;
                    //控制列的显示
                    _UCOutlookGridGroupAnalysis.ColumnDisplays = _UCMasterQuery.newSumDataGridViewMaster.ColumnDisplays;
                    _UCOutlookGridGroupAnalysis.OnLoadData += _UCBillOutlookGridAnalysis_OnLoadData;
                    _UCOutlookGridGroupAnalysis.LoadDataToGrid<M>(list);

                }

                //这里可以重构掉 用属性事件驱动

                /*
                     customersDataGridView.Columns["CustomerID"].Visible = false;
                    customersDataGridView.Columns["ContactName"].DisplayIndex = 0;
    customersDataGridView.Columns["ContactTitle"].DisplayIndex = 1;
    customersDataGridView.Columns["City"].DisplayIndex = 2;
    customersDataGridView.Columns["Country"].DisplayIndex = 3;
    customersDataGridView.Columns["CompanyName"].DisplayIndex = 4;
                 */


                #endregion

            }




        }


        public bool WithOutlook { get; set; }
        /// <summary>
        /// 相关外键表或实体查询实体。如果是视图传入T时起作用
        /// </summary>
        public Type ReladtedEntityType
        {
            get; set;
        }


        public BaseNavigatorGeneric()
        {
            if (this.DesignMode)
            {
                return;
            }
            InitializeComponent();
            frm = new frmFormProperty();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {



                    Krypton.Toolkit.KryptonButton button设置查询条件 = new Krypton.Toolkit.KryptonButton();
                    button设置查询条件.Text = "设置查询条件";
                    button设置查询条件.ToolTipValues.Description = "对查询条件进行个性化设置。";
                    button设置查询条件.ToolTipValues.EnableToolTips = true;
                    button设置查询条件.ToolTipValues.Heading = "提示";
                    button设置查询条件.Click += button设置查询条件_Click;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button设置查询条件);


                    Krypton.Toolkit.KryptonButton button表格显示设置 = new Krypton.Toolkit.KryptonButton();
                    button表格显示设置.Text = "表格显示设置";
                    button表格显示设置.ToolTipValues.Description = "对表格显示设置进行个性化设置。";
                    button表格显示设置.ToolTipValues.EnableToolTips = true;
                    button表格显示设置.ToolTipValues.Heading = "提示";
                    button表格显示设置.Click += button表格显示设置_Click;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button表格显示设置);

                }
            }


        }

        private async void button表格显示设置_Click(object sender, EventArgs e)
        {
            await UIBizService.SetGridViewAsync(typeof(M), _UCMasterQuery.newSumDataGridViewMaster, CurMenuInfo, true);
        }

        private async void button设置查询条件_Click(object sender, EventArgs e)
        {
            bool rs = await UIBizService.SetQueryConditionsAsync(CurMenuInfo, QueryFilter, QueryDto);
            if (rs)
            {
                LoadQueryConditionToUI();
            }
        }


        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// 这里是用于分析。查询来的结果和要分析的是一样的数据。所有可以共用

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

        /// <summary>
        /// 这里提借枚举型的共用的名称值显示字典
        /// </summary>
        public virtual void BuildColNameDataDictionary()
        {
            MasterColNameDataDictionary.TryAdd(nameof(DataStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));
            MasterColNameDataDictionary.TryAdd(nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));
            MasterColNameDataDictionary.TryAdd(nameof(PayStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus)));
            MasterColNameDataDictionary.TryAdd(nameof(Priority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority)));
            MasterColNameDataDictionary.TryAdd(nameof(PurReProcessWay), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay)));


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
                    Property();
                    break;
                case MenuItemEnums.审核:
                    await Review();
                    break;
                case MenuItemEnums.反审:
                    ReReview();
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
                    UIExcelHelper.ExportExcel(_UCMasterQuery.newSumDataGridViewMaster);
                    break;
                default:
                    break;
            }
        }

        //https://www.cnblogs.com/westsoft/p/8594379.html  三联单
        public virtual async Task Print(RptMode rptMode)
        {
            List<M> selectlist = GetSelectResult();
            if (_PrintConfig == null || _PrintConfig.tb_PrintTemplates == null)
            {
                _PrintConfig = PrintHelper<M>.GetPrintConfig(selectlist);
            }
            bool rs = await PrintHelper<M>.Print(selectlist, rptMode, _PrintConfig);
            if (rs && rptMode == RptMode.PRINT)
            {
                toolStripSplitButtonPrint.Enabled = false;
            }
        }


        public virtual List<M> GetSelectResult()
        {
            List<M> selectlist = new List<M>();
            if (_UCMasterQuery != null)
            {
                _UCMasterQuery.newSumDataGridViewMaster.EndEdit();
                if (true)
                {
                    #region 批量处理
                    if (_UCMasterQuery.newSumDataGridViewMaster.SelectedRows != null)
                    {
                        foreach (DataGridViewRow dr in _UCMasterQuery.newSumDataGridViewMaster.Rows)
                        {
                            if (!(dr.DataBoundItem is M))
                            {
                                MessageBox.Show("TODO:请调试这里");
                            }
                            selectlist.Add((M)dr.DataBoundItem);
                            //if (_UCMasterQuery.newSumDataGridViewMaster.UseSelectedColumn && (bool)dr.Cells["Selected"].Value)
                            //{
                            //    selectlist.Add((M)dr.DataBoundItem);
                            //}
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 单行处理
                    if (_UCMasterQuery.newSumDataGridViewMaster.CurrentRow != null)
                    {
                        var dr = _UCMasterQuery.newSumDataGridViewMaster.CurrentRow;
                        if (!(dr.DataBoundItem is M))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        selectlist.Add((M)dr.DataBoundItem);
                    }
                    #endregion
                }
            }

            return selectlist;
        }



        protected virtual void Submit()
        {

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

        protected virtual void ReReview()
        {

        }

        protected frmFormProperty frm = null;
        protected virtual void Property()
        {
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
                // ToolBarEnabledControl(MenuItemEnums.属性);
                //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("属性", EditEntity);
            }
        }

        public virtual void BuildLimitQueryConditions()
        {

        }

        private void workspaceCellAdding(object sender, WorkspaceCellEventArgs e)
        {
            e.Cell.Button.ButtonDisplayLogic = ButtonDisplayLogic.None;
            // Hide the default close and context buttons, they are not relevant for this demo
            e.Cell.Button.CloseButtonAction = CloseButtonAction.None;
            e.Cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            e.Cell.Button.ContextButtonDisplay = ButtonDisplay.Hide;


        }

        /// <summary>
        /// whereLambda
        /// Expression<Func<User, bool>> condition1 = t => t.Name.Contains("张");
        //// Expression<Func<User, bool>> condition2 = t => t.Age > 18;
        // Expression<Func<User, bool>> condition3 = t => t.Gender == "男";
        // Expression<Func<User, bool>> condition4 = t => t.Money > 1000;
        //var lambda = condition1.And(condition2).And(condition3).Or(condition4);
        // var users = UserDbContext.Query(lambda);
        /// </summary>
        public Expression<Func<M, bool>> LimitQueryConditions { get; set; }


        private void _UCBillOutlookGridAnalysis_OnLoadData(object rowObj)
        {

        }

        public void Builder()
        {
            BuildInvisibleCols();
            BuildLimitQueryConditions();
            BuildColNameDataDictionary();
            BuildQueryCondition();
            BuildSummaryCols();
            BuildColDisplayTypes();
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public virtual void BuildQueryCondition()
        {
            //添加默认全局的
            // base.QueryConditions.Add(c => c.Created_by);
            // List<string> slist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
        }

        /// <summary>
        /// 隱藏的列
        /// </summary>
        public virtual void BuildInvisibleCols()
        {

        }
        public virtual void BuildSummaryCols()
        {
            ////添加默认全局的
            //// base.QueryConditions.Add(c => c.Created_by);

            //List<string> mlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            //_UCBillMasterQuery.SummaryCols = mlist;

            //List<string> clist = ExpressionHelper.ExpressionListToStringList(ChildSummaryCols);
            //_UCBillChildQuery.SummaryCols = clist;
        }

        public static string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.LayoutConfigDirectory);
        string xmlfilepath = System.IO.Path.Combine(basePath, "Navigator" + typeof(M).Name + "Persistence.xml");

        protected virtual async Task Exit(object thisform)
        {
            if (_UCMasterQuery != null && _UCMasterQuery.newSumDataGridViewMaster != null)
            {
                UIBizService.SaveGridSettingData(CurMenuInfo, _UCMasterQuery.newSumDataGridViewMaster, typeof(M));
            }

            //保存配置
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.NewLineChars = Environment.NewLine;
            settings.Indent = true;

            using XmlWriter xmlWriter = XmlWriter.Create(xmlfilepath, settings);
            {
                kryptonWorkspace1.SaveLayoutToXml(xmlWriter);
                xmlWriter.Close();//要关闭，否则下面再用时会报错。
            }


            //=============!!
            //保存超级用户的布局为默认布局
            if (MainForm.Instance.AppContext.IsSuperUser && System.IO.File.Exists(xmlfilepath))
            {
                //加载XML文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlfilepath);
                //获取XML字符串
                string xmlStr = xmldoc.InnerXml;
                //字符串转XML
                //xmldoc.LoadXml(xmlStr);
                CurMenuInfo.DefaultLayout = xmlStr;
                await MainForm.Instance.AppContext.Db.Storageable<tb_MenuInfo>(CurMenuInfo).ExecuteReturnEntityAsync();
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
            if (page != null)
            {
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }

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

        private DragManager _dm;

        public KryptonPageCollection Kpages { get; set; } = new KryptonPageCollection();

        private async void BaseNavigatorGeneric_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            else
            {
                #region 菜单权限控制
                //权限菜单
                if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                {
                    CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath == this.ToString()).FirstOrDefault();
                    if (CurMenuInfo == null && !MainForm.Instance.AppContext.IsSuperUser)
                    {
                        MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                        return;
                    }
                }

                foreach (var item in base.BaseToolStrip.Items)
                {
                    if (item is ToolStripButton)
                    {
                        ToolStripButton subItem = item as ToolStripButton;
                        subItem.Click += Item_Click;
                        UIHelper.ControlButton<ToolStripButton>(this.CurMenuInfo, subItem);
                    }
                    if (item is ToolStripDropDownButton)
                    {
                        ToolStripDropDownButton subItem = item as ToolStripDropDownButton;
                        subItem.Click += Item_Click;
                        UIHelper.ControlButton<ToolStripDropDownButton>(this.CurMenuInfo, subItem);
                        //下一级
                        if (subItem.HasDropDownItems)
                        {
                            foreach (var sub in subItem.DropDownItems)
                            {
                                ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                subStripMenuItem.Click += Item_Click;
                                UIHelper.ControlButton<ToolStripMenuItem>(this.CurMenuInfo, subStripMenuItem);
                            }
                        }
                    }
                    //打印特殊处理
                    if (item is ToolStripSplitButton)
                    {
                        ToolStripSplitButton subItem = item as ToolStripSplitButton;
                        subItem.Click += Item_Click;
                        UIHelper.ControlButton<ToolStripSplitButton>(this.CurMenuInfo, subItem);
                        //下一级
                        if (subItem.HasDropDownItems)
                        {
                            foreach (var sub in subItem.DropDownItems)
                            {
                                ToolStripItem subStripMenuItem = sub as ToolStripItem;
                                subStripMenuItem.Click += Item_Click;
                                UIHelper.ControlButton<ToolStripItem>(this.CurMenuInfo, subStripMenuItem);
                            }
                        }
                    }

                }

                #endregion
            }

            _dm = new DragManager();
            _dm.StateCommon.Feedback = PaletteDragFeedback.Rounded;
            _dm.DragTargetProviders.Add(kryptonWorkspace1);

            kryptonWorkspace1.DragPageNotify = _dm;


            kryptonWorkspace1.AllowDrop = true;
            kryptonWorkspace1.AllowPageDrag = true;
            kryptonWorkspace1.Root.Children.Clear();

            //添加查询条件
            Builder();


            this.CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath == this.ToString()).FirstOrDefault();
            if (CurMenuInfo == null && !MainForm.Instance.AppContext.IsSuperUser)
            {
                MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                return;
            }

            LoadQueryConditionToUI(4);



            //默认加一个一。不然不显示，加载时清除，以后完善

            //创建面板并加入
            Kpages = new KryptonPageCollection();
            strings = AddNavParts();
            if (Kpages.Count == 0)
            {
                foreach (NavParts[] navs in strings)
                {
                    for (int i = 0; i < navs.Length; i++)
                    {
                        switch (navs[i])
                        {
                            //case NavParts.查询条件:
                            //    Kpages.Add(QueryCondition());
                            //    break;
                            case NavParts.查询结果:
                                Kpages.Add(MasterQuery());
                                break;
                            case NavParts.明细:
                                Kpages.Add(ChildQuery());
                                break;
                            case NavParts.关联单据:
                                Kpages.Add(Child_RelatedQuery());
                                break;
                            case NavParts.分组显示:
                                Kpages.Add(UCOutlookGridGroupAnalysisLoad());
                                break;
                            case NavParts.结果分析1:
                                Kpages.Add(UCOutlookGridAnalysis1Load());
                                break;
                            case NavParts.结果分析2:
                                Kpages.Add(UCOutlookGridAnalysis2Load());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            try
            {
                //Location of XML file
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                if (System.IO.File.Exists(xmlfilepath) && AuthorizeController.GetQueryPageLayoutCustomize(MainForm.Instance.AppContext))
                {
                    // Create the XmlNodeReader object.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlfilepath);
                    XmlNodeReader nodeReader = new XmlNodeReader(doc);
                    // Set the validation settings.
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Parse;
                    //settings.ValidationType = ValidationType.Schema;
                    //settings.Schemas.Add("urn:bookstore-schema", "books.xsd");
                    //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                    //settings.NewLineChars = Environment.NewLine;
                    //settings.Indent = true;

                    using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "KW")
                            {
                                //加载停靠信息
                                kryptonWorkspace1.LoadLayoutFromXml(reader, Kpages);
                            }
                        }

                    }
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
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "KW")
                                {
                                    //加载停靠信息
                                    kryptonWorkspace1.LoadLayoutFromXml(reader, Kpages);
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

            //如果加载过的停靠信息中不正常。就手动初始化 暂时认为没有关闭单个情况，隐藏了close
            if (kryptonWorkspace1.AllPages().Count() < Kpages.Count)
            {
                foreach (NavParts[] navs in strings)
                {
                    List<KryptonPage> kryptonPages = new List<KryptonPage>();
                    foreach (NavParts item in navs)
                    {
                        KryptonPage page = Kpages.Where(c => c.UniqueName == item.ToString()).FirstOrDefault();
                        kryptonPages.Add(page);
                    }
                    kryptonWorkspace1.Root.Children.AddRange(CreateCell(kryptonPages.ToArray()));
                }
            }

            #region 请求缓存
            //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
            await UIBizService.RequestCache<M>();
            await UIBizService.RequestCache<C>();
            #endregion

            List<M> list = new List<M>();
            _UCMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            _UCMasterQuery.ShowSummaryCols();

            BaseMainDataGridView = _UCMasterQuery.newSumDataGridViewMaster;
            _UCMasterQuery.newSumDataGridViewMaster.NeedSaveColumnsXml = false;
            await UIBizService.SetGridViewAsync(typeof(M), BaseMainDataGridView, CurMenuInfo, false, _UCMasterQuery.InvisibleCols, _UCMasterQuery.DefaultHideCols);

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
            cell.Pages.Add(UIForKryptonHelper.NewPage(title, control));
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

        public BaseEntity QueryDto { get => _queryDto; set => _queryDto = value; }

        /// <summary>
        /// 默认不是模糊查询
        /// 这个实际可以优化到基类统一起来。只要是有查询条件生成的情况下的都可以用
        /// </summary>
        /// <param name="useLike"></param>
        public void LoadQueryConditionToUI(decimal QueryConditionShowColQty = 5)
        {
            Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器 = kryptonPanelQuery;
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            // UIQueryHelper<M> uIQueryHelper = new UIQueryHelper<M>();
            // uIQueryHelper.ReladtedEntityType = ReladtedEntityType;
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();

            tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
            if (menuSetting != null)
            {
                QueryDto = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryFilter, menuSetting);
            }
            else
            {
                QueryDto = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryFilter, QueryConditionShowColQty);
            }


            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;

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


        /// <summary>
        /// 主表要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> MasterSummaryCols { get; set; } = new List<Expression<Func<M, object>>>();




        /// <summary>
        /// 主表表不可见的列
        /// </summary>
        public List<Expression<Func<M, object>>> MasterInvisibleCols { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillMasterQuery _UCMasterQuery;

        private KryptonPage MasterQuery()
        {
            _UCMasterQuery = new UCBillMasterQuery();
            _UCMasterQuery.entityType = typeof(M);
            _UCMasterQuery.Name = "BaseNavGen_UCMasterQuery";
            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            _UCMasterQuery.SummaryCols = masterlist;
            _UCMasterQuery.InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(MasterInvisibleCols);

            //一般主单的主键不用显示 这里统一处理？
            string PKColName = BaseUIHelper.GetEntityPrimaryKey<M>();
            if (!_UCMasterQuery.InvisibleCols.Contains(PKColName))
            {
                _UCMasterQuery.InvisibleCols.Add(PKColName);
            }
            _UCMasterQuery.DefaultHideCols = new HashSet<string>();

            UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCMasterQuery.InvisibleCols, _UCMasterQuery.DefaultHideCols);


            _UCMasterQuery.ColNameDataDictionary = MasterColNameDataDictionary;
            _UCMasterQuery.ColDisplayTypes = MasterColDisplayTypes;
            KryptonPage page = NewPage(NavParts.查询结果.ToString(), 1, _UCMasterQuery);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }


        #endregion

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
            List<string> childlist = RuinorExpressionHelper.ExpressionListToStringList(ChildSummaryCols);
            _UCBillChildQuery.InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(ChildInvisibleCols);
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
            List<string> childlist = RuinorExpressionHelper.ExpressionListToStringList(ChildRelatedSummaryCols);
            _UCBillChildQuery_Related.InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(ChildRelatedInvisibleCols);
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

        private async Task<KryptonPage> UCOutlookGridGroupAnalysisLoad()
        {
            //先加载一遍缓存
            await UIBizService.RequestCache<M>();
            await UIBizService.RequestCache<C>();
            //去检测产品视图的缓存并且转换为强类型
            await UIBizService.RequestCache(typeof(View_ProdDetail));

            _UCOutlookGridGroupAnalysis = new UCBillOutlookGridAnalysis();
            _UCOutlookGridGroupAnalysis.entityType = typeof(M);
            _UCOutlookGridGroupAnalysis.ColDisplayTypes = _UCMasterQuery.ColDisplayTypes;


            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            _UCOutlookGridGroupAnalysis.SummaryCols = masterlist;
            _UCOutlookGridGroupAnalysis.InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(MasterInvisibleCols);
            _UCOutlookGridGroupAnalysis.ColNameDataDictionary = MasterColNameDataDictionary;
            UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCOutlookGridGroupAnalysis.InvisibleCols);
            KryptonPage page = NewPage(NavParts.分组显示.ToString(), 1, _UCOutlookGridGroupAnalysis);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }

        #endregion

        #region 分析1

        /// <summary>
        /// 分析结果中要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> OutlookGridAnalysis1SubtotalColumns { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillOutlookGridAnalysis _UCOutlookGridAnalysis1;


        /// <summary>
        /// 加载分析数据 部分 数据 不能太多。不然性能影响体验
        /// </summary>
        /// <returns></returns>

        private KryptonPage UCOutlookGridAnalysis1Load()
        {
            _UCOutlookGridAnalysis1 = new UCBillOutlookGridAnalysis();
            //_UCOutlookGridAnalysis1.entityType = typeof(M);
            //_UCOutlookGridAnalysis1.ColDisplayTypes = _UCMasterQuery.ColDisplayTypes;
            //List<string> masterlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            //_UCOutlookGridAnalysis1.SummaryCols = masterlist;
            // _UCOutlookGridAnalysis1.InvisibleCols = ExpressionHelper.ExpressionListToStringList(MasterInvisibleCols);
            // _UCOutlookGridAnalysis1.ColNameDataDictionary = MasterColNameDataDictionary;


            KryptonPage page = NewPage(NavParts.结果分析1.ToString(), 1, _UCOutlookGridAnalysis1);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }


        #endregion

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





    }
}
