using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Dm.Config;
using HLH.Lib.Helper;
using HLH.Lib.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Log4Net;
using RUINORERP.Extensions;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Commands;
using RUINORERP.Server.Network.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.SmartReminder;
using RUINORERP.Server.Workflow.WFReminder;
using RUINORERP.Server.Workflow.WFScheduled;
using SharpYaml.Tokens;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server.Host;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using RUINORERP.Server.Network.Monitoring;
using RUINORERP.Server.Comm;
using WorkflowCore.Interface;
using RUINORERP.Server.Controls;

namespace RUINORERP.Server
{
    public partial class frmMainNew : Form
    {
        /// <summary>
        /// 注册时定义好了可以使用的功能模块
        /// </summary>
        public List<GlobalFunctionModule> CanUsefunctionModules = new List<GlobalFunctionModule>();

        /// <summary>
        /// 系统保护数据
        /// </summary>
        public tb_sys_RegistrationInfo registrationInfo = new tb_sys_RegistrationInfo();

        public LockManager lockManager = new LockManager();

        /// <summary>
        /// 可配置性全局参数 不要设置为只读 readonly
        /// </summary>
        public IOptionsMonitor<SystemGlobalconfig> Globalconfig;

        //保存要更新 分发的配置数据，（客户不在线时） 是不是 类似的情况。通知都是这样。比方管理员发布一个通知 在线吗上收到。没在线的 上线后收到
        public ConcurrentDictionary<long, BaseConfig> UpdateConfigDataList = new ConcurrentDictionary<long, BaseConfig>();

        /// <summary>
        /// 保存服务器的一些缓存信息。让客户端可以根据一些机制来获取。得到最新的信息
        /// 将来要考虑放到还有其它信息时，key中换为实例。查找时添加类型? 将这个数据保存到缓存中，不过期。定时更新
        /// </summary>
        // public ConcurrentDictionary<string, CacheInfo> CacheInfoList = new ConcurrentDictionary<string, CacheInfo>();
        //MyCacheManager Cache 代替上面的集合

        //保存系统所有提醒的业务数据配置,系统每分钟检测。
        public ConcurrentDictionary<long, ReminderData> ReminderBizDataList = new ConcurrentDictionary<long, ReminderData>();

        /// <summary>
        /// 保存启动的工作流队列 2023-11-18
        /// 暂时用的是通过C端传的单号来找到对应的流程号。实际不科学
        /// </summary>
        public ConcurrentDictionary<string, string> workflowlist = new ConcurrentDictionary<string, string>();
        public ILogger<frmMainNew> _logger { get; set; }

        //一个消息缓存列表，有处理过的。未处理的。未看的。临时性还是固定到表的？
        public Queue<ReminderData> MessageList = new Queue<ReminderData>();
        public IServiceCollection _services { get; set; }
        public IServiceProvider _ServiceProvider { get; set; }

        public bool IsDebug { get; set; } = false;

        private static frmMainNew _main;
        public static frmMainNew Instance
        {
            get { return _main; }
        }

        /// <summary>
        /// 定时器用于定期更新服务器信息
        /// </summary>
        public System.Windows.Forms.Timer UpdateServerInfoTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// 初始化服务器信息更新定时器
        /// </summary>
        private void InitializeServerInfoTimer()
        {
            UpdateServerInfoTimer.Interval = 1000; // 每秒更新一次
            UpdateServerInfoTimer.Tick += UpdateServerInfoTimer_Tick;
        }

