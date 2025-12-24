using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using FluentValidation.Results;
using RUINORERP.UI.UControls;
using SqlSugar;
using System;
using Autofac;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbType = SqlSugar.DbType;
using RUINORERP.Model;
using System.Threading;
using RUINORERP.UI.Common;
using System.Runtime.Remoting.Messaging;
using RUINORERP.UI.BI;
using RUINORERP.UI.Network;
using System.Reflection;
using RUINORERP.UI.UserCenter;
using RUINORERP.Business;
using RUINORERP.Business.Config;
using RUINORERP.UI.IM;

using System.Net;

using Microsoft.Extensions.Hosting;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.SnowflakeIdHelper;
using RUINORERP.UI.Log;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using System.Globalization;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using System.Collections;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using System.Diagnostics;
using RUINORERP.Business.Security;

using RUINORERP.Common.Extensions;
using Castle.Core.Smtp;
using System.IO;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using RUINORERP.UI.ToolForm;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using System.Linq.Expressions;
using Google.Protobuf.Collections;
using ExCSS;
using RUINORERP.Model.Models;
using RUINORERP.UI.SysConfig;
using RUINORERP.Common.Helper;
using System.Windows.Input;
using SourceLibrary.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Mysqlx;
using RUINORERP.Model.CommonModel;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Mysqlx.Prepare;
using FastReport.DevComponents.DotNetBar;
using FastReport.Table;
using System.Xml;
using RUINORERP.Model.TransModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;

using RUINORERP.Global;
using RUINORERP.PacketSpec.Commands;
using HLH.Lib.Security;
using System.Xml.Linq;
using AutoMapper;
using Netron.GraphLib.IO.NML;
using RUINORERP.Global.EnumExt;

using SourceGrid;
using log4net;

using RUINORERP.UI.Monitoring.Auditing;
using Match = System.Text.RegularExpressions.Match;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using HLH.Lib.Helper;
using RUINORERP.UI.BusinessService.SmartMenuService;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Plugin;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.Business.CommService;

using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.UI.Network.Services;
using System.ComponentModel;
using Padding = System.Windows.Forms.Padding;
using System.Windows.Forms;


namespace RUINORERP.UI
{
    public partial class MainForm : KryptonForm
    {
        /// <summary>
        /// 登录状态枚举
        /// </summary>
        public enum LoginStatus
        {
            /// <summary>
            /// 未登录
            /// </summary>
            None,
            /// <summary>
            /// 登录中
            /// </summary>
            LoggingIn,
            /// <summary>
            /// 已登录
            /// </summary>
            LoggedIn,
            /// <summary>
            /// 锁定状态
            /// </summary>
            Locked,
            /// <summary>
            /// 登出中
            /// </summary>
            LoggingOut
        }

        /// <summary>
        /// 系统更新方法（重载）
        /// </summary>
        /// <param name="ShowMessageBox">是否显示消息框</param>
        /// <returns>更新操作是否成功</returns>
        public async Task<bool> UpdateSys(bool ShowMessageBox)
        {
            // 调用带默认参数的方法
            return await UpdateSys(ShowMessageBox, false);
        }

        public UILogManager logManager;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ITableSchemaManager _tableSchemaManager;
        private readonly MessageService _messageService;
        /// <summary>
        /// 菜单跟踪器菜单推荐器
        /// </summary>
        private readonly MenuTracker _menuTracker;
        //IOptions<T> 提供对配置设置的单例访问。它在整个应用程序生命周期中保持相同的实例，这意味着即使在配置文件更改后，通过 IOptions<T> 获取的值也不会改变
        //。

        //IOptionsMonitor<T> 是一个单例服务，但它可以监听配置文件的更改并自动更新其值。当文件发生更改时，它会自动重新加载配置，使得下一次访问 CurrentValue 属性时能够获取到最新的配置值。这种机制使得 IOptionsMonitor<T> 适用于那些需要实时反映配置更改的场景
        //。

        //IOptionsSnapshot<T> 的生命周期是作用域（Scoped），这意味着对于每一次HTTP请求，都会提供一个新的实例。如果在请求过程中配置文件发生了更改，这个实例仍然保持旧的值，直到新的请求到达，才会获取到新的配置值。因此，IOptionsSnapshot<T> 适合用在那些需要每个请求都使用最新配置快照的场景

        /// <summary>
        /// 配置管理器
        /// </summary>
        private UIConfigManager _configManager;
        private readonly EnhancedMessageManager _messageManager;

        /// <summary>
        /// 系统锁定状态标志
        /// </summary>
        public bool IsLocked { get; private set; }

        #region 当前系统中所有用户信息
        private List<UserInfo> userInfos = new List<UserInfo>();



        /// <summary>
        /// 当前系统所有用户信息列表
        /// </summary>
        public List<UserInfo> UserInfos { get => userInfos; set => userInfos = value; }



