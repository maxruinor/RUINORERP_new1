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
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.UI.Common;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using RUINORERP.Business;
using HLH.Lib.Security;
using AutoUpdateTools;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI
{
    public partial class FrmLogin : Krypton.Toolkit.KryptonForm
    {
        public FrmLogin(EasyClientService _ecs)
        {
            InitializeComponent();
            ecs = _ecs;
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

            /*
            object obj = null;
            obj = DbHelperSQL.GetSingle("select F9 from tb_information where F20='EN001' ");
            if (obj != null)
            {
                UserSettings.Instance.RegisterCode = obj.ToString();
            }
            else
            {
                ToolForm.frmUserGuide guide = new ToolForm.frmUserGuide();
                if (guide.ShowDialog() != DialogResult.OK)
                {
                    //this.DialogResult = DialogResult.Cancel;
                }
            }
            */



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


            txtUserName.Focus();

        }

        static CancellationTokenSource source = new CancellationTokenSource();
        public EasyClientService ecs { get; set; }

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
                        if (ecs != null)
                        {
                            if (ecs.client != null && ecs.client.IsConnected == true)
                            {
                                if (!ecs.client.Socket.Connected)
                                {
                                    await ecs.Stop();
                                }
                            }
                        }

                        //远程授权 ，如果切换了服务器。前面的链接就要断开重新来。
                        if (ecs != null && ecs.IsConnected)
                        {
                            if (ecs.ServerIp != txtServerIP.Text || ecs.Port.ToString() != txtPort.Text)
                            {
                                //IP都换了。要全部重新连接
                                ecs.LoginStatus = false;
                                Program.AppContextData.IsOnline = false;
                                bool status = await ecs.Stop();
                                if (status)
                                {
                                    MainForm.Instance.ShowMsg("服务器切换中，请稍后...");
                                }
                            }
                        }

                        bool isInitPwd = false;
                        //传入账号密码返回结果
                        bool ok = PTPrincipal.Login(this.txtUserName.Text, this.txtPassWord.Text, Program.AppContextData, ref isInitPwd);
                        if (ok)
                        {
                            if (!Program.AppContextData.IsSuperUser || txtUserName.Text != "admin")
                            {
                                ServerAuthorizer serverAuthorizer = new ServerAuthorizer();

                                // 通过依赖注入获取UserLoginService
                                var userLogin = Startup.ServiceProvider.GetService<UserLoginService>();
                                
                                // 如果依赖注入未能获取到服务，则手动创建（向后兼容）
                                if (userLogin == null)
                                {
                                    userLogin = new UserLoginService(MainForm.Instance.communicationService);
                                }

                                try
                                {                                
                                    // 在执行登录操作前，先连接到服务器
                                    var serverIp = txtServerIP.Text.Trim();
                                    if (!int.TryParse(txtPort.Text.Trim(), out var serverPort))
                                    {
                                        MessageBox.Show("端口号格式不正确，请检查服务器配置。", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //base.Cursor = Cursors.Default;
                                        return;
                                    }
                                    
                                    // 检查是否已经连接，如果没有则建立连接
                                    if (!MainForm.Instance.communicationService.IsConnected)
                                    {
                                        var connected = await MainForm.Instance.communicationService.ConnectAsync(serverIp, serverPort);
                                        if (!connected)
                                        {
                                            MessageBox.Show("无法连接到服务器，请检查网络连接和服务器配置。", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //base.Cursor = Cursors.Default;
                                            return;
                                        }
                                    }
                                    
                                    // 8. 执行登录操作
                                    var loginSuccess = await userLogin.LoginAsync(txtUserName.Text.Trim(), txtPassWord.Text.Trim());
                                    
                                    // 检查登录结果
                                    if (!loginSuccess.Success)
                                    {
                                        MessageBox.Show($"登录失败: {loginSuccess.Message}", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //base.Cursor = Cursors.Default;
                                        return;
                                    }
                                    
                                    // 登录成功，继续执行后续操作
                                    bool result = await serverAuthorizer.loginRunningOperationAsync(ecs, UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, 3);
                                    //UITools.SuperSleep(1000);
                                    if (!result)
                                    {
                                        MessageBox.Show("验证失败或超时请重试");
                                        //MainForm.Instance.logger.LogInformation("验证失败或超时请重试");
                                        //base.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else
                                    {
                                        MainForm.Instance.AppContext.CurrentUser.在线状态 = true;
                                    }

                                    //如果已经登陆 ，则提示要不要T掉原来的。
                                    bool AlreadyLogged = await serverAuthorizer.AlreadyloggedinAsync(ecs, UserGlobalConfig.Instance.UseName, 3);
                                    if (AlreadyLogged)
                                    {
                                        if (MessageBox.Show("该用户已经登陆，是否强制在线用户下线\r\n否则系统即将退出。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                        {
                                            //新的要保留，只传用户名不行。
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                                            //ClientService.请求强制用户下线(UserGlobalConfig.Instance.UseName);
                                        }
                                        else
                                        {
                                            //自己退出
                                            Application.Exit();
                                            return;
                                        }
                                    }
                                    //如果为初始密码则提示弹窗！
                                    IsInitPassword = isInitPwd;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"登录过程中发生异常: {ex.Message}\n\n详细信息: {ex.InnerException?.Message}", "登录异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //base.Cursor = Cursors.Default;
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
                            //BizCodeGenerator.Instance.RedisServerIP = UserGlobalConfig.Instance.ServerIP;
                            Program.AppContextData.IsOnline = true;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }
                        else
                        {
                            Program.AppContextData.IsOnline = false;
                            this.txtUserName.Focus();
                            this.txtUserName.SelectAll();
                            this.errorProvider1.SetError(this.txtUserName, "账号密码有误");
                            //base.Cursor = Cursors.Default;
                            this.Refresh();
                            return;
                        }
                        //  base.Cursor = Cursors.Arrow;
                        //base.Cursor = Cursors.Default;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请检查你的用户名和密码是否正确。" + ex.Message, "登陆出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainForm.Instance.logger.Error("登陆出错", ex);
                    //Application.Exit();
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



        private void btncancel_Click(object sender, EventArgs e)
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
                    //  Console.WriteLine("task1状态：" + task1.Status);
                    //  Console.WriteLine("IsFaulted状态：" + task1.IsFaulted);//由于未处理的异常，任务已完成。
                    //  Console.WriteLine("IsCompleted状态：" + task1.IsCompleted);//获取一个值，该值指示任务是否已完成。
                });
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

        private void txtServerIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}