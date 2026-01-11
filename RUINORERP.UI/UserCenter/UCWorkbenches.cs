using FastReport.DevComponents.AdvTree;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;

using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.UserCenter.DataParts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RUINORERP.UI.UserCenter
{
    public partial class UCWorkbenches : UserControl
    {
        public UCWorkbenches()
        {
            InitializeComponent();
        }
        private const string xmlFileNameWithExtension = "UCWorkbenchesPersistence.xml";
        public KryptonDockingWorkspace ws = null;
        private NavigatorMode _mode = NavigatorMode.HeaderBarCheckButtonHeaderGroup;
        KryptonPage todoPage = null;
        KryptonPage cellSettingPage = null;
        //创建面板并加入
        public KryptonPageCollection Kpages { get; set; } = new KryptonPageCollection();

        private async void UCWorkbenches_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }


            #region 请求缓存
            //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
            await UIBizService.RequestCache<tb_Prod>();
            await UIBizService.RequestCache<tb_Employee>();
            #endregion

            // Setup docking functionality
            ws = kryptonDockingManager1.ManageWorkspace(kryptonDockableWorkspaceQuery);
            kryptonDockingManager1.ManageControl(kryptonPanelMainBig, ws);
            kryptonDockingManager1.ManageFloating(MainForm.Instance);
            kryptonDockableWorkspaceQuery.WorkspaceCellAdding += kryptonDockableWorkspaceQuery_WorkspaceCellAdding;
            kryptonDockingManager1.FloatingWindowAdding += KryptonDockingManager1_FloatingWindowAdding;
            kryptonDockingManager1.FloatingWindowRemoved += KryptonDockingManager1_FloatingWindowRemoved;
            kryptonDockingManager1.ShowPageContextMenu += KryptonDockingManager1_ShowPageContextMenu;
            kryptonDockableWorkspaceQuery.DockChanged += KryptonDockableWorkspaceQuery_DockChanged;
            kryptonDockableWorkspaceQuery.ControlAdded += KryptonDockableWorkspaceQuery_ControlAdded;
            kryptonDockingManager1.DockspaceAdding += KryptonDockingManager1_DockspaceAdding;
            kryptonDockingManager1.DockspaceCellAdding += KryptonDockingManager1_DockspaceCellAdding;
            UCTodoList todoList = Startup.GetFromFac<UCTodoList>();

            //todoList= MainForm.Instance.AppContext.GetRequiredService<UCTodoList>();

            todoPage = UIForKryptonHelper.NewPage("待办事项", todoList);
            todoPage.AllowDrop = false;
            todoPage.AutoHiddenSlideSize = new Size(165, 200); // 增加宽度30像素 1
            todoPage.Width = 165; // 显式设置宽度以影响停靠状态的初始大小
            todoPage.MinimumSize = new Size(165, 150); // 设置最小宽度，确保初始加载不被压缩
            ButtonSpecAny buttonSpecRefresh = new ButtonSpecAny();
            buttonSpecRefresh.Text = "刷新";
            buttonSpecRefresh.Click += ButtonSpecAny_Click;
            todoPage.ButtonSpecs.Add(buttonSpecRefresh);
            // Add initial docking pages
            //kryptonDockingManager1.AddToWorkspace("Workspace", new KryptonPage[] { NewDocument(), NewDocument() });
            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Left, new KryptonPage[] { todoPage });
            //kryptonDockingManager1.AddDockspace("Control", DockingEdge.Bottom, new KryptonPage[] { NewInput(), NewPropertyGrid(), NewInput(), NewPropertyGrid() });

            UCCellSetting cellSetting = Startup.GetFromFac<UCCellSetting>();
            cellSettingPage = UIForKryptonHelper.NewPage(GlobalConstants.UCCellSettingName, cellSetting);
            cellSettingPage.AllowDrop = false;
            cellSettingPage.SetFlags(KryptonPageFlags.All);
            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Right, new KryptonPage[] { cellSettingPage });
            kryptonDockingManager1.MakeAutoHiddenRequest(cellSettingPage.UniqueName);//默认加载时隐藏

            if (Kpages.Count == 0)
            {
                await BuilderComponents(Kpages);
            }
            // 异步加载默认布局,避免阻塞UI线程
            await LoadDefaultLayoutFromDbAsync(Kpages);

            // 布局加载完成后加载初始页面
            LoadInitPages();

            for (int i = 0; i < kryptonDockingManager1.Pages.Count(); i++)
            {
                kryptonDockingManager1.Pages[i].ClearFlags(KryptonPageFlags.DockingAllowClose);
            }

            InitSettingPages();

            cellSetting.Kpages = Kpages;
            cellSetting.BuilderCellListTreeView();
            // kryptonDockingManager1.ShowPageContextMenu += new System.EventHandler<Krypton.Docking.ContextPageEventArgs>(this.kryptonDockingManager_ShowPageContextMenu);
            // kryptonDockingManager1.ShowWorkspacePageContextMenu += new System.EventHandler<Krypton.Docking.ContextPageEventArgs>(this.kryptonDockingManager_ShowWorkspacePageContextMenu);

            // 强制设置左侧停靠栏的初始宽度（使用延迟调用确保在框架首轮布局完成后强制覆盖宽度）
            //this.BeginInvoke(new MethodInvoker(() =>
            //{
            //    KryptonWorkspaceCell todoCell = kryptonDockingManager1.FindPageElement(todoPage) as KryptonWorkspaceCell;
            //    if (todoCell != null)
            //    {
            //        todoCell.Width = 245;
            //    }
            //}));

            // UI加载完成后，统一调用各工作单元的LoadData方法加载数据
            await LoadAllWorkCellsDataAsync();
        }

        /// <summary>
        /// 统一加载所有工作单元的数据
        /// 修改为顺序加载，避免并行导致的数据库连接池耗尽问题
        /// </summary>
        private async Task LoadAllWorkCellsDataAsync()
        {
            try
            {
                // 获取所有工作单元页面
                var workCellPages = Kpages.ToList();
                MainForm.Instance.logger?.LogInformation("开始加载 {Count} 个工作单元数据", workCellPages.Count);

                int successCount = 0;
                int failCount = 0;

                // 顺序加载每个工作单元的数据，避免并行导致的数据库连接竞争
                foreach (var page in workCellPages)
                {
                    foreach (Control control in page.Controls)
                    {
                        if (control is UCBaseCell workCell)
                        {
                            try
                            {
                                await workCell.LoadData();
                                successCount++;
                            }
                            catch (Exception ex)
                            {
                                failCount++;
                                MainForm.Instance.logger?.LogError(ex, "加载工作单元 {PageName} 数据失败", page.Text);
                            }
                            break;
                        }
                    }
                }

                MainForm.Instance.logger?.LogInformation(
                    "工作单元数据加载完成，成功: {Success}，失败: {Fail}",
                    successCount, failCount);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "统一加载工作单元数据失败");
            }
        }

        private void ButtonSpecAny_Click(object sender, EventArgs e)
        {

        }

        private void KryptonDockingManager1_DockspaceCellAdding(object sender, DockspaceCellEventArgs e)
        {
            //设置待办事项无法拖动
            if (e.DockspaceControl is KryptonDockspace dockspace)
            {
                var todo = dockspace.CellForPage(todoPage);
                if (todo != null)
                {
                    todo.AllowPageDrag = false;
                    //todo.ContextMenuStrip = new ContextMenuStrip();
                    //todo.ContextMenuStrip.Items.Add("测试");
                }
            }
        }

        private void KryptonDockingManager1_DockspaceAdding(object sender, DockspaceEventArgs e)
        {
            if (e.DockspaceControl is KryptonDockspace dockspace)
            {
                var todo = dockspace.CellForPage(todoPage);
                if (todo != null)
                {
                    todo.AllowPageDrag = false;
                }
            }
        }

        private void KryptonDockableWorkspaceQuery_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is KryptonWorkspaceCell cell)
            {
                //cell.AllowPageDrag = false;
            }
        }


        private void KryptonDockingManager1_ShowPageContextMenu(object sender, ContextPageEventArgs e)
        {
            //不显示右键
            e.Cancel = true;
        }

        private void KryptonDockingManager1_FloatingWindowRemoved(object sender, FloatingWindowEventArgs e)
        {

        }

        private void KryptonDockingManager1_FloatingWindowAdding(object sender, FloatingWindowEventArgs e)
        {
            e.FloatingWindow.CloseBox = false;
        }

        private void KryptonDockableWorkspaceQuery_DockChanged(object sender, EventArgs e)
        {

        }

        private void kryptonDockableWorkspaceQuery_WorkspaceCellAdding(object sender, WorkspaceCellEventArgs e)
        {
            e.Cell.Button.CloseButtonAction = CloseButtonAction.HidePage;
            e.Cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            KryptonWorkspaceCell cell = e.Cell;
            cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            cell.CloseAction += Cell_CloseAction;
            cell.SelectedPageChanged += Cell_SelectedPageChanged;
            cell.ShowContextMenu += Cell_ShowContextMenu;
            cell.Dock = DockStyle.Fill;
            cell.AllowDrop = true;
            cell.AllowPageDrag = true;
            //这里可以对具体的单元设置
            if (cell.Pages.FirstOrDefault(c => c.Name == "") != null)
            {

            }
        }


        /// <summary>
        /// 数据概览（优化版）
        /// 增加空配置检测和友好提示
        /// </summary>
        private async Task BuilderComponents(KryptonPageCollection Kpages)
        {
            try
            {
                tb_RoleInfo CurrentRole = MainForm.Instance.AppContext.CurrentRole;
                tb_UserInfo CurrentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;

                // 先按用户和角色查找配置
                tb_WorkCenterConfig centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList
                    .FirstOrDefault(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentUser.User_ID);

                // 如果没有用户级配置或配置为空，则使用角色级配置
                if (centerConfig == null ||
                    (centerConfig != null && centerConfig.DataOverview.Split(',').ToList().Count == 0))
                {
                    centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList
                        .FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
                }
                else if (string.IsNullOrEmpty(centerConfig.DataOverview))
                {
                    centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList
                        .FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
                }

                // 新增：检测空配置并提示用户
                if (centerConfig == null)
                {
                    MainForm.Instance.PrintInfoLog(
                        "当前角色未配置数据概览单元，界面将显示空白。请联系管理员配置。"
                    );
                }
                else if (string.IsNullOrEmpty(centerConfig.DataOverview))
                {
                    MainForm.Instance.PrintInfoLog(
                        "当前角色配置了工作台，但数据概览为空。"
                    );
                }

                if (centerConfig != null)
                {
                    List<string> DataOverviewItems = centerConfig.DataOverview.Split(',').ToList();

                    foreach (var item in DataOverviewItems)
                    {
                        if (item.IsNullOrEmpty())
                        {
                            continue;
                        }
                        数据概览 DataOverview = (数据概览)Enum.Parse(typeof(数据概览), item);

                        // 异步加载每个数据单元
                        await LoadDataUnitAsync(DataOverview, Kpages);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "构建数据概览组件时发生异常");

                this.Invoke((MethodInvoker)(() =>
                {
                    System.Windows.Forms.MessageBox.Show(
                        "加载工作台数据概览失败，请刷新页面重试。\n\n" +
                        "错误信息：" + ex.Message,
                        "加载失败",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );
                }));
            }
        }

        /// <summary>
        /// 异步加载单个数据单元
        /// </summary>
        private async Task LoadDataUnitAsync(数据概览 DataOverview, KryptonPageCollection Kpages)
        {
            try
            {
                Control controlToAdd = null;
                string pageName = "";
                
                // 创建控件实例
                switch (DataOverview)
                {
                    case 数据概览.销售情况概览:
                        controlToAdd = new UCSalePerformanceCell();
                        pageName = "销售情况概览";
                        break;

                    case 数据概览.销售单元:
                        controlToAdd = new UCSaleCell();
                        pageName = "销售单元";
                        break;

                    case 数据概览.采购单元:
                        controlToAdd = new UCPURCell();
                        pageName = "采购单元";
                        break;

                    case 数据概览.库存单元:
                        controlToAdd = new UCStockCell();
                        pageName = "库存单元";
                        break;

                    case 数据概览.生产单元:
                        controlToAdd = new UCMRPCell();
                        pageName = "生产单元";
                        break;

                    default:
                        return;
                }

                // 在UI线程中创建页面并添加到集合
                await Task.Run(() =>
                {
                    if (controlToAdd != null)
                    {
                        KryptonPage page = UIForKryptonHelper.NewPage(pageName, controlToAdd);

                        this.Invoke((MethodInvoker)(() =>
                        {
                            Kpages.Add(page);
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "加载数据单元 {DataOverview} 失败", DataOverview);
            }
        }





        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            //SaveLayoutToXml();
            SaveAsDefaultLayoutToDb();
        }



        private void InitSettingPages()
        {
            // Setup docking functionality
            kryptonDockableWorkspaceQuery.AllowDrop = true;
            kryptonDockableWorkspaceQuery.AllowPageDrag = true;
            kryptonDockableWorkspaceQuery.Dock = DockStyle.Fill;
            // Set correct initial ribbon palette buttons
            //UpdatePaletteButtons();
            foreach (var item in Kpages)
            {
                KryptonWorkspaceCell cell = kryptonDockableWorkspaceQuery.CellForPage(item);
                cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
                cell.CloseAction += Cell_CloseAction;
                cell.SelectedPageChanged += Cell_SelectedPageChanged;
                cell.ShowContextMenu += Cell_ShowContextMenu;
                cell.Dock = DockStyle.Fill;
                cell.AllowDrop = true;
                cell.AllowPageDrag = true;
            }
        }
        #region cell
        private void Cell_ShowContextMenu(object sender, ShowContextMenuArgs e)
        {
            //KryptonWorkspaceCell kwc = sender as KryptonWorkspaceCell;
            //if (kwc.SelectedPage == null)
            //{
            //    return;
            //}
            //不显示右键
            e.Cancel = true;

            /*
             显示并自定义
            // Yes we want to show a context menu
            e.Cancel = false;

            // Provide the navigator specific menu
            e.KryptonContextMenu = kcmNavigator;

            // Only enable the appropriate options
            kcmFirst.Enabled = (kryptonNavigator1.SelectedIndex > 0);
            kcmPrevious.Enabled = (kryptonNavigator1.SelectedIndex > 0);
            kcmNext.Enabled = (kryptonNavigator1.SelectedIndex < (kryptonNavigator1.Pages.Count - 1));
            kcmLast.Enabled = (kryptonNavigator1.SelectedIndex < (kryptonNavigator1.Pages.Count - 1));
             */
        }
        private void Cell_SelectedPageChanged(object sender, EventArgs e)
        {
            KryptonWorkspaceCell kwc = sender as KryptonWorkspaceCell;
            if (kwc.SelectedPage == null)
            {
                return;
            }
            kwc.Button.CloseButtonDisplay = ButtonDisplay.Hide;

            //选到了第一个。或其他全关了
            //kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.ShowDisabled;
        }
        private void Cell_CloseAction(object sender, CloseActionEventArgs e)
        {
            //关闭事件
            e.Action = CloseButtonAction.HidePage;
        }




        #endregion


        #region 布局
        private void SaveLayoutToXml()
        {
            try
            {
                //保存配置
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.NewLineChars = Environment.NewLine;
                settings.Indent = true;
                string xmlfilepath = System.IO.Path.Combine(Application.StartupPath, xmlFileNameWithExtension);
                if (ws != null)
                {
                    using XmlWriter xmlWriter = XmlWriter.Create(xmlfilepath, settings);
                    {
                        ws.SaveElementToXml(xmlWriter);
                        xmlWriter.Close();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private async Task SaveAsDefaultLayoutToDb()
        {
            try
            {
                //保存配置
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.NewLineChars = Environment.NewLine;
                settings.Indent = true;
                string xmlfilepath = System.IO.Path.Combine(Application.StartupPath, xmlFileNameWithExtension);
                if (ws != null)
                {
                    using XmlWriter xmlWriter = XmlWriter.Create(xmlfilepath, settings);
                    {
                        ws.SaveElementToXml(xmlWriter);
                        xmlWriter.Close();
                    }
                }

                //保存用户的布局
                if (System.IO.File.Exists(xmlfilepath))
                {
                    //加载XML文件
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlfilepath);
                    //获取XML字符串
                    string xmlStr = xmldoc.InnerXml;
                    //字符串转XML
                    //xmldoc.LoadXml(xmlStr);
                    MainForm.Instance.AppContext.CurrentUser_Role_Personalized.WorkCellLayout = xmlStr;
                    var affcet = await MainForm.Instance.AppContext.Db.Storageable<tb_UserPersonalized>(MainForm.Instance.AppContext.CurrentUser_Role_Personalized).ExecuteReturnEntityAsync();
                    if (affcet.UserPersonalizedID > 0)
                    {
                        MainForm.Instance.PrintInfoLog("工作台布局保存成功");
                    }
                }

            }
            catch (Exception ex)
            {
                // 新增：统一异常处理和日志记录
                MainForm.Instance.logger?.LogError(ex, "保存工作台布局时发生异常");

                this.Invoke((MethodInvoker)(() =>
                {
                    System.Windows.Forms.MessageBox.Show(
                        "保存工作台布局失败：" + ex.Message + "\n\n" +
                        "请重试或联系管理员。",
                        "保存失败",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );
                }));
            }
        }

        /// <summary>
        /// 从数据库加载默认布局（异步优化版）
        /// 避免阻塞UI线程，提升用户体验
        /// </summary>
        /// <param name="Kpages">Krypton页面集合</param>
        private async Task LoadDefaultLayoutFromDbAsync(KryptonPageCollection Kpages)
        {
            //检查是否有布局配置
            bool hasLayoutConfig = MainForm.Instance.AppContext.CurrentUser != null
                && MainForm.Instance.AppContext.CurrentUser_Role_Personalized != null
                && !string.IsNullOrEmpty(MainForm.Instance.AppContext.CurrentUser_Role_Personalized.WorkCellLayout);

            if (!hasLayoutConfig)
            {
                return;
            }

            try
            {
                //在后台线程解析XML，避免阻塞UI
                XmlDocument xmldoc = await Task.Run(() =>
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(MainForm.Instance.AppContext.CurrentUser_Role_Personalized.WorkCellLayout);
                    return doc;
                });

                //在UI线程中加载布局，但使用Yield让出控制权
                await Task.Yield(); // 让UI有机会响应用户操作

                XmlNodeReader nodeReader = new XmlNodeReader(xmldoc);
                XmlReaderSettings settings = new XmlReaderSettings();

                using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                        {
                            //加载停靠信息 - 必须在UI线程执行
                            ws.LoadElementFromXml(reader, Kpages);
                            
                            // 每加载一个元素就让出控制权,让UI有机会刷新
                            await Task.Yield();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "从数据库加载默认布局时发生异常");
            }
        }

        /*
         保存一下代码！
         */
        //private void LoadLayoutFromXml(KryptonPageCollection Kpages)
        //{
        //    //加载布局
        //    try
        //    {
        //        //没有个性化文件时用默认的
        //        if (!string.IsNullOrEmpty(MainForm.Instance.AppContext.CurrentUser_Role_Personalized.WorkCellLayout))
        //        {
        //            #region 优化从数据库取
        //            //加载XML文件
        //            XmlDocument xmldoc = new XmlDocument();
        //            //获取XML字符串
        //            string xmlStr = xmldoc.InnerXml;
        //            //字符串转XML
        //            xmldoc.LoadXml(MainForm.Instance.AppContext.CurrentUser_Role.WorkDefaultLayout);

        //            XmlNodeReader nodeReader = new XmlNodeReader(xmldoc);
        //            XmlReaderSettings settings = new XmlReaderSettings();
        //            using (XmlReader reader = XmlReader.Create(nodeReader, settings))
        //            {
        //                while (reader.Read())
        //                {
        //                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
        //                    {
        //                        //加载停靠信息
        //                        ws.LoadElementFromXml(reader, Kpages);
        //                    }
        //                }

        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            string xmlfilepath = System.IO.Path.Combine(Application.StartupPath, xmlFileNameWithExtension);
        //            //Location of XML file
        //            if (System.IO.File.Exists(xmlfilepath))
        //            {
        //                #region load
        //                // Create the XmlNodeReader object.
        //                XmlDocument doc = new XmlDocument();
        //                doc.Load(xmlfilepath);
        //                XmlNodeReader nodeReader = new XmlNodeReader(doc);
        //                // Set the validation settings.
        //                XmlReaderSettings settings = new XmlReaderSettings();
        //                //settings.ValidationType = ValidationType.Schema;
        //                //settings.Schemas.Add("urn:bookstore-schema", "books.xsd");
        //                //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
        //                //settings.NewLineChars = Environment.NewLine;
        //                //settings.Indent = true;

        //                using (XmlReader reader = XmlReader.Create(nodeReader, settings))
        //                {
        //                    while (reader.Read())
        //                    {
        //                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
        //                        {
        //                            //加载停靠信息
        //                            ws.LoadElementFromXml(reader, Kpages);
        //                        }
        //                    }

        //                }
        //                #endregion
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MainForm.Instance.uclog.AddLog("加载查询页布局配置文件出错。" + ex.Message, Global.UILogType.错误);
        //        MainForm.Instance.logger.LogError(ex, "加载查询页布局配置文件出错。");
        //    }
        //}

        #endregion

        private void btnReload_Click(object sender, EventArgs e)
        {
            //KryptonPage[] pages = kryptonDockingManager1.PagesFloating;
            //kryptonDockingManager1.RemovePages(pages, true);
            //kryptonDockingManager1.ClearStoredPages(pages);
            //kryptonDockingManager1.Clear();
            //kryptonDockingManager1.ClearAllStoredPages();
            //Kpages.Clear();

            todoPage.Visible = true;
            //BuilderComponents(Kpages);
            LoadInitPages();
            foreach (KryptonPage page in Kpages)
            {
                //page.ClearFlags(KryptonPageFlags.DockingAllowClose);
                page.Visible = true;
            }
        }


        private void LoadInitPages()
        {
            //如果加载过的停靠信息中不正常。就手动初始化
            foreach (KryptonPage page in Kpages)
            {
                if (!(page is KryptonStorePage) && !kryptonDockingManager1.ContainsPage(page.UniqueName))
                {
                    switch (page.UniqueName)
                    {
                        case "查询条件":
                            //kryptonDockingManager1.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());

                            break;
                        case "明细信息":
                            // kryptonDockingManager1.AddDockspace("Control", DockingEdge.Left, Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                            break;
                        case "单据信息":

                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "生产").ToArray());
                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "单据信息").ToArray());
                            break;
                        default:
                            //kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == page.UniqueName).ToArray());

                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == page.UniqueName).ToArray());


                            break;

                    }
                }
            }
        }

        private void toolStripbtnProperty_Click(object sender, EventArgs e)
        {

        }

        private void btnLayout_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "网格分布":
                    kryptonDockableWorkspaceQuery.ApplyGridPages();
                    break;
                case "纵向分布":
                    kryptonDockableWorkspaceQuery.ApplyGridPages(false, Orientation.Horizontal, 1);
                    break;
                case "横向分布":
                    kryptonDockableWorkspaceQuery.ApplyGridPages(false, Orientation.Vertical, 1);
                    break;
                default:
                    break;
            }
        }
    }
}
