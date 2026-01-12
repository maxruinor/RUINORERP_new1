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
        /// 欢迎流程完成事件，用于在Load后通知可以显示登录界面
        /// </summary>
        private TaskCompletionSource<bool> _welcomeCompletionTcs = new TaskCompletionSource<bool>();

        /// <summary>
        /// 当前公告内容
        /// </summary>
        private string _currentAnnouncement = null;

        /// <summary>
        /// 用于显示公告的Label控件
        /// </summary>
        private System.Windows.Forms.Label _lblAnnouncement = null;
        private System.Windows.Forms.Panel _panelAnnouncement = null;
        private System.Windows.Forms.Button _btnCloseAnnouncement = null;

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
                _eventManager.AnnouncementReceived += OnAnnouncementReceived;
            }

            // 创建公告显示控件
            CreateAnnouncementControls();
        }

        /// <summary>
        /// 创建公告显示控件
        /// </summary>
        private void CreateAnnouncementControls()
        {
            // 创建公告面板
            _panelAnnouncement = new System.Windows.Forms.Panel
            {
                BackColor = System.Drawing.Color.FromArgb(255, 255, 224),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Visible = false,
                Location = new System.Drawing.Point(86, 50),
                Size = new System.Drawing.Size(250, 80),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            // 创建公告标题标签
            var lblAnnouncementTitle = new System.Windows.Forms.Label
            {
                Text = "📢 系统公告",
                Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(139, 69, 19),
                Location = new System.Drawing.Point(8, 8),
                AutoSize = true
            };

            // 创建公告内容标签
            _lblAnnouncement = new System.Windows.Forms.Label
            {
                Font = new System.Drawing.Font("宋体", 8.5F),
                ForeColor = System.Drawing.Color.FromArgb(60, 60, 60),
                Location = new System.Drawing.Point(8, 30),
                Size = new System.Drawing.Size(234, 30),
                MaximumSize = new System.Drawing.Size(234, 50),
                AutoSize = true
            };

            // 创建关闭公告按钮
            _btnCloseAnnouncement = new System.Windows.Forms.Button
            {
                Text = "×",
                Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.FromArgb(139, 69, 19),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = System.Windows.Forms.Cursors.Hand,
                Size = new System.Drawing.Size(20, 20),
                Location = new System.Drawing.Point(225, 2),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
            };

            _btnCloseAnnouncement.Click += (s, e) =>
            {
                _panelAnnouncement.Visible = false;
            };

            // 添加控件到面板
            _panelAnnouncement.Controls.Add(lblAnnouncementTitle);
            _panelAnnouncement.Controls.Add(_lblAnnouncement);
            _panelAnnouncement.Controls.Add(_btnCloseAnnouncement);

            // 添加到窗体
            this.Controls.Add(_panelAnnouncement);
        }

        /// <summary>
        /// 显示公告信息
        /// </summary>
        /// <param name="content">公告内容</param>
        private void DisplayAnnouncement(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                if (_panelAnnouncement != null)
                {
                    _panelAnnouncement.Visible = false;
                }
                return;
            }

            // 调整登录界面位置，为公告留出空间
            MoveLoginFormDown();

            // 显示公告
            if (_lblAnnouncement != null && _panelAnnouncement != null)
            {
                _lblAnnouncement.Text = content;
                _lblAnnouncement.MaximumSize = new System.Drawing.Size(234, 50);
                _panelAnnouncement.Visible = true;

                MainForm.Instance?.logger?.LogInformation("显示系统公告: {Content}", content);
            }
        }

        /// <summary>
        /// 调整登录表单位置，为公告留出空间
        /// </summary>
        private void MoveLoginFormDown()
        {
            // 调整其他控件位置，向下移动80像素
            int offset = 85;

            foreach (System.Windows.Forms.Control ctrl in this.Controls)
            {
                // 跳过公告相关控件
                if (ctrl == _panelAnnouncement)
                    continue;

                // 只调整特定控件
                if (ctrl.Name == "lblID" || ctrl.Name == "txtUserName")
                {
                    ctrl.Top += offset;
                }
                else if (ctrl.Name == "lblpwd" || ctrl.Name == "txtPassWord")
                {
                    ctrl.Top += offset;
                }
                else if (ctrl.Name == "chksaveIDpwd")
                {
                    ctrl.Top += offset;
                }
                else if (ctrl.Name == "btnok" || ctrl.Name == "btncancel")
                {
                    ctrl.Top += offset;
                }
            }

            // 调整窗体高度
            this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height + offset);
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
                _eventManager.AnnouncementReceived -= OnAnnouncementReceived;
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// 欢迎流程完成事件处理
        /// </summary>
        /// <param name="success">欢迎流程是否成功完成</param>
        private void OnWelcomeCompleted(bool success)
        {
            _welcomeCompleted = success;

            if (!_welcomeCompletionTcs.TrySetResult(success))
            {
                MainForm.Instance?.logger?.LogDebug("欢迎完成事件已触发，但TaskCompletionSource已完成");
            }

            MainForm.Instance?.logger?.LogInformation("收到欢迎流程完成通知: {Status}", success ? "成功" : "失败");
        }

        /// <summary>
        /// 公告接收事件处理
        /// </summary>
        /// <param name="content">公告内容</param>
        private void OnAnnouncementReceived(string content)
        {
            try
            {
                _currentAnnouncement = content;

                MainForm.Instance?.logger?.LogInformation("收到服务器公告: {Content}", content);

                // 在UI线程中显示公告
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => DisplayAnnouncement(content)));
                }
                else
                {
                    DisplayAnnouncement(content);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, "显示公告时发生异常");
            }
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
        /// 完成连接和欢迎验证后显示登录界面
        /// </summary>
        private async void frmLogin_Load(object sender, EventArgs e)
        {
            //Opacity = 0.0;
            //fadeTimer.Start();

            System.Diagnostics.Debug.WriteLine($"UI: {Thread.CurrentThread.ManagedThreadId}");

            // 先加载保存的用户配置
            LoadUserConfig();

            // 初始化原始服务器信息，用于IP地址变更检测
            _originalServerIP = txtServerIP.Text.Trim();
            _originalServerPort = txtPort.Text.Trim();

            // 在后台执行连接和欢迎验证流程
            // 使用Task.Run避免阻塞UI线程
            _ = Task.Run(async () => await InitializeConnectionAndWelcomeFlowAsync());
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
        /// 初始化连接并等待欢迎流程完成
        /// 在后台执行，确保登录界面正常显示
        /// </summary>
        private async Task InitializeConnectionAndWelcomeFlowAsync()
        {
            try
            {
                MainForm.Instance?.logger?.LogInformation("开始初始化连接和欢迎流程...");

                // 验证服务器地址和端口
                if (string.IsNullOrWhiteSpace(txtServerIP.Text) || !int.TryParse(txtPort.Text, out int serverPort) || serverPort <= 0)
                {
                    MainForm.Instance?.logger?.LogWarning("服务器配置不完整，等待用户输入");
                    return;
                }

                // 1. 连接服务器
                bool connectResult = await connectionManager.ConnectAsync(txtServerIP.Text.Trim(), serverPort);
                if (!connectResult)
                {
                    MainForm.Instance?.logger?.LogWarning("无法连接到服务器 {ServerIP}:{ServerPort}", txtServerIP.Text.Trim(), serverPort);
                    // 连接失败，但不阻止登录界面显示，用户可以修改配置后重试
                    return;
                }

                MainForm.Instance?.logger?.LogInformation("服务器连接成功，等待欢迎消息...");

                // 2. 等待欢迎流程完成（等待最多10秒）
                // 欢迎流程由WelcomeCommandHandler自动处理，我们只需要等待确认
                var welcomeTimeout = TimeSpan.FromSeconds(10);
                var welcomeTask = _welcomeCompletionTcs.Task;
                var completedTask = await Task.WhenAny(welcomeTask, Task.Delay(welcomeTimeout));

                if (completedTask == welcomeTask && await welcomeTask)
                {
                    _welcomeCompleted = true;
                    MainForm.Instance?.logger?.LogInformation("欢迎流程验证通过，服务器连接已就绪");
                }
                else
                {
                    MainForm.Instance?.logger?.LogWarning("欢迎流程验证超时，但连接已建立");
                    // 不阻止登录流程，服务器端有超时保护机制
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, "初始化连接和欢迎流程时发生异常");
            }
        }

        static CancellationTokenSource source = new CancellationTokenSource();


        public bool IsInitPassword { get; set; } = false;

        private async void btnok_Click(object sender, EventArgs e)
        {
            if (txtServerIP.Text.Trim().Length == 0 || txtPort.Text.Trim().Length == 0)
            {
                // 确保在UI线程中显示消息框
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("请输入服务器IP和端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                    return;
                }
                else
                {
                    MessageBox.Show("请输入服务器IP和端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 验证基本输入
            if (txtUserName.Text.Trim() == "")
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        errorProvider1.SetError(txtUserName, "用户名不能为空");
                        txtUserName.Focus();
                        return;
                    }));
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtUserName, "用户名不能为空");
                    txtUserName.Focus();
                    return;
                }
            }

            // 检查IP地址不变时逻辑，快捷认证登录
            bool isConnected = MainForm.Instance?.communicationService?.IsConnected ?? false;
            string currentToken = await _userLoginService.GetCurrentAccessToken();

            // 检查IP和端口是否发生变化
            bool ipOrPortChanged = false;
            if (isConnected)
            {
                string currentServerIP = connectionManager.CurrentServerAddress ?? "";
                string currentServerPort = connectionManager.CurrentServerPort.ToString();
                string newServerIP = txtServerIP.Text.Trim();
                string newServerPort = txtPort.Text.Trim();

                ipOrPortChanged = !string.Equals(currentServerIP, newServerIP, StringComparison.OrdinalIgnoreCase) ||
                                 !string.Equals(currentServerPort, newServerPort, StringComparison.OrdinalIgnoreCase);

                // 如果IP或端口发生变化，先断开现有连接
                if (ipOrPortChanged)
                {
                    try
                    {
                        MainForm.Instance.PrintInfoLog($"检测到服务器地址变更，从 {currentServerIP}:{currentServerPort} 变更为 {newServerIP}:{newServerPort}，正在取消重连并断开现有连接...");

                        // 使用新的方法取消重连并强制断开连接
                        await MainForm.Instance.communicationService.CancelReconnectAndForceDisconnectAsync();
                        isConnected = false;

                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger?.LogError(ex, "切换服务器时断开连接失败");
                        // 即使断开连接失败，也继续尝试新连接
                    }
                }
            }

            // 如果已连接且有Token，且IP和端口未发生变化，尝试快捷登录验证
            if (isConnected && !string.IsNullOrEmpty(currentToken) && !ipOrPortChanged
                && connectionManager.CurrentServerAddress == txtServerIP.Text && connectionManager.CurrentServerPort.ToString() == txtPort.Text
                && txtUserName.Text == UserGlobalConfig.Instance.UseName
                && txtPassWord.Text == UserGlobalConfig.Instance.PassWord
                )
            {
                try
                {
                    var quickLoginResult = await QuickValidateLoginAsync(currentToken, isConnected);
                    if (quickLoginResult != null && quickLoginResult.IsSuccess)
                    {
                        // 快捷登录验证成功，直接设置在线状态并完成登录
                        MainForm.Instance.AppContext.CurUserInfo.在线状态 = true;

                        // 保存用户配置
                        try
                        {
                            UserGlobalConfig.Instance.AutoSavePwd = chksaveIDpwd.Checked;
                            UserGlobalConfig.Instance.IsSupperUser = Program.AppContextData.IsSuperUser;
                            UserGlobalConfig.Instance.AutoRminderUpdate = chkAutoReminderUpdate.Checked;
                            UserGlobalConfig.Instance.Serialize();
                            MainForm.Instance.logger?.LogDebug("成功保存用户配置");
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger?.LogError(ex, "保存用户配置失败，但不影响登录流程");
                            // 只记录错误，不中断登录流程
                        }

                        Program.AppContextData.IsOnline = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        MainForm.Instance.logger?.LogWarning($"快捷登录验证失败: {quickLoginResult?.ErrorMessage}，将继续使用常规登录流程");
                    }
                }
                catch (Exception ex)
                {
                    // 捕获快捷登录验证过程中的异常
                    MainForm.Instance.logger?.LogError(ex, "执行快捷登录验证时发生异常，将继续使用常规登录流程");
                    // 不向用户显示错误，而是静默继续常规登录流程
                }
            }

            // 常规登录流程开始
            try
            {
                using (StatusBusy busy = new StatusBusy("正在登录..."))
                {
                    // 保存用户配置
                    UserGlobalConfig.Instance.UseName = txtUserName.Text;
                    UserGlobalConfig.Instance.PassWord = txtPassWord.Text;
                    UserGlobalConfig.Instance.ServerIP = txtServerIP.Text;
                    UserGlobalConfig.Instance.ServerPort = txtPort.Text;

                    // 验证服务器端口
                    if (!int.TryParse(txtPort.Text.Trim(), out var serverPort))
                    {
                        // 确保在UI线程中显示消息框
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                MessageBox.Show("端口号格式不正确，请检查服务器配置。", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                            return;
                        }
                        else
                        {
                            MessageBox.Show("端口号格式不正确，请检查服务器配置。", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // 执行本地权限验证
                    bool isInitPwd = false;
                    var localAuthSuccess = PTPrincipal.Login(txtUserName.Text, txtPassWord.Text, Program.AppContextData, ref isInitPwd);

                    if (!localAuthSuccess)
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                errorProvider1.SetError(txtUserName, "账号密码有误");
                                txtUserName.Focus();
                                txtUserName.SelectAll();
                                return;
                            }));
                            return;
                        }
                        else
                        {
                            errorProvider1.SetError(txtUserName, "账号密码有误");
                            txtUserName.Focus();
                            txtUserName.SelectAll();
                            return;
                        }
                    }

                    // 如果是超级管理员且为admin用户，直接完成登录
                    if (Program.AppContextData.IsSuperUser && txtUserName.Text == "admin")
                    {
                        await CompleteAdminLogin(isInitPwd);
                        return;
                    }

                    // 使用新的登录流程服务处理登录
                    await ExecuteNewLoginFlow(isInitPwd, serverPort);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "登录过程中发生异常");

                // 异常情况下，断开连接
                if (MainForm.Instance?.communicationService?.IsConnected == true)
                {
                    await MainForm.Instance.communicationService.Disconnect();
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                }

                var errorMessage = ex is TimeoutException || ex.Message.Contains("超时") || ex.Message.Contains("Timeout")
                    ? "网络连接超时，请检查服务器地址是否正确或网络连接是否正常。"
                    : $"登录失败: {ex.Message}";

                // 确保在UI线程中显示消息框
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(errorMessage, "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show(errorMessage, "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        private string IPToIPv4(string strIP, int Port)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(strIP), Port);

            // 检查地址是否是 IPv4 映射到 IPv6 的地址
            if (endpoint.Address.IsIPv4MappedToIPv6)
            {
                // 转换为 IPv4 地址
                IPAddress ipv4Address = endpoint.Address.MapToIPv4();
                string ipv4PortString = $"{ipv4Address}:{endpoint.Port}";
                //System.Diagnostics.Debug.WriteLine(ipv4PortString); // 输出：192.168.0.99:57276
                return ipv4PortString;
            }
            else
            {
                // 地址已经是 IPv4 地址
                string ipv4PortString = $"{endpoint.Address}:{endpoint.Port}";
                return ipv4PortString;
                //System.Diagnostics.Debug.WriteLine(ipv4PortString);
            }
        }



        private async void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建局部的取消令牌源，避免静态实例导致的问题
                using (var localSource = new CancellationTokenSource())
                {
                    // 立即取消
                    localSource.Cancel();
                }

                // 取消登录时，如果已连接则断开连接
                if (MainForm.Instance?.communicationService?.IsConnected == true)
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
        /// 当用户修改服务器IP地址时，检测是否发生变更并设置标志位
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

            // 如果IP或端口已发生变更，且连接状态有效，则触发重新连接和欢迎流程
            if (ipChanged && connectionManager.IsConnected)
            {
                MainForm.Instance?.logger?.LogInformation($"检测到服务器地址变更，准备重新连接: {_originalServerIP}:{_originalServerPort} -> {currentIP}:{currentPort}");

                // 使用防抖机制，避免频繁触发
                await DebouncedReconnectAsync();
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
        /// 防抖动的重新连接方法
        /// 避免用户在输入过程中频繁触发重连
        /// </summary>
        private int _reconnectDebounceTimer = 0;
        private const int DebounceDelayMs = 1500; // 1.5秒防抖

        private async Task DebouncedReconnectAsync()
        {
            int timerId = System.Threading.Interlocked.Increment(ref _reconnectDebounceTimer);

            await Task.Delay(DebounceDelayMs);

            // 检查是否是最新的调用
            if (timerId == _reconnectDebounceTimer)
            {
                await ReconnectAndWelcomeAsync();
            }
        }

        /// <summary>
        /// 重新连接并执行欢迎流程
        /// </summary>
        private async Task ReconnectAndWelcomeAsync()
        {
            try
            {
                MainForm.Instance?.logger?.LogInformation("开始重新连接并执行欢迎流程...");

                // 验证服务器配置
                if (string.IsNullOrWhiteSpace(txtServerIP.Text) || !int.TryParse(txtPort.Text, out int serverPort))
                {
                    MainForm.Instance?.logger?.LogWarning("服务器配置无效，跳过重新连接");
                    return;
                }

                // 断开现有连接
                if (connectionManager.IsConnected)
                {
                    await connectionManager.DisconnectAsync();
                    await Task.Delay(500); // 等待断开完成
                }

                // 更新原始服务器信息
                _originalServerIP = txtServerIP.Text.Trim();
                _originalServerPort = txtPort.Text.Trim();

                // 清除当前公告显示
                if (_panelAnnouncement != null)
                {
                    _panelAnnouncement.Visible = false;
                }

                // 重置欢迎流程状态
                _welcomeCompletionTcs = new TaskCompletionSource<bool>();
                _welcomeCompleted = false;

                // 执行连接和欢迎流程
                await InitializeConnectionAndWelcomeFlowAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, "重新连接和欢迎流程时发生异常");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 快速验证登录（基于现有Token）
        /// </summary>
        private async Task<LoginResponse> QuickValidateLoginAsync(string token, bool isConnected)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || !isConnected)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("无效的连接状态或Token");
                }

                // 验证Token有效性
                var tokenValid = await _userLoginService.ValidateTokenAsync(token);
                if (!tokenValid)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("Token已失效");
                }

                // Token有效，返回成功响应
                return new LoginResponse
                {
                    IsSuccess = true,
                    Message = "快捷登录验证成功",
                    Username = UserGlobalConfig.Instance.UseName
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "快捷登录验证时发生异常");
                return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>($"快捷登录验证失败: {ex.Message}");
            }
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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "完成超级管理员登录时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 执行新的登录流程
        /// </summary>
        private async Task ExecuteNewLoginFlow(bool isInitPwd, int serverPort)
        {
            try
            {
                // 设置登录状态
                if (MainForm.Instance != null)
                {
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.LoggingIn;
                }

                // 创建取消令牌，仅用于网络请求阶段的超时控制
                using var networkCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));


                // 1. 连接服务器
                // 无论是否已连接，都尝试连接以确保使用最新的IP和端口
                // 如果已连接但IP/端口已变更，上面的逻辑已经断开了连接
                if (!connectionManager.IsConnected)
                {
                    MainForm.Instance.PrintInfoLog($"正在连接到服务器 {txtServerIP.Text.Trim()}:{serverPort}...");
                    var connected = await connectionManager.ConnectAsync(txtServerIP.Text.Trim(), serverPort);
                    if (!connected)
                    {
                        throw new Exception($"无法连接到服务器 {txtServerIP.Text.Trim()}:{serverPort}");
                    }
                    MainForm.Instance.PrintInfoLog("服务器连接成功");
                }
                else
                {
                    MainForm.Instance.logger?.LogDebug("已连接到服务器，跳过连接步骤");
                }

                // 2. 执行登录验证


                var loginResponse = await _userLoginService.LoginAsync(
                    txtUserName.Text,
                    txtPassWord.Text,
                       networkCts.Token);

                // 网络请求阶段完成后，处理登录结果和用户交互（无超时限制）
                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    await HandleLoginSuccess(loginResponse, isInitPwd);
                }
                else
                {
                    var errorMsg = loginResponse?.ErrorMessage ?? "登录失败，请检查用户名和密码";
                    // 确保在UI线程中显示消息框
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show(errorMsg, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    else
                    {
                        MessageBox.Show(errorMsg, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 确保在UI线程中显示消息框
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("登录操作已超时或被取消，请重试。", "登录超时", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                else
                {
                    MessageBox.Show("登录操作已超时或被取消，请重试。", "登录超时", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                // 重置登录状态
                if (MainForm.Instance != null)
                {
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
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
                            return;
                        }
                    }
                }

                // 设置在线状态
                if (MainForm.Instance?.AppContext?.CurUserInfo != null)
                {
                    MainForm.Instance.AppContext.CurUserInfo.在线状态 = true;
                }

                // 请求元数据同步
                await _cacheClientService.RequestAllCacheSyncMetadataAsync();

                //登录成功后要去服务器请求最新配置
                try
                {
                    if (_configSyncService != null)
                    {
                        MainForm.Instance.PrintInfoLog("正在请求最新配置文件...");
                        bool configSyncSuccess = await _configSyncService.RequestCommonConfigsAsync();
                        if (configSyncSuccess)
                        {
                            MainForm.Instance.PrintInfoLog("配置文件请求发送成功，等待服务器响应");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger?.LogError(ex, "请求最新配置文件时发生异常");
                }


                // 获取锁状态列表
                try
                {
                    var lockManagementService = Startup.GetFromFac<ClientLockManagementService>();
                    if (lockManagementService != null)
                    {
                        MainForm.Instance.logger?.LogDebug("正在获取锁状态列表...");
                        var lockResponse = await lockManagementService.GetLockStatusListAsync();
                        if (lockResponse != null && lockResponse.IsSuccess)
                        {
                            //MainForm.Instance.PrintInfoLog("成功获取锁状态列表，锁数量: {LockCount}", 
                            //    lockResponse.LockInfoList?.Count ?? 0);
                        }
                        else
                        {
                            MainForm.Instance.logger?.LogWarning("获取锁状态列表失败: {ErrorMessage}",
                                lockResponse?.Message ?? "未知错误");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger?.LogError(ex, "获取锁状态列表时发生异常");
                    // 不抛出异常，因为这不影响登录成功
                }

                // 保存用户配置
                await SaveUserConfig(isInitPwd);

                // 记录登录时间
                if (Program.AppContextData.CurUserInfo.登录时间 < DateTime.Now.AddYears(-30))
                {
                    Program.AppContextData.CurUserInfo.登录时间 = DateTime.Now;
                }

                // 完成登录
                Program.AppContextData.IsOnline = true;

                // 启动心跳
                MainForm.Instance.communicationService.StartHeartbeat();

                // 在UI线程中完成登录
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }));
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
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
                if (this.InvokeRequired)
                {
                    return (DuplicateLoginAction)this.Invoke(new Func<DuplicateLoginAction>(() =>
                    {
                        // 由于DuplicateLoginDialog现在处理了完整的强制下线逻辑，这里只需要返回用户选择
                        using var dialog = new Forms.DuplicateLoginDialog(_userLoginService, duplicateResult, txtUserName.Text, txtPassWord.Text, txtServerIP.Text, int.Parse(txtPort.Text));
                        var result = dialog.ShowDialog(this);
                        return result == DialogResult.OK ? DuplicateLoginAction.ForceOfflineOthers : DuplicateLoginAction.Cancel;
                    }));
                }
                else
                {
                    // 由于DuplicateLoginDialog现在处理了完整的强制下线逻辑，这里只需要返回用户选择
                    using var dialog = new Forms.DuplicateLoginDialog(_userLoginService, duplicateResult, txtUserName.Text, txtPassWord.Text, txtServerIP.Text, int.Parse(txtPort.Text));
                    var result = dialog.ShowDialog(this);
                    return result == DialogResult.OK ? DuplicateLoginAction.ForceOfflineOthers : DuplicateLoginAction.Cancel;
                }
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
    }
}