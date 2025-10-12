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
using RUINORERP.Server.Comm;
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
using RUINORERP.Server.BizService;
using RUINORERP.Server.Commands;
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


using WorkflowCore.Interface;
using WorkflowCore.Primitives;
using WorkflowCore.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.Network.Core;

namespace RUINORERP.Server
{
    public partial class frmMain : Form
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
        public ILogger<frmMain> _logger { get; set; }


        //一个消息缓存列表，有处理过的。未处理的。未看的。临时性还是固定到表的？

        public Queue<ReminderData> MessageList = new Queue<ReminderData>();
        public IServiceCollection _services { get; set; }
        public IServiceProvider _ServiceProvider { get; set; }

        public bool IsDebug { get; set; } = false;

        private static frmMain _main;
        public static frmMain Instance
        {
            get { return _main; }
        }

        /// <summary>
        /// 定时器用于定期更新服务器信息
        /// </summary>
        private System.Windows.Forms.Timer UpdateServerInfoTimer = new System.Windows.Forms.Timer();

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
                        tslblStatus.Text = serverInfo.ToString();
                        // 这里可以根据实际UI控件名称进行修改
                        // 例如，如果有专门的标签显示端口、在线用户数等
                        // lblPort.Text = $"端口: {serverInfo.Port}";
                        // lblOnlineUsers.Text = $"在线会话: {serverInfo.CurrentConnections}/{serverInfo.MaxConnections}";
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

