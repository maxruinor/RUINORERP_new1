using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using FluentValidation.Results;
using RUINORERP.UI.UControls;
using SqlSugar;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DbType = SqlSugar.DbType;
using RUINORERP.Model;
using System.Threading;
using RUINORERP.UI.Common;
using System.Runtime.Remoting.Messaging;
using RUINORERP.UI.BI;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common;
using System.Reflection;
using RUINORERP.UI.UserCenter;
using RUINORERP.Business;

using System.Security.Claims;
using SuperSocket.ProtoBase;
using SuperSocket.ClientEngine;
using RUINORERP.UI.SuperSocketClient;
using System.Net;
using TransInstruction;
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
using TransInstruction.DataPortal;
using RUINORERP.Common.Extensions;
using Castle.Core.Smtp;
using System.IO;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using RUINORERP.UI.IM;
using System.Runtime.InteropServices;
using RUINORERP.UI.ToolForm;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using System.Linq.Expressions;
using Google.Protobuf.Collections;
using ExCSS;
using RUINORERP.Model.Models;
using RUINORERP.Business.CommService;
using RUINORERP.UI.SysConfig;
using RUINORERP.Common.Helper;
using System.Windows.Input;
using SourceLibrary.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Mysqlx;
using RUINORERP.Model.CommonModel;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using Mysqlx.Prepare;
using FastReport.DevComponents.DotNetBar;
using FastReport.Table;
using System.Xml;
using RUINORERP.Model.TransModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;
using RUINORERP.UI.ClientCmdService;
using RUINORERP.Global;
using TransInstruction.CommandService;
using HLH.Lib.Security;
using Netron.NetronLight;





namespace RUINORERP.UI
{
    public partial class MainForm : KryptonForm
    {

        //IOptions<T> 提供对配置设置的单例访问。它在整个应用程序生命周期中保持相同的实例，这意味着即使在配置文件更改后，通过 IOptions<T> 获取的值也不会改变
        //。

        //IOptionsMonitor<T> 是一个单例服务，但它可以监听配置文件的更改并自动更新其值。当文件发生更改时，它会自动重新加载配置，使得下一次访问 CurrentValue 属性时能够获取到最新的配置值。这种机制使得 IOptionsMonitor<T> 适用于那些需要实时反映配置更改的场景
        //。

        //IOptionsSnapshot<T> 的生命周期是作用域（Scoped），这意味着对于每一次HTTP请求，都会提供一个新的实例。如果在请求过程中配置文件发生了更改，这个实例仍然保持旧的值，直到新的请求到达，才会获取到新的配置值。因此，IOptionsSnapshot<T> 适合用在那些需要每个请求都使用最新配置快照的场景

        /// <summary>
        /// 可配置性全局参数 不能设置为只读
        /// </summary>
        public IOptionsMonitor<SystemGlobalconfig> Globalconfig;

        #region 当前系统中所有用户信息
        private List<UserInfo> userInfos = new List<UserInfo>();

        /// <summary>
        /// 当前系统所有用户信息列表
        /// </summary>
        public List<UserInfo> UserInfos { get => userInfos; set => userInfos = value; }


        #endregion

        /// <summary>
        /// 指令调度员
        /// </summary>
        public ClientCommandDispatcher dispatcher;


        /// <summary>
        /// 保存服务器的一些缓存信息。让客户端可以根据一些机制来获取。得到最新的信息
        /// </summary>
        public ConcurrentDictionary<string, CacheInfo> CacheInfoList = new ConcurrentDictionary<string, CacheInfo>();

        /// <summary>
        /// 这个用来缓存，录入表单时的详情产品数据。后面看优化为一个全局缓存。
        /// </summary>
        public List<View_ProdDetail> list = new List<View_ProdDetail>();

        //一个消息缓存列表，有处理过的。未处理的。未看的。临时性还是固定到表的？
        public Queue<ReminderData> MessageList = new Queue<ReminderData>();

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

        public MainForm(ILogger<MainForm> _logger, IOptionsMonitor<SystemGlobalconfig> config)
        {
            InitializeComponent();
            lblStatusGlobal.Text = string.Empty;
            logger = _logger;
            _main = this;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            kryptonDockingManager1.DefaultCloseRequest = DockingCloseRequest.RemovePageAndDispose;
            kryptonDockableWorkspace1.ShowMaximizeButton = false;
            kryptonDockableWorkspace1.WorkspaceCellRemoved += KryptonDockableWorkspace1_WorkspaceCellRemoved;
            kryptonDockableWorkspace1.ControlRemoved += KryptonDockableWorkspace1_ControlRemoved;
            kryptonDockableWorkspace1.PageCloseClicked += KryptonDockableWorkspace1_PageCloseClicked;


            ecs.OnConnectClosed += Ecs_OnConnectClosed;
            if (config != null)
            {
                Globalconfig = config;
                // 监听配置变化
                Globalconfig.OnChange(updatedConfig =>
                {
                    Console.WriteLine($"Configuration has changed: {updatedConfig.SomeSetting}");
                });
            }

            AppContext = Program.AppContextData;


            var clientCommandRegistry = new ClientCommandRegistry();
            var clientCommandHandlers = clientCommandRegistry.AutoRegisterCommandHandler();
            dispatcher = new ClientCommandDispatcher(clientCommandHandlers);

        }

