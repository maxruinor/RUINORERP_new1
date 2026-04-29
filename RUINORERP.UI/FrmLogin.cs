using AutoUpdateTools;
using HLH.Lib.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.Security;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

namespace RUINORERP.UI
{
    public partial class FrmLogin : Krypton.Toolkit.KryptonForm
    {
        /// <summary>
        /// 标记IP地址是否发生变更
        /// </summary>
        private bool _ipAddressChanged = false;

        /// <summary>
        /// 保存原始服务器IP地址，用于变更检测
        /// </summary>
        private string _originalServerIP = string.Empty;

        /// <summary>
        /// 保存原始服务器端口，用于变更检测
        /// </summary>
        private string _originalServerPort = string.Empty;
        private readonly CacheClientService _cacheClientService;
        private readonly ConnectionManager connectionManager;
        private readonly UserLoginService _userLoginService;
        private readonly TokenManager _tokenManager;
        private readonly ConfigSyncService _configSyncService;
        private readonly ClientEventManager _eventManager;

        /// <summary>
        /// 标记欢迎流程是否已完成
        /// </summary>
        private bool _welcomeCompleted = false;

        /// <summary>
        /// 欢迎信息，用于存储服务器发送的公告等内容
        /// </summary>
        private string _welcomeAnnouncement = string.Empty;

        /// <summary>
        /// 欢迎流程完成事件，用于在Load后通知可以显示登录界面
        /// 传递参数: (是否成功, 公告内容)
        /// </summary>
        private TaskCompletionSource<(bool success, string announcement)> _welcomeCompletionTcs =
            new TaskCompletionSource<(bool, string)>();


        public FrmLogin()
        {
            InitializeComponent();
            _cacheClientService = Startup.GetFromFac<CacheClientService>();
            connectionManager = Startup.GetFromFac<ConnectionManager>();
            _userLoginService = Startup.GetFromFac<UserLoginService>();
            _tokenManager = Startup.GetFromFac<TokenManager>();
            _configSyncService = Startup.GetFromFac<ConfigSyncService>();
            _eventManager = Startup.GetFromFac<ClientEventManager>();

            // 订阅欢迎流程完成事件
            if (_eventManager != null)
            {
                _eventManager.WelcomeCompleted += OnWelcomeCompleted;
            }
        }

        /// <summary>
        /// 窗体关闭时释放资源
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 取消订阅欢迎流程完成事件
            if (_eventManager != null)
            {
                _eventManager.WelcomeCompleted -= OnWelcomeCompleted;
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// 欢迎流程完成事件处理
        /// </summary>
        /// <param name="success">欢迎流程是否成功完成</param>
        /// <param name="announcement">公告内容</param>
        private void OnWelcomeCompleted(bool success, string announcement)
        {
            _welcomeCompleted = success;
            _welcomeAnnouncement = announcement;

            if (success)
            {
                MainForm.Instance?.ShowStatusText("服务器欢迎消息验证成功");
            }
            else
            {
                MainForm.Instance?.ShowStatusText("服务器欢迎消息验证失败");
            }

            if (!_welcomeCompletionTcs.TrySetResult((success, announcement)))
            {
                MainForm.Instance?.logger?.LogDebug("欢迎完成事件已触发，但TaskCompletionSource已完成");
            }
        }

        /// <summary>
        /// 显示公告信息
        /// </summary>
        /// <param name="content">公告内容</param>
        private void DisplayAnnouncement(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                panelAnnouncement.Visible = false;
                return;
            }

            // 直接显示公告，不调整登录界面位置
            if (lblAnnouncement != null && panelAnnouncement != null)
            {
                lblAnnouncement.Text = content;
                lblAnnouncement.MaximumSize = new System.Drawing.Size(234, 50);
                panelAnnouncement.Visible = true;
                MainForm.Instance?.PrintInfoLog($"显示系统公告: {content}");
            }
        }

        /// <summary>
        /// 关闭公告按钮点击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseAnnouncement_Click(object sender, EventArgs e)
        {
            panelAnnouncement.Visible = false;
        }

        private bool m_showing = true;
        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            //是否為從透明到不透明,也就是顯示出來,m_showing 默認值為true
            if (m_showing)
            {

                double d = 1000.0 / fadeTimer.Interval / 100.0;
                if (Opacity + d >= 1.0)
                {
                    //透明度已經達到.0
                    Opacity = 1.0;
                    //停止定時器
                    fadeTimer.Stop();
                }
                else
                {
                    //繼續增加透明度
                    Opacity += d;
                }
            }
            else
            {
                //從不透明到透明,也就是對話框消失
                double d = 1000.0 / fadeTimer.Interval / 100.0;
                if (Opacity - d <= 0.0)
                {
                    Opacity = 0.0;
                    fadeTimer.Stop();
                }
                else
                {
                    Opacity -= d;
                }
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// ✅ 修改：登录界面显示时自动尝试连接服务器，获取欢迎信息
        /// 初始连接失败不进行自动重连，只有登录成功后才会启用重连机制
        /// </summary>
        private async void frmLogin_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"UI: {Thread.CurrentThread.ManagedThreadId}");

            // 先加载保存的用户配置
            LoadUserConfig();

            // 初始化原始服务器信息，用于IP地址变更检测
            _originalServerIP = txtServerIP.Text.Trim();
            _originalServerPort = txtPort.Text.Trim();

            // ✅ 修改：登录界面显示时自动尝试连接服务器
            MainForm.Instance?.ShowStatusText("正在连接服务器...");
            