        public IWorkflowHost host;
        public frmMain(ILogger<frmMain> logger, IWorkflowHost workflowHost, IOptionsMonitor<SystemGlobalconfig> config)
        {
            InitializeComponent();
            _main = this;
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


            //ILoggerFactory loggerFactory = LoggerFactory.Create(logbuilder => logbuilder
            // .AddFilter("Microsoft", LogLevel.Debug)
            // .AddFilter("System", LogLevel.Debug)
            // .AddFilter("Namespace.Class", LogLevel.Debug)
            // //.AddFile()//成功写入到文件
            // .AddLog4Net()
            //);
            //_logger = loggerFactory.CreateLogger<frmMain>();

            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        internal ConcurrentDictionary<string, SessionforBiz> sessionListBiz = new ConcurrentDictionary<string, SessionforBiz>();

        async private void StartServerUI()
        {
            Application.DoEvents();
            // timerServerInfo.Start();
            //btnStartServer.Enabled = false;
            tsBtnStartServer.Enabled = false;
            ServerStart = true;
            try
            {
                //_logger.LogError("LogErrorLogError11");
                PrintInfoLog("开始启动服务器");
                _logger.LogInformation("开始启动socket服务器");
                // BaseKXGame.Instance.Initinal();

                //Tools.ShowMsg("RoleService Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                //准备工作 准备数据

                //智能提醒  先不启用。还没有做好
                //var inventoryMonitor = Startup.GetFromFac<SmartReminderMonitorStarter>();
                //await inventoryMonitor.StartAsync(CancellationToken.None);



                //启动socket
                frmMain.Instance.PrintInfoLog("StartServerUI Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                StartSendData();
                await StartServer();

            }
            catch (Exception ex)
            {
                _logger.Error("StartServerUI ex" + ex.Message);
                frmMain.Instance._logger.LogError("StartServerUI", ex);
                //log4netHelper.error("StartServer总异常", ex);
                _logger.LogInformation(ex, "StartServer总异常");
                Console.WriteLine("StartServer总异常" + ex.Message);
                //throw;
            }
        }


        private void StartSendData()
        {
            Task.Run(() => { StartSendService(); });
        }

        public Task StartSendService()
        {
            frmMain.Instance.PrintInfoLog("StartSendService Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //https://www.cnblogs.com/doforfuture/p/6293926.html
            while (true)
            {
                try
                {
                    Thread.Sleep(10);
                    if (frmMain.Instance.sessionListBiz.Count > 0)
                    {
                        foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                        {
                            // SessionforGame sg;
                            while (item.Value.DataQueue.Count > 0)
                            {
                                byte[] sendData;
                                bool rs = item.Value.DataQueue.TryDequeue(out sendData);
                                if (sendData != null && rs)
                                {
                                    if (sendData.Length > 338420)
                                    {

                                    }
                                    if (sendData.Length > 0)
                                    {
                                        item.Value.Send(sendData);
                                    }

                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    frmMain.Instance.PrintInfoLog(ex.Message + "StartSendService!#");
                    _logger.LogError("数据发送服务出错:Task StartSendService", ex);
                }
            }

        }

        bool ServerStart = false;
        static System.Timers.Timer timer = null;

        static System.Timers.Timer ReminderTimer = null;

        //frmUserManage frmusermange = Startup.GetFromFac<frmUserManage>();

        /// <summary>
        /// 唯一硬件信息
        /// </summary>
        public string UniqueHarewareInfo { get; set; }

        private async void frmMain_Load(object sender, EventArgs e)
        {

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;
            string UseDebug = AppSettings.GetValue("UseDebug");
            if (UseDebug == 0.ToString())
            {
                toolStripddbtnDebug.Visible = false;
            }

            try
            {

                HardwareInfoService infoService = new HardwareInfoService();
                UniqueHarewareInfo = infoService.GetHardDiskId() + infoService.GetMacAddress();

                #region 检查是否注册
                ////没有返回Null，如果结果大于1条会抛出错误
                registrationInfo = await Program.AppContextData.Db.CopyNew().Queryable<tb_sys_RegistrationInfo>().SingleAsync();
                if (registrationInfo == null)
                {
                    registrationInfo = new tb_sys_RegistrationInfo();
                    registrationInfo.PurchaseDate = DateTime.Now;
                    registrationInfo.RegistrationDate = DateTime.Now;
                    registrationInfo.Created_at = DateTime.Now;
                    registrationInfo.IsRegistered = false;
                    registrationInfo.ExpirationDate = DateTime.Now.AddDays(30);

                    //如果没有注册信息。直接跳到注册？ 或 没有任何菜单？
                    frmRegister frm = Startup.GetFromFac<frmRegister>();
                    frm.EditEntity = registrationInfo;
                    frm.MdiParent = this;
                    frm.Show();
                    frm.Activate();
                    //如果注册成功后。重新打开软件
                    toolStrip1.Visible = false;
                    menuStrip1.Visible = false;
                    return;
                }
                else
                {
                    if (registrationInfo.IsRegistered)
                    {
                        //验证基础的注册信息
                        if (CheckRegistered(registrationInfo) == false)
                        {
                            //如果注册成功后。重新打开软件
                            toolStrip1.Visible = false;
                            menuStrip1.Visible = false;
                            return;
                        }
                        else
                        {

                            //检测硬件是不是换过，利用机器来对比
                            //生成机器码前 不加密。因为注册码生成时。提供商要审核时。要看到明码
                            //要注册 使用模块这里要处理一下因为保存的是加密后的。生成注册信息（机器码时为了给注册人审核，用了解密后的信息来生成）
                            registrationInfo.FunctionModule = EncryptionHelper.AesDecryptByHashKey(registrationInfo.FunctionModule, "FunctionModule");
                            if (registrationInfo.MachineCode != frmMain.Instance.CreateMachineCode(registrationInfo))
                            {
                                //您没有购买任何模块功能或修改过授权数据，请联系管理员
                                //MessageBox.Show("硬件有变化请重新注册", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //toolStrip1.Visible = false;
                                //menuStrip1.Visible = false;
                                //return;
                            }


                            //注册成功！！！
                            //注意这里并不是直接取字段值。因为这个值会放到加密串的。明码可能会修改
                            //功能模块可以显示到UI。但是保存到DB中是加密了的。取出来到时也要解密
                            if (!string.IsNullOrEmpty(registrationInfo.FunctionModule))
                            {
                                //解密 上面已经解密了这里不再解密
                                //registrationInfo.FunctionModule = EncryptionHelper.AesDecryptByHashKey(registrationInfo.FunctionModule, "FunctionModule");

                                if (registrationInfo.FunctionModule == null)
                                {
                                    //您没有购买任何模块功能或修改过授权数据，请联系管理员
                                    MessageBox.Show("您没有购买任何模块功能或修改过授权数据，请联系管理员");
                                    Application.Exit();
                                    return;
                                }
                                //将,号隔开的枚举名称字符串变成List<GlobalFunctionModule>
                                List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();
                                string[] enumNameArray = registrationInfo.FunctionModule.Split(',');
                                foreach (var item in enumNameArray)
                                {
                                    CanUsefunctionModules.Add((GlobalFunctionModule)Enum.Parse(typeof(GlobalFunctionModule), item));
                                }
                            }
                        }
                    }
                    else
                    {
                        //如果没有注册直接隐藏
                        toolStrip1.Visible = false;
                        menuStrip1.Visible = false;
                        return;
                    }
                }

                #endregion

                // var logger = new LoggerFactory().AddLog4Net().CreateLogger("logs");
                //logger.LogError($"{DateTime.Now} LogError 日志");

                _logger.LogError("加载了ERP服务器窗体：frmMain_Load");
                this.IsMdiContainer = true; // 设置父窗体为MDI容器
                menuStrip1.MdiWindowListItem = 窗口ToolStripMenuItem;
                //InitAll();

                //手动初始化
                BizCacheHelper.Instance = Startup.GetFromFac<BizCacheHelper>();
                BizCacheHelper.InitManager();

                IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();
                cache.Set("test1", "test123");
                await InitConfig(true);

                MyCacheManager.Instance.CacheInfoList.OnAdd += CacheInfoList_OnAdd;
                MyCacheManager.Instance.CacheInfoList.OnClear += CacheInfoList_OnClear;
                MyCacheManager.Instance.CacheInfoList.OnUpdate += CacheInfoList_OnUpdate;
                MyCacheManager.Instance.CacheEntityList.OnRemove += CacheEntityList_OnRemove;
                //1分钟检查一次
                timer = new System.Timers.Timer(120000);
                timer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            //CheckSystemProtection();
                            CheckCacheList();
                            CheckReminderBizDataList();
                            lockManager.CheckLocks();
                            //  ServerLockManagerCmd cmd = new ServerLockManagerCmd(CmdOperation.Send);
                            // cmd.BuildDataPacketBroadcastLockStatus();
                        }
                        ));
                    }
                    else
                    {
                        // CheckSystemProtection();
                        CheckCacheList();
                        CheckReminderBizDataList();
                        // lockManager.CheckLocks();
                        // ServerLockManagerCmd cmd = new ServerLockManagerCmd(CmdOperation.Send);
                        // cmd.BuildDataPacketBroadcastLockStatus();
                    }
                });
                timer.Enabled = true;

                //1分钟检查一次
                //ReminderTimer = new System.Timers.Timer(60000);
                //ReminderTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
                //{
                //    if (this.InvokeRequired)
                //    {
                //        this.Invoke(new MethodInvoker(() => CheckReminderBizDataList()));
                //    }
                //    else
                //    {
                //        CheckReminderBizDataList();
                //    }
                //});
                //ReminderTimer.Enabled = true;
                //ReminderTimer.Start();

            }
            catch (Exception loadex)
            {


            }

        }

        private bool CheckRegistered(tb_sys_RegistrationInfo regInfo)
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
        /// 注册时会用到。启动程序时用到。 每次检测时用到
        /// </summary>
        /// <param name="regInfo"></param>
        /// <returns></returns>
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
                //ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                //{
                //    NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                //},
                // 指定只序列化Id, Name, 和 Age字段
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" } }
            };