        private void KryptonDockableWorkspace1_PageCloseClicked(object sender, UniqueNameEventArgs e)
        {
            if (e.UniqueName == "控制中心" || e.UniqueName == "工作台")
            {

                return;
            }
            if(kryptonDockableWorkspace1.ActivePage!=null && kryptonDockableWorkspace1.ActivePage.UniqueName==e.UniqueName)
            {
                 var control = kryptonDockableWorkspace1.ActivePage.Controls[0];
                 if (control.GetType() != null && control.GetType().BaseType.Name == "BaseListGeneric`1")
                 {
                     // 获取泛型参数类型
                     Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                     if (genericArguments.Length > 0)
                     {
                         Type genericParameterType = genericArguments[0];
                         var baseUControl = (BaseUControl)control;
                 
                         UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo,  baseUControl.BaseDataGridView1, genericParameterType);
                     }
                 }

                if (control.GetType() != null && control.GetType().BaseType.Name.Contains("BaseBillQueryMC"))
                {
                    // 获取泛型参数类型
                    Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                    if (genericArguments.Length > 0)
                    {
                        Type genericParameterType = genericArguments[0];
                        var baseUControl = (BaseQuery)control;

                        UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);

                        if (genericArguments.Length == 2 && !genericArguments[0].Name.Equals(genericArguments[1].Name))
                        {
                            UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseSubDataGridView, genericArguments[1]);

                        }

                    }
                }


