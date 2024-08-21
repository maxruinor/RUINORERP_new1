using AutoMapper;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
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
using System.Windows.Forms;
using System.Xml;

namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 单表带条件查询 并且可以带分析outlook
    /// 作废
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public partial class BaseMasterQueryWithCondition<M> : UserControl where M : class
    {

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



        public BaseMasterQueryWithCondition()
        {
            InitializeComponent();
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


            }

            //统计明细类数量改为100
            txtMaxRow.Text = "100";
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
                    Print();
                    break;
                case MenuItemEnums.预览:

                    break;
                case MenuItemEnums.导出:
                    break;
                default:
                    break;
            }


        }


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

        protected virtual void ReReview()
        {

        }
        protected virtual void Property()
        {
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = "SOB.frx";
            //frm.ShowDialog();
        }

        public virtual void BuildLimitQueryConditions()
        {

        }



        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }


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



        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        [MustOverride]
        protected async virtual void Query()
        {
            //if (QueryHandler != null)
            //{
            //    OnQuery()
            //}
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;
            BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");

            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要



            List<M> list = new List<M>();

            //提取指定的列名，即条件集合
            int PageNum = 1;
            int PageSize = int.Parse(txtMaxRow.Text);


            // queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            // LimitQueryConditions = QueryConditionFilter.GetFilterExpression<M>();
            // list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, LimitQueryConditions, QueryDto, PageNum, PageSize) as List<M>;
            list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDto, PageNum, PageSize) as List<M>;


            _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();
            _UCBillMasterQuery.ShowSummaryCols();
            if (_UCBillOutlookGridAnalysis != null)
            {
                _UCBillOutlookGridAnalysis.FieldNameList = _UCBillMasterQuery.newSumDataGridViewMaster.FieldNameList;
                _UCBillOutlookGridAnalysis.bindingSourceOutlook.DataSource = list;
                //控制列的显示
                _UCBillOutlookGridAnalysis.ColumnDisplays = _UCBillMasterQuery.newSumDataGridViewMaster.ColumnDisplays;
                _UCBillOutlookGridAnalysis.OnLoadData += _UCBillOutlookGridAnalysis_OnLoadData;
                _UCBillOutlookGridAnalysis.LoadDataToGrid<M>(list);

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




        }

        private void _UCBillOutlookGridAnalysis_OnLoadData(object rowObj)
        {

        }

        ////        public List<Expression<Func<M, object>>> QueryConditions = new List<Expression<Func<M, object>>>();
        //public List<QueryParameter<M>> _queryParameters = new List<QueryParameter<M>>();

        ///// <summary>
        ///// 生成查询UI时传入的针对每个字段的条件
        ///// </summary>
        //public List<QueryParameter<M>> QueryParameters { get => _queryParameters; set => _queryParameters = value; }

        public void Builder()
        {
            BuildInvisibleCols();
            BuildLimitQueryConditions();
            BuildColNameDataDictionary();
            BuildQueryCondition();
            BuildSummaryCols();
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public virtual void BuildQueryCondition()
        {
            //添加默认的
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(M).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
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


        protected virtual void Exit(object thisform)
        {
            //保存配置
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.NewLineChars = Environment.NewLine;
            settings.Indent = true;
            using XmlWriter xmlWriter = XmlWriter.Create("SingleQuery" + typeof(M).Name + "Persistence.xml", settings);
            {
                //xmlWriter.WriteStartDocument(true);
                ////文档类型
                //xmlWriter.WriteDocType("Html", null, null, "<!ENTITY h \"hardcover\">");

                //xmlWriter.WriteStartElement("Html");
                ////命名空间
                //xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www/XMLSchema-instance");
                //xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, @"http://.xsd");
                ////指令
                //String PItext = "type=\"text/xsl\" href=\"book.xsl\"";
                //xmlWriter.WriteProcessingInstruction("xml-stylesheet", PItext);
                ////注释
                //xmlWriter.WriteComment("标题头");
                ////cdata
                //xmlWriter.WriteCData(@"<javasritpt><javasritpt>");
                ws.SaveElementToXml(xmlWriter);
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


        private KryptonDockingWorkspace ws;
        private void BaseBillQueryMC_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            Builder();
            this.CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath == this.ToString()).FirstOrDefault();
            if (this.CurMenuInfo == null)
            {
                MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                return;
            }

            // Setup docking functionality
            ws = kryptonDockingManagerQuery.ManageWorkspace(kryptonDockableWorkspaceQuery);
            kryptonDockingManagerQuery.ManageControl(kryptonPanelQuery, ws);

            kryptonDockingManagerQuery.ManageFloating(MainForm.Instance);

            //创建面板并加入
            KryptonPageCollection Kpages = new KryptonPageCollection();
            if (Kpages.Count == 0)
            {
                Kpages.Add(QueryCondition());

                Kpages.Add(MasterQuery());

                if (WithOutlook)
                {
                    Kpages.Add(UCBillOutlookGridAnalysisLoad());
                }
            }
            try
            {
                //Location of XML file
                string xmlFilePath = "SingleQuery" + typeof(M).Name + "Persistence.xml";
                if (System.IO.File.Exists(xmlFilePath))
                {
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
                if (!(page is KryptonStorePage) && !kryptonDockingManagerQuery.ContainsPage(page))
                {
                    switch (page.UniqueName)
                    {
                        case "查询条件":
                            //kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                            kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                            break;
                        case "查询结果":
                            kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "查询结果").ToArray());
                            break;
                        case "结果分析":
                            kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "结果分析").ToArray());
                            break;
                    }
                }
            }



            List<M> list = new List<M>();
            _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            _UCBillMasterQuery.ShowSummaryCols();

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



        private UCQueryCondition _UCBillQueryCondition;
        private KryptonPage QueryCondition()
        {
            _UCBillQueryCondition = new UCQueryCondition();
            _UCBillQueryCondition.entityType = typeof(M);
            LoadQueryConditionToUI(_UCBillQueryCondition.kryptonPanelQuery);

            KryptonPage page = NewPage("查询条件", 1, _UCBillQueryCondition);
            // Document pages cannot be docked or auto hidden
            //page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            page.AllowDrop = false;
            page.ClearFlags(KryptonPageFlags.All);

            return page;
        }


        private BaseEntity _queryDto = new BaseEntity();

        public BaseEntity QueryDto { get => _queryDto; set => _queryDto = value; }

        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public void LoadQueryConditionToUI(Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            UIQueryHelper<M> uIQueryHelper = new UIQueryHelper<M>();
            uIQueryHelper.ReladtedEntityType = ReladtedEntityType;
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
    | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            //暂时默认了uselike



            //   QueryDto = uIQueryHelper.SetQueryUI(true, kryptonPanel条件生成容器, QueryConditionFilter, 4);
            QueryDto = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, 4);

            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
        }


        /// <summary>
        /// 主表要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> MasterSummaryCols { get; set; } = new List<Expression<Func<M, object>>>();




        /// <summary>
        /// 主表表不可见的列
        /// </summary>
        public List<Expression<Func<M, object>>> MasterInvisibleCols { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillMasterQuery _UCBillMasterQuery;

        private KryptonPage MasterQuery()
        {
            _UCBillMasterQuery = new UCBillMasterQuery();
            _UCBillMasterQuery.entityType = typeof(M);
            List<string> masterlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            _UCBillMasterQuery.SummaryCols = masterlist;
            _UCBillMasterQuery.InvisibleCols = ExpressionHelper.ExpressionListToStringList(MasterInvisibleCols);
            ControlMasterColumnsInvisible(_UCBillMasterQuery.InvisibleCols);

            _UCBillMasterQuery.ColNameDataDictionary = MasterColNameDataDictionary;

            KryptonPage page = NewPage("查询结果", 1, _UCBillMasterQuery);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }

        /// <summary>
        /// 控制字段是否显示，添加到里面的是不显示的
        /// </summary>
        /// <param name="InvisibleCols"></param>
        public void ControlMasterColumnsInvisible(List<string> InvisibleCols)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Fields != null)
                {
                    foreach (var item in CurMenuInfo.tb_P4Fields)
                    {
                        if (item != null)
                        {
                            if (item.tb_fieldinfo != null)
                            {
                                if (!item.IsVisble && !item.tb_fieldinfo.IsChild && !InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
                                {
                                    InvisibleCols.Add(item.tb_fieldinfo.FieldName);
                                }
                            }
                        }

                    }

                }
            }
        }


        /// <summary>
        /// 分析结果中要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> AnalysisSubtotalColumns { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillOutlookGridAnalysis _UCBillOutlookGridAnalysis;


        /// <summary>
        /// 加载分析数据 部分 数据 不能太多。不然性能影响体验
        /// </summary>
        /// <returns></returns>

        private KryptonPage UCBillOutlookGridAnalysisLoad()
        {
            _UCBillOutlookGridAnalysis = new UCBillOutlookGridAnalysis();
            _UCBillOutlookGridAnalysis.entityType = typeof(M);
            List<string> masterlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            _UCBillOutlookGridAnalysis.SummaryCols = masterlist;
            _UCBillOutlookGridAnalysis.InvisibleCols = ExpressionHelper.ExpressionListToStringList(MasterInvisibleCols);
            _UCBillOutlookGridAnalysis.ColNameDataDictionary = MasterColNameDataDictionary;

            KryptonPage page = NewPage("结果分析", 1, _UCBillOutlookGridAnalysis);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }


    }
}