using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Business.Security;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

using RUINORERP.UI.Common;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using RUINORERP.Business;
using HLH.Lib.Security;
using AutoUpdateTools;
using RUINORERP.UI.Network.Services;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

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

        public FrmLogin()
        {
            InitializeComponent();

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
            using (StatusBusy busy = new StatusBusy("正在验证凭据..."))
            {
                //PTPrincipal.LoginAsync(this.txtUserName.Text, this.txtPassWord.Text, Program.AppContextData);
                //PTPrincipal.Login(this.txtUserName.Text, this.txtPassWord.Text);
                //MainForm.Instance.AppContext.test = "waton";
                //MainForm.Instance.AppContext.User
                try
                {
                    errorProvider1.Clear();
                    if (this.txtUserName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(this.txtUserName, "用户名不能为空");
                        txtUserName.Focus();
                        return;
                    }
                    else
                    {
                        //base.Cursor = Cursors.WaitCursor;
                        UserGlobalConfig.Instance.UseName = this.txtUserName.Text;
                        UserGlobalConfig.Instance.PassWord = this.txtPassWord.Text;
                        UserGlobalConfig.Instance.ServerIP = txtServerIP.Text;
                        UserGlobalConfig.Instance.ServerPort = txtPort.Text;


                        bool isInitPwd = false;
                        //传入账号密码返回结果


                        // 设置登录状态为登录中
                        if (MainForm.Instance != null)
                        {
                            MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.LoggingIn;
                        }

                        // 如果检测到IP地址变更，先断开原有连接
                        if (_ipAddressChanged)
                        {
                            if (MainForm.Instance != null && MainForm.Instance.communicationService != null &&
                                MainForm.Instance.communicationService.IsConnected)
                            {
                                try
                                {
                                    var disconnectResult = await MainForm.Instance.communicationService.Disconnect();
                                    MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                                    MainForm.Instance.logger?.LogInformation($"IP地址变更，断开连接结果: {disconnectResult} [{_originalServerIP}:{_originalServerPort}]");
                                }
                                catch (Exception ex)
                                {
                                    MainForm.Instance.logger?.LogError(ex, "断开原服务器连接时发生错误");
                                }
                            }
                        }

                        bool ok = PTPrincipal.Login(this.txtUserName.Text, this.txtPassWord.Text, Program.AppContextData, ref isInitPwd);
                        if (ok)
                        {
                            if (!Program.AppContextData.IsSuperUser || txtUserName.Text != "admin")
                            {

                                // 通过依赖注入获取UserLoginService
                                var userLogin = Startup.ServiceProvider.GetService<UserLoginService>();



                                try
                                {
                                    // 在执行登录操作前，先连接到服务器
                                    var serverIp = txtServerIP.Text.Trim();
                                    if (!int.TryParse(txtPort.Text.Trim(), out var serverPort))
                                    {
                                        MessageBox.Show("端口号格式不正确，请检查服务器配置。", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        // 重置登录状态
                                        if (MainForm.Instance != null)
                                        {
                                            MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                                        }
                                        //base.Cursor = Cursors.Default;
                                        return;
                                    }

                                    // IP地址变更检测和连接管理
                                    bool shouldConnect = false;

                                    // 检查IP地址是否发生变更
                                    if (_ipAddressChanged)
                                    {
                                        // IP地址已变更，需要重新建立连接
                                        shouldConnect = true;
                                        MainForm.Instance.logger?.LogInformation($"检测到IP变更，准备连接新服务器 [{serverIp}:{serverPort}]");
                                    }
                                    else if (!MainForm.Instance.communicationService.IsConnected)
                                    {
                                        // IP地址未变更，但未连接，需要建立连接
                                        shouldConnect = true;
                                        MainForm.Instance.logger?.LogInformation($"当前未连接，准备建立连接 [{serverIp}:{serverPort}]");
                                    }
                                    else
                                    {
                                        // 检查当前连接的服务器是否就是目标服务器
                                        var currentAddress = MainForm.Instance.communicationService.GetCurrentServerAddress();
                                        var currentPort = MainForm.Instance.communicationService.GetCurrentServerPort();

                                        if (!string.Equals(currentAddress, serverIp, StringComparison.OrdinalIgnoreCase) ||
                                            currentPort != serverPort)
                                        {
                                            // 虽然_ipAddressChanged为false，但实际连接的服务器与目标服务器不一致
                                            shouldConnect = true;
                                            MainForm.Instance.logger?.LogInformation($"服务器地址不一致，需要重新连接 [目标: {serverIp}:{serverPort}]");
                                        }
                                        else
                                        {
                                            // IP地址未变更且已连接到正确服务器，直接使用现有连接
                                            MainForm.Instance.logger?.LogInformation("使用现有连接");
                                        }
                                    }

                                    // 如果需要建立连接
                                    if (shouldConnect)
                                    {
                                        var disconnectResult = await MainForm.Instance.communicationService.Disconnect();

                                        // 修复后的代码
                                        await Task.Delay(100); // 使用异步等待，避免阻塞UI线程

                                        // 添加连接超时控制，防止无限等待
                                        var connectTask = MainForm.Instance.communicationService.ConnectAsync(serverIp, serverPort);
                                        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10)); // 10秒连接超时
                                        
                                        var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                                        
                                        if (completedTask == timeoutTask)
                                        {
                                            MessageBox.Show("连接服务器超时，请检查网络连接和服务器配置。", "连接超时", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                        
                                        var connected = await connectTask;
                                        if (!connected)
                                        {
                                            // 连接失败，断开连接（如果有部分连接）
                                            if (MainForm.Instance.communicationService.IsConnected)
                                            {
                                                var disconnectResult2 = await MainForm.Instance.communicationService.Disconnect();
                                            }

                                            MessageBox.Show("无法连接到服务器，请检查网络连接和服务器配置。", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

                                        // 连接成功后更新原始IP地址
                                        _originalServerIP = serverIp;
                                        _originalServerPort = txtPort.Text.Trim();
                                        _ipAddressChanged = false;
                                    }

                                 
                                    // 8. 执行登录操作，添加登录超时控制
                                    using var cts = new CancellationTokenSource();
                                    var loginTask = userLogin.LoginAsync(UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, cts.Token);
                                    var loginTimeoutTask = Task.Delay(TimeSpan.FromSeconds(5), cts.Token); // 5秒登录超时
                                    
                                    var completedLoginTask = await Task.WhenAny(loginTask, loginTimeoutTask);
                                    
                                    if (completedLoginTask == loginTimeoutTask)
                                    {
                                        // 超时后取消登录任务
                                        cts.Cancel();
                                        // 不要直接return，而是等待任务完成或处理异常
                                        try
                                        {
                                            // 使用Task.Run包装任务处理，避免阻塞UI
                                            _ = Task.Run(async () => 
                                            {
                                                try
                                                {
                                                    // 尝试等待任务完成，让它有机会释放锁
                                                    await loginTask.ConfigureAwait(false);
                                                }
                                                catch (OperationCanceledException)
                                                {
                                                    // 预期的取消异常，忽略
                                                }
                                                catch (Exception)
                                                {
                                                    // 忽略其他异常
                                                }
                                            });
                                        }
                                        finally
                                        {
                                            MessageBox.Show("登录超时，请检查网络连接或稍后重试。", "登录超时", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        return;
                                    }
                                    
                                    // 确保不会因为取消而抛出异常
                                    var loginSuccess = await loginTask.ConfigureAwait(false);

                                    // 检查登录结果
                                    if (loginSuccess == null || !loginSuccess.IsSuccess)
                                    {
                                        string errorMsg = loginSuccess?.ErrorMessage ?? "登录失败，请检查用户名和密码";
                                        MessageBox.Show(errorMsg, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    else
                                    {
                                        MainForm.Instance.AppContext.CurrentUser.在线状态 = true;
                                    }
                                    
                                    //如果为初始密码则提示弹窗！
                                    IsInitPassword = isInitPwd;
                                }
                                catch (OperationCanceledException)
                                {
                                    // 处理操作被取消的情况，此时锁应该已经在UserLoginService中释放
                                    MessageBox.Show("登录操作已取消，您可以重新尝试登录。", "操作取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    // 异常情况下，断开连接
                                    if (MainForm.Instance != null && MainForm.Instance.communicationService != null && MainForm.Instance.communicationService.IsConnected)
                                    {
                                        var disconnectResult = await MainForm.Instance.communicationService.Disconnect();
                                    }

                                    // 判断是否为超时异常，提供更友好的错误提示
                                    string errorMessage;
                                    if (ex is TimeoutException || ex.Message.Contains("超时") || ex.Message.Contains("Timeout"))
                                    {
                                        errorMessage = "网络连接超时，请检查服务器地址是否正确或网络连接是否正常。";
                                    }
                                    else
                                    {
                                        errorMessage = "请检查你的用户名和密码是否正确。" + ex.Message;
                                    }

                                    MessageBox.Show(errorMessage, "登陆出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    MainForm.Instance.logger.Error("登陆出错", ex);
                                    return;
                                }
                            }
                            else
                            {
                                MainForm.Instance.AppContext.CurrentRole = new Model.tb_RoleInfo();
                                MainForm.Instance.AppContext.CurrentRole.RoleName = "超级管理员";
                                MainForm.Instance.AppContext.Roles = new List<Model.tb_RoleInfo>();
                                MainForm.Instance.AppContext.Roles.Add(MainForm.Instance.AppContext.CurrentRole);
                            }

                            //只保存一次，注销不算 1990-1-1 不算
                            if (Program.AppContextData.CurrentUser.登陆时间 < System.DateTime.Now.AddYears(-30))
                            {
                                Program.AppContextData.CurrentUser.登陆时间 = System.DateTime.Now;
                            }

                            UserGlobalConfig.Instance.AutoSavePwd = chksaveIDpwd.Checked;
                            UserGlobalConfig.Instance.IsSupperUser = Program.AppContextData.IsSuperUser;
                            UserGlobalConfig.Instance.AutoRminderUpdate = chkAutoReminderUpdate.Checked;
                            UserGlobalConfig.Instance.Serialize();
                            //先指定一下服务器IP
                            //BizCodeService.RedisServerIP = UserGlobalConfig.Instance.ServerIP;
                            Program.AppContextData.IsOnline = true;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }
                        else
                        {
                            Program.AppContextData.IsOnline = false;

                            // 登录失败，断开连接
                            if (MainForm.Instance != null && MainForm.Instance.communicationService != null && MainForm.Instance.communicationService.IsConnected)
                            {
                                var disconnectResult = await MainForm.Instance.communicationService.Disconnect();
                                MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                            }

                            this.txtUserName.Focus();
                            this.txtUserName.SelectAll();
                            this.errorProvider1.SetError(this.txtUserName, "账号密码有误");
                            this.Refresh();
                            return;
                        }
                        //  base.Cursor = Cursors.Arrow;
                        //base.Cursor = Cursors.Default;

                    }
                }
                catch (Exception ex)
                {
                    // 异常情况下，断开连接
                    if (MainForm.Instance != null && MainForm.Instance.communicationService != null && MainForm.Instance.communicationService.IsConnected)
                    {
                        var disconnectResult = await MainForm.Instance.communicationService.Disconnect();
                        MainForm.Instance.CurrentLoginStatus = MainForm.LoginStatus.None;
                    }

                    // 判断是否为超时异常，提供更友好的错误提示
                    string errorMessage;
                    if (ex is TimeoutException || ex.Message.Contains("超时") || ex.Message.Contains("Timeout"))
                    {
                        errorMessage = "网络连接超时，请检查服务器地址是否正确或网络连接是否正常。";
                    }
                    else
                    {
                        errorMessage = "请检查你的用户名和密码是否正确。" + ex.Message;
                    }

                    MessageBox.Show(errorMessage, "登陆出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainForm.Instance.logger.Error("登陆出错", ex);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
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
                if (MainForm.Instance != null && MainForm.Instance.communicationService != null && MainForm.Instance.communicationService.IsConnected)
                {
                    var disconnectResult = await MainForm.Instance.communicationService.Disconnect();
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
    }
}