            // 序列化对象
            string jsonString = JsonConvert.SerializeObject(regInfo, settings);
            string originalInfo = frmMain.Instance.UniqueHarewareInfo + jsonString;
            string key = "ruinor1234567890";
            string reginfo = HLH.Lib.Security.EncryptionHelper.AesEncrypt(originalInfo, key);
            return reginfo;
        }

        private void CacheInfoList_OnUpdate(object sender, CacheManager.Core.Internal.CacheActionEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values.ToArray())
            {
                //BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        private void CacheInfoList_OnClear(object sender, CacheManager.Core.Internal.CacheClearEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values.ToArray())
            {
                //  BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        private void CacheInfoList_OnAdd(object sender, CacheManager.Core.Internal.CacheActionEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values.ToArray())
            {
                //  BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        private void CacheEntityList_OnRemove(object sender, CacheManager.Core.Internal.CacheActionEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values.ToArray())
            {
                //  BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true是通过了检测</returns>
        private bool CheckSystemProtection()
        {
            bool result = false;
            try
            {
                if (registrationInfo != null)
                {
                    if (!registrationInfo.IsRegistered)
                    {
                        result = true;
                        //断开所有链接
                        Shutdown();
                        Application.Exit();
                        return false;
                    }
                    else
                    {

                        //如果验证不通过 或当前时间小于限制时间。就退出
                        if (!CheckRegistered(registrationInfo) || registrationInfo.ExpirationDate.Date <= DateTime.Now.Date)
                        {
                            _logger.LogError("Shutdown:验证不通过 或当前时间小于限制时间");
                            result = true;
                            //断开所有链接
                            Shutdown();
                            Application.Exit();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                frmMain.Instance.PrintInfoLog($"CheckSystemProtection：{ex.Message} ");
            }
            return result;
        }

        private async void CheckReminderBizDataList()
        {
            #region 根据ReminderBizDataList检查各种提醒的业务是不是要启动工作流了。
            try
            {
                foreach (var item in ReminderBizDataList)
                {
                    ReminderData BizData = item.Value;
                    if (BizData != null)
                    {
                        //这里要判断规则，目前暂时todo 写死,提前一天启动提醒
                        //如果启动时间 提前1天
                        if (System.DateTime.Now > BizData.StartTime.AddDays(-1) && System.DateTime.Now < BizData.EndTime && string.IsNullOrEmpty(BizData.WorkflowId))
                        {
                            //启动
                            var workflowId = await host.StartWorkflow("ReminderWorkflow", 1, BizData);
                            BizData.WorkflowId = workflowId;
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"启动{BizData.BizType} ，成功启动提醒工作流{workflowId}。");
                            }
                            continue;
                        }
                    }
                    else
                    {
                        if (frmMain.Instance.IsDebug)
                        {
                            frmMain.Instance.PrintInfoLog($"提醒业务数据为空：{item.Key}。");
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                frmMain.Instance.PrintInfoLog($"根据CheckReminderBizDataList检测数据启动工作流时出错：{exc.Message} ");
            }
            #endregion

        }

        private void CheckCacheList()
        {
            #region 根据CacheInfoList检查更新过期的缓存。
            try
            {

                foreach (var item in BizCacheHelper.Manager.NewTableList)
                {
                    CacheInfo cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(item.Key) as CacheInfo;
                    if (cacheInfo != null)
                    {
                        if (!MyCacheManager.Instance.CacheEntityList.Exists(item.Key))
                        {
                            BizCacheHelper.Instance.SetDictDataSource(item.Key, true);
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"检查更新过期的缓存 ，成功添加{item.Key}。");
                            }
                            //只有缓存概率有变化就发到客户端。客户端再根据这个与他本地实际的缓存数据行对比来请求真正的缓存数据
                            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
                            {
                                //  BizService.UserService.发送缓存信息列表(PlayerSession);
                            }
                        }
                    }
                    else
                    {
                        //if (frmMain.Instance.IsDebug)
                        //{
                        //    frmMain.Instance.PrintInfoLog($"检查更新过期的缓存时没有找到{item.Key}概览信息。");
                        //}
                    }
                }
            }
            catch (Exception exc)
            {
                frmMain.Instance.PrintInfoLog($"根据CacheInfoList检查更新过期的缓存出错：{exc.Message} ");
            }
            #endregion

        }

        /// <summary>
        /// 定时器会可能停止 可能是线程退出了
        /// </summary>
        /// <param name="state"></param>
        private void CheckAndRemoveExpiredSessions(object state)
        {
            try
            {
                var currentTime = DateTime.Now;
                var keysToRemove = new ConcurrentBag<string>();

                // 遍历会话集合，检查每个会话的LastActiveTime
                foreach (var kvp in sessionListBiz)
                {
                    if ((currentTime - kvp.Value.LastActiveTime).TotalMinutes > 1)
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // 删除过期的会话
                foreach (var key in keysToRemove)
                {
                    if (sessionListBiz.TryRemove(key, out var removedSession))
                    {
                        Console.WriteLine($"移除过期会话: {key}");
                    }
                }

                try
                {

                }
                catch (Exception ex)
                {
                    //线程
                    //frmMain.Instance.PrintInfoLog($"CheckAndRemoveExpiredSessions{ex.Message}。");
                    Console.WriteLine($"CheckAndRemoveExpiredSessions: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                //线程
                //frmMain.Instance.PrintInfoLog($"CheckAndRemoveExpiredSessions{ex.Message}。");
                Console.WriteLine($"CheckAndRemoveExpiredSessions: {ex.Message}");
            }
        }

        public async Task InitConfig(bool LoadData = true)
        {
            BizCacheHelper cacheHelper = Startup.GetFromFac<BizCacheHelper>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            cacheHelper.InitCacheDict(LoadData);
            stopwatch.Stop();
            frmMain.Instance.PrintInfoLog($"InitConfig总执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
            await Task.Delay(0);
        }

        private async void InitAll()
        {
            if (!ServerStart)
            {
                DbInit();
                StartServerUI();
                RUINORERP.Extensions.SqlsugarSetup.CheckEvent += SqlsugarSetup_CheckEvent;
            }
            else
            {
                frmMain.Instance.PrintInfoLog("服务器已经启动");
            }

            // 启动监控
            var reminderService = Startup.GetFromFac<SmartReminderService>();
            //不能加await
            Task.Run(async () => await reminderService.StartAsync(CancellationToken.None));


            // 每120秒（120000毫秒）执行一次检查
            System.Threading.Timer timerStatus = new System.Threading.Timer(CheckAndRemoveExpiredSessions, null, 0, 1000);

            //加载提醒数据

            DataServiceChannel loadService = Startup.GetFromFac<DataServiceChannel>();
            loadService.LoadCRMFollowUpPlansData(ReminderBizDataList);

            //启动每天要执行的定时任务
            //启动
            //var DailyworkflowId = await host.StartWorkflow("DailyTaskWorkflow", 1, null);
            GlobalScheduledData globalScheduled = new GlobalScheduledData();
            var DailyworkflowId = await host.StartWorkflow("NightlyWorkflow", 1, globalScheduled);
            frmMain.Instance.PrintInfoLog($"NightlyWorkflow每日任务启动{DailyworkflowId}。");
        }

        IHost _host = null;
        private NetworkServer _networkServer;
        async Task StartServer()
        {
            frmMain.Instance.PrintInfoLog("StartServer Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            try
            {

                /*
          _host = MultipleServerHostBuilder.Create()
         .AddServer<ServiceforBiz<BizPackageInfo>, BizPackageInfo, BizPipelineFilter>(builder =>
        {
            builder.ConfigureServerOptions((ctx, config) =>
            {
                //获取服务配置
                // ReSharper disable once ConvertToLambdaExpression
                var configSection = config.GetSection("ServiceforBiz");
                //tslblStatus.Text = "服务已启动。";
                //if (IsDebug)
                //{
                //    PrintMsg($"port:{configSection.GetSection("listeners").GetSection("0").GetSection("port").Value}");
                tslblStatus.Text = "服务已启动，端口：" + configSection.GetSection("listeners").GetSection("0").GetSection("port").Value;
                //}
                return configSection;
            })
            .UsePackageDecoder<MyPackageDecoder>()//注册自定义解包器
            .UseSession<SessionforBiz>()

        //注册用于处理连接、关闭的Session处理器
        .UseSessionHandler(async (session) =>
        {
            if (session.RemoteEndPoint is IPEndPoint iP)
            {
                var remoteIp = iP.Address.ToString();

                // 检查IP是否被封禁
                if (BlacklistManager.IsIpBanned(remoteIp))
                {
                    await session.CloseAsync(SuperSocket.Connection.CloseReason.ServerShutdown); // 立即关闭连接
                    return;
                }
            }


            SessionforBiz sessionforBiz = session as SessionforBiz;
            sessionListBiz.TryAdd(session.SessionID, session as SessionforBiz);


            if (frmuserList == null)
            {
                frmuserList = Startup.GetFromFac<frmUserList>();
            }
            if (frmuserList.IsHandleCreated)
            {
                frmuserList.Invoke(new Action(() =>
                {
                    sessionforBiz.User.PropertyChanged -= frmuserList.UserInfo_PropertyChanged;
                    sessionforBiz.User.PropertyChanged += frmuserList.UserInfo_PropertyChanged;
                    frmuserList.UserInfos.Add(sessionforBiz.User);
                }));
            }

            if (sessionforBiz.User != null)
            {
                while (MessageList.Count > 0 && sessionforBiz.User.超级用户)
                {
                    ReminderData MessageInfo = MessageList.Dequeue();
                    SystemService.process请求协助处理(sessionforBiz, MessageInfo);
                }
            }
            //广播出去
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values.ToArray())
            {
                BizService.UserService.发送在线列表(PlayerSession);
            }

            PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序] Session connected: {session.RemoteEndPoint}");
            await Task.Delay(0);
        }, async (session, reason) =>
        {
            try
            {
                SessionforBiz sg = session as SessionforBiz;
                //if (sg.player != null && sg.player.Online)
                //{
                // SephirothServer.CommandServer.RoleService.角色退出(sg);
                //}
                if (frmuserList == null)
                {
                    frmuserList = Startup.GetFromFac<frmUserList>();
                }
                if (frmuserList.IsHandleCreated)
                {
                    frmuserList.Invoke(new Action(() =>
                    {
                        frmuserList.UserInfos.CollectionChanged -= frmuserList.UserInfos_CollectionChanged;
                        frmuserList.UserInfos.CollectionChanged += frmuserList.UserInfos_CollectionChanged;
                        frmuserList.UserInfos.Remove(sg.User);
                    }));
                }

                if (reason.Reason != SuperSocket.Connection.CloseReason.ServerShutdown)
                {
                    PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序]  {session.RemoteEndPoint} closed，原因：: {reason.Reason}");
                }
                sessionListBiz.Remove(sg.SessionID, out sg);
                if (sg != null)
                {
                    PrintMsg(sg.User.用户名 + "断开连接");
                    //谁突然掉线或退出。服务器主动将他的锁在别人电脑上的单据释放
                    //SystemService.process断开连接锁定释放(sg.User.UserID);

                    //移除再广播出去 服务器主动将他的锁在别人电脑上的单据释放
                    lockManager.RemoveLockByUserID(sg.User.UserID);
                }

                ServerLockManagerCmd cmd = new ServerLockManagerCmd(CmdOperation.Send);
                cmd.BuildDataPacketBroadcastLockStatus();

                //广播出去
                foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
                {
                    BizService.UserService.发送在线列表(PlayerSession);
                }

            }
            catch (Exception quitex)
            {
                frmMain.Instance._logger.LogError("客户端退出", quitex);

            }

            await Task.Delay(0);
        })
                .ConfigureServices((context, services) =>
                {
                    IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();
                    // services = Startup.Services;
                    //services.AddMemoryCache();
                    services.AddMemoryCacheSetupWithInstance(cache);

                    //services.AddSingleton<CommandDispatcher>();
                    //services.AddTransient<ICommandHandler, LoginCommandHandler>();

                    foreach (var service in Startup.services)
                    {
                        // 假设 service 是一个 ServiceDescriptor 对象
                        // 将 service 注册添加到 OtherServices 中
                        services.Add(service);
                    }

                })

                .UseCommand(commandOptions =>
                {

                    commandOptions.AddCommand<BizCommand>();
                    commandOptions.AddCommand<XTCommand>();
                });
        })

    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.ClearProviders(); //去掉默认添加的日志提供程序
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
        // the defaults be overridden by the configuration.
        if (isWindows)
        {
            //注意这里配置日志级别 配置文件不生效？
            // Default the EventLogLoggerProvider to warning or above
            logging.AddFilter<EventLogLoggerProvider>(level => level >= Microsoft.Extensions.Logging.LogLevel.Error);
        }
        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
        if (isWindows)
        {
            // Add the EventLogLoggerProvider on windows machines
            //logging.AddEventLog();//这个写到了事件查看器中。没有必要
            logging.ClearProviders();
            //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
            //引用的long4net.dll要版本一样。
            string conn = AppSettings.GetValue("ConnectString");
            string key = "ruinor1234567890";
            string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);

            //logging.AddLog4Net();
            logging.AddProvider(new Log4NetProviderByCustomeDb("Log4net_db.config", newconn, Program.AppContextData));

        }
    })//.UseLog4Net()

.Build();

     */

                var _logger = Startup.ServiceProvider.GetService<ILogger<NetworkServer>>();
                // 创建NetworkServer实例
                _networkServer = new NetworkServer(Startup.services, _logger);

                // 启动网络服务器，使用配置的端口（默认为8009）
                //                int port = int.TryParse(AppSettings.GetValue("ERPServer"), out int configuredPort) ? configuredPort : 8009;
                //              int maxConnections = int.TryParse(AppSettings.GetValue("MaxConnections"), out int configuredMax) ? configuredMax : 1000;

                _host = await _networkServer.StartAsync(CancellationToken.None);

                //if (started)
                //{
                //    //tslblStatus.Text = "服务已启动，端口：" + port;
                //    //frmMain.Instance.PrintInfoLog($"网络服务器启动成功，监听端口: {port}");
                //}
                //else
                //{
                //    tslblStatus.Text = "服务启动失败";
                //    frmMain.Instance.PrintInfoLog("网络服务器启动失败");
                //}
            }
            catch (Exception hostex)
            {
                frmMain.Instance._logger.LogError("NetworkServer启动异常", hostex);
                frmMain.Instance.PrintInfoLog("NetworkServer启动异常: " + hostex.Message);
                tslblStatus.Text = "服务启动异常";
            }
            

        }


        public void StartupServer()
        {
            try
            {
                // 启动新线程执行异步服务器启动
                Task.Run(async () =>
                {
                    await StartServer();
                });
            }
            catch (Exception e)
            {
                Instance.PrintInfoLog($"启动SocketServer失败：{e.Message}.");
            }
        }

        async public void Shutdown()
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
                if (timer != null)
                {
                    timer.Dispose();
                }
                if (ReminderTimer != null)
                {
                    ReminderTimer.Dispose();
                }
            }
            catch (Exception e)
            {
                Instance.PrintInfoLog($"关闭SocketServer失败：{e.Message}.");
            }
            finally
            {
                if (_host != null)
                {
                    _host.Dispose();
                    _host = null;
                }
            }
        }

        public async Task DrainAllServers()
        {
            if (_host == null)
            {
                return;
            }
            //_host.StopAsync();
            foreach (var service in _host.Services.GetServices<IHostedService>())
            {
                if (service is IServer server)
                {
                    await DrainServer(server);
                    await service.StopAsync(CancellationToken.None);
                }
            }

            _host.WaitForShutdownAsync().GetAwaiter();
        }

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
                        // _logger.LogError(ex.Message, ex);
                        Instance.PrintInfoLog(ex.Message);
                    }
                }
            }
        }

        /*
         * 控制台-可以在控制台查看日志输出
        调试-vs工具 -》开始调试-》输出窗口进行查看日志输出
        EventSource-可使用PerfView 实用工具收集和查看日志
        EventLog-》仅在windows系统下可以使用事件查看器查看日志
        TraceSource
        AzureAppServicesFile
        AzureAppServicesBlob
        ApplicationInsights
         * **/

        #region show UI

        delegate void printMsg(string s);//创建一个代理

        public void PrintMsg(string t)//这个就是我们的函数，我们把要对控件进行的操作放在这里
        {
            if (!this.IsHandleCreated)
            {
                return;
            }
            if (!richTextBox1.InvokeRequired)//判断是否需要进行唤醒的请求，如果控件与主线程在一个线程内，可以写成 if(!InvokeRequired)
            {
                PrintInfoLog(t);
            }
            else
            {
                printMsg a1 = new printMsg(PrintMsg);
                if (this.IsHandleCreated)
                {
                    Invoke(a1, new object[] { t });//执行唤醒操作
                }

            }
        }

        #endregion
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                // 在这里编写 Ctrl + P 按下时要执行的代码
                Console.WriteLine("Ctrl + P 被按下");
                frmInput frmPassword = new frmInput();
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
                        MessageBox.Show("密码错误");
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DbInit()
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
            foreach (var item in collections.ToArray())
            {
                Console.WriteLine("{0}={1}", item.Key, item.Value);
            }

            //ILoggerFactory loggerFactory = LoggerFactory.Create(logbuilder => logbuilder
            // .AddFilter("Microsoft", LogLevel.Debug)
            // .AddFilter("System", LogLevel.Debug)
            // .AddFilter("Namespace.Class", LogLevel.Debug)
            // //.AddFile()//成功写入到文件
            // .AddLog4Net()
            //);
            //var logger = loggerFactory.CreateLogger("kxServerlogs");

            //PSqlHelper.Initialization(cache, configuration.GetSection("es_BLL_ITEM_CACHE"), configuration["ConnectionStrings:Npgsql"], null, logger);
        }

        private void SqlsugarSetup_CheckEvent(string sql)
        {
            if (IsDebug)
            {
                Instance.PrintInfoLog(sql);
            }
        }


        public void PrintInfoLog(string msg)
        {

            // Console.WriteLine(msg);
            if (!System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis"))
            {
                try
                {
                    if (IsDisposed || !frmMain.Instance.IsHandleCreated) return;

                    // 确保最多只有1000行
                    EnsureMaxLines(frmMain.Instance.richTextBox1, 1000);

                    // 将消息格式化为带时间戳和行号的字符串
                    string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {msg}\r\n";
                    if (frmMain.Instance.InvokeRequired)
                    {

                    }
                    frmMain.Instance.Invoke(new EventHandler(delegate
                    {
                        frmMain.Instance.richTextBox1.SelectionColor = Color.Black;
                        frmMain.Instance.richTextBox1.AppendText(formattedMsg);
                        frmMain.Instance.richTextBox1.SelectionColor = Color.Black;
                        frmMain.Instance.richTextBox1.ScrollToCaret(); // 滚动到最新的消息

                    }
                    ));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PrintInfoLog时出错" + ex.Message);
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
                frmMain.Instance.Invoke(new MethodInvoker(() =>
                {
                    rtb.Text = rtb.Text.Remove(start, end - start);
                }));
            }
        }

        public frmWFManage frmWF = Startup.GetFromFac<frmWFManage>();

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "工作流管理")
            {
                if (frmWF == null)
                {
                    frmWF = Startup.GetFromFac<frmWFManage>();
                }
                frmWF.RefreshData();//ReminderBizDataList
                frmWF.MdiParent = this;
                frmWF.Show();
                frmWF.Activate();

            }
            if (e.ClickedItem.Text == "启动服务")
            {
                //Load时就启动了。不重复启动了。
                // InitAll();
            }

            //if (e.ClickedItem.Text == "在线用户管理")
            //{
            //    //frmUserOnline frmuser = Startup.GetFromFac<frmUserOnline>();
            //    //frmuser.MdiParent = this;
            //    //frmuser.Show();
            //    if (frmusermange == null)
            //    {
            //        frmusermange = Startup.GetFromFac<frmUserManage>();
            //    }
            //    //frmUserManage 
            //    frmusermange.MdiParent = this;
            //    frmusermange.Show();
            //    frmusermange.Activate();
            //}

            if (e.ClickedItem.Text == "缓存管理")
            {
                frmCacheManage frmCahe = Startup.GetFromFac<frmCacheManage>();
                frmCahe.MdiParent = this;
                frmCahe.Show();
                frmCahe.Activate();
            }

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Shutdown();
        }
        private void 层叠排列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void 水平平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void 垂直平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void tsmY_Click(object sender, EventArgs e)
        {
            tsmY.Checked = true;
            tsmNo.Checked = !tsmY.Checked;
            IsDebug = tsmY.Checked;
            PrintMsg($"当前调式模式:{IsDebug}");
        }

        private void tsmNo_Click(object sender, EventArgs e)
        {
            tsmY.Checked = false;
            tsmNo.Checked = !tsmY.Checked;
            IsDebug = !tsmNo.Checked;
            PrintMsg($"当前调式模式:{IsDebug}");
        }

        private void tsbtnDataViewer_Click(object sender, EventArgs e)
        {
            frmMemoryDataViewer frm = Startup.GetFromFac<frmMemoryDataViewer>();
            frm.MdiParent = this;
            frm.Show();
            frm.Activate();
        }

        private void 参数配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGlobalConfig frm = Startup.GetFromFac<frmGlobalConfig>();
            frm.MdiParent = this;
            frm.Show();
            frm.Activate();
        }

        private void tsBtnStartServer_Click(object sender, EventArgs e)
        {
            if (frmuserList == null)
            {
                frmuserList = Startup.GetFromFac<frmUserList>();
            }
            InitAll();
            timer.Start();
            // 添加定时器用于刷新服务器信息
            UpdateServerInfoTimer.Start();
            tsBtnStartServer.Enabled = false;
        }

        private void 系统注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //注册成功后。才可能启动服务器
            frmRegister frm = Startup.GetFromFac<frmRegister>();
            frm.MdiParent = this;
            frm.Show();
            frm.Activate();
        }
        //frmUserListManage frmUserList = Startup.GetFromFac<frmUserListManage>();

        private void 黑名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlacklist frmBlacklist = new frmBlacklist();
            frmBlacklist.MdiParent = this;
            frmBlacklist.Show();
            frmBlacklist.Activate();
        }
        frmUserList frmuserList = Startup.GetFromFac<frmUserList>();
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (frmuserList == null)
            {
                frmuserList = Startup.GetFromFac<frmUserList>();
            }

            frmuserList.MdiParent = this;
            frmuserList.Show();
            frmuserList.Activate();
        }
    }
}
