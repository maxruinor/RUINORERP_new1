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
using System.IO;
using System.Linq;
using RUINORERP.Model.ConfigModel;
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
using RUINORERP.Business.Cache;
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
using RUINORERP.PacketSpec.Serialization;
using System.Reflection;
using RUINORERP.Business.Config;

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
        /// 服务器信息更新定时器
        /// </summary>
        private System.Windows.Forms.Timer _serverInfoTimer;

        /// <summary>
        /// 初始化服务器信息更新定时器
        /// </summary>
        private void InitializeServerInfoTimer()
        {
            _serverInfoTimer = new System.Windows.Forms.Timer();
            _serverInfoTimer.Interval = 1000; // 每秒更新一次
            _serverInfoTimer.Tick += UpdateServerInfoTimer_Tick;
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
        public IWorkflowHost WorkflowHost;
        private NetworkServer _networkServer;

        private readonly EntityCacheInitializationService _entityCacheInitializationService;

        public frmMainNew(ILogger<frmMainNew> logger, IWorkflowHost workflowHost, IOptionsMonitor<SystemGlobalconfig> config)
        {
            InitializeComponent();
            _main = this;
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _logger = logger;
            _services = Startup.services;
            WorkflowHost = workflowHost;

            // 注入缓存初始化服务
            _entityCacheInitializationService = Program.ServiceProvider.GetRequiredService<EntityCacheInitializationService>();

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

            // 初始化服务器监控Tab页（默认显示）
            InitializeDefaultTab();
        }

        /// <summary>
        /// 初始化菜单和工具栏事件
        /// </summary>
        private void InitializeMenuAndToolbarEvents()
        {
            // 注意：事件绑定已在设计器文件中完成，此处仅保留扩展功能的事件绑定
            // 避免重复绑定导致事件处理程序被多次调用

            // 工具栏事件 - 这些需要在代码中额外绑定
            toolStripButtonRefreshData.Click += (s, e) => RefreshCurrentTab();

            // 如果需要添加新的控件事件，请在此处添加
        }

        /// <summary>
        /// 初始化默认Tab页
        /// </summary>
        private void InitializeDefaultTab()
        {
            try
            {
                // 默认显示服务器监控Tab页
                ShowTabPage("服务器监控");
            }
            catch (Exception ex)
            {
                PrintErrorLog($"初始化默认Tab页时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化导航按钮事件
        /// </summary>
        private void InitializeNavigationButtons()
        {
            // 事件绑定已在设计器文件中完成，此处不再需要lambda绑定
            // 保留此方法以便后续扩展
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
        /// 检查服务器配置是否有效
        /// 使用基于FluentValidation的ConfigValidationService进行配置验证
        /// </summary>
        /// <returns>配置是否有效</returns>
        private bool CheckServerConfiguration()
        {
            try
            {
                PrintInfoLog("正在检查服务器配置...");
                var serverConfig = GetServerConfig();
                if (serverConfig == null)
                {
                    PrintErrorLog("无法获取服务器配置实例");
                    MessageBox.Show("服务器配置初始化失败，无法获取配置信息。", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 从依赖注入容器获取配置验证服务（现在使用FluentValidation实现）
                var validationService = Program.ServiceProvider.GetRequiredService<RUINORERP.Business.Config.IConfigValidationService>();
                
                // 执行配置验证 - 现在会使用我们实现的FluentValidation验证器
                var validationResult = validationService.ValidateConfig(serverConfig);
                
                // 检查验证结果
                if (!validationResult.IsValid)
                {
                    PrintErrorLog($"配置验证失败: {validationResult.GetErrorMessage()}");
                    MessageBox.Show($"服务器配置验证失败:\n{validationResult.GetErrorMessage()}", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                // 注意：详细的路径验证（包括环境变量解析、路径可访问性检查等）
                // 已经在ServerConfigValidator中实现，这里保留一些额外的确保逻辑
                PrintInfoLog("正在执行额外的文件存储路径验证...");
                IConfigManagerService configManagerService = Startup.GetFromFac<IConfigManagerService>();
                // 解析环境变量路径（作为额外的验证保障）
                string resolvedPath = configManagerService.ResolveEnvironmentVariables(serverConfig.FileStoragePath);
                
                if (!string.IsNullOrEmpty(resolvedPath))
                {
                    try
                    {
                        // 确保目录存在，如果不存在则尝试创建（这是一个主动操作，而非严格验证）
                        if (!Directory.Exists(resolvedPath))
                        {
                            PrintInfoLog($"文件存储目录不存在，正在创建: {resolvedPath}");
                            Directory.CreateDirectory(resolvedPath);
                            PrintInfoLog($"文件存储目录创建成功: {resolvedPath}");
                        }

                       

                        PrintInfoLog($"文件存储路径检查通过: {resolvedPath}");
                    }
                    catch (Exception ex)
                    {
                        // 这里的异常处理主要是为了记录日志，因为基本验证已经通过
                        PrintInfoLog($"文件路径操作时发生警告: {ex.Message}");
                    }
                }
                
                PrintInfoLog("服务器配置检查全部通过");
                return true;
            }
            catch (Exception ex)
            {
                PrintErrorLog($"检查服务器配置时发生错误: {ex.Message}");
                MessageBox.Show($"检查服务器配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 获取服务器配置实例
        /// 使用依赖注入容器中的ServerConfig单例或通过ConfigManagerService加载配置
        /// </summary>
        private RUINORERP.Model.ConfigModel.ServerConfig GetServerConfig()
        {
            try
            {
                // 优先使用DI容器中的ServerConfig单例（已通过Startup.cs配置）
                var serverConfig = Startup.GetFromFac<ServerConfig>();
                
                // 如果需要进行环境变量解析或其他后处理，可以使用ConfigManagerService
                if (serverConfig != null && !string.IsNullOrEmpty(serverConfig.FileStoragePath))
                {
                    var configManager = Startup.GetFromFac<RUINORERP.Business.Config.IConfigManagerService>();
                    if (configManager != null)
                    {
                        serverConfig.FileStoragePath = configManager.ResolveEnvironmentVariables(serverConfig.FileStoragePath);
                    }
                }
                
                PrintInfoLog("服务器配置加载成功");
                return serverConfig;
            }
            catch (Exception ex)
            {
                PrintErrorLog($"加载服务器配置失败: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// 验证单个文件分类路径
        /// </summary>
        private void ValidateCategoryPath(string categoryPath, string basePath, string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryPath))
            {
                string fullCategoryPath = Path.Combine(basePath, categoryPath);
                if (!Directory.Exists(fullCategoryPath))
                {
                    PrintInfoLog($"{categoryName}分类目录不存在，正在创建: {fullCategoryPath}");
                    Directory.CreateDirectory(fullCategoryPath);
                }
            }
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        private async Task StartServerAsync()
        {
            // 防止重复启动
            if (!toolStripButtonStartServer.Enabled)
            {
                PrintInfoLog("服务器正在启动或已启动，请勿重复操作");
                return;
            }

            try
            {
                // 检查服务器配置
                if (!CheckServerConfiguration())
                {
                    PrintErrorLog("服务器配置检查失败，启动被取消");
                    ShowTabPage("系统配置");
                    return;
                }

                // 立即禁用启动按钮，防止重复点击
                SetServerButtonsEnabled(false);

                PrintInfoLog("开始启动服务器...");

                // 启动核心服务
                await StartServerCore();

                PrintInfoLog("服务器启动完成");

                // 启动服务器后异步加载缓存，不阻塞UI
                PrintInfoLog("开始异步加载缓存数据...");
                Task.Run(async () =>
                {
                    try
                    {
                        // 记录开始时间，便于分析性能
                        var startTime = DateTime.Now;

                        // 执行缓存初始化
                        await _entityCacheInitializationService.InitializeAllCacheAsync();

                        // 计算耗时并记录完成信息
                        var elapsedTime = DateTime.Now - startTime;
                        this.BeginInvoke(new Action(() =>
                        {
                            PrintInfoLog($"缓存数据加载完成，耗时: {elapsedTime.TotalSeconds:F2}秒");
                        }));
                    }
                    catch (Exception ex)
                    {
                        // 更详细的错误记录
                        this.BeginInvoke(new Action(() =>
                        {
                            PrintErrorLog($"加载缓存数据时出错: {ex.Message}");
                            // 对于关键错误，可以记录更详细的信息
                            PrintErrorLog($"异常类型: {ex.GetType().Name}");

                            // 如果有内部异常，也记录下来
                            if (ex.InnerException != null)
                            {
                                PrintErrorLog($"内部异常: {ex.InnerException.Message}");
                            }
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                PrintErrorLog($"启动服务器时出错: {ex.Message}");

                // 发生错误时重新启用启动按钮
                SetServerButtonsEnabled(true, false);

                MessageBox.Show($"启动服务器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        private async Task StopServerAsync()
        {
            try
            {
                PrintInfoLog("正在停止服务器...");

                // 调用ShutdownAsync方法停止服务器
                await ShutdownAsync();

                // 停止定时器
                _serverInfoTimer?.Stop();

                // 更新UI状态
                SetServerButtonsEnabled(true, false);

                PrintInfoLog("服务器已停止");
                MessageBox.Show("服务器已停止", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                PrintErrorLog($"停止服务器时出错: {ex.Message}");
                MessageBox.Show($"停止服务器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重新加载配置
        /// </summary>
        private async Task ReloadConfigAsync()
        {
            try
            {
                PrintInfoLog("正在重新加载配置...");

                // 重新读取配置文件
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile("configuration.json");
                var configuration = builder.Build();
                var collections = configuration.AsEnumerable();

                // 重新初始化数据库配置
                DbInit();

                PrintInfoLog("配置已重新加载");
                MessageBox.Show("配置已重新加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                PrintErrorLog($"重新加载配置时出错: {ex.Message}");
                MessageBox.Show($"重新加载配置时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 退出应用程序
        /// </summary>
        private async Task ExitApplicationAsync()
        {
            try
            {
                // 确认退出
                DialogResult result = MessageBox.Show("确定要退出应用程序吗？", "确认退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 停止服务器
                    await StopServerAsync();

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
                    // 创建服务器监控控件实例，传入正在运行的NetworkServer实例
                    var serverMonitorControl = new ServerMonitorControl(_networkServer);
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
                    // 创建全局配置控件实例
                    var globalConfigControl = new GlobalConfigControl();
                    globalConfigControl.Dock = DockStyle.Fill;
                    return globalConfigControl;
                case "数据查看":
                    // 创建数据查看控件实例
                    var dataViewerControl = new DataViewerControl();
                    dataViewerControl.Dock = DockStyle.Fill;
                    return dataViewerControl;
                case "注册信息":
                    var registrationManagementControl = new RegistrationManagementControl();
                    registrationManagementControl.Dock = DockStyle.Fill;
                    return registrationManagementControl;
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
            // 创建缓存管理控件实例
            var cacheManagementForm = new Controls.CacheManagementControl();
            cacheManagementForm.Dock = DockStyle.Fill;
            return cacheManagementForm;
        }

        private void frmMainNew_Load(object sender, EventArgs e)
        {
            // 初始化界面
            InitializeUI();

            Initialize();
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

        /// <summary>
        /// 刷新数据 - 已废弃，请使用RefreshCurrentTab方法
        /// </summary>
        [Obsolete("请使用RefreshCurrentTab方法替代")]
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
                PrintInfoLog("更新服务器状态时出错: " + ex.Message);
            }
        }

        public void PrintInfoLog(string msg)
        {
            if (IsIISProcess()) return;

            SafeLogOperation(msg, Color.Black);
        }

        /// <summary>
        /// 检查是否为IIS进程
        /// </summary>
        private bool IsIISProcess()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis");
        }

        /// <summary>
        /// 安全执行日志操作
        /// </summary>
        private void SafeLogOperation(string msg, Color color)
        {
            try
            {
                if (IsDisposed || !IsHandleCreated) return;

                EnsureMaxLines(richTextBoxLog, 1000);
                string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {msg}\r\n";

                Action logAction = () =>
                {
                    richTextBoxLog.SelectionColor = color;
                    richTextBoxLog.AppendText(formattedMsg);
                    richTextBoxLog.SelectionColor = Color.Black;
                    richTextBoxLog.ScrollToCaret();
                };

                if (InvokeRequired)
                {
                    BeginInvoke(new System.Windows.Forms.MethodInvoker(logAction));
                }
                else
                {
                    logAction();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"日志操作出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 打印错误日志到主窗体的RichTextBox控件，使用红色文本显示
        /// </summary>
        /// <param name="msg">错误消息内容</param>
        public void PrintErrorLog(string msg)
        {
            if (IsIISProcess()) return;

            SafeLogOperation($"[错误] {msg}", Color.Red);
        }

        private void EnsureMaxLines(RichTextBox rtb, int maxLines)
        {
            // 确保所有对RichTextBox的操作都在UI线程中执行
            if (rtb.InvokeRequired)
            {
                rtb.BeginInvoke(new System.Windows.Forms.MethodInvoker(() => EnsureMaxLines(rtb, maxLines)));
                return;
            }

            try
            {
                // 计算当前的行数
                int currentLines = rtb.GetLineFromCharIndex(rtb.Text.Length) + 1;

                // 如果行数超过了最大限制，则删除旧的行
                if (currentLines > maxLines)
                {
                    int linesToRemove = currentLines - maxLines;
                    int start = rtb.GetFirstCharIndexFromLine(0);
                    int end = rtb.GetFirstCharIndexFromLine(linesToRemove);

                    if (end > start && end <= rtb.Text.Length)
                    {
                        rtb.Text = rtb.Text.Remove(start, end - start);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，避免影响主程序
                Console.WriteLine($"EnsureMaxLines error: {ex.Message}");
            }
        }

        private async void frmMainNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            await ShutdownAsync();
        }


        IHost host;
        /// <summary>
        /// 启动服务器核心逻辑
        /// </summary>
        private async Task StartServerCore()
        {
            PrintInfoLog("StartServerCore Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            try
            {
                var _logger = Startup.GetFromFac<ILogger<frmMainNew>>();

                // 从DI容器获取NetworkServer实例
                if (_networkServer == null)
                {
                    _networkServer = Startup.GetFromFac<NetworkServer>();
                }

                // 启动网络服务器
                host = await _networkServer.StartAsync(CancellationToken.None);

                // 检查启动是否成功
                if (host != null)
                {
                    PrintInfoLog("网络服务器启动成功");

                    // 启动服务器信息刷新定时器
                    if (_serverInfoTimer != null)
                    {
                        _serverInfoTimer.Start();
                    }

                    // 更新UI状态
                    SetServerButtonsEnabled(false, true);

                    // 启动监控服务
                    var reminderService = Startup.GetFromFac<SmartReminderService>();
                    await Task.Run(async () => await reminderService.StartAsync(CancellationToken.None));

                    // 每120秒（120000毫秒）执行一次检查
                    System.Threading.Timer timerStatus = new System.Threading.Timer(CheckAndRemoveExpiredSessions, null, 0, 1000);

                    //加载提醒数据
                    DataServiceChannel loadService = Startup.GetFromFac<DataServiceChannel>();
                    loadService.LoadCRMFollowUpPlansData(ReminderBizDataList);

                    //启动每天要执行的定时任务
                    GlobalScheduledData globalScheduled = new GlobalScheduledData();
                    var DailyworkflowId = await WorkflowHost.StartWorkflow("NightlyWorkflow", 1, globalScheduled);
                    PrintInfoLog($"NightlyWorkflow每日任务启动{DailyworkflowId}。");

                    // 初始化配置
                    await Task.Run(async () => await InitConfig(false));

                    PrintInfoLog("服务器核心启动完成");
                }
                else
                {
                    // 启动失败
                    throw new Exception("网络服务器返回null，启动失败");
                }
            }
            catch (Exception hostex)
            {
                _logger?.LogError($"NetworkServer启动异常: {hostex.Message}", hostex);
                PrintErrorLog($"NetworkServer启动异常: {hostex.Message}");
                throw; // 重新抛出异常，让上层处理
            }
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public async Task ShutdownAsync()
        {
            try
            {
                // 停止NetworkServer
                if (_networkServer != null)
                {
                    await _networkServer.StopAsync();
                    _networkServer = null;
                }

                // 清理定时器资源
                if (_serverInfoTimer != null)
                {
                    _serverInfoTimer.Stop();
                    _serverInfoTimer.Dispose();
                    _serverInfoTimer = null;
                }
            }
            catch (Exception e)
            {
                PrintErrorLog($"关闭SocketServer失败：{e.Message}");
            }
            finally
            {
                if (host != null)
                {
                    host.Dispose();
                    host = null;
                }
            }
        }

        /// <summary>
        /// 关闭所有服务器会话
        /// </summary>
        public async Task DrainAllServers()
        {
            if (WorkflowHost == null)
            {
                return;
            }

            foreach (var service in host.Services.GetServices<IHostedService>())
            {
                if (service is IServer server)
                {
                    await DrainServer(server);
                    await service.StopAsync(CancellationToken.None);
                }
            }

            await host.WaitForShutdownAsync();
        }

        /// <summary>
        /// 关闭指定服务器的所有会话
        /// </summary>
        /// <param name="server">服务器实例</param>
        private async Task DrainServer(IServer server)
        {
            var container = server.GetSessionContainer();
            if (container != null)
            {
                var serverSessions = container.GetSessions();
                foreach (var session in serverSessions.ToArray())
                {
                    try
                    {
                        await session.CloseAsync(SuperSocket.Connection.CloseReason.ServerShutdown);
                    }
                    catch (Exception ex)
                    {
                        PrintErrorLog($"关闭会话异常: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 处理快捷键
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                HandleCtrlPShortcut();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 处理Ctrl+P快捷键
        /// </summary>
        private void HandleCtrlPShortcut()
        {
            Console.WriteLine("Ctrl + P 被按下");

            using (var frmPassword = new frmInput())
            {
                frmPassword.txtInputContent.PasswordChar = '*';
                frmPassword.WindowState = FormWindowState.Normal;

                if (frmPassword.ShowDialog() == DialogResult.OK)
                {
                    if (frmPassword.InputContent.Trim() == "amwtjhwxf")
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void DbInit()
        {
            try
            {
                var sut = new ServiceCollection()
                    .AddMemoryCache()
                    .AddDistributedMemoryCache() // 为了测试，我们就不使用redis之类的东西了，用个内存实现模拟就好
                    .AddSingleton<ICacheAdapter, MemoryCacheAdapter>()  // 添加缓存适配器
                    .AddSingleton<ICacheAdapter>(i => new DistributedCacheAdapter(i.GetRequiredService<IDistributedCache>(), "distribute"))
                    .BuildServiceProvider();

                IDistributedCache cache = sut.GetService<IDistributedCache>() as IDistributedCache;

                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile("configuration.json");
                var configuration = builder.Build();
                var collections = configuration.AsEnumerable();

                PrintInfoLog("数据库初始化完成");
            }
            catch (Exception ex)
            {
                PrintErrorLog($"数据库初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="loadData">是否加载数据</param>
        public async Task InitConfig(bool loadData = true)
        {
            try
            {
                var _logger = Startup.GetFromFac<ILogger<frmMainNew>>();

                // 使用缓存初始化服务
                var entityCacheInitializationService = Startup.GetFromFac<EntityCacheInitializationService>();

                if (loadData)
                {
                    var startTime = DateTime.Now;

                    // 初始化实体缓存
                    await entityCacheInitializationService.InitializeAllCacheAsync();

                    var endTime = DateTime.Now;
                    var executionTime = endTime - startTime;

                    PrintInfoLog($"初始化缓存完成，执行时间: {executionTime.TotalSeconds:F2} 秒");
                    _logger?.LogInformation($"初始化缓存完成，执行时间: {executionTime.TotalSeconds:F2} 秒");
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"初始化配置失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 检查并移除过期的会话
        /// </summary>
        /// <param name="state">定时器状态</param>
        private void CheckAndRemoveExpiredSessions(object state)
        {
            try
            {
                // 获取会话服务
                var sessionService = Startup.GetFromFac<ISessionService>();
                if (sessionService != null)
                {
                    // 清理超时会话
                    int cleanedCount = sessionService.CleanupTimeoutSessions();
                    if (cleanedCount > 0)
                    {
                        PrintInfoLog($"清理了 {cleanedCount} 个超时会话");
                    }
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"检查过期会话时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证注册信息
        /// </summary>
        /// <param name="regInfo">注册信息</param>
        /// <returns>是否注册有效</returns>
        public bool CheckRegistered(tb_sys_RegistrationInfo regInfo)
        {
            string key = "ruinor1234567890"; // 这应该是一个保密的密钥
            string machineCode = regInfo.MachineCode; // 这可以是机器的硬件信息或其他唯一标识
            // 假设用户输入的注册码
            string userProvidedCode = regInfo.RegistrationCode;
            bool isValid = SecurityService.ValidateRegistrationCode(userProvidedCode, key, machineCode);
            Console.WriteLine($"Is the provided registration code valid? {isValid}");
            return isValid;
        }

        /// <summary>
        /// 唯一硬件信息
        /// </summary>
        public string UniqueHarewareInfo { get; set; }

        /// <summary>
        /// 创建机器码
        /// </summary>
        /// <param name="regInfo">注册信息</param>
        /// <returns>机器码</returns>
        public string CreateMachineCode(tb_sys_RegistrationInfo regInfo)
        {
            //指定关键字段 这些字段生成加密的机器码
            List<string> cols = new List<string>();
            cols.Add("CompanyName");
            cols.Add("ContactName");
            cols.Add("PhoneNumber");
            cols.Add("ConcurrentUsers");
            cols.Add("ExpirationDate");
            cols.Add("ProductVersion");
            cols.Add("LicenseType");
            cols.Add("FunctionModule");

            //只序列化指定的列
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new SelectiveContractResolver(cols),
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" } }
            };

            // 序列化对象
            string jsonString = JsonConvert.SerializeObject(regInfo, settings);
            string originalInfo = this.UniqueHarewareInfo + jsonString;
            string key = "ruinor1234567890";
            string reginfo = HLH.Lib.Security.EncryptionHelper.AesEncrypt(originalInfo, key);
            return reginfo;
        }

        /// <summary>
        /// 服务器监控按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonServerMonitor_Click(object sender, EventArgs e)
        {
            ShowTabPage("服务器监控");
        }

        /// <summary>
        /// 用户管理按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonUserList_Click(object sender, EventArgs e)
        {
            ShowTabPage("用户管理");
        }

        /// <summary>
        /// 缓存管理按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonCacheManage_Click(object sender, EventArgs e)
        {
            ShowTabPage("缓存管理");
        }

        /// <summary>
        /// 工作流管理按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonWorkflow_Click(object sender, EventArgs e)
        {
            ShowTabPage("工作流管理");
        }

        /// <summary>
        /// 黑名单管理按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonBlacklist_Click(object sender, EventArgs e)
        {
            ShowTabPage("黑名单管理");
        }

        /// <summary>
        /// 系统配置按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonSystemConfig_Click(object sender, EventArgs e)
        {
            ShowTabPage("系统配置");
        }

        /// <summary>
        /// 数据查看按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonDataViewer_Click(object sender, EventArgs e)
        {
            ShowTabPage("数据查看");
        }

        /// <summary>
        /// 创建工作流管理控件
        /// </summary>
        /// <returns>工作流管理控件</returns>
        private Control CreateWorkflowManagementControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "工作流管理功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }

        /// <summary>
        /// 创建黑名单管理控件
        /// </summary>
        /// <returns>黑名单管理控件</returns>
        private Control CreateBlacklistManagementControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "黑名单管理功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }

        /// <summary>
        /// 创建系统配置控件
        /// </summary>
        /// <returns>系统配置控件</returns>
        private Control CreateSystemConfigControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "系统配置功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }

        /// <summary>
        /// 创建数据查看控件
        /// </summary>
        /// <returns>数据查看控件</returns>
        private Control CreateDataViewerControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "数据查看功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }

        /// <summary>
        /// 创建服务器监控控件
        /// </summary>
        /// <returns>服务器监控控件</returns>
        private Control CreateServerMonitorControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "服务器监控功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }

        /// <summary>
        /// 创建用户管理控件
        /// </summary>
        /// <returns>用户管理控件</returns>
        private Control CreateUserManagementControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "用户管理功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }

        /// <summary>
        /// 工作流测试工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonWorkflowTest_Click(object sender, EventArgs e)
        {
            ShowTabPage("工作流测试");
        }

        /// <summary>
        /// 系统配置工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonSystemConfig_Click(object sender, EventArgs e)
        {
            ShowTabPage("系统配置");
        }






        /// <summary>
        /// 启动服务器菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await StartServerAsync();
        }

        /// <summary>
        /// 停止服务器菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await StopServerAsync();
        }

        /// <summary>
        /// 重新加载配置菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void reloadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ReloadConfigAsync();
        }

        /// <summary>
        /// 退出菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExitApplicationAsync();
        }

        /// <summary>
        /// 用户管理菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("用户管理");
        }

        /// <summary>
        /// 缓存管理菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void cacheManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("缓存管理");
        }

        /// <summary>
        /// 工作流管理菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void workflowManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("工作流管理");
        }

        /// <summary>
        /// 黑名单管理菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void blacklistManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("黑名单管理");
        }

        /// <summary>
        /// 系统配置菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void systemConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("系统配置");
        }

        /// <summary>
        /// 注册信息菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void registrationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("注册信息");
        }

        /// <summary>
        /// 数据查看菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void dataViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("数据查看");
        }

        /// <summary>
        /// 关闭所有菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllTabs();
        }

        /// <summary>
        /// 关于菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        /// <summary>
        /// 帮助文档菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void helpDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        /// <summary>
        /// 设置服务器按钮状态
        /// </summary>
        /// <param name="startEnabled">启动按钮是否可用</param>
        /// <param name="stopEnabled">停止按钮是否可用</param>
        private void SetServerButtonsEnabled(bool startEnabled, bool stopEnabled = true)
        {
            this.BeginInvoke(new Action(() =>
            {
                toolStripButtonStartServer.Enabled = startEnabled;
                startServerToolStripMenuItem.Enabled = startEnabled;
                toolStripButtonStopServer.Enabled = stopEnabled;
                stopServerToolStripMenuItem.Enabled = stopEnabled;
            }));
        }

        /// <summary>
        /// 启动服务器工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void toolStripButtonStartServer_Click(object sender, EventArgs e)
        {
            await StartServerAsync();
        }

        /// <summary>
        /// 停止服务器工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void toolStripButtonStopServer_Click(object sender, EventArgs e)
        {
            await StopServerAsync();
        }

        /// <summary>
        /// 刷新数据工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonRefreshData_Click(object sender, EventArgs e)
        {
            RefreshCurrentTab();
        }

        /// <summary>
        /// 用户管理工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonUserManagement_Click(object sender, EventArgs e)
        {
            ShowTabPage("用户管理");
        }

        /// <summary>
        /// 缓存管理工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonCacheManagement_Click(object sender, EventArgs e)
        {
            ShowTabPage("缓存管理");
        }

        /// <summary>
        /// 创建注册信息控件
        /// </summary>
        /// <returns>注册信息控件</returns>
        private Control CreateRegistrationInfoControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "注册信息功能开发中...";
            label.AutoSize = true;
            label.Font = new Font("微软雅黑", 12f);
            label.ForeColor = Color.Gray;
            label.Location = new Point((panel.Width - label.Width) / 2, (panel.Height - label.Height) / 2);

            panel.Controls.Add(label);
            return panel;
        }
    }
}