        /// <summary>
        /// 处理重连失败事件，自动进入注销锁定状态
        /// </summary>
        private void OnReconnectFailed()
        {
            try
            {
                logger?.LogWarning("客户端重连失败，检查当前登录状态");

                // 只有在已登录状态下才进入锁定状态，避免登录失败后重复弹出登录窗口
                if (CurrentLoginStatus == LoginStatus.LoggedIn)
                {
                    logger?.LogWarning("当前为已登录状态，自动进入注销锁定状态");

                    // 在UI线程上执行注销操作
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            // 更新锁定状态显示
                            UpdateLockStatus(true);
                            LogLock();
                        }));
                    }
                    else
                    {
                        // 更新锁定状态显示
                        UpdateLockStatus(true);
                        LogLock();
                    }
                }
                else
                {

                    // 如果当前不是登录中状态且已连接，则断开连接
                    if (CurrentLoginStatus != LoginStatus.LoggingIn && communicationService != null && communicationService.IsConnected)
                    {
                        Invoke(new Action(async () =>
                        {
                            var disconnectResult = await communicationService.Disconnect();
                            logger?.LogInformation($"重连失败处理中断开连接结果: {disconnectResult}");
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理重连失败事件时发生异常");
            }
        }

        /// <summary>
        /// 处理心跳失败达到阈值事件
        /// 当连续心跳失败次数达到阈值且客户端尚未锁定时，触发客户端锁定
        /// </summary>
        private void OnHeartbeatFailureThresholdReached()
        {
            try
            {
                logger?.LogWarning("心跳失败达到阈值，检查当前登录和锁定状态");

                // 只有在已登录且未锁定状态下才进入锁定状态
                if (CurrentLoginStatus == LoginStatus.LoggedIn && !IsLocked)
                {
                    logger?.LogWarning("当前为已登录且未锁定状态，自动进入注销锁定状态");

                    // 在UI线程上执行注销操作
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            // 更新锁定状态显示
                            UpdateLockStatus(true);
                            LogLock();
                        }));
                    }
                    else
                    {
                        // 更新锁定状态显示
                        UpdateLockStatus(true);
                        LogLock();
                    }
                }
                else
                {
                    logger?.LogInformation($"当前状态不符合心跳锁定条件，登录状态: {CurrentLoginStatus}, 锁定状态: {IsLocked}");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理心跳失败阈值事件时发生异常");
            }
        }

        #endregion

        // 在表单关闭时取消订阅事件，避免内存泄漏
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);


        }







        /// <summary>
        /// 这个用来缓存，录入表单时的详情产品数据。后面看优化为一个全局缓存。
        /// </summary>
        public List<View_ProdDetail> View_ProdDetailList = new List<View_ProdDetail>();

        // 消息相关的集合和锁已移至MessageManager类中管理
        // 移除了内部ReminderData类，使用RUINORERP.Model.TransModel.ReminderData

        ///// <summary>
        ///// 用于连接上服务器后。保存与服务器连接的id
        ///   逻辑已经 在这里有了 EasyClientService
        ///// </summary>
        //public string SessionID { get; set; }
        /// <summary>
        /// 保存了所有工作流的队列，包括通知，审批相关
        /// </summary>
        public ConcurrentDictionary<string, string> _workflowItemlist = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
        /// </summary>
        public ConcurrentDictionary<string, string> WorkflowItemlist { get; set; }

        private static MainForm _main;
        public static MainForm Instance
        {
            get { return _main; }
        }
        public ApplicationContext AppContext { set; get; }
        public ILogger<MainForm> logger { get; set; }
        public string Version { get => version; set => version = value; }

        public readonly AuditLogHelper auditLogHelper;
        public AuditLogHelper AuditLogHelper => auditLogHelper;

        public readonly FMAuditLogHelper fmauditLogHelper;
        public FMAuditLogHelper FMAuditLogHelper => fmauditLogHelper;

        private System.Threading.Timer _autoSaveTimer;
        private System.Threading.Timer _clientVersionUpdateTimer;

        /// <summary>


        /// <summary>
        /// 注销状态标记，用于防止注销操作被重复执行
        /// </summary>
        private bool _isLoggingOut = false;

        /// <summary>
        /// 用于注销状态同步的锁对象
        /// </summary>
        private readonly object _logoutLock = new object();

        /// <summary>
        /// 获取或设置注销状态，确保线程安全
        /// </summary>
        public bool IsLoggingOut
        {
            get
            {
                lock (_logoutLock)
                {
                    return _isLoggingOut;
                }
            }
            set
            {
                lock (_logoutLock)
                {
                    _isLoggingOut = value;
                }
            }
        }

        /// <summary>
        /// 登录状态字段
        /// </summary>
        private LoginStatus _loginStatus = LoginStatus.None;

        /// <summary>
        /// 登录状态同步锁对象
        /// </summary>
        private readonly object _loginStatusLock = new object();

        /// <summary>
        /// 获取或设置登录状态，确保线程安全
        /// </summary>
        public LoginStatus CurrentLoginStatus
        {
            get
            {
                lock (_loginStatusLock)
                {
                    return _loginStatus;
                }
            }
            set
            {
                lock (_loginStatusLock)
                {
                    _loginStatus = value;
                }

                // 更新状态栏显示
                UpdateStatusBarDisplay();
            }
        }

        /// <summary>
        /// 更新状态栏显示
        /// </summary>
        private void UpdateStatusBarDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatusBarDisplay));
                return;
            }

            switch (CurrentLoginStatus)
            {
                case LoginStatus.LoggedIn:
                    if (AppContext.CurUserInfo != null)
                    {
                        if (AppContext.CurUserInfo.UserInfo != null && AppContext.CurUserInfo.UserInfo.tb_employee != null)
                        {
                            this.SystemOperatorState.Text = $"已登录: {AppContext.CurUserInfo.UserInfo.UserName}-{AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name}【{AppContext.CurrentRole.RoleName}】";
                        }
                        else
                        {
                            this.SystemOperatorState.Text = $"已登录: {AppContext.CurUserInfo.UserInfo.UserName}【{AppContext.CurrentRole.RoleName}】";
                        }
                    }
                    break;
                case LoginStatus.LoggingIn:
                    this.SystemOperatorState.Text = "登录中...";
                    break;
                case LoginStatus.Locked:
                    this.SystemOperatorState.Text = "锁定";
                    break;
                case LoginStatus.LoggingOut:
                    this.SystemOperatorState.Text = "登出中...";
                    break;
                case LoginStatus.None:
                default:
                    this.SystemOperatorState.Text = "未登录";
                    break;
            }
        }

        public ClientCommunicationService communicationService;


        public MainForm(ILogger<MainForm> _logger, AuditLogHelper _auditLogHelper,
            FMAuditLogHelper _fmauditLogHelper, EnhancedMessageManager messageManager)
        {
            InitializeComponent();

            // 通过依赖注入获取缓存管理器
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();

            _messageService = Startup.GetFromFac<MessageService>();

            lblStatusGlobal.Text = string.Empty;
            auditLogHelper = _auditLogHelper;
            fmauditLogHelper = _fmauditLogHelper;
            logger = _logger;
            _main = this;
            // 初始化消息管理器
            _messageManager = messageManager;
            // 订阅消息状态变更事件，用于更新UI显示
            _messageManager.MessageStatusChanged += OnMessageStatusChanged;

            #region 新的客户端通讯模块的调用
            // 通过依赖注入获取核心组件
            communicationService = Startup.ServiceProvider.GetService<ClientCommunicationService>();


            // 订阅重连失败事件，当重连失败时自动进入注销锁定状态
            if (communicationService != null)
            {
                communicationService.ReconnectFailed += OnReconnectFailed;
                // 订阅心跳失败阈值事件，当连续心跳失败达到阈值时触发锁定
                communicationService.HeartbeatFailureThresholdReached += OnHeartbeatFailureThresholdReached;
            }
            #endregion


            // 移除禁用跨线程检查的代码，这是不安全的做法
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            kryptonDockingManager1.DefaultCloseRequest = DockingCloseRequest.RemovePageAndDispose;
            kryptonDockableWorkspace1.ShowMaximizeButton = false;
            kryptonDockableWorkspace1.WorkspaceCellRemoved += KryptonDockableWorkspace1_WorkspaceCellRemoved;
            kryptonDockableWorkspace1.ControlRemoved += KryptonDockableWorkspace1_ControlRemoved;
            kryptonDockableWorkspace1.PageCloseClicked += KryptonDockableWorkspace1_PageCloseClicked;






            AppContext = Program.AppContextData;
            SourceGrid.Cells.Views.Cell viewGreen = new SourceGrid.Cells.Views.Cell();
            // 初始化日志管理器
            logManager = new UILogManager(this, uclog.grid, viewGreen);


            _menuTracker = Startup.GetFromFac<MenuTracker>();

            // 设置5分钟自动保存定时器
            _autoSaveTimer = new System.Threading.Timer(_ =>
            {
                _menuTracker.AutoSave();
            }, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

            // 设置1分钟客户端版本信息更新定时器
            _clientVersionUpdateTimer = new System.Threading.Timer(_ =>
            {
                UpdateCurrentUserModuleAndForm();
            }, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));



        }

        TabCloseHandler tabCloseHandler = new TabCloseHandler();
        /// <summary>
        /// 关闭事件  由 page的最右边x来决定的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KryptonDockableWorkspace1_PageCloseClicked(object sender, UniqueNameEventArgs e)
        {
            if (e.UniqueName == "控制中心" || e.UniqueName == "工作台")
            {

                return;
            }
            if (kryptonDockableWorkspace1.ActivePage != null && kryptonDockableWorkspace1.ActivePage.UniqueName == e.UniqueName)
            {
                var control = kryptonDockableWorkspace1.ActivePage.Controls[0];
                tabCloseHandler.ProcessControlOnClose(control);
                return;


            }

        }

        private void KryptonDockableWorkspace1_ControlRemoved(object sender, ControlEventArgs e)
        {

        }

        private void KryptonDockableWorkspace1_WorkspaceCellRemoved(object sender, WorkspaceCellEventArgs e)
        {

        }


        /// <summary>
        /// 更新当前用户信息中的当前模块和当前窗体
        /// </summary>
        public void UpdateCurrentUserModuleAndForm()
        {
            try
            {
                if (AppContext?.CurrentUser != null)
                {
                    if (kryptonDockableWorkspace1?.ActivePage != null)
                    {
                        AppContext.CurrentUser.当前模块 = "主界面";
                        AppContext.CurrentUser.当前窗体 = kryptonDockableWorkspace1.ActivePage.Text;
                    }
                    else
                    {
                        AppContext.CurrentUser.当前模块 = "主界面";
                        AppContext.CurrentUser.当前窗体 = "工作台";
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "更新当前用户模块和窗体信息失败");
            }
        }


        /// <summary>
        /// 初始化工具栏 刷新工具栏
        /// </summary>
        private void RefreshToolbar()
        {
            //加载前先保存
            // await _menuTracker.SaveToDb();

            _menuTracker.LoadFromDb();

            //目前就一个旧菜单。先保存一下再清除。加到最后
            toolStripFunctionMenu.Items.Clear();


            var SearcherList = MenuList.Where(c => c.MenuType == "行为菜单").OrderBy(c => c.CaptionCN).ToList();

            //常用菜单搜索按钮 添加到20个
            // 添加Top20菜单
            foreach (var menuId in _menuTracker.GetTopMenus())
            {
                var menuInfo = SearcherList.FirstOrDefault(m => m.MenuID == menuId);
                if (menuInfo == null) continue;

                var btn = new ToolStripButton(menuInfo.MenuName)
                {
                    Tag = menuInfo,
                    //Image = Image.FromStream(Common.DataBindingHelper.GetResource("toolbarmenulist"))
                };


                btn.Click += (s, e) =>
                {
                    if (s is ToolStripButton item && item.Tag is tb_MenuInfo menuInfo)
                    {
                        //按钮事件
                        try
                        {
                            menuPowerHelper.ExecuteEvents(menuInfo, null);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                };

                toolStripFunctionMenu.Items.Add(btn);

            }

            //最后添加那个菜单搜索的
            toolStripFunctionMenu.Items.Add(toolStripMenuSearcher);
        }






        private byte[] _byteArray;



        private void buttonSpecNavigator1_Click(object sender, EventArgs e)
        {
            // Are we currently showing fully expanded?
            if (kryptonNavigator1.NavigatorMode == NavigatorMode.OutlookFull)
            {
                // Switch to mini mode and reverse direction of arrow
                kryptonNavigator1.NavigatorMode = NavigatorMode.OutlookMini;
                buttonSpecNavigator1.TypeRestricted = PaletteNavButtonSpecStyle.ArrowRight;
            }
            else
            {
                // Switch to full mode and reverse direction of arrow
                kryptonNavigator1.NavigatorMode = NavigatorMode.OutlookFull;
                buttonSpecNavigator1.TypeRestricted = PaletteNavButtonSpecStyle.ArrowLeft;
            }
        }
        string UpdatefilePath = "UpdateLog.txt";


        /// <summary>
        /// 系统更新方法
        /// </summary>
        /// <param name="ShowMessageBox">是否显示消息框</param>
        /// <param name="forceUpdate">是否强制更新，忽略客户端自动更新配置</param>
        /// <summary>
        /// 检查和执行系统更新
        /// 支持跳过版本功能
        /// </summary>
        /// <param name="ShowMessageBox">是否显示消息框</param>
        /// <param name="forceUpdate">是否强制更新，忽略跳过版本设置</param>
        /// <returns>更新操作是否成功</returns>
        public async Task<bool> UpdateSys(bool ShowMessageBox, bool forceUpdate = false)
        {
            bool rs = false;
            var validatorMonitor = Startup.GetFromFac<SystemGlobalConfig>();

            // 如果未配置自动更新且非强制更新，则不执行更新检查
            if (!validatorMonitor.客户端自动更新 && !forceUpdate)
            {
                if (ShowMessageBox)
                {
                    MessageBox.Show("系统自动更新功能已关闭，请在系统全局设置中开启此功能。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            // 配置为开启自动更新或强制更新的情况，继续执行更新检查
            try
            {
                AutoUpdate.FrmUpdate Update = new AutoUpdate.FrmUpdate();

                // 检查是否有更新
                bool hasUpdates = Update.CheckHasUpdates();

                // 检查是否需要跳过版本（仅在非强制更新且有更新的情况下）
                bool shouldSkipVersion = !forceUpdate && hasUpdates;

                // 检查跳过版本的标记文件
                if (shouldSkipVersion)
                {
                    string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RUINORERP");
                    string skipVersionFilePath = Path.Combine(appDataPath, "SkippedVersions.xml");
                    string skipCurrentVersionFilePath = Path.Combine(Application.StartupPath, "skipcurrentversion.txt");

                    // 如果存在跳过版本的标记文件，询问用户是否仍要检查更新
                    if (File.Exists(skipVersionFilePath) || File.Exists(skipCurrentVersionFilePath))
                    {
                        DialogResult result = MessageBox.Show(
                            "检测到您已跳过当前版本的更新。\n是否仍要检查并安装最新版本？",
                            "更新提示",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            if (ShowMessageBox)
                            {
                                MessageBox.Show("更新已跳过，您可以稍后在系统设置中手动检查更新。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return false;
                        }
                    }
                }

                if (hasUpdates)
                {
                    var dialogResult = MessageBox.Show("服务器有新版本，更新前请保存当前操作，关闭系统。\r\n确定更新吗？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                    if (dialogResult == DialogResult.OK)
                    {
                        Process.Start(Update.currentexeName);
                        // 登录成功，重置注销状态
                        IsLoggingOut = false;
                        rs = true;

                        // 等待2秒，确保更新程序启动
                        await Task.Delay(1500);
                    }
                    else
                    {
                        rs = false;
                    }
                }
                else
                {
                    if (ShowMessageBox)
                    {


                        // 检查是否有可回滚的版本
                        bool hasRollbackVersions = Update.CheckHasRollbackVersions();

                        if (hasRollbackVersions)
                        {
                            // 设置为回滚模式，强制显示回滚界面
                            Update.SetRollbackMode();

                            // 如果有可回滚的版本，显示更新窗体（会自动进入回滚模式）
                            Update.ShowDialog();
                        }
                        else
                        {
                            // 如果没有可回滚的版本，才显示提示信息
                            MessageBox.Show("已经是最新版本，不需要更新。", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    rs = false;
                }
                await Task.Delay(10); // 假设操作需要一段时间
                return rs;
            }
            catch
            {
                return rs;
            }

            return rs;
        }


        private void AddMDIChildWindow()
        {
            frmTest f = new frmTest();
            f.Text = "Child " + (2).ToString();
            //f.MdiParent = this;
            f.ShowDialog();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            //  LoginServerByEasyClient();
            // kryptonDockingManager1.AddFloatingWindow("Floating", new KryptonPage[] { NewInput() });
        }



        private KryptonPage NewLog()
        {
            uclog.Height = 30;
            KryptonPage pageLog = NewPage("系统日志", 2, uclog);

            // pageLog.ClearFlags(KryptonPageFlags.All);
            pageLog.ClearFlags(KryptonPageFlags.DockingAllowClose);
            pageLog.ClearFlags(KryptonPageFlags.DockingAllowFloating);//控制托出的单独窗体是否能关掉
            pageLog.Height = 30;
            return pageLog;
        }


        /// <summary>
        /// 获取消息管理器实例
        /// </summary>
        /// <returns>EnhancedMessageManager实例</returns>
        private EnhancedMessageManager GetMessageManager()
        {
            return _messageManager;
        }

        /// <summary>
        /// 处理消息状态变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="message">变更状态的消息</param>
        private void OnMessageStatusChanged(object sender, MessageData message)
        {
            if (message==null)
            {
                return;
            }
            // 在这里可以更新UI，例如状态栏的未读消息计数
            logger?.LogDebug($"消息状态已变更: {message.Id}, 已读: {message.IsRead}");
        }

        private KryptonPage NewIMList()
        {
            #region 消息中心
            // 获取MessageManager实例
            var messageManager = GetMessageManager();
            // 创建MessageListControl实例，直接在构造函数中传入messageManager参数
            MessageListControl messageListControl = new RUINORERP.UI.IM.MessageListControl(messageManager);
            // 创建消息中心tab页
            KryptonPage pageMessageCenter = NewPage("消息中心", 1, messageListControl);
            pageMessageCenter.TextTitle = "消息中心";
            pageMessageCenter.TextDescription = "系统消息和通知";
            pageMessageCenter.UniqueName = "消息中心";
            pageMessageCenter.AllowDrop = false;
            pageMessageCenter.ClearFlags(KryptonPageFlags.All);
            #endregion
            KryptonPage pageMsgList = NewPage("消息中心", 2, messageListControl);
            //pageMsg.ClearFlags(KryptonPageFlags.All);
            pageMsgList.ClearFlags(KryptonPageFlags.DockingAllowClose);
            pageMsgList.ClearFlags(KryptonPageFlags.DockingAllowFloating);//控制托出的单独窗体是否能关掉
            pageMsgList.Height = 30;
            return pageMsgList;
        }


        //private KryptonPage NewIMMsg()

        //    KryptonPage pageMsg = NewPage("通讯中心", 2, ucMsg);
        //    //pageMsg.ClearFlags(KryptonPageFlags.All);
        //    pageMsg.ClearFlags(KryptonPageFlags.DockingAllowClose);
        //    pageMsg.ClearFlags(KryptonPageFlags.DockingAllowFloating);//控制托出的单独窗体是否能关掉
        //    pageMsg.Height = 30;
        //    return pageMsg;
        //}
        private KryptonPage NewForm()
        {
            return NewPage("NewForm ", 1, new UserControl1());
        }
        private KryptonPage LocList()
        {
            //return NewPage("LocList ", 1, new UCLocList());

            return NewPage("tb_LocationTypeEdit", 1, Startup.GetFromFac<UCUnitEdit>());
        }

        public KryptonPage NewPage(string name, int image, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage();
            p.Text = name;
            p.TextTitle = name;
            p.Name = name;
            p.Height = 30;
            //p.TextDescription = name + _count.ToString();
            p.ImageSmall = imageListSmall.Images[image] as Bitmap;
            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.ClearFlags(KryptonPageFlags.DockingAllowClose);
            p.Controls.Add(content);
            //  _count++;
            return p;
        }

        private UClog _uclog;
        public UClog uclog
        {
            get
            {
                if (_uclog == null)
                {
                    // 使用延迟初始化，但最好是通过构造函数注入
                    _uclog = Startup.GetFromFac<UClog>();
                }
                return _uclog;
            }
        }
        // public IM.UCMessager ucMsg = new IM.UCMessager();


        private string version = string.Empty;



        /// <summary>
        /// 保存锁定信息集合
        /// </summary>
        //public ConcurrentDictionary<long, BillLockInfo> LockInfoList = new ConcurrentDictionary<long, BillLockInfo>();


#warning TODO: 这里需要完善具体逻辑，当前仅为占位
        // public LockManager lockManager = new LockManager();


        /// <summary>
        /// 转发单据审核锁定 https://www.cnblogs.com/fanfan-90/p/12151924.html
        /// 锁单
        /// </summary>
        public IMemoryCache CacheLockManager { get; set; }


        ///监控升级标记文件 
        FileSystemWatcher watcher = new FileSystemWatcher();

        private void InitUpdateSystemWatcher()
        {
            //UpdatefilePath = System.IO.Path.Combine(Application.ExecutablePath, UpdatefilePath);

            // 设置监控的路径
            watcher.Path = Path.GetDirectoryName(Application.ExecutablePath);
            if (watcher.Path == null)
            {
                System.Diagnostics.Debug.WriteLine("文件路径无效。");
                return;
            }

            // 设置监控的文件名
            watcher.Filter = Path.GetFileName(UpdatefilePath);
            if (watcher.Filter == null)
            {
                System.Diagnostics.Debug.WriteLine("文件名无效。");
                return;
            }

            // 设置监控的更改类型
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            // watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += (sender, e) =>
            {
                if (e.Name == UpdatefilePath && e.ChangeType == WatcherChangeTypes.Changed)
                {
                    System.Diagnostics.Debug.WriteLine($"文件已修改: {e.FullPath}");

                    // 尝试读取文件内容
                    string content = string.Empty;
                    bool isFileAccessed = false;

                    // 重试机制，最多重试5次
                    int retryCount = 0;
                    while (retryCount < 5 && !isFileAccessed)
                    {
                        try
                        {
                            // 尝试读取文件内容
                            content = File.ReadAllText(UpdatefilePath);
                            isFileAccessed = true;
                        }
                        catch (IOException ioEx)
                        {
                            // 文件被占用，等待2秒后重试
                            System.Diagnostics.Debug.WriteLine($"文件被占用，正在重试... ({retryCount + 1}/5)");
                            Thread.Sleep(2000);
                            retryCount++;
                        }
                        catch (Exception ex)
                        {
                            // 其他异常
                            System.Diagnostics.Debug.WriteLine($"读取文件时发生错误: {ex.Message}");
                            break;
                        }
                    }

                    if (isFileAccessed)
                    {
                        System.Diagnostics.Debug.WriteLine("文件内容：");
                        System.Diagnostics.Debug.WriteLine(content);

                        if (content == "取消升级")
                        {
                            // 处理取消升级逻辑
                        }
                        else if (content == "升级中" || content == "升级完成")
                        {
                            // 确保当前程序退出
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("文件读取失败，重试次数已用尽。");
                    }
                }
            };
            watcher.EnableRaisingEvents = true;

        }

        public (string Version, DateTime LastUpdateTime, string url) ParseXmlInfo(string xmlFilePath)
        {
            try
            {
                var doc = XDocument.Load(xmlFilePath);

                // 获取Application版本
                var version = doc.Descendants("Application")
                                .Elements("Version")
                                .FirstOrDefault()?.Value;

                // 获取最后更新时间
                var lastUpdate = doc.Descendants("LastUpdateTime")
                                    .FirstOrDefault()?.Value;

                // 获取最后更新时间
                var url = doc.Descendants("Url")
                                    .FirstOrDefault()?.Value;


                DateTime.TryParse(lastUpdate, out var lastUpdateTime);

                return (version, lastUpdateTime, url);
            }
            catch (Exception ex)
            {
                // 添加异常处理
                //MessageBox.Show($"解析XML失败: {ex.Message}");
                return (null, DateTime.MinValue, null);
            }
        }

        #region 客户端版本 更新 配置情况

        #endregion

        private async void MainForm_Load(object sender, EventArgs e)
        {

            #region 进度条
            progressBar.Visible = true;
            // 初始化 ProgressManager
            ProgressManager.Instance.Initialize(lblStatusGlobal, progressBar);
            #endregion



            InitUpdateSystemWatcher();

            //只是表示如何使用。暂时没有用到
            CacheLockManager = Startup.GetFromFac<IMemoryCache>();

            authorizeController = Startup.GetFromFac<AuthorizeController>();

            //cache.Set("test1", "test123");

            UIConfigManager configManager = Startup.GetFromFac<UIConfigManager>();
            await configManager.LoadConfigValues();


            // 使用CacheInitializationService 只加载要缓存的表结构。缓存从服务器取
            var cacheInitializationService = Startup.GetFromFac<EntityCacheInitializationService>();
            cacheInitializationService.InitializeAllTableSchemas();
            
            // 验证初始化是否成功
            var tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();
            if (tableSchemaManager != null && !tableSchemaManager.IsInitialized)
            {
                System.Diagnostics.Debug.WriteLine("警告：客户端表结构初始化可能未完成，当前表数量为0");
            }
            else if (tableSchemaManager != null)
            {
                System.Diagnostics.Debug.WriteLine($"客户端表结构初始化成功，共注册了 {tableSchemaManager.GetAllTableNames().Count} 个表");
            }
            this.Text = "企业数字化集成ERP v3.2" + "-" + Program.ERPVersion;
            //MessageBox.Show("登陆成功后，请要系统设置中添加公司基本资料。");
            using (StatusBusy busy = new StatusBusy("检测系统是否为最新版本 请稍候"))
            {
                //更新是不是可以异步？
                if (!UserGlobalConfig.Instance.IsSupperUser)
                {
                    await UpdateSys(false);
                }
            }

            //按账号恢复关之前的布局
            //kryptonDockableWorkspace1.LoadLayoutFromArray(_byteArray);
            try
            {
                //kryptonDockableWorkspace1.LoadLayoutFromFile("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading from File");
            }
            //
            ClearData();
            ClearUI();

            RUINORERP.Extensions.SqlsugarSetup.CheckEvent += SqlsugarSetup_CheckEvent;
            RUINORERP.Extensions.SqlsugarSetup.RemindEvent += SqlsugarSetup_RemindEvent;
            bool islogin = await Login();
            if (!islogin)
            {
                return;
            }
            else
            {
                await UIBizService.RequestCache(typeof(tb_RoleInfo), useBackground: true);
                await UIBizService.RequestCache(typeof(tb_ProductType), useBackground: true);
                await UIBizService.RequestCache(typeof(View_ProdDetail), useBackground: true);

                using (StatusBusy busy = new StatusBusy("系统正在【初始化】 请稍候"))
                {
                    tb_MenuInfoController<tb_MenuInfo> menuInfoController = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
                    List<tb_MenuInfo> menuList = await menuInfoController.QueryAsync();
                    //var rslist = _cacheManager.CacheEntityList.Get(nameof(tb_MenuInfo));
                    //if (rslist != null)
                    //{
                    MainForm.Instance.AppContext.UserMenuList = menuList;
                    #region 检查是否注册
                    ////没有返回Null，如果结果大于1条会抛出错误
                    tb_sys_RegistrationInfo registrationInfo = await Program.AppContextData.Db.CopyNew().Queryable<tb_sys_RegistrationInfo>().SingleAsync();
                    if (registrationInfo == null)
                    {
                        MessageBox.Show("系统未注册，请在服务器端先完成注册", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                    else
                    {
                        MainForm.Instance.AppContext.CanUsefunctionModules = new List<GlobalFunctionModule>();
                        //注册成功！！！
                        //注意这里并不是直接取字段值。因为这个值会放到加密串的。明码可能会修改
                        //功能模块可以显示到UI。但是保存到DB中是加密了的。取出来到时也要解密
                        if (!string.IsNullOrEmpty(registrationInfo.FunctionModule))
                        {
                            //解密
                            registrationInfo.FunctionModule = EncryptionHelper.AesDecryptByHashKey(registrationInfo.FunctionModule, "FunctionModule");
                            //将,号隔开的枚举名称字符串变成List<GlobalFunctionModule>
                            List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();
                            string[] enumNameArray = registrationInfo.FunctionModule.Split(',');
                            foreach (var item in enumNameArray)
                            {
                                MainForm.Instance.AppContext.CanUsefunctionModules.Add((GlobalFunctionModule)Enum.Parse(typeof(GlobalFunctionModule), item));
                            }
                        }
                    }

                    if (MainForm.Instance.AppContext.CanUsefunctionModules == null)
                    {
                        MainForm.Instance.AppContext.CanUsefunctionModules = new List<GlobalFunctionModule>();
                    }

                    #endregion
                    //这里做一个事件。缓存中的变化了。这里也变化一下。todo:
                    try
                    {
                        bool rs = mc.CheckMenuInitialized();
                        if (!rs)
                        {
                            //如果从来没有初始化过菜单，则执行
                            InitMenu();
                        }
                    }
                    catch (Exception ex)
                    {
                        // logger.Error(ex);
                        MainForm.Instance.uclog.AddLog(ex.Message + ex.StackTrace);
                    }

                    //设计关联列和目标列
                    View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
                    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                    .AndIF(true, w => w.CNName.Length > 0)
                    .ToExpression();//注意 这一句 不能少
                                    //list = await dc.BaseQueryByWhereAsync(exp);
                                    //list = MainForm.Instance.list;
                                    //   TryRequestCache(nameof(View_ProdDetail), typeof(View_ProdDetail));

                }
            }

            #region 注册缓存用的类型

            Initialize();

            #endregion
            timer1.Start();
            Stopwatch stopwatchLoadUI = Stopwatch.StartNew();

            // 直接在主线程中加载UI，避免线程间操作问题
            try
            {
                LoadUIMenus();
                LoadUIForIM_LogPages();
            }
            catch (Exception ex)
            {
                // 记录错误但不影响主流程
                MainForm.Instance.logger.LogError(ex, "UI加载过程中发生错误");
            }

            stopwatchLoadUI.Stop();
            MainForm.Instance.uclog.AddLog($"LoadUIPages 执行时间：{stopwatchLoadUI.ElapsedMilliseconds} 毫秒");
            kryptonDockableWorkspace1.ActivePageChanged += kryptonDockableWorkspace1_ActivePageChanged;
            GetActivePage(kryptonDockableWorkspace1);

            // 在应用程序启动代码中添加
            var initializationService = Startup.GetFromFac<IDefaultRowAuthPolicyInitializationService>();
            await initializationService.InitializeDefaultPoliciesAsync();

            // 更新当前用户信息中的当前模块和当前窗体
            UpdateCurrentUserModuleAndForm();





            if (AppContext.IsSuperUser)
            {
                tsbtnSysTest.Visible = true;
            }

            MainForm.Instance.kryptonDockingManager1.PageCloseRequest += KryptonDockingManager1_PageCloseRequest;

            MainForm.Instance.kryptonDockingManager1.DockspaceCellRemoved += KryptonDockingManager1_DockspaceCellRemoved;

            MainForm.Instance.kryptonDockingManager1.DockspaceRemoved += KryptonDockingManager1_DockspaceRemoved;

            // 异步请求缓存，不阻塞UI线程，使用后台异步模式避免超时问题
            _ = Task.Run(async () =>
            {
                try
                {
                    // 使用后台异步模式，不设置超时，让后台管理器处理
                    await UIBizService.RequestCache<tb_Currency>(false, 15000, CancellationToken.None, useBackground: true);
                }
                catch (Exception ex)
                {
                    // 异常处理，记录日志但不影响主流程
                    MainForm.Instance.logger.LogError(ex, "后台缓存请求失败，使用本地缓存数据");
                }
            });

            // 异步延迟3秒执行本位币别查询事件，不会阻止UI线程
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                #region 本位币别
                #region 查询对应的项目组

                // 使用Invoke确保UI操作在主线程中执行
                this.Invoke(new Action(() => PrintInfoLog("正在查询项目组...")));

                //todo 后面再优化为缓存级吧
                List<tb_ProjectGroup> projectGroups = new List<tb_ProjectGroup>();
                List<tb_ProjectGroupEmployees> groupEmployees = new List<tb_ProjectGroupEmployees>();
                groupEmployees = MainForm.Instance.AppContext.Db.CopyNew()
                .Queryable<tb_ProjectGroupEmployees>()
                .Includes(a => a.tb_projectgroup, b => b.tb_department)
                .Includes(c => c.tb_projectgroup, d => d.tb_ProjectGroupAccountMappers, e => e.tb_fm_account)
                .Where(c => c.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.Id).ToList();
                MainForm.Instance.AppContext.projectGroups = groupEmployees.Select(c => c.tb_projectgroup).ToList();
                #endregion

                #region  本位币别查询
                this.Invoke(new Action(() => PrintInfoLog("正在查询本位币别...")));

                List<tb_Currency> currencies = new List<tb_Currency>();
                currencies = _cacheManager.GetEntityList<tb_Currency>(nameof(tb_Currency));

                tb_Currency currency = currencies.Where(m => m.Is_BaseCurrency.HasValue && m.Is_BaseCurrency.Value == true).FirstOrDefault();
                if (currency != null)
                {
                    MainForm.Instance.AppContext.BaseCurrency = currency;
                }
                else
                {
                    MainForm.Instance.AppContext.BaseCurrency = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_Currency>()
                    .Where(c => c.Is_BaseCurrency.HasValue && c.Is_BaseCurrency.Value == true).Single();
                    if (MainForm.Instance.AppContext.BaseCurrency == null)
                    {
                        this.Invoke(new Action(() => MessageBox.Show("请在基础设置中配置本位币别。")));
                    }
                }

                this.Invoke(new Action(() => PrintInfoLog("本位币别查询完成。")));
                #endregion



                #endregion
            });

            //mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
            //
            mapper = AppContext.GetRequiredService<IMapper>();

            GetAutoUpdateConfig();


            // 异步延迟3秒执行本位币别查询事件，不会阻止UI线程
            _ = Task.Run(async () =>
            {
                await Task.Delay(10000);
                #region  正在查询账期的设置
                this.Invoke(new Action(() => PrintInfoLog("正在查询账期的设置")));

                List<tb_PaymentMethod> PaymentMethods = new List<tb_PaymentMethod>();
                PaymentMethods = _cacheManager.GetEntityList<tb_PaymentMethod>(nameof(tb_PaymentMethod));

                tb_PaymentMethod paymentMethod = PaymentMethods.Where(m => m.Paytype_Name == DefaultPaymentMethod.账期.ToString()).FirstOrDefault();
                if (paymentMethod != null)
                {
                    MainForm.Instance.AppContext.PaymentMethodOfPeriod = paymentMethod;
                }
                else
                {
                    MainForm.Instance.AppContext.PaymentMethodOfPeriod = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_PaymentMethod>()
                    .Where(c => c.Paytype_Name == DefaultPaymentMethod.账期.ToString()).Single();
                    if (MainForm.Instance.AppContext.BaseCurrency == null)
                    {
                        this.Invoke(new Action(() => MessageBox.Show("请在基础设置中的付款方式添加【账期】。")));
                    }
                }

                this.Invoke(new Action(() => PrintInfoLog("账期设置查询完成。")));
                #endregion
            });

        }

        #region 注册缓存用的类型


        public static void Initialize()
        {
            // 预注册常用类型
            TypeResolver.PreRegisterCommonTypes();

            // 注册当前程序集中的所有类型
            Assembly.GetExecutingAssembly().RegisterAllTypesFromAssembly();

            // 注册其他相关程序集
            var relatedAssemblies = new[]
            {
            "RUINORERP.PacketSpec",
            "RUINORERP.Model",
        };

            foreach (var assemblyName in relatedAssemblies)
            {
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    assembly.RegisterAllTypesFromAssembly();
                }
                catch
                {
                    // 忽略加载失败的程序集
                }
            }
        }

        #endregion




        private bool SqlsugarSetup_RemindEvent(SqlSugarException ex)
        {
            if (HandleUniqueConstraintException(ex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string ExtractDuplicateValue(string errorMsg)
        {
            // 匹配 "重复键值为 (XXX)" 中的 XXX
            Match match = Regex.Match(errorMsg, @"重复键值为 \((?<value>.*?)\)");
            return match.Success ? match.Groups["value"].Value : "";
        }
        private bool HandleUniqueConstraintException(SqlSugarException ex)
        {
            bool handled = false;
            string errorMsg = ex.Message.ToLower();
            // 判断是否为唯一约束冲突（中文/英文消息兼容）
            if (errorMsg.Contains("unique key") || errorMsg.Contains("重复键"))
            {
                try
                {
                    // 提取重复的订单编号
                    string uniquekey = Regex.Match(errorMsg, @"\((.*?)\)").Groups[1].Value;
                    string value = ExtractDuplicateValue(ex.Message);

                    // 尝试从异常消息中提取表名
                    string tableName = string.Empty;
                    Match tableMatch = Regex.Match(ex.Message, @"object '(.*?)'");
                    if (tableMatch.Success)
                    {
                        tableName = tableMatch.Groups[1].Value;
                        // 如果表名包含.dbo.，提取后面的部分
                        if (tableName.Contains(".dbo."))
                        {
                            tableName = tableName.Split(new string[] { ".dbo." }, StringSplitOptions.None)[1];
                        }
                    }

                    string tableDescription = string.Empty;
                    // 尝试通过IEntityInfoService获取表的中文描述
                    try
                    {
                        var entityInfoService = Startup.GetFromFac<RUINORERP.Business.BizMapperService.IEntityMappingService>();
                        if (entityInfoService != null && !string.IsNullOrEmpty(tableName))
                        {
                            var entityInfo = entityInfoService.GetEntityInfoByTableName(tableName);
                            if (entityInfo != null && !string.IsNullOrEmpty(entityInfo.TableDescription))
                            {
                                tableDescription = entityInfo.TableDescription;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // 如果获取实体信息失败，继续使用原有的错误提示
                    }

                    // 根据是否获取到表描述，显示不同的错误信息
                    string message = string.IsNullOrEmpty(tableDescription)
                        ? $"【{value}】已存在，请检查后重试！"
                        : $"{tableDescription}中【{value}】已存在，请检查后重试！";

                    MessageBox.Show(
                        message,
                        "唯一性错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                catch (Exception)
                {
                    // 如果处理过程中出现异常，显示默认错误提示
                    string value = ExtractDuplicateValue(ex.Message);
                    MessageBox.Show(
                        $"【{value}】已存在，请检查后重试！",
                        "唯一性错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                handled = true;
            }
            return handled;
        }


        public IMapper mapper { get; set; }



        private void KryptonDockingManager1_DockspaceRemoved(object sender, DockspaceEventArgs e)
        {

        }

        private void KryptonDockingManager1_DockspaceCellRemoved(object sender, DockspaceCellEventArgs e)
        {

        }

        private void KryptonDockingManager1_PageCloseRequest(object sender, CloseRequestEventArgs e)
        {

        }


        public AuthorizeController authorizeController;


        private void kryptonDockableWorkspace1_ActivePageChanged(object sender, ActivePageChangedEventArgs e)
        {
            GetActivePage(sender);
            // 更新当前用户信息中的当前模块和当前窗体
            UpdateCurrentUserModuleAndForm();
        }

        private void GetActivePage(object sender)
        {
            KryptonDockableWorkspace _kryptonDockableWorkspace = sender as KryptonDockableWorkspace;
            KryptonPage kp = _kryptonDockableWorkspace.ActivePage;
            if (kp != null)
            {
                AppContext.log.ModName = kp.TextTitle;
                if (_kryptonDockableWorkspace.ActiveControl != null)
                {
                    AppContext.log.Path = _kryptonDockableWorkspace.ActiveControl.ToString();
                }

                // 更新当前用户信息中的当前模块和当前窗体
                if (AppContext?.CurrentUser != null)
                {
                    AppContext.CurrentUser.当前模块 = "主界面";
                    AppContext.CurrentUser.当前窗体 = kp.TextTitle;
                }
            }
            else
            {
                // 如果没有活动页面，设置默认值
                if (AppContext?.CurrentUser != null)
                {
                    AppContext.CurrentUser.当前模块 = "主界面";
                    AppContext.CurrentUser.当前窗体 = "工作台";
                }
            }
        }




        // 锁定状态标签
        private ToolStripStatusLabel _lockStatusLabel;
        // 当前锁定状态
        private bool _isLocked = false;

        /// <summary>
        /// 更新锁定状态显示
        /// </summary>
        /// <param name="isLocked">是否锁定</param>
        public void UpdateLockStatus(bool isLocked)
        {
            _isLocked = isLocked;
            if (_lockStatusLabel != null && this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    _lockStatusLabel.Text = isLocked ? "状态: 锁定" : "状态: 正常";
                    _lockStatusLabel.ForeColor = isLocked ? Color.Red : Color.Green;
                }));
            }
            else if (_lockStatusLabel != null)
            {
                _lockStatusLabel.Text = isLocked ? "状态: 锁定" : "状态: 正常";
                _lockStatusLabel.ForeColor = isLocked ? Color.Red : Color.Green;
            }
        }

        private void LoadStatus()
        {
            // 创建StatusStrip控件
            StatusStrip statusStrip = new StatusStrip();
            statusStrip.Dock = DockStyle.Bottom;
            // 添加账号信息
            ToolStripStatusLabel accountLabel = new ToolStripStatusLabel();
            accountLabel.Text = "账号: user1";
            statusStrip.Items.Add(accountLabel);
            // 添加角色信息
            ToolStripStatusLabel roleLabel = new ToolStripStatusLabel();
            roleLabel.Text = "角色: 管理员";
            statusStrip.Items.Add(roleLabel);
            // 添加操作位置信息
            ToolStripStatusLabel locationLabel = new ToolStripStatusLabel();
            locationLabel.Text = "操作位置: 主界面";
            statusStrip.Items.Add(locationLabel);
            // 添加锁定状态信息
            _lockStatusLabel = new ToolStripStatusLabel();
            _lockStatusLabel.Text = "状态: 正常";
            _lockStatusLabel.ForeColor = Color.Green;
            statusStrip.Items.Add(_lockStatusLabel);
            // 添加进度条
            ToolStripProgressBar progressBar = new ToolStripProgressBar();
            progressBar.Value = 50;
            statusStrip.Items.Add(progressBar);
            // 添加版本信息
            ToolStripStatusLabel versionLabel = new ToolStripStatusLabel();
            versionLabel.Text = "版本: v1.0";
            statusStrip.Items.Add(versionLabel);
            // 将StatusStrip控件添加到窗口的Controls集合中
            this.Controls.Add(statusStrip);
        }


        private void LoadUIForIM_LogPages()
        {
            using (StatusBusy busy = new StatusBusy("系统准备中.... 请稍候"))
            {
                if (kryptonDockingManager1.Pages.Count() == 0 && kryptonDockingManager1.Cells.Length == 0)
                {
                    KryptonDockingWorkspace w = kryptonDockingManager1.ManageWorkspace(kryptonDockableWorkspace1);
                    kryptonDockingManager1.ManageControl(kryptonPanelBigg, w);
                    kryptonDockingManager1.ManageFloating(this);
                }

                // Add initial docking pages
                //kryptonDockingManager1.AddToWorkspace("Workspace", new KryptonPage[] { NewDocument(), NewDocument() });
                // kryptonDockingManager1.AddDockspace("Control", DockingEdge.Right, new KryptonPage[] { NewPropertyGrid(), NewInput(), NewPropertyGrid(), NewInput() });


                // KryptonPage[] myppages = new KryptonPage[] { NewLog(), NewIMMsg() };
                KryptonPage[] myppages = new KryptonPage[] { NewLog() };
                if (kryptonDockingManager1.Pages.FirstOrDefault(p => p.Name == "系统日志") == null)
                {
                    kryptonDockingManager1.AddDockspace("Control", DockingEdge.Bottom, myppages);
                    kryptonDockingManager1.MakeAutoHiddenRequest(myppages[0].UniqueName);
                    //kryptonDockingManager1.MakeAutoHiddenRequest(myppages[1].UniqueName);   IMMMMMM
                }

                KryptonPage IMPage = NewIMList();
                IMPage.AllowDrop = false;
                IMPage.SetFlags(KryptonPageFlags.All);
                kryptonDockingManager1.AddDockspace("Control", DockingEdge.Right, new KryptonPage[] { IMPage });
                kryptonDockingManager1.MakeAutoHiddenRequest(IMPage.UniqueName);//默认加载时隐藏

                InitCenterPages();
                LoadDefaultSkinMenu();
            }
        }

        /// <summary>
        /// 加载控制中心页
        /// </summary>
        private void LoadUIMenus()
        {
            //如果没有初始始化menu
            using (StatusBusy busy = new StatusBusy("系统正在加载数据... 请稍候"))
            {
                //InitEditObjectValue();
                LoadMenuOfTop();
                LoadMenuPagesByLeft();
            }
        }

        #region  加载左边菜单
        private void LoadMenuPagesByLeft()
        {
            kryptonNavigator1.Pages.Clear();
            if (AppContext.CurUserInfo.Name == "超级管理员")
            {
                foreach (tb_ModuleDefinition item in AppContext.CurUserInfo.UserModList)
                {
                    // Create new page with title and image
                    KryptonPage p = new KryptonPage();
                    p.Text = item.ModuleName;
                    p.TextTitle = item.ModuleName;
                    p.Name = item.ModuleID.ToString();
                    p.TextDescription = item.ModuleName;// + item.BizType.ToString();
                                                        // p.ImageSmall = imageListSmall.Images[image];
                    KryptonTreeView TreeView1 = new KryptonTreeView();
                    TreeView1.MouseDoubleClick += TreeView1_DoubleClick;
                    TreeView1.NodeMouseDoubleClick += TreeView1_NodeMouseDoubleClick;

                    List<tb_MenuInfo> tempList = new List<tb_MenuInfo>();
                    foreach (tb_MenuInfo menuInfo in item.tb_MenuInfos)
                    {
                        //不重复,超级管理员。不用控制菜单启用可见
                        if (!tempList.Contains(menuInfo))
                        {
                            tempList.Add(menuInfo);
                        }

                    }
                    //如果是顶级菜单是和模块名相同，跳过
                    var modMenu = tempList.FirstOrDefault(c => c.Parent_id == 0 && c.MenuName == item.ModuleName);
                    if (modMenu != null)
                    {
                        var subMenus = tempList.Where(c => c.Parent_id != 0 && c.Parent_id == modMenu.MenuID).OrderBy(c => c.Sort);
                        foreach (tb_MenuInfo subMenu in subMenus)
                        {
                            // Add the control for display inside the page
                            TreeNode nodeRoot = new TreeNode();
                            nodeRoot.Text = subMenu.MenuName;
                            nodeRoot.Name = subMenu.MenuID.ToString();
                            nodeRoot.Tag = subMenu;

                            TreeView1.Nodes.Add(nodeRoot);
                            List<tb_MenuInfo> sortlist = tempList.OrderBy(t => t.Sort).ToList();
                            Bind(nodeRoot, sortlist, subMenu.MenuID);
                            TreeView1.HideSelection = false;
                            // TreeView1.Nodes.Clear();
                            TreeView1.Dock = DockStyle.Fill;
                            p.ClearFlags(KryptonPageFlags.DockingAllowClose);
                            //TreeView1.Nodes[0].Expand();
                            TreeView1.ExpandAll();
                            p.Controls.Add(TreeView1);

                        }
                    }
                    kryptonNavigator1.Pages.Add(p);
                }
            }
            else
            {
                //先指定添加顶级菜单 即模块
                foreach (tb_P4Menu ParentItem in AppContext.CurrentRole.tb_P4Menus.Where(c => c.tb_menuinfo.Parent_id == 0))
                {
                    if (!ParentItem.tb_menuinfo.IsVisble) continue;

                    // Create new page with title and image
                    KryptonPage p = new KryptonPage();
                    p.Text = ParentItem.tb_menuinfo.MenuName;
                    p.TextTitle = ParentItem.tb_menuinfo.MenuName;
                    p.Name = ParentItem.ModuleID.ToString();
                    p.TextDescription = ParentItem.tb_menuinfo.MenuName;// + item.BizType.ToString();
                                                                        // p.ImageSmall = imageListSmall.Images[image];

                    #region 添加子菜单
                    KryptonTreeView TreeView1 = new KryptonTreeView();
                    TreeView1.MouseDoubleClick += TreeView1_DoubleClick;

                    TreeView1.NodeMouseDoubleClick += TreeView1_NodeMouseDoubleClick;
                    //如果是顶级菜单是和模块名相同，跳过

                    List<tb_MenuInfo> tempList = new List<tb_MenuInfo>();
                    foreach (tb_P4Menu P4Menu in AppContext.CurrentRole.tb_P4Menus.Where(c => c.tb_menuinfo.MenuID != ParentItem.MenuID).Where(c => c.IsVisble).ToList())
                    {
                        //不重复
                        if (!tempList.Contains(P4Menu.tb_menuinfo) && P4Menu.tb_menuinfo.IsVisble)
                        {
                            tempList.Add(P4Menu.tb_menuinfo);
                        }
                    }
                    List<tb_MenuInfo> sortlist = tempList.OrderBy(t => t.Sort).ToList();
                    Bind(TreeView1, sortlist, ParentItem);
                    TreeView1.HideSelection = false;
                    // TreeView1.Nodes.Clear();
                    TreeView1.Dock = DockStyle.Fill;
                    p.ClearFlags(KryptonPageFlags.DockingAllowClose);
                    // TreeView1.Nodes[0].Expand();
                    TreeView1.ExpandAll();
                    p.Controls.Add(TreeView1);
                    #endregion
                    if (TreeView1.TreeView.Nodes.Count == 0)
                    {
                        continue;
                    }
                    kryptonNavigator1.Pages.Add(p);
                }
            }
        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Tag is tb_MenuInfo menuInfo)
                {
                    menuPowerHelper.ExecuteEvents(menuInfo, null);
                }
            }
        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private void TreeView1_DoubleClick(object sender, EventArgs e)
        {

            //menuPowerHelper.ExecuteEvents();
        }


        //递归方法
        private void Bind(KryptonTreeView tree, List<tb_MenuInfo> resourceList, tb_P4Menu ParentItem)
        {
            //            var childList = resourceList.Where(t => t.Parent_id == 0).OrderBy(m => m.CaptionCN).ThenBy(t => t.Sort);
            var childList = resourceList.Where(t => t.Parent_id == ParentItem.MenuID).OrderBy(m => m.Sort);
            foreach (tb_MenuInfo tb_menuinfo in childList)
            {
                TreeNode nodeRoot = new TreeNode();
                nodeRoot.Text = tb_menuinfo.MenuName;
                nodeRoot.Name = tb_menuinfo.MenuID.ToString();
                nodeRoot.Tag = tb_menuinfo;
                tree.Nodes.Add(nodeRoot);
                Bind(nodeRoot, resourceList, tb_menuinfo.MenuID);
            }
        }


        //递归方法
        private void Bind(TreeNode parNode, List<tb_MenuInfo> resourceList, long nodeId)
        {
            var childList = resourceList.Where(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.MenuID.ToString();
                node.Text = nodeObj.MenuName;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, resourceList, nodeObj.MenuID);
            }
        }

        #endregion
        private void SqlsugarSetup_CheckEvent(string sql)
        {
            if (AuthorizeController.GetShowDebugInfoAuthorization(AppContext))
            {

                logManager.AddLog("sql", sql);
            }
        }

        #region ApplyAuthorizationRules

        private void ApplyAuthorizationRules()
        {

            bool rs = true;// Csla.Rules.BusinessRules.HasPermission(Program.cslaAppContext, Csla.Rules.AuthorizationActions.CreateObject, typeof(Business.UseCsla.tb_LocationTypeEditBindingList));
        }

        #endregion

        #region Login/Logout


        private async Task<bool> Login()
        {
            // 检查是否已经在登录过程中或已登录，避免重复弹出登录窗口
            if (CurrentLoginStatus == LoginStatus.LoggingIn || CurrentLoginStatus == LoginStatus.LoggedIn)
            {
                logger?.LogWarning("当前已经在登录过程中或已登录，忽略重复登录请求");
                return CurrentLoginStatus == LoginStatus.LoggedIn;
            }

            // 设置登录状态为登录中
            CurrentLoginStatus = LoginStatus.LoggingIn;

            bool rs = false;
            RUINORERP.Business.Security.PTPrincipal.Logout(AppContext);

            try
            {
                FrmLogin loginForm = new FrmLogin();
                if (loginForm.ShowDialog(this) == DialogResult.OK)
                {
                    tb_SystemConfigController<tb_SystemConfig> ctr = Startup.GetFromFac<tb_SystemConfigController<tb_SystemConfig>>();
                    List<tb_SystemConfig> config = ctr.Query();
                    if (config.Count > 0)
                    {
                        AppContext.SysConfig = config[0];
                        try
                        {
                            AppContext.FMConfig = JsonConvert.DeserializeObject<FMConfiguration>(config[0].FMConfig);
                        }
                        catch (Exception)
                        {
                            logger.LogError("请对财务模块参数进行正确的配置。");
                        }
                    }
                    var ctrBillNoRule = Startup.GetFromFac<tb_sys_BillNoRuleController<tb_sys_BillNoRule>>();
                    List<tb_sys_BillNoRule> BillNoRules = ctrBillNoRule.Query();
                    AppContext.BillNoRules = BillNoRules;

                    // 登录成功，重置状态
                    IsLoggingOut = false;
                    CurrentLoginStatus = LoginStatus.LoggedIn;
                    rs = true;
                    if (AppContext.CurUserInfo != null)
                    {
                        if (AppContext.CurUserInfo.UserInfo != null && AppContext.CurUserInfo.UserInfo.tb_employee != null)
                        {
                            this.SystemOperatorState.Text = $"登陆: {AppContext.CurUserInfo.UserInfo.UserName}-{AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name}【{AppContext.CurrentRole.RoleName}】";
                        }
                        else
                        {
                            this.SystemOperatorState.Text = $"登陆: {AppContext.CurUserInfo.UserInfo.UserName}【{AppContext.CurrentRole.RoleName}】";
                        }
                    }

                    //加载角色
                    //toolStripDropDownBtnRoles.DropDownItems.Clear();
                    //超级管理员没有权限组
                    if (AppContext.Roles != null)
                    {
                        if (toolStripDropDownBtnRoles.DropDownItems[0] is ToolStripComboBox comboBoxRoles)
                        {
                            foreach (var item in AppContext.Roles)
                            {
                                comboBoxRoles.Items.Add(item.RoleName);
                            }
                            comboBoxRoles.SelectedItem = AppContext.Roles[0].RoleName;
                        }
                        this.cmbRoles.SelectedIndexChanged += new System.EventHandler(this.cmbRoles_SelectedIndexChanged);
                    }

                    //记入审计日志
                    //MainForm.Instance.AuditLogHelper.CreateAuditLog("登陆", $"{System.Environment.MachineName}-成功登陆服务器");
                    if (MainForm.Instance.AppContext.CurUserInfo != null && MainForm.Instance.AppContext.CurUserInfo.UserInfo != null)
                    {
                        try
                        {
                            MainForm.Instance.AppContext.CurUserInfo.UserInfo.Lastlogin_at = System.DateTime.Now;
                            await MainForm.Instance.AppContext.Db.CopyNew().Storageable<tb_UserInfo>(MainForm.Instance.AppContext.CurUserInfo.UserInfo).ExecuteReturnEntityAsync();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("更新最后登陆时间出错。", ex);
                        }

                    }

                    //登陆时已经获取了 登陆人所在部门对应的公司。特殊情况才会进入这个查询
                    if (MainForm.Instance.AppContext.CompanyInfo == null)
                    {
                        List<tb_Company> Companylist = await MainForm.Instance.AppContext.Db.Queryable<tb_Company>().ToListAsync();
                        if (Companylist.Count > 0)
                        {
                            MainForm.Instance.AppContext.CompanyInfo = Companylist[0];
                        }
                    }
                    if (AppContext.CompanyInfo != null)
                    {
                        this.Text = AppContext.CompanyInfo.ShortName + "企业数字化集成ERP v3.2" + "-" + Program.ERPVersion;
                    }

                    await UIBizService.RequestCache(nameof(tb_RoleInfo));

                    if (loginForm.IsInitPassword)
                    {
                        MessageBox.Show("初始密码【123456】有风险，请及时修改！\r\n修改路径：【系统设置】->【个性化设置】->【密码修改】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


#warning TODO: 这里需要完善具体逻辑，当前仅为占位

                    //ClientLockManagerCmd cmd = new ClientLockManagerCmd(CommandDirection.Send);
                    //cmd.lockCmd = LockCmd.Broadcast;
                    //MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);
                    //cmd.LockChanged += (sender, e) =>
                    //{
                    //    //使用事件模式来查询某一个单据被谁锁定
                    //};
                }
                else
                {
                    // 登录失败，设置状态为未登录
                    CurrentLoginStatus = LoginStatus.None;

                    // 检查是否与服务器连接，如果连接则断开
                    if (communicationService != null && communicationService.IsConnected)
                    {
                        var disconnectResult = await communicationService.Disconnect();
                        logger?.LogInformation($"登录过程中断开连接结果: {disconnectResult}");
                    }

                    Application.Exit();
                }
                UserGlobalConfig.Instance.Serialize();
                // reset menus, etc.
                ApplyAuthorizationRules();
                // notify all documents
                //加载uc
                return rs;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "登录过程中发生异常");
                CurrentLoginStatus = LoginStatus.None;

                // 异常情况下断开连接
                if (communicationService != null && communicationService.IsConnected)
                {
                    var disconnectResult = await communicationService.Disconnect();
                    logger?.LogInformation($"异常处理中断开连接结果: {disconnectResult}");
                }
                return false;
            }
        }


        private async Task Logout()
        {
            try
            {
                // 设置状态为登出中
                CurrentLoginStatus = LoginStatus.LoggingOut;

                MainForm.Instance.AuditLogHelper.CreateAuditLog("登出", "开始登出服务器");
                if (MainForm.Instance.AppContext.CurUserInfo != null && MainForm.Instance.AppContext.CurUserInfo.UserInfo != null)
                {
                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.Lastlogout_at = System.DateTime.Now;
                    var result = await MainForm.Instance.AppContext.Db.Updateable<tb_UserInfo>(MainForm.Instance.AppContext.CurUserInfo.UserInfo)
                    .UpdateColumns(it => new { it.Lastlogout_at }).ExecuteCommandAsync();
                }
                _menuTracker.SaveToDb();
                ClearUI();
                ClearData();
                Application.DoEvents();

                // 登出完成，断开与服务器的连接
                if (communicationService != null && communicationService.IsConnected)
                {
                    var disconnectResult = await communicationService.Disconnect();
                    logger?.LogInformation($"登出过程中断开连接结果: {disconnectResult}");
                }

                // 设置状态为未登录
                CurrentLoginStatus = LoginStatus.None;

                MainForm.Instance.AuditLogHelper.CreateAuditLog("登出", "成功登出服务器并断开连接");

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        /// <summary>
        /// 系统锁定，禁用所有用户操作，仅允许重新登录
        /// </summary>
        public void LogLock()
        {

            // 检查是否已经在注销过程中，防止重复执行
            if (IsLoggingOut || CurrentLoginStatus == LoginStatus.LoggingOut || CurrentLoginStatus == LoginStatus.LoggingIn)
            {
                logger?.LogWarning("锁定操作已在进行中或正在登录，忽略重复调用");
                return;
            }

            // 设置状态
            IsLoggingOut = true;
            CurrentLoginStatus = LoginStatus.Locked;
            IsLocked = true; // 设置锁定状态标志

            MainForm.Instance.Invoke(new Action(async () =>
            {
                try
                {
                    Program.AppContextData.IsOnline = false;
                    MainForm.Instance.AppContext.CurrentUser.授权状态 = false;
                    MainForm.Instance.AppContext.CurrentUser.在线状态 = false;

                    // 清除UI元素
                    ClearUI();
                    ClearRoles();

                    // 禁用所有UI控件
                    DisableAllUIComponents();

                    System.GC.Collect();

                    // 尝试重新登录
                    bool islogin = await Login();
                    if (!islogin)
                    {
                        // 登录失败时重置状态
                        IsLoggingOut = false;
                        CurrentLoginStatus = LoginStatus.None;
                        return;
                    }

                    // 登录成功后加载界面
                    LoadUIMenus();
                    LoadUIForIM_LogPages();

                    // 登录成功后重置状态
                    IsLoggingOut = false;
                    CurrentLoginStatus = LoginStatus.LoggedIn;
                    IsLocked = false; // 重置锁定状态
                    // 更新锁定状态为正常
                    UpdateLockStatus(false);
                    // 重新启用UI组件
                    ReenableUIComponents();
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "注销过程中发生异常");
                    // 异常情况下重置注销状态
                    IsLoggingOut = false;
                }
            }));
        }

        #endregion

        UCWorkbenches _UCWorkbenches = null;
        private void InitCenterPages()
        {

            // Setup docking functionality

            kryptonDockableWorkspace1.AllowDrop = false;
            kryptonDockableWorkspace1.AllowPageDrag = false;

            // Set correct initial ribbon palette buttons
            //UpdatePaletteButtons();

            var _UCInitControlCenter = Startup.GetFromFac<UCInitControlCenter>(); //获取服务Service1


            KryptonWorkspaceCell cell = kryptonDockableWorkspace1.ActiveCell;
            if (cell == null || kryptonDockableWorkspace1.PageCount == 0)
            {
                cell = new KryptonWorkspaceCell();
                kryptonDockableWorkspace1.Root.Children.Add(cell);
                //cell.Button.CloseButtonDisplay = ButtonDisplay.Logic;
                cell.ContextMenu = null;
                cell.ContextMenuStrip = null;
                cell.CloseAction += Cell_CloseAction;
                cell.SelectedPageChanged += Cell_SelectedPageChanged;
                cell.ShowContextMenu += Cell_ShowContextMenu;

                #region 创建初始页

                KryptonPage pageControlCenter = new KryptonPage();
                pageControlCenter.Text = "控制中心";
                pageControlCenter.TextTitle = "控制中心";
                pageControlCenter.TextDescription = "控制中心";
                pageControlCenter.UniqueName = "控制中心";
                //  p.ImageSmall = imageListSmall.Images[image];
                // Form2 frm = new Form2();
                // frm.Controls.Add(_UCInitControlCenter);
                // frm.Show();
                // Add the control for display inside the page
                _UCInitControlCenter.Dock = DockStyle.Fill;
                //_UCInitControlCenter.TopLevel = false;
                pageControlCenter.Controls.Add(_UCInitControlCenter);

                #endregion

                _UCWorkbenches = Startup.GetFromFac<UCWorkbenches>(); //获取服务Service1



                #region 工作台

                KryptonPage pageWorkbenches = new KryptonPage();
                pageWorkbenches.Text = "工作台";
                pageWorkbenches.TextTitle = "工作台";
                pageWorkbenches.TextDescription = "工作台";
                pageWorkbenches.UniqueName = "工作台";
                //  p.ImageSmall = imageListSmall.Images[image];
                // Form2 frm = new Form2();
                // frm.Controls.Add(_UCInitControlCenter);
                // frm.Show();
                // Add the control for display inside the page
                _UCWorkbenches.Dock = DockStyle.Fill;
                pageWorkbenches.Controls.Add(_UCWorkbenches);

                #endregion


                //KryptonPage page = UI.Common.ControlHelper.NewPage("菜单初始化", 1, _MenuInit);
                pageControlCenter.AllowDrop = false;
                pageControlCenter.ClearFlags(KryptonPageFlags.All);

                /*
                page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden |
                           KryptonPageFlags.DockingAllowDocked |
                           KryptonPageFlags.DockingAllowClose);*/
                cell.Pages.Add(pageControlCenter);

                pageWorkbenches.AllowDrop = false;
                pageWorkbenches.ClearFlags(KryptonPageFlags.All);
                cell.Pages.Add(pageWorkbenches);



                //pWorkbenches.AllowDrop = false;
                //pWorkbenches.ClearFlags(KryptonPageFlags.All);

                //cell.Pages.Add(pWorkbenches);
            }

            cell.SelectedPage = cell.Pages.Where(x => x.Text == "工作台").FirstOrDefault();
            kryptonDockableWorkspace1.ActivePage = kryptonDockableWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == "工作台");
        }

        private void Cell_ShowContextMenu(object sender, ShowContextMenuArgs e)
        {
            KryptonWorkspaceCell kwc = sender as KryptonWorkspaceCell;
            if (kwc.SelectedPage == null)
            {
                return;
            }
            if (kwc.SelectedPage.TextTitle == "控制中心"
                || kwc.SelectedPage.TextTitle == "工作台"

                )
            {
                //不显示右键
                e.Cancel = true;
            }
            //都不显示显示右键
            e.Cancel = true;
            return;

            //添加了自定义的，也同时还加载了默认的。暂时不使用了。
            //显示并自定义
            // Yes we want to show a context menu
            e.Cancel = false;


            // Provide the navigator specific menu
            e.KryptonContextMenu = new KryptonContextMenu();

            KryptonContextMenuItems customItems = new KryptonContextMenuItems();
            KryptonContextMenuSeparator customSeparator = new KryptonContextMenuSeparator();
            KryptonContextMenuItem customItem1 = new KryptonContextMenuItem("Custom Item 1", new EventHandler(OnCustomMenuItem));
            KryptonContextMenuItem customItem2 = new KryptonContextMenuItem("Custom Item 2", new EventHandler(OnCustomMenuItem));
            customItem1.Tag = kwc.SelectedPage;
            customItem2.Tag = kwc.SelectedPage;
            customItems.Items.AddRange(new KryptonContextMenuItemBase[] { customSeparator, customItem1, customItem2 });

            // Add set of custom items into the provided menu
            e.KryptonContextMenu.Items.Add(customItems);


        }

        private void OnCustomMenuItem(object sender, EventArgs e)
        {
            KryptonContextMenuItem menuItem = (KryptonContextMenuItem)sender;
            KryptonPage page = (KryptonPage)menuItem.Tag;
            MessageBox.Show("Clicked menu option '" + menuItem.Text + "' for the page '" + page.Text + "'.", "Page Context Menu");
        }

        private void Cell_SelectedPageChanged(object sender, EventArgs e)
        {

            KryptonWorkspaceCell kwc = sender as KryptonWorkspaceCell;
            if (kwc.SelectedPage == null)
            {
                return;
            }
            if (kwc.SelectedPage.TextTitle == "控制中心"
                || kwc.SelectedPage.TextTitle == "工作台"

                )
            {
                kwc.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            }
            else
            {
                kwc.Button.CloseButtonDisplay = ButtonDisplay.Logic;
            }

            //选到了第一个。或其他全关了
            //kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.ShowDisabled;
        }


        private void Cell_CloseAction(object sender, CloseActionEventArgs e)
        {
            //关闭事件
            e.Action = CloseButtonAction.RemovePageAndDispose;
        }


        private async Task InitCacheConfig(bool LoadData)
        {
            // _cacheManager.InitCacheDict(LoadData); // 已废弃，使用CacheInitializationService替代
            await Task.Delay(5);
        }

        /// <summary>
        /// 只执行一次,初始化菜单
        /// </summary>
        private async Task InitMenu()
        {
            //List<MenuAssemblyInfo> list = UIHelper.RegisterForm();
            //CreateMenu(list, 0, 0);
            Stopwatch stopwatch = Stopwatch.StartNew();
            InitModuleMenu init = Startup.GetFromFac<InitModuleMenu>();
            try
            {
                await init.InitModuleAndMenuAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            stopwatch.Stop();
            MainForm.Instance.logger.LogInformation($"初始化菜单执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
            MainForm.Instance.uclog.AddLog($"初始化菜单执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
        }


        private List<tb_MenuInfo> LoadTypes()
        {
            tb_MenuInfo menuInfoparent = new tb_MenuInfo();
            List<tb_MenuInfo> menuList = new List<tb_MenuInfo>();
            Type[]? types = System.Reflection.Assembly.GetExecutingAssembly()?.GetExportedTypes();
            if (types != null)
            {
                var descType = typeof(MenuAttrAssemblyInfo);
                foreach (Type type in types)
                {
                    // 强制为自定义特性
                    MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;
                    // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                    if (attribute == null || !attribute.Enabled)
                        continue;

                    if (!attribute.Enabled)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(attribute.MenuPath))
                    {
                        continue;
                    }
                    Model.tb_MenuInfo info = new tb_MenuInfo();
                    string[] sz = attribute.MenuPath.Split('|');
                    if (sz.Length == 2)
                    {
                        menuInfoparent.MenuName = sz[0];
                        menuInfoparent.IsVisble = true;
                        menuInfoparent.IsEnabled = true;
                        menuInfoparent.CaptionCN = sz[0];
                        menuInfoparent.MenuType = "导航菜单";
                    }

                    #region menu 
                    info.MenuName = attribute.Describe;
                    info.IsVisble = true;
                    info.IsEnabled = true;
                    info.CaptionCN = attribute.Describe;
                    info.ClassPath = type.FullName;
                    info.FormName = type.Name;
                    info.Parent_id = 0;
                    if (attribute.MenuBizType.HasValue)
                    {
                        info.BizType = (int)attribute.MenuBizType;
                    }
                    info.MenuType = "行为菜单";
                    #endregion
                    menuList.Add(info);
                }
            }
            return menuList;
        }

        tb_MenuInfoController<tb_MenuInfo> mc = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();


        private void CreateMenu(List<MenuAttrAssemblyInfo> list, int i, long parent_id)
        {

            var arrs = list.GroupBy(x => x.MenuPath.Split('|')[i]);
            //输出
            foreach (var a in arrs)
            {
                tb_MenuInfo menuInfoparent = new tb_MenuInfo();
                // menuInfoparent.MenuID = IdHelper.GetLongId(); //会自动生成ID 第一次这样运行出错，可能没有初始化暂时不管
                menuInfoparent.MenuName = a.Key;
                menuInfoparent.IsVisble = true;
                menuInfoparent.IsEnabled = true;
                menuInfoparent.CaptionCN = a.Key;
                menuInfoparent.MenuType = "导航菜单";
                menuInfoparent.Parent_id = parent_id;
                menuInfoparent.Created_at = System.DateTime.Now;
                mc.AddMenuInfo(menuInfoparent);
                Console.Write(a.Key + ":" + "\r\n");
                foreach (var it in a)
                {
                    //如果最后为空则是行为菜单了
                    if (it.MenuPath.Split('|').Last<string>() == "")
                    {
                        Model.tb_MenuInfo menu = new tb_MenuInfo();
                        // menu.MenuID = IdHelper.GetLongId();
                        menu.MenuName = it.Caption;
                        menu.IsVisble = true;
                        menu.IsEnabled = true;
                        menu.CaptionCN = it.Caption;
                        menu.ClassPath = it.ClassPath;
                        menu.FormName = it.ClassName;
                        menu.Parent_id = menuInfoparent.MenuID;
                        menu.BIBaseForm = it.BIBaseForm;
                        menu.BIBizBaseForm = it.BIBizBaseForm;
                        menu.BizInterface = it.BizInterface;
                        menu.UIPropertyIdentifier = it.UIPropertyIdentifier;
                        if (it.MenuBizType.HasValue)
                        {
                            menu.BizType = (int)it.MenuBizType.Value;
                        }

                        menu.MenuType = "行为菜单";
                        menu.EntityName = it.EntityName;
                        menu.Created_at = System.DateTime.Now;
                        mc.AddMenuInfo(menu);
                    }
                    else
                    {
                        //再次分组 //排除前面不适合的？
                        var newlist = list.Where(w => w.MenuPath.Split('|').Last<string>() != "").ToList();
                        CreateMenu(newlist, i + 1, menuInfoparent.MenuID);
                    }
                    Console.Write(it + " " + "\r\n");
                }
            }
        }

        private void AnalysisMenuPath(MenuAttrAssemblyInfo menuInfo, string caption)
        {
            if (menuInfo.MenuPath.Contains("caption"))
            {
                menuInfo.MenuPath = menuInfo.MenuPath.Replace(caption + "|", "");
            }
        }

        /// <summary>
        /// 缓存给后面使用的菜单列表，注意权限控制
        /// </summary>
        internal List<tb_MenuInfo> MenuList = new List<tb_MenuInfo>();
        private void LoadMenuOfTop()
        {
            using (StatusBusy busy = new StatusBusy("系统正在初始化 请稍候"))
            {
                Thread.Sleep(100);
                //System.Timers.Timer AutoTaskTimer = new System.Timers.Timer(3600000);//每隔一个小时执行一次，没用winfrom自带的
                //System.Timers.Timer AutoTaskTimer = new System.Timers.Timer(1800000);//每隔半个小时执行一次，没用winfrom自带的
                //AutoTaskTimer.Elapsed += AutoTaskTimer_Elapsed;//委托，要执行的方法
                //AutoTaskTimer.AutoReset = true;//获取该定时器自动执行
                //AutoTaskTimer.Enabled = true;//这个一定要写，要不然定时器不会执行的
                /*
                voidHandler handler = new voidHandler(LoadEvent);
                //异步操作接口(注意BeginInvoke方法的不同！)
                IAsyncResult result = handler.BeginInvoke(new AsyncCallback(callback), "AsycState:OK");
                Thread.Sleep(2000);
                */
                ////加载完成后，才执行
                //frmMainCenter center = new frmMainCenter();
                // center.WindowState = FormWindowState.Maximized;
                //center.MdiParent = frmMain.Instance;
                //center.Show();
                //TX();
                //this.tbr_Toolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbr_Toolbar_ButtonClick);
                this.menuStripMain.Items.Clear();
                //MenuPowerHelper p = new MenuPowerHelper();
                MenuPowerHelper p = Startup.GetFromFac<MenuPowerHelper>();
                MenuList = p.AddMenu(this.menuStripMain);

                // 添加"我的工具"菜单
                AddMyToolsMenu();

                //绑定的搜索下拉
                #region

                toolStripMenuSearcher.ComboBox.DataSource = null;
                toolStripMenuSearcher.ComboBox.DataBindings.Clear();
                toolStripMenuSearcher.ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                var SearcherList = MenuList.Where(c => c.MenuType == "行为菜单" && !c.MenuName.Contains("高级查询")).OrderBy(c => c.CaptionCN).ToList();

                AutoCompleteStringCollection sc = new AutoCompleteStringCollection();

                foreach (tb_MenuInfo menuInfo in SearcherList)
                {
                    sc.Add(menuInfo.CaptionCN).ToString();
                }

                toolStripMenuSearcher.ComboBox.AutoCompleteCustomSource = sc;
                toolStripMenuSearcher.ComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

                DropDownListHelper.InitDropList<tb_MenuInfo>(SearcherList, toolStripMenuSearcher.ComboBox, "MenuID", "CaptionCN", false);

                toolStripMenuSearcher.ComboBox.SelectedIndexChanged += (s, e) =>
                {
                    //按钮事件
                    if (s is ComboBox && (s as ComboBox).SelectedItem is tb_MenuInfo menuInfo)
                    {
                        try
                        {
                            menuPowerHelper.ExecuteEvents(menuInfo, null);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                };


                RefreshToolbar();



                #endregion


                //ComboBoxHelper.InitDropList(bs, toolStripMenuSearcher.ComboBox, "ProjectGroup_ID", "ProjectGroupName", ComboBoxStyle.DropDownList, false);

                p.OtherEvent += p_OtherEvent;
                p.MainMenu = this.menuStripMain;

                voidHandler handler = new voidHandler(LoadEvent);
                //异步操作接口(注意BeginInvoke方法的不同！)
                IAsyncResult result = handler.BeginInvoke(new AsyncCallback(callback), "AsycState:OK");
                Thread.Sleep(100);
                //for csla 
                //if (AuthenticationType == "Windows")
                if (false)
                {
                    // AppDomain.CurrentDomain.SetPrincipalPolicy(
                    //   System.Security.Principal.PrincipalPolicy.WindowsPrincipal);
                    // ApplyAuthorizationRules();
                }
                else
                {

                }
                // initialize cache of role list
                //var task = RoleList.CacheListAsync();




            }
        }

        /// <summary>
        /// 添加"我的工具"菜单并注册插件菜单项
        /// </summary>
        private void AddMyToolsMenu()
        {
            try
            {
                // 创建"我的工具"菜单项
                ToolStripMenuItem myToolsMenu = new ToolStripMenuItem("我的工具");
                this.menuStripMain.Items.Add(myToolsMenu);

                // 从Autofac中获取PluginManager实例
                var pluginManager = Startup.GetFromFac<RUINORERP.Plugin.PluginManager>();
                if (pluginManager != null)
                {
                    // 初始化插件管理器
                    pluginManager.Initialize();

                    // 注册所有插件的菜单项
                    pluginManager.RegisterPluginMenuItems(myToolsMenu);
                }
            }
            catch (Exception ex)
            {
                logger.Error("添加我的工具菜单失败: " + ex.Message, ex);
            }
        }

        void p_OtherEvent(object obj)
        {
            //MessageBox.Show("sdfdsfds");
        }



        private void LoadEvent()
        {
            try
            {

                // HLH.Lib.Helper.log4netHelper.info("登陆系统" + DateTime.Now.ToString());

                //frmMain.Instance.PrintInfoLog("登陆系统-并开始检测授权");


                // SMTAPI.Entity.MultiUser.Instance.Serialize(SMTAPI.Entity.MultiUser.Instance);
                // LoadStores();



                // this.Text = this.Text + "------当前用户:" + SMTAPI.Entity.MultiUser.Instance.CurrentUser.ShopInfo;

                //TX();

                // frmMain.Instance.PrintInfoLog(EBAYAPI.Biz.ServiceForOther.GeteBayOfficialTime());
            }
            catch (Exception ex)
            {
                // frmMain.Instance.PrintInfoLog("登陆系统-并开始检测授权" + ex.Message);
            }
        }


        #region  显示提示，不阻塞进程
        public void ShowTips(string Title, string Content, object para)
        {
            try
            {

                kryptonTaskDialog.RadioButtons.Clear();
                //if (checkBoxRadioButtons.Checked)
                //    kryptonTaskDialog.RadioButtons.AddRange(new KryptonTaskDialogCommand[] { kryptonTaskDialogCommand1, kryptonTaskDialogCommand2, kryptonTaskDialogCommand3 });

                kryptonTaskDialog.CommandButtons.Clear();
                // if (checkBoxCommandButtons.Checked)
                //kryptonTaskDialog.CommandButtons.AddRange(new KryptonTaskDialogCommand[] { kryptonTaskDialogCommand4, kryptonTaskDialogCommand5, kryptonTaskDialogCommand6 });
                kryptonTaskDialogCommand1.Tag = para;
                kryptonTaskDialog.CommandButtons.AddRange(new KryptonTaskDialogCommand[] { kryptonTaskDialogCommand1 });



                kryptonTaskDialog.WindowTitle = Title;
                //kryptonTaskDialog.MainInstruction = textBoxMainInstructions.Text;
                kryptonTaskDialog.Content = Content;
                //  kryptonTaskDialog.Icon = (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), comboBoxIcon.Text);
                //  kryptonTaskDialog.CommonButtons = commonButtons;
                //  kryptonTaskDialog.CheckboxText = textBoxCheckBoxText.Text;
                //  kryptonTaskDialog.CheckboxState = checkBoxInitialState.Checked;
                kryptonTaskDialog.FooterText = "请及时处理。";
                // kryptonTaskDialog.FooterHyperlink = textBoxFooterHyperlink.Text;
                // kryptonTaskDialog.FooterIcon = (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), comboBoxFooterIcon.Text);
                kryptonTaskDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public delegate void ShowHandler(string Title, string Content, object para);
        public void ShowCallback(IAsyncResult result)
        {
            ShowHandler handler = (ShowHandler)((AsyncResult)result).AsyncDelegate;
            handler.EndInvoke(result);
            // MainForm.Instance.PrintInfoLog(handler.EndInvoke(result));
            MainForm.Instance.PrintInfoLog(result.AsyncState.ToString());
            MainForm.Instance.PrintInfoLog("ShowCallback 加载完成。");

        }

        #endregion

        public delegate void voidHandler();
        void callback(IAsyncResult result)
        {
            voidHandler handler = (voidHandler)((AsyncResult)result).AsyncDelegate;
            handler.EndInvoke(result);
            //frmMain.Instance.PrintInfoLog(handler.EndInvoke(result));
            //MainForm.Instance.PrintInfoLog(result.AsyncState.ToString());
            // MainForm.Instance.PrintInfoLog("加载完成。");

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            // Get access to current active cell or create new cell if none are present
            KryptonWorkspaceCell cell = kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                kryptonDockableWorkspace1.Root.Children.Add(cell);
            }

            // Create new document to be added into workspace
            //KryptonPage page = NewInput();
            //cell.Pages.Add(page);


            KryptonPage page1 = LocList();
            cell.Pages.Add(page1);


            KryptonPage page_NewForm = NewForm();
            cell.Pages.Add(page_NewForm);
            // Make the new page the selected page
            cell.SelectedPage = page1;
        }

        //ConnectionString = "server=192.168.0.254;uid=sa;pwd=SA!@#123sa ;database=erp",
        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            SqlSugarClient db = new SqlSugarClient(
        new ConnectionConfig()

        {

            ConnectionString = "server=192.168.1.250;uid=sa;pwd=sa ;database=erp",

            DbType = DbType.SqlServer,//设置数据库类型

            IsAutoCloseConnection = true,//自动释放数据库，如果存在事务，在事务结束之后释放。

            InitKeyType = InitKeyType.Attribute//从实体特性中读取主键自增列信息

        });

            //aop监听sql，此段会在每一个"操作语句"执行时都进入....eg:getbyWhere这里会执行两次

            db.Aop.OnLogExecuting = (sql, pars) =>
            {

                string sqlStempt = sql + "参数值：" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));

            };

            //db.DbFirst.IsCreateAttribute().CreateClassFile("c:\\temp\\sq", "Models");
            db.DbFirst.IsCreateAttribute().CreateClassFile(@"G:\数据仓库\软件编程\RUINORERP\RUINORERPTester\RUINORERPTester\Models", "Model");

            //参数1：简化用法



            //var dt = db.Ado.GetDataTable("select * from tb_Supplier where id>3");


        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            SqlSugarClient db = new SqlSugarClient(
        new ConnectionConfig()

        {

            ConnectionString = "server=192.168.0.250;uid=sa;pwd=sa;database=erp",

            DbType = DbType.SqlServer,//设置数据库类型

            IsAutoCloseConnection = true,//自动释放数据库，如果存在事务，在事务结束之后释放。

            InitKeyType = InitKeyType.Attribute//从实体特性中读取主键自增列信息

        });

            //aop监听sql，此段会在每一个"操作语句"执行时都进入....eg:getbyWhere这里会执行两次

            db.Aop.OnLogExecuting = (sql, pars) =>
            {

                string sqlStempt = sql + "参数值：" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));

            };

            // SnowFlakeSingle.WorkId = 1; //从配置文件读取一定要不一样

            //参数1：简化用法
            var insertObj = new tb_Unit();
            insertObj.Unit_ID = 11;
            insertObj.UnitName = "pcs";
            insertObj.Notes = "test";
            //insertObj.Add<tb_Unit>(insertObj);
            // insertObj.FindAllList();

            db.Insertable(insertObj).ExecuteCommand(); //都是参数化实现
                                                       //long id = db.Insertable(insertObj).ExecuteReturnSnowflakeId();
            db.Deleteable<object>().AS("[tb_Unit]").Where("Unit_ID=@id", new { id = 1604030127954595840 }).ExecuteCommand();

            db.Deleteable<object>().AS("[tb_Unit]").Where("Unit_ID=@id", new { id = 4 }).ExecuteCommand();

            tb_Unit tbunit = new tb_Unit();
            tbunit.Unit_ID = 4;
            //tbunit.UnitName = "";


            tb_UnitValidator validator = MainForm.Instance.AppContext.GetRequiredService<tb_UnitValidator>();
            ValidationResult results = validator.Validate(tbunit);
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;

            db.Insertable(tbunit).ExecuteReturnSnowflakeId(); //都是参数化实现




            MessageBox.Show(results.Errors.Count.ToString());
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            Form2 frm = Startup.GetFromFac<Form2>();
            frm.Show();
            return;

            // RoleService rs = new RoleService();

            //Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType("RUINORERP.UI.SS.MenuInit");
            //var test = Startup.GetFromFacByName(t.FullName, t);
            //var SS = Startup.GetFromFac<RUINORERP.UI.SS.MenuInit>();

            var menu = Startup.GetFromFacByName<UserControl>("MENU");

            KryptonWorkspaceCell cell = kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                kryptonDockableWorkspace1.Root.Children.Add(cell);
            }

            // Create new document to be added into workspace
            KryptonPage page = NewPage("Input111", 1, menu);
            cell.Pages.Add(page);
        }


        /// <summary>
        /// 加载皮肤
        /// </summary>
        private void LoadDefaultSkinMenu()
        {
            Array enumValues = Enum.GetValues(typeof(PaletteMode));
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            while (e.MoveNext())
            {

                currentValue = (int)e.Current;
                currentName = e.Current.ToString();
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = currentName;
                tsmi.Name = currentName;
                tsmi.Tag = currentValue;
                tsmi.Click += Tsmi_Click;
                toolStripddbtnSkin.DropDownItems.Add(tsmi);
            }
        }

        private void Tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsi = sender as ToolStripMenuItem;
            kryptonManager.GlobalPaletteMode = (PaletteMode)(tsi.Tag.ObjToInt());
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            kryptonManager.GlobalPaletteMode = PaletteMode.Office2010Blue;
        }


        private void btnUnDoTest_Click(object sender, EventArgs e)
        {
            frmTest frm = new frmTest();
            frm.Show();
        }




        async private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //按账号保存关闭前的布局
            _byteArray = kryptonDockableWorkspace1.SaveLayoutToArray();
            try
            {



                watcher.EnableRaisingEvents = false;
                watcher.Dispose();

                _menuTracker.SaveToDb(); // 确保退出时保存

                //if (MessageBox.Show(this, "确定退出本系统吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //{

                e.Cancel = false;
                System.GC.Collect();
                await Logout();

                var disconnectResult = await communicationService.Disconnect();
                logger?.LogInformation($"程序关闭时断开连接结果: {disconnectResult}");

                logManager.Dispose();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "MainForm_FormClosing");
            }
            finally
            {
                System.GC.Collect();
                //Application.Exit();
                Environment.Exit(0); // 强制终止进程
            }

        }

        internal void PrintInfoLog(string msg, Color c)
        {
            try
            {
                MainForm.Instance.Invoke(new Action(() =>
                {
                    uclog.AddLog("ex", msg);
                }));

            }
            catch (Exception exx)
            {

            }
        }

        internal void PrintInfoLog(string msg, Exception ex)
        {

            try
            {
                MainForm.Instance.Invoke(new Action(() =>
                {
                    uclog.AddLog("ex", msg + ex.Message);
                    MainForm.Instance.logger.LogError(ex, "PrintInfoLog");
                }));

            }
            catch (Exception exx)
            {

            }
        }

        internal void PrintInfoLog(string msg)
        {
            try
            {

                logManager.AddLog("日志", msg);
                //MainForm.Instance.Invoke(new Action(() =>
                //{
                //    uclog.AddLog("print", msg);
                //}));

            }
            catch (Exception ex)
            {

            }


        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            UCUnitList t = Startup.GetFromFac<UCUnitList>();// 或进一步取字段特性也可以
            InitModuleMenu init = Startup.GetFromFac<InitModuleMenu>();
            var btns = init.FindControls<ToolStrip>(t as Control);
            foreach (var item in btns)
            {
                if (item.GetType().Name == "ToolStrip")
                {
                    foreach (var btnItem in item.Items)
                    {
                        if (btnItem is ToolStripItem)
                        {
                            ToolStripItem btn = btnItem as ToolStripItem;
                            //按钮名至少大于1
                            if (btn.Text.Trim().Length > 1)
                            {
                                //添加
                            }
                        }
                    }
                }
            }
        }

        private void btnSwichLang_Click(object sender, EventArgs e)
        {
            int currentLcid = Thread.CurrentThread.CurrentUICulture.LCID;
            currentLcid = (currentLcid == 2052) ? 1033 : 2052;

            // Changes the CurrentUICulture property before changing the resources that are loaded for the win-form.
            //在更改为获胜表单加载的资源之前更改CurrentUICulture属性。
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLcid);

            // Reapplies resources. 重新分配资源。
            //ComponentResourceManager resources = new ComponentResourceManager(typeof(MyForm));
            //resources.ApplyResources(myButton, "myButton");
            //resources.ApplyResources(this, "$this");
        }


        /// <summary>
        /// 清除数据相关的
        /// </summary>
        private async Task ClearData()
        {
            Program.AppContextData.IsOnline = false;

            AppContext.CurUserInfo = null;
            AppContext.IsSuperUser = false;
            RUINORERP.Extensions.SqlsugarSetup.CheckEvent -= SqlsugarSetup_CheckEvent;
            RUINORERP.Extensions.SqlsugarSetup.RemindEvent -= SqlsugarSetup_RemindEvent;
        }

        /// <summary>
        /// 清除UI相关的
        /// </summary>
        private void ClearUI()
        {
            uclog.Clear();
            menuStripMain.Items.Clear();
            kryptonNavigator1.Pages.Clear();
            toolStripddbtnSkin.DropDownItems.Clear();
            //MainForm.Instance.uclog
            //关掉所有窗体
            //  到时做功能  一个锁定  是隐藏， 注销就是删除，退出。直接退出
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }

            //cell.Pages.Clear();
            //清除日志
            //kryptonDockingManager1.RemoveAllPages(false);
            //角色不一样，工作台显示不一样
            // kryptonDockingManager1.RemovePages(kryptonDockingManager1.Pages.Where(x => x.Text != "控制中心" || x.Text != "工作台").ToArray(), false);
            kryptonDockingManager1.RemovePages(kryptonDockingManager1.Pages.Where(x => x.Text != "通讯中心" || x.Text != "系统日志").ToArray(), false);
        }



        /// <summary>
        /// 禁用所有UI组件，确保在锁定状态下用户无法操作
        /// </summary>
        private void DisableAllUIComponents()
        {
            try
            {
                // 禁用主菜单
                menuStripMain.Enabled = false;

                // 禁用所有工具栏
                foreach (ToolStrip toolStrip in this.Controls.OfType<ToolStrip>())
                {
                    toolStrip.Enabled = false;
                }

                // 禁用所有下拉菜单
                foreach (ToolStripDropDownItem item in menuStripMain.Items.OfType<ToolStripDropDownItem>())
                {
                    item.Enabled = false;
                }

                // 禁用导航控件
                kryptonNavigator1.Enabled = false;

                // 禁用停靠管理器中的交互操作
                kryptonDockableWorkspace1.AllowPageDrag = false;

                // 禁用状态栏交互

                logger?.LogInformation("已禁用所有UI组件，系统处于锁定状态");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "禁用UI组件时发生异常");
            }
        }



        /// <summary>
        /// 重新启用所有UI组件，用于登录成功后恢复正常操作
        /// </summary>
        private void ReenableUIComponents()
        {
            try
            {
                // 重新启用主菜单
                menuStripMain.Enabled = true;

                // 重新启用所有工具栏
                foreach (ToolStrip toolStrip in this.Controls.OfType<ToolStrip>())
                {
                    toolStrip.Enabled = true;
                }

                // 重新启用所有下拉菜单
                foreach (ToolStripDropDownItem item in menuStripMain.Items.OfType<ToolStripDropDownItem>())
                {
                    item.Enabled = true;
                }

                // 重新启用导航控件
                kryptonNavigator1.Enabled = true;

                // 重新启用停靠管理器中的交互操作
                kryptonDockableWorkspace1.AllowPageDrag = true;



                logger?.LogInformation("已重新启用所有UI组件，系统恢复正常操作状态");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "重新启用UI组件时发生异常");
            }
        }

        private void ClearRoles()
        {
            if (toolStripDropDownBtnRoles.DropDownItems[0] is ToolStripComboBox comboBoxRoles)
            {
                comboBoxRoles.Items.Clear();
            }
            this.cmbRoles.SelectedIndexChanged -= new System.EventHandler(this.cmbRoles_SelectedIndexChanged);
        }

        private void kryptonTaskDialogCommand1_Execute(object sender, EventArgs e)
        {
            //执行审核 权限检查 然后打开菜单。加载数据
            /*
            MenuPowerHelper mp = Startup.GetFromFac<MenuPowerHelper>();
            var objPara = (sender as KryptonTaskDialogCommand).Tag;

            tb_MenuInfo menuInfo = AppContext.CurUserInfo.UserMenuList.Where(m => m.MenuID == 1723965918893182976).FirstOrDefault();
            mp.ExecuteEvents(menuInfo);
            */
        }



        private void toolBtnExit_Click(object sender, EventArgs e)
        {
            // 使用Invoke确保在UI线程上调用Application.Exit()
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => Application.Exit()));
            }
            else
            {
                try
                {
                    Application.DoEvents();
                    Application.Exit();
                }
                catch (Exception)
                {

                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        private void toolBtnlogOff_Click(object sender, EventArgs e)
        {
            LogLock();
        }

        private void kryptonbtnInitLoadMenu_Click(object sender, EventArgs e)
        {
            using (StatusBusy busy = new StatusBusy("系统正在加载数据... 请稍候"))
            {
                //InitEditObjectValue();
                LoadMenuOfTop();
                LoadMenuPagesByLeft();
            }
        }

        private async void toolStripBtnUpdate_Click(object sender, EventArgs e)
        {
            await UpdateSys(true);
        }

        //public async Task Create(CreateUpdateRoleDto createUpdateRoleDto)
        //{
        //    IRoleService _roleService = new RoleService(new );
        //    RequiredHelper.IsValid(createUpdateRoleDto);
        //    await _roleService.CreateAsync(createUpdateRoleDto);
        //    return Create();
        //}



        #region 特殊的操作

        public static bool FreeTimeExecuteCnce = false;

        #region 最后一次活动时间


        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// 取得最后一次输入时间（秒）
        /// </summary>
        /// <returns></returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = (int)Marshal.SizeOf(vLastInputInfo);

            if (!GetLastInputInfo(ref vLastInputInfo)) return 0;

            long timeInMilliseconds = Environment.TickCount - (long)vLastInputInfo.dwTime;
            long timeInSeconds = timeInMilliseconds / 1000;

            // 处理可能的负数
            if (timeInSeconds < 0)
            {
                timeInSeconds = 0;
            }

            if (timeInSeconds == 0)
            {
                FreeTimeExecuteCnce = false;
            }
            else
            {
                FreeTimeExecuteCnce = true;
            }


            return timeInSeconds;
        }

        //[DllImport("user32.dll")]
        //static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        ///// <summary>
        ///// 取得最后一次输入时间 ms(毫秒)
        ///// 要修改为秒 by watson 2024-10-29
        ///// </summary>
        ///// <returns></returns>
        //public static long GetLastInputTime()
        //{
        //    LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
        //    vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
        //    if (!GetLastInputInfo(ref vLastInputInfo)) return 0;
        //    return Environment.TickCount - (long)vLastInputInfo.dwTime;
        //}

        #endregion

        public void DeleteColumnsConfigFiles()
        {
            try
            {
                string folderPath = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig");
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        // 此方法已迁移到 NotificationService
        // public void ShowMsg(string msg, string Caption = null)
        // { ... }


        /*
        public List<MoveForm> ListMoveForms = new List<MoveForm>();

        /// <summary>
        /// 获取下一个需要显示的窗口的纵坐标
        /// </summary>
        /// <returns></returns>
        public int TopPointY
        {
            get
            {
                int topPointY = Screen.AllScreens[0].WorkingArea.Height;
                foreach (MoveForm m in this.ListMoveForms)
                {
                    if (m.Y < topPointY)
                    {
                        topPointY = m.Y;
                    }
                }
                return topPointY;
            }
        }
        */

        #endregion

        #region 提醒
        // 通知功能已迁移到 NotificationService
        #endregion

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripDropDownBtnRoles.DropDownItems[0] is ToolStripComboBox comboBoxRoles)
            {
                tb_RoleInfo roleInfo = AppContext.Roles.Where(c => c.RoleName == comboBoxRoles.SelectedItem.ToString()).FirstOrDefault();
                if (roleInfo != null)
                {
                    //相当于注销一次
                    ClearUI();
                    AppContext.CurUserInfo.UserModList.Clear();
                    PTPrincipal.SetCurrentUserInfo(AppContext, AppContext.CurUserInfo.UserInfo, roleInfo);
                    //await InitConfig();
                    LoadUIMenus();
                    LoadUIForIM_LogPages();
                    this.SystemOperatorState.Text = $"登陆: {AppContext.CurUserInfo.Name}【{AppContext.CurrentRole.RoleName}】";
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                MainForm.Instance.AppContext.CurrentUser.静止时间 = GetLastInputTime();


                if (GetLastInputTime() > 30 && !MainForm.Instance.AppContext.IsOnline)
                {
                    GetAutoUpdateConfig();
                    //刷新工作台数据？
                    //指向工作台
                    KryptonWorkspaceCell cell = kryptonDockableWorkspace1.ActiveCell;
                    if (cell != null || kryptonDockableWorkspace1.PageCount > 0)
                    {
                        KryptonPage databaord = cell.Pages.Where(x => x.Text == "工作台").FirstOrDefault();
                        if (databaord != null)
                        {
                            cell.SelectedPage = databaord;
                            kryptonDockableWorkspace1.ActivePage = kryptonDockableWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == "工作台");
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取更新配置通过心跳给服务器
        /// </summary>
        private void GetAutoUpdateConfig()
        {
            try
            {
                //读取更新版本的文件配置情况。通过心跳回到服务器上去 
                // 解析现有配置文件
                var (version, updateTime, url) = ParseXmlInfo("AutoUpdaterList.xml");
                // 显示结果
                System.Diagnostics.Debug.WriteLine($"当前版本: {version}");
                System.Diagnostics.Debug.WriteLine($"最后更新时间: {updateTime:yyyy-MM-dd}");
                //重置一下
                MainForm.Instance.AppContext.CurrentUser.客户端版本 = string.Empty;
                if (!string.IsNullOrEmpty(version))
                {
                    MainForm.Instance.AppContext.CurrentUser.客户端版本 += "-" + version;
                }

                MainForm.Instance.AppContext.CurrentUser.客户端版本 += "-" + updateTime;

                if (!string.IsNullOrEmpty(url))
                {
                    MainForm.Instance.AppContext.CurrentUser.客户端版本 += "-" + url;
                }
            }
            catch (Exception)
            {

            }
            finally { }

        }


        public void ShowStatusText(string text)
        {
            this.lblStatusGlobal.Text = text;
            this.lblStatusGlobal.Visible = true;
            statusTimer.Start();
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            statusTimer.Stop();
            this.lblStatusGlobal.Visible = false;
        }

        private async void btntsbRefresh_Click(object sender, EventArgs e)
        {

            logger.LogInformation("获取到最新的验证配置111");

            logger.Debug("2222");
            // 注意：validatorMonitor和validatorConfig来自不同的配置源，可能导致值不一致
            // validatorMonitor: 通过IOptionsMonitor获取，使用ASP.NET Core配置系统
            // validatorConfig: 通过ConfigManagerService获取，使用自定义配置管理系统

            // 使用ConfigManagerService获取最新配置（推荐，确保使用统一的配置源）
            var configManagerService = Startup.GetFromFac<IConfigManagerService>();
            var validatorConfig = configManagerService.GetConfig<GlobalValidatorConfig>();
            if (validatorConfig?.SomeSetting != null && validatorConfig.SomeSetting.Trim().Length > 0)
            {
                // 处理配置值
                logger.LogInformation("获取到最新的验证配置: {Setting}", validatorConfig.SomeSetting);
            }
            var systemGlobalConfig = configManagerService.GetConfig<SystemGlobalConfig>();
            // 避免使用多个不同的配置源，统一使用ConfigManagerService
            // 以下代码已注释，避免配置不一致问题
            var validatorMonitor = Startup.GetFromFac<IOptionsMonitor<ServerGlobalConfig>>();
            var monitor = Startup.GetFromFac<IOptionsMonitor<GlobalValidatorConfig>>();
            var serverConfigValidator = Startup.GetFromFac<ServerGlobalConfig>();

            await UI.Common.UIBizService.RequestCache<tb_UserInfo>(true);
            MainForm.Instance.logger.LogError("LoginWebServer" + System.DateTime.Now.ToString());
            var ss = Business.BizMapperService.EntityMappingHelper.GetEntityType(BizType.采购订单);

            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)
            {
                if (p.ProcessName.ToLower() + ".exe" == "企业数字化集成ERP.exe".ToLower())
                {
                    //break;
                }

                if (p.ProcessName.ToUpper().Contains("RUINORERP.UI"))
                {

                }
            }

            LoginWebServer();
            //if (MainForm.Instance.AppContext.IsSuperUser)
            //{
            //    //请为空时。是请求全部
            //    ClientService.请求缓存(string.Empty);
            //    SystemOptimizerService.异常信息发送("测试异常信息发送");
            //}

            RefreshToolbar();
        }

        public async Task LoginWebServer()
        {
            try
            {
                //HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                //ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
                //var webServerUrl = configManager.GetValue("WebServerUrl");
                //bool islogin = await httpWebService.Login(MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName, MainForm.Instance.AppContext.CurUserInfo.UserInfo.Password, webServerUrl + @"/login");
                //if (islogin)
                //{
                //    MainForm.Instance.uclog.AddLog($"{webServerUrl}登陆成功。");
                //    //var ulid = Ulid.NewUlid();
                //    //var ulidString = ulid.ToString();
                //    //System.Diagnostics.Debug.WriteLine($"Generated ULID: {ulidString}");
                //}
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "LoginWebServer");
            }
        }

        private void lblServerStatus_DoubleClick(object sender, EventArgs e)
        {
            lblServerInfo.Visible = true;
        }

        private void lblServerStatus_Click(object sender, EventArgs e)
        {
            lblServerInfo.Visible = false;
        }

        private void tsbtnloginFileServer_Click(object sender, EventArgs e)
        {

            var entityInfoService = Startup.GetFromFac<Business.BizMapperService.IEntityMappingService>();

            // 获取实体信息
            var entityInfo = entityInfoService.GetEntityInfo(BizType.借出单);


            //如果删除了。服务器上的工作流就可以删除了。

#warning TODO: 这里需要完善具体逻辑，当前仅为占位

            //RequestReminderCommand request = new RequestReminderCommand();
            //request.requestType = RequestReminderType.删除提醒;
            //ReminderData reminderRequest = new ReminderData();
            //reminderRequest.BizPrimaryKey = 11231321;
            //reminderRequest.BizType = BizType.CRM跟进计划;
            //request.requestInfo = reminderRequest;
            //MainForm.Instance.dispatcher.DispatchAsync(request, CancellationToken.None);
        }

        private void tsbtnSysTest_Click(object sender, EventArgs e)
        {
            // 确保密钥长度正确
            // string key = EncryptionHelper.NormalizeKeyLength("黄利华", 32);

            //一次性统计加密码一下密码：
            string enPwd = EncryptionHelper.AesEncryptByHashKey("123456", "张三");

            string pwd = EncryptionHelper.AesDecryptByHashKey(enPwd, "张三");

            frmTest f = new frmTest();
            f.Text = "Child " + (2).ToString();
            //f.MdiParent = this;
            f.ShowDialog();
        }

        /// <summary>
        /// 刷新全局的一些配置 权限等
        /// </summary>
        internal void RefreshGlobalConfig()
        {
            _ = Task.Run(async () =>
               {
                   await Task.Delay(2000);

                   #region  必要配置数据重新加载
                   // 异步延迟3秒执行本位币别查询事件，不会阻止UI线程
                   //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);

                   string username = UserGlobalConfig.Instance.UseName;
                   string password = UserGlobalConfig.Instance.PassWord;
                   string EnPassword = EncryptionHelper.AesEncryptByHashKey(password, username);

                   tb_UserInfoController<tb_UserInfo> ctrUser = AppContext.GetRequiredService<tb_UserInfoController<tb_UserInfo>>();
                   List<tb_UserInfo> users = new List<tb_UserInfo>();
                   users = await ctrUser.QueryByNavWithMoreInfoAsync(u => u.UserName == username && u.Password == EnPassword && u.is_available && u.is_enabled);
                   if (users != null)
                   {
                       AppContext.CurUserInfo.UserModList.Clear();
                       tb_UserInfo user = users[0];
                       if (user != null)
                       {
                           PTPrincipal.SetCurrentUserInfo(AppContext, AppContext.CurUserInfo.UserInfo);
                       }
                   }




                   #region 查询对应的项目组

                   //todo 后面再优化为缓存级吧
                   List<tb_ProjectGroup> projectGroups = new List<tb_ProjectGroup>();
                   List<tb_ProjectGroupEmployees> groupEmployees = new List<tb_ProjectGroupEmployees>();
                   groupEmployees = await MainForm.Instance.AppContext.Db.CopyNew()
                   .Queryable<tb_ProjectGroupEmployees>()
                   .Includes(a => a.tb_projectgroup, b => b.tb_department)
                   .Includes(c => c.tb_projectgroup, d => d.tb_ProjectGroupAccountMappers, e => e.tb_fm_account)
                   .Where(c => c.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.Id).ToListAsync();

                   MainForm.Instance.AppContext.projectGroups = groupEmployees.Select(c => c.tb_projectgroup).ToList();


                   #endregion

                   var ctr = Startup.GetFromFac<tb_SystemConfigController<tb_SystemConfig>>();
                   List<tb_SystemConfig> config = ctr.Query();
                   if (config.Count > 0)
                   {
                       AppContext.SysConfig = config[0];
                       AppContext.FMConfig = JsonConvert.DeserializeObject<FMConfiguration>(config[0].FMConfig);
                       AppContext.FunctionConfig = JsonConvert.DeserializeObject<FunctionConfiguration>(config[0].FunctionConfiguration);

                   }
                   var ctrBillNoRule = Startup.GetFromFac<tb_sys_BillNoRuleController<tb_sys_BillNoRule>>();
                   List<tb_sys_BillNoRule> BillNoRules = ctrBillNoRule.Query();
                   AppContext.BillNoRules = BillNoRules;


                   #endregion


               });
        }

        /// <summary>
        /// 服务缓存测试菜单项点击事件处理
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void 服务缓存测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "打开服务缓存测试窗体时发生错误");
                MessageBox.Show($"打开服务缓存测试窗体时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