        /// <summary>
        /// 定时器事件处理方法，用于更新服务器信息
        /// </summary>
        private void UpdateServerInfoTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_networkServer != null)
                {
                    var serverInfo = _networkServer.GetServerInfo();
                    // 更新UI显示
                    this.BeginInvoke(new Action(() =>
                    {
                        // 更新状态栏显示服务器信息
                        toolStripStatusLabelServerStatus.Text = $"服务状态: {serverInfo.Status}";
                        toolStripStatusLabelConnectionCount.Text = $"连接数: {serverInfo.CurrentConnections}/{serverInfo.MaxConnections}";
                        
                        // 更新额外的服务器信息
                        toolStripStatusLabelMessage.Text = $"服务器IP: {serverInfo.ServerIp}, 端口: {serverInfo.Port}";
                        
                        // 记录服务器信息到日志
                        // PrintInfoLog($"服务器信息 - IP: {serverInfo.ServerIp}, 端口: {serverInfo.Port}, 当前连接: {serverInfo.CurrentConnections}, 最大连接: {serverInfo.MaxConnections}");
                    }));
                }
            }
            catch (Exception ex)
            {
                Instance.PrintInfoLog("更新服务器信息时出错: " + ex.Message);
            }
        }

        private readonly ISessionService _sessionService;
        public IWorkflowHost host;
        private NetworkServer _networkServer;

        public frmMainNew(ILogger<frmMainNew> logger, IWorkflowHost workflowHost, IOptionsMonitor<SystemGlobalconfig> config)
        {
            InitializeComponent();
            _main = this;
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _logger = logger;
            _services = Startup.services;
            host = workflowHost;

            // 初始化服务器信息更新定时器
            InitializeServerInfoTimer();

            Globalconfig = config;
            // 监听配置变化
            Globalconfig.OnChange(updatedConfig =>
            {
                Console.WriteLine($"Configuration has changed: {updatedConfig.SomeSetting}");
            });

            // 初始化导航按钮事件
            InitializeNavigationButtons();
            
            // 初始化菜单和工具栏事件
            InitializeMenuAndToolbarEvents();
        }

        /// <summary>
        /// 初始化菜单和工具栏事件
        /// </summary>
        private void InitializeMenuAndToolbarEvents()
        {
            // 系统菜单事件
            startServerToolStripMenuItem.Click += (s, e) => StartServer();
            stopServerToolStripMenuItem.Click += (s, e) => StopServer();
            reloadConfigToolStripMenuItem.Click += (s, e) => ReloadConfig();
            exitToolStripMenuItem.Click += (s, e) => ExitApplication();
            
            // 管理菜单事件
            userManagementToolStripMenuItem.Click += (s, e) => ShowTabPage("用户管理");
            cacheManagementToolStripMenuItem.Click += (s, e) => ShowTabPage("缓存管理");
            workflowManagementToolStripMenuItem.Click += (s, e) => ShowTabPage("工作流管理");
            blacklistManagementToolStripMenuItem.Click += (s, e) => ShowTabPage("黑名单管理");
            
            // 配置菜单事件
            systemConfigToolStripMenuItem.Click += (s, e) => ShowTabPage("系统配置");
            registrationInfoToolStripMenuItem.Click += (s, e) => ShowTabPage("系统配置");
            dataViewerToolStripMenuItem.Click += (s, e) => ShowTabPage("数据查看");
            
            // 窗口菜单事件
            closeAllToolStripMenuItem.Click += (s, e) => CloseAllTabs();
            
            // 帮助菜单事件
            aboutToolStripMenuItem.Click += (s, e) => ShowAbout();
            helpDocumentationToolStripMenuItem.Click += (s, e) => ShowHelp();
            
            // 工具栏事件
            toolStripButtonStartServer.Click += (s, e) => StartServer();
            toolStripButtonStopServer.Click += (s, e) => StopServer();
            toolStripButtonRefreshData.Click += (s, e) => RefreshCurrentTab();
            toolStripButtonUserManagement.Click += (s, e) => ShowTabPage("用户管理");
            toolStripButtonCacheManagement.Click += (s, e) => ShowTabPage("缓存管理");
            toolStripButtonWorkflowTest.Click += (s, e) => ShowTabPage("工作流管理");
            toolStripButtonSystemConfig.Click += (s, e) => ShowTabPage("系统配置");
        }

        /// <summary>
        /// 初始化导航按钮事件
        /// </summary>
        private void InitializeNavigationButtons()
        {
            buttonServerMonitor.Click += (s, e) => ShowTabPage("服务器监控");
            buttonUserList.Click += (s, e) => ShowTabPage("用户管理");
            buttonCacheManage.Click += (s, e) => ShowTabPage("缓存管理");
            buttonWorkflow.Click += (s, e) => ShowTabPage("工作流管理");
            buttonBlacklist.Click += (s, e) => ShowTabPage("黑名单管理");
            buttonSystemConfig.Click += (s, e) => ShowTabPage("系统配置");
            buttonDataViewer.Click += (s, e) => ShowTabPage("数据查看");
        }

        /// <summary>
        /// 显示指定的Tab页
        /// </summary>
        /// <param name="tabName">Tab页名称</param>
        private void ShowTabPage(string tabName)
        {
            // 查找是否已存在该Tab页
            TabPage existingTabPage = null;
            foreach (TabPage tabPage in tabControlMain.TabPages)
            {
                if (tabPage.Text == tabName)
                {
                    existingTabPage = tabPage;
                    break;
                }
            }

            // 如果不存在，则创建新的Tab页
            if (existingTabPage == null)
            {
                existingTabPage = new TabPage(tabName);
                tabControlMain.TabPages.Add(existingTabPage);
                
                // 根据Tab页名称创建对应的内容控件
                Control contentControl = CreateContentControl(tabName);
                if (contentControl != null)
                {
                    contentControl.Dock = DockStyle.Fill;
                    existingTabPage.Controls.Add(contentControl);
                }
            }

            // 切换到该Tab页
            tabControlMain.SelectedTab = existingTabPage;
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        private void StartServer()
        {
            try
            {
                // 启动服务器逻辑
                if (!frmMain.Instance.ServerStart)
                {
                    frmMain.Instance.InitAll();
                    frmMain.Instance.timer.Start();
                    frmMain.Instance.UpdateServerInfoTimer.Start();
                    frmMain.Instance.tsBtnStartServer.Enabled = false;
                    
                    MessageBox.Show("服务器已启动", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("服务器已经启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动服务器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        private void StopServer()
        {
            try
            {
                // 停止服务器逻辑
                if (frmMain.Instance.ServerStart)
                {
                    frmMain.Instance.Shutdown();
                    frmMain.Instance.timer.Stop();
                    frmMain.Instance.UpdateServerInfoTimer.Stop();
                    frmMain.Instance.tsBtnStartServer.Enabled = true;
                    
                    MessageBox.Show("服务器已停止", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("服务器未启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止服务器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重新加载配置
        /// </summary>
        private void ReloadConfig()
        {
            try
            {
                // 重新加载配置逻辑
                Task.Run(async () => await frmMain.Instance.InitConfig(true)).Wait();
                MessageBox.Show("配置已重新加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重新加载配置时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 退出应用程序
        /// </summary>
        private void ExitApplication()
        {
            try
            {
                // 确认退出
                DialogResult result = MessageBox.Show("确定要退出应用程序吗？", "确认退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 停止服务器
                    StopServer();
                    
                    // 关闭主窗体
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"退出应用程序时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭所有Tab页
        /// </summary>
        private void CloseAllTabs()
        {
            try
            {
                // 确认关闭
                if (tabControlMain.TabPages.Count > 1)
                {
                    DialogResult result = MessageBox.Show($"确定要关闭所有 {tabControlMain.TabPages.Count - 1} 个Tab页吗？", "确认关闭", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // 关闭除第一个Tab页外的所有Tab页
                        for (int i = tabControlMain.TabPages.Count - 1; i > 0; i--)
                        {
                            tabControlMain.TabPages.RemoveAt(i);
                        }
                        
                        MessageBox.Show("所有Tab页已关闭", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("没有可关闭的Tab页", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭Tab页时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示关于信息
        /// </summary>
        private void ShowAbout()
        {
            MessageBox.Show("RUINOR ERP 服务器管理端\n版本: 2.2\n版权所有 (C) RUINOR", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示帮助信息
        /// </summary>
        private void ShowHelp()
        {
            MessageBox.Show("请参考用户手册获取帮助信息", "帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 刷新当前Tab页
        /// </summary>
        private void RefreshCurrentTab()
        {
            try
            {
                if (tabControlMain.SelectedTab != null)
                {
                    string tabName = tabControlMain.SelectedTab.Text;
                    
                    // 重新创建内容控件
                    Control oldControl = null;
                    if (tabControlMain.SelectedTab.Controls.Count > 0)
                    {
                        oldControl = tabControlMain.SelectedTab.Controls[0];
                    }
                    
                    // 移除旧控件
                    if (oldControl != null)
                    {
                        tabControlMain.SelectedTab.Controls.Remove(oldControl);
                        oldControl.Dispose();
                    }
                    
                    // 创建新控件
                    Control newControl = CreateContentControl(tabName);
                    if (newControl != null)
                    {
                        newControl.Dock = DockStyle.Fill;
                        tabControlMain.SelectedTab.Controls.Add(newControl);
                    }
                    
                    MessageBox.Show($"{tabName} 页面已刷新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新页面时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据Tab页名称创建对应的内容控件
        /// </summary>
        /// <param name="tabName">Tab页名称</param>
        /// <returns>内容控件</returns>
        private Control CreateContentControl(string tabName)
        {
            switch (tabName)
            {
                case "服务器监控":
                    // 创建服务器监控控件实例
                    var serverMonitorControl = new ServerMonitorControl();
                    serverMonitorControl.Dock = DockStyle.Fill;
                    return serverMonitorControl;
                case "用户管理":
                    // 创建用户管理控件实例
                    var userManagementControl = new UserManagementControl();
                    userManagementControl.Dock = DockStyle.Fill;
                    return userManagementControl;
                case "缓存管理":
                    // 这里将创建缓存管理控件
                    return CreateCacheManagementControl();
                case "工作流管理":
                    // 创建工作流管理控件实例
                    var workflowManagementControl = new WorkflowManagementControl();
                    workflowManagementControl.Dock = DockStyle.Fill;
                    return workflowManagementControl;
                case "黑名单管理":
                    // 创建黑名单管理控件实例
                    var blacklistManagementControl = new BlacklistManagementControl();
                    blacklistManagementControl.Dock = DockStyle.Fill;
                    return blacklistManagementControl;
                case "系统配置":
                    // 创建系统管理控件实例
                    var systemManagementControl = new SystemManagementControl();
                    systemManagementControl.Dock = DockStyle.Fill;
                    return systemManagementControl;
                case "数据查看":
                    // 创建数据查看控件实例
                    var dataViewerControl = new DataViewerControl();
                    dataViewerControl.Dock = DockStyle.Fill;
                    return dataViewerControl;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 创建缓存管理控件
        /// </summary>
        /// <returns>缓存管理控件</returns>
        private Control CreateCacheManagementControl()
        {
            // 创建缓存管理窗体实例
            var cacheManagementForm = new frmCacheManage();
            cacheManagementForm.Top = 0;
            cacheManagementForm.Left = 0;
            cacheManagementForm.Width = tabControlMain.SelectedTab.Width;
            cacheManagementForm.Height = tabControlMain.SelectedTab.Height;
            cacheManagementForm.FormBorderStyle = FormBorderStyle.None;
            cacheManagementForm.TopLevel = false;
            cacheManagementForm.Dock = DockStyle.Fill;
            cacheManagementForm.Show();
            return cacheManagementForm;
        }

        private void frmMainNew_Load(object sender, EventArgs e)
        {
            // 初始化界面
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "RUINOR ERP 服务器管理端";
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // 刷新一次数据
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                // 更新服务器状态信息
                UpdateServerStatus();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序
                Console.WriteLine($"刷新监控数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新服务器状态信息
        /// </summary>
        public void UpdateServerStatus()
        {
            try
            {
                if (_networkServer == null) return;

                var serverInfo = _networkServer.GetServerInfo();

                // 更新状态栏显示服务器信息
                toolStripStatusLabelServerStatus.Text = $"服务状态: {serverInfo.Status}";
                toolStripStatusLabelConnectionCount.Text = $"连接数: {serverInfo.CurrentConnections}/{serverInfo.MaxConnections}";
                toolStripStatusLabelMessage.Text = $"服务器IP: {serverInfo.ServerIp}, 端口: {serverInfo.Port}";
            }
            catch (Exception ex)
            {
                Instance.PrintInfoLog("更新服务器状态时出错: " + ex.Message);
            }
        }

        public void PrintInfoLog(string msg)
        {
            if (!System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis"))
            {
                try
                {
                    if (IsDisposed || !Instance.IsHandleCreated) return;

                    // 确保最多只有1000行
                    EnsureMaxLines(Instance.richTextBoxLog, 1000);

                    // 将消息格式化为带时间戳和行号的字符串
                    string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {msg}\r\n";
                    if (Instance.InvokeRequired)
                    {

                    }
                    Instance.Invoke(new EventHandler(delegate
                    {
                        Instance.richTextBoxLog.SelectionColor = Color.Black;
                        Instance.richTextBoxLog.AppendText(formattedMsg);
                        Instance.richTextBoxLog.SelectionColor = Color.Black;
                        Instance.richTextBoxLog.ScrollToCaret(); // 滚动到最新的消息

                    }
                    ));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PrintInfoLog时出错" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 打印错误日志到主窗体的RichTextBox控件，使用红色文本显示
        /// </summary>
        /// <param name="msg">错误消息内容</param>
        public void PrintErrorLog(string msg)
        {
            if (!System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis"))
            {
                try
                {
                    if (IsDisposed || !Instance.IsHandleCreated) return;

                    // 确保最多只有1000行
                    EnsureMaxLines(Instance.richTextBoxLog, 1000);

                    // 将消息格式化为带时间戳和行号的字符串
                    string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] [错误] {msg}\r\n";
                    if (Instance.InvokeRequired)
                    {

                    }
                    Instance.Invoke(new EventHandler(delegate
                    {
                        Instance.richTextBoxLog.SelectionColor = Color.Red;
                        Instance.richTextBoxLog.AppendText(formattedMsg);
                        Instance.richTextBoxLog.SelectionColor = Color.Black;
                        Instance.richTextBoxLog.ScrollToCaret(); // 滚动到最新的消息

                    }
                    ));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PrintErrorLog时出错" + ex.Message);
                }
            }
        }

        private void EnsureMaxLines(RichTextBox rtb, int maxLines)
        {
            // 计算当前的行数
            int currentLines = rtb.GetLineFromCharIndex(rtb.Text.Length) + 1;

            // 如果行数超过了最大限制，则删除旧的行
            if (currentLines > maxLines)
            {
                int linesToRemove = currentLines - maxLines;
                int start = rtb.GetFirstCharIndexFromLine(0);
                int end = rtb.GetFirstCharIndexFromLine(linesToRemove);

                // 确保richTextBox1控件在主线程中被操作
                Instance.Invoke(new MethodInvoker(() =>
                {
                    rtb.Text = rtb.Text.Remove(start, end - start);
                }));
            }
        }

        private void frmMainNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 在这里处理窗体关闭逻辑
            // 例如停止服务器、清理资源等
        }
    }
}