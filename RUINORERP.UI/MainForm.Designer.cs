using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;

namespace RUINORERP.UI
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                // 取消事件订阅，避免内存泄漏
                if (communicationService != null)
                {
                    communicationService.ReconnectFailed -= OnReconnectFailed;
                }
                
   
                
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            this.buttonSpecNavigator1 = new Krypton.Navigator.ButtonSpecNavigator();
            this.kryptonPage5 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage2 = new Krypton.Navigator.KryptonPage();
            this.btnSwichLang = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton6 = new Krypton.Toolkit.KryptonButton();
            this.btnUnDoTest = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton5 = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton4 = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton3 = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            this.kryptonPage4 = new Krypton.Navigator.KryptonPage();
            this.kryptonbtnInitLoadMenu = new Krypton.Toolkit.KryptonButton();
            this.kryptonPage3 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage1 = new Krypton.Navigator.KryptonPage();
            this.buttonTopArrow = new Krypton.Navigator.ButtonSpecNavigator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SystemOperatorState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripddbtnSkin = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownBtnRoles = new System.Windows.Forms.ToolStripDropDownButton();
            this.cmbRoles = new System.Windows.Forms.ToolStripComboBox();
            this.lblServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusGlobal = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblServerInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.lblVer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolBtnlogOff = new System.Windows.Forms.ToolStripButton();
            this.toolBtnExit = new System.Windows.Forms.ToolStripButton();
            this.btntsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbtnloginFileServer = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSysTest = new System.Windows.Forms.ToolStripButton();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭所有窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务缓存测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonDockableWorkspace1 = new Krypton.Docking.KryptonDockableWorkspace();
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.kryptonTaskDialog = new Krypton.Toolkit.KryptonTaskDialog();
            this.kryptonTaskDialogCommand1 = new Krypton.Toolkit.KryptonTaskDialogCommand();
            this.kryptonDockingManager1 = new Krypton.Docking.KryptonDockingManager();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusTimer = new System.Windows.Forms.Timer(this.components);
            this.kryptonContextMenuItems1 = new Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItem1 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonPanelBigg = new Krypton.Toolkit.KryptonPanel();
            this.kryptonSeparator1 = new Krypton.Toolkit.KryptonSeparator();
            this.toolStripFunctionMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripMenuSearcher = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainerMenu = new System.Windows.Forms.ToolStripContainer();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).BeginInit();
            this.kryptonPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).BeginInit();
            this.kryptonPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspace1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelBigg)).BeginInit();
            this.kryptonPanelBigg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSeparator1)).BeginInit();
            this.toolStripFunctionMenu.SuspendLayout();
            this.toolStripContainerMenu.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainerMenu.ContentPanel.SuspendLayout();
            this.toolStripContainerMenu.TopToolStripPanel.SuspendLayout();
            this.toolStripContainerMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonNavigator1
            // 
            this.kryptonNavigator1.AutoSize = true;
            this.kryptonNavigator1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.kryptonNavigator1.Bar.BarMapExtraText = Krypton.Navigator.MapKryptonPageText.DescriptionTitle;
            this.kryptonNavigator1.Bar.BarMapImage = Krypton.Navigator.MapKryptonPageImage.None;
            this.kryptonNavigator1.Bar.BarMapText = Krypton.Navigator.MapKryptonPageText.TitleDescription;
            this.kryptonNavigator1.Bar.BarMultiline = Krypton.Navigator.BarMultiline.Singleline;
            this.kryptonNavigator1.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryptonNavigator1.Bar.CheckButtonStyle = Krypton.Toolkit.ButtonStyle.NavigatorStack;
            this.kryptonNavigator1.Bar.ItemAlignment = Krypton.Toolkit.RelativePositionAlign.Near;
            this.kryptonNavigator1.Bar.ItemMaximumSize = new System.Drawing.Size(200, 200);
            this.kryptonNavigator1.Bar.ItemMinimumSize = new System.Drawing.Size(20, 20);
            this.kryptonNavigator1.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.Auto;
            this.kryptonNavigator1.Bar.ItemSizing = Krypton.Navigator.BarItemSizing.SameHeight;
            this.kryptonNavigator1.Bar.TabBorderStyle = Krypton.Toolkit.TabBorderStyle.RoundedOutsizeMedium;
            this.kryptonNavigator1.Bar.TabStyle = Krypton.Toolkit.TabStyle.HighProfile;
            this.kryptonNavigator1.Button.ButtonDisplayLogic = Krypton.Navigator.ButtonDisplayLogic.ContextNextPrevious;
            this.kryptonNavigator1.Button.ButtonSpecs.Add(this.buttonSpecNavigator1);
            this.kryptonNavigator1.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.HidePage;
            this.kryptonNavigator1.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonNavigator1.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.SelectPage;
            this.kryptonNavigator1.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.ShowEnabled;
            this.kryptonNavigator1.Button.ContextMenuMapImage = Krypton.Navigator.MapKryptonPageImage.Small;
            this.kryptonNavigator1.Button.ContextMenuMapText = Krypton.Navigator.MapKryptonPageText.Title;
            this.kryptonNavigator1.Button.NextButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            this.kryptonNavigator1.Button.NextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            this.kryptonNavigator1.Button.PreviousButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            this.kryptonNavigator1.Button.PreviousButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            this.kryptonNavigator1.ControlKryptonFormFeatures = false;
            this.kryptonNavigator1.Dock = System.Windows.Forms.DockStyle.Left;
            this.kryptonNavigator1.Header.HeaderPositionBar = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonNavigator1.Header.HeaderPositionPrimary = Krypton.Toolkit.VisualOrientation.Top;
            this.kryptonNavigator1.Header.HeaderPositionSecondary = Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryptonNavigator1.Header.HeaderStyleBar = Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonNavigator1.Header.HeaderStylePrimary = Krypton.Toolkit.HeaderStyle.Primary;
            this.kryptonNavigator1.Header.HeaderStyleSecondary = Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonNavigator1.Header.HeaderValuesPrimary.Heading = "";
            this.kryptonNavigator1.Header.HeaderValuesPrimary.MapDescription = Krypton.Navigator.MapKryptonPageText.None;
            this.kryptonNavigator1.Header.HeaderValuesPrimary.MapHeading = Krypton.Navigator.MapKryptonPageText.TitleText;
            this.kryptonNavigator1.Header.HeaderValuesPrimary.MapImage = Krypton.Navigator.MapKryptonPageImage.SmallMedium;
            this.kryptonNavigator1.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigator1.Name = "kryptonNavigator1";
            this.kryptonNavigator1.NavigatorMode = Krypton.Navigator.NavigatorMode.OutlookFull;
            this.kryptonNavigator1.Owner = null;
            this.kryptonNavigator1.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigator1.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage5,
            this.kryptonPage2,
            this.kryptonPage4});
            this.kryptonNavigator1.SelectedIndex = 0;
            this.kryptonNavigator1.Size = new System.Drawing.Size(168, 650);
            this.kryptonNavigator1.TabIndex = 0;
            // 
            // buttonSpecNavigator1
            // 
            this.buttonSpecNavigator1.Edge = Krypton.Toolkit.PaletteRelativeEdgeAlign.Near;
            this.buttonSpecNavigator1.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowLeft;
            this.buttonSpecNavigator1.TypeRestricted = Krypton.Navigator.PaletteNavButtonSpecStyle.ArrowLeft;
            this.buttonSpecNavigator1.UniqueName = "8DAB706D7A2246700297F8B9DAF38BF6";
            this.buttonSpecNavigator1.Click += new System.EventHandler(this.buttonSpecNavigator1_Click);
            // 
            // kryptonPage5
            // 
            this.kryptonPage5.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage5.Flags = 65534;
            this.kryptonPage5.LastVisibleSet = true;
            this.kryptonPage5.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage5.Name = "kryptonPage5";
            this.kryptonPage5.Size = new System.Drawing.Size(166, 522);
            this.kryptonPage5.Text = "仓库";
            this.kryptonPage5.TextDescription = "";
            this.kryptonPage5.TextTitle = "仓库系统";
            this.kryptonPage5.ToolTipTitle = "Page ToolTip";
            this.kryptonPage5.UniqueName = "89E27923BF004A86CE91623446B075D9";
            // 
            // kryptonPage2
            // 
            this.kryptonPage2.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage2.Controls.Add(this.btnSwichLang);
            this.kryptonPage2.Controls.Add(this.kryptonButton6);
            this.kryptonPage2.Controls.Add(this.btnUnDoTest);
            this.kryptonPage2.Controls.Add(this.kryptonButton5);
            this.kryptonPage2.Controls.Add(this.kryptonButton4);
            this.kryptonPage2.Controls.Add(this.kryptonButton3);
            this.kryptonPage2.Controls.Add(this.kryptonButton2);
            this.kryptonPage2.Controls.Add(this.kryptonButton1);
            this.kryptonPage2.Flags = 65534;
            this.kryptonPage2.LastVisibleSet = true;
            this.kryptonPage2.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage2.Name = "kryptonPage2";
            this.kryptonPage2.Size = new System.Drawing.Size(127, 450);
            this.kryptonPage2.Text = "业务系统";
            this.kryptonPage2.TextTitle = "业务系统";
            this.kryptonPage2.ToolTipTitle = "Page ToolTip";
            this.kryptonPage2.UniqueName = "62F97060D4C3476BF6A7AEB6C91A9721";
            // 
            // btnSwichLang
            // 
            this.btnSwichLang.Location = new System.Drawing.Point(28, 387);
            this.btnSwichLang.Name = "btnSwichLang";
            this.btnSwichLang.Size = new System.Drawing.Size(90, 25);
            this.btnSwichLang.TabIndex = 8;
            this.btnSwichLang.Values.Text = "语言切换";
            this.btnSwichLang.Click += new System.EventHandler(this.btnSwichLang_Click);
            // 
            // kryptonButton6
            // 
            this.kryptonButton6.Location = new System.Drawing.Point(28, 338);
            this.kryptonButton6.Name = "kryptonButton6";
            this.kryptonButton6.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton6.TabIndex = 7;
            this.kryptonButton6.Values.Text = "菜单测试";
            this.kryptonButton6.Click += new System.EventHandler(this.kryptonButton6_Click);
            // 
            // btnUnDoTest
            // 
            this.btnUnDoTest.Location = new System.Drawing.Point(28, 77);
            this.btnUnDoTest.Name = "btnUnDoTest";
            this.btnUnDoTest.Size = new System.Drawing.Size(90, 25);
            this.btnUnDoTest.TabIndex = 6;
            this.btnUnDoTest.Values.Text = "撤销测试";
            this.btnUnDoTest.Click += new System.EventHandler(this.btnUnDoTest_Click);
            // 
            // kryptonButton5
            // 
            this.kryptonButton5.Location = new System.Drawing.Point(28, 295);
            this.kryptonButton5.Name = "kryptonButton5";
            this.kryptonButton5.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton5.TabIndex = 5;
            this.kryptonButton5.Values.Text = "框架测试";
            this.kryptonButton5.Click += new System.EventHandler(this.kryptonButton5_Click);
            // 
            // kryptonButton4
            // 
            this.kryptonButton4.Location = new System.Drawing.Point(28, 251);
            this.kryptonButton4.Name = "kryptonButton4";
            this.kryptonButton4.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton4.TabIndex = 4;
            this.kryptonButton4.Values.Text = "增删改查";
            this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);
            // 
            // kryptonButton3
            // 
            this.kryptonButton3.Location = new System.Drawing.Point(28, 24);
            this.kryptonButton3.Name = "kryptonButton3";
            this.kryptonButton3.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton3.TabIndex = 3;
            this.kryptonButton3.Values.Text = "生成实体";
            this.kryptonButton3.Click += new System.EventHandler(this.kryptonButton3_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(28, 194);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton2.TabIndex = 2;
            this.kryptonButton2.Values.Text = "添加TAB进去";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(28, 139);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton1.TabIndex = 1;
            this.kryptonButton1.Values.Text = "添加tab";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // kryptonPage4
            // 
            this.kryptonPage4.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage4.Controls.Add(this.kryptonbtnInitLoadMenu);
            this.kryptonPage4.Flags = 65534;
            this.kryptonPage4.LastVisibleSet = true;
            this.kryptonPage4.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage4.Name = "kryptonPage4";
            this.kryptonPage4.Size = new System.Drawing.Size(175, 444);
            this.kryptonPage4.Text = "系统设置";
            this.kryptonPage4.TextTitle = "系统菜单";
            this.kryptonPage4.ToolTipTitle = "系统设置";
            this.kryptonPage4.UniqueName = "E304C11079C1473385846F007F3FB0FB";
            // 
            // kryptonbtnInitLoadMenu
            // 
            this.kryptonbtnInitLoadMenu.Location = new System.Drawing.Point(28, 24);
            this.kryptonbtnInitLoadMenu.Name = "kryptonbtnInitLoadMenu";
            this.kryptonbtnInitLoadMenu.Size = new System.Drawing.Size(90, 25);
            this.kryptonbtnInitLoadMenu.TabIndex = 4;
            this.kryptonbtnInitLoadMenu.Values.Text = "加载菜单";
            this.kryptonbtnInitLoadMenu.Click += new System.EventHandler(this.kryptonbtnInitLoadMenu_Click);
            // 
            // kryptonPage3
            // 
            this.kryptonPage3.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage3.Flags = 65534;
            this.kryptonPage3.LastVisibleSet = true;
            this.kryptonPage3.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage3.Name = "kryptonPage3";
            this.kryptonPage3.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage3.Text = "kryptonPage3";
            this.kryptonPage3.ToolTipTitle = "Page ToolTip";
            this.kryptonPage3.UniqueName = "270CB1D87A4A4C95ABAD1F8B8CF6B1AD";
            // 
            // kryptonPage1
            // 
            this.kryptonPage1.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage1.Flags = 65534;
            this.kryptonPage1.LastVisibleSet = true;
            this.kryptonPage1.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage1.Name = "kryptonPage1";
            this.kryptonPage1.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage1.Text = "kryptonPage1";
            this.kryptonPage1.ToolTipTitle = "Page ToolTip";
            this.kryptonPage1.UniqueName = "4334EE2FF2B048C77C8564CA4EC9B011";
            // 
            // buttonTopArrow
            // 
            this.buttonTopArrow.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowUp;
            this.buttonTopArrow.TypeRestricted = Krypton.Navigator.PaletteNavButtonSpecStyle.ArrowUp;
            this.buttonTopArrow.UniqueName = "E1E00112A6104D0EE1E00112A6104D0E";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemOperatorState,
            this.toolStripddbtnSkin,
            this.toolStripDropDownBtnRoles,
            this.lblServerStatus,
            this.lblStatusGlobal,
            this.lblServerInfo,
            this.progressBar,
            this.toolStripDropDownButton1,
            this.lblVer});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(1353, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // SystemOperatorState
            // 
            this.SystemOperatorState.Image = global::RUINORERP.UI.Properties.Resources.user_blue;
            this.SystemOperatorState.Margin = new System.Windows.Forms.Padding(0, 3, 50, 2);
            this.SystemOperatorState.Name = "SystemOperatorState";
            this.SystemOperatorState.Size = new System.Drawing.Size(16, 17);
            // 
            // toolStripddbtnSkin
            // 
            this.toolStripddbtnSkin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem1});
            this.toolStripddbtnSkin.Image = global::RUINORERP.UI.Properties.Resources.RGB;
            this.toolStripddbtnSkin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripddbtnSkin.Margin = new System.Windows.Forms.Padding(0, 2, 50, 0);
            this.toolStripddbtnSkin.Name = "toolStripddbtnSkin";
            this.toolStripddbtnSkin.Size = new System.Drawing.Size(62, 20);
            this.toolStripddbtnSkin.Text = "风格";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItem3.Text = "2010";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItem1.Text = "2017";
            // 
            // toolStripDropDownBtnRoles
            // 
            this.toolStripDropDownBtnRoles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbRoles});
            this.toolStripDropDownBtnRoles.Image = global::RUINORERP.UI.Properties.Resources.users_1;
            this.toolStripDropDownBtnRoles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownBtnRoles.Name = "toolStripDropDownBtnRoles";
            this.toolStripDropDownBtnRoles.Size = new System.Drawing.Size(62, 20);
            this.toolStripDropDownBtnRoles.Text = "角色";
            // 
            // cmbRoles
            // 
            this.cmbRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoles.Name = "cmbRoles";
            this.cmbRoles.Size = new System.Drawing.Size(121, 25);
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoToolTip = true;
            this.lblServerStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblServerStatus.DoubleClickEnabled = true;
            this.lblServerStatus.Image = global::RUINORERP.UI.Properties.Resources.ServerStatus;
            this.lblServerStatus.Margin = new System.Windows.Forms.Padding(100, 3, 0, 2);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(16, 17);
            this.lblServerStatus.Text = "服务器状态";
            this.lblServerStatus.Click += new System.EventHandler(this.lblServerStatus_Click);
            this.lblServerStatus.DoubleClick += new System.EventHandler(this.lblServerStatus_DoubleClick);
            // 
            // lblStatusGlobal
            // 
            this.lblStatusGlobal.Margin = new System.Windows.Forms.Padding(50, 3, 0, 2);
            this.lblStatusGlobal.Name = "lblStatusGlobal";
            this.lblStatusGlobal.Size = new System.Drawing.Size(85, 17);
            this.lblStatusGlobal.Text = "全局状态提示";
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.Margin = new System.Windows.Forms.Padding(50, 3, 0, 2);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Padding = new System.Windows.Forms.Padding(300, 0, 0, 0);
            this.lblServerInfo.Size = new System.Drawing.Size(319, 17);
            this.lblServerInfo.Text = "....";
            this.lblServerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblServerInfo.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(150, 16);
            this.progressBar.ToolTipText = "全局进度条";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Visible = false;
            // 
            // lblVer
            // 
            this.lblVer.Name = "lblVer";
            this.lblVer.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.lblVer.Size = new System.Drawing.Size(128, 17);
            this.lblVer.Text = "v3.1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnUpdate,
            this.toolBtnlogOff,
            this.toolBtnExit,
            this.btntsbRefresh,
            this.tsbtnloginFileServer,
            this.tsbtnSysTest});
            this.toolStrip1.Location = new System.Drawing.Point(3, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(426, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripBtnUpdate
            // 
            this.toolStripBtnUpdate.Image = global::RUINORERP.UI.Properties.Resources.download;
            this.toolStripBtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnUpdate.Name = "toolStripBtnUpdate";
            this.toolStripBtnUpdate.Size = new System.Drawing.Size(79, 22);
            this.toolStripBtnUpdate.Text = "在线更新";
            this.toolStripBtnUpdate.Click += new System.EventHandler(this.toolStripBtnUpdate_Click);
            // 
            // toolBtnlogOff
            // 
            this.toolBtnlogOff.Image = global::RUINORERP.UI.Properties.Resources._lock;
            this.toolBtnlogOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnlogOff.Margin = new System.Windows.Forms.Padding(50, 1, 0, 2);
            this.toolBtnlogOff.Name = "toolBtnlogOff";
            this.toolBtnlogOff.Size = new System.Drawing.Size(53, 22);
            this.toolBtnlogOff.Text = "注销";
            this.toolBtnlogOff.Click += new System.EventHandler(this.toolBtnlogOff_Click);
            // 
            // toolBtnExit
            // 
            this.toolBtnExit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolBtnExit.Image = global::RUINORERP.UI.Properties.Resources.exit_y;
            this.toolBtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnExit.Name = "toolBtnExit";
            this.toolBtnExit.Size = new System.Drawing.Size(53, 22);
            this.toolBtnExit.Text = "退出";
            this.toolBtnExit.Click += new System.EventHandler(this.toolBtnExit_Click);
            // 
            // btntsbRefresh
            // 
            this.btntsbRefresh.Image = global::RUINORERP.UI.Properties.Resources.Refresh;
            this.btntsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btntsbRefresh.Name = "btntsbRefresh";
            this.btntsbRefresh.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.btntsbRefresh.Size = new System.Drawing.Size(103, 22);
            this.btntsbRefresh.Text = "刷新";
            this.btntsbRefresh.Click += new System.EventHandler(this.btntsbRefresh_Click);
            // 
            // tsbtnloginFileServer
            // 
            this.tsbtnloginFileServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnloginFileServer.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnloginFileServer.Image")));
            this.tsbtnloginFileServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnloginFileServer.Name = "tsbtnloginFileServer";
            this.tsbtnloginFileServer.Size = new System.Drawing.Size(76, 22);
            this.tsbtnloginFileServer.Text = "文件服务器";
            this.tsbtnloginFileServer.Click += new System.EventHandler(this.tsbtnloginFileServer_Click);
            // 
            // tsbtnSysTest
            // 
            this.tsbtnSysTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnSysTest.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSysTest.Image")));
            this.tsbtnSysTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSysTest.Name = "tsbtnSysTest";
            this.tsbtnSysTest.Size = new System.Drawing.Size(76, 22);
            this.tsbtnSysTest.Text = "系统测试用";
            this.tsbtnSysTest.Visible = false;
            this.tsbtnSysTest.Click += new System.EventHandler(this.tsbtnSysTest_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口ToolStripMenuItem,
            this.系统工具ToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1353, 24);
            this.menuStripMain.TabIndex = 9;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // 窗口ToolStripMenuItem
            // 
            this.窗口ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭所有窗口ToolStripMenuItem,
            this.刷新菜单ToolStripMenuItem});
            this.窗口ToolStripMenuItem.Name = "窗口ToolStripMenuItem";
            this.窗口ToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.窗口ToolStripMenuItem.Text = "【窗口】";
            // 
            // 关闭所有窗口ToolStripMenuItem
            // 
            this.关闭所有窗口ToolStripMenuItem.Name = "关闭所有窗口ToolStripMenuItem";
            this.关闭所有窗口ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.关闭所有窗口ToolStripMenuItem.Text = "关闭所有窗口";
            // 
            // 刷新菜单ToolStripMenuItem
            // 
            this.刷新菜单ToolStripMenuItem.Name = "刷新菜单ToolStripMenuItem";
            this.刷新菜单ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.刷新菜单ToolStripMenuItem.Text = "刷新菜单";
            // 
            // 系统工具ToolStripMenuItem
            // 
            this.系统工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务缓存测试ToolStripMenuItem});
            this.系统工具ToolStripMenuItem.Name = "系统工具ToolStripMenuItem";
            this.系统工具ToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.系统工具ToolStripMenuItem.Text = "【系统工具】";
            // 
            // 服务缓存测试ToolStripMenuItem
            // 
            this.服务缓存测试ToolStripMenuItem.Name = "服务缓存测试ToolStripMenuItem";
            this.服务缓存测试ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.服务缓存测试ToolStripMenuItem.Text = "服务缓存测试";
            this.服务缓存测试ToolStripMenuItem.Click += new System.EventHandler(this.服务缓存测试ToolStripMenuItem_Click);
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            // 
            // kryptonDockableWorkspace1
            // 
            this.kryptonDockableWorkspace1.ActivePage = null;
            this.kryptonDockableWorkspace1.AutoHiddenHost = false;
            this.kryptonDockableWorkspace1.CompactFlags = ((Krypton.Workspace.CompactFlags)(((Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | Krypton.Workspace.CompactFlags.PromoteLeafs)));
            this.kryptonDockableWorkspace1.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.GridBackgroundCustom1;
            this.kryptonDockableWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDockableWorkspace1.Location = new System.Drawing.Point(178, 0);
            this.kryptonDockableWorkspace1.Name = "kryptonDockableWorkspace1";
            // 
            // 
            // 
            this.kryptonDockableWorkspace1.Root.UniqueName = "D799A95FA1784B884F921928717E3ABD";
            this.kryptonDockableWorkspace1.Root.WorkspaceControl = this.kryptonDockableWorkspace1;
            this.kryptonDockableWorkspace1.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonDockableWorkspace1.ShowMaximizeButton = false;
            this.kryptonDockableWorkspace1.Size = new System.Drawing.Size(1175, 650);
            this.kryptonDockableWorkspace1.SplitterWidth = 5;
            this.kryptonDockableWorkspace1.TabIndex = 2;
            this.kryptonDockableWorkspace1.TabStop = true;
            // 
            // imageListSmall
            // 
            this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmall.Images.SetKeyName(0, "document_plain.png");
            this.imageListSmall.Images.SetKeyName(1, "preferences.png");
            this.imageListSmall.Images.SetKeyName(2, "information2.png");
            // 
            // kryptonTaskDialog
            // 
            this.kryptonTaskDialog.CheckboxText = null;
            this.kryptonTaskDialog.CommandButtons.AddRange(new Krypton.Toolkit.KryptonTaskDialogCommand[] {
            this.kryptonTaskDialogCommand1});
            this.kryptonTaskDialog.Content = null;
            this.kryptonTaskDialog.DefaultRadioButton = null;
            this.kryptonTaskDialog.FooterHyperlink = null;
            this.kryptonTaskDialog.FooterText = null;
            this.kryptonTaskDialog.MainInstruction = null;
            this.kryptonTaskDialog.TextExtra = "Ctrl+C to copy";
            this.kryptonTaskDialog.WindowTitle = null;
            // 
            // kryptonTaskDialogCommand1
            // 
            this.kryptonTaskDialogCommand1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kryptonTaskDialogCommand1.Text = "现在处理";
            this.kryptonTaskDialogCommand1.Execute += new System.EventHandler(this.kryptonTaskDialogCommand1_Execute);
            // 
            // kryptonDockingManager1
            // 
            this.kryptonDockingManager1.DefaultCloseRequest = Krypton.Docking.DockingCloseRequest.RemovePageAndDispose;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusTimer
            // 
            this.statusTimer.Interval = 8000;
            this.statusTimer.Tick += new System.EventHandler(this.statusTimer_Tick);
            // 
            // kryptonContextMenuItem1
            // 
            this.kryptonContextMenuItem1.Text = "Menu Item";
            // 
            // kryptonPanelBigg
            // 
            this.kryptonPanelBigg.Controls.Add(this.kryptonDockableWorkspace1);
            this.kryptonPanelBigg.Controls.Add(this.kryptonSeparator1);
            this.kryptonPanelBigg.Controls.Add(this.kryptonNavigator1);
            this.kryptonPanelBigg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelBigg.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelBigg.Name = "kryptonPanelBigg";
            this.kryptonPanelBigg.Size = new System.Drawing.Size(1353, 650);
            this.kryptonPanelBigg.TabIndex = 15;
            // 
            // kryptonSeparator1
            // 
            this.kryptonSeparator1.Dock = System.Windows.Forms.DockStyle.Left;
            this.kryptonSeparator1.Location = new System.Drawing.Point(168, 0);
            this.kryptonSeparator1.Name = "kryptonSeparator1";
            this.kryptonSeparator1.Size = new System.Drawing.Size(10, 650);
            this.kryptonSeparator1.TabIndex = 3;
            // 
            // toolStripFunctionMenu
            // 
            this.toolStripFunctionMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripFunctionMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripFunctionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuSearcher});
            this.toolStripFunctionMenu.Location = new System.Drawing.Point(15, 49);
            this.toolStripFunctionMenu.Name = "toolStripFunctionMenu";
            this.toolStripFunctionMenu.Size = new System.Drawing.Size(135, 25);
            this.toolStripFunctionMenu.TabIndex = 17;
            this.toolStripFunctionMenu.Text = "toolStrip2";
            // 
            // toolStripMenuSearcher
            // 
            this.toolStripMenuSearcher.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.toolStripMenuSearcher.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.toolStripMenuSearcher.AutoToolTip = true;
            this.toolStripMenuSearcher.Name = "toolStripMenuSearcher";
            this.toolStripMenuSearcher.Size = new System.Drawing.Size(121, 25);
            this.toolStripMenuSearcher.ToolTipText = "菜单搜索";
            // 
            // toolStripContainerMenu
            // 
            // 
            // toolStripContainerMenu.BottomToolStripPanel
            // 
            this.toolStripContainerMenu.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainerMenu.ContentPanel
            // 
            this.toolStripContainerMenu.ContentPanel.AutoScroll = true;
            this.toolStripContainerMenu.ContentPanel.Controls.Add(this.kryptonPanelBigg);
            this.toolStripContainerMenu.ContentPanel.Size = new System.Drawing.Size(1353, 650);
            this.toolStripContainerMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainerMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainerMenu.Name = "toolStripContainerMenu";
            this.toolStripContainerMenu.Size = new System.Drawing.Size(1353, 746);
            this.toolStripContainerMenu.TabIndex = 18;
            this.toolStripContainerMenu.Text = "toolStripContainer1";
            // 
            // toolStripContainerMenu.TopToolStripPanel
            // 
            this.toolStripContainerMenu.TopToolStripPanel.Controls.Add(this.menuStripMain);
            this.toolStripContainerMenu.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainerMenu.TopToolStripPanel.Controls.Add(this.toolStripFunctionMenu);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1353, 746);
            this.Controls.Add(this.toolStripContainerMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "企业数字化集成ERP v2.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).EndInit();
            this.kryptonPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).EndInit();
            this.kryptonPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspace1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelBigg)).EndInit();
            this.kryptonPanelBigg.ResumeLayout(false);
            this.kryptonPanelBigg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSeparator1)).EndInit();
            this.toolStripFunctionMenu.ResumeLayout(false);
            this.toolStripFunctionMenu.PerformLayout();
            this.toolStripContainerMenu.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMenu.BottomToolStripPanel.PerformLayout();
            this.toolStripContainerMenu.ContentPanel.ResumeLayout(false);
            this.toolStripContainerMenu.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMenu.TopToolStripPanel.PerformLayout();
            this.toolStripContainerMenu.ResumeLayout(false);
            this.toolStripContainerMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private KryptonNavigator kryptonNavigator1;
        private KryptonPage kryptonPage3;
        private KryptonPage kryptonPage5;
        private KryptonPage kryptonPage1;
        private ButtonSpecNavigator buttonTopArrow;
        private KryptonPage kryptonPage2;
        private ButtonSpecNavigator buttonSpecNavigator1;
        private KryptonButton kryptonButton1;
        private KryptonButton kryptonButton2;
        private KryptonButton kryptonButton3;
        private KryptonButton kryptonButton4;
        private KryptonButton kryptonButton5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripddbtnSkin;
        private System.Windows.Forms.MenuStrip menuStripMain;
        internal System.Windows.Forms.ToolStripStatusLabel SystemOperatorState;
        private System.Windows.Forms.ToolStripMenuItem 窗口ToolStripMenuItem;
        internal KryptonDockableWorkspace kryptonDockableWorkspace1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private Krypton.Toolkit.KryptonButton btnUnDoTest;
        internal Krypton.Docking.KryptonDockingManager kryptonDockingManager1;
        internal KryptonManager kryptonManager;
        private System.Windows.Forms.ToolStripMenuItem 关闭所有窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新菜单ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListSmall;
        private KryptonButton kryptonButton6;
        private KryptonButton btnSwichLang;
        private KryptonPage kryptonPage4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private KryptonTaskDialog kryptonTaskDialog;
        private KryptonTaskDialogCommand kryptonTaskDialogCommand1;
        private System.Windows.Forms.ToolStripButton toolBtnlogOff;
        private System.Windows.Forms.ToolStripButton toolBtnExit;
        private KryptonButton kryptonbtnInitLoadMenu;
        private System.Windows.Forms.ToolStripButton toolStripBtnUpdate;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownBtnRoles;
        private System.Windows.Forms.ToolStripComboBox cmbRoles;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusGlobal;
        private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.ToolStripButton btntsbRefresh;
        private System.Windows.Forms.ToolStripStatusLabel lblServerStatus;
        public System.Windows.Forms.ToolStripStatusLabel lblServerInfo;
        private KryptonContextMenuItems kryptonContextMenuItems1;
        private KryptonContextMenuItem kryptonContextMenuItem1;
        private KryptonPanel kryptonPanelBigg;
        private KryptonSeparator kryptonSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtnloginFileServer;
        private System.Windows.Forms.ToolStripButton tsbtnSysTest;
        public System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStrip toolStripFunctionMenu;
        private System.Windows.Forms.ToolStripContainer toolStripContainerMenu;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripComboBox toolStripMenuSearcher;
        private System.Windows.Forms.ToolStripMenuItem 系统工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 服务缓存测试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblVer;
    }
}

