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
using TransInstruction;
using RUINORERP.UI.Common;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using RUINORERP.Business;

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

            txtUserName.Focus();
        }



        static CancellationTokenSource source = new CancellationTokenSource();
        public EasyClientService ecs { get; set; }
        private async void btnok_Click(object sender, EventArgs e)

        {
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
                        //传入帐号密码返回结果
                        bool ok = PTPrincipal.Login(this.txtUserName.Text, this.txtPassWord.Text, Program.AppContextData);
                        if (ok)
                        {
                            if (!Program.AppContextData.IsSuperUser || txtUserName.Text != "admin")
                            {
                                ServerAuthorizer serverAuthorizer = new ServerAuthorizer();
                                await serverAuthorizer.LongRunningOperationAsync(ecs, UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, 3);
                                // LoginServerByEasyClient(userName, password);
                                UITools.SuperSleep(1000);
                                if (ecs.LoginStatus)
                                {

                                }
                                else
                                {
                                    MessageBox.Show("验证失败或超时请重试");
                                    //MainForm.Instance.logger.LogInformation("验证失败或超时请重试");
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
                            if (Program.AppContextData.ClientInfo.loginTime < System.DateTime.Now.AddYears(-30))
                            {
                                Program.AppContextData.ClientInfo.loginTime = System.DateTime.Now;
                            }

                            UserGlobalConfig.Instance.AutoSavePwd = chksaveIDpwd.Checked;
                            UserGlobalConfig.Instance.IsSupperUser = Program.AppContextData.IsSuperUser;
                            UserGlobalConfig.Instance.AutoRminderUpdate = chkAutoReminderUpdate.Checked;
                            UserGlobalConfig.Instance.Serialize();

                            Program.AppContextData.IsOnline = true;
                            //先指定一下服务器IP
                            //BizCodeGenerator.Instance.RedisServerIP = UserGlobalConfig.Instance.ServerIP;

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }
                        else
                        {
                            Program.AppContextData.IsOnline = false;
                            this.txtUserName.Focus();
                            this.txtUserName.SelectAll();
                            this.errorProvider1.SetError(this.txtUserName, "帐号密码有误");
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






        private void btncancel_Click(object sender, EventArgs e)
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
            txtServerIP.Visible = chkSelectServer.Checked;
        }
    }
}