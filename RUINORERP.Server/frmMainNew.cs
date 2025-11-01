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
using RUINORERP.Server.Services.BizCode;
using RUINORERP.Server.BNR;
using TextBox = System.Windows.Forms.TextBox;
using Button = System.Windows.Forms.Button;

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
        /// 全局日志级别控制器
        /// </summary>
        private static LogLevel _currentLogLevel = LogLevel.Error;

        /// <summary>
        /// 日志级别菜单项列表
        /// </summary>
        private static int _batchUpdateThreshold = 5;
        
        /// <summary>
        /// Log4Net BufferSize값
        /// </summary>
        private static int _logBufferSize = 10;

        /// <summary>
        /// 日志级别
        /// </summary>
        private List<ToolStripMenuItem> _logLevelMenuItems = new List<ToolStripMenuItem>();

        /// <summary>
        /// 批량 업데이트 임계값 메뉴항목 리스트
        /// </summary>
        private List<ToolStripMenuItem> _batchThresholdMenuItems = new List<ToolStripMenuItem>();
        
        /// <summary>
        /// BufferSize 메뉴항목 리스트
        /// </summary>
        private List<ToolStripMenuItem> _bufferSizeMenuItems = new List<ToolStripMenuItem>();

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
        /// <summary>
        /// 디버그 모드 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender">이벤트 소스</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonDebugMode_Click(object sender, EventArgs e)
        {
            // 드롭다운 메뉴를 표시하거나 디버그 모드를 직접 전환
            if (toolStripButtonDebugMode.HasDropDownItems)
            {
                // 이미 드롭다운 메뉴 항목이 있는 경우 메뉴 표시
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
        /// 网络监控按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void toolStripButtonNetworkMonitor_Click(object sender, EventArgs e)
        {
            IsNetworkMonitorEnabled = toolStripButtonNetworkMonitor.Checked;

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

        }
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
                Instance.PrintInfoLog("服务器信息更新过程中发生错误: " + ex.Message);
            }
        }

        private readonly ISessionService _sessionService;
        public IWorkflowHost WorkflowHost;
        private NetworkServer _networkServer;

        private readonly EntityCacheInitializationService _entityCacheInitializationService;

        public frmMainNew(ILogger<frmMainNew> logger, IWorkflowHost workflowHost, IOptionsMonitor<SystemGlobalConfig> config)
        {
            InitializeComponent();
            _main = this;
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _logger = logger;
            WorkflowHost = workflowHost;

            // 注入缓存初始化服务
            _entityCacheInitializationService = Program.ServiceProvider.GetRequiredService<EntityCacheInitializationService>();

            // 初始化服务器信息更新定时器
            InitializeServerInfoTimer();

            Globalconfig = config;
            // 监听配置变更
            Globalconfig.OnChange(updatedConfig =>
            {
                Console.WriteLine($"Configuration has changed: {updatedConfig.SomeSetting}");
            });

            // 初始化导航按钮事件
            InitializeNavigationButtons();

            // 初始化菜单和工具栏事件
            InitializeMenuAndToolbarEvents();

            // 로그 레벨 메뉴 초기화
            InitializeLogLevelMenu();

            // 初始化服务器监控选项卡页面（默认显示）
            InitializeDefaultTab();
        }

        /// <summary>
        /// 初始化菜单和工具栏事件
        /// </summary>
        private void InitializeMenuAndToolbarEvents()
        {
            // 注意：事件绑定已在设计器文件中完成。此处仅保留附加功能的事件绑定
            // 防止因重复绑定导致事件处理程序被多次调用

            // 工具栏事件 - 这些需要在代码中额外绑定
            toolStripButtonRefreshData.Click += (s, e) => RefreshCurrentTab();

            // 추가 컨트롤 이벤트가 필요한 경우 여기에 추가
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
                    
                    // 메뉴 항목 선택 상태 업데이트
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
                    
                    // 메뉴 항목 선택 상태 업데이트
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
                            // 일괄 업데이트 임계값 설정
                            SetBatchUpdateThreshold(threshold);
                            
                            // 메뉴 항목 선택 상태 업데이트
                            UpdateBatchThresholdMenuCheckState();
                            
                            // 임계값 변경 기록
                            PrintInfoLog($"데이터베이스 일괄 업데이트 임계값이 설정되었습니다: {threshold}");
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
                    
                    // 메뉴 항목 선택 상태 업데이트
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
                            // BufferSize 설정
                            SetLogBufferSize(bufferSize);
                            
                            // 메뉴 항목 선택 상태 업데이트
                            UpdateBufferSizeMenuCheckState();
                            
                            // BufferSize 변경 기록
                            PrintInfoLog($"로그 BufferSize가 설정되었습니다: {bufferSize}");
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
        /// 설정全局日志级别
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        public static void SetGlobalLogLevel(LogLevel logLevel)
        {
            _currentLogLevel = logLevel;
            // 记录日志级别变更
            var loggerFactory = Program.ServiceProvider.GetService<ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger<frmMainNew>();
            logger?.LogInformation($"全局日志级别已设置: {logLevel}");
            
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
                
                // 업데이트 메뉴 항목 선택 상태
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
                
                // 업데이트 메뉴 항목 선택 상태
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
            // 이미 해당 탭 페이지가 있는지 확인
            TabPage existingTabPage = null;
            foreach (TabPage tabPage in tabControlMain.TabPages)
            {
                if (tabPage.Text == tabName)
                {
                    existingTabPage = tabPage;
                    break;
                }
            }

            // 존재하지 않으면 새로운 탭 페이지 생성
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

            // 해당 탭 페이지로 전환
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

                // 구성 검증 실행 - 이제 FluentValidation 검증기를 사용합니다
                var validationResult = validationService.ValidateConfig(serverConfig);

                // 검증 결과 확인
                if (!validationResult.IsValid)
                {
                    PrintErrorLog($"配置验证失败: {validationResult.GetErrorMessage()}");
                    MessageBox.Show($"服务器配置验证失败:\n{validationResult.GetErrorMessage()}", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 注意：详细路径验证（环境变量解析、路径可访问性检查等）
                // 이미 ServerConfigValidator에서 구현되어 있으므로 여기서는 추가적인 보장 로직을 유지합니다
                PrintInfoLog("正在执行额外的文件存储路径验证...");
                IConfigManagerService configManagerService = Startup.GetFromFac<IConfigManagerService>();
                // 环境变量路径解析（作为额外的验证保障）
                string resolvedPath = configManagerService.ResolveEnvironmentVariables(serverConfig.FileStoragePath);

                if (!string.IsNullOrEmpty(resolvedPath))
                {
                    try
                    {
                        // 디렉터리 존재 여부 확인, 존재하지 않으면 생성 (이것은 활성화된 작업이지만 엄격한 검증이 아닙니다)
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
        private RUINORERP.Model.ConfigModel.ServerConfig GetServerConfig()
        {
            try
            {
                // 优先使用依赖注入容器的ServerConfig单例（在Startup.cs中配置）
                var serverConfig = Startup.GetFromFac<ServerConfig>();

                // 환경 변수 해석 또는 기타 후처리가 필요한 경우 ConfigManagerService 사용
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
            // 중복 시작 방지
            if (!toolStripButtonStartServer.Enabled)
            {
                PrintInfoLog("服务器已经启动或正在启动中。请避免重复操作");
                return;
            }

            try
            {
                // 서버 구성 확인
                if (!CheckServerConfiguration())
                {
                    PrintErrorLog("服务器配置检查失败，启动已取消");
                ShowTabPage("系统配置");
                    return;
                }

                // 즉시 시작 버튼 비활성화, 중복 클릭 방지
                SetServerButtonsEnabled(false);

                PrintInfoLog("服务器启动中...");

                // 핵심 서비스 시작
                await StartServerCore();

                PrintInfoLog("服务器启动完成");

                // 구성에 따라 시작 시 캐시 로드 여부 결정
                var serverConfig = GetServerConfig();
                if (serverConfig != null && serverConfig.LoadCacheOnStartup)
                {
                    // 服务器启动后异步加载缓存，不阻塞UI
                    PrintInfoLog("开始异步加载缓存数据...");
                    await Task.Run(async () =>
                    {
                        try
                        {
                            // 记录开始时间用于性能分析
                            var startTime = DateTime.Now;

                            // 캐시 초기화 실행
                            await _entityCacheInitializationService.InitializeAllCacheAsync();

                            // 소요 시간 계산 및 완료 정보 기록
                            var elapsedTime = DateTime.Now - startTime;
                            this.BeginInvoke(new Action(() =>
                            {
                                PrintInfoLog($"缓存数据加载完成，耗时: {elapsedTime.TotalSeconds:F2}秒");
                            }));
                        }
                        catch (Exception ex)
                        {
                            // 더 자세한 오류 기록
                            this.BeginInvoke(new Action(() =>
                            {
                                PrintErrorLog($"加载缓存数据时发生错误: {ex.Message}");
                                // 중요한 오류의 경우 더 자세한 정보 기록
                                PrintErrorLog($"异常类型: {ex.GetType().Name}");

                                // 내부 예외가 있는 경우 기록
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

                // 오류 발생 시 시작 버튼 다시 활성화
                SetServerButtonsEnabled(true, false);

                MessageBox.Show($"启动服务器时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // ShutdownAsync 메서드 호출하여 서버 중지
                await ShutdownAsync();

                // 타이머 중지
                _serverInfoTimer?.Stop();

                // UI 상태 업데이트
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

                    MessageBox.Show($"{tabName}页面已刷新", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                case "数据查看":
                    // 创建数据查看控件实例
                    var dataViewerControl = new DataViewerControl();
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

            // 刷新一次数据
            RefreshData();
        }

        /// <summary>
        /// 데이터 새로 고침 - 폐기됨, RefreshCurrentTab 메서드 사용 권장
        /// </summary>
        [Obsolete("RefreshCurrentTab 메서드를 대체하여 사용하세요")]
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
                Console.WriteLine($"刷新监控数据时发生错误: {ex.Message}");
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
        /// 检查是否为IIS进程
        /// </summary>
        private bool IsIISProcess()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis");
        }

        /// <summary>
        /// 执行安全的日志操作
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
                Console.WriteLine($"日志操作时发生错误: {ex.Message}");
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

        private void EnsureMaxLines(RichTextBox rtb, int maxLines)
        {
            // 确保所有RichTextBox操作在UI线程中执行
            if (rtb.InvokeRequired)
            {
                rtb.BeginInvoke(new System.Windows.Forms.MethodInvoker(() => EnsureMaxLines(rtb, maxLines)));
                return;
            }

            try
            {
                // 计算当前行数
                int currentLines = rtb.GetLineFromCharIndex(rtb.Text.Length) + 1;

                // 如果行数超过最大限制则删除旧行
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
                // 记录错误但不抛出异常以避免影响程序
                Console.WriteLine($"EnsureMaxLines错误: {ex.Message}");
            }
        }

        private async void frmMainNew_FormClosing(object sender, FormClosingEventArgs e)
        {
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

                    // 每秒检查一次
                    System.Threading.Timer timerStatus = new System.Threading.Timer(CheckAndRemoveExpiredSessions, null, 0, 1000);

                    // 加载提醒数据
                    DataServiceChannel loadService = Startup.GetFromFac<DataServiceChannel>();
                    loadService.LoadCRMFollowUpPlansData(ReminderBizDataList);

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
                    throw new Exception("网络服务器返回null，启动失败");
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
                // 关闭NetworkServer
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
                PrintErrorLog($"SocketServer关闭失败: {e.Message}");
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
                    .AddDistributedMemoryCache() // 为测试使用内存实现模拟，而不使用redis等
                    .AddSingleton<ICacheAdapter, MemoryCacheAdapter>()  // 캐시 어댑터 추가
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
                    _logger?.LogInformation($"缓存初始化完成, 执行时间: {executionTime.TotalSeconds:F2} 秒");
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
        /// 检查注册信息
        /// </summary>
        /// <param name="regInfo">注册信息</param>
        /// <returns>注册是否有效</returns>
        public bool CheckRegistered(tb_sys_RegistrationInfo regInfo)
        {
            string key = "ruinor1234567890"; // 这应该是一个密钥
            string machineCode = regInfo.MachineCode; // 这可能是计算机的硬件信息或唯一标识符
            // 사용자가 입력한 등록 코드 가정
            string userProvidedCode = regInfo.RegistrationCode;
            bool isValid = SecurityService.ValidateRegistrationCode(userProvidedCode, key, machineCode);
            Console.WriteLine($"提供的注册码是否有效? {isValid}");
            return isValid;
        }

        /// <summary>
        /// 唯一硬件信息
        /// </summary>
        public string UniqueHarewareInfo { get; set; }

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <param name="regInfo">注册信息</param>
        /// <returns>机器码</returns>
        public string CreateMachineCode(tb_sys_RegistrationInfo regInfo)
        {
            // 指定用于生成加密机器码的关键字段
            List<string> cols = new List<string>();
            cols.Add("CompanyName");
            cols.Add("ContactName");
            cols.Add("PhoneNumber");
            cols.Add("ConcurrentUsers");
            cols.Add("ExpirationDate");
            cols.Add("ProductVersion");
            cols.Add("LicenseType");
            cols.Add("FunctionModule");

            // 지정된 열만 직렬화
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new SelectiveContractResolver(cols),
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" } }
            };

            // 객체 직렬화
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
        /// 序列管理导航按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void buttonSequenceManagement_Click(object sender, EventArgs e)
        {
            ShowTabPage("序列管理");
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
        /// <param name="e">이벤트 매개변수</param>
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
        /// 数据查看菜单项点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void dataViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTabPage("数据查看");
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

        private void toolStripButtonSystemCheck_Click(object sender, EventArgs e)
        {
            BizCodeGenerateService _bizCodeService = Startup.GetFromFac<BizCodeGenerateService>();
            var ss = _bizCodeService.GenerateBizBillNo(BizType.请购单);
            PrintInfoLog(ss);
            _logger.LogInformation("Information");
            _logger.Debug("Debug");
            _logger.Warn("Warn");
            _logger.LogError("error");

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
                            Console.WriteLine($"Log4Net BufferSize已更新为{bufferSize}。");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新Log4Net BufferSize时发生错误: {ex.Message}");
            }
        }
    }
}