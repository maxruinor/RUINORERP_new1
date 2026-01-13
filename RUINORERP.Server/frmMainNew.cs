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
using RUINORERP.Business.BNR;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Config;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Log4Net;
using RUINORERP.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.ConfigModel;

using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Controls;
using RUINORERP.Server.Network.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Monitoring;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Services;
using RUINORERP.Server.Services.BizCode;
using RUINORERP.Server.SmartReminder;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.WFReminder;
using RUINORERP.Server.Workflow.WFScheduled;
using SharpYaml.Tokens;
using SqlSugar;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server.Host;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WorkflowCore.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using EnumHelper = RUINORERP.Common.Helper.EnumHelper;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using TextBox = System.Windows.Forms.TextBox;

namespace RUINORERP.Server
{
    public partial class frmMainNew : Form, IDisposable
    {
        /// <summary>
        /// 注册时定义好了可以使用的功能模块
        /// </summary>
        public List<GlobalFunctionModule> CanUsefunctionModules = new List<GlobalFunctionModule>();

        /// <summary>
        /// 系统保护数据
        /// </summary>
        public tb_sys_RegistrationInfo registrationInfo = new tb_sys_RegistrationInfo();

        /// <summary>
        /// 可配置性全局参数 不要设置为只读 readonly
        /// </summary>
        public IOptionsMonitor<SystemGlobalConfig> Globalconfig;

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

        // 提醒工作流调度器
        private RUINORERP.Server.Workflow.ReminderWorkflowScheduler _reminderScheduler;

        /// <summary>
        /// 保存启动的工作流队列 2023-11-18
        /// 暂时用的是通过C端传的单号来找到对应的流程号。实际不科学
        /// </summary>
        public ConcurrentDictionary<string, string> workflowlist = new ConcurrentDictionary<string, string>();
        public ILogger<frmMainNew> _logger { get; set; }

        //一个消息缓存列表，有处理过的。未处理的。未看的。临时性还是固定到表的？
        public Queue<ReminderData> MessageList = new Queue<ReminderData>();
        public IServiceCollection _services { get; set; }

        public bool IsDebug { get; set; } = false;
        private static frmMainNew _main;
        /// <summary>
        /// 网络监控开关状态
        /// </summary>
        public bool IsNetworkMonitorEnabled { get; set; } = false;

        /// <summary>
        /// 表示一个命令过滤类别（一级菜单）
        /// </summary>
        public class CommandFilterCategory
        {
            public CommandCategory Category { get; set; }
            public string Name { get; set; }
            public List<CommandFilterType> CommandTypes { get; set; } = new List<CommandFilterType>();
        }

        /// <summary>
        /// 表示一个命令类型（二级菜单）
        /// </summary>
        public class CommandFilterType
        {
            public string TypeName { get; set; }
            public CommandCategory Category { get; set; }
            public List<CommandFilterItem> CommandItems { get; set; } = new List<CommandFilterItem>();
        }

        /// <summary>
        /// 表示一个具体命令（三级菜单）
        /// </summary>
        public class CommandFilterItem
        {
            public ushort CommandCode { get; set; }
            public string Name { get; set; }
            public CommandCategory Category { get; set; }
        }

        /// <summary>
        /// 命令过滤菜单数据模型
        /// </summary>
        private List<CommandFilterCategory> _commandFilterCategories = new List<CommandFilterCategory>();

        /// <summary>
        /// 当前选中的命令过滤器列表
        /// </summary>
        private List<ushort> _selectedCommandFilters = new List<ushort>();

        /// <summary>
        /// 命令过滤菜单项字典
        /// </summary>
        private Dictionary<ushort, ToolStripMenuItem> _commandFilterMenuItems = new Dictionary<ushort, ToolStripMenuItem>();

        /// <summary>
        /// 全局日志级别控制器
        /// </summary>
        private static LogLevel _currentLogLevel = LogLevel.Error;

        /// <summary>
        /// 日志级别菜单项列表
        /// </summary>
        private static int _batchUpdateThreshold = 5;

        /// <summary>
        /// Log4Net BufferSize值
        /// </summary>
        private static int _logBufferSize = 10;

        /// <summary>
        /// 日志级别
        /// </summary>
        private List<ToolStripMenuItem> _logLevelMenuItems = new List<ToolStripMenuItem>();

        /// <summary>
        /// 批量更新阈值菜单项列表
        /// </summary>
        private List<ToolStripMenuItem> _batchThresholdMenuItems = new List<ToolStripMenuItem>();

        /// <summary>
        /// BufferSize菜单项列表
        /// </summary>
        private List<ToolStripMenuItem> _bufferSizeMenuItems = new List<ToolStripMenuItem>();

        // 添加内存监控服务字段
        private MemoryMonitoringService _memoryMonitoringService;
        private bool _disposed = false;

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
        /// 会话清理定时器
        /// </summary>
        private System.Threading.Timer _sessionCleanupTimer;

        /// <summary>
        /// 初始化服务器信息更新定时器
        /// </summary>
        /// <summary>
        /// 调试模式按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonDebugMode_Click(object sender, EventArgs e)
        {
            // 显示下拉菜单或直接切换调试模式
            if (toolStripButtonDebugMode.HasDropDownItems)
            {
                // 如果已有下拉菜单项则显示菜单
                toolStripButtonDebugMode.ShowDropDown();
            }
            else
            {
                // 为保持与旧版本的兼容性，直接切换调试模式
                IsDebug = !IsDebug;

                if (IsDebug)
                {
                    // 调试模式激活，将日志级别设置为Debug
                    SetGlobalLogLevel(LogLevel.Debug);
                    PrintInfoLog("调试模式已激活。日志级别已设置为Debug。");
                    toolStripButtonDebugMode.BackColor = Color.LightBlue;
                }
                else
                {
                    // 调试模式关闭，将日志级别恢复为Error
                    SetGlobalLogLevel(LogLevel.Error);
                    PrintInfoLog("调试模式已关闭。日志级别已恢复为Error。");
                    toolStripButtonDebugMode.BackColor = Color.Transparent;
                }
            }
        }