            // 禁用登录按钮，等待连接完成
            btnok.Enabled = false;
            btnok.Text = "连接中...";
            
            // 启动初始连接（不重连）
            _ = Task.Run(async () => await ConnectAndWaitWelcomeAsync(isInitialConnection: true));
        }

        /// <summary>
        /// 统一的连接并等待欢迎消息方法
        /// ✅ 优化：合并 PerformInitialConnectionAsync 和 InitializeConnectionAndWelcomeFlowAsync 的重复逻辑
        /// </summary>
        /// <param name="isInitialConnection">是否为初始连接（登录界面加载时的自动连接）</param>
        /// <param name="serverIP">服务器IP（可选，默认从UI读取）</param>
        /// <param name="serverPort">服务器端口（可选，默认从UI读取）</param>
        /// <returns>连接结果和公告内容</returns>
        private async Task<(bool connected, string announcement)> ConnectAndWaitWelcomeAsync(
            bool isInitialConnection = false,
            string serverIP = null,
            int? serverPort = null)
        {
            // 从UI读取服务器配置（如果未提供）
            string targetServerIP = serverIP ?? txtServerIP.Text.Trim();
            int targetServerPort = serverPort ?? (int.TryParse(txtPort.Text.Trim(), out int port) ? port : 0);

            // 验证服务器配置
            if (string.IsNullOrWhiteSpace(targetServerIP) || targetServerPort <= 0)
            {
                if (isInitialConnection)
                {
                    InvokeIfRequired(() =>
                    {
                        MainForm.Instance?.ShowStatusText("请输入有效的服务器IP和端口");
                        btnok.Enabled = true;
                        btnok.Text = "登录";
                    });
                }
                MainForm.Instance?.logger?.LogWarning("[连接] 无效的服务器配置 - IP: {IP}, Port: {Port}", targetServerIP, targetServerPort);
                return (false, string.Empty);
            }

            try
            {
                // ✅ 关键：重置欢迎完成信号源，确保可以接收新的欢迎消息
                // 使用新的信号源避免与之前的连接复用导致的状态混乱
                var localTcs = new TaskCompletionSource<(bool success, string announcement)>();
                _welcomeCompletionTcs = localTcs;

                // ✅ 关键：初始连接时禁用自动重连，只有登录成功后才启用
                if (isInitialConnection)
                {
                    connectionManager.AutoReconnect = false;
                    MainForm.Instance?.logger?.LogInformation("[初始连接] 已禁用自动重连，仅在登录成功后启用");
                }

                // 检测是否为外网地址，动态调整超时时间
                bool isExternalNetwork = IsExternalNetworkAddress(targetServerIP);
                // ✅ 优化：双网卡问题解决后，缩短超时时间（外网20秒，内网10秒）
                int connectTimeoutSeconds = isExternalNetwork ? 20 : 10;

                // 更新UI状态提示
                InvokeIfRequired(() =>
                {
                    MainForm.Instance?.ShowStatusText($"正在连接到服务器 {targetServerIP}:{targetServerPort}...");
                });
                MainForm.Instance?.logger?.LogInformation("[连接] 尝试连接到服务器 {ServerIP}:{ServerPort} (外网: {IsExternal})",
                    targetServerIP, targetServerPort, isExternalNetwork);

                // 执行连接
                var connectTask = connectionManager.ConnectAsync(targetServerIP, targetServerPort);
                var completedTask = await Task.WhenAny(connectTask, Task.Delay(TimeSpan.FromSeconds(connectTimeoutSeconds)));

                bool connectResult = false;
                if (completedTask == connectTask)
                {
                    connectResult = await connectTask;
                }
                else
                {
                    MainForm.Instance?.logger?.LogWarning("[连接] 连接超时 - {ServerIP}:{ServerPort}", targetServerIP, targetServerPort);
                }

                if (!connectResult)
                {
                    string errorMsg = isInitialConnection
                        ? $"无法连接到服务器 {targetServerIP}:{targetServerPort}，请检查配置后点击登录重试"
                        : $"无法连接到服务器 {targetServerIP}:{targetServerPort}";
                    
                    if (isInitialConnection)
                    {
                        InvokeIfRequired(() =>
                        {
                            MainForm.Instance?.ShowStatusText(errorMsg);
                            btnok.Enabled = true;
                            btnok.Text = "登录";
                        });
                    }
                    MainForm.Instance?.logger?.LogWarning("[连接] 连接失败 - {ServerIP}:{ServerPort}", targetServerIP, targetServerPort);
                    return (false, string.Empty);
                }

                // 连接成功，等待欢迎消息
                MainForm.Instance?.logger?.LogInformation("[连接] 连接成功，等待欢迎消息");
                InvokeIfRequired(() =>
                {
                    MainForm.Instance?.ShowStatusText("服务器连接成功，等待欢迎信息...");
                });

                // ✅ 优化：缩短欢迎消息等待时间至10秒
                var welcomeTimeout = TimeSpan.FromSeconds(10);
                var welcomeTask = await Task.WhenAny(localTcs.Task, Task.Delay(welcomeTimeout));

                string announcement = string.Empty;
                if (welcomeTask == localTcs.Task)
                {
                    var (success, msg) = await localTcs.Task;
                    if (success && !string.IsNullOrEmpty(msg))
                    {
                        announcement = msg;
                        MainForm.Instance?.logger?.LogInformation("[欢迎消息] 收到公告: {Announcement}", announcement);
                        
                        // 在UI线程中显示公告（仅初始连接时显示）
                        if (isInitialConnection)
                        {
                            InvokeIfRequired(() =>
                            {
                                DisplayAnnouncement(announcement);
                                MainForm.Instance?.ShowStatusText($"服务器连接成功 | 公告: {announcement}");
                            });
                        }
                    }
                    else
                    {
                        MainForm.Instance?.logger?.LogDebug("[欢迎消息] 欢迎流程完成，但未收到公告内容");
                        InvokeIfRequired(() =>
                        {
                            MainForm.Instance?.ShowStatusText("服务器连接成功");
                        });
                    }
                }
                else
                {
                    MainForm.Instance?.logger?.LogWarning("[欢迎消息] 等待超时，未收到欢迎消息");
                    InvokeIfRequired(() =>
                    {
                        MainForm.Instance?.ShowStatusText("服务器连接成功（未收到欢迎消息）");
                    });
                }

                // 初始连接成功时启用登录按钮
                if (isInitialConnection)
                {
                    InvokeIfRequired(() =>
                    {
                        btnok.Enabled = true;
                        btnok.Text = "登录";
                    });
                }

                return (true, announcement);
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, "[连接] 连接过程发生异常 - {ServerIP}:{ServerPort}", targetServerIP, targetServerPort);
                
                if (isInitialConnection)
                {
                    InvokeIfRequired(() =>
                    {
                        MainForm.Instance?.ShowStatusText($"连接服务器时发生错误: {ex.Message}");
                        btnok.Enabled = true;
                        btnok.Text = "登录";
                    });
                }
                return (false, string.Empty);
            }
        }



        /// <summary>
        /// 加载保存的用户配置
        /// </summary>
        private void LoadUserConfig()
        {
            //已經讀出保存的用戶設置 ,並將保存的用戶名和密碼顯示在對話框中
            if (UserGlobalConfig.Instance.AutoSavePwd == true)
            {
                this.txtUserName.Text = UserGlobalConfig.Instance.UseName;
                this.txtPassWord.Text = UserGlobalConfig.Instance.PassWord;
                txtServerIP.Text = UserGlobalConfig.Instance.ServerIP;
                txtPort.Text = UserGlobalConfig.Instance.ServerPort;
                chksaveIDpwd.Checked = true;
                if (UserGlobalConfig.Instance.IsSupperUser)
                {
                    chkAutoReminderUpdate.Visible = true;
                    chkAutoReminderUpdate.Checked = UserGlobalConfig.Instance.AutoRminderUpdate;
                }
            }
            else
            {
                txtUserName.Text = "";
                txtPassWord.Text = "";
            }

            if (UserGlobalConfig.Instance.AutoSavePwd)
            {
                chksaveIDpwd.Checked = true;
            }
            else
            {
                chksaveIDpwd.Checked = false;
            }
        }

        /// <summary>
        /// 取消令牌源，用于取消异步操作
        /// </summary>
        private CancellationTokenSource _loginCancellationTokenSource;

        public bool IsInitPassword { get; set; } = false;

        private async void btnok_Click(object sender, EventArgs e)
        {
            // ✅ 修复：手动点击登录时，显式启动后台任务（如自动重连）
            connectionManager.StartBackgroundTasks();

            // 初始化取消令牌源
            _loginCancellationTokenSource = new CancellationTokenSource();

            // ✅ 优化：使用防抖而非完全禁用，允许用户点击“取消”
            btnok.Text = "登录中...";  // ✅ 登录按钮显示“登录中”
            btnok.Enabled = false;  // 防止重复点击
            btncancel.Enabled = true;  // ✅ 保持取消按钮可用

            if (txtServerIP.Text.Trim().Length == 0 || txtPort.Text.Trim().Length == 0)
            {
                MainForm.Instance?.logger?.LogDebug("[登录] 服务器IP或端口为空");
                InvokeIfRequired(() =>
                {
                    MessageBox.Show("请输入服务器IP和端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                    btnok.Enabled = true;
                });
                return;
            }

            // 验证基本输入
            if (txtUserName.Text.Trim() == "")
            {
                MainForm.Instance?.logger?.LogDebug("[登录] 用户名为空");
                InvokeIfRequired(() =>
                {
                    errorProvider1.SetError(txtUserName, "用户名不能为空");
                    txtUserName.Focus();
                    btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                    btnok.Enabled = true;
                });
                return;
            }

            MainForm.Instance?.logger?.LogInformation("[登录] 输入验证通过 - 用户名: {Username}, 服务器: {ServerIP}:{ServerPort}",
                txtUserName.Text, txtServerIP.Text, txtPort.Text);

            // ✅ 优化：使用统一的连接方法，检测是否需要重新连接
            string serverIP = txtServerIP.Text.Trim();
            int.TryParse(txtPort.Text.Trim(), out int serverPort);
            
            // 判断是否需要重连：检查IP/端口是否变化
            bool needsReconnect = true;
            if (connectionManager.IsConnected)
            {
                string currentServerIP = (connectionManager.CurrentServerAddress ?? "").Trim();
                int currentPort = connectionManager.CurrentServerPort;

                // 如果IP和端口都没变，不需要重连
                if (string.Equals(currentServerIP, serverIP, StringComparison.OrdinalIgnoreCase) &&
                    currentPort == serverPort)
                {
                    needsReconnect = false;
                    MainForm.Instance.logger?.LogDebug("服务器地址未变更，使用现有连接");
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"服务器地址已变更: {currentServerIP}:{currentPort} -> {serverIP}:{serverPort}，准备重新连接");
                }
            }

            // 如果需要重连或未连接，使用统一的连接方法
            if (needsReconnect || !connectionManager.IsConnected)
            {
                // 使用统一的连接方法，非初始连接模式（isInitialConnection=false）
                // 登录按钮触发的连接不需要显示公告（公告已在初始连接时显示）
                var (connected, _) = await ConnectAndWaitWelcomeAsync(
                    isInitialConnection: false,
                    serverIP: serverIP,
                    serverPort: serverPort);

                if (!connected)
                {
                    bool isExternalNetwork = IsExternalNetworkAddress(serverIP);
                    InvokeIfRequired(() =>
                    {
                        string errorMessage = isExternalNetwork
                            ? "无法连接到服务器。\n\n可能原因：\n1. 服务器IP或端口配置错误\n2. 服务器防火墙未开放端口\n3. 路由器端口映射未配置\n4. 网络连接不稳定\n\n建议：\n• 检查服务器地址和端口是否正确\n• 确认服务器端防火墙已放行端口\n• 检查路由器端口映射配置\n• 尝试使用内网连接测试服务器状态"
                            : "无法连接到服务器，请检查IP和端口配置。";

                        MessageBox.Show(errorMessage, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnok.Text = "登录";
                        btnok.Enabled = true;
                    });
                    return;
                }
            }

            // 常规登录流程开始
            try
            {
                using (StatusBusy busy = new StatusBusy("正在登录..."))
                {
                    // 执行本地权限验证
                    MainForm.Instance?.logger?.LogDebug("[登录] 正在执行本地权限验证...");
                    var (loginSucceed, isInitPwd) = await PTPrincipal.Login(txtUserName.Text, txtPassWord.Text, Program.AppContextData);

                    if (!loginSucceed)
                    {
                        MainForm.Instance?.logger?.LogDebug("[登录] 本地权限验证失败 - 用户名: {Username}", txtUserName.Text);
                        InvokeIfRequired(() =>
                        {
                            errorProvider1.SetError(txtUserName, "账号密码有误");
                            txtUserName.Focus();
                            txtUserName.SelectAll();
                            btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                            btnok.Enabled = true;
                        });
                        return;
                    }

                    MainForm.Instance?.logger?.LogInformation("[登录] 本地权限验证成功");

                    // 如果是超级管理员且为admin用户，直接完成登录
                    if (Program.AppContextData.IsSuperUser && txtUserName.Text == "admin")
                    {
                        MainForm.Instance?.logger?.LogInformation("[登录] 检测到admin超级管理员，使用简化登录流程");
                        await CompleteAdminLogin(isInitPwd);
                        return;
                    }

                    // 保存用户配置（在登录前保存服务器地址）
                    UserGlobalConfig.Instance.UseName = txtUserName.Text;
                    UserGlobalConfig.Instance.PassWord = txtPassWord.Text;
                    UserGlobalConfig.Instance.ServerIP = txtServerIP.Text;
                    UserGlobalConfig.Instance.ServerPort = txtPort.Text;

                    // 执行新的登录流程（连接逻辑已在前面统一处理）
                    MainForm.Instance?.logger?.LogInformation("[登录] 开始执行网络登录流程...");
                    await ExecuteNewLoginFlow(isInitPwd);
                }
            }
            catch (OperationCanceledException)
            {
                MainForm.Instance?.logger?.LogDebug("[登录取消] 登录操作被用户取消或超时");
                MainForm.Instance.PrintInfoLog("登录操作已取消");
                InvokeIfRequired(() =>
                {
                    btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                    btnok.Enabled = true;
                });
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "[登录异常] 登录过程中发生异常 - ExceptionType: {Type}, Message: {Message}",
                    ex.GetType().Name, ex.Message);

                // 异常情况下，断开连接
                if (MainForm.Instance?.communicationService?.ConnectionManager.IsConnected == true)
                {
                    await MainForm.Instance.communicationService.Disconnect();
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                }

                var errorMessage = ex is TimeoutException || ex.Message.Contains("超时") || ex.Message.Contains("Timeout")
                    ? "网络连接超时，请检查服务器地址是否正确或网络连接是否正常。"
                    : $"登录失败: {ex.Message}";

                MainForm.Instance?.logger?.LogError("[登录错误] {ErrorMessage}", errorMessage);
                InvokeIfRequired(() =>
                {
                    MessageBox.Show(errorMessage, "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                    btnok.Enabled = true; // ✅ 确保按钮可用，便于快速重试
                });
            }
            finally
            {
                // 清理取消令牌源
                if (_loginCancellationTokenSource != null)
                {
                    _loginCancellationTokenSource.Dispose();
                    _loginCancellationTokenSource = null;
                }
                MainForm.Instance?.logger?.LogDebug("[登录] 清理完成");
            }
        }






        private async void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ 优化：如果正在登录，立即取消异步操作并恢复按钮状态
                if (_loginCancellationTokenSource != null && !_loginCancellationTokenSource.IsCancellationRequested)
                {
                    _loginCancellationTokenSource.Cancel();

                    // ✅ 立即更新按钮状态，给用户即时反馈
                    btncancel.Text = "取消中...";  // ✅ 取消按钮显示"取消中"
                    btncancel.Enabled = false;  // 防止重复点击取消

                    MainForm.Instance?.PrintInfoLog("正在取消登录操作...");
                    return;
                }

                // 取消登录时，如果已连接则断开连接
                if (MainForm.Instance?.communicationService?.ConnectionManager.IsConnected == true)
                {
                    // 使用异步断开连接，但设置超时以避免长时间等待
                    var disconnectTask = MainForm.Instance.communicationService.Disconnect();
                    if (await Task.WhenAny(disconnectTask, Task.Delay(2000)) == disconnectTask)
                    {
                        // 断开连接成功完成
                        await disconnectTask;
                        MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                    }
                    else
                    {
                        // 断开连接超时，继续取消流程
                    }
                }

                // 设置对话框结果并关闭
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断流程
                MainForm.Instance?.logger?.LogError(ex, "取消登录时发生错误");

                // ✅ 异常时也恢复按钮状态
                btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                btnok.Enabled = true;
                btncancel.Text = "取消";
                btncancel.Enabled = true;
            }
        }

        private void chksaveIDpwd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                txtPassWord.Focus();
                this.txtPassWord.SelectAll();
            }
        }

        private void txtPassWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chksaveIDpwd.Focus();
            }
        }

        private void chksaveIDpwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnok.Focus();
            }
        }

        /// <summary>
        /// 键盘处理事件
        /// 主要实现的功能是重写键盘命令事件。使用户在使用的时候，如果当前焦点不是在Button(按钮)上的话，就可以用Enter代替Tab键了。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((!(ActiveControl is Button)) && (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Enter))
            {
                if (keyData == Keys.Enter)
                {
                    //如果登录按钮正在焦点则执行登录
                    if (btnok.Focused)
                    {
                        //登录
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                    else
                    {
                        System.Windows.Forms.SendKeys.Send("{TAB}");
                    }

                    return true;
                }
                if (keyData == Keys.Down)
                {
                    System.Windows.Forms.SendKeys.Send("{TAB}");
                }
                else
                    SendKeys.Send("+{Tab}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private void chkSelectServer_CheckedChanged(object sender, EventArgs e)
        {
            ///txtServerIP.Visible = chkSelectServer.Checked;
            gbIPPort.Visible = chkSelectServer.Checked;
        }

        /// <summary>
        /// 服务器IP地址文本变更事件处理
        /// ✅ 关键修改：在登录界面上，IP/端口变更不触发自动重连，仅记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtServerIP_TextChanged(object sender, EventArgs e)
        {
            // 检测IP地址是否发生变更
            string currentIP = txtServerIP.Text.Trim();
            string currentPort = txtPort.Text.Trim();

            bool ipChanged = !string.Equals(currentIP, _originalServerIP, StringComparison.OrdinalIgnoreCase) ||
                              !string.Equals(currentPort, _originalServerPort, StringComparison.OrdinalIgnoreCase);

            _ipAddressChanged = ipChanged;

            // ✅ 关键修改：在登录界面上，IP/端口变更不触发自动重连
            // 用户需要点击“登录”按钮才会建立连接
            if (ipChanged)
            {
                // 仅显示状态提示，不触发重连
                MainForm.Instance?.ShowStatusText("服务器地址已变更，请点击登录按钮重新连接");
                MainForm.Instance.PrintInfoLog($"检测到服务器地址变更: {_originalServerIP}:{_originalServerPort} -> {currentIP}:{currentPort}，请点击登录按钮");
            }
        }

        /// <summary>
        /// 服务器端口文本变更事件处理
        /// 端口变更检测逻辑与IP地址变更检测相同，仅设置标志位，不在输入过程中断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            // 端口变更检测逻辑与IP地址变更检测相同
            txtServerIP_TextChanged(sender, e);
        }

        /// <summary>
        /// ✅ UI线程安全辅助方法 - 确保Action在UI线程执行
        /// </summary>
        /// <param name="action">要在UI线程执行的操作</param>
        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// ✅ UI线程安全辅助方法 - 确保Func在UI线程执行并返回结果
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="func">要在UI线程执行的函数</param>
        /// <returns>函数执行结果</returns>
        private T InvokeIfRequired<T>(Func<T> func)
        {
            if (InvokeRequired)
            {
                return (T)Invoke(func);
            }
            else
            {
                return func();
            }
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 完成超级管理员登录
        /// </summary>
        private async Task CompleteAdminLogin(bool isInitPwd)
        {
            try
            {
                MainForm.Instance.AppContext.CurrentRole = new Model.tb_RoleInfo
                {
                    RoleName = "超级管理员"
                };
                MainForm.Instance.AppContext.Roles = new List<Model.tb_RoleInfo>
                {
                    MainForm.Instance.AppContext.CurrentRole
                };

                // 保存用户配置
                await SaveUserConfig(isInitPwd);

                Program.AppContextData.IsOnline = true;

                // ✅ 使用UI线程安全辅助方法完成登录
                InvokeIfRequired(() =>
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                });
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "完成超级管理员登录时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 执行新的登录流程
        /// ✅ 优化：连接逻辑已在 btnok_Click 中通过 ConnectAndWaitWelcomeAsync 统一处理
        /// </summary>
        /// <param name="isInitPwd">是否为初始密码</param>
        private async Task ExecuteNewLoginFlow(bool isInitPwd)
        {
            MainForm.Instance?.logger?.LogInformation("[登录流程] 开始执行 - IsInitPwd: {IsInitPwd}", isInitPwd);

            try
            {
                // 设置登录状态
                if (MainForm.Instance != null)
                {
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.LoggingIn;
                }

                // 执行登录验证
                MainForm.Instance?.logger?.LogInformation("[登录流程] 正在发送登录请求到服务器...");

                // ✅ 关键优化：为网络登录验证单独设置超时，不受连接时间影响
                // 登录验证需要更长时间（包含重试），使用独立的超时控制
                using var loginTimeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(90)); // ✅ 外网环境下增加至90秒
                using var loginLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    loginTimeoutCts.Token,
                    _loginCancellationTokenSource?.Token ?? CancellationToken.None);

                var loginResponse = await _userLoginService.LoginAsync(
                    txtUserName.Text,
                    txtPassWord.Text,
                    loginLinkedCts.Token);

                MainForm.Instance?.logger?.LogDebug("[登录流程] 收到登录响应 - IsNull: {IsNull}, IsSuccess: {IsSuccess}",
                    loginResponse == null, loginResponse?.IsSuccess);

                // 网络请求阶段完成后，处理登录结果和用户交互（无超时限制）
                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    MainForm.Instance?.logger?.LogInformation("[登录流程] 登录验证成功，准备进入登录后初始化...");

                    // ✅ 关键修复：登录验证成功后，不再受超时限制
                    // 先关闭登录超时令牌，避免后续非关键操作触发超时
                    loginTimeoutCts.Cancel();
                    MainForm.Instance?.logger?.LogDebug("[登录流程] 已取消登录超时令牌，后续操作不受超时限制");

                    await HandleLoginSuccess(loginResponse, isInitPwd);
                }
                else
                {
                    var errorMsg = loginResponse?.ErrorMessage ?? "登录失败，请检查用户名和密码";
                    MainForm.Instance?.logger?.LogDebug("[登录流程] 登录失败 - ErrorMessage: {ErrorMessage}", errorMsg);

                    MessageBox.Show(errorMsg, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                    btnok.Enabled = true; // ✅ 确保按钮可用，便于快速重试
                }
            }
            catch (OperationCanceledException ex)
            {
                MainForm.Instance?.logger?.LogDebug(ex, "[登录流程取消] 登录操作被取消 - IsUserCanceled: {IsUserCanceled}",
                    _loginCancellationTokenSource?.IsCancellationRequested ?? false);

                MessageBox.Show("登录操作已超时或被取消，请重试。", "登录超时", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                btnok.Enabled = true; // ✅ 确保按钮可用，便于快速重试

                // 取消时清理服务器会话，避免积累无效会话
                try
                {
                    MainForm.Instance?.logger?.LogDebug("[登录流程取消] 正在清理服务器会话...");
                    await _userLoginService.CancelLoginAsync(MainForm.Instance?.AppContext?.SessionId);
                    await connectionManager.DisconnectAsync();
                    MainForm.Instance?.logger?.LogDebug("[登录流程取消] 会话清理完成");
                }
                catch (Exception cancelEx)
                {
                    MainForm.Instance?.logger?.LogWarning(cancelEx, "[登录流程取消] 清理资源失败");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, "[登录流程异常] 登录流程发生异常 - ExceptionType: {Type}, Message: {Message}",
                    ex.GetType().Name, ex.Message);
                throw; // 重新抛出，让外层处理
            }
            finally
            {
                // 重置登录状态
                if (MainForm.Instance != null)
                {
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                    MainForm.Instance?.logger?.LogDebug("[登录流程] 登录状态已重置");
                }
            }
        }

        /// <summary>
        /// 处理登录成功
        /// </summary>
        private async Task HandleLoginSuccess(LoginResponse loginResponse, bool isInitPwd)
        {
            try
            {

                // 检查是否存在重复登录情况
                if (loginResponse.HasDuplicateLogin && loginResponse.DuplicateLoginResult != null)
                {
                    MainForm.Instance.logger?.LogWarning($"检测到用户 {txtUserName.Text} 存在重复登录");

                    // 检查是否需要用户确认
                    if (loginResponse.DuplicateLoginResult.RequireUserConfirmation)
                    {
                        // 显示重复登录对话框让用户选择操作
                        var userAction = await ShowDuplicateLoginDialog(loginResponse.DuplicateLoginResult);

                        // 如果用户取消登录，则清理状态并返回
                        if (userAction == DuplicateLoginAction.Cancel)
                        {
                            // 取消登录
                            MainForm.Instance.PrintInfoLog("您已取消登录操作");
                            await _userLoginService.CancelLoginAsync(loginResponse.SessionId);
                            await connectionManager.DisconnectAsync();

                            // ✅ 修复：重置按钮状态，让用户可以重新输入
                            btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                            btnok.Enabled = true;
                            return;
                        }
                        // ✅ 修复：如果用户选择了强制下线，等待服务器处理完成后，提示用户重新登录
                        else if (userAction == DuplicateLoginAction.ForceOfflineOthers)
                        {
                            MainForm.Instance.PrintInfoLog("强制下线操作已完成");

                            // ✅ 关键修复：清理SessionId和Token，避免重新登录时使用旧状态
                            await _userLoginService.CleanupLoginStateAsync();

                            // 断开当前连接
                            await connectionManager.DisconnectAsync();

                            // ✅ 关键修复：不继续登录流程，而是提示用户重新登录
                            MessageBox.Show(
                                "已成功强制对方下线。\n\n请在登录界面重新输入用户名和密码进行登录。",
                                "强制下线成功",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // 重置按钮状态
                            btnok.Text = "登录";  // ✅ 恢复登录按钮文本
                            btnok.Enabled = true;

                            // 不继续执行后续登录逻辑，直接返回
                            return;
                        }
                    }
                }

                // 设置在线状态
                if (MainForm.Instance?.AppContext?.CurUserInfo != null)
                {
                    MainForm.Instance.AppContext.CurUserInfo.IsOnline = true;
                }

                // ✅ 关键优化：将非关键路径的请求改为异步后台执行，不阻塞登录流程
                // 这些操作失败不影响登录成功，可以稍后重试

                // 1. 缓存元数据同步（后台执行）
                _ = Task.Run(async () =>
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    try
                    {
                        MainForm.Instance?.logger?.LogDebug("开始缓存元数据同步...");
                        await _cacheClientService.RequestAllCacheSyncMetadataAsync(cts.Token);
                        MainForm.Instance?.logger?.LogInformation("缓存元数据同步完成");
                    }
                    catch (OperationCanceledException)
                    {
                        MainForm.Instance?.logger?.LogWarning("缓存元数据同步超时或已取消");
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance?.logger?.LogWarning(ex, "缓存元数据同步失败，将在后台重试");
                    }
                });

                // 2. 配置同步（后台执行）
                _ = Task.Run(async () =>
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    try
                    {
                        if (_configSyncService != null)
                        {
                            MainForm.Instance?.logger?.LogDebug("开始配置同步...");
                            bool configSyncSuccess = await _configSyncService.RequestCommonConfigsAsync(
                                forceRefresh: false,
                                ct: cts.Token);

                            if (configSyncSuccess)
                            {
                                MainForm.Instance?.logger?.LogInformation("配置文件同步成功");
                            }
                            else
                            {
                                MainForm.Instance?.logger?.LogWarning("配置文件同步失败，将使用本地缓存");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        MainForm.Instance?.logger?.LogWarning("配置同步超时或已取消");
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance?.logger?.LogError(ex, "配置同步失败");
                    }
                    finally
                    {
                        // ✅ 无论配置同步是否成功，都初始化性能监控
                        InitializePerformanceMonitoring();
                    }
                });

                // 3. 锁状态获取（后台执行）
                _ = Task.Run(async () =>
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                    try
                    {
                        var lockManagementService = Startup.GetFromFac<ClientLockManagementService>();
                        if (lockManagementService != null)
                        {
                            MainForm.Instance?.logger?.LogDebug("开始获取锁状态列表...");
                            var lockResponse = await lockManagementService.GetLockStatusListAsync();

                            if (lockResponse != null && lockResponse.IsSuccess)
                            {
                                int lockCount = lockResponse.LockInfoList?.Count ?? 0;
                                MainForm.Instance?.logger?.LogDebug("成功获取锁状态列表，锁数量: {LockCount}", lockCount);
                            }
                            else
                            {
                                MainForm.Instance?.logger?.LogWarning("获取锁状态列表失败: {ErrorMessage}",
                                    lockResponse?.Message ?? "未知错误");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        MainForm.Instance?.logger?.LogWarning("获取锁状态列表超时");
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance?.logger?.LogError(ex, "获取锁状态列表时发生异常");
                    }
                });

                // 保存用户配置
                await SaveUserConfig(isInitPwd);

                // 🆕 如果用户勾选了“记住密码”，设置自动重新登录凭据
                if (chksaveIDpwd.Checked)
                {
                    try
                    {
                        MainForm.Instance.communicationService.SetAutoReloginCredentials(
                            txtUserName.Text,
                            txtPassWord.Text
                        );
                        MainForm.Instance.logger?.LogDebug("已设置自动重新登录凭据");
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger?.LogWarning(ex, "设置自动重新登录凭据失败");
                    }
                }

                // ✅ 修复：简化登录时间记录逻辑
                // 原逻辑：如果登录时间是30年前的默认值，则更新为当前时间
                // 新逻辑：直接更新为当前时间，确保准确性
                try
                {
                    if (Program.AppContextData.CurUserInfo != null)
                    {
                        Program.AppContextData.CurUserInfo.LoginTime = DateTime.Now;
                        MainForm.Instance.logger?.LogDebug("已记录登录时间: {LoginTime}", DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger?.LogWarning(ex, "记录登录时间失败");
                }

                // 完成登录
                Program.AppContextData.IsOnline = true;

                // 启动心跳（自动重连机制已在UserLoginService中启用）
                MainForm.Instance.communicationService.StartHeartbeat();
                MainForm.Instance.logger?.LogInformation("心跳服务已启动");

                // ✅ 新增：后台检测更新（不阻塞登录流程）
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 延迟2秒，确保MainForm已完全加载
                        await Task.Delay(2000);

                        // 检查是否配置了自动更新
                        if (!Program.AppContextData.SystemGlobalConfig.客户端自动更新)
                        {
                            MainForm.Instance?.logger?.LogDebug("[更新检测] 自动更新功能未启用，跳过检测");
                            return;
                        }

                        // 如果刚刚完成更新，跳过检测
                        if (Program.JustUpdated)
                        {
                            MainForm.Instance?.logger?.LogDebug("[更新检测] 程序刚刚完成更新，跳过本次检测");
                            Program.JustUpdated = false; // 重置标记
                            return;
                        }

                        MainForm.Instance?.logger?.LogInformation("[更新检测] 开始后台检测更新...");

                        // 创建更新检查实例
                        var updateForm = new AutoUpdate.FrmUpdate();

                        // 检查是否有更新
                        bool hasUpdates = updateForm.CheckHasUpdates();

                        if (hasUpdates)
                        {
                            MainForm.Instance?.logger?.LogInformation("[更新检测] 检测到新版本，准备提示用户");

                            // 在UI线程显示提示
                            await Task.Run(() =>
                            {
                                if (MainForm.Instance != null && MainForm.Instance.InvokeRequired)
                                {
                                    MainForm.Instance.Invoke(new Action(() =>
                                    {
                                        var result = MessageBox.Show(
                                            "服务器有新版本可用！\n\n建议立即更新以获得最新功能和修复。\n是否现在更新？（更新前请保存当前工作）",
                                            "发现新版本",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Information);

                                        if (result == DialogResult.Yes)
                                        {
                                            // 启动更新程序
                                            System.Diagnostics.Process.Start(updateForm.currentexeName);

                                            // 关闭当前应用程序
                                            Application.Exit();
                                        }
                                        else
                                        {
                                            MainForm.Instance?.logger?.LogInformation("[更新检测] 用户选择稍后更新");
                                        }
                                    }));
                                }
                            });
                        }
                        else
                        {
                            MainForm.Instance?.logger?.LogDebug("[更新检测] 当前已是最新版本");
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance?.logger?.LogWarning(ex, "[更新检测] 后台检测更新失败");
                    }
                });

                // ✅ 记录登录成功完成
                MainForm.Instance.logger?.LogInformation("登录流程全部完成，准备关闭登录窗口");

                // ✅ 使用UI线程安全辅助方法完成登录
                InvokeIfRequired(() =>
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                });
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "处理登录成功后的操作时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 显示重复登录对话框
        /// </summary>
        private Task<DuplicateLoginAction> ShowDuplicateLoginDialog(DuplicateLoginResult duplicateResult)
        {
            return Task.Run(() =>
            {
                // ✅ 使用UI线程安全辅助方法显示对话框
                return InvokeIfRequired(() =>
                {
                    // 由于DuplicateLoginDialog现在处理了完整的强制下线逻辑，这里只需要返回用户选择
                    using var dialog = new Forms.DuplicateLoginDialog(_userLoginService, duplicateResult, txtUserName.Text, txtPassWord.Text, txtServerIP.Text, int.Parse(txtPort.Text));
                    var result = dialog.ShowDialog(this);
                    return result == DialogResult.OK ? DuplicateLoginAction.ForceOfflineOthers : DuplicateLoginAction.Cancel;
                });
            });
        }

        /// <summary>
        /// 保存用户配置
        /// </summary>
        private async Task SaveUserConfig(bool isInitPwd)
        {
            try
            {
                UserGlobalConfig.Instance.AutoSavePwd = chksaveIDpwd.Checked;
                UserGlobalConfig.Instance.IsSupperUser = Program.AppContextData.IsSuperUser;
                UserGlobalConfig.Instance.AutoRminderUpdate = chkAutoReminderUpdate.Checked;
                UserGlobalConfig.Instance.Serialize();

                MainForm.Instance.logger?.LogDebug("成功保存用户配置");

                // 如果为初始密码则提示（这里可以根据需要添加提示逻辑）
                IsInitPassword = isInitPwd;

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "保存用户配置失败");
                // 不抛出异常，因为这不影响登录成功
            }
        }

        /// <summary>
        /// 初始化性能监控模块
        /// 在登录成功并完成配置同步后调用
        /// </summary>
        private void InitializePerformanceMonitoring()
        {
            try
            {
                var logger = MainForm.Instance?.logger;
                logger?.LogDebug("开始初始化性能监控模块...");

                // 获取性能监控服务（通过DI容器）
                var performanceMonitorService = Startup.GetFromFac<RUINORERP.UI.Network.Services.ClientPerformanceMonitorService>();
                if (performanceMonitorService != null)
                {
                    // 启动性能监控服务（包含Manager的启动）
                    performanceMonitorService.Start();
                }
                else
                {
                    logger?.LogWarning("未找到客户端性能监控服务实例");
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, "初始化性能监控模块失败");
            }
        }

        /// <summary>
        /// 检测是否为外网地址
        /// 根据IP地址或域名判断目标服务器是否位于外网
        /// </summary>
        /// <param name="host">服务器地址（IP或域名）</param>
        /// <returns>是否为外网地址</returns>
        private bool IsExternalNetworkAddress(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return false;

            host = host.Trim();

            // 本地地址
            if (host == "127.0.0.1" ||
                host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                host == "::1")
            {
                return false;
            }

            // 尝试解析为IP地址
            if (IPAddress.TryParse(host, out var ipAddress))
            {
                // 检查是否为私有IP地址（局域网）
                return !IsPrivateIP(ipAddress);
            }

            // 如果是域名，尝试解析
            try
            {
                var hostEntry = Dns.GetHostEntry(host);
                foreach (var ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // 只要有一个IP是外网地址，就认为是外网
                        if (!IsPrivateIP(ip))
                            return true;
                    }
                }
                // 所有解析到的IP都是内网地址
                return false;
            }
            catch
            {
                // 解析失败，保守起见认为是外网（给予更长的超时时间）
                return true;
            }
        }

        /// <summary>
        /// 检查IP地址是否为私有地址（局域网）
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>是否为私有IP</returns>
        private bool IsPrivateIP(IPAddress ip)
        {
            if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                return false;

            var bytes = ip.GetAddressBytes();

            // 10.0.0.0/8
            if (bytes[0] == 10)
                return true;

            // 172.16.0.0/12
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                return true;

            // 192.168.0.0/16
            if (bytes[0] == 192 && bytes[1] == 168)
                return true;

            // 127.0.0.0/8 (回环地址)
            if (bytes[0] == 127)
                return true;

            // 169.254.0.0/16 (链路本地地址)
            if (bytes[0] == 169 && bytes[1] == 254)
                return true;

            return false;
        }
    }
}