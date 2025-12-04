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
        private readonly CacheClientService _cacheClientService; // 缓存客户端服务，用于订阅表
        private readonly ConnectionManager connectionManager;
        private readonly UserLoginService userLoginService;
        private readonly TokenManager _tokenManager;
        private readonly UserLoginService _userLoginService;
        private readonly LoginFlowService _loginFlowService;
        public FrmLogin()
        {
            InitializeComponent();
            _cacheClientService = Startup.GetFromFac<CacheClientService>();
            connectionManager = Startup.GetFromFac<ConnectionManager>();
            _userLoginService = Startup.GetFromFac<UserLoginService>();
            _tokenManager = Startup.GetFromFac<TokenManager>();
            _loginFlowService = Startup.GetFromFac<LoginFlowService>();
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

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //Opacity = 0.0;
            //fadeTimer.Start();


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

            Console.WriteLine($"UI: {Thread.CurrentThread.ManagedThreadId}");

            // 初始化原始服务器信息，用于IP地址变更检测
            _originalServerIP = txtServerIP.Text.Trim();
            _originalServerPort = txtPort.Text.Trim();

            txtUserName.Focus();

        }

        static CancellationTokenSource source = new CancellationTokenSource();


        public bool IsInitPassword { get; set; } = false;

        private async void btnok_Click(object sender, EventArgs e)
        {
            if (txtServerIP.Text.Trim().Length == 0 || txtPort.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入服务器IP和端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 验证基本输入
            if (txtUserName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtUserName, "用户名不能为空");
                txtUserName.Focus();
                return;
            }

            // 检查IP地址不变时逻辑，快捷认证登录
            bool isConnected = MainForm.Instance?.communicationService?.IsConnected ?? false;
            string currentToken = await _userLoginService.GetCurrentAccessToken();

            // 如果已连接且有Token，尝试快捷登录验证
            if (isConnected && !string.IsNullOrEmpty(currentToken)
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
                        MainForm.Instance.AppContext.CurrentUser.在线状态 = true;

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
                        MessageBox.Show("端口号格式不正确，请检查服务器配置。", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 执行本地权限验证
                    bool isInitPwd = false;
                    var localAuthSuccess = PTPrincipal.Login(txtUserName.Text, txtPassWord.Text, Program.AppContextData, ref isInitPwd);

                    if (!localAuthSuccess)
                    {
                        errorProvider1.SetError(txtUserName, "账号密码有误");
                        txtUserName.Focus();
                        txtUserName.SelectAll();
                        return;
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

                MessageBox.Show(errorMessage, "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //Console.WriteLine(ipv4PortString); // 输出：192.168.0.99:57276
                return ipv4PortString;
            }
            else
            {
                // 地址已经是 IPv4 地址
                string ipv4PortString = $"{endpoint.Address}:{endpoint.Port}";
                return ipv4PortString;
                //Console.WriteLine(ipv4PortString);
            }
        }



        private async void btncancel_Click(object sender, EventArgs e)
        {
            try
            {

                //在指定的毫秒数后取消task执行
                source.CancelAfter(0);
                //取消任务后的回调
                source.Token.Register(() =>
                {
                    //不延迟会获取不到正确的状态
                    Thread.Sleep(50);
                });

                // 取消登录时，如果已连接则断开连接
                if (MainForm.Instance?.communicationService?.IsConnected == true)
                {
                    await MainForm.Instance.communicationService.Disconnect();
                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                }

                this.DialogResult = DialogResult.Cancel;
                this.Close();

            }
            catch (Exception)
            {

            }
            finally
            {
                Application.DoEvents();
                Application.Exit();
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
                    //如果登陆按钮正在焦点则执行登陆
                    if (btnok.Focused)
                    {
                        //登陆
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
        private void txtServerIP_TextChanged(object sender, EventArgs e)
        {
            // 检测IP地址是否发生变更，仅设置标志位，不在输入过程中断开连接
            string currentIP = txtServerIP.Text.Trim();
            string currentPort = txtPort.Text.Trim();

            _ipAddressChanged = !string.Equals(currentIP, _originalServerIP, StringComparison.OrdinalIgnoreCase) ||
                               !string.Equals(currentPort, _originalServerPort, StringComparison.OrdinalIgnoreCase);

            // 注意：连接断开逻辑已移至登录按钮点击事件中处理
            // 这样可以避免用户输入过程中的频繁连接/断开操作
        }

        /// <summary>
        /// 服务器端口文本变更事件处理
        /// 端口变更检测逻辑与IP地址变更检测相同，仅设置标志位，不在输入过程中断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            // 端口变更检测逻辑与IP地址变更检测相同，仅设置标志位
            txtServerIP_TextChanged(sender, e);
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

                // 执行网络请求阶段的登录流程（带超时控制）
                var loginResponse = await _loginFlowService.ExecuteLoginFlowAsync(
                    txtUserName.Text,
                    txtPassWord.Text,
                    txtServerIP.Text.Trim(),
                    serverPort,
                    networkCts.Token);

                // 网络请求阶段完成后，处理登录结果和用户交互（无超时限制）
                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    await HandleLoginSuccess(loginResponse, isInitPwd);
                }
                else
                {
                    var errorMsg = loginResponse?.ErrorMessage ?? "登录失败，请检查用户名和密码";
                    MessageBox.Show(errorMsg, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("登录操作已超时或被取消，请重试。", "登录超时", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        
                        // 根据用户选择处理
                        switch (userAction)
                        {
                            case DuplicateLoginAction.ForceOfflineOthers:
                                // 强制其他会话下线
                                var forceResult = await _userLoginService.HandleDuplicateLoginAsync(
                                    loginResponse.SessionId, 
                                    txtUserName.Text, 
                                    DuplicateLoginAction.ForceOfflineOthers);
                                
                                if (!forceResult)
                                {
                                    MessageBox.Show("处理重复登录失败，请重试", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    // 清理本地登录状态
                                    await _userLoginService.CancelLoginAsync(loginResponse.SessionId);
                                    return;
                                }
                                break;
                                
                            case DuplicateLoginAction.Cancel:
                                // 取消登录
                                MessageBox.Show("您已取消登录操作", "登录取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                        }
                    }
                }

                // 设置在线状态
                if (MainForm.Instance?.AppContext?.CurrentUser != null)
                {
                    MainForm.Instance.AppContext.CurrentUser.在线状态 = true;
                }

                // 请求元数据同步
                await _cacheClientService.RequestAllCacheSyncMetadataAsync();

                // 保存用户配置
                await SaveUserConfig(isInitPwd);

                // 记录登录时间
                if (Program.AppContextData.CurrentUser.登陆时间 < DateTime.Now.AddYears(-30))
                {
                    Program.AppContextData.CurrentUser.登陆时间 = DateTime.Now;
                }

                // 完成登录
                Program.AppContextData.IsOnline = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
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
                        using var dialog = new Forms.DuplicateLoginDialog(duplicateResult);
                        var result = dialog.ShowDialog(this);
                        return result == DialogResult.OK ? dialog.SelectedAction : DuplicateLoginAction.Cancel;
                    }));
                }
                else
                {
                    using var dialog = new Forms.DuplicateLoginDialog(duplicateResult);
                    var result = dialog.ShowDialog(this);
                    return result == DialogResult.OK ? dialog.SelectedAction : DuplicateLoginAction.Cancel;
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