        /// <summary>
        /// 初始化网络监控菜单
        /// </summary>
        private void InitializeNetworkMonitorMenu()
        {
            try
            {
                // 清空现有菜单项
                toolStripButtonNetworkMonitor.DropDownItems.Clear();
                _commandFilterMenuItems.Clear();

                // 创建全选菜单项
                var selectAllItem = new ToolStripMenuItem("全选");
                selectAllItem.Click += (s, e) => SelectAllCommands();
                toolStripButtonNetworkMonitor.DropDownItems.Add(selectAllItem);

                // 创建取消全选菜单项
                var deselectAllItem = new ToolStripMenuItem("取消全选");
                deselectAllItem.Click += (s, e) => DeselectAllCommands();
                toolStripButtonNetworkMonitor.DropDownItems.Add(deselectAllItem);

                // 添加分隔符
                toolStripButtonNetworkMonitor.DropDownItems.Add(new ToolStripSeparator());

                // 生成命令过滤数据模型
                PopulateCommandFilterModel();

                // 创建菜单层次结构
                foreach (var category in _commandFilterCategories)
                {
                    var categoryMenuItem = new ToolStripMenuItem(category.Name);

                    foreach (var type in category.CommandTypes)
                    {
                        var typeMenuItem = new ToolStripMenuItem(type.TypeName);

                        foreach (var item in type.CommandItems)
                        {
                            var commandMenuItem = new ToolStripMenuItem($"{item.Name} (0x{item.CommandCode:X4})")
                            {
                                CheckOnClick = true,
                                Tag = item.CommandCode
                            };
                            commandMenuItem.Click += CommandFilterMenuItem_Click;

                            // 存储命令菜单项引用
                            _commandFilterMenuItems[item.CommandCode] = commandMenuItem;

                            typeMenuItem.DropDownItems.Add(commandMenuItem);
                        }

                        categoryMenuItem.DropDownItems.Add(typeMenuItem);
                    }

                    toolStripButtonNetworkMonitor.DropDownItems.Add(categoryMenuItem);
                }

                // 添加分隔符
                toolStripButtonNetworkMonitor.DropDownItems.Add(new ToolStripSeparator());

                // 添加监控状态切换菜单项
                var toggleMonitoringItem = new ToolStripMenuItem("启用/禁用监控");
                toggleMonitoringItem.Click += ToggleNetworkMonitoring;
                toolStripButtonNetworkMonitor.DropDownItems.Add(toggleMonitoringItem);

            }
            catch (Exception ex)
            {
                PrintErrorLog($"初始化网络监控菜单时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 填充命令过滤数据模型
        /// </summary>
        private void PopulateCommandFilterModel()
        {
            try
            {
                // 获取所有命令类别
                var categories = Enum.GetValues(typeof(CommandCategory)).Cast<CommandCategory>();

                foreach (var category in categories)
                {
                    // 创建类别对象
                    var categoryInfo = EnumHelper.GetEnumDescription(category);
                    var filterCategory = new CommandFilterCategory
                    {
                        Category = category,
                        Name = categoryInfo
                    };

                    // 为每个类别创建一个类型（简化实现，未来可根据实际需求扩展）
                    var filterType = new CommandFilterType
                    {
                        TypeName = $"{categoryInfo}命令",
                        Category = category
                    };

                    // 反射获取CommandCatalog中该类别的命令
                    var commandFields = typeof(CommandCatalog).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    foreach (var field in commandFields)
                    {
                        // 检查字段是否为ushort类型
                        if (field.FieldType == typeof(ushort))
                        {
                            var commandCode = (ushort)field.GetValue(null);
                            var commandCategory = (CommandCategory)(commandCode >> 8);

                            // 如果命令类别匹配
                            if (commandCategory == category)
                            {
                                // 获取命令描述
                                var description = field.Name;
                                // 使用字段名称作为描述
                                // 由于XmlDocumentation类不可用，我们直接使用字段名
                                description = field.Name;

                                // 创建命令项
                                var commandItem = new CommandFilterItem
                                {
                                    CommandCode = commandCode,
                                    Name = description,
                                    Category = category
                                };

                                filterType.CommandItems.Add(commandItem);
                            }
                        }
                    }

                    // 只有在有命令项时才添加类型
                    if (filterType.CommandItems.Count > 0)
                    {
                        filterCategory.CommandTypes.Add(filterType);
                        _commandFilterCategories.Add(filterCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"填充命令过滤模型时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 网络监控按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonNetworkMonitor_Click(object sender, EventArgs e)
        {
            // 显示下拉菜单
            if (toolStripButtonNetworkMonitor.HasDropDownItems)
            {
                toolStripButtonNetworkMonitor.ShowDropDown();
            }
            else
            {
                // 如果菜单尚未初始化，则初始化菜单
                InitializeNetworkMonitorMenu();
                toolStripButtonNetworkMonitor.ShowDropDown();
            }
        }

        /// <summary>
        /// 切换网络监控状态
        /// </summary>
        private void ToggleNetworkMonitoring(object sender, EventArgs e)
        {
            IsNetworkMonitorEnabled = !IsNetworkMonitorEnabled;
            // ToolStripDropDownButton不支持Checked属性，使用颜色变化作为视觉反馈
            if (IsNetworkMonitorEnabled)
            {
                toolStripButtonNetworkMonitor.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                toolStripButtonNetworkMonitor.ForeColor = System.Drawing.SystemColors.ControlText;
            }

            if (IsNetworkMonitorEnabled)
            {
                PrintInfoLog("网络监控已激活。将显示所有套接字收发信息。");
                toolStripButtonNetworkMonitor.BackColor = Color.LightGreen;
            }
            else
            {
                PrintInfoLog("网络监控已关闭。将不再显示套接字收发信息。");
                toolStripButtonNetworkMonitor.BackColor = Color.Transparent;
            }

            // 通知SuperSocketCommandAdapter网络监控状态变更
            RUINORERP.Server.Network.SuperSocket.SuperSocketCommandAdapter<IAppSession>.SetNetworkMonitorEnabled(IsNetworkMonitorEnabled);
            // 设置过滤器
            RUINORERP.Server.Network.SuperSocket.SuperSocketCommandAdapter<IAppSession>.SetCommandFilters(_selectedCommandFilters);
        }

        /// <summary>
        /// 命令过滤菜单项点击事件
        /// </summary>
        private void CommandFilterMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is ushort commandCode)
            {
                if (menuItem.Checked)
                {
                    // 添加到已选过滤器
                    if (!_selectedCommandFilters.Contains(commandCode))
                    {
                        _selectedCommandFilters.Add(commandCode);
                    }
                }
                else
                {
                    // 从已选过滤器移除
                    _selectedCommandFilters.Remove(commandCode);
                }

                // 更新过滤器设置
                RUINORERP.Server.Network.SuperSocket.SuperSocketCommandAdapter<IAppSession>.SetCommandFilters(_selectedCommandFilters);

                // 记录过滤变更
                PrintInfoLog($"网络监控过滤器已更新。当前选中{_selectedCommandFilters.Count}个命令。");
            }
        }

        /// <summary>
        /// 全选命令
        /// </summary>
        private void SelectAllCommands()
        {
            _selectedCommandFilters.Clear();

            foreach (var menuItem in _commandFilterMenuItems.Values)
            {
                menuItem.Checked = true;
                if (menuItem.Tag is ushort commandCode)
                {
                    _selectedCommandFilters.Add(commandCode);
                }
            }

            // 更新过滤器设置
            RUINORERP.Server.Network.SuperSocket.SuperSocketCommandAdapter<IAppSession>.SetCommandFilters(_selectedCommandFilters);
            PrintInfoLog("已选择所有网络命令进行监控。");
        }

        /// <summary>
        /// 取消全选命令
        /// </summary>
        private void DeselectAllCommands()
        {
            foreach (var menuItem in _commandFilterMenuItems.Values)
            {
                menuItem.Checked = false;
            }

            _selectedCommandFilters.Clear();

            // 更新过滤器设置
            RUINORERP.Server.Network.SuperSocket.SuperSocketCommandAdapter<IAppSession>.SetCommandFilters(_selectedCommandFilters);
            PrintInfoLog("已取消所有网络命令选择。");
        }
        /// <summary>
        /// 初始化服务器信息更新定时器
        /// </summary>
        private void InitializeServerInfoTimer()
        {
            _serverInfoTimer = new System.Windows.Forms.Timer();
            _serverInfoTimer.Interval = 5000; // 每5秒更新一次，减少UI线程压力
            _serverInfoTimer.Tick += UpdateServerInfoTimer_Tick;
        }

        /// <summary>
        /// 定时器事件处理方法，用于更新服务器信息
        /// </summary>
        private void UpdateServerInfoTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 使用异步方式执行更新，避免阻塞UI线程
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (_networkServer != null)
                        {
                            var serverInfo = _networkServer.GetServerInfo();
                            // 更新UI显示
                            this.Invoke(new Action(() =>
                            {
                                // 更新状态栏服务器信息
                                toolStripStatusLabelServerStatus.Text = $"服务状态: {serverInfo.Status}";
                                toolStripStatusLabelConnectionCount.Text = $"连接数: {serverInfo.CurrentConnections}/{serverInfo.MaxConnections}";

                                // 更新附加服务器信息
                                toolStripStatusLabelMessage.Text = $"服务器IP: {serverInfo.ServerIp}, 端口: {serverInfo.Port}";

                                // 记录服务器信息日志
                                // PrintInfoLog($"服务器信息 - IP: {serverInfo.ServerIp}, 端口: {serverInfo.Port}, 当前连接: {serverInfo.CurrentConnections}, 最大连接: {serverInfo.MaxConnections}");
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        // 确保在UI线程上记录错误日志
                        this.Invoke(new Action(() =>
                        {
                            Instance.PrintInfoLog("服务器信息更新过程中发生错误: " + ex.Message);
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                Instance.PrintInfoLog("启动服务器信息更新任务时发生错误: " + ex.Message);
            }
        }

        private readonly ISessionService _sessionService;
        public IWorkflowHost WorkflowHost;
        private NetworkServer _networkServer;

        private readonly EntityCacheInitializationService _entityCacheInitializationService;
        private readonly IRegistrationService _registrationService;
        public frmMainNew(ILogger<frmMainNew> logger,

            IWorkflowHost workflowHost, IOptionsMonitor<SystemGlobalConfig> config)
        {
            InitializeComponent();
            _main = this;
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _registrationService = Startup.GetFromFac<IRegistrationService>();
            _logger = logger;
            WorkflowHost = workflowHost;
            // 注入缓存初始化服务
            _entityCacheInitializationService = Program.ServiceProvider.GetRequiredService<EntityCacheInitializationService>();

            // 初始化内存监控服务
            _memoryMonitoringService = Program.ServiceProvider.GetRequiredService<MemoryMonitoringService>();
            _memoryMonitoringService.MemoryUsageWarning += OnMemoryUsageWarning;
            _memoryMonitoringService.MemoryUsageCritical += OnMemoryUsageCritical;

            // 初始化服务器信息更新定时器
            InitializeServerInfoTimer();

            Globalconfig = config;
            // 初始化导航按钮事件
            InitializeNavigationButtons();

            // 初始化菜单和工具栏事件
            InitializeMenuAndToolbarEvents();

            // 初始化日志级别菜单
            InitializeLogLevelMenu();

            // 初始化服务器监控选项卡页面（默认显示）
            InitializeDefaultTab();

            // 启动UI日志处理泵
            StartUiLogPump();
        }

        // 添加内存使用事件处理方法
        private void OnMemoryUsageWarning(object sender, MemoryUsageEventArgs e)
        {
            PrintInfoLog($"内存使用警告: 当前使用 {e.MemoryInfo.WorkingSetMB} MB");
        }

        private void OnMemoryUsageCritical(object sender, MemoryUsageEventArgs e)
        {
            PrintErrorLog($"内存使用严重: 当前使用 {e.MemoryInfo.WorkingSetMB} MB，正在执行垃圾回收");
            // 在后台线程执行垃圾回收，避免阻塞UI
            Task.Run(() => _memoryMonitoringService.ForceGarbageCollection());
        }

        /// <summary>
        /// 初始化菜单和工具栏事件
        /// </summary>
        private void InitializeMenuAndToolbarEvents()
        {
            // 注意：事件绑定已在设计器文件中完成。此处仅保留附加功能的事件绑定
            // 防止因重复绑定导致事件处理程序被多次调用

            // 工具栏事件 - 这些需要在代码中额外绑定
            //toolStripButtonRefreshData.Click += (s, e) => RefreshCurrentTab();

            // 如果需要额外的控件事件在此添加
        }

        /// <summary>
        /// 初始化日志级别菜单
        /// </summary>
        private void InitializeLogLevelMenu()
        {
            try
            {
                // 清空现有菜单项
                toolStripButtonDebugMode.DropDownItems.Clear();
                _logLevelMenuItems.Clear();
                _batchThresholdMenuItems.Clear();
                _bufferSizeMenuItems.Clear();

                // 获取所有LogLevel枚举值
                var logLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();

                // 为每个LogLevel创建菜单项
                foreach (var level in logLevels)
                {
                    var menuItem = new ToolStripMenuItem(level.ToString());
                    menuItem.Tag = level;
                    menuItem.Click += LogLevelMenuItem_Click;
                    toolStripButtonDebugMode.DropDownItems.Add(menuItem);
                    _logLevelMenuItems.Add(menuItem);
                }

                // 添加分隔符
                var separator1 = new ToolStripSeparator();
                toolStripButtonDebugMode.DropDownItems.Add(separator1);

                // 添加批量更新阈值菜单项
                var batchThresholdMenu = new ToolStripMenuItem("批量更新阈值");
                var thresholdValues = new int[] { 1, 5, 10, 20, 50 };

                foreach (var threshold in thresholdValues)
                {
                    var thresholdItem = new ToolStripMenuItem(threshold.ToString());
                    thresholdItem.Tag = threshold;
                    thresholdItem.Click += BatchThresholdMenuItem_Click;
                    batchThresholdMenu.DropDownItems.Add(thresholdItem);
                    _batchThresholdMenuItems.Add(thresholdItem);
                }

                // 添加自定义阈值选项
                var customThresholdItem = new ToolStripMenuItem("自定义...");
                customThresholdItem.Click += CustomThresholdMenuItem_Click;
                batchThresholdMenu.DropDownItems.Add(customThresholdItem);

                toolStripButtonDebugMode.DropDownItems.Add(batchThresholdMenu);

                // 添加分隔符
                var separator2 = new ToolStripSeparator();
                toolStripButtonDebugMode.DropDownItems.Add(separator2);

                // 添加BufferSize菜单项
                var bufferSizeMenu = new ToolStripMenuItem("日志BufferSize");
                var bufferSizeValues = new int[] { 1, 10, 50, 100, 200 };

                foreach (var bufferSize in bufferSizeValues)
                {
                    var bufferSizeItem = new ToolStripMenuItem(bufferSize.ToString());
                    bufferSizeItem.Tag = bufferSize;
                    bufferSizeItem.Click += BufferSizeMenuItem_Click;
                    bufferSizeMenu.DropDownItems.Add(bufferSizeItem);
                    _bufferSizeMenuItems.Add(bufferSizeItem);
                }

                // 添加自定义BufferSize选项
                var customBufferSizeItem = new ToolStripMenuItem("自定义...");
                customBufferSizeItem.Click += CustomBufferSizeMenuItem_Click;
                bufferSizeMenu.DropDownItems.Add(customBufferSizeItem);

                toolStripButtonDebugMode.DropDownItems.Add(bufferSizeMenu);

                // 添加分隔符和调试模式开关
                var separator3 = new ToolStripSeparator();
                toolStripButtonDebugMode.DropDownItems.Add(separator3);

                var debugModeItem = new ToolStripMenuItem("调试模式开关");
                debugModeItem.Checked = IsDebug;
                debugModeItem.Click += (s, e) =>
                {
                    IsDebug = !IsDebug;
                    debugModeItem.Checked = IsDebug;

                    if (IsDebug)
                    {
                        // 启用调试模式，设置日志级别为Debug
                        SetGlobalLogLevel(LogLevel.Debug);
                        PrintInfoLog("调试模式已启用，日志级别设置为Debug");
                        toolStripButtonDebugMode.BackColor = Color.LightBlue;
                    }
                    else
                    {
                        // 禁用调试模式，恢复日志级别为Error
                        SetGlobalLogLevel(LogLevel.Error);
                        PrintInfoLog("调试模式已禁用，日志级别恢复为Error");
                        toolStripButtonDebugMode.BackColor = Color.Transparent;
                    }
                };
                toolStripButtonDebugMode.DropDownItems.Add(debugModeItem);

                // 更新菜单项选中状态
                UpdateLogLevelMenuCheckState();
                UpdateBatchThresholdMenuCheckState();
                UpdateBufferSizeMenuCheckState();
            }
            catch (Exception ex)
            {
                PrintErrorLog($"初始化日志级别菜单时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新日志级别菜单项的选中状态
        /// </summary>
        private void UpdateLogLevelMenuCheckState()
        {
            foreach (var menuItem in _logLevelMenuItems)
            {
                if (menuItem.Tag is LogLevel level)
                {
                    menuItem.Checked = level == _currentLogLevel;
                }
            }
        }

        /// <summary>
        /// 更新批量更新阈值菜单项的选中状态
        /// </summary>
        private void UpdateBatchThresholdMenuCheckState()
        {
            foreach (var menuItem in _batchThresholdMenuItems)
            {
                if (menuItem.Tag is int threshold)
                {
                    menuItem.Checked = threshold == _batchUpdateThreshold;
                }
            }
        }

        /// <summary>
        /// 更新BufferSize菜单项的选中状态
        /// </summary>
        private void UpdateBufferSizeMenuCheckState()
        {
            foreach (var menuItem in _bufferSizeMenuItems)
            {
                if (menuItem.Tag is int bufferSize)
                {
                    menuItem.Checked = bufferSize == _logBufferSize;
                }
            }
        }

        /// <summary>
        /// 日志级别菜单项点击事件
        /// </summary>
        private void LogLevelMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is ToolStripMenuItem menuItem && menuItem.Tag is LogLevel level)
                {
                    // 设置全局日志级别
                    SetGlobalLogLevel(level);

                    // 更新菜单项选择状态
                    UpdateLogLevelMenuCheckState();

                    // 记录日志级别变更
                    PrintInfoLog($"全局日志级别已设置: {level}");
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"设置日志级别时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量更新阈值菜单项点击事件
        /// </summary>
        private void BatchThresholdMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is ToolStripMenuItem menuItem && menuItem.Tag is int threshold)
                {
                    // 设置批量更新阈值
                    SetBatchUpdateThreshold(threshold);

                    // 更新菜单项选择状态
                    UpdateBatchThresholdMenuCheckState();

                    // 记录阈值变更
                    PrintInfoLog($"数据库批量更新阈值已设置: {threshold}");
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"设置批量更新阈值时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 自定义阈值菜单项点击事件
        /// </summary>
        private void CustomThresholdMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var inputForm = new Form())
                {
                    inputForm.Text = "自定义阈值设置";
                    inputForm.Width = 300;
                    inputForm.Height = 150;
                    inputForm.StartPosition = FormStartPosition.CenterParent;
                    inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    inputForm.MaximizeBox = false;
                    inputForm.MinimizeBox = false;

                    var label = new Label();
                    label.Text = "请输入批量更新阈值:";
                    label.Location = new Point(20, 20);
                    label.AutoSize = true;

                    var textBox = new TextBox();
                    textBox.Location = new Point(20, 45);
                    textBox.Width = 200;
                    textBox.Text = _batchUpdateThreshold.ToString();

                    var okButton = new Button();
                    okButton.Text = "确定";
                    okButton.Location = new Point(60, 80);
                    okButton.DialogResult = DialogResult.OK;

                    var cancelButton = new Button();
                    cancelButton.Text = "取消";
                    cancelButton.Location = new Point(160, 80);
                    cancelButton.DialogResult = DialogResult.Cancel;

                    inputForm.Controls.Add(label);
                    inputForm.Controls.Add(textBox);
                    inputForm.Controls.Add(okButton);
                    inputForm.Controls.Add(cancelButton);

                    inputForm.AcceptButton = okButton;
                    inputForm.CancelButton = cancelButton;

                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        if (int.TryParse(textBox.Text, out int threshold) && threshold > 0)
                        {
                            // 设置批量更新阈值
                            SetBatchUpdateThreshold(threshold);

                            // 更新菜单项选择状态
                            UpdateBatchThresholdMenuCheckState();

                            // 记录阈值变更
                            PrintInfoLog($"数据库批量更新阈值已设置: {threshold}");
                        }
                        else
                        {
                            MessageBox.Show("请输入有效的正整数阈值。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"设置自定义阈值时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// BufferSize菜单项点击事件
        /// </summary>
        private void BufferSizeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is ToolStripMenuItem menuItem && menuItem.Tag is int bufferSize)
                {
                    // 设置BufferSize
                    SetLogBufferSize(bufferSize);

                    // 更新菜单项选择状态
                    UpdateBufferSizeMenuCheckState();

                    // 记录BufferSize变更
                    PrintInfoLog($"日志BufferSize已设置: {bufferSize}");
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"设置BufferSize时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 自定义BufferSize菜单项点击事件
        /// </summary>
        private void CustomBufferSizeMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var inputForm = new Form())
                {
                    inputForm.Text = "自定义BufferSize设置";
                    inputForm.Width = 300;
                    inputForm.Height = 150;
                    inputForm.StartPosition = FormStartPosition.CenterParent;
                    inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    inputForm.MaximizeBox = false;
                    inputForm.MinimizeBox = false;

                    var label = new Label();
                    label.Text = "请输入BufferSize值:";
                    label.Location = new Point(20, 20);
                    label.AutoSize = true;

                    var textBox = new TextBox();
                    textBox.Location = new Point(20, 45);
                    textBox.Width = 200;
                    textBox.Text = _logBufferSize.ToString();

                    var okButton = new Button();
                    okButton.Text = "确定";
                    okButton.Location = new Point(60, 80);
                    okButton.DialogResult = DialogResult.OK;

                    var cancelButton = new Button();
                    cancelButton.Text = "取消";
                    cancelButton.Location = new Point(160, 80);
                    cancelButton.DialogResult = DialogResult.Cancel;

                    inputForm.Controls.Add(label);
                    inputForm.Controls.Add(textBox);
                    inputForm.Controls.Add(okButton);
                    inputForm.Controls.Add(cancelButton);

                    inputForm.AcceptButton = okButton;
                    inputForm.CancelButton = cancelButton;

                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        if (int.TryParse(textBox.Text, out int bufferSize) && bufferSize > 0)
                        {
                            // 设置BufferSize
                            SetLogBufferSize(bufferSize);

                            // 更新菜单项选择状态
                            UpdateBufferSizeMenuCheckState();

                            // 记录BufferSize变更
                            PrintInfoLog($"日志BufferSize已设置: {bufferSize}");
                        }
                        else
                        {
                            MessageBox.Show("请输入有效的正整数BufferSize值。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"设置自定义BufferSize时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置全局日志级别
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        public static void SetGlobalLogLevel(LogLevel logLevel)
        {
            _currentLogLevel = logLevel;
            // 记录日志级别变更
            var loggerFactory = Program.ServiceProvider.GetService<ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger<frmMainNew>();


            // 更新菜单项选中状态
            Instance?.UpdateLogLevelMenuCheckState();
        }

        /// <summary>
        /// 设置数据库批量更新阈值
        /// </summary>
        /// <param name="threshold">批量更新阈值</param>
        public static void SetBatchUpdateThreshold(int threshold)
        {
            if (threshold > 0)
            {
                _batchUpdateThreshold = threshold;
                // 设置DatabaseSequenceService的阈值
                DatabaseSequenceService.SetBatchUpdateThreshold(threshold);

                // 更新菜单项选择状态
                Instance?.UpdateBatchThresholdMenuCheckState();
            }
        }

        /// <summary>
        /// 设置日志BufferSize
        /// </summary>
        /// <param name="bufferSize">BufferSize值</param>
        public static void SetLogBufferSize(int bufferSize)
        {
            if (bufferSize > 0)
            {
                _logBufferSize = bufferSize;
                // 设置Log4Net的BufferSize
                UpdateLogBufferSize(bufferSize);

                // 更新菜单项选择状态
                Instance?.UpdateBufferSizeMenuCheckState();
            }
        }

        /// <summary>
        /// 获取当前全局日志级别
        /// </summary>
        /// <returns>当前日志级别</returns>
        public static LogLevel GetGlobalLogLevel()
        {
            return _currentLogLevel;
        }

        /// <summary>
        /// 获取当前批量更新阈值
        /// </summary>
        /// <returns>当前阈值</returns>
        public static int GetBatchUpdateThreshold()
        {
            return _batchUpdateThreshold;
        }

        /// <summary>
        /// 获取当前日志BufferSize
        /// </summary>
        /// <returns>当前BufferSize值</returns>
        public static int GetLogBufferSize()
        {
            return _logBufferSize;
        }

        /// <summary>
        /// 初始化默认选项卡页面
        /// </summary>
        private void InitializeDefaultTab()
        {
            try
            {
                // 默认显示服务器监控选项卡页面
                ShowTabPage("服务器监控");
            }
            catch (Exception ex)
            {
                PrintErrorLog($"初始化默认选项卡页面时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化导航按钮事件
        /// </summary>
        private void InitializeNavigationButtons()
        {
            // 事件绑定已在设计器文件中完成。此处不需要Lambda绑定
            // 保留此方法以备后续扩展
        }

        /// <summary>
        /// 显示指定的选项卡页面
        /// </summary>
        /// <param name="tabName">选项卡页面名称</param>
        private void ShowTabPage(string tabName)
        {
            // 检查是否已存在该选项卡页面
            TabPage existingTabPage = null;
            foreach (TabPage tabPage in tabControlMain.TabPages)
            {
                if (tabPage.Text == tabName)
                {
                    existingTabPage = tabPage;
                    break;
                }
            }

            // 如果不存在则创建新的选项卡页面
            if (existingTabPage == null)
            {
                existingTabPage = new TabPage(tabName);
                tabControlMain.TabPages.Add(existingTabPage);

                // 根据选项卡页面名称创建对应的内容控件
                Control contentControl = CreateContentControl(tabName);
                if (contentControl != null)
                {
                    contentControl.Dock = DockStyle.Fill;
                    existingTabPage.Controls.Add(contentControl);
                }
            }

            // 切换到该选项卡页面
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

                // 从依赖注入容器获取配置验证服务（当前使用FluentValidation）
                var validationService = Program.ServiceProvider.GetRequiredService<RUINORERP.Business.Config.IConfigValidationService>();

                // 执行配置验证 - 现在使用FluentValidation验证器
                var validationResult = validationService.ValidateConfig(serverConfig);

                // 检查验证结果
                if (!validationResult.IsValid)
                {
                    PrintErrorLog($"配置验证失败: {validationResult.GetErrorMessage()}");
                    MessageBox.Show($"服务器配置验证失败:\n{validationResult.GetErrorMessage()}", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 注意：详细路径验证（环境变量解析、路径可访问性检查等）
                // 注意：详细路径验证（环境变量解析、路径可访问性检查等）
                // 已在ServerConfigValidator中实现，因此此处保持额外的保障逻辑
                PrintInfoLog("正在执行额外的文件存储路径验证...");
                IConfigManagerService configManagerService = Startup.GetFromFac<IConfigManagerService>();
                // 环境变量路径解析（作为额外的验证保障）
                string resolvedPath = configManagerService.ResolveEnvironmentVariables(serverConfig.FileStoragePath);

                if (!string.IsNullOrEmpty(resolvedPath))
                {
                    try
                    {
                        // 检查目录是否存在，如果不存在则创建（这是一个激活操作，但不是严格验证）
                        if (!Directory.Exists(resolvedPath))
                        {
                            PrintInfoLog($"文件存储目录不存在，正在创建: {resolvedPath}");
                            Directory.CreateDirectory(resolvedPath);
                            PrintInfoLog($"文件存储目录创建成功: {resolvedPath}");
                        }



                        PrintInfoLog($"文件存储路径验证成功: {resolvedPath}");
                    }
                    catch (Exception ex)
                    {
                        // 此处的异常处理主要用于日志记录，基本验证已经通过
                        PrintInfoLog($"文件路径操作时出现警告: {ex.Message}");
                    }
                }

                PrintInfoLog("服务器配置检查完成");
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
        /// 通过依赖注入容器的ServerConfig单例或ConfigManagerService加载配置
        /// </summary>
        private RUINORERP.Model.ConfigModel.ServerGlobalConfig GetServerConfig()
        {
            try
            {
                // 优先使用依赖注入容器的ServerConfig单例（在Startup.cs中配置）
                var serverConfig = Startup.GetFromFac<ServerGlobalConfig>();

                // 如果需要环境变量解析或其他后处理，使用ConfigManagerService
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
                PrintErrorLog($"服务器配置加载失败: {ex.Message}");
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
                PrintInfoLog("服务器已经启动或正在启动中。请避免重复操作");
                return;
            }

            try
            {
                // 检查服务器配置
                if (!CheckServerConfiguration())
                {
                    PrintErrorLog("服务器配置检查失败，启动已取消");
                    ShowTabPage("系统配置");
                    return;
                }

                // 立即禁用启动按钮，防止重复点击
                SetServerButtonsEnabled(false);

                // 检查系统注册状态
                if (!await ValidateSystemRegistrationAsync())
                {
                    PrintErrorLog("系统注册验证失败，服务器启动被终止");
                    throw new Exception("系统注册验证失败，无法启动服务器");
                }

                PrintInfoLog("服务器启动中...");


                // 启动核心服务
                await StartServerCore();

                PrintInfoLog("服务器启动完成");

                // 根据配置决定启动时是否加载缓存
                var serverConfig = GetServerConfig();
                if (serverConfig != null && serverConfig.LoadCacheOnStartup)
                {
                    // 服务器启动后异步加载缓存，不阻塞UI
                    PrintInfoLog("开始异步加载缓存数据...");
                    // 使用ConfigureAwait(false)避免死锁风险
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // 记录开始时间用于性能分析
                            var startTime = DateTime.Now;

                            // 执行缓存初始化
                            await _entityCacheInitializationService.InitializeAllCacheAsync().ConfigureAwait(false);

                            // 计算耗时并记录完成信息
                            var elapsedTime = DateTime.Now - startTime;
                            this.BeginInvoke(new Action(() =>
                            {
                                PrintInfoLog($"缓存数据加载完成，耗时: {elapsedTime.TotalSeconds:F2}秒");
                            }));
                        }
                        catch (Exception ex)
                        {
                            // 记录更详细的错误
                            this.BeginInvoke(new Action(() =>
                            {
                                PrintErrorLog($"加载缓存数据时发生错误: {ex.Message}");
                                // 对于重要错误记录更详细的信息
                                PrintErrorLog($"异常类型: {ex.GetType().Name}");

                                // 如果有内部异常则记录
                                if (ex.InnerException != null)
                                {
                                    PrintErrorLog($"内部异常: {ex.InnerException.Message}");
                                }
                            }));
                        }
                    });
                }
                else if (serverConfig != null)
                {
                    PrintInfoLog("根据配置，启动时不执行缓存数据加载");
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"启动服务器时发生错误: {ex.Message}");

                // 发生错误时重新激活启动按钮
                SetServerButtonsEnabled(true, false);

                // 显示更友好的错误消息，特别是端口占用情况
                string errorMessage = ex.Message;
                string errorTitle = "错误";

                // 检查是否是端口占用错误，提供更友好的显示
                if (ex.Message.Contains("端口已被占用") || ex.Message.Contains("端口") && ex.Message.Contains("占用"))
                {
                    errorTitle = "端口占用错误";
                    // 端口占用错误已经包含详细解决方案，直接显示
                }
                else if (ex is InvalidOperationException)
                {
                    errorTitle = "配置错误";
                }

                MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("服务器已成功停止", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                PrintErrorLog($"停止服务器时发生错误: {ex.Message}");
                MessageBox.Show($"停止服务器时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                PrintInfoLog("配置重新加载完成");
                MessageBox.Show("配置重新加载完成", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                PrintErrorLog($"重新加载配置时发生错误: {ex.Message}");
                MessageBox.Show($"重新加载配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DialogResult result = MessageBox.Show("确定要退出应用程序吗？", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 停止服务器
                    await StopServerAsync();

                    // 关闭主窗口
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"退出应用程序时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭所有选项卡页面
        /// </summary>
        private void CloseAllTabs()
        {
            try
            {
                // 确认关闭
                if (tabControlMain.TabPages.Count > 1)
                {
                    DialogResult result = MessageBox.Show($"确定要关闭所有 {tabControlMain.TabPages.Count - 1} 个选项卡页面吗？", "关闭确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // 关闭除第一个选项卡页面外的所有页面
                        for (int i = tabControlMain.TabPages.Count - 1; i > 0; i--)
                        {
                            tabControlMain.TabPages.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("没有可关闭的选项卡页面", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭选项卡页面时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// 刷新当前选项卡页面
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新页面时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据选项卡页面名称创建对应的内容控件
        /// </summary>
        /// <param name="tabName">选项卡页面名称</param>
        /// <returns>内容控件</returns>
        private Control CreateContentControl(string tabName)
        {
            switch (tabName)
            {
                case "服务器监控":
                    // 创建服务器监控控件实例，并传入正在运行的NetworkServer实例
                    var serverMonitorControl = new ServerMonitorControl(_networkServer);
                    serverMonitorControl.Dock = DockStyle.Fill;
                    return serverMonitorControl;
                case "用户管理":
                    // 创建用户管理控件实例
                    var userManagementControl = new UserManagementControl();
                    userManagementControl.Dock = DockStyle.Fill;
                    return userManagementControl;
                case "缓存管理":
                    // 在这里创建缓存管理控件
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
                case "锁定数据管理":
                    // 创建锁定数据管理控件实例
                    var dataViewerControl = new LockDataViewerControl();
                    dataViewerControl.Dock = DockStyle.Fill;
                    return dataViewerControl;
                case "注册信息":
                    var registrationManagementControl = new RegistrationManagementControl();
                    registrationManagementControl.Dock = DockStyle.Fill;
                    return registrationManagementControl;
                case "序列管理":
                    var sequenceManagementControl = new Controls.SequenceManagementControl();
                    sequenceManagementControl.Dock = DockStyle.Fill;
                    return sequenceManagementControl;
                case "文件管理":
                    // 创建文件管理控件实例
                    var fileManagementControl = new Controls.FileManagementControl();
                    fileManagementControl.Dock = DockStyle.Fill;
                    return fileManagementControl;
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

        private async void frmMainNew_Load(object sender, EventArgs e)
        {
            // 检查系统注册状态
            CheckSystemRegistration();

            // 初始化界面
            InitializeUI();

            Initialize();

            // 主窗体初始化完成后，调度工作流（避免空引用异常）
            await ScheduleWorkflowsAfterInitialization();
        }

        /// <summary>
        /// 主窗体初始化完成后调度工作流
        /// </summary>
        private async Task ScheduleWorkflowsAfterInitialization()
        {
            try
            {
                PrintInfoLog("开始调度工作流...");

                // 配置库存快照工作流执行时间（方便调试和测试）
                //#if DEBUG
                //// 调试模式：启用调试模式，每5分钟执行一次
                //InventorySnapshotWorkflowConfig.DebugMode = false;
                //InventorySnapshotWorkflowConfig.DebugExecutionIntervalMinutes = 5;

                //// 或者设置特定执行时间（例如：当前时间+2分钟）
                //// var now = DateTime.Now;
                //// var debugExecutionTime = now.AddMinutes(2);
                //// InventorySnapshotWorkflowConfig.DailyExecutionTime = new TimeSpan(debugExecutionTime.Hour, debugExecutionTime.Minute, debugExecutionTime.Second);
                //#else
                //// 生产模式：默认凌晨1点执行
                InventorySnapshotWorkflowConfig.DailyExecutionTime = new TimeSpan(1, 0, 0);
                //#endif

                // 调度工作流
                await SafetyStockWorkflowConfig.ScheduleDailySafetyStockCalculation(WorkflowHost);
                await InventorySnapshotWorkflowConfig.ScheduleInventorySnapshot(WorkflowHost);
                await TempImageCleanupWorkflowConfig.ScheduleTempImageCleanup(WorkflowHost);

                PrintInfoLog("工作流调度完成");
            }
            catch (Exception ex)
            {
                PrintInfoLog($"工作流调度失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"工作流调度错误: {ex.Message}");
            }
        }

        #region 已注册的缓存类型


        public static void Initialize()
        {
            // 预注册常用类型
            TypeResolver.PreRegisterCommonTypes();

            // 注册当前程序集的所有类型
            Assembly.GetExecutingAssembly().RegisterAllTypesFromAssembly();

            // 注册相关的其他程序集
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
            UpdateServerStatus();
            // 刷新一次数据
            RefreshCurrentTab();
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

                // 更新状态栏中的服务器信息
                toolStripStatusLabelServerStatus.Text = $"服务状态: {serverInfo.Status}";
                toolStripStatusLabelConnectionCount.Text = $"连接数: {serverInfo.CurrentConnections}/{serverInfo.MaxConnections}";
                toolStripStatusLabelMessage.Text = $"服务器IP: {serverInfo.ServerIp}, 端口: {serverInfo.Port}";
            }
            catch (Exception ex)
            {
                PrintInfoLog("更新服务器状态时发生错误: " + ex.Message);
            }
        }

        public void PrintInfoLog(string msg)
        {
            if (IsIISProcess()) return;

            SafeLogOperation(msg, Color.Black);
        }

        /// <summary>
        /// 检查是否为IIS进程 - 安全轻量级实现
        /// </summary>
        /// <returns>是否为IIS进程</returns>
        private bool IsIISProcess()
        {
            try
            {
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                if (currentProcess == null)
                    return false;

                ProcessModule mainModule = currentProcess.MainModule;
                if (mainModule == null)
                    return false;

                string moduleName = mainModule.FileName.ToLower();
                return moduleName.Contains("iis") || moduleName.Contains("w3wp") || moduleName.Contains("aspnet");
            }
            catch (Exception)
            {
                // 忽略所有异常，确保此检查不会影响主程序
                return false;
            }
        }

        /// <summary>
        /// 执行安全的日志操作 - 确保作为次要功能不影响主程序运行
        /// </summary>
        public void SafeLogOperation(string msg, Color color)
        {
            // 最外层的安全检查 - 快速失败
            if (string.IsNullOrWhiteSpace(msg)) return;
            if (IsIISProcess()) return;

            try
            {
                // 确保格式化消息有效且安全
                string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {msg}\r\n";

                // 严格限制消息长度，防止过大的消息导致问题
                if (formattedMsg.Length > 500)
                {
                    formattedMsg = formattedMsg.Substring(0, 497) + "...\r\n";
                }

                // 使用轻量级的线程池任务处理日志操作，避免阻塞调用线程
                Task.Run(() =>
                {
                    try
                    {
                        // 将Color转换为LogLevel
                        LogLevel logLevel;
                        if (color == Color.Red)
                        {
                            logLevel = LogLevel.Error;
                        }
                        else if (color == Color.Yellow)
                        {
                            logLevel = LogLevel.Warning;
                        }
                        else // Color.Green, Color.Blue或其他颜色
                        {
                            logLevel = LogLevel.Information;
                        }

                        // 优先记录到文件日志 - 更可靠
                        LogToFile(formattedMsg, logLevel);

                        // 仅当UI日志启用时，才尝试记录到UI控件
                        if (_uiLoggingEnabled)
                        {
                            TryUILogging(formattedMsg, color);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 捕获所有异常，确保日志操作不会影响主程序
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"日志后台处理错误: {ex.Message}");
                        }
                        catch { }
                    }
                });
            }
            catch (Exception)
            {
                // 最外层异常处理 - 确保绝对不会影响主程序
                // 不记录任何错误，避免级联问题
            }
        }

        // 标记UI日志是否可用的标志
        private volatile bool _uiLoggingEnabled = true;

        // 日志生产者-消费者队列相关
        private readonly BlockingCollection<string> _uiLogQueue = new BlockingCollection<string>(new ConcurrentQueue<string>(), 1000);
        private CancellationTokenSource _uiLogCts;
        private const int UI_LOG_BATCH_INTERVAL = 200; // 日志批次处理间隔（毫秒）
        private const int UI_LOG_MAX_BATCH_SIZE = 4096; // 最大日志批次大小

        /// <summary>
        /// 启动UI日志处理泵
        /// </summary>
        private void StartUiLogPump()
        {
            if (_uiLogCts != null)
                return;

            _uiLogCts = new CancellationTokenSource();
            var ct = _uiLogCts.Token;

            Task.Factory.StartNew(async () =>
            {
                var sb = new StringBuilder();
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        string msg;
                        // 等待新消息，超时后批量刷新
                        if (!_uiLogQueue.TryTake(out msg, UI_LOG_BATCH_INTERVAL, ct))
                        {
                            if (sb.Length > 0)
                            {
                                var batch = sb.ToString();
                                sb.Clear();
                                AppendLogToUi(batch);
                            }
                            continue;
                        }

                        sb.Append(msg);
                        // 继续尝试拉取以批量合并（短时间窗口）
                        while (_uiLogQueue.TryTake(out msg))
                        {
                            sb.Append(msg);
                            if (sb.Length > UI_LOG_MAX_BATCH_SIZE)
                                break;
                        }

                        var batch2 = sb.ToString();
                        sb.Clear();
                        AppendLogToUi(batch2);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch
                    {
                        // 忽略异常，确保日志处理泵不会崩溃
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 停止UI日志处理泵
        /// </summary>
        private void StopUiLogPump()
        {
            try
            {
                _uiLogCts?.Cancel();
                _uiLogCts?.Dispose();
                _uiLogCts = null;
            }
            catch
            {
                // 忽略异常
            }
        }

        /// <summary>
        /// 将日志批量追加到UI控件
        /// </summary>
        private void AppendLogToUi(string batch)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(() => AppendLogToUi(batch)));
                }
                catch
                {
                    // 窗体已关闭，忽略异常
                }
                return;
            }

            if (richTextBoxLog == null || richTextBoxLog.IsDisposed || !richTextBoxLog.IsHandleCreated)
                return;

            try
            {
                // 最小化本机richedit操作，提高性能
                richTextBoxLog.SuspendLayout();
                richTextBoxLog.AppendText(batch);
                richTextBoxLog.ScrollToCaret();

                // 确保日志行数不超过限制
                EnsureMaxLinesSafe(richTextBoxLog, 500);
            }
            catch (Exception)
            {
                // 如果出现严重错误，禁用UI日志
                _uiLoggingEnabled = false;
            }
            finally
            {
                try
                {
                    richTextBoxLog.ResumeLayout();
                }
                catch
                {
                    // 忽略异常
                }
            }
        }

        /// <summary>
        /// 安全地将日志消息加入UI日志队列
        /// </summary>
        private void SafeLogEnqueue(string formattedMsg)
        {
            if (!_uiLoggingEnabled)
                return;

            if (!_uiLogQueue.TryAdd(formattedMsg))
            {
                // 队列满时回退到文件记录
                LogToFile(formattedMsg, LogLevel.Information);
            }
        }

        /// <summary>
        /// 尝试使用UI控件进行日志记录
        /// </summary>
        /// <returns>是否成功记录日志</returns>
        private bool TryUILogging(string logMessage, Color color)
        {
            // 首先检查UI日志是否已被禁用
            if (!_uiLoggingEnabled)
                return false;
            try
            {
                // 将日志消息加入UI日志队列
                SafeLogEnqueue(logMessage);
                return true;
            }
            catch (ObjectDisposedException)
            {
                // 控件已被释放，禁用UI日志
                _uiLoggingEnabled = false;
                return false;
            }
            catch (InvalidOperationException)
            {
                // 操作无效，禁用UI日志
                _uiLoggingEnabled = false;
                return false;
            }
            catch (Exception)
            {
                // 任何其他异常都返回失败
                return false;
            }
        }

        /// <summary>
        /// 在UI线程上执行实际的日志记录操作
        /// </summary>
        /// <param name="logMessage">日志消息</param>
        /// <returns>是否成功记录日志</returns>
        private bool PerformUILogging(string logMessage)
        {
            try
            {
                // 再次检查控件状态 - 双重保险
                if (richTextBoxLog == null || richTextBoxLog.IsDisposed || !richTextBoxLog.IsHandleCreated || IsDisposed)
                {
                    _uiLoggingEnabled = false;
                    return false;
                }

                // 安全地限制日志行数
                EnsureMaxLinesSafe(richTextBoxLog, 500);

                // 安全地追加文本 - 使用更安全的方式
                lock (richTextBoxLog) // 防止多线程同时访问
                {
                    try
                    {
                        richTextBoxLog.AppendText(logMessage);
                    }
                    catch (AccessViolationException)
                    {
                        // 严重的内存访问异常，禁用UI日志
                        _uiLoggingEnabled = false;
                        return false;
                    }
                }

                // 尝试滚动到末尾，但失败也不影响
                try { richTextBoxLog.ScrollToCaret(); } catch { }

                return true;
            }
            catch (AccessViolationException)
            {
                // 严重的内存访问异常，禁用UI日志
                _uiLoggingEnabled = false;
                return false;
            }
            catch (Exception)
            {
                // 其他UI异常，返回失败
                return false;
            }
        }

        // TryAppendTextSafe方法已被TryUILogging替代，不再使用

        /// <summary>
        /// 当日志控件不可用时，将日志记录到文件 - 轻量级实现
        /// </summary>
        private void LogToFile(string logMessage, LogLevel logLevel)
        {
            try
            {
                // 简化日志目录和文件名，减少IO操作
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

                // 仅在目录不存在时创建，减少系统调用
                if (!Directory.Exists(logDir))
                {
                    try { Directory.CreateDirectory(logDir); } catch { return; } // 创建失败则直接返回
                }

                string logFile = Path.Combine(logDir, $"ServerLog_{DateTime.Now:yyyyMMdd}.txt");

                // 根据日志级别确定显示文本
                string level = logLevel switch
                {
                    LogLevel.Error => "[错误] ",
                    LogLevel.Warning => "[警告] ",
                    LogLevel.Information => "[信息] ",
                    LogLevel.Debug => "[调试] ",
                    LogLevel.Trace => "[跟踪] ",
                    _ => "[日志] "
                };

                // 添加进程和线程信息，便于诊断
                string processInfo = $"[PID:{Process.GetCurrentProcess().Id}] [TID:{Thread.CurrentThread.ManagedThreadId}] ";
                string fullLogMessage = $"{level}{processInfo}{logMessage}";

                // 使用更安全的文件写入方式
                // 使用FileStream和StreamWriter，设置FileShare选项以允许其他进程读取日志文件
                using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(fullLogMessage);
                }
            }
            catch (Exception)
            {
                // 忽略所有异常，确保文件日志不会影响主程序
                // 不再向控制台输出错误，避免级联问题
            }
        }

        /// <summary>
        /// 将错误日志输出到主窗口的RichTextBox控件并以红色文本显示
        /// </summary>
        /// <param name="msg">错误消息内容</param>
        public void PrintErrorLog(string msg)
        {
            if (IsIISProcess()) return;

            SafeLogOperation($"[错误] {msg}", Color.Red);
        }

        /// <summary>
        /// 安全地限制RichTextBox的行数 - 避免使用可能导致AccessViolation的操作
        /// </summary>
        private void EnsureMaxLinesSafe(RichTextBox rtb, int maxLines)
        {
            // 最安全的实现 - 完全避免可能导致AccessViolation的复杂操作
            try
            {
                // 检查控件状态 - 更严格的检查
                if (rtb == null || rtb.IsDisposed || !rtb.IsHandleCreated || IsDisposed)
                    return;

                // 使用文本长度作为判断依据，避免使用GetLineFromCharIndex等可能导致问题的方法
                const int estimatedCharsPerLine = 100;
                const int maxChars = 5000; // 大约50行，保守估计

                // 只在文本长度超过阈值时进行处理
                if (rtb.TextLength > maxChars)
                {
                    try
                    {
                        // 使用更安全的方式：清空整个控件
                        // 这种方式避免了任何可能引发AccessViolation的字符串操作
                        rtb.Clear();
                        // 可以选择添加一条提示信息
                        rtb.AppendText("[日志已自动清空，只显示最新内容]\r\n");
                    }
                    catch (AccessViolationException)
                    {
                        // 如果清空操作也引发AccessViolation，禁用UI日志
                        _uiLoggingEnabled = false;
                    }
                }
            }
            catch (AccessViolationException)
            {
                // 特别捕获AccessViolationException，这是最危险的异常
                // 在这种情况下，我们应该完全放弃UI日志
                _uiLoggingEnabled = false;
            }
            catch (ObjectDisposedException)
            {
                // 控件已被释放，禁用UI日志
                _uiLoggingEnabled = false;
            }
            catch (Exception)
            {
                // 忽略所有其他异常，确保此操作不会影响主程序
            }
        }

        private async void frmMainNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 禁止新的UI日志，停止泵
            _uiLoggingEnabled = false;
            StopUiLogPump();

            await ShutdownAsync();
        }


        IHost host;
        /// <summary>
        /// 服务器核心逻辑启动
        /// </summary>
        private async Task StartServerCore()
        {
            PrintInfoLog("StartServerCore Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            try
            {

                var _logger = Startup.GetFromFac<ILogger<frmMainNew>>();

                // 从依赖注入容器获取NetworkServer实例
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
                    _logger?.LogInformation($"ERP网络服务器启动成功");
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

                    // 启动服务器锁管理器服务 - 直接通过接口调用，无需类型转换
                    var lockManager = Startup.GetFromFac<ILockManagerService>();
                    if (lockManager != null)
                    {
                        await Task.Run(async () => await lockManager.StartAsync());
                    }

                    // 每5秒检查一次，减少系统负载
                    if (_sessionCleanupTimer != null)
                    {
                        _sessionCleanupTimer.Dispose();
                    }
                    _sessionCleanupTimer = new System.Threading.Timer(CheckAndRemoveExpiredSessions, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                    // 加载提醒数据
                    DataServiceChannel loadService = Startup.GetFromFac<DataServiceChannel>();
                    await loadService.LoadCRMFollowUpPlansData(ReminderBizDataList);

                    // 启动提醒工作流调度器
                    _reminderScheduler = new RUINORERP.Server.Workflow.ReminderWorkflowScheduler(
                        WorkflowHost,
                        loadService,
                        Startup.GetFromFac<Microsoft.Extensions.Logging.ILogger<RUINORERP.Server.Workflow.ReminderWorkflowScheduler>>()
                    );
                    _reminderScheduler.Start();
                    PrintInfoLog("提醒工作流调度器已启动");

                    // 启动每日执行的定时任务
                    GlobalScheduledData globalScheduled = new GlobalScheduledData();
                    var DailyworkflowId = await WorkflowHost.StartWorkflow("NightlyWorkflow", 1, globalScheduled);
                    PrintInfoLog($"NightlyWorkflow每日任务启动{DailyworkflowId}.");

                    // 初始化配置
                    await Task.Run(async () => await InitConfig(false));

                    PrintInfoLog("服务器核心启动完成");
                }
                else
                {
                    // 启动失败
                    throw new Exception("网络服务器返回null，启动失败。");
                }
            }
            catch (Exception hostex)
            {
                _logger?.LogError($"NetworkServer启动异常: {hostex.Message}", hostex);
                PrintErrorLog($"NetworkServer启动异常: {hostex.Message}");
                throw; // 重新抛出异常以便上层处理
            }
        }

        /// <summary>
        /// 服务器关闭
        /// </summary>
        public async Task ShutdownAsync()
        {
            try
            {
                // 1. 立即禁用UI日志，防止后续操作触发AccessViolationException
                _uiLoggingEnabled = false;
                StopUiLogPump();  // 确保日志泵被停止
                PrintInfoLog("UI日志已禁用");

                // 2. 关闭NetworkServer
                if (_networkServer != null)
                {
                    await _networkServer.StopAsync();
                    _networkServer = null;
                }

                // 3. 清理定时器资源
                if (_serverInfoTimer != null)
                {
                    _serverInfoTimer.Stop();
                    _serverInfoTimer.Dispose();
                    _serverInfoTimer = null;
                }

                // 4. 清理会话清理定时器资源
                if (_sessionCleanupTimer != null)
                {
                    _sessionCleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _sessionCleanupTimer.Dispose();
                    _sessionCleanupTimer = null;
                }

                // 5. 停止并清理内存监控服务
                if (_memoryMonitoringService != null)
                {
                    // 移除事件订阅
                    _memoryMonitoringService.MemoryUsageWarning -= OnMemoryUsageWarning;
                    _memoryMonitoringService.MemoryUsageCritical -= OnMemoryUsageCritical;
                    // 释放资源
                    _memoryMonitoringService.Dispose();
                    _memoryMonitoringService = null;
                }
            }
            catch (Exception e)
            {
                // 使用文件日志记录错误，因为UI日志可能已禁用
                LogToFile($"SocketServer关闭失败: {e.Message}", LogLevel.Error);
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
            System.Diagnostics.Debug.WriteLine("Ctrl + P 被按下");

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
                    .AddDistributedMemoryCache() // 为测试使用内存实现模拟，而不使用redis等
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

                    PrintInfoLog($"缓存初始化完成, 执行时间: {executionTime.TotalSeconds:F2} 秒");
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"初始化配置失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 检查并移除过期会话
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
                        PrintInfoLog($"已清理 {cleanedCount} 个超时会话");
                    }
                }
            }
            catch (Exception ex)
            {
                PrintErrorLog($"检查过期会话时发生错误: {ex.Message}");
            }
        }


        /// <summary>
        /// 检查系统注册状态并加载注册信息
        /// </summary>
        private async Task CheckSystemRegistration()
        {
            try
            {
                PrintInfoLog("正在检查系统注册状态...");

                // 检查是否处于调试模式
                if (IsDebug)
                {
                    PrintInfoLog("系统处于调试模式，跳过注册状态检查");
                    return;
                }

                // 从依赖注入容器获取注册服务
                var registrationService = Startup.GetFromFac<IRegistrationService>();
                if (registrationService == null)
                {
                    PrintErrorLog("无法获取注册服务，注册检查失败");
                    return;
                }

                // 从数据库获取注册信息
                var registrationInfo = await registrationService.GetRegistrationInfoAsync();
                if (registrationInfo == null)
                {
                    PrintErrorLog("无法从数据库获取注册信息");
                    return;
                }

                // 将注册信息赋值给实例变量
                frmMainNew.Instance.registrationInfo = registrationInfo;

                // 检查注册状态
                bool isRegistered = registrationService.CheckRegistered(registrationInfo);

                if (isRegistered)
                {
                    PrintInfoLog($"系统注册验证成功，许可用户数: {registrationInfo.ConcurrentUsers}");
                    PrintInfoLog($"注册到期时间: {registrationInfo.ExpirationDate:yyyy-MM-dd HH:mm:ss}");

                    // 检查是否即将过期（7天内）
                    var daysUntilExpiration = registrationInfo.ExpirationDate - DateTime.Now;
                    if (daysUntilExpiration.TotalDays <= 7 && daysUntilExpiration.TotalDays > 0)
                    {
                        PrintInfoLog($"警告：注册许可将在 {daysUntilExpiration.TotalDays:F0} 天后到期");
                    }
                }
                else
                {
                    PrintErrorLog("系统未注册或注册已过期，请及时进行系统注册");

                    // 检查是否过期
                    if (registrationService.IsRegistrationExpired(registrationInfo))
                    {
                        PrintErrorLog("注册许可已过期");
                    }
                    else if (!registrationInfo.IsRegistered)
                    {
                        PrintErrorLog("系统尚未注册");
                    }
                }

                // 记录功能模块信息
                if (!string.IsNullOrEmpty(registrationInfo.FunctionModule))
                {
                    try
                    {
                        //string decryptedModules = EncryptionHelper.AesDecryptByHashKey(registrationInfo.FunctionModule, "FunctionModule");
                        string decryptedModules = registrationInfo.FunctionModule;
                        PrintInfoLog($"已授权功能模块: {decryptedModules}");
                    }
                    catch (Exception ex)
                    {
                        PrintErrorLog($"解析功能模块信息失败: {ex.Message}");
                    }
                }

                PrintInfoLog("系统注册状态检查完成");
            }
            catch (Exception ex)
            {
                PrintErrorLog($"检查系统注册状态时发生错误: {ex.Message}");
                _logger?.LogError(ex, "检查系统注册状态失败");
            }
        }

        /// <summary>
        /// 验证系统注册状态（用于服务器启动时的严格验证）
        /// </summary>
        /// <returns>注册验证是否通过</returns>
        private async Task<bool> ValidateSystemRegistrationAsync()
        {
            try
            {
                PrintInfoLog("正在执行服务器启动时的注册验证...");

                // 检查是否处于调试模式
                if (IsDebug)
                {
                    PrintInfoLog("系统处于调试模式，跳过注册严格验证");
                    return true;
                }

                // 从依赖注入容器获取注册服务
                var registrationService = Startup.GetFromFac<IRegistrationService>();
                if (registrationService == null)
                {
                    PrintErrorLog("无法获取注册服务，注册验证失败");
                    return false;
                }

                // 从数据库获取注册信息
                var registrationInfo = await registrationService.GetRegistrationInfoAsync();
                if (registrationInfo == null)
                {
                    PrintErrorLog("无法从数据库获取注册信息，注册验证失败");
                    return false;
                }

                // 将注册信息赋值给实例变量
                frmMainNew.Instance.registrationInfo = registrationInfo;

                /*
                // 生成机器码
                string machineCode = _registrationService.CreateMachineCode(registrationInfo);
                //根据注册信息实时生成机器码与注册信息中的机器码比较检测
                if (!machineCode.Equals(registrationInfo.MachineCode))
                {
                    PrintErrorLog("机器码检测失败，服务器无法启动");
                    MessageBox.Show("机器码检测失败,如果您有更换过主机，请联系软件提供商更新注册信息。",
                                  "机器码检测失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                */


                // 执行严格的注册验证
                bool isRegistered = registrationService.CheckRegistered(registrationInfo);

                if (!isRegistered)
                {
                    if (registrationService.IsRegistrationExpired(registrationInfo))
                    {
                        PrintErrorLog("注册许可已过期，服务器无法启动");
                        MessageBox.Show("系统注册许可已过期，请联系软件提供商续期。",
                                      "注册过期", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!registrationInfo.IsRegistered)
                    {
                        PrintErrorLog("系统未注册，服务器无法启动");
                        MessageBox.Show("系统未注册，请先进行系统注册。",
                                      "未注册", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        PrintErrorLog("注册信息验证失败，服务器无法启动");
                        MessageBox.Show("注册信息验证失败，请检查注册码是否正确。",
                                      "注册验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }

                // 验证用户数限制
                if (registrationInfo.ConcurrentUsers <= 0)
                {
                    PrintErrorLog("注册许可的用户数配置无效，服务器无法启动");
                    MessageBox.Show("注册许可的并发用户数配置无效。",
                                  "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 检查注册到期时间
                var daysUntilExpiration = registrationInfo.ExpirationDate - DateTime.Now;
                if (daysUntilExpiration.TotalDays <= 0)
                {
                    PrintErrorLog("注册许可已过期，服务器无法启动");
                    MessageBox.Show("系统注册许可已过期，请联系软件提供商续期。",
                                  "注册过期", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (daysUntilExpiration.TotalDays <= 7)
                {
                    PrintInfoLog($"警告：注册许可将在 {daysUntilExpiration.TotalDays:F0} 天后到期");
                }

                PrintInfoLog($"系统注册验证成功，许可用户数: {registrationInfo.ConcurrentUsers}");
                PrintInfoLog($"注册到期时间: {registrationInfo.ExpirationDate:yyyy-MM-dd HH:mm:ss}");

                // 记录功能模块信息
                if (!string.IsNullOrEmpty(registrationInfo.FunctionModule))
                {
                    try
                    {
                        //string decryptedModules = EncryptionHelper.AesDecryptByHashKey(registrationInfo.FunctionModule, "FunctionModule");
                        string decryptedModules = registrationInfo.FunctionModule;
                        PrintInfoLog($"已授权功能模块: {decryptedModules}");
                    }
                    catch (Exception ex)
                    {
                        PrintErrorLog($"解析功能模块信息失败: {ex.Message}");
                    }
                }

                PrintInfoLog("服务器启动时的注册验证完成");
                return true;
            }
            catch (Exception ex)
            {
                PrintErrorLog($"执行注册验证时发生错误: {ex.Message}");
                _logger?.LogError(ex, "服务器启动时注册验证失败");
                MessageBox.Show($"执行注册验证时发生错误: {ex.Message}",
                              "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 唯一硬件信息
        /// </summary>
        public string UniqueHarewareInfo { get; set; }


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
        /// 锁定数据管理按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonDataViewer_Click(object sender, EventArgs e)
        {
            ShowTabPage("锁定数据管理");
        }

        /// <summary>
        /// 序列管理导航按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonSequenceManagement_Click(object sender, EventArgs e)
        {
            ShowTabPage("序列管理");
        }

        /// <summary>
        /// 文件管理按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonFileManagement_Click(object sender, EventArgs e)
        {
            ShowTabPage("文件管理");
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
        /// 创建锁定数据管理控件
        /// </summary>
        /// <returns>锁定数据管理控件</returns>
        private Control CreateDataViewerControl()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = "锁定数据管理功能开发中...";
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
        /// 服务器启动菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await StartServerAsync();
        }

        /// <summary>
        /// 服务器停止菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await StopServerAsync();
        }

        /// <summary>
        /// 配置重新加载菜单项点击事件
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
        /// 序列管理菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void sequenceManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("序列管理");
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
        /// 锁定数据管理菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void dataViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("锁定数据管理");
        }

        /// <summary>
        /// 全部关闭菜单项点击事件
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
        /// 服务器启动工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void toolStripButtonStartServer_Click(object sender, EventArgs e)
        {
            await StartServerAsync();
        }

        /// <summary>
        /// 服务器停止工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void toolStripButtonStopServer_Click(object sender, EventArgs e)
        {
            await StopServerAsync();
        }

        /// <summary>
        /// 数据刷新工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonRefreshData_Click(object sender, EventArgs e)
        {
            var logger = Startup.GetFromFac<ILogger<frmMainNew>>();
            logger.LogInformation("刷新数据3");
            logger.Info("刷新数据4");
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
        /// 序列管理工具栏按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonSequenceManagement_Click(object sender, EventArgs e)
        {
            ShowTabPage("序列管理");
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

        private async void toolStripButtonSystemCheck_Click(object sender, EventArgs e)
        {
            ServerBizCodeGenerateService _bizCodeService = Startup.GetFromFac<ServerBizCodeGenerateService>();
            var ss = await _bizCodeService.GenerateBizBillNoAsync(BizType.请购单);
            PrintInfoLog(ss);
            _logger.LogInformation("Information");
            _logger.Debug("Debug");
            _logger.Warn("Warn");
            _logger.LogError("error");

            var monitor = Startup.GetFromFac<IOptionsMonitor<ServerGlobalConfig>>();

            var systemconfig = Startup.GetFromFac<ServerGlobalConfig>();
        }

        /// <summary>
        /// 更新Log4Net的BufferSize
        /// </summary>
        /// <param name="bufferSize">新的BufferSize值</param>
        private static void UpdateLogBufferSize(int bufferSize)
        {
            try
            {
                // 获取日志存储库
                var loggerRepository = log4net.LogManager.GetRepository("RUINORERP_Shared_LoggerRepository");
                if (loggerRepository != null)
                {
                    // 获取所有appender
                    var appenders = loggerRepository.GetAppenders();
                    foreach (var appender in appenders)
                    {
                        // 如果是AdoNetAppender，则更新BufferSize
                        if (appender is log4net.Appender.AdoNetAppender adoNetAppender)
                        {
                            adoNetAppender.BufferSize = bufferSize;
                            // 激活选项以应用更改
                            adoNetAppender.ActivateOptions();
                            System.Diagnostics.Debug.WriteLine($"Log4Net BufferSize已更新为{bufferSize}。");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新Log4Net BufferSize时发生错误: {ex.Message}");
            }
        }
    }
}