                if (control.GetType() != null && control.GetType().BaseType.Name.Contains("BaseNavigatorGeneric"))
                {
                    // 获取泛型参数类型
                    Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                    if (genericArguments.Length > 0)
                    {
                        Type genericParameterType = genericArguments[0];
                        var baseUControl = (BaseNavigator)control;
                        UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);
                    }
                }

            }

        }

        private void KryptonDockableWorkspace1_ControlRemoved(object sender, ControlEventArgs e)
        {

        }

        private void KryptonDockableWorkspace1_WorkspaceCellRemoved(object sender, WorkspaceCellEventArgs e)
        {

        }

        public EasyClientService ecs = new EasyClientService();





        private void Ecs_OnConnectClosed(bool isconect)
        {
            // ecs.LoginStatus = false;
            ecs.IsConnected = isconect;
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

        /// <summary>
        /// 主动更新，就有提示，被动 就不需要提示了。TODO 后面完善
        /// </summary>
        public async Task<bool> UpdateSys(bool ActiveUpdate)
        {
            bool rs = false;

            AutoUpdate.FrmUpdate Update = new AutoUpdate.FrmUpdate();
            if (Update.CheckHasUpdates())
            {
                MessageBox.Show("服务器有新版本，更新前请保存当前操作，关闭系统。", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Process.Start(Update.currentexeName);
                rs = true;
            }
            else
            {
                if (ActiveUpdate)
                {
                    MessageBox.Show("已经是最新版本，不需要更新。", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                rs = false;
            }
            await Task.Delay(10); // 假设操作需要一段时间
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

        //private KryptonPage NewInput()
        //{
        //    return NewPage("LocationTypeList", 0, new UCCustomerVendorList());
        //}



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

        private KryptonPage NewIMMsg()
        {
            ucMsg.Height = 30;
            KryptonPage pageMsg = NewPage("通讯中心", 2, ucMsg);
            //pageMsg.ClearFlags(KryptonPageFlags.All);
            pageMsg.ClearFlags(KryptonPageFlags.DockingAllowClose);
            pageMsg.ClearFlags(KryptonPageFlags.DockingAllowFloating);//控制托出的单独窗体是否能关掉
            pageMsg.Height = 30;
            return pageMsg;
        }
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

        public UClog uclog = Startup.GetFromFac<UClog>();
        public IM.UCMessager ucMsg = new IM.UCMessager();

        private string version = string.Empty;



        /// <summary>
        /// 保存锁定信息集合
        /// </summary>
        public ConcurrentDictionary<long, BillLockInfo> LockInfoList = new ConcurrentDictionary<long, BillLockInfo>();

        /// <summary>
        /// 转发单据审核锁定 https://www.cnblogs.com/fanfan-90/p/12151924.html
        /// 锁单
        /// </summary>
        public IMemoryCache CacheTemp { get; set; }



        private async void MainForm_Load(object sender, EventArgs e)
        {
            CacheTemp = Startup.GetFromFac<IMemoryCache>();
            authorizeController = Startup.GetFromFac<AuthorizeController>();

            //cache.Set("test1", "test123");

            //手动初始化  打开就加载。省得登陆后还没有加载完
            BizCacheHelper.Instance = Startup.GetFromFac<BizCacheHelper>();
            BizCacheHelper.InitManager();
            await InitConfig(false);

            //先加载一遍缓存
            var tableNames = CacheInfoList.Keys.ToList();
            foreach (var nextTableName in tableNames)
            {
                TryRequestCache(nextTableName);
            }


            this.Text = "企业数字化集成ERP v2.0" + "_" + AppContext.CurrentUser.客户端版本;
            //MessageBox.Show("登陆成功后，请要系统设置中添加公司基本资料。");



            InitRemind();
            // logger.LogInformation("打开主窗体准备进入系统");
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
            if (!Login())
            {
                return;
            }
            else
            {


                UIBizSrvice.RequestCache(typeof(tb_RoleInfo));
                UIBizSrvice.RequestCache(typeof(tb_ProductType));
                UIBizSrvice.RequestCache(typeof(View_ProdDetail));

                using (StatusBusy busy = new StatusBusy("系统正在【初始化】 请稍候"))
                {
                    tb_MenuInfoController<tb_MenuInfo> menuInfoController = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
                    List<tb_MenuInfo> menuList = await menuInfoController.QueryAsync();
                    //var rslist = BizCacheHelper.Manager.CacheEntityList.Get(nameof(tb_MenuInfo));
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

                    #endregion

                    if (UserGlobalConfig.Instance.MenuPersonalizationlist == null)
                    {
                        UserGlobalConfig.Instance.MenuPersonalizationlist = new ConcurrentDictionary<string, MenuPersonalization>();
                    }

                    //设计关联列和目标列
                    View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
                    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                    .AndIF(true, w => w.CNName.Length > 0)
                    .ToExpression();//注意 这一句 不能少
                                    //list = await dc.BaseQueryByWhereAsync(exp);
                                    //list = MainForm.Instance.list;
                    TryRequestCache(nameof(View_ProdDetail), typeof(View_ProdDetail));

                    //这里做一个事件。缓存中的变化了。这里也变化一下。todo:
                    try
                    {
                        bool rs = mc.CheckMenuInitialized();
                        if (!rs)
                        {
                            //如果从来没有初始化过菜单，则执行
                            InitMenu();
                        }

                        //启动心跳线程 这里等待基本就假死了
                        //await Task.Run(() => { StartHeartbeatService(); });
                    }
                    catch (Exception ex)
                    {
                        // logger.Error(ex);
                        MainForm.Instance.uclog.AddLog(ex.Message + ex.StackTrace);
                    }

                }
            }


            timer1.Start();
            Stopwatch stopwatchLoadUI = Stopwatch.StartNew();
            LoadUIMenus();
            LoadUIForIM_LogPages();
            stopwatchLoadUI.Stop();
            MainForm.Instance.uclog.AddLog($"LoadUIPages 执行时间：{stopwatchLoadUI.ElapsedMilliseconds} 毫秒");
            kryptonDockableWorkspace1.ActivePageChanged += kryptonDockableWorkspace1_ActivePageChanged;
            GetActivePage(kryptonDockableWorkspace1);


            LoginWebServer();

            System.Windows.Forms.Timer timerStatus = new System.Windows.Forms.Timer();
            timerStatus.Interval = 1000; // 设置定时器间隔为1000毫秒（1秒）
            timerStatus.Tick += (sender, e) => RefreshData();
            timerStatus.Start();

            if (AppContext.IsSuperUser)
            {
                tsbtnSysTest.Visible = true;
            }

            MainForm.Instance.kryptonDockingManager1.PageCloseRequest += KryptonDockingManager1_PageCloseRequest;

            MainForm.Instance.kryptonDockingManager1.DockspaceCellRemoved += KryptonDockingManager1_DockspaceCellRemoved;

            MainForm.Instance.kryptonDockingManager1.DockspaceRemoved += KryptonDockingManager1_DockspaceRemoved;

        }

        private void KryptonDockingManager1_DockspaceRemoved(object sender, DockspaceEventArgs e)
        {

        }

        private void KryptonDockingManager1_DockspaceCellRemoved(object sender, DockspaceCellEventArgs e)
        {

        }

        private void KryptonDockingManager1_PageCloseRequest(object sender, CloseRequestEventArgs e)
        {

        }

        BizTypeMapper mapper = new BizTypeMapper();
        public AuthorizeController authorizeController;
        private void RefreshData()
        {
            try
            {


                // 更新状态栏信息
                //if (ecs.client.Socket == null)
                //{
                //    lblServerInfo.Text = $"Server:{UserGlobalConfig.Instance.ServerIP},Connected:{ecs.IsConnected}";
                //}
                //else
                //{
                //    lblServerInfo.Text = $"Server:{UserGlobalConfig.Instance.ServerIP},Connected:{ecs.IsConnected}，sessionID:{ecs.client.Socket.LocalEndPoint}";
                //}
                if (ecs.client.Socket == null)
                {
                    lblServerStatus.ToolTipText = $"Server:{UserGlobalConfig.Instance.ServerIP},Port:{UserGlobalConfig.Instance.ServerPort},Connected:{ecs.IsConnected},FreeTime:{GetLastInputTime()}";
                }
                else
                {
                    lblServerStatus.ToolTipText = $"Server:{UserGlobalConfig.Instance.ServerIP},Port:{UserGlobalConfig.Instance.ServerPort}，Connected:{ecs.IsConnected}，LocIP:{ecs.client.Socket.LocalEndPoint},FreeTime:{GetLastInputTime()}";
                }





                lblServerInfo.Text = lblServerStatus.ToolTipText;
                if (MessageList.Count > 0)
                {



                    ReminderData MessageInfo = MessageList.Dequeue();
                    //NotificationBox notificationBox = new NotificationBox();
                    //notificationBox.ShowForm(MessageInfo.Content);
                    MessagePrompt messager = new MessagePrompt();
                    messager.mapper = mapper;

                    if (MessageInfo.ReceiverEmployeeIDs == null)
                    {
                        MessageInfo.SenderEmployeeName = "系统";
                    }
                    else
                    {
                        var Employee_ID = MessageInfo.ReceiverEmployeeIDs.FirstOrDefault(c => c == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID);
                        var userinfo = MainForm.Instance.UserInfos.FirstOrDefault(c => c.Employee_ID == Employee_ID);
                        if (userinfo == null)
                        {
                            MessageInfo.SenderEmployeeName = "系统";
                        }
                        else
                        {
                            MessageInfo.SenderEmployeeName = userinfo.姓名;
                        }
                    }


                    messager.txtSender.Text = MessageInfo.SenderEmployeeName;
                    if (MessageInfo.RemindSubject.IsNotEmptyOrNull())
                    {
                        messager.txtSubject.Text = "【" + MessageInfo.BizType + "】" + MessageInfo.RemindSubject;
                    }
                    else
                    {
                        messager.txtSubject.Text = "请求协助";
                    }
                    //#region 消息命令处理器
                    //var command = new ReceiveMessageCommand(Model.TransModel.MessageType.Text, "888");

                    //command.message = MessageInfo.ReminderContent;
                    //_dispatcher.Dispatch(command);
                    //#endregion

                    messager.Content = MessageInfo.ReminderContent;
                    messager.ReminderData = MessageInfo;
                    messager.Show();
                    messager.TopMost = true;
                    //MainForm.Instance.ShowMsg(MessageInfo.Content);
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void kryptonDockableWorkspace1_ActivePageChanged(object sender, ActivePageChangedEventArgs e)
        {
            GetActivePage(sender);
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
            }
        }


        #region 心跳包
        public static int HeartbeatCounter = 0;

        #endregion



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

                KryptonPage IMPage = NewIMMsg();
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
                LoadMenu();
                LoadUIPagesByLeft();
            }
        }

        #region  加载左边菜单
        private void LoadUIPagesByLeft()
        {
            kryptonNavigator1.Pages.Clear();
            if (AppContext.IsSuperUser)
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

                    if (item.tb_P4Menus != null)
                    {
                        List<tb_MenuInfo> tempList = new List<tb_MenuInfo>();
                        foreach (tb_P4Menu P4Menu in item.tb_P4Menus.Where(c => c.IsVisble).ToList())
                        {
                            //不重复
                            if (!tempList.Contains(P4Menu.tb_menuinfo))
                            {
                                tempList.Add(P4Menu.tb_menuinfo);
                            }
                        }
                        //如果是顶级菜单是和模块名相同，跳过

                        var modMenu = tempList.FirstOrDefault(c => c.Parent_id == 0 && c.MenuName == item.ModuleName);
                        var subMenus = tempList.Where(c => c.Parent_id != 0 && c.Parent_id == modMenu.MenuID).OrderBy(c => c.Sort);
                        foreach (tb_MenuInfo subMenu in subMenus)
                        {
                            if (!subMenu.IsEnabled || !subMenu.IsVisble)
                            {
                                continue;
                            }
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
                foreach (tb_ModuleDefinition item in AppContext.CurUserInfo.UserModList)
                {

                    // Create new page with title and image
                    KryptonPage p = new KryptonPage();
                    p.Text = item.ModuleName;
                    p.TextTitle = item.ModuleName;
                    p.Name = item.ModuleID.ToString();
                    p.TextDescription = item.ModuleName;// + item.BizType.ToString();
                                                        // p.ImageSmall = imageListSmall.Images[image];

                    if (item.tb_P4Menus != null)
                    {
                        KryptonTreeView TreeView1 = new KryptonTreeView();
                        TreeView1.MouseDoubleClick += TreeView1_DoubleClick;

                        TreeView1.NodeMouseDoubleClick += TreeView1_NodeMouseDoubleClick;
                        //如果是顶级菜单是和模块名相同，跳过

                        List<tb_MenuInfo> tempList = new List<tb_MenuInfo>();
                        foreach (tb_P4Menu P4Menu in item.tb_P4Menus.Where(c => c.IsVisble).ToList())
                        {
                            //不重复
                            if (!tempList.Contains(P4Menu.tb_menuinfo))
                            {
                                tempList.Add(P4Menu.tb_menuinfo);
                            }
                        }
                        List<tb_MenuInfo> sortlist = tempList.OrderBy(t => t.Sort).ToList();
                        Bind(TreeView1, sortlist);
                        // Bind(nodeRoot, tempList, P4Menu.tb_menuinfo.MenuID);
                        TreeView1.HideSelection = false;
                        // TreeView1.Nodes.Clear();
                        TreeView1.Dock = DockStyle.Fill;
                        p.ClearFlags(KryptonPageFlags.DockingAllowClose);
                        // TreeView1.Nodes[0].Expand();
                        TreeView1.ExpandAll();
                        p.Controls.Add(TreeView1);

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
        private void Bind(KryptonTreeView tree, List<tb_MenuInfo> resourceList)
        {
            //            var childList = resourceList.Where(t => t.Parent_id == 0).OrderBy(m => m.CaptionCN).ThenBy(t => t.Sort);
            var childList = resourceList.Where(t => t.Parent_id == 0).OrderBy(m => m.Sort);
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
            //   List<tb_MenuInfo> mlist = resourceList.FindAll(delegate (tb_MenuInfo p) { return p.Parent_id == nodeId; });
            //  var sortlist = mlist.OrderBy(o => o.CaptionCN).ThenBy(t => t.Sort).ToList();
            // var childList = resourceList.Where(t => t.Parent_id == nodeId).OrderBy(m => m.CaptionCN).ThenBy(t => t.Sort);
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
                MainForm.Instance.uclog.AddLog(sql);
            }
        }

        #region ApplyAuthorizationRules

        private void ApplyAuthorizationRules()
        {

            bool rs = true;// Csla.Rules.BusinessRules.HasPermission(Program.cslaAppContext, Csla.Rules.AuthorizationActions.CreateObject, typeof(Business.UseCsla.tb_LocationTypeEditBindingList));

            /*
            // Project menu
            this.NewProjectToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.CreateObject, typeof(ProjectTracker.Library.ProjectEdit));
            this.EditProjectToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.GetObject, typeof(ProjectTracker.Library.ProjectEdit));
            if (Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, typeof(ProjectTracker.Library.ProjectEdit)))
                this.EditProjectToolStripMenuItem.Text =
                  "Edit project";
            else
                this.EditProjectToolStripMenuItem.Text =
                  "View project";
            this.DeleteProjectToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.DeleteObject, typeof(ProjectTracker.Library.ProjectEdit));

            // Resource menu
            this.NewResourceToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.CreateObject, typeof(ProjectTracker.Library.ResourceEdit));
            this.EditResourceToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.GetObject, typeof(ProjectTracker.Library.ResourceEdit));
            if (Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, typeof(ProjectTracker.Library.ResourceEdit)))
                this.EditResourceToolStripMenuItem.Text =
                  "Edit resource";
            else
                this.EditResourceToolStripMenuItem.Text =
                  "View resource";
            this.DeleteResourceToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.DeleteObject, typeof(ProjectTracker.Library.ResourceEdit));

            // Admin menu
            this.EditRolesToolStripMenuItem.Enabled =
              Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, typeof(ProjectTracker.Library.Admin.RoleEditBindingList));
            */
        }

        #endregion

        #region Login/Logout


        private bool Login()
        {
            bool rs = false;
            RUINORERP.Business.Security.PTPrincipal.Logout(AppContext);
            // if (this.LoginToolStripButton.Text == "登出")
            //{
            FrmLogin loginForm = new FrmLogin(ecs);
            if (loginForm.ShowDialog(this) == DialogResult.OK)
            {
                tb_SystemConfigController<tb_SystemConfig> ctr = Startup.GetFromFac<tb_SystemConfigController<tb_SystemConfig>>();
                List<tb_SystemConfig> config = ctr.Query();
                if (config.Count > 0)
                {
                    AppContext.SysConfig = config[0];
                }
                rs = true;
                if (AppContext.CurUserInfo != null)
                {
                    this.SystemOperatorState.Text = $"登陆: {AppContext.CurUserInfo.Name}【{AppContext.CurrentRole.RoleName}】";
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
                AuditLogHelper.Instance.CreateAuditLog("登陆", $"{System.Environment.MachineName}-成功登陆服务器");
                if (MainForm.Instance.AppContext.CurUserInfo != null && MainForm.Instance.AppContext.CurUserInfo.UserInfo != null)
                {
                    try
                    {
                        MainForm.Instance.AppContext.CurUserInfo.UserInfo.Lastlogin_at = System.DateTime.Now;
                        MainForm.Instance.AppContext.Db.CopyNew().Storageable<tb_UserInfo>(MainForm.Instance.AppContext.CurUserInfo.UserInfo).ExecuteReturnEntityAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("更新最后登陆时间出错。", ex);
                    }

                }
                if (MainForm.Instance.AppContext.CompanyInfo == null)
                {
                    MainForm.Instance.AppContext.CompanyInfo = MainForm.Instance.AppContext.Db.Queryable<tb_Company>().Single();
                }
                if (AppContext.CompanyInfo != null)
                {
                    this.Text = AppContext.CompanyInfo.ShortName + "企业数字化集成ERP v2.0" + "_" + AppContext.CurrentUser.客户端版本;
                }

                UIBizSrvice.RequestCache(nameof(tb_RoleInfo));
            }
            else
            {
                Application.Exit();
            }
            UserGlobalConfig.Instance.Serialize();
            // reset menus, etc.
            ApplyAuthorizationRules();
            // notify all documents
            //加载uc
            return rs;
        }


        private async void Logout()
        {
            try
            {
                this.SystemOperatorState.Text = "登出";
                AuditLogHelper.Instance.CreateAuditLog("登出", "成功登出服务器");
                MainForm.Instance.AppContext.CurUserInfo.UserInfo.Lastlogout_at = System.DateTime.Now;
                var result = await MainForm.Instance.AppContext.Db.Updateable<tb_UserInfo>(MainForm.Instance.AppContext.CurUserInfo.UserInfo)
                .UpdateColumns(it => new { it.Lastlogout_at }).ExecuteCommandAsync();
                ClearUI();

                ClearData();
                Application.DoEvents();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void LogLock()
        {
            MainForm.Instance.Invoke(new Action(() =>
            {
                this.SystemOperatorState.Text = "注销";
                Program.AppContextData.IsOnline = false;
                MainForm.Instance.AppContext.CurrentUser.授权状态 = false;
                MainForm.Instance.AppContext.CurrentUser.在线状态 = false;
                ClearUI();
                ClearRoles();
                System.GC.Collect();
                //标记一下状态为注销，可以保存到上下文传到心跳包中
                if (!Login())
                {
                    return;
                }
                // await InitConfig();
                LoadUIMenus();
                LoadUIForIM_LogPages();
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
                /*
                #region 工作台

                KryptonPage pWorkbenches = new KryptonPage();
                pWorkbenches.Text = "我的工作台";
                pWorkbenches.TextTitle = "我的工作台";
                pWorkbenches.TextDescription = "我的工作台";
                pWorkbenches.UniqueName = "我的工作台";
                //  p.ImageSmall = imageListSmall.Images[image];
                // Form2 frm = new Form2();
                // frm.Controls.Add(_UCInitControlCenter);
                // frm.Show();
                // Add the control for display inside the page
                _UCWorkbenches.Dock = DockStyle.Fill;
                //_UCWorkbenches.TopLevel = false;
                pWorkbenches.Controls.Add(_UCWorkbenches);

                #endregion
                */

                //var _UCWorkCenter = Startup.GetFromFac<UCWorkCenter>(); //获取服务Service1

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


        private async Task InitConfig(bool LoadData)
        {
            BizCacheHelper.Instance.InitDict(LoadData);
            await Task.Delay(10);
        }

        /// <summary>
        /// 只执行一次,初始化菜单
        /// </summary>
        private void InitMenu()
        {
            //List<MenuAssemblyInfo> list = UIHelper.RegisterForm();
            //CreateMenu(list, 0, 0);
            Stopwatch stopwatch = Stopwatch.StartNew();
            InitModuleMenu init = Startup.GetFromFac<InitModuleMenu>();
            try
            {
                init.InitModuleAndMenu();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            stopwatch.Stop();
            MainForm.Instance.logger.LogInformation($"初始化菜单InitMenu 执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
            MainForm.Instance.uclog.AddLog($"初始化菜单InitMenu 执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
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
        private List<tb_MenuInfo> LoadTypesNew()
        {

            tb_MenuInfo menuInfoparent = new tb_MenuInfo();
            List<tb_MenuInfo> menuList = new List<tb_MenuInfo>();
            #region

            /*
            List<string> haust = new List<string>();
            haust.Add("0001");
            haust.Add("1001");
            haust.Add("0111");
            haust.Add("0011");

            var arr = haust.GroupBy(x => x.Substring(2, 2));//这一句是重点
            //输出
            foreach (var a in arr)
            {
                Console.Write(a.Key + ":");
                foreach (var it in a)
                {
                    Console.Write(it + " ");
                }
            }
            */
            #endregion
            List<MenuAttrAssemblyInfo> list = UIHelper.RegisterForm();

            var arrs = list.GroupBy(x => x.MenuPath.Split('|')[0]);
            //输出
            foreach (var a in arrs)
            {
                menuInfoparent.MenuName = a.Key;
                menuInfoparent.IsVisble = true;
                menuInfoparent.IsEnabled = true;
                menuInfoparent.CaptionCN = a.Key;
                menuInfoparent.MenuType = "导航菜单";
                menuInfoparent.Parent_id = 0;
                mc.AddMenuInfo(menuInfoparent);
                Console.Write(a.Key + ":" + "\r\n");
                foreach (var it in a)
                {
                    //如果最后为空则是行为菜单了
                    if (it.MenuPath.Split('|').Last<string>() == "")
                    {
                        Model.tb_MenuInfo menu = new tb_MenuInfo();
                        menu.MenuName = it.ClassName;
                        menu.IsVisble = true;
                        menu.IsEnabled = true;
                        menu.CaptionCN = it.Caption;
                        menu.ClassPath = it.ClassPath;
                        menu.FormName = it.ClassName;
                        menu.Parent_id = menuInfoparent.MenuID;
                        if (it.MenuBizType.HasValue)
                        {
                            menu.BizType = (int)it.MenuBizType;
                        }

                        menu.MenuType = "行为菜单";
                        mc.AddMenuInfo(menu);
                    }
                    else
                    {
                        //再次分组

                    }
                    Console.Write(it + " " + "\r\n");
                }
                Console.WriteLine();
                // menuList.Add(menuInfoparent);
            }

            List<KeyValuePair<MenuAttrAssemblyInfo, string[]>> kvlist = new List<KeyValuePair<MenuAttrAssemblyInfo, string[]>>();
            int max = 0;
            foreach (var item in list)
            {
                string[] mps = item.MenuPath.Split('|');
                KeyValuePair<MenuAttrAssemblyInfo, string[]> kv = new KeyValuePair<MenuAttrAssemblyInfo, string[]>(item, mps.ToArray<string>());
                if (mps.Length > max)
                {
                    max = mps.Length;
                }

                kvlist.Add(kv);
                //  AnalysisMenuPath(item, mps[0]);
            }



            //IEnumerable<IGrouping<string, Cart_Model>> query = list_CartModel.GroupBy(pet => pet.ShopId, pet => pet);

            //分组
            //                    var groups = list
            ////                                .Select((item, index) => new { Item = item, GroupIndex = index % GroupCount })
            //                                .GroupBy(item => item.GroupIndex, (key, group) => group.Select(groupItem => groupItem.Item).ToList())
            //                                .ToList();

            //return groups;



            return menuList;
        }


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
        private void LoadMenu()
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
                p.OtherEvent += p_OtherEvent;
                p.MainMenu = this.menuStripMain;
                ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
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


            tb_UnitValidator validator = new tb_UnitValidator();
            ValidationResult results = validator.Validate(tbunit);
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;

            db.Insertable(tbunit).ExecuteCommand(); //都是参数化实现




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
                //if (MessageBox.Show(this, "确定退出本系统吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                //{

                e.Cancel = false;
                System.GC.Collect();
                Logout();
                //}
                //else
                //{
                //    e.Cancel = true;
                //    return;
                //}
                await ecs.client.Close();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "MainForm_FormClosing");
            }
            System.GC.Collect();
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
                MainForm.Instance.Invoke(new Action(() =>
                {
                    uclog.AddLog("print", msg);
                }));

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
        private async void ClearData()
        {
            Program.AppContextData.IsOnline = false;
            ecs.LoginStatus = false;
            AppContext.CurUserInfo = null;
            AppContext.IsSuperUser = false;
            RUINORERP.Extensions.SqlsugarSetup.CheckEvent -= SqlsugarSetup_CheckEvent;
            await ecs.Stop();
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
                Application.Exit();
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
                LoadMenu();
                LoadUIPagesByLeft();
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

        public void ShowMsg(string msg, string Caption = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            try
            {
                //
                //otificationBox.Instance().ShowForm(msg);//显示窗体
                this.Invoke(new Action(() =>
                {
                    if (Caption == null)
                    {
                        Caption = "消息提醒";
                    }

                    MSNRemind(Caption, $"{msg}\r\n  [点击查看]", 2000, 5000, 3000, true, true, false, false, true, true);

                }));

            }
            catch (Exception ex)
            {

            }
        }


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

        TaskbarNotifier taskbarNotifier1;

        private void InitRemind()
        {
            taskbarNotifier1 = new TaskbarNotifier();
            taskbarNotifier1.SetBackgroundBitmap(global::RUINORERP.UI.Properties.Resources.skin, Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(global::RUINORERP.UI.Properties.Resources.close, Color.FromArgb(255, 0, 255), new System.Drawing.Point(127, 8));
            taskbarNotifier1.TitleRectangle = new System.Drawing.Rectangle(40, 9, 70, 25);
            taskbarNotifier1.ContentRectangle = new System.Drawing.Rectangle(8, 41, 133, 68);
            taskbarNotifier1.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier1.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier1.CloseClick += new EventHandler(CloseClick);
        }
        void TitleClick(object obj, EventArgs ea)
        {
            //后面 可以修改ea的值，传类型和值，对应 处理
        }



        void ContentClick(object obj, EventArgs ea)
        {
            if (1 == 1)
            {
                //后面 可以修改ea的值，传类型和值，对应 处理
            }
        }

        void CloseClick(object obj, EventArgs ea)
        {
            taskbarNotifier1.Close();
        }

        /// <summary>
        /// MSN头像
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <param name="DelayShow"></param>
        /// <param name="DelayStay"></param>
        /// <param name="DelayHide"></param>
        /// <param name="CloseClickable"></param>
        /// <param name="TitleClickable"></param>
        /// <param name="ContentClickable"></param>
        /// <param name="SelectionRectangle"></param>
        /// <param name="KeepVisibleOnMouseOver"></param>
        /// <param name="ReShowOnMouseOver"></param>
        public void MSNRemind(string Title, string Content, int DelayShow, int DelayStay, int DelayHide, bool CloseClickable, bool TitleClickable, bool ContentClickable, bool SelectionRectangle, bool KeepVisibleOnMouseOver, bool ReShowOnMouseOver)
        {
            if (Title.Length == 0 || Content.Length == 0)
            {
                MessageBox.Show("Enter a title and a content Text");
                return;
            }
            taskbarNotifier1.CloseClickable = CloseClickable;
            taskbarNotifier1.TitleClickable = TitleClickable;
            taskbarNotifier1.ContentClickable = ContentClickable;
            taskbarNotifier1.EnableSelectionRectangle = SelectionRectangle;
            taskbarNotifier1.KeepVisibleOnMousOver = KeepVisibleOnMouseOver;	// Added Rev 002
            taskbarNotifier1.ReShowOnMouseOver = ReShowOnMouseOver;			// Added Rev 002
            taskbarNotifier1.Show(Title, Content, DelayShow, DelayStay, DelayHide);
        }

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
                    PTPrincipal.GetAllAuthorizationInfo(AppContext, AppContext.CurUserInfo.UserInfo, roleInfo);
                    //await InitConfig();
                    LoadUIMenus();
                    LoadUIForIM_LogPages();
                    this.SystemOperatorState.Text = $"登陆: {AppContext.CurUserInfo.Name}【{AppContext.CurrentRole.RoleName}】";
                }
            }
        }
        private CacheFetchManager _cacheFetchManager = new CacheFetchManager();
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //!MainForm.Instance.AppContext.IsOnline  屏蔽了更新工作台。可能会卡列。需要优化
                if (GetLastInputTime() > 30 && !MainForm.Instance.AppContext.IsOnline)
                {
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

                //超过60 就去抓一下缓存  如果不好用。则用线程定时器
                if (GetLastInputTime() > 5 && MainForm.Instance.AppContext.IsOnline)
                {
                    var tableNames = CacheInfoList.Keys.ToList();
                    string nextTableName = _cacheFetchManager.GetNextTableName(tableNames);
                    TryRequestCache(nextTableName);
                }

            }
            catch (Exception ex)
            {
                
            }
        }



        public void TryRequestCache(string nextTableName, Type elementType = null)
        {
            if (nextTableName != null)
            {
                //对比缓存信息概率。行数变化了也要请求最新的
                bool IsView_ProdDetail = false;
                CacheInfo info = new CacheInfo();
                //先从缓存中取出缓存概览数据中的基本信息。再对比行数。
                if (MainForm.Instance.CacheInfoList.TryGetValue(nextTableName, out info))
                {
                    if (nextTableName.Equals(nameof(View_ProdDetail)))
                    {
                        IsView_ProdDetail = true;
                    }
                }


                // 您的抓取缓存逻辑FetchCacheForTable
                //UIBizSrvice.RequestCache(nextTableName);
                bool needRequestCache = false;
                //Type elementType = null;
                #region
                var cachelist = BizCacheHelper.Manager.CacheEntityList.Get(nextTableName);
                if (cachelist != null)
                {
                    Type listType = cachelist.GetType();
                    if (TypeHelper.IsGenericList(listType))
                    {
                        #region  强类型
                        List<object> oldlist = new List<object>();
                        foreach (object ca in (IEnumerable)cachelist)
                        {
                            oldlist.Add(ca);
                        }

                        //提取产品视图缓存转为强类型
                        if (info != null && IsView_ProdDetail && !list.Count.Equals(oldlist.Count))
                        {
                            list.Clear();
                            foreach (var item in oldlist)
                            {
                                list.Add(item as View_ProdDetail);
                            }
                        }

                        if (oldlist.Count == 0 || oldlist.Count != info.CacheCount)
                        {
                            needRequestCache = true;
                        }

                        #endregion
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        //elementType = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + nextTableName);
                        BizCacheHelper.Manager.NewTableTypeList.TryGetValue(nextTableName, out elementType);

                        List<object> myList = TypeHelper.ConvertJArrayToList(elementType, cachelist as JArray);

                        //提取产品视图缓存转为强类型
                        if (info != null && IsView_ProdDetail && !list.Count.Equals(myList.Count))
                        {
                            list.Clear();
                            foreach (var item in myList)
                            {
                                list.Add(item as View_ProdDetail);
                            }
                            #region  jsonlist
                            if (myList.Count == 0 || myList.Count != info.CacheCount)
                            {
                                needRequestCache = true;
                            }
                            #endregion
                        }
                        else
                        {
                            needRequestCache = true;
                        }

                    }
                }
                else
                {
                    //请求发送缓存
                    needRequestCache = true;
                }
                #endregion
                if (needRequestCache)
                {
                    UIBizSrvice.RequestCache(nextTableName, elementType);
                    _cacheFetchManager.UpdateLastCacheFetchInfo(nextTableName);
                    if (authorizeController.GetShowDebugInfoAuthorization())
                    {
                        if (elementType == null)
                        {
                            PrintInfoLog($"请求了缓存：{nextTableName}");
                        }
                        else
                        {
                            PrintInfoLog($"请求了缓存：{nextTableName}-{elementType.Name}");
                        }
                    }
                }
            }
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

        private void btntsbRefresh_Click(object sender, EventArgs e)
        {
            LoginWebServer();
            if (MainForm.Instance.AppContext.IsSuperUser)
            {
                //请为空时。是请求全部
                ClientService.请求缓存(string.Empty);
                SystemOptimizerService.异常信息发送("测试异常信息发送");
            }
        }

        public async void LoginWebServer()
        {
            try
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
                var webServerUrl = configManager.GetValue("WebServerUrl");
                bool islogin = await httpWebService.Login(MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName, MainForm.Instance.AppContext.CurUserInfo.UserInfo.Password, webServerUrl + @"/login");
                if (islogin)
                {
                    MainForm.Instance.uclog.AddLog($"{webServerUrl}登陆成功。");
                    //var ulid = Ulid.NewUlid();
                    //var ulidString = ulid.ToString();
                    //Console.WriteLine($"Generated ULID: {ulidString}");
                }
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
            //如果删除了。服务器上的工作流就可以删除了。
            RequestReminderCommand request = new RequestReminderCommand();
            request.requestType = RequestReminderType.删除提醒;
            ReminderData reminderRequest = new ReminderData();
            reminderRequest.BizPrimaryKey = 11231321;
            reminderRequest.BizType = BizType.CRM跟进计划;
            request.requestInfo = reminderRequest;
            MainForm.Instance.dispatcher.DispatchAsync(request, CancellationToken.None);
        }

        private void tsbtnSysTest_Click(object sender, EventArgs e)
        {
            // 确保密钥长度正确
            // string key = EncryptionHelper.NormalizeKeyLength("黄利华", 32);

            //一次性统计加密码一下密码：
            string enPwd = EncryptionHelper.AesEncryptByHashKey("123456", "张三");

            string pwd = EncryptionHelper.AesDecryptByHashKey(enPwd, "张三");
        }
    